using System;
using System.Collections.ObjectModel;
using System.Threading;

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
        /// <param name="source">The source to retrieve content from. Can be a file (local) or a URL (remote). In either case, this path can be absolute or relative. If it is relative, 
        /// it is combined with <paramref name="sourceBaseUri"/>.</param>
        /// <param name="cacheDirectory">The directory to cache remote content in. If null, white space or an empty string, remote content will not be cached.</param>
        /// <param name="sourceBaseUri">The base URI if <paramref name="source"/> is a relative URI. </param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>A read-only collection of the lines that constitute the content.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="source"/> is null, white space, or an empty string.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="source"/> is a relative URI but <paramref name="sourceBaseUri"/> is null, white space, or an empty string.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="sourceBaseUri"/> is not an absolute URI.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="source"/> is not a valid URI.</exception>
        /// <exception cref="ArgumentException">Thrown if the URI generated from <paramref name="source"/> and <paramref name="sourceBaseUri"/> is a URI with an unsupported scheme (file, http and https are supported, schemes such as ftp are not).</exception>
        /// <exception cref="ContentRetrievalException">Thrown if a remote source does not exist.</exception>
        /// <exception cref="ContentRetrievalException">Thrown if access to a remote source is forbidden.</exception>
        /// <exception cref="ContentRetrievalException">Thrown if a remote source cannot be retrieved from after three attempts.</exception>
        ReadOnlyCollection<string> GetContent(string source, string cacheDirectory = null, string sourceBaseUri = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}