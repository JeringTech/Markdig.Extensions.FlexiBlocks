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
            Assert.Equal(expectedResult.Class, result.Class);
            Assert.Equal(expectedResult.CopyIconMarkup, result.CopyIconMarkup);
            Assert.Equal(expectedResult.Title, result.Title);
            Assert.Equal(expectedResult.Language, result.Language);
            Assert.Equal(expectedResult.CodeClassFormat, result.CodeClassFormat);
            Assert.Equal(expectedResult.CodeClass, result.CodeClass);
            Assert.Equal(expectedResult.SyntaxHighlighter, result.SyntaxHighlighter);
            Assert.Equal(expectedResult.HighlightJSClassPrefix, result.HighlightJSClassPrefix);
            Assert.Equal(expectedResult.LineNumberLineRanges, result.LineNumberLineRanges);
            Assert.Equal(expectedResult.HighlightLineRanges, result.HighlightLineRanges);
            Assert.Equal(expectedResult.LineEmbellishmentClassesPrefix, result.LineEmbellishmentClassesPrefix);
            Assert.Equal(expectedResult.HiddenLinesIconMarkup, result.HiddenLinesIconMarkup);
        }

        public static IEnumerable<object[]> FlexiCodeBlockOptions_CanBePopulated_Data()
        {
            const string dummyClass = "dummyClass";
            const string dummyCopyIconMarkup = "dummyCopyIconMarkup";
            const string dummyTitle = "dummyTitle";
            const string dummyLanguage = "dummyLanguage";
            const string dummyCodeClassFormat = "dummy-{0}";
            const SyntaxHighlighter dummySyntaxHighlighter = SyntaxHighlighter.HighlightJS;
            const string dummyHighlightJSClassPrefix = "dummyHighlightJSClassPrefix";
            const string dummyHiddenLinesIconMarkup = "dummyHiddenLinesIconMarkup";

            var dummyLineNumberLineRange1 = new NumberedLineRange(1, -1, 1);
            var dummyLineNumberLineRanges1 = new List<NumberedLineRange> { dummyLineNumberLineRange1 };
            var dummyLineNumberLineRange2 = new NumberedLineRange(10, 15, 8);
            var dummyLineNumberLineRanges2 = new List<NumberedLineRange> { dummyLineNumberLineRange2 };

            var dummyHighlightLineRange1 = new LineRange(1, -1);
            var dummyHighlightLineRanges1 = new List<LineRange> { dummyHighlightLineRange1 };
            var dummyHighlightLineRange2 = new LineRange(10, 15);
            var dummyHighlightLineRanges2 = new List<LineRange> { dummyHighlightLineRange2 };

            const string dummyLineEmbellishementClassesPrefix = "dummyLineEmbellishementClassesPrefix";

            const string dummyAttribute1 = "dummyAttribute1";
            const string dummyAttributeValue1 = "dummyAttributeValue1";
            var dummyAttributes1 = new Dictionary<string, string> { { dummyAttribute1, dummyAttributeValue1 } };
            const string dummyAttribute2 = "dummyAttribute2";
            const string dummyAttributeValue2 = "dummyAttributeValue2";
            var dummyAttributes2 = new Dictionary<string, string> { { dummyAttribute2, dummyAttributeValue2 } };

            return new object[][]
            {
                // Populating FlexiCodeBlockOptions containing default values
                new object[]
                {
                    new SerializableWrapper<FlexiCodeBlockOptions>(new FlexiCodeBlockOptions()),
                    new SerializableWrapper<FlexiCodeBlockOptions>(new FlexiCodeBlockOptions(
                        dummyClass,
                        dummyCopyIconMarkup,
                        dummyTitle,
                        dummyLanguage,
                        dummyCodeClassFormat,
                        dummySyntaxHighlighter,
                        dummyHighlightJSClassPrefix,
                        dummyLineNumberLineRanges1,
                        dummyHighlightLineRanges1,
                        dummyLineEmbellishementClassesPrefix,
                        dummyHiddenLinesIconMarkup,
                        dummyAttributes1)),
                    $@"{{
    ""{nameof(FlexiCodeBlockOptions.Class)}"": ""{dummyClass}"",
    ""{nameof(FlexiCodeBlockOptions.CopyIconMarkup)}"": ""{dummyCopyIconMarkup}"",
    ""{nameof(FlexiCodeBlockOptions.Title)}"": ""{dummyTitle}"",
    ""{nameof(FlexiCodeBlockOptions.Language)}"": ""{dummyLanguage}"",
    ""{nameof(FlexiCodeBlockOptions.CodeClassFormat)}"": ""{dummyCodeClassFormat}"",
    ""{nameof(FlexiCodeBlockOptions.SyntaxHighlighter)}"": ""{dummySyntaxHighlighter}"",
    ""{nameof(FlexiCodeBlockOptions.HighlightJSClassPrefix)}"": ""{dummyHighlightJSClassPrefix}"",
    ""{nameof(FlexiCodeBlockOptions.HiddenLinesIconMarkup)}"": ""{dummyHiddenLinesIconMarkup}"",
    ""{nameof(FlexiCodeBlockOptions.LineNumberLineRanges)}"": [
        {{ 
            ""{nameof(LineRange.StartLineNumber)}"": {dummyLineNumberLineRange1.LineRange.StartLineNumber},
            ""{nameof(LineRange.EndLineNumber)}"": {dummyLineNumberLineRange1.LineRange.EndLineNumber},
            ""{nameof(NumberedLineRange.FirstNumber)}"": {dummyLineNumberLineRange1.FirstNumber}
        }}
    ],
    ""{nameof(FlexiCodeBlockOptions.HighlightLineRanges)}"": [
        {{ 
            ""{nameof(LineRange.StartLineNumber)}"": {dummyHighlightLineRange1.StartLineNumber},
            ""{nameof(LineRange.EndLineNumber)}"": {dummyHighlightLineRange1.EndLineNumber}
        }}
    ],
    ""{nameof(FlexiCodeBlockOptions.LineEmbellishmentClassesPrefix)}"": ""{dummyLineEmbellishementClassesPrefix}"",
    ""{nameof(FlexiCodeBlockOptions.Attributes)}"": {{
        ""{dummyAttribute1}"": ""{dummyAttributeValue1}""
    }}
}}"
                },

                // Populating FlexiCodeBlockOptions with an existing attributes collection (should be replaced instead of appended to)
                new object[]
                {
                    new SerializableWrapper<FlexiCodeBlockOptions>(new FlexiCodeBlockOptions(lineNumberLineRanges: dummyLineNumberLineRanges1,
                        highlightLineRanges: dummyHighlightLineRanges1, attributes: dummyAttributes1)),
                    new SerializableWrapper<FlexiCodeBlockOptions>(new FlexiCodeBlockOptions(lineNumberLineRanges: dummyLineNumberLineRanges2,
                        highlightLineRanges: dummyHighlightLineRanges2, attributes: dummyAttributes2)),
                    $@"{{
    ""{nameof(FlexiCodeBlockOptions.LineNumberLineRanges)}"": [
        {{ 
            ""{nameof(LineRange.StartLineNumber)}"": {dummyLineNumberLineRange2.LineRange.StartLineNumber},
            ""{nameof(LineRange.EndLineNumber)}"": {dummyLineNumberLineRange2.LineRange.EndLineNumber},
            ""{nameof(NumberedLineRange.FirstNumber)}"": {dummyLineNumberLineRange2.FirstNumber}
        }}
    ],
    ""{nameof(FlexiCodeBlockOptions.HighlightLineRanges)}"": [
        {{ 
            ""{nameof(LineRange.StartLineNumber)}"": {dummyHighlightLineRange2.StartLineNumber},
            ""{nameof(LineRange.EndLineNumber)}"": {dummyHighlightLineRange2.EndLineNumber}
        }}
    ],
    ""{nameof(FlexiCodeBlockOptions.Attributes)}"": {{
        ""{dummyAttribute2}"": ""{dummyAttributeValue2}""
    }}
}}"
                },

                // Defaults for LineRanges in HighlightLineRanges work
                new object[]
                {
                    new SerializableWrapper<FlexiCodeBlockOptions>(new FlexiCodeBlockOptions()),
                    new SerializableWrapper<FlexiCodeBlockOptions>(new FlexiCodeBlockOptions(highlightLineRanges: dummyHighlightLineRanges1)),
                    $@"{{
    ""{nameof(FlexiCodeBlockOptions.HighlightLineRanges)}"": [{{ }}]
}}"
                },

                // Defaults for NumberedLineRanges in NumberedLineRanges work
                new object[]
                {
                    new SerializableWrapper<FlexiCodeBlockOptions>(new FlexiCodeBlockOptions()),
                    new SerializableWrapper<FlexiCodeBlockOptions>(new FlexiCodeBlockOptions(lineNumberLineRanges: dummyLineNumberLineRanges1)),
                    $@"{{
    ""{nameof(FlexiCodeBlockOptions.LineNumberLineRanges)}"": [{{ }}]
}}"
                },
            };
        }

        [Fact]
        public void ValidateAndPopulate_ThrowsFlexiBlocksExceptionIfCodeLanguageClassFormatIsAnInvalidFormat()
        {
            // Arrange
            const string dummyCodeLanguageClassFormat = "dummy-{0}-{1}";

            // Act and assert
            FlexiBlocksException result = Assert.
                Throws<FlexiBlocksException>(() => new FlexiCodeBlockOptions(codeClassFormat: dummyCodeLanguageClassFormat, language: "dummyLanguage"));
            Assert.Equal(string.Format(Strings.FlexiBlocksException_OptionIsAnInvalidFormat,
                    nameof(FlexiCodeBlockOptions.CodeClassFormat),
                    dummyCodeLanguageClassFormat),
                result.Message);
            Assert.IsType<FormatException>(result.InnerException);
        }

        [Fact]
        public void ValidateAndPopulate_ThrowsFlexiBlocksExceptionIfSyntaxHighlighterIsNotWithinTheRangeOfValidValuesForTheEnumSyntaxHighlighter()
        {
            // Arrange
            const SyntaxHighlighter dummySyntaxHighlighter = (SyntaxHighlighter)100; // Arbitrary int that is unlikely to ever be used in the enum

            // Act and assert
            FlexiBlocksException result = Assert.
                Throws<FlexiBlocksException>(() => new FlexiCodeBlockOptions(syntaxHighlighter: dummySyntaxHighlighter));
            Assert.Equal(string.Format(Strings.FlexiBlocksException_OptionMustBeAValidEnumValue,
                    dummySyntaxHighlighter,
                    nameof(FlexiCodeBlockOptions.SyntaxHighlighter),
                    nameof(SyntaxHighlighter)),
                result.Message);
        }

        [Theory]
        [MemberData(nameof(ValidateAndPopulate_ThrowsFlexiBlocksExceptionIfHighlightLineRangesAreInvalid_Data))]
        public void ValidateAndPopulate_ThrowsFlexiBlocksExceptionIfHighlightLineRangesAreInvalid(SerializableWrapper<List<LineRange>> dummyHighlightLineRangesWrapper,
            string expectedExceptionMessage)
        {
            // Act and assert
            FlexiBlocksException result = Assert.
                Throws<FlexiBlocksException>(() => new FlexiCodeBlockOptions(highlightLineRanges: dummyHighlightLineRangesWrapper.Value));
            Assert.Equal(expectedExceptionMessage, result.Message);
        }

        public static IEnumerable<object[]> ValidateAndPopulate_ThrowsFlexiBlocksExceptionIfHighlightLineRangesAreInvalid_Data()
        {
            return new object[][]
            {
                // Overlapping line ranges
                new object[]{
                    new SerializableWrapper<List<LineRange>>(new List<LineRange>{
                        new LineRange(2, -1),
                        new LineRange(5, -1)
                    }),
                    string.Format(Strings.FlexiBlocksException_OptionLineRangesCannotOverlap, nameof(FlexiCodeBlockOptions.HighlightLineRanges), "[2, -1]", "[5, -1]")
                },
                // Overlapping line ranges
                new object[]{
                    new SerializableWrapper<List<LineRange>>(new List<LineRange>{
                        new LineRange(2, 5),
                        new LineRange(5, 11)
                    }),
                    string.Format(Strings.FlexiBlocksException_OptionLineRangesCannotOverlap, nameof(FlexiCodeBlockOptions.HighlightLineRanges),  "[2, 5]", "[5, 11]")
                },
                // Line ranges not in sequential order
                new object[]{
                    new SerializableWrapper<List<LineRange>>(new List<LineRange>{
                        new LineRange(6, 13),
                        new LineRange(2, 4)
                    }),
                    string.Format(Strings.FlexiBlocksException_OptionLineRangesMustBeSequential, nameof(FlexiCodeBlockOptions.HighlightLineRanges), "[6, 13]", "[2, 4]")
                }
            };
        }

        [Theory]
        [MemberData(nameof(ValidateAndPopulate_ThrowsFlexiBlocksExceptionIfLineNumberLineRangesAreInvalid_Data))]
        public void ValidateAndPopulate_ThrowsFlexiBlocksExceptionIfLineNumberLineRangesAreInvalid(SerializableWrapper<List<NumberedLineRange>> dummyLineNumberLineRangesWrapper,
            string expectedExceptionMessage)
        {
            // Act and assert
            FlexiBlocksException result = Assert.
                Throws<FlexiBlocksException>(() => new FlexiCodeBlockOptions(lineNumberLineRanges: dummyLineNumberLineRangesWrapper.Value));
            Assert.Equal(expectedExceptionMessage, result.Message);
        }

        public static IEnumerable<object[]> ValidateAndPopulate_ThrowsFlexiBlocksExceptionIfLineNumberLineRangesAreInvalid_Data()
        {
            return new object[][]
            {
                // Overlapping line ranges
                new object[]{
                    new SerializableWrapper<List<NumberedLineRange>>(new List<NumberedLineRange>{
                        new NumberedLineRange(2), // End line number is -1 by default
                        new NumberedLineRange(5)
                    }),
                    string.Format(Strings.FlexiBlocksException_OptionLineRangesCannotOverlap, nameof(FlexiCodeBlockOptions.LineNumberLineRanges), "[2, -1]", "[5, -1]")
                },
                // Overlapping line ranges
                new object[]{
                    new SerializableWrapper<List<NumberedLineRange>>(new List<NumberedLineRange>{
                        new NumberedLineRange(2, 5),
                        new NumberedLineRange(5, 11)
                    }),
                    string.Format(Strings.FlexiBlocksException_OptionLineRangesCannotOverlap, nameof(FlexiCodeBlockOptions.LineNumberLineRanges), "[2, 5]", "[5, 11]")
                },
                // Line ranges not in sequential order
                new object[]{
                    new SerializableWrapper<List<NumberedLineRange>>(new List<NumberedLineRange>{
                        new NumberedLineRange(6, 13),
                        new NumberedLineRange(2, 4)
                    }),
                    string.Format(Strings.FlexiBlocksException_OptionLineRangesMustBeSequential, nameof(FlexiCodeBlockOptions.LineNumberLineRanges), "[6, 13]", "[2, 4]")
                }
            };
        }

        [Fact]
        public void ValidateAndPopulate_PopulatesCodeLanguageClassIfLanguageAndCodeLanguageClassFormatAreDefined()
        {
            // Arrange
            const string dummyLanguage = "dummy-language";
            const string dummyCodeLanguageClassFormat = "dummy-{0}";

            // Act
            var result = new FlexiCodeBlockOptions(language: dummyLanguage, codeClassFormat: dummyCodeLanguageClassFormat);

            // Assert
            Assert.Equal(string.Format(dummyCodeLanguageClassFormat, dummyLanguage), result.CodeClass);
        }

        [Fact]
        public void ValidateAndPopulate_SetsCodeLanguageClassToNullIfLanguageOrCodeLanguageClassFormatIsUndefined()
        {
            // Arrange
            var testSubject = new FlexiCodeBlockOptions(language: "dummyLanguage");
            string initialClass = testSubject.CodeClass;

            // Act
            JsonConvert.PopulateObject("{\"codeClassFormat\": null}", testSubject);

            // Assert
            Assert.NotNull(initialClass);
            Assert.Null(testSubject.CodeClass);
        }
    }
}
