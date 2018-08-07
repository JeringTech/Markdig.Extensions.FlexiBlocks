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
                mockFileService.Setup(f => f.Open(dummyFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)).Returns(dummyFileStream);
                Mock<FileCacheService> testSubject = CreateMockFileCacheService(fileService: mockFileService.Object);
                testSubject.CallBase = true;
                testSubject.Setup(t => t.CreatePath(dummyIdentifier)).Returns(dummyFilePath);

                // Act
                bool result = testSubject.Object.TryGetReadOnlyFileStream(dummyIdentifier, out FileStream resultFileStream);

                // Assert
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
            var dummyIOException = new IOException();
            Mock<IFileService> mockFileService = _mockRepository.Create<IFileService>();
            mockFileService.Setup(f => f.Exists(dummyFilePath)).Returns(true);
            mockFileService.Setup(f => f.Open(dummyFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)).Throws(dummyExceptionWrapper.Value);
            Mock<FileCacheService> testSubject = CreateMockFileCacheService(fileService: mockFileService.Object);
            testSubject.CallBase = true;
            testSubject.Setup(t => t.CreatePath(dummyIdentifier)).Returns(dummyFilePath);

            // Act
            bool result = testSubject.Object.TryGetReadOnlyFileStream(dummyIdentifier, out FileStream resultFileStream);

            // Assert
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


        [Fact]
        public void TryGetReadOnlyFileStream_ThrowsIOExceptionIfCacheFileIsInUseAndRemainsInUseOnTheThirdTryToOpenIt()
        {

        }

        [Fact]
        public void GetWriteOnlyFileStream_ThrowsArgumentExceptionIfIdentifierIsNullWhiteSpaceOrAnEmptyString()
        {

        }

        [Fact]
        public void GetWriteOnlyFileStream_ReturnsFileStramIfCacheFileCanBeOpenedOrCreated()
        {

        }

        [Fact]
        public void GetWriteOnlyFileStream_ThrowsIOExceptionIfCacheFileExistsButIsInUseAndRemainsInUseOnTheThirdTryToOpenIt()
        {

        }

        [Fact]
        public void CreatePath_CreatesPath()
        {

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
