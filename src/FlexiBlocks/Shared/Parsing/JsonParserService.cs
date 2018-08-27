using Markdig.Helpers;
using Newtonsoft.Json;

namespace Jering.Markdig.Extensions.FlexiBlocks
{
    /// <summary>
    /// The default implementation of <see cref="IJsonParserService"/>.
    /// </summary>
    public class JsonParserService : IJsonParserService
    {
        private readonly JsonSerializer _jsonSerializer;

        /// <summary>
        /// Creates a <see cref="JsonParserService"/> instance.
        /// </summary>
        public JsonParserService()
        {
            _jsonSerializer = JsonSerializer.Create();
        }

        /// <inheritdoc />
        public (int numLines, T result) Parse<T>(StringSlice stringSlice)
        {
            var textLineReader = new TextLineReader(stringSlice.Text, stringSlice.Start);
            using (var jsonTextReader = new JsonTextReader(textLineReader))
            {
                T result = _jsonSerializer.Deserialize<T>(jsonTextReader);
                return (textLineReader.LinesRead, result);
            }
        }
    }
}
