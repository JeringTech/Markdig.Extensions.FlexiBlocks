using FlexiBlocks.JsonOptions;
using FlexiBlocks.Sections;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace FlexiBlocks.Tests.Sections
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
            FlexiSectionBlockParser sectionBlockParser = CreateSectionBlockParser(headingBlockParser: mockHeadingBlockParser.Object);

            // Act
            BlockState result = sectionBlockParser.TryOpen(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.None, result);
        }

        [Theory]
        [MemberData(nameof(TryOpen_SetsHeaderDataAndReturnsBlockStateBreakIfSectioningContentElementIsNoneOrUndefined_Data))]
        public void TryOpen_SetsHeaderDataAndReturnsBlockStateBreakIfSectioningContentElementIsNoneOrUndefined(FlexiSectionBlockOptions dummySectionBlockOptions)
        {
            // Arrange
            const int dummyHeadingLevel = 1;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<HeadingBlockParser> mockHeadingBlockParser = _mockRepository.Create<HeadingBlockParser>();
            mockHeadingBlockParser.Setup(h => h.TryOpen(dummyBlockProcessor)).Returns(BlockState.Break);
            var dummyHeadingBlock = new HeadingBlock(null) { Level = dummyHeadingLevel };
            dummyBlockProcessor.NewBlocks.Push(dummyHeadingBlock);
            Mock<FlexiSectionBlockParser> mockSectionBlockParser = CreateMockSectionBlockParser(headingBlockParser: mockHeadingBlockParser.Object);
            mockSectionBlockParser.CallBase = true;
            mockSectionBlockParser.Setup(s => s.CreateSectionOptions(dummyBlockProcessor, dummyHeadingLevel)).Returns(dummySectionBlockOptions);

            // Act
            BlockState result = mockSectionBlockParser.Object.TryOpen(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.Break, result);
            Assert.Equal(dummySectionBlockOptions.HeaderIconMarkup, dummyHeadingBlock.GetData(FlexiSectionBlockParser.HEADER_ICON_MARKUP_KEY));
            Assert.Equal(dummySectionBlockOptions.HeaderClassNameFormat, dummyHeadingBlock.GetData(FlexiSectionBlockParser.HEADER_CLASS_NAME_FORMAT_KEY));
        }

        public static IEnumerable<object[]> TryOpen_SetsHeaderDataAndReturnsBlockStateBreakIfSectioningContentElementIsNoneOrUndefined_Data()
        {
            return new object[][]
            {
                new object[]{ new FlexiSectionBlockOptions() { WrapperElement = SectioningContentElement.None} },
                new object[]{ new FlexiSectionBlockOptions() { WrapperElement = SectioningContentElement.Undefined } }
            };
        }

        [Fact]
        public void TryOpen_IfSuccessfulSetsHeaderDataCreatesNewSectionBlockAndReturnsBlockStateContinue()
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
            var dummySectionBlockOptions = new FlexiSectionBlockOptions()
            {
                WrapperElement = SectioningContentElement.Section // Anything other than undefined and none to avoid break
            };
            Mock<FlexiSectionBlockParser> sectionBlockParser = CreateMockSectionBlockParser(headingBlockParser: mockHeadingBlockParser.Object);
            sectionBlockParser.CallBase = true;
            sectionBlockParser.Setup(s => s.CreateSectionOptions(dummyBlockProcessor, dummyLevel)).Returns(dummySectionBlockOptions);

            // Act
            BlockState result = sectionBlockParser.Object.TryOpen(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.Continue, result);
            var resultSectionBlock = dummyBlockProcessor.NewBlocks.Peek() as FlexiSectionBlock;
            Assert.NotNull(resultSectionBlock);
            Assert.Equal(dummyLevel, resultSectionBlock.Level);
            Assert.Equal(dummyColumn, resultSectionBlock.Column);
            Assert.Equal(dummySourceSpan, resultSectionBlock.Span); // SourceSpan is a struct, so object.Equals tests for value equality (not reference equality)
            Assert.Same(dummySectionBlockOptions, resultSectionBlock.SectionBlockOptions);
            Assert.Equal(dummySectionBlockOptions.HeaderIconMarkup, dummyHeadingBlock.GetData(FlexiSectionBlockParser.HEADER_ICON_MARKUP_KEY));
            Assert.Equal(dummySectionBlockOptions.HeaderClassNameFormat, dummyHeadingBlock.GetData(FlexiSectionBlockParser.HEADER_CLASS_NAME_FORMAT_KEY));
        }

        [Fact]
        public void TryContinue_ReturnsBlockStateContinueIfTheCurrentCharIsNotTheOpeningCharOfAHeadingBlock()
        {
            // Arrange
            var dummyStringSlice = new StringSlice("@"); // Any first character but #
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyStringSlice;
            var dummyHeadingBlockParser = new HeadingBlockParser();
            FlexiSectionBlockParser sectionBlockParser = CreateSectionBlockParser(headingBlockParser: dummyHeadingBlockParser);

            // Act
            BlockState result = sectionBlockParser.TryContinue(dummyBlockProcessor, null);

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
            FlexiSectionBlockParser sectionBlockParser = CreateSectionBlockParser(headingBlockParser: mockHeadingBlockParser.Object);

            // Act
            BlockState result = sectionBlockParser.TryContinue(dummyBlockProcessor, null);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.Continue, result);
        }

        [Theory]
        [MemberData(nameof(TryContinue_ReturnsBlockStateNoneAndRemovesNewHeadingBlockIfNewSectionIsAChildOfTheCurrentSection_Data))]
        public void TryContinue_ReturnsBlockStateNoneAndRemovesNewHeadingBlockIfNewSectionIsAChildOfTheCurrentSection(int dummyHeadingBlockLevel, int dummySectionBlockLevel)
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
            var dummySectionBlock = new FlexiSectionBlock(null)
            {
                Level = dummySectionBlockLevel
            };
            FlexiSectionBlockParser sectionBlockParser = CreateSectionBlockParser(headingBlockParser: mockHeadingBlockParser.Object);

            // Act
            BlockState result = sectionBlockParser.TryContinue(dummyBlockProcessor, dummySectionBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.None, result);
            Assert.Empty(dummyBlockProcessor.NewBlocks);
        }

        public static IEnumerable<object[]> TryContinue_ReturnsBlockStateNoneAndRemovesNewHeadingBlockIfNewSectionIsAChildOfTheCurrentSection_Data()
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
            const int dummySectionBlockLevel = 1;
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
            var dummySectionBlock = new FlexiSectionBlock(null)
            {
                Level = dummySectionBlockLevel
            };
            FlexiSectionBlockParser sectionBlockParser = CreateSectionBlockParser(headingBlockParser: mockHeadingBlockParser.Object);

            // Act
            BlockState result = sectionBlockParser.TryContinue(dummyBlockProcessor, dummySectionBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.Continue, result);
            Assert.Empty(dummyBlockProcessor.NewBlocks);
        }

        [Theory]
        [MemberData(nameof(CreateSectionOptions_CreatesSectionOptionsUsingSectionsExtensionOptionsWrapperElementsIfWrapperElementIsUndefined_Data))]
        public void CreateSectionOptions_CreatesSectionOptionsUsingSectionsExtensionOptionsWrapperElementsIfSectionBlockOptionsWrapperElementIsUndefined(int dummyLevel, SectioningContentElement expectedWrapperElement)
        {
            // Arrange
            const int dummyLineIndex = 1;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.LineIndex = dummyLineIndex;
            var mockJsonOptionsService = new Mock<FlexiOptionsService>();
            mockJsonOptionsService.Setup(j => j.TryPopulateOptions(dummyBlockProcessor, It.IsAny<FlexiSectionBlockOptions>(), dummyLineIndex)); // SectionBlockOptions will be a fresh instance (cloned)
            FlexiSectionBlockParser sectionBlockParser = CreateSectionBlockParser();

            // Act
            FlexiSectionBlockOptions result = sectionBlockParser.CreateSectionOptions(dummyBlockProcessor, dummyLevel);

            // Assert
            _mockRepository.VerifyAll();
            Assert.NotNull(result);
            Assert.Equal(result.WrapperElement, expectedWrapperElement);
        }

        public static IEnumerable<object[]> CreateSectionOptions_CreatesSectionOptionsUsingSectionsExtensionOptionsWrapperElementsIfWrapperElementIsUndefined_Data()
        {
            return new object[][]
            {
                new object[]{1, SectioningContentElement.None},
                new object[]{2, SectioningContentElement.Section}
            };
        }

        [Theory]
        [MemberData(nameof(SectionBlockOnClosed_SetsUpIdentifierGenerationAndAutoLinkingAccordingToOptions_Data))]
        public void SectionBlockOnClosed_SetsUpIdentifierGenerationAndAutoLinkingAccordingToOptions(bool dummyGenerateIdentifier,
            bool dummyAutoLink,
            Times expectedSetupIdentifierInvocations,
            Times expectedSetupAutoLinkInvocations)
        {
            // Arrange
            var dummyHeadingBlock = new HeadingBlock(null);
            var dummySectionBlockOptions = new FlexiSectionBlockOptions() { GenerateIdentifier = dummyGenerateIdentifier, AutoLinkable = dummyAutoLink };
            var dummySectionBlock = new FlexiSectionBlock(null)
            {
                SectionBlockOptions = dummySectionBlockOptions
            };
            dummySectionBlock.Add(dummyHeadingBlock);
            Mock<IdentifierService> mockIdentifierService = _mockRepository.Create<IdentifierService>();
            Mock<AutoLinkService> mockAutoLinkService = _mockRepository.Create<AutoLinkService>();
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            FlexiSectionBlockParser sectionBlockParser = CreateSectionBlockParser(autoLinkService: mockAutoLinkService.Object, identifierService: mockIdentifierService.Object);

            // Act
            sectionBlockParser.SectionBlockOnClosed(dummyBlockProcessor, dummySectionBlock);

            // Assert
            mockIdentifierService.Verify(i => i.SetupIdentifierGeneration(dummyHeadingBlock), expectedSetupIdentifierInvocations);
            mockAutoLinkService.Verify(a => a.SetupAutoLink(dummyBlockProcessor, dummySectionBlock, dummyHeadingBlock), expectedSetupAutoLinkInvocations);
        }

        public static IEnumerable<object[]> SectionBlockOnClosed_SetsUpIdentifierGenerationAndAutoLinkingAccordingToOptions_Data()
        {
            return new object[][]
            {
                new object[] {false, false, Times.Never(), Times.Never()},
                new object[] {true, false, Times.Once(), Times.Never()},
                new object[] {false, true, Times.Never(), Times.Never()},
                new object[] {true, true, Times.Once(), Times.Once()}
            };
        }

        [Fact]
        public void SectionBlockOnClosed_ThrowsExceptionIfGenerateIdentifierIsTrueButTheSectionBlockHasNoChildHeadingBlock()
        {
            // Arrange
            var dummySectionBlockOptions = new FlexiSectionBlockOptions() { GenerateIdentifier = true };
            var dummySectionBlock = new FlexiSectionBlock(null)
            {
                SectionBlockOptions = dummySectionBlockOptions
            };
            Mock<IdentifierService> mockIdentifierService = _mockRepository.Create<IdentifierService>();
            Mock<AutoLinkService> mockAutoLinkService = _mockRepository.Create<AutoLinkService>();
            FlexiSectionBlockParser sectionBlockParser = CreateSectionBlockParser(autoLinkService: mockAutoLinkService.Object, identifierService: mockIdentifierService.Object);

            // Act and assert
            Assert.Throws<InvalidOperationException>(() => sectionBlockParser.SectionBlockOnClosed(null, dummySectionBlock));
        }

        private FlexiSectionBlockParser CreateSectionBlockParser(FlexiSectionsExtensionOptions sectionsExtensionOptions = null,
            HeadingBlockParser headingBlockParser = null,
            AutoLinkService autoLinkService = null,
            IdentifierService identifierService = null,
            FlexiOptionsService jsonOptionsService = null)
        {
            return new FlexiSectionBlockParser(
                sectionsExtensionOptions ?? new FlexiSectionsExtensionOptions(),
                headingBlockParser ?? new HeadingBlockParser(),
                autoLinkService ?? new AutoLinkService(),
                identifierService ?? new IdentifierService(),
                jsonOptionsService ?? new FlexiOptionsService());
        }

        private Mock<FlexiSectionBlockParser> CreateMockSectionBlockParser(FlexiSectionsExtensionOptions sectionsExtensionOptions = null,
            HeadingBlockParser headingBlockParser = null,
            AutoLinkService autoLinkService = null,
            IdentifierService identifierService = null,
            FlexiOptionsService jsonOptionsService = null)
        {
            return _mockRepository.Create<FlexiSectionBlockParser>(
                sectionsExtensionOptions ?? new FlexiSectionsExtensionOptions(),
                headingBlockParser ?? new HeadingBlockParser(),
                autoLinkService ?? new AutoLinkService(),
                identifierService ?? new IdentifierService(),
                jsonOptionsService ?? new FlexiOptionsService());
        }
    }
}
