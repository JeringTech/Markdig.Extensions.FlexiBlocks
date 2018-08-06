using Jering.IocServices.System.Net.Http;
using Jering.IocServices.System.IO;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using System.Collections.Generic;
using Xunit;
using Microsoft.Extensions.Logging;
using System;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiIncludeBlocks
{
    public class ContentRetrievalServiceUnitTests
    {
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
        public void GetContent_ThrowsArgumentExceptionIfUriSchemeIsNotHttpHttpsOrFile(string dummyUri, string dummyScheme)
        {
            // Arrange
            ContentRetrievalService testSubject = CreateContentRetrievalService();

            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => testSubject.GetContent(dummyUri));
            Assert.Equal(string.Format(Strings.ArgumentException_UriSchemeUnsupported, dummyUri, dummyScheme), result.Message);
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
        public void GetContent_GetsContentIfUriPointsToAFileOnDisk()
        {

        }

        public static IEnumerable<object[]> GetContent_GetsContentIfUriPointsToAFileOnDisk_Data()
        {
            // file://, C:/
            return new object[][]
            {
            };
        }

        private ContentRetrievalService CreateContentRetrievalService(IHttpClientService httpClientService = null,
            IFileService fileService = null,
            ILoggerFactory loggerFactory = null)
        {
            return new ContentRetrievalService(httpClientService, fileService, loggerFactory);
        }
    }
}
