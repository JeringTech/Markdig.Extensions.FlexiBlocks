## Sections
A typical article is divided into logical sections. Often, the HTML for logical sections are demarcated by heading elements.
The [HTML spec](https://html.spec.whatwg.org/multipage/sections.html#headings-and-sections) encourages wrapping of 
logical sections in [sectioning content elements](https://html.spec.whatwg.org/multipage/dom.html#sectioning-content-2).
This extension wraps logical sections in `<section>` elements, with nesting dependent on [ATX heading](https://spec.commonmark.org/0.28/#atx-headings)
levels.

Sequential higher-level sections are nested:

```````````````````````````````` example
# foo
## foo
### foo
#### foo
.
<h1>foo</h1>
<section id="foo">
<h2>foo</h2>
<section id="foo-1">
<h3>foo</h3>
<section id="foo-2">
<h4>foo</h4>
</section>
</section>
</section>
````````````````````````````````

Sequential lower-level sections are not nested.:

```````````````````````````````` example
## foo
# foo
.
<section id="foo">
<h2>foo</h2>
</section>
<h1>foo</h1>
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
### foo
## foo
.
<h1>foo</h1>
<section id="foo">
<h2>foo</h2>
<section id="foo-1">
<h3>foo</h3>
</section>
</section>
<section id="foo-2">
<h2>foo</h2>
</section>
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

To enable wrapping of level 1 headers, set `SectionExtensionOptions.DefaultSectionBlockOptions.Level1WrapperElement` to any `SectioningContentElement` value other than `None` and `Undefined`. For example:

```````````````````````````````` options
{
    "sections": {
        "defaultSectionBlockOptions": {
            "level1WrapperElement": "article"
        }
    }
}
```````````````````````````````` example
# foo
## foo
.
<article id="foo">
<h1>foo</h1>
<section id="foo-1">
<h2>foo</h2>
</section>
</article>
````````````````````````````````

To change the element used to wrapped level 2+ headers, set `SectionExtensionOptions.DefaultSectionBlockOptions.Level2PlusWrapperElement". For example:

```````````````````````````````` options
{
    "sections": {
        "defaultSectionBlockOptions": {
            "level2PlusWrapperElement": "nav"
        }
    }
}
```````````````````````````````` example
## foo
.
<nav id="foo">
<h2>foo</h2>
</nav>
````````````````````````````````

Kebab-case (lowercase words joined by dashes) IDs are generated for each section:

```````````````````````````````` example
## Foo Bar Baz
.
<section id="foo-bar-baz">
<h2>Foo Bar Baz</h2>
</section>
````````````````````````````````

Auto generation of IDs can be disabled by setting `SectionExtensionOptions.DefaultSectionBlockOptions.GenerateIdentifier` to `false`:

```````````````````````````````` options
{
    "sections": {
        "defaultSectionBlockOptions": {
            "generateIdentifier": false
        }
    }
}
```````````````````````````````` example
## Foo Bar Baz
.
<section>
<h2>Foo Bar Baz</h2>
</section>
````````````````````````````````

Sections can be linked to by the text content of their headings:

```````````````````````````````` example
[foo]

## foo
### foo bar
[foo bar]
#### foo bar baz

[foo bar baz]
.
<p><a href="#foo">foo</a></p>
<section id="foo">
<h2>foo</h2>
<section id="foo-bar">
<h3>foo bar</h3>
<p><a href="#foo-bar">foo bar</a></p>
<section id="foo-bar-baz">
<h4>foo bar baz</h4>
<p><a href="#foo-bar-baz">foo bar baz</a></p>
</section>
</section>
</section>
````````````````````````````````

Linking to sections by the text content of their headings can be disabled by setting `SectionExtensionOptions.DefaultSectionBlockOptions.AutoLinkable` to `false` (note 
that linking to sections is also disabled if `SectionExtensionOptions.DefaultSectionBlockOptions.GenerateIdentifier` is set to `false`):

```````````````````````````````` options
{
    "sections": {
        "defaultSectionBlockOptions": {
            "autoLinkable": false
        }
    }
}
```````````````````````````````` example
[foo]

## foo
### foo bar
[foo bar]
#### foo bar baz

[foo bar baz]
.
<p>[foo]</p>
<section id="foo">
<h2>foo</h2>
<section id="foo-bar">
<h3>foo bar</h3>
<p>[foo bar]</p>
<section id="foo-bar-baz">
<h4>foo bar baz</h4>
<p>[foo bar baz]</p>
</section>
</section>
</section>
````````````````````````````````