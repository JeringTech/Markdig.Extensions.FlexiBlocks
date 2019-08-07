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
    public class AdvancedFlexiTableBlockParserUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_SetsOpeningCharacters()
        {
            // Act
            var testSubject = new AdvancedFlexiTableBlockParser(_mockRepository.Create<IFlexiTableBlockFactory>().Object);

            // Assert
            Assert.Single(testSubject.OpeningCharacters);
            Assert.Equal('+', testSubject.OpeningCharacters[0]);
        }

        [Fact]
        public void TryOpenBlock_ReturnsBlockStateNoneIfCurrentLineHasCodeIndent()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = 4; // IsCodeIndent is an expression bodied member that derives its value from Column
            ExposedAdvancedFlexiTableBlockParser testSubject = CreateExposedAdvancedFlexiTableBlockParser();

            // Act
            BlockState result = testSubject.ExposedTryOpenBlock(dummyBlockProcessor);

            // Assert
            Assert.Equal(BlockState.None, result);
        }

        [Fact]
        public void TryOpenBlock_ReturnsBlockStateNoneIfCurrentLineIsNotAColumnDefinitionsLine()
        {
            // Arrange
            const string dummyText = "dummyText";
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyText);
            Mock<ExposedAdvancedFlexiTableBlockParser> mockTestSubject = CreateMockExposedAdvancedFlexiTableBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.
                Protected().
                Setup<List<ColumnDefinition>>("TryParseColumnDefinitionsLine", ItExpr.Is<StringSlice>(line => line.Text == dummyText), '+', -1).
                Returns((List<ColumnDefinition>)null);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryOpenBlock(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.None, result);
        }

        [Fact]
        public void TryOpenBlock_CreatesProxyTableBlockAndReturnsBlockStateContinueDiscardIfSuccessful()
        {
            // Arrange
            const int dummyNumColumns = 5;
            const string dummyText = "dummyText";
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyText);
            Mock<IFlexiTableBlockFactory> mockFlexiTableBlockFactory = _mockRepository.Create<IFlexiTableBlockFactory>();
            var dummyColumnDefinitions = new List<ColumnDefinition>();
            for (int i = 0; i < dummyNumColumns; i++)
            {
                dummyColumnDefinitions.Add(new ColumnDefinition(ContentAlignment.None, 0, 1));
            }
            var dummyProxyTableBlock = new ProxyTableBlock(null);
            Mock<ExposedAdvancedFlexiTableBlockParser> mockTestSubject = CreateMockExposedAdvancedFlexiTableBlockParser(mockFlexiTableBlockFactory.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.
                Protected().
                Setup<List<ColumnDefinition>>("TryParseColumnDefinitionsLine", ItExpr.Is<StringSlice>(line => line.Text == dummyText), '+', -1).
                Returns(dummyColumnDefinitions);
            mockFlexiTableBlockFactory.
                Setup(f => f.CreateProxy(dummyBlockProcessor, mockTestSubject.Object)).
                Returns(dummyProxyTableBlock);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryOpenBlock(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.ContinueDiscard, result);
            Assert.Single(dummyBlockProcessor.NewBlocks);
            var resultProxyTableBlock = dummyBlockProcessor.NewBlocks.Pop() as ProxyTableBlock;
            Assert.Same(dummyProxyTableBlock, resultProxyTableBlock);
            Assert.Same(dummyColumnDefinitions, resultProxyTableBlock.ColumnDefinitions);
            Assert.Equal(dummyNumColumns, resultProxyTableBlock.NumColumns);
            Assert.Equal(dummyText, resultProxyTableBlock.Lines.ToString());
        }

        [Fact]
        public void TryContinueBlock_ReturnsBlockStateBreakIfCurrentCharIsNotAVerticalBarOrPlus()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice("dummyText");
            ExposedAdvancedFlexiTableBlockParser testSubject = CreateExposedAdvancedFlexiTableBlockParser();

            // Act
            BlockState result = testSubject.ExposedTryContinueBlock(dummyBlockProcessor, null);

            // Assert
            Assert.Equal(BlockState.Break, result);
        }

        [Fact]
        public void TryContinueBlock_UndoesProxyTableBlockAndReturnsBlockStateBreakIfIfCurrentCharIsVerticalBarButLineIsNotAContentLine()
        {
            // Arrange
            var dummyProxyTableBlock = new ProxyTableBlock(null);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice("|");
            Mock<ExposedAdvancedFlexiTableBlockParser> mockTestSubject = CreateMockExposedAdvancedFlexiTableBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.TryParseContentLine(dummyBlockProcessor, dummyProxyTableBlock)).Returns(false);
            mockTestSubject.Protected().Setup("Undo", dummyBlockProcessor, dummyProxyTableBlock);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryContinueBlock(dummyBlockProcessor, dummyProxyTableBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.Break, result);
        }

        [Fact]
        public void TryContinueBlock_UndoesProxyTableBlockAndReturnsBlockStateBreakIfIfCurrentCharIsPlusButLineIsNotASeparatorLine()
        {
            // Arrange
            const string dummyText = "+ dummyText";
            var dummyProxyTableBlock = new ProxyTableBlock(null);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyText);
            Mock<ExposedAdvancedFlexiTableBlockParser> mockTestSubject = CreateMockExposedAdvancedFlexiTableBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.TryParseSeparatorLine(It.Is<StringSlice>(line => line.Text == dummyText), dummyProxyTableBlock)).Returns(false);
            mockTestSubject.Protected().Setup("Undo", dummyBlockProcessor, dummyProxyTableBlock);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryContinueBlock(dummyBlockProcessor, dummyProxyTableBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.Break, result);
        }

        [Fact]
        public void TryContinueBlock_ReturnsBlockStateContinueDiscardIfCurrentCharIsVerticalBarAndLineIsAContentLine()
        {
            // Arrange
            const string dummyText = "| dummyText";
            var dummyProxyTableBlock = new ProxyTableBlock(null);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyText);
            Mock<ExposedAdvancedFlexiTableBlockParser> mockTestSubject = CreateMockExposedAdvancedFlexiTableBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.TryParseContentLine(dummyBlockProcessor, dummyProxyTableBlock)).Returns(true);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryContinueBlock(dummyBlockProcessor, dummyProxyTableBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.ContinueDiscard, result);
            Assert.Equal(dummyText, dummyProxyTableBlock.Lines.ToString());
            Assert.Equal(dummyText.Length - 1, dummyProxyTableBlock.Span.End); // Block span end is updated
        }

        [Fact]
        public void TryContinueBlock_ReturnsBlockStateContinueDiscardIfCurrentCharIsPlusAndLineIsASeparatorLine()
        {
            // Arrange
            const string dummyText = "+ dummyText";
            var dummyProxyTableBlock = new ProxyTableBlock(null);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyText);
            Mock<ExposedAdvancedFlexiTableBlockParser> mockTestSubject = CreateMockExposedAdvancedFlexiTableBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.TryParseSeparatorLine(It.Is<StringSlice>(line => line.Text == dummyText), dummyProxyTableBlock)).Returns(true);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryContinueBlock(dummyBlockProcessor, dummyProxyTableBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.ContinueDiscard, result);
            Assert.Equal(dummyText, dummyProxyTableBlock.Lines.ToString());
            Assert.Equal(dummyText.Length - 1, dummyProxyTableBlock.Span.End); // Block span end is updated
        }

        [Fact]
        public void TryParseContentLine_ReturnsFalseIfCurrentRowIsOpenButCurrentLinesCellsAreUnaligned()
        {
            // Arrange
            const string dummyText = "dummyText";
            var dummyColumnDefinitions = new List<ColumnDefinition>();
            var dummyCurrentRow = new Row();
            var dummyProxyTableBlock = new ProxyTableBlock(null);
            dummyProxyTableBlock.ColumnDefinitions = dummyColumnDefinitions;
            dummyProxyTableBlock.Rows.Add(dummyCurrentRow);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyText);
            Mock<AdvancedFlexiTableBlockParser> mockTestSubject = CreateMockAdvancedFlexiTableBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.ValidateCellAlignment(It.Is<StringSlice>(line => line.Text == dummyText),
                    It.Is<List<ColumnDefinition>>(columnDefinitions => columnDefinitions == dummyColumnDefinitions),
                    It.Is<Row>(row => row == dummyCurrentRow))).
                Returns(false);

            // Act
            bool result = mockTestSubject.Object.TryParseContentLine(dummyBlockProcessor, dummyProxyTableBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.False(result);
        }

        [Fact]
        public void TryParseContentLine_ReturnsFalseIfCurrentRowIsClosedAndCurrentLineIsNotARowLine()
        {
            // Arrange
            const string dummyText = "dummyText";
            var dummyColumnDefinitions = new List<ColumnDefinition>();
            var dummyCurrentRow = new Row();
            dummyCurrentRow.IsOpen = false;
            var dummyProxyTableBlock = new ProxyTableBlock(null);
            dummyProxyTableBlock.ColumnDefinitions = dummyColumnDefinitions;
            dummyProxyTableBlock.Rows.Add(dummyCurrentRow);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyText);
            Mock<AdvancedFlexiTableBlockParser> mockTestSubject = CreateMockAdvancedFlexiTableBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.
                Setup(t => t.TryCreateRow(It.Is<StringSlice>(line => line.Text == dummyText),
                    It.Is<List<ColumnDefinition>>(columnDefinitions => columnDefinitions == dummyColumnDefinitions),
                    It.Is<Row>(row => row == dummyCurrentRow),
                    1,
                    dummyBlockProcessor)).
                Returns((Row)null);

            // Act
            bool result = mockTestSubject.Object.TryParseContentLine(dummyBlockProcessor, dummyProxyTableBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.False(result);
        }

        [Fact]
        public void TryParseContentLine_ReturnsTrueIfCurrentRowIsOpenAndCurrentLinesCellsAreAligned()
        {
            // Arrange
            const string dummyText = "dummyText";
            var dummyColumnDefinitions = new List<ColumnDefinition>();
            var dummyCurrentRow = new Row();
            var dummyProxyTableBlock = new ProxyTableBlock(null);
            dummyProxyTableBlock.ColumnDefinitions = dummyColumnDefinitions;
            dummyProxyTableBlock.Rows.Add(dummyCurrentRow);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyText);
            Mock<AdvancedFlexiTableBlockParser> mockTestSubject = CreateMockAdvancedFlexiTableBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.ValidateCellAlignment(It.Is<StringSlice>(line => line.Text == dummyText),
                    It.Is<List<ColumnDefinition>>(columnDefinitions => columnDefinitions == dummyColumnDefinitions),
                    It.Is<Row>(row => row == dummyCurrentRow))).
                Returns(true);
            mockTestSubject.Protected().Setup("ExtractContent",
                ItExpr.Is<StringSlice>(line => line.Text == dummyText),
                ItExpr.Is<Row>(row => row == dummyCurrentRow));

            // Act
            bool result = mockTestSubject.Object.TryParseContentLine(dummyBlockProcessor, dummyProxyTableBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.True(result);
        }

        [Fact]
        public void TryParseContentLine_ReturnsTrueIfCurrentRowIsClosedAndCurrentLineIsARowLine()
        {
            // Arrange
            const string dummyText = "dummyText";
            var dummyColumnDefinitions = new List<ColumnDefinition>();
            var dummyCurrentRow = new Row();
            var dummyNewRow = new Row();
            dummyCurrentRow.IsOpen = false;
            var dummyProxyTableBlock = new ProxyTableBlock(null);
            dummyProxyTableBlock.ColumnDefinitions = dummyColumnDefinitions;
            dummyProxyTableBlock.Rows.Add(dummyCurrentRow);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyText);
            Mock<AdvancedFlexiTableBlockParser> mockTestSubject = CreateMockAdvancedFlexiTableBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.
                Setup(t => t.TryCreateRow(It.Is<StringSlice>(line => line.Text == dummyText),
                    It.Is<List<ColumnDefinition>>(columnDefinitions => columnDefinitions == dummyColumnDefinitions),
                    It.Is<Row>(row => row == dummyCurrentRow),
                    1,
                    dummyBlockProcessor)).
                Returns(dummyNewRow);
            mockTestSubject.Protected().Setup("ExtractContent",
                ItExpr.Is<StringSlice>(line => line.Text == dummyText),
                ItExpr.Is<Row>(row => row == dummyNewRow));

            // Act
            bool result = mockTestSubject.Object.TryParseContentLine(dummyBlockProcessor, dummyProxyTableBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.True(result);
            Assert.Equal(2, dummyProxyTableBlock.Rows.Count);
            Assert.Same(dummyNewRow, dummyProxyTableBlock.Rows[1]);
        }

        [Theory]
        [MemberData(nameof(ValidateCellAlignment_ValidatesCellAlignment_Data))]
        public void ValidateCellAlignment_ValidatesCellAlignment(string dummyText,
            List<ColumnDefinition> dummyColumnDefinitions,
            Row dummyCurrentRow,
            bool expectedResult)
        {
            // Arrange
            var dummyLine = new StringSlice(dummyText);
            AdvancedFlexiTableBlockParser testSubject = CreateAdvancedFlexiTableBlockParser();

            // Act
            bool result = testSubject.ValidateCellAlignment(dummyLine, dummyColumnDefinitions, dummyCurrentRow);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ValidateCellAlignment_ValidatesCellAlignment_Data()
        {
            // Leaves row open if all cells remain open - cells with column span
            Cell dummyCell1 = CreateCell(0, 1);
            Cell dummyCell2 = CreateCell(2, 4);

            // Closes row and unaligned cells if any cells are unaligned - old cell contains new cells
            Cell dummyCell3 = CreateCell(0, 1); // IsOpen is changed by ValidateCellAlignment, so can't reuse dummyCell1

            // Closes row and unaligned cells if any cells are unaligned - old cells and new cells intersect
            Cell dummyCell4 = CreateCell(0, 1);
            Cell dummyCell5 = CreateCell(2, 3);

            return new object[][]
            {
                // Valid row - cells with no column span
                new object[]
                {
                    "|a|aa|aaa|",
                    new List<ColumnDefinition>
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 2),
                        new ColumnDefinition(ContentAlignment.None, 2, 5),
                        new ColumnDefinition(ContentAlignment.None, 5, 9)
                    },
                    new Row()
                    {
                        CreateCell(0, 0),
                        CreateCell(1, 1),
                        CreateCell(2, 2)
                    },
                    true
                },
                // Valid row - cells with column span
                new object[]
                {
                    "|aaa|aaaaaa|",
                    new List<ColumnDefinition>
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 2),
                        new ColumnDefinition(ContentAlignment.None, 2, 4),
                        new ColumnDefinition(ContentAlignment.None, 4, 6),
                        new ColumnDefinition(ContentAlignment.None, 6, 8),
                        new ColumnDefinition(ContentAlignment.None, 8, 11)
                    },
                    new Row()
                    {
                        dummyCell1,
                        dummyCell1,
                        dummyCell2,
                        dummyCell2,
                        dummyCell2
                    },
                    true
                },
                // Invalid row - new cell contains old cells
                new object[]
                {
                    "|aaaaaaaaa|",
                    new List<ColumnDefinition>
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 5),
                        new ColumnDefinition(ContentAlignment.None, 5, 10),
                    },
                    new Row()
                    {
                        CreateCell(0, 0),
                        CreateCell(1, 1)
                    },
                    false
                },
                // Invalid row - old cell contains new cells
                new object[]
                {
                    "|aaaa|aaaa|",
                    new List<ColumnDefinition>
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 5),
                        new ColumnDefinition(ContentAlignment.None, 5, 10),
                    },
                    new Row()
                    {
                        dummyCell3,
                        dummyCell3
                    },
                    false
                },
                // Invalid row - old cells and new cells intersect
                new object[]
                {
                    "|a|aaa|a|",
                    new List<ColumnDefinition>
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 2),
                        new ColumnDefinition(ContentAlignment.None, 2, 4),
                        new ColumnDefinition(ContentAlignment.None, 4, 6),
                        new ColumnDefinition(ContentAlignment.None, 6, 8),
                    },
                    new Row()
                    {
                        dummyCell4,
                        dummyCell4,
                        dummyCell5,
                        dummyCell5
                    },
                    false
                },
                // Invalid row - mix of aligned and unaligned cells
                new object[]
                {
                    "|a|aaa|a|",
                    new List<ColumnDefinition>
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 2),
                        new ColumnDefinition(ContentAlignment.None, 2, 4),
                        new ColumnDefinition(ContentAlignment.None, 4, 6),
                        new ColumnDefinition(ContentAlignment.None, 6, 8),
                    },
                    new Row()
                    {
                        CreateCell(0, 0),
                        CreateCell(1, 1),
                        CreateCell(2, 2),
                        CreateCell(3, 3)
                    },
                    false
                },
                // Invalid row - no ending '|'
                new object[]
                {
                    "|a|aa|aaa",
                    new List<ColumnDefinition>
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 2),
                        new ColumnDefinition(ContentAlignment.None, 2, 5),
                        new ColumnDefinition(ContentAlignment.None, 5, 9)
                    },
                    new Row()
                    {
                        CreateCell(0, 0),
                        CreateCell(1, 1),
                        CreateCell(2, 2)
                    },
                    false
                },
            };
        }

        [Theory]
        [MemberData(nameof(TryCreateRow_TriesToCreateRow_Data))]
        public void TryCreateRow_TriesToCreateRow(string dummyText,
            List<ColumnDefinition> dummyColumnDefinitions,
            Row dummyLastRow,
            int dummyRowIndex,
            int dummyLineIndex,
            Row expectedRow)
        {
            // Arrange
            var dummyLine = new StringSlice(dummyText);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.LineIndex = dummyLineIndex;
            AdvancedFlexiTableBlockParser testSubject = CreateAdvancedFlexiTableBlockParser();

            // Act
            Row result = testSubject.TryCreateRow(dummyLine, dummyColumnDefinitions, dummyLastRow, dummyRowIndex, dummyBlockProcessor);

            // Assert
            Assert.Equal(expectedRow, result);
        }

        public static IEnumerable<object[]> TryCreateRow_TriesToCreateRow_Data()
        {
            // Creates new cells if there is no last row
            Cell dummyCell1 = CreateCell(1, 2, startOffset: 2, endOffset: 6);
            Cell dummyCell2 = CreateCell(3, 5, startOffset: 6, endOffset: 12);

            // Assigns line index and row indices to cells
            Cell dummyCell3 = CreateCell(1, 2, 4, 4, startOffset: 2, endOffset: 6, lineIndex: 3);
            Cell dummyCell4 = CreateCell(3, 5, 4, 4, startOffset: 6, endOffset: 12, lineIndex: 3);

            // Returns false if a an open cell in the last row is not aligned - unaligned cell starts at same column
            Cell dummyCell5 = CreateCell(0, 0);
            dummyCell5.IsOpen = false;
            Cell dummyCell6 = CreateCell(1, 2);

            // Returns false if a an open cell in the last row is not aligned - unaligned cell does not start at same column
            Cell dummyCell7 = CreateCell(0, 1);
            dummyCell7.IsOpen = false;

            return new object[][]
            {
                // Creates new cells if there is no last row
                new object[]
                {
                    "|a|aaa|aaaaa|",
                    new List<ColumnDefinition>
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 2),
                        new ColumnDefinition(ContentAlignment.None, 2, 4),
                        new ColumnDefinition(ContentAlignment.None, 4, 6),
                        new ColumnDefinition(ContentAlignment.None, 6, 8),
                        new ColumnDefinition(ContentAlignment.None, 8, 10),
                        new ColumnDefinition(ContentAlignment.None, 10, 12)
                    },
                    null,
                    0,
                    0,
                    new Row()
                    {
                        CreateCell(0, 0, endOffset: 2),
                        dummyCell1,
                        dummyCell1,
                        dummyCell2,
                        dummyCell2,
                        dummyCell2
                    }
                },
                // Assigns line index and row indices to cells
                new object[]
                {
                    "|a|aaa|aaaaa|",
                    new List<ColumnDefinition>
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 2),
                        new ColumnDefinition(ContentAlignment.None, 2, 4),
                        new ColumnDefinition(ContentAlignment.None, 4, 6),
                        new ColumnDefinition(ContentAlignment.None, 6, 8),
                        new ColumnDefinition(ContentAlignment.None, 8, 10),
                        new ColumnDefinition(ContentAlignment.None, 10, 12)
                    },
                    null,
                    4,
                    3,
                    new Row()
                    {
                        CreateCell(0, 0, 4, 4, endOffset: 2, lineIndex: 3),
                        dummyCell3,
                        dummyCell3,
                        dummyCell4,
                        dummyCell4,
                        dummyCell4
                    }
                },
                // Returns null if an open cell in the last row is not aligned - unaligned cell starts at same column
                new object[]
                {
                    "|a|a|a|",
                    new List<ColumnDefinition>
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 2),
                        new ColumnDefinition(ContentAlignment.None, 2, 4),
                        new ColumnDefinition(ContentAlignment.None, 4, 6)
                    },
                    new Row()
                    {
                        dummyCell5,
                        dummyCell6,
                        dummyCell6
                    },
                    0,
                    0,
                    null
                },
                // Returns null if a an open cell in the last row is not aligned - unaligned cell does not start at same column
                new object[]
                {
                    "|a|aaa|",
                    new List<ColumnDefinition>
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 2),
                        new ColumnDefinition(ContentAlignment.None, 2, 4),
                        new ColumnDefinition(ContentAlignment.None, 4, 6)
                    },
                    new Row()
                    {
                        dummyCell7,
                        dummyCell7,
                        CreateCell(2, 2)
                    },
                    0,
                    0,
                    null
                },
                // Returns null if ending '|' is missing
                new object[]
                {
                    "|a|aaa|aaaaa",
                    new List<ColumnDefinition>
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 2),
                        new ColumnDefinition(ContentAlignment.None, 2, 4),
                        new ColumnDefinition(ContentAlignment.None, 4, 6),
                        new ColumnDefinition(ContentAlignment.None, 6, 8),
                        new ColumnDefinition(ContentAlignment.None, 8, 10),
                        new ColumnDefinition(ContentAlignment.None, 10, 12)
                    },
                    null,
                    0,
                    0,
                    null
                },
            };
        }

        // Can't be part of above theory since we have to test for reference equality
        [Fact]
        public void TryCreateRow_NewRowReferencesAlignedOpenCellsFromLastRow()
        {
            // Arrange
            var dummyLine = new StringSlice("|a|aaaa|a|aa|");
            var dummyColumnDefinitions = new List<ColumnDefinition>
            {
                new ColumnDefinition(ContentAlignment.None, 0, 2),
                new ColumnDefinition(ContentAlignment.None, 2, 4),
                new ColumnDefinition(ContentAlignment.None, 4, 7),
                new ColumnDefinition(ContentAlignment.None, 7, 9),
                new ColumnDefinition(ContentAlignment.None, 9, 12)
            };
            Cell dummyLastRowCell1 = CreateCell(0, 0, 1, 1);
            Cell dummyLastRowCell2 = CreateCell(1, 1);
            dummyLastRowCell2.IsOpen = false;
            Cell dummyLastRowCell3 = CreateCell(2, 3);
            dummyLastRowCell3.IsOpen = false;
            Cell dummyLastRowCell4 = CreateCell(4, 4, 0, 1);
            var dummyLastRow = new Row()
            {
                dummyLastRowCell1,
                dummyLastRowCell2,
                dummyLastRowCell3,
                dummyLastRowCell3,
                dummyLastRowCell4
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            AdvancedFlexiTableBlockParser testSubject = CreateAdvancedFlexiTableBlockParser();

            // Act
            Row result = testSubject.TryCreateRow(dummyLine, dummyColumnDefinitions, dummyLastRow, 0, dummyBlockProcessor);

            // Assert
            Cell resultCell1 = result[0];
            Assert.Same(dummyLastRowCell1, resultCell1); // Aligned cells in last row are referenced
            Assert.Equal(1, resultCell1.StartRowIndex);
            Assert.Equal(2, resultCell1.EndRowIndex);
            Cell resultCell4 = result[4];
            Assert.Same(dummyLastRowCell4, resultCell4); // Aligned cells in last row are referenced
            Assert.Equal(0, resultCell4.StartRowIndex);
            Assert.Equal(2, resultCell4.EndRowIndex);
        }

        [Theory]
        [MemberData(nameof(TryParseSeparatorLine_ReturnsFalseIfThereIsNoOpenRow_Data))]
        public void TryParseSeparatorLine_ReturnsFalseIfThereIsNoOpenRow(List<Row> dummyRows)
        {
            // Arrange
            var dummyProxyTableBlock = new ProxyTableBlock(null);
            foreach (Row row in dummyRows)
            {
                dummyProxyTableBlock.Rows.Add(row);
            }
            AdvancedFlexiTableBlockParser testSubject = CreateAdvancedFlexiTableBlockParser();

            // Act
            bool result = testSubject.TryParseSeparatorLine(default, dummyProxyTableBlock);

            // Assert
            Assert.False(result);
        }

        public static IEnumerable<object[]> TryParseSeparatorLine_ReturnsFalseIfThereIsNoOpenRow_Data()
        {
            return new object[][]
            {
                // Empty
                new object[]{new List<Row>()},
                // Last row already closed
                new object[]{ new List<Row> { new Row(), new Row { IsOpen = false} } }
            };
        }

        [Fact]
        public void TryParseSeparatorLine_ReturnsFalseIfAHeadSeparatorLineHasNotBeenParsedFirstContentCharacterIsEqualButLineIsNotAHeadSeparatorLine()
        {
            // Arrange
            const string dummyText = "+=";
            var dummyColumnDefinitions = new List<ColumnDefinition>();
            var dummyProxyTableBlock = new ProxyTableBlock(null);
            dummyProxyTableBlock.Rows.Add(new Row()); // At least 1 open row
            dummyProxyTableBlock.ColumnDefinitions = dummyColumnDefinitions;
            Mock<AdvancedFlexiTableBlockParser> testSubject = CreateMockAdvancedFlexiTableBlockParser();
            testSubject.CallBase = true;
            testSubject.Setup(t => t.TryParseHeadSeparatorLine(It.Is<StringSlice>(line => line.Text == dummyText),
                    It.Is<List<ColumnDefinition>>(columnDefinitions => columnDefinitions == dummyColumnDefinitions),
                    It.Is<List<Row>>(rows => rows == dummyProxyTableBlock.Rows))).
                Returns(false);

            // Act
            bool result = testSubject.Object.TryParseSeparatorLine(new StringSlice(dummyText), dummyProxyTableBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.False(result);
        }

        [Theory]
        [MemberData(nameof(TryParseSeparatorLine_ReturnsFalseIfLineIsNeitherAHeadSeparatorLineNorARowSeparatorLine_Data))]
        public void TryParseSeparatorLine_ReturnsFalseIfLineIsNeitherAHeadSeparatorLineNorARowSeparatorLine(bool dummyHasHeaderRows,
            string dummyText)
        {
            // Arrange
            var dummyColumnDefinitions = new List<ColumnDefinition>();
            var dummyRow = new Row();
            var dummyProxyTableBlock = new ProxyTableBlock(null);
            dummyProxyTableBlock.Rows.Add(dummyRow); // At least 1 open row
            dummyProxyTableBlock.ColumnDefinitions = dummyColumnDefinitions;
            dummyProxyTableBlock.HasHeaderRows = dummyHasHeaderRows;
            Mock<AdvancedFlexiTableBlockParser> testSubject = CreateMockAdvancedFlexiTableBlockParser();
            testSubject.CallBase = true;
            testSubject.Setup(t => t.TryParseRowSeparatorLine(It.Is<StringSlice>(line => line.Text == dummyText),
                    It.Is<List<ColumnDefinition>>(columnDefinitions => columnDefinitions == dummyColumnDefinitions),
                    It.Is<Row>(row => row == dummyRow))).
                Returns(false);

            // Act
            bool result = testSubject.Object.TryParseSeparatorLine(new StringSlice(dummyText), dummyProxyTableBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.False(result);
        }

        public static IEnumerable<object[]> TryParseSeparatorLine_ReturnsFalseIfLineIsNeitherAHeadSeparatorLineNorARowSeparatorLine_Data()
        {
            return new object[][]
            {
                new object[]{true, "+="},
                new object[]{false, "+-"}
            };
        }

        [Fact]
        public void TryParseSeparatorLine_ReturnsTrueIfAHeadSeparatorLineHasNotBeenParsedFirstContentCharacterIsEqualAndLineIsAHeadSeparatorLine()
        {
            // Arrange
            const string dummyText = "+=";
            var dummyColumnDefinitions = new List<ColumnDefinition>();
            var dummyRow = new Row();
            var dummyProxyTableBlock = new ProxyTableBlock(null);
            dummyProxyTableBlock.Rows.Add(dummyRow); // At least 1 open row
            dummyProxyTableBlock.ColumnDefinitions = dummyColumnDefinitions;
            Mock<AdvancedFlexiTableBlockParser> testSubject = CreateMockAdvancedFlexiTableBlockParser();
            testSubject.CallBase = true;
            testSubject.Setup(t => t.TryParseHeadSeparatorLine(It.Is<StringSlice>(line => line.Text == dummyText),
                    It.Is<List<ColumnDefinition>>(columnDefinitions => columnDefinitions == dummyColumnDefinitions),
                    It.Is<List<Row>>(rows => rows == dummyProxyTableBlock.Rows))).
                Returns(true);
            testSubject.Protected().Setup("ExtractContent", ItExpr.Is<StringSlice>(line => line.Text == dummyText), ItExpr.Is<Row>(row => row == dummyRow));

            // Act
            bool result = testSubject.Object.TryParseSeparatorLine(new StringSlice(dummyText), dummyProxyTableBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.True(result);
            Assert.True(dummyProxyTableBlock.HasHeaderRows);
            Assert.False(dummyRow.IsOpen);
        }

        [Theory]
        [MemberData(nameof(TryParseSeparatorLine_ReturnsTrueIfLineIsARowSeparatorLine_Data))]
        public void TryParseSeparatorLine_ReturnsTrueIfLineIsARowSeparatorLine(bool dummyHasHeaderRows,
            string dummyText)
        {
            // Arrange
            var dummyColumnDefinitions = new List<ColumnDefinition>();
            var dummyRow = new Row();
            var dummyProxyTableBlock = new ProxyTableBlock(null);
            dummyProxyTableBlock.Rows.Add(dummyRow); // At least 1 open row
            dummyProxyTableBlock.ColumnDefinitions = dummyColumnDefinitions;
            dummyProxyTableBlock.HasHeaderRows = dummyHasHeaderRows;
            Mock<AdvancedFlexiTableBlockParser> testSubject = CreateMockAdvancedFlexiTableBlockParser();
            testSubject.CallBase = true;
            testSubject.Setup(t => t.TryParseRowSeparatorLine(It.Is<StringSlice>(line => line.Text == dummyText),
                    It.Is<List<ColumnDefinition>>(columnDefinitions => columnDefinitions == dummyColumnDefinitions),
                    It.Is<Row>(row => row == dummyRow))).
                Returns(true);
            testSubject.Protected().Setup("ExtractContent", ItExpr.Is<StringSlice>(line => line.Text == dummyText), ItExpr.Is<Row>(row => row == dummyRow));

            // Act
            bool result = testSubject.Object.TryParseSeparatorLine(new StringSlice(dummyText), dummyProxyTableBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.True(result);
            Assert.False(dummyRow.IsOpen);
        }

        public static IEnumerable<object[]> TryParseSeparatorLine_ReturnsTrueIfLineIsARowSeparatorLine_Data()
        {
            return new object[][]
            {
                new object[]{true, "+="},
                new object[]{false, "+-"}
            };
        }

        [Theory]
        [MemberData(nameof(TryParseHeadSeparatorLine_ReturnsFalseIfHeadSeparatorLineIsInvalid_Data))]
        public void TryParseHeadSeparatorLine_ReturnsFalseIfHeadSeparatorLineIsInvalid(string dummyText,
            List<ColumnDefinition> dummyColumnDefinitions)
        {
            // Arrange
            var dummyLine = new StringSlice(dummyText);
            AdvancedFlexiTableBlockParser testSubject = CreateAdvancedFlexiTableBlockParser();

            // Act
            bool result = testSubject.TryParseHeadSeparatorLine(dummyLine, dummyColumnDefinitions, new List<Row>());

            // Assert
            Assert.False(result);
        }

        public static IEnumerable<object[]> TryParseHeadSeparatorLine_ReturnsFalseIfHeadSeparatorLineIsInvalid_Data()
        {
            return new object[][]
            {
                // Returns false if any '+' is unaligned
                new object[]
                {
                    "+==+===+",
                    new List<ColumnDefinition>()
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 4),
                        new ColumnDefinition(ContentAlignment.None, 4, 7)
                    }
                },
                new object[]
                {
                    "+===+=+=",
                    new List<ColumnDefinition>()
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 4),
                        new ColumnDefinition(ContentAlignment.None, 4, 7)
                    }
                },
                // Returns false if any '+' is missing - first '+' is never missing since method isn't called if so
                new object[]
                {
                    "+==+===" ,
                    new List<ColumnDefinition>()
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 3),
                        new ColumnDefinition(ContentAlignment.None, 3, 7)
                    }
                },
                new object[]
                {
                    "+======+",
                    new List<ColumnDefinition>()
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 5),
                        new ColumnDefinition(ContentAlignment.None, 5, 7)
                    }
                },
                // Returns false if any of the content characters in a row separator isn't '='
                new object[]
                {
                    "+ ==+==+",
                    new List<ColumnDefinition>()
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 4),
                        new ColumnDefinition(ContentAlignment.None, 4, 7)
                    }
                },
                new object[]
                {
                    "+==+ test +",
                    new List<ColumnDefinition>()
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 3),
                        new ColumnDefinition(ContentAlignment.None, 3, 10)
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(TryParseHeadSeparatorLine_ReturnsTrueAndUpdatesStatesIfHeadSeparatorLineIsValid_Data))]
        public void TryParseHeadSeparatorLine_ReturnsTrueAndUpdatesStatesIfHeadSeparatorLineIsValid(string dummyText,
            List<ColumnDefinition> dummyColumnDefinitions,
            List<Row> dummyRows)
        {
            // Arrange
            var dummyLine = new StringSlice(dummyText);
            AdvancedFlexiTableBlockParser testSubject = CreateAdvancedFlexiTableBlockParser();

            // Act
            bool result = testSubject.TryParseHeadSeparatorLine(dummyLine, dummyColumnDefinitions, dummyRows);

            // Assert
            Assert.True(result);
            for (int i = 0; i < dummyRows.Count; i++)
            {
                Assert.True(dummyRows[i].IsHeaderRow);
            }
            Row lastRow = dummyRows.Last();
            for (int i = 0; i < lastRow.Count; i++)
            {
                Assert.False(lastRow[i].IsOpen);
            }
        }

        public static IEnumerable<object[]> TryParseHeadSeparatorLine_ReturnsTrueAndUpdatesStatesIfHeadSeparatorLineIsValid_Data()
        {
            return new object[][]
            {
                //Returns true, sets Row.IsHeaderRow to true and sets Cell.IsOpen to false if head separator line is valid - single header row
                new object[]
                {
                    "+==+===+",
                    new List<ColumnDefinition>()
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 3),
                        new ColumnDefinition(ContentAlignment.None, 3, 7)
                    },
                    new List<Row>()
                    {
                        new Row()
                        {
                            CreateCell(0, 0),
                            CreateCell(1, 1)
                        }
                    }
                },
                //Returns true, sets Row.IsHeaderRow to true and sets Cell.IsOpen to false if head separator line is valid - multiple header rows
                new object[]
                {
                    "+==+===+",
                    new List<ColumnDefinition>()
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 3),
                        new ColumnDefinition(ContentAlignment.None, 3, 7)
                    },
                    new List<Row>()
                    {
                        new Row()
                        {
                            CreateCell(0, 0),
                            CreateCell(1, 1)
                        },
                        new Row()
                        {
                            CreateCell(0, 0),
                            CreateCell(1, 1)
                        }
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(TryParseRowSeparatorLine_ReturnsFalseIfRowSeparatorLineIsInvalid_Data))]
        public void TryParseRowSeparatorLine_ReturnsFalseIfRowSeparatorLineIsInvalid(string dummyText,
            List<ColumnDefinition> dummyColumnDefinitions,
            Row dummyLastRow)
        {
            // Arrange
            var dummyLine = new StringSlice(dummyText);
            AdvancedFlexiTableBlockParser testSubject = CreateAdvancedFlexiTableBlockParser();

            // Act
            bool result = testSubject.TryParseRowSeparatorLine(dummyLine, dummyColumnDefinitions, dummyLastRow);

            // Assert
            Assert.False(result);
        }

        public static IEnumerable<object[]> TryParseRowSeparatorLine_ReturnsFalseIfRowSeparatorLineIsInvalid_Data()
        {
            // Last row cells spans less columns
            Cell dummyCell1 = CreateCell(0, 1);

            return new object[][]
            {
                // Last row cell spans more columns
                new object[]
                {
                    "+ test +---+",
                    new List<ColumnDefinition>
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 7),
                        new ColumnDefinition(ContentAlignment.None, 7, 11)
                    },
                    new Row()
                    {
                        dummyCell1,
                        dummyCell1
                    }
                },
                new object[]
                {
                    "+---+ test +",
                    new List<ColumnDefinition>
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 4),
                        new ColumnDefinition(ContentAlignment.None, 4, 11)
                    },
                    new Row()
                    {
                        dummyCell1,
                        dummyCell1
                    }
                },
                // Last row cells spans less columns
                new object[]
                {
                    "+ test test +",
                    new List<ColumnDefinition>
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 7),
                        new ColumnDefinition(ContentAlignment.None, 7, 12)
                    },
                    new Row()
                    {
                        CreateCell(0, 0),
                        CreateCell(1, 1)
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(TryParseRowSeparatorLine_ReturnsTrueAndUpdatesStateIfRowSeparatorLineIsValid_Data))]
        public void TryParseRowSeparatorLine_ReturnsTrueAndUpdatesStateIfRowSeparatorLineIsValid(string dummyText,
            List<ColumnDefinition> dummyColumnDefinitions,
            Row dummyLastRow,
            bool[] expectedCellIsOpens)
        {
            // Arrange
            var dummyLine = new StringSlice(dummyText);
            AdvancedFlexiTableBlockParser testSubject = CreateAdvancedFlexiTableBlockParser();

            // Act
            bool result = testSubject.TryParseRowSeparatorLine(dummyLine, dummyColumnDefinitions, dummyLastRow);

            // Assert
            Assert.True(result);
            int cellIndex = 0;
            for (int columnIndex = 0; columnIndex < dummyColumnDefinitions.Count;)
            {
                Cell cell = dummyLastRow[columnIndex];
                Assert.Equal(expectedCellIsOpens[cellIndex++], cell.IsOpen);

                columnIndex = cell.EndColumnIndex + 1;
            }
        }

        public static IEnumerable<object[]> TryParseRowSeparatorLine_ReturnsTrueAndUpdatesStateIfRowSeparatorLineIsValid_Data()
        {
            // Cells with column span
            Cell dummyCell1 = CreateCell(0, 1);
            Cell dummyCell2 = CreateCell(2, 3);

            // Adjacent open cells
            Cell dummyCell3 = CreateCell(1, 2);

            // Multiple open and close cells
            Cell dummyCell4 = CreateCell(0, 1);
            Cell dummyCell5 = CreateCell(2, 3);

            // Open cells with '-'s and '+'s as content
            Cell dummyCell6 = CreateCell(0, 1);

            return new object[][]
            {
                // Cells with no column span
                new object[]
                {
                    "+ test +---+",
                    new List<ColumnDefinition>
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 7),
                        new ColumnDefinition(ContentAlignment.None, 7, 11)
                    },
                    new Row()
                    {
                        CreateCell(0, 0),
                        CreateCell(1, 1)
                    },
                    new bool[]{ true, false }
                },
                // Cells with column span
                new object[]
                {
                    "+ test +-+-+",
                    new List<ColumnDefinition>
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 3),
                        new ColumnDefinition(ContentAlignment.None, 3, 7),
                        new ColumnDefinition(ContentAlignment.None, 7, 9),
                        new ColumnDefinition(ContentAlignment.None, 9, 11)
                    },
                    new Row()
                    {
                        dummyCell1,
                        dummyCell1,
                        dummyCell2,
                        dummyCell2
                    },
                    new bool[]{ true, false }
                },
                // Adjacent open cells
                new object[]
                {
                    "+ test + test +",
                    new List<ColumnDefinition>
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 7),
                        new ColumnDefinition(ContentAlignment.None, 7, 11),
                        new ColumnDefinition(ContentAlignment.None, 11, 14)
                    },
                    new Row()
                    {
                        CreateCell(0, 0),
                        dummyCell3,
                        dummyCell3
                    },
                    new bool[]{ true, true }
                },
                // Multiple open and close cells
                new object[]
                {
                    "+-+-+ test +---+ test +--+",
                    new List<ColumnDefinition>
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 2),
                        new ColumnDefinition(ContentAlignment.None, 2, 4),
                        new ColumnDefinition(ContentAlignment.None, 4, 8),
                        new ColumnDefinition(ContentAlignment.None, 8, 11),
                        new ColumnDefinition(ContentAlignment.None, 11, 15),
                        new ColumnDefinition(ContentAlignment.None, 15, 22),
                        new ColumnDefinition(ContentAlignment.None, 22, 25)
                    },
                    new Row()
                    {
                        dummyCell4,
                        dummyCell4,
                        dummyCell5,
                        dummyCell5,
                        CreateCell(4, 4),
                        CreateCell(5, 5),
                        CreateCell(6, 6)
                    },
                    new bool[]{ false, true, false, true, false }
                },
                // Open cells with '-'s and '+'s as content
                new object[]
                {
                    "+ test + test +-test-+",
                    new List<ColumnDefinition>
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 7),
                        new ColumnDefinition(ContentAlignment.None, 7, 14),
                        new ColumnDefinition(ContentAlignment.None, 14, 21)
                    },
                    new Row()
                    {
                        dummyCell6,
                        dummyCell6,
                        CreateCell(2, 2)
                    },
                    new bool[]{ true, true }
                },
            };
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

        public class ExposedAdvancedFlexiTableBlockParser : AdvancedFlexiTableBlockParser
        {
            public ExposedAdvancedFlexiTableBlockParser(IFlexiTableBlockFactory flexiTableBlockFactory) : base(flexiTableBlockFactory)
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

        private AdvancedFlexiTableBlockParser CreateAdvancedFlexiTableBlockParser(IFlexiTableBlockFactory flexiTableBlockFactory = null)
        {
            return new AdvancedFlexiTableBlockParser(flexiTableBlockFactory ?? _mockRepository.Create<IFlexiTableBlockFactory>().Object);
        }

        private Mock<AdvancedFlexiTableBlockParser> CreateMockAdvancedFlexiTableBlockParser(IFlexiTableBlockFactory flexiTableBlockFactory = null)
        {
            return _mockRepository.Create<AdvancedFlexiTableBlockParser>(flexiTableBlockFactory ?? _mockRepository.Create<IFlexiTableBlockFactory>().Object);
        }

        private ExposedAdvancedFlexiTableBlockParser CreateExposedAdvancedFlexiTableBlockParser(IFlexiTableBlockFactory flexiTableBlockFactory = null)
        {
            return new ExposedAdvancedFlexiTableBlockParser(flexiTableBlockFactory ?? _mockRepository.Create<IFlexiTableBlockFactory>().Object);
        }

        private Mock<ExposedAdvancedFlexiTableBlockParser> CreateMockExposedAdvancedFlexiTableBlockParser(IFlexiTableBlockFactory flexiTableBlockFactory = null)
        {
            return _mockRepository.Create<ExposedAdvancedFlexiTableBlockParser>(flexiTableBlockFactory ?? _mockRepository.Create<IFlexiTableBlockFactory>().Object);
        }
    }
}
