# FlexiTableBlocks
FlexiTableBlocks are [pipe tables](https://github.com/lunet-io/markdig/blob/master/src/Markdig.Tests/Specs/PipeTableSpecs.md) or [grid tables](https://github.com/lunet-io/markdig/blob/master/src/Markdig.Tests/Specs/GridTableSpecs.md)
that are rendered for compatibility with the [flexible card-style responsive tables method](https://www.jeremytcd.com/articles/css-only-responsive-tables).

## Basic Syntax
The following is a pipe table FlexiTableBlock:
```````````````````````````````` none
--------------- Markdown ---------------
a | b
- | -
0 | 1
2 | 3
--------------- Expected Markup ---------------
<table class="flexi-table-block">
<thead>
<tr>
<th>a</th>
<th>b</th>
</tr>
</thead>
<tbody>
<tr>
<td data-label="a"><span>0</span></td>
<td data-label="b"><span>1</span></td>
</tr>
<tr>
<td data-label="a"><span>2</span></td>
<td data-label="b"><span>3</span></td>
</tr>
</tbody>
</table>
````````````````````````````````

The following is a grid table FlexiTableBlock:
```````````````````````````````` none
--------------- Markdown ---------------
+---+---+
| a | b |
+===+===+
| 0 | 1 |
+---+---+
| 2 | 3 |
--------------- Expected Markup ---------------
<table class="flexi-table-block">
<col style="width:50%">
<col style="width:50%">
<thead>
<tr>
<th>a</th>
<th>b</th>
</tr>
</thead>
<tbody>
<tr>
<td data-label="a"><span>0</span></td>
<td data-label="b"><span>1</span></td>
</tr>
<tr>
<td data-label="a"><span>2</span></td>
<td data-label="b"><span>3</span></td>
</tr>
</tbody>
</table>
````````````````````````````````

## Options

### `FlexiTableBlockOptions`
Options for a FlexiTableBlock. To specify FlexiTableBlockOptions for a FlexiTableBlock, the 
[FlexiOptionsBlocks](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiOptionsBlocksSpecs.md#flexioptionsblocks) extension must be enabled.
Specifying FlexiTableBlockOptions using the FlexiOptionsBlocks extension only works for grid table FlexiTableBlocks.
To specify default FlexiTableBlockOptions for all FlexiTableBlocks, use [FlexiTableBlocksExtensionOptions](#flexitableblocksextensionoptions).

#### Properties
- `Class`
  - Type: `string`
  - Description: The FlexiTableBlock's outermost element's class. If this value is null, whitespace or an empty string, no class is assigned.
  - Default: "flexi-table-block"
  - Usage:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiOptionsBlocks
    --------------- Markdown ---------------
    @{
        "class": "alternative-class"
    }
    +---+---+
    | a | b |
    +===+===+
    | 0 | 1 |
    +---+---+
    | 2 | 3 |
    --------------- Expected Markup ---------------
    <table class="alternative-class">
    <col style="width:50%">
    <col style="width:50%">
    <thead>
    <tr>
    <th>a</th>
    <th>b</th>
    </tr>
    </thead>
    <tbody>
    <tr>
    <td data-label="a"><span>0</span></td>
    <td data-label="b"><span>1</span></td>
    </tr>
    <tr>
    <td data-label="a"><span>2</span></td>
    <td data-label="b"><span>3</span></td>
    </tr>
    </tbody>
    </table>
    ````````````````````````````````

- `WrapperElement`
  - Type: `string`
  - Description: The element that will wrap td contents.
    If this value is null, whitespace or an empty string, no wrapper element is rendered.
  - Default: "span", for ARIA compatibility - https://www.w3.org/TR/2017/NOTE-wai-aria-practices-1.1-20171214/examples/table/table.html
  - Usage:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiOptionsBlocks
    --------------- Markdown ---------------
    @{
        "wrapperElement": "div"
    }
    +---+---+
    | a | b |
    +===+===+
    | 0 | 1 |
    +---+---+
    | 2 | 3 |
    --------------- Expected Markup ---------------
    <table class="flexi-table-block">
    <col style="width:50%">
    <col style="width:50%">
    <thead>
    <tr>
    <th>a</th>
    <th>b</th>
    </tr>
    </thead>
    <tbody>
    <tr>
    <td data-label="a"><div>0</div></td>
    <td data-label="b"><div>1</div></td>
    </tr>
    <tr>
    <td data-label="a"><div>2</div></td>
    <td data-label="b"><div>3</div></td>
    </tr>
    </tbody>
    </table>
    ````````````````````````````````

- `LabelAttribute`
  - Type: `string`
  - Description: The td attribute used to store its corresponding th's contents.
    If this value is null, whitespace or an empty string, no attribute is rendered.
  - Default: "data-label"
  - Usage:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiOptionsBlocks
    --------------- Markdown ---------------
    @{
        "labelAttribute": "data-header-content"
    }
    +---+---+
    | a | b |
    +===+===+
    | 0 | 1 |
    +---+---+
    | 2 | 3 |
    --------------- Expected Markup ---------------
    <table class="flexi-table-block">
    <col style="width:50%">
    <col style="width:50%">
    <thead>
    <tr>
    <th>a</th>
    <th>b</th>
    </tr>
    </thead>
    <tbody>
    <tr>
    <td data-header-content="a"><span>0</span></td>
    <td data-header-content="b"><span>1</span></td>
    </tr>
    <tr>
    <td data-header-content="a"><span>2</span></td>
    <td data-header-content="b"><span>3</span></td>
    </tr>
    </tbody>
    </table>
    ````````````````````````````````

- `Attributes`
  - Type: `IDictionary<string, string>`
  - Description: The HTML attributes for the FlexiTableBlock's outermost element.
    If this value is null, no attributes will be assigned to the outermost element.
  - Default: `null`
  - Usage:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiOptionsBlocks
    --------------- Markdown ---------------
    @{
        "attributes": {
            "id" : "table-1",
            "class" : "block"
        }
    }
    +---+---+
    | a | b |
    +===+===+
    | 0 | 1 |
    +---+---+
    | 2 | 3 |
    --------------- Expected Markup ---------------
    <table id="table-1" class="block flexi-table-block">
    <col style="width:50%">
    <col style="width:50%">
    <thead>
    <tr>
    <th>a</th>
    <th>b</th>
    </tr>
    </thead>
    <tbody>
    <tr>
    <td data-label="a"><span>0</span></td>
    <td data-label="b"><span>1</span></td>
    </tr>
    <tr>
    <td data-label="a"><span>2</span></td>
    <td data-label="b"><span>3</span></td>
    </tr>
    </tbody>
    </table>
    ````````````````````````````````
    If a value is specified for the class attribute, it will not override the outermost element's generated class. Instead, it will be 
    prepended to the generated class. In the above example, this results in the outermost element's class attribute having the value 
    `block flexi-table-block`.

### `FlexiTableBlocksExtensionOptions`
Global options for FlexiTableBlocks. These options can be used to define defaults for all FlexiTableBlocks. They have
lower precedence than block specific options specified using the FlexiOptionsBlocks extension.  

FlexiTableBlocksExtensionOptions can be specified when enabling the FlexiTableBlocks extension:
``` 
MyMarkdownPipelineBuilder.UseFlexiTableBlocks(myFlexiTableBlocksExtensionOptions);
```

#### Properties
- `DefaultBlockOptions`
  - Type: `FlexiSectionBlockOptions`
  - Description: Default `FlexiSectionBlockOptions` for all FlexiSectionBlocks. 
  - Usage:
    ```````````````````````````````` none
    --------------- Extension Options ---------------
    {
        "flexiTableBlocks": {
            "defaultBlockOptions": {
                "class": "alternative-class",
                "wrapperElement": "div",
                "labelAttribute": "data-header-content",
                "attributes": {
                    "class": "block"
                }
            }
        }
    }
    --------------- Markdown ---------------
    +---+---+
    | a | b |
    +===+===+
    | 0 | 1 |
    +---+---+
    | 2 | 3 |  

    a | b
    - | -
    0 | 1
    2 | 3
    --------------- Expected Markup ---------------
    <table class="block alternative-class">
    <col style="width:50%">
    <col style="width:50%">
    <thead>
    <tr>
    <th>a</th>
    <th>b</th>
    </tr>
    </thead>
    <tbody>
    <tr>
    <td data-header-content="a"><div>0</div></td>
    <td data-header-content="b"><div>1</div></td>
    </tr>
    <tr>
    <td data-header-content="a"><div>2</div></td>
    <td data-header-content="b"><div>3</div></td>
    </tr>
    </tbody>
    </table>
    <table class="block alternative-class">
    <thead>
    <tr>
    <th>a</th>
    <th>b</th>
    </tr>
    </thead>
    <tbody>
    <tr>
    <td data-header-content="a"><div>0</div></td>
    <td data-header-content="b"><div>1</div></td>
    </tr>
    <tr>
    <td data-header-content="a"><div>2</div></td>
    <td data-header-content="b"><div>3</div></td>
    </tr>
    </tbody>
    </table>
    ````````````````````````````````

    Default FlexiTableBlockOptions have lower precedence than block specific options:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiOptionsBlocks
    --------------- Extension Options ---------------
    {
        "flexiTableBlocks": {
            "defaultBlockOptions": {
                "wrapperElement": "div"
            }
        }
    }
    --------------- Markdown ---------------
    a | b
    - | -
    0 | 1
    2 | 3

    @{
        "wrapperElement": "span"
    }
    +---+---+
    | a | b |
    +===+===+
    | 0 | 1 |
    +---+---+
    | 2 | 3 |  
    --------------- Expected Markup ---------------
    <table class="flexi-table-block">
    <thead>
    <tr>
    <th>a</th>
    <th>b</th>
    </tr>
    </thead>
    <tbody>
    <tr>
    <td data-label="a"><div>0</div></td>
    <td data-label="b"><div>1</div></td>
    </tr>
    <tr>
    <td data-label="a"><div>2</div></td>
    <td data-label="b"><div>3</div></td>
    </tr>
    </tbody>
    </table>
    <table class="flexi-table-block">
    <col style="width:50%">
    <col style="width:50%">
    <thead>
    <tr>
    <th>a</th>
    <th>b</th>
    </tr>
    </thead>
    <tbody>
    <tr>
    <td data-label="a"><span>0</span></td>
    <td data-label="b"><span>1</span></td>
    </tr>
    <tr>
    <td data-label="a"><span>2</span></td>
    <td data-label="b"><span>3</span></td>
    </tr>
    </tbody>
    </table>
    ````````````````````````````````

## Mechanics

The contents of `<th>` elements are HTML escaped when used as values of label attributes:

```````````````````````````````` none
--------------- Markdown ---------------
"a" | &b&
- | - 
0 | 1 
2 | 3 
--------------- Expected Markup ---------------
<table class="flexi-table-block">
<thead>
<tr>
<th>&quot;a&quot;</th>
<th>&amp;b&amp;</th>
</tr>
</thead>
<tbody>
<tr>
<td data-label="&quot;a&quot;"><span>0</span></td>
<td data-label="&amp;b&amp;"><span>1</span></td>
</tr>
<tr>
<td data-label="&quot;a&quot;"><span>2</span></td>
<td data-label="&amp;b&amp;"><span>3</span></td>
</tr>
</tbody>
</table>
````````````````````````````````

HTML tags are removed from the contents of `<th>` elements when such contents are used as values of label attributes:

```````````````````````````````` none
--------------- Markdown ---------------
+---+---+
| a | b |
|   |   |
| a |   |
+===+===+
| 0 | 1 |
+---+---+
| 2 | 3 |
--------------- Expected Markup ---------------
<table class="flexi-table-block">
<col style="width:50%">
<col style="width:50%">
<thead>
<tr>
<th><p>a</p>
<p>a</p>
</th>
<th>b</th>
</tr>
</thead>
<tbody>
<tr>
<td data-label="aa"><span>0</span></td>
<td data-label="b"><span>1</span></td>
</tr>
<tr>
<td data-label="aa"><span>2</span></td>
<td data-label="b"><span>3</span></td>
</tr>
</tbody>
</table>
````````````````````````````````