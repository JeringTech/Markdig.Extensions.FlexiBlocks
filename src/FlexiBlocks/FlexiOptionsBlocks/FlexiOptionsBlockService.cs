using Jering.IocServices.Newtonsoft.Json;
using Markdig.Parsers;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks
{
    /// <summary>
    /// The default implementation of <see cref="IFlexiOptionsBlockService"/>.
    /// </summary>
    public class FlexiOptionsBlockService : IFlexiOptionsBlockService
    {
        private readonly IJsonSerializerService _jsonSerializerService;

        /// <summary>
        /// Creates a <see cref="FlexiOptionsBlockService"/> instance.
        /// </summary>
        /// <param name="jsonSerializerService">The service that will handle JSON deserialization.</param>
        public FlexiOptionsBlockService(IJsonSerializerService jsonSerializerService)
        {
            _jsonSerializerService = jsonSerializerService ?? throw new ArgumentNullException(nameof(jsonSerializerService));
        }

        /// <inheritdoc />
        public virtual T TryExtractOptions<T>(BlockProcessor processor, int consumingBlockStartLineNumber) where T : class
        {
            FlexiOptionsBlock flexiOptionsBlock = TryGetFlexiOptionsBlock(processor, consumingBlockStartLineNumber);

            if (flexiOptionsBlock == null)
            {
                return null;
            }

            string json = flexiOptionsBlock.Lines.ToString();

            try
            {
                using (var jsonTextReader = new JsonTextReader(new StringReader(json)))
                {
                    return _jsonSerializerService.Deserialize<T>(jsonTextReader);
                }
            }
            catch (Exception exception)
            {
                // If a FlexiBlocksException is thrown while validating the new object, it is wrapped in a TargetInvocationException.
                bool innerIsFlexiBlocksException = exception.InnerException is FlexiBlocksException;

                throw new FlexiBlocksException(flexiOptionsBlock,
                    innerIsFlexiBlocksException ? null : string.Format(Strings.FlexiBlocksException_UnableToParseJson, json), // If we got to validation, deserialization succeeded
                    innerIsFlexiBlocksException ? exception.InnerException : exception);
            }
        }

        /// <inheritdoc />
        public virtual bool TryPopulateOptions<T>(BlockProcessor processor, T target, int consumingBlockStartLineNumber) where T : class
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            FlexiOptionsBlock flexiOptionsBlock = TryGetFlexiOptionsBlock(processor, consumingBlockStartLineNumber);

            if (flexiOptionsBlock == null)
            {
                return false;
            }

            string json = flexiOptionsBlock.Lines.ToString();

            try
            {
                using (var jsonTextReader = new JsonTextReader(new StringReader(json)))
                {
                    _jsonSerializerService.Populate(jsonTextReader, target);
                }
                return true;
            }
            catch (Exception exception)
            {
                // If a FlexiBlocksException is thrown while validating the populated object, it is wrapped in a TargetInvocationException.
                bool innerIsFlexiBlocksException = exception.InnerException is FlexiBlocksException;

                throw new FlexiBlocksException(flexiOptionsBlock,
                    innerIsFlexiBlocksException ? null : string.Format(Strings.FlexiBlocksException_UnableToParseJson, json), // If we got to validation, deserialization succeeded
                    innerIsFlexiBlocksException ? exception.InnerException : exception);
            }
        }

        /// <inheritdoc />
        public virtual FlexiOptionsBlock TryGetFlexiOptionsBlock(BlockProcessor processor, int consumingBlockStartLineNumber)
        {
            if(processor == null)
            {
                throw new ArgumentNullException(nameof(processor));
            }

            if (processor.Document.GetData(FlexiOptionsBlockParser.PENDING_FLEXI_OPTIONS_BLOCK) is FlexiOptionsBlock flexiOptionsBlock)
            {
                if (flexiOptionsBlock.Line + flexiOptionsBlock.Lines.Count != consumingBlockStartLineNumber)
                {
                    throw new FlexiBlocksException(flexiOptionsBlock, Strings.FlexiBlocksException_MispositionedFlexiOptionsBlock);
                }

                processor.Document.RemoveData(FlexiOptionsBlockParser.PENDING_FLEXI_OPTIONS_BLOCK);

                return flexiOptionsBlock;
            }

            return null;
        }
    }
}
