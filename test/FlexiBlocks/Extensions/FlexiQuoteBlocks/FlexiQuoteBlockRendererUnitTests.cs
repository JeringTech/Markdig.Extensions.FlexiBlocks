using Jering.Markdig.Extensions.FlexiBlocks.FlexiQuoteBlocks;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiQuoteBlocks
{
    public class FlexiQuoteBlockRendererUnitTests
    {
        [Fact]
        public void WriteBlock_OnlyWritesChildrenIfEnableHtmlForBlockIsFalse()
        {
            // Arrange
            const string dummyQuote = "dummyQuote";
            var dummyQuoteContainerInline = new ContainerInline();
            dummyQuoteContainerInline.AppendChild(new LiteralInline(dummyQuote));
            var dummyQuoteParagraphBlock = new ParagraphBlock() { Inline = dummyQuoteContainerInline };
            var dummyQuotePartBlock = new PlainContainerBlock(null);
            dummyQuotePartBlock.Add(dummyQuoteParagraphBlock);
            const string dummyCitation = "dummyCitation";
            var dummyCitationContainerInline = new ContainerInline();
            dummyCitationContainerInline.AppendChild(new LiteralInline(dummyCitation));
            var dummyCitationPartBlock = new PlainLeafBlock(null);
            dummyCitationPartBlock.Inline = dummyCitationContainerInline;
            FlexiQuoteBlock dummyFlexiQuoteBlock = CreateFlexiQuoteBlock(quotePart: dummyQuotePartBlock, citationPart: dummyCitationPartBlock);
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter)
            {
                EnableHtmlForBlock = false
            };
            ExposedFlexiQuoteBlockRenderer testSubject = CreateExposedFlexiQuoteBlockRenderer();

            // Act
            testSubject.ExposedWriteBlock(dummyHtmlRenderer, dummyFlexiQuoteBlock);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal($"{dummyQuote}\n{dummyCitation}\n", result, ignoreLineEndingDifferences: true);
        }

        [Theory]
        [MemberData(nameof(WriteBlock_WritesBlock_Data))]
        public void WriteBlock_WritesBlock(FlexiQuoteBlock dummyFlexiQuoteBlock,
            string expectedResult)
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
            ExposedFlexiQuoteBlockRenderer testSubject = CreateExposedFlexiQuoteBlockRenderer();

            // Act
            testSubject.ExposedWriteBlock(dummyHtmlRenderer, dummyFlexiQuoteBlock);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(expectedResult, result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> WriteBlock_WritesBlock_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string dummyCiteUrl = "dummyCiteUrl";
            const string dummyAttributeKey1 = "dummyAttributeKey1";
            const string dummyAttributeValue1 = "dummyAttributeValue1";
            const string dummyAttributeKey2 = "dummyAttributeKey2";
            const string dummyAttributeValue2 = "dummyAttributeValue2";
            const string dummyClass = "dummyClass";
            const string dummyIcon = "<dummyIcon></dummyIcon>";
            const string dummyIconWithClass = "<dummyIcon class=\"__icon\"></dummyIcon>";

            // Dummy quote part
            const string dummyQuote = "dummyQuote";
            var dummyQuoteContainerInline = new ContainerInline();
            dummyQuoteContainerInline.AppendChild(new LiteralInline(dummyQuote));
            var dummyQuoteParagraphBlock = new ParagraphBlock() { Inline = dummyQuoteContainerInline };
            var dummyQuotePartBlock = new PlainContainerBlock(null);
            dummyQuotePartBlock.Add(dummyQuoteParagraphBlock);

            // Dummy citation part
            const string dummyCitation = "dummyCitation";
            var dummyCitationContainerInline = new ContainerInline();
            dummyCitationContainerInline.AppendChild(new LiteralInline(dummyCitation));
            var dummyCitationPartBlock = new PlainLeafBlock(null);
            dummyCitationPartBlock.Inline = dummyCitationContainerInline;

            return new object[][]
            {
                // BlockName is assigned as a class of the root element and all default classes are prepended with it
                new object[]{
                    CreateFlexiQuoteBlock(dummyBlockName),
                    $@"<div class=""{dummyBlockName} {dummyBlockName}_no-icon"">
<div class=""{dummyBlockName}__content"">
<blockquote class=""{dummyBlockName}__blockquote"">
</blockquote>
<p class=""{dummyBlockName}__citation"">— </p>
</div>
</div>
"
                },
                // If icon is valid HTML, it is rendered with a default class and a _has-icon class is rendered
                new object[]{
                    CreateFlexiQuoteBlock(icon: dummyIcon),
                    $@"<div class="" _has-icon"">
{dummyIconWithClass}
<div class=""__content"">
<blockquote class=""__blockquote"">
</blockquote>
<p class=""__citation"">— </p>
</div>
</div>
"
                },
                // If icon is null, whitespace or an empty string, no icon is rendered and a _no-icon class is rendered (null case already verified in other tests)
                new object[]{
                    CreateFlexiQuoteBlock(icon: " "),
                    @"<div class="" _no-icon"">
<div class=""__content"">
<blockquote class=""__blockquote"">
</blockquote>
<p class=""__citation"">— </p>
</div>
</div>
"
                },
                new object[]{
                    CreateFlexiQuoteBlock(icon: string.Empty),
                    @"<div class="" _no-icon"">
<div class=""__content"">
<blockquote class=""__blockquote"">
</blockquote>
<p class=""__citation"">— </p>
</div>
</div>
"
                },
                // If attributes are specified, they're written
                new object[]{
                    CreateFlexiQuoteBlock(attributes: new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{ { dummyAttributeKey1, dummyAttributeValue1 }, { dummyAttributeKey2, dummyAttributeValue2 } })),
                    $@"<div class="" _no-icon"" {dummyAttributeKey1}=""{dummyAttributeValue1}"" {dummyAttributeKey2}=""{dummyAttributeValue2}"">
<div class=""__content"">
<blockquote class=""__blockquote"">
</blockquote>
<p class=""__citation"">— </p>
</div>
</div>
"
                },
                // If classes are specified, they're appended to default classes
                new object[]{
                    CreateFlexiQuoteBlock(attributes: new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{ { "class", dummyClass } })),
                    $@"<div class="" _no-icon {dummyClass}"">
<div class=""__content"">
<blockquote class=""__blockquote"">
</blockquote>
<p class=""__citation"">— </p>
</div>
</div>
"
                },
                // If CiteUrl is specified, it is assigned to the blockquote element's cite attribute
                new object[]{
                    CreateFlexiQuoteBlock(citeUrl: dummyCiteUrl),
                    $@"<div class="" _no-icon"">
<div class=""__content"">
<blockquote class=""__blockquote"" cite=""{dummyCiteUrl}"">
</blockquote>
<p class=""__citation"">— </p>
</div>
</div>
"
                },
                // If CiteUrl is null, whitespace or an empty string, no cite attribute is rendered for the blockquote element (null case already verified in other tests)
                new object[]{
                    CreateFlexiQuoteBlock(citeUrl: " "),
                    @"<div class="" _no-icon"">
<div class=""__content"">
<blockquote class=""__blockquote"">
</blockquote>
<p class=""__citation"">— </p>
</div>
</div>
"
                },
                new object[]{
                    CreateFlexiQuoteBlock(citeUrl: string.Empty),
                    @"<div class="" _no-icon"">
<div class=""__content"">
<blockquote class=""__blockquote"">
</blockquote>
<p class=""__citation"">— </p>
</div>
</div>
"
                },
                // Quote part is rendered
                new object[]{
                    CreateFlexiQuoteBlock(quotePart: dummyQuotePartBlock),
                    $@"<div class="" _no-icon"">
<div class=""__content"">
<blockquote class=""__blockquote"">
<p>{dummyQuote}</p>
</blockquote>
<p class=""__citation"">— </p>
</div>
</div>
"
                },
                // Citation part is rendered
                new object[]{
                    CreateFlexiQuoteBlock(citationPart: dummyCitationPartBlock),
                    $@"<div class="" _no-icon"">
<div class=""__content"">
<blockquote class=""__blockquote"">
</blockquote>
<p class=""__citation"">— {dummyCitation}</p>
</div>
</div>
"
                }
            };
        }

        public class ExposedFlexiQuoteBlockRenderer : FlexiQuoteBlockRenderer
        {
            public void ExposedWriteBlock(HtmlRenderer htmlRenderer, FlexiQuoteBlock flexiQuoteBlock)
            {
                WriteBlock(htmlRenderer, flexiQuoteBlock);
            }
        }

        private ExposedFlexiQuoteBlockRenderer CreateExposedFlexiQuoteBlockRenderer()
        {
            return new ExposedFlexiQuoteBlockRenderer();
        }

        private static FlexiQuoteBlock CreateFlexiQuoteBlock(string blockName = default,
            string icon = default,
            int citeLink = default,
            ReadOnlyDictionary<string, string> attributes = default,
            BlockParser blockParser = default,
            string citeUrl = default,
            PlainContainerBlock quotePart = default,
            PlainLeafBlock citationPart = default)
        {
            var result = new FlexiQuoteBlock(blockName, icon, citeLink, attributes, blockParser)
            {
                quotePart ?? new PlainContainerBlock(null), citationPart ?? new PlainLeafBlock(null)
            };

            result.CiteUrl = citeUrl;

            return result;
        }
    }
}
