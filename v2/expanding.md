---
order: 5
---

# Expanding

When performing searches based on user input, the user may misspell one or more words, and then not receive the results they were looking for. To improve the user experience when the user the missells a word, we've implemented a concept called **expanding** - eg. if the user enters **bikr**, we determine the word is spelled incorrectly, and thus searches for suggested words as well - in this case **bike**.

With this package, you can use the `Expand` method in the `HunspellTextAnalyzer` class to expand a search text:

```csharp
@using System.Web.Hosting
@using Skybrud.TextAnalysis.Hunspell
@using Skybrud.TextAnalysis.Hunspell.Expand

@{
   
    // Map the path to the dictionary and affix files
    string dic = HostingEnvironment.MapPath("~/App_Data/Hunspell/en-US.dic");
    string aff = HostingEnvironment.MapPath("~/App_Data/Hunspell/en-US.aff");

    // Load a new text analyzer (Hunspell wrapper)
    HunspellTextAnalyzer analyzer = HunspellTextAnalyzer.CreateFromFiles(dic, aff);
    
    // Expand the search text
    HunspellExpandResult expandResult = analyzer.Expand("bikr");

    // Generate the raw Examine query
    <pre>@expandResult.Query.ToRawQuery(new []{ "nodeName" })</pre>

}
```

Calling the `ToRawQuery` in the example above returns the following Examine query:

<div class="highlight"><pre style="white-space: pre-wrap;">(((nodeName:(bikr bikr*)) OR (nodeName:(bike bike*)) OR (nodeName:(bike bike*))))</pre></div>

According to the dictionary, **bikr** is not a known word, but it might be a correct word - just not known to the dictionary. So we're making sure to search for **bikr** as well as **bike**.

Ideally we shouldn't be searching for **bike** twice, but this is currently a minor issue with our expand implementation. At worst, this might mess a bit with the relevance of the results returned from the search.

## Morphing

In the example with **bikr**, we are basing the search on the misspelled word as well as any suggested words for **bikr**. 

With the Danish dictionary from <a href="https://stavekontrolden.dk/" target="_blank" rel="noopener"><strong>Stavekontrolden</strong></a>, our implementation supports [**morphing**](./../morphing/) - meaning that for each word in the search text, we can find it's stem word, and then get all the inflections of the stem word.

Since the Danish affix file is structured a bit different than the other affix files I've been able to test with, morphing unfortunately isn't supported for other languages (eg. `en-US` or `en-GB`).

```csharp
@using System.Web.Hosting
@using Skybrud.TextAnalysis.Hunspell
@using Skybrud.TextAnalysis.Hunspell.Expand

@{
   
    // Map the path to the dictionary and affix files
    string dic = HostingEnvironment.MapPath("~/App_Data/Hunspell/da-DK.dic");
    string aff = HostingEnvironment.MapPath("~/App_Data/Hunspell/da-DK.aff");

    // Load a new text analyzer (Hunspell wrapper)
    HunspellTextAnalyzer analyzer = HunspellTextAnalyzer.CreateFromFiles(dic, aff);
    
    // Expand the search text
    HunspellExpandResult expandResult = analyzer.Expand("cykel");

    // Generate the raw Examine query
    <pre>@expandResult.Query.ToRawQuery(new []{ "nodeName" })</pre>

}
```

In this example, we're expanding the Danish word **cykel** (which is already a stem word), so the generated Examine query describes an OR based search for the inflections of **cykel**:

<div class="highlight"><pre style="white-space: pre-wrap;">((((nodeName:(cykel cykel*)) OR (nodeName:(cykler cykler*)) OR (nodeName:(cyklers cyklers*)) OR (nodeName:(cyklerne cyklerne*)) OR (nodeName:(cyklernes cyklernes*)) OR (nodeName:(cyklen cyklen*)) OR (nodeName:(cyklens cyklens*)) OR (nodeName:(cykels cykels*)))))</pre></div>