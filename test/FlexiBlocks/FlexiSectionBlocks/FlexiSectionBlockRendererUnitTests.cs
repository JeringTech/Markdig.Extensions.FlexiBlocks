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
        public void WriteFlexiBlock_RendersFlexiSectionBlock(FlexiSectionBlock dummyFlexiSectionBlock, string expectedResult)
        {
            // Arrange
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter); // Note that markdig changes dummyStringWriter.NewLine to '\n'
                var flexiSectionBlockRenderer = new FlexiSectionBlockRenderer();

                // Act
                flexiSectionBlockRenderer.Write(dummyHtmlRenderer, dummyFlexiSectionBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> WriteFlexiBlock_RendersFlexiSectionBlock_Data()
        {
            const int dummyLevel = 2;
            const string dummyHeaderContent = "dummyHeaderContent";
            const string dummyIconMarkup = "dummyIconMarkup";
            const string dummyAttribute = "dummyAttribute";
            const string dummyAttributeValue = "dummyAttributeValue";
            const string dummyClass = "dummyClass";
            const string dummyID = "dummyID";

            return new object[][]
            {
                // Writes attributes if specified
                new object[]
                {
                    new FlexiSectionBlock(null)
                    {
                        FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: null, attributes: new Dictionary<string, string>{
                            { dummyAttribute, dummyAttributeValue }
                        }),
                    },
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
                    $"<section class=\"{dummyClass}\">\n<header>\n<h0></h0>\n<button>\n</button>\n</header>\n</section>\n"
                },
                // Does not write class if Class is null, whitespace or an empty string
                new object[]
                {
                    new FlexiSectionBlock(null)
                    {
                        FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: null){ Class = null }
                    },
                    "<section>\n<header>\n<h0></h0>\n<button>\n</button>\n</header>\n</section>\n"
                },
                new object[]
                {
                    new FlexiSectionBlock(null)
                    {
                        FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: null){ Class = " " }
                    },
                    "<section>\n<header>\n<h0></h0>\n<button>\n</button>\n</header>\n</section>\n"
                },
                new object[]
                {
                    new FlexiSectionBlock(null)
                    {
                        FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: null){ Class = string.Empty }
                    },
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
                    "<section>\n<header>\n<h0></h0>\n<button>\n</button>\n</header>\n</section>\n"
                },
                new object[]
                {
                    new FlexiSectionBlock(null)
                    {
                        FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: null),
                        ID = " "
                    },
                    "<section>\n<header>\n<h0></h0>\n<button>\n</button>\n</header>\n</section>\n"
                },
                new object[]
                {
                    new FlexiSectionBlock(null)
                    {
                        FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: null),
                        ID = string.Empty
                    },
                    "<section>\n<header>\n<h0></h0>\n<button>\n</button>\n</header>\n</section>\n"
                },
                // Renders expected SectioningContentElement
                new object[]
                {
                    new FlexiSectionBlock(null)
                    {
                        FlexiSectionBlockOptions = new FlexiSectionBlockOptions(SectioningContentElement.Article, linkIconMarkup: null)
                    },
                    "<article>\n<header>\n<h0></h0>\n<button>\n</button>\n</header>\n</article>\n"
                },
                // Renders header content
                new object[]
                {
                    new FlexiSectionBlock(null)
                    {
                        HeaderContent = dummyHeaderContent,
                        FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: null)
                    },
                    $"<section>\n<header>\n<h0>{dummyHeaderContent}</h0>\n<button>\n</button>\n</header>\n</section>\n"
                },
                // Renders expected level 
                new object[]
                {
                    new FlexiSectionBlock(null)
                    {
                        Level = dummyLevel,
                        FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: null)
                    },
                    $"<section>\n<header>\n<h{dummyLevel}></h{dummyLevel}>\n<button>\n</button>\n</header>\n</section>\n"
                },
                // Renders link icon markup if specified
                new object[]
                {
                    new FlexiSectionBlock(null)
                    {
                        FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: dummyIconMarkup)
                    },
                    $"<section>\n<header>\n<h0></h0>\n<button>\n{dummyIconMarkup}\n</button>\n</header>\n</section>\n"
                },
                // Does not render link icon markup if LinkIconMarkup is null, whitespace or an empty string
                new object[]
                {
                    new FlexiSectionBlock(null)
                    {
                        FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: null)
                    },
                    "<section>\n<header>\n<h0></h0>\n<button>\n</button>\n</header>\n</section>\n"
                },
                new object[]
                {
                    new FlexiSectionBlock(null)
                    {
                        FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: " ")
                    },
                    "<section>\n<header>\n<h0></h0>\n<button>\n</button>\n</header>\n</section>\n"
                },
                new object[]
                {
                    new FlexiSectionBlock(null)
                    {
                        FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: string.Empty)
                    },
                    "<section>\n<header>\n<h0></h0>\n<button>\n</button>\n</header>\n</section>\n"
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
            var dummyFlexiSectionBlock = new FlexiSectionBlock(null)
            {
                FlexiSectionBlockOptions = new FlexiSectionBlockOptions(linkIconMarkup: null)
            };
            dummyFlexiSectionBlock.Add(dummyParagraphBlock);

            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter); // Note that markdig changes dummyStringWriter.NewLine to '\n'
                var flexiSectionBlockRenderer = new FlexiSectionBlockRenderer();

                // Act
                flexiSectionBlockRenderer.Write(dummyHtmlRenderer, dummyFlexiSectionBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            Assert.Equal($"<section>\n<header>\n<h0></h0>\n<button>\n</button>\n</header>\n<p>{dummyChildText}</p>\n</section>\n", result);
        }
    }
}
