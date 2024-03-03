---
order: 2
---

# Suggestions

If you already have used the Hunspell dictionary determine whether a given word is spelled incorrectly, you can use the `GetSuggestions` method in the `HunspellTextAnalyzer` class:

```csharp
@using System.Web.Hosting
@using Skybrud.TextAnalysis.Hunspell

@{
   
    // Map the path to the dictionary and affix files
    string dic = HostingEnvironment.MapPath("~/App_Data/Hunspell/en-US.dic");
    string aff = HostingEnvironment.MapPath("~/App_Data/Hunspell/en-US.aff");

    // Load a new text analyzer (Hunspell wrapper)
    HunspellTextAnalyzer analyzer = HunspellTextAnalyzer.CreateFromFiles(dic, aff);

    // Get suggestions for "bikr"
    string[] suggestions = analyzer.Suggest("bikr");

    // Iterate through the suggestions
    foreach (string suggestion in suggestions) {

        <pre>@suggestion</pre>

    }


}
```

The word **bikr** (spelled incorrectly), as used in this example, returns **bike** as the only suggestion. Other words may return multiple suggestions.