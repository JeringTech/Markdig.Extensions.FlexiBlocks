---
blockOptions: "../src/FlexiBlocks/Extensions/FlexiIncludeBlocks/FlexiIncludeBlockOptions.cs"
utilityTypes: ["../src/FlexiBlocks/Extensions/FlexiIncludeBlocks/Clipping.cs"]
extensionOptions: "../src/FlexiBlocks/Extensions/FlexiIncludeBlocks/FlexiIncludeBlocksExtensionOptions.cs"
requiresOptionsExtension: false
---

# FlexiIncludeBlocks
A FlexiIncludeBlock includes content from a local or remote source. FlexiIncludeBlocks can include content as markdown or in code blocks, this facilitates
testing example code and keeping markdown dry.

In this document, we refer to URIs as *sources*. The data contained in a source is referred to as *content*.
The specs in this document use the following dummy sources:
- exampleInclude.md, with content:
  ```
  This is example markdown.
  - This is a list item.
  > This is a blockquote.
  ```
- exampleIncludeWithNestedInclude.md, with content:
  ```
  i{
      "type": "markdown",
      "source": "exampleInclude.md"    
  }
  ```
- exampleInclude.js, with content:
  ```
  function exampleFunction(arg) {
      // Example comment
      return arg + 'dummyString';
  }

  //#region utility methods
  function add(a, b) {
      return a + b;
  }
  //#endregion utility methods
  ```

The markdown passed directly to Markdig is referred to as *root content*. If root content is read from a source (e.g read from a file), 
the source is referred to as the *root source*.

## Usage
```csharp
using System.IO;
using Markdig;
using Jering.Markdig.Extensions.FlexiIncludeBlocks;

...

// Write a dummy file to disk
string tempPath = Path.GetTempPath();
File.WriteAllText(Path.Combine(tempPath, "exampleInclude.js"), @"function exampleFunction(arg) {
    // Example comment
    return arg + 'dummyString';
}");

var markdownPipelineBuilder = new MarkdownPipelineBuilder();
markdownPipelineBuilder.UseFlexiIncludeBlocks(new FlexiIncludeBlocksExtensionOptions(baseUri: tempPath));
MarkdownPipeline markdownPipeline = markdownPipelineBuilder.Build();

string markdown = "i{ \"source\": \"exampleInclude.js\" }"; // Root content
string html = Markdown.ToHtml(markdown, markdownPipeline);
string expectedHtml = @"<div class=""flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases"">
<header class=""flexi-code__header"">
<button class=""flexi-code__copy-button"" aria-label=""Copy code"">
<svg class=""flexi-code__copy-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""18px"" height=""18px"" viewBox=""0 0 18 18""><path fill=""none"" d=""M0,0h18v18H0V0z""/><path d=""M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z""/></svg>
</button>
</header>
<pre class=""flexi-code__pre""><code class=""flexi-code__code"">function exampleFunction(arg) {
    // Example comment
    return arg + 'dummyString';
}
</code></pre>
</div>";

Assert.Equal(expectedHtml, html)
```

