using System;
using System.Collections.ObjectModel;
using System.Threading;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    /// <summary>
    /// An abstraction for retrieving sources.
    /// </summary>
    public interface ISourceRetrieverService
    {
        /// <summary>
        /// Retrieves a source.
        /// </summary>
        /// <param name="sourceUri">The URI of the source to retrieve.</param>
        /// <param name="cacheDirectory">
        /// <para>The directory to cache remote sources in.</para>
        /// <para>If null, whitespace or an empty string, remote sources will not be cached.</para>
        /// </param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>The source as a read-only collection of the lines.</returns>
        /// <exception cref="FlexiBlocksException">Thrown if <paramref name="sourceUri"/> is remote and does not exist.</exception>
        /// <exception cref="FlexiBlocksException">Thrown if <paramref name="sourceUri"/> is remote and access to it is forbidden.</exception>
        /// <exception cref="FlexiBlocksException">Thrown if <paramref name="sourceUri"/> is remote and cannot be retrieved from after multiple attempts.</exception>
        /// <exception cref="FlexiBlocksException">Thrown if <paramref name="sourceUri"/> is an invalid local URI.</exception>
        ReadOnlyCollection<string> GetSource(Uri sourceUri, string cacheDirectory = null, CancellationToken cancellationToken = default);
    }
}