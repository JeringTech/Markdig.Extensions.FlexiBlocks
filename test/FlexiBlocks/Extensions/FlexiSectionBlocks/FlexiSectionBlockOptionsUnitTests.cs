using Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiSectionBlocks
{
    public class FlexiSectionBlockOptionsUnitTests
    {
        [Theory]
        [MemberData(nameof(FlexiSectionBlockOptions_CanBePopulated_Data))]
        public void FlexiSectionBlockOptions_CanBePopulated(SerializableWrapper<FlexiSectionBlockOptions> dummyInitialOptionsWrapper,
                   SerializableWrapper<FlexiSectionBlockOptions> dummyExpectedOptionsWrapper,
                   string dummyJson)
        {
            // Act
            JsonConvert.PopulateObject(dummyJson, dummyInitialOptionsWrapper.Value);

            // Assert
            FlexiSectionBlockOptions result = dummyInitialOptionsWrapper.Value;
            FlexiSectionBlockOptions expectedResult = dummyExpectedOptionsWrapper.Value;
            Assert.Equal(expectedResult.BlockName, result.BlockName);
            Assert.Equal(expectedResult.Element, result.Element);
            Assert.Equal(expectedResult.GenerateID, result.GenerateID);
            Assert.Equal(expectedResult.LinkIcon, result.LinkIcon);
            Assert.Equal(expectedResult.ReferenceLinkable, result.ReferenceLinkable);
            Assert.Equal(expectedResult.RenderingMode, result.RenderingMode);
            Assert.Equal(expectedResult.Attributes, result.Attributes);
        }

        public static IEnumerable<object[]> FlexiSectionBlockOptions_CanBePopulated_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const SectioningContentElement dummyElement = SectioningContentElement.Aside;
            const bool dummyGenerateID = false;
            const string dummyLinkIcon = "dummyLinkIcon";
            const bool dummyReferenceLinkable = false;
            const FlexiSectionBlockRenderingMode dummyRenderingMode = FlexiSectionBlockRenderingMode.Classic;
            const string dummyAttribute1 = "dummyAttribute1";
            const string dummyAttributeValue1 = "dummyAttributeValue1";
            var dummyAttributes1 = new Dictionary<string, string> { { dummyAttribute1, dummyAttributeValue1 } };
            const string dummyAttribute2 = "dummyAttribute2";
            const string dummyAttributeValue2 = "dummyAttributeValue2";
            var dummyAttributes2 = new Dictionary<string, string> { { dummyAttribute2, dummyAttributeValue2 } };

            return new object[][]
            {
                // Populating FlexiSectionBlockOptions containing default values
                new object[]
                {
                    new SerializableWrapper<FlexiSectionBlockOptions>(new FlexiSectionBlockOptions()),
                    new SerializableWrapper<FlexiSectionBlockOptions>(new FlexiSectionBlockOptions(dummyBlockName,
                        dummyElement,
                        dummyGenerateID,
                        dummyLinkIcon,
                        dummyReferenceLinkable,
                        dummyRenderingMode,
                        dummyAttributes1)),
                    $@"{{
    ""{nameof(FlexiSectionBlockOptions.BlockName)}"": ""{dummyBlockName}"",
    ""{nameof(FlexiSectionBlockOptions.Element)}"": ""{dummyElement}"",
    ""{nameof(FlexiSectionBlockOptions.GenerateID)}"": ""{dummyGenerateID}"",
    ""{nameof(FlexiSectionBlockOptions.LinkIcon)}"": ""{dummyLinkIcon}"",
    ""{nameof(FlexiSectionBlockOptions.ReferenceLinkable)}"": ""{dummyReferenceLinkable}"",
    ""{nameof(FlexiSectionBlockOptions.RenderingMode)}"": ""{dummyRenderingMode}"",
    ""{nameof(FlexiSectionBlockOptions.Attributes)}"": {{
        ""{dummyAttribute1}"": ""{dummyAttributeValue1}""
    }}
}}"
                },

                // Populating FlexiSectionBlockOptions with an existing attributes collection (should be replaced instead of appended to)
                new object[]
                {
                    new SerializableWrapper<FlexiSectionBlockOptions>(new FlexiSectionBlockOptions(attributes: dummyAttributes1)),
                    new SerializableWrapper<FlexiSectionBlockOptions>(new FlexiSectionBlockOptions(attributes: dummyAttributes2)),
                    $@"{{
    ""{nameof(FlexiSectionBlockOptions.Attributes)}"": {{
        ""{dummyAttribute2}"": ""{dummyAttributeValue2}""
    }}
}}"
                }
            };
        }
    }
}
