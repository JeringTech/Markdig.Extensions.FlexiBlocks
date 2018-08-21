using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using System;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiIncludeBlocks
{
    public class ClippingAreaUnitTests
    {
        [Theory]
        [MemberData(nameof(Constructor_ThrowsArgumentExceptionIfBothOrNeitherOfStartLineSubstringAndNumberAreDefined_Data))]
        public void Constructor_ThrowsArgumentExceptionIfBothOrNeitherOfStartLineSubstringAndNumberAreDefined(int dummyStartLineNumber, string  dummyStartLineSubstring)
        {
            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => new ClippingArea(dummyStartLineNumber, -1, dummyStartLineSubstring));
            Assert.Equal(string.Format(Strings.ArgumentException_OneAndOnlyOneArgumentMustBeDefined, "startDemarcationLineSubstring", "startLineNumber"), result.Message);
        }

        public static IEnumerable<object[]> Constructor_ThrowsArgumentExceptionIfBothOrNeitherOfStartLineSubstringAndNumberAreDefined_Data()
        {
            return new object[][]
            {
                new object[]{0 , null},
                new object[]{0 , ""},
                new object[]{0 , " "},
                new object[]{1, "dummyStartLineSubstring"} // start line number is defined if it is >= 1
            };
        }

        [Theory]
        [MemberData(nameof(Constructor_ThrowsArgumentExceptionIfBothOrNeitherOfEndDemarcationLineSubstringAndNumberAreDefined_Data))]
        public void Constructor_ThrowsArgumentExceptionIfBothOrNeitherOfEndDemarcationLineSubstringAndNumberAreDefined(int dummyEndLineNumber, string dummyEndDemarcationLineSubstring)
        {
            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => new ClippingArea(1, dummyEndLineNumber, endDemarcationLineSubstring: dummyEndDemarcationLineSubstring));
            Assert.Equal(string.Format(Strings.ArgumentException_OneAndOnlyOneArgumentMustBeDefined, "endDemarcationLineSubstring", "endLineNumber"), result.Message);
        }

        public static IEnumerable<object[]> Constructor_ThrowsArgumentExceptionIfBothOrNeitherOfEndDemarcationLineSubstringAndNumberAreDefined_Data()
        {
            return new object[][]
            {
                new object[]{0 , null},
                new object[]{0 , ""},
                new object[]{0 , " "},
                new object[]{1, "dummyEndDemarcationLineSubstring"}, // end line number is defined if it is != 0
                new object[]{-1, "dummyEndDemarcationLineSubstring"}
            };
        }

        [Fact]
        public void Constructor_ThrowsArgumentExceptionIfBothStartAndEndLineNumberAreDefinedEndLineNumberIsNotMinus1ButIsLessThanStartLineNumber()
        {
            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => new ClippingArea(2, 1));
            Assert.Equal(Strings.ArgumentException_EndLineNumberMustNotBeLessThanStartLineNumber, result.Message);
        }

        [Fact]
        public void Constructor_ThrowsArgumentExceptionIfDedentLengthIsLessThan0()
        {
            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => new ClippingArea(1, -1, dedentLength: -1));
            Assert.Equal(string.Format(Strings.ArgumentException_ArgumentMustNotBeNegative, "dedentLength"), result.Message);
        }

        [Fact]
        public void Constructor_ThrowsArgumentExceptionIfCollapseRatioIsLessThan1()
        {
            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => new ClippingArea(1, -1, collapseRatio: 0));
            Assert.Equal(string.Format(Strings.ArgumentException_ArgumentMustBeLargerThan0, "collapseRatio"), result.Message);
        }
    }
}
