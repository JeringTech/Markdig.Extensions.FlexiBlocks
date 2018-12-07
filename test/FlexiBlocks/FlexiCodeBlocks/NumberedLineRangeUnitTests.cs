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
            Assert.Equal(string.Format(Strings.FlexiBlocksException_Shared_OptionMustBeGreaterThan0, nameof(NumberedLineRange.FirstNumber), firstNumber),
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

        [Theory]
        [MemberData(nameof(Equals_ReturnsTrueIfObjIsAnIdenticalNumberedLineRangeOtherwiseReturnsFalse_Data))]
        public void Equals_ReturnsTrueIfObjIsAnIdenticalNumberedLineRangeOtherwiseReturnsFalse(SerializableWrapper<NumberedLineRange> dummyNumberedLineRangeWrapper,
            SerializableWrapper<object> dummyObjWrapper,
            bool expectedResult)
        {
            // Act
            bool result = dummyNumberedLineRangeWrapper.Value.Equals(dummyObjWrapper.Value);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> Equals_ReturnsTrueIfObjIsAnIdenticalNumberedLineRangeOtherwiseReturnsFalse_Data()
        {
            const int dummyStartLineNumber = 4; // Arbitrary
            const int dummyEndLineNumber = 25; // Arbitrary
            const int dummyFirstNumber = 123; // Arbitrary

            return new object[][]
            {
                new object[]{new SerializableWrapper<NumberedLineRange>(new NumberedLineRange()),
                    new SerializableWrapper<object>("not a line number range"),
                    false },
                new object[]{new SerializableWrapper<NumberedLineRange>(new NumberedLineRange()),
                    new SerializableWrapper<object>(new NumberedLineRange()),
                    true },
                // False if the NumberedLineRanges differ in any way
                new object[]{new SerializableWrapper<NumberedLineRange>(new NumberedLineRange()),
                    new SerializableWrapper<object>(new NumberedLineRange(dummyStartLineNumber)),
                    false },
                new object[]{new SerializableWrapper<NumberedLineRange>(new NumberedLineRange()),
                    new SerializableWrapper<object>(new NumberedLineRange(endLineNumber: dummyEndLineNumber)),
                    false },
                new object[]{new SerializableWrapper<NumberedLineRange>(new NumberedLineRange()),
                    new SerializableWrapper<object>(new NumberedLineRange(firstNumber: dummyFirstNumber)),
                    false },
            };
        }

        [Theory]
        [MemberData(nameof(GetHashCode_ReturnsSameHashCodeForIdenticalNumberedLineRanges_Data))]
        public void GetHashCode_ReturnsSameHashCodeForIdenticalNumberedLineRanges(SerializableWrapper<NumberedLineRange> dummyLine1RangeWrapper,
            SerializableWrapper<NumberedLineRange> dummyLine2RangeWrapper,
            bool identical)
        {
            // Act and assert
            Assert.Equal(identical, dummyLine1RangeWrapper.Value.GetHashCode() == dummyLine2RangeWrapper.Value.GetHashCode());
        }

        public static IEnumerable<object[]> GetHashCode_ReturnsSameHashCodeForIdenticalNumberedLineRanges_Data()
        {
            const int dummyStartLineNumber = 4; // Arbitrary
            const int dummyEndLineNumber = 25; // Arbitrary
            const int dummyFirstNumber = 123; // Arbitrary

            return new object[][]
            {
                new object[]{new SerializableWrapper<NumberedLineRange>(new NumberedLineRange()),
                    new SerializableWrapper<NumberedLineRange>(new NumberedLineRange()),
                    true },
                new object[]{new SerializableWrapper<NumberedLineRange>(new NumberedLineRange()),
                    new SerializableWrapper<NumberedLineRange>(new NumberedLineRange(dummyStartLineNumber)),
                    false },
                new object[]{new SerializableWrapper<NumberedLineRange>(new NumberedLineRange()),
                    new SerializableWrapper<NumberedLineRange>(new NumberedLineRange(endLineNumber: dummyEndLineNumber)),
                    false },
                new object[]{new SerializableWrapper<NumberedLineRange>(new NumberedLineRange()),
                    new SerializableWrapper<NumberedLineRange>(new NumberedLineRange(firstNumber: dummyFirstNumber)),
                    false }
            };
        }
    }
}
