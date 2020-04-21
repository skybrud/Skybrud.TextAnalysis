using System.Collections.Generic;
using Skybrud.Umbraco.Search.Models.Options;

namespace Skybrud.TextAnalysis.Search {

    /// <summary>
    /// Class representing a basic list of nested queries.
    /// </summary>
    public abstract class ListBase {

        #region Properties

        /// <summary>
        /// Gets the operator of the query for this list.
        /// </summary>
        public abstract string Operator { get; }

        /// <summary>
        /// Gets a reference the the list of the query.
        /// </summary>
        public List<object> Query { get; set; }

        /// <summary>
        /// Gets the amount of items in the list.
        /// </summary>
        public int Count => Query.Count;

        /// <summary>
        /// Gets the name of the list (may be used for debugging).
        /// </summary>
        public string Name { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new list.
        /// </summary>
        protected ListBase() {
            Query = new List<object>();
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Appends the specified string <paramref name="query"/>.
        /// </summary>
        /// <param name="query">The string query to be appended.</param>
        public virtual void Append(string query) {
            if (query.Length == 2 && query[1] == '\'') return;
            Query.Add(query);
        }

        /// <summary>
        /// Returns the raw query for the specified array of <paramref name="fields"/>.
        /// </summary>
        /// <param name="fields">The array fields.</param>
        /// <returns>A string representing the raw query.</returns>
        public string ToRawQuery(string[] fields) {

            List<string> temp = new List<string>();

            foreach (object item in Query) {

                if (item is string str) {
                    List<string> temp2 = new List<string>();
                    foreach (var field in fields) {
                        temp2.Add(field + ":" + str);
                    }
                    temp.Add("(" + string.Join(" OR ", temp2) + ")");
                }

                if (item is ListBase list) {
                    temp.Add(list.ToRawQuery(fields));
                }

            }

            return "(" + string.Join(" " + Operator + " ", temp) + ")";

        }

        /// <summary>
        /// Returns the raw query for the specified list of <paramref name="fields"/>.
        /// </summary>
        /// <param name="fields">The list fields.</param>
        /// <returns>A string representing the raw query.</returns>
        public string ToRawQuery(FieldList fields) {

            List<string> temp = new List<string>();

            foreach (object item in Query) {

                if (item is string str) {

                    List<string> temp2 = new List<string>();

                    foreach (Field field in fields) {

                        if (field.Boost != null) {
                            temp2.Add($"{field.FieldName}:({str} {str}*)^{field.Boost}");
                        }

                        if (field.Fuzz != null) {
                            temp2.Add($"{field.FieldName}:{str}~{field.Fuzz}");
                        }

                        temp2.Add(field.FieldName + ":" + str);

                    }

                    temp.Add("(" + string.Join(" OR ", temp2) + ")");

                }

                if (item is ListBase list) {
                    temp.Add(list.ToRawQuery(fields));
                }

            }

            return "(" + string.Join(" " + Operator + " ", temp) + ")";

        }

        #endregion

    }

}