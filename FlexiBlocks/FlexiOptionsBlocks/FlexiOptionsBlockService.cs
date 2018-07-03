using Markdig.Parsers;
using Newtonsoft.Json;
using System;

namespace FlexiBlocks.FlexiOptionsBlocks
{
    public class FlexiOptionsBlockService
    {
        /// <summary>
        /// Attempts to extract an object of type <typeparamref name="T"/> from the <see cref="FlexiOptionsBlock"/> held by <paramref name="processor"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="processor"></param>
        /// <param name="blockStartLine"></param>
        /// <returns>
        /// Instance of type <typeparamref name="T"/> or null if no <see cref="FlexiOptionsBlock"/> exists.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="processor"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the <see cref="FlexiOptionsBlock"/>'s JSON cannot be parsed.</exception>
        public virtual T TryExtractOptions<T>(BlockProcessor processor, int blockStartLine) where T : class
        {
            if (processor == null)
            {
                throw new ArgumentNullException(nameof(processor));
            }

            FlexiOptionsBlock flexiOptionsBlock = TryGetFlexiOptionsBlock(processor, blockStartLine);

            if (flexiOptionsBlock == null)
            {
                return null;
            }

            string json = flexiOptionsBlock.Lines.ToString();

            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (JsonException jsonException)
            {
                throw new InvalidOperationException(string.Format(Strings.InvalidOperationException_UnableToParseJson,
                    json,
                    flexiOptionsBlock.Line,
                    flexiOptionsBlock.Column),
                    jsonException);
            }
        }

        /// <summary>
        /// Attempts to extract an object of type <typeparamref name="T"/> from  the <see cref="FlexiOptionsBlock"/> held by <paramref name="processor"/> 
        /// and to populate <paramref name="target"/> with values from the extracted object. Properties in <paramref name="target"/> 
        /// retain their values if the extracted object does not contain replacements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="processor"></param>
        /// <param name="target"></param>
        /// <param name="blockStartLine"></param>
        /// <returns>True if <paramref name="target"/> is successfully populated, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="processor"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="target"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the FlexiOptionsBlock's JSON cannot be parsed.</exception>
        public virtual bool TryPopulateOptions<T>(BlockProcessor processor, T target, int blockStartLine) where T : class
        {
            if (processor == null)
            {
                throw new ArgumentNullException(nameof(processor));
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            FlexiOptionsBlock flexiOptionsBlock = TryGetFlexiOptionsBlock(processor, blockStartLine);

            if(flexiOptionsBlock == null)
            {
                return false;
            }

            string json = flexiOptionsBlock.Lines.ToString();

            try
            {
                JsonConvert.PopulateObject(json, target);
                return true;
            }
            catch (JsonException jsonException)
            {
                throw new InvalidOperationException(string.Format(Strings.InvalidOperationException_UnableToParseJson,
                    json, flexiOptionsBlock.Line, flexiOptionsBlock.Column), jsonException);
            }
        }

        /// <summary>
        /// Attempts to retrieve the <see cref="FlexiOptionsBlock"/> held by <paramref name="processor"/>.
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="blockStartLine"></param>
        /// <returns>
        /// A <see cref="FlexiOptionsBlock"/> if successful, null otherwise.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if <see cref="FlexiOptionsBlock"/> does not immediately precede current line.
        /// </exception>
        public virtual FlexiOptionsBlock TryGetFlexiOptionsBlock(BlockProcessor processor, int blockStartLine)
        {
            if (processor.Document.GetData(FlexiOptionsBlockParser.FLEXI_OPTIONS_BLOCK) is FlexiOptionsBlock flexiOptionsBlock)
            {
                if (flexiOptionsBlock.EndLine + 1 != blockStartLine)
                {
                    throw new InvalidOperationException(string.Format(Strings.InvalidOperationException_FlexiOptionsBlockDoesNotImmediatelyPrecedeConsumingBlock,
                        flexiOptionsBlock.Lines.ToString(),
                        flexiOptionsBlock.Line,
                        flexiOptionsBlock.Column));
                }

                processor.Document.RemoveData(FlexiOptionsBlockParser.FLEXI_OPTIONS_BLOCK);

                return flexiOptionsBlock;
            }

            return null;
        }
    }
}
