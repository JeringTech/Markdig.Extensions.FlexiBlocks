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
            const string expectedHtml = @"<section class=""flexi-section flexi-section_level_1 flexi-section_has-link-icon"" id=""foo"">
<header class=""flexi-section__header"">
<h1 class=""flexi-section__heading"">foo</h1>
<button class=""flexi-section__link-button"" title=""Copy link"" aria-label=""Copy link"">
<svg class=""flexi-section__link-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path d=""M0 0h24v24H0z"" fill=""none""/><path d=""M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z""/></svg>
</button>
</header>
<div class=""flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases"">
<header class=""flexi-code__header"">
<span class=""flexi-code__title""></span>
<button class=""flexi-code__copy-button"" title=""Copy code"" aria-label=""Copy code"">
<svg class=""flexi-code__copy-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path fill=""none"" d=""M0 0h24v24H0V0z""/><path d=""M16 1H2v16h2V3h12V1zm-1 4l6 6v12H6V5h9zm-1 7h5.5L14 6.5V12z""/></svg>
</button>
</header>
<pre class=""flexi-code__pre""><code class=""flexi-code__code"">    Code with leading spaces
</code></pre>
</div>
</section>
";

            // Act
            string result = Markdown.ToHtml(dummyMarkdown, markdownPipeline);

            // Assert
            Assert.Equal(expectedHtml, result, ignoreLineEndingDifferences: true);
        }

        // Refer to the commit that this test was added in for its full context
        [Fact]
        public void FlexiSectionBlocks_ArentAffectedByPrecedingListBlocks()
        {
            // Arrange
            var markdownPipelineBuilder = new MarkdownPipelineBuilder();
            markdownPipelineBuilder.
                UseFlexiSectionBlocks().
                UseFlexiCodeBlocks();
            MarkdownPipeline markdownPipeline = markdownPipelineBuilder.Build();

            const string dummyMarkdown = @"# This heading is followed by a list
- list item 1
- list item 2

# This heading is preceded by a list";
            const string expectedHtml = @"<section class=""flexi-section flexi-section_level_1 flexi-section_has-link-icon"" id=""this-heading-is-followed-by-a-list"">
<header class=""flexi-section__header"">
<h1 class=""flexi-section__heading"">This heading is followed by a list</h1>
<button class=""flexi-section__link-button"" title=""Copy link"" aria-label=""Copy link"">
<svg class=""flexi-section__link-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path d=""M0 0h24v24H0z"" fill=""none""/><path d=""M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z""/></svg>
</button>
</header>
<ul>
<li>list item 1</li>
<li>list item 2</li>
</ul>
</section>
<section class=""flexi-section flexi-section_level_1 flexi-section_has-link-icon"" id=""this-heading-is-preceded-by-a-list"">
<header class=""flexi-section__header"">
<h1 class=""flexi-section__heading"">This heading is preceded by a list</h1>
<button class=""flexi-section__link-button"" title=""Copy link"" aria-label=""Copy link"">
<svg class=""flexi-section__link-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path d=""M0 0h24v24H0z"" fill=""none""/><path d=""M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z""/></svg>
</button>
</header>
</section>
";

            // Act
            string result = Markdown.ToHtml(dummyMarkdown, markdownPipeline);

            // Assert
            Assert.Equal(expectedHtml, result, ignoreLineEndingDifferences: true);
        }
    }
}
