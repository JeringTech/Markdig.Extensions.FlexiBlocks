---
blockOptions: "../src/FlexiBlocks/Extensions/FlexiFigureBlocks/FlexiFigureBlockOptions.cs"
extensionOptions: "../src/FlexiBlocks/Extensions/FlexiFigureBlocks/FlexiFigureBlocksExtensionOptions.cs"
---

# FlexiFigureBlocks
A FlexiFigureBlock is a figure and its caption. FlexiFigureBlocks are automatically numbered and easy to link to.

## Usage
```csharp
using Markdig;
using Jering.Markdig.Extensions.FlexiBlocks;

...
var markdownPipelineBuilder = new MarkdownPipelineBuilder();
markdownPipelineBuilder.UseFlexiFigureBlocks(/* Optional extension options */);

MarkdownPipeline markdownPipeline = markdownPipelineBuilder.Build();

string markdown = @"+++ figure
This is a figure!
+++
Caption
+++";
string html = Markdown.ToHtml(markdown, markdownPipeline);
string expectedHtml = @"<figure class=""flexi-figure flexi-figure_has-name"" id=""figure-1"">
<div class=""flexi-figure__content"">
<p>This is a figure!</p>
</div>
<figcaption class=""flexi-figure__caption""><span class=""flexi-figure__name"">Figure 1. </span>Caption</figcaption>
</figure>";

Assert.Equal(expectedHtml, html)
```

# Basics
In markdown, a FlexiFigureBlock is a multi-part block with two parts - the first contains the figure, the second its caption:

```````````````````````````````` none
--------------- Markdown ---------------
+++ figure
This is a figure!
+++
Caption
+++
--------------- Expected Markup ---------------
<figure class="flexi-figure flexi-figure_has-name" id="figure-1">
<div class="flexi-figure__content">
<p>This is a figure!</p>
</div>
<figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
</figure>
````````````````````````````````

