using Jering.Markdig.Extensions.FlexiBlocks.Alerts;
using Markdig.Helpers;
using Markdig.Parsers;
using Moq;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.Alerts
{
    public class FlexiAlertParserUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiAlertFactoryIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiAlertParser(null));
        }

        [Fact]
        public void TryOpenBlock_ReturnsBlockStateNoneIfCurrentLineHasCodeIndent()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = 4; // IsCodeIndent is an expression bodied member that derives its value from Column
            ExposedFlexiAlertParser testSubject = CreateExposedFlexiAlertParser();

            // Act
            BlockState result = testSubject.ExposedTryOpenBlock(dummyBlockProcessor);

            // Assert
            Assert.Equal(BlockState.None, result);
        }

        [Fact]
        public void TryOpenBlock_IfSuccessfulCreatesNewFlexiAlertAndReturnsBlockStateContinue()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice("dummyString");
            var dummyFlexiAlert = new FlexiAlert(null, _mockRepository.Create<IFlexiAlertOptions>().Object, null);
            Mock<IBlockFactory<FlexiAlert>> mockFlexiAlertFactory = _mockRepository.Create<IBlockFactory<FlexiAlert>>();
            Mock<ExposedFlexiAlertParser> mockTestSubject = CreateMockExposedFlexiAlertParser(mockFlexiAlertFactory.Object);
            mockTestSubject.CallBase = true;
            mockFlexiAlertFactory.Setup(f => f.Create(dummyBlockProcessor, mockTestSubject.Object)).Returns(dummyFlexiAlert);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryOpenBlock(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.Continue, result);
            var resultFlexiAlert = dummyBlockProcessor.NewBlocks.Peek() as FlexiAlert;
            Assert.Same(dummyFlexiAlert, resultFlexiAlert);
        }

        [Fact]
        public void TryContinueBlock_ReturnsBlockStateNoneIfCurrentLineHasCodeIndent()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = 4; // IsCodeIndent is an expression bodied member that derives its value from Column
            ExposedFlexiAlertParser testSubject = CreateExposedFlexiAlertParser();

            // Act
            BlockState result = testSubject.ExposedTryContinueBlock(dummyBlockProcessor, null);

            // Assert
            Assert.Equal(BlockState.None, result);
        }

        [Fact]
        public void TryContinueBlock_ReturnsBlockStateNoneIfCurrentLineDoesNotBeginWithExclamationMarkAndIsNotBlank()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice("dummyString");
            ExposedFlexiAlertParser testSubject = CreateExposedFlexiAlertParser();

            // Act
            BlockState result = testSubject.ExposedTryContinueBlock(dummyBlockProcessor, null);

            // Assert
            Assert.Equal(BlockState.None, result);
        }

        [Fact]
        public void TryContinueBlock_ReturnsBlockStateBreakDiscardIfCurrentLineIsBlank()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice("");
            ExposedFlexiAlertParser testSubject = CreateExposedFlexiAlertParser();

            // Act
            BlockState result = testSubject.ExposedTryContinueBlock(dummyBlockProcessor, null);

            // Assert
            Assert.Equal(BlockState.BreakDiscard, result);
        }

        [Fact]
        public void TryContinueBlock_UpdatesFlexiAlertAndReturnsBlockStateContinueIfFlexiAlertCanBeContinued()
        {
            // Arrange
            var dummyStringSlice = new StringSlice("!dummyString");
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyStringSlice;
            var dummyFlexiAlert = new FlexiAlert(null, _mockRepository.Create<IFlexiAlertOptions>().Object, null);
            ExposedFlexiAlertParser testSubject = CreateExposedFlexiAlertParser();

            // Act
            BlockState result = testSubject.ExposedTryContinueBlock(dummyBlockProcessor, dummyFlexiAlert);

            // Assert
            Assert.Equal(BlockState.Continue, result);
            Assert.Equal(dummyStringSlice.End, dummyFlexiAlert.Span.End);
            Assert.Equal(dummyStringSlice.Start + 1, dummyBlockProcessor.Start); // Skips !
        }

        public class ExposedFlexiAlertParser : FlexiAlertParser
        {
            public ExposedFlexiAlertParser(IBlockFactory<FlexiAlert> flexiAlertFactory) : base(flexiAlertFactory)
            {
            }

            public BlockState ExposedTryOpenBlock(BlockProcessor processor)
            {
                return TryOpenBlock(processor);
            }

            public BlockState ExposedTryContinueBlock(BlockProcessor processor, FlexiAlert block)
            {
                return TryContinueBlock(processor, block);
            }
        }

        private ExposedFlexiAlertParser CreateExposedFlexiAlertParser(IBlockFactory<FlexiAlert> flexiAlertFactory = null)
        {
            return new ExposedFlexiAlertParser(flexiAlertFactory ?? _mockRepository.Create<IBlockFactory<FlexiAlert>>().Object);
        }

        private Mock<ExposedFlexiAlertParser> CreateMockExposedFlexiAlertParser(IBlockFactory<FlexiAlert> flexiAlertFactory = null)
        {
            return _mockRepository.Create<ExposedFlexiAlertParser>(flexiAlertFactory ?? _mockRepository.Create<IBlockFactory<FlexiAlert>>().Object);
        }
    }
}
