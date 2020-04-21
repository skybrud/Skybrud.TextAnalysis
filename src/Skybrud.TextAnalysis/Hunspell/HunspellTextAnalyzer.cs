using System;
using System.Collections.Generic;
using System.Linq;
using Skybrud.TextAnalysis.Hunspell;
using Skybrud.TextAnalysis.Search;

namespace Skybrud.TextAnalysis {

    public class HunspellTextAnalyzer {

        #region Properties

        public NHunspell.Hunspell Hunspell { get; }

        public Affix Affix { get; }

        public MyDictionary Dictionary { get; }

        #endregion

        #region Constructors

        public HunspellTextAnalyzer(NHunspell.Hunspell hunspell, Affix affix, MyDictionary dictionary) {
            Hunspell = hunspell;
            Affix = affix;
            Dictionary = dictionary;
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Returns whether the specified <paramref name="word"/> is spelled correctly. Same as calling the
        /// <see cref="NHunspell.Hunspell.Spell"/> method directly.
        /// </summary>
        /// <param name="word">The word to check.</param>
        /// <returns></returns>
        public bool Spell(string word) {
            return Hunspell.Spell(word);
        }

        /// <summary>
        /// Gets an array of stems for the specified <paramref name="word"/>.
        /// </summary>
        /// <param name="word">The word to get the stem(s) for.</param>
        /// <returns>An array of <see cref="StemResult"/>.</returns>
        /// <remarks>This method is similar to the <see cref="NHunspell.Hunspell.Stem"/> method, but differs in the way
        /// that has better support for working with compound words. For instance in Danish, the stem of
        /// <c>webredaktør</c> is <c>redaktør</c> because <c>webredaktør</c> isn't in the dictionary. And if we try to
        /// morph the stem, we get variations of <c>redaktør</c> instead of <c>webredaktør</c>. When morphing an
        /// instance of <see cref="StemResult"/>, the prefix (if any) is kept in the morphed variations.
        /// </remarks>
        public StemResult[] Stem(string word) {

            List<StemResult> temp = new List<StemResult>();

            foreach (string stem in Hunspell.Stem(word)) {
                int pos = word.IndexOf(stem, StringComparison.InvariantCultureIgnoreCase);
                temp.Add(new StemResult(stem, pos > 0 ? word.Substring(0, pos) : null));
            }

            return temp.ToArray();

        }

        /// <summary>
        /// Returns an array of morphed variations of the specified <paramref name="stem"/> word.
        /// </summary>
        /// <param name="stem">The stem word to morph.</param>
        /// <returns>An array of the morphed variations.</returns>
        public string[] Morph(StemResult stem) {

            List<string> temp = new List<string>();

            if (Dictionary.TryGet(stem.Stem, out List<MyDictionaryItem> list)) {
                foreach (MyDictionaryItem item in list) {
                    foreach (Variant variant in item.Variants) {
                        temp.Add(stem.Prefix + variant.Value);
                    }
                }
            }

            return temp.ToArray();

        }

        public string[] Suggest(string word) {
            return Hunspell.Suggest(word).ToArray();
        }

        public TextExtendResult Extend(string text) {

            // Split the text query into multiple pieces so we can analyze each word separately
            string[] pieces = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            AndList query = new AndList();

            List<List<Word>> temp1 = new List<List<Word>>();

            for (int i = 0; i < pieces.Length; i++) {

                List<Word> temp2 = new List<Word>();
                temp1.Add(temp2);


                string piece = pieces[i];

                temp2.Add(new Word(WordType.Input, piece));

                OrList or = new OrList { Name = "O0" };
                query.Query.Add(or);

                if (IsAlpha(piece) == false) {
                    or.Append(piece);
                    continue;
                }

                if (Hunspell.Spell(piece)) {

                    if (i < pieces.Length - 1) {

                        string x = piece;
                        string y = pieces[i + 1];
                        string z = x + y;

                        bool ignore = false;

                        switch (x) {
                            case "med":
                            case "mens":
                                ignore = true;
                                break;
                        }

                        switch (y) {
                            case "med":
                            case "mens":
                                ignore = true;
                                break;
                        }

                        if (ignore == false && Hunspell.Spell(z)) {

                            AndList a2 = new AndList { Name = "A2" };
                            
                            if (Hunspell.Spell(x)) {
                                OrList or1 = new OrList { Name = "O1" };
                                foreach (var stem in Hunspell.Stem(x)) {
                                    if (Dictionary.TryGet(stem, out var list)) {
                                        foreach (MyDictionaryItem item in list) {
                                            foreach (Variant variant in item.Variants) {
                                                or1.Append(variant.Value);
                                            }
                                        }
                                    }
                                    else {
                                        or1.Append(stem);
                                    }
                                }
                                a2.Query.Add(or1);
                            } else {
                                a2.Append(x);
                            }

                            if (Hunspell.Spell(y)) {
                                OrList or2 = new OrList { Name = "O2" };
                                foreach (var stem in Hunspell.Stem(y)) {
                                    if (Dictionary.TryGet(stem, out var list)) {
                                        foreach (MyDictionaryItem item in list) {
                                            foreach (Variant variant in item.Variants) {
                                                or2.Append(variant.Value);
                                            }
                                        }
                                    }
                                    else {
                                        or2.Append(stem);
                                    }
                                }
                                a2.Query.Add(or2);
                            } else {
                                a2.Append(y);
                            }

                            or.Query.Add(a2);

                            foreach (string stem in Hunspell.Stem(z)) {
                                temp2.Add(Word.Suggestion(stem, z));
                                OrList or3 = new OrList { Name = "O3" };
                                if (Dictionary.TryGet(stem, out var items)) {
                                    foreach (MyDictionaryItem item in items) {
                                        foreach (Variant variant in item.Variants) {
                                            or3.Append(variant.Value);
                                        }
                                    }
                                }
                                if (or3.Count > 0) or.Query.Add(or3);
                            }

                            i++;

                            continue;

                        }

                    }
                    
                    OrList or4 = new OrList { Name = "O4" };

                    foreach (string stem in Hunspell.Stem(piece)) {

                        // Append the stem if it isn't equal to the input
                        if (piece != stem) temp2.Add(Word.Stem(stem, piece));

                        // Lookup the stem in the custom dictionary
                        if (Dictionary.TryGet(stem, out var items)) {
                            foreach (MyDictionaryItem item in items) {
                                foreach (Variant variant in item.Variants) {
                                    or4.Append(variant.Value);
                                }
                            }
                        }

                        // Fallback: append the stem if it wasn't found in the custom dictionary
                        or4.Append(stem);

                    }

                    // Append the OR list to the parent list if not empty
                    if (or4.Count > 0) {
                        or4.Query = or4.Query.Distinct().ToList();
                        or.Query.Add(or4);
                    }

                } else {

                    temp2[0].IsMisspelled = true;

                    if (i < pieces.Length - 1) {

                        string x = piece;
                        string y = pieces[i + 1];
                        string z = x + y;

                        if (Hunspell.Spell(z)) {

                            AndList and = new AndList();
                            or.Query.Add(and);

                            and.Append(x);

                            if (Hunspell.Spell(y)) {
                                OrList or2 = new OrList();
                                and.Query.Add(or2);
                                foreach (string stem in Hunspell.Stem(y)) {
                                    if (Dictionary.TryGet(stem, out var list)) {
                                        foreach (MyDictionaryItem item in list) {
                                            foreach (Variant variant in item.Variants) {
                                                or2.Append(variant.Value);
                                            }
                                        }
                                    }
                                    else {
                                        or2.Append(stem);
                                    }
                                }
                            } else {
                                and.Append(y);
                            }

                            OrList or5 = new OrList();
                            foreach (string stem in Hunspell.Stem(z)) {
                                if (z != stem) temp2.Add(Word.Stem(stem, z));
                                if (Dictionary.TryGet(stem, out var items)) {
                                    foreach (MyDictionaryItem item in items) {
                                        foreach (Variant variant in item.Variants) {
                                            or5.Append(variant.Value);
                                        }
                                    }
                                }
                            }

                            if (or5.Query.Any()) {
                                or5.Query = or5.Query.Distinct().ToList();
                                or.Query.Add(or5);
                            }

                            i++;

                            continue;

                        }

                    }

                    or.Append(piece);

                    foreach (string suggestion in Hunspell.Suggest(piece)) {
                        temp2.Add(new Word(WordType.Suggestion, suggestion));
                        or.Append(suggestion);

                        foreach (string stem in Hunspell.Stem(suggestion)) {
                            if (Dictionary.TryGet(stem, out var list)) {
                                foreach (MyDictionaryItem item in list) {
                                    foreach (Variant variant in item.Variants) {
                                        or.Append(variant.Value);
                                    }
                                }
                            } else {
                                or.Append(stem);
                            }
                        }

                    }

                    //if (i < pieces.Length - 1) {
                    //    foreach (var suggestion in hunspell.Suggest(piece + " " + pieces[i + 1])) {
                    //        temp2.Add(new Word(WordType.Suggestion, suggestion));
                    //    }
                    //}

                }

            }

            return new TextExtendResult(temp1.Select(x => x.ToArray()).ToArray(), query);

        }

        /// <summary>
        /// Computes the Levenshtein distance between two strings.
        /// </summary>
        /// <see cref="http://www.dotnetperls.com/levenshtein" />
        public int Levenshtein(string s, string t) {
            
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0) {
                return m;
            }

            if (m == 0) {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++) {
            }

            for (int j = 0; j <= m; d[0, j] = j++) {
            }

            // Step 3
            for (int i = 1; i <= n; i++) {
                //Step 4
                for (int j = 1; j <= m; j++) {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];

        }

        private bool IsAlpha(string value) {
            return value?.All(char.IsLetter) ?? false;
        }

        #endregion

    }

}