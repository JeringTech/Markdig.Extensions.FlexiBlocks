using Jering.Markdig.Extensions.FlexiBlocks.IncludeBlocks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.IncludeBlocks
{
    public class IncludeBlockOptionsUnitTests
    {
        [Theory]
        [MemberData(nameof(IncludeBlockOptions_CanBePopulated_Data))]
        public void IncludeBlockOptions_CanBePopulated(SerializableWrapper<IncludeBlockOptions> dummyInitialOptionsWrapper,
            SerializableWrapper<IncludeBlockOptions> dummyExpectedOptionsWrapper,
            string dummyJson)
        {
            // Act
            JsonConvert.PopulateObject(dummyJson, dummyInitialOptionsWrapper.Value);

            // Assert
            IncludeBlockOptions result = dummyInitialOptionsWrapper.Value;
            IncludeBlockOptions expectedResult = dummyExpectedOptionsWrapper.Value;
            Assert.Equal(expectedResult.Source, result.Source);
            Assert.Equal(expectedResult.Clippings, result.Clippings);
            Assert.Equal(expectedResult.Type, result.Type);
            Assert.Equal(expectedResult.Cache, result.Cache);
            Assert.Equal(expectedResult.CacheDirectory, result.CacheDirectory);
        }

        public static IEnumerable<object[]> IncludeBlockOptions_CanBePopulated_Data()
        {
            const string dummySource = "dummySource";
            const IncludeType dummyType = IncludeType.Markdown;
            const bool dummyCache = false;
            const string dummyCacheDirectory = "dummyCacheDirectory";

            var dummyClipping1 = new Clipping(10, 15);
            var dummyClippings1 = new List<Clipping> { dummyClipping1 };
            var dummyClipping2 = new Clipping(2, 21);
            var dummyClippings2 = new List<Clipping> { dummyClipping2 };

            return new object[][]
            {
                // Populating IncludeBlockOptions containing default values
                new object[]
                {
                    new SerializableWrapper<IncludeBlockOptions>(new IncludeBlockOptions()),
                    new SerializableWrapper<IncludeBlockOptions>(new IncludeBlockOptions(dummySource,
                        dummyClippings1,
                        dummyType,
                        dummyCache,
                        dummyCacheDirectory)),
                    $@"{{
    ""{nameof(IncludeBlockOptions.Source)}"": ""{dummySource}"",
    ""{nameof(IncludeBlockOptions.Type)}"": ""{dummyType}"",
    ""{nameof(IncludeBlockOptions.Cache)}"": ""{dummyCache}"",
    ""{nameof(IncludeBlockOptions.CacheDirectory)}"": ""{dummyCacheDirectory}"",
    ""{nameof(IncludeBlockOptions.Clippings)}"": [
        {{
            ""{nameof(Clipping.Start)}"": ""{dummyClipping1.Start}"",
            ""{nameof(Clipping.End)}"": ""{dummyClipping1.End}""
        }}
    ]
}}"
                },

                // Populating IncludeBlockOptions with an existing clippings collection (should be replaced instead of appended to)
                new object[]
                {
                    new SerializableWrapper<IncludeBlockOptions>(new IncludeBlockOptions(clippings: dummyClippings1)),
                    new SerializableWrapper<IncludeBlockOptions>(new IncludeBlockOptions(clippings: dummyClippings2)),
                    $@"{{
    ""{nameof(IncludeBlockOptions.Clippings)}"": [
        {{
            ""{nameof(Clipping.Start)}"": ""{dummyClipping2.Start}"",
            ""{nameof(Clipping.End)}"": ""{dummyClipping2.End}""
        }}
    ]
}}"
                }
            };
        }
    }
}
