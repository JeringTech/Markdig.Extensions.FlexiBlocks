using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using Newtonsoft.Json;
using System;
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
            Assert.Equal(expectedResult.BaseUri, result.BaseUri);
            Assert.Equal(expectedResult.Type, result.Type);
            Assert.Equal(expectedResult.CacheOnDisk, result.CacheOnDisk);
            Assert.Equal(expectedResult.DiskCacheDirectory, result.DiskCacheDirectory);
            Assert.Equal(expectedResult.Clippings, result.Clippings);
            Assert.Equal(expectedResult.Attributes, result.Attributes);
        }

        public static IEnumerable<object[]> FlexiIncludeBlockOptions_CanBePopulated_Data()
        {
            const string dummySourceUri = "dummySourceUri";
            const string dummyBaseUri = "C:/dummy/base/uri";
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
                        dummyBaseUri,
                        dummyType,
                        dummyCacheOnDisk,
                        dummyDiskCacheDirectory,
                        dummyClippings1)),
                    $@"{{
    ""{nameof(FlexiIncludeBlockOptions.SourceUri)}"": ""{dummySourceUri}"",
    ""{nameof(FlexiIncludeBlockOptions.BaseUri)}"": ""{dummyBaseUri}"",
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
            Assert.Equal(string.Format(Strings.FlexiBlocksException_OptionsMustNotBeNull, nameof(FlexiIncludeBlockOptions.SourceUri)), result.Message);
        }

        [Theory]
        [MemberData(nameof(ValidateAndPopulate_ThrowsFlexiBlocksExceptionIfSourceUriSchemeIsUnsupported_Data))]
        public void ValidateAndPopulate_ThrowsFlexiBlocksExceptionIfSourceUriSchemeIsUnsupported(string dummySourceUri, string expectedScheme)
        {
            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => new FlexiIncludeBlockOptions(dummySourceUri));
            Assert.Equal(string.Format(Strings.FlexiBlocksException_OptionMustBeAUriWithASupportedScheme,
                    nameof(FlexiIncludeBlockOptions.SourceUri),
                    dummySourceUri,
                    expectedScheme),
                result.Message);
        }

        public static IEnumerable<object[]> ValidateAndPopulate_ThrowsFlexiBlocksExceptionIfSourceUriSchemeIsUnsupported_Data()
        {
            return new object[][]
            {
                        new object[]{ "ftp://base/uri", "ftp" },
                        new object[]{ "mailto:base@uri.com", "mailto" },
                        new object[]{ "gopher://base.uri.com/", "gopher" }
            };
        }

        [Theory]
        [MemberData(nameof(ValidateAndPopulate_ThrowsFlexiBlocksExceptionIfBaseUriIsNotAnAbsoluteUri_Data))]
        public void ValidateAndPopulate_ThrowsFlexiBlocksExceptionIfBaseUriIsNotAnAbsoluteUri(string dummyBaseUri)
        {
            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => new FlexiIncludeBlockOptions(baseUri: dummyBaseUri));
            Assert.Equal(string.Format(Strings.FlexiBlocksException_OptionMustBeAnAbsoluteUri, nameof(FlexiIncludeBlockOptions.BaseUri), dummyBaseUri), result.Message);
        }

        public static IEnumerable<object[]> ValidateAndPopulate_ThrowsFlexiBlocksExceptionIfBaseUriIsNotAnAbsoluteUri_Data()
        {
            return new object[][]
            {
                // Common relative (non absolute) URIs, see http://www.ietf.org/rfc/rfc3986.txt, section 5.4.1
                new object[]{ "./relative/uri" },
                new object[]{ "../relative/uri" },
                new object[]{ "/relative/uri"  },
                new object[]{ "relative/uri"  }
            };
        }

        [Theory]
        [MemberData(nameof(ValidateAndPopulate_ThrowsFlexiBlocksExceptionIfBaseUriSchemeIsUnsupported_Data))]
        public void ValidateAndPopulate_ThrowsFlexiBlocksExceptionIfBaseUriSchemeIsUnsupported(string dummyBaseUri, string expectedScheme)
        {
            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => new FlexiIncludeBlockOptions(baseUri: dummyBaseUri));
            Assert.Equal(string.Format(Strings.FlexiBlocksException_OptionMustBeAUriWithASupportedScheme,
                    nameof(FlexiIncludeBlockOptions.BaseUri),
                    dummyBaseUri,
                    expectedScheme),
                result.Message);
        }

        public static IEnumerable<object[]> ValidateAndPopulate_ThrowsFlexiBlocksExceptionIfBaseUriSchemeIsUnsupported_Data()
        {
            return new object[][]
            {
                        new object[]{ "ftp://base/uri", "ftp" },
                        new object[]{ "mailto:base@uri.com", "mailto" },
                        new object[]{ "gopher://base.uri.com/", "gopher" }
            };
        }

        [Theory]
        [MemberData(nameof(ValidateAndPopulate_PopulatesNormalizedSourceUri_Data))]
        public void ValidateAndPopulate_PopulatesNormalizedSourceUri(string dummySourceUri, string dummyBaseUri, string expectedAbsoluteUri)
        {
            // Act
            var result = new FlexiIncludeBlockOptions(dummySourceUri, dummyBaseUri);

            // Assert
            Assert.Equal(expectedAbsoluteUri, result.NormalizedSourceUri.AbsoluteUri);
        }

        public static IEnumerable<object[]> ValidateAndPopulate_PopulatesNormalizedSourceUri_Data()
        {
            return new object[][]
            {
                // Absolute SourceUri
                new object[]
                {
                    "C:/absolute/source/uri",
                    null,
                    "file:///C:/absolute/source/uri"
                },
                // Relative SourceUri with specified BaseUri
                new object[]
                {
                    "relative/source/uri",
                    "http://absolute.base/uri/",
                    "http://absolute.base/uri/relative/source/uri"
                },
                // Relative SourceUri with null BaseUri
                new object[]
                {
                    "relative/source/uri",
                    null,
                    $"{new Uri(Directory.GetCurrentDirectory()).AbsoluteUri}/relative/source/uri"
                },
                // Relative SourceUri with whitespace BaseUri
                new object[]
                {
                    "relative/source/uri",
                    " ",
                    $"{new Uri(Directory.GetCurrentDirectory()).AbsoluteUri}/relative/source/uri"
                },
                // Relative SourceUri with empty BaseUri
                new object[]
                {
                    "relative/source/uri",
                    string.Empty,
                    $"{new Uri(Directory.GetCurrentDirectory()).AbsoluteUri}/relative/source/uri"
                },
            };
        }

        [Fact]
        public void ValidateAndPopulate_ThrowsFlexiBlocksExceptionIfTypeIsNotWithinTheRangeOfValidValuesForTheEnumIncludeType()
        {
            // Arrange
            const IncludeType dummyType = (IncludeType)100; // Arbitrary int that is unlikely to ever be used in the enum

            // Act and assert
            FlexiBlocksException result = Assert.
                Throws<FlexiBlocksException>(() => new FlexiIncludeBlockOptions(type: dummyType));
            Assert.Equal(string.Format(Strings.FlexiBlocksException_OptionMustBeAValidEnumValue,
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
