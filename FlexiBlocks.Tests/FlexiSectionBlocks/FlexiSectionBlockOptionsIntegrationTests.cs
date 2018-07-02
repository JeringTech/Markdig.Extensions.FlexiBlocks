using FlexiBlocks.FlexiSectionBlocks;
using System.Collections.Generic;
using Xunit;

namespace FlexiBlocks.Tests.FlexiSectionBlocks
{
    public class FlexiSectionBlockOptionsIntegrationTests
    {
        [Fact]
        public void Clone_ReturnsADeepClone()
        {
            // Arrange
            const bool dummyGenerateIdentifier = false;
            const bool dummyAutoLinkable = false;
            const SectioningContentElement dummySectioningContentElement = SectioningContentElement.Article;
            const string dummyAttributeKey1 = "dummyAttributeKey1";
            const string dummyAttributeValue1 = "dummyAttributeValue1";
            const string dummyAttributeKey2 = "dummyAttributeKey2";
            const string dummyAttributeValue2 = "dummyAttributeValue2";
            var dummyAttributes = new HtmlAttributeDictionary()
            {
                {dummyAttributeKey1, dummyAttributeValue1 },
                {dummyAttributeKey2, dummyAttributeValue2 }
            };
            var sectionBlockOptions = new FlexiSectionBlockOptions()
            {
                GenerateIdentifier = dummyGenerateIdentifier,
                AutoLinkable = dummyAutoLinkable,
                WrapperElement = dummySectioningContentElement,
                Attributes = dummyAttributes
            };

            // Act
            FlexiSectionBlockOptions result = sectionBlockOptions.Clone();

            // Assert
            Assert.NotSame(sectionBlockOptions, result);
            Assert.Equal(dummyGenerateIdentifier, result.GenerateIdentifier);
            Assert.Equal(dummyAutoLinkable, result.AutoLinkable);
            Assert.Equal(dummySectioningContentElement, result.WrapperElement);
            HtmlAttributeDictionary resultAttributes = result.Attributes;
            Assert.Equal(2, resultAttributes.Count);
            Assert.Equal(dummyAttributeValue1, resultAttributes[dummyAttributeKey1]);
            Assert.Equal(dummyAttributeValue2, resultAttributes[dummyAttributeKey2]);
        }
    }
}
