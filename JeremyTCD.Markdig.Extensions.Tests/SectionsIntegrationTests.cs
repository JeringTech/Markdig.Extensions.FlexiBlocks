using Markdig;
using Xunit;

namespace JeremyTCD.Markdig.Extensions.Tests
{
    public class SectionsIntegrationTests
    {
        [Fact]
        public void Test()
        {
            string markdown = "# foo\n## foo\n### foo";
            string expectedResult = "<h1>foo</h1>\n<section>\n<h2>foo</h2>\n<section>\n<h3>foo</h3>\n</section>\n</section>\n";

            MarkdownPipeline pipeline = new MarkdownPipelineBuilder().UseSections().Build();
            string result = Markdown.ToHtml(markdown, pipeline);

            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public void Test2()
        {
            //string markdown = "# foo\n## foo\n### foo\n#### foo\n##### foo\n###### foo";
            //string result =
            //Assert.Equal("test", "test");
        }
    }
}
