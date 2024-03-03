---
order: 3
---

# Stemming

A given word is either a stem word it self or an inflection of a stem word. Eg. **bikes** is an inflection of **bike**, and **car** is the stem word of **cars**. In this context, *stemming* is getting the stem word from an inflected word.

The underlying **WeCantSpell.Hunspell** package doesn't support stemming directly, but by putting the right pieces together, the `Stem` method in the `HunspellTextAnalyzer` from this package can be used to get the stem word.

```csharp
@using System.Web.Hosting
@using Skybrud.TextAnalysis.Hunspell
@using Skybrud.TextAnalysis.Hunspell.Stem

@{
   
    // Map the path to the dictionary and affix files
    string dic = HostingEnvironment.MapPath("~/App_Data/Hunspell/en-US.dic");
    string aff = HostingEnvironment.MapPath("~/App_Data/Hunspell/en-US.aff");

    // Load a new text analyzer (Hunspell wrapper)
    HunspellTextAnalyzer analyzer = HunspellTextAnalyzer.CreateFromFiles(dic, aff);

    // Get the stem words of "bikes" (underlying package only ever returns one stem)
    HunspellStemResult[] stems = analyzer.Stem("bikes");

}
```

In this example, the `Stem` method returns an array with the stem word **bike** as the only item.

Notice that the underlying **WeCantSpell.Hunspell** package doesn't support returning multiple stem words (opposed to the older **NHunspell** package used in **`v1.x`** of this package).

## Compound Words

In some languages (eg. Danish), compound words are spelled as one word (without any separators). Eg. **summer house** (mix of *summer* and *house*) is spelled **sommerhus** (mix of *sommer* and *hus*) in Danish.

This gives a few problems with the implementation in the **WeCantSpell.Hunspell** package, so the `HunspellStemResult` class is something we've build on top of their implementation.

To add better support for working with compound words in these languages, the `HunspellStemResult` class exposes the `Stem` and `Prefix` properties, as well as the `Value` property, which is a mix of the `Prefix` and `Stem` properties. This is in particular useful for [**morph operations**](./../morphing/).

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

    // Get the stem words of "webredaktører" (underlying package only ever returns one stem)
    HunspellStemResult[] stems = analyzer.Stem("webredaktører");

    foreach (HunspellStemResult stem in stems) {

        <pre>Value: @stem.Value</pre>
        <pre>Stem: @stem.Stem</pre>
        <pre>Prefix: @stem.Prefix</pre>
        <br />

    }

}
```

In this example, the Danish compound word **webredaktører** will be split into **web** (`Prefix` property) and **redaktør** (`Stem` property), which then combined is **webredaktør** (`Value` property). [**Morph operations**](./../morphing/) will only be based on the stem, but then the prefix is automatically prepended to each inflection returned by the morph operation, ensuring the final result is still correct for compounded words.