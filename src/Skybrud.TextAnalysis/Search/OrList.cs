namespace Skybrud.TextAnalysis.Search {

    /// <summary>
    /// Class representing an OR based list of nested queries.
    /// </summary>
    public class OrList : ListBase {
        
        /// <inheritdoc />
        public override string Operator => "OR";

        /// <summary>
        /// Initializes a new empty list.
        /// </summary>
        public OrList() { }

        /// <summary>
        /// Initializes a new list based on the specified <paramref name="items"/>.
        /// </summary>
        /// <param name="items">The array of items.</param>
        public OrList(params object[] items) {
            Query.AddRange(items);
        }

    }

}