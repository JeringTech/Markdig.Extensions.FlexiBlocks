using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;

namespace Jering.Markdig.Extensions.FlexiBlocks
{
    /// <summary>
    /// Represents a block that consists of just JSON.
    /// </summary>
    public abstract class JsonBlock : LeafBlock
    {
        /// <summary>
        /// Number of open objects in the JSON that has been parsed.
        /// </summary>
        private int _numOpenObjects;

        /// <summary>
        /// True if the JSON parsed so far ends within a string, for example if the JSON parsed so far is "{ \"part". False otherwise.
        /// </summary>
        private bool _withinString;

        /// <summary>
        /// Creates a <see cref="JsonBlock"/> instance.
        /// </summary>
        /// <param name="parser">The parser for this block.</param>
        protected JsonBlock(BlockParser parser) : base(parser)
        {
        }

        /// <summary>
        /// <para>Parses the next line in the block.</para>
        /// <para>Markdig parses markdown line by line, unfortunately, JSON.NET does not expose any efficient method for parsing JSON over several method calls. The commonly used 
        /// JsonSerializer.Deserialize is a static method and does not store any state, so deserialization must occur within a single call.</para>
        /// <para>On each TryContinue call, we could append the line to a string stored on the block and attempt to deserialize the string. Such an
        /// approach is inefficient, potentially allocating tons of strings, including one that spans the entire document, on top of using 
        /// exceptions as control flow.</para>
        /// <para>This method is an alternative approach. On each TryContinue call, it iterates through each character to determine whether the JSON
        /// has ended. It does this according to the specification documented here: https://www.json.org/. Basically, braces have no semantic meaning
        /// if the occur within strings, otherwise, { opens an object and } closes an object. This method simply tallies braces till opening
        /// and closing braces are balanced.</para>
        /// <para>It does not catch syntactic errors in JSON. Also, if braces aren't balanced, it could well end up iterating through all characters
        /// in the document. Nonetheless, it is more efficient than other methods that have been considered.</para>
        /// </summary>
        /// <param name="line">The line to parse.</param>
        /// <returns>The state of this block after parsing <paramref name="line"/>.</returns>
        public BlockState ParseLine(StringSlice line)
        {
            char previousChar = line.PeekCharExtra(-1);
            char currentChar = line.CurrentChar;

            while (currentChar != '\0')
            {
                if (!_withinString)
                {
                    if (currentChar == '{')
                    {
                        _numOpenObjects++;
                    }
                    else if (currentChar == '}')
                    {
                        // Braces balanced
                        if (--_numOpenObjects == 0)
                        {
                            UpdateSpanEnd(line.End);

                            return BlockState.Break;
                        }
                    }
                    else if (previousChar != '\\' && currentChar == '"')
                    {
                        _withinString = true;
                    }
                }
                else if (previousChar != '\\' && currentChar == '"')
                {
                    _withinString = false;
                }

                previousChar = currentChar;
                currentChar = line.NextChar();
            }

            return BlockState.Continue;
        }
    }
}
