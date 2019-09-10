using Jering.IocServices.System.IO;
using Jering.Markdig.Extensions.FlexiBlocks.ContextObjects;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Jering.Markdig.Extensions.FlexiBlocks.IncludeBlocks
{
    /// <summary>
    /// The implementation of <see cref="IJsonBlockFactory{TMain, TProxy}"/> for creating <see cref="IncludeBlock"/>s.
    /// </summary>
    public class IncludeBlockFactory : IJsonBlockFactory<IncludeBlock, ProxyJsonBlock>
    {
        internal const string CLOSING_INCLUDE_BLOCKS_KEY = "closingIncludeBlocksKey";
        // We only support a subset of schemes. For the full list of schemes, see https://docs.microsoft.com/en-sg/dotnet/api/system.uri.scheme?view=netstandard-2.0#System_Uri_Scheme
        private static readonly string[] _supportedSchemes = new string[] { "file", "http", "https" };
        private static readonly StringSlice _codeBlockFence = new StringSlice("```");
        private static readonly ReadOnlyCollection<Clipping> _defaultClippings = new ReadOnlyCollection<Clipping>(new List<Clipping> { new Clipping() });
        private static readonly Uri _defaultRootBaseUri = new Uri(Directory.GetCurrentDirectory() + "/");

        private readonly IContextObjectsService _contextObjectsService;
        private readonly IDirectoryService _directoryService;
        private readonly IOptionsService<IIncludeBlockOptions, IIncludeBlocksExtensionOptions> _optionsService;
        private readonly IContentRetrieverService _contentRetrieverService;
        private readonly ILeadingWhitespaceEditorService _leadingWhitespaceEditorService;

        /// <summary>
        /// The key for the <see cref="List{T}"/> containing <see cref="IncludeBlock"/> trees.
        /// </summary>
        public const string INCLUDE_BLOCK_TREES_KEY = "includeBlocksTreesKey";

        /// <summary>
        /// Creates an <see cref="IncludeBlockFactory"/>.
        /// </summary>
        /// <param name="contextObjectsService">The service for storing <see cref="IncludeBlock"/> trees.</param>
        /// <param name="directoryService">The service for validating cache directories.</param>
        /// <param name="optionsService">The service for creating <see cref="IIncludeBlockOptions"/> and <see cref="IIncludeBlocksExtensionOptions"/>.</param>
        /// <param name="contentRetrieverService">The service that handles content retrieval.</param>
        /// <param name="leadingWhitespaceEditorService">The service for editing of leading whitespace.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contextObjectsService"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="directoryService"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="optionsService"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentRetrieverService"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="leadingWhitespaceEditorService"/> is <c>null</c>.</exception>
        public IncludeBlockFactory(IContextObjectsService contextObjectsService,
            IDirectoryService directoryService,
            IOptionsService<IIncludeBlockOptions, IIncludeBlocksExtensionOptions> optionsService,
            IContentRetrieverService contentRetrieverService,
            ILeadingWhitespaceEditorService leadingWhitespaceEditorService)
        {
            _contextObjectsService = contextObjectsService ?? throw new ArgumentNullException(nameof(contextObjectsService));
            _directoryService = directoryService ?? throw new ArgumentNullException(nameof(directoryService));
            _optionsService = optionsService ?? throw new ArgumentNullException(nameof(optionsService));
            _contentRetrieverService = contentRetrieverService ?? throw new ArgumentNullException(nameof(contentRetrieverService));
            _leadingWhitespaceEditorService = leadingWhitespaceEditorService ?? throw new ArgumentNullException(nameof(leadingWhitespaceEditorService));
        }

        /// <summary>
        /// Creates a <see cref="ProxyJsonBlock"/>.
        /// </summary>
        /// <param name="blockProcessor">The <see cref="BlockProcessor"/> processing the <see cref="ProxyJsonBlock"/>.</param>
        /// <param name="blockParser">The <see cref="BlockParser"/> parsing the <see cref="ProxyJsonBlock"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="blockProcessor"/> is <c>null</c>.</exception>
        public ProxyJsonBlock CreateProxyJsonBlock(BlockProcessor blockProcessor, BlockParser blockParser)
        {
            if (blockProcessor == null)
            {
                throw new ArgumentNullException(nameof(blockProcessor));
            }

            return new ProxyJsonBlock(nameof(IncludeBlock), blockParser)
            {
                Column = blockProcessor.Column,
                Span = { Start = blockProcessor.Start } // JsonBlockParser.ParseLine will update the span's end
                // Line is assigned by BlockProcessor
            };
        }

        /// <summary>
        /// Creates an <see cref="IncludeBlock"/>.
        /// </summary>
        /// <param name="proxyJsonBlock">The <see cref="ProxyJsonBlock"/> containing data for the <see cref="IncludeBlock"/>.</param>
        /// <param name="blockProcessor">The <see cref="BlockProcessor"/> processing the <see cref="IncludeBlock"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="proxyJsonBlock"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="blockProcessor"/> is <c>null</c>.</exception>
        /// <exception cref="OptionsException">Thrown if an option is invalid.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an <see cref="IncludeBlock"/> cycle is found.</exception>
        /// <exception cref="BlockException">Thrown if an exception is thrown while processing the <see cref="IncludeBlock"/>'s included content.</exception>
        public IncludeBlock Create(ProxyJsonBlock proxyJsonBlock, BlockProcessor blockProcessor)
        {
            (IIncludeBlockOptions includeBlockOptions, IIncludeBlocksExtensionOptions includeBlocksExtensionOptions) = _optionsService.
                CreateOptions(blockProcessor, proxyJsonBlock);

            // Source
            string source = includeBlockOptions.Source;
            ValidateSource(source);

            // Type
            IncludeType type = includeBlockOptions.Type;
            ValidateType(type);

            // Cache directory
            string cacheDirectory = ResolveAndValidateCacheDirectory(includeBlockOptions.Cache, includeBlockOptions.CacheDirectory);

            // Parent
            IncludeBlock parent = ResolveParent(blockProcessor);

            // Containing source
            string containingSource = ResolveContainingSource(parent);

            // Source absolute URI
            Uri sourceAbsoluteUri = ResolveSourceAbsoluteUri(source, includeBlocksExtensionOptions.BaseUri, parent);

            // Create block
            var includeBlock = new IncludeBlock(sourceAbsoluteUri,
                includeBlockOptions.Clippings,
                type,
                cacheDirectory,
                parent,
                containingSource,
                proxyJsonBlock.Parser)
            {
                Column = proxyJsonBlock.Column,
                Line = proxyJsonBlock.Line,
                Span = proxyJsonBlock.Span
            };
            parent?.Children.Add(includeBlock);

            ProcessIncludeBlock(includeBlock, proxyJsonBlock, blockProcessor);

            return null;
        }

        internal virtual void ValidateSource(string source)
        {
            if (source == null)
            {
                throw new OptionsException(nameof(IIncludeBlockOptions.Source), Strings.OptionsException_Shared_ValueMustNotBeNull);
            }
        }

        internal virtual void ValidateType(IncludeType type)
        {
            if (!Enum.IsDefined(typeof(IncludeType), type))
            {
                throw new OptionsException(nameof(IIncludeBlockOptions.Type),
                        string.Format(Strings.OptionsException_Shared_ValueMustBeAValidEnumValue,
                            type,
                            nameof(IncludeType)));
            }
        }

        internal virtual string ResolveAndValidateCacheDirectory(bool cache, string cacheDirectory)
        {
            if (cache)
            {
                if (string.IsNullOrWhiteSpace(cacheDirectory))
                {
                    return Path.Combine(Directory.GetCurrentDirectory(), "ContentCache"); // Always valid
                }

                if (!_directoryService.Exists(cacheDirectory))
                {
                    throw new OptionsException(nameof(IIncludeBlockOptions.CacheDirectory), string.Format(Strings.OptionsException_Shared_DirectoryDoesNotExist, cacheDirectory));
                }

                return cacheDirectory;
            }
            else
            {
                return null;
            }
        }

        internal virtual IncludeBlock ResolveParent(BlockProcessor blockProcessor)
        {
            Stack<IncludeBlock> closingIncludeBlocks = GetOrCreateClosingIncludeBlocks(blockProcessor);
            return closingIncludeBlocks.FirstOrDefault();
        }

        internal virtual string ResolveContainingSource(IncludeBlock parent)
        {
            return parent?.Source.AbsoluteUri;
        }

        internal virtual Uri ResolveSourceAbsoluteUri(string source, string rootBaseUri, IncludeBlock parent)
        {
            if (Uri.TryCreate(source, UriKind.Absolute, out Uri sourceAbsoluteUri))
            {
                // Invalid scheme
                if (!_supportedSchemes.Contains(sourceAbsoluteUri.Scheme))
                {
                    throw new OptionsException(nameof(IIncludeBlockOptions.Source),
                        string.Format(Strings.OptionsException_IncludeBlockFactory_ValueMustBeAUriWithASupportedScheme,
                            source,
                            sourceAbsoluteUri.Scheme));
                }
            }
            else if (parent != null) // Source is relative so parent's absolute source URI should be used as base
            {
                if (!Uri.TryCreate(parent.Source, source, out sourceAbsoluteUri))
                {
                    // Source is neither a valid absolute URI nor a valid relative URI
                    throw new OptionsException(nameof(IIncludeBlockOptions.Source),
                        string.Format(Strings.OptionsException_IncludeBlockFactory_ValueMustBeAValidUri,
                            source));
                }
            }
            else // Source is relative, include block is in root content - root base URI should be used as base
            {
                // Normalize rootBaseUri. A base URI must be absolute, see http://www.ietf.org/rfc/rfc3986.txt, section 5.1.
                //
                // Note: On Windows, "/**/*" is not considered an absolute URI, absolute URIs must start with "<drive letter>:/" or "file:///".
                // On Linux and MacOS, "/**/*" can be an absolute URI or a relative URI. The Framework authors chose to allow "/**/*" to pass this check on Linux and macOS.
                // That isn't a huge issue since an exception will be thrown if we try to retrieve anything from a URI that has a relative URI as its base.
                // However it is worth noting for unit testing.
                //
                // The difference in behaviour on Windows and Linux/macOS is documented here - https://github.com/dotnet/corefx/issues/22098.
                Uri baseAbsoluteUri;
                if (rootBaseUri == null)
                {
                    baseAbsoluteUri = _defaultRootBaseUri; // Default root base uri, always absolute and always has a valid scheme
                }
                else
                {
                    if (!Uri.TryCreate(rootBaseUri, UriKind.Absolute, out baseAbsoluteUri))
                    {
                        throw new OptionsException(nameof(IIncludeBlocksExtensionOptions.BaseUri),
                            string.Format(Strings.OptionsException_IncludeBlockFactory_ValueMustBeAnAbsoluteUri,
                                rootBaseUri));
                    }

                    if (!_supportedSchemes.Contains(baseAbsoluteUri.Scheme)) // Invalid scheme
                    {
                        throw new OptionsException(nameof(IIncludeBlocksExtensionOptions.BaseUri),
                            string.Format(Strings.OptionsException_IncludeBlockFactory_ValueMustBeAUriWithASupportedScheme,
                                rootBaseUri,
                                baseAbsoluteUri.Scheme));
                    }
                }

                if (!Uri.TryCreate(baseAbsoluteUri, source, out sourceAbsoluteUri))
                {
                    // Source is neither a valid absolute URI nor a valid relative URI
                    throw new OptionsException(nameof(IIncludeBlockOptions.Source),
                        string.Format(Strings.OptionsException_IncludeBlockFactory_ValueMustBeAValidUri,
                            source));
                }
            }

            return sourceAbsoluteUri;
        }

        internal virtual void ProcessIncludeBlock(IncludeBlock includeBlock, ProxyJsonBlock proxyJsonBlock, BlockProcessor blockProcessor)
        {
            // If the IncludeBlock's type is markdown, we need to ensure we don't have a cycle of includes.  
            bool isMarkdown = includeBlock.Type == IncludeType.Markdown;
            Stack<IncludeBlock> closingIncludeBlocks = null;
            if (isMarkdown)
            {
                closingIncludeBlocks = GetOrCreateClosingIncludeBlocks(blockProcessor);
                CheckForCycle(closingIncludeBlocks, includeBlock);
                closingIncludeBlocks.Push(includeBlock);
            }

            // Retrieve source
            ReadOnlyCollection<string> content = _contentRetrieverService.GetContent(includeBlock.Source, includeBlock.CacheDirectory);

            // Parent of new blocks
            ContainerBlock parentOfNewBlocks = proxyJsonBlock.Parent;
            parentOfNewBlocks.Remove(proxyJsonBlock);

            try
            {
                // Convert content into blocks and add them to parentOfNewBlocks
                ProcessContent(blockProcessor, includeBlock, parentOfNewBlocks, content);
            }
            catch (Exception exception)
            {
                // IncludeBlocks can reference sources other than the entry markdown document. If an exception is thrown while processing content from such sources,
                // we must provide context. Specifically, we must specify which source and where within the source the unrecoverable situation was encountered.
                throw new BlockException(includeBlock,
                    string.Format(Strings.BlockException_IncludeBlockFactory_ExceptionOccurredWhileProcessingContent, includeBlock.Source.AbsoluteUri),
                    exception);
            }

            // Remove IncludeBlock from stack used to check for cycles
            if (isMarkdown)
            {
                closingIncludeBlocks.Pop();
            }

            // Add to trees if IncludeBlock is a root block
            TryAddToIncludeBlockTrees(includeBlock, blockProcessor);
        }

        internal virtual Stack<IncludeBlock> GetOrCreateClosingIncludeBlocks(BlockProcessor blockProcessor)
        {
            if (!(blockProcessor.Document.GetData(CLOSING_INCLUDE_BLOCKS_KEY) is Stack<IncludeBlock> closingIncludeBlocks))
            {
                closingIncludeBlocks = new Stack<IncludeBlock>();
                blockProcessor.Document.SetData(CLOSING_INCLUDE_BLOCKS_KEY, closingIncludeBlocks);
            }

            return closingIncludeBlocks;
        }

        internal virtual void CheckForCycle(Stack<IncludeBlock> closingIncludeBlocks, IncludeBlock includeBlock)
        {
            if (closingIncludeBlocks.Count == 0)
            {
                return;
            }

            string containingSource = includeBlock.ContainingSource;
            int line = includeBlock.Line;
            foreach (IncludeBlock closingIncludeBlock in closingIncludeBlocks)
            {
                if (closingIncludeBlock.ContainingSource == containingSource &&
                    closingIncludeBlock.Line == line)
                {
                    // Cycle found
                    string cycleDescription = PrintIncludeBlockForCycleDescription(includeBlock);
                    foreach (IncludeBlock cycleIncludeBlock in closingIncludeBlocks)
                    {
                        cycleDescription = PrintIncludeBlockForCycleDescription(cycleIncludeBlock) + " >\n" + cycleDescription;

                        if (cycleIncludeBlock == closingIncludeBlock)
                        {
                            break;
                        }
                    }

                    throw new InvalidOperationException(string.Format(Strings.InvalidOperationException_IncludeBlockFactory_CycleFound, cycleDescription));
                }
            }
        }

        internal virtual void ProcessContent(BlockProcessor blockProcessor,
            IncludeBlock includeBlock,
            ContainerBlock parentOfNewBlocks,
            ReadOnlyCollection<string> content)
        {
            // The method used here is also used for GridTables. Using a child processor avoids conflicts with existing 
            // open blocks in the root processor.
            BlockProcessor childProcessor = blockProcessor.CreateChild();
            childProcessor.Open(parentOfNewBlocks);

            // If content is code, start with ```
            bool isCode = includeBlock.Type == IncludeType.Code;
            if (isCode)
            {
                childProcessor.ProcessLine(_codeBlockFence);
            }

            // TODO this has the potential to be really slow if content has lots of lines and we've got lots of clippings using start/end strings
            // Clippings - need not be sequential, they can also overlap
            int contentNumLines = content.Count;
            foreach (Clipping clipping in includeBlock.Clippings ?? _defaultClippings)
            {
                string before = clipping.Before;
                if (isCode && before != null)
                {
                    childProcessor.ProcessLine(new StringSlice(before)); // No issue even if Before is multiline since we're in a code block
                }

                int startLineNumber = -1;
                (string startString, string endString) = clipping.GetNormalizedStartAndEndStrings();
                bool startStringSpecified = startString != null;
                bool endStringSpecified = endString != null;
                (int normalizedStartLine, int normalizedEndLine) normalizedStartAndEndLines = default;
                if (!startStringSpecified || !endStringSpecified)
                {
                    normalizedStartAndEndLines = clipping.GetNormalizedStartAndEndLines(contentNumLines);
                }
                if (startStringSpecified)
                {
                    int lastIndex = contentNumLines - 2; // Since demarcation lines are not included in the clipping, the last line cannot be a start demarcation line.
                    for (int i = 0; i <= lastIndex; i++)
                    {
                        if (content[i].Contains(startString))
                        {
                            startLineNumber = i + 2;
                            break;
                        }
                    }

                    if (startLineNumber == -1)
                    {
                        throw new OptionsException(nameof(Clipping.StartString),
                            string.Format(Strings.OptionsException_IncludeBlockFactory_NoLineContainsStartString, startString));
                    }
                }
                else
                {
                    startLineNumber = normalizedStartAndEndLines.normalizedStartLine;
                }

                // If we encounter an invalid block in the included content, this ensures the BlockException thrown has the right line number in the included content's source.
                childProcessor.LineIndex = startLineNumber - 1;

                for (int lineNumber = startLineNumber; lineNumber <= contentNumLines; lineNumber++)
                {
                    var stringSlice = new StringSlice(content[lineNumber - 1]);

                    if (!stringSlice.IsEmpty)
                    {
                        if (clipping.Indent > 0)
                        {
                            stringSlice = _leadingWhitespaceEditorService.Indent(stringSlice, clipping.Indent);
                        }

                        _leadingWhitespaceEditorService.Dedent(ref stringSlice, clipping.Dedent);
                        _leadingWhitespaceEditorService.Collapse(ref stringSlice, clipping.Collapse);
                    }

                    childProcessor.ProcessLine(stringSlice);

                    // Check whether we've reached the end of the clipping
                    if (endStringSpecified)
                    {
                        if (lineNumber == contentNumLines)
                        {
                            throw new OptionsException(nameof(Clipping.EndString),
                                string.Format(Strings.OptionsException_IncludeBlockFactory_NoLineContainsEndString, endString));
                        }

                        // Check if next line contains the end line substring
                        if (content[lineNumber].Contains(endString))
                        {
                            break;
                        }
                    }
                    else if (lineNumber == normalizedStartAndEndLines.normalizedEndLine)
                    {
                        break;
                    }
                }

                string after = clipping.After;
                if (isCode && after != null)
                {
                    childProcessor.ProcessLine(new StringSlice(after)); // No issue even if Before is multiline since we're in a code block
                }
            }

            if (isCode) // If content is code, end with ```
            {
                childProcessor.ProcessLine(_codeBlockFence);
            }

            // Ensure that the last replacement block has been closed. While the block never makes it to the OpenedBlocks collection in the root processor, 
            // calling Close for it ensures that it and its children's Close methods and events get called.
            childProcessor.Close(parentOfNewBlocks.LastChild);

            // BlockProcessors are pooled. Once we're done with innerProcessor, we must release it. This also removes all references to
            // tempContainerBlock, which should allow it to be collected quickly.
            childProcessor.ReleaseChild();
        }

        // IncludeBlockTrees is useful for keeping track of sources referenced by root sources. It 
        // must be retrievable after markdown processing, e.g after Markdown.ToHtml returns. This means
        // we can't save it in MarkdownDocument data. Instead we must save it as a context object.
        //
        // Returns null if there is no context object store (either a MarkdownParserContext or a ContextObjectsStore).
        internal virtual void TryAddToIncludeBlockTrees(IncludeBlock includeBlock, BlockProcessor blockProcessor)
        {
            if (includeBlock.ParentIncludeBlock != null) // Not the root of a tree
            {
                return;
            }

            if (!_contextObjectsService.TryGetContextObject(INCLUDE_BLOCK_TREES_KEY, blockProcessor, out object includeBlockTreesObject) ||
                !(includeBlockTreesObject is List<IncludeBlock> includeBlockTrees))
            {
                // TODO if we're unable to add, we might end up instantiating tons of lists unnecessarily,
                // ContextObjectsService should have a ContextObjectsSupported method.
                includeBlockTrees = new List<IncludeBlock>();
                if (!_contextObjectsService.TryAddContextObject(INCLUDE_BLOCK_TREES_KEY, includeBlockTrees, blockProcessor))
                {
                    return; // No context object store
                }
            }

            includeBlockTrees.Add(includeBlock);
        }

        private string PrintIncludeBlockForCycleDescription(IncludeBlock includeBlock)
        {
            // A cycle can't start at a root IncludeBlock (rootIncludeBlock.ContainingSource == null, never == currentIncludeBlock.ContainingSource),
            // so we don't need to handle case where ContainingSource is null.
            return $"Source: {includeBlock.ContainingSource}, Line Number: {includeBlock.Line + 1}";
        }
    }
}
