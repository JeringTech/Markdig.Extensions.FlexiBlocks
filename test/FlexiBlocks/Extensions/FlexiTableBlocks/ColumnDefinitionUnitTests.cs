using Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks;
using System;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiTableBlocks
{
    public class ColumnDefinitionUnitTests
    {
        [Fact]
        public void Constructor_ThrowsArgumentOutOfRangeExceptionIfStartOffsetIsNegative()
        {
            // Act and assert
            Assert.Throws<ArgumentOutOfRangeException>(() => CreateColumnDefinition(startOffset: -1));
        }

        [Theory]
        [MemberData(nameof(ThrowsArgumentOutOfRangeExceptionIfEndOffsetIsNotGreaterThanStartOffset_Data))]
        public void ThrowsArgumentOutOfRangeExceptionIfEndOffsetIsNotGreaterThanStartOffset_Name(int dummyStartOffset, int dummyEndOffset)
        {
            // Act and assert
            Assert.Throws<ArgumentOutOfRangeException>(() => CreateColumnDefinition(startOffset: dummyStartOffset, endOffset: dummyEndOffset));
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

        [Theory]
        [MemberData(nameof(Equals_ReturnsTrueIfObjIsAnIdenticalColumnDefinitionOtherwiseReturnsFalse_Data))]
        public void Equals_ReturnsTrueIfObjIsAnIdenticalColumnDefinitionOtherwiseReturnsFalse(ColumnDefinition dummyColumnDefinition,
           object dummyObj,
           bool expectedResult)
        {
            // Act
            bool result = dummyColumnDefinition.Equals(dummyObj);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> Equals_ReturnsTrueIfObjIsAnIdenticalColumnDefinitionOtherwiseReturnsFalse_Data()
        {
            return new object[][]
            {
                new object[]
                {
                    CreateColumnDefinition(),
                    "not a ColumnDefinition",
                    false
                },
                new object[]
                {
                    CreateColumnDefinition(),
                    CreateColumnDefinition(),
                    true
                },
                // False if ContentAlignment is different
                new object[]
                {
                    CreateColumnDefinition(ContentAlignment.End),
                    CreateColumnDefinition(ContentAlignment.Start),
                    false
                },
                // False if StartOffset is different
                new object[]
                {
                    CreateColumnDefinition(startOffset: 5, endOffset: 8),
                    CreateColumnDefinition(startOffset: 3, endOffset: 8),
                    false
                },
                // False if EndOffset is different
                new object[]
                {
                    CreateColumnDefinition(endOffset: 9),
                    CreateColumnDefinition(endOffset: 8),
                    false
                }
            };
        }

        private static ColumnDefinition CreateColumnDefinition(ContentAlignment contentAlignment = ContentAlignment.None,
            int startOffset = 0,
            int endOffset = 1)
        {
            return new ColumnDefinition(contentAlignment, startOffset, endOffset);
        }
    }
}
