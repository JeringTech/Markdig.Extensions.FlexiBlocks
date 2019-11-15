using Jering.IocServices.System.Net.Http;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiIncludeBlocks
{
    //These integration tests ensure that ContentRetrieverService utilizes its dependencies correctly, with an emphasis on caching of sources
    public class ContentRetrieverServiceIntegrationTests : IClassFixture<ContentRetrieverServiceIntegrationTestsFixture>, IDisposable
    {
        private readonly ContentRetrieverServiceIntegrationTestsFixture _fixture;
        private readonly string _dummyFile;
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default);
        private IServiceProvider _serviceProvider;

        public ContentRetrieverServiceIntegrationTests(ContentRetrieverServiceIntegrationTestsFixture fixture)
        {
            _fixture = fixture;
            _dummyFile = Path.Combine(fixture.TempDirectory, "dummyFile");

            var services = new ServiceCollection();
            services.AddFlexiBlocks();
            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void GetContent_CachesSourcesInMemory()
        {
            // Arrange
            const string dummySource = "this\nis\na\ndummy\nsource";
            var dummyUri = new Uri(_dummyFile);
            using (FileStream fileStream = File.Open(_dummyFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(dummySource);
                fileStream.Write(bytes, 0, bytes.Length);
            }
            IContentRetrieverService testSubject = _serviceProvider.GetRequiredService<IContentRetrieverService>();

            // Act
            var threads = new List<Thread>();
            var results = new ConcurrentBag<ReadOnlyCollection<string>>();
            for (int i = 0; i < 3; i++)
            {
                var thread = new Thread(() => results.Add(testSubject.GetContent(dummyUri)));
                thread.Start();
                threads.Add(thread);
            }
            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            // Assert
            ReadOnlyCollection<string>[] resultsArr = results.ToArray();
            Assert.Equal(3, resultsArr.Length);
            Assert.Equal(dummySource.Split('\n'), resultsArr[0]);
            Assert.Same(resultsArr[0], resultsArr[1]); // Result should get cached after first call
            Assert.Same(resultsArr[1], resultsArr[2]); // The same relation is transitive, so we do not need to check if the item at index 0 is the same as the item at index 2
        }

        [Fact]
        public void GetContent_CachesRemoteSourcesInFiles()
        {
            // Arrange
            // Arbitrary permalink
            const string url = "https://raw.githubusercontent.com/JeringTech/Markdig.Extensions.FlexiBlocks/6998b1c27821d8393ad39beb54f782515c39d98b/test/FlexiBlocks.Tests/exampleInclude.md";
            var dummyUri = new Uri(url);
            IContentRetrieverService testSubject = _serviceProvider.GetRequiredService<IContentRetrieverService>();
            ReadOnlyCollection<string> expectedResult = testSubject.GetContent(dummyUri, _fixture.TempDirectory);
            ((IDisposable)_serviceProvider).Dispose(); // Avoid retrieving from in-memory cache

            Mock<IHttpClientService> mockHttpClientService = _mockRepository.Create<IHttpClientService>();
            var services = new ServiceCollection();
            services.AddFlexiBlocks();
            services.AddSingleton(mockHttpClientService.Object);
            _serviceProvider = services.BuildServiceProvider();
            testSubject = _serviceProvider.GetRequiredService<IContentRetrieverService>();

            // Act
            ReadOnlyCollection<string> result = testSubject.GetContent(dummyUri, _fixture.TempDirectory);

            // Assert
            Assert.Equal(expectedResult, result);
            mockHttpClientService.Verify(h => h.GetAsync(It.IsAny<Uri>(), HttpCompletionOption.ResponseHeadersRead, default), Times.Never);
        }

        public void Dispose()
        {
            ((IDisposable)_serviceProvider).Dispose();
        }
    }

    public class ContentRetrieverServiceIntegrationTestsFixture : IDisposable
    {
        public string TempDirectory { get; } = Path.Combine(Path.GetTempPath(), nameof(ContentRetrieverServiceIntegrationTests)); // Dummy file for creating dummy file streams

        public ContentRetrieverServiceIntegrationTestsFixture()
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
