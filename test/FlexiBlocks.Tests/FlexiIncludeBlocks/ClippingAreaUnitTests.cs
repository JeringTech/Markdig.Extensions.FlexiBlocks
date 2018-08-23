using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using System;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiIncludeBlocks
{
    public class ClippingAreaUnitTests
    {
        // TODO defaults 

        [Theory]
        [MemberData(nameof(Constructor_ThrowsArgumentExceptionIfStartLineNumberIsLessThan1_Data))]
        public void Constructor_ThrowsArgumentExceptionIfStartLineNumberIsLessThan1(int dummyStartLineNumber)
        {
            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => new Clipping(dummyStartLineNumber));
            Assert.Equal(string.Format(Strings.ArgumentException_ArgumentMustBeLargerThan0, "startLineNumber"), result.Message);
        }

        public static IEnumerable<object[]> Constructor_ThrowsArgumentExceptionIfStartLineNumberIsLessThan1_Data()
        {
            return new object[][]
            {
                new object[]{0},
                new object[]{-1}
            };
        }

        [Theory]
        [MemberData(nameof(Constructor_ThrowsArgumentExceptionIfEndLineNumberIsNotMinus1ButIsLessThanStartLineNumber_Data))]
        public void Constructor_ThrowsArgumentExceptionIfEndLineNumberIsNotMinus1ButIsLessThanStartLineNumber(int dummyStartLineNumber, int dummyEndLineNumber)
        {
            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => new Clipping(dummyStartLineNumber, dummyEndLineNumber));
            Assert.Equal(Strings.ArgumentException_EndLineNumberMustNotBeLessThanStartLineNumber, result.Message);
        }

        public static IEnumerable<object[]> Constructor_ThrowsArgumentExceptionIfEndLineNumberIsNotMinus1ButIsLessThanStartLineNumber_Data()
        {
            return new object[][]
            {
                new object[]{1, 0},
                new object[]{2, 1},
                new object[]{2, -2},
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

        [Fact]
        public void Constructor_ThrowsArgumentExceptionIfDedentLengthIsLessThan0()
        {
            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => new Clipping(dedentLength: -1));
            Assert.Equal(string.Format(Strings.ArgumentException_ArgumentMustNotBeNegative, "dedentLength"), result.Message);
        }

        [Theory]
        [MemberData(nameof(Constructor_ThrowsArgumentExceptionIfCollapseRatioIsNotInTheExpectedInterval_Data))]
        public void Constructor_ThrowsArgumentExceptionIfCollapseRatioIsNotInTheExpectedInterval(float dummyCollapseRatio)
        {
            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => new Clipping(collapseRatio: dummyCollapseRatio));
            Assert.Equal(string.Format(Strings.ArgumentException_ArgumentMustBeInInterval, "collapseRatio", "[0, 1]"), result.Message);
        }

        public static IEnumerable<object[]> Constructor_ThrowsArgumentExceptionIfCollapseRatioIsNotInTheExpectedInterval_Data()
        {
            return new object[][]
            {
                new object[]{-1},
                new object[]{-0.1},
                new object[]{2},
                new object[]{1.1},
            };
        }
    }
}
