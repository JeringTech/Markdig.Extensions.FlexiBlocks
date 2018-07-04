﻿using Markdig.Extensions.Tables;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks
{
    /// <summary>
    /// A HTML renderer for a <see cref="Table"/>. Based on <see cref="HtmlTableRenderer"/>. This renderer wraps the contents of td elements and adds
    /// a label attribute to each td element.
    /// </summary>
    public class FlexiTableBlockRenderer : HtmlObjectRenderer<Table>
    {
        private readonly HtmlRenderer _stripRenderer;
        private readonly StringWriter _stringWriter;
        private readonly FlexiTableBlockOptions _defaultFlexiTableBlockOptions;

        public FlexiTableBlockRenderer(FlexiTableBlockOptions defaultFlexiTableBlockOptions)
        {
            _defaultFlexiTableBlockOptions = defaultFlexiTableBlockOptions;

            _stringWriter = new StringWriter();
            _stripRenderer = new HtmlRenderer(_stringWriter)
            {
                EnableHtmlForBlock = false,
                EnableHtmlForInline = false
            };
        }

        protected override void Write(HtmlRenderer renderer, Table obj)
        {
            // Table's created using the pipe table syntax do not have their own FlexiTableOptions. This is because PipeTableParser is an inline parser and so does not work 
            // with FlexiOptionsBlocks.
            FlexiTableBlockOptions flexiTableBlockOptions = (FlexiTableBlockOptions)obj.GetData(FlexiTableBlocksExtension.FLEXI_TABLE_BLOCK_OPTIONS_KEY) ?? _defaultFlexiTableBlockOptions;
            bool renderWrapper = !string.IsNullOrWhiteSpace(flexiTableBlockOptions.WrapperElementName);
            bool renderLabelAttribute = !string.IsNullOrWhiteSpace(flexiTableBlockOptions.LabelAttributeName);

            renderer.EnsureLine();
            // TODO merge attributes? - ideally, PipeTableParser should be converted to a BlockParser so that the GenericAttributes extension is not required
            renderer.Write("<table").
                WriteHtmlAttributeDictionary(flexiTableBlockOptions.Attributes).
                WriteAttributes(obj).
                WriteLine(">");

            bool hasBody = false;
            bool hasAlreadyHeader = false;
            bool isHeaderOpen = false;

            bool hasColumnWidth = false;
            foreach (var tableColumnDefinition in obj.ColumnDefinitions)
            {
                if (tableColumnDefinition.Width != 0.0f && tableColumnDefinition.Width != 1.0f)
                {
                    hasColumnWidth = true;
                    break;
                }
            }

            if (hasColumnWidth)
            {
                foreach (var tableColumnDefinition in obj.ColumnDefinitions)
                {
                    double width = Math.Round(tableColumnDefinition.Width * 100) / 100;
                    string widthValue = string.Format(CultureInfo.InvariantCulture, "{0:0.##}", width);
                    renderer.WriteLine($"<col style=\"width:{widthValue}%\">");
                }
            }

            // Store th contents
            List<string> labels = null;

            foreach (var rowObj in obj)
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

                    renderer.EnsureLine();
                    renderer.Write(row.IsHeader ? "<th" : "<td");
                    if (!row.IsHeader && renderLabelAttribute && i < labels.Count)
                    {
                        renderer.Write($" {flexiTableBlockOptions.LabelAttributeName}=\"{labels[i]}\"");
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
                    renderer.WriteAttributes(cell);
                    renderer.Write(">");

                    if (!row.IsHeader && renderWrapper)
                    {
                        renderer.Write($"<{flexiTableBlockOptions.WrapperElementName}>");
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
                        renderer.Write($"</{flexiTableBlockOptions.WrapperElementName}>");
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
            renderer.WriteLine("</table>");
        }
    }
}