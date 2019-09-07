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
            Assert.Throws<ArgumentNullException>(() => new ExposedFencedBlockParser(null, 'a', false, FenceTrailingCharacters.All));
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
            const string dummyText = "dummyText";
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyText);
            Mock<ExposedFencedBlockParser> mockTestSubject = CreateMockExposedFencedBlockParser();
            mockTestSubject.CallBase = true;
            int dummyFenceCharCount;
            mockTestSubject.Setup(t => t.LineContainsOpeningFence(It.Is<StringSlice>(s => s.Text == dummyText), out dummyFenceCharCount)).Returns(false);

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
            int dummyFenceCharCount = 3;
            const int dummyIndent = 2;
            const string dummyText = "dummyText";
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyText);
            dummyBlockProcessor.Column = dummyIndent;
            DummyProxyFencedBlock dummyProxyFencedBlock = _mockRepository.Create<DummyProxyFencedBlock>(null).Object;
            Mock<IFencedBlockFactory<Block, DummyProxyFencedBlock>> mockFencedBlockFactory = _mockRepository.Create<IFencedBlockFactory<Block, DummyProxyFencedBlock>>();
            Mock<ExposedFencedBlockParser> mockTestSubject = CreateMockExposedFencedBlockParser(mockFencedBlockFactory.Object);
            mockFencedBlockFactory.
                Setup(f => f.CreateProxyFencedBlock(dummyIndent, dummyFenceCharCount, dummyBlockProcessor, mockTestSubject.Object)).
                Returns(dummyProxyFencedBlock);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.LineContainsOpeningFence(It.Is<StringSlice>(s => s.Text == dummyText), out dummyFenceCharCount)).Returns(true);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryOpenBlock(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.ContinueDiscard, result);
            Assert.Same(dummyProxyFencedBlock, dummyBlockProcessor.NewBlocks.Peek());
        }

        [Fact]
        public void TryOpenBlock_ThrowsBlockExceptionIfAnExceptionIsThrownWhileCreatingProxyFencedBlock()
        {
            // Arrange
            int dummyFenceCharCount = 3;
            const int dummyLineIndex = 6;
            const int dummyIndent = 2;
            const string dummyText = "dummyText";
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyText);
            dummyBlockProcessor.Column = dummyIndent;
            dummyBlockProcessor.LineIndex = dummyLineIndex;
            DummyProxyFencedBlock dummyProxyFencedBlock = _mockRepository.Create<DummyProxyFencedBlock>(null).Object;
            Mock<IFencedBlockFactory<Block, DummyProxyFencedBlock>> mockFencedBlockFactory = _mockRepository.Create<IFencedBlockFactory<Block, DummyProxyFencedBlock>>();
            var dummyException = new Exception();
            Mock<ExposedFencedBlockParser> mockTestSubject = CreateMockExposedFencedBlockParser(mockFencedBlockFactory.Object);
            mockFencedBlockFactory.
                Setup(f => f.CreateProxyFencedBlock(dummyIndent, dummyFenceCharCount, dummyBlockProcessor, mockTestSubject.Object)).
                Throws(dummyException);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.LineContainsOpeningFence(It.Is<StringSlice>(s => s.Text == dummyText), out dummyFenceCharCount)).Returns(true);

            // Act and assert
            BlockException result = Assert.Throws<BlockException>(() => mockTestSubject.Object.ExposedTryOpenBlock(dummyBlockProcessor));
            Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock, nameof(Block), dummyLineIndex + 1, dummyIndent,
                    Strings.BlockException_Shared_ExceptionOccurredWhileCreatingBlock),
                result.Message);
            Assert.Same(dummyException, result.InnerException);
            _mockRepository.VerifyAll();
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
            const string dummyText = "dummyText";
            const int dummyOpeningFenceCharCount = 4;
            Mock<DummyProxyFencedBlock> mockProxyFencedBlock = _mockRepository.Create<DummyProxyFencedBlock>(null);
            mockProxyFencedBlock.Setup(p => p.OpeningFenceCharCount).Returns(dummyOpeningFenceCharCount);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyText);
            Mock<ExposedFencedBlockParser> mockTestSubject = CreateMockExposedFencedBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.LineContainsClosingFence(It.Is<StringSlice>(s => s.Text == dummyText), dummyOpeningFenceCharCount)).Returns(false);
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
            const string dummyText = "dummyText";
            const int dummyOpeningFenceCharCount = 3;
            Mock<DummyProxyFencedBlock> mockProxyFencedBlock = _mockRepository.Create<DummyProxyFencedBlock>(null);
            mockProxyFencedBlock.Setup(p => p.OpeningFenceCharCount).Returns(dummyOpeningFenceCharCount);
            const int dummyLineEnd = 10;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyText, 0, dummyLineEnd);
            Mock<ExposedFencedBlockParser> mockTestSubject = CreateMockExposedFencedBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.LineContainsClosingFence(It.Is<StringSlice>(s => s.Text == dummyText), dummyOpeningFenceCharCount)).Returns(true);

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
        public void LineContainsOpeningFence_ReturnsTrueIfLineContainsAnOpeningFenceAndFalseOtherwise(FenceTrailingCharacters dummyFenceTrailingCharacters,
            string dummyLineText,
            char dummyFenceChar,
            bool expectedContains,
            int expectedFenceCharCount)
        {
            // Arrange
            var dummyLine = new StringSlice(dummyLineText);
            ExposedFencedBlockParser testSubject = CreateExposedFencedBlockParser(fenceChar: dummyFenceChar, fenceTrailingCharacters: dummyFenceTrailingCharacters);

            // Act
            bool resultContains = testSubject.LineContainsOpeningFence(dummyLine, out int resultFenceCharCount);

            // Assert
            Assert.Equal(expectedContains, resultContains);
            Assert.Equal(expectedFenceCharCount, resultFenceCharCount);
        }

        public static IEnumerable<object[]> LineContainsOpeningFence_ReturnsTrueIfLineContainsAnOpeningFenceAndFalseOtherwise_Data()
        {
            return new object[][]
            {
                // Insufficient fence characters
                new object[]{FenceTrailingCharacters.All, "1", '1', false, 1},
                // No trailing characters
                new object[]{FenceTrailingCharacters.All, "```", '`', true, 3},
                // All characters
                new object[]{FenceTrailingCharacters.All, "`````` 8&a` ", '`', true, 6},
                // Whitespace characters only
                new object[]{FenceTrailingCharacters.Whitespace, "****   ", '*', true, 4},
                new object[]{FenceTrailingCharacters.Whitespace, "***  fh ", '*', false, 3},
                // All characters but fence character
                new object[]{FenceTrailingCharacters.AllButFenceCharacter, "``` d(h`", '`', false, 3},
                new object[]{FenceTrailingCharacters.AllButFenceCharacter, "``` j^4 ", '`', true, 3},
            };
        }

        [Theory]
        [MemberData(nameof(LineContainsClosingFence_ReturnsTrueIfLineContainsAClosingFenceAndFalseOtherwise_Data))]
        public void LineContainsClosingFence_ReturnsTrueIfLineContainsAClosingFenceAndFalseOtherwise(bool dummyMatchingFencesRequired,
            string dummyLineText,
            char dummyFenceChar,
            int dummyOpeningFenceCharCount,
            bool expectedResult)
        {
            // Arrange
            var dummyLine = new StringSlice(dummyLineText);
            ExposedFencedBlockParser testSubject = CreateExposedFencedBlockParser(fenceChar: dummyFenceChar, matchingFencesRequired: dummyMatchingFencesRequired);

            // Act
            bool result = testSubject.LineContainsClosingFence(dummyLine, dummyOpeningFenceCharCount);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> LineContainsClosingFence_ReturnsTrueIfLineContainsAClosingFenceAndFalseOtherwise_Data()
        {
            return new object[][]
            {
                // Matching fences required
                new object[]{true, "```", '`', 3, true},
                new object[]{true, "`````", '`', 3, false},
                new object[]{true, "`", '`', 3, false},
                new object[]{true, "***   ", '*', 3, true}, // Trailing whitespace
                new object[]{true, "+++ h3* ", '+', 3, false}, // Trailing non-whitespace characters
                // Matching fences not required
                new object[]{false, "```", '`', 3, true},
                new object[]{false, "`````", '`', 3, true},
                new object[]{false, "`", '`', 3, false},
                new object[]{false, "***   ", '*', 3, true}, // Trailing whitespace
                new object[]{false, "+++ h3* ", '+', 3, false} // Trailing non-whitespace characters
            };
        }

        private ExposedFencedBlockParser CreateExposedFencedBlockParser(IFencedBlockFactory<Block, DummyProxyFencedBlock> FencedBlockFactory = null,
            char fenceChar = default,
            bool matchingFencesRequired = default,
            FenceTrailingCharacters fenceTrailingCharacters = default)
        {
            return new ExposedFencedBlockParser(FencedBlockFactory ?? _mockRepository.Create<IFencedBlockFactory<Block, DummyProxyFencedBlock>>().Object,
                fenceChar,
                matchingFencesRequired,
                fenceTrailingCharacters);
        }

        private Mock<ExposedFencedBlockParser> CreateMockExposedFencedBlockParser(IFencedBlockFactory<Block, DummyProxyFencedBlock> FencedBlockFactory = null,
            char fenceChar = default,
            bool matchingFencesRequired = default,
            FenceTrailingCharacters fenceTrailingCharacters = default)
        {
            return _mockRepository.Create<ExposedFencedBlockParser>(FencedBlockFactory ?? _mockRepository.Create<IFencedBlockFactory<Block, DummyProxyFencedBlock>>().Object,
                fenceChar,
                matchingFencesRequired,
                fenceTrailingCharacters);
        }

        public class ExposedFencedBlockParser : FencedBlockParser<Block, DummyProxyFencedBlock>
        {
            public ExposedFencedBlockParser(IFencedBlockFactory<Block, DummyProxyFencedBlock> FencedBlockFactory,
                char fenceChar,
                bool matchingFencesRequired,
                FenceTrailingCharacters fenceTrailingCharacters) :
                base(FencedBlockFactory, fenceChar, matchingFencesRequired, fenceTrailingCharacters)
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
