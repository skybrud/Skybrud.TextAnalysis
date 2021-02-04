using System.Collections.Generic;
using System.Linq;
using Skybrud.TextAnalysis.Hunspell.Affix;

namespace Skybrud.TextAnalysis.Hunspell.Dictionary {

    /// <summary>
    /// Class representing a Hunnspull dictionary item.
    /// </summary>
    public class HunspellDictionaryItem {

        private HunspellDictionaryItemType _type;
        private List<Variant> _variants;

        #region Properties

        /// <summary>
        /// Gets the stem of the item.
        /// </summary>
        public string Stem { get; }

        /// <summary>
        /// Gets the flags of the item.
        /// </summary>
        public int[] Flags { get; }

        /// <summary>
        /// Gets the most likely type of the item.
        /// </summary>
        public HunspellDictionaryItemType Type {
            get {
                if (_variants == null) Init();
                return _type;
            }
        }

        /// <summary>
        /// Gets the variants/variations/inflections of the item.
        /// </summary>
        public List<Variant> Variants {
            get {
                if (_variants == null) Init();
                return _variants;
            }
        }

        /// <summary>
        /// Gets a reference to the <see cref="HunspellAffix"/> this item belongs to.
        /// </summary>
        protected HunspellAffix Affix { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new dictionary based on the specified <paramref name="stem"/>, <paramref name="flags"/> and <paramref name="affix"/> file.
        /// </summary>
        /// <param name="stem">The stem of the item.</param>
        /// <param name="flags">The flags of the item.</param>
        /// <param name="affix">A reference to the parent affix file.</param>
        public HunspellDictionaryItem(string stem, int[] flags, HunspellAffix affix) {
            Stem = stem;
            Flags = flags;
            Affix = affix;
        }

        #endregion

        #region Member methods

        private void Init() {

            _variants = new List<Variant>();

            _type = HunspellDictionaryItemType.Ukendt;

            _variants.Add(new Variant(Stem, string.Empty));

            if (Flags.Contains(55)) _type = HunspellDictionaryItemType.Proprium;

            if (Flags.Contains(6)) _type = HunspellDictionaryItemType.Verbum;
            if (Flags.Contains(140)) _type = HunspellDictionaryItemType.Verbum;
            if (Flags.Contains(143)) _type = HunspellDictionaryItemType.Verbum;
            if (Flags.Contains(148)) _type = HunspellDictionaryItemType.Verbum;

            if (Flags.Contains(46)) _type = HunspellDictionaryItemType.Substantiv;
            if (Flags.Contains(73)) _type = HunspellDictionaryItemType.Substantiv;
            if (Flags.Contains(193)) _type = HunspellDictionaryItemType.Substantiv;
            if (Flags.Contains(194)) _type = HunspellDictionaryItemType.Substantiv;
            if (Flags.Contains(252)) _type = HunspellDictionaryItemType.Substantiv;
            if (Flags.Contains(254)) _type = HunspellDictionaryItemType.Substantiv;
            if (Flags.Contains(736)) _type = HunspellDictionaryItemType.Substantiv;
            if (Flags.Contains(737)) _type = HunspellDictionaryItemType.Substantiv;
            if (Flags.Contains(815)) _type = HunspellDictionaryItemType.Substantiv;
            
            foreach (int flag in Flags) {
                if (Affix.SuffixRuleSets.TryGetValue(flag.ToString(), out SuffixRuleSet ruleSet)) {
                    ruleSet.Process(this);
                }
            }

        }

        /// <summary>
        /// Adds a variant with the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value of the variant.</param>
        /// <param name="comment">A comment about the variant.</param>
        public void AddVariant(string value, string comment) {

            _variants.Add(new Variant(value, comment));

            switch ((comment ?? string.Empty).Split('+').Last()) {
                case "GENITIV":
                case "PLUR_BEK":
                case "PLUR_UBEK":
                case "BESTEMT_ENTAL":
                    _type = HunspellDictionaryItemType.Substantiv;
                    break;
            }

        }

        #endregion

    }

}