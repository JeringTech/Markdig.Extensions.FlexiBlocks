## Json Options
Per-block options are useful for many extensions. For example, per-block options would allow a code extension to add line-numbers to select code blocks. 
Json options facilitates per-block options, using a simple and consistent syntax.

Json options are specified as a string above the block they apply to. The first line must begin with `@{`:

```````````````````````````````` example
@{"wrapperElement": "Aside"}
# foo
.
<aside id="foo">
<h1>foo</h1>
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
<h1>foo</h1>
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
<h1>foo</h1>

````````````````````````````````
