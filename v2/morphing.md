---
order: 4
---

# Morphing

*Morphing* is the concept of transforming a stem word into the variations/inflections of the stem word.

<div class="alert alert-info">
    The <strong>WeCantSpell.Hunspell</strong> package doesn't support morphing - this is something we're adding on top of the <strong>WeCantSpell.Hunspell</strong> in this package.<br />
    <br />
    Unfortunately, our implementation has currently only been confirmed working with the Danish dictionary files provided by <a href="https://stavekontrolden.dk/" target="_blank" rel="noopener"><strong>Stavekontrolden</strong></a>.
</div>

The `Morph` methods needs an instance of `HunspellStemResult` from a successful [**stem operation**](./../stemming/) - eg. as shown in the example below where we're finding the stem words of the Danish **cykel**:


```csharp
@using System.Web.Hosting
@using Skybrud.TextAnalysis.Hunspell
@using Skybrud.TextAnalysis.Hunspell.Stem

@{
   
    // Map the path to the dictionary and affix files
    string dic = HostingEnvironment.MapPath("~/App_Data/Hunspell/da-DK.dic");
    string aff = HostingEnvironment.MapPath("~/App_Data/Hunspell/da-DK.aff");

    // Load a new text analyzer (Hunspell wrapper)
    HunspellTextAnalyzer analyzer = HunspellTextAnalyzer.CreateFromFiles(dic, aff);
    
    // Get the stem words of "cykel" (underlying package only ever returns one stem)
    HunspellStemResult[] stems = analyzer.Stem("cykel");
    
    // Iterate through the stems
    foreach (HunspellStemResult stem in stems) {

        <h3>@stem.Value</h3>

        // Get the inflections of "stem" (through morphing)
        string[] inflections = analyzer.Morph(stem);

        // Iterate through the inflections
        foreach (string inflection in inflections) {
            <pre>@inflection</pre>
        }

    }

}
```

Morphing the word **cykel** returns the following inflections (including the word itself):

- cykel
- cykler
- cyklers
- cyklerne
- cyklernes
- cyklen
- cyklens
- cykels