using Jering.IocServices.System.IO;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiPictureBlocks;
using Moq;
using System;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiPictureBlocks
{
    public class ImageServiceUnitTests
    {
        private static readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFileServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new ImageService(null));
        }

        [Fact]
        public void GetImageDimensions_ThrowsInvalidOperationExceptionIfAnExceptionIsThrownWhileAttemptingToReadDimensionsFromFile()
        {
            // Arrange
            const string dummyPath = "dummyPath";
            var dummyException = new Exception();
            Mock<IFileService> mockFileService = _mockRepository.Create<IFileService>();
            mockFileService.Setup(f => f.Open(dummyPath, FileMode.Open, FileAccess.Read, FileShare.Read)).Throws(dummyException);
            ImageService testSubject = CreateImageService(mockFileService.Object);

            // Act and assert
            InvalidOperationException result = Assert.Throws<InvalidOperationException>(() => testSubject.GetImageDimensions(dummyPath));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.InvalidOperationsException_ImageService_ExceptionThrownWhileAttemptingToReadDimensionsFromLocalImageFile, dummyPath),
                result.Message);
            Assert.Same(dummyException, result.InnerException);
        }

        private static ImageService CreateImageService(IFileService fileService = null)
        {
            return new ImageService(fileService ?? _mockRepository.Create<IFileService>().Object);
        }
    }
}
