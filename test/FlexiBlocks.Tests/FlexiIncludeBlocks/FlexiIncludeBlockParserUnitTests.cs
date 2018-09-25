using Jering.IocServices.Newtonsoft.Json;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiIncludeBlocks
{
    // Refer to FlexiIncludeBlocksIntegrationTests for thorough testing of Cycle detection
    public class FlexiIncludeBlockParserUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void TryOpenFlexiBlock_ReturnsBlockStateNoneIfInCodeIndent()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            // These three lines just set IsCodeIndent to true
            dummyBlockProcessor.Column = 0;
            dummyBlockProcessor.RestartIndent();
            dummyBlockProcessor.Column = 4;
            ExposedFlexiIncludeBlockParser testSubject = CreateExposedFlexiIncludBlockParser();

            // Act
            BlockState result = testSubject.ExposedTryOpenFlexiBlock(dummyBlockProcessor);

            // Assert
            Assert.True(dummyBlockProcessor.IsCodeIndent);
            Assert.Equal(BlockState.None, result);
        }

        [Theory]
        [MemberData(nameof(TryOpenFlexiBlock_ReturnsBlockStateNoneIfLineDoesNotBeginWithExpectedCharacters_Data))]
        public void TryOpenFlexiBlock_ReturnsBlockStateNoneIfLineDoesNotBeginWithExpectedCharacters(string line)
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(line);
            ExposedFlexiIncludeBlockParser testSubject = CreateExposedFlexiIncludBlockParser();

            // Act
            BlockState result = testSubject.ExposedTryOpenFlexiBlock(dummyBlockProcessor);

            // Assert
            Assert.Equal(BlockState.None, result);
        }

        public static IEnumerable<object[]> TryOpenFlexiBlock_ReturnsBlockStateNoneIfLineDoesNotBeginWithExpectedCharacters_Data()
        {
            return new object[][]
            {
                    // Character after + must be an open brace
                    new string[]{ "+a" },
                    // No whitespace allowed between + and {
                    new string[]{"+ {"},
                    new string[]{"+\n{"}
            };
        }

        [Fact]
        public void TryOpenFlexiBlock_CreatesFlexiIncludeBlockIfSuccessful()
        {
            // Arrange
            const int dummyColumn = 1;
            var dummyStringSlice = new StringSlice("+{dummy");
            var dummyParentFlexiIncludeBlock = new FlexiIncludeBlock(null, null);
            var dummyClosingFlexiIncludeBlocks = new Stack<FlexiIncludeBlock>();
            dummyClosingFlexiIncludeBlocks.Push(dummyParentFlexiIncludeBlock);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = dummyColumn;
            dummyBlockProcessor.Line = dummyStringSlice;
            dummyBlockProcessor.Document.SetData(FlexiIncludeBlockParser.CLOSING_FLEXI_INCLUDE_BLOCKS_KEY, dummyClosingFlexiIncludeBlocks);
            ExposedFlexiIncludeBlockParser testSubject = CreateExposedFlexiIncludBlockParser();

            // Act
            BlockState result = testSubject.ExposedTryOpenFlexiBlock(dummyBlockProcessor);

            // Assert
            Assert.Equal(1, dummyBlockProcessor.Start); // Advances past +
            Assert.Equal(BlockState.Continue, result);
            Assert.Single(dummyBlockProcessor.NewBlocks);
            var newBlock = dummyBlockProcessor.NewBlocks.Peek() as FlexiIncludeBlock;
            Assert.NotNull(newBlock);
            Assert.Equal(dummyColumn, newBlock.Column);
            Assert.Equal(0, newBlock.Span.Start); // Includes +
            Assert.Same(dummyParentFlexiIncludeBlock, newBlock.ParentFlexiIncludeBlock);
        }

        [Fact]
        public void CloseFlexiBlock_ChecksForCycleIfIncludeTypeIsMarkdown()
        {
            // Arrange
            var dummyFlexiIncludeBlockOptions = new FlexiIncludeBlockOptions("C:/dummy", type: IncludeType.Markdown);
            var dummyFlexiIncludeBlock = new FlexiIncludeBlock(null, null);
            dummyFlexiIncludeBlock.Setup(dummyFlexiIncludeBlockOptions, null); // Sets absolute URI
            var dummyClosingFlexiIncludeBlocks = new Stack<FlexiIncludeBlock>(new FlexiIncludeBlock[] { dummyFlexiIncludeBlock });
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummySource = new ReadOnlyCollection<string>(new List<string>());
            Mock<ISourceRetrieverService> mockSourceRetrieverService = _mockRepository.Create<ISourceRetrieverService>();
            mockSourceRetrieverService.
                Setup(s => s.GetSource(dummyFlexiIncludeBlock.AbsoluteSourceUri, dummyFlexiIncludeBlockOptions.ResolvedDiskCacheDirectory, default)).
                Returns(dummySource);
            Mock<ExposedFlexiIncludeBlockParser> mockTestSubject = CreateMockExposedFlexiIncludeBlockParser(sourceRetrieverService: mockSourceRetrieverService.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.SetupFlexiIncludeBlock(dummyFlexiIncludeBlock));
            mockTestSubject.Setup(t => t.GetOrCreateClosingFlexiIncludeBlocks(dummyBlockProcessor)).Returns(dummyClosingFlexiIncludeBlocks);
            mockTestSubject.Setup(t => t.CheckForCycle(dummyClosingFlexiIncludeBlocks, dummyFlexiIncludeBlock));
            mockTestSubject.Setup(t => t.ReplaceFlexiIncludeBlock(dummyBlockProcessor, dummyFlexiIncludeBlock, dummySource));

            // Act
            bool result = mockTestSubject.Object.ExposedCloseFlexiBlock(dummyBlockProcessor, dummyFlexiIncludeBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.False(result);
            Assert.Empty(dummyClosingFlexiIncludeBlocks); // Removed once the block is replaced
        }

        [Theory]
        [MemberData(nameof(CloseFlexiBlock_WrapsExceptionsForCompleteContext_Data))]
        public void CloseFlexiBlock_WrapsExceptionsForCompleteContext(FlexiIncludeBlock dummyFlexiIncludeBlock,
            string expectedResultMessage)
        {
            // Arrange
            var dummyFlexiIncludeBlockOptions = new FlexiIncludeBlockOptions();
            dummyFlexiIncludeBlock.FlexiIncludeBlockOptions = dummyFlexiIncludeBlockOptions;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummySource = new ReadOnlyCollection<string>(new List<string>());
            Mock<ISourceRetrieverService> mockSourceRetrieverService = _mockRepository.Create<ISourceRetrieverService>();
            mockSourceRetrieverService.
                Setup(s => s.GetSource(dummyFlexiIncludeBlock.AbsoluteSourceUri, dummyFlexiIncludeBlockOptions.ResolvedDiskCacheDirectory, default)).
                Returns(dummySource);
            var dummyException = new FlexiBlocksException();
            Mock<ExposedFlexiIncludeBlockParser> mockTestSubject = CreateMockExposedFlexiIncludeBlockParser(sourceRetrieverService: mockSourceRetrieverService.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.SetupFlexiIncludeBlock(dummyFlexiIncludeBlock));
            mockTestSubject.Setup(t => t.ReplaceFlexiIncludeBlock(dummyBlockProcessor, dummyFlexiIncludeBlock, dummySource)).Throws(dummyException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.ExposedCloseFlexiBlock(dummyBlockProcessor, dummyFlexiIncludeBlock));
            _mockRepository.VerifyAll();
            Assert.Equal(expectedResultMessage, result.Message);
            Assert.Same(dummyException, result.InnerException);
        }

        public static IEnumerable<object[]> CloseFlexiBlock_WrapsExceptionsForCompleteContext_Data()
        {
            var dummyAbsoluteSourceUri = new Uri("C:/dummy");
            var dummyLines = new StringLineGroup(2)
            {
                new StringSlice("dummyLine1"),
                new StringSlice("dummyLine2")
            };
            const string dummyContainingSource = "dummyContainingSource";
            const int dummyLineNumberInContainingSource = 5;

            return new object[][]
            {
                new object[]
                {
                    new FlexiIncludeBlock(null, null)
                    {
                        ClippingProcessingStage = ClippingProcessingStage.None,
                        AbsoluteSourceUri = dummyAbsoluteSourceUri
                    },
                    string.Format(Strings.FlexiBlocksException_InvalidFlexiBlock, nameof(FlexiIncludeBlock), 1, 0, Strings.FlexiBlocksException_FlexiIncludeBlocks_ExceptionOccurredWhileProcessingBlock)
                },
                new object[]{
                    new FlexiIncludeBlock(null, null)
                    {
                        ContainingSourceUri = dummyContainingSource,
                        LineNumberInContainingSource = dummyLineNumberInContainingSource,
                        AbsoluteSourceUri = dummyAbsoluteSourceUri,
                        ClippingProcessingStage = ClippingProcessingStage.Source,
                        Lines = dummyLines
                    },
                    string.Format(Strings.FlexiBlocksException_InvalidFlexiBlock, nameof(FlexiIncludeBlock), dummyLineNumberInContainingSource, 0,
                        string.Format(Strings.FlexiBlocksException_FlexiIncludeBlocks_ExceptionOccurredWhileProcessingSource, dummyAbsoluteSourceUri.AbsoluteUri))
                },
                new object[]{
                    new FlexiIncludeBlock(null, null)
                    {
                        ClippingProcessingStage = ClippingProcessingStage.BeforeContent,
                        ContainingSourceUri = dummyContainingSource,
                        LineNumberInContainingSource = dummyLineNumberInContainingSource,
                        Lines = dummyLines
                    },
                    string.Format(Strings.FlexiBlocksException_InvalidFlexiBlock, nameof(FlexiIncludeBlock), dummyLineNumberInContainingSource, 0,
                        string.Format(Strings.FlexiBlocksException_FlexiIncludeBlocks_ExceptionOccurredWhileProcessingContent, nameof(ClippingProcessingStage.BeforeContent)))
                },
                new object[]{
                    new FlexiIncludeBlock(null, null)
                    {
                        ClippingProcessingStage = ClippingProcessingStage.AfterContent,
                        ContainingSourceUri = dummyContainingSource,
                        LineNumberInContainingSource = dummyLineNumberInContainingSource,
                        Lines = dummyLines
                    },
                    string.Format(Strings.FlexiBlocksException_InvalidFlexiBlock, nameof(FlexiIncludeBlock), dummyLineNumberInContainingSource, 0,
                        string.Format(Strings.FlexiBlocksException_FlexiIncludeBlocks_ExceptionOccurredWhileProcessingContent, nameof(ClippingProcessingStage.AfterContent)))
                },
            };
        }

        [Fact]
        public void GetOrCreateStack_GetsFlexiIncludeBlockStackIfOneAlreadyExists()
        {
            // Arrange
            var dummyFlexiIncludeBlockStack = new Stack<FlexiIncludeBlock>();
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Document.SetData(FlexiIncludeBlockParser.CLOSING_FLEXI_INCLUDE_BLOCKS_KEY, dummyFlexiIncludeBlockStack);
            FlexiIncludeBlockParser testSubject = CreateExposedFlexiIncludBlockParser();

            // Act
            Stack<FlexiIncludeBlock> result = testSubject.GetOrCreateClosingFlexiIncludeBlocks(dummyBlockProcessor);

            // Assert
            Assert.Same(dummyFlexiIncludeBlockStack, result);
        }

        [Fact]
        public void GetOrCreateStack_CreatesFlexiIncludeBlockStackIfOneDoesntAlreadyExist()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            FlexiIncludeBlockParser testSubject = CreateExposedFlexiIncludBlockParser();

            // Act
            Stack<FlexiIncludeBlock> result = testSubject.GetOrCreateClosingFlexiIncludeBlocks(dummyBlockProcessor);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void SetupFlexiIncludeBlock_ThrowsFlexiBlocksExceptionIfJsonCannotBeDeserialized()
        {
            // Arrange
            const string dummyJson = "dummyJson";
            var dummyJsonException = new JsonException();
            Mock<IJsonSerializerService> mockJsonSerializerService = _mockRepository.Create<IJsonSerializerService>();
            mockJsonSerializerService.Setup(j => j.Populate(It.IsAny<JsonTextReader>(), It.IsAny<FlexiIncludeBlockOptions>())).Throws(dummyJsonException);
            var dummyFlexiIncludeBlock = new FlexiIncludeBlock(null, null) { Lines = new StringLineGroup(dummyJson) };
            FlexiIncludeBlockParser testSubject = CreateExposedFlexiIncludBlockParser(jsonSerializerService: mockJsonSerializerService.Object);

            // Act and assert
            FlexiBlocksException result = Assert.
                Throws<FlexiBlocksException>(() => testSubject.SetupFlexiIncludeBlock(dummyFlexiIncludeBlock));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.FlexiBlocksException_UnableToParseJson, dummyJson), result.Message);
            Assert.Same(dummyJsonException, result.InnerException);
        }

        [Fact]
        public void SetupFlexiIncludeBlock_ThrowsFlexiBlocksExceptionIfValidationOfPopulatedObjectFails()
        {
            // Arrange
            var dummyFlexiBlocksException = new FlexiBlocksException();
            var dummyTargetInvocationException = new TargetInvocationException(dummyFlexiBlocksException);
            Mock<IJsonSerializerService> mockJsonSerializerService = _mockRepository.Create<IJsonSerializerService>();
            mockJsonSerializerService.Setup(j => j.Populate(It.IsAny<JsonTextReader>(), It.IsAny<FlexiIncludeBlockOptions>())).Throws(dummyTargetInvocationException);
            var dummyFlexiIncludeBlock = new FlexiIncludeBlock(null, null);
            FlexiIncludeBlockParser testSubject = CreateExposedFlexiIncludBlockParser(jsonSerializerService: mockJsonSerializerService.Object);

            // Act and assert
            FlexiBlocksException result = Assert.
                Throws<FlexiBlocksException>(() => testSubject.SetupFlexiIncludeBlock(dummyFlexiIncludeBlock));
            _mockRepository.VerifyAll();
            Assert.Same(dummyFlexiBlocksException, result);
        }

        [Fact]
        public void SetupFlexiIncludeBlock_SetsUpFlexiIncludeBlock()
        {
            // Arrange
            const string dummySourceUri = "C:/dummy/source/uri";
            var dummyFlexiIncludeBlockOptions = new FlexiIncludeBlockOptions(dummySourceUri);
            Mock<IOptions<FlexiIncludeBlocksExtensionOptions>> mockOptionsAccessor = _mockRepository.Create<IOptions<FlexiIncludeBlocksExtensionOptions>>();
            mockOptionsAccessor.Setup(o => o.Value).Returns(new FlexiIncludeBlocksExtensionOptions() { DefaultBlockOptions = dummyFlexiIncludeBlockOptions });
            Mock<IJsonSerializerService> mockJsonSerializerService = _mockRepository.Create<IJsonSerializerService>();
            mockJsonSerializerService.Setup(j => j.Populate(It.IsAny<JsonTextReader>(), It.IsAny<FlexiIncludeBlockOptions>()));
            var dummyFlexiIncludeBlock = new FlexiIncludeBlock(null, null);
            FlexiIncludeBlockParser testSubject = CreateExposedFlexiIncludBlockParser(mockOptionsAccessor.Object, jsonSerializerService: mockJsonSerializerService.Object);

            // Act
            testSubject.SetupFlexiIncludeBlock(dummyFlexiIncludeBlock); // Should set FlexiIncludeBlock's absolute source URI

            // Assert
            Assert.Equal(dummySourceUri, dummyFlexiIncludeBlock.AbsoluteSourceUri.OriginalString);
        }

        [Fact]
        public void ReplaceFlexIncludeBlock_WrapsContentInACodeBlockIfIncludeTypeIsCode()
        {
            // Arrange
            var dummySource = new ReadOnlyCollection<string>(new string[] { "dummy", "source" });
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyFlexiIncludeBlock = new FlexiIncludeBlock(null, null);
            dummyBlockProcessor.Document.Add(dummyFlexiIncludeBlock); // Set document as parent of FlexiIncludeBlock
            dummyFlexiIncludeBlock.FlexiIncludeBlockOptions = new FlexiIncludeBlockOptions(); // Default content type is Code
            ExposedFlexiIncludeBlockParser testSubject = CreateExposedFlexiIncludBlockParser();

            // Act
            testSubject.ReplaceFlexiIncludeBlock(dummyBlockProcessor, dummyFlexiIncludeBlock, dummySource);

            // Assert
            Assert.Single(dummyBlockProcessor.Document);
            var resultCodeBlock = dummyBlockProcessor.Document[0] as FencedCodeBlock;
            Assert.NotNull(resultCodeBlock);
            Assert.Equal(string.Join("\n", dummySource), resultCodeBlock.Lines.ToString());
        }

        [Fact]
        public void ReplaceFlexIncludeBlock_AddsBeforeAndAfterContentIfTheyAreNotNull()
        {
            // Arrange
            var dummySource = new ReadOnlyCollection<string>(new string[] { "dummy", "source" });
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyFlexiIncludeBlock = new FlexiIncludeBlock(null, null);
            dummyBlockProcessor.Document.Add(dummyFlexiIncludeBlock); // Set document as parent of FlexiIncludeBlock
            var dummyClipping = new Clipping(beforeContent: "# dummy before", afterContent: "> dummy\n > after");
            var dummyClippings = new Clipping[] { dummyClipping };
            dummyFlexiIncludeBlock.FlexiIncludeBlockOptions = new FlexiIncludeBlockOptions("dummySource", type: IncludeType.Markdown, clippings: dummyClippings);
            ExposedFlexiIncludeBlockParser testSubject = CreateExposedFlexiIncludBlockParser();

            // Act
            testSubject.ReplaceFlexiIncludeBlock(dummyBlockProcessor, dummyFlexiIncludeBlock, dummySource);

            // Assert
            Assert.Equal(3, dummyBlockProcessor.Document.Count);
            // First block (from before text)
            var resultHeadingBlock = dummyBlockProcessor.Document[0] as HeadingBlock;
            Assert.NotNull(resultHeadingBlock);
            Assert.Equal("dummy before", resultHeadingBlock.Lines.ToString());
            // Second block (from content)
            var resultParagraphBlock1 = dummyBlockProcessor.Document[1] as ParagraphBlock;
            Assert.NotNull(resultParagraphBlock1);
            Assert.Equal(string.Join("\n", dummySource), resultParagraphBlock1.Lines.ToString());
            // Third block (from after text)
            var resultQuoteBlock = dummyBlockProcessor.Document[2] as QuoteBlock;
            Assert.NotNull(resultQuoteBlock);
            var resultParagraphBlock2 = resultQuoteBlock[0] as ParagraphBlock;
            Assert.NotNull(resultParagraphBlock2);
            Assert.Equal("dummy\nafter", resultParagraphBlock2.Lines.ToString());
        }

        [Fact]
        public void ReplaceFlexIncludeBlock_ThrowsFlexiBlocksExceptionIfNoLineContainsStartLineSubStringOfAClipping()
        {
            // Arrange
            var dummySource = new ReadOnlyCollection<string>(new string[] { "dummy", "source" });
            const string dummyStartLineSubstring = "dummyStartLineSubstring";
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyFlexiIncludeBlock = new FlexiIncludeBlock(null, null);
            var dummyMarkdownDocument = new MarkdownDocument();
            dummyMarkdownDocument.Add(dummyFlexiIncludeBlock); // ReplaceFlexiIncludeBlock uses FlexiIncludeBlock's parent
            var dummyClipping = new Clipping(startDemarcationLineSubstring: dummyStartLineSubstring);
            dummyFlexiIncludeBlock.FlexiIncludeBlockOptions = new FlexiIncludeBlockOptions("dummySource", clippings: new Clipping[] { dummyClipping });
            ExposedFlexiIncludeBlockParser testSubject = CreateExposedFlexiIncludBlockParser();

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => testSubject.ReplaceFlexiIncludeBlock(dummyBlockProcessor, dummyFlexiIncludeBlock, dummySource));
            Assert.Equal(string.Format(Strings.FlexiBlocksException_FlexiIncludeBlocks_InvalidClippingNoLineContainsStartLineSubstring, dummyStartLineSubstring),
                result.Message);
        }

        [Fact]
        public void ReplaceFlexIncludeBlock_ThrowsFlexiBlocksExceptionIfNoLineContainsEndLineSubStringOfAClipping()
        {
            var dummySource = new ReadOnlyCollection<string>(new string[] { "dummy", "source" });
            const string dummyEndLineSubstring = "dummyEndLineSubstring";
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyFlexiIncludeBlock = new FlexiIncludeBlock(null, null);
            var dummyMarkdownDocument = new MarkdownDocument();
            dummyMarkdownDocument.Add(dummyFlexiIncludeBlock); // ReplaceFlexiIncludeBlock uses FlexiIncludeBlock's parent
            var dummyClipping = new Clipping(endDemarcationLineSubstring: dummyEndLineSubstring);
            dummyFlexiIncludeBlock.FlexiIncludeBlockOptions = new FlexiIncludeBlockOptions("dummySource", clippings: new Clipping[] { dummyClipping });
            ExposedFlexiIncludeBlockParser testSubject = CreateExposedFlexiIncludBlockParser();

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => testSubject.ReplaceFlexiIncludeBlock(dummyBlockProcessor, dummyFlexiIncludeBlock, dummySource));
            Assert.Equal(string.Format(Strings.FlexiBlocksException_FlexiIncludeBlocks_InvalidClippingNoLineContainsEndLineSubstring, dummyEndLineSubstring),
                result.Message);
        }

        [Theory]
        [MemberData(nameof(ReplaceFlexiIncludeBlock_ClipsLinesAccordingToStartAndEndLineNumbersAndSubstrings_Data))]
        public void ReplaceFlexiIncludeBlock_ClipsLinesAccordingToStartAndEndLineNumbersAndSubstrings(SerializableWrapper<Clipping[]> dummyClippingsWrapper, string[] expectedResult)
        {
            // Arrange
            var dummySource = new ReadOnlyCollection<string>(new string[] { "line1", "line2", "line3", "line4", "line5" });
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyFlexiIncludeBlock = new FlexiIncludeBlock(null, null);
            dummyBlockProcessor.Document.Add(dummyFlexiIncludeBlock); // Set document as parent of FlexiIncludeBlock
            dummyFlexiIncludeBlock.FlexiIncludeBlockOptions = new FlexiIncludeBlockOptions("dummySource", type: IncludeType.Markdown, clippings: dummyClippingsWrapper.Value);
            ExposedFlexiIncludeBlockParser testSubject = CreateExposedFlexiIncludBlockParser();

            // Act
            testSubject.ReplaceFlexiIncludeBlock(dummyBlockProcessor, dummyFlexiIncludeBlock, dummySource);

            // Assert
            Assert.Single(dummyBlockProcessor.Document);
            var resultParagraphBlock = dummyBlockProcessor.Document[0] as ParagraphBlock;
            Assert.NotNull(resultParagraphBlock);
            Assert.Equal(string.Join("\n", expectedResult), resultParagraphBlock.Lines.ToString());
        }

        public static IEnumerable<object[]> ReplaceFlexiIncludeBlock_ClipsLinesAccordingToStartAndEndLineNumbersAndSubstrings_Data()
        {
            return new object[][]
            {
                    // Single clipping that includes all lines using line numbers
                    new object[]
                    {
                        new SerializableWrapper<Clipping[]>(new Clipping[] { new Clipping(endLineNumber: 5)}),
                        new string[] { "line1", "line2", "line3", "line4", "line5" }
                    },
                    // Single clipping that includes all lines using -1 as end line number
                    new object[]
                    {
                        new SerializableWrapper<Clipping[]>(new Clipping[] { new Clipping()}),
                        new string[] { "line1", "line2", "line3", "line4", "line5" }
                    },
                    // Single clipping that includes a single line using line numbers
                    new object[]
                    {
                        new SerializableWrapper<Clipping[]>(new Clipping[] { new Clipping(3, 3)}),
                        new string[] { "line3" }
                    },
                    // Single clipping that includes a single line using substrings
                    new object[]
                    {
                        new SerializableWrapper<Clipping[]>(new Clipping[] { new Clipping(startDemarcationLineSubstring: "line2", endDemarcationLineSubstring: "line4")}),
                        new string[] { "line3" }
                    },
                    // Single clipping that includes a single line using line numbers and substrings
                    new object[]
                    {
                        new SerializableWrapper<Clipping[]>(new Clipping[] { new Clipping(endLineNumber: 5, startDemarcationLineSubstring: "line4")}),
                        new string[] { "line5" }
                    },
                    // Single clipping that includes a subset of lines using line numbers
                    new object[]
                    {
                        new SerializableWrapper<Clipping[]>(new Clipping[] { new Clipping(2, 4)}),
                        new string[] { "line2", "line3", "line4" }
                    },
                    // Single clipping that includes a subset of lines using substrings
                    new object[]
                    {
                        new SerializableWrapper<Clipping[]>(new Clipping[] { new Clipping(startDemarcationLineSubstring: "line1", endDemarcationLineSubstring: "line5")}),
                        new string[] { "line2", "line3", "line4" }
                    },
                    // Single clipping that includes a subset of lines using line numbers and substrings
                    new object[]
                    {
                        new SerializableWrapper<Clipping[]>(new Clipping[] { new Clipping(2, endDemarcationLineSubstring: "line5")}),
                        new string[] { "line2", "line3", "line4" }
                    },
                    // Multiple clippings that do not overlap
                    new object[]
                    {
                        new SerializableWrapper<Clipping[]>(new Clipping[] {
                            new Clipping(endLineNumber: 2),
                            new Clipping(startDemarcationLineSubstring: "line2", endDemarcationLineSubstring: "line5"),
                            new Clipping(endLineNumber: 5, startDemarcationLineSubstring: "line4")
                        }),
                        new string[] { "line1", "line2", "line3", "line4", "line5" }
                    },
                    // Multiple clippings that overlap
                    new object[]
                    {
                        new SerializableWrapper<Clipping[]>(new Clipping[] {
                            new Clipping(endLineNumber: 3),
                            new Clipping(startDemarcationLineSubstring: "line1", endDemarcationLineSubstring: "line5"),
                            new Clipping(endLineNumber: 5, startDemarcationLineSubstring: "line3")
                        }),
                        new string[] { "line1", "line2", "line3", "line2", "line3", "line4", "line4", "line5" }
                    },
            };
        }

        public class ExposedFlexiIncludeBlockParser : FlexiIncludeBlockParser
        {
            public ExposedFlexiIncludeBlockParser(IOptions<FlexiIncludeBlocksExtensionOptions> extensionOptionsAccessor, ISourceRetrieverService sourceRetrieverService, IJsonSerializerService jsonSerializerService, ILeadingWhitespaceEditorService leadingWhitespaceEditorService) :
                base(extensionOptionsAccessor, sourceRetrieverService, jsonSerializerService, leadingWhitespaceEditorService)
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

            public bool ExposedCloseFlexiBlock(BlockProcessor processor, Block block)
            {
                return CloseFlexiBlock(processor, block);
            }
        }

        private Mock<ExposedFlexiIncludeBlockParser> CreateMockExposedFlexiIncludeBlockParser(IOptions<FlexiIncludeBlocksExtensionOptions> extensionOptionsAccessor = null,
            ISourceRetrieverService sourceRetrieverService = null,
            IJsonSerializerService jsonSerializerService = null,
            ILeadingWhitespaceEditorService leadingWhitespaceEditorService = null)
        {
            return _mockRepository.Create<ExposedFlexiIncludeBlockParser>(extensionOptionsAccessor ?? CreateExtensionOptionsAccessor(),
                sourceRetrieverService ?? _mockRepository.Create<ISourceRetrieverService>().Object,
                jsonSerializerService ?? _mockRepository.Create<IJsonSerializerService>().Object,
                leadingWhitespaceEditorService ?? _mockRepository.Create<ILeadingWhitespaceEditorService>().Object);
        }

        private ExposedFlexiIncludeBlockParser CreateExposedFlexiIncludBlockParser(IOptions<FlexiIncludeBlocksExtensionOptions> extensionOptionsAccessor = null,
            ISourceRetrieverService sourceRetrieverService = null,
            IJsonSerializerService jsonSerializerService = null,
            ILeadingWhitespaceEditorService leadingWhitespaceEditorService = null)
        {
            return new ExposedFlexiIncludeBlockParser(extensionOptionsAccessor ?? CreateExtensionOptionsAccessor(),
                sourceRetrieverService ?? _mockRepository.Create<ISourceRetrieverService>().Object,
                jsonSerializerService ?? _mockRepository.Create<IJsonSerializerService>().Object,
                leadingWhitespaceEditorService ?? _mockRepository.Create<ILeadingWhitespaceEditorService>().Object);
        }

        private IOptions<FlexiIncludeBlocksExtensionOptions> CreateExtensionOptionsAccessor()
        {
            var dummyExtensionOptions = new FlexiIncludeBlocksExtensionOptions();
            Mock<IOptions<FlexiIncludeBlocksExtensionOptions>> mockExtensionOptionsAccessor = _mockRepository.Create<IOptions<FlexiIncludeBlocksExtensionOptions>>();
            mockExtensionOptionsAccessor.Setup(e => e.Value).Returns(dummyExtensionOptions);

            return mockExtensionOptionsAccessor.Object;
        }
    }
}
