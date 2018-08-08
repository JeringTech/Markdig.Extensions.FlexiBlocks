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
    public class FileCacheServiceUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default);

        [Theory]
        [MemberData(nameof(TryGetReadOnlyFileStream_ThrowsArgumentExceptionIfIdentifierIsNullWhiteSpaceOrAnEmptyString_Data))]
        public void TryGetReadOnlyFileStream_ThrowsArgumentExceptionIfIdentifierIsNullWhiteSpaceOrAnEmptyString(string dummyIdentifier)
        {
            // Arrange
            FileCacheService fileCacheService = CreateFileCacheService();

            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => fileCacheService.TryGetReadOnlyFileStream(dummyIdentifier, out FileStream fileStream));
            Assert.Equal(string.Format(Strings.ArgumentException_CannotBeNullWhiteSpaceOrAnEmptyString, "identifier"), result.Message);
        }

        public static IEnumerable<object[]> TryGetReadOnlyFileStream_ThrowsArgumentExceptionIfIdentifierIsNullWhiteSpaceOrAnEmptyString_Data()
        {
            return new object[][]
            {
                new object[]{null},
                new object[]{""},
                new object[]{" "}
            };
        }

        [Fact]
        public void TryGetReadOnlyFileStream_ReturnsFalseIfCacheFileDoesNotExist()
        {
            // Arrange
            const string dummyIdentifier = "dummyIdentifier";
            const string dummyFilePath = "dummyFilePath";
            Mock<IFileService> mockFileService = _mockRepository.Create<IFileService>();
            mockFileService.Setup(f => f.Exists(dummyFilePath)).Returns(false);
            Mock<FileCacheService> testSubject = CreateMockFileCacheService(fileService: mockFileService.Object);
            testSubject.CallBase = true;
            testSubject.Setup(t => t.CreatePath(dummyIdentifier)).Returns(dummyFilePath);

            // Act
            bool result = testSubject.Object.TryGetReadOnlyFileStream(dummyIdentifier, out FileStream resultFileStream);

            // Assert
            _mockRepository.VerifyAll();
            Assert.False(result);
            Assert.Null(resultFileStream);
        }

        [Fact]
        public void TryGetReadOnlyFileStream_ReturnsTrueAndAssignsFileStreamIfCacheFileExistsAndCanBeOpened()
        {
            // Arrange
            const string dummyIdentifier = "dummyIdentifier";
            const string dummyFilePath = "dummyFilePath";
            // Place dummy file in bin and use same name on every test so that it gets deleted eventually when project is cleaned and rebuilt
            using (FileStream dummyFileStream = File.Open(Directory.GetCurrentDirectory() + "/dummy.txt", FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
            {
                Mock<IFileService> mockFileService = _mockRepository.Create<IFileService>();
                mockFileService.Setup(f => f.Exists(dummyFilePath)).Returns(true);
                Mock<FileCacheService> testSubject = CreateMockFileCacheService(fileService: mockFileService.Object);
                testSubject.CallBase = true;
                testSubject.Setup(t => t.CreatePath(dummyIdentifier)).Returns(dummyFilePath);
                testSubject.Setup(t => t.GetStream(dummyFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)).Returns(dummyFileStream);

                // Act
                bool result = testSubject.Object.TryGetReadOnlyFileStream(dummyIdentifier, out FileStream resultFileStream);

                // Assert
                _mockRepository.VerifyAll();
                Assert.True(result);
                Assert.Same(dummyFileStream, resultFileStream);
            }
        }

        [Theory]
        [MemberData(nameof(TryGetReadOnlyFileStream_ReturnsFalseIfCacheFileIsDeletedBetweenFileExistsAndFileOpen_Data))]
        public void TryGetReadOnlyFileStream_ReturnsFalseIfCacheFileIsDeletedBetweenFileExistsAndFileOpen(ISerializableWrapper<Exception> dummyExceptionWrapper)
        {
            // Arrange
            const string dummyIdentifier = "dummyIdentifier";
            const string dummyFilePath = "dummyFilePath";
            Mock<IFileService> mockFileService = _mockRepository.Create<IFileService>();
            mockFileService.Setup(f => f.Exists(dummyFilePath)).Returns(true);
            Mock<FileCacheService> testSubject = CreateMockFileCacheService(fileService: mockFileService.Object);
            testSubject.CallBase = true;
            testSubject.Setup(t => t.CreatePath(dummyIdentifier)).Returns(dummyFilePath);
            testSubject.Setup(t => t.GetStream(dummyFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)).Throws(dummyExceptionWrapper.Value);

            // Act
            bool result = testSubject.Object.TryGetReadOnlyFileStream(dummyIdentifier, out FileStream resultFileStream);

            // Assert
            _mockRepository.VerifyAll();
            Assert.False(result);
            Assert.Null(resultFileStream);
        }

        public static IEnumerable<object[]> TryGetReadOnlyFileStream_ReturnsFalseIfCacheFileIsDeletedBetweenFileExistsAndFileOpen_Data()
        {
            return new object[][]
            {
                new object[]{new SerializableWrapper<FileNotFoundException>(new FileNotFoundException())},
                new object[]{new SerializableWrapper<DirectoryNotFoundException>(new DirectoryNotFoundException())},
            };
        }

        [Theory]
        [MemberData(nameof(GetWriteOnlyFileStream_ThrowsArgumentExceptionIfIdentifierIsNullWhiteSpaceOrAnEmptyString_Data))]
        public void GetWriteOnlyFileStream_ThrowsArgumentExceptionIfIdentifierIsNullWhiteSpaceOrAnEmptyString(string dummyIdentifier)
        {
            // Arrange
            FileCacheService fileCacheService = CreateFileCacheService();

            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => fileCacheService.GetWriteOnlyFileStream(dummyIdentifier));
            Assert.Equal(string.Format(Strings.ArgumentException_CannotBeNullWhiteSpaceOrAnEmptyString, "identifier"), result.Message);
        }

        public static IEnumerable<object[]> GetWriteOnlyFileStream_ThrowsArgumentExceptionIfIdentifierIsNullWhiteSpaceOrAnEmptyString_Data()
        {
            return new object[][]
            {
                new object[]{null},
                new object[]{""},
                new object[]{" "}
            };
        }

        [Fact]
        public void GetWriteOnlyFileStream_ReturnsFileStreamIfCacheFileCanBeOpenedOrCreated()
        {
            // Arrange
            const string dummyIdentifier = "dummyIdentifier";
            const string dummyFilePath = "dummyFilePath";
            // Place dummy file in bin and use same name on every test so that it gets deleted eventually when project is cleaned and rebuilt
            using (FileStream dummyFileStream = File.Open(Directory.GetCurrentDirectory() + "/dummy.txt", FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
            {
                Mock<FileCacheService> testSubject = CreateMockFileCacheService();
                testSubject.CallBase = true;
                testSubject.Setup(t => t.CreatePath(dummyIdentifier)).Returns(dummyFilePath);
                testSubject.Setup(t => t.GetStream(dummyFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)).Returns(dummyFileStream);

                // Act
                FileStream result = testSubject.Object.GetWriteOnlyFileStream(dummyIdentifier);

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
            const string dummyRootDirectory = "dummyRootDirectory";
            const string dummyIdentifier = "dummyIdentifier";
            var dummyOptions = new FileCacheServiceOptions { RootDirectory = dummyRootDirectory };
            Mock<IOptions<FileCacheServiceOptions>> mockOptions = _mockRepository.Create<IOptions<FileCacheServiceOptions>>();
            mockOptions.Setup(o => o.Value).Returns(dummyOptions);
            FileCacheService testSubject = CreateFileCacheService(mockOptions.Object);

            // Act
            string result = testSubject.CreatePath(dummyIdentifier);
            Assert.Equal($"{dummyRootDirectory}{Path.DirectorySeparatorChar}{dummyIdentifier}{Path.DirectorySeparatorChar}.txt", result);
        }

        private Mock<FileCacheService> CreateMockFileCacheService(IOptions<FileCacheServiceOptions> optionsAccessor = null, IFileService fileService = null, ILoggerFactory loggerFactory = null)
        {
            return _mockRepository.Create<FileCacheService>(optionsAccessor, fileService, loggerFactory);
        }

        private FileCacheService CreateFileCacheService(IOptions<FileCacheServiceOptions> optionsAccessor = null, IFileService fileService = null, ILoggerFactory loggerFactory = null)
        {
            return new FileCacheService(optionsAccessor, fileService, loggerFactory);
        }
    }
}
