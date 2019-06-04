using Jering.IocServices.Newtonsoft.Json;
using Jering.Markdig.Extensions.FlexiBlocks.OptionsBlocks;
using Markdig.Parsers;
using Markdig.Syntax;
using Newtonsoft.Json;
using System;

namespace Jering.Markdig.Extensions.FlexiBlocks
{
    /// <summary>
    /// The default implementation of <see cref="IBlockOptionsFactory{T}"/>.
    /// </summary>
    public class BlockOptionsFactory<T> : IBlockOptionsFactory<T> where T : IBlockOptions<T>
    {
        private readonly IJsonSerializerService _jsonSerializerService;

        /// <summary>
        /// Creates a <see cref="BlockOptionsFactory{T}"/>.
        /// </summary>
        /// <param name="jsonSerializerService">The service that handles JSON deserialization.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="jsonSerializerService"/> is <c>null</c>.</exception>
        public BlockOptionsFactory(IJsonSerializerService jsonSerializerService)
        {
            _jsonSerializerService = jsonSerializerService ?? throw new ArgumentNullException(nameof(jsonSerializerService));
        }

        /// <inheritdoc />
        public virtual T Create(T defaultBlockOptions, BlockProcessor blockProcessor)
        {
            if (blockProcessor == null)
            {
                throw new ArgumentNullException(nameof(blockProcessor));
            }

            // Get OptionsBlock
            OptionsBlock optionsBlock = TryGetOptionsBlock(blockProcessor);
            if (optionsBlock == null)
            {
                return defaultBlockOptions;
            }

            return Create(defaultBlockOptions, optionsBlock);
        }

        /// <inheritdoc />
        public virtual T Create(T defaultBlockOptions, LeafBlock leafBlock)
        {
            if (defaultBlockOptions == null)
            {
                throw new ArgumentNullException(nameof(defaultBlockOptions));
            }

            if (leafBlock == null)
            {
                throw new ArgumentNullException(nameof(leafBlock));
            }

            T result = defaultBlockOptions.Clone();

            try
            {
                using (var jsonTextReader = new JsonTextReader(new LeafBlockReader(leafBlock)))
                {
                    _jsonSerializerService.Populate(jsonTextReader, result);
                }
            }
            catch (Exception exception)
            {
                throw new BlockException(leafBlock, string.Format(Strings.OptionsException_BlockOptionsFactory_InvalidJson, leafBlock.Lines.ToString()), exception);
            }

            return result;
        }

        /// <summary>
        /// Returns <c>null</c> if unable to retrieve an <see cref="OptionsBlock"/>.
        /// </summary>
        internal virtual OptionsBlock TryGetOptionsBlock(BlockProcessor processor)
        {
            MarkdownDocument markdownDocument = processor.Document;
            if (markdownDocument.GetData(OptionsBlockFactory.PENDING_OPTIONS_BLOCK) is OptionsBlock OptionsBlock)
            {
                markdownDocument.RemoveData(OptionsBlockFactory.PENDING_OPTIONS_BLOCK);

                return OptionsBlock;
            }

            return null;
        }
    }
}
