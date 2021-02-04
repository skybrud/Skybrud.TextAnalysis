using System.Linq;
using Skybrud.TextAnalysis.Hunspell.Dictionary;

namespace Skybrud.TextAnalysis.Hunspell.Affix {

    /// <summary>
    /// Class representing a suffix rule set from an Hunspell affix file.
    /// </summary>
    public class SuffixRuleSet {

        #region Properties

        /// <summary>
        /// Gets the name of the rule set (if specified via a comment on the line before).
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the comment of the rule set.
        /// </summary>
        public string Comment { get; }

        /// <summary>
        /// Gets an array of the individual rules of the rule set.
        /// </summary>
        public SuffixRule[] Rules { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new rule set based on the specified <paramref name="name"/>, <paramref name="comment"/> and array of <paramref name="rules"/>.
        /// </summary>
        /// <param name="name">The name of the rule set.</param>
        /// <param name="comment">An optional comment about the rule set.</param>
        /// <param name="rules">The individual rules of the rule set.</param>
        public SuffixRuleSet(string name, string comment, string[] rules) {
            Name = name;
            Comment = comment;
            Rules = rules.Select(x => new SuffixRule(x)).ToArray();
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Processes the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">The dictionary item to parse.</param>
        public void Process(HunspellDictionaryItem item) {
            foreach (SuffixRule rule in Rules) {
                rule.Process(item);
            }
        }

        #endregion

    }

}