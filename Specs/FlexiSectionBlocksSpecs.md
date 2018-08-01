## FlexiSectionBlocks
A typical article is divided into logical sections. Often, the HTML for logical sections are demarcated by heading elements.
The [HTML spec](https://html.spec.whatwg.org/multipage/sections.html#headings-and-sections) encourages wrapping of 
logical sections in [sectioning content elements](https://html.spec.whatwg.org/multipage/dom.html#sectioning-content-2).
This extension wraps logical sections in `<section>` elements, with nesting dependent on [ATX heading](https://spec.commonmark.org/0.28/#atx-headings)
levels.

Additionally, this extension wraps heading elements (`<h1>, <h2> etc`) in `<header>` elements and renders icon markup. Wrapping heading elements
in header elements is standard usage of the `<header>` element, as per MDN's [documentation](https://developer.mozilla.org/en-US/docs/Web/HTML/Element/header).

Sequential higher-level sections are nested:

```````````````````````````````` example
# foo
## foo
### foo
#### foo
.
<header class="header-level-1">
<h1>foo</h1>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
<section id="foo">
<header class="header-level-2">
<h2>foo</h2>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
<section id="foo-1">
<header class="header-level-3">
<h3>foo</h3>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
<section id="foo-2">
<header class="header-level-4">
<h4>foo</h4>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
</section>
</section>
</section>
````````````````````````````````

Sequential lower-level sections are not nested:

```````````````````````````````` example
## foo
# foo
.
<section id="foo">
<header class="header-level-2">
<h2>foo</h2>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
</section>
<header class="header-level-1">
<h1>foo</h1>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
````````````````````````````````

Sequential same-level sections are not nested:

```````````````````````````````` example
## foo
## foo
.
<section id="foo">
<header class="header-level-2">
<h2>foo</h2>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
</section>
<section id="foo-1">
<header class="header-level-2">
<h2>foo</h2>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
</section>
````````````````````````````````

Mixed sections:

```````````````````````````````` example
# foo
## foo
### foo
## foo
.
<header class="header-level-1">
<h1>foo</h1>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
<section id="foo">
<header class="header-level-2">
<h2>foo</h2>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
<section id="foo-1">
<header class="header-level-3">
<h3>foo</h3>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
</section>
</section>
<section id="foo-2">
<header class="header-level-2">
<h2>foo</h2>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
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
<header class="header-level-1">
<h1>foo</h1>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
<p>Level 1 content.</p>
<section id="foo">
<header class="header-level-2">
<h2>foo</h2>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
<ul>
<li>Level 2 content line 1.</li>
<li>Level 2 content line 2.</li>
</ul>
<section id="foo-1">
<header class="header-level-3">
<h3>foo</h3>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
<blockquote>
<p>Level 3 content line 1.
Level 3 content line 2.</p>
</blockquote>
</section>
</section>
````````````````````````````````

To enable wrapping of level 1 headers, set `SectionsExtensionOptions.Level1WrapperElement` to any `SectioningContentElement` value other than `None` and `Undefined`. For example:

```````````````````````````````` extensionOptions
{
    "flexisectionblocks": {
        "level1WrapperElement": "article"
    }
}
```````````````````````````````` example
# foo
## foo
.
<article id="foo">
<header class="header-level-1">
<h1>foo</h1>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
<section id="foo-1">
<header class="header-level-2">
<h2>foo</h2>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
</section>
</article>
````````````````````````````````

To change the element used to wrap level 2+ headers, set `SectionsExtensionOptions.Level2PlusWrapperElement". For example:

```````````````````````````````` extensionOptions
{
    "flexisectionblocks": {
        "level2PlusWrapperElement": "nav"
    }
}
```````````````````````````````` example
## foo
.
<nav id="foo">
<header class="header-level-2">
<h2>foo</h2>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
</nav>
````````````````````````````````

The icon used for headers can be customized by setting `FlexiSectionBlocksExtensionOptions.DefaultFlexiSectionBlockOptions.HeaderIconMarkup`. For example:

```````````````````````````````` extensionOptions
{
    "flexisectionblocks": {
        "defaultFlexiSectionBlockOptions": {
            "headerIconMarkup": "<svg><use xlink:href=\"#custom-link-icon\"></use></svg>"
        }
    }
}
```````````````````````````````` example
## foo
.
<section id="foo">
<header class="header-level-2">
<h2>foo</h2>
<svg><use xlink:href="#custom-link-icon"></use></svg>
</header>
</section>
````````````````````````````````

The format string for header classes can be customized by setting `FlexiSectionBlocksExtensionOptions.DefaultFlexiSectionBlockOptions.HeaderClassNameFormat`. For example:

```````````````````````````````` extensionOptions
{
    "flexisectionblocks": {
        "defaultFlexiSectionBlockOptions": {
            "headerClassNameFormat": "custom-{0}"
        }
    }
}
```````````````````````````````` example
## foo
.
<section id="foo">
<header class="custom-2">
<h2>foo</h2>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
</section>
````````````````````````````````

Kebab-case (lowercase words joined by dashes) IDs are generated for each section:

```````````````````````````````` example
## Foo Bar Baz
.
<section id="foo-bar-baz">
<header class="header-level-2">
<h2>Foo Bar Baz</h2>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
</section>
````````````````````````````````

Auto generation of IDs can be disabled by setting `FlexiSectionBlocksExtensionOptions.DefaultFlexiSectionBlockOptions.GenerateIdentifier` to `false`:

```````````````````````````````` extensionOptions
{
    "flexisectionblocks": {
        "defaultFlexiSectionBlockOptions": {
            "generateIdentifier": false
        }
    }
}
```````````````````````````````` example
## Foo Bar Baz
.
<section>
<header class="header-level-2">
<h2>Foo Bar Baz</h2>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
</section>
````````````````````````````````

Sections can be linked to by the text content of their headings:

```````````````````````````````` example
[foo]

## foo
### foo bar
[foo bar]
#### foo bar baz

[Link Text][foo bar baz]
.
<p><a href="#foo">foo</a></p>
<section id="foo">
<header class="header-level-2">
<h2>foo</h2>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
<section id="foo-bar">
<header class="header-level-3">
<h3>foo bar</h3>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
<p><a href="#foo-bar">foo bar</a></p>
<section id="foo-bar-baz">
<header class="header-level-4">
<h4>foo bar baz</h4>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
<p><a href="#foo-bar-baz">Link Text</a></p>
</section>
</section>
</section>
````````````````````````````````

Linking to sections by the text content of their headings can be disabled by setting `FlexiSectionBlocksExtensionOptions.DefaultFlexiSectionBlockOptions.AutoLinkable` to `false` (note 
that linking to sections is also disabled if `FlexiSectionBlocksExtensionOptions.DefaultFlexiSectionBlockOptions.GenerateIdentifier` is set to `false`):

```````````````````````````````` extensionOptions
{
    "flexisectionblocks": {
        "defaultFlexiSectionBlockOptions": {
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
<header class="header-level-2">
<h2>foo</h2>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
<section id="foo-bar">
<header class="header-level-3">
<h3>foo bar</h3>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
<p>[foo bar]</p>
<section id="foo-bar-baz">
<header class="header-level-4">
<h4>foo bar baz</h4>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
<p>[foo bar baz]</p>
</section>
</section>
</section>
````````````````````````````````

Per-FlexiSectionBlock options can be specified if the FlexiOptionsBlocks extension is enabled:

```````````````````````````````` extraExtensions
FlexiOptionsBlocks
```````````````````````````````` example
@{
    "wrapperElement": "article"
}
# foo
@{
    "headerIconMarkup": "<svg><use xlink:href=\"#custom-link-icon\"></use></svg>"
}
## foo
@{
    "headerClassNameFormat": "custom-{0}"
}
## foo
.
<article id="foo">
<header class="header-level-1">
<h1>foo</h1>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
<section id="foo-1">
<header class="header-level-2">
<h2>foo</h2>
<svg><use xlink:href="#custom-link-icon"></use></svg>
</header>
</section>
<section id="foo-2">
<header class="custom-2">
<h2>foo</h2>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
</section>
</article>
````````````````````````````````