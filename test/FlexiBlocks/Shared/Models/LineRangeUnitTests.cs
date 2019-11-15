using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests
{
    public class LineRangeUnitTests
    {
        [Fact]
        public void Constructor_ThrowsOptionsExceptionIfStartLineIs0()
        {
            // Arrange
            const int dummyStartLine = 0;

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => new LineRange(dummyStartLine));
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                    nameof(LineRange.StartLine),
                    string.Format(Strings.OptionsException_Shared_InvalidValue,
                        dummyStartLine)),
                result.Message);
        }

        [Fact]
        public void Constructor_ThrowsOptionsExceptionIfEndLineIs0()
        {
            // Arrange
            const int dummyEndLine = 0;

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => new LineRange(endLine: dummyEndLine));
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                    nameof(LineRange.EndLine),
                    string.Format(Strings.OptionsException_Shared_InvalidValue,
                        dummyEndLine)),
                result.Message);
        }

        [Theory]
        [MemberData(nameof(Constructor_ThrowsOptionsExceptionIfStartAndEndLinesAreAnInvalidCombination_Data))]
        public void Constructor_ThrowsOptionsExceptionIfStartAndEndLinesAreAnInvalidCombination(int dummyStartLine, int dummyEndLine)
        {
            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => new LineRange(dummyStartLine, dummyEndLine));
            Assert.Equal(string.Format(Strings.OptionsException_LineRange_EndLineBeStartLineOrALineAfterIt, dummyStartLine, dummyEndLine), result.Message);
        }

        public static IEnumerable<object[]> Constructor_ThrowsOptionsExceptionIfStartAndEndLinesAreAnInvalidCombination_Data()
        {
            return new object[][]
            {
                // end line > 0 && start line > 0 && end line < start line
                new object[]{ 124, 63 },
                // end line < 0 && start line < 0 && end line < start line
                new object[]{ -10, -11 }
            };
        }

        [Theory]
        [MemberData(nameof(Constructor_DoesNotThrowAnyExceptionIfStartAndEndLinesAreAValidCombination_Data))]
        public void Constructor_DoesNotThrowAnyExceptionIfStartAndEndLinesAreAValidCombination(int dummyStartLine, int dummyEndLine)
        {
            // Act
            var result = new LineRange(dummyStartLine, dummyEndLine);

            // Assert
            Assert.Equal(dummyStartLine, result.StartLine);
            Assert.Equal(dummyEndLine, result.EndLine);
        }

        public static IEnumerable<object[]> Constructor_DoesNotThrowAnyExceptionIfStartAndEndLinesAreAValidCombination_Data()
        {
            return new object[][]
            {
                // start line> 0 && end line< 0
                new object[]{ 1231, -12 },
                // start line< 0 && end line> 0
                new object[]{-42, 5},
                // start line> 0 && end line> 0 && end line> start line
                new object[]{ 10, 124 },
                // start line< 0 && end line< 0 && end line> start line
                new object[]{ -12, -3 },
            };
        }

        [Theory]
        [MemberData(nameof(GetNormalizedStartAndEndLines_ThrowsOptionsExceptionIfNormalizedStartAndEndLinesAreAnInvalidCombination_Data))]
        public void GetNormalizedStartAndEndLines_ThrowsOptionsExceptionIfNormalizedStartAndEndLinesAreAnInvalidCombination(int dummyStartLine,
            int dummyEndLine,
            int dummyNumLines,
            int expectedNormalizedStartLine,
            int expectedNormalizedEndLine)
        {
            // Arrange
            var testSubject = new LineRange(dummyStartLine, dummyEndLine);

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => testSubject.GetNormalizedStartAndEndLines(dummyNumLines));
            Assert.Equal(string.Format(Strings.OptionsException_LineRange_UnableToNormalize, testSubject, dummyNumLines, expectedNormalizedStartLine, expectedNormalizedEndLine),
                result.Message);
        }

        public static IEnumerable<object[]> GetNormalizedStartAndEndLines_ThrowsOptionsExceptionIfNormalizedStartAndEndLinesAreAnInvalidCombination_Data()
        {
            return new object[][]
            {
                // Normalized start line < 1
                new object[]{-4, 5, 3, 0, 5},
                // Normalized start line > num lines
                new object[]{6, 7, 4, 6, 7},
                // Normalized end line < normalized start line
                new object[]{3, -5, 5, 3, 1},
                // Normalized end line > num lines
                new object[]{3, 10, 5, 3, 10}
            };
        }

        [Theory]
        [MemberData(nameof(GetNormalizedStartAndEndLines_GetsNormalizedStartAndEndLines_Data))]
        public void GetNormalizedStartAndEndLines_GetsNormalizedStartAndEndLines(int dummyStartLine,
            int dummyEndLine,
            int dummyNumLines,
            int expectedNormalizedStartLine,
            int expectedNormalizedEndLine)
        {
            // Arrange
            var testSubject = new LineRange(dummyStartLine, dummyEndLine);

            // Act
            (int normalizedStartLine, int normalizedEndLine) = testSubject.GetNormalizedStartAndEndLines(dummyNumLines);

            // Assert
            Assert.Equal(expectedNormalizedStartLine, normalizedStartLine);
            Assert.Equal(expectedNormalizedEndLine, normalizedEndLine);
        }

        public static IEnumerable<object[]> GetNormalizedStartAndEndLines_GetsNormalizedStartAndEndLines_Data()
        {
            return new object[][]
            {
                // Normalized start line == normalized end line
                new object[]{6, -5, 10, 6, 6},
                // Normalized start line < normalized end line
                new object[]{-4, -3, 8, 5, 6}
            };
        }

        [Theory]
        [MemberData(nameof(GetRelativePosition_GetsRelativePosition_Data))]
        public void GetRelativePosition_GetsRelativePosition(int dummyStartLine,
            int dummyEndLine,
            int dummyLineNumber,
            int dummyNumLines,
            int expectedResult)
        {
            // Arrange
            var testSubject = new LineRange(dummyStartLine, dummyEndLine);

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
            Assert.Equal("StartLine: 2, EndLine: 4", result);
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
            const int dummyStartLine = 4; // Arbitrary
            const int dummyEndLine = 25; // Arbitrary

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
                    new LineRange(dummyStartLine),
                    false },
                new object[]{new LineRange(),
                    new LineRange(endLine: dummyEndLine),
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
            const int dummyStartLine = 4; // Arbitrary
            const int dummyEndLine = 25; // Arbitrary

            return new object[][]
            {
                new object[]{new LineRange(),
                    new LineRange(),
                    true },
                new object[]{new LineRange(),
                    new LineRange(dummyStartLine),
                    false },
                new object[]{new LineRange(),
                    new LineRange(endLine: dummyEndLine),
                    false }
            };
        }
    }
}
