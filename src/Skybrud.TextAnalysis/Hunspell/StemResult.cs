namespace Skybrud.TextAnalysis.Hunspell {
    
    public class StemResult {

        public string Prefix { get; }

        public string Stem { get; }

        public StemResult(string stem, string prefix) {
            Prefix = prefix ?? string.Empty;
            Stem = stem;
        }

    }

}