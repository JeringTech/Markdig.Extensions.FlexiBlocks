using Jering.Markdig.Extensions.FlexiBlocks.FlexiCardsBlocks;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiCardsBlocks
{
    public class FlexiCardsBlockRendererUnitTests
    {
        [Fact]
        public void WriteBlock_OnlyWritesChildrenIfEnableHtmlForBlockIsFalse()
        {
            // Arrange
            // Dummy title part
            const string dummyTitle = "dummyTitle";
            var dummyTitleContainerInline = new ContainerInline();
            dummyTitleContainerInline.AppendChild(new LiteralInline(dummyTitle));
            var dummyTitlePartBlock = new PlainLeafBlock(null);
            dummyTitlePartBlock.Inline = dummyTitleContainerInline;
            // Dummy content part
            const string dummyContent = "dummyContent";
            var dummyContentContainerInline = new ContainerInline();
            dummyContentContainerInline.AppendChild(new LiteralInline(dummyContent));
            var dummyContentParagraphBlock = new ParagraphBlock() { Inline = dummyContentContainerInline };
            var dummyContentPartBlock = new PlainContainerBlock(null);
            dummyContentPartBlock.Add(dummyContentParagraphBlock);
            // Dummy footnote part
            const string dummyFootnote = "dummyFootnote";
            var dummyFootnoteContainerInline = new ContainerInline();
            dummyFootnoteContainerInline.AppendChild(new LiteralInline(dummyFootnote));
            var dummyFootnotePartBlock = new PlainLeafBlock(null);
            dummyFootnotePartBlock.Inline = dummyFootnoteContainerInline;
            FlexiCardsBlock dummyFlexiCardsBlock = CreateFlexiCardsBlock(children: new List<FlexiCardBlock>{
                CreateFlexiCardBlock(titlePart: dummyTitlePartBlock, contentPart: dummyContentPartBlock, footnotePart: dummyFootnotePartBlock)
            });
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter)
            {
                EnableHtmlForBlock = false
            };
            ExposedFlexiCardsBlockRenderer testSubject = CreateExposedFlexiCardsBlockRenderer();

            // Act
            testSubject.ExposedWriteBlock(dummyHtmlRenderer, dummyFlexiCardsBlock);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal($"{dummyTitle}\n{dummyContent}\n{dummyFootnote}\n", result, ignoreLineEndingDifferences: true);
        }

        [Theory]
        [MemberData(nameof(WriteBlock_WritesBlock_Data))]
        public void WriteBlock_WritesBlock(FlexiCardsBlock dummyFlexiCardsBlock, string expectedResult)
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
            ExposedFlexiCardsBlockRenderer testSubject = CreateExposedFlexiCardsBlockRenderer();

            // Act
            testSubject.ExposedWriteBlock(dummyHtmlRenderer, dummyFlexiCardsBlock);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(expectedResult, result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> WriteBlock_WritesBlock_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const FlexiCardBlockSize dummyCardSize = FlexiCardBlockSize.Medium;
            const string dummyAttributeKey1 = "dummyAttributeKey1";
            const string dummyAttributeValue1 = "dummyAttributeValue1";
            const string dummyAttributeKey2 = "dummyAttributeKey2";
            const string dummyAttributeValue2 = "dummyAttributeValue2";
            const string dummyClass = "dummyClass";

            // Dummy title part
            const string dummyTitle = "dummyTitle";
            var dummyTitleContainerInline = new ContainerInline();
            dummyTitleContainerInline.AppendChild(new LiteralInline(dummyTitle));
            var dummyTitlePartBlock = new PlainLeafBlock(null);
            dummyTitlePartBlock.Inline = dummyTitleContainerInline;

            // Dummy content part
            const string dummyContent = "dummyContent";
            var dummyContentContainerInline = new ContainerInline();
            dummyContentContainerInline.AppendChild(new LiteralInline(dummyContent));
            var dummyContentParagraphBlock = new ParagraphBlock() { Inline = dummyContentContainerInline };
            var dummyContentPartBlock = new PlainContainerBlock(null);
            dummyContentPartBlock.Add(dummyContentParagraphBlock);

            // Dummy footnote part
            const string dummyFootnote = "dummyFootnote";
            var dummyFootnoteContainerInline = new ContainerInline();
            dummyFootnoteContainerInline.AppendChild(new LiteralInline(dummyFootnote));
            var dummyFootnotePartBlock = new PlainLeafBlock(null);
            dummyFootnotePartBlock.Inline = dummyFootnoteContainerInline;

            return new object[][]
            {
                // BlockName is assigned as a class of the root element and all default classes are prepended with it
                new object[]{
                    CreateFlexiCardsBlock(dummyBlockName),
                    $@"<div class=""{dummyBlockName} {dummyBlockName}_size_small"">
</div>
"
                },
                // CardSize is rendered in a modifier class of the root element
                new object[]{
                    CreateFlexiCardsBlock(cardSize: dummyCardSize),
                    $@"<div class="" _size_{dummyCardSize.ToString().ToLower()}"">
</div>
"
                },
                // If attributes are specified, they're written
                new object[]{
                    CreateFlexiCardsBlock(attributes: new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{ { dummyAttributeKey1, dummyAttributeValue1 }, { dummyAttributeKey2, dummyAttributeValue2 } })),
                    $@"<div class="" _size_small"" {dummyAttributeKey1}=""{dummyAttributeValue1}"" {dummyAttributeKey2}=""{dummyAttributeValue2}"">
</div>
"
                },
                // If classes are specified, they're appended to default classes
                new object[]{
                    CreateFlexiCardsBlock(attributes: new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{ { "class", dummyClass } })),
                    $@"<div class="" _size_small {dummyClass}"">
</div>
"
                },
                // Child FlexiCardBlocks are rendered
                new object[]{
                    CreateFlexiCardsBlock(children: new List<FlexiCardBlock>{
                        CreateFlexiCardBlock(titlePart: dummyTitlePartBlock, contentPart: dummyContentPartBlock, footnotePart: dummyFootnotePartBlock)
                    }),
                    $@"<div class="" _size_small"">
<div class=""__card __card_no_background-icon"">
<p class=""__card-title"">{dummyTitle}</p>
<div class=""__card-content"">
<p>{dummyContent}</p>
</div>
<p class=""__card-footnote"">{dummyFootnote}</p>
</div>
</div>
"
                }
            };
        }

        [Theory]
        [MemberData(nameof(WriteCard_WritesCard_Data))]
        public void WriteCard_WritesCard(FlexiCardBlock dummyFlexiCardBlock, string dummyBlockName, string expectedResult)
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
            FlexiCardsBlockRenderer testSubject = CreateFlexiCardsBlockRenderer();

            // Act
            testSubject.WriteCard(dummyHtmlRenderer, dummyFlexiCardBlock, dummyBlockName);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(expectedResult, result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> WriteCard_WritesCard_Data()
        {
            const string dummyUrl = "dummyUrl";
            const string dummyBlockName = "dummyBlockName";
            const string dummyAttributeKey1 = "dummyAttributeKey1";
            const string dummyAttributeValue1 = "dummyAttributeValue1";
            const string dummyAttributeKey2 = "dummyAttributeKey2";
            const string dummyAttributeValue2 = "dummyAttributeValue2";
            const string dummyClass = "dummyClass";
            const string dummyBackgroundIcon = "<dummyBackgroundIcon></dummyBackgroundIcon>";
            const string dummyBackgroundIconWithClass = "<dummyBackgroundIcon class=\"__card-background-icon\"></dummyBackgroundIcon>";

            // Dummy title part
            const string dummyTitle = "dummyTitle";
            var dummyTitleContainerInline = new ContainerInline();
            dummyTitleContainerInline.AppendChild(new LiteralInline(dummyTitle));
            var dummyTitlePartBlock = new PlainLeafBlock(null);
            dummyTitlePartBlock.Inline = dummyTitleContainerInline;

            // Dummy content part
            const string dummyContent = "dummyContent";
            var dummyContentContainerInline = new ContainerInline();
            dummyContentContainerInline.AppendChild(new LiteralInline(dummyContent));
            var dummyContentParagraphBlock = new ParagraphBlock() { Inline = dummyContentContainerInline };
            var dummyContentPartBlock = new PlainContainerBlock(null);
            dummyContentPartBlock.Add(dummyContentParagraphBlock);

            // Dummy footnote part
            const string dummyFootnote = "dummyFootnote";
            var dummyFootnoteContainerInline = new ContainerInline();
            dummyFootnoteContainerInline.AppendChild(new LiteralInline(dummyFootnote));
            var dummyFootnotePartBlock = new PlainLeafBlock(null);
            dummyFootnotePartBlock.Inline = dummyFootnoteContainerInline;

            return new object[][]
            {
                // Root element has tag name "a", has a href attribute and has an is_link modifier class if URL is specified
                new object[]{
                    CreateFlexiCardBlock(url: dummyUrl),
                    null,
                    $@"<a class=""__card __card_is_link __card_no_background-icon"" href=""{dummyUrl}"">
<p class=""__card-title""></p>
<div class=""__card-content"">
</div>
<p class=""__card-footnote""></p>
</a>
"
                },
                // Root element tag name is div if URL is null, whitespace or an empty string (null case already verified in other tests)
                new object[]{
                    CreateFlexiCardBlock(url: " "),
                    null,
                    @"<div class=""__card __card_no_background-icon"">
<p class=""__card-title""></p>
<div class=""__card-content"">
</div>
<p class=""__card-footnote""></p>
</div>
"
                },
                new object[]{
                    CreateFlexiCardBlock(url: string.Empty),
                    null,
                    @"<div class=""__card __card_no_background-icon"">
<p class=""__card-title""></p>
<div class=""__card-content"">
</div>
<p class=""__card-footnote""></p>
</div>
"
                },
                // BlockName prepended to all default classes
                new object[]{
                    CreateFlexiCardBlock(),
                    dummyBlockName,
                    $@"<div class=""{dummyBlockName}__card {dummyBlockName}__card_no_background-icon"">
<p class=""{dummyBlockName}__card-title""></p>
<div class=""{dummyBlockName}__card-content"">
</div>
<p class=""{dummyBlockName}__card-footnote""></p>
</div>
"
                },
                // If background icon is valid HTML, it is rendered with a default class and a _has_background-icon class is rendered
                new object[]{
                    CreateFlexiCardBlock(backgroundIcon: dummyBackgroundIcon),
                    null,
                    $@"<div class=""__card __card_has_background-icon"">
{dummyBackgroundIconWithClass}
<p class=""__card-title""></p>
<div class=""__card-content"">
</div>
<p class=""__card-footnote""></p>
</div>
"
                },
                // If background icon is null, whitespace or an empty string, no icon is rendered and a _no_background-icon class is rendered (null case already verified in other tests)
                new object[]{
                    CreateFlexiCardBlock(backgroundIcon: " "),
                    null,
                    @"<div class=""__card __card_no_background-icon"">
<p class=""__card-title""></p>
<div class=""__card-content"">
</div>
<p class=""__card-footnote""></p>
</div>
"
                },
                new object[]{
                    CreateFlexiCardBlock(backgroundIcon: string.Empty),
                    null,
                    @"<div class=""__card __card_no_background-icon"">
<p class=""__card-title""></p>
<div class=""__card-content"">
</div>
<p class=""__card-footnote""></p>
</div>
"
                },
                // If attributes are specified, they're written
                new object[]{
                    CreateFlexiCardBlock(attributes: new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{ { dummyAttributeKey1, dummyAttributeValue1 }, { dummyAttributeKey2, dummyAttributeValue2 } })),
                    null,
                    $@"<div class=""__card __card_no_background-icon"" {dummyAttributeKey1}=""{dummyAttributeValue1}"" {dummyAttributeKey2}=""{dummyAttributeValue2}"">
<p class=""__card-title""></p>
<div class=""__card-content"">
</div>
<p class=""__card-footnote""></p>
</div>
"
                },
                // If classes are specified, they're appended to default classes
                new object[]{
                    CreateFlexiCardBlock(attributes: new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{ { "class", dummyClass } })),
                    null,
                    $@"<div class=""__card __card_no_background-icon {dummyClass}"">
<p class=""__card-title""></p>
<div class=""__card-content"">
</div>
<p class=""__card-footnote""></p>
</div>
"
                },
                // Title part is rendered
                new object[]{
                    CreateFlexiCardBlock(titlePart: dummyTitlePartBlock),
                    null,
                    $@"<div class=""__card __card_no_background-icon"">
<p class=""__card-title"">{dummyTitle}</p>
<div class=""__card-content"">
</div>
<p class=""__card-footnote""></p>
</div>
"
                },
                // Content part is rendered
                new object[]{
                    CreateFlexiCardBlock(contentPart: dummyContentPartBlock),
                    null,
                    $@"<div class=""__card __card_no_background-icon"">
<p class=""__card-title""></p>
<div class=""__card-content"">
<p>{dummyContent}</p>
</div>
<p class=""__card-footnote""></p>
</div>
"
                },
                // Footnote part is rendered
                new object[]{
                    CreateFlexiCardBlock(footnotePart: dummyFootnotePartBlock),
                    null,
                    $@"<div class=""__card __card_no_background-icon"">
<p class=""__card-title""></p>
<div class=""__card-content"">
</div>
<p class=""__card-footnote"">{dummyFootnote}</p>
</div>
"
                }
            };
        }

        public class ExposedFlexiCardsBlockRenderer : FlexiCardsBlockRenderer
        {
            public void ExposedWriteBlock(HtmlRenderer htmlRenderer, FlexiCardsBlock flexiCardsBlock)
            {
                WriteBlock(htmlRenderer, flexiCardsBlock);
            }
        }

        private ExposedFlexiCardsBlockRenderer CreateExposedFlexiCardsBlockRenderer()
        {
            return new ExposedFlexiCardsBlockRenderer();
        }

        private FlexiCardsBlockRenderer CreateFlexiCardsBlockRenderer()
        {
            return new FlexiCardsBlockRenderer();
        }

        private static FlexiCardBlock CreateFlexiCardBlock(string url = default,
            string backgroundIcon = default,
            ReadOnlyDictionary<string, string> attributes = default,
            BlockParser blockParser = default,
            PlainLeafBlock titlePart = default,
            PlainContainerBlock contentPart = default,
            PlainLeafBlock footnotePart = default)
        {
            return new FlexiCardBlock(url, backgroundIcon, attributes, blockParser)
            {
                titlePart ?? new PlainLeafBlock(blockParser),
                contentPart ?? new PlainContainerBlock(blockParser),
                footnotePart ?? new PlainLeafBlock(blockParser),
            };
        }

        private static FlexiCardsBlock CreateFlexiCardsBlock(string blockName = default,
            FlexiCardBlockSize cardSize = default,
            ReadOnlyDictionary<string, string> attributes = default,
            BlockParser blockParser = default,
            List<FlexiCardBlock> children = default)
        {
            var result = new FlexiCardsBlock(blockName, cardSize, attributes, blockParser);

            if (children != null)
            {
                foreach(FlexiCardBlock child in children)
                {
                    result.Add(child);
                }
            }
            return result;
        }
    }
}
