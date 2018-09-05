using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.Shared
{
    // These tests also cover ExposedOptionsManager.
    public class FlexiBlocksMarkdownPipelineBuilderExtensionsIntegrationTests : IDisposable
    {
        private IServiceProvider _serviceProvider;

        [Fact]
        public void SetOptions_SetsOptions()
        {
            // Arrange
            var dummyExtensionOptions = new DummyExtensionOptions();
            var dummyServices = new ServiceCollection();
            dummyServices.AddFlexiBlocks();
            _serviceProvider = dummyServices.BuildServiceProvider();

            // Act
            FlexiBlocksMarkdownPipelineBuilderExtensions.SetOptions(dummyExtensionOptions, _serviceProvider);

            // Assert 
            Assert.Same(dummyExtensionOptions, _serviceProvider.GetRequiredService<IOptions<DummyExtensionOptions>>().Value);
        }

        [Fact]
        public void SetOptions_ThrowsFlexiBlocksExceptionIfIOptionsServiceIsNotAnExposedOptionsManager()
        {
            // Arrange
            var dummyExtensionOptions = new DummyExtensionOptions();
            var dummyServices = new ServiceCollection();
            dummyServices.AddFlexiBlocks();
            dummyServices.AddSingleton<IOptions<DummyExtensionOptions>, OptionsManager<DummyExtensionOptions>>(); // Overwrite ExposedOptionsManager
            IServiceProvider dummyServiceProvider = dummyServices.BuildServiceProvider();

            // Act and assert
            FlexiBlocksException result =Assert.Throws<FlexiBlocksException>(() => FlexiBlocksMarkdownPipelineBuilderExtensions.SetOptions(dummyExtensionOptions, dummyServiceProvider));
            Assert.Equal(Strings.FlexiBlocksException_UnableToSetOptions, result.Message);
        }

        public class DummyBlockOptions : IMarkdownObjectOptions<DummyBlockOptions>
        {
            public HtmlAttributeDictionary Attributes { get; set; }

            public DummyBlockOptions Clone()
            {
                return null;
            }
        }

        public class DummyExtensionOptions : IMarkdownExtensionOptions<DummyBlockOptions>
        {
            public DummyBlockOptions DefaultBlockOptions { get; set; }
        }

        public void Dispose()
        {
            (_serviceProvider as IDisposable)?.Dispose();
        }
    }
}
