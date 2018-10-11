using Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiTableBlocks
{
    public class FlexiTableBlockOptionsUnitTests
    {
        [Theory]
        [MemberData(nameof(FlexiTableBlockOptions_CanBePopulated_Data))]
        public void FlexiTableBlockOptions_CanBePopulated(SerializableWrapper<FlexiTableBlockOptions> dummyInitialOptionsWrapper,
            SerializableWrapper<FlexiTableBlockOptions> dummyExpectedOptionsWrapper,
            string dummyJson)
        {
            // Act
            JsonConvert.PopulateObject(dummyJson, dummyInitialOptionsWrapper.Value);

            // Assert
            FlexiTableBlockOptions result = dummyInitialOptionsWrapper.Value;
            FlexiTableBlockOptions expectedResult = dummyExpectedOptionsWrapper.Value;
            Assert.Equal(expectedResult.Class, result.Class);
            Assert.Equal(expectedResult.WrapperElement, result.WrapperElement);
            Assert.Equal(expectedResult.LabelAttribute, result.LabelAttribute);
            Assert.Equal(expectedResult.Attributes, result.Attributes);
        }

        public static IEnumerable<object[]> FlexiTableBlockOptions_CanBePopulated_Data()
        {
            const string dummyClass = "dummyClass";
            const string dummyWrapperElement = "dummyWrapperElement";
            const string dummyLabelAttribute = "dummyLabelAttribute";
            const string dummyAttribute1 = "dummyAttribute1";
            const string dummyAttributeValue1 = "dummyAttributeValue1";
            var dummyAttributes1 = new Dictionary<string, string> { { dummyAttribute1, dummyAttributeValue1 } };
            const string dummyAttribute2 = "dummyAttribute2";
            const string dummyAttributeValue2 = "dummyAttributeValue2";
            var dummyAttributes2 = new Dictionary<string, string> { { dummyAttribute2, dummyAttributeValue2 } };

            return new object[][]
            {
                // Populating FlexiTableBlockOptions containing default values
                new object[]
                {
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions()),
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions(
                        dummyClass,
                        dummyWrapperElement,
                        dummyLabelAttribute,
                        dummyAttributes1)),
                    $@"{{
    ""{nameof(FlexiTableBlockOptions.Class)}"": ""{dummyClass}"",
    ""{nameof(FlexiTableBlockOptions.WrapperElement)}"": ""{dummyWrapperElement}"",
    ""{nameof(FlexiTableBlockOptions.LabelAttribute)}"": ""{dummyLabelAttribute}"",
    ""{nameof(FlexiTableBlockOptions.Attributes)}"": {{
        ""{dummyAttribute1}"": ""{dummyAttributeValue1}""
    }}
}}"
                },

                // Populating FlexiTableBlockOptions with an existing attributes collection (should be replaced instead of appended to)
                new object[]
                {
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions(attributes: dummyAttributes1)),
                    new SerializableWrapper<FlexiTableBlockOptions>(new FlexiTableBlockOptions(attributes: dummyAttributes2)),
                    $@"{{
    ""{nameof(FlexiTableBlockOptions.Attributes)}"": {{
        ""{dummyAttribute2}"": ""{dummyAttributeValue2}""
    }}
}}"
                }
            };
        }
    }
}
