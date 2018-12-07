using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
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
            Assert.Equal(expectedResult.SourceUri, result.SourceUri);
            Assert.Equal(expectedResult.Type, result.Type);
            Assert.Equal(expectedResult.CacheOnDisk, result.CacheOnDisk);
            Assert.Equal(expectedResult.DiskCacheDirectory, result.DiskCacheDirectory);
            Assert.Equal(expectedResult.Clippings, result.Clippings);
            Assert.Equal(expectedResult.Attributes, result.Attributes);
        }

        public static IEnumerable<object[]> FlexiIncludeBlockOptions_CanBePopulated_Data()
        {
            const string dummySourceUri = "dummySourceUri";
            const IncludeType dummyType = IncludeType.Markdown;
            const bool dummyCacheOnDisk = false;
            const string dummyDiskCacheDirectory = "dummyDiskCacheDirectory";

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
                    new SerializableWrapper<FlexiIncludeBlockOptions>(new FlexiIncludeBlockOptions(dummySourceUri,
                        dummyType,
                        dummyCacheOnDisk,
                        dummyDiskCacheDirectory,
                        dummyClippings1)),
                    $@"{{
    ""{nameof(FlexiIncludeBlockOptions.SourceUri)}"": ""{dummySourceUri}"",
    ""{nameof(FlexiIncludeBlockOptions.Type)}"": ""{dummyType}"",
    ""{nameof(FlexiIncludeBlockOptions.CacheOnDisk)}"": ""{dummyCacheOnDisk}"",
    ""{nameof(FlexiIncludeBlockOptions.DiskCacheDirectory)}"": ""{dummyDiskCacheDirectory}"",
    ""{nameof(FlexiIncludeBlockOptions.Clippings)}"": [
        {{
            ""{nameof(Clipping.StartLineNumber)}"": ""{dummyClipping1.StartLineNumber}"",
            ""{nameof(Clipping.EndLineNumber)}"": ""{dummyClipping1.EndLineNumber}""
        }}
    ]
}}"
                },

                // Populating FlexiIncludeBlockOptions with an existing attributes collection (should be replaced instead of appended to)
                new object[]
                {
                    new SerializableWrapper<FlexiIncludeBlockOptions>(new FlexiIncludeBlockOptions(clippings: dummyClippings1)),
                    new SerializableWrapper<FlexiIncludeBlockOptions>(new FlexiIncludeBlockOptions(clippings: dummyClippings2)),
                    $@"{{
    ""{nameof(FlexiIncludeBlockOptions.Clippings)}"": [
        {{
            ""{nameof(Clipping.StartLineNumber)}"": ""{dummyClipping2.StartLineNumber}"",
            ""{nameof(Clipping.EndLineNumber)}"": ""{dummyClipping2.EndLineNumber}""
        }}
    ]
}}"
                }
            };
        }

        [Fact]
        public void ValidateAndPopulate_ThrowsFlexiBlocksExceptionIfSourceUriIsNull()
        {
            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => new FlexiIncludeBlockOptions(null));
            Assert.Equal(string.Format(Strings.FlexiBlocksException_Shared_OptionsMustNotBeNull, nameof(FlexiIncludeBlockOptions.SourceUri)), result.Message);
        }

        [Fact]
        public void ValidateAndPopulate_ThrowsFlexiBlocksExceptionIfTypeIsNotWithinTheRangeOfValidValuesForTheEnumIncludeType()
        {
            // Arrange
            const IncludeType dummyType = (IncludeType)100; // Arbitrary int that is unlikely to ever be used in the enum

            // Act and assert
            FlexiBlocksException result = Assert.
                Throws<FlexiBlocksException>(() => new FlexiIncludeBlockOptions(type: dummyType));
            Assert.Equal(string.Format(Strings.FlexiBlocksException_Shared_OptionMustBeAValidEnumValue,
                    dummyType,
                    nameof(FlexiIncludeBlockOptions.Type),
                    nameof(IncludeType)),
                result.Message);
        }

        [Theory]
        [MemberData(nameof(ValidateAndPopulate_PopulatesResolvedDiskCacheDirectory_Data))]
        public void ValidateAndPopulate_PopulatesResolvedDiskCacheDirectory(bool dummyCacheOnDisk, string dummyDiskCacheDirectory, string expectedResolvedDiskCacheDirectory)
        {
            // Act
            var result = new FlexiIncludeBlockOptions(cacheOnDisk: dummyCacheOnDisk, diskCacheDirectory: dummyDiskCacheDirectory);

            // Assert
            Assert.Equal(expectedResolvedDiskCacheDirectory, result.ResolvedDiskCacheDirectory);
        }

        public static IEnumerable<object[]> ValidateAndPopulate_PopulatesResolvedDiskCacheDirectory_Data()
        {
            const string dummyDiskCacheDirectory = "dummyDiskCacheDirectory";
            string dummyDefaultDiskCacheDirectory = Path.Combine(Directory.GetCurrentDirectory(), "SourceCache");
            return new object[][]
            {
                // CacheOnDisk == true, DiskCacheDirectory defined
                new object[]
                {
                    true,
                    dummyDiskCacheDirectory,
                    dummyDiskCacheDirectory
                },
                // CacheOnDisk == true, DiskCacheDirectory == null
                new object[]
                {
                    true,
                    null,
                    dummyDefaultDiskCacheDirectory
                },
                // CacheOnDisk == true, DiskCacheDirectory == whitespace
                new object[]
                {
                    true,
                    " ",
                    dummyDefaultDiskCacheDirectory
                },
                // CacheOnDisk == true, DiskCacheDirectory == empty string
                new object[]
                {
                    true,
                    string.Empty,
                    dummyDefaultDiskCacheDirectory
                }
            };
        }

        [Fact]
        public void ValidateAndPopulate_SetsResolvedDiskCacheDirectoryToNullIfCacheOnDiskIsFalse()
        {
            // Arrange
            const string dummyDiskCacheDirectory = "dummyDiskCacheDirectory";
            var testSubject = new FlexiIncludeBlockOptions(diskCacheDirectory: dummyDiskCacheDirectory);

            // Act
            string initialResolvedDiskCacheDirectory = testSubject.ResolvedDiskCacheDirectory;
            JsonConvert.PopulateObject("{\"cacheOnDisk\": false}", testSubject);

            // Assert
            Assert.Equal(dummyDiskCacheDirectory, initialResolvedDiskCacheDirectory);
            Assert.Null(testSubject.ResolvedDiskCacheDirectory); // Writes over initial value
        }
    }
}
