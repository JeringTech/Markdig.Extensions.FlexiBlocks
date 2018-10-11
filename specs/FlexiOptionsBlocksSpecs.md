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
[FlexiCodeBlock](https://github.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiCodeBlocksSpecs.md):
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
<div class="fcb">
<header>
<span>ExampleDocument.cs</span>
<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
</header>
<pre><code>public string ExampleFunction(string arg)
{
    // Example comment
    return arg + &quot;dummyString&quot;;
}</code></pre>
</div>
````````````````````````````````

The JSON can span any number of lines, as long as it is valid. The following is a FlexiOptionsBlock for a
[FlexiAlertBlock](https://github.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiAlertBlocksSpecs.md): 
```````````````````````````````` none
--------------- Extra Extensions ---------------
FlexiAlertBlocks
--------------- Markdown ---------------
@{
    "type": "warning"
}
! This is a FlexiAlertBlock.
--------------- Expected Markup ---------------
<div class="flexi-alert-block-warning">
<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="m1 21h22l-11-19-11 19zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"/></svg>
<div class="flexi-alert-block-content">
<p>This is a FlexiAlertBlock.</p>
</div>
</div>
````````````````````````````````

All FlexiBlocks options types have an `Attributes` property of type `IDictionary<string, string>`. Key-value
pairs in this dictionary are assigned to outermost elements as attributes. The following is a FlexiOptionsBlock for a
[FlexiTableBlock](https://github.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiTableBlocksSpecs.md): 
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
<table id="table-1">
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

All FlexiBlocks extensions allow for default per-block options. These can be specified when registering extensions.
For example, when registering the FlexiSectionsBlocks extension, you can specify a [FlexiSectionBlocksExtensionOptions](https://github.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiSectionBlocksSpecs.md#flexisectionblocksextensionoptions)
instance:

``` 
MyMarkdownPipelineBuilder.UseFlexiSectionBlocks(myFlexiSectionBlocksExtensionOptions);
```
The extension options instance contains a default FlexiSectionBlockOptions instance.
The following is a FlexiOptionsBlock for a
[FlexiSectionBlock](https://github.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiSectionBlocksSpecs.md): 
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
<nav class="section-level-1" id="foo">
<header>
<h1>foo</h1>
<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M17 7h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8zm9-4h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8z"/></svg>
</header>
</nav>
<article class="section-level-1" id="foo-1">
<header>
<h1>foo</h1>
<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M17 7h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8zm9-4h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8z"/></svg>
</header>
</article>
````````````````````````````````
