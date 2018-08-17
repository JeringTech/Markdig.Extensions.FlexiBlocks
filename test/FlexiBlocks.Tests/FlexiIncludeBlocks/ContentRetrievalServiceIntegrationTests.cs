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
    /// <summary>
    /// These integration tests ensure that ContentRetrievalService utilizes its dependencies correctly.
    /// </summary>
    public class ContentRetrievalServiceIntegrationTests : IClassFixture<ContentRetrievalServiceIntegrationTestsFixture>, IDisposable
    {
        private readonly ContentRetrievalServiceIntegrationTestsFixture _fixture;
        private readonly string _dummyFile;
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default);
        private IServiceProvider _serviceProvider;

        public ContentRetrievalServiceIntegrationTests(ContentRetrievalServiceIntegrationTestsFixture fixture)
        {
            _fixture = fixture;
            _dummyFile = Path.Combine(fixture.TempDirectory, "dummyFile");

            var services = new ServiceCollection();
            services.AddFlexiBlocks();
            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void GetContent_CachesContentInMemory()
        {
            // Arrange
            const string dummyContent = "this\nis\ndummy\ncontent";
            using(FileStream fileStream = File.Open(_dummyFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(dummyContent);
                fileStream.Write(bytes, 0, bytes.Length);
            }
            IContentRetrievalService testSubject = _serviceProvider.GetRequiredService<IContentRetrievalService>();

            // Act
            var threads = new List<Thread>();
            var results = new ConcurrentBag<ReadOnlyCollection<string>>();
            for (int i = 0; i < 3; i++)
            {
                var thread = new Thread(() => results.Add(testSubject.GetContent(_dummyFile)));
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
            Assert.Equal(dummyContent.Split('\n'), resultsArr[0]);
            Assert.Same(resultsArr[0], resultsArr[1]); // Result should get cached after first call
            Assert.Same(resultsArr[1], resultsArr[2]); // The same relation is transitive, so we do not need to check if the item at index 0 is the same as the item at index 2
        }

        [Fact]
        public void GetContent_CachesRemoteContentInFiles()
        {
            // Arrange
            // Arbitrary permalink
            const string url = "https://raw.githubusercontent.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/197d9c30f19945e6629a124760b5aada60cd094e/src/FlexiBlocks/AssemblyInfo.cs";
            IContentRetrievalService testSubject = _serviceProvider.GetRequiredService<IContentRetrievalService>();
            ReadOnlyCollection<string> expectedResult = testSubject.GetContent(url, _fixture.TempDirectory);
            ((IDisposable)_serviceProvider).Dispose(); // Avoid retrieving from in-memory cache

            Mock<IHttpClientService> mockHttpClientService = _mockRepository.Create<IHttpClientService>();
            var services = new ServiceCollection();
            services.AddFlexiBlocks();
            services.AddSingleton(mockHttpClientService.Object);
            _serviceProvider = services.BuildServiceProvider();
            testSubject = _serviceProvider.GetRequiredService<IContentRetrievalService>();
            
            // Act
            ReadOnlyCollection<string> result = testSubject.GetContent(url, _fixture.TempDirectory);

            // Assert
            Assert.Equal(expectedResult, result);
            mockHttpClientService.Verify(h => h.GetAsync(It.IsAny<Uri>(), HttpCompletionOption.ResponseHeadersRead, default(CancellationToken)), Times.Never);
        }

        public void Dispose()
        {
            ((IDisposable)_serviceProvider).Dispose();
        }
    }
}
