---
blockOptions: "../src/FlexiBlocks/Extensions/FlexiTabsBlocks/FlexiTabsBlockOptions.cs"
utilityTypes: ["../src/FlexiBlocks/Extensions/FlexiTabsBlocks/FlexiTabBlockOptions.cs"]
extensionOptions: "../src/FlexiBlocks/Extensions/FlexiTabsBlocks/FlexiTabsBlocksExtensionOptions.cs"
---

# FlexiTabsBlocks
A FlexiTabsBlock is a tabbed collection of panels. Each panel and its associated tab is represented by a FlexiTabBlock.

## Usage
```csharp
using Markdig;
using Jering.Markdig.Extensions.FlexiBlocks;

...
var markdownPipelineBuilder = new MarkdownPipelineBuilder();
markdownPipelineBuilder.UseFlexiTabsBlocks(/* Optional extension options */);

MarkdownPipeline markdownPipeline = markdownPipelineBuilder.Build();

string markdown = @"///
+++ tab
Tab 1
+++
Panel 1
+++

+++ tab
Tab 2
+++
Panel 2
+++
///";
string html = Markdown.ToHtml(markdown, markdownPipeline);
string expectedHtml = @"<div class=""flexi-tabs"">
<div class=""flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal"">
<div class=""flexi-tabs__tab-list scrollable-indicators__scrollable"" role=""tablist"">
<button class=""flexi-tabs__tab flexi-tabs__tab_selected"" role=""tab"" aria-selected=""true"">Tab 1</button>
<button class=""flexi-tabs__tab"" role=""tab"" aria-selected=""false"" tabindex=""-1"">Tab 2</button>
</div>
<div class=""scrollable-indicators__indicator scrollable-indicators__indicator_start""></div>
<div class=""scrollable-indicators__indicator scrollable-indicators__indicator_end""></div>
</div>
<div class=""flexi-tabs__tab-panel"" tabindex=""0"" role=""tabpanel"" aria-label=""Tab 1"">
<p>Panel 1</p>
</div>
<div class=""flexi-tabs__tab-panel flexi-tabs__tab-panel_hidden"" tabindex=""0"" role=""tabpanel"" aria-label=""Tab 2"">
<p>Panel 2</p>
</div>
</div>";

Assert.Equal(expectedHtml, html)
```

