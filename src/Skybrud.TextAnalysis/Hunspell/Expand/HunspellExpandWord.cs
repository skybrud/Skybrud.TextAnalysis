using System;

namespace Skybrud.TextAnalysis.Hunspell.Expand {

    public class HunspellExpandWord {

        public HunspellExpandWordType Type { get; set; }

        public string Value { get; set; }

        public bool IsMisspelled { get; set; }

        public string Comment { get; set; }

        public HunspellExpandWord(HunspellExpandWordType type, string value) {
            Type = type;
            Value = value.Split(new [] {"(underbegreb)"}, StringSplitOptions.RemoveEmptyEntries)[0];
        }

        public HunspellExpandWord(HunspellExpandWordType type, string value, string comment) : this(type, value) {
            Comment = comment;
        }

        public static HunspellExpandWord Suggestion(string value, string from) {
            return new HunspellExpandWord(HunspellExpandWordType.Suggestion, value, "via " + from);
        }

        public static HunspellExpandWord Stem(string value, string from) {
            return new HunspellExpandWord(HunspellExpandWordType.Stem, value, "via " + from);
        }

    }
}