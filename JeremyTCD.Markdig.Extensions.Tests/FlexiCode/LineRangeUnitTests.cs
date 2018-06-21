using JeremyTCD.Markdig.Extensions.FlexiCode;
using System;
using System.Collections.Generic;
using Xunit;

namespace JeremyTCD.Markdig.Extensions.Tests.FlexiCode
{
    public class LineRangeUnitTests
    {
        [Theory]
        [MemberData(nameof(Constructor_ThrowsExceptionIfStartLineIsInvalid_Data))]
        public void Constructor_ThrowsExceptionIfStartLineIsInvalid(int dummyStartLine)
        {
            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => new LineRange(dummyStartLine, 0));
            Assert.Equal(string.Format(Strings.ArgumentException_InvalidStartLine, dummyStartLine), result.Message);
        }

        public static IEnumerable<object[]> Constructor_ThrowsExceptionIfStartLineIsInvalid_Data()
        {
            return new object[][]
            {
                new object[]{ 0 },
                new object[]{ -1 }
            };
        }

        [Theory]
        [MemberData(nameof(Constructor_ThrowsExceptionIfEndLineIsInvalid_Data))]
        public void Constructor_ThrowsExceptionIfEndLineIsInvalid(int dummyStartLine, int dummyEndLine)
        {
            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => new LineRange(dummyStartLine, dummyEndLine));
            Assert.Equal(string.Format(Strings.ArgumentException_InvalidEndLine, dummyEndLine, dummyStartLine), result.Message);
        }

        public static IEnumerable<object[]> Constructor_ThrowsExceptionIfEndLineIsInvalid_Data()
        {
            return new object[][]
            {
                new object[]{ 2, 1 },
                new object[]{ 1, -2 },
            };
        }

        [Theory]
        [MemberData(nameof(Constructor_CorrectlyAssignsValuesIfSuccessful_Data))]
        public void Constructor_CorrectlyAssignsValuesIfSuccessful(int dummyStartLine, int dummyEndLine)
        {
            // Act
            var result = new LineRange(dummyStartLine, dummyEndLine);

            // Assert
            Assert.Equal(dummyStartLine, result.StartLine);
            Assert.Equal(dummyEndLine, result.EndLine);
        }

        public static IEnumerable<object[]> Constructor_CorrectlyAssignsValuesIfSuccessful_Data()
        {
            return new object[][]
            {
                new object[]{ 2, -1 }, // -1 = infinity
                new object[]{ 1, 1 }, // Start line can be the same as end line
                new object[]{ 2, 3}
            };
        }

        [Theory]
        [MemberData(nameof(Contains_ReturnsTrueIfRangeContainsLineOtherwiseReturnsFalse_Data))]
        public void Contains_ReturnsTrueIfRangeContainsLineOtherwiseReturnsFalse(LineRange dummyLineRange, int dummyLine, bool expectedResult)
        {
            // Act
            bool result = dummyLineRange.Contains(dummyLine);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> Contains_ReturnsTrueIfRangeContainsLineOtherwiseReturnsFalse_Data()
        {
            return new object[][]
            {
                new object[]{ new LineRange(10, 12), 10, true}, // Range is inclusive of start line
                new object[]{ new LineRange(1, 5), 5, true}, // Range is inclusive of end line
                new object[]{ new LineRange(2, 2), 2, true}, // Single line range
                new object[]{ new LineRange(3, -1), 1000, true}, // -1 = infinity
                new object[]{ new LineRange(4, 8), -1, false }, // Negative numbers can't be in a range
                new object[]{ new LineRange(9, 13), 0, false }, // 0 can't be in a range
                new object[]{ new LineRange(11, 14), 10, false }, // Before range
                new object[]{ new LineRange(22, 105), 106, false }, // After range
            };
        }

        [Theory]
        [MemberData(nameof(CompareTo_ReturnsMinus1IfCurrentRangeOccursBeforeLineRange0IfTheRangesOverlapAnd1IfCurrentRangeOccursAfterLineRange_Data))]
        public void CompareTo_ReturnsMinus1IfCurrentRangeOccursBeforeLineRange0IfTheRangesOverlapAnd1IfCurrentRangeOccursAfterLineRange(
            LineRange dummyMainLineRange, LineRange dummyOtherLineRange, int expectedResult)
        {
            // Act
            int result = dummyMainLineRange.CompareTo(dummyOtherLineRange);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> CompareTo_ReturnsMinus1IfCurrentRangeOccursBeforeLineRange0IfTheRangesOverlapAnd1IfCurrentRangeOccursAfterLineRange_Data()
        {
            return new object[][]
            {
                new object[]{ new LineRange(1, 5), new LineRange(6, 10), -1 }, // Before
                new object[]{ new LineRange(11, 15), new LineRange(6, 10), 1 }, // After
                new object[]{ new LineRange(2, 7), new LineRange(7, 11), 0 }, // Overlap at end of main line range
                new object[]{ new LineRange(11, 15), new LineRange(7, 11), 0 }, // Overlap at start of main line range
                new object[]{ new LineRange(5, 12), new LineRange(8, 12), 0 }, // Main line range contains other line range
                new object[]{ new LineRange(5, -1), new LineRange(1000, 1234), 0 }, // Main line range contains other line range
                new object[]{ new LineRange(8, 12), new LineRange(5, 12), 0 }, // Other line range contains main line range
                new object[]{ new LineRange(1000, 1234), new LineRange(5, -1), 0 }, // Other line range contains main line range
            };
        }
    }
}
