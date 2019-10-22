using Jering.Markdig.Extensions.FlexiBlocks.FlexiPictureBlocks;
using Markdig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiPictureBlocks
{
    // Integration tests that do not fit in amongst specs for this extension
    public class ImageServiceIntegrationTests : IClassFixture<ImageServiceIntegrationTestsFixture>
    {
        private readonly ImageServiceIntegrationTestsFixture _fixture;
        private const string _dummyImageFileName = "exampleImage.png";

        public ImageServiceIntegrationTests(ImageServiceIntegrationTestsFixture fixture)
        {
            _fixture = fixture;
            _fixture.RecreateDirectory();
        }

        [Fact]
        public void GetImageDimensions_ThrowsInvalidOperationExceptionIfFileIsCorruptedOrEncodedInAnUnsupportedFormat()
        {
            // Arrange
            string dummyPath = Path.Combine(_fixture.TempDirectory, _dummyImageFileName);
            File.WriteAllText(dummyPath, "not an image");
            IServiceCollection dummyServiceCollection = new ServiceCollection().AddFlexiPictureBlocks();
            using (ServiceProvider serviceProvider = dummyServiceCollection.BuildServiceProvider())
            {
                IImageService testSubject = serviceProvider.GetRequiredService<IImageService>();

                // Act and assert
                InvalidOperationException result = Assert.Throws<InvalidOperationException>(() => testSubject.GetImageDimensions(dummyPath));
                Assert.Equal(string.Format(Strings.InvalidOperationException_ImageService_UnableToReadDimensionsFromImageFile, dummyPath),
                    result.Message);
            }
        }

        [Fact]
        public void GetImageDimensions_RetrievesImageDimensions()
        {
            // Arrange
            string dummySourcePath = Path.Combine(Directory.GetCurrentDirectory(), _dummyImageFileName);
            string dummyTargetPath = Path.Combine(_fixture.TempDirectory, _dummyImageFileName);
            File.Copy(dummySourcePath, dummyTargetPath);
            IServiceCollection dummyServiceCollection = new ServiceCollection().AddFlexiPictureBlocks();
            using (ServiceProvider serviceProvider = dummyServiceCollection.BuildServiceProvider())
            {
                IImageService testSubject = serviceProvider.GetRequiredService<IImageService>();

                // Act
                (int width, int height) = testSubject.GetImageDimensions(dummyTargetPath);

                // Assert
                Assert.Equal(155, width); // TODO would be better if we generated the image as part of this test so we can verify that the right dimensions are retrieved
                Assert.Equal(78, height);
            }
        }
    }

    public class ImageServiceIntegrationTestsFixture : IDisposable
    {
        public string TempDirectory { get; } = Path.Combine(Path.GetTempPath(), nameof(ImageServiceIntegrationTests)); // Dummy directory for containing images

        public void RecreateDirectory()
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

        // Called after all tests are complete
        public void Dispose()
        {
            TryDeleteDirectory();
        }
    }
}
