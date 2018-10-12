using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks
{
    /// <summary>
    /// A parser that creates <see cref="FlexiSectionBlock"/>s from markdown.
    /// </summary>
    public class FlexiSectionBlockParser : FlexiBlockParser
    {
        internal const string OPEN_FLEXI_SECTION_BLOCKS_KEY = "openFlexiSectionBlocksKey";
        internal const string SECTION_IDS_KEY = "sectionIDsKey";
        internal const string SECTION_LINK_REFERENCE_DEFINITIONS_KEY = "sectionLinkReferenceDefinitionsKey";
        private readonly FlexiSectionBlocksExtensionOptions _extensionOptions;
        private readonly IFlexiOptionsBlockService _flexiOptionsBlockService;

        /// <summary>
        /// Creates a <see cref="FlexiSectionBlockParser"/> instance.
        /// </summary>
        /// <param name="extensionOptionsAccessor">The accessor for <see cref="FlexiSectionBlocksExtensionOptions"/>.</param>
        /// <param name="flexiOptionsBlockService">The service that will handle populating of <see cref="FlexiSectionBlockOptions"/>.</param>
        public FlexiSectionBlockParser(IOptions<FlexiSectionBlocksExtensionOptions> extensionOptionsAccessor,
            IFlexiOptionsBlockService flexiOptionsBlockService)
        {
            OpeningCharacters = new[] { '#' };

            _extensionOptions = extensionOptionsAccessor?.Value ?? throw new ArgumentNullException(nameof(extensionOptionsAccessor));
            _flexiOptionsBlockService = flexiOptionsBlockService ?? throw new ArgumentNullException(nameof(flexiOptionsBlockService));
        }

        /// <summary>
        /// Opens a <see cref="FlexiSectionBlock"/> if a line begins with 0 to 3 spaces followed by 1-6 unescaped "#" characters followed by a space of the end of the line.
        /// </summary>
        /// <param name="processor">The block processor for the document that contains a line with first non-white-space character "#".</param>
        /// <returns>
        /// <see cref="BlockState.None"/> if the current line has code indent or if the current line does not start with the expected characters.
        /// <see cref="BlockState.Break"/> if a <see cref="FlexiSectionBlock"/> is opened.
        /// </returns>
        /// <exception cref="FlexiBlocksException">Thrown if <see cref="FlexiSectionBlockOptions.ClassFormat" /> is not a valid format.</exception>
        protected override BlockState TryOpenFlexiBlock(BlockProcessor processor)
        {
            if (processor == null)
            {
                throw new ArgumentNullException(nameof(processor));
            }

            if (processor.IsCodeIndent)
            {
                return BlockState.None;
            }

            FlexiSectionBlock flexiSectionBlock = TryCreateFlexiSectionBlock(processor);

            if (flexiSectionBlock == null)
            {
                return BlockState.None;
            }

            processor.NewBlocks.Push(flexiSectionBlock);

            // Setup
            SetupFlexiSectionBlock(processor, flexiSectionBlock);

            // Update open FlexiSectionBlocks
            UpdateOpenFlexiSectionBlocks(processor, flexiSectionBlock);

            return BlockState.ContinueDiscard;
        }

        /// <summary>
        /// Always continues.
        /// </summary>
        /// <param name="processor">The block processor for the <see cref="FlexiSectionBlock"/> to try and continue.</param>
        /// <param name="block">The <see cref="FlexiSectionBlock"/> to try and continue.</param>
        /// <returns>
        /// <see cref="BlockState.Continue"/>.
        /// </returns>
        protected override BlockState TryContinueFlexiBlock(BlockProcessor processor, Block block)
        {
            // If BlockState.Skip is returned, this parser ignores the line, allowing other blocks to see if they can be continued. Note that returning BlockState.Skip 
            // will result in the block being closed by default, so we have to manually set IsOpen to true. 
            // 
            // It is important that BlockState.Continue isn't returned, otherwise, Markdig calls BlockProcessor.RestartIndext(), effectively consuming
            // this line's leading whitespace. This messes up blocks that require the leading whitespace, like code blocks.
            block.IsOpen = true;

            return BlockState.Skip;
        }

        // Returns null if a FlexiSectionBlock can't be created
        internal virtual FlexiSectionBlock TryCreateFlexiSectionBlock(BlockProcessor processor)
        {
            // An ATX heading consists of a string of characters, parsed as inline content, 
            // between an opening sequence of 1–6 unescaped # characters
            char c;
            int numStartCharsToDiscard = 0;
            int level = 1;
            while ((c = processor.Line.PeekChar(level)) == '#')
            {
                if (++level > 6)
                {
                    return null;
                }
            }

            // The opening sequence of # characters must be followed by a space or by the end of line.
            if (c == ' ')
            {
                numStartCharsToDiscard = level + 1; // Skip space after opening #s
            }
            else if (c == '\0')
            {
                numStartCharsToDiscard = level;
            }
            else
            {
                return null;
            }

            // Create FlexiSectionBlock
            var flexiSectionBlock = new FlexiSectionBlock(this)
            {
                Level = level,
                Column = processor.Column,
                Span = { Start = processor.Line.Start },
            };

            // Discard redundant start characters
            processor.Column += numStartCharsToDiscard;
            processor.Line.Start += numStartCharsToDiscard;

            return flexiSectionBlock;
        }

        internal virtual void SetupFlexiSectionBlock(BlockProcessor processor, FlexiSectionBlock flexiSectionBlock)
        {
            // Trim closing hashes
            TrimClosingHashes(processor);

            // Setup the source end position of this element
            flexiSectionBlock.Span.End = processor.Line.End;

            // Set FlexiSectionBlock header content
            processor.Line.Trim();
            flexiSectionBlock.HeaderContent = processor.Line.ToString();

            // Create options
            flexiSectionBlock.FlexiSectionBlockOptions = CreateFlexiSectionBlockOptions(processor, flexiSectionBlock.Level);

            // Generate ID
            GenerateID(processor, flexiSectionBlock);

            // Setup auto linking
            SetupAutoLinking(processor, flexiSectionBlock);
        }

        internal virtual void TrimClosingHashes(BlockProcessor processor)
        {
            // The optional closing sequence of #s must be preceded by a space and may be followed by spaces
            // only.
            char c;
            int state = 0; // 0 > in trailing spaces, 1 > in closing #s
            for (int i = processor.Line.End; i >= processor.Line.Start - 1; i--)
            {
                c = processor.Line[i];

                if (state == 0)
                {
                    if (c == ' ')
                    {
                        continue;
                    }
                    if (c == '#')
                    {
                        state = 1;
                        continue;
                    }
                }
                if (state == 1)
                {
                    if (c == '#')
                    {
                        continue;
                    }
                    if (c == ' ')
                    {
                        processor.Line.End = i - 1;
                    }
                }

                break;
            }
        }

        internal virtual FlexiSectionBlockOptions CreateFlexiSectionBlockOptions(BlockProcessor processor, int level)
        {
            FlexiSectionBlockOptions result = _extensionOptions.DefaultBlockOptions.Clone();

            // Apply FlexiOptionsBlock options if they exist
            _flexiOptionsBlockService.TryPopulateOptions(processor, result, processor.LineIndex);

            // Generate and populate class
            if (!string.IsNullOrWhiteSpace(result.ClassFormat))
            {
                try
                {
                    result.Class = string.Format(result.ClassFormat, level);
                }
                catch (FormatException formatException)
                {
                    throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_OptionIsAnInvalidFormat, nameof(result.ClassFormat), result.ClassFormat),
                        formatException);
                }
            }

            return result;
        }

        internal virtual void GenerateID(BlockProcessor processor, FlexiSectionBlock flexiSectionBlock)
        {
            if (!flexiSectionBlock.FlexiSectionBlockOptions.GenerateIdentifier)
            {
                return;
            }

            string id = null;
            if (flexiSectionBlock.FlexiSectionBlockOptions.Attributes?.TryGetValue("id", out id) == true)
            {
                flexiSectionBlock.ID = id;
                return;
            }

            // Get section IDs
            Dictionary<string, int> sectionIDs = GetOrCreateSectionIDs(processor);

            // If header content is empty or whitespace, use section
            id = string.IsNullOrWhiteSpace(flexiSectionBlock.HeaderContent) ? "section" : LinkHelper.UrilizeAsGfm(flexiSectionBlock.HeaderContent);

            // Check for duplicate ids and append an integer if necessary
            if (sectionIDs.TryGetValue(id, out int numDuplicates))
            {
                sectionIDs[id] = ++numDuplicates;
                id = $"{id}-{numDuplicates}";
            }
            else
            {
                sectionIDs.Add(id, 0);
            }

            flexiSectionBlock.ID = id;
        }

        internal virtual Dictionary<string, int> GetOrCreateSectionIDs(BlockProcessor processor)
        {
            if (!(processor.Document.GetData(SECTION_IDS_KEY) is Dictionary<string, int> sectionIDs))
            {
                sectionIDs = new Dictionary<string, int>();
                processor.Document.SetData(SECTION_IDS_KEY, sectionIDs);
            }

            return sectionIDs;
        }

        internal virtual void SetupAutoLinking(BlockProcessor processor, FlexiSectionBlock flexiSectionBlock)
        {
            if (flexiSectionBlock.ID == null || !flexiSectionBlock.FlexiSectionBlockOptions.AutoLinkable)
            {
                return;
            }

            Dictionary<string, SectionLinkReferenceDefinition> sectionLinkReferenceDefinitions = GetOrCreateSectionLinkReferenceDefinitions(processor);

            sectionLinkReferenceDefinitions[flexiSectionBlock.HeaderContent] = new SectionLinkReferenceDefinition()
            {
                FlexiSectionBlock = flexiSectionBlock,
                CreateLinkInline = CreateLinkInline
            };
        }

        internal virtual Dictionary<string, SectionLinkReferenceDefinition> GetOrCreateSectionLinkReferenceDefinitions(BlockProcessor processor)
        {
            if (!(processor.Document.GetData(SECTION_LINK_REFERENCE_DEFINITIONS_KEY) is Dictionary<string, SectionLinkReferenceDefinition> sectionLinkReferenceDefinitions))
            {
                sectionLinkReferenceDefinitions = new Dictionary<string, SectionLinkReferenceDefinition>();
                processor.Document.SetData(SECTION_LINK_REFERENCE_DEFINITIONS_KEY, sectionLinkReferenceDefinitions);
                processor.Document.ProcessInlinesBegin += DocumentOnProcessInlinesBegin;
            }

            return sectionLinkReferenceDefinitions;
        }

        internal virtual Inline CreateLinkInline(InlineProcessor inlineState, LinkReferenceDefinition linkReferenceDefinition, Inline child)
        {
            var sectionLinkReferenceDefinition = (SectionLinkReferenceDefinition)linkReferenceDefinition;
            return new LinkInline()
            {
                Url = HtmlHelper.Unescape("#" + sectionLinkReferenceDefinition.FlexiSectionBlock.ID),
                Title = HtmlHelper.Unescape(linkReferenceDefinition.Title)
            };
        }

        // Inserts <see cref="SectionLinkReferenceDefinition"/>s into the <see cref="MarkdownDocument"/>'s <see cref="LinkReferenceDefinition" />s,
        // allowing for auto linking to sections via header text. Logic in this method is called just before inline processing begins to avoid 
        // overriding vanilla <see cref="LinkReferenceDefinition"/>s.
        internal virtual void DocumentOnProcessInlinesBegin(InlineProcessor processor, Inline inline)
        {
            // Get SectionLinkReferenceDefinition map
            foreach (var keyPair in (Dictionary<string, SectionLinkReferenceDefinition>)processor.Document.GetData(SECTION_LINK_REFERENCE_DEFINITIONS_KEY))
            {
                // Avoid overriding existing LinkReferenceDefinitions
                if (!processor.Document.TryGetLinkReferenceDefinition(keyPair.Key, out LinkReferenceDefinition linkReferenceDefinition))
                {
                    processor.Document.SetLinkReferenceDefinition(keyPair.Key, keyPair.Value);
                }
            }
        }

        internal virtual void UpdateOpenFlexiSectionBlocks(BlockProcessor processor, FlexiSectionBlock flexiSectionBlock)
        {
            Stack<Stack<FlexiSectionBlock>> openSectionBlocks = GetOrCreateOpenFlexiSectionBlocks(processor);

            // Discard stacks for closed trees. When a sectioning content root is closed, all of its children are closed, so the last section in its tree
            // will be closed.
            while (openSectionBlocks.Count > 0)
            {
                // Branches will always have at least 1 FlexiSectionBlock
                if (openSectionBlocks.Peek().Peek().IsOpen)
                {
                    break;
                }

                openSectionBlocks.Pop();
            }

            // Create a new stack for a new tree
            if (!(processor.CurrentContainer is FlexiSectionBlock))
            {
                var newStack = new Stack<FlexiSectionBlock>();
                newStack.Push(flexiSectionBlock);
                openSectionBlocks.Push(newStack);
            }
            else
            {
                Stack<FlexiSectionBlock> currentStack = openSectionBlocks.Peek(); // If CurrentContainer is a FlexiSectionBlock, at least 1 child stack exists
                // Close open FlexiSectionBlocks that have the same or higher levels
                FlexiSectionBlock flexiSectionBlockToClose = null;
                while (currentStack.Count > 0)
                {
                    if (currentStack.Peek().Level < flexiSectionBlock.Level)
                    {
                        break;
                    }

                    flexiSectionBlockToClose = currentStack.Pop();
                }
                if (flexiSectionBlockToClose != null)
                {
                    processor.Close(flexiSectionBlockToClose);
                }

                // Add new FlexiSectionBlock to current stack
                currentStack.Push(flexiSectionBlock);
            }
        }

        // We use stacks to traverse section trees, in a DFS like manner. Since sectioning content roots like blockquotes have their own discrete section trees, 
        // we maintain a stack of stacks.
        internal virtual Stack<Stack<FlexiSectionBlock>> GetOrCreateOpenFlexiSectionBlocks(BlockProcessor processor)
        {
            if (!(processor.Document.GetData(OPEN_FLEXI_SECTION_BLOCKS_KEY) is Stack<Stack<FlexiSectionBlock>> openFlexiSectionBlocks))
            {
                openFlexiSectionBlocks = new Stack<Stack<FlexiSectionBlock>>();
                processor.Document.SetData(OPEN_FLEXI_SECTION_BLOCKS_KEY, openFlexiSectionBlocks);
            }

            return openFlexiSectionBlocks;
        }
    }
}
