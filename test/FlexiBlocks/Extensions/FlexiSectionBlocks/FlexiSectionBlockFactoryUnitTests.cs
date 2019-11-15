using Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;
namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiSectionBlocks
{
    public class FlexiSectionBlockFactoryUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfOptionsServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiSectionBlockFactory(null, _mockRepository.Create<IFlexiSectionHeadingBlockFactory>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiSectionHeadingBlockFactoryIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() =>
                new FlexiSectionBlockFactory(_mockRepository.Create<IOptionsService<IFlexiSectionBlockOptions, IFlexiSectionBlocksExtensionOptions>>().Object, null));
        }

        [Fact]
        public void Create_CreatesFlexiSectionBlockAndUpdatesOpenFlexiSectionBlocks()
        {
            // Arrange
            var dummyLine = new StringSlice("dummyLine", 4, 8);
            const int dummyColumn = 4;
            const string dummyBlockName = "dummyBlockName";
            const string dummyResolvedBlockName = "dummyResolvedBlockName";
            const SectioningContentElement dummyElement = SectioningContentElement.Aside;
            const string dummyLinkIcon = "dummyLinkIcon";
            const FlexiSectionBlockRenderingMode dummyRenderingMode = FlexiSectionBlockRenderingMode.Classic;
            const int dummyLevel = 5;
            var dummyAttributes = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
            Mock<IFlexiSectionBlockOptions> dummyFlexiSectionBlockOptions = _mockRepository.Create<IFlexiSectionBlockOptions>();
            dummyFlexiSectionBlockOptions.Setup(f => f.BlockName).Returns(dummyBlockName);
            dummyFlexiSectionBlockOptions.Setup(f => f.Element).Returns(dummyElement);
            dummyFlexiSectionBlockOptions.Setup(f => f.RenderingMode).Returns(dummyRenderingMode);
            dummyFlexiSectionBlockOptions.Setup(f => f.LinkIcon).Returns(dummyLinkIcon);
            dummyFlexiSectionBlockOptions.Setup(f => f.Attributes).Returns(dummyAttributes);
            Mock<BlockParser> dummyBlockParser = _mockRepository.Create<BlockParser>();
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = dummyColumn;
            dummyBlockProcessor.Line = dummyLine;
            var dummyFlexiSectionHeadingBlock = new FlexiSectionHeadingBlock(null);
            Mock<IFlexiSectionHeadingBlockFactory> mockFlexiSectionHeadingBlockFactory = _mockRepository.Create<IFlexiSectionHeadingBlockFactory>();
            mockFlexiSectionHeadingBlockFactory.
                Setup(f => f.Create(dummyBlockProcessor, dummyFlexiSectionBlockOptions.Object, dummyBlockParser.Object)).
                Returns(dummyFlexiSectionHeadingBlock);
            Mock<IOptionsService<IFlexiSectionBlockOptions, IFlexiSectionBlocksExtensionOptions>> mockOptionsService = _mockRepository.Create<IOptionsService<IFlexiSectionBlockOptions, IFlexiSectionBlocksExtensionOptions>>();
            mockOptionsService.Setup(o => o.CreateOptions(dummyBlockProcessor)).Returns((dummyFlexiSectionBlockOptions.Object, null));
            Mock<FlexiSectionBlockFactory> mockTestSubject = CreateMockFlexiSectionBlockFactory(mockOptionsService.Object, mockFlexiSectionHeadingBlockFactory.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.ValidateLevel(dummyLevel));
            mockTestSubject.Setup(t => t.ResolveBlockName(dummyBlockName)).Returns(dummyResolvedBlockName);
            mockTestSubject.Setup(t => t.ValidateElement(dummyElement));
            mockTestSubject.Setup(t => t.ValidateRenderingMode(dummyRenderingMode));
            mockTestSubject.Setup(t => t.UpdateOpenFlexiSectionBlocks(dummyBlockProcessor, It.IsAny<FlexiSectionBlock>()));

            // Act
            FlexiSectionBlock result = mockTestSubject.Object.Create(dummyLevel, dummyBlockProcessor, dummyBlockParser.Object);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyResolvedBlockName, result.BlockName);
            Assert.Equal(dummyElement, result.Element);
            Assert.Equal(dummyLinkIcon, result.LinkIcon);
            Assert.Equal(dummyRenderingMode, result.RenderingMode);
            Assert.Equal(dummyLevel, result.Level);
            Assert.Same(dummyAttributes, result.Attributes);
            Assert.Same(dummyBlockParser.Object, result.Parser);
            Assert.Equal(dummyColumn, result.Column);
            Assert.Equal(dummyLine.Start, result.Span.Start);
            Assert.Equal(dummyLine.End, result.Span.End);
            Assert.Single(result);
            Assert.Same(dummyFlexiSectionHeadingBlock, result[0]);
        }

        [Theory]
        [MemberData(nameof(ValidateLevel_ThrowsArgumentExceptionIfLevelIsLessThan1OrGreaterThan6_Data))]
        public void ValidateLevel_ThrowsArgumentExceptionIfLevelIsLessThan1OrGreaterThan6(int dummyLevel)
        {
            // Arrange
            FlexiSectionBlockFactory testSubject = CreateFlexiSectionBlockFactory();

            // Act
            ArgumentOutOfRangeException result = Assert.Throws<ArgumentOutOfRangeException>(() => testSubject.ValidateLevel(dummyLevel));

            // Assert
            Assert.Equal($@"{string.Format(Strings.ArgumentOutOfRangeException_Shared_ValueMustBeWithinRange, "[1, 6]", dummyLevel)}
Parameter name: level",
                result.Message, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> ValidateLevel_ThrowsArgumentExceptionIfLevelIsLessThan1OrGreaterThan6_Data()
        {
            return new object[][]
            {
                        // < 1
                        new object[]{0},
                        // > 6
                        new object[]{7}
            };
        }

        [Theory]
        [MemberData(nameof(ValidateLevel_DoesNothingIfLevelIsBetween1And6Inclusive_Data))]
        public void ValidateLevel_DoesNothingIfLevelIsBetween1And6Inclusive(int dummyLevel)
        {
            // Arrange
            FlexiSectionBlockFactory testSubject = CreateFlexiSectionBlockFactory();

            // Act and assert
            testSubject.ValidateLevel(dummyLevel); // Pass as long as this doesn't throw
        }

        public static IEnumerable<object[]> ValidateLevel_DoesNothingIfLevelIsBetween1And6Inclusive_Data()
        {
            return new object[][]
            {
                        new object[]{1},
                        new object[]{2},
                        new object[]{3},
                        new object[]{4},
                        new object[]{5},
                        new object[]{6}
            };
        }

        [Theory]
        [MemberData(nameof(ResolveBlockName_ResolvesBlockName_Data))]
        public void ResolveBlockName_ResolvesBlockName(string dummyBlockName, string expectedResult)
        {
            // Arrange
            FlexiSectionBlockFactory testSubject = CreateFlexiSectionBlockFactory();

            // Act
            string result = testSubject.ResolveBlockName(dummyBlockName);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ResolveBlockName_ResolvesBlockName_Data()
        {
            const string defaultBlockName = "flexi-section";

            return new object[][]
            {
                new object[]{"dummyBlockName", "dummyBlockName"},
                // If null, whitespace or an empty string, returns flexi-section
                new object[]{null, defaultBlockName},
                new object[]{" ", defaultBlockName},
                new object[]{string.Empty, defaultBlockName},
            };
        }

        [Fact]
        public void ValidateElement_ThrowsOptionsExceptionIfElementIsInvalid()
        {
            // Arrange
            FlexiSectionBlockFactory testSubject = CreateFlexiSectionBlockFactory();
            const SectioningContentElement dummyElement = (SectioningContentElement)9;

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => testSubject.ValidateElement(dummyElement));
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                            nameof(IFlexiSectionBlockOptions.Element),
                            string.Format(Strings.OptionsException_Shared_ValueMustBeAValidEnumValue, dummyElement,
                            nameof(SectioningContentElement))),
                        result.Message);
        }

        [Fact]
        public void ValidateRenderingMode_ThrowsOptionsExceptionIfRenderingModeIsInvalid()
        {
            // Arrange
            FlexiSectionBlockFactory testSubject = CreateFlexiSectionBlockFactory();
            const FlexiSectionBlockRenderingMode dummyRenderingMode = (FlexiSectionBlockRenderingMode)9;

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => testSubject.ValidateRenderingMode(dummyRenderingMode));
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                            nameof(IFlexiSectionBlockOptions.RenderingMode),
                            string.Format(Strings.OptionsException_Shared_ValueMustBeAValidEnumValue, dummyRenderingMode,
                            nameof(FlexiSectionBlockRenderingMode))),
                        result.Message);
        }

        [Fact]
        public void UpdateOpenFlexiSectionBlocks_IgnoresAncestorContainerBlocksThatAreClosed()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            // Create abitrary closed blocks (ListItemBlock and ListBlock) and set BlockProcessor.CurrentContainer to dummyListItemBlock.
            // Closed blocks should be ignored, so UpdateOpenFlexiSectionBlocks should not create a new stack.
            Mock<BlockParser> mockBlockParser = _mockRepository.Create<BlockParser>();
            mockBlockParser.Setup(b => b.TryContinue(dummyBlockProcessor, It.IsAny<Block>())).Returns(BlockState.Continue);
            var dummyListItemBlock = new ListItemBlock(mockBlockParser.Object);
            var dummyListBlock = new ListBlock(null) { dummyListItemBlock };
            FlexiSectionBlock dummyFlexiSectionBlock = CreateFlexiSectionBlock();
            dummyFlexiSectionBlock.Add(dummyListBlock);
            dummyBlockProcessor.Open(dummyListItemBlock);
            dummyBlockProcessor.ProcessLine(new StringSlice("")); // This line sets BlockProcessor.CurrentContainer to dummyListItemBlock
            dummyListItemBlock.IsOpen = false;
            dummyListBlock.IsOpen = false;
            FlexiSectionBlock dummyNewFlexiSectionBlock = CreateFlexiSectionBlock();
            var dummyCurrentBranch = new Stack<FlexiSectionBlock>();
            dummyCurrentBranch.Push(dummyFlexiSectionBlock);
            var dummyOpenSectionBlocks = new Stack<Stack<FlexiSectionBlock>>();
            dummyOpenSectionBlocks.Push(dummyCurrentBranch);
            Mock<FlexiSectionBlockFactory> mockTestSubject = CreateMockFlexiSectionBlockFactory();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.GetOrCreateOpenFlexiSectionBlocks(dummyBlockProcessor.Document)).Returns(dummyOpenSectionBlocks);

            // Act
            mockTestSubject.Object.UpdateOpenFlexiSectionBlocks(dummyBlockProcessor, dummyNewFlexiSectionBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Single(dummyCurrentBranch);
            Assert.Equal(dummyNewFlexiSectionBlock, dummyCurrentBranch.Peek());
        }

        [Fact]
        public void UpdateOpenFlexiSectionBlocks_DiscardsStacksForClosedTreesAndCreatesANewStackForANewTree()
        {
            // Arrange
            var dummyCurrentBranch = new Stack<FlexiSectionBlock>();
            dummyCurrentBranch.Push(CreateFlexiSectionBlock());
            FlexiSectionBlock dummyClosedFlexiSectionBlock = CreateFlexiSectionBlock();
            dummyClosedFlexiSectionBlock.IsOpen = false;
            var dummyClosedBranchStack = new Stack<FlexiSectionBlock>();
            dummyClosedBranchStack.Push(dummyClosedFlexiSectionBlock);
            var dummyOpenSectionBlocks = new Stack<Stack<FlexiSectionBlock>>();
            dummyOpenSectionBlocks.Push(dummyCurrentBranch);
            dummyOpenSectionBlocks.Push(dummyClosedBranchStack);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.ProcessLine(new StringSlice("")); // This line sets BlockProcessor.CurrentContainer to a MarkdownDocument
            FlexiSectionBlock dummyNewFlexiSectionBlock = CreateFlexiSectionBlock();
            Mock<FlexiSectionBlockFactory> mockTestSubject = CreateMockFlexiSectionBlockFactory();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.GetOrCreateOpenFlexiSectionBlocks(dummyBlockProcessor.Document)).Returns(dummyOpenSectionBlocks);

            // Act
            mockTestSubject.Object.UpdateOpenFlexiSectionBlocks(dummyBlockProcessor, dummyNewFlexiSectionBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(2, dummyOpenSectionBlocks.Count); // Closed tree removed, open tree remains, new tree added
            Assert.Same(dummyNewFlexiSectionBlock, dummyOpenSectionBlocks.Pop().Peek());
            Assert.Same(dummyCurrentBranch, dummyOpenSectionBlocks.Peek());
        }

        [Fact]
        public void UpdateOpenFlexiSectionBlocks_ClosesFlexiSectionBlocksInCurrentBranchWithTheSameOrHigherLevels()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<BlockParser> mockBlockParser = _mockRepository.Create<BlockParser>();
            mockBlockParser.Setup(b => b.TryContinue(dummyBlockProcessor, It.IsAny<Block>())).Returns(BlockState.Continue);
            FlexiSectionBlock dummyLevel1FlexiSectionBlock = CreateFlexiSectionBlock(level: 1, blockParser: mockBlockParser.Object);
            FlexiSectionBlock dummyLevel2FlexiSectionBlock = CreateFlexiSectionBlock(level: 2, blockParser: mockBlockParser.Object);
            FlexiSectionBlock dummyLevel3FlexiSectionBlock = CreateFlexiSectionBlock(level: 3, blockParser: mockBlockParser.Object);
            dummyBlockProcessor.Open(dummyLevel1FlexiSectionBlock);
            dummyBlockProcessor.Open(dummyLevel2FlexiSectionBlock);
            dummyBlockProcessor.Open(dummyLevel3FlexiSectionBlock);
            dummyBlockProcessor.ProcessLine(new StringSlice("")); // Sets BlockProcessor.CurrentContainer to a FlexiSectionBlock
            var dummyCurrentBranch = new Stack<FlexiSectionBlock>();
            dummyCurrentBranch.Push(dummyLevel1FlexiSectionBlock);
            dummyCurrentBranch.Push(dummyLevel2FlexiSectionBlock);
            dummyCurrentBranch.Push(dummyLevel3FlexiSectionBlock);
            var dummyOpenSectionBlocks = new Stack<Stack<FlexiSectionBlock>>();
            dummyOpenSectionBlocks.Push(dummyCurrentBranch);
            FlexiSectionBlock dummyNewFlexiSectionBlock = CreateFlexiSectionBlock(level: 2);
            Mock<FlexiSectionBlockFactory> mockTestSubject = CreateMockFlexiSectionBlockFactory();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.GetOrCreateOpenFlexiSectionBlocks(dummyBlockProcessor.Document)).Returns(dummyOpenSectionBlocks);

            // Act
            mockTestSubject.Object.UpdateOpenFlexiSectionBlocks(dummyBlockProcessor, dummyNewFlexiSectionBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(2, dummyCurrentBranch.Count); // Level 2 and level 3 blocks get removed
            Assert.Same(dummyNewFlexiSectionBlock, dummyCurrentBranch.Pop());
            Assert.Same(dummyLevel1FlexiSectionBlock, dummyCurrentBranch.Peek());
            mockBlockParser.Verify(b => b.Close(dummyBlockProcessor, dummyLevel2FlexiSectionBlock)); // Gets closed
            mockBlockParser.Verify(b => b.Close(dummyBlockProcessor, dummyLevel3FlexiSectionBlock)); // Gets closed
        }

        [Fact]
        public void GetOrCreateOpenFlexiSectionBlocks_GetsReferenceLinkableFlexiSectionBlocksIfItAlreadyExists()
        {
            // Arrange
            var dummyOpenFlexiSectionBlocks = new Stack<Stack<FlexiSectionBlock>>();
            var dummyMarkdownDocument = new MarkdownDocument();
            dummyMarkdownDocument.SetData(FlexiSectionBlockFactory.OPEN_PROXY_SECTION_BLOCKS_KEY, dummyOpenFlexiSectionBlocks);
            FlexiSectionBlockFactory testSubject = CreateFlexiSectionBlockFactory();

            // Act
            Stack<Stack<FlexiSectionBlock>> result = testSubject.GetOrCreateOpenFlexiSectionBlocks(dummyMarkdownDocument);

            // Assert
            Assert.Same(dummyOpenFlexiSectionBlocks, result);
        }

        [Fact]
        public void GetOrCreateOpenFlexiSectionBlocks_CreatesOpenFlexiSectionBlocksIfItDoesNotAlreadyExist()
        {
            // Arrange
            var dummyMarkdownDocument = new MarkdownDocument();
            FlexiSectionBlockFactory testSubject = CreateFlexiSectionBlockFactory();

            // Act
            Stack<Stack<FlexiSectionBlock>> result = testSubject.GetOrCreateOpenFlexiSectionBlocks(dummyMarkdownDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Same(result, dummyMarkdownDocument.GetData(FlexiSectionBlockFactory.OPEN_PROXY_SECTION_BLOCKS_KEY));
        }

        private Mock<FlexiSectionBlockFactory> CreateMockFlexiSectionBlockFactory(IOptionsService<IFlexiSectionBlockOptions, IFlexiSectionBlocksExtensionOptions> optionsService = default,
            IFlexiSectionHeadingBlockFactory flexiSectionHeadingBlockFactory = default)
        {
            return _mockRepository.Create<FlexiSectionBlockFactory>(optionsService ?? _mockRepository.Create<IOptionsService<IFlexiSectionBlockOptions, IFlexiSectionBlocksExtensionOptions>>().Object,
                flexiSectionHeadingBlockFactory ?? _mockRepository.Create<IFlexiSectionHeadingBlockFactory>().Object);
        }

        private FlexiSectionBlockFactory CreateFlexiSectionBlockFactory(IOptionsService<IFlexiSectionBlockOptions, IFlexiSectionBlocksExtensionOptions> optionsService = default,
            IFlexiSectionHeadingBlockFactory flexiSectionHeadingBlockFactory = default)
        {
            return new FlexiSectionBlockFactory(optionsService ?? _mockRepository.Create<IOptionsService<IFlexiSectionBlockOptions, IFlexiSectionBlocksExtensionOptions>>().Object,
                flexiSectionHeadingBlockFactory ?? _mockRepository.Create<IFlexiSectionHeadingBlockFactory>().Object);
        }

        private static FlexiSectionBlock CreateFlexiSectionBlock(string blockName = default,
            SectioningContentElement element = default,
            string linkIcon = default,
            FlexiSectionBlockRenderingMode renderingMode = default,
            int level = default,
            ReadOnlyDictionary<string, string> attributes = default,
            BlockParser blockParser = default)
        {
            return new FlexiSectionBlock(blockName, element, linkIcon, renderingMode, level, attributes, blockParser);
        }
    }
}
