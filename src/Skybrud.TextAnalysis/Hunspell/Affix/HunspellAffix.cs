using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Skybrud.TextAnalysis.Hunspell.Affix {

    /// <summary>
    /// Class representing a Hunspell affix file.
    ///
    /// The affix file defines a number of different rule sets. For instance, the suffix rule sets defines how
    /// different suffixes can be appended to the stem of a particular word. Eg. so that <c>car</c> becomes <c>cars</c>
    /// in plural, but <c>fox</c> becomes <c>foxes</c>.
    /// </summary>
    public class HunspellAffix {

        #region Properties

        /// <summary>
        /// Gets a dictionary with all suffix rule sets of the affix file.
        /// </summary>
        public Dictionary<string, SuffixRuleSet> SuffixRuleSets { get; }

        #endregion

        #region Constructors

        private HunspellAffix(string raw) {

            string[] lines = raw.Split('\n');

            SuffixRuleSets = new Dictionary<string, SuffixRuleSet>();

            for (int i = 0; i < lines.Length; i++)             {

                if (lines[i].Length == 0 || lines[i][0] == '#') continue;

                string last = i == 0 ? null : lines[i - 1];

                if (lines[i].StartsWith("SFX "))                 {

                    string[] pieces = lines[i].Split(' ', '\t');

                    string name = pieces[1];
                    string comment = last != null && last.StartsWith("#") ? last.Substring(1).Trim() : null;
                    int count = int.Parse(pieces[3]);
                    string[] rules = lines.Skip(i + 1).Take(count).ToArray();

                    var set = new SuffixRuleSet(name, comment, rules);
                    SuffixRuleSets.Add(set.Name, set);

                    i += count;

                }

            }

        }

        #endregion

        #region Static methods

        /// <summary>
        /// Loads the affix file at the specified <paramref name="path"/>.
        /// </summary>
        /// <param name="path">The path to the affix file.</param>
        /// <returns>A new <see cref="HunspellAffix"/> instance.</returns>
        public static HunspellAffix Load(string path) {
            return new HunspellAffix(File.ReadAllText(path));
        }

        #endregion

    }

}