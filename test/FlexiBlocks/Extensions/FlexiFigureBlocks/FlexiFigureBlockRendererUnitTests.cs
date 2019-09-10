using Jering.Markdig.Extensions.FlexiBlocks.FlexiFigureBlocks;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiFigureBlocks
{
    public class FlexiFigureBlockRendererUnitTests
    {
        [Fact]
        public void WriteBlock_OnlyWritesChildrenIfEnableHtmlForBlockIsFalse()
        {
            // Arrange
            const string dummyContent = "dummyContent";
            var dummyContentContainerInline = new ContainerInline();
            dummyContentContainerInline.AppendChild(new LiteralInline(dummyContent));
            var dummyContentParagraphBlock = new ParagraphBlock() { Inline = dummyContentContainerInline };
            var dummyContentPartBlock = new PlainContainerBlock(null);
            dummyContentPartBlock.Add(dummyContentParagraphBlock);
            const string dummyCaption = "dummyCaption";
            var dummyCaptionContainerInline = new ContainerInline();
            dummyCaptionContainerInline.AppendChild(new LiteralInline(dummyCaption));
            var dummyCaptionPartBlock = new PlainLeafBlock(null);
            dummyCaptionPartBlock.Inline = dummyCaptionContainerInline;
            FlexiFigureBlock dummyFlexiFigureBlock = CreateFlexiFigureBlock(contentPart: dummyContentPartBlock, captionPart: dummyCaptionPartBlock);
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter)
            {
                EnableHtmlForBlock = false
            };
            ExposedFlexiFigureBlockRenderer testSubject = CreateExposedFlexiFigureBlockRenderer();

            // Act
            testSubject.ExposedWriteBlock(dummyHtmlRenderer, dummyFlexiFigureBlock);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal($"{dummyContent}\n{dummyCaption}\n", result, ignoreLineEndingDifferences: true);
        }

        [Theory]
        [MemberData(nameof(WriteBlock_WritesBlock_Data))]
        public void WriteBlock_WritesBlock(FlexiFigureBlock dummyFlexiFigureBlock,
            string expectedResult)
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
            ExposedFlexiFigureBlockRenderer testSubject = CreateExposedFlexiFigureBlockRenderer();

            // Act
            testSubject.ExposedWriteBlock(dummyHtmlRenderer, dummyFlexiFigureBlock);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(expectedResult, result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> WriteBlock_WritesBlock_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string dummyName = "dummyName";
            const string dummyAttributeKey1 = "dummyAttributeKey1";
            const string dummyAttributeValue1 = "dummyAttributeValue1";
            const string dummyAttributeKey2 = "dummyAttributeKey2";
            const string dummyAttributeValue2 = "dummyAttributeValue2";
            const string dummyClass = "dummyClass";
            const string dummyID = "dummyID";

            // Dummy content part
            const string dummyContent = "dummyContent";
            var dummyContentContainerInline = new ContainerInline();
            dummyContentContainerInline.AppendChild(new LiteralInline(dummyContent));
            var dummyContentParagraphBlock = new ParagraphBlock() { Inline = dummyContentContainerInline };
            var dummyContentPartBlock = new PlainContainerBlock(null);
            dummyContentPartBlock.Add(dummyContentParagraphBlock);

            // Dummy caption part
            const string dummyCaption = "dummyCaption";
            var dummyCaptionContainerInline = new ContainerInline();
            dummyCaptionContainerInline.AppendChild(new LiteralInline(dummyCaption));
            var dummyCaptionPartBlock = new PlainLeafBlock(null);
            dummyCaptionPartBlock.Inline = dummyCaptionContainerInline;

            return new object[][]
            {
                // BlockName is assigned as a class of the root element and all default classes are prepended with it
                new object[]{
                    CreateFlexiFigureBlock(dummyBlockName),
                    $@"<figure class=""{dummyBlockName} {dummyBlockName}_no_name"">
<div class=""{dummyBlockName}__content"">
</div>
<figcaption class=""{dummyBlockName}__caption""><span class=""{dummyBlockName}__name""></span></figcaption>
</figure>
"
                },
                // If attributes are specified, they're written
                new object[]{
                    CreateFlexiFigureBlock(attributes: new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{ { dummyAttributeKey1, dummyAttributeValue1 }, { dummyAttributeKey2, dummyAttributeValue2 } })),
                    $@"<figure class="" _no_name"" {dummyAttributeKey1}=""{dummyAttributeValue1}"" {dummyAttributeKey2}=""{dummyAttributeValue2}"">
<div class=""__content"">
</div>
<figcaption class=""__caption""><span class=""__name""></span></figcaption>
</figure>
"
                },
                // If classes are specified, they're appended to default classes
                new object[]{
                    CreateFlexiFigureBlock(attributes: new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{ { "class", dummyClass } })),
                    $@"<figure class="" _no_name {dummyClass}"">
<div class=""__content"">
</div>
<figcaption class=""__caption""><span class=""__name""></span></figcaption>
</figure>
"
                },
                // If ID (FlexiFigureBlock property) is specified, it is written
                new object[]{
                    CreateFlexiFigureBlock(id: dummyID),
                    $@"<figure class="" _no_name"" id=""{dummyID}"">
<div class=""__content"">
</div>
<figcaption class=""__caption""><span class=""__name""></span></figcaption>
</figure>
"
                },
                // ID in attributes is never written
                new object[]{
                    CreateFlexiFigureBlock(attributes: new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{ { "id", dummyID } })),
                    @"<figure class="" _no_name"">
<div class=""__content"">
</div>
<figcaption class=""__caption""><span class=""__name""></span></figcaption>
</figure>
"
                },
                // Content part is rendered
                new object[]{
                    CreateFlexiFigureBlock(contentPart: dummyContentPartBlock),
                    $@"<figure class="" _no_name"">
<div class=""__content"">
<p>{dummyContent}</p>
</div>
<figcaption class=""__caption""><span class=""__name""></span></figcaption>
</figure>
"
                },
                // If RenderName is true and Name is not null, whitespace or an empty string, Name is rendered in caption
                new object[]{
                    CreateFlexiFigureBlock(name: dummyName, renderName: true),
                    $@"<figure class="" _has_name"">
<div class=""__content"">
</div>
<figcaption class=""__caption""><span class=""__name"">{dummyName}. </span></figcaption>
</figure>
"
                },
                // If RenderName is false, Name isn't rendered even if Name is not null, whitespace or an empty string
                new object[]{
                    CreateFlexiFigureBlock(name: dummyName, renderName: false),
                    @"<figure class="" _no_name"">
<div class=""__content"">
</div>
<figcaption class=""__caption""><span class=""__name""></span></figcaption>
</figure>
"
                },
                // If Name is null, whitespace or an empty string, Name isn't is rendered even if RenderName is true (null case already verified in other tests)
                new object[]{
                    CreateFlexiFigureBlock(name: " ", renderName: true),
                    @"<figure class="" _no_name"">
<div class=""__content"">
</div>
<figcaption class=""__caption""><span class=""__name""></span></figcaption>
</figure>
"
                },
                new object[]{
                    CreateFlexiFigureBlock(name: string.Empty, renderName: true),
                    @"<figure class="" _no_name"">
<div class=""__content"">
</div>
<figcaption class=""__caption""><span class=""__name""></span></figcaption>
</figure>
"
                },
                // Caption part is rendered
                new object[]{
                    CreateFlexiFigureBlock(captionPart: dummyCaptionPartBlock),
                    $@"<figure class="" _no_name"">
<div class=""__content"">
</div>
<figcaption class=""__caption""><span class=""__name""></span>{dummyCaption}</figcaption>
</figure>
"
                }
            };
        }

        public class ExposedFlexiFigureBlockRenderer : FlexiFigureBlockRenderer
        {
            public void ExposedWriteBlock(HtmlRenderer htmlRenderer, FlexiFigureBlock flexiFigureBlock)
            {
                WriteBlock(htmlRenderer, flexiFigureBlock);
            }
        }

        private ExposedFlexiFigureBlockRenderer CreateExposedFlexiFigureBlockRenderer()
        {
            return new ExposedFlexiFigureBlockRenderer();
        }

        private static FlexiFigureBlock CreateFlexiFigureBlock(string blockName = default,
            string name = default,
            bool renderName = default,
            string autoLinkLabel = default,
            string id = default,
            ReadOnlyDictionary<string, string> attributes = default,
            BlockParser blockParser = default,
            PlainContainerBlock contentPart = default,
            PlainLeafBlock captionPart = default)
        {
            return new FlexiFigureBlock(blockName, name, renderName, autoLinkLabel, id, attributes, blockParser)
            {
                contentPart ?? new PlainContainerBlock(null), captionPart ?? new PlainLeafBlock(null)
            };
        }
    }
}
