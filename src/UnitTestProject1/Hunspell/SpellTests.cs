using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skybrud.TextAnalysis.Hunspell;

namespace UnitTestProject1.Hunspell  {
    
    [TestClass]
    public class SpellTests {
        
        [TestMethod]
        public void børnehave() {
            HunspellTextAnalyzer analyzer = UnitTestHelper.GetDanishTextAnalyzer();
            bool result = analyzer.Spell("børnehave");
            Assert.IsTrue(result);
        }
        
        [TestMethod]
        public void børnehaver() {
            HunspellTextAnalyzer analyzer = UnitTestHelper.GetDanishTextAnalyzer();
            bool result = analyzer.Spell("børnehaver");
            Assert.IsTrue(result);
        }
        
        [TestMethod]
        public void chefredaktør() {
            HunspellTextAnalyzer analyzer = UnitTestHelper.GetDanishTextAnalyzer();
            bool result = analyzer.Spell("chefredaktør");
            Assert.IsTrue(result);
        }
        
        [TestMethod]
        public void webredaktør() {
            HunspellTextAnalyzer analyzer = UnitTestHelper.GetDanishTextAnalyzer();
            bool result = analyzer.Spell("webredaktør");
            Assert.IsTrue(result);
        }
        
        [TestMethod]
        public void webredaktører() {
            HunspellTextAnalyzer analyzer = UnitTestHelper.GetDanishTextAnalyzer();
            bool result = analyzer.Spell("webredaktører");
            Assert.IsTrue(result);
        }
        
        [TestMethod]
        public void somerhus() {
            HunspellTextAnalyzer analyzer = UnitTestHelper.GetDanishTextAnalyzer();
            bool result = analyzer.Spell("somerhus");
            Assert.IsFalse(result);
        }
        
        [TestMethod]
        public void sommerhus() {
            HunspellTextAnalyzer analyzer = UnitTestHelper.GetDanishTextAnalyzer();
            bool result = analyzer.Spell("sommerhus");
            Assert.IsTrue(result);
        }
        
        [TestMethod]
        public void avisuddeler() {
            HunspellTextAnalyzer analyzer = UnitTestHelper.GetDanishTextAnalyzer();
            bool result = analyzer.Spell("avisuddeler");
            Assert.IsTrue(result);
        }
        
        [TestMethod]
        public void avisuddelerens() {
            HunspellTextAnalyzer analyzer = UnitTestHelper.GetDanishTextAnalyzer();
            bool result = analyzer.Spell("avisuddelerens");
            Assert.IsTrue(result);
        }
        
        [TestMethod]
        public void Avisuddelerens2() {
            HunspellTextAnalyzer analyzer = UnitTestHelper.GetDanishTextAnalyzer();
            bool result = analyzer.Spell("Avisuddelerens");
            Assert.IsTrue(result);
        }
        
        [TestMethod]
        public void karen() {
            HunspellTextAnalyzer analyzer = UnitTestHelper.GetDanishTextAnalyzer();
            bool result = analyzer.Spell("karen");
            Assert.IsFalse(result);
        }
        
        [TestMethod]
        public void Karen() {
            HunspellTextAnalyzer analyzer = UnitTestHelper.GetDanishTextAnalyzer();
            bool result = analyzer.Spell("Karen");
            Assert.IsTrue(result);
        }
        
        [TestMethod]
        public void landzonetilladelse() {
            HunspellTextAnalyzer analyzer = UnitTestHelper.GetDanishTextAnalyzer();
            bool result = analyzer.Spell("landzonetilladelse");
            Assert.IsTrue(result);
        }
        
        [TestMethod]
        public void EAN() {
            HunspellTextAnalyzer analyzer = UnitTestHelper.GetDanishTextAnalyzer();
            bool result = analyzer.Spell("EAN");
            Assert.IsTrue(result);
        }
        
        [TestMethod]
        public void ABCDEFG() {
            HunspellTextAnalyzer analyzer = UnitTestHelper.GetDanishTextAnalyzer();
            bool result = analyzer.Spell("ABCDEFG");
            Assert.IsFalse(result);
        }

    }

}