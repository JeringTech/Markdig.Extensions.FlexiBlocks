## FlexiIncludeBlocks
The FlexiIncludeBlocks extension provides ways to include content from both local and remote documents.

In the following specs, `exampleInclude.md` has the following contents:
```
This is example markdown.
```
`exampleIncludeWithNestedInclude.md` has the following contents:
```
This is example markdown with an include.

+{
    "contentType": "Markdown",
    "source": "./exampleInclude.md"    
}
```
And `exampleInclude.js` has the following contents:
```
function exampleFunction(arg) {
    // Example comment
    return arg + 'dummyString';
}

//#region utility methods
function add(a, b) {
    return a + b;
}
//#endregion utility methods
```

A FlexiIncludeBlock is an `IncludeOptions` instance in JSON form with `+` prepended immediately before the opening `{`. This first line
must begin with `+{`:
```````````````````````````````` example
This is an example article.
+{
    "contentType": "Markdown",
    "source": "./exampleInclude.md"
}
.
<p>This is an example article.</p>
<p>This is example markdown.</p>
````````````````````````````````

A FlexiIncludeBlock can retrieve remote content over HTTP or HTTPS:
```````````````````````````````` extraExtensions
FlexiCodeBlocks
```````````````````````````````` example
This is an example article.
+{
    "source": "https://raw.githubusercontent.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/42e2be4e8adb15b0fb28193fc615f520243420f0/src/FlexiBlocks/FlexiIncludeBlocks/ContentType.cs"
}
.
<p>This is an example article.</p>
<div class="fcb">
<header>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
</header>
<pre><code>namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    /// &lt;summary&gt;
    /// Include content types.
    /// &lt;/summary&gt;
    public enum ContentType
    {
        /// &lt;summary&gt;
        /// Code include content.
        /// &lt;/summary&gt;
        Code,

        /// &lt;summary&gt;
        /// Markdown include content.
        /// &lt;/summary&gt;
        Markdown
    }
}</code></pre>
</div>
````````````````````````````````

Includes can be nested:
```````````````````````````````` example
This is an example article.

+{
    "contentType": "Markdown",
    "source": "./exampleIncludeWithNestedInclude.md"
}
.
<p>This is an example article.</p>
<p>This is example markdown with an include.</p>
<p>This is example markdown.</p>
````````````````````````````````

Includes can be used within any kind of container block, such as a list item:
```````````````````````````````` example
- First item.
- Second item

  +{
      "contentType": "Markdown",
      "source": "./exampleInclude.md"
  }
- Third item
.
<ul>
<li><p>First item.</p></li>
<li><p>Second item</p>
<p>This is example markdown.</p></li>
<li><p>Third item</p></li>
</ul>
````````````````````````````````

Or a blockquote:
```````````````````````````````` example
> First line.
> +{
>     "contentType": "Markdown",
>     "source": "./exampleInclude.md"
> }
> Third line
.
<blockquote>
<p>First line.</p>
<p>This is example markdown.</p>
<p>Third line</p>
</blockquote>
````````````````````````````````

Content can be included as a code block, using the FlexiCodeBlocks extension:
```````````````````````````````` extraExtensions
FlexiCodeBlocks
```````````````````````````````` example
This is an example article.
+{
    "contentType": "Code",
    "source": "./exampleInclude.md"
}
.
<p>This is an example article.</p>
<div class="fcb">
<header>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
</header>
<pre><code>This is example markdown.</code></pre>
</div>
````````````````````````````````

Code is the default content type, `IncludeOptions.ContentType` can be omitted when including a code block:
```````````````````````````````` extraExtensions
FlexiCodeBlocks
```````````````````````````````` example
This is an example article.
+{
    "source": "./exampleInclude.md"
}
.
<p>This is an example article.</p>
<div class="fcb">
<header>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
</header>
<pre><code>This is example markdown.</code></pre>
</div>
````````````````````````````````

