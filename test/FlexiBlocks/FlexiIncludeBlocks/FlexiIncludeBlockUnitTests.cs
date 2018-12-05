using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using Markdig.Helpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiIncludeBlocks
{
    // TODO SerializableWrapper does not work with FlexiIncludeBlock (test discovery hangs), seems like it has something to do with FlexiIncludeBlock not being 
    // JSON serializable/deserializable
    public class FlexiIncludeBlockUnitTests
    {
        [Fact]
        public void Constructor_AddsFlexiIncludeBlockToTreeIfItIsNotARootBlock()
        {
            // Arrange
            var dummyParentFlexiIncludeBlock = new FlexiIncludeBlock(null, null);

            // Act
            var result = new FlexiIncludeBlock(dummyParentFlexiIncludeBlock, null);

            // Assert
            Assert.Single(dummyParentFlexiIncludeBlock.ChildFlexiIncludeBlocks);
            Assert.Same(result, dummyParentFlexiIncludeBlock.ChildFlexiIncludeBlocks[0]);
            Assert.Same(dummyParentFlexiIncludeBlock, result.ParentFlexiIncludeBlock);
        }

        [Theory]
        [MemberData(nameof(Setup_ThrowsFlexiBlocksExceptionIfSourceUriSchemeIsUnsupported_Data))]
        public void Setup_ThrowsFlexiBlocksExceptionIfSourceUriSchemeIsUnsupported(string dummySourceUri, string expectedScheme)
        {
            // Arrange
            var testSubject = new FlexiIncludeBlock(null, null);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => testSubject.Setup(new FlexiIncludeBlockOptions(dummySourceUri), null));
            Assert.Equal(string.Format(Strings.FlexiBlocksException_FlexiIncludeBlock_OptionMustBeAUriWithASupportedScheme,
                    nameof(FlexiIncludeBlockOptions.SourceUri),
                    dummySourceUri,
                    expectedScheme),
                result.Message);
        }

        public static IEnumerable<object[]> Setup_ThrowsFlexiBlocksExceptionIfSourceUriSchemeIsUnsupported_Data()
        {
            return new object[][]
            {
                        new object[]{ "ftp://base/uri", "ftp" },
                        new object[]{ "mailto:base@uri.com", "mailto" },
                        new object[]{ "gopher://base.uri.com/", "gopher" }
            };
        }

        [Theory]
        [MemberData(nameof(Setup_ThrowsFlexiBlocksExceptionIfRootBaseUriIsNotAnAbsoluteUri_Data))]
        public void Setup_ThrowsFlexiBlocksExceptionIfRootBaseUriIsNotAnAbsoluteUri(string dummyRootBaseUri)
        {
            // Arrange
            var testSubject = new FlexiIncludeBlock(null, null);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => testSubject.Setup(new FlexiIncludeBlockOptions("relative/uri"), dummyRootBaseUri));
            Assert.Equal(string.Format(Strings.FlexiBlocksException_FlexiIncludeBlock_OptionMustBeAnAbsoluteUri, nameof(FlexiIncludeBlocksExtensionOptions.RootBaseUri), dummyRootBaseUri), result.Message);
        }

        public static IEnumerable<object[]> Setup_ThrowsFlexiBlocksExceptionIfRootBaseUriIsNotAnAbsoluteUri_Data()
        {
            return new object[][]
            {
                // Common relative (non absolute) URIs, see http://www.ietf.org/rfc/rfc3986.txt, section 5.4.1
                // Note: "/relative/uri" is considered a relative URI on Windows but it is considered an absolute URI on 
                // Linux/macOS, so we can't include a test for it.
                new object[]{ "./relative/uri" },
                new object[]{ "../relative/uri" },
                new object[]{ "relative/uri"  }
            };
        }

        [Theory]
        [MemberData(nameof(Setup_ThrowsFlexiBlocksExceptionIfRootBaseUriSchemeIsUnsupported_Data))]
        public void Setup_ThrowsFlexiBlocksExceptionIfRootBaseUriSchemeIsUnsupported(string dummyRootBaseUri, string expectedScheme)
        {
            // Arrange
            var testSubject = new FlexiIncludeBlock(null, null);

            // Act and assert
            FlexiBlocksException result = Assert.
                Throws<FlexiBlocksException>(() => testSubject.Setup(new FlexiIncludeBlockOptions("relative/uri"), dummyRootBaseUri));
            Assert.Equal(string.Format(Strings.FlexiBlocksException_FlexiIncludeBlock_OptionMustBeAUriWithASupportedScheme,
                    nameof(FlexiIncludeBlocksExtensionOptions.RootBaseUri),
                    dummyRootBaseUri,
                    expectedScheme),
                result.Message);
        }

        public static IEnumerable<object[]> Setup_ThrowsFlexiBlocksExceptionIfRootBaseUriSchemeIsUnsupported_Data()
        {
            return new object[][]
            {
                        new object[]{ "ftp://base/uri", "ftp" },
                        new object[]{ "mailto:base@uri.com", "mailto" },
                        new object[]{ "gopher://base.uri.com/", "gopher" }
            };
        }

        [Theory]
        [MemberData(nameof(Setup_PopulatesAbsoluteSourceUri_Data))]
        public void Setup_PopulatesAbsoluteSourceUri(string dummySourceUri,
            string dummyRootBaseUri,
            string dummyParentAbsoluteSourceUri,
            string expectedAbsoluteSourceUri)
        {
            // Arrange
            FlexiIncludeBlock dummyParentFlexiIncludeBlock = dummyParentAbsoluteSourceUri == null ? null : new FlexiIncludeBlock(null, null);
            dummyParentFlexiIncludeBlock?.Setup(new FlexiIncludeBlockOptions(dummyParentAbsoluteSourceUri), null);
            var dummyFlexiIncludeBlockOptions = new FlexiIncludeBlockOptions(dummySourceUri);
            var testSubject = new FlexiIncludeBlock(dummyParentFlexiIncludeBlock, null);
            testSubject.Setup(dummyFlexiIncludeBlockOptions, dummyRootBaseUri);

            // Act
            Uri result = testSubject.AbsoluteSourceUri;

            // Assert
            Assert.Equal(expectedAbsoluteSourceUri, result.AbsoluteUri);
        }

        public static IEnumerable<object[]> Setup_PopulatesAbsoluteSourceUri_Data()
        {
            const string absoluteSourceUri = "C:/absolute/source/uri";
            const string relativeSourceUri = "../../../relative/source/uri";

            return new object[][]
            {
                // Absolute SourceUri
                new object[]
                {
                    absoluteSourceUri,
                    null,
                    null,
                    new Uri(absoluteSourceUri).AbsoluteUri
                },
                // Relative SourceUri with specified parent FlexiIncludeBlock
                new object[]
                {
                    relativeSourceUri,
                    null,
                    absoluteSourceUri,
                    new Uri(new Uri(absoluteSourceUri), relativeSourceUri).AbsoluteUri
                },
                // Relative SourceUri with no parent FlexiIncludeBlock
                new object[]
                {
                    relativeSourceUri,
                    absoluteSourceUri,
                    null,
                    new Uri(new Uri(absoluteSourceUri), relativeSourceUri).AbsoluteUri
                },
            };
        }

        [Theory]
        [MemberData(nameof(Setup_PopulatesContainingSourceUri_Data))]
        public void Setup_PopulatesContainingSourceUri(ClippingProcessingStage dummyParentClippingProcessingStage,
            string dummyParentAbsoluteSourceUri,
            string dummyGrandparentAbsoluteSourceUri,
            string expectedContainingSourceUri)
        {
            // Arrange
            FlexiIncludeBlock dummyGrandparentFlexiIncludeBlock = dummyGrandparentAbsoluteSourceUri == null ? null : new FlexiIncludeBlock(null, null)
            {
                AbsoluteSourceUri = new Uri(dummyGrandparentAbsoluteSourceUri)
            };
            FlexiIncludeBlock dummyParentFlexiIncludeBlock = dummyParentAbsoluteSourceUri == null ? null : new FlexiIncludeBlock(dummyGrandparentFlexiIncludeBlock, null)
            {
                ClippingProcessingStage = dummyParentClippingProcessingStage,
                AbsoluteSourceUri = new Uri(dummyParentAbsoluteSourceUri)
            };
            var testSubject = new FlexiIncludeBlock(dummyParentFlexiIncludeBlock, null);
            testSubject.Setup(new FlexiIncludeBlockOptions("C:/dummy"), null);

            // Act
            string result = testSubject.ContainingSourceUri;

            // Assert
            Assert.Equal(expectedContainingSourceUri, result);
        }

        public static IEnumerable<object[]> Setup_PopulatesContainingSourceUri_Data()
        {
            return new object[][]
            {
                // Parent ClippingProcessingStage is source
                new object[]
                {
                    ClippingProcessingStage.Source,
                    "C:/parent/absolute/uri",
                    null,
                    new Uri("C:/parent/absolute/uri").AbsoluteUri
                },
                // Parent ClippingProcessingStage is not BeforeContent and Grandparent exists
                new object[]
                {
                    ClippingProcessingStage.BeforeContent,
                    "C:/parent/absolute/uri",
                    "C:/grand/parent/absolute/uri",
                    new Uri("C:/grand/parent/absolute/uri").AbsoluteUri
                },
                // Parent ClippingProcessingStage is not AfterContent and Grandparent exists
                new object[]
                {
                    ClippingProcessingStage.AfterContent,
                    "C:/parent/absolute/uri",
                    "C:/grand/parent/absolute/uri",
                    new Uri("C:/grand/parent/absolute/uri").AbsoluteUri
                },
                // No Parent
                new object[]
                {
                    ClippingProcessingStage.Source,
                    null,
                    null,
                    null
                },
                // Parent ClippingProcessingStage is not source and no grandparent
                new object[]
                {
                    ClippingProcessingStage.AfterContent,
                    "C:/parent/absolute/uri",
                    null,
                    null
                },
            };
        }

        [Fact]
        public void Setup_PopulatesLineNumberInContainingSourceWhenParentFlexiIncludeBlockClippingProcessingStageIsSource()
        {
            // Arrange
            var dummyParentFlexiIncludeBlock = new FlexiIncludeBlock(null, null)
            {
                ClippingProcessingStage = ClippingProcessingStage.Source,
                LastProcessedLineLineNumber = 2, // Arbitrary
                AbsoluteSourceUri = new Uri("C:/dummy")
            };
            var testSubject = new FlexiIncludeBlock(dummyParentFlexiIncludeBlock, null)
            {
                Lines = new StringLineGroup(2)
                        {
                            new StringSlice("dummyLine1"),
                            new StringSlice("dummyLine2")
                        }
            };
            testSubject.Setup(new FlexiIncludeBlockOptions("C:/dummy"), null);

            // Act
            int result = testSubject.LineNumberInContainingSource;

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void Setup_PopulatesLineNumberInContainingSourceWhenParentFlexiIncludeBlockClippingProcessingStageIsNotSource()
        {
            // Arrange
            const int dummyParentLineNumberInContainingSource = 2;
            var dummyGrandparentFlexiIncludeBlock = new FlexiIncludeBlock(null, null)
            {
                AbsoluteSourceUri = new Uri("C:/dummy")
            };
            var dummyParentFlexiIncludeBlock = new FlexiIncludeBlock(dummyGrandparentFlexiIncludeBlock, null)
            {
                ClippingProcessingStage = ClippingProcessingStage.BeforeContent,
                LineNumberInContainingSource = dummyParentLineNumberInContainingSource
            };
            var testSubject = new FlexiIncludeBlock(dummyParentFlexiIncludeBlock, null);
            testSubject.Setup(new FlexiIncludeBlockOptions("C:/dummy"), null);

            // Act
            int result = testSubject.LineNumberInContainingSource;

            // Assert
            Assert.Equal(dummyParentLineNumberInContainingSource, result);
        }

        [Fact]
        public void ToString_ReturnsTheStringRepresentationOfTheInstanceWhenContainingSourceUriIsNull()
        {
            // Arrange
            const int dummyLineIndex = 2;
            var testSubject = new FlexiIncludeBlock(null, null)
            {
                ContainingSourceUri = null,
                Line = dummyLineIndex
            };

            // Act
            string result = testSubject.ToString();

            // Assert
            Assert.Equal($"Source: Root, Line: {dummyLineIndex + 1}", result);
        }

        [Fact]
        public void ToString_ReturnsTheStringRepresentationOfTheInstanceWhenClippingProcessingStageIsSource()
        {
            // Arrange
            const string dummyContainingSourceUri = "dummyContainingSource";
            const int dummyLineNumberInContainingSource = 2;
            var dummyParentFlexiIncludeBlock = new FlexiIncludeBlock(null, null)
            {
                ClippingProcessingStage = ClippingProcessingStage.Source,
            };
            var testSubject = new FlexiIncludeBlock(dummyParentFlexiIncludeBlock, null)
            {
                ContainingSourceUri = dummyContainingSourceUri,
                LineNumberInContainingSource = dummyLineNumberInContainingSource
            };

            // Act
            string result = testSubject.ToString();

            // Assert
            Assert.Equal($"Source URI: {dummyContainingSourceUri}, Line: {dummyLineNumberInContainingSource}", result);
        }

        [Fact]
        public void ToString_ReturnsTheStringRepresentationOfTheInstanceWhenClippingProcessingStageIsNotSource()
        {
            // Arrange
            const string dummyContainingSourceUri = "dummyContainingSourceUri";
            const int dummyLineNumberInContainingSource = 2;
            var dummyParentFlexiIncludeBlock = new FlexiIncludeBlock(null, null)
            {
                ClippingProcessingStage = ClippingProcessingStage.BeforeContent,
            };
            var testSubject = new FlexiIncludeBlock(dummyParentFlexiIncludeBlock, null)
            {
                ContainingSourceUri = dummyContainingSourceUri,
                LineNumberInContainingSource = dummyLineNumberInContainingSource
            };

            // Act
            string result = testSubject.ToString();

            // Assert
            Assert.Equal($"Source URI: {dummyContainingSourceUri}, Line: {dummyLineNumberInContainingSource}, {nameof(ClippingProcessingStage.BeforeContent)}", result);
        }
    }
}