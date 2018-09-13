using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using System;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiCodeBlocks
{
    public class LineNumberRangeUnitTests
    {
        [Theory]
        [MemberData(nameof(Constructor_ThrowsExceptionIfFirstLineNumberIsInvalid_Data))]
        public void Constructor_ThrowsExceptionIfFirstLineNumberIsInvalid(int firstLineNumber)
        {
            // Act and assert
            ArgumentOutOfRangeException result = Assert.Throws<ArgumentOutOfRangeException>(() => new LineNumberRange(1, 1, firstLineNumber));
            Assert.Equal(string.Format(Strings.ArgumentOutOfRangeException_LineNumberMustBeGreaterThan0, firstLineNumber) + "\nParameter name: firstLineNumber", 
                result.Message,
                ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Constructor_ThrowsExceptionIfFirstLineNumberIsInvalid_Data()
        {
            return new object[][]
            {
                new object[]{ 0 }, // Cannot be less than 1
                new object[]{ -1 } // Cannot be less than 1
            };
        }

        [Fact]
        public void Constructor_CorrectlyAssignsValuesIfSuccessful()
        {
            // Arrange
            const int dummyStartLineNumber = 1;
            const int dummyEndLineNumber = 2;
            const int dummyFirstLineNumber = 3;

            // Act
            var result = new LineNumberRange(dummyStartLineNumber, dummyEndLineNumber, dummyFirstLineNumber);

            // Assert
            Assert.Equal(dummyStartLineNumber, result.LineRange.StartLineNumber);
            Assert.Equal(dummyEndLineNumber, result.LineRange.EndLineNumber);
            Assert.Equal(dummyFirstLineNumber, result.FirstLineNumber);
        }

        [Theory]
        [MemberData(nameof(LastLineNumber_ReturnsLastLineNumber_Data))]
        public void LastLineNumber_ReturnsLastLineNumber(int dummyStartLine, int dummyEndLine, int dummyStartLineNumber, int expectedLastLineNumber)
        {
            // Arrange
            var lineNumberRange = new LineNumberRange(dummyStartLine, dummyEndLine, dummyStartLineNumber);

            // Act and assert
            Assert.Equal(expectedLastLineNumber, lineNumberRange.LastLineNumber);
        }

        public static IEnumerable<object[]> LastLineNumber_ReturnsLastLineNumber_Data()
        {
            return new object[][]
            {
                new object[]{ 2, 5, 1, 4 },
                new object[]{ 3, -1, 10, -1 } // Infinite range of lines
            };
        }

        [Fact]
        public void ToString_ReturnsLineNumberRangeAsString()
        {
            // Arrange
            var lineNumberRange = new LineNumberRange(1, 5, 10);

            // Act
            string result = lineNumberRange.ToString();

            // Assert
            Assert.Equal("Lines: [1, 5], Line numbers: [10, 14]", result);
        }
    }
}
