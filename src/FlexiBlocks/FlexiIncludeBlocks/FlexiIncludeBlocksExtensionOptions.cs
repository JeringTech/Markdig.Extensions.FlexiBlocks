using System.IO;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    /// <summary>
    /// Represents options for the <see cref="FlexiIncludeBlocksExtension"/>.
    /// </summary>
    public class FlexiIncludeBlocksExtensionOptions : IFlexiBlocksExtensionOptions<FlexiIncludeBlockOptions>
    {
        /// <summary>
        /// <para>Gets the base URI for <see cref="FlexiIncludeBlock"/>s in the root source.</para>
        /// <para>Defaults to the application's current directory.</para>
        /// </summary>
        public string RootBaseUri { get; set; } = Directory.GetCurrentDirectory() + "/";

        /// <summary>
        /// Gets or sets the default <see cref="FlexiIncludeBlockOptions"/>.
        /// </summary>
        public FlexiIncludeBlockOptions DefaultBlockOptions { get; set; } = new FlexiIncludeBlockOptions();
    }
}