using Jering.Markdig.Extensions.FlexiBlocks.FlexiBannerBlocks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiBannerBlocks
{
    public class FlexiBannerBlockOptionsUnitTests
    {
        [Theory]
        [MemberData(nameof(FlexiBannerBlockOptions_CanBePopulated_Data))]
        public void FlexiBannerBlockOptions_CanBePopulated(FlexiBannerBlockOptions dummyInitialOptions,
            FlexiBannerBlockOptions dummyExpectedOptions,
            string dummyJson)
        {
            // Act
            JsonConvert.PopulateObject(dummyJson, dummyInitialOptions);

            // Assert
            FlexiBannerBlockOptions result = dummyInitialOptions;
            FlexiBannerBlockOptions expectedResult = dummyExpectedOptions;
            Assert.Equal(expectedResult.BlockName, result.BlockName);
            Assert.Equal(expectedResult.LogoIcon, result.LogoIcon);
            Assert.Equal(expectedResult.BackgroundIcon, result.BackgroundIcon);
            Assert.Equal(expectedResult.Attributes, result.Attributes);
        }

        public static IEnumerable<object[]> FlexiBannerBlockOptions_CanBePopulated_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string dummyLogoIcon = "dummyLogoIcon";
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
                    new FlexiBannerBlockOptions(),
                    new FlexiBannerBlockOptions(dummyBlockName,
                        dummyLogoIcon,
                        dummyBackgroundIcon,
                        dummyAttributes1),
                    $@"{{
    ""{nameof(FlexiBannerBlockOptions.BlockName)}"": ""{dummyBlockName}"",
    ""{nameof(FlexiBannerBlockOptions.LogoIcon)}"": ""{dummyLogoIcon}"",
    ""{nameof(FlexiBannerBlockOptions.BackgroundIcon)}"": ""{dummyBackgroundIcon}"",
    ""{nameof(FlexiBannerBlockOptions.Attributes)}"": {{
        ""{dummyAttribute1}"": ""{dummyAttributeValue1}""
    }}
}}"
                },

                // Existing values should not be overwritten if no corresponding new value
                new object[]
                {
                    new FlexiBannerBlockOptions(dummyBlockName),
                    new FlexiBannerBlockOptions(dummyBlockName, dummyLogoIcon), // Block name should stay the same
                    $@"{{ ""{nameof(FlexiBannerBlockOptions.LogoIcon)}"": ""{dummyLogoIcon}"" }}"
                },

                // Populating FlexiBannerBlockOptions with an existing attributes collection should replace the entire collection
                new object[]
                {
                    new FlexiBannerBlockOptions(attributes: dummyAttributes1),
                    new FlexiBannerBlockOptions(attributes: dummyAttributes2),
                    $@"{{
    ""{nameof(FlexiBannerBlockOptions.Attributes)}"": {{
        ""{dummyAttribute2}"": ""{dummyAttributeValue2}""
    }}
}}"
                }
            };
        }
    }
}
