using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Jering.Markdig.Extensions.FlexiBlocks
{
    /// <summary>
    /// An abstraction representing options for a FlexiBlock.
    /// </summary>
    /// <typeparam name="T">The type of the FlexiBlock.</typeparam>
    public abstract class FlexiBlockOptions<T>
    {
        /// <summary>
        /// Gets or sets the HTML attributes for the outermost element of the FlexiBlock.
        /// </summary>
        [JsonProperty]
        public ReadOnlyDictionary<string, string> Attributes { get; private set; }

        /// <summary>
        /// Creates a FlexiBlockOptions instance.
        /// </summary>
        /// <param name="attributes">
        /// <para>The HTML attributes for the outermost element of the FlexiBlock.</para>
        /// </param>
        protected FlexiBlockOptions(IDictionary<string, string> attributes)
        {
            Attributes = attributes == null ? null : new ReadOnlyDictionary<string, string>(attributes);
        }

        /// <summary>
        /// Validates options and populates generated properties.
        /// </summary>
        protected virtual void ValidateAndPopulate()
        {
            // Do nothing by default
        }

        /// <summary>
        /// <para>Validates options and populates generated properties.</para>
        /// <para>FlexiBlockOptions are typically cloned from a default instance then customized using JSON. When the default instance is created,
        /// it must be validated, and generated properties must be populated. When a clone is customized using JSON, it must be
        /// re-validated and its generated properties must be re-populated.</para>
        /// <para>This method facilitates running of the same validation and generated property population logic both in the constructor and after customization using JSON.</para>
        /// <para>The <see cref="OnDeserializedAttribute"/> indicates to JSON.NET that this method should be called immediately after deserialization.</para>
        /// <para>This method is defined here because virtual methods cannot be marked with the <see cref="OnDeserializedAttribute"/> and
        /// implementations of abstract methods (or interface members) are always virtual.</para>
        /// </summary>
        [OnDeserialized]
        private void ValidateAndPopulateWrapper(StreamingContext _)
        {
            ValidateAndPopulate();
        }

        /// <summary>
        /// Returns a shallow clone.
        /// </summary>
        public T Clone()
        {
            return (T)MemberwiseClone();
        }
    }
}
