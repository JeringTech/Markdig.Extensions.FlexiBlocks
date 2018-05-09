## Sections
A typical article is divided into logical sections. Often, the HTML for logical sections are demarcated by heading elements.
The [HTML spec](https://html.spec.whatwg.org/multipage/sections.html#headings-and-sections) encourages wrapping of 
logical sections in [sectioning content elements](https://html.spec.whatwg.org/multipage/dom.html#sectioning-content-2).
This extension wraps logical sections in `<section>` elements, with nesting dependent on [ATX heading](https://spec.commonmark.org/0.28/#atx-headings)
levels.

Sequential higher level sections are nested. Kebab-case IDs are generated for each section.
Note that the `<h1>` element is not wrapped, it should be inserted as a child of an `<article>` element.:

```````````````````````````````` example
# Foo
## Foo Bar
### Foo Bar Baz
.
<h1>Foo</h1>
<section id="foo-bar">
<h2>Foo Bar</h2>
<section id="foo-bar-baz">
<h3>Foo Bar Baz</h3>
</section>
</section>
````````````````````````````````

Sequential lower level sections are not nested. IDs with appended integers are generated to avoid duplicate IDs.:

```````````````````````````````` example
### foo
## foo
# foo
.
<section id="foo">
<h3>foo</h3>
</section>
<section id="foo-1">
<h2>foo</h2>
</section>
<h1>foo</h1>
````````````````````````````````

Mixed levels:

```````````````````````````````` example
## foo
### foo
# foo
.
<section id="foo">
<h2>foo</h2>
<section id="foo-1">
<h3>foo</h3>
</section>
</section>
<h1>foo</h1>
````````````````````````````````

Same levels:

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

Sections with content (child containers):

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
<h1>foo</h1>
<p>Level 1 content.</p>
<section id="foo">
<h2>foo</h2>
<ul>
<li>Level 2 content line 1.</li>
<li>Level 2 content line 2.</li>
</ul>
<section id="foo-1">
<h3>foo</h3>
<blockquote>
<p>Level 3 content line 1.
Level 3 content line 2.</p>
</blockquote>
</section>
</section>
````````````````````````````````