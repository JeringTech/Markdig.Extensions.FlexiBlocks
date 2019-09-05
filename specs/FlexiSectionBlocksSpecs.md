---
blockOptions: "../src/FlexiBlocks/Extensions/FlexiSectionBlocks/FlexiSectionBlockOptions.cs"
extensionOptions: "../src/FlexiBlocks/Extensions/FlexiSectionBlocks/FlexiSectionBlocksExtensionOptions.cs"
---

# FlexiSectionBlocks
A FlexiSectionBlocks is a logical section of a markdown document.   

Markdown articles are typically divided into logical sections by [ATX headings](https://spec.commonmark.org/0.28/#atx-headings). For example:
```
# Indoor Herb Gardens
An introduction..

## Getting Started

### Growing Herbs from Cuttings
Information on growing herbs from cuttings..

## Caring for Herbs

### Watering Herbs
Information on watering herbs..
```
The [HTML spec](https://html.spec.whatwg.org/multipage/sections.html#headings-and-sections) encourages wrapping of 
logical sections in [sectioning content elements](https://html.spec.whatwg.org/multipage/dom.html#sectioning-content-2), like so:

```
<article>
<h1>Indoor Herb Gardens</h1>
<p>An introduction..</p>
<section>
<h2>Getting Started</h2>
<section>
<h3>Growing Herbs from Cuttings</h3>
<p>Information on growing herbs from cuttings..</p>
</section>
</section>
<section>
<h2>Caring for Herbs</h2>
<section>
<h3>Watering</h3>
<p>Information on watering herbs..</p>
</section>
</section>
</article>
```
`FlexiSectionBlocks` facilitates such wrapping, which in turn allows for efficient manipulation of an article's outline (TODO add link to relevant article).

## Usage
```csharp
using Markdig;
using Jering.Markdig.Extensions.FlexiBlocks;

...
var markdownPipelineBuilder = new MarkdownPipelineBuilder();
markdownPipelineBuilder.UseFlexiSectionBlocks(/* Optional extension options */);

MarkdownPipeline markdownPipeline = markdownPipelineBuilder.Build();

string markdown = @"# foo
## bar";
string html = Markdown.ToHtml(markdown, markdownPipeline);
string expectedHtml = @"<section class=""flexi-section flexi-section_level_1 flexi-section_has_link-icon"" id=""foo"">
<header class=""flexi-section__header"">
<h1 class=""flexi-section__heading"">foo</h1>
<button class=""flexi-section__link-button"" title=""Copy link"" aria-label=""Copy link"">
<svg class=""flexi-section__link-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path d=""M0 0h24v24H0z"" fill=""none""/><path d=""M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z""/></svg>
</button>
</header>
<p>Some markdown..</p>
<section class=""flexi-section flexi-section_level_2 flexi-section_has_link-icon"" id=""bar"">
<header class=""flexi-section__header"">
<h2 class=""flexi-section__heading"">bar</h2>
<button class=""flexi-section__link-button"" title=""Copy link"" aria-label=""Copy link"">
<svg class=""flexi-section__link-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path d=""M0 0h24v24H0z"" fill=""none""/><path d=""M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z""/></svg>
</button>
</header>
<p>More markdown..</p>
</section>
</section>";

Assert.Equal(expectedHtml, html)
```

# Basics
In markdown, a FlexiSectionBlock consists of an ATX heading and the content between it and
- The next ATX heading of equal or lower level in the same [sectioning root](https://html.spec.whatwg.org/multipage/sections.html#sectioning-root) or
- The end of the article.

An [ATX heading](https://spec.commonmark.org/0.28/#atx-headings) demarcates the start of a FlexiSectionBlock: 
```````````````````````````````` none
--------------- Markdown ---------------
# Indoor Herb Gardens
An introduction..

## Getting Started

### Growing Herbs from Cuttings
Information on growing herbs from cuttings..

## Caring for Herbs

### Watering Herbs
Information on watering herbs..
--------------- Expected Markup ---------------
<section class="flexi-section flexi-section_level_1 flexi-section_has_link-icon" id="indoor-herb-gardens">
<header class="flexi-section__header">
<h1 class="flexi-section__heading">Indoor Herb Gardens</h1>
<button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
<svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
</button>
</header>
<p>An introduction..</p>
<section class="flexi-section flexi-section_level_2 flexi-section_has_link-icon" id="getting-started">
<header class="flexi-section__header">
<h2 class="flexi-section__heading">Getting Started</h2>
<button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
<svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
</button>
</header>
<section class="flexi-section flexi-section_level_3 flexi-section_has_link-icon" id="growing-herbs-from-cuttings">
<header class="flexi-section__header">
<h3 class="flexi-section__heading">Growing Herbs from Cuttings</h3>
<button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
<svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
</button>
</header>
<p>Information on growing herbs from cuttings..</p>
</section>
</section>
<section class="flexi-section flexi-section_level_2 flexi-section_has_link-icon" id="caring-for-herbs">
<header class="flexi-section__header">
<h2 class="flexi-section__heading">Caring for Herbs</h2>
<button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
<svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
</button>
</header>
<section class="flexi-section flexi-section_level_3 flexi-section_has_link-icon" id="watering-herbs">
<header class="flexi-section__header">
<h3 class="flexi-section__heading">Watering Herbs</h3>
<button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
<svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
</button>
</header>
<p>Information on watering herbs..</p>
</section>
</section>
</section>
````````````````````````````````

! By default, a FlexiSectionBlock has a header with a heading and a link button. These elements, along with a FlexiSectionBlock's root element, are assigned default classes. Default classes comply with 
! [BEM methodology](https://en.bem.info/).  
!
! By default, a FlexiSectionBlock is also assigned an ID generated from its heading content.  
!
! FlexiSectionBlocks can be customized, we explain how in [options].

## Options
### `FlexiSectionBlockOptions`
Options for a FlexiSectionBlock. To specify `FlexiSectionBlockOptions` for a FlexiSectionBlock, the [Options](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/OptionsBlocksSpecs.md#options) extension must be enabled.

#### Properties

##### `BlockName`
- Type: `string`
- Description: The `FlexiSectionBlock`'s [BEM block name](https://en.bem.info/methodology/naming-convention/#block-name).
  In compliance with [BEM methodology](https://en.bem.info), this value is the `FlexiSectionBlock`'s root element's class as well as the prefix for all other classes in the block.
  This value should contain only valid [CSS class characters](https://www.w3.org/TR/CSS21/syndata.html#characters).
  If this value is `null`, whitespace or an empty string, the `FlexiSectionBlock`'s block name is "flexi-section".
- Default: "flexi-section"
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  @{ "blockName": "section" }
  ## foo
  --------------- Expected Markup ---------------
  <section class="section section_level_2 section_has_link-icon" id="foo">
  <header class="section__header">
  <h2 class="section__heading">foo</h2>
  <button class="section__link-button" title="Copy link" aria-label="Copy link">
  <svg class="section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
  </button>
  </header>
  </section>
  ````````````````````````````````

##### `Element`
- Type: `SectioningContentElement`
- Description: The `FlexiSectionBlock`'s root element's type.
  The element must be a [sectioning content](https://html.spec.whatwg.org/#sectioning-content) element.
- Default: `SectioningContentElement.Section`
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  @{ "element": "nav" }
  ## foo
  --------------- Expected Markup ---------------
  <nav class="flexi-section flexi-section_level_2 flexi-section_has_link-icon" id="foo">
  <header class="flexi-section__header">
  <h2 class="flexi-section__heading">foo</h2>
  <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
  <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
  </button>
  </header>
  </nav>
  ````````````````````````````````

##### `GenerateID`
- Type: `bool`
- Description: The value specifying whether to generate an ID for the `FlexiSectionBlock`.
  The generated ID is assigned to the `FlexiSectionBlock`'s root element.
  The generated ID is the `FlexiSectionBlock`'s heading content in kebab-case (lowercase words joined by dashes).
  For example, if the heading content is "Foo Bar Baz", the generated ID is "foo-bar-baz".
  If the generated ID is a duplicate of another `FlexiSectionBlock`'s ID, "-<duplicate index>" is appended.
  For example, the second `FlexiSectionBlock` with heading content "Foo Bar Baz" will have ID "foo-bar-baz-1".
  The generated ID precedence over any ID specified in `Attributes`.
- Default: true
- Examples:
  This value is true by default:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  ## Foo Bar Baz
  --------------- Expected Markup ---------------
  <section class="flexi-section flexi-section_level_2 flexi-section_has_link-icon" id="foo-bar-baz">
  <header class="flexi-section__header">
  <h2 class="flexi-section__heading">Foo Bar Baz</h2>
  <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
  <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
  </button>
  </header>
  </section>
  ````````````````````````````````
  "-<duplicate index>" is appended if the generated ID is a duplicate:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  ## foo
  ### `foo`
  ## foo 1
  --------------- Expected Markup ---------------
  <section class="flexi-section flexi-section_level_2 flexi-section_has_link-icon" id="foo">
  <header class="flexi-section__header">
  <h2 class="flexi-section__heading">foo</h2>
  <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
  <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
  </button>
  </header>
  <section class="flexi-section flexi-section_level_3 flexi-section_has_link-icon" id="foo-1">
  <header class="flexi-section__header">
  <h3 class="flexi-section__heading"><code>foo</code></h3>
  <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
  <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
  </button>
  </header>
  </section>
  </section>
  <section class="flexi-section flexi-section_level_2 flexi-section_has_link-icon" id="foo-1-1">
  <header class="flexi-section__header">
  <h2 class="flexi-section__heading">foo 1</h2>
  <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
  <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
  </button>
  </header>
  </section>
  ````````````````````````````````
  If this value is false, no ID is generated:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  @{ "generateID": false }
  ## Foo Bar Baz
  --------------- Expected Markup ---------------
  <section class="flexi-section flexi-section_level_2 flexi-section_has_link-icon">
  <header class="flexi-section__header">
  <h2 class="flexi-section__heading">Foo Bar Baz</h2>
  <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
  <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
  </button>
  </header>
  </section>
  ````````````````````````````````
  The generated ID takes precedence over any ID in `Attributes`:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  @{ 
      "attributes": {
        "id" : "my-custom-id"
      }
  }
  ## Foo Bar Baz
  --------------- Expected Markup ---------------
  <section class="flexi-section flexi-section_level_2 flexi-section_has_link-icon" id="foo-bar-baz">
  <header class="flexi-section__header">
  <h2 class="flexi-section__heading">Foo Bar Baz</h2>
  <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
  <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
  </button>
  </header>
  </section>
  ````````````````````````````````

##### `LinkIcon`
- Type: `string`
- Description: The `FlexiSectionBlock`'s link icon as an HTML fragment.
  A class attribute with value "<`BlockName`>__link-icon" is added to this fragment's first start tag.
  If this value is `null`, whitespace or an empty string, no link icon is rendered.
- Default: the [Material Design link icon](https://material.io/tools/icons/?icon=file_copy&style=baseline)
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  @{
      "linkIcon": "<svg><use xlink:href=\"#material-design-link\"/></svg>"
  }
  ## foo
  --------------- Expected Markup ---------------
  <section class="flexi-section flexi-section_level_2 flexi-section_has_link-icon" id="foo">
  <header class="flexi-section__header">
  <h2 class="flexi-section__heading">foo</h2>
  <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
  <svg class="flexi-section__link-icon"><use xlink:href="#material-design-link"/></svg>
  </button>
  </header>
  </section>
  ````````````````````````````````
  No link icon is are rendered if this value is `null`, whitespace or an empty string:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  @{ "linkIcon": null }
  # foo
  --------------- Expected Markup ---------------
  <section class="flexi-section flexi-section_level_1 flexi-section_no_link-icon" id="foo">
  <header class="flexi-section__header">
  <h1 class="flexi-section__heading">foo</h1>
  <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
  </button>
  </header>
  </section>
  ````````````````````````````````

##### `ReferenceLinkable`
- Type: `bool`
- Description: The value specifying whether the `FlexiSectionBlock` is [reference-linkable](https://spec.commonmark.org/0.28/#reference-link).
  If this value and `GenerateID` are both true, the `FlexiSectionBlock` is reference-linkable.
  Otherwise, it isn't.
  If a `FlexiSectionBlock` is reference-linkable, its [link label](https://spec.commonmark.org/0.28/#link-label) content
  is its heading content. For example, "## Foo Bar Baz" can be linked to using "[Foo Bar Baz]".
  If a `FlexiSectionBlock`'s ID has "-<duplicate index>" appended (see `GenerateID`),
  you can link to it using "<heading content> <duplicate index>".
  For example, the second "## Foo Bar baz" can be linked to using "[Foo Bar Baz 1]".
- Default: true
- Examples:
  This value is true by default:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  [foo]

  ## foo

  [foo]
  [Link Text][foo]
  --------------- Expected Markup ---------------
  <p><a href="#foo">foo</a></p>
  <section class="flexi-section flexi-section_level_2 flexi-section_has_link-icon" id="foo">
  <header class="flexi-section__header">
  <h2 class="flexi-section__heading">foo</h2>
  <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
  <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
  </button>
  </header>
  <p><a href="#foo">foo</a>
  <a href="#foo">Link Text</a></p>
  </section>
  ````````````````````````````````
  If this value is false, the `FlexiSectionBlock` is not reference-linkable:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  [foo]

  @{ "referenceLinkable": false }
  ## foo

  [foo]
  [Link Text][foo]
  --------------- Expected Markup ---------------
  <p>[foo]</p>
  <section class="flexi-section flexi-section_level_2 flexi-section_has_link-icon">
  <header class="flexi-section__header">
  <h2 class="flexi-section__heading">foo</h2>
  <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
  <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
  </button>
  </header>
  <p>[foo]
  [Link Text][foo]</p>
  </section>
  ````````````````````````````````
  If `GenerateID` is false, the `FlexiSectionBlock` is not reference-linkable:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  [foo]

  @{ "generateID": false }
  ## foo

  [foo]
  [Link Text][foo]
  --------------- Expected Markup ---------------
  <p>[foo]</p>
  <section class="flexi-section flexi-section_level_2 flexi-section_has_link-icon">
  <header class="flexi-section__header">
  <h2 class="flexi-section__heading">foo</h2>
  <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
  <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
  </button>
  </header>
  <p>[foo]
  [Link Text][foo]</p>
  </section>
  ````````````````````````````````
  If the `FlexiSectionBlock`'s generated ID has "-<duplicate index>" appended (see `GenerateID`), 
  you can link to it using "<heading content> <duplicate index>":
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  ## Rosemary
  ### Watering
  Rosemary watering needs..

  ## Lemon Balm
  ### Watering
  Lemon Balm watering needs..

  ## Peppermint
  ### Watering
  Similar to [Lemon Balm watering needs][watering 1]...
  --------------- Expected Markup ---------------
  <section class="flexi-section flexi-section_level_2 flexi-section_has_link-icon" id="rosemary">
  <header class="flexi-section__header">
  <h2 class="flexi-section__heading">Rosemary</h2>
  <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
  <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
  </button>
  </header>
  <section class="flexi-section flexi-section_level_3 flexi-section_has_link-icon" id="watering">
  <header class="flexi-section__header">
  <h3 class="flexi-section__heading">Watering</h3>
  <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
  <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
  </button>
  </header>
  <p>Rosemary watering needs..</p>
  </section>
  </section>
  <section class="flexi-section flexi-section_level_2 flexi-section_has_link-icon" id="lemon-balm">
  <header class="flexi-section__header">
  <h2 class="flexi-section__heading">Lemon Balm</h2>
  <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
  <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
  </button>
  </header>
  <section class="flexi-section flexi-section_level_3 flexi-section_has_link-icon" id="watering-1">
  <header class="flexi-section__header">
  <h3 class="flexi-section__heading">Watering</h3>
  <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
  <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
  </button>
  </header>
  <p>Lemon Balm watering needs..</p>
  </section>
  </section>
  <section class="flexi-section flexi-section_level_2 flexi-section_has_link-icon" id="peppermint">
  <header class="flexi-section__header">
  <h2 class="flexi-section__heading">Peppermint</h2>
  <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
  <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
  </button>
  </header>
  <section class="flexi-section flexi-section_level_3 flexi-section_has_link-icon" id="watering-2">
  <header class="flexi-section__header">
  <h3 class="flexi-section__heading">Watering</h3>
  <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
  <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
  </button>
  </header>
  <p>Similar to <a href="#watering-1">Lemon Balm watering needs</a>...</p>
  </section>
  </section>
  ````````````````````````````````

##### `RenderingMode`
- Type: `FlexiSectionBlockRenderingMode`
- Description: The `FlexiSectionBlock`'s rendering mode.
- Default: `FlexiSectionBlockRenderingMode.Standard`
- Examples:
  This value is `FlexiSectionBlockRenderingMode.Standard` by default:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  ## foo
  --------------- Expected Markup ---------------
  <section class="flexi-section flexi-section_level_2 flexi-section_has_link-icon" id="foo">
  <header class="flexi-section__header">
  <h2 class="flexi-section__heading">foo</h2>
  <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
  <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
  </button>
  </header>
  </section>
  ````````````````````````````````
  If this value is `FlexiSectionBlockRenderingMode.Classic`, the `FlexiSectionBlock` is rendered the same way [ATX headings](https://spec.commonmark.org/0.28/#atx-headings) are 
  rendered in CommonMark Spec examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  @{ "renderingMode": "classic" }
  ## foo
  --------------- Expected Markup ---------------
  <h2>foo</h2>
  ````````````````````````````````

##### `Attributes`
- Type: `IDictionary<string, string>`
- Description: The HTML attributes for the `FlexiSectionBlock`'s root element.
  Attribute names must be lowercase.
  If classes are specified, they are appended to default classes. This facilitates [BEM mixes](https://en.bem.info/methodology/quick-start/#mix).
  If the `FlexiSectionBlock` has a generated ID, it takes precedence over any ID in this value.
  If this value is `null`, default classes are still assigned to the root element.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  @{
      "attributes": {
          "id" : "section-1",
          "class" : "block"
      },
      "generateID": false
  }
  ## foo
  --------------- Expected Markup ---------------
  <section class="flexi-section flexi-section_level_2 flexi-section_has_link-icon block" id="section-1">
  <header class="flexi-section__header">
  <h2 class="flexi-section__heading">foo</h2>
  <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
  <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
  </button>
  </header>
  </section>
  ````````````````````````````````

### `FlexiSectionBlocksExtensionOptions`
Options for the FlexiSectionBlocks extension. There are two ways to specify these options:
- Pass a `FlexiSectionBlocksExtensionOptions` when calling `MarkdownPipelineBuilderExtensions.UseFlexiSectionBlocks(this MarkdownPipelineBuilder pipelineBuilder, IFlexiSectionBlocksExtensionOptions options)`.
- Insert a `FlexiSectionBlocksExtensionOptions` into a `MarkdownParserContext.Properties` with key `typeof(IFlexiSectionBlocksExtensionOptions)`. Pass the `MarkdownParserContext` when you call a markdown processing method
  like `Markdown.ToHtml(markdown, stringWriter, markdownPipeline, yourMarkdownParserContext)`.  
  This method allows for different extension options when reusing a pipeline. Options specified using this method take precedence.

#### Constructor Parameters

##### `defaultBlockOptions`
- Type: `IFlexiSectionBlockOptions`
- Description: Default `IFlexiSectionBlockOptions` for all `FlexiSectionBlock`s.
  If this value is `null`, a `FlexiSectionBlockOptions` with default values is used.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extension Options ---------------
  {
      "flexiSectionBlocks": {
          "defaultBlockOptions": {
              "blockName": "section",
              "element": "nav",
              "generateID": false,
              "linkIcon": "<svg><use xlink:href=\"#material-design-link\"/></svg>",
              "attributes": {
                  "class": "block"
              }
          }
      }
  }
  --------------- Markdown ---------------
  # foo
  ## bar

  [foo]
  [bar]
  --------------- Expected Markup ---------------
  <nav class="section section_level_1 section_has_link-icon block">
  <header class="section__header">
  <h1 class="section__heading">foo</h1>
  <button class="section__link-button" title="Copy link" aria-label="Copy link">
  <svg class="section__link-icon"><use xlink:href="#material-design-link"/></svg>
  </button>
  </header>
  <nav class="section section_level_2 section_has_link-icon block">
  <header class="section__header">
  <h2 class="section__heading">bar</h2>
  <button class="section__link-button" title="Copy link" aria-label="Copy link">
  <svg class="section__link-icon"><use xlink:href="#material-design-link"/></svg>
  </button>
  </header>
  <p>[foo]
  [bar]</p>
  </nav>
  </nav>
  ````````````````````````````````
  Default `FlexiSectionBlockOptions` have lower precedence than block specific options:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Extension Options ---------------
  {
      "flexiSectionBlocks": {
          "defaultBlockOptions": {
              "element": "nav"
          }
      }
  }
  --------------- Markdown ---------------
  @{
      "element": "article"
  }
  # foo
  ## bar
  --------------- Expected Markup ---------------
  <article class="flexi-section flexi-section_level_1 flexi-section_has_link-icon" id="foo">
  <header class="flexi-section__header">
  <h1 class="flexi-section__heading">foo</h1>
  <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
  <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
  </button>
  </header>
  <nav class="flexi-section flexi-section_level_2 flexi-section_has_link-icon" id="bar">
  <header class="flexi-section__header">
  <h2 class="flexi-section__heading">bar</h2>
  <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
  <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
  </button>
  </header>
  </nav>
  </article>
  ````````````````````````````````

## Mechanics
As per the [HTML spec](https://html.spec.whatwg.org/multipage/sections.html#headings-and-sections), [sectioning roots](https://html.spec.whatwg.org/multipage/sections.html#sectioning-root)
have their own logical-section trees. In the following example, the level 1 ATX heading in the blockquote does not cause its preceding FlexiSectionBlock to close. Instead, it starts
a new logical-section tree within the blockquote:

```````````````````````````````` none
--------------- Markdown ---------------
# foo

> # foo
> ## foo

## foo
--------------- Expected Markup ---------------
<section class="flexi-section flexi-section_level_1 flexi-section_has_link-icon" id="foo">
<header class="flexi-section__header">
<h1 class="flexi-section__heading">foo</h1>
<button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
<svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
</button>
</header>
<blockquote>
<section class="flexi-section flexi-section_level_1 flexi-section_has_link-icon" id="foo-1">
<header class="flexi-section__header">
<h1 class="flexi-section__heading">foo</h1>
<button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
<svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
</button>
</header>
<section class="flexi-section flexi-section_level_2 flexi-section_has_link-icon" id="foo-2">
<header class="flexi-section__header">
<h2 class="flexi-section__heading">foo</h2>
<button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
<svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
</button>
</header>
</section>
</section>
</blockquote>
<section class="flexi-section flexi-section_level_2 flexi-section_has_link-icon" id="foo-3">
<header class="flexi-section__header">
<h2 class="flexi-section__heading">foo</h2>
<button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
<svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
</button>
</header>
</section>
</section>
````````````````````````````````