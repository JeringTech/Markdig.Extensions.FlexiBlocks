---
blockOptions: "../src/FlexiBlocks/Extensions/FlexiCardsBlocks/FlexiCardsBlockOptions.cs"
utilityTypes: ["../src/FlexiBlocks/Extensions/FlexiCardsBlocks/FlexiCardBlockOptions.cs"]
extensionOptions: "../src/FlexiBlocks/Extensions/FlexiCardsBlocks/FlexiCardsBlocksExtensionOptions.cs"
---

# FlexiCardsBlocks
A FlexiCardsBlock is a collection of FlexiCardBlocks. A FlexiCardBlock is a decorative element for displaying and linking to information.

## Usage
```csharp
using Markdig;
using Jering.Markdig.Extensions.FlexiBlocks;

...
var markdownPipelineBuilder = new MarkdownPipelineBuilder();
markdownPipelineBuilder.UseFlexiCardsBlocks(/* Optional extension options */);

MarkdownPipeline markdownPipeline = markdownPipelineBuilder.Build();

string markdown = @"[[[
+++ card
Title 1
+++
Content 1
+++
Footnote 1
+++

+++ card
Title 2
+++
Content 2
+++
Footnote 2
+++
[[[";
string html = Markdown.ToHtml(markdown, markdownPipeline);
string expectedHtml = @"<div class=""flexi-cards flexi-cards_size_small"">
<div class=""flexi-cards__card flexi-cards__card_not-link flexi-cards__card_no-background-icon"">
<p class=""flexi-cards__card-title"">Title 1</p>
<div class=""flexi-cards__card-content"">
<p>Content 1</p>
</div>
<p class=""flexi-cards__card-footnote"">Footnote 1</p>
</div>
<div class=""flexi-cards__card flexi-cards__card_not-link flexi-cards__card_no-background-icon"">
<p class=""flexi-cards__card-title"">Title 2</p>
<div class=""flexi-cards__card-content"">
<p>Content 2</p>
</div>
<p class=""flexi-cards__card-footnote"">Footnote 2</p>
</div>
</div>";

Assert.Equal(expectedHtml, html)
```

