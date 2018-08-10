using System.Collections.ObjectModel;
using System.Threading;
using System;
using System.IO;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    /// <summary>
    /// An abstraction for retrieving content from external sources.
    /// </summary>
    public interface IContentRetrievalService
    {
        /// <summary>
        /// Retrieve content from a source.
        /// </summary>
        /// <param name="source">The source to retrieve content from. Can be a file (local) or a Url (remote). In either case, this path can be absolute or relative. If it is relative, 
        /// it is combined with <see cref="ContentRetrievalServiceOptions.BaseUri"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A read-only collection of the lines that constitute the source.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="source"/> is null, white space, or an empty string.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="source"/> is not a valid URI.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="source"/> is a URI with an unsupported scheme (file, http and https are supported, schemes such as ftp are not).</exception>
        /// <exception cref="ContentRetrievalException">Thrown if a remote source does not exist.</exception>
        /// <exception cref="ContentRetrievalException">Thrown if access to a remote source is forbidden.</exception>
        /// <exception cref="ContentRetrievalException">Thrown if a remote source cannot be retrieved from after three attempts.</exception>
        ReadOnlyCollection<string> GetContent(string source, CancellationToken cancellationToken = default(CancellationToken));
    }
}