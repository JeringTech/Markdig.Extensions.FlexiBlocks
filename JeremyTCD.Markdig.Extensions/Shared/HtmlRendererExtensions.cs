using Markdig.Renderers;
using System.Collections.Generic;

namespace JeremyTCD.Markdig.Extensions
{
    public static class HtmlRendererExtensions
    {
        public static HtmlRenderer WriteCustomAttributes(this HtmlRenderer htmlRenderer, Dictionary<string, string> attributes)
        {
            if (attributes != null)
            {
                foreach (KeyValuePair<string, string> attribute in attributes)
                {
                    htmlRenderer.Write($" {attribute.Key}=\"");
                    htmlRenderer.WriteEscape(attribute.Value);
                    htmlRenderer.Write($"\"");
                }
            }

            return htmlRenderer;
        }
    }
}
