using BenchmarkDotNet.Attributes;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks;
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
    ""lineNumberLineRanges"": [
        {
            ""startLineNumber"": 1,
            ""endLineNumber"": 8,
            ""firstNumber"": 1
        },
        {
            ""startLineNumber"": 11,
            ""endLineNumber"": -1,
            ""firstNumber"": 32
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

        [GlobalSetup(Target = nameof(FlexiIncludeBlock_ParseAndRender))]
        public void FlexiIncludeBlock_ParseAndRender_Setup()
        {
            var pipelineBuilder = new MarkdownPipelineBuilder();
            pipelineBuilder.
                UseFlexiIncludeBlocks().
                UseFlexiOptionsBlocks();
            _pipeline = pipelineBuilder.Build();

            WritePreview(nameof(FlexiIncludeBlocksExtension), FlexiIncludeBlock_ParseAndRender());
        }

        [Benchmark]
        public string FlexiIncludeBlock_ParseAndRender()
        {
            return Markdown.ToHtml(@"+{
    ""sourceUri"": ""https://raw.githubusercontent.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/6998b1c27821d8393ad39beb54f782515c39d98b/test/FlexiBlocks.Tests/exampleInclude.js"",
    ""clippings"":[{""startLineNumber"": 7, ""endDemarcationLineSubstring"": ""#endregion utility methods"", ""dedentLength"": 1, ""collapseRatio"": 0.5}]
}", _pipeline);
        }

        [GlobalSetup(Target = nameof(FlexiSectionBlock_ParseAndRender))]
        public void FlexiSectionBlock_ParseAndRender_Setup()
        {
            var pipelineBuilder = new MarkdownPipelineBuilder();
            pipelineBuilder.
                UseFlexiSectionBlocks().
                UseFlexiOptionsBlocks();
            _pipeline = pipelineBuilder.Build();

            WritePreview(nameof(FlexiSectionBlocksExtension), FlexiSectionBlock_ParseAndRender());
        }

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
                UseFlexiOptionsBlocks();
            _pipeline = pipelineBuilder.Build();

            WritePreview(nameof(FlexiTableBlocksExtension), FlexiTableBlock_ParseAndRender());
        }

        [Benchmark]
        public string FlexiTableBlock_ParseAndRender()
        {
            return Markdown.ToHtml(@"@{
    ""wrapperElement"": ""div""
}
+---+---+
| a | b |
+===+===+
| 0 | 1 |
+---+---+
| 2 | 3 |", _pipeline);
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
