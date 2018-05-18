using Markdig.Parsers;
using Newtonsoft.Json;
using System;

namespace JeremyTCD.Markdig.Extensions.JsonOptions
{
    public class JsonOptionsService
    {
        /// <summary>
        /// Attempts to extract an object of type <typeparamref name="T"/> from <paramref name="processor"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="processor"></param>
        /// <returns>
        /// Instance of type <typeparamref name="T"/> or null if no JSON options exists.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="processor"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the JSON cannot be parsed.</exception>
        public virtual T TryExtractOptions<T>(BlockProcessor processor) where T : class
        {
            if (processor == null)
            {
                throw new ArgumentNullException(nameof(processor));
            }

            JsonOptionsBlock jsonOptionsBlock = TryGetJsonOptionsBlock(processor);

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
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="processor"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="target"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the JSON cannot be parsed.</exception>
        public virtual bool TryPopulateOptions<T>(BlockProcessor processor, T target) where T : class
        {
            if (processor == null)
            {
                throw new ArgumentNullException(nameof(processor));
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            JsonOptionsBlock jsonOptionsBlock = TryGetJsonOptionsBlock(processor);

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
        /// Attempts to retrieve a <see cref="JsonOptionsBlock"/> from <paramref name="processor"/>.
        /// </summary>
        /// <param name="processor"></param>
        /// <returns>
        /// A <see cref="JsonOptionsBlock"/> if successful, null otherwise.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if <see cref="JsonOptionsBlock"/> does not immediately precede current line.
        /// </exception>
        public virtual JsonOptionsBlock TryGetJsonOptionsBlock(BlockProcessor processor)
        {
            if (processor.Document.GetData(JsonOptionsParser.JSON_OPTIONS) is JsonOptionsBlock jsonOptionsBlock)
            {
                if (jsonOptionsBlock.EndLine + 1 != processor.LineIndex)
                {
                    throw new InvalidOperationException(string.Format(Strings.InvalidOperationException_JsonOptionsDoesNotImmediatelyPrecedeConsumingBlock,
                        jsonOptionsBlock.Lines.ToString(),
                        jsonOptionsBlock.Line,
                        jsonOptionsBlock.Column));
                }

                processor.Document.RemoveData(JsonOptionsParser.JSON_OPTIONS);

                return jsonOptionsBlock;
            }

            return null;
        }
    }
}
