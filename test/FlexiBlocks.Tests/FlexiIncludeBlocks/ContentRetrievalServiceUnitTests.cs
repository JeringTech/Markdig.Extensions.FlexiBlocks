using Jering.IocServices.System.Net.Http;
using Jering.IocServices.System.IO;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using System.Collections.Generic;
using Xunit;
using Microsoft.Extensions.Logging;
using System;
using Moq;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Collections.ObjectModel;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Text;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiIncludeBlocks
{
    public class ContentRetrievalServiceUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default);

        [Theory]
        [MemberData(nameof(Constructor_ThrowsArgumentExceptionIfBaseUriIsNotAbsolute_Data))]
        public void Constructor_ThrowsArgumentExceptionIfBaseUriIsNotAbsolute(string dummyBaseUri)
        {
            // Arrange
            var dummyOptions = new ContentRetrievalServiceOptions
            {
                BaseUri = dummyBaseUri
            };
            Mock<IOptions<ContentRetrievalServiceOptions>> mockOptionsAccessor = _mockRepository.Create<IOptions<ContentRetrievalServiceOptions>>();
            mockOptionsAccessor.Setup(o => o.Value).Returns(dummyOptions);

            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => new ContentRetrievalService(null, null, null, null, mockOptionsAccessor.Object));
            Assert.Equal(string.Format(Strings.ArgumentException_BaseUriMustBeAbsolute, dummyBaseUri), result.Message);
        }

        public static IEnumerable<object[]> Constructor_ThrowsArgumentExceptionIfBaseUriIsNotAbsolute_Data()
        {
            return new object[][]
            {
                // Common relative URIs, see http://www.ietf.org/rfc/rfc3986.txt, section 5.4.1
                new object[]{ "./relative/uri" },
                new object[]{ "../relative/uri" },
                new object[]{ "/relative/uri"  },
                new object[]{ "relative/uri"  }
            };
        }

        [Theory]
        [MemberData(nameof(Constructor_ThrowsArgumentExceptionIfBaseUriSchemeIsUnsupported_Data))]
        public void Constructor_ThrowsArgumentExceptionIfBaseUriSchemeIsUnsupported(string dummyBaseUri, string expectedScheme)
        {
            // Arrange
            var dummyOptions = new ContentRetrievalServiceOptions
            {
                BaseUri = dummyBaseUri
            };
            Mock<IOptions<ContentRetrievalServiceOptions>> mockOptionsAccessor = _mockRepository.Create<IOptions<ContentRetrievalServiceOptions>>();
            mockOptionsAccessor.Setup(o => o.Value).Returns(dummyOptions);

            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => new ContentRetrievalService(null, null, null, null, mockOptionsAccessor.Object));
            Assert.Equal(string.Format(Strings.ArgumentException_BaseUriSchemeUnsupported, dummyBaseUri, expectedScheme), result.Message);
        }

        public static IEnumerable<object[]> Constructor_ThrowsArgumentExceptionIfBaseUriSchemeIsUnsupported_Data()
        {
            return new object[][]
            {
                new object[]{ "ftp://base/uri", "ftp" },
                new object[]{ "mailto:base@uri.com", "mailto" },
                new object[]{ "gopher://base.uri.com/", "gopher" }
            };
        }

        [Theory]
        [MemberData(nameof(GetContent_ThrowsArgumentExceptionIfSourceIsNullWhiteSpaceOrAnEmptyString_Data))]
        public void GetContent_ThrowsArgumentExceptionIfSourceIsNullWhiteSpaceOrAnEmptyString(string dummySource)
        {
            // Arrange
            ContentRetrievalService testSubject = CreateContentRetrievalService();

            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => testSubject.GetContent(dummySource));
            Assert.Equal(string.Format(Strings.ArgumentException_CannotBeNullWhiteSpaceOrAnEmptyString, "source"), result.Message);
        }

        public static IEnumerable<object[]> GetContent_ThrowsArgumentExceptionIfSourceIsNullWhiteSpaceOrAnEmptyString_Data()
        {
            return new object[][]
            {
                new object[]{ null },
                new object[]{ " " },
                new object[]{ string.Empty },
            };
        }

        [Fact]
        public void GetContent_RetrievesContentOnlyOnceAndCachesContentInMemory()
        {
            // Arrange
            const string dummySource = "dummySource";
            var dummyContent = new ReadOnlyCollection<string>(new string[0]);
            Mock<ContentRetrievalService> testSubject = CreateMockContentRetrievalService();
            testSubject.CallBase = true;
            testSubject.Setup(t => t.GetContentCore(dummySource, default(CancellationToken))).
                Callback(() => Thread.Sleep(100)).
                Returns(dummyContent);

            // Act
            var threads = new List<Thread>();
            var results = new ConcurrentBag<ReadOnlyCollection<string>>();
            for (int i = 0; i < 3; i++)
            {
                var thread = new Thread(() => results.Add(testSubject.Object.GetContent(dummySource)));
                thread.Start();
                threads.Add(thread);
            }
            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            // Assert
            testSubject.Verify(t => t.GetContent(dummySource, default(CancellationToken)), Times.Exactly(3));
            testSubject.Verify(t => t.GetContentCore(dummySource, default(CancellationToken)), Times.Once); // Lazy should prevent GetContentCore from being called multiple times
            ReadOnlyCollection<string>[] resultsArr = results.ToArray();
            Assert.Equal(3, resultsArr.Length);
            Assert.Same(resultsArr[0], resultsArr[1]); // Result should get cached after first call
            Assert.Same(resultsArr[1], resultsArr[2]); // The same relation is transitive, so we do not need to check if the item at index 0 is the same as the item at index 2
        }

        [Theory]
        [MemberData(nameof(GetContentCore_GetsContentIfUriPointsToAFileOnDisk_Data))]
        public void GetContentCore_GetsContentIfUriPointsToAFileOnDisk(string dummyBaseUri, string dummySource, string expectedAbsolutePath)
        {
            // Arrange
            var dummyContent = new string[] { "dummyContent" };
            var dummyOptions = new ContentRetrievalServiceOptions
            {
                BaseUri = dummyBaseUri
            };
            Mock<IOptions<ContentRetrievalServiceOptions>> mockOptionsAccessor = _mockRepository.Create<IOptions<ContentRetrievalServiceOptions>>();
            mockOptionsAccessor.Setup(o => o.Value).Returns(dummyOptions);
            Mock<IFileService> mockFileService = _mockRepository.Create<IFileService>();
            mockFileService.Setup(f => f.ReadAllLines(expectedAbsolutePath)).Returns(dummyContent); // URI.TryCreate creates a normalized absolute path for the file
            ContentRetrievalService testSubject = CreateContentRetrievalService(fileService: mockFileService.Object, optionsAccessor: mockOptionsAccessor.Object);

            // Act
            ReadOnlyCollection<string> result = testSubject.GetContentCore(dummySource, CancellationToken.None);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyContent, result); // xUnit compares the contents of the collections
        }

        public static IEnumerable<object[]> GetContentCore_GetsContentIfUriPointsToAFileOnDisk_Data()
        {
            return new object[][]
            {
                new object[]{"C:/", "file://C:/path/to/file.txt", "C:/path/to/file.txt"}, // Absolute path with scheme (Unused base URI)
                new object[]{"C:/", "C:/path/to/file.txt", "C:/path/to/file.txt"}, // Absolute path without scheme (Unused base URI)
                new object[]{ "file://C:/", "path/to/file.txt", "C:/path/to/file.txt"}, // Relative path, base URI with scheme
                new object[]{ "C:/", "path/to/file.txt", "C:/path/to/file.txt"}, // Relative path, base URI without scheme
            };
        }

        [Theory]
        [MemberData(nameof(GetContentCore_ThrowsArgumentExceptionIfUriSchemeIsUnsupported_Data))]
        public void GetContentCore_ThrowsArgumentExceptionIfUriSchemeIsUnsupported(string dummyUri, string expectedScheme)
        {
            // Arrange
            ContentRetrievalService testSubject = CreateContentRetrievalService();

            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => testSubject.GetContentCore(dummyUri, CancellationToken.None));
            Assert.Equal(string.Format(Strings.ArgumentException_UriSchemeUnsupported, dummyUri, expectedScheme), result.Message);
        }

        public static IEnumerable<object[]> GetContentCore_ThrowsArgumentExceptionIfUriSchemeIsUnsupported_Data()
        {
            return new object[][]
            {
                new object[]{ "ftp://path/to/file.txt", "ftp" },
                new object[]{ "mailto:user@example.com", "mailto" },
                new object[]{ "gopher://www.example.com/file.txt", "gopher" }
            };
        }

        [Theory]
        [MemberData(nameof(GetContentCore_CallsGetRemoteContentIfUriSchemeIsHttpOrHttps_Data))]
        public void GetContentCore_CallsGetRemoteContentIfUriSchemeIsHttpOrHttps(string dummyBaseUri, string dummySource, string expectedAbsoluteUri)
        {
            // Arrange
            var dummyContent = new ReadOnlyCollection<string>(new string[0]);
            var dummyOptions = new ContentRetrievalServiceOptions
            {
                BaseUri = dummyBaseUri
            };
            Mock<IOptions<ContentRetrievalServiceOptions>> mockOptionsAccessor = _mockRepository.Create<IOptions<ContentRetrievalServiceOptions>>();
            mockOptionsAccessor.Setup(o => o.Value).Returns(dummyOptions);
            Mock<ContentRetrievalService> testSubject = CreateMockContentRetrievalService(optionsAccessor: mockOptionsAccessor.Object);
            testSubject.CallBase = true;
            testSubject.Setup(c => c.GetRemoteContent(It.Is<Uri>(u => u.AbsoluteUri == expectedAbsoluteUri), CancellationToken.None)).Returns(dummyContent);

            // Act
            ReadOnlyCollection<string> result = testSubject.Object.GetContentCore(dummySource, CancellationToken.None);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Same(dummyContent, result); // xUnit compares the contents of the collections
        }

        public static IEnumerable<object[]> GetContentCore_CallsGetRemoteContentIfUriSchemeIsHttpOrHttps_Data()
        {
            return new object[][]
            {
                new object[]{"http://www.jering.tech", "http://www.jering.tech/file.txt", "http://www.jering.tech/file.txt"}, // Absolute HTTP URI (unused base URI)
                new object[]{"http://www.jering.tech", "file.txt", "http://www.jering.tech/file.txt"}, // Relative HTTP URI
                new object[]{"https://www.jering.tech", "https://www.jering.tech/file.txt", "https://www.jering.tech/file.txt"}, // Absolute HTTPS URI (unused base URI)
                new object[]{"https://www.jering.tech", "file.txt", "https://www.jering.tech/file.txt"}, // Relative HTTPS URI
            };
        }

        [Fact]
        public void GetRemoteContent_RetrievesContentFromFileCacheIfContentExistsInFileCache()
        {
            // Arrange
            var dummyContent = new ReadOnlyCollection<string>(new string[0]);
            const string dummyCacheIdentifier = "dummyCacheIdentifier";
            var dummyUri = new Uri("C:/dummy/uri");
            FileStream dummyFileStream = null;
            try
            {
                dummyFileStream = File.Open(Directory.GetCurrentDirectory() + "/dummy.txt", FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
                Mock<IFileCacheService> mockFileCacheService = _mockRepository.Create<IFileCacheService>();
                mockFileCacheService.Setup(f => f.TryGetCacheFile(dummyCacheIdentifier, out dummyFileStream)).Returns(true);
                Mock<ContentRetrievalService> testSubject = CreateMockContentRetrievalService(fileCacheService: mockFileCacheService.Object);
                testSubject.CallBase = true;
                testSubject.Setup(c => c.GetCacheIdentifier(dummyUri.AbsoluteUri)).Returns(dummyCacheIdentifier);
                testSubject.Setup(c => c.ReadAllLines(dummyFileStream)).Returns(dummyContent);

                // Act
                ReadOnlyCollection<string> result = testSubject.Object.GetRemoteContent(dummyUri, default(CancellationToken));

                // Assert
                _mockRepository.VerifyAll();
                Assert.Same(dummyContent, result);
            }
            finally
            {
                dummyFileStream?.Dispose();
            }
        }

        [Fact]
        public void GetRemoteContent_RetrievesContentFromRemoteSource()
        {
            // Arrange
            var dummyContent = new ReadOnlyCollection<string>(new string[0]);
            const string dummyContentString = "dummy\nContent\nString";
            const string dummyCacheIdentifier = "dummyCacheIdentifier";
            var dummyUri = new Uri("C:/dummy/uri");
            FileStream dummyNullFileStream = null; // Moq requires a variable to set up out parameter result values
            Mock<IFileCacheService> mockFileCacheService = _mockRepository.Create<IFileCacheService>();
            mockFileCacheService.Setup(f => f.TryGetCacheFile(dummyCacheIdentifier, out dummyNullFileStream)).Returns(false);
            var dummyHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(dummyContentString)))
            };
            Mock<IHttpClientService> mockHttpClientService = _mockRepository.Create<IHttpClientService>();
            mockHttpClientService.Setup(h => h.GetAsync(dummyUri, HttpCompletionOption.ResponseHeadersRead, default(CancellationToken))).ReturnsAsync(dummyHttpResponseMessage);
            FileStream dummyReadWriteFileStream = null;
            try
            {
                dummyReadWriteFileStream = File.Open(Directory.GetCurrentDirectory() + "/dummyWrite.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.None); // FileMode.Create so stream is empty
                mockFileCacheService.Setup(f => f.CreateOrGetCacheFile(dummyCacheIdentifier)).Returns(dummyReadWriteFileStream);
                Mock<ContentRetrievalService> testSubject = CreateMockContentRetrievalService(mockHttpClientService.Object, fileCacheService: mockFileCacheService.Object);
                testSubject.CallBase = true;
                testSubject.Setup(c => c.GetCacheIdentifier(dummyUri.AbsoluteUri)).Returns(dummyCacheIdentifier);
                testSubject.Setup(c => c.ReadAllLines(dummyReadWriteFileStream)).Returns(dummyContent);

                // Act
                ReadOnlyCollection<string> result = testSubject.Object.GetRemoteContent(dummyUri, default(CancellationToken));

                // Assert
                _mockRepository.VerifyAll();
                Assert.Equal(dummyContent, result);
                using(var streamReader = new StreamReader(dummyReadWriteFileStream))
                {
                    Assert.Equal(dummyContentString, streamReader.ReadToEnd());
                }
            }
            finally
            {
                dummyReadWriteFileStream?.Dispose();
            }
        }

        public void GetRemoteContent_ThrowsContentRetrievalExceptionIfRemoteSourceDoesNotExist()
        {

        }

        public void GetRemoteContent_ThrowsContentRetrievalExceptionIfAccessToRemoteContentIsForbidden()
        {

        }

        public void GetRemoteContent_ThrowsContentRetrievalExceptionIfARemoteSourceCannotBeRetrievedFromAfterThreeAttempts()
        {
            // Use SetupSequence to return different types of exceptions/status codes, test logging
        }

        // TODO get cache identifier
        // TODO read all lines

        private ContentRetrievalService CreateContentRetrievalService(IHttpClientService httpClientService = null,
            IFileService fileService = null,
            IFileCacheService fileCacheService = null,
            ILoggerFactory loggerFactory = null,
            IOptions<ContentRetrievalServiceOptions> optionsAccessor = null)
        {
            return new ContentRetrievalService(httpClientService, fileService, fileCacheService, loggerFactory, optionsAccessor);
        }

        private Mock<ContentRetrievalService> CreateMockContentRetrievalService(IHttpClientService httpClientService = null,
            IFileService fileService = null,
            IFileCacheService fileCacheService = null,
            ILoggerFactory loggerFactory = null,
            IOptions<ContentRetrievalServiceOptions> optionsAccessor = null)
        {
            return _mockRepository.Create<ContentRetrievalService>(httpClientService, fileService, fileCacheService, loggerFactory, optionsAccessor);
        }
    }
}
