namespace Skybrud.TextAnalysis.Hunspell {
    
    public class HunspellStemResult {

        public string Prefix { get; }

        public string Stem { get; }

        public HunspellStemResult(string stem, string prefix) {
            Prefix = prefix ?? string.Empty;
            Stem = stem;
        }

    }

}