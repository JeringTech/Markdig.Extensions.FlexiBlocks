using JeremyTCD.Markdig.Extensions.Sections;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace JeremyTCD.Markdig.Extensions.Tests.Sections
{
    public class SectionsRendererIntegrationTests
    {
        [Fact]
        public void Write_WritesWrapperAttributesAndChildren()
        {
            // Arrange
            const string dummyChildText = "dummyChildText";
            const string dummyAttributeName = "dummyAttributeName";
            const string dummyAttributeValue = "dummyAttributeValue";
            const SectioningContentElement dummySectioningContentElement = SectioningContentElement.Article;
            var dummyContainerInline = new ContainerInline();
            dummyContainerInline.AppendChild(new LiteralInline(dummyChildText));
            var dummyParagraphBlock = new ParagraphBlock()
            {
                Inline = dummyContainerInline
            };
            var dummySectionBlock = new SectionBlock(null)
            {
                Level = 1,
                SectionBlockOptions = new SectionBlockOptions()
                {
                    Level1WrapperElement = dummySectioningContentElement,
                    Attributes = new Dictionary<string, string>() { { dummyAttributeName, dummyAttributeValue } }
                }
            };
            dummySectionBlock.Add(dummyParagraphBlock);
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter); // Note that markdig changes dummyStringWriter.NewLine to '\n'
                var sectionsRenderer = new SectionsRenderer();

                // Act
                sectionsRenderer.Write(dummyHtmlRenderer, dummySectionBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            string expectedResultElementName = dummySectioningContentElement.ToString().ToLower();
            Assert.Equal($"<{expectedResultElementName} {dummyAttributeName}=\"{dummyAttributeValue}\">\n<p>{dummyChildText}</p>\n</{expectedResultElementName}>\n", result);
        }

        [Theory]
        [MemberData(nameof(Write_UsesLevel2PlusWrapperElementIfSectionBlockIsLevel2Plus_Data))]
        public void Write_UsesLevel2PlusWrapperElementIfSectionBlockIsLevel2Plus(int level)
        {
            // Arrange
            const SectioningContentElement dummySectioningContentElement = SectioningContentElement.Nav;
            var dummySectionBlock = new SectionBlock(null)
            {
                Level = level,
                SectionBlockOptions = new SectionBlockOptions()
                {
                    Level2PlusWrapperElement = dummySectioningContentElement
                }
            };
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter); // Note that markdig changes dummyStringWriter.NewLine to '\n'
                var sectionsRenderer = new SectionsRenderer();

                // Act
                sectionsRenderer.Write(dummyHtmlRenderer, dummySectionBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            string expectedResultElementName = dummySectioningContentElement.ToString().ToLower();
            Assert.Equal($"<{expectedResultElementName}>\n</{expectedResultElementName}>\n", result);
        }

        public static IEnumerable<object[]> Write_UsesLevel2PlusWrapperElementIfSectionBlockIsLevel2Plus_Data()
        {
            return new object[][]
            {
                new object[]{2},
                new object[]{3}
            };
        }

        [Fact]
        public void Write_OnlyWritesChildrenIfWrapperElementIsNone()
        {
            // Arrange
            const string dummyChildText = "dummyChildText";
            var dummyContainerInline = new ContainerInline();
            dummyContainerInline.AppendChild(new LiteralInline(dummyChildText));
            var dummyParagraphBlock = new ParagraphBlock()
            {
                Inline = dummyContainerInline
            };
            var dummySectionBlock = new SectionBlock(null)
            {
                Level = 1,
                SectionBlockOptions = new SectionBlockOptions()
                {
                    Level1WrapperElement = SectioningContentElement.None
                }
            };
            dummySectionBlock.Add(dummyParagraphBlock);
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter); // Note that markdig changes dummyStringWriter.NewLine to '\n'
                var sectionsRenderer = new SectionsRenderer();

                // Act
                sectionsRenderer.Write(dummyHtmlRenderer, dummySectionBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            Assert.Equal($"<p>{dummyChildText}</p>\n", result);
        }
    }
}
