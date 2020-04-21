namespace Skybrud.TextAnalysis.Search {

    public class OrList : ListBase {

        public override string Operator => "OR";

        public OrList() { }

        public OrList(params object[] items) {
            Query.AddRange(items);
        }

    }

}