using Skybrud.TextAnalysis.Search;

namespace Skybrud.TextAnalysis.Hunspell.Expand {

    /// <summary>
    /// Class representing the result of a Hunspell expand operation.
    /// </summary>
    public class HunspellExpandResult {

        #region Properties

        /// <summary>
        /// Gets the text that was expanded.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets a two dimensional array with the words that was the result of the expand operation.
        /// </summary>
        public HunspellExpandWord[][] Words { get; }

        /// <summary>
        /// Gets a reference to the query that was the result of the expand operation.
        /// </summary>
        public AndList Query { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance based on the specified parameters.
        /// </summary>
        /// <param name="text">The input text that was expanded.</param>
        /// <param name="words">A two dimensional array with the words that was the result of the expand operation.</param>
        /// <param name="query">The query that was the result of the expand operation.</param>
        public HunspellExpandResult(string text, HunspellExpandWord[][] words, AndList query) {
            Text = text;
            Words = words;
            Query = query;
        }

        #endregion

    }

}