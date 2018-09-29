using Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiAlertBlocks
{
    public class FlexiAlertBlockRendererUnitTests
    {
        [Theory]
        [MemberData(nameof(WriteFlexiBlock_RendersFlexiAlertBlock_Data))]
        public void WriteFlexiBlock_RendersFlexiAlertBlock(SerializableWrapper<FlexiAlertBlock> dummyFlexiAlertBlockWrapper, string expectedResult)
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

        public static IEnumerable<object[]> WriteFlexiBlock_RendersFlexiAlertBlock_Data()
        {
            const string dummyIconMarkup = "dummyIconMarkup";
            const string dummyAttribute = "dummyAttribute";
            const string dummyAttributeValue = "dummyAttributeValue";
            const string dummyClass = "dummyClass";

            return new object[][]
            {
                // Writes attributes if specified
                new object[]
                {
                    new SerializableWrapper<FlexiAlertBlock>(
                        new FlexiAlertBlock(null)
                        {
                            FlexiAlertBlockOptions = new FlexiAlertBlockOptions(attributes: new Dictionary<string, string>{
                                { dummyAttribute, dummyAttributeValue }
                            })
                        }
                    ),
                    $"<div {dummyAttribute}=\"{dummyAttributeValue}\" class=\"fab-info\">\n<div class=\"fab-content\">\n</div>\n</div>\n"
                },
                // Any value for the class attribute is prepended to the default class value
                new object[]
                {
                    new SerializableWrapper<FlexiAlertBlock>(
                        new FlexiAlertBlock(null)
                        {
                            FlexiAlertBlockOptions = new FlexiAlertBlockOptions(attributes: new Dictionary<string, string>{
                                { "class", dummyClass }
                            })
                        }
                    ),
                    $"<div class=\"{dummyClass} fab-info\">\n<div class=\"fab-content\">\n</div>\n</div>\n"
                },
                // Does not render default class if Class is null
                new object[]
                {
                    new SerializableWrapper<FlexiAlertBlock>(
                        new FlexiAlertBlock(null)
                        {
                            FlexiAlertBlockOptions = new FlexiAlertBlockOptions(classFormat: null)
                        }
                    ),
                    "<div>\n<div class=\"fab-content\">\n</div>\n</div>\n"
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
                    $"<div class=\"fab-info\">\n{dummyIconMarkup}\n<div class=\"fab-content\">\n</div>\n</div>\n"
                },
                // Does not render content class if ContentClass is null, whitespace or an empty string
                new object[]
                {
                    new SerializableWrapper<FlexiAlertBlock>(
                        new FlexiAlertBlock(null)
                        {
                            FlexiAlertBlockOptions = new FlexiAlertBlockOptions(contentClass: null)
                        }
                    ),
                    "<div class=\"fab-info\">\n<div>\n</div>\n</div>\n"
                },
                new object[]
                {
                    new SerializableWrapper<FlexiAlertBlock>(
                        new FlexiAlertBlock(null)
                        {
                            FlexiAlertBlockOptions = new FlexiAlertBlockOptions(contentClass: " ")
                        }
                    ),
                    "<div class=\"fab-info\">\n<div>\n</div>\n</div>\n"
                },
                new object[]
                {
                    new SerializableWrapper<FlexiAlertBlock>(
                        new FlexiAlertBlock(null)
                        {
                            FlexiAlertBlockOptions = new FlexiAlertBlockOptions(contentClass: string.Empty)
                        }
                    ),
                    "<div class=\"fab-info\">\n<div>\n</div>\n</div>\n"
                },
            };
        }

        [Fact]
        public void WriteFlexiBlock_WritesChildren()
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
            Assert.Equal($"<div class=\"fab-info\">\n<div class=\"fab-content\">\n<p>{dummyChildText}</p>\n</div>\n</div>\n", result);
        }
    }
}
