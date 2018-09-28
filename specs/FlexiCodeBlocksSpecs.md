# FlexiCodeBlocks
FlexiCodeBlocks contain code. They enhance code with aesthetic and functional features like syntax highlighting,
line highlighting, line numbering and more.

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
<div>
<header>
<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
</header>
<pre><code>public string ExampleFunction(string arg)
{
    // Example comment
    return arg + &quot;dummyString&quot;;
}</code></pre>
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
<div>
<header>
<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
</header>
<pre><code>public string ExampleFunction(string arg)
{
    // Example comment
    return arg + &quot;dummyString&quot;;
}</code></pre>
</div>
````````````````````````````````

## Options

### `LineRange`
Represents a range of lines. Used by [FlexiCodeBlockOptions](#flexicodeblockoptions).

#### Properties
- `StartLineNumber`
  - Type: `int`
  - Description: Start line number of this range.
    This value must be greater than 0.
  - Default: `1`
- `EndLineNumber`
  - Type: `int`
  - Description: End line number of this range.
    If this value is -1, this range extends to the last line. If it is not -1, it must be greater than or equal to `StartLineNumber`.
  - Default: `-1`

### `LineNumberRange`
Represents a range of line numbers for a range of lines. Used by [FlexiCodeBlockOptions](#flexicodeblockoptions).

#### Properties
- `StartLineNumber`
  - Type: `int`
  - Description: Start line number of the range of lines that this `LineNumberRange` applies to.
    This value must be greater than 0.
  - Default: `1`
- `EndLineNumber`
  - Type: `int`
  - Description: End line number of the range of lines that this `LineNumberRange` applies to.
    If this value is -1, this range extends to the last line. If it is not -1, it must be greater than or equal to `StartLineNumber`.
  - Default: `-1`
- `FirstLineNumber`
  - Type: `int`
  - Description: Line number of the first line in the range of lines that this `LineNumberRange` applies to..
    This value must be greater than 0.
  - Default: `1`

### `FlexiCodeBlockOptions`
Options for a FlexiCodeBlock. To specify FlexiCodeBlockOptions for a FlexiCodeBlock, the 
[FlexiOptionsBlock](https://github.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiOptionsBlocksSpecs.md#flexioptionsblocks) extension must be enabled. To specify default FlexiCodeBlockOptions for all FlexiCodeBlocks,
use [FlexiCodeBlocksExtensionOptions](#flexicodeblocksextensionoptions).

#### Properties
- `CopyIconMarkup`
  - Type: `string`
  - Description: The markup for the FlexiCodeBlock's copy icon.
    If this value is null, whitespace or an empty string, no copy icon is rendered.
  - Default: [Material Design "File Copy" Icon](https://material.io/tools/icons/?icon=file_copy&style=sharp)
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
    <div>
    <header>
    <svg><use xlink:href="#material-design-copy"></use></svg>
    </header>
    <pre><code>public string ExampleFunction(string arg)
    {
        // Example comment
        return arg + &quot;dummyString&quot;;
    }</code></pre>
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
    <div>
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
    <div>
    <header>
    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
    </header>
    <pre><code class="language-csharp"><span class="token keyword">public</span> <span class="token keyword">string</span> <span class="token function">ExampleFunction</span><span class="token punctuation">(</span><span class="token keyword">string</span> arg<span class="token punctuation">)</span>
    <span class="token punctuation">{</span>
        <span class="token comment">// Example comment</span>
        <span class="token keyword">return</span> arg <span class="token operator">+</span> <span class="token string">"dummyString"</span><span class="token punctuation">;</span>
    <span class="token punctuation">}</span></code></pre>
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
    <div>
    <header>
    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
    </header>
    <pre><code class="lang-csharp"><span class="token keyword">public</span> <span class="token keyword">string</span> <span class="token function">ExampleFunction</span><span class="token punctuation">(</span><span class="token keyword">string</span> arg<span class="token punctuation">)</span>
    <span class="token punctuation">{</span>
        <span class="token comment">// Example comment</span>
        <span class="token keyword">return</span> arg <span class="token operator">+</span> <span class="token string">"dummyString"</span><span class="token punctuation">;</span>
    <span class="token punctuation">}</span></code></pre>
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
    <div>
    <header>
    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
    </header>
    <pre><code class="language-csharp"><span class="hljs-function"><span class="hljs-keyword">public</span> <span class="hljs-keyword">string</span> <span class="hljs-title">ExampleFunction</span>(<span class="hljs-params"><span class="hljs-keyword">string</span> arg</span>)
    </span>{
        <span class="hljs-comment">// Example comment</span>
        <span class="hljs-keyword">return</span> arg + <span class="hljs-string">"dummyString"</span>;
    }</code></pre>
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
    <div>
    <header>
    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
    </header>
    <pre><code class="language-csharp"><span class="highlightjs-function"><span class="highlightjs-keyword">public</span> <span class="highlightjs-keyword">string</span> <span class="highlightjs-title">ExampleFunction</span>(<span class="highlightjs-params"><span class="highlightjs-keyword">string</span> arg</span>)
    </span>{
        <span class="highlightjs-comment">// Example comment</span>
        <span class="highlightjs-keyword">return</span> arg + <span class="highlightjs-string">"dummyString"</span>;
    }</code></pre>
    </div>
    ````````````````````````````````

- `LineNumberRanges`
  - Type: `IList<LineNumberRange>`
  - Description: The `LineNumberRange`s that specify the line number for each line of code.
    If this value is null, no line numbers will be rendered.
  - Default: `null`
  - Usage:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiOptionsBlocks
    --------------- Markdown ---------------
    @{
        "lineNumberRanges": [
            {
                "startLineNumber": 1,
                "endLineNumber": 8,
                "firstLineNumber": 1
            },
            {
                "startLineNumber": 11,
                "endLineNumber": -1,
                "firstLineNumber": 32
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
    <div>
    <header>
    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
    </header>
    <pre><code><span class="line"><span class="line-number">1</span><span class="line-text">public class ExampleClass</span></span>
    <span class="line"><span class="line-number">2</span><span class="line-text">{</span></span>
    <span class="line"><span class="line-number">3</span><span class="line-text">    public string ExampleFunction1(string arg)</span></span>
    <span class="line"><span class="line-number">4</span><span class="line-text">    {</span></span>
    <span class="line"><span class="line-number">5</span><span class="line-text">        // Example comment</span></span>
    <span class="line"><span class="line-number">6</span><span class="line-text">        return arg + &quot;dummyString&quot;;</span></span>
    <span class="line"><span class="line-number">7</span><span class="line-text">    }</span></span>
    <span class="line"><span class="line-number">8</span><span class="line-text"></span></span>
    <span class="line"><span class="line-text">    // Some functions omitted for brevity</span></span>
    <span class="line"><span class="line-text">    ...</span></span>
    <span class="line"><span class="line-number">32</span><span class="line-text"></span></span>
    <span class="line"><span class="line-number">33</span><span class="line-text">    public string ExampleFunction3(string arg)</span></span>
    <span class="line"><span class="line-number">34</span><span class="line-text">    {</span></span>
    <span class="line"><span class="line-number">35</span><span class="line-text">        // Example comment</span></span>
    <span class="line"><span class="line-number">36</span><span class="line-text">        return arg + &quot;dummyString&quot;;</span></span>
    <span class="line"><span class="line-number">37</span><span class="line-text">    }</span></span>
    <span class="line"><span class="line-number">38</span><span class="line-text">}</span></span></code></pre>
    </div>
    ````````````````````````````````
    The markdown in the above spec can be simplified by ommitting [`LineNumberRange`](#linenumberrange) properties that were set to their default values. For example, the first `LineNumberRange` can be
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
    <div>
    <header>
    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
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
  - Description: The prefix for line number and line highlighting classes (line embellishment classes).
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
        "lineNumberRanges": [{}]
    }
    ```
    public string ExampleFunction(string arg)
    {
        // Example comment
        return arg + "dummyString";
    }
    ```
    --------------- Expected Markup ---------------
    <div>
    <header>
    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
    </header>
    <pre><code><span class="le-line le-highlight"><span class="le-line-number">1</span><span class="le-line-text">public string ExampleFunction(string arg)</span></span>
    <span class="le-line le-highlight"><span class="le-line-number">2</span><span class="le-line-text">{</span></span>
    <span class="le-line le-highlight"><span class="le-line-number">3</span><span class="le-line-text">    // Example comment</span></span>
    <span class="le-line le-highlight"><span class="le-line-number">4</span><span class="le-line-text">    return arg + &quot;dummyString&quot;;</span></span>
    <span class="le-line le-highlight"><span class="le-line-number">5</span><span class="le-line-text">}</span></span></code></pre>
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
            "class" : "fcb"
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
    <div id="code-1" class="fcb">
    <header>
    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
    </header>
    <pre><code>public string ExampleFunction(string arg)
    {
        // Example comment
        return arg + &quot;dummyString&quot;;
    }</code></pre>
    </div>
    ````````````````````````````````

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
                "copyIconMarkup": "<svg><use xlink:href=\"#material-design-copy\"></use></svg>",
                "title": "ExampleDocument.cs",
                "language": "csharp",
                "codeClassFormat": "lang-{0}",
                "syntaxHighlighter": "highlightJS",
                "highlightJSClassPrefix": "highlightjs-",
                "lineNumberRanges": [{}],
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
    <div>
    <header>
    <span>ExampleDocument.cs</span>
    <svg><use xlink:href="#material-design-copy"></use></svg>
    </header>
    <pre><code class="lang-csharp"><span class="line highlight"><span class="line-number">1</span><span class="line-text"><span class="highlightjs-function"><span class="highlightjs-keyword">public</span> <span class="highlightjs-keyword">string</span> <span class="highlightjs-title">ExampleFunction</span>(<span class="highlightjs-params"><span class="highlightjs-keyword">string</span> arg</span>)</span></span>
    <span class="line highlight"><span class="line-number">2</span><span class="line-text"></span>{</span></span>
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
                "lineNumberRanges": [{}]
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
        "lineNumberRanges": [
            {
                "firstLineNumber": 6
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
    <div>
    <header>
    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
    </header>
    <pre><code><span class="line"><span class="line-number">1</span><span class="line-text">public string ExampleFunction1(string arg)</span></span>
    <span class="line"><span class="line-number">2</span><span class="line-text">{</span></span>
    <span class="line"><span class="line-number">3</span><span class="line-text">    // Example comment</span></span>
    <span class="line"><span class="line-number">4</span><span class="line-text">    return arg + &quot;dummyString&quot;;</span></span>
    <span class="line"><span class="line-number">5</span><span class="line-text">}</span></span></code></pre>
    </div>
    <div>
    <header>
    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
    </header>
    <pre><code><span class="line"><span class="line-number">6</span><span class="line-text">public string ExampleFunction2(string arg)</span></span>
    <span class="line"><span class="line-number">7</span><span class="line-text">{</span></span>
    <span class="line"><span class="line-number">8</span><span class="line-text">    // Example comment</span></span>
    <span class="line"><span class="line-number">9</span><span class="line-text">    return arg + &quot;dummyString&quot;;</span></span>
    <span class="line"><span class="line-number">10</span><span class="line-text">}</span></span></code></pre>
    </div>
    ````````````````````````````````