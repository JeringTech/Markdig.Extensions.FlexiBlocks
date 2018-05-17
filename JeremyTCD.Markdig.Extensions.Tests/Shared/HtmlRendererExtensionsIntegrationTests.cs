using Markdig.Renderers;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace JeremyTCD.Markdig.Extensions.Tests.Shared
{
    public class HtmlRendererExtensionsIntegrationTests
    {
        [Fact]
        public void WriteCustomAttributes_WritesAttributes()
        {
            // Arrange
            string dummyAttribute1 = "dummyAttribute1";
            string dummyValue1 = "dummyValue1";
            string dummyAttribute2 = "dummyAttribute2";
            string dummyValue2 = "dummyValue2";
            Dictionary<string, string> dummyAttributes = new Dictionary<string, string>()
            {
                { dummyAttribute1, dummyValue1 },
                { dummyAttribute2, dummyValue2 }
            };
            string result = null;
            using (StringWriter stringWriter = new StringWriter())
            {
                HtmlRenderer htmlRenderer = new HtmlRenderer(stringWriter);

                // Act
                htmlRenderer.WriteCustomAttributes(dummyAttributes);
                result = stringWriter.ToString();
            }

            // Assert
            Assert.Equal($" {dummyAttribute1}=\"{dummyValue1}\" {dummyAttribute2}=\"{dummyValue2}\"", result);
        }
    }
}
