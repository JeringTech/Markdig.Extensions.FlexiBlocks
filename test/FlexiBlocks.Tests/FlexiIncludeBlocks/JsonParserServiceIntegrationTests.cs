using Markdig.Helpers;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiIncludeBlocks
{
    public class JsonParserServiceIntegrationTests
    {
        [Theory]
        [MemberData(nameof(Parse_ParsesJson_Data))]
        public void Parse_ParsesJson(string dummyJson, int dummyStartIndex, int expectedNumLines)
        {
            // Arrange
            var dummyStringSlice = new StringSlice(dummyJson, dummyStartIndex, dummyJson.Length - 1);
            var testSubject = new JsonParserService();

            // Act
            (int numLines, DummyModel result) = testSubject.Parse<DummyModel>(dummyStringSlice);

            // Assert
            Assert.Equal(expectedNumLines, numLines);
            Assert.Equal("dummyValue1", result.Property1);
            Assert.Equal("dummyValue2", result.Property2);
        }

        public static IEnumerable<object[]> Parse_ParsesJson_Data()
        {
            return new object[][]
            {
                // Entire string is JSON
                new object[]{
                    @"{
    ""Property1"": ""dummyValue1"",
    ""Property2"": ""dummyValue2""
}",
                    0,
                    4
                },
                // Entire string is JSON - single line
                new object[]{
                    @"{ ""Property1"": ""dummyValue1"", ""Property2"": ""dummyValue2"" }",
                    0,
                    1
                },
                // Preceding lines and characters aren't JSON
                new object[]{
                    @"This is a line.
+{
    ""Property1"": ""dummyValue1"",
    ""Property2"": ""dummyValue2""
}".Replace("\r\n", "\n"),
                    17,
                    4
                },
                // Following lines and characters aren't JSON
                new object[]{
                    @"{
    ""Property1"": ""dummyValue1"",
    ""Property2"": ""dummyValue2""
} This isn't JSON
This isn't JSON either".Replace("\r\n", "\n"),
                    0,
                    4
                }
            };
        }

        private class DummyModel
        {
            public string Property1 { get; set; }
            public string Property2 { get; set; }
        }
    }
}
