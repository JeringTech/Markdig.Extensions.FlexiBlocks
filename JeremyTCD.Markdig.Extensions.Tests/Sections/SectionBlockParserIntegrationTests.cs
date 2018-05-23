using JeremyTCD.Markdig.Extensions.JsonOptions;
using JeremyTCD.Markdig.Extensions.Sections;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace JeremyTCD.Markdig.Extensions.Tests.Sections
{
    public class SectionBlockParserIntegrationTests
    {
        private MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void TryOpen_ReturnsBlockStateNoneIfCurrentLineIsNotAHeadingBlock()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<HeadingBlockParser> mockHeadingBlockParser = _mockRepository.Create<HeadingBlockParser>();
            mockHeadingBlockParser.Setup(h => h.TryOpen(dummyBlockProcessor)).Returns(BlockState.None);
            SectionBlockParser sectionBlockParser = CreateSectionBlockParser(headingBlockParser: mockHeadingBlockParser.Object);

            // Act
            BlockState result = sectionBlockParser.TryOpen(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.None, result);
        }



        [Theory]
        [MemberData(nameof(TryOpen_ReturnsBlockStateBreakIfSectioningContentElementIsNoneOrUndefined_Data))]
        public void TryOpen_ReturnsBlockStateBreakIfSectioningContentElementIsNoneOrUndefined(SectionBlockOptions dummySectionBlockOptions)
        {
            // Arrange
            const int dummyHeadingLevel = 1;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<HeadingBlockParser> mockHeadingBlockParser = _mockRepository.Create<HeadingBlockParser>();
            mockHeadingBlockParser.Setup(h => h.TryOpen(dummyBlockProcessor)).Returns(BlockState.Break);
            var dummyHeadingBlock = new HeadingBlock(null) { Level = dummyHeadingLevel };
            dummyBlockProcessor.NewBlocks.Push(dummyHeadingBlock);
            Mock<SectionBlockParser> mockSectionBlockParser = CreateMockSectionBlockParser(headingBlockParser: mockHeadingBlockParser.Object);
            mockSectionBlockParser.CallBase = true;
            mockSectionBlockParser.Setup(s => s.CreateSectionOptions(dummyBlockProcessor, dummyHeadingLevel)).Returns(dummySectionBlockOptions);

            // Act
            BlockState result = mockSectionBlockParser.Object.TryOpen(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.Break, result);
        }

        public static IEnumerable<object[]> TryOpen_ReturnsBlockStateBreakIfSectioningContentElementIsNoneOrUndefined_Data()
        {
            return new object[][]
            {
                new object[]{ new SectionBlockOptions() { WrapperElement = SectioningContentElement.None } },
                new object[]{ new SectionBlockOptions() { WrapperElement = SectioningContentElement.Undefined } }
            };
        }

        [Fact]
        public void TryOpen_IfSuccessfulCreatesNewSectionBlockAndReturnsBlockStateContinue()
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
            Mock<JsonOptionsService> mockJsonOptionsService = _mockRepository.Create<JsonOptionsService>();
            mockJsonOptionsService.Setup(j => j.TryPopulateOptions(dummyBlockProcessor, It.IsAny<SectionBlockOptions>())); // A clone of dummySectionBlockOptions is passed to TryPopulateOptions
            SectionBlockParser sectionBlockParser = CreateSectionBlockParser(headingBlockParser: mockHeadingBlockParser.Object, jsonOptionsService: mockJsonOptionsService.Object);

            // Act
            BlockState result = sectionBlockParser.TryOpen(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.Continue, result);
            var resultSectionBlock = dummyBlockProcessor.NewBlocks.Peek() as SectionBlock;
            Assert.NotNull(resultSectionBlock);
            Assert.Equal(dummyLevel, resultSectionBlock.Level);
            Assert.Equal(dummyColumn, resultSectionBlock.Column);
            Assert.Equal(dummySourceSpan, resultSectionBlock.Span); // SourceSpan is a struct, so object.Equals tests for value equality (not reference equality)
        }

        [Fact]
        public void TryContinue_ReturnsBlockStateContinueIfTheCurrentCharIsNotTheOpeningCharOfAHeadingBlock()
        {
            // Arrange
            var dummyStringSlice = new StringSlice("@"); // Any first character but #
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyStringSlice;
            var dummyHeadingBlockParser = new HeadingBlockParser();
            SectionBlockParser sectionBlockParser = CreateSectionBlockParser(headingBlockParser: dummyHeadingBlockParser);

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
            SectionBlockParser sectionBlockParser = CreateSectionBlockParser(headingBlockParser: mockHeadingBlockParser.Object);

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
            var dummySectionBlock = new SectionBlock(null)
            {
                Level = dummySectionBlockLevel
            };
            SectionBlockParser sectionBlockParser = CreateSectionBlockParser(headingBlockParser: mockHeadingBlockParser.Object);

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
            var dummySectionBlock = new SectionBlock(null)
            {
                Level = dummySectionBlockLevel
            };
            SectionBlockParser sectionBlockParser = CreateSectionBlockParser(headingBlockParser: mockHeadingBlockParser.Object);

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
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var mockJsonOptionsService = new Mock<JsonOptionsService>();
            mockJsonOptionsService.Setup(j => j.TryPopulateOptions(dummyBlockProcessor, It.IsAny<SectionBlockOptions>())); // SectionBlockOptions will be a fresh instance (cloned)
            SectionBlockParser sectionBlockParser = CreateSectionBlockParser();

            // Act
            SectionBlockOptions result = sectionBlockParser.CreateSectionOptions(dummyBlockProcessor, dummyLevel);

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
            var dummySectionBlockOptions = new SectionBlockOptions() { GenerateIdentifier = dummyGenerateIdentifier, AutoLinkable = dummyAutoLink };
            var dummySectionBlock = new SectionBlock(null)
            {
                SectionBlockOptions = dummySectionBlockOptions
            };
            dummySectionBlock.Add(dummyHeadingBlock);
            Mock<IdentifierService> mockIdentifierService = _mockRepository.Create<IdentifierService>();
            Mock<AutoLinkService> mockAutoLinkService = _mockRepository.Create<AutoLinkService>();
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            SectionBlockParser sectionBlockParser = CreateSectionBlockParser(autoLinkService: mockAutoLinkService.Object, identifierService: mockIdentifierService.Object);

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
            var dummySectionBlockOptions = new SectionBlockOptions() { GenerateIdentifier = true };
            var dummySectionBlock = new SectionBlock(null)
            {
                SectionBlockOptions = dummySectionBlockOptions
            };
            Mock<IdentifierService> mockIdentifierService = _mockRepository.Create<IdentifierService>();
            Mock<AutoLinkService> mockAutoLinkService = _mockRepository.Create<AutoLinkService>();
            SectionBlockParser sectionBlockParser = CreateSectionBlockParser(autoLinkService: mockAutoLinkService.Object, identifierService: mockIdentifierService.Object);

            // Act and assert
            Assert.Throws<InvalidOperationException>(() => sectionBlockParser.SectionBlockOnClosed(null, dummySectionBlock));
        }

        private SectionBlockParser CreateSectionBlockParser(SectionsExtensionOptions sectionsExtensionOptions = null,
            HeadingBlockParser headingBlockParser = null,
            AutoLinkService autoLinkService = null,
            IdentifierService identifierService = null,
            JsonOptionsService jsonOptionsService = null)
        {
            return new SectionBlockParser(
                sectionsExtensionOptions ?? new SectionsExtensionOptions(),
                headingBlockParser ?? new HeadingBlockParser(),
                autoLinkService ?? new AutoLinkService(),
                identifierService ?? new IdentifierService(),
                jsonOptionsService ?? new JsonOptionsService());
        }

        private Mock<SectionBlockParser> CreateMockSectionBlockParser(SectionsExtensionOptions sectionsExtensionOptions = null,
            HeadingBlockParser headingBlockParser = null,
            AutoLinkService autoLinkService = null,
            IdentifierService identifierService = null,
            JsonOptionsService jsonOptionsService = null)
        {
            return _mockRepository.Create<SectionBlockParser>(
                sectionsExtensionOptions ?? new SectionsExtensionOptions(),
                headingBlockParser ?? new HeadingBlockParser(),
                autoLinkService ?? new AutoLinkService(),
                identifierService ?? new IdentifierService(),
                jsonOptionsService ?? new JsonOptionsService());
        }
    }
}
