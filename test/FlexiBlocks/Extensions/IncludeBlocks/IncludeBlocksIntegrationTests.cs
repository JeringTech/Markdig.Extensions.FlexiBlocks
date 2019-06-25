using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.IncludeBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.OptionsBlocks;
using Markdig;
using Markdig.Parsers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.IncludeBlocks
{
    // Integration tests that don't fit in amongst the specs.
    public class IncludeBlocksIntegrationTests : IClassFixture<IncludeBlocksIntegrationTestsFixture>
    {
        private readonly IncludeBlocksIntegrationTestsFixture _fixture;

        public IncludeBlocksIntegrationTests(IncludeBlocksIntegrationTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void IncludeBlocks_ConstructsAndExposesIncludeBlockTrees()
        {
            // Arrange
            const string dummySource1 = "./dummyContent1.md";
            const string dummySource2 = "./dummyContent2.md";
            const string dummySource3 = "./dummyContent3.md";
            string dummyRootContent = $@"+{{
""type"": ""markdown"",
""source"": ""{dummySource1}"",
}}

+{{
""type"": ""markdown"",
""source"": ""{dummySource2}""
}}";
            const string dummyContent1 = "This is dummy markdown";
            string dummyContent2 = $@"+{{
""type"": ""markdown"",
""source"": ""{dummySource3}"",
}}";
            const string dummyContent3 = "This is dummy markdown";
            string dummySource1AbsolutePath = new Uri($"{_fixture.TempDirectory}/{nameof(dummyContent1)}.md").AbsolutePath;
            string dummySource2AbsolutePath = new Uri($"{_fixture.TempDirectory}/{nameof(dummyContent2)}.md").AbsolutePath;
            string dummySource3AbsolutePath = new Uri($"{_fixture.TempDirectory}/{nameof(dummyContent3)}.md").AbsolutePath;
            File.WriteAllText(dummySource1AbsolutePath, dummyContent1);
            File.WriteAllText(dummySource2AbsolutePath, dummyContent2);
            File.WriteAllText(dummySource3AbsolutePath, dummyContent3);

            // Need to dispose of services after each test so that the in-memory cache doesn't affect results
            var services = new ServiceCollection();
            services.AddIncludeBlocks();
            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                var dummyMarkdownPipelineBuilder = new MarkdownPipelineBuilder();
                dummyMarkdownPipelineBuilder.Extensions.Add(serviceProvider.GetRequiredService<IBlockExtension<IncludeBlock>>());
                MarkdownPipeline dummyMarkdownPipeline = dummyMarkdownPipelineBuilder.Build();
                var dummyMarkdownParserContext = new MarkdownParserContext();
                dummyMarkdownParserContext.Properties[typeof(IIncludeBlocksExtensionOptions)] = new IncludeBlocksExtensionOptions(baseUri: _fixture.TempDirectory + "/");

                // Act
                MarkdownParser.Parse(dummyRootContent, dummyMarkdownPipeline, dummyMarkdownParserContext);

                // Assert
                dummyMarkdownParserContext.Properties.TryGetValue(IncludeBlockFactory.INCLUDE_BLOCK_TREES_KEY, out object treesObject);
                var includeBlockTrees = treesObject as List<IncludeBlock>;
                Assert.NotNull(includeBlockTrees);
                Assert.Equal(2, includeBlockTrees.Count);
                // First IncludeBlock includes source 1
                Assert.Equal(dummySource1AbsolutePath, includeBlockTrees[0].Source.AbsolutePath);
                // Second IncludeBlock includes source 2, which includes source 3
                Assert.Equal(dummySource2AbsolutePath, includeBlockTrees[1].Source.AbsolutePath);
                Assert.Single(includeBlockTrees[1].Children);
                Assert.Equal(dummySource3AbsolutePath, includeBlockTrees[1].Children[0].Source.AbsolutePath);
                Assert.Equal(includeBlockTrees[1], includeBlockTrees[1].Children[0].ParentIncludeBlock);
            }
        }

        [Theory]
        [MemberData(nameof(IncludeBlocks_ThrowsBlockExceptionIfACycleIsFound_Data))]
        public void IncludeBlocks_ThrowsBlockExceptionIfACycleIsFound(string dummyRootContent, int dummyEntryInvalidIncludeBlockLineNumber,
            string dummyContent1, int dummyContent1InvalidIncludeBlockLineNumber,
            string dummyContent2, int dummyContent2InvalidIncludeBlockLineNumber,
            string dummyContent3,
            string expectedCycleDescription)
        {
            // Arrange
            var dummySource1Uri = new Uri($"{_fixture.TempDirectory}/{nameof(dummyContent1)}.md");
            var dummySource2Uri = new Uri($"{_fixture.TempDirectory}/{nameof(dummyContent2)}.md");
            var dummySource3Uri = new Uri($"{_fixture.TempDirectory}/{nameof(dummyContent3)}.md");
            File.WriteAllText(dummySource1Uri.AbsolutePath, dummyContent1);
            File.WriteAllText(dummySource2Uri.AbsolutePath, dummyContent2);
            File.WriteAllText(dummySource3Uri.AbsolutePath, dummyContent3);

            // Need to dispose of services after each test so that the in-memory cache doesn't affect results
            var services = new ServiceCollection();
            services.AddIncludeBlocks();
            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                // TODO use service provider to manually add extension instead of UseIncludeBlocks!
                var dummyMarkdownPipelineBuilder = new MarkdownPipelineBuilder();
                dummyMarkdownPipelineBuilder.Extensions.Add(serviceProvider.GetRequiredService<IBlockExtension<IncludeBlock>>());
                MarkdownPipeline dummyMarkdownPipeline = dummyMarkdownPipelineBuilder.Build();
                var dummyMarkdownParserContext = new MarkdownParserContext();
                dummyMarkdownParserContext.Properties[typeof(IIncludeBlocksExtensionOptions)] = new IncludeBlocksExtensionOptions(baseUri: _fixture.TempDirectory + "/");

                // Act and assert
                BlockException result = Assert.Throws<BlockException>(() => MarkdownParser.Parse(dummyRootContent, dummyMarkdownPipeline, dummyMarkdownParserContext));
                Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock, nameof(IncludeBlock), dummyEntryInvalidIncludeBlockLineNumber, 0,
                        string.Format(Strings.BlockException_IncludeBlockFactory_ExceptionOccurredWhileProcessingContent,
                            dummySource1Uri.AbsoluteUri)),
                    result.Message);
                Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock, nameof(IncludeBlock), dummyContent1InvalidIncludeBlockLineNumber, 0,
                        string.Format(Strings.BlockException_IncludeBlockFactory_ExceptionOccurredWhileProcessingContent,
                            dummySource2Uri.AbsoluteUri)),
                    result.InnerException.Message);
                Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock, nameof(IncludeBlock), dummyContent2InvalidIncludeBlockLineNumber, 0,
                        string.Format(Strings.BlockException_IncludeBlockFactory_ExceptionOccurredWhileProcessingContent,
                            dummySource1Uri.AbsoluteUri)),
                    result.InnerException.InnerException.Message);
                Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock, nameof(IncludeBlock), dummyContent1InvalidIncludeBlockLineNumber, 0,
                        Strings.BlockException_BlockException_ExceptionOccurredWhileProcessingBlock),
                    result.InnerException.InnerException.InnerException.Message);
                Assert.Equal(string.Format(Strings.InvalidOperationException_IncludeBlockFactory_CycleFound,
                        string.Format(expectedCycleDescription, dummySource1Uri.AbsoluteUri, dummySource2Uri.AbsoluteUri)),
                    result.InnerException.InnerException.InnerException.InnerException.Message,
                    ignoreLineEndingDifferences: true);
            }
        }

        public static IEnumerable<object[]> IncludeBlocks_ThrowsBlockExceptionIfACycleIsFound_Data()
        {
            return new object[][]
            {
                // Basic circular include
                new object[]
                {
                    @"+{
""type"": ""markdown"",
""source"": ""./dummyContent1.md""
}",
                    1,
                    @"+{
""type"": ""markdown"",
""source"": ""./dummyContent2.md""
}",
                    1,
                    @"+{
""type"": ""markdown"",
""source"": ""./dummyContent1.md""
}",
                    1,
                    null,
                    @"Source: {0}, Line Number: 1 >
Source: {1}, Line Number: 1 >
Source: {0}, Line Number: 1"
                },
                // Valid includes don't affect identification of circular includes
                new object[]
                {
                    @"+{
""type"": ""markdown"",
""source"": ""./dummyContent1.md"",
""clippings"": [{""start"": 2, ""end"": 2}]
}

+{
""type"": ""markdown"",
""source"": ""./dummyContent1.md""
}",
                    7,
                    @"+{
""type"": ""markdown"",
""source"": ""./dummyContent3.md""
}

+{
""type"": ""markdown"",
""source"": ""./dummyContent2.md""
}",
                    6,
                    @"+{
""type"": ""Code"",
""source"": ""./dummyContent1.md""
}

+{
""type"": ""markdown"",
""source"": ""./dummyContent1.md""
}",
                    6,
                    "This is a line",
                    @"Source: {0}, Line Number: 6 >
Source: {1}, Line Number: 6 >
Source: {0}, Line Number: 6"
                },
                // Circular includes that uses clippings are caught
                new object[]
                {
                    @"+{
""type"": ""markdown"",
""source"": ""./dummyContent1.md"",
""clippings"": [{""start"": 2, ""end"": 2}]
}

+{
""type"": ""markdown"",
""source"": ""./dummyContent1.md"",
""clippings"": [{""start"": 6}]
}",
                    7,
                    @"+{
