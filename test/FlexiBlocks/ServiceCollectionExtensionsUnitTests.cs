﻿using Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.ContextObjects;
using Jering.Markdig.Extensions.FlexiBlocks.OptionsBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.IncludeBlocks;
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
        public void AddIncludeBlocks_AddsServicesForTheIncludeBlocksExtension()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddIncludeBlocks();
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            // Act and assert
            serviceProvider.GetRequiredService<IBlockExtension<IncludeBlock>>(); // As long as this doesn't throw, all required services have been added
        }

        [Fact]
        public void AddOptionsBlock_AddsServicesForTheOptionsBlocksExtension()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddOptionsBlocks();
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            // Act and assert
            serviceProvider.GetRequiredService<IBlockExtension<OptionsBlock>>(); // As long as this doesn't throw, all required services have been added
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
    }
}