using Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Moq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiSectionBlocks
{
    public class FlexiSectionBlockRendererUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void WriteBlock_OnlyWritesChildrenIfEnableHtmlForBlockIsFalse()
        {
            // Arrange
            const string dummyChildText = "dummyChildText";
            var dummyContainerInline = new ContainerInline();
            dummyContainerInline.AppendChild(new LiteralInline(dummyChildText));
            var dummyParagraphBlock = new ParagraphBlock() { Inline = dummyContainerInline };
            FlexiSectionBlock dummyFlexiSectionBlock = CreateFlexiSectionBlock();
            dummyFlexiSectionBlock.Add(dummyParagraphBlock);
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter)
            {
                EnableHtmlForBlock = false
            };
            ExposedFlexiSectionBlockRenderer testSubject = CreateExposedFlexiSectionBlockRenderer();

            // Act
            testSubject.ExposedWriteBlock(dummyHtmlRenderer, dummyFlexiSectionBlock);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(dummyChildText + "\n", result, ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void WriteBlock_RendersClassicFlexiSectionBlockIfRenderingModeIsClassic()
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
            FlexiSectionBlock dummyFlexiSectionBlock = CreateFlexiSectionBlock(renderingMode: FlexiSectionBlockRenderingMode.Classic);
            Mock<ExposedFlexiSectionBlockRenderer> mockTestSubject = CreateMockExposedFlexiSectionBlockRenderer();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.WriteClassic(dummyHtmlRenderer, dummyFlexiSectionBlock));

            // Act
            mockTestSubject.Object.ExposedWriteBlock(dummyHtmlRenderer, dummyFlexiSectionBlock);

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void WriteBlock_RendersStandardFlexiSectionBlockIfRenderingModeIsStandard()
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
            FlexiSectionBlock dummyFlexiSectionBlock = CreateFlexiSectionBlock(renderingMode: FlexiSectionBlockRenderingMode.Standard);
            Mock<ExposedFlexiSectionBlockRenderer> mockTestSubject = CreateMockExposedFlexiSectionBlockRenderer();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.WriteStandard(dummyHtmlRenderer, dummyFlexiSectionBlock));

            // Act
            mockTestSubject.Object.ExposedWriteBlock(dummyHtmlRenderer, dummyFlexiSectionBlock);

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void WriteClassic_RendersClassicFlexiSectionBlock()
        {
            // Arrange
            const int dummyLevel = 6;
            const string dummyHeadingText = "dummyHeadingText";
            var dummyHeadingContainerInline = new ContainerInline();
            dummyHeadingContainerInline.AppendChild(new LiteralInline(dummyHeadingText));
            FlexiSectionHeadingBlock dummyFlexiSectionHeadingBlock = CreateFlexiSectionHeadingBlock();
            dummyFlexiSectionHeadingBlock.Inline = dummyHeadingContainerInline;
            const string dummyChildText = "dummyChildText";
            var dummyChildContainerInline = new ContainerInline();
            dummyChildContainerInline.AppendChild(new LiteralInline(dummyChildText));
            var dummyChildBlock = new ParagraphBlock() { Inline = dummyChildContainerInline };
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
            FlexiSectionBlockRenderer testSubject = CreateFlexiSectionBlockRenderer();
            FlexiSectionBlock flexiSectionBlock = CreateFlexiSectionBlock(level: dummyLevel);
            flexiSectionBlock.Add(dummyFlexiSectionHeadingBlock);
            flexiSectionBlock.Add(dummyChildBlock);

            // Act
            testSubject.WriteClassic(dummyHtmlRenderer, flexiSectionBlock);

            // Assert
            string result = dummyStringWriter.ToString();
            Assert.Equal($@"<h{dummyLevel}>{dummyHeadingText}</h{dummyLevel}>
<p>{dummyChildText}</p>
",
                result,
                ignoreLineEndingDifferences: true);
        }

        [Theory]
        [MemberData(nameof(WriteStandard_RendersStandardFlexiSectionBlock_Data))]
        public void WriteStandard_RendersStandardFlexiSectionBlock(FlexiSectionBlock dummyFlexiSectionBlock,
            FlexiSectionHeadingBlock dummyFlexiSectionHeadingBlock,
            string expectedResult)
        {
            // Arrange
            dummyFlexiSectionBlock.Add(dummyFlexiSectionHeadingBlock);
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter); // Note that markdig changes dummyStringWriter.NewLine to '\n'
            FlexiSectionBlockRenderer testSubject = CreateFlexiSectionBlockRenderer();

            // Act
            testSubject.Write(dummyHtmlRenderer, dummyFlexiSectionBlock);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(expectedResult, result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> WriteStandard_RendersStandardFlexiSectionBlock_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string dummyLinkIcon = "<dummyLinkIcon></dummyLinkIcon>";
            const string dummyLinkIconWithClass = "<dummyLinkIcon class=\"__link-icon\"></dummyLinkIcon>";
            const string dummyAttributeKey1 = "dummyAttributeKey1";
            const string dummyAttributeValue1 = "dummyAttributeValue1";
            const string dummyAttributeKey2 = "dummyAttributeKey2";
            const string dummyAttributeValue2 = "dummyAttributeValue2";
            const string dummyClass = "dummyClass";
            const string dummyID = "dummyID";
            const string dummyGeneratedID = "dummyGeneratedID";
            const SectioningContentElement dummyElement = SectioningContentElement.Aside;
            const int dummyLevel = 2;
            const string dummyHeadingContent = "dummyHeadingContent";

            var dummyHeadingContainerInline = new ContainerInline();
            dummyHeadingContainerInline.AppendChild(new LiteralInline(dummyHeadingContent));
            FlexiSectionHeadingBlock dummyFlexiSectionHeadingBlockWithContent = CreateFlexiSectionHeadingBlock();
            dummyFlexiSectionHeadingBlockWithContent.Inline = dummyHeadingContainerInline;

            return new object[][]
            {
                // BlockName is assigned as a class of the root element and all default classes are prepended with it
                new object[]
                {
                    CreateFlexiSectionBlock(dummyBlockName, level: dummyLevel),
                    CreateFlexiSectionHeadingBlock(),
                    $@"<section class=""{dummyBlockName} {dummyBlockName}_level_2 {dummyBlockName}_no-link-icon"">
<header class=""{dummyBlockName}__header"">
<h2 class=""{dummyBlockName}__heading""></h2>
<button class=""{dummyBlockName}__link-button"" aria-label=""Copy link"">
</button>
</header>
</section>
"
                },
                // If LinkIcon is valid HTML, it is rendered with a default class and a _has-link-icon class is rendered
                new object[]
                {
                    CreateFlexiSectionBlock(linkIcon: dummyLinkIcon, level: dummyLevel),
                    CreateFlexiSectionHeadingBlock(),
                    $@"<section class="" _level_2 _has-link-icon"">
<header class=""__header"">
<h2 class=""__heading""></h2>
<button class=""__link-button"" aria-label=""Copy link"">
{dummyLinkIconWithClass}
</button>
</header>
</section>
"
                },
                // If LinkIcon is null, whitespace or an empty string, no copy icon is rendered and a _no-link-icon class is rendered (null case verified by other tests)
                new object[]
                {
                    CreateFlexiSectionBlock(linkIcon: " ", level: dummyLevel),
                    CreateFlexiSectionHeadingBlock(),
                    @"<section class="" _level_2 _no-link-icon"">
<header class=""__header"">
<h2 class=""__heading""></h2>
<button class=""__link-button"" aria-label=""Copy link"">
</button>
</header>
</section>
"
                },
                new object[]
                {
                    CreateFlexiSectionBlock(linkIcon: string.Empty, level: dummyLevel),
                    CreateFlexiSectionHeadingBlock(),
                    @"<section class="" _level_2 _no-link-icon"">
<header class=""__header"">
<h2 class=""__heading""></h2>
<button class=""__link-button"" aria-label=""Copy link"">
</button>
</header>
</section>
"
                },
                // If an ID is generated, it is written
                new object[]
                {
                    CreateFlexiSectionBlock(level: dummyLevel),
                    CreateFlexiSectionHeadingBlock(generatedID: dummyGeneratedID),
                    $@"<section class="" _level_2 _no-link-icon"" id=""{dummyGeneratedID}"">
<header class=""__header"">
<h2 class=""__heading""></h2>
<button class=""__link-button"" aria-label=""Copy link"">
</button>
</header>
</section>
"
                },
                // If attributes specified, they're written
                new object[]
                {
                    CreateFlexiSectionBlock(level: dummyLevel,
                        attributes: new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{ { dummyAttributeKey1, dummyAttributeValue1 }, { dummyAttributeKey2, dummyAttributeValue2 } })),
                    CreateFlexiSectionHeadingBlock(),
                    $@"<section class="" _level_2 _no-link-icon"" {dummyAttributeKey1}=""{dummyAttributeValue1}"" {dummyAttributeKey2}=""{dummyAttributeValue2}"">
<header class=""__header"">
<h2 class=""__heading""></h2>
<button class=""__link-button"" aria-label=""Copy link"">
</button>
</header>
</section>
"
                },
                // Class attribute value is appended to default classes
                new object[]
                {
                    CreateFlexiSectionBlock(level: dummyLevel,
                        attributes: new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{ { "class", dummyClass } })),
                    CreateFlexiSectionHeadingBlock(),
                    $@"<section class="" _level_2 _no-link-icon {dummyClass}"">
<header class=""__header"">
<h2 class=""__heading""></h2>
<button class=""__link-button"" aria-label=""Copy link"">
</button>
</header>
</section>
"
                },
                // Generated ID takes precedence over any ID in attributes
                new object[]
                {
                    CreateFlexiSectionBlock(level: dummyLevel,
                        attributes: new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{ { "id", dummyID } })),
                    CreateFlexiSectionHeadingBlock(generatedID: dummyGeneratedID),
                    $@"<section class="" _level_2 _no-link-icon"" id=""{dummyGeneratedID}"">
