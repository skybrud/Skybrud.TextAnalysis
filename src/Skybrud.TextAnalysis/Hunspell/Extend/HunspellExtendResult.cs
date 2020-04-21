using Skybrud.TextAnalysis.Search;

namespace Skybrud.TextAnalysis.Hunspell {

    public class HunspellExtendResult {

        public Word[][] Words { get; }

        public AndList Query { get; }

        public HunspellExtendResult(Word[][] words, AndList query) {
            Words = words;
            Query = query;
        }

    }

}