using JeremyTCD.Markdig.Extensions.JsonOptions;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace JeremyTCD.Markdig.Extensions.Tests
{
    public class JsonParserIntegrationTests
    {
        [Fact]
        public void TryOpen_ReturnsBlockStateNoneIfInCodeIndent()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = CreateBlockProcessor();
            // These three lines just set IsCodeIndent to true
            dummyBlockProcessor.Column = 0;
            dummyBlockProcessor.RestartIndent();
            dummyBlockProcessor.Column = 4;
            JsonOptionsParser jsonOptionsParser = new JsonOptionsParser();

            // Act
            BlockState result = jsonOptionsParser.TryOpen(dummyBlockProcessor);

            // Assert
            Assert.True(dummyBlockProcessor.IsCodeIndent);
            Assert.Equal(BlockState.None, result);
        }

        [Theory]
        [MemberData(nameof(TryOpen_ReturnsBlockStateNoneIfLineDoesNotBeginWithExpectedCharacters_Data))]
        public void TryOpen_ReturnsBlockStateNoneIfLineDoesNotBeginWithExpectedCharacters(string line)
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(line);
            JsonOptionsParser jsonOptionsParser = new JsonOptionsParser();

            // Act
            BlockState result = jsonOptionsParser.TryOpen(dummyBlockProcessor);

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
        public void TryOpen_SetsProcessorLineStartCreatesJsonOptionsBlockAndReturnsBlockStateIfSuccessful()
        {
            // Arrange
            int dummyColumn = 1;
            BlockProcessor dummyBlockProcessor = CreateBlockProcessor();
            dummyBlockProcessor.Column = dummyColumn;
            dummyBlockProcessor.Line = new StringSlice("@{dummy");
            BlockState dummyBlockState = BlockState.Continue;
            Mock<JsonOptionsParser> mockJsonOptionsParser = new Mock<JsonOptionsParser>
            {
                CallBase = true
            };
            mockJsonOptionsParser.Setup(j => j.TryContinue(dummyBlockProcessor, It.IsAny<JsonOptionsBlock>())).Returns(dummyBlockState);

            // Act
            BlockState result = mockJsonOptionsParser.Object.TryOpen(dummyBlockProcessor);

            // Assert
            Assert.Equal(dummyBlockState, result);
            Assert.Equal(1, dummyBlockProcessor.Line.Start);
            dummyBlockProcessor.NewBlocks.TryPop(out Block block);
            Assert.IsType<JsonOptionsBlock>(block);
            Assert.Equal(dummyColumn, block.Column);
            Assert.Equal(1, block.Span.Start);
        }

        [Theory]
        [MemberData(nameof(TryContinue_ReturnsBlockStateBreakAndSetsBlockSpaneEndIfLineIsACompleteJsonString_Data))]
        public void TryContinue_ReturnsBlockStateBreakAndSetsBlockSpaneEndIfLineIsACompleteJsonString(string line)
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(line);
            JsonOptionsParser jsonOptionsParser = new JsonOptionsParser();
            JsonOptionsBlock jsonOptionsBlock = new JsonOptionsBlock(null);

            // Act
            BlockState result = jsonOptionsParser.TryContinue(dummyBlockProcessor, jsonOptionsBlock);

            // Assert
            Assert.Equal(BlockState.Break, result);
            Assert.Equal(line.Length - 1, jsonOptionsBlock.Span.End);
            Assert.False(jsonOptionsBlock.EndsInString);
            Assert.Equal(0, jsonOptionsBlock.NumOpenBrackets);
        }

        public static IEnumerable<object[]> TryContinue_ReturnsBlockStateBreakAndSetsBlockSpaneEndIfLineIsACompleteJsonString_Data()
        {
            return new object[][]
            {
                new string[]{"{\"option\": \"value\"}"},
                // Strings can contain curly brackets
                new string[]{"{\"option\": \"{value}\"}"},
                // Strings can contain escaped quotes
                new string[]{"{\"option\": \"\\\"value\\\"\"}"},
            };
        }

        [Theory]
        [MemberData(nameof(TryContinue_ReturnsBlockStateContinueIfLineIsAPartialJsonString_Data))]
        public void TryContinue_ReturnsBlockStateContinueIfLineIsAPartialJsonString(string line, bool endsInString, int numOpenBrackets)
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(line);
            JsonOptionsParser jsonOptionsParser = new JsonOptionsParser();
            JsonOptionsBlock jsonOptionsBlock = new JsonOptionsBlock(null);

            // Act
            BlockState result = jsonOptionsParser.TryContinue(dummyBlockProcessor, jsonOptionsBlock);

            // Assert
            Assert.Equal(BlockState.Continue, result);
            Assert.Equal(endsInString, jsonOptionsBlock.EndsInString);
            Assert.Equal(numOpenBrackets, jsonOptionsBlock.NumOpenBrackets);
        }

        public static IEnumerable<object[]> TryContinue_ReturnsBlockStateContinueIfLineIsAPartialJsonString_Data()
        {
            return new object[][]
            {
                new object[]{"{\"option\": \"value\",", false, 1},
                new object[]{"{\"option\": \"val", true, 1},
                new object[]{"{\"option\": { \"subOption\":", false, 2},
            };
        }


        // BlockProcessor can't be mocked since its members aren't virtual. Markdig does not apply IOC conventions either, so there is not interface to mock.
        private BlockProcessor CreateBlockProcessor()
        {
            StringBuilderCache stringBuilderCache = new StringBuilderCache();
            MarkdownDocument markdownDocument = new MarkdownDocument();
            BlockParserList blockParserList = new BlockParserList(new BlockParser[0]);

            return new BlockProcessor(stringBuilderCache, markdownDocument, blockParserList);
        }
    }
}
