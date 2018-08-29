using System.Collections.Generic;
using Markdig.Helpers;
using Markdig.Parsers;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.Shared
{
    public class JsonBlockUnitTests
    {
        [Theory]
        [MemberData(nameof(ParseLine_ParsesTheLineAndReturnsTheStateOfTheBlock_Data))]
        public void ParseLine_ParsesTheLineAndReturnsTheStateOfTheBlock(string[] lines)
        {
            // Arrange
            var testSubject = new DummyJsonBlock(null);

            // Act and assert
            for(int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                BlockState result = testSubject.ParseLine(new StringSlice(line));
                
                if(i == lines.Length - 1)
                {
                    Assert.Equal(BlockState.Break, result);
                }
                else
                {
                    Assert.Equal(BlockState.Continue, result);
                }
            }
        }

        public static IEnumerable<object[]> ParseLine_ParsesTheLineAndReturnsTheStateOfTheBlock_Data()
        {
            return new object[][]
            {
                // Multi-line JSON
                new object[]{
                    new string[]{
                        "{",
                        "    \"property1\": \"value1\",",
                        "    \"property2\": \"value2\"",
                        "}"
                    }
                },
                // Single-line JSON
                new object[]
                {
                    new string[]{
                        "{\"property1\": \"value1\", \"property2\": \"value2\"}"
                    }
                },
                // Braces in strings
                new object[]{
                    new string[]{
                        "{",
                        "    \"}property1\": \"}value1\",",
                        "    \"{property2\": \"{value2\"",
                        "}"
                    }
                },
                // Nested objects
                new object[]{
                    new string[]{
                        "{",
                        "    \"property1\": \"value1\",",
                        "    \"property2\": {",
                        "        \"property3\": \"value3\"",
                        "    }",
                        "}"
                    }
                },
                // Escaped quotes in strings
                new object[]{
                    new string[]{
                        "{",
                        "    \"prop\"erty1\": \"value1\"\",",
                        "    \"\"property2\": \"val\"ue2\"",
                        "}"
                    }
                },
            };
        }

        private class DummyJsonBlock : JsonBlock
        {
            public DummyJsonBlock(BlockParser parser) : base(parser)
            {
            }
        }
    }
}
