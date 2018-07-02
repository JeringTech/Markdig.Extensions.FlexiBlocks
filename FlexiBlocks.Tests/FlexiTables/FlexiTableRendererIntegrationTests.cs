using FlexiBlocks.ResponsiveTables;
using Markdig.Extensions.Tables;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace FlexiBlocks.Tests.ResponsiveTables
{
    /// <summary>
    /// Refer to ResponsiveTablesSpecs.md for additional tests.
    /// </summary>
    public class FlexiTableRendererIntegrationTests
    {
        [Theory]
        [MemberData(nameof(Write_DoesNotWrapTDElementsIfWrapperElementNameIsNullWhitespaceOrEmpty_Data))]
        public void Write_DoesNotWrapTDElementsIfWrapperElementNameIsNullWhitespaceOrEmpty(FlexiTableOptions dummyOptions)
        {
            // Arrange
            Table dummyTable = CreateTable();
            string result = null;
            using(var stringWriter = new StringWriter())
            {
                var htmlRenderer = new HtmlRenderer(stringWriter);
                var responsiveTableRenderer = new FlexiTableRenderer(dummyOptions);

                // Act
                responsiveTableRenderer.Write(htmlRenderer, dummyTable);
                result = stringWriter.ToString();
            }

            // Assert
            Assert.Equal(@"<table>
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
", result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Write_DoesNotWrapTDElementsIfWrapperElementNameIsNullWhitespaceOrEmpty_Data()
        {
            return new object[][]
            {
                new object[]{
                    new FlexiTableOptions(){WrapperElementName = string.Empty}
                },
                new object[]{
                    new FlexiTableOptions(){WrapperElementName = null}
                },
                new object[]{
                    new FlexiTableOptions(){WrapperElementName = " "}
                }
            };
        }

        [Theory]
        [MemberData(nameof(Write_DoesNotRenderLabelAttributeIfLabelAttributeNameIsNullWhitespaceOrEmpty_Data))]
        public void Write_DoesNotRenderLabelAttributeIfLabelAttributeNameIsNullWhitespaceOrEmpty(FlexiTableOptions dummyOptions)
        {
            // Arrange
            Table dummyTable = CreateTable();
            string result = null;
            using (var stringWriter = new StringWriter())
            {
                var htmlRenderer = new HtmlRenderer(stringWriter);
                var responsiveTableRenderer = new FlexiTableRenderer(dummyOptions);

                // Act
                responsiveTableRenderer.Write(htmlRenderer, dummyTable);
                result = stringWriter.ToString();
            }

            // Assert
            Assert.Equal(@"<table>
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
", result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Write_DoesNotRenderLabelAttributeIfLabelAttributeNameIsNullWhitespaceOrEmpty_Data()
        {
            return new object[][]
            {
                new object[]{
                    new FlexiTableOptions(){LabelAttributeName = string.Empty}
                },
                new object[]{
                    new FlexiTableOptions(){LabelAttributeName = null}
                },
                new object[]{
                    new FlexiTableOptions(){LabelAttributeName = " "}
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
