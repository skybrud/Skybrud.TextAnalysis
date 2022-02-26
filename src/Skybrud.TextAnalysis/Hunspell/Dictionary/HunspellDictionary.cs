using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Skybrud.TextAnalysis.Hunspell.Affix;

namespace Skybrud.TextAnalysis.Hunspell.Dictionary {

    /// <summary>
    /// Class representing a Hunspell dictionary.
    /// </summary>
    public class HunspellDictionary {

        #region Properties

        /// <summary>
        /// Gets a reference to the internal dictionary.
        /// </summary>
        public Dictionary<string, List<HunspellDictionaryItem>> Dictionary { get; }

        /// <summary>
        /// If loading the dictionary fails, the <see cref="Exception"/> property with the exception describing why the load failed.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Gets whether the dictionary was successfully loaded.
        /// </summary>
        public bool IsSuccessful { get; }

        #endregion

        #region Constructors

        private HunspellDictionary(Dictionary<string, List<HunspellDictionaryItem>> dictionary, Exception exception) {
            Dictionary = dictionary;
            Exception = exception;
            IsSuccessful = exception == null;
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Gets the value associated with the specified <paramref name="stem"/> word.
        /// </summary>
        /// <param name="stem">The stem word.</param>
        /// <param name="list">A list of dictionary items matching the stem word.</param>
        /// <returns><c>true</c> if the dictionary contains the specified stem word; otherwise, <c>false</c>.</returns>
        public bool TryGet(string stem, out List<HunspellDictionaryItem> list) {
            return Dictionary.TryGetValue(stem, out list);
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Loads the dictionary at the specified <paramref name="path"/>.
        /// </summary>
        /// <param name="path">The path to the dictonary (<c></c>.dic<c> file)</c>.</param>
        /// <param name="affix">The affix file to be used with the dictionary.</param>
        /// <returns>A new <see cref="HunspellDictionary"/> instance.</returns>
        public static HunspellDictionary Load(string path, HunspellAffix affix) {

            Dictionary<string, List<HunspellDictionaryItem>> temp = new Dictionary<string, List<HunspellDictionaryItem>>();

            try {

                string[] lines = File.ReadAllLines(path);

                for (int i = 1; i < lines.Length - 1; i++) {

                    string line = lines[i];

                    if (string.IsNullOrWhiteSpace(line)) continue;

                    try {

                        if (line == "lægge, lægger, lægges, lagde, lagdes, læggende, læggendes, lagt, lagts, lagte, lagtes, læg") continue;
                    
                        // Danish dictionary have some additional comments or operators that makes the parser fail, so we need to skip those
                        int index = line.IndexOf(" al:", StringComparison.CurrentCultureIgnoreCase);
                        if (index > 0) line = line.Substring(0, index);
                        
                        index = line.IndexOf(" st:", StringComparison.CurrentCultureIgnoreCase);
                        if (index > 0) line = line.Substring(0, index);

                        index = line.IndexOf(" ph:", StringComparison.CurrentCultureIgnoreCase);
                        if (index > 0) line = line.Substring(0, index);
                        
                        string word;
                        string options;

                        if (line[0] == '"') {
                            
                            index = line.IndexOf("\"", 1, StringComparison.Ordinal);
                            if (index < 0) throw new Exception($"Unable to parse line {i + 1}: {line}");

                            word = line.Substring(1, index - 1);

                            options = line.Substring(index + 1);

                        } else {
                            
                            index = line.IndexOf("/", 1, StringComparison.Ordinal);

                            if (index > 0) {
                                word = line.Substring(0, index);
                                options = line.Substring(index + 1);
                            } else {
                                word = line;
                                options = string.Empty;
                            }

                        }

                        try {

                            int[] flags = string.IsNullOrWhiteSpace(options) ? new int[0] : options.Split(',').Select(int.Parse).ToArray();

                            HunspellDictionaryItem item = new HunspellDictionaryItem(word, flags, affix);

                            // Sammensætning, fugeelement
                            if (item.Flags.Length > 0 && item.Flags[0] == 941) continue;

                            if (temp.TryGetValue(item.Stem, out List<HunspellDictionaryItem> list) == false) {
                                list = new List<HunspellDictionaryItem>();
                                temp.Add(item.Stem, list);
                            }

                            list.Add(item);

                        } catch (Exception ex) {

                            throw new Exception($"Unable to parse line {i + 1}: {line}\r\n\r\nWord: {word}\r\nOptions: {options}", ex);

                        }

                    } catch (Exception ex) {

                        throw new Exception($"Unable to parse line {i + 1}: {line}", ex);

                    }

                }

                return new HunspellDictionary(temp, null);

            } catch (Exception ex) {

                return new HunspellDictionary(temp, ex);

            }

        }

        #endregion

    }

}