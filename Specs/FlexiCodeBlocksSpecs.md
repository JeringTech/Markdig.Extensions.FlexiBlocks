## FlexiCodeBlocks
FlexiCodeBlocks have the following configurable features:

- Title
- Copy code icon
- Syntax highlighting
- Line numbers
- Line highlighting

These features can be configured at the extension level using `FlexiCodeBlocksExtensionOptions` and can also be configured at the 
block level using FlexiBlockOptions.

FlexiCodeBlocks have the same syntax as CommonMark fenced and indented code blocks.
The following is an example of a fenced FlexiCodeBlock with the default options:

```````````````````````````````` example
```
public string ExampleFunction(string arg)
{
    // Example comment
    return arg + "dummyString";
}
```
.
<div class="fcb">
<header>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
</header>
<pre><code>public string ExampleFunction(string arg)
{
    // Example comment
    return arg + &quot;dummyString&quot;;
}</code></pre>
</div>
````````````````````````````````

The following is an example of an indented FlexiCodeBlock with the default options:

```````````````````````````````` example
    public string ExampleFunction(string arg)
    {
        // Example comment
        return arg + "dummyString";
    }
.
<div class="fcb">
<header>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
</header>
<pre><code>public string ExampleFunction(string arg)
{
    // Example comment
    return arg + &quot;dummyString&quot;;
}</code></pre>
</div>
````````````````````````````````

`FlexiCodeBlockOptions.Title` can be used to define a title for a FlexiCodeBlock:

```````````````````````````````` extraExtensions
FlexiOptionsBlocks
```````````````````````````````` example
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
.
<div class="fcb">
<header>
<span>ExampleDocument.cs</span>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
</header>
<pre><code>public string ExampleFunction(string arg)
{
    // Example comment
    return arg + &quot;dummyString&quot;;
}</code></pre>
</div>
````````````````````````````````

`FlexiCodeBlockOptions.CopyIconMarkup` can be used to customize the copy icon for a FlexiCodeBlock:

```````````````````````````````` extraExtensions
FlexiOptionsBlocks
```````````````````````````````` example
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
.
<div class="fcb">
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

