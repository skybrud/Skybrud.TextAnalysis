using System.Collections.Generic;
using System.Linq;

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
        public virtual string ToRawQuery(string[] fields) {

            List<string> temp = new List<string>();

            foreach (object item in Query) {
                
                switch (item) {
                    
                    case string str:
                        List<string> temp2 = new List<string>();
                        foreach (string field in fields) {
                            temp2.Add($"{field}:({str} {str}*)");
                        }
                        temp.Add($"({string.Join(" OR ", temp2)})");
                        break;
                    
                    case ListBase list:
                        temp.Add(list.ToRawQuery(fields));
                        break;

                }

            }

            return $"({string.Join($" {Operator} ", temp)})";

        }

        /// <summary>
        /// Returns the raw query for the specified collection of <paramref name="fields"/>.
        /// </summary>
        /// <param name="fields">The collection fields.</param>
        /// <returns>A string representing the raw query.</returns>
        public virtual string ToRawQuery(IEnumerable<Field> fields) {
            return ToRawQuery(fields.ToArray());
        }

        /// <summary>
        /// Returns the raw query for the specified array of <paramref name="fields"/>.
        /// </summary>
        /// <param name="fields">The array fields.</param>
        /// <returns>A string representing the raw query.</returns>
        public virtual string ToRawQuery(Field[] fields) {

            List<string> temp = new List<string>();

            foreach (object item in Query) {
                
                switch (item)  {
                    
                    case string str: {
                        
                        List<string> temp2 = new List<string>();

                        foreach (Field field in fields) {

                            if (field.Boost != null) {
                                temp2.Add($"{field.FieldName}:({str} {str}*)^{field.Boost}");
                            }

                            if (field.Fuzz != null) {
                                temp2.Add($"{field.FieldName}:{str}~{field.Fuzz}");
                            }

                            temp2.Add($"{field.FieldName}:({str} {str}*)");

                        }

                        temp.Add($"({string.Join(" OR ", temp2)})");
                        break;
                    }
                    
                    case ListBase list:
                        temp.Add(list.ToRawQuery(fields));
                        break;

                }

            }

            return $"({string.Join(" " + Operator + " ", temp.Distinct())})";

        }

        #endregion

    }

}