FlexiCodeOptions can be applied to FlexiIncludeBlocks of type `Code`:
```````````````````````````````` extraExtensions
FlexiOptionsBlocks
FlexiCodeBlocks
```````````````````````````````` example
This is an example article.
@{
    "language": "javascript"
}
+{
    "source": "./exampleInclude.js"
}
.
<p>This is an example article.</p>
<div class="fcb">
<header>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
</header>
<pre><code class="language-javascript"><span class="token keyword">function</span> <span class="token function">exampleFunction</span><span class="token punctuation">(</span>arg<span class="token punctuation">)</span> <span class="token punctuation">{</span>
    <span class="token comment">// Example comment</span>
    <span class="token keyword">return</span> arg <span class="token operator">+</span> <span class="token string">'dummyString'</span><span class="token punctuation">;</span>
<span class="token punctuation">}</span>

<span class="token comment">//#region utility methods</span>
<span class="token keyword">function</span> <span class="token function">add</span><span class="token punctuation">(</span>a<span class="token punctuation">,</span> b<span class="token punctuation">)</span> <span class="token punctuation">{</span>
    <span class="token keyword">return</span> a <span class="token operator">+</span> b<span class="token punctuation">;</span>
<span class="token punctuation">}</span>
<span class="token comment">//#endregion utility methods</span></code></pre>
</div>
````````````````````````````````

Included content can be clipped using line numbers:
```````````````````````````````` extraExtensions
FlexiCodeBlocks
```````````````````````````````` example
+{
    "source": "./exampleInclude.js",
    "clippings":[{"endLineNumber": 4}, {"startLineNumber": 7, "endLineNumber": 9}]
}
.
<div class="fcb">
<header>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
</header>
<pre><code>function exampleFunction(arg) {
    // Example comment
    return arg + 'dummyString';
}
function add(a, b) {
    return a + b;
}</code></pre>
</div>
````````````````````````````````

Included content can also be clipped using demarcation line substrings:
```````````````````````````````` extraExtensions
FlexiCodeBlocks
```````````````````````````````` example
+{
    "source": "./exampleInclude.js",
    "clippings":[{"startDemarcationLineSubstring": "#region utility methods", "endDemarcationLineSubstring": "#endregion utility methods"}]
}
.
<div class="fcb">
<header>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
</header>
<pre><code>function add(a, b) {
    return a + b;
}</code></pre>
</div>
````````````````````````````````

Included content can also be clipped using a combination of line numbers and line substrings:
```````````````````````````````` extraExtensions
FlexiCodeBlocks
```````````````````````````````` example
+{
    "source": "./exampleInclude.js",
    "clippings":[{"startLineNumber": 7, 
        "endDemarcationLineSubstring": "#endregion utility methods"}]
}
.
<div class="fcb">
<header>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
</header>
<pre><code>function add(a, b) {
    return a + b;
}</code></pre>
</div>
````````````````````````````````

Text can be prepended and appended to each clipping:
```````````````````````````````` extraExtensions
FlexiCodeBlocks
```````````````````````````````` example
+{
    "source": "./exampleInclude.js",
    "clippings":[{
        "endLineNumber": 1,
        "afterText": "..."
    },
    {
        "startLineNumber": 4,
        "endLineNumber": 4
    },
    {
        "startLineNumber": 7, 
        "endLineNumber": 7,
        "beforeText": ""
    },
    {
        "startLineNumber": 9, 
        "endLineNumber": 9,
        "beforeText": "..."
    }]
}
.
<div class="fcb">
<header>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
</header>
<pre><code>function exampleFunction(arg) {
...
}

function add(a, b) {
...
}</code></pre>
</div>
````````````````````````````````

A clipping can be dedented:
```````````````````````````````` extraExtensions
FlexiCodeBlocks
```````````````````````````````` example
+{
    "source": "./exampleInclude.js",
    "clippings":[{"endLineNumber": 4, "dedentLength": 2}],
}
.
<div class="fcb">
<header>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
</header>
<pre><code>function exampleFunction(arg) {
  // Example comment
  return arg + 'dummyString';
}</code></pre>
</div>
````````````````````````````````

Leading white space in a clipping can also be collapsed:
```````````````````````````````` extraExtensions
FlexiCodeBlocks
```````````````````````````````` example
+{
    "source": "./exampleInclude.js",
    "clippings":[{"endLineNumber": 4, "collapseRatio": 0.5}]
}
.
<div class="fcb">
<header>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
</header>
<pre><code>function exampleFunction(arg) {
  // Example comment
  return arg + 'dummyString';
}</code></pre>
</div>
````````````````````````````````