To enable syntax highlighting, assign a valid
[Prism language alias](https://prismjs.com/index.html#languages-list) to `FlexiCodeBlockOptions.Langauge`:

```````````````````````````````` extraExtensions
FlexiOptionsBlocks
```````````````````````````````` example
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
.
<div class="fcb">
<header>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
</header>
<pre><code class="language-csharp"><span class="token keyword">public</span> <span class="token keyword">string</span> <span class="token function">ExampleFunction</span><span class="token punctuation">(</span><span class="token keyword">string</span> arg<span class="token punctuation">)</span>
<span class="token punctuation">{</span>
    <span class="token comment">// Example comment</span>
    <span class="token keyword">return</span> arg <span class="token operator">+</span> <span class="token string">"dummyString"</span><span class="token punctuation">;</span>
<span class="token punctuation">}</span></code></pre>
</div>
````````````````````````````````

If you prefer the syntax highlighter [HighlightJS](http://highlightjs.readthedocs.io/en/latest/index.html), set the 
value of `FlexiCodeBlockOptions.SyntaxHighlighter` to `HighlightJS` and assign a valid [HighlightJS language alias](http://highlightjs.readthedocs.io/en/latest/css-classes-reference.html#language-names-and-aliases)
to `FlexiCodeBlockOptions.Language`:

```````````````````````````````` extraExtensions
FlexiOptionsBlocks
```````````````````````````````` example
@{
    "language": "csharp",
    "syntaxHighlighter": "highlightJS"
}
```
public string ExampleFunction(string arg)
{
    // Example comment
    return arg + "dummyString";
}
```
.
<div class="fcb">
<header>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
</header>
<pre><code class="language-csharp"><span class="hljs-function"><span class="hljs-keyword">public</span> <span class="hljs-keyword">string</span> <span class="hljs-title">ExampleFunction</span>(<span class="hljs-params"><span class="hljs-keyword">string</span> arg</span>)
</span>{
    <span class="hljs-comment">// Example comment</span>
    <span class="hljs-keyword">return</span> arg + <span class="hljs-string">"dummyString"</span>;
}</code></pre>
</div>
````````````````````````````````

Assign a prefix to `FlexiCodeBlockOptions.HighlightJSClassPrefix` to customize the prefix for HighlightJS classes:

```````````````````````````````` extraExtensions
FlexiOptionsBlocks
```````````````````````````````` example
@{
    "language": "csharp",
    "syntaxHighlighter": "highlightJS",
    "highlightJSClassPrefix": "my-prefix-"
}
```
public string ExampleFunction(string arg)
{
    // Example comment
    return arg + "dummyString";
}
```
.
<div class="fcb">
<header>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
</header>
<pre><code class="language-csharp"><span class="my-prefix-function"><span class="my-prefix-keyword">public</span> <span class="my-prefix-keyword">string</span> <span class="my-prefix-title">ExampleFunction</span>(<span class="my-prefix-params"><span class="my-prefix-keyword">string</span> arg</span>)
</span>{
    <span class="my-prefix-comment">// Example comment</span>
    <span class="my-prefix-keyword">return</span> arg + <span class="my-prefix-string">"dummyString"</span>;
}</code></pre>
</div>
````````````````````````````````

If you prefer to do highlighting client-side, set `highlightSyntax` to `false`. As long as `FlexiCodeBlockOptions.Langauge` is not
null, whitespace or an empty string, a language class will be assigned to the `code` element:

```````````````````````````````` extraExtensions
FlexiOptionsBlocks
```````````````````````````````` example
@{
    "language": "html",
    "highlightSyntax": false
}
```
<div>"<" and "&" are escaped</div>
```
.
<div class="fcb">
<header>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
</header>
<pre><code class="language-html">&lt;div&gt;&quot;&lt;&quot; and &quot;&amp;&quot; are escaped&lt;/div&gt;</code></pre>
</div>
````````````````````````````````

Add line numbers by setting `FlexiCodeBlockOptions.RenderLineNumbers` to true:

```````````````````````````````` extraExtensions
FlexiOptionsBlocks
```````````````````````````````` example
@{
    "renderLineNumbers": true
}
```
public string ExampleFunction(string arg)
{
    // Example comment
    return arg + "dummyString";
}
```
.
<div class="fcb">
<header>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
</header>
<pre><code><span class="line"><span class="line-number">1</span><span class="line-text">public string ExampleFunction(string arg)</span></span>
<span class="line"><span class="line-number">2</span><span class="line-text">{</span></span>
<span class="line"><span class="line-number">3</span><span class="line-text">    // Example comment</span></span>
<span class="line"><span class="line-number">4</span><span class="line-text">    return arg + &quot;dummyString&quot;;</span></span>
<span class="line"><span class="line-number">5</span><span class="line-text">}</span></span></code></pre>
</div>
````````````````````````````````

Customize which numbers line number sequences start from and the lines that line numbers are rendered for using
`FlexiCodeBlockOptions.LineNumberRanges`:

```````````````````````````````` extraExtensions
FlexiOptionsBlocks
```````````````````````````````` example
@{
    "renderLineNumbers": true,
    "lineNumberRanges": [
        {
            "startLine": 1,
            "endLine": 8,
            "startLineNumber": 1
        },
        {
            "startLine": 11,
            "endLine": -1,
            "startLineNumber": 32
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
.
<div class="fcb">
<header>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
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

Highlight lines using `FlexiCodeBlockOptions.HighlightLineRanges` (line highlighting should not be confused with syntax highlighting - a highlighted line is simply
a line with perhaps a different background color, syntax highlighting adds color to syntax tokens):

```````````````````````````````` extraExtensions
FlexiOptionsBlocks
```````````````````````````````` example
@{
    "highlightLineRanges": [
        {
            "startLine": 1,
            "endLine": 1
        },
        {
            "startLine": 3,
            "endLine": 4
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
.
<div class="fcb">
<header>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
</header>
<pre><code><span class="line highlight"><span class="line-number">1</span><span class="line-text">public string ExampleFunction(string arg)</span></span>
<span class="line"><span class="line-number">2</span><span class="line-text">{</span></span>
<span class="line highlight"><span class="line-number">3</span><span class="line-text">    // Example comment</span></span>
<span class="line highlight"><span class="line-number">4</span><span class="line-text">    return arg + &quot;dummyString&quot;;</span></span>
<span class="line"><span class="line-number">5</span><span class="line-text">}</span></span></code></pre>
</div>
````````````````````````````````

Certain characters within code elements must be escaped. If syntax highlighting isn't enabled, the characters
`<`, `>` and `&` are escaped:
```````````````````````````````` example
```
<div>"<" and "&" are escaped</div>
```
.
<div class="fcb">
<header>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
</header>
<pre><code>&lt;div&gt;&quot;&lt;&quot; and &quot;&amp;&quot; are escaped&lt;/div&gt;</code></pre>
</div>
````````````````````````````````

Both Prism and HighlightJS cannot process escaped characters, so it isn't possible to escape code then pass it to the highlighters. Fortunately,
both of them can do escaping on their own. Prism, escapes `<` and `&` characters:

```````````````````````````````` extraExtensions
FlexiOptionsBlocks
```````````````````````````````` example
@{
    "language": "html"
}
```
<div>"<" and "&" are escaped</div>
```
.
<div class="fcb">
<header>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
</header>
<pre><code class="language-html"><span class="token tag"><span class="token tag"><span class="token punctuation">&lt;</span>div</span><span class="token punctuation">></span></span>"&lt;" and "&amp;" are escaped<span class="token tag"><span class="token tag"><span class="token punctuation">&lt;/</span>div</span><span class="token punctuation">></span></span></code></pre>
</div>
````````````````````````````````

HighlightJS, escapes `<`, `>` and `&` characters:

```````````````````````````````` extraExtensions
FlexiOptionsBlocks
```````````````````````````````` example
@{
    "language": "html",
    "syntaxHighlighter": "highlightJS"
}
```
<div>"<" and "&" are escaped</div>
```
.
<div class="fcb">
<header>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
</header>
<pre><code class="language-html"><span class="hljs-tag">&lt;<span class="hljs-name">div</span>&gt;</span>"<span class="hljs-tag">&lt;<span class="hljs-name">"</span> <span class="hljs-attr">and</span> "&amp;" <span class="hljs-attr">are</span> <span class="hljs-attr">escaped</span>&lt;/<span class="hljs-attr">div</span>&gt;</span></code></pre>
</div>
````````````````````````````````