using Newtonsoft.Json;
using System;

namespace JeremyTCD.Markdig.Extensions.JsonOptions
{
    public class JsonOptionsTools
    {
        /// <summary>
        /// Extracts an object of type <typeparamref name="T"/> from a <see cref="JsonOptionsBlock"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="processor"></param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="jsonOptionsBlock"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the JSON contained in <paramref name="jsonOptionsBlock"/> cannot be parsed.</exception>
        public static T ExtractObject<T>(JsonOptionsBlock jsonOptionsBlock) where T : class
        {
            if (jsonOptionsBlock == null)
            {
                throw new ArgumentNullException(nameof(jsonOptionsBlock));
            }

            string json = jsonOptionsBlock.Lines.ToString();
            try
            {
                T result = JsonConvert.DeserializeObject<T>(json);

                return result;
            }
            catch (JsonException jsonException)
            {
                throw new InvalidOperationException(string.Format(Strings.InvalidOperationException_UnableToParseJson, 
                    json, jsonOptionsBlock.Line, jsonOptionsBlock.Column), jsonException);
            }

        }

        /// <summary>
        /// Populates <paramref name="target"/> from a <see cref="JsonOptionsBlock"/>. Properties in <paramref name="target"/> retain their values
        /// if the <see cref="JsonOptionsBlock"/> does not contain replacement values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="processor"></param>
        /// <param name="target"></param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="jsonOptionsBlock"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="target"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the JSON contained in <paramref name="jsonOptionsBlock"/> cannot be parsed.</exception>
        public static void PopulateObject<T>(JsonOptionsBlock jsonOptionsBlock, T target) where T : class
        {
            if (jsonOptionsBlock == null)
            {
                throw new ArgumentNullException(nameof(jsonOptionsBlock));
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            string json = jsonOptionsBlock.Lines.ToString();
            try
            {
                JsonConvert.PopulateObject(json, target);
            }
            catch (JsonException jsonException)
            {
                throw new InvalidOperationException(string.Format(Strings.InvalidOperationException_UnableToParseJson,
                    json, jsonOptionsBlock.Line, jsonOptionsBlock.Column), jsonException);
            }
        }
    }
}
