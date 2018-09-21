using Markdig.Parsers;
using Markdig.Syntax;
using System;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks
{
    /// <summary>
    /// A parser that creates <see cref="FlexiOptionsBlock"/>s from markdown.
    /// </summary>
    public class FlexiOptionsBlockParser : FlexiBlockParser
    {
        /// <summary>
        /// The key for storing the most recently created <see cref="FlexiOptionsBlock"/>.
        /// </summary>
        public const string PENDING_FLEXI_OPTIONS_BLOCK = "lastFlexiOptionsBlock";

        /// <summary>
        /// Creates a <see cref="FlexiOptionsBlockParser"/> instance.
        /// </summary>
        public FlexiOptionsBlockParser()
        {
            OpeningCharacters = new[] { '@' };
        }

        /// <summary>
        /// Opens a <see cref="FlexiOptionsBlock"/> if a line begins with 0 to 3 spaces followed by "@{".
        /// </summary>
        /// <param name="processor">The block processor for the document that contains a line with first non-white-space character "@".</param>
        /// <returns>
        /// <see cref="BlockState.None"/> if the current line has code indent or if the current line does not start with the expected characters.
        /// <see cref="BlockState.Break"/> if a <see cref="FlexiOptionsBlock"/> is opened and the current line contains the entire JSON string.
        /// <see cref="BlockState.Continue"/> if a <see cref="FlexiOptionsBlock"/> is opened and the current line contains part of the JSON string.
        /// </returns>
        protected override BlockState TryOpenFlexiBlock(BlockProcessor processor)
        {
            if (processor == null)
            {
                throw new ArgumentNullException(nameof(processor));
            }

            if (processor.IsCodeIndent)
            {
                return BlockState.None;
            }

            // First line of a FlexiOptionsBlock must begin with @{
            if (processor.PeekChar(1) != '{')
            {
                return BlockState.None;
            }

            var flexiOptionsBlock = new FlexiOptionsBlock(this)
            {
                Column = processor.Column,
                Span = { Start = processor.Start } // FlexiOptionsBlock.ParseLine will update the span's end
            };

            processor.NewBlocks.Push(flexiOptionsBlock);

            // Dispose of @ (JSON starts at the curly bracket)
            processor.NextChar();

            return flexiOptionsBlock.ParseLine(processor.Line);
        }

        /// <summary>
        /// Continues a <see cref="FlexiOptionsBlock"/> if its JSON is incomplete.
        /// </summary>
        /// <param name="processor">The block processor for the <see cref="FlexiOptionsBlock"/> to try and continue.</param>
        /// <param name="block">The <see cref="FlexiOptionsBlock"/> to try and continue.</param>
        /// <returns>
        /// <see cref="BlockState.Continue"/> if the <see cref="FlexiOptionsBlock"/> is still open.
        /// <see cref="BlockState.Break"/> if the <see cref="FlexiOptionsBlock"/> has ended and should be closed.
        /// </returns>
        protected override BlockState TryContinueFlexiBlock(BlockProcessor processor, Block block)
        {
            if (processor == null)
            {
                throw new ArgumentNullException(nameof(processor));
            }

            if (block == null)
            {
                throw new ArgumentNullException(nameof(block));
            }

            var flexiOptionsBlock = (FlexiOptionsBlock)block;

            return flexiOptionsBlock.ParseLine(processor.Line);
        }

        /// <summary>
        /// Adds the <see cref="FlexiOptionsBlock"/> to the root document's data.
        /// </summary>
        /// <param name="processor">The block processor for the <see cref="FlexiOptionsBlock"/> that is closing.</param>
        /// <param name="block">The <see cref="FlexiOptionsBlock"/> that is closing.</param>
        /// <returns>Returns false, indicating that the <see cref="FlexiOptionsBlock"/> should be discarded from the tree of blocks.</returns>
        /// <exception cref="FlexiBlocksException">Thrown if there is an uncomsumed <see cref="FlexiOptionsBlock"/>.</exception>
        protected override bool CloseFlexiBlock(BlockProcessor processor, Block block)
        {
            if (processor == null)
            {
                throw new ArgumentNullException(nameof(processor));
            }

            if (block == null)
            {
                throw new ArgumentNullException(nameof(block));
            }

            if (processor.Document.GetData(PENDING_FLEXI_OPTIONS_BLOCK) is FlexiOptionsBlock pendingFlexiOptionsBlock)
            {
                // There is an unconsumed FlexiOptionsBlock
                throw new FlexiBlocksException(pendingFlexiOptionsBlock, Strings.FlexiBlocksException_FlexiOptionsBlocks_UnconsumedBlock);
            }

            // Save the options block to document data. There are two reasons for this. Firstly, it makes it easy to detect if an options block goes unused.
            // Secondly, it means that the options block does not need to be a sibling of the block that consumes it. This can occur
            // when extensions like FlexiSectionBlocks are used. When a container block only ends when a new container block
            // is encountered, an options block can end up being a child of the container block that precedes the container block that the options apply to.
            // Searching through the tree of blocks is a brittle approach. This simple approach is relatively robust.
            processor.Document.SetData(PENDING_FLEXI_OPTIONS_BLOCK, block);

            // If true is returned, the block is kept as a child of its parent for rendering later on. If false is returned,
            // the block is discarded. We don't need the block any more, so we return false.
            return false;
        }
    }
}
