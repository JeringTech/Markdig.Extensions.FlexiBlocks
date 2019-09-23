---
blockOptions: "../src/FlexiBlocks/Extensions/FlexiBannerBlocks/FlexiBannerBlockOptions.cs"
extensionOptions: "../src/FlexiBlocks/Extensions/FlexiBannerBlocks/FlexiBannerBlocksExtensionOptions.cs"
---

# FlexiBannerBlocks
A FlexiBannerBlock is a decorative banner.

## Usage
```csharp
using Markdig;
using Jering.Markdig.Extensions.FlexiBlocks;

...
var markdownPipelineBuilder = new MarkdownPipelineBuilder();
markdownPipelineBuilder.UseFlexiBannerBlocks(/* Optional extension options */);

MarkdownPipeline markdownPipeline = markdownPipelineBuilder.Build();

string markdown = @"+++ banner
Title
+++
Blurb
+++";
string html = Markdown.ToHtml(markdown, markdownPipeline);
string expectedHtml = @"<div class=""flexi-banner flexi-banner_no-logo-icon flexi-banner_no-background-icon"">
<h1 class=""flexi-banner__title"">Title</h1>
<p class=""flexi-banner__blurb"">Blurb</p>
</div>";

Assert.Equal(expectedHtml, html)
```

# Basics
In markdown, a FlexiBannerBlock is a multi-part block with two parts - the first contains the title, the second, the blurb:
```````````````````````````````` none
--------------- Markdown ---------------
+++ banner
Title
+++
Blurb
+++
--------------- Expected Markup ---------------
<div class="flexi-banner flexi-banner_no-logo-icon flexi-banner_no-background-icon">
<h1 class="flexi-banner__title">Title</h1>
<p class="flexi-banner__blurb">Blurb</p>
</div>
````````````````````````````````

! Generated elements are assigned classes that comply with [BEM methodology](https://en.bem.info/). These classes can be customized. We explain how in [options].

Both parts can only contain inline markdown such as text with empasis ([Commonmark - Blocks and inlines](https://spec.commonmark.org/0.28/#blocks-and-inlines)):

```````````````````````````````` none
--------------- Markdown ---------------
+++ banner
*Title*
+++
**Blurb**
+++
--------------- Expected Markup ---------------
<div class="flexi-banner flexi-banner_no-logo-icon flexi-banner_no-background-icon">
<h1 class="flexi-banner__title"><em>Title</em></h1>
<p class="flexi-banner__blurb"><strong>Blurb</strong></p>
</div>
````````````````````````````````

## Options
### `FlexiBannerBlockOptions`
Options for a FlexiBannerBlock. To specify `FlexiBannerBlockOptions` for a FlexiBannerBlock, the [Options](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiOptionsBlocksSpecs.md#options) extension must be enabled.

#### Properties

##### `BlockName`
- Type: `string`
- Description: The `FlexiBannerBlock`'s [BEM block name](https://en.bem.info/methodology/naming-convention/#block-name).
  In compliance with [BEM methodology](https://en.bem.info), this value is the `FlexiBannerBlock`'s root element's class as well as the prefix for all other classes in the block.
  This value should contain only valid [CSS class characters](https://www.w3.org/TR/CSS21/syndata.html#characters).
  If this value is `null`, whitespace or an empty string, the `FlexiBannerBlock`'s block name is "flexi-banner".
- Default: "flexi-banner"
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  o{ "blockName": "banner" }
  +++ banner
  Title
  +++
  Blurb
  +++
  --------------- Expected Markup ---------------
  <div class="banner banner_no-logo-icon banner_no-background-icon">
  <h1 class="banner__title">Title</h1>
  <p class="banner__blurb">Blurb</p>
  </div>
  ````````````````````````````````

##### `LogoIcon`
- Type: `string`
- Description: The `FlexiBannerBlock`'s logo icon as an HTML fragment.
  A class attribute with value "<`BlockName`>__logo-icon" is added to this fragment's first start tag.
  If this value is `null`, whitespace or an empty string, no logo icon is rendered.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  o{ "logoIcon": "<svg><use xlink:href=\"#logo-icon\"/></svg>" }
  +++ banner
  Title
  +++
  Blurb
  +++
  --------------- Expected Markup ---------------
  <div class="flexi-banner flexi-banner_has-logo-icon flexi-banner_no-background-icon">
  <svg class="flexi-banner__logo-icon"><use xlink:href="#logo-icon"/></svg>
  <h1 class="flexi-banner__title">Title</h1>
  <p class="flexi-banner__blurb">Blurb</p>
  </div>
  ````````````````````````````````

##### `BackgroundIcon`
- Type: `string`
- Description: The `FlexiBannerBlock`'s background icon as an HTML fragment.
  A class attribute with value "<`BlockName`>__background-icon" is added to this fragment's first start tag.
  If this value is `null`, whitespace or an empty string, no background icon is rendered.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  o{ "backgroundIcon": "<svg><use xlink:href=\"#background-icon\"/></svg>" }
  +++ banner
  Title
  +++
  Blurb
  +++
  --------------- Expected Markup ---------------
  <div class="flexi-banner flexi-banner_no-logo-icon flexi-banner_has-background-icon">
  <svg class="flexi-banner__background-icon"><use xlink:href="#background-icon"/></svg>
  <h1 class="flexi-banner__title">Title</h1>
  <p class="flexi-banner__blurb">Blurb</p>
  </div>
  ````````````````````````````````

##### `Attributes`
- Type: `IDictionary<string, string>`
- Description: The HTML attributes for the `FlexiBannerBlock`'s root element.
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
  +++ banner
  Title
  +++
  Blurb
  +++
  --------------- Expected Markup ---------------
  <div class="flexi-banner flexi-banner_no-logo-icon flexi-banner_no-background-icon my-custom-class" id="my-custom-id">
  <h1 class="flexi-banner__title">Title</h1>
  <p class="flexi-banner__blurb">Blurb</p>
  </div>
  ````````````````````````````````

### `FlexiBannerBlocksExtensionOptions`
Options for the FlexiBannerBlocks extension. There are two ways to specify these options:
- Pass a `FlexiBannerBlocksExtensionOptions` when calling `MarkdownPipelineBuilderExtensions.UseFlexiBannerBlocks(this MarkdownPipelineBuilder pipelineBuilder, IFlexiBannerBlocksExtensionOptions options)`.
- Insert a `FlexiBannerBlocksExtensionOptions` into a `MarkdownParserContext.Properties` with key `typeof(IFlexiBannerBlocksExtensionOptions)`. Pass the `MarkdownParserContext` when you call a markdown processing method
  like `Markdown.ToHtml(markdown, stringWriter, markdownPipeline, yourMarkdownParserContext)`.  
  This method allows for different extension options when reusing a pipeline. Options specified using this method take precedence.

#### Constructor Parameters

##### `defaultBlockOptions`
- Type: `IFlexiBannerBlockOptions`
- Description: Default `IFlexiBannerBlockOptions` for all `FlexiBannerBlock`s.
  If this value is `null`, a `FlexiBannerBlockOptions` with default values is used.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extension Options ---------------
  {
      "flexiBannerBlocks": {
          "defaultBlockOptions": {
              "logoIcon": "<svg><use xlink:href=\"#logo-icon\"/></svg>",
              "backgroundIcon": "<svg><use xlink:href=\"#background-icon\"/></svg>",
              "attributes": {
                  "class": "block"
              }
          }
      }
  }
  --------------- Markdown ---------------
  +++ banner
  Title
  +++
  Blurb
  +++
  --------------- Expected Markup ---------------
  <div class="flexi-banner flexi-banner_has-logo-icon flexi-banner_has-background-icon block">
  <svg class="flexi-banner__background-icon"><use xlink:href="#background-icon"/></svg>
  <svg class="flexi-banner__logo-icon"><use xlink:href="#logo-icon"/></svg>
  <h1 class="flexi-banner__title">Title</h1>
  <p class="flexi-banner__blurb">Blurb</p>
  </div>
  ````````````````````````````````
  `defaultBlockOptions` has lower precedence than block specific options:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Extension Options ---------------
  {
      "flexiBannerBlocks": {
          "defaultBlockOptions": {
              "blockName": "banner"
          }
      }
  }
  --------------- Markdown ---------------
  +++ banner
  Title
  +++
  Blurb
  +++

  o{ "blockName": "special-banner" }
  +++ banner
  Title
  +++
  Blurb
  +++
  --------------- Expected Markup ---------------
  <div class="banner banner_no-logo-icon banner_no-background-icon">
  <h1 class="banner__title">Title</h1>
  <p class="banner__blurb">Blurb</p>
  </div>
  <div class="special-banner special-banner_no-logo-icon special-banner_no-background-icon">
  <h1 class="special-banner__title">Title</h1>
  <p class="special-banner__blurb">Blurb</p>
  </div>
  ````````````````````````````````

## Incomplete Features

### Configurable Heading Level
Banners could be used within articles, in place of default section headings.