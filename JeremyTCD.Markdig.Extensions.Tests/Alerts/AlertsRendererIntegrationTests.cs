using JeremyTCD.Markdig.Extensions.Alerts;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace JeremyTCD.Markdig.Extensions.Tests.Alerts
{
    public class AlertsRendererIntegrationTests
    {
        [Theory]
        [MemberData(nameof(Write_WritesAttributesAndIconMarkup_Data))]
        public void Write_WritesAttributesAndIconMarkup(AlertBlock dummyAlertBlock, string expectedResult)
        {
            // Arrange
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter); // Note that markdig changes dummyStringWriter.NewLine to '\n'
                var alertsRenderer = new AlertsRenderer();

                // Act
                alertsRenderer.Write(dummyHtmlRenderer, dummyAlertBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> Write_WritesAttributesAndIconMarkup_Data()
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
                            Attributes = new Dictionary<string, string>(){ { dummyAttribute, dummyAttributeValue } }
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
                }
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
                var alertsRenderer = new AlertsRenderer();

                // Act
                alertsRenderer.Write(dummyHtmlRenderer, dummyAlertBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            Assert.Equal($"<div>\n<p>{dummyChildText}</p>\n</div>\n", result);
        }
    }
}
