using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.Shared
{
    public class FlexiBlockParserUnitTests
    {
        private static readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void TryOpen_ThrowsArgumentNullExceptionIfProcessorIsNull()
        {
            // Arrange
            Mock<FlexiBlockParser> mockTestSubject = _mockRepository.Create<FlexiBlockParser>();
            mockTestSubject.CallBase = true;

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => mockTestSubject.Object.TryOpen(null));
        }

        [Fact]
        public void TryOpen_DoesNotInterfereWithFlexiBlocksExceptionsWithBlockContextIfAFlexiBlockIsCreated()
        {
            // Arrange
            Mock<Block> mockBlock = _mockRepository.Create<Block>(null);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.NewBlocks.Push(mockBlock.Object);
            var dummyFlexiBlocksException = new FlexiBlocksException(mockBlock.Object);
            Mock<FlexiBlockParser> mockTestSubject = _mockRepository.Create<FlexiBlockParser>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("TryOpenFlexiBlock", dummyBlockProcessor).Throws(dummyFlexiBlocksException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.TryOpen(dummyBlockProcessor));
            _mockRepository.VerifyAll();
            Assert.Same(dummyFlexiBlocksException, result);
            Assert.Null(result.InnerException);
        }

        [Theory]
        [MemberData(nameof(TryOpen_DoesNotInterfereWithFlexiBlocksExceptionsWithBlockOrLineContextIfNoFlexiBlockIsCreated_Data))]
        public void TryOpen_DoesNotInterfereWithFlexiBlocksExceptionsWithBlockOrLineContextIfNoFlexiBlockIsCreated(SerializableWrapper<FlexiBlocksException> dummyExceptionWrapper)
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<FlexiBlockParser> mockTestSubject = _mockRepository.Create<FlexiBlockParser>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("TryOpenFlexiBlock", dummyBlockProcessor).Throws(dummyExceptionWrapper.Value);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.TryOpen(dummyBlockProcessor));
            _mockRepository.VerifyAll();
            Assert.Same(dummyExceptionWrapper.Value, result);
            Assert.Null(result.InnerException);
        }

        public static IEnumerable<object[]> TryOpen_DoesNotInterfereWithFlexiBlocksExceptionsWithBlockOrLineContextIfNoFlexiBlockIsCreated_Data()
        {
            return new object[][]
            {
                // FlexiBlocksException with block context
                new object[]
                {
                    new SerializableWrapper<FlexiBlocksException>(new FlexiBlocksException(_mockRepository.Create<Block>(null).Object))
                },
                // FlexiBlocksException with line context
                new object[]
                {
                    new SerializableWrapper<FlexiBlocksException>(new FlexiBlocksException(1, 0)) // Arbitrary line and column
                }
            };
        }

        [Fact]
        public void TryOpen_WrapsExceptionsInAFlexiBlocksExceptionWithAnInvalidBlockMessageIfAFlexiBlockIsCreated()
        {
            // Arrange
            Mock<Block> mockBlock = _mockRepository.Create<Block>(null);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.NewBlocks.Push(mockBlock.Object);
            var dummyException = new ArgumentException(); // Arbitrary type
            Mock<FlexiBlockParser> mockTestSubject = _mockRepository.Create<FlexiBlockParser>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("TryOpenFlexiBlock", dummyBlockProcessor).Throws(dummyException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.TryOpen(dummyBlockProcessor));
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

        [Fact]
        public void TryOpen_WrapsExceptionInAFlexiBlocksExceptionWithAnInvalidMarkdownMessageIfNoFlexiBlockIsCreated()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyException = new ArgumentException(); // Arbitrary type
            Mock<FlexiBlockParser> mockTestSubject = _mockRepository.Create<FlexiBlockParser>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("TryOpenFlexiBlock", dummyBlockProcessor).Throws(dummyException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.TryOpen(dummyBlockProcessor));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.FlexiBlocksException_FlexiBlocksException_InvalidMarkdown,
                    dummyBlockProcessor.LineIndex + 1,
                    dummyBlockProcessor.Column,
                    string.Format(Strings.FlexiBlocksException_FlexiBlockParser_ExceptionOccurredWhileAttemptingToOpenBlock, "FlexiBlockParserProxy")),
                result.Message,
                ignoreLineEndingDifferences: true);
            Assert.Same(dummyException, result.InnerException);
        }

        [Fact]
        public void TryContinue_ThrowsArgumentNullExceptionIfProcessorIsNull()
        {
            // Arrange
            Mock<FlexiBlockParser> mockTestSubject = _mockRepository.Create<FlexiBlockParser>();
            mockTestSubject.CallBase = true;

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => mockTestSubject.Object.TryContinue(null, _mockRepository.Create<Block>(null).Object));
        }

        [Fact]
        public void TryContinue_ThrowsArgumentNullExceptionIfBlockIsNull()
        {
            // Arrange
            Mock<FlexiBlockParser> mockTestSubject = _mockRepository.Create<FlexiBlockParser>();
            mockTestSubject.CallBase = true;

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => mockTestSubject.Object.TryContinue(MarkdigTypesFactory.CreateBlockProcessor(), null));
        }

        [Fact]
        public void TryContinue_DoesNotInterfereWithFlexiBlocksExceptionsWithBlockContext()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<Block> mockBlock = _mockRepository.Create<Block>(null);
            var dummyFlexiBlocksException = new FlexiBlocksException(mockBlock.Object);
            Mock<FlexiBlockParser> mockTestSubject = _mockRepository.Create<FlexiBlockParser>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("TryContinueFlexiBlock", dummyBlockProcessor, mockBlock.Object).Throws(dummyFlexiBlocksException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.TryContinue(dummyBlockProcessor, mockBlock.Object));
            _mockRepository.VerifyAll();
            Assert.Same(dummyFlexiBlocksException, result);
            Assert.Null(result.InnerException);
        }

        [Fact]
        public void TryContinue_WrapsExceptionsInFlexiBlocksExceptions()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<Block> mockBlock = _mockRepository.Create<Block>(null);
            var dummyException = new ArgumentException(); // Arbitrary type
            Mock<FlexiBlockParser> mockTestSubject = _mockRepository.Create<FlexiBlockParser>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("TryContinueFlexiBlock", dummyBlockProcessor, mockBlock.Object).Throws(dummyException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.TryContinue(dummyBlockProcessor, mockBlock.Object));
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

        [Fact]
        public void Close_ThrowsArgumentNullExceptionIfProcessorIsNull()
        {
            // Arrange
            Mock<FlexiBlockParser> mockTestSubject = _mockRepository.Create<FlexiBlockParser>();
            mockTestSubject.CallBase = true;

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => mockTestSubject.Object.Close(null, _mockRepository.Create<Block>(null).Object));
        }

        [Fact]
        public void CloseThrowsArgumentNullExceptionIfBlockIsNull()
        {
            // Arrange
            Mock<FlexiBlockParser> mockTestSubject = _mockRepository.Create<FlexiBlockParser>();
            mockTestSubject.CallBase = true;

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => mockTestSubject.Object.Close(MarkdigTypesFactory.CreateBlockProcessor(), null));
        }

        [Fact]
        public void Close_DoesNotInterfereWithFlexiBlocksExceptionsWithBlockContext()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<Block> mockBlock = _mockRepository.Create<Block>(null);
            var dummyFlexiBlocksException = new FlexiBlocksException(mockBlock.Object);
            Mock<FlexiBlockParser> mockTestSubject = _mockRepository.Create<FlexiBlockParser>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("CloseFlexiBlock", dummyBlockProcessor, mockBlock.Object).Throws(dummyFlexiBlocksException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.Close(dummyBlockProcessor, mockBlock.Object));
            _mockRepository.VerifyAll();
            Assert.Same(dummyFlexiBlocksException, result);
            Assert.Null(result.InnerException);
        }

        [Fact]
        public void Close_WrapsExceptionsInFlexiBlocksExceptions()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<Block> mockBlock = _mockRepository.Create<Block>(null);
            var dummyException = new ArgumentException(); // Arbitrary type
            Mock<FlexiBlockParser> mockTestSubject = _mockRepository.Create<FlexiBlockParser>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("CloseFlexiBlock", dummyBlockProcessor, mockBlock.Object).Throws(dummyException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.Close(dummyBlockProcessor, mockBlock.Object));
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
    }
}
