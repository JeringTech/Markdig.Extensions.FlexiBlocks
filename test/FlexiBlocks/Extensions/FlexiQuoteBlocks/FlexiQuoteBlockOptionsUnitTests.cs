using Jering.Markdig.Extensions.FlexiBlocks.FlexiQuoteBlocks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiQuoteBlocks
{
    public class FlexiQuoteBlockOptionsUnitTests
    {
        [Theory]
        [MemberData(nameof(FlexiQuoteBlockOptions_CanBePopulated_Data))]
        public void FlexiQuoteBlockOptions_CanBePopulated(FlexiQuoteBlockOptions dummyInitialOptions,
            FlexiQuoteBlockOptions dummyExpectedOptions,
            string dummyJson)
        {
            // Act
            JsonConvert.PopulateObject(dummyJson, dummyInitialOptions);

            // Assert
            FlexiQuoteBlockOptions result = dummyInitialOptions;
            FlexiQuoteBlockOptions expectedResult = dummyExpectedOptions;
            Assert.Equal(expectedResult.BlockName, result.BlockName);
            Assert.Equal(expectedResult.Icon, result.Icon);
            Assert.Equal(expectedResult.CiteLink, result.CiteLink);
            Assert.Equal(expectedResult.Attributes, result.Attributes);
        }

        public static IEnumerable<object[]> FlexiQuoteBlockOptions_CanBePopulated_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string dummyIcon = "dummyIcon";
            const int dummyCiteLink = -3;
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
                    new FlexiQuoteBlockOptions(),
                    new FlexiQuoteBlockOptions(dummyBlockName,
                        dummyIcon,
                        dummyCiteLink,
                        dummyAttributes1),
                    $@"{{
    ""{nameof(FlexiQuoteBlockOptions.BlockName)}"": ""{dummyBlockName}"",
    ""{nameof(FlexiQuoteBlockOptions.Icon)}"": ""{dummyIcon}"",
    ""{nameof(FlexiQuoteBlockOptions.CiteLink)}"": ""{dummyCiteLink}"",
    ""{nameof(FlexiQuoteBlockOptions.Attributes)}"": {{
        ""{dummyAttribute1}"": ""{dummyAttributeValue1}""
    }}
}}"
                },

                // Existing values should not be overwritten if no corresponding new value
                new object[]
                {
                    new FlexiQuoteBlockOptions(dummyBlockName),
                    new FlexiQuoteBlockOptions(dummyBlockName, dummyIcon), // Block name should stay the same
                    $@"{{ ""{nameof(FlexiQuoteBlockOptions.Icon)}"": ""{dummyIcon}"" }}"
                },

                // Populating FlexiQuoteBlockOptions with an existing attributes collection should replace the entire collection
                new object[]
                {
                    new FlexiQuoteBlockOptions(attributes: dummyAttributes1),
                    new FlexiQuoteBlockOptions(attributes: dummyAttributes2),
                    $@"{{
    ""{nameof(FlexiQuoteBlockOptions.Attributes)}"": {{
        ""{dummyAttribute2}"": ""{dummyAttributeValue2}""
    }}
}}"
                }
            };
        }
    }
}
