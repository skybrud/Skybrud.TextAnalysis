using System.Collections.Generic;
using System.Linq;

namespace Skybrud.TextAnalysis.Hunspell {

    public class MyDictionaryItem {

        private MyDictionaryItemType _type;
        private List<Variant> _variants;

        #region Properties

        public string Stem { get; }

        public int[] Flags { get; }

        /// <summary>
        /// Gets the most likely type of the item.
        /// </summary>
        public MyDictionaryItemType Type {
            get {
                if (_variants == null) Init();
                return _type;
            }
        }

        public List<Variant> Variants {
            get {
                if (_variants == null) Init();
                return _variants;
            }
        }

        protected Affix Affix { get; }

        #endregion

        public MyDictionaryItem(string stem, int[] flags, Affix affix) {
            Stem = stem;
            Flags = flags;
            Affix = affix;
        }

        private void Init() {

            _variants = new List<Variant>();

            _type = MyDictionaryItemType.Ukendt;

            _variants.Add(new Variant(Stem, string.Empty));

            if (Flags.Contains(55)) _type = MyDictionaryItemType.Proprium;

            if (Flags.Contains(6)) _type = MyDictionaryItemType.Verbum;
            if (Flags.Contains(140)) _type = MyDictionaryItemType.Verbum;
            if (Flags.Contains(143)) _type = MyDictionaryItemType.Verbum;
            if (Flags.Contains(148)) _type = MyDictionaryItemType.Verbum;

            if (Flags.Contains(46)) _type = MyDictionaryItemType.Substantiv;
            if (Flags.Contains(73)) _type = MyDictionaryItemType.Substantiv;
            if (Flags.Contains(193)) _type = MyDictionaryItemType.Substantiv;
            if (Flags.Contains(194)) _type = MyDictionaryItemType.Substantiv;
            if (Flags.Contains(252)) _type = MyDictionaryItemType.Substantiv;
            if (Flags.Contains(254)) _type = MyDictionaryItemType.Substantiv;
            if (Flags.Contains(736)) _type = MyDictionaryItemType.Substantiv;
            if (Flags.Contains(737)) _type = MyDictionaryItemType.Substantiv;
            if (Flags.Contains(815)) _type = MyDictionaryItemType.Substantiv;
            
            foreach (int flag in Flags) {
                if (Affix.SuffixRuleSets.TryGetValue(flag.ToString(), out SuffixRuleSet ruleSet)) {
                    ruleSet.Process(this);
                }
            }

        }

        public void AddVariant(string value, string comment) {

            _variants.Add(new Variant(value, comment));

            switch ((comment ?? string.Empty).Split('+').Last()) {
                case "GENITIV":
                case "PLUR_BEK":
                case "PLUR_UBEK":
                case "BESTEMT_ENTAL":
                    _type = MyDictionaryItemType.Substantiv;
                    break;
            }

        }

    }

}