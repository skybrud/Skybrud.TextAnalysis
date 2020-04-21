namespace Skybrud.TextAnalysis.Hunspell {

    public class Variant {

        public string Type { get; }

        public string Value { get; }

        public Variant(string value, string type) {
            Value = value;
            Type = type;
        }

    }

}