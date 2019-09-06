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
        [MemberData(nameof(WriteInt_WritesInt_Data))]
        public void WriteInt_WritesInt(int dummyInt, string expectedResult)
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.WriteInt(dummyInt);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> WriteInt_WritesInt_Data()
        {
            return new object[][]
            {
                new object[]{3, "3"},
                new object[]{54, "54"},
                new object[]{898, "898"},
                new object[]{2384, "2384"},
                new object[]{12456, "12456"},
                new object[]{490586, "490586"},
                new object[]{2984576, "2984576"},
                new object[]{21478950, "21478950"},
                new object[]{348905764, "348905764"},
                new object[]{1904857690, "1904857690"},
                new object[]{-1904857690, "-1904857690"},
            };
        }

        [Fact]
        public void WriteAttribute_WithoutCondition_WritesAttribute()
        {
            // Arrange
            const string dummyAttributeName = "dummyAttributeName";
            const string dummyValue = "dummyValue";
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.WriteAttribute(dummyAttributeName, dummyValue);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal($" {dummyAttributeName}=\"{dummyValue}\"", result);
        }

        [Theory]
        [MemberData(nameof(WriteAttribute_WithCondition_WritesAttributeConditionally_Data))]
        public void WriteAttribute_WithCondition_WritesAttributeConditionally(bool dummyCondition, string dummyAttributeName, string dummyValue, string expectedResult)
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.WriteAttribute(dummyCondition, dummyAttributeName, dummyValue);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> WriteAttribute_WithCondition_WritesAttributeConditionally_Data()
        {
            const string dummyAttributeName = "dummyAttributeName";
            const string dummyValue = "dummyValue";

            return new object[][]
            {
                new object[]{ true, dummyAttributeName, dummyValue, $" {dummyAttributeName}=\"{dummyValue}\"" },
                new object[]{ false, string.Empty, string.Empty, string.Empty }
            };
        }

        [Theory]
        [MemberData(nameof(WriteAttributeValue_WritesAttributeValue_Data))]
        public void WriteAttributeValue_WritesAttributeValue(ReadOnlyDictionary<string, string> dummyAttributes, string dummyAttributeKey, string expectedResult)
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.WriteAttributeValue(dummyAttributes, dummyAttributeKey);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> WriteAttributeValue_WritesAttributeValue_Data()
        {
            const string dummyAttributeKey = "dummyAttributeKey";
            const string dummyValue = "dummyValue";

            return new object[][]
            {
                new object[]{ new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { {dummyAttributeKey, dummyValue } }), dummyAttributeKey, $" {dummyValue}" },
                // Null attributes
                new object[]{ null, dummyAttributeKey, string.Empty },
                // Attributes doesn't contain key
                new object[]{ new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()), dummyAttributeKey, string.Empty }
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

        [Theory]
        [MemberData(nameof(WriteAttributesExcept_OneExclude_Data))]
        public void WriteAttributesExcept_OneExclude(ReadOnlyDictionary<string, string> dummyAttributes, string dummyExcluded, string expectedResult)
        {
            // Arrange
            var stringWriter = new StringWriter();
            var htmlRenderer = new HtmlRenderer(stringWriter);

            // Act
            htmlRenderer.WriteAttributesExcept(dummyAttributes, dummyExcluded);
            string result = stringWriter.ToString();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> WriteAttributesExcept_OneExclude_Data()
        {
            const string dummyExcluded = "dummyExcluded";
            const string dummyNotExcluded = "dummyNotExcluded";
            const string dummyNotExcludedValue = "dummyNotExcludedValue";

            return new object[][]
            {
                new object[]{null, dummyExcluded, string.Empty},
                new object[]{
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{
                        { dummyExcluded, "dummyExcludedValue" },
                        { dummyNotExcluded, dummyNotExcludedValue }
                    }),
                    dummyExcluded,
                    $" {dummyNotExcluded}=\"{dummyNotExcludedValue}\""}
            };
        }

        [Theory]
        [MemberData(nameof(WriteAttributesExcept_TwoExcludes_Data))]
        public void WriteAttributesExcept_TwoExcludes(ReadOnlyDictionary<string, string> dummyAttributes, string dummyExcluded1, string dummyExcluded2, string expectedResult)
        {
            // Arrange
            var stringWriter = new StringWriter();
            var htmlRenderer = new HtmlRenderer(stringWriter);

            // Act
            htmlRenderer.WriteAttributesExcept(dummyAttributes, dummyExcluded1, dummyExcluded2);
            string result = stringWriter.ToString();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> WriteAttributesExcept_TwoExcludes_Data()
        {
            const string dummyExcluded1 = "dummyExcluded1";
            const string dummyExcluded2 = "dummyExcluded2";
            const string dummyNotExcluded = "dummyNotExcluded";
            const string dummyNotExcludedValue = "dummyNotExcludedValue";

            return new object[][]
            {
                new object[]{null, dummyExcluded1, dummyExcluded2, string.Empty},
                new object[]{
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{
                        { dummyExcluded1, "dummyExcluded1Value" },
                        { dummyExcluded2, "dummyExcluded2Value" },
                        { dummyNotExcluded, dummyNotExcludedValue }
                    }),
                    dummyExcluded1,
                    dummyExcluded2,
                    $" {dummyNotExcluded}=\"{dummyNotExcludedValue}\""}
            };
        }

        [Theory]
        [MemberData(nameof(WriteHasFeatureClass_BlockModifierClass_WritesHasFeatureClass_Data))]
        public void WriteHasFeatureClass_BlockModifierClass_WritesHasFeatureClass(bool dummyHasFeature, string dummyBlockName, string dummyFeatureName, string expectedResult)
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.WriteHasFeatureClass(dummyHasFeature, dummyBlockName, dummyFeatureName);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> WriteHasFeatureClass_BlockModifierClass_WritesHasFeatureClass_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string dummyFeatureName = "dummyFeatureName";

            return new object[][]
            {
                new object[]{true, dummyBlockName, dummyFeatureName, $" {dummyBlockName}_has_{dummyFeatureName}"},
                new object[]{false, dummyBlockName, dummyFeatureName, $" {dummyBlockName}_no_{dummyFeatureName}" }
            };
        }

        [Theory]
        [MemberData(nameof(WriteHasFeatureClass_ElementModifierClass_WritesHasFeatureClass_Data))]
        public void WriteHasFeatureClass_ElementModifierClass_WritesHasFeatureClass(bool dummyHasFeature, string dummyBlockName, string dummyElementName, string dummyFeatureName, string expectedResult)
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.WriteHasFeatureClass(dummyHasFeature, dummyBlockName, dummyElementName, dummyFeatureName);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> WriteHasFeatureClass_ElementModifierClass_WritesHasFeatureClass_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string dummyElementName = "dummyElementName";
            const string dummyFeatureName = "dummyFeatureName";

            return new object[][]
            {
                new object[]{true, dummyBlockName, dummyElementName, dummyFeatureName, $" {dummyBlockName}__{dummyElementName}_has_{dummyFeatureName}"},
                new object[]{false, dummyBlockName, dummyElementName, dummyFeatureName, $" {dummyBlockName}__{dummyElementName}_no_{dummyFeatureName}" }
            };
        }

        [Theory]
        [MemberData(nameof(WriteIsTypeClass_ElementModifierClass_WritesIsTypeClass_Data))]
        public void WriteIsTypeClass_ElementModifierClass_WritesIsTypeClass(bool dummyIsType, string dummyBlockName, string dummyElementName, string dummyTypeName, string expectedResult)
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.WriteIsTypeClass(dummyIsType, dummyBlockName, dummyElementName, dummyTypeName);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> WriteIsTypeClass_ElementModifierClass_WritesIsTypeClass_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string dummyElementName = "dummyElementName";
            const string dummyTypeName = "dummyTypeName";

            return new object[][]
            {
                new object[]{true, dummyBlockName, dummyElementName, dummyTypeName, $" {dummyBlockName}__{dummyElementName}_is_{dummyTypeName}"},
                new object[]{false, dummyBlockName, dummyElementName, dummyTypeName, string.Empty }
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
        public void WriteStartTagLine_WritesStartTagLine()
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
        public void WriteStartTagWithClasses_WritesStartTagWithClasses()
        {
            // Arrange
            const string dummyTagName = "dummyTagName";
            const string dummyBlockName = "dummyBlockName";
            const string dummyElementName = "dummyElementName";
            const string dummyClasses = "dummyClasses";
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.WriteStartTagWithClasses(dummyTagName, dummyBlockName, dummyElementName, dummyClasses);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal($"<{dummyTagName} class=\"{dummyBlockName}__{dummyElementName} {dummyClasses}\">", result, ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void WriteStartTagLineWithClasses_WritesStartTagLineWithClasses()
        {
            // Arrange
            const string dummyTagName = "dummyTagName";
            const string dummyBlockName = "dummyBlockName";
            const string dummyElementName = "dummyElementName";
            const string dummyClasses = "dummyClasses";
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.WriteStartTagLineWithClasses(dummyTagName, dummyBlockName, dummyElementName, dummyClasses);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal($"<{dummyTagName} class=\"{dummyBlockName}__{dummyElementName} {dummyClasses}\">\n", result, ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void WriteStartTagWithAttributes_WritesStartTagWithAttributes()
        {
            // Arrange
            const string dummyTagName = "dummyTagName";
            const string dummyBlockName = "dummyBlockName";
            const string dummyElementName = "dummyElementName";
            const string dummyAttributes = "dummyAttributes";
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.WriteStartTagWithAttributes(dummyTagName, dummyBlockName, dummyElementName, dummyAttributes);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal($"<{dummyTagName} class=\"{dummyBlockName}__{dummyElementName}\" {dummyAttributes}>", result, ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void WriteStartTagLineWithAttributes_WritesStartTagLineWithAttributes()
        {
            // Arrange
            const string dummyTagName = "dummyTagName";
            const string dummyBlockName = "dummyBlockName";
            const string dummyElementName = "dummyElementName";
            const string dummyAttributes = "dummyAttributes";
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.WriteStartTagLineWithAttributes(dummyTagName, dummyBlockName, dummyElementName, dummyAttributes);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal($"<{dummyTagName} class=\"{dummyBlockName}__{dummyElementName}\" {dummyAttributes}>\n", result, ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void WriteStartTagWithModifierClassAndAttributes_WritesStartTagWithModifierClassAndAttributes()
        {
            // Arrange
            const string dummyTagName = "dummyTagName";
            const string dummyBlockName = "dummyBlockName";
            const string dummyElementName = "dummyElementName";
            const string dummyModifier = "dummyModifier";
            const string dummyAttributes = "dummyAttributes";
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.WriteStartTagWithModifierClassAndAttributes(dummyTagName, dummyBlockName, dummyElementName, dummyModifier, dummyAttributes);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal($"<{dummyTagName} class=\"{dummyBlockName}__{dummyElementName} {dummyBlockName}__{dummyElementName}_{dummyModifier}\" {dummyAttributes}>", result, ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void WriteStartTagLineWithModifierClassAndAttributes_WritesStartTagLineWithModifierClassAndAttributes()
        {
            // Arrange
            const string dummyTagName = "dummyTagName";
            const string dummyBlockName = "dummyBlockName";
            const string dummyElementName = "dummyElementName";
            const string dummyModifier = "dummyModifier";
            const string dummyAttributes = "dummyAttributes";
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.WriteStartTagLineWithModifierClassAndAttributes(dummyTagName, dummyBlockName, dummyElementName, dummyModifier, dummyAttributes);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal($"<{dummyTagName} class=\"{dummyBlockName}__{dummyElementName} {dummyBlockName}__{dummyElementName}_{dummyModifier}\" {dummyAttributes}>\n", result, ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void WriteStartTagLineWithModifierClass_WritesStartTagLineWithModifierClass()
        {
            // Arrange
            const string dummyTagName = "dummyTagName";
            const string dummyBlockName = "dummyBlockName";
            const string dummyElementName = "dummyElementName";
            const string dummyModifier = "dummyModifier";
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.WriteStartTagLineWithModifierClass(dummyTagName, dummyBlockName, dummyElementName, dummyModifier);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal($"<{dummyTagName} class=\"{dummyBlockName}__{dummyElementName} {dummyBlockName}__{dummyElementName}_{dummyModifier}\">\n", result, ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void WriteStartTagLineWithClassesAndAttributes_WritesStartTagLineWithClassesAndAttributes()
        {
            // Arrange
            const string dummyTagName = "dummyTagName";
            const string dummyBlockName = "dummyBlockName";
            const string dummyElementName = "dummyElementName";
            const string dummyClasses = "dummyClasses";
            const string dummyAttributes = "dummyAttributes";
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.WriteStartTagLineWithClassesAndAttributes(dummyTagName, dummyBlockName, dummyElementName, dummyClasses, dummyAttributes);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal($"<{dummyTagName} class=\"{dummyBlockName}__{dummyElementName} {dummyClasses}\" {dummyAttributes}>\n", result, ignoreLineEndingDifferences: true);
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
        public void WriteElementModifierClass_WritesElementModifierClass()
        {
            // Arrange
            const string dummyBlockName = "dummyBlockName";
            const string dummyElementName = "dummyElementName";
            const string dummyModifier = "dummyModifier";
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.WriteElementModifierClass(dummyBlockName, dummyElementName, dummyModifier);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal($" {dummyBlockName}__{dummyElementName}_{dummyModifier}", result);
        }

        [Theory]
        [MemberData(nameof(WriteElementModifierClass_WithCondition_WritesElementModifierClassConditionally_Data))]
        public void WriteElementModifierClass_WithCondition_WritesElementModifierClassConditionally(bool dummyCondition, string dummyBlockName, string dummyElementName, string dummyModifier, string expectedResult)
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var testSubject = new HtmlRenderer(dummyStringWriter);

            // Act
            testSubject.WriteElementModifierClass(dummyCondition, dummyBlockName, dummyElementName, dummyModifier);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> WriteElementModifierClass_WithCondition_WritesElementModifierClassConditionally_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string dummyElementName = "dummyElementName";
            const string dummyModifier = "dummyModifier";

            return new object[][]
            {
                new object[]{ true, dummyBlockName, dummyElementName, dummyModifier, $" {dummyBlockName}__{dummyElementName}_{dummyModifier}" },
                new object[]{ false, dummyBlockName, dummyElementName, dummyModifier, string.Empty }
            };
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
        [MemberData(nameof(Write_WithCondition_CharPartStringPart_WritesConditionally_Data))]
        public void Write_WithCondition_CharPartStringPart_WritesConditionally(bool dummyCondition, char dummyPart1, string dummyPart2, string expectedResult)
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

        public static IEnumerable<object[]> Write_WithCondition_CharPartStringPart_WritesConditionally_Data()
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
        [MemberData(nameof(Write_WithCondition_StringPartStringPart_WritesConditionally_Data))]
        public void Write_WithCondition_StringPartStringPart_WritesConditionally(bool dummyCondition, string dummyPart1, string dummyPart2, string expectedResult)
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

        public static IEnumerable<object[]> Write_WithCondition_StringPartStringPart_WritesConditionally_Data()
        {
            const string dummyPart1 = "dummyPart1";
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

        [Theory]
        [MemberData(nameof(WriteHtmlFragmentWithClass_WritesHtmlFragmentWithClassConditionally_Data))]
        public void WriteHtmlFragmentWithClass_WritesHtmlFragmentWithClassConditionally(bool dummyCondition,
            string dummyHtmlFragment,
            string dummyBlockName,
            string dummyElementName,
            string expectedHtml)
        {
            // Arrange
            var stringWriter = new StringWriter();
            var htmlRenderer = new HtmlRenderer(stringWriter);

            // Act
            htmlRenderer.WriteHtmlFragment(dummyCondition, dummyHtmlFragment, dummyBlockName, dummyElementName);
            string result = stringWriter.ToString();

            // Assert
            Assert.Equal(expectedHtml, result);
        }

        public static IEnumerable<object[]> WriteHtmlFragmentWithClass_WritesHtmlFragmentWithClassConditionally_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string dummyElementName = "dummyElementName";

            return new object[][]
            {
                // Single element
                new object[]{true, "<test></test>", dummyBlockName, dummyElementName, $"<test class=\"{dummyBlockName}__{dummyElementName}\"></test>"},
                // Multiple nested elements
                new object[]{true, "<test><test></test></test>", dummyBlockName, dummyElementName, $"<test class=\"{dummyBlockName}__{dummyElementName}\"><test></test></test>"},
                // Series of elements
                new object[]{true, "<test></test><test></test>", dummyBlockName, dummyElementName, $"<test class=\"{dummyBlockName}__{dummyElementName}\"></test><test></test>"},
                // Characters and spaces before first element
                new object[]{true, "this is a valid HTML fragment <test></test>", dummyBlockName, dummyElementName, $"this is a valid HTML fragment <test class=\"{dummyBlockName}__{dummyElementName}\"></test>"},
                // Self closing tag
                new object[]{true, "<test/>", dummyBlockName, dummyElementName, $"<test class=\"{dummyBlockName}__{dummyElementName}\"/>"},
                // No elements                
                new object[]{true, "this is a valid HTML fragment", dummyBlockName, dummyElementName, "this is a valid HTML fragment"},
                // Null block name
                new object[]{true, "<test></test>", null, dummyElementName, $"<test class=\"__{dummyElementName}\"></test>"},
                // Null element name
                new object[]{true, "<test></test>", dummyBlockName, null, $"<test class=\"{dummyBlockName}__\"></test>"},
                // Null block and element names
                new object[]{true, "<test></test>", null, null, "<test class=\"__\"></test>"},
                // Condition false
                new object[]{false, "<test></test>", dummyBlockName, dummyElementName, string.Empty}
            };
        }

        [Theory]
        [MemberData(nameof(WriteChildren_WithSpecifiedImplicitParagraphsSetting_Data))]
        public void WriteChildren_WithSpecifiedImplicitParagraphsSetting(bool dummyImplicitParagraphs, string dummyText, string expectedResult)
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

        public static IEnumerable<object[]> WriteChildren_WithSpecifiedImplicitParagraphsSetting_Data()
        {
            return new object[][]
            {
                new object[] {true, "dummyText", "dummyText"},
                new object[] {false, "dummyText", "<p>dummyText</p>\n"}
            };
        }

        [Theory]
        [MemberData(nameof(WriteLeafInline_WithSpecifiedEnableHtmlForInlineSetting_Data))]
        public void WriteLeafInline_WithSpecifiedEnableHtmlForInlineSetting(bool dummyEnableHtmlForInline, Inline dummyInline, string expectedResult)
        {
            // Arrange
            var dummyContainerInline = new ContainerInline();
            dummyContainerInline.AppendChild(dummyInline);
            var dummyParagraphBlock = new ParagraphBlock() { Inline = dummyContainerInline };
            var stringWriter = new StringWriter();
            var htmlRenderer = new HtmlRenderer(stringWriter);
            htmlRenderer.ImplicitParagraph = !dummyEnableHtmlForInline; // Should get stored and reset after rendering

            // Act
            htmlRenderer.WriteLeafInline(dummyParagraphBlock, dummyEnableHtmlForInline);
            string result = stringWriter.ToString();

            // Arrange
            Assert.Equal(expectedResult, result);
            Assert.Equal(!dummyEnableHtmlForInline, htmlRenderer.ImplicitParagraph);
        }

        public static IEnumerable<object[]> WriteLeafInline_WithSpecifiedEnableHtmlForInlineSetting_Data()
        {
            const string dummyUrl = "dummyUrl";
            const string dummyTitle = "dummyTitle";

            // Note, LinkInline can't be shared because it can only have 1 parent. Also, LinkInline is arbitrarily chosen.
            return new object[][]
            {
                new object[] {true, new LinkInline(dummyUrl, dummyTitle), $"<a href=\"{dummyUrl}\" title=\"{dummyTitle}\"></a>"},
                new object[] {false, new LinkInline(dummyUrl, dummyTitle), string.Empty}
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
