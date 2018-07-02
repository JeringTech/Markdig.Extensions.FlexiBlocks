using FlexiBlocks.Alerts;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace FlexiBlocks.Tests.Alerts
{
    public class FlexiAlertBlockRendererIntegrationTests
    {
        [Theory]
        [MemberData(nameof(Write_RendersAlert_Data))]
        public void Write_RendersAlert(FlexiAlertBlock dummyAlertBlock, string expectedResult)
        {
            // Arrange
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter); // Note that markdig changes dummyStringWriter.NewLine to '\n'
                var alertBlockRenderer = new FlexiAlertBlockRenderer();

                // Act
                alertBlockRenderer.Write(dummyHtmlRenderer, dummyAlertBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> Write_RendersAlert_Data()
        {
            const string dummyIconMarkup = "dummyIconMarkup";
            const string dummyAttribute = "dummyAttribute";
            const string dummyAttributeValue = "dummyAttributeValue";

            return new object[][]
            {
                // Writes attributes if specified
                new object[]
                {
                    new FlexiAlertBlock(null)
                    {
                        AlertBlockOptions = new FlexiAlertBlockOptions()
                        {
                            Attributes = new HtmlAttributeDictionary(){ { dummyAttribute, dummyAttributeValue } }
                        }
                    },
                    $"<div {dummyAttribute}=\"{dummyAttributeValue}\">\n</div>\n"
                },
                // Writes icon markup if specified
                new object[]
                {
                    new FlexiAlertBlock(null)
                    {
                        AlertBlockOptions = new FlexiAlertBlockOptions(){IconMarkup = dummyIconMarkup}
                    },
                    $"<div>\n{dummyIconMarkup}\n<div class=\"alert-content\">\n</div>\n</div>\n"
                },
            };
        }

        [Theory]
        [MemberData(nameof(Write_DoesNotRenderContentClassIfContentClassNameIsNullWhitespaceOrEmpty_Data))]
        public void Write_DoesNotRenderContentClassIfContentClassNameIsNullWhitespaceOrEmpty(string dummyContentClassName)
        {
            // Arrange
            const string dummyIconMarkup = "dummyIconMarkup";
            var dummyAlertBlock = new FlexiAlertBlock(null)
            {
                AlertBlockOptions = new FlexiAlertBlockOptions() { IconMarkup = dummyIconMarkup, ContentClassName = dummyContentClassName }
            };
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter); // Note that markdig changes dummyStringWriter.NewLine to '\n'
                var alertBlockRenderer = new FlexiAlertBlockRenderer();

                // Act
                alertBlockRenderer.Write(dummyHtmlRenderer, dummyAlertBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            Assert.Equal($"<div>\n{dummyIconMarkup}\n<div>\n</div>\n</div>\n", result);
        }

        public static IEnumerable<object[]> Write_DoesNotRenderContentClassIfContentClassNameIsNullWhitespaceOrEmpty_Data()
        {
            return new object[][]
            {
                new object[]{string.Empty},
                new object[]{" "},
                new object[]{null},
            };
        }

        [Fact]
        public void Write_WritesChildren()
        {
            // Arrange
            const string dummyChildText = "dummyChildText";
            var dummyContainerInline = new ContainerInline();
            dummyContainerInline.AppendChild(new LiteralInline(dummyChildText));
            var dummyParagraphBlock = new ParagraphBlock()
            {
                Inline = dummyContainerInline
            };
            var dummyAlertBlock = new FlexiAlertBlock(null)
            {
                AlertBlockOptions = new FlexiAlertBlockOptions()
            };
            dummyAlertBlock.Add(dummyParagraphBlock);

            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter); // Note that markdig changes dummyStringWriter.NewLine to '\n'
                var alertBlockRenderer = new FlexiAlertBlockRenderer();

                // Act
                alertBlockRenderer.Write(dummyHtmlRenderer, dummyAlertBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            Assert.Equal($"<div>\n<p>{dummyChildText}</p>\n</div>\n", result);
        }
    }
}
