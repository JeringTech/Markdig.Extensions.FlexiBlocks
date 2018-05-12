using Markdig.Parsers;
using Markdig.Syntax;
using Newtonsoft.Json;
using System;

namespace JeremyTCD.Markdig.Extensions
{
    public class JsonOptionsTools
    {
        /// <summary>
        /// Extracts an object of type <typeparamref name="T"/> from a preceding <see cref="JsonOptionsBlock"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="processor"></param>
        /// <returns>
        /// The extracted object if the extraction succeeded, null otherwise.
        /// </returns>
        public static T ExtractObject<T>(BlockProcessor processor) where T : class
        {
            T result = null;

            if (processor.CurrentBlock is ContainerBlock currentBlock && currentBlock.LastChild is JsonOptionsBlock jsonOptionsBlock)
            {
                string json = jsonOptionsBlock.Lines.ToString();
                try
                {
                    result  = JsonConvert.DeserializeObject<T>(json);
                }
                catch (JsonException jsonException)
                {
                    // TODO improve exception message
                    throw new InvalidOperationException($"Unable to parse json \"{json}\" at {jsonOptionsBlock.ToPositionText()}", jsonException);
                }

                currentBlock.Remove(jsonOptionsBlock);
            }

            return result;
        }

        /// <summary>
        /// Populates an object of type <typeparamref name="T"/> from a preceding <see cref="JsonOptionsBlock"/>. Properties in the object retain their values
        /// if the <see cref="JsonOptionsBlock"/> does not contain replacement values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="processor"></param>
        /// <param name="target"></param>
        /// <returns>
        /// True if population succeeded, false otherwise.
        /// </returns>
        public static bool PopulateObject<T>(BlockProcessor processor, T target) where T : class
        {
            if (processor.CurrentBlock is ContainerBlock currentBlock && currentBlock.LastChild is JsonOptionsBlock jsonOptionsBlock)
            {
                string json = jsonOptionsBlock.Lines.ToString();
                try
                {
                    JsonConvert.PopulateObject(json, target);
                }
                catch (JsonException jsonException)
                {
                    // TODO improve exception message
                    throw new InvalidOperationException($"Unable to parse json \"{json}\" at {jsonOptionsBlock.ToPositionText()}", jsonException);
                }

                currentBlock.Remove(jsonOptionsBlock);

                return true;
            }

            return false;
        }
    }
}
