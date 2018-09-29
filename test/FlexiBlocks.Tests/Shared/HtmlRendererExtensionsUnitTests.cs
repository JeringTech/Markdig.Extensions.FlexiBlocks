using Markdig.Renderers;
using System.Collections.ObjectModel;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.Shared
{
    public class HtmlRendererExtensionsUnitTests
    {
        [Fact]
        public void WriteHtmlAttributeDictionary_WritesAttributes()
        {
            // Arrange
            const string dummyAttribute1 = "dummyAttribute1";
            const string dummyValue1 = "dummyValue1";
            const string dummyAttribute2 = "dummyAttribute2";
            const string dummyValue2 = "dummyValue2";
            var dummyAttributes = new HtmlAttributeDictionary()
            {
                { dummyAttribute1, dummyValue1 },
                { dummyAttribute2, dummyValue2 }
            };
            string result = null;
            using (var stringWriter = new StringWriter())
            {
                var htmlRenderer = new HtmlRenderer(stringWriter);

                // Act
                htmlRenderer.WriteAttributes(new ReadOnlyDictionary<string, string>(dummyAttributes));
                result = stringWriter.ToString();
            }

            // Assert
            Assert.Equal($" {dummyAttribute1}=\"{dummyValue1}\" {dummyAttribute2}=\"{dummyValue2}\"", result);
        }
    }
}
