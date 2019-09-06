using Markdig.Renderers;
using Markdig.Syntax;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace Jering.Markdig.Extensions.FlexiBlocks
{
    /// <summary>
    /// <para>Extensions for <see cref="HtmlRenderer"/>.</para>
    /// <para>Overloads are used except when different methods would have the same signatures, e.g a WriteStartTag method that take an attributes string and 
    /// one that takes a classes string.</para>
    /// </summary>
    public static class HtmlRendererExtensions
    {
        /// <summary>
        /// https://www.w3.org/TR/html5/infrastructure.html#space-characters
        /// </summary>
        private static readonly ImmutableHashSet<char> _spaceChars = ImmutableHashSet.Create(new char[] { ' ', '\t', '\r', '\n', '\f' });

        private static readonly char[] _digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        /// <summary>
        /// Writes an integer.
        /// </summary>
        public static HtmlRenderer WriteInt(this HtmlRenderer htmlRenderer, int integer)
        {
            if (integer < 0)
            {
                htmlRenderer.Write('-');
                integer = -integer;
            }

            // https://stackoverflow.com/questions/4483886/how-can-i-get-a-count-of-the-total-number-of-digits-in-a-number
            int firstDivisor;
            if (integer < 10)
            {
                firstDivisor = 1;
            }
            else if (integer < 100)
            {
                firstDivisor = 10;
            }
            else if (integer < 1000)
            {
                firstDivisor = 100;
            }
            else if (integer < 10000)
            {
                firstDivisor = 1000;
            }
            else if (integer < 100000)
            {
                firstDivisor = 10000;
            }
            else if (integer < 1000000)
            {
                firstDivisor = 100000;
            }
            else if (integer < 10000000)
            {
                firstDivisor = 1000000;
            }
            else if (integer < 100000000)
            {
                firstDivisor = 10000000;
            }
            else if (integer < 1000000000)
            {
                firstDivisor = 100000000;
            }
            else
            {
                firstDivisor = 1000000000;
            }

            for (int i = firstDivisor; i > 0; i /= 10)
            {
                htmlRenderer.Write(_digits[integer / i]);
                integer %= i;
            }

            return htmlRenderer;
        }

        /// <summary>
        /// Writes and HTML attribute.
        /// </summary>
        public static HtmlRenderer WriteAttribute(this HtmlRenderer htmlRenderer, string attributeName, string value)
        {
            return htmlRenderer.
                Write(' '). // Never the first attribute (all elements have class attribute)
                Write(attributeName).
                Write("=\"").
                Write(value).
                Write('"');
        }

        /// <summary>
        /// Writes and HTML attribute conditionally.
        /// </summary>
        public static HtmlRenderer WriteAttribute(this HtmlRenderer htmlRenderer, bool condition, string attributeName, string value)
        {
            return condition ? htmlRenderer.WriteAttribute(attributeName, value) : htmlRenderer;
        }

        /// <summary>
        /// Writes an HTML attribute value if it exists in a dictionary of attributes.
        /// </summary>
        public static HtmlRenderer WriteAttributeValue(this HtmlRenderer htmlRenderer, ReadOnlyDictionary<string, string> attributes, string attributeKey)
        {
            string value = null;

            return attributes?.TryGetValue(attributeKey, out value) == true ? htmlRenderer.Write(' ').Write(value) : htmlRenderer;
        }

        /// <summary>
        /// Writes HTML attributes.
        /// </summary>
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
        /// Writes HTML attributes excluding the specified attribute.
        /// </summary>
        public static HtmlRenderer WriteAttributesExcept(this HtmlRenderer htmlRenderer, ReadOnlyDictionary<string, string> attributes, string excluded)
        {
            return htmlRenderer.WriteAttributesExcept(attributes, excluded, null); // Keys can't be null (Dictionary throws if you try to add a key-value pair with null key)
        }

        /// <summary>
        /// Writes HTML attributes excluding the specified attributes.
        /// </summary>
        public static HtmlRenderer WriteAttributesExcept(this HtmlRenderer htmlRenderer, ReadOnlyDictionary<string, string> attributes, string excluded1, string excluded2)
        {
            if (attributes == null)
            {
                return htmlRenderer;
            }

            foreach (KeyValuePair<string, string> attribute in attributes)
            {
                string key = attribute.Key;

                if (key == excluded1 || key == excluded2)
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
        /// If <paramref name="hasFeature"/> is true, writes " {<paramref name="blockName"/>}_has_{<paramref name="featureName"/>}". 
        /// Otherwise, writes " {<paramref name="blockName"/>}_no_{<paramref name="featureName"/>}". 
        /// </summary>
        public static HtmlRenderer WriteHasFeatureClass(this HtmlRenderer htmlRenderer, bool hasFeature, string blockName, string featureName)
        {
            return htmlRenderer.WriteBlockKeyValueModifierClass(blockName, hasFeature ? "has" : "no", featureName);
        }

        /// <summary>
        /// If <paramref name="hasFeature"/> is true, writes " {<paramref name="blockName"/>}__{<paramref name="elementName"/>}_has_{<paramref name="featureName"/>}". 
        /// Otherwise, writes " {<paramref name="blockName"/>}__{<paramref name="elementName"/>}_no_{<paramref name="featureName"/>}". 
        /// </summary>
        public static HtmlRenderer WriteHasFeatureClass(this HtmlRenderer htmlRenderer, bool hasFeature, string blockName, string elementName, string featureName)
        {
            return htmlRenderer.WriteElementKeyValueModifierClass(blockName, elementName, hasFeature ? "has" : "no", featureName);
        }

        /// <summary>
        /// If <paramref name="isType"/> is true, writes " {<paramref name="blockName"/>}__{<paramref name="elementName"/>}_is_{<paramref name="typeName"/>}". 
        /// </summary>
        public static HtmlRenderer WriteIsTypeClass(this HtmlRenderer htmlRenderer, bool isType, string blockName, string elementName, string typeName)
        {
            return isType ? htmlRenderer.WriteElementKeyValueModifierClass(blockName, elementName, "is", typeName) : htmlRenderer;
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
                WriteEndTag(tagName).
                WriteLine();
        }

        /// <summary>
        /// Writes "&lt;/{<paramref name="tagName"/>}&gt;\n" if <paramref name="condition"/> is true.
        /// </summary>
        public static HtmlRenderer WriteEndTagLine(this HtmlRenderer htmlRenderer, bool condition, string tagName)
        {
            return condition ? htmlRenderer.WriteEndTagLine(tagName) : htmlRenderer;
        }

        /// <summary>
        /// Writes "&lt;{<paramref name="tagName"/>} class=\"{<paramref name="blockName"/>}__{<paramref name="elementName"/>}\"&gt;".
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
        /// Writes "&lt;{<paramref name="tagName"/>} class=\"{<paramref name="blockName"/>}__{<paramref name="elementName"/>}\"&gt;\n".
        /// </summary>
        public static HtmlRenderer WriteStartTagLine(this HtmlRenderer htmlRenderer,
            string tagName,
            string blockName,
            string elementName)
        {
            return htmlRenderer.
                WriteStartTag(tagName, blockName, elementName).
                WriteLine();
        }

        /// <summary>
        /// Writes "&lt;{<paramref name="tagName"/>} class=\"{<paramref name="blockName"/>}__{<paramref name="elementName"/>} {<paramref name="classes"/>}\"&gt;".
        /// </summary>
        public static HtmlRenderer WriteStartTagWithClasses(this HtmlRenderer htmlRenderer,
            string tagName,
            string blockName,
            string elementName,
            string classes)
        {
            return htmlRenderer.
                Write('<').
                Write(tagName).
                Write(" class=\"").
                WriteElementClass(blockName, elementName).
                Write(' ').
                Write(classes).
                Write("\">");
        }

        /// <summary>
        /// Writes "&lt;{<paramref name="tagName"/>} class=\"{<paramref name="blockName"/>}__{<paramref name="elementName"/>} {<paramref name="classes"/>}\"&gt;\n".
        /// </summary>
        public static HtmlRenderer WriteStartTagLineWithClasses(this HtmlRenderer htmlRenderer,
            string tagName,
            string blockName,
            string elementName,
            string classes)
        {
            return htmlRenderer.
                WriteStartTagWithClasses(tagName, blockName, elementName, classes).
                WriteLine();
        }

        /// <summary>
        /// Writes "&lt;{<paramref name="tagName"/>} class=\"{<paramref name="blockName"/>}__{<paramref name="elementName"/>}\" {<paramref name="attributes"/>}&gt;".
        /// </summary>
        public static HtmlRenderer WriteStartTagWithAttributes(this HtmlRenderer htmlRenderer,
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
                Write(">");
        }

        /// <summary>
        /// Writes "&lt;{<paramref name="tagName"/>} class=\"{<paramref name="blockName"/>}__{<paramref name="elementName"/>}\" {<paramref name="attributes"/>}&gt;\n".
        /// </summary>
        public static HtmlRenderer WriteStartTagLineWithAttributes(this HtmlRenderer htmlRenderer,
            string tagName,
            string blockName,
            string elementName,
            string attributes)
        {
            return htmlRenderer.
                WriteStartTagWithAttributes(tagName, blockName, elementName, attributes).
                WriteLine();
        }

        /// <summary>
        /// Writes "&lt;{<paramref name="tagName"/>} class=\"{<paramref name="blockName"/>}__{<paramref name="elementName"/>} {<paramref name="blockName"/>}__{<paramref name="elementName"/>}_{<paramref name="modifier"/>}\" {<paramref name="attributes"/>}&gt;".
        /// </summary>
        public static HtmlRenderer WriteStartTagWithModifierClassAndAttributes(this HtmlRenderer htmlRenderer,
            string tagName,
            string blockName,
            string elementName,
            string modifier,
            string attributes)
        {
            return htmlRenderer.
                Write('<').
                Write(tagName).
                Write(" class=\"").
                WriteElementClass(blockName, elementName).
                WriteElementModifierClass(blockName, elementName, modifier).
                Write("\" ").
                Write(attributes).
                Write(">");
        }

        /// <summary>
        /// Writes "&lt;{<paramref name="tagName"/>} class=\"{<paramref name="blockName"/>}__{<paramref name="elementName"/>} {<paramref name="blockName"/>}__{<paramref name="elementName"/>}_{<paramref name="modifier"/>}\" {<paramref name="attributes"/>}&gt;\n".
        /// </summary>
        public static HtmlRenderer WriteStartTagLineWithModifierClassAndAttributes(this HtmlRenderer htmlRenderer,
            string tagName,
            string blockName,
            string elementName,
            string modifier,
            string attributes)
        {
            return htmlRenderer.
                WriteStartTagWithModifierClassAndAttributes(tagName, blockName, elementName, modifier, attributes).
                WriteLine();
        }

        /// <summary>
        /// Writes "&lt;{<paramref name="tagName"/>} class=\"{<paramref name="blockName"/>}__{<paramref name="elementName"/>} {<paramref name="blockName"/>}__{<paramref name="elementName"/>}_{<paramref name="modifier"/>}\"&gt;\n".
        /// </summary>
        public static HtmlRenderer WriteStartTagLineWithModifierClass(this HtmlRenderer htmlRenderer,
            string tagName,
            string blockName,
            string elementName,
            string modifier)
        {
            return htmlRenderer.
                Write('<').
                Write(tagName).
                Write(" class=\"").
                WriteElementClass(blockName, elementName).
                WriteElementModifierClass(blockName, elementName, modifier).
                WriteLine("\">");
        }

        /// <summary>
        /// Writes "&lt;{<paramref name="tagName"/>} class=\"{<paramref name="blockName"/>__<paramref name="elementName"/>} {<paramref name="classes"/>}\" {<paramref name="attributes"/>}&gt;\n".
        /// </summary>
        public static HtmlRenderer WriteStartTagLineWithClassesAndAttributes(this HtmlRenderer htmlRenderer,
            string tagName,
            string blockName,
            string elementName,
            string classes,
            string attributes)
        {
            return htmlRenderer.
                Write('<').
                Write(tagName).
                Write(" class=\"").
                WriteElementClass(blockName, elementName).
                Write(' ').
                Write(classes).
                Write("\" ").
                Write(attributes).
                WriteLine(">");
        }

        /// <summary>
        /// Writes <a href="https://en.bem.info/methodology/naming-convention/#element-name">BEM element class</a>, " {<paramref name="blockName"/>}__{<paramref name="elementName"/>}". 
        /// </summary>
        public static HtmlRenderer WriteElementClass(this HtmlRenderer htmlRenderer, string blockName, string elementName)
        {
            return htmlRenderer.
                Write(blockName).
                Write("__").
                Write(elementName);
        }

        /// <summary>
        /// Writes <a href="https://en.bem.info/methodology/naming-convention/#element-modifier-name">BEM element modifier class</a>, " {<paramref name="blockName"/>}__{<paramref name="elementName"/>}_{<paramref name="modifier"/>}". 
        /// </summary>
        public static HtmlRenderer WriteElementModifierClass(this HtmlRenderer htmlRenderer, string blockName, string elementName, string modifier)
        {
            return htmlRenderer.
                Write(' '). // Never the first class
                WriteElementClass(blockName, elementName).
                Write('_').
                Write(modifier);
        }

        /// <summary>
        /// Writes <a href="https://en.bem.info/methodology/naming-convention/#element-modifier-name">BEM element modifier class</a>, " {<paramref name="blockName"/>}__{<paramref name="elementName"/>}_{<paramref name="modifier"/>}" if condition is true. 
        /// </summary>
        public static HtmlRenderer WriteElementModifierClass(this HtmlRenderer htmlRenderer, bool condition, string blockName, string elementName, string modifier)
        {
            return condition ? htmlRenderer.WriteElementModifierClass(blockName, elementName, modifier) : htmlRenderer;
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
                WriteElementModifierClass(blockName, elementName, modifierKey).
                Write('_').
                Write(modifierValue);
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
        /// <para>Writes <paramref name="part"/> if <paramref name="condition"/> is true.</para> 
        /// <para>If <paramref name="part"/> is <c>null</c> an empty string is written in its place.</para>
        /// </summary>
        public static HtmlRenderer Write(this HtmlRenderer htmlRenderer,
            bool condition,
            string part)
        {
            return condition ? htmlRenderer.Write(part) : htmlRenderer;
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
            return condition ? htmlRenderer.Write(part1).Write(part2) :  htmlRenderer;
        }

        /// <summary>
        /// Writes <paramref name="part1"/> and <paramref name="part2"/> sequentially if <paramref name="condition"/> is true.
        /// </summary>
        public static HtmlRenderer Write(this HtmlRenderer htmlRenderer,
            bool condition,
            string part1,
            string part2)
        {
            return condition ? htmlRenderer.Write(part1).Write(part2) : htmlRenderer;
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
        /// <para>Conditionally writes an HTML fragment, adding a class attribute to its first tag.</para>
        /// <para>If the HTML fragment has no elements, no class attribute is added.</para>
        /// </summary>
        public static HtmlRenderer WriteHtmlFragment(this HtmlRenderer htmlRenderer,
            bool condition,
            string htmlFragment,
            string blockName,
            string elementName)
        {
            if (!condition)
            {
                return htmlRenderer;
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
                WriteElementClass(blockName, elementName).
                Write("\"").
                Write(htmlFragment, firstIndexAfterTagName, htmlFragment.Length - firstIndexAfterTagName);
        }

        /// <summary>
        /// Writes children with the specified implicit paragraphs setting.
        /// </summary>
        public static HtmlRenderer WriteChildren(this HtmlRenderer htmlRenderer, ContainerBlock containerBlock, bool implicitParagraphs)
        {
            bool initialImplicitParagraph = htmlRenderer.ImplicitParagraph;
            htmlRenderer.ImplicitParagraph = implicitParagraphs;

            htmlRenderer.WriteChildren(containerBlock);

            htmlRenderer.ImplicitParagraph = initialImplicitParagraph;

            return htmlRenderer;
        }

        /// <summary>
        /// Writes <see cref="LeafBlock.Inline"/> with the specified enable HTML for inlinesetting.
        /// </summary>
        public static HtmlRenderer WriteLeafInline(this HtmlRenderer htmlRenderer, LeafBlock leafBlock, bool enableHtmlForInline)
        {
            bool initialEnableHtmlForInline = htmlRenderer.EnableHtmlForInline;
            htmlRenderer.EnableHtmlForInline = enableHtmlForInline;

            htmlRenderer.WriteLeafInline(leafBlock);

            htmlRenderer.EnableHtmlForInline = initialEnableHtmlForInline;

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
                    if (nameEnd == -1 && _spaceChars.Contains(c))
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
