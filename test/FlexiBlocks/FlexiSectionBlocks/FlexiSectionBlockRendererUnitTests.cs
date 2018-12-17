using Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiSectionBlocks
{
    public class FlexiSectionBlockRendererUnitTests
    {
        // Can't use SerializableWrapper for FlexiSectionBlock - "null, whitespace or empty string" tests get serialized into the same parameters, which causes
        // xUnit to skip them.
        [Theory]
        [MemberData(nameof(WriteFlexiBlock_RendersFlexiSectionBlock_Data))]
        public void WriteFlexiBlock_RendersFlexiSectionBlock(FlexiSectionBlock dummyFlexiSectionBlock, string dummyHeadingContent, string expectedResult)
        {
            // Arrange
            var dummyContainerInline = new ContainerInline();
            dummyContainerInline.AppendChild(new LiteralInline(dummyHeadingContent));
            var dummyFlexiSectionHeadingBlock = new FlexiSectionHeadingBlock(null)
            {
                Inline = dummyContainerInline
            };
            dummyFlexiSectionBlock.Add(dummyFlexiSectionHeadingBlock);
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter); // Note that markdig changes dummyStringWriter.NewLine to '\n'
                var testSubject = new FlexiSectionBlockRenderer();

                // Act
                testSubject.Write(dummyHtmlRenderer, dummyFlexiSectionBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> WriteFlexiBlock_RendersFlexiSectionBlock_Data()
        {
            const int dummyLevel = 2;
            const string dummyIconMarkup = "dummyIconMarkup";
            const string dummyAttribute = "dummyAttribute";
            const string dummyAttributeValue = "dummyAttributeValue";
            const string dummyClass = "dummyClass";
            const string dummyID = "dummyID";
            const string dummyHeadingContent = "dummyHeadingContent";

            return new object[][]
            {
                // Writes attributes if specified
                new object[]
                {
                    new FlexiSectionBlock(null)
                    {
                        FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: null, attributes: new Dictionary<string, string>{
                            { dummyAttribute, dummyAttributeValue }
                        })
                    },
                    string.Empty,
                    $"<section {dummyAttribute}=\"{dummyAttributeValue}\">\n<header>\n<h0></h0>\n<button>\n</button>\n</header>\n</section>\n"
                },
                // Writes class if specified
                new object[]
                {
                    new FlexiSectionBlock(null)
                    {
                        FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: null)
                        {
                            Class = dummyClass
                        }
                    },
                    string.Empty,
                    $"<section class=\"{dummyClass}\">\n<header>\n<h0></h0>\n<button>\n</button>\n</header>\n</section>\n"
                },
                // Does not write class if Class is null, whitespace or an empty string
                new object[]
                {
                    new FlexiSectionBlock(null)
                    {
                        FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: null){ Class = null }
                    },
                    string.Empty,
                    "<section>\n<header>\n<h0></h0>\n<button>\n</button>\n</header>\n</section>\n"
                },
                new object[]
                {
                    new FlexiSectionBlock(null)
                    {
                        FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: null){ Class = " " }
                    },
                    string.Empty,
                    "<section>\n<header>\n<h0></h0>\n<button>\n</button>\n</header>\n</section>\n"
                },
                new object[]
                {
                    new FlexiSectionBlock(null)
                    {
                        FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: null){ Class = string.Empty }
                    },
                    string.Empty,
                    "<section>\n<header>\n<h0></h0>\n<button>\n</button>\n</header>\n</section>\n"
                },
                // Writes id if specified
                new object[]
                {
                    new FlexiSectionBlock(null)
                    {
                        FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: null),
                        ID = dummyID
                    },
                    string.Empty,
                    $"<section id=\"{dummyID}\">\n<header>\n<h0></h0>\n<button>\n</button>\n</header>\n</section>\n"
                },
                // Does not write id if ID is null, whitespace or an empty string
                new object[]
                {
                    new FlexiSectionBlock(null)
                    {
                        FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: null),
                        ID = null
                    },
                    string.Empty,
                    "<section>\n<header>\n<h0></h0>\n<button>\n</button>\n</header>\n</section>\n"
                },
                new object[]
                {
                    new FlexiSectionBlock(null)
                    {
                        FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: null),
                        ID = " "
                    },
                    string.Empty,
                    "<section>\n<header>\n<h0></h0>\n<button>\n</button>\n</header>\n</section>\n"
                },
                new object[]
                {
                    new FlexiSectionBlock(null)
                    {
                        FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: null),
                        ID = string.Empty
                    },
                    string.Empty,
                    "<section>\n<header>\n<h0></h0>\n<button>\n</button>\n</header>\n</section>\n"
                },
                // Renders expected SectioningContentElement
                new object[]
                {
                    new FlexiSectionBlock(null)
                    {
                        FlexiSectionBlockOptions = new FlexiSectionBlockOptions(SectioningContentElement.Article, linkIconMarkup: null)
                    },
                    string.Empty,
                    "<article>\n<header>\n<h0></h0>\n<button>\n</button>\n</header>\n</article>\n"
                },
                // Renders header content
                new object[]
                {
                    new FlexiSectionBlock(null)
                    {
                        FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: null)
                    },
                    dummyHeadingContent,
                    $"<section>\n<header>\n<h0>{dummyHeadingContent}</h0>\n<button>\n</button>\n</header>\n</section>\n"
                },
                // Renders expected level 
                new object[]
                {
                    new FlexiSectionBlock(null)
                    {
                        Level = dummyLevel,
                        FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: null)
                    },
                    string.Empty,
                    $"<section>\n<header>\n<h{dummyLevel}></h{dummyLevel}>\n<button>\n</button>\n</header>\n</section>\n"
                },
                // Renders link icon markup if specified
                new object[]
                {
                    new FlexiSectionBlock(null)
                    {
                        FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: dummyIconMarkup)
                    },
                    string.Empty,
                    $"<section>\n<header>\n<h0></h0>\n<button>\n{dummyIconMarkup}\n</button>\n</header>\n</section>\n"
                },
                // Does not render link icon markup if LinkIconMarkup is null, whitespace or an empty string
                new object[]
                {
                    new FlexiSectionBlock(null)
                    {
                        FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: null)
                    },
                    string.Empty,
                    "<section>\n<header>\n<h0></h0>\n<button>\n</button>\n</header>\n</section>\n"
                },
                new object[]
                {
                    new FlexiSectionBlock(null)
                    {
                        FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: " ")
                    },
                    string.Empty,
                    "<section>\n<header>\n<h0></h0>\n<button>\n</button>\n</header>\n</section>\n"
                },
                new object[]
                {
                    new FlexiSectionBlock(null)
                    {
                        FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: string.Empty)
                    },
                    string.Empty,
                    "<section>\n<header>\n<h0></h0>\n<button>\n</button>\n</header>\n</section>\n"
                }
            };
        }

        [Fact]
        public void WriteFlexiBlock_WritesChildren()
        {
            // Arrange
            var dummyFlexiSectionHeadingBlock = new FlexiSectionHeadingBlock(null);
            const string dummyChildText = "dummyChildText";
            var dummyContainerInline = new ContainerInline();
            dummyContainerInline.AppendChild(new LiteralInline(dummyChildText));
            var dummyParagraphBlock = new ParagraphBlock()
            {
                Inline = dummyContainerInline
            };
            var dummyFlexiSectionBlock = new FlexiSectionBlock(null)
            {
                FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: null)
            };
            dummyFlexiSectionBlock.Add(dummyFlexiSectionHeadingBlock);
            dummyFlexiSectionBlock.Add(dummyParagraphBlock);

            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter); // Note that markdig changes dummyStringWriter.NewLine to '\n'
                var testSubject = new FlexiSectionBlockRenderer();

                // Act
                testSubject.Write(dummyHtmlRenderer, dummyFlexiSectionBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            Assert.Equal($"<section>\n<header>\n<h0></h0>\n<button>\n</button>\n</header>\n<p>{dummyChildText}</p>\n</section>\n", result);
        }

        [Fact]
        public void WriteFlexiBlock_OnlyWritesChildrenIfEnableHtmlForBlockIsFalse()
        {
            // Arrange
            const string dummyChildText = "dummyChildText";
            var dummyContainerInline = new ContainerInline();
            dummyContainerInline.AppendChild(new LiteralInline(dummyChildText));
            var dummyParagraphBlock = new ParagraphBlock()
            {
                Inline = dummyContainerInline
            };
            var dummyFlexiSectionBlock = new FlexiSectionBlock(null)
            {
                FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: null)
            };
            dummyFlexiSectionBlock.Add(dummyParagraphBlock);

            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter)
                {
                    EnableHtmlForBlock = false
                };
                var testSubject = new FlexiSectionBlockRenderer();

                // Act
                testSubject.Write(dummyHtmlRenderer, dummyFlexiSectionBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            Assert.Equal(dummyChildText, result);
        }
    }
}
