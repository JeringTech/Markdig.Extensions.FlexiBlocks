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
            bool dummyGenerateIdentifier = false;
            bool dummyAutoLinkable = false;
            SectioningContentElement dummyLevel1WrapperElement = SectioningContentElement.Article;
            SectioningContentElement dummyLevel2PlusWrapperElement = SectioningContentElement.Nav;
            string dummyAttributeKey1 = "dummyAttributeKey1";
            string dummyAttributeValue1 = "dummyAttributeValue1";
            string dummyAttributeKey2 = "dummyAttributeKey2";
            string dummyAttributeValue2 = "dummyAttributeValue2";
            Dictionary<string, string> dummyAttributes = new Dictionary<string, string>()
            {
                {dummyAttributeKey1, dummyAttributeValue1 },
                {dummyAttributeKey2, dummyAttributeValue2 }
            };
            SectionBlockOptions sectionBlockOptions = new SectionBlockOptions()
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
