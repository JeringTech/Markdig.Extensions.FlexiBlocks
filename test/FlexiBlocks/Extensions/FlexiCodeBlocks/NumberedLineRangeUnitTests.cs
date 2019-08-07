using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiCodeBlocks
{
    public class NumberedLineRangeUnitTests
    {
        [Theory]
        [MemberData(nameof(Constructor_ThrowsOptionsExceptionIfStartNumberIsInvalid_Data))]
        public void Constructor_ThrowsOptionsExceptionIfStartNumberIsInvalid(int startNumber)
        {
            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => new NumberedLineRange(1, 1, startNumber));
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                nameof(NumberedLineRange.StartNumber),
                string.Format(Strings.OptionsException_Shared_ValueMustBeIntegerGreaterThan0, startNumber)),
                result.Message);
        }

        public static IEnumerable<object[]> Constructor_ThrowsOptionsExceptionIfStartNumberIsInvalid_Data()
        {
            return new object[][]
            {
                new object[]{ 0 }, // Cannot be less than 1
                new object[]{ -1 } // Cannot be less than 1
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
            Assert.Equal("Start: 1, End: 5, StartNumber: 10", result);
        }

        [Theory]
        [MemberData(nameof(Equals_ReturnsTrueIfObjIsAnIdenticalNumberedLineRangeOtherwiseReturnsFalse_Data))]
        public void Equals_ReturnsTrueIfObjIsAnIdenticalNumberedLineRangeOtherwiseReturnsFalse(NumberedLineRange dummyNumberedLineRange,
            object dummyObj,
            bool expectedResult)
        {
            // Act
            bool result = dummyNumberedLineRange.Equals(dummyObj);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> Equals_ReturnsTrueIfObjIsAnIdenticalNumberedLineRangeOtherwiseReturnsFalse_Data()
        {
            const int dummyStart = 4; // Arbitrary
            const int dummyEnd = 25; // Arbitrary
            const int dummyStartNumber = 123; // Arbitrary

            return new object[][]
            {
                new object[]{new NumberedLineRange(),
                    "not a numbered line range",
                    false },
                new object[]{new NumberedLineRange(),
                    new NumberedLineRange(),
                    true },
                // False if the NumberedLineRanges differ in any way
                new object[]{new NumberedLineRange(),
                    new NumberedLineRange(dummyStart),
                    false },
                new object[]{new NumberedLineRange(),
                    new NumberedLineRange(end: dummyEnd),
                    false },
                new object[]{new NumberedLineRange(),
                    new NumberedLineRange(startNumber: dummyStartNumber),
                    false },
            };
        }

        [Theory]
        [MemberData(nameof(GetHashCode_ReturnsSameHashCodeForIdenticalNumberedLineRanges_Data))]
        public void GetHashCode_ReturnsSameHashCodeForIdenticalNumberedLineRanges(NumberedLineRange dummyNumberedLineRange1,
            NumberedLineRange dummyNumberedLineRange2,
            bool identical)
        {
            // Act and assert
            Assert.Equal(identical, dummyNumberedLineRange1.GetHashCode() == dummyNumberedLineRange2.GetHashCode());
        }

        public static IEnumerable<object[]> GetHashCode_ReturnsSameHashCodeForIdenticalNumberedLineRanges_Data()
        {
            const int dummyStart = 4; // Arbitrary
            const int dummyEnd = 25; // Arbitrary
            const int dummyStartNumber = 123; // Arbitrary

            return new object[][]
            {
                new object[]{new NumberedLineRange(),
                    new NumberedLineRange(),
                    true },
                new object[]{new NumberedLineRange(),
                    new NumberedLineRange(dummyStart),
                    false },
                new object[]{new NumberedLineRange(),
                    new NumberedLineRange(end: dummyEnd),
                    false },
                new object[]{new NumberedLineRange(),
                    new NumberedLineRange(startNumber: dummyStartNumber),
                    false }
            };
        }
    }
}
