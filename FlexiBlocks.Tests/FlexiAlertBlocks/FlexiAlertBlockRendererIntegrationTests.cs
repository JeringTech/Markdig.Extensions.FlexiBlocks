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
        public void Write_RendersAlert(SerializableWrapper<FlexiAlertBlock> dummyFlexiAlertBlockWrapper, string expectedResult)
        {
            // Arrange
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter); // Note that markdig changes dummyStringWriter.NewLine to '\n'
                var flexiAlertBlockRenderer = new FlexiAlertBlockRenderer();

                // Act
                flexiAlertBlockRenderer.Write(dummyHtmlRenderer, dummyFlexiAlertBlockWrapper.Value);
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
                    new SerializableWrapper<FlexiAlertBlock>(
                        new FlexiAlertBlock(null)
                        {
                            FlexiAlertBlockOptions = new FlexiAlertBlockOptions()
                            {
                                Attributes = new HtmlAttributeDictionary(){ { dummyAttribute, dummyAttributeValue } }
                            }
                        }
                    ),
                    $"<div {dummyAttribute}=\"{dummyAttributeValue}\">\n<div class=\"flexi-alert-content\">\n</div>\n</div>\n"
                },
                // Writes icon markup if specified
                new object[]
                {
                    new SerializableWrapper<FlexiAlertBlock>(
                        new FlexiAlertBlock(null)
                        {
                            FlexiAlertBlockOptions = new FlexiAlertBlockOptions(){IconMarkup = dummyIconMarkup}
                        }
                    ),
                    $"<div>\n{dummyIconMarkup}\n<div class=\"flexi-alert-content\">\n</div>\n</div>\n"
                },
            };
        }

        [Theory]
        [MemberData(nameof(Write_DoesNotRenderContentClassIfContentClassNameIsNullWhitespaceOrEmpty_Data))]
        public void Write_DoesNotRenderContentClassIfContentClassNameIsNullWhitespaceOrEmpty(string dummyContentClassName)
        {
            // Arrange
            var dummyFlexiAlertBlock = new FlexiAlertBlock(null)
            {
                FlexiAlertBlockOptions = new FlexiAlertBlockOptions() { ContentClassName = dummyContentClassName }
            };
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter); // Note that markdig changes dummyStringWriter.NewLine to '\n'
                var flexiAlertBlockRenderer = new FlexiAlertBlockRenderer();

                // Act
                flexiAlertBlockRenderer.Write(dummyHtmlRenderer, dummyFlexiAlertBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            Assert.Equal($"<div>\n<div>\n</div>\n</div>\n", result);
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
            var dummyFlexiAlertBlock = new FlexiAlertBlock(null)
            {
                FlexiAlertBlockOptions = new FlexiAlertBlockOptions()
            };
            dummyFlexiAlertBlock.Add(dummyParagraphBlock);

            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter); // Note that markdig changes dummyStringWriter.NewLine to '\n'
                var flexiAlertBlockRenderer = new FlexiAlertBlockRenderer();

                // Act
                flexiAlertBlockRenderer.Write(dummyHtmlRenderer, dummyFlexiAlertBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            Assert.Equal($"<div>\n<div class=\"flexi-alert-content\">\n<p>{dummyChildText}</p>\n</div>\n</div>\n", result);
        }
    }
}
