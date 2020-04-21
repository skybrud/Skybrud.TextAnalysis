using System.Linq;

namespace Skybrud.TextAnalysis.Hunspell {

    public class SuffixRuleSet     {

        public string Name { get; }

        public string Comment { get; }

        public SuffixRule[] Rules { get; }

        public SuffixRuleSet(string name, string comment, string[] rules) {
            Name = name;
            Comment = comment;
            Rules = rules.Select(x => new SuffixRule(x)).ToArray();
        }

        public void Process(MyDictionaryItem item) {
            foreach (SuffixRule rule in Rules) {
                rule.Process(item);
            }
        }

    }

}