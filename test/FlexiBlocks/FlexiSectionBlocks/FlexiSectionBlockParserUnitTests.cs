using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiSectionBlocks
{
    public class FlexiSectionBlockParserUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiOptionsBlockServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiSectionBlockParser(null, new FlexiSectionBlocksExtensionOptions()));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfExtensionOptionsIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiSectionBlockParser(
                _mockRepository.Create<IFlexiOptionsBlockService>().Object,
                null));
        }

        [Fact]
        public void TryOpenFlexiBlock_ReturnsBlockStateNoneIfCurrentLineHasCodeIndent()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = 4; // IsCodeIndent is an expression bodied member that derives its value from Column
            ExposedFlexiSectionBlockParser testSubject = CreateExposedFlexiSectionBlockParser();

            // Act
            BlockState result = testSubject.ExposedTryOpenFlexiBlock(dummyBlockProcessor);

            // Assert
            Assert.Equal(BlockState.None, result);
        }

        [Fact]
        public void TryOpenFlexiBlock_ReturnsBlockStateNoneIfAFlexiSectionBlockCantBeCreated()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<ExposedFlexiSectionBlockParser> mockTestSubject = CreateMockExposedFlexiSectionBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.TryCreateFlexiSectionBlock(dummyBlockProcessor)).Returns((FlexiSectionBlock)null);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryOpenFlexiBlock(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.None, result);
        }

        [Fact]
        public void TryOpenFlexiBlock_ReturnsBlockStateContinueDiscardAndAddsNewFlexiSectionBlockToNewBlocksIfSuccessful()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyFlexiSectionBlock = new FlexiSectionBlock(null);
            Mock<ExposedFlexiSectionBlockParser> mockTestSubject = CreateMockExposedFlexiSectionBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.TryCreateFlexiSectionBlock(dummyBlockProcessor)).Returns(dummyFlexiSectionBlock);
            mockTestSubject.Setup(t => t.SetupFlexiSectionBlock(dummyBlockProcessor, dummyFlexiSectionBlock));
            mockTestSubject.Setup(t => t.UpdateOpenFlexiSectionBlocks(dummyBlockProcessor, dummyFlexiSectionBlock));

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryOpenFlexiBlock(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.ContinueDiscard, result);
            Assert.Single(dummyBlockProcessor.NewBlocks);
            Assert.Same(dummyFlexiSectionBlock, dummyBlockProcessor.NewBlocks.Peek());
        }

        [Theory]
        [MemberData(nameof(TryCreateFlexiSectionBlock_ReturnsNullIfAFlexiSectionBlockCannotBeOpened_Data))]
        public void TryCreateFlexiSectionBlock_ReturnsNullIfAFlexiSectionBlockCannotBeOpened(string dummyLineText)
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyLineText);
            ExposedFlexiSectionBlockParser testSubject = CreateExposedFlexiSectionBlockParser();

            // Act
            FlexiSectionBlock result = testSubject.TryCreateFlexiSectionBlock(dummyBlockProcessor);

            // Assert
            Assert.Null(result);
        }

        public static IEnumerable<object[]> TryCreateFlexiSectionBlock_ReturnsNullIfAFlexiSectionBlockCannotBeOpened_Data()
        {
            return new object[][]
            {
                // Too many hashes
                new object[]
                {
                    "####### Dummy"
                },
                // Character after hashes is not a space of the end of the line
                new object[]
                {
                    "###Dummy"
                },
            };
        }

        [Theory]
        [MemberData(nameof(TryCreateFlexiSectionBlock_ReturnsReturnsFlexiSectionBlockIfSuccessful_Data))]
        public void TryCreateFlexiSectionBlock_ReturnsReturnsFlexiSectionBlockIfSuccessful(string dummyLineText,
            int expectedLevel,
            int expectedProcessorColumn,
            int expectedLineStart)
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyLineText);
            ExposedFlexiSectionBlockParser testSubject = CreateExposedFlexiSectionBlockParser();

            // Act
            FlexiSectionBlock result = testSubject.TryCreateFlexiSectionBlock(dummyBlockProcessor);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedLevel, result.Level);
            Assert.Equal(0, result.Column);
            Assert.Equal(0, result.Span.Start);
            Assert.Equal(expectedProcessorColumn, dummyBlockProcessor.Column);
            Assert.Equal(expectedLineStart, dummyBlockProcessor.Line.Start);
        }

        public static IEnumerable<object[]> TryCreateFlexiSectionBlock_ReturnsReturnsFlexiSectionBlockIfSuccessful_Data()
        {
            return new object[][]
            {
                // Level 1
                new object[] { "# Dummy", 1, 2, 2 },
                // Level 2
                new object[] { "## Dummy", 2, 3, 3 },
                // Level 3
                new object[] { "### Dummy", 3, 4, 4 },
                // Level 4
                new object[] { "#### Dummy", 4, 5, 5 },
                // Level 5
                new object[] { "##### Dummy", 5, 6, 6 },
                // Level 6
                new object[] { "###### Dummy", 6, 7, 7 },
                // EoL after hashes
                new object[] { "####", 4, 4, 4 },
                // Multiple spaces after hashes
                new object[] { "#####  Dummy", 5, 6, 6 },
            };
        }

        [Fact]
        public void SetupFlexiSectionBlock_SetsUpFlexiSectionBlock()
        {
            // Arrange
            const string dummyLineText = "  dummy  ";
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyLineText);
            const int dummyLevel = 2;
            var dummyFlexiSectionBlock = new FlexiSectionBlock(null) { Level = dummyLevel };
            var dummyFlexiSectionBlockOptions = new FlexiSectionBlockOptions();
            Mock<ExposedFlexiSectionBlockParser> mockTestSubject = CreateMockExposedFlexiSectionBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.TrimClosingHashes(dummyBlockProcessor));
            mockTestSubject.Setup(t => t.CreateFlexiSectionBlockOptions(dummyBlockProcessor, dummyLevel)).Returns(dummyFlexiSectionBlockOptions);
            mockTestSubject.Setup(t => t.GenerateID(dummyBlockProcessor, dummyFlexiSectionBlock));
            mockTestSubject.Setup(t => t.SetupAutoLinking(dummyBlockProcessor, dummyFlexiSectionBlock));

            // Act
            mockTestSubject.Object.SetupFlexiSectionBlock(dummyBlockProcessor, dummyFlexiSectionBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyLineText.Length - 1, dummyFlexiSectionBlock.Span.End);
            Assert.Equal(dummyLineText.Trim(), dummyBlockProcessor.Line.ToString());
            Assert.Equal(dummyLineText.Trim(), dummyFlexiSectionBlock.HeaderContent);
            Assert.Same(dummyFlexiSectionBlockOptions, dummyFlexiSectionBlock.FlexiSectionBlockOptions);
        }

        [Theory]
        [MemberData(nameof(TrimClosingHashes_TrimsClosingHashes_Data))]
        public void TrimClosingHashes_TrimsClosingHashes(string dummyLineText, string expectedLine)
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyLineText);
            ExposedFlexiSectionBlockParser testSubject = CreateExposedFlexiSectionBlockParser();

            // Act
            testSubject.TrimClosingHashes(dummyBlockProcessor);

            // Assert
            Assert.Equal(expectedLine, dummyBlockProcessor.Line.ToString());
        }

        public static IEnumerable<object[]> TrimClosingHashes_TrimsClosingHashes_Data()
        {
            return new object[][]
            {
                // Typical end hashes
                new object[]
                {
                    "## Dummy ##",
                    "## Dummy"
                },
                // End hashes with trailing spaces
                new object[]
                {
                    "## Dummy ##  ",
                    "## Dummy"
                },
                // End hashes without leading space
                new object[]
                {
                    "## Dummy##  ",
                    "## Dummy##  "
                },
                // End hashes without multiple leading spaces
                new object[]
                {
                    "## Dummy  ##",
                    "## Dummy "
                }
            };
        }

        [Fact]
        public void CreateFlexiSectionBlockOptions_ThrowsFlexiBlocksExceptionIfClassFormatIsInvalid()
        {
            // Arrange
            const string dummyClassFormat = "dummy-{0}-{1}";
            var dummyExtensionOptions = new FlexiSectionBlocksExtensionOptions
            {
                DefaultBlockOptions = new FlexiSectionBlockOptions(classFormat: dummyClassFormat)
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<IFlexiOptionsBlockService> mockFlexiOptionsBlockService = _mockRepository.Create<IFlexiOptionsBlockService>();
            mockFlexiOptionsBlockService.Setup(f => f.TryPopulateOptions(dummyBlockProcessor, It.IsAny<FlexiSectionBlockOptions>(), 0));
            FlexiSectionBlockParser testSubject = CreateExposedFlexiSectionBlockParser(dummyExtensionOptions, mockFlexiOptionsBlockService.Object);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => testSubject.CreateFlexiSectionBlockOptions(dummyBlockProcessor, 0));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.FlexiBlocksException_Shared_OptionIsAnInvalidFormat, nameof(FlexiSectionBlockOptions.ClassFormat), dummyClassFormat),
                result.Message);
            Assert.IsType<FormatException>(result.InnerException);
        }

        [Theory]
        [MemberData(nameof(CreateFlexiSectionBlocksOptions_CreatesFlexiOptionsBlockOptionsAndPopulatesClassIfClassFormatIsNotNullWhitespaceOrAnEmptyString_Data))]
        public void CreateFlexiSectionBlocksOptions_CreatesFlexiOptionsBlockOptionsAndPopulatesClassIfClassFormatIsNotNullWhitespaceOrAnEmptyString(string dummyClassFormat,
            int dummyLevel,
            string expectedClass)
        {
            // Arrange
            var dummyExtensionOptions = new FlexiSectionBlocksExtensionOptions
            {
                DefaultBlockOptions = new FlexiSectionBlockOptions(classFormat: dummyClassFormat)
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<IFlexiOptionsBlockService> mockFlexiOptionsBlockService = _mockRepository.Create<IFlexiOptionsBlockService>();
            mockFlexiOptionsBlockService.Setup(f => f.TryPopulateOptions(dummyBlockProcessor, It.IsAny<FlexiSectionBlockOptions>(), 0));
            FlexiSectionBlockParser testSubject = CreateExposedFlexiSectionBlockParser(dummyExtensionOptions, mockFlexiOptionsBlockService.Object);

            // Act
            FlexiSectionBlockOptions result = testSubject.CreateFlexiSectionBlockOptions(dummyBlockProcessor, dummyLevel);

            // Assert
            _mockRepository.VerifyAll();
            Assert.NotNull(result);
            Assert.Equal(expectedClass, result.Class);
        }

        public static IEnumerable<object[]> CreateFlexiSectionBlocksOptions_CreatesFlexiOptionsBlockOptionsAndPopulatesClassIfClassFormatIsNotNullWhitespaceOrAnEmptyString_Data()
        {
            const string dummyClassFormat = "dummy-{0}";
            const int dummyLevel = 2;

            return new object[][]
            {
                new object[]{dummyClassFormat, dummyLevel, string.Format(dummyClassFormat, dummyLevel)},
                new object[]{null, dummyLevel, null},
                new object[]{" ", dummyLevel, null},
                new object[]{string.Empty, dummyLevel, null}
            };
        }

        [Fact]
        public void GenerateID_DoesNotGenerateIDIfGenerateIdentifierIsFalse()
        {
            // Arrange
            var dummyFlexiSectionBlock = new FlexiSectionBlock(null)
            {
                FlexiSectionBlockOptions = new FlexiSectionBlockOptions(generateIdentifier: false)
            };
            FlexiSectionBlockParser testSubject = CreateExposedFlexiSectionBlockParser();

            // Act
            testSubject.GenerateID(null, dummyFlexiSectionBlock);

            // Assert
            Assert.Null(dummyFlexiSectionBlock.ID);
        }

        [Fact]
        public void GenerateID_UsesExistingIDIfItExists()
        {
            // Arrange
            const string dummyID = "dummyID";
            var dummyFlexiSectionBlock = new FlexiSectionBlock(null)
            {
                FlexiSectionBlockOptions = new FlexiSectionBlockOptions(attributes: new Dictionary<string, string> { { "id", dummyID } })
            };
            FlexiSectionBlockParser testSubject = CreateExposedFlexiSectionBlockParser();

            // Act
            testSubject.GenerateID(null, dummyFlexiSectionBlock);

            // Assert
            Assert.Equal(dummyID, dummyFlexiSectionBlock.ID);
        }

        [Theory]
        [MemberData(nameof(GenerateID_GeneratesID_Data))]
        public void GenerateID_GeneratesID(string dummyHeaderContent,
            SerializableWrapper<Dictionary<string, int>> dummySectionIDsWrapper,
            string expectedID,
            SerializableWrapper<Dictionary<string, int>> expectedSectionIDsWrapper)
        {
            // Arrange
            var dummyFlexiSectionBlock = new FlexiSectionBlock(null)
            {
                FlexiSectionBlockOptions = new FlexiSectionBlockOptions(),
                HeaderContent = dummyHeaderContent
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<ExposedFlexiSectionBlockParser> mockTestSubject = CreateMockExposedFlexiSectionBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.GetOrCreateSectionIDs(dummyBlockProcessor)).Returns(dummySectionIDsWrapper.Value);

            // Act
            mockTestSubject.Object.GenerateID(dummyBlockProcessor, dummyFlexiSectionBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(expectedID, dummyFlexiSectionBlock.ID);
            Assert.Equal(expectedSectionIDsWrapper.Value, dummySectionIDsWrapper.Value);
        }

        public static IEnumerable<object[]> GenerateID_GeneratesID_Data()
        {
            return new object[][]
            {
                // Typical usage
                new object[]
                {
                    "Header Content",
                    new SerializableWrapper<Dictionary<string, int>>(new Dictionary<string, int>()),
                    "header-content",
                    new SerializableWrapper<Dictionary<string, int>>(new Dictionary<string, int>{{"header-content", 0}})
                },
                // ID already in use
                new object[]
                {
                    "Header Content",
                    new SerializableWrapper<Dictionary<string, int>>(new Dictionary<string, int>{{"header-content", 1 }}),
                    "header-content-2",
                    new SerializableWrapper<Dictionary<string, int>>(new Dictionary<string, int>{{ "header-content", 2 }})
                },
                // Header content is an empty string
                new object[]
                {
                    string.Empty,
                    new SerializableWrapper<Dictionary<string, int>>(new Dictionary<string, int>()),
                    "section",
                    new SerializableWrapper<Dictionary<string, int>>(new Dictionary<string, int>{{"section", 0}})
                },
                // Header content is whitespace
                new object[]
                {
                    " ",
                    new SerializableWrapper<Dictionary<string, int>>(new Dictionary<string, int>()),
                    "section",
                    new SerializableWrapper<Dictionary<string, int>>(new Dictionary<string, int>{{"section", 0}})
                }
            };
        }

        [Fact]
        public void GetOrCreateSectionIDs_GetsSectionIDsIfItAlreadyExists()
        {
            // Arrange
            var dummySectionIDs = new Dictionary<string, int>();
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Document.SetData(FlexiSectionBlockParser.SECTION_IDS_KEY, dummySectionIDs);
            FlexiSectionBlockParser testSubject = CreateExposedFlexiSectionBlockParser();

            // Act
            Dictionary<string, int> result = testSubject.GetOrCreateSectionIDs(dummyBlockProcessor);

            // Assert
            Assert.Same(dummySectionIDs, result);
        }

        [Fact]
        public void GetOrCreateSectionIDs_CreatesSectionIDsIfItDoesNotAlreadyExist()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            FlexiSectionBlockParser testSubject = CreateExposedFlexiSectionBlockParser();

            // Act
            Dictionary<string, int> result = testSubject.GetOrCreateSectionIDs(dummyBlockProcessor);

            // Assert
            Assert.NotNull(result);
        }

        [Theory]
        [MemberData(nameof(SetupAutoLinking_DoesNothingIfIDIsNullOrAutoLinkableIsFalse_Data))]
        public void SetupAutoLinking_DoesNothingIfIDIsNullOrAutoLinkableIsFalse(string dummyID, bool dummyAutoLinkable)
        {
            // Arrange
            var dummyFlexiSectionBlock = new FlexiSectionBlock(null)
            {
                ID = dummyID,
                FlexiSectionBlockOptions = new FlexiSectionBlockOptions(autoLinkable: dummyAutoLinkable)
            };
            FlexiSectionBlockParser testSubject = CreateExposedFlexiSectionBlockParser();

            // Act and assert
            testSubject.SetupAutoLinking(null, dummyFlexiSectionBlock); // As long as this doesn't throw, auto linking has not been setup
        }

        public static IEnumerable<object[]> SetupAutoLinking_DoesNothingIfIDIsNullOrAutoLinkableIsFalse_Data()
        {
            return new object[][]
            {
                new object[]{null, true},
                new object[]{"dummy-id", false},
                new object[]{null, false}
            };
        }

        [Fact]
        public void SetupAutoLinking_SetsUpAutoLinking()
        {
            // Arrange
            const string dummyHeaderContent = "dummyHeaderContent";
            var dummyFlexiSectionBlock = new FlexiSectionBlock(null)
            {
                HeaderContent = dummyHeaderContent,
                ID = "dummy-id",
                FlexiSectionBlockOptions = new FlexiSectionBlockOptions(autoLinkable: true)
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummySectionLinkReferenceDefinitions = new Dictionary<string, SectionLinkReferenceDefinition>();
            Mock<ExposedFlexiSectionBlockParser> mockTestSubject = CreateMockExposedFlexiSectionBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.GetOrCreateSectionLinkReferenceDefinitions(dummyBlockProcessor)).Returns(dummySectionLinkReferenceDefinitions);

            // Act
            mockTestSubject.Object.SetupAutoLinking(dummyBlockProcessor, dummyFlexiSectionBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.True(dummySectionLinkReferenceDefinitions.TryGetValue(dummyHeaderContent, out SectionLinkReferenceDefinition result));
            Assert.Same(dummyFlexiSectionBlock, result.FlexiSectionBlock);
            Assert.NotNull(result.CreateLinkInline);
        }

        [Fact]
        public void GetOrCreateSectionLinkReferenceDefinitions_GetsSectionLinkReferenceDefinitionsIfItAlreadyExists()
        {
            // Arrange
            var dummySectionLinkReferenceDefinitions = new Dictionary<string, SectionLinkReferenceDefinition>();
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Document.SetData(FlexiSectionBlockParser.SECTION_LINK_REFERENCE_DEFINITIONS_KEY, dummySectionLinkReferenceDefinitions);
            FlexiSectionBlockParser testSubject = CreateExposedFlexiSectionBlockParser();

            // Act
            Dictionary<string, SectionLinkReferenceDefinition> result = testSubject.GetOrCreateSectionLinkReferenceDefinitions(dummyBlockProcessor);

            // Assert
            Assert.Same(dummySectionLinkReferenceDefinitions, result);
        }

        [Fact]
        public void GetOrCreateSectionLinkReferenceDefinitions_CreatesSectionLinkReferenceDefinitionsIfItDoesNotAlreadyExist()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            FlexiSectionBlockParser testSubject = CreateExposedFlexiSectionBlockParser();

            // Act
            Dictionary<string, SectionLinkReferenceDefinition> result = testSubject.GetOrCreateSectionLinkReferenceDefinitions(dummyBlockProcessor);

            // Assert
            Assert.NotNull(result);
            // No easy way to check whether a delegate has been added to the Document.ProcessInlinesBegin event here, specs cover this though.
        }

        [Fact]
        public void CreateLinkInline_CreatesLinkInline()
        {
            // Arrange
            const string dummyID = "dummyID";
            const string dummyTitle = "dummyTitle";
            var dummyFlexiSectionBlock = new FlexiSectionBlock(null) { ID = dummyID };
            var dummySectionLinkReferenceDefinition = new SectionLinkReferenceDefinition() { FlexiSectionBlock = dummyFlexiSectionBlock, Title = dummyTitle };
            FlexiSectionBlockParser testSubject = CreateExposedFlexiSectionBlockParser();

            // Act
            var result = (LinkInline)testSubject.CreateLinkInline(null, dummySectionLinkReferenceDefinition, null);

            // Assert
            Assert.Equal($"#{dummyID}", result.Url);
            Assert.Equal(dummyTitle, result.Title);
        }

        [Fact]
        public void DocumentOnProcessInlinesBegin_AddsSectionLinkReferenceDefinitionsWithoutOverridingExistingLinkReferenceDefinitions()
        {
            // Arrange
            const string dummySectionLinkReferenceDefinition1Key = "dummySectionLinkReferenceDefinition1Key";
            const string dummySectionLinkReferenceDefinition2Key = "dummySectionLinkReferenceDefinition2Key";
            var dummySectionLinkReferenceDefinition2 = new SectionLinkReferenceDefinition();
            var dummySectionLinkReferenceDefinitions = new Dictionary<string, SectionLinkReferenceDefinition>()
                {
                    { dummySectionLinkReferenceDefinition1Key, new SectionLinkReferenceDefinition() },
                    { dummySectionLinkReferenceDefinition2Key, dummySectionLinkReferenceDefinition2 }
                };
            var dummyLinkReferenceDefinition = new LinkReferenceDefinition();
            var dummyDocument = new MarkdownDocument();
            dummyDocument.SetData(FlexiSectionBlockParser.SECTION_LINK_REFERENCE_DEFINITIONS_KEY, dummySectionLinkReferenceDefinitions);
            dummyDocument.SetLinkReferenceDefinition(dummySectionLinkReferenceDefinition1Key, dummyLinkReferenceDefinition);
            InlineProcessor dummyInlineProcessor = MarkdigTypesFactory.CreateInlineProcessor(markdownDocument: dummyDocument);
            FlexiSectionBlockParser testSubject = CreateExposedFlexiSectionBlockParser();

            // Act
            testSubject.DocumentOnProcessInlinesBegin(dummyInlineProcessor, null);

            // Assert
            Dictionary<string, LinkReferenceDefinition> resultLinks = dummyDocument.GetLinkReferenceDefinitions().Links;
            Assert.Equal(2, resultLinks.Count);
            Assert.Same(dummyLinkReferenceDefinition, resultLinks[dummySectionLinkReferenceDefinition1Key]); // Not overriden
            Assert.Same(dummySectionLinkReferenceDefinition2, resultLinks[dummySectionLinkReferenceDefinition2Key]); // Added
        }

        [Fact]
        public void UpdateOpenFlexiSectionBlocks_DiscardsStacksForClosedTreesAndCreatesANewStackForANewTree()
        {
            // Arrange
            var dummyOpenTreeStack = new Stack<FlexiSectionBlock>();
            dummyOpenTreeStack.Push(new FlexiSectionBlock(null));
            var dummyClosedTreeStack = new Stack<FlexiSectionBlock>();
            dummyClosedTreeStack.Push(new FlexiSectionBlock(null) { IsOpen = false });
            var dummyOpenSectionBlocks = new Stack<Stack<FlexiSectionBlock>>();
            dummyOpenSectionBlocks.Push(dummyOpenTreeStack);
            dummyOpenSectionBlocks.Push(dummyClosedTreeStack);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyNewFlexiSectionBlock = new FlexiSectionBlock(null);
            Mock<ExposedFlexiSectionBlockParser> mockTestSubject = CreateMockExposedFlexiSectionBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.GetOrCreateOpenFlexiSectionBlocks(dummyBlockProcessor)).Returns(dummyOpenSectionBlocks);

            // Act
            mockTestSubject.Object.UpdateOpenFlexiSectionBlocks(dummyBlockProcessor, dummyNewFlexiSectionBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(2, dummyOpenSectionBlocks.Count); // Closed tree removed, open tree remains, new tree added
            Assert.Same(dummyNewFlexiSectionBlock, dummyOpenSectionBlocks.Pop().Peek());
            Assert.Same(dummyOpenTreeStack, dummyOpenSectionBlocks.Peek());
        }

        [Fact]
        public void UpdateOpenFlexiSectionBlocks_ClosesFlexiSectionBlocksInCurrentTreesStackWithTheSameOrLowerLevels()
        {
            // Arrange
            var dummyOpenTreeStack = new Stack<FlexiSectionBlock>();
            dummyOpenTreeStack.Push(new FlexiSectionBlock(null) { Level = 1 });
            var dummyFlexiSectionBlockToBeClosed = new FlexiSectionBlock(null) { Level = 2 };
            dummyOpenTreeStack.Push(dummyFlexiSectionBlockToBeClosed);
            dummyOpenTreeStack.Push(new FlexiSectionBlock(null) { Level = 3 });
            var dummyOpenSectionBlocks = new Stack<Stack<FlexiSectionBlock>>();
            dummyOpenSectionBlocks.Push(dummyOpenTreeStack);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            // These three lines set BlockProcessor.CurrentContainer to a FlexiSectionBlock
            Mock<BlockParser> mockBlockParser = _mockRepository.Create<BlockParser>();
            mockBlockParser.Setup(b => b.TryContinue(dummyBlockProcessor, It.IsAny<Block>())).Returns(BlockState.Continue);
            dummyBlockProcessor.Open(new FlexiSectionBlock(mockBlockParser.Object));
            dummyBlockProcessor.ProcessLine(new StringSlice(""));
            var dummyNewFlexiSectionBlock = new FlexiSectionBlock(null) { Level = 2 };
            Mock<ExposedFlexiSectionBlockParser> mockTestSubject = CreateMockExposedFlexiSectionBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.GetOrCreateOpenFlexiSectionBlocks(dummyBlockProcessor)).Returns(dummyOpenSectionBlocks);

            // Act
            mockTestSubject.Object.UpdateOpenFlexiSectionBlocks(dummyBlockProcessor, dummyNewFlexiSectionBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(2, dummyOpenTreeStack.Count);
            Assert.Same(dummyNewFlexiSectionBlock, dummyOpenTreeStack.Peek());
        }

        public class ExposedFlexiSectionBlockParser : FlexiSectionBlockParser
        {
            public ExposedFlexiSectionBlockParser(FlexiSectionBlocksExtensionOptions extensionOptions, IFlexiOptionsBlockService flexiOptionsBlockService) :
                base(flexiOptionsBlockService, extensionOptions)
            {
            }

            public BlockState ExposedTryOpenFlexiBlock(BlockProcessor processor)
            {
                return TryOpenFlexiBlock(processor);
            }

            public BlockState ExposedTryContinueFlexiBlock(BlockProcessor processor, Block block)
            {
                return TryContinueFlexiBlock(processor, block);
            }
        }

        private ExposedFlexiSectionBlockParser CreateExposedFlexiSectionBlockParser(FlexiSectionBlocksExtensionOptions extensionOptions = null,
            IFlexiOptionsBlockService flexiOptionsBlockService = null)
        {
            return new ExposedFlexiSectionBlockParser(
                extensionOptions ?? new FlexiSectionBlocksExtensionOptions(),
                flexiOptionsBlockService ?? _mockRepository.Create<IFlexiOptionsBlockService>().Object);
        }

        private Mock<ExposedFlexiSectionBlockParser> CreateMockExposedFlexiSectionBlockParser(FlexiSectionBlocksExtensionOptions extensionOptions = null,
            IFlexiOptionsBlockService flexiOptionsBlockService = null)
        {
            return _mockRepository.Create<ExposedFlexiSectionBlockParser>(
                extensionOptions ?? new FlexiSectionBlocksExtensionOptions(),
                flexiOptionsBlockService ?? _mockRepository.Create<IFlexiOptionsBlockService>().Object);
        }
    }
}
