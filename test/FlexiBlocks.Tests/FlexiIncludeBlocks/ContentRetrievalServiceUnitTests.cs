using Jering.IocServices.System.Net.Http;
using Jering.IocServices.System.IO;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using System.Collections.Generic;
using Xunit;
using Microsoft.Extensions.Logging;
using System;
using Moq;
using Microsoft.Extensions.Options;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiIncludeBlocks
{
    public class ContentRetrievalServiceUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default);

        [Theory]
        [MemberData(nameof(GetContent_ThrowsArgumentExceptionIfUriIsNotAValidAbsoluteUri_Data))]
        public void GetContent_ThrowsArgumentExceptionIfUriIsNotAValidAbsoluteUri(string dummyUri)
        {
            // Arrange
            ContentRetrievalService testSubject = CreateContentRetrievalService();

            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => testSubject.GetContent(dummyUri));
            Assert.Equal(string.Format(Strings.ArgumentException_UriMustBeAbsolute, dummyUri), result.Message);
        }

        public static IEnumerable<object[]> GetContent_ThrowsArgumentExceptionIfUriIsNotAValidAbsoluteUri_Data()
        {
            return new object[][]
            {
                new object[]{ "./path/to/file.txt" },
                new object[]{ "../path/to/file.txt" },
                new object[]{ "/path/to/file.txt"  },
                new object[]{ "path/to/file.txt"  }
            };
        }

        [Theory]
        [MemberData(nameof(GetContent_ThrowsArgumentExceptionIfUriSchemeIsNotHttpHttpsOrFile_Data))]
        public void GetContent_ThrowsArgumentExceptionIfUriSchemeIsNotHttpHttpsOrFile(string dummyUri, string expectedScheme)
        {
            // Arrange
            ContentRetrievalService testSubject = CreateContentRetrievalService();

            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => testSubject.GetContent(dummyUri));
            Assert.Equal(string.Format(Strings.ArgumentException_UriSchemeUnsupported, dummyUri, expectedScheme), result.Message);
        }

        public static IEnumerable<object[]> GetContent_ThrowsArgumentExceptionIfUriSchemeIsNotHttpHttpsOrFile_Data()
        {
            return new object[][]
            {
                new object[]{ "ftp://path/to/file.txt", "ftp" },
                new object[]{ "mailto:user@example.com", "mailto" },
                new object[]{ "gopher://www.example.com/file.txt", "gopher" }
            };
        }

        [Theory]
        [MemberData(nameof(GetContent_GetsContentIfUriPointsToAFileOnDisk_Data))]
        public void GetContent_GetsContentIfUriPointsToAFileOnDisk(string dummyUri, string expectedAbsolutePath)
        {
            // Arrange
            var dummyContent = new string[0];
            Mock<IFileService> mockFileService = _mockRepository.Create<IFileService>();
            mockFileService.Setup(f => f.ReadAllLines(expectedAbsolutePath)).Returns(dummyContent); // Uri.TryCreate creates a normalized absolute path for the file
            ContentRetrievalService testSubject = CreateContentRetrievalService(fileService: mockFileService.Object);

            // Act
            string[] result = testSubject.GetContent(dummyUri);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Same(dummyContent, result);
        }

        public static IEnumerable<object[]> GetContent_GetsContentIfUriPointsToAFileOnDisk_Data()
        {
            return new object[][]
            {
                new object[]{"file://C:/path/to/file.txt", "C:/path/to/file.txt"},
                new object[]{"C:/path/to/file.txt", "C:/path/to/file.txt" }
            };
        }

        private ContentRetrievalService CreateContentRetrievalService(IHttpClientService httpClientService = null,
            IFileService fileService = null,
            IFileCacheService fileCacheService = null,
            ILoggerFactory loggerFactory = null,
            IOptions<ContentRetrievalServiceOptions> optionsAccessor = null)
        {
            return new ContentRetrievalService(httpClientService, fileService, fileCacheService, loggerFactory, optionsAccessor);
        }
    }
}
