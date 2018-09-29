using Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
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
    // As far as integration tests for FlexiIncludeBlocks go, success cases are covered in FlexiIncludeBlockSpecs, so here we just test for exceptions.
    public class FlexiIncludeBlocksIntegrationTests : IClassFixture<FlexiIncludeBlocksIntegrationTestsFixture>
    {
        private readonly FlexiIncludeBlocksIntegrationTestsFixture _fixture;

        public FlexiIncludeBlocksIntegrationTests(FlexiIncludeBlocksIntegrationTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [MemberData(nameof(FlexiIncludeBlocks_ThrowsFlexiIncludeBlocksExceptionIfACycleIsFound_Data))]
        public void FlexiIncludeBlocks_ThrowsFlexiIncludeBlocksExceptionIfACycleIsFound(string dummyEntryMarkdown, int dummyEntryOffendingFIBLineNum,
            string dummyMarkdown1, int dummyMarkdown1OffendingFIBLineNum,
            string dummyMarkdown2, int dummyMarkdown2OffendingFIBLineNum,
            string dummyMarkdown3,
            string expectedCycleDescription)
        {
            // Arrange
            File.WriteAllText(Path.Combine(_fixture.TempDirectory, $"{nameof(dummyMarkdown1)}.md"), dummyMarkdown1);
            File.WriteAllText(Path.Combine(_fixture.TempDirectory, $"{nameof(dummyMarkdown2)}.md"), dummyMarkdown2);
            File.WriteAllText(Path.Combine(_fixture.TempDirectory, $"{nameof(dummyMarkdown3)}.md"), dummyMarkdown3);

            // Need to dispose of services between tests so that DiskCacheService's in memory cache doesn't affect results
            var services = new ServiceCollection();
            services.AddFlexiBlocks();
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            using ((IDisposable)serviceProvider)
            {
                var dummyMarkdownPipelineBuilder = new MarkdownPipelineBuilder();
                FlexiIncludeBlocksExtension flexiIncludeBlocksExtension = serviceProvider.GetRequiredService<FlexiIncludeBlocksExtension>();
                dummyMarkdownPipelineBuilder.Extensions.Add(flexiIncludeBlocksExtension);
                FlexiIncludeBlocksExtensionOptions dummyExtensionOptions = serviceProvider.GetRequiredService<IOptions<FlexiIncludeBlocksExtensionOptions>>().Value;
                dummyExtensionOptions.RootBaseUri = _fixture.TempDirectory + "/";
                var dummyRootBaseUri = new Uri(dummyExtensionOptions.RootBaseUri);
                string dummyMarkdown1SourceUri = dummyRootBaseUri + $"{nameof(dummyMarkdown1)}.md";
                string dummyMarkdown2SourceUri = dummyRootBaseUri + $"{nameof(dummyMarkdown2)}.md";
                MarkdownPipeline dummyMarkdownPipeline = dummyMarkdownPipelineBuilder.Build();

                // Act and assert
                FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => MarkdownParser.Parse(dummyEntryMarkdown, dummyMarkdownPipeline));
                Assert.Equal(string.Format(Strings.FlexiBlocksException_InvalidFlexiBlock, nameof(FlexiIncludeBlock), dummyEntryOffendingFIBLineNum, 0,
                        string.Format(Strings.FlexiBlocksException_FlexiIncludeBlocks_ExceptionOccurredWhileProcessingSource,
                            dummyMarkdown1SourceUri)),
                    result.Message);
                Assert.Equal(string.Format(Strings.FlexiBlocksException_InvalidFlexiBlock, nameof(FlexiIncludeBlock), dummyMarkdown1OffendingFIBLineNum, 0,
                        string.Format(Strings.FlexiBlocksException_FlexiIncludeBlocks_ExceptionOccurredWhileProcessingSource,
                            dummyMarkdown2SourceUri)),
                    result.InnerException.Message);
                Assert.Equal(string.Format(Strings.FlexiBlocksException_InvalidFlexiBlock, nameof(FlexiIncludeBlock), dummyMarkdown2OffendingFIBLineNum, 0,
                        string.Format(Strings.FlexiBlocksException_FlexiIncludeBlocks_ExceptionOccurredWhileProcessingSource,
                            dummyMarkdown1SourceUri)),
                    result.InnerException.InnerException.Message);
                Assert.Equal(string.Format(Strings.FlexiBlocksException_InvalidFlexiBlock, nameof(FlexiIncludeBlock), dummyMarkdown1OffendingFIBLineNum, 0,
                        Strings.FlexiBlocksException_FlexiIncludeBlocks_ExceptionOccurredWhileProcessingBlock),
                    result.InnerException.InnerException.InnerException.Message);
                Assert.Equal(string.Format(Strings.FlexiBlocksException_FlexiIncludeBlocks_CycleFound,
                        string.Format(expectedCycleDescription, dummyMarkdown1SourceUri, dummyMarkdown2SourceUri)),
                    result.InnerException.InnerException.InnerException.InnerException.Message,
                    ignoreLineEndingDifferences: true);
            }
        }

        public static IEnumerable<object[]> FlexiIncludeBlocks_ThrowsFlexiIncludeBlocksExceptionIfACycleIsFound_Data()
        {
            return new object[][]
            {
                // Basic circular include
                new object[]
                {
                    @"+{
""type"": ""markdown"",
""sourceUri"": ""./dummyMarkdown1.md""
}",
                    1,
                    @"+{
""type"": ""markdown"",
""sourceUri"": ""./dummyMarkdown2.md""
}",
                    1,
                    @"+{
""type"": ""markdown"",
""sourceUri"": ""./dummyMarkdown1.md""
}",
                    1,
                    null,
                    @"Source URI: {0}, Line: 1 >
Source URI: {1}, Line: 1 >
Source URI: {0}, Line: 1"
                },
                // Valid includes don't affect identification of circular includes
                new object[]
                {
                    @"+{
""type"": ""markdown"",
""sourceUri"": ""./dummyMarkdown1.md"",
""clippings"": [{""startLineNumber"": 2, ""endLineNumber"": 2}]
}

