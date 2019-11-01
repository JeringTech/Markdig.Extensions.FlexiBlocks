# FlexiOptionsBlocks
A FlexiOptionsBlock contains options for another block.  

We often require per-block options when working with markdown. For example, we might want line numbers for one code block but not another.
Attempts have been made to facilitate this, such as using [query string](https://github.com/middleman/middleman-syntax#markdown) like syntax:

````
``` javascript?line_numbers=false
function exampleFunction(arg) {
    return arg + 'dummyString';
}
```
````

or [custom syntaxes](https://michelf.ca/projects/php-markdown/extra/#spe-attr):
```
## Le Site ##    {.main .shine #the-site lang=fr}
```

These existing solutions work, but they require custom parsing logic and are typically specific to one kind of block.  

You can use FlexiOptionsBlocks to specify per-block options for all kinds of blocks. We designed FlexiOptionsBlocks with the following goals:

- Easy to learn and remember: A FlexiOptionsBlock is just JSON prepended with `o`.
- Easy to add to existing blocks: Enabling OptionBlocks for a block requires little more than defining a simple options type to deserialize the JSON to.

## Basics
In markdown, a FlexiOptionsBlock is JSON prepended with `o`. A FlexiOptionsBlock must immediately precede the block it applies to. Its first line must begin with `o{`, whitespace is not 
allowed between the opening `o` and `{`.

The following is an example FlexiOptionsBlock. Here, we use it to specify a title for a 
[FlexiCodeBlock](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiCodeBlocksSpecs.md):
```````````````````````````````` none
--------------- Extra Extensions ---------------
FlexiCodeBlocks
--------------- Markdown ---------------
o{ "title": "ExampleDocument.cs" }
```
public string ExampleFunction(string arg)
{
    // Example comment
    return arg + "dummyString";
}
```
--------------- Expected Markup ---------------
<div class="flexi-code flexi-code_has-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
<header class="flexi-code__header">
<span class="flexi-code__title">ExampleDocument.cs</span>
<button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
<svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
</button>
</header>
<pre class="flexi-code__pre"><code class="flexi-code__code">public string ExampleFunction(string arg)
{
    // Example comment
    return arg + &quot;dummyString&quot;;
}
</code></pre>
</div>
````````````````````````````````

The JSON can span any number of lines, as long as it is valid. The following is a FlexiOptionsBlock for a
[FlexiAlertBlock](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiAlertBlocksSpecs.md): 
```````````````````````````````` none
--------------- Extra Extensions ---------------
FlexiAlertBlocks
--------------- Markdown ---------------
o{
    "type": "warning"
}
! This is a FlexiAlertBlock.
--------------- Expected Markup ---------------
<div class="flexi-alert flexi-alert_type_warning flexi-alert_has-icon">
<svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"/></svg>
<div class="flexi-alert__content">
<p>This is a FlexiAlertBlock.</p>
</div>
</div>
````````````````````````````````

Options types for FlexiBlocks have an `Attributes` property of type `IDictionary<string, string>`. Key-value
pairs in this dictionary are assigned to outermost elements as attributes. The following is a FlexiOptionsBlock for a
[FlexiTableBlock](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiTableBlocksSpecs.md): 
```````````````````````````````` none
--------------- Extra Extensions ---------------
FlexiTableBlocks
--------------- Markdown ---------------
o{
    "attributes": {
        "id" : "table-1"
    }
}
+---+---+
| a | b |
+===+===+
| 0 | 1 |
+---+---+
| 2 | 3 |
--------------- Expected Markup ---------------
<div class="flexi-table flexi-table_type_cards" id="table-1">
<table class="flexi-table__table">
<thead class="flexi-table__head">
<tr class="flexi-table__row">
<th class="flexi-table__header">
a
</th>
<th class="flexi-table__header">
b
</th>
</tr>
</thead>
<tbody class="flexi-table__body">
<tr class="flexi-table__row">
<td class="flexi-table__data">
<div class="flexi-table__label">
a
</div>
<div class="flexi-table__content">
0
</div>
</td>
<td class="flexi-table__data">
<div class="flexi-table__label">
b
</div>
<div class="flexi-table__content">
1
</div>
</td>
</tr>
<tr class="flexi-table__row">
<td class="flexi-table__data">
<div class="flexi-table__label">
a
</div>
<div class="flexi-table__content">
2
</div>
</td>
<td class="flexi-table__data">
<div class="flexi-table__label">
b
</div>
<div class="flexi-table__content">
3
</div>
</td>
</tr>
</tbody>
</table>
</div>
````````````````````````````````

All FlexiBlocks extensions allow for default per-block options. These can be specified when registering extensions.
For example, when registering the [FlexiSectionBlocks](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiSectionBlocksSpecs.md) extension,
you can specify a [FlexiSectionBlocksExtensionOptions](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiSectionBlocksSpecs.md#flexisectionblocksextensionoptions)
instance:

``` 
MyMarkdownPipelineBuilder.UseFlexiSectionBlocks(myFlexiSectionBlocksExtensionOptions);
```

The extension options instance contains a default [FlexiSectionBlockOptions](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiSectionBlocksSpecs.md#flexisectionblockoptions) instance.
The following is a FlexiOptionsBlock for a [FlexiSectionBlock](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiSectionBlocksSpecs.md): 
```````````````````````````````` none
--------------- Extra Extensions ---------------
FlexiSectionBlocks
--------------- Extension Options ---------------
{
    "flexiSectionBlocks": {
        "defaultBlockOptions": {
            "element": "nav"
        }
    }
}
--------------- Markdown ---------------
# foo

o{
    "element": "article"
}
# foo
--------------- Expected Markup ---------------
<nav class="flexi-section flexi-section_level_1 flexi-section_has-link-icon" id="foo">
<header class="flexi-section__header">
<h1 class="flexi-section__heading">foo</h1>
<button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
<svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
</button>
</header>
</nav>
<article class="flexi-section flexi-section_level_1 flexi-section_has-link-icon" id="foo-1">
<header class="flexi-section__header">
<h1 class="flexi-section__heading">foo</h1>
<button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
<svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
</button>
</header>
</article>
````````````````````````````````
