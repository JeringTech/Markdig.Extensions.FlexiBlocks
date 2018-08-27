using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using System;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiIncludeBlocks
{
    public class ClippingUnitTests
    {
        [Theory]
        [MemberData(nameof(Constructor_ThrowsArgumentOutOfRangeExceptionIfStartLineNumberIsLessThan1_Data))]
        public void Constructor_ThrowsArgumentOutOfRangeExceptionIfStartLineNumberIsLessThan1(int dummyStartLineNumber)
        {
            // Act and assert
            ArgumentOutOfRangeException result = Assert.Throws<ArgumentOutOfRangeException>(() => new Clipping(dummyStartLineNumber));
            Assert.Equal(string.Format(Strings.ArgumentOutOfRangeException_LineNumberMustBeGreaterThan0, dummyStartLineNumber) + "\nParameter name: startLineNumber", 
                result.Message,
                ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Constructor_ThrowsArgumentOutOfRangeExceptionIfStartLineNumberIsLessThan1_Data()
        {
            return new object[][]
            {
                new object[]{0},
                new object[]{-1}
            };
        }

        [Theory]
        [MemberData(nameof(Constructor_ThrowsArgumentOutOfRangeExceptionIfEndLineNumberIsNotMinus1AndIsLessThanStartLineNumber_Data))]
        public void Constructor_ThrowsArgumentOutOfRangeExceptionIfEndLineNumberIsNotMinus1AndIsLessThanStartLineNumber(int dummyStartLineNumber, int dummyEndLineNumber)
        {
            // Act and assert
            ArgumentOutOfRangeException result = Assert.Throws<ArgumentOutOfRangeException>(() => new Clipping(dummyStartLineNumber, dummyEndLineNumber));
            Assert.Equal(string.Format(Strings.ArgumentOutOfRangeException_EndLineNumberMustBeMinus1OrGreaterThanOrEqualToStartLineNumber, dummyEndLineNumber, dummyStartLineNumber) + "\nParameter name: endLineNumber",
                result.Message,
                ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Constructor_ThrowsArgumentOutOfRangeExceptionIfEndLineNumberIsNotMinus1AndIsLessThanStartLineNumber_Data()
        {
            return new object[][]
            {
                new object[]{1, 0},
                new object[]{2, 1},
                new object[]{2, -2},
            };
        }

        [Fact]
        public void Constructor_ThrowsArgumentOutOfRangeExceptionIfDedentLengthIsLessThan0()
        {
            // Arrange
            const int dummyDedentLength = -1;

            // Act and assert
            ArgumentOutOfRangeException result = Assert.Throws<ArgumentOutOfRangeException>(() => new Clipping(dedentLength: dummyDedentLength));
            Assert.Equal(string.Format(Strings.ArgumentOutOfRangeException_ValueCannotBeNegative, dummyDedentLength) + "\nParameter name: dedentLength", 
                result.Message,
                ignoreLineEndingDifferences: true);
        }

        [Theory]
        [MemberData(nameof(Constructor_ThrowsArgumentOutOfRangeExceptionIfCollapseRatioIsNotInTheExpectedRange_Data))]
        public void Constructor_ThrowsArgumentOutOfRangeExceptionIfCollapseRatioIsNotInTheExpectedRange(float dummyCollapseRatio)
        {
            // Act and assert
            ArgumentOutOfRangeException result = Assert.Throws<ArgumentOutOfRangeException>(() => new Clipping(collapseRatio: dummyCollapseRatio));
            Assert.Equal(string.Format(Strings.ArgumentOutOfRangeException_ValueMustBeWithinRange, "[0, 1]", dummyCollapseRatio) + "\nParameter name: collapseRatio", 
                result.Message,
                ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Constructor_ThrowsArgumentOutOfRangeExceptionIfCollapseRatioIsNotInTheExpectedRange_Data()
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
