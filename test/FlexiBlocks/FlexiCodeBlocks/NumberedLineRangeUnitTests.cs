using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiCodeBlocks
{
    public class NumberedLineRangeUnitTests
    {
        [Theory]
        [MemberData(nameof(Constructor_ThrowsFlexiBlocksExceptionIfFirstNumberIsInvalid_Data))]
        public void Constructor_ThrowsFlexiBlocksExceptionIfFirstNumberIsInvalid(int firstNumber)
        {
            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => new NumberedLineRange(1, 1, firstNumber));
            Assert.Equal(string.Format(Strings.FlexiBlocksException_OptionMustBeGreaterThan0, nameof(NumberedLineRange.FirstNumber), firstNumber),
                result.Message,
                ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Constructor_ThrowsFlexiBlocksExceptionIfFirstNumberIsInvalid_Data()
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
            const int dummyFirstNumber = 3;

            // Act
            var result = new NumberedLineRange(dummyStartLineNumber, dummyEndLineNumber, dummyFirstNumber);

            // Assert
            Assert.Equal(dummyStartLineNumber, result.StartLineNumber);
            Assert.Equal(dummyEndLineNumber, result.EndLineNumber);
            Assert.Equal(dummyFirstNumber, result.FirstNumber);
        }

        [Theory]
        [MemberData(nameof(LastLineNumber_ReturnsLastLineNumber_Data))]
        public void LastLineNumber_ReturnsLastLineNumber(int dummyStartLine, int dummyEndLine, int dummyStartLineNumber, int expectedLastLineNumber)
        {
            // Arrange
            var numberedLineRange = new NumberedLineRange(dummyStartLine, dummyEndLine, dummyStartLineNumber);

            // Act and assert
            Assert.Equal(expectedLastLineNumber, numberedLineRange.LastLineNumber);
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
        public void ToString_ReturnsNumberedLineRangeAsString()
        {
            // Arrange
            var numberedLineRange = new NumberedLineRange(1, 5, 10);

            // Act
            string result = numberedLineRange.ToString();

            // Assert
            Assert.Equal("Lines: [1, 5], Line numbers: [10, 14]", result);
        }
    }
}
