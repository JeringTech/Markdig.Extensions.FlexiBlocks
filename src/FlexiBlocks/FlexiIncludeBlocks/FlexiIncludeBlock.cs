using System;
using System.Collections.Generic;
using System.Linq;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    /// <summary>
    /// Represents a block that includes content.
    /// </summary>
    public class FlexiIncludeBlock : JsonBlock
    {
        // We only support a subset of schemes. For the full list of schemes, see https://docs.microsoft.com/en-sg/dotnet/api/system.uri.scheme?view=netstandard-2.0#System_Uri_Scheme
        private static readonly string[] _supportedSchemes = new string[] { "file", "http", "https" };
        private Uri _absoluteSourceUri;

        /// <summary>
        /// Creates a <see cref="FlexiIncludeBlock"/> instance.
        /// </summary>
        /// <param name="parentFlexiIncludeBlock">This block's parent.</param>
        /// <param name="parser">The parser for this block.</param>
        public FlexiIncludeBlock(FlexiIncludeBlock parentFlexiIncludeBlock, FlexiIncludeBlockParser parser) : base(parser)
        {
            if (parentFlexiIncludeBlock != null)
            {
                ParentFlexiIncludeBlock = parentFlexiIncludeBlock;
                parentFlexiIncludeBlock.ChildFlexiIncludeBlocks.Add(this);
            }
            ChildFlexiIncludeBlocks = new List<FlexiIncludeBlock>();
        }

        /// <summary>
        /// Gets or sets the options for this block.
        /// </summary>
        public FlexiIncludeBlockOptions FlexiIncludeBlockOptions { get; internal set; }

        /// <summary>
        /// The absolute URI of this <see cref="FlexiIncludeBlock"/>'s source.
        /// </summary>
        public Uri AbsoluteSourceUri { get => _absoluteSourceUri; internal set => _absoluteSourceUri = value; }

        /// <summary>
        /// Gets the URI of the source that contains this <see cref="FlexiIncludeBlock"/>.
        /// </summary>
        public string ContainingSourceUri { get; internal set; }

        /// <summary>
        /// Gets this <see cref="FlexiIncludeBlock"/>'s line number in the source that contains it.
        /// </summary>
        public int LineNumberInContainingSource { get; internal set; }

        /// <summary>
        /// Gets or sets this <see cref="FlexiIncludeBlock"/>'s parent <see cref="FlexiIncludeBlock"/>.
        /// </summary>
        public FlexiIncludeBlock ParentFlexiIncludeBlock { get; }

        /// <summary>
        /// Gets or sets this <see cref="FlexiIncludeBlock"/>'s child <see cref="FlexiIncludeBlock"/>s.
        /// </summary>
        public List<FlexiIncludeBlock> ChildFlexiIncludeBlocks { get; }

        /// <summary>
        /// Gets or sets the current clipping's processing stage.
        /// </summary>
        internal ClippingProcessingStage ClippingProcessingStage { get; set; }

        /// <summary>
        /// <para>Gets or sets the line number of the last processed line.</para>
        /// <para>This value allows child <see cref="FlexiIncludeBlock"/>s to determine their line numbers.</para>
        /// </summary>
        internal int LastProcessedLineLineNumber { get; set; }

        /// <summary>
        /// <para>Populates generated properties.</para>
        /// <para>The <see cref="FlexiIncludeBlockOptions"/> for a <see cref="FlexiIncludeBlock"/> aren't available at instantiation. This method
        /// is a systematic way to specify <see cref="FlexiIncludeBlockOptions"/> for <see cref="FlexiIncludeBlock"/>s.</para>
        /// </summary>
        /// <param name="flexiIncludeBlockOptions">The <see cref="FlexiIncludeBlockOptions"/> for this <see cref="FlexiIncludeBlock"/>.</param>
        /// <param name="rootBaseUri">A base URI for generating this <see cref="FlexiIncludeBlock"/>'s <see cref="AbsoluteSourceUri"/>.</param>
        internal void Setup(FlexiIncludeBlockOptions flexiIncludeBlockOptions, string rootBaseUri)
        {
            FlexiIncludeBlockOptions = flexiIncludeBlockOptions ?? throw new ArgumentNullException(nameof(flexiIncludeBlockOptions));

            // Generate AbsoluteSourceUri
            if (Uri.TryCreate(flexiIncludeBlockOptions.SourceUri, UriKind.Absolute, out _absoluteSourceUri))
            {
                // Invalid scheme
                if (!_supportedSchemes.Contains(_absoluteSourceUri.Scheme))
                {
                    throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_FlexiIncludeBlock_OptionMustBeAUriWithASupportedScheme,
                        nameof(flexiIncludeBlockOptions.SourceUri),
                        flexiIncludeBlockOptions.SourceUri,
                        _absoluteSourceUri.Scheme));
                }
            }
            else if (ParentFlexiIncludeBlock != null) // SourceUri is relative and parent's absolute source URI should be used as base
            {
                // FlexiIncludeBlocks are always setup before they are added to the stack and used as parents, so we can safely 
                // assume that the parent has an AbsoluteSourceUri.
                if (!Uri.TryCreate(ParentFlexiIncludeBlock.AbsoluteSourceUri, flexiIncludeBlockOptions.SourceUri, out _absoluteSourceUri))
                {
                    // If we get to this point, SourceUri is neither a valid absolute URI nor a valid relative URI
                    throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_FlexiIncludeBlock_OptionMustBeAValidUri, nameof(flexiIncludeBlockOptions.SourceUri), flexiIncludeBlockOptions.SourceUri));
                }
            }
            else // SourceUri is relative and root base URI should be used as base
            {
                // Normalize the base URI. A base URI must be absolute, see http://www.ietf.org/rfc/rfc3986.txt, section 5.1
                //
                // Note: On Windows, "/**/*" is not considered an absolute URI, absolute URIs must start with "<drive letter>:/" or "file:///".
                // On Linux and MacOS, "/**/*" is considered an absolute URI, in some cases, it can be an absolute URI ("/<drive letter>/**/*"), but 
                // it can also be a path relative to a root. This means that relative URIs could pass through this check on Linux and macOS.
                // That isn't a huge issue though, an exception will be thrown if we try to retrieve anything from a URI that has a relative URI as its base.
                // The difference in behaviour between Windows and Linux/macOS is documented here - https://github.com/dotnet/corefx/issues/22098.
                if (!Uri.TryCreate(rootBaseUri, UriKind.Absolute, out Uri baseUri))
                {
                    throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_FlexiIncludeBlock_OptionMustBeAnAbsoluteUri, nameof(FlexiIncludeBlocksExtensionOptions.RootBaseUri), rootBaseUri));
                }

                // Invalid scheme
                if (!_supportedSchemes.Contains(baseUri.Scheme))
                {
                    throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_FlexiIncludeBlock_OptionMustBeAUriWithASupportedScheme,
                        nameof(FlexiIncludeBlocksExtensionOptions.RootBaseUri),
                        rootBaseUri,
                        baseUri.Scheme));
                }

                if (!Uri.TryCreate(baseUri, flexiIncludeBlockOptions.SourceUri, out _absoluteSourceUri))
                {
                    // If we get to this point, SourceUri is neither a valid absolute URI nor a valid relative URI
                    throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_FlexiIncludeBlock_OptionMustBeAValidUri, nameof(flexiIncludeBlockOptions.SourceUri), flexiIncludeBlockOptions.SourceUri));
                }
            }

            // If a FlexiIncludeBlock 
            // - has no parent or
            // - is before/after content and has no grandparent
            // Then it exists in the root source, which has no URI. This block handles the inverse of those cases.
            if (ParentFlexiIncludeBlock != null)
            {
                // Generate ContainingSourceUri and LineNumberInContainingSource
                if (ParentFlexiIncludeBlock.ClippingProcessingStage == ClippingProcessingStage.Source)
                {
                    ContainingSourceUri = ParentFlexiIncludeBlock.AbsoluteSourceUri.AbsoluteUri;
                    LineNumberInContainingSource = ParentFlexiIncludeBlock.LastProcessedLineLineNumber - Lines.Count + 1;
                }
                else if (ParentFlexiIncludeBlock.ParentFlexiIncludeBlock != null)
                {
                    // Before or after content, containing source is grandparent's source
                    ContainingSourceUri = ParentFlexiIncludeBlock.ParentFlexiIncludeBlock.AbsoluteSourceUri.AbsoluteUri;
                    // TODO how can we find the line number of the before/after content in its containing source?
                    LineNumberInContainingSource = ParentFlexiIncludeBlock.LineNumberInContainingSource;
                }
            }
        }

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            if (ContainingSourceUri == null)
            {
                return $"Source: Root, Line: {Line + 1}";
            }
            else
            {
                string stage = ParentFlexiIncludeBlock.ClippingProcessingStage != ClippingProcessingStage.Source ? $", {ParentFlexiIncludeBlock.ClippingProcessingStage}" : string.Empty;

                return $"Source URI: {ContainingSourceUri}, Line: {LineNumberInContainingSource}{stage}";
            }
        }
    }
}
