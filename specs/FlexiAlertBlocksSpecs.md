---
blockOptions: "../src/FlexiBlocks/Extensions/FlexiAlertBlocks/FlexiAlertBlockOptions.cs"
extensionOptions: "../src/FlexiBlocks/Extensions/FlexiAlertBlocks/FlexiAlertBlocksExtensionOptions.cs"
---

# FlexiAlertBlocks
A FlexiAlertBlock contains content you'd like to draw readers attention to, such as a warning.

## Usage
```csharp
using Markdig;
using Jering.Markdig.Extensions.FlexiBlocks;

...
var markdownPipelineBuilder = new MarkdownPipelineBuilder();
markdownPipelineBuilder.UseFlexiAlertBlocks(/* Optional extension options */);

MarkdownPipeline markdownPipeline = markdownPipelineBuilder.Build();

string markdown = "! This is a FlexiAlertBlock."
string html = Markdown.ToHtml(markdown, markdownPipeline);
string expectedHtml = @"<div class=""flexi-alert flexi-alert_type_info flexi-alert_has-icon"">
<svg class=""flexi-alert__icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path d=""M0 0h24v24H0z"" fill=""none""/><path d=""M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z""/></svg>
<div class=""flexi-alert__content"">
<p>This is a FlexiAlertBlock.</p>
</div>
</div>";

Assert.Equal(expectedHtml, html)
```

# Basics
In markdown, a FlexiAlertBlock is a sequence of lines each starting with `!`. For example:

```````````````````````````````` none
--------------- Markdown ---------------
! This is a FlexiAlertBlock.
! This is important information.
--------------- Expected Markup ---------------
<div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon">
<svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
<div class="flexi-alert__content">
<p>This is a FlexiAlertBlock.
This is important information.</p>
</div>
</div>
````````````````````````````````

