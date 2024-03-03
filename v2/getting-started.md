---
order: 0
---

# Getting Started

Hunspell revolves around dictionary files (`.dic`) and affix files (`.aff`). The dictionary contains all the words of a given language, with references to rules indicating how the words may be inflected. The affix then contains all the rules, so the `.dic` and `.aff` files go hand in hand.

## Where to find a dictionary?

This package doesn't contain the dictionaries needed for the package to work, so you'll instead need to find these elsewhere. 

The [**WeCantSpell.Hunspell**](https://www.nuget.org/packages/WeCantSpell.Hunspell/), which is used internally in this package, refers to the [**titoBouzout/Dictionaries** GitHub repository](https://github.com/titoBouzout/Dictionaries). Here you'll be able to find dictionaries for many different languages.

However most of the dictionaries are several years old, so you may be able to find newer and more updated Hunspell dictionaries from other sources. For instance, the Danish dictionary in this repository is by the group <a href="https://stavekontrolden.dk/" target="_blank" rel="noopener"><strong>Stavekontrolden</strong></a>. So instead of using a six year old dictionary, you can grab the most recent version from their website instead.

## Loading a dictionary

This package is a wrapper for the [**WeCantSpell.Hunspell**](https://www.nuget.org/packages/WeCantSpell.Hunspell/) package, and builds on top of their implementation of Hunspell.

Our wrapper is represented by the <code type="Skybrud.TextAnalysis.Hunspell.HunspellTextAnalyzer">HunspellTextAnalyzer</code> class. If you have the `.dic` and `.aff` files on disk, you may load it as shown in the example below:

```csharp
@using System.Web.Hosting
@using Skybrud.TextAnalysis.Hunspell

@{
   
    // Map the path to the dictionary and affix files
    string dic = HostingEnvironment.MapPath("~/App_Data/Hunspell/da-DK.dic");
    string aff = HostingEnvironment.MapPath("~/App_Data/Hunspell/da-DK.aff");

    // Load a new text analyzer (Hunspell wrapper)
    HunspellTextAnalyzer analyzer = HunspellTextAnalyzer.CreateFromFiles(dic, aff);

}
```

## Websites and Multi Lingual

This package targets .NET Standard, allowing it be used used in a number different applications and scenarios. We build this package to improve the user experience of text based search to be used in either ASP.NET or ASP.NET Core.

Loading a dictionary takes a bit of tike - not much, but enough to matter if you load the dictionary over and over again for each request. So in a web based context, it may be recommended to save the a loaded <code type="Skybrud.TextAnalysis.Hunspell.HunspellTextAnalyzer">HunspellTextAnalyzer</code> instance either for a duration of time or for the duration of the application. On the other hand, this may use a bit more memory. I don't have any exact numbers, but this is usually a price we are happy to pay for faster access to the Hunspell dictionaries.

A given site or web application may also use more than one language, so the `HunspellRepository` shown below illustrates a way to load and access dictionaries based on a given culture.

If, for instance, the `HunspellRepository` is hooked up with dependency injection, you can control the lifetime of instances of this class. Eg. something like `services.AddSingleton<HunspellRepository>()` will ensure dictionaries stay loaded from the first time they're requested and until the application is shutdown.

```csharp
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.Hosting;
using Skybrud.TextAnalysis.Hunspell;

namespace HunspellTests {
    
    /// <summary>
    /// Class representing a repository for loading and accessing culture specific instances of
    /// <see cref="HunspellTextAnalyzer"/>.
    /// </summary>
    public class HunspellRepository {
        
        private readonly Dictionary<string, HunspellTextAnalyzer> _analyzers;

        /// <summary>
        /// Initializes a new repository.
        /// </summary>
        public HunspellRepository() {
            _analyzers = new Dictionary<string, HunspellTextAnalyzer>();
        }
        
        /// <summary>
        /// Returns the <see cref="HunspellTextAnalyzer"/> for the current culture, or <c>null</c> if unable to load a
        /// new text analyzer.
        /// </summary>
        /// <returns>An instance of <see cref="HunspellTextAnalyzer"/> if successful; otherwise, <c>false</c>.</returns>
        public HunspellTextAnalyzer GetAnalyzer() {
            return GetAnalyzer(CultureInfo.CurrentCulture);
        }
        
        /// <summary>
        /// Returns the <see cref="HunspellTextAnalyzer"/> for the specified <paramref name="cultureInfo"/>, or
        /// <c>null</c> if unable to load a new text analyzer.
        /// </summary>
        /// <param name="cultureInfo">The culture info.</param>
        /// <returns>An instance of <see cref="HunspellTextAnalyzer"/> if successful; otherwise, <c>false</c>.</returns>
        public HunspellTextAnalyzer GetAnalyzer(CultureInfo cultureInfo) {
            
            // Base the file name on the culture name 
            string filename = cultureInfo.Name;

            // Have we already loaded the analyzer for "culture"?
            if (_analyzers.TryGetValue(filename, out HunspellTextAnalyzer analyzer)) return analyzer;

            // Map the path to the Hunspell directory
            string dir = HostingEnvironment
                .MapPath("~/App_Data/Hunspell");

            // Map the paths to the dictionary and affix files
            string dicPath = $"{dir}/{filename}.dic";
            string affPath = $"{dir}/{filename}.aff";

            // Return null if neither file exists
            if (!File.Exists(dicPath)) return null;
            if (!File.Exists(affPath)) return null;

            // Initialize a new analyzer
            analyzer = HunspellTextAnalyzer
                .CreateFromFiles(dicPath, affPath);

            // Append the analyzer to the internal dictionary
            _analyzers.Add(filename, analyzer);

            // Return the analyzer
            return analyzer;

        }

        /// <summary>
        /// Gets the <see cref="HunspellTextAnalyzer"/> for the specified <paramref name="cultureInfo"/>.
        /// </summary>
        /// <param name="cultureInfo">The culture info.</param>
        /// <param name="analyzer">When this method returns, holds the loaded <see cref="HunspellTextAnalyzer"/> if
        /// successful; otherwise, <c>false</c>.</param>
        /// <returns><c>true</c> if successful; otherwise, <c>false</c>.</returns>
        public bool TryGetAnalyzer(CultureInfo cultureInfo, out HunspellTextAnalyzer analyzer) {
            analyzer = GetAnalyzer(cultureInfo);
            return analyzer != null;
        }

    }

}
```

The `HunspellRepository` class is an example how to set this up - the class is not part of this package.


<style>
    .highlight.csharp {
        max-width: 900px;
    }
</style>