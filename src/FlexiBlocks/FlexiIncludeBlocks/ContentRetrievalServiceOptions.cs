using System.IO;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    /// <summary>
    /// Options for <see cref="ContentRetrievalService"/>.
    /// </summary>
    public class ContentRetrievalServiceOptions
    {
        /// <summary>
        /// Gets or sets the base URI for content sources that are specified using relative URIs.
        /// </summary>
        public string BaseUri  { get; set; } = Directory.GetCurrentDirectory();
    }
}