using JeremyTCD.Markdig.Extensions.Sections;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Collections.Generic;
using Xunit;

namespace JeremyTCD.Markdig.Extensions.Tests.Sections
{
    public class AutoLinkServiceIntegrationTests
    {
        [Fact]
        public void SetupAutoLink_CreatesSectionLinkReferenceDefinitionAndAddsItToMapOfSectionLinkReferenceDefinitions()
        {
            // Arrange
            string dummyHeadingText = "dummyHeadingText";
            HeadingBlock dummyHeadingBlock = new HeadingBlock(null)
            {
                Lines = new StringLineGroup(dummyHeadingText)
            };
            SectionBlock dummySectionBlock = new SectionBlock(null);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            AutoLinkService autoLinkService = new AutoLinkService();

            // Act
            autoLinkService.SetupAutoLink(dummyBlockProcessor, dummySectionBlock, dummyHeadingBlock);

            // Assert
            Dictionary<string, SectionLinkReferenceDefinition> resultSlrds = dummyBlockProcessor.Document.GetData(AutoLinkService.AUTO_LINKS_KEY) as Dictionary<string, SectionLinkReferenceDefinition>;
            Assert.NotNull(resultSlrds);
            resultSlrds.TryGetValue(dummyHeadingText, out SectionLinkReferenceDefinition resultSlrd);
            Assert.NotNull(resultSlrd);
            Assert.Equal(dummySectionBlock, resultSlrd.SectionBlock);
        }

        [Fact]
        public void DocumentOnProcessInlinesBegin_AddsSectionLinkReferenceDefinitionsWithoutOverridingExistingLinkReferenceDefinitions()
        {
            // Arrange
            string dummySlrd1Key = "dummySlrd1Key";
            SectionLinkReferenceDefinition dummySlrd1 = new SectionLinkReferenceDefinition();
            string dummySlrd2Key = "dummySlrd2Key";
            SectionLinkReferenceDefinition dummySlrd2 = new SectionLinkReferenceDefinition();
            Dictionary<string, SectionLinkReferenceDefinition> dummySlrds = new Dictionary<string, SectionLinkReferenceDefinition>()
            {
                { dummySlrd1Key, dummySlrd1 },
                { dummySlrd2Key, dummySlrd2 }
            };
            LinkReferenceDefinition dummyLfd = new LinkReferenceDefinition();
            MarkdownDocument dummyDocument = new MarkdownDocument();
            dummyDocument.SetData(AutoLinkService.AUTO_LINKS_KEY, dummySlrds);
            dummyDocument.SetLinkReferenceDefinition(dummySlrd1Key, dummyLfd);
            InlineProcessor dummyInlineProcessor = MarkdigTypesFactory.CreateInlineProcessor(markdownDocument: dummyDocument);
            AutoLinkService autoLinkService = new AutoLinkService();

            // Act
            autoLinkService.DocumentOnProcessInlinesBegin(dummyInlineProcessor, null);

            // Assert
            Dictionary<string, LinkReferenceDefinition> resultLinks = dummyDocument.GetLinkReferenceDefinitions().Links;
            Assert.Equal(2, resultLinks.Count);
            Assert.Same(dummyLfd, resultLinks[dummySlrd1Key]); // Not overriden
            Assert.Same(dummySlrd2, resultLinks[dummySlrd2Key]); // Added
            Assert.False(resultLinks.ContainsKey(AutoLinkService.AUTO_LINKS_KEY));
        }

        [Fact]
        public void CreateLinkInline_CreatesLinkInline()
        {
            // Arrange
            string dummyID = "dummyID";
            string dummyTitle = "dummyTitle";
            SectionBlock dummySectionBlock = new SectionBlock(null)
            {
                SectionBlockOptions = new SectionBlockOptions() { Attributes = new Dictionary<string, string> { { "id", dummyID } } }
            };
            SectionLinkReferenceDefinition dummySlrd = new SectionLinkReferenceDefinition()
            {
                SectionBlock = dummySectionBlock,
                Title = dummyTitle
            };
            AutoLinkService autoLinkService = new AutoLinkService();

            // Act
            LinkInline result = autoLinkService.CreateLinkInline(null, dummySlrd, null) as LinkInline;

            // Assert
            Assert.NotNull(result);
            Assert.Equal($"#{dummyID}", result.GetDynamicUrl());
            Assert.Equal(dummyTitle, result.Title);
        }
    }
}
