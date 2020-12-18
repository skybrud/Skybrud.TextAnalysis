namespace Skybrud.TextAnalysis.Search {

    /// <summary>
    /// Class representin a text field.
    /// </summary>
    public class Field {
        
        #region Properties

        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the boost value of the field.
        /// </summary>
        public int? Boost { get; set; }

        /// <summary>
        /// Gets or sets the fuzzy value of the field.
        /// </summary>
        public float? Fuzz { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new field with the specified <paramref name="fieldName"/>.
        /// </summary>
        /// <param name="fieldName">The name of the field.</param>
        public Field(string fieldName) {
            FieldName = fieldName;
            Boost = null;
            Fuzz = null;
        }

        /// <summary>
        /// Initializes a new field with the specified <paramref name="fieldName"/> and <paramref name="boost"/> value.
        /// </summary>
        /// <param name="fieldName">The name of the field.</param>
        /// <param name="boost">The boost value of the field.</param>
        public Field(string fieldName, int? boost) {
            FieldName = fieldName;
            Boost = boost;
        }

        /// <summary>
        /// Initializes a new field with the specified <paramref name="fieldName"/>, <paramref name="boost"/> and <paramref name="fuzz"/> values.
        /// </summary>
        /// <param name="fieldName">The name of the field.</param>
        /// <param name="boost">The boost value of the field.</param>
        /// <param name="fuzz">The fuzzy value of the field.</param>
        public Field(string fieldName, int? boost, float? fuzz) {
            FieldName = fieldName;
            Boost = boost;
            Fuzz = fuzz;
        }

        #endregion

    }

}