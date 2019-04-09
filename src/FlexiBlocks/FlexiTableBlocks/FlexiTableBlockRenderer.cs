using Markdig.Extensions.Tables;
using Markdig.Renderers;
using Markdig.Syntax;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks
{
    /// <summary>
    /// A renderer that renders FlexiTableBlocks as HTML.
    /// </summary>
    public class FlexiTableBlockRenderer : FlexiBlockRenderer<Table>
    {
        private readonly FlexiTableBlockOptions _defaultFlexiTableBlockOptions;
        private readonly HtmlRenderer _stripRenderer;
        private readonly StringWriter _stringWriter;

        /// <summary>
        /// Creates a <see cref="FlexiTableBlockRenderer"/> instance.
        /// </summary>
        /// <param name="extensionOptions">Extension options.</param>
        public FlexiTableBlockRenderer(FlexiTableBlocksExtensionOptions extensionOptions)
        {
            _defaultFlexiTableBlockOptions = extensionOptions?.DefaultBlockOptions ?? throw new ArgumentNullException(nameof(extensionOptions));
            _stringWriter = new StringWriter();
            _stripRenderer = new HtmlRenderer(_stringWriter)
            {
                EnableHtmlForBlock = false,
                EnableHtmlForInline = false
            };
        }

        /// <summary>
        /// Renders a FlexiTableBlock as HTML.
        /// </summary>
        /// <param name="renderer">The renderer to write to.</param>
        /// <param name="obj">The FlexiTableBlock to render.</param>
        protected override void WriteFlexiBlock(HtmlRenderer renderer, Table obj)
        {
            // Table's created using the pipe table syntax do not have their own FlexiTableOptions. This is because PipeTableParser is an inline parser and so does not work 
            // with FlexiOptionsBlocks.
            FlexiTableBlockOptions flexiTableBlockOptions = (FlexiTableBlockOptions)obj.GetData(FlexiTableBlocksExtension.FLEXI_TABLE_BLOCK_OPTIONS_KEY) ?? _defaultFlexiTableBlockOptions;

            // Add class to attributes
            IDictionary<string, string> attributes = flexiTableBlockOptions.Attributes;
            if (!string.IsNullOrWhiteSpace(flexiTableBlockOptions.Class))
            {
                attributes = new HtmlAttributeDictionary(attributes)
                {
                    { "class", flexiTableBlockOptions.Class }
                };
            }

            // TODO merge attributes? - ideally, PipeTableParser should be converted to a BlockParser so that the GenericAttributes extension is not required
            // Wrap table in a div. Why?
            // - The "auto" algorithm for determining a table's width basically adds up the minimum content widths (MCW) of each column > https://www.w3.org/TR/CSS2/tables.html#auto-table-layout.
            // - When using "overflow-wrap: break-word" MCW does not take soft wrap oppurtunities into account > https://www.w3.org/TR/css-text-3/#valdef-overflow-wrap-break-word. 
            // - The above two points result in long words not wrapping in table cells. Instead, long words cause long cells, in turn causing tables to overflow their parents.
            // - This will no longer be an issue when "overflow-wrap: anywhere" works.
            // - For now, <table> elements must be wrapped in <div>s with "overflow: auto". It is possible to set "overflow: auto" on tables themselves but this will not always work because table widths
            //   are overriden by sum of MCWs of its columns (i.e even if you set a fixed width for a table, it gets overriden in most cases). It is possible to make "overflow: auto" on tables work by 
            //   setting the table's display to block (Github does this), but this is a hack that just happens to work (that "display: block" doesn't affect rendering of the table, which should have
            //   "display: table", is a coincidence).
            renderer.Write("<div").
                WriteAttributes(attributes).
                WriteAttributes(obj).
                WriteLine(">").
                WriteLine("<table>");

            bool hasBody = false;
            bool hasAlreadyHeader = false;
            bool isHeaderOpen = false;

            bool hasColumnWidth = false;
            foreach (TableColumnDefinition tableColumnDefinition in obj.ColumnDefinitions)
            {
                if (tableColumnDefinition.Width != 0.0f && tableColumnDefinition.Width != 1.0f)
                {
                    hasColumnWidth = true;
                    break;
                }
            }

            if (hasColumnWidth)
            {
                foreach (TableColumnDefinition tableColumnDefinition in obj.ColumnDefinitions)
                {
                    double width = Math.Round(tableColumnDefinition.Width * 100) / 100;
                    string widthValue = string.Format(CultureInfo.InvariantCulture, "{0:0.##}", width);
                    renderer.WriteLine($"<col style=\"width:{widthValue}%\">");
                }
            }

            // Determine whether wrapper and label attributes should be rendered
            bool renderWrapper = !string.IsNullOrWhiteSpace(flexiTableBlockOptions.WrapperElement);
            bool renderLabelAttribute = !string.IsNullOrWhiteSpace(flexiTableBlockOptions.LabelAttribute);

            // Store th contents
            List<string> labels = null;

            foreach (Block rowObj in obj)
            {
                var row = (TableRow)rowObj;
                if (row.IsHeader)
                {
                    // Don't allow more than 1 thead
                    if (!hasAlreadyHeader)
                    {
                        if (renderLabelAttribute)
                        {
                            labels = new List<string>(row.Count);
                        }
                        renderer.WriteLine("<thead>");
                        isHeaderOpen = true;
                    }
                    hasAlreadyHeader = true;
                }
                else if (!hasBody)
                {
                    if (isHeaderOpen)
                    {
                        renderer.WriteLine("</thead>");
                        isHeaderOpen = false;
                    }

                    renderer.WriteLine("<tbody>");
                    hasBody = true;
                }

                renderer.WriteLine("<tr>");
                for (int i = 0; i < row.Count; i++)
                {
                    Block cellObj = row[i];
                    var cell = (TableCell)cellObj;

                    if (row.IsHeader && renderLabelAttribute)
                    {
                        _stripRenderer.Write(cell);
                        labels.Add(_stringWriter.ToString());
                        _stringWriter.GetStringBuilder().Length = 0;
                    }

                    renderer.
                        EnsureLine().
                        Write(row.IsHeader ? "<th" : "<td");

                    if (!row.IsHeader && renderLabelAttribute && labels?.Count > i) // labels may be null if table has no header
                    {
                        renderer.Write($" {flexiTableBlockOptions.LabelAttribute}=\"{labels[i]}\"");
                    }
                    if (cell.ColumnSpan != 1)
                    {
                        renderer.Write($" colspan=\"{cell.ColumnSpan}\"");
                    }
                    if (cell.RowSpan != 1)
                    {
                        renderer.Write($" rowspan=\"{cell.RowSpan}\"");
                    }
                    if (obj.ColumnDefinitions.Count > 0)
                    {
                        int columnIndex = cell.ColumnIndex < 0 || cell.ColumnIndex >= obj.ColumnDefinitions.Count
                            ? i
                            : cell.ColumnIndex;
                        columnIndex = columnIndex >= obj.ColumnDefinitions.Count ? obj.ColumnDefinitions.Count - 1 : columnIndex;
                        TableColumnAlign? alignment = obj.ColumnDefinitions[columnIndex].Alignment;
                        if (alignment.HasValue)
                        {
                            switch (alignment)
                            {
                                case TableColumnAlign.Center:
                                    renderer.Write(" style=\"text-align: center;\"");
                                    break;
                                case TableColumnAlign.Right:
                                    renderer.Write(" style=\"text-align: right;\"");
                                    break;
                                case TableColumnAlign.Left:
                                    renderer.Write(" style=\"text-align: left;\"");
                                    break;
                            }
                        }
                    }
                    renderer.
                        WriteAttributes(cell).
                        Write(">");

                    if (!row.IsHeader && renderWrapper)
                    {
                        renderer.Write($"<{flexiTableBlockOptions.WrapperElement}>");
                    }

                    bool previousImplicitParagraph = renderer.ImplicitParagraph;
                    if (cell.Count == 1)
                    {
                        renderer.ImplicitParagraph = true;
                    }

                    renderer.Write(cell);
                    renderer.ImplicitParagraph = previousImplicitParagraph;

                    if (!row.IsHeader && renderWrapper)
                    {
                        renderer.Write($"</{flexiTableBlockOptions.WrapperElement}>");
                    }
                    renderer.WriteLine(row.IsHeader ? "</th>" : "</td>");
                }
                renderer.WriteLine("</tr>");
            }

            if (hasBody)
            {
                renderer.WriteLine("</tbody>");
            }
            else if (isHeaderOpen)
            {
                renderer.WriteLine("</thead>");
            }
            renderer.
                WriteLine("</table>").
                WriteLine("</div>");
        }
    }
}
