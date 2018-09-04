using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.Shared
{
    public class FlexiBlockParserUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void TryOpen_DoesNotInterfereWithFlexiBlocksExceptionsThrownByTryOpenFlexiBlock()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyFlexiBlocksException = new FlexiBlocksException();
            Mock<FlexiBlockParser> mockTestSubject = _mockRepository.Create<FlexiBlockParser>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(f => f.TryOpenFlexiBlock(dummyBlockProcessor)).Throws(dummyFlexiBlocksException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.TryOpen(dummyBlockProcessor));
            _mockRepository.VerifyAll();
            Assert.Same(dummyFlexiBlocksException, result);
            Assert.Null(result.InnerException);
        }

        [Fact]
        public void TryOpen_WrapsAnyNonFlexiBlocksExceptionThrownByTryOpenFlexiBlockInAFlexiBlocksExceptionWithAnInvalidBlockMessageIfAFlexiBlockIsCreated()
        {
            // Arrange
            var dummyBlock = new DummyBlock(null);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.NewBlocks.Push(dummyBlock);
            var dummyException = new ArgumentException(); // Arbitrary type
            Mock<FlexiBlockParser> mockTestSubject = _mockRepository.Create<FlexiBlockParser>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(f => f.TryOpenFlexiBlock(dummyBlockProcessor)).Throws(dummyException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.TryOpen(dummyBlockProcessor));
            _mockRepository.VerifyAll();
            Assert.Equal(@"The DummyBlock starting at line ""1"", column ""0"", is invalid:
An unexpected exception occurred. Refer to the inner exception for more details.",
                result.Message, ignoreLineEndingDifferences: true);
            Assert.Same(dummyException, result.InnerException);
        }

        [Fact]
        public void TryOpen_WrapsAnyNonFlexiBlocksExceptionThrownByTryOpenFlexiBlockInAFlexiBlocksExceptionWithAnInvalidMarkdownMessageIfNoFlexiBlockIsCreated()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyException = new ArgumentException(); // Arbitrary type
            Mock<FlexiBlockParser> mockTestSubject = _mockRepository.Create<FlexiBlockParser>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(f => f.TryOpenFlexiBlock(dummyBlockProcessor)).Throws(dummyException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.TryOpen(dummyBlockProcessor));
            _mockRepository.VerifyAll();
            Assert.Equal(@"The markdown at line ""1"", column ""0"" is invalid:
An unexpected exception occurred in ""FlexiBlockParserProxy"" while attempting to open a block. Refer to the inner exception for more details.",
                result.Message, ignoreLineEndingDifferences: true);
            Assert.Same(dummyException, result.InnerException);
        }

        [Fact]
        public void TryContinue_DoesNotInterfereWithFlexiBlocksExceptionsThrownByTryContinueFlexiBlock()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyBlock = new DummyBlock(null);
            var dummyFlexiBlocksException = new FlexiBlocksException();
            Mock<FlexiBlockParser> mockTestSubject = _mockRepository.Create<FlexiBlockParser>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(f => f.TryContinueFlexiBlock(dummyBlockProcessor, dummyBlock)).Throws(dummyFlexiBlocksException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.TryContinue(dummyBlockProcessor, dummyBlock));
            _mockRepository.VerifyAll();
            Assert.Same(dummyFlexiBlocksException, result);
            Assert.Null(result.InnerException);
        }

        [Fact]
        public void TryContinue_WrapsNonFlexiBlocksExceptionsThrownByTryContinueFlexiBlockInFlexiBlocksExceptions()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyBlock = new DummyBlock(null);
            var dummyException = new ArgumentException(); // Arbitrary type
            Mock<FlexiBlockParser> mockTestSubject = _mockRepository.Create<FlexiBlockParser>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(f => f.TryContinueFlexiBlock(dummyBlockProcessor, dummyBlock)).Throws(dummyException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.TryContinue(dummyBlockProcessor, dummyBlock));
            _mockRepository.VerifyAll();
            Assert.Equal(@"The DummyBlock starting at line ""1"", column ""0"", is invalid:
An unexpected exception occurred. Refer to the inner exception for more details.", 
                result.Message, ignoreLineEndingDifferences: true);
            Assert.Same(dummyException, result.InnerException);
        }

        [Fact]
        public void Close_DoesNotInterfereWithFlexiBlocksExceptionsThrownByCloseFlexiBlock()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyBlock = new DummyBlock(null);
            var dummyFlexiBlocksException = new FlexiBlocksException();
            Mock<FlexiBlockParser> mockTestSubject = _mockRepository.Create<FlexiBlockParser>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(f => f.CloseFlexiBlock(dummyBlockProcessor, dummyBlock)).Throws(dummyFlexiBlocksException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.Close(dummyBlockProcessor, dummyBlock));
            _mockRepository.VerifyAll();
            Assert.Same(dummyFlexiBlocksException, result);
            Assert.Null(result.InnerException);
        }

        [Fact]
        public void Close_WrapsNonFlexiBlocksExceptionsThrownByTryContinueFlexiBlockInFlexiBlocksExceptions()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyBlock = new DummyBlock(null);
            var dummyException = new ArgumentException(); // Arbitrary type
            Mock<FlexiBlockParser> mockTestSubject = _mockRepository.Create<FlexiBlockParser>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(f => f.CloseFlexiBlock(dummyBlockProcessor, dummyBlock)).Throws(dummyException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.Close(dummyBlockProcessor, dummyBlock));
            _mockRepository.VerifyAll();
            Assert.Equal(@"The DummyBlock starting at line ""1"", column ""0"", is invalid:
An unexpected exception occurred. Refer to the inner exception for more details.",
                result.Message, ignoreLineEndingDifferences: true);
            Assert.Same(dummyException, result.InnerException);
        }

        private class DummyBlock : Block
        {
            public DummyBlock(BlockParser parser) : base(parser)
            {
            }
        }
    }
}
