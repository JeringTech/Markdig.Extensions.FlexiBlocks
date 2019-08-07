using System.Collections.ObjectModel;

namespace Jering.Markdig.Extensions.FlexiBlocks.IncludeBlocks
{
    /// <summary>
    /// An abstraction for <see cref="IncludeBlock"/> options.
    /// </summary>
    public interface IIncludeBlockOptions : IBlockOptions<IIncludeBlockOptions>
    {
        /// <summary>
        /// Gets the <see cref="IncludeBlock"/>'s source.
        /// </summary>
        string Source { get; }

        /// <summary>
        /// Gets the <see cref="Clipping"/>s specifying content from the source to include.
        /// </summary>
        ReadOnlyCollection<Clipping> Clippings { get; }

        /// <summary>
        /// Gets the <see cref="IncludeBlock"/>'s type.
        /// </summary>
        IncludeType Type { get; }

        /// <summary>
        /// Gets the value specifying whether or not to cache the <see cref="IncludeBlock"/>'s content on disk.
        /// </summary>
        bool Cache { get; }

        /// <summary>
        /// Gets the directory to cache the <see cref="IncludeBlock"/>'s content in.
        /// </summary>
        string CacheDirectory { get; }
    }
}