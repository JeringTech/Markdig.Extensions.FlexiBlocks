using System;
using System.IO;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiIncludeBlocks
{
    public class ContentRetrievalServiceIntegrationTestsFixture : IDisposable
    {
        public string TempDirectory { get; } = Path.Combine(Path.GetTempPath(), nameof(ContentRetrievalServiceIntegrationTests)); // Dummy file for creating dummy file streams

        public ContentRetrievalServiceIntegrationTestsFixture()
        {
            Directory.CreateDirectory(TempDirectory);
        }

        public void Dispose()
        {
            try
            {
                Directory.Delete(TempDirectory, true);
            }
            catch
            {
                // Do nothing
            }
        }
    }
}
