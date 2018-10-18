using Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks;
using Markdig.Extensions.Tables;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Moq;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiTableBlocks
{
    public class FlexiTableBlockRendererUnitTests
    {
        [Theory]
        [MemberData(nameof(WriteFlexiBlock_RendersFlexiTableBlock_Data))]
        public void WriteFlexiBlock_RendersFlexiTableBlock(SerializableWrapper<FlexiTableBlockOptions> dummyFlexiTableBlockOptionsWrapper, string expectedResult)
        {
            // Arrange
            Table dummyTable = CreateTable();
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
            const string expectedNoWrapperElementTable = @"<table>
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
";
            const string expectedNoLabelAttributeTable = @"<table>
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
";
            const string dummyClass = "dummyClass";
            const string dummyWrapperElement = "dummyWrapperElement";
            const string dummyLabelAttribute = "dummyLabelAttribute";
            const string dummyAttribute = "dummyAttribute";
            const string dummyAttributeValue = "dummyAttributeValue";

            return new object[][]
            {
                // Renders class
                new object[]
                {
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions(dummyClass)),
                    $@"<table class=""{dummyClass}"">
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
"
                },
                // Does not render class if Class is null, whitespace or an empty string
                new object[]
                {
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions(null)),
                    @"<table>
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
"
                },
                new object[]
                {
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions(" ")),
                    @"<table>
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
"
                },
                new object[]
                {
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions(string.Empty)),
                    @"<table>
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
"
                },
                // Writes attributes if specified, if a value for the class attribute is specified, it is prepended to the default class
                new object[]
                {
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions(attributes: new Dictionary<string, string>{
                                { dummyAttribute, dummyAttributeValue },
                                {"class", dummyClass }
                            })),
                    $@"<table {dummyAttribute}=""{dummyAttributeValue}"" class=""{dummyClass} flexi-table-block"">
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
"
                },
                // Renders wrapper elements
                new object[]
                {
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions(null, dummyWrapperElement)),
                    $@"<table>
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
"
                },
                // Does not renders wrapper elements if WrapperElement is null, whitespace or an empty string
                new object[]
                {
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions(null, null)),
                    expectedNoWrapperElementTable
                },
                new object[]
                {
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions(null, " ")),
                    expectedNoWrapperElementTable
                },
                new object[]
                {
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions(null, string.Empty)),
                    expectedNoWrapperElementTable
                },
                // Renders wrapper elements
                new object[]
                {
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions(null, labelAttribute: dummyLabelAttribute)),
                    $@"<table>
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
"
                },
                // Does not renders label attributes if LabelAttribute is null, whitespace or an empty string
                new object[]
                {
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions(null, labelAttribute: null)),
                    expectedNoLabelAttributeTable
                },
                new object[]
                {
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions(null, labelAttribute: " ")),
                    expectedNoLabelAttributeTable
                },
                new object[]
                {
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions(null, labelAttribute: string.Empty)),
                    expectedNoLabelAttributeTable
                }
            };
        }

        private FlexiTableBlockRenderer CreateFlexiTableBlockRenderer(FlexiTableBlocksExtensionOptions extensionOptions = null)
        {
            return new FlexiTableBlockRenderer(extensionOptions ?? new FlexiTableBlocksExtensionOptions());
        }

        private Table CreateTable()
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
