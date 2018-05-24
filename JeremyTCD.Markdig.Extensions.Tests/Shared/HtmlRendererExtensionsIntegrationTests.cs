using Markdig.Renderers;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace JeremyTCD.Markdig.Extensions.Tests.Shared
{
    public class HtmlRendererExtensionsIntegrationTests
    {
        [Fact]
        public void WriteAttributeMap_WritesAttributes()
        {
            // Arrange
            const string dummyAttribute1 = "dummyAttribute1";
            const string dummyValue1 = "dummyValue1";
            const string dummyAttribute2 = "dummyAttribute2";
            const string dummyValue2 = "dummyValue2";
            var dummyAttributes = new Dictionary<string, string>()
            {
                { dummyAttribute1, dummyValue1 },
                { dummyAttribute2, dummyValue2 }
            };
            string result = null;
            using (var stringWriter = new StringWriter())
            {
                var htmlRenderer = new HtmlRenderer(stringWriter);

                // Act
                htmlRenderer.WriteAttributeMap(dummyAttributes);
                result = stringWriter.ToString();
            }

            // Assert
            Assert.Equal($" {dummyAttribute1}=\"{dummyValue1}\" {dummyAttribute2}=\"{dummyValue2}\"", result);
        }
    }
}
