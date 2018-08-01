## FlexiIncludeBlocks
The FlexiIncludeBlocks extension provides ways to include content from both local and remote documents.

In the following
example, `exampleInclude.md` has the following contents:
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

Content can be included as a code block. Using the FlexiCodeBlocks extension:
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

TODO FlexiCodeOptions can be applied to included FlexiCodeBlocks:


TODO Content can be included from remote sources:

TODO Content can be clipped using line ranges and regions:


