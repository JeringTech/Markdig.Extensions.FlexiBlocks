using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiOptionsBlocks
{
    public class FlexiOptionsBlockParserUnitTests
    {
        [Fact]
        public void TryOpenFlexiBlock_ReturnsBlockStateNoneIfInCodeIndent()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            // These three lines just set IsCodeIndent to true
            dummyBlockProcessor.Column = 0;
            dummyBlockProcessor.RestartIndent();
            dummyBlockProcessor.Column = 4;
            var flexiOptionsBlockParser = new FlexiOptionsBlockParser();

            // Act
            BlockState result = flexiOptionsBlockParser.TryOpen(dummyBlockProcessor);

            // Assert
            Assert.True(dummyBlockProcessor.IsCodeIndent);
            Assert.Equal(BlockState.None, result);
        }

        [Theory]
        [MemberData(nameof(TryOpenFlexiBlock_ReturnsBlockStateNoneIfLineDoesNotBeginWithExpectedCharacters_Data))]
        public void TryOpenFlexiBlock_ReturnsBlockStateNoneIfLineDoesNotBeginWithExpectedCharacters(string line)
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(line);
            var flexiOptionsBlockParser = new FlexiOptionsBlockParser();

            // Act
            BlockState result = flexiOptionsBlockParser.TryOpen(dummyBlockProcessor);

            // Assert
            Assert.Equal(BlockState.None, result);
        }

        public static IEnumerable<object[]> TryOpenFlexiBlock_ReturnsBlockStateNoneIfLineDoesNotBeginWithExpectedCharacters_Data()
        {
            return new object[][]
            {
                // Character after @ must be an opening brace
                new string[]{"@a"},
                // No whitespace between @ and {
                new string[]{"@ {"},
                new string[]{"@\n{"}
            };
        }

        [Fact]
        public void TryOpenFlexiBlock_SetsProcessorLineStartCreatesFlexiOptionsBlockAndReturnsBlockStateIfSuccessful()
        {
            // Arrange
            const int dummyColumn = 1;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = dummyColumn;
            dummyBlockProcessor.Line = new StringSlice("@{dummy");
            var testSubject = new FlexiOptionsBlockParser();

            // Act
            BlockState result = testSubject.TryOpen(dummyBlockProcessor);

            // Assert
            Assert.Equal(BlockState.Continue, result);
            Assert.Equal(1, dummyBlockProcessor.Line.Start);
            Assert.Single(dummyBlockProcessor.NewBlocks);
            Block block = dummyBlockProcessor.NewBlocks.Peek();
            Assert.IsType<FlexiOptionsBlock>(block);
            Assert.Equal(dummyColumn, block.Column);
            Assert.Equal(0, block.Span.Start);
        }

        [Fact]
        public void CloseFlexiBlock_ThrowsFlexiBlocksExceptionIfThereIsAnUncomsumedFlexiOptionsBlock()
        {
            // Arrange
            const int dummyLineIndex = 1;
            const int dummyColumn = 2;
            var dummyPendingFlexiOptionsBlock = new FlexiOptionsBlock(null)
            {
                Line = dummyLineIndex,
                Column = dummyColumn
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Document.SetData(FlexiOptionsBlockParser.PENDING_FLEXI_OPTIONS_BLOCK, dummyPendingFlexiOptionsBlock);
            var flexiOptionsBlockParser = new FlexiOptionsBlockParser();

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => flexiOptionsBlockParser.Close(dummyBlockProcessor, null));
            Assert.Equal(string.Format(Strings.FlexiBlocksException_InvalidFlexiBlock,
                    typeof(FlexiOptionsBlock).Name,
                    dummyLineIndex + 1,
                    dummyColumn,
                    Strings.FlexiBlocksException_UnconsumedFlexiOptionsBlock),
                result.Message);
        }

        [Fact]
        public void CloseFlexiBlock_AddsFlexiOptionsBlockToDocumentData()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyFlexiOptionsBlock = new FlexiOptionsBlock(null);
            var flexiOptionsBlockParser = new FlexiOptionsBlockParser();

            // Act
            bool result = flexiOptionsBlockParser.Close(dummyBlockProcessor, dummyFlexiOptionsBlock);

            // Assert
            Assert.False(result);
            Assert.Same(dummyFlexiOptionsBlock, dummyBlockProcessor.Document.GetData(FlexiOptionsBlockParser.PENDING_FLEXI_OPTIONS_BLOCK));
        }
    }
}