# Basics
In markdown, a FlexiIncludeBlock is an [`FlexiIncludeBlockOptions`](#includeblockoptions) object in JSON form, prepended with `i`. The following is a FlexiIncludeBlock:

```````````````````````````````` none
--------------- Extra Extensions ---------------
FlexiCodeBlocks
--------------- Markdown ---------------
i{ "source": "exampleInclude.js" }
--------------- Expected Markup ---------------
<div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
<header class="flexi-code__header">
<button class="flexi-code__copy-button" aria-label="Copy code">
<svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
</button>
</header>
<pre class="flexi-code__pre"><code class="flexi-code__code">function exampleFunction(arg) {
    // Example comment
    return arg + 'dummyString';
}

//#region utility methods
function add(a, b) {
    return a + b;
}
//#endregion utility methods
</code></pre>
</div>
````````````````````````````````
Refer to [`FlexiIncludeBlockOptions`](#includeblockoptions) for information on configuring FlexiIncludeBlocks.

The JSON can span multiple lines:
```````````````````````````````` none
--------------- Markdown ---------------
i{
    "type": "markdown",
    "source": "exampleInclude.md"    
}
--------------- Expected Markup ---------------
<p>This is example markdown.</p>
<ul>
<li>This is a list item.</li>
</ul>
<blockquote>
<p>This is a blockquote.</p>
</blockquote>
````````````````````````````````

The starting `i` character must immediately precede `{`:
```````````````````````````````` none
--------------- Markdown ---------------
i {
    "type": "markdown",
    "source": "exampleInclude.md"    
}
--------------- Expected Markup ---------------
<p>i {
&quot;type&quot;: &quot;markdown&quot;,
&quot;source&quot;: &quot;exampleInclude.md&quot;<br />
}</p>
````````````````````````````````

## Options
### `FlexiIncludeBlockOptions`
Options for a FlexiIncludeBlock.

#### Properties

##### `Source`
- Type: `string`
- Description: The `FlexiIncludeBlock`'s source.
  This value must either be a relative URI or an absolute URI with scheme file, http or https.
  If this value is a relative URI and the `FlexiIncludeBlock` is in root content, `IFlexiIncludeBlocksExtensionOptions.BaseUri`
  is used as the base URI.
  If this value is a relative URI and the `FlexiIncludeBlock` is in non-root content, the absolute URI of its containing source is used as the base URI.
  For example, consider standard Markdig usage: `string html = Markdown.ToHtml(rootContent, yourMarkdownPipeline);`.
  To Markdig, root content has no associated source, it is just a string containing markup.
  To work around this limitation, if the root content contains a `FlexiIncludeBlock` with a relative URI source like "../my/path/file1.md", `FlexiIncludeBlocksExtension`
  uses `IFlexiIncludeBlocksExtensionOptions.BaseUri` to resolve the absolute URI of "file1.md".
  As such, `IFlexiIncludeBlocksExtensionOptions.BaseUri` is typically configured as the absolute URI of the root source.
  If "file1.md" contains a FlexiIncludeBlock with source "../my/path/file2.md", we use the previously resolved absolute URI of "file1.md" as the base URI to
  resolve the absolute URI of "file2.md".
  Note that retrieving content from remote sources can introduce security issues. As far as possible, retrieve remote content only from trusted or permanent links. For example,
  from [Github permalink](https://help.github.com/articles/getting-permanent-links-to-files/)s. Additionally, consider sanitizing generated HTML.
- Default: `string.Empty`
- Examples:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  i{
      "type": "markdown",
      "source": "https://raw.githubusercontent.com/JeringTech/Markdig.Extensions.FlexiBlocks/cf4cc222079d2c3845c74826bd7aa1c2c6fd967f/test/FlexiBlocks/exampleInclude.md"
  }
  --------------- Expected Markup ---------------
  <p>This is example markdown.</p>
  <ul>
  <li>This is a list item.</li>
  </ul>
  <blockquote>
  <p>This is a blockquote.</p>
  </blockquote>
  ````````````````````````````````

##### `Clippings`
- Type: `IList<Clipping>`
- Description: The `Clipping`s specifying content from the source to include.
  If this value is `null` or empty, all content from the source is included.
- Default: `null`
- Examples:  
  Clipping using line numbers:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiCodeBlocks
  --------------- Markdown ---------------
  i{
      "source": "exampleInclude.js",
      "clippings":[{"endLine": 4}, {"startLine": 7, "endLine": -2}]
  }
  --------------- Expected Markup ---------------
  <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
  <header class="flexi-code__header">
  <button class="flexi-code__copy-button" aria-label="Copy code">
  <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
  </button>
  </header>
  <pre class="flexi-code__pre"><code class="flexi-code__code">function exampleFunction(arg) {
      // Example comment
      return arg + 'dummyString';
  }
  function add(a, b) {
      return a + b;
  }
  </code></pre>
  </div>
  ````````````````````````````````

  Clipping using a region:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiCodeBlocks
  --------------- Markdown ---------------
  i{
      "source": "exampleInclude.js",
      "clippings":[{"region": "utility methods"}]
  }
  --------------- Expected Markup ---------------
  <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
  <header class="flexi-code__header">
  <button class="flexi-code__copy-button" aria-label="Copy code">
  <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
  </button>
  </header>
  <pre class="flexi-code__pre"><code class="flexi-code__code">function add(a, b) {
      return a + b;
  }
  </code></pre>
  </div>
  ````````````````````````````````

  Clipping using demarcation line substrings:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiCodeBlocks
  --------------- Markdown ---------------
  i{
      "source": "exampleInclude.js",
      "clippings":[{"startString": "#region utility methods", "endString": "#endregion utility methods"}]
  }
  --------------- Expected Markup ---------------
  <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
  <header class="flexi-code__header">
  <button class="flexi-code__copy-button" aria-label="Copy code">
  <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
  </button>
  </header>
  <pre class="flexi-code__pre"><code class="flexi-code__code">function add(a, b) {
      return a + b;
  }
  </code></pre>
  </div>
  ````````````````````````````````

  Clipping using a combination of line numbers and demarcation line substrings:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiCodeBlocks
  --------------- Markdown ---------------
  i{
      "source": "exampleInclude.js",
      "clippings":[{"startLine": 7, "endString": "#endregion utility methods"}]
  }
  --------------- Expected Markup ---------------
  <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
  <header class="flexi-code__header">
  <button class="flexi-code__copy-button" aria-label="Copy code">
  <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
  </button>
  </header>
  <pre class="flexi-code__pre"><code class="flexi-code__code">function add(a, b) {
      return a + b;
  }
  </code></pre>
  </div>
  ````````````````````````````````

  Content can be appended and prepended to clippings in code type FlexiIncludeBlocks:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiCodeBlocks
  --------------- Markdown ---------------
  i{
      "source": "exampleInclude.js",
      "clippings":[{
          "endLine": 1,
          "after": "..."
      },
      {
          "startLine": 4,
          "endLine": 4
      },
      {
          "startLine": 7, 
          "endLine": 7,
          "before": ""
      },
      {
          "startLine": 9, 
          "endLine": 9,
          "before": "..."
      }]
  }
  --------------- Expected Markup ---------------
  <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
  <header class="flexi-code__header">
  <button class="flexi-code__copy-button" aria-label="Copy code">
  <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
  </button>
  </header>
  <pre class="flexi-code__pre"><code class="flexi-code__code">function exampleFunction(arg) {
  ...
  }

  function add(a, b) {
  ...
  }
  </code></pre>
  </div>
  ````````````````````````````````

  Dedenting leading whitespace in a clipping:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiCodeBlocks
  --------------- Markdown ---------------
  i{
      "source": "exampleInclude.js",
      "clippings":[{"dedent": 2}],
  }
  --------------- Expected Markup ---------------
  <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
  <header class="flexi-code__header">
  <button class="flexi-code__copy-button" aria-label="Copy code">
  <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
  </button>
  </header>
  <pre class="flexi-code__pre"><code class="flexi-code__code">function exampleFunction(arg) {
    // Example comment
    return arg + 'dummyString';
  }

  //#region utility methods
  function add(a, b) {
    return a + b;
  }
  //#endregion utility methods
  </code></pre>
  </div>
  ````````````````````````````````

  Indenting leading whitespace in a clipping:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiCodeBlocks
  --------------- Markdown ---------------
  i{
      "source": "exampleInclude.js",
      "clippings":[{"indent": 2}],
  }
  --------------- Expected Markup ---------------
  <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
  <header class="flexi-code__header">
  <button class="flexi-code__copy-button" aria-label="Copy code">
  <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
  </button>
  </header>
  <pre class="flexi-code__pre"><code class="flexi-code__code">  function exampleFunction(arg) {
        // Example comment
        return arg + 'dummyString';
    }

    //#region utility methods
    function add(a, b) {
        return a + b;
    }
    //#endregion utility methods
  </code></pre>
  </div>
  ````````````````````````````````

  Collapsing leading whitespace in a clipping:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiCodeBlocks
  --------------- Markdown ---------------
  i{
      "source": "exampleInclude.js",
      "clippings":[{"collapse": 0.5}]
  }
  --------------- Expected Markup ---------------
  <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
  <header class="flexi-code__header">
  <button class="flexi-code__copy-button" aria-label="Copy code">
  <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
  </button>
  </header>
  <pre class="flexi-code__pre"><code class="flexi-code__code">function exampleFunction(arg) {
    // Example comment
    return arg + 'dummyString';
  }

  //#region utility methods
  function add(a, b) {
    return a + b;
  }
  //#endregion utility methods
  </code></pre>
  </div>
  ````````````````````````````````

##### `Type`
- Type: `FlexiIncludeType`
- Description: The `FlexiIncludeBlock`'s type.
  If this value is `FlexiIncludeType.Code`, the included content is rendered in a code block.
  If this value is `FlexiIncludeType.Markdown`, the included content is processed as markdown.
- Default: `FlexiIncludeType.Code`
- Examples:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  i{
      "type": "markdown",
      "source": "exampleInclude.md"
  }
  --------------- Expected Markup ---------------
  <p>This is example markdown.</p>
  <ul>
  <li>This is a list item.</li>
  </ul>
  <blockquote>
  <p>This is a blockquote.</p>
  </blockquote>
  ````````````````````````````````

##### `Cache`
- Type: `bool`
- Description: The value specifying whether to cache the `FlexiIncludeBlock`'s content on disk.
  If this value is true and the `FlexiIncludeBlock`'s source is remote, the source's content is cached on disk.
  Caching-on-disk slows down the first markdown-to-HTML run on a system, but significantly speeds up subsequent runs:
  If on-disk caching is enabled and content from remote source "x" is included, on the first run, all content in "x" is retrieved from a server and
  cached in memory as well as on disk.
  Subsequent requests to retrieve content from "x" during the same run will retrieve content from the in-memory cache.
  At the end of the first run, the in-memory cache is discarded when the process dies.
  For subsequent runs on the system, if content from "x" is included again, all content from "x" is retrieved from the on-disk cache, avoiding
  round trips to a server.
  If you are only going to execute one run on a system (e.g when doing CI/CD), the run will take less time if on-disk caching is disabled.
  If you are doing multiple runs on a system, on-disk caching should be enabled.
- Default: `true`
- Examples:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  i{
      "cacheOnDisk": false,
      "type": "markdown",
      "source": "https://raw.githubusercontent.com/JeringTech/Markdig.Extensions.FlexiBlocks/cf4cc222079d2c3845c74826bd7aa1c2c6fd967f/test/FlexiBlocks/exampleInclude.md"
  }
  --------------- Expected Markup ---------------
  <p>This is example markdown.</p>
  <ul>
  <li>This is a list item.</li>
  </ul>
  <blockquote>
  <p>This is a blockquote.</p>
  </blockquote>
  ````````````````````````````````

##### `CacheDirectory`
- Type: `string`
- Description: The directory to cache the `FlexiIncludeBlock`'s content in.
  This option is only relevant if caching on disk is enabled.
  If this value is `null`, whitespace or an empty string, a folder named "ContentCache" in the application's working directory is used instead.
  Note that the application's working directory is what `Directory.GetCurrentDirectory` returns.
- Default: `null`

### `Clipping`
Represents a clipping from a sequence of lines.

#### Properties

##### `StartLine`
- Type: `int`
- Description: The line number of the `Clipping`'s start line.
  If this value is `-n`, the start line is the nth last line. For example, if this value is `-2`, the start line is the 2nd last line.
  This value must not be `0`.
- Default: `1`

##### `EndLine`
- Type: `int`
- Description: The line number of the `Clipping`'s end line.
  If this value is `-n`, the end line is the nth last line. For example, if this value is `-2`, the end line is the 2nd last line.
  This value must not be `0` or an integer representing a line before the start line.
- Default: `-1`

##### `Region`
- Type: `string`
- Description: The name of the region that the `Clipping` contains.
  This value is an alternative to `StartLine` and `EndLine` and takes precedence over them.
  It is shorthand for specifying `StartString` and `EndString` -
  if this value is not `null`, whitespace or an empty string, `StartString` is set to "#region <`Region`>" and `EndString` is
  set to "#endregion".
- Default: `null`

##### `StartString`
- Type: `string`
- Description: The substring that the line immediately preceding the `Clipping` contains.
  This value is an alternative to `StartLine`.
  If this value is not `null`, whitespace or an empty string, it takes precedence over `StartLine` and `Region`.
- Default: `null`

##### `EndString`
- Type: `string`
- Description: The substring that the line immediately after the `Clipping` contains.
  This value is an alternative to `EndLine`.
  If this value is not `null`, whitespace or an empty string, it takes precedence over `EndLine` and `Region`.
- Default: `null`

##### `Dedent`
- Type: `int`
- Description: The number of leading whitespace characters to remove from each line in the `Clipping`.
  This value must not be negative.
- Default: 0

##### `Indent`
- Type: `int`
- Description: The number of leading whitespace characters to add to each line in the `Clipping`.
  Due to Markdig limitations, this value may not work properly if the `FlexiIncludeBlock` this `Clipping` belongs to has `IFlexiIncludeBlockOptions.Type`
  `FlexiIncludeType.Markdown`.
  This value must not be negative.
- Default: 0

##### `Collapse`
- Type: `float`
- Description: The proportion of leading whitespace characters (after dedenting and indenting) to keep.
  For example, if there are 9 leading whitespace characters after dedenting and this value is 0.33, the final number of leading whitespace characters will be 3.
  This value must be in the range [0, 1].
- Default: 1

##### `Before`
- Type: `string`
- Description: The content to be prepended to the `Clipping`.
  This value is ignored if the `FlexiIncludeBlock` this `Clipping` belongs to has `IFlexiIncludeBlockOptions.Type` `FlexiIncludeType.Markdown`.
- Default: `null`

##### `After`
- Type: `string`
- Description: The content to be appended to the `Clipping`.
  This value is ignored if the `FlexiIncludeBlock` this `Clipping` belongs to has `IFlexiIncludeBlockOptions.Type` `FlexiIncludeType.Markdown` .
- Default: `null`

### `FlexiIncludeBlocksExtensionOptions`
Options for the FlexiIncludeBlocks extension. There are two ways to specify these options:
- Pass a `FlexiIncludeBlocksExtensionOptions` when calling `MarkdownPipelineBuilderExtensions.UseFlexiIncludeBlocks(this MarkdownPipelineBuilder pipelineBuilder, IFlexiIncludeBlocksExtensionOptions options)`.
- Insert a `FlexiIncludeBlocksExtensionOptions` into a `MarkdownParserContext.Properties` with key `typeof(IFlexiIncludeBlocksExtensionOptions)`. Pass the `MarkdownParserContext` when you call a markdown processing method
  like `Markdown.ToHtml(markdown, stringWriter, markdownPipeline, yourMarkdownParserContext)`.  
  This method allows for different extension options when reusing a pipeline. Options specified using this method take precedence.

#### Constructor Parameters

##### `defaultBlockOptions`
- Type: `IFlexiIncludeBlockOptions`
- Description: Default `IFlexiIncludeBlockOptions` for all `FlexiIncludeBlock`s.
  If this value is `null`, a `FlexiIncludeBlockOptions` with default values is used.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extension Options ---------------
  {
      "flexiIncludeBlocks": {
          "defaultBlockOptions": {
              "type": "markdown"
          }
      }
  }
  --------------- Markdown ---------------
  i{
      "source": "exampleInclude.md"
  }

  i{
      "source": "exampleInclude.md"
  }
  --------------- Expected Markup ---------------
  <p>This is example markdown.</p>
  <ul>
  <li>This is a list item.</li>
  </ul>
  <blockquote>
  <p>This is a blockquote.</p>
  </blockquote>
  <p>This is example markdown.</p>
  <ul>
  <li>This is a list item.</li>
  </ul>
  <blockquote>
  <p>This is a blockquote.</p>
  </blockquote>
  ````````````````````````````````

  Default FlexiIncludeBlockOptions have lower precedence than block specific options:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiCodeBlocks
  --------------- Extension Options ---------------
  {
      "flexiIncludeBlocks": {
          "defaultBlockOptions": {
              "type": "markdown"
          }
      }
  }
  --------------- Markdown ---------------
  i{
      "source": "exampleInclude.md"
  }

  i{
      "type": "code",
      "source": "exampleInclude.md"
  }
  --------------- Expected Markup ---------------
  <p>This is example markdown.</p>
  <ul>
  <li>This is a list item.</li>
  </ul>
  <blockquote>
  <p>This is a blockquote.</p>
  </blockquote>
  <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
  <header class="flexi-code__header">
  <button class="flexi-code__copy-button" aria-label="Copy code">
  <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
  </button>
  </header>
  <pre class="flexi-code__pre"><code class="flexi-code__code">This is example markdown.
  - This is a list item.
  &gt; This is a blockquote.
  </code></pre>
  </div>
  ````````````````````````````````

##### `baseUri`
- Type: `string`
- Description: The base URI for `FlexiIncludeBlock`s in root content.
  If this value is `null`, the application's working directory is used as the base URI.
  Note that the application's working directory is what `Directory.GetCurrentDirectory` returns.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extension Options ---------------
  {
      "flexiIncludeBlocks": {
          "baseUri": "https://raw.githubusercontent.com"
      }
  }
  --------------- Markdown ---------------
  i{
      "type": "markdown",
      "source": "JeringTech/Markdig.Extensions.FlexiBlocks/cf4cc222079d2c3845c74826bd7aa1c2c6fd967f/test/FlexiBlocks/exampleInclude.md"
  }
  --------------- Expected Markup ---------------
  <p>This is example markdown.</p>
  <ul>
  <li>This is a list item.</li>
  </ul>
  <blockquote>
  <p>This is a blockquote.</p>
  </blockquote>
  ````````````````````````````````

## Mechanics
An [FlexiOptionsBlock](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiOptionsBlocksSpecs.md) can be placed before a FlexiIncludeBlock. 
It will be applied to the first block included by the FlexiIncludeBlock. For example, a FlexiIncludeBlock with type `FlexiIncludeType.Code` includes a single code block -
if the [FlexiCodeBlocks](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiCodeBlocksSpecs.md) extension is enabled, we can 
specify FlexiCodeBlockOptions for the generated FlexiCodeBlock:

```````````````````````````````` none
--------------- Extra Extensions ---------------
FlexiOptionsBlocks
FlexiCodeBlocks
--------------- Markdown ---------------
o{
    "language": "javascript"
}
i{
    "source": "exampleInclude.js"
}
--------------- Expected Markup ---------------
<div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_language_javascript flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
<header class="flexi-code__header">
<button class="flexi-code__copy-button" aria-label="Copy code">
<svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
</button>
</header>
<pre class="flexi-code__pre"><code class="flexi-code__code"><span class="token keyword">function</span> <span class="token function">exampleFunction</span><span class="token punctuation">(</span>arg<span class="token punctuation">)</span> <span class="token punctuation">{</span>
    <span class="token comment">// Example comment</span>
    <span class="token keyword">return</span> arg <span class="token operator">+</span> <span class="token string">'dummyString'</span><span class="token punctuation">;</span>
<span class="token punctuation">}</span>

<span class="token comment">//#region utility methods</span>
<span class="token keyword">function</span> <span class="token function">add</span><span class="token punctuation">(</span>a<span class="token punctuation">,</span> b<span class="token punctuation">)</span> <span class="token punctuation">{</span>
    <span class="token keyword">return</span> a <span class="token operator">+</span> b<span class="token punctuation">;</span>
<span class="token punctuation">}</span>
<span class="token comment">//#endregion utility methods</span>
</code></pre>
</div>
````````````````````````````````

You can nest FlexiIncludeBlocks:

```````````````````````````````` none
--------------- Markdown ---------------
i{
    "type": "markdown",
    "source": "https://raw.githubusercontent.com/JeringTech/Markdig.Extensions.FlexiBlocks/cf4cc222079d2c3845c74826bd7aa1c2c6fd967f/test/FlexiBlocks/exampleIncludeWithNestedInclude.md"
}
--------------- Expected Markup ---------------
<p>This is example markdown with an include.</p>
<p>This is example markdown.</p>
<ul>
<li>This is a list item.</li>
</ul>
<blockquote>
<p>This is a blockquote.</p>
</blockquote>
````````````````````````````````

You can use FlexiIncludeBlocks anywhere you might use a typical block. For example, in a list item:
```````````````````````````````` none
--------------- Markdown ---------------
- First item.
- Second item  

  i{
      "type": "markdown",
      "source": "exampleInclude.md"
  }
- Third item
--------------- Expected Markup ---------------
<ul>
<li><p>First item.</p></li>
<li><p>Second item</p>
<p>This is example markdown.</p>
<ul>
<li>This is a list item.</li>
</ul>
<blockquote>
<p>This is a blockquote.</p>
</blockquote></li>
<li><p>Third item</p></li>
</ul>
````````````````````````````````

Or a blockquote:
```````````````````````````````` none
--------------- Markdown ---------------
> First line.
> i{
>     "type": "markdown",
>     "source": "exampleInclude.md"
> }
> Third line
--------------- Expected Markup ---------------
<blockquote>
<p>First line.</p>
<p>This is example markdown.</p>
<ul>
<li>This is a list item.</li>
</ul>
<blockquote>
<p>This is a blockquote.</p>
</blockquote>
<p>Third line</p>
</blockquote>
````````````````````````````````