# Basics
In markdown, a FlexiCardsBlock is a fenced block containing FlexiCardBlocks. Its fences consist of `[` characters and mostly behave the same as 
[fences for code blocks](https://spec.commonmark.org/0.28/#fenced-code-blocks). The exception is that unlike fences for a code block, a 
FlexiCardsBlock's fences must have the same number of `[` characters.
A FlexiCardBlock is a multi-part block with three parts - the first contains the title, the second, the content, and the third, the footnote.
```````````````````````````````` none
--------------- Markdown ---------------
[[[
+++ card
Title 1
+++
Content 1
+++
Footnote 1
+++

+++ card
Title 2
+++
Content 2
+++
Footnote 2
+++
[[[
--------------- Expected Markup ---------------
<div class="flexi-cards flexi-cards_size_small">
<div class="flexi-cards__card flexi-cards__card_not-link flexi-cards__card_no-background-icon">
<p class="flexi-cards__card-title">Title 1</p>
<div class="flexi-cards__card-content">
<p>Content 1</p>
</div>
<p class="flexi-cards__card-footnote">Footnote 1</p>
</div>
<div class="flexi-cards__card flexi-cards__card_not-link flexi-cards__card_no-background-icon">
<p class="flexi-cards__card-title">Title 2</p>
<div class="flexi-cards__card-content">
<p>Content 2</p>
</div>
<p class="flexi-cards__card-footnote">Footnote 2</p>
</div>
</div>
````````````````````````````````

! Generated elements are assigned classes that comply with [BEM methodology](https://en.bem.info/). These classes can be customized. We explain how in [options].

Content parts can contain markdown blocks such as code blocks, lists and ATX headings. Title and footnote parts can only contain inline markdown
such as text with empasis ([Commonmark - Blocks and inlines](https://spec.commonmark.org/0.28/#blocks-and-inlines)):

```````````````````````````````` none
--------------- Extra Extensions ---------------
FlexiCodeBlocks
--------------- Markdown ---------------
[[[
+++ card
*Title 1*
+++
```
Content 1
```
+++
**Footnote 1**
+++
[[[
--------------- Expected Markup ---------------
<div class="flexi-cards flexi-cards_size_small">
<div class="flexi-cards__card flexi-cards__card_not-link flexi-cards__card_no-background-icon">
<p class="flexi-cards__card-title"><em>Title 1</em></p>
<div class="flexi-cards__card-content">
<div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
<header class="flexi-code__header">
<button class="flexi-code__copy-button" aria-label="Copy code">
<svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
</button>
</header>
<pre class="flexi-code__pre"><code class="flexi-code__code">Content 1
</code></pre>
</div>
</div>
<p class="flexi-cards__card-footnote"><strong>Footnote 1</strong></p>
</div>
</div>
````````````````````````````````

## Options
### `FlexiCardsBlockOptions`
Options for a FlexiCardsBlock. To specify `FlexiCardsBlockOptions` for a FlexiCardsBlock, the [Options](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiOptionsBlocksSpecs.md#options) extension must be enabled.

#### Properties

##### `BlockName`
- Type: `string`
- Description: The `FlexiCardsBlock`'s [BEM block name](https://en.bem.info/methodology/naming-convention/#block-name).
  In compliance with [BEM methodology](https://en.bem.info), this value is the `FlexiCardsBlock`'s root element's class as well as the prefix for all other classes in the block.
  This value should contain only valid [CSS class characters](https://www.w3.org/TR/CSS21/syndata.html#characters).
  If this value is `null`, whitespace or an empty string, the `FlexiCardsBlock`'s block name is "flexi-cards".
- Default: "flexi-cards"
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  o{ "blockName": "cards" }
  [[[
  +++ card
  Title 1
  +++
  Content 1
  +++
  Footnote 1
  +++
  [[[
  --------------- Expected Markup ---------------
  <div class="cards cards_size_small">
  <div class="cards__card cards__card_not-link cards__card_no-background-icon">
  <p class="cards__card-title">Title 1</p>
  <div class="cards__card-content">
  <p>Content 1</p>
  </div>
  <p class="cards__card-footnote">Footnote 1</p>
  </div>
  </div>
  ````````````````````````````````

##### `CardSize`
- Type: `FlexiCardBlockSize`
- Description: The display size of contained `FlexiCardBlock`s.
  A class attribute with value "<`BlockName`>_size_<`CardSize`>" is added to the `FlexiCardsBlock`'s root element.
- Default: `FlexiCardBlockSize.Small`
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  o{ "cardSize": "medium" }
  [[[
  +++ card
  Title 1
  +++
  Content 1
  +++
  Footnote 1
  +++
  [[[
  --------------- Expected Markup ---------------
  <div class="flexi-cards flexi-cards_size_medium">
  <div class="flexi-cards__card flexi-cards__card_not-link flexi-cards__card_no-background-icon">
  <p class="flexi-cards__card-title">Title 1</p>
  <div class="flexi-cards__card-content">
  <p>Content 1</p>
  </div>
  <p class="flexi-cards__card-footnote">Footnote 1</p>
  </div>
  </div>
  ````````````````````````````````

##### `DefaultCardOptions`
- Type: `IFlexiCardBlockOptions`
- Description: The default `IFlexiCardBlockOptions` for contained `FlexiCardBlock`s.
  If this value is `null`, a `FlexiCardBlockOptions` with default values is used.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  o{ 
      "defaultCardOptions": {
          "backgroundIcon": "<svg><use xlink:href=\"#background-icon\"/></svg>"
      }
  }
  [[[
  +++ card
  Title 1
  +++
  Content 1
  +++
  Footnote 1
  +++
  [[[
  --------------- Expected Markup ---------------
  <div class="flexi-cards flexi-cards_size_small">
  <div class="flexi-cards__card flexi-cards__card_not-link flexi-cards__card_has-background-icon">
  <svg class="flexi-cards__card-background-icon"><use xlink:href="#background-icon"/></svg>
  <p class="flexi-cards__card-title">Title 1</p>
  <div class="flexi-cards__card-content">
  <p>Content 1</p>
  </div>
  <p class="flexi-cards__card-footnote">Footnote 1</p>
  </div>
  </div>
  ````````````````````````````````
  `defaultCardOptions` has lower precedence than card specific options:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  o{ 
      "defaultCardOptions": {
          "backgroundIcon": "<svg><use xlink:href=\"#background-icon\"/></svg>"
      }
  }
  [[[
  +++ card
  Title 1
  +++
  Content 1
  +++
  Footnote 1
  +++

  o{"backgroundIcon": "<svg><use xlink:href=\"#alternative-icon\"/></svg>"}
  +++ card
  Title 2
  +++
  Content 2
  +++
  Footnote 2
  +++
  [[[
  --------------- Expected Markup ---------------
  <div class="flexi-cards flexi-cards_size_small">
  <div class="flexi-cards__card flexi-cards__card_not-link flexi-cards__card_has-background-icon">
  <svg class="flexi-cards__card-background-icon"><use xlink:href="#background-icon"/></svg>
  <p class="flexi-cards__card-title">Title 1</p>
  <div class="flexi-cards__card-content">
  <p>Content 1</p>
  </div>
  <p class="flexi-cards__card-footnote">Footnote 1</p>
  </div>
  <div class="flexi-cards__card flexi-cards__card_not-link flexi-cards__card_has-background-icon">
  <svg class="flexi-cards__card-background-icon"><use xlink:href="#alternative-icon"/></svg>
  <p class="flexi-cards__card-title">Title 2</p>
  <div class="flexi-cards__card-content">
  <p>Content 2</p>
  </div>
  <p class="flexi-cards__card-footnote">Footnote 2</p>
  </div>
  </div>
  ````````````````````````````````

##### `Attributes`
- Type: `IDictionary<string, string>`
- Description: The HTML attributes for the `FlexiCardsBlock`'s root element.
  Attribute names must be lowercase.
  If the class attribute is specified, its value is appended to default classes. This facilitates [BEM mixes](https://en.bem.info/methodology/quick-start/#mix).
  If this value is `null`, default classes are still assigned to the root element.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  o{
      "attributes": {
          "id" : "my-custom-id",
          "class" : "my-custom-class"
      }
  }
  [[[
  +++ card
  Title 1
  +++
  Content 1
  +++
  Footnote 1
  +++
  [[[
  --------------- Expected Markup ---------------
  <div class="flexi-cards flexi-cards_size_small my-custom-class" id="my-custom-id">
  <div class="flexi-cards__card flexi-cards__card_not-link flexi-cards__card_no-background-icon">
  <p class="flexi-cards__card-title">Title 1</p>
  <div class="flexi-cards__card-content">
  <p>Content 1</p>
  </div>
  <p class="flexi-cards__card-footnote">Footnote 1</p>
  </div>
  </div>
  ````````````````````````````````

### `FlexiCardBlockOptions`
Options for a `FlexiCardBlock`.

#### Properties

##### `Url`
- Type: `string`
- Description: The URL the `FlexiCardBlock` points to.
  If this value is not `null`, whitespace or an empty string, the `FlexiCardBlock`'s outermost element is an `<a>`
  with this value as its `href`.
  Otherwise, the `FlexiCardsBlock`'s outermost element is a `<div>`.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  [[[
  o{"url": "/url?a=1&b=2"}
  +++ card
  Title 1
  +++
  Content 1
  +++
  Footnote 1
  +++

  +++ card
  Title 2
  +++
  Content 2
  +++
  Footnote 2
  +++
  [[[
  --------------- Expected Markup ---------------
  <div class="flexi-cards flexi-cards_size_small">
  <a class="flexi-cards__card flexi-cards__card_is-link flexi-cards__card_no-background-icon" href="/url?a=1&amp;b=2">
  <p class="flexi-cards__card-title">Title 1</p>
  <div class="flexi-cards__card-content">
  <p>Content 1</p>
  </div>
  <p class="flexi-cards__card-footnote">Footnote 1</p>
  </a>
  <div class="flexi-cards__card flexi-cards__card_not-link flexi-cards__card_no-background-icon">
  <p class="flexi-cards__card-title">Title 2</p>
  <div class="flexi-cards__card-content">
  <p>Content 2</p>
  </div>
  <p class="flexi-cards__card-footnote">Footnote 2</p>
  </div>
  </div>
  ````````````````````````````````

##### `BackgroundIcon`
- Type: `string`
- Description: The `FlexiCardBlock`'s background icon as an HTML fragment.
  A class attribute with value "<parent block name>__card_background-icon" where <parent block name> is the block name of
  the `FlexiCardBlock`'s parent `FlexiCardsBlock`, is added to this fragment's first start tag.
  If this value is `null`, whitespace or an empty string, no background icon is rendered.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  [[[
  o{"backgroundIcon": "<svg><use xlink:href=\"#background-icon\"/></svg>"}
  +++ card
  Title 1
  +++
  Content 1
  +++
  Footnote 1
  +++
  [[[
  --------------- Expected Markup ---------------
  <div class="flexi-cards flexi-cards_size_small">
  <div class="flexi-cards__card flexi-cards__card_not-link flexi-cards__card_has-background-icon">
  <svg class="flexi-cards__card-background-icon"><use xlink:href="#background-icon"/></svg>
  <p class="flexi-cards__card-title">Title 1</p>
  <div class="flexi-cards__card-content">
  <p>Content 1</p>
  </div>
  <p class="flexi-cards__card-footnote">Footnote 1</p>
  </div>
  </div>
  ````````````````````````````````

##### `Attributes`
- Type: `IDictionary<string, string>`
- Description: The HTML attributes for the `FlexiCardBlock`'s root element.
  Attribute names must be lowercase.
  If the class attribute is specified, its value is appended to default classes. This facilitates [BEM mixes](https://en.bem.info/methodology/quick-start/#mix).
  If this value is `null`, default classes are still assigned to the root element.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  [[[
  o{
      "attributes": {
          "id" : "my-custom-id",
          "class" : "my-custom-class"
      }
  }
  +++ card
  Title 1
  +++
  Content 1
  +++
  Footnote 1
  +++
  [[[
  --------------- Expected Markup ---------------
  <div class="flexi-cards flexi-cards_size_small">
  <div class="flexi-cards__card flexi-cards__card_not-link flexi-cards__card_no-background-icon my-custom-class" id="my-custom-id">
  <p class="flexi-cards__card-title">Title 1</p>
  <div class="flexi-cards__card-content">
  <p>Content 1</p>
  </div>
  <p class="flexi-cards__card-footnote">Footnote 1</p>
  </div>
  </div>
  ````````````````````````````````

### `FlexiCardsBlocksExtensionOptions`
Options for the FlexiCardsBlocks extension. There are two ways to specify these options:
- Pass a `FlexiCardsBlocksExtensionOptions` when calling `MarkdownPipelineBuilderExtensions.UseFlexiCardsBlocks(this MarkdownPipelineBuilder pipelineBuilder, IFlexiCardsBlocksExtensionOptions options)`.
- Insert a `FlexiCardsBlocksExtensionOptions` into a `MarkdownParserContext.Properties` with key `typeof(IFlexiCardsBlocksExtensionOptions)`. Pass the `MarkdownParserContext` when you call a markdown processing method
  like `Markdown.ToHtml(markdown, stringWriter, markdownPipeline, yourMarkdownParserContext)`.  
  This method allows for different extension options when reusing a pipeline. Options specified using this method take precedence.

#### Constructor Parameters

##### `defaultBlockOptions`
- Type: `IFlexiCardsBlockOptions`
- Description: Default `IFlexiCardsBlockOptions` for all `FlexiCardsBlock`s.
  If this value is `null`, a `FlexiCardsBlockOptions` with default values is used.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extension Options ---------------
  {
      "flexiCardsBlocks": {
          "defaultBlockOptions": {
              "cardSize": "medium",
              "defaultCardOptions": {
                  "backgroundIcon": "<svg><use xlink:href=\"#background-icon\"/></svg>"
              },
              "attributes": {
                  "class": "block"
              }
          }
      }
  }
  --------------- Markdown ---------------
  [[[
  +++ card
  Title 1
  +++
  Content 1
  +++
  Footnote 1
  +++
  [[[
  --------------- Expected Markup ---------------
  <div class="flexi-cards flexi-cards_size_medium block">
  <div class="flexi-cards__card flexi-cards__card_not-link flexi-cards__card_has-background-icon">
  <svg class="flexi-cards__card-background-icon"><use xlink:href="#background-icon"/></svg>
  <p class="flexi-cards__card-title">Title 1</p>
  <div class="flexi-cards__card-content">
  <p>Content 1</p>
  </div>
  <p class="flexi-cards__card-footnote">Footnote 1</p>
  </div>
  </div>
  ````````````````````````````````
  `defaultBlockOptions` has lower precedence than block specific options:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Extension Options ---------------
  {
      "flexiCardsBlocks": {
          "defaultBlockOptions": {
              "blockName": "cards",
              "defaultCardOptions": {
                  "backgroundIcon": "<svg><use xlink:href=\"#background-icon\"/></svg>"
              }
          }
      }
  }
  --------------- Markdown ---------------
  [[[
  o{ "backgroundIcon": "<svg><use xlink:href=\"#alternative-icon\"/></svg>" }
  +++ card
  Title 1
  +++
  Content 1
  +++
  Footnote 1
  +++
  [[[

  o{ "blockName": "special-cards" }
  [[[
  +++ card
  Title 2
  +++
  Content 2
  +++
  Footnote 2
  +++
  [[[
  --------------- Expected Markup ---------------
  <div class="cards cards_size_small">
  <div class="cards__card cards__card_not-link cards__card_has-background-icon">
  <svg class="cards__card-background-icon"><use xlink:href="#alternative-icon"/></svg>
  <p class="cards__card-title">Title 1</p>
  <div class="cards__card-content">
  <p>Content 1</p>
  </div>
  <p class="cards__card-footnote">Footnote 1</p>
  </div>
  </div>
  <div class="special-cards special-cards_size_small">
  <div class="special-cards__card special-cards__card_not-link special-cards__card_has-background-icon">
  <svg class="special-cards__card-background-icon"><use xlink:href="#background-icon"/></svg>
  <p class="special-cards__card-title">Title 2</p>
  <div class="special-cards__card-content">
  <p>Content 2</p>
  </div>
  <p class="special-cards__card-footnote">Footnote 2</p>
  </div>
  </div>
  ````````````````````````````````

## Mechanics
### Nesting FlexiCardsBlocks
FlexiCardBlock opening and closing fences can contain any number of `[` characters as long as there are at least 3.
The number of characters in each opening/closing pair of fences must match. To nest a FlexiCardsBlock, use
fences with a different number of `[` characters than the containing FlexiCardsBlock:
```````````````````````````````` none
--------------- Markdown ---------------
[[[
+++ card
Title 1
+++
[[[[
+++ card
Nested card
+++
Nested card content
+++
Nested card footnote
+++
[[[[
+++
Footnote 1
+++
[[[
--------------- Expected Markup ---------------
<div class="flexi-cards flexi-cards_size_small">
<div class="flexi-cards__card flexi-cards__card_not-link flexi-cards__card_no-background-icon">
<p class="flexi-cards__card-title">Title 1</p>
<div class="flexi-cards__card-content">
<div class="flexi-cards flexi-cards_size_small">
<div class="flexi-cards__card flexi-cards__card_not-link flexi-cards__card_no-background-icon">
<p class="flexi-cards__card-title">Nested card</p>
<div class="flexi-cards__card-content">
<p>Nested card content</p>
</div>
<p class="flexi-cards__card-footnote">Nested card footnote</p>
</div>
</div>
</div>
<p class="flexi-cards__card-footnote">Footnote 1</p>
</div>
</div>
````````````````````````````````

