---
blockOptions: "../src/FlexiBlocks/Extensions/IncludeBlocks/IncludeBlockOptions.cs"
utilityTypes: ["../src/FlexiBlocks/Extensions/IncludeBlocks/Clipping.cs"]
extensionOptions: "../src/FlexiBlocks/Extensions/IncludeBlocks/IncludeBlocksExtensionOptions.cs"
---

# IncludeBlocks
An IncludeBlock includes content from a local or remote source. IncludeBlocks can include content as markdown or in code blocks, this facilitates
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
  +{
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
using System.Io;
using Markdig;
using Jering.Markdig.Extensions.IncludeBlocks;

...

// Write a dummy file to disk
string tempPath = Path.GetTempPath();
File.WriteAllText(tempPath + "exampleInclude.js", @"function exampleFunction(arg) {
    // Example comment
    return arg + 'dummyString';
}");

var markdownPipelineBuilder = new MarkdownPipelineBuilder();
markdownPipelineBuilder.UseIncludeBlocks(new IncludeBlocksExtensionOptions(baseUri: tempPath));
MarkdownPipeline markdownPipeline = markdownPipelineBuilder.Build();

string markdown = "+{ "source": "exampleInclude.js" }"; // Root content
string html = Markdown.ToHtml(markdown, markdownPipeline);
string expectedHtml = @"<div class=""flexi-code flexi-code_no_title flexi-code_no_syntax-highlights flexi-code_no_line-numbers flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases"">
<header class=""flexi-code__header"">
<span class=""flexi-code__title""></span>
<button class=""flexi-code__copy-button"" title=""Copy code"" aria-label=""Copy code"">
<svg class=""flexi-code__copy-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""18px"" height=""18px"" viewBox=""0 0 18 18""><path fill=""none"" d=""M0,0h18v18H0V0z""/><path d=""M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z""/></svg>
</button>
</header>
<pre class=""flexi-code__pre""><code class=""flexi-code__code"">function exampleFunction(arg) {
    // Example comment
    return arg + 'dummyString';
}</code></pre>
</div>";

Assert.Equal(expectedHtml, html)
```

# Basics
In markdown, an IncludeBlock is an [`IncludeBlockOptions`](#includeblockoptions) object in JSON form, prepended with `+`. The following is an IncludeBlock:

```````````````````````````````` none
--------------- Markdown ---------------
+{ "type": "markdown", "source": "exampleInclude.md" }
--------------- Expected Markup ---------------
<p>This is example markdown.</p>
<ul>
<li>This is a list item.</li>
</ul>
<blockquote>
<p>This is a blockquote.</p>
</blockquote>
````````````````````````````````
Refer to [`IncludeBlockOptions`](#includeblockoptions) for information on configuring IncludeBlocks.  

The JSON can span multiple lines:
```````````````````````````````` none
--------------- Markdown ---------------
+{
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

The starting `+` character must immediately precede `{`. The following is a [list item](https://spec.commonmark.org/0.28/#list-items)
instead of an IncludeBlock because there is a space between `+` and `{`:
```````````````````````````````` none
--------------- Markdown ---------------
+ {
    "type": "markdown",
    "source": "exampleInclude.md"    
}
--------------- Expected Markup ---------------
<ul>
<li>{
&quot;type&quot;: &quot;markdown&quot;,
&quot;source&quot;: &quot;exampleInclude.md&quot;<br />
}</li>
</ul>
````````````````````````````````

## Options
### `IncludeBlockOptions`
Options for an IncludeBlock. To specify `IncludeBlockOptions` for an IncludeBlock, the [Options](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/OptionsBlocksSpecs.md#options) extension must be enabled.

#### Properties

##### `Source`
- Type: `string`
- Description: The `IncludeBlock`'s source.
  This value must either be a relative URI or an absolute URI with scheme file, http or https.
  If this value is a relative URI and the `IncludeBlock` is in root content, `IIncludeBlocksExtensionOptions.BaseUri`
  is used as the base URI.
  If this value is a relative URI and the `IncludeBlock` is in non-root content, the absolute URI of its containing source is used as the base URI.
  For example, consider standard Markdig usage: `string html = Markdown.ToHtml(rootContent, yourMarkdownPipeline);`.
  To Markdig, root content has no associated source, it is just a string containing markup.
  To work around this limitation, if the root content contains an `IncludeBlock` with a relative URI source like "../my/path/file1.md", `IncludeBlocksExtension`
  uses `IIncludeBlocksExtensionOptions.BaseUri` to resolve the absolute URI of "file1.md".
  As such, `IIncludeBlocksExtensionOptions.BaseUri` is typically configured as the absolute URI of the root source.
  If "file1.md" contains an IncludeBlock with source "../my/path/file2.md", we use the previously resolved absolute URI of "file1.md" as the base URI to
  resolve the absolute URI of "file2.md".
  Note that retrieving content from remote sources can introduce security issues. As far as possible, retrieve remote content only from trusted or permanent links. For example,
  from [Github permalink](https://help.github.com/articles/getting-permanent-links-to-files/)s. Additionally, consider sanitizing generated HTML.
- Default: `string.Empty`
- Examples:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  +{
      "type": "markdown",
      "source": "https://raw.githubusercontent.com/JeringTech/Markdig.Extensions.FlexiBlocks/bb51313054e8d93ada0c1e779fb4db6eac9bb6f1/test/FlexiBlocks/exampleInclude.md"
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
  +{
      "source": "exampleInclude.js",
      "clippings":[{"endLine": 4}, {"startLine": 7, "endLine": -2}]
  }
  --------------- Expected Markup ---------------
  <div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
  <header class="flexi-code__header">
  <span class="flexi-code__title"></span>
  <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
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
  +{
      "source": "exampleInclude.js",
      "clippings":[{"region": "utility methods"}]
  }
  --------------- Expected Markup ---------------
  <div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
  <header class="flexi-code__header">
  <span class="flexi-code__title"></span>
  <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
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
  +{
      "source": "exampleInclude.js",
      "clippings":[{"startString": "#region utility methods", "endString": "#endregion utility methods"}]
  }
  --------------- Expected Markup ---------------
  <div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
  <header class="flexi-code__header">
  <span class="flexi-code__title"></span>
  <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
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
  +{
      "source": "exampleInclude.js",
      "clippings":[{"startLine": 7, "endString": "#endregion utility methods"}]
  }
  --------------- Expected Markup ---------------
  <div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
  <header class="flexi-code__header">
  <span class="flexi-code__title"></span>
  <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
  <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
  </button>
  </header>
  <pre class="flexi-code__pre"><code class="flexi-code__code">function add(a, b) {
      return a + b;
  }
  </code></pre>
  </div>
  ````````````````````````````````

  Content can be appended and prepended to clippings in code type IncludeBlocks:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiCodeBlocks
  --------------- Markdown ---------------
  +{
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
  <div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
  <header class="flexi-code__header">
  <span class="flexi-code__title"></span>
  <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
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
  +{
      "source": "exampleInclude.js",
      "clippings":[{"dedent": 2}],
  }
  --------------- Expected Markup ---------------
  <div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
  <header class="flexi-code__header">
  <span class="flexi-code__title"></span>
  <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
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
  +{
      "source": "exampleInclude.js",
      "clippings":[{"indent": 2}],
  }
  --------------- Expected Markup ---------------
  <div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
  <header class="flexi-code__header">
  <span class="flexi-code__title"></span>
  <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
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
  +{
      "source": "exampleInclude.js",
      "clippings":[{"collapse": 0.5}]
  }
  --------------- Expected Markup ---------------
  <div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
  <header class="flexi-code__header">
  <span class="flexi-code__title"></span>
  <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
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
- Type: `IncludeType`
- Description: The `IncludeBlock`'s type.
  If this value is `IncludeType.Code`, the included content is rendered in a code block.
  If this value is `IncludeType.Markdown`, the included content is processed as markdown.
- Default: `IncludeType.Code`
- Examples:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  +{
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
- Description: The value specifying whether to cache the `IncludeBlock`'s content on disk.
  If this value is true and the `IncludeBlock`'s source is remote, the source's content is cached on disk.
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
  +{
      "cacheOnDisk": false,
      "type": "markdown",
      "source": "https://raw.githubusercontent.com/JeringTech/Markdig.Extensions.FlexiBlocks/6998b1c27821d8393ad39beb54f782515c39d98b/test/FlexiBlocks.Tests/exampleInclude.md"
  }
  --------------- Expected Markup ---------------
  <p>This is example markdown.</p>
  ````````````````````````````````

##### `CacheDirectory`
- Type: `string`
- Description: The directory to cache the `IncludeBlock`'s content in.
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
  Due to Markdig limitations, this value may not work properly if the `IncludeBlock` this `Clipping` belongs to has `IIncludeBlockOptions.Type`
  `IncludeType.Markdown`.
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
  This value is ignored if the `IncludeBlock` this `Clipping` belongs to has `IIncludeBlockOptions.Type` `IncludeType.Markdown`.
- Default: `null`

##### `After`
- Type: `string`
- Description: The content to be appended to the `Clipping`.
  This value is ignored if the `IncludeBlock` this `Clipping` belongs to has `IIncludeBlockOptions.Type` `IncludeType.Markdown` .
- Default: `null`

### `IncludeBlocksExtensionOptions`
Options for the IncludeBlocks extension. There are two ways to specify these options:
- Pass an `IncludeBlocksExtensionOptions` when calling `MarkdownPipelineBuilderExtensions.UseIncludeBlocks(this MarkdownPipelineBuilder pipelineBuilder, IIncludeBlocksExtensionOptions options)`.
- Insert an `IncludeBlocksExtensionOptions` into a `MarkdownParserContext.Properties` with key `typeof(IIncludeBlocksExtensionOptions)`. Pass the `MarkdownParserContext` when you call a markdown processing method
  like `Markdown.ToHtml(markdown, stringWriter, markdownPipeline, yourMarkdownParserContext)`.  
  This method allows for different extension options when reusing a pipeline. Options specified using this method take precedence.

#### Constructor Parameters

##### `defaultBlockOptions`
- Type: `IIncludeBlockOptions`
- Description: Default `IIncludeBlockOptions` for all `IncludeBlock`s.
  If this value is `null`, an `IncludeBlockOptions` with default values is used.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extension Options ---------------
  {
      "includeBlocks": {
          "defaultBlockOptions": {
              "type": "markdown"
          }
      }
  }
  --------------- Markdown ---------------
  +{
      "source": "exampleInclude.md"
  }

  +{
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

  Default IncludeBlockOptions have lower precedence than block specific options:
  ```````````````````````````````` none
  --------------- Extra Extensions ---------------
  FlexiCodeBlocks
  --------------- Extension Options ---------------
  {
      "includeBlocks": {
          "defaultBlockOptions": {
              "type": "markdown"
          }
      }
  }
  --------------- Markdown ---------------
  +{
      "source": "exampleInclude.md"
  }

  +{
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
  <div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_no_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
  <header class="flexi-code__header">
  <span class="flexi-code__title"></span>
  <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
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
- Description: The base URI for `IncludeBlock`s in root content.
  If this value is `null`, the application's working directory is used as the base URI.
  Note that the application's working directory is what `Directory.GetCurrentDirectory` returns.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extension Options ---------------
  {
      "includeBlocks": {
          "baseUri": "https://raw.githubusercontent.com"
      }
  }
  --------------- Markdown ---------------
  +{
      "type": "markdown",
      "source": "JeremyTCD/Markdig.Extensions.FlexiBlocks/390395942467555e47ad3cc575d1c8ebbceead15/test/FlexiBlocks.Tests/exampleInclude.md"
  }
  --------------- Expected Markup ---------------
  <p>This is example markdown.</p>
  ````````````````````````````````

## Mechanics
An [OptionsBlock](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/OptionsBlocksSpecs.md) can be placed before an IncludeBlock. 
It will be applied to the first block included by the IncludeBlock. For example, an IncludeBlock with type `IncludeType.Code` includes a single code block -
if the [FlexiCodeBlocks](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiCodeBlocksSpecs.md) extension is enabled, we can 
specify FlexiCodeBlockOptions for the generated FlexiCodeBlock:

```````````````````````````````` none
--------------- Extra Extensions ---------------
OptionsBlocks
FlexiCodeBlocks
--------------- Markdown ---------------
@{
    "language": "javascript"
}
+{
    "source": "exampleInclude.js"
}
--------------- Expected Markup ---------------
<div class="flexi-code flexi-code_no_title flexi-code_has_copy-icon flexi-code_language-javascript flexi-code_has_syntax-highlights flexi-code_no_line-numbers flexi-code_has_omitted-lines-icon flexi-code_no_highlighted-lines flexi-code_no_highlighted-phrases">
<header class="flexi-code__header">
<span class="flexi-code__title"></span>
<button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
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

You can nest IncludeBlocks:

```````````````````````````````` none
--------------- Markdown ---------------
+{
    "type": "markdown",
    "source": "https://raw.githubusercontent.com/JeringTech/Markdig.Extensions.FlexiBlocks/bb51313054e8d93ada0c1e779fb4db6eac9bb6f1/test/FlexiBlocks/exampleIncludeWithNestedInclude.md"
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

You can use IncludeBlocks anywhere you might use a typical block. For example, in a list item:
```````````````````````````````` none
--------------- Markdown ---------------
- First item.
- Second item  

  +{
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
> +{
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