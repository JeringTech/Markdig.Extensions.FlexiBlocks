using Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiTableBlocks
{
    public class FlexiTableBlockFactoryUnitTests
    {
        private static readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Empty };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfOptionsServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiTableBlockFactory(null));
        }

        [Fact]
        public void Create_CreatesFlexiTableBlock()
        {
            // Arrange
            const string dummyBlockName = "dummyBlockName";
            const string dummyResolvedBlockName = "dummyResolvedBlockName";
            const FlexiTableType dummyType = FlexiTableType.FixedTitles;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyAttributes = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
            Mock<IFlexiTableBlockOptions> mockFlexiTableBlockOptions = _mockRepository.Create<IFlexiTableBlockOptions>();
            mockFlexiTableBlockOptions.Setup(f => f.BlockName).Returns(dummyBlockName);
            mockFlexiTableBlockOptions.Setup(f => f.Type).Returns(dummyType);
            mockFlexiTableBlockOptions.Setup(f => f.Attributes).Returns(dummyAttributes);
            Mock<IOptionsService<IFlexiTableBlockOptions, IFlexiTableBlocksExtensionOptions>> mockOptionsService = _mockRepository.
                Create<IOptionsService<IFlexiTableBlockOptions, IFlexiTableBlocksExtensionOptions>>();
            mockOptionsService.Setup(f => f.CreateOptions(dummyBlockProcessor)).
                Returns((mockFlexiTableBlockOptions.Object, null));
            var dummyProxyTableBlock = new ProxyTableBlock(null);
            FlexiTableBlock dummyFlexiTableBlock = CreateFlexiTableBlock();
            Mock<FlexiTableBlockFactory> mockTestSubject = CreateMockFlexiTableBlockFactory(mockOptionsService.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.ResolveBlockName(dummyBlockName)).Returns(dummyResolvedBlockName);
            mockTestSubject.Setup(t => t.ValidateType(dummyType));
            mockTestSubject.Setup(t => t.CreateFlexiTableBlock(dummyResolvedBlockName, dummyType, dummyAttributes, dummyProxyTableBlock, dummyBlockProcessor)).Returns(dummyFlexiTableBlock);

            // Act
            FlexiTableBlock result = mockTestSubject.Object.Create(dummyProxyTableBlock, dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void CreateProxy_ThrowsArgumentNullExceptionIfBlockProcessorIsNull()
        {
            // Arrange
            Mock<BlockParser> dummyBlockParser = _mockRepository.Create<BlockParser>();
            FlexiTableBlockFactory testSubject = CreateFlexiTableBlockFactory();

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => testSubject.CreateProxy(null, dummyBlockParser.Object));
        }

        [Fact]
        public void CreateProxy_CreatesProxyTableBlock()
        {
            // Arrange
            const int dummyColumn = 2;
            const int dummyLineStart = 7;
            const int dummyLineEnd = 99;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = dummyColumn;
            dummyBlockProcessor.Line = new StringSlice("", dummyLineStart, dummyLineEnd);
            Mock<BlockParser> dummyBlockParser = _mockRepository.Create<BlockParser>();
            FlexiTableBlockFactory testSubject = CreateFlexiTableBlockFactory();

            // Act
            ProxyTableBlock result = testSubject.CreateProxy(dummyBlockProcessor, dummyBlockParser.Object);

            // Assert
            Assert.Equal(dummyColumn, result.Column);
            Assert.Equal(dummyLineStart, result.Span.Start);
            Assert.Equal(dummyLineEnd, result.Span.End);
            Assert.Same(dummyBlockParser.Object, result.Parser);
            Assert.Equal(nameof(FlexiTableBlock), result.MainTypeName);
        }

        private static FlexiTableBlock CreateFlexiTableBlock(string blockName = default,
            FlexiTableType type = default,
            ReadOnlyDictionary<string, string> attributes = default,
            BlockParser blockParser = default,
            params FlexiTableRowBlock[] flexiTableRowBlocks)
        {
            var result = new FlexiTableBlock(blockName, type, attributes, blockParser);

            foreach (FlexiTableRowBlock flexiTableRowBlock in flexiTableRowBlocks)
            {
                result.Add(flexiTableRowBlock);
            }

            return result;
        }

        [Theory]
        [MemberData(nameof(ResolveBlockName_ResolvesBlockName_Data))]
        public void ResolveBlockName_ResolvesBlockName(string dummyBlockName, string expectedResult)
        {
            // Arrange
            FlexiTableBlockFactory testSubject = CreateFlexiTableBlockFactory();

            // Act
            string result = testSubject.ResolveBlockName(dummyBlockName);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ResolveBlockName_ResolvesBlockName_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string defaultBlockName = "flexi-table";

            return new object[][]
            {
                new object[]{dummyBlockName, dummyBlockName},
                new object[]{null, defaultBlockName},
                new object[]{" ", defaultBlockName},
                new object[]{string.Empty, defaultBlockName}
            };
        }

        [Fact]
        public void ValidateType_ThrowsOptionsExceptionIfTypeIsInvalid()
        {
            // Arrange
            FlexiTableBlockFactory testSubject = CreateFlexiTableBlockFactory();
            const FlexiTableType dummyType = (FlexiTableType)9;

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => testSubject.ValidateType(dummyType));
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                            nameof(IFlexiTableBlockOptions.Type),
                            string.Format(Strings.OptionsException_Shared_ValueMustBeAValidEnumValue, dummyType,
                                nameof(FlexiTableType))),
                        result.Message);
        }

        [Theory]
        [MemberData(nameof(CreateFlexiTableBlock_CreatesFlexiTableBlock_Data))]
        public void CreateFlexiTableBlock_CreatesFlexiTableBlock(FlexiTableType dummyType,
            Row[] dummyRows)
        {
            // Arrange
            const int dummyNumColumns = 6;
            const int dummyColumn = 4;
            const int dummyLine = 5;
            const int dummyLineStart = 3;
            const int dummyLineEnd = 10;
            const string dummyBlockName = "dummyBlockName";
            BlockParser dummyBlockParser = _mockRepository.Create<BlockParser>().Object;
            var dummyColumnDefinitions = new List<ColumnDefinition>();
            var dummyAttributes = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyProxyTableBlock = new ProxyTableBlock(dummyBlockParser);
            dummyProxyTableBlock.Column = dummyColumn;
            dummyProxyTableBlock.Line = dummyLine;
            dummyProxyTableBlock.Span = new SourceSpan(dummyLineStart, dummyLineEnd);
            foreach (Row row in dummyRows)
            {
                dummyProxyTableBlock.Rows.Add(row);
            }
            dummyProxyTableBlock.ColumnDefinitions = dummyColumnDefinitions;
            dummyProxyTableBlock.NumColumns = dummyNumColumns;
            var dummyFlexiTableRowBlock1 = new FlexiTableRowBlock(false);
            var dummyFlexiTableRowBlock2 = new FlexiTableRowBlock(false);
            var dummyFlexiTableRowBlock3 = new FlexiTableRowBlock(false);
            Mock<FlexiTableBlockFactory> mockTestSubject = CreateMockFlexiTableBlockFactory();
            mockTestSubject.CallBase = true;
            mockTestSubject.
                Setup(t => t.CreateFlexiTableRowBlock(dummyType, It.IsAny<BlockProcessor>(), dummyColumnDefinitions, dummyNumColumns, 0, dummyRows[0], dummyRows[0].IsHeaderRow)).
                Returns(dummyFlexiTableRowBlock1);
            mockTestSubject.
                Setup(t => t.CreateFlexiTableRowBlock(dummyType, It.IsAny<BlockProcessor>(), dummyColumnDefinitions, dummyNumColumns, 1, dummyRows[1], dummyRows[1].IsHeaderRow)).
                Returns(dummyFlexiTableRowBlock2);
            mockTestSubject.
                Setup(t => t.CreateFlexiTableRowBlock(dummyType, It.IsAny<BlockProcessor>(), dummyColumnDefinitions, dummyNumColumns, 2, dummyRows[2], dummyRows[2].IsHeaderRow)).
                Returns(dummyFlexiTableRowBlock3);

            // Act
            FlexiTableBlock result = mockTestSubject.Object.CreateFlexiTableBlock(dummyBlockName, dummyType, dummyAttributes, dummyProxyTableBlock, dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyBlockName, result.BlockName);
            Assert.Equal(dummyType, result.Type);
            Assert.Same(dummyAttributes, result.Attributes);
            Assert.Same(dummyBlockParser, result.Parser);
            Assert.Equal(dummyColumn, result.Column);
            Assert.Equal(dummyLine, result.Line);
            Assert.Equal(dummyLineStart, result.Span.Start);
            Assert.Equal(dummyLineEnd, result.Span.End);
            Assert.Equal(3, result.Count);
            Assert.Same(dummyFlexiTableRowBlock1, result[0]);
            Assert.Same(dummyFlexiTableRowBlock2, result[1]);
            Assert.Same(dummyFlexiTableRowBlock3, result[2]);
        }

        public static IEnumerable<object[]> CreateFlexiTableBlock_CreatesFlexiTableBlock_Data()
        {
            return new object[][]
            {
                // Non-unreponsive type tables can only have 1 header row
                new object[]
                {
                    FlexiTableType.Cards,
                    new Row[]
                    {
                        new Row(){IsHeaderRow = true},
                        new Row(){IsHeaderRow = false},
                        new Row(){IsHeaderRow = false}
                    }
                },
                new object[]
                {
                    FlexiTableType.FixedTitles,
                    new Row[]
                    {
                        new Row(){IsHeaderRow = true},
                        new Row(){IsHeaderRow = false},
                        new Row(){IsHeaderRow = false}
                    }
                },
                // Unresponsive type tables can have more than 1 header rows
                new object[]
                {
                    FlexiTableType.Unresponsive,
                    new Row[]
                    {
                        new Row(){IsHeaderRow = true},
                        new Row(){IsHeaderRow = true},
                        new Row(){IsHeaderRow = false}
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(CreateFlexiTableBlock_ThrowsOptionsExceptionIfTypeIsNotUnresponsiveAndTableHasMultipleHeaderRows_Data))]
        public void CreateFlexiTableBlock_ThrowsOptionsExceptionIfTypeIsNotUnresponsiveAndTableHasMultipleHeaderRows(FlexiTableType dummyType,
            Row[] dummyRows)
        {
            // Arrange
            const int dummyNumColumns = 6;
            var dummyColumnDefinitions = new List<ColumnDefinition>();
            var dummyAttributes = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyProxyTableBlock = new ProxyTableBlock(null);
            foreach (Row row in dummyRows)
            {
                dummyProxyTableBlock.Rows.Add(row);
            }
            dummyProxyTableBlock.ColumnDefinitions = dummyColumnDefinitions;
            dummyProxyTableBlock.NumColumns = dummyNumColumns;
            Mock<FlexiTableBlockFactory> mockTestSubject = CreateMockFlexiTableBlockFactory();
            mockTestSubject.CallBase = true;
            mockTestSubject.
                Setup(t => t.CreateFlexiTableRowBlock(dummyType, It.IsAny<BlockProcessor>(), dummyColumnDefinitions, dummyNumColumns, 0, dummyRows[0], dummyRows[0].IsHeaderRow)).
                Returns(new FlexiTableRowBlock(true));

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => mockTestSubject.Object.CreateFlexiTableBlock(default, dummyType, dummyAttributes, dummyProxyTableBlock, dummyBlockProcessor));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                    nameof(IFlexiTableBlockOptions.Type),
                    Strings.OptionsException_FlexiTableBlockFactory_TypeInvalidForTablesWithMultipleHeaderRows),
                result.Message);
        }

        public static IEnumerable<object[]> CreateFlexiTableBlock_ThrowsOptionsExceptionIfTypeIsNotUnresponsiveAndTableHasMultipleHeaderRows_Data()
        {
            return new object[][]
            {
                // OptionsException is thrown if non-responsive type tables have more than 1 header row
                new object[]
                {
                    FlexiTableType.Cards,
                    new Row[]
                    {
                        new Row(){IsHeaderRow = true},
                        new Row(){IsHeaderRow = true},
                        new Row(){IsHeaderRow = false}
                    }
                },
                new object[]
                {
                    FlexiTableType.FixedTitles,
                    new Row[]
                    {
                        new Row(){IsHeaderRow = true},
                        new Row(){IsHeaderRow = true},
                        new Row(){IsHeaderRow = false}
                    }
                }
            };
        }

        [Fact]
        public void CreateFlexiTableRowBlock_CreatesFlexiTableRowBlock()
        {
            // Arrange
            const FlexiTableType dummyFlexiTableType = FlexiTableType.FixedTitles;
            BlockProcessor dummyChildBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyColumnDefinitions = new List<ColumnDefinition>();
            const int dummyNumColumns = 4;
            const int dummyRowIndex = 1;
            const bool dummyIsHeaderRow = true;
            Cell dummyCell1 = CreateCell(0, 1, 1, 1); // Cell with colspan
            Cell dummyCell2 = CreateCell(2, 2, 0, 1); // Cell with rowspan
            Cell dummyCell3 = CreateCell(3, 3, 1, 1); // Cell with neither colspan nor rowspan
            var dummyRow = new Row() { dummyCell1, dummyCell1, dummyCell2, dummyCell3 };
            var dummyFlexiTableCellBlock1 = new FlexiTableCellBlock(0, 0, ContentAlignment.None);
            var dummyFlexiTableCellBlock2 = new FlexiTableCellBlock(0, 0, ContentAlignment.None);
            Mock<FlexiTableBlockFactory> mockTestSubject = CreateMockFlexiTableBlockFactory();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.CreateFlexiTableCellBlock(dummyFlexiTableType, dummyChildBlockProcessor, dummyColumnDefinitions, dummyCell1)).Returns(dummyFlexiTableCellBlock1);
            mockTestSubject.Setup(t => t.CreateFlexiTableCellBlock(dummyFlexiTableType, dummyChildBlockProcessor, dummyColumnDefinitions, dummyCell3)).Returns(dummyFlexiTableCellBlock2);

            // Act
            FlexiTableRowBlock result = mockTestSubject.
                Object.
                CreateFlexiTableRowBlock(dummyFlexiTableType, dummyChildBlockProcessor, dummyColumnDefinitions, dummyNumColumns, dummyRowIndex, dummyRow, dummyIsHeaderRow);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(2, result.Count);
            Assert.Same(dummyFlexiTableCellBlock1, result[0]);
            Assert.Same(dummyFlexiTableCellBlock2, result[1]);
        }

        [Theory]
        [MemberData(nameof(CreateFlexiTableCellBlock_ThrowsOptionsExceptionIfTypeIsNotUnresponsiveAndACellsHasColumnOrRowSpan_Data))]
        public void CreateFlexiTableCellBlock_ThrowsOptionsExceptionIfTypeIsNotUnresponsiveAndACellsHasColumnOrRowSpan(FlexiTableType dummyType,
            Cell dummyCell)
        {
            // Arrange
            FlexiTableBlockFactory testSubject = CreateFlexiTableBlockFactory();

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => testSubject.CreateFlexiTableCellBlock(dummyType, null, null, dummyCell));
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                    nameof(IFlexiTableBlockOptions.Type),
                    Strings.OptionsException_FlexiTableBlockFactory_TypeInvalidForTablesWithCellsThatHaveRowspanOrColspan),
                result.Message);
        }

        public static IEnumerable<object[]> CreateFlexiTableCellBlock_ThrowsOptionsExceptionIfTypeIsNotUnresponsiveAndACellsHasColumnOrRowSpan_Data()
        {
            return new object[][]
            {
                new object[] { FlexiTableType.Cards, CreateCell(0, 1) },
                new object[] { FlexiTableType.Cards, CreateCell(startRowIndex: 0, endRowIndex: 1) },
                new object[] { FlexiTableType.FixedTitles, CreateCell(0, 1) },
                new object[] { FlexiTableType.FixedTitles, CreateCell(startRowIndex: 0, endRowIndex: 1) }
            };
        }

        [Theory]
        [MemberData(nameof(CreateFlexiTableCellBlock_CreatesFlexiTableCellBlock_Data))]
        public void CreateFlexiTableCellBlock_CreatesFlexiTableCellBlock(FlexiTableType dummyFlexiTableType,
            List<ColumnDefinition> dummyColumnDefinitions,
            Cell dummyCell,
            int expectedColspan,
            int expectedRowspan,
            ContentAlignment expectedContentAlignment)
        {
            // Arrange
            BlockProcessor dummyChildBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            FlexiTableBlockFactory testSubject = CreateFlexiTableBlockFactory();

            // Act
            FlexiTableCellBlock result = testSubject.CreateFlexiTableCellBlock(dummyFlexiTableType, dummyChildBlockProcessor, dummyColumnDefinitions, dummyCell);

            // Assert
            Assert.Equal(expectedColspan, result.Colspan);
            Assert.Equal(expectedRowspan, result.Rowspan);
            Assert.Equal(expectedContentAlignment, result.ContentAlignment);
        }

        public static IEnumerable<object[]> CreateFlexiTableCellBlock_CreatesFlexiTableCellBlock_Data()
        {
            return new object[][]
            {
                // Cell with colspan and rowspan
                new object[]
                {
                    FlexiTableType.Unresponsive,
                    null,
                    CreateCell(1, 2, 2, 4),
                    2,
                    3,
                    ContentAlignment.None
                },
                // Cards type table cell with content alignment
                new object[]
                {
                    FlexiTableType.Cards,
                    new List<ColumnDefinition> { new ColumnDefinition(ContentAlignment.Start, 0, 1), new ColumnDefinition(ContentAlignment.Center, 0, 1) },
                    CreateCell(1, 1),
                    1,
                    1,
                    ContentAlignment.Center
                },
                // Fixed titles type table cell
                new object[]
                {
                    FlexiTableType.FixedTitles,
                    null,
                    CreateCell(1, 1),
                    1,
                    1,
                    ContentAlignment.None
                }
            };
        }

        // Can't be part of above theories since we have to verify that the expected blocks are created
        [Fact]
        public void CreateFlexiTableCellBlock_ProcessesCellChildren()
        {
            // Arrange
            const int dummyLineIndex = 6;
            const string dummyParagraph = "dummyParagraph";
            const string dummyCode = "dummyCode";
            Cell dummyCell = CreateCell(lineIndex: 6);
            using (var stringReader = new StringReader($@"{dummyParagraph}

```
{dummyCode}
```
"))
            {
                string line;
                while ((line = stringReader.ReadLine()) != null)
                {
                    dummyCell.Lines.Add(new StringSlice(line));
                }
            }
            // We have to create child so there is no root markdown document block, otherwise BlockProcessor will try to continue FlexiTableCellBlocki
            BlockProcessor dummyChildBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor().CreateChild();
            FlexiTableBlockFactory testSubject = CreateFlexiTableBlockFactory();

            // Act
            FlexiTableCellBlock result = testSubject.CreateFlexiTableCellBlock(FlexiTableType.Unresponsive, dummyChildBlockProcessor, null, dummyCell);

            // Assert
            Assert.Equal(2, result.Count);
            var resultParagraphBlock = result[0] as ParagraphBlock;
            Assert.NotNull(resultParagraphBlock);
            Assert.Equal(dummyParagraph, resultParagraphBlock.Lines.ToString());
            Assert.Equal(dummyLineIndex, resultParagraphBlock.Line);
            var resultCodeBlock = result[1] as CodeBlock;
            Assert.NotNull(resultCodeBlock);
            Assert.Equal(dummyCode, resultCodeBlock.Lines.ToString());
            Assert.Equal(dummyLineIndex + 2, resultCodeBlock.Line);
        }

        private Mock<FlexiTableBlockFactory> CreateMockFlexiTableBlockFactory(IOptionsService<IFlexiTableBlockOptions, IFlexiTableBlocksExtensionOptions> optionsService = null)
        {
            return _mockRepository.
                Create<FlexiTableBlockFactory>(optionsService ?? _mockRepository.Create<IOptionsService<IFlexiTableBlockOptions, IFlexiTableBlocksExtensionOptions>>().Object);
        }

        private FlexiTableBlockFactory CreateFlexiTableBlockFactory(IOptionsService<IFlexiTableBlockOptions, IFlexiTableBlocksExtensionOptions> optionsService = null)
        {
            return new FlexiTableBlockFactory(optionsService ?? _mockRepository.Create<IOptionsService<IFlexiTableBlockOptions, IFlexiTableBlocksExtensionOptions>>().Object);
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
    }
}
