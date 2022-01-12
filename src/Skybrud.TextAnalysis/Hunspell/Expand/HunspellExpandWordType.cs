namespace Skybrud.TextAnalysis.Hunspell.Expand {
    
    namespace Skybrud.TextAnalysis.Hunspell.Expand {

        /// <summary>
        /// Enum class representing the type of an expanded word.
        /// </summary>
        public enum HunspellExpandWordType {

            /// <summary>
            /// Indicates that the word is based on user input.
            /// </summary>
            Input,

            /// <summary>
            /// Indicates that the word is a stem.
            /// </summary>
            Stem,

            /// <summary>
            /// Indicates that the word is a suggestion (eg. if the input word was spelled incorrectly).
            /// </summary>
            Suggestion,

            /// <summary>
            /// Indicates that the word is a synonym to the input word.
            /// </summary>
            Synonym

        }

    }

}