<header class=""__header"">
<h2 class=""__heading""></h2>
<button class=""__link-button"" aria-label=""Copy link"">
</button>
</header>
</section>
"
                },
                // Specified sectioning content element is rendered
                new object[]
                {
                    CreateFlexiSectionBlock(element: dummyElement, level: dummyLevel),
                    CreateFlexiSectionHeadingBlock(),
                    $@"<{dummyElement.ToString().ToLower()} class="" _level_2 _no-link-icon"">
<header class=""__header"">
<h2 class=""__heading""></h2>
<button class=""__link-button"" aria-label=""Copy link"">
</button>
</header>
</{dummyElement.ToString().ToLower()}>
"
                },
                // Header content is rendered
                new object[]
                {
                    CreateFlexiSectionBlock(level: dummyLevel),
                    dummyFlexiSectionHeadingBlockWithContent,
                    $@"<section class="" _level_2 _no-link-icon"">
<header class=""__header"">
<h2 class=""__heading"">{dummyHeadingContent}</h2>
<button class=""__link-button"" aria-label=""Copy link"">
</button>
</header>
</section>
"
                }
            };
        }

        // Can't be part of the theories above since we have to insert dummyChildBlock after FlexiSectionHeadingBlock
        [Fact]
        public void WriteStandard_RendersChildren()
        {
            // Arrange
            const string dummyChildContent = "dummyChildText";
            var dummyChildContainerInline = new ContainerInline();
            dummyChildContainerInline.AppendChild(new LiteralInline(dummyChildContent));
            var dummyChildBlock = new ParagraphBlock() { Inline = dummyChildContainerInline };
            FlexiSectionBlock dummyFlexiSectionBlock = CreateFlexiSectionBlock(level: 2); // Level must be specified or we'll get an out of range exception
            dummyFlexiSectionBlock.Add(CreateFlexiSectionHeadingBlock());
            dummyFlexiSectionBlock.Add(dummyChildBlock);
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter); // Note that markdig changes dummyStringWriter.NewLine to '\n'
            FlexiSectionBlockRenderer testSubject = CreateFlexiSectionBlockRenderer();

            // Act
            testSubject.Write(dummyHtmlRenderer, dummyFlexiSectionBlock);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal($@"<section class="" _level_2 _no-link-icon"">
<header class=""__header"">
<h2 class=""__heading""></h2>
<button class=""__link-button"" aria-label=""Copy link"">
</button>
</header>
<p>{dummyChildContent}</p>
</section>
",
                result,
                ignoreLineEndingDifferences: true);
        }

        public class ExposedFlexiSectionBlockRenderer : FlexiSectionBlockRenderer
        {
            public void ExposedWriteBlock(HtmlRenderer htmlRenderer, FlexiSectionBlock block)
            {
                WriteBlock(htmlRenderer, block);
            }
        }

        private ExposedFlexiSectionBlockRenderer CreateExposedFlexiSectionBlockRenderer()
        {
            return new ExposedFlexiSectionBlockRenderer();
        }

        private Mock<ExposedFlexiSectionBlockRenderer> CreateMockExposedFlexiSectionBlockRenderer()
        {
            return _mockRepository.Create<ExposedFlexiSectionBlockRenderer>();
        }

        private FlexiSectionBlockRenderer CreateFlexiSectionBlockRenderer()
        {
            return new FlexiSectionBlockRenderer();
        }

        private static FlexiSectionBlock CreateFlexiSectionBlock(string blockName = default,
            SectioningContentElement element = default,
            string linkIcon = default,
            FlexiSectionBlockRenderingMode renderingMode = default,
            int level = default,
            ReadOnlyDictionary<string, string> attributes = default,
            BlockParser blockParser = default)
        {
            return new FlexiSectionBlock(blockName, element, linkIcon, renderingMode, level, attributes, blockParser);
        }

        private static FlexiSectionHeadingBlock CreateFlexiSectionHeadingBlock(BlockParser blockParser = default,
            string generatedID = default)
        {
            return new FlexiSectionHeadingBlock(blockParser)
            {
                Lines = new StringLineGroup(1), // Xunit calls ToString on all theory arguments, if a LeafBlock doesn't have Lines, ToString throws.
                GeneratedID = generatedID
            };
        }
    }
}
