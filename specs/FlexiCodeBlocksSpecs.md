# FlexiCodeBlocks
FlexiCodeBlocks contain code. They enhance code with aesthetic and functional features like syntax highlighting,
line highlighting, line numbering and more.

## Prerequisites
To use syntax highlighting, [NodeJS](https://nodejs.org/en/) must be installed and node.exe's directory must be added to the `Path` environment variable.

## Basic Syntax
A FlexiCodeBlock is a sequence of [fenced](https://spec.commonmark.org/0.28/#fenced-code-blocks) or [indented](https://spec.commonmark.org/0.28/#indented-code-blocks) lines. 
Basic-syntax-wise, FlexiCodeBlocks are identical to [CommonMark](https://spec.commonmark.org/0.28/) code blocks. The following is a fenced FlexiCodeBlock:

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
<div class="flexi-code-block">
<header>
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
By default, a FlexiCodeBlock is rendered with a "copy code" icon. The icon markup and more can be customized or omitted - refer to the [options section](#options) for details.

The following is an indented FlexiCodeBlock:
```````````````````````````````` none
--------------- Markdown ---------------
    public string ExampleFunction(string arg)
    {
        // Example comment
        return arg + "dummyString";
    }
--------------- Expected Markup ---------------
<div class="flexi-code-block">
<header>
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

## Options

### `LineRange`
Represents a range of lines. Used by [FlexiCodeBlockOptions](#flexicodeblockoptions).

#### Properties
- `StartLineNumber`
  - Type: `int`
  - Description: The line number of this LineRange's start line.
    This value must be greater than 0.
  - Default: `1`
- `EndLineNumber`
  - Type: `int`
  - Description: The line number of this LineRange's end line..
    If this value is -1, this range extends to the last line. If it is not -1, it must be greater than or equal to `StartLineNumber`.
  - Default: `-1`

### `NumberedLineRange`
Represents a range of lines with an associated sequence of numbers. Used by [FlexiCodeBlockOptions](#flexicodeblockoptions).

#### Properties
- `StartLineNumber`
  - Type: `int`
  - Description: The line number of this NumberedLineRange's start line.
    This value must be greater than 0.
  - Default: `1`
- `EndLineNumber`
  - Type: `int`
  - Description: The line number of this NumberedLineRange's end line.
    If this value is -1, this range extends to the last line. If it is not -1, it must be greater than or equal to `StartLineNumber`.
  - Default: `-1`
- `FirstNumber`
  - Type: `int`
  - Description: The number associated with this NumberedLineRange's start line.
    The number associated with each subsequent line is incremented by 1.
    This value must be greater than 0.
  - Default: `1`

### `FlexiCodeBlockOptions`
Options for a FlexiCodeBlock. To specify FlexiCodeBlockOptions for a FlexiCodeBlock, the 
[FlexiOptionsBlocks](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiOptionsBlocksSpecs.md#flexioptionsblocks) extension must be enabled. To specify default FlexiCodeBlockOptions for all FlexiCodeBlocks,
use [FlexiCodeBlocksExtensionOptions](#flexicodeblocksextensionoptions).

#### Properties
- `Class`
  - Type: `string`
  - Description: The FlexiCodeBlock's outermost element's class. If this value is null, whitespace or an empty string, no class is assigned.
  - Default: "flexi-code-block"
  - Usage:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiOptionsBlocks
    --------------- Markdown ---------------
    @{
        "class": "alternative-class"
    }
    ```
    public string ExampleFunction(string arg)
    {
        // Example comment
        return arg + "dummyString";
    }
    ```
    --------------- Expected Markup ---------------
    <div class="alternative-class">
    <header>
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

- `CopyIconMarkup`
  - Type: `string`
  - Description: The markup for the FlexiCodeBlock's copy icon.
    If this value is null, whitespace or an empty string, no copy icon is rendered.
  - Default: [Material Design "File Copy" Icon](https://material.io/tools/icons/?icon=file_copy&style=baseline)
  - Usage:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiOptionsBlocks
    --------------- Markdown ---------------
    @{
        "copyIconMarkup": "<svg><use xlink:href=\"#material-design-copy\"></use></svg>"
    }
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
    <button>
    <svg><use xlink:href="#material-design-copy"></use></svg>
    </button>
    </header>
    <pre><code><span class="line"><span class="line-text">public string ExampleFunction(string arg)</span></span>
    <span class="line"><span class="line-text">{</span></span>
    <span class="line"><span class="line-text">    // Example comment</span></span>
    <span class="line"><span class="line-text">    return arg + &quot;dummyString&quot;;</span></span>
    <span class="line"><span class="line-text">}</span></span></code></pre>
    </div>
    ````````````````````````````````

- `Title`
  - Type: `string`
  - Description: The FlexiCodeBlock's title.
    If this value is null, whitespace or an empty string, no title is rendered.
  - Default: `null`
  - Usage:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiOptionsBlocks
    --------------- Markdown ---------------
    @{
        "title": "ExampleDocument.cs"
    }
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

- `Language`
  - Type: `string`
  - Description: The language for syntax highlighting of the FlexiCodeBlock's code.
    The value must be a valid language alias for the chosen syntax highlighter (defaults to Prism).
    - Valid langauge aliases for Prism can be found here: https://prismjs.com/index.html#languages-list.
    - Valid language aliases for HighlightJS can be found here: http://highlightjs.readthedocs.io/en/latest/css-classes-reference.html#language-names-and-aliases.</para>
    
    If this value is null, whitespace or an empty string, syntax highlighting is disabled and no class is assigned to the FlexiCodeBlock's code element.
  - Default: `null`
  - Usage:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiOptionsBlocks
    --------------- Markdown ---------------
    @{
        "language": "csharp"
    }
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
    <button>
    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
    </button>
    </header>
    <pre><code class="language-csharp"><span class="line"><span class="line-text"><span class="token keyword">public</span> <span class="token keyword">string</span> <span class="token function">ExampleFunction</span><span class="token punctuation">(</span><span class="token keyword">string</span> arg<span class="token punctuation">)</span></span></span>
    <span class="line"><span class="line-text"><span class="token punctuation">{</span></span></span>
    <span class="line"><span class="line-text">    <span class="token comment">// Example comment</span></span></span>
    <span class="line"><span class="line-text">    <span class="token keyword">return</span> arg <span class="token operator">+</span> <span class="token string">"dummyString"</span><span class="token punctuation">;</span></span></span>
    <span class="line"><span class="line-text"><span class="token punctuation">}</span></span></span></code></pre>
    </div>
    ````````````````````````````````
    By default, if a language is specified for a FlexiCodeBlock, a language class is assigned to the code element and syntax highlighting is performed.

- `CodeClassFormat`
  - Type: `string`
  - Description: The format for the FlexiCodeBlock's code element's class.
    The FlexiCodeBlock's language will replace "{0}" in the format.
    If this value or the FlexiCodeBlock's language are null, whitespace or an empty string, no class is assigned to the code element.
  - Default: "language-{0}"
  - Usage:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiOptionsBlocks
    --------------- Markdown ---------------
    @{
        "codeClassFormat": "lang-{0}",
        "language": "csharp"
    }
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
    <button>
    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
    </button>
    </header>
    <pre><code class="lang-csharp"><span class="line"><span class="line-text"><span class="token keyword">public</span> <span class="token keyword">string</span> <span class="token function">ExampleFunction</span><span class="token punctuation">(</span><span class="token keyword">string</span> arg<span class="token punctuation">)</span></span></span>
    <span class="line"><span class="line-text"><span class="token punctuation">{</span></span></span>
    <span class="line"><span class="line-text">    <span class="token comment">// Example comment</span></span></span>
    <span class="line"><span class="line-text">    <span class="token keyword">return</span> arg <span class="token operator">+</span> <span class="token string">"dummyString"</span><span class="token punctuation">;</span></span></span>
    <span class="line"><span class="line-text"><span class="token punctuation">}</span></span></span></code></pre>
    </div>
    ````````````````````````````````

- `SyntaxHighlighter`
  - Type: `SyntaxHighlighter`
  - Description: The syntax highlighter to use for syntax highlighting.
    If this value is `SyntaxHighlighter.None`, syntax highlighting will be disabled.
  - Default: `SyntaxHighlighter.Prism`
  - Usage:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiOptionsBlocks
    --------------- Markdown ---------------
    @{
        "syntaxHighlighter": "highlightJS",
        "language": "csharp",
    }
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
    <button>
    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
    </button>
    </header>
    <pre><code class="language-csharp"><span class="line"><span class="line-text"><span class="hljs-function"><span class="hljs-keyword">public</span> <span class="hljs-keyword">string</span> <span class="hljs-title">ExampleFunction</span>(<span class="hljs-params"><span class="hljs-keyword">string</span> arg</span>)</span></span></span>
    <span class="line"><span class="line-text">{</span></span>
    <span class="line"><span class="line-text">    <span class="hljs-comment">// Example comment</span></span></span>
    <span class="line"><span class="line-text">    <span class="hljs-keyword">return</span> arg + <span class="hljs-string">"dummyString"</span>;</span></span>
    <span class="line"><span class="line-text">}</span></span></code></pre>
    </div>
    ````````````````````````````````
  
- `HighlightJSClassPrefix`
  - Type: `string`
  - Description: The prefix for HighlightJS classes.
    This option is only relevant if HighlightJS is the selected syntax highlighter.
    If this value is null, whitespace or an empty string, no prefix is prepended to HighlightJS classes.
  - Default: "hljs-"
  - Usage:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiOptionsBlocks
    --------------- Markdown ---------------
    @{
        "language": "csharp",
        "syntaxHighlighter": "highlightJS",
        "highlightJSClassPrefix": "highlightjs-"
    }
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
    <button>
    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
    </button>
    </header>
    <pre><code class="language-csharp"><span class="line"><span class="line-text"><span class="highlightjs-function"><span class="highlightjs-keyword">public</span> <span class="highlightjs-keyword">string</span> <span class="highlightjs-title">ExampleFunction</span>(<span class="highlightjs-params"><span class="highlightjs-keyword">string</span> arg</span>)</span></span></span>
    <span class="line"><span class="line-text">{</span></span>
    <span class="line"><span class="line-text">    <span class="highlightjs-comment">// Example comment</span></span></span>
    <span class="line"><span class="line-text">    <span class="highlightjs-keyword">return</span> arg + <span class="highlightjs-string">"dummyString"</span>;</span></span>
    <span class="line"><span class="line-text">}</span></span></code></pre>
    </div>
    ````````````````````````````````

- `LineNumberLineRanges`
  - Type: `IList<NumberedLineRange>`
  - Description: The `NumberedLineRange`s that specify the line number to render for each line of code.
    If this value is null, no line numbers will be rendered.
  - Default: `null`
  - Usage:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiOptionsBlocks
    --------------- Markdown ---------------
    @{
        "lineNumberLineRanges": [
            {
                "startLineNumber": 1,
                "endLineNumber": 8,
                "firstNumber": 1
            },
            {
                "startLineNumber": 11,
                "endLineNumber": -1,
                "firstNumber": 32
            }
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

        // Some functions omitted for brevity
        ...

        public string ExampleFunction3(string arg)
        {
            // Example comment
            return arg + "dummyString";
        }
    }
    ```
    --------------- Expected Markup ---------------
    <div class="flexi-code-block">
    <header>
    <button>
    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
    </button>
    </header>
    <pre><code><span class="line"><span class="line-number">1</span><span class="line-text">public class ExampleClass</span></span>
    <span class="line"><span class="line-number">2</span><span class="line-text">{</span></span>
    <span class="line"><span class="line-number">3</span><span class="line-text">    public string ExampleFunction1(string arg)</span></span>
    <span class="line"><span class="line-number">4</span><span class="line-text">    {</span></span>
    <span class="line"><span class="line-number">5</span><span class="line-text">        // Example comment</span></span>
    <span class="line"><span class="line-number">6</span><span class="line-text">        return arg + &quot;dummyString&quot;;</span></span>
    <span class="line"><span class="line-number">7</span><span class="line-text">    }</span></span>
    <span class="line"><span class="line-number">8</span><span class="line-text"><br></span></span>
    <span class="line"><span class="line-number"><svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 8c1.1 0 2-.9 2-2s-.9-2-2-2-2 .9-2 2 .9 2 2 2zm0 2c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2zm0 6c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2z"/></svg></span><span class="line-text">    // Some functions omitted for brevity</span></span>
    <span class="line"><span class="line-number"><svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 8c1.1 0 2-.9 2-2s-.9-2-2-2-2 .9-2 2 .9 2 2 2zm0 2c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2zm0 6c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2z"/></svg></span><span class="line-text">    ...</span></span>
    <span class="line"><span class="line-number">32</span><span class="line-text"><br></span></span>
    <span class="line"><span class="line-number">33</span><span class="line-text">    public string ExampleFunction3(string arg)</span></span>
    <span class="line"><span class="line-number">34</span><span class="line-text">    {</span></span>
    <span class="line"><span class="line-number">35</span><span class="line-text">        // Example comment</span></span>
    <span class="line"><span class="line-number">36</span><span class="line-text">        return arg + &quot;dummyString&quot;;</span></span>
    <span class="line"><span class="line-number">37</span><span class="line-text">    }</span></span>
    <span class="line"><span class="line-number">38</span><span class="line-text">}</span></span></code></pre>
    </div>
    ````````````````````````````````
    The markdown in the above spec can be simplified by ommitting [`NumberedLineRange`](#numberedlinerange) properties that were set to their default values. For example, the first `NumberedLineRange` can be
    specified as:
    ```
    {
        "endLineNumber": 8
    }
    ```

- `HighlightLineRanges`
  - Type: `IList<LineRange>`
  - Description: The `LineRange`s that specify which lines of code to highlight.
    If this value is null, no lines will be highlighted.
    Line highlighting should not be confused with syntax highlighting. While syntax highlighting highlights tokens in code, line highlighting highlights entire lines.
  - Default: `null`
  - Usage:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiOptionsBlocks
    --------------- Markdown ---------------
    @{
        "highlightLineRanges": [
            {
                "startLineNumber": 1,
                "endLineNumber": 1
            },
            {
                "startLineNumber": 3,
                "endLineNumber": 4
            }
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
    <div class="flexi-code-block">
    <header>
    <button>
    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
    </button>
    </header>
    <pre><code><span class="line highlight"><span class="line-text">public string ExampleFunction(string arg)</span></span>
    <span class="line"><span class="line-text">{</span></span>
    <span class="line highlight"><span class="line-text">    // Example comment</span></span>
    <span class="line highlight"><span class="line-text">    return arg + &quot;dummyString&quot;;</span></span>
    <span class="line"><span class="line-text">}</span></span></code></pre>
    </div>
    ````````````````````````````````
    The markdown in the above spec can be simplified by omitting [`LineRange`](#linerange) properties that were set to their default values. For example, the first `LineRange` can be
    specified as:
    ```
    {
        "endLineNumber": 1
    }
    ```

- `LineEmbellishmentClassesPrefix`
  - Type: `string`
  - Description: The prefix for line embellishment classes (line embellishments are markup elements added to facilitate per-line styling).
    If this value is null, whitespace or an empty string, no prefix is added to line embellishment classes.
  - Default: `null`
  - Usage:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiOptionsBlocks
    --------------- Markdown ---------------
    @{
        "lineEmbellishmentClassesPrefix": "le-",
        "highlightLineRanges": [{}],
        "lineNumberLineRanges": [{}]
    }
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
    <button>
    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
    </button>
    </header>
    <pre><code><span class="le-line le-highlight"><span class="le-line-number">1</span><span class="le-line-text">public string ExampleFunction(string arg)</span></span>
    <span class="le-line le-highlight"><span class="le-line-number">2</span><span class="le-line-text">{</span></span>
    <span class="le-line le-highlight"><span class="le-line-number">3</span><span class="le-line-text">    // Example comment</span></span>
    <span class="le-line le-highlight"><span class="le-line-number">4</span><span class="le-line-text">    return arg + &quot;dummyString&quot;;</span></span>
    <span class="le-line le-highlight"><span class="le-line-number">5</span><span class="le-line-text">}</span></span></code></pre>
    </div>
    ````````````````````````````````

- `HiddenLinesIconMarkup`
  - Type: `string`
  - Description: The markup for the icon that represents hidden lines.
    If this value is null, whitespace or an empty string, no hidden lines icons are rendered.
  - Default: [Material Design "More Vert" Icon](https://material.io/tools/icons/?search=vert&icon=more_vert&style=baseline)
  - Usage:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiOptionsBlocks
    --------------- Markdown ---------------
    @{
        "hiddenLinesIconMarkup": "<svg><use xlink:href=\"#material-design-more-vert\"></use></svg>",
        "lineNumberLineRanges": [{"startLineNumber": 1, "endLineNumber": 2, "firstNumber": 1}, {"startLineNumber": 4, "firstNumber":10}]
    }
    ```
    public string ExampleFunction(string arg)
    {
    // Omitted for brevity
        // Example comment
        return arg + "dummyString";
    }
    ```
    --------------- Expected Markup ---------------
    <div class="flexi-code-block">
    <header>
    <button>
    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
    </button>
    </header>
    <pre><code><span class="line"><span class="line-number">1</span><span class="line-text">public string ExampleFunction(string arg)</span></span>
    <span class="line"><span class="line-number">2</span><span class="line-text">{</span></span>
    <span class="line"><span class="line-number"><svg><use xlink:href="#material-design-more-vert"></use></svg></span><span class="line-text">// Omitted for brevity</span></span>
    <span class="line"><span class="line-number">10</span><span class="line-text">    // Example comment</span></span>
    <span class="line"><span class="line-number">11</span><span class="line-text">    return arg + &quot;dummyString&quot;;</span></span>
    <span class="line"><span class="line-number">12</span><span class="line-text">}</span></span></code></pre>
    </div>
    ````````````````````````````````

- `Attributes`
  - Type: `IDictionary<string, string>`
  - Description: The HTML attributes for the outermost element of the FlexiCodeBlock.
    If this value is null, no attributes will be assigned to the outermost element.
  - Default: `null`
  - Usage:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiOptionsBlocks
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
    <div id="code-1" class="block flexi-code-block">
    <header>
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
    If a value is specified for the class attribute, it will not override the outermost element's generated class. Instead, it will be 
    prepended to the generated class. In the above example, this results in the outermost element's class attribute having the value 
    `block flexi-code-block`.


### `FlexiCodeBlocksExtensionOptions`
Global options for FlexiCodeBlocks. These options can be used to define defaults for all FlexiCodeBlocks. They have
lower precedence than block specific options specified using the FlexiOptionsBlocks extension.  

FlexiCodeBlocksExtensionOptions can be specified when enabling the FlexiCodeBlocks extension:
``` 
MyMarkdownPipelineBuilder.UseFlexiCodeBlocks(myFlexiCodeBlocksExtensionOptions);
```

#### Properties
- `DefaultBlockOptions`
  - Type: `FlexiCodeBlockOptions`
  - Description: Default `FlexiCodeBlockOptions` for all FlexiCodeBlocks. 
  - Usage:
    ```````````````````````````````` none
    --------------- Extension Options ---------------
    {
        "flexiCodeBlocks": {
            "defaultBlockOptions": {
                "class": "alternative-class",
                "copyIconMarkup": "<svg><use xlink:href=\"#material-design-copy\"></use></svg>",
                "title": "ExampleDocument.cs",
                "language": "csharp",
                "codeClassFormat": "lang-{0}",
                "syntaxHighlighter": "highlightJS",
                "highlightJSClassPrefix": "highlightjs-",
                "lineNumberLineRanges": [{}],
                "highlightLineRanges": [{}]
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
    --------------- Expected Markup ---------------
    <div class="alternative-class">
    <header>
    <span>ExampleDocument.cs</span>
    <button>
    <svg><use xlink:href="#material-design-copy"></use></svg>
    </button>
    </header>
    <pre><code class="lang-csharp"><span class="line highlight"><span class="line-number">1</span><span class="line-text"><span class="highlightjs-function"><span class="highlightjs-keyword">public</span> <span class="highlightjs-keyword">string</span> <span class="highlightjs-title">ExampleFunction</span>(<span class="highlightjs-params"><span class="highlightjs-keyword">string</span> arg</span>)</span></span></span>
    <span class="line highlight"><span class="line-number">2</span><span class="line-text">{</span></span>
    <span class="line highlight"><span class="line-number">3</span><span class="line-text">    <span class="highlightjs-comment">// Example comment</span></span></span>
    <span class="line highlight"><span class="line-number">4</span><span class="line-text">    <span class="highlightjs-keyword">return</span> arg + <span class="highlightjs-string">"dummyString"</span>;</span></span>
    <span class="line highlight"><span class="line-number">5</span><span class="line-text">}</span></span></code></pre>
    </div>
    ````````````````````````````````

    Default FlexiCodeBlockOptions have lower precedence than block specific options:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiOptionsBlocks
    --------------- Extension Options ---------------
    {
        "flexiCodeBlocks": {
            "defaultBlockOptions": {
                "lineNumberLineRanges": [{}]
            }
        }
    }
    --------------- Markdown ---------------
    ```
    public string ExampleFunction1(string arg)
    {
        // Example comment
        return arg + "dummyString";
    }
    ```

    @{
        "lineNumberLineRanges": [
            {
                "firstNumber": 6
            }
        ]
    }
    ```
    public string ExampleFunction2(string arg)
    {
        // Example comment
        return arg + "dummyString";
    }
    ```
    --------------- Expected Markup ---------------
    <div class="flexi-code-block">
    <header>
    <button>
    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
    </button>
    </header>
    <pre><code><span class="line"><span class="line-number">1</span><span class="line-text">public string ExampleFunction1(string arg)</span></span>
    <span class="line"><span class="line-number">2</span><span class="line-text">{</span></span>
    <span class="line"><span class="line-number">3</span><span class="line-text">    // Example comment</span></span>
    <span class="line"><span class="line-number">4</span><span class="line-text">    return arg + &quot;dummyString&quot;;</span></span>
    <span class="line"><span class="line-number">5</span><span class="line-text">}</span></span></code></pre>
    </div>
    <div class="flexi-code-block">
    <header>
    <button>
    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
    </button>
    </header>
    <pre><code><span class="line"><span class="line-number">6</span><span class="line-text">public string ExampleFunction2(string arg)</span></span>
    <span class="line"><span class="line-number">7</span><span class="line-text">{</span></span>
    <span class="line"><span class="line-number">8</span><span class="line-text">    // Example comment</span></span>
    <span class="line"><span class="line-number">9</span><span class="line-text">    return arg + &quot;dummyString&quot;;</span></span>
    <span class="line"><span class="line-number">10</span><span class="line-text">}</span></span></code></pre>
    </div>
    ````````````````````````````````