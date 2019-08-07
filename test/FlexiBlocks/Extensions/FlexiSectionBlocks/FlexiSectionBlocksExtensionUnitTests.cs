using Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks;
using Markdig;
using Markdig.Parsers;
using Moq;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiSectionBlocks
{
    public class FlexiSectionBlocksExtensionUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiSectionBlockParserIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiSectionBlocksExtension(
                null,
                _mockRepository.Create<BlockRenderer<FlexiSectionBlock>>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiSectionBlockRendererIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiSectionBlocksExtension(
                _mockRepository.Create<BlockParser<FlexiSectionBlock>>().Object,
                null));
        }

        [Fact]
        public void SetupParsers_RemovesHeadingBlockParser()
        {
            // Arrange
            var dummyMarkdownPipelineBuilder = new MarkdownPipelineBuilder();
            Assert.NotNull(dummyMarkdownPipelineBuilder.BlockParsers.Find<HeadingBlockParser>()); // Markdig includes HeadingBlockParser by default
            ExposedFlexiSectionBlocksExtension testSubject = CreateExposedFlexiSectionBlocksExtension();

            // Act
            testSubject.ExposedSetupParsers(dummyMarkdownPipelineBuilder);

            // Assert
            Assert.Null(dummyMarkdownPipelineBuilder.BlockParsers.Find<HeadingBlockParser>());
        }

        private ExposedFlexiSectionBlocksExtension CreateExposedFlexiSectionBlocksExtension(BlockParser<FlexiSectionBlock> flexiSectionBlockParser = null,
                BlockRenderer<FlexiSectionBlock> flexiSectionBlockRenderer = null)
        {
            return new ExposedFlexiSectionBlocksExtension(flexiSectionBlockParser ?? _mockRepository.Create<BlockParser<FlexiSectionBlock>>().Object,
                flexiSectionBlockRenderer ?? _mockRepository.Create<BlockRenderer<FlexiSectionBlock>>().Object);
        }

        private class ExposedFlexiSectionBlocksExtension : FlexiSectionBlocksExtension
        {
            public ExposedFlexiSectionBlocksExtension(BlockParser<FlexiSectionBlock> flexiSectionBlockParser,
                BlockRenderer<FlexiSectionBlock> flexiSectionBlockRenderer) : base(flexiSectionBlockParser, flexiSectionBlockRenderer)
            {
            }

            public void ExposedSetupParsers(MarkdownPipelineBuilder markdownPipelineBuilder)
            {
                base.SetupParsers(markdownPipelineBuilder);
            }
        }
    }
}
