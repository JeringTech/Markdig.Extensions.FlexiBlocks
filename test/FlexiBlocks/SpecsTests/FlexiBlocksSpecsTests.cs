using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.Specs
{
    public class IncludeBlocksSpecs
    {
        [Theory]
        [InlineData("IncludeBlocks")]
        [InlineData("All")]
        public void IncludeBlocks_Spec1(string extensions)
        {
            //     Start line number: 84
            //     --------------- Markdown ---------------
            //     +{ "type": "markdown", "source": "exampleInclude.md" }
            //     --------------- Expected Markup ---------------
            //     <p>This is example markdown.</p>
            //     <ul>
            //     <li>This is a list item.</li>
            //     </ul>
            //     <blockquote>
            //     <p>This is a blockquote.</p>
            //     </blockquote>

            SpecTestHelper.AssertCompliance("+{ \"type\": \"markdown\", \"source\": \"exampleInclude.md\" }",
                "<p>This is example markdown.</p>\n<ul>\n<li>This is a list item.</li>\n</ul>\n<blockquote>\n<p>This is a blockquote.</p>\n</blockquote>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("IncludeBlocks")]
        [InlineData("All")]
        public void IncludeBlocks_Spec2(string extensions)
        {
            //     Start line number: 99
            //     --------------- Markdown ---------------
            //     +{
            //         "type": "markdown",
            //         "source": "exampleInclude.md"    
            //     }
            //     --------------- Expected Markup ---------------
            //     <p>This is example markdown.</p>
            //     <ul>
            //     <li>This is a list item.</li>
            //     </ul>
            //     <blockquote>
            //     <p>This is a blockquote.</p>
            //     </blockquote>

            SpecTestHelper.AssertCompliance("+{\n    \"type\": \"markdown\",\n    \"source\": \"exampleInclude.md\"    \n}",
                "<p>This is example markdown.</p>\n<ul>\n<li>This is a list item.</li>\n</ul>\n<blockquote>\n<p>This is a blockquote.</p>\n</blockquote>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("IncludeBlocks")]
        [InlineData("All")]
        public void IncludeBlocks_Spec3(string extensions)
        {
            //     Start line number: 117
            //     --------------- Markdown ---------------
            //     + {
            //         "type": "markdown",
            //         "source": "exampleInclude.md"    
            //     }
            //     --------------- Expected Markup ---------------
            //     <ul>
            //     <li>{
            //     &quot;type&quot;: &quot;markdown&quot;,
            //     &quot;source&quot;: &quot;exampleInclude.md&quot;<br />
            //     }</li>
            //     </ul>

            SpecTestHelper.AssertCompliance("+ {\n    \"type\": \"markdown\",\n    \"source\": \"exampleInclude.md\"    \n}",
                "<ul>\n<li>{\n&quot;type&quot;: &quot;markdown&quot;,\n&quot;source&quot;: &quot;exampleInclude.md&quot;<br />\n}</li>\n</ul>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("IncludeBlocks")]
        [InlineData("All")]
        public void IncludeBlocks_Spec4(string extensions)
        {
            //     Start line number: 157
            //     --------------- Markdown ---------------
            //     +{
            //         "type": "markdown",
            //         "source": "https://raw.githubusercontent.com/JeringTech/Markdig.Extensions.FlexiBlocks/bb51313054e8d93ada0c1e779fb4db6eac9bb6f1/test/FlexiBlocks/exampleInclude.md"
            //     }
            //     --------------- Expected Markup ---------------
            //     <p>This is example markdown.</p>
            //     <ul>
            //     <li>This is a list item.</li>
            //     </ul>
            //     <blockquote>
            //     <p>This is a blockquote.</p>
            //     </blockquote>

            SpecTestHelper.AssertCompliance("+{\n    \"type\": \"markdown\",\n    \"source\": \"https://raw.githubusercontent.com/JeringTech/Markdig.Extensions.FlexiBlocks/bb51313054e8d93ada0c1e779fb4db6eac9bb6f1/test/FlexiBlocks/exampleInclude.md\"\n}",
                "<p>This is example markdown.</p>\n<ul>\n<li>This is a list item.</li>\n</ul>\n<blockquote>\n<p>This is a blockquote.</p>\n</blockquote>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("IncludeBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void IncludeBlocks_Spec5(string extensions)
        {
            //     Start line number: 180
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     +{
            //         "source": "exampleInclude.js",
            //         "clippings":[{"end": 4}, {"start": 7, "end": -2}]
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">function exampleFunction(arg) {
            //         // Example comment
            //         return arg + 'dummyString';
            //     }
            //     function add(a, b) {
            //         return a + b;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("+{\n    \"source\": \"exampleInclude.js\",\n    \"clippings\":[{\"end\": 4}, {\"start\": 7, \"end\": -2}]\n}",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">function exampleFunction(arg) {\n    // Example comment\n    return arg + 'dummyString';\n}\nfunction add(a, b) {\n    return a + b;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("IncludeBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void IncludeBlocks_Spec6(string extensions)
        {
            //     Start line number: 208
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     +{
            //         "source": "exampleInclude.js",
            //         "clippings":[{"region": "utility methods"}]
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">function add(a, b) {
            //         return a + b;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("+{\n    \"source\": \"exampleInclude.js\",\n    \"clippings\":[{\"region\": \"utility methods\"}]\n}",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">function add(a, b) {\n    return a + b;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("IncludeBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void IncludeBlocks_Spec7(string extensions)
        {
            //     Start line number: 232
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     +{
            //         "source": "exampleInclude.js",
            //         "clippings":[{"startString": "#region utility methods", "endString": "#endregion utility methods"}]
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">function add(a, b) {
            //         return a + b;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("+{\n    \"source\": \"exampleInclude.js\",\n    \"clippings\":[{\"startString\": \"#region utility methods\", \"endString\": \"#endregion utility methods\"}]\n}",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">function add(a, b) {\n    return a + b;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("IncludeBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void IncludeBlocks_Spec8(string extensions)
        {
            //     Start line number: 256
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     +{
            //         "source": "exampleInclude.js",
            //         "clippings":[{"start": 7, "endString": "#endregion utility methods"}]
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">function add(a, b) {
            //         return a + b;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("+{\n    \"source\": \"exampleInclude.js\",\n    \"clippings\":[{\"start\": 7, \"endString\": \"#endregion utility methods\"}]\n}",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">function add(a, b) {\n    return a + b;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("IncludeBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void IncludeBlocks_Spec9(string extensions)
        {
            //     Start line number: 280
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     +{
            //         "source": "exampleInclude.js",
            //         "clippings":[{
            //             "end": 1,
            //             "after": "..."
            //         },
            //         {
            //             "start": 4,
            //             "end": 4
            //         },
            //         {
            //             "start": 7, 
            //             "end": 7,
            //             "before": ""
            //         },
            //         {
            //             "start": 9, 
            //             "end": 9,
            //             "before": "..."
            //         }]
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">function exampleFunction(arg) {
            //     ...
            //     }
            //     
            //     function add(a, b) {
            //     ...
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("+{\n    \"source\": \"exampleInclude.js\",\n    \"clippings\":[{\n        \"end\": 1,\n        \"after\": \"...\"\n    },\n    {\n        \"start\": 4,\n        \"end\": 4\n    },\n    {\n        \"start\": 7, \n        \"end\": 7,\n        \"before\": \"\"\n    },\n    {\n        \"start\": 9, \n        \"end\": 9,\n        \"before\": \"...\"\n    }]\n}",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">function exampleFunction(arg) {\n...\n}\n\nfunction add(a, b) {\n...\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("IncludeBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void IncludeBlocks_Spec10(string extensions)
        {
            //     Start line number: 325
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     +{
            //         "source": "exampleInclude.js",
            //         "clippings":[{"dedent": 2}],
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">function exampleFunction(arg) {
            //       // Example comment
            //       return arg + 'dummyString';
            //     }
            //     
            //     //#region utility methods
            //     function add(a, b) {
            //       return a + b;
            //     }
            //     //#endregion utility methods
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("+{\n    \"source\": \"exampleInclude.js\",\n    \"clippings\":[{\"dedent\": 2}],\n}",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">function exampleFunction(arg) {\n  // Example comment\n  return arg + 'dummyString';\n}\n\n//#region utility methods\nfunction add(a, b) {\n  return a + b;\n}\n//#endregion utility methods\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("IncludeBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void IncludeBlocks_Spec11(string extensions)
        {
            //     Start line number: 356
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     +{
            //         "source": "exampleInclude.js",
            //         "clippings":[{"indent": 2}],
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">  function exampleFunction(arg) {
            //           // Example comment
            //           return arg + 'dummyString';
            //       }
            //     
            //       //#region utility methods
            //       function add(a, b) {
            //           return a + b;
            //       }
            //       //#endregion utility methods
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("+{\n    \"source\": \"exampleInclude.js\",\n    \"clippings\":[{\"indent\": 2}],\n}",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">  function exampleFunction(arg) {\n      // Example comment\n      return arg + 'dummyString';\n  }\n\n  //#region utility methods\n  function add(a, b) {\n      return a + b;\n  }\n  //#endregion utility methods\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("IncludeBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void IncludeBlocks_Spec12(string extensions)
        {
            //     Start line number: 387
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     +{
            //         "source": "exampleInclude.js",
            //         "clippings":[{"collapse": 0.5}]
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">function exampleFunction(arg) {
            //       // Example comment
            //       return arg + 'dummyString';
            //     }
            //     
            //     //#region utility methods
            //     function add(a, b) {
            //       return a + b;
            //     }
            //     //#endregion utility methods
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("+{\n    \"source\": \"exampleInclude.js\",\n    \"clippings\":[{\"collapse\": 0.5}]\n}",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">function exampleFunction(arg) {\n  // Example comment\n  return arg + 'dummyString';\n}\n\n//#region utility methods\nfunction add(a, b) {\n  return a + b;\n}\n//#endregion utility methods\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("IncludeBlocks")]
        [InlineData("All")]
        public void IncludeBlocks_Spec13(string extensions)
        {
            //     Start line number: 424
            //     --------------- Markdown ---------------
            //     +{
            //         "type": "markdown",
            //         "source": "exampleInclude.md"
            //     }
            //     --------------- Expected Markup ---------------
            //     <p>This is example markdown.</p>
            //     <ul>
            //     <li>This is a list item.</li>
            //     </ul>
            //     <blockquote>
            //     <p>This is a blockquote.</p>
            //     </blockquote>

            SpecTestHelper.AssertCompliance("+{\n    \"type\": \"markdown\",\n    \"source\": \"exampleInclude.md\"\n}",
                "<p>This is example markdown.</p>\n<ul>\n<li>This is a list item.</li>\n</ul>\n<blockquote>\n<p>This is a blockquote.</p>\n</blockquote>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("IncludeBlocks")]
        [InlineData("All")]
        public void IncludeBlocks_Spec14(string extensions)
        {
            //     Start line number: 455
            //     --------------- Markdown ---------------
            //     +{
            //         "cacheOnDisk": false,
            //         "type": "markdown",
            //         "source": "https://raw.githubusercontent.com/JeringTech/Markdig.Extensions.FlexiBlocks/6998b1c27821d8393ad39beb54f782515c39d98b/test/FlexiBlocks.Tests/exampleInclude.md"
            //     }
            //     --------------- Expected Markup ---------------
            //     <p>This is example markdown.</p>

            SpecTestHelper.AssertCompliance("+{\n    \"cacheOnDisk\": false,\n    \"type\": \"markdown\",\n    \"source\": \"https://raw.githubusercontent.com/JeringTech/Markdig.Extensions.FlexiBlocks/6998b1c27821d8393ad39beb54f782515c39d98b/test/FlexiBlocks.Tests/exampleInclude.md\"\n}",
                "<p>This is example markdown.</p>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("IncludeBlocks")]
        [InlineData("All")]
        public void IncludeBlocks_Spec15(string extensions)
        {
            //     Start line number: 554
            //     --------------- Extension Options ---------------
            //     {
            //         "includeBlocks": {
            //             "defaultBlockOptions": {
            //                 "type": "markdown"
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     +{
            //         "source": "exampleInclude.md"
            //     }
            //     
            //     +{
            //         "source": "exampleInclude.md"
            //     }
            //     --------------- Expected Markup ---------------
            //     <p>This is example markdown.</p>
            //     <ul>
            //     <li>This is a list item.</li>
            //     </ul>
            //     <blockquote>
            //     <p>This is a blockquote.</p>
            //     </blockquote>
            //     <p>This is example markdown.</p>
            //     <ul>
            //     <li>This is a list item.</li>
            //     </ul>
            //     <blockquote>
            //     <p>This is a blockquote.</p>
            //     </blockquote>

            SpecTestHelper.AssertCompliance("+{\n    \"source\": \"exampleInclude.md\"\n}\n\n+{\n    \"source\": \"exampleInclude.md\"\n}",
                "<p>This is example markdown.</p>\n<ul>\n<li>This is a list item.</li>\n</ul>\n<blockquote>\n<p>This is a blockquote.</p>\n</blockquote>\n<p>This is example markdown.</p>\n<ul>\n<li>This is a list item.</li>\n</ul>\n<blockquote>\n<p>This is a blockquote.</p>\n</blockquote>",
                extensions,
                false,
                "{\n    \"includeBlocks\": {\n        \"defaultBlockOptions\": {\n            \"type\": \"markdown\"\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("IncludeBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void IncludeBlocks_Spec16(string extensions)
        {
            //     Start line number: 589
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Extension Options ---------------
            //     {
            //         "includeBlocks": {
            //             "defaultBlockOptions": {
            //                 "type": "markdown"
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     +{
            //         "source": "exampleInclude.md"
            //     }
            //     
            //     +{
            //         "type": "code",
            //         "source": "exampleInclude.md"
            //     }
            //     --------------- Expected Markup ---------------
            //     <p>This is example markdown.</p>
            //     <ul>
            //     <li>This is a list item.</li>
            //     </ul>
            //     <blockquote>
            //     <p>This is a blockquote.</p>
            //     </blockquote>
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">This is example markdown.
            //     - This is a list item.
            //     &gt; This is a blockquote.
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("+{\n    \"source\": \"exampleInclude.md\"\n}\n\n+{\n    \"type\": \"code\",\n    \"source\": \"exampleInclude.md\"\n}",
                "<p>This is example markdown.</p>\n<ul>\n<li>This is a list item.</li>\n</ul>\n<blockquote>\n<p>This is a blockquote.</p>\n</blockquote>\n<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">This is example markdown.\n- This is a list item.\n&gt; This is a blockquote.\n</code></pre>\n</div>",
                extensions,
                false,
                "{\n    \"includeBlocks\": {\n        \"defaultBlockOptions\": {\n            \"type\": \"markdown\"\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("IncludeBlocks")]
        [InlineData("All")]
        public void IncludeBlocks_Spec17(string extensions)
        {
            //     Start line number: 638
            //     --------------- Extension Options ---------------
            //     {
            //         "includeBlocks": {
            //             "baseUri": "https://raw.githubusercontent.com"
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     +{
            //         "type": "markdown",
            //         "source": "JeremyTCD/Markdig.Extensions.FlexiBlocks/390395942467555e47ad3cc575d1c8ebbceead15/test/FlexiBlocks.Tests/exampleInclude.md"
            //     }
            //     --------------- Expected Markup ---------------
            //     <p>This is example markdown.</p>

            SpecTestHelper.AssertCompliance("+{\n    \"type\": \"markdown\",\n    \"source\": \"JeremyTCD/Markdig.Extensions.FlexiBlocks/390395942467555e47ad3cc575d1c8ebbceead15/test/FlexiBlocks.Tests/exampleInclude.md\"\n}",
                "<p>This is example markdown.</p>",
                extensions,
                false,
                "{\n    \"includeBlocks\": {\n        \"baseUri\": \"https://raw.githubusercontent.com\"\n    }\n}");
        }

        [Theory]
        [InlineData("IncludeBlocks_OptionsBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void IncludeBlocks_Spec18(string extensions)
        {
            //     Start line number: 660
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "language": "javascript"
            //     }
            //     +{
            //         "source": "exampleInclude.js"
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_language-javascript flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="token keyword">function</span> <span class="token function">exampleFunction</span><span class="token punctuation">(</span>arg<span class="token punctuation">)</span> <span class="token punctuation">{</span>
            //         <span class="token comment">// Example comment</span>
            //         <span class="token keyword">return</span> arg <span class="token operator">+</span> <span class="token string">'dummyString'</span><span class="token punctuation">;</span>
            //     <span class="token punctuation">}</span>
            //     
            //     <span class="token comment">//#region utility methods</span>
            //     <span class="token keyword">function</span> <span class="token function">add</span><span class="token punctuation">(</span>a<span class="token punctuation">,</span> b<span class="token punctuation">)</span> <span class="token punctuation">{</span>
            //         <span class="token keyword">return</span> a <span class="token operator">+</span> b<span class="token punctuation">;</span>
            //     <span class="token punctuation">}</span>
            //     <span class="token comment">//#endregion utility methods</span>
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"language\": \"javascript\"\n}\n+{\n    \"source\": \"exampleInclude.js\"\n}",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_language-javascript flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\"><span class=\"token keyword\">function</span> <span class=\"token function\">exampleFunction</span><span class=\"token punctuation\">(</span>arg<span class=\"token punctuation\">)</span> <span class=\"token punctuation\">{</span>\n    <span class=\"token comment\">// Example comment</span>\n    <span class=\"token keyword\">return</span> arg <span class=\"token operator\">+</span> <span class=\"token string\">'dummyString'</span><span class=\"token punctuation\">;</span>\n<span class=\"token punctuation\">}</span>\n\n<span class=\"token comment\">//#region utility methods</span>\n<span class=\"token keyword\">function</span> <span class=\"token function\">add</span><span class=\"token punctuation\">(</span>a<span class=\"token punctuation\">,</span> b<span class=\"token punctuation\">)</span> <span class=\"token punctuation\">{</span>\n    <span class=\"token keyword\">return</span> a <span class=\"token operator\">+</span> b<span class=\"token punctuation\">;</span>\n<span class=\"token punctuation\">}</span>\n<span class=\"token comment\">//#endregion utility methods</span>\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("IncludeBlocks")]
        [InlineData("All")]
        public void IncludeBlocks_Spec19(string extensions)
        {
            //     Start line number: 695
            //     --------------- Markdown ---------------
            //     +{
            //         "type": "markdown",
            //         "source": "https://raw.githubusercontent.com/JeringTech/Markdig.Extensions.FlexiBlocks/bb51313054e8d93ada0c1e779fb4db6eac9bb6f1/test/FlexiBlocks/exampleIncludeWithNestedInclude.md"
            //     }
            //     --------------- Expected Markup ---------------
            //     <p>This is example markdown with an include.</p>
            //     <p>This is example markdown.</p>
            //     <ul>
            //     <li>This is a list item.</li>
            //     </ul>
            //     <blockquote>
            //     <p>This is a blockquote.</p>
            //     </blockquote>

            SpecTestHelper.AssertCompliance("+{\n    \"type\": \"markdown\",\n    \"source\": \"https://raw.githubusercontent.com/JeringTech/Markdig.Extensions.FlexiBlocks/bb51313054e8d93ada0c1e779fb4db6eac9bb6f1/test/FlexiBlocks/exampleIncludeWithNestedInclude.md\"\n}",
                "<p>This is example markdown with an include.</p>\n<p>This is example markdown.</p>\n<ul>\n<li>This is a list item.</li>\n</ul>\n<blockquote>\n<p>This is a blockquote.</p>\n</blockquote>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("IncludeBlocks")]
        [InlineData("All")]
        public void IncludeBlocks_Spec20(string extensions)
        {
            //     Start line number: 713
            //     --------------- Markdown ---------------
            //     - First item.
            //     - Second item  
            //     
            //       +{
            //           "type": "markdown",
            //           "source": "exampleInclude.md"
            //       }
            //     - Third item
            //     --------------- Expected Markup ---------------
            //     <ul>
            //     <li><p>First item.</p></li>
            //     <li><p>Second item</p>
            //     <p>This is example markdown.</p>
            //     <ul>
            //     <li>This is a list item.</li>
            //     </ul>
            //     <blockquote>
            //     <p>This is a blockquote.</p>
            //     </blockquote></li>
            //     <li><p>Third item</p></li>
            //     </ul>

            SpecTestHelper.AssertCompliance("- First item.\n- Second item  \n\n  +{\n      \"type\": \"markdown\",\n      \"source\": \"exampleInclude.md\"\n  }\n- Third item",
                "<ul>\n<li><p>First item.</p></li>\n<li><p>Second item</p>\n<p>This is example markdown.</p>\n<ul>\n<li>This is a list item.</li>\n</ul>\n<blockquote>\n<p>This is a blockquote.</p>\n</blockquote></li>\n<li><p>Third item</p></li>\n</ul>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("IncludeBlocks")]
        [InlineData("All")]
        public void IncludeBlocks_Spec21(string extensions)
        {
            //     Start line number: 739
            //     --------------- Markdown ---------------
            //     > First line.
            //     > +{
            //     >     "type": "markdown",
            //     >     "source": "exampleInclude.md"
            //     > }
            //     > Third line
            //     --------------- Expected Markup ---------------
            //     <blockquote>
            //     <p>First line.</p>
            //     <p>This is example markdown.</p>
            //     <ul>
            //     <li>This is a list item.</li>
            //     </ul>
            //     <blockquote>
            //     <p>This is a blockquote.</p>
            //     </blockquote>
            //     <p>Third line</p>
            //     </blockquote>

            SpecTestHelper.AssertCompliance("> First line.\n> +{\n>     \"type\": \"markdown\",\n>     \"source\": \"exampleInclude.md\"\n> }\n> Third line",
                "<blockquote>\n<p>First line.</p>\n<p>This is example markdown.</p>\n<ul>\n<li>This is a list item.</li>\n</ul>\n<blockquote>\n<p>This is a blockquote.</p>\n</blockquote>\n<p>Third line</p>\n</blockquote>",
                extensions,
                false);
        }
    }

    public class OptionsBlocksSpecs
    {
        [Theory]
        [InlineData("All")]
        public void OptionsBlocks_Spec1(string extensions)
        {
            //     Start line number: 34
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     @{ "title": "ExampleDocument.cs" }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_has-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title">ExampleDocument.cs</span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + &quot;dummyString&quot;;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{ \"title\": \"ExampleDocument.cs\" }\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div class=\"flexi-code flexi-code_has-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\">ExampleDocument.cs</span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">public string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + &quot;dummyString&quot;;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("All")]
        public void OptionsBlocks_Spec2(string extensions)
        {
            //     Start line number: 65
            //     --------------- Extra Extensions ---------------
            //     FlexiAlertBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "type": "warning"
            //     }
            //     ! This is a FlexiAlertBlock.
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-alert flexi-alert_type_warning flexi-alert_has-icon">
            //     <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>This is a FlexiAlertBlock.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"type\": \"warning\"\n}\n! This is a FlexiAlertBlock.",
                "<div class=\"flexi-alert flexi-alert_type_warning flexi-alert_has-icon\">\n<svg class=\"flexi-alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>This is a FlexiAlertBlock.</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("All")]
        public void OptionsBlocks_Spec3(string extensions)
        {
            //     Start line number: 85
            //     --------------- Extra Extensions ---------------
            //     FlexiTableBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "attributes": {
            //             "id" : "table-1"
            //         }
            //     }
            //     +---+---+
            //     | a | b |
            //     +===+===+
            //     | 0 | 1 |
            //     +---+---+
            //     | 2 | 3 |
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards" id="table-1">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     a
            //     </th>
            //     <th class="flexi-table__header">
            //     b
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     a
            //     </div>
            //     <div class="flexi-table__content">
            //     0
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     b
            //     </div>
            //     <div class="flexi-table__content">
            //     1
            //     </div>
            //     </td>
            //     </tr>
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     a
            //     </div>
            //     <div class="flexi-table__content">
            //     2
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     b
            //     </div>
            //     <div class="flexi-table__content">
            //     3
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"attributes\": {\n        \"id\" : \"table-1\"\n    }\n}\n+---+---+\n| a | b |\n+===+===+\n| 0 | 1 |\n+---+---+\n| 2 | 3 |",
                "<div class=\"flexi-table flexi-table_type_cards\" id=\"table-1\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\na\n</th>\n<th class=\"flexi-table__header\">\nb\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\na\n</div>\n<div class=\"flexi-table__content\">\n0\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nb\n</div>\n<div class=\"flexi-table__content\">\n1\n</div>\n</td>\n</tr>\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\na\n</div>\n<div class=\"flexi-table__content\">\n2\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nb\n</div>\n<div class=\"flexi-table__content\">\n3\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("All")]
        public void OptionsBlocks_Spec4(string extensions)
        {
            //     Start line number: 166
            //     --------------- Extra Extensions ---------------
            //     FlexiSectionBlocks
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiSectionBlocks": {
            //             "defaultBlockOptions": {
            //                 "element": "nav"
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     # foo
            //     
            //     @{
            //         "element": "article"
            //     }
            //     # foo
            //     --------------- Expected Markup ---------------
            //     <nav class="flexi-section flexi-section_level_1 flexi-section_has-link-icon" id="foo">
            //     <header class="flexi-section__header">
            //     <h1 class="flexi-section__heading">foo</h1>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     </nav>
            //     <article class="flexi-section flexi-section_level_1 flexi-section_has-link-icon" id="foo-1">
            //     <header class="flexi-section__header">
            //     <h1 class="flexi-section__heading">foo</h1>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     </article>

            SpecTestHelper.AssertCompliance("# foo\n\n@{\n    \"element\": \"article\"\n}\n# foo",
                "<nav class=\"flexi-section flexi-section_level_1 flexi-section_has-link-icon\" id=\"foo\">\n<header class=\"flexi-section__header\">\n<h1 class=\"flexi-section__heading\">foo</h1>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n</nav>\n<article class=\"flexi-section flexi-section_level_1 flexi-section_has-link-icon\" id=\"foo-1\">\n<header class=\"flexi-section__header\">\n<h1 class=\"flexi-section__heading\">foo</h1>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n</article>",
                extensions,
                false,
                "{\n    \"flexiSectionBlocks\": {\n        \"defaultBlockOptions\": {\n            \"element\": \"nav\"\n        }\n    }\n}");
        }
    }

    public class FlexiAlertBlocksSpecs
    {
        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec1(string extensions)
        {
            //     Start line number: 37
            //     --------------- Markdown ---------------
            //     ! This is a FlexiAlertBlock.
            //     ! This is important information.
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon">
            //     <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>This is a FlexiAlertBlock.
            //     This is important information.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! This is a FlexiAlertBlock.\n! This is important information.",
                "<div class=\"flexi-alert flexi-alert_type_info flexi-alert_has-icon\">\n<svg class=\"flexi-alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>This is a FlexiAlertBlock.\nThis is important information.</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec2(string extensions)
        {
            //     Start line number: 58
            //     --------------- Markdown ---------------
            //     !This line will render identically to the next line.
            //     ! This line will render identically to the previous line.
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon">
            //     <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>This line will render identically to the next line.
            //     This line will render identically to the previous line.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("!This line will render identically to the next line.\n! This line will render identically to the previous line.",
                "<div class=\"flexi-alert flexi-alert_type_info flexi-alert_has-icon\">\n<svg class=\"flexi-alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>This line will render identically to the next line.\nThis line will render identically to the previous line.</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec3(string extensions)
        {
            //     Start line number: 74
            //     --------------- Markdown ---------------
            //     ! These lines belong to the same FlexiAlertBlock.
            //      ! These lines belong to the same FlexiAlertBlock.
            //       ! These lines belong to the same FlexiAlertBlock.
            //        ! These lines belong to the same FlexiAlertBlock.
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon">
            //     <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>These lines belong to the same FlexiAlertBlock.
            //     These lines belong to the same FlexiAlertBlock.
            //     These lines belong to the same FlexiAlertBlock.
            //     These lines belong to the same FlexiAlertBlock.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! These lines belong to the same FlexiAlertBlock.\n ! These lines belong to the same FlexiAlertBlock.\n  ! These lines belong to the same FlexiAlertBlock.\n   ! These lines belong to the same FlexiAlertBlock.",
                "<div class=\"flexi-alert flexi-alert_type_info flexi-alert_has-icon\">\n<svg class=\"flexi-alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>These lines belong to the same FlexiAlertBlock.\nThese lines belong to the same FlexiAlertBlock.\nThese lines belong to the same FlexiAlertBlock.\nThese lines belong to the same FlexiAlertBlock.</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec4(string extensions)
        {
            //     Start line number: 94
            //     --------------- Markdown ---------------
            //     ! This FlexiAlertBlock
            //     contains multiple
            //     lazy continuation lines.
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon">
            //     <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>This FlexiAlertBlock
            //     contains multiple
            //     lazy continuation lines.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! This FlexiAlertBlock\ncontains multiple\nlazy continuation lines.",
                "<div class=\"flexi-alert flexi-alert_type_info flexi-alert_has-icon\">\n<svg class=\"flexi-alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>This FlexiAlertBlock\ncontains multiple\nlazy continuation lines.</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec5(string extensions)
        {
            //     Start line number: 112
            //     --------------- Markdown ---------------
            //     ! This is a FlexiAlertBlock.
            //     
            //     ! This is another FlexiAlertBlock.
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon">
            //     <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>This is a FlexiAlertBlock.</p>
            //     </div>
            //     </div>
            //     <div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon">
            //     <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>This is another FlexiAlertBlock.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! This is a FlexiAlertBlock.\n\n! This is another FlexiAlertBlock.",
                "<div class=\"flexi-alert flexi-alert_type_info flexi-alert_has-icon\">\n<svg class=\"flexi-alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>This is a FlexiAlertBlock.</p>\n</div>\n</div>\n<div class=\"flexi-alert flexi-alert_type_info flexi-alert_has-icon\">\n<svg class=\"flexi-alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>This is another FlexiAlertBlock.</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec6(string extensions)
        {
            //     Start line number: 134
            //     --------------- Markdown ---------------
            //     ![This is an image](/url)
            //     
            //     ![This is neither an image nor a FlexiAlertBlock. Whether or not a line is a valid image, if it begins with `![`, it does not start a FlexiAlertBlock.
            //     --------------- Expected Markup ---------------
            //     <p><img src="/url" alt="This is an image" /></p>
            //     <p>![This is neither an image nor a FlexiAlertBlock. Whether or not a line is a valid image, if it begins with <code>![</code>, it does not start a FlexiAlertBlock.</p>

            SpecTestHelper.AssertCompliance("![This is an image](/url)\n\n![This is neither an image nor a FlexiAlertBlock. Whether or not a line is a valid image, if it begins with `![`, it does not start a FlexiAlertBlock.",
                "<p><img src=\"/url\" alt=\"This is an image\" /></p>\n<p>![This is neither an image nor a FlexiAlertBlock. Whether or not a line is a valid image, if it begins with <code>![</code>, it does not start a FlexiAlertBlock.</p>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec7(string extensions)
        {
            //     Start line number: 146
            //     --------------- Markdown ---------------
            //     ! [This is a FlexiAlertBlock]
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon">
            //     <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>[This is a FlexiAlertBlock]</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! [This is a FlexiAlertBlock]",
                "<div class=\"flexi-alert flexi-alert_type_info flexi-alert_has-icon\">\n<svg class=\"flexi-alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>[This is a FlexiAlertBlock]</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec8(string extensions)
        {
            //     Start line number: 160
            //     --------------- Markdown ---------------
            //     ! This is a FlexiAlertBlock
            //     ![This is an image in a FlexiAlertBlock](/url)
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon">
            //     <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>This is a FlexiAlertBlock
            //     <img src="/url" alt="This is an image in a FlexiAlertBlock" /></p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! This is a FlexiAlertBlock\n![This is an image in a FlexiAlertBlock](/url)",
                "<div class=\"flexi-alert flexi-alert_type_info flexi-alert_has-icon\">\n<svg class=\"flexi-alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>This is a FlexiAlertBlock\n<img src=\"/url\" alt=\"This is an image in a FlexiAlertBlock\" /></p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec9(string extensions)
        {
            //     Start line number: 188
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{ "blockName": "alert" }
            //     ! This is a FlexiAlertBlock.
            //     --------------- Expected Markup ---------------
            //     <div class="alert alert_type_info alert_has-icon">
            //     <svg class="alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
            //     <div class="alert__content">
            //     <p>This is a FlexiAlertBlock.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("@{ \"blockName\": \"alert\" }\n! This is a FlexiAlertBlock.",
                "<div class=\"alert alert_type_info alert_has-icon\">\n<svg class=\"alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"/></svg>\n<div class=\"alert__content\">\n<p>This is a FlexiAlertBlock.</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec10(string extensions)
        {
            //     Start line number: 215
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{ "type": "warning" }
            //     ! This is a FlexiAlertBlock.
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-alert flexi-alert_type_warning flexi-alert_has-icon">
            //     <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>This is a FlexiAlertBlock.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("@{ \"type\": \"warning\" }\n! This is a FlexiAlertBlock.",
                "<div class=\"flexi-alert flexi-alert_type_warning flexi-alert_has-icon\">\n<svg class=\"flexi-alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>This is a FlexiAlertBlock.</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec11(string extensions)
        {
            //     Start line number: 238
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{ "icon": "<svg><use xlink:href=\"#alert-icon\"></use></svg>" }
            //     ! This is a FlexiAlertBlock.
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon">
            //     <svg class="flexi-alert__icon"><use xlink:href="#alert-icon"></use></svg>
            //     <div class="flexi-alert__content">
            //     <p>This is a FlexiAlertBlock.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("@{ \"icon\": \"<svg><use xlink:href=\\\"#alert-icon\\\"></use></svg>\" }\n! This is a FlexiAlertBlock.",
                "<div class=\"flexi-alert flexi-alert_type_info flexi-alert_has-icon\">\n<svg class=\"flexi-alert__icon\"><use xlink:href=\"#alert-icon\"></use></svg>\n<div class=\"flexi-alert__content\">\n<p>This is a FlexiAlertBlock.</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec12(string extensions)
        {
            //     Start line number: 253
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "icon": null,
            //         "type": "no-default-icon"
            //     }
            //     ! This is a FlexiAlertBlock.
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-alert flexi-alert_type_no-default-icon flexi-alert_no-icon">
            //     <div class="flexi-alert__content">
            //     <p>This is a FlexiAlertBlock.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"icon\": null,\n    \"type\": \"no-default-icon\"\n}\n! This is a FlexiAlertBlock.",
                "<div class=\"flexi-alert flexi-alert_type_no-default-icon flexi-alert_no-icon\">\n<div class=\"flexi-alert__content\">\n<p>This is a FlexiAlertBlock.</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec13(string extensions)
        {
            //     Start line number: 278
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "attributes": {
            //             "id" : "my-custom-id",
            //             "class" : "my-custom-class"
            //         }
            //     }
            //     ! This is a FlexiAlertBlock.
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon my-custom-class" id="my-custom-id">
            //     <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>This is a FlexiAlertBlock.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"attributes\": {\n        \"id\" : \"my-custom-id\",\n        \"class\" : \"my-custom-class\"\n    }\n}\n! This is a FlexiAlertBlock.",
                "<div class=\"flexi-alert flexi-alert_type_info flexi-alert_has-icon my-custom-class\" id=\"my-custom-id\">\n<svg class=\"flexi-alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>This is a FlexiAlertBlock.</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec14(string extensions)
        {
            //     Start line number: 313
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiAlertBlocks": {
            //             "defaultBlockOptions": {
            //                 "icon": "<svg><use xlink:href=\"#alert-icon\"></use></svg>",
            //                 "attributes": {
            //                     "class": "block"
            //                 }
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     ! This is a FlexiAlertBlock.
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon block">
            //     <svg class="flexi-alert__icon"><use xlink:href="#alert-icon"></use></svg>
            //     <div class="flexi-alert__content">
            //     <p>This is a FlexiAlertBlock.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! This is a FlexiAlertBlock.",
                "<div class=\"flexi-alert flexi-alert_type_info flexi-alert_has-icon block\">\n<svg class=\"flexi-alert__icon\"><use xlink:href=\"#alert-icon\"></use></svg>\n<div class=\"flexi-alert__content\">\n<p>This is a FlexiAlertBlock.</p>\n</div>\n</div>",
                extensions,
                false,
                "{\n    \"flexiAlertBlocks\": {\n        \"defaultBlockOptions\": {\n            \"icon\": \"<svg><use xlink:href=\\\"#alert-icon\\\"></use></svg>\",\n            \"attributes\": {\n                \"class\": \"block\"\n            }\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiAlertBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec15(string extensions)
        {
            //     Start line number: 336
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiAlertBlocks": {
            //             "defaultBlockOptions": {
            //                 "blockName": "alert"
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     ! This is a FlexiAlertBlock
            //     
            //     @{
            //         "blockName": "special-alert"
            //     }
            //     ! This is a FlexiAlertBlock with block specific options.
            //     --------------- Expected Markup ---------------
            //     <div class="alert alert_type_info alert_has-icon">
            //     <svg class="alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
            //     <div class="alert__content">
            //     <p>This is a FlexiAlertBlock</p>
            //     </div>
            //     </div>
            //     <div class="special-alert special-alert_type_info special-alert_has-icon">
            //     <svg class="special-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
            //     <div class="special-alert__content">
            //     <p>This is a FlexiAlertBlock with block specific options.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! This is a FlexiAlertBlock\n\n@{\n    \"blockName\": \"special-alert\"\n}\n! This is a FlexiAlertBlock with block specific options.",
                "<div class=\"alert alert_type_info alert_has-icon\">\n<svg class=\"alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"/></svg>\n<div class=\"alert__content\">\n<p>This is a FlexiAlertBlock</p>\n</div>\n</div>\n<div class=\"special-alert special-alert_type_info special-alert_has-icon\">\n<svg class=\"special-alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"/></svg>\n<div class=\"special-alert__content\">\n<p>This is a FlexiAlertBlock with block specific options.</p>\n</div>\n</div>",
                extensions,
                false,
                "{\n    \"flexiAlertBlocks\": {\n        \"defaultBlockOptions\": {\n            \"blockName\": \"alert\"\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiAlertBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec16(string extensions)
        {
            //     Start line number: 375
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiAlertBlocks": {
            //             "icons": {
            //                 "closer-look": "<svg height=\"24\" viewBox=\"0 0 24 24\" width=\"24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M15.5 14h-.79l-.28-.27C15.41 12.59 16 11.11 16 9.5 16 5.91 13.09 3 9.5 3S3 5.91 3 9.5 5.91 16 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z\"/></svg>",
            //                 "help": "<svg width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 17h-2v-2h2v2zm2.07-7.75l-.9.92C13.45 12.9 13 13.5 13 15h-2v-.5c0-1.1.45-2.1 1.17-2.83l1.24-1.26c.37-.36.59-.86.59-1.41 0-1.1-.9-2-2-2s-2 .9-2 2H8c0-2.21 1.79-4 4-4s4 1.79 4 4c0 .88-.36 1.68-.93 2.25z\"/></svg>"
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     @{ "type": "closer-look" }
            //     ! This is a closer look at some topic.
            //     
            //     @{ "type": "help" }
            //     ! This is a helpful tip.
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-alert flexi-alert_type_closer-look flexi-alert_has-icon">
            //     <svg class="flexi-alert__icon" height="24" viewBox="0 0 24 24" width="24" xmlns="http://www.w3.org/2000/svg"><path d="M15.5 14h-.79l-.28-.27C15.41 12.59 16 11.11 16 9.5 16 5.91 13.09 3 9.5 3S3 5.91 3 9.5 5.91 16 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>This is a closer look at some topic.</p>
            //     </div>
            //     </div>
            //     <div class="flexi-alert flexi-alert_type_help flexi-alert_has-icon">
            //     <svg class="flexi-alert__icon" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 17h-2v-2h2v2zm2.07-7.75l-.9.92C13.45 12.9 13 13.5 13 15h-2v-.5c0-1.1.45-2.1 1.17-2.83l1.24-1.26c.37-.36.59-.86.59-1.41 0-1.1-.9-2-2-2s-2 .9-2 2H8c0-2.21 1.79-4 4-4s4 1.79 4 4c0 .88-.36 1.68-.93 2.25z"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>This is a helpful tip.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("@{ \"type\": \"closer-look\" }\n! This is a closer look at some topic.\n\n@{ \"type\": \"help\" }\n! This is a helpful tip.",
                "<div class=\"flexi-alert flexi-alert_type_closer-look flexi-alert_has-icon\">\n<svg class=\"flexi-alert__icon\" height=\"24\" viewBox=\"0 0 24 24\" width=\"24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M15.5 14h-.79l-.28-.27C15.41 12.59 16 11.11 16 9.5 16 5.91 13.09 3 9.5 3S3 5.91 3 9.5 5.91 16 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>This is a closer look at some topic.</p>\n</div>\n</div>\n<div class=\"flexi-alert flexi-alert_type_help flexi-alert_has-icon\">\n<svg class=\"flexi-alert__icon\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 17h-2v-2h2v2zm2.07-7.75l-.9.92C13.45 12.9 13 13.5 13 15h-2v-.5c0-1.1.45-2.1 1.17-2.83l1.24-1.26c.37-.36.59-.86.59-1.41 0-1.1-.9-2-2-2s-2 .9-2 2H8c0-2.21 1.79-4 4-4s4 1.79 4 4c0 .88-.36 1.68-.93 2.25z\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>This is a helpful tip.</p>\n</div>\n</div>",
                extensions,
                false,
                "{\n    \"flexiAlertBlocks\": {\n        \"icons\": {\n            \"closer-look\": \"<svg height=\\\"24\\\" viewBox=\\\"0 0 24 24\\\" width=\\\"24\\\" xmlns=\\\"http://www.w3.org/2000/svg\\\"><path d=\\\"M15.5 14h-.79l-.28-.27C15.41 12.59 16 11.11 16 9.5 16 5.91 13.09 3 9.5 3S3 5.91 3 9.5 5.91 16 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z\\\"/></svg>\",\n            \"help\": \"<svg width=\\\"24\\\" height=\\\"24\\\" viewBox=\\\"0 0 24 24\\\"><path d=\\\"M0 0h24v24H0z\\\" fill=\\\"none\\\"/><path d=\\\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 17h-2v-2h2v2zm2.07-7.75l-.9.92C13.45 12.9 13 13.5 13 15h-2v-.5c0-1.1.45-2.1 1.17-2.83l1.24-1.26c.37-.36.59-.86.59-1.41 0-1.1-.9-2-2-2s-2 .9-2 2H8c0-2.21 1.79-4 4-4s4 1.79 4 4c0 .88-.36 1.68-.93 2.25z\\\"/></svg>\"\n        }\n    }\n}");
        }
    }

    public class FlexiCodeBlocksSpecs
    {
        [Theory]
        [InlineData("FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec1(string extensions)
        {
            //     Start line number: 55
            //     --------------- Markdown ---------------
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + &quot;dummyString&quot;;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">public string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + &quot;dummyString&quot;;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec2(string extensions)
        {
            //     Start line number: 88
            //     --------------- Markdown ---------------
            //     ~~~
            //     <html>
            //         <head>
            //             <title>Example Page</title>
            //         </head>
            //         <body>
            //             <p>Example content.</p>
            //         </body>
            //     </html>
            //     ~~~
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">&lt;html&gt;
            //         &lt;head&gt;
            //             &lt;title&gt;Example Page&lt;/title&gt;
            //         &lt;/head&gt;
            //         &lt;body&gt;
            //             &lt;p&gt;Example content.&lt;/p&gt;
            //         &lt;/body&gt;
            //     &lt;/html&gt;
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("~~~\n<html>\n    <head>\n        <title>Example Page</title>\n    </head>\n    <body>\n        <p>Example content.</p>\n    </body>\n</html>\n~~~",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">&lt;html&gt;\n    &lt;head&gt;\n        &lt;title&gt;Example Page&lt;/title&gt;\n    &lt;/head&gt;\n    &lt;body&gt;\n        &lt;p&gt;Example content.&lt;/p&gt;\n    &lt;/body&gt;\n&lt;/html&gt;\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec3(string extensions)
        {
            //     Start line number: 121
            //     --------------- Markdown ---------------
            //         public exampleFunction(arg: string): string {
            //             // Example comment
            //             return arg + "dummyString";
            //         }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">public exampleFunction(arg: string): string {
            //         // Example comment
            //         return arg + &quot;dummyString&quot;;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("    public exampleFunction(arg: string): string {\n        // Example comment\n        return arg + \"dummyString\";\n    }",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">public exampleFunction(arg: string): string {\n    // Example comment\n    return arg + &quot;dummyString&quot;;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec4(string extensions)
        {
            //     Start line number: 157
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "blockName": "code"
            //     }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="code code_no-title code_has-copy-icon code_no-syntax-highlights code_no-line-numbers code_has-omitted-lines-icon code_no-highlighted-lines code_no-highlighted-phrases">
            //     <header class="code__header">
            //     <span class="code__title"></span>
            //     <button class="code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="code__pre"><code class="code__code">public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + &quot;dummyString&quot;;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"blockName\": \"code\"\n}\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div class=\"code code_no-title code_has-copy-icon code_no-syntax-highlights code_no-line-numbers code_has-omitted-lines-icon code_no-highlighted-lines code_no-highlighted-phrases\">\n<header class=\"code__header\">\n<span class=\"code__title\"></span>\n<button class=\"code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"code__pre\"><code class=\"code__code\">public string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + &quot;dummyString&quot;;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec5(string extensions)
        {
            //     Start line number: 194
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{ "title" : "ExampleDocument.cs" }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_has-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title">ExampleDocument.cs</span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + &quot;dummyString&quot;;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{ \"title\" : \"ExampleDocument.cs\" }\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div class=\"flexi-code flexi-code_has-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\">ExampleDocument.cs</span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">public string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + &quot;dummyString&quot;;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec6(string extensions)
        {
            //     Start line number: 230
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{ "copyIcon": "<svg><use xlink:href=\"#material-design-copy\"></use></svg>" }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon"><use xlink:href="#material-design-copy"></use></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + &quot;dummyString&quot;;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{ \"copyIcon\": \"<svg><use xlink:href=\\\"#material-design-copy\\\"></use></svg>\" }\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\"><use xlink:href=\"#material-design-copy\"></use></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">public string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + &quot;dummyString&quot;;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec7(string extensions)
        {
            //     Start line number: 259
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{ "copyIcon": null }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_no-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + &quot;dummyString&quot;;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{ \"copyIcon\": null }\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_no-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">public string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + &quot;dummyString&quot;;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec8(string extensions)
        {
            //     Start line number: 298
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{ "language": "csharp" }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_language-csharp flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="token keyword">public</span> <span class="token keyword">string</span> <span class="token function">ExampleFunction</span><span class="token punctuation">(</span><span class="token keyword">string</span> arg<span class="token punctuation">)</span>
            //     <span class="token punctuation">{</span>
            //         <span class="token comment">// Example comment</span>
            //         <span class="token keyword">return</span> arg <span class="token operator">+</span> <span class="token string">"dummyString"</span><span class="token punctuation">;</span>
            //     <span class="token punctuation">}</span>
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{ \"language\": \"csharp\" }\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_language-csharp flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\"><span class=\"token keyword\">public</span> <span class=\"token keyword\">string</span> <span class=\"token function\">ExampleFunction</span><span class=\"token punctuation\">(</span><span class=\"token keyword\">string</span> arg<span class=\"token punctuation\">)</span>\n<span class=\"token punctuation\">{</span>\n    <span class=\"token comment\">// Example comment</span>\n    <span class=\"token keyword\">return</span> arg <span class=\"token operator\">+</span> <span class=\"token string\">\"dummyString\"</span><span class=\"token punctuation\">;</span>\n<span class=\"token punctuation\">}</span>\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec9(string extensions)
        {
            //     Start line number: 334
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "syntaxHighlighter": "highlightJS",
            //         "language": "typescript"
            //     }
            //     ```
            //     public exampleFunction(arg: string): string {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_language-typescript flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="hljs-keyword">public</span> exampleFunction(arg: <span class="hljs-built_in">string</span>): <span class="hljs-built_in">string</span> {
            //         <span class="hljs-comment">// Example comment</span>
            //         <span class="hljs-keyword">return</span> arg + <span class="hljs-string">"dummyString"</span>;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"syntaxHighlighter\": \"highlightJS\",\n    \"language\": \"typescript\"\n}\n```\npublic exampleFunction(arg: string): string {\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_language-typescript flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\"><span class=\"hljs-keyword\">public</span> exampleFunction(arg: <span class=\"hljs-built_in\">string</span>): <span class=\"hljs-built_in\">string</span> {\n    <span class=\"hljs-comment\">// Example comment</span>\n    <span class=\"hljs-keyword\">return</span> arg + <span class=\"hljs-string\">\"dummyString\"</span>;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec10(string extensions)
        {
            //     Start line number: 376
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "lineNumbers": [
            //             { "start": 2, "end": 8, "startNumber": 4 },
            //             { "start": 10, "end": -2, "startNumber": 32 }
            //         ]
            //     }
            //     ```
            //     
            //     public class ExampleClass
            //     {
            //         public string ExampleFunction1(string arg)
            //         {
            //             // Example comment
            //             return arg + "dummyString";
            //         }
            //     
            //         public string ExampleFunction3(string arg)
            //         {
            //             // Example comment
            //             return arg + "dummyString";
            //         }
            //     }
            //     
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_has-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="flexi-code__line-prefix"><svg class="flexi-code__omitted-lines-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 8c1.1 0 2-.9 2-2s-.9-2-2-2-2 .9-2 2 .9 2 2 2zm0 2c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2zm0 6c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2z"/></svg></span><span class="flexi-code__line flexi-code__line_omitted-lines">Lines 1 to 3 omitted for brevity</span>
            //     <span class="flexi-code__line-prefix">4</span><span class="flexi-code__line">public class ExampleClass</span>
            //     <span class="flexi-code__line-prefix">5</span><span class="flexi-code__line">{</span>
            //     <span class="flexi-code__line-prefix">6</span><span class="flexi-code__line">    public string ExampleFunction1(string arg)</span>
            //     <span class="flexi-code__line-prefix">7</span><span class="flexi-code__line">    {</span>
            //     <span class="flexi-code__line-prefix">8</span><span class="flexi-code__line">        // Example comment</span>
            //     <span class="flexi-code__line-prefix">9</span><span class="flexi-code__line">        return arg + &quot;dummyString&quot;;</span>
            //     <span class="flexi-code__line-prefix">10</span><span class="flexi-code__line">    }</span>
            //     <span class="flexi-code__line-prefix"><svg class="flexi-code__omitted-lines-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 8c1.1 0 2-.9 2-2s-.9-2-2-2-2 .9-2 2 .9 2 2 2zm0 2c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2zm0 6c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2z"/></svg></span><span class="flexi-code__line flexi-code__line_omitted-lines">Lines 11 to 31 omitted for brevity</span>
            //     <span class="flexi-code__line-prefix">32</span><span class="flexi-code__line">    public string ExampleFunction3(string arg)</span>
            //     <span class="flexi-code__line-prefix">33</span><span class="flexi-code__line">    {</span>
            //     <span class="flexi-code__line-prefix">34</span><span class="flexi-code__line">        // Example comment</span>
            //     <span class="flexi-code__line-prefix">35</span><span class="flexi-code__line">        return arg + &quot;dummyString&quot;;</span>
            //     <span class="flexi-code__line-prefix">36</span><span class="flexi-code__line">    }</span>
            //     <span class="flexi-code__line-prefix">37</span><span class="flexi-code__line">}</span>
            //     <span class="flexi-code__line-prefix"><svg class="flexi-code__omitted-lines-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 8c1.1 0 2-.9 2-2s-.9-2-2-2-2 .9-2 2 .9 2 2 2zm0 2c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2zm0 6c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2z"/></svg></span><span class="flexi-code__line flexi-code__line_omitted-lines">Lines 38 to the end omitted for brevity</span>
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"lineNumbers\": [\n        { \"start\": 2, \"end\": 8, \"startNumber\": 4 },\n        { \"start\": 10, \"end\": -2, \"startNumber\": 32 }\n    ]\n}\n```\n\npublic class ExampleClass\n{\n    public string ExampleFunction1(string arg)\n    {\n        // Example comment\n        return arg + \"dummyString\";\n    }\n\n    public string ExampleFunction3(string arg)\n    {\n        // Example comment\n        return arg + \"dummyString\";\n    }\n}\n\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_has-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\"><span class=\"flexi-code__line-prefix\"><svg class=\"flexi-code__omitted-lines-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 8c1.1 0 2-.9 2-2s-.9-2-2-2-2 .9-2 2 .9 2 2 2zm0 2c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2zm0 6c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2z\"/></svg></span><span class=\"flexi-code__line flexi-code__line_omitted-lines\">Lines 1 to 3 omitted for brevity</span>\n<span class=\"flexi-code__line-prefix\">4</span><span class=\"flexi-code__line\">public class ExampleClass</span>\n<span class=\"flexi-code__line-prefix\">5</span><span class=\"flexi-code__line\">{</span>\n<span class=\"flexi-code__line-prefix\">6</span><span class=\"flexi-code__line\">    public string ExampleFunction1(string arg)</span>\n<span class=\"flexi-code__line-prefix\">7</span><span class=\"flexi-code__line\">    {</span>\n<span class=\"flexi-code__line-prefix\">8</span><span class=\"flexi-code__line\">        // Example comment</span>\n<span class=\"flexi-code__line-prefix\">9</span><span class=\"flexi-code__line\">        return arg + &quot;dummyString&quot;;</span>\n<span class=\"flexi-code__line-prefix\">10</span><span class=\"flexi-code__line\">    }</span>\n<span class=\"flexi-code__line-prefix\"><svg class=\"flexi-code__omitted-lines-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 8c1.1 0 2-.9 2-2s-.9-2-2-2-2 .9-2 2 .9 2 2 2zm0 2c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2zm0 6c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2z\"/></svg></span><span class=\"flexi-code__line flexi-code__line_omitted-lines\">Lines 11 to 31 omitted for brevity</span>\n<span class=\"flexi-code__line-prefix\">32</span><span class=\"flexi-code__line\">    public string ExampleFunction3(string arg)</span>\n<span class=\"flexi-code__line-prefix\">33</span><span class=\"flexi-code__line\">    {</span>\n<span class=\"flexi-code__line-prefix\">34</span><span class=\"flexi-code__line\">        // Example comment</span>\n<span class=\"flexi-code__line-prefix\">35</span><span class=\"flexi-code__line\">        return arg + &quot;dummyString&quot;;</span>\n<span class=\"flexi-code__line-prefix\">36</span><span class=\"flexi-code__line\">    }</span>\n<span class=\"flexi-code__line-prefix\">37</span><span class=\"flexi-code__line\">}</span>\n<span class=\"flexi-code__line-prefix\"><svg class=\"flexi-code__omitted-lines-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 8c1.1 0 2-.9 2-2s-.9-2-2-2-2 .9-2 2 .9 2 2 2zm0 2c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2zm0 6c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2z\"/></svg></span><span class=\"flexi-code__line flexi-code__line_omitted-lines\">Lines 38 to the end omitted for brevity</span>\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec11(string extensions)
        {
            //     Start line number: 439
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "omittedLinesIcon": "<svg><use xlink:href=\"#material-design-more-vert\"></use></svg>",
            //         "lineNumbers": [{"end": 2}, {"start": 4, "startNumber":10}]
            //     }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //     
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_has-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="flexi-code__line-prefix">1</span><span class="flexi-code__line">public string ExampleFunction(string arg)</span>
            //     <span class="flexi-code__line-prefix">2</span><span class="flexi-code__line">{</span>
            //     <span class="flexi-code__line-prefix"><svg class="flexi-code__omitted-lines-icon"><use xlink:href="#material-design-more-vert"></use></svg></span><span class="flexi-code__line flexi-code__line_omitted-lines">Lines 3 to 9 omitted for brevity</span>
            //     <span class="flexi-code__line-prefix">10</span><span class="flexi-code__line">}</span>
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"omittedLinesIcon\": \"<svg><use xlink:href=\\\"#material-design-more-vert\\\"></use></svg>\",\n    \"lineNumbers\": [{\"end\": 2}, {\"start\": 4, \"startNumber\":10}]\n}\n```\npublic string ExampleFunction(string arg)\n{\n\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_has-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\"><span class=\"flexi-code__line-prefix\">1</span><span class=\"flexi-code__line\">public string ExampleFunction(string arg)</span>\n<span class=\"flexi-code__line-prefix\">2</span><span class=\"flexi-code__line\">{</span>\n<span class=\"flexi-code__line-prefix\"><svg class=\"flexi-code__omitted-lines-icon\"><use xlink:href=\"#material-design-more-vert\"></use></svg></span><span class=\"flexi-code__line flexi-code__line_omitted-lines\">Lines 3 to 9 omitted for brevity</span>\n<span class=\"flexi-code__line-prefix\">10</span><span class=\"flexi-code__line\">}</span>\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec12(string extensions)
        {
            //     Start line number: 469
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "omittedLinesIcon": null,
            //         "lineNumbers": [{"end": 2}, {"start": 4, "startNumber":10}]
            //     }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //     
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_has-line-numbers flexi-code_no-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="flexi-code__line-prefix">1</span><span class="flexi-code__line">public string ExampleFunction(string arg)</span>
            //     <span class="flexi-code__line-prefix">2</span><span class="flexi-code__line">{</span>
            //     <span class="flexi-code__line-prefix"></span><span class="flexi-code__line flexi-code__line_omitted-lines">Lines 3 to 9 omitted for brevity</span>
            //     <span class="flexi-code__line-prefix">10</span><span class="flexi-code__line">}</span>
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"omittedLinesIcon\": null,\n    \"lineNumbers\": [{\"end\": 2}, {\"start\": 4, \"startNumber\":10}]\n}\n```\npublic string ExampleFunction(string arg)\n{\n\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_has-line-numbers flexi-code_no-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\"><span class=\"flexi-code__line-prefix\">1</span><span class=\"flexi-code__line\">public string ExampleFunction(string arg)</span>\n<span class=\"flexi-code__line-prefix\">2</span><span class=\"flexi-code__line\">{</span>\n<span class=\"flexi-code__line-prefix\"></span><span class=\"flexi-code__line flexi-code__line_omitted-lines\">Lines 3 to 9 omitted for brevity</span>\n<span class=\"flexi-code__line-prefix\">10</span><span class=\"flexi-code__line\">}</span>\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec13(string extensions)
        {
            //     Start line number: 505
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "highlightedLines": [
            //             { "end": 1 },
            //             { "start": 3, "end": 4 }
            //         ]
            //     }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_has-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="flexi-code__line flexi-code__line_highlighted">public string ExampleFunction(string arg)</span>
            //     {
            //     <span class="flexi-code__line flexi-code__line_highlighted">    // Example comment</span>
            //     <span class="flexi-code__line flexi-code__line_highlighted">    return arg + &quot;dummyString&quot;;</span>
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"highlightedLines\": [\n        { \"end\": 1 },\n        { \"start\": 3, \"end\": 4 }\n    ]\n}\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_has-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\"><span class=\"flexi-code__line flexi-code__line_highlighted\">public string ExampleFunction(string arg)</span>\n{\n<span class=\"flexi-code__line flexi-code__line_highlighted\">    // Example comment</span>\n<span class=\"flexi-code__line flexi-code__line_highlighted\">    return arg + &quot;dummyString&quot;;</span>\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec14(string extensions)
        {
            //     Start line number: 547
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "highlightedPhrases": [
            //             { "regex": "return (.*?);", "included": [1] },
            //             { "regex": "string arg" }
            //         ]
            //     }
            //     ```
            //     public class ExampleClass
            //     {
            //         public string ExampleFunction1(string arg)
            //         {
            //             // Example comment
            //             return arg + "dummyString";
            //         }
            //     
            //         public string ExampleFunction2(string arg)
            //         {
            //             // Example comment
            //             return arg + "dummyString";
            //         }
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_has-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">public class ExampleClass
            //     {
            //         public string ExampleFunction1(<span class="flexi-code__highlighted-phrase">string arg</span>)
            //         {
            //             // Example comment
            //             return arg + &quot;dummyString&quot;;
            //         }
            //     
            //         public string ExampleFunction2(<span class="flexi-code__highlighted-phrase">string arg</span>)
            //         {
            //             // Example comment
            //             return <span class="flexi-code__highlighted-phrase">arg + &quot;dummyString&quot;</span>;
            //         }
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"highlightedPhrases\": [\n        { \"regex\": \"return (.*?);\", \"included\": [1] },\n        { \"regex\": \"string arg\" }\n    ]\n}\n```\npublic class ExampleClass\n{\n    public string ExampleFunction1(string arg)\n    {\n        // Example comment\n        return arg + \"dummyString\";\n    }\n\n    public string ExampleFunction2(string arg)\n    {\n        // Example comment\n        return arg + \"dummyString\";\n    }\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_has-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">public class ExampleClass\n{\n    public string ExampleFunction1(<span class=\"flexi-code__highlighted-phrase\">string arg</span>)\n    {\n        // Example comment\n        return arg + &quot;dummyString&quot;;\n    }\n\n    public string ExampleFunction2(<span class=\"flexi-code__highlighted-phrase\">string arg</span>)\n    {\n        // Example comment\n        return <span class=\"flexi-code__highlighted-phrase\">arg + &quot;dummyString&quot;</span>;\n    }\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec15(string extensions)
        {
            //     Start line number: 605
            //     --------------- Markdown ---------------
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + &quot;dummyString&quot;;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">public string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + &quot;dummyString&quot;;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec16(string extensions)
        {
            //     Start line number: 632
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{ "renderingMode": "classic" }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <pre><code>public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + &quot;dummyString&quot;;
            //     }
            //     </code></pre>

            SpecTestHelper.AssertCompliance("@{ \"renderingMode\": \"classic\" }\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<pre><code>public string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + &quot;dummyString&quot;;\n}\n</code></pre>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec17(string extensions)
        {
            //     Start line number: 661
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "attributes": {
            //             "id" : "code-1",
            //             "class" : "block"
            //         }
            //     }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases block" id="code-1">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + &quot;dummyString&quot;;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"attributes\": {\n        \"id\" : \"code-1\",\n        \"class\" : \"block\"\n    }\n}\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases block\" id=\"code-1\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">public string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + &quot;dummyString&quot;;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec18(string extensions)
        {
            //     Start line number: 766
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiCodeBlocks": {
            //             "defaultBlockOptions": {
            //                 "blockName": "code",
            //                 "title": "ExampleDocument.cs",
            //                 "copyIcon": "<svg><use xlink:href=\"#material-design-copy\"></use></svg>",
            //                 "language": "html",
            //                 "syntaxHighlighter": "highlightjs",
            //                 "lineNumbers": [{}],
            //                 "omittedLinesIcon": "<svg><use xlink:href=\"#material-design-more-vert\"></use></svg>",
            //                 "highlightedLines": [{"start": 3, "end": 3}],
            //                 "highlightedPhrases": [{"regex":"</.*?>"}],
            //                 "attributes": {"class": "block"}
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     ```
            //     <html>
            //         <head>
            //             <title>Example Page</title>
            //         </head>
            //         <body>
            //             <p>Example content.</p>
            //         </body>
            //     </html>
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="code code_has-title code_has-copy-icon code_language-html code_has-syntax-highlights code_has-line-numbers code_has-omitted-lines-icon code_has-highlighted-lines code_has-highlighted-phrases block">
            //     <header class="code__header">
            //     <span class="code__title">ExampleDocument.cs</span>
            //     <button class="code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="code__copy-icon"><use xlink:href="#material-design-copy"></use></svg>
            //     </button>
            //     </header>
            //     <pre class="code__pre"><code class="code__code"><span class="code__line-prefix">1</span><span class="code__line"><span class="hljs-tag">&lt;<span class="hljs-name">html</span>&gt;</span></span>
            //     <span class="code__line-prefix">2</span><span class="code__line">    <span class="hljs-tag">&lt;<span class="hljs-name">head</span>&gt;</span></span>
            //     <span class="code__line-prefix">3</span><span class="code__line code__line_highlighted">        <span class="hljs-tag">&lt;<span class="hljs-name">title</span>&gt;</span>Example Page<span class="code__highlighted-phrase"><span class="hljs-tag">&lt;/<span class="hljs-name">title</span>&gt;</span></span></span>
            //     <span class="code__line-prefix">4</span><span class="code__line">    <span class="code__highlighted-phrase"><span class="hljs-tag">&lt;/<span class="hljs-name">head</span>&gt;</span></span></span>
            //     <span class="code__line-prefix">5</span><span class="code__line">    <span class="hljs-tag">&lt;<span class="hljs-name">body</span>&gt;</span></span>
            //     <span class="code__line-prefix">6</span><span class="code__line">        <span class="hljs-tag">&lt;<span class="hljs-name">p</span>&gt;</span>Example content.<span class="code__highlighted-phrase"><span class="hljs-tag">&lt;/<span class="hljs-name">p</span>&gt;</span></span></span>
            //     <span class="code__line-prefix">7</span><span class="code__line">    <span class="code__highlighted-phrase"><span class="hljs-tag">&lt;/<span class="hljs-name">body</span>&gt;</span></span></span>
            //     <span class="code__line-prefix">8</span><span class="code__line"><span class="code__highlighted-phrase"><span class="hljs-tag">&lt;/<span class="hljs-name">html</span>&gt;</span></span></span>
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("```\n<html>\n    <head>\n        <title>Example Page</title>\n    </head>\n    <body>\n        <p>Example content.</p>\n    </body>\n</html>\n```",
                "<div class=\"code code_has-title code_has-copy-icon code_language-html code_has-syntax-highlights code_has-line-numbers code_has-omitted-lines-icon code_has-highlighted-lines code_has-highlighted-phrases block\">\n<header class=\"code__header\">\n<span class=\"code__title\">ExampleDocument.cs</span>\n<button class=\"code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"code__copy-icon\"><use xlink:href=\"#material-design-copy\"></use></svg>\n</button>\n</header>\n<pre class=\"code__pre\"><code class=\"code__code\"><span class=\"code__line-prefix\">1</span><span class=\"code__line\"><span class=\"hljs-tag\">&lt;<span class=\"hljs-name\">html</span>&gt;</span></span>\n<span class=\"code__line-prefix\">2</span><span class=\"code__line\">    <span class=\"hljs-tag\">&lt;<span class=\"hljs-name\">head</span>&gt;</span></span>\n<span class=\"code__line-prefix\">3</span><span class=\"code__line code__line_highlighted\">        <span class=\"hljs-tag\">&lt;<span class=\"hljs-name\">title</span>&gt;</span>Example Page<span class=\"code__highlighted-phrase\"><span class=\"hljs-tag\">&lt;/<span class=\"hljs-name\">title</span>&gt;</span></span></span>\n<span class=\"code__line-prefix\">4</span><span class=\"code__line\">    <span class=\"code__highlighted-phrase\"><span class=\"hljs-tag\">&lt;/<span class=\"hljs-name\">head</span>&gt;</span></span></span>\n<span class=\"code__line-prefix\">5</span><span class=\"code__line\">    <span class=\"hljs-tag\">&lt;<span class=\"hljs-name\">body</span>&gt;</span></span>\n<span class=\"code__line-prefix\">6</span><span class=\"code__line\">        <span class=\"hljs-tag\">&lt;<span class=\"hljs-name\">p</span>&gt;</span>Example content.<span class=\"code__highlighted-phrase\"><span class=\"hljs-tag\">&lt;/<span class=\"hljs-name\">p</span>&gt;</span></span></span>\n<span class=\"code__line-prefix\">7</span><span class=\"code__line\">    <span class=\"code__highlighted-phrase\"><span class=\"hljs-tag\">&lt;/<span class=\"hljs-name\">body</span>&gt;</span></span></span>\n<span class=\"code__line-prefix\">8</span><span class=\"code__line\"><span class=\"code__highlighted-phrase\"><span class=\"hljs-tag\">&lt;/<span class=\"hljs-name\">html</span>&gt;</span></span></span>\n</code></pre>\n</div>",
                extensions,
                false,
                "{\n    \"flexiCodeBlocks\": {\n        \"defaultBlockOptions\": {\n            \"blockName\": \"code\",\n            \"title\": \"ExampleDocument.cs\",\n            \"copyIcon\": \"<svg><use xlink:href=\\\"#material-design-copy\\\"></use></svg>\",\n            \"language\": \"html\",\n            \"syntaxHighlighter\": \"highlightjs\",\n            \"lineNumbers\": [{}],\n            \"omittedLinesIcon\": \"<svg><use xlink:href=\\\"#material-design-more-vert\\\"></use></svg>\",\n            \"highlightedLines\": [{\"start\": 3, \"end\": 3}],\n            \"highlightedPhrases\": [{\"regex\":\"</.*?>\"}],\n            \"attributes\": {\"class\": \"block\"}\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec19(string extensions)
        {
            //     Start line number: 816
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiCodeBlocks": {
            //             "defaultBlockOptions": {
            //                 "lineNumbers": [{}]
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     
            //     @{
            //         "lineNumbers": [
            //             {
            //                 "start": 2, "startNumber": 25
            //             }
            //         ]
            //     }
            //     ```
            //     
            //     body {
            //         display: flex;
            //         align-items: center;
            //         font-size: 13px;
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_has-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="flexi-code__line-prefix">1</span><span class="flexi-code__line">public string ExampleFunction(string arg)</span>
            //     <span class="flexi-code__line-prefix">2</span><span class="flexi-code__line">{</span>
            //     <span class="flexi-code__line-prefix">3</span><span class="flexi-code__line">    // Example comment</span>
            //     <span class="flexi-code__line-prefix">4</span><span class="flexi-code__line">    return arg + &quot;dummyString&quot;;</span>
            //     <span class="flexi-code__line-prefix">5</span><span class="flexi-code__line">}</span>
            //     </code></pre>
            //     </div>
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_has-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="flexi-code__line-prefix"><svg class="flexi-code__omitted-lines-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 8c1.1 0 2-.9 2-2s-.9-2-2-2-2 .9-2 2 .9 2 2 2zm0 2c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2zm0 6c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2z"/></svg></span><span class="flexi-code__line flexi-code__line_omitted-lines">Lines 1 to 24 omitted for brevity</span>
            //     <span class="flexi-code__line-prefix">25</span><span class="flexi-code__line">body {</span>
            //     <span class="flexi-code__line-prefix">26</span><span class="flexi-code__line">    display: flex;</span>
            //     <span class="flexi-code__line-prefix">27</span><span class="flexi-code__line">    align-items: center;</span>
            //     <span class="flexi-code__line-prefix">28</span><span class="flexi-code__line">    font-size: 13px;</span>
            //     <span class="flexi-code__line-prefix">29</span><span class="flexi-code__line">}</span>
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```\n\n@{\n    \"lineNumbers\": [\n        {\n            \"start\": 2, \"startNumber\": 25\n        }\n    ]\n}\n```\n\nbody {\n    display: flex;\n    align-items: center;\n    font-size: 13px;\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_has-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\"><span class=\"flexi-code__line-prefix\">1</span><span class=\"flexi-code__line\">public string ExampleFunction(string arg)</span>\n<span class=\"flexi-code__line-prefix\">2</span><span class=\"flexi-code__line\">{</span>\n<span class=\"flexi-code__line-prefix\">3</span><span class=\"flexi-code__line\">    // Example comment</span>\n<span class=\"flexi-code__line-prefix\">4</span><span class=\"flexi-code__line\">    return arg + &quot;dummyString&quot;;</span>\n<span class=\"flexi-code__line-prefix\">5</span><span class=\"flexi-code__line\">}</span>\n</code></pre>\n</div>\n<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_has-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\"><span class=\"flexi-code__line-prefix\"><svg class=\"flexi-code__omitted-lines-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 8c1.1 0 2-.9 2-2s-.9-2-2-2-2 .9-2 2 .9 2 2 2zm0 2c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2zm0 6c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2z\"/></svg></span><span class=\"flexi-code__line flexi-code__line_omitted-lines\">Lines 1 to 24 omitted for brevity</span>\n<span class=\"flexi-code__line-prefix\">25</span><span class=\"flexi-code__line\">body {</span>\n<span class=\"flexi-code__line-prefix\">26</span><span class=\"flexi-code__line\">    display: flex;</span>\n<span class=\"flexi-code__line-prefix\">27</span><span class=\"flexi-code__line\">    align-items: center;</span>\n<span class=\"flexi-code__line-prefix\">28</span><span class=\"flexi-code__line\">    font-size: 13px;</span>\n<span class=\"flexi-code__line-prefix\">29</span><span class=\"flexi-code__line\">}</span>\n</code></pre>\n</div>",
                extensions,
                false,
                "{\n    \"flexiCodeBlocks\": {\n        \"defaultBlockOptions\": {\n            \"lineNumbers\": [{}]\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec20(string extensions)
        {
            //     Start line number: 887
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "language": "csharp",
            //         "highlightedLines": [
            //             { "start": 3, "end": 3 },
            //             { "start": 8, "end": 8 }
            //         ],
            //         "highlightedPhrases": [
            //             { "regex": "Multiline.*?1" },
            //             { "regex": "/.*?/", "included": [1] }
            //         ]
            //     }
            //     ```
            //     /* 
            //         Multiline
            //         comment
            //         1
            //     */
            //     /* 
            //         Multiline
            //         comment
            //         2
            //     */
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_language-csharp flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_has-highlighted-lines flexi-code_has-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="token comment">/* 
            //         <span class="flexi-code__highlighted-phrase">Multiline</span></span>
            //     <span class="flexi-code__line flexi-code__line_highlighted"><span class="token comment"><span class="flexi-code__highlighted-phrase">    comment</span></span></span>
            //     <span class="token comment"><span class="flexi-code__highlighted-phrase">    1</span>
            //     */</span>
            //     <span class="flexi-code__highlighted-phrase"><span class="token comment">/* 
            //         Multiline</span></span>
            //     <span class="flexi-code__line flexi-code__line_highlighted"><span class="flexi-code__highlighted-phrase"><span class="token comment">    comment</span></span></span>
            //     <span class="flexi-code__highlighted-phrase"><span class="token comment">    2
            //     */</span></span>
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"language\": \"csharp\",\n    \"highlightedLines\": [\n        { \"start\": 3, \"end\": 3 },\n        { \"start\": 8, \"end\": 8 }\n    ],\n    \"highlightedPhrases\": [\n        { \"regex\": \"Multiline.*?1\" },\n        { \"regex\": \"/.*?/\", \"included\": [1] }\n    ]\n}\n```\n/* \n    Multiline\n    comment\n    1\n*/\n/* \n    Multiline\n    comment\n    2\n*/\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_language-csharp flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_has-highlighted-lines flexi-code_has-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\"><span class=\"token comment\">/* \n    <span class=\"flexi-code__highlighted-phrase\">Multiline</span></span>\n<span class=\"flexi-code__line flexi-code__line_highlighted\"><span class=\"token comment\"><span class=\"flexi-code__highlighted-phrase\">    comment</span></span></span>\n<span class=\"token comment\"><span class=\"flexi-code__highlighted-phrase\">    1</span>\n*/</span>\n<span class=\"flexi-code__highlighted-phrase\"><span class=\"token comment\">/* \n    Multiline</span></span>\n<span class=\"flexi-code__line flexi-code__line_highlighted\"><span class=\"flexi-code__highlighted-phrase\"><span class=\"token comment\">    comment</span></span></span>\n<span class=\"flexi-code__highlighted-phrase\"><span class=\"token comment\">    2\n*/</span></span>\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec21(string extensions)
        {
            //     Start line number: 938
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "language": "csharp",
            //         "highlightedPhrases": [
            //             { "regex": "comment\\s+re" },
            //             { "regex": "\\+ \"d" }
            //         ]
            //     }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_language-csharp flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_has-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="token keyword">public</span> <span class="token keyword">string</span> <span class="token function">ExampleFunction</span><span class="token punctuation">(</span><span class="token keyword">string</span> arg<span class="token punctuation">)</span>
            //     <span class="token punctuation">{</span>
            //         <span class="token comment">// Example <span class="flexi-code__highlighted-phrase">comment</span></span><span class="flexi-code__highlighted-phrase">
            //         <span class="token keyword">re</span></span><span class="token keyword">turn</span> arg <span class="flexi-code__highlighted-phrase"><span class="token operator">+</span> <span class="token string">"d</span></span><span class="token string">ummyString"</span><span class="token punctuation">;</span>
            //     <span class="token punctuation">}</span>
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"language\": \"csharp\",\n    \"highlightedPhrases\": [\n        { \"regex\": \"comment\\\\s+re\" },\n        { \"regex\": \"\\\\+ \\\"d\" }\n    ]\n}\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_language-csharp flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_has-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\"><span class=\"token keyword\">public</span> <span class=\"token keyword\">string</span> <span class=\"token function\">ExampleFunction</span><span class=\"token punctuation\">(</span><span class=\"token keyword\">string</span> arg<span class=\"token punctuation\">)</span>\n<span class=\"token punctuation\">{</span>\n    <span class=\"token comment\">// Example <span class=\"flexi-code__highlighted-phrase\">comment</span></span><span class=\"flexi-code__highlighted-phrase\">\n    <span class=\"token keyword\">re</span></span><span class=\"token keyword\">turn</span> arg <span class=\"flexi-code__highlighted-phrase\"><span class=\"token operator\">+</span> <span class=\"token string\">\"d</span></span><span class=\"token string\">ummyString\"</span><span class=\"token punctuation\">;</span>\n<span class=\"token punctuation\">}</span>\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec22(string extensions)
        {
            //     Start line number: 975
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "highlightedPhrases": [
            //             { "regex": "comment\\s+re" },
            //             { "regex": "(return )(arg)" },
            //             { "regex": "return" },
            //             { "regex": "rg \\+" }
            //         ]
            //     }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_has-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">public string ExampleFunction(string arg)
            //     {
            //         // Example <span class="flexi-code__highlighted-phrase">comment
            //         return arg +</span> &quot;dummyString&quot;;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"highlightedPhrases\": [\n        { \"regex\": \"comment\\\\s+re\" },\n        { \"regex\": \"(return )(arg)\" },\n        { \"regex\": \"return\" },\n        { \"regex\": \"rg \\\\+\" }\n    ]\n}\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_has-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">public string ExampleFunction(string arg)\n{\n    // Example <span class=\"flexi-code__highlighted-phrase\">comment\n    return arg +</span> &quot;dummyString&quot;;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec23(string extensions)
        {
            //     Start line number: 1012
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "language": "csharp",
            //         "highlightedPhrases": [
            //             { "regex": "string ExampleFunction" },
            //             { "regex": "return" },
            //             { "regex": "(\"dum)myStr(ing\")" }
            //         ]
            //     }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_language-csharp flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_has-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="token keyword">public</span> <span class="flexi-code__highlighted-phrase"><span class="token keyword">string</span> <span class="token function">ExampleFunction</span></span><span class="token punctuation">(</span><span class="token keyword">string</span> arg<span class="token punctuation">)</span>
            //     <span class="token punctuation">{</span>
            //         <span class="token comment">// Example comment</span>
            //         <span class="flexi-code__highlighted-phrase"><span class="token keyword">return</span></span> arg <span class="token operator">+</span> <span class="flexi-code__highlighted-phrase"><span class="token string">"dum</span></span><span class="token string">myStr<span class="flexi-code__highlighted-phrase">ing"</span></span><span class="token punctuation">;</span>
            //     <span class="token punctuation">}</span>
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"language\": \"csharp\",\n    \"highlightedPhrases\": [\n        { \"regex\": \"string ExampleFunction\" },\n        { \"regex\": \"return\" },\n        { \"regex\": \"(\\\"dum)myStr(ing\\\")\" }\n    ]\n}\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_language-csharp flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_has-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\"><span class=\"token keyword\">public</span> <span class=\"flexi-code__highlighted-phrase\"><span class=\"token keyword\">string</span> <span class=\"token function\">ExampleFunction</span></span><span class=\"token punctuation\">(</span><span class=\"token keyword\">string</span> arg<span class=\"token punctuation\">)</span>\n<span class=\"token punctuation\">{</span>\n    <span class=\"token comment\">// Example comment</span>\n    <span class=\"flexi-code__highlighted-phrase\"><span class=\"token keyword\">return</span></span> arg <span class=\"token operator\">+</span> <span class=\"flexi-code__highlighted-phrase\"><span class=\"token string\">\"dum</span></span><span class=\"token string\">myStr<span class=\"flexi-code__highlighted-phrase\">ing\"</span></span><span class=\"token punctuation\">;</span>\n<span class=\"token punctuation\">}</span>\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec24(string extensions)
        {
            //     Start line number: 1052
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "language": "html"
            //     }
            //     ```
            //     <div class="my-class">&</div>
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_language-html flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="token tag"><span class="token tag"><span class="token punctuation">&lt;</span>div</span> <span class="token attr-name">class</span><span class="token attr-value"><span class="token punctuation">=</span><span class="token punctuation">"</span>my-class<span class="token punctuation">"</span></span><span class="token punctuation">></span></span>&amp;<span class="token tag"><span class="token tag"><span class="token punctuation">&lt;/</span>div</span><span class="token punctuation">></span></span>
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"language\": \"html\"\n}\n```\n<div class=\"my-class\">&</div>\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_language-html flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\"><span class=\"token tag\"><span class=\"token tag\"><span class=\"token punctuation\">&lt;</span>div</span> <span class=\"token attr-name\">class</span><span class=\"token attr-value\"><span class=\"token punctuation\">=</span><span class=\"token punctuation\">\"</span>my-class<span class=\"token punctuation\">\"</span></span><span class=\"token punctuation\">></span></span>&amp;<span class=\"token tag\"><span class=\"token tag\"><span class=\"token punctuation\">&lt;/</span>div</span><span class=\"token punctuation\">></span></span>\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec25(string extensions)
        {
            //     Start line number: 1076
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "language": "html",
            //         "syntaxHighlighter": "highlightjs"
            //     }
            //     ```
            //     <div class="my-class">&</div>
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_language-html flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="hljs-tag">&lt;<span class="hljs-name">div</span> <span class="hljs-attr">class</span>=<span class="hljs-string">"my-class"</span>&gt;</span>&amp;<span class="hljs-tag">&lt;/<span class="hljs-name">div</span>&gt;</span>
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"language\": \"html\",\n    \"syntaxHighlighter\": \"highlightjs\"\n}\n```\n<div class=\"my-class\">&</div>\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_language-html flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\"><span class=\"hljs-tag\">&lt;<span class=\"hljs-name\">div</span> <span class=\"hljs-attr\">class</span>=<span class=\"hljs-string\">\"my-class\"</span>&gt;</span>&amp;<span class=\"hljs-tag\">&lt;/<span class=\"hljs-name\">div</span>&gt;</span>\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec26(string extensions)
        {
            //     Start line number: 1101
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     ```
            //     <div class="my-class">&</div>
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">&lt;div class=&quot;my-class&quot;&gt;&amp;&lt;/div&gt;
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("```\n<div class=\"my-class\">&</div>\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">&lt;div class=&quot;my-class&quot;&gt;&amp;&lt;/div&gt;\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec27(string extensions)
        {
            //     Start line number: 1122
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "highlightedPhrases": [{ "regex": "div" }]
            //     }
            //     ```
            //     <div class="my-class">&</div>
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_has-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">&lt;<span class="flexi-code__highlighted-phrase">div</span> class=&quot;my-class&quot;&gt;&amp;&lt;/<span class="flexi-code__highlighted-phrase">div</span>&gt;
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"highlightedPhrases\": [{ \"regex\": \"div\" }]\n}\n```\n<div class=\"my-class\">&</div>\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_has-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">&lt;<span class=\"flexi-code__highlighted-phrase\">div</span> class=&quot;my-class&quot;&gt;&amp;&lt;/<span class=\"flexi-code__highlighted-phrase\">div</span>&gt;\n</code></pre>\n</div>",
                extensions,
                false);
        }
    }

    public class FlexiSectionBlocksSpecs
    {
        [Theory]
        [InlineData("FlexiSectionBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec1(string extensions)
        {
            //     Start line number: 92
            //     --------------- Markdown ---------------
            //     # Indoor Herb Gardens
            //     An introduction..
            //     
            //     ## Getting Started
            //     
            //     ### Growing Herbs from Cuttings
            //     Information on growing herbs from cuttings..
            //     
            //     ## Caring for Herbs
            //     
            //     ### Watering Herbs
            //     Information on watering herbs..
            //     --------------- Expected Markup ---------------
            //     <section class="flexi-section flexi-section_level_1 flexi-section_has-link-icon" id="indoor-herb-gardens">
            //     <header class="flexi-section__header">
            //     <h1 class="flexi-section__heading">Indoor Herb Gardens</h1>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <p>An introduction..</p>
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="getting-started">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">Getting Started</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <section class="flexi-section flexi-section_level_3 flexi-section_has-link-icon" id="growing-herbs-from-cuttings">
            //     <header class="flexi-section__header">
            //     <h3 class="flexi-section__heading">Growing Herbs from Cuttings</h3>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <p>Information on growing herbs from cuttings..</p>
            //     </section>
            //     </section>
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="caring-for-herbs">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">Caring for Herbs</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <section class="flexi-section flexi-section_level_3 flexi-section_has-link-icon" id="watering-herbs">
            //     <header class="flexi-section__header">
            //     <h3 class="flexi-section__heading">Watering Herbs</h3>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <p>Information on watering herbs..</p>
            //     </section>
            //     </section>
            //     </section>

            SpecTestHelper.AssertCompliance("# Indoor Herb Gardens\nAn introduction..\n\n## Getting Started\n\n### Growing Herbs from Cuttings\nInformation on growing herbs from cuttings..\n\n## Caring for Herbs\n\n### Watering Herbs\nInformation on watering herbs..",
                "<section class=\"flexi-section flexi-section_level_1 flexi-section_has-link-icon\" id=\"indoor-herb-gardens\">\n<header class=\"flexi-section__header\">\n<h1 class=\"flexi-section__heading\">Indoor Herb Gardens</h1>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<p>An introduction..</p>\n<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"getting-started\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">Getting Started</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<section class=\"flexi-section flexi-section_level_3 flexi-section_has-link-icon\" id=\"growing-herbs-from-cuttings\">\n<header class=\"flexi-section__header\">\n<h3 class=\"flexi-section__heading\">Growing Herbs from Cuttings</h3>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<p>Information on growing herbs from cuttings..</p>\n</section>\n</section>\n<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"caring-for-herbs\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">Caring for Herbs</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<section class=\"flexi-section flexi-section_level_3 flexi-section_has-link-icon\" id=\"watering-herbs\">\n<header class=\"flexi-section__header\">\n<h3 class=\"flexi-section__heading\">Watering Herbs</h3>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<p>Information on watering herbs..</p>\n</section>\n</section>\n</section>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec2(string extensions)
        {
            //     Start line number: 173
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{ "blockName": "section" }
            //     ## foo
            //     --------------- Expected Markup ---------------
            //     <section class="section section_level_2 section_has-link-icon" id="foo">
            //     <header class="section__header">
            //     <h2 class="section__heading">foo</h2>
            //     <button class="section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     </section>

            SpecTestHelper.AssertCompliance("@{ \"blockName\": \"section\" }\n## foo",
                "<section class=\"section section_level_2 section_has-link-icon\" id=\"foo\">\n<header class=\"section__header\">\n<h2 class=\"section__heading\">foo</h2>\n<button class=\"section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n</section>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec3(string extensions)
        {
            //     Start line number: 196
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{ "element": "nav" }
            //     ## foo
            //     --------------- Expected Markup ---------------
            //     <nav class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="foo">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">foo</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     </nav>

            SpecTestHelper.AssertCompliance("@{ \"element\": \"nav\" }\n## foo",
                "<nav class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"foo\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">foo</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n</nav>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec4(string extensions)
        {
            //     Start line number: 226
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     ## Foo Bar Baz
            //     --------------- Expected Markup ---------------
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="foo-bar-baz">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">Foo Bar Baz</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     </section>

            SpecTestHelper.AssertCompliance("## Foo Bar Baz",
                "<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"foo-bar-baz\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">Foo Bar Baz</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n</section>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec5(string extensions)
        {
            //     Start line number: 242
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     ## foo
            //     ### `foo`
            //     ## foo 1
            //     --------------- Expected Markup ---------------
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="foo">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">foo</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <section class="flexi-section flexi-section_level_3 flexi-section_has-link-icon" id="foo-1">
            //     <header class="flexi-section__header">
            //     <h3 class="flexi-section__heading"><code>foo</code></h3>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     </section>
            //     </section>
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="foo-1-1">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">foo 1</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     </section>

            SpecTestHelper.AssertCompliance("## foo\n### `foo`\n## foo 1",
                "<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"foo\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">foo</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<section class=\"flexi-section flexi-section_level_3 flexi-section_has-link-icon\" id=\"foo-1\">\n<header class=\"flexi-section__header\">\n<h3 class=\"flexi-section__heading\"><code>foo</code></h3>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n</section>\n</section>\n<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"foo-1-1\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">foo 1</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n</section>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec6(string extensions)
        {
            //     Start line number: 276
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{ "generateID": false }
            //     ## Foo Bar Baz
            //     --------------- Expected Markup ---------------
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">Foo Bar Baz</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     </section>

            SpecTestHelper.AssertCompliance("@{ \"generateID\": false }\n## Foo Bar Baz",
                "<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">Foo Bar Baz</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n</section>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec7(string extensions)
        {
            //     Start line number: 293
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{ 
            //         "attributes": {
            //           "id" : "my-custom-id"
            //         }
            //     }
            //     ## Foo Bar Baz
            //     --------------- Expected Markup ---------------
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="foo-bar-baz">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">Foo Bar Baz</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     </section>

            SpecTestHelper.AssertCompliance("@{ \n    \"attributes\": {\n      \"id\" : \"my-custom-id\"\n    }\n}\n## Foo Bar Baz",
                "<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"foo-bar-baz\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">Foo Bar Baz</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n</section>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec8(string extensions)
        {
            //     Start line number: 321
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "linkIcon": "<svg><use xlink:href=\"#material-design-link\"></use></svg>"
            //     }
            //     ## foo
            //     --------------- Expected Markup ---------------
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="foo">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">foo</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon"><use xlink:href="#material-design-link"></use></svg>
            //     </button>
            //     </header>
            //     </section>

            SpecTestHelper.AssertCompliance("@{\n    \"linkIcon\": \"<svg><use xlink:href=\\\"#material-design-link\\\"></use></svg>\"\n}\n## foo",
                "<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"foo\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">foo</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\"><use xlink:href=\"#material-design-link\"></use></svg>\n</button>\n</header>\n</section>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec9(string extensions)
        {
            //     Start line number: 340
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{ "linkIcon": null }
            //     # foo
            //     --------------- Expected Markup ---------------
            //     <section class="flexi-section flexi-section_level_1 flexi-section_no-link-icon" id="foo">
            //     <header class="flexi-section__header">
            //     <h1 class="flexi-section__heading">foo</h1>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     </button>
            //     </header>
            //     </section>

            SpecTestHelper.AssertCompliance("@{ \"linkIcon\": null }\n# foo",
                "<section class=\"flexi-section flexi-section_level_1 flexi-section_no-link-icon\" id=\"foo\">\n<header class=\"flexi-section__header\">\n<h1 class=\"flexi-section__heading\">foo</h1>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n</button>\n</header>\n</section>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec10(string extensions)
        {
            //     Start line number: 369
            //     --------------- Markdown ---------------
            //     [foo]
            //     
            //     ## foo
            //     
            //     [foo]
            //     [Link Text][foo]
            //     --------------- Expected Markup ---------------
            //     <p><a href="#foo">foo</a></p>
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="foo">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">foo</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <p><a href="#foo">foo</a>
            //     <a href="#foo">Link Text</a></p>
            //     </section>

            SpecTestHelper.AssertCompliance("[foo]\n\n## foo\n\n[foo]\n[Link Text][foo]",
                "<p><a href=\"#foo\">foo</a></p>\n<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"foo\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">foo</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<p><a href=\"#foo\">foo</a>\n<a href=\"#foo\">Link Text</a></p>\n</section>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec11(string extensions)
        {
            //     Start line number: 391
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     [foo]
            //     
            //     @{ "autoLinkable": false }
            //     ## foo
            //     
            //     [foo]
            //     [Link Text][foo]
            //     --------------- Expected Markup ---------------
            //     <p>[foo]</p>
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">foo</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <p>[foo]
            //     [Link Text][foo]</p>
            //     </section>

            SpecTestHelper.AssertCompliance("[foo]\n\n@{ \"autoLinkable\": false }\n## foo\n\n[foo]\n[Link Text][foo]",
                "<p>[foo]</p>\n<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">foo</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<p>[foo]\n[Link Text][foo]</p>\n</section>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec12(string extensions)
        {
            //     Start line number: 416
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     [foo]
            //     
            //     @{ "generateID": false }
            //     ## foo
            //     
            //     [foo]
            //     [Link Text][foo]
            //     --------------- Expected Markup ---------------
            //     <p>[foo]</p>
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">foo</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <p>[foo]
            //     [Link Text][foo]</p>
            //     </section>

            SpecTestHelper.AssertCompliance("[foo]\n\n@{ \"generateID\": false }\n## foo\n\n[foo]\n[Link Text][foo]",
                "<p>[foo]</p>\n<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">foo</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<p>[foo]\n[Link Text][foo]</p>\n</section>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec13(string extensions)
        {
            //     Start line number: 442
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     ## Rosemary
            //     ### Watering
            //     Rosemary watering needs..
            //     
            //     ## Lemon Balm
            //     ### Watering
            //     Lemon Balm watering needs..
            //     
            //     ## Peppermint
            //     ### Watering
            //     Similar to [Lemon Balm watering needs][watering 1]...
            //     --------------- Expected Markup ---------------
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="rosemary">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">Rosemary</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <section class="flexi-section flexi-section_level_3 flexi-section_has-link-icon" id="watering">
            //     <header class="flexi-section__header">
            //     <h3 class="flexi-section__heading">Watering</h3>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <p>Rosemary watering needs..</p>
            //     </section>
            //     </section>
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="lemon-balm">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">Lemon Balm</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <section class="flexi-section flexi-section_level_3 flexi-section_has-link-icon" id="watering-1">
            //     <header class="flexi-section__header">
            //     <h3 class="flexi-section__heading">Watering</h3>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <p>Lemon Balm watering needs..</p>
            //     </section>
            //     </section>
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="peppermint">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">Peppermint</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <section class="flexi-section flexi-section_level_3 flexi-section_has-link-icon" id="watering-2">
            //     <header class="flexi-section__header">
            //     <h3 class="flexi-section__heading">Watering</h3>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <p>Similar to <a href="#watering-1">Lemon Balm watering needs</a>...</p>
            //     </section>
            //     </section>

            SpecTestHelper.AssertCompliance("## Rosemary\n### Watering\nRosemary watering needs..\n\n## Lemon Balm\n### Watering\nLemon Balm watering needs..\n\n## Peppermint\n### Watering\nSimilar to [Lemon Balm watering needs][watering 1]...",
                "<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"rosemary\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">Rosemary</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<section class=\"flexi-section flexi-section_level_3 flexi-section_has-link-icon\" id=\"watering\">\n<header class=\"flexi-section__header\">\n<h3 class=\"flexi-section__heading\">Watering</h3>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<p>Rosemary watering needs..</p>\n</section>\n</section>\n<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"lemon-balm\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">Lemon Balm</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<section class=\"flexi-section flexi-section_level_3 flexi-section_has-link-icon\" id=\"watering-1\">\n<header class=\"flexi-section__header\">\n<h3 class=\"flexi-section__heading\">Watering</h3>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<p>Lemon Balm watering needs..</p>\n</section>\n</section>\n<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"peppermint\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">Peppermint</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<section class=\"flexi-section flexi-section_level_3 flexi-section_has-link-icon\" id=\"watering-2\">\n<header class=\"flexi-section__header\">\n<h3 class=\"flexi-section__heading\">Watering</h3>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<p>Similar to <a href=\"#watering-1\">Lemon Balm watering needs</a>...</p>\n</section>\n</section>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec14(string extensions)
        {
            //     Start line number: 517
            //     --------------- Markdown ---------------
            //     ## foo
            //     --------------- Expected Markup ---------------
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="foo">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">foo</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     </section>

            SpecTestHelper.AssertCompliance("## foo",
                "<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"foo\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">foo</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n</section>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec15(string extensions)
        {
            //     Start line number: 532
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{ "renderingMode": "classic" }
            //     ## foo
            //     --------------- Expected Markup ---------------
            //     <h2>foo</h2>

            SpecTestHelper.AssertCompliance("@{ \"renderingMode\": \"classic\" }\n## foo",
                "<h2>foo</h2>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec16(string extensions)
        {
            //     Start line number: 551
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "attributes": {
            //             "id" : "section-1",
            //             "class" : "block"
            //         },
            //         "generateID": false
            //     }
            //     ## foo
            //     --------------- Expected Markup ---------------
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon block" id="section-1">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">foo</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     </section>

            SpecTestHelper.AssertCompliance("@{\n    \"attributes\": {\n        \"id\" : \"section-1\",\n        \"class\" : \"block\"\n    },\n    \"generateID\": false\n}\n## foo",
                "<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon block\" id=\"section-1\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">foo</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n</section>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec17(string extensions)
        {
            //     Start line number: 589
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiSectionBlocks": {
            //             "defaultBlockOptions": {
            //                 "blockName": "section",
            //                 "element": "nav",
            //                 "generateID": false,
            //                 "linkIcon": "<svg><use xlink:href=\"#material-design-link\"></use></svg>",
            //                 "attributes": {
            //                     "class": "block"
            //                 }
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     # foo
            //     ## bar
            //     
            //     [foo]
            //     [bar]
            //     --------------- Expected Markup ---------------
            //     <nav class="section section_level_1 section_has-link-icon block">
            //     <header class="section__header">
            //     <h1 class="section__heading">foo</h1>
            //     <button class="section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="section__link-icon"><use xlink:href="#material-design-link"></use></svg>
            //     </button>
            //     </header>
            //     <nav class="section section_level_2 section_has-link-icon block">
            //     <header class="section__header">
            //     <h2 class="section__heading">bar</h2>
            //     <button class="section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="section__link-icon"><use xlink:href="#material-design-link"></use></svg>
            //     </button>
            //     </header>
            //     <p>[foo]
            //     [bar]</p>
            //     </nav>
            //     </nav>

            SpecTestHelper.AssertCompliance("# foo\n## bar\n\n[foo]\n[bar]",
                "<nav class=\"section section_level_1 section_has-link-icon block\">\n<header class=\"section__header\">\n<h1 class=\"section__heading\">foo</h1>\n<button class=\"section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"section__link-icon\"><use xlink:href=\"#material-design-link\"></use></svg>\n</button>\n</header>\n<nav class=\"section section_level_2 section_has-link-icon block\">\n<header class=\"section__header\">\n<h2 class=\"section__heading\">bar</h2>\n<button class=\"section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"section__link-icon\"><use xlink:href=\"#material-design-link\"></use></svg>\n</button>\n</header>\n<p>[foo]\n[bar]</p>\n</nav>\n</nav>",
                extensions,
                false,
                "{\n    \"flexiSectionBlocks\": {\n        \"defaultBlockOptions\": {\n            \"blockName\": \"section\",\n            \"element\": \"nav\",\n            \"generateID\": false,\n            \"linkIcon\": \"<svg><use xlink:href=\\\"#material-design-link\\\"></use></svg>\",\n            \"attributes\": {\n                \"class\": \"block\"\n            }\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiSectionBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec18(string extensions)
        {
            //     Start line number: 631
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiSectionBlocks": {
            //             "defaultBlockOptions": {
            //                 "element": "nav"
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     @{
            //         "element": "article"
            //     }
            //     # foo
            //     ## bar
            //     --------------- Expected Markup ---------------
            //     <article class="flexi-section flexi-section_level_1 flexi-section_has-link-icon" id="foo">
            //     <header class="flexi-section__header">
            //     <h1 class="flexi-section__heading">foo</h1>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <nav class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="bar">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">bar</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     </nav>
            //     </article>

            SpecTestHelper.AssertCompliance("@{\n    \"element\": \"article\"\n}\n# foo\n## bar",
                "<article class=\"flexi-section flexi-section_level_1 flexi-section_has-link-icon\" id=\"foo\">\n<header class=\"flexi-section__header\">\n<h1 class=\"flexi-section__heading\">foo</h1>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<nav class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"bar\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">bar</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n</nav>\n</article>",
                extensions,
                false,
                "{\n    \"flexiSectionBlocks\": {\n        \"defaultBlockOptions\": {\n            \"element\": \"nav\"\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiSectionBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec19(string extensions)
        {
            //     Start line number: 672
            //     --------------- Markdown ---------------
            //     # foo
            //     
            //     > # foo
            //     > ## foo
            //     
            //     ## foo
            //     --------------- Expected Markup ---------------
            //     <section class="flexi-section flexi-section_level_1 flexi-section_has-link-icon" id="foo">
            //     <header class="flexi-section__header">
            //     <h1 class="flexi-section__heading">foo</h1>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <blockquote>
            //     <section class="flexi-section flexi-section_level_1 flexi-section_has-link-icon" id="foo-1">
            //     <header class="flexi-section__header">
            //     <h1 class="flexi-section__heading">foo</h1>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="foo-2">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">foo</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     </section>
            //     </section>
            //     </blockquote>
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="foo-3">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">foo</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     </section>
            //     </section>

            SpecTestHelper.AssertCompliance("# foo\n\n> # foo\n> ## foo\n\n## foo",
                "<section class=\"flexi-section flexi-section_level_1 flexi-section_has-link-icon\" id=\"foo\">\n<header class=\"flexi-section__header\">\n<h1 class=\"flexi-section__heading\">foo</h1>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<blockquote>\n<section class=\"flexi-section flexi-section_level_1 flexi-section_has-link-icon\" id=\"foo-1\">\n<header class=\"flexi-section__header\">\n<h1 class=\"flexi-section__heading\">foo</h1>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"foo-2\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">foo</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n</section>\n</section>\n</blockquote>\n<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"foo-3\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">foo</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n</section>\n</section>",
                extensions,
                false);
        }
    }

    public class FlexiTableBlocksSpecs
    {
        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec1(string extensions)
        {
            //     Start line number: 78
            //     --------------- Markdown ---------------
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1   | cell 2   |
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("| header 1 | header 2 |\n|----------|----------|\n| cell 1   | cell 2   |",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec2(string extensions)
        {
            //     Start line number: 121
            //     --------------- Markdown ---------------
            //     The following is not a table:
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1   | cell 2   |
            //     
            //     The following is a table:
            //     
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1   | cell 2   |
            //     --------------- Expected Markup ---------------
            //     <p>The following is not a table:
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1   | cell 2   |</p>
            //     <p>The following is a table:</p>
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("The following is not a table:\n| header 1 | header 2 |\n|----------|----------|\n| cell 1   | cell 2   |\n\nThe following is a table:\n\n| header 1 | header 2 |\n|----------|----------|\n| cell 1   | cell 2   |",
                "<p>The following is not a table:\n| header 1 | header 2 |\n|----------|----------|\n| cell 1   | cell 2   |</p>\n<p>The following is a table:</p>\n<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec3(string extensions)
        {
            //     Start line number: 176
            //     --------------- Markdown ---------------
            //     | header 1 | header 2 | header 3 |
            //     |:---------|:--------:|---------:|
            //     | cell 1   | cell 2   | cell 3   |
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header flexi-table__header_align_start">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header flexi-table__header_align_center">
            //     header 2
            //     </th>
            //     <th class="flexi-table__header flexi-table__header_align_end">
            //     header 3
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data flexi-table__data_align_start">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data flexi-table__data_align_center">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     <td class="flexi-table__data flexi-table__data_align_end">
            //     <div class="flexi-table__label">
            //     header 3
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 3
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("| header 1 | header 2 | header 3 |\n|:---------|:--------:|---------:|\n| cell 1   | cell 2   | cell 3   |",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header flexi-table__header_align_start\">\nheader 1\n</th>\n<th class=\"flexi-table__header flexi-table__header_align_center\">\nheader 2\n</th>\n<th class=\"flexi-table__header flexi-table__header_align_end\">\nheader 3\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data flexi-table__data_align_start\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data flexi-table__data_align_center\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n<td class=\"flexi-table__data flexi-table__data_align_end\">\n<div class=\"flexi-table__label\">\nheader 3\n</div>\n<div class=\"flexi-table__content\">\ncell 3\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec4(string extensions)
        {
            //     Start line number: 230
            //     --------------- Markdown ---------------
            //     |:-------|-------:|
            //     | cell 1 | cell 2 |
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data flexi-table__data_align_start">
            //     cell 1
            //     </td>
            //     <td class="flexi-table__data flexi-table__data_align_end">
            //     cell 2
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("|:-------|-------:|\n| cell 1 | cell 2 |",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data flexi-table__data_align_start\">\ncell 1\n</td>\n<td class=\"flexi-table__data flexi-table__data_align_end\">\ncell 2\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec5(string extensions)
        {
            //     Start line number: 252
            //     --------------- Markdown ---------------
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1   | cell 2   |
            //     |----------|----------|
            //     | cell 3   | cell 4   |
            //     |----------|----------|
            //     | cell 5   | cell 6   |
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     <hr />
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     <hr />
            //     </div>
            //     </td>
            //     </tr>
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 3
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 4
            //     </div>
            //     </td>
            //     </tr>
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     <hr />
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     <hr />
            //     </div>
            //     </td>
            //     </tr>
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 5
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 6
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("| header 1 | header 2 |\n|----------|----------|\n| cell 1   | cell 2   |\n|----------|----------|\n| cell 3   | cell 4   |\n|----------|----------|\n| cell 5   | cell 6   |",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\n<hr />\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\n<hr />\n</div>\n</td>\n</tr>\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 3\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 4\n</div>\n</td>\n</tr>\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\n<hr />\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\n<hr />\n</div>\n</td>\n</tr>\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 5\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 6\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec6(string extensions)
        {
            //     Start line number: 371
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{ "type": "unresponsive" }
            //     | header 1 | header 2 |
            //     | header 3 | header 4 |
            //     |----------|----------|
            //     | cell 1   | cell 2   |
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_unresponsive">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 3
            //     </th>
            //     <th class="flexi-table__header">
            //     header 4
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     cell 1
            //     </td>
            //     <td class="flexi-table__data">
            //     cell 2
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("@{ \"type\": \"unresponsive\" }\n| header 1 | header 2 |\n| header 3 | header 4 |\n|----------|----------|\n| cell 1   | cell 2   |",
                "<div class=\"flexi-table flexi-table_type_unresponsive\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 3\n</th>\n<th class=\"flexi-table__header\">\nheader 4\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\ncell 1\n</td>\n<td class=\"flexi-table__data\">\ncell 2\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec7(string extensions)
        {
            //     Start line number: 416
            //     --------------- Markdown ---------------
            //     | cell 1   | cell 2   |
            //     | cell 3   | cell 4   |
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     cell 1
            //     </td>
            //     <td class="flexi-table__data">
            //     cell 2
            //     </td>
            //     </tr>
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     cell 3
            //     </td>
            //     <td class="flexi-table__data">
            //     cell 4
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("| cell 1   | cell 2   |\n| cell 3   | cell 4   |",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\ncell 1\n</td>\n<td class=\"flexi-table__data\">\ncell 2\n</td>\n</tr>\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\ncell 3\n</td>\n<td class=\"flexi-table__data\">\ncell 4\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec8(string extensions)
        {
            //     Start line number: 446
            //     --------------- Markdown ---------------
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     | \| \| \| | \| \| \| |
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     | | |
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     | | |
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("| header 1 | header 2 |\n|----------|----------|\n| \\| \\| \\| | \\| \\| \\| |",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\n| | |\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\n| | |\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec9(string extensions)
        {
            //     Start line number: 489
            //     --------------- Markdown ---------------
            //     | header 1 | header 2 |
            //     |---|---|
            //     | cell 1 | cell 2 |
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("| header 1 | header 2 |\n|---|---|\n| cell 1 | cell 2 |",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec10(string extensions)
        {
            //     Start line number: 532
            //     --------------- Markdown ---------------
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1 | cell 2 | cell 3 |
            //     --------------- Expected Markup ---------------
            //     <p>| header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1 | cell 2 | cell 3 |</p>

            SpecTestHelper.AssertCompliance("| header 1 | header 2 |\n|----------|----------|\n| cell 1 | cell 2 | cell 3 |",
                "<p>| header 1 | header 2 |\n|----------|----------|\n| cell 1 | cell 2 | cell 3 |</p>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec11(string extensions)
        {
            //     Start line number: 544
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     |cell 1    |    cell 2|
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">cell 2
            //     </code></pre>
            //     </div>
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("| header 1 | header 2 |\n|----------|----------|\n|cell 1    |    cell 2|",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\n<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">cell 2\n</code></pre>\n</div>\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec12(string extensions)
        {
            //     Start line number: 598
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     | **header 1** | [header 2](url) | *header 3* |
            //     |----------|----------|----------|
            //     | `cell 1` |    cell 2 | > cell 3 |
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     <strong>header 1</strong>
            //     </th>
            //     <th class="flexi-table__header">
            //     <a href="url">header 2</a>
            //     </th>
            //     <th class="flexi-table__header">
            //     <em>header 3</em>
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     <strong>header 1</strong>
            //     </div>
            //     <div class="flexi-table__content">
            //     <code>cell 1</code>
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     <a href="url">header 2</a>
            //     </div>
            //     <div class="flexi-table__content">
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">cell 2
            //     </code></pre>
            //     </div>
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     <em>header 3</em>
            //     </div>
            //     <div class="flexi-table__content">
            //     <blockquote>
            //     <p>cell 3</p>
            //     </blockquote>
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("| **header 1** | [header 2](url) | *header 3* |\n|----------|----------|----------|\n| `cell 1` |    cell 2 | > cell 3 |",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\n<strong>header 1</strong>\n</th>\n<th class=\"flexi-table__header\">\n<a href=\"url\">header 2</a>\n</th>\n<th class=\"flexi-table__header\">\n<em>header 3</em>\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\n<strong>header 1</strong>\n</div>\n<div class=\"flexi-table__content\">\n<code>cell 1</code>\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\n<a href=\"url\">header 2</a>\n</div>\n<div class=\"flexi-table__content\">\n<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">cell 2\n</code></pre>\n</div>\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\n<em>header 3</em>\n</div>\n<div class=\"flexi-table__content\">\n<blockquote>\n<p>cell 3</p>\n</blockquote>\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec13(string extensions)
        {
            //     Start line number: 665
            //     --------------- Markdown ---------------
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1   | cell 2   |
            //     This line ends the table
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>
            //     <p>This line ends the table</p>

            SpecTestHelper.AssertCompliance("| header 1 | header 2 |\n|----------|----------|\n| cell 1   | cell 2   |\nThis line ends the table",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>\n<p>This line ends the table</p>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec14(string extensions)
        {
            //     Start line number: 710
            //     --------------- Markdown ---------------
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1   | cell 2   |
            //     | This line invalidates the preceding table
            //     
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1   | cell 2   |
            //     
            //     | This line does not invalidate the preceding table
            //     --------------- Expected Markup ---------------
            //     <p>| header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1   | cell 2   |
            //     | This line invalidates the preceding table</p>
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>
            //     <p>| This line does not invalidate the preceding table</p>

            SpecTestHelper.AssertCompliance("| header 1 | header 2 |\n|----------|----------|\n| cell 1   | cell 2   |\n| This line invalidates the preceding table\n\n| header 1 | header 2 |\n|----------|----------|\n| cell 1   | cell 2   |\n\n| This line does not invalidate the preceding table",
                "<p>| header 1 | header 2 |\n|----------|----------|\n| cell 1   | cell 2   |\n| This line invalidates the preceding table</p>\n<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>\n<p>| This line does not invalidate the preceding table</p>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec15(string extensions)
        {
            //     Start line number: 778
            //     --------------- Markdown ---------------
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+--------------+--------------+",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec16(string extensions)
        {
            //     Start line number: 823
            //     --------------- Markdown ---------------
            //     The following is not a table:
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+
            //     
            //     The following is a table:
            //     
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <p>The following is not a table:
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+</p>
            //     <p>The following is a table:</p>
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("The following is not a table:\n+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+--------------+--------------+\n\nThe following is a table:\n\n+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+--------------+--------------+",
                "<p>The following is not a table:\n+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+--------------+--------------+</p>\n<p>The following is a table:</p>\n<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec17(string extensions)
        {
            //     Start line number: 884
            //     --------------- Markdown ---------------
            //     +:-------------+:------------:+-------------:+
            //     | header 1     | header 2     | header 3     |
            //     +==============+==============+==============+
            //     | cell 1       | cell 2       | cell 3       |
            //     +--------------+--------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header flexi-table__header_align_start">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header flexi-table__header_align_center">
            //     header 2
            //     </th>
            //     <th class="flexi-table__header flexi-table__header_align_end">
            //     header 3
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data flexi-table__data_align_start">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data flexi-table__data_align_center">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     <td class="flexi-table__data flexi-table__data_align_end">
            //     <div class="flexi-table__label">
            //     header 3
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 3
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("+:-------------+:------------:+-------------:+\n| header 1     | header 2     | header 3     |\n+==============+==============+==============+\n| cell 1       | cell 2       | cell 3       |\n+--------------+--------------+--------------+",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header flexi-table__header_align_start\">\nheader 1\n</th>\n<th class=\"flexi-table__header flexi-table__header_align_center\">\nheader 2\n</th>\n<th class=\"flexi-table__header flexi-table__header_align_end\">\nheader 3\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data flexi-table__data_align_start\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data flexi-table__data_align_center\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n<td class=\"flexi-table__data flexi-table__data_align_end\">\n<div class=\"flexi-table__label\">\nheader 3\n</div>\n<div class=\"flexi-table__content\">\ncell 3\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec18(string extensions)
        {
            //     Start line number: 940
            //     --------------- Markdown ---------------
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec19(string extensions)
        {
            //     Start line number: 985
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{ "type": "unresponsive" }
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     + still cell 1 +--------------+
            //     | still cell 1 | cell 3       |
            //     +--------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_unresponsive">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data" rowspan="2">
            //     cell 1
            //     still cell 1
            //     still cell 1
            //     </td>
            //     <td class="flexi-table__data">
            //     cell 2
            //     </td>
            //     </tr>
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     cell 3
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("@{ \"type\": \"unresponsive\" }\n+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+ still cell 1 +--------------+\n| still cell 1 | cell 3       |\n+--------------+--------------+",
                "<div class=\"flexi-table flexi-table_type_unresponsive\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\" rowspan=\"2\">\ncell 1\nstill cell 1\nstill cell 1\n</td>\n<td class=\"flexi-table__data\">\ncell 2\n</td>\n</tr>\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\ncell 3\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec20(string extensions)
        {
            //     Start line number: 1032
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{ "type": "unresponsive" }
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +==============+==============+
            //     | still cell 1 | still cell 2 |
            //     +==============+==============+
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_unresponsive">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data" rowspan="2">
            //     <h1>cell 1</h1>
            //     <h1>still cell 1</h1>
            //     </td>
            //     <td class="flexi-table__data" rowspan="2">
            //     <h1>cell 2</h1>
            //     <h1>still cell 2</h1>
            //     </td>
            //     </tr>
            //     <tr class="flexi-table__row">
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("@{ \"type\": \"unresponsive\" }\n+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+==============+==============+\n| still cell 1 | still cell 2 |\n+==============+==============+",
                "<div class=\"flexi-table flexi-table_type_unresponsive\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\" rowspan=\"2\">\n<h1>cell 1</h1>\n<h1>still cell 1</h1>\n</td>\n<td class=\"flexi-table__data\" rowspan=\"2\">\n<h1>cell 2</h1>\n<h1>still cell 2</h1>\n</td>\n</tr>\n<tr class=\"flexi-table__row\">\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec21(string extensions)
        {
            //     Start line number: 1076
            //     --------------- Markdown ---------------
            //     +--------------+--------------+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+
            //     | cell 3       | cell 4       |
            //     +--------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     cell 1
            //     </td>
            //     <td class="flexi-table__data">
            //     cell 2
            //     </td>
            //     </tr>
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     cell 3
            //     </td>
            //     <td class="flexi-table__data">
            //     cell 4
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("+--------------+--------------+\n| cell 1       | cell 2       |\n+--------------+--------------+\n| cell 3       | cell 4       |\n+--------------+--------------+",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\ncell 1\n</td>\n<td class=\"flexi-table__data\">\ncell 2\n</td>\n</tr>\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\ncell 3\n</td>\n<td class=\"flexi-table__data\">\ncell 4\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec22(string extensions)
        {
            //     Start line number: 1109
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{ "type": "unresponsive" }
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +--------------+--------------+
            //     | header 3     | header 4     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_unresponsive">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 3
            //     </th>
            //     <th class="flexi-table__header">
            //     header 4
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     cell 1
            //     </td>
            //     <td class="flexi-table__data">
            //     cell 2
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("@{ \"type\": \"unresponsive\" }\n+--------------+--------------+\n| header 1     | header 2     |\n+--------------+--------------+\n| header 3     | header 4     |\n+==============+==============+\n| cell 1       | cell 2       |\n+--------------+--------------+",
                "<div class=\"flexi-table flexi-table_type_unresponsive\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 3\n</th>\n<th class=\"flexi-table__header\">\nheader 4\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\ncell 1\n</td>\n<td class=\"flexi-table__data\">\ncell 2\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec23(string extensions)
        {
            //     Start line number: 1157
            //     --------------- Markdown ---------------
            //     +--------------+----------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+
            //     
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==========+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+
            //     
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1   | cell 2   |
            //     +--------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <p>+--------------+----------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+</p>
            //     <p>+--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==========+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+</p>
            //     <p>+--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1   | cell 2   |
            //     +--------------+--------------+</p>

            SpecTestHelper.AssertCompliance("+--------------+----------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+--------------+--------------+\n\n+--------------+--------------+\n| header 1     | header 2     |\n+==============+==========+\n| cell 1       | cell 2       |\n+--------------+--------------+\n\n+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1   | cell 2   |\n+--------------+--------------+",
                "<p>+--------------+----------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+--------------+--------------+</p>\n<p>+--------------+--------------+\n| header 1     | header 2     |\n+==============+==========+\n| cell 1       | cell 2       |\n+--------------+--------------+</p>\n<p>+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1   | cell 2   |\n+--------------+--------------+</p>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec24(string extensions)
        {
            //     Start line number: 1195
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{ "type": "unresponsive" }
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1                      |
            //     +--------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_unresponsive">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data" colspan="2">
            //     cell 1
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("@{ \"type\": \"unresponsive\" }\n+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1                      |\n+--------------+--------------+",
                "<div class=\"flexi-table flexi-table_type_unresponsive\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\" colspan=\"2\">\ncell 1\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec25(string extensions)
        {
            //     Start line number: 1230
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{ "type": "unresponsive" }
            //     +--------------+--------------+--------------+
            //     | header 1     | header 2     | header 3     |
            //     +==============+==============+==============+
            //     | no span      | rowspan and colspan         |
            //     +--------------+                             +
            //     | no span      |                             |
            //     +--------------+--------------+--------------+
            //     | rowspan      | colspan                     |
            //     +              +--------------+--------------+
            //     |              | no span      | no span      |
            //     +--------------+--------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_unresponsive">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     <th class="flexi-table__header">
            //     header 3
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     no span
            //     </td>
            //     <td class="flexi-table__data" colspan="2" rowspan="2">
            //     rowspan and colspan
            //     </td>
            //     </tr>
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     no span
            //     </td>
            //     </tr>
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data" rowspan="2">
            //     rowspan
            //     </td>
            //     <td class="flexi-table__data" colspan="2">
            //     colspan
            //     </td>
            //     </tr>
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     no span
            //     </td>
            //     <td class="flexi-table__data">
            //     no span
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("@{ \"type\": \"unresponsive\" }\n+--------------+--------------+--------------+\n| header 1     | header 2     | header 3     |\n+==============+==============+==============+\n| no span      | rowspan and colspan         |\n+--------------+                             +\n| no span      |                             |\n+--------------+--------------+--------------+\n| rowspan      | colspan                     |\n+              +--------------+--------------+\n|              | no span      | no span      |\n+--------------+--------------+--------------+",
                "<div class=\"flexi-table flexi-table_type_unresponsive\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n<th class=\"flexi-table__header\">\nheader 3\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\nno span\n</td>\n<td class=\"flexi-table__data\" colspan=\"2\" rowspan=\"2\">\nrowspan and colspan\n</td>\n</tr>\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\nno span\n</td>\n</tr>\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\" rowspan=\"2\">\nrowspan\n</td>\n<td class=\"flexi-table__data\" colspan=\"2\">\ncolspan\n</td>\n</tr>\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\nno span\n</td>\n<td class=\"flexi-table__data\">\nno span\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec26(string extensions)
        {
            //     Start line number: 1298
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       |       cell 2 |
            //     +--------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">   cell 2
            //     </code></pre>
            //     </div>
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       |       cell 2 |\n+--------------+--------------+",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\n<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">   cell 2\n</code></pre>\n</div>\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec27(string extensions)
        {
            //     Start line number: 1354
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     +--------------+-----------------+--------------+
            //     | **header 1** | [header 2](url) | *header 3*   |
            //     +==============+=================+==============+
            //     | - cell 1     | ```             | > cell 3     |
            //     | - cell 2     | cell 2          | > cell 3     |
            //     | - cell 3     | ```             | > cell 3     |
            //     +--------------+-----------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     <strong>header 1</strong>
            //     </th>
            //     <th class="flexi-table__header">
            //     <a href="url">header 2</a>
            //     </th>
            //     <th class="flexi-table__header">
            //     <em>header 3</em>
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     <strong>header 1</strong>
            //     </div>
            //     <div class="flexi-table__content">
            //     <ul>
            //     <li>cell 1</li>
            //     <li>cell 2</li>
            //     <li>cell 3</li>
            //     </ul>
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     <a href="url">header 2</a>
            //     </div>
            //     <div class="flexi-table__content">
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title"></span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0V0z"/><path d="M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">cell 2
            //     </code></pre>
            //     </div>
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     <em>header 3</em>
            //     </div>
            //     <div class="flexi-table__content">
            //     <blockquote>
            //     <p>cell 3
            //     cell 3
            //     cell 3</p>
            //     </blockquote>
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("+--------------+-----------------+--------------+\n| **header 1** | [header 2](url) | *header 3*   |\n+==============+=================+==============+\n| - cell 1     | ```             | > cell 3     |\n| - cell 2     | cell 2          | > cell 3     |\n| - cell 3     | ```             | > cell 3     |\n+--------------+-----------------+--------------+",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\n<strong>header 1</strong>\n</th>\n<th class=\"flexi-table__header\">\n<a href=\"url\">header 2</a>\n</th>\n<th class=\"flexi-table__header\">\n<em>header 3</em>\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\n<strong>header 1</strong>\n</div>\n<div class=\"flexi-table__content\">\n<ul>\n<li>cell 1</li>\n<li>cell 2</li>\n<li>cell 3</li>\n</ul>\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\n<a href=\"url\">header 2</a>\n</div>\n<div class=\"flexi-table__content\">\n<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\"></span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0V0z\"/><path d=\"M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">cell 2\n</code></pre>\n</div>\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\n<em>header 3</em>\n</div>\n<div class=\"flexi-table__content\">\n<blockquote>\n<p>cell 3\ncell 3\ncell 3</p>\n</blockquote>\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec28(string extensions)
        {
            //     Start line number: 1431
            //     --------------- Markdown ---------------
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     This line ends the table
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>
            //     <p>This line ends the table</p>

            SpecTestHelper.AssertCompliance("+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\nThis line ends the table",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>\n<p>This line ends the table</p>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec29(string extensions)
        {
            //     Start line number: 1477
            //     --------------- Markdown ---------------
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+
            //     | This line invalidates the preceding table
            //     
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     + This line invalidates the preceding table
            //     
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+
            //     
            //     | This line does not invalidate the preceding table
            //     
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     
            //     + This line does not invalidate the preceding table
            //     --------------- Expected Markup ---------------
            //     <p>+--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+
            //     | This line invalidates the preceding table</p>
            //     <p>+--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |</p>
            //     <ul>
            //     <li>This line invalidates the preceding table</li>
            //     </ul>
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>
            //     <p>| This line does not invalidate the preceding table</p>
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>
            //     <ul>
            //     <li>This line does not invalidate the preceding table</li>
            //     </ul>

            SpecTestHelper.AssertCompliance("+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+--------------+--------------+\n| This line invalidates the preceding table\n\n+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+ This line invalidates the preceding table\n\n+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+--------------+--------------+\n\n| This line does not invalidate the preceding table\n\n+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n\n+ This line does not invalidate the preceding table",
                "<p>+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+--------------+--------------+\n| This line invalidates the preceding table</p>\n<p>+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |</p>\n<ul>\n<li>This line invalidates the preceding table</li>\n</ul>\n<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>\n<p>| This line does not invalidate the preceding table</p>\n<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>\n<ul>\n<li>This line does not invalidate the preceding table</li>\n</ul>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec30(string extensions)
        {
            //     Start line number: 1617
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{ "blockName": "table" }
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1   | cell 2   |
            //     --------------- Expected Markup ---------------
            //     <div class="table table_type_cards">
            //     <table class="table__table">
            //     <thead class="table__head">
            //     <tr class="table__row">
            //     <th class="table__header">
            //     header 1
            //     </th>
            //     <th class="table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="table__body">
            //     <tr class="table__row">
            //     <td class="table__data">
            //     <div class="table__label">
            //     header 1
            //     </div>
            //     <div class="table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="table__data">
            //     <div class="table__label">
            //     header 2
            //     </div>
            //     <div class="table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("@{ \"blockName\": \"table\" }\n| header 1 | header 2 |\n|----------|----------|\n| cell 1   | cell 2   |",
                "<div class=\"table table_type_cards\">\n<table class=\"table__table\">\n<thead class=\"table__head\">\n<tr class=\"table__row\">\n<th class=\"table__header\">\nheader 1\n</th>\n<th class=\"table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"table__body\">\n<tr class=\"table__row\">\n<td class=\"table__data\">\n<div class=\"table__label\">\nheader 1\n</div>\n<div class=\"table__content\">\ncell 1\n</div>\n</td>\n<td class=\"table__data\">\n<div class=\"table__label\">\nheader 2\n</div>\n<div class=\"table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec31(string extensions)
        {
            //     Start line number: 1671
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{ "type": "cards" }
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("@{ \"type\": \"cards\" }\n+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+--------------+--------------+",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec32(string extensions)
        {
            //     Start line number: 1718
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{ "type": "fixedTitles" }
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1   | cell 2   |
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_fixed-titles">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     cell 1
            //     </td>
            //     <td class="flexi-table__data">
            //     cell 2
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("@{ \"type\": \"fixedTitles\" }\n| header 1 | header 2 |\n|----------|----------|\n| cell 1   | cell 2   |",
                "<div class=\"flexi-table flexi-table_type_fixed-titles\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\ncell 1\n</td>\n<td class=\"flexi-table__data\">\ncell 2\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec33(string extensions)
        {
            //     Start line number: 1753
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{ "type": "unresponsive" }
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_unresponsive">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     cell 1
            //     </td>
            //     <td class="flexi-table__data">
            //     cell 2
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("@{ \"type\": \"unresponsive\" }\n+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+--------------+--------------+",
                "<div class=\"flexi-table flexi-table_type_unresponsive\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\ncell 1\n</td>\n<td class=\"flexi-table__data\">\ncell 2\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec34(string extensions)
        {
            //     Start line number: 1798
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "attributes": {
            //             "id" : "my-custom-id",
            //             "class" : "my-custom-class"
            //         }
            //     }
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1   | cell 2   |
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards my-custom-class" id="my-custom-id">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"attributes\": {\n        \"id\" : \"my-custom-id\",\n        \"class\" : \"my-custom-class\"\n    }\n}\n| header 1 | header 2 |\n|----------|----------|\n| cell 1   | cell 2   |",
                "<div class=\"flexi-table flexi-table_type_cards my-custom-class\" id=\"my-custom-id\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec35(string extensions)
        {
            //     Start line number: 1863
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiTableBlocks": {
            //             "defaultBlockOptions": {
            //                 "blockName": "table",
            //                 "type": "unresponsive",
            //                 "attributes": {
            //                     "class": "block"
            //                 }
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <div class="table table_type_unresponsive block">
            //     <table class="table__table">
            //     <thead class="table__head">
            //     <tr class="table__row">
            //     <th class="table__header">
            //     header 1
            //     </th>
            //     <th class="table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="table__body">
            //     <tr class="table__row">
            //     <td class="table__data">
            //     cell 1
            //     </td>
            //     <td class="table__data">
            //     cell 2
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+--------------+--------------+",
                "<div class=\"table table_type_unresponsive block\">\n<table class=\"table__table\">\n<thead class=\"table__head\">\n<tr class=\"table__row\">\n<th class=\"table__header\">\nheader 1\n</th>\n<th class=\"table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"table__body\">\n<tr class=\"table__row\">\n<td class=\"table__data\">\ncell 1\n</td>\n<td class=\"table__data\">\ncell 2\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false,
                "{\n    \"flexiTableBlocks\": {\n        \"defaultBlockOptions\": {\n            \"blockName\": \"table\",\n            \"type\": \"unresponsive\",\n            \"attributes\": {\n                \"class\": \"block\"\n            }\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiTableBlocks_OptionsBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec36(string extensions)
        {
            //     Start line number: 1909
            //     --------------- Extra Extensions ---------------
            //     OptionsBlocks
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiTableBlocks": {
            //             "defaultBlockOptions": {
            //                 "type": "unresponsive"
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1   | cell 2   |
            //     
            //     @{ "type": "cards" }
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_unresponsive">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     cell 1
            //     </td>
            //     <td class="flexi-table__data">
            //     cell 2
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("| header 1 | header 2 |\n|----------|----------|\n| cell 1   | cell 2   |\n\n@{ \"type\": \"cards\" }\n+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+--------------+--------------+",
                "<div class=\"flexi-table flexi-table_type_unresponsive\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\ncell 1\n</td>\n<td class=\"flexi-table__data\">\ncell 2\n</td>\n</tr>\n</tbody>\n</table>\n</div>\n<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false,
                "{\n    \"flexiTableBlocks\": {\n        \"defaultBlockOptions\": {\n            \"type\": \"unresponsive\"\n        }\n    }\n}");
        }
    }
}

