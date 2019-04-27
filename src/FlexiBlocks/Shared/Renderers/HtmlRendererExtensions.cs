using Markdig.Renderers;
using Markdig.Syntax;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace Jering.Markdig.Extensions.FlexiBlocks
{
    /// <summary>
    /// Extensions for <see cref="HtmlRenderer"/>.
    /// </summary>
    public static class HtmlRendererExtensions
    {
        /// <summary>
        /// <a>https://www.w3.org/TR/html5/infrastructure.html#space-characters</a>.
        /// </summary>
        private static readonly ImmutableHashSet<char> SPACE_CHARS = ImmutableHashSet.Create(new char[] { ' ', '\t', '\r', '\n', '\f' });

        /// <summary>
        /// If <paramref name="hasOption"/> is true, writes " {<paramref name="blockName"/>}_has-{<paramref name="optionName"/>}". 
        /// Otherwise, writes " {<paramref name="blockName"/>}_no-{<paramref name="optionName"/>}". 
        /// </summary>
        public static HtmlRenderer WriteHasOptionClass(this HtmlRenderer htmlRenderer, bool hasOption, string blockName, string optionName)
        {
            return htmlRenderer.
                Write(' '). // Never the first class
                Write(blockName).
                Write('_').
                Write(hasOption ? "has-" : "no-"). // Boolean modifier
                Write(optionName);
        }

        /// <summary>
        /// Writes "&lt;/{<paramref name="tagName"/>}&gt;".
        /// </summary>
        public static HtmlRenderer WriteEndTag(this HtmlRenderer htmlRenderer, string tagName)
        {
            return htmlRenderer.
                Write("</").
                Write(tagName).
                Write(">");
        }

        /// <summary>
        /// Writes "&lt;/{<paramref name="tagName"/>}&gt;\n".
        /// </summary>
        public static HtmlRenderer WriteEndTagLine(this HtmlRenderer htmlRenderer, string tagName)
        {
            return htmlRenderer.
                Write("</").
                Write(tagName).
                WriteLine(">");
        }

        /// <summary>
        /// Writes "&lt;/{<paramref name="tagName"/>}&gt;\n" if <paramref name="condition"/> is true.
        /// </summary>
        public static HtmlRenderer WriteEndTagLine(this HtmlRenderer htmlRenderer, bool condition, string tagName)
        {
            if (!condition)
            {
                return htmlRenderer;
            }

            return htmlRenderer.WriteEndTagLine(tagName);
        }

        /// <summary>
        /// Writes "&lt;{<paramref name="tagName"/>} class=\"{<paramref name="blockName"/>-<paramref name="elementName"/>}\" &gt;".
        /// </summary>
        public static HtmlRenderer WriteStartTag(this HtmlRenderer htmlRenderer,
            string tagName,
            string blockName,
            string elementName)
        {
            return htmlRenderer.
                Write('<').
                Write(tagName).
                Write(" class=\"").
                WriteElementClass(blockName, elementName).
                Write("\">");
        }

        /// <summary>
        /// Writes "&lt;{<paramref name="tagName"/>} class=\"{<paramref name="blockName"/>-<paramref name="elementName"/>}\"&gt;\n".
        /// </summary>
        public static HtmlRenderer WriteStartTagLine(this HtmlRenderer htmlRenderer,
            string tagName,
            string blockName,
            string elementName)
        {
            return htmlRenderer.
                Write('<').
                Write(tagName).
                Write(" class=\"").
                WriteElementClass(blockName, elementName).
                WriteLine("\">");
        }

        /// <summary>
        /// Writes "&lt;{<paramref name="tagName"/>} class=\"{<paramref name="blockName"/>-<paramref name="elementName"/>}\" {<paramref name="attributes"/>}&gt;\n".
        /// </summary>
        public static HtmlRenderer WriteStartTagLine(this HtmlRenderer htmlRenderer,
            string tagName,
            string blockName,
            string elementName,
            string attributes)
        {
            return htmlRenderer.
                Write('<').
                Write(tagName).
                Write(" class=\"").
                WriteElementClass(blockName, elementName).
                Write("\" ").
                Write(attributes).
                WriteLine(">");
        }

        /// <summary>
        /// Writes <a href="https://en.bem.info/methodology/naming-convention/#element-name">BEM element class</a>, "{<paramref name="blockName"/>}__{<paramref name="elementName"/>}". 
        /// </summary>
        public static HtmlRenderer WriteElementClass(this HtmlRenderer htmlRenderer, string blockName, string elementName)
        {
            return htmlRenderer.
                Write(blockName).
                Write("__").
                Write(elementName);
        }

        /// <summary>
        /// Writes <a href="https://en.bem.info/methodology/quick-start/#key-value">BEM key-value modifier class, " {<paramref name="blockName"/>}_{<paramref name="modifierKey"/>}_{<paramref name="modifierValue"/>}".</a>
        /// </summary>
        public static HtmlRenderer WriteBlockKeyValueModifierClass(this HtmlRenderer htmlRenderer, string blockName, string modifierKey, char modifierValue)
        {
            return htmlRenderer.
                Write(' '). // Never the first class
                Write(blockName).
                Write('_').
                Write(modifierKey).
                Write('_').
                Write(modifierValue);
        }

        /// <summary>
        /// Writes <a href="https://en.bem.info/methodology/quick-start/#key-value">BEM key-value modifier class, " {<paramref name="blockName"/>}_{<paramref name="modifierKey"/>}_{<paramref name="modifierValue"/>}".</a>
        /// </summary>

        public static HtmlRenderer WriteBlockKeyValueModifierClass(this HtmlRenderer htmlRenderer, string blockName, string modifierKey, string modifierValue)
        {
            return htmlRenderer.
                Write(' '). // Never the first class
                Write(blockName).
                Write('_').
                Write(modifierKey).
                Write('_').
                Write(modifierValue);
        }

        /// <summary>
        /// Writes <a href="https://en.bem.info/methodology/quick-start/#key-value">BEM key-value modifier class, " {<paramref name="blockName"/>}__{<paramref name="elementName"/>}_{<paramref name="modifierKey"/>}_{<paramref name="modifierValue"/>}".</a>
        /// </summary>

        public static HtmlRenderer WriteElementKeyValueModifierClass(this HtmlRenderer htmlRenderer,
            string blockName,
            string elementName,
            string modifierKey,
            string modifierValue)
        {
            return htmlRenderer.
                Write(' '). // Never the first class
                WriteElementClass(blockName, elementName).
                Write('_').
                Write(modifierKey).
                Write('_').
                Write(modifierValue);
        }

        /// <summary>
        /// <para>Writes <paramref name="text"/> if <paramref name="condition"/> is true.</para> 
        /// <para>If <paramref name="text"/> is <c>null</c> an empty string is written in its place.</para>
        /// </summary>
        public static HtmlRenderer Write(this HtmlRenderer htmlRenderer,
            bool condition,
            string text)
        {
            if (!condition)
            {
                return htmlRenderer;
            }

            htmlRenderer.Write(text);

            return htmlRenderer;
        }

        /// <summary>
        /// <para>Writes <paramref name="part1"/> and <paramref name="part2"/> sequentially if <paramref name="condition"/> is true.</para>
        /// <para>If <paramref name="part2"/> is <c>null</c>, an empty string is written in its place.</para>
        /// </summary>
        public static HtmlRenderer Write(this HtmlRenderer htmlRenderer,
            bool condition,
            char part1,
            string part2)
        {
            if (!condition)
            {
                return htmlRenderer;
            }

            htmlRenderer.
                Write(part1).
                Write(part2);

            return htmlRenderer;
        }

        /// <summary>
        /// <para>Writes <paramref name="part1"/>, <paramref name="part2"/> and <paramref name="part3"/> sequentially if <paramref name="condition"/> is true.</para>
        /// <para>If a part is <c>null</c>, an empty string is written in its place.</para>
        /// </summary>
        public static HtmlRenderer Write(this HtmlRenderer htmlRenderer,
            bool condition,
            string part1,
            string part2,
            string part3)
        {
            if (!condition)
            {
                return htmlRenderer;
            }

            htmlRenderer.
                Write(part1).
                Write(part2).
                Write(part3);

            return htmlRenderer;
        }

        /// <summary>
        /// <para>Writes <paramref name="part1"/>, <paramref name="part2"/>, <paramref name="part3"/> and <paramref name="part4"/> sequentially if <paramref name="condition"/> is true.</para>
        /// <para>If a part is <c>null</c>, an empty string is written in its place.</para>
        /// </summary>
        public static HtmlRenderer Write(this HtmlRenderer htmlRenderer,
            bool condition,
            char part1,
            string part2,
            string part3,
            string part4)
        {
            if (!condition)
            {
                return htmlRenderer;
            }

            htmlRenderer.
                Write(part1).
                Write(part2).
                Write(part3).
                Write(part4);

            return htmlRenderer;
        }

        /// <summary>
        /// Writes HTML attributes.
        /// </summary>
        /// <param name="htmlRenderer">The renderer to write to.</param>
        /// <param name="attributes">
        /// <para>The attributes to write.</para>
        /// <para>If this value is <c>null</c>, nothing is written.</para>
        /// </param>
        public static HtmlRenderer WriteAttributes(this HtmlRenderer htmlRenderer, ReadOnlyDictionary<string, string> attributes)
        {
            if (attributes == null)
            {
                return htmlRenderer;
            }

            foreach (KeyValuePair<string, string> attribute in attributes)
            {
                htmlRenderer.
                    Write(' ').
                    Write(attribute.Key).
                    Write("=\"").
                    WriteEscape(attribute.Value).
                    Write('"');
            }

            return htmlRenderer;
        }

        /// <summary>
        /// <para>Writes HTML attributes excluding the class attribute.</para>
        /// <para>Why exclude class attributes? HTML attributes are written to root elements of FlexiBlocks. Root elements have at least one default class for consistency with 
        /// <a href="https://en.bem.info/">BEM methodology</a>. Default classes may be generated in real time. To write both default classes and user specified classes efficiently 
        /// (without extra string allocations), renderers may have to write class attributes.</para>
        /// </summary>
        /// <param name="htmlRenderer">The renderer to write to.</param>
        /// <param name="attributes">
        /// <para>The attributes to write.</para>
        /// <para>If this value is <c>null</c>, nothing is written.</para>
        /// </param>
        public static HtmlRenderer WriteAttributesExcludingClass(this HtmlRenderer htmlRenderer, ReadOnlyDictionary<string, string> attributes)
        {
            if (attributes == null)
            {
                return htmlRenderer;
            }

            foreach (KeyValuePair<string, string> attribute in attributes)
            {
                string key = attribute.Key;

                if (key == "class")
                {
                    continue;
                }

                htmlRenderer.
                    Write(' ').
                    Write(key).
                    Write("=\"").
                    WriteEscape(attribute.Value).
                    Write('"');
            }

            return htmlRenderer;
        }

        /// <summary>
        /// Writes HTML attributes excluding the class and ID attributes.
        /// </summary>
        public static HtmlRenderer WriteAttributesExcludingClassAndID(this HtmlRenderer htmlRenderer, ReadOnlyDictionary<string, string> attributes)
        {
            if (attributes == null)
            {
                return htmlRenderer;
            }

            foreach (KeyValuePair<string, string> attribute in attributes)
            {
                string key = attribute.Key;

                if (key == "class" || key == "id")
                {
                    continue;
                }

                htmlRenderer.
                    Write(' ').
                    Write(key).
                    Write("=\"").
                    WriteEscape(attribute.Value).
                    Write('"');
            }

            return htmlRenderer;
        }

        /// <summary>
        /// <para>Conditionally writes an HTML fragment, adding a class attribute to its first tag.</para>
        /// <para>If the HTML fragment has no elements, no class attribute is added.</para>
        /// </summary>
        /// <param name="htmlRenderer">The renderer to write to.</param>
        /// <param name="condition">If this is false, nothing is written.</param>
        /// <param name="htmlFragment">
        /// <para>The HTML fragment to write.</para>
        /// <para>This value must be a valid HTML fragment (https://www.w3.org/TR/html5/syntax) for class attribute addition to work as expected.</para>
        /// </param>
        /// <param name="classValuePart1">
        /// <para>Part 1 of the class attribute's value.</para>
        /// <para>If this value is <c>null</c>, it is not written.</para>
        /// <para>If this value and <paramref name="classValuePart2" /> are both <c>null</c>, no class attribute is written.</para>
        /// </param>
        /// <param name="classValuePart2">
        /// <para>Part 2 of the class attribute's value.</para>
        /// <para>If this value is <c>null</c>, it is not written.</para>
        /// <para>If this value and <paramref name="classValuePart1" /> are both <c>null</c>, no class attribute is written.</para>
        /// </param>
        public static HtmlRenderer WriteHtmlFragmentWithClass(this HtmlRenderer htmlRenderer,
            bool condition,
            string htmlFragment,
            string classValuePart1,
            string classValuePart2)
        {
            if (!condition)
            {
                return htmlRenderer;
            }

            bool writeClassValuePart1 = classValuePart1 != null;
            bool writeClassValuePart2 = classValuePart2 != null;

            if (!writeClassValuePart1 && !writeClassValuePart2)
            {
                return htmlRenderer.Write(htmlFragment);
            }

            (int startIndex, int _, int tagNameEndIndex) = FindFirstTag(htmlFragment);

            // Invalid fragment or fragment has no tags or no class value
            if (startIndex == -1)
            {
                return htmlRenderer.Write(htmlFragment);
            }

            int firstIndexAfterTagName = tagNameEndIndex + 1;

            return htmlRenderer.
                Write(htmlFragment, 0, firstIndexAfterTagName).
                Write(" class=\"").
                Write(classValuePart1).
                Write(classValuePart2).
                Write("\"").
                Write(htmlFragment, firstIndexAfterTagName, htmlFragment.Length - firstIndexAfterTagName);
        }

        /// <summary>
        /// Writes children with the a specified implicit paragraphs setting.
        /// </summary>
        /// <param name="htmlRenderer">The renderer to write to.</param>
        /// <param name="containerBlock">The block containing children to write.</param>
        /// <param name="implicitParagraphs">The boolean value specifying whether or not to render &lt;p&gt; elements.</param>
        public static HtmlRenderer WriteChildren(this HtmlRenderer htmlRenderer, ContainerBlock containerBlock, bool implicitParagraphs)
        {
            bool initialImplicitParagraph = htmlRenderer.ImplicitParagraph;
            htmlRenderer.ImplicitParagraph = implicitParagraphs;

            htmlRenderer.WriteChildren(containerBlock);

            htmlRenderer.ImplicitParagraph = initialImplicitParagraph;

            return htmlRenderer;
        }

        /// <summary>
        /// <para>Finds the first tag in an HTML fragment.</para>
        /// </summary>
        /// <param name="htmlFragment">
        /// <para>The HTML fragment to search.</para>
        /// <para>If this value is not a valid HTML fragment (https://www.w3.org/TR/html5/syntax), the values returned by this method may not be valid.</para>
        /// </param>
        /// <returns>
        /// <para>If <paramref name="htmlFragment"/> contains at least 1 tag, returns (int startIndex, int endIndex, int nameEnd).</para>
        /// <para>Otherwise, returns (-1, -1, -1).</para>
        /// </returns>
        internal static (int startIndex, int endIndex, int nameEnd) FindFirstTag(string htmlFragment)
        {
            int startIndex = -1;
            int nameEnd = -1;
            for (int i = 0; i < htmlFragment.Length; i++)
            {
                char c = htmlFragment[i];

                if (c == '<')
                {
                    startIndex = i;
                }
                else if (startIndex > -1)
                {
                    if (nameEnd == -1 && SPACE_CHARS.Contains(c))
                    {
                        nameEnd = i - 1;
                    }
                    else if (nameEnd == -1 && c == '/')
                    {
                        nameEnd = i - 1; // Self closing tag
                    }
                    else if (c == '>')
                    {
                        return (startIndex, i, nameEnd == -1 ? i - 1 : nameEnd); // Wrong if tag is empty "<>" but that would be invalid HTML
                    }
                }
            }

            // Invalid fragment or fragment has no tags
            return (-1, -1, -1);
        }
    }
}
