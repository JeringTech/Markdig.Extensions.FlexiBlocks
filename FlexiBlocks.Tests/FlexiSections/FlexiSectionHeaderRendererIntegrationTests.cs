using FlexiBlocks.Sections;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace FlexiBlocks.Tests.Sections
{
    public class FlexiSectionHeaderRendererIntegrationTests
    {
        [Theory]
        [MemberData(nameof(Write_WritesHeader_Data))]
        public void Write_WritesHeader(int dummyLevel)
        {
            // Arrange
            const string dummyChildText = "dummyChildText";
            const string dummyHeaderClassNameFormat = "dummyHeaderClassNameFormat-{0}";
            const string dummyHeaderIconMarkup = "dummyHeaderIconMarkup";
            var dummyContainerInline = new ContainerInline();
            dummyContainerInline.AppendChild(new LiteralInline(dummyChildText));
            var dummyHeadingBlock = new HeadingBlock(null) { Level = dummyLevel, Inline = dummyContainerInline };
            dummyHeadingBlock.SetData(FlexiSectionBlockParser.HEADER_CLASS_NAME_FORMAT_KEY, dummyHeaderClassNameFormat);
            dummyHeadingBlock.SetData(FlexiSectionBlockParser.HEADER_ICON_MARKUP_KEY, dummyHeaderIconMarkup);
            var sectionHeaderRenderer = new FlexiSectionHeaderBlockRenderer();

            string result = null;
            using (var stringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(stringWriter);

                // Act
                sectionHeaderRenderer.Write(dummyHtmlRenderer, dummyHeadingBlock);
                result = stringWriter.ToString();
            }

            // Assert
            Assert.Equal($"<header class=\"{string.Format(dummyHeaderClassNameFormat, dummyLevel)}\">\n<h{dummyLevel}>{dummyChildText}</h{dummyLevel}>\n{dummyHeaderIconMarkup}\n</header>\n", result);
        }

        public static IEnumerable<object[]> Write_WritesHeader_Data()
        {
            return new object[][]
            {
                new object[]{ 1 },
                new object[]{ 6 }
            };
        }

        [Theory]
        [MemberData(nameof(Write_DoesNotWriteHeaderClassIfHeaderClassNameFormatIsNullWhitespaceOrEmpty_Data))]
        public void Write_DoesNotWriteHeaderClassIfHeaderClassNameFormatIsNullWhitespaceOrEmpty(string dummyHeaderIconMarkup)
        {
            // Arrange
            const int dummyLevel = 1;
            const string dummyHeaderClassNameFormat = "dummyHeaderClassNameFormat-{0}";
            const string dummyChildText = "dummyChildText";
            var dummyContainerInline = new ContainerInline();
            dummyContainerInline.AppendChild(new LiteralInline(dummyChildText));
            var dummyHeadingBlock = new HeadingBlock(null) { Level = dummyLevel, Inline = dummyContainerInline };
            dummyHeadingBlock.SetData(FlexiSectionBlockParser.HEADER_CLASS_NAME_FORMAT_KEY, dummyHeaderClassNameFormat);
            dummyHeadingBlock.SetData(FlexiSectionBlockParser.HEADER_ICON_MARKUP_KEY, dummyHeaderIconMarkup);
            var sectionHeaderRenderer = new FlexiSectionHeaderBlockRenderer();

            string result = null;
            using (var stringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(stringWriter);

                // Act
                sectionHeaderRenderer.Write(dummyHtmlRenderer, dummyHeadingBlock);
                result = stringWriter.ToString();
            }

            // Assert
            Assert.Equal($"<header class=\"{string.Format(dummyHeaderClassNameFormat, dummyLevel)}\">\n<h{dummyLevel}>{dummyChildText}</h{dummyLevel}>\n</header>\n", result);
        }

        public static IEnumerable<object[]> Write_DoesNotWriteHeaderClassIfHeaderClassNameFormatIsNullWhitespaceOrEmpty_Data()
        {
            return new object[][]
            {
                new object[]{ null },
                new object[]{ " " },
                new object[]{ string.Empty }
            };
        }

        [Theory]
        [MemberData(nameof(Write_DoesNotWriteHeaderIconIfHeaderIconMarkupIsNullWhitespaceOrEmpty_Data))]
        public void Write_DoesNotWriteHeaderIconIfHeaderIconMarkupIsNullWhitespaceOrEmpty(string dummyHeaderClassNameFormat)
        {
            // Arrange
            const int dummyLevel = 1;
            const string dummyChildText = "dummyChildText";
            const string dummyHeaderIconMarkup = "dummyHeaderIconMarkup";
            var dummyContainerInline = new ContainerInline();
            dummyContainerInline.AppendChild(new LiteralInline(dummyChildText));
            var dummyHeadingBlock = new HeadingBlock(null) { Level = dummyLevel, Inline = dummyContainerInline };
            dummyHeadingBlock.SetData(FlexiSectionBlockParser.HEADER_CLASS_NAME_FORMAT_KEY, dummyHeaderClassNameFormat);
            dummyHeadingBlock.SetData(FlexiSectionBlockParser.HEADER_ICON_MARKUP_KEY, dummyHeaderIconMarkup);
            var sectionHeaderRenderer = new FlexiSectionHeaderBlockRenderer();

            string result = null;
            using (var stringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(stringWriter);

                // Act
                sectionHeaderRenderer.Write(dummyHtmlRenderer, dummyHeadingBlock);
                result = stringWriter.ToString();
            }

            // Assert
            Assert.Equal($"<header>\n<h{dummyLevel}>{dummyChildText}</h{dummyLevel}>\n{dummyHeaderIconMarkup}\n</header>\n", result);
        }

        public static IEnumerable<object[]> Write_DoesNotWriteHeaderIconIfHeaderIconMarkupIsNullWhitespaceOrEmpty_Data()
        {
            return new object[][]
            {
                new object[]{ null },
                new object[]{ " " },
                new object[]{ string.Empty }
            };
        }
    }
}