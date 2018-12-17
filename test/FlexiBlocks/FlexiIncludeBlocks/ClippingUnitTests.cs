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

        [Theory]
        [MemberData(nameof(Equals_ReturnsTrueIfObjIsAnIdenticalClippingOtherwiseReturnsFalse_Data))]
        public void Equals_ReturnsTrueIfObjIsAnIdenticalClippingOtherwiseReturnsFalse(SerializableWrapper<Clipping> dummyClippingWrapper,
            SerializableWrapper<object> dummyObjWrapper,
            bool expectedResult)
        {
            // Act
            bool result = dummyClippingWrapper.Value.Equals(dummyObjWrapper.Value);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> Equals_ReturnsTrueIfObjIsAnIdenticalClippingOtherwiseReturnsFalse_Data()
        {
            // Arbitrary values
            const int dummyStartLineNumber = 4;
            const int dummyEndLineNumber = 25;
            const string dummyStartDemarcationLineSubstring = "dummyStartDemarcationLineSubstring";
            const string dummyEndDemarcationLineSubstring = "dummyEndDemarcationLineSubstring";
            const int dummyDedentLength = 3;
            const float dummyCollapseRatio = 0.5F;
            const string dummyBeforeContent = "dummyBeforeContent";
            const string dummyAfterContent = "dummyAfterContent";

            return new object[][]
            {
                new object[]{new SerializableWrapper<Clipping>(new Clipping()),
                    new SerializableWrapper<object>("not a line range"),
                    false },
                new object[]{
                    new SerializableWrapper<Clipping>(new Clipping()),
                    new SerializableWrapper<object>(new Clipping()),
                    true
                },
                // False if the Clippings differ in any way
                new object[]{
                    new SerializableWrapper<Clipping>(new Clipping()),
                    new SerializableWrapper<object>(new Clipping(dummyStartLineNumber)),
                    false
                },
                new object[]{
                    new SerializableWrapper<Clipping>(new Clipping()),
                    new SerializableWrapper<object>(new Clipping(endLineNumber: dummyEndLineNumber)),
                    false
                },
                new object[]{
                    new SerializableWrapper<Clipping>(new Clipping()),
                    new SerializableWrapper<object>(new Clipping(startDemarcationLineSubstring: dummyStartDemarcationLineSubstring)),
                    false
                },
                new object[]{
                    new SerializableWrapper<Clipping>(new Clipping()),
                    new SerializableWrapper<object>(new Clipping(endDemarcationLineSubstring: dummyEndDemarcationLineSubstring)),
                    false
                },
                new object[]{
                    new SerializableWrapper<Clipping>(new Clipping()),
                    new SerializableWrapper<object>(new Clipping(dedentLength: dummyDedentLength)),
                    false
                },
                new object[]{
                    new SerializableWrapper<Clipping>(new Clipping()),
                    new SerializableWrapper<object>(new Clipping(collapseRatio: dummyCollapseRatio)),
                    false
                },
                new object[]{
                    new SerializableWrapper<Clipping>(new Clipping()),
                    new SerializableWrapper<object>(new Clipping(beforeContent: dummyBeforeContent)),
                    false
                },
                new object[]{
                    new SerializableWrapper<Clipping>(new Clipping()),
                    new SerializableWrapper<object>(new Clipping(afterContent: dummyAfterContent)),
                    false
                },
            };
        }

        [Theory]
        [MemberData(nameof(GetHashCode_ReturnsSameHashCodeForIdenticalClippings_Data))]
        public void GetHashCode_ReturnsSameHashCodeForIdenticalClippings(SerializableWrapper<Clipping> dummyLine1RangeWrapper,
            SerializableWrapper<Clipping> dummyLine2RangeWrapper,
            bool identical)
        {
            // Act and assert
            Assert.Equal(identical, dummyLine1RangeWrapper.Value.GetHashCode() == dummyLine2RangeWrapper.Value.GetHashCode());
        }

        public static IEnumerable<object[]> GetHashCode_ReturnsSameHashCodeForIdenticalClippings_Data()
        {
            // Arbitrary values
            const int dummyStartLineNumber = 4;
            const int dummyEndLineNumber = 25;
            const string dummyStartDemarcationLineSubstring = "dummyStartDemarcationLineSubstring";
            const string dummyEndDemarcationLineSubstring = "dummyEndDemarcationLineSubstring";
            const int dummyDedentLength = 3;
            const float dummyCollapseRatio = 0.5F;
            const string dummyBeforeContent = "dummyBeforeContent";
            const string dummyAfterContent = "dummyAfterContent";

            return new object[][]
            {
                new object[]{
                    new SerializableWrapper<Clipping>(new Clipping()),
                    new SerializableWrapper<Clipping>(new Clipping()),
                    true
                },
                // False if the Clippings differ in any way
                new object[]{
                    new SerializableWrapper<Clipping>(new Clipping()),
                    new SerializableWrapper<Clipping>(new Clipping(dummyStartLineNumber)),
                    false
                },
                new object[]{
                    new SerializableWrapper<Clipping>(new Clipping()),
                    new SerializableWrapper<Clipping>(new Clipping(endLineNumber: dummyEndLineNumber)),
                    false
                },
                new object[]{
                    new SerializableWrapper<Clipping>(new Clipping()),
                    new SerializableWrapper<Clipping>(new Clipping(startDemarcationLineSubstring: dummyStartDemarcationLineSubstring)),
                    false
                },
                new object[]{
                    new SerializableWrapper<Clipping>(new Clipping()),
                    new SerializableWrapper<Clipping>(new Clipping(endDemarcationLineSubstring: dummyEndDemarcationLineSubstring)),
                    false
                },
                new object[]{
                    new SerializableWrapper<Clipping>(new Clipping()),
                    new SerializableWrapper<Clipping>(new Clipping(dedentLength: dummyDedentLength)),
                    false
                },
                new object[]{
                    new SerializableWrapper<Clipping>(new Clipping()),
                    new SerializableWrapper<Clipping>(new Clipping(collapseRatio: dummyCollapseRatio)),
                    false
                },
                new object[]{
                    new SerializableWrapper<Clipping>(new Clipping()),
                    new SerializableWrapper<Clipping>(new Clipping(beforeContent: dummyBeforeContent)),
                    false
                },
                new object[]{
                    new SerializableWrapper<Clipping>(new Clipping()),
                    new SerializableWrapper<Clipping>(new Clipping(afterContent: dummyAfterContent)),
                    false
                },
            };
        }
    }
}
