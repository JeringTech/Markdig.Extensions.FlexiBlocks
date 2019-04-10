using Markdig.Helpers;
using Markdig.Parsers;
using System;

namespace Jering.Markdig.Extensions.FlexiBlocks.Alerts
{
    /// <summary>
    /// A parser that parses <see cref="FlexiAlert"/>s in markdown.
    /// </summary>
    public class FlexiAlertParser : BlockParser<FlexiAlert>
    {
        private readonly IBlockFactory<FlexiAlert> _flexiAlertFactory;

        /// <summary>
        /// Creates a <see cref="FlexiAlertParser"/>.
        /// </summary>
        /// <param name="flexiAlertFactory">The factory for building <see cref="FlexiAlert"/>s.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="flexiAlertFactory"/> is null.</exception>
        public FlexiAlertParser(IBlockFactory<FlexiAlert> flexiAlertFactory)
        {
            OpeningCharacters = new[] { '!' };
            _flexiAlertFactory = flexiAlertFactory ?? throw new ArgumentNullException(nameof(flexiAlertFactory));
        }

        /// <summary>
        /// Opens a <see cref="FlexiAlert"/> if a line begins with 0 to 3 spaces followed by "!".
        /// </summary>
        /// <param name="processor">The block processor for the document that contains a line with first non-white-space character "!".</param>
        /// <returns>
        /// <see cref="BlockState.None"/> if the current line has code indent. 
        /// <see cref="BlockState.Continue"/> if a <see cref="FlexiAlert"/> is opened.
        /// </returns>
        protected override BlockState TryOpenBlock(BlockProcessor processor)
        {
            if (processor.IsCodeIndent)
            {
                return BlockState.None;
            }

            // Create block
            FlexiAlert flexiAlert = _flexiAlertFactory.Create(processor, this);
            processor.NewBlocks.Push(flexiAlert);

            // Skip ! and first whitespace char after !
            if (processor.NextChar().IsSpaceOrTab())
            {
                processor.NextChar();
            }

            return BlockState.Continue;
        }

        /// <summary>
        /// Continues a <see cref="FlexiAlert"/> if the current line begins with 0 to 3 spaces followed by "!".  
        /// </summary>
        /// <param name="processor">The block processor for the <see cref="FlexiAlert"/> to try continuing.</param>
        /// <param name="block">The <see cref="FlexiAlert"/> to try continuing.</param>
        /// <returns>
        /// <see cref="BlockState.None"/> if the current line has code indent or if the current line does not begin with the expected characters.
        /// <see cref="BlockState.BreakDiscard"/> if the current line is blank, indicating that the <see cref="FlexiAlert"/> has ended and should be closed.
        /// <see cref="BlockState.Continue"/> if the <see cref="FlexiAlert"/> remains open.
        /// </returns>
        protected override BlockState TryContinueBlock(BlockProcessor processor, FlexiAlert block)
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
    }
}
