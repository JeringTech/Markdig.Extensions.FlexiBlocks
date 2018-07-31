## FlexiIncludeBlocks
The FlexiIncludeBlocks extension provides ways to include content from both local and remote documents.

In the following
example, `exampleArticleSection.md` has the following contents:
```
#### Example Article Section
This is an example article section.
```
`exampleArticleSectionWithNestedInclude.md` has the following contents:
```
### Example Article Section with Nested Include
This is an example article section with a nested include.

+{
    "contentType": "Markdown",
    "source": "./exampleArticleSection.md"    
}
```

A FlexiIncludeBlock is an `IncludeOptions` instance in JSON form with `+` prepended immediately before the opening `{`. This first line
must begin with `+{`:
```````````````````````````````` example
# Example Article
This is an example article.

+{
    "contentType": "Markdown",
    "source": "./exampleArticleSection.md"
}
.
<header class="header-level-1">
<h1>Example Article</h1>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
<p>This is an example article.</p>
<section id="example-article-section">
<header class="header-level-4">
<h4>Example Article Section</h4>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
<p>This is an example article section.</p>
</section>
````````````````````````````````

Includes can be nested:
```````````````````````````````` example
# Example Article
This is an example article.

+{
    "contentType": "Markdown",
    "source": "./exampleArticleSectionWithNestedInclude.md"
}
.
<header class="header-level-1">
<h1>Example Article</h1>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
<p>This is an example article.</p>
<section id="example-article-section-with-nested-include">
<header class="header-level-3">
<h3>Example Article Section with Nested Include</h3>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
<p>This is an example article section with a nested include.</p>
<section id="example-article-section">
<header class="header-level-4">
<h4>Example Article Section</h4>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
<p>This is an example article section.</p>
</section>
</section>
````````````````````````````````

Includes can be used within any kind of container block, such as a list item:
```````````````````````````````` example
- First item.
- Second item
  +{
      "contentType": "Markdown",
      "source": "./exampleArticleSection.md"
  }
- Third item
.
<ul>
<li>First item.</li>
<li>Second item
<section id="example-article-section">
<header class="header-level-4">
<h4>Example Article Section</h4>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
<p>This is an example article section.</p>
</section></li>
<li>Third item</li>
</ul>
````````````````````````````````

Or a blockquote:


TODO Content can be included from remote sources:

TODO Content can be clipped using line ranges and regions:

TODO Content can be included as a code block:

TODO FlexiCodeOptions can be applied to included code blocks:

