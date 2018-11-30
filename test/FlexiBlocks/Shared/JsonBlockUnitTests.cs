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
        public void ParseLine_ParsesTheLineUpdatesBlockSpanEndAndReturnsTheStateOfTheBlock(string dummyJson)
        {
            // Arrange
            var testSubject = new DummyJsonBlock(null);
            var lineReader = new LineReader(dummyJson);

            // Act and assert
            while(true)
            {
                BlockState result = testSubject.ParseLine(lineReader.ReadLine().Value);

                if (result == BlockState.Break)
                {
                    Assert.Null(lineReader.ReadLine()); // If result is break we must be at the last line
                    break;
                }
                else
                {
                    Assert.Equal(BlockState.Continue, result);
                }
            }
            Assert.Equal(dummyJson.Length - 1, testSubject.Span.End);
        }

        public static IEnumerable<object[]> ParseLine_ParsesTheLineAndReturnsTheStateOfTheBlock_Data()
        {
            return new object[][]
            {
                // Multi-line JSON
                new object[]
                {
                    @"{
    ""property1"": ""value1"",
    ""property2"": ""value2""
}"
                },
                // Single-line JSON
                new object[]
                {
                    @"{""property1"": ""value1"", ""property2"": ""value2""}"
                },
                // Braces in strings
                new object[]
                {
                    @"{
    ""}property1"": ""}value1"",
    ""{property2"": ""{value2""
}"
                },
                // Nested objects
                new object[]
                {
                    @"{
    ""property1"": ""value1"",
    ""property2"": {
        ""property3"": ""value3""
    }
}"
                },
                // Escaped quotes in strings
                new object[]
                {
                    @"{
    ""prop\""erty1"": ""value1\"""",
    ""\""property2"": ""val\""ue2""
}"
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
