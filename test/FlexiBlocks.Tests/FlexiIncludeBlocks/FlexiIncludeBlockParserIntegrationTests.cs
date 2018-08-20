using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiIncludeBlocks
{
    public class FlexiIncludeBlockParserIntegrationTests
    {
        [Fact]
        public void ReplaceFlexIncludeBlock_WrapsContentInACodeBlockIfIncludeOptionsContentTypeIsCode()
        {
            // Arrange
            var dummyContent = new ReadOnlyCollection<string>(new string[] { "dummy", "content" });
            BlockProcessor dummyBlockProcessor = CreateBlockProcessor();
            var dummyFlexiIncludeBlock = new FlexiIncludeBlock(null);
            dummyBlockProcessor.Document.Add(dummyFlexiIncludeBlock); // Set document as parent of flexi include block
            var dummyIncludeOptions = new IncludeOptions("dummySource"); // Default content type is Code
            FlexiIncludeBlockParser testSubject = CreateFlexiIncludBlockParser();

            // Act
            testSubject.ReplaceFlexiIncludeBlock(dummyBlockProcessor, dummyFlexiIncludeBlock, dummyContent, dummyIncludeOptions);

            // Assert
            Assert.Single(dummyBlockProcessor.Document);
            var resultCodeBlock = dummyBlockProcessor.Document[0] as FencedCodeBlock;
            Assert.NotNull(resultCodeBlock);
            Assert.Equal(string.Join("\n", dummyContent), resultCodeBlock.Lines.ToString());
        }

        [Fact]
        public void ReplaceFlexIncludeBlock_AddsBeforeAndAfterTextIfTheyAreNotNull()
        {

        }

        [Fact]
        public void ReplaceFlexIncludeBlock_ThrowsInvalidOperationExceptionIfNoLineContainsStartLineSubStringOfAClippingArea()
        {
            // Arrange
            var dummyContent = new ReadOnlyCollection<string>(new string[]{ "dummy", "content"});
            const string dummyStartLineSubstring = "dummyStartLineSubstring";
            BlockProcessor dummyBlockProcessor = CreateBlockProcessor();
            var dummyFlexiIncludeBlock = new FlexiIncludeBlock(null);
            var dummyClippingArea = new ClippingArea(0, -1, dummyStartLineSubstring);
            var dummyIncludeOptions = new IncludeOptions("dummySource", clippingAreas: new List<ClippingArea> { dummyClippingArea });
            FlexiIncludeBlockParser testSubject = CreateFlexiIncludBlockParser();

            // Act and assert
            InvalidOperationException result = Assert.Throws<InvalidOperationException>(() => testSubject.ReplaceFlexiIncludeBlock(dummyBlockProcessor, dummyFlexiIncludeBlock, dummyContent, dummyIncludeOptions));
            Assert.Equal(string.Format(Strings.InvalidOperationException_InvalidClippingAreaNoLineContainsStartLineSubstring, dummyStartLineSubstring),
                result.Message);
        }

        [Fact]
        public void ReplaceFlexIncludeBlock_ThrowsInvalidOperationExceptionIfNoLineContainsEndLineSubStringOfAClippingArea()
        {
            var dummyContent = new ReadOnlyCollection<string>(new string[] { "dummy", "content" });
            const string dummyEndLineSubstring = "dummyEndLineSubstring";
            BlockProcessor dummyBlockProcessor = CreateBlockProcessor();
            var dummyFlexiIncludeBlock = new FlexiIncludeBlock(null);
            var dummyClippingArea = new ClippingArea(1, 0, endLineSubstring: dummyEndLineSubstring);
            var dummyIncludeOptions = new IncludeOptions("dummySource", clippingAreas: new List<ClippingArea> { dummyClippingArea });
            FlexiIncludeBlockParser testSubject = CreateFlexiIncludBlockParser();

            // Act and assert
            InvalidOperationException result = Assert.Throws<InvalidOperationException>(() => testSubject.ReplaceFlexiIncludeBlock(dummyBlockProcessor, dummyFlexiIncludeBlock, dummyContent, dummyIncludeOptions));
            Assert.Equal(string.Format(Strings.InvalidOperationException_InvalidClippingAreaNoLineContainsEndLineSubstring, dummyEndLineSubstring),
                result.Message);
        }

        [Fact]
        public void ReplaceFlexiIncludeBlock_ClipsLinesAccordingToStartAndEndLineNumbers()
        {

        }

        [Fact]
        public void ReplaceFlexiIncludeBlock_ClipsLinesAccordingToStartAndEndLineSubstrings()
        {

        }

        private FlexiIncludeBlockParser CreateFlexiIncludBlockParser(IOptions<FlexiIncludeBlocksExtensionOptions> extensionOptionsAccessor = null,
            IContentRetrievalService contentRetrievalService = null)
        {
            return new FlexiIncludeBlockParser(extensionOptionsAccessor, contentRetrievalService);
        }

        /// <summary>
        /// Create a default BlockProcessor. Markdig's defaults are specified in MarkdownPipelineBuilder, but aren't accessible through it.
        /// </summary>
        private BlockProcessor CreateBlockProcessor()
        {
            var stringBuilders = new StringBuilderCache();

            var parsers = new OrderedList<BlockParser>()
            {
                new ThematicBreakParser(),
                new HeadingBlockParser(),
                new QuoteBlockParser(),
                new ListBlockParser(),

                new HtmlBlockParser(),
                new FencedCodeBlockParser(),
                new IndentedCodeBlockParser(),
                new ParagraphBlockParser(),
            };

            var markdownDocument = new MarkdownDocument();

            return new BlockProcessor(stringBuilders, markdownDocument, new BlockParserList(parsers));
        }
    }
}
