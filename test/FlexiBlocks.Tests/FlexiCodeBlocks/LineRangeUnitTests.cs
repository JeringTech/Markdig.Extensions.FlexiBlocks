using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using System;
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
            Assert.Equal("2 - 4", result);
        }

        [Theory]
        [MemberData(nameof(Constructor_ThrowsArgumentOutOfrangeExceptionIfStartLineNumberIsLessThan1_Data))]
        public void Constructor_ThrowsArgumentOutOfrangeExceptionIfStartLineNumberIsLessThan1(int dummyStartLine)
        {
            // Act and assert
            ArgumentOutOfRangeException result = Assert.Throws<ArgumentOutOfRangeException>(() => new LineRange(dummyStartLine, 0));
            Assert.Equal(string.Format(Strings.ArgumentException_LineNumberMustBeGreaterThan0, dummyStartLine) + "\nParameter name: startLineNumber", 
                result.Message,
                ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Constructor_ThrowsArgumentOutOfrangeExceptionIfStartLineNumberIsLessThan1_Data()
        {
            return new object[][]
            {
                new object[]{ 0 },
                new object[]{ -1 }
            };
        }

        [Theory]
        [MemberData(nameof(Constructor_ThrowsArgumentOutOfRangeExceptionIfEndLineNumberIsInvalid_Data))]
        public void Constructor_ThrowsArgumentOutOfRangeExceptionIfEndLineNumberIsInvalid(int dummyStartLineNumber, int dummyEndLineNumber)
        {
            // Act and assert
            ArgumentOutOfRangeException result = Assert.Throws<ArgumentOutOfRangeException>(() => new LineRange(dummyStartLineNumber, dummyEndLineNumber));
            Assert.Equal(string.Format(Strings.ArgumentException_EndLineNumberMustBeMinus1OrGreaterThanOrEqualToStartLineNumber, dummyEndLineNumber, dummyStartLineNumber) + "\nParameter name: endLineNumber",
                result.Message,
                ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Constructor_ThrowsArgumentOutOfRangeExceptionIfEndLineNumberIsInvalid_Data()
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
        [MemberData(nameof(CompareTo_ReturnsMinus1IfRangeOccursBeforeLineRange0IfTheRangesOverlapAnd1IfRangeOccursAfterLineRange_Data))]
        public void CompareTo_ReturnsMinus1IfRangeOccursBeforeLineRange0IfTheRangesOverlapAnd1IfRangeOccursAfterLineRange(
            int primaryRangeStartLine, int primaryRangeEndLine,
            int secondaryRangeStartLine, int secondaryRangeEndLine,
            int expectedResult)
        {
            // Arrange
            var primaryLineRange = new LineRange(primaryRangeStartLine, primaryRangeEndLine);
            var secondaryLineRange = new LineRange(secondaryRangeStartLine, secondaryRangeEndLine);

            // Act
            int result = primaryLineRange.CompareTo(secondaryLineRange);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> CompareTo_ReturnsMinus1IfRangeOccursBeforeLineRange0IfTheRangesOverlapAnd1IfRangeOccursAfterLineRange_Data()
        {
            return new object[][]
            {
                new object[]{ 1, 5, 6, 10, -1 }, // Before
                new object[]{ 11, 15, 6, 10, 1 }, // After
                new object[]{ 2, 7, 7, 11, 0 }, // Overlap at end of main line range
                new object[]{ 11, 15, 7, 11, 0 }, // Overlap at start of main line range
                new object[]{ 5, 12, 8, 12, 0 }, // Main line range contains other line range
                new object[]{ 5, -1, 1000, 1234, 0 }, // Main line range contains other line range
                new object[]{ 8, 12, 5, 12, 0 }, // Other line range contains main line range
                new object[]{ 1000, 1234, 5, -1, 0 }, // Other line range contains main line range
            };
        }
    }
}
