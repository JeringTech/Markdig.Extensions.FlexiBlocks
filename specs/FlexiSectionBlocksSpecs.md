# FlexiSectionBlocks
Markdown articles are typically divided into logical sections by [ATX heading](https://spec.commonmark.org/0.28/#atx-headings)s. For example:
```
# Indoor Herb Gardens
An introduction..

## Getting Started

### Growing Herbs from Cuttings
Information on growing herbs from cuttings..

## Caring for Herbs

### Watering Herbs
Information on watering herbs..
```
The [HTML spec](https://html.spec.whatwg.org/multipage/sections.html#headings-and-sections) encourages wrapping of 
logical sections in [sectioning content elements](https://html.spec.whatwg.org/multipage/dom.html#sectioning-content-2), like so:

```
<article>
<h1>Indoor Herb Gardens</h1>
<p>An introduction..</p>
<section>
<h2>Getting Started</h2>
<section>
<h3>Growing Herbs from Cuttings</h3>
<p>Information on growing herbs from cuttings..</p>
</section>
</section>
<section>
<h2>Caring for Herbs</h2>
<section>
<h3>Watering</h3>
<p>Information on watering herbs..</p>
</section>
</section>
</article>
```
Wrapping logical sections in sectioning content elements facilitates efficient manipulation of an article's outline (TODO add link to relevant article).

FlexiSectionBlocks are logical sections of a markdown article. They facilitate wrapping of such sections in sectioning content elements.

## Basic Syntax
A FlexiSectionBlock consists of an ATX heading and the content between it and
- The next ATX heading of equal or lower level 
[that is not a child of a container block](#mechanics) or
- The end of the article.

Valid [ATX heading](https://spec.commonmark.org/0.28/#atx-headings)s demarcate the start of FlexiSectionBlocks: 
```````````````````````````````` none
--------------- Markdown ---------------
# Indoor Herb Gardens
An introduction..

## Getting Started

### Growing Herbs from Cuttings
Information on growing herbs from cuttings..

## Caring for Herbs

### Watering Herbs
Information on watering herbs..
--------------- Expected Markup ---------------
<section class="section-level-1" id="indoor-herb-gardens">
<header>
<h1>Indoor Herb Gardens</h1>
<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M17 7h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8zm9-4h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8z"/></svg>
</header>
<p>An introduction..</p>
<section class="section-level-2" id="getting-started">
<header>
<h2>Getting Started</h2>
<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M17 7h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8zm9-4h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8z"/></svg>
</header>
<section class="section-level-3" id="growing-herbs-from-cuttings">
<header>
<h3>Growing Herbs from Cuttings</h3>
<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M17 7h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8zm9-4h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8z"/></svg>
</header>
<p>Information on growing herbs from cuttings..</p>
</section>
</section>
<section class="section-level-2" id="caring-for-herbs">
<header>
<h2>Caring for Herbs</h2>
<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M17 7h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8zm9-4h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8z"/></svg>
</header>
<section class="section-level-3" id="watering-herbs">
<header>
<h3>Watering Herbs</h3>
<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M17 7h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8zm9-4h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8z"/></svg>
</header>
<p>Information on watering herbs..</p>
</section>
</section>
</section>
````````````````````````````````

By default, a FlexiSectionBlock's outermost element is assigned a generated class and ID. Also, its heading is nested in a header element with a link
icon. The class, ID, markup for the link icon and more can be customized or omitted - refer to the [options section](#options) for details.
 
## Options

### `FlexiSectionBlockOptions`
Options for a FlexiSectionBlock. To specify FlexiSectionBlockOptions for a FlexiSectionBlock, the 
[FlexiOptionsBlocks](https://github.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiOptionsBlocksSpecs.md#flexioptionsblocks) extension must be enabled. To specify default FlexiSectionBlockOptions for all FlexiSectionBlocks,
use [FlexiSectionBlocksExtensionOptions](#flexisectionblocksextensionoptions).

#### Properties
- `Element`
  - Type: `SectioningContentElement`
  - Description: The sectioning content element used as the outermost element of the FlexiSectionBlock.
  - Default: `SectioningContentElement.Section`
  - Usage:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiOptionsBlocks
    --------------- Markdown ---------------
    @{
        "element": "nav"
    }
    ## foo
    --------------- Expected Markup ---------------
    <nav class="section-level-2" id="foo">
    <header>
    <h2>foo</h2>
    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M17 7h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8zm9-4h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8z"/></svg>
    </header>
    </nav>
    ````````````````````````````````

- `GenerateIdentifier`
  - Type: `bool`
  - Description: The boolean value specifying whether or not an ID should be generated for the FlexiSectionBlock's 
    outermost element.
    If this value is true, an ID will be generated from the FlexiSectionBlock's header's content. 
    Otherwise, no ID will be generated.
  - Default: `true`
  - Usage:
    By default, this value is true, so a kebab-case (lowercase words joined by dashes) ID is generated for the FlexiSectionBlock:
    ```````````````````````````````` none
    --------------- Markdown ---------------
    ## Foo Bar Baz
    --------------- Expected Markup ---------------
    <section class="section-level-2" id="foo-bar-baz">
    <header>
    <h2>Foo Bar Baz</h2>
    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M17 7h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8zm9-4h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8z"/></svg>
    </header>
    </section>
    ````````````````````````````````
    When this value is false, no ID is generated for the FlexiSectionBlock:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiOptionsBlocks
    --------------- Markdown ---------------
    @{
        "generateIdentifier": false
    }
    ## Foo Bar Baz
    --------------- Expected Markup ---------------
    <section class="section-level-2">
    <header>
    <h2>Foo Bar Baz</h2>
    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M17 7h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8zm9-4h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8z"/></svg>
    </header>
    </section>
    ````````````````````````````````

- `AutoLinkable`
  - Type: `bool`
  - Description: The boolean value specifying whether or not the FlexiSectionBlock should be linkable to using its
    header's content (auto-linkable).
    If this value is true and the FlexiSectionBlock's outermost element has an ID, enables auto-linking for
    the FlexiSectionBlock. Otherwise, auto-linking will be disabled.
  - Default: `true`
  - Usage:
    By default, this value is true, so the FlexiSectionBlock can be linked to using its heading's content:
    ```````````````````````````````` none
    --------------- Markdown ---------------
    [foo]

    ## foo

    [foo]
    [Link Text][foo]
    --------------- Expected Markup ---------------
    <p><a href="#foo">foo</a></p>
    <section class="section-level-2" id="foo">
    <header>
    <h2>foo</h2>
    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M17 7h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8zm9-4h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8z"/></svg>
    </header>
    <p><a href="#foo">foo</a>
    <a href="#foo">Link Text</a></p>
    </section>
    ````````````````````````````````
    When this value is false, the FlexiSectionBlock cannot be linked to using its heading's content:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiOptionsBlocks
    --------------- Markdown ---------------
    [foo]

    @{
        "autoLinkable": false
    }
    ## foo

    [foo]
    [Link Text][foo]
    --------------- Expected Markup ---------------
    <p>[foo]</p>
    <section class="section-level-2" id="foo">
    <header>
    <h2>foo</h2>
    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M17 7h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8zm9-4h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8z"/></svg>
    </header>
    <p>[foo]
    [Link Text][foo]</p>
    </section>
    ````````````````````````````````

- `ClassFormat`
  - Type: `string`
  - Description: The format for the FlexiSectionBlock's outermost element's class.
    The FlexiSectionBlock's level will replace "{0}" in the format. 
    If this value is null, whitespace or an empty string, no class is assigned.
  - Default: "section-level-{0}"
  - Usage: 
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiOptionsBlocks
    --------------- Markdown ---------------
    @{
        "classFormat": "level-{0}"
    }
    ## foo
    --------------- Expected Markup ---------------
    <section class="level-2" id="foo">
    <header>
    <h2>foo</h2>
    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M17 7h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8zm9-4h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8z"/></svg>
    </header>
    </section>
    ````````````````````````````````

- `LinkIconMarkup`
  - Type: `string`
  - Description: The markup for the FlexiSectionBlock's link icon.
    If this value is null, whitespace or an empty string, no copy icon is rendered.
  - Default: [Material Design "File Copy" Icon](https://material.io/tools/icons/?icon=link&style=sharp)
  - Usage:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiOptionsBlocks
    --------------- Markdown ---------------
    @{
        "linkIconMarkup": "<svg><use xlink:href=\"#material-design-link\"></use></svg>"
    }
    ## foo
    --------------- Expected Markup ---------------
    <section class="section-level-2" id="foo">
    <header>
    <h2>foo</h2>
    <svg><use xlink:href="#material-design-link"></use></svg>
    </header>
    </section>
    ````````````````````````````````

- `Attributes`
  - Type: `IDictionary<string, string>`
  - Description: The HTML attributes for the FlexiSectionBlock's outermost element.
    If this value is null, no attributes will be assigned to the outermost element.
  - Default: `null`
  - Usage:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiOptionsBlocks
    --------------- Markdown ---------------
    @{
        "attributes": {
            "id" : "section-1",
            "class" : "block"
        }
    }
    ## foo
    --------------- Expected Markup ---------------
    <section id="section-1" class="block section-level-2">
    <header>
    <h2>foo</h2>
    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M17 7h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8zm9-4h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8z"/></svg>
    </header>
    </section>
    ````````````````````````````````
    If a value is specified for the class attribute, it will not override the outermost element's generated class. Instead, it will be 
    prepended to the generated class. In the above example, this results in the outermost element's class attribute having the value 
    `block secion-level-2`.

### `FlexiSectionBlocksExtensionOptions`
Global options for FlexiSectionBlocks. These options can be used to define defaults for all FlexiSectionBlocks. They have
lower precedence than block specific options specified using the FlexiOptionsBlocks extension.  

FlexiSectionBlocksExtensionOptions can be specified when enabling the FlexiSectionBlocks extension:
``` 
MyMarkdownPipelineBuilder.UseFlexiSectionBlocks(myFlexiSectionBlocksExtensionOptions);
```

#### Properties
- `DefaultBlockOptions`
  - Type: `FlexiSectionBlockOptions`
  - Description: Default `FlexiSectionBlockOptions` for all FlexiSectionBlocks. 
  - Usage:
    ```````````````````````````````` none
    --------------- Extension Options ---------------
    {
        "flexiSectionBlocks": {
            "defaultBlockOptions": {
                "element": "nav",
                "classFormat": "level-{0}",
                "linkIconMarkup": "<svg><use xlink:href=\"#material-design-link\"></use></svg>",
                "attributes": {
                    "class": "block"
                }
            }
        }
    }
    --------------- Markdown ---------------
    # foo

    # foo
    --------------- Expected Markup ---------------
    <nav class="block level-1" id="foo">
    <header>
    <h1>foo</h1>
    <svg><use xlink:href="#material-design-link"></use></svg>
    </header>
    </nav>
    <nav class="block level-1" id="foo-1">
    <header>
    <h1>foo</h1>
    <svg><use xlink:href="#material-design-link"></use></svg>
    </header>
    </nav>
    ````````````````````````````````

    Default FlexiSectionBlockOptions have lower precedence than block specific options:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiOptionsBlocks
    --------------- Extension Options ---------------
    {
        "flexiSectionBlocks": {
            "defaultBlockOptions": {
                "element": "nav"
            }
        }
    }
    --------------- Markdown ---------------
    # foo

    @{
        "element": "article"
    }
    # foo
    --------------- Expected Markup ---------------
    <nav class="section-level-1" id="foo">
    <header>
    <h1>foo</h1>
    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M17 7h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8zm9-4h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8z"/></svg>
    </header>
    </nav>
    <article class="section-level-1" id="foo-1">
    <header>
    <h1>foo</h1>
    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M17 7h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8zm9-4h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8z"/></svg>
    </header>
    </article>
    ````````````````````````````````

## Mechanics
As per the [HTML spec](https://html.spec.whatwg.org/multipage/sections.html#headings-and-sections), [sectioning roots](https://html.spec.whatwg.org/multipage/sections.html#sectioning-root)
have their own section trees:

```````````````````````````````` none
--------------- Markdown ---------------
# foo

> # foo
> ## foo

## foo
--------------- Expected Markup ---------------
<section class="section-level-1" id="foo">
<header>
<h1>foo</h1>
<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M17 7h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8zm9-4h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8z"/></svg>
</header>
<blockquote>
<section class="section-level-1" id="foo-1">
<header>
<h1>foo</h1>
<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M17 7h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8zm9-4h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8z"/></svg>
</header>
<section class="section-level-2" id="foo-2">
<header>
<h2>foo</h2>
<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M17 7h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8zm9-4h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8z"/></svg>
</header>
</section>
</section>
</blockquote>
<section class="section-level-2" id="foo-3">
<header>
<h2>foo</h2>
<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M17 7h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8zm9-4h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8z"/></svg>
</header>
</section>
</section>
````````````````````````````````
In the above spec, the level 1 ATX heading in the blockquote does not cause its preceding FlexiSectionBlock to close. Instead, it starts
a new section tree within the blockquote.