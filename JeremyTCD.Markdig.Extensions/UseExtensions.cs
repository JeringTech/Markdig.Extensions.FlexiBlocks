using Markdig;

namespace JeremyTCD.Markdig.Extensions
{
    public static class UseExtensions
    {
        public static MarkdownPipelineBuilder UseSections(this MarkdownPipelineBuilder pipelineBuilder, SectionOptions options = null)
        {
            if (!pipelineBuilder.Extensions.Contains<SectionExtension>())
            {
                pipelineBuilder.Extensions.Add(new SectionExtension(options));
            }

            return pipelineBuilder;
        }
    }
}
