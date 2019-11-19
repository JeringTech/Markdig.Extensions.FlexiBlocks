using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests
{
    public class BlockParserUnitTests
    {
        private static readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void TryOpen_ThrowsArgumentNullExceptionIfProcessorIsNull()
        {
            // Arrange
            Mock<BlockParser<Block>> mockTestSubject = CreateMockBlockParser<Block>();
            mockTestSubject.CallBase = true;

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => mockTestSubject.Object.TryOpen(null));
        }

        [Fact]
        public void TryOpen_DoesNotInterfereWithBlockExceptionWithBlockContext()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyBlockException = new BlockException(_mockRepository.Create<Block>(null).Object); // Pass block to fake block context
            Mock<BlockParser<Block>> mockTestSubject = CreateMockBlockParser<Block>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("TryOpenBlock", dummyBlockProcessor).Throws(dummyBlockException);

            // Act and assert
            BlockException result = Assert.Throws<BlockException>(() => mockTestSubject.Object.TryOpen(dummyBlockProcessor));
            _mockRepository.VerifyAll();
            Assert.Same(dummyBlockException, result);
            Assert.Null(result.InnerException);
        }

        [Fact]
        public void TryOpen_DoesNotInterfereWithBlockExceptionWithLineContextIfNoBlockIsCreated()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyBlockException = new BlockException(1, 0);
            Mock<BlockParser<Block>> mockTestSubject = CreateMockBlockParser<Block>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("TryOpenBlock", dummyBlockProcessor).Throws(dummyBlockException);

            // Act and assert
            BlockException result = Assert.Throws<BlockException>(() => mockTestSubject.Object.TryOpen(dummyBlockProcessor));
            _mockRepository.VerifyAll();
            Assert.Same(dummyBlockException, result);
            Assert.Null(result.InnerException);
        }

        [Theory]
        [MemberData(nameof(TryOpen_WrapsNonBlockExceptionsAndBlockExceptionsWithoutBlockContextInBlockExceptionsWithBlockContextIfABlockIsCreated_Data))]
        public void TryOpen_WrapsNonBlockExceptionsAndBlockExceptionsWithoutBlockContextInBlockExceptionWithsBlockContextIfABlockIsCreated(Exception dummyException)
        {
            // Arrange
            Mock<Block> mockBlock = _mockRepository.Create<Block>(null);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.NewBlocks.Push(mockBlock.Object);
            Mock<BlockParser<Block>> mockTestSubject = CreateMockBlockParser<Block>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("TryOpenBlock", dummyBlockProcessor).Throws(dummyException);

            // Act and assert
            BlockException result = Assert.Throws<BlockException>(() => mockTestSubject.Object.TryOpen(dummyBlockProcessor));
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

        public static IEnumerable<object[]> TryOpen_WrapsNonBlockExceptionsAndBlockExceptionsWithoutBlockContextInBlockExceptionsWithBlockContextIfABlockIsCreated_Data()
        {
            return new object[][]
            {
                // Non BlockException
                new object[]{ new ArgumentException()},
                // BlockException with no context
                new object[]{ new BlockException()},
                // BlockException with line context
                new object[]{ new BlockException(1, 0)}
            };
        }

        [Theory]
        [MemberData(nameof(TryOpen_WrapsNonBlockExceptionsAndBlockExceptionsWithNoContextInBlockExceptionsWithLineContextIfNoBlockIsCreated_Data))]
        public void TryOpen_WrapsNonBlockExceptionsAndBlockExceptionsWithNoContextInBlockExceptionsWithLineContextIfNoBlockIsCreated(Exception dummyException)
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<BlockParser<Block>> mockTestSubject = CreateMockBlockParser<Block>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("TryOpenBlock", dummyBlockProcessor).Throws(dummyException);

            // Act and assert
            BlockException result = Assert.Throws<BlockException>(() => mockTestSubject.Object.TryOpen(dummyBlockProcessor));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock,
                    "block of unknown type",
                    dummyBlockProcessor.LineIndex + 1,
                    dummyBlockProcessor.Column,
                    string.Format(Strings.BlockException_BlockParser_ExceptionOccurredWhileAttemptingToOpenBlock, mockTestSubject.Object.GetType().Name)),
                result.Message,
                ignoreLineEndingDifferences: true);
            Assert.Same(dummyException, result.InnerException);
        }

        public static IEnumerable<object[]> TryOpen_WrapsNonBlockExceptionsAndBlockExceptionsWithNoContextInBlockExceptionsWithLineContextIfNoBlockIsCreated_Data()
        {
            return new object[][]
            {
                // Non BlockException
                new object[]{ new ArgumentException()},
                // BlockException with no context
                new object[]{ new BlockException()},
            };
        }

        [Fact]
        public void TryContinueBlock_ReturnsBlockStateNone()
        {
            // Arrange
            ExposedBlockParser testSubject = CreateExposedBlockParser();

            // Act
            BlockState result = testSubject.ExposedTryContinueBlock(null, null);

            // Assert
            Assert.Equal(BlockState.None, result);
        }

        [Fact]
        public void TryContinue_ThrowsArgumentNullExceptionIfProcessorIsNull()
        {
            // Arrange
            Mock<BlockParser<Block>> mockTestSubject = CreateMockBlockParser<Block>();
            mockTestSubject.CallBase = true;

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => mockTestSubject.Object.TryContinue(null, _mockRepository.Create<Block>(null).Object));
        }

        [Fact]
        public void TryContinue_ThrowsArgumentNullExceptionIfBlockIsNotOfTypeT()
        {
            // Arrange
            Mock<BlockParser<LeafBlock>> mockTestSubject = CreateMockBlockParser<LeafBlock>();
            mockTestSubject.CallBase = true;
            Mock<ContainerBlock> dummyContainerBlock = _mockRepository.Create<ContainerBlock>(null);

            // Act and assert
            ArgumentException result = Assert.
                Throws<ArgumentException>(() => mockTestSubject.Object.TryContinue(MarkdigTypesFactory.CreateBlockProcessor(), dummyContainerBlock.Object));
            Assert.
                Equal(string.Format(Strings.ArgumentException_Shared_ValueMustBeOfExpectedType,
                        "block",
                        typeof(LeafBlock).Name,
                        dummyContainerBlock.Object.GetType().Name),
                    result.Message);
        }

        [Fact]
        public void TryContinue_DoesNotInterfereWithBlockExceptionWithBlockContext()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<Block> dummyBlock = _mockRepository.Create<Block>(null);
            var dummyBlockException = new BlockException(dummyBlock.Object);
            Mock<BlockParser<Block>> mockTestSubject = CreateMockBlockParser<Block>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("TryContinueBlock", dummyBlockProcessor, dummyBlock.Object).Throws(dummyBlockException);

            // Act and assert
            BlockException result = Assert.Throws<BlockException>(() => mockTestSubject.Object.TryContinue(dummyBlockProcessor, dummyBlock.Object));
            _mockRepository.VerifyAll();
            Assert.Same(dummyBlockException, result);
            Assert.Null(result.InnerException);
        }

        [Theory]
        [MemberData(nameof(TryContinue_WrapsNonBlockExceptionsAndBlockExceptionsWithoutBlockContextInBlockExceptionsWithBlockContext_Data))]
        public void TryContinue_WrapsNonBlockExceptionsAndBlockExceptionsWithoutBlockContextInBlockExceptionsWithBlockContext(Exception dummyException)
        {
            // Arrange
            const int dummyLineIndex = 4;
            const int dummyColumn = 5;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<Block> mockBlock = _mockRepository.Create<Block>(null);
            mockBlock.Object.Line = dummyLineIndex; // Line isn't virtual
            mockBlock.Object.Column = dummyColumn; // Column isn't virtual
            Mock<BlockParser<Block>> mockTestSubject = CreateMockBlockParser<Block>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("TryContinueBlock", dummyBlockProcessor, mockBlock.Object).Throws(dummyException);

            // Act and assert
            BlockException result = Assert.Throws<BlockException>(() => mockTestSubject.Object.TryContinue(dummyBlockProcessor, mockBlock.Object));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock,
                    mockBlock.Object.GetType().Name,
                    dummyLineIndex + 1,
                    dummyColumn,
                    Strings.BlockException_BlockException_ExceptionOccurredWhileProcessingBlock),
                result.Message,
                ignoreLineEndingDifferences: true);
            Assert.Same(dummyException, result.InnerException);
        }

        public static IEnumerable<object[]> TryContinue_WrapsNonBlockExceptionsAndBlockExceptionsWithoutBlockContextInBlockExceptionsWithBlockContext_Data()
        {
            return new object[][]
            {
                // Non BlockException
                new object[]{ new ArgumentException()},
                // BlockException with no context
                new object[]{ new BlockException()},
                // BlockException with line context
                new object[]{ new BlockException(1, 0) }
            };
        }

        [Fact]
        public void CloseBlock_ReturnsTrue()
        {
            // Arrange
            ExposedBlockParser testSubject = CreateExposedBlockParser();

            // Act
            bool result = testSubject.ExposedCloseBlock(null, null);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Close_ThrowsArgumentNullExceptionIfProcessorIsNull()
        {
            // Arrange
            Mock<BlockParser<Block>> mockTestSubject = CreateMockBlockParser<Block>();
            mockTestSubject.CallBase = true;

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => mockTestSubject.Object.Close(null, _mockRepository.Create<Block>(null).Object));
        }

        [Fact]
        public void Close_ThrowsNullExceptionIfBlockIsNotOfTypeT()
        {
            // Arrange
            Mock<BlockParser<ContainerBlock>> mockTestSubject = CreateMockBlockParser<ContainerBlock>();
            mockTestSubject.CallBase = true;
            Mock<LeafBlock> dummyLeafBlock = _mockRepository.Create<LeafBlock>(null);

            // Act and assert
            ArgumentException result = Assert.
                Throws<ArgumentException>(() => mockTestSubject.Object.Close(MarkdigTypesFactory.CreateBlockProcessor(), dummyLeafBlock.Object));
            Assert.Equal(string.Format(Strings.ArgumentException_Shared_ValueMustBeOfExpectedType,
                    "block",
                    typeof(ContainerBlock).Name,
                    dummyLeafBlock.Object.GetType().Name),
                result.Message);
        }

        [Fact]
        public void Close_DoesNotInterfereWithBlockExceptionWithBlockContext()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<Block> dummyBlock = _mockRepository.Create<Block>(null);
            var dummyBlockException = new BlockException(dummyBlock.Object); // So it has block context
            Mock<BlockParser<Block>> mockTestSubject = CreateMockBlockParser<Block>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("CloseBlock", dummyBlockProcessor, dummyBlock.Object).Throws(dummyBlockException);

            // Act and assert
            BlockException result = Assert.Throws<BlockException>(() => mockTestSubject.Object.Close(dummyBlockProcessor, dummyBlock.Object));
            _mockRepository.VerifyAll();
            Assert.Same(dummyBlockException, result);
            Assert.Null(result.InnerException);
        }

        [Theory]
        [MemberData(nameof(Close_WrapsNonBlockExceptionsAndBlockExceptionsWithoutBlockContextInBlockExceptionsWithBlockContext_Data))]
        public void Close_WrapsNonBlockExceptionsAndBlockExceptionsWithoutBlockContextInBlockExceptionsWithBlockContext(Exception dummyException)
        {
            // Arrange
            const int dummyLineIndex = 4;
            const int dummyColumn = 5;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<Block> mockBlock = _mockRepository.Create<Block>(null);
            mockBlock.Object.Line = dummyLineIndex; // Line isn't virtual
            mockBlock.Object.Column = dummyColumn; // Column isn't virtual
            Mock<BlockParser<Block>> mockTestSubject = CreateMockBlockParser<Block>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("CloseBlock", dummyBlockProcessor, mockBlock.Object).Throws(dummyException);

            // Act and assert
            BlockException result = Assert.Throws<BlockException>(() => mockTestSubject.Object.Close(dummyBlockProcessor, mockBlock.Object));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock,
                    mockBlock.Object.GetType().Name,
                    dummyLineIndex + 1,
                    dummyColumn,
                    Strings.BlockException_BlockException_ExceptionOccurredWhileProcessingBlock),
                result.Message,
                ignoreLineEndingDifferences: true);
            Assert.Same(dummyException, result.InnerException);
        }

        public static IEnumerable<object[]> Close_WrapsNonBlockExceptionsAndBlockExceptionsWithoutBlockContextInBlockExceptionsWithBlockContext_Data()
        {
            return new object[][]
            {
                // Non BlockException
                new object[]{ new ArgumentException()},
                // BlockException with no context
                new object[]{ new BlockException()},
                // BlockException with line context
                new object[]{ new BlockException(1, 0)},
            };
        }

        private Mock<BlockParser<T>> CreateMockBlockParser<T>()
            where T : Block
        {
            return _mockRepository.Create<BlockParser<T>>();
        }

        private ExposedBlockParser CreateExposedBlockParser()
        {
            return new ExposedBlockParser();
        }

        private class ExposedBlockParser : BlockParser<Block>
        {
            public BlockState ExposedTryContinueBlock(BlockProcessor blockProcessor, Block block)
            {
                return TryContinueBlock(blockProcessor, block);
            }

            public bool ExposedCloseBlock(BlockProcessor blockProcessor, Block block)
            {
                return CloseBlock(blockProcessor, block);
            }

            protected override BlockState TryOpenBlock(BlockProcessor blockProcessor)
            {
                return BlockState.None;   // Do nothing
            }
        }
    }
}
