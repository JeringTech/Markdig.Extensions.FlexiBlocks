---
blockOptions: "../src/FlexiBlocks/Extensions/FlexiTableBlocks/FlexiTableBlockOptions.cs"
extensionOptions: "../src/FlexiBlocks/Extensions/FlexiTableBlocks/FlexiTableBlocksExtensionOptions.cs"
---

# FlexiTableBlocks
FlexiTableBlocks are tables inspired by Markdig's [pipe tables](https://github.com/lunet-io/markdig/blob/master/src/Markdig.Tests/Specs/PipeTableSpecs.md) and
[grid tables](https://github.com/lunet-io/markdig/blob/master/src/Markdig.Tests/Specs/GridTableSpecs.md). FlexiTableBlocks are
rendered for use with [CSS-only responsive table methods](https://www.jering.tech/articles/card-style-css-only-responsive-tables).

## Usage
```csharp
using Markdig;
using Jering.Markdig.Extensions.FlexiBlocks;

...
var markdownPipelineBuilder = new MarkdownPipelineBuilder();
markdownPipelineBuilder.UseFlexiTableBlocks(/* Optional extension options */);

MarkdownPipeline markdownPipeline = markdownPipelineBuilder.Build();

string markdown = @"+--------------+--------------+
| header 1     | header 2     |
+==============+==============+
| cell 1       | cell 2       |
+--------------+--------------+"
string html = Markdown.ToHtml(markdown, markdownPipeline);
string expectedHtml = @"<div class=""flexi-table flexi-table_type_cards"">
<table class=""flexi-table__table"">
<thead class=""flexi-table__head"">
<tr class=""flexi-table__row"">
<th class=""flexi-table__header"">
header 1
</th>
<th class=""flexi-table__header"">
header 2
</th>
</tr>
</thead>
<tbody class=""flexi-table__body"">
<tr class=""flexi-table__row"">
<td class=""flexi-table__data"">
<div class=""flexi-table__label"">
header 1
</div>
<div class=""flexi-table__content"">
cell 1
</div>
</td>
<td class=""flexi-table__data"">
<div class=""flexi-table__label"">
header 2
</div>
<div class=""flexi-table__content"">
cell 2
</div>
</td>
</tr>
</tbody>
</table>
</div>";

Assert.Equal(expectedHtml, html)
```

## Basics
There are two syntaxes for FlexiTableBlocks:

### Basic FlexiTableBlocks
A basic FlexiTableBlock consists of:
- Multiple row lines, which are <cell> one or more times, followed by '|', where <cell> is '|' followed by any number of any characters excluding '\n' and '\0'.
- An optional column definitions line, which is <column definition> one or more times followed by '|', where <column definition> is '|' followed optionally by ':', 
  followed by one or more '-', followed optionally by ':'. The column definitions line indicates content-alignment for cells in each column and demarcates 
  the table's head from its body.

This is a basic FlexiTableBlock:
```````````````````````````````` none
--------------- Markdown ---------------
| header 1 | header 2 |
|----------|----------|
| cell 1   | cell 2   |
--------------- Expected Markup ---------------
<div class="flexi-table flexi-table_type_cards">
<table class="flexi-table__table">
<thead class="flexi-table__head">
<tr class="flexi-table__row">
<th class="flexi-table__header">
header 1
</th>
<th class="flexi-table__header">
header 2
</th>
</tr>
</thead>
<tbody class="flexi-table__body">
<tr class="flexi-table__row">
<td class="flexi-table__data">
<div class="flexi-table__label">
header 1
</div>
<div class="flexi-table__content">
cell 1
</div>
</td>
<td class="flexi-table__data">
<div class="flexi-table__label">
header 2
</div>
<div class="flexi-table__content">
cell 2
</div>
</td>
</tr>
</tbody>
</table>
</div>
````````````````````````````````

If a basic FlexiTableBlock follows a paragraph block, there must be an empty line between them:
```````````````````````````````` none
--------------- Markdown ---------------
The following is not a table:
| header 1 | header 2 |
|----------|----------|
| cell 1   | cell 2   |

The following is a table:

| header 1 | header 2 |
|----------|----------|
| cell 1   | cell 2   |
--------------- Expected Markup ---------------
<p>The following is not a table:
| header 1 | header 2 |
|----------|----------|
| cell 1   | cell 2   |</p>
<p>The following is a table:</p>
<div class="flexi-table flexi-table_type_cards">
<table class="flexi-table__table">
<thead class="flexi-table__head">
<tr class="flexi-table__row">
<th class="flexi-table__header">
header 1
</th>
<th class="flexi-table__header">
header 2
</th>
</tr>
</thead>
<tbody class="flexi-table__body">
<tr class="flexi-table__row">
<td class="flexi-table__data">
<div class="flexi-table__label">
header 1
</div>
<div class="flexi-table__content">
cell 1
</div>
</td>
<td class="flexi-table__data">
<div class="flexi-table__label">
header 2
</div>
<div class="flexi-table__content">
cell 2
</div>
</td>
</tr>
</tbody>
</table>
</div>
````````````````````````````````

':'s in the column definitions line indicate content-alignment for all cells in the column:
```````````````````````````````` none
--------------- Markdown ---------------
| header 1 | header 2 | header 3 |
|:---------|:--------:|---------:|
| cell 1   | cell 2   | cell 3   |
--------------- Expected Markup ---------------
<div class="flexi-table flexi-table_type_cards">
<table class="flexi-table__table">
<thead class="flexi-table__head">
<tr class="flexi-table__row">
<th class="flexi-table__header flexi-table__header_align_start">
header 1
</th>
<th class="flexi-table__header flexi-table__header_align_center">
header 2
</th>
<th class="flexi-table__header flexi-table__header_align_end">
header 3
</th>
</tr>
</thead>
<tbody class="flexi-table__body">
<tr class="flexi-table__row">
<td class="flexi-table__data flexi-table__data_align_start">
<div class="flexi-table__label">
header 1
</div>
<div class="flexi-table__content">
cell 1
</div>
</td>
<td class="flexi-table__data flexi-table__data_align_center">
<div class="flexi-table__label">
header 2
</div>
<div class="flexi-table__content">
cell 2
</div>
</td>
<td class="flexi-table__data flexi-table__data_align_end">
<div class="flexi-table__label">
header 3
</div>
<div class="flexi-table__content">
cell 3
</div>
</td>
</tr>
</tbody>
</table>
</div>
````````````````````````````````

If the first line is a column definitions line, the table will be rendered without a head:
```````````````````````````````` none
--------------- Markdown ---------------
|:-------|-------:|
| cell 1 | cell 2 |
--------------- Expected Markup ---------------
<div class="flexi-table flexi-table_type_cards">
<table class="flexi-table__table">
<tbody class="flexi-table__body">
<tr class="flexi-table__row">
<td class="flexi-table__data flexi-table__data_align_start">
cell 1
</td>
<td class="flexi-table__data flexi-table__data_align_end">
cell 2
</td>
</tr>
</tbody>
</table>
</div>
````````````````````````````````

If there are multiple column definition lines, the second one and on are parsed as row lines:
```````````````````````````````` none
--------------- Markdown ---------------
| header 1 | header 2 |
|----------|----------|
| cell 1   | cell 2   |
|----------|----------|
| cell 3   | cell 4   |
|----------|----------|
| cell 5   | cell 6   |
--------------- Expected Markup ---------------
<div class="flexi-table flexi-table_type_cards">
<table class="flexi-table__table">
<thead class="flexi-table__head">
<tr class="flexi-table__row">
<th class="flexi-table__header">
header 1
</th>
<th class="flexi-table__header">
header 2
</th>
</tr>
</thead>
<tbody class="flexi-table__body">
<tr class="flexi-table__row">
<td class="flexi-table__data">
<div class="flexi-table__label">
header 1
</div>
<div class="flexi-table__content">
cell 1
</div>
</td>
<td class="flexi-table__data">
<div class="flexi-table__label">
header 2
</div>
<div class="flexi-table__content">
cell 2
</div>
</td>
</tr>
<tr class="flexi-table__row">
<td class="flexi-table__data">
<div class="flexi-table__label">
header 1
</div>
<div class="flexi-table__content">
<hr />
</div>
</td>
<td class="flexi-table__data">
<div class="flexi-table__label">
header 2
</div>
<div class="flexi-table__content">
<hr />
</div>
</td>
</tr>
<tr class="flexi-table__row">
<td class="flexi-table__data">
<div class="flexi-table__label">
header 1
</div>
<div class="flexi-table__content">
cell 3
</div>
</td>
<td class="flexi-table__data">
<div class="flexi-table__label">
header 2
</div>
<div class="flexi-table__content">
cell 4
</div>
</td>
</tr>
<tr class="flexi-table__row">
<td class="flexi-table__data">
<div class="flexi-table__label">
header 1
</div>
<div class="flexi-table__content">
<hr />
</div>
</td>
<td class="flexi-table__data">
<div class="flexi-table__label">
header 2
</div>
<div class="flexi-table__content">
<hr />
</div>
</td>
</tr>
<tr class="flexi-table__row">
<td class="flexi-table__data">
<div class="flexi-table__label">
header 1
</div>
<div class="flexi-table__content">
cell 5
</div>
</td>
<td class="flexi-table__data">
<div class="flexi-table__label">
header 2
</div>
<div class="flexi-table__content">
cell 6
</div>
</td>
</tr>
</tbody>
</table>
</div>
````````````````````````````````

Multiple header rows are allowed for unresponsive type tables:
```````````````````````````````` none
--------------- Extra Extensions ---------------
OptionsBlocks
--------------- Markdown ---------------
@{ "type": "unresponsive" }
| header 1 | header 2 |
| header 3 | header 4 |
|----------|----------|
| cell 1   | cell 2   |
--------------- Expected Markup ---------------
<div class="flexi-table flexi-table_type_unresponsive">
<table class="flexi-table__table">
<thead class="flexi-table__head">
<tr class="flexi-table__row">
<th class="flexi-table__header">
header 1
</th>
<th class="flexi-table__header">
header 2
</th>
</tr>
<tr class="flexi-table__row">
<th class="flexi-table__header">
header 3
</th>
<th class="flexi-table__header">
header 4
</th>
</tr>
</thead>
<tbody class="flexi-table__body">
<tr class="flexi-table__row">
<td class="flexi-table__data">
cell 1
</td>
<td class="flexi-table__data">
cell 2
</td>
</tr>
</tbody>
</table>
</div>
````````````````````````````````

The column definitions line is optional:
```````````````````````````````` none
--------------- Markdown ---------------
| cell 1   | cell 2   |
| cell 3   | cell 4   |
--------------- Expected Markup ---------------
<div class="flexi-table flexi-table_type_cards">
<table class="flexi-table__table">
<tbody class="flexi-table__body">
<tr class="flexi-table__row">
<td class="flexi-table__data">
cell 1
</td>
<td class="flexi-table__data">
cell 2
</td>
</tr>
<tr class="flexi-table__row">
<td class="flexi-table__data">
cell 3
</td>
<td class="flexi-table__data">
cell 4
</td>
</tr>
</tbody>
</table>
</div>
````````````````````````````````

To use '|' within a cell, escape it:
```````````````````````````````` none
--------------- Markdown ---------------
| header 1 | header 2 |
|----------|----------|
| \| \| \| | \| \| \| |
--------------- Expected Markup ---------------
<div class="flexi-table flexi-table_type_cards">
<table class="flexi-table__table">
<thead class="flexi-table__head">
<tr class="flexi-table__row">
<th class="flexi-table__header">
header 1
</th>
<th class="flexi-table__header">
header 2
</th>
</tr>
</thead>
<tbody class="flexi-table__body">
<tr class="flexi-table__row">
<td class="flexi-table__data">
<div class="flexi-table__label">
header 1
</div>
<div class="flexi-table__content">
| | |
</div>
</td>
<td class="flexi-table__data">
<div class="flexi-table__label">
header 2
</div>
<div class="flexi-table__content">
| | |
</div>
</td>
</tr>
</tbody>
</table>
</div>
````````````````````````````````

Cells do not need to be aligned:
```````````````````````````````` none
--------------- Markdown ---------------
| header 1 | header 2 |
|---|---|
| cell 1 | cell 2 |
--------------- Expected Markup ---------------
<div class="flexi-table flexi-table_type_cards">
<table class="flexi-table__table">
<thead class="flexi-table__head">
<tr class="flexi-table__row">
<th class="flexi-table__header">
header 1
</th>
<th class="flexi-table__header">
header 2
</th>
</tr>
</thead>
<tbody class="flexi-table__body">
<tr class="flexi-table__row">
<td class="flexi-table__data">
<div class="flexi-table__label">
header 1
</div>
<div class="flexi-table__content">
cell 1
</div>
</td>
<td class="flexi-table__data">
<div class="flexi-table__label">
header 2
</div>
<div class="flexi-table__content">
cell 2
</div>
</td>
</tr>
</tbody>
</table>
</div>
````````````````````````````````

If the first row/column definition has x cells/column definitions, all subsequent lines must have x cells/column definitions:
```````````````````````````````` none
--------------- Markdown ---------------
| header 1 | header 2 |
|----------|----------|
| cell 1 | cell 2 | cell 3 |
--------------- Expected Markup ---------------
<p>| header 1 | header 2 |
|----------|----------|
| cell 1 | cell 2 | cell 3 |</p>
````````````````````````````````

Leading whitespace of cell content is preserved, trailing whitespace is discarded:
```````````````````````````````` none
--------------- Extra Extensions ---------------
FlexiCodeBlocks
--------------- Markdown ---------------
| header 1 | header 2 |
|----------|----------|
|cell 1    |    cell 2|
--------------- Expected Markup ---------------
<div class="flexi-table flexi-table_type_cards">
<table class="flexi-table__table">
<thead class="flexi-table__head">
<tr class="flexi-table__row">
<th class="flexi-table__header">
header 1
</th>
<th class="flexi-table__header">
header 2
</th>
</tr>
</thead>
<tbody class="flexi-table__body">
<tr class="flexi-table__row">
<td class="flexi-table__data">
<div class="flexi-table__label">
header 1
</div>
<div class="flexi-table__content">
cell 1
</div>
</td>
<td class="flexi-table__data">
<div class="flexi-table__label">
header 2
</div>
<div class="flexi-table__content">
<div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
<header class="flexi-code__header">
<span class="flexi-code__title"></span>
<button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
<svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
</button>
</header>
<pre class="flexi-code__pre"><code class="flexi-code__code">cell 2
</code></pre>
</div>
</div>
</td>
</tr>
</tbody>
</table>
</div>
````````````````````````````````

Cell content may be any valid markdown that can fit within a line:
```````````````````````````````` none
--------------- Extra Extensions ---------------
FlexiCodeBlocks
--------------- Markdown ---------------
| **header 1** | [header 2](url) | *header 3* |
|----------|----------|----------|
| `cell 1` |    cell 2 | > cell 3 |
--------------- Expected Markup ---------------
<div class="flexi-table flexi-table_type_cards">
<table class="flexi-table__table">
<thead class="flexi-table__head">
<tr class="flexi-table__row">
<th class="flexi-table__header">
<strong>header 1</strong>
</th>
<th class="flexi-table__header">
<a href="url">header 2</a>
</th>
<th class="flexi-table__header">
<em>header 3</em>
</th>
</tr>
</thead>
<tbody class="flexi-table__body">
<tr class="flexi-table__row">
<td class="flexi-table__data">
<div class="flexi-table__label">
<strong>header 1</strong>
</div>
<div class="flexi-table__content">
<code>cell 1</code>
</div>
</td>
<td class="flexi-table__data">
<div class="flexi-table__label">
<a href="url">header 2</a>
</div>
<div class="flexi-table__content">
<div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
<header class="flexi-code__header">
<span class="flexi-code__title"></span>
<button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
<svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
</button>
</header>
<pre class="flexi-code__pre"><code class="flexi-code__code">cell 2
</code></pre>
</div>
</div>
</td>
<td class="flexi-table__data">
<div class="flexi-table__label">
<em>header 3</em>
</div>
<div class="flexi-table__content">
<blockquote>
<p>cell 3</p>
</blockquote>
</div>
</td>
</tr>
</tbody>
</table>
</div>
````````````````````````````````

Any line that does not begin with '|' ends a basic FlexiTableBlock:
```````````````````````````````` none
--------------- Markdown ---------------
| header 1 | header 2 |
|----------|----------|
| cell 1   | cell 2   |
This line ends the table
--------------- Expected Markup ---------------
<div class="flexi-table flexi-table_type_cards">
<table class="flexi-table__table">
<thead class="flexi-table__head">
<tr class="flexi-table__row">
<th class="flexi-table__header">
header 1
</th>
<th class="flexi-table__header">
header 2
</th>
</tr>
</thead>
<tbody class="flexi-table__body">
<tr class="flexi-table__row">
<td class="flexi-table__data">
<div class="flexi-table__label">
header 1
</div>
<div class="flexi-table__content">
cell 1
</div>
</td>
<td class="flexi-table__data">
<div class="flexi-table__label">
header 2
</div>
<div class="flexi-table__content">
cell 2
</div>
</td>
</tr>
</tbody>
</table>
</div>
<p>This line ends the table</p>
````````````````````````````````

If the line following a basic FlexiTableBlock begins with '|', there must be an empty line before it:
```````````````````````````````` none
--------------- Markdown ---------------
| header 1 | header 2 |
|----------|----------|
| cell 1   | cell 2   |
| This line invalidates the preceding table

| header 1 | header 2 |
|----------|----------|
| cell 1   | cell 2   |

| This line does not invalidate the preceding table
--------------- Expected Markup ---------------
<p>| header 1 | header 2 |
|----------|----------|
| cell 1   | cell 2   |
| This line invalidates the preceding table</p>
<div class="flexi-table flexi-table_type_cards">
<table class="flexi-table__table">
<thead class="flexi-table__head">
<tr class="flexi-table__row">
<th class="flexi-table__header">
header 1
</th>
<th class="flexi-table__header">
header 2
</th>
</tr>
</thead>
<tbody class="flexi-table__body">
<tr class="flexi-table__row">
<td class="flexi-table__data">
<div class="flexi-table__label">
header 1
</div>
<div class="flexi-table__content">
cell 1
</div>
</td>
<td class="flexi-table__data">
<div class="flexi-table__label">
header 2
</div>
<div class="flexi-table__content">
cell 2
</div>
</td>
</tr>
</tbody>
</table>
</div>
<p>| This line does not invalidate the preceding table</p>
````````````````````````````````

### Advanced FlexiTableBlocks

An advanced FlexiTableBlock consists of four kinds of lines:
- A column definitions line, which is <column definition> one or more times followed by '+', where <column definition> is '+' followed optionally by ':', 
  followed by one or more '-', followed optionally by ':'. The column definitions line is the mandatory-first-line of an advanced FlexiTableBlock
  and indicates content-alignment for cells in each column.
- Mutiple content lines, which are <content> one or more times, followed by '|', where <content> is '|' followed by a series of characters. '|'s must be aligned with '+'s 
  in the column definitions line. Content lines contain content for cells.
- An optional head separator line, which is <head separator> one or more times followed by '+', where <head separator> is '+' followed by one or more '='. '+'s must be 
  aligned with '+'s in the column definitions line. The head separator line demarcates the table's head from its body.
- Multiple row separator lines, which are <row separator> one or more times followed by '+', where <row separator> is '+' followed by one or more '-' or a series of 
  characters. '+'s must be aligned with '+'s in the column definitions line. Row separator lines demarcate rows.

This is an advanced FlexiTableBlock:
```````````````````````````````` none
--------------- Markdown ---------------
+--------------+--------------+
| header 1     | header 2     |
+==============+==============+
| cell 1       | cell 2       |
+--------------+--------------+
--------------- Expected Markup ---------------
<div class="flexi-table flexi-table_type_cards">
<table class="flexi-table__table">
<thead class="flexi-table__head">
<tr class="flexi-table__row">
<th class="flexi-table__header">
header 1
</th>
<th class="flexi-table__header">
header 2
</th>
</tr>
</thead>
<tbody class="flexi-table__body">
<tr class="flexi-table__row">
<td class="flexi-table__data">
<div class="flexi-table__label">
header 1
</div>
<div class="flexi-table__content">
cell 1
</div>
</td>
<td class="flexi-table__data">
<div class="flexi-table__label">
header 2
</div>
<div class="flexi-table__content">
cell 2
</div>
</td>
</tr>
</tbody>
</table>
</div>
````````````````````````````````

If an advanced FlexiTableBlock follows a paragraph block, there must be an empty line between them:
```````````````````````````````` none
--------------- Markdown ---------------
The following is not a table:
+--------------+--------------+
| header 1     | header 2     |
+==============+==============+
| cell 1       | cell 2       |
+--------------+--------------+

The following is a table:

+--------------+--------------+
| header 1     | header 2     |
+==============+==============+
| cell 1       | cell 2       |
+--------------+--------------+
--------------- Expected Markup ---------------
<p>The following is not a table:
+--------------+--------------+
| header 1     | header 2     |
+==============+==============+
| cell 1       | cell 2       |
+--------------+--------------+</p>
<p>The following is a table:</p>
<div class="flexi-table flexi-table_type_cards">
<table class="flexi-table__table">
<thead class="flexi-table__head">
<tr class="flexi-table__row">
<th class="flexi-table__header">
header 1
</th>
<th class="flexi-table__header">
header 2
</th>
</tr>
</thead>
<tbody class="flexi-table__body">
<tr class="flexi-table__row">
<td class="flexi-table__data">
<div class="flexi-table__label">
header 1
</div>
<div class="flexi-table__content">
cell 1
</div>
</td>
<td class="flexi-table__data">
<div class="flexi-table__label">
header 2
</div>
<div class="flexi-table__content">
cell 2
</div>
</td>
</tr>
</tbody>
</table>
</div>
````````````````````````````````

':'s in the column definitions line indicate content-alignment for all cells in the column:
```````````````````````````````` none
--------------- Markdown ---------------
+:-------------+:------------:+-------------:+
| header 1     | header 2     | header 3     |
+==============+==============+==============+
| cell 1       | cell 2       | cell 3       |
+--------------+--------------+--------------+
--------------- Expected Markup ---------------
<div class="flexi-table flexi-table_type_cards">
<table class="flexi-table__table">
<thead class="flexi-table__head">
<tr class="flexi-table__row">
<th class="flexi-table__header flexi-table__header_align_start">
header 1
</th>
<th class="flexi-table__header flexi-table__header_align_center">
header 2
</th>
<th class="flexi-table__header flexi-table__header_align_end">
header 3
</th>
</tr>
</thead>
<tbody class="flexi-table__body">
<tr class="flexi-table__row">
<td class="flexi-table__data flexi-table__data_align_start">
<div class="flexi-table__label">
header 1
</div>
<div class="flexi-table__content">
cell 1
</div>
</td>
<td class="flexi-table__data flexi-table__data_align_center">
<div class="flexi-table__label">
header 2
</div>
<div class="flexi-table__content">
cell 2
</div>
</td>
<td class="flexi-table__data flexi-table__data_align_end">
<div class="flexi-table__label">
header 3
</div>
<div class="flexi-table__content">
cell 3
</div>
</td>
</tr>
</tbody>
</table>
</div>
````````````````````````````````

The last row separator line is optional
```````````````````````````````` none
--------------- Markdown ---------------
+--------------+--------------+
| header 1     | header 2     |
+==============+==============+
| cell 1       | cell 2       |
--------------- Expected Markup ---------------
<div class="flexi-table flexi-table_type_cards">
<table class="flexi-table__table">
<thead class="flexi-table__head">
<tr class="flexi-table__row">
<th class="flexi-table__header">
header 1
</th>
<th class="flexi-table__header">
header 2
</th>
</tr>
</thead>
<tbody class="flexi-table__body">
<tr class="flexi-table__row">
<td class="flexi-table__data">
<div class="flexi-table__label">
header 1
</div>
<div class="flexi-table__content">
cell 1
</div>
</td>
<td class="flexi-table__data">
<div class="flexi-table__label">
header 2
</div>
<div class="flexi-table__content">
cell 2
</div>
</td>
</tr>
</tbody>
</table>
</div>
````````````````````````````````

If the content of a row separator is not '-'s, the content is added to the cell above and the cell will span to the next row.
Row span is only allowed for unresponsive tables:
```````````````````````````````` none
--------------- Extra Extensions ---------------
OptionsBlocks
--------------- Markdown ---------------
@{ "type": "unresponsive" }
+--------------+--------------+
| header 1     | header 2     |
+==============+==============+
| cell 1       | cell 2       |
+ still cell 1 +--------------+
| still cell 1 | cell 3       |
+--------------+--------------+
--------------- Expected Markup ---------------
<div class="flexi-table flexi-table_type_unresponsive">
<table class="flexi-table__table">
<thead class="flexi-table__head">
<tr class="flexi-table__row">
<th class="flexi-table__header">
header 1
</th>
<th class="flexi-table__header">
header 2
</th>
</tr>
</thead>
<tbody class="flexi-table__body">
<tr class="flexi-table__row">
<td class="flexi-table__data" rowspan="2">
cell 1
still cell 1
still cell 1
</td>
<td class="flexi-table__data">
cell 2
</td>
</tr>
<tr class="flexi-table__row">
<td class="flexi-table__data">
cell 3
</td>
</tr>
</tbody>
</table>
</div>
````````````````````````````````

If there are multiple head separator lines, the second one and on are parsed as content lines:
```````````````````````````````` none
--------------- Extra Extensions ---------------
OptionsBlocks
--------------- Markdown ---------------
@{ "type": "unresponsive" }
+--------------+--------------+
| header 1     | header 2     |
+==============+==============+
| cell 1       | cell 2       |
+==============+==============+
| still cell 1 | still cell 2 |
+==============+==============+
--------------- Expected Markup ---------------
<div class="flexi-table flexi-table_type_unresponsive">
<table class="flexi-table__table">
<thead class="flexi-table__head">
<tr class="flexi-table__row">
<th class="flexi-table__header">
header 1
</th>
<th class="flexi-table__header">
header 2
</th>
</tr>
</thead>
<tbody class="flexi-table__body">
<tr class="flexi-table__row">
<td class="flexi-table__data" rowspan="2">
<h1>cell 1</h1>
<h1>still cell 1</h1>
</td>
<td class="flexi-table__data" rowspan="2">
<h1>cell 2</h1>
<h1>still cell 2</h1>
</td>
</tr>
<tr class="flexi-table__row">
</tr>
</tbody>
</table>
</div>
````````````````````````````````

The head separator line is optional:
```````````````````````````````` none
--------------- Markdown ---------------
+--------------+--------------+
| cell 1       | cell 2       |
+--------------+--------------+
| cell 3       | cell 4       |
+--------------+--------------+
--------------- Expected Markup ---------------
<div class="flexi-table flexi-table_type_cards">
<table class="flexi-table__table">
<tbody class="flexi-table__body">
<tr class="flexi-table__row">
<td class="flexi-table__data">
cell 1
</td>
<td class="flexi-table__data">
cell 2
</td>
</tr>
<tr class="flexi-table__row">
<td class="flexi-table__data">
cell 3
</td>
<td class="flexi-table__data">
cell 4
</td>
</tr>
</tbody>
</table>
</div>
````````````````````````````````

Multiple header rows are allowed for unresponsive type tables:
```````````````````````````````` none
--------------- Extra Extensions ---------------
OptionsBlocks
--------------- Markdown ---------------
@{ "type": "unresponsive" }
+--------------+--------------+
| header 1     | header 2     |
+--------------+--------------+
| header 3     | header 4     |
+==============+==============+
| cell 1       | cell 2       |
+--------------+--------------+
--------------- Expected Markup ---------------
<div class="flexi-table flexi-table_type_unresponsive">
<table class="flexi-table__table">
<thead class="flexi-table__head">
<tr class="flexi-table__row">
<th class="flexi-table__header">
header 1
</th>
<th class="flexi-table__header">
header 2
</th>
</tr>
<tr class="flexi-table__row">
<th class="flexi-table__header">
header 3
</th>
<th class="flexi-table__header">
header 4
</th>
</tr>
</thead>
<tbody class="flexi-table__body">
<tr class="flexi-table__row">
<td class="flexi-table__data">
cell 1
</td>
<td class="flexi-table__data">
cell 2
</td>
</tr>
</tbody>
</table>
</div>
````````````````````````````````

Cells must be aligned:
```````````````````````````````` none
--------------- Markdown ---------------
+--------------+----------+
| header 1     | header 2     |
+==============+==============+
| cell 1       | cell 2       |
+--------------+--------------+

+--------------+--------------+
| header 1     | header 2     |
+==============+==========+
| cell 1       | cell 2       |
+--------------+--------------+

+--------------+--------------+
| header 1     | header 2     |
+==============+==============+
| cell 1   | cell 2   |
+--------------+--------------+
--------------- Expected Markup ---------------
<p>+--------------+----------+
| header 1     | header 2     |
+==============+==============+
| cell 1       | cell 2       |
+--------------+--------------+</p>
<p>+--------------+--------------+
| header 1     | header 2     |
+==============+==========+
| cell 1       | cell 2       |
+--------------+--------------+</p>
<p>+--------------+--------------+
| header 1     | header 2     |
+==============+==============+
| cell 1   | cell 2   |
+--------------+--------------+</p>
````````````````````````````````

Cells can have column span in unresponsive type tables:
```````````````````````````````` none
--------------- Extra Extensions ---------------
OptionsBlocks
--------------- Markdown ---------------
@{ "type": "unresponsive" }
+--------------+--------------+
| header 1     | header 2     |
+==============+==============+
| cell 1                      |
+--------------+--------------+
--------------- Expected Markup ---------------
<div class="flexi-table flexi-table_type_unresponsive">
<table class="flexi-table__table">
<thead class="flexi-table__head">
<tr class="flexi-table__row">
<th class="flexi-table__header">
header 1
</th>
<th class="flexi-table__header">
header 2
</th>
</tr>
</thead>
<tbody class="flexi-table__body">
<tr class="flexi-table__row">
<td class="flexi-table__data" colspan="2">
cell 1
</td>
</tr>
</tbody>
</table>
</div>
````````````````````````````````

Cells can have both row and column span in unresponsive type tables:
```````````````````````````````` none
--------------- Extra Extensions ---------------
OptionsBlocks
--------------- Markdown ---------------
@{ "type": "unresponsive" }
+--------------+--------------+--------------+
| header 1     | header 2     | header 3     |
+==============+==============+==============+
| no span      | rowspan and colspan         |
+--------------+                             +
| no span      |                             |
+--------------+--------------+--------------+
| rowspan      | colspan                     |
+              +--------------+--------------+
|              | no span      | no span      |
+--------------+--------------+--------------+
--------------- Expected Markup ---------------
<div class="flexi-table flexi-table_type_unresponsive">
<table class="flexi-table__table">
<thead class="flexi-table__head">
<tr class="flexi-table__row">
<th class="flexi-table__header">
header 1
</th>
<th class="flexi-table__header">
header 2
</th>
<th class="flexi-table__header">
header 3
</th>
</tr>
</thead>
<tbody class="flexi-table__body">
<tr class="flexi-table__row">
<td class="flexi-table__data">
no span
</td>
<td class="flexi-table__data" colspan="2" rowspan="2">
rowspan and colspan
</td>
</tr>
<tr class="flexi-table__row">
<td class="flexi-table__data">
no span
</td>
</tr>
<tr class="flexi-table__row">
<td class="flexi-table__data" rowspan="2">
rowspan
</td>
<td class="flexi-table__data" colspan="2">
colspan
</td>
</tr>
<tr class="flexi-table__row">
<td class="flexi-table__data">
no span
</td>
<td class="flexi-table__data">
no span
</td>
</tr>
</tbody>
</table>
</div>
````````````````````````````````

Leading whitespace of cell content is preserved, trailing whitespace is discarded:
```````````````````````````````` none
--------------- Extra Extensions ---------------
FlexiCodeBlocks
--------------- Markdown ---------------
+--------------+--------------+
| header 1     | header 2     |
+==============+==============+
| cell 1       |       cell 2 |
+--------------+--------------+
--------------- Expected Markup ---------------
<div class="flexi-table flexi-table_type_cards">
<table class="flexi-table__table">
<thead class="flexi-table__head">
<tr class="flexi-table__row">
<th class="flexi-table__header">
header 1
</th>
<th class="flexi-table__header">
header 2
</th>
</tr>
</thead>
<tbody class="flexi-table__body">
<tr class="flexi-table__row">
<td class="flexi-table__data">
<div class="flexi-table__label">
header 1
</div>
<div class="flexi-table__content">
cell 1
</div>
</td>
<td class="flexi-table__data">
<div class="flexi-table__label">
header 2
</div>
<div class="flexi-table__content">
<div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
<header class="flexi-code__header">
<span class="flexi-code__title"></span>
<button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
<svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
</button>
</header>
<pre class="flexi-code__pre"><code class="flexi-code__code">   cell 2
</code></pre>
</div>
</div>
</td>
</tr>
</tbody>
</table>
</div>
````````````````````````````````

Cell content may be any valid markdown:
```````````````````````````````` none
--------------- Extra Extensions ---------------
FlexiCodeBlocks
--------------- Markdown ---------------
+--------------+-----------------+--------------+
| **header 1** | [header 2](url) | *header 3*   |
+==============+=================+==============+
| - cell 1     | ```             | > cell 3     |
| - cell 2     | cell 2          | > cell 3     |
| - cell 3     | ```             | > cell 3     |
+--------------+-----------------+--------------+
--------------- Expected Markup ---------------
<div class="flexi-table flexi-table_type_cards">
<table class="flexi-table__table">
<thead class="flexi-table__head">
<tr class="flexi-table__row">
<th class="flexi-table__header">
<strong>header 1</strong>
</th>
<th class="flexi-table__header">
<a href="url">header 2</a>
</th>
<th class="flexi-table__header">
<em>header 3</em>
</th>
</tr>
</thead>
<tbody class="flexi-table__body">
<tr class="flexi-table__row">
<td class="flexi-table__data">
<div class="flexi-table__label">
<strong>header 1</strong>
</div>
<div class="flexi-table__content">
<ul>
<li>cell 1</li>
<li>cell 2</li>
<li>cell 3</li>
</ul>
</div>
</td>
<td class="flexi-table__data">
<div class="flexi-table__label">
<a href="url">header 2</a>
</div>
<div class="flexi-table__content">
<div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
<header class="flexi-code__header">
<span class="flexi-code__title"></span>
<button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
<svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
</button>
</header>
<pre class="flexi-code__pre"><code class="flexi-code__code">cell 2
</code></pre>
</div>
</div>
</td>
<td class="flexi-table__data">
<div class="flexi-table__label">
<em>header 3</em>
</div>
<div class="flexi-table__content">
<blockquote>
<p>cell 3
cell 3
cell 3</p>
</blockquote>
</div>
</td>
</tr>
</tbody>
</table>
</div>
````````````````````````````````

Any line that does not begin with '+' ends a basic FlexiTableBlock:
```````````````````````````````` none
--------------- Markdown ---------------
+--------------+--------------+
| header 1     | header 2     |
+==============+==============+
| cell 1       | cell 2       |
This line ends the table
--------------- Expected Markup ---------------
<div class="flexi-table flexi-table_type_cards">
<table class="flexi-table__table">
<thead class="flexi-table__head">
<tr class="flexi-table__row">
<th class="flexi-table__header">
header 1
</th>
<th class="flexi-table__header">
header 2
</th>
</tr>
</thead>
<tbody class="flexi-table__body">
<tr class="flexi-table__row">
<td class="flexi-table__data">
<div class="flexi-table__label">
header 1
</div>
<div class="flexi-table__content">
cell 1
</div>
</td>
<td class="flexi-table__data">
<div class="flexi-table__label">
header 2
</div>
<div class="flexi-table__content">
cell 2
</div>
</td>
</tr>
</tbody>
</table>
</div>
<p>This line ends the table</p>
````````````````````````````````

If the line following a basic FlexiTableBlock begins with '+' or '|', there must be an empty line before it:
```````````````````````````````` none
--------------- Markdown ---------------
+--------------+--------------+
| header 1     | header 2     |
+==============+==============+
| cell 1       | cell 2       |
+--------------+--------------+
| This line invalidates the preceding table

+--------------+--------------+
| header 1     | header 2     |
+==============+==============+
| cell 1       | cell 2       |
+ This line invalidates the preceding table

+--------------+--------------+
| header 1     | header 2     |
+==============+==============+
| cell 1       | cell 2       |
+--------------+--------------+

| This line does not invalidate the preceding table

+--------------+--------------+
| header 1     | header 2     |
+==============+==============+
| cell 1       | cell 2       |

+ This line does not invalidate the preceding table
--------------- Expected Markup ---------------
<p>+--------------+--------------+
| header 1     | header 2     |
+==============+==============+
| cell 1       | cell 2       |
+--------------+--------------+
| This line invalidates the preceding table</p>
<p>+--------------+--------------+
| header 1     | header 2     |
+==============+==============+
| cell 1       | cell 2       |</p>
<ul>
<li>This line invalidates the preceding table</li>
</ul>
<div class="flexi-table flexi-table_type_cards">
<table class="flexi-table__table">
<thead class="flexi-table__head">
<tr class="flexi-table__row">
<th class="flexi-table__header">
header 1
</th>
<th class="flexi-table__header">
header 2
</th>
</tr>
</thead>
<tbody class="flexi-table__body">
<tr class="flexi-table__row">
<td class="flexi-table__data">
<div class="flexi-table__label">
header 1
</div>
<div class="flexi-table__content">
cell 1
</div>
</td>
<td class="flexi-table__data">
<div class="flexi-table__label">
header 2
</div>
<div class="flexi-table__content">
cell 2
</div>
</td>
</tr>
</tbody>
</table>
</div>
<p>| This line does not invalidate the preceding table</p>
<div class="flexi-table flexi-table_type_cards">
<table class="flexi-table__table">
<thead class="flexi-table__head">
<tr class="flexi-table__row">
<th class="flexi-table__header">
header 1
</th>
<th class="flexi-table__header">
header 2
</th>
</tr>
</thead>
<tbody class="flexi-table__body">
<tr class="flexi-table__row">
<td class="flexi-table__data">
<div class="flexi-table__label">
header 1
</div>
<div class="flexi-table__content">
cell 1
</div>
</td>
<td class="flexi-table__data">
<div class="flexi-table__label">
header 2
</div>
<div class="flexi-table__content">
cell 2
</div>
</td>
</tr>
</tbody>
</table>
</div>
<ul>
<li>This line does not invalidate the preceding table</li>
</ul>
````````````````````````````````

### Basic vs Advanced FlexiTableBlocks

Advanced FlexiTableBlocks have all the features of basic FlexiTableBlocks along with the following:
- Multiline cells
- Cell column span and row span

These extra features come at the cost of verbosity and syntax rigidity. We recommend using basic
FlexiTableBlocks unless you require the above-mentioned features.

## Options
### `FlexiTableBlockOptions`
Options for a FlexiTableBlock. To specify `FlexiTableBlockOptions` for a FlexiTableBlock, the [Options](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/OptionsBlocksSpecs.md#options) extension must be enabled.

#### Properties

##### `BlockName`
- Type: `string`
- Description: The `FlexiTableBlock`'s [BEM block name](https://en.bem.info/methodology/naming-convention/#block-name).
  In compliance with [BEM methodology](https://en.bem.info), this value is the `FlexiTableBlock`'s root element's class as well as the prefix for all other classes in the block.
  This value should contain only valid [CSS class characters](https://www.w3.org/TR/CSS21/syndata.html#characters).
  If this value is `null`, whitespace or an empty string, the `FlexiTableBlock`'s block name is "flexi-table".
- Default: "flexi-table"
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  @{ "blockName": "table" }
  | header 1 | header 2 |
  |----------|----------|
  | cell 1   | cell 2   |
  --------------- Expected Markup ---------------
  <div class="table table_type_cards">
  <table class="table__table">
  <thead class="table__head">
  <tr class="table__row">
  <th class="table__header">
  header 1
  </th>
  <th class="table__header">
  header 2
  </th>
  </tr>
  </thead>
  <tbody class="table__body">
  <tr class="table__row">
  <td class="table__data">
  <div class="table__label">
  header 1
  </div>
  <div class="table__content">
  cell 1
  </div>
  </td>
  <td class="table__data">
  <div class="table__label">
  header 2
  </div>
  <div class="table__content">
  cell 2
  </div>
  </td>
  </tr>
  </tbody>
  </table>
  </div>
  ````````````````````````````````

##### `Type`
- Type: `FlexiTableType`
- Description: The `FlexiTableBlock`'s type.
  This value is used in the root element's default [modifier class](https://en.bem.info/methodology/quick-start/#modifier),
  "<`BlockName`>_type_<`Type`>".
  This value affects the structure of generated HTML.
- Default: `FlexiTableType.Cards`
- Examples:
  Cards type:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  @{ "type": "cards" }
  +--------------+--------------+
  | header 1     | header 2     |
  +==============+==============+
  | cell 1       | cell 2       |
  +--------------+--------------+
  --------------- Expected Markup ---------------
  <div class="flexi-table flexi-table_type_cards">
  <table class="flexi-table__table">
  <thead class="flexi-table__head">
  <tr class="flexi-table__row">
  <th class="flexi-table__header">
  header 1
  </th>
  <th class="flexi-table__header">
  header 2
  </th>
  </tr>
  </thead>
  <tbody class="flexi-table__body">
  <tr class="flexi-table__row">
  <td class="flexi-table__data">
  <div class="flexi-table__label">
  header 1
  </div>
  <div class="flexi-table__content">
  cell 1
  </div>
  </td>
  <td class="flexi-table__data">
  <div class="flexi-table__label">
  header 2
  </div>
  <div class="flexi-table__content">
  cell 2
  </div>
  </td>
  </tr>
  </tbody>
  </table>
  </div>
  ````````````````````````````````
  Fixed titles type: 
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  @{ "type": "fixedTitles" }
  | header 1 | header 2 |
  |----------|----------|
  | cell 1   | cell 2   |
  --------------- Expected Markup ---------------
  <div class="flexi-table flexi-table_type_fixed-titles">
  <table class="flexi-table__table">
  <thead class="flexi-table__head">
  <tr class="flexi-table__row">
  <th class="flexi-table__header">
  header 1
  </th>
  <th class="flexi-table__header">
  header 2
  </th>
  </tr>
  </thead>
  <tbody class="flexi-table__body">
  <tr class="flexi-table__row">
  <td class="flexi-table__data">
  cell 1
  </td>
  <td class="flexi-table__data">
  cell 2
  </td>
  </tr>
  </tbody>
  </table>
  </div>
  ````````````````````````````````
  Unresponsive type:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  @{ "type": "unresponsive" }
  +--------------+--------------+
  | header 1     | header 2     |
  +==============+==============+
  | cell 1       | cell 2       |
  +--------------+--------------+
  --------------- Expected Markup ---------------
  <div class="flexi-table flexi-table_type_unresponsive">
  <table class="flexi-table__table">
  <thead class="flexi-table__head">
  <tr class="flexi-table__row">
  <th class="flexi-table__header">
  header 1
  </th>
  <th class="flexi-table__header">
  header 2
  </th>
  </tr>
  </thead>
  <tbody class="flexi-table__body">
  <tr class="flexi-table__row">
  <td class="flexi-table__data">
  cell 1
  </td>
  <td class="flexi-table__data">
  cell 2
  </td>
  </tr>
  </tbody>
  </table>
  </div>
  ````````````````````````````````

##### `Attributes`
- Type: `IDictionary<string, string>`
- Description: The HTML attributes for the `FlexiTableBlock`'s root element.
  Attribute names must be lowercase.
  If classes are specified, they are appended to default classes. This facilitates [BEM mixes](https://en.bem.info/methodology/quick-start/#mix).
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
  | header 1 | header 2 |
  |----------|----------|
  | cell 1   | cell 2   |
  --------------- Expected Markup ---------------
  <div class="flexi-table flexi-table_type_cards my-custom-class" id="my-custom-id">
  <table class="flexi-table__table">
  <thead class="flexi-table__head">
  <tr class="flexi-table__row">
  <th class="flexi-table__header">
  header 1
  </th>
  <th class="flexi-table__header">
  header 2
  </th>
  </tr>
  </thead>
  <tbody class="flexi-table__body">
  <tr class="flexi-table__row">
  <td class="flexi-table__data">
  <div class="flexi-table__label">
  header 1
  </div>
  <div class="flexi-table__content">
  cell 1
  </div>
  </td>
  <td class="flexi-table__data">
  <div class="flexi-table__label">
  header 2
  </div>
  <div class="flexi-table__content">
  cell 2
  </div>
  </td>
  </tr>
  </tbody>
  </table>
  </div>
  ````````````````````````````````

### `FlexiTableBlocksExtensionOptions`
Options for the FlexiTableBlocks extension. There are two ways to specify these options:
- Pass a `FlexiTableBlocksExtensionOptions` when calling `MarkdownPipelineBuilderExtensions.UseFlexiTableBlocks(this MarkdownPipelineBuilder pipelineBuilder, IFlexiTableBlocksExtensionOptions options)`.
- Insert a `FlexiTableBlocksExtensionOptions` into a `MarkdownParserContext.Properties` with key `typeof(IFlexiTableBlocksExtensionOptions)`. Pass the `MarkdownParserContext` when you call a markdown processing method
  like `Markdown.ToHtml(markdown, stringWriter, markdownPipeline, yourMarkdownParserContext)`.  
  This method allows for different extension options when reusing a pipeline. Options specified using this method take precedence.

#### Constructor Parameters

##### `defaultBlockOptions`
- Type: `IFlexiTableBlockOptions`
- Description: Default `IFlexiTableBlockOptions` for all `FlexiTableBlock`s.
  If this value is `null`, a `FlexiTableBlockOptions` with default values is used.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extension Options ---------------
  {
      "flexiTableBlocks": {
          "defaultBlockOptions": {
              "blockName": "table",
              "type": "unresponsive",
              "attributes": {
                  "class": "block"
              }
          }
      }
  }
  --------------- Markdown ---------------
  +--------------+--------------+
  | header 1     | header 2     |
  +==============+==============+
  | cell 1       | cell 2       |
  +--------------+--------------+
  --------------- Expected Markup ---------------
  <div class="table table_type_unresponsive block">
  <table class="table__table">
  <thead class="table__head">
  <tr class="table__row">
  <th class="table__header">
  header 1
  </th>
  <th class="table__header">
  header 2
  </th>
  </tr>
  </thead>
  <tbody class="table__body">
  <tr class="table__row">
  <td class="table__data">
  cell 1
  </td>
  <td class="table__data">
  cell 2
  </td>
  </tr>
  </tbody>
  </table>
  </div>
  ````````````````````````````````
  `defaultBlockOptions` has lower precedence than block specific options:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Extension Options ---------------
  {
      "flexiTableBlocks": {
          "defaultBlockOptions": {
              "type": "unresponsive"
          }
      }
  }
  --------------- Markdown ---------------
  | header 1 | header 2 |
  |----------|----------|
  | cell 1   | cell 2   |

  @{ "type": "cards" }
  +--------------+--------------+
  | header 1     | header 2     |
  +==============+==============+
  | cell 1       | cell 2       |
  +--------------+--------------+
  --------------- Expected Markup ---------------
  <div class="flexi-table flexi-table_type_unresponsive">
  <table class="flexi-table__table">
  <thead class="flexi-table__head">
  <tr class="flexi-table__row">
  <th class="flexi-table__header">
  header 1
  </th>
  <th class="flexi-table__header">
  header 2
  </th>
  </tr>
  </thead>
  <tbody class="flexi-table__body">
  <tr class="flexi-table__row">
  <td class="flexi-table__data">
  cell 1
  </td>
  <td class="flexi-table__data">
  cell 2
  </td>
  </tr>
  </tbody>
  </table>
  </div>
  <div class="flexi-table flexi-table_type_cards">
  <table class="flexi-table__table">
  <thead class="flexi-table__head">
  <tr class="flexi-table__row">
  <th class="flexi-table__header">
  header 1
  </th>
  <th class="flexi-table__header">
  header 2
  </th>
  </tr>
  </thead>
  <tbody class="flexi-table__body">
  <tr class="flexi-table__row">
  <td class="flexi-table__data">
  <div class="flexi-table__label">
  header 1
  </div>
  <div class="flexi-table__content">
  cell 1
  </div>
  </td>
  <td class="flexi-table__data">
  <div class="flexi-table__label">
  header 2
  </div>
  <div class="flexi-table__content">
  cell 2
  </div>
  </td>
  </tr>
  </tbody>
  </table>
  </div>
  ````````````````````````````````
