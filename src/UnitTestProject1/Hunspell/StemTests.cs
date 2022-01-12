using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skybrud.TextAnalysis.Hunspell;
using Skybrud.TextAnalysis.Hunspell.Stem;

namespace UnitTestProject1.Hunspell  {
    
    [TestClass]
    public class StemTests {
        
        [TestMethod]
        public void Hesten_Stem() {

            HunspellTextAnalyzer analyzer = UnitTestHelper.GetDanishTextAnalyzer();

            HunspellStemResult[] result = analyzer.Stem("hesten");

            Assert.AreEqual(1, result.Length);
            
            Assert.AreEqual("hest", result[0].Value);
            Assert.AreEqual("hest", result[0].Stem);
            Assert.AreEqual("", result[0].Prefix);

        }
        
        [TestMethod]
        public void Webredaktør_Stem() {

            HunspellTextAnalyzer analyzer = UnitTestHelper.GetDanishTextAnalyzer();

            HunspellStemResult[] result = analyzer.Stem("webredaktør");

            Assert.AreEqual(1, result.Length);
            
            Assert.AreEqual("webredaktør", result[0].Value);
            Assert.AreEqual("redaktør", result[0].Stem);
            Assert.AreEqual("web", result[0].Prefix);

        }

    }

}