using Jering.Markdig.Extensions.FlexiBlocks.FlexiQuoteBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiQuoteBlocks
{
    public class FlexiQuoteBlockFactoryUnitTests
    {
        private static readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Empty };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfOptionsServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiQuoteBlockFactory(null, new PlainBlockParser()));
        }

        [Fact]
        public void Create_CreatesFlexiQuoteBlock()
        {
            // Arrange
            const int dummyColumn = 2;
            var dummyLine = new StringSlice("dummyText", 3, 8);
            const string dummyBlockName = "dummyBlockName";
            const string dummyResolvedBlockName = "dummyResolvedBlockName";
            const string dummyIcon = "dummyIcon";
            const int dummyCiteLink = -3;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = dummyColumn;
            dummyBlockProcessor.Line = dummyLine;
            Mock<BlockParser> dummyBlockParser = _mockRepository.Create<BlockParser>();
            var dummyAttributes = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
            Mock<IFlexiQuoteBlockOptions> mockFlexiQuoteBlockOptions = _mockRepository.Create<IFlexiQuoteBlockOptions>();
            mockFlexiQuoteBlockOptions.Setup(f => f.BlockName).Returns(dummyBlockName);
            mockFlexiQuoteBlockOptions.Setup(f => f.Icon).Returns(dummyIcon);
            mockFlexiQuoteBlockOptions.Setup(f => f.CiteLink).Returns(dummyCiteLink);
            mockFlexiQuoteBlockOptions.Setup(f => f.Attributes).Returns(dummyAttributes);
            Mock<IOptionsService<IFlexiQuoteBlockOptions, IFlexiQuoteBlocksExtensionOptions>> mockOptionsService = _mockRepository.
                Create<IOptionsService<IFlexiQuoteBlockOptions, IFlexiQuoteBlocksExtensionOptions>>();
            mockOptionsService.Setup(f => f.CreateOptions(dummyBlockProcessor)).
                Returns((mockFlexiQuoteBlockOptions.Object, (IFlexiQuoteBlocksExtensionOptions)null));
            Mock<FlexiQuoteBlockFactory> mockTestSubject = CreateMockFlexiQuoteBlockFactory(mockOptionsService.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.ResolveBlockName(dummyBlockName)).Returns(dummyResolvedBlockName);

            // Act
            FlexiQuoteBlock result = mockTestSubject.Object.Create(dummyBlockProcessor, dummyBlockParser.Object);

            // TODO how can we verify that ProcessInlineEnd event handler is added? Due to language restrictions, no simple solution, got to get Block.OnProcessInlinesEnd to fire
            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyResolvedBlockName, result.BlockName);
            Assert.Equal(dummyIcon, result.Icon);
            Assert.Equal(dummyCiteLink, result.CiteLink);
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
            FlexiQuoteBlockFactory testSubject = CreateFlexiQuoteBlockFactory();

            // Act
            string result = testSubject.ResolveBlockName(dummyBlockName);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ResolveBlockName_ResolvesBlockName_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string defaultBlockName = "flexi-quote";

            return new object[][]
            {
                new object[]{dummyBlockName, dummyBlockName},
                new object[]{null, defaultBlockName},
                new object[]{" ", defaultBlockName},
                new object[]{string.Empty, defaultBlockName}
            };
        }

        [Fact]
        public void ExtractCiteUrl_DoesNothingIfThereAreNoLinkInlinesInCitationBlock()
        {
            // Arrange
            LeafBlock dummyCitationBlock = _mockRepository.Create<LeafBlock>(null).Object;
            InlineProcessor dummyInlineProcessor = MarkdigTypesFactory.CreateInlineProcessor();
            dummyInlineProcessor.ProcessInlineLeaf(dummyCitationBlock); // Sets InlineProcessor.Block to dummyCitationBlock
            FlexiQuoteBlockFactory testSubject = CreateFlexiQuoteBlockFactory();

            // Act and assert
            testSubject.ExtractCiteUrl(dummyInlineProcessor, null); // If this doesn't throw, we never attempted to extract cite URL
        }

        [Fact]
        public void ExtractCiteUrl_ExtractsCiteUrlIfSuccessful()
        {
            // Arrange
            const string dummyUrl = "dummyLink";
            var dummyAuthorLinkInline = new LinkInline();
            var dummyCiteLinkInline = new LinkInline(dummyUrl, null);
            var dummyContainerInline = new ContainerInline();
            dummyContainerInline.
                AppendChild(dummyAuthorLinkInline).
                AppendChild(dummyCiteLinkInline);
            LeafBlock dummyCitationBlock = _mockRepository.Create<LeafBlock>(null).Object;
            InlineProcessor dummyInlineProcessor = MarkdigTypesFactory.CreateInlineProcessor();
            dummyInlineProcessor.ProcessInlineLeaf(dummyCitationBlock); // Sets InlineProcessor.Block to dummyCitationBlock
            dummyCitationBlock.Inline = dummyContainerInline; // Replace container created in ProcessInlineLeaf
            FlexiQuoteBlock dummyFlexiQuoteBlock = CreateFlexiQuoteBlock();
            dummyFlexiQuoteBlock.Add(dummyCitationBlock);
            Mock<FlexiQuoteBlockFactory> mockTestSubject = CreateMockFlexiQuoteBlockFactory();
            mockTestSubject.Setup(m => m.NormalizeCiteLinkIndex(2, dummyFlexiQuoteBlock)).Returns(1);

            // Act
            mockTestSubject.Object.ExtractCiteUrl(dummyInlineProcessor, null);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyUrl, dummyFlexiQuoteBlock.CiteUrl);
        }

        [Theory]
        [MemberData(nameof(NormalizeCiteLinkIndex_ThrowsBlockExceptionIfCiteLinkIndexIsInvalid_Data))]
        public void NormalizeCiteLinkIndex_ThrowsBlockExceptionIfCiteLinkIndexIsInvalid(int dummyNumLinks, int dummyCiteLinkIndex)
        {
            // Arrange
            const int dummyLineIndex = 5;
            const int dummyColumn = 2;
            FlexiQuoteBlock dummyFlexiQuoteBlock = CreateFlexiQuoteBlock(citeLink: dummyCiteLinkIndex);
            dummyFlexiQuoteBlock.Column = dummyColumn;
            dummyFlexiQuoteBlock.Line = dummyLineIndex;
            FlexiQuoteBlockFactory testSubject = CreateFlexiQuoteBlockFactory();

            // Act and assert
            BlockException result = Assert.Throws<BlockException>(() => testSubject.NormalizeCiteLinkIndex(dummyNumLinks, dummyFlexiQuoteBlock));
            Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock,
                    nameof(FlexiQuoteBlock),
                    dummyLineIndex + 1,
                    dummyColumn,
                    Strings.BlockException_BlockException_ExceptionOccurredWhileProcessingBlock),
                result.Message);
            var resultOptionsException = result.InnerException as OptionsException;
            Assert.IsType<OptionsException>(resultOptionsException);
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                    nameof(IFlexiQuoteBlockOptions.CiteLink),
                    string.Format(Strings.OptionsException_FlexiQuoteBlockFactory_UnableToNormalize, dummyCiteLinkIndex, dummyNumLinks)),
                resultOptionsException.Message);
        }

        public static IEnumerable<object[]> NormalizeCiteLinkIndex_ThrowsBlockExceptionIfCiteLinkIndexIsInvalid_Data()
        {
            return new object[][]
            {
                // Positive index - greater than largest possible
                new object[]{5, 5},
                new object[]{5, 6},
                // Negative index - smaller than smallest possible
                new object[]{3, -4}
            };
        }

        [Theory]
        [MemberData(nameof(NormalizeCiteLinkIndex_NormalizesCiteLinkIndexIfSuccessful_Data))]
        public void NormalizeCiteLinkIndex_NormalizesCiteLinkIndexIfSuccessful(int dummyNumLinks, int dummyCiteLinkIndex, int expectedCiteLinkIndex)
        {
            // Arrange
            FlexiQuoteBlock dummyFlexiQuoteBlock = CreateFlexiQuoteBlock(citeLink: dummyCiteLinkIndex);
            FlexiQuoteBlockFactory testSubject = CreateFlexiQuoteBlockFactory();

            // Act
            int result = testSubject.NormalizeCiteLinkIndex(dummyNumLinks, dummyFlexiQuoteBlock);

            // Assert
            Assert.Equal(expectedCiteLinkIndex, result);
        }

        public static IEnumerable<object[]> NormalizeCiteLinkIndex_NormalizesCiteLinkIndexIfSuccessful_Data()
        {
            return new object[][]
            {
                // Negative cite link index
                new object[]{1, -1, 0},
                new object[]{3, -3, 0},
                new object[]{5, -2, 3},
                // Positive cite link index
                new object[]{5, 1, 1},
                // 0
                new object[]{3, 0, 0}
            };
        }

        private Mock<FlexiQuoteBlockFactory> CreateMockFlexiQuoteBlockFactory(IOptionsService<IFlexiQuoteBlockOptions, IFlexiQuoteBlocksExtensionOptions> optionsService = null,
            PlainBlockParser plainBlockParser = null)
        {
            return _mockRepository.
                Create<FlexiQuoteBlockFactory>(optionsService ?? _mockRepository.Create<IOptionsService<IFlexiQuoteBlockOptions, IFlexiQuoteBlocksExtensionOptions>>().Object,
                plainBlockParser ?? new PlainBlockParser());
        }

        private FlexiQuoteBlockFactory CreateFlexiQuoteBlockFactory(IOptionsService<IFlexiQuoteBlockOptions, IFlexiQuoteBlocksExtensionOptions> optionsService = null,
            PlainBlockParser plainBlockParser = null)
        {
            return new FlexiQuoteBlockFactory(optionsService ?? _mockRepository.Create<IOptionsService<IFlexiQuoteBlockOptions, IFlexiQuoteBlocksExtensionOptions>>().Object,
                plainBlockParser ?? new PlainBlockParser());
        }

        private static FlexiQuoteBlock CreateFlexiQuoteBlock(string blockName = default,
            string icon = default,
            int citeLink = default,
            ReadOnlyDictionary<string, string> attributes = default,
            BlockParser blockParser = default)
        {
            return new FlexiQuoteBlock(blockName, icon, citeLink, attributes, blockParser);
        }
    }
}
