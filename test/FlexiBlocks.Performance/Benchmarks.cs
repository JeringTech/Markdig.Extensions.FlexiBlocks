using BenchmarkDotNet.Attributes;
using Markdig;

namespace Jering.Markdig.Extensions.FlexiBlocks.Performance
{
    [MemoryDiagnoser]
    public class Benchmarks
    {
        private MarkdownPipeline _pipeline;

        [GlobalSetup(Target = nameof(FlexiAlertBlock_ParseAndRender))]
        public void FlexiAlertBlock_ParseAndRender_Setup()
        {
            var pipelineBuilder = new MarkdownPipelineBuilder();
            pipelineBuilder.UseFlexiAlertBlocks();
            _pipeline = pipelineBuilder.Build();
        }

        [Benchmark]
        public string FlexiAlertBlock_ParseAndRender()
        {
            return Markdown.ToHtml(@"! critical-warning
! This is a critical warning.", _pipeline);
        }
    }
}
