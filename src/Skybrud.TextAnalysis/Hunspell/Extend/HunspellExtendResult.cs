using Skybrud.TextAnalysis.Search;

namespace Skybrud.TextAnalysis.Hunspell {

    public class HunspellExtendResult {

        public HunspellExtendWord[][] Words { get; }

        public AndList Query { get; }

        public HunspellExtendResult(HunspellExtendWord[][] words, AndList query) {
            Words = words;
            Query = query;
        }

    }

}