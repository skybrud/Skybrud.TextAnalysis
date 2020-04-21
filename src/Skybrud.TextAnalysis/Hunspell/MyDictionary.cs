using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Skybrud.TextAnalysis.Hunspell {

    public class MyDictionary {

        #region Properties

        /// <summary>
        /// Gets a reference to the internal dictionary.
        /// </summary>
        public Dictionary<string, List<MyDictionaryItem>> Dictionary { get; }

        #endregion

        #region Constructors

        private MyDictionary(Dictionary<string, List<MyDictionaryItem>> dictionary) {
            Dictionary = dictionary;
        }

        private MyDictionary(MyDictionaryItem[] array) {

            Dictionary = new Dictionary<string, List<MyDictionaryItem>>();

            foreach (MyDictionaryItem item in array) {

                if (Dictionary.TryGetValue(item.Stem, out List<MyDictionaryItem> list) == false) {
                    list = new List<MyDictionaryItem>();
                    Dictionary.Add(item.Stem, list);
                }

                list.Add(item);

            }

        }

        #endregion

        #region Member methods

        public bool TryGet(string stem, out List<MyDictionaryItem> list) {
            return Dictionary.TryGetValue(stem, out list);
        }

        public static MyDictionary Load(string path, Affix affix) {

            Dictionary<string, List<MyDictionaryItem>> temp = new Dictionary<string, List<MyDictionaryItem>>();

            foreach (string line in File.ReadAllLines(path).Skip(1)) {

                if (line == "lægge, lægger, lægges, lagde, lagdes, læggende, læggendes, lagt, lagts, lagte, lagtes, læg") continue;

                string[] hest = line.Split('/', ',');

                try {

                    MyDictionaryItem item = new MyDictionaryItem(hest[0], hest.Skip(1).Select(int.Parse).ToArray(), affix);

                    // Sammensætning, fugeelement
                    if (item.Flags.Length > 0 && item.Flags[0] == 941) continue;

                    if (temp.TryGetValue(item.Stem, out List<MyDictionaryItem> list) == false) {
                        list = new List<MyDictionaryItem>();
                        temp.Add(item.Stem, list);
                    }

                    list.Add(item);

                } catch (Exception ex) {

                    throw new Exception("Unable to parse line: " + line, ex);

                }

            }

            return new MyDictionary(temp);

        }

        #endregion

    }

}