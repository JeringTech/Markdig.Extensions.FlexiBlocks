using Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Moq;
using System;
using Xunit;
using Markdig;
using Markdig.Renderers;
using System.IO;
using Markdig.Extensions.Tables;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiTableBlocks
{
    public class FlexiTableBlocksExtensionUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiTableBlockRendererIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiTableBlocksExtension(
                null,
                _mockRepository.Create<IFlexiOptionsBlockService>().Object,
                new FlexiTableBlocksExtensionOptions()));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiOptionsBlockServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiTableBlocksExtension(
                new FlexiTableBlockRenderer(new FlexiTableBlocksExtensionOptions()),
                null,
                new FlexiTableBlocksExtensionOptions()));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiTableBlocksExtensionOptionsIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiTableBlocksExtension(
                new FlexiTableBlockRenderer(new FlexiTableBlocksExtensionOptions()),
                _mockRepository.Create<IFlexiOptionsBlockService>().Object,
                null));
        }

        [Fact]
        public void SetupRenderers_RemovesAnyExistingHtmlTableRenderer()
        {
            // Arrange
            var dummyPipelineBuilder = new MarkdownPipelineBuilder();
            MarkdownPipeline dummyMarkdownPipeline = dummyPipelineBuilder.Build();
            var dummyFlexiTableBlockRenderer = new FlexiTableBlockRenderer(new FlexiTableBlocksExtensionOptions());
            var testSubject = new ExposedFlexiTableBlocksExtension(dummyFlexiTableBlockRenderer,
                _mockRepository.Create<IFlexiOptionsBlockService>().Object,
                new FlexiTableBlocksExtensionOptions());
            var dummyRenderer = new HtmlRenderer(new StringWriter());
            dummyRenderer.ObjectRenderers.Add(new HtmlTableRenderer());

            // Act
            testSubject.ExposedSetupRenderers(dummyMarkdownPipeline, dummyRenderer);

            // Assert
            Assert.Null(dummyRenderer.ObjectRenderers.Find<HtmlTableRenderer>());
            Assert.Contains(dummyFlexiTableBlockRenderer, dummyRenderer.ObjectRenderers);
        }

        private class ExposedFlexiTableBlocksExtension : FlexiTableBlocksExtension
        {
            public ExposedFlexiTableBlocksExtension(FlexiTableBlockRenderer flexiTableBlockRenderer, IFlexiOptionsBlockService flexiOptionsBlockService, FlexiTableBlocksExtensionOptions extensionOptions) :
                base(flexiTableBlockRenderer, flexiOptionsBlockService, extensionOptions)
            {
            }

            public void ExposedSetupRenderers(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
            {
                SetupRenderers(pipeline, renderer);
            }
        }
    }
}
