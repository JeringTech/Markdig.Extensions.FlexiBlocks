## Flexi Code
Flexi code has the following features:

- Copy code icon
- Code block title
- Syntax highlighting
- Line numbers

These features can be configured at the extension level using `FlexiCodeExtensionOptions` and can also be configured at the 
block level using JSON options.

Flexi code blocks have the same syntax as CommonMark fenced and indented code blocks.
The following is an example of a fenced flexi code block with the default options:

```````````````````````````````` example
```
Code here!
```
.
<div class="flexi-code">
<header>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
</header>
<pre><code>Code here!
</code></pre>
</div>
````````````````````````````````

The following is an example of an indented flexi code block with the default options:

```````````````````````````````` example
    Code here!
.
<div class="flexi-code">
<header>
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0,0h24v24H0V0z"/><path d="M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z"/></svg>
</header>
<pre><code>Code here!
</code></pre>
</div>
````````````````````````````````