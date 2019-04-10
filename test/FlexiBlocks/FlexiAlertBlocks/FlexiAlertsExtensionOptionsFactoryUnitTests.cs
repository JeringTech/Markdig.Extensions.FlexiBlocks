using Jering.Markdig.Extensions.FlexiBlocks.ContextObjects;
using Jering.Markdig.Extensions.FlexiBlocks.Alerts;
using Markdig.Parsers;
using Moq;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.Alerts
{
    public class FlexiAlertsExtensionOptionsFactoryUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfContextObjectServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiAlertsExtensionOptionsFactory(null));
        }

        [Fact]
        public void Create_ReturnsIFlexiAlertsExtensionOptionsFromContextObjectsIfItExists()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<IContextObjectsService> mockContextObjectsService = _mockRepository.Create<IContextObjectsService>();
            Mock<IFlexiAlertsExtensionOptions> dummyFlexiAlertsExtensionOptions = _mockRepository.Create<IFlexiAlertsExtensionOptions>();
            object dummyValue = dummyFlexiAlertsExtensionOptions.Object;
            mockContextObjectsService.Setup(c => c.TryGetContextObject(typeof(IFlexiAlertsExtensionOptions), dummyBlockProcessor, out dummyValue)).Returns(true);
            FlexiAlertsExtensionOptionsFactory testSubject = CreateFlexiAlertsExtensionOptionsFactory(mockContextObjectsService.Object);

            // Act
            IFlexiAlertsExtensionOptions result = testSubject.Create(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Same(dummyValue, result);
        }

        [Fact]
        public void Create_ReturnsNewFlexiAlertsExtensionOptionsIfContextObjectIsNotAnIFlexiAlertsExtensionOptions()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<IContextObjectsService> mockContextObjectsService = _mockRepository.Create<IContextObjectsService>();
            var dummyValue = new object();
            mockContextObjectsService.Setup(c => c.TryGetContextObject(typeof(IFlexiAlertsExtensionOptions), dummyBlockProcessor, out dummyValue)).Returns(true);
            mockContextObjectsService.Setup(c => c.TrySetContextObject(typeof(IFlexiAlertsExtensionOptions), It.IsAny<FlexiAlertsExtensionOptions>(), dummyBlockProcessor));
            FlexiAlertsExtensionOptionsFactory testSubject = CreateFlexiAlertsExtensionOptionsFactory(mockContextObjectsService.Object);

            // Act
            IFlexiAlertsExtensionOptions result = testSubject.Create(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.IsType<FlexiAlertsExtensionOptions>(result);
        }

        [Fact]
        public void Create_ReturnsNewFlexiAlertsExtensionOptionsIfThereIsNoContextObjectForTheGivenKey()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<IContextObjectsService> mockContextObjectsService = _mockRepository.Create<IContextObjectsService>();
            object dummyValue = null;
            mockContextObjectsService.Setup(c => c.TryGetContextObject(typeof(IFlexiAlertsExtensionOptions), dummyBlockProcessor, out dummyValue)).Returns(false);
            mockContextObjectsService.Setup(c => c.TrySetContextObject(typeof(IFlexiAlertsExtensionOptions), It.IsAny<FlexiAlertsExtensionOptions>(), dummyBlockProcessor));
            FlexiAlertsExtensionOptionsFactory testSubject = CreateFlexiAlertsExtensionOptionsFactory(mockContextObjectsService.Object);

            // Act
            IFlexiAlertsExtensionOptions result = testSubject.Create(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.IsType<FlexiAlertsExtensionOptions>(result);
        }

        private FlexiAlertsExtensionOptionsFactory CreateFlexiAlertsExtensionOptionsFactory(IContextObjectsService contextObjectService = null)
        {
            return new FlexiAlertsExtensionOptionsFactory(contextObjectService);
        }
    }
}
