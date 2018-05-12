using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Newtonsoft.Json.Linq;

namespace JeremyTCD.Markdig.Extensions
{
    public class JsonOptionsParser : BlockParser
    {
        public JsonOptionsParser()
        {
            // If options block is not consumed by the following block, it is rendered as a paragraph or in the preceding paragraph, so {, despite being common, should work fine.
            OpeningCharacters = new[] { 'o' };
        }

        public override BlockState TryOpen(BlockProcessor processor)
        {
            if (processor.IsCodeIndent)
            {
                return BlockState.None;
            }

            if(!processor.Line.MatchLowercase("options {"))
            {
                return BlockState.None;
            }

            // Discard "options " by starting line at "{"
            processor.Line.Start = 8;

            var jsonOptionsblock = new JsonOptionsBlock(this)
            {
                Column = processor.Column,
                Span = { Start = processor.Line.Start }
            };
            processor.NewBlocks.Push(jsonOptionsblock);

            if (JsonComplete(processor.Line, jsonOptionsblock))
            {
                return BlockState.Break;
            }

            return BlockState.Continue;
        }

        public override BlockState TryContinue(BlockProcessor processor, Block block)
        {
            if(JsonComplete(processor.Line, (JsonOptionsBlock)block))
            {
                return BlockState.Break;
            }

            return BlockState.Continue;
        }

        /// <summary>
        /// Determines whether or not the <see cref="JsonOptionsBlock"/> is complete. The JSON spec allows for 
        /// unescaped curly brackets within strings - https://www.json.org/, so this method ignores everything between unescaped quotes.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="block"></param>
        private bool JsonComplete(StringSlice line, JsonOptionsBlock block)
        {
            char pc = line.PeekCharExtra(-1);
            char c = line.CurrentChar;

            while (c != '\0')
            {
                if (!block.InString)
                {
                    if (c == '{')
                    {
                        block.NumOpenBrackets++;
                    }
                    else if(c == '}')
                    {
                        if(--block.NumOpenBrackets == 0)
                        {
                            // TODO fill in span end
                            return true;
                        }
                    }
                    else if (pc != '\\' && c == '"')
                    {
                        block.InString = true;
                    }
                }
                else if (pc != '\\' && c == '"')
                {
                    block.InString = false;
                }

                pc = c;
                c = line.NextChar();
            }

            return false;
        }

        private bool TryParse(ref StringSlice slice, out JObject options)
        {
            try
            {
                // Json options must be a paragraph or part of a paragraph just before an element.
                // 
                options = JObject.Parse(slice.ToString());

                return true;
            }
            catch
            {
                options = null;

                return false;
            }
        }
    }
}
