using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Moq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests
{
    public class HtmlRendererExtensionsUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Theory]
        [MemberData(nameof(WriteHasOptionClass__WritesHasOptionClass_Data))]
        public void WriteHasOptionClass_WritesHasOptionClass(bool dummyHasOption, string dummyBlockName, string dummyOptionName, string expectedResult)
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.WriteHasOptionClass(dummyHasOption, dummyBlockName, dummyOptionName);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> WriteHasOptionClass__WritesHasOptionClass_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string dummyOptionName = "dummyOptionName";

            return new object[][]
            {
                new object[]{true, dummyBlockName, dummyOptionName, $" {dummyBlockName}_has-{dummyOptionName}"},
                new object[]{false, dummyBlockName, dummyOptionName, $" {dummyBlockName}_no-{dummyOptionName}" }
            };
        }

        [Fact]
        public void WriteEndTag_WritesEndTag()
        {
            // Arrange
            const string dummyTagName = "dummyTagName";
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.WriteEndTag(dummyTagName);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal($"</{dummyTagName}>", result);
        }

        [Fact]
        public void WriteEndTagLine_WithoutCondition_WritesEndTagLine()
        {
            // Arrange
            const string dummyTagName = "dummyTagName";
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.WriteEndTagLine(dummyTagName);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal($"</{dummyTagName}>\n", result, ignoreLineEndingDifferences: true);
        }

        [Theory]
        [MemberData(nameof(WriteEndTagLine_WithCondition_WritesEndTagLineConditionally_Data))]
        public void WriteEndTagLine_WithCondition_WritesEndTagLineConditionally(bool dummyCondition, string dummyTagName, string expectedResult)
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.WriteEndTagLine(dummyCondition, dummyTagName);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> WriteEndTagLine_WithCondition_WritesEndTagLineConditionally_Data()
        {
            const string dummyTagName = "dummyTagName";

            return new object[][]
            {
                new object[]{ true, dummyTagName, $"</{dummyTagName}>\n" },
                new object[]{ false, string.Empty, string.Empty }
            };
        }

        [Fact]
        public void WriteStartTag_WritesStartTag()
        {
            // Arrange
            const string dummyBlockName = "dummyBlockName";
            const string dummyElementName = "dummyElementName";
            const string dummyTagName = "dummyTagName";
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.WriteStartTag(dummyTagName, dummyBlockName, dummyElementName);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal($"<{dummyTagName} class=\"{dummyBlockName}__{dummyElementName}\">", result);
        }

        [Fact]
        public void WriteStartTagLine_WithoutAttributes_WritesStartTagLineWithoutAttributes()
        {
            // Arrange
            const string dummyBlockName = "dummyBlockName";
            const string dummyElementName = "dummyElementName";
            const string dummyTagName = "dummyTagName";
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.WriteStartTagLine(dummyTagName, dummyBlockName, dummyElementName);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal($"<{dummyTagName} class=\"{dummyBlockName}__{dummyElementName}\">\n", result, ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void WriteStartTagLine_WithAttributes_WritesStartTagLineWithAttributes()
        {
            // Arrange
            const string dummyBlockName = "dummyBlockName";
            const string dummyElementName = "dummyElementName";
            const string dummyTagName = "dummyTagName";
            const string dummyAttributes = "dummyAttributes";
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.WriteStartTagLine(dummyTagName, dummyBlockName, dummyElementName, dummyAttributes);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal($"<{dummyTagName} class=\"{dummyBlockName}__{dummyElementName}\" {dummyAttributes}>\n", result, ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void WriteElementClass_WritesElementClass()
        {
            // Arrange
            const string dummyBlockName = "dummyBlockName";
            const string dummyElementName = "dummyElementName";
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.WriteElementClass(dummyBlockName, dummyElementName);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal($"{dummyBlockName}__{dummyElementName}", result);
        }

        [Fact]
        public void WriteBlockKeyValueModifierClass_WithCharModifierValue_WritesBlockKeyValueModifierClassWithCharModifierValue()
        {
            // Arrange
            const string dummyBlockName = "dummyBlockName";
            const string dummyModifierKey = "dummyModifierKey";
            const char dummyModifierValue = 'a';
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.WriteBlockKeyValueModifierClass(dummyBlockName, dummyModifierKey, dummyModifierValue);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal($" {dummyBlockName}_{dummyModifierKey}_{dummyModifierValue}", result);
        }

        [Fact]
        public void WriteBlockKeyValueModifierClass_WithStringModifierValue_WritesBlockKeyValueModifierClassWithStringModifierValue()
        {
            // Arrange
            const string dummyBlockName = "dummyBlockName";
            const string dummyModifierKey = "dummyModifierKey";
            const string dummyModifierValue = "dummyModifierValue";
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.WriteBlockKeyValueModifierClass(dummyBlockName, dummyModifierKey, dummyModifierValue);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal($" {dummyBlockName}_{dummyModifierKey}_{dummyModifierValue}", result);
        }

        [Fact]
        public void WriteElementKeyValueModifierClass_WritesElementKeyValueModifierClass()
        {
            // Arrange
            const string dummyElementName = "dummyElementName";
            const string dummyBlockName = "dummyBlockName";
            const string dummyModifierKey = "dummyModifierKey";
            const string dummyModifierValue = "dummyModifierValue";
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.WriteElementKeyValueModifierClass(dummyBlockName, dummyElementName, dummyModifierKey, dummyModifierValue);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal($" {dummyBlockName}__{dummyElementName}_{dummyModifierKey}_{dummyModifierValue}", result);
        }

        [Theory]
        [MemberData(nameof(Write_WithCondition_OnePart_WritesConditionally_Data))]
        public void Write_WithCondition_OnePart_WritesConditionally(bool dummyCondition, string dummyText, string expectedResult)
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.Write(dummyCondition, dummyText);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> Write_WithCondition_OnePart_WritesConditionally_Data()
        {
            const string dummyText = "dummyText";

            return new object[][]
            {
                new object[]{true, dummyText, dummyText},
                new object[]{true, null, string.Empty},
                new object[]{false, dummyText, string.Empty}
            };
        }

        [Theory]
        [MemberData(nameof(Write_WithCondition_TwoParts_WritesConditionally_Data))]
        public void Write_WithCondition_TwoParts_WritesConditionally(bool dummyCondition, char dummyPart1, string dummyPart2, string expectedResult)
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.Write(dummyCondition, dummyPart1, dummyPart2);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> Write_WithCondition_TwoParts_WritesConditionally_Data()
        {
            const char dummyPart1 = 'a';
            const string dummyPart2 = "dummyPart2";

            return new object[][]
            {
                new object[]{true, dummyPart1, dummyPart2, dummyPart1 + dummyPart2},
                new object[]{true, dummyPart1, null, dummyPart1},
                new object[]{false, dummyPart1, dummyPart2, string.Empty}
            };
        }

        [Theory]
        [MemberData(nameof(Write_WithCondition_ThreeParts_WritesConditionally_Data))]
        public void Write_WithCondition_ThreeParts_WritesConditionally(bool dummyCondition,
            string dummyPart1,
            string dummyPart2,
            string dummyPart3,
            string expectedResult)
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.Write(dummyCondition, dummyPart1, dummyPart2, dummyPart3);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> Write_WithCondition_ThreeParts_WritesConditionally_Data()
        {
            const string dummyPart1 = "dummyPart1";
            const string dummyPart2 = "dummyPart2";
            const string dummyPart3 = "dummyPart3";

            return new object[][]
            {
                new object[]{true, dummyPart1, dummyPart2, dummyPart3, dummyPart1 + dummyPart2 + dummyPart3},
                new object[]{true, dummyPart1, null, null, dummyPart1},
                new object[]{false, dummyPart1, dummyPart2, dummyPart3, string.Empty}
            };
        }

        [Theory]
        [MemberData(nameof(Write_WithCondition_FourParts_WritesConditionally_Data))]
        public void Write_WithCondition_FourParts_WritesConditionally(bool dummyCondition,
            char dummyPart1,
            string dummyPart2,
            string dummyPart3,
            string dummyPart4,
            string expectedResult)
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.Write(dummyCondition, dummyPart1, dummyPart2, dummyPart3, dummyPart4);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> Write_WithCondition_FourParts_WritesConditionally_Data()
        {
            const char dummyPart1 = 'a';
            const string dummyPart2 = "dummyPart2";
            const string dummyPart3 = "dummyPart3";
            const string dummyPart4 = "dummyPart4";

            return new object[][]
            {
                new object[]{true, dummyPart1, dummyPart2, dummyPart3, dummyPart4, dummyPart1 + dummyPart2 + dummyPart3 + dummyPart4},
                new object[]{true, dummyPart1, null, null, null, dummyPart1},
                new object[]{false, dummyPart1, dummyPart2, dummyPart3, dummyPart4, string.Empty}
            };
        }

        [Fact]
        public void WriteAttributes_WritesNothingIfAttributesIsNull()
        {
            // Arrange
            var stringWriter = new StringWriter();
            var htmlRenderer = new HtmlRenderer(stringWriter);

            // Act
            htmlRenderer.WriteAttributes(null);
            string result = stringWriter.ToString();

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void WriteAttributes_WritesAttributes()
        {
            // Arrange
            const string dummyAttribute1 = "dummyAttribute1";
            const string dummyValue1 = "dummyValue1";
            const string dummyAttribute2 = "dummyAttribute2";
            const string dummyValue2 = "dummyValue2";
            var dummyAttributes = new ReadOnlyDictionary<string, string>(
                new Dictionary<string, string>
                {
                    { dummyAttribute1, dummyValue1 },
                    { dummyAttribute2, dummyValue2 }
                });
            var stringWriter = new StringWriter();
            var htmlRenderer = new HtmlRenderer(stringWriter);

            // Act
            htmlRenderer.WriteAttributes(dummyAttributes);
            string result = stringWriter.ToString();

            // Assert
            Assert.Equal($" {dummyAttribute1}=\"{dummyValue1}\" {dummyAttribute2}=\"{dummyValue2}\"", result);
        }

        [Fact]
        public void WriteAttributesExcludingClass_WritesNothingIfAttributesIsNull()
        {
            // Arrange
            var stringWriter = new StringWriter();
            var htmlRenderer = new HtmlRenderer(stringWriter);

            // Act
            htmlRenderer.WriteAttributesExcludingClass(null);
            string result = stringWriter.ToString();

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void WriteAttributesExcludingClass_WritesAttributesExcludingClass()
        {
            // Arrange
            const string dummyAttribute = "dummyAttribute";
            const string dummyValue = "dummyValue";
            var dummyAttributes = new ReadOnlyDictionary<string, string>(
                new Dictionary<string, string>
                {
                    { dummyAttribute, dummyValue },
                    { "class", "dummyClass" }
                });
            var stringWriter = new StringWriter();
            var htmlRenderer = new HtmlRenderer(stringWriter);

            // Act
            htmlRenderer.WriteAttributesExcludingClass(dummyAttributes);
            string result = stringWriter.ToString();

            // Assert
            Assert.Equal($" {dummyAttribute}=\"{dummyValue}\"", result);
        }

        [Fact]
        public void WriteAttributesExcludingClassAndID_WritesNothingIfAttributesIsNull()
        {
            // Arrange
            var stringWriter = new StringWriter();
            var htmlRenderer = new HtmlRenderer(stringWriter);

            // Act
            htmlRenderer.WriteAttributesExcludingClassAndID(null);
            string result = stringWriter.ToString();

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void WriteAttributesExcludingClassAndID_WritesAttributesExcludingClassAndID()
        {
            // Arrange
            const string dummyAttribute = "dummyAttribute";
            const string dummyValue = "dummyValue";
            var dummyAttributes = new ReadOnlyDictionary<string, string>(
                new Dictionary<string, string>
                {
                    { dummyAttribute, dummyValue },
                    { "class", "dummyClass" },
                    { "id", "dummyID" }
                });
            var stringWriter = new StringWriter();
            var htmlRenderer = new HtmlRenderer(stringWriter);

            // Act
            htmlRenderer.WriteAttributesExcludingClassAndID(dummyAttributes);
            string result = stringWriter.ToString();

            // Assert
            Assert.Equal($" {dummyAttribute}=\"{dummyValue}\"", result);
        }

        [Theory]
        [MemberData(nameof(WriteHtmlFragmentWithClass_WritesHtmlFragmentWithClassConditionally_Data))]
        public void WriteHtmlFragmentWithClass_WritesHtmlFragmentWithClassConditionally(bool dummyCondition,
            string dummyHtmlFragment,
            string dummyClassValuePart1,
            string dummyClassValuePart2,
            string expectedHtml)
        {
            // Arrange
            var stringWriter = new StringWriter();
            var htmlRenderer = new HtmlRenderer(stringWriter);

            // Act
            htmlRenderer.WriteHtmlFragmentWithClass(dummyCondition, dummyHtmlFragment, dummyClassValuePart1, dummyClassValuePart2);
            string result = stringWriter.ToString();

            // Assert
            Assert.Equal(expectedHtml, result);
        }

        public static IEnumerable<object[]> WriteHtmlFragmentWithClass_WritesHtmlFragmentWithClassConditionally_Data()
        {
            const string dummyClassValuePart1 = "dummyClassValuePart1";
            const string dummyClassValuePart2 = "dummyClassValuePart2";

            return new object[][]
            {
                // Single element
                new object[]{true, "<test></test>", dummyClassValuePart1, dummyClassValuePart2, $"<test class=\"{dummyClassValuePart1}{dummyClassValuePart2}\"></test>"},
                // Multiple nested elements
                new object[]{true, "<test><test></test></test>", dummyClassValuePart1, dummyClassValuePart2, $"<test class=\"{dummyClassValuePart1}{dummyClassValuePart2}\"><test></test></test>"},
                // Series of elements
                new object[]{true, "<test></test><test></test>", dummyClassValuePart1, dummyClassValuePart2, $"<test class=\"{dummyClassValuePart1}{dummyClassValuePart2}\"></test><test></test>"},
                // Characters and spaces before first element
                new object[]{true, "this is a valid HTML fragment <test></test>", dummyClassValuePart1, dummyClassValuePart2, $"this is a valid HTML fragment <test class=\"{dummyClassValuePart1}{dummyClassValuePart2}\"></test>"},
                // Self closing tag
                new object[]{true, "<test/>", dummyClassValuePart1, dummyClassValuePart2, $"<test class=\"{dummyClassValuePart1}{dummyClassValuePart2}\"/>"},
                // No elements                
                new object[]{true, "this is a valid HTML fragment", dummyClassValuePart1, dummyClassValuePart2, "this is a valid HTML fragment"},
                // Null class value part 1
                new object[]{true, "<test></test>", null, dummyClassValuePart2, $"<test class=\"{dummyClassValuePart2}\"></test>"},
                // Null class value part 2
                new object[]{true, "<test></test>", dummyClassValuePart1, null, $"<test class=\"{dummyClassValuePart1}\"></test>"},
                // Null class values
                new object[]{true, "<test></test>", null, null, "<test></test>"},
                // Condition false
                new object[]{false, "<test></test>", dummyClassValuePart1, dummyClassValuePart2, string.Empty}
            };
        }

        [Theory]
        [MemberData(nameof(WriteChildren_WithCorrectImplicitParagraphsSetting_Data))]
        public void WriteChildren_WithCorrectImplicitParagraphsSetting(bool dummyImplicitParagraphs, string dummyText, string expectedResult)
        {
            // Arrange
            var dummyContainerInline = new ContainerInline();
            dummyContainerInline.AppendChild(new LiteralInline(dummyText));
            var dummyParagraphBlock = new ParagraphBlock() { Inline = dummyContainerInline };
            Mock<ContainerBlock> dummyContainerBlock = _mockRepository.Create<ContainerBlock>(null);
            dummyContainerBlock.CallBase = true;
            dummyContainerBlock.Object.Add(dummyParagraphBlock);
            var stringWriter = new StringWriter();
            var htmlRenderer = new HtmlRenderer(stringWriter);
            htmlRenderer.ImplicitParagraph = !dummyImplicitParagraphs; // Should get stored and reset after rendering

            // Act
            htmlRenderer.WriteChildren(dummyContainerBlock.Object, dummyImplicitParagraphs);
            string result = stringWriter.ToString();

            // Arrange
            Assert.Equal(expectedResult, result);
            Assert.Equal(!dummyImplicitParagraphs, htmlRenderer.ImplicitParagraph);
        }

        public static IEnumerable<object[]> WriteChildren_WithCorrectImplicitParagraphsSetting_Data()
        {
            return new object[][]
            {
                new object[] {true, "dummyText", "dummyText"},
                new object[] {false, "dummyText", "<p>dummyText</p>\n"}
            };
        }

        [Theory]
        [MemberData(nameof(FindFirstTag_FindsFirstTag_Data))]
        public void FindFirstTag_FindsFirstTag(string dummyHtmlFragment, (int, int, int) expectedResult)
        {
            // Act
            (int, int, int) result = HtmlRendererExtensions.FindFirstTag(dummyHtmlFragment);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> FindFirstTag_FindsFirstTag_Data()
        {
            return new object[][]
            {
                // Start tag
                new object[]{"<dummyTag>", (0, 9, 8)},
                // Self closing tag
                new object[]{ "<dummyTag/>", (0, 10, 8)},
                // Start tag with attributes
                new object[]{ "<dummyTag attributes=\"value\">", (0, 28, 8)},
                // Start tag with leading characters
                new object[]{ "leading characters <dummyTag>", (19, 28, 27)},
                // Incomplete start tag
                new object[]{ "<dummyTag", (-1, -1, -1)},
                // Fragment with no start tag
                new object[]{ "fragment", (-1, -1, -1)},
                // Start tag with random whitespace characters
                new object[]{ @"<dummyTag
attributes = ""value""
/>", (0, 32, 8)}
            };
        }
    }
}
