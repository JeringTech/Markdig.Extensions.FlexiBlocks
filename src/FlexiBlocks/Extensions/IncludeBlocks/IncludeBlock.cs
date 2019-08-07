using Markdig.Parsers;
using Markdig.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Jering.Markdig.Extensions.FlexiBlocks.IncludeBlocks
{
    /// <summary>
    /// Represents a block that includes content from a local or remote source.
    /// </summary>
    public class IncludeBlock : Block
    {
        /// <summary>
        /// Creates an <see cref="IncludeBlock"/>.
        /// </summary>
        /// <param name="source">The <see cref="IncludeBlock"/>'s source.</param>
        /// <param name="clippings">The <see cref="Clipping"/>s specifying content from the source to include.</param>
        /// <param name="type">The <see cref="IncludeBlock"/>'s type.</param>
        /// <param name="cacheDirectory">The directory to cache the <see cref="IncludeBlock"/>'s content in.</param>
        /// <param name="parentIncludeBlock">The <see cref="IncludeBlock"/>'s parent <see cref="IncludeBlock"/>.</param>
        /// <param name="containingSource">The URI of the source that contains the <see cref="IncludeBlock"/>.</param>
        /// <param name="blockParser">The <see cref="BlockParser"/> parsing the <see cref="IncludeBlock"/>.</param>
        public IncludeBlock(Uri source,
            ReadOnlyCollection<Clipping> clippings,
            IncludeType type,
            string cacheDirectory,
            IncludeBlock parentIncludeBlock,
            string containingSource,
            BlockParser blockParser) : base(blockParser)
        {
            Source = source;
            Clippings = clippings;
            Type = type;
            CacheDirectory = cacheDirectory;
            ParentIncludeBlock = parentIncludeBlock;
            ContainingSource = containingSource;
            Children = new List<IncludeBlock>();
        }

        /// <summary>
        /// Gets the <see cref="IncludeBlock"/>'s source.
        /// </summary>
        public Uri Source { get; }

        /// <summary>
        /// Gets the <see cref="Clipping"/>s specifying content from the source to include.
        /// </summary>
        public ReadOnlyCollection<Clipping> Clippings { get; }

        /// <summary>
        /// Gets the <see cref="IncludeBlock"/>'s type.
        /// </summary>
        public IncludeType Type { get; }

        /// <summary>
        /// Gets the directory to cache the <see cref="IncludeBlock"/>'s content in.
        /// </summary>
        public string CacheDirectory { get; }

        /// <summary>
        /// Gets the <see cref="IncludeBlock"/>'s parent <see cref="IncludeBlock"/>.
        /// </summary>
        public IncludeBlock ParentIncludeBlock { get; }

        /// <summary>
        /// Gets the URI of the source that contains the <see cref="IncludeBlock"/>.
        /// </summary>
        public string ContainingSource { get; }

        /// <summary>
        /// Gets the <see cref="IncludeBlock"/>'s child <see cref="IncludeBlock"/>s.
        /// </summary>
        public List<IncludeBlock> Children { get; }
    }
}
