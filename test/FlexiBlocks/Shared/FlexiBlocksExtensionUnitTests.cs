using Markdig;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Syntax;
using Moq;
using Moq.Protected;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.Shared
{
    public class FlexiBlocksExtensionUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Setup_ThrowsArgumentNullExceptionIfPipelineBuilderIsNull()
        {
            // Arrange
            Mock<FlexiBlocksExtension> mockTestSubject = _mockRepository.Create<FlexiBlocksExtension>();
            mockTestSubject.CallBase = true;

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => mockTestSubject.Object.Setup(null));
        }

        [Fact]
        public void Setup_ThrowsArgumentNullExceptionIfPipelineIsNull()
        {
            // Arrange
            Mock<FlexiBlocksExtension> mockTestSubject = _mockRepository.Create<FlexiBlocksExtension>();
            mockTestSubject.CallBase = true;

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => mockTestSubject.Object.Setup(null, _mockRepository.Create<IMarkdownRenderer>().Object));
        }

        [Fact]
        public void Setup_ThrowsArgumentNullExceptionIfRendererIsNull()
        {
            // Arrange
            Mock<FlexiBlocksExtension> mockTestSubject = _mockRepository.Create<FlexiBlocksExtension>();
            mockTestSubject.CallBase = true;

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => mockTestSubject.Object.Setup(new MarkdownPipelineBuilder().Build(), null));
        }

        [Fact]
        public void OnClosed_ThrowsArgumentNullExceptionIfProcessorIsNull()
        {
            // Arrange
            Mock<ExposedFlexiBlocksExtension> mockTestSubject = _mockRepository.Create<ExposedFlexiBlocksExtension>();
            mockTestSubject.CallBase = true;

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => mockTestSubject.Object.ExposedOnClosed(null, _mockRepository.Create<Block>(null).Object));
        }

        [Fact]
        public void OnClosed_ThrowsArgumentNullExceptionIfBlockIsNull()
        {
            // Arrange
            Mock<ExposedFlexiBlocksExtension> mockTestSubject = _mockRepository.Create<ExposedFlexiBlocksExtension>();
            mockTestSubject.CallBase = true;

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => mockTestSubject.Object.ExposedOnClosed(MarkdigTypesFactory.CreateBlockProcessor(), null));
        }

        [Fact]
        public void OnClosed_DoesNotInterfereWithFlexiBlocksExceptionsWithBlockContext()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyBlock = new DummyBlock(null);
            var dummyFlexiBlocksException = new FlexiBlocksException(new DummyBlock(null));
            Mock<ExposedFlexiBlocksExtension> mockTestSubject = _mockRepository.Create<ExposedFlexiBlocksExtension>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("OnFlexiBlockClosed", dummyBlockProcessor, dummyBlock).Throws(dummyFlexiBlocksException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.ExposedOnClosed(dummyBlockProcessor, dummyBlock));
            _mockRepository.VerifyAll();
            Assert.Same(dummyFlexiBlocksException, result);
            Assert.Null(result.InnerException);
        }

        [Fact]
        public void OnClosed_WrapsNonFlexiBlocksExceptionsThrownByOnFlexiBlockClosedInFlexiBlocksExceptions()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyBlock = new DummyBlock(null);
            var dummyException = new ArgumentException(); // Arbitrary type
            Mock<ExposedFlexiBlocksExtension> mockTestSubject = _mockRepository.Create<ExposedFlexiBlocksExtension>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("OnFlexiBlockClosed", dummyBlockProcessor, dummyBlock).Throws(dummyException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.ExposedOnClosed(dummyBlockProcessor, dummyBlock));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.FlexiBlocksException_InvalidFlexiBlock,
                    nameof(DummyBlock),
                    dummyBlock.Line + 1,
                    dummyBlock.Column,
                    Strings.FlexiBlocksException_ExceptionOccurredWhileProcessingABlock),
                result.Message,
                ignoreLineEndingDifferences: true);
            Assert.Same(dummyException, result.InnerException);
        }

        public class ExposedFlexiBlocksExtension : FlexiBlocksExtension
        {
            public override void SetupParsers(MarkdownPipelineBuilder pipelineBuilder)
            {
            }

            public void ExposedOnClosed(BlockProcessor processor, Block block)
            {
                OnClosed(processor, block);
            }
        }

        public class DummyBlock : Block
        {
            public DummyBlock(BlockParser parser) : base(parser)
            {
            }
        }
    }
}
