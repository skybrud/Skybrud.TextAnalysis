namespace Skybrud.TextAnalysis.Search {

    /// <summary>
    /// Class representing an AND based list of nested queries.
    /// </summary>
    public class AndList : ListBase {

        /// <inheritdoc />
        public override string Operator => "AND";

        /// <summary>
        /// Initializes a new empty list.
        /// </summary>
        public AndList() { }

        /// <summary>
        /// Initializes a new list based on the specified <paramref name="items"/>.
        /// </summary>
        /// <param name="items">The array of items.</param>
        public AndList(params object[] items) {
            Query.AddRange(items);
        }

    }

}