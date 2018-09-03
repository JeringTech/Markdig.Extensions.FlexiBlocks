using Markdig.Syntax;
using System;
#if NETSTANDARD2_0
using System.Runtime.Serialization;
#endif

namespace Jering.Markdig.Extensions.FlexiBlocks
{
    /// <summary>
    /// Represents an unrecoverable situation encountered within FlexiBlocks.
    /// </summary>
#if NETSTANDARD2_0
    [Serializable]
#endif
    public class FlexiBlocksException : Exception
    {
        private readonly bool _customMessage;

        /// <summary>
        /// The description of the problem causing the FlexiBlock to be invalid.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The line number of the offending markdown.
        /// </summary>
        public int LineNumber { get; }

        /// <summary>
        /// The column of the offending markdown.
        /// </summary>
        public int Column { get; }

        /// <summary>
        /// The name of the type of the offending block.
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
        /// Creates a <see cref="FlexiBlocksException"/> instance representing an unexpected unrecoverable situation encountered when parsing markdown.
        /// </summary>
        /// <param name="lineIndex">The line index of the offending markdown.</param>
        /// <param name="column">The column that the offending markdown starts at.</param>
        public FlexiBlocksException(int lineIndex, int column) : this(lineIndex, column, null, null)
        {
        }

        /// <summary>
        /// Creates a <see cref="FlexiBlocksException"/> instance representing an unexpected unrecoverable situation encountered when parsing markdown.
        /// </summary>
        /// <param name="lineIndex">The line index of the offending markdown.</param>
        /// <param name="column">The column that the offending markdown starts at.</param>
        /// <param name="innerException">This exception's inner exception.</param>
        public FlexiBlocksException(int lineIndex, int column, Exception innerException) : this(lineIndex, column, null, innerException)
        {
        }

        /// <summary>
        /// Creates a <see cref="FlexiBlocksException"/> instance representing an unrecoverable situation encountered when parsing markdown.
        /// </summary>
        /// <param name="lineIndex">The line index of the offending markdown.</param>
        /// <param name="column">The column that the offending markdown starts at.</param>
        /// <param name="description">A description of the problem.</param>
        public FlexiBlocksException(int lineIndex, int column, string description) : this(lineIndex, column, description, null)
        {
        }

        /// <summary>
        /// Creates a <see cref="FlexiBlocksException"/> instance representing an unrecoverable situation encountered when parsing markdown.
        /// </summary>
        /// <param name="lineIndex">The line index of the offending markdown.</param>
        /// <param name="column">The column that the offending markdown starts at.</param>
        /// <param name="description">A description of the problem.</param>
        /// <param name="innerException">This exception's inner exception.</param>
        public FlexiBlocksException(int lineIndex, int column, string description, Exception innerException) : base(null, innerException)
        {
            LineNumber = lineIndex + 1;
            Column = column;
            Description = description;

            _customMessage = true;
        }

        /// <summary>
        /// Creates a <see cref="FlexiBlocksException"/> instance representing an unexpected unrecoverable situation encountered when processing a FlexiBlock.
        /// </summary>
        /// <param name="invalidFlexiBlock">The offending FlexiBlock.</param>
        public FlexiBlocksException(Block invalidFlexiBlock) : this(invalidFlexiBlock, null, null)
        {
        }

        /// <summary>
        /// Creates a <see cref="FlexiBlocksException"/> instance representing an unexpected unrecoverable situation encountered when processing a FlexiBlock.
        /// </summary>
        /// <param name="invalidFlexiBlock">The offending FlexiBlock.</param>
        /// <param name="innerException">This exception's inner exception.</param>
        public FlexiBlocksException(Block invalidFlexiBlock, Exception innerException) : this(invalidFlexiBlock, null, innerException)
        {
        }

        /// <summary>
        /// Creates a <see cref="FlexiBlocksException"/> instance representing an unrecoverable situation encountered when processing a FlexiBlock.
        /// </summary>
        /// <param name="invalidFlexiBlock">The offending FlexiBlock.</param>
        /// <param name="description">A description of the problem.</param>
        public FlexiBlocksException(Block invalidFlexiBlock, string description) : this(invalidFlexiBlock, description, null)
        {
        }

        /// <summary>
        /// Creates a <see cref="FlexiBlocksException"/> instance representing an unrecoverable situation encountered when processing a FlexiBlock.
        /// </summary>
        /// <param name="invalidFlexiBlock">The offending FlexiBlock.</param>
        /// <param name="description">A description of the problem.</param>
        /// <param name="innerException">This exception's inner exception.</param>
        public FlexiBlocksException(Block invalidFlexiBlock, string description, Exception innerException) : base(null, innerException)
        {
            Description = description;
            LineNumber = invalidFlexiBlock.Line + 1;
            Column = invalidFlexiBlock.Column;
            BlockTypeName = invalidFlexiBlock.GetType().Name;

            _customMessage = true;
        }

#if NETSTANDARD2_0
        /// <summary>
        /// Creates a <see cref="FlexiBlocksException"/> instance.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected FlexiBlocksException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
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

            info.AddValue(nameof(Description), Description, Description.GetType());
            info.AddValue(nameof(LineNumber), LineNumber, LineNumber.GetType());
            info.AddValue(nameof(Column), Column, Column.GetType());
            info.AddValue(nameof(BlockTypeName), BlockTypeName, BlockTypeName.GetType());
        }
#endif

        /// <summary>
        /// Gets the message that describes the problem.
        /// </summary>
        public override string Message
        {
            get
            {
                if (_customMessage && BlockTypeName != null) // The exception represents an unrecoverable situation encountered when processing a FlexiBlock.
                {
                    return string.Format(Strings.FlexiBlocksException_InvalidFlexiBlock,
                        BlockTypeName,
                        LineNumber,
                        Column,
                        Description ?? Strings.FlexiBlocksException_UnexpectedException);
                }
                else if(_customMessage) // The exception represents an unrecoverable situation encountered while parsing markdown.
                {
                    return string.Format(Strings.FlexiBlocksException_InvalidMarkdown,
                        LineNumber,
                        Column,
                        Description ?? Strings.FlexiBlocksException_UnexpectedException);
                }

                return base.Message;
            }
        }
    }
}