! Generated elements are assigned classes that comply with [BEM methodology](https://en.bem.info/). These classes can be customized. We explain how in [options].

The figure part can contain markdown blocks such as code blocks, lists and ATX headings. The caption part can only contain inline markdown
such as text with empasis ([Commonmark - Blocks and inlines](https://spec.commonmark.org/0.28/#blocks-and-inlines)):
```````````````````````````````` none
--------------- Extra Extensions ---------------
FlexiCodeBlocks
--------------- Markdown ---------------
+++ figure
```
This is a figure!
```
+++
**Caption**
+++
--------------- Expected Markup ---------------
<figure class="flexi-figure flexi-figure_has-name" id="figure-1">
<div class="flexi-figure__content">
<div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
<header class="flexi-code__header">
<button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
<svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
</button>
</header>
<pre class="flexi-code__pre"><code class="flexi-code__code">This is a figure!
</code></pre>
</div>
</div>
<figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span><strong>Caption</strong></figcaption>
</figure>
````````````````````````````````

FlexiFigureBlocks are automatically numbered according to their positions. The nth figure is assigned the ID "figure-n" and is named
"Figure n". Figure names are rendered at the beginning of captions:

```````````````````````````````` none
--------------- Markdown ---------------
+++ figure
This is the first figure!
+++
Caption
+++

+++ figure
This is the second figure!
+++
Caption
+++

+++ figure
This is the third figure!
+++
Caption
+++
--------------- Expected Markup ---------------
<figure class="flexi-figure flexi-figure_has-name" id="figure-1">
<div class="flexi-figure__content">
<p>This is the first figure!</p>
</div>
<figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
</figure>
<figure class="flexi-figure flexi-figure_has-name" id="figure-2">
<div class="flexi-figure__content">
<p>This is the second figure!</p>
</div>
<figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 2. </span>Caption</figcaption>
</figure>
<figure class="flexi-figure flexi-figure_has-name" id="figure-3">
<div class="flexi-figure__content">
<p>This is the third figure!</p>
</div>
<figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 3. </span>Caption</figcaption>
</figure>
````````````````````````````````

A [reference link definition](https://spec.commonmark.org/0.28/#link-reference-definition) is created for each FlexiFigureBlock. 
The reference link definition created for the nth FlexiFigureBlock is:
```
[Figure n]: #figure-n
```
You can link to the nth FlexiFigureBlock using the [reference link](https://spec.commonmark.org/0.28/#reference-link) "[Figure n]" (not case sensitive):
```````````````````````````````` none
--------------- Markdown ---------------
[figure 1]
[figure 2]

+++ figure
This is the first figure!
+++
Caption
+++

+++ figure
This is the second figure!
+++
Caption
+++

[Figure 1]
[Figure 2]
--------------- Expected Markup ---------------
<p><a href="#figure-1">figure 1</a>
<a href="#figure-2">figure 2</a></p>
<figure class="flexi-figure flexi-figure_has-name" id="figure-1">
<div class="flexi-figure__content">
<p>This is the first figure!</p>
</div>
<figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
</figure>
<figure class="flexi-figure flexi-figure_has-name" id="figure-2">
<div class="flexi-figure__content">
<p>This is the second figure!</p>
</div>
<figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 2. </span>Caption</figcaption>
</figure>
<p><a href="#figure-1">Figure 1</a>
<a href="#figure-2">Figure 2</a></p>
````````````````````````````````

## Options
### `FlexiFigureBlockOptions`
Options for a FlexiFigureBlock. To specify `FlexiFigureBlockOptions` for a FlexiFigureBlock, the [Options](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiOptionsBlocksSpecs.md#options) extension must be enabled.

#### Properties

##### `BlockName`
- Type: `string`
- Description: The `FlexiFigureBlock`'s [BEM block name](https://en.bem.info/methodology/naming-convention/#block-name).
  In compliance with [BEM methodology](https://en.bem.info), this value is the `FlexiFigureBlock`'s root element's class as well as the prefix for all other classes in the block.
  This value should contain only valid [CSS class characters](https://www.w3.org/TR/CSS21/syndata.html#characters).
  If this value is `null`, whitespace or an empty string, the `FlexiFigureBlock`'s block name is "flexi-figure".
- Default: "flexi-figure"
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  o{ "blockName": "figure" }
  +++ figure
  This is a figure!
  +++
  Caption
  +++
  --------------- Expected Markup ---------------
  <figure class="figure figure_has-name" id="figure-1">
  <div class="figure__content">
  <p>This is a figure!</p>
  </div>
  <figcaption class="figure__caption"><span class="figure__name">Figure 1. </span>Caption</figcaption>
  </figure>
  ````````````````````````````````

##### `ReferenceLinkable`
- Type: `bool`
- Description: The value specifying whether the `FlexiFigureBlock` is [reference-linkable](https://spec.commonmark.org/0.28/#reference-link).
  If this value is true and `GenerateID` is true or an ID is specified in `Attributes`,
  the `FlexiFigureBlock` is reference-linkable. Otherwise, it isn't.
  If a `FlexiFigureBlock` is reference-linkable, you can link to it using its name as label content. For example,
  the first `FlexiFigureBlock` in a document can be linked to using "[Figure 1]" or "[figure 1]" (label content is not case sensitive).
- Default: true
- Examples:
  This value is true by default:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  [figure 1]
  [Figure 1]

  +++ figure
  This is a figure!
  +++
  Caption
  +++

  [figure 1]
  [Figure 1]
  --------------- Expected Markup ---------------
  <p><a href="#figure-1">figure 1</a>
  <a href="#figure-1">Figure 1</a></p>
  <figure class="flexi-figure flexi-figure_has-name" id="figure-1">
  <div class="flexi-figure__content">
  <p>This is a figure!</p>
  </div>
  <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
  </figure>
  <p><a href="#figure-1">figure 1</a>
  <a href="#figure-1">Figure 1</a></p>
  ````````````````````````````````
  If this value is false, the `FlexiFigureBlock` is not reference-linkable:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  o{ "referenceLinkable": false }
  +++ figure
  This is a figure!
  +++
  Caption
  +++

  [figure 1]
  --------------- Expected Markup ---------------
  <figure class="flexi-figure flexi-figure_has-name" id="figure-1">
  <div class="flexi-figure__content">
  <p>This is a figure!</p>
  </div>
  <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
  </figure>
  <p>[figure 1]</p>
  ````````````````````````````````
  If `GenerateID` is false and no ID is specified in `Attributes`, the `FlexiFigureBlock` is not reference-linkable:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  o{ "generateID": false }
  +++ figure
  This is a figure!
  +++
  Caption
  +++

  [figure 1]
  --------------- Expected Markup ---------------
  <figure class="flexi-figure flexi-figure_has-name">
  <div class="flexi-figure__content">
  <p>This is a figure!</p>
  </div>
  <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
  </figure>
  <p>[figure 1]</p>
  ````````````````````````````````
  If `GenerateID` is false and an ID is specified in `Attributes`, the `FlexiFigureBlock` is reference-linkable:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  o{ 
      "generateID": false,
      "attributes": { "id": "custom-id" }
  }
  +++ figure
  This is a figure!
  +++
  Caption
  +++

  [figure 1]
  --------------- Expected Markup ---------------
  <figure class="flexi-figure flexi-figure_has-name" id="custom-id">
  <div class="flexi-figure__content">
  <p>This is a figure!</p>
  </div>
  <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
  </figure>
  <p><a href="#custom-id">figure 1</a></p>
  ````````````````````````````````

##### `LinkLabelContent`
- Type: `string`
- Description: The content of the [link label](https://spec.commonmark.org/0.28/#link-label) for linking to the `FlexiFigureBlock`.
  If this value is not `null`, whitespace or an empty string, it is expected in place of the `FlexiFigureBlock`'s name as link label content.
  For example, if this value for the first `FlexiFigureBlock` in a document is "first", you'd link to it using "[first]" instead of "[figure 1]".
  Often, `FlexiFigureBlock` positions in a document aren't fixed. With custom link label content, reference-links to `FlexiFigureBlock`s do not need to be updated every time
  positions change.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  o{"linkLabelContent": "my figure"}
  +++ figure
  This is the first figure!
  +++
  Caption
  +++

  +++ figure
  This is the second figure!
  +++
  Caption
  +++

  [my figure]
  [figure 2]
  --------------- Expected Markup ---------------
  <figure class="flexi-figure flexi-figure_has-name" id="figure-1">
  <div class="flexi-figure__content">
  <p>This is the first figure!</p>
  </div>
  <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
  </figure>
  <figure class="flexi-figure flexi-figure_has-name" id="figure-2">
  <div class="flexi-figure__content">
  <p>This is the second figure!</p>
  </div>
  <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 2. </span>Caption</figcaption>
  </figure>
  <p><a href="#figure-1">Figure 1</a>
  <a href="#figure-2">figure 2</a></p>
  ````````````````````````````````
  Custom link label content allows you to avoid updating reference-links every time positions change. In the following example, we've appended a figure
  to the previous example. We need to update the label "figure 2", but we do not need to update the label "my figure":
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  +++ figure
  This is the new first figure!
  +++
  Caption
  +++

  o{"linkLabelContent": "my figure"}
  +++ figure
  This is the first figure!
  +++
  Caption
  +++

  +++ figure
  This is the second figure!
  +++
  Caption
  +++

  [figure 1]
  [my figure]
  [figure 3]
  --------------- Expected Markup ---------------
  <figure class="flexi-figure flexi-figure_has-name" id="figure-1">
  <div class="flexi-figure__content">
  <p>This is the new first figure!</p>
  </div>
  <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
  </figure>
  <figure class="flexi-figure flexi-figure_has-name" id="figure-2">
  <div class="flexi-figure__content">
  <p>This is the first figure!</p>
  </div>
  <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 2. </span>Caption</figcaption>
  </figure>
  <figure class="flexi-figure flexi-figure_has-name" id="figure-3">
  <div class="flexi-figure__content">
  <p>This is the second figure!</p>
  </div>
  <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 3. </span>Caption</figcaption>
  </figure>
  <p><a href="#figure-1">figure 1</a>
  <a href="#figure-2">Figure 2</a>
  <a href="#figure-3">figure 3</a></p>
  ````````````````````````````````

##### `GenerateID`
- Type: `bool`
- Description: The value specifying whether to generate an ID for the `FlexiFigureBlock`.
  The generated ID is assigned to the `FlexiFigureBlock`'s root element.
  The generated ID is the `FlexiFigureBlock`'s generated name in kebab-case (lowercase words joined by dashes). For example,
  the generated ID of the first `FlexiFigureBlock` in a document is "figure-1".
  Any ID specified in `Attributes` takes precedence over the generated ID.
- Default: true
- Examples:
  This value is true by default:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  +++ figure
  This is a figure!
  +++
  Caption
  +++
  --------------- Expected Markup ---------------
  <figure class="flexi-figure flexi-figure_has-name" id="figure-1">
  <div class="flexi-figure__content">
  <p>This is a figure!</p>
  </div>
  <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
  </figure>
  ````````````````````````````````
  If this value is false, no ID is generated:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  o{ "generateID": false }
  +++ figure
  This is a figure!
  +++
  Caption
  +++
  --------------- Expected Markup ---------------
  <figure class="flexi-figure flexi-figure_has-name">
  <div class="flexi-figure__content">
  <p>This is a figure!</p>
  </div>
  <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
  </figure>
  ````````````````````````````````
  Any ID specified in `Attributes` takes precedence over the generated ID. If you plan on 
  styling the `FlexiFigureBlock`, a custom ID is preferable since it means you don't need
  to update CSS selectors every time `FlexiFigureBlock` positions change:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  o{ 
      "attributes": {
        "id" : "my-custom-id"
      }
  }
  +++ figure
  This is a figure!
  +++
  Caption
  +++
  --------------- Expected Markup ---------------
  <figure class="flexi-figure flexi-figure_has-name" id="my-custom-id">
  <div class="flexi-figure__content">
  <p>This is a figure!</p>
  </div>
  <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
  </figure>
  ````````````````````````````````

##### `RenderName`
- Type: `bool`
- Description: The value specifying whether to render the `FlexiFigureBlock`'s name.
  If true, the `FlexiFigureBlock`'s name is rendered at the beginning of its caption followed by `. `.
  For example, the caption of the first `FlexiFigureBlock` in a document will begin with "Figure 1. ".
- Default: true
- Examples:
  This value is true by default:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  +++ figure
  This is a figure!
  +++
  Caption
  +++
  --------------- Expected Markup ---------------
  <figure class="flexi-figure flexi-figure_has-name" id="figure-1">
  <div class="flexi-figure__content">
  <p>This is a figure!</p>
  </div>
  <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
  </figure>
  ````````````````````````````````
  If this value is false, the `FlexiFigureBlock`'s name isn't rendered:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  o{"renderName": false}
  +++ figure
  This is a figure!
  +++
  Caption
  +++
  --------------- Expected Markup ---------------
  <figure class="flexi-figure flexi-figure_no-name" id="figure-1">
  <div class="flexi-figure__content">
  <p>This is a figure!</p>
  </div>
  <figcaption class="flexi-figure__caption">Caption</figcaption>
  </figure>
  ````````````````````````````````

##### `Attributes`
- Type: `IDictionary<string, string>`
- Description: The HTML attributes for the `FlexiFigureBlock`'s root element.
  Attribute names must be lowercase.
  If the class attribute is specified, its value is appended to default classes. This facilitates [BEM mixes](https://en.bem.info/methodology/quick-start/#mix).
  If this value has an ID value, it takes precedence over the generated ID.
  If this value is `null`, default classes are still assigned to the root element.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  o{
      "attributes": {
          "id" : "my-figure",
          "class" : "block"
      }
  }
  +++ figure
  This is a figure!
  +++
  Caption
  +++
  --------------- Expected Markup ---------------
  <figure class="flexi-figure flexi-figure_has-name block" id="my-figure">
  <div class="flexi-figure__content">
  <p>This is a figure!</p>
  </div>
  <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
  </figure>
  ````````````````````````````````

### `FlexiFigureBlocksExtensionOptions`
Options for the FlexiFigureBlocks extension. There are two ways to specify these options:
- Pass a `FlexiFigureBlocksExtensionOptions` when calling `MarkdownPipelineBuilderExtensions.UseFlexiFigureBlocks(this MarkdownPipelineBuilder pipelineBuilder, IFlexiFigureBlocksExtensionOptions options)`.
- Insert a `FlexiFigureBlocksExtensionOptions` into a `MarkdownParserContext.Properties` with key `typeof(IFlexiFigureBlocksExtensionOptions)`. Pass the `MarkdownParserContext` when you call a markdown processing method
  like `Markdown.ToHtml(markdown, stringWriter, markdownPipeline, yourMarkdownParserContext)`.  
  This method allows for different extension options when reusing a pipeline. Options specified using this method take precedence.

#### Constructor Parameters

##### `defaultBlockOptions`
- Type: `IFlexiFigureBlockOptions`
- Description: Default `IFlexiFigureBlockOptions` for all `FlexiFigureBlock`s.
  If this value is `null`, a `FlexiFigureBlockOptions` with default values is used.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extension Options ---------------
  {
      "flexiFigureBlocks": {
          "defaultBlockOptions": {
              "blockName": "figure",
              "renderName": false
          }
      }
  }
  --------------- Markdown ---------------
  +++ figure
  This is a figure!
  +++
  Caption
  +++
  --------------- Expected Markup ---------------
  <figure class="figure figure_no-name" id="figure-1">
  <div class="figure__content">
  <p>This is a figure!</p>
  </div>
  <figcaption class="figure__caption">Caption</figcaption>
  </figure>
  ````````````````````````````````
  Default `FlexiFigureBlockOptions` have lower precedence than block specific options:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Extension Options ---------------
  {
      "flexiFigureBlocks": {
          "defaultBlockOptions": {
              "renderName": false
          }
      }
  }
  --------------- Markdown ---------------
  o{
      "renderName": true
  }
  +++ figure
  This is a figure!
  +++
  Caption
  +++

  +++ figure
  This is a figure!
  +++
  Caption
  +++
  --------------- Expected Markup ---------------
  <figure class="flexi-figure flexi-figure_has-name" id="figure-1">
  <div class="flexi-figure__content">
  <p>This is a figure!</p>
  </div>
  <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
  </figure>
  <figure class="flexi-figure flexi-figure_no-name" id="figure-2">
  <div class="flexi-figure__content">
  <p>This is a figure!</p>
  </div>
  <figcaption class="flexi-figure__caption">Caption</figcaption>
  </figure>
  ````````````````````````````````

## Incomplete Features

### Continguous Figure Numbers
We should have contiguous figure numbers across documentation consisting of multiple markdown documents. This would make it easier to refer to figures.
E.g "Refer to figure 12" vs "Refer to figure 1 in *Setting up your environment*".

To make contiguous figure numbers happen, we can add a start number option to FlexiFigureBlocksExtensionOptions. When we're done processing a document, 
we'd increment the start number by the number of figures found and pass the new start number FlexiFigureBlocksExtensionOptions for the next document.  
This approach has some inefficiences - Adding a figure in one document could change figure numbers in many documents. We'd have to re-process all documents 
every time one changes. Moreover, we won't be able to process documents in parallel.

An alternative would sectioning based numbering: `Figure 1-1.`, `Figure 2-11.`. We can add a figure number prefix option to FlexiFigureBlocksExtensionOptions.
Callers will have to figure out the order of documents in the documentation and pass figure number prefixes accordingly. 

### Links
Each figure should have a button for copying its link, like a section.
