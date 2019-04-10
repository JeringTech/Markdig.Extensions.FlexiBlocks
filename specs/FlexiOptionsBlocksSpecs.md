# FlexiOptionsBlocks
FlexiOptionsBlocks contain options for other blocks.  

When working with markdown, there is often a need for per-block options.
Attempts have been made to facilitate this, such as using 
a [query string](https://github.com/middleman/middleman-syntax#markdown) like syntax:

````
``` javascript?line_numbers=false
function exampleFunction(arg) {
    return arg + 'dummyString';
}
```
````

or a [custom syntax](https://michelf.ca/projects/php-markdown/extra/#spe-attr):
```
## Le Site ##    {.main .shine #the-site lang=fr}
```

Existing solutions work, but they require custom parsing logic and are typically specific to one kind of block.

The FlexiOptionsBlocks extension provides a consistent way to specify per-block options for all kinds of blocks. It is designed with the following goals:

- Easy to learn and remember: Syntactically, a FlexiOptionsBlock is simply JSON prepended with `@`.
- Easy to add to existing markdown extensions: Enabling FlexiOptionBlocks for an extension requires little more than defining a
  simple options type to deserialize the JSON to.

## Basic Syntax
A FlexiOptionsBlock is simply JSON prepended with `@`. It must immediately precede the block it applies to and
its first line must begin with `@{`.

The following is a FlexiOptionsBlock for a 
[FlexiCodeBlock](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiCodeBlocksSpecs.md):
```````````````````````````````` none
--------------- Extra Extensions ---------------
FlexiCodeBlocks
--------------- Markdown ---------------
@{ "title": "ExampleDocument.cs" }
```
public string ExampleFunction(string arg)
{
    // Example comment
    return arg + "dummyString";
}
```
--------------- Expected Markup ---------------
<div class="flexi-code-block">
<header>
<span>ExampleDocument.cs</span>
<button>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
</button>
</header>
<pre><code><span class="line"><span class="line-text">public string ExampleFunction(string arg)</span></span>
<span class="line"><span class="line-text">{</span></span>
<span class="line"><span class="line-text">    // Example comment</span></span>
<span class="line"><span class="line-text">    return arg + &quot;dummyString&quot;;</span></span>
<span class="line"><span class="line-text">}</span></span></code></pre>
</div>
````````````````````````````````

The JSON can span any number of lines, as long as it is valid. The following is a FlexiOptionsBlock for a
[FlexiAlertBlock](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiAlertBlocksSpecs.md): 
```````````````````````````````` none
--------------- Extra Extensions ---------------
FlexiAlertBlocks
--------------- Markdown ---------------
@{
    "type": "warning"
}
! This is a FlexiAlertBlock.
--------------- Expected Markup ---------------
<div class="flexi-alert-block flexi-alert-block_warning">
<svg class="flexi-alert-block__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"/></svg>
<div class="flexi-alert-block__content">
<p>This is a FlexiAlertBlock.</p>
</div>
</div>
````````````````````````````````

All FlexiBlocks options types have an `Attributes` property of type `IDictionary<string, string>`. Key-value
pairs in this dictionary are assigned to outermost elements as attributes. The following is a FlexiOptionsBlock for a
[FlexiTableBlock](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiTableBlocksSpecs.md): 
```````````````````````````````` none
--------------- Extra Extensions ---------------
FlexiTableBlocks
--------------- Markdown ---------------
@{
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
<div id="table-1" class="flexi-table-block">
<table>
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
</div>
````````````````````````````````

All FlexiBlocks extensions allow for default per-block options. These can be specified when registering extensions.
For example, when registering the FlexiSectionsBlocks extension, you can specify a [FlexiSectionBlocksExtensionOptions](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiSectionBlocksSpecs.md#flexisectionblocksextensionoptions)
instance:

``` 
MyMarkdownPipelineBuilder.UseFlexiSectionBlocks(myFlexiSectionBlocksExtensionOptions);
```
The extension options instance contains a default FlexiSectionBlockOptions instance.
The following is a FlexiOptionsBlock for a
[FlexiSectionBlock](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiSectionBlocksSpecs.md): 
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

@{
    "element": "article"
}
# foo
--------------- Expected Markup ---------------
<nav class="flexi-section-block-1" id="foo">
<header>
<h1>foo</h1>
<button>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
</button>
</header>
</nav>
<article class="flexi-section-block-1" id="foo-1">
<header>
<h1>foo</h1>
<button>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
</button>
</header>
</article>
````````````````````````````````
