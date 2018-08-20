using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    /// <summary>
    /// Contains options for including content.
    /// </summary>
    public class IncludeOptions
    {
        /// <summary>
        /// Creates an <see cref="IncludeOptions"/> instance. Validates arguments.
        /// </summary>
        /// <param name="source">The source of the content to include. This argument should either be a relative URI or an absolute URI with scheme 
        /// file, HTTP or HTTPS.</param>
        /// <param name="contentType">The type of the content to include.</param>
        /// <param name="clippingAreas">The list of clipping areas to use when clipping the content.</param>
        /// <param name="cacheOnDisk">The boolean value specifying whether or not to cache content on disk. True if content should be cached on disk,
        /// false otherwise. Only remote content (retrieved using HTTP or HTTPS) is affected by this argument since local content is already on disk.</param>
        public IncludeOptions(string source,
            ContentType contentType = ContentType.Code,
            List<ClippingArea> clippingAreas = null,
            bool cacheOnDisk = true)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentException(string.Format(Strings.ArgumentException_MustBeDefined, nameof(source)));
            }

            if (!Enum.IsDefined(typeof(ContentType), contentType))
            {
                throw new ArgumentException(string.Format(Strings.ArgumentException_InvalidEnumArgument,
                    nameof(contentType),
                    (int)contentType,
                    nameof(ContentType)));
            }

            Source = source;
            ContentType = contentType;
            ClippingAreas = clippingAreas;
            CacheOnDisk = cacheOnDisk;
        }

        /// <summary>
        /// Gets the source of the content to include.
        /// </summary>
        public string Source { get; }

        /// <summary>
        /// Gets the type of the content to include.
        /// </summary>
        public ContentType ContentType { get; }

        /// <summary>
        /// Gets the list of clipping areas to use when clipping the content.
        /// </summary>
        public List<ClippingArea> ClippingAreas { get; }

        /// <summary>
        /// Gets the boolean value specifying whether or not to cache content on disk. 
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)] // If CacheOnDisk isn't specified in JSON, the constructor arg cacheOnDisk is set to true
        public bool CacheOnDisk { get; }
    }
}
