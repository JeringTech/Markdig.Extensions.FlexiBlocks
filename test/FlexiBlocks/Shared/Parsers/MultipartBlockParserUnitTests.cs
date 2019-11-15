using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests
{
    public class MultipartBlockParserUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfMultipartBlockFactoryIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new ExposedMultipartBlockParser(null, default, default));
        }

        [Theory]
        [MemberData(nameof(Constructor_ThrowsArgumentExceptionIfPartTypesIsNullOrEmpty_Data))]
        public void Constructor_ThrowsArgumentExceptionIfPartTypesIsNullOrEmpty(PartType[] dummyPartTypes)
        {
            // Act and assert
            Assert.Throws<ArgumentException>(() => new ExposedMultipartBlockParser(_mockRepository.Create<IMultipartBlockFactory<DummyMultipartBlock>>().Object,
                "dummyName",
                dummyPartTypes));
        }

        public static IEnumerable<object[]> Constructor_ThrowsArgumentExceptionIfPartTypesIsNullOrEmpty_Data()
        {
            return new object[][]
            {
                // Null
                new object[]{null},
                // Empty
                new object[]{new PartType[0]}
            };
        }

        [Theory]
        [MemberData(nameof(Constructor_ThrowsArgumentExceptionIfNameIsNullWhitespaceOrAnEmptyString_Data))]
        public void Constructor_ThrowsArgumentExceptionIfNameIsNullWhitespaceOrAnEmptyString(string dummyName)
        {
            // Act and assert
            Assert.Throws<ArgumentException>(() => new ExposedMultipartBlockParser(_mockRepository.Create<IMultipartBlockFactory<DummyMultipartBlock>>().Object,
                dummyName,
                new PartType[] { PartType.Container }));
        }

        public static IEnumerable<object[]> Constructor_ThrowsArgumentExceptionIfNameIsNullWhitespaceOrAnEmptyString_Data()
        {
            return new object[][]
            {
                new object[]{null},
                new object[]{" "},
                new object[]{string.Empty}
            };
        }

        [Fact]
        public void Constructor_SetsOpeningCharacters()
        {
            // Act
            var testSubject = new ExposedMultipartBlockParser(_mockRepository.Create<IMultipartBlockFactory<DummyMultipartBlock>>().Object,
                "dummyName",
                new PartType[] { PartType.Container });

            // Assert
            Assert.Single(testSubject.OpeningCharacters);
            Assert.Equal('+', testSubject.OpeningCharacters[0]);
        }

        [Fact]
        public void TryOpenBlock_ReturnsBlockStateNoneIfLineHasCodeIndent()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = 4; // Code indent
            dummyBlockProcessor.Line = new StringSlice(""); // To avoid null reference exception
            ExposedMultipartBlockParser testSubject = CreateExposedMultipartBlockParser();

            // Act
            BlockState result = testSubject.ExposedTryOpenBlock(dummyBlockProcessor);

            // Assert
            Assert.Equal(BlockState.None, result);
        }

        [Fact]
        public void TryOpenBlock_ReturnsBlockStateNoneIfIsNotAnOpeningLine()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<ExposedMultipartBlockParser> mockTestSubject = CreateMockExposedMultipartBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.IsOpeningLine(dummyBlockProcessor)).Returns(false);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryOpenBlock(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.None, result);
        }

        [Fact]
        public void TryOpenBlock_ThrowsBlockExceptionIfAnExceptionIsThrownWhileCreatingPartBlock()
        {
            // Arrange
            const int dummyLineIndex = 8;
            const int dummyColumn = 3;
            const PartType dummyPartType = PartType.Leaf;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.LineIndex = dummyLineIndex;
            dummyBlockProcessor.Column = dummyColumn;
            var dummyException = new Exception();
            Mock<IMultipartBlockFactory<DummyMultipartBlock>> mockMultipartBlockFactory = _mockRepository.Create<IMultipartBlockFactory<DummyMultipartBlock>>();
            mockMultipartBlockFactory.Setup(m => m.CreatePart(dummyPartType, dummyBlockProcessor)).Throws(dummyException);
            Mock<ExposedMultipartBlockParser> mockTestSubject = CreateMockExposedMultipartBlockParser(mockMultipartBlockFactory.Object, "dummyName", new PartType[] { dummyPartType });
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(m => m.IsOpeningLine(dummyBlockProcessor)).Returns(true);

            // Act and assert
            BlockException result = Assert.Throws<BlockException>(() => mockTestSubject.Object.ExposedTryOpenBlock(dummyBlockProcessor));
            Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock, nameof(DummyMultipartBlock), dummyLineIndex + 1, dummyColumn,
                    Strings.BlockException_Shared_ExceptionOccurredWhileCreatingBlock),
                result.Message);
            _mockRepository.VerifyAll();
            Assert.Same(dummyException, result.InnerException);
        }

        [Fact]
        public void TryOpenBlock_ThrowsBlockExceptionIfAnExceptionIsThrownWhileCreatingMultipartBlock()
        {
            // Arrange
            const int dummyLineIndex = 8;
            const int dummyColumn = 3;
            const PartType dummyPartType = PartType.Leaf;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.LineIndex = dummyLineIndex;
            dummyBlockProcessor.Column = dummyColumn;
            var dummyException = new Exception();
            Mock<IMultipartBlockFactory<DummyMultipartBlock>> mockMultipartBlockFactory = _mockRepository.Create<IMultipartBlockFactory<DummyMultipartBlock>>();
            mockMultipartBlockFactory.Setup(m => m.CreatePart(dummyPartType, dummyBlockProcessor)).Returns((Block)null);
            Mock<ExposedMultipartBlockParser> mockTestSubject = CreateMockExposedMultipartBlockParser(mockMultipartBlockFactory.Object, "dummyName", new PartType[] { dummyPartType });
            mockMultipartBlockFactory.Setup(m => m.Create(dummyBlockProcessor, mockTestSubject.Object)).Throws(dummyException);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(m => m.IsOpeningLine(dummyBlockProcessor)).Returns(true);

            // Act and assert
            BlockException result = Assert.Throws<BlockException>(() => mockTestSubject.Object.ExposedTryOpenBlock(dummyBlockProcessor));
            Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock, nameof(DummyMultipartBlock), dummyLineIndex + 1, dummyColumn,
                    Strings.BlockException_Shared_ExceptionOccurredWhileCreatingBlock),
                result.Message);
            _mockRepository.VerifyAll();
            Assert.Same(dummyException, result.InnerException);
        }

        [Fact]
        public void TryOpenBlock_ReturnsContinueDiscardAddsNewPartAndMultipartBlocksToNewBlocksAndAddsMultipartBlockToOpenMultipartBlocksIfSuccessful()
        {
            // Arrange
            const int dummyLineIndex = 8;
            const int dummyColumn = 3;
            const PartType dummyPartType = PartType.Leaf;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.LineIndex = dummyLineIndex;
            dummyBlockProcessor.Column = dummyColumn;
            var dummyException = new Exception();
            Block dummyPartBlock = _mockRepository.Create<Block>(null).Object;
            var dummyMultipartBlock = new DummyMultipartBlock(null);
            var dummyOpenMultipartBlocks = new Stack<ContainerBlock>();
            Mock<IMultipartBlockFactory<DummyMultipartBlock>> mockMultipartBlockFactory = _mockRepository.Create<IMultipartBlockFactory<DummyMultipartBlock>>();
            mockMultipartBlockFactory.Setup(m => m.CreatePart(dummyPartType, dummyBlockProcessor)).Returns(dummyPartBlock);
            Mock<ExposedMultipartBlockParser> mockTestSubject = CreateMockExposedMultipartBlockParser(mockMultipartBlockFactory.Object, "dummyName", new PartType[] { dummyPartType });
            mockMultipartBlockFactory.Setup(m => m.Create(dummyBlockProcessor, mockTestSubject.Object)).Returns(dummyMultipartBlock);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(m => m.IsOpeningLine(dummyBlockProcessor)).Returns(true);
            mockTestSubject.Setup(m => m.GetOrCreateOpenMultipartBlocks(dummyBlockProcessor)).Returns(dummyOpenMultipartBlocks);

            // Act and assert
            BlockState result = mockTestSubject.Object.ExposedTryOpenBlock(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.ContinueDiscard, result);
            Stack<Block> newBlocks = dummyBlockProcessor.NewBlocks;
            Assert.Equal(2, newBlocks.Count);
            Assert.Same(dummyMultipartBlock, newBlocks.Pop()); // Has got to be at the top of the stack since part block is its child
            Assert.Same(dummyPartBlock, newBlocks.Pop());
            Assert.Single(dummyOpenMultipartBlocks);
            Assert.Same(dummyMultipartBlock, dummyOpenMultipartBlocks.Pop());
        }

        [Fact]
        public void TryContinueBlock_ReturnsBlockStateContinueIfLineHasCodeIndent()
        {
            // Arrange
            var dummyMultipartBlock = new DummyMultipartBlock(null);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = 4; // Code indent
            Mock<ExposedMultipartBlockParser> mockTestSubject = CreateMockExposedMultipartBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.GetOrCreateOpenMultipartBlocks(dummyBlockProcessor)).Returns((Stack<ContainerBlock>)null);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryContinueBlock(dummyBlockProcessor, dummyMultipartBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.Continue, result);
        }

        [Fact]
        public void TryContinueBlock_ReturnsBlockStateContinueIfBlockIsNotTheMostRecentlyOpenedMultipartBlock()
        {
            // Arrange
            var dummyMultipartBlock = new DummyMultipartBlock(null);
            var dummyOpenMultipartBlocks = new Stack<ContainerBlock>();
            dummyOpenMultipartBlocks.Push(dummyMultipartBlock);
            dummyOpenMultipartBlocks.Push(new DummyMultipartBlock(null));
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<ExposedMultipartBlockParser> mockTestSubject = CreateMockExposedMultipartBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.GetOrCreateOpenMultipartBlocks(dummyBlockProcessor)).Returns(dummyOpenMultipartBlocks);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryContinueBlock(dummyBlockProcessor, dummyMultipartBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.Continue, result);
        }

        [Fact]
        public void TryContinueBlock_ReturnsBlockStateContinueIfLineIsNotAPartDividerLine()
        {
            // Arrange
            var dummyMultipartBlock = new DummyMultipartBlock(null);
            var dummyOpenMultipartBlocks = new Stack<ContainerBlock>();
            dummyOpenMultipartBlocks.Push(dummyMultipartBlock);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<ExposedMultipartBlockParser> mockTestSubject = CreateMockExposedMultipartBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.GetOrCreateOpenMultipartBlocks(dummyBlockProcessor)).Returns(dummyOpenMultipartBlocks);
            mockTestSubject.Setup(t => t.IsPartDividerLine(dummyBlockProcessor)).Returns(false);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryContinueBlock(dummyBlockProcessor, dummyMultipartBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.Continue, result);
        }

        [Fact]
        public void TryContinueBlock_ReturnsBlockStateBreakDiscardUpdatesSpanEndAndUpdatesOpenMultipartBlocksIfMultipartBlockHasExpectedNumberOfParts()
        {
            // Arrange
            const int dummyEnd = 54;
            const int dummyNumParts = 3;
            var dummyMultipartBlock = new DummyMultipartBlock(null);
            for (int i = 0; i < dummyNumParts; i++)
            {
                dummyMultipartBlock.Add(_mockRepository.Create<Block>(null).Object);
            }
            var dummyOpenMultipartBlocks = new Stack<ContainerBlock>();
            dummyOpenMultipartBlocks.Push(dummyMultipartBlock);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(string.Empty, 0, dummyEnd);
            Mock<ExposedMultipartBlockParser> mockTestSubject = CreateMockExposedMultipartBlockParser(partTypes: new PartType[dummyNumParts]);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.GetOrCreateOpenMultipartBlocks(dummyBlockProcessor)).Returns(dummyOpenMultipartBlocks);
            mockTestSubject.Setup(t => t.IsPartDividerLine(dummyBlockProcessor)).Returns(true);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryContinueBlock(dummyBlockProcessor, dummyMultipartBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.BreakDiscard, result);
            Assert.Empty(dummyOpenMultipartBlocks);
            Assert.Equal(dummyEnd, dummyMultipartBlock.Span.End);
        }

        [Fact]
        public void TryContinueBlock_ReturnsBlockStateContinueDiscardUpdatesSpanOfLastChildClosesItAndAddsNewPartToNewBlocksIfANewPartIsOpened()
        {
            // Arrange
            var dummyPartTypes = new PartType[] { PartType.Container, PartType.Leaf };
            const int dummyEnd = 54;
            var dummyMultipartBlock = new DummyMultipartBlock(null);
            Block dummyLastPartBlock = _mockRepository.Create<Block>(null).Object;
            Block dummyNewPartBlock = _mockRepository.Create<Block>(null).Object;
            dummyMultipartBlock.Add(dummyLastPartBlock);
            var dummyOpenMultipartBlocks = new Stack<ContainerBlock>();
            dummyOpenMultipartBlocks.Push(dummyMultipartBlock);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(string.Empty, 0, dummyEnd);
            Mock<IMultipartBlockFactory<DummyMultipartBlock>> mockMultipartBlockFactory = _mockRepository.Create<IMultipartBlockFactory<DummyMultipartBlock>>();
            mockMultipartBlockFactory.Setup(m => m.CreatePart(dummyPartTypes[1], dummyBlockProcessor)).Returns(dummyNewPartBlock);
            Mock<ExposedMultipartBlockParser> mockTestSubject = CreateMockExposedMultipartBlockParser(mockMultipartBlockFactory.Object,
                partTypes: dummyPartTypes);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.GetOrCreateOpenMultipartBlocks(dummyBlockProcessor)).Returns(dummyOpenMultipartBlocks);
            mockTestSubject.Setup(t => t.IsPartDividerLine(dummyBlockProcessor)).Returns(true);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryContinueBlock(dummyBlockProcessor, dummyMultipartBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.ContinueDiscard, result);
            Assert.Equal(dummyEnd, dummyLastPartBlock.Span.End);
            Stack<Block> newBlocks = dummyBlockProcessor.NewBlocks;
            Assert.Single(newBlocks);
            Assert.Same(dummyNewPartBlock, newBlocks.Pop());
        }

        [Fact]
        public void CloseBlock_ThrowsBlockExceptionIfBlockDoesNotHaveTheExpectedNumberOfParts()
        {
            // Arrange
            const int dummyLineIndex = 5;
            const int dummyColumn = 2;
            const int dummyExpectedNumParts = 4;
            const int dummyActualNumParts = 2;
            var dummyMultipartBlock = new DummyMultipartBlock(null)
            {
                Line = dummyLineIndex,
                Column = dummyColumn
            };
            for (int i = 0; i < dummyActualNumParts; i++)
            {
                dummyMultipartBlock.Add(_mockRepository.Create<Block>(null).Object);
            }
            ExposedMultipartBlockParser testSubject = CreateExposedMultipartBlockParser(partTypes: new PartType[dummyExpectedNumParts]);

            // Act and assert
            BlockException result = Assert.Throws<BlockException>(() => testSubject.ExposedCloseBlock(null, dummyMultipartBlock));
            Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock, nameof(DummyMultipartBlock),
                    dummyLineIndex + 1,
                    dummyColumn,
                    string.Format(Strings.BlockException_MultipartBlockParser_IncorrectNumberOfParts, nameof(DummyMultipartBlock), dummyExpectedNumParts, dummyActualNumParts)),
                result.Message);
        }

        [Fact]
        public void CloseBlock_ReturnsTrueIfSuccessful()
        {
            // Arrange
            const int dummyNumParts = 4;
            var dummyMultipartBlock = new DummyMultipartBlock(null);
            for (int i = 0; i < dummyNumParts; i++)
            {
                dummyMultipartBlock.Add(_mockRepository.Create<Block>(null).Object);
            }
            ExposedMultipartBlockParser testSubject = CreateExposedMultipartBlockParser(partTypes: new PartType[dummyNumParts]);

            // Act
            bool result = testSubject.ExposedCloseBlock(null, dummyMultipartBlock);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [MemberData(nameof(IsPartDividerLine_ReturnsTrueIfLineIsPartDividerLineOtherwiseReturnsFalse_Data))]
        public void IsPartDividerLine_ReturnsTrueIfLineIsPartDividerLineOtherwiseReturnsFalse(string dummyText, bool expectedResult)
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyText);
            ExposedMultipartBlockParser testSubject = CreateExposedMultipartBlockParser();

            // Act
            bool result = testSubject.IsPartDividerLine(dummyBlockProcessor);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> IsPartDividerLine_ReturnsTrueIfLineIsPartDividerLineOtherwiseReturnsFalse_Data()
        {
            return new object[][]
            {
                new object[]{"+++", true},
                // Trailing whitespace
                new object[]{"+++  ", true},
                // Does not start with three '+'s
                new object[]{"++-", false},
                new object[]{"-++", false},
                new object[]{"+-+", false},
                new object[]{" +++", false},
                // Non-whitespace trailing characters
                new object[]{"+++- ", false},
                new object[]{"+++ -", false}
            };
        }

        [Theory]
        [MemberData(nameof(IsOpeningLine_ReturnsTrueIfLineIsOpeningLineOtherwiseReturnsFalse_Data))]
        public void IsOpeningLine_ReturnsTrueIfLineIsOpeningLineOtherwiseReturnsFalse(string dummyName, string dummyText, bool expectedResult)
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyText);
            ExposedMultipartBlockParser testSubject = CreateExposedMultipartBlockParser(name: dummyName);

            // Act
            bool result = testSubject.IsOpeningLine(dummyBlockProcessor);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> IsOpeningLine_ReturnsTrueIfLineIsOpeningLineOtherwiseReturnsFalse_Data()
        {
            const string dummyName = "dummyName";

            return new object[][]
            {
                new object[]{dummyName, $"+++ {dummyName}", true},
                // Trailing whitespace
                new object[]{dummyName, $"+++ {dummyName}  ", true},
                // Does not start with "+++ <name>" - note, first char is always '+' since IsOpeningLine is called from TryOpenBlock
                new object[]{dummyName, $"+-+ {dummyName}  ", false},
                new object[]{dummyName, $"++- {dummyName}  ", false},
                new object[]{dummyName, $"+++-{dummyName}  ", false},
                new object[]{dummyName, "+++ fakeName", false},
                new object[]{dummyName, "+++ dummyNam", false},
                // Non-whitespace trailing characters
                new object[]{dummyName, $"+++ {dummyName}-", false},
                new object[]{dummyName, $"+++ {dummyName} -", false},
            };
        }

        [Fact]
        public void GetOrCreateOpenMultipartBlocks_GetsOpenMultipartBlocksIfItAlreadyExists()
        {
            // Arrange
            ExposedMultipartBlockParser testSubject = CreateExposedMultipartBlockParser();
            var dummyOpenMultipartBlocks = new Stack<ContainerBlock>();
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Document.SetData(testSubject.OPEN_MULTIPART_BLOCKS_KEY, dummyOpenMultipartBlocks);

            // Act
            Stack<ContainerBlock> result = testSubject.GetOrCreateOpenMultipartBlocks(dummyBlockProcessor);

            // Assert
            Assert.Same(dummyOpenMultipartBlocks, result);
        }

        [Fact]
        public void GetOrCreateOpenMultipartBlocks_CreatesOpenMultipartBlocksIfItDoesNotAlreadyExist()
        {
            // Arrange
            ExposedMultipartBlockParser testSubject = CreateExposedMultipartBlockParser();
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();

            // Act
            Stack<ContainerBlock> result = testSubject.GetOrCreateOpenMultipartBlocks(dummyBlockProcessor);

            // Assert
            Assert.NotNull(result);
            Assert.Same(result, dummyBlockProcessor.Document.GetData(testSubject.OPEN_MULTIPART_BLOCKS_KEY));
        }

        private Mock<ExposedMultipartBlockParser> CreateMockExposedMultipartBlockParser(IMultipartBlockFactory<DummyMultipartBlock> multipartBlockFactory = default,
            string name = default,
            PartType[] partTypes = default)
        {
            return _mockRepository.Create<ExposedMultipartBlockParser>(multipartBlockFactory ?? _mockRepository.Create<IMultipartBlockFactory<DummyMultipartBlock>>().Object,
                name ?? "dummyName",
                partTypes ?? new PartType[] { PartType.Leaf });
        }

        private ExposedMultipartBlockParser CreateExposedMultipartBlockParser(IMultipartBlockFactory<DummyMultipartBlock> multipartBlockFactory = default,
            string name = default,
            PartType[] partTypes = default)
        {
            return new ExposedMultipartBlockParser(multipartBlockFactory ?? _mockRepository.Create<IMultipartBlockFactory<DummyMultipartBlock>>().Object,
                name ?? "dummyName",
                partTypes ?? new PartType[] { PartType.Leaf });
        }

        public class ExposedMultipartBlockParser : MultipartBlockParser<DummyMultipartBlock>
        {
            public ExposedMultipartBlockParser(IMultipartBlockFactory<DummyMultipartBlock> multipartBlockFactory, string name, PartType[] partTypes) :
                base(multipartBlockFactory, name, partTypes)
            {
            }

            public BlockState ExposedTryOpenBlock(BlockProcessor blockProcessor)
            {
                return TryOpenBlock(blockProcessor);
            }

            public BlockState ExposedTryContinueBlock(BlockProcessor blockProcessor, DummyMultipartBlock dummyMultipartBlock)
            {
                return TryContinueBlock(blockProcessor, dummyMultipartBlock);
            }

            public bool ExposedCloseBlock(BlockProcessor blockProcessor, DummyMultipartBlock dummyMultipartBlock)
            {
                return CloseBlock(blockProcessor, dummyMultipartBlock);
            }
        }

        public class DummyMultipartBlock : ContainerBlock
        {
            public DummyMultipartBlock(BlockParser parser) : base(parser)
            {
            }
        }
    }
}
