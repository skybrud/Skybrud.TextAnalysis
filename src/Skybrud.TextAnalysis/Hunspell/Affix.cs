using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Skybrud.TextAnalysis.Hunspell {

    public class Affix {

        public Dictionary<string, SuffixRuleSet> SuffixRuleSets { get; }

        private Affix(string raw)         {

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

        public static Affix Load(string path) {
            return new Affix(File.ReadAllText(path));
        }

    }

}