using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiIncludeBlocks
{
    public class FlexiIncludeBlockOptionsUnitTests
    {
        [Theory]
        [MemberData(nameof(FlexiIncludeBlockOptions_CanBePopulated_Data))]
        public void FlexiIncludeBlockOptions_CanBePopulated(SerializableWrapper<FlexiIncludeBlockOptions> dummyInitialOptionsWrapper,
            SerializableWrapper<FlexiIncludeBlockOptions> dummyExpectedOptionsWrapper,
            string dummyJson)
        {
            // Act
            JsonConvert.PopulateObject(dummyJson, dummyInitialOptionsWrapper.Value);

            // Assert
            FlexiIncludeBlockOptions result = dummyInitialOptionsWrapper.Value;
            FlexiIncludeBlockOptions expectedResult = dummyExpectedOptionsWrapper.Value;
            Assert.Equal(expectedResult.Source, result.Source);
            Assert.Equal(expectedResult.Clippings, result.Clippings);
            Assert.Equal(expectedResult.Type, result.Type);
            Assert.Equal(expectedResult.Cache, result.Cache);
            Assert.Equal(expectedResult.CacheDirectory, result.CacheDirectory);
        }

        public static IEnumerable<object[]> FlexiIncludeBlockOptions_CanBePopulated_Data()
        {
            const string dummySource = "dummySource";
            const FlexiIncludeType dummyType = FlexiIncludeType.Markdown;
            const bool dummyCache = false;
            const string dummyCacheDirectory = "dummyCacheDirectory";

            var dummyClipping1 = new Clipping(10, 15);
            var dummyClippings1 = new List<Clipping> { dummyClipping1 };
            var dummyClipping2 = new Clipping(2, 21);
            var dummyClippings2 = new List<Clipping> { dummyClipping2 };

            return new object[][]
            {
                // Populating FlexiIncludeBlockOptions containing default values
                new object[]
                {
                    new SerializableWrapper<FlexiIncludeBlockOptions>(new FlexiIncludeBlockOptions()),
                    new SerializableWrapper<FlexiIncludeBlockOptions>(new FlexiIncludeBlockOptions(dummySource,
                        dummyClippings1,
                        dummyType,
                        dummyCache,
                        dummyCacheDirectory)),
                    $@"{{
    ""{nameof(FlexiIncludeBlockOptions.Source)}"": ""{dummySource}"",
    ""{nameof(FlexiIncludeBlockOptions.Type)}"": ""{dummyType}"",
    ""{nameof(FlexiIncludeBlockOptions.Cache)}"": ""{dummyCache}"",
    ""{nameof(FlexiIncludeBlockOptions.CacheDirectory)}"": ""{dummyCacheDirectory}"",
    ""{nameof(FlexiIncludeBlockOptions.Clippings)}"": [
        {{
            ""{nameof(Clipping.StartLine)}"": ""{dummyClipping1.StartLine}"",
            ""{nameof(Clipping.EndLine)}"": ""{dummyClipping1.EndLine}""
        }}
    ]
}}"
                },

                // Populating FlexiIncludeBlockOptions with an existing clippings collection (should be replaced instead of appended to)
                new object[]
                {
                    new SerializableWrapper<FlexiIncludeBlockOptions>(new FlexiIncludeBlockOptions(clippings: dummyClippings1)),
                    new SerializableWrapper<FlexiIncludeBlockOptions>(new FlexiIncludeBlockOptions(clippings: dummyClippings2)),
                    $@"{{
    ""{nameof(FlexiIncludeBlockOptions.Clippings)}"": [
        {{
            ""{nameof(Clipping.StartLine)}"": ""{dummyClipping2.StartLine}"",
            ""{nameof(Clipping.EndLine)}"": ""{dummyClipping2.EndLine}""
        }}
    ]
}}"
                }
            };
        }
    }
}
