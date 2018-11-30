using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Markdig;
using Markdig.Parsers;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiAlertBlocks
{
    // Incomplete, we should test common unrecoverable situations to ensure that exception messages are detailed enough to be actionable
    public class FlexiAlertBlocksIntegrationTests
    {
        [Theory]
        [MemberData(nameof(FlexiAlertBlockParser_ThrowsFlexiBlocksExceptionsWithBlockContext_Data))]
        public void FlexiAlertBlockParser_ThrowsFlexiBlocksExceptionsWithBlockContext(string dummyMarkdown, string expectedExceptionMessage, string expectedInnerExceptionMessage)
        {
            // Arrange
            // Need to dispose of services between tests so that DiskCacheService's in memory cache doesn't affect results
            var dummyMarkdownPipelineBuilder = new MarkdownPipelineBuilder();
            dummyMarkdownPipelineBuilder.
                UseFlexiOptionsBlocks().
                UseFlexiAlertBlocks();
            MarkdownPipeline dummyMarkdownPipeline = dummyMarkdownPipelineBuilder.Build();

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => MarkdownParser.Parse(dummyMarkdown, dummyMarkdownPipeline));
            Assert.Equal(expectedExceptionMessage, result.Message);
            Assert.Equal(expectedInnerExceptionMessage, result.InnerException.Message);
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
                        Strings.FlexiBlocksException_ExceptionOccurredWhileProcessingABlock),
                    string.Format(Strings.FlexiBlocksException_OptionIsAnInvalidFormat, "ClassFormat", "dummy-{0}-{1}")
                }
            };
        }
    }
}
