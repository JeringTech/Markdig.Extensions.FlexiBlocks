using Markdig;
using Markdig.Parsers;
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
        public void OnClosed_DoesNotInterfereWithFlexiBlocksExceptionsThrownByOnFlexiBlockClosed()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyBlock = new DummyBlock(null);
            var dummyFlexiBlocksException = new FlexiBlocksException();
            Mock<FlexiBlocksExtensionExposer> mockTestSubject = _mockRepository.Create<FlexiBlocksExtensionExposer>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("OnFlexiBlockClosed", dummyBlockProcessor, dummyBlock).Throws(dummyFlexiBlocksException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.OnClosed(dummyBlockProcessor, dummyBlock));
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
            Mock<FlexiBlocksExtensionExposer> mockTestSubject = _mockRepository.Create<FlexiBlocksExtensionExposer>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("OnFlexiBlockClosed", dummyBlockProcessor, dummyBlock).Throws(dummyException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.OnClosed(dummyBlockProcessor, dummyBlock));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.FlexiBlocksException_InvalidFlexiBlock,
                    $"Flexi{nameof(DummyBlock)}",
                    dummyBlock.Line + 1,
                    dummyBlock.Column,
                    Strings.FlexiBlocksException_ExceptionOccurredWhileProcessingABlock),
                result.Message,
                ignoreLineEndingDifferences: true);
            Assert.Same(dummyException, result.InnerException);
        }

        public class FlexiBlocksExtensionExposer : FlexiBlocksExtension
        {
            public override void Setup(MarkdownPipelineBuilder pipelineBuilder)
            {
            }


            public void OnClosed(BlockProcessor processor, Block block)
            {
                base.OnClosed(processor, block);
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
