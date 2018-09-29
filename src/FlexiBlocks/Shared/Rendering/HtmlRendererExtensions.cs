using Markdig.Renderers;
using Markdig.Syntax;
using System.Collections.Generic;

namespace Jering.Markdig.Extensions.FlexiBlocks
{
    /// <summary>
    /// Extensions for <see cref="HtmlRenderer"/>.
    /// </summary>
    public static class HtmlRendererExtensions
    {
        /// <summary>
        /// Writes HTML attributes.
        /// </summary>
        /// <param name="htmlRenderer">The renderer to write to.</param>
        /// <param name="attributes">The attributes to write.</param>
        public static HtmlRenderer WriteAttributes(this HtmlRenderer htmlRenderer, IDictionary<string, string> attributes)
        {
            if (attributes != null)
            {
                foreach (KeyValuePair<string, string> attribute in attributes)
                {
                    htmlRenderer.Write($" {attribute.Key}=\"");
                    htmlRenderer.WriteEscape(attribute.Value);
                    htmlRenderer.Write("\"");
                }
            }

            return htmlRenderer;
        }

        /// <summary>
        /// Writes children with the a specified implicit paragraphs setting.
        /// </summary>
        /// <param name="htmlRenderer">The renderer to write to.</param>
        /// <param name="containerBlock">The block containing children to write.</param>
        /// <param name="implicitParagraph">The boolean value specifying whether or not to render &lt;p&gt; elements.</param>
        public static HtmlRenderer WriteChildren(this HtmlRenderer htmlRenderer, ContainerBlock containerBlock, bool implicitParagraph)
        {
            bool initialImplicitParagraph = htmlRenderer.ImplicitParagraph;
            htmlRenderer.ImplicitParagraph = implicitParagraph;

            htmlRenderer.WriteChildren(containerBlock);

            htmlRenderer.ImplicitParagraph = initialImplicitParagraph;

            return htmlRenderer;
        }
    }
}
