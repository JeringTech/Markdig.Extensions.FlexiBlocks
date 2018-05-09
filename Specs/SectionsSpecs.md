## Sections
A typical article is divided into logical sections. Often, the HTML for logical sections are demarcated by heading elements.
The [HTML spec](https://html.spec.whatwg.org/multipage/sections.html#headings-and-sections) encourages wrapping of 
logical sections in [sectioning content elements](https://html.spec.whatwg.org/multipage/dom.html#sectioning-content-2).
This extension wraps logical sections in `<section>` elements, with nesting dependent on [ATX heading](https://spec.commonmark.org/0.28/#atx-headings)
levels.

Sequential higher-level sections are nested:

```````````````````````````````` example
# foo
## foo bar
### foo bar baz
.
<article id="foo">
<h1>foo</h1>
<section id="foo-bar">
<h2>foo bar</h2>
<section id="foo-bar-baz">
<h3>foo bar baz</h3>
</section>
</section>
</article>
````````````````````````````````

Sequential lower-level sections are not nested.:

```````````````````````````````` example
## foo
# foo
.
<section id="foo">
<h2>foo</h2>
</section>
<article id="foo-1">
<h1>foo</h1>
</article>
````````````````````````````````

Sequential same-level sections are not nested:

```````````````````````````````` example
## foo
## foo
.
<section id="foo">
<h2>foo</h2>
</section>
<section id="foo-1">
<h2>foo</h2>
</section>
````````````````````````````````

Mixed sections:

```````````````````````````````` example
# foo
## foo
# foo

.
<article id="foo">
<h1>foo</h1>
<section id="foo-1">
<h2>foo</h2>
</section>
</article>
<article id="foo-2">
<h1>foo</h1>
</article>
````````````````````````````````

Sections wrap content:

```````````````````````````````` example
# foo
Level 1 content.
## foo
- Level 2 content line 1.
- Level 2 content line 2.
### foo
> Level 3 content line 1.
> Level 3 content line 2.
.
<article id="foo">
<h1>foo</h1>
<p>Level 1 content.</p>
<section id="foo-1">
<h2>foo</h2>
<ul>
<li>Level 2 content line 1.</li>
<li>Level 2 content line 2.</li>
</ul>
<section id="foo-2">
<h3>foo</h3>
<blockquote>
<p>Level 3 content line 1.
Level 3 content line 2.</p>
</blockquote>
</section>
</section>
</article>
````````````````````````````````

TODO: id style, github etc
Kebab-case (lowercase words joined by dashes) IDs are generated for each section.

```````````````````````````````` example
# Foo
## Foo Bar
.
<article id="foo">
<h1>Foo</h1>
<section id="foo-bar">
<h2>Foo Bar</h2>
</section>
</article>
````````````````````````````````

AutoLinks allow linking to sections using the text in their headings:

```````````````````````````````` example
[foo]

# foo
## foo bar
[foo bar]
### foo bar baz

[foo bar baz]
.
<p><a href="#foo">foo</a></p>
<article id="foo">
<h1>foo</h1>
<section id="foo-bar">
<h2>foo bar</h2>
<p><a href="#foo-bar">foo bar</a></p>
<section id="foo-bar-baz">
<h3>foo bar baz</h3>
<p><a href="#foo-bar-baz">foo bar baz</a></p>
</section>
</section>
</article>
````````````````````````````````