! By default, a FlexiAlertBlock has icon and content elements. These elements, along with a FlexiAlertBlock's root element, are assigned default classes. Default classes comply with 
! [BEM methodology](https://en.bem.info/).  
!
! FlexiAlertBlocks can be customized, we explain how in [options].

The space after the starting `!` of each line is optional:

```````````````````````````````` none
--------------- Markdown ---------------
!This line will render identically to the next line.
! This line will render identically to the previous line.
--------------- Expected Markup ---------------
<div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon">
<svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
<div class="flexi-alert__content">
<p>This line will render identically to the next line.
This line will render identically to the previous line.</p>
</div>
</div>
````````````````````````````````

Starting `!`s can be preceded by up to three spaces:

```````````````````````````````` none
--------------- Markdown ---------------
! These lines belong to the same FlexiAlertBlock.
 ! These lines belong to the same FlexiAlertBlock.
  ! These lines belong to the same FlexiAlertBlock.
   ! These lines belong to the same FlexiAlertBlock.
--------------- Expected Markup ---------------
<div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon">
<svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
<div class="flexi-alert__content">
<p>These lines belong to the same FlexiAlertBlock.
These lines belong to the same FlexiAlertBlock.
These lines belong to the same FlexiAlertBlock.
These lines belong to the same FlexiAlertBlock.</p>
</div>
</div>
````````````````````````````````

[Lazy continuation lines](https://spec.commonmark.org/0.28/#lazy-continuation-line) are allowed within a FlexiAlertBlock:

```````````````````````````````` none
--------------- Markdown ---------------
! This FlexiAlertBlock
contains multiple
lazy continuation lines.
--------------- Expected Markup ---------------
<div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon">
<svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
<div class="flexi-alert__content">
<p>This FlexiAlertBlock
contains multiple
lazy continuation lines.</p>
</div>
</div>
````````````````````````````````

A blank line closes a FlexiAlertBlock:

```````````````````````````````` none
--------------- Markdown ---------------
! This is a FlexiAlertBlock.

! This is another FlexiAlertBlock.
--------------- Expected Markup ---------------
<div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon">
<svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
<div class="flexi-alert__content">
<p>This is a FlexiAlertBlock.</p>
</div>
</div>
<div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon">
<svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
<div class="flexi-alert__content">
<p>This is another FlexiAlertBlock.</p>
</div>
</div>
````````````````````````````````

The first line of a FlexiAlertBlock cannot begin with `![`. This avoids conflicts with markdown images:

```````````````````````````````` none
--------------- Markdown ---------------
![This is an image](/url)

![This is neither an image nor a FlexiAlertBlock. Whether or not a line is a valid image, if it begins with `![`, it does not start a FlexiAlertBlock.
--------------- Expected Markup ---------------
<p><img src="/url" alt="This is an image" /></p>
<p>![This is neither an image nor a FlexiAlertBlock. Whether or not a line is a valid image, if it begins with <code>![</code>, it does not start a FlexiAlertBlock.</p>
````````````````````````````````

If you want the first line to begin with `[`, add a space between `!` and `[` instead:

```````````````````````````````` none
--------------- Markdown ---------------
! [This is a FlexiAlertBlock]
--------------- Expected Markup ---------------
<div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon">
<svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
<div class="flexi-alert__content">
<p>[This is a FlexiAlertBlock]</p>
</div>
</div>
````````````````````````````````

A line beginning with `![` *within* a FlexiAlertBlock is treated as a lazy continuation line:

```````````````````````````````` none
--------------- Markdown ---------------
! This is a FlexiAlertBlock
![This is an image in a FlexiAlertBlock](/url)
--------------- Expected Markup ---------------
<div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon">
<svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
<div class="flexi-alert__content">
<p>This is a FlexiAlertBlock
<img src="/url" alt="This is an image in a FlexiAlertBlock" /></p>
</div>
</div>
````````````````````````````````

## Options
### `FlexiAlertBlockOptions`
Options for a FlexiAlertBlock. To specify `FlexiAlertBlockOptions` for a FlexiAlertBlock, the [Options](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiOptionsBlocksSpecs.md#options) extension must be enabled.

#### Properties

##### `BlockName`
- Type: `string`
- Description: The `FlexiAlertBlock`'s [BEM block name](https://en.bem.info/methodology/naming-convention/#block-name).
  In compliance with [BEM methodology](https://en.bem.info), this value is the `FlexiAlertBlock`'s root element's class as well as the prefix for all other classes in the block.
  This value should contain only valid [CSS class characters](https://www.w3.org/TR/CSS21/syndata.html#characters).
  If this value is `null`, whitespace or an empty string, the `FlexiAlertBlock`'s block name is "flexi-alert".
- Default: "flexi-alert"
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  o{ "blockName": "alert" }
  ! This is a FlexiAlertBlock.
  --------------- Expected Markup ---------------
  <div class="alert alert_type_info alert_has-icon">
  <svg class="alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
  <div class="alert__content">
  <p>This is a FlexiAlertBlock.</p>
  </div>
  </div>
  ````````````````````````````````

##### `Type`
- Type: `string`
- Description: The `FlexiAlertBlock`'s type.
  This value is used in the root element's default [modifier class](https://en.bem.info/methodology/quick-start/#modifier),
  "<`BlockName`>_type_<`Type`>".
  As such, this value should contain only valid [CSS class characters](https://www.w3.org/TR/CSS21/syndata.html#characters).
  This value is also used to retrieve an icon if `Icon` is `null`, whitespace or an empty string.
  Icons for custom types can be defined in `IFlexiAlertBlocksExtensionOptions.Icons`. The default implementation of `IFlexiAlertBlocksExtensionOptions.Icons`
  contains icons for types "info", "warning" and "critical-warning".
  If this value is `null`, whitespace or an empty string, the `FlexiAlertBlock`'s type is "info".
- Default: "info"
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  o{ "type": "warning" }
  ! This is a FlexiAlertBlock.
  --------------- Expected Markup ---------------
  <div class="flexi-alert flexi-alert_type_warning flexi-alert_has-icon">
  <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"/></svg>
  <div class="flexi-alert__content">
  <p>This is a FlexiAlertBlock.</p>
  </div>
  </div>
  ````````````````````````````````

##### `Icon`
- Type: `string`
- Description: The `FlexiAlertBlock`'s icon as an HTML fragment.
  A class attribute with value "<`BlockName`>__icon" is added to this fragment's first start tag.
  If this value is `null`, whitespace or an empty string, an attempt is made to retrieve an icon for the `FlexiAlertBlock`'s type from
  `IFlexiAlertBlocksExtensionOptions.Icons`, failing which no icon is rendered.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  o{ "icon": "<svg><use xlink:href=\"#alert-icon\"/></svg>" }
  ! This is a FlexiAlertBlock.
  --------------- Expected Markup ---------------
  <div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon">
  <svg class="flexi-alert__icon"><use xlink:href="#alert-icon"/></svg>
  <div class="flexi-alert__content">
  <p>This is a FlexiAlertBlock.</p>
  </div>
  </div>
  ````````````````````````````````
  No icon is rendered if this value is `null`, whitespace or an empty string and there is no default icon for the `FlexiAlertBlock`'s type:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  o{
      "icon": null,
      "type": "no-default-icon"
  }
  ! This is a FlexiAlertBlock.
  --------------- Expected Markup ---------------
  <div class="flexi-alert flexi-alert_type_no-default-icon flexi-alert_no-icon">
  <div class="flexi-alert__content">
  <p>This is a FlexiAlertBlock.</p>
  </div>
  </div>
  ````````````````````````````````

##### `Attributes`
- Type: `IDictionary<string, string>`
- Description: The HTML attributes for the `FlexiAlertBlock`'s root element.
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
  ! This is a FlexiAlertBlock.
  --------------- Expected Markup ---------------
  <div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon my-custom-class" id="my-custom-id">
  <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
  <div class="flexi-alert__content">
  <p>This is a FlexiAlertBlock.</p>
  </div>
  </div>
  ````````````````````````````````

### `FlexiAlertBlocksExtensionOptions`
Options for the FlexiAlertBlocks extension. There are two ways to specify these options:
- Pass a `FlexiAlertBlocksExtensionOptions` when calling `MarkdownPipelineBuilderExtensions.UseFlexiAlertBlocks(this MarkdownPipelineBuilder pipelineBuilder, IFlexiAlertBlocksExtensionOptions options)`.
- Insert a `FlexiAlertBlocksExtensionOptions` into a `MarkdownParserContext.Properties` with key `typeof(IFlexiAlertBlocksExtensionOptions)`. Pass the `MarkdownParserContext` when you call a markdown processing method
  like `Markdown.ToHtml(markdown, stringWriter, markdownPipeline, yourMarkdownParserContext)`.  
  This method allows for different extension options when reusing a pipeline. Options specified using this method take precedence.

#### Constructor Parameters

##### `defaultBlockOptions`
- Type: `IFlexiAlertBlockOptions`
- Description: Default `IFlexiAlertBlockOptions` for all `FlexiAlertBlock`s.
  If this value is `null`, a `FlexiAlertBlockOptions` with default values is used.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extension Options ---------------
  {
      "flexiAlertBlocks": {
          "defaultBlockOptions": {
              "icon": "<svg><use xlink:href=\"#alert-icon\"/></svg>",
              "attributes": {
                  "class": "block"
              }
          }
      }
  }
  --------------- Markdown ---------------
  ! This is a FlexiAlertBlock.
  --------------- Expected Markup ---------------
  <div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon block">
  <svg class="flexi-alert__icon"><use xlink:href="#alert-icon"/></svg>
  <div class="flexi-alert__content">
  <p>This is a FlexiAlertBlock.</p>
  </div>
  </div>
  ````````````````````````````````
  `defaultBlockOptions` has lower precedence than block specific options:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Extension Options ---------------
  {
      "flexiAlertBlocks": {
          "defaultBlockOptions": {
              "blockName": "alert"
          }
      }
  }
  --------------- Markdown ---------------
  ! This is a FlexiAlertBlock

  o{
      "blockName": "special-alert"
  }
  ! This is a FlexiAlertBlock with block specific options.
  --------------- Expected Markup ---------------
  <div class="alert alert_type_info alert_has-icon">
  <svg class="alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
  <div class="alert__content">
  <p>This is a FlexiAlertBlock</p>
  </div>
  </div>
  <div class="special-alert special-alert_type_info special-alert_has-icon">
  <svg class="special-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
  <div class="special-alert__content">
  <p>This is a FlexiAlertBlock with block specific options.</p>
  </div>
  </div>
  ````````````````````````````````

##### `icons`
- Type: `IDictionary<string, string>`
- Description: A map of `FlexiAlertBlock` types to icon HTML fragments.
  If this value is `null`, a map of icon HTML fragments containing types "info", "warning" and "critical-warning" is used.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Extension Options ---------------
  {
      "flexiAlertBlocks": {
          "icons": {
              "closer-look": "<svg><use xlink:href=\"#closer-look-icon\"/></svg>",
              "help": "<svg><use xlink:href=\"#help-icon\"/></svg>"
          }
      }
  }
  --------------- Markdown ---------------
  o{ "type": "closer-look" }
  ! This is a closer look at some topic.

  o{ "type": "help" }
  ! This is a helpful tip.
  --------------- Expected Markup ---------------
  <div class="flexi-alert flexi-alert_type_closer-look flexi-alert_has-icon">
  <svg class="flexi-alert__icon"><use xlink:href="#closer-look-icon"/></svg>
  <div class="flexi-alert__content">
  <p>This is a closer look at some topic.</p>
  </div>
  </div>
  <div class="flexi-alert flexi-alert_type_help flexi-alert_has-icon">
  <svg class="flexi-alert__icon"><use xlink:href="#help-icon"/></svg>
  <div class="flexi-alert__content">
  <p>This is a helpful tip.</p>
  </div>
  </div>
  ````````````````````````````````
  Default icons:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  o{ "type": "info" }
  ! Info

  o{ "type": "warning" }
  ! Warning

  o{ "type": "critical-warning" }
  ! Critical warning
  --------------- Expected Markup ---------------
  <div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon">
  <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
  <div class="flexi-alert__content">
  <p>Info</p>
  </div>
  </div>
  <div class="flexi-alert flexi-alert_type_warning flexi-alert_has-icon">
  <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"/></svg>
  <div class="flexi-alert__content">
  <p>Warning</p>
  </div>
  </div>
  <div class="flexi-alert flexi-alert_type_critical-warning flexi-alert_has-icon">
  <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  <div class="flexi-alert__content">
  <p>Critical warning</p>
  </div>
  </div>
  ````````````````````````````````
