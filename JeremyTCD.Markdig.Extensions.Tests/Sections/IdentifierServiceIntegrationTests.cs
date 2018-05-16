using JeremyTCD.Markdig.Extensions.Sections;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace JeremyTCD.Markdig.Extensions.Tests.Sections
{
    public class IdentifierServiceIntegrationTests
    {
        [Fact]
        public void HeadingBlockOnProcessInlinesEnd_ThrowsExceptionIfHeadingBlocksParentIsNotASectionBlock()
        {
            // Arrange
            HeadingBlock dummyHeadingBlock = new HeadingBlock(null);
            InlineProcessor dummyInlineProcessor = MarkdigTypesFactory.CreateInlineProcessor();
            dummyInlineProcessor.ProcessInlineLeaf(dummyHeadingBlock); // Sets dummyInlineProcessor.Block
            IdentifierService identifierService = new IdentifierService();

            // Act and Assert
            Assert.Throws<InvalidOperationException>(() => identifierService.HeadingBlockOnProcessInlinesEnd(dummyInlineProcessor, null));
        }

        [Fact]
        public void HeadingBlockOnProcessInlinesEnd_DoesNothingIfSectionBlockAlreadyHasAnID()
        {
            // Arrange
            string dummyID = "dummyID";
            SectionBlockOptions dummySectionBlockOptions = new SectionBlockOptions();
            dummySectionBlockOptions.Attributes.Add("id", dummyID);
            HeadingBlock dummyHeadingBlock = new HeadingBlock(null);
            SectionBlock dummySectionBlock = new SectionBlock(null) { SectionBlockOptions = dummySectionBlockOptions };
            dummySectionBlock.Add(dummyHeadingBlock); // Sets dummyHeadingBlock.Parent to dummySectionBlock
            InlineProcessor dummyInlineProcessor = MarkdigTypesFactory.CreateInlineProcessor();
            dummyInlineProcessor.ProcessInlineLeaf(dummyHeadingBlock); // Sets dummyInlineProcessor.Block
            IdentifierService identifierService = new IdentifierService();

            // Act
            identifierService.HeadingBlockOnProcessInlinesEnd(dummyInlineProcessor, null);

            // Assert
            Assert.Equal(dummyID, dummySectionBlockOptions.Attributes["id"]); // id unchanged
        }

        [Fact]
        public void HeadingBlockOnProcessInlinesEnd_IfSuccessfulAddsIDToDuplicateCheckingMapAndToSectionBlocksAttributes()
        {
            // Arrange
            string dummyID = "dummyID";
            HeadingBlock dummyHeadingBlock = new HeadingBlock(null)
            {
                Lines = new StringLineGroup(dummyID)
            };
            SectionBlockOptions dummySectionBlockOptions = new SectionBlockOptions();
            SectionBlock dummySectionBlock = new SectionBlock(null) { SectionBlockOptions = dummySectionBlockOptions };
            dummySectionBlock.Add(dummyHeadingBlock); // Sets dummyHeadingBlock.Parent to dummySectionBlock
            InlineProcessor dummyInlineProcessor = MarkdigTypesFactory.CreateInlineProcessor();
            dummyInlineProcessor.ProcessInlineLeaf(dummyHeadingBlock); // Set dummyInlineProcessor.Block and creates InlineBlocks
            IdentifierService identifierService = new IdentifierService();

            // Act
            identifierService.HeadingBlockOnProcessInlinesEnd(dummyInlineProcessor, null);

            // Assert
            Dictionary<string, int> resultIdentifiers = dummyInlineProcessor.Document.GetData(IdentifierService.SECTION_IDS_KEY) as Dictionary<string, int>;
            Assert.NotNull(resultIdentifiers);
            Assert.True(resultIdentifiers.TryGetValue(dummyID.ToLower(), out int numDuplicates)); // IDs are converted to kebab case (all lowercase, words seperated by dashes)
            Assert.Equal(0, numDuplicates);
            dummySectionBlockOptions.Attributes.TryGetValue("id", out string resultID);
            Assert.Equal(dummyID.ToLower(), resultID);
        }

        [Fact]
        public void HeadingBlockOnProcessInlinesEnd_IfSuccessfulButHeadingBlockTextIsNullOrWhitespaceUsesSectionAsID()
        {
            // Arrange
            string dummyID = "";
            HeadingBlock dummyHeadingBlock = new HeadingBlock(null)
            {
                Lines = new StringLineGroup(dummyID)
            };
            SectionBlockOptions dummySectionBlockOptions = new SectionBlockOptions();
            SectionBlock dummySectionBlock = new SectionBlock(null) { SectionBlockOptions = dummySectionBlockOptions };
            dummySectionBlock.Add(dummyHeadingBlock); // Sets dummyHeadingBlock.Parent to dummySectionBlock
            InlineProcessor dummyInlineProcessor = MarkdigTypesFactory.CreateInlineProcessor();
            dummyInlineProcessor.ProcessInlineLeaf(dummyHeadingBlock); // Set dummyInlineProcessor.Block and creates InlineBlocks
            IdentifierService identifierService = new IdentifierService();

            // Act
            identifierService.HeadingBlockOnProcessInlinesEnd(dummyInlineProcessor, null);

            // Assert
            dummySectionBlockOptions.Attributes.TryGetValue("id", out string resultID);
            Assert.Equal("section", resultID);
        }

        [Fact]
        public void HeadingBlockOnProcessInlinesEnd_IfSuccesfulButIDIsInUseAppendsIntegerToID()
        {
            // Arrange
            string dummyID = "dummyID";
            HeadingBlock dummyHeadingBlock = new HeadingBlock(null)
            {
                Lines = new StringLineGroup(dummyID)
            };
            SectionBlockOptions dummySectionBlockOptions = new SectionBlockOptions();
            SectionBlock dummySectionBlock = new SectionBlock(null) { SectionBlockOptions = dummySectionBlockOptions };
            dummySectionBlock.Add(dummyHeadingBlock); // Sets dummyHeadingBlock.Parent to dummySectionBlock
            InlineProcessor dummyInlineProcessor = MarkdigTypesFactory.CreateInlineProcessor();
            dummyInlineProcessor.ProcessInlineLeaf(dummyHeadingBlock); // Set dummyInlineProcessor.Block and creates InlineBlocks
            IdentifierService identifierService = new IdentifierService();

            // Act
            identifierService.HeadingBlockOnProcessInlinesEnd(dummyInlineProcessor, null);
            dummySectionBlockOptions.Attributes.Clear(); // HeadingBlockOnProcessInlinesEnd does nothing if block already has an id attribute
            identifierService.HeadingBlockOnProcessInlinesEnd(dummyInlineProcessor, null);
            string resultSecondID = dummySectionBlockOptions.Attributes["id"];
            dummySectionBlockOptions.Attributes.Clear();
            identifierService.HeadingBlockOnProcessInlinesEnd(dummyInlineProcessor, null);
            string resultThirdID = dummySectionBlockOptions.Attributes["id"];

            // Assert
            Dictionary<string, int> resultIdentifiers = dummyInlineProcessor.Document.GetData(IdentifierService.SECTION_IDS_KEY) as Dictionary<string, int>;
            Assert.NotNull(resultIdentifiers);
            resultIdentifiers.TryGetValue(dummyID.ToLower(), out int numDuplicates);
            Assert.Equal(2, numDuplicates);
            Assert.Equal("dummyid-1", resultSecondID);
            Assert.Equal("dummyid-2", resultThirdID);
        }
    }
}
