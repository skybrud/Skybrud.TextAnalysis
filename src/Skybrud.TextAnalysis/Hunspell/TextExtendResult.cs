using Skybrud.TextAnalysis.Search;

namespace Skybrud.TextAnalysis.Hunspell {

    public class TextExtendResult {

        public Word[][] Words { get; }

        public AndList Query { get; }

        public TextExtendResult(Word[][] words, AndList query) {
            Words = words;
            Query = query;
        }

    }

}