+{
""type"": ""markdown"",
""sourceUri"": ""./dummyMarkdown1.md""
}",
                    7,
                    @"+{
""type"": ""markdown"",
""sourceUri"": ""./dummyMarkdown3.md""
}

+{
""type"": ""markdown"",
""sourceUri"": ""./dummyMarkdown2.md""
}",
                    6,
                    @"+{
""type"": ""Code"",
""sourceUri"": ""./dummyMarkdown1.md""
}

+{
""type"": ""markdown"",
""sourceUri"": ""./dummyMarkdown1.md""
}",
                    6,
                    "This is a line",
                    @"Source URI: {0}, Line: 6 >
Source URI: {1}, Line: 6 >
Source URI: {0}, Line: 6"
                },
                // Circular includes that uses clippings are caught
                new object[]
                {
                    @"+{
""type"": ""markdown"",
""sourceUri"": ""./dummyMarkdown1.md"",
""clippings"": [{""startLineNumber"": 2, ""endLineNumber"": 2}]
}

+{
""type"": ""markdown"",
""sourceUri"": ""./dummyMarkdown1.md"",
""clippings"": [{""startLineNumber"": 6, ""endLineNumber"": -1}]
}",
                    7,
                    @"+{
""type"": ""markdown"",
""sourceUri"": ""./dummyMarkdown3.md""
}

+{
""type"": ""markdown"",
""sourceUri"": ""./dummyMarkdown2.md"",
""clippings"": [{""startLineNumber"": 6, ""endLineNumber"": -1}]
}",
                    6,
                    @"+{
""type"": ""Code"",
""sourceUri"": ""./dummyMarkdown1.md""
}

+{
""type"": ""markdown"",
""sourceUri"": ""./dummyMarkdown1.md""
}",
                    6,
                    "This is a line",
                    @"Source URI: {0}, Line: 6 >
Source URI: {1}, Line: 6 >
Source URI: {0}, Line: 6"
                }
            };
        }

        // This test is similar to the theory above.The thing is that, messages differ for before/after content.
        // The exception chain is stupidly long. It is a cycle, and we need the context for each FlexiIncludeBlock, but
        // some kind of simplification should be attempted if time permits.
        [Fact]
        public void FlexiIncludeBlocks_ThrowsFlexiIncludeBlocksExceptionIfACycleThatPassesThroughBeforeOrAfterContentIsFound()
        {
            // Arrange
            const string dummyEntryMarkdown = @"+{
    ""type"": ""markdown"",
    ""sourceUri"": ""./dummyMarkdown1.md""
}";
            const int dummyEntryOffendingFIBLineNum = 1;
            const string dummyMarkdown1 = @"+{
    ""type"": ""markdown"",
    ""sourceUri"": ""./dummyMarkdown3.md"",
    ""clippings"": [{
                        ""beforeContent"": ""This is a line.
+{
                            \""type\"": \""markdown\"",
                            \""sourceUri\"": \""./dummyMarkdown2.md\""
                        }""
                    }]
}";
            const int dummyMarkdown1OffendingFIBLineNum = 1;
            const string dummyMarkdown2 = @"+{
    ""type"": ""markdown"",
    ""sourceUri"": ""./dummyMarkdown3.md"",
    ""clippings"": [{
                        ""afterContent"": ""+{
                            \""type\"": \""markdown\"",
                            \""sourceUri\"": \""./dummyMarkdown1.md\""
                        }""
                    }]
}";
            const string dummyMarkdown3 = "This is a line.";
            const string expectedCycleDescription = @"Source URI: {0}, Line: 1 >
