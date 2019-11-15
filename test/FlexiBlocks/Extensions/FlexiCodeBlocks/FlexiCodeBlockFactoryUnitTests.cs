using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;
namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiCodeBlocks
{
    public class FlexiCodeBlockFactoryUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfOptionsServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiCodeBlockFactory(null));
        }

        [Fact]
        public void CreateProxyFencedBlock_ThrowsArgumentNullExceptionIfBlockProcessorIsNull()
        {
            // Arrange
            Mock<BlockParser> dummyBlockParser = _mockRepository.Create<BlockParser>();
            FlexiCodeBlockFactory testSubject = CreateFlexiCodeBlockFactory();

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => testSubject.CreateProxyFencedBlock(0, 0, null, dummyBlockParser.Object));
        }

        [Fact]
        public void CreateProxyFencedBlock_CreatesProxyFencedBlock()
        {
            // Arrange
            const int dummyOpeningFenceIndent = 5;
            const int dummyOpeningFenceCharCount = 6;
            const int dummyColumn = 2;
            const int dummyLineStart = 7;
            const int dummyLineEnd = 99;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = dummyColumn;
            dummyBlockProcessor.Line = new StringSlice("", dummyLineStart, dummyLineEnd);
            Mock<BlockParser> dummyBlockParser = _mockRepository.Create<BlockParser>();
            FlexiCodeBlockFactory testSubject = CreateFlexiCodeBlockFactory();

            // Act
            ProxyFencedLeafBlock result = testSubject.CreateProxyFencedBlock(dummyOpeningFenceIndent,
                dummyOpeningFenceCharCount,
                dummyBlockProcessor,
                dummyBlockParser.Object);

            // Assert
            Assert.Equal(dummyOpeningFenceIndent, result.OpeningFenceIndent);
            Assert.Equal(dummyOpeningFenceCharCount, result.OpeningFenceCharCount);
            Assert.Equal(dummyColumn, result.Column);
            Assert.Equal(dummyLineStart, result.Span.Start);
            Assert.Equal(dummyLineEnd, result.Span.End);
            Assert.Same(dummyBlockParser.Object, result.Parser);
            Assert.Equal(nameof(FlexiCodeBlock), result.MainTypeName);
        }

        [Fact]
        public void CreateProxyLeafBlock_ThrowsArgumentNullExceptionIfBlockProcessorIsNull()
        {
            // Arrange
            Mock<BlockParser> dummyBlockParser = _mockRepository.Create<BlockParser>();
            FlexiCodeBlockFactory testSubject = CreateFlexiCodeBlockFactory();

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => testSubject.CreateProxyLeafBlock(null, dummyBlockParser.Object));
        }

        [Fact]
        public void Create_FromProxyFencedLeafBlock_CreatesFlexiCodeBlock()
        {
            // Arrange
            var dummyProxyFencedLeafBlock = new ProxyFencedLeafBlock(0, 0, null, null);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            FlexiCodeBlock dummyFlexiCodeBlock = CreateFlexiCodeBlock();
            Mock<FlexiCodeBlockFactory> mockTestSubject = CreateMockFlexiCodeBlockFactory();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.CreateCore(dummyProxyFencedLeafBlock, dummyBlockProcessor)).Returns(dummyFlexiCodeBlock);

            // Act
            FlexiCodeBlock result = mockTestSubject.Object.Create(dummyProxyFencedLeafBlock, dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Same(dummyFlexiCodeBlock, result);
        }

        [Fact]
        public void CreateProxyLeafBlock_CreatesProxyLeafBlock()
        {
            // Arrange
            const int dummyColumn = 2;
            const int dummyLineStart = 7;
            const int dummyLineEnd = 99;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = dummyColumn;
            dummyBlockProcessor.Line = new StringSlice("", dummyLineStart, dummyLineEnd);
            Mock<BlockParser> dummyBlockParser = _mockRepository.Create<BlockParser>();
            FlexiCodeBlockFactory testSubject = CreateFlexiCodeBlockFactory();

            // Act
            ProxyLeafBlock result = testSubject.CreateProxyLeafBlock(dummyBlockProcessor, dummyBlockParser.Object);

            // Assert
            Assert.Equal(dummyColumn, result.Column);
            Assert.Equal(dummyLineStart, result.Span.Start);
            Assert.Equal(dummyLineEnd, result.Span.End);
            Assert.Same(dummyBlockParser.Object, result.Parser);
            Assert.Equal(nameof(FlexiCodeBlock), result.MainTypeName);
        }

        [Fact]
        public void Create_FromProxyLeafBlock_CreatesFlexiCodeBlock()
        {
            // Arrange
            var dummyProxyLeafBlock = new ProxyLeafBlock(null, null);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            FlexiCodeBlock dummyFlexiCodeBlock = CreateFlexiCodeBlock();
            Mock<FlexiCodeBlockFactory> mockTestSubject = CreateMockFlexiCodeBlockFactory();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.CreateCore(dummyProxyLeafBlock, dummyBlockProcessor)).Returns(dummyFlexiCodeBlock);

            // Act
            FlexiCodeBlock result = mockTestSubject.Object.Create(dummyProxyLeafBlock, dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Same(dummyFlexiCodeBlock, result);
        }

        [Fact]
        public void CreateCore_ThrowsArgumentNullExceptionIfProxyLeafBlockIsNull()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            FlexiCodeBlockFactory testSubject = CreateFlexiCodeBlockFactory();

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => testSubject.CreateCore(null, dummyBlockProcessor));
        }

        [Fact]
        public void CreateCore_CreatesFlexiCodeBlock()
        {
            // Arrange
            const int dummyLine = 6;
            const int dummyColumn = 2;
            var dummySpan = new SourceSpan(5, 10);
            const string dummyBlockName = "dummyBlockName";
            const string dummyTitle = "dummyTitle";
            const string dummyCopyIcon = "dummyCopyIcon";
            const bool dummyRenderHeader = false;
            const string dummyLanguage = "dummyLanguage";
            var dummyLines = new StringLineGroup(2);
            dummyLines.Add(new StringSlice("dummy"));
            dummyLines.Add(new StringSlice("line"));
            const SyntaxHighlighter dummySyntaxHighlighter = SyntaxHighlighter.HighlightJS;
            var dummyLineNumbers = new ReadOnlyCollection<NumberedLineRange>(new List<NumberedLineRange>());
            var dummyResolvedLineNumbers = new ReadOnlyCollection<NumberedLineRange>(new List<NumberedLineRange>());
            const string dummyOmittedLinesIcon = "dummyOmittedLinesIcon";
            var dummyHighlightedLines = new ReadOnlyCollection<LineRange>(new List<LineRange>());
            var dummyResolvedHighlightedLines = new ReadOnlyCollection<LineRange>(new List<LineRange>());
            var dummyHighlightedPhrases = new ReadOnlyCollection<PhraseGroup>(new List<PhraseGroup>());
            var dummyResolvedHighlightedPhrases = new ReadOnlyCollection<Phrase>(new List<Phrase>());
            const FlexiCodeBlockRenderingMode dummyRenderingMode = FlexiCodeBlockRenderingMode.Classic;
            var dummyAttributes = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
            Mock<BlockParser> dummyBlockParser = _mockRepository.Create<BlockParser>();
            var dummyProxyLeafBlock = new ProxyLeafBlock(null, dummyBlockParser.Object);
            dummyProxyLeafBlock.Line = dummyLine;
            dummyProxyLeafBlock.Column = dummyColumn;
            dummyProxyLeafBlock.Span = dummySpan;
            dummyProxyLeafBlock.Lines = dummyLines;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<IFlexiCodeBlockOptions> mockFlexiCodeBlockOptions = _mockRepository.Create<IFlexiCodeBlockOptions>();
            mockFlexiCodeBlockOptions.Setup(f => f.BlockName).Returns(dummyBlockName);
            mockFlexiCodeBlockOptions.Setup(f => f.Title).Returns(dummyTitle);
            mockFlexiCodeBlockOptions.Setup(f => f.CopyIcon).Returns(dummyCopyIcon);
            mockFlexiCodeBlockOptions.Setup(f => f.RenderHeader).Returns(dummyRenderHeader);
            mockFlexiCodeBlockOptions.Setup(f => f.Language).Returns(dummyLanguage);
            mockFlexiCodeBlockOptions.Setup(f => f.SyntaxHighlighter).Returns(dummySyntaxHighlighter);
            mockFlexiCodeBlockOptions.Setup(f => f.LineNumbers).Returns(dummyLineNumbers);
            mockFlexiCodeBlockOptions.Setup(f => f.OmittedLinesIcon).Returns(dummyOmittedLinesIcon);
            mockFlexiCodeBlockOptions.Setup(f => f.HighlightedLines).Returns(dummyHighlightedLines);
            mockFlexiCodeBlockOptions.Setup(f => f.HighlightedPhrases).Returns(dummyHighlightedPhrases);
            mockFlexiCodeBlockOptions.Setup(f => f.RenderingMode).Returns(dummyRenderingMode);
            mockFlexiCodeBlockOptions.Setup(f => f.Attributes).Returns(dummyAttributes);
            Mock<IOptionsService<IFlexiCodeBlockOptions, IFlexiCodeBlocksExtensionOptions>> mockOptionsService = _mockRepository.
                Create<IOptionsService<IFlexiCodeBlockOptions, IFlexiCodeBlocksExtensionOptions>>();
            mockOptionsService.Setup(f => f.CreateOptions(dummyBlockProcessor)).
                Returns((mockFlexiCodeBlockOptions.Object, null));
            Mock<FlexiCodeBlockFactory> mockTestSubject = CreateMockFlexiCodeBlockFactory(mockOptionsService.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.ResolveBlockName(dummyBlockName)).Returns(dummyBlockName);
            mockTestSubject.Setup(t => t.ValidateSyntaxHighlighter(dummySyntaxHighlighter));
            mockTestSubject.Setup(t => t.TryCreateSortedLineRanges(dummyLineNumbers, dummyLines.Count)).Returns(dummyResolvedLineNumbers);
            mockTestSubject.Setup(t => t.ValidateSortedLineNumbers(It.Is<ReadOnlyCollection<NumberedLineRange>>(lineNumbers => lineNumbers == dummyResolvedLineNumbers), dummyLines.Count));
            mockTestSubject.Setup(t => t.TryCreateSortedLineRanges(dummyHighlightedLines, dummyLines.Count)).Returns(dummyResolvedHighlightedLines);
            mockTestSubject.
                Setup(t => t.ResolveHighlightedPhrases(dummyLines.ToString(), It.Is<ReadOnlyCollection<PhraseGroup>>(highlightedPhrases => highlightedPhrases == dummyHighlightedPhrases)))
                .Returns(dummyResolvedHighlightedPhrases);
            mockTestSubject.Setup(t => t.ValidateRenderingMode(dummyRenderingMode));

            // Act
            FlexiCodeBlock result = mockTestSubject.Object.CreateCore(dummyProxyLeafBlock, dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyBlockName, result.BlockName);
            Assert.Equal(dummyTitle, result.Title);
            Assert.Equal(dummyCopyIcon, result.CopyIcon);
            Assert.Equal(dummyRenderHeader, result.RenderHeader);
            Assert.Equal(dummyLanguage, result.Language);
            Assert.Equal(dummyLines.ToString(), result.Code);
            Assert.Equal(dummyLines.Count, result.CodeNumLines);
            Assert.Equal(dummySyntaxHighlighter, result.SyntaxHighlighter);
            Assert.Same(dummyResolvedLineNumbers, result.LineNumbers);
            Assert.Same(dummyOmittedLinesIcon, result.OmittedLinesIcon);
            Assert.Same(dummyResolvedHighlightedLines, result.HighlightedLines);
            Assert.Same(dummyResolvedHighlightedPhrases, result.HighlightedPhrases);
            Assert.Equal(dummyRenderingMode, result.RenderingMode);
            Assert.Same(dummyAttributes, result.Attributes);
            Assert.Same(dummyBlockParser.Object, result.Parser);
            Assert.Equal(dummyLine, result.Line);
            Assert.Equal(dummyColumn, result.Column);
            Assert.Equal(dummySpan, result.Span);
        }

        [Theory]
        [MemberData(nameof(ResolveBlockName_ResolvesBlockName_Data))]
        public void ResolveBlockName_ResolvesBlockName(string dummyBlockName, string expectedResult)
        {
            // Arrange
            FlexiCodeBlockFactory testSubject = CreateFlexiCodeBlockFactory();

            // Act
            string result = testSubject.ResolveBlockName(dummyBlockName);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ResolveBlockName_ResolvesBlockName_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string defaultBlockName = "flexi-code";

            return new object[][]
            {
                new object[]{dummyBlockName, dummyBlockName},
                new object[]{null, defaultBlockName},
                new object[]{" ", defaultBlockName},
                new object[]{string.Empty, defaultBlockName}
            };
        }

        [Fact]
        public void ValidateSyntaxHighlighter_ThrowsOptionsExceptionIfSyntaxHighlighterIsInvalid()
        {
            // Arrange
            FlexiCodeBlockFactory testSubject = CreateFlexiCodeBlockFactory();
            const SyntaxHighlighter dummySyntaxHighlighter = (SyntaxHighlighter)9;

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => testSubject.ValidateSyntaxHighlighter(dummySyntaxHighlighter));
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                            nameof(IFlexiCodeBlockOptions.SyntaxHighlighter),
                            string.Format(Strings.OptionsException_Shared_ValueMustBeAValidEnumValue, dummySyntaxHighlighter,
                                nameof(SyntaxHighlighter))),
                        result.Message);
        }

        [Theory]
        [MemberData(nameof(TryCreateSortedLineRanges_ReturnsNullIfLineRangesIsNullOrEmpty_Data))]
        public void TryCreateSortedLineRanges_ReturnsNullIfLineRangesIsNullOrEmpty(ReadOnlyCollection<LineRange> dummyLineRanges)
        {
            // Arrange
            FlexiCodeBlockFactory testSubject = CreateFlexiCodeBlockFactory();

            // Act
            ReadOnlyCollection<LineRange> result = testSubject.TryCreateSortedLineRanges(dummyLineRanges, 0);

            // Assert
            Assert.Null(result);
        }

        public static IEnumerable<object[]> TryCreateSortedLineRanges_ReturnsNullIfLineRangesIsNullOrEmpty_Data()
        {
            return new object[][]
            {
                // null
                new object[]{null},
                // Empty
                new object[]{ new ReadOnlyCollection<LineRange>(new List<LineRange>()) }
            };
        }

        // Sorting logic is tested in LineRangeComparerUnitTests, here we're just ensuring that if LineRanges is in order, it is returned.
        [Fact]
        public void TryCreateSortedLineRanges_ReturnsLineRangesIfItIsInOrder()
        {
            // Arrange
            var dummyLineRanges = new ReadOnlyCollection<LineRange>(new List<LineRange>()
            {
                new LineRange(endLine: 9),
                new LineRange(startLine: 8, endLine: 10),
                new LineRange(startLine: 12, endLine: 15)
            });
            FlexiCodeBlockFactory testSubject = CreateFlexiCodeBlockFactory();

            // Act
            ReadOnlyCollection<LineRange> result = testSubject.TryCreateSortedLineRanges(dummyLineRanges, 100);

            // Assert
            Assert.Same(dummyLineRanges, result);
        }

        // Sorting logic is tested in LineRangeComparerUnitTests, here we're just ensuring that if LineRanges is NOT in order, a sorted collection is returned.
        [Fact]
        public void TryCreateSortedLineRanges_ReturnsSortedCollectionIfLineRangesIsNotInOrder()
        {
            // Arrange
            var dummyLineRange1 = new LineRange(endLine: 9);
            var dummyLineRange2 = new LineRange(startLine: 8, endLine: 10);
            var dummyLineRange3 = new LineRange(startLine: 12, endLine: 15);
            var dummyLineRanges = new ReadOnlyCollection<LineRange>(new List<LineRange>()
            {
                dummyLineRange2,
                dummyLineRange3,
                dummyLineRange1
            });
            FlexiCodeBlockFactory testSubject = CreateFlexiCodeBlockFactory();

            // Act
            ReadOnlyCollection<LineRange> result = testSubject.TryCreateSortedLineRanges(dummyLineRanges, 100);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Same(dummyLineRange1, result[0]);
            Assert.Same(dummyLineRange2, result[1]);
            Assert.Same(dummyLineRange3, result[2]);
        }

        [Theory]
        [MemberData(nameof(ValidateSortedLineNumbers_ThrowsOptionsExceptionIfNumberedLineRangesOverlap_Data))]
        public void ValidateSortedLineNumbers_ThrowsOptionsExceptionIfNumberedLineRangesOverlap(List<NumberedLineRange> dummyLineNumbers,
            string expectedExceptionMessage)
        {
            // Arrange
            FlexiCodeBlockFactory testSubject = CreateFlexiCodeBlockFactory();

            // Act
            OptionsException result = Assert.
                Throws<OptionsException>(() => testSubject.ValidateSortedLineNumbers(new ReadOnlyCollection<NumberedLineRange>(dummyLineNumbers), 100));

            // Assert
            Assert.Equal(expectedExceptionMessage, result.Message);
        }

        public static IEnumerable<object[]> ValidateSortedLineNumbers_ThrowsOptionsExceptionIfNumberedLineRangesOverlap_Data()
        {
            var dummyNumberedLineRange1 = new NumberedLineRange(11, 16, 15);
            var dummyNumberedLineRange2 = new NumberedLineRange(16, 20, 20);
            var dummyNumberedLineRange3 = new NumberedLineRange(14, 20, 11);
            var dummyNumberedLineRange4 = new NumberedLineRange(22, 56, 17);

            return new object[][]
            {
                // Start and end lines can't overlap
                new object[]{
                    new List<NumberedLineRange>()
                    {
                        new NumberedLineRange(1, 10, 1),
                        dummyNumberedLineRange1, // Doesn't overlap with previous range
                        dummyNumberedLineRange2 // Overlaps previous range by 1 line
                    },
                    string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                        nameof(IFlexiCodeBlockOptions.LineNumbers),
                        string.Format(Strings.OptionsException_FlexiCodeBlocks_OverlappingLineNumbers, dummyNumberedLineRange1, dummyNumberedLineRange2))
                },
                // Numbers can't overlap
                new object[]{
                    new List<NumberedLineRange>()
                    {
                        new NumberedLineRange(1, 10, 1),
                        dummyNumberedLineRange3, // Doesn't overlap with previous range
                        dummyNumberedLineRange4 // Overlaps previous range by 1 line
                    },
                    string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                        nameof(IFlexiCodeBlockOptions.LineNumbers),
                        string.Format(Strings.OptionsException_FlexiCodeBlocks_OverlappingLineNumbers, dummyNumberedLineRange3, dummyNumberedLineRange4))
                }
            };
        }

        [Theory]
        [MemberData(nameof(ResolveHighlightedPhrases_ResolvesHighlightedPhrases_Data))]
        public void ResolveHighlightedPhrases_ResolvesHighlightedPhrases(string dummyCode,
            ReadOnlyCollection<PhraseGroup> dummyHighlightedPhrases,
            ReadOnlyCollection<Phrase> expectedPhrases)
        {
            // Arrange
            FlexiCodeBlockFactory testSubject = CreateFlexiCodeBlockFactory();

            // Act
            ReadOnlyCollection<Phrase> result = testSubject.ResolveHighlightedPhrases(dummyCode, dummyHighlightedPhrases);

            // Assert
            Assert.Equal(expectedPhrases, result);
        }

        public static IEnumerable<object[]> ResolveHighlightedPhrases_ResolvesHighlightedPhrases_Data()
        {
            return new object[][]
            {
                // highlightedPhrases is null
                new object[]{
                    "dummyCode",
                    null,
                    null
                },
                // highlightedPhrases is empty
                new object[]{
                    "dummyCode",
                    new ReadOnlyCollection<PhraseGroup>(new List<PhraseGroup>()),
                    null
                },
                // Code is an empty string
                new object[]{
                    string.Empty,
                    new ReadOnlyCollection<PhraseGroup>(new List<PhraseGroup>() { new PhraseGroup(string.Empty, null) }),
                    null
                },
                // No regex matches
                new object[]{"1",
                    new ReadOnlyCollection<PhraseGroup>(new List<PhraseGroup>() { new PhraseGroup("2", null) }),
                    null
                },
                // Results are sorted (Sorting logic is tested in PhraseUnitTests, here we're just verifying that phrases are sorted)
                new object[]{
                    "121212",
                    new ReadOnlyCollection<PhraseGroup>(new List<PhraseGroup>() { new PhraseGroup("1", new int[] { -1, -3, -2 }), new PhraseGroup("2", new int[]{2, 0, 1 })}),
                    new ReadOnlyCollection<Phrase>(new List<Phrase>() {
                        new Phrase(0, 0),
                        new Phrase(1, 1),
                        new Phrase(2, 2),
                        new Phrase(3, 3),
                        new Phrase(4, 4),
                        new Phrase(5, 5)}
                    )
                }
            };
        }

        [Fact]
        public void ValidateRenderingMode_ThrowsOptionsExceptionIfRenderingModeIsInvalid()
        {
            // Arrange
            FlexiCodeBlockFactory testSubject = CreateFlexiCodeBlockFactory();
            const FlexiCodeBlockRenderingMode dummyRenderingMode = (FlexiCodeBlockRenderingMode)9;

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => testSubject.ValidateRenderingMode(dummyRenderingMode));
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                            nameof(IFlexiCodeBlockOptions.RenderingMode),
                            string.Format(Strings.OptionsException_Shared_ValueMustBeAValidEnumValue,
                            dummyRenderingMode,
                            nameof(FlexiCodeBlockRenderingMode))),
                        result.Message);
        }

        private Mock<FlexiCodeBlockFactory> CreateMockFlexiCodeBlockFactory(
            IOptionsService<IFlexiCodeBlockOptions, IFlexiCodeBlocksExtensionOptions> optionsService = null)
        {
            return _mockRepository.Create<FlexiCodeBlockFactory>(optionsService ??
                _mockRepository.Create<IOptionsService<IFlexiCodeBlockOptions, IFlexiCodeBlocksExtensionOptions>>().Object);
        }

        private FlexiCodeBlockFactory CreateFlexiCodeBlockFactory(
            IOptionsService<IFlexiCodeBlockOptions, IFlexiCodeBlocksExtensionOptions> optionsService = null)
        {
            return new FlexiCodeBlockFactory(optionsService ??
                _mockRepository.Create<IOptionsService<IFlexiCodeBlockOptions, IFlexiCodeBlocksExtensionOptions>>().Object);
        }

        private FlexiCodeBlock CreateFlexiCodeBlock(string blockName = default,
            string title = default,
            string copyIcon = default,
            bool renderHeader = true,
            string language = default,
            string code = default,
            int codeNumLines = default,
            SyntaxHighlighter syntaxHighlighter = default,
            ReadOnlyCollection<NumberedLineRange> lineNumbers = default,
            string omittedLinesIcon = default,
            ReadOnlyCollection<LineRange> highlightedLines = default,
            ReadOnlyCollection<Phrase> highlightedPhrases = default,
            FlexiCodeBlockRenderingMode renderingMode = default,
            ReadOnlyDictionary<string, string> attributes = default,
            BlockParser blockParser = default)
        {
            return new FlexiCodeBlock(blockName, title, copyIcon, renderHeader, language, code, codeNumLines, syntaxHighlighter, lineNumbers,
                omittedLinesIcon, highlightedLines, highlightedPhrases, renderingMode, attributes, blockParser);
        }
    }
}
