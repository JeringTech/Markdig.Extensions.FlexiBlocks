using Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiSectionBlocks
{
    public class FlexiSectionBlockOptionsUnitTests
    {
        [Theory]
        [MemberData(nameof(FlexiSectionBlockOptions_CanBePopulated_Data))]
        public void FlexiSectionBlockOptions_CanBePopulated(SerializableWrapper<FlexiSectionBlockOptions> dummyInitialOptionsWrapper,
                   SerializableWrapper<FlexiSectionBlockOptions> dummyExpectedOptionsWrapper,
                   string dummyJson)
        {
            // Act
            JsonConvert.PopulateObject(dummyJson, dummyInitialOptionsWrapper.Value);

            // Assert
            FlexiSectionBlockOptions result = dummyInitialOptionsWrapper.Value;
            FlexiSectionBlockOptions expectedResult = dummyExpectedOptionsWrapper.Value;
            Assert.Equal(expectedResult.Element, result.Element);
            Assert.Equal(expectedResult.GenerateIdentifier, result.GenerateIdentifier);
            Assert.Equal(expectedResult.AutoLinkable, result.AutoLinkable);
            Assert.Equal(expectedResult.ClassFormat, result.ClassFormat);
            Assert.Equal(expectedResult.LinkIconMarkup, result.LinkIconMarkup);
            Assert.Equal(expectedResult.Attributes, result.Attributes);
        }

        public static IEnumerable<object[]> FlexiSectionBlockOptions_CanBePopulated_Data()
        {
            const SectioningContentElement dummyElement = SectioningContentElement.Aside;
            const bool dummyGenerateIdentifier = false;
            const bool dummyAutoLinkable = false;
            const string dummyClassFormat = "dummy-{0}";
            const string dummyLinkIconMarkdup = "dummyLinkIconMarkup";
            const string dummyAttribute1 = "dummyAttribute1";
            const string dummyAttributeValue1 = "dummyAttributeValue1";
            var dummyAttributes1 = new Dictionary<string, string> { { dummyAttribute1, dummyAttributeValue1 } };
            const string dummyAttribute2 = "dummyAttribute2";
            const string dummyAttributeValue2 = "dummyAttributeValue2";
            var dummyAttributes2 = new Dictionary<string, string> { { dummyAttribute2, dummyAttributeValue2 } };

            return new object[][]
            {
                // Populating FlexiSectionBlockOptions containing default values
                new object[]
                {
                    new SerializableWrapper<FlexiSectionBlockOptions>(new FlexiSectionBlockOptions()),
                    new SerializableWrapper<FlexiSectionBlockOptions>(new FlexiSectionBlockOptions(dummyElement,
                        dummyGenerateIdentifier,
                        dummyAutoLinkable,
                        dummyClassFormat,
                        dummyLinkIconMarkdup,
                        dummyAttributes1)),
                    $@"{{
    ""{nameof(FlexiSectionBlockOptions.Element)}"": ""{dummyElement}"",
    ""{nameof(FlexiSectionBlockOptions.GenerateIdentifier)}"": ""{dummyGenerateIdentifier}"",
    ""{nameof(FlexiSectionBlockOptions.AutoLinkable)}"": ""{dummyAutoLinkable}"",
    ""{nameof(FlexiSectionBlockOptions.ClassFormat)}"": ""{dummyClassFormat}"",
    ""{nameof(FlexiSectionBlockOptions.LinkIconMarkup)}"": ""{dummyLinkIconMarkdup}"",
    ""{nameof(FlexiSectionBlockOptions.Attributes)}"": {{
        ""{dummyAttribute1}"": ""{dummyAttributeValue1}""
    }}
}}"
                },

                // Populating FlexiSectionBlockOptions with an existing attributes collection (should be replaced instead of appended to)
                new object[]
                {
                    new SerializableWrapper<FlexiSectionBlockOptions>(new FlexiSectionBlockOptions(attributes: dummyAttributes1)),
                    new SerializableWrapper<FlexiSectionBlockOptions>(new FlexiSectionBlockOptions(attributes: dummyAttributes2)),
                    $@"{{
    ""{nameof(FlexiSectionBlockOptions.Attributes)}"": {{
        ""{dummyAttribute2}"": ""{dummyAttributeValue2}""
    }}
}}"
                }
            };
        }

        [Fact]
        public void ValidateAndPopulate_ThrowsFlexiBlocksExceptionIfSectioningContentElementIsNotWithinTheRangeOfValidValuesForTheEnumSectioningContentElement()
        {
            // Arrange
            const SectioningContentElement dummyElement = (SectioningContentElement)100; // Arbitrary int that is unlikely to ever be used in the enum

            // Act and assert
            FlexiBlocksException result = Assert.
                Throws<FlexiBlocksException>(() => new FlexiSectionBlockOptions(element: dummyElement));
            Assert.Equal(string.Format(Strings.FlexiBlocksException_Shared_OptionMustBeAValidEnumValue,
                    dummyElement,
                    nameof(FlexiSectionBlockOptions.Element),
                    nameof(SectioningContentElement)),
                result.Message);
        }
    }
}
