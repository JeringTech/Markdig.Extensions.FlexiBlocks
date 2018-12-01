using Markdig.Syntax;
using System;
using System.Runtime.Serialization;

namespace Jering.Markdig.Extensions.FlexiBlocks
{
    /// <summary>
    /// Represents an unrecoverable situation encountered within FlexiBlocks.
    /// </summary>
    [Serializable]
    public class FlexiBlocksException : Exception
    {
        /// <summary>
        /// Gets the context of the unrecoverable situation.
        /// </summary>
        public Context Context { get; }

        /// <summary>
        /// Gets the description of the problem causing the FlexiBlock to be invalid.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the line number of the offending markdown.
        /// </summary>
        public int LineNumber { get; }

        /// <summary>
        /// Gets the column of the offending markdown.
        /// </summary>
        public int Column { get; }

        /// <summary>
        /// Gets the name of the type of the offending block.
        /// </summary>
        public string BlockTypeName { get; }

        /// <summary>
        /// Creates a <see cref="FlexiBlocksException"/> instance representing an unrecoverable situation.
        /// </summary>
        public FlexiBlocksException()
        {
        }

        /// <summary>
        /// Creates a <see cref="FlexiBlocksException"/> instance representing an unrecoverable situation.
        /// </summary>
        /// <param name="message">This exception's message.</param>
        public FlexiBlocksException(string message) : base(message)
        {
        }

        /// <summary>
        /// Creates a <see cref="FlexiBlocksException"/> instance representing an unrecoverable situation.
        /// </summary>
        /// <param name="message">This exception's message.</param>
        /// <param name="innerException">This exception's inner exception.</param>
        public FlexiBlocksException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates a <see cref="FlexiBlocksException"/> instance representing an unrecoverable situation encountered when parsing markdown.
        /// </summary>
        /// <param name="lineNumber">The line number of the offending markdown.</param>
        /// <param name="column">The column that the offending markdown starts at.</param>
        /// <param name="description">A description of the problem.</param>
        /// <param name="innerException">This exception's inner exception.</param>
        public FlexiBlocksException(int lineNumber, int column, string description = null, Exception innerException = null) : base(null, innerException)
        {
            LineNumber = lineNumber;
            Column = column;
            Description = description;

            Context = Context.Line;
        }

        /// <summary>
        /// Creates a <see cref="FlexiBlocksException"/> instance representing an unrecoverable situation encountered when processing a FlexiBlock.
        /// </summary>
        /// <param name="invalidFlexiBlock">The offending FlexiBlock.</param>
        /// <param name="lineNumber">The line number of the offending markdown.</param>
        /// <param name="column">The column that the offending markdown starts at.</param>
        /// <param name="description">A description of the problem.</param>
        /// <param name="innerException">This exception's inner exception.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="invalidFlexiBlock"/> is null.</exception>
        public FlexiBlocksException(Block invalidFlexiBlock, string description = null, Exception innerException = null, int? lineNumber = null, int? column = null) : base(null, innerException)
        {
            if (invalidFlexiBlock == null)
            {
                throw new ArgumentNullException(nameof(invalidFlexiBlock));
            }

            Description = description;
            LineNumber = lineNumber ?? invalidFlexiBlock.Line + 1;
            Column = column ?? invalidFlexiBlock.Column;
            BlockTypeName = invalidFlexiBlock.GetType().Name;
            Context = Context.Block;
        }

        /// <summary>
        /// Creates a <see cref="FlexiBlocksException"/> instance.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected FlexiBlocksException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Context = (Context)info.GetInt32(nameof(Context));
            Description = info.GetString(nameof(Description));
            LineNumber = info.GetInt32(nameof(LineNumber));
            Column = info.GetInt32(nameof(LineNumber));
            BlockTypeName = info.GetString(nameof(BlockTypeName));
        }

        /// <summary>
        /// Gets object data for binary serialization.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(Context), Context, typeof(int));
            info.AddValue(nameof(Description), Description, typeof(string));
            info.AddValue(nameof(LineNumber), LineNumber, typeof(int));
            info.AddValue(nameof(Column), Column, typeof(int));
            info.AddValue(nameof(BlockTypeName), BlockTypeName, typeof(string));
        }

        /// <summary>
        /// Gets the message that describes the problem.
        /// </summary>
        public override string Message
        {
            get
            {
                if (Context == Context.Block) // The exception represents an unrecoverable situation encountered when processing a FlexiBlock.
                {
                    return string.Format(Strings.FlexiBlocksException_InvalidFlexiBlock,
                        BlockTypeName,
                        LineNumber,
                        Column,
                        Description ?? Strings.FlexiBlocksException_ExceptionOccurredWhileProcessingABlock);
                }
                else if (Context == Context.Line) // The exception represents an unrecoverable situation encountered while parsing markdown.
                {
                    return string.Format(Strings.FlexiBlocksException_InvalidMarkdown,
                        LineNumber,
                        Column,
                        Description ?? Strings.FlexiBlocksException_ExceptionOccurredWhileProcessingABlock);
                }

                return base.Message;
            }
        }
    }
}
