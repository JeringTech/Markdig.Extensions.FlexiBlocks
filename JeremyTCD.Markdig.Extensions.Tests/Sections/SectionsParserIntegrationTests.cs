using JeremyTCD.Markdig.Extensions.JsonOptions;
using JeremyTCD.Markdig.Extensions.Sections;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace JeremyTCD.Markdig.Extensions.Tests.Sections
{
    public class SectionsParserIntegrationTests
    {
        private MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void TryOpen_ReturnsBlockStateNoneIfCurrentLineIsNotAHeadingBlock()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<HeadingBlockParser> mockHeadingBlockParser = _mockRepository.Create<HeadingBlockParser>();
            mockHeadingBlockParser.Setup(h => h.TryOpen(dummyBlockProcessor)).Returns(BlockState.None);
            SectionsParser sectionsParser = CreateSectionsParser(headingBlockParser: mockHeadingBlockParser.Object);

            // Act
            BlockState result = sectionsParser.TryOpen(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.None, result);
        }



        [Theory]
        [MemberData(nameof(TryOpen_ReturnsBlockStateBreakIfSectioningContentElementIsNoneOrUndefined_Data))]
        public void TryOpen_ReturnsBlockStateBreakIfSectioningContentElementIsNoneOrUndefined(SectionBlockOptions dummySectionBlockOptions)
        {
            // Arrange
            int dummyHeadingLevel = 1;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<HeadingBlockParser> mockHeadingBlockParser = _mockRepository.Create<HeadingBlockParser>();
            mockHeadingBlockParser.Setup(h => h.TryOpen(dummyBlockProcessor)).Returns(BlockState.Break);
            var dummyHeadingBlock = new HeadingBlock(null) { Level = dummyHeadingLevel };
            dummyBlockProcessor.NewBlocks.Push(dummyHeadingBlock);
            Mock<SectionsParser> mockSectionsParser = CreateMockSectionsParser(headingBlockParser: mockHeadingBlockParser.Object);
            mockSectionsParser.CallBase = true;
            mockSectionsParser.Setup(s => s.CreateSectionOptions(dummyBlockProcessor, dummyHeadingLevel)).Returns(dummySectionBlockOptions);

            // Act
            BlockState result = mockSectionsParser.Object.TryOpen(dummyBlockProcessor);

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
            SectionsParser sectionsParser = CreateSectionsParser(headingBlockParser: mockHeadingBlockParser.Object, jsonOptionsService: mockJsonOptionsService.Object);

            // Act
            BlockState result = sectionsParser.TryOpen(dummyBlockProcessor);

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
            SectionsParser sectionsParser = CreateSectionsParser(headingBlockParser: dummyHeadingBlockParser);

            // Act
            BlockState result = sectionsParser.TryContinue(dummyBlockProcessor, null);

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
            SectionsParser sectionsParser = CreateSectionsParser(headingBlockParser: mockHeadingBlockParser.Object);

            // Act
            BlockState result = sectionsParser.TryContinue(dummyBlockProcessor, null);

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
            SectionsParser sectionsParser = CreateSectionsParser(headingBlockParser: mockHeadingBlockParser.Object);

            // Act
            BlockState result = sectionsParser.TryContinue(dummyBlockProcessor, dummySectionBlock);

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
            SectionsParser sectionsParser = CreateSectionsParser(headingBlockParser: mockHeadingBlockParser.Object);

            // Act
            BlockState result = sectionsParser.TryContinue(dummyBlockProcessor, dummySectionBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.Continue, result);
            Assert.Empty(dummyBlockProcessor.NewBlocks);
        }

        // TODO CreateSectionOptions

        // TODO SectionBlockOnClosed

        private SectionsParser CreateSectionsParser(SectionsOptions sectionsOptions = null,
            HeadingBlockParser headingBlockParser = null,
            AutoLinkService autoLinkService = null,
            IdentifierService identifierService = null,
            JsonOptionsService jsonOptionsService = null)
        {
            return new SectionsParser(
                sectionsOptions ?? new SectionsOptions(),
                headingBlockParser ?? new HeadingBlockParser(),
                autoLinkService ?? new AutoLinkService(),
                identifierService ?? new IdentifierService(),
                jsonOptionsService ?? new JsonOptionsService());
        }

        private Mock<SectionsParser> CreateMockSectionsParser(SectionsOptions sectionsOptions = null,
            HeadingBlockParser headingBlockParser = null,
            AutoLinkService autoLinkService = null,
            IdentifierService identifierService = null,
            JsonOptionsService jsonOptionsService = null)
        {
            return _mockRepository.Create<SectionsParser>(
                sectionsOptions ?? new SectionsOptions(),
                headingBlockParser ?? new HeadingBlockParser(),
                autoLinkService ?? new AutoLinkService(),
                identifierService ?? new IdentifierService(),
                jsonOptionsService ?? new JsonOptionsService());
        }
    }
}
