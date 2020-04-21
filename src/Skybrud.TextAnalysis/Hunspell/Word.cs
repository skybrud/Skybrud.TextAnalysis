using System;

namespace Skybrud.TextAnalysis.Hunspell {

    public class Word {

        public WordType Type { get; set; }

        public string Value { get; set; }

        public bool IsMisspelled { get; set; }

        public string Comment { get; set; }

        public Word(WordType type, string value) {
            Type = type;
            Value = value.Split(new [] {"(underbegreb)"}, StringSplitOptions.RemoveEmptyEntries)[0];
        }

        public Word(WordType type, string value, string comment) : this(type, value) {
            Comment = comment;
        }

        public static Word Suggestion(string value, string from) {
            return new Word(WordType.Suggestion, value, "via " + from);
        }

        public static Word Stem(string value, string from) {
            return new Word(WordType.Stem, value, "via " + from);
        }

    }

    public enum WordType {
        Input,
        Stem,
        Suggestion,
        Synonym
    }

}