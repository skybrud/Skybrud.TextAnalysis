namespace Skybrud.TextAnalysis.Hunspell.Stem {
    
    public class HunspellStemResult {

        public string Prefix { get; }

        public string Stem { get; }

        public string Value { get; }

        public HunspellStemResult(string stem, string prefix) {
            Prefix = prefix ?? string.Empty;
            Stem = stem;
            Value = Prefix + Stem;
        }

    }

}