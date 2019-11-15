using Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiTableBlocks
{
    public class FlexiTableBlockRendererUnitTests
    {
        [Fact]
        public void WriteBlock_OnlyWritesChildrenIfEnableHtmlForBlockIsFalse()
        {
            // Arrange
            const string dummyChildText = "dummyChildText";
            var dummyContainerInline = new ContainerInline();
            dummyContainerInline.AppendChild(new LiteralInline(dummyChildText));
            var dummyParagraphBlock = new ParagraphBlock() { Inline = dummyContainerInline };
            FlexiTableBlock dummyFlexiTableBlock = CreateFlexiTableBlock();
            dummyFlexiTableBlock.Add(dummyParagraphBlock);
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter)
            {
                EnableHtmlForBlock = false
            };
            ExposedFlexiTableBlockRenderer testSubject = CreateExposedFlexiTableBlockRenderer();

            // Act
            testSubject.ExposedWriteBlock(dummyHtmlRenderer, dummyFlexiTableBlock);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(dummyChildText + "\n", result, ignoreLineEndingDifferences: true);
        }

        [Theory]
        [MemberData(nameof(WriteBlock_WritesBlock_Data))]
        public void WriteBlock_WritesBlock(FlexiTableBlock dummyFlexiTableBlock,
            string expectedResult)
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
            ExposedFlexiTableBlockRenderer testSubject = CreateExposedFlexiTableBlockRenderer();

            // Act
            testSubject.ExposedWriteBlock(dummyHtmlRenderer, dummyFlexiTableBlock);
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

            return new object[][]
            {
                // BlockName is assigned as a class of the root element and all default classes are prepended with it
                new object[]{
                    CreateFlexiTableBlock(dummyBlockName),
                    $@"<div class=""{dummyBlockName} {dummyBlockName}_type_cards"">
<table class=""{dummyBlockName}__table"">
</table>
</div>
"
                },
                // If type is specified, a type modifier class is rendered. Also, labels are rendered if type is cards
                new object[]
                {
                    CreateFlexiTableBlock(
                        type: FlexiTableType.Cards,
                        flexiTableRowBlocks: new FlexiTableRowBlock[]{ CreateFlexiTableRowBlock(isHeaderRow: true), CreateFlexiTableRowBlock() }
                    ),
                    @"<div class="" _type_cards"">
<table class=""__table"">
<thead class=""__head"">
<tr class=""__row"">
<th class=""__header"">
dummyHeaderCell1
</th>
<th class=""__header"">
dummyHeaderCell2
</th>
</tr>
</thead>
<tbody class=""__body"">
<tr class=""__row"">
<td class=""__data"">
<div class=""__label"">
dummyHeaderCell1
</div>
<div class=""__content"">
dummyDataCell1
</div>
</td>
<td class=""__data"">
<div class=""__label"">
dummyHeaderCell2
</div>
<div class=""__content"">
dummyDataCell2
</div>
</td>
</tr>
</tbody>
</table>
</div>
"
                },
                new object[]
                {
                    CreateFlexiTableBlock(
                        type: FlexiTableType.FixedTitles,
                        flexiTableRowBlocks: new FlexiTableRowBlock[]{ CreateFlexiTableRowBlock(isHeaderRow: true), CreateFlexiTableRowBlock() }
                    ),
                    @"<div class="" _type_fixed-titles"">
<table class=""__table"">
<thead class=""__head"">
<tr class=""__row"">
<th class=""__header"">
dummyHeaderCell1
</th>
<th class=""__header"">
dummyHeaderCell2
</th>
</tr>
</thead>
<tbody class=""__body"">
<tr class=""__row"">
<td class=""__data"">
dummyDataCell1
</td>
<td class=""__data"">
dummyDataCell2
</td>
</tr>
</tbody>
</table>
</div>
"
                },
                new object[]
                {
                    CreateFlexiTableBlock(
                        type: FlexiTableType.Unresponsive,
                        flexiTableRowBlocks: new FlexiTableRowBlock[]{ CreateFlexiTableRowBlock(isHeaderRow: true), CreateFlexiTableRowBlock() }
                    ),
                    @"<div class="" _type_unresponsive"">
<table class=""__table"">
<thead class=""__head"">
<tr class=""__row"">
<th class=""__header"">
dummyHeaderCell1
</th>
<th class=""__header"">
dummyHeaderCell2
</th>
</tr>
</thead>
<tbody class=""__body"">
<tr class=""__row"">
<td class=""__data"">
dummyDataCell1
</td>
<td class=""__data"">
dummyDataCell2
</td>
</tr>
</tbody>
</table>
</div>
"
                },
                // If attributes are specified, they're written
                new object[]{
                    CreateFlexiTableBlock(attributes: new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{ { dummyAttributeKey1, dummyAttributeValue1 }, { dummyAttributeKey2, dummyAttributeValue2 } })),
                    $@"<div class="" _type_cards"" {dummyAttributeKey1}=""{dummyAttributeValue1}"" {dummyAttributeKey2}=""{dummyAttributeValue2}"">
<table class=""__table"">
</table>
</div>
"
                },
                // If classes are specified, they're appended to default classes
                new object[]{
                    CreateFlexiTableBlock(attributes: new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{ { "class", dummyClass } })),
                    $@"<div class="" _type_cards {dummyClass}"">
<table class=""__table"">
</table>
</div>
"
                },
                // Structure - No header rows, cards
                new object[]
                {
                    CreateFlexiTableBlock(flexiTableRowBlocks: new FlexiTableRowBlock[]{ CreateFlexiTableRowBlock() }),
                    @"<div class="" _type_cards"">
<table class=""__table"">
<tbody class=""__body"">
<tr class=""__row"">
<td class=""__data"">
dummyDataCell1
</td>
<td class=""__data"">
dummyDataCell2
</td>
</tr>
</tbody>
</table>
</div>
"
                },
                // Structure - No header rows, unresponsive
                new object[]
                {
                    CreateFlexiTableBlock(type: FlexiTableType.Unresponsive, flexiTableRowBlocks: new FlexiTableRowBlock[]{ CreateFlexiTableRowBlock() }),
                    @"<div class="" _type_unresponsive"">
<table class=""__table"">
<tbody class=""__body"">
<tr class=""__row"">
<td class=""__data"">
dummyDataCell1
</td>
<td class=""__data"">
dummyDataCell2
</td>
</tr>
</tbody>
</table>
</div>
"
                },
                // Structure - Multiple header rows, cards
                new object[]
                {
                    CreateFlexiTableBlock(
                        flexiTableRowBlocks: new FlexiTableRowBlock[]
                        {
                            CreateFlexiTableRowBlock(isHeaderRow: true),
                            CreateFlexiTableRowBlock(startNum: 3, isHeaderRow: true),
                            CreateFlexiTableRowBlock()
                        }
                    ),
                    @"<div class="" _type_cards"">
<table class=""__table"">
<thead class=""__head"">
<tr class=""__row"">
<th class=""__header"">
dummyHeaderCell1
</th>
<th class=""__header"">
dummyHeaderCell2
</th>
</tr>
<tr class=""__row"">
<th class=""__header"">
dummyHeaderCell3
</th>
<th class=""__header"">
dummyHeaderCell4
</th>
</tr>
</thead>
<tbody class=""__body"">
<tr class=""__row"">
<td class=""__data"">
<div class=""__label"">
dummyHeaderCell1
</div>
<div class=""__content"">
dummyDataCell1
</div>
</td>
<td class=""__data"">
<div class=""__label"">
dummyHeaderCell2
</div>
<div class=""__content"">
dummyDataCell2
</div>
</td>
</tr>
</tbody>
</table>
</div>
"
                },
                // Structure - Multiple header rows, unresponsive
                new object[]
                {
                    CreateFlexiTableBlock(
                        type: FlexiTableType.Unresponsive,
                        flexiTableRowBlocks: new FlexiTableRowBlock[]
                        {
                            CreateFlexiTableRowBlock(isHeaderRow: true),
                            CreateFlexiTableRowBlock(startNum: 3, isHeaderRow: true),
                            CreateFlexiTableRowBlock()
                        }
                    ),
                    @"<div class="" _type_unresponsive"">
<table class=""__table"">
<thead class=""__head"">
<tr class=""__row"">
<th class=""__header"">
dummyHeaderCell1
</th>
<th class=""__header"">
dummyHeaderCell2
</th>
</tr>
<tr class=""__row"">
<th class=""__header"">
dummyHeaderCell3
</th>
<th class=""__header"">
dummyHeaderCell4
</th>
</tr>
</thead>
<tbody class=""__body"">
<tr class=""__row"">
<td class=""__data"">
dummyDataCell1
</td>
<td class=""__data"">
dummyDataCell2
</td>
</tr>
</tbody>
</table>
</div>
"
                },
                // Structure - No data rows, cards
                new object[]
                {
                    CreateFlexiTableBlock(
                        flexiTableRowBlocks: new FlexiTableRowBlock[]{ CreateFlexiTableRowBlock(isHeaderRow: true) }),
                    @"<div class="" _type_cards"">
<table class=""__table"">
<thead class=""__head"">
<tr class=""__row"">
<th class=""__header"">
dummyHeaderCell1
</th>
<th class=""__header"">
dummyHeaderCell2
</th>
</tr>
</thead>
</table>
</div>
"
                },
                // Structure - No data rows, unresponsive
                new object[]
                {
                    CreateFlexiTableBlock(
                        type: FlexiTableType.Unresponsive,
                        flexiTableRowBlocks: new FlexiTableRowBlock[]{ CreateFlexiTableRowBlock(isHeaderRow: true) }),
                    @"<div class="" _type_unresponsive"">
<table class=""__table"">
<thead class=""__head"">
<tr class=""__row"">
<th class=""__header"">
dummyHeaderCell1
</th>
<th class=""__header"">
dummyHeaderCell2
</th>
</tr>
</thead>
</table>
</div>
"
                },
                // Structure - Multiple data rows, cards
                new object[]
                {
                    CreateFlexiTableBlock(
                        flexiTableRowBlocks: new FlexiTableRowBlock[]{ CreateFlexiTableRowBlock(isHeaderRow: true), CreateFlexiTableRowBlock(), CreateFlexiTableRowBlock(startNum: 3) }),
                    @"<div class="" _type_cards"">
<table class=""__table"">
<thead class=""__head"">
<tr class=""__row"">
<th class=""__header"">
dummyHeaderCell1
</th>
<th class=""__header"">
dummyHeaderCell2
</th>
</tr>
</thead>
<tbody class=""__body"">
<tr class=""__row"">
<td class=""__data"">
<div class=""__label"">
dummyHeaderCell1
</div>
<div class=""__content"">
dummyDataCell1
</div>
</td>
<td class=""__data"">
<div class=""__label"">
dummyHeaderCell2
</div>
<div class=""__content"">
dummyDataCell2
</div>
</td>
</tr>
<tr class=""__row"">
<td class=""__data"">
<div class=""__label"">
dummyHeaderCell1
</div>
<div class=""__content"">
dummyDataCell3
</div>
</td>
<td class=""__data"">
<div class=""__label"">
dummyHeaderCell2
</div>
<div class=""__content"">
dummyDataCell4
</div>
</td>
</tr>
</tbody>
</table>
</div>
"
                },
                // Structure - Multiple data rows, unresponsive
                new object[]
                {
                    CreateFlexiTableBlock(
                        type: FlexiTableType.Unresponsive,
                        flexiTableRowBlocks: new FlexiTableRowBlock[]{ CreateFlexiTableRowBlock(isHeaderRow: true), CreateFlexiTableRowBlock(), CreateFlexiTableRowBlock(startNum: 3) }),
                    @"<div class="" _type_unresponsive"">
<table class=""__table"">
<thead class=""__head"">
<tr class=""__row"">
<th class=""__header"">
dummyHeaderCell1
</th>
<th class=""__header"">
dummyHeaderCell2
</th>
</tr>
</thead>
<tbody class=""__body"">
<tr class=""__row"">
<td class=""__data"">
dummyDataCell1
</td>
<td class=""__data"">
dummyDataCell2
</td>
</tr>
<tr class=""__row"">
<td class=""__data"">
dummyDataCell3
</td>
<td class=""__data"">
dummyDataCell4
</td>
</tr>
</tbody>
</table>
</div>
"
                },
                // If a cell's colspan or rowspan are larger than 1, colspan or rowspan attributes are rendered
                new object[]
                {
                    CreateFlexiTableBlock(
                        type: FlexiTableType.Unresponsive,
                        flexiTableRowBlocks: new FlexiTableRowBlock[]
                        {
                            CreateFlexiTableRowBlock(3, isHeaderRow: true, colspans: new int[] { 1, 2, 3 }, rowspans: new int[] { 2, 1, 3 }),
                            CreateFlexiTableRowBlock(3, isHeaderRow: false, colspans: new int[] { 1, 2, 3 }, rowspans: new int[] { 2, 1, 3 })
                        }
                    ),
                    @"<div class="" _type_unresponsive"">
<table class=""__table"">
<thead class=""__head"">
<tr class=""__row"">
<th class=""__header"" rowspan=""2"">
dummyHeaderCell1
</th>
<th class=""__header"" colspan=""2"">
dummyHeaderCell2
</th>
<th class=""__header"" colspan=""3"" rowspan=""3"">
dummyHeaderCell3
</th>
</tr>
</thead>
<tbody class=""__body"">
<tr class=""__row"">
<td class=""__data"" rowspan=""2"">
dummyDataCell1
</td>
<td class=""__data"" colspan=""2"">
dummyDataCell2
</td>
<td class=""__data"" colspan=""3"" rowspan=""3"">
dummyDataCell3
</td>
</tr>
</tbody>
</table>
</div>
"
                },
                // Content alignment
                new object[]
                {
                    CreateFlexiTableBlock(
                        type: FlexiTableType.Unresponsive,
                        flexiTableRowBlocks: new FlexiTableRowBlock[]
                        {
                            CreateFlexiTableRowBlock(3, isHeaderRow: true, contentAlignments: new ContentAlignment[] { ContentAlignment.Start, ContentAlignment.Center, ContentAlignment.End }),
                            CreateFlexiTableRowBlock(3, isHeaderRow: false, contentAlignments: new ContentAlignment[] { ContentAlignment.Start, ContentAlignment.Center, ContentAlignment.End })
                        }
                    ),
                    @"<div class="" _type_unresponsive"">
<table class=""__table"">
<thead class=""__head"">
<tr class=""__row"">
<th class=""__header __header_align_start"">
dummyHeaderCell1
</th>
<th class=""__header __header_align_center"">
dummyHeaderCell2
</th>
<th class=""__header __header_align_end"">
dummyHeaderCell3
</th>
</tr>
</thead>
<tbody class=""__body"">
<tr class=""__row"">
<td class=""__data __data_align_start"">
dummyDataCell1
</td>
<td class=""__data __data_align_center"">
dummyDataCell2
</td>
<td class=""__data __data_align_end"">
dummyDataCell3
</td>
</tr>
</tbody>
</table>
</div>
"
                },
            };
        }

        public class ExposedFlexiTableBlockRenderer : FlexiTableBlockRenderer
        {
            public void ExposedWriteBlock(HtmlRenderer htmlRenderer, FlexiTableBlock flexiTableBlock)
            {
                WriteBlock(htmlRenderer, flexiTableBlock);
            }
        }

        private static FlexiTableRowBlock CreateFlexiTableRowBlock(int numCells = 2,
            int startNum = 1,
            bool isHeaderRow = false,
            int[] colspans = null,
            int[] rowspans = null,
            ContentAlignment[] contentAlignments = null)
        {
            string cellType = isHeaderRow ? "Header" : "Data";
            var result = new FlexiTableRowBlock(isHeaderRow);

            for (int i = 0; i < numCells; i++)
            {
                var cell = new FlexiTableCellBlock(colspans?[i] ?? 0, rowspans?[i] ?? 0, contentAlignments?[i] ?? ContentAlignment.None) { CreateParagraphBlock($"dummy{cellType}Cell{startNum++}") };
                result.Add(cell);
            }

            return result;
        }

        private static ParagraphBlock CreateParagraphBlock(string content)
        {
            var containerInline = new ContainerInline();
            containerInline.AppendChild(new LiteralInline(content));
            return new ParagraphBlock() { Inline = containerInline };
        }

        private ExposedFlexiTableBlockRenderer CreateExposedFlexiTableBlockRenderer()
        {
            return new ExposedFlexiTableBlockRenderer();
        }

        private static FlexiTableBlock CreateFlexiTableBlock(string blockName = default,
            FlexiTableType type = default,
            ReadOnlyDictionary<string, string> attributes = default,
            BlockParser blockParser = default,
            params FlexiTableRowBlock[] flexiTableRowBlocks)
        {
            var result = new FlexiTableBlock(blockName, type, attributes, blockParser);

            foreach (FlexiTableRowBlock flexiTableRowBlock in flexiTableRowBlocks)
            {
                result.Add(flexiTableRowBlock);
            }

            return result;
        }
    }
}
