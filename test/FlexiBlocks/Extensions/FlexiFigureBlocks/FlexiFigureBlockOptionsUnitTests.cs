using Jering.Markdig.Extensions.FlexiBlocks.FlexiFigureBlocks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiFigureBlocks
{
    public class FlexiFigureBlockOptionsUnitTests
    {
        [Theory]
        [MemberData(nameof(FlexiFigureBlockOptions_CanBePopulated_Data))]
        public void FlexiFigureBlockOptions_CanBePopulated(FlexiFigureBlockOptions dummyInitialOptions,
            FlexiFigureBlockOptions dummyExpectedOptions,
            string dummyJson)
        {
            // Act
            JsonConvert.PopulateObject(dummyJson, dummyInitialOptions);

            // Assert
            FlexiFigureBlockOptions result = dummyInitialOptions;
            FlexiFigureBlockOptions expectedResult = dummyExpectedOptions;
            Assert.Equal(expectedResult.BlockName, result.BlockName);
            Assert.Equal(expectedResult.ReferenceLinkable, result.ReferenceLinkable);
            Assert.Equal(expectedResult.LinkLabelContent, result.LinkLabelContent);
            Assert.Equal(expectedResult.GenerateID, result.GenerateID);
            Assert.Equal(expectedResult.RenderName, result.RenderName);
            Assert.Equal(expectedResult.Attributes, result.Attributes);
        }

        public static IEnumerable<object[]> FlexiFigureBlockOptions_CanBePopulated_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const bool dummyReferenceLinkable = false;
            const string dummyLinkLabelContent = "dummyLinkLabelContent";
            const bool dummyGenerateID = false;
            const bool dummyRenderName = false;
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
                    new FlexiFigureBlockOptions(),
                    new FlexiFigureBlockOptions(dummyBlockName,
                        dummyReferenceLinkable,
                        dummyLinkLabelContent,
                        dummyGenerateID,
                        dummyRenderName,
                        dummyAttributes1),
                    $@"{{
    ""{nameof(FlexiFigureBlockOptions.BlockName)}"": ""{dummyBlockName}"",
    ""{nameof(FlexiFigureBlockOptions.ReferenceLinkable)}"": ""{dummyReferenceLinkable}"",
    ""{nameof(FlexiFigureBlockOptions.LinkLabelContent)}"": ""{dummyLinkLabelContent}"",
    ""{nameof(FlexiFigureBlockOptions.GenerateID)}"": ""{dummyGenerateID}"",
    ""{nameof(FlexiFigureBlockOptions.RenderName)}"": ""{dummyRenderName}"",
    ""{nameof(FlexiFigureBlockOptions.Attributes)}"": {{
        ""{dummyAttribute1}"": ""{dummyAttributeValue1}""
    }}
}}"
                },

                // Existing values should not be overwritten if no corresponding new value
                new object[]
                {
                    new FlexiFigureBlockOptions(dummyBlockName),
                    new FlexiFigureBlockOptions(dummyBlockName, dummyReferenceLinkable), // Block name should stay the same
                    $@"{{ ""{nameof(FlexiFigureBlockOptions.ReferenceLinkable)}"": ""{dummyReferenceLinkable}"" }}"
                },

                // Populating FlexiFigureBlockOptions with an existing attributes collection should replace the entire collection
                new object[]
                {
                    new FlexiFigureBlockOptions(attributes: dummyAttributes1),
                    new FlexiFigureBlockOptions(attributes: dummyAttributes2),
                    $@"{{
    ""{nameof(FlexiFigureBlockOptions.Attributes)}"": {{
        ""{dummyAttribute2}"": ""{dummyAttributeValue2}""
    }}
}}"
                }
            };
        }
    }
}
