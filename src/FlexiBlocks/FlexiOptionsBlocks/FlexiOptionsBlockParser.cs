using Markdig.Parsers;
using Markdig.Syntax;
using System;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks
{
    /// <summary>
    /// A markdown parser that creates <see cref="FlexiOptionsBlock"/>s.
    /// </summary>
    public class FlexiOptionsBlockParser : BlockParser
    {
        /// <summary>
        /// Key for storing <see cref="FlexiOptionsBlock"/>s in <see cref="BlockProcessor.Document"/> data.
        /// </summary>
        public const string FLEXI_OPTIONS_BLOCK = "flexiOptionsBlock";

        /// <summary>
        /// Creates a <see cref="FlexiOptionsBlockParser"/> instance.
        /// </summary>
        public FlexiOptionsBlockParser()
        {
            // If options block is not consumed by the following block, it is rendered as a paragraph or in the preceding paragraph, so {, despite being common, should work fine.
            OpeningCharacters = new[] { '@' };
        }

        /// <summary>
        /// Opens a FlexiOptionsBlock if a line begins with "@{".
        /// </summary>
        /// <param name="processor"></param>
        /// <returns>
        /// <see cref="BlockState.None"/> if the current line has code indent or if the current line does not start with @{.
        /// <see cref="BlockState.Break"/> if the current line contains the entire JSON string.
        /// <see cref="BlockState.Continue"/> if the current line contains part of the JSON string.
        /// </returns>
        public override BlockState TryOpen(BlockProcessor processor)
        {
            if (processor.IsCodeIndent)
            {
                return BlockState.None;
            }

            // First line of a FlexiOptionsBlock must begin with @{
            if (processor.Line.PeekChar() != '{')
            {
                return BlockState.None;
            }

            // Dispose of @ (BlockProcessor appends processor.Line to the new FlexiOptionsBlock, so it must start at the curly bracket)
            processor.Line.Start++;

            var flexiOptionsBlock = new FlexiOptionsBlock(this)
            {
                Column = processor.Column,
                Span = { Start = processor.Line.Start }
            };
            processor.NewBlocks.Push(flexiOptionsBlock);

            return flexiOptionsBlock.ParseLine(processor.Line);
        }

        /// <summary>
        /// Determines whether or not the <see cref="FlexiOptionsBlock"/> is complete.
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="block"></param>
        /// <returns>
        /// <see cref="BlockState.Continue"/> if <paramref name="block"/> is still open.
        /// <see cref="BlockState.Break"/> if <paramref name="block"/> has ended and should be closed.
        /// </returns>
        public override BlockState TryContinue(BlockProcessor processor, Block block)
        {
            var flexiOptionsBlock = (FlexiOptionsBlock)block;

            return flexiOptionsBlock.ParseLine(processor.Line);
        }

        /// <summary>
        /// Adds closing <see cref="FlexiOptionsBlock"/>s to <see cref="BlockProcessor.Document"/> data.
        /// </summary>
        /// <param name="processor">The processor for the block that is closing.</param>
        /// <param name="block">The block that is closing.</param>
        /// <returns>Returns false, indicating that the block should be discarded from the tree of blocks.</returns>
        public override bool Close(BlockProcessor processor, Block block)
        {
            if (processor.Document.GetData(FLEXI_OPTIONS_BLOCK) is FlexiOptionsBlock pendingFlexiOptionsBlock)
            {
                // There is an unused FlexiOptionsBlock
                throw new InvalidOperationException(string.Format(
                    Strings.InvalidOperationException_UnusedFlexiOptionsBlock,
                    pendingFlexiOptionsBlock.Lines.ToString(),
                    pendingFlexiOptionsBlock.Line,
                    pendingFlexiOptionsBlock.Column));
            }

            // Save the options block to document data. There are two reasons for this. Firstly, it makes it easy to detect if an options block goes unused.
            // Secondly, it means that the options block does not need to be a sibling of the block that consumes it. This can occur in
            // when extensions like FlexiSectionBlocks is used - when a container block only ends when a new container block
            // is encountered, the options block ends up being a child of the container block that precedes the container block that the options apply to.
            processor.Document.SetData(FLEXI_OPTIONS_BLOCK, block);

            // If true is returned, the block is kept as a child of its parent for rendering later on. If false is returned,
            // the block is discarded. We don't need the block any more.
            return false;
        }
    }
}
