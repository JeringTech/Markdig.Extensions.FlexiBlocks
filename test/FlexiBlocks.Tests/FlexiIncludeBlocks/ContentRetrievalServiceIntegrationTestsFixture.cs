using System;
using System.IO;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiIncludeBlocks
{
    public class ContentRetrievalServiceIntegrationTestsFixture : IDisposable
    {
        public string TempDirectory { get; } = Path.Combine(Path.GetTempPath(), nameof(ContentRetrievalServiceIntegrationTests)); // Dummy file for creating dummy file streams

        public ContentRetrievalServiceIntegrationTestsFixture()
        {
            TryDeleteDirectory(); // Delete directory if it already exists so that pre-existing files don't mess up tests
            Directory.CreateDirectory(TempDirectory);
        }

        private void TryDeleteDirectory()
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

        public void Dispose()
        {
            TryDeleteDirectory();
        }
    }
}
