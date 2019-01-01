using Markdig;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiCodeBlocks
{
    // Integration tests that do not fit in amongst specs for this extension
    public class FlexiCodeBlocksIntegrationTests
    {
        [Fact]
        public void FlexiCodeBlocks_EscapesSpecialCharactersWithCharacterReferencesWhenHighlightingUsingPrism()
        {
            // Arrange
            var markdownPipelineBuilder = new MarkdownPipelineBuilder();
            markdownPipelineBuilder.
                UseFlexiOptionsBlocks().
                UseFlexiCodeBlocks();
            MarkdownPipeline markdownPipeline = markdownPipelineBuilder.Build();

            const string dummyMarkdown = @"@{""language"":""html""}
```
<div>
&
</div>
```";
            const string expectedHtml = @"<div class=""flexi-code-block"">
<header>
<button>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path fill=""none"" d=""M0 0h24v24H0V0z""/><path d=""M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z""/></svg>
</button>
</header>
<pre><code class=""language-html""><span class=""line""><span class=""line-text""><span class=""token tag""><span class=""token tag""><span class=""token punctuation"">&lt;</span>div</span><span class=""token punctuation"">></span></span></span></span>
<span class=""line""><span class=""line-text"">&amp;</span></span>
<span class=""line""><span class=""line-text""><span class=""token tag""><span class=""token tag""><span class=""token punctuation"">&lt;/</span>div</span><span class=""token punctuation"">></span></span></span></span></code></pre>
</div>
";

            // Act
            string result = Markdown.ToHtml(dummyMarkdown, markdownPipeline);

            // Assert
            Assert.Equal(expectedHtml, result, ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void FlexiCodeBlocks_EscapesSpecialCharactersWithCharacterReferencesWhenHighlightingUsingHighlightJS()
        {
            // Arrange
            var markdownPipelineBuilder = new MarkdownPipelineBuilder();
            markdownPipelineBuilder.
                UseFlexiOptionsBlocks().
                UseFlexiCodeBlocks();
            MarkdownPipeline markdownPipeline = markdownPipelineBuilder.Build();

            const string dummyMarkdown = @"@{""language"":""html"", ""syntaxHighlighter"":""highlightjs""}
```
<div>
&
</div>
```";
            const string expectedHtml = @"<div class=""flexi-code-block"">
<header>
<button>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path fill=""none"" d=""M0 0h24v24H0V0z""/><path d=""M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z""/></svg>
</button>
</header>
<pre><code class=""language-html""><span class=""line""><span class=""line-text""><span class=""hljs-tag"">&lt;<span class=""hljs-name"">div</span>&gt;</span></span></span>
<span class=""line""><span class=""line-text"">&amp;</span></span>
<span class=""line""><span class=""line-text""><span class=""hljs-tag"">&lt;/<span class=""hljs-name"">div</span>&gt;</span></span></span></code></pre>
</div>
";

            // Act
            string result = Markdown.ToHtml(dummyMarkdown, markdownPipeline);

            // Assert
            Assert.Equal(expectedHtml, result, ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void FlexiCodeBlocks_SplitsUpMultiLineElementsBeforeApplyingLineEmbellishments()
        {
            // Arrange
            var markdownPipelineBuilder = new MarkdownPipelineBuilder();
            markdownPipelineBuilder.
                UseFlexiOptionsBlocks().
                UseFlexiCodeBlocks();
            MarkdownPipeline markdownPipeline = markdownPipelineBuilder.Build();

            const string dummyMarkdown = @"@{
    ""language"": ""css""
}
```
/*
    Multi-line elements
    get split up
    to facilitate
    line embellishing
*/
```";
            const string expectedHtml = @"<div class=""flexi-code-block"">
<header>
<button>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path fill=""none"" d=""M0 0h24v24H0V0z""/><path d=""M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z""/></svg>
</button>
</header>
<pre><code class=""language-css""><span class=""line""><span class=""line-text""><span class=""token comment"">/*</span></span></span>
<span class=""line""><span class=""line-text""><span class=""token comment"">    Multi-line elements</span></span></span>
<span class=""line""><span class=""line-text""><span class=""token comment"">    get split up</span></span></span>
<span class=""line""><span class=""line-text""><span class=""token comment"">    to facilitate</span></span></span>
<span class=""line""><span class=""line-text""><span class=""token comment"">    line embellishing</span></span></span>
<span class=""line""><span class=""line-text""><span class=""token comment"">*/</span></span></span></code></pre>
</div>
";

            // Act
            string result = Markdown.ToHtml(dummyMarkdown, markdownPipeline);

            // Assert
            Assert.Equal(expectedHtml, result, ignoreLineEndingDifferences: true);
        }
    }
}
