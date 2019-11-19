using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiCodeBlocks
{
    public class IndentedFlexiCodeBlockParserUnitTests
    {
        private static readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiCodeBlockFactoryIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new IndentedFlexiCodeBlockParser(null));
        }

        [Theory]
        [MemberData(nameof(CanInterrupt_ReturnsFalseIfBlockIsAParagraphBlockReturnsTrueOtherwise_Data))]
        public void CanInterrupt_ReturnsFalseIfBlockIsAParagraphBlockReturnsTrueOtherwise(Block dummyBlock, bool expectedResult)
        {
            // Arrange
            IndentedFlexiCodeBlockParser testSubject = CreateIndentedFlexiCodeBlockParser();

            // Act
            bool result = testSubject.CanInterrupt(null, dummyBlock);

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

        [Fact]
        public void TryOpenBlock_ParsesLine()
        {
            // Arrange
            const BlockState dummyBlockState = BlockState.Continue;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<ExposedIndentedFlexiCodeBlockParser> mockTestSubject = CreateMockExposedIndentedFlexiCodeBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.ParseLine(dummyBlockProcessor, null)).Returns(dummyBlockState);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryOpenBlock(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyBlockState, result);
        }

        [Fact]
        public void TryContinueBlock_ParsesLine()
        {
            // Arrange
            const BlockState dummyBlockState = BlockState.Continue;
            var dummyProxyLeafBlock = new ProxyLeafBlock(null, null);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<ExposedIndentedFlexiCodeBlockParser> mockTestSubject = CreateMockExposedIndentedFlexiCodeBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.ParseLine(dummyBlockProcessor, dummyProxyLeafBlock)).Returns(dummyBlockState);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryContinueBlock(dummyBlockProcessor, dummyProxyLeafBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyBlockState, result);
        }

        [Fact]
        public void CloseProxy_RemovesTrailingBlankLinesAndReturnsNewFlexiCodeBlock()
        {
            // Arrange
            var dummyLines = new StringLineGroup(3);
            dummyLines.Add(new StringSlice(string.Empty)); // Should not get removed
            dummyLines.Add(new StringSlice("dummy line"));
            dummyLines.Add(new StringSlice(string.Empty)); // Should get removed
            dummyLines.Add(new StringSlice(string.Empty)); // Should get removed
            var dummyProxyLeafBlock = new ProxyLeafBlock(null, null);
            dummyProxyLeafBlock.Lines = dummyLines;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyFlexiCodeBlock = new FlexiCodeBlock(default, default, default, default, default, default, default, default, default, default, default, default, default, default, default);
            Mock<IFlexiCodeBlockFactory> mockFlexiCodeBlockFactory = _mockRepository.Create<IFlexiCodeBlockFactory>();
            mockFlexiCodeBlockFactory.Setup(f => f.Create(dummyProxyLeafBlock, dummyBlockProcessor)).Returns(dummyFlexiCodeBlock);
            ExposedIndentedFlexiCodeBlockParser testSubject = CreateExposedIndentedFlexiCodeBlockParser(mockFlexiCodeBlockFactory.Object);

            // Act
            FlexiCodeBlock result = testSubject.ExposedCloseProxy(dummyBlockProcessor, dummyProxyLeafBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(2, dummyProxyLeafBlock.Lines.Count);
            Assert.Equal("\ndummy line", dummyProxyLeafBlock.Lines.ToString());
        }

        [Theory]
        [MemberData(nameof(ParseLine_ReturnsBlockStateNoneIfAProxyLeafBlockCannotBeOpenedOrAProxyLeafBlockMustBeClosed_Data))]
        public void ParseLine_ReturnsBlockStateNoneIfAProxyLeafBlockCannotBeOpenedOrAProxyLeafBlockMustBeClosed(StringSlice dummyCurrentLine,
            ProxyLeafBlock dummyProxyLeafBlock,
            int dummyColumn)
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyCurrentLine;
            dummyBlockProcessor.Column = dummyColumn;
            IndentedFlexiCodeBlockParser testSubject = CreateIndentedFlexiCodeBlockParser();

            // Act
            BlockState result = testSubject.ParseLine(dummyBlockProcessor, dummyProxyLeafBlock);

            // Assert
            Assert.Equal(BlockState.None, result);
        }

        public static IEnumerable<object[]> ParseLine_ReturnsBlockStateNoneIfAProxyLeafBlockCannotBeOpenedOrAProxyLeafBlockMustBeClosed_Data()
        {
            return new object[][]
            {
                // Line is empty - Don't open a block
                new object[]{new StringSlice(string.Empty), null, 0},
                // Indent < 4 - Don't open a block
                new object[]{new StringSlice("dummyLine"), null, 3},
                // Indent < 4 - Close block
                new object[]{new StringSlice("dummyLine"), new ProxyLeafBlock(null, null), 3}
            };
        }

        [Fact]
        public void ParseLine_CorrectsIndentCreatesAProxyLeafBlockAndReturnsBlockStateContinueIfAProxyLeafBlockCanBeOpened()
        {
            // Arrange
            var dummyProxyLeafBlock = new ProxyLeafBlock(null, null);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice("dummyLine");
            dummyBlockProcessor.Column = 6;
            Mock<IFlexiCodeBlockFactory> mockFlexiCodeBlockFactory = _mockRepository.Create<IFlexiCodeBlockFactory>();
            IndentedFlexiCodeBlockParser testSubject = CreateIndentedFlexiCodeBlockParser(mockFlexiCodeBlockFactory.Object);
            mockFlexiCodeBlockFactory.Setup(f => f.CreateProxyLeafBlock(dummyBlockProcessor, testSubject)).Returns(dummyProxyLeafBlock);

            // Act
            BlockState result = testSubject.ParseLine(dummyBlockProcessor, null);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.Continue, result);
            Assert.Equal(4, dummyBlockProcessor.Column); // Corrects indent
            Assert.Single(dummyBlockProcessor.NewBlocks);
            Assert.Same(dummyProxyLeafBlock, dummyBlockProcessor.NewBlocks.Pop());
        }

        [Fact]
        public void ParseLine_CorrectsIndentUpdatesSpanEndAndReturnsBlockStateContinueIfAProxyLeafBlockCanBeContinued()
        {
            // Arrange
            var dummyProxyLeafBlock = new ProxyLeafBlock(null, null);
            var dummyLine = new StringSlice("dummyLine");
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyLine;
            dummyBlockProcessor.Column = 5;
            IndentedFlexiCodeBlockParser testSubject = CreateIndentedFlexiCodeBlockParser();

            // Act
            BlockState result = testSubject.ParseLine(dummyBlockProcessor, dummyProxyLeafBlock);

            // Assert
            Assert.Equal(BlockState.Continue, result);
            Assert.Equal(4, dummyBlockProcessor.Column); // Corrects indent
            Assert.Equal(dummyProxyLeafBlock.Span.End, dummyLine.Length - 1);
        }

        private IndentedFlexiCodeBlockParser CreateIndentedFlexiCodeBlockParser(IFlexiCodeBlockFactory flexiCodeBlockFactory = null)
        {
            return new IndentedFlexiCodeBlockParser(flexiCodeBlockFactory ?? _mockRepository.Create<IFlexiCodeBlockFactory>().Object);
        }

        private ExposedIndentedFlexiCodeBlockParser CreateExposedIndentedFlexiCodeBlockParser(IFlexiCodeBlockFactory flexiCodeBlockFactory = null)
        {
            return new ExposedIndentedFlexiCodeBlockParser(flexiCodeBlockFactory ?? _mockRepository.Create<IFlexiCodeBlockFactory>().Object);
        }

        private Mock<ExposedIndentedFlexiCodeBlockParser> CreateMockExposedIndentedFlexiCodeBlockParser(IFlexiCodeBlockFactory flexiCodeBlockFactory = null)
        {
            return _mockRepository.Create<ExposedIndentedFlexiCodeBlockParser>(flexiCodeBlockFactory ?? _mockRepository.Create<IFlexiCodeBlockFactory>().Object);
        }

        public class ExposedIndentedFlexiCodeBlockParser : IndentedFlexiCodeBlockParser
        {
            public ExposedIndentedFlexiCodeBlockParser(IFlexiCodeBlockFactory flexiCodeBlockFactory) : base(flexiCodeBlockFactory)
            {
            }

            public BlockState ExposedTryOpenBlock(BlockProcessor blockProcessor)
            {
                return TryOpenBlock(blockProcessor);
            }

            public BlockState ExposedTryContinueBlock(BlockProcessor blockProcessor, ProxyLeafBlock proxyLeafBlock)
            {
                return TryContinueBlock(blockProcessor, proxyLeafBlock);
            }

            public FlexiCodeBlock ExposedCloseProxy(BlockProcessor blockProcessor, ProxyLeafBlock proxyLeafBlock)
            {
                return CloseProxy(blockProcessor, proxyLeafBlock);
            }
        }
    }
}
