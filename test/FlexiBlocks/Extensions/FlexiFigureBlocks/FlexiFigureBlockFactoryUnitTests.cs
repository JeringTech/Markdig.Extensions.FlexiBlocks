using Jering.Markdig.Extensions.FlexiBlocks.FlexiFigureBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiFigureBlocks
{
    public class FlexiFigureBlockFactoryUnitTests
    {
        private static readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Empty };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfOptionsServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiFigureBlockFactory(null, new PlainBlockParser()));
        }

        [Fact]
        public void Create_CreatesFlexiFigureBlock()
        {
            // Arrange
            const int dummyColumn = 2;
            var dummyLine = new StringSlice("dummyText", 3, 8);
            const string dummyBlockName = "dummyBlockName";
            const string dummyResolvedBlockName = "dummyResolvedBlockName";
            const int dummyFigureNumber = 5;
            const bool dummyGenerateID = true;
            const string dummyID = "dummyID";
            const bool dummyReferenceLinkable = true;
            const bool dummyResolvedReferenceLinkable = false;
            const string dummyLinkLabelContent = "dummyLinkLabelContent";
            const bool dummyLinkLabelContentSpecified = true;
            const bool dummyRenderName = true;
            const string dummyName = "dummyName";
            const string dummyResolvedLinkLabelContent = "dummyResolvedLinkLabelContent";
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = dummyColumn;
            dummyBlockProcessor.Line = dummyLine;
            Mock<BlockParser> dummyBlockParser = _mockRepository.Create<BlockParser>();
            var dummyAttributes = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
            Mock<IFlexiFigureBlockOptions> mockFlexiFigureBlockOptions = _mockRepository.Create<IFlexiFigureBlockOptions>();
            mockFlexiFigureBlockOptions.Setup(f => f.BlockName).Returns(dummyBlockName);
            mockFlexiFigureBlockOptions.Setup(f => f.GenerateID).Returns(dummyGenerateID);
            mockFlexiFigureBlockOptions.Setup(f => f.ReferenceLinkable).Returns(dummyReferenceLinkable);
            mockFlexiFigureBlockOptions.Setup(f => f.RenderName).Returns(dummyRenderName);
            mockFlexiFigureBlockOptions.Setup(f => f.LinkLabelContent).Returns(dummyLinkLabelContent);
            mockFlexiFigureBlockOptions.Setup(f => f.Attributes).Returns(dummyAttributes);
            Mock<IOptionsService<IFlexiFigureBlockOptions, IFlexiFigureBlocksExtensionOptions>> mockOptionsService = _mockRepository.
                Create<IOptionsService<IFlexiFigureBlockOptions, IFlexiFigureBlocksExtensionOptions>>();
            mockOptionsService.Setup(f => f.CreateOptions(dummyBlockProcessor)).
                Returns((mockFlexiFigureBlockOptions.Object, (IFlexiFigureBlocksExtensionOptions)null));
            Mock<FlexiFigureBlockFactory> mockTestSubject = CreateMockFlexiFigureBlockFactory(mockOptionsService.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.ResolveBlockName(dummyBlockName)).Returns(dummyResolvedBlockName);
            mockTestSubject.Setup(t => t.GetFlexiFigureBlockNumber(dummyBlockProcessor)).Returns(dummyFigureNumber);
            mockTestSubject.Setup(t => t.ResolveID(dummyGenerateID, dummyFigureNumber, dummyAttributes)).Returns(dummyID);
            mockTestSubject.Setup(t => t.ResolveReferenceLinkable(dummyReferenceLinkable, dummyID)).Returns(dummyResolvedReferenceLinkable);
            mockTestSubject.Setup(t => t.IsLinkLabelContentSpecified(dummyLinkLabelContent)).Returns(dummyLinkLabelContentSpecified);
            mockTestSubject.Setup(t => t.ResolveName(dummyResolvedReferenceLinkable, dummyLinkLabelContentSpecified, dummyRenderName, dummyFigureNumber)).Returns(dummyName);
            mockTestSubject.Setup(t => t.ResolveLinkLabelContent(dummyResolvedReferenceLinkable, dummyLinkLabelContentSpecified, dummyName, dummyLinkLabelContent)).Returns(dummyResolvedLinkLabelContent);

            // Act
            FlexiFigureBlock result = mockTestSubject.Object.Create(dummyBlockProcessor, dummyBlockParser.Object);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyResolvedBlockName, result.BlockName);
            Assert.Equal(dummyName, result.Name);
            Assert.Equal(dummyRenderName, result.RenderName);
            Assert.Equal(dummyResolvedLinkLabelContent, result.LinkLabelContent);
            Assert.Equal(dummyID, result.ID);
            Assert.Same(dummyAttributes, result.Attributes);
            Assert.Same(dummyBlockParser.Object, result.Parser);
            Assert.Equal(dummyColumn, result.Column);
            Assert.Equal(dummyLine.Start, result.Span.Start);
            Assert.Equal(0, result.Span.End);
        }

        [Theory]
        [MemberData(nameof(ResolveBlockName_ResolvesBlockName_Data))]
        public void ResolveBlockName_ResolvesBlockName(string dummyBlockName, string expectedResult)
        {
            // Arrange
            FlexiFigureBlockFactory testSubject = CreateFlexiFigureBlockFactory();

            // Act
            string result = testSubject.ResolveBlockName(dummyBlockName);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ResolveBlockName_ResolvesBlockName_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string defaultBlockName = "flexi-figure";

            return new object[][]
            {
                new object[]{dummyBlockName, dummyBlockName},
                new object[]{null, defaultBlockName},
                new object[]{" ", defaultBlockName},
                new object[]{string.Empty, defaultBlockName}
            };
        }

        [Fact]
        public void GetFlexiFigureBlockNumber_IfThereIsAStoredNumberReturnsItAndIncrementsStoredNumberByOne()
        {
            // Arrange
            const int dummyStoredNumber = 6;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Document.SetData(FlexiFigureBlockFactory.NEXT_FLEXI_FIGURE_BLOCK_NUMBER_KEY, dummyStoredNumber);
            FlexiFigureBlockFactory testSubject = CreateFlexiFigureBlockFactory();

            // Act
            int result = testSubject.GetFlexiFigureBlockNumber(dummyBlockProcessor);

            // Assert
            Assert.Equal(dummyStoredNumber, result);
            object newStoredNumber = dummyBlockProcessor.Document.GetData(FlexiFigureBlockFactory.NEXT_FLEXI_FIGURE_BLOCK_NUMBER_KEY);
            Assert.Equal(dummyStoredNumber + 1, newStoredNumber); // Incremented by 1
        }

        [Theory]
        [MemberData(nameof(GetFlexiFigureBlockNumber_IfThereIsNoStoredNumberReturnsOneAndStoresTwo_Data))]
        public void GetFlexiFigureBlockNumber_IfThereIsNoStoredNumberReturnsOneAndStoresTwo(BlockProcessor dummyBlockProcessor)
        {
            // Arrange
            FlexiFigureBlockFactory testSubject = CreateFlexiFigureBlockFactory();

            // Act
            int result = testSubject.GetFlexiFigureBlockNumber(dummyBlockProcessor);

            // Assert
            Assert.Equal(1, result);
            object newStoredNumber = dummyBlockProcessor.Document.GetData(FlexiFigureBlockFactory.NEXT_FLEXI_FIGURE_BLOCK_NUMBER_KEY);
            Assert.Equal(2, newStoredNumber);
        }

        public static IEnumerable<object[]> GetFlexiFigureBlockNumber_IfThereIsNoStoredNumberReturnsOneAndStoresTwo_Data()
        {
            BlockProcessor dummyBlockProcessorWithNonIntNextNumber = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessorWithNonIntNextNumber.Document.SetData(FlexiFigureBlockFactory.NEXT_FLEXI_FIGURE_BLOCK_NUMBER_KEY, new object());

            return new object[][]
            {
                new object[]{MarkdigTypesFactory.CreateBlockProcessor()}, // No next number
                new object[]{dummyBlockProcessorWithNonIntNextNumber}
            };
        }

        [Theory]
        [MemberData(nameof(ResolveID_ResolvesID_Data))]
        public void ResolveID_ResolvesID(bool dummyGenerateID, int dummyFigureNumber, ReadOnlyDictionary<string, string> dummyAttributes, string expectedResult)
        {
            // Arrange
            FlexiFigureBlockFactory testSubject = CreateFlexiFigureBlockFactory();

            // Act
            string result = testSubject.ResolveID(dummyGenerateID, dummyFigureNumber, dummyAttributes);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ResolveID_ResolvesID_Data()
        {
            const string dummyCustomID = "dummyCustomID";
            const int dummyFigureNumber = 2;
            const string dummyGenerateID = "figure-2";

            return new object[][]
            {
                // Attributes contains custom id that isn't null, whitespace or an empty string
                new object[]{false, 0, new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { "id", dummyCustomID } }), dummyCustomID },
                // No custom id and generateID is false
                new object[]{false, 0, null, null },
                new object[]{false, 0, new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()), null },
                new object[]{false, 0, new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { "id", " " } }), null },
                new object[]{false, 0, new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { "id", null } }), null },
                new object[]{false, 0, new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { "id", string.Empty } }), null },
                // No custom id and generateID is true
                new object[]{true, dummyFigureNumber, null, dummyGenerateID },
                new object[]{true, dummyFigureNumber, new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()), dummyGenerateID },
            };
        }

        [Theory]
        [MemberData(nameof(ResolveReferenceLinkable_ResolvesReferenceLinkable_Data))]
        public void ResolveReferenceLinkable_ResolvesReferenceLinkable(bool dummyReferenceLinkable, string dummyID, bool expectedResult)
        {
            // Arrange
            FlexiFigureBlockFactory testSubject = CreateFlexiFigureBlockFactory();

            // Act
            bool result = testSubject.ResolveReferenceLinkable(dummyReferenceLinkable, dummyID);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ResolveReferenceLinkable_ResolvesReferenceLinkable_Data()
        {
            const string dummyID = "dummyID";

            return new object[][]
            {
                new object[]{true, null, false},
                new object[]{false, null, false},
                new object[]{false, dummyID, false},
                new object[]{true, dummyID, true},
            };
        }

        [Theory]
        [MemberData(nameof(IsLinkLabelContentSpecified_ChecksIfLinkLabelContentIsSpecified_Data))]
        public void IsLinkLabelContentSpecified_ChecksIfLinkLabelContentIsSpecified(string dummyLinkLabelContent, bool expectedResult)
        {
            // Arrange
            FlexiFigureBlockFactory testSubject = CreateFlexiFigureBlockFactory();

            // Act
            bool result = testSubject.IsLinkLabelContentSpecified(dummyLinkLabelContent);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> IsLinkLabelContentSpecified_ChecksIfLinkLabelContentIsSpecified_Data()
        {
            return new object[][]
            {
                new object[]{" ", false},
                new object[]{null, false},
                new object[]{string.Empty, false},
                new object[]{"dummyLinkLabelContent", true}
            };
        }

        [Theory]
        [MemberData(nameof(ResolveName_ResolvesName_Data))]
        public void ResolveName_ResolvesName(bool dummyReferenceLinkable, bool dummyLinkLabelContentSpecified, bool dummyRenderName, int dummyFigureNumber, string expectedResult)
        {
            // Arrange
            FlexiFigureBlockFactory testSubject = CreateFlexiFigureBlockFactory();

            // Act
            string result = testSubject.ResolveName(dummyReferenceLinkable, dummyLinkLabelContentSpecified, dummyRenderName, dummyFigureNumber);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ResolveName_ResolvesName_Data()
        {
            const int dummyFigureNumber = 4;
            string dummyGeneratedName = $"Figure {dummyFigureNumber}";

            return new object[][]
            {
                // So long as renderName is true, we must generate name
                new object[]{false, false, true, dummyFigureNumber, dummyGeneratedName},
                new object[]{false, true, true, dummyFigureNumber, dummyGeneratedName},
                new object[]{true, false, true, dummyFigureNumber, dummyGeneratedName},
                new object[]{true, true, true, dummyFigureNumber, dummyGeneratedName},
                // If reference-linking is enabled and there is no link label content, we must generate name for use as reference-link label
                new object[]{true, false, false, dummyFigureNumber, dummyGeneratedName},
                // Reference-linking disabled and name not rendered
                new object[]{false, false, false, 0, null},
                new object[]{false, true, false, 0, null},
                // Reference-linking enabled but link label content is specified
                new object[]{true, true, false, 0, null}
            };
        }

        [Theory]
        [MemberData(nameof(ResolveLinkLabelContent_ResolvesLinkLabelContent_Data))]
        public void ResolveLinkLabelContent_ResolvesLinkLabelContent(bool dummyReferenceLinkable, bool dummyLinkLabelContentSpecified, string dummyName, string dummyLinkLabelContent, string expectedResult)
        {
            // Arrange
            FlexiFigureBlockFactory testSubject = CreateFlexiFigureBlockFactory();

            // Act
            string result = testSubject.ResolveLinkLabelContent(dummyReferenceLinkable, dummyLinkLabelContentSpecified, dummyName, dummyLinkLabelContent);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ResolveLinkLabelContent_ResolvesLinkLabelContent_Data()
        {
            const string dummyName = "dummyName";
            const string dummyLinkLabelContent = "dummyLinkLabelContent";

            return new object[][]
            {
                // Reference-linking disabled, no need for a label content
                new object[]{false, false, dummyName, dummyLinkLabelContent, null},
                // Reference-linking enabled, returns link label content if it's specified
                new object[]{true, true, dummyName, dummyLinkLabelContent, dummyLinkLabelContent},
                // Reference-linking enabled, returns name if link label content isn't specified
                new object[]{true, false, dummyName, dummyLinkLabelContent, dummyName},
            };
        }

        [Fact]
        public void SetupReferenceLinking_AddsFlexiFigureBlockToReferenceLinkableFlexiFigureBlocksIfReferenceLinkableIsTrue()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            FlexiFigureBlock dummyFlexiFigureBlock = CreateFlexiFigureBlock();
            var dummyReferenceLinkableFlexiFigureBlocks = new List<FlexiFigureBlock>();
            Mock<FlexiFigureBlockFactory> mockTestSubject = CreateMockFlexiFigureBlockFactory();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.GetOrCreateReferenceLinkableFlexiFigureBlocks(dummyBlockProcessor.Document)).Returns(dummyReferenceLinkableFlexiFigureBlocks);

            // Act
            mockTestSubject.Object.SetupReferenceLinking(dummyFlexiFigureBlock, true, dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Single(dummyReferenceLinkableFlexiFigureBlocks);
            Assert.Same(dummyFlexiFigureBlock, dummyReferenceLinkableFlexiFigureBlocks[0]);
        }

        [Fact]
        public void SetupReferenceLinking_DoesNothingIfReferenceLinkableIsFalse()
        {
            // Arrange
            FlexiFigureBlockFactory testSubject = CreateFlexiFigureBlockFactory();

            // Act and assert
            testSubject.SetupReferenceLinking(null, false, null); // If this doesn't throw, method isn't trying to do anything
        }

        [Fact]
        public void GetOrCreateReferenceLinkableFlexiFigureBlocks_GetsReferenceLinkableFlexiFigureBlocksIfItAlreadyExists()
        {
            // Arrange
            var dummyReferenceLinkableFlexiFigureBlocks = new List<FlexiFigureBlock>();
            var dummyMarkdownDocument = new MarkdownDocument();
            dummyMarkdownDocument.SetData(FlexiFigureBlockFactory.REFERENCE_LINKABLE_FLEXI_FIGURE_BLOCKS_KEY, dummyReferenceLinkableFlexiFigureBlocks);
            FlexiFigureBlockFactory testSubject = CreateFlexiFigureBlockFactory();

            // Act
            List<FlexiFigureBlock> result = testSubject.GetOrCreateReferenceLinkableFlexiFigureBlocks(dummyMarkdownDocument);

            // Assert
            Assert.Same(dummyReferenceLinkableFlexiFigureBlocks, result);
        }

        [Fact]
        public void GetOrCreateReferenceLinkableFlexiFigureBlocks_CreatesReferenceLinkableFlexiFigureBlocksIfItDoesNotAlreadyExist()
        {
            // Arrange
            var dummyMarkdownDocument = new MarkdownDocument();
            FlexiFigureBlockFactory testSubject = CreateFlexiFigureBlockFactory();

            // Act
            List<FlexiFigureBlock> result = testSubject.GetOrCreateReferenceLinkableFlexiFigureBlocks(dummyMarkdownDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Same(result, dummyMarkdownDocument.GetData(FlexiFigureBlockFactory.REFERENCE_LINKABLE_FLEXI_FIGURE_BLOCKS_KEY));
        }

        [Fact]
        public void DocumentOnProcessInlinesBegin_CreatesAndAddsALinkReferenceDefinitionForEachReferenceLinkableFlexiFigureBlock()
        {
            // Arrange
            const string dummyLinkLabelContent = "dummyLinkLabelContent";
            const string dummyID = "dummyID";
            const string dummyName = "dummyName";
            FlexiFigureBlock dummyFlexiFigureBlock = CreateFlexiFigureBlock(name: dummyName, linkLabelContent: dummyLinkLabelContent, id: dummyID);
            InlineProcessor dummyInlineProcessor = MarkdigTypesFactory.CreateInlineProcessor();
            dummyInlineProcessor.Document.SetData(FlexiFigureBlockFactory.REFERENCE_LINKABLE_FLEXI_FIGURE_BLOCKS_KEY, new List<FlexiFigureBlock> { dummyFlexiFigureBlock });
            FlexiFigureBlockFactory testSubject = CreateFlexiFigureBlockFactory();

            // Act
            testSubject.DocumentOnProcessInlinesBegin(dummyInlineProcessor, null);

            // Assert
            Dictionary<string, LinkReferenceDefinition> linkReferenceDefinitions = (dummyInlineProcessor.Document.GetData(typeof(LinkReferenceDefinitionGroup)) as LinkReferenceDefinitionGroup)?.Links;
            Assert.Single(linkReferenceDefinitions);
            LinkReferenceDefinition resultLinkReferenceDefinition = linkReferenceDefinitions.Values.First();
            Assert.Equal(dummyLinkLabelContent, resultLinkReferenceDefinition.Label);
            Assert.Equal($"#{dummyID}", resultLinkReferenceDefinition.Url);
            Assert.Equal(dummyName, resultLinkReferenceDefinition.Title);
        }

        [Fact]
        public void DocumentOnProcessInlinesBegin_DoesNotReplaceExistingLinkReferenceDefinitions()
        {
            // Arrange
            const string dummyLinkLabelContent = "dummyLinkLabelContent";
            InlineProcessor dummyInlineProcessor = MarkdigTypesFactory.CreateInlineProcessor();
            FlexiFigureBlock dummyFlexiFigureBlock = CreateFlexiFigureBlock(linkLabelContent: dummyLinkLabelContent);
            dummyInlineProcessor.Document.SetData(FlexiFigureBlockFactory.REFERENCE_LINKABLE_FLEXI_FIGURE_BLOCKS_KEY,
                new List<FlexiFigureBlock> { dummyFlexiFigureBlock });
            var dummyLinkReferenceDefinition = new LinkReferenceDefinition();
            dummyInlineProcessor.Document.SetLinkReferenceDefinition(dummyLinkLabelContent, dummyLinkReferenceDefinition);
            FlexiFigureBlockFactory testSubject = CreateFlexiFigureBlockFactory();

            // Act
            testSubject.DocumentOnProcessInlinesBegin(dummyInlineProcessor, null);

            // Assert
            Dictionary<string, LinkReferenceDefinition> linkReferenceDefinitions =
                (dummyInlineProcessor.Document.GetData(typeof(LinkReferenceDefinitionGroup)) as LinkReferenceDefinitionGroup)?.Links;
            Assert.Single(linkReferenceDefinitions);
            Assert.Same(dummyLinkReferenceDefinition, linkReferenceDefinitions.Values.First()); // Doesn't get replaced
        }

        [Theory]
        [MemberData(nameof(CreateLinkInline_CreatesLinkInline_Data))]
        public void CreateLinkInline_CreatesLinkInline(string dummyLabel, string dummyName, string expectedContent)
        {
            // Arrange
            const string dummyUrl = "dummyUrl";
            var dummyLiteralInline = new LiteralInline(dummyLabel);
            var linkReferenceDefinition = new LinkReferenceDefinition(dummyLabel, dummyUrl, dummyName); // We use the title property to hold the block's name
            FlexiFigureBlockFactory testSubject = CreateFlexiFigureBlockFactory();

            // Act
            Inline result = testSubject.CreateLinkInline(null, linkReferenceDefinition, dummyLiteralInline);

            // Assert
            Assert.IsType<LinkInline>(result);
            Assert.Equal(dummyUrl, ((LinkInline)result).Url);
            Assert.Equal(expectedContent, dummyLiteralInline.ToString());
        }

        public static IEnumerable<object[]> CreateLinkInline_CreatesLinkInline_Data()
        {
            const string dummyLabel = "dummyLabel";
            const string dummyName = "dummyName";

            return new object[][]
            {
                // If label and name are the same, child content remains as label 
                new object[]{dummyLabel, dummyLabel, dummyLabel},
                // If label and name differ, replaces child content with name
                new object[]{dummyLabel, dummyName, dummyName}
            };
        }

        private Mock<FlexiFigureBlockFactory> CreateMockFlexiFigureBlockFactory(IOptionsService<IFlexiFigureBlockOptions, IFlexiFigureBlocksExtensionOptions> optionsService = null,
            PlainBlockParser plainBlockParser = null)
        {
            return _mockRepository.
                Create<FlexiFigureBlockFactory>(optionsService ?? _mockRepository.Create<IOptionsService<IFlexiFigureBlockOptions, IFlexiFigureBlocksExtensionOptions>>().Object,
                plainBlockParser ?? new PlainBlockParser());
        }

        private FlexiFigureBlockFactory CreateFlexiFigureBlockFactory(IOptionsService<IFlexiFigureBlockOptions, IFlexiFigureBlocksExtensionOptions> optionsService = null,
            PlainBlockParser plainBlockParser = null)
        {
            return new FlexiFigureBlockFactory(optionsService ?? _mockRepository.Create<IOptionsService<IFlexiFigureBlockOptions, IFlexiFigureBlocksExtensionOptions>>().Object,
                plainBlockParser ?? new PlainBlockParser());
        }

        private static FlexiFigureBlock CreateFlexiFigureBlock(string blockName = default,
            string name = default,
            bool renderName = default,
            string linkLabelContent = default,
            string id = default,
            ReadOnlyDictionary<string, string> attributes = default,
            BlockParser blockParser = default)
        {
            return new FlexiFigureBlock(blockName, name, renderName, linkLabelContent, id, attributes, blockParser);
        }
    }
}
