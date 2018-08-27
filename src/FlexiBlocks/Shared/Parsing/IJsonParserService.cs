using System;
using Markdig.Helpers;
using Newtonsoft.Json;

namespace Jering.Markdig.Extensions.FlexiBlocks
{
    /// <summary>
    /// An abstraction for parsing JSON that exists within markdown.
    /// </summary>
    public interface IJsonParserService
    {
        /// <summary>
        /// <para>Deserializes JSON in markdown contained in a string slice. Records the number of lines that the JSON spans.</para>
        /// <para>There is no trivial way to deserialize JSON over multiple JSON.NET method calls.
        /// For example, we can't call JsonSerializer.Deserialize with half the JSON, store deserialization state
        /// somewhere, and then call JsonSerializer.Deserialize again with the remaining JSON when we have access to it.</para>
        /// <para>Markdig processes markdown line by line, so to deserialize JSON in markdown, we need to "lookahead" 
        /// to parse JSON in a single call to JsonSerializer.Deserialize. We also need to record the number of lines that 
        /// the JSON spans so we can skip processing them as markdown.</para>
        /// <para>This method provides all of the required functionality for parsing JSON in markdown.</para>
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="stringSlice">The string slice containing the markdown that contains JSON.
        /// The string slice must start at the first character of the JSON.</param>
        /// <returns>A value tuple containing the number of lines that the JSON spans as well as the deserialization result. </returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="stringSlice"/>'s text is an empty string.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="stringSlice"/>'s start index is not within the interval containing its text's indices.</exception>
        /// <exception cref="JsonException">Thrown if the JSON cannot be deserialized.</exception>
        (int numLines, T result) Parse<T>(StringSlice stringSlice);
    }
}
