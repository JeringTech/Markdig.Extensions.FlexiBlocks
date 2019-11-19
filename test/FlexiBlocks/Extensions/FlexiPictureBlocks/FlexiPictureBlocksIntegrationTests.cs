using Jering.Markdig.Extensions.FlexiBlocks.FlexiPictureBlocks;
using Markdig;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiPictureBlocks
{
    // Integration tests that do not fit in amongst specs for this extension
    public class FlexiPictureBlocksIntegrationTests : IClassFixture<FlexiPictureBlocksIntegrationTestsFixture>
    {
        private readonly FlexiPictureBlocksIntegrationTestsFixture _fixture;
        private const string _dummyImageFileName = "exampleImage.png";

        public FlexiPictureBlocksIntegrationTests(FlexiPictureBlocksIntegrationTestsFixture fixture)
        {
            _fixture = fixture;
            _fixture.RecreateDirectory();
            string sourcePath = Path.Combine(Directory.GetCurrentDirectory(), _dummyImageFileName);
            string targetPath = Path.Combine(_fixture.TempDirectory, _dummyImageFileName);
            File.Copy(sourcePath, targetPath);
        }

        [Theory]
        [MemberData(nameof(FlexiPictureBlocks_PerformsFileOperationsWhenRequired_Data))]
        public void FlexiPictureBlocks_PerformsFileOperationsWhenRequired(string dummyMarkdown, string expectedHtml)
        {
            // Arrange
            var flexiPictureBlocksExtensionOptions = new FlexiPictureBlocksExtensionOptions(localMediaDirectory: _fixture.TempDirectory);
            var markdownPipelineBuilder = new MarkdownPipelineBuilder();
            markdownPipelineBuilder.
                UseFlexiOptionsBlocks().
                UseFlexiPictureBlocks(flexiPictureBlocksExtensionOptions);
            MarkdownPipeline markdownPipeline = markdownPipelineBuilder.Build();

            // Act
            string result = Markdown.ToHtml(dummyMarkdown, markdownPipeline);

            // Assert
            Assert.Equal(expectedHtml, result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> FlexiPictureBlocks_PerformsFileOperationsWhenRequired_Data()
        {
            const double dummyWidth = 300;
            const double dummyHeight = 200;
            const double dummyAspectRatio = dummyHeight / dummyWidth * 100;
            string dummySrc = $"/url/{_dummyImageFileName}";
            const double expectedWidth = 155;
            const double expectedAspectRatio = 78 / expectedWidth * 100;

            return new object[][]
            {
                // Retrieves width and height
                new object[]
                {
                    $@"p{{
    ""src"": ""{dummySrc}""
}}",
                    $@"<div class=""flexi-picture flexi-picture_no-alt flexi-picture_is-lazy flexi-picture_has-width flexi-picture_has-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner"">
<button class=""flexi-picture__exit-fullscreen-button"" aria-label=""Exit fullscreen"">
<svg class=""flexi-picture__exit-fullscreen-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path d=""M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z""/><path d=""M0 0h24v24H0z"" fill=""none""/></svg>
</button>
<div class=""flexi-picture__container"" style=""width:{expectedWidth}px"">
<div class=""flexi-picture__error-notice"">
<svg class=""flexi-picture__error-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path d=""M0 0h24v24H0z"" fill=""none""/><path d=""M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z""/></svg>
</div>
<div class=""flexi-picture__spinner spinner"">
    <div class=""spinner__rects"">
        <div class=""spinner__rect-1""></div>
        <div class=""spinner__rect-2""></div>
        <div class=""spinner__rect-3""></div>
    </div>
</div>
<div class=""flexi-picture__picture-container"" style=""width:{expectedWidth}px"">
<picture class=""flexi-picture__picture"" style=""padding-bottom:{expectedAspectRatio}%"">
<img class=""flexi-picture__image"" data-src=""{dummySrc}"" tabindex=""-1"">
</picture>
</div>
</div>
</div>
"
                },
                // Specified width and height take precedence
                new object[]
                {
                    $@"p{{
    ""src"": ""{dummySrc}"",
    ""width"": {dummyWidth},
    ""height"": {dummyHeight}
}}",
                    $@"<div class=""flexi-picture flexi-picture_no-alt flexi-picture_is-lazy flexi-picture_has-width flexi-picture_has-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner"">
<button class=""flexi-picture__exit-fullscreen-button"" aria-label=""Exit fullscreen"">
<svg class=""flexi-picture__exit-fullscreen-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path d=""M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z""/><path d=""M0 0h24v24H0z"" fill=""none""/></svg>
</button>
<div class=""flexi-picture__container"" style=""width:{dummyWidth}px"">
<div class=""flexi-picture__error-notice"">
<svg class=""flexi-picture__error-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path d=""M0 0h24v24H0z"" fill=""none""/><path d=""M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z""/></svg>
</div>
<div class=""flexi-picture__spinner spinner"">
    <div class=""spinner__rects"">
        <div class=""spinner__rect-1""></div>
        <div class=""spinner__rect-2""></div>
        <div class=""spinner__rect-3""></div>
    </div>
</div>
<div class=""flexi-picture__picture-container"" style=""width:{dummyWidth}px"">
<picture class=""flexi-picture__picture"" style=""padding-bottom:{dummyAspectRatio}%"">
<img class=""flexi-picture__image"" data-src=""{dummySrc}"" tabindex=""-1"">
</picture>
</div>
</div>
</div>
"
                },
                // EnableFileOperations set to false disables retrieval of dimensions
                new object[]
                {
                    $@"p{{
    ""src"": ""{dummySrc}"",
    ""enableFileOperations"": false
}}",
                    $@"<div class=""flexi-picture flexi-picture_no-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner"">
<button class=""flexi-picture__exit-fullscreen-button"" aria-label=""Exit fullscreen"">
<svg class=""flexi-picture__exit-fullscreen-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path d=""M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z""/><path d=""M0 0h24v24H0z"" fill=""none""/></svg>
</button>
<div class=""flexi-picture__container"">
<div class=""flexi-picture__error-notice"">
<svg class=""flexi-picture__error-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path d=""M0 0h24v24H0z"" fill=""none""/><path d=""M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z""/></svg>
</div>
<div class=""flexi-picture__spinner spinner"">
    <div class=""spinner__rects"">
        <div class=""spinner__rect-1""></div>
        <div class=""spinner__rect-2""></div>
        <div class=""spinner__rect-3""></div>
    </div>
</div>
<div class=""flexi-picture__picture-container"">
<picture class=""flexi-picture__picture"">
<img class=""flexi-picture__image"" data-src=""{dummySrc}"" tabindex=""-1"">
</picture>
</div>
</div>
</div>
"
                }
            };
        }
    }

    public class FlexiPictureBlocksIntegrationTestsFixture : IDisposable
    {
        public string TempDirectory { get; } = Path.Combine(Path.GetTempPath(), nameof(FlexiPictureBlocksIntegrationTests)); // Dummy directory for containing images

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
