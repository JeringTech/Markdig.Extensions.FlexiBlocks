using Jering.Markdig.Extensions.FlexiBlocks.FlexiTabsBlocks;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiTabsBlocks
{
    public class FlexiTabsBlockRendererUnitTests
    {
        [Fact]
        public void WriteBlock_OnlyWritesChildrenIfEnableHtmlForBlockIsFalse()
        {
            // Arrange
            // Dummy tab part
            const string dummyTab = "dummyTab";
            var dummyTabContainerInline = new ContainerInline();
            dummyTabContainerInline.AppendChild(new LiteralInline(dummyTab));
            var dummyTabPartBlock = new PlainLeafBlock(null);
            dummyTabPartBlock.Inline = dummyTabContainerInline;
            // Dummy panel part
            const string dummyPanel = "dummyPanel";
            var dummyPanelContainerInline = new ContainerInline();
            dummyPanelContainerInline.AppendChild(new LiteralInline(dummyPanel));
            var dummyPanelParagraphBlock = new ParagraphBlock() { Inline = dummyPanelContainerInline };
            var dummyPanelPartBlock = new PlainContainerBlock(null);
            dummyPanelPartBlock.Add(dummyPanelParagraphBlock);
            FlexiTabsBlock dummyFlexiTabsBlock = CreateFlexiTabsBlock(children: new List<FlexiTabBlock>{
                CreateFlexiTabBlock(tabPart: dummyTabPartBlock, panelPart: dummyPanelPartBlock)
            });
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter)
            {
                EnableHtmlForBlock = false
            };
            ExposedFlexiTabsBlockRenderer testSubject = CreateExposedFlexiTabsBlockRenderer();

            // Act
            testSubject.ExposedWriteBlock(dummyHtmlRenderer, dummyFlexiTabsBlock);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal($"{dummyTab}\n{dummyPanel}\n", result, ignoreLineEndingDifferences: true);
        }

        [Theory]
        [MemberData(nameof(WriteBlock_WritesBlock_Data))]
        public void WriteBlock_WritesBlock(FlexiTabsBlock dummyFlexiTabsBlock, string expectedResult)
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
            ExposedFlexiTabsBlockRenderer testSubject = CreateExposedFlexiTabsBlockRenderer();

            // Act
            testSubject.ExposedWriteBlock(dummyHtmlRenderer, dummyFlexiTabsBlock);
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

            // Dummy tab part
            const string dummyTab = "dummyTab";
            var dummyTabContainerInline = new ContainerInline();
            dummyTabContainerInline.AppendChild(new LiteralInline(dummyTab));
            var dummyTabPartBlock = new PlainLeafBlock(null);
            dummyTabPartBlock.Inline = dummyTabContainerInline;

            // Dummy panel part
            const string dummyPanel = "dummyPanel";
            var dummyPanelContainerInline = new ContainerInline();
            dummyPanelContainerInline.AppendChild(new LiteralInline(dummyPanel));
            var dummyPanelParagraphBlock = new ParagraphBlock() { Inline = dummyPanelContainerInline };
            var dummyPanelPartBlock = new PlainContainerBlock(null);
            dummyPanelPartBlock.Add(dummyPanelParagraphBlock);

            return new object[][]
            {
                // BlockName is assigned as a class of the root element and all default classes are prepended with it
                new object[]{
                    CreateFlexiTabsBlock(dummyBlockName),
                    $@"<div class=""{dummyBlockName}"">
<div class=""{dummyBlockName}__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal"">
<div class=""{dummyBlockName}__tab-list scrollable-indicators__scrollable"" role=""tablist"">
</div>
<div class=""scrollable-indicators__indicator scrollable-indicators__indicator_start""></div>
<div class=""scrollable-indicators__indicator scrollable-indicators__indicator_end""></div>
</div>
</div>
"
                },
                // If attributes are specified, they're written
                new object[]{
                    CreateFlexiTabsBlock(attributes: new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{ { dummyAttributeKey1, dummyAttributeValue1 }, { dummyAttributeKey2, dummyAttributeValue2 } })),
                    $@"<div class="""" {dummyAttributeKey1}=""{dummyAttributeValue1}"" {dummyAttributeKey2}=""{dummyAttributeValue2}"">
<div class=""__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal"">
<div class=""__tab-list scrollable-indicators__scrollable"" role=""tablist"">
</div>
<div class=""scrollable-indicators__indicator scrollable-indicators__indicator_start""></div>
<div class=""scrollable-indicators__indicator scrollable-indicators__indicator_end""></div>
</div>
</div>
"
                },
                // If classes are specified, they're appended to default classes
                new object[]{
                    CreateFlexiTabsBlock(attributes: new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{ { "class", dummyClass } })),
                    $@"<div class="" {dummyClass}"">
<div class=""__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal"">
<div class=""__tab-list scrollable-indicators__scrollable"" role=""tablist"">
</div>
<div class=""scrollable-indicators__indicator scrollable-indicators__indicator_start""></div>
<div class=""scrollable-indicators__indicator scrollable-indicators__indicator_end""></div>
</div>
</div>
"
                },
                // Child FlexiTabBlocks are rendered
                new object[]{
                    CreateFlexiTabsBlock(children: new List<FlexiTabBlock>{
                        CreateFlexiTabBlock(tabPart: dummyTabPartBlock, panelPart: dummyPanelPartBlock)
                    }),
                    $@"<div class="""">
<div class=""__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal"">
<div class=""__tab-list scrollable-indicators__scrollable"" role=""tablist"">
<button class=""__tab __tab_selected"" title=""View panel"" role=""tab"" aria-selected=""true"">{dummyTab}</button>
</div>
<div class=""scrollable-indicators__indicator scrollable-indicators__indicator_start""></div>
<div class=""scrollable-indicators__indicator scrollable-indicators__indicator_end""></div>
</div>
<div class=""__tab-panel"" tabindex=""0"" role=""tabpanel"" aria-label=""{dummyTab}"">
<p>{dummyPanel}</p>
</div>
</div>
"
                }
            };
        }

        [Fact]
        public void WriteTab_WritesUnselectedTabIfIndexIsLargerThan0()
        {
            // Arrange
            FlexiTabBlock dummyFlexiTabBlock = CreateFlexiTabBlock();
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
            FlexiTabsBlockRenderer testSubject = CreateFlexiTabsBlockRenderer();

            // Act
            testSubject.WriteTab(dummyHtmlRenderer, dummyFlexiTabBlock, null, 1);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(@"<button class=""__tab"" title=""View panel"" role=""tab"" aria-selected=""false"" tabindex=""-1""></button>
",
                result,
                ignoreLineEndingDifferences: true);
        }

        [Theory]
        [MemberData(nameof(WritePanel_WritesPanel_Data))]
        public void WritePanel_WritesPanel(FlexiTabBlock dummyFlexiTabBlock, string dummyBlockName, int dummyIndex, string expectedResult)
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
            FlexiTabsBlockRenderer testSubject = CreateFlexiTabsBlockRenderer();

            // Act
            testSubject.WritePanel(dummyHtmlRenderer, dummyFlexiTabBlock, dummyBlockName, dummyIndex);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(expectedResult, result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> WritePanel_WritesPanel_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string dummyAttributeKey1 = "dummyAttributeKey1";
            const string dummyAttributeValue1 = "dummyAttributeValue1";
            const string dummyAttributeKey2 = "dummyAttributeKey2";
            const string dummyAttributeValue2 = "dummyAttributeValue2";
            const string dummyClass = "dummyClass";

            // Dummy tab part
            const string dummyTab = "dummyTab";
            var dummyTabContainerInline = new ContainerInline();
            dummyTabContainerInline.AppendChild(new LiteralInline(dummyTab));
            var dummyTabPartBlock = new PlainLeafBlock(null);
            dummyTabPartBlock.Inline = dummyTabContainerInline;

            // Dummy panel part
            const string dummyPanel = "dummyPanel";
            var dummyPanelContainerInline = new ContainerInline();
            dummyPanelContainerInline.AppendChild(new LiteralInline(dummyPanel));
            var dummyPanelParagraphBlock = new ParagraphBlock() { Inline = dummyPanelContainerInline };
            var dummyPanelPartBlock = new PlainContainerBlock(null);
            dummyPanelPartBlock.Add(dummyPanelParagraphBlock);

            return new object[][]
            {
                // BlockName prepended to all default classes
                new object[]{
                    CreateFlexiTabBlock(),
                    dummyBlockName,
                    0,
                    $@"<div class=""{dummyBlockName}__tab-panel"" tabindex=""0"" role=""tabpanel"" aria-label="""">
</div>
"
                },
                // Root element has hidden modifier class if index is larger than 0 (index 0 case covered by other tests)
                new object[]{
                    CreateFlexiTabBlock(),
                    null,
                    1,
                    @"<div class=""__tab-panel __tab-panel_hidden"" tabindex=""0"" role=""tabpanel"" aria-label="""">
</div>
"
                },
                // If attributes are specified, they're written
                new object[]{
                    CreateFlexiTabBlock(attributes: new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{ { dummyAttributeKey1, dummyAttributeValue1 }, { dummyAttributeKey2, dummyAttributeValue2 } })),
                    null,
                    0,
                    $@"<div class=""__tab-panel"" {dummyAttributeKey1}=""{dummyAttributeValue1}"" {dummyAttributeKey2}=""{dummyAttributeValue2}"" tabindex=""0"" role=""tabpanel"" aria-label="""">
</div>
"
                },
                // If classes are specified, they're appended to default classes
                new object[]{
                    CreateFlexiTabBlock(attributes: new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{ { "class", dummyClass } })),
                    null,
                    0,
                    $@"<div class=""__tab-panel {dummyClass}"" tabindex=""0"" role=""tabpanel"" aria-label="""">
</div>
"
                },
                // Tab part is rendered as aria-label value
                new object[]{
                    CreateFlexiTabBlock(tabPart: dummyTabPartBlock),
                    null,
                    0,
                    $@"<div class=""__tab-panel"" tabindex=""0"" role=""tabpanel"" aria-label=""{dummyTab}"">
</div>
"
                },
                // Panel part is rendered
                new object[]{
                    CreateFlexiTabBlock(panelPart: dummyPanelPartBlock),
                    null,
                    0,
                    $@"<div class=""__tab-panel"" tabindex=""0"" role=""tabpanel"" aria-label="""">
<p>{dummyPanel}</p>
</div>
"
                }
            };
        }

        public class ExposedFlexiTabsBlockRenderer : FlexiTabsBlockRenderer
        {
            public void ExposedWriteBlock(HtmlRenderer htmlRenderer, FlexiTabsBlock flexiTabsBlock)
            {
                WriteBlock(htmlRenderer, flexiTabsBlock);
            }
        }

        private ExposedFlexiTabsBlockRenderer CreateExposedFlexiTabsBlockRenderer()
        {
            return new ExposedFlexiTabsBlockRenderer();
        }

        private FlexiTabsBlockRenderer CreateFlexiTabsBlockRenderer()
        {
            return new FlexiTabsBlockRenderer();
        }

        private static FlexiTabBlock CreateFlexiTabBlock(ReadOnlyDictionary<string, string> attributes = default,
            BlockParser blockParser = default,
            PlainLeafBlock tabPart = default,
            PlainContainerBlock panelPart = default)
        {
            return new FlexiTabBlock(attributes, blockParser)
            {
                tabPart ?? new PlainLeafBlock(blockParser),
                panelPart ?? new PlainContainerBlock(blockParser),
            };
        }

        private static FlexiTabsBlock CreateFlexiTabsBlock(string blockName = default,
            ReadOnlyDictionary<string, string> attributes = default,
            BlockParser blockParser = default,
            List<FlexiTabBlock> children = default)
        {
            var result = new FlexiTabsBlock(blockName, attributes, blockParser);

            if (children != null)
            {
                foreach (FlexiTabBlock child in children)
                {
                    result.Add(child);
                }
            }
            return result;
        }
    }
}
