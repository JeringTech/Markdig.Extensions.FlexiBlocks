using Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Moq;
using System;
using System.Collections.ObjectModel;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiAlertBlocks
{
    public class FlexiAlertBlockParserUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_SetsOpeningCharacters()
        {
            // Act
            var testSubject = new FlexiAlertBlockParser(_mockRepository.Create<IFlexiAlertBlockFactory>().Object);

            // Assert
            Assert.Single(testSubject.OpeningCharacters);
            Assert.Equal('!', testSubject.OpeningCharacters[0]);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiAlertBlockFactoryIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiAlertBlockParser(null));
        }

        [Fact]
        public void TryOpenBlock_ParsesLine()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            const BlockState dummyBlockState = BlockState.Continue;
            Mock<ExposedFlexiAlertBlockParser> mockTestSubject = CreateMockExposedFlexiAlertBlockParser();
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
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            FlexiAlertBlock dummyFlexiAlertBlock = CreateFlexiAlertBlock();
            const BlockState dummyBlockState = BlockState.Continue;
            Mock<ExposedFlexiAlertBlockParser> mockTestSubject = CreateMockExposedFlexiAlertBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.ParseLine(dummyBlockProcessor, dummyFlexiAlertBlock)).Returns(dummyBlockState);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryContinueBlock(dummyBlockProcessor, dummyFlexiAlertBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyBlockState, result);
        }

        [Fact]
        public void ParseLine_ReturnsBlockStateNoneIfCurrentLineHasCodeIndent()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = 4; // IsCodeIndent is an expression bodied member that derives its value from Column
            ExposedFlexiAlertBlockParser testSubject = CreateExposedFlexiAlertBlockParser();

            // Act
            BlockState result = testSubject.ParseLine(dummyBlockProcessor, null);

            // Assert
            Assert.Equal(BlockState.None, result);
        }

        [Fact]
        public void ParseLine_ReturnsBlockStateNoneIfCurrentLineDoesNotBeginWithExclamationMarkAndIsNotBlank()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice("dummyString");
            ExposedFlexiAlertBlockParser testSubject = CreateExposedFlexiAlertBlockParser();

            // Act
            BlockState result = testSubject.ParseLine(dummyBlockProcessor, null);

            // Assert
            Assert.Equal(BlockState.None, result);
        }

        [Fact]
        public void ParseLine_ReturnsBlockStateBreakDiscardIfCurrentLineIsBlank()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(string.Empty);
            ExposedFlexiAlertBlockParser testSubject = CreateExposedFlexiAlertBlockParser();

            // Act
            BlockState result = testSubject.ParseLine(dummyBlockProcessor, null);

            // Assert
            Assert.Equal(BlockState.BreakDiscard, result);
        }

        [Fact]
        public void ParseLine_ReturnsBlockStateNoneIfSecondCharacterIsOpeningSquareBracket()
        {
            // Arrange
            var dummyLine = new StringSlice("![");
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyLine;
            ExposedFlexiAlertBlockParser testSubject = CreateExposedFlexiAlertBlockParser();

            // Act
            BlockState result = testSubject.ParseLine(dummyBlockProcessor, null);

            // Assert
            Assert.Equal(BlockState.None, result);
        }

        [Fact]
        public void ParseLine_IfSuccessfullyParsedStartLineOfABlockCreatesNewFlexiAlertBlockAdvancesCurrentCharAndReturnsBlockStateContinue()
        {
            // Arrange
            var dummyLine = new StringSlice("! dummyLine");
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyLine;
            FlexiAlertBlock dummyFlexiAlertBlock = CreateFlexiAlertBlock();
            Mock<IFlexiAlertBlockFactory> mockFlexiAlertBlockFactory = _mockRepository.Create<IFlexiAlertBlockFactory>();
            ExposedFlexiAlertBlockParser testSubject = CreateExposedFlexiAlertBlockParser(mockFlexiAlertBlockFactory.Object);
            mockFlexiAlertBlockFactory.Setup(f => f.Create(dummyBlockProcessor, testSubject)).Returns(dummyFlexiAlertBlock);

            // Act
            BlockState result = testSubject.ParseLine(dummyBlockProcessor, null);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.Continue, result);
            var resultBlock = dummyBlockProcessor.NewBlocks.Peek() as FlexiAlertBlock;
            Assert.Same(dummyFlexiAlertBlock, resultBlock);
            Assert.Equal(2, dummyBlockProcessor.Start); // Skips ! and first space
        }

        [Fact]
        public void ParseLine_UpdatesFlexiAlertBlockAndReturnsBlockStateContinueIfFlexiAlertBlockCanBeContinued()
        {
            // Arrange
            var dummyLine = new StringSlice("! dummyLine");
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyLine;
            FlexiAlertBlock dummyFlexiAlertBlock = CreateFlexiAlertBlock();
            ExposedFlexiAlertBlockParser testSubject = CreateExposedFlexiAlertBlockParser();

            // Act
            BlockState result = testSubject.ParseLine(dummyBlockProcessor, dummyFlexiAlertBlock);

            // Assert
            Assert.Equal(BlockState.Continue, result);
            Assert.Equal(dummyLine.End, dummyFlexiAlertBlock.Span.End);
            Assert.Equal(2, dummyBlockProcessor.Start); // Skips ! and first space
        }

        public class ExposedFlexiAlertBlockParser : FlexiAlertBlockParser
        {
            public ExposedFlexiAlertBlockParser(IFlexiAlertBlockFactory flexiAlertBlockFactory) : base(flexiAlertBlockFactory)
            {
            }

            public BlockState ExposedTryOpenBlock(BlockProcessor processor)
            {
                return TryOpenBlock(processor);
            }

            public BlockState ExposedTryContinueBlock(BlockProcessor processor, FlexiAlertBlock flexiAlertBlock)
            {
                return TryContinueBlock(processor, flexiAlertBlock);
            }
        }

        private FlexiAlertBlock CreateFlexiAlertBlock(string blockName = null,
            string type = null,
            string icon = null,
            ReadOnlyDictionary<string, string> attributes = null,
            BlockParser blockParser = null)
        {
            return new FlexiAlertBlock(blockName, type, icon, attributes, blockParser);
        }

        private ExposedFlexiAlertBlockParser CreateExposedFlexiAlertBlockParser(IFlexiAlertBlockFactory flexiAlertBlockFactory = null)
        {
            return new ExposedFlexiAlertBlockParser(flexiAlertBlockFactory ?? _mockRepository.Create<IFlexiAlertBlockFactory>().Object);
        }

        private Mock<ExposedFlexiAlertBlockParser> CreateMockExposedFlexiAlertBlockParser(IFlexiAlertBlockFactory flexiAlertBlockFactory = null)
        {
            return _mockRepository.Create<ExposedFlexiAlertBlockParser>(flexiAlertBlockFactory ?? _mockRepository.Create<IFlexiAlertBlockFactory>().Object);
        }
    }
}
