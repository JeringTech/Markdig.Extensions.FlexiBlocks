using Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Markdig;
using Markdig.Parsers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiTableBlocks
{
    public class FlexiTableBlocksIntegrationTests
    {
        [Fact]
        public void FlexiTableBlocks_ExceptionsForBlocksInCellsHaveCorrectLineNumbers()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddFlexiTableBlocks();
            services.AddFlexiSectionBlocks();
            services.AddFlexiOptionsBlocks();
            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                var dummyMarkdownPipelineBuilder = new MarkdownPipelineBuilder();
                dummyMarkdownPipelineBuilder.Extensions.Add(serviceProvider.GetRequiredService<IBlockExtension<FlexiTableBlock>>());
                dummyMarkdownPipelineBuilder.Extensions.Add(serviceProvider.GetRequiredService<IBlockExtension<FlexiSectionBlock>>());
                dummyMarkdownPipelineBuilder.Extensions.Add(serviceProvider.GetRequiredService<IBlockExtension<FlexiOptionsBlock>>());
                MarkdownPipeline dummyMarkdownPipeline = dummyMarkdownPipelineBuilder.Build();
                // Note: "" counts as 1 character
                const string dummyMarkdown = @"
+--------------+---------------------+--------------+
| header 1     | header 2            | header 3     |
+==============+=====================+==============+
| - cell 1     | ```                 | > cell 3     |
| - cell 2     | cell 2              | > cell 3     |
| - cell 3     | ```                 | > cell 3     |
|              |                     |              |
|              | o{""element"": 100}   |              |
|              | # Section           |              |
+--------------+---------------------+--------------+";

                // Act and assert
                BlockException result = Assert.Throws<BlockException>(() => MarkdownParser.Parse(dummyMarkdown, dummyMarkdownPipeline));
                Assert.Equal(10, result.LineNumber);
            }
        }
    }
}
