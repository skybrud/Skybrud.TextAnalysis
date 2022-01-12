using System.IO;
using Skybrud.TextAnalysis.Hunspell;

namespace UnitTestProject1 {
    
    public static class UnitTestHelper {

        private static HunspellTextAnalyzer _danish;

        public static HunspellTextAnalyzer GetDanishTextAnalyzer() {
            
            if (_danish != null) return _danish;

            string dll = typeof(UnitTestHelper).Assembly.Location;

            string dir = Path.GetDirectoryName(dll);

            string filename = "da_DK-2.6.229";

            _danish = HunspellTextAnalyzer.CreateFromFiles($"{dir}/{filename}.dic", $"{dir}/{filename}.aff");

            return _danish;

        }

    }

}