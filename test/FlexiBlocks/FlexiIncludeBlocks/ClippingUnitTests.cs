using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiIncludeBlocks
{
    public class ClippingUnitTests
    {
        [Theory]
        [MemberData(nameof(Constructor_ThrowsFlexiBlocksExceptionIfStartLineNumberIsLessThan1_Data))]
        public void Constructor_ThrowsFlexiBlocksExceptionIfStartLineNumberIsLessThan1(int dummyStartLineNumber)
        {
            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => new Clipping(dummyStartLineNumber));
            Assert.Equal(string.Format(Strings.FlexiBlocksException_Shared_OptionMustBeGreaterThan0, nameof(Clipping.StartLineNumber), dummyStartLineNumber),
                result.Message,
                ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Constructor_ThrowsFlexiBlocksExceptionIfStartLineNumberIsLessThan1_Data()
        {
            return new object[][]
            {
                new object[]{0},
                new object[]{-1}
            };
        }

        [Theory]
        [MemberData(nameof(Constructor_ThrowsFlexiBlocksExceptionIfEndLineNumberIsNotMinus1AndIsLessThanStartLineNumber_Data))]
        public void Constructor_ThrowsFlexiBlocksExceptionIfEndLineNumberIsNotMinus1AndIsLessThanStartLineNumber(int dummyStartLineNumber, int dummyEndLineNumber)
        {
            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => new Clipping(dummyStartLineNumber, dummyEndLineNumber));
            Assert.Equal(string.Format(Strings.FlexiBlocksException_Shared_EndLineNumberMustBeMinus1OrGreaterThanOrEqualToStartLineNumber, nameof(Clipping.EndLineNumber), dummyEndLineNumber, dummyStartLineNumber),
                result.Message,
                ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Constructor_ThrowsFlexiBlocksExceptionIfEndLineNumberIsNotMinus1AndIsLessThanStartLineNumber_Data()
        {
            return new object[][]
            {
                new object[]{1, 0},
                new object[]{2, 1},
                new object[]{2, -2},
            };
        }

        [Fact]
        public void Constructor_ThrowsFlexiBlocksExceptionIfDedentLengthIsLessThan0()
        {
            // Arrange
            const int dummyDedentLength = -1;

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => new Clipping(dedentLength: dummyDedentLength));
            Assert.Equal(string.Format(Strings.FlexiBlocksException_Shared_OptionMustBeGreaterThan0, nameof(Clipping.DedentLength), dummyDedentLength),
                result.Message,
                ignoreLineEndingDifferences: true);
        }

        [Theory]
        [MemberData(nameof(Constructor_ThrowsFlexiBlocksExceptionIfCollapseRatioIsNotInTheExpectedRange_Data))]
        public void Constructor_ThrowsFlexiBlocksExceptionIfCollapseRatioIsNotInTheExpectedRange(float dummyCollapseRatio)
        {
            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => new Clipping(collapseRatio: dummyCollapseRatio));
            Assert.Equal(string.Format(Strings.FlexiBlocksException_Clipping_OptionMustBeWithinRange, nameof(Clipping.CollapseRatio), "[0, 1]", dummyCollapseRatio),
                result.Message,
                ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Constructor_ThrowsFlexiBlocksExceptionIfCollapseRatioIsNotInTheExpectedRange_Data()
        {
            return new object[][]
            {
                new object[]{-1},
                new object[]{-0.1},
                new object[]{2},
                new object[]{1.1},
            };
        }

        [Theory]
        [MemberData(nameof(Constructor_CreatesClippingInstanceIfStartAndEndLineNumbersAreValid_Data))]
        public void Constructor_CreatesClippingInstanceIfStartAndEndLineNumbersAreValid(int dummyStartLineNumber, int dummyEndLineNumber)
        {
            // Act
            var result = new Clipping(dummyStartLineNumber, dummyEndLineNumber);

            // Assert
            Assert.Equal(dummyStartLineNumber, result.StartLineNumber);
            Assert.Equal(dummyEndLineNumber, result.EndLineNumber);
        }

        public static IEnumerable<object[]> Constructor_CreatesClippingInstanceIfStartAndEndLineNumbersAreValid_Data()
        {
            return new object[][]
            {
                new object[]{ 1, -1 }, // All lines
                new object[]{ 2, 2 }, // Single line
                new object[]{ 3, 4 } // Range of lines
            };
        }
    }
}
