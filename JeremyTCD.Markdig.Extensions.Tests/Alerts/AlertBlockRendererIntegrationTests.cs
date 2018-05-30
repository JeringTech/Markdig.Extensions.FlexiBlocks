using JeremyTCD.Markdig.Extensions.Alerts;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace JeremyTCD.Markdig.Extensions.Tests.Alerts
{
    public class AlertBlockRendererIntegrationTests
    {
        [Theory]
        [MemberData(nameof(Write_RendersAlert_Data))]
        public void Write_RendersAlert(AlertBlock dummyAlertBlock, string expectedResult)
        {
            // Arrange
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter); // Note that markdig changes dummyStringWriter.NewLine to '\n'
                var alertBlockRenderer = new AlertBlockRenderer();

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
                    new AlertBlock(null)
                    {
                        AlertBlockOptions = new AlertBlockOptions()
                        {
                            Attributes = new HtmlAttributeDictionary(){ { dummyAttribute, dummyAttributeValue } }
                        }
                    },
                    $"<div {dummyAttribute}=\"{dummyAttributeValue}\">\n</div>\n"
                },
                // Writes icon markup if specified
                new object[]
                {
                    new AlertBlock(null)
                    {
                        AlertBlockOptions = new AlertBlockOptions(){IconMarkup = dummyIconMarkup}
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
            var dummyAlertBlock = new AlertBlock(null)
            {
                AlertBlockOptions = new AlertBlockOptions() { IconMarkup = dummyIconMarkup, ContentClassName = dummyContentClassName }
            };
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter); // Note that markdig changes dummyStringWriter.NewLine to '\n'
                var alertBlockRenderer = new AlertBlockRenderer();

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
            var dummyAlertBlock = new AlertBlock(null)
            {
                AlertBlockOptions = new AlertBlockOptions()
            };
            dummyAlertBlock.Add(dummyParagraphBlock);

            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter); // Note that markdig changes dummyStringWriter.NewLine to '\n'
                var alertBlockRenderer = new AlertBlockRenderer();

                // Act
                alertBlockRenderer.Write(dummyHtmlRenderer, dummyAlertBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            Assert.Equal($"<div>\n<p>{dummyChildText}</p>\n</div>\n", result);
        }
    }
}
