using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skybrud.TextAnalysis.Hunspell;
using Skybrud.TextAnalysis.Hunspell.Stem;
using Skybrud.Essentials.Strings.Extensions;

namespace UnitTestProject1.Hunspell  {
    
    [TestClass]
    public class MorphTests {
        
        [TestMethod]
        public void Hesten_Stem() {

            HunspellTextAnalyzer analyzer = UnitTestHelper.GetDanishTextAnalyzer();

            HunspellStemResult stem = analyzer.Stem("hesten").FirstOrDefault();

            Assert.IsNotNull(stem);

            string[] morph = analyzer.Morph(stem);

            Assert.AreEqual(8, morph.Length);
            
            Assert.AreEqual("hest", morph[0]);
            Assert.AreEqual("hestes", morph[1]);
            Assert.AreEqual("hestene", morph[2]);
            Assert.AreEqual("heste", morph[3]);
            Assert.AreEqual("hestenes", morph[4]);
            Assert.AreEqual("hesten", morph[5]);
            Assert.AreEqual("hestens", morph[6]);
            Assert.AreEqual("hests", morph[7]);

        }
        
        [TestMethod]
        public void Webredaktør_Stem() {

            HunspellTextAnalyzer analyzer = UnitTestHelper.GetDanishTextAnalyzer();

            HunspellStemResult stem = analyzer.Stem("webredaktør").FirstOrDefault();

            Assert.IsNotNull(stem);

            string[] morph = analyzer.Morph(stem);

            Assert.AreEqual(8, morph.Length);
            
            Assert.AreEqual("webredaktør", morph[0]);
            Assert.AreEqual("webredaktørs", morph[1]);
            Assert.AreEqual("webredaktører", morph[2]);
            Assert.AreEqual("webredaktørerne", morph[3]);
            Assert.AreEqual("webredaktørernes", morph[4]);
            Assert.AreEqual("webredaktørers", morph[5]);
            Assert.AreEqual("webredaktøren", morph[6]);
            Assert.AreEqual("webredaktørens", morph[7]);

        }

    }

}