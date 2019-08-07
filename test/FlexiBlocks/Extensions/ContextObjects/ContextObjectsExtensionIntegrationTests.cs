using Jering.Markdig.Extensions.FlexiBlocks.ContextObjects;
using Markdig;
using Markdig.Parsers;
using Markdig.Renderers;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.ContextObjects
{
    public class ContextObjectsExtensionIntegrationTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };
        private static readonly object _dummyKey = new object();

        [Fact]
        public void ContextObjectsExtension_ContextObjectsAddedToContextObjectsStoreWhileBuildingPipelineCanBeRetrieved()
        {
            // Arrange
            var dummyValue = new object();
            Mock<Verifier> mockVerifier = _mockRepository.Create<Verifier>();
            mockVerifier.Setup(v => v.Verify(dummyValue)); // Verifies that correct value is returned by ContextObjectsService in DummyParser.TryOpen
            var dummyServices = new ServiceCollection();
            dummyServices.
                AddContextObjects().
                AddSingleton<DummyExtension>().
                AddSingleton<DummyParser>().
                AddSingleton(mockVerifier.Object);
            ServiceProvider serviceProvider = dummyServices.BuildServiceProvider();
            ContextObjectsExtension testSubject = serviceProvider.GetRequiredService<ContextObjectsExtension>();
            testSubject.ContextObjectsStore.Add(_dummyKey, dummyValue); // Add context object to ContextObjectsStore
            var markdownPipelineBuilder = new MarkdownPipelineBuilder();
            markdownPipelineBuilder.Extensions.Add(testSubject);
            markdownPipelineBuilder.Extensions.Add(serviceProvider.GetRequiredService<DummyExtension>());
            MarkdownPipeline markdownPipeline = markdownPipelineBuilder.Build();

            // Act
            Markdown.ToHtml("dummyMarkdown", markdownPipeline);

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void ContextObjectsExtension_ContextObjectsAddedToMarkdownParserContextPropertiesCanBeRetrieved()
        {
            // Arrange
            var dummyValue = new object();
            Mock<Verifier> mockVerifier = _mockRepository.Create<Verifier>();
            mockVerifier.Setup(v => v.Verify(dummyValue)); // Verifies that correct value is returned by ContextObjectsService in DummyParser.TryOpen
            var dummyServices = new ServiceCollection();
            dummyServices.
                AddContextObjects().
                AddSingleton<DummyExtension>().
                AddSingleton<DummyParser>().
                AddSingleton(mockVerifier.Object);
            ServiceProvider serviceProvider = dummyServices.BuildServiceProvider();
            var markdownPipelineBuilder = new MarkdownPipelineBuilder();
            markdownPipelineBuilder.Extensions.Add(serviceProvider.GetRequiredService<ContextObjectsExtension>());
            markdownPipelineBuilder.Extensions.Add(serviceProvider.GetRequiredService<DummyExtension>());
            MarkdownPipeline markdownPipeline = markdownPipelineBuilder.Build();
            var markdownParserContext = new MarkdownParserContext();
            markdownParserContext.Properties.Add(_dummyKey, dummyValue); // Add context object to MarkdownParserContext
            var dummyStringWriter = new StringWriter();

            // Act
            Markdown.ToHtml("dummyMarkdown", dummyStringWriter, markdownPipeline, markdownParserContext);

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void ContextObjectsExtension_ContextObjectsAddedToMarkdownParserContextPropertiesTakePrecedenceOver()
        {
            // Arrange
            var dummyValue = new object();
            Mock<Verifier> mockVerifier = _mockRepository.Create<Verifier>();
            mockVerifier.Setup(v => v.Verify(dummyValue)); // Verifies that correct value is returned by ContextObjectsService in DummyParser.TryOpen
            var dummyServices = new ServiceCollection();
            dummyServices.
                AddContextObjects().
                AddSingleton<DummyExtension>().
                AddSingleton<DummyParser>().
                AddSingleton(mockVerifier.Object);
            ServiceProvider serviceProvider = dummyServices.BuildServiceProvider();
            ContextObjectsExtension testSubject = serviceProvider.GetRequiredService<ContextObjectsExtension>();
            testSubject.ContextObjectsStore.Add(_dummyKey, new object()); // Add context object to ContextObjectsStore
            var markdownPipelineBuilder = new MarkdownPipelineBuilder();
            markdownPipelineBuilder.Extensions.Add(testSubject);
            markdownPipelineBuilder.Extensions.Add(serviceProvider.GetRequiredService<DummyExtension>());
            MarkdownPipeline markdownPipeline = markdownPipelineBuilder.Build();
            var markdownParserContext = new MarkdownParserContext();
            markdownParserContext.Properties.Add(_dummyKey, dummyValue); // Add context object to MarkdownParserContext, this object should take precedence
            var dummyStringWriter = new StringWriter();

            // Act
            Markdown.ToHtml("dummyMarkdown", dummyStringWriter, markdownPipeline, markdownParserContext);

            // Assert
            _mockRepository.VerifyAll();
        }

        public class Verifier
        {
            public virtual void Verify(object obj)
            {
                // Do nothing
            }
        }

        private class DummyParser : BlockParser
        {
            private readonly IContextObjectsService _contextObjectsService;
            private readonly Verifier _verifier;

            public DummyParser(IContextObjectsService contextObjectsService, Verifier verifier)
            {
                _contextObjectsService = contextObjectsService;
                _verifier = verifier;
            }

            public override BlockState TryOpen(BlockProcessor processor)
            {
                _contextObjectsService.TryGetContextObject(_dummyKey, processor, out object value);

                _verifier.Verify(value);

                return BlockState.None;
            }
        }

        private class DummyExtension : IMarkdownExtension
        {
            private readonly DummyParser _dummyParser;

            public DummyExtension(DummyParser dummyParser)
            {
                _dummyParser = dummyParser;
            }

            public void Setup(MarkdownPipelineBuilder pipeline)
            {
                pipeline.BlockParsers.Insert(0, _dummyParser); // Needs to be before paragraph parser, which is a "catch all" parser consuming all lines
            }

            public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
            {
                // Do nothing
            }
        }

        private ContextObjectsExtension CreateContextObjectsExtension(ContextObjectsStore contextObjectsStore = null)
        {
            return new ContextObjectsExtension(contextObjectsStore);
        }
    }
}