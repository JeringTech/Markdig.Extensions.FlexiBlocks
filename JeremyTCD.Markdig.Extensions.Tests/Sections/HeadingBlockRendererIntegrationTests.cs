using JeremyTCD.Markdig.Extensions.Sections;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace JeremyTCD.Markdig.Extensions.Tests.Sections
{
    public class HeadingBlockRendererIntegrationTests
    {
        [Theory]
        [MemberData(nameof(Write_WritesHeaderAndHeading_Data))]
        public void Write_WritesHeaderAndHeading(string dummyChildText, int dummyLevel, string expectedResult)
        {
            // Arrange
            var dummyContainerInline = new ContainerInline();
            dummyContainerInline.AppendChild(new LiteralInline(dummyChildText));
            var dummyHeadingBlock = new HeadingBlock(null) { Level = dummyLevel, Inline = dummyContainerInline };
            var headingBlockRenderer = new HeadingBlockRenderer();

            string result = null;
            using (var stringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(stringWriter);

                // Act
                headingBlockRenderer.Write(dummyHtmlRenderer, dummyHeadingBlock);
                result = stringWriter.ToString();
            }

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> Write_WritesHeaderAndHeading_Data()
        {
            const string dummyChildText = "dummyChildText";

            return new object[][]
            {
                new object[]{ dummyChildText, 1, $"<header class=\"header-level-1\">\n<h1>{dummyChildText}</h1>\n</header>\n"},
                new object[]{ dummyChildText, 6, $"<header class=\"header-level-6\">\n<h6>{dummyChildText}</h6>\n</header>\n"}
            };
        }

        [Fact]
        public void Write_WritesIconMarkupIfSpecified()
        {
            // Arrange
            const string dummyIconMarkup = "dummyIconMarkup";
            const string dummyChildText = "dummyChildText";
            const int dummyLevel = 1;
            var dummyContainerInline = new ContainerInline();
            dummyContainerInline.AppendChild(new LiteralInline(dummyChildText));
            var dummyHeadingBlock = new HeadingBlock(null) { Level = dummyLevel, Inline = dummyContainerInline };
            dummyHeadingBlock.SetData(SectionBlockParser.ICON_MARKUP_KEY, dummyIconMarkup);
            var headingBlockRenderer = new HeadingBlockRenderer();

            string result = null;
            using (var stringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(stringWriter);

                // Act
                headingBlockRenderer.Write(dummyHtmlRenderer, dummyHeadingBlock);
                result = stringWriter.ToString();
            }

            // Assert
            Assert.Equal($"<header class=\"header-level-{dummyLevel}\">\n<h{dummyLevel}>{dummyChildText}</h{dummyLevel}>\n{dummyIconMarkup}\n</header>\n", result);
        }
    }
}