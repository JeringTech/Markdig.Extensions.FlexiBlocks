using Jering.IocServices.System.IO;
using Jering.IocServices.System.Net.Http;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiIncludeBlocks
{
    public class ContentRetrieverServiceUnitTests : IClassFixture<ContentRetrieverServiceUnitTestsFixture>
    {
        private readonly string _dummyFile;
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default);

        public ContentRetrieverServiceUnitTests(ContentRetrieverServiceUnitTestsFixture fixture)
        {
            _dummyFile = Path.Combine(fixture.TempDirectory, "dummyFile");
        }

        [Theory]
        [MemberData(nameof(GetContent_ThrowsArgumentExceptionIfSourceIsNullWhiteSpaceOrAnEmptyString_Data))]
        public void GetContent_ThrowsArgumentExceptionIfSourceIsNullWhiteSpaceOrAnEmptyString(string dummySource)
        {
            // Arrange
            ContentRetrieverService testSubject = CreateContentRetrieverService();

            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => testSubject.GetContent(dummySource));
            Assert.Equal(string.Format(Strings.ArgumentException_ValueCannotBeNullWhiteSpaceOrAnEmptyString, "source"), result.Message);
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

        [Theory]
        [MemberData(nameof(GetContent_ThrowsArgumentExceptionIfSourceIsARelativeUriButSourceBaseUriIsNullWhiteSpaceOrAnEmptyString_Data))]
        public void GetContent_ThrowsArgumentExceptionIfSourceIsARelativeUriButSourceBaseUriIsNullWhiteSpaceOrAnEmptyString(string dummySource)
        {
            // Arrange
            ContentRetrieverService testSubject = CreateContentRetrieverService();

            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => testSubject.GetContent(dummySource));
            Assert.Equal(string.Format(Strings.ArgumentException_BaseUriMustBeDefinedIfSourceIsNotAnAbsoluteUri, "sourceBaseUri"), result.Message);
        }

        public static IEnumerable<object[]> GetContent_ThrowsArgumentExceptionIfSourceIsARelativeUriButSourceBaseUriIsNullWhiteSpaceOrAnEmptyString_Data()
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
        [MemberData(nameof(GetContent_ThrowsArgumentExceptionIfSourceBaseUriIsNotAbsolute_Data))]
        public void GetContent_ThrowsArgumentExceptionIfSourceBaseUriIsNotAbsolute(string dummyBaseUri)
        {
            // Arrange
            const string dummySource = "./dummy/source";
            ContentRetrieverService testSubject = CreateContentRetrieverService();

            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => testSubject.GetContent(dummySource, sourceBaseUri: dummyBaseUri));
            Assert.Equal(string.Format(Strings.ArgumentException_BaseUriMustBeAbsolute, dummyBaseUri), result.Message);
        }

        public static IEnumerable<object[]> GetContent_ThrowsArgumentExceptionIfSourceBaseUriIsNotAbsolute_Data()
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
        [MemberData(nameof(GetContent_ThrowsArgumentExceptionIfUriSchemeIsUnsupported_Data))]
        public void GetContent_ThrowsArgumentExceptionIfUriSchemeIsUnsupported(string dummySource, string expectedScheme)
        {
            // Arrange
            ContentRetrieverService testSubject = CreateContentRetrieverService();

            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => testSubject.GetContent(dummySource));
            Assert.Equal(string.Format(Strings.ArgumentException_UriSchemeUnsupported, dummySource, expectedScheme), result.Message);
        }

        public static IEnumerable<object[]> GetContent_ThrowsArgumentExceptionIfUriSchemeIsUnsupported_Data()
        {
            return new object[][]
            {
                    new object[]{ "ftp://base/uri", "ftp" },
                    new object[]{ "mailto:base@uri.com", "mailto" },
                    new object[]{ "gopher://base.uri.com/", "gopher" }
            };
        }

        [Fact]
        public void GetContent_RetrievesContentOnlyOnceAndCachesContentInMemory()
        {
            // Arrange
            const string dummySource = "C:/dummySource";
            var dummyContent = new ReadOnlyCollection<string>(new string[0]);
            Mock<ContentRetrieverService> mockTestSubject = CreateMockContentRetrieverService();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.GetContentCore(It.Is<Uri>(uri => uri.AbsolutePath == dummySource), null, default(CancellationToken))).
                Callback(() => Thread.Sleep(200)). // Arbitrary sleep duration
                Returns(dummyContent);
            // Mock<T>.Object isn't thread safe, if multiple threads call it at the same time, multiple instances are instantiated - https://github.com/moq/moq4/blob/9ca16446b9bbfbe12a78b5f8cad8afaa755c13dc/src/Moq/Mock.Generic.cs#L316
            // If multiple instances are instantiated, multiple _cache instances are created and GetContentCore gets called multiple times.
            ContentRetrieverService testSubject = mockTestSubject.Object;

            // Act
            var threads = new List<Thread>();
            var results = new ConcurrentBag<ReadOnlyCollection<string>>();
            for (int i = 0; i < 3; i++)
            {
                var thread = new Thread(() => results.Add(testSubject.GetContent(dummySource)));
                thread.Start();
                threads.Add(thread);
            }
            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            // Assert
            mockTestSubject.Verify(t => t.GetContent(dummySource, null, null, default(CancellationToken)), Times.Exactly(3));
            mockTestSubject.Verify(t => t.GetContentCore(It.Is<Uri>(uri => uri.AbsolutePath == dummySource), null, default(CancellationToken)), Times.Once); // Lazy should prevent GetContentCore from being called multiple times
            ReadOnlyCollection<string>[] resultsArr = results.ToArray();
            Assert.Equal(3, resultsArr.Length);
            Assert.Same(resultsArr[0], dummyContent);
            Assert.Same(resultsArr[0], resultsArr[1]); // Result should get cached after first call
            Assert.Same(resultsArr[1], resultsArr[2]); // The same relation is transitive, so we do not need to check if the item at index 0 is the same as the item at index 2
        }

        [Theory]
        [MemberData(nameof(GetContent_CallsGetContentCoreWithNormalizedUri_Data))]
        public void GetContent_CallsGetContentCoreWithNormalizedUri(string dummySourceBaseUri, string dummySource, string expectedAbsoluteUri)
        {
            // Arrange
            var dummyContent = new ReadOnlyCollection<string>(new string[0]);
            Mock<ContentRetrieverService> mockTestSubject = CreateMockContentRetrieverService();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.GetContentCore(It.Is<Uri>(uri => uri.AbsoluteUri == expectedAbsoluteUri), null, default(CancellationToken))).Returns(dummyContent);

            // Act
            ReadOnlyCollection<string> result = mockTestSubject.Object.GetContent(dummySource, sourceBaseUri: dummySourceBaseUri, cancellationToken: CancellationToken.None);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyContent, result);
        }

        public static IEnumerable<object[]> GetContent_CallsGetContentCoreWithNormalizedUri_Data()
        {
            return new object[][]
            {
                    new object[]{"C:/", "file://C:/path/to/file.txt", "file:///C:/path/to/file.txt"}, // Absolute path with scheme (Unused base URI)
                    new object[]{"C:/", "C:/path/to/file.txt", "file:///C:/path/to/file.txt"}, // Absolute path without scheme (Unused base URI)
                    new object[]{ "file://C:/", "path/to/file.txt", "file:///C:/path/to/file.txt"}, // Relative path, base URI with scheme
                    new object[]{ "C:/", "path/to/file.txt", "file:///C:/path/to/file.txt"}, // Relative path, base URI without scheme
                    new object[]{"http://www.jering.tech", "http://www.jering.tech/file.txt", "http://www.jering.tech/file.txt"}, // Absolute HTTP URI (unused base URI)
                    new object[]{"http://www.jering.tech", "file.txt", "http://www.jering.tech/file.txt"}, // Relative HTTP URI
                    new object[]{"https://www.jering.tech", "https://www.jering.tech/file.txt", "https://www.jering.tech/file.txt"}, // Absolute HTTPS URI (unused base URI)
                    new object[]{"https://www.jering.tech", "file.txt", "https://www.jering.tech/file.txt"}, // Relative HTTPS URI
            };
        }

        [Fact]
        public void GetContentCore_ReadsContentUsingFileServiceIfSchemeIsFile()
        {
            // Arrange
            var dummyUri = new Uri("C:/dummy/path");
            var dummyContent = new string[] { "dummy", "content" };
            Mock<IFileService> mockFileService = _mockRepository.Create<IFileService>();
            mockFileService.Setup(f => f.ReadAllLines(dummyUri.AbsolutePath)).Returns(dummyContent);
            ContentRetrieverService testSubject = CreateContentRetrieverService(fileService: mockFileService.Object);

            // Act
            ReadOnlyCollection<string> result = testSubject.GetContentCore(dummyUri, null, default(CancellationToken));

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyContent, result);
        }

        [Fact]
        public void GetContentCore_CallsGetsRemoteContentIfSchemeIsNotFile()
        {
            // Arrange
            const string dummyCacheDirectory = "dummyCacheDirectory";
            var dummyUri = new Uri("http://www.dummy.uri");
            var dummyContent = new ReadOnlyCollection<string>(new string[0]);
            Mock<ContentRetrieverService> mockTestSubject = CreateMockContentRetrieverService();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.GetRemoteContent(dummyUri, dummyCacheDirectory, default(CancellationToken))).Returns(dummyContent);

            // Act
            ReadOnlyCollection<string> result = mockTestSubject.Object.GetContentCore(dummyUri, dummyCacheDirectory, default(CancellationToken));

            // Assert
            _mockRepository.VerifyAll();
            Assert.Same(dummyContent, result);
        }

        [Fact]
        public void GetRemoteContent_RetrievesContentFromFileCacheIfCacheDirectoryIsDefinedAndContentExistsInFileCache()
        {
            // Arrange
            var dummyContent = new ReadOnlyCollection<string>(new string[0]);
            const string dummyCacheIdentifier = "dummyCacheIdentifier";
            const string dummyCacheDirectory = "dummyCacheDirectory";
            var dummyUri = new Uri("C:/dummy/uri");
            FileStream dummyFileStream = null;
            try
            {
                dummyFileStream = File.Open(_dummyFile, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
                Mock<IFileCacheService> mockFileCacheService = _mockRepository.Create<IFileCacheService>();
                mockFileCacheService.Setup(f => f.TryGetCacheFile(dummyCacheIdentifier, dummyCacheDirectory)).Returns((true, dummyFileStream));
                Mock<ContentRetrieverService> mockTestSubject = CreateMockContentRetrieverService(fileCacheService: mockFileCacheService.Object);
                mockTestSubject.CallBase = true;
                mockTestSubject.Setup(c => c.GetCacheIdentifier(dummyUri.AbsoluteUri)).Returns(dummyCacheIdentifier);
                mockTestSubject.Setup(c => c.ReadAllLines(dummyFileStream)).Returns(dummyContent);

                // Act
                ReadOnlyCollection<string> result = mockTestSubject.Object.GetRemoteContent(dummyUri, dummyCacheDirectory, default(CancellationToken));

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
        public void GetRemoteContent_RetrievesContentFromRemoteSourceIfCacheDirectoryIsDefinedButContentDoesNotExistInFileCache()
        {
            // Arrange
            const string dummyCacheIdentifier = "dummyCacheIdentifier";
            const string dummyCacheDirectory = "dummyCacheDirectory";
            const string dummyContentString = "dummy\nContent\nString";
            Mock<IFileCacheService> mockFileCacheService = _mockRepository.Create<IFileCacheService>();
            mockFileCacheService.Setup(f => f.TryGetCacheFile(dummyCacheIdentifier, dummyCacheDirectory)).Returns((false, null));
            FileStream dummyFileStream = null;
            try
            {
                var dummyContentStream = new MemoryStream(Encoding.UTF8.GetBytes(dummyContentString));
                var dummyHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(dummyContentStream)
                };
                var dummyUri = new Uri("C:/dummy/uri");
                Mock<IHttpClientService> mockHttpClientService = _mockRepository.Create<IHttpClientService>();
                mockHttpClientService.Setup(h => h.GetAsync(dummyUri, HttpCompletionOption.ResponseHeadersRead, default(CancellationToken))).ReturnsAsync(dummyHttpResponseMessage);
                dummyFileStream = File.Open(_dummyFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                mockFileCacheService.Setup(f => f.CreateOrGetCacheFile(dummyCacheIdentifier, dummyCacheDirectory)).Returns(dummyFileStream);
                Mock<ContentRetrieverService> mockTestSubject = CreateMockContentRetrieverService(mockHttpClientService.Object, fileCacheService: mockFileCacheService.Object);
                mockTestSubject.CallBase = true;
                mockTestSubject.Setup(c => c.GetCacheIdentifier(dummyUri.AbsoluteUri)).Returns(dummyCacheIdentifier);

                // Act
                ReadOnlyCollection<string> result = mockTestSubject.Object.GetRemoteContent(dummyUri, dummyCacheDirectory, default(CancellationToken));

                // Assert
                _mockRepository.VerifyAll();
                Assert.Equal(dummyContentString.Split('\n'), result);
            }
            finally
            {
                dummyFileStream?.Dispose();
            }
        }

        [Fact]
        public void GetRemoteContent_RetrievesContentFromRemoteSourceIfCacheDirectoryIsNotDefined()
        {
            // Arrange
            const string dummyContentString = "dummy\nContent\nString";
            var dummyUri = new Uri("C:/dummy/uri");
            var dummyContentStream = new MemoryStream(Encoding.UTF8.GetBytes(dummyContentString));
            var dummyHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(dummyContentStream)
            };
            Mock<IHttpClientService> mockHttpClientService = _mockRepository.Create<IHttpClientService>();
            mockHttpClientService.Setup(h => h.GetAsync(dummyUri, HttpCompletionOption.ResponseHeadersRead, default(CancellationToken))).ReturnsAsync(dummyHttpResponseMessage);
            ContentRetrieverService testSubject = CreateContentRetrieverService(mockHttpClientService.Object);

            // Act
            ReadOnlyCollection<string> result = testSubject.GetRemoteContent(dummyUri, null, default(CancellationToken));

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyContentString.Split('\n'), result);
        }

        [Fact]
        public void GetRemoteContent_ThrowsContentRetrieverExceptionIfRemoteSourceDoesNotExist()
        {
            // Arrange
            var dummyUri = new Uri("C:/dummy/uri");
            var dummyHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
            Mock<IHttpClientService> mockHttpClientService = _mockRepository.Create<IHttpClientService>();
            mockHttpClientService.Setup(h => h.GetAsync(dummyUri, HttpCompletionOption.ResponseHeadersRead, default(CancellationToken))).ReturnsAsync(dummyHttpResponseMessage);
            ContentRetrieverService testSubject = CreateContentRetrieverService(mockHttpClientService.Object);

            // Act and assert
            ContentRetrieverException result = Assert.Throws<ContentRetrieverException>(() => testSubject.GetRemoteContent(dummyUri, null, default(CancellationToken)));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.ContentRetrieverException_RemoteUriDoesNotExist, dummyUri.AbsoluteUri), result.Message);
        }

        [Fact]
        public void GetRemoteContent_ThrowsContentRetrieverExceptionIfAccessToRemoteContentIsForbidden()
        {
            // Arrange
            var dummyUri = new Uri("C:/dummy/uri");
            var dummyHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.Forbidden);
            Mock<IHttpClientService> mockHttpClientService = _mockRepository.Create<IHttpClientService>();
            mockHttpClientService.Setup(h => h.GetAsync(dummyUri, HttpCompletionOption.ResponseHeadersRead, default(CancellationToken))).ReturnsAsync(dummyHttpResponseMessage);
            ContentRetrieverService testSubject = CreateContentRetrieverService(mockHttpClientService.Object);

            // Act and assert
            ContentRetrieverException result = Assert.Throws<ContentRetrieverException>(() => testSubject.GetRemoteContent(dummyUri, null, default(CancellationToken)));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.ContentRetrieverException_RemoteUriAccessForbidden, dummyUri.AbsoluteUri), result.Message);
        }

        [Fact]
        public void GetRemoteContent_ThrowsContentRetrieverExceptionIfARemoteSourceCannotBeRetrievedFromAfterThreeAttempts()
        {
            // Arrange
            var dummyUri = new Uri("C:/dummy/uri");
            var dummyHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            Mock<IHttpClientService> mockHttpClientService = _mockRepository.Create<IHttpClientService>();
            mockHttpClientService.
                SetupSequence(h => h.GetAsync(dummyUri, HttpCompletionOption.ResponseHeadersRead, default(CancellationToken))).
                ReturnsAsync(dummyHttpResponseMessage).
                ThrowsAsync(new OperationCanceledException()).
                ThrowsAsync(new HttpRequestException());
            ContentRetrieverService testSubject = CreateContentRetrieverService(mockHttpClientService.Object);

            // Act and assert
            ContentRetrieverException result = Assert.Throws<ContentRetrieverException>(() => testSubject.GetRemoteContent(dummyUri, null, default(CancellationToken)));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.ContentRetrieverException_FailedAfterMultipleAttempts, dummyUri.AbsoluteUri), result.Message);
        }

        [Fact]
        public void GetCacheIdentifier_GetsIdentifier()
        {
            // Arrange
            const string dummyAbsoluteUri = "file://C:/dummy/absolute/path";
            ContentRetrieverService testSubject = CreateContentRetrieverService();

            // Act
            string result = testSubject.GetCacheIdentifier(dummyAbsoluteUri);

            // Assert
            Assert.Equal("B09E67B0F1899D8BB5C8D1F087DDC9CF", result);
        }

        [Fact]
        public void ReadAllLines_ReadsAllLines()
        {
            // Arrange
            const string dummyLines = "these\nare\ndummy\nlines";
            var dummyMemoryStream = new MemoryStream(Encoding.UTF8.GetBytes(dummyLines));
            ContentRetrieverService testSubject = CreateContentRetrieverService();

            // Act
            ReadOnlyCollection<string> result = testSubject.ReadAllLines(dummyMemoryStream);

            // Assert
            Assert.Equal(dummyLines.Split('\n'), result);
        }

        private ContentRetrieverService CreateContentRetrieverService(IHttpClientService httpClientService = null,
            IFileService fileService = null,
            IFileCacheService fileCacheService = null,
            ILoggerFactory loggerFactory = null)
        {
            return new ContentRetrieverService(httpClientService, fileService, fileCacheService, loggerFactory);
        }

        private Mock<ContentRetrieverService> CreateMockContentRetrieverService(IHttpClientService httpClientService = null,
            IFileService fileService = null,
            IFileCacheService fileCacheService = null,
            ILoggerFactory loggerFactory = null)
        {
            return _mockRepository.Create<ContentRetrieverService>(httpClientService, fileService, fileCacheService, loggerFactory);
        }
    }
}
