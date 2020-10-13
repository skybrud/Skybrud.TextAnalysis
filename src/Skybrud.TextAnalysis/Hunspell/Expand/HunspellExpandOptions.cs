namespace Skybrud.TextAnalysis.Hunspell.Expand {

    /// <summary>
    /// Class with options for the <see cref="HunspellTextAnalyzer.Expand(HunspellExpandOptions)"/> method.
    /// </summary>
    public class HunspellExpandOptions {

        #region Properties

        /// <summary>
        /// Gets or sets the text. For a search, this will typically be the raw string the user enters into the search field.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the maximum Levenshtein distance.
        /// </summary>
        /// <remarks>For misspelled words, the <see cref="HunspellTextAnalyzer.Expand(HunspellExpandOptions)"/> method
        /// will automatically use the dictionary to find suggestions. The Levenshtein algorithm is then used to
        /// compute the distance between the input word and the suggested word. Low distance means high similarity.
        ///
        /// If specified, suggested words with a distance highter than the value of this property will be ignored.</remarks>
        public int MaxDistance { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance with default options.
        /// </summary>
        public HunspellExpandOptions() { }

        /// <summary>
        /// Initializes a new instance based on the specified <paramref name="text"/>.
        /// </summary>
        /// <param name="text">Gets or sets the text. This will typically be the raw string the user enters into the search field.</param>
        public HunspellExpandOptions(string text) {
            Text = text;
        }

        #endregion

    }

}