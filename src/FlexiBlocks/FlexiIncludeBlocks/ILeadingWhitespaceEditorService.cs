using Markdig.Helpers;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    /// <summary>
    /// An abstraction for editing leading whitespace of <see cref="StringSlice"/>s.
    /// </summary>
    public interface ILeadingWhitespaceEditorService
    {
        /// <summary>
        /// Removes whitespaces from the beginning of a <see cref="StringSlice"/>.
        /// </summary>
        /// <param name="line">The <see cref="StringSlice"/> to dedent.</param>
        /// <param name="dedentLength">
        /// <para>The number of whitespaces to remove.</para>
        /// <para>This value cannot be negative.</para>
        /// </param>
        void Dedent(ref StringSlice line, int dedentLength);

        /// <summary>
        /// Collapses whitespaces at the beginning of a <see cref="StringSlice"/>.
        /// </summary>
        /// <param name="line">The <see cref="StringSlice"/> whose whitespace will be collapsed.</param>
        /// <param name="collapseRatio">
        /// <para>The final leading whitespace count over the initial leading whitespace count.</para>
        /// <para>This value must be within the range [0, 1].</para>
        /// </param>
        void Collapse(ref StringSlice line, float collapseRatio);
    }
}
