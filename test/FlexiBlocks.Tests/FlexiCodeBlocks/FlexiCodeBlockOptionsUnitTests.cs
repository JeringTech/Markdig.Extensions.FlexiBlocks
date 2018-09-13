using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiCodeBlocks
{
    public class FlexiCodeBlockOptionsUnitTests
    {
        [Fact]
        public void ValidateAndPopulate_ThrowsFlexiBlocksExceptionIfCodeLanguageClassFormatIsAnInvalidFormat()
        {
            // Arrange
            const string dummyCodeLanguageClassFormat = "dummy-{0}-{1}";

            // Act and assert
            FlexiBlocksException result = Assert.
                Throws<FlexiBlocksException>(() => new FlexiCodeBlockOptions(codeClassFormat: dummyCodeLanguageClassFormat, language: "dummyLanguage"));
            Assert.Equal(string.Format(Strings.FlexiBlocksException_InvalidFormat,
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
            Assert.Equal(string.Format(Strings.FlexiBlocksException_InvalidEnumValue,
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
                    string.Format(Strings.FlexiBlocksException_LineRangesCannotOverlap, nameof(FlexiCodeBlockOptions.HighlightLineRanges), "[2, -1]", "[5, -1]")
                },
                // Overlapping line ranges
                new object[]{
                    new SerializableWrapper<List<LineRange>>(new List<LineRange>{
                        new LineRange(2, 5),
                        new LineRange(5, 11)
                    }),
                    string.Format(Strings.FlexiBlocksException_LineRangesCannotOverlap, nameof(FlexiCodeBlockOptions.HighlightLineRanges),  "[2, 5]", "[5, 11]")
                },
                // Line ranges not in sequential order
                new object[]{
                    new SerializableWrapper<List<LineRange>>(new List<LineRange>{
                        new LineRange(6, 13),
                        new LineRange(2, 4)
                    }),
                    string.Format(Strings.FlexiBlocksException_LineRangesMustBeSequential, nameof(FlexiCodeBlockOptions.HighlightLineRanges), "[6, 13]", "[2, 4]")
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
                    string.Format(Strings.FlexiBlocksException_LineRangesCannotOverlap, nameof(FlexiCodeBlockOptions.LineNumberRanges), "[2, -1]", "[5, -1]")
                },
                // Overlapping line ranges
                new object[]{
                    new SerializableWrapper<List<LineNumberRange>>(new List<LineNumberRange>{
                        new LineNumberRange(2, 5),
                        new LineNumberRange(5, 11)
                    }),
                    string.Format(Strings.FlexiBlocksException_LineRangesCannotOverlap, nameof(FlexiCodeBlockOptions.LineNumberRanges), "[2, 5]", "[5, 11]")
                },
                // Line ranges not in sequential order
                new object[]{
                    new SerializableWrapper<List<LineNumberRange>>(new List<LineNumberRange>{
                        new LineNumberRange(6, 13),
                        new LineNumberRange(2, 4)
                    }),
                    string.Format(Strings.FlexiBlocksException_LineRangesMustBeSequential, nameof(FlexiCodeBlockOptions.LineNumberRanges), "[6, 13]", "[2, 4]")
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
