using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

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
        private const SectioningContentElement _defaultElement = SectioningContentElement.Section;
        private const bool _defaultGenerateIdentifier = true;
        private const bool _defaultAutoLinkable = true;
        private const string _defaultClassFormat = "flexi-section-block-{0}";
        private const string _defaultLinkIconMarkup = Icons.MATERIAL_DESIGN_LINK;

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
        /// <para>Defaults to "flexi-section-block-{0}".</para> 
        /// </param>
        /// <param name="linkIconMarkup">
        /// <para>The markup for the <see cref="FlexiSectionBlock"/>'s link icon.</para>
        /// <para>If this value is null, whitespace or an empty string, no link icon is rendered.</para>
        /// <para>Defaults to the material design link icon - https://material.io/tools/icons/?icon=link&amp;style=baseline.</para>
        /// </param>
        /// <param name="attributes">
        /// <para>The HTML attributes for the <see cref="FlexiSectionBlock"/>'s outermost element.</para>
        /// <para>If this value is null, no attributes will be assigned to the outermost element.</para>
        /// <para>Defaults to null.</para>
        /// </param>
        public FlexiSectionBlockOptions(
            SectioningContentElement element = _defaultElement,
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
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public SectioningContentElement Element { get; private set; }

        /// <summary>
        /// Gets or sets the boolean value specifying whether or not an ID should be generated for the <see cref="FlexiSectionBlock"/>'s 
        /// outermost element.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public bool GenerateIdentifier { get; private set; }

        /// <summary>
        /// Gets or sets the boolean value specifying whether or not the <see cref="FlexiSectionBlock"/> should be linkable to using its
        /// header's content (auto-linkable).
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public bool AutoLinkable { get; private set; }

        /// <summary>
        /// Gets or sets the format for the <see cref="FlexiSectionBlock" />'s outermost element's class.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public string ClassFormat { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="FlexiSectionBlock" />'s outermost element's class.
        /// </summary>
        public string Class { get; internal set; }

        /// <summary>
        /// Gets or sets the markup for the <see cref="FlexiSectionBlock"/>'s link icon.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public string LinkIconMarkup { get; private set; }

        /// <summary>
        /// Validates options and populates generated properties.
        /// </summary>
        /// <exception cref="FlexiBlocksException"></exception>
        protected override void ValidateAndPopulate()
        {
            if (!Enum.IsDefined(typeof(SectioningContentElement), Element))
            {
                throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_Shared_OptionMustBeAValidEnumValue,
                        Element,
                        nameof(Element),
                        nameof(SectioningContentElement)));
            }
        }
    }
}
