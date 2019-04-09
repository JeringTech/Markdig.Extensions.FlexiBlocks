using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests
{
    public static class MarkdigTypesFactory
    {
        // BlockProcessor can't be mocked since its members aren't virtual. Markdig does not apply IOC conventions either, so there is not interface to mock.
        public static InlineProcessor CreateInlineProcessor(
            StringBuilderCache stringBuilderCache = null,
            MarkdownDocument markdownDocument = null,
            InlineParserList inlineParsers = null,
            bool preciseSourceLocation = false)
        {
            return new InlineProcessor(
                stringBuilderCache ?? new StringBuilderCache(),
                markdownDocument ?? new MarkdownDocument(),
                inlineParsers ?? new InlineParserList(new InlineParser[0]),
                preciseSourceLocation,
                null);
        }

        // BlockProcessor can't be mocked since its members aren't virtual. Markdig does not apply IOC conventions either, so there is not interface to mock.
        public static BlockProcessor CreateBlockProcessor(
            StringBuilderCache stringBuilderCache = null,
            MarkdownDocument markdownDocument = null,
            BlockParserList blockParsers = null)
        {
            return new BlockProcessor(
                stringBuilderCache ?? new StringBuilderCache(),
                markdownDocument ?? new MarkdownDocument(),
                blockParsers ?? new BlockParserList(
                    new BlockParser[]{
                        new ThematicBreakParser(),
                        new HeadingBlockParser(),
                        new QuoteBlockParser(),
                        new ListBlockParser(),

                        new HtmlBlockParser(),
                        new FencedCodeBlockParser(),
                        new IndentedCodeBlockParser(),
                        new ParagraphBlockParser(),
                    }
                ),
                null);
        }
    }
}
