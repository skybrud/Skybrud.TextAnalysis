using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Skybrud.Essentials.Strings;
using Skybrud.TextAnalysis.Hunspell.Affix;
using Skybrud.TextAnalysis.Hunspell.Dictionary;
using Skybrud.TextAnalysis.Hunspell.Expand;
using Skybrud.TextAnalysis.Hunspell.Expand.Skybrud.TextAnalysis.Hunspell.Expand;
using Skybrud.TextAnalysis.Hunspell.Stem;
using Skybrud.TextAnalysis.Search;
using WeCantSpell.Hunspell;

namespace Skybrud.TextAnalysis.Hunspell {
    
    /// <summary>
    /// Class representing a Hunspell text analyzer that wraps the dictionary and affix file functionalities.
    /// </summary>
    public class HunspellTextAnalyzer {

        #region Properties
        
        /// <summary>
        /// Gets a reference to the underlying Hunspell word list.
        /// </summary>
        public WordList WordList { get; }

        /// <summary>
        /// Gets a reference to the underlying Hunspell affix file.
        /// </summary>
        public HunspellAffix Affix { get; }
        
        /// <summary>
        /// Gets a reference to the underlying Hunspell dictionary file.
        /// </summary>
        public HunspellDictionary Dictionary { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance based on the specified <paramref name="wordList"/>, <paramref name="affix"/> file and <paramref name="dictionary"/> file.
        /// </summary>
        /// <param name="wordList">A reference to the word list.</param>
        /// <param name="affix">A reference to the affix file.</param>
        /// <param name="dictionary">A reference to the dictionary file.</param>
        public HunspellTextAnalyzer(WordList wordList, HunspellAffix affix, HunspellDictionary dictionary) {
            WordList = wordList;
            Affix = affix;
            Dictionary = dictionary;
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Returns whether the specified <paramref name="word"/> is spelled correctly. Same as calling the <see cref="WeCantSpell.Hunspell.WordList.Check"/> method directly.
        /// </summary>
        /// <param name="word">The word to check.</param>
        /// <returns><c>true</c> of <paramref name="word"/> is spelled correctly; otherwise <c>false</c>.</returns>
        public bool Spell(string word) {
            return WordList.Check(word);
        }
        
        /// <summary>
        /// Returns whether the specified <paramref name="word"/> is spelled correctly. Same as calling the <see cref="WeCantSpell.Hunspell.WordList.Check"/> method directly.
        /// </summary>
        /// <param name="word">The word to check.</param>
        /// <param name="result">When this method returns, the value will contain information about the word.</param>
        /// <returns><c>true</c> of <paramref name="word"/> is spelled correctly; otherwise <c>false</c>.</returns>
        public bool Spell(string word, out SpellCheckResult result) {
            result = WordList.CheckDetails(word);
            return result.Correct;
        }

        /// <summary>
        /// Returns an array with the stems of the specified <paramref name="word"/>.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns>An array of <see cref="HunspellStemResult"/>.</returns>
        /// <remarks>The <c>NHunspell</c> package we used in previous versions had support for multiple stems for a
        /// given word (as the same word may have different meanings). The <c>WeCantSpell.Hunspell</c> doesn't support
        /// this, so the array will always have a length of either <c>0</c> or <c>1</c>.</remarks>
        public HunspellStemResult[] Stem(string word) {

            SpellCheckResult details = WordList.CheckDetails(word);

            if (!details.Correct) return new HunspellStemResult[0];

            if (string.IsNullOrEmpty(details.Root)) {
                return new[] {
                    new HunspellStemResult(word, null)
                };
            }

            List<HunspellStemResult> temp = new List<HunspellStemResult>();

            if (details.Info.HasFlag(SpellCheckResultType.Compound)) {
                
                if (string.IsNullOrEmpty(details.Root)) throw new Exception("NAH");
                if (word.IndexOf(details.Root, StringComparison.OrdinalIgnoreCase) > 0)  throw new Exception("NAH 2");

                string prefix = details.Root;

                word = word.Substring(prefix.Length);

                details = WordList.CheckDetails(word);

                temp.Add(new HunspellStemResult(string.IsNullOrWhiteSpace(details.Root) ? word : details.Root, prefix));

            } else {

                temp.Add(new HunspellStemResult(details.Root));

            }

            return temp.ToArray();

        }

        /// <summary>
        /// Returns an array of morphed variations of the specified <paramref name="stem"/> word.
        /// </summary>
        /// <param name="stem">The stem word to morph.</param>
        /// <returns>An array of the morphed variations.</returns>
        public string[] Morph(HunspellStemResult stem) {

            List<string> temp = new List<string>();

            // Look up the stem in the dictionary to get the variations
            if (Dictionary.TryGet(stem.Stem, out List<HunspellDictionaryItem> list)) {
                foreach (HunspellDictionaryItem item in list) {
                    foreach (Variant variant in item.Variants) {
                        temp.Add(stem.Prefix + variant.Value);
                    }
                }
            }

            // Append the stem and prefix - at least as a fallback
            temp.Add(stem.Value);

            // Return the variations as an array
            return temp.Distinct().ToArray();

        }

        /// <summary>
        /// Returns an array of suggestions based on the specified <paramref name="word"/>.
        /// </summary>
        /// <param name="word">The (misspelled) word.</param>
        /// <returns>An array of suggestions.</returns>
        public string[] Suggest(string word) {
            
            // The underlying Hunspell package may throw an OutOfBounds exception for all capital words (eg.
            // abbreviations), so since the package won't be able to provide any suggestions for "word", we might as
            // well catch the exception and return an empty array (aka no suggestions)
            try {
                return WordList.Suggest(word).ToArray();
            } catch {
                return new string[0];
            }

        }

        /// <summary>
        /// Expands the specified <paramref name="text"/> with known variations based the dictionary and affix file.
        /// </summary>
        /// <param name="text">The text to extend.</param>
        /// <returns>An instance of <see cref="HunspellExpandResult"/> with the result of the operation.</returns>
        public virtual HunspellExpandResult Expand(string text) {
            return Expand(new HunspellExpandOptions(text));
        }

        /// <summary>
        /// Expands the value of the <see cref="HunspellExpandOptions.Text"/> property of the specified <paramref name="options"/>.
        /// </summary>
        /// <param name="options">The options for expanding <see cref="HunspellExpandOptions.Text"/>.</param>
        /// <returns>A instance containing the extended result.</returns>
        public virtual HunspellExpandResult Expand(HunspellExpandOptions options) {

            if (options == null) throw new ArgumentNullException(nameof(options));
            if (string.IsNullOrWhiteSpace(options.Text)) throw new ArgumentNullException(nameof(options.Text));

            // Split the text query into multiple pieces so we can analyze each word separately
            string[] pieces = options.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            AndList query = new AndList();

            List<List<HunspellExpandWord>> temp1 = new List<List<HunspellExpandWord>>();

            for (int i = 0; i < pieces.Length; i++) {

                List<HunspellExpandWord> temp2 = new List<HunspellExpandWord>();
                temp1.Add(temp2);

                string piece = pieces[i];

                temp2.Add(new HunspellExpandWord(HunspellExpandWordType.Input, piece));

                OrList or = new OrList { Name = "O0" };
                query.Query.Add(or);

                if (StringUtils.IsAlphabetic(piece) == false) {
                    or.Append(piece);
                    continue;
                }

                if (Spell(piece)) {

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

                        if (ignore == false && Spell(z)) {

                            AndList a2 = new AndList { Name = "A2" };
                            
                            if (Spell(x)) {
                                
                                OrList or1 = new OrList { Name = "O1" };
                                
                                // Iterate over the stem(s) of the specified word
                                foreach (HunspellStemResult stem in Stem(x)) {

                                    // Append each variant/morph to the list
                                    foreach (string variant in Morph(stem)) {
                                        or1.Append(variant);
                                    }

                                }
                                
                                a2.Query.Add(or1);

                            } else {
                                a2.Append(x);
                            }

                            if (Spell(y)) {
                                
                                OrList or2 = new OrList { Name = "O2" };

                                // Iterate over the stem(s) of the specified word
                                foreach (HunspellStemResult stem in Stem(y)) {
                                    
                                    // Append each variant/morph to the list
                                    foreach (string variant in Morph(stem)) {
                                        or2.Append(variant);
                                    }

                                }
                                
                                a2.Query.Add(or2);

                            } else {
                                a2.Append(y);
                            }

                            or.Query.Add(a2);

                            // Iterate over the stem(s) of "z"
                            foreach (HunspellStemResult stem in Stem(z)) {
                                
                                temp2.Add(HunspellExpandWord.Suggestion(stem.Prefix + stem.Stem, z));
                                
                                OrList or3 = new OrList { Name = "O3" };

                                // Append each variant/morph to the list
                                foreach (string variant in Morph(stem)) {
                                    or3.Append(variant);
                                }

                                if (or3.Count > 0) or.Query.Add(or3);

                            }

                            i++;

                            continue;

                        }

                    }
                    
                    OrList or4 = new OrList { Name = "O4" };

                    // Append the input word
                    or4.Append(piece);

                    // Iterate over the stem(s) of "piece"
                    foreach (HunspellStemResult stem in Stem(piece)) {

                        // Append the stem if it isn't equal to the input
                        if (piece != stem.Value) temp2.Add(HunspellExpandWord.Stem(stem.Value, piece));
                        
                        // Append each variant/morph to the list
                        foreach (string variant in Morph(stem)) {
                            or4.Append(variant);
                        }

                        // Fallback: append the stem if it wasn't found in the custom dictionary
                        or4.Append(stem.Value);

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

                        // Are "x" and "y" spelled correctly when put together as a single word? (eg. compound words in Danish)
                        if (Spell(z)) {

                            AndList and = new AndList();
                            or.Query.Add(and);

                            and.Append(x);

                            if (Spell(y)) {
                                
                                OrList or2 = new OrList();
                                and.Query.Add(or2);

                                // Iterate over the stem(s) of "y"
                                foreach (HunspellStemResult stem in Stem(y)) {

                                    // Append each variant/morph to the list
                                    foreach (string variant in Morph(stem)) {
                                        or2.Append(variant);
                                    }

                                }

                            } else {
                            
                                and.Append(y);

                            }

                            OrList or5 = new OrList();

                            // Iterate over the stem(s) of "z"
                            foreach (HunspellStemResult stem in Stem(z)) {
                            
                                if (z != stem.Value) temp2.Add(HunspellExpandWord.Stem(stem.Value, z));

                                // Append each variant/morph to the list
                                foreach (string variant in Morph(stem)) {
                                    or5.Append(variant);
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

                    // Get the suggestions for "piece"
                    string[] suggestions = Suggest(piece);

                    // If case insensitivity is enabled, we're looking for a suggestion that is spelled the same way,
                    // but with different casing. If a match is found, all other suggestions are ignored as they may
                    // lead to a higher amount of "unrelated" search results.
                    //
                    // This will typically be the case if the user enters a name in lowercase (eg. "ole" instead of "Ole"),
                    // which then technically is a spelling error. The same may be the case for abbreviations.
                    if (options.CaseInsentive) {
                        
                        string insensitiveMatch = suggestions.FirstOrDefault(x => string.Equals(x, piece, StringComparison.InvariantCultureIgnoreCase));

                        if (insensitiveMatch != null) {

                            // Append the match with the correct casing
                            or.Append(insensitiveMatch);

                            // Iterate over the stem(s) of "suggestion"
                            foreach (HunspellStemResult stem in Stem(insensitiveMatch)) {

                                // Append each variant/morph to the list
                                foreach (string variant in Morph(stem)) {
                                    or.Append(variant);
                                }

                            }

                            continue;

                        }

                    }

                    // Append the word as entered by the user
                    or.Append(piece);

                    foreach (string suggestion in suggestions) {

                        // Calculate the Levenshtein distance
                        int distance = StringUtils.Levenshtein(piece, suggestion);

                        // Skip the suggestion if the Levenshtein distance is higher than the allowed maximum
                        if (options.MaxDistance > 0 && distance > options.MaxDistance) continue;
                        
                        temp2.Add(new HunspellExpandWord(HunspellExpandWordType.Suggestion, suggestion, "Levenshtein: " + distance));
                        or.Append(suggestion);

                        // Iterate over the stem(s) of "suggestion"
                        foreach (HunspellStemResult stem in Stem(suggestion)) {

                            // Append each variant/morph to the list
                            foreach (string variant in Morph(stem)) {
                                or.Append(variant);
                            }

                        }

                    }

                }

            }

            return new HunspellExpandResult(options.Text, temp1.Select(x => x.ToArray()).ToArray(), query);

        }

        #endregion

        #region Static methods

        /// <summary>
        /// Initializes a new Hunspell text analyzer from the specified <paramref name="dictionaryPath"/> and <paramref name="affixPath"/>.
        /// </summary>
        /// <param name="dictionaryPath">The path to the dictionary (<c>.dic</c>) file.</param>
        /// <param name="affixPath">The path to the dictionary (<c>.aff</c>) file.</param>
        /// <returns>A new <see cref="HunspellTextAnalyzer"/>.</returns>
        public static HunspellTextAnalyzer CreateFromFiles(string dictionaryPath, string affixPath) {

            using FileStream dictionaryStream = File.OpenRead(dictionaryPath);

            using FileStream affixStream = File.OpenRead(affixPath);
    
            WordList wordList = WordList.CreateFromStreams(dictionaryStream, affixStream);

            HunspellAffix affix = HunspellAffix.Load(affixPath);

            HunspellDictionary dictionary = HunspellDictionary.Load(dictionaryPath, affix);

            return new HunspellTextAnalyzer(wordList, affix, dictionary);

        }

        #endregion

    }

}