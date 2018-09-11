using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks
{
    /// <summary>
    /// <para>An implementation of <see cref="FlexiBlockOptions{T}"/> representing options for a <see cref="FlexiAlertBlock" />.</para>
    /// 
    /// <para>This class is primarily used through the <see cref="FlexiAlertBlocksExtension"/>. To that end, this class is designed to be populated from JSON.
    /// This class may occasionally be created manually for use as the default <see cref="FlexiAlertBlock" /> options, so it accomodates manual creation as well.</para>
    /// 
    /// <para>Markdig is designed to be extensible, as a result, any third party extension can access a FlexiCodeBlock's options. To prevent inconsistent state, 
    /// this class is immutable.</para>
    /// </summary>
    public class FlexiAlertBlockOptions : FlexiBlockOptions<FlexiAlertBlockOptions>
    {
        private const string _defaultClassFormat = "fab-{0}";
        private const string _defaultContentClass = "fab-content";
        private const string _defaultType = "info";

        /// <summary>
        /// Creates a <see cref="FlexiAlertBlockOptions"/> instance.
        /// </summary>
        /// <param name="iconMarkup">
        /// <para>The markup for the <see cref="FlexiAlertBlock" />'s icon.</para>
        /// <para>If the value is null, whitespace or an empty string, an attempt is made to retrieve icon markup for this block's
        /// type from <see cref="FlexiAlertBlocksExtensionOptions"/>, failing which, no icon is rendered.</para>
        /// <para>Defaults to null.</para>
        /// </param>
        /// <param name="classFormat">
        /// <para>The format for the <see cref="FlexiAlertBlock" />'s outermost element's class.</para>
        /// <para>The <see cref="FlexiAlertBlock" />'s type will replace "{0}" in the format.</para> 
        /// <para>If the value is null, whitespace or an empty string, no class is assigned.</para>
        /// <para>Defaults to "fab-{0}".</para>
        /// </param>
        /// <param name="contentClass">
        /// <para>The class of the <see cref="FlexiAlertBlock" />'s content wrapper.</para>  
        /// <para>If the value is null, whitespace or an empty string, no class is assigned.</para>
        /// <para>Defaults to "fab-content".</para>
        /// </param>
        /// <param name="type">
        /// <para>The <see cref="FlexiAlertBlock"/>'s type.</para>
        /// <para>If the value is null, whitespace or an empty string, the <see cref="FlexiAlertBlock"/> will have no type.</para>
        /// <para>Defaults to "info".</para>
        /// </param>
        /// <param name="attributes">
        /// <para>The HTML attributes for the <see cref="FlexiAlertBlock"/>'s outermost element.</para>
        /// <para>If this dictionary is null, no attributes will be assigned to the outermost element.</para>
        /// <para>Defaults to null.</para>
        /// </param>
        public FlexiAlertBlockOptions(
            string iconMarkup = default,
            string classFormat = _defaultClassFormat,
            string contentClass = _defaultContentClass,
            string type = _defaultType,
            IDictionary<string, string> attributes = default) : base(attributes)
        {
            IconMarkup = iconMarkup;
            ClassFormat = classFormat;
            ContentClass = contentClass;
            Type = type;

            ValidateAndPopulate();
        }

        /// <summary>
        /// Gets or sets the markup for the <see cref="FlexiAlertBlock" />'s icon.
        /// </summary>
        [JsonProperty]
        public string IconMarkup { get; internal set; }

        /// <summary>
        /// Gets or sets the format for the <see cref="FlexiAlertBlock" />'s outermost element's class.
        /// </summary>
        [JsonProperty]
        public string ClassFormat { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="FlexiAlertBlock" />'s outermost element's class.
        /// </summary>
        public string Class { get; private set; }

        /// <summary>
        /// Gets or sets the class of the <see cref="FlexiAlertBlock" />'s content wrapper.  
        /// </summary>
        [JsonProperty]
        public string ContentClass { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="FlexiAlertBlock"/>'s type.
        /// </summary>
        [JsonProperty]
        public string Type { get; private set; }

        /// <summary>
        /// Validates options and populates generated properties.
        /// </summary>
        /// <exception cref="FlexiBlocksException">Thrown if <see cref="ClassFormat"/> is an invalid format.</exception>
        protected override void ValidateAndPopulate()
        {
            if (!string.IsNullOrWhiteSpace(ClassFormat) &&
                !string.IsNullOrWhiteSpace(Type))
            {
                try
                {
                    Class = string.Format(ClassFormat, Type);
                }
                catch (FormatException formatException)
                {
                    throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_InvalidFormat, nameof(ClassFormat), ClassFormat),
                        formatException);
                }
            }
            else
            {
                // When this object is populated from JSON, and ClassFormat or AlertType is set to null, any existing Class value must
                // be discarded.
                Class = null;
            }
        }
    }
}
