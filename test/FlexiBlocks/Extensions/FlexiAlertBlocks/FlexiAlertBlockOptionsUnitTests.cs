using Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks;
using Newtonsoft.Json;
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
            Assert.Equal(expectedResult.BlockName, result.BlockName);
            Assert.Equal(expectedResult.Type, result.Type);
            Assert.Equal(expectedResult.Icon, result.Icon);
            Assert.Equal(expectedResult.Attributes, result.Attributes);
        }

        public static IEnumerable<object[]> FlexiAlertBlockOptions_CanBePopulated_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string dummyType = "dummyType";
            const string dummyIcon = "dummyIcon";
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
                    new SerializableWrapper<FlexiAlertBlockOptions>(new FlexiAlertBlockOptions()),
                    new SerializableWrapper<FlexiAlertBlockOptions>(new FlexiAlertBlockOptions(dummyBlockName,
                        dummyType,
                        dummyIcon,
                        dummyAttributes1)),
                    $@"{{
    ""{nameof(FlexiAlertBlockOptions.BlockName)}"": ""{dummyBlockName}"",
    ""{nameof(FlexiAlertBlockOptions.Type)}"": ""{dummyType}"",
    ""{nameof(FlexiAlertBlockOptions.Icon)}"": ""{dummyIcon}"",
    ""{nameof(FlexiAlertBlockOptions.Attributes)}"": {{
        ""{dummyAttribute1}"": ""{dummyAttributeValue1}""
    }}
}}"
                },

                // Existing values should not be overwritten if no corresponding new value
                new object[]
                {
                    new SerializableWrapper<FlexiAlertBlockOptions>(new FlexiAlertBlockOptions(dummyBlockName)),
                    new SerializableWrapper<FlexiAlertBlockOptions>(new FlexiAlertBlockOptions(dummyBlockName, dummyType)), // Block name should stay the same
                    $@"{{ ""{nameof(FlexiAlertBlockOptions.Type)}"": ""{dummyType}"" }}"
                },

                // Populating FlexiAlertBlockOptions with an existing attributes collection should replace the entire collection
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
    }
}
