using Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiAlertBlocks
{
    public class FlexiAlertBlockParserUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void TryOpenFlexiBlock_ReturnsBlockStateNoneIfCurrentLineHasCodeIndent()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = 4; // IsCodeIndent is an expression bodied member that derives its value from Column
            FlexiAlertBlockParser flexiAlertBlockParser = CreateFlexiAlertBlockParser();

            // Act
            BlockState result = flexiAlertBlockParser.TryOpenFlexiBlock(dummyBlockProcessor);

            // Assert
            Assert.Equal(BlockState.None, result);
        }

        [Fact]
        public void TryOpenFlexiBlock_IfSuccessfulCreatesNewFlexiAlertBlockAndReturnsBlockStateContinue()
        {
            // Arrange
            const int dummyInitialColumn = 2;
            const int dummyInitialStart = 1;
            var dummyStringSlice = new StringSlice("dummyString") { Start = dummyInitialStart };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyStringSlice;
            dummyBlockProcessor.Column = dummyInitialColumn;
            var dummyFlexiAlertBlockOptions = new FlexiAlertBlockOptions();
            Mock<FlexiAlertBlockParser> mockFlexiAlertBlockParser = CreateMockFlexiAlertBlockParser();
            mockFlexiAlertBlockParser.CallBase = true;
            mockFlexiAlertBlockParser.Setup(a => a.CreateFlexiAlertBlockOptions(dummyBlockProcessor)).Returns(dummyFlexiAlertBlockOptions);

            // Act
            BlockState result = mockFlexiAlertBlockParser.Object.TryOpenFlexiBlock(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.Continue, result);
            Assert.Equal(dummyInitialStart + 1, dummyBlockProcessor.Line.Start); // Skips '!'
            Assert.Equal(dummyInitialColumn + 1, dummyBlockProcessor.Column); // Skips '!'
            var resultFlexiAlertBlock = dummyBlockProcessor.NewBlocks.Peek() as FlexiAlertBlock;
            Assert.NotNull(resultFlexiAlertBlock);
            Assert.Same(dummyFlexiAlertBlockOptions, resultFlexiAlertBlock.FlexiAlertBlockOptions);
            Assert.Equal(dummyInitialColumn, resultFlexiAlertBlock.Column); // Includes '!'
            Assert.Equal(dummyInitialStart, resultFlexiAlertBlock.Span.Start); // Includes '!'
            Assert.Equal(dummyStringSlice.End, resultFlexiAlertBlock.Span.End);
        }

        [Fact]
        public void TryContinueFlexiBlock_ReturnsBlockStateNoneIfCurrentLineHasCodeIndent()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = 4; // IsCodeIndent is an expression bodied member that derives its value from Column
            FlexiAlertBlockParser flexiAlertBlockParser = CreateFlexiAlertBlockParser();

            // Act
            BlockState result = flexiAlertBlockParser.TryContinueFlexiBlock(dummyBlockProcessor, null);

            // Assert
            Assert.Equal(BlockState.None, result);
        }

        [Fact]
        public void TryContinueFlexiBlock_ReturnsBlockStateNoneIfCurrentLineDoesNotBeginWithExclamationMarkAndIsNotBlank()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice("dummyString");
            FlexiAlertBlockParser flexiAlertBlockParser = CreateFlexiAlertBlockParser();

            // Act
            BlockState result = flexiAlertBlockParser.TryContinueFlexiBlock(dummyBlockProcessor, null);

            // Assert
            Assert.Equal(BlockState.None, result);
        }

        [Fact]
        public void TryContinueFlexiBlock_ReturnsBlockStateBreakDiscardIfCurrentLineIsBlank()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice("");
            FlexiAlertBlockParser flexiAlertBlockParser = CreateFlexiAlertBlockParser();

            // Act
            BlockState result = flexiAlertBlockParser.TryContinueFlexiBlock(dummyBlockProcessor, null);

            // Assert
            Assert.Equal(BlockState.BreakDiscard, result);
        }

        [Fact]
        public void TryContinueFlexiBlock_ReturnsBlockStateContinueIfBlockCanBeContinued()
        {
            // Arrange
            var dummyStringSlice = new StringSlice("!dummyString");
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyStringSlice;
            var dummyFlexiAlertBlock = new FlexiAlertBlock(null);
            FlexiAlertBlockParser flexiAlertBlockParser = CreateFlexiAlertBlockParser();

            // Act
            BlockState result = flexiAlertBlockParser.TryContinueFlexiBlock(dummyBlockProcessor, dummyFlexiAlertBlock);

            // Assert
            Assert.Equal(BlockState.Continue, result);
            Assert.Equal(dummyStringSlice.End, dummyFlexiAlertBlock.Span.End);
            Assert.Equal(dummyStringSlice.Start + 1, dummyBlockProcessor.Start); // Skips !
        }

        [Fact]
        public void CreateFlexiAlertBlockOptions_RetrievesIconMarkupFromExtensionOptionsIfItIsntSpecified()
        {
            // Arrange
            const int dummyLineIndex = 1;
            const string dummyAlertType = "dummyAlertType";
            const string dummyIconMarkup = "dummyIconMarkup";
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.LineIndex = dummyLineIndex;
            Mock<IFlexiOptionsBlockService> mockFlexiOptionsBlockService = _mockRepository.Create<IFlexiOptionsBlockService>();
            mockFlexiOptionsBlockService.Setup(j => j.TryPopulateOptions(dummyBlockProcessor, It.IsAny<FlexiAlertBlockOptions>(), dummyLineIndex));
            var dummyExtensionOptions = new FlexiAlertBlocksExtensionOptions { DefaultBlockOptions = new FlexiAlertBlockOptions(alertType: dummyAlertType) };
            dummyExtensionOptions.IconMarkups[dummyAlertType] = dummyIconMarkup;
            Mock<IOptions<FlexiAlertBlocksExtensionOptions>> mockExtensionOptionsAccessor = _mockRepository.Create<IOptions<FlexiAlertBlocksExtensionOptions>>();
            mockExtensionOptionsAccessor.Setup(e => e.Value).Returns(dummyExtensionOptions);
            FlexiAlertBlockParser flexiAlertBlockParser = CreateFlexiAlertBlockParser(mockExtensionOptionsAccessor.Object, mockFlexiOptionsBlockService.Object);

            // Act
            FlexiAlertBlockOptions result = flexiAlertBlockParser.CreateFlexiAlertBlockOptions(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyIconMarkup, result.IconMarkup);
        }

        private FlexiAlertBlockParser CreateFlexiAlertBlockParser(IOptions<FlexiAlertBlocksExtensionOptions> extensionOptionsAccessor = null,
            IFlexiOptionsBlockService flexiOptionsBlockService = null)
        {
            return new FlexiAlertBlockParser(
                extensionOptionsAccessor,
                flexiOptionsBlockService ?? new FlexiOptionsBlockService(null));
        }

        private Mock<FlexiAlertBlockParser> CreateMockFlexiAlertBlockParser(IOptions<FlexiAlertBlocksExtensionOptions> extensionOptionsAccessor = null,
            IFlexiOptionsBlockService flexiOptionsBlockService = null)
        {
            return _mockRepository.Create<FlexiAlertBlockParser>(
                extensionOptionsAccessor,
                flexiOptionsBlockService ?? new FlexiOptionsBlockService(null));
        }
    }
}
