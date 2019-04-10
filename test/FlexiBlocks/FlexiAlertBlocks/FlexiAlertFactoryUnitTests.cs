using Jering.Markdig.Extensions.FlexiBlocks.Alerts;
using Jering.Markdig.Extensions.FlexiBlocks.Options;
using Markdig.Helpers;
using Markdig.Parsers;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.Alerts
{
    public class FlexiAlertFactoryUnitTests // TODO ordering when constructor is tested
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiBlockOptionsFactoryIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiAlertFactory(null, _mockRepository.Create<IFlexiAlertsExtensionOptionsFactory>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiAlertsExtensionOptionsFactoryIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiAlertFactory(_mockRepository.Create<IFlexiBlockOptionsFactory>().Object, null));
        }

        [Fact]
        public void CreateBlock_CreatesBlock()
        {
            // Arrange
            const string dummyIconHtmlFragment = "dummyIconHtmlFragment";
            const int dummySpanStart = 3;
            const int dummySpanEnd = 4;
            var dummyStringSlice = new StringSlice("dummyText", dummySpanStart, dummySpanEnd);
            const int dummyColumn = 2;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyStringSlice;
            dummyBlockProcessor.Column = dummyColumn;
            Mock<IFlexiAlertOptions> dummyFlexiAlertOptions = _mockRepository.Create<IFlexiAlertOptions>();
            Mock<IFlexiAlertsExtensionOptions> mockFlexiAlertsExtensionOptions = _mockRepository.Create<IFlexiAlertsExtensionOptions>();
            mockFlexiAlertsExtensionOptions.Setup(f => f.DefaultBlockOptions).Returns(dummyFlexiAlertOptions.Object);
            Mock<IFlexiBlockOptionsFactory> mockFlexiBlockOptionsFactory = _mockRepository.Create<IFlexiBlockOptionsFactory>();
            mockFlexiBlockOptionsFactory.Setup(o => o.Create(dummyFlexiAlertOptions.Object, dummyBlockProcessor)).Returns(dummyFlexiAlertOptions.Object);
            Mock<IFlexiAlertsExtensionOptionsFactory> mockFlexiAlertsExtensionOptionsFactory = _mockRepository.Create<IFlexiAlertsExtensionOptionsFactory>();
            mockFlexiAlertsExtensionOptionsFactory.Setup(f => f.Create(dummyBlockProcessor)).Returns(mockFlexiAlertsExtensionOptions.Object);
            Mock<BlockParser<FlexiAlert>> dummyBlockParser = _mockRepository.Create<BlockParser<FlexiAlert>>();
            Mock<ExposedFlexiAlertFactory> mockTestSubject = CreateMockExposedFlexiAlertFactory(mockFlexiBlockOptionsFactory.Object, mockFlexiAlertsExtensionOptionsFactory.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.GetIconHtmlFragment(dummyFlexiAlertOptions.Object, mockFlexiAlertsExtensionOptions.Object)).Returns(dummyIconHtmlFragment);

            // Act
            FlexiAlert result = mockTestSubject.Object.ExposedCreateBlock(dummyBlockProcessor, dummyBlockParser.Object);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyIconHtmlFragment, result.IconHtmlFragment);
            Assert.Same(dummyFlexiAlertOptions.Object, result.FlexiBlockOptions);
            Assert.Same(dummyBlockParser.Object, result.Parser);
            Assert.Equal(dummyColumn, result.Column);
            Assert.Equal(dummySpanStart, result.Span.Start);
            Assert.Equal(dummySpanEnd, result.Span.End);
        }

        [Fact]
        public void GetIconHtmlFragment_ReturnsIconHtmlFragmentIfItIsNotNull()
        {
            // Arrange
            const string dummyIconHtmlFragment = "dummyIconHtmlFragment";
            Mock<IFlexiAlertOptions> mockFlexiAlertOptions = _mockRepository.Create<IFlexiAlertOptions>();
            mockFlexiAlertOptions.Setup(f => f.IconHtmlFragment).Returns(dummyIconHtmlFragment);
            FlexiAlertFactory testSubject = CreateFlexiAlertFactory();

            // Act
            string result = testSubject.GetIconHtmlFragment(mockFlexiAlertOptions.Object, null);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyIconHtmlFragment, result);
        }

        [Fact]
        public void GetIconHtmlFragment_ReturnsNullIfIconHtmlFragmentAndTypeAreBothNull()
        {
            // Arrange
            Mock<IFlexiAlertOptions> mockFlexiAlertOptions = _mockRepository.Create<IFlexiAlertOptions>();
            mockFlexiAlertOptions.Setup(f => f.IconHtmlFragment).Returns((string)null);
            mockFlexiAlertOptions.Setup(f => f.Type).Returns((string)null);
            FlexiAlertFactory testSubject = CreateFlexiAlertFactory();

            // Act
            string result = testSubject.GetIconHtmlFragment(mockFlexiAlertOptions.Object, null);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Null(result);
        }

        [Fact]
        public void GetIconHtmlFragment_ReturnsNullIfIconHtmlFragmentIsNullTypeIsNotNullButIconHtmlFragmentsDoesNotHaveType()
        {
            // Arrange
            const string dummyType = "dummyType";
            var dummyIconHtmlFragments = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
            Mock<IFlexiAlertOptions> mockFlexiAlertOptions = _mockRepository.Create<IFlexiAlertOptions>();
            mockFlexiAlertOptions.Setup(f => f.IconHtmlFragment).Returns((string)null);
            mockFlexiAlertOptions.Setup(f => f.Type).Returns(dummyType);
            Mock<IFlexiAlertsExtensionOptions> mockFlexiAlertsExtensionOptions = _mockRepository.Create<IFlexiAlertsExtensionOptions>();
            mockFlexiAlertsExtensionOptions.Setup(f => f.IconHtmlFragments).Returns(dummyIconHtmlFragments);
            FlexiAlertFactory testSubject = CreateFlexiAlertFactory();

            // Act
            string result = testSubject.GetIconHtmlFragment(mockFlexiAlertOptions.Object, mockFlexiAlertsExtensionOptions.Object);

            // Assert
            _mockRepository.VerifyAll();
            mockFlexiAlertOptions.Verify(f => f.Type, Times.Exactly(2));
            Assert.Null(result);
        }

        [Fact]
        public void GetIconHtmlFragment_ReturnsIconHtmlFragmentsValueIfIconHtmlFragmentIsNullTypeIsNotNullAndIconHtmlFragmentsHasType()
        {
            // Arrange
            const string dummyIconHtmlFragment = "dummyIconHtmlFragment";
            const string dummyType = "dummyType";
            var dummyIconHtmlFragments = new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { dummyType, dummyIconHtmlFragment } });
            Mock<IFlexiAlertOptions> mockFlexiAlertOptions = _mockRepository.Create<IFlexiAlertOptions>();
            mockFlexiAlertOptions.Setup(f => f.IconHtmlFragment).Returns((string)null);
            mockFlexiAlertOptions.Setup(f => f.Type).Returns(dummyType);
            Mock<IFlexiAlertsExtensionOptions> mockFlexiAlertsExtensionOptions = _mockRepository.Create<IFlexiAlertsExtensionOptions>();
            mockFlexiAlertsExtensionOptions.Setup(f => f.IconHtmlFragments).Returns(dummyIconHtmlFragments);
            FlexiAlertFactory testSubject = CreateFlexiAlertFactory();

            // Act
            string result = testSubject.GetIconHtmlFragment(mockFlexiAlertOptions.Object, mockFlexiAlertsExtensionOptions.Object);

            // Assert
            _mockRepository.VerifyAll();
            mockFlexiAlertOptions.Verify(f => f.Type, Times.Exactly(2));
            Assert.Equal(dummyIconHtmlFragment, result);
        }

        public class ExposedFlexiAlertFactory : FlexiAlertFactory
        {
            public ExposedFlexiAlertFactory(IFlexiBlockOptionsFactory flexiBlockOptionsFactory, IFlexiAlertsExtensionOptionsFactory flexiAlertsExtensionOptionsFactory) : base(flexiBlockOptionsFactory, flexiAlertsExtensionOptionsFactory)
            {
            }

            public FlexiAlert ExposedCreateBlock(BlockProcessor blockProcessor, BlockParser<FlexiAlert> blockParser = null)
            {
                return CreateBlock(blockProcessor, blockParser);
            }
        }

        private Mock<ExposedFlexiAlertFactory> CreateMockExposedFlexiAlertFactory(IFlexiBlockOptionsFactory flexiBlockOptionsFactory = null, IFlexiAlertsExtensionOptionsFactory flexiAlertsExtensionOptionsFactory = null)
        {
            return _mockRepository.Create<ExposedFlexiAlertFactory>(flexiBlockOptionsFactory, flexiAlertsExtensionOptionsFactory);
        }

        private FlexiAlertFactory CreateFlexiAlertFactory(IFlexiBlockOptionsFactory flexiBlockOptionsFactory = null, IFlexiAlertsExtensionOptionsFactory flexiAlertsExtensionOptionsFactory = null)
        {
            return new FlexiAlertFactory(flexiBlockOptionsFactory ?? _mockRepository.Create<IFlexiBlockOptionsFactory>().Object,
                flexiAlertsExtensionOptionsFactory ?? _mockRepository.Create<IFlexiAlertsExtensionOptionsFactory>().Object);
        }
    }
}
