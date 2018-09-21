using System;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    /// <summary>
    /// Represents a block that includes content.
    /// </summary>
    public class FlexiIncludeBlock : JsonBlock
    {
        private FlexiIncludeBlock _parentFlexiIncludeBlock;
        private string _containingSourceUri;
        private int _lineNumberInContainingSource;

        /// <summary>
        /// Creates a <see cref="FlexiIncludeBlock"/> instance.
        /// </summary>
        /// <param name="parser">The parser for this block.</param>
        public FlexiIncludeBlock(FlexiIncludeBlockParser parser) : base(parser)
        {
        }

        /// <summary>
        /// The options for this block.
        /// </summary>
        public FlexiIncludeBlockOptions FlexiIncludeBlockOptions { get; set; }

        /// <summary>
        /// Gets or sets the current clipping's processing stage.
        /// </summary>
        public ClippingProcessingStage ClippingProcessingStage { get; set; }

        /// <summary>
        /// <para>Gets or sets the line number of the last processed line.</para>
        /// <para>This value allows child <see cref="FlexiIncludeBlock"/>s to determine their line numbers.</para>
        /// </summary>
        public int LastProcessedLineLineNumber { get; set; }

        /// <summary>
        /// Gets the URI of the source that contains this <see cref="FlexiIncludeBlock"/>.
        /// </summary>
        public string ContainingSourceUri
        {
            get
            {
                if(_containingSourceUri != null)
                {
                    return _containingSourceUri;
                }

                // Root
                if(ParentFlexiIncludeBlock == null)
                {
                    return null;
                }

                if(ParentFlexiIncludeBlock.ClippingProcessingStage == ClippingProcessingStage.Source)
                {
                    return _containingSourceUri = ParentFlexiIncludeBlock.FlexiIncludeBlockOptions.SourceUri;
                }

                // Before or after content in a FlexiIncludeBlock in root
                if(ParentFlexiIncludeBlock.ParentFlexiIncludeBlock == null)
                {
                    return null;
                }

                // Before or after content, containing source is grandparent's source
                return _containingSourceUri = ParentFlexiIncludeBlock.ParentFlexiIncludeBlock.FlexiIncludeBlockOptions.SourceUri;
            }
        }

        /// <summary>
        /// Gets this <see cref="FlexiIncludeBlock"/>'s line number in the source that contains it.
        /// </summary>
        public int LineNumberInContainingSource
        {
            get
            {
                if(_lineNumberInContainingSource == 0 && ParentFlexiIncludeBlock != null)
                {
                    _lineNumberInContainingSource = ParentFlexiIncludeBlock.LastProcessedLineLineNumber - Lines.Count + 1;
                }

                return _lineNumberInContainingSource;
            }
        }

        /// <summary>
        /// Gets or sets this <see cref="FlexiIncludeBlock"/>'s parent <see cref="FlexiIncludeBlock"/>.
        /// </summary>
        public FlexiIncludeBlock ParentFlexiIncludeBlock
        {
            get
            {
                return _parentFlexiIncludeBlock;
            }
            set
            {
                if(_parentFlexiIncludeBlock != null && _parentFlexiIncludeBlock != value)
                {
                    throw new ArgumentException(string.Format(Strings.ArgumentException_PropertyAlreadyHasAValue, nameof(ParentFlexiIncludeBlock)));
                }
                _parentFlexiIncludeBlock = value;
            }
        }

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            if (ParentFlexiIncludeBlock == null)
            {
                return $"Source: Root, Line: {Line + 1}";
            }
            else if (ParentFlexiIncludeBlock.ClippingProcessingStage != ClippingProcessingStage.Source)
            {
                return $"{ParentFlexiIncludeBlock.ClippingProcessingStage}, Line: {LineNumberInContainingSource}";
            }
            else
            {
                return $"Source URI: {ContainingSourceUri}, Line: {LineNumberInContainingSource}";
            }
        }
    }
}
