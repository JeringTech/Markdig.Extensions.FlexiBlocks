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
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void TryOpen_DoesNotInterfereWithFlexiBlocksExceptionsWithBlockContextIfAFlexiBlockIsCreated()
        {
            // Arrange
            var dummyBlock = new DummyBlock(null);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.NewBlocks.Push(dummyBlock);
            var dummyFlexiBlocksException = new FlexiBlocksException(new DummyBlock(null));
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
                    new SerializableWrapper<FlexiBlocksException>(new FlexiBlocksException(new DummyBlock(null)))
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
            var dummyBlock = new DummyBlock(null);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.NewBlocks.Push(dummyBlock);
            var dummyException = new ArgumentException(); // Arbitrary type
            Mock<FlexiBlockParser> mockTestSubject = _mockRepository.Create<FlexiBlockParser>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("TryOpenFlexiBlock", dummyBlockProcessor).Throws(dummyException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.TryOpen(dummyBlockProcessor));
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
            Assert.Equal(string.Format(Strings.FlexiBlocksException_InvalidMarkdown,
                    dummyBlockProcessor.LineIndex + 1,
                    dummyBlockProcessor.Column,
                    string.Format(Strings.FlexiBlocksException_ExceptionOccurredWhileAttemptingToOpenBlock, "FlexiBlockParserProxy")),
                result.Message,
                ignoreLineEndingDifferences: true);
            Assert.Same(dummyException, result.InnerException);
        }

        [Fact]
        public void TryContinue_DoesNotInterfereWithFlexiBlocksExceptionsWithBlockContext()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyContinueBlock = new DummyBlock(null);
            var dummyFlexiBlocksException = new FlexiBlocksException(new DummyBlock(null));
            Mock<FlexiBlockParser> mockTestSubject = _mockRepository.Create<FlexiBlockParser>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("TryContinueFlexiBlock", dummyBlockProcessor, dummyContinueBlock).Throws(dummyFlexiBlocksException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.TryContinue(dummyBlockProcessor, dummyContinueBlock));
            _mockRepository.VerifyAll();
            Assert.Same(dummyFlexiBlocksException, result);
            Assert.Null(result.InnerException);
        }

        [Fact]
        public void TryContinue_WrapsExceptionsInFlexiBlocksExceptions()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyBlock = new DummyBlock(null);
            var dummyException = new ArgumentException(); // Arbitrary type
            Mock<FlexiBlockParser> mockTestSubject = _mockRepository.Create<FlexiBlockParser>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("TryContinueFlexiBlock", dummyBlockProcessor, dummyBlock).Throws(dummyException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.TryContinue(dummyBlockProcessor, dummyBlock));
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

        [Fact]
        public void Close_DoesNotInterfereWithFlexiBlocksExceptionsWithBlockContext()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyBlock = new DummyBlock(null);
            var dummyFlexiBlocksException = new FlexiBlocksException(new DummyBlock(null));
            Mock<FlexiBlockParser> mockTestSubject = _mockRepository.Create<FlexiBlockParser>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("CloseFlexiBlock", dummyBlockProcessor, dummyBlock).Throws(dummyFlexiBlocksException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.Close(dummyBlockProcessor, dummyBlock));
            _mockRepository.VerifyAll();
            Assert.Same(dummyFlexiBlocksException, result);
            Assert.Null(result.InnerException);
        }

        [Fact]
        public void Close_WrapsExceptionsInFlexiBlocksExceptions()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyBlock = new DummyBlock(null);
            var dummyException = new ArgumentException(); // Arbitrary type
            Mock<FlexiBlockParser> mockTestSubject = _mockRepository.Create<FlexiBlockParser>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("CloseFlexiBlock", dummyBlockProcessor, dummyBlock).Throws(dummyException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.Close(dummyBlockProcessor, dummyBlock));
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

        private class DummyBlock : Block
        {
            public DummyBlock(BlockParser parser) : base(parser)
            {
            }
        }
    }
}
