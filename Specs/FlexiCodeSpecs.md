## Flexi Code
Flexi code has the following features:

- Copy code icon
- Code block title
- Syntax highlighting
- Line numbers

These features can be configured at the extension level using `FlexiCodeExtensionOptions` and can also be configured at the 
block level using JSON options.

Flexi code blocks have the same syntax as CommonMark fenced and indented code blocks.
The following is an example of a fenced flexi code block with the default options:

```````````````````````````````` example
```
public string ExampleFunction(string arg)
{
    // Example comment
    return arg + "dummyString";
}
```
.
<div class="flexi-code">
<header>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
</header>
<pre><code>public string ExampleFunction(string arg)
{
    // Example comment
    return arg + &quot;dummyString&quot;;
}
</code></pre>
</div>
````````````````````````````````

The following is an example of an indented flexi code block with the default options:

```````````````````````````````` example
    public string ExampleFunction(string arg)
    {
        // Example comment
        return arg + "dummyString";
    }
.
<div class="flexi-code">
<header>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
</header>
<pre><code>public string ExampleFunction(string arg)
{
    // Example comment
    return arg + &quot;dummyString&quot;;
}
</code></pre>
</div>
````````````````````````````````

The following is an example of a fenced flexi code block with code syntax highlighting:
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
<div class="flexi-code">
<header>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
</header>
<pre><code><span class="token keyword">public</span> <span class="token keyword">string</span> <span class="token function">ExampleFunction</span><span class="token punctuation">(</span><span class="token keyword">string</span> arg<span class="token punctuation">)</span>
<span class="token punctuation">{</span>
    <span class="token comment">// Example comment</span>
    <span class="token keyword">return</span> arg <span class="token operator">+</span> <span class="token string">"dummyString"</span><span class="token punctuation">;</span>
<span class="token punctuation">}</span>
</code></pre>
</div>
````````````````````````````````

Under the hood, the syntax highlighter, Prism, escapes `<` and `&` characters:
```````````````````````````````` example
@{
    "language": "html"
}
```
<div>"<" and "&" are escaped</div>
```
.
<div class="flexi-code">
<header>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
</header>
<pre><code><span class="token tag"><span class="token tag"><span class="token punctuation">&lt;</span>div</span><span class="token punctuation">></span></span>"&lt;" and "&amp;" are escaped<span class="token tag"><span class="token tag"><span class="token punctuation">&lt;/</span>div</span><span class="token punctuation">></span></span>
</code></pre>
</div>
````````````````````````````````

If you prefer to do highlighting client-side, set `highlight` to `false`. A language class will be assigned to the `code` element:
```````````````````````````````` example
@{
    "language": "html",
    "highlight": false
}
```
<div>"<" and "&" are escaped</div>
```
.
<div class="flexi-code">
<header>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
</header>
<pre><code class="language-html">&lt;div&gt;&quot;&lt;&quot; and &quot;&amp;&quot; are escaped&lt;/div&gt;
</code></pre>
</div>
````````````````````````````````