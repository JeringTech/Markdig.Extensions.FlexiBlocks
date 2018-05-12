## Json Options
Per-block options are useful for many extensions. For example, per-block options would allow a code extension to add line-numbers to select code blocks. 
Json options facilitates per-block options, using a simple and consistent syntax.

Json options are specified as a string above the block they apply to:

```````````````````````````````` example
options {"WrapperElement": "Aside"}
# foo
.
<aside id="foo">
<h1>foo</h1>
</aside>
````````````````````````````````

Options can be specified across several lines:

```````````````````````````````` example
options {
    "WrapperElement": "Aside"
}
# foo
.
<aside id="foo">
<h1>foo</h1>
</aside>
````````````````````````````````

TODO escape result before comparing, why doesn't "WrapperElement..." get turned into a blockquote?
The first line must begin with `options {`:

```````````````````````````````` example
options 
{
    "WrapperElement": "Aside"
}
# foo
.
<p>options
{
    "WrapperElement": "Aside"
}</p>
<h1>foo</h1>

````````````````````````````````

TODO some block that accepts strings in options to test escaping of quotes
Any valid json, including things like escaped quotes, are allowed:
```````````````````````````````` example
{
    "test": "\"test\""
}
# foo
.
<aside id="foo">
<h1>foo</h1>
</aside>
````````````````````````````````
