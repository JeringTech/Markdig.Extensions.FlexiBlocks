using Jering.Markdig.Extensions.FlexiBlocks.FlexiTabsBlocks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiTabsBlocks
{
    public class FlexiTabBlockOptionsUnitTests
    {
        [Theory]
        [MemberData(nameof(FlexiTabBlockOptions_CanBePopulated_Data))]
        public void FlexiTabBlockOptions_CanBePopulated(FlexiTabBlockOptions dummyInitialOptions,
            FlexiTabBlockOptions dummyExpectedOptions,
            string dummyJson)
        {
            // Act
            JsonConvert.PopulateObject(dummyJson, dummyInitialOptions);

            // Assert
            FlexiTabBlockOptions result = dummyInitialOptions;
            FlexiTabBlockOptions expectedResult = dummyExpectedOptions;
            Assert.Equal(expectedResult.Attributes, result.Attributes);
        }

        public static IEnumerable<object[]> FlexiTabBlockOptions_CanBePopulated_Data()
        {
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
                    new FlexiTabBlockOptions(),
                    new FlexiTabBlockOptions(dummyAttributes1),
                    $@"{{
    ""{nameof(FlexiTabBlockOptions.Attributes)}"": {{
        ""{dummyAttribute1}"": ""{dummyAttributeValue1}""
    }}
}}"
                },

                // Populating FlexiTabBlockOptions with an existing attributes collection should replace the entire collection
                new object[]
                {
                    new FlexiTabBlockOptions(dummyAttributes1),
                    new FlexiTabBlockOptions(dummyAttributes2),
                    $@"{{
    ""{nameof(FlexiTabBlockOptions.Attributes)}"": {{
        ""{dummyAttribute2}"": ""{dummyAttributeValue2}""
    }}
}}"
                }
            };
        }
    }
}