# Basics
In markdown, a FlexiTabsBlock is a fenced block containing FlexiTabBlocks. Its fences consist of `/` characters and mostly behave the same as 
[fences for code blocks](https://spec.commonmark.org/0.28/#fenced-code-blocks). The exception is that unlike fences for a code block, a 
FlexiTabsBlock's fences must have the same number of `/` characters.
A FlexiTabBlock is a multi-part block with two parts - the first contains the tab, the second the panel.
```````````````````````````````` none
--------------- Markdown ---------------
///
+++ tab
Tab 1
+++
Panel 1
+++

+++ tab
Tab 2
+++
Panel 2
+++
///
--------------- Expected Markup ---------------
<div class="flexi-tabs">
<div class="flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal">
<div class="flexi-tabs__tab-list scrollable-indicators__scrollable" role="tablist">
<button class="flexi-tabs__tab flexi-tabs__tab_selected" role="tab" aria-selected="true">Tab 1</button>
<button class="flexi-tabs__tab" role="tab" aria-selected="false" tabindex="-1">Tab 2</button>
</div>
<div class="scrollable-indicators__indicator scrollable-indicators__indicator_start"></div>
<div class="scrollable-indicators__indicator scrollable-indicators__indicator_end"></div>
</div>
<div class="flexi-tabs__tab-panel" tabindex="0" role="tabpanel" aria-label="Tab 1">
<p>Panel 1</p>
</div>
<div class="flexi-tabs__tab-panel flexi-tabs__tab-panel_hidden" tabindex="0" role="tabpanel" aria-label="Tab 2">
<p>Panel 2</p>
</div>
</div>
````````````````````````````````

! Generated elements are assigned classes that comply with [BEM methodology](https://en.bem.info/). These classes can be customized. We explain how in [options].

Panel parts can contain markdown blocks such as code blocks, lists and ATX headings. Tab parts can only contain inline markdown
such as text with empasis ([Commonmark - Blocks and inlines](https://spec.commonmark.org/0.28/#blocks-and-inlines)):

```````````````````````````````` none
--------------- Extra Extensions ---------------
FlexiCodeBlocks
--------------- Markdown ---------------
///
+++ tab
*Tab 1*
+++
- Panel 1
+++

+++ tab
**Tab 2**
+++
```
Panel 2
```
+++
///
--------------- Expected Markup ---------------
<div class="flexi-tabs">
<div class="flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal">
<div class="flexi-tabs__tab-list scrollable-indicators__scrollable" role="tablist">
<button class="flexi-tabs__tab flexi-tabs__tab_selected" role="tab" aria-selected="true"><em>Tab 1</em></button>
<button class="flexi-tabs__tab" role="tab" aria-selected="false" tabindex="-1"><strong>Tab 2</strong></button>
</div>
<div class="scrollable-indicators__indicator scrollable-indicators__indicator_start"></div>
<div class="scrollable-indicators__indicator scrollable-indicators__indicator_end"></div>
</div>
<div class="flexi-tabs__tab-panel" tabindex="0" role="tabpanel" aria-label="Tab 1">
<ul>
<li>Panel 1</li>
</ul>
</div>
<div class="flexi-tabs__tab-panel flexi-tabs__tab-panel_hidden" tabindex="0" role="tabpanel" aria-label="Tab 2">
<div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
<header class="flexi-code__header">
<button class="flexi-code__copy-button" aria-label="Copy code">
<svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
</button>
</header>
<pre class="flexi-code__pre"><code class="flexi-code__code">Panel 2
</code></pre>
</div>
</div>
</div>
````````````````````````````````

## Options
### `FlexiTabsBlockOptions`
Options for a FlexiTabsBlock. To specify `FlexiTabsBlockOptions` for a FlexiTabsBlock, the [Options](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiOptionsBlocksSpecs.md#options) extension must be enabled.

#### Properties

##### `BlockName`
- Type: `string`
- Description: The `FlexiTabsBlock`'s [BEM block name](https://en.bem.info/methodology/naming-convention/#block-name).
  In compliance with [BEM methodology](https://en.bem.info), this value is the `FlexiTabsBlock`'s root element's class as well as the prefix for all other classes in the block.
  This value should contain only valid [CSS class characters](https://www.w3.org/TR/CSS21/syndata.html#characters).
  If this value is `null`, whitespace or an empty string, the `FlexiTabsBlock`'s block name is "flexi-tabs".
- Default: "flexi-tabs"
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  o{ "blockName": "tabs" }
  ///
  +++ tab
  Tab 1
  +++
  Panel 1
  +++
  ///
  --------------- Expected Markup ---------------
  <div class="tabs">
  <div class="tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal">
  <div class="tabs__tab-list scrollable-indicators__scrollable" role="tablist">
  <button class="tabs__tab tabs__tab_selected" role="tab" aria-selected="true">Tab 1</button>
  </div>
  <div class="scrollable-indicators__indicator scrollable-indicators__indicator_start"></div>
  <div class="scrollable-indicators__indicator scrollable-indicators__indicator_end"></div>
  </div>
  <div class="tabs__tab-panel" tabindex="0" role="tabpanel" aria-label="Tab 1">
  <p>Panel 1</p>
  </div>
  </div>
  ````````````````````````````````

##### `DefaultTabOptions`
- Type: `IFlexiTabBlockOptions`
- Description: The default `IFlexiTabBlockOptions` for contained `FlexiTabBlock`s.
  If this value is `null`, a `FlexiTabBlockOptions` with default values is used.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  o{ 
      "defaultTabOptions": {
          "attributes": {
              "class" : "my-custom-class"
          }
      }
  }
  ///
  +++ tab
  Tab 1
  +++
  Panel 1
  +++
  ///
  --------------- Expected Markup ---------------
  <div class="flexi-tabs">
  <div class="flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal">
  <div class="flexi-tabs__tab-list scrollable-indicators__scrollable" role="tablist">
  <button class="flexi-tabs__tab flexi-tabs__tab_selected" role="tab" aria-selected="true">Tab 1</button>
  </div>
  <div class="scrollable-indicators__indicator scrollable-indicators__indicator_start"></div>
  <div class="scrollable-indicators__indicator scrollable-indicators__indicator_end"></div>
  </div>
  <div class="flexi-tabs__tab-panel my-custom-class" tabindex="0" role="tabpanel" aria-label="Tab 1">
  <p>Panel 1</p>
  </div>
  </div>
  ````````````````````````````````
  `defaultTabOptions` has lower precedence than tab specific options:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  o{ 
      "defaultTabOptions": {
          "attributes": {
              "class" : "my-custom-class"
          }
      }
  }
  ///
  +++ tab
  Tab 1
  +++
  Panel 1
  +++

  o{ 
      "attributes": {
          "class" : "alt-custom-class"
      }
  }
  +++ tab
  Tab 2
  +++
  Panel 2
  +++
  ///
  --------------- Expected Markup ---------------
  <div class="flexi-tabs">
  <div class="flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal">
  <div class="flexi-tabs__tab-list scrollable-indicators__scrollable" role="tablist">
  <button class="flexi-tabs__tab flexi-tabs__tab_selected" role="tab" aria-selected="true">Tab 1</button>
  <button class="flexi-tabs__tab" role="tab" aria-selected="false" tabindex="-1">Tab 2</button>
  </div>
  <div class="scrollable-indicators__indicator scrollable-indicators__indicator_start"></div>
  <div class="scrollable-indicators__indicator scrollable-indicators__indicator_end"></div>
  </div>
  <div class="flexi-tabs__tab-panel my-custom-class" tabindex="0" role="tabpanel" aria-label="Tab 1">
  <p>Panel 1</p>
  </div>
  <div class="flexi-tabs__tab-panel flexi-tabs__tab-panel_hidden alt-custom-class" tabindex="0" role="tabpanel" aria-label="Tab 2">
  <p>Panel 2</p>
  </div>
  </div>
  ````````````````````````````````

##### `Attributes`
- Type: `IDictionary<string, string>`
- Description: The HTML attributes for the `FlexiTabsBlock`'s root element.
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
  ///
  +++ tab
  Tab 1
  +++
  Panel 1
  +++
  ///
  --------------- Expected Markup ---------------
  <div class="flexi-tabs my-custom-class" id="my-custom-id">
  <div class="flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal">
  <div class="flexi-tabs__tab-list scrollable-indicators__scrollable" role="tablist">
  <button class="flexi-tabs__tab flexi-tabs__tab_selected" role="tab" aria-selected="true">Tab 1</button>
  </div>
  <div class="scrollable-indicators__indicator scrollable-indicators__indicator_start"></div>
  <div class="scrollable-indicators__indicator scrollable-indicators__indicator_end"></div>
  </div>
  <div class="flexi-tabs__tab-panel" tabindex="0" role="tabpanel" aria-label="Tab 1">
  <p>Panel 1</p>
  </div>
  </div>
  ````````````````````````````````

### `FlexiTabBlockOptions`
Options for a `FlexiTabBlock`.

#### Properties

##### `Attributes`
- Type: `IDictionary<string, string>`
- Description: The HTML attributes for the `FlexiTabBlock`'s root element.
  Attribute names must be lowercase.
  If the class attribute is specified, its value is appended to default classes. This facilitates [BEM mixes](https://en.bem.info/methodology/quick-start/#mix).
  If this value is `null`, default classes are still assigned to the root element.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Markdown ---------------
  ///
  o{
      "attributes": {
          "id" : "my-custom-id",
          "class" : "my-custom-class"
      }
  }
  +++ tab
  Tab 1
  +++
  Panel 1
  +++
  ///
  --------------- Expected Markup ---------------
  <div class="flexi-tabs">
  <div class="flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal">
  <div class="flexi-tabs__tab-list scrollable-indicators__scrollable" role="tablist">
  <button class="flexi-tabs__tab flexi-tabs__tab_selected" role="tab" aria-selected="true">Tab 1</button>
  </div>
  <div class="scrollable-indicators__indicator scrollable-indicators__indicator_start"></div>
  <div class="scrollable-indicators__indicator scrollable-indicators__indicator_end"></div>
  </div>
  <div class="flexi-tabs__tab-panel my-custom-class" id="my-custom-id" tabindex="0" role="tabpanel" aria-label="Tab 1">
  <p>Panel 1</p>
  </div>
  </div>
  ````````````````````````````````

### `FlexiTabsBlocksExtensionOptions`
Options for the FlexiTabsBlocks extension. There are two ways to specify these options:
- Pass a `FlexiTabsBlocksExtensionOptions` when calling `MarkdownPipelineBuilderExtensions.UseFlexiTabsBlocks(this MarkdownPipelineBuilder pipelineBuilder, IFlexiTabsBlocksExtensionOptions options)`.
- Insert a `FlexiTabsBlocksExtensionOptions` into a `MarkdownParserContext.Properties` with key `typeof(IFlexiTabsBlocksExtensionOptions)`. Pass the `MarkdownParserContext` when you call a markdown processing method
  like `Markdown.ToHtml(markdown, stringWriter, markdownPipeline, yourMarkdownParserContext)`.  
  This method allows for different extension options when reusing a pipeline. Options specified using this method take precedence.

#### Constructor Parameters

##### `defaultBlockOptions`
- Type: `IFlexiTabsBlockOptions`
- Description: Default `IFlexiTabsBlockOptions` for all `FlexiTabsBlock`s.
  If this value is `null`, a `FlexiTabsBlockOptions` with default values is used.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extension Options ---------------
  {
      "flexiTabsBlocks": {
          "defaultBlockOptions": {
              "defaultTabOptions": {
                "attributes": {
                    "class" : "tab-class"
                }
              },
              "attributes": {
                  "class": "tabs-class"
              }
          }
      }
  }
  --------------- Markdown ---------------
  ///
  +++ tab
  Tab 1
  +++
  Panel 1
  +++
  ///
  --------------- Expected Markup ---------------
  <div class="flexi-tabs tabs-class">
  <div class="flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal">
  <div class="flexi-tabs__tab-list scrollable-indicators__scrollable" role="tablist">
  <button class="flexi-tabs__tab flexi-tabs__tab_selected" role="tab" aria-selected="true">Tab 1</button>
  </div>
  <div class="scrollable-indicators__indicator scrollable-indicators__indicator_start"></div>
  <div class="scrollable-indicators__indicator scrollable-indicators__indicator_end"></div>
  </div>
  <div class="flexi-tabs__tab-panel tab-class" tabindex="0" role="tabpanel" aria-label="Tab 1">
  <p>Panel 1</p>
  </div>
  </div>
  ````````````````````````````````
  `defaultBlockOptions` has lower precedence than block specific options:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiOptionsBlocks
  --------------- Extension Options ---------------
  {
      "flexiTabsBlocks": {
          "defaultBlockOptions": {
              "defaultTabOptions": {
                  "attributes": {
                      "class" : "tab-class"
                  }
              },
              "attributes": {
                  "class": "tabs-class"
              }
          }
      }
  }
  --------------- Markdown ---------------
  ///
  o{              
      "attributes": {
          "class": "alt-tab-class"
      }
  }
  +++ tab
  Tab 1
  +++
  Panel 1
  +++
  ///

  o{              
      "attributes": {
          "class": "alt-tabs-class"
      }
  }
  ///
  +++ tab
  Tab 2
  +++
  Panel 2
  +++
  ///
  --------------- Expected Markup ---------------
  <div class="flexi-tabs tabs-class">
  <div class="flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal">
  <div class="flexi-tabs__tab-list scrollable-indicators__scrollable" role="tablist">
  <button class="flexi-tabs__tab flexi-tabs__tab_selected" role="tab" aria-selected="true">Tab 1</button>
  </div>
  <div class="scrollable-indicators__indicator scrollable-indicators__indicator_start"></div>
  <div class="scrollable-indicators__indicator scrollable-indicators__indicator_end"></div>
  </div>
  <div class="flexi-tabs__tab-panel alt-tab-class" tabindex="0" role="tabpanel" aria-label="Tab 1">
  <p>Panel 1</p>
  </div>
  </div>
  <div class="flexi-tabs alt-tabs-class">
  <div class="flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal">
  <div class="flexi-tabs__tab-list scrollable-indicators__scrollable" role="tablist">
  <button class="flexi-tabs__tab flexi-tabs__tab_selected" role="tab" aria-selected="true">Tab 2</button>
  </div>
  <div class="scrollable-indicators__indicator scrollable-indicators__indicator_start"></div>
  <div class="scrollable-indicators__indicator scrollable-indicators__indicator_end"></div>
  </div>
  <div class="flexi-tabs__tab-panel tab-class" tabindex="0" role="tabpanel" aria-label="Tab 2">
  <p>Panel 2</p>
  </div>
  </div>
  ````````````````````````````````

## Mechanics
### Nesting FlexiTabsBlocks
FlexiTabBlock opening and closing fences can contain any number of `/` characters as long as there are at least 3.
The number of characters in each opening/closing pair of fences must match. To nest a FlexiTabsBlock, use
fences with a different number of `/` characters than the containing FlexiTabsBlock:
```````````````````````````````` none
--------------- Markdown ---------------
///
+++ tab
Tab 1
+++
////
+++ tab
Nested tab
+++
Nested panel
+++
////
+++
///
--------------- Expected Markup ---------------
<div class="flexi-tabs">
<div class="flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal">
<div class="flexi-tabs__tab-list scrollable-indicators__scrollable" role="tablist">
<button class="flexi-tabs__tab flexi-tabs__tab_selected" role="tab" aria-selected="true">Tab 1</button>
</div>
<div class="scrollable-indicators__indicator scrollable-indicators__indicator_start"></div>
<div class="scrollable-indicators__indicator scrollable-indicators__indicator_end"></div>
</div>
<div class="flexi-tabs__tab-panel" tabindex="0" role="tabpanel" aria-label="Tab 1">
<div class="flexi-tabs">
<div class="flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal">
<div class="flexi-tabs__tab-list scrollable-indicators__scrollable" role="tablist">
<button class="flexi-tabs__tab flexi-tabs__tab_selected" role="tab" aria-selected="true">Nested tab</button>
</div>
<div class="scrollable-indicators__indicator scrollable-indicators__indicator_start"></div>
<div class="scrollable-indicators__indicator scrollable-indicators__indicator_end"></div>
</div>
<div class="flexi-tabs__tab-panel" tabindex="0" role="tabpanel" aria-label="Nested tab">
<p>Nested panel</p>
</div>
</div>
</div>
</div>
````````````````````````````````

