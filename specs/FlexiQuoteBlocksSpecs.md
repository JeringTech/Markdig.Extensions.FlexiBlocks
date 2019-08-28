---
blockOptions: "../src/FlexiBlocks/Extensions/FlexiQuoteBlocks/FlexiQuoteBlockOptions.cs"
extensionOptions: "../src/FlexiBlocks/Extensions/FlexiQuoteBlocks/FlexiQuoteBlocksExtensionOptions.cs"
---

# FlexiQuoteBlocks
A FlexiQuoteBlock is a quote and its citation. FlexiQuoteBlocks adhere to the [HTML spec
for block quotes](https://html.spec.whatwg.org/multipage/grouping-content.html#the-blockquote-element).

## Usage
```csharp
using Markdig;
using Jering.Markdig.Extensions.FlexiBlocks;

...
var markdownPipelineBuilder = new MarkdownPipelineBuilder();
markdownPipelineBuilder.UseFlexiQuoteBlocks(/* Optional extension options */);

MarkdownPipeline markdownPipeline = markdownPipelineBuilder.Build();

string markdown = @"+++ quote
This is a quote!
+++
Author, in ""[Work](work-url.com)""
+++";
string html = Markdown.ToHtml(markdown, markdownPipeline);
string expectedHtml = @"<div class=\"flexi-quote flexi-quote_has_icon\">
<svg class=\"flexi-quote__icon\" xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 14 10\"><path d=\"M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z\"/></svg>
<div class=\"flexi-quote__content\">
<blockquote class=\"flexi-quote__blockquote\" cite=\"work-url.com\">
<p>This is a quote!</p>
</blockquote>
<p class=\"flexi-quote__citation\">— Author, in <cite><a href=\"work-url.com\">Work</a></cite></p>
</div>
</div>";

Assert.Equal(expectedHtml, html)
```

# Basics
In markdown, a FlexiQuoteBlock is a multi-part block with two parts - the first contains the quote, the second its citation:

```````````````````````````````` none
--------------- Markdown ---------------
+++ quote
This is a quote!
+++
Author, in Work
+++
--------------- Expected Markup ---------------
<div class="flexi-quote flexi-quote_has_icon">
<svg class="flexi-quote__icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 14 10"><path d="M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z"/></svg>
<div class="flexi-quote__content">
<blockquote class="flexi-quote__blockquote">
<p>This is a quote!</p>
</blockquote>
<p class="flexi-quote__citation">— Author, in Work</p>
</div>
</div>
````````````````````````````````

! Generated elements are assigned classes that comply with [BEM methodology](https://en.bem.info/). These classes can be customized. We explain how in [options].

The quote part can contain markdown blocks such as code blocks, lists and ATX headings. The citation part can only contain inline markdown
such as text with empasis ([Commonmark - Blocks and inlines](https://spec.commonmark.org/0.28/#blocks-and-inlines)):

```````````````````````````````` none
--------------- Extra Extensions ---------------
FlexiCodeBlocks
--------------- Markdown ---------------
+++ quote
```
Code you'd like to quote
```
+++
*Author*, in **Work**
+++
--------------- Expected Markup ---------------
<div class="flexi-quote flexi-quote_has_icon">
<svg class="flexi-quote__icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 14 10"><path d="M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z"/></svg>
<div class="flexi-quote__content">
<blockquote class="flexi-quote__blockquote">
<div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
<header class="flexi-code__header">
<span class="flexi-code__title"></span>
<button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
<svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
</button>
</header>
<pre class="flexi-code__pre"><code class="flexi-code__code">Code you'd like to quote
</code></pre>
</div>
</blockquote>
<p class="flexi-quote__citation">— <em>Author</em>, in <strong>Work</strong></p>
</div>
</div>
````````````````````````````````

To be [HTML spec compliant](https://html.spec.whatwg.org/multipage/text-level-semantics.html#the-cite-element), 
the title of the work where the quote comes from should be in a &lt;cite&gt; element. This extension provides
a way to do this - any string wrapped in double quotes is rendered in a &lt;cite&gt; element:

```````````````````````````````` none
--------------- Markdown ---------------
+++ quote
This is a quote!
+++
Author, in ""Work""
+++
--------------- Expected Markup ---------------
<div class="flexi-quote flexi-quote_has_icon">
<svg class="flexi-quote__icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 14 10"><path d="M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z"/></svg>
<div class="flexi-quote__content">
<blockquote class="flexi-quote__blockquote">
<p>This is a quote!</p>
</blockquote>
<p class="flexi-quote__citation">— Author, in <cite>Work</cite></p>
</div>
</div>
````````````````````````````````

If the citation contains links, the blockquote's cite attribute is rendered with the last link's URL as its value:
```````````````````````````````` none
--------------- Markdown ---------------
+++ quote
This is a quote!
+++
[Author](author-url.com), in ""[Work](work-url.com)""
+++
--------------- Expected Markup ---------------
<div class="flexi-quote flexi-quote_has_icon">
<svg class="flexi-quote__icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 14 10"><path d="M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z"/></svg>
<div class="flexi-quote__content">
<blockquote class="flexi-quote__blockquote" cite="work-url.com">
<p>This is a quote!</p>
</blockquote>
<p class="flexi-quote__citation">— <a href="author-url.com">Author</a>, in <cite><a href="work-url.com">Work</a></cite></p>
</div>
</div>
````````````````````````````````

! You can specify which link's URL to use, we explain how in [options].

## Options
### `FlexiQuoteBlockOptions`
Options for a FlexiQuoteBlock. To specify `FlexiQuoteBlockOptions` for a FlexiQuoteBlock, the [Options](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/OptionsBlocksSpecs.md#options) extension must be enabled.

#### Properties

##### `BlockName`
- Type: `string`
- Description: The `FlexiQuoteBlock`'s [BEM block name](https://en.bem.info/methodology/naming-convention/#block-name).
  In compliance with [BEM methodology](https://en.bem.info), this value is the `FlexiQuoteBlock`'s root element's class as well as the prefix for all other classes in the block.
  This value should contain only valid [CSS class characters](https://www.w3.org/TR/CSS21/syndata.html#characters).
  If this value is `null`, whitespace or an empty string, the `FlexiQuoteBlock`'s block name is "flexi-quote".
- Default: "flexi-quote"
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  @{ "blockName": "quote" }
  +++ quote
  This is a quote!
  +++
  Author, in Work
  +++
  --------------- Expected Markup ---------------
  <div class="quote quote_has_icon">
  <svg class="quote__icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 14 10"><path d="M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z"/></svg>
  <div class="quote__content">
  <blockquote class="quote__blockquote">
  <p>This is a quote!</p>
  </blockquote>
  <p class="quote__citation">— Author, in Work</p>
  </div>
  </div>
  ````````````````````````````````

##### `Icon`
- Type: `string`
- Description: The `FlexiQuoteBlock`'s icon as an HTML fragment.
  A class attribute with value "<`BlockName`>__icon" is added to this fragment's first start tag.
  If this value is `null`, whitespace or an empty string, no icon is rendered.
- Default: an opening quotation mark icon
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  @{ "icon": "<svg><use xlink:href=\"#alert-icon\"/></svg>" }
  +++ quote
  This is a quote!
  +++
  Author, in Work
  +++
  --------------- Expected Markup ---------------
  <div class="flexi-quote flexi-quote_has_icon">
  <svg class="flexi-quote__icon"><use xlink:href="#alert-icon"/></svg>
  <div class="flexi-quote__content">
  <blockquote class="flexi-quote__blockquote">
  <p>This is a quote!</p>
  </blockquote>
  <p class="flexi-quote__citation">— Author, in Work</p>
  </div>
  </div>
  ````````````````````````````````
  No icon is rendered if this value is `null`, whitespace or an empty string:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  @{ "icon": null }
  +++ quote
  This is a quote!
  +++
  Author, in Work
  +++
  --------------- Expected Markup ---------------
  <div class="flexi-quote flexi-quote_no_icon">
  <div class="flexi-quote__content">
  <blockquote class="flexi-quote__blockquote">
  <p>This is a quote!</p>
  </blockquote>
  <p class="flexi-quote__citation">— Author, in Work</p>
  </div>
  </div>
  ````````````````````````````````

##### `CiteLink`
- Type: `int`
- Description: The index of the link in the `FlexiQuoteBlock`'s citation that points to the work where its quote comes from.
  The link's URL is assigned to the `FlexiQuoteBlock`'s blockquote element's cite attribute,
  in compliance with [HTML specifications](https://html.spec.whatwg.org/multipage/grouping-content.html#the-blockquote-element).
  If this value is `-n`, the link is the nth last link. For example, if this value is `-2`, the link is the 2nd last link.
  If the `FlexiQuoteBlock` has at least one link in its citation, this value must be within the logical range of indices.
- Default: `-1` (the last link)
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  @{"citeLink": 1}
  +++ quote
  This is a quote!
  +++
  [Author](author-url.com), in ""[Work](work-url.com)"" from ""[Guide](guide-url.com)""
  +++

  @{"citeLink": -2}
  +++ quote
  This is a quote!
  +++
  [Author](author-url.com), in ""[Work](work-url.com)"" from ""[Guide](guide-url.com)""
  +++
  --------------- Expected Markup ---------------
  <div class="flexi-quote flexi-quote_has_icon">
  <svg class="flexi-quote__icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 14 10"><path d="M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z"/></svg>
  <div class="flexi-quote__content">
  <blockquote class="flexi-quote__blockquote" cite="work-url.com">
  <p>This is a quote!</p>
  </blockquote>
  <p class="flexi-quote__citation">— <a href="author-url.com">Author</a>, in <cite><a href="work-url.com">Work</a></cite> from <cite><a href="guide-url.com">Guide</a></cite></p>
  </div>
  </div>
  <div class="flexi-quote flexi-quote_has_icon">
  <svg class="flexi-quote__icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 14 10"><path d="M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z"/></svg>
  <div class="flexi-quote__content">
  <blockquote class="flexi-quote__blockquote" cite="work-url.com">
  <p>This is a quote!</p>
  </blockquote>
  <p class="flexi-quote__citation">— <a href="author-url.com">Author</a>, in <cite><a href="work-url.com">Work</a></cite> from <cite><a href="guide-url.com">Guide</a></cite></p>
  </div>
  </div>
  ````````````````````````````````

##### `Attributes`
- Type: `IDictionary<string, string>`
- Description: The HTML attributes for the `FlexiQuoteBlock`'s root element.
  Attribute names must be lowercase.
  If the class attribute is specified, its value is appended to default classes. This facilitates [BEM mixes](https://en.bem.info/methodology/quick-start/#mix).
  If this value is `null`, default classes are still assigned to the root element.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  @{
      "attributes": {
          "id" : "my-custom-id",
          "class" : "my-custom-class"
      }
  }
  +++ quote
  This is a quote!
  +++
  Author, in Work
  +++
  --------------- Expected Markup ---------------
  <div class="flexi-quote flexi-quote_has_icon my-custom-class" id="my-custom-id">
  <svg class="flexi-quote__icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 14 10"><path d="M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z"/></svg>
  <div class="flexi-quote__content">
  <blockquote class="flexi-quote__blockquote">
  <p>This is a quote!</p>
  </blockquote>
  <p class="flexi-quote__citation">— Author, in Work</p>
  </div>
  </div>
  ````````````````````````````````

### `FlexiQuoteBlocksExtensionOptions`
Options for the FlexiQuoteBlocks extension. There are two ways to specify these options:
- Pass a `FlexiQuoteBlocksExtensionOptions` when calling `MarkdownPipelineBuilderExtensions.UseFlexiQuoteBlocks(this MarkdownPipelineBuilder pipelineBuilder, IFlexiQuoteBlocksExtensionOptions options)`.
- Insert a `FlexiQuoteBlocksExtensionOptions` into a `MarkdownParserContext.Properties` with key `typeof(IFlexiQuoteBlocksExtensionOptions)`. Pass the `MarkdownParserContext` when you call a markdown processing method
  like `Markdown.ToHtml(markdown, stringWriter, markdownPipeline, yourMarkdownParserContext)`.  
  This method allows for different extension options when reusing a pipeline. Options specified using this method take precedence.

#### Constructor Parameters

##### `defaultBlockOptions`
- Type: `IFlexiQuoteBlockOptions`
- Description: Default `IFlexiQuoteBlockOptions` for all `FlexiQuoteBlock`s.
  If this value is `null`, a `FlexiQuoteBlockOptions` with default values is used.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extension Options ---------------
  {
      "flexiQuoteBlocks": {
          "defaultBlockOptions": {
              "icon": "<svg><use xlink:href=\"#quote-icon\"/></svg>",
              "citeLink": 0,
              "attributes": {
                  "class": "block"
              }
          }
      }
  }
  --------------- Markdown ---------------
  +++ quote
  This is a quote!
  +++
  ""[Work](work-url.com)"" by [Author](author-url.com)
  +++
  --------------- Expected Markup ---------------
  <div class="flexi-quote flexi-quote_has_icon block">
  <svg class="flexi-quote__icon"><use xlink:href="#quote-icon"/></svg>
  <div class="flexi-quote__content">
  <blockquote class="flexi-quote__blockquote" cite="work-url.com">
  <p>This is a quote!</p>
  </blockquote>
  <p class="flexi-quote__citation">— <cite><a href="work-url.com">Work</a></cite> by <a href="author-url.com">Author</a></p>
  </div>
  </div>
  ````````````````````````````````
  `defaultBlockOptions` has lower precedence than block specific options:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Extension Options ---------------
  {
      "flexiQuoteBlocks": {
          "defaultBlockOptions": {
              "blockName": "quote"
          }
      }
  }
  --------------- Markdown ---------------
  +++ quote
  This is a quote!
  +++
  Author, in Work
  +++

  @{ "blockName": "special-quote" }
  +++ quote
  This is a quote!
  +++
  Author, in Work
  +++
  --------------- Expected Markup ---------------
  <div class="quote quote_has_icon">
  <svg class="quote__icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 14 10"><path d="M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z"/></svg>
  <div class="quote__content">
  <blockquote class="quote__blockquote">
  <p>This is a quote!</p>
  </blockquote>
  <p class="quote__citation">— Author, in Work</p>
  </div>
  </div>
  <div class="special-quote special-quote_has_icon">
  <svg class="special-quote__icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 14 10"><path d="M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z"/></svg>
  <div class="special-quote__content">
  <blockquote class="special-quote__blockquote">
  <p>This is a quote!</p>
  </blockquote>
  <p class="special-quote__citation">— Author, in Work</p>
  </div>
  </div>
  ````````````````````````````````

