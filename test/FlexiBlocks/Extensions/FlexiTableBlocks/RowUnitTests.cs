using Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks;
using Markdig.Helpers;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiTableBlocks
{
    public class RowUnitTests
    {
        [Theory]
        [MemberData(nameof(Equals_ReturnsTrueIfObjIsAnIdenticalRowOtherwiseReturnsFalse_Data))]
        public void Equals_ReturnsTrueIfObjIsAnIdenticalRowOtherwiseReturnsFalse(Row dummyRow,
           object dummyObj,
           bool expectedResult)
        {
            // Act
            bool result = dummyRow.Equals(dummyObj);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> Equals_ReturnsTrueIfObjIsAnIdenticalRowOtherwiseReturnsFalse_Data()
        {
            Cell dummyCell1 = CreateCell();
            dummyCell1.Lines.Add(new StringSlice("dummyText1"));
            Cell dummyCell2 = CreateCell();
            dummyCell2.Lines.Add(new StringSlice("dummyText2"));
            Cell dummyCell3 = CreateCell();
            dummyCell3.Lines.Add(new StringSlice("dummyText3"));

            return new object[][]
            {
                new object[]
                {
                    new Row(),
                    "not a Row",
                    false
                },
                new object[]
                {
                    new Row(){ dummyCell1, dummyCell2 },
                    new Row(){ dummyCell1, dummyCell2 },
                    true
                },
                // False if cells are different
                new object[]
                {
                    new Row(){ dummyCell3, dummyCell2 },
                    new Row(){ dummyCell1, dummyCell2 },
                    false
                },
                // False if IsHeaderRow is different
                new object[]
                {
                    new Row(){ IsHeaderRow = false },
                    new Row(){ IsHeaderRow = true },
                    false
                },
                // False if IsOpen is different
                new object[]
                {
                    new Row(){ IsOpen = false },
                    new Row(){ IsOpen = true },
                    false
                }
            };
        }

        private static Cell CreateCell(int startColumnIndex = 0,
            int endColumnIndex = 0,
            int startCellIndex = 0,
            int endCellIndex = 0,
            int startOffset = 0,
            int endOffset = 1,
            int lineIndex = 0)
        {
            return new Cell(startColumnIndex, endColumnIndex, startCellIndex, endCellIndex, startOffset, endOffset, lineIndex);
        }
    }
}
