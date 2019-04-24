using Markdig.Parsers;
using System;

namespace Jering.Markdig.Extensions.FlexiBlocks.OptionsBlocks
{
    /// <summary>
    /// The implementation of <see cref="IJsonBlockFactory{TMain, TProxy}"/> for creating <see cref="OptionsBlock"/>s.
    /// </summary>
    public class OptionsBlockFactory : IJsonBlockFactory<OptionsBlock, ProxyJsonBlock>
    {
        /// <summary>
        /// The key for storing the most recently created <see cref="OptionsBlock"/>.
        /// </summary>
        public const string PENDING_OPTIONS_BLOCK = "lastOptionsBlock";

        /// <summary>
        /// Creates a <see cref="ProxyJsonBlock"/>.
        /// </summary>
        /// <param name="blockProcessor">The <see cref="BlockProcessor" /> processing the <see cref="ProxyJsonBlock"/>.</param>
        /// <param name="blockParser">The <see cref="BlockParser"/> parsing the <see cref="ProxyJsonBlock"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="blockProcessor"/> is <c>null</c>.</exception>
        public ProxyJsonBlock CreateProxyJsonBlock(BlockProcessor blockProcessor, BlockParser blockParser)
        {
            if (blockProcessor == null)
            {
                throw new ArgumentNullException(nameof(blockProcessor));
            }

            return new ProxyJsonBlock(nameof(OptionsBlock), blockParser)
            {
                Column = blockProcessor.Column,
                Span = { Start = blockProcessor.Start } // JsonBlockParser.ParseLine will update the span's end
                // Line is assigned by BlockProcessor
            };
        }

        /// <summary>
        /// Creates an <see cref="OptionsBlock"/>.
        /// </summary>
        /// <param name="proxyJsonBlock">The <see cref="ProxyJsonBlock"/> containing data for the <see cref="OptionsBlock"/>.</param>
        /// <param name="blockProcessor">The <see cref="BlockProcessor" /> processing the <see cref="OptionsBlock"/>.</param>
        /// <exception cref="BlockException">Thrown if there is an unconsumed <see cref="OptionsBlock"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="proxyJsonBlock"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="blockProcessor"/> is <c>null</c>.</exception>
        public OptionsBlock Create(ProxyJsonBlock proxyJsonBlock, BlockProcessor blockProcessor)
        {
            if (proxyJsonBlock == null)
            {
                throw new ArgumentNullException(nameof(proxyJsonBlock));
            }

            if (blockProcessor == null)
            {
                throw new ArgumentNullException(nameof(blockProcessor));
            }

            if (blockProcessor.Document.GetData(PENDING_OPTIONS_BLOCK) is OptionsBlock pendingOptionsBlock)
            {
                // There is an unconsumed OptionsBlock
                throw new BlockException(pendingOptionsBlock, Strings.BlockException_OptionsBlockParser_UnconsumedBlock);
            }

            var optionsBlock = new OptionsBlock(proxyJsonBlock.Parser)
            {
                Lines = proxyJsonBlock.Lines,
                Line = proxyJsonBlock.Line,
                Column = proxyJsonBlock.Column,
                Span = proxyJsonBlock.Span
            };

            // Save the options block to document data. There are two reasons for this. Firstly, it's easy to detect if an options block goes unused.
            // Secondly, the options block does not need to be a sibling of the block that consumes it. This can occur
            // when extensions like FlexiSections are used. If a container block only ends when a new container block
            // is encountered, an options block can end up being a child of the container block that precedes the container block the options apply to.
            // Searching through the tree of blocks is a brittle approach. This simple approach is relatively robust.
            blockProcessor.Document.SetData(PENDING_OPTIONS_BLOCK, optionsBlock);

            return null; // Block is already in Document, it does not need to be added to tree
        }
    }
}
