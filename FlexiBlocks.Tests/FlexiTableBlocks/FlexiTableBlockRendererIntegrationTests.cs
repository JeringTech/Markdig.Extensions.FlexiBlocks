using FlexiBlocks.FlexiTableBlocks;
using Markdig.Extensions.Tables;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace FlexiBlocks.Tests.FlexiTableBlocks
{
    public class FlexiTableBlockRendererIntegrationTests
    {
        [Theory]
        [MemberData(nameof(Write_RendersWrapperElementsIfWrapperElementNameIsNotNullWhitespaceOrAnEmptyString_Data))]
        public void Write_RendersWrapperElementsIfWrapperElementNameIsNotNullWhitespaceOrAnEmptyString(string dummyWrapperElementName, string expectedTDContentsFormat)
        {
            // Arrange
            var dummyFlexiTableBlockOptions = new FlexiTableBlockOptions
            {
                WrapperElementName = dummyWrapperElementName
            };
            Table dummyTable = CreateTable();
            dummyTable.SetData(FlexiTableBlocksExtension.FLEXI_TABLE_BLOCK_OPTIONS_KEY, dummyFlexiTableBlockOptions);
            string result = null;
            using (var stringWriter = new StringWriter())
            {
                var htmlRenderer = new HtmlRenderer(stringWriter);
                var flexiTableBlockRenderer = new FlexiTableBlockRenderer(null);

                // Act
                flexiTableBlockRenderer.Write(htmlRenderer, dummyTable);
                result = stringWriter.ToString();
            }

            // Assert
            Assert.Equal($@"<table>
<thead>
<tr>
<th>a</th>
<th>b</th>
</tr>
</thead>
<tbody>
<tr>
<td data-label=""a"">{string.Format(expectedTDContentsFormat, 0)}</td>
<td data-label=""b"">{string.Format(expectedTDContentsFormat, 1)}</td>
</tr>
</tbody>
</table>
", result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Write_RendersWrapperElementsIfWrapperElementNameIsNotNullWhitespaceOrAnEmptyString_Data()
        {
            const string dummyWrapperElementName = "dummyWrapperElementName";

            return new object[][]
            {
                new object[]
                {
                    dummyWrapperElementName,
                    $"<{dummyWrapperElementName}>{{0}}</{dummyWrapperElementName}>"
                },
                new object[]{
                    string.Empty,
                    "{0}"
                },
                new object[]{
                    null,
                    "{0}"
                },
                new object[]{
                    " ",
                    "{0}"
                }
            };
        }

        [Theory]
        [MemberData(nameof(Write_RendersLabelAttributeIfLabelAttributeNameIsNotNullWhitespaceOrAnEmptyString_Data))]
        public void Write_RendersLabelAttributeIfLabelAttributeNameIsNotNullWhitespaceOrAnEmptyString(string dummyLabelAttributeName, string expectedTDElementFormat)
        {
            // Arrange
            var dummyFlexiTableBlockOptions = new FlexiTableBlockOptions
            {
                LabelAttributeName = dummyLabelAttributeName
            };
            Table dummyTable = CreateTable();
            dummyTable.SetData(FlexiTableBlocksExtension.FLEXI_TABLE_BLOCK_OPTIONS_KEY, dummyFlexiTableBlockOptions);
            string result = null;
            using (var stringWriter = new StringWriter())
            {
                var htmlRenderer = new HtmlRenderer(stringWriter);
                var flexiTableBlockRenderer = new FlexiTableBlockRenderer(null);

                // Act
                flexiTableBlockRenderer.Write(htmlRenderer, dummyTable);
                result = stringWriter.ToString();
            }

            // Assert
            Assert.Equal($@"<table>
<thead>
<tr>
<th>a</th>
<th>b</th>
</tr>
</thead>
<tbody>
<tr>
{string.Format(expectedTDElementFormat, "a", "<span>0</span>")}
{string.Format(expectedTDElementFormat, "b", "<span>1</span>")}
</tr>
</tbody>
</table>
", result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Write_RendersLabelAttributeIfLabelAttributeNameIsNotNullWhitespaceOrAnEmptyString_Data()
        {
            const string dummyLabelAttributeName = "dummyLabelAttributeName";

            return new object[][]
            {
                new object[]
                {
                    dummyLabelAttributeName,
                    $"<td {dummyLabelAttributeName}=\"{{0}}\">{{1}}</td>"
                },
                new object[]{
                    string.Empty,
                    "<td>{1}</td>"
                },
                new object[]{
                    null,
                    "<td>{1}</td>"
                },
                new object[]{
                    " ",
                    "<td>{1}</td>"
                }
            };
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
