using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiCodeBlocks
{
    public class FlexiCodeBlockOptionsUnitTests
    {
        [Theory]
        [MemberData(nameof(FlexiCodeBlockOptions_CanBePopulated_Data))]
        public void FlexiCodeBlockOptions_CanBePopulated(SerializableWrapper<FlexiCodeBlockOptions> dummyInitialOptionsWrapper,
            SerializableWrapper<FlexiCodeBlockOptions> dummyExpectedOptionsWrapper,
            string dummyJson)
        {
            // Act
            JsonConvert.PopulateObject(dummyJson, dummyInitialOptionsWrapper.Value);

            // Assert
            FlexiCodeBlockOptions result = dummyInitialOptionsWrapper.Value;
            FlexiCodeBlockOptions expectedResult = dummyExpectedOptionsWrapper.Value;
            Assert.Equal(expectedResult.BlockName, result.BlockName);
            Assert.Equal(expectedResult.Title, result.Title);
            Assert.Equal(expectedResult.CopyIcon, result.CopyIcon);
            Assert.Equal(expectedResult.RenderHeader, result.RenderHeader);
            Assert.Equal(expectedResult.Language, result.Language);
            Assert.Equal(expectedResult.SyntaxHighlighter, result.SyntaxHighlighter);
            Assert.Equal(expectedResult.LineNumbers, result.LineNumbers);
            Assert.Equal(expectedResult.OmittedLinesIcon, result.OmittedLinesIcon);
            Assert.Equal(expectedResult.HighlightedLines, result.HighlightedLines);
            Assert.Equal(expectedResult.HighlightedPhrases, result.HighlightedPhrases);
            Assert.Equal(expectedResult.RenderingMode, result.RenderingMode);
            Assert.Equal(expectedResult.Attributes, result.Attributes);
        }

        public static IEnumerable<object[]> FlexiCodeBlockOptions_CanBePopulated_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string dummyTitle = "dummyTitle";
            const string dummyCopyIcon = "dummyCopyIcon";
            const bool dummyRenderHeader = false;
            const string dummyLanguage = "dummyLanguage";
            const SyntaxHighlighter dummySyntaxHighlighter = SyntaxHighlighter.HighlightJS;
            var dummyNumberedLineRange1 = new NumberedLineRange(1, startNumber: 1);
            var dummyLineNumbers1 = new List<NumberedLineRange> { dummyNumberedLineRange1 };
            var dummyNumberedLineRange2 = new NumberedLineRange(10, 15, 8);
            var dummyLineNumbers2 = new List<NumberedLineRange> { dummyNumberedLineRange2 };
            const string dummyOmittedLinesIcon = "dummyOmittedLinesIcon";
            var dummyLineRange1 = new LineRange(1);
            var dummyHighlightedLines1 = new List<LineRange> { dummyLineRange1 };
            var dummyLineRange2 = new LineRange(10, 15);
            var dummyHighlightedLines2 = new List<LineRange> { dummyLineRange2 };
            const string dummyAttribute1 = "dummyAttribute1";
            const string dummyAttributeValue1 = "dummyAttributeValue1";
            var dummyAttributes1 = new Dictionary<string, string> { { dummyAttribute1, dummyAttributeValue1 } };
            const string dummyAttribute2 = "dummyAttribute2";
            const string dummyAttributeValue2 = "dummyAttributeValue2";
            var dummyAttributes2 = new Dictionary<string, string> { { dummyAttribute2, dummyAttributeValue2 } };
            var dummyPhraseGroup1 = new PhraseGroup("dummyRegex1", new int[] { 1, 2, 3 });
            var dummyHighlightedPhrases1 = new List<PhraseGroup>() { dummyPhraseGroup1 };
            var dummyPhraseGroup2 = new PhraseGroup("dummyRegex2", new int[] { -3, -2, -1 });
            var dummyHighlightedPhrases2 = new List<PhraseGroup>() { dummyPhraseGroup2 };
            const FlexiCodeBlockRenderingMode dummyRenderingMode = FlexiCodeBlockRenderingMode.Classic;

            return new object[][]
            {
                // Populating FlexiCodeBlockOptions containing default values
                new object[]
                {
                    new SerializableWrapper<FlexiCodeBlockOptions>(new FlexiCodeBlockOptions()),
                    new SerializableWrapper<FlexiCodeBlockOptions>(new FlexiCodeBlockOptions(
                        dummyBlockName,
                        dummyTitle,
                        dummyCopyIcon,
                        dummyRenderHeader,
                        dummyLanguage,
                        dummySyntaxHighlighter,
                        dummyLineNumbers1,
                        dummyOmittedLinesIcon,
                        dummyHighlightedLines1,
                        dummyHighlightedPhrases1,
                        dummyRenderingMode,
                        dummyAttributes1)),
                    $@"{{
    ""{nameof(FlexiCodeBlockOptions.BlockName)}"": ""{dummyBlockName}"",
    ""{nameof(FlexiCodeBlockOptions.Title)}"": ""{dummyTitle}"",
    ""{nameof(FlexiCodeBlockOptions.CopyIcon)}"": ""{dummyCopyIcon}"",
    ""{nameof(FlexiCodeBlockOptions.RenderHeader)}"": ""{dummyRenderHeader}"",
    ""{nameof(FlexiCodeBlockOptions.Language)}"": ""{dummyLanguage}"",
    ""{nameof(FlexiCodeBlockOptions.SyntaxHighlighter)}"": ""{dummySyntaxHighlighter}"",
    ""{nameof(FlexiCodeBlockOptions.LineNumbers)}"": [
        {{ 
            ""{nameof(NumberedLineRange.StartLine)}"": {dummyNumberedLineRange1.StartLine},
            ""{nameof(NumberedLineRange.EndLine)}"": {dummyNumberedLineRange1.EndLine},
            ""{nameof(NumberedLineRange.StartLine)}"": {dummyNumberedLineRange1.StartLine}
        }}
    ],
    ""{nameof(FlexiCodeBlockOptions.OmittedLinesIcon)}"": ""{dummyOmittedLinesIcon}"",
    ""{nameof(FlexiCodeBlockOptions.HighlightedLines)}"": [
        {{ 
            ""{nameof(LineRange.StartLine)}"": {dummyLineRange1.StartLine},
            ""{nameof(LineRange.EndLine)}"": {dummyLineRange1.EndLine}
        }}
    ],
    ""{nameof(FlexiCodeBlockOptions.HighlightedPhrases)}"": [
        {{ 
            ""{nameof(PhraseGroup.Regex)}"": ""{dummyPhraseGroup1.Regex}"",
            ""{nameof(PhraseGroup.IncludedMatches)}"": [{string.Join(",", dummyPhraseGroup1.IncludedMatches)}]
        }}
    ],
    ""{nameof(FlexiCodeBlockOptions.RenderingMode)}"": ""{dummyRenderingMode}"",
    ""{nameof(FlexiCodeBlockOptions.Attributes)}"": {{
        ""{dummyAttribute1}"": ""{dummyAttributeValue1}""
    }}
}}"
                },

                // Populating FlexiCodeBlockOptions with existing collections (they should be replaced instead of appended to)
                new object[]
                {
                    new SerializableWrapper<FlexiCodeBlockOptions>(new FlexiCodeBlockOptions(lineNumbers: dummyLineNumbers1,
                        highlightedLines: dummyHighlightedLines1,
                        highlightedPhrases: dummyHighlightedPhrases1,
                        attributes: dummyAttributes1)),
                    new SerializableWrapper<FlexiCodeBlockOptions>(new FlexiCodeBlockOptions(lineNumbers: dummyLineNumbers2,
                        highlightedLines: dummyHighlightedLines2,
                        highlightedPhrases: dummyHighlightedPhrases2,
                        attributes: dummyAttributes2)),
                    $@"{{
    ""{nameof(FlexiCodeBlockOptions.LineNumbers)}"": [
        {{ 
            ""{nameof(NumberedLineRange.StartLine)}"": {dummyNumberedLineRange2.StartLine},
            ""{nameof(NumberedLineRange.EndLine)}"": {dummyNumberedLineRange2.EndLine},
            ""{nameof(NumberedLineRange.StartNumber)}"": {dummyNumberedLineRange2.StartNumber}
        }}
    ],
    ""{nameof(FlexiCodeBlockOptions.HighlightedLines)}"": [
        {{ 
            ""{nameof(LineRange.StartLine)}"": {dummyLineRange2.StartLine},
            ""{nameof(LineRange.EndLine)}"": {dummyLineRange2.EndLine}
        }}
    ],
    ""{nameof(FlexiCodeBlockOptions.HighlightedPhrases)}"": [
        {{ 
            ""{nameof(PhraseGroup.Regex)}"": ""{dummyPhraseGroup2.Regex}"",
            ""{nameof(PhraseGroup.IncludedMatches)}"": [{string.Join(",", dummyPhraseGroup2.IncludedMatches)}]
        }}
    ],
    ""{nameof(FlexiCodeBlockOptions.Attributes)}"": {{
        ""{dummyAttribute2}"": ""{dummyAttributeValue2}""
    }}
}}"
                },

                // Defaults for LineRanges in HighlightedLines work
                new object[]
                {
                    new SerializableWrapper<FlexiCodeBlockOptions>(new FlexiCodeBlockOptions()),
                    new SerializableWrapper<FlexiCodeBlockOptions>(new FlexiCodeBlockOptions(highlightedLines: dummyHighlightedLines1)),
                    $@"{{
    ""{nameof(FlexiCodeBlockOptions.HighlightedLines)}"": [{{ }}]
}}"
                },

                // Defaults for NumberedLineRanges in NumberedLineRanges work
                new object[]
                {
                    new SerializableWrapper<FlexiCodeBlockOptions>(new FlexiCodeBlockOptions()),
                    new SerializableWrapper<FlexiCodeBlockOptions>(new FlexiCodeBlockOptions(lineNumbers: dummyLineNumbers1)),
                    $@"{{
    ""{nameof(FlexiCodeBlockOptions.LineNumbers)}"": [{{ }}]
}}"
                },
            };
        }

        [Fact]
        public void FlexiCodeBlockOptions_PopulatingThrowsExceptionIfAPhraseGroupsRegexIsNull()
        {
            // Arrange
            string dummyJson = $@"{{
    ""{nameof(FlexiCodeBlockOptions.HighlightedPhrases)}"": [
        {{ 
            ""{nameof(PhraseGroup.Regex)}"": null
        }}
    ]
}}";
            var dummyFlexiCodeBlockOptions = new FlexiCodeBlockOptions();

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => JsonConvert.PopulateObject(dummyJson, dummyFlexiCodeBlockOptions));
        }

        [Fact]
        public void FlexiCodeBlockOptions_ThrowsExceptionIfAPhraseGroupsRegexIsUnspecified()
        {
            // Arrange
            string dummyJson = $@"{{""{nameof(FlexiCodeBlockOptions.HighlightedPhrases)}"": [{{ }}]}}";
            var dummyFlexiCodeBlockOptions = new FlexiCodeBlockOptions();

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => JsonConvert.PopulateObject(dummyJson, dummyFlexiCodeBlockOptions));
        }
    }
}
