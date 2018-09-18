using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Markdig;
using Markdig.Parsers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiAlertBlocks
{
    // Incomplete, we should test common unrecoverable situations to ensure that exception messages are detailed enough to be actionable
    public class FlexiAlertBlocksIntegrationTests
    {
        [Theory]
        [MemberData(nameof(FlexiAlertBlockParser_ThrowsFlexiBlocksExceptionsWithBlockContext_Data))]
        public void FlexiAlertBlockParser_ThrowsFlexiBlocksExceptionsWithBlockContext(string dummyMarkdown, string expectedMessage)
        {
            // Arrange
            // Need to dispose of services between tests so that FileCacheService's in memory cache doesn't affect results
            var services = new ServiceCollection();
            services.AddFlexiBlocks();
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            using ((IDisposable)serviceProvider)
            {
                var dummyMarkdownPipelineBuilder = new MarkdownPipelineBuilder();
                dummyMarkdownPipelineBuilder.
                    UseFlexiOptionsBlocks(serviceProvider: serviceProvider).
                    UseFlexiAlertBlocks(serviceProvider: serviceProvider);
                MarkdownPipeline dummyMarkdownPipeline = dummyMarkdownPipelineBuilder.Build();

                // Act and assert
                FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => MarkdownParser.Parse(dummyMarkdown, dummyMarkdownPipeline));
                Assert.Equal(expectedMessage, result.Message);
            }
        }

        public static IEnumerable<object[]> FlexiAlertBlockParser_ThrowsFlexiBlocksExceptionsWithBlockContext_Data()
        {
            return new object[][]
            {
                new object[]
                {
                    @"@{
    ""classFormat"": ""dummy-{0}-{1}"",
}
! This is an alert block.",
                    string.Format(Strings.FlexiBlocksException_InvalidFlexiBlock, nameof(FlexiOptionsBlock), 1, 0,
                        string.Format(Strings.FlexiBlocksException_OptionIsAnInvalidFormat, "ClassFormat", "dummy-{0}-{1}"))
                }
            };
        }
    }
}
