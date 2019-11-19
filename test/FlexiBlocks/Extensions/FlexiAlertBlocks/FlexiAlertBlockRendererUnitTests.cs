using Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiAlertBlocks
{
    public class FlexiAlertBlockRendererUnitTests
    {
        [Fact]
        public void WriteBlock_OnlyWritesChildrenIfEnableHtmlForBlockIsFalse()
        {
            // Arrange
            const string dummyChildText = "dummyChildText";
            var dummyContainerInline = new ContainerInline();
            dummyContainerInline.AppendChild(new LiteralInline(dummyChildText));
            var dummyParagraphBlock = new ParagraphBlock() { Inline = dummyContainerInline };
            FlexiAlertBlock dummyFlexiAlertBlock = CreateFlexiAlertBlock();
            dummyFlexiAlertBlock.Add(dummyParagraphBlock);
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter)
            {
                EnableHtmlForBlock = false
            };
            ExposedFlexiAlertBlockRenderer testSubject = CreateExposedFlexiAlertBlockRenderer();

            // Act
            testSubject.ExposedWriteBlock(dummyHtmlRenderer, dummyFlexiAlertBlock);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(dummyChildText + "\n", result, ignoreLineEndingDifferences: true);
        }

        [Theory]
        [MemberData(nameof(WriteBlock_WritesBlock_Data))]
        public void WriteBlock_WritesBlock(FlexiAlertBlock dummyFlexiAlertBlock,
            string expectedResult)
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
            ExposedFlexiAlertBlockRenderer testSubject = CreateExposedFlexiAlertBlockRenderer();

            // Act
            testSubject.ExposedWriteBlock(dummyHtmlRenderer, dummyFlexiAlertBlock);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(expectedResult, result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> WriteBlock_WritesBlock_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string dummyType = "dummyType";
            const string dummyAttributeKey1 = "dummyAttributeKey1";
            const string dummyAttributeValue1 = "dummyAttributeValue1";
            const string dummyAttributeKey2 = "dummyAttributeKey2";
            const string dummyAttributeValue2 = "dummyAttributeValue2";
            const string dummyClass = "dummyClass";
            const string dummyIcon = "<dummyIcon></dummyIcon>";
            const string dummyIconWithClass = "<dummyIcon class=\"__icon\"></dummyIcon>";
            const string dummyChildText = "dummyChildText";

            var dummyChildContainerInline = new ContainerInline();
            dummyChildContainerInline.AppendChild(new LiteralInline(dummyChildText));
            var dummyChildBlock = new ParagraphBlock() { Inline = dummyChildContainerInline };
            FlexiAlertBlock dummyFlexiAlertBlockWithChild = CreateFlexiAlertBlock();
            dummyFlexiAlertBlockWithChild.Add(dummyChildBlock);

            return new object[][]
            {
                // BlockName is assigned as a class of the root element and all default classes are prepended with it
                new object[]{
                    CreateFlexiAlertBlock(dummyBlockName),
                    $@"<div class=""{dummyBlockName} {dummyBlockName}_type_ {dummyBlockName}_no-icon"">
<div class=""{dummyBlockName}__content"">
</div>
</div>
"
                },
                // If type is specified, a language modifier class is rendered
                new object[]{
                    CreateFlexiAlertBlock(type: dummyType),
                    $@"<div class="" _type_{dummyType} _no-icon"">
<div class=""__content"">
</div>
</div>
"
                },
                // If icon is valid HTML, it is rendered with a default class and a _has-icon class is rendered
                new object[]{
                    CreateFlexiAlertBlock(icon: dummyIcon),
                    $@"<div class="" _type_ _has-icon"">
{dummyIconWithClass}
<div class=""__content"">
</div>
</div>
"
                },
                // If icon is null, whitespace or an empty string, no icon is rendered and a _no-icon class is rendered (null case already verified in other tests)
                new object[]{
                    CreateFlexiAlertBlock(icon: " "),
                    @"<div class="" _type_ _no-icon"">
<div class=""__content"">
</div>
</div>
"
                },
                new object[]{
                    CreateFlexiAlertBlock(icon: string.Empty),
                    @"<div class="" _type_ _no-icon"">
<div class=""__content"">
</div>
</div>
"
                },
                // If attributes specified, they're written
                new object[]{
                    CreateFlexiAlertBlock(attributes: new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{ { dummyAttributeKey1, dummyAttributeValue1 }, { dummyAttributeKey2, dummyAttributeValue2 } })),
                    $@"<div class="" _type_ _no-icon"" {dummyAttributeKey1}=""{dummyAttributeValue1}"" {dummyAttributeKey2}=""{dummyAttributeValue2}"">
<div class=""__content"">
</div>
</div>
"
                },
                // If classes are specified, they're appended to default classes
                new object[]{
                    CreateFlexiAlertBlock(attributes: new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{ { "class", dummyClass } })),
                    $@"<div class="" _type_ _no-icon {dummyClass}"">
<div class=""__content"">
</div>
</div>
"
                },
                // Children are rendered
                new object[]{
                    dummyFlexiAlertBlockWithChild,
                    $@"<div class="" _type_ _no-icon"">
<div class=""__content"">
<p>{dummyChildText}</p>
</div>
</div>
"
                }
            };
        }

        public class ExposedFlexiAlertBlockRenderer : FlexiAlertBlockRenderer
        {
            public void ExposedWriteBlock(HtmlRenderer htmlRenderer, FlexiAlertBlock flexiAlertBlock)
            {
                WriteBlock(htmlRenderer, flexiAlertBlock);
            }
        }

        private ExposedFlexiAlertBlockRenderer CreateExposedFlexiAlertBlockRenderer()
        {
            return new ExposedFlexiAlertBlockRenderer();
        }

        private static FlexiAlertBlock CreateFlexiAlertBlock(string blockName = default,
            string type = default,
            string icon = default,
            ReadOnlyDictionary<string, string> attributes = default,
            BlockParser blockParser = default)
        {
            return new FlexiAlertBlock(blockName, type, icon, attributes, blockParser);
        }
    }
}
