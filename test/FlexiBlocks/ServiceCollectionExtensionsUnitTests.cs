using Jering.Markdig.Extensions.FlexiBlocks.ContextObjects;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiBannerBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiCardsBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiFigureBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiPictureBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiQuoteBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiTabsBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiVideoBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests
{
    public class ServiceCollectionExtensionsUnitTests
    {
        [Fact]
        public void AddContextObjects_AddsServicesForTheContextObjectsExtension()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddContextObjects();
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            // Act and assert
            serviceProvider.GetRequiredService<ContextObjectsExtension>(); // As long as this doesn't throw, all required services have been added
        }

        [Fact]
        public void AddFlexiIncludeBlocks_AddsServicesForTheFlexiIncludeBlocksExtension()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddFlexiIncludeBlocks();
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            // Act and assert
            serviceProvider.GetRequiredService<IBlockExtension<FlexiIncludeBlock>>(); // As long as this doesn't throw, all required services have been added
        }

        [Fact]
        public void AddFlexiOptionsBlock_AddsServicesForTheFlexiOptionsBlocksExtension()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddFlexiOptionsBlocks();
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            // Act and assert
            serviceProvider.GetRequiredService<IBlockExtension<FlexiOptionsBlock>>(); // As long as this doesn't throw, all required services have been added
        }

        [Fact]
        public void AddFlexiAlertBlocks_AddsServicesForTheFlexiAlertBlocksExtension()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddFlexiAlertBlocks();
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            // Act and assert
            serviceProvider.GetRequiredService<IBlockExtension<FlexiAlertBlock>>(); // As long as this doesn't throw, all required services have been added
        }

        [Fact]
        public void AddFlexiBannerBlocks_AddsServicesForTheFlexiBannerBlocksExtension()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddFlexiBannerBlocks();
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            // Act and assert
            serviceProvider.GetRequiredService<IBlockExtension<FlexiBannerBlock>>(); // As long as this doesn't throw, all required services have been added
        }

        [Fact]
        public void AddFlexiCardsBlocks_AddsServicesForTheFlexiCardsBlocksExtension()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddFlexiCardsBlocks();
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            // Act and assert
            serviceProvider.GetRequiredService<IBlockExtension<FlexiCardsBlock>>(); // As long as this doesn't throw, all required services have been added
        }

        [Fact]
        public void AddFlexiCodeBlocks_AddsServicesForTheFlexiCodeBlocksExtension()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddFlexiCodeBlocks();
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            // Act and assert
            serviceProvider.GetRequiredService<IBlockExtension<FlexiCodeBlock>>(); // As long as this doesn't throw, all required services have been added
        }

        [Fact]
        public void AddFlexiFigureBlocks_AddsServicesForTheFlexiFigureBlocksExtension()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddFlexiFigureBlocks();
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            // Act and assert
            serviceProvider.GetRequiredService<IBlockExtension<FlexiFigureBlock>>(); // As long as this doesn't throw, all required services have been added
        }

        [Fact]
        public void AddFlexiPictureBlocks_AddsServicesForTheFlexiPictureBlocksExtension()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddFlexiPictureBlocks();
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            // Act and assert
            serviceProvider.GetRequiredService<IBlockExtension<FlexiPictureBlock>>(); // As long as this doesn't throw, all required services have been added
        }

        [Fact]
        public void AddFlexiQuoteBlocks_AddsServicesForTheFlexiQuoteBlocksExtension()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddFlexiQuoteBlocks();
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            // Act and assert
            serviceProvider.GetRequiredService<IBlockExtension<FlexiQuoteBlock>>(); // As long as this doesn't throw, all required services have been added
        }

        [Fact]
        public void AddFlexiSectionBlocks_AddsServicesForTheFlexiSectionBlocksExtension()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddFlexiSectionBlocks();
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            // Act and assert
            serviceProvider.GetRequiredService<IBlockExtension<FlexiSectionBlock>>(); // As long as this doesn't throw, all required services have been added
        }

        [Fact]
        public void AddFlexiTableBlocks_AddsServicesForTheFlexiTableBlocksExtension()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddFlexiTableBlocks();
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            // Act and assert
            serviceProvider.GetRequiredService<IBlockExtension<FlexiTableBlock>>(); // As long as this doesn't throw, all required services have been added
        }

        [Fact]
        public void AddFlexiTabsBlocks_AddsServicesForTheFlexiTabsBlocksExtension()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddFlexiTabsBlocks();
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            // Act and assert
            serviceProvider.GetRequiredService<IBlockExtension<FlexiTabsBlock>>(); // As long as this doesn't throw, all required services have been added
        }

        [Fact]
        public void AddFlexiVideoBlocks_AddsServicesForTheFlexiVideoBlocksExtension()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddFlexiVideoBlocks();
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            // Act and assert
            serviceProvider.GetRequiredService<IBlockExtension<FlexiVideoBlock>>(); // As long as this doesn't throw, all required services have been added
        }
    }
}
