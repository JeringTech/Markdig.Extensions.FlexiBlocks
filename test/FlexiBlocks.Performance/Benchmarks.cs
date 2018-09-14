using BenchmarkDotNet.Attributes;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using Markdig;
using System;
using System.IO;

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
            pipelineBuilder.
                UseFlexiAlertBlocks().
                UseFlexiOptionsBlocks();
            _pipeline = pipelineBuilder.Build();

            WritePreview(nameof(FlexiAlertBlocksExtension), FlexiAlertBlock_ParseAndRender());
        }

        [Benchmark]
        public string FlexiAlertBlock_ParseAndRender()
        {
            return Markdown.ToHtml(@"@{ ""type"": ""info"" }
! This is a critical warning.", _pipeline);
        }

        [GlobalSetup(Target = nameof(FlexiCodeBlock_ParseAndRender))]
        public void FlexiCodeBlock_ParseAndRender_Setup()
        {
            var pipelineBuilder = new MarkdownPipelineBuilder();
            pipelineBuilder.
                UseFlexiCodeBlocks().
                UseFlexiOptionsBlocks();
            _pipeline = pipelineBuilder.Build();

            WritePreview(nameof(FlexiCodeBlocksExtension), FlexiCodeBlock_ParseAndRender());
        }

        [Benchmark]
        public string FlexiCodeBlock_ParseAndRender()
        {
            return Markdown.ToHtml(@"@{
    ""language"": ""csharp"",
    ""lineNumberRanges"": [
        {
            ""startLineNumber"": 1,
            ""endLineNumber"": 8,
            ""firstLineNumber"": 1
        },
        {
            ""startLineNumber"": 11,
            ""endLineNumber"": -1,
            ""firstLineNumber"": 32
        }
    ],
    ""highlightLineRanges"": [
        {
            ""startLineNumber"": 3,
            ""endLineNumber"": 7
        },
        {
            ""startLineNumber"": 12,
            ""endLineNumber"": 16
        }
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
