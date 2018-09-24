# FlexiIncludeBlocks
FlexiIncludeBlocks include content from local or remote sources. Content can be included as markdown or in a code block. Common applications for 
FlexiIncludeBlocks include avoiding duplicate markdown across `.md` documents and using tested code examples.

The specs in this document use the following dummy sources:
- exampleInclude.md, with content:
  ```
  This is example markdown.
  ```
- exampleIncludeWithNestedInclude.md, with content:
  ```
  +{
      "type": "Markdown",
      "sourceUri": "./exampleInclude.md"    
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

## Basic Syntax
A FlexiIncludeBlock is a [`FlexiIncludeBlockOptions`](#flexiincludeblockoptions) object in JSON form, prepended with `+`. The following is a FlexiIncludeBlock:

```````````````````````````````` none
--------------- Markdown ---------------
+{ "type": "Markdown", "sourceUri": "./exampleInclude.md" }
--------------- Expected Markup ---------------
<p>This is example markdown.</p>
````````````````````````````````
Refer to [`FlexiIncludeBlockOptions`](#flexiincludeblockoptions) for more information on configuring FlexiIncludeBlocks.  

The JSON can span any number of lines, as long as it is valid:
```````````````````````````````` none
--------------- Markdown ---------------
+{
    "type": "Markdown",
    "sourceUri": "./exampleInclude.md"    
}
--------------- Expected Markup ---------------
<p>This is example markdown.</p>
````````````````````````````````

Starting `+` characters must immediately precede opening `{` characters. The following is a [list item](https://spec.commonmark.org/0.28/#list-items)
because there is a space between the starting `+` and the opening `{`:
```````````````````````````````` none
--------------- Markdown ---------------
+ {
    "type": "Markdown",
    "sourceUri": "./exampleInclude.md"    
}
--------------- Expected Markup ---------------
<ul>
<li>{
&quot;type&quot;: &quot;Markdown&quot;,
&quot;sourceUri&quot;: &quot;./exampleInclude.md&quot;<br />
}</li>
</ul>
````````````````````````````````

## Options
The FlexiIncludeBlocks extension has the following options types:

### `Clipping`
Represents a clipping from a sequence of lines. Used by [FlexiIncludeBlockOptions](#flexiincludeblockoptions).

#### Properties
- `StartLineNumber`
  - Type: `int`
  - Description: The line number of the line that this clipping starts at.
    This value must be greater than 0.
  - Default: `1`
- `endLineNumber`
  - Type: `int`
  - Description: The line number of the line that this clipping ends at.
    If this value is -1, this clipping extends to the last line. If it is not -1, it must be greater than or equal to `StartLineNumber`.
  - Default: `-1`
- `startDemarcationLineSubstring`
  - Type: `string`
  - Description: A substring that the line immediately preceding this clipping contains.
    If this value is not null, whitespace or an empty string, it takes precedence over `StartLineNumber`.
  - Default: `null`
- `endDemarcationLineSubstring`
  - Type: `string`
  - Description: A substring that the line immediately after this clipping contains.
    If this value is not null, whitespace or an empty string, it takes precedence over `EndLineNumber`.
  - Default: `null`
- `dedentLength`
  - Type: `int`
  - Description: The number of leading whitespace characters to remove from each line in this clipping.
    This value must not be negative.
  - Default: `0`
- `collapseRatio`
  - Type: `float`
  - Description: The proportion of leading whitespace characters (after dedenting) to keep.
    For example, if there are 9 leading whitespace characters after dedenting, and this value is 0.33, the final number of leading whitespace characters will be 3. 
    This value must be in the range [0, 1].
  - Default: `1`
- `beforeContent`
  - Type: `string`
  - Description: The content to be prepended to this clipping.
    This value will be processed as markdown if the FlexiIncludeBlock that this clipping belongs to has Markdown as its content type.
  - Default: `null`
- `afterContent`
  - Type: `string`
  - Description: The content to be appended to this clipping.
    This value will be processed as markdown if the FlexiIncludeBlock that this clipping belongs to has Markdown as its content type.
  - Default: `null`

### `FlexiIncludeBlockOptions`
Options for a FlexiIncludeBlock. To specify default FlexiIncludeBlockOptions for all FlexiIncludeBlocks,
use [FlexiIncludeBlocksExtensionOptions](#flexiincludeblocksextensionoptions).

#### Properties
- `SourceUri`
  - Type: `string`
  - Description: The URI of the source.
    This value must either be a relative URI or an absolute URI with scheme file, http or https.
  - Default: `string.Empty`
  - Usage:
    ```````````````````````````````` none
    --------------- Markdown ---------------
    +{
        "type": "markdown",
        "sourceUri": "https://raw.githubusercontent.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/6998b1c27821d8393ad39beb54f782515c39d98b/test/FlexiBlocks.Tests/exampleInclude.md"
    }
    --------------- Expected Markup ---------------
    <p>This is example markdown.</p>
    ````````````````````````````````
    If this value is a relative URI for a FlexiIncludeBlock in the root source, [FlexiIncludeBlocksExtensionOptions](#flexiincludeblocksextensionoptions).RootBaseUri
    is used as the base URI. If this value is a relative URI for a FlexiIncludeBlock in an included source, the URI of the source is used as the base URI.

    For example, consider a typical Markdig usage:
    ```
    string markup = Markdown.ToHtml(rootSourceAsAString, myMarkdownPipeline);
    ```
    Note how the root source has no associated URI. It is just a string. If the root source contains a FlexiIncludeBlock with `SourceUri` 
    "../my/path/file1.md", we need a base URI to generate the full path for `file1.md`. [FlexiIncludeBlocksExtensionOptions](#flexiincludeblocksextensionoptions).RootBaseUri 
    is used - typically we'd configure it to the URI of `rootSourceAsAString`.
    If `file1.md` contains a FlexiIncludeBlock with `SourceUri` "../my/path/file2.md", since the absolute path of `file1.md` is known, we 
    use it to generate the absolute path for `file2.md`.

    Do keep in mind that retrieving remote sources can introduce security issues. As far as possible, retrieve remote content only from trusted or permanent links. For example, 
    the source URI in the above spec is a [Github permalink](https://help.github.com/articles/getting-permanent-links-to-files/).
    Additionally, consider sanitizing generated markup.

- `Type`
  - Type: `IncludeType`
  - Description: The FlexiIncludeBlock's type.
    If this value is `IncludeType.Code`, a single code block containing the included content is generated. If this value is `IncludeType.Markdown`,
    included content is processed as markdown.
  - Default: `IncludeType.Code`
  - Usage:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiCodeBlocks
    --------------- Markdown ---------------
    +{
        "type": "Code",
        "sourceUri": "./exampleInclude.js"
    }
    --------------- Expected Markup ---------------
    <div>
    <header>
    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
    </header>
    <pre><code>function exampleFunction(arg) {
        // Example comment
        return arg + 'dummyString';
    }

    //#region utility methods
    function add(a, b) {
        return a + b;
    }
    //#endregion utility methods</code></pre>
    </div>
    ````````````````````````````````
    By default, `Type` is `IncludeType.Code`, so the markdown in the above spec can be simplified to:
    ```
    +{
        "sourceUri": "./exampleInclude.js"
    }
    ```

- `CacheOnDisk`
  - Type: `bool`
  - Description: The boolean value specifying whether or not to cache sources on disk.
    If this value is true, sources will be cached on disk, otherwise, content will not be cached on disk.
    On-disk caching only applies to remote sources (retrieved using HTTP or HTTPS) since local sources (retrieved from the file system) are already on disk.
  - Default: `true`
  - Usage
    ```````````````````````````````` none
    --------------- Markdown ---------------
    +{
        "cacheOnDisk": false,
        "type": "markdown",
        "sourceURI": "https://raw.githubusercontent.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/6998b1c27821d8393ad39beb54f782515c39d98b/test/FlexiBlocks.Tests/exampleInclude.md"
    }
    --------------- Expected Markup ---------------
    <p>This is example markdown.</p>
    ````````````````````````````````
    By default, on an initial run, when arbitrary remote source "a" is included, FlexiIncludeBlocks will 
    retrieve remote source "a" from a server, and immediately cache it in memory as well as on disk.
    Subsequent requests to retrieve "a" during the same run will retrieve it from 
    the in-memory cache. At the end of the initial run, the in-memory cache is lost when the
    process dies. For the next run, FlexiIncludeBlocks will 
    retrieve remote source "a" from the on-disk cache, avoiding a round trip to a server.

    This means that if you are only going to execute one run on a system (e.g on a CI/CD system), then performance 
    will be better if on-disk caching is disabled. 

- `DiskCacheDirectory`
  - Type: `string`
  - Description: The directory for the on-disk cache.
    This option is only relevant if caching on disk is enabled.
    If this value is null, whitespace or an empty string, a folder named "SourceCache" in the application's current directory is used.
  - Default: `null`

- `Clippings`
  - Type: `IList<Clipping>`
  - Description: The list of clippings from the source to include.
    If this value is null or empty, the entire source is included.
  - Default: `null`
  - Usage:  
    Refer to the [`Clipping`](#clipping) type for the full list of clipping options. Clipping using line numbers:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiCodeBlocks
    --------------- Markdown ---------------
    +{
        "sourceUri": "./exampleInclude.js",
        "clippings":[{"endLineNumber": 4}, {"startLineNumber": 7, "endLineNumber": 9}]
    }
    --------------- Expected Markup ---------------
    <div>
    <header>
    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
    </header>
    <pre><code>function exampleFunction(arg) {
        // Example comment
        return arg + 'dummyString';
    }
    function add(a, b) {
        return a + b;
    }</code></pre>
    </div>
    ````````````````````````````````

    Clipping using demarcation line substrings:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiCodeBlocks
    --------------- Markdown ---------------
    +{
        "sourceUri": "./exampleInclude.js",
        "clippings":[{"startDemarcationLineSubstring": "#region utility methods", "endDemarcationLineSubstring": "#endregion utility methods"}]
    }
    --------------- Expected Markup ---------------
    <div>
    <header>
    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
    </header>
    <pre><code>function add(a, b) {
        return a + b;
    }</code></pre>
    </div>
    ````````````````````````````````

    Clipping using a combination of line numbers and demarcation line substrings:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiCodeBlocks
    --------------- Markdown ---------------
    +{
        "sourceUri": "./exampleInclude.js",
        "clippings":[{"startLineNumber": 7, "endDemarcationLineSubstring": "#endregion utility methods"}]
    }
    --------------- Expected Markup ---------------
    <div>
    <header>
    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
    </header>
    <pre><code>function add(a, b) {
        return a + b;
    }</code></pre>
    </div>
    ````````````````````````````````

    Appending and prepending content to clippings:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiCodeBlocks
    --------------- Markdown ---------------
    +{
        "sourceUri": "./exampleInclude.js",
        "clippings":[{
            "endLineNumber": 1,
            "afterContent": "..."
        },
        {
            "startLineNumber": 4,
            "endLineNumber": 4
        },
        {
            "startLineNumber": 7, 
            "endLineNumber": 7,
            "beforeContent": ""
        },
        {
            "startLineNumber": 9, 
            "endLineNumber": 9,
            "beforeContent": "..."
        }]
    }
    --------------- Expected Markup ---------------
    <div>
    <header>
    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
    </header>
    <pre><code>function exampleFunction(arg) {
    ...
    }

    function add(a, b) {
    ...
    }</code></pre>
    </div>
    ````````````````````````````````

    Dedenting leading whitespace in a clipping:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiCodeBlocks
    --------------- Markdown ---------------
    +{
        "sourceUri": "./exampleInclude.js",
        "clippings":[{"dedentLength": 2}],
    }
    --------------- Expected Markup ---------------
    <div>
    <header>
    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
    </header>
    <pre><code>function exampleFunction(arg) {
      // Example comment
      return arg + 'dummyString';
    }

    //#region utility methods
    function add(a, b) {
      return a + b;
    }
    //#endregion utility methods</code></pre>
    </div>
    ````````````````````````````````

    Collapsing leading whitespace in a clipping:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    FlexiCodeBlocks
    --------------- Markdown ---------------
    +{
        "sourceUri": "./exampleInclude.js",
        "clippings":[{"collapseRatio": 0.5}]
    }
    --------------- Expected Markup ---------------
    <div>
    <header>
    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
    </header>
    <pre><code>function exampleFunction(arg) {
      // Example comment
      return arg + 'dummyString';
    }

    //#region utility methods
    function add(a, b) {
      return a + b;
    }
    //#endregion utility methods</code></pre>
    </div>
    ````````````````````````````````

### `FlexiIncludeBlocksExtensionOptions`
Global options for FlexiIncludeBlocks. These options can be used to define defaults for all FlexiIncludeBlocks. They have
lower precedence than block specific options.  

FlexiIncludeBlocksExtensionOptions can be specified when enabling the FlexiIncludeBlocks extension:
``` 
MyMarkdownPipelineBuilder.UseFlexiIncludeBlocks(myFlexiIncludeBlocksExtensionOptions);
```

#### Properties
- RootBaseUri
  - Type: `string`
  - Description: The base URI for FlexiIncludeBlocks in the root source.
  - Default: The application's current directory.
  - Usage:
    ```````````````````````````````` none
    --------------- Extension Options ---------------
    {
        "flexiIncludeBlocks": {
            "rootBaseUri": "https://raw.githubusercontent.com"
        }
    }
    --------------- Markdown ---------------
    +{
        "type": "markdown",
        "sourceUri": "JeremyTCD/Markdig.Extensions.FlexiBlocks/390395942467555e47ad3cc575d1c8ebbceead15/test/FlexiBlocks.Tests/exampleInclude.md"
    }
    --------------- Expected Markup ---------------
    <p>This is example markdown.</p>
    ````````````````````````````````
  
- DefaultBlockOptions
  - Type: `FlexiIncludeBlockOptions`
  - Description: Default `FlexiIncludeBlockOptions` for all FlexiIncludeBlocks. 
  - Usage:
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
    +{
        "sourceUri": "./exampleInclude.md"
    }

    +{
        "sourceUri": "./exampleInclude.md"
    }
    --------------- Expected Markup ---------------
    <p>This is example markdown.</p>
    <p>This is example markdown.</p>
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
    +{
        "sourceUri": "./exampleInclude.md"
    }

    +{
        "type": "code",
        "sourceUri": "./exampleInclude.md"
    }
    --------------- Expected Markup ---------------
    <p>This is example markdown.</p>
    <div>
    <header>
    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
    </header>
    <pre><code>This is example markdown.</code></pre>
    </div>
    ````````````````````````````````

## Mechanics
A [FlexiOptionsBlock](https://github.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiOptionsBlocksSpecs.md) can be placed before a FlexiIncludeBlock. 
It will be applied to the first block generated by the FlexiIncludeBlock. For example, a FlexiIncludeBlock with type `IncludeType.Code` generates a single code block -
if the [FlexiCodeBlocks](https://github.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiCodeBlocksSpecs.md) extension is enabled, we can specify FlexiCodeBlockOptions for the generated FlexiCodeBlock.  :
```````````````````````````````` none
--------------- Extra Extensions ---------------
FlexiOptionsBlocks
FlexiCodeBlocks
--------------- Markdown ---------------
@{
    "language": "javascript"
}
+{
    "sourceUri": "./exampleInclude.js"
}
--------------- Expected Markup ---------------
<div>
<header>
<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
</header>
<pre><code class="language-javascript"><span class="token keyword">function</span> <span class="token function">exampleFunction</span><span class="token punctuation">(</span>arg<span class="token punctuation">)</span> <span class="token punctuation">{</span>
    <span class="token comment">// Example comment</span>
    <span class="token keyword">return</span> arg <span class="token operator">+</span> <span class="token string">'dummyString'</span><span class="token punctuation">;</span>
<span class="token punctuation">}</span>

<span class="token comment">//#region utility methods</span>
<span class="token keyword">function</span> <span class="token function">add</span><span class="token punctuation">(</span>a<span class="token punctuation">,</span> b<span class="token punctuation">)</span> <span class="token punctuation">{</span>
    <span class="token keyword">return</span> a <span class="token operator">+</span> b<span class="token punctuation">;</span>
<span class="token punctuation">}</span>
<span class="token comment">//#endregion utility methods</span></code></pre>
</div>
````````````````````````````````

FlexiIncludeBlocks can be nested:
```````````````````````````````` none
--------------- Markdown ---------------
+{
    "type": "Markdown",
    "sourceUri": "https://raw.githubusercontent.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/390395942467555e47ad3cc575d1c8ebbceead15/test/FlexiBlocks.Tests/exampleIncludeWithNestedInclude.md"
}
--------------- Expected Markup ---------------
<p>This is example markdown with an include.</p>
<p>This is example markdown.</p>
````````````````````````````````

FlexiIncludeBlocks can be used anywhere a typical block can be used. For example, in a list item:
```````````````````````````````` none
--------------- Markdown ---------------
- First item.
- Second item  

  +{
      "type": "Markdown",
      "sourceUri": "./exampleInclude.md"
  }
- Third item
--------------- Expected Markup ---------------
<ul>
<li><p>First item.</p></li>
<li><p>Second item</p>
<p>This is example markdown.</p></li>
<li><p>Third item</p></li>
</ul>
````````````````````````````````

Or a blockquote:
```````````````````````````````` none
--------------- Markdown ---------------
> First line.
> +{
>     "type": "Markdown",
>     "sourceUri": "./exampleInclude.md"
> }
> Third line
--------------- Expected Markup ---------------
<blockquote>
<p>First line.</p>
<p>This is example markdown.</p>
<p>Third line</p>
</blockquote>
````````````````````````````````