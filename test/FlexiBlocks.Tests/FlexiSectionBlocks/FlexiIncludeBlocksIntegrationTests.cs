using Markdig;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiSectionBlocks
{
    // Integration tests that do not fit in amongst specs for this extension
    public class FlexiSectionBlocksIntegrationTests
    {
        [Fact]
        public void FlexiSectionBlocks_DoesNotAffectLeadingWhitespaceOfChildBlocks()
        {
            // Arrange
            var markdownPipelineBuilder = new MarkdownPipelineBuilder();
            markdownPipelineBuilder.
                UseFlexiSectionBlocks().
                UseFlexiCodeBlocks();
            MarkdownPipeline markdownPipeline = markdownPipelineBuilder.Build();

            const string dummyMarkdown = @"# foo
```
    Code with leading spaces
```";
            const string expectedHtml = @"<section class=""flexi-section-block-1"" id=""foo"">
<header>
<h1>foo</h1>
<button>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path d=""M0 0h24v24H0z"" fill=""none""/><path d=""M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z""/></svg>
</button>
</header>
<div class=""flexi-code-block"">
<header>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path fill=""none"" d=""M0 0h24v24H0V0z""/><path d=""M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z""/></svg>
</header>
<pre><code><span class=""line""><span class=""line-text"">    Code with leading spaces</span></span></code></pre>
</div>
</section>
";

            // Act
            string result = Markdown.ToHtml(dummyMarkdown, markdownPipeline);

            // Assert
            Assert.Equal(expectedHtml, result, ignoreLineEndingDifferences: true);
        }
    }
}
