using JeremyTCD.Markdig.Extensions.Sections;
using System.Collections.Generic;
using Xunit;

namespace JeremyTCD.Markdig.Extensions.Tests.Sections
{
    public class SectionBlockOptionsIntegrationTests
    {
        [Fact]
        public void Clone_ReturnsADeepClone()
        {
            // Arrange
            const bool dummyGenerateIdentifier = false;
            const bool dummyAutoLinkable = false;
            const SectioningContentElement dummyLevel1WrapperElement = SectioningContentElement.Article;
            const SectioningContentElement dummyLevel2PlusWrapperElement = SectioningContentElement.Nav;
            const string dummyAttributeKey1 = "dummyAttributeKey1";
            const string dummyAttributeValue1 = "dummyAttributeValue1";
            const string dummyAttributeKey2 = "dummyAttributeKey2";
            const string dummyAttributeValue2 = "dummyAttributeValue2";
            var dummyAttributes = new Dictionary<string, string>()
            {
                {dummyAttributeKey1, dummyAttributeValue1 },
                {dummyAttributeKey2, dummyAttributeValue2 }
            };
            var sectionBlockOptions = new SectionBlockOptions()
            {
                GenerateIdentifier = dummyGenerateIdentifier,
                AutoLinkable = dummyAutoLinkable,
                Level1WrapperElement = dummyLevel1WrapperElement,
                Level2PlusWrapperElement = dummyLevel2PlusWrapperElement,
                Attributes = dummyAttributes
            };

            // Act
            SectionBlockOptions result = sectionBlockOptions.Clone();

            // Assert
            Assert.NotSame(sectionBlockOptions, result);
            Assert.Equal(dummyGenerateIdentifier, result.GenerateIdentifier);
            Assert.Equal(dummyAutoLinkable, result.AutoLinkable);
            Assert.Equal(dummyLevel1WrapperElement, result.Level1WrapperElement);
            Assert.Equal(dummyLevel2PlusWrapperElement, result.Level2PlusWrapperElement);
            Dictionary<string, string> resultAttributes = result.Attributes;
            Assert.Equal(2, resultAttributes.Count);
            Assert.Equal(dummyAttributeValue1, resultAttributes[dummyAttributeKey1]);
            Assert.Equal(dummyAttributeValue2, resultAttributes[dummyAttributeKey2]);
        }
    }
}
