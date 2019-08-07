using BenchmarkDotNet.Attributes;
using Jering.Markdig.Extensions.FlexiBlocks.IncludeBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks;
using Markdig;
using System;
using System.IO;

namespace Jering.Markdig.Extensions.FlexiBlocks.Performance
{
    // TODO each block should have its own benchmarks class, every feature of each block should have its own benchmark.
    // Current benchmarks aren't very useful.
    [MemoryDiagnoser]
    public class Benchmarks
    {
        private MarkdownPipeline _pipeline;

        [GlobalSetup(Target = nameof(IncludeBlock_ParseAndRender))]
        public void IncludeBlock_ParseAndRender_Setup()
        {
            var pipelineBuilder = new MarkdownPipelineBuilder();
            pipelineBuilder.
                UseIncludeBlocks().
                UseOptionsBlocks();
            _pipeline = pipelineBuilder.Build();

            WritePreview(nameof(IncludeBlocksExtension), IncludeBlock_ParseAndRender());
        }

        [Benchmark]
        public string IncludeBlock_ParseAndRender()
        {
            return Markdown.ToHtml(@"+{
    ""source"": ""https://raw.githubusercontent.com/JeringTech/Markdig.Extensions.FlexiBlocks/6998b1c27821d8393ad39beb54f782515c39d98b/test/FlexiBlocks.Tests/exampleInclude.js"",
    ""clippings"":[{""start"": 7, ""endString"": ""#endregion utility methods"", ""dedent"": 1, ""collapse"": 0.5}]
}", _pipeline);
        }

        [GlobalSetup(Target = nameof(FlexiAlertBlock_ParseAndRender))]
        public void FlexiAlertBlock_ParseAndRender_Setup()
        {
            var pipelineBuilder = new MarkdownPipelineBuilder();
            pipelineBuilder.
                UseFlexiAlertBlocks().
                UseOptionsBlocks();
            _pipeline = pipelineBuilder.Build();

            WritePreview(nameof(FlexiAlertBlocksExtension), FlexiAlertBlock_ParseAndRender());
        }

        [Benchmark]
        public string FlexiAlertBlock_ParseAndRender()
        {
            return Markdown.ToHtml(@"@{ ""type"": ""warning"" }
! This is a warning.", _pipeline);
        }

        [GlobalSetup(Target = nameof(FlexiCodeBlock_ParseAndRender))]
        public void FlexiCodeBlock_ParseAndRender_Setup()
        {
            var pipelineBuilder = new MarkdownPipelineBuilder();
            pipelineBuilder.
                UseFlexiCodeBlocks().
                UseOptionsBlocks();
            _pipeline = pipelineBuilder.Build();

            WritePreview(nameof(FlexiCodeBlocksExtension), FlexiCodeBlock_ParseAndRender());
        }

        // TODO slow
        [Benchmark]
        public string FlexiCodeBlock_ParseAndRender()
        {
            return Markdown.ToHtml(@"@{
    ""language"": ""csharp"",
    ""lineNumbers"": [
        {
            ""start"": 1,
            ""end"": 8,
            ""startNumber"": 1
        },
        {
            ""start"": 11,
            ""end"": -1,
            ""startNumber"": 32
        }
    ],
    ""highlightedLines"": [
        {
            ""start"": 3,
            ""end"": 7
        },
        {
            ""start"": 12,
            ""end"": 16
        }
    ],
    ""highlightedPhrases"": [
        { ""regex"": ""public .*?\\)"", ""included"": [1] }
    ]
}
```
public class ExampleClass
{
    public string ExampleFunction1(string arg)
    {
        // Example comment
        return arg + ""dummyString"";
    }

    // Some functions omitted for brevity
    // ...

    public string ExampleFunction3(string arg)
    {
        // Example comment
        return arg + ""dummyString"";
    }
}
```", _pipeline);
        }

        [GlobalSetup(Target = nameof(FlexiSectionBlock_ParseAndRender))]
        public void FlexiSectionBlock_ParseAndRender_Setup()
        {
            var pipelineBuilder = new MarkdownPipelineBuilder();
            pipelineBuilder.
                UseFlexiSectionBlocks().
                UseOptionsBlocks();
            _pipeline = pipelineBuilder.Build();

            WritePreview(nameof(FlexiSectionBlocksExtension), FlexiSectionBlock_ParseAndRender());
        }

        // TODO allocates way more than expected.
        // Try pooling using ConcurrentBags.
        [Benchmark]
        public string FlexiSectionBlock_ParseAndRender()
        {
            return Markdown.ToHtml(@"@{
    ""element"": ""article""
}
# foo

> # foo
> ## foo

## foo", _pipeline);
        }

        [GlobalSetup(Target = nameof(FlexiTableBlock_ParseAndRender))]
        public void FlexiTableBlock_ParseAndRender_Setup()
        {
            var pipelineBuilder = new MarkdownPipelineBuilder();
            pipelineBuilder.
                UseFlexiTableBlocks().
                UseOptionsBlocks();
            _pipeline = pipelineBuilder.Build();

            WritePreview(nameof(FlexiTableBlocksExtension), FlexiTableBlock_ParseAndRender());
        }

        [Benchmark]
        public string FlexiTableBlock_ParseAndRender()
        {
            return Markdown.ToHtml(@"
+-----+-----+
| a   | b   |
+=====+=====+
| `0` | *1* |
+-----+-----+
| > 2 | ``` |
|     | 3   |
|     | ``` |", _pipeline);
        }

        private void WritePreview(string extensionName, string preview)
        {
            Console.WriteLine($"// {extensionName} Benchmark Preview:");
            Console.WriteLine("//");

            using (var stringReader = new StringReader(preview))
            {
                string line;
                while ((line = stringReader.ReadLine()) != null)
                {
                    Console.WriteLine($"// {line}");
                }
            }

            Console.WriteLine();
        }
    }
}
