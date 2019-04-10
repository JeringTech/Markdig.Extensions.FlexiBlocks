using Jering.Markdig.Extensions.FlexiBlocks.Alerts;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.Alerts
{
    public class FlexiAlertOptionsUnitTests
    {
        [Theory]
        [MemberData(nameof(FlexiAlertOptions_CanBePopulated_Data))]
        public void FlexiAlertOptions_CanBePopulated(SerializableWrapper<FlexiAlertOptions> dummyInitialOptionsWrapper,
            SerializableWrapper<FlexiAlertOptions> dummyExpectedOptionsWrapper,
            string dummyJson)
        {
            // Act
            JsonConvert.PopulateObject(dummyJson, dummyInitialOptionsWrapper.Value);

            // Assert
            FlexiAlertOptions result = dummyInitialOptionsWrapper.Value;
            FlexiAlertOptions expectedResult = dummyExpectedOptionsWrapper.Value;
            Assert.Equal(expectedResult.BlockName, result.BlockName);
            Assert.Equal(expectedResult.Type, result.Type);
            Assert.Equal(expectedResult.IconHtmlFragment, result.IconHtmlFragment);
            Assert.Equal(expectedResult.Attributes, result.Attributes);
        }

        public static IEnumerable<object[]> FlexiAlertOptions_CanBePopulated_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string dummyType = "dummyType";
            const string dummyIconHtmlFragment = "dummyIconHtmlFragment";
            const string dummyAttribute1 = "dummyAttribute1";
            const string dummyAttributeValue1 = "dummyAttributeValue1";
            var dummyAttributes1 = new Dictionary<string, string> { { dummyAttribute1, dummyAttributeValue1 } };
            const string dummyAttribute2 = "dummyAttribute2";
            const string dummyAttributeValue2 = "dummyAttributeValue2";
            var dummyAttributes2 = new Dictionary<string, string> { { dummyAttribute2, dummyAttributeValue2 } };

            return new object[][]
            {
                // Each new value should overwrite its corresponding existing value
                new object[]
                {
                    new SerializableWrapper<FlexiAlertOptions>(new FlexiAlertOptions()),
                    new SerializableWrapper<FlexiAlertOptions>(new FlexiAlertOptions(dummyBlockName,
                        dummyType,
                        dummyIconHtmlFragment,
                        dummyAttributes1)),
                    $@"{{
    ""{nameof(FlexiAlertOptions.BlockName)}"": ""{dummyBlockName}"",
    ""{nameof(FlexiAlertOptions.Type)}"": ""{dummyType}"",
    ""{nameof(FlexiAlertOptions.IconHtmlFragment)}"": ""{dummyIconHtmlFragment}"",
    ""{nameof(FlexiAlertOptions.Attributes)}"": {{
        ""{dummyAttribute1}"": ""{dummyAttributeValue1}""
    }}
}}"
                },

                // Existing values should not be overwritten if no corresponding new value
                new object[]
                {
                    new SerializableWrapper<FlexiAlertOptions>(new FlexiAlertOptions(dummyBlockName)),
                    new SerializableWrapper<FlexiAlertOptions>(new FlexiAlertOptions(dummyBlockName, dummyType)), // Block name should stay the same
                    $@"{{ ""{nameof(FlexiAlertOptions.Type)}"": ""{dummyType}"" }}"
                },

                // Populating FlexiAlertOptions with an existing attributes collection should replace the entire collection
                new object[]
                {
                    new SerializableWrapper<FlexiAlertOptions>(new FlexiAlertOptions(attributes: dummyAttributes1)),
                    new SerializableWrapper<FlexiAlertOptions>(new FlexiAlertOptions(attributes: dummyAttributes2)),
                    $@"{{
    ""{nameof(FlexiAlertOptions.Attributes)}"": {{
        ""{dummyAttribute2}"": ""{dummyAttributeValue2}""
    }}
}}"
                }
            };
        }
    }
}
