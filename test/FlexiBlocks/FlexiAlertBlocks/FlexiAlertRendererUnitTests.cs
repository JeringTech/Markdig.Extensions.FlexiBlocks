using Jering.Markdig.Extensions.FlexiBlocks.Alerts;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Moq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.Alerts
{
    public class FlexiAlertRendererUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void WriteBlock_OnlyWritesChildrenIfEnableHtmlForFlexiAlertIsFalse()
        {
            // Arrange
            const string dummyChildText = "dummyChildText";
            var dummyContainerInline = new ContainerInline();
            dummyContainerInline.AppendChild(new LiteralInline(dummyChildText));
            var dummyParagraphBlock = new ParagraphBlock() { Inline = dummyContainerInline };
            var dummyFlexiAlert = new FlexiAlert(null, _mockRepository.Create<IFlexiAlertOptions>().Object, null);
            dummyFlexiAlert.Add(dummyParagraphBlock);

            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter)
            {
                EnableHtmlForBlock = false
            };
            ExposedFlexiAlertRenderer testSubject = CreateExposedFlexiAlertRenderer();

            // Act
            testSubject.ExposedWriteBlock(dummyHtmlRenderer, dummyFlexiAlert);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(dummyChildText + "\n", result, ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void WriteBlock_WritesWithoutDefaultClassesIfBlockNameIsNull()
        {
            // Arrange
            var dummyHtmlRenderer = new HtmlRenderer(new StringWriter());
            Mock<IFlexiAlertOptions> mockFlexiAlertOptions = _mockRepository.Create<IFlexiAlertOptions>();
            mockFlexiAlertOptions.Setup(f => f.BlockName).Returns((string)null);
            var dummyFlexiAlert = new FlexiAlert(null, mockFlexiAlertOptions.Object, null);
            Mock<ExposedFlexiAlertRenderer> mockTestSubject = CreateMockExposedFlexiAlertRenderer();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.WriteWithoutDefaultClasses(dummyHtmlRenderer, dummyFlexiAlert));

            // Act
            mockTestSubject.Object.ExposedWriteBlock(dummyHtmlRenderer, dummyFlexiAlert);

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void WriteBlock_WritesWithDefaultClassesIfBlockNameIsNotNull()
        {
            // Arrange
            const string dummyBlockName = "dummyBlockName";
            Mock<IFlexiAlertOptions> mockFlexiAlertOptions = _mockRepository.Create<IFlexiAlertOptions>();
            mockFlexiAlertOptions.Setup(f => f.BlockName).Returns(dummyBlockName);
            var dummyFlexiAlert = new FlexiAlert(null, mockFlexiAlertOptions.Object, null);
            var dummyHtmlRenderer = new HtmlRenderer(new StringWriter());
            Mock<ExposedFlexiAlertRenderer> mockTestSubject = CreateMockExposedFlexiAlertRenderer();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.WriteWithDefaultClasses(dummyHtmlRenderer, dummyFlexiAlert));

            // Act
            mockTestSubject.Object.ExposedWriteBlock(dummyHtmlRenderer, dummyFlexiAlert);

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void WriteWithoutDefaultClasses_WritesAttributesIfItsNotNull()
        {
            // Arrange
            const string dummyAttribute = "dummyAttribute";
            const string dummyAttributeValue = "dummyAttributeValue";
            var dummyAttributes = new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { dummyAttribute, dummyAttributeValue } });
            Mock<IFlexiAlertOptions> mockFlexiAlertOptions = _mockRepository.Create<IFlexiAlertOptions>();
            mockFlexiAlertOptions.Setup(f => f.Attributes).Returns(dummyAttributes);
            var dummyFlexiAlert = new FlexiAlert(null, mockFlexiAlertOptions.Object, null);

            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter); // Note that markdig changes dummyStringWriter.NewLine to '\n'
            FlexiAlertRenderer testSubject = CreateFlexiAlertRenderer();

            // Act
            testSubject.WriteWithoutDefaultClasses(dummyHtmlRenderer, dummyFlexiAlert);
            string result = dummyStringWriter.ToString();

            // Assert
            _mockRepository.VerifyAll();
            Assert.
                Equal($"<div {dummyAttribute}=\"{dummyAttributeValue}\">\n<div>\n</div>\n</div>\n", result);
        }

        [Fact]
        public void WriteWithoutDefaultClasses_WritesIconHtmlFragmentIfItIsNotNull()
        {
            // Arrange
            const string dummyIconHtmlFragment = "dummyIconHtmlFragment";
            Mock<IFlexiAlertOptions> mockFlexiAlertOptions = _mockRepository.Create<IFlexiAlertOptions>();
            mockFlexiAlertOptions.Setup(f => f.Attributes).Returns((ReadOnlyDictionary<string, string>)null);
            var dummyFlexiAlert = new FlexiAlert(dummyIconHtmlFragment, mockFlexiAlertOptions.Object, null);
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter); // Note that markdig changes dummyStringWriter.NewLine to '\n'
            FlexiAlertRenderer testSubject = CreateFlexiAlertRenderer();

            // Act
            testSubject.WriteWithoutDefaultClasses(dummyHtmlRenderer, dummyFlexiAlert);
            string result = dummyStringWriter.ToString();

            // Assert
            _mockRepository.VerifyAll();
            Assert.
                Equal($"<div>\n{dummyIconHtmlFragment}\n<div>\n</div>\n</div>\n", result);
        }

        [Fact]
        public void WriteWithoutDefaultClasses_WritesChildren()
        {
            // Arrange
            const string dummyChildText = "dummyChildText";
            var dummyContainerInline = new ContainerInline();
            dummyContainerInline.AppendChild(new LiteralInline(dummyChildText));
            var dummyParagraphBlock = new ParagraphBlock() { Inline = dummyContainerInline };
            Mock<IFlexiAlertOptions> mockFlexiAlertOptions = _mockRepository.Create<IFlexiAlertOptions>();
            mockFlexiAlertOptions.Setup(f => f.Attributes).Returns((ReadOnlyDictionary<string, string>)null);
            var dummyFlexiAlert = new FlexiAlert(null, mockFlexiAlertOptions.Object, null);
            dummyFlexiAlert.Add(dummyParagraphBlock);
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter); // Note that markdig changes dummyStringWriter.NewLine to '\n'
            FlexiAlertRenderer testSubject = CreateFlexiAlertRenderer();

            // Act
            testSubject.Write(dummyHtmlRenderer, dummyFlexiAlert);
            string result = dummyStringWriter.ToString();

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal($"<div>\n<div>\n<p>{dummyChildText}</p>\n</div>\n</div>\n", result);
        }

        [Fact]
        public void WriteWithDefaultClasses_WritesAttributesIfItsNotNull()
        {
            // Arrange
            const string dummyBlockName = "dummyBlockName";
            const string dummyAttribute = "dummyAttribute";
            const string dummyAttributeValue = "dummyAttributeValue";
            var dummyAttributes = new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { dummyAttribute, dummyAttributeValue } });
            Mock<IFlexiAlertOptions> mockFlexiAlertOptions = _mockRepository.Create<IFlexiAlertOptions>();
            mockFlexiAlertOptions.Setup(f => f.BlockName).Returns(dummyBlockName);
            mockFlexiAlertOptions.Setup(f => f.Attributes).Returns(dummyAttributes);
            mockFlexiAlertOptions.Setup(f => f.Type).Returns((string)null);
            var dummyFlexiAlert = new FlexiAlert(null, mockFlexiAlertOptions.Object, null);
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter); // Note that markdig changes dummyStringWriter.NewLine to '\n'
            FlexiAlertRenderer testSubject = CreateFlexiAlertRenderer();

            // Act
            testSubject.WriteWithDefaultClasses(dummyHtmlRenderer, dummyFlexiAlert);
            string result = dummyStringWriter.ToString();

            // Assert
            _mockRepository.VerifyAll();
            Assert.
                Equal($"<div {dummyAttribute}=\"{dummyAttributeValue}\" class=\"{dummyBlockName}\">\n<div class=\"{dummyBlockName}__content\">\n</div>\n</div>\n", result);
        }

        [Fact]
        public void WriteWithDefaultClasses_WritesTypeModifierClassIfTypeIsNotNull()
        {
            // Arrange
            const string dummyBlockName = "dummyBlockName";
            const string dummyType = "dummyType";
            Mock<IFlexiAlertOptions> mockFlexiAlertOptions = _mockRepository.Create<IFlexiAlertOptions>();
            mockFlexiAlertOptions.Setup(f => f.BlockName).Returns(dummyBlockName);
            mockFlexiAlertOptions.Setup(f => f.Attributes).Returns((ReadOnlyDictionary<string, string>)null);
            mockFlexiAlertOptions.Setup(f => f.Type).Returns(dummyType);
            var dummyFlexiAlert = new FlexiAlert(null, mockFlexiAlertOptions.Object, null);
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter); // Note that markdig changes dummyStringWriter.NewLine to '\n'
            FlexiAlertRenderer testSubject = CreateFlexiAlertRenderer();

            // Act
            testSubject.WriteWithDefaultClasses(dummyHtmlRenderer, dummyFlexiAlert);
            string result = dummyStringWriter.ToString();

            // Assert
            _mockRepository.VerifyAll();
            Assert.
                Equal($"<div class=\"{dummyBlockName} {dummyBlockName}_{dummyType}\">\n<div class=\"{dummyBlockName}__content\">\n</div>\n</div>\n", result);
        }

        [Fact]
        public void WriteWithDefaultClasses_WritesIconHtmlFragmentIfItsNotNull()
        {
            // Arrange
            const string dummyBlockName = "dummyBlockName";
            const string dummyIconHtmlFragment = "<dummyIconHtmlFragment>";
            Mock<IFlexiAlertOptions> mockFlexiAlertOptions = _mockRepository.Create<IFlexiAlertOptions>();
            mockFlexiAlertOptions.Setup(f => f.BlockName).Returns(dummyBlockName);
            mockFlexiAlertOptions.Setup(f => f.Attributes).Returns((ReadOnlyDictionary<string, string>)null);
            mockFlexiAlertOptions.Setup(f => f.Type).Returns((string)null);
            var dummyFlexiAlert = new FlexiAlert(dummyIconHtmlFragment, mockFlexiAlertOptions.Object, null);
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter); // Note that markdig changes dummyStringWriter.NewLine to '\n'
            FlexiAlertRenderer testSubject = CreateFlexiAlertRenderer();

            // Act
            testSubject.WriteWithDefaultClasses(dummyHtmlRenderer, dummyFlexiAlert);
            string result = dummyStringWriter.ToString();

            // Assert
            _mockRepository.VerifyAll();
            Assert.
                Equal($"<div class=\"{dummyBlockName}\">\n<dummyIconHtmlFragment class=\"{dummyBlockName}__icon\">\n<div class=\"{dummyBlockName}__content\">\n</div>\n</div>\n", result);
        }

        public class ExposedFlexiAlertRenderer : FlexiAlertRenderer
        {
            public void ExposedWriteBlock(HtmlRenderer htmlRenderer, FlexiAlert flexiAlert)
            {
                WriteBlock(htmlRenderer, flexiAlert);
            }
        }

        private Mock<ExposedFlexiAlertRenderer> CreateMockExposedFlexiAlertRenderer()
        {
            return _mockRepository.Create<ExposedFlexiAlertRenderer>();
        }

        private ExposedFlexiAlertRenderer CreateExposedFlexiAlertRenderer()
        {
            return new ExposedFlexiAlertRenderer();
        }

        private FlexiAlertRenderer CreateFlexiAlertRenderer()
        {
            return new FlexiAlertRenderer();
        }
    }
}
