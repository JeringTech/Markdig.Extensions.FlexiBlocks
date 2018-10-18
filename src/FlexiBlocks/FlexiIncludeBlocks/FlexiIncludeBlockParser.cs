using Jering.IocServices.Newtonsoft.Json;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    /// <summary>
    /// A parser that creates <see cref="FlexiIncludeBlock"/>s from markdown.
    /// </summary>
    public class FlexiIncludeBlockParser : FlexiBlockParser
    {
        internal const string CLOSING_FLEXI_INCLUDE_BLOCKS_KEY = "closingFlexiIncludeBlocksKey";
        private static readonly StringSlice _codeBlockFence = new StringSlice("```");
        private static readonly ReadOnlyCollection<Clipping> _defaultClippings = new ReadOnlyCollection<Clipping>(new List<Clipping> { new Clipping() });

        private readonly FlexiIncludeBlocksExtensionOptions _extensionOptions;
        private readonly ISourceRetrieverService _sourceRetrieverService;
        private readonly IJsonSerializerService _jsonSerializerService;
        private readonly ILeadingWhitespaceEditorService _leadingWhitespaceEditorService;
        private readonly List<FlexiIncludeBlock> _flexiIncludeBlockTrees;

        /// <summary>
        /// Creates a <see cref="FlexiIncludeBlockParser"/> instance.
        /// </summary>
        /// <param name="sourceRetrieverService">The service that will handle content retrieval.</param>
        /// <param name="jsonSerializerService">The service that will handle JSON deserialization.</param>
        /// <param name="leadingWhitespaceEditorService">The service that will handle editing of leading whitespace.</param>
        /// <param name="extensionOptions">Extension options.</param>
        public FlexiIncludeBlockParser(ISourceRetrieverService sourceRetrieverService,
            IJsonSerializerService jsonSerializerService,
            ILeadingWhitespaceEditorService leadingWhitespaceEditorService,
            FlexiIncludeBlocksExtensionOptions extensionOptions)
        {
            _extensionOptions = extensionOptions ?? throw new ArgumentNullException(nameof(extensionOptions));
            _sourceRetrieverService = sourceRetrieverService ?? throw new ArgumentNullException(nameof(sourceRetrieverService));
            _jsonSerializerService = jsonSerializerService ?? throw new ArgumentNullException(nameof(jsonSerializerService));
            _leadingWhitespaceEditorService = leadingWhitespaceEditorService ?? throw new ArgumentNullException(nameof(leadingWhitespaceEditorService));

            OpeningCharacters = new[] { '+' };
            _flexiIncludeBlockTrees = new List<FlexiIncludeBlock>();
        }

        /// <summary>
        /// Opens a <see cref="FlexiIncludeBlock"/> if a line begins with 0 to 3 spaces followed by "+{".
        /// </summary>
        /// <param name="processor">The block processor for the document that contains a line with first non-white-space character "+".</param>
        /// <returns>
        /// <see cref="BlockState.None"/> if the current line has code indent or if the current line does not start with the expected characters.
        /// <see cref="BlockState.Break"/> if a <see cref="FlexiIncludeBlock"/> is opened and the current line contains the entire JSON string.
        /// <see cref="BlockState.Continue"/> if a <see cref="FlexiIncludeBlock"/> is opened and the current line contains part of the JSON string.
        /// </returns>
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

            // First line of a FlexiIncludeBlock must begin with +{
            if (processor.Line.PeekChar() != '{')
            {
                return BlockState.None;
            }

            // Get or create stack
            Stack<FlexiIncludeBlock> closingFlexiIncludeBlocks = GetOrCreateClosingFlexiIncludeBlocks(processor);

            // Create FlexiIncludeBlock
            FlexiIncludeBlock parentFlexiIncludeBlock = closingFlexiIncludeBlocks.FirstOrDefault();
            var flexiIncludeBlock = new FlexiIncludeBlock(parentFlexiIncludeBlock, this)
            {
                Column = processor.Column,
                Span = { Start = processor.Start } // FlexiOptionsBlock.ParseLine will update the span's end
            };
            processor.NewBlocks.Push(flexiIncludeBlock);

            // Add to trees if FlexiIncludeBlock is a root block
            if(parentFlexiIncludeBlock == null)
            {
                _flexiIncludeBlockTrees.Add(flexiIncludeBlock);
            }

            // Dispose of + (JSON starts at the curly bracket)
            processor.NextChar();

            return flexiIncludeBlock.ParseLine(processor.Line);
        }

        /// <summary>
        /// Continues a <see cref="FlexiIncludeBlock"/> if its JSON is incomplete.
        /// </summary>
        /// <param name="processor">The block processor for the <see cref="FlexiIncludeBlock"/> to try and continue.</param>
        /// <param name="block">The <see cref="FlexiIncludeBlock"/> to try and continue.</param>
        /// <returns>
        /// <see cref="BlockState.Continue"/> if the <see cref="FlexiIncludeBlock"/> is still open.
        /// <see cref="BlockState.Break"/> if the <see cref="FlexiIncludeBlock"/> has ended and should be closed.
        /// </returns>
        protected override BlockState TryContinueFlexiBlock(BlockProcessor processor, Block block)
        {
            if (processor == null)
            {
                throw new ArgumentNullException(nameof(processor));
            }

            if (block == null)
            {
                throw new ArgumentNullException(nameof(block));
            }

            var flexiIncludeBlock = (FlexiIncludeBlock)block;

            return flexiIncludeBlock.ParseLine(processor.Line);
        }

        /// <summary>
        /// Replaces the <see cref="FlexiIncludeBlock"/> with blocks generated from its content.
        /// </summary>
        /// <param name="processor">The block processor for the <see cref="FlexiIncludeBlock"/> that is closing.</param>
        /// <param name="block">The <see cref="FlexiIncludeBlock"/> that is closing.</param>
        /// <returns>Returns false, indicating that the <see cref="FlexiIncludeBlock"/> should be discarded from the tree of blocks.</returns>
        protected override bool CloseFlexiBlock(BlockProcessor processor, Block block)
        {
            if (processor == null)
            {
                throw new ArgumentNullException(nameof(processor));
            }

            if (block == null)
            {
                throw new ArgumentNullException(nameof(block));
            }

            var flexiIncludeBlock = (FlexiIncludeBlock)block;

            // FlexiIncludeBlocks can reference sources other than the entry markdown document. If an exception is thrown while processing content from such sources,
            // we must flesh out the context. Specifically, we must indicate which source and where within the source the unrecoverable situation was encountered.
            try
            {
                // Create options
                SetupFlexiIncludeBlock(flexiIncludeBlock);

                // If the FlexiIncludeBlock's type is markdown, we need to ensure that we don't have a cycle of includes.  
                Stack<FlexiIncludeBlock> closingFlexiIncludeBlocks = null;
                if (flexiIncludeBlock.FlexiIncludeBlockOptions.Type == IncludeType.Markdown)
                {
                    closingFlexiIncludeBlocks = GetOrCreateClosingFlexiIncludeBlocks(processor);
                    CheckForCycle(closingFlexiIncludeBlocks, flexiIncludeBlock);
                }

                // Retrieve source
                ReadOnlyCollection<string> source = _sourceRetrieverService.GetSource(flexiIncludeBlock.AbsoluteSourceUri,
                    flexiIncludeBlock.FlexiIncludeBlockOptions.ResolvedDiskCacheDirectory);

                // Convert source into blocks and replace flexiIncludeBlock with the newly created blocks
                ReplaceFlexiIncludeBlock(processor, flexiIncludeBlock, source);

                // Remove FlexiIncludeBlock from graph used to check for cycles
                if (flexiIncludeBlock.FlexiIncludeBlockOptions.Type == IncludeType.Markdown)
                {
                    closingFlexiIncludeBlocks.Pop();
                }
            }
            catch (Exception exception)
            {
                string description = null;
                if (flexiIncludeBlock.ClippingProcessingStage == ClippingProcessingStage.None)
                {
                    description = Strings.FlexiBlocksException_FlexiIncludeBlocks_ExceptionOccurredWhileProcessingBlock;
                }
                else if (flexiIncludeBlock.ClippingProcessingStage == ClippingProcessingStage.Source)
                {
                    description = string.Format(Strings.FlexiBlocksException_FlexiIncludeBlocks_ExceptionOccurredWhileProcessingSource, flexiIncludeBlock.AbsoluteSourceUri.AbsoluteUri);
                }
                else // Before or after content
                {
                    description = string.Format(Strings.FlexiBlocksException_FlexiIncludeBlocks_ExceptionOccurredWhileProcessingContent, flexiIncludeBlock.ClippingProcessingStage);
                }

                throw new FlexiBlocksException(flexiIncludeBlock,
                    description,
                    exception,
                    // FlexiIncludeBlock.Line is the index of the current line if included content actually got inlined. What we really want
                    // is the line in the containing source (unless we are dealing with Root).
                    flexiIncludeBlock.ContainingSourceUri == null ? flexiIncludeBlock.Line + 1 : flexiIncludeBlock.LineNumberInContainingSource,
                    flexiIncludeBlock.Column);
            }

            // Discard the FlexiIncludeBlock
            return false;
        }

        internal virtual Stack<FlexiIncludeBlock> GetOrCreateClosingFlexiIncludeBlocks(BlockProcessor processor)
        {
            if (!(processor.Document.GetData(CLOSING_FLEXI_INCLUDE_BLOCKS_KEY) is Stack<FlexiIncludeBlock> closingFlexiIncludeBlocks))
            {
                closingFlexiIncludeBlocks = new Stack<FlexiIncludeBlock>();
                processor.Document.SetData(CLOSING_FLEXI_INCLUDE_BLOCKS_KEY, closingFlexiIncludeBlocks);
            }

            return closingFlexiIncludeBlocks;
        }

        // Create FlexiIncludeBlock options and setup FlexiIncludeBlock
        internal virtual void SetupFlexiIncludeBlock(FlexiIncludeBlock flexiIncludeBlock)
        {
            FlexiIncludeBlockOptions flexiIncludeBlockOptions = _extensionOptions.DefaultBlockOptions.Clone();
            string json = flexiIncludeBlock.Lines.ToString();

            try
            {
                using (var jsonTextReader = new JsonTextReader(new StringReader(json)))
                {
                    _jsonSerializerService.Populate(jsonTextReader, flexiIncludeBlockOptions);
                }
            }
            catch (Exception exception)
            {
                // If a FlexiBlocksException is thrown while validating the populated object, it is wrapped in a TargetInvocationException that we can discard.
                if(exception.InnerException is FlexiBlocksException)
                {
                    // Preserve call stack
                    ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
                }

                throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_UnableToParseJson, json), exception);
            }

            // Setup FlexiIncludeBlock
            flexiIncludeBlock.Setup(flexiIncludeBlockOptions, _extensionOptions.RootBaseUri);
        }

        internal virtual void CheckForCycle(Stack<FlexiIncludeBlock> closingFlexiIncludeBlocks, FlexiIncludeBlock flexiIncludeBlock)
        {
            if (closingFlexiIncludeBlocks.Count > 0 && flexiIncludeBlock.ParentFlexiIncludeBlock.ClippingProcessingStage == ClippingProcessingStage.Source)
            {
                for (int i = closingFlexiIncludeBlocks.Count - 1; i > -1; i--)
                {
                    FlexiIncludeBlock closingFlexiIncludeBlock = closingFlexiIncludeBlocks.ElementAt(i);

                    if (closingFlexiIncludeBlock.ContainingSourceUri == flexiIncludeBlock.ContainingSourceUri &&
                        closingFlexiIncludeBlock.LineNumberInContainingSource == flexiIncludeBlock.LineNumberInContainingSource)
                    {
                        // Cycle found
                        string cycleDescription = "";
                        for (; i > -1; i--)
                        {
                            cycleDescription += closingFlexiIncludeBlocks.ElementAt(i) + " >\n";
                        }

                        throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_FlexiIncludeBlocks_CycleFound, cycleDescription + flexiIncludeBlock));
                    }
                }
            }

            closingFlexiIncludeBlocks.Push(flexiIncludeBlock);
        }

        internal virtual void ProcessBeforeOrAfterContent(BlockProcessor processor, FlexiIncludeBlock flexiIncludeBlock, string content)
        {
            flexiIncludeBlock.LastProcessedLineLineNumber = 0;

            // If text is an empty string, LineReader.ReadLine immediately returns null. We want an empty line
            // if content is an empty string.
            if (content.Length == 0)
            {
                flexiIncludeBlock.LastProcessedLineLineNumber = 1;
                processor.ProcessLine(new StringSlice(content));

                return;
            }

            var lineReader = new LineReader(content);
            StringSlice? lineText;
            while ((lineText = lineReader.ReadLine()) != null)
            {
                flexiIncludeBlock.LastProcessedLineLineNumber++;
                processor.ProcessLine(lineText.Value);
            }
        }

        internal virtual void ReplaceFlexiIncludeBlock(BlockProcessor processor,
            FlexiIncludeBlock flexiIncludeBlock,
            ReadOnlyCollection<string> source)
        {
            ContainerBlock parent = flexiIncludeBlock.Parent;

            // Remove the FlexiIncludeBlock
            parent.Remove(flexiIncludeBlock);

            // The method used here is also used by GridTable. The child processor facilitates avoidance of conflicts with existing 
            // open blocks in the root processor.
            BlockProcessor childProcessor = processor.CreateChild();
            childProcessor.Open(parent);

            // MarkdownObject.Line is the line that the block starts at, it is set by BlockProcessor.ProcessNewBlocks. We need to set 
            // LineIndex to the line that the include block starts at for FlexiOptionsBlocks to work.
            childProcessor.LineIndex = flexiIncludeBlock.Line;

            // If content is code, start with ```
            if (flexiIncludeBlock.FlexiIncludeBlockOptions.Type == IncludeType.Code)
            {
                childProcessor.ProcessLine(_codeBlockFence);
            }

            ReadOnlyCollection<Clipping> clippings = flexiIncludeBlock.FlexiIncludeBlockOptions.Clippings?.Count > 0 ?
                flexiIncludeBlock.FlexiIncludeBlockOptions.Clippings : _defaultClippings;

            // Clippings need not be sequential, they can also overlap
            foreach (Clipping clipping in clippings)
            {
                if (clipping.BeforeContent != null)
                {
                    flexiIncludeBlock.ClippingProcessingStage = ClippingProcessingStage.BeforeContent;
                    ProcessBeforeOrAfterContent(childProcessor, flexiIncludeBlock, clipping.BeforeContent);
                }

                int startLineNumber = -1;
                if (clipping.StartDemarcationLineSubstring != null)
                {
                    for (int i = 0; i < source.Count - 1; i++) // Since demarcation lines are not included in the clipping, the last line cannot be a start demarcation line.
                    {
                        if (source[i].Contains(clipping.StartDemarcationLineSubstring))
                        {
                            startLineNumber = i + 2;
                            break;
                        }
                    }

                    if (startLineNumber == -1)
                    {
                        throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_FlexiIncludeBlocks_InvalidClippingNoLineContainsStartLineSubstring, clipping.StartDemarcationLineSubstring));
                    }
                }
                else
                {
                    startLineNumber = clipping.StartLineNumber;
                }

                flexiIncludeBlock.ClippingProcessingStage = ClippingProcessingStage.Source;

                for (int lineNumber = startLineNumber; lineNumber <= source.Count; lineNumber++)
                {
                    var stringSlice = new StringSlice(source[lineNumber - 1]);

                    _leadingWhitespaceEditorService.Dedent(ref stringSlice, clipping.DedentLength);
                    _leadingWhitespaceEditorService.Collapse(ref stringSlice, clipping.CollapseRatio);

                    // To identify FlexiIncludeBlock's their exact line numbers in their containing sources must be known. For this to be possible, 
                    // a parent FlexiIncludeBlock must keep track of the line number of the last line that has been processed.
                    flexiIncludeBlock.LastProcessedLineLineNumber = lineNumber;
                    childProcessor.ProcessLine(stringSlice);

                    // Check whether we've reached the end of the clipping
                    if (clipping.EndDemarcationLineSubstring != null)
                    {
                        if (lineNumber == source.Count)
                        {
                            throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_FlexiIncludeBlocks_InvalidClippingNoLineContainsEndLineSubstring, clipping.EndDemarcationLineSubstring));
                        }

                        // Check if next line contains the end line substring
                        if (source[lineNumber].Contains(clipping.EndDemarcationLineSubstring))
                        {
                            break;
                        }
                    }
                    else if (lineNumber == clipping.EndLineNumber)
                    {
                        break;
                    }
                }

                if (clipping.AfterContent != null)
                {
                    flexiIncludeBlock.ClippingProcessingStage = ClippingProcessingStage.AfterContent;
                    ProcessBeforeOrAfterContent(childProcessor, flexiIncludeBlock, clipping.AfterContent);
                }
            }

            if (flexiIncludeBlock.FlexiIncludeBlockOptions.Type == IncludeType.Code) // If content is code, end with ```
            {
                childProcessor.ProcessLine(_codeBlockFence);
            }

            // Ensure that the last replacement block has been closed. While the block never makes it to the OpenedBlocks collection in the root processor, 
            // calling Close for it ensures that it and its children's Close methods and events get called.
            childProcessor.Close(parent.LastChild);

            // BlockProcessors are pooled. Once we're done with innerProcessor, we must release it. This also removes all references to
            // tempContainerBlock, which should allow it to be collected quickly.
            childProcessor.ReleaseChild();
        }

        internal List<FlexiIncludeBlock> GetFlexiIncludeBlockTrees()
        {
            return _flexiIncludeBlockTrees;
        }
    }
}
