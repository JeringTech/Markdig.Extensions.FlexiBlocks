using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using Markdig.Helpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiIncludeBlocks
{
    public class FlexiIncludeBlockUnitTests
    {
        [Theory]
        [MemberData(nameof(ContainingSourceUri_ReturnsTheUriOfTheSourceThatTheFlexiIncludeBlockIsContainedIn_Data))]
        public void ContainingSourceUri_ReturnsTheUriOfTheSourceThatTheFlexiIncludeBlockIsContainedIn(FlexiIncludeBlock dummyParentFlexiIncludeBlock,
            string expectedResult)
        {
            // Arrange
            var testSubject = new FlexiIncludeBlock(null)
            {
                ParentFlexiIncludeBlock = dummyParentFlexiIncludeBlock
            };

            // Act
            string result = testSubject.ContainingSourceUri;

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ContainingSourceUri_ReturnsTheUriOfTheSourceThatTheFlexiIncludeBlockIsContainedIn_Data()
        {
            const string dummyContainingSourceUri = "dummyContainingSourceUri";

            return new object[][]
            {
                // Root
                new object[]
                {
                    null,
                    null,
                },
                // Source stage
                new object[]
                {
                    new FlexiIncludeBlock(null)
                    {
                        ClippingProcessingStage = ClippingProcessingStage.Source,
                        FlexiIncludeBlockOptions = new FlexiIncludeBlockOptions(dummyContainingSourceUri)
                    },
                    dummyContainingSourceUri
                },
                // BeforeContent stage
                new object[]
                {
                    new FlexiIncludeBlock(null)
                    {
                        ClippingProcessingStage = ClippingProcessingStage.BeforeContent,
                        ParentFlexiIncludeBlock = new FlexiIncludeBlock(null)
                        {
                            FlexiIncludeBlockOptions = new FlexiIncludeBlockOptions(dummyContainingSourceUri)
                        }
                    },
                    dummyContainingSourceUri
                },
                // BeforeContent stage in root
                new object[]
                {
                    new FlexiIncludeBlock(null)
                    {
                        ClippingProcessingStage = ClippingProcessingStage.BeforeContent
                    },
                    null
                },
                // AfterContent stage
                new object[]
                {
                    new FlexiIncludeBlock(null)
                    {
                        ClippingProcessingStage = ClippingProcessingStage.AfterContent,
                        ParentFlexiIncludeBlock = new FlexiIncludeBlock(null)
                        {
                            FlexiIncludeBlockOptions = new FlexiIncludeBlockOptions(dummyContainingSourceUri)
                        }
                    },
                    dummyContainingSourceUri
                },
                // AfterContent stage in root
                new object[]
                {
                    new FlexiIncludeBlock(null)
                    {
                        ClippingProcessingStage = ClippingProcessingStage.AfterContent,
                    },
                    null
                }
            };
        }

        [Fact]
        public void LineNumberInContainingSource_ReturnsFlexiIncludeBlocksLineNumberInTheSourceThatContainsIt()
        {
            // Arrange
            const int dummyLastProcessedLineLineNumber = 2;
            var dummyLines = new StringLineGroup(2)
            {
                new StringSlice("dummyLine1"),
                new StringSlice("dummyLine2")
            };
            var dummyParentFlexiIncludeBlock = new FlexiIncludeBlock(null)
            {
                LastProcessedLineLineNumber = dummyLastProcessedLineLineNumber
            };
            var testSubject = new FlexiIncludeBlock(null)
            {
                ParentFlexiIncludeBlock = dummyParentFlexiIncludeBlock,
                Lines = dummyLines
            };

            // Act
            int result = testSubject.LineNumberInContainingSource;

            // Assert
            Assert.Equal(dummyLastProcessedLineLineNumber - dummyLines.Count + 1, result);
        }

        [Fact]
        public void ParentFlexiIncludeBlock_ThrowsArgumentExceptionIfSetMoreThanOnceWithDifferentValues()
        {
            // Arrange
            var dummyInitialParentFlexiIncludeBlock = new FlexiIncludeBlock(null);
            var dummyAlternateParentFlexiIncludeBlock = new FlexiIncludeBlock(null);
            var testSubject = new FlexiIncludeBlock(null) { ParentFlexiIncludeBlock = dummyInitialParentFlexiIncludeBlock };

            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => testSubject.ParentFlexiIncludeBlock = dummyAlternateParentFlexiIncludeBlock);
            Assert.Equal(string.Format(Strings.ArgumentException_PropertyAlreadyHasAValue, nameof(FlexiIncludeBlock.ParentFlexiIncludeBlock)), result.Message);
        }

        [Theory]
        [MemberData(nameof(ToString_ReturnsTheStringRepresentationOfTheInstance_Data))]
        public void ToString_ReturnsTheStringRepresentationOfTheInstance(FlexiIncludeBlock dummyParentFlexiIncludeBlock,
            string expectedResult)
        {
            // Arrange
            var testSubject = new FlexiIncludeBlock(null)
            {
                ParentFlexiIncludeBlock = dummyParentFlexiIncludeBlock
            };

            // Act
            string result = testSubject.ToString();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        // TODO SerializableWrapper does not work with FlexiIncludeBlocks (test discovery hangs), seems like it has something to do with FlexiIncludeBlock not being 
        // JSON serializable/deserializable
        public static IEnumerable<object[]> ToString_ReturnsTheStringRepresentationOfTheInstance_Data()
        {
            const int dummyLastProcessedLineNumber = 2; // Arbitrary
            const string dummySourceUri = "dummySourceUri";

            return new object[][]
            {
                // No Parent
                new object[]
                {
                    null,
                    "Source: Root, Line: 1"
                },
                // Parent's ClippingProcessingStage is Source
                new object[]
                {
                    new FlexiIncludeBlock(null)
                    {
                        FlexiIncludeBlockOptions = new FlexiIncludeBlockOptions(dummySourceUri),
                        LastProcessedLineLineNumber = dummyLastProcessedLineNumber,
                        ClippingProcessingStage = ClippingProcessingStage.Source
                    },
                    $"Source URI: {dummySourceUri}, Line: {dummyLastProcessedLineNumber + 1}"
                },
                // Parent's ClippingProcessingStage is BeforeContent
                new object[]
                {
                    new FlexiIncludeBlock(null)
                    {
                        FlexiIncludeBlockOptions = new FlexiIncludeBlockOptions(dummySourceUri),
                        LastProcessedLineLineNumber = dummyLastProcessedLineNumber,
                        ClippingProcessingStage = ClippingProcessingStage.BeforeContent
                    },
                    $"{ClippingProcessingStage.BeforeContent}, Line: {dummyLastProcessedLineNumber + 1}"
                },
                // Parent's ClippingProcessingStage is AfterContent
                new object[]
                {
                    new FlexiIncludeBlock(null)
                    {
                        FlexiIncludeBlockOptions = new FlexiIncludeBlockOptions(dummySourceUri),
                        LastProcessedLineLineNumber = dummyLastProcessedLineNumber,
                        ClippingProcessingStage = ClippingProcessingStage.AfterContent
                    },
                    $"{ClippingProcessingStage.AfterContent}, Line: {dummyLastProcessedLineNumber + 1}"
                }
            };
        }
    }
}