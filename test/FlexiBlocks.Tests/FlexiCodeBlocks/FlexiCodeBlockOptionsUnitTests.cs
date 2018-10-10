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
            Assert.Equal(expectedResult.LineNumberRanges, result.LineNumberRanges);
            Assert.Equal(expectedResult.HighlightLineRanges, result.HighlightLineRanges);
            Assert.Equal(expectedResult.LineEmbellishmentClassesPrefix, result.LineEmbellishmentClassesPrefix);
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

            var dummyLineNumberRange1 = new LineNumberRange(1, -1, 1);
            var dummyLineNumberRanges1 = new List<LineNumberRange> { dummyLineNumberRange1 };
            var dummyLineNumberRange2 = new LineNumberRange(10, 15, 8);
            var dummyLineNumberRanges2 = new List<LineNumberRange> { dummyLineNumberRange2 };

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
                        dummyLineNumberRanges1,
                        dummyHighlightLineRanges1,
                        dummyLineEmbellishementClassesPrefix,
                        dummyAttributes1)),
                    $@"{{
    ""{nameof(FlexiCodeBlockOptions.Class)}"": ""{dummyClass}"",
    ""{nameof(FlexiCodeBlockOptions.CopyIconMarkup)}"": ""{dummyCopyIconMarkup}"",
    ""{nameof(FlexiCodeBlockOptions.Title)}"": ""{dummyTitle}"",
    ""{nameof(FlexiCodeBlockOptions.Language)}"": ""{dummyLanguage}"",
    ""{nameof(FlexiCodeBlockOptions.CodeClassFormat)}"": ""{dummyCodeClassFormat}"",
    ""{nameof(FlexiCodeBlockOptions.SyntaxHighlighter)}"": ""{dummySyntaxHighlighter}"",
    ""{nameof(FlexiCodeBlockOptions.HighlightJSClassPrefix)}"": ""{dummyHighlightJSClassPrefix}"",
    ""{nameof(FlexiCodeBlockOptions.LineNumberRanges)}"": [
        {{ 
            ""{nameof(LineRange.StartLineNumber)}"": {dummyLineNumberRange1.LineRange.StartLineNumber},
            ""{nameof(LineRange.EndLineNumber)}"": {dummyLineNumberRange1.LineRange.EndLineNumber},
            ""{nameof(LineNumberRange.FirstLineNumber)}"": {dummyLineNumberRange1.FirstLineNumber}
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
                    new SerializableWrapper<FlexiCodeBlockOptions>(new FlexiCodeBlockOptions(lineNumberRanges: dummyLineNumberRanges1,
                        highlightLineRanges: dummyHighlightLineRanges1, attributes: dummyAttributes1)),
                    new SerializableWrapper<FlexiCodeBlockOptions>(new FlexiCodeBlockOptions(lineNumberRanges: dummyLineNumberRanges2,
                        highlightLineRanges: dummyHighlightLineRanges2, attributes: dummyAttributes2)),
                    $@"{{
    ""{nameof(FlexiCodeBlockOptions.LineNumberRanges)}"": [
        {{ 
            ""{nameof(LineRange.StartLineNumber)}"": {dummyLineNumberRange2.LineRange.StartLineNumber},
            ""{nameof(LineRange.EndLineNumber)}"": {dummyLineNumberRange2.LineRange.EndLineNumber},
            ""{nameof(LineNumberRange.FirstLineNumber)}"": {dummyLineNumberRange2.FirstLineNumber}
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

                // Defaults for LineNumberRanges in LineNumberRanges work
                new object[]
                {
                    new SerializableWrapper<FlexiCodeBlockOptions>(new FlexiCodeBlockOptions()),
                    new SerializableWrapper<FlexiCodeBlockOptions>(new FlexiCodeBlockOptions(lineNumberRanges: dummyLineNumberRanges1)),
                    $@"{{
    ""{nameof(FlexiCodeBlockOptions.LineNumberRanges)}"": [{{ }}]
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
        public void ValidateAndPopulate_ThrowsFlexiBlocksExceptionIfLineNumberLineRangesAreInvalid(SerializableWrapper<List<LineNumberRange>> dummyLineNumberLineRangesWrapper,
            string expectedExceptionMessage)
        {
            // Act and assert
            FlexiBlocksException result = Assert.
                Throws<FlexiBlocksException>(() => new FlexiCodeBlockOptions(lineNumberRanges: dummyLineNumberLineRangesWrapper.Value));
            Assert.Equal(expectedExceptionMessage, result.Message);
        }

        public static IEnumerable<object[]> ValidateAndPopulate_ThrowsFlexiBlocksExceptionIfLineNumberLineRangesAreInvalid_Data()
        {
            return new object[][]
            {
                // Overlapping line ranges
                new object[]{
                    new SerializableWrapper<List<LineNumberRange>>(new List<LineNumberRange>{
                        new LineNumberRange(2), // End line number is -1 by default
                        new LineNumberRange(5)
                    }),
                    string.Format(Strings.FlexiBlocksException_OptionLineRangesCannotOverlap, nameof(FlexiCodeBlockOptions.LineNumberRanges), "[2, -1]", "[5, -1]")
                },
                // Overlapping line ranges
                new object[]{
                    new SerializableWrapper<List<LineNumberRange>>(new List<LineNumberRange>{
                        new LineNumberRange(2, 5),
                        new LineNumberRange(5, 11)
                    }),
                    string.Format(Strings.FlexiBlocksException_OptionLineRangesCannotOverlap, nameof(FlexiCodeBlockOptions.LineNumberRanges), "[2, 5]", "[5, 11]")
                },
                // Line ranges not in sequential order
                new object[]{
                    new SerializableWrapper<List<LineNumberRange>>(new List<LineNumberRange>{
                        new LineNumberRange(6, 13),
                        new LineNumberRange(2, 4)
                    }),
                    string.Format(Strings.FlexiBlocksException_OptionLineRangesMustBeSequential, nameof(FlexiCodeBlockOptions.LineNumberRanges), "[6, 13]", "[2, 4]")
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
