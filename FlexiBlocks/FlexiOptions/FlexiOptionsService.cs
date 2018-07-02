using Markdig.Parsers;
using Newtonsoft.Json;
using System;

namespace FlexiBlocks.JsonOptions
{
    public class FlexiOptionsService
    {
        /// <summary>
        /// Attempts to extract an object of type <typeparamref name="T"/> from <paramref name="processor"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="processor"></param>
        /// <param name="blockStartLine"></param>
        /// <returns>
        /// Instance of type <typeparamref name="T"/> or null if no JSON options exists.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="processor"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the JSON cannot be parsed.</exception>
        public virtual T TryExtractOptions<T>(BlockProcessor processor, int blockStartLine) where T : class
        {
            if (processor == null)
            {
                throw new ArgumentNullException(nameof(processor));
            }

            FlexiOptionsBlock jsonOptionsBlock = TryGetJsonOptionsBlock(processor, blockStartLine);

            if (jsonOptionsBlock == null)
            {
                return null;
            }

            string json = jsonOptionsBlock.Lines.ToString();

            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (JsonException jsonException)
            {
                throw new InvalidOperationException(string.Format(Strings.InvalidOperationException_UnableToParseJson,
                    json,
                    jsonOptionsBlock.Line,
                    jsonOptionsBlock.Column),
                    jsonException);
            }
        }

        /// <summary>
        /// Attempts to populate <paramref name="target"/> from <paramref name="processor"/>. Properties in <paramref name="target"/> retain their values
        /// if the JSON does not contain replacement values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="processor"></param>
        /// <param name="target"></param>
        /// <param name="blockStartLine"></param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="processor"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="target"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the JSON cannot be parsed.</exception>
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

            FlexiOptionsBlock jsonOptionsBlock = TryGetJsonOptionsBlock(processor, blockStartLine);

            if(jsonOptionsBlock == null)
            {
                return false;
            }

            string json = jsonOptionsBlock.Lines.ToString();

            try
            {
                JsonConvert.PopulateObject(json, target);
                return true;
            }
            catch (JsonException jsonException)
            {
                throw new InvalidOperationException(string.Format(Strings.InvalidOperationException_UnableToParseJson,
                    json, jsonOptionsBlock.Line, jsonOptionsBlock.Column), jsonException);
            }
        }

        /// <summary>
        /// Attempts to retrieve a <see cref="FlexiOptionsBlock"/> from <paramref name="processor"/>.
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="blockStartLine"></param>
        /// <returns>
        /// A <see cref="FlexiOptionsBlock"/> if successful, null otherwise.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if <see cref="FlexiOptionsBlock"/> does not immediately precede current line.
        /// </exception>
        public virtual FlexiOptionsBlock TryGetJsonOptionsBlock(BlockProcessor processor, int blockStartLine)
        {
            if (processor.Document.GetData(FlexiOptionsBlockParser.FLEXI_OPTIONS) is FlexiOptionsBlock jsonOptionsBlock)
            {
                if (jsonOptionsBlock.EndLine + 1 != blockStartLine)
                {
                    throw new InvalidOperationException(string.Format(Strings.InvalidOperationException_JsonOptionsDoesNotImmediatelyPrecedeConsumingBlock,
                        jsonOptionsBlock.Lines.ToString(),
                        jsonOptionsBlock.Line,
                        jsonOptionsBlock.Column));
                }

                processor.Document.RemoveData(FlexiOptionsBlockParser.FLEXI_OPTIONS);

                return jsonOptionsBlock;
            }

            return null;
        }
    }
}
