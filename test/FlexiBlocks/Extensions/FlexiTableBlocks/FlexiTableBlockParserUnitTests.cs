using Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiTableBlocks
{
    public class FlexiTableBlockParserUnitTests
    {
        private static readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiCodeBlockFactoryIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new ExposedFlexiTableBlockParser(null));
        }

        [Theory]
        [MemberData(nameof(CanInterrupt_ReturnsFalseIfBlockIsAParagraphBlockReturnsTrueOtherwise_Data))]
        public void CanInterrupt_ReturnsFalseIfBlockIsAParagraphBlockReturnsTrueOtherwise(Block dummyBlock, bool expectedResult)
        {
            // Arrange
            Mock<FlexiTableBlockParser> mockTestSubject = CreateMockFlexiTableBlockParser();
            mockTestSubject.CallBase = true;

            // Act
            bool result = mockTestSubject.Object.CanInterrupt(null, dummyBlock);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> CanInterrupt_ReturnsFalseIfBlockIsAParagraphBlockReturnsTrueOtherwise_Data()
        {
            return new object[][]
            {
                new object[]{ new ParagraphBlock(null), false },
                new object[]{ _mockRepository.Create<Block>(null).Object, true }
            };
        }

        [Theory]
        [MemberData(nameof(TryParseColumnDefinitionsLine_TriesToParseColumnDefinitionsLine_Data))]
        public void TryParseColumnDefinitionsLine_TriesToParseColumnDefinitionsLine(string dummyText,
            char dummyColumnDefinitionStartChar,
            int dummyNumColumns,
            List<ColumnDefinition> expectedColumnDefinitions)
        {
            // Arrange
            ExposedFlexiTableBlockParser testSubject = CreateExposedFlexiTableBlockParser();

            // Act
            List<ColumnDefinition> result = testSubject.ExposedTryParseColumnDefinitionsLine(new StringSlice(dummyText), dummyColumnDefinitionStartChar, dummyNumColumns);

            // Assert
            Assert.Equal(expectedColumnDefinitions, result);
        }

        public static IEnumerable<object[]> TryParseColumnDefinitionsLine_TriesToParseColumnDefinitionsLine_Data()
        {
            return new object[][]
            {
                // No column definitions
                new object[]{ "+", '+', -1, null },
                new object[]{ "|   ", '|', -1, null },
                // First non-space char after a '+' isn't ':' or '-'
                new object[]{ "++", '+', -1, null },
                new object[]{ "|a", '|', -1, null },
                new object[]{ "+--+a", '+', -1, null },
                // No ending '+'
                new object[]{ "|--", '|', -1, null },
                new object[]{ "+--:", '+', -1, null },
                // Spaces between '+', ':' or '-'
                new object[]{ "|- -|", '|', -1, null },
                new object[]{ "+ --+", '+', -1, null },
                new object[]{ "|-- |", '|', -1, null },
                new object[]{ "+ :--+", '+', -1, null },
                new object[]{ "|: --|", '|', -1, null },
                new object[]{ "+-- :+", '+', -1, null },
                new object[]{ "|--: |", '|', -1, null },
                // Line has trailing whitespace
                new object[]{ "+--+ ", '+', -1, null },
                // Too many column definitions
                new object[]{ "+---+---+", '+', 1, null},
                // Too few column definitions
                new object[]{ "+---+---+", '+', 3, null},
                // Start content alignment
                new object[]{ "|:--|", '|', 1, new List<ColumnDefinition> { new ColumnDefinition(ContentAlignment.Start, 0, 4) } },
                // End content alignment
                new object[]{ "+--:+", '+', 1, new List<ColumnDefinition> { new ColumnDefinition(ContentAlignment.End, 0, 4) } },
                // Center content alignment
                new object[]{ "|:--:|", '|', 1, new List<ColumnDefinition> { new ColumnDefinition(ContentAlignment.Center, 0, 5) } },
                // Multiple column definitions
                new object[]{ "+---+---+", '+', -1, new List<ColumnDefinition>
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 4),
                        new ColumnDefinition(ContentAlignment.None, 4, 8)
                    }
                },
                new object[]{ "|---|--|-|", '|', 3, new List<ColumnDefinition>
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 4),
                        new ColumnDefinition(ContentAlignment.None, 4, 7),
                        new ColumnDefinition(ContentAlignment.None, 7, 9)
                    }
                },
                new object[]{ "+-+--+---+", '+', -1, new List<ColumnDefinition>
                    {
                        new ColumnDefinition(ContentAlignment.None, 0, 2),
                        new ColumnDefinition(ContentAlignment.None, 2, 5),
                        new ColumnDefinition(ContentAlignment.None, 5, 9)
                    }
                },
                new object[]{ "|:---|:---:|---:|", '|', 3, new List<ColumnDefinition>
                    {
                        new ColumnDefinition(ContentAlignment.Start, 0, 5),
                        new ColumnDefinition(ContentAlignment.Center, 5, 11),
                        new ColumnDefinition(ContentAlignment.End, 11, 16)
                    }
                }
            };
        }

        [Fact]
        public void ExtractContent_ThrowsArgumentNullExceptionIfTargetRowIsNull()
        {
            // Arrange
            var dummyLine = new StringSlice("");
            ExposedFlexiTableBlockParser testSubject = CreateExposedFlexiTableBlockParser();

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => testSubject.ExposedExtractContent(dummyLine, null));
        }

        [Theory]
        [MemberData(nameof(ExtractContent_ExtractsContent_Data))]
        public void ExtractContent_ExtractsContent(string dummyText,
            Row dummyTargetRow,
            string[] expectedCellContent)
        {
            // Arrange
            var dummyLine = new StringSlice(dummyText);
            ExposedFlexiTableBlockParser testSubject = CreateExposedFlexiTableBlockParser();

            // Act
            testSubject.ExposedExtractContent(dummyLine, dummyTargetRow);

            // Assert
            int cellIndex = 0;
            for (int columnIndex = 0; columnIndex < dummyTargetRow.Count;)
            {
                Cell cell = dummyTargetRow[columnIndex];
                Assert.Equal(expectedCellContent[cellIndex++], cell.Lines.ToString());
                columnIndex = cell.EndColumnIndex + 1;
            }
        }

        public static IEnumerable<object[]> ExtractContent_ExtractsContent_Data()
        {
            // Cells with column span
            Cell dummyCell1 = CreateCell(0, 1, startOffset: 0, endOffset: 17);
            Cell dummyCell2 = CreateCell(2, 4, startOffset: 17, endOffset: 34);

            // Mix of open and closed cells
            Cell dummyCell3 = CreateCell(0, 0, startOffset: 0, endOffset: 4);
            dummyCell3.IsOpen = false;
            Cell dummyCell4 = CreateCell(1, 2, startOffset: 4, endOffset: 21);
            Cell dummyCell5 = CreateCell(3, 4, startOffset: 21, endOffset: 26);
            dummyCell5.IsOpen = false;

            // Cells that already have lines
            Cell dummyCell6 = CreateCell(0, 0, startOffset: 0, endOffset: 19);
            dummyCell6.Lines.Add(new StringSlice(" cell 1 content 1"));
            Cell dummyCell7 = CreateCell(1, 1, startOffset: 19, endOffset: 38);
            dummyCell7.Lines.Add(new StringSlice(" cell 2 content 1"));

            return new object[][]
            {
                // Cells with no column span
                new object[]
                {
                    "| cell 1 content | cell 2 content | cell 3 content |",
                    new Row()
                    {
                        CreateCell(0, 0, startOffset: 0, endOffset: 17),
                        CreateCell(1, 1, startOffset: 17, endOffset: 34),
                        CreateCell(2, 2, startOffset: 34, endOffset: 51)
                    },
                    new string[]{" cell 1 content", " cell 2 content", " cell 3 content" }
                },
                // Cells with column span
                new object[]
                {
                    "| cell 1 content | cell 2 content |",
                    new Row()
                    {
                        dummyCell1,
                        dummyCell1,
                        dummyCell2,
                        dummyCell2,
                        dummyCell2
                    },
                    new string[]{" cell 1 content", " cell 2 content" }
                },
                // Mix of open and closed cells
                new object[]
                {
                    "+---+ cell 1 content +----+ cell 2 content +",
                    new Row()
                    {
                        dummyCell3,
                        dummyCell4,
                        dummyCell4,
                        dummyCell5,
                        dummyCell5,
                        CreateCell(5, 5, startOffset: 26, endOffset: 43)
                    },
                    new string[]{string.Empty, " cell 1 content", string.Empty, " cell 2 content" }
                },
                // Cells that already have lines
                new object[]
                {
                    "| cell 1 content 2 | cell 2 content 2 |",
                    new Row()
                    {
                        dummyCell6,
                        dummyCell7
                    },
                    new string[]{" cell 1 content 1\n cell 1 content 2", " cell 2 content 1\n cell 2 content 2" }
                },
            };
        }

        [Fact]
        public void Undo_ThrowsArgumentNullExceptionIfBlockProcessorIsNull()
        {
            // Arrange
            var dummyProxyTableBlock = new ProxyTableBlock(null);
            ExposedFlexiTableBlockParser testSubject = CreateExposedFlexiTableBlockParser();

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => testSubject.ExposedUndo(null, dummyProxyTableBlock));
        }

        [Fact]
        public void Undo_ThrowsArgumentNullExceptionIfProxyTableBlockIsNull()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            ExposedFlexiTableBlockParser testSubject = CreateExposedFlexiTableBlockParser();

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => testSubject.ExposedUndo(dummyBlockProcessor, null));
        }

        [Fact]
        public void Undo_ReplacesProxyTableBlockWithAParagraphBlock()
        {
            // Arrange
            const string dummyText = "dummyText";
            var dummyProxyTableBlock = new ProxyTableBlock(null);
            dummyProxyTableBlock.Lines = new StringLineGroup(dummyText);
            BlockParser dummyBlockParser = _mockRepository.Create<BlockParser>().Object;
            ContainerBlock dummyParent = _mockRepository.Create<ContainerBlock>(dummyBlockParser).Object; // Must specify block parser since we're calling ProcessLine later
            dummyParent.Add(dummyProxyTableBlock); // Assigns dummyParent to dummyProxyTableBlock.Parent
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Open(dummyParent);
            dummyBlockProcessor.Open(dummyProxyTableBlock);
            ExposedFlexiTableBlockParser testSubject = CreateExposedFlexiTableBlockParser();

            // Act
            testSubject.ExposedUndo(dummyBlockProcessor, dummyProxyTableBlock);

            // Assert
            Assert.Single(dummyParent);
            var resultParagraphBlock = dummyParent[0] as ParagraphBlock;
            Assert.NotNull(resultParagraphBlock);
            Assert.Equal(dummyText, resultParagraphBlock.Lines.ToString());
            // Verify that ParagraphBlock remains open
            dummyBlockProcessor.ProcessLine(new StringSlice(dummyText));
            Assert.Equal($"{dummyText}\n{dummyText}", resultParagraphBlock.Lines.ToString());
        }

        [Fact]
        public void CloseProxy_ReturnsNewFlexiTableBlock()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyProxyTableBlock = new ProxyTableBlock(null);
            var dummyFlexiTableBlock = new FlexiTableBlock(default, default, default, default);
            Mock<IFlexiTableBlockFactory> mockFlexiTableBlockFactory = _mockRepository.Create<IFlexiTableBlockFactory>();
            mockFlexiTableBlockFactory.Setup(f => f.Create(dummyProxyTableBlock, dummyBlockProcessor)).Returns(dummyFlexiTableBlock);
            ExposedFlexiTableBlockParser testSubject = CreateExposedFlexiTableBlockParser(mockFlexiTableBlockFactory.Object);

            // Act
            FlexiTableBlock result = testSubject.ExposedCloseProxy(dummyBlockProcessor, dummyProxyTableBlock);

            // Assert
            Assert.Same(dummyFlexiTableBlock, result);
        }

        private Mock<FlexiTableBlockParser> CreateMockFlexiTableBlockParser(IFlexiTableBlockFactory flexiTableBlockFactory = null)
        {
            return _mockRepository.Create<FlexiTableBlockParser>(flexiTableBlockFactory ?? _mockRepository.Create<IFlexiTableBlockFactory>().Object);
        }

        private ExposedFlexiTableBlockParser CreateExposedFlexiTableBlockParser(IFlexiTableBlockFactory flexiTableBlockFactory = null)
        {
            return new ExposedFlexiTableBlockParser(flexiTableBlockFactory ?? _mockRepository.Create<IFlexiTableBlockFactory>().Object);
        }

        private static Cell CreateCell(int startColumnIndex = 0,
            int endColumnIndex = 0,
            int startRowIndex = 0,
            int endRowIndex = 0,
            int startOffset = 0,
            int endOffset = 1,
            int lineIndex = 0)
        {
            return new Cell(startColumnIndex, endColumnIndex, startRowIndex, endRowIndex, startOffset, endOffset, lineIndex);
        }

        private class ExposedFlexiTableBlockParser : FlexiTableBlockParser
        {
            public ExposedFlexiTableBlockParser(IFlexiTableBlockFactory flexiTableBlockFactory) : base(flexiTableBlockFactory)
            {
            }

            public List<ColumnDefinition> ExposedTryParseColumnDefinitionsLine(StringSlice line, char columnDefinitionStartChar, int numColumns)
            {
                return TryParseColumnDefinitionsLine(line, columnDefinitionStartChar, numColumns);
            }

            public void ExposedExtractContent(StringSlice line, Row targetRow)
            {
                ExtractContent(line, targetRow);
            }

            public void ExposedUndo(BlockProcessor blockProcessor, ProxyTableBlock proxyTableBlock)
            {
                Undo(blockProcessor, proxyTableBlock);
            }

            public FlexiTableBlock ExposedCloseProxy(BlockProcessor blockProcessor, ProxyTableBlock proxyTableBlock)
            {
                return CloseProxy(blockProcessor, proxyTableBlock);
            }

            protected override BlockState TryOpenBlock(BlockProcessor blockProcessor)
            {
                // Do nothing
                return BlockState.None;
            }
        }
    }
}
