using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests
{
    public class FencedBlockParserUnitTests
    {
        private static readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFencedBlockFactoryIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new ExposedFencedBlockParser(null));
        }

        [Fact]
        public void TryOpenBlock_ReturnsBlockStateNoneIfLineHasCodeIndent()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = 4; // Code indent
            dummyBlockProcessor.Line = new StringSlice(""); // To avoid null reference exception
            ExposedFencedBlockParser testSubject = CreateExposedFencedBlockParser();

            // Act
            BlockState result = testSubject.ExposedTryOpenBlock(dummyBlockProcessor);

            // Assert
            Assert.Equal(BlockState.None, result);
        }

        [Fact]
        public void TryOpenBlock_ReturnsBlockStateNoneIfLineDoesNotContainAnOpeningFence()
        {
            // Arrange
            const char dummyFenceChar = '~';
            var dummyLine = new StringSlice(dummyFenceChar.ToString());
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyLine;
            Mock<ExposedFencedBlockParser> mockTestSubject = CreateMockExposedFencedBlockParser();
            mockTestSubject.CallBase = true;
            int dummyFenceCharCount;
            mockTestSubject.Setup(t => t.LineContainsOpeningFence(dummyLine, dummyFenceChar, out dummyFenceCharCount)).Returns(false);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryOpenBlock(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.None, result);
        }

        [Fact]
        public void TryOpenBlock_ReturnsContinueDiscardAndAddsNewProxyFencedBlockToNewBlocksIfLineContainsAnOpeningFence()
        {
            // Arrange
            const char dummyFenceChar = '~';
            int dummyFenceCharCount = 3;
            const int dummyIndent = 2;
            var dummyLine = new StringSlice(dummyFenceChar.ToString());
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyLine;
            dummyBlockProcessor.Column = dummyIndent;
            Mock<DummyProxyFencedBlock> dummyProxyFencedBlock = _mockRepository.Create<DummyProxyFencedBlock>(null);
            Mock<IFencedBlockFactory<Block, DummyProxyFencedBlock>> mockFencedBlockFactory = _mockRepository.Create<IFencedBlockFactory<Block, DummyProxyFencedBlock>>();
            Mock<ExposedFencedBlockParser> mockTestSubject = CreateMockExposedFencedBlockParser(mockFencedBlockFactory.Object);
            mockFencedBlockFactory.
                Setup(f => f.CreateProxyFencedBlock(dummyIndent, dummyFenceCharCount, dummyFenceChar, dummyBlockProcessor, mockTestSubject.Object)).
                Returns(dummyProxyFencedBlock.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.LineContainsOpeningFence(dummyLine, dummyFenceChar, out dummyFenceCharCount)).Returns(true);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryOpenBlock(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.ContinueDiscard, result);
            Assert.Same(dummyProxyFencedBlock.Object, dummyBlockProcessor.NewBlocks.Peek());
        }

        [Fact]
        public void TryContinueBlock_ReturnsBlockStateContinueAndUpdatesLineStartIfLineHasCodeIndent()
        {
            // Arrange
            Mock<DummyProxyFencedBlock> dummyProxyFencedBlock = _mockRepository.Create<DummyProxyFencedBlock>(null);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = 4; // Code indent
            Mock<ExposedFencedBlockParser> mockTestSubject = CreateMockExposedFencedBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.UpdateLineStart(dummyBlockProcessor, dummyProxyFencedBlock.Object));

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryContinueBlock(dummyBlockProcessor, dummyProxyFencedBlock.Object);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.Continue, result);
        }

        [Fact]
        public void TryContinueBlock_ReturnsBlockStateContinueAndUpdatesLineStartIfLineDoesNotContainAClosingFence()
        {
            // Arrange
            const char dummyFenceChar = '~';
            const int dummyOpeningFenceCharCount = 4;
            Mock<DummyProxyFencedBlock> mockProxyFencedBlock = _mockRepository.Create<DummyProxyFencedBlock>(null);
            mockProxyFencedBlock.Setup(p => p.FenceChar).Returns(dummyFenceChar);
            mockProxyFencedBlock.Setup(p => p.OpeningFenceCharCount).Returns(dummyOpeningFenceCharCount);
            var dummyLine = new StringSlice();
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyLine;
            Mock<ExposedFencedBlockParser> mockTestSubject = CreateMockExposedFencedBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.LineContainsClosingFence(dummyLine, dummyFenceChar, dummyOpeningFenceCharCount)).Returns(false);
            mockTestSubject.Setup(t => t.UpdateLineStart(dummyBlockProcessor, mockProxyFencedBlock.Object));

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryContinueBlock(dummyBlockProcessor, mockProxyFencedBlock.Object);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.Continue, result);
        }

        [Fact]
        public void TryContinueBlock_ReturnsBlockStateBreadDiscardAndUpdatesSpanEndIfLineContainsAClosingFence()
        {
            // Arrange
            const char dummyFenceChar = '~';
            const int dummyOpeningFenceCharCount = 4;
            Mock<DummyProxyFencedBlock> mockProxyFencedBlock = _mockRepository.Create<DummyProxyFencedBlock>(null);
            mockProxyFencedBlock.Setup(p => p.FenceChar).Returns(dummyFenceChar);
            mockProxyFencedBlock.Setup(p => p.OpeningFenceCharCount).Returns(dummyOpeningFenceCharCount);
            const int dummyLineEnd = 10;
            var dummyLine = new StringSlice("", 0, dummyLineEnd);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyLine;
            Mock<ExposedFencedBlockParser> mockTestSubject = CreateMockExposedFencedBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.LineContainsClosingFence(dummyLine, dummyFenceChar, dummyOpeningFenceCharCount)).Returns(true);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryContinueBlock(dummyBlockProcessor, mockProxyFencedBlock.Object);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.BreakDiscard, result);
            Assert.Equal(dummyLineEnd, mockProxyFencedBlock.Object.Span.End);
        }

        [Fact]
        public void CloseProxy_CreatesFencedBlock()
        {
            // Arrange
            Mock<DummyProxyFencedBlock> dummyProxyFencedBlock = _mockRepository.Create<DummyProxyFencedBlock>(null);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<Block> dummyBlock = _mockRepository.Create<Block>(null);
            Mock<IFencedBlockFactory<Block, DummyProxyFencedBlock>> mockFencedBlockFactory = _mockRepository.Create<IFencedBlockFactory<Block, DummyProxyFencedBlock>>();
            mockFencedBlockFactory.Setup(f => f.Create(dummyProxyFencedBlock.Object, dummyBlockProcessor)).Returns(dummyBlock.Object);
            ExposedFencedBlockParser testSubject = CreateExposedFencedBlockParser(mockFencedBlockFactory.Object);

            // Act
            Block result = testSubject.ExposedCloseProxy(dummyBlockProcessor, dummyProxyFencedBlock.Object);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Same(dummyBlock.Object, result);
        }

        [Theory]
        [MemberData(nameof(UpdateLineStart_UpdatesLineStart_Data))]
        public void UpdateLineStart_UpdatesLineStart(string dummyLineText, int dummyOpeningFenceIndent, int dummyLineIndent, int expectedResult)
        {
            // Arrange
            var dummyLine = new StringSlice(dummyLineText);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyLine;
            dummyBlockProcessor.Column = dummyLineIndent;
            Mock<DummyProxyFencedBlock> mockProxyFencedBlock = _mockRepository.Create<DummyProxyFencedBlock>(null);
            mockProxyFencedBlock.Setup(p => p.OpeningFenceIndent).Returns(dummyOpeningFenceIndent);
            ExposedFencedBlockParser testSubject = CreateExposedFencedBlockParser();

            // Act
            testSubject.UpdateLineStart(dummyBlockProcessor, mockProxyFencedBlock.Object);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(expectedResult, dummyBlockProcessor.Indent);
        }

        public static IEnumerable<object[]> UpdateLineStart_UpdatesLineStart_Data()
        {
            return new object[][]
            {
                // Line indent greater than opening fence indent
                new object[]{"     dummyLineText", 3, 5, 3},
                // Line indent less than opening fence indent
                new object[]{"  dummyLineText", 3, 2, 2},
                // Line indent equal to opening fence indent
                new object[]{"   dummyLineText", 3, 3, 3},
            };
        }

        [Theory]
        [MemberData(nameof(LineContainsOpeningFence_ReturnsTrueIfLineContainsAnOpeningFenceAndFalseOtherwise_Data))]
        public void LineContainsOpeningFence_ReturnsTrueIfLineContainsAnOpeningFenceAndFalseOtherwise(string dummyLineText,
            char dummyFenceChar,
            bool expectedContains,
            int expectedFenceCharCount)
        {
            // Arrange
            var dummyLine = new StringSlice(dummyLineText);
            ExposedFencedBlockParser testSubject = CreateExposedFencedBlockParser();

            // Act
            bool resultContains = testSubject.LineContainsOpeningFence(dummyLine, dummyFenceChar, out int resultFenceCharCount);

            // Assert
            Assert.Equal(expectedContains, resultContains);
            Assert.Equal(expectedFenceCharCount, resultFenceCharCount);
        }

        public static IEnumerable<object[]> LineContainsOpeningFence_ReturnsTrueIfLineContainsAnOpeningFenceAndFalseOtherwise_Data()
        {
            return new object[][]
            {
                // Standard fence
                new object[]{"```", '`', true, 3},
                // Fence with trailing whitespace
                new object[]{"***   ", '*', true, 3},
                // Backtick fence with trailing non-whitespace chars
                new object[]{"```info string ", '`', true, 3},
                // Tilde fence with trailing non-whitespace chars
                new object[]{"~~~info string ~#%(*& 123434", '~', true, 3},
                // Fence with lots of chars
                new object[]{"~~~~~~~~~~", '~', true, 10},
                // Line has insufficent chars
                new object[]{"1", '1', false, 0},
                // Insufficent fence chars
                new object[]{"** ", '*', false, 2},
                // Line with inline code
                new object[]{"``` inline code`", '`', false, 3},
                // Non-backtick fence with trailing non-whitespace chars
                new object[]{"+++info string ", '+', false, 3}
            };
        }

        [Theory]
        [MemberData(nameof(LineContainsClosingFence_ReturnsTrueIfLineContainsAClosingFenceAndFalseOtherwise_Data))]
        public void LineContainsClosingFence_ReturnsTrueIfLineContainsAClosingFenceAndFalseOtherwise(string dummyLineText,
            char dummyFenceChar,
            int dummyOpeningFenceCharCount,
            bool expectedResult)
        {
            // Arrange
            var dummyLine = new StringSlice(dummyLineText);
            ExposedFencedBlockParser testSubject = CreateExposedFencedBlockParser();

            // Act
            bool result = testSubject.LineContainsClosingFence(dummyLine, dummyFenceChar, dummyOpeningFenceCharCount);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> LineContainsClosingFence_ReturnsTrueIfLineContainsAClosingFenceAndFalseOtherwise_Data()
        {
            return new object[][]
            {
                // Standard fence
                new object[]{"```", '`', 3, true},
                // Fence with trailing whitespace
                new object[]{"***   ", '*', 3, true},
                // Fence with more chars than opening fence
                new object[]{"`````", '`', 4, true},
                // Fence with lots of chars
                new object[]{"~~~~~~~~~~", '~', 5, true},
                // Line with insufficent chars
                new object[]{"-----", '-', 6, false},
                // Insufficient fence chars
                new object[]{"111   ", '1', 4, false},
                // Fence has trailing non-whitespace chars
                new object[]{"+++ non-whitespace chars", '+', 3, false}
            };
        }

        private ExposedFencedBlockParser CreateExposedFencedBlockParser(IFencedBlockFactory<Block, DummyProxyFencedBlock> fencedBlockFactory = null)
        {
            return new ExposedFencedBlockParser(fencedBlockFactory ?? _mockRepository.Create<IFencedBlockFactory<Block, DummyProxyFencedBlock>>().Object);
        }

        private Mock<ExposedFencedBlockParser> CreateMockExposedFencedBlockParser(IFencedBlockFactory<Block, DummyProxyFencedBlock> fencedBlockFactory = null)
        {
            return _mockRepository.Create<ExposedFencedBlockParser>(fencedBlockFactory ?? _mockRepository.Create<IFencedBlockFactory<Block, DummyProxyFencedBlock>>().Object);
        }

        public class ExposedFencedBlockParser : FencedBlockParser<Block, DummyProxyFencedBlock>
        {
            public ExposedFencedBlockParser(IFencedBlockFactory<Block, DummyProxyFencedBlock> fencedBlockFactory) : base(fencedBlockFactory)
            {
            }

            public BlockState ExposedTryOpenBlock(BlockProcessor blockProcessor)
            {
                return TryOpenBlock(blockProcessor);
            }

            public BlockState ExposedTryContinueBlock(BlockProcessor blockProcessor, DummyProxyFencedBlock proxyBlock)
            {
                return TryContinueBlock(blockProcessor, proxyBlock);
            }

            public Block ExposedCloseProxy(BlockProcessor blockProcessor, DummyProxyFencedBlock proxyBlock)
            {
                return CloseProxy(blockProcessor, proxyBlock);
            }
        }

        public abstract class DummyProxyFencedBlock : Block, IProxyFencedBlock
        {
            protected DummyProxyFencedBlock(BlockParser parser) : base(parser)
            {
            }

            public abstract int OpeningFenceIndent { get; }

            public abstract int OpeningFenceCharCount { get; }

            public abstract char FenceChar { get; }

            public abstract string MainTypeName { get; }
        }
    }
}
