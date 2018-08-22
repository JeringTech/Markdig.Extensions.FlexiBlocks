using System;
using System.IO;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiIncludeBlocks
{
    public class FlexiIncludeBlockParserIntegrationTestsFixture : IDisposable
    {
        public string TempDirectory { get; } = Path.Combine(Path.GetTempPath(), nameof(FlexiIncludeBlockParserIntegrationTests)); // Dummy file for creating dummy file streams

        public FlexiIncludeBlockParserIntegrationTestsFixture()
        {
            TryDeleteDirectory();
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
