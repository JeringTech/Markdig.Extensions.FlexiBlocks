using Markdig.Parsers;
using Markdig.Syntax;
using System;

namespace Jering.Markdig.Extensions.FlexiBlocks
{
    /// <summary>
    /// <para>An abstraction for parsing FlexiBlocks.</para> 
    /// 
    /// <para>The primary functionality that this class provides is consistent exception handling, which facilitates easy locating of issues
    /// in markdown.</para>
    /// 
    /// <para>Without this class's approach to exception handling,
    /// exceptions thrown by a parser will bubble up through Markdig and into the user facing application with
    /// no information about the location of the offending markdown.</para>
    /// 
    /// This class ensures that any exception (besides FlexiBlocksExceptions) thrown by a parser is wrapped in a FlexiBlockException with, at the very least, the
    /// line and column of the offending markdown.
    /// </summary>
    public abstract class FlexiBlockParser : BlockParser
    {
        /// <summary>
        /// Opens a FlexiBlock if a line contains the expected content. If multiple blocks are added to the processor's NewBlocks stack,
        /// the FlexiBlock must be at the top of it for correct generation of exception messages.
        /// </summary>
        /// <param name="processor">The block processor for the document that contains the line.</param>
        /// <returns>The state of the block.</returns>
        protected abstract BlockState TryOpenFlexiBlock(BlockProcessor processor);

        /// <summary>
        /// Continues a FlexiBlock if block specific expectations are met.
        /// </summary>
        /// <param name="processor">The block processor for the FlexiBlock to try and continue.</param>
        /// <param name="block">The FlexiBlock to try and continue.</param>
        /// <returns>The state of the block.</returns>
        protected virtual BlockState TryContinueFlexiBlock(BlockProcessor processor, Block block)
        {
            // Do nothing by default
            return BlockState.None;
        }

        /// <summary>
        /// Closes a FlexiBlock.
        /// </summary>
        /// <param name="processor">The block processor for the FlexiBlock to close.</param>
        /// <param name="block">The FlexiBlock to close.</param>
        /// <returns>False if the block should be discarded, true otherwise.</returns>
        /// <exception cref="FlexiBlocksException">Thrown if an exception is thrown while attempting to close the FlexiBlock.</exception>
        protected virtual bool CloseFlexiBlock(BlockProcessor processor, Block block)
        {
            // Keep the block by default
            return true;
        }

        /// <summary>
        /// Opens a FlexiBlock if a line contains the expected content.
        /// </summary>
        /// <param name="processor">The block processor for the document that contains the line.</param>
        /// <returns>The state of the block.</returns>
        /// <exception cref="FlexiBlocksException">Thrown if an exception is thrown while attempting to open the FlexiBlock.</exception>
        /// <exception cref="FlexiBlocksException">Thrown if an exception is thrown while setting up the newly opened FlexiBlock.</exception>
        public sealed override BlockState TryOpen(BlockProcessor processor)
        {
            if (processor == null)
            {
                throw new ArgumentNullException(nameof(processor));
            }

            try
            {
                return TryOpenFlexiBlock(processor);
            }
            catch (Exception exception) when ((exception as FlexiBlocksException)?.Context != Context.Block)
            {
                // The FlexiBlock must always be at the top of the NewBlocks stack
                Block newBlock = processor.NewBlocks.Count == 0 ? null : processor.NewBlocks.Peek();

                if (newBlock == null)
                {
                    // Can't add any more specific context
                    if((exception as FlexiBlocksException)?.Context == Context.Line)
                    {
                        throw;
                    }

                    throw new FlexiBlocksException(processor.LineIndex + 1,
                        processor.Column,
                        string.Format(Strings.FlexiBlocksException_FlexiBlockParser_ExceptionOccurredWhileAttemptingToOpenBlock, GetType().Name),
                        exception);
                }
                else
                {
                    throw new FlexiBlocksException(newBlock, innerException: exception);
                }
            }
        }

        /// <summary>
        /// Continues a FlexiBlock if block specific expectations are met.
        /// </summary>
        /// <param name="processor">The block processor for the FlexiBlock to try and continue.</param>
        /// <param name="block">The FlexiBlock to try and continue.</param>
        /// <returns>The state of the block.</returns>
        /// <exception cref="FlexiBlocksException">Thrown if an exception is thrown while attempting to continue the FlexiBlock.</exception>
        public sealed override BlockState TryContinue(BlockProcessor processor, Block block)
        {
            if (processor == null)
            {
                throw new ArgumentNullException(nameof(processor));
            }

            if (block == null)
            {
                throw new ArgumentNullException(nameof(block));
            }

            try
            {
                return TryContinueFlexiBlock(processor, block);
            }
            catch (Exception exception) when ((exception as FlexiBlocksException)?.Context != Context.Block)
            {
                throw new FlexiBlocksException(block, innerException: exception);
            }
        }

        /// <summary>
        /// Closes a FlexiBlock.
        /// </summary>
        /// <param name="processor">The block processor for the FlexiBlock to close.</param>
        /// <param name="block">The FlexiBlock to close.</param>
        /// <returns>False if the block should be discarded, true otherwise.</returns>
        /// <exception cref="FlexiBlocksException">Thrown if an exception is thrown while attempting to close the FlexiBlock.</exception>
        public sealed override bool Close(BlockProcessor processor, Block block)
        {
            if (processor == null)
            {
                throw new ArgumentNullException(nameof(processor));
            }

            if (block == null)
            {
                throw new ArgumentNullException(nameof(block));
            }

            try
            {
                return CloseFlexiBlock(processor, block);
            }
            catch (Exception exception) when ((exception as FlexiBlocksException)?.Context != Context.Block)
            {
                throw new FlexiBlocksException(block, innerException: exception);
            }
        }
    }
}
