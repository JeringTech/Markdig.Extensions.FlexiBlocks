## FlexiIncludeBlocks
The FlexiIncludeBlocks extension provides ways to include content from both local and remote documents.

In the following
example, `exampleArticleSection.md` has the following contents:
```
## Example Article Section
This is an example article section.
```
`exampleArticleSectionWithNestedInclude.md` has the following contents:
```
## Example Article Section with Nested Include
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
<h1>Example Article</h1>
<p>This is an example article.</p>
<h2>Example Article Section</h2>
<p>This is an example article section.</p>
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
<h1>Example Article</h1>
<p>This is an example article.</p>
<h2>Example Article Section with Nested Include</h2>
<p>This is an example article section with a nested include.</p>
<h2>Example Article Section</h2>
<p>This is an example article section.</p>
````````````````````````````````

TODO Includes exist within kind of container block:
- list
- blockquote

TODO Content can be included as a code block:

TODO FlexiCodeOptions can be applied to included code blocks:

TODO Content can be included from remote sources:

TODO Content can be clipped using line ranges and regions: