---
blockOptions: "../src/FlexiBlocks/Extensions/FlexiCodeBlocks/FlexiCodeBlockOptions.cs"
utilityTypes: ["../src/FlexiBlocks/Shared/Models/LineRange.cs", "../src/FlexiBlocks/Extensions/FlexiCodeBlocks/NumberedLineRange.cs", "../src/FlexiBlocks/Extensions/FlexiCodeBlocks/PhraseGroup.cs"]
extensionOptions: "../src/FlexiBlocks/Extensions/FlexiCodeBlocks/FlexiCodeBlocksExtensionOptions.cs"
---

# FlexiCodeBlocks
A FlexiCodeBlock displays code. FlexiCodeBlocks enhance code aesthetically and functionally with features like syntax highlighting,
line highlighting, phrase highlighting, line numbering and more.

## Prerequisites
To use syntax highlighting, [NodeJS](https://nodejs.org/en/) must be installed and node.exe's directory must be added to the `Path` environment variable.

## Usage
```csharp
using Markdig;
using Jering.Markdig.Extensions.FlexiBlocks;

...
var markdownPipelineBuilder = new MarkdownPipelineBuilder();
markdownPipelineBuilder.UseFlexiCodeBlocks(/* Optional extension options */);

MarkdownPipeline markdownPipeline = markdownPipelineBuilder.Build();

string markdown = @"```
public string ExampleFunction(string arg)
{
    // Example comment
    return arg + ""dummyString"";
}
```"
string html = Markdown.ToHtml(markdown, markdownPipeline);
string expectedHtml = @"<div class=""flexi-code flexi-code_no_title flexi-code_no_syntax-highlights flexi-code_no_line-numbers flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases"">
<header class=""flexi-code__header"">
<span class=""flexi-code__title""></span>
<button class=""flexi-code__copy-button"" title=""Copy code"" aria-label=""Copy code"">
<svg class=""flexi-code__copy-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""18px"" height=""18px"" viewBox=""0 0 18 18""><path fill=""none"" d=""M0,0h18v18H0V0z""/><path d=""M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z""/></svg>
</button>
</header>
<pre class=""flexi-code__pre""><code class=""flexi-code__code"">public string ExampleFunction(string arg)
{
    // Example comment
    return arg + &quot;dummyString&quot;;
}</code></pre>
</div>";

Assert.Equal(expectedHtml, html)
```

# Basics
In markdown, a FlexiCodeBlock is a sequence of [fenced](https://spec.commonmark.org/0.28/#fenced-code-blocks) or [indented](https://spec.commonmark.org/0.28/#indented-code-blocks) lines - identical to 
[CommonMark](https://spec.commonmark.org/0.28/) code blocks. For example:

```````````````````````````````` none
--------------- Markdown ---------------
```
public string ExampleFunction(string arg)
{
    // Example comment
    return arg + "dummyString";
}
```
--------------- Expected Markup ---------------
<div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
<header class="flexi-code__header">
<span class="flexi-code__title"></span>
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

! By default, a FlexiCodeBlock has a header, a copy button and more. Each element is assigned a default class. Default classes comply with 
! [BEM methodology](https://en.bem.info/).  
!
! FlexiCodeBlocks can be customized, we explain how in [options].

Like [CommonMark](https://spec.commonmark.org/0.28/) code blocks, a FlexiCodeBlock's fence can consist of tildes:

```````````````````````````````` none
--------------- Markdown ---------------
~~~
<html>
    <head>
        <title>Example Page</title>
    </head>
    <body>
        <p>Example content.</p>
    </body>
</html>
~~~
--------------- Expected Markup ---------------
<div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
<header class="flexi-code__header">
<span class="flexi-code__title"></span>
<button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
<svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
</button>
</header>
<pre class="flexi-code__pre"><code class="flexi-code__code">&lt;html&gt;
    &lt;head&gt;
        &lt;title&gt;Example Page&lt;/title&gt;
    &lt;/head&gt;
    &lt;body&gt;
        &lt;p&gt;Example content.&lt;/p&gt;
    &lt;/body&gt;
&lt;/html&gt;
</code></pre>
</div>
````````````````````````````````

The following is an indented FlexiCodeBlock:
```````````````````````````````` none
--------------- Markdown ---------------
    public exampleFunction(arg: string): string {
        // Example comment
        return arg + "dummyString";
    }
--------------- Expected Markup ---------------
<div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
<header class="flexi-code__header">
<span class="flexi-code__title"></span>
<button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
<svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
</button>
</header>
<pre class="flexi-code__pre"><code class="flexi-code__code">public exampleFunction(arg: string): string {
    // Example comment
    return arg + &quot;dummyString&quot;;
}
</code></pre>
</div>
````````````````````````````````

## Options
### `FlexiCodeBlockOptions`
Options for a FlexiCodeBlock. To specify `FlexiCodeBlockOptions` for a FlexiCodeBlock, the [Options](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/OptionsBlocksSpecs.md#options) extension must be enabled.

#### Properties

##### `BlockName`
- Type: `string`
- Description: The `FlexiCodeBlock`'s [BEM block name](https://en.bem.info/methodology/naming-convention/#block-name).
  In compliance with [BEM methodology](https://en.bem.info), this value is the `FlexiCodeBlock`'s root element's class as well as the prefix for all other classes in the block.
  This value should contain only valid [CSS class characters](https://www.w3.org/TR/CSS21/syndata.html#characters).
  If this value is `null`, whitespace or an empty string, the `FlexiCodeBlock`'s block name is "flexi-code".
- Default: "flexi-code"
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  @{
      "blockName": "code"
  }
  ```
  public string ExampleFunction(string arg)
  {
      // Example comment
      return arg + "dummyString";
  }
  ```
  --------------- Expected Markup ---------------
  <div class="code code_no_title code_has_copy-icon code_no_syntax-highlights code_no_line-numbers code_has_omitted-lines-icon code_no_highlighted-lines code_no_highlighted-phrases">
  <header class="code__header">
  <span class="code__title"></span>
  <button class="code__copy-button" title="Copy code" aria-label="Copy code">
  <svg class="code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
  </button>
  </header>
  <pre class="code__pre"><code class="code__code">public string ExampleFunction(string arg)
  {
      // Example comment
      return arg + &quot;dummyString&quot;;
  }
  </code></pre>
  </div>
  ````````````````````````````````

##### `Title`
- Type: `string`
- Description: The `FlexiCodeBlock`'s title.
  If this value is `null`, whitespace or an empty string, no title is rendered.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  @{ "title" : "ExampleDocument.cs" }
  ```
  public string ExampleFunction(string arg)
  {
      // Example comment
      return arg + "dummyString";
  }
  ```
  --------------- Expected Markup ---------------
  <div class="flexi-code flexi-code_has_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
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

##### `CopyIcon`
- Type: `string`
- Description: The `FlexiCodeBlock`'s copy button icon as an HTML fragment.
  The class "<`BlockName`>__copy-icon" is assigned to this fragment's first start tag.
  If this value is `null`, whitespace or an empty string, no copy icon is rendered.
- Default: a copy file icon
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  @{ "copyIcon": "<svg><use xlink:href=\"#material-design-copy\"/></svg>" }
  ```
  public string ExampleFunction(string arg)
  {
      // Example comment
      return arg + "dummyString";
  }
  ```
  --------------- Expected Markup ---------------
  <div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
  <header class="flexi-code__header">
  <span class="flexi-code__title"></span>
  <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
  <svg class="flexi-code__copy-icon"><use xlink:href="#material-design-copy"/></svg>
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
  No copy icon is are rendered if this value is `null`, whitespace or an empty string:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  @{ "copyIcon": null }
  ```
  public string ExampleFunction(string arg)
  {
      // Example comment
      return arg + "dummyString";
  }
  ```
  --------------- Expected Markup ---------------
  <div class="flexi-code flexi-code_no_title flexi-code_no_copy-icon flexi-code_no_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
  <header class="flexi-code__header">
  <span class="flexi-code__title"></span>
  <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
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

##### `Language`
- Type: `string`
- Description: The programming language of the `FlexiCodeBlock`'s code.
  If `SyntaxHighlighter` is not `SyntaxHighlighter.None`, this value is passed to the chosen syntax highlighter.
  Therefore, this value must be a language alias supported by the chosen syntax highlighter.
  [Valid language aliases for Prism.](https://prismjs.com/index.html#languages-list)
  [Valid language aliases for HighlightJS](http://highlightjs.readthedocs.io/en/latest/css-classes-reference.html#language-names-and-aliases).
  The class "<`BlockName`>__code_language-<language>" is assigned to the `FlexiCodeBlock`'s root element.
  If this value is `null`, whitespace or an empty string, syntax highlighting is disabled and no language class is assigned to the root element.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  @{ "language": "csharp" }
  ```
  public string ExampleFunction(string arg)
  {
      // Example comment
      return arg + "dummyString";
  }
  ```
  --------------- Expected Markup ---------------
  <div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_language-csharp flexi-code_has_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
  <header class="flexi-code__header">
  <span class="flexi-code__title"></span>
  <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
  <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
  </button>
  </header>
  <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="token keyword">public</span> <span class="token keyword">string</span> <span class="token function">ExampleFunction</span><span class="token punctuation">(</span><span class="token keyword">string</span> arg<span class="token punctuation">)</span>
  <span class="token punctuation">{</span>
      <span class="token comment">// Example comment</span>
      <span class="token keyword">return</span> arg <span class="token operator">+</span> <span class="token string">"dummyString"</span><span class="token punctuation">;</span>
  <span class="token punctuation">}</span>
  </code></pre>
  </div>
  ````````````````````````````````

##### `SyntaxHighlighter`
- Type: `SyntaxHighlighter`
- Description: The syntax highlighter to highlight the `FlexiCodeBlock`'s code with.
  If this value is `SyntaxHighlighter.None`, or `Language` is `null`, whitespace or an empty string,
  syntax highlighting is disabled.
- Default: `SyntaxHighlighter.Prism`
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  @{
      "syntaxHighlighter": "highlightJS",
      "language": "typescript"
  }
  ```
  public exampleFunction(arg: string): string {
      // Example comment
      return arg + "dummyString";
  }
  ```
  --------------- Expected Markup ---------------
  <div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_language-typescript flexi-code_has_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
  <header class="flexi-code__header">
  <span class="flexi-code__title"></span>
  <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
  <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
  </button>
  </header>
  <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="hljs-keyword">public</span> exampleFunction(arg: <span class="hljs-built_in">string</span>): <span class="hljs-built_in">string</span> {
      <span class="hljs-comment">// Example comment</span>
      <span class="hljs-keyword">return</span> arg + <span class="hljs-string">"dummyString"</span>;
  }
  </code></pre>
  </div>
  ````````````````````````````````

##### `LineNumbers`
- Type: `IList<NumberedLineRange>`
- Description: The `NumberedLineRange`s specifying line numbers to render.
  If line numbers are specified for some but not all lines, an omitted lines icon is rendered for each line with no line number. You can customize
  the icon by specifying `OmittedLinesIcon`.
  If line numbers are specified for some but not all lines, an omitted lines notice is inserted into each empty line with no line number.
  The notice "line {0} omitted for brevity" is inserted if a single line is omitted and the notice "lines {0} to {1} omitted for brevity",
  is inserted if multiple lines are omitted.
  Contained ranges must not overlap.
  If this value is `null`, no line numbers are rendered.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  @{
      "lineNumbers": [
          { "startLine": 2, "endLine": 8, "startNumber": 4 },
          { "startLine": 10, "endLine": -2, "startNumber": 32 }
      ]
  }
  ```

  public class ExampleClass
  {
      public string ExampleFunction1(string arg)
      {
          // Example comment
          return arg + "dummyString";
      }

      public string ExampleFunction3(string arg)
      {
          // Example comment
          return arg + "dummyString";
      }
  }

  ```
  --------------- Expected Markup ---------------
  <div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_has_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
  <header class="flexi-code__header">
  <span class="flexi-code__title"></span>
  <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
  <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
  </button>
  </header>
  <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="flexi-code__line-prefix"><svg class="flexi-code__omitted-lines-icon" xmlns="http://www.w3.org/2000/svg" width="2px" height="10px" viewBox="0 0 2 10"><rect shape-rendering="crispEdges" width="2" height="2"/><rect shape-rendering="crispEdges" y="4" width="2" height="2"/><rect shape-rendering="crispEdges" y="8" width="2" height="2"/></svg></span><span class="flexi-code__line flexi-code__line_omitted-lines">Lines 1 to 3 omitted for brevity</span>
  <span class="flexi-code__line-prefix">4</span><span class="flexi-code__line">public class ExampleClass</span>
  <span class="flexi-code__line-prefix">5</span><span class="flexi-code__line">{</span>
  <span class="flexi-code__line-prefix">6</span><span class="flexi-code__line">    public string ExampleFunction1(string arg)</span>
  <span class="flexi-code__line-prefix">7</span><span class="flexi-code__line">    {</span>
  <span class="flexi-code__line-prefix">8</span><span class="flexi-code__line">        // Example comment</span>
  <span class="flexi-code__line-prefix">9</span><span class="flexi-code__line">        return arg + &quot;dummyString&quot;;</span>
  <span class="flexi-code__line-prefix">10</span><span class="flexi-code__line">    }</span>
  <span class="flexi-code__line-prefix"><svg class="flexi-code__omitted-lines-icon" xmlns="http://www.w3.org/2000/svg" width="2px" height="10px" viewBox="0 0 2 10"><rect shape-rendering="crispEdges" width="2" height="2"/><rect shape-rendering="crispEdges" y="4" width="2" height="2"/><rect shape-rendering="crispEdges" y="8" width="2" height="2"/></svg></span><span class="flexi-code__line flexi-code__line_omitted-lines">Lines 11 to 31 omitted for brevity</span>
  <span class="flexi-code__line-prefix">32</span><span class="flexi-code__line">    public string ExampleFunction3(string arg)</span>
  <span class="flexi-code__line-prefix">33</span><span class="flexi-code__line">    {</span>
  <span class="flexi-code__line-prefix">34</span><span class="flexi-code__line">        // Example comment</span>
  <span class="flexi-code__line-prefix">35</span><span class="flexi-code__line">        return arg + &quot;dummyString&quot;;</span>
  <span class="flexi-code__line-prefix">36</span><span class="flexi-code__line">    }</span>
  <span class="flexi-code__line-prefix">37</span><span class="flexi-code__line">}</span>
  <span class="flexi-code__line-prefix"><svg class="flexi-code__omitted-lines-icon" xmlns="http://www.w3.org/2000/svg" width="2px" height="10px" viewBox="0 0 2 10"><rect shape-rendering="crispEdges" width="2" height="2"/><rect shape-rendering="crispEdges" y="4" width="2" height="2"/><rect shape-rendering="crispEdges" y="8" width="2" height="2"/></svg></span><span class="flexi-code__line flexi-code__line_omitted-lines">Lines 38 to the end omitted for brevity</span>
  </code></pre>
  </div>
  ````````````````````````````````

##### `OmittedLinesIcon`
- Type: `string`
- Description: The `FlexiCodeBlock`'s omitted lines icon as an HTML fragment.
  The class "<`BlockName`>__omitted-lines-icon" is assigned to this fragment's first start tag.
  If this value is `null`, whitespace or an empty string, no omitted lines icons are rendered.
- Default: a vertical ellipsis icon
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  @{
      "omittedLinesIcon": "<svg><use xlink:href=\"#material-design-more-vert\"/></svg>",
      "lineNumbers": [{"endLine": 2}, {"startLine": 4, "startNumber":10}]
  }
  ```
  public string ExampleFunction(string arg)
  {

  }
  ```
  --------------- Expected Markup ---------------
  <div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_has_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
  <header class="flexi-code__header">
  <span class="flexi-code__title"></span>
  <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
  <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
  </button>
  </header>
  <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="flexi-code__line-prefix">1</span><span class="flexi-code__line">public string ExampleFunction(string arg)</span>
  <span class="flexi-code__line-prefix">2</span><span class="flexi-code__line">{</span>
  <span class="flexi-code__line-prefix"><svg class="flexi-code__omitted-lines-icon"><use xlink:href="#material-design-more-vert"/></svg></span><span class="flexi-code__line flexi-code__line_omitted-lines">Lines 3 to 9 omitted for brevity</span>
  <span class="flexi-code__line-prefix">10</span><span class="flexi-code__line">}</span>
  </code></pre>
  </div>
  ````````````````````````````````
  No omitted lines icons are rendered if this value is `null`, white space or an empty string:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  @{
      "omittedLinesIcon": null,
      "lineNumbers": [{"endLine": 2}, {"startLine": 4, "startNumber":10}]
  }
  ```
  public string ExampleFunction(string arg)
  {

  }
  ```
  --------------- Expected Markup ---------------
  <div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_has_line-numbers flexi-code_no_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
  <header class="flexi-code__header">
  <span class="flexi-code__title"></span>
  <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
  <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
  </button>
  </header>
  <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="flexi-code__line-prefix">1</span><span class="flexi-code__line">public string ExampleFunction(string arg)</span>
  <span class="flexi-code__line-prefix">2</span><span class="flexi-code__line">{</span>
  <span class="flexi-code__line-prefix"></span><span class="flexi-code__line flexi-code__line_omitted-lines">Lines 3 to 9 omitted for brevity</span>
  <span class="flexi-code__line-prefix">10</span><span class="flexi-code__line">}</span>
  </code></pre>
  </div>
  ````````````````````````````````

##### `HighlightedLines`
- Type: `IList<LineRange>`
- Description: The `LineRange`s specifying lines to highlight.
  If this value is `null`, no lines are highlighted.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  @{
      "highlightedLines": [
          { "endLine": 1 },
          { "startLine": 3, "endLine": 4 }
      ]
  }
  ```
  public string ExampleFunction(string arg)
  {
      // Example comment
      return arg + "dummyString";
  }
  ```
  --------------- Expected Markup ---------------
  <div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_has_highlighted-lines flexi-code_no_highlighted-phrases">
  <header class="flexi-code__header">
  <span class="flexi-code__title"></span>
  <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
  <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
  </button>
  </header>
  <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="flexi-code__line flexi-code__line_highlighted">public string ExampleFunction(string arg)</span>
  {
  <span class="flexi-code__line flexi-code__line_highlighted">    // Example comment</span>
  <span class="flexi-code__line flexi-code__line_highlighted">    return arg + &quot;dummyString&quot;;</span>
  }
  </code></pre>
  </div>
  ````````````````````````````````

##### `HighlightedPhrases`
- Type: `IList<PhraseGroup>`
- Description: The `PhraseGroup`s specifying phrases to highlight.
  If the regex expression of a `PhraseGroup` has groups, only groups are highlighted, entire matches are not highlighted.
  If the regex expression of a `PhraseGroup` has no groups, entire matches are highlighted.
  If this value is `null`, no phrases are highlighted.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  @{
      "highlightedPhrases": [
          { "regex": "return (.*?);", "includedMatches": [1] },
          { "regex": "string arg" }
      ]
  }
  ```
  public class ExampleClass
  {
      public string ExampleFunction1(string arg)
      {
          // Example comment
          return arg + "dummyString";
      }

      public string ExampleFunction2(string arg)
      {
          // Example comment
          return arg + "dummyString";
      }
  }
  ```
  --------------- Expected Markup ---------------
  <div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_has_highlighted-phrases">
  <header class="flexi-code__header">
  <span class="flexi-code__title"></span>
  <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
  <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
  </button>
  </header>
  <pre class="flexi-code__pre"><code class="flexi-code__code">public class ExampleClass
  {
      public string ExampleFunction1(<span class="flexi-code__highlighted-phrase">string arg</span>)
      {
          // Example comment
          return arg + &quot;dummyString&quot;;
      }

      public string ExampleFunction2(<span class="flexi-code__highlighted-phrase">string arg</span>)
      {
          // Example comment
          return <span class="flexi-code__highlighted-phrase">arg + &quot;dummyString&quot;</span>;
      }
  }
  </code></pre>
  </div>
  ````````````````````````````````

##### `RenderingMode`
- Type: `FlexiCodeBlockRenderingMode`
- Description: The `FlexiCodeBlock`'s rendering mode.
- Default: `FlexiCodeBlockRenderingMode.Standard`
- Examples:
  This value is `FlexiCodeBlockRenderingMode.Standard` by default:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  ```
  public string ExampleFunction(string arg)
  {
      // Example comment
      return arg + "dummyString";
  }
  ```
  --------------- Expected Markup ---------------
  <div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
  <header class="flexi-code__header">
  <span class="flexi-code__title"></span>
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
  If this value is `FlexiCodeBlockRenderingMode.Classic`, the `FlexiCodeBlock` is rendered the same way [code blocks](https://spec.commonmark.org/0.28/#indented-code-blocks) are 
  rendered in CommonMark Spec examples:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Markdown ---------------
  @{ "renderingMode": "classic" }
  ```
  public string ExampleFunction(string arg)
  {
      // Example comment
      return arg + "dummyString";
  }
  ```
  --------------- Expected Markup ---------------
  <pre><code>public string ExampleFunction(string arg)
  {
      // Example comment
      return arg + &quot;dummyString&quot;;
  }
  </code></pre>
  ````````````````````````````````

##### `Attributes`
- Type: `IDictionary<string, string>`
- Description: The HTML attributes for the `FlexiCodeBlock`'s root element.
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
          "id" : "code-1",
          "class" : "block"
      }
  }
  ```
  public string ExampleFunction(string arg)
  {
      // Example comment
      return arg + "dummyString";
  }
  ```
  --------------- Expected Markup ---------------
  <div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases block" id="code-1">
  <header class="flexi-code__header">
  <span class="flexi-code__title"></span>
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

### `LineRange`
Represents a range of lines.

#### Properties

##### `Start`
- Type: `int`
- Description: The line number of the `LineRange`'s start line.
  If this value is `-n`, the start line is the nth last line. For example, if this value is `-2`, the start line is the 2nd last line.
  This value must not be `0`.
- Default: `1`

##### `End`
- Type: `int`
- Description: The line number of the `LineRange`'s end line.
  If this value is `-n`, the end line is the nth last line. For example, if this value is `-2`, the end line is the 2nd last line.
  This value must not be `0` or an integer representing a line before the start line.
- Default: `-1`

### `NumberedLineRange`
Represents a range of lines with an associated sequence of numbers.

#### Properties

##### `StartLine`
- Type: `int`
- Description: The line number of the `NumberedLineRange`'s start line.
  If this value is `-n`, the start line is the nth last line. For example, if this value is `-2`, the start line is the 2nd last line.
  This value must not be `0`.
- Default: `1`

##### `EndLine`
- Type: `int`
- Description: The line number of the `NumberedLineRange`'s end line.
  If this value is `-n`, the end line is the nth last line. For example, if this value is `-2`, the end line is the 2nd last line.
  This value must not be `0` or an integer representing a line before the start line.
- Default: `-1`

##### `StartNumber`
- Type: `int`
- Description: The number associated with this `NumberedLineRange`'s start line.
  The number associated with each subsequent line is incremented by 1.
  This value must be greater than 0.
- Default: `1`

### `PhraseGroup`
Represents phrases in a body of text.

#### Properties

##### `Regex`
- Type: `string`
- Description: The regex expression for the `PhraseGroup`.
  This value is required.

##### `IncludedMatches`
- Type: `int[]`
- Description: The indices of the regex matches included in the `PhraseGroup`.
  This array can contain negative values. If a value is `-n`, the nth last match is included. For example,
  if a value is `-2`, the 2nd last match is included.
  If this value is `null` or empty, all matches are included.
- Default: `null`

### `FlexiCodeBlocksExtensionOptions`
Options for the FlexiCodeBlocks extension. There are two ways to specify these options:
- Pass a `FlexiCodeBlocksExtensionOptions` when calling `MarkdownPipelineBuilderExtensions.UseFlexiCodeBlocks(this MarkdownPipelineBuilder pipelineBuilder, IFlexiCodeBlocksExtensionOptions options)`.
- Insert a `FlexiCodeBlocksExtensionOptions` into a `MarkdownParserContext.Properties` with key `typeof(IFlexiCodeBlocksExtensionOptions)`. Pass the `MarkdownParserContext` when you call a markdown processing method
  like `Markdown.ToHtml(markdown, stringWriter, markdownPipeline, yourMarkdownParserContext)`.  
  This method allows for different extension options when reusing a pipeline. Options specified using this method take precedence.

#### Constructor Parameters

##### `defaultBlockOptions`
- Type: `IFlexiCodeBlockOptions`
- Description: Default `IFlexiCodeBlockOptions` for all `FlexiCodeBlock`s.
  If this value is `null`, a `FlexiCodeBlockOptions` with default values is used.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extension Options ---------------
  {
      "flexiCodeBlocks": {
          "defaultBlockOptions": {
              "blockName": "code",
              "title": "ExampleDocument.cs",
              "copyIcon": "<svg><use xlink:href=\"#material-design-copy\"/></svg>",
              "language": "html",
              "syntaxHighlighter": "highlightjs",
              "lineNumbers": [{}],
              "omittedLinesIcon": "<svg><use xlink:href=\"#material-design-more-vert\"/></svg>",
              "highlightedLines": [{"startLine": 3, "endLine": 3}],
              "highlightedPhrases": [{"regex":"</.*?>"}],
              "attributes": {"class": "block"}
          }
      }
  }
  --------------- Markdown ---------------
  ```
  <html>
      <head>
          <title>Example Page</title>
      </head>
      <body>
          <p>Example content.</p>
      </body>
  </html>
  ```
  --------------- Expected Markup ---------------
  <div class="code code_has_title code_has_copy-icon code_language-html code_has_syntax-highlights code_has_line-numbers code_has_omitted-lines-icon code_has_highlighted-lines code_has_highlighted-phrases block">
  <header class="code__header">
  <span class="code__title">ExampleDocument.cs</span>
  <button class="code__copy-button" title="Copy code" aria-label="Copy code">
  <svg class="code__copy-icon"><use xlink:href="#material-design-copy"/></svg>
  </button>
  </header>
  <pre class="code__pre"><code class="code__code"><span class="code__line-prefix">1</span><span class="code__line"><span class="hljs-tag">&lt;<span class="hljs-name">html</span>&gt;</span></span>
  <span class="code__line-prefix">2</span><span class="code__line">    <span class="hljs-tag">&lt;<span class="hljs-name">head</span>&gt;</span></span>
  <span class="code__line-prefix">3</span><span class="code__line code__line_highlighted">        <span class="hljs-tag">&lt;<span class="hljs-name">title</span>&gt;</span>Example Page<span class="code__highlighted-phrase"><span class="hljs-tag">&lt;/<span class="hljs-name">title</span>&gt;</span></span></span>
  <span class="code__line-prefix">4</span><span class="code__line">    <span class="code__highlighted-phrase"><span class="hljs-tag">&lt;/<span class="hljs-name">head</span>&gt;</span></span></span>
  <span class="code__line-prefix">5</span><span class="code__line">    <span class="hljs-tag">&lt;<span class="hljs-name">body</span>&gt;</span></span>
  <span class="code__line-prefix">6</span><span class="code__line">        <span class="hljs-tag">&lt;<span class="hljs-name">p</span>&gt;</span>Example content.<span class="code__highlighted-phrase"><span class="hljs-tag">&lt;/<span class="hljs-name">p</span>&gt;</span></span></span>
  <span class="code__line-prefix">7</span><span class="code__line">    <span class="code__highlighted-phrase"><span class="hljs-tag">&lt;/<span class="hljs-name">body</span>&gt;</span></span></span>
  <span class="code__line-prefix">8</span><span class="code__line"><span class="code__highlighted-phrase"><span class="hljs-tag">&lt;/<span class="hljs-name">html</span>&gt;</span></span></span>
  </code></pre>
  </div>
  ````````````````````````````````

  `defaultBlockOptions` has lower precedence than block specific options:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  OptionsBlocks
  --------------- Extension Options ---------------
  {
      "flexiCodeBlocks": {
          "defaultBlockOptions": {
              "lineNumbers": [{}]
          }
      }
  }
  --------------- Markdown ---------------
  ```
  public string ExampleFunction(string arg)
  {
      // Example comment
      return arg + "dummyString";
  }
  ```

  @{
      "lineNumbers": [
          {
              "startLine": 2, "startNumber": 25
          }
      ]
  }
  ```

  body {
      display: flex;
      align-items: center;
      font-size: 13px;
  }
  ```
  --------------- Expected Markup ---------------
  <div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_has_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
  <header class="flexi-code__header">
  <span class="flexi-code__title"></span>
  <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
  <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
  </button>
  </header>
  <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="flexi-code__line-prefix">1</span><span class="flexi-code__line">public string ExampleFunction(string arg)</span>
  <span class="flexi-code__line-prefix">2</span><span class="flexi-code__line">{</span>
  <span class="flexi-code__line-prefix">3</span><span class="flexi-code__line">    // Example comment</span>
  <span class="flexi-code__line-prefix">4</span><span class="flexi-code__line">    return arg + &quot;dummyString&quot;;</span>
  <span class="flexi-code__line-prefix">5</span><span class="flexi-code__line">}</span>
  </code></pre>
  </div>
  <div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_has_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
  <header class="flexi-code__header">
  <span class="flexi-code__title"></span>
  <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
  <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
  </button>
  </header>
  <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="flexi-code__line-prefix"><svg class="flexi-code__omitted-lines-icon" xmlns="http://www.w3.org/2000/svg" width="2px" height="10px" viewBox="0 0 2 10"><rect shape-rendering="crispEdges" width="2" height="2"/><rect shape-rendering="crispEdges" y="4" width="2" height="2"/><rect shape-rendering="crispEdges" y="8" width="2" height="2"/></svg></span><span class="flexi-code__line flexi-code__line_omitted-lines">Lines 1 to 24 omitted for brevity</span>
  <span class="flexi-code__line-prefix">25</span><span class="flexi-code__line">body {</span>
  <span class="flexi-code__line-prefix">26</span><span class="flexi-code__line">    display: flex;</span>
  <span class="flexi-code__line-prefix">27</span><span class="flexi-code__line">    align-items: center;</span>
  <span class="flexi-code__line-prefix">28</span><span class="flexi-code__line">    font-size: 13px;</span>
  <span class="flexi-code__line-prefix">29</span><span class="flexi-code__line">}</span>
  </code></pre>
  </div>
  ````````````````````````````````

## Mechanics
### Intersecting HTML Elements
Syntax and phrase elements that intersect line elements get split. Order of elements is preserved after splitting:

```````````````````````````````` none
--------------- Extra Extensions ---------------
OptionsBlocks
--------------- Markdown ---------------
@{
    "language": "csharp",
    "highlightedLines": [
        { "startLine": 3, "endLine": 3 },
        { "startLine": 8, "endLine": 8 }
    ],
    "highlightedPhrases": [
        { "regex": "Multiline.*?1" },
        { "regex": "/.*?/", "includedMatches": [1] }
    ]
}
```
/* 
    Multiline
    comment
    1
*/
/* 
    Multiline
    comment
    2
*/
```
--------------- Expected Markup ---------------
<div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_language-csharp flexi-code_has_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_has_highlighted-lines flexi-code_has_highlighted-phrases">
<header class="flexi-code__header">
<span class="flexi-code__title"></span>
<button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
<svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
</button>
</header>
<pre class="flexi-code__pre"><code class="flexi-code__code"><span class="token comment">/* 
    <span class="flexi-code__highlighted-phrase">Multiline</span></span>
<span class="flexi-code__line flexi-code__line_highlighted"><span class="token comment"><span class="flexi-code__highlighted-phrase">    comment</span></span></span>
<span class="token comment"><span class="flexi-code__highlighted-phrase">    1</span>
*/</span>
<span class="flexi-code__highlighted-phrase"><span class="token comment">/* 
    Multiline</span></span>
<span class="flexi-code__line flexi-code__line_highlighted"><span class="flexi-code__highlighted-phrase"><span class="token comment">    comment</span></span></span>
<span class="flexi-code__highlighted-phrase"><span class="token comment">    2
*/</span></span>
</code></pre>
</div>
````````````````````````````````

If a phrase element intersects a syntax element and one isn't contained by the other, the element that starts later gets split:

```````````````````````````````` none
--------------- Extra Extensions ---------------
OptionsBlocks
--------------- Markdown ---------------
@{
    "language": "csharp",
    "highlightedPhrases": [
        { "regex": "comment\\s+re" },
        { "regex": "\\+ \"d" }
    ]
}
```
public string ExampleFunction(string arg)
{
    // Example comment
    return arg + "dummyString";
}
```
--------------- Expected Markup ---------------
<div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_language-csharp flexi-code_has_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_has_highlighted-phrases">
<header class="flexi-code__header">
<span class="flexi-code__title"></span>
<button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
<svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
</button>
</header>
<pre class="flexi-code__pre"><code class="flexi-code__code"><span class="token keyword">public</span> <span class="token keyword">string</span> <span class="token function">ExampleFunction</span><span class="token punctuation">(</span><span class="token keyword">string</span> arg<span class="token punctuation">)</span>
<span class="token punctuation">{</span>
    <span class="token comment">// Example <span class="flexi-code__highlighted-phrase">comment</span></span><span class="flexi-code__highlighted-phrase">
    <span class="token keyword">re</span></span><span class="token keyword">turn</span> arg <span class="flexi-code__highlighted-phrase"><span class="token operator">+</span> <span class="token string">"d</span></span><span class="token string">ummyString"</span><span class="token punctuation">;</span>
<span class="token punctuation">}</span>
</code></pre>
</div>
````````````````````````````````

Intersecting and adjacent phrases are combined:

```````````````````````````````` none
--------------- Extra Extensions ---------------
OptionsBlocks
--------------- Markdown ---------------
@{
    "highlightedPhrases": [
        { "regex": "comment\\s+re" },
        { "regex": "(return )(arg)" },
        { "regex": "return" },
        { "regex": "rg \\+" }
    ]
}
```
public string ExampleFunction(string arg)
{
    // Example comment
    return arg + "dummyString";
}
```
--------------- Expected Markup ---------------
<div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_has_highlighted-phrases">
<header class="flexi-code__header">
<span class="flexi-code__title"></span>
<button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
<svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
</button>
</header>
<pre class="flexi-code__pre"><code class="flexi-code__code">public string ExampleFunction(string arg)
{
    // Example <span class="flexi-code__highlighted-phrase">comment
    return arg +</span> &quot;dummyString&quot;;
}
</code></pre>
</div>
````````````````````````````````

Contained elements never get split:
```````````````````````````````` none
--------------- Extra Extensions ---------------
OptionsBlocks
--------------- Markdown ---------------
@{
    "language": "csharp",
    "highlightedPhrases": [
        { "regex": "string ExampleFunction" },
        { "regex": "return" },
        { "regex": "(\"dum)myStr(ing\")" }
    ]
}
```
public string ExampleFunction(string arg)
{
    // Example comment
    return arg + "dummyString";
}
```
--------------- Expected Markup ---------------
<div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_language-csharp flexi-code_has_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_has_highlighted-phrases">
<header class="flexi-code__header">
<span class="flexi-code__title"></span>
<button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
<svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
</button>
</header>
<pre class="flexi-code__pre"><code class="flexi-code__code"><span class="token keyword">public</span> <span class="flexi-code__highlighted-phrase"><span class="token keyword">string</span> <span class="token function">ExampleFunction</span></span><span class="token punctuation">(</span><span class="token keyword">string</span> arg<span class="token punctuation">)</span>
<span class="token punctuation">{</span>
    <span class="token comment">// Example comment</span>
    <span class="flexi-code__highlighted-phrase"><span class="token keyword">return</span></span> arg <span class="token operator">+</span> <span class="flexi-code__highlighted-phrase"><span class="token string">"dum</span></span><span class="token string">myStr<span class="flexi-code__highlighted-phrase">ing"</span></span><span class="token punctuation">;</span>
<span class="token punctuation">}</span>
</code></pre>
</div>
````````````````````````````````

### HTML Encoding
If syntax highlighting is enabled, the chosen syntax highlighter performs the encoding. Prism encodes
'<' and '&' characters:

```````````````````````````````` none
--------------- Extra Extensions ---------------
OptionsBlocks
--------------- Markdown ---------------
@{
    "language": "html"
}
```
<div class="my-class">&</div>
```
--------------- Expected Markup ---------------
<div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_language-html flexi-code_has_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
<header class="flexi-code__header">
<span class="flexi-code__title"></span>
<button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
<svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
</button>
</header>
<pre class="flexi-code__pre"><code class="flexi-code__code"><span class="token tag"><span class="token tag"><span class="token punctuation">&lt;</span>div</span> <span class="token attr-name">class</span><span class="token attr-value"><span class="token punctuation">=</span><span class="token punctuation">"</span>my-class<span class="token punctuation">"</span></span><span class="token punctuation">></span></span>&amp;<span class="token tag"><span class="token tag"><span class="token punctuation">&lt;/</span>div</span><span class="token punctuation">></span></span>
</code></pre>
</div>
````````````````````````````````

HighlightJS encodes '<', '&' and '>' characters:
```````````````````````````````` none
--------------- Extra Extensions ---------------
OptionsBlocks
--------------- Markdown ---------------
@{
    "language": "html",
    "syntaxHighlighter": "highlightjs"
}
```
<div class="my-class">&</div>
```
--------------- Expected Markup ---------------
<div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_language-html flexi-code_has_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
<header class="flexi-code__header">
<span class="flexi-code__title"></span>
<button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
<svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
</button>
</header>
<pre class="flexi-code__pre"><code class="flexi-code__code"><span class="hljs-tag">&lt;<span class="hljs-name">div</span> <span class="hljs-attr">class</span>=<span class="hljs-string">"my-class"</span>&gt;</span>&amp;<span class="hljs-tag">&lt;/<span class="hljs-name">div</span>&gt;</span>
</code></pre>
</div>
````````````````````````````````

If syntax highlighting is disabled, '<', '&', '"", and '>' characters are encoded:
```````````````````````````````` none
--------------- Extra Extensions ---------------
OptionsBlocks
--------------- Markdown ---------------
```
<div class="my-class">&</div>
```
--------------- Expected Markup ---------------
<div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
<header class="flexi-code__header">
<span class="flexi-code__title"></span>
<button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
<svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
</button>
</header>
<pre class="flexi-code__pre"><code class="flexi-code__code">&lt;div class=&quot;my-class&quot;&gt;&amp;&lt;/div&gt;
</code></pre>
</div>
````````````````````````````````

Encoding does not affect highlighted phrases (regex expressions are evaluated before encoding):
```````````````````````````````` none
--------------- Extra Extensions ---------------
OptionsBlocks
--------------- Markdown ---------------
@{
    "highlightedPhrases": [{ "regex": "div" }]
}
```
<div class="my-class">&</div>
```
--------------- Expected Markup ---------------
<div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_has_highlighted-phrases">
<header class="flexi-code__header">
<span class="flexi-code__title"></span>
<button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
<svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
</button>
</header>
<pre class="flexi-code__pre"><code class="flexi-code__code">&lt;<span class="flexi-code__highlighted-phrase">div</span> class=&quot;my-class&quot;&gt;&amp;&lt;/<span class="flexi-code__highlighted-phrase">div</span>&gt;
</code></pre>
</div>
````````````````````````````````