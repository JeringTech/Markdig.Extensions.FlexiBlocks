using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks
{
    /// <summary>
    /// <para>An implementation of <see cref="FlexiBlockOptions{T}"/> representing options for a <see cref="FlexiSectionBlock" />.</para>
    /// 
    /// <para>This class is primarily used through the <see cref="FlexiOptionsBlocksExtension"/>. To that end, this class is designed to be populated from JSON.
    /// This class may occasionally be created manually for use as the default <see cref="FlexiSectionBlock" /> options, so it accomodates manual creation as well.</para>
    /// 
    /// <para>Markdig is designed to be extensible, as a result, any third party extension can access a FlexiCodeBlock's options. To prevent inconsistent state, 
    /// this class is immutable.</para>
    /// </summary>
    public class FlexiSectionBlockOptions : FlexiBlockOptions<FlexiSectionBlockOptions>
    {
        private const string _defaultLinkIconMarkup = "<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M17 7h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8zm9-4h-4v2h4c1.65 0 3 1.35 3 3s-1.35 3-3 3h-4v2h4c2.76 0 5-2.24 5-5s-2.24-5-5-5zm-6 8H7c-1.65 0-3-1.35-3-3s1.35-3 3-3h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-2zm-3-4h8v2H8z\"/></svg>";
        private const string _defaultClassFormat = "section-level-{0}";
        private const bool _defaultGenerateIdentifier = true;
        private const bool _defaultAutoLinkable = true;
        private const SectioningContentElement _defaultWrapperElement = SectioningContentElement.Section;

        /// <summary>
        /// Creates a <see cref="FlexiSectionBlockOptions"/> instance.
        /// </summary>
        /// <param name="element">
        /// <para>The sectioning content element used as the outermost element of the <see cref="FlexiSectionBlock"/>.</para>
        /// <para>Defaults to <see cref="SectioningContentElement.Section"/>.</para>
        /// </param>
        /// <param name="generateIdentifier">
        /// <para>The boolean value specifying whether or not an ID should be generated for the <see cref="FlexiSectionBlock"/>'s 
        /// outermost element.</para>
        /// <para>If this value is true, an ID will be generated from the <see cref="FlexiSectionBlock"/>'s header's content. 
        /// Otherwise, no ID will be generated.</para>
        /// <para>Defaults to true.</para>
        /// </param>
        /// <param name="autoLinkable">
        /// <para>The boolean value specifying whether or not the <see cref="FlexiSectionBlock"/> should be linkable to using its
        /// header's content (auto-linkable).</para>
        /// <para>If this value is true and the <see cref="FlexiSectionBlock"/>'s outermost element has an ID, enables auto-linking for
        /// the <see cref="FlexiSectionBlock"/>. Otherwise, auto-linking will be disabled.</para>
        /// <para>Defaults to true.</para>
        /// </param>
        /// <param name="classFormat">
        /// <para>The format for the <see cref="FlexiSectionBlock" />'s outermost element's class.</para>
        /// <para>The <see cref="FlexiSectionBlock" />'s level will replace "{0}" in the format.</para> 
        /// <para>If this value is null, whitespace or an empty string, no class is assigned.</para>
        /// <para>Defaults to "section-level-{0}".</para> 
        /// </param>
        /// <param name="linkIconMarkup">
        /// <para>The markup for the <see cref="FlexiSectionBlock"/>'s link icon.</para>
        /// <para>If this value is null, whitespace or an empty string, no copy icon is rendered.</para>
        /// <para>Defaults to https://material.io/tools/icons/?icon=link&amp;style=sharp, licensed under an Apache License Version 2 license - https://www.apache.org/licenses/LICENSE-2.0.html.</para>
        /// </param>
        /// <param name="attributes">
        /// <para>The HTML attributes for the <see cref="FlexiSectionBlock"/>'s outermost element.</para>
        /// <para>If this value is null, no attributes will be assigned to the outermost element.</para>
        /// <para>Defaults to null.</para>
        /// </param>
        public FlexiSectionBlockOptions(
            SectioningContentElement element = _defaultWrapperElement,
            bool generateIdentifier = _defaultGenerateIdentifier,
            bool autoLinkable = _defaultAutoLinkable,
            string classFormat = _defaultClassFormat,
            string linkIconMarkup = _defaultLinkIconMarkup,
            IDictionary<string, string> attributes = default) : base(attributes)
        {
            LinkIconMarkup = linkIconMarkup;
            ClassFormat = classFormat;
            GenerateIdentifier = generateIdentifier;
            AutoLinkable = autoLinkable;
            Element = element;

            ValidateAndPopulate();
        }

        /// <summary>
        /// Gets or sets the sectioning content element used as the outermost element of the <see cref="FlexiSectionBlock"/>.
        /// </summary>
        [JsonProperty]
        public SectioningContentElement Element { get; private set; }

        /// <summary>
        /// Gets or sets the boolean value specifying whether or not an ID should be generated for the <see cref="FlexiSectionBlock"/>'s 
        /// outermost element.
        /// </summary>
        [JsonProperty]
        public bool GenerateIdentifier { get; private set; }

        /// <summary>
        /// Gets or sets the boolean value specifying whether or not the <see cref="FlexiSectionBlock"/> should be linkable to using its
        /// header's content (auto-linkable).
        /// </summary>
        [JsonProperty]
        public bool AutoLinkable { get; private set; }

        /// <summary>
        /// Gets or sets the format for the <see cref="FlexiSectionBlock" />'s outermost element's class.
        /// </summary>
        [JsonProperty]
        public string ClassFormat { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="FlexiSectionBlock" />'s outermost element's class.
        /// </summary>
        public string Class { get; internal set; }

        /// <summary>
        /// Gets or sets the markup for the <see cref="FlexiSectionBlock"/>'s link icon.
        /// </summary>
        [JsonProperty]
        public string LinkIconMarkup { get; private set; }

        /// <summary>
        /// Validates options and populates generated properties.
        /// </summary>
        /// <exception cref="FlexiBlocksException"></exception>
        protected override void ValidateAndPopulate()
        {
            if (!Enum.IsDefined(typeof(SectioningContentElement), Element))
            {
                throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_OptionMustBeAValidEnumValue,
                        Element,
                        nameof(Element),
                        nameof(SectioningContentElement)));
            }
        }
    }
}
