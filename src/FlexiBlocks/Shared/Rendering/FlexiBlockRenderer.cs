using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using System;

namespace Jering.Markdig.Extensions.FlexiBlocks
{
    /// <summary>
    /// <para>An abstraction for rendering FlexiBlocks.</para> 
    /// 
    /// <para>The primary functionality that this class provides is consistent exception handling, which facilitates easy locating of issues
    /// in markdown.</para>
    /// 
    /// <para>Without this class's approach to exception handling,
    /// exceptions thrown by a renderer will bubble up through Markdig and into the user facing application with
    /// no information about the location of the offending markdown.</para>
    /// 
    /// This class ensures that any exception (besides FlexiBlocksExceptions) thrown by a renderer is wrapped in a FlexiBlockException with, at the very least, the
    /// line and column of the offending markdown.
    /// </summary>
    /// <typeparam name="T">The type of FlexiBlock that this renderer renders.</typeparam>
    public abstract class FlexiBlockRenderer<T> : HtmlObjectRenderer<T> where T : Block
    {
        /// <summary>
        /// Renders a FlexiBlock as HTML.
        /// </summary>
        /// <param name="renderer">The renderer to write to.</param>
        /// <param name="obj">The FlexiBlock to render.</param>
        protected abstract void WriteFlexiBlock(HtmlRenderer renderer, T obj);

        /// <summary>
        /// Renders a FlexiBlock as HTML.
        /// </summary>
        /// <param name="renderer">The renderer to write to.</param>
        /// <param name="obj">The FlexiBlock to render.</param>
        protected sealed override void Write(HtmlRenderer renderer, T obj)
        {
            if (renderer == null)
            {
                throw new ArgumentNullException(nameof(renderer));
            }

            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            try
            {
                renderer.EnsureLine();

                WriteFlexiBlock(renderer, obj);
            }
            catch (Exception exception) when ((exception as FlexiBlocksException)?.Context != FlexiBlockExceptionContext.Block)
            {
                throw new FlexiBlocksException(obj as Block, innerException: exception);
            }
        }
    }
}
