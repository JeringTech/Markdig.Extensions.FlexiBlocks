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
            Mock<Block> mockBlock = _mockRepository.Create<Block>(null);
            var dummyFlexiBlocksException = new FlexiBlocksException(mockBlock.Object);
            Mock<ExposedFlexiBlocksExtension> mockTestSubject = _mockRepository.Create<ExposedFlexiBlocksExtension>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("OnFlexiBlockClosed", dummyBlockProcessor, mockBlock.Object).Throws(dummyFlexiBlocksException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.ExposedOnClosed(dummyBlockProcessor, mockBlock.Object));
            _mockRepository.VerifyAll();
            Assert.Same(dummyFlexiBlocksException, result);
            Assert.Null(result.InnerException);
        }

        [Fact]
        public void OnClosed_WrapsNonFlexiBlocksExceptionsThrownByOnFlexiBlockClosedInFlexiBlocksExceptions()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<Block> mockBlock = _mockRepository.Create<Block>(null);
            var dummyException = new ArgumentException(); // Arbitrary type
            Mock<ExposedFlexiBlocksExtension> mockTestSubject = _mockRepository.Create<ExposedFlexiBlocksExtension>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("OnFlexiBlockClosed", dummyBlockProcessor, mockBlock.Object).Throws(dummyException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.ExposedOnClosed(dummyBlockProcessor, mockBlock.Object));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.FlexiBlocksException_FlexiBlocksException_InvalidFlexiBlock,
                    mockBlock.Object.GetType().Name,
                    mockBlock.Object.Line + 1,
                    mockBlock.Object.Column,
                    Strings.FlexiBlocksException_FlexiBlocksException_ExceptionOccurredWhileProcessingABlock),
                result.Message,
                ignoreLineEndingDifferences: true);
            Assert.Same(dummyException, result.InnerException);
        }

        public class ExposedFlexiBlocksExtension : FlexiBlocksExtension
        {
            protected override void SetupParsers(MarkdownPipelineBuilder pipelineBuilder)
            {
            }

            public void ExposedOnClosed(BlockProcessor processor, Block block)
            {
                OnClosed(processor, block);
            }
        }
    }
}
