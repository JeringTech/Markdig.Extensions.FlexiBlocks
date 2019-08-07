using Jering.IocServices.System.IO;
using Jering.IocServices.System.Net.Http;
using Jering.Markdig.Extensions.FlexiBlocks.IncludeBlocks;
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

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.IncludeBlocks
{
    public class ContentRetrieverServiceUnitTests : IClassFixture<ContentRetrieverServiceUnitTestsFixture>
    {
        private readonly string _dummyFile;
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        public ContentRetrieverServiceUnitTests(ContentRetrieverServiceUnitTestsFixture fixture)
        {
            _dummyFile = Path.Combine(fixture.TempDirectory, "dummyFile");
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfHttpClientServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new ContentRetrieverService(
                null,
                _mockRepository.Create<IFileService>().Object,
                _mockRepository.Create<IDiskCacheService>().Object,
                _mockRepository.Create<ILoggerFactory>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFileServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new ContentRetrieverService(
                _mockRepository.Create<IHttpClientService>().Object,
                null,
                _mockRepository.Create<IDiskCacheService>().Object,
                _mockRepository.Create<ILoggerFactory>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfDiskCacheServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new ContentRetrieverService(
                _mockRepository.Create<IHttpClientService>().Object,
                _mockRepository.Create<IFileService>().Object,
                null,
                _mockRepository.Create<ILoggerFactory>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfLoggerFactoryIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new ContentRetrieverService(
                _mockRepository.Create<IHttpClientService>().Object,
                _mockRepository.Create<IFileService>().Object,
                _mockRepository.Create<IDiskCacheService>().Object,
                null));
        }

        [Fact]
        public void GetContent_ThrowsArgumentNullExceptionIfSourceIsNull()
        {
            // Arrange
            ContentRetrieverService testSubject = CreateContentRetrieverService();

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => testSubject.GetContent(null));
        }

        [Fact]
        public void GetContent_RetrievesContentOnlyOnceAndCachesContentInMemory()
        {
            // Arrange
            var dummySource = new Uri("C:/dummySource");
            var dummyContent = new ReadOnlyCollection<string>(new string[0]);
            const string dummyCacheDirectory = "dummyCacheDirectory";
            var dummyCancellationToken = new CancellationToken();
            Mock<ContentRetrieverService> mockTestSubject = CreateMockContentRetrieverService();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.GetContentCore(dummySource, dummyCacheDirectory, dummyCancellationToken)).
                Callback(() => Thread.Sleep(200)). // Arbitrary sleep duration
                Returns(dummyContent);
            // Mock<T>.Object isn't thread safe, if multiple threads call it at the same time, multiple instances are instantiated - https://github.com/moq/moq4/blob/9ca16446b9bbfbe12a78b5f8cad8afaa755c13dc/src/Moq/Mock.Generic.cs#L316
            // If multiple instances are instantiated, multiple _cache instances are created and GetContentCore gets called multiple times.
            ContentRetrieverService testSubject = mockTestSubject.Object;

            // Act
            var threads = new List<Thread>();
            var results = new ConcurrentBag<ReadOnlyCollection<string>>();
            for (int i = 0; i < 3; i++) // Arbitrary number of threads
            {
                var thread = new Thread(() => results.Add(testSubject.GetContent(dummySource, dummyCacheDirectory, dummyCancellationToken)));
                thread.Start();
                threads.Add(thread);
            }
            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            // Assert
            mockTestSubject.Verify(t => t.GetContent(dummySource, dummyCacheDirectory, dummyCancellationToken), Times.Exactly(3));
            mockTestSubject.Verify(t => t.GetContentCore(dummySource, dummyCacheDirectory, dummyCancellationToken), Times.Once); // Lazy should prevent GetContentCore from being called multiple times
            ReadOnlyCollection<string>[] resultsArr = results.ToArray();
            Assert.Equal(3, resultsArr.Length);
            Assert.Same(resultsArr[0], dummyContent);
            Assert.Same(resultsArr[0], resultsArr[1]); // Result should get cached after first call
            Assert.Same(resultsArr[1], resultsArr[2]); // Same relation is transitive
        }

        [Fact]
        public void GetContent_PropagatesExceptionsThrownByGetContentCore()
        {
            // Arrange
            var dummySource = new Uri("C:/dummySource");
            const string dummyCacheDirectory = "dummyCacheDirectory";
            var dummyCancellationToken = new CancellationToken();
            var dummyException = new Exception();
            Mock<ContentRetrieverService> mockTestSubject = CreateMockContentRetrieverService();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.GetContentCore(dummySource, dummyCacheDirectory, dummyCancellationToken)).Throws(dummyException);

            // Act and assert
            Exception result = Assert.Throws<Exception>(() => mockTestSubject.Object.GetContent(dummySource, dummyCacheDirectory, dummyCancellationToken));
            Assert.Same(dummyException, result);
        }

        [Fact]
        public void GetContentCore_GetsContentFromLocalSourceIfSchemeIsFile()
        {
            // Arrange
            var dummySource = new Uri("C:/dummy/path");
            var dummyContent = new ReadOnlyCollection<string>(new string[0]);
            Mock<ContentRetrieverService> mockTestSubject = CreateMockContentRetrieverService();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.GetContentFromLocalSource(dummySource)).Returns(dummyContent);

            // Act
            ReadOnlyCollection<string> result = mockTestSubject.Object.GetContentCore(dummySource, default, default);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Same(dummyContent, result);
        }

        [Fact]
        public void GetContentCore_GetsContentFromOnDiskCacheIfSchemeIsHttpOrHttpsAndContentHasBeenCachedOnDisk()
        {
            // Arrange
            const string dummyCacheDirectory = "dummyCacheDirectory";
            var dummySource = new Uri("http://www.dummy.uri");
            var dummyContent = new ReadOnlyCollection<string>(new string[0]);
            Mock<ContentRetrieverService> mockTestSubject = CreateMockContentRetrieverService();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.TryGetContentFromDiskCache(dummySource, dummyCacheDirectory)).Returns(dummyContent);

            // Act
            ReadOnlyCollection<string> result = mockTestSubject.Object.GetContentCore(dummySource, dummyCacheDirectory, default);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Same(dummyContent, result);
        }

        [Theory]
        [MemberData(nameof(GetContentCore_GetsContentFromRemoteSourceIfSchemeIsHttpOrHttpsAndContentHasNotBeenCachedOnDisk_Data))]
        public void GetContentCore_GetsContentFromRemoteSourceIfSchemeIsHttpOrHttpsAndContentHasNotBeenCachedOnDisk(string dummyUri)
        {
            // Arrange
            const string dummyCacheDirectory = "dummyCacheDirectory";
            var dummyCancellationToken = new CancellationToken();
            var dummySource = new Uri(dummyUri);
            var dummyContent = new ReadOnlyCollection<string>(new string[0]);
            Mock<ContentRetrieverService> mockTestSubject = CreateMockContentRetrieverService();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.TryGetContentFromDiskCache(dummySource, dummyCacheDirectory)).Returns((ReadOnlyCollection<string>)null);
            mockTestSubject.Setup(t => t.GetContentFromRemoteSource(dummySource, dummyCacheDirectory, dummyCancellationToken)).Returns(dummyContent);

            // Act
            ReadOnlyCollection<string> result = mockTestSubject.Object.GetContentCore(dummySource, dummyCacheDirectory, dummyCancellationToken);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Same(dummyContent, result);
        }

        public static IEnumerable<object[]> GetContentCore_GetsContentFromRemoteSourceIfSchemeIsHttpOrHttpsAndContentHasNotBeenCachedOnDisk_Data()
        {
            return new object[][]
            {
                new object[]{ "http://www.dummy.uri" },
                new object[]{ "https://www.dummy.uri" },
            };
        }

        [Fact]
        public void GetContentCore_ThrowsArgumentExceptionIfSourceSchemeIsUnsupported()
        {
            // Arrange
            const string dummyScheme = "ftp";
            var dummySource = new Uri($"{dummyScheme}://example@test.com");
            ContentRetrieverService testSubject = CreateContentRetrieverService();

            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => testSubject.GetContentCore(dummySource, default, default));
            Assert.Equal($@"{string.Format(Strings.ArgumentException_ContentRetrieverService_UnsupportedScheme, dummySource.AbsoluteUri, dummyScheme)}
Parameter name: source",
                result.Message,
                ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void GetContentFromLocalSource_GetsContentUsingFileService()
        {
            // Arrange
            var dummySource = new Uri("C:/dummy/path");
            var dummyContent = new ReadOnlyCollection<string>(new string[0]);
            Mock<IFileService> mockFileService = _mockRepository.Create<IFileService>();
            FileStream dummyFileStream = null;
            try
            {
                dummyFileStream = File.Open(_dummyFile, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
                mockFileService.Setup(f => f.Open(dummySource.AbsolutePath, FileMode.Open, FileAccess.Read, FileShare.Read)).Returns(dummyFileStream);
                Mock<ContentRetrieverService> mockTestSubject = CreateMockContentRetrieverService(fileService: mockFileService.Object);
                mockTestSubject.CallBase = true;
                mockTestSubject.Setup(c => c.ReadAndNormalizeAllLines(dummyFileStream)).Returns(dummyContent);

                // Act
                ReadOnlyCollection<string> result = mockTestSubject.Object.GetContentFromLocalSource(dummySource);

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
        public void GetContentFromLocalSource_ThrowsArgumentExceptionIfSourceIsInvalid()
        {
            // Arrange
            var dummySource = new Uri("C:/dummy/path");
            var dummyException = new IOException();
            Mock<IFileService> mockFileService = _mockRepository.Create<IFileService>();
            mockFileService.Setup(f => f.Open(dummySource.AbsolutePath, FileMode.Open, FileAccess.Read, FileShare.Read)).Throws(dummyException);
            ContentRetrieverService testSubject = CreateContentRetrieverService(fileService: mockFileService.Object);

            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => testSubject.GetContentFromLocalSource(dummySource));
            _mockRepository.VerifyAll();
            Assert.Equal($@"{string.Format(Strings.ArgumentException_ContentRetrieverService_InvalidLocalUri, dummySource.AbsolutePath)}
Parameter name: source",
                result.Message,
                ignoreLineEndingDifferences: true);
        }

        [Theory]
        [MemberData(nameof(TryGetContentFromDiskCache_ReturnsNullIfCacheDirectoryIsNullWhitespaceOrAnEmptyString_Data))]
        public void TryGetContentFromDiskCache_ReturnsNullIfCacheDirectoryIsNullWhitespaceOrAnEmptyString(string dummyCacheDirectory)
        {
            // Arrange
            ContentRetrieverService testSubject = CreateContentRetrieverService();

            // Act
            ReadOnlyCollection<string> result = testSubject.TryGetContentFromDiskCache(null, dummyCacheDirectory);

            // Assert
            Assert.Null(result);
        }

        public static IEnumerable<object[]> TryGetContentFromDiskCache_ReturnsNullIfCacheDirectoryIsNullWhitespaceOrAnEmptyString_Data()
        {
            return new object[][]
            {
                new object[]{null},
                new object[]{" "},
                new object[]{string.Empty}
            };
        }

        [Fact]
        public void TryGetContentFromDiskCache_GetsContentFromDiskCacheIfCacheDirectoryIsNotNullWhitespaceOrAnEmptyStringAndContentExistsInDiskCache()
        {
            // Arrange
            var dummyContent = new ReadOnlyCollection<string>(new string[0]);
            const string dummyCacheDirectory = "dummyCacheDirectory";
            var dummySource = new Uri("C:/dummy/uri");
            FileStream dummyFileStream = null;
            try
            {
                dummyFileStream = File.Open(_dummyFile, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
                Mock<IDiskCacheService> mockDiskCacheService = _mockRepository.Create<IDiskCacheService>();
                mockDiskCacheService.Setup(f => f.TryGetCacheFile(dummySource.AbsoluteUri, dummyCacheDirectory)).Returns(dummyFileStream);
                Mock<ContentRetrieverService> mockTestSubject = CreateMockContentRetrieverService(diskCacheService: mockDiskCacheService.Object);
                mockTestSubject.CallBase = true;
                mockTestSubject.Setup(c => c.ReadAndNormalizeAllLines(dummyFileStream)).Returns(dummyContent);

                // Act
                ReadOnlyCollection<string> result = mockTestSubject.Object.TryGetContentFromDiskCache(dummySource, dummyCacheDirectory);

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
        public void TryGetContentFromDiskCache_ReturnsNullIfCacheDirectoryIsNotNullWhitespaceOrAnEmptyStringButContentDoesNotExistInDiskCache()
        {
            // Arrange
            const string dummyCacheDirectory = "dummyCacheDirectory";
            var dummySource = new Uri("C:/dummy/uri");
            Mock<IDiskCacheService> mockDiskCacheService = _mockRepository.Create<IDiskCacheService>();
            mockDiskCacheService.Setup(f => f.TryGetCacheFile(dummySource.AbsoluteUri, dummyCacheDirectory)).Returns((FileStream)null);
            ContentRetrieverService testSubject = CreateContentRetrieverService(diskCacheService: mockDiskCacheService.Object);

            // Act
            ReadOnlyCollection<string> result = testSubject.TryGetContentFromDiskCache(dummySource, dummyCacheDirectory);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Null(result);
        }

        [Theory]
        [MemberData(nameof(GetContentFromRemoteSource_RetrievesContentFromRemoteSourceButDoesNotCacheContentIfCacheDirectoryIsNullWhitespaceOrAnEmptyString_Data))]
        public void GetContentFromRemoteSource_RetrievesContentFromRemoteSourceButDoesNotCacheContentIfCacheDirectoryIsNullWhitespaceOrAnEmptyString(string dummyCacheDirectory)
        {
            // Arrange
            var dummyCancellationToken = new CancellationToken();
            var dummySource = new Uri("C:/dummy/uri");
            var dummyContent = new ReadOnlyCollection<string>(new string[0]);
            var dummyHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(new MemoryStream())
            };
            Mock<IHttpClientService> mockHttpClientService = _mockRepository.Create<IHttpClientService>();
            mockHttpClientService.Setup(h => h.GetAsync(dummySource, HttpCompletionOption.ResponseHeadersRead, dummyCancellationToken)).ReturnsAsync(dummyHttpResponseMessage);
            Mock<ContentRetrieverService> mockTestSubject = CreateMockContentRetrieverService(mockHttpClientService.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.ReadAndNormalizeAllLines(It.IsAny<Stream>())).Returns(dummyContent);

            // Act
            ReadOnlyCollection<string> result = mockTestSubject.Object.GetContentFromRemoteSource(dummySource, dummyCacheDirectory, dummyCancellationToken);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Same(dummyContent, result);
        }

        public static IEnumerable<object[]> GetContentFromRemoteSource_RetrievesContentFromRemoteSourceButDoesNotCacheContentIfCacheDirectoryIsNullWhitespaceOrAnEmptyString_Data()
        {
            return new object[][]
            {
                new object[]{null},
                new object[]{" "},
                new object[]{string.Empty}
            };
        }

        [Fact]
        public void GetContentFromRemoteSource_RetrievesContentFromRemoteSourceAndCachesContentIfCacheDirectoryIsNotNullWhitespaceOrAnEmptyStringAndContentDoesNotExistInDiskCache()
        {
            // Arrange
            const string dummyCacheDirectory = "dummyCacheDirectory";
            const string dummyContent = "dummyContent";
            var dummyCancellationToken = new CancellationToken();
            var dummySource = new Uri("C:/dummy/uri");
            var dummyContentLines = new ReadOnlyCollection<string>(new string[0]);
            var dummyHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(dummyContent)))
            };
            Mock<IHttpClientService> mockHttpClientService = _mockRepository.Create<IHttpClientService>();
            mockHttpClientService.Setup(h => h.GetAsync(dummySource, HttpCompletionOption.ResponseHeadersRead, dummyCancellationToken)).ReturnsAsync(dummyHttpResponseMessage);
            FileStream dummyFileStream = null;
            try
            {
                File.WriteAllText(_dummyFile, ""); // Empty file
                dummyFileStream = File.Open(_dummyFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
                Mock<IDiskCacheService> mockDiskCacheService = _mockRepository.Create<IDiskCacheService>();
                mockDiskCacheService.Setup(d => d.CreateOrGetCacheFile(dummySource.AbsoluteUri, dummyCacheDirectory)).Returns(dummyFileStream);
                Mock<ContentRetrieverService> mockTestSubject = CreateMockContentRetrieverService(mockHttpClientService.Object, diskCacheService: mockDiskCacheService.Object);
                mockTestSubject.CallBase = true;
                mockTestSubject.
                    Setup(t => t.ReadAndNormalizeAllLines(It.Is<Stream>(stream => stream.Position == 0 && dummyContent == new StreamReader(stream).ReadToEnd()))).
                    Returns(dummyContentLines);

                // Act
                ReadOnlyCollection<string> result = mockTestSubject.Object.GetContentFromRemoteSource(dummySource, dummyCacheDirectory, dummyCancellationToken);

                // Assert
                _mockRepository.VerifyAll();
                Assert.Same(dummyContentLines, result);
            }
            finally
            {
                dummyFileStream?.Dispose();
            }
        }

        [Fact]
        public void GetContentFromRemoteSource_RetrievesContentFromRemoteSourceButDoesNotCacheContentIfContentAlreadyExistsInDiskCache()
        {
            // Arrange
            const string dummyCacheDirectory = "dummyCacheDirectory";
            const string dummyContent = "dummyContent";
            var dummyCancellationToken = new CancellationToken();
            var dummySource = new Uri("C:/dummy/uri");
            var dummyContentLines = new ReadOnlyCollection<string>(new string[0]);
            var dummyHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(dummyContent)))
            };
            Mock<IHttpClientService> mockHttpClientService = _mockRepository.Create<IHttpClientService>();
            mockHttpClientService.Setup(h => h.GetAsync(dummySource, HttpCompletionOption.ResponseHeadersRead, dummyCancellationToken)).ReturnsAsync(dummyHttpResponseMessage);
            FileStream dummyFileStream = null;
            try
            {
                File.WriteAllText(_dummyFile, dummyContent); // Overwrite any existing text
                dummyFileStream = File.Open(_dummyFile, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
                Mock<IDiskCacheService> mockDiskCacheService = _mockRepository.Create<IDiskCacheService>();
                mockDiskCacheService.Setup(d => d.CreateOrGetCacheFile(dummySource.AbsoluteUri, dummyCacheDirectory)).Returns(dummyFileStream);
                Mock<ContentRetrieverService> mockTestSubject = CreateMockContentRetrieverService(mockHttpClientService.Object, diskCacheService: mockDiskCacheService.Object);
                mockTestSubject.CallBase = true;
                mockTestSubject.Setup(t => t.ReadAndNormalizeAllLines(It.Is<Stream>(stream => dummyContent == new StreamReader(stream).ReadToEnd()))).Returns(dummyContentLines);

                // Act
                ReadOnlyCollection<string> result = mockTestSubject.Object.GetContentFromRemoteSource(dummySource, dummyCacheDirectory, dummyCancellationToken);

                // Assert
                _mockRepository.VerifyAll();
                Assert.Same(dummyContentLines, result);
            }
            finally
            {
                dummyFileStream?.Dispose();
            }
        }

        [Fact]
        public void GetContentFromRemoteSource_ThrowsArgumentExceptionIfRemoteSourceDoesNotExist()
        {
            // Arrange
            var dummySource = new Uri("C:/dummy/uri");
            var dummyCancellationToken = new CancellationToken();
            var dummyHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
            Mock<IHttpClientService> mockHttpClientService = _mockRepository.Create<IHttpClientService>();
            mockHttpClientService.Setup(h => h.GetAsync(dummySource, HttpCompletionOption.ResponseHeadersRead, dummyCancellationToken)).ReturnsAsync(dummyHttpResponseMessage);
            ContentRetrieverService testSubject = CreateContentRetrieverService(mockHttpClientService.Object);

            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => testSubject.GetContentFromRemoteSource(dummySource, null, dummyCancellationToken));
            _mockRepository.VerifyAll();
            Assert.Equal($@"{string.Format(Strings.ArgumentException_ContentRetrieverService_RemoteUriDoesNotExist, dummySource.AbsoluteUri)}
Parameter name: source",
                result.Message, ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void GetContentFromRemoteSource_ThrowsArgumentExceptionIfAccessToRemoteSourceIsForbidden()
        {
            // Arrange
            var dummySource = new Uri("C:/dummy/uri");
            var dummyCancellationToken = new CancellationToken();
            var dummyHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.Forbidden);
            Mock<IHttpClientService> mockHttpClientService = _mockRepository.Create<IHttpClientService>();
            mockHttpClientService.Setup(h => h.GetAsync(dummySource, HttpCompletionOption.ResponseHeadersRead, dummyCancellationToken)).ReturnsAsync(dummyHttpResponseMessage);
            ContentRetrieverService testSubject = CreateContentRetrieverService(mockHttpClientService.Object);

            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => testSubject.GetContentFromRemoteSource(dummySource, null, dummyCancellationToken));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format($@"{Strings.ArgumentException_ContentRetrieverService_RemoteUriAccessForbidden}
Parameter name: source", dummySource.AbsoluteUri),
                result.Message, ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void GetContentFromRemoteSource_LogsWarningsThenThrowsArgumentExceptionIfARemoteSourceCannotBeRetrievedFromAfterThreeAttempts()
        {
            // Arrange
            var dummySource = new Uri("C:/dummy/uri");
            var dummyCancellationToken = new CancellationToken();
            var dummyHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError); // Arbitrary status code that might represent an intermittent issue
            Mock<ILogger> mockLogger = _mockRepository.Create<ILogger>();
            mockLogger.Setup(l => l.IsEnabled(LogLevel.Warning)).Returns(true);
            Mock<ILoggerFactory> mockLoggerFactory = _mockRepository.Create<ILoggerFactory>();
            mockLoggerFactory.Setup(l => l.CreateLogger(typeof(ContentRetrieverService).FullName)).Returns(mockLogger.Object);
            var dummyHttpRequestException = new HttpRequestException();
            Mock<IHttpClientService> mockHttpClientService = _mockRepository.Create<IHttpClientService>();
            mockHttpClientService.
                SetupSequence(h => h.GetAsync(dummySource, HttpCompletionOption.ResponseHeadersRead, dummyCancellationToken)).
                ReturnsAsync(dummyHttpResponseMessage).
                ThrowsAsync(new OperationCanceledException()).
                ThrowsAsync(dummyHttpRequestException);
            ContentRetrieverService testSubject = CreateContentRetrieverService(mockHttpClientService.Object, loggerFactory: mockLoggerFactory.Object);

            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => testSubject.GetContentFromRemoteSource(dummySource, null, dummyCancellationToken));
            _mockRepository.VerifyAll();
            mockLogger.Verify(l => l.Log(LogLevel.Warning, 0,
                // object is of type FormattedLogValues
                It.Is<object>(f => f.ToString() == string.Format(Strings.LogWarning_ContentRetrieverService_FailureStatusCode, dummySource.AbsoluteUri, (int)HttpStatusCode.InternalServerError, 2)),
                null, It.IsAny<Func<object, Exception, string>>()), Times.Once);
            mockLogger.Verify(l => l.Log(LogLevel.Warning, 0,
                // object is of type FormattedLogValues
                It.Is<object>(f => f.ToString() == string.Format(Strings.LogWarning_ContentRetrieverService_Timeout, dummySource.AbsoluteUri, 1)),
                null, It.IsAny<Func<object, Exception, string>>()), Times.Once);
            mockLogger.Verify(l => l.Log(LogLevel.Warning, 0,
                // object is of type FormattedLogValues
                It.Is<object>(f => f.ToString() == string.Format(Strings.LogWarning_ContentRetrieverService_HttpRequestException, dummyHttpRequestException.Message, dummySource.AbsoluteUri, 0)),
                null, It.IsAny<Func<object, Exception, string>>()), Times.Once);
            Assert.Equal($@"{string.Format(Strings.ArgumentException_ContentRetrieverService_FailedAfterMultipleAttempts, dummySource.AbsoluteUri)}
Parameter name: source",
                result.Message, ignoreLineEndingDifferences: true);
        }

        [Theory]
        [MemberData(nameof(ReadAndNormalizeLines_ReadsAndNormalizesAllLines_Data))]
        public void ReadAndNormalizeLines_ReadsAndNormalizesAllLines(string dummyLines, string[] expectedResult)
        {
            // Arrange
            var dummyMemoryStream = new MemoryStream(Encoding.UTF8.GetBytes(dummyLines));
            ContentRetrieverService testSubject = CreateContentRetrieverService();

            // Act
            ReadOnlyCollection<string> result = testSubject.ReadAndNormalizeAllLines(dummyMemoryStream);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ReadAndNormalizeLines_ReadsAndNormalizesAllLines_Data()
        {
            return new object[][]
            {
                    new object[]
                    {
                        "these\nare\ndummy\nlines",
                        new string[]
                        {
                            "these",
                            "are",
                            "dummy",
                            "lines"
                        }
                    },
                    // Replaces null characters
                    new object[]
                    {
                        "these\0\nare\ndu\0mmy\nlines",
                        new string[]
                        {
                            "these\uFFFD",
                            "are",
                            "du\uFFFDmmy",
                            "lines"
                        }
                    }
            };
        }

        private Mock<ContentRetrieverService> CreateMockContentRetrieverService(IHttpClientService httpClientService = null,
            IFileService fileService = null,
            IDiskCacheService diskCacheService = null,
            ILoggerFactory loggerFactory = null)
        {
            return _mockRepository.Create<ContentRetrieverService>(httpClientService ?? _mockRepository.Create<IHttpClientService>().Object,
                fileService ?? _mockRepository.Create<IFileService>().Object,
                diskCacheService ?? _mockRepository.Create<IDiskCacheService>().Object,
                loggerFactory ?? _mockRepository.Create<ILoggerFactory>().Object);
        }

        private ContentRetrieverService CreateContentRetrieverService(IHttpClientService httpClientService = null,
            IFileService fileService = null,
            IDiskCacheService diskCacheService = null,
            ILoggerFactory loggerFactory = null)
        {
            return new ContentRetrieverService(httpClientService ?? _mockRepository.Create<IHttpClientService>().Object,
                fileService ?? _mockRepository.Create<IFileService>().Object,
                diskCacheService ?? _mockRepository.Create<IDiskCacheService>().Object,
                loggerFactory ?? _mockRepository.Create<ILoggerFactory>().Object);
        }
    }

    public class ContentRetrieverServiceUnitTestsFixture : IDisposable
    {
        public string TempDirectory { get; } = Path.Combine(Path.GetTempPath(), nameof(ContentRetrieverServiceUnitTests)); // Dummy file for creating dummy file streams

        public ContentRetrieverServiceUnitTestsFixture()
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
