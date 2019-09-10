using Jering.Markdig.Extensions.FlexiBlocks.FlexiCardsBlocks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiCardsBlocks
{
    public class FlexiCardBlockOptionsUnitTests
    {
        [Theory]
        [MemberData(nameof(FlexiCardBlockOptions_CanBePopulated_Data))]
        public void FlexiCardBlockOptions_CanBePopulated(FlexiCardBlockOptions dummyInitialOptions,
            FlexiCardBlockOptions dummyExpectedOptions,
            string dummyJson)
        {
            // Act
            JsonConvert.PopulateObject(dummyJson, dummyInitialOptions);

            // Assert
            FlexiCardBlockOptions result = dummyInitialOptions;
            FlexiCardBlockOptions expectedResult = dummyExpectedOptions;
            Assert.Equal(expectedResult.Url, result.Url);
            Assert.Equal(expectedResult.BackgroundIcon, result.BackgroundIcon);
            Assert.Equal(expectedResult.Attributes, result.Attributes);
        }

        public static IEnumerable<object[]> FlexiCardBlockOptions_CanBePopulated_Data()
        {
            const string dummyUrl = "dummyUrl";
            const string dummyBackgroundIcon = "dummyBackgroundIcon";
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
                    new FlexiCardBlockOptions(),
                    new FlexiCardBlockOptions(dummyUrl,
                        dummyBackgroundIcon,
                        dummyAttributes1),
                    $@"{{
    ""{nameof(FlexiCardBlockOptions.Url)}"": ""{dummyUrl}"",
    ""{nameof(FlexiCardBlockOptions.BackgroundIcon)}"": ""{dummyBackgroundIcon}"",
    ""{nameof(FlexiCardBlockOptions.Attributes)}"": {{
        ""{dummyAttribute1}"": ""{dummyAttributeValue1}""
    }}
}}"
                },

                // Existing values should not be overwritten if no corresponding new value
                new object[]
                {
                    new FlexiCardBlockOptions(dummyUrl),
                    new FlexiCardBlockOptions(dummyUrl, dummyBackgroundIcon), // Url should stay the same
                    $@"{{ ""{nameof(FlexiCardBlockOptions.BackgroundIcon)}"": ""{dummyBackgroundIcon}"" }}"
                },

                // Populating FlexiCardBlockOptions with an existing attributes collection should replace the entire collection
                new object[]
                {
                    new FlexiCardBlockOptions(attributes: dummyAttributes1),
                    new FlexiCardBlockOptions(attributes: dummyAttributes2),
                    $@"{{
    ""{nameof(FlexiCardBlockOptions.Attributes)}"": {{
        ""{dummyAttribute2}"": ""{dummyAttributeValue2}""
    }}
}}"
                }
            };
        }
    }
}
