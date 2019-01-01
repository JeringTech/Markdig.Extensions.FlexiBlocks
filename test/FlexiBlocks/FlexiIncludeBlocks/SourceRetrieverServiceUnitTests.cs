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
    public class SourceRetrieverServiceUnitTests : IClassFixture<SourceRetrieverServiceUnitTestsFixture>
    {
        private readonly string _dummyFile;
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        public SourceRetrieverServiceUnitTests(SourceRetrieverServiceUnitTestsFixture fixture)
        {
            _dummyFile = Path.Combine(fixture.TempDirectory, "dummyFile");
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfHttpClientServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new SourceRetrieverService(
                null,
                _mockRepository.Create<IFileService>().Object,
                _mockRepository.Create<IDiskCacheService>().Object,
                _mockRepository.Create<ILoggerFactory>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFileServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new SourceRetrieverService(
                _mockRepository.Create<IHttpClientService>().Object,
                null,
                _mockRepository.Create<IDiskCacheService>().Object,
                _mockRepository.Create<ILoggerFactory>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfDiskCacheServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new SourceRetrieverService(
                _mockRepository.Create<IHttpClientService>().Object,
                _mockRepository.Create<IFileService>().Object,
                null,
                _mockRepository.Create<ILoggerFactory>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfLoggerFactoryIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new SourceRetrieverService(
                _mockRepository.Create<IHttpClientService>().Object,
                _mockRepository.Create<IFileService>().Object,
                _mockRepository.Create<IDiskCacheService>().Object,
                null));
        }

        [Fact]
        public void GetSource_ThrowsArgumentNullExceptionIfSourceUriIsNull()
        {
            // Arrange
            SourceRetrieverService testSubject = CreateSourceRetrieverService();

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => testSubject.GetSource(null));
        }

        [Fact]
        public void GetSource_RetrievesSourceOnlyOnceAndCachesSourceInMemory()
        {
            // Arrange
            var dummySourceUri = new Uri("C:/dummySourceUri");
            var dummySource = new ReadOnlyCollection<string>(new string[0]);
            Mock<SourceRetrieverService> mockTestSubject = CreateMockSourceRetrieverService();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.GetSourceCore(dummySourceUri, null, default)).
                Callback(() => Thread.Sleep(200)). // Arbitrary sleep duration
                Returns(dummySource);
            // Mock<T>.Object isn't thread safe, if multiple threads call it at the same time, multiple instances are instantiated - https://github.com/moq/moq4/blob/9ca16446b9bbfbe12a78b5f8cad8afaa755c13dc/src/Moq/Mock.Generic.cs#L316
            // If multiple instances are instantiated, multiple _cache instances are created and GetSourceCore gets called multiple times.
            SourceRetrieverService testSubject = mockTestSubject.Object;

            // Act
            var threads = new List<Thread>();
            var results = new ConcurrentBag<ReadOnlyCollection<string>>();
            for (int i = 0; i < 3; i++)
            {
                var thread = new Thread(() => results.Add(testSubject.GetSource(dummySourceUri)));
                thread.Start();
                threads.Add(thread);
            }
            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            // Assert
            mockTestSubject.Verify(t => t.GetSource(dummySourceUri, null, default), Times.Exactly(3));
            mockTestSubject.Verify(t => t.GetSourceCore(dummySourceUri, null, default), Times.Once); // Lazy should prevent GetSourceCore from being called multiple times
            ReadOnlyCollection<string>[] resultsArr = results.ToArray();
            Assert.Equal(3, resultsArr.Length);
            Assert.Same(resultsArr[0], dummySource);
            Assert.Same(resultsArr[0], resultsArr[1]); // Result should get cached after first call
            Assert.Same(resultsArr[1], resultsArr[2]); // The same relation is transitive, so we do not need to check if the item at index 0 is the same as the item at index 2
        }

        [Fact]
        public void GetSource_PropagatesFlexiBlocksExceptionsThrownByGetSourceCore()
        {
            // Arrange
            var dummySourceUri = new Uri("C:/dummySourceUri");
            var dummyFlexiBlocksException = new FlexiBlocksException();
            Mock<SourceRetrieverService> mockTestSubject = CreateMockSourceRetrieverService();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.GetSourceCore(dummySourceUri, null, default)).Throws(dummyFlexiBlocksException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.GetSource(dummySourceUri));
            Assert.Same(dummyFlexiBlocksException, result);
        }

        [Fact]
        public void GetSourceCore_ReadsSourceUsingFileServiceIfSchemeIsFile()
        {
            // Arrange
            var dummyUri = new Uri("C:/dummy/path");
            var dummySource = new ReadOnlyCollection<string>(new string[0]);
            Mock<IFileService> mockFileService = _mockRepository.Create<IFileService>();
            FileStream dummyFileStream = null;
            try
            {
                dummyFileStream = File.Open(_dummyFile, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
                mockFileService.Setup(f => f.Open(dummyUri.AbsolutePath, FileMode.Open, FileAccess.Read, FileShare.Read)).Returns(dummyFileStream);
                Mock<SourceRetrieverService> mockTestSubject = CreateMockSourceRetrieverService(fileService: mockFileService.Object);
                mockTestSubject.CallBase = true;
                mockTestSubject.Setup(c => c.ReadAndNormalizeAllLines(dummyFileStream)).Returns(dummySource);

                // Act
                ReadOnlyCollection<string> result = mockTestSubject.Object.GetSourceCore(dummyUri, null, default);

                // Assert
                _mockRepository.VerifyAll();
                Assert.Same(dummySource, result);
            }
            finally
            {
                dummyFileStream?.Dispose();
            }
        }

        [Fact]
        public void GetSourceCore_ThrowsFlexiBlocksExceptionIfLocalUriIsInvalid()
        {
            // Arrange
            var dummyUri = new Uri("C:/dummy/path");
            var dummyException = new IOException();
            Mock<IFileService> mockFileService = _mockRepository.Create<IFileService>();
            mockFileService.Setup(f => f.Open(dummyUri.AbsolutePath, FileMode.Open, FileAccess.Read, FileShare.Read)).Throws(dummyException);
            SourceRetrieverService testSubject = CreateSourceRetrieverService(fileService: mockFileService.Object);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => testSubject.GetSourceCore(dummyUri, null, default));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.FlexiBlocksException_SourceRetrieverService_InvalidLocalUri, dummyUri.AbsolutePath), result.Message);
        }

        [Fact]
        public void GetSourceCore_CallsGetsRemoteSourceIfSchemeIsNotFile()
        {
            // Arrange
            const string dummyCacheDirectory = "dummyCacheDirectory";
            var dummyUri = new Uri("http://www.dummy.uri");
            var dummySource = new ReadOnlyCollection<string>(new string[0]);
            Mock<SourceRetrieverService> mockTestSubject = CreateMockSourceRetrieverService();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.GetRemoteSource(dummyUri, dummyCacheDirectory, default)).Returns(dummySource);

            // Act
            ReadOnlyCollection<string> result = mockTestSubject.Object.GetSourceCore(dummyUri, dummyCacheDirectory, default);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Same(dummySource, result);
        }

        [Fact]
        public void GetRemoteSource_RetrievesSourceFromDiskCacheIfCacheDirectoryIsDefinedAndSourceExistsInDiskCache()
        {
            // Arrange
            var dummySource = new ReadOnlyCollection<string>(new string[0]);
            const string dummyCacheDirectory = "dummyCacheDirectory";
            var dummyUri = new Uri("C:/dummy/uri");
            FileStream dummyFileStream = null;
            try
            {
                dummyFileStream = File.Open(_dummyFile, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
                Mock<IDiskCacheService> mockDiskCacheService = _mockRepository.Create<IDiskCacheService>();
                mockDiskCacheService.Setup(f => f.TryGetCacheFile(dummyUri.AbsoluteUri, dummyCacheDirectory)).Returns(dummyFileStream);
                Mock<SourceRetrieverService> mockTestSubject = CreateMockSourceRetrieverService(diskCacheService: mockDiskCacheService.Object);
                mockTestSubject.CallBase = true;
                mockTestSubject.Setup(c => c.ReadAndNormalizeAllLines(dummyFileStream)).Returns(dummySource);

                // Act
                ReadOnlyCollection<string> result = mockTestSubject.Object.GetRemoteSource(dummyUri, dummyCacheDirectory, default);

                // Assert
                _mockRepository.VerifyAll();
                Assert.Same(dummySource, result);
            }
            finally
            {
                dummyFileStream?.Dispose();
            }
        }

        [Fact]
        public void GetRemoteSource_RetrievesSourceFromRemoteSourceIfCacheDirectoryIsDefinedButSourceDoesNotExistInDiskCache()
        {
            // Arrange
            const string dummyCacheDirectory = "dummyCacheDirectory";
            const string dummySourceString = "dummy\nSource\nString";
            var dummySourceUri = new Uri("C:/dummy/uri");
            Mock<IDiskCacheService> mockDiskCacheService = _mockRepository.Create<IDiskCacheService>();
            mockDiskCacheService.Setup(f => f.TryGetCacheFile(dummySourceUri.AbsoluteUri, dummyCacheDirectory)).Returns((FileStream)null);
            FileStream dummyFileStream = null;
            try
            {
                var dummySourceStream = new MemoryStream(Encoding.UTF8.GetBytes(dummySourceString));
                var dummyHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(dummySourceStream)
                };
                Mock<IHttpClientService> mockHttpClientService = _mockRepository.Create<IHttpClientService>();
                mockHttpClientService.Setup(h => h.GetAsync(dummySourceUri, HttpCompletionOption.ResponseHeadersRead, default)).ReturnsAsync(dummyHttpResponseMessage);
                dummyFileStream = File.Open(_dummyFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                mockDiskCacheService.Setup(f => f.CreateOrGetCacheFile(dummySourceUri.AbsoluteUri, dummyCacheDirectory)).Returns(dummyFileStream);
                SourceRetrieverService testSubject = CreateSourceRetrieverService(mockHttpClientService.Object, diskCacheService: mockDiskCacheService.Object);

                // Act
                ReadOnlyCollection<string> result = testSubject.GetRemoteSource(dummySourceUri, dummyCacheDirectory, default);

                // Assert
                _mockRepository.VerifyAll();
                Assert.Equal(dummySourceString.Split('\n'), result);
            }
            finally
            {
                dummyFileStream?.Dispose();
            }
        }

        [Fact]
        public void GetRemoteSource_RetrievesSourceFromRemoteSourceIfCacheDirectoryIsNotDefined()
        {
            // Arrange
            const string dummySourceString = "dummy\nSource\nString";
            var dummyUri = new Uri("C:/dummy/uri");
            var dummySourceStream = new MemoryStream(Encoding.UTF8.GetBytes(dummySourceString));
            var dummyHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(dummySourceStream)
            };
            Mock<IHttpClientService> mockHttpClientService = _mockRepository.Create<IHttpClientService>();
            mockHttpClientService.Setup(h => h.GetAsync(dummyUri, HttpCompletionOption.ResponseHeadersRead, default)).ReturnsAsync(dummyHttpResponseMessage);
            SourceRetrieverService testSubject = CreateSourceRetrieverService(mockHttpClientService.Object);

            // Act
            ReadOnlyCollection<string> result = testSubject.GetRemoteSource(dummyUri, null, default);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummySourceString.Split('\n'), result);
        }

        [Fact]
        public void GetRemoteSource_ThrowsFlexiBlocksExceptionIfRemoteSourceDoesNotExist()
        {
            // Arrange
            var dummyUri = new Uri("C:/dummy/uri");
            var dummyHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
            Mock<IHttpClientService> mockHttpClientService = _mockRepository.Create<IHttpClientService>();
            mockHttpClientService.Setup(h => h.GetAsync(dummyUri, HttpCompletionOption.ResponseHeadersRead, default)).ReturnsAsync(dummyHttpResponseMessage);
            SourceRetrieverService testSubject = CreateSourceRetrieverService(mockHttpClientService.Object);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => testSubject.GetRemoteSource(dummyUri, null, default));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.FlexiBlocksException_SourceRetrieverService_RemoteUriDoesNotExist, dummyUri.AbsoluteUri), result.Message);
        }

        [Fact]
        public void GetRemoteSource_ThrowsFlexiBlocksExceptionIfAccessToRemoteSourceIsForbidden()
        {
            // Arrange
            var dummyUri = new Uri("C:/dummy/uri");
            var dummyHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.Forbidden);
            Mock<IHttpClientService> mockHttpClientService = _mockRepository.Create<IHttpClientService>();
            mockHttpClientService.Setup(h => h.GetAsync(dummyUri, HttpCompletionOption.ResponseHeadersRead, default)).ReturnsAsync(dummyHttpResponseMessage);
            SourceRetrieverService testSubject = CreateSourceRetrieverService(mockHttpClientService.Object);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => testSubject.GetRemoteSource(dummyUri, null, default));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.FlexiBlocksException_SourceRetrieverService_RemoteUriAccessForbidden, dummyUri.AbsoluteUri), result.Message);
        }

        [Fact]
        public void GetRemoteSource_LogsWarningsThenThrowsFlexiBlocksExceptionIfARemoteSourceCannotBeRetrievedFromAfterThreeAttempts()
        {
            // Arrange
            var dummyUri = new Uri("C:/dummy/uri");
            var dummyHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError); // Arbitrary status code that might represent an intermittent issue
            Mock<ILogger> mockLogger = _mockRepository.Create<ILogger>();
            mockLogger.Setup(l => l.IsEnabled(LogLevel.Warning)).Returns(true);
            Mock<ILoggerFactory> mockLoggerFactory = _mockRepository.Create<ILoggerFactory>();
            mockLoggerFactory.Setup(l => l.CreateLogger(typeof(SourceRetrieverService).FullName)).Returns(mockLogger.Object);
            var dummyHttpRequestException = new HttpRequestException();
            Mock<IHttpClientService> mockHttpClientService = _mockRepository.Create<IHttpClientService>();
            mockHttpClientService.
                SetupSequence(h => h.GetAsync(dummyUri, HttpCompletionOption.ResponseHeadersRead, default)).
                ReturnsAsync(dummyHttpResponseMessage).
                ThrowsAsync(new OperationCanceledException()).
                ThrowsAsync(dummyHttpRequestException);
            SourceRetrieverService testSubject = CreateSourceRetrieverService(mockHttpClientService.Object, loggerFactory: mockLoggerFactory.Object);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => testSubject.GetRemoteSource(dummyUri, null, default));
            _mockRepository.VerifyAll();
            mockLogger.Verify(l => l.Log(LogLevel.Warning, 0,
                // object is of type FormattedLogValues
                It.Is<object>(f => f.ToString() == string.Format(Strings.LogWarning_SourceRetrieverService_FailureStatusCode, dummyUri.AbsoluteUri, (int)HttpStatusCode.InternalServerError, 2)),
                null, It.IsAny<Func<object, Exception, string>>()), Times.Once);
            mockLogger.Verify(l => l.Log(LogLevel.Warning, 0,
                // object is of type FormattedLogValues
                It.Is<object>(f => f.ToString() == string.Format(Strings.LogWarning_SourceRetrieverService_Timeout, dummyUri.AbsoluteUri, 1)),
                null, It.IsAny<Func<object, Exception, string>>()), Times.Once);
            mockLogger.Verify(l => l.Log(LogLevel.Warning, 0,
                // object is of type FormattedLogValues
                It.Is<object>(f => f.ToString() == string.Format(Strings.LogWarning_SourceRetrieverService_HttpRequestException, dummyHttpRequestException.Message, dummyUri.AbsoluteUri, 0)),
                null, It.IsAny<Func<object, Exception, string>>()), Times.Once);
            Assert.Equal(string.Format(Strings.FlexiBlocksException_SourceRetrieverService_FailedAfterMultipleAttempts, dummyUri.AbsoluteUri), result.Message);
        }

        [Theory]
        [MemberData(nameof(ReadAndNormalizeLines_ReadsAndNormalizesAllLines_Data))]
        public void ReadAndNormalizeLines_ReadsAndNormalizesAllLines(string dummyLines, string[] expectedResult)
        {
            // Arrange
            var dummyMemoryStream = new MemoryStream(Encoding.UTF8.GetBytes(dummyLines));
            SourceRetrieverService testSubject = CreateSourceRetrieverService();

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

        private Mock<SourceRetrieverService> CreateMockSourceRetrieverService(IHttpClientService httpClientService = null,
            IFileService fileService = null,
            IDiskCacheService diskCacheService = null,
            ILoggerFactory loggerFactory = null)
        {
            return _mockRepository.Create<SourceRetrieverService>(httpClientService ?? _mockRepository.Create<IHttpClientService>().Object,
                fileService ?? _mockRepository.Create<IFileService>().Object,
                diskCacheService ?? _mockRepository.Create<IDiskCacheService>().Object,
                loggerFactory ?? _mockRepository.Create<ILoggerFactory>().Object);
        }

        private SourceRetrieverService CreateSourceRetrieverService(IHttpClientService httpClientService = null,
            IFileService fileService = null,
            IDiskCacheService diskCacheService = null,
            ILoggerFactory loggerFactory = null)
        {
            return new SourceRetrieverService(httpClientService ?? _mockRepository.Create<IHttpClientService>().Object,
                fileService ?? _mockRepository.Create<IFileService>().Object,
                diskCacheService ?? _mockRepository.Create<IDiskCacheService>().Object,
                loggerFactory ?? _mockRepository.Create<ILoggerFactory>().Object);
        }
    }

    public class SourceRetrieverServiceUnitTestsFixture : IDisposable
    {
        public string TempDirectory { get; } = Path.Combine(Path.GetTempPath(), nameof(SourceRetrieverServiceUnitTests)); // Dummy file for creating dummy file streams

        public SourceRetrieverServiceUnitTestsFixture()
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
