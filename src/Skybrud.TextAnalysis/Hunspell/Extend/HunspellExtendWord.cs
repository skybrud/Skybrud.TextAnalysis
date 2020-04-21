using System;

namespace Skybrud.TextAnalysis.Hunspell {

    public class HunspellExtendWord {

        public HunspellExtendWordType Type { get; set; }

        public string Value { get; set; }

        public bool IsMisspelled { get; set; }

        public string Comment { get; set; }

        public HunspellExtendWord(HunspellExtendWordType type, string value) {
            Type = type;
            Value = value.Split(new [] {"(underbegreb)"}, StringSplitOptions.RemoveEmptyEntries)[0];
        }

        public HunspellExtendWord(HunspellExtendWordType type, string value, string comment) : this(type, value) {
            Comment = comment;
        }

        public static HunspellExtendWord Suggestion(string value, string from) {
            return new HunspellExtendWord(HunspellExtendWordType.Suggestion, value, "via " + from);
        }

        public static HunspellExtendWord Stem(string value, string from) {
            return new HunspellExtendWord(HunspellExtendWordType.Stem, value, "via " + from);
        }

    }
}