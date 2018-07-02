using FlexiBlocks.FlexiAlertBlocks;
using Xunit;

namespace FlexiBlocks.Tests.FlexiAlertBlocks
{
    public class FlexiAlertBlockOptionsIntegrationTests
    {
        [Fact]
        public void Clone_ReturnsADeepClone()
        {
            // Arrange
            const string dummyIconMarkup = "dummyIconMarkup";
            const string dummyAttributeKey1 = "dummyAttributeKey1";
            const string dummyAttributeValue1 = "dummyAttributeValue1";
            const string dummyAttributeKey2 = "dummyAttributeKey2";
            const string dummyAttributeValue2 = "dummyAttributeValue2";
            var dummyAttributes = new HtmlAttributeDictionary()
            {
                {dummyAttributeKey1, dummyAttributeValue1 },
                {dummyAttributeKey2, dummyAttributeValue2 }
            };
            var flexiAlertBlockOptions = new FlexiAlertBlockOptions()
            {
                IconMarkup = dummyIconMarkup,
                Attributes = dummyAttributes
            };

            // Act
            FlexiAlertBlockOptions result = flexiAlertBlockOptions.Clone();

            // Assert
            Assert.NotSame(flexiAlertBlockOptions, result);
            Assert.Equal(dummyIconMarkup, result.IconMarkup);
            HtmlAttributeDictionary resultAttributes = result.Attributes;
            Assert.Equal(2, resultAttributes.Count);
            Assert.Equal(dummyAttributeValue1, resultAttributes[dummyAttributeKey1]);
            Assert.Equal(dummyAttributeValue2, resultAttributes[dummyAttributeKey2]);
        }
    }
}
