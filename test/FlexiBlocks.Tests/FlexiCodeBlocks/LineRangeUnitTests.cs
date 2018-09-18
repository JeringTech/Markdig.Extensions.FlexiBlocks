using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiCodeBlocks
{
    public class LineRangeUnitTests
    {
        [Theory]
        [MemberData(nameof(NumLines_ReturnsNumberOfLinesInRange_Data))]
        public void NumLines_ReturnsNumberOfLinesInRange(int startLineNumber, int endLineNumber, int expectedNumLines)
        {
            // Arrange
            var lineRange = new LineRange(startLineNumber, endLineNumber);

            // Assert
            Assert.Equal(expectedNumLines, lineRange.NumLines);
        }

        public static IEnumerable<object[]> NumLines_ReturnsNumberOfLinesInRange_Data()
        {
            return new object[][]
            {
                new object[]{ 2, 5, 4 },
                new object[]{ 4, 4, 1 }, // Single line
                new object[]{ 3, -1, -1 } // Infinite number of lines
            };
        }

        [Fact]
        public void ToString_ReturnsLineRangeAsString()
        {
            // Arrange
            var lineRange = new LineRange(2, 4);

            // Act
            string result = lineRange.ToString();

            // Assert
            Assert.Equal("[2, 4]", result);
        }

        [Theory]
        [MemberData(nameof(Constructor_ThrowsFlexiBlocksExceptionIfStartLineNumberIsLessThan1_Data))]
        public void Constructor_ThrowsFlexiBlocksExceptionIfStartLineNumberIsLessThan1(int dummyStartLine)
        {
            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => new LineRange(dummyStartLine, 0));
            Assert.Equal(string.Format(Strings.FlexiBlocksException_OptionMustBeGreaterThan0, nameof(LineRange.StartLineNumber), dummyStartLine),
                result.Message,
                ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Constructor_ThrowsFlexiBlocksExceptionIfStartLineNumberIsLessThan1_Data()
        {
            return new object[][]
            {
                new object[]{ 0 },
                new object[]{ -1 }
            };
        }

        [Theory]
        [MemberData(nameof(Constructor_ThrowsFlexiBlocksExceptionIfEndLineNumberIsInvalid_Data))]
        public void Constructor_ThrowsFlexiBlocksExceptionIfEndLineNumberIsInvalid(int dummyStartLineNumber, int dummyEndLineNumber)
        {
            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => new LineRange(dummyStartLineNumber, dummyEndLineNumber));
            Assert.Equal(string.Format(Strings.FlexiBlocksException_EndLineNumberMustBeMinus1OrGreaterThanOrEqualToStartLineNumber, nameof(LineRange.EndLineNumber),
                    dummyEndLineNumber, dummyStartLineNumber),
                result.Message,
                ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Constructor_ThrowsFlexiBlocksExceptionIfEndLineNumberIsInvalid_Data()
        {
            return new object[][]
            {
                new object[]{ 2, 1 },
                new object[]{ 1, -2 },
            };
        }

        [Theory]
        [MemberData(nameof(Constructor_CorrectlyAssignsValuesIfSuccessful_Data))]
        public void Constructor_CorrectlyAssignsValuesIfSuccessful(int dummyStartLineNumber, int dummyEndLineNumber)
        {
            // Act
            var result = new LineRange(dummyStartLineNumber, dummyEndLineNumber);

            // Assert
            Assert.Equal(dummyStartLineNumber, result.StartLineNumber);
            Assert.Equal(dummyEndLineNumber, result.EndLineNumber);
        }

        public static IEnumerable<object[]> Constructor_CorrectlyAssignsValuesIfSuccessful_Data()
        {
            return new object[][]
            {
                new object[]{ 2, -1 }, // -1 = infinity
                new object[]{ 1, 1 }, // Start line number can be the same as end line number
                new object[]{ 2, 3}
            };
        }

        [Theory]
        [MemberData(nameof(Contains_ReturnsTrueIfRangeContainsLineOtherwiseReturnsFalse_Data))]
        public void Contains_ReturnsTrueIfRangeContainsLineOtherwiseReturnsFalse(int startLineNumber, int endLineNumber, int dummyLineNumber, bool expectedResult)
        {
            // Arrange
            var lineRange = new LineRange(startLineNumber, endLineNumber);

            // Act
            bool result = lineRange.Contains(dummyLineNumber);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> Contains_ReturnsTrueIfRangeContainsLineOtherwiseReturnsFalse_Data()
        {
            return new object[][]
            {
                new object[]{ 10, 12, 10, true}, // Range is inclusive of start line number
                new object[]{ 1, 5, 5, true}, // Range is inclusive of end line number
                new object[]{ 2, 2, 2, true}, // Single line range
                new object[]{ 3, -1, 1000, true}, // -1 = infinity
                new object[]{ 4, 8, -1, false }, // Negative numbers can't be in a range
                new object[]{ 9, 13, 0, false }, // 0 can't be in a range
                new object[]{ 11, 14, 10, false }, // Before range
                new object[]{ 22, 105, 106, false }, // After range
            };
        }

        [Theory]
        [MemberData(nameof(Before_ReturnsTrueIfRangeOccursBeforeLineOtherwiseReturnsFalse_Data))]
        public void Before_ReturnsTrueIfRangeOccursBeforeLineOtherwiseReturnsFalse(int startLineNumber, int endLineNumber, int dummyLineNumber, bool expectedResult)
        {
            // Arrange
            var lineRange = new LineRange(startLineNumber, endLineNumber);

            // Act
            bool result = lineRange.Before(dummyLineNumber);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> Before_ReturnsTrueIfRangeOccursBeforeLineOtherwiseReturnsFalse_Data()
        {
            return new object[][]
            {
                new object[]{ 2, 5, 6, true}, // Range occurs just before line
                new object[]{ 2, 5, 1, false}, // Range occurs after line
                new object[]{ 2, 5, 3, false}, // Range contains line
                new object[]{ 2, -1, 6, false}, // Range contains line
                new object[]{ 2, 5, 5, false}, // Range contains line
                new object[]{ 2, 5, 2, false}, // Range contains line
            };
        }
    }
}
