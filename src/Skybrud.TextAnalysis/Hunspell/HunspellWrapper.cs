using System;
using Skybrud.TextAnalysis.Hunspell.Affix;
using Skybrud.TextAnalysis.Hunspell.Dictionary;

namespace Skybrud.TextAnalysis.Hunspell {

    /// <summary>
    /// Wrapper class for <see cref="NHunspell.Hunspell"/>, <see cref="HunspellAffix"/> and <see cref="HunspellDictionary"/>.
    /// </summary>
    public class HunspellWrapper : IDisposable {

        #region Properties

        /// <summary>
        /// Gets a reference to the internal <see cref="NHunspell.Hunspell"/>.
        /// </summary>
        public NHunspell.Hunspell Hunspell { get; }

        /// <summary>
        /// Gets a reference to the <see cref="Affix"/> instance.
        /// </summary>
        public HunspellAffix Affix { get; }

        /// <summary>
        /// Gets a reference to the <see cref="Dictionary"/> instance.
        /// </summary>
        public HunspellDictionary Dictionary { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance from the specified <paramref name="affixFile"/> and <paramref name="dicFile"/>.
        /// </summary>
        /// <param name="affixFile">The path to the affix file (<c>.aff</c>).</param>
        /// <param name="dicFile">The path to the dictionary file (<c>.dic</c>).</param>
        public HunspellWrapper(string affixFile, string dicFile) {
            Hunspell = new NHunspell.Hunspell(affixFile, dicFile);
            Affix = HunspellAffix.Load(affixFile);
            Dictionary = HunspellDictionary.Load(dicFile, Affix);
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Disposes the wrapper and the internal <see cref="NHunspell.Hunspell"/> instance.
        /// </summary>
        public void Dispose() {
            Hunspell?.Dispose();
        }

        #endregion

    }

}