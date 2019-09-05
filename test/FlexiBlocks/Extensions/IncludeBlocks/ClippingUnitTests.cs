using Jering.Markdig.Extensions.FlexiBlocks.IncludeBlocks;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.IncludeBlocks
{
    public class ClippingUnitTests
    {
        [Fact]
        public void Constructor_ThrowsOptionsExceptionIfDedentIsNegative()
        {
            // Arrange
            const int dummyDedent = -1;

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => new Clipping(dedent: dummyDedent));
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                    nameof(Clipping.Dedent),
                    string.Format(Strings.OptionsException_Shared_ValueMustNotBeNegative, dummyDedent)),
                result.Message,
                ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void Constructor_ThrowsOptionsExceptionIfIndentIsNegative()
        {
            // Arrange
            const int dummyIndent = -1;

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => new Clipping(indent: dummyIndent));
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                    nameof(Clipping.Indent),
                    string.Format(Strings.OptionsException_Shared_ValueMustNotBeNegative, dummyIndent)),
                result.Message,
                ignoreLineEndingDifferences: true);
        }

        [Theory]
        [MemberData(nameof(Constructor_ThrowsOptionsExceptionIfCollapseIsNotInTheExpectedRange_Data))]
        public void Constructor_ThrowsOptionsExceptionIfCollapseIsNotInTheExpectedRange(float dummyCollapse)
        {
            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => new Clipping(collapse: dummyCollapse));
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                    nameof(Clipping.Collapse),
                    string.Format(Strings.OptionsException_Shared_ValueMustBeWithinRange, dummyCollapse, "[0, 1]")),
                result.Message,
                ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Constructor_ThrowsOptionsExceptionIfCollapseIsNotInTheExpectedRange_Data()
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
        [MemberData(nameof(Equals_ReturnsTrueIfObjIsAnIdenticalClippingOtherwiseReturnsFalse_Data))]
        public void Equals_ReturnsTrueIfObjIsAnIdenticalClippingOtherwiseReturnsFalse(Clipping dummyClipping,
            object dummyObj,
            bool expectedResult)
        {
            // Act
            bool result = dummyClipping.Equals(dummyObj);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> Equals_ReturnsTrueIfObjIsAnIdenticalClippingOtherwiseReturnsFalse_Data()
        {
            // Arbitrary values
            const int dummyStartLine = 4;
            const int dummyEndLine = 25;
            const string dummyRegion = "dummyRegion";
            const string dummyStartString = "dummyStartString";
            const string dummyEndString = "dummyEndString";
            const int dummyDedent = 3;
            const int dummyIndent = 5;
            const float dummyCollapse = 0.5F;
            const string dummyBefore = "dummyBefore";
            const string dummyAfter = "dummyAfter";

            return new object[][]
            {
                new object[]{new Clipping(),
                    "not a line range",
                    false },
                new object[]{
                    new Clipping(),
                    new Clipping(),
                    true
                },
                // False if the Clippings differ in any way
                new object[]{
                    new Clipping(),
                    new Clipping(dummyStartLine),
                    false
                },
                new object[]{
                    new Clipping(),
                    new Clipping(endLine: dummyEndLine),
                    false
                },
                new object[]{
                    new Clipping(),
                    new Clipping(region: dummyRegion),
                    false
                },
                new object[]{
                    new Clipping(),
                    new Clipping(startString: dummyStartString),
                    false
                },
                new object[]{
                    new Clipping(),
                    new Clipping(endString: dummyEndString),
                    false
                },
                new object[]{
                    new Clipping(),
                    new Clipping(dedent: dummyDedent),
                    false
                },
                new object[]{
                    new Clipping(),
                    new Clipping(indent: dummyIndent),
                    false
                },
                new object[]{
                    new Clipping(),
                    new Clipping(collapse: dummyCollapse),
                    false
                },
                new object[]{
                    new Clipping(),
                    new Clipping(before: dummyBefore),
                    false
                },
                new object[]{
                    new Clipping(),
                    new Clipping(after: dummyAfter),
                    false
                }
            };
        }

        [Theory]
        [MemberData(nameof(GetHashCode_ReturnsSameHashCodeForIdenticalClippings_Data))]
        public void GetHashCode_ReturnsSameHashCodeForIdenticalClippings(Clipping dummyClipping1,
            Clipping dummyClipping2,
            bool identical)
        {
            // Act and assert
            Assert.Equal(identical, dummyClipping1.GetHashCode() == dummyClipping2.GetHashCode());
        }

        public static IEnumerable<object[]> GetHashCode_ReturnsSameHashCodeForIdenticalClippings_Data()
        {
            // Arbitrary values
            const int dummyStartLine = 4;
            const int dummyEndLine = 25;
            const string dummyRegion = "dummyRegion";
            const string dummyStartString = "dummyStartString";
            const string dummyEndString = "dummyEndString";
            const int dummyDedent = 3;
            const int dummyIndent = 5;
            const float dummyCollapse = 0.5F;
            const string dummyBefore = "dummyBefore";
            const string dummyAfter = "dummyAfter";

            return new object[][]
            {
                new object[]{
                    new Clipping(),
                    new Clipping(),
                    true
                },
                // False if the Clippings differ in any way
                new object[]{
                    new Clipping(),
                    new Clipping(dummyStartLine),
                    false
                },
                new object[]{
                    new Clipping(),
                    new Clipping(endLine: dummyEndLine),
                    false
                },
                new object[]{
                    new Clipping(),
                    new Clipping(region: dummyRegion),
                    false
                },
                new object[]{
                    new Clipping(),
                    new Clipping(startString: dummyStartString),
                    false
                },
                new object[]{
                    new Clipping(),
                    new Clipping(endString: dummyEndString),
                    false
                },
                new object[]{
                    new Clipping(),
                    new Clipping(dedent: dummyDedent),
                    false
                },
                new object[]{
                    new Clipping(),
                    new Clipping(indent: dummyIndent),
                    false
                },
                new object[]{
                    new Clipping(),
                    new Clipping(collapse: dummyCollapse),
                    false
                },
                new object[]{
                    new Clipping(),
                    new Clipping(before: dummyBefore),
                    false
                },
                new object[]{
                    new Clipping(),
                    new Clipping(after: dummyAfter),
                    false
                }
            };
        }
    }
}
