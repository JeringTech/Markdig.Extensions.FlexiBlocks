using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiSectionBlocks
{
    public class FlexiSectionBlockParserIntegrationTests
    {
        private MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void TryOpen_ReturnsBlockStateNoneIfCurrentLineIsNotAHeadingBlock()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<HeadingBlockParser> mockHeadingBlockParser = _mockRepository.Create<HeadingBlockParser>();
            mockHeadingBlockParser.Setup(h => h.TryOpen(dummyBlockProcessor)).Returns(BlockState.None);
            FlexiSectionBlockParser flexiSectionBlockParser = CreateFlexiSectionBlockParser(headingBlockParser: mockHeadingBlockParser.Object);

            // Act
            BlockState result = flexiSectionBlockParser.TryOpen(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.None, result);
        }

        [Theory]
        [MemberData(nameof(TryOpen_SetsHeaderDataAndReturnsBlockStateBreakIfSectioningContentElementIsNoneOrUndefined_Data))]
        public void TryOpen_SetsHeaderDataAndReturnsBlockStateBreakIfSectioningContentElementIsNoneOrUndefined(SerializableWrapper<FlexiSectionBlockOptions> dummyFlexiSectionBlockOptionsWrapper)
        {
            // Arrange
            const int dummyHeadingLevel = 1;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<HeadingBlockParser> mockHeadingBlockParser = _mockRepository.Create<HeadingBlockParser>();
            mockHeadingBlockParser.Setup(h => h.TryOpen(dummyBlockProcessor)).Returns(BlockState.Break);
            var dummyHeadingBlock = new HeadingBlock(null) { Level = dummyHeadingLevel };
            dummyBlockProcessor.NewBlocks.Push(dummyHeadingBlock);
            Mock<FlexiSectionBlockParser> mockFlexiSectionBlockParser = CreateMockFlexiSectionBlockParser(headingBlockParser: mockHeadingBlockParser.Object);
            mockFlexiSectionBlockParser.CallBase = true;
            mockFlexiSectionBlockParser.Setup(s => s.CreateFlexiSectionBlockOptions(dummyBlockProcessor, dummyHeadingLevel)).Returns(dummyFlexiSectionBlockOptionsWrapper.Value);

            // Act
            BlockState result = mockFlexiSectionBlockParser.Object.TryOpen(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.Break, result);
            Assert.Equal(dummyFlexiSectionBlockOptionsWrapper.Value.HeaderIconMarkup, dummyHeadingBlock.GetData(FlexiSectionBlockParser.HEADER_ICON_MARKUP_KEY));
            Assert.Equal(dummyFlexiSectionBlockOptionsWrapper.Value.HeaderClassNameFormat, dummyHeadingBlock.GetData(FlexiSectionBlockParser.HEADER_CLASS_NAME_FORMAT_KEY));
        }

        public static IEnumerable<object[]> TryOpen_SetsHeaderDataAndReturnsBlockStateBreakIfSectioningContentElementIsNoneOrUndefined_Data()
        {
            return new object[][]
            {
                new object[]{
                    new SerializableWrapper<FlexiSectionBlockOptions>(
                        new FlexiSectionBlockOptions() { WrapperElement = SectioningContentElement.None}
                    )
                },
                new object[]{
                    new SerializableWrapper<FlexiSectionBlockOptions>(
                        new FlexiSectionBlockOptions() { WrapperElement = SectioningContentElement.Undefined }
                    )
                }
            };
        }

        [Fact]
        public void TryOpen_IfSuccessfulSetsHeaderDataCreatesNewFlexiSectionBlockAndReturnsBlockStateContinue()
        {
            // Arrange
            const int dummyLevel = 2;
            const int dummyColumn = 1;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<HeadingBlockParser> mockHeadingBlockParser = _mockRepository.Create<HeadingBlockParser>();
            mockHeadingBlockParser.Setup(h => h.TryOpen(dummyBlockProcessor)).Returns(BlockState.Break);
            var dummySourceSpan = new SourceSpan(3, 4);
            var dummyHeadingBlock = new HeadingBlock(null) { Level = dummyLevel, Column = dummyColumn, Span = dummySourceSpan };
            dummyBlockProcessor.NewBlocks.Push(dummyHeadingBlock);
            var dummyFlexiSectionBlockOptions = new FlexiSectionBlockOptions()
            {
                WrapperElement = SectioningContentElement.Section // Anything other than undefined and none to avoid break
            };
            Mock<FlexiSectionBlockParser> flexiSectionBlockParser = CreateMockFlexiSectionBlockParser(headingBlockParser: mockHeadingBlockParser.Object);
            flexiSectionBlockParser.CallBase = true;
            flexiSectionBlockParser.Setup(s => s.CreateFlexiSectionBlockOptions(dummyBlockProcessor, dummyLevel)).Returns(dummyFlexiSectionBlockOptions);

            // Act
            BlockState result = flexiSectionBlockParser.Object.TryOpen(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.Continue, result);
            var resultFlexiSectionBlock = dummyBlockProcessor.NewBlocks.Peek() as FlexiSectionBlock;
            Assert.NotNull(resultFlexiSectionBlock);
            Assert.Equal(dummyLevel, resultFlexiSectionBlock.Level);
            Assert.Equal(dummyColumn, resultFlexiSectionBlock.Column);
            Assert.Equal(dummySourceSpan, resultFlexiSectionBlock.Span); // SourceSpan is a struct, so object.Equals tests for value equality (not reference equality)
            Assert.Same(dummyFlexiSectionBlockOptions, resultFlexiSectionBlock.FlexiSectionBlockOptions);
            Assert.Equal(dummyFlexiSectionBlockOptions.HeaderIconMarkup, dummyHeadingBlock.GetData(FlexiSectionBlockParser.HEADER_ICON_MARKUP_KEY));
            Assert.Equal(dummyFlexiSectionBlockOptions.HeaderClassNameFormat, dummyHeadingBlock.GetData(FlexiSectionBlockParser.HEADER_CLASS_NAME_FORMAT_KEY));
        }

        [Fact]
        public void TryContinue_ReturnsBlockStateContinueIfTheCurrentCharIsNotTheOpeningCharOfAHeadingBlock()
        {
            // Arrange
            var dummyStringSlice = new StringSlice("@"); // Any first character but #
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyStringSlice;
            var dummyHeadingBlockParser = new HeadingBlockParser();
            FlexiSectionBlockParser flexiSectionBlockParser = CreateFlexiSectionBlockParser(headingBlockParser: dummyHeadingBlockParser);

            // Act
            BlockState result = flexiSectionBlockParser.TryContinue(dummyBlockProcessor, null);

            // Assert
            Assert.Equal(BlockState.Continue, result);
        }

        [Fact]
        public void TryContinue_ReturnsBlockStateContinueIfCurrentLineIsNotAHeadingBlock()
        {
            // Arrange
            var dummyStringSlice = new StringSlice("#");
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyStringSlice;
            Mock<HeadingBlockParser> mockHeadingBlockParser = _mockRepository.Create<HeadingBlockParser>();
            mockHeadingBlockParser.Setup(h => h.TryOpen(dummyBlockProcessor)).Returns(BlockState.None);
            FlexiSectionBlockParser flexiSectionBlockParser = CreateFlexiSectionBlockParser(headingBlockParser: mockHeadingBlockParser.Object);

            // Act
            BlockState result = flexiSectionBlockParser.TryContinue(dummyBlockProcessor, null);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.Continue, result);
        }

        [Theory]
        [MemberData(nameof(TryContinue_ReturnsBlockStateNoneAndRemovesNewHeadingBlockIfNewSectionIsNotAChildOfTheCurrentSection_Data))]
        public void TryContinue_ReturnsBlockStateNoneAndRemovesNewHeadingBlockIfNewSectionIsNotAChildOfTheCurrentSection(int dummyHeadingBlockLevel, int dummyFlexiSectionBlockLevel)
        {
            // Arrange
            var dummyStringSlice = new StringSlice("#");
            var dummyHeadingBlock = new HeadingBlock(null)
            {
                Level = dummyHeadingBlockLevel
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyStringSlice;
            dummyBlockProcessor.NewBlocks.Push(dummyHeadingBlock);
            Mock<HeadingBlockParser> mockHeadingBlockParser = _mockRepository.Create<HeadingBlockParser>();
            mockHeadingBlockParser.Setup(h => h.TryOpen(dummyBlockProcessor)).Returns(BlockState.Break);
            var dummyFlexiSectionBlock = new FlexiSectionBlock(null)
            {
                Level = dummyFlexiSectionBlockLevel
            };
            FlexiSectionBlockParser flexiSectionBlockParser = CreateFlexiSectionBlockParser(headingBlockParser: mockHeadingBlockParser.Object);

            // Act
            BlockState result = flexiSectionBlockParser.TryContinue(dummyBlockProcessor, dummyFlexiSectionBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.None, result);
            Assert.Empty(dummyBlockProcessor.NewBlocks);
        }

        public static IEnumerable<object[]> TryContinue_ReturnsBlockStateNoneAndRemovesNewHeadingBlockIfNewSectionIsNotAChildOfTheCurrentSection_Data()
        {
            return new object[][]
            {
                new object[]{1, 1},
                new object[]{1, 2}
            };
        }

        [Fact]
        public void TryContinue_ReturnsBlockStateContinueAndRemovesNewHeadingBlockIfNewSectionIsAChildOfTheCurrentSection()
        {
            // Arrange
            const int dummyHeadingBlockLevel = 2;
            const int dummyFlexiSectionBlockLevel = 1;
            var dummyStringSlice = new StringSlice("#");
            var dummyHeadingBlock = new HeadingBlock(null)
            {
                Level = dummyHeadingBlockLevel
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyStringSlice;
            dummyBlockProcessor.NewBlocks.Push(dummyHeadingBlock);
            Mock<HeadingBlockParser> mockHeadingBlockParser = _mockRepository.Create<HeadingBlockParser>();
            mockHeadingBlockParser.Setup(h => h.TryOpen(dummyBlockProcessor)).Returns(BlockState.Break);
            var dummyFlexiSectionBlock = new FlexiSectionBlock(null)
            {
                Level = dummyFlexiSectionBlockLevel
            };
            FlexiSectionBlockParser flexiSectionBlockParser = CreateFlexiSectionBlockParser(headingBlockParser: mockHeadingBlockParser.Object);

            // Act
            BlockState result = flexiSectionBlockParser.TryContinue(dummyBlockProcessor, dummyFlexiSectionBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.Continue, result);
            Assert.Empty(dummyBlockProcessor.NewBlocks);
        }

        [Theory]
        [MemberData(nameof(CreateFlexiSectionBlockOptions_UsesFlexiSectionBlocksExtensionOptionsWrapperElementsIfNecessary_Data))]
        public void CreateFlexiSectionBlockOptions_UsesFlexiSectionBlocksExtensionOptionsWrapperElementsIfNecessary(int dummyLevel, SectioningContentElement expectedWrapperElement)
        {
            // Arrange
            const int dummyLineIndex = 1;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.LineIndex = dummyLineIndex;
            var mockFlexiOptionsBlockService = new Mock<FlexiOptionsBlockService>();
            mockFlexiOptionsBlockService.Setup(j => j.TryPopulateOptions(dummyBlockProcessor, It.IsAny<FlexiSectionBlockOptions>(), dummyLineIndex)); // FlexiSectionBlockOptions will be a fresh instance (cloned)
            FlexiSectionBlockParser flexiSectionBlockParser = CreateFlexiSectionBlockParser();

            // Act
            FlexiSectionBlockOptions result = flexiSectionBlockParser.CreateFlexiSectionBlockOptions(dummyBlockProcessor, dummyLevel);

            // Assert
            _mockRepository.VerifyAll();
            Assert.NotNull(result);
            Assert.Equal(result.WrapperElement, expectedWrapperElement);
        }

        public static IEnumerable<object[]> CreateFlexiSectionBlockOptions_UsesFlexiSectionBlocksExtensionOptionsWrapperElementsIfNecessary_Data()
        {
            return new object[][]
            {
                new object[]{1, SectioningContentElement.None},
                new object[]{2, SectioningContentElement.Section}
            };
        }

        [Theory]
        [MemberData(nameof(FlexiSectionBlockOnClosed_SetsUpIdentifierGenerationAndAutoLinkingAccordingToOptions_Data))]
        public void FlexiSectionBlockOnClosed_SetsUpIdentifierGenerationAndAutoLinkingAccordingToOptions(bool dummyGenerateIdentifier,
            bool dummyAutoLink,
            bool setupIdentifierGenerationInvoked,
            bool setupAutoLinkInvoked)
        {
            // Arrange
            var dummyHeadingBlock = new HeadingBlock(null);
            var dummyFlexiSectionBlockOptions = new FlexiSectionBlockOptions() { GenerateIdentifier = dummyGenerateIdentifier, AutoLinkable = dummyAutoLink };
            var dummyFlexiSectionBlock = new FlexiSectionBlock(null)
            {
                FlexiSectionBlockOptions = dummyFlexiSectionBlockOptions
            };
            dummyFlexiSectionBlock.Add(dummyHeadingBlock);
            Mock<IdentifierService> mockIdentifierService = _mockRepository.Create<IdentifierService>();
            Mock<AutoLinkService> mockAutoLinkService = _mockRepository.Create<AutoLinkService>();
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            FlexiSectionBlockParser flexiSectionBlockParser = CreateFlexiSectionBlockParser(autoLinkService: mockAutoLinkService.Object, identifierService: mockIdentifierService.Object);

            // Act
            flexiSectionBlockParser.FlexiSectionBlockOnClosed(dummyBlockProcessor, dummyFlexiSectionBlock);

            // Assert
            mockIdentifierService.Verify(i => i.SetupIdentifierGeneration(dummyHeadingBlock), setupIdentifierGenerationInvoked ? Times.Once() : Times.Never());
            mockAutoLinkService.Verify(a => a.SetupAutoLink(dummyBlockProcessor, dummyFlexiSectionBlock, dummyHeadingBlock), setupAutoLinkInvoked ? Times.Once() : Times.Never());
        }

        public static IEnumerable<object[]> FlexiSectionBlockOnClosed_SetsUpIdentifierGenerationAndAutoLinkingAccordingToOptions_Data()
        {
            return new object[][]
            {
                new object[] {false, false, false, false},
                new object[] {true, false, true, false},
                // Can't do auto linking if identifier generation is disabled
                new object[] {false, true, false, false},
                new object[] {true, true, true, true}
            };
        }

        [Fact]
        public void FlexiSectionBlockOnClosed_ThrowsExceptionIfGenerateIdentifierIsTrueButTheFlexiSectionBlockHasNoChildHeadingBlock()
        {
            // Arrange
            var dummyFlexiSectionBlockOptions = new FlexiSectionBlockOptions() { GenerateIdentifier = true };
            var dummyFlexiSectionBlock = new FlexiSectionBlock(null)
            {
                FlexiSectionBlockOptions = dummyFlexiSectionBlockOptions
            };
            Mock<IdentifierService> mockIdentifierService = _mockRepository.Create<IdentifierService>();
            Mock<AutoLinkService> mockAutoLinkService = _mockRepository.Create<AutoLinkService>();
            FlexiSectionBlockParser flexiSectionBlockParser = CreateFlexiSectionBlockParser(autoLinkService: mockAutoLinkService.Object, identifierService: mockIdentifierService.Object);

            // Act and assert
            Assert.Throws<InvalidOperationException>(() => flexiSectionBlockParser.FlexiSectionBlockOnClosed(null, dummyFlexiSectionBlock));
        }

        private FlexiSectionBlockParser CreateFlexiSectionBlockParser(FlexiSectionBlocksExtensionOptions sectionsExtensionOptions = null,
            HeadingBlockParser headingBlockParser = null,
            AutoLinkService autoLinkService = null,
            IdentifierService identifierService = null,
            FlexiOptionsBlockService flexiOptionsBlockService = null)
        {
            return new FlexiSectionBlockParser(
                sectionsExtensionOptions ?? new FlexiSectionBlocksExtensionOptions(),
                headingBlockParser ?? new HeadingBlockParser(),
                autoLinkService ?? new AutoLinkService(),
                identifierService ?? new IdentifierService(),
                flexiOptionsBlockService ?? new FlexiOptionsBlockService());
        }

        private Mock<FlexiSectionBlockParser> CreateMockFlexiSectionBlockParser(FlexiSectionBlocksExtensionOptions sectionsExtensionOptions = null,
            HeadingBlockParser headingBlockParser = null,
            AutoLinkService autoLinkService = null,
            IdentifierService identifierService = null,
            FlexiOptionsBlockService flexiOptionsBlockService = null)
        {
            return _mockRepository.Create<FlexiSectionBlockParser>(
                sectionsExtensionOptions ?? new FlexiSectionBlocksExtensionOptions(),
                headingBlockParser ?? new HeadingBlockParser(),
                autoLinkService ?? new AutoLinkService(),
                identifierService ?? new IdentifierService(),
                flexiOptionsBlockService ?? new FlexiOptionsBlockService());
        }
    }
}
