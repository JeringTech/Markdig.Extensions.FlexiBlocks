using Markdig.Syntax;
using System;

namespace Jering.Markdig.Extensions.FlexiBlocks
{
    /// <summary>
    /// Represents an unrecoverable situation caused by an invalid FlexiBlock.
    /// </summary>
    public class FlexiBlocksException : Exception
    {
        /// <summary>
        /// The description of the problem causing the FlexiBlock to be invalid.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The invalid FlexiBlock.
        /// </summary>
        public Block InvalidFlexiBlock { get; }

        /// <summary>
        /// Creates a <see cref="FlexiBlocksException"/> instance.
        /// </summary>
        /// <param name="invalidFlexiBlock">The invalid FlexiBlock.</param>
        /// <param name="description">The description of the problem causing the FlexiBlock to be invalid.</param>
        public FlexiBlocksException(Block invalidFlexiBlock, string description)
        {
            InvalidFlexiBlock = invalidFlexiBlock;
            Description = description;
        }

        /// <summary>
        /// Creates a <see cref="FlexiBlocksException"/> instance.
        /// </summary>
        /// <param name="invalidFlexiBlock">The invalid FlexiBlock.</param>
        /// <param name="description">The description of the problem causing the FlexiBlock to be invalid.</param>
        /// <param name="innerException">This exception's inner exception.</param>
        public FlexiBlocksException(Block invalidFlexiBlock, string description, Exception innerException) : base(null, innerException)
        {
            InvalidFlexiBlock = invalidFlexiBlock;
            Description = description;
        }

        /// <inheritdoc />
        public override string Message
        {
            get
            {
                return string.Format(Strings.FlexiBlocksException_InvalidFlexiBlock,
                    InvalidFlexiBlock.GetType().Name,
                    InvalidFlexiBlock.Line + 1,
                    InvalidFlexiBlock.Column,
                    Description);
            }
        }
    }
}