Source URI: {0}, Line: 1, BeforeContent >
Source URI: {1}, Line: 1 >
Source URI: {1}, Line: 1, AfterContent >
Source URI: {0}, Line: 1";
            File.WriteAllText(Path.Combine(_fixture.TempDirectory, $"{nameof(dummyMarkdown1)}.md"), dummyMarkdown1);
            File.WriteAllText(Path.Combine(_fixture.TempDirectory, $"{nameof(dummyMarkdown2)}.md"), dummyMarkdown2);
            File.WriteAllText(Path.Combine(_fixture.TempDirectory, $"{nameof(dummyMarkdown3)}.md"), dummyMarkdown3);

            // Need to dispose of services between tests so that DiskCacheService's in memory cache doesn't affect results
            var services = new ServiceCollection();
            services.AddFlexiBlocks();
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            using ((IDisposable)serviceProvider)
            {
                var dummyMarkdownPipelineBuilder = new MarkdownPipelineBuilder();
                FlexiIncludeBlocksExtension flexiIncludeBlocksExtension = serviceProvider.GetRequiredService<FlexiIncludeBlocksExtension>();
                dummyMarkdownPipelineBuilder.Extensions.Add(flexiIncludeBlocksExtension);
                FlexiIncludeBlocksExtensionOptions dummyExtensionOptions = serviceProvider.GetRequiredService<IOptions<FlexiIncludeBlocksExtensionOptions>>().Value;
                dummyExtensionOptions.RootBaseUri = _fixture.TempDirectory + "/";
                var dummyRootBaseUri = new Uri(dummyExtensionOptions.RootBaseUri);
                string dummyMarkdown1SourceUri = dummyRootBaseUri + $"{nameof(dummyMarkdown1)}.md";
                string dummyMarkdown2SourceUri = dummyRootBaseUri + $"{nameof(dummyMarkdown2)}.md";
                MarkdownPipeline dummyMarkdownPipeline = dummyMarkdownPipelineBuilder.Build();

                // Act and assert
                FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => MarkdownParser.Parse(dummyEntryMarkdown, dummyMarkdownPipeline));
                Assert.Equal(string.Format(Strings.FlexiBlocksException_InvalidFlexiBlock, nameof(FlexiIncludeBlock), dummyEntryOffendingFIBLineNum, 0,
                        string.Format(Strings.FlexiBlocksException_FlexiIncludeBlocks_ExceptionOccurredWhileProcessingSource,
                            dummyMarkdown1SourceUri)),
                    result.Message);
                Assert.Equal(string.Format(Strings.FlexiBlocksException_InvalidFlexiBlock, nameof(FlexiIncludeBlock), dummyMarkdown1OffendingFIBLineNum, 0,
                        string.Format(Strings.FlexiBlocksException_FlexiIncludeBlocks_ExceptionOccurredWhileProcessingContent, nameof(ClippingProcessingStage.BeforeContent))),
                    result.InnerException.Message);
                Assert.Equal(string.Format(Strings.FlexiBlocksException_InvalidFlexiBlock, nameof(FlexiIncludeBlock), dummyMarkdown1OffendingFIBLineNum, 0,
                        string.Format(Strings.FlexiBlocksException_FlexiIncludeBlocks_ExceptionOccurredWhileProcessingSource,
                            dummyMarkdown2SourceUri)),
                    result.InnerException.InnerException.Message);
                Assert.Equal(string.Format(Strings.FlexiBlocksException_InvalidFlexiBlock, nameof(FlexiIncludeBlock), dummyMarkdown1OffendingFIBLineNum, 0,
                        string.Format(Strings.FlexiBlocksException_FlexiIncludeBlocks_ExceptionOccurredWhileProcessingContent, nameof(ClippingProcessingStage.AfterContent))),
                    result.InnerException.InnerException.InnerException.Message);
                Assert.Equal(string.Format(Strings.FlexiBlocksException_InvalidFlexiBlock, nameof(FlexiIncludeBlock), dummyMarkdown1OffendingFIBLineNum, 0,
                        string.Format(Strings.FlexiBlocksException_FlexiIncludeBlocks_ExceptionOccurredWhileProcessingSource,
                            dummyMarkdown1SourceUri)),
                    result.InnerException.InnerException.InnerException.InnerException.Message);
                Assert.Equal(string.Format(Strings.FlexiBlocksException_InvalidFlexiBlock, nameof(FlexiIncludeBlock), dummyMarkdown1OffendingFIBLineNum, 0,
                        Strings.FlexiBlocksException_FlexiIncludeBlocks_ExceptionOccurredWhileProcessingBlock),
                    result.InnerException.InnerException.InnerException.InnerException.InnerException.Message);
                Assert.Equal(string.Format(Strings.FlexiBlocksException_FlexiIncludeBlocks_CycleFound,
                        string.Format(expectedCycleDescription, dummyMarkdown1SourceUri, dummyMarkdown2SourceUri)),
                    result.InnerException.InnerException.InnerException.InnerException.InnerException.InnerException.Message,
                    ignoreLineEndingDifferences: true);
            }
        }

        [Fact]
        public void FlexiIncludeBlocks_ThrowsFlexiIncludeBlocksExceptionIfAnIncludedSourceHasInvalidBlocks()
        {
            // Arrange
            const string dummyClassFormat = "dummy-{0}-{1}";
            const string dummyEntryMarkdown = @"+{
    ""type"": ""markdown"",
    ""sourceUri"": ""./dummyMarkdown1.md""
}";
            string dummyMarkdown1 = $@"@{{
    ""classFormat"": ""{dummyClassFormat}""
}}
! This is a FlexiAlertBlock.
";
            var dummyFlexiIncludeBlocksExtensionOptions = new FlexiIncludeBlocksExtensionOptions
            {
                RootBaseUri = _fixture.TempDirectory + "/"
            };
            var dummyRootBaseUri = new Uri(dummyFlexiIncludeBlocksExtensionOptions.RootBaseUri);
            File.WriteAllText(Path.Combine(dummyRootBaseUri.AbsolutePath, $"{nameof(dummyMarkdown1)}.md"), dummyMarkdown1);
            var dummyMarkdownPipelineBuilder = new MarkdownPipelineBuilder();
            dummyMarkdownPipelineBuilder.
                UseFlexiIncludeBlocks(dummyFlexiIncludeBlocksExtensionOptions).
                UseFlexiAlertBlocks().
                UseFlexiOptionsBlocks();
            MarkdownPipeline dummyMarkdownPipeline = dummyMarkdownPipelineBuilder.Build();

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => MarkdownParser.Parse(dummyEntryMarkdown, dummyMarkdownPipeline));
            // From bottom to top, this is the exception chain:
            // FormatException > FlexiBlocksException for invalid option > FlexiBlocksException for invalid FlexiOptionsBlock > FlexiBlocksException for invalid FlexiIncludeBlock
            Assert.Equal(string.Format(Strings.FlexiBlocksException_InvalidFlexiBlock, nameof(FlexiIncludeBlock), 1, 0,
                    string.Format(Strings.FlexiBlocksException_FlexiIncludeBlocks_ExceptionOccurredWhileProcessingSource,
                        dummyRootBaseUri + $"{nameof(dummyMarkdown1)}.md")),
                result.Message);
            Assert.Equal(string.Format(Strings.FlexiBlocksException_InvalidFlexiBlock, nameof(FlexiOptionsBlock), 1, 0, Strings.FlexiBlocksException_ExceptionOccurredWhileProcessingABlock),
                result.InnerException.Message);
            Assert.Equal(string.Format(Strings.FlexiBlocksException_OptionIsAnInvalidFormat, nameof(FlexiAlertBlockOptions.ClassFormat), dummyClassFormat),
                result.InnerException.InnerException.Message);
            Assert.IsType<FormatException>(result.InnerException.InnerException.InnerException);
        }
    }

    public class FlexiIncludeBlocksIntegrationTestsFixture : IDisposable
    {
        public string TempDirectory { get; } = Path.Combine(Path.GetTempPath(), nameof(FlexiIncludeBlocksIntegrationTests)); // Dummy file for creating dummy file streams

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
