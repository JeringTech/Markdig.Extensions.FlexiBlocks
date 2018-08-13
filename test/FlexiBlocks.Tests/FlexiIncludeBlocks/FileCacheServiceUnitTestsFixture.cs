using System;
using System.IO;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiIncludeBlocks
{
    public class FileCacheServiceUnitTestsFixture : IDisposable
    {
        public string TempDirectory { get; } = Path.Combine(Path.GetTempPath(), nameof(FileCacheServiceUnitTests)); // Dummy file for creating dummy file streams

        public FileCacheServiceUnitTestsFixture()
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
