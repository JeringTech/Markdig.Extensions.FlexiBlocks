using Newtonsoft.Json;
using System.IO;

namespace Jering.Markdig.Extensions.FlexiBlocks.IncludeBlocks
{
    /// <summary>
    /// <para>The default implementation of <see cref="IIncludeBlocksExtensionOptions"/>.</para>
    /// 
    /// <para>This class is immutable.</para>
    /// </summary>
    public class IncludeBlocksExtensionOptions : ExtensionOptions<IIncludeBlockOptions>, IIncludeBlocksExtensionOptions
    {
        /// <summary>
        /// Creates an <see cref="IncludeBlocksExtensionOptions" />.
        /// </summary>
        /// <param name="defaultBlockOptions">
        /// <para>Default <see cref="IIncludeBlockOptions"/> for all <see cref="IncludeBlock"/>s.</para>
        /// <para>If this value is <c>null</c>, an <see cref="IncludeBlockOptions"/> with default values is used.</para>
        /// <para>Defaults to <c>null</c>.</para>
        /// </param>
        /// <param name="baseUri">
        /// <para>The base URI for <see cref="IncludeBlock"/>s in root content.</para>
        /// <para>If this value is <c>null</c>, the application's working directory is used as the base URI.
        /// Note that the application's working directory is what <see cref="Directory.GetCurrentDirectory"/> returns.</para>
        /// <para>Defaults to <c>null</c>.</para>
        /// </param>
        public IncludeBlocksExtensionOptions(IIncludeBlockOptions defaultBlockOptions = null, string baseUri = null) :
            base(defaultBlockOptions ?? new IncludeBlockOptions())
        {
            BaseUri = baseUri;
        }

        /// <summary>
        /// Creates an <see cref="IncludeBlocksExtensionOptions"/>.
        /// </summary>
        public IncludeBlocksExtensionOptions() : this(null, null)
        {
        }

        /// <inheritdoc />
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public string BaseUri { get; private set; }
    }
}