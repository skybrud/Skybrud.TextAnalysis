namespace Skybrud.TextAnalysis.Hunspell.Dictionary {

    /// <summary>
    /// Enum class representing the type of a dictionary item.
    /// </summary>
    public enum HunspellDictionaryItemType {

        /// <summary>
        /// Indicates that the type is not known.
        /// </summary>
        Ukendt,
        
        /// <summary>
        /// Indicates that the item represents a noun.
        /// </summary>
        Substantiv,

        /// <summary>
        /// Indicates that the item represents a verb.
        /// </summary>
        Verbum,

        /// <summary>
        /// Indicates that the item represents a name.
        /// </summary>
        Proprium

    }

}