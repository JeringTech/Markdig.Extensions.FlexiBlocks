## Flexi Option Blocks
Per-block options are useful for many extensions. For example, per-block options would allow a code extension to add line-numbers to select code blocks. 
Json options facilitates per-block options, using a simple and consistent syntax.

Json options are specified as a string above the block they apply to. The first line must begin with `@{`:

```````````````````````````````` example
@{"wrapperElement": "Aside"}
# foo
.
<aside id="foo">
<header class="header-level-1">
<h1>foo</h1>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
</aside>
````````````````````````````````

Options can be specified across several lines:

```````````````````````````````` example
@{
    "wrapperElement": "Aside"
}
# foo
.
<aside id="foo">
<header class="header-level-1">
<h1>foo</h1>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>
</aside>
````````````````````````````````

If the first line does not begin with `@{`, the string becomes a paragraph:

```````````````````````````````` example
@
{
    "wrapperElement": "Aside"
}
# foo
.
<p>@
{
&quot;wrapperElement&quot;: &quot;Aside&quot;
}</p>
<header class="header-level-1">
<h1>foo</h1>
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"></path></svg>
</header>

````````````````````````````````
