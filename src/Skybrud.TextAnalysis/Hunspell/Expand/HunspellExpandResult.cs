using Skybrud.TextAnalysis.Search;

namespace Skybrud.TextAnalysis.Hunspell.Expand {

    public class HunspellExpandResult {

        public string Text { get; }

        public HunspellExpandWord[][] Words { get; }

        public AndList Query { get; }

        public HunspellExpandResult(string text, HunspellExpandWord[][] words, AndList query) {
            Text = text;
            Words = words;
            Query = query;
        }

    }

}