using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Markdig;
using Markdig.Parsers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiIncludeBlocks
{
    // Integration tests that don't fit in amongst the specs.
    public class FlexiIncludeBlocksIntegrationTests : IClassFixture<FlexiIncludeBlocksIntegrationTestsFixture>
    {
        private readonly FlexiIncludeBlocksIntegrationTestsFixture _fixture;

        public FlexiIncludeBlocksIntegrationTests(FlexiIncludeBlocksIntegrationTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void FlexiIncludeBlocks_ConstructsAndExposesFlexiIncludeBlockTrees()
        {
            // Arrange
            const string dummySource1 = "./dummyContent1.md";
            const string dummySource2 = "./dummyContent2.md";
            const string dummySource3 = "./dummyContent3.md";
            string dummyRootContent = $@"i{{
""type"": ""markdown"",
""source"": ""{dummySource1}"",
}}

i{{
""type"": ""markdown"",
""source"": ""{dummySource2}""
}}";
            const string dummyContent1 = "This is dummy markdown";
            string dummyContent2 = $@"i{{
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
            services.AddFlexiIncludeBlocks();
            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                var dummyMarkdownPipelineBuilder = new MarkdownPipelineBuilder();
                dummyMarkdownPipelineBuilder.Extensions.Add(serviceProvider.GetRequiredService<IBlockExtension<FlexiIncludeBlock>>());
                MarkdownPipeline dummyMarkdownPipeline = dummyMarkdownPipelineBuilder.Build();
                var dummyMarkdownParserContext = new MarkdownParserContext();
                dummyMarkdownParserContext.Properties[typeof(IFlexiIncludeBlocksExtensionOptions)] = new FlexiIncludeBlocksExtensionOptions(baseUri: _fixture.TempDirectory + "/");

                // Act
                MarkdownParser.Parse(dummyRootContent, dummyMarkdownPipeline, dummyMarkdownParserContext);

                // Assert
                dummyMarkdownParserContext.Properties.TryGetValue(FlexiIncludeBlockFactory.FLEXI_INCLUDE_BLOCK_TREES_KEY, out object treesObject);
                var flexiIncludeBlockTrees = treesObject as List<FlexiIncludeBlock>;
                Assert.NotNull(flexiIncludeBlockTrees);
                Assert.Equal(2, flexiIncludeBlockTrees.Count);
                // First FlexiIncludeBlock includes source 1
                Assert.Equal(dummySource1AbsolutePath, flexiIncludeBlockTrees[0].Source.AbsolutePath);
                // Second FlexiIncludeBlock includes source 2, which includes source 3
                Assert.Equal(dummySource2AbsolutePath, flexiIncludeBlockTrees[1].Source.AbsolutePath);
                Assert.Single(flexiIncludeBlockTrees[1].Children);
                Assert.Equal(dummySource3AbsolutePath, flexiIncludeBlockTrees[1].Children[0].Source.AbsolutePath);
                Assert.Equal(flexiIncludeBlockTrees[1], flexiIncludeBlockTrees[1].Children[0].ParentFlexiIncludeBlock);
            }
        }

        [Theory]
        [MemberData(nameof(FlexiIncludeBlocks_ThrowsBlockExceptionIfACycleIsFound_Data))]
        public void FlexiIncludeBlocks_ThrowsBlockExceptionIfACycleIsFound(string dummyRootContent, int dummyEntryInvalidFlexiIncludeBlockLineNumber,
            string dummyContent1, int dummyContent1InvalidFlexiIncludeBlockLineNumber,
            string dummyContent2, int dummyContent2InvalidFlexiIncludeBlockLineNumber,
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
            services.AddFlexiIncludeBlocks();
            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                // TODO use service provider to manually add extension instead of UseFlexiIncludeBlocks!
                var dummyMarkdownPipelineBuilder = new MarkdownPipelineBuilder();
                dummyMarkdownPipelineBuilder.Extensions.Add(serviceProvider.GetRequiredService<IBlockExtension<FlexiIncludeBlock>>());
                MarkdownPipeline dummyMarkdownPipeline = dummyMarkdownPipelineBuilder.Build();
                var dummyMarkdownParserContext = new MarkdownParserContext();
                dummyMarkdownParserContext.Properties[typeof(IFlexiIncludeBlocksExtensionOptions)] = new FlexiIncludeBlocksExtensionOptions(baseUri: _fixture.TempDirectory + "/");

                // Act and assert
                BlockException result = Assert.Throws<BlockException>(() => MarkdownParser.Parse(dummyRootContent, dummyMarkdownPipeline, dummyMarkdownParserContext));
                Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock, nameof(FlexiIncludeBlock), dummyEntryInvalidFlexiIncludeBlockLineNumber, 0,
                        string.Format(Strings.BlockException_FlexiIncludeBlockFactory_ExceptionOccurredWhileProcessingContent,
                            dummySource1Uri.AbsoluteUri)),
                    result.Message);
                Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock, nameof(FlexiIncludeBlock), dummyContent1InvalidFlexiIncludeBlockLineNumber, 0,
                        string.Format(Strings.BlockException_FlexiIncludeBlockFactory_ExceptionOccurredWhileProcessingContent,
                            dummySource2Uri.AbsoluteUri)),
                    result.InnerException.Message);
                Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock, nameof(FlexiIncludeBlock), dummyContent2InvalidFlexiIncludeBlockLineNumber, 0,
                        string.Format(Strings.BlockException_FlexiIncludeBlockFactory_ExceptionOccurredWhileProcessingContent,
                            dummySource1Uri.AbsoluteUri)),
                    result.InnerException.InnerException.Message);
                Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock, nameof(FlexiIncludeBlock), dummyContent1InvalidFlexiIncludeBlockLineNumber, 0,
                        Strings.BlockException_BlockException_ExceptionOccurredWhileProcessingBlock),
                    result.InnerException.InnerException.InnerException.Message);
                Assert.Equal(string.Format(Strings.InvalidOperationException_FlexiIncludeBlockFactory_CycleFound,
                        string.Format(expectedCycleDescription, dummySource1Uri.AbsoluteUri, dummySource2Uri.AbsoluteUri)),
                    result.InnerException.InnerException.InnerException.InnerException.Message,
                    ignoreLineEndingDifferences: true);
            }
        }

        public static IEnumerable<object[]> FlexiIncludeBlocks_ThrowsBlockExceptionIfACycleIsFound_Data()
        {
            return new object[][]
            {
                // Basic circular include
                new object[]
                {
                    @"i{
""type"": ""markdown"",
""source"": ""./dummyContent1.md""
}",
                    1,
                    @"i{
""type"": ""markdown"",
""source"": ""./dummyContent2.md""
}",
                    1,
                    @"i{
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
                    @"i{
""type"": ""markdown"",
""source"": ""./dummyContent1.md"",
""clippings"": [{""startLine"": 2, ""endLine"": 2}]
}

i{
""type"": ""markdown"",
""source"": ""./dummyContent1.md""
}",
                    7,
                    @"i{
""type"": ""markdown"",
""source"": ""./dummyContent3.md""
}

i{
""type"": ""markdown"",
""source"": ""./dummyContent2.md""
}",
                    6,
                    @"i{
""type"": ""Code"",
""source"": ""./dummyContent1.md""
}

i{
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
                    @"i{
""type"": ""markdown"",
""source"": ""./dummyContent1.md"",
""clippings"": [{""startLine"": 2, ""endLine"": 2}]
}

i{
""type"": ""markdown"",
""source"": ""./dummyContent1.md"",
""clippings"": [{""startLine"": 6}]
}",
                    7,
                    @"i{
""type"": ""markdown"",
""source"": ""./dummyContent3.md""
}

i{
""type"": ""markdown"",
""source"": ""./dummyContent2.md"",
""clippings"": [{""startLine"": 6}]
}",
                    6,
                    @"i{
""type"": ""Code"",
""source"": ""./dummyContent1.md""
}

i{
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
        public void FlexiIncludeBlocks_ThrowsBlockExceptionIfAnIncludedContentHasAnInvalidBlock()
        {
            // Arrange
            const int dummyRenderingMode = 12;
            const string dummyRootContent = @"
i{
    ""type"": ""markdown"",
    ""source"": ""./dummyContent1.md""
}";
            string dummyContent1 = $@"This is valid markdown.

o{{
    ""renderingMode"": ""{dummyRenderingMode}""
}}
```
This is a FlexiCodeBlock with an invalid option.
```
";
            // Need to dispose of services after each test so that the in-memory cache doesn't affect results
            var services = new ServiceCollection();
            services.
                AddFlexiOptionsBlocks().
                AddFlexiIncludeBlocks().
                AddFlexiCodeBlocks();

            // Write to file
            var dummySource1Uri = new Uri($"{_fixture.TempDirectory}/{nameof(dummyContent1)}.md");
            File.WriteAllText(dummySource1Uri.AbsolutePath, dummyContent1);

            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                var dummyMarkdownPipelineBuilder = new MarkdownPipelineBuilder();
                dummyMarkdownPipelineBuilder.Extensions.Add(serviceProvider.GetRequiredService<IBlockExtension<FlexiIncludeBlock>>());
                dummyMarkdownPipelineBuilder.Extensions.Add(serviceProvider.GetRequiredService<IBlockExtension<FlexiOptionsBlock>>());
                dummyMarkdownPipelineBuilder.Extensions.Add(serviceProvider.GetRequiredService<IBlockExtension<FlexiCodeBlock>>());
                MarkdownPipeline dummyMarkdownPipeline = dummyMarkdownPipelineBuilder.Build();
                var dummyMarkdownParserContext = new MarkdownParserContext();
                dummyMarkdownParserContext.Properties[typeof(IFlexiIncludeBlocksExtensionOptions)] = new FlexiIncludeBlocksExtensionOptions(baseUri: _fixture.TempDirectory + "/");

                // Act and assert
                BlockException result = Assert.Throws<BlockException>(() => MarkdownParser.Parse(dummyRootContent, dummyMarkdownPipeline, dummyMarkdownParserContext));
                // From bottom to top, this is the exception chain: 
                // OptionsException > BlockException for invalid FlexiCodeBlock > BlockException for invalid FlexiIncludeBlock
                Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock, nameof(FlexiIncludeBlock), 2, 0,
                        string.Format(Strings.BlockException_FlexiIncludeBlockFactory_ExceptionOccurredWhileProcessingContent, dummySource1Uri.AbsoluteUri)),
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

    public class FlexiIncludeBlocksIntegrationTestsFixture : IDisposable
    {
        public string TempDirectory { get; } = Path.Combine(Path.GetTempPath(), nameof(FlexiIncludeBlocksIntegrationTests));

        public FlexiIncludeBlocksIntegrationTestsFixture()
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
