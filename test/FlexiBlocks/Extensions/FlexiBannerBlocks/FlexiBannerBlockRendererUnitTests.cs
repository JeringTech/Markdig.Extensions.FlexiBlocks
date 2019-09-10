using Jering.Markdig.Extensions.FlexiBlocks.FlexiBannerBlocks;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Syntax.Inlines;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiBannerBlocks
{
    public class FlexiBannerBlockRendererUnitTests
    {
        [Fact]
        public void WriteBlock_OnlyWritesChildrenIfEnableHtmlForBlockIsFalse()
        {
            // Arrange
            const string dummyTitle = "dummyTitle";
            var dummyTitleContainerInline = new ContainerInline();
            dummyTitleContainerInline.AppendChild(new LiteralInline(dummyTitle));
            var dummyTitlePartBlock = new PlainLeafBlock(null);
            dummyTitlePartBlock.Inline = dummyTitleContainerInline;
            const string dummyBlurb = "dummyBlurb";
            var dummyBlurbContainerInline = new ContainerInline();
            dummyBlurbContainerInline.AppendChild(new LiteralInline(dummyBlurb));
            var dummyBlurbPartBlock = new PlainLeafBlock(null);
            dummyBlurbPartBlock.Inline = dummyBlurbContainerInline;
            FlexiBannerBlock dummyFlexiBannerBlock = CreateFlexiBannerBlock(titlePart: dummyTitlePartBlock, blurbPart: dummyBlurbPartBlock);
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter)
            {
                EnableHtmlForBlock = false
            };
            ExposedFlexiBannerBlockRenderer testSubject = CreateExposedFlexiBannerBlockRenderer();

            // Act
            testSubject.ExposedWriteBlock(dummyHtmlRenderer, dummyFlexiBannerBlock);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal($"{dummyTitle}\n{dummyBlurb}\n", result, ignoreLineEndingDifferences: true);
        }

        [Theory]
        [MemberData(nameof(WriteBlock_WritesBlock_Data))]
        public void WriteBlock_WritesBlock(FlexiBannerBlock dummyFlexiBannerBlock,
            string expectedResult)
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
            ExposedFlexiBannerBlockRenderer testSubject = CreateExposedFlexiBannerBlockRenderer();

            // Act
            testSubject.ExposedWriteBlock(dummyHtmlRenderer, dummyFlexiBannerBlock);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(expectedResult, result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> WriteBlock_WritesBlock_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string dummyAttributeKey1 = "dummyAttributeKey1";
            const string dummyAttributeValue1 = "dummyAttributeValue1";
            const string dummyAttributeKey2 = "dummyAttributeKey2";
            const string dummyAttributeValue2 = "dummyAttributeValue2";
            const string dummyClass = "dummyClass";
            const string dummyLogoIcon = "<dummyLogoIcon></dummyLogoIcon>";
            const string dummyLogoIconWithClass = "<dummyLogoIcon class=\"__logo-icon\"></dummyLogoIcon>";
            const string dummyBackgroundIcon = "<dummyBackgroundIcon></dummyBackgroundIcon>";
            const string dummyBackgroundIconWithClass = "<dummyBackgroundIcon class=\"__background-icon\"></dummyBackgroundIcon>";

            // Dummy title part
            const string dummyTitle = "dummyTitle";
            var dummyTitleContainerInline = new ContainerInline();
            dummyTitleContainerInline.AppendChild(new LiteralInline(dummyTitle));
            var dummyTitlePartBlock = new PlainLeafBlock(null);
            dummyTitlePartBlock.Inline = dummyTitleContainerInline;

            // Dummy blurb part
            const string dummyBlurb = "dummyBlurb";
            var dummyBlurbContainerInline = new ContainerInline();
            dummyBlurbContainerInline.AppendChild(new LiteralInline(dummyBlurb));
            var dummyBlurbPartBlock = new PlainLeafBlock(null);
            dummyBlurbPartBlock.Inline = dummyBlurbContainerInline;

            return new object[][]
            {
                // BlockName is assigned as a class of the root element and all default classes are prepended with it
                new object[]{
                    CreateFlexiBannerBlock(dummyBlockName),
                    $@"<div class=""{dummyBlockName} {dummyBlockName}_no_logo-icon {dummyBlockName}_no_background-icon"">
<h1 class=""{dummyBlockName}__title""></h1>
<p class=""{dummyBlockName}__blurb""></p>
</div>
"
                },
                // If logo icon is valid HTML, it is rendered with a default class and a _has_logo-icon class is rendered
                new object[]{
                    CreateFlexiBannerBlock(logoIcon: dummyLogoIcon),
                    $@"<div class="" _has_logo-icon _no_background-icon"">
{dummyLogoIconWithClass}
<h1 class=""__title""></h1>
<p class=""__blurb""></p>
</div>
"
                },
                // If logo icon is null, whitespace or an empty string, no icon is rendered and a _no_logo-icon class is rendered (null case already verified in other tests)
                new object[]{
                    CreateFlexiBannerBlock(logoIcon: " "),
                    @"<div class="" _no_logo-icon _no_background-icon"">
<h1 class=""__title""></h1>
<p class=""__blurb""></p>
</div>
"
                },
                new object[]{
                    CreateFlexiBannerBlock(logoIcon: string.Empty),
                    @"<div class="" _no_logo-icon _no_background-icon"">
<h1 class=""__title""></h1>
<p class=""__blurb""></p>
</div>
"
                },
                // If background icon is valid HTML, it is rendered with a default class and a _has_background-icon class is rendered
                new object[]{
                    CreateFlexiBannerBlock(backgroundIcon: dummyBackgroundIcon),
                    $@"<div class="" _no_logo-icon _has_background-icon"">
{dummyBackgroundIconWithClass}
<h1 class=""__title""></h1>
<p class=""__blurb""></p>
</div>
"
                },
                // If background icon is null, whitespace or an empty string, no icon is rendered and a _no_background-icon class is rendered (null case already verified in other tests)
                new object[]{
                    CreateFlexiBannerBlock(backgroundIcon: " "),
                    @"<div class="" _no_logo-icon _no_background-icon"">
<h1 class=""__title""></h1>
<p class=""__blurb""></p>
</div>
"
                },
                new object[]{
                    CreateFlexiBannerBlock(backgroundIcon: string.Empty),
                    @"<div class="" _no_logo-icon _no_background-icon"">
<h1 class=""__title""></h1>
<p class=""__blurb""></p>
</div>
"
                },
                // If attributes are specified, they're written
                new object[]{
                    CreateFlexiBannerBlock(attributes: new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{ { dummyAttributeKey1, dummyAttributeValue1 }, { dummyAttributeKey2, dummyAttributeValue2 } })),
                    $@"<div class="" _no_logo-icon _no_background-icon"" {dummyAttributeKey1}=""{dummyAttributeValue1}"" {dummyAttributeKey2}=""{dummyAttributeValue2}"">
<h1 class=""__title""></h1>
<p class=""__blurb""></p>
</div>
"
                },
                // If classes are specified, they're appended to default classes
                new object[]{
                    CreateFlexiBannerBlock(attributes: new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{ { "class", dummyClass } })),
                    $@"<div class="" _no_logo-icon _no_background-icon {dummyClass}"">
<h1 class=""__title""></h1>
<p class=""__blurb""></p>
</div>
"
                },
                // Title part is rendered
                new object[]{
                    CreateFlexiBannerBlock(titlePart: dummyTitlePartBlock),
                    $@"<div class="" _no_logo-icon _no_background-icon"">
<h1 class=""__title"">{dummyTitle}</h1>
<p class=""__blurb""></p>
</div>
"
                },
                // Blurb part is rendered
                new object[]{
                    CreateFlexiBannerBlock(blurbPart: dummyBlurbPartBlock),
                    $@"<div class="" _no_logo-icon _no_background-icon"">
<h1 class=""__title""></h1>
<p class=""__blurb"">{dummyBlurb}</p>
</div>
"
                }
            };
        }

        public class ExposedFlexiBannerBlockRenderer : FlexiBannerBlockRenderer
        {
            public void ExposedWriteBlock(HtmlRenderer htmlRenderer, FlexiBannerBlock flexiBannerBlock)
            {
                WriteBlock(htmlRenderer, flexiBannerBlock);
            }
        }

        private ExposedFlexiBannerBlockRenderer CreateExposedFlexiBannerBlockRenderer()
        {
            return new ExposedFlexiBannerBlockRenderer();
        }

        private static FlexiBannerBlock CreateFlexiBannerBlock(string blockName = default,
            string logoIcon = default,
            string backgroundIcon = default,
            ReadOnlyDictionary<string, string> attributes = default,
            BlockParser blockParser = default,
            PlainLeafBlock titlePart = default,
            PlainLeafBlock blurbPart = default)
        {
            return new FlexiBannerBlock(blockName, logoIcon, backgroundIcon, attributes, blockParser)
            {
                titlePart ?? new PlainLeafBlock(null), blurbPart ?? new PlainLeafBlock(null)
            };
        }
    }
}
