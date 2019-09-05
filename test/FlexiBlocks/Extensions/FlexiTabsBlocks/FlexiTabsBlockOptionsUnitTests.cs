using Jering.Markdig.Extensions.FlexiBlocks.FlexiTabsBlocks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiTabsBlocks
{
    public class FlexiTabsBlockOptionsUnitTests
    {
        [Theory]
        [MemberData(nameof(FlexiTabsBlockOptions_CanBePopulated_Data))]
        public void FlexiTabsBlockOptions_CanBePopulated(FlexiTabsBlockOptions dummyInitialOptions,
            FlexiTabsBlockOptions dummyExpectedOptions,
            string dummyJson)
        {
            // Act
            JsonConvert.PopulateObject(dummyJson, dummyInitialOptions);

            // Assert
            FlexiTabsBlockOptions result = dummyInitialOptions;
            FlexiTabsBlockOptions expectedResult = dummyExpectedOptions;
            Assert.Equal(expectedResult.BlockName, result.BlockName);
            Assert.Equal(expectedResult.DefaultTabOptions.Attributes, result.DefaultTabOptions.Attributes);
            Assert.Equal(expectedResult.Attributes, result.Attributes);
        }

        public static IEnumerable<object[]> FlexiTabsBlockOptions_CanBePopulated_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string dummyAttribute1 = "dummyAttribute1";
            const string dummyAttributeValue1 = "dummyAttributeValue1";
            var dummyTabsAttributes1 = new Dictionary<string, string> { { dummyAttribute1, dummyAttributeValue1 } };
            var dummyTabAttributes1 = new Dictionary<string, string> { { dummyAttribute1, dummyAttributeValue1 } };
            const string dummyAttribute2 = "dummyAttribute2";
            const string dummyAttributeValue2 = "dummyAttributeValue2";
            var dummyTabsAttributes2 = new Dictionary<string, string> { { dummyAttribute2, dummyAttributeValue2 } };
            var dummyFlexiTabBlockOptions = new FlexiTabBlockOptions(dummyTabAttributes1);

            return new object[][]
            {
                // Each new value should overwrite its corresponding existing value
                new object[]
                {
                    new FlexiTabsBlockOptions(),
                    new FlexiTabsBlockOptions(dummyBlockName,
                        dummyFlexiTabBlockOptions,
                        dummyTabsAttributes1),
                    $@"{{
    ""{nameof(FlexiTabsBlockOptions.BlockName)}"": ""{dummyBlockName}"",
    ""{nameof(FlexiTabsBlockOptions.DefaultTabOptions)}"": {{
        ""{nameof(FlexiTabBlockOptions.Attributes)}"": {{
            ""{dummyAttribute1}"": ""{dummyAttributeValue1}""
        }}
    }},
    ""{nameof(FlexiTabsBlockOptions.Attributes)}"": {{
        ""{dummyAttribute1}"": ""{dummyAttributeValue1}""
    }}
}}"
                },

                // Existing values should not be overwritten if no corresponding new value
                new object[]
                {
                    new FlexiTabsBlockOptions(dummyBlockName),
                    new FlexiTabsBlockOptions(dummyBlockName, dummyFlexiTabBlockOptions), // Block name should stay the same
                    $@"{{ 
    ""{nameof(FlexiTabsBlockOptions.DefaultTabOptions)}"": {{
        ""{nameof(FlexiTabBlockOptions.Attributes)}"": {{
            ""{dummyAttribute1}"": ""{dummyAttributeValue1}""
        }}
    }}
}}"
                },

                // Populating FlexiTabsBlockOptions with an existing attributes collection should replace the entire collection
                new object[]
                {
                    new FlexiTabsBlockOptions(attributes: dummyTabsAttributes1),
                    new FlexiTabsBlockOptions(attributes: dummyTabsAttributes2),
                    $@"{{
    ""{nameof(FlexiTabsBlockOptions.Attributes)}"": {{
        ""{dummyAttribute2}"": ""{dummyAttributeValue2}""
    }}
}}"
                }
            };
        }

        [Fact]
        public void Clone_ShallowClonesDefaultTabOptions()
        {
            // Arrange
            var dummyAttributes = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
            var dummyFlexiTabBlockOptions = new FlexiTabBlockOptions(dummyAttributes);
            var dummyFlexiTabsBlockOptions = new FlexiTabsBlockOptions(defaultTabOptions: dummyFlexiTabBlockOptions);

            // Act
            var result = (FlexiTabsBlockOptions)dummyFlexiTabsBlockOptions.Clone();

            // Assert
            Assert.NotSame(dummyFlexiTabBlockOptions, result.DefaultTabOptions);
            // This is fine since if attributes are specified in JSON, the entire collection is replaced -
            // i.e. we won't mess up the attributes collection of the original FlexiTabBlockOptions instance.
            Assert.Same(dummyAttributes, result.DefaultTabOptions.Attributes);
        }
    }
}
