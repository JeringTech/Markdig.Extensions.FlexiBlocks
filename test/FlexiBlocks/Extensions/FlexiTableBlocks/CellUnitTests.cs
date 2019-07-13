using Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks;
using Markdig.Helpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiTableBlocks
{
    public class CellUnitTests
    {
        [Fact]
        public void Constructor_ThrowsArgumentOutOfRangeExceptionIfStartColumnIndexIsNegative()
        {
            // Act and assert
            Assert.Throws<ArgumentOutOfRangeException>(() => CreateCell(-1));
        }

        [Fact]
        public void Constructor_ThrowsArgumentOutOfRangeExceptionIfEndColumnIndexIsLessThanStartColumnIndex()
        {
            // Act and assert
            Assert.Throws<ArgumentOutOfRangeException>(() => CreateCell(1, 0));
        }

        [Fact]
        public void Constructor_ThrowsArgumentOutOfRangeExceptionIfStartRowIndexIsNegative()
        {
            // Act and assert
            Assert.Throws<ArgumentOutOfRangeException>(() => CreateCell(startRowIndex: -1));
        }

        [Fact]
        public void Constructor_ThrowsArgumentOutOfRangeExceptionIfEndRowIndexIsLessThanStartRowIndex()
        {
            // Act and assert
            Assert.Throws<ArgumentOutOfRangeException>(() => CreateCell(startRowIndex: 1, endRowIndex: 0));
        }

        [Fact]
        public void Constructor_ThrowsArgumentOutOfRangeExceptionIfStartOffsetIsNegative()
        {
            // Act and assert
            Assert.Throws<ArgumentOutOfRangeException>(() => CreateCell(startOffset: -1));
        }

        [Theory]
        [MemberData(nameof(ThrowsArgumentOutOfRangeExceptionIfEndOffsetIsNotGreaterThanStartOffset_Data))]
        public void ThrowsArgumentOutOfRangeExceptionIfEndOffsetIsNotGreaterThanStartOffset_Name(int dummyStartOffset, int dummyEndOffset)
        {
            // Act and assert
            Assert.Throws<ArgumentOutOfRangeException>(() => CreateCell(startOffset: dummyStartOffset, endOffset: dummyEndOffset));
        }

        public static IEnumerable<object[]> ThrowsArgumentOutOfRangeExceptionIfEndOffsetIsNotGreaterThanStartOffset_Data()
        {
            return new object[][]
            {
                // Equal
                new object[]{1, 1},
                // Less than
                new object[]{1, 0}
            };
        }

        [Fact]
        public void Constructor_ThrowsArgumentOutOfRangeExceptionIfLineIndexIsNegative()
        {
            // Act and assert
            Assert.Throws<ArgumentOutOfRangeException>(() => CreateCell(lineIndex: -1));
        }

        [Theory]
        [MemberData(nameof(Equals_ReturnsTrueIfObjIsAnIdenticalCellOtherwiseReturnsFalse_Data))]
        public void Equals_ReturnsTrueIfObjIsAnIdenticalCellOtherwiseReturnsFalse(Cell dummyCell,
           object dummyObj,
           bool expectedResult)
        {
            // Act
            bool result = dummyCell.Equals(dummyObj);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> Equals_ReturnsTrueIfObjIsAnIdenticalCellOtherwiseReturnsFalse_Data()
        {
            Cell dummyCell1 = CreateCell();
            dummyCell1.Lines.Add(new StringSlice("dummyText"));
            Cell dummyCell2 = CreateCell();
            dummyCell2.Lines.Add(new StringSlice("dummyText"));
            Cell dummyCell3 = CreateCell();
            dummyCell3.Lines.Add(new StringSlice("dummyText2"));

            return new object[][]
            {
                new object[]
                {
                    CreateCell(),
                    "not a cell",
                    false
                },
                new object[]
                {
                    dummyCell1,
                    dummyCell2,
                    true
                },
                // False if cell lines differ
                new object[]
                {
                    dummyCell2,
                    dummyCell3,
                    false
                }
                // TODO other properties
            };
        }

        private static Cell CreateCell(int startColumnIndex = 0,
            int endColumnIndex = 0,
            int startRowIndex = 0,
            int endRowIndex = 0,
            int startOffset = 0,
            int endOffset = 1,
            int lineIndex = 0)
        {
            return new Cell(startColumnIndex, endColumnIndex, startRowIndex, endRowIndex, startOffset, endOffset, lineIndex);
        }
    }
}
