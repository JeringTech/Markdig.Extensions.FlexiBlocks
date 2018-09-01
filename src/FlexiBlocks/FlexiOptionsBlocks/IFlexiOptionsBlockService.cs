using Markdig.Parsers;
using System;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks
{
    /// <summary>
    /// An abstraction for utilizing <see cref="FlexiOptionsBlock"/>s.
    /// </summary>
    public interface IFlexiOptionsBlockService
    {
        /// <summary>
        /// Attempts to extract an object from the <see cref="FlexiOptionsBlock"/> held by a block processor.
        /// </summary>
        /// <typeparam name="T">The type of object to extract.</typeparam>
        /// <param name="processor">The block processor holding the <see cref="FlexiOptionsBlock"/>.</param>
        /// <param name="consumingBlockStartLineNumber">The start line number of the block that the options will apply to.</param>
        /// <returns>An instance of type <typeparamref name="T"/> or null if the block processor does not hold a <see cref="FlexiOptionsBlock"/>.</returns>
        /// <exception cref="FlexiBlocksException">Thrown if the <see cref="FlexiOptionsBlock"/>'s JSON cannot be deserialized.</exception>
        T TryExtractOptions<T>(BlockProcessor processor, int consumingBlockStartLineNumber) where T : class;

        /// <summary>
        /// Attempts to populate an object from the <see cref="FlexiOptionsBlock"/> held by a block processor. 
        /// </summary>
        /// <typeparam name="T">The type of object to extract.</typeparam>
        /// <param name="processor">The block processor holding the <see cref="FlexiOptionsBlock"/>.</param>
        /// <param name="target">The object to populate.</param>
        /// <param name="consumingBlockStartLineNumber">The start line number of the block that the options will apply to.</param>
        /// <returns>True if the object is successfully populated, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="target"/> is null.</exception>
        /// <exception cref="FlexiBlocksException">Thrown if the <see cref="FlexiOptionsBlock"/>'s JSON cannot be deserialized.</exception>
        bool TryPopulateOptions<T>(BlockProcessor processor, T target, int consumingBlockStartLineNumber) where T : class;

        /// <summary>
        /// Attempts to retrieve the <see cref="FlexiOptionsBlock"/> held by a block processor.
        /// </summary>
        /// <param name="processor">The block processor holding the <see cref="FlexiOptionsBlock"/>.</param>
        /// <param name="consumingBlockStartLineNumber">The start line number of the block that the options will apply to.</param>
        /// <returns>
        /// A <see cref="FlexiOptionsBlock"/> if successful, null otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="processor"/> is null.</exception>
        /// <exception cref="FlexiBlocksException">Thrown if the <see cref="FlexiOptionsBlock"/> does not immediately precede the block that the options 
        /// will apply to.</exception>
        FlexiOptionsBlock TryGetFlexiOptionsBlock(BlockProcessor processor, int consumingBlockStartLineNumber);
    }
}
