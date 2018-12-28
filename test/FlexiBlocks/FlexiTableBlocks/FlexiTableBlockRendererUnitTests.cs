using Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks;
using Markdig.Extensions.Tables;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiTableBlocks
{
    public class FlexiTableBlockRendererUnitTests
    {
        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfExtensionOptionsIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiTableBlockRenderer(null));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfDefaultBlockOptionsIsNull()
        {
            // Arrange
            var dummyExtensionOptions = new FlexiTableBlocksExtensionOptions()
            {
                DefaultBlockOptions = null
            };

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiTableBlockRenderer(dummyExtensionOptions));
        }

        // Table can't be serialized
        [Theory]
        [MemberData(nameof(WriteFlexiBlock_RendersFlexiTableBlock_Data))]
        public void WriteFlexiBlock_RendersFlexiTableBlock(
            Table dummyTable,
            SerializableWrapper<FlexiTableBlockOptions> dummyFlexiTableBlockOptionsWrapper,
            string expectedResult)
        {
            // Arrange
            dummyTable.SetData(FlexiTableBlocksExtension.FLEXI_TABLE_BLOCK_OPTIONS_KEY, dummyFlexiTableBlockOptionsWrapper.Value);

            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter); // Note that markdig changes dummyStringWriter.NewLine to '\n'
                FlexiTableBlockRenderer flexiTableBlockRenderer = CreateFlexiTableBlockRenderer();

                // Act
                flexiTableBlockRenderer.Write(dummyHtmlRenderer, dummyTable);
                result = dummyStringWriter.ToString();
            }

            // Assert
            Assert.Equal(expectedResult, result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> WriteFlexiBlock_RendersFlexiTableBlock_Data()
        {
            const string expectedNoWrapperElementTable = @"<div>
<table>
<thead>
<tr>
<th>a</th>
<th>b</th>
</tr>
</thead>
<tbody>
<tr>
<td data-label=""a"">0</td>
<td data-label=""b"">1</td>
</tr>
</tbody>
</table>
</div>
";
            const string expectedNoLabelAttributeTable = @"<div>
<table>
<thead>
<tr>
<th>a</th>
<th>b</th>
</tr>
</thead>
<tbody>
<tr>
<td><span>0</span></td>
<td><span>1</span></td>
</tr>
</tbody>
</table>
</div>
";
            const string dummyClass = "dummyClass";
            const string dummyWrapperElement = "dummyWrapperElement";
            const string dummyLabelAttribute = "dummyLabelAttribute";
            const string dummyAttribute = "dummyAttribute";
            const string dummyAttributeValue = "dummyAttributeValue";
            Table dummyBasicTable = CreateBasicTable();

            return new object[][]
            {
                // Renders class
                new object[]
                {
                    dummyBasicTable,
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions(dummyClass)),
                        $@"<div class=""{dummyClass}"">
<table>
<thead>
<tr>
<th>a</th>
<th>b</th>
</tr>
</thead>
<tbody>
<tr>
<td data-label=""a""><span>0</span></td>
<td data-label=""b""><span>1</span></td>
</tr>
</tbody>
</table>
</div>
"
                },
                // Does not render class if Class is null, whitespace or an empty string
                new object[]
                {
                    dummyBasicTable,
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions(null)),
                    @"<div>
<table>
<thead>
<tr>
<th>a</th>
<th>b</th>
</tr>
</thead>
<tbody>
<tr>
<td data-label=""a""><span>0</span></td>
<td data-label=""b""><span>1</span></td>
</tr>
</tbody>
</table>
</div>
"
                },
                new object[]
                {
                    dummyBasicTable,
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions(" ")),
                    @"<div>
<table>
<thead>
<tr>
<th>a</th>
<th>b</th>
</tr>
</thead>
<tbody>
<tr>
<td data-label=""a""><span>0</span></td>
<td data-label=""b""><span>1</span></td>
</tr>
</tbody>
</table>
</div>
"
                },
                new object[]
                {
                    dummyBasicTable,
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions(string.Empty)),
                    @"<div>
<table>
<thead>
<tr>
<th>a</th>
<th>b</th>
</tr>
</thead>
<tbody>
<tr>
<td data-label=""a""><span>0</span></td>
<td data-label=""b""><span>1</span></td>
</tr>
</tbody>
</table>
</div>
"
                },
                // Writes attributes if specified, if a value for the class attribute is specified, it is prepended to the default class
                new object[]
                {
                    dummyBasicTable,
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions(attributes: new Dictionary<string, string>{
                                { dummyAttribute, dummyAttributeValue },
                                {"class", dummyClass }
                            })),
                    $@"<div {dummyAttribute}=""{dummyAttributeValue}"" class=""{dummyClass} flexi-table-block"">
<table>
<thead>
<tr>
<th>a</th>
<th>b</th>
</tr>
</thead>
<tbody>
<tr>
<td data-label=""a""><span>0</span></td>
<td data-label=""b""><span>1</span></td>
</tr>
</tbody>
</table>
</div>
"
                },
                // Renders wrapper elements
                new object[]
                {
                    dummyBasicTable,
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions(null, dummyWrapperElement)),
                    $@"<div>
<table>
<thead>
<tr>
<th>a</th>
<th>b</th>
</tr>
</thead>
<tbody>
<tr>
<td data-label=""a""><{dummyWrapperElement}>0</{dummyWrapperElement}></td>
<td data-label=""b""><{dummyWrapperElement}>1</{dummyWrapperElement}></td>
</tr>
</tbody>
</table>
</div>
"
                },
                // Does not renders wrapper elements if WrapperElement is null, whitespace or an empty string
                new object[]
                {
                    dummyBasicTable,
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions(null, null)),
                    expectedNoWrapperElementTable
                },
                new object[]
                {
                    dummyBasicTable,
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions(null, " ")),
                    expectedNoWrapperElementTable
                },
                new object[]
                {
                    dummyBasicTable,
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions(null, string.Empty)),
                    expectedNoWrapperElementTable
                },
                // Renders wrapper elements
                new object[]
                {
                    dummyBasicTable,
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions(null, labelAttribute: dummyLabelAttribute)),
                    $@"<div>
<table>
<thead>
<tr>
<th>a</th>
<th>b</th>
</tr>
</thead>
<tbody>
<tr>
<td {dummyLabelAttribute}=""a""><span>0</span></td>
<td {dummyLabelAttribute}=""b""><span>1</span></td>
</tr>
</tbody>
</table>
</div>
"
                },
                // Does not renders label attributes if LabelAttribute is null, whitespace or an empty string
                new object[]
                {
                    dummyBasicTable,
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions(null, labelAttribute: null)),
                    expectedNoLabelAttributeTable
                },
                new object[]
                {
                    dummyBasicTable,
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions(null, labelAttribute: " ")),
                    expectedNoLabelAttributeTable
                },
                new object[]
                {
                    dummyBasicTable,
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions(null, labelAttribute: string.Empty)),
                    expectedNoLabelAttributeTable
                },
                // Renders colspan and rowspan
                new object[]
                {
                    CreateTableWithColspanAndRowspan(),
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions()),
                    @"<div class=""flexi-table-block"">
<table>
<tbody>
<tr>
<td colspan=""2"" rowspan=""2""><span>0</span></td>
</tr>
</tbody>
</table>
</div>
"
                },
                // Renders text-align attributes
                new object[]
                {
                    CreateTableWithTextAlign(),
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions()),
                    @"<div class=""flexi-table-block"">
<table>
<tbody>
<tr>
<td style=""text-align: left;""><span>0</span></td>
<td style=""text-align: center;""><span>1</span></td>
<td style=""text-align: right;""><span>2</span></td>
</tr>
</tbody>
</table>
</div>
"
                },
                // Renders table with only header row
                new object[]
                {
                    CreateTableWithOnlyHeaderRow(),
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions()),
                    @"<div class=""flexi-table-block"">
<table>
<thead>
<tr>
<th>a</th>
<th>b</th>
</tr>
</thead>
</table>
</div>
"
                }
            };
        }

        private FlexiTableBlockRenderer CreateFlexiTableBlockRenderer(FlexiTableBlocksExtensionOptions extensionOptions = null)
        {
            return new FlexiTableBlockRenderer(extensionOptions ?? new FlexiTableBlocksExtensionOptions());
        }

        private static Table CreateTableWithOnlyHeaderRow()
        {
            var result = new Table();
            var contents = new string[] { "a", "b"  };
            var row = new TableRow
            {
                IsHeader = true
            };

            for (int i = 0; i < contents.Length; i++)
            {
                var literalInline = new LiteralInline(contents[i]);
                var containerInline = new ContainerInline();
                containerInline.AppendChild(literalInline);
                var paragraphBlock = new ParagraphBlock
                {
                    Inline = containerInline
                };
                var tableCell = new TableCell
                {
                     paragraphBlock
                };

                row.Add(tableCell);
            }
            result.Add(row);

            return result;
        }

        private static Table CreateTableWithTextAlign()
        {
            var result = new Table();
            var row = new TableRow();
            for (int i = 0; i < 3; i++)
            {
                var literalInline = new LiteralInline(i.ToString());
                var containerInline = new ContainerInline();
                containerInline.AppendChild(literalInline);
                var paragraphBlock = new ParagraphBlock
                {
                    Inline = containerInline
                };
                var cell = new TableCell { paragraphBlock };
                row.Add(cell);
            }
            result.Add(row);

            result.ColumnDefinitions.Add(new TableColumnDefinition { Alignment = TableColumnAlign.Left });
            result.ColumnDefinitions.Add(new TableColumnDefinition { Alignment = TableColumnAlign.Center });
            result.ColumnDefinitions.Add(new TableColumnDefinition { Alignment = TableColumnAlign.Right });

            return result;
        }

        private static Table CreateTableWithColspanAndRowspan()
        {
            var result = new Table();
            var literalInline = new LiteralInline("0");
            var containerInline = new ContainerInline();
            containerInline.AppendChild(literalInline);
            var paragraphBlock = new ParagraphBlock
            {
                Inline = containerInline
            };
            var cell = new TableCell
            {
                RowSpan = 2,
                ColumnSpan = 2
            };
            cell.Add(paragraphBlock);

            var row = new TableRow
            {
                cell
            };
            result.Add(row);

            return result;
        }

        private static Table CreateBasicTable()
        {
            var result = new Table();
            var contents = new string[] { "a", "b", "0", "1", "2", "3" };
            const int numColumns = 2;
            var currentRow = new TableRow
            {
                IsHeader = true
            };

            for (int i = 0; i < contents.Length; i++)
            {
                if (i > 0 && i % numColumns == 0)
                {
                    result.Add(currentRow);
                    currentRow = new TableRow();
                }

                var literalInline = new LiteralInline(contents[i]);
                var containerInline = new ContainerInline();
                containerInline.AppendChild(literalInline);
                var paragraphBlock = new ParagraphBlock
                {
                    Inline = containerInline
                };
                var tableCell = new TableCell
                {
                     paragraphBlock
                };

                currentRow.Add(tableCell);
            }

            return result;
        }
    }
}
