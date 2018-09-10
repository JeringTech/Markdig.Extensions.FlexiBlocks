using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Microsoft.Extensions.Options;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks
{
    /// <summary>
    /// A parser that creates <see cref="FlexiAlertBlock"/>s from markdown.
    /// </summary>
    public class FlexiAlertBlockParser : FlexiBlockParser
    {
        private readonly FlexiAlertBlocksExtensionOptions _extensionOptions;
        private readonly IFlexiOptionsBlockService _flexiOptionsBlockService;

        /// <summary>
        /// Creates a <see cref="FlexiAlertBlockParser"/> instance.
        /// </summary>
        /// <param name="extensionOptionsAccessor">The accessor for <see cref="FlexiAlertBlocksExtensionOptions"/>.</param>
        /// <param name="flexiOptionsBlockService">The service that will handle populating of <see cref="FlexiAlertBlockOptions"/>.</param>
        public FlexiAlertBlockParser(IOptions<FlexiAlertBlocksExtensionOptions> extensionOptionsAccessor,
            IFlexiOptionsBlockService flexiOptionsBlockService)
        {
            OpeningCharacters = new[] { '!' };

            _extensionOptions = extensionOptionsAccessor?.Value ?? new FlexiAlertBlocksExtensionOptions();
            _flexiOptionsBlockService = flexiOptionsBlockService;
        }

        /// <summary>
        /// Opens a <see cref="FlexiAlertBlock"/> if a line begins with 0 to 3 spaces followed by "!".
        /// </summary>
        /// <param name="processor">The block processor for the document that contains a line with first non-white-space character "!".</param>
        /// <returns>
        /// <see cref="BlockState.None"/> if current line has code indent. 
        /// <see cref="BlockState.Continue"/> if a <see cref="FlexiAlertBlock"/> is opened.
        /// </returns>
        public override BlockState TryOpenFlexiBlock(BlockProcessor processor)
        {
            if (processor.IsCodeIndent)
            {
                return BlockState.None;
            }

            // Create block
            var flexiAlertBlock = new FlexiAlertBlock(this)
            {
                Column = processor.Column,
                Span = new SourceSpan(processor.Start, processor.Line.End) // Might be the only line, we'll update End if there are more lines
            };
            processor.NewBlocks.Push(flexiAlertBlock);

            // Create options
            flexiAlertBlock.FlexiAlertBlockOptions = CreateFlexiAlertBlockOptions(processor);

            // Skip ! and first whitespace char after !
            if (processor.NextChar().IsSpaceOrTab())
            {
                processor.NextChar();
            }

            return BlockState.Continue;
        }

        /// <summary>
        /// Continues a <see cref="FlexiAlertBlock"/> if the current line begins with 0 to 3 spaces followed by "!".
        /// </summary>
        /// <param name="processor">The block processor for the <see cref="FlexiAlertBlock"/> to try and continue.</param>
        /// <param name="block">The <see cref="FlexiAlertBlock"/> to try and continue.</param>
        /// <returns>
        /// <see cref="BlockState.None"/> if the current line has code indent or if the current line does not begin with the expected characters.
        /// <see cref="BlockState.BreakDiscard"/> if the current line is blank, indicating that the <see cref="FlexiAlertBlock"/> has ended and should be closed.
        /// <see cref="BlockState.Continue"/> if the <see cref="FlexiAlertBlock"/> is still open.
        /// </returns>
        public override BlockState TryContinueFlexiBlock(BlockProcessor processor, Block block)
        {
            if (processor.IsCodeIndent)
            {
                return BlockState.None;
            }

            if (processor.CurrentChar != OpeningCharacters[0])
            {
                return processor.IsBlankLine ? BlockState.BreakDiscard : BlockState.None;
            }

            // Skip opening char and first whitespace following it
            if (processor.NextChar().IsSpaceOrTab())
            {
                processor.NextChar();
            }

            block.UpdateSpanEnd(processor.Line.End);

            return BlockState.Continue;
        }

        internal virtual FlexiAlertBlockOptions CreateFlexiAlertBlockOptions(BlockProcessor processor)
        {
            FlexiAlertBlockOptions result = _extensionOptions.DefaultBlockOptions.Clone();

            _flexiOptionsBlockService.TryPopulateOptions(processor, result, processor.LineIndex);

            // Set icon markup (precedence - FlexiOptionsBlock > default FlexiAlertBlockOptions > FlexiAlertBlocksExtensionOptions.IconMarkups)
            if (result.IconMarkup == null &&
                _extensionOptions.IconMarkups != null &&
                _extensionOptions.IconMarkups.TryGetValue(result.AlertType, out string iconMarkup))
            {
                result.IconMarkup = iconMarkup;
            }

            return result;
        }
    }
}
