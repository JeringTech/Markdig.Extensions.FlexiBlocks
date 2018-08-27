using Jering.IocServices.System.IO;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiIncludeBlocks
{
    public class FileCacheServiceUnitTests : IClassFixture<FileCacheServiceUnitTestsFixture>
    {
        private readonly string _dummyFile;
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default);

        public FileCacheServiceUnitTests(FileCacheServiceUnitTestsFixture fixture)
        {
            _dummyFile = Path.Combine(fixture.TempDirectory, "dummyFile");
        }

        [Theory]
        [MemberData(nameof(TryGetCacheFile_ThrowsArgumentExceptionIfIdentifierIsNullWhiteSpaceOrAnEmptyString_Data))]
        public void TryGetCacheFile_ThrowsArgumentExceptionIfIdentifierIsNullWhiteSpaceOrAnEmptyString(string dummyIdentifier)
        {
            // Arrange
            FileCacheService fileCacheService = CreateFileCacheService();

            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => fileCacheService.TryGetCacheFile(dummyIdentifier, null));
            Assert.Equal(string.Format(Strings.ArgumentException_ValueCannotBeNullWhiteSpaceOrAnEmptyString, "identifier"), result.Message);
        }

        public static IEnumerable<object[]> TryGetCacheFile_ThrowsArgumentExceptionIfIdentifierIsNullWhiteSpaceOrAnEmptyString_Data()
        {
            return new object[][]
            {
                new object[]{null},
                new object[]{""},
                new object[]{" "}
            };
        }

        [Theory]
        [MemberData(nameof(TryGetCacheFile_ThrowsArgumentExceptionIfCacheDirectoryIsNullWhiteSpaceOrAnEmptyString_Data))]
        public void TryGetCacheFile_ThrowsArgumentExceptionIfCacheDirectoryIsNullWhiteSpaceOrAnEmptyString(string dummyCacheDirectory)
        {
            // Arrange
            const string dummyIdentifier = "dummyIdentifier";
            FileCacheService fileCacheService = CreateFileCacheService();

            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => fileCacheService.TryGetCacheFile(dummyIdentifier, dummyCacheDirectory));
            Assert.Equal(string.Format(Strings.ArgumentException_ValueCannotBeNullWhiteSpaceOrAnEmptyString, "cacheDirectory"), result.Message);
        }

        public static IEnumerable<object[]> TryGetCacheFile_ThrowsArgumentExceptionIfCacheDirectoryIsNullWhiteSpaceOrAnEmptyString_Data()
        {
            return new object[][]
            {
                new object[]{null},
                new object[]{""},
                new object[]{" "}
            };
        }

        [Fact]
        public void TryGetCacheFile_ReturnsFalseIfCacheFileDoesNotExist()
        {
            // Arrange
            const string dummyIdentifier = "dummyIdentifier";
            const string dummyCacheDirectory = "dummyCacheDirectory";
            const string dummyFilePath = "dummyFilePath";
            Mock<IFileService> mockFileService = _mockRepository.Create<IFileService>();
            mockFileService.Setup(f => f.Exists(dummyFilePath)).Returns(false);
            Mock<FileCacheService> mockTestSubject = CreateMockFileCacheService(fileService: mockFileService.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.CreatePath(dummyIdentifier, dummyCacheDirectory)).Returns(dummyFilePath);

            // Act
            (bool resultBool, FileStream resultFileStream) = mockTestSubject.Object.TryGetCacheFile(dummyIdentifier, dummyCacheDirectory);

            // Assert
            _mockRepository.VerifyAll();
            Assert.False(resultBool);
            Assert.Null(resultFileStream);
        }

        [Fact]
        public void TryGetCacheFile_ReturnsTrueAndAFileStreamIfCacheFileExistsAndCanBeOpened()
        {
            // Arrange
            const string dummyIdentifier = "dummyIdentifier";
            const string dummyCacheDirectory = "dummyCacheDirectory";
            const string dummyFilePath = "dummyFilePath";
            using (FileStream dummyFileStream = File.Open(_dummyFile, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
            {
                Mock<IFileService> mockFileService = _mockRepository.Create<IFileService>();
                mockFileService.Setup(f => f.Exists(dummyFilePath)).Returns(true);
                Mock<FileCacheService> mockTestSubject = CreateMockFileCacheService(fileService: mockFileService.Object);
                mockTestSubject.CallBase = true;
                mockTestSubject.Setup(t => t.CreatePath(dummyIdentifier, dummyCacheDirectory)).Returns(dummyFilePath);
                mockTestSubject.Setup(t => t.GetStream(dummyFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)).Returns(dummyFileStream);

                // Act
                (bool resultBool, FileStream resultFileStream) = mockTestSubject.Object.TryGetCacheFile(dummyIdentifier, dummyCacheDirectory);

                // Assert
                _mockRepository.VerifyAll();
                Assert.True(resultBool);
                Assert.Same(dummyFileStream, resultFileStream);
            }
        }

        [Theory]
        [MemberData(nameof(TryGetCacheFile_ReturnsFalseIfCacheFileIsDeletedBetweenFileExistsAndFileOpen_Data))]
        public void TryGetCacheFile_ReturnsFalseIfCacheFileIsDeletedBetweenFileExistsAndFileOpen(ISerializableWrapper<Exception> dummyExceptionWrapper)
        {
            // Arrange
            const string dummyCacheDirectory = "dummyCacheDirectory";
            const string dummyIdentifier = "dummyIdentifier";
            const string dummyFilePath = "dummyFilePath";
            Mock<IFileService> mockFileService = _mockRepository.Create<IFileService>();
            mockFileService.Setup(f => f.Exists(dummyFilePath)).Returns(true);
            Mock<FileCacheService> mockTestSubject = CreateMockFileCacheService(fileService: mockFileService.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.CreatePath(dummyIdentifier, dummyCacheDirectory)).Returns(dummyFilePath);
            mockTestSubject.Setup(t => t.GetStream(dummyFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)).Throws(dummyExceptionWrapper.Value);

            // Act
            (bool resultBool, FileStream resultFileStream) = mockTestSubject.Object.TryGetCacheFile(dummyIdentifier, dummyCacheDirectory);

            // Assert
            _mockRepository.VerifyAll();
            Assert.False(resultBool);
            Assert.Null(resultFileStream);
        }

        public static IEnumerable<object[]> TryGetCacheFile_ReturnsFalseIfCacheFileIsDeletedBetweenFileExistsAndFileOpen_Data()
        {
            return new object[][]
            {
                new object[]{new SerializableWrapper<FileNotFoundException>(new FileNotFoundException())},
                new object[]{new SerializableWrapper<DirectoryNotFoundException>(new DirectoryNotFoundException())},
            };
        }

        [Theory]
        [MemberData(nameof(CreateOrGetCacheFile_ThrowsArgumentExceptionIfIdentifierIsNullWhiteSpaceOrAnEmptyString_Data))]
        public void CreateOrGetCacheFile_ThrowsArgumentExceptionIfIdentifierIsNullWhiteSpaceOrAnEmptyString(string dummyIdentifier)
        {
            // Arrange
            FileCacheService fileCacheService = CreateFileCacheService();

            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => fileCacheService.CreateOrGetCacheFile(dummyIdentifier, null));
            Assert.Equal(string.Format(Strings.ArgumentException_ValueCannotBeNullWhiteSpaceOrAnEmptyString, "identifier"), result.Message);
        }

        public static IEnumerable<object[]> CreateOrGetCacheFile_ThrowsArgumentExceptionIfIdentifierIsNullWhiteSpaceOrAnEmptyString_Data()
        {
            return new object[][]
            {
                new object[]{null},
                new object[]{""},
                new object[]{" "}
            };
        }

        [Theory]
        [MemberData(nameof(CreateOrGetCacheFile_ThrowsArgumentExceptionIfCacheDirectoryIsNullWhiteSpaceOrAnEmptyString_Data))]
        public void CreateOrGetCacheFile_ThrowsArgumentExceptionIfCacheDirectoryIsNullWhiteSpaceOrAnEmptyString(string dummyCacheDirectory)
        {
            // Arrange
            const string dummyIdentifier = "dummyIdentifier";
            FileCacheService fileCacheService = CreateFileCacheService();

            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => fileCacheService.CreateOrGetCacheFile(dummyIdentifier, dummyCacheDirectory));
            Assert.Equal(string.Format(Strings.ArgumentException_ValueCannotBeNullWhiteSpaceOrAnEmptyString, "cacheDirectory"), result.Message);
        }

        public static IEnumerable<object[]> CreateOrGetCacheFile_ThrowsArgumentExceptionIfCacheDirectoryIsNullWhiteSpaceOrAnEmptyString_Data()
        {
            return new object[][]
            {
                new object[]{null},
                new object[]{""},
                new object[]{" "}
            };
        }

        [Fact]
        public void CreateOrGetCacheFile_ThrowsArgumentExceptionIfCacheDirectoryIsInvalid()
        {
            // Arrange
            const string dummyIdentifier = "dummyIdentifier";
            const string dummyCacheDirectory = "dummyCacheDirectory";
            var dummyException = new IOException();
            Mock<IDirectoryService> mockDirectoryService = _mockRepository.Create<IDirectoryService>();
            mockDirectoryService.Setup(d => d.CreateDirectory(dummyCacheDirectory)).Throws(dummyException);
            FileCacheService testSubject = CreateFileCacheService(directoryService: mockDirectoryService.Object);

            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => testSubject.CreateOrGetCacheFile(dummyIdentifier, dummyCacheDirectory));

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.ArgumentException_InvalidCacheDirectory, dummyCacheDirectory), result.Message);
            Assert.Same(dummyException, result.InnerException);
        }

        [Fact]
        public void CreateOrGetCacheFile_ReturnsFileStreamIfSuccessful()
        {
            // Arrange
            const string dummyFilePath = "dummyFilePath";
            const string dummyIdentifier = "dummyIdentifier";
            const string dummyCacheDirectory = "dummyCacheDirectory";
            Mock<IDirectoryService> mockDirectoryService = _mockRepository.Create<IDirectoryService>();
            mockDirectoryService.Setup(d => d.CreateDirectory(dummyCacheDirectory)); // Do nothing
            using (FileStream dummyFileStream = File.Open(_dummyFile, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
            {
                Mock<FileCacheService> mockTestSubject = CreateMockFileCacheService(directoryService: mockDirectoryService.Object);
                mockTestSubject.CallBase = true;
                mockTestSubject.Setup(t => t.CreatePath(dummyIdentifier, dummyCacheDirectory)).Returns(dummyFilePath);
                mockTestSubject.Setup(t => t.GetStream(dummyFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None)).Returns(dummyFileStream);

                // Act
                FileStream result = mockTestSubject.Object.CreateOrGetCacheFile(dummyIdentifier, dummyCacheDirectory);

                // Assert
                _mockRepository.VerifyAll();
                Assert.Same(dummyFileStream, result);
            }
        }

        [Fact]
        public void GetStream_ThrowsIOExceptionIfCacheFileExistsButIsInUseAndRemainsInUseOnTheThirdTryToOpenIt()
        {
            // Arrange
            const string dummyFilePath = "dummyFilePath";
            const FileMode dummyFileMode = FileMode.Open;
            const FileAccess dummyFileAccess = FileAccess.Read;
            const FileShare dummyFileShare = FileShare.Read;
            var dummyIOException = new IOException();
            Mock<IFileService> mockFileService = _mockRepository.Create<IFileService>();
            mockFileService.Setup(f => f.Open(dummyFilePath, dummyFileMode, dummyFileAccess, dummyFileShare)).Throws(dummyIOException);
            FileCacheService testSubject = CreateFileCacheService(fileService: mockFileService.Object);

            // Act and assert
            IOException result = Assert.Throws<IOException>(() => testSubject.GetStream(dummyFilePath, dummyFileMode, dummyFileAccess, dummyFileShare));
            _mockRepository.VerifyAll();
            mockFileService.Verify(f => f.Open(dummyFilePath, dummyFileMode, dummyFileAccess, dummyFileShare), Times.Exactly(3));
            Assert.Same(dummyIOException, result);
        }

        [Fact]
        public void CreatePath_CreatesPath()
        {
            // Arrange 
            const string dummyCacheDirectory = "dummyCacheDirectory";
            const string dummyIdentifier = "dummyIdentifier";
            FileCacheService testSubject = CreateFileCacheService();

            // Act
            string result = testSubject.CreatePath(dummyIdentifier, dummyCacheDirectory);
            Assert.Equal($"{dummyCacheDirectory}{Path.DirectorySeparatorChar}{dummyIdentifier}.txt", result);
        }

        private Mock<FileCacheService> CreateMockFileCacheService(IFileService fileService = null, IDirectoryService directoryService = null, ILoggerFactory loggerFactory = null)
        {
            return _mockRepository.Create<FileCacheService>(fileService, directoryService, loggerFactory);
        }

        private FileCacheService CreateFileCacheService(IFileService fileService = null, IDirectoryService directoryService = null, ILoggerFactory loggerFactory = null)
        {
            return new FileCacheService(fileService, directoryService, loggerFactory);
        }
    }
}
