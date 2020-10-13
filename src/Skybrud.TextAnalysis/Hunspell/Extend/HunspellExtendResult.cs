using Skybrud.TextAnalysis.Search;

namespace Skybrud.TextAnalysis.Hunspell {

    public class HunspellExtendResult {

        public string Text { get; }

        public HunspellExtendWord[][] Words { get; }

        public AndList Query { get; }

        public HunspellExtendResult(string text, HunspellExtendWord[][] words, AndList query) {
            Text = text;
            Words = words;
            Query = query;
        }

    }

}