using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests
{
    public class LineRangeUnitTests
    {
        [Fact]
        public void Constructor_ThrowsOptionsExceptionIfStartIs0()
        {
            // Arrange
            const int dummyStart = 0;

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => new LineRange(dummyStart));
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                    nameof(LineRange.Start),
                    string.Format(Strings.OptionsException_Shared_InvalidValue,
                        dummyStart)),
                result.Message);
        }

        [Fact]
        public void Constructor_ThrowsOptionsExceptionIfEndIs0()
        {
            // Arrange
            const int dummyEnd = 0;

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => new LineRange(end: dummyEnd));
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                    nameof(LineRange.End),
                    string.Format(Strings.OptionsException_Shared_InvalidValue,
                        dummyEnd)),
                result.Message);
        }

        [Theory]
        [MemberData(nameof(Constructor_ThrowsOptionsExceptionIfStartAndEndAreAnInvalidCombination_Data))]
        public void Constructor_ThrowsOptionsExceptionIfStartAndEndAreAnInvalidCombination(int dummyStart, int dummyEnd)
        {
            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => new LineRange(dummyStart, dummyEnd));
            Assert.Equal(string.Format(Strings.OptionsException_LineRange_EndLineBeStartLineOrALineAfterIt, dummyStart, dummyEnd), result.Message);
        }

        public static IEnumerable<object[]> Constructor_ThrowsOptionsExceptionIfStartAndEndAreAnInvalidCombination_Data()
        {
            return new object[][]
            {
                // end > 0 && start > 0 && end < start
                new object[]{ 124, 63 },
                // end < 0 && start < 0 && end < start
                new object[]{ -10, -11 }
            };
        }

        [Theory]
        [MemberData(nameof(Constructor_DoesNotThrowAnyExceptionIfStartAndEndAreAValidCombination_Data))]
        public void Constructor_DoesNotThrowAnyExceptionIfStartAndEndAreAValidCombination(int dummyStart, int dummyEnd)
        {
            // Act
            var result = new LineRange(dummyStart, dummyEnd);

            // Assert
            Assert.Equal(dummyStart, result.Start);
            Assert.Equal(dummyEnd, result.End);
        }

        public static IEnumerable<object[]> Constructor_DoesNotThrowAnyExceptionIfStartAndEndAreAValidCombination_Data()
        {
            return new object[][]
            {
                // start > 0 && end < 0
                new object[]{ 1231, -12 },
                // start < 0 && end > 0
                new object[]{-42, 5},
                // start > 0 && end > 0 && end > start
                new object[]{ 10, 124 },
                // start < 0 && end < 0 && end > start
                new object[]{ -12, -3 },
            };
        }

        [Theory]
        [MemberData(nameof(GetNormalizedStartAndEnd_ThrowsOptionsExceptionIfNormalizedStartAndEndAreAnInvalidCombination_Data))]
        public void GetNormalizedStartAndEnd_ThrowsOptionsExceptionIfNormalizedStartAndEndAreAnInvalidCombination(int dummyStart,
            int dummyEnd,
            int dummyNumLines,
            int expectedNormalizedStart,
            int expectedNormalizedEnd)
        {
            // Arrange
            var testSubject = new LineRange(dummyStart, dummyEnd);

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => testSubject.GetNormalizedStartAndEnd(dummyNumLines));
            Assert.Equal(string.Format(Strings.OptionsException_LineRange_UnableToNormalize, testSubject, dummyNumLines, expectedNormalizedStart, expectedNormalizedEnd),
                result.Message);
        }

        public static IEnumerable<object[]> GetNormalizedStartAndEnd_ThrowsOptionsExceptionIfNormalizedStartAndEndAreAnInvalidCombination_Data()
        {
            return new object[][]
            {
                // Normalized start < 1
                new object[]{-4, 5, 3, 0, 5},
                // Normalized start > num lines
                new object[]{6, 7, 4, 6, 7},
                // Normalized end < normalized start
                new object[]{3, -5, 5, 3, 1},
                // Normalized end > num lines
                new object[]{3, 10, 5, 3, 10}
            };
        }

        [Theory]
        [MemberData(nameof(GetNormalizedStartAndEnd_GetsNormalizedStartAndEnd_Data))]
        public void GetNormalizedStartAndEnd_GetsNormalizedStartAndEnd(int dummyStart,
            int dummyEnd,
            int dummyNumLines,
            int expectedNormalizedStart,
            int expectedNormalizedEnd)
        {
            // Arrange
            var testSubject = new LineRange(dummyStart, dummyEnd);

            // Act
            (int normalizedStart, int normalizedEnd) = testSubject.GetNormalizedStartAndEnd(dummyNumLines);

            // Assert
            Assert.Equal(expectedNormalizedStart, normalizedStart);
            Assert.Equal(expectedNormalizedEnd, normalizedEnd);
        }

        public static IEnumerable<object[]> GetNormalizedStartAndEnd_GetsNormalizedStartAndEnd_Data()
        {
            return new object[][]
            {
                // Normalized start == normalized end
                new object[]{6, -5, 10, 6, 6},
                // Normalized start < normalized end
                new object[]{-4, -3, 8, 5, 6}
            };
        }

        [Theory]
        [MemberData(nameof(GetRelativePosition_GetsRelativePosition_Data))]
        public void GetRelativePosition_GetsRelativePosition(int dummyStart,
            int dummyEnd,
            int dummyLineNumber,
            int dummyNumLines,
            int expectedResult)
        {
            // Arrange
            var testSubject = new LineRange(dummyStart, dummyEnd);

            // Act
            int result = testSubject.GetRelativePosition(dummyLineNumber, dummyNumLines);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> GetRelativePosition_GetsRelativePosition_Data()
        {
            return new object[][]
            {
                // Before line
                new object[]{ -9, 15, 21, 18, -1 },
                // Contains line
                new object[]{3, 123, 3, 291, 0},
                new object[]{3, 123, 123, 291, 0},
                new object[]{3, 123, 24, 291, 0},
                // After line
                new object[]{ -12, -3, 1, 15, 1}
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
            Assert.Equal("Start: 2, End: 4", result);
        }

        [Theory]
        [MemberData(nameof(Equals_ReturnsTrueIfObjIsAnIdenticalLineRangeOtherwiseReturnsFalse_Data))]
        public void Equals_ReturnsTrueIfObjIsAnIdenticalLineRangeOtherwiseReturnsFalse(LineRange dummyLineRange,
            object dummyObj,
            bool expectedResult)
        {
            // Act
            bool result = dummyLineRange.Equals(dummyObj);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> Equals_ReturnsTrueIfObjIsAnIdenticalLineRangeOtherwiseReturnsFalse_Data()
        {
            const int dummyStart = 4; // Arbitrary
            const int dummyEnd = 25; // Arbitrary

            return new object[][]
            {
                new object[]{new LineRange(),
                    "not a line range",
                    false },
                new object[]{new LineRange(),
                    new LineRange(),
                    true },
                // False if the LineRanges differ in any way
                new object[]{new LineRange(),
                    new LineRange(dummyStart),
                    false },
                new object[]{new LineRange(),
                    new LineRange(end: dummyEnd),
                    false },
            };
        }

        [Theory]
        [MemberData(nameof(GetHashCode_ReturnsSameHashCodeForIdenticalLineRanges_Data))]
        public void GetHashCode_ReturnsSameHashCodeForIdenticalLineRanges(LineRange dummyLineRange1,
            LineRange dummyLineRange2,
            bool identical)
        {
            // Act and assert
            Assert.Equal(identical, dummyLineRange1.GetHashCode() == dummyLineRange2.GetHashCode());
        }

        public static IEnumerable<object[]> GetHashCode_ReturnsSameHashCodeForIdenticalLineRanges_Data()
        {
            const int dummyStart = 4; // Arbitrary
            const int dummyEnd = 25; // Arbitrary

            return new object[][]
            {
                new object[]{new LineRange(),
                    new LineRange(),
                    true },
                new object[]{new LineRange(),
                    new LineRange(dummyStart),
                    false },
                new object[]{new LineRange(),
                    new LineRange(end: dummyEnd),
                    false }
            };
        }
    }
}
