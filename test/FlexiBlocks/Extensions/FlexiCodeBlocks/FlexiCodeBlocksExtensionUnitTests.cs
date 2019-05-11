using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using Markdig;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Moq;
using System;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiCodeBlocks
{
    public class FlexiCodeBlocksExtensionUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfIndentedFlexiCodeBlockParserIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiCodeBlocksExtension(
                null,
                _mockRepository.Create<ProxyBlockParser<FlexiCodeBlock, ProxyFencedLeafBlock>>().Object,
                _mockRepository.Create<BlockRenderer<FlexiCodeBlock>>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFencedFlexiCodeBlockParserIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiCodeBlocksExtension(
                _mockRepository.Create<ProxyBlockParser<FlexiCodeBlock, ProxyLeafBlock>>().Object,
                null,
                _mockRepository.Create<BlockRenderer<FlexiCodeBlock>>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiCodeBlockRendererIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiCodeBlocksExtension(
                _mockRepository.Create<ProxyBlockParser<FlexiCodeBlock, ProxyLeafBlock>>().Object,
                _mockRepository.Create<ProxyBlockParser<FlexiCodeBlock, ProxyFencedLeafBlock>>().Object,
                null));
        }

        [Fact]
        public void SetupParsers_RemovesCodeBlockBlockParsers()
        {
            // Arrange
            var dummyMarkdownPipelineBuilder = new MarkdownPipelineBuilder();
            Assert.NotNull(dummyMarkdownPipelineBuilder.BlockParsers.Find<FencedCodeBlockParser>()); // Markdig includes a FencedCodeBlockParser by default
            Assert.NotNull(dummyMarkdownPipelineBuilder.BlockParsers.Find<IndentedCodeBlockParser>()); // Markdig includes a FencedCodeBlockParser by default
            ExposedFlexiCodeBlocksExtension testSubject = CreateExposedFlexiCodeBlocksExtension();

            // Act
            testSubject.ExposedSetupParsers(dummyMarkdownPipelineBuilder);

            // Assert
            Assert.Null(dummyMarkdownPipelineBuilder.BlockParsers.Find<FencedCodeBlockParser>());
            Assert.Null(dummyMarkdownPipelineBuilder.BlockParsers.Find<IndentedCodeBlockParser>());
        }

        [Fact]
        public void SetupRenderers_RemovesCodeBlockRenderer()
        {
            // Arrange
            var dummyHtmlRenderer = new HtmlRenderer(_mockRepository.Create<TextWriter>().Object);
            Assert.NotNull(dummyHtmlRenderer.ObjectRenderers.Find<CodeBlockRenderer>()); // Markdig includes a CodeBlockRenderer by default
            ExposedFlexiCodeBlocksExtension testSubject = CreateExposedFlexiCodeBlocksExtension();

            // Act
            testSubject.ExposedSetupRenderers(null, dummyHtmlRenderer);

            // Assert
            Assert.Null(dummyHtmlRenderer.ObjectRenderers.Find<CodeBlockRenderer>());
        }

        private ExposedFlexiCodeBlocksExtension CreateExposedFlexiCodeBlocksExtension(ProxyBlockParser<FlexiCodeBlock, ProxyLeafBlock> indentedFlexiCodeBlockParser = null,
                ProxyBlockParser<FlexiCodeBlock, ProxyFencedLeafBlock> fencedFlexiCodeBlockParser = null,
                BlockRenderer<FlexiCodeBlock> flexiCodeBlockRenderer = null)
        {
            return new ExposedFlexiCodeBlocksExtension(indentedFlexiCodeBlockParser ?? _mockRepository.Create<ProxyBlockParser<FlexiCodeBlock, ProxyLeafBlock>>().Object,
                fencedFlexiCodeBlockParser ?? _mockRepository.Create<ProxyBlockParser<FlexiCodeBlock, ProxyFencedLeafBlock>>().Object,
                flexiCodeBlockRenderer ?? _mockRepository.Create<BlockRenderer<FlexiCodeBlock>>().Object);
        }

        private class ExposedFlexiCodeBlocksExtension : FlexiCodeBlocksExtension
        {
            public ExposedFlexiCodeBlocksExtension(ProxyBlockParser<FlexiCodeBlock, ProxyLeafBlock> indentedFlexiCodeBlockParser,
                ProxyBlockParser<FlexiCodeBlock, ProxyFencedLeafBlock> fencedFlexiCodeBlockParser,
                BlockRenderer<FlexiCodeBlock> flexiCodeBlockRenderer) : base(indentedFlexiCodeBlockParser, fencedFlexiCodeBlockParser, flexiCodeBlockRenderer)
            {
            }

            public void ExposedSetupParsers(MarkdownPipelineBuilder markdownPipelineBuilder)
            {
                base.SetupParsers(markdownPipelineBuilder);
            }

            public void ExposedSetupRenderers(MarkdownPipeline markdownPipeline, IMarkdownRenderer markdownRenderer)
            {
                base.SetupRenderers(markdownPipeline, markdownRenderer);
            }
        }
    }
}
