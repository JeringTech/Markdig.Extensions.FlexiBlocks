using Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Moq;
using Moq.Protected;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiTableBlocks
{
    public class BasicFlexiTableBlockParserUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_SetsOpeningCharacters()
        {
            // Act
            var testSubject = new BasicFlexiTableBlockParser(_mockRepository.Create<IFlexiTableBlockFactory>().Object);

            // Assert
            Assert.Single(testSubject.OpeningCharacters);
            Assert.Equal('|', testSubject.OpeningCharacters[0]);
        }

        [Fact]
        public void TryOpenBlock_ReturnsBlockStateNoneIfCurrentLineHasCodeIndent()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = 4; // IsCodeIndent is an expression bodied member that derives its value from Column
            ExposedBasicFlexiTableBlockParser testSubject = CreateExposedBasicFlexiTableBlockParser();

            // Act
            BlockState result = testSubject.ExposedTryOpenBlock(dummyBlockProcessor);

            // Assert
            Assert.Equal(BlockState.None, result);
        }

        [Fact]
        public void TryOpenBlock_ReturnsBlockStateNoneIfCurrentLineIsNeitherAColumnDefinitionsLineNorARowLine()
        {
            // Arrange
            const string dummyText = "dummyText";
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyText);
            Mock<ExposedBasicFlexiTableBlockParser> mockTestSubject = CreateMockExposedBasicFlexiTableBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.
                Protected().
                Setup<List<ColumnDefinition>>("TryParseColumnDefinitionsLine", ItExpr.Is<StringSlice>(line => line.Text == dummyText), '|', -1).
                Returns((List<ColumnDefinition>)null);
            mockTestSubject.Setup(t => t.TryParseRowLine(dummyBlockProcessor, 0, -1)).Returns((Row)null);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryOpenBlock(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.None, result);
        }

        [Fact]
        public void TryOpenBlock_ReturnsBlockStateContinueDiscardIfCurrentLineIsAColumnDefinitionsLine()
        {
            // Arrange
            const int dummyNumColumnDefinitions = 6;
            const string dummyText = "dummyText";
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyText);
            var dummyColumnDefinitions = new List<ColumnDefinition>();
            for (int i = 0; i < dummyNumColumnDefinitions; i++)
            {
                dummyColumnDefinitions.Add(new ColumnDefinition(ContentAlignment.None, 0, 1));
            }
            var dummyProxyTableBlock = new ProxyTableBlock(null);
            Mock<IFlexiTableBlockFactory> mockFlexiTableBlockFactory = _mockRepository.Create<IFlexiTableBlockFactory>();
            Mock<ExposedBasicFlexiTableBlockParser> mockTestSubject = CreateMockExposedBasicFlexiTableBlockParser(mockFlexiTableBlockFactory.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.
                Protected().
                Setup<List<ColumnDefinition>>("TryParseColumnDefinitionsLine", ItExpr.Is<StringSlice>(line => line.Text == dummyText), '|', -1).
                Returns(dummyColumnDefinitions);
            mockFlexiTableBlockFactory.Setup(f => f.CreateProxy(dummyBlockProcessor, mockTestSubject.Object)).Returns(dummyProxyTableBlock);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryOpenBlock(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.ContinueDiscard, result);
            Assert.Single(dummyBlockProcessor.NewBlocks);
            var resultProxyTableBlock = dummyBlockProcessor.NewBlocks.Pop() as ProxyTableBlock;
            Assert.Same(dummyProxyTableBlock, resultProxyTableBlock);
            Assert.Same(dummyColumnDefinitions, resultProxyTableBlock.ColumnDefinitions);
            Assert.Equal(dummyNumColumnDefinitions, resultProxyTableBlock.NumColumns);
            Assert.Equal(dummyText, resultProxyTableBlock.Lines.ToString());
        }

        [Fact]
        public void TryOpenBlock_ReturnsBlockStateContinueDiscardIfCurrentLineIsARowLine()
        {
            // Arrange
            const int dummyNumCells = 6;
            const string dummyText = "dummyText";
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyText);
            var dummyProxyTableBlock = new ProxyTableBlock(null);
            Mock<IFlexiTableBlockFactory> mockFlexiTableBlockFactory = _mockRepository.Create<IFlexiTableBlockFactory>();
            var dummyRow = new Row();
            for (int i = 0; i < dummyNumCells; i++)
            {
                dummyRow.Add(CreateCell());
            }
            Mock<ExposedBasicFlexiTableBlockParser> mockTestSubject = CreateMockExposedBasicFlexiTableBlockParser(mockFlexiTableBlockFactory.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.
                Protected().
                Setup<List<ColumnDefinition>>("TryParseColumnDefinitionsLine", ItExpr.Is<StringSlice>(line => line.Text == dummyText), '|', -1).
                Returns((List<ColumnDefinition>)null);
            mockTestSubject.Setup(t => t.TryParseRowLine(dummyBlockProcessor, 0, -1)).Returns(dummyRow);
            mockFlexiTableBlockFactory.Setup(f => f.CreateProxy(dummyBlockProcessor, mockTestSubject.Object)).Returns(dummyProxyTableBlock);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryOpenBlock(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.ContinueDiscard, result);
            Assert.Single(dummyBlockProcessor.NewBlocks);
            var resultProxyTableBlock = dummyBlockProcessor.NewBlocks.Pop() as ProxyTableBlock;
            Assert.Same(dummyProxyTableBlock, resultProxyTableBlock);
            Assert.Equal(dummyNumCells, resultProxyTableBlock.NumColumns);
            Assert.Equal(dummyText, resultProxyTableBlock.Lines.ToString());
            Assert.Single(dummyProxyTableBlock.Rows);
            Assert.Same(dummyRow, dummyProxyTableBlock.Rows[0]);
        }

        [Fact]
        public void TryContinueBlock_ReturnsBlockStateContinueDiscardIfColumnDefinitionsHasNotBeenDefinedAndTheCurrentLineIsAValidColumnDefinitionsLinee()
        {
            // Arrange
            const string dummyText = "| dummyText";
            const int dummyNumColumns = 8;
            var dummyProxyTableBlock = new ProxyTableBlock(null);
            for (int i = 0; i < 2; i++)
            {
                dummyProxyTableBlock.Rows.Add(new Row());
            }
            dummyProxyTableBlock.NumColumns = dummyNumColumns;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyText);
            var dummyColumnDefinitions = new List<ColumnDefinition>();
            Mock<ExposedBasicFlexiTableBlockParser> mockTestSubject = CreateMockExposedBasicFlexiTableBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.
                Protected().
                Setup<List<ColumnDefinition>>("TryParseColumnDefinitionsLine", ItExpr.Is<StringSlice>(line => line.Text == dummyText), '|', dummyNumColumns).
                Returns(dummyColumnDefinitions);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryContinueBlock(dummyBlockProcessor, dummyProxyTableBlock);

            // Assert
            _mockRepository.VerifyAll();
            foreach (Row row in dummyProxyTableBlock.Rows)
            {
                Assert.True(row.IsHeaderRow);
            }
            Assert.Same(dummyColumnDefinitions, dummyProxyTableBlock.ColumnDefinitions);
            Assert.Equal(dummyText.Length - 1, dummyProxyTableBlock.Span.End); // Block span end is updated
            Assert.Equal(dummyText, dummyProxyTableBlock.Lines.ToString());
            Assert.Equal(BlockState.ContinueDiscard, result);
        }

        [Fact]
        public void TryContinueBlock_ReturnsBlockStateContinueDiscardIfColumnDefinitionsHasNotBeenDefinedTheCurrentLineIsNotAValidColumnDefinitionsLineButItIsAValidRowLine()
        {
            // Arrange
            const string dummyText = "| dummyText";
            const int dummyNumColumns = 8;
            const int dummyNumRows = 4;
            var dummyProxyTableBlock = new ProxyTableBlock(null);
            for (int i = 0; i < dummyNumRows; i++)
            {
                dummyProxyTableBlock.Rows.Add(new Row());
            }
            dummyProxyTableBlock.NumColumns = dummyNumColumns;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyText);
            var dummyRow = new Row();
            Mock<ExposedBasicFlexiTableBlockParser> mockTestSubject = CreateMockExposedBasicFlexiTableBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.
                Protected().
                Setup<List<ColumnDefinition>>("TryParseColumnDefinitionsLine", ItExpr.Is<StringSlice>(line => line.Text == dummyText), '|', dummyNumColumns).
                Returns((List<ColumnDefinition>)null);
            mockTestSubject.Setup(t => t.TryParseRowLine(dummyBlockProcessor, dummyNumRows, dummyNumColumns)).Returns(dummyRow);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryContinueBlock(dummyBlockProcessor, dummyProxyTableBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyProxyTableBlock.Span.End, dummyText.Length - 1); // Block span end is updated
            Assert.Same(dummyProxyTableBlock.Rows.Last(), dummyRow);
            Assert.Equal(dummyText, dummyProxyTableBlock.Lines.ToString());
            Assert.Equal(BlockState.ContinueDiscard, result);
        }

        [Fact]
        public void TryContinueBlock_ReturnsBlockStateContinueDiscardIfColumnDefinitionsHasBeenDefinedAndTheCurrentLineIsAValidRowLine()
        {
            // Arrange
            const string dummyText = "| dummyText";
            const int dummyNumColumns = 8;
            const int dummyNumRows = 4;
            var dummyColumnDefinitions = new List<ColumnDefinition>();
            var dummyProxyTableBlock = new ProxyTableBlock(null);
            for (int i = 0; i < dummyNumRows; i++)
            {
                dummyProxyTableBlock.Rows.Add(new Row());
            }
            dummyProxyTableBlock.NumColumns = dummyNumColumns;
            dummyProxyTableBlock.ColumnDefinitions = dummyColumnDefinitions;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyText);
            var dummyRow = new Row();
            Mock<ExposedBasicFlexiTableBlockParser> mockTestSubject = CreateMockExposedBasicFlexiTableBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.TryParseRowLine(dummyBlockProcessor, dummyNumRows, dummyNumColumns)).Returns(dummyRow);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryContinueBlock(dummyBlockProcessor, dummyProxyTableBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyProxyTableBlock.Span.End, dummyText.Length - 1); // Block span end is updated
            Assert.Same(dummyProxyTableBlock.Rows.Last(), dummyRow);
            Assert.Equal(dummyText, dummyProxyTableBlock.Lines.ToString());
            Assert.Equal(BlockState.ContinueDiscard, result);
        }

        [Fact]
        public void TryContinueBlock_UndoesProxyTableBlockAndReturnsBlockStateBreakIfLineIsNeitherAValidColumnDefinitionsLineNorAValidRowLine()
        {
            // Arrange
            const string dummyText = "| dummyText";
            const int dummyNumColumns = 8;
            const int dummyNumRows = 4;
            var dummyProxyTableBlock = new ProxyTableBlock(null);
            for (int i = 0; i < dummyNumRows; i++)
            {
                dummyProxyTableBlock.Rows.Add(new Row());
            }
            dummyProxyTableBlock.NumColumns = dummyNumColumns;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyText);
            Mock<ExposedBasicFlexiTableBlockParser> mockTestSubject = CreateMockExposedBasicFlexiTableBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.TryParseRowLine(dummyBlockProcessor, dummyNumRows, dummyNumColumns)).Returns((Row)null);
            mockTestSubject.
                Protected().
                Setup<List<ColumnDefinition>>("TryParseColumnDefinitionsLine", ItExpr.Is<StringSlice>(line => line.Text == dummyText), '|', dummyNumColumns).
                Returns((List<ColumnDefinition>)null);
            mockTestSubject.Protected().Setup("Undo", dummyBlockProcessor, dummyProxyTableBlock);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryContinueBlock(dummyBlockProcessor, dummyProxyTableBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.Break, result);
        }

        [Fact]
        public void TryContinueBlock_UndoesProxyTableBlockAndReturnsBlockStateBreakIfColumnDefinitionsIsAlreadyDefinedAndLineIsNotAValidRowLine()
        {
            // Arrange
            const int dummyNumColumns = 8;
            const int dummyNumRows = 4;
            var dummyColumnDefinitions = new List<ColumnDefinition>();
            var dummyProxyTableBlock = new ProxyTableBlock(null);
            for (int i = 0; i < dummyNumRows; i++)
            {
                dummyProxyTableBlock.Rows.Add(new Row());
            }
            dummyProxyTableBlock.NumColumns = dummyNumColumns;
            dummyProxyTableBlock.ColumnDefinitions = dummyColumnDefinitions;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice("|");
            Mock<ExposedBasicFlexiTableBlockParser> mockTestSubject = CreateMockExposedBasicFlexiTableBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.TryParseRowLine(dummyBlockProcessor, dummyNumRows, dummyNumColumns)).Returns((Row)null);
            mockTestSubject.Protected().Setup("Undo", dummyBlockProcessor, dummyProxyTableBlock);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryContinueBlock(dummyBlockProcessor, dummyProxyTableBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.Break, result);
        }

        [Fact]
        public void TryContinueBlock_ReturnsBlockStateBreakIfTheCurrentLineDoesNotStartWithAVerticalBar()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice("dummyText");
            ExposedBasicFlexiTableBlockParser testSubject = CreateExposedBasicFlexiTableBlockParser();

            // Act
            BlockState result = testSubject.ExposedTryContinueBlock(dummyBlockProcessor, null);

            // Assert
            Assert.Equal(BlockState.Break, result);
        }

        [Theory]
        [MemberData(nameof(TryParseRowLine_TriesToParseRowLine_Data))]
        public void TryParseRowLine_TriesToParseRowLine(string dummyText,
            int dummyLineIndex,
            int dummyRowIndex,
            int dummyNumCells,
            Row expectedRow)
        {
            // Arrange
            var dummyLine = new StringSlice(dummyText);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyLine;
            dummyBlockProcessor.LineIndex = dummyLineIndex;
            BasicFlexiTableBlockParser testSubject = CreateBasicFlexiTableBlockParser();

            // Act
            Row result = testSubject.TryParseRowLine(dummyBlockProcessor, dummyRowIndex, dummyNumCells);

            // Assert
            Assert.Equal(expectedRow, result);
        }

        public static IEnumerable<object[]> TryParseRowLine_TriesToParseRowLine_Data()
        {
            return new object[][]
            {
                // Creates cells if line is valid
                new object[]
                {
                    "|a|aaa|aaaaa|",
                    0,
                    0,
                    3,
                    new Row()
                    {
                        CreateCell(endOffset: 2, lines: new StringLineGroup("a")),
                        CreateCell(1, 1, startOffset: 2, endOffset: 6, lines: new StringLineGroup("aaa")),
                        CreateCell(2, 2, startOffset: 6, endOffset: 12, lines: new StringLineGroup("aaaaa"))
                    }
                },
                // Creates cells if line is valid - line with trailing whitespace
                new object[]
                {
                    "|a|aaa|aaaaa|    ",
                    0,
                    0,
                    -1,
                    new Row()
                    {
                        CreateCell(endOffset: 2, lines: new StringLineGroup("a")),
                        CreateCell(1, 1, startOffset: 2, endOffset: 6, lines: new StringLineGroup("aaa")),
                        CreateCell(2, 2, startOffset: 6, endOffset: 12, lines: new StringLineGroup("aaaaa"))
                    }
                },
                // Returns null if ending '|' is missing
                new object[]
                {
                    "|a|aaa|aaaaa",
                    0,
                    0,
                    -1,
                    null
                },
                // Returns null if there are no cells
                new object[]
                {
                    "|",
                    0,
                    0,
                    -1,
                    null
                },
                // Returns null if row has too many cells
                new object[]
                {
                    "|a|aaa|aaaaa|",
                    0,
                    0,
                    2,
                    null
                },
                // Returns null if row has too few cells
                new object[]
                {
                    "|a|aaa|aaaaa|",
                    0,
                    0,
                    4,
                    null
                },
                // Line with escaped '|'s
                new object[]
                {
                    "|\\||\\|\\||\\|\\|\\||",
                    0,
                    0,
                    3,
                    new Row()
                    {
                        CreateCell(endOffset: 3, lines: new StringLineGroup("\\|")),
                        CreateCell(1, 1, startOffset: 3, endOffset: 8, lines: new StringLineGroup("\\|\\|")),
                        CreateCell(2, 2, startOffset: 8, endOffset: 15, lines: new StringLineGroup("\\|\\|\\|"))
                    }
                },
                // LineIndex and RowIndex are assigned to all cells
                new object[]
                {
                    "|a|aaa|aaaaa|",
                    4,
                    6,
                    3,
                    new Row()
                    {
                        CreateCell(startRowIndex: 6, endRowIndex: 6, endOffset: 2, lineIndex: 4, lines: new StringLineGroup("a")),
                        CreateCell(1, 1, 6, 6, 2, 6, 4, new StringLineGroup("aaa")),
                        CreateCell(2, 2, 6, 6, 6, 12, 4, lines: new StringLineGroup("aaaaa"))
                    }
                },
            };
        }

        private BasicFlexiTableBlockParser CreateBasicFlexiTableBlockParser(IFlexiTableBlockFactory flexiTableBlockFactory = null)
        {
            return new BasicFlexiTableBlockParser(flexiTableBlockFactory ?? _mockRepository.Create<IFlexiTableBlockFactory>().Object);
        }

        private static Cell CreateCell(int startColumnIndex = 0,
            int endColumnIndex = 0,
            int startRowIndex = 0,
            int endRowIndex = 0,
            int startOffset = 0,
            int endOffset = 1,
            int lineIndex = 0,
            StringLineGroup lines = default)
        {
            return new Cell(startColumnIndex, endColumnIndex, startRowIndex, endRowIndex, startOffset, endOffset, lineIndex) { Lines = lines };
        }

        public class ExposedBasicFlexiTableBlockParser : BasicFlexiTableBlockParser
        {
            public ExposedBasicFlexiTableBlockParser(IFlexiTableBlockFactory flexiTableBlockFactory) : base(flexiTableBlockFactory)
            {
            }

            public BlockState ExposedTryOpenBlock(BlockProcessor blockProcessor)
            {
                return TryOpenBlock(blockProcessor);
            }

            public BlockState ExposedTryContinueBlock(BlockProcessor blockProcessor, ProxyTableBlock proxyTableBlock)
            {
                return TryContinueBlock(blockProcessor, proxyTableBlock);
            }
        }

        private Mock<ExposedBasicFlexiTableBlockParser> CreateMockExposedBasicFlexiTableBlockParser(IFlexiTableBlockFactory flexiTableBlockFactory = null)
        {
            return _mockRepository.Create<ExposedBasicFlexiTableBlockParser>(flexiTableBlockFactory ?? _mockRepository.Create<IFlexiTableBlockFactory>().Object);
        }

        private ExposedBasicFlexiTableBlockParser CreateExposedBasicFlexiTableBlockParser(IFlexiTableBlockFactory flexiTableBlockFactory = null)
        {
            return new ExposedBasicFlexiTableBlockParser(flexiTableBlockFactory ?? _mockRepository.Create<IFlexiTableBlockFactory>().Object);
        }
    }
}