""type"": ""markdown"",
""source"": ""./dummyContent3.md""
}

+{
""type"": ""markdown"",
""source"": ""./dummyContent2.md"",
""clippings"": [{""start"": 6}]
}",
                    6,
                    @"+{
""type"": ""Code"",
""source"": ""./dummyContent1.md""
}

+{
""type"": ""markdown"",
""source"": ""./dummyContent1.md""
}",
                    6,
                    "This is a line",
                    @"Source: {0}, Line Number: 6 >
Source: {1}, Line Number: 6 >
Source: {0}, Line Number: 6"
                }
            };
        }

        [Fact]
        public void IncludeBlocks_ThrowsBlockExceptionIfAnIncludedContentHasAnInvalidBlock()
        {
            // Arrange
            const int dummyRenderingMode = 12;
            const string dummyRootContent = @"
+{
    ""type"": ""markdown"",
    ""source"": ""./dummyContent1.md""
}";
            string dummyContent1 = $@"This is valid markdown.

@{{
    ""renderingMode"": ""{dummyRenderingMode}""
}}
```
This is a FlexiCodeBlock with an invalid option.
```
";
            // Need to dispose of services after each test so that the in-memory cache doesn't affect results
            var services = new ServiceCollection();
            services.
                AddOptionsBlocks().
                AddIncludeBlocks().
                AddFlexiCodeBlocks();

            // Write to file
            var dummySource1Uri = new Uri($"{_fixture.TempDirectory}/{nameof(dummyContent1)}.md");
            File.WriteAllText(dummySource1Uri.AbsolutePath, dummyContent1);

            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                var dummyMarkdownPipelineBuilder = new MarkdownPipelineBuilder();
                dummyMarkdownPipelineBuilder.Extensions.Add(serviceProvider.GetRequiredService<IBlockExtension<IncludeBlock>>());
                dummyMarkdownPipelineBuilder.Extensions.Add(serviceProvider.GetRequiredService<IBlockExtension<OptionsBlock>>());
                dummyMarkdownPipelineBuilder.Extensions.Add(serviceProvider.GetRequiredService<IBlockExtension<FlexiCodeBlock>>());
                MarkdownPipeline dummyMarkdownPipeline = dummyMarkdownPipelineBuilder.Build();
                var dummyMarkdownParserContext = new MarkdownParserContext();
                dummyMarkdownParserContext.Properties[typeof(IIncludeBlocksExtensionOptions)] = new IncludeBlocksExtensionOptions(baseUri: _fixture.TempDirectory + "/");

                // Act and assert
                BlockException result = Assert.Throws<BlockException>(() => MarkdownParser.Parse(dummyRootContent, dummyMarkdownPipeline, dummyMarkdownParserContext));
                // From bottom to top, this is the exception chain: 
                // OptionsException > BlockException for invalid FlexiCodeBlock > BlockException for invalid IncludeBlock
                Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock, nameof(IncludeBlock), 2, 0,
                        string.Format(Strings.BlockException_IncludeBlockFactory_ExceptionOccurredWhileProcessingContent, dummySource1Uri.AbsoluteUri)),
                    result.Message);
                Assert.IsType<BlockException>(result.InnerException);
                Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock, nameof(FlexiCodeBlock), 6, 0,
                        Strings.BlockException_BlockException_ExceptionOccurredWhileProcessingBlock),
                    result.InnerException.Message);
                Assert.IsType<OptionsException>(result.InnerException.InnerException);
                Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                        nameof(IFlexiCodeBlockOptions.RenderingMode),
                        string.Format(Strings.OptionsException_Shared_ValueMustBeAValidEnumValue, dummyRenderingMode, nameof(FlexiCodeBlockRenderingMode))),
                    result.InnerException.InnerException.Message);
            }
        }
    }

    public class IncludeBlocksIntegrationTestsFixture : IDisposable
    {
        public string TempDirectory { get; } = Path.Combine(Path.GetTempPath(), nameof(IncludeBlocksIntegrationTests));

        public IncludeBlocksIntegrationTestsFixture()
        {
            TryDeleteDirectory();
            Directory.CreateDirectory(TempDirectory);
        }

        private void TryDeleteDirectory()
        {
            try
            {
                Directory.Delete(TempDirectory, true);
            }
            catch
            {
                // Do nothing
            }
        }

        public void Dispose()
        {
            TryDeleteDirectory();
        }
    }
}
