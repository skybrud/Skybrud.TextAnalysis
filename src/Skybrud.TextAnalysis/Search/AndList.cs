namespace Skybrud.TextAnalysis.Search {

    public class AndList : ListBase {

        public override string Operator => "AND";

        public AndList() { }

        public AndList(params object[] items) {
            Query.AddRange(items);
        }

    }

}