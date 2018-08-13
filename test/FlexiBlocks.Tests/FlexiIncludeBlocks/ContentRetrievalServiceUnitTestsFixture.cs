using System;
using System.IO;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiIncludeBlocks
{
    public class ContentRetrievalServiceUnitTestsFixture : IDisposable
    {
        public string TempDirectory { get; } = Path.Combine(Path.GetTempPath(), nameof(ContentRetrievalServiceUnitTests)); // Dummy file for creating dummy file streams

        public ContentRetrievalServiceUnitTestsFixture()
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
