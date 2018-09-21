using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    /// <summary>
    /// Represents options for a <see cref="FlexiIncludeBlock" />.
    /// </summary>
    public class FlexiIncludeBlockOptions : FlexiBlockOptions<FlexiIncludeBlockOptions>
    {
        // We only support a subset of schemes. For the full list of schemes, see https://docs.microsoft.com/en-sg/dotnet/api/system.uri.scheme?view=netstandard-2.0#System_Uri_Scheme
        private static readonly string[] _supportedSchemes = new string[] { "file", "http", "https" };
        private const string _defaultSourceUri = "";
        private const IncludeType _defaultIncludeType = IncludeType.Code;
        private const bool _defaultCacheOnDisk = true;

        /// <summary>
        /// Creates a <see cref="FlexiIncludeBlockOptions"/> instance. 
        /// </summary>
        /// <param name="sourceUri">
        /// <para>The URI of the source.</para>
        /// <para>This value must either be a relative URI or an absolute URI with scheme file, http or https.</para>
        /// <para>Defaults to <see cref="string.Empty"/>.</para>
        /// </param>
        /// <param name="baseUri">
        /// <para>The base for the source URI.</para>
        /// <para>This option is only relevant if the source URI is a relative URI.</para>
        /// <para>If this value is null, whitespace or an empty string, the application's current directory is used instead.</para>
        /// <para>Otherwise, it must be an absolute URI with scheme file, http or https.</para>
        /// <para>Defaults to null.</para>
        /// </param>
        /// <param name="type">
        /// <para>The <see cref="FlexiIncludeBlock"/>'s type.</para>
        /// <para>If this value is <see cref="IncludeType.Code"/>, a single code block containing the included content is generated. 
        /// If this value is <see cref="IncludeType.Markdown"/>, included content is processed as markdown.</para>
        /// <para>Defaults to <see cref="IncludeType.Code"/>.</para>
        /// </param>
        /// <param name="cacheOnDisk">
        /// <para>The boolean value specifying whether or not to cache sources on disk.</para>
        /// <para>If this value is true, sources will be cached on disk, otherwise, content will not be cached on disk.</para>
        /// <para>On-disk caching only applies to remote sources (retrieved using HTTP or HTTPS) since local sources (retrieved from the file system) are already on disk.</para>
        /// <para>Defaults to true.</para>
        /// </param>
        /// <param name="diskCacheDirectory">
        /// <para>The directory for the on-disk source cache.</para>
        /// <para>This option is only relevant if caching on disk is enabled.</para>
        /// <para>If this value is null, whitespace or an empty string, a folder named "SourceCache" in the application's current directory is used instead.</para>
        /// <para>Defaults to null.</para>
        /// </param>
        /// <param name="clippings">
        /// <para>The list of clippings from the source to include.</para>
        /// <para>If this value is null or empty, the entire source is included.</para>
        /// <para>Defaults to null.</para>
        /// </param>
        public FlexiIncludeBlockOptions(string sourceUri = _defaultSourceUri,
            string baseUri = default,
            IncludeType type = _defaultIncludeType,
            bool cacheOnDisk = _defaultCacheOnDisk,
            string diskCacheDirectory = default,
            IList<Clipping> clippings = default) : base(null)
        {
            SourceUri = sourceUri;
            BaseUri = baseUri;
            Type = type;
            CacheOnDisk = cacheOnDisk;
            DiskCacheDirectory = diskCacheDirectory;
            Clippings = clippings == null ? null : new ReadOnlyCollection<Clipping>(clippings);

            ValidateAndPopulate();
        }

        /// <summary>
        /// Gets the URI of the source.
        /// </summary>
        [JsonProperty]
        public string SourceUri { get; private set; }

        /// <summary>
        /// Gets the base for the source URI.
        /// </summary>
        [JsonProperty]
        public string BaseUri { get; private set; }

        /// <summary>
        /// Gets the normalized URI of the source.
        /// </summary>
        public Uri NormalizedSourceUri { get; private set; }

        /// <summary>
        /// Gets the <see cref="FlexiIncludeBlock"/>'s type.
        /// </summary>
        [JsonProperty]
        public IncludeType Type { get; private set; }

        /// <summary>
        /// Gets the boolean value specifying whether or not to cache content on disk.
        /// </summary>
        [JsonProperty]
        public bool CacheOnDisk { get; private set; }

        /// <summary>
        /// Gets the directory for the on-disk source cache.
        /// </summary>
        [JsonProperty]
        public string DiskCacheDirectory { get; private set; }

        /// <summary>
        /// <para>Gets the resolved directory for the on-disk source cache.</para>
        /// <para>This value takes <see cref="CacheOnDisk"/> into account.</para>
        /// </summary>
        public string ResolvedDiskCacheDirectory { get; private set; }

        /// <summary>
        /// Gets the list of clippings from the source to include.
        /// </summary>
        [JsonProperty]
        public ReadOnlyCollection<Clipping> Clippings { get; private set; }

        /// <summary>
        /// Validates options and populates generated properties.
        /// </summary>
        /// <exception cref="FlexiBlocksException">Thrown if <see cref="SourceUri"/> is null.</exception>
        /// <exception cref="FlexiBlocksException">Thrown if <see cref="SourceUri"/> is an absolute URI with an invalid scheme.</exception>
        /// <exception cref="FlexiBlocksException">Thrown if <see cref="BaseUri"/> is not an absolute URI.</exception>
        /// <exception cref="FlexiBlocksException">Thrown if <see cref="BaseUri"/> has an invalid scheme.</exception>
        /// <exception cref="FlexiBlocksException">Thrown if <see cref="SourceUri"/> is not a valid URI.</exception>
        /// <exception cref="FlexiBlocksException">Thrown if <see cref="Type"/> is not within the range of valid values for the enum <see cref="IncludeType"/>.</exception>
        protected override void ValidateAndPopulate()
        {
            if (SourceUri == null)
            {
                throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_OptionsMustNotBeNull, nameof(SourceUri)));
            }

            if (Uri.TryCreate(SourceUri, UriKind.Absolute, out Uri normalizedSourceUri))
            {
                // Invalid scheme
                if (!_supportedSchemes.Contains(normalizedSourceUri.Scheme))
                {
                    throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_OptionMustBeAUriWithASupportedScheme,
                        nameof(SourceUri),
                        SourceUri,
                        normalizedSourceUri.Scheme));
                }
            }
            else // SourceUri is not an absolute URI
            {
                string baseUri = string.IsNullOrWhiteSpace(BaseUri) ? Directory.GetCurrentDirectory() + "/" : BaseUri;

                // Normalize the base URI. A base URI must be absolute, see http://www.ietf.org/rfc/rfc3986.txt, section 5.1
                if (!Uri.TryCreate(baseUri, UriKind.Absolute, out Uri normalizedBaseUri))
                {
                    throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_OptionMustBeAnAbsoluteUri, nameof(BaseUri), BaseUri));
                }

                // Invalid scheme
                if (!_supportedSchemes.Contains(normalizedBaseUri.Scheme))
                {
                    throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_OptionMustBeAUriWithASupportedScheme,
                        nameof(BaseUri),
                        BaseUri,
                        normalizedBaseUri.Scheme));
                }

                if (!Uri.TryCreate(normalizedBaseUri, SourceUri, out normalizedSourceUri))
                {
                    // If we get to this point, SourceUri is neither a valid absolute URI nor a valid relative URI
                    throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_OptionMustBeAValidUri, nameof(SourceUri), SourceUri));
                }
            }

            NormalizedSourceUri = normalizedSourceUri;

            if (!Enum.IsDefined(typeof(IncludeType), Type))
            {
                throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_OptionMustBeAValidEnumValue,
                        Type,
                        nameof(Type),
                        nameof(IncludeType)));
            }

            // ResolvedDiskCacheDirectory is necessary so that CacheOnDisk can be false while DiskCacheDirectory isn't null, whitespace or an empty string.
            // Such situations can occur for default FlexiIncludeBlockOptions.
            if (CacheOnDisk)
            {
                ResolvedDiskCacheDirectory =  string.IsNullOrWhiteSpace(DiskCacheDirectory) ?
                    Path.Combine(Directory.GetCurrentDirectory(), "SourceCache") :
                    DiskCacheDirectory;
            }
            else
            {
                ResolvedDiskCacheDirectory = null;
            }
        }
    }
}
