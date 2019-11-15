using Markdig;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Syntax;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests
{
    public class BlockExtensionUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Setup_Parsers_ThrowsArgumentNullExceptionIfPipelineBuilderIsNull()
        {
            // Arrange
            Mock<BlockExtension<Block>> mockTestSubject = CreateMockBlockExtension();
            mockTestSubject.CallBase = true;

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => mockTestSubject.Object.Setup(null));
        }

        [Theory]
        [MemberData(nameof(Setup_Parsers_DoesNothingIfExtensionHasNoBlockParsers_Data))]
        public void Setup_Parsers_DoesNothingIfExtensionHasNoBlockParsers(BlockParser[] dummyBlockParsers)
        {
            // Arrange
            var dummyMarkdownPipelineBuilder = new MarkdownPipelineBuilder();
            Mock<BlockExtension<Block>> mockTestSubject = CreateMockBlockExtension(null, dummyBlockParsers);
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("SetupParsers", dummyMarkdownPipelineBuilder);
            int expectedNumBlockParsers = dummyMarkdownPipelineBuilder.BlockParsers.Count;

            // Act
            mockTestSubject.Object.Setup(dummyMarkdownPipelineBuilder);

            // Assert
            Assert.Equal(expectedNumBlockParsers, dummyMarkdownPipelineBuilder.BlockParsers.Count);
        }

        public static IEnumerable<object[]> Setup_Parsers_DoesNothingIfExtensionHasNoBlockParsers_Data()
        {
            return new object[][]{
                new object[]{null},
                new object[]{new BlockParser[0]}
            };
        }

        [Fact]
        public void Setup_Parsers_DoesNotInsertNullIntoMarkdownPipelineBuilder()
        {
            // Arrange
            var dummyMarkdownPipelineBuilder = new MarkdownPipelineBuilder();
            Mock<BlockExtension<Block>> mockTestSubject = CreateMockBlockExtension();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("SetupParsers", dummyMarkdownPipelineBuilder);
            int expectedNumBlockParsers = dummyMarkdownPipelineBuilder.BlockParsers.Count;

            // Act
            mockTestSubject.Object.Setup(dummyMarkdownPipelineBuilder);

            // Assert
            Assert.Equal(expectedNumBlockParsers, dummyMarkdownPipelineBuilder.BlockParsers.Count);
        }

        [Fact]
        public void Setup_Parsers_InsertsExtensionBlockParsers()
        {
            // Arrange
            Mock<BlockParser> dummyGlobalBlockParser = _mockRepository.Create<BlockParser>();
            Mock<BlockParser> dummyNonGlobalBlockParser = _mockRepository.Create<BlockParser>();
            dummyNonGlobalBlockParser.Object.OpeningCharacters = new char[] { 'a' }; // Arbitrary opening character, can't mock because OpeningCharacters isn't virtual
            var dummyMarkdownPipelineBuilder = new MarkdownPipelineBuilder();
            Mock<BlockExtension<Block>> mockTestSubject = CreateMockBlockExtension(null, dummyGlobalBlockParser.Object, dummyNonGlobalBlockParser.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("SetupParsers", dummyMarkdownPipelineBuilder);

            // Act
            mockTestSubject.Object.Setup(dummyMarkdownPipelineBuilder);

            // Assert
            OrderedList<BlockParser> blockParsers = dummyMarkdownPipelineBuilder.BlockParsers;
            Assert.Contains(dummyNonGlobalBlockParser.Object, blockParsers);
            Assert.Contains(dummyGlobalBlockParser.Object, blockParsers);
            ParagraphBlockParser paragraphBlockParser = blockParsers.Find<ParagraphBlockParser>();
            Assert.True(blockParsers.IndexOf(dummyGlobalBlockParser.Object) < blockParsers.IndexOf(paragraphBlockParser)); // Global parsers must be inserted before catch all ParagraphBlockParser
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Setup_Renderers_ThrowsArgumentNullExceptionIfRendererIsNull()
        {
            // Arrange
            Mock<BlockExtension<Block>> mockTestSubject = CreateMockBlockExtension();
            mockTestSubject.CallBase = true;

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => mockTestSubject.Object.Setup(new MarkdownPipelineBuilder().Build(), null));
        }

        [Fact]
        public void Setup_Renderers_ThrowsArgumentNullExceptionIfPipelineIsNull()
        {
            // Arrange
            Mock<BlockExtension<Block>> mockTestSubject = CreateMockBlockExtension();
            mockTestSubject.CallBase = true;

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => mockTestSubject.Object.Setup(null, _mockRepository.Create<IMarkdownRenderer>().Object));
        }

        [Fact]
        public void Setup_Renderers_InsertsExtensionBlockRendererIntoHtmlRendererIfItDoesNotContainABlockRendererOfTheSameType()
        {
            // Arrange
            MarkdownPipeline dummyMarkdownPipeline = new MarkdownPipelineBuilder().Build();
            Mock<BlockRenderer<Block>> dummyBlockRenderer = _mockRepository.Create<BlockRenderer<Block>>();
            var dummyHtmlRenderer = new HtmlRenderer(new StringWriter());
            Mock<BlockExtension<Block>> mockTestSubject = CreateMockBlockExtension(blockRenderer: dummyBlockRenderer.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("SetupRenderers", dummyMarkdownPipeline, dummyHtmlRenderer);

            // Act
            mockTestSubject.Object.Setup(dummyMarkdownPipeline, dummyHtmlRenderer);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Contains(dummyBlockRenderer.Object, dummyHtmlRenderer.ObjectRenderers);
        }

        [Fact]
        public void Setup_Renderers_DoesNothingIfHtmlRendererContainsABlockRendererOfSameType()
        {
            // Arrange
            MarkdownPipeline dummyMarkdownPipeline = new MarkdownPipelineBuilder().Build();
            Mock<BlockRenderer<Block>> dummyNewBlockRenderer = _mockRepository.Create<BlockRenderer<Block>>();
            var dummyHtmlRenderer = new HtmlRenderer(new StringWriter());
            Mock<BlockExtension<Block>> mockTestSubject = CreateMockBlockExtension(blockRenderer: dummyNewBlockRenderer.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("SetupRenderers", dummyMarkdownPipeline, dummyHtmlRenderer);
            Mock<BlockRenderer<Block>> dummyExistingBlockRenderer = _mockRepository.Create<BlockRenderer<Block>>();
            dummyHtmlRenderer.ObjectRenderers.Add(dummyExistingBlockRenderer.Object);

            // Act
            mockTestSubject.Object.Setup(dummyMarkdownPipeline, dummyHtmlRenderer);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Contains(dummyExistingBlockRenderer.Object, dummyHtmlRenderer.ObjectRenderers);
            Assert.DoesNotContain(dummyNewBlockRenderer.Object, dummyHtmlRenderer.ObjectRenderers);
        }

        [Fact]
        public void Setup_Renderers_DoesNothingIfBlockRendererIsNull()
        {
            // Arrange
            MarkdownPipeline dummyMarkdownPipeline = new MarkdownPipelineBuilder().Build();
            var dummyHtmlRenderer = new HtmlRenderer(new StringWriter());
            Mock<BlockExtension<Block>> mockTestSubject = CreateMockBlockExtension();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("SetupRenderers", dummyMarkdownPipeline, dummyHtmlRenderer);
            int expectedNumObjectRenderers = dummyHtmlRenderer.ObjectRenderers.Count;

            // Act
            mockTestSubject.Object.Setup(dummyMarkdownPipeline, dummyHtmlRenderer);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(expectedNumObjectRenderers, dummyHtmlRenderer.ObjectRenderers.Count);
        }

        [Fact]
        public void Setup_Renderers_DoesNothingIfMarkdownRendererIsNotAHtmlRenderer()
        {
            // Arrange
            MarkdownPipeline dummyMarkdownPipeline = new MarkdownPipelineBuilder().Build();
            Mock<BlockRenderer<Block>> dummyBlockRenderer = _mockRepository.Create<BlockRenderer<Block>>();
            Mock<IMarkdownRenderer> dummyMarkdownRenderer = _mockRepository.Create<IMarkdownRenderer>();
            Mock<BlockExtension<Block>> mockTestSubject = CreateMockBlockExtension(blockRenderer: dummyBlockRenderer.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("SetupRenderers", dummyMarkdownPipeline, dummyMarkdownRenderer.Object);

            // Act
            mockTestSubject.Object.Setup(dummyMarkdownPipeline, dummyMarkdownRenderer.Object);

            // Assert
            _mockRepository.VerifyAll();
            dummyMarkdownRenderer.Verify(m => m.ObjectRenderers, Times.Never);
        }

        [Fact]
        public void OnClosed_ThrowsArgumentNullExceptionIfProcessorIsNull()
        {
            // Arrange
            ExposedBlockExtension testSubject = CreatedExposedBlockExtension();

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => testSubject.ExposedOnClosed(null, _mockRepository.Create<Block>(null).Object));
        }

        [Fact]
        public void OnClosed_ThrowsArgumentNullExceptionIfBlockIsNull()
        {
            // Arrange
            ExposedBlockExtension testSubject = CreatedExposedBlockExtension();

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => testSubject.ExposedOnClosed(MarkdigTypesFactory.CreateBlockProcessor(), null));
        }

        [Fact]
        public void OnClosed_DoesNotInterfereWithBlockExceptionsWithBlockContext()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<Block> mockBlock = _mockRepository.Create<Block>(null);
            var dummyBlockException = new BlockException(mockBlock.Object);
            Mock<ExposedBlockExtension> mockTestSubject = CreateMockExposedBlockExtension();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("OnBlockClosed", dummyBlockProcessor, mockBlock.Object).Throws(dummyBlockException);

            // Act and assert
            BlockException result = Assert.Throws<BlockException>(() => mockTestSubject.Object.ExposedOnClosed(dummyBlockProcessor, mockBlock.Object));
            _mockRepository.VerifyAll();
            Assert.Same(dummyBlockException, result);
            Assert.Null(result.InnerException);
        }

        [Theory]
        [MemberData(nameof(OnClosed_WrapsNonBlockExceptionsAndBlockExceptionsWithoutBlockContextInBlockExceptionsWithBlockContext_Data))]
        public void OnClosed_WrapsNonBlockExceptionsAndBlockExceptionsWithoutBlockContextInBlockExceptionsWithBlockContext(Exception dummyException)
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<Block> mockBlock = _mockRepository.Create<Block>(null);
            Mock<ExposedBlockExtension> mockTestSubject = CreateMockExposedBlockExtension();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("OnBlockClosed", dummyBlockProcessor, mockBlock.Object).Throws(dummyException);

            // Act and assert
            BlockException result = Assert.Throws<BlockException>(() => mockTestSubject.Object.ExposedOnClosed(dummyBlockProcessor, mockBlock.Object));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock,
                    mockBlock.Object.GetType().Name,
                    mockBlock.Object.Line + 1,
                    mockBlock.Object.Column,
                    Strings.BlockException_BlockException_ExceptionOccurredWhileProcessingBlock),
                result.Message,
                ignoreLineEndingDifferences: true);
            Assert.Same(dummyException, result.InnerException);
        }

        public static IEnumerable<object[]> OnClosed_WrapsNonBlockExceptionsAndBlockExceptionsWithoutBlockContextInBlockExceptionsWithBlockContext_Data()
        {
            return new object[][]
            {
                // Non BlockException
                new object[]{ new ArgumentException()},
                // BlockException without block context
                new object[]{ new BlockException()},
            };
        }

        public class ExposedBlockExtension : BlockExtension<Block>
        {
            public ExposedBlockExtension(BlockRenderer<Block> blockRenderer, params BlockParser[] blockParsers) : base(blockRenderer, blockParsers)
            {
            }

            public void ExposedOnClosed(BlockProcessor processor, Block block)
            {
                OnClosed(processor, block);
            }

            public void ExposedSetupParsers(MarkdownPipelineBuilder markdownPipelineBuilder)
            {
                SetupParsers(markdownPipelineBuilder);
            }

            public void ExposedSetupRenderers(MarkdownPipeline markdownPipeline, IMarkdownRenderer markdownRenderer)
            {
                SetupRenderers(markdownPipeline, markdownRenderer);
            }
        }

        private Mock<ExposedBlockExtension> CreateMockExposedBlockExtension(BlockRenderer<Block> blockRenderer = null, params BlockParser[] blockParsers)
        {
            return _mockRepository.Create<ExposedBlockExtension>(blockRenderer, blockParsers);
        }

        private ExposedBlockExtension CreatedExposedBlockExtension(BlockRenderer<Block> blockRenderer = null, params BlockParser[] blockParsers)
        {
            return new ExposedBlockExtension(blockRenderer, blockParsers);
        }

        private Mock<BlockExtension<Block>> CreateMockBlockExtension(BlockRenderer<Block> blockRenderer = null, params BlockParser[] blockParsers)
        {
            return _mockRepository.Create<BlockExtension<Block>>(blockRenderer, blockParsers);
        }
    }
}
