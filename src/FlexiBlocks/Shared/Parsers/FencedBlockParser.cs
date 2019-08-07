using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using System;

namespace Jering.Markdig.Extensions.FlexiBlocks
{
    /// <summary>
    /// An abstraction for parsing fenced <see cref="Block"/>s.
    /// </summary>
    /// <typeparam name="TMain">The type of fenced <see cref="Block"/> this parser parsers.</typeparam>
    /// <typeparam name="TProxy">The type of <see cref="IProxyFencedBlock"/> to collect data for the fenced <see cref="Block"/>.</typeparam>
    public abstract class FencedBlockParser<TMain, TProxy> : ProxyBlockParser<TMain, TProxy>
        where TMain : Block
        where TProxy : Block, IProxyFencedBlock
    {
        private readonly IFencedBlockFactory<TMain, TProxy> _fencedBlockFactory;

        /// <summary>
        /// Creates a <see cref="FencedBlockParser{TMain, TProxy}"/>.
        /// </summary>
        /// <param name="fencedBlockFactory">The factory for creating fenced <see cref="Block"/>s.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="fencedBlockFactory"/> is <c>null</c>.</exception>
        protected FencedBlockParser(IFencedBlockFactory<TMain, TProxy> fencedBlockFactory)
        {
            _fencedBlockFactory = fencedBlockFactory ?? throw new ArgumentNullException(nameof(fencedBlockFactory));
        }

        /// <summary>
        /// Opens a <typeparamref name="TProxy"/> if a line begins with at least 3 fence characters.
        /// </summary>
        /// <param name="blockProcessor">The <see cref="BlockProcessor" /> for the document that contains a line with a fence character as its first character.</param>
        /// <returns>
        /// <see cref="BlockState.None"/> if the current line has code indent or if the current line does not contain an opening fence.
        /// <see cref="BlockState.ContinueDiscard"/> if the current line contains an opening fence and a <typeparamref name="TProxy"/> is opened.
        ///</returns>
        protected override BlockState TryOpenBlock(BlockProcessor blockProcessor)
        {
            char fenceChar = blockProcessor.CurrentChar;
            if (blockProcessor.IsCodeIndent ||
                !LineContainsOpeningFence(blockProcessor.Line, fenceChar, out int fenceCharCount))
            {
                return BlockState.None;
            }

            TProxy proxyFencedBlock = _fencedBlockFactory.CreateProxyFencedBlock(blockProcessor.Indent, fenceCharCount, fenceChar, blockProcessor, this);
            blockProcessor.NewBlocks.Push(proxyFencedBlock);

            return BlockState.ContinueDiscard;
        }

        /// <summary>
        /// Continues a <typeparamref name="TProxy"/> if the current line is not a closing fence.  
        /// </summary>
        /// <param name="blockProcessor">The <see cref="BlockProcessor" /> for the <typeparamref name="TProxy"/> to try continuing.</param>
        /// <param name="block">The <typeparamref name="TProxy"/> to try continuing.</param>
        /// <returns>
        /// <see cref="BlockState.BreakDiscard"/> if the current line contains a closing fence and the <typeparamref name="TProxy"/> is closed.
        /// <see cref="BlockState.Continue"/> if the current line has code indent or the current line does not contain a closing fence.
        /// </returns>
        protected override BlockState TryContinueBlock(BlockProcessor blockProcessor, TProxy block)
        {
            if (blockProcessor.IsCodeIndent || // Closing fence cannot be indented by more than 3 spaces
                !LineContainsClosingFence(blockProcessor.Line, block.FenceChar, block.OpeningFenceCharCount))
            {
                UpdateLineStart(blockProcessor, block);
                return BlockState.Continue;
            }

            block.UpdateSpanEnd(blockProcessor.Line.End);
            return BlockState.BreakDiscard;
        }

        /// <inheritdoc />
        protected override TMain CloseProxy(BlockProcessor blockProcessor, TProxy proxyBlock)
        {
            return _fencedBlockFactory.Create(proxyBlock, blockProcessor);
        }

        // Subtracts opening fence indent from current line indent
        internal virtual void UpdateLineStart(BlockProcessor blockProcessor, TProxy block)
        {
            blockProcessor.GoToColumn(blockProcessor.ColumnBeforeIndent + Math.Min(block.OpeningFenceIndent, blockProcessor.Indent));
        }

        internal virtual bool LineContainsOpeningFence(StringSlice line, char fenceChar, out int fenceCharCount)
        {
            fenceCharCount = 0;
            int start = line.Start;
            int end = line.End;

            if (end - start < 2) // Line must have at least 3 chars. Indices, so (end - start + 1) < 3
            {
                return false;
            }

            string text = line.Text;
            bool firstNonFenceCharFound = false,
                fenceCharIsTilde = false,
                fenceCharIsBacktick = false;
            for (int i = start; i <= end; i++)
            {
                char peekedChar = text[i];
                if (!firstNonFenceCharFound)
                {
                    if (firstNonFenceCharFound = peekedChar != fenceChar)
                    {
                        if ((fenceCharCount = i - start) < 3) // Opening fence must have at least 3 chars
                        {
                            return false;
                        }

                        fenceCharIsTilde = fenceChar == '~';
                        fenceCharIsBacktick = fenceChar == '`';
                    }
                    else
                    {
                        continue;
                    }
                }

                if (fenceCharIsTilde) // Any char is allowed after a tilde opening fence
                {
                    continue;
                }

                if (fenceCharIsBacktick)
                {
                    if (peekedChar == '`') // If fence char is backtick, non-whitespace chars other than backticks (to avoid issues with inline code) are allowed after the fence
                    {
                        return false;
                    }
                }
                else if (!peekedChar.IsWhitespace())
                {
                    return false; // If fence char isn't backtick, only whitespace chars are allowed after the fence
                }
            }

            if (!firstNonFenceCharFound)
            {
                fenceCharCount = end - start + 1; // All characters in line are fence chars
            }

            return true;
        }

        internal virtual bool LineContainsClosingFence(StringSlice line, char fenceChar, int openingFenceCharCount)
        {
            int start = line.Start;
            int end = line.End;

            if (end - start + 1 < openingFenceCharCount) // Closing fence must have at least as many fence chars as opening fence
            {
                return false;
            }

            string text = line.Text;
            bool firstNonFenceCharFound = false;
            for (int i = start; i <= end; i++)
            {
                char peekedChar = text[i];
                if (!firstNonFenceCharFound)
                {
                    if (firstNonFenceCharFound = peekedChar != fenceChar)
                    {
                        if (i - start < openingFenceCharCount) // Closing fence must have at least as many fence chars as opening fence
                        {
                            return false;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                if (!peekedChar.IsWhitespace()) // Only whitespaces allowed after the fence
                {
                    return false;
                }
            }

            return true;
        }
    }
}
