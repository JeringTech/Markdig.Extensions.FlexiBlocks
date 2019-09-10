using Jering.Markdig.Extensions.FlexiBlocks.FlexiCardsBlocks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiCardsBlocks
{
    public class FlexiCardsBlockOptionsUnitTests
    {
        [Theory]
        [MemberData(nameof(FlexiCardsBlockOptions_CanBePopulated_Data))]
        public void FlexiCardsBlockOptions_CanBePopulated(FlexiCardsBlockOptions dummyInitialOptions,
            FlexiCardsBlockOptions dummyExpectedOptions,
            string dummyJson)
        {
            // Act
            JsonConvert.PopulateObject(dummyJson, dummyInitialOptions);

            // Assert
            FlexiCardsBlockOptions result = dummyInitialOptions;
            FlexiCardsBlockOptions expectedResult = dummyExpectedOptions;
            Assert.Equal(expectedResult.BlockName, result.BlockName);
            Assert.Equal(expectedResult.CardSize, result.CardSize);
            Assert.Equal(expectedResult.DefaultCardOptions.Url, result.DefaultCardOptions.Url);
            Assert.Equal(expectedResult.DefaultCardOptions.BackgroundIcon, result.DefaultCardOptions.BackgroundIcon);
            Assert.Equal(expectedResult.DefaultCardOptions.Attributes, result.DefaultCardOptions.Attributes);
            Assert.Equal(expectedResult.Attributes, result.Attributes);
        }

        public static IEnumerable<object[]> FlexiCardsBlockOptions_CanBePopulated_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const FlexiCardBlockSize dummyCardSize = FlexiCardBlockSize.Medium;
            const string dummyUrl = "dummyUrl";
            const string dummyBackgroundIcon = "dummyBackgroundIcon";
            const string dummyAttribute1 = "dummyAttribute1";
            const string dummyAttributeValue1 = "dummyAttributeValue1";
            var dummyCardsAttributes1 = new Dictionary<string, string> { { dummyAttribute1, dummyAttributeValue1 } };
            var dummyCardAttributes1 = new Dictionary<string, string> { { dummyAttribute1, dummyAttributeValue1 } };
            const string dummyAttribute2 = "dummyAttribute2";
            const string dummyAttributeValue2 = "dummyAttributeValue2";
            var dummyCardsAttributes2 = new Dictionary<string, string> { { dummyAttribute2, dummyAttributeValue2 } };

            return new object[][]
            {
                // Each new value should overwrite its corresponding existing value
                new object[]
                {
                    new FlexiCardsBlockOptions(),
                    new FlexiCardsBlockOptions(dummyBlockName,
                        dummyCardSize,
                        new FlexiCardBlockOptions(dummyUrl, dummyBackgroundIcon, dummyCardAttributes1),
                        dummyCardsAttributes1),
                    $@"{{
    ""{nameof(FlexiCardsBlockOptions.BlockName)}"": ""{dummyBlockName}"",
    ""{nameof(FlexiCardsBlockOptions.CardSize)}"": ""{dummyCardSize}"",
    ""{nameof(FlexiCardsBlockOptions.DefaultCardOptions)}"": {{
        ""{nameof(FlexiCardBlockOptions.Url)}"": ""{dummyUrl}"",
        ""{nameof(FlexiCardBlockOptions.BackgroundIcon)}"": ""{dummyBackgroundIcon}"",
        ""{nameof(FlexiCardBlockOptions.Attributes)}"": {{
            ""{dummyAttribute1}"": ""{dummyAttributeValue1}""
        }}
    }},
    ""{nameof(FlexiCardsBlockOptions.Attributes)}"": {{
        ""{dummyAttribute1}"": ""{dummyAttributeValue1}""
    }}
}}"
                },

                // Existing values should not be overwritten if no corresponding new value
                new object[]
                {
                    new FlexiCardsBlockOptions(dummyBlockName),
                    new FlexiCardsBlockOptions(dummyBlockName, dummyCardSize), // Block name should stay the same
                    $@"{{ ""{nameof(FlexiCardsBlockOptions.CardSize)}"": ""{dummyCardSize}"" }}"
                },

                // Populating FlexiCardsBlockOptions with an existing attributes collection should replace the entire collection
                new object[]
                {
                    new FlexiCardsBlockOptions(attributes: dummyCardsAttributes1),
                    new FlexiCardsBlockOptions(attributes: dummyCardsAttributes2),
                    $@"{{
    ""{nameof(FlexiCardsBlockOptions.Attributes)}"": {{
        ""{dummyAttribute2}"": ""{dummyAttributeValue2}""
    }}
}}"
                }
            };
        }

        [Fact]
        public void Clone_ShallowClonesDefaultCardOptions()
        {
            // Arrange
            const string dummyUrl = "dummyUrl";
            const string dummyBackgroundIcon = "dummyBackgroundIcon";
            var dummyAttributes = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
            var dummyFlexiCardBlockOptions = new FlexiCardBlockOptions(dummyUrl, dummyBackgroundIcon, dummyAttributes);
            var dummyFlexiCardsBlockOptions = new FlexiCardsBlockOptions(defaultCardOptions: dummyFlexiCardBlockOptions);

            // Act
            var result = (FlexiCardsBlockOptions)dummyFlexiCardsBlockOptions.Clone();

            // Assert
            Assert.NotSame(dummyFlexiCardBlockOptions, result.DefaultCardOptions);
            Assert.Equal(dummyUrl, result.DefaultCardOptions.Url);
            Assert.Equal(dummyBackgroundIcon, result.DefaultCardOptions.BackgroundIcon);
            // This is fine since if attributes are specified in JSON, the entire collection is replaced -
            // i.e. we won't mess up the attributes collection of the original FlexiCardBlockOptions instance.
            Assert.Same(dummyAttributes, result.DefaultCardOptions.Attributes);
        }
    }
}
