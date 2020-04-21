using System.Collections.Generic;
using Skybrud.Umbraco.Search.Models.Options;

namespace Skybrud.TextAnalysis.Search {

    public abstract class ListBase {

        public abstract string Operator { get; }

        public List<object> Query { get; set; }

        public int Count => Query.Count;

        public string Name { get; set; }

        protected ListBase() {
            Query = new List<object>();
        }

        public void Append(string item) {
            if (item.Length == 2 && item[1] == '\'') return;
            Query.Add(item);
        }

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

    }

}