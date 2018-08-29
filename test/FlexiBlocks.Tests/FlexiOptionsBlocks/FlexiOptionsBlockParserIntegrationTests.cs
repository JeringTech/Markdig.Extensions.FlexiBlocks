using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiOptionsBlocks
{
    public class FlexiOptionsBlockParserIntegrationTests
    {
        [Fact]
        public void TryOpen_ReturnsBlockStateNoneIfInCodeIndent()
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
        [MemberData(nameof(TryOpen_ReturnsBlockStateNoneIfLineDoesNotBeginWithExpectedCharacters_Data))]
        public void TryOpen_ReturnsBlockStateNoneIfLineDoesNotBeginWithExpectedCharacters(string line)
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

        public static IEnumerable<object[]> TryOpen_ReturnsBlockStateNoneIfLineDoesNotBeginWithExpectedCharacters_Data()
        {
            return new object[][]
            {
                // No whitespace between @ and {
                new string[]{"@ {"},
                new string[]{"@\n{"}
            };
        }

        [Fact]
        public void TryOpen_SetsProcessorLineStartCreatesFlexiOptionsBlockAndReturnsBlockStateIfSuccessful()
        {
            // Arrange
            const int dummyColumn = 1;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = dummyColumn;
            dummyBlockProcessor.Line = new StringSlice("@{dummy");
            const BlockState dummyBlockState = BlockState.Continue;
            var mockFlexiOptionsBlockParser = new Mock<FlexiOptionsBlockParser>
            {
                CallBase = true
            };
            mockFlexiOptionsBlockParser.Setup(j => j.TryContinue(dummyBlockProcessor, It.IsAny<FlexiOptionsBlock>())).Returns(dummyBlockState);

            // Act
            BlockState result = mockFlexiOptionsBlockParser.Object.TryOpen(dummyBlockProcessor);

            // Assert
            Assert.Equal(dummyBlockState, result);
            Assert.Equal(1, dummyBlockProcessor.Line.Start);
            Assert.Single(dummyBlockProcessor.NewBlocks);
            Block block = dummyBlockProcessor.NewBlocks.Peek();
            Assert.IsType<FlexiOptionsBlock>(block);
            Assert.Equal(dummyColumn, block.Column);
            Assert.Equal(1, block.Span.Start);
        }

        [Fact]
        public void Close_ThrowsExceptionIfDocumentDataAlreadyContainsAFlexiOptionsBlock()
        {
            // Arrange
            const string dummyPendingJson = "dummyPendingJson";
            const int dummyPendingLine = 1;
            const int dummyPendingColumn = 2;
            var dummyPendingFlexiOptionsBlock = new FlexiOptionsBlock(null)
            {
                Lines = new StringLineGroup(dummyPendingJson),
                Line = dummyPendingLine,
                Column = dummyPendingColumn
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Document.SetData(FlexiOptionsBlockParser.FLEXI_OPTIONS_BLOCK, dummyPendingFlexiOptionsBlock);
            var flexiOptionsBlockParser = new FlexiOptionsBlockParser();

            // Act and assert
            InvalidOperationException result = Assert.Throws<InvalidOperationException>(() => flexiOptionsBlockParser.Close(dummyBlockProcessor, null));

            // Assert
            Assert.Equal(string.Format(Strings.InvalidOperationException_UnusedFlexiOptionsBlock, dummyPendingJson, dummyPendingLine, dummyPendingColumn), result.Message);
        }

        [Fact]
        public void Close_AddsFlexiOptionsBlockToDocumentData()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyFlexiOptionsBlock = new FlexiOptionsBlock(null);
            var flexiOptionsBlockParser = new FlexiOptionsBlockParser();

            // Act
            bool result = flexiOptionsBlockParser.Close(dummyBlockProcessor, dummyFlexiOptionsBlock);

            // Assert
            Assert.False(result);
            Assert.Same(dummyFlexiOptionsBlock, dummyBlockProcessor.Document.GetData(FlexiOptionsBlockParser.FLEXI_OPTIONS_BLOCK));
        }
    }
}
