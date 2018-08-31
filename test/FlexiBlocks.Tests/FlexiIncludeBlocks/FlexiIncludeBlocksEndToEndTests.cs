using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using Markdig;
using Markdig.Parsers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiIncludeBlocks
{
    public class FlexiIncludeBlocksEndToEndTests : IClassFixture<FlexiIncludeBlocksEndToEndTestsFixture>
    {
        private readonly FlexiIncludeBlocksEndToEndTestsFixture _fixture;

        public FlexiIncludeBlocksEndToEndTests(FlexiIncludeBlocksEndToEndTestsFixture fixture)
        {
            _fixture = fixture;
        }

        // As far as end to end tests go, success cases are covered in FlexiIncludeBlockSpecs, so here we just test for exceptions.
        [Theory]
        [MemberData(nameof(FlexiIncludeBlockParser_ThrowsIfACycleIsFound_Data))]
        public void FlexiIncludeBlockParser_ThrowsIfACycleIsFound(string dummyEntryMarkdown, string dummyMarkdown1, string dummyMarkdown2, string dummyMarkdown3,
            string expectedCycleDescription)
        {
            // Arrange
            File.WriteAllText(Path.Combine(_fixture.TempDirectory, $"{nameof(dummyMarkdown1)}.md"), dummyMarkdown1);
            File.WriteAllText(Path.Combine(_fixture.TempDirectory, $"{nameof(dummyMarkdown2)}.md"), dummyMarkdown2);
            File.WriteAllText(Path.Combine(_fixture.TempDirectory, $"{nameof(dummyMarkdown3)}.md"), dummyMarkdown3);

            // Need to dispose of services between tests so that FileCacheService's in memory cache doesn't affect results
            var services = new ServiceCollection();
            services.AddFlexiBlocks();
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            using ((IDisposable)serviceProvider)
            {
                var dummyMarkdownPipelineBuilder = new MarkdownPipelineBuilder();
                dummyMarkdownPipelineBuilder.Extensions.Add(serviceProvider.GetRequiredService<FlexiIncludeBlocksExtension>());
                FlexiIncludeBlocksExtensionOptions dummyExtensionOptions = serviceProvider.GetRequiredService<IOptions<FlexiIncludeBlocksExtensionOptions>>().Value;
                dummyExtensionOptions.SourceBaseUri = _fixture.TempDirectory + "/";
                MarkdownPipeline dummyMarkdownPipeline = dummyMarkdownPipelineBuilder.Build();

                // Act and assert
                InvalidOperationException result = Assert.Throws<InvalidOperationException>(() => MarkdownParser.Parse(dummyEntryMarkdown, dummyMarkdownPipeline));
                Assert.Equal(string.Format(Strings.InvalidOperationException_CycleInIncludes, expectedCycleDescription), result.Message, ignoreLineEndingDifferences: true);
            }
        }

        public static IEnumerable<object[]> FlexiIncludeBlockParser_ThrowsIfACycleIsFound_Data()
        {
            return new object[][]
            {
                // Basic circular include
                new object[]
                {
                    @"+{
    ""contentType"": ""Markdown"",
    ""source"": ""./dummyMarkdown1.md""
}",
                    @"+{
    ""contentType"": ""Markdown"",
    ""source"": ""./dummyMarkdown2.md""
}",
                    @"+{
    ""contentType"": ""Markdown"",
    ""source"": ""./dummyMarkdown1.md""
}",
                    null,
                    @"Source: ./dummyMarkdown1.md, Line: 1 >
Source: ./dummyMarkdown2.md, Line: 1 >
Source: ./dummyMarkdown1.md, Line: 1"
                },
                // Valid includes don't affect identification of circular includes
                new object[]
                {
                    @"+{
    ""contentType"": ""Markdown"",
    ""source"": ""./dummyMarkdown1.md"",
    ""clippings"": [{""startLineNumber"": 2, ""endLineNumber"": 2}]
}

+{
    ""contentType"": ""Markdown"",
    ""source"": ""./dummyMarkdown1.md""
}",
                    @"+{
    ""contentType"": ""Markdown"",
    ""source"": ""./dummyMarkdown3.md""
}

+{
    ""contentType"": ""Markdown"",
    ""source"": ""./dummyMarkdown2.md""
}",
                    @"+{
    ""contentType"": ""Code"",
    ""source"": ""./dummyMarkdown1.md""
}

+{
    ""contentType"": ""Markdown"",
    ""source"": ""./dummyMarkdown1.md""
}",
                    "This is a line",
                    @"Source: ./dummyMarkdown1.md, Line: 6 >
Source: ./dummyMarkdown2.md, Line: 6 >
Source: ./dummyMarkdown1.md, Line: 6"
                },
                // Circular includes that uses clippings are caught
                new object[]
                {
                    @"+{
    ""contentType"": ""Markdown"",
    ""source"": ""./dummyMarkdown1.md"",
    ""clippings"": [{""startLineNumber"": 2, ""endLineNumber"": 2}]
}

+{
    ""contentType"": ""Markdown"",
    ""source"": ""./dummyMarkdown1.md"",
    ""clippings"": [{""startLineNumber"": 6, ""endLineNumber"": -1}]
}",
                    @"+{
    ""contentType"": ""Markdown"",
    ""source"": ""./dummyMarkdown3.md""
}

+{
    ""contentType"": ""Markdown"",
    ""source"": ""./dummyMarkdown2.md"",
    ""clippings"": [{""startLineNumber"": 6, ""endLineNumber"": -1}]
}",
                    @"+{
    ""contentType"": ""Code"",
    ""source"": ""./dummyMarkdown1.md""
}

+{
    ""contentType"": ""Markdown"",
    ""source"": ""./dummyMarkdown1.md""
}",
                    "This is a line",
                    @"Source: ./dummyMarkdown1.md, Line: 6 >
Source: ./dummyMarkdown2.md, Line: 6 >
Source: ./dummyMarkdown1.md, Line: 6"
                },
                // Circular includes originating from before and after content are caught
                new object[]
                {
                    @"+{
    ""contentType"": ""Markdown"",
    ""source"": ""./dummyMarkdown1.md""
}",
                    @"+{
    ""contentType"": ""Markdown"",
    ""source"": ""./dummyMarkdown3.md"",
    ""clippings"": [{
                        ""beforeContent"": ""This is a line.
+{
                            \""contentType\"": \""Markdown\"",
                            \""source\"": \""./dummyMarkdown2.md\""
                        }""
                    }]
}",
                    @"+{
    ""contentType"": ""Markdown"",
    ""source"": ""./dummyMarkdown3.md"",
    ""clippings"": [{
                        ""afterContent"": ""+{
                            \""contentType\"": \""Markdown\"",
                            \""source\"": \""./dummyMarkdown1.md\""
                        }""
                    }]
}",
                    "This is a line.",
                    @"Source: ./dummyMarkdown1.md, Line: 1 >
Source: BeforeContent, Line: 2 >
Source: ./dummyMarkdown2.md, Line: 1 >
Source: AfterContent, Line: 1 >
Source: ./dummyMarkdown1.md, Line: 1"
                }
            };
        }
    }
}
