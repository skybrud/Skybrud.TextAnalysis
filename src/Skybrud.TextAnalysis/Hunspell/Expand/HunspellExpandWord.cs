using System;

namespace Skybrud.TextAnalysis.Hunspell.Expand {

    /// <summary>
    /// Class representing a word in a Hunspell expand result.
    /// </summary>
    public class HunspellExpandWord {

        #region Properties

        /// <summary>
        /// Gets the type of the word.
        /// </summary>
        public HunspellExpandWordType Type { get; set; }

        /// <summary>
        /// Gets the value of the word.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets whether the word was misspelled.
        /// </summary>
        public bool IsMisspelled { get; set; }

        /// <summary>
        /// Gets the comment of the word.
        /// </summary>
        public string Comment { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance based on the specified <paramref name="type"/> and <paramref name="value"/>.
        /// </summary>
        /// <param name="type">The type of the word.</param>
        /// <param name="value">The value of the word.</param>
        public HunspellExpandWord(HunspellExpandWordType type, string value) {
            Type = type;
            Value = value.Split(new [] {"(underbegreb)"}, StringSplitOptions.RemoveEmptyEntries)[0];
        }

        /// <summary>
        /// Initializes a new instance based on the specified <paramref name="type"/>, <paramref name="value"/> and <paramref name="comment"/>.
        /// </summary>
        /// <param name="type">The type of the word.</param>
        /// <param name="value">The value of the word.</param>
        /// <param name="comment">A comment about the word and it's origin.</param>
        public HunspellExpandWord(HunspellExpandWordType type, string value, string comment) : this(type, value) {
            Comment = comment;
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Returns a new suggestion based on the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The suggestion.</param>
        /// <param name="from">The word that resulted in the suggestion.</param>
        /// <returns>An instance of <see cref="HunspellExpandWord"/>.</returns>
        public static HunspellExpandWord Suggestion(string value, string from) {
            return new HunspellExpandWord(HunspellExpandWordType.Suggestion, value, "via " + from);
        }

        /// <summary>
        /// Returns a new stem based on the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The suggestion.</param>
        /// <param name="from">The word that resulted in the stem.</param>
        /// <returns>An instance of <see cref="HunspellExpandWord"/>.</returns>
        public static HunspellExpandWord Stem(string value, string from) {
            return new HunspellExpandWord(HunspellExpandWordType.Stem, value, "via " + from);
        }

        #endregion

    }

}