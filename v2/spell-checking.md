---
order: 1
---

# Spell Checking

By loading an instance of the <code type="Skybrud.TextAnalysis.Hunspell.HunspellTextAnalyzer">HunspellTextAnalyzer</code> class from the dictionary and affix files, the `Spell` method can be used to check whether a given word is spelled correctly - eg. **bike** (correct) and **bikr** (incorrect):

```csharp
@using System.Web.Hosting
@using Skybrud.TextAnalysis.Hunspell

@{
   
    // Map the path to the dictionary and affix files
    string dic = HostingEnvironment.MapPath("~/App_Data/Hunspell/en-US.dic");
    string aff = HostingEnvironment.MapPath("~/App_Data/Hunspell/en-US.aff");

    // Load a new text analyzer (Hunspell wrapper)
    HunspellTextAnalyzer analyzer = HunspellTextAnalyzer.CreateFromFiles(dic, aff);

    // Is "bike" spelled correctly?
    bool spell1 = analyzer.Spell("bike");
    <pre>@spell1</pre>

    // Is "bikr" spelled correctly?
    bool spell2 = analyzer.Spell("bikr");
    <pre>@spell2</pre>

}
```

Loading different dictionaries determine whether a given word is spelled correctly, according to the language/culture a specific dictionary represents. Even loading either `en-US.dic` or `en-GB.dic` can change whether **color** and **colour** are spelled correctly.