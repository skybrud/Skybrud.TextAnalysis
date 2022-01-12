using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skybrud.TextAnalysis.Hunspell;
using Skybrud.TextAnalysis.Hunspell.Expand;

namespace UnitTestProject1.Hunspell  {
    
    [TestClass]
    public class ExpandTests {
        
        [TestMethod]
        public void karen() {

            // "karen" is spelled incorrectly, as it should be "Karen" instead. This unit tests is to ensure that the
            // analyzer will see that "karen" is incorrect, but pick the suggestion with the same spelling, but with
            // different casing

            HunspellTextAnalyzer analyzer = UnitTestHelper.GetDanishTextAnalyzer();

            HunspellExpandResult result = analyzer.Expand("karen");

            string actual = result.Query.ToRawQuery(new[] {"nodeName"});

            // Due to the incorect spelling, the query gets a bit longer that it should be. Ideally we should fix this
            // so we don't search for the same word twice, as it may affect the score/relevant when sorting the
            // diffeent search results. But for now, this is an acceptable result

            Assert.AreEqual("(((nodeName:(Karen Karen*)) OR (nodeName:(Karen Karen*)) OR (nodeName:(Karens Karens*))))", actual);

        }
        
        [TestMethod]
        public void Karen() {

            HunspellTextAnalyzer analyzer = UnitTestHelper.GetDanishTextAnalyzer();

            HunspellExpandResult result = analyzer.Expand("Karen");

            string actual = result.Query.ToRawQuery(new[] {"nodeName"});

            Assert.AreEqual("((((nodeName:(Karen Karen*)) OR (nodeName:(Karens Karens*)))))", actual);

        }

    }

}