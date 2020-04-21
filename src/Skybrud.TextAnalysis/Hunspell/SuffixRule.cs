using System.Linq;
using System.Text.RegularExpressions;

namespace Skybrud.TextAnalysis.Hunspell {

    public class SuffixRule {

        public string Raw { get; }

        public string Value1 { get; }

        public string Value2 { get; }

        public string Value3 { get; }

        public string Comment { get; }

        public SuffixRule(string rule) {

            Raw = rule;

            string[] pieces = rule.Split(' ', '\t');

            Value1 = pieces[2];
            Value2 = pieces[3];
            Value3 = pieces[4];
            Comment = pieces.Length >= 6 ? pieces[5] : null;

        }

        public void Process(MyDictionaryItem item) {

            Match m1 = Regex.Match(Value3, "^([0-9a-zA-Z]+)$");
            Match m2 = Regex.Match(Value3, "^\\[([0-9a-zA-Z]+)\\]$");
            Match m3 = Regex.Match(Value3, "^\\[\\^([0-9a-zA-Z]+)\\]$");

            Match m4 = Regex.Match(Value3, "^([0-9a-zA-Z]+)$");

            Match m5 = Regex.Match(Value3, "^\\[\\^([0-9a-zA-Z]+)\\]([a-z]+)$");

            string suffix = Value2.Split('/')[0];

            if (suffix == "0") return;

            string comment = Raw;

            if (Value3 == ".") {
                item.AddVariant(item.Stem + Value2.Split('/')[0], comment);
                return;
            }

            if (m1.Success) {
                comment += " M1";
                if (item.Stem.EndsWith(m1.Groups[1].Value)) {
                    item.AddVariant(item.Stem + Value2.Split('/')[0], comment);
                }
                return;
            }

            if (m2.Success) {
                comment += " M2";
                char[] allowed = m2.Groups[1].Value.ToArray();
                if (allowed.Contains(item.Stem[item.Stem.Length - 1])) {
                    item.AddVariant(item.Stem + Value2.Split('/')[0], comment);
                }
                return;
            }

            if (m3.Success) {
                comment += " M3";
                char[] disallowed = m3.Groups[1].Value.ToArray();
                if (disallowed.All(x => x != item.Stem[item.Stem.Length - 1])) {
                    item.AddVariant(item.Stem + Value2.Split('/')[0], comment);
                }
                return;
            }

            if (m4.Success) {
                comment += " M4";
                if (item.Stem.EndsWith(m4.Groups[1].Value)) {
                    item.AddVariant(item.Stem + Value2.Split('/')[0], comment);
                }
                return;
            }

            if (m5.Success) {

                comment += " M5";

                char[] range = m5.Groups[1].Value.ToArray();

                int pos = item.Stem.Length - m5.Groups[2].Value.Length - 1;

                // If the first value is different from "0", it means that we should remove the value from the end of the stem
                string stem = Value1 == "0" ? item.Stem : item.Stem.Substring(0, item.Stem.Length - Value1.Length);
                
                if (item.Stem.EndsWith(m5.Groups[2].Value) && range.Contains(item.Stem[pos]) == false) {
                    item.AddVariant(stem + Value2.Split('/')[0], comment);
                }

                return;

            }

        }

    }
}