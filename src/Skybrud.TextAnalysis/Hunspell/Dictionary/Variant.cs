namespace Skybrud.TextAnalysis.Hunspell.Dictionary {

    /// <summary>
    /// Class representing a variant/variation/inflection of a word.
    /// </summary>
    public class Variant {

        /// <summary>
        /// Gets the type of the variant.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Gets the value of the variant.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Initializes a new variant.
        /// </summary>
        /// <param name="value">The value of the variant.</param>
        /// <param name="type">The type of the variant.</param>
        public Variant(string value, string type) {
            Value = value;
            Type = type;
        }

    }

}