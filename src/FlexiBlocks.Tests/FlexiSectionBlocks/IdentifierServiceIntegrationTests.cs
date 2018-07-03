using Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using System;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiSectionBlocks
{
    public class IdentifierServiceIntegrationTests
    {
        [Fact]
        public void HeadingBlockOnProcessInlinesEnd_ThrowsExceptionIfHeadingBlocksParentIsNotAFlexiSectionBlock()
        {
            // Arrange
            var dummyHeadingBlock = new HeadingBlock(null);
            InlineProcessor dummyInlineProcessor = MarkdigTypesFactory.CreateInlineProcessor();
            dummyInlineProcessor.ProcessInlineLeaf(dummyHeadingBlock); // Sets dummyInlineProcessor.Block
            var identifierService = new IdentifierService();

            // Act and Assert
            Assert.Throws<InvalidOperationException>(() => identifierService.HeadingBlockOnProcessInlinesEnd(dummyInlineProcessor, null));
        }

        [Fact]
        public void HeadingBlockOnProcessInlinesEnd_DoesNothingIfFlexiSectionBlockAlreadyHasAnID()
        {
            // Arrange
            const string dummyID = "dummyID";
            var dummyFlexiSectionBlockOptions = new FlexiSectionBlockOptions();
            dummyFlexiSectionBlockOptions.Attributes.Add("id", dummyID);
            var dummyHeadingBlock = new HeadingBlock(null);
            var dummyFlexiSectionBlock = new FlexiSectionBlock(null) { FlexiSectionBlockOptions = dummyFlexiSectionBlockOptions };
            dummyFlexiSectionBlock.Add(dummyHeadingBlock); // Sets dummyHeadingBlock.Parent to dummyFlexiSectionBlock
            InlineProcessor dummyInlineProcessor = MarkdigTypesFactory.CreateInlineProcessor();
            dummyInlineProcessor.ProcessInlineLeaf(dummyHeadingBlock); // Sets dummyInlineProcessor.Block
            var identifierService = new IdentifierService();

            // Act
            identifierService.HeadingBlockOnProcessInlinesEnd(dummyInlineProcessor, null);

            // Assert
            Assert.Equal(dummyID, dummyFlexiSectionBlockOptions.Attributes["id"]); // id unchanged
        }

        [Fact]
        public void HeadingBlockOnProcessInlinesEnd_IfSuccessfulAddsIDToDuplicateCheckingMapAndToFlexiSectionBlocksAttributes()
        {
            // Arrange
            const string dummyID = "dummyID";
            var dummyHeadingBlock = new HeadingBlock(null)
            {
                Lines = new StringLineGroup(dummyID)
            };
            var dummyFlexiSectionBlockOptions = new FlexiSectionBlockOptions();
            var dummyFlexiSectionBlock = new FlexiSectionBlock(null) { FlexiSectionBlockOptions = dummyFlexiSectionBlockOptions };
            dummyFlexiSectionBlock.Add(dummyHeadingBlock); // Sets dummyHeadingBlock.Parent to dummyFlexiSectionBlock
            InlineProcessor dummyInlineProcessor = MarkdigTypesFactory.CreateInlineProcessor();
            dummyInlineProcessor.ProcessInlineLeaf(dummyHeadingBlock); // Set dummyInlineProcessor.Block and creates InlineBlocks
            var identifierService = new IdentifierService();

            // Act
            identifierService.HeadingBlockOnProcessInlinesEnd(dummyInlineProcessor, null);

            // Assert
            var resultIdentifiers = dummyInlineProcessor.Document.GetData(IdentifierService.SECTION_IDS_KEY) as Dictionary<string, int>;
            Assert.NotNull(resultIdentifiers);
            Assert.True(resultIdentifiers.TryGetValue(dummyID.ToLower(), out int numDuplicates)); // IDs are converted to kebab case (all lowercase, words seperated by dashes)
            Assert.Equal(0, numDuplicates);
            dummyFlexiSectionBlockOptions.Attributes.TryGetValue("id", out string resultID);
            Assert.Equal(dummyID.ToLower(), resultID);
        }

        [Fact]
        public void HeadingBlockOnProcessInlinesEnd_IfSuccessfulButHeadingBlockTextIsNullOrWhitespaceUsesSectionAsID()
        {
            // Arrange
            const string dummyID = "";
            var dummyHeadingBlock = new HeadingBlock(null)
            {
                Lines = new StringLineGroup(dummyID)
            };
            var dummyFlexiSectionBlockOptions = new FlexiSectionBlockOptions();
            var dummyFlexiSectionBlock = new FlexiSectionBlock(null) { FlexiSectionBlockOptions = dummyFlexiSectionBlockOptions };
            dummyFlexiSectionBlock.Add(dummyHeadingBlock); // Sets dummyHeadingBlock.Parent to dummyFlexiSectionBlock
            InlineProcessor dummyInlineProcessor = MarkdigTypesFactory.CreateInlineProcessor();
            dummyInlineProcessor.ProcessInlineLeaf(dummyHeadingBlock); // Set dummyInlineProcessor.Block and creates InlineBlocks
            var identifierService = new IdentifierService();

            // Act
            identifierService.HeadingBlockOnProcessInlinesEnd(dummyInlineProcessor, null);

            // Assert
            dummyFlexiSectionBlockOptions.Attributes.TryGetValue("id", out string resultID);
            Assert.Equal("section", resultID);
        }

        [Fact]
        public void HeadingBlockOnProcessInlinesEnd_IfSuccesfulButIDIsInUseAppendsIntegerToID()
        {
            // Arrange
            const string dummyID = "dummyID";
            var dummyHeadingBlock = new HeadingBlock(null)
            {
                Lines = new StringLineGroup(dummyID)
            };
            var dummyFlexiSectionBlockOptions = new FlexiSectionBlockOptions();
            var dummyFlexiSectionBlock = new FlexiSectionBlock(null) { FlexiSectionBlockOptions = dummyFlexiSectionBlockOptions };
            dummyFlexiSectionBlock.Add(dummyHeadingBlock); // Sets dummyHeadingBlock.Parent to dummyFlexiSectionBlock
            InlineProcessor dummyInlineProcessor = MarkdigTypesFactory.CreateInlineProcessor();
            dummyInlineProcessor.ProcessInlineLeaf(dummyHeadingBlock); // Set dummyInlineProcessor.Block and creates InlineBlocks
            var identifierService = new IdentifierService();

            // Act
            identifierService.HeadingBlockOnProcessInlinesEnd(dummyInlineProcessor, null);
            dummyFlexiSectionBlockOptions.Attributes.Clear(); // HeadingBlockOnProcessInlinesEnd does nothing if block already has an id attribute
            identifierService.HeadingBlockOnProcessInlinesEnd(dummyInlineProcessor, null);
            string resultSecondID = dummyFlexiSectionBlockOptions.Attributes["id"];
            dummyFlexiSectionBlockOptions.Attributes.Clear();
            identifierService.HeadingBlockOnProcessInlinesEnd(dummyInlineProcessor, null);
            string resultThirdID = dummyFlexiSectionBlockOptions.Attributes["id"];

            // Assert
            var resultIdentifiers = dummyInlineProcessor.Document.GetData(IdentifierService.SECTION_IDS_KEY) as Dictionary<string, int>;
            Assert.NotNull(resultIdentifiers);
            resultIdentifiers.TryGetValue(dummyID.ToLower(), out int numDuplicates);
            Assert.Equal(2, numDuplicates);
            Assert.Equal("dummyid-1", resultSecondID);
            Assert.Equal("dummyid-2", resultThirdID);
        }
    }
}
