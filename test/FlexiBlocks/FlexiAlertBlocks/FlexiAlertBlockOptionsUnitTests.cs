using Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiAlertBlocks
{
    public class FlexiAlertBlockOptionsUnitTests
    {
        [Theory]
        [MemberData(nameof(FlexiAlertBlockOptions_CanBePopulated_Data))]
        public void FlexiAlertBlockOptions_CanBePopulated(SerializableWrapper<FlexiAlertBlockOptions> dummyInitialOptionsWrapper,
            SerializableWrapper<FlexiAlertBlockOptions> dummyExpectedOptionsWrapper,
            string dummyJson)
        {
            // Act
            JsonConvert.PopulateObject(dummyJson, dummyInitialOptionsWrapper.Value);

            // Assert
            FlexiAlertBlockOptions result = dummyInitialOptionsWrapper.Value;
            FlexiAlertBlockOptions expectedResult = dummyExpectedOptionsWrapper.Value;
            Assert.Equal(expectedResult.IconMarkup, result.IconMarkup);
            Assert.Equal(expectedResult.ClassFormat, result.ClassFormat);
            Assert.Equal(expectedResult.Class, result.Class);
            Assert.Equal(expectedResult.ContentClass, result.ContentClass);
            Assert.Equal(expectedResult.Type, result.Type);
            Assert.Equal(expectedResult.Attributes, result.Attributes);
        }

        public static IEnumerable<object[]> FlexiAlertBlockOptions_CanBePopulated_Data()
        {
            const string dummyIconMarkup = "dummyIconMarkup";
            const string dummyClassFormat = "dummy-{0}";
            const string dummyContentClass = "dummyContentClass";
            const string dummyType = "dummyType";
            const string dummyAttribute1 = "dummyAttribute1";
            const string dummyAttributeValue1 = "dummyAttributeValue1";
            var dummyAttributes1 = new Dictionary<string, string> { { dummyAttribute1, dummyAttributeValue1 } };
            const string dummyAttribute2 = "dummyAttribute2";
            const string dummyAttributeValue2 = "dummyAttributeValue2";
            var dummyAttributes2 = new Dictionary<string, string> { { dummyAttribute2, dummyAttributeValue2 } };

            return new object[][]
            {
                // Populating FlexiAlertBlockOptions containing default values
                new object[]
                {
                    new SerializableWrapper<FlexiAlertBlockOptions>(new FlexiAlertBlockOptions()),
                    new SerializableWrapper<FlexiAlertBlockOptions>(new FlexiAlertBlockOptions(dummyType,
                        dummyClassFormat,
                        dummyIconMarkup,
                        dummyContentClass,
                        dummyAttributes1)),
                    $@"{{
    ""{nameof(FlexiAlertBlockOptions.IconMarkup)}"": ""{dummyIconMarkup}"",
    ""{nameof(FlexiAlertBlockOptions.ClassFormat)}"": ""{dummyClassFormat}"",
    ""{nameof(FlexiAlertBlockOptions.ContentClass)}"": ""{dummyContentClass}"",
    ""{nameof(FlexiAlertBlockOptions.Type)}"": ""{dummyType}"",
    ""{nameof(FlexiAlertBlockOptions.Attributes)}"": {{
        ""{dummyAttribute1}"": ""{dummyAttributeValue1}""
    }}
}}"
                },

                // Populating FlexiAlertBlockOptions with an existing attributes collection (should be replaced instead of appended to)
                new object[]
                {
                    new SerializableWrapper<FlexiAlertBlockOptions>(new FlexiAlertBlockOptions(attributes: dummyAttributes1)),
                    new SerializableWrapper<FlexiAlertBlockOptions>(new FlexiAlertBlockOptions(attributes: dummyAttributes2)),
                    $@"{{
    ""{nameof(FlexiAlertBlockOptions.Attributes)}"": {{
        ""{dummyAttribute2}"": ""{dummyAttributeValue2}""
    }}
}}"
                }
            };
        }

        [Fact]
        public void ValidateAndPopulate_ThrowsFlexiBlocksExceptionIfClassFormatIsAnInvalidFormat()
        {
            // Arrange
            const string dummyClassFormat = "dummy-{0}-{1}";

            // Act and assert
            FlexiBlocksException result = Assert.
                Throws<FlexiBlocksException>(() => new FlexiAlertBlockOptions(classFormat: dummyClassFormat, type: "dummyAlertType"));
            Assert.Equal(string.Format(Strings.FlexiBlocksException_Shared_OptionIsAnInvalidFormat,
                    nameof(FlexiAlertBlockOptions.ClassFormat),
                    dummyClassFormat),
                result.Message);
            Assert.IsType<FormatException>(result.InnerException);
        }

        [Fact]
        public void ValidateAndPopulate_PopulatesClassifClassFormatAndAlertTypeAreDefined()
        {
            // Arrange
            const string dummyClassFormat = "dummy-{0}";
            const string dummyAlertType = "dummyAlertType";

            // Act
            var testSubject = new FlexiAlertBlockOptions(classFormat: dummyClassFormat, type: dummyAlertType);

            // Assert
            Assert.Equal(string.Format(dummyClassFormat, dummyAlertType), testSubject.Class);
        }

        [Fact]
        public void ValidateAndPopulate_SetsClassToNullIfClassFormatOrAlertTypeIsUndefined()
        {
            // Arrange
            var testSubject = new FlexiAlertBlockOptions();
            string initialClass = testSubject.Class;

            // Act
            JsonConvert.PopulateObject("{\"classFormat\": null}", testSubject);

            // Assert
            Assert.NotNull(initialClass);
            Assert.Null(testSubject.Class);
        }
    }
}
