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

        #endregion

        #region Constructors

        private HunspellDictionary(Dictionary<string, List<HunspellDictionaryItem>> dictionary) {
            Dictionary = dictionary;
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

            foreach (string line in File.ReadAllLines(path).Skip(1)) {

                if (line == "lægge, lægger, lægges, lagde, lagdes, læggende, læggendes, lagt, lagts, lagte, lagtes, læg") continue;

                string[] hest = line.Split('/', ',');

                try {

                    HunspellDictionaryItem item = new HunspellDictionaryItem(hest[0], hest.Skip(1).Select(int.Parse).ToArray(), affix);

                    // Sammensætning, fugeelement
                    if (item.Flags.Length > 0 && item.Flags[0] == 941) continue;

                    if (temp.TryGetValue(item.Stem, out List<HunspellDictionaryItem> list) == false) {
                        list = new List<HunspellDictionaryItem>();
                        temp.Add(item.Stem, list);
                    }

                    list.Add(item);

                } catch (Exception ex) {

                    throw new Exception("Unable to parse line: " + line, ex);

                }

            }

            return new HunspellDictionary(temp);

        }

        #endregion

    }

}