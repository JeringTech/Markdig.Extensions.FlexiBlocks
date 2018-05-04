using Markdig;

namespace JeremyTCD.Markdig.Extensions
{
    public static class UseExtensions
    {
        public static MarkdownPipelineBuilder UseSections(this MarkdownPipelineBuilder pipelineBuilder)
        {
            pipelineBuilder.Extensions.AddIfNotAlready<SectionExtension>();
            return pipelineBuilder;
        }
    }
}
