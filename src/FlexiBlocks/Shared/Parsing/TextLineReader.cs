using System;
using System.IO;

namespace Jering.Markdig.Extensions.FlexiBlocks
{
    /// <summary>
    /// An implementation of <see cref="TextReader"/> that keeps track of the number of lines it has read.
    /// </summary>
    public class TextLineReader : TextReader
    {
        private readonly string _text;
        private int _currentCharIndex;

        /// <summary>
        /// Number of lines read.
        /// </summary>
        public int LinesRead { get; private set; }

        /// <summary>
        /// Creates a <see cref="TextLineReader"/> instance.
        /// </summary>
        /// <param name="text">The text to read from.</param>
        /// <param name="startCharIndex">The index of the first character to read.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="text"/> is null or an empty string.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="startCharIndex"/> is not within the interval containing text's indices.</exception>
        public TextLineReader(string text, int startCharIndex)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException(Strings.ArgumentException_ValueCannotBeNullOrAnEmptyString, nameof(text));
            }

            if (startCharIndex < 0 || startCharIndex >= text.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startCharIndex),
                    string.Format(Strings.ArgumentOutOfRangeException_ValueMustBeWithinTheIntervalContainingBuffersIndices, nameof(text)));
            }

            _text = text;
            _currentCharIndex = startCharIndex;
        }

        /// <summary>
        /// Reads characters from the current line.
        /// </summary>
        /// <param name="buffer">The buffer to write characters to.</param>
        /// <param name="index">The index of the first element in <paramref name="buffer"/> to overwrite.</param>
        /// <param name="count">The maximum number of characters to read.</param>
        /// <returns>The number of characters read.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="buffer"/> is null</exception>.
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="index"/> is not within the interval containing <paramref name="buffer"/>'s indices.</exception>.
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="count"/> is negative or greater than the number of elements in <paramref name="buffer"/>.</exception>
        public override int Read(char[] buffer, int index, int count)
        {
            if(buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if(index < 0 || index >= buffer.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index),
                    string.Format(Strings.ArgumentOutOfRangeException_ValueMustBeWithinTheIntervalContainingBuffersIndices, nameof(buffer)));
            }

            if(count < 0 || count > buffer.Length - index)
            {
                throw new ArgumentOutOfRangeException(nameof(count),
                    Strings.ArgumentOutOfRangeException_CountCannotBeNegativeOrGreaterThanTheNumberOfEmptyElementsInBuffer);
            }

            // All chars have been read
            if (_currentCharIndex >= _text.Length)
            {
                return 0;
            }

            int numChars = 0;

            while(true)
            {
                char currentChar = _text[_currentCharIndex];
                 _currentCharIndex++;

                if (currentChar == '\n')
                {
                    LinesRead++;
                    return numChars;
                }

                // Consider \r alone to denote a new line, TextReader.ReadLine does this too.
                if (currentChar == '\r')
                {
                    if (_currentCharIndex < _text.Length && _text[_currentCharIndex] == '\n')
                    {
                        _currentCharIndex++;
                    }

                    LinesRead++;
                    return numChars;
                }

                // Current char is not a new line character, add it to the buffer
                buffer[index + numChars] = currentChar;
                numChars++;

                if (_currentCharIndex >= _text.Length)
                {
                    LinesRead++;
                    return numChars;
                }

                // Buffer is full
                if (numChars == count)
                {
                    return numChars;
                }
            }
        }
    }
}
