namespace Skybrud.TextAnalysis.Hunspell.Stem {
    
    /// <summary>
    /// Class representing a Hunspell stem result.
    /// </summary>
    public class HunspellStemResult {

        /// <summary>
        /// Gets the prefix of the word. For some compound words, the stem is only the last part, in which case we need
        /// to keep track of the prefix - eg. if we later need to morph the word.
        /// </summary>
        public string Prefix { get; }

        /// <summary>
        /// Gets the stem.
        /// </summary>
        public string Stem { get; }

        /// <summary>
        /// Gets the combined value of <see cref="Prefix"/> and <see cref="Stem"/>.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Initializes a new instance based on the specified <paramref name="stem"/> and <paramref name="prefix"/>.
        /// </summary>
        /// <param name="stem">The stem of the input word.</param>
        /// <param name="prefix">An optional prefix of the input word.</param>
        public HunspellStemResult(string stem, string prefix) {
            Prefix = prefix ?? string.Empty;
            Stem = stem;
            Value = Prefix + Stem;
        }

    }

}