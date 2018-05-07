









using Markdig;
using Xunit;

namespace JeremyTCD.Markdig.Extensions.Tests
{

    // Tabs in lines are not expanded to [spaces].  However,
    // in contexts where whitespace helps to define block structure,
    // tabs behave as if they were replaced by spaces with a tab stop
    // of 4 characters  
    public class TabsTests
    {

        // Thus, for example, a tab can be used instead of four spaces
        // in an indented code block.  (Note, however, that internal
        // tabs are passed through as literal tabs, not expanded to
        // spaces.)        
        [Fact]
        public void Tabs_Spec1_CommonMark()
        {
            // The following Markdown:
            //     →foo→baz→→bim
            //
            // Should be rendered as:
            //     <pre><code>foo→baz→→bim
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("\tfoo\tbaz\t\tbim", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>foo\tbaz\t\tbim\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Tabs_Spec2_CommonMark()
        {
            // The following Markdown:
            //       →foo→baz→→bim
            //
            // Should be rendered as:
            //     <pre><code>foo→baz→→bim
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("  \tfoo\tbaz\t\tbim", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>foo\tbaz\t\tbim\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Tabs_Spec3_CommonMark()
        {
            // The following Markdown:
            //         a→a
            //         ὐ→a
            //
            // Should be rendered as:
            //     <pre><code>a→a
            //     ὐ→a
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("    a\ta\n    ὐ\ta", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>a\ta\nὐ\ta\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }

        // In the following example, a continuation paragraph of a list
        // item is indented with a tab; this has exactly the same effect
        // as indentation with four spaces would:        
        [Fact]
        public void Tabs_Spec4_CommonMark()
        {
            // The following Markdown:
            //       - foo
            //     
            //     →bar
            //
            // Should be rendered as:
            //     <ul>
            //     <li>
            //     <p>foo</p>
            //     <p>bar</p>
            //     </li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("  - foo\n\n\tbar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Tabs_Spec5_CommonMark()
        {
            // The following Markdown:
            //     - foo
            //     
            //     →→bar
            //
            // Should be rendered as:
            //     <ul>
            //     <li>
            //     <p>foo</p>
            //     <pre><code>  bar
            //     </code></pre>
            //     </li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- foo\n\n\t\tbar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>\n<p>foo</p>\n<pre><code>  bar\n</code></pre>\n</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }

        // Normally the `>` that begins a block quote may be followed
        // optionally by a space, which is not considered part of the
        // content.  In the following case `>` is followed by a tab,
        // which is treated as if it were expanded into three spaces.
        // Since one of these spaces is considered part of the
        // delimiter, `foo` is considered to be indented six spaces
        // inside the block quote context, so we get an indented
        // code block starting with two spaces.        
        [Fact]
        public void Tabs_Spec6_CommonMark()
        {
            // The following Markdown:
            //     >→→foo
            //
            // Should be rendered as:
            //     <blockquote>
            //     <pre><code>  foo
            //     </code></pre>
            //     </blockquote>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml(">\t\tfoo", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<pre><code>  foo\n</code></pre>\n</blockquote>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Tabs_Spec7_CommonMark()
        {
            // The following Markdown:
            //     -→→foo
            //
            // Should be rendered as:
            //     <ul>
            //     <li>
            //     <pre><code>  foo
            //     </code></pre>
            //     </li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("-\t\tfoo", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>\n<pre><code>  foo\n</code></pre>\n</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Tabs_Spec8_CommonMark()
        {
            // The following Markdown:
            //         foo
            //     →bar
            //
            // Should be rendered as:
            //     <pre><code>foo
            //     bar
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("    foo\n\tbar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>foo\nbar\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Tabs_Spec9_CommonMark()
        {
            // The following Markdown:
            //      - foo
            //        - bar
            //     → - baz
            //
            // Should be rendered as:
            //     <ul>
            //     <li>foo
            //     <ul>
            //     <li>bar
            //     <ul>
            //     <li>baz</li>
            //     </ul>
            //     </li>
            //     </ul>
            //     </li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml(" - foo\n   - bar\n\t - baz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>foo\n<ul>\n<li>bar\n<ul>\n<li>baz</li>\n</ul>\n</li>\n</ul>\n</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Tabs_Spec10_CommonMark()
        {
            // The following Markdown:
            //     #→Foo
            //
            // Should be rendered as:
            //     <h1>Foo</h1>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("#\tFoo", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h1>Foo</h1>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Tabs_Spec11_CommonMark()
        {
            // The following Markdown:
            //     *→*→*→
            //
            // Should be rendered as:
            //     <hr />

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*\t*\t*\t", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<hr />");

            Assert.Equal(expectedResult, result);
        }
    }

    // Indicators of block structure always take precedence over indicators
    // of inline structure.  So, for example, the following is a list with
    // two items, not a list with one item containing a code span:  
    public class PrecedenceTests
    {
        
        [Fact]
        public void Precedence_Spec12_CommonMark()
        {
            // The following Markdown:
            //     - `one
            //     - two`
            //
            // Should be rendered as:
            //     <ul>
            //     <li>`one</li>
            //     <li>two`</li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- `one\n- two`", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>`one</li>\n<li>two`</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }
    }

    // A line consisting of 0-3 spaces of indentation, followed by a sequence
    // of three or more matching `-`, `_`, or `*` characters, each followed
    // optionally by any number of spaces, forms a
    // [thematic break](@).  
    public class ThematicBreaksTests
    {
        
        [Fact]
        public void ThematicBreaks_Spec13_CommonMark()
        {
            // The following Markdown:
            //     ***
            //     ---
            //     ___
            //
            // Should be rendered as:
            //     <hr />
            //     <hr />
            //     <hr />

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("***\n---\n___", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<hr />\n<hr />\n<hr />");

            Assert.Equal(expectedResult, result);
        }

        // Wrong characters:        
        [Fact]
        public void ThematicBreaks_Spec14_CommonMark()
        {
            // The following Markdown:
            //     +++
            //
            // Should be rendered as:
            //     <p>+++</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("+++", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>+++</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void ThematicBreaks_Spec15_CommonMark()
        {
            // The following Markdown:
            //     ===
            //
            // Should be rendered as:
            //     <p>===</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("===", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>===</p>");

            Assert.Equal(expectedResult, result);
        }

        // Not enough characters:        
        [Fact]
        public void ThematicBreaks_Spec16_CommonMark()
        {
            // The following Markdown:
            //     --
            //     **
            //     __
            //
            // Should be rendered as:
            //     <p>--
            //     **
            //     __</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("--\n**\n__", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>--\n**\n__</p>");

            Assert.Equal(expectedResult, result);
        }

        // One to three spaces indent are allowed:        
        [Fact]
        public void ThematicBreaks_Spec17_CommonMark()
        {
            // The following Markdown:
            //      ***
            //       ***
            //        ***
            //
            // Should be rendered as:
            //     <hr />
            //     <hr />
            //     <hr />

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml(" ***\n  ***\n   ***", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<hr />\n<hr />\n<hr />");

            Assert.Equal(expectedResult, result);
        }

        // Four spaces is too many:        
        [Fact]
        public void ThematicBreaks_Spec18_CommonMark()
        {
            // The following Markdown:
            //         ***
            //
            // Should be rendered as:
            //     <pre><code>***
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("    ***", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>***\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void ThematicBreaks_Spec19_CommonMark()
        {
            // The following Markdown:
            //     Foo
            //         ***
            //
            // Should be rendered as:
            //     <p>Foo
            //     ***</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("Foo\n    ***", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>Foo\n***</p>");

            Assert.Equal(expectedResult, result);
        }

        // More than three characters may be used:        
        [Fact]
        public void ThematicBreaks_Spec20_CommonMark()
        {
            // The following Markdown:
            //     _____________________________________
            //
            // Should be rendered as:
            //     <hr />

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("_____________________________________", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<hr />");

            Assert.Equal(expectedResult, result);
        }

        // Spaces are allowed between the characters:        
        [Fact]
        public void ThematicBreaks_Spec21_CommonMark()
        {
            // The following Markdown:
            //      - - -
            //
            // Should be rendered as:
            //     <hr />

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml(" - - -", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<hr />");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void ThematicBreaks_Spec22_CommonMark()
        {
            // The following Markdown:
            //      **  * ** * ** * **
            //
            // Should be rendered as:
            //     <hr />

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml(" **  * ** * ** * **", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<hr />");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void ThematicBreaks_Spec23_CommonMark()
        {
            // The following Markdown:
            //     -     -      -      -
            //
            // Should be rendered as:
            //     <hr />

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("-     -      -      -", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<hr />");

            Assert.Equal(expectedResult, result);
        }

        // Spaces are allowed at the end:        
        [Fact]
        public void ThematicBreaks_Spec24_CommonMark()
        {
            // The following Markdown:
            //     - - - -    
            //
            // Should be rendered as:
            //     <hr />

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- - - -    ", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<hr />");

            Assert.Equal(expectedResult, result);
        }

        // However, no other characters may occur in the line:        
        [Fact]
        public void ThematicBreaks_Spec25_CommonMark()
        {
            // The following Markdown:
            //     _ _ _ _ a
            //     
            //     a------
            //     
            //     ---a---
            //
            // Should be rendered as:
            //     <p>_ _ _ _ a</p>
            //     <p>a------</p>
            //     <p>---a---</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("_ _ _ _ a\n\na------\n\n---a---", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>_ _ _ _ a</p>\n<p>a------</p>\n<p>---a---</p>");

            Assert.Equal(expectedResult, result);
        }

        // It is required that all of the [non-whitespace characters] be the same.
        // So, this is not a thematic break:        
        [Fact]
        public void ThematicBreaks_Spec26_CommonMark()
        {
            // The following Markdown:
            //      *-*
            //
            // Should be rendered as:
            //     <p><em>-</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml(" *-*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>-</em></p>");

            Assert.Equal(expectedResult, result);
        }

        // Thematic breaks do not need blank lines before or after:        
        [Fact]
        public void ThematicBreaks_Spec27_CommonMark()
        {
            // The following Markdown:
            //     - foo
            //     ***
            //     - bar
            //
            // Should be rendered as:
            //     <ul>
            //     <li>foo</li>
            //     </ul>
            //     <hr />
            //     <ul>
            //     <li>bar</li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- foo\n***\n- bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>foo</li>\n</ul>\n<hr />\n<ul>\n<li>bar</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }

        // Thematic breaks can interrupt a paragraph:        
        [Fact]
        public void ThematicBreaks_Spec28_CommonMark()
        {
            // The following Markdown:
            //     Foo
            //     ***
            //     bar
            //
            // Should be rendered as:
            //     <p>Foo</p>
            //     <hr />
            //     <p>bar</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("Foo\n***\nbar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>Foo</p>\n<hr />\n<p>bar</p>");

            Assert.Equal(expectedResult, result);
        }

        // If a line of dashes that meets the above conditions for being a
        // thematic break could also be interpreted as the underline of a [setext
        // heading], the interpretation as a
        // [setext heading] takes precedence. Thus, for example,
        // this is a setext heading, not a paragraph followed by a thematic break:        
        [Fact]
        public void ThematicBreaks_Spec29_CommonMark()
        {
            // The following Markdown:
            //     Foo
            //     ---
            //     bar
            //
            // Should be rendered as:
            //     <h2>Foo</h2>
            //     <p>bar</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("Foo\n---\nbar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h2>Foo</h2>\n<p>bar</p>");

            Assert.Equal(expectedResult, result);
        }

        // When both a thematic break and a list item are possible
        // interpretations of a line, the thematic break takes precedence:        
        [Fact]
        public void ThematicBreaks_Spec30_CommonMark()
        {
            // The following Markdown:
            //     * Foo
            //     * * *
            //     * Bar
            //
            // Should be rendered as:
            //     <ul>
            //     <li>Foo</li>
            //     </ul>
            //     <hr />
            //     <ul>
            //     <li>Bar</li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("* Foo\n* * *\n* Bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>Foo</li>\n</ul>\n<hr />\n<ul>\n<li>Bar</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }

        // If you want a thematic break in a list item, use a different bullet:        
        [Fact]
        public void ThematicBreaks_Spec31_CommonMark()
        {
            // The following Markdown:
            //     - Foo
            //     - * * *
            //
            // Should be rendered as:
            //     <ul>
            //     <li>Foo</li>
            //     <li>
            //     <hr />
            //     </li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- Foo\n- * * *", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>Foo</li>\n<li>\n<hr />\n</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }
    }

    // An [ATX heading](@)
    // consists of a string of characters, parsed as inline content, between an
    // opening sequence of 1--6 unescaped `#` characters and an optional
    // closing sequence of any number of unescaped `#` characters.
    // The opening sequence of `#` characters must be followed by a
    // [space] or by the end of line. The optional closing sequence of `#`s must be
    // preceded by a [space] and may be followed by spaces only.  The opening
    // `#` character may be indented 0-3 spaces.  The raw contents of the
    // heading are stripped of leading and trailing spaces before being parsed
    // as inline content.  The heading level is equal to the number of `#`
    // characters in the opening sequence  
    public class ATXHeadingsTests
    {

        // Simple headings:        
        [Fact]
        public void ATXHeadings_Spec32_CommonMark()
        {
            // The following Markdown:
            //     # foo
            //     ## foo
            //     ### foo
            //     #### foo
            //     ##### foo
            //     ###### foo
            //
            // Should be rendered as:
            //     <h1>foo</h1>
            //     <h2>foo</h2>
            //     <h3>foo</h3>
            //     <h4>foo</h4>
            //     <h5>foo</h5>
            //     <h6>foo</h6>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("# foo\n## foo\n### foo\n#### foo\n##### foo\n###### foo", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h1>foo</h1>\n<h2>foo</h2>\n<h3>foo</h3>\n<h4>foo</h4>\n<h5>foo</h5>\n<h6>foo</h6>");

            Assert.Equal(expectedResult, result);
        }

        // More than six `#` characters is not a heading:        
        [Fact]
        public void ATXHeadings_Spec33_CommonMark()
        {
            // The following Markdown:
            //     ####### foo
            //
            // Should be rendered as:
            //     <p>####### foo</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("####### foo", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>####### foo</p>");

            Assert.Equal(expectedResult, result);
        }

        // At least one space is required between the `#` characters and the
        // heading's contents, unless the heading is empty.  Note that many
        // implementations currently do not require the space.  However, the
        // space was required by the
        // [original ATX implementation](http://www.aaronsw.com/2002/atx/atx.py),
        // and it helps prevent things like the following from being parsed as
        // headings:        
        [Fact]
        public void ATXHeadings_Spec34_CommonMark()
        {
            // The following Markdown:
            //     #5 bolt
            //     
            //     #hashtag
            //
            // Should be rendered as:
            //     <p>#5 bolt</p>
            //     <p>#hashtag</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("#5 bolt\n\n#hashtag", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>#5 bolt</p>\n<p>#hashtag</p>");

            Assert.Equal(expectedResult, result);
        }

        // This is not a heading, because the first `#` is escaped:        
        [Fact]
        public void ATXHeadings_Spec35_CommonMark()
        {
            // The following Markdown:
            //     \## foo
            //
            // Should be rendered as:
            //     <p>## foo</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("\\## foo", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>## foo</p>");

            Assert.Equal(expectedResult, result);
        }

        // Contents are parsed as inlines:        
        [Fact]
        public void ATXHeadings_Spec36_CommonMark()
        {
            // The following Markdown:
            //     # foo *bar* \*baz\*
            //
            // Should be rendered as:
            //     <h1>foo <em>bar</em> *baz*</h1>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("# foo *bar* \\*baz\\*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h1>foo <em>bar</em> *baz*</h1>");

            Assert.Equal(expectedResult, result);
        }

        // Leading and trailing blanks are ignored in parsing inline content:        
        [Fact]
        public void ATXHeadings_Spec37_CommonMark()
        {
            // The following Markdown:
            //     #                  foo                     
            //
            // Should be rendered as:
            //     <h1>foo</h1>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("#                  foo                     ", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h1>foo</h1>");

            Assert.Equal(expectedResult, result);
        }

        // One to three spaces indentation are allowed:        
        [Fact]
        public void ATXHeadings_Spec38_CommonMark()
        {
            // The following Markdown:
            //      ### foo
            //       ## foo
            //        # foo
            //
            // Should be rendered as:
            //     <h3>foo</h3>
            //     <h2>foo</h2>
            //     <h1>foo</h1>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml(" ### foo\n  ## foo\n   # foo", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h3>foo</h3>\n<h2>foo</h2>\n<h1>foo</h1>");

            Assert.Equal(expectedResult, result);
        }

        // Four spaces are too much:        
        [Fact]
        public void ATXHeadings_Spec39_CommonMark()
        {
            // The following Markdown:
            //         # foo
            //
            // Should be rendered as:
            //     <pre><code># foo
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("    # foo", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code># foo\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void ATXHeadings_Spec40_CommonMark()
        {
            // The following Markdown:
            //     foo
            //         # bar
            //
            // Should be rendered as:
            //     <p>foo
            //     # bar</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo\n    # bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo\n# bar</p>");

            Assert.Equal(expectedResult, result);
        }

        // A closing sequence of `#` characters is optional:        
        [Fact]
        public void ATXHeadings_Spec41_CommonMark()
        {
            // The following Markdown:
            //     ## foo ##
            //       ###   bar    ###
            //
            // Should be rendered as:
            //     <h2>foo</h2>
            //     <h3>bar</h3>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("## foo ##\n  ###   bar    ###", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h2>foo</h2>\n<h3>bar</h3>");

            Assert.Equal(expectedResult, result);
        }

        // It need not be the same length as the opening sequence:        
        [Fact]
        public void ATXHeadings_Spec42_CommonMark()
        {
            // The following Markdown:
            //     # foo ##################################
            //     ##### foo ##
            //
            // Should be rendered as:
            //     <h1>foo</h1>
            //     <h5>foo</h5>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("# foo ##################################\n##### foo ##", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h1>foo</h1>\n<h5>foo</h5>");

            Assert.Equal(expectedResult, result);
        }

        // Spaces are allowed after the closing sequence:        
        [Fact]
        public void ATXHeadings_Spec43_CommonMark()
        {
            // The following Markdown:
            //     ### foo ###     
            //
            // Should be rendered as:
            //     <h3>foo</h3>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("### foo ###     ", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h3>foo</h3>");

            Assert.Equal(expectedResult, result);
        }

        // A sequence of `#` characters with anything but [spaces] following it
        // is not a closing sequence, but counts as part of the contents of the
        // heading:        
        [Fact]
        public void ATXHeadings_Spec44_CommonMark()
        {
            // The following Markdown:
            //     ### foo ### b
            //
            // Should be rendered as:
            //     <h3>foo ### b</h3>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("### foo ### b", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h3>foo ### b</h3>");

            Assert.Equal(expectedResult, result);
        }

        // The closing sequence must be preceded by a space:        
        [Fact]
        public void ATXHeadings_Spec45_CommonMark()
        {
            // The following Markdown:
            //     # foo#
            //
            // Should be rendered as:
            //     <h1>foo#</h1>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("# foo#", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h1>foo#</h1>");

            Assert.Equal(expectedResult, result);
        }

        // Backslash-escaped `#` characters do not count as part
        // of the closing sequence:        
        [Fact]
        public void ATXHeadings_Spec46_CommonMark()
        {
            // The following Markdown:
            //     ### foo \###
            //     ## foo #\##
            //     # foo \#
            //
            // Should be rendered as:
            //     <h3>foo ###</h3>
            //     <h2>foo ###</h2>
            //     <h1>foo #</h1>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("### foo \\###\n## foo #\\##\n# foo \\#", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h3>foo ###</h3>\n<h2>foo ###</h2>\n<h1>foo #</h1>");

            Assert.Equal(expectedResult, result);
        }

        // ATX headings need not be separated from surrounding content by blank
        // lines, and they can interrupt paragraphs:        
        [Fact]
        public void ATXHeadings_Spec47_CommonMark()
        {
            // The following Markdown:
            //     ****
            //     ## foo
            //     ****
            //
            // Should be rendered as:
            //     <hr />
            //     <h2>foo</h2>
            //     <hr />

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("****\n## foo\n****", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<hr />\n<h2>foo</h2>\n<hr />");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void ATXHeadings_Spec48_CommonMark()
        {
            // The following Markdown:
            //     Foo bar
            //     # baz
            //     Bar foo
            //
            // Should be rendered as:
            //     <p>Foo bar</p>
            //     <h1>baz</h1>
            //     <p>Bar foo</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("Foo bar\n# baz\nBar foo", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>Foo bar</p>\n<h1>baz</h1>\n<p>Bar foo</p>");

            Assert.Equal(expectedResult, result);
        }

        // ATX headings can be empty:        
        [Fact]
        public void ATXHeadings_Spec49_CommonMark()
        {
            // The following Markdown:
            //     ## 
            //     #
            //     ### ###
            //
            // Should be rendered as:
            //     <h2></h2>
            //     <h1></h1>
            //     <h3></h3>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("## \n#\n### ###", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h2></h2>\n<h1></h1>\n<h3></h3>");

            Assert.Equal(expectedResult, result);
        }
    }

    // A [setext heading](@) consists of one or more
    // lines of text, each containing at least one [non-whitespace
    // character], with no more than 3 spaces indentation, followed by
    // a [setext heading underline].  The lines of text must be such
    // that, were they not followed by the setext heading underline,
    // they would be interpreted as a paragraph:  they cannot be
    // interpretable as a [code fence], [ATX heading][ATX headings],
    // [block quote][block quotes], [thematic break][thematic breaks],
    // [list item][list items], or [HTML block][HTML blocks].
    // 
    // A [setext heading underline](@) is a sequence of
    // `=` characters or a sequence of `-` characters, with no more than 3
    // spaces indentation and any number of trailing spaces.  If a line
    // containing a single `-` can be interpreted as an
    // empty [list items], it should be interpreted this way
    // and not as a [setext heading underline].
    // 
    // The heading is a level 1 heading if `=` characters are used in
    // the [setext heading underline], and a level 2 heading if `-`
    // characters are used.  The contents of the heading are the result
    // of parsing the preceding lines of text as CommonMark inline
    // content.
    // 
    // In general, a setext heading need not be preceded or followed by a
    // blank line.  However, it cannot interrupt a paragraph, so when a
    // setext heading comes after a paragraph, a blank line is needed between
    // them  
    public class SetextHeadingsTests
    {

        // Simple examples:        
        [Fact]
        public void SetextHeadings_Spec50_CommonMark()
        {
            // The following Markdown:
            //     Foo *bar*
            //     =========
            //     
            //     Foo *bar*
            //     ---------
            //
            // Should be rendered as:
            //     <h1>Foo <em>bar</em></h1>
            //     <h2>Foo <em>bar</em></h2>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("Foo *bar*\n=========\n\nFoo *bar*\n---------", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h1>Foo <em>bar</em></h1>\n<h2>Foo <em>bar</em></h2>");

            Assert.Equal(expectedResult, result);
        }

        // The content of the header may span more than one line:        
        [Fact]
        public void SetextHeadings_Spec51_CommonMark()
        {
            // The following Markdown:
            //     Foo *bar
            //     baz*
            //     ====
            //
            // Should be rendered as:
            //     <h1>Foo <em>bar
            //     baz</em></h1>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("Foo *bar\nbaz*\n====", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h1>Foo <em>bar\nbaz</em></h1>");

            Assert.Equal(expectedResult, result);
        }

        // The underlining can be any length:        
        [Fact]
        public void SetextHeadings_Spec52_CommonMark()
        {
            // The following Markdown:
            //     Foo
            //     -------------------------
            //     
            //     Foo
            //     =
            //
            // Should be rendered as:
            //     <h2>Foo</h2>
            //     <h1>Foo</h1>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("Foo\n-------------------------\n\nFoo\n=", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h2>Foo</h2>\n<h1>Foo</h1>");

            Assert.Equal(expectedResult, result);
        }

        // The heading content can be indented up to three spaces, and need
        // not line up with the underlining:        
        [Fact]
        public void SetextHeadings_Spec53_CommonMark()
        {
            // The following Markdown:
            //        Foo
            //     ---
            //     
            //       Foo
            //     -----
            //     
            //       Foo
            //       ===
            //
            // Should be rendered as:
            //     <h2>Foo</h2>
            //     <h2>Foo</h2>
            //     <h1>Foo</h1>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("   Foo\n---\n\n  Foo\n-----\n\n  Foo\n  ===", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h2>Foo</h2>\n<h2>Foo</h2>\n<h1>Foo</h1>");

            Assert.Equal(expectedResult, result);
        }

        // Four spaces indent is too much:        
        [Fact]
        public void SetextHeadings_Spec54_CommonMark()
        {
            // The following Markdown:
            //         Foo
            //         ---
            //     
            //         Foo
            //     ---
            //
            // Should be rendered as:
            //     <pre><code>Foo
            //     ---
            //     
            //     Foo
            //     </code></pre>
            //     <hr />

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("    Foo\n    ---\n\n    Foo\n---", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>Foo\n---\n\nFoo\n</code></pre>\n<hr />");

            Assert.Equal(expectedResult, result);
        }

        // The setext heading underline can be indented up to three spaces, and
        // may have trailing spaces:        
        [Fact]
        public void SetextHeadings_Spec55_CommonMark()
        {
            // The following Markdown:
            //     Foo
            //        ----      
            //
            // Should be rendered as:
            //     <h2>Foo</h2>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("Foo\n   ----      ", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h2>Foo</h2>");

            Assert.Equal(expectedResult, result);
        }

        // Four spaces is too much:        
        [Fact]
        public void SetextHeadings_Spec56_CommonMark()
        {
            // The following Markdown:
            //     Foo
            //         ---
            //
            // Should be rendered as:
            //     <p>Foo
            //     ---</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("Foo\n    ---", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>Foo\n---</p>");

            Assert.Equal(expectedResult, result);
        }

        // The setext heading underline cannot contain internal spaces:        
        [Fact]
        public void SetextHeadings_Spec57_CommonMark()
        {
            // The following Markdown:
            //     Foo
            //     = =
            //     
            //     Foo
            //     --- -
            //
            // Should be rendered as:
            //     <p>Foo
            //     = =</p>
            //     <p>Foo</p>
            //     <hr />

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("Foo\n= =\n\nFoo\n--- -", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>Foo\n= =</p>\n<p>Foo</p>\n<hr />");

            Assert.Equal(expectedResult, result);
        }

        // Trailing spaces in the content line do not cause a line break:        
        [Fact]
        public void SetextHeadings_Spec58_CommonMark()
        {
            // The following Markdown:
            //     Foo  
            //     -----
            //
            // Should be rendered as:
            //     <h2>Foo</h2>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("Foo  \n-----", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h2>Foo</h2>");

            Assert.Equal(expectedResult, result);
        }

        // Nor does a backslash at the end:        
        [Fact]
        public void SetextHeadings_Spec59_CommonMark()
        {
            // The following Markdown:
            //     Foo\
            //     ----
            //
            // Should be rendered as:
            //     <h2>Foo\</h2>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("Foo\\\n----", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h2>Foo\\</h2>");

            Assert.Equal(expectedResult, result);
        }

        // Since indicators of block structure take precedence over
        // indicators of inline structure, the following are setext headings:        
        [Fact]
        public void SetextHeadings_Spec60_CommonMark()
        {
            // The following Markdown:
            //     `Foo
            //     ----
            //     `
            //     
            //     <a title="a lot
            //     ---
            //     of dashes"/>
            //
            // Should be rendered as:
            //     <h2>`Foo</h2>
            //     <p>`</p>
            //     <h2>&lt;a title=&quot;a lot</h2>
            //     <p>of dashes&quot;/&gt;</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("`Foo\n----\n`\n\n<a title=\"a lot\n---\nof dashes\"/>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h2>`Foo</h2>\n<p>`</p>\n<h2>&lt;a title=&quot;a lot</h2>\n<p>of dashes&quot;/&gt;</p>");

            Assert.Equal(expectedResult, result);
        }

        // The setext heading underline cannot be a [lazy continuation
        // line] in a list item or block quote:        
        [Fact]
        public void SetextHeadings_Spec61_CommonMark()
        {
            // The following Markdown:
            //     > Foo
            //     ---
            //
            // Should be rendered as:
            //     <blockquote>
            //     <p>Foo</p>
            //     </blockquote>
            //     <hr />

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("> Foo\n---", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<p>Foo</p>\n</blockquote>\n<hr />");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void SetextHeadings_Spec62_CommonMark()
        {
            // The following Markdown:
            //     > foo
            //     bar
            //     ===
            //
            // Should be rendered as:
            //     <blockquote>
            //     <p>foo
            //     bar
            //     ===</p>
            //     </blockquote>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("> foo\nbar\n===", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<p>foo\nbar\n===</p>\n</blockquote>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void SetextHeadings_Spec63_CommonMark()
        {
            // The following Markdown:
            //     - Foo
            //     ---
            //
            // Should be rendered as:
            //     <ul>
            //     <li>Foo</li>
            //     </ul>
            //     <hr />

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- Foo\n---", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>Foo</li>\n</ul>\n<hr />");

            Assert.Equal(expectedResult, result);
        }

        // A blank line is needed between a paragraph and a following
        // setext heading, since otherwise the paragraph becomes part
        // of the heading's content:        
        [Fact]
        public void SetextHeadings_Spec64_CommonMark()
        {
            // The following Markdown:
            //     Foo
            //     Bar
            //     ---
            //
            // Should be rendered as:
            //     <h2>Foo
            //     Bar</h2>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("Foo\nBar\n---", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h2>Foo\nBar</h2>");

            Assert.Equal(expectedResult, result);
        }

        // But in general a blank line is not required before or after
        // setext headings:        
        [Fact]
        public void SetextHeadings_Spec65_CommonMark()
        {
            // The following Markdown:
            //     ---
            //     Foo
            //     ---
            //     Bar
            //     ---
            //     Baz
            //
            // Should be rendered as:
            //     <hr />
            //     <h2>Foo</h2>
            //     <h2>Bar</h2>
            //     <p>Baz</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("---\nFoo\n---\nBar\n---\nBaz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<hr />\n<h2>Foo</h2>\n<h2>Bar</h2>\n<p>Baz</p>");

            Assert.Equal(expectedResult, result);
        }

        // Setext headings cannot be empty:        
        [Fact]
        public void SetextHeadings_Spec66_CommonMark()
        {
            // The following Markdown:
            //     
            //     ====
            //
            // Should be rendered as:
            //     <p>====</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("\n====", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>====</p>");

            Assert.Equal(expectedResult, result);
        }

        // Setext heading text lines must not be interpretable as block
        // constructs other than paragraphs.  So, the line of dashes
        // in these examples gets interpreted as a thematic break:        
        [Fact]
        public void SetextHeadings_Spec67_CommonMark()
        {
            // The following Markdown:
            //     ---
            //     ---
            //
            // Should be rendered as:
            //     <hr />
            //     <hr />

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("---\n---", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<hr />\n<hr />");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void SetextHeadings_Spec68_CommonMark()
        {
            // The following Markdown:
            //     - foo
            //     -----
            //
            // Should be rendered as:
            //     <ul>
            //     <li>foo</li>
            //     </ul>
            //     <hr />

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- foo\n-----", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>foo</li>\n</ul>\n<hr />");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void SetextHeadings_Spec69_CommonMark()
        {
            // The following Markdown:
            //         foo
            //     ---
            //
            // Should be rendered as:
            //     <pre><code>foo
            //     </code></pre>
            //     <hr />

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("    foo\n---", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>foo\n</code></pre>\n<hr />");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void SetextHeadings_Spec70_CommonMark()
        {
            // The following Markdown:
            //     > foo
            //     -----
            //
            // Should be rendered as:
            //     <blockquote>
            //     <p>foo</p>
            //     </blockquote>
            //     <hr />

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("> foo\n-----", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<p>foo</p>\n</blockquote>\n<hr />");

            Assert.Equal(expectedResult, result);
        }

        // If you want a heading with `> foo` as its literal text, you can
        // use backslash escapes:        
        [Fact]
        public void SetextHeadings_Spec71_CommonMark()
        {
            // The following Markdown:
            //     \> foo
            //     ------
            //
            // Should be rendered as:
            //     <h2>&gt; foo</h2>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("\\> foo\n------", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h2>&gt; foo</h2>");

            Assert.Equal(expectedResult, result);
        }

        // **Compatibility note:**  Most existing Markdown implementations
        // do not allow the text of setext headings to span multiple lines.
        // But there is no consensus about how to interpret
        // 
        // ``` markdown
        // Foo
        // bar
        // ---
        // baz
        // ```
        // 
        // One can find four different interpretations:
        // 
        // 1. paragraph "Foo", heading "bar", paragraph "baz"
        // 2. paragraph "Foo bar", thematic break, paragraph "baz"
        // 3. paragraph "Foo bar --- baz"
        // 4. heading "Foo bar", paragraph "baz"
        // 
        // We find interpretation 4 most natural, and interpretation 4
        // increases the expressive power of CommonMark, by allowing
        // multiline headings.  Authors who want interpretation 1 can
        // put a blank line after the first paragraph:        
        [Fact]
        public void SetextHeadings_Spec72_CommonMark()
        {
            // The following Markdown:
            //     Foo
            //     
            //     bar
            //     ---
            //     baz
            //
            // Should be rendered as:
            //     <p>Foo</p>
            //     <h2>bar</h2>
            //     <p>baz</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("Foo\n\nbar\n---\nbaz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>Foo</p>\n<h2>bar</h2>\n<p>baz</p>");

            Assert.Equal(expectedResult, result);
        }

        // Authors who want interpretation 2 can put blank lines around
        // the thematic break,        
        [Fact]
        public void SetextHeadings_Spec73_CommonMark()
        {
            // The following Markdown:
            //     Foo
            //     bar
            //     
            //     ---
            //     
            //     baz
            //
            // Should be rendered as:
            //     <p>Foo
            //     bar</p>
            //     <hr />
            //     <p>baz</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("Foo\nbar\n\n---\n\nbaz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>Foo\nbar</p>\n<hr />\n<p>baz</p>");

            Assert.Equal(expectedResult, result);
        }

        // or use a thematic break that cannot count as a [setext heading
        // underline], such as        
        [Fact]
        public void SetextHeadings_Spec74_CommonMark()
        {
            // The following Markdown:
            //     Foo
            //     bar
            //     * * *
            //     baz
            //
            // Should be rendered as:
            //     <p>Foo
            //     bar</p>
            //     <hr />
            //     <p>baz</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("Foo\nbar\n* * *\nbaz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>Foo\nbar</p>\n<hr />\n<p>baz</p>");

            Assert.Equal(expectedResult, result);
        }

        // Authors who want interpretation 3 can use backslash escapes:        
        [Fact]
        public void SetextHeadings_Spec75_CommonMark()
        {
            // The following Markdown:
            //     Foo
            //     bar
            //     \---
            //     baz
            //
            // Should be rendered as:
            //     <p>Foo
            //     bar
            //     ---
            //     baz</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("Foo\nbar\n\\---\nbaz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>Foo\nbar\n---\nbaz</p>");

            Assert.Equal(expectedResult, result);
        }
    }

    // An [indented code block](@) is composed of one or more
    // [indented chunks] separated by blank lines.
    // An [indented chunk](@) is a sequence of non-blank lines,
    // each indented four or more spaces. The contents of the code block are
    // the literal contents of the lines, including trailing
    // [line endings], minus four spaces of indentation.
    // An indented code block has no [info string]  
    public class IndentedCodeBlocksTests
    {

        // An indented code block cannot interrupt a paragraph, so there must be
        // a blank line between a paragraph and a following indented code block.
        // (A blank line is not needed, however, between a code block and a following
        // paragraph.)        
        [Fact]
        public void IndentedCodeBlocks_Spec76_CommonMark()
        {
            // The following Markdown:
            //         a simple
            //           indented code block
            //
            // Should be rendered as:
            //     <pre><code>a simple
            //       indented code block
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("    a simple\n      indented code block", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>a simple\n  indented code block\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }

        // If there is any ambiguity between an interpretation of indentation
        // as a code block and as indicating that material belongs to a [list
        // item][list items], the list item interpretation takes precedence:        
        [Fact]
        public void IndentedCodeBlocks_Spec77_CommonMark()
        {
            // The following Markdown:
            //       - foo
            //     
            //         bar
            //
            // Should be rendered as:
            //     <ul>
            //     <li>
            //     <p>foo</p>
            //     <p>bar</p>
            //     </li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("  - foo\n\n    bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void IndentedCodeBlocks_Spec78_CommonMark()
        {
            // The following Markdown:
            //     1.  foo
            //     
            //         - bar
            //
            // Should be rendered as:
            //     <ol>
            //     <li>
            //     <p>foo</p>
            //     <ul>
            //     <li>bar</li>
            //     </ul>
            //     </li>
            //     </ol>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("1.  foo\n\n    - bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ol>\n<li>\n<p>foo</p>\n<ul>\n<li>bar</li>\n</ul>\n</li>\n</ol>");

            Assert.Equal(expectedResult, result);
        }

        // The contents of a code block are literal text, and do not get parsed
        // as Markdown:        
        [Fact]
        public void IndentedCodeBlocks_Spec79_CommonMark()
        {
            // The following Markdown:
            //         <a/>
            //         *hi*
            //     
            //         - one
            //
            // Should be rendered as:
            //     <pre><code>&lt;a/&gt;
            //     *hi*
            //     
            //     - one
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("    <a/>\n    *hi*\n\n    - one", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>&lt;a/&gt;\n*hi*\n\n- one\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }

        // Here we have three chunks separated by blank lines:        
        [Fact]
        public void IndentedCodeBlocks_Spec80_CommonMark()
        {
            // The following Markdown:
            //         chunk1
            //     
            //         chunk2
            //       
            //      
            //      
            //         chunk3
            //
            // Should be rendered as:
            //     <pre><code>chunk1
            //     
            //     chunk2
            //     
            //     
            //     
            //     chunk3
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("    chunk1\n\n    chunk2\n  \n \n \n    chunk3", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>chunk1\n\nchunk2\n\n\n\nchunk3\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }

        // Any initial spaces beyond four will be included in the content, even
        // in interior blank lines:        
        [Fact]
        public void IndentedCodeBlocks_Spec81_CommonMark()
        {
            // The following Markdown:
            //         chunk1
            //           
            //           chunk2
            //
            // Should be rendered as:
            //     <pre><code>chunk1
            //       
            //       chunk2
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("    chunk1\n      \n      chunk2", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>chunk1\n  \n  chunk2\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }

        // An indented code block cannot interrupt a paragraph.  (This
        // allows hanging indents and the like.)        
        [Fact]
        public void IndentedCodeBlocks_Spec82_CommonMark()
        {
            // The following Markdown:
            //     Foo
            //         bar
            //     
            //
            // Should be rendered as:
            //     <p>Foo
            //     bar</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("Foo\n    bar\n", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>Foo\nbar</p>");

            Assert.Equal(expectedResult, result);
        }

        // However, any non-blank line with fewer than four leading spaces ends
        // the code block immediately.  So a paragraph may occur immediately
        // after indented code:        
        [Fact]
        public void IndentedCodeBlocks_Spec83_CommonMark()
        {
            // The following Markdown:
            //         foo
            //     bar
            //
            // Should be rendered as:
            //     <pre><code>foo
            //     </code></pre>
            //     <p>bar</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("    foo\nbar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>foo\n</code></pre>\n<p>bar</p>");

            Assert.Equal(expectedResult, result);
        }

        // And indented code can occur immediately before and after other kinds of
        // blocks:        
        [Fact]
        public void IndentedCodeBlocks_Spec84_CommonMark()
        {
            // The following Markdown:
            //     # Heading
            //         foo
            //     Heading
            //     ------
            //         foo
            //     ----
            //
            // Should be rendered as:
            //     <h1>Heading</h1>
            //     <pre><code>foo
            //     </code></pre>
            //     <h2>Heading</h2>
            //     <pre><code>foo
            //     </code></pre>
            //     <hr />

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("# Heading\n    foo\nHeading\n------\n    foo\n----", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h1>Heading</h1>\n<pre><code>foo\n</code></pre>\n<h2>Heading</h2>\n<pre><code>foo\n</code></pre>\n<hr />");

            Assert.Equal(expectedResult, result);
        }

        // The first line can be indented more than four spaces:        
        [Fact]
        public void IndentedCodeBlocks_Spec85_CommonMark()
        {
            // The following Markdown:
            //             foo
            //         bar
            //
            // Should be rendered as:
            //     <pre><code>    foo
            //     bar
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("        foo\n    bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>    foo\nbar\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }

        // Blank lines preceding or following an indented code block
        // are not included in it:        
        [Fact]
        public void IndentedCodeBlocks_Spec86_CommonMark()
        {
            // The following Markdown:
            //     
            //         
            //         foo
            //         
            //     
            //
            // Should be rendered as:
            //     <pre><code>foo
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("\n    \n    foo\n    \n", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>foo\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }

        // Trailing spaces are included in the code block's content:        
        [Fact]
        public void IndentedCodeBlocks_Spec87_CommonMark()
        {
            // The following Markdown:
            //         foo  
            //
            // Should be rendered as:
            //     <pre><code>foo  
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("    foo  ", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>foo  \n</code></pre>");

            Assert.Equal(expectedResult, result);
        }
    }

    // A [code fence](@) is a sequence
    // of at least three consecutive backtick characters (`` ` ``) or
    // tildes (`~`).  (Tildes and backticks cannot be mixed.)
    // A [fenced code block](@)
    // begins with a code fence, indented no more than three spaces.
    // 
    // The line with the opening code fence may optionally contain some text
    // following the code fence; this is trimmed of leading and trailing
    // spaces and called the [info string](@).
    // The [info string] may not contain any backtick
    // characters.  (The reason for this restriction is that otherwise
    // some inline code would be incorrectly interpreted as the
    // beginning of a fenced code block.)
    // 
    // The content of the code block consists of all subsequent lines, until
    // a closing [code fence] of the same type as the code block
    // began with (backticks or tildes), and with at least as many backticks
    // or tildes as the opening code fence.  If the leading code fence is
    // indented N spaces, then up to N spaces of indentation are removed from
    // each line of the content (if present).  (If a content line is not
    // indented, it is preserved unchanged.  If it is indented less than N
    // spaces, all of the indentation is removed.)
    // 
    // The closing code fence may be indented up to three spaces, and may be
    // followed only by spaces, which are ignored.  If the end of the
    // containing block (or document) is reached and no closing code fence
    // has been found, the code block contains all of the lines after the
    // opening code fence until the end of the containing block (or
    // document).  (An alternative spec would require backtracking in the
    // event that a closing code fence is not found.  But this makes parsing
    // much less efficient, and there seems to be no real down side to the
    // behavior described here.)
    // 
    // A fenced code block may interrupt a paragraph, and does not require
    // a blank line either before or after.
    // 
    // The content of a code fence is treated as literal text, not parsed
    // as inlines.  The first word of the [info string] is typically used to
    // specify the language of the code sample, and rendered in the `class`
    // attribute of the `code` tag.  However, this spec does not mandate any
    // particular treatment of the [info string]  
    public class FencedCodeBlocksTests
    {

        // Here is a simple example with backticks:        
        [Fact]
        public void FencedCodeBlocks_Spec88_CommonMark()
        {
            // The following Markdown:
            //     ```
            //     <
            //      >
            //     ```
            //
            // Should be rendered as:
            //     <pre><code>&lt;
            //      &gt;
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("```\n<\n >\n```", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>&lt;\n &gt;\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }

        // With tildes:        
        [Fact]
        public void FencedCodeBlocks_Spec89_CommonMark()
        {
            // The following Markdown:
            //     ~~~
            //     <
            //      >
            //     ~~~
            //
            // Should be rendered as:
            //     <pre><code>&lt;
            //      &gt;
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("~~~\n<\n >\n~~~", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>&lt;\n &gt;\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }

        // Fewer than three backticks is not enough:        
        [Fact]
        public void FencedCodeBlocks_Spec90_CommonMark()
        {
            // The following Markdown:
            //     ``
            //     foo
            //     ``
            //
            // Should be rendered as:
            //     <p><code>foo</code></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("``\nfoo\n``", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><code>foo</code></p>");

            Assert.Equal(expectedResult, result);
        }

        // The closing code fence must use the same character as the opening
        // fence:        
        [Fact]
        public void FencedCodeBlocks_Spec91_CommonMark()
        {
            // The following Markdown:
            //     ```
            //     aaa
            //     ~~~
            //     ```
            //
            // Should be rendered as:
            //     <pre><code>aaa
            //     ~~~
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("```\naaa\n~~~\n```", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>aaa\n~~~\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void FencedCodeBlocks_Spec92_CommonMark()
        {
            // The following Markdown:
            //     ~~~
            //     aaa
            //     ```
            //     ~~~
            //
            // Should be rendered as:
            //     <pre><code>aaa
            //     ```
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("~~~\naaa\n```\n~~~", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>aaa\n```\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }

        // The closing code fence must be at least as long as the opening fence:        
        [Fact]
        public void FencedCodeBlocks_Spec93_CommonMark()
        {
            // The following Markdown:
            //     ````
            //     aaa
            //     ```
            //     ``````
            //
            // Should be rendered as:
            //     <pre><code>aaa
            //     ```
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("````\naaa\n```\n``````", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>aaa\n```\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void FencedCodeBlocks_Spec94_CommonMark()
        {
            // The following Markdown:
            //     ~~~~
            //     aaa
            //     ~~~
            //     ~~~~
            //
            // Should be rendered as:
            //     <pre><code>aaa
            //     ~~~
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("~~~~\naaa\n~~~\n~~~~", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>aaa\n~~~\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }

        // Unclosed code blocks are closed by the end of the document
        // (or the enclosing [block quote][block quotes] or [list item][list items]):        
        [Fact]
        public void FencedCodeBlocks_Spec95_CommonMark()
        {
            // The following Markdown:
            //     ```
            //
            // Should be rendered as:
            //     <pre><code></code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("```", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code></code></pre>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void FencedCodeBlocks_Spec96_CommonMark()
        {
            // The following Markdown:
            //     `````
            //     
            //     ```
            //     aaa
            //
            // Should be rendered as:
            //     <pre><code>
            //     ```
            //     aaa
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("`````\n\n```\naaa", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>\n```\naaa\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void FencedCodeBlocks_Spec97_CommonMark()
        {
            // The following Markdown:
            //     > ```
            //     > aaa
            //     
            //     bbb
            //
            // Should be rendered as:
            //     <blockquote>
            //     <pre><code>aaa
            //     </code></pre>
            //     </blockquote>
            //     <p>bbb</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("> ```\n> aaa\n\nbbb", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<pre><code>aaa\n</code></pre>\n</blockquote>\n<p>bbb</p>");

            Assert.Equal(expectedResult, result);
        }

        // A code block can have all empty lines as its content:        
        [Fact]
        public void FencedCodeBlocks_Spec98_CommonMark()
        {
            // The following Markdown:
            //     ```
            //     
            //       
            //     ```
            //
            // Should be rendered as:
            //     <pre><code>
            //       
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("```\n\n  \n```", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>\n  \n</code></pre>");

            Assert.Equal(expectedResult, result);
        }

        // A code block can be empty:        
        [Fact]
        public void FencedCodeBlocks_Spec99_CommonMark()
        {
            // The following Markdown:
            //     ```
            //     ```
            //
            // Should be rendered as:
            //     <pre><code></code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("```\n```", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code></code></pre>");

            Assert.Equal(expectedResult, result);
        }

        // Fences can be indented.  If the opening fence is indented,
        // content lines will have equivalent opening indentation removed,
        // if present:        
        [Fact]
        public void FencedCodeBlocks_Spec100_CommonMark()
        {
            // The following Markdown:
            //      ```
            //      aaa
            //     aaa
            //     ```
            //
            // Should be rendered as:
            //     <pre><code>aaa
            //     aaa
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml(" ```\n aaa\naaa\n```", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>aaa\naaa\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void FencedCodeBlocks_Spec101_CommonMark()
        {
            // The following Markdown:
            //       ```
            //     aaa
            //       aaa
            //     aaa
            //       ```
            //
            // Should be rendered as:
            //     <pre><code>aaa
            //     aaa
            //     aaa
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("  ```\naaa\n  aaa\naaa\n  ```", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>aaa\naaa\naaa\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void FencedCodeBlocks_Spec102_CommonMark()
        {
            // The following Markdown:
            //        ```
            //        aaa
            //         aaa
            //       aaa
            //        ```
            //
            // Should be rendered as:
            //     <pre><code>aaa
            //      aaa
            //     aaa
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("   ```\n   aaa\n    aaa\n  aaa\n   ```", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>aaa\n aaa\naaa\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }

        // Four spaces indentation produces an indented code block:        
        [Fact]
        public void FencedCodeBlocks_Spec103_CommonMark()
        {
            // The following Markdown:
            //         ```
            //         aaa
            //         ```
            //
            // Should be rendered as:
            //     <pre><code>```
            //     aaa
            //     ```
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("    ```\n    aaa\n    ```", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>```\naaa\n```\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }

        // Closing fences may be indented by 0-3 spaces, and their indentation
        // need not match that of the opening fence:        
        [Fact]
        public void FencedCodeBlocks_Spec104_CommonMark()
        {
            // The following Markdown:
            //     ```
            //     aaa
            //       ```
            //
            // Should be rendered as:
            //     <pre><code>aaa
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("```\naaa\n  ```", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>aaa\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void FencedCodeBlocks_Spec105_CommonMark()
        {
            // The following Markdown:
            //        ```
            //     aaa
            //       ```
            //
            // Should be rendered as:
            //     <pre><code>aaa
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("   ```\naaa\n  ```", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>aaa\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }

        // This is not a closing fence, because it is indented 4 spaces:        
        [Fact]
        public void FencedCodeBlocks_Spec106_CommonMark()
        {
            // The following Markdown:
            //     ```
            //     aaa
            //         ```
            //
            // Should be rendered as:
            //     <pre><code>aaa
            //         ```
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("```\naaa\n    ```", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>aaa\n    ```\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }

        // Code fences (opening and closing) cannot contain internal spaces:        
        [Fact]
        public void FencedCodeBlocks_Spec107_CommonMark()
        {
            // The following Markdown:
            //     ``` ```
            //     aaa
            //
            // Should be rendered as:
            //     <p><code></code>
            //     aaa</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("``` ```\naaa", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><code></code>\naaa</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void FencedCodeBlocks_Spec108_CommonMark()
        {
            // The following Markdown:
            //     ~~~~~~
            //     aaa
            //     ~~~ ~~
            //
            // Should be rendered as:
            //     <pre><code>aaa
            //     ~~~ ~~
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("~~~~~~\naaa\n~~~ ~~", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>aaa\n~~~ ~~\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }

        // Fenced code blocks can interrupt paragraphs, and can be followed
        // directly by paragraphs, without a blank line between:        
        [Fact]
        public void FencedCodeBlocks_Spec109_CommonMark()
        {
            // The following Markdown:
            //     foo
            //     ```
            //     bar
            //     ```
            //     baz
            //
            // Should be rendered as:
            //     <p>foo</p>
            //     <pre><code>bar
            //     </code></pre>
            //     <p>baz</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo\n```\nbar\n```\nbaz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo</p>\n<pre><code>bar\n</code></pre>\n<p>baz</p>");

            Assert.Equal(expectedResult, result);
        }

        // Other blocks can also occur before and after fenced code blocks
        // without an intervening blank line:        
        [Fact]
        public void FencedCodeBlocks_Spec110_CommonMark()
        {
            // The following Markdown:
            //     foo
            //     ---
            //     ~~~
            //     bar
            //     ~~~
            //     # baz
            //
            // Should be rendered as:
            //     <h2>foo</h2>
            //     <pre><code>bar
            //     </code></pre>
            //     <h1>baz</h1>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo\n---\n~~~\nbar\n~~~\n# baz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h2>foo</h2>\n<pre><code>bar\n</code></pre>\n<h1>baz</h1>");

            Assert.Equal(expectedResult, result);
        }

        // An [info string] can be provided after the opening code fence.
        // Opening and closing spaces will be stripped, and the first word, prefixed
        // with `language-`, is used as the value for the `class` attribute of the
        // `code` element within the enclosing `pre` element.        
        [Fact]
        public void FencedCodeBlocks_Spec111_CommonMark()
        {
            // The following Markdown:
            //     ```ruby
            //     def foo(x)
            //       return 3
            //     end
            //     ```
            //
            // Should be rendered as:
            //     <pre><code class="language-ruby">def foo(x)
            //       return 3
            //     end
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("```ruby\ndef foo(x)\n  return 3\nend\n```", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code class=\"language-ruby\">def foo(x)\n  return 3\nend\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void FencedCodeBlocks_Spec112_CommonMark()
        {
            // The following Markdown:
            //     ~~~~    ruby startline=3 $%@#$
            //     def foo(x)
            //       return 3
            //     end
            //     ~~~~~~~
            //
            // Should be rendered as:
            //     <pre><code class="language-ruby">def foo(x)
            //       return 3
            //     end
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("~~~~    ruby startline=3 $%@#$\ndef foo(x)\n  return 3\nend\n~~~~~~~", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code class=\"language-ruby\">def foo(x)\n  return 3\nend\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void FencedCodeBlocks_Spec113_CommonMark()
        {
            // The following Markdown:
            //     ````;
            //     ````
            //
            // Should be rendered as:
            //     <pre><code class="language-;"></code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("````;\n````", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code class=\"language-;\"></code></pre>");

            Assert.Equal(expectedResult, result);
        }

        // [Info strings] for backtick code blocks cannot contain backticks:        
        [Fact]
        public void FencedCodeBlocks_Spec114_CommonMark()
        {
            // The following Markdown:
            //     ``` aa ```
            //     foo
            //
            // Should be rendered as:
            //     <p><code>aa</code>
            //     foo</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("``` aa ```\nfoo", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><code>aa</code>\nfoo</p>");

            Assert.Equal(expectedResult, result);
        }

        // Closing code fences cannot have [info strings]:        
        [Fact]
        public void FencedCodeBlocks_Spec115_CommonMark()
        {
            // The following Markdown:
            //     ```
            //     ``` aaa
            //     ```
            //
            // Should be rendered as:
            //     <pre><code>``` aaa
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("```\n``` aaa\n```", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>``` aaa\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }
    }

    // An [HTML block](@) is a group of lines that is treated
    // as raw HTML (and will not be escaped in HTML output).
    // 
    // There are seven kinds of [HTML block], which can be defined
    // by their start and end conditions.  The block begins with a line that
    // meets a [start condition](@) (after up to three spaces
    // optional indentation).  It ends with the first subsequent line that
    // meets a matching [end condition](@), or the last line of
    // the document or other [container block]), if no line is encountered that meets the
    // [end condition].  If the first line meets both the [start condition]
    // and the [end condition], the block will contain just that line.
    // 
    // 1.  **Start condition:**  line begins with the string `<script`,
    // `<pre`, or `<style` (case-insensitive), followed by whitespace,
    // the string `>`, or the end of the line.\
    // **End condition:**  line contains an end tag
    // `</script>`, `</pre>`, or `</style>` (case-insensitive; it
    // need not match the start tag).
    // 
    // 2.  **Start condition:** line begins with the string `<!--`.\
    // **End condition:**  line contains the string `-->`.
    // 
    // 3.  **Start condition:** line begins with the string `<?`.\
    // **End condition:** line contains the string `?>`.
    // 
    // 4.  **Start condition:** line begins with the string `<!`
    // followed by an uppercase ASCII letter.\
    // **End condition:** line contains the character `>`.
    // 
    // 5.  **Start condition:**  line begins with the string
    // `<![CDATA[`.\
    // **End condition:** line contains the string `]]>`.
    // 
    // 6.  **Start condition:** line begins the string `<` or `</`
    // followed by one of the strings (case-insensitive) `address`,
    // `article`, `aside`, `base`, `basefont`, `blockquote`, `body`,
    // `caption`, `center`, `col`, `colgroup`, `dd`, `details`, `dialog`,
    // `dir`, `div`, `dl`, `dt`, `fieldset`, `figcaption`, `figure`,
    // `footer`, `form`, `frame`, `frameset`,
    // `h1`, `h2`, `h3`, `h4`, `h5`, `h6`, `head`, `header`, `hr`,
    // `html`, `iframe`, `legend`, `li`, `link`, `main`, `menu`, `menuitem`,
    // `meta`, `nav`, `noframes`, `ol`, `optgroup`, `option`, `p`, `param`,
    // `section`, `source`, `summary`, `table`, `tbody`, `td`,
    // `tfoot`, `th`, `thead`, `title`, `tr`, `track`, `ul`, followed
    // by [whitespace], the end of the line, the string `>`, or
    // the string `/>`.\
    // **End condition:** line is followed by a [blank line].
    // 
    // 7.  **Start condition:**  line begins with a complete [open tag]
    // or [closing tag] (with any [tag name] other than `script`,
    // `style`, or `pre`) followed only by [whitespace]
    // or the end of the line.\
    // **End condition:** line is followed by a [blank line].
    // 
    // HTML blocks continue until they are closed by their appropriate
    // [end condition], or the last line of the document or other [container block].
    // This means any HTML **within an HTML block** that might otherwise be recognised
    // as a start condition will be ignored by the parser and passed through as-is,
    // without changing the parser's state  
    public class HTMLBlocksTests
    {

        // For instance, `<pre>` within a HTML block started by `<table>` will not affect
        // the parser state; as the HTML block was started in by start condition 6, it
        // will end at any blank line. This can be surprising:        
        [Fact]
        public void HTMLBlocks_Spec116_CommonMark()
        {
            // The following Markdown:
            //     <table><tr><td>
            //     <pre>
            //     **Hello**,
            //     
            //     _world_.
            //     </pre>
            //     </td></tr></table>
            //
            // Should be rendered as:
            //     <table><tr><td>
            //     <pre>
            //     **Hello**,
            //     <p><em>world</em>.
            //     </pre></p>
            //     </td></tr></table>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<table><tr><td>\n<pre>\n**Hello**,\n\n_world_.\n</pre>\n</td></tr></table>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<table><tr><td>\n<pre>\n**Hello**,\n<p><em>world</em>.\n</pre></p>\n</td></tr></table>");

            Assert.Equal(expectedResult, result);
        }

        // In this case, the HTML block is terminated by the newline — the `**hello**`
        // text remains verbatim — and regular parsing resumes, with a paragraph,
        // emphasised `world` and inline and block HTML following.
        // 
        // All types of [HTML blocks] except type 7 may interrupt
        // a paragraph.  Blocks of type 7 may not interrupt a paragraph.
        // (This restriction is intended to prevent unwanted interpretation
        // of long tags inside a wrapped paragraph as starting HTML blocks.)
        // 
        // Some simple examples follow.  Here are some basic HTML blocks
        // of type 6:        
        [Fact]
        public void HTMLBlocks_Spec117_CommonMark()
        {
            // The following Markdown:
            //     <table>
            //       <tr>
            //         <td>
            //                hi
            //         </td>
            //       </tr>
            //     </table>
            //     
            //     okay.
            //
            // Should be rendered as:
            //     <table>
            //       <tr>
            //         <td>
            //                hi
            //         </td>
            //       </tr>
            //     </table>
            //     <p>okay.</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<table>\n  <tr>\n    <td>\n           hi\n    </td>\n  </tr>\n</table>\n\nokay.", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<table>\n  <tr>\n    <td>\n           hi\n    </td>\n  </tr>\n</table>\n<p>okay.</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void HTMLBlocks_Spec118_CommonMark()
        {
            // The following Markdown:
            //      <div>
            //       *hello*
            //              <foo><a>
            //
            // Should be rendered as:
            //      <div>
            //       *hello*
            //              <foo><a>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml(" <div>\n  *hello*\n         <foo><a>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact(" <div>\n  *hello*\n         <foo><a>");

            Assert.Equal(expectedResult, result);
        }

        // A block can also start with a closing tag:        
        [Fact]
        public void HTMLBlocks_Spec119_CommonMark()
        {
            // The following Markdown:
            //     </div>
            //     *foo*
            //
            // Should be rendered as:
            //     </div>
            //     *foo*

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("</div>\n*foo*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("</div>\n*foo*");

            Assert.Equal(expectedResult, result);
        }

        // Here we have two HTML blocks with a Markdown paragraph between them:        
        [Fact]
        public void HTMLBlocks_Spec120_CommonMark()
        {
            // The following Markdown:
            //     <DIV CLASS="foo">
            //     
            //     *Markdown*
            //     
            //     </DIV>
            //
            // Should be rendered as:
            //     <DIV CLASS="foo">
            //     <p><em>Markdown</em></p>
            //     </DIV>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<DIV CLASS=\"foo\">\n\n*Markdown*\n\n</DIV>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<DIV CLASS=\"foo\">\n<p><em>Markdown</em></p>\n</DIV>");

            Assert.Equal(expectedResult, result);
        }

        // The tag on the first line can be partial, as long
        // as it is split where there would be whitespace:        
        [Fact]
        public void HTMLBlocks_Spec121_CommonMark()
        {
            // The following Markdown:
            //     <div id="foo"
            //       class="bar">
            //     </div>
            //
            // Should be rendered as:
            //     <div id="foo"
            //       class="bar">
            //     </div>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<div id=\"foo\"\n  class=\"bar\">\n</div>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<div id=\"foo\"\n  class=\"bar\">\n</div>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void HTMLBlocks_Spec122_CommonMark()
        {
            // The following Markdown:
            //     <div id="foo" class="bar
            //       baz">
            //     </div>
            //
            // Should be rendered as:
            //     <div id="foo" class="bar
            //       baz">
            //     </div>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<div id=\"foo\" class=\"bar\n  baz\">\n</div>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<div id=\"foo\" class=\"bar\n  baz\">\n</div>");

            Assert.Equal(expectedResult, result);
        }

        // An open tag need not be closed:        
        [Fact]
        public void HTMLBlocks_Spec123_CommonMark()
        {
            // The following Markdown:
            //     <div>
            //     *foo*
            //     
            //     *bar*
            //
            // Should be rendered as:
            //     <div>
            //     *foo*
            //     <p><em>bar</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<div>\n*foo*\n\n*bar*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<div>\n*foo*\n<p><em>bar</em></p>");

            Assert.Equal(expectedResult, result);
        }

        // A partial tag need not even be completed (garbage
        // in, garbage out):        
        [Fact]
        public void HTMLBlocks_Spec124_CommonMark()
        {
            // The following Markdown:
            //     <div id="foo"
            //     *hi*
            //
            // Should be rendered as:
            //     <div id="foo"
            //     *hi*

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<div id=\"foo\"\n*hi*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<div id=\"foo\"\n*hi*");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void HTMLBlocks_Spec125_CommonMark()
        {
            // The following Markdown:
            //     <div class
            //     foo
            //
            // Should be rendered as:
            //     <div class
            //     foo

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<div class\nfoo", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<div class\nfoo");

            Assert.Equal(expectedResult, result);
        }

        // The initial tag doesn't even need to be a valid
        // tag, as long as it starts like one:        
        [Fact]
        public void HTMLBlocks_Spec126_CommonMark()
        {
            // The following Markdown:
            //     <div *???-&&&-<---
            //     *foo*
            //
            // Should be rendered as:
            //     <div *???-&&&-<---
            //     *foo*

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<div *???-&&&-<---\n*foo*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<div *???-&&&-<---\n*foo*");

            Assert.Equal(expectedResult, result);
        }

        // In type 6 blocks, the initial tag need not be on a line by
        // itself:        
        [Fact]
        public void HTMLBlocks_Spec127_CommonMark()
        {
            // The following Markdown:
            //     <div><a href="bar">*foo*</a></div>
            //
            // Should be rendered as:
            //     <div><a href="bar">*foo*</a></div>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<div><a href=\"bar\">*foo*</a></div>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<div><a href=\"bar\">*foo*</a></div>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void HTMLBlocks_Spec128_CommonMark()
        {
            // The following Markdown:
            //     <table><tr><td>
            //     foo
            //     </td></tr></table>
            //
            // Should be rendered as:
            //     <table><tr><td>
            //     foo
            //     </td></tr></table>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<table><tr><td>\nfoo\n</td></tr></table>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<table><tr><td>\nfoo\n</td></tr></table>");

            Assert.Equal(expectedResult, result);
        }

        // Everything until the next blank line or end of document
        // gets included in the HTML block.  So, in the following
        // example, what looks like a Markdown code block
        // is actually part of the HTML block, which continues until a blank
        // line or the end of the document is reached:        
        [Fact]
        public void HTMLBlocks_Spec129_CommonMark()
        {
            // The following Markdown:
            //     <div></div>
            //     ``` c
            //     int x = 33;
            //     ```
            //
            // Should be rendered as:
            //     <div></div>
            //     ``` c
            //     int x = 33;
            //     ```

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<div></div>\n``` c\nint x = 33;\n```", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<div></div>\n``` c\nint x = 33;\n```");

            Assert.Equal(expectedResult, result);
        }

        // To start an [HTML block] with a tag that is *not* in the
        // list of block-level tags in (6), you must put the tag by
        // itself on the first line (and it must be complete):        
        [Fact]
        public void HTMLBlocks_Spec130_CommonMark()
        {
            // The following Markdown:
            //     <a href="foo">
            //     *bar*
            //     </a>
            //
            // Should be rendered as:
            //     <a href="foo">
            //     *bar*
            //     </a>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<a href=\"foo\">\n*bar*\n</a>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<a href=\"foo\">\n*bar*\n</a>");

            Assert.Equal(expectedResult, result);
        }

        // In type 7 blocks, the [tag name] can be anything:        
        [Fact]
        public void HTMLBlocks_Spec131_CommonMark()
        {
            // The following Markdown:
            //     <Warning>
            //     *bar*
            //     </Warning>
            //
            // Should be rendered as:
            //     <Warning>
            //     *bar*
            //     </Warning>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<Warning>\n*bar*\n</Warning>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<Warning>\n*bar*\n</Warning>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void HTMLBlocks_Spec132_CommonMark()
        {
            // The following Markdown:
            //     <i class="foo">
            //     *bar*
            //     </i>
            //
            // Should be rendered as:
            //     <i class="foo">
            //     *bar*
            //     </i>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<i class=\"foo\">\n*bar*\n</i>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<i class=\"foo\">\n*bar*\n</i>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void HTMLBlocks_Spec133_CommonMark()
        {
            // The following Markdown:
            //     </ins>
            //     *bar*
            //
            // Should be rendered as:
            //     </ins>
            //     *bar*

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("</ins>\n*bar*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("</ins>\n*bar*");

            Assert.Equal(expectedResult, result);
        }

        // These rules are designed to allow us to work with tags that
        // can function as either block-level or inline-level tags.
        // The `<del>` tag is a nice example.  We can surround content with
        // `<del>` tags in three different ways.  In this case, we get a raw
        // HTML block, because the `<del>` tag is on a line by itself:        
        [Fact]
        public void HTMLBlocks_Spec134_CommonMark()
        {
            // The following Markdown:
            //     <del>
            //     *foo*
            //     </del>
            //
            // Should be rendered as:
            //     <del>
            //     *foo*
            //     </del>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<del>\n*foo*\n</del>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<del>\n*foo*\n</del>");

            Assert.Equal(expectedResult, result);
        }

        // In this case, we get a raw HTML block that just includes
        // the `<del>` tag (because it ends with the following blank
        // line).  So the contents get interpreted as CommonMark:        
        [Fact]
        public void HTMLBlocks_Spec135_CommonMark()
        {
            // The following Markdown:
            //     <del>
            //     
            //     *foo*
            //     
            //     </del>
            //
            // Should be rendered as:
            //     <del>
            //     <p><em>foo</em></p>
            //     </del>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<del>\n\n*foo*\n\n</del>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<del>\n<p><em>foo</em></p>\n</del>");

            Assert.Equal(expectedResult, result);
        }

        // Finally, in this case, the `<del>` tags are interpreted
        // as [raw HTML] *inside* the CommonMark paragraph.  (Because
        // the tag is not on a line by itself, we get inline HTML
        // rather than an [HTML block].)        
        [Fact]
        public void HTMLBlocks_Spec136_CommonMark()
        {
            // The following Markdown:
            //     <del>*foo*</del>
            //
            // Should be rendered as:
            //     <p><del><em>foo</em></del></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<del>*foo*</del>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><del><em>foo</em></del></p>");

            Assert.Equal(expectedResult, result);
        }

        // HTML tags designed to contain literal content
        // (`script`, `style`, `pre`), comments, processing instructions,
        // and declarations are treated somewhat differently.
        // Instead of ending at the first blank line, these blocks
        // end at the first line containing a corresponding end tag.
        // As a result, these blocks can contain blank lines:
        // 
        // A pre tag (type 1):        
        [Fact]
        public void HTMLBlocks_Spec137_CommonMark()
        {
            // The following Markdown:
            //     <pre language="haskell"><code>
            //     import Text.HTML.TagSoup
            //     
            //     main :: IO ()
            //     main = print $ parseTags tags
            //     </code></pre>
            //     okay
            //
            // Should be rendered as:
            //     <pre language="haskell"><code>
            //     import Text.HTML.TagSoup
            //     
            //     main :: IO ()
            //     main = print $ parseTags tags
            //     </code></pre>
            //     <p>okay</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<pre language=\"haskell\"><code>\nimport Text.HTML.TagSoup\n\nmain :: IO ()\nmain = print $ parseTags tags\n</code></pre>\nokay", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre language=\"haskell\"><code>\nimport Text.HTML.TagSoup\n\nmain :: IO ()\nmain = print $ parseTags tags\n</code></pre>\n<p>okay</p>");

            Assert.Equal(expectedResult, result);
        }

        // A script tag (type 1):        
        [Fact]
        public void HTMLBlocks_Spec138_CommonMark()
        {
            // The following Markdown:
            //     <script type="text/javascript">
            //     // JavaScript example
            //     
            //     document.getElementById("demo").innerHTML = "Hello JavaScript!";
            //     </script>
            //     okay
            //
            // Should be rendered as:
            //     <script type="text/javascript">
            //     // JavaScript example
            //     
            //     document.getElementById("demo").innerHTML = "Hello JavaScript!";
            //     </script>
            //     <p>okay</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<script type=\"text/javascript\">\n// JavaScript example\n\ndocument.getElementById(\"demo\").innerHTML = \"Hello JavaScript!\";\n</script>\nokay", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<script type=\"text/javascript\">\n// JavaScript example\n\ndocument.getElementById(\"demo\").innerHTML = \"Hello JavaScript!\";\n</script>\n<p>okay</p>");

            Assert.Equal(expectedResult, result);
        }

        // A style tag (type 1):        
        [Fact]
        public void HTMLBlocks_Spec139_CommonMark()
        {
            // The following Markdown:
            //     <style
            //       type="text/css">
            //     h1 {color:red;}
            //     
            //     p {color:blue;}
            //     </style>
            //     okay
            //
            // Should be rendered as:
            //     <style
            //       type="text/css">
            //     h1 {color:red;}
            //     
            //     p {color:blue;}
            //     </style>
            //     <p>okay</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<style\n  type=\"text/css\">\nh1 {color:red;}\n\np {color:blue;}\n</style>\nokay", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<style\n  type=\"text/css\">\nh1 {color:red;}\n\np {color:blue;}\n</style>\n<p>okay</p>");

            Assert.Equal(expectedResult, result);
        }

        // If there is no matching end tag, the block will end at the
        // end of the document (or the enclosing [block quote][block quotes]
        // or [list item][list items]):        
        [Fact]
        public void HTMLBlocks_Spec140_CommonMark()
        {
            // The following Markdown:
            //     <style
            //       type="text/css">
            //     
            //     foo
            //
            // Should be rendered as:
            //     <style
            //       type="text/css">
            //     
            //     foo

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<style\n  type=\"text/css\">\n\nfoo", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<style\n  type=\"text/css\">\n\nfoo");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void HTMLBlocks_Spec141_CommonMark()
        {
            // The following Markdown:
            //     > <div>
            //     > foo
            //     
            //     bar
            //
            // Should be rendered as:
            //     <blockquote>
            //     <div>
            //     foo
            //     </blockquote>
            //     <p>bar</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("> <div>\n> foo\n\nbar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<div>\nfoo\n</blockquote>\n<p>bar</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void HTMLBlocks_Spec142_CommonMark()
        {
            // The following Markdown:
            //     - <div>
            //     - foo
            //
            // Should be rendered as:
            //     <ul>
            //     <li>
            //     <div>
            //     </li>
            //     <li>foo</li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- <div>\n- foo", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>\n<div>\n</li>\n<li>foo</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }

        // The end tag can occur on the same line as the start tag:        
        [Fact]
        public void HTMLBlocks_Spec143_CommonMark()
        {
            // The following Markdown:
            //     <style>p{color:red;}</style>
            //     *foo*
            //
            // Should be rendered as:
            //     <style>p{color:red;}</style>
            //     <p><em>foo</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<style>p{color:red;}</style>\n*foo*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<style>p{color:red;}</style>\n<p><em>foo</em></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void HTMLBlocks_Spec144_CommonMark()
        {
            // The following Markdown:
            //     <!-- foo -->*bar*
            //     *baz*
            //
            // Should be rendered as:
            //     <!-- foo -->*bar*
            //     <p><em>baz</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<!-- foo -->*bar*\n*baz*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<!-- foo -->*bar*\n<p><em>baz</em></p>");

            Assert.Equal(expectedResult, result);
        }

        // Note that anything on the last line after the
        // end tag will be included in the [HTML block]:        
        [Fact]
        public void HTMLBlocks_Spec145_CommonMark()
        {
            // The following Markdown:
            //     <script>
            //     foo
            //     </script>1. *bar*
            //
            // Should be rendered as:
            //     <script>
            //     foo
            //     </script>1. *bar*

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<script>\nfoo\n</script>1. *bar*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<script>\nfoo\n</script>1. *bar*");

            Assert.Equal(expectedResult, result);
        }

        // A comment (type 2):        
        [Fact]
        public void HTMLBlocks_Spec146_CommonMark()
        {
            // The following Markdown:
            //     <!-- Foo
            //     
            //     bar
            //        baz -->
            //     okay
            //
            // Should be rendered as:
            //     <!-- Foo
            //     
            //     bar
            //        baz -->
            //     <p>okay</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<!-- Foo\n\nbar\n   baz -->\nokay", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<!-- Foo\n\nbar\n   baz -->\n<p>okay</p>");

            Assert.Equal(expectedResult, result);
        }

        // A processing instruction (type 3):        
        [Fact]
        public void HTMLBlocks_Spec147_CommonMark()
        {
            // The following Markdown:
            //     <?php
            //     
            //       echo '>';
            //     
            //     ?>
            //     okay
            //
            // Should be rendered as:
            //     <?php
            //     
            //       echo '>';
            //     
            //     ?>
            //     <p>okay</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<?php\n\n  echo '>';\n\n?>\nokay", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<?php\n\n  echo '>';\n\n?>\n<p>okay</p>");

            Assert.Equal(expectedResult, result);
        }

        // A declaration (type 4):        
        [Fact]
        public void HTMLBlocks_Spec148_CommonMark()
        {
            // The following Markdown:
            //     <!DOCTYPE html>
            //
            // Should be rendered as:
            //     <!DOCTYPE html>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<!DOCTYPE html>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<!DOCTYPE html>");

            Assert.Equal(expectedResult, result);
        }

        // CDATA (type 5):        
        [Fact]
        public void HTMLBlocks_Spec149_CommonMark()
        {
            // The following Markdown:
            //     <![CDATA[
            //     function matchwo(a,b)
            //     {
            //       if (a < b && a < 0) then {
            //         return 1;
            //     
            //       } else {
            //     
            //         return 0;
            //       }
            //     }
            //     ]]>
            //     okay
            //
            // Should be rendered as:
            //     <![CDATA[
            //     function matchwo(a,b)
            //     {
            //       if (a < b && a < 0) then {
            //         return 1;
            //     
            //       } else {
            //     
            //         return 0;
            //       }
            //     }
            //     ]]>
            //     <p>okay</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<![CDATA[\nfunction matchwo(a,b)\n{\n  if (a < b && a < 0) then {\n    return 1;\n\n  } else {\n\n    return 0;\n  }\n}\n]]>\nokay", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<![CDATA[\nfunction matchwo(a,b)\n{\n  if (a < b && a < 0) then {\n    return 1;\n\n  } else {\n\n    return 0;\n  }\n}\n]]>\n<p>okay</p>");

            Assert.Equal(expectedResult, result);
        }

        // The opening tag can be indented 1-3 spaces, but not 4:        
        [Fact]
        public void HTMLBlocks_Spec150_CommonMark()
        {
            // The following Markdown:
            //       <!-- foo -->
            //     
            //         <!-- foo -->
            //
            // Should be rendered as:
            //       <!-- foo -->
            //     <pre><code>&lt;!-- foo --&gt;
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("  <!-- foo -->\n\n    <!-- foo -->", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("  <!-- foo -->\n<pre><code>&lt;!-- foo --&gt;\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void HTMLBlocks_Spec151_CommonMark()
        {
            // The following Markdown:
            //       <div>
            //     
            //         <div>
            //
            // Should be rendered as:
            //       <div>
            //     <pre><code>&lt;div&gt;
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("  <div>\n\n    <div>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("  <div>\n<pre><code>&lt;div&gt;\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }

        // An HTML block of types 1--6 can interrupt a paragraph, and need not be
        // preceded by a blank line.        
        [Fact]
        public void HTMLBlocks_Spec152_CommonMark()
        {
            // The following Markdown:
            //     Foo
            //     <div>
            //     bar
            //     </div>
            //
            // Should be rendered as:
            //     <p>Foo</p>
            //     <div>
            //     bar
            //     </div>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("Foo\n<div>\nbar\n</div>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>Foo</p>\n<div>\nbar\n</div>");

            Assert.Equal(expectedResult, result);
        }

        // However, a following blank line is needed, except at the end of
        // a document, and except for blocks of types 1--5, above:        
        [Fact]
        public void HTMLBlocks_Spec153_CommonMark()
        {
            // The following Markdown:
            //     <div>
            //     bar
            //     </div>
            //     *foo*
            //
            // Should be rendered as:
            //     <div>
            //     bar
            //     </div>
            //     *foo*

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<div>\nbar\n</div>\n*foo*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<div>\nbar\n</div>\n*foo*");

            Assert.Equal(expectedResult, result);
        }

        // HTML blocks of type 7 cannot interrupt a paragraph:        
        [Fact]
        public void HTMLBlocks_Spec154_CommonMark()
        {
            // The following Markdown:
            //     Foo
            //     <a href="bar">
            //     baz
            //
            // Should be rendered as:
            //     <p>Foo
            //     <a href="bar">
            //     baz</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("Foo\n<a href=\"bar\">\nbaz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>Foo\n<a href=\"bar\">\nbaz</p>");

            Assert.Equal(expectedResult, result);
        }

        // This rule differs from John Gruber's original Markdown syntax
        // specification, which says:
        // 
        // > The only restrictions are that block-level HTML elements —
        // > e.g. `<div>`, `<table>`, `<pre>`, `<p>`, etc. — must be separated from
        // > surrounding content by blank lines, and the start and end tags of the
        // > block should not be indented with tabs or spaces.
        // 
        // In some ways Gruber's rule is more restrictive than the one given
        // here:
        // 
        // - It requires that an HTML block be preceded by a blank line.
        // - It does not allow the start tag to be indented.
        // - It requires a matching end tag, which it also does not allow to
        //   be indented.
        // 
        // Most Markdown implementations (including some of Gruber's own) do not
        // respect all of these restrictions.
        // 
        // There is one respect, however, in which Gruber's rule is more liberal
        // than the one given here, since it allows blank lines to occur inside
        // an HTML block.  There are two reasons for disallowing them here.
        // First, it removes the need to parse balanced tags, which is
        // expensive and can require backtracking from the end of the document
        // if no matching end tag is found. Second, it provides a very simple
        // and flexible way of including Markdown content inside HTML tags:
        // simply separate the Markdown from the HTML using blank lines:
        // 
        // Compare:        
        [Fact]
        public void HTMLBlocks_Spec155_CommonMark()
        {
            // The following Markdown:
            //     <div>
            //     
            //     *Emphasized* text.
            //     
            //     </div>
            //
            // Should be rendered as:
            //     <div>
            //     <p><em>Emphasized</em> text.</p>
            //     </div>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<div>\n\n*Emphasized* text.\n\n</div>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<div>\n<p><em>Emphasized</em> text.</p>\n</div>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void HTMLBlocks_Spec156_CommonMark()
        {
            // The following Markdown:
            //     <div>
            //     *Emphasized* text.
            //     </div>
            //
            // Should be rendered as:
            //     <div>
            //     *Emphasized* text.
            //     </div>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<div>\n*Emphasized* text.\n</div>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<div>\n*Emphasized* text.\n</div>");

            Assert.Equal(expectedResult, result);
        }

        // Some Markdown implementations have adopted a convention of
        // interpreting content inside tags as text if the open tag has
        // the attribute `markdown=1`.  The rule given above seems a simpler and
        // more elegant way of achieving the same expressive power, which is also
        // much simpler to parse.
        // 
        // The main potential drawback is that one can no longer paste HTML
        // blocks into Markdown documents with 100% reliability.  However,
        // *in most cases* this will work fine, because the blank lines in
        // HTML are usually followed by HTML block tags.  For example:        
        [Fact]
        public void HTMLBlocks_Spec157_CommonMark()
        {
            // The following Markdown:
            //     <table>
            //     
            //     <tr>
            //     
            //     <td>
            //     Hi
            //     </td>
            //     
            //     </tr>
            //     
            //     </table>
            //
            // Should be rendered as:
            //     <table>
            //     <tr>
            //     <td>
            //     Hi
            //     </td>
            //     </tr>
            //     </table>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<table>\n\n<tr>\n\n<td>\nHi\n</td>\n\n</tr>\n\n</table>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<table>\n<tr>\n<td>\nHi\n</td>\n</tr>\n</table>");

            Assert.Equal(expectedResult, result);
        }

        // There are problems, however, if the inner tags are indented
        // *and* separated by spaces, as then they will be interpreted as
        // an indented code block:        
        [Fact]
        public void HTMLBlocks_Spec158_CommonMark()
        {
            // The following Markdown:
            //     <table>
            //     
            //       <tr>
            //     
            //         <td>
            //           Hi
            //         </td>
            //     
            //       </tr>
            //     
            //     </table>
            //
            // Should be rendered as:
            //     <table>
            //       <tr>
            //     <pre><code>&lt;td&gt;
            //       Hi
            //     &lt;/td&gt;
            //     </code></pre>
            //       </tr>
            //     </table>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<table>\n\n  <tr>\n\n    <td>\n      Hi\n    </td>\n\n  </tr>\n\n</table>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<table>\n  <tr>\n<pre><code>&lt;td&gt;\n  Hi\n&lt;/td&gt;\n</code></pre>\n  </tr>\n</table>");

            Assert.Equal(expectedResult, result);
        }
    }

    // A [link reference definition](@)
    // consists of a [link label], indented up to three spaces, followed
    // by a colon (`:`), optional [whitespace] (including up to one
    // [line ending]), a [link destination],
    // optional [whitespace] (including up to one
    // [line ending]), and an optional [link
    // title], which if it is present must be separated
    // from the [link destination] by [whitespace].
    // No further [non-whitespace characters] may occur on the line  
    public class LinkReferenceDefinitionsTests
    {

        // A [link reference definition]
        // does not correspond to a structural element of a document.  Instead, it
        // defines a label which can be used in [reference links]
        // and reference-style [images] elsewhere in the document.  [Link
        // reference definitions] can come either before or after the links that use
        // them.        
        [Fact]
        public void LinkReferenceDefinitions_Spec159_CommonMark()
        {
            // The following Markdown:
            //     [foo]: /url "title"
            //     
            //     [foo]
            //
            // Should be rendered as:
            //     <p><a href="/url" title="title">foo</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo]: /url \"title\"\n\n[foo]", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/url\" title=\"title\">foo</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void LinkReferenceDefinitions_Spec160_CommonMark()
        {
            // The following Markdown:
            //        [foo]: 
            //           /url  
            //                'the title'  
            //     
            //     [foo]
            //
            // Should be rendered as:
            //     <p><a href="/url" title="the title">foo</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("   [foo]: \n      /url  \n           'the title'  \n\n[foo]", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/url\" title=\"the title\">foo</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void LinkReferenceDefinitions_Spec161_CommonMark()
        {
            // The following Markdown:
            //     [Foo*bar\]]:my_(url) 'title (with parens)'
            //     
            //     [Foo*bar\]]
            //
            // Should be rendered as:
            //     <p><a href="my_(url)" title="title (with parens)">Foo*bar]</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[Foo*bar\\]]:my_(url) 'title (with parens)'\n\n[Foo*bar\\]]", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"my_(url)\" title=\"title (with parens)\">Foo*bar]</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void LinkReferenceDefinitions_Spec162_CommonMark()
        {
            // The following Markdown:
            //     [Foo bar]:
            //     <my%20url>
            //     'title'
            //     
            //     [Foo bar]
            //
            // Should be rendered as:
            //     <p><a href="my%20url" title="title">Foo bar</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[Foo bar]:\n<my%20url>\n'title'\n\n[Foo bar]", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"my%20url\" title=\"title\">Foo bar</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // The title may extend over multiple lines:        
        [Fact]
        public void LinkReferenceDefinitions_Spec163_CommonMark()
        {
            // The following Markdown:
            //     [foo]: /url '
            //     title
            //     line1
            //     line2
            //     '
            //     
            //     [foo]
            //
            // Should be rendered as:
            //     <p><a href="/url" title="
            //     title
            //     line1
            //     line2
            //     ">foo</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo]: /url '\ntitle\nline1\nline2\n'\n\n[foo]", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/url\" title=\"\ntitle\nline1\nline2\n\">foo</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // However, it may not contain a [blank line]:        
        [Fact]
        public void LinkReferenceDefinitions_Spec164_CommonMark()
        {
            // The following Markdown:
            //     [foo]: /url 'title
            //     
            //     with blank line'
            //     
            //     [foo]
            //
            // Should be rendered as:
            //     <p>[foo]: /url 'title</p>
            //     <p>with blank line'</p>
            //     <p>[foo]</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo]: /url 'title\n\nwith blank line'\n\n[foo]", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[foo]: /url 'title</p>\n<p>with blank line'</p>\n<p>[foo]</p>");

            Assert.Equal(expectedResult, result);
        }

        // The title may be omitted:        
        [Fact]
        public void LinkReferenceDefinitions_Spec165_CommonMark()
        {
            // The following Markdown:
            //     [foo]:
            //     /url
            //     
            //     [foo]
            //
            // Should be rendered as:
            //     <p><a href="/url">foo</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo]:\n/url\n\n[foo]", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/url\">foo</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // The link destination may not be omitted:        
        [Fact]
        public void LinkReferenceDefinitions_Spec166_CommonMark()
        {
            // The following Markdown:
            //     [foo]:
            //     
            //     [foo]
            //
            // Should be rendered as:
            //     <p>[foo]:</p>
            //     <p>[foo]</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo]:\n\n[foo]", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[foo]:</p>\n<p>[foo]</p>");

            Assert.Equal(expectedResult, result);
        }

        // Both title and destination can contain backslash escapes
        // and literal backslashes:        
        [Fact]
        public void LinkReferenceDefinitions_Spec167_CommonMark()
        {
            // The following Markdown:
            //     [foo]: /url\bar\*baz "foo\"bar\baz"
            //     
            //     [foo]
            //
            // Should be rendered as:
            //     <p><a href="/url%5Cbar*baz" title="foo&quot;bar\baz">foo</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo]: /url\\bar\\*baz \"foo\\\"bar\\baz\"\n\n[foo]", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/url%5Cbar*baz\" title=\"foo&quot;bar\\baz\">foo</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // A link can come before its corresponding definition:        
        [Fact]
        public void LinkReferenceDefinitions_Spec168_CommonMark()
        {
            // The following Markdown:
            //     [foo]
            //     
            //     [foo]: url
            //
            // Should be rendered as:
            //     <p><a href="url">foo</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo]\n\n[foo]: url", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"url\">foo</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // If there are several matching definitions, the first one takes
        // precedence:        
        [Fact]
        public void LinkReferenceDefinitions_Spec169_CommonMark()
        {
            // The following Markdown:
            //     [foo]
            //     
            //     [foo]: first
            //     [foo]: second
            //
            // Should be rendered as:
            //     <p><a href="first">foo</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo]\n\n[foo]: first\n[foo]: second", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"first\">foo</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // As noted in the section on [Links], matching of labels is
        // case-insensitive (see [matches]).        
        [Fact]
        public void LinkReferenceDefinitions_Spec170_CommonMark()
        {
            // The following Markdown:
            //     [FOO]: /url
            //     
            //     [Foo]
            //
            // Should be rendered as:
            //     <p><a href="/url">Foo</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[FOO]: /url\n\n[Foo]", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/url\">Foo</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void LinkReferenceDefinitions_Spec171_CommonMark()
        {
            // The following Markdown:
            //     [ΑΓΩ]: /φου
            //     
            //     [αγω]
            //
            // Should be rendered as:
            //     <p><a href="/%CF%86%CE%BF%CF%85">αγω</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[ΑΓΩ]: /φου\n\n[αγω]", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/%CF%86%CE%BF%CF%85\">αγω</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // Here is a link reference definition with no corresponding link.
        // It contributes nothing to the document.        
        [Fact]
        public void LinkReferenceDefinitions_Spec172_CommonMark()
        {
            // The following Markdown:
            //     [foo]: /url
            //
            // Should be rendered as:


            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo]: /url", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("");

            Assert.Equal(expectedResult, result);
        }

        // Here is another one:        
        [Fact]
        public void LinkReferenceDefinitions_Spec173_CommonMark()
        {
            // The following Markdown:
            //     [
            //     foo
            //     ]: /url
            //     bar
            //
            // Should be rendered as:
            //     <p>bar</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[\nfoo\n]: /url\nbar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>bar</p>");

            Assert.Equal(expectedResult, result);
        }

        // This is not a link reference definition, because there are
        // [non-whitespace characters] after the title:        
        [Fact]
        public void LinkReferenceDefinitions_Spec174_CommonMark()
        {
            // The following Markdown:
            //     [foo]: /url "title" ok
            //
            // Should be rendered as:
            //     <p>[foo]: /url &quot;title&quot; ok</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo]: /url \"title\" ok", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[foo]: /url &quot;title&quot; ok</p>");

            Assert.Equal(expectedResult, result);
        }

        // This is a link reference definition, but it has no title:        
        [Fact]
        public void LinkReferenceDefinitions_Spec175_CommonMark()
        {
            // The following Markdown:
            //     [foo]: /url
            //     "title" ok
            //
            // Should be rendered as:
            //     <p>&quot;title&quot; ok</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo]: /url\n\"title\" ok", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>&quot;title&quot; ok</p>");

            Assert.Equal(expectedResult, result);
        }

        // This is not a link reference definition, because it is indented
        // four spaces:        
        [Fact]
        public void LinkReferenceDefinitions_Spec176_CommonMark()
        {
            // The following Markdown:
            //         [foo]: /url "title"
            //     
            //     [foo]
            //
            // Should be rendered as:
            //     <pre><code>[foo]: /url &quot;title&quot;
            //     </code></pre>
            //     <p>[foo]</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("    [foo]: /url \"title\"\n\n[foo]", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>[foo]: /url &quot;title&quot;\n</code></pre>\n<p>[foo]</p>");

            Assert.Equal(expectedResult, result);
        }

        // This is not a link reference definition, because it occurs inside
        // a code block:        
        [Fact]
        public void LinkReferenceDefinitions_Spec177_CommonMark()
        {
            // The following Markdown:
            //     ```
            //     [foo]: /url
            //     ```
            //     
            //     [foo]
            //
            // Should be rendered as:
            //     <pre><code>[foo]: /url
            //     </code></pre>
            //     <p>[foo]</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("```\n[foo]: /url\n```\n\n[foo]", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>[foo]: /url\n</code></pre>\n<p>[foo]</p>");

            Assert.Equal(expectedResult, result);
        }

        // A [link reference definition] cannot interrupt a paragraph.        
        [Fact]
        public void LinkReferenceDefinitions_Spec178_CommonMark()
        {
            // The following Markdown:
            //     Foo
            //     [bar]: /baz
            //     
            //     [bar]
            //
            // Should be rendered as:
            //     <p>Foo
            //     [bar]: /baz</p>
            //     <p>[bar]</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("Foo\n[bar]: /baz\n\n[bar]", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>Foo\n[bar]: /baz</p>\n<p>[bar]</p>");

            Assert.Equal(expectedResult, result);
        }

        // However, it can directly follow other block elements, such as headings
        // and thematic breaks, and it need not be followed by a blank line.        
        [Fact]
        public void LinkReferenceDefinitions_Spec179_CommonMark()
        {
            // The following Markdown:
            //     # [Foo]
            //     [foo]: /url
            //     > bar
            //
            // Should be rendered as:
            //     <h1><a href="/url">Foo</a></h1>
            //     <blockquote>
            //     <p>bar</p>
            //     </blockquote>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("# [Foo]\n[foo]: /url\n> bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h1><a href=\"/url\">Foo</a></h1>\n<blockquote>\n<p>bar</p>\n</blockquote>");

            Assert.Equal(expectedResult, result);
        }

        // Several [link reference definitions]
        // can occur one after another, without intervening blank lines.        
        [Fact]
        public void LinkReferenceDefinitions_Spec180_CommonMark()
        {
            // The following Markdown:
            //     [foo]: /foo-url "foo"
            //     [bar]: /bar-url
            //       "bar"
            //     [baz]: /baz-url
            //     
            //     [foo],
            //     [bar],
            //     [baz]
            //
            // Should be rendered as:
            //     <p><a href="/foo-url" title="foo">foo</a>,
            //     <a href="/bar-url" title="bar">bar</a>,
            //     <a href="/baz-url">baz</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo]: /foo-url \"foo\"\n[bar]: /bar-url\n  \"bar\"\n[baz]: /baz-url\n\n[foo],\n[bar],\n[baz]", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/foo-url\" title=\"foo\">foo</a>,\n<a href=\"/bar-url\" title=\"bar\">bar</a>,\n<a href=\"/baz-url\">baz</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // [Link reference definitions] can occur
        // inside block containers, like lists and block quotations.  They
        // affect the entire document, not just the container in which they
        // are defined:        
        [Fact]
        public void LinkReferenceDefinitions_Spec181_CommonMark()
        {
            // The following Markdown:
            //     [foo]
            //     
            //     > [foo]: /url
            //
            // Should be rendered as:
            //     <p><a href="/url">foo</a></p>
            //     <blockquote>
            //     </blockquote>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo]\n\n> [foo]: /url", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/url\">foo</a></p>\n<blockquote>\n</blockquote>");

            Assert.Equal(expectedResult, result);
        }
    }

    // A sequence of non-blank lines that cannot be interpreted as other
    // kinds of blocks forms a [paragraph](@).
    // The contents of the paragraph are the result of parsing the
    // paragraph's raw content as inlines.  The paragraph's raw content
    // is formed by concatenating the lines and removing initial and final
    // [whitespace]  
    public class ParagraphsTests
    {

        // A simple example with two paragraphs:        
        [Fact]
        public void Paragraphs_Spec182_CommonMark()
        {
            // The following Markdown:
            //     aaa
            //     
            //     bbb
            //
            // Should be rendered as:
            //     <p>aaa</p>
            //     <p>bbb</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("aaa\n\nbbb", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>aaa</p>\n<p>bbb</p>");

            Assert.Equal(expectedResult, result);
        }

        // Paragraphs can contain multiple lines, but no blank lines:        
        [Fact]
        public void Paragraphs_Spec183_CommonMark()
        {
            // The following Markdown:
            //     aaa
            //     bbb
            //     
            //     ccc
            //     ddd
            //
            // Should be rendered as:
            //     <p>aaa
            //     bbb</p>
            //     <p>ccc
            //     ddd</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("aaa\nbbb\n\nccc\nddd", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>aaa\nbbb</p>\n<p>ccc\nddd</p>");

            Assert.Equal(expectedResult, result);
        }

        // Multiple blank lines between paragraph have no effect:        
        [Fact]
        public void Paragraphs_Spec184_CommonMark()
        {
            // The following Markdown:
            //     aaa
            //     
            //     
            //     bbb
            //
            // Should be rendered as:
            //     <p>aaa</p>
            //     <p>bbb</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("aaa\n\n\nbbb", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>aaa</p>\n<p>bbb</p>");

            Assert.Equal(expectedResult, result);
        }

        // Leading spaces are skipped:        
        [Fact]
        public void Paragraphs_Spec185_CommonMark()
        {
            // The following Markdown:
            //       aaa
            //      bbb
            //
            // Should be rendered as:
            //     <p>aaa
            //     bbb</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("  aaa\n bbb", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>aaa\nbbb</p>");

            Assert.Equal(expectedResult, result);
        }

        // Lines after the first may be indented any amount, since indented
        // code blocks cannot interrupt paragraphs.        
        [Fact]
        public void Paragraphs_Spec186_CommonMark()
        {
            // The following Markdown:
            //     aaa
            //                  bbb
            //                                            ccc
            //
            // Should be rendered as:
            //     <p>aaa
            //     bbb
            //     ccc</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("aaa\n             bbb\n                                       ccc", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>aaa\nbbb\nccc</p>");

            Assert.Equal(expectedResult, result);
        }

        // However, the first line may be indented at most three spaces,
        // or an indented code block will be triggered:        
        [Fact]
        public void Paragraphs_Spec187_CommonMark()
        {
            // The following Markdown:
            //        aaa
            //     bbb
            //
            // Should be rendered as:
            //     <p>aaa
            //     bbb</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("   aaa\nbbb", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>aaa\nbbb</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Paragraphs_Spec188_CommonMark()
        {
            // The following Markdown:
            //         aaa
            //     bbb
            //
            // Should be rendered as:
            //     <pre><code>aaa
            //     </code></pre>
            //     <p>bbb</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("    aaa\nbbb", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>aaa\n</code></pre>\n<p>bbb</p>");

            Assert.Equal(expectedResult, result);
        }

        // Final spaces are stripped before inline parsing, so a paragraph
        // that ends with two or more spaces will not end with a [hard line
        // break]:        
        [Fact]
        public void Paragraphs_Spec189_CommonMark()
        {
            // The following Markdown:
            //     aaa     
            //     bbb     
            //
            // Should be rendered as:
            //     <p>aaa<br />
            //     bbb</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("aaa     \nbbb     ", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>aaa<br />\nbbb</p>");

            Assert.Equal(expectedResult, result);
        }
    }

    // [Blank lines] between block-level elements are ignored,
    // except for the role they play in determining whether a [list]
    // is [tight] or [loose]  
    public class BlankLinesTests
    {

        // Blank lines at the beginning and end of the document are also ignored.        
        [Fact]
        public void BlankLines_Spec190_CommonMark()
        {
            // The following Markdown:
            //       
            //     
            //     aaa
            //       
            //     
            //     # aaa
            //     
            //       
            //
            // Should be rendered as:
            //     <p>aaa</p>
            //     <h1>aaa</h1>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("  \n\naaa\n  \n\n# aaa\n\n  ", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>aaa</p>\n<h1>aaa</h1>");

            Assert.Equal(expectedResult, result);
        }
    }

    // A [block quote marker](@)
    // consists of 0-3 spaces of initial indent, plus (a) the character `>` together
    // with a following space, or (b) a single character `>` not followed by a space.
    // 
    // The following rules define [block quotes]:
    // 
    // 1.  **Basic case.**  If a string of lines *Ls* constitute a sequence
    //     of blocks *Bs*, then the result of prepending a [block quote
    //     marker] to the beginning of each line in *Ls*
    //     is a [block quote](#block-quotes) containing *Bs*.
    // 
    // 2.  **Laziness.**  If a string of lines *Ls* constitute a [block
    //     quote](#block-quotes) with contents *Bs*, then the result of deleting
    //     the initial [block quote marker] from one or
    //     more lines in which the next [non-whitespace character] after the [block
    //     quote marker] is [paragraph continuation
    //     text] is a block quote with *Bs* as its content.
    //     [Paragraph continuation text](@) is text
    //     that will be parsed as part of the content of a paragraph, but does
    //     not occur at the beginning of the paragraph.
    // 
    // 3.  **Consecutiveness.**  A document cannot contain two [block
    //     quotes] in a row unless there is a [blank line] between them.
    // 
    // Nothing else counts as a [block quote](#block-quotes)  
    public class BlockQuotesTests
    {

        // Here is a simple example:        
        [Fact]
        public void BlockQuotes_Spec191_CommonMark()
        {
            // The following Markdown:
            //     > # Foo
            //     > bar
            //     > baz
            //
            // Should be rendered as:
            //     <blockquote>
            //     <h1>Foo</h1>
            //     <p>bar
            //     baz</p>
            //     </blockquote>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("> # Foo\n> bar\n> baz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<h1>Foo</h1>\n<p>bar\nbaz</p>\n</blockquote>");

            Assert.Equal(expectedResult, result);
        }

        // The spaces after the `>` characters can be omitted:        
        [Fact]
        public void BlockQuotes_Spec192_CommonMark()
        {
            // The following Markdown:
            //     ># Foo
            //     >bar
            //     > baz
            //
            // Should be rendered as:
            //     <blockquote>
            //     <h1>Foo</h1>
            //     <p>bar
            //     baz</p>
            //     </blockquote>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("># Foo\n>bar\n> baz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<h1>Foo</h1>\n<p>bar\nbaz</p>\n</blockquote>");

            Assert.Equal(expectedResult, result);
        }

        // The `>` characters can be indented 1-3 spaces:        
        [Fact]
        public void BlockQuotes_Spec193_CommonMark()
        {
            // The following Markdown:
            //        > # Foo
            //        > bar
            //      > baz
            //
            // Should be rendered as:
            //     <blockquote>
            //     <h1>Foo</h1>
            //     <p>bar
            //     baz</p>
            //     </blockquote>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("   > # Foo\n   > bar\n > baz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<h1>Foo</h1>\n<p>bar\nbaz</p>\n</blockquote>");

            Assert.Equal(expectedResult, result);
        }

        // Four spaces gives us a code block:        
        [Fact]
        public void BlockQuotes_Spec194_CommonMark()
        {
            // The following Markdown:
            //         > # Foo
            //         > bar
            //         > baz
            //
            // Should be rendered as:
            //     <pre><code>&gt; # Foo
            //     &gt; bar
            //     &gt; baz
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("    > # Foo\n    > bar\n    > baz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>&gt; # Foo\n&gt; bar\n&gt; baz\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }

        // The Laziness clause allows us to omit the `>` before
        // [paragraph continuation text]:        
        [Fact]
        public void BlockQuotes_Spec195_CommonMark()
        {
            // The following Markdown:
            //     > # Foo
            //     > bar
            //     baz
            //
            // Should be rendered as:
            //     <blockquote>
            //     <h1>Foo</h1>
            //     <p>bar
            //     baz</p>
            //     </blockquote>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("> # Foo\n> bar\nbaz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<h1>Foo</h1>\n<p>bar\nbaz</p>\n</blockquote>");

            Assert.Equal(expectedResult, result);
        }

        // A block quote can contain some lazy and some non-lazy
        // continuation lines:        
        [Fact]
        public void BlockQuotes_Spec196_CommonMark()
        {
            // The following Markdown:
            //     > bar
            //     baz
            //     > foo
            //
            // Should be rendered as:
            //     <blockquote>
            //     <p>bar
            //     baz
            //     foo</p>
            //     </blockquote>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("> bar\nbaz\n> foo", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<p>bar\nbaz\nfoo</p>\n</blockquote>");

            Assert.Equal(expectedResult, result);
        }

        // Laziness only applies to lines that would have been continuations of
        // paragraphs had they been prepended with [block quote markers].
        // For example, the `> ` cannot be omitted in the second line of
        // 
        // ``` markdown
        // > foo
        // > ---
        // ```
        // 
        // without changing the meaning:        
        [Fact]
        public void BlockQuotes_Spec197_CommonMark()
        {
            // The following Markdown:
            //     > foo
            //     ---
            //
            // Should be rendered as:
            //     <blockquote>
            //     <p>foo</p>
            //     </blockquote>
            //     <hr />

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("> foo\n---", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<p>foo</p>\n</blockquote>\n<hr />");

            Assert.Equal(expectedResult, result);
        }

        // Similarly, if we omit the `> ` in the second line of
        // 
        // ``` markdown
        // > - foo
        // > - bar
        // ```
        // 
        // then the block quote ends after the first line:        
        [Fact]
        public void BlockQuotes_Spec198_CommonMark()
        {
            // The following Markdown:
            //     > - foo
            //     - bar
            //
            // Should be rendered as:
            //     <blockquote>
            //     <ul>
            //     <li>foo</li>
            //     </ul>
            //     </blockquote>
            //     <ul>
            //     <li>bar</li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("> - foo\n- bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<ul>\n<li>foo</li>\n</ul>\n</blockquote>\n<ul>\n<li>bar</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }

        // For the same reason, we can't omit the `> ` in front of
        // subsequent lines of an indented or fenced code block:        
        [Fact]
        public void BlockQuotes_Spec199_CommonMark()
        {
            // The following Markdown:
            //     >     foo
            //         bar
            //
            // Should be rendered as:
            //     <blockquote>
            //     <pre><code>foo
            //     </code></pre>
            //     </blockquote>
            //     <pre><code>bar
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml(">     foo\n    bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<pre><code>foo\n</code></pre>\n</blockquote>\n<pre><code>bar\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void BlockQuotes_Spec200_CommonMark()
        {
            // The following Markdown:
            //     > ```
            //     foo
            //     ```
            //
            // Should be rendered as:
            //     <blockquote>
            //     <pre><code></code></pre>
            //     </blockquote>
            //     <p>foo</p>
            //     <pre><code></code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("> ```\nfoo\n```", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<pre><code></code></pre>\n</blockquote>\n<p>foo</p>\n<pre><code></code></pre>");

            Assert.Equal(expectedResult, result);
        }

        // Note that in the following case, we have a [lazy
        // continuation line]:        
        [Fact]
        public void BlockQuotes_Spec201_CommonMark()
        {
            // The following Markdown:
            //     > foo
            //         - bar
            //
            // Should be rendered as:
            //     <blockquote>
            //     <p>foo
            //     - bar</p>
            //     </blockquote>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("> foo\n    - bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<p>foo\n- bar</p>\n</blockquote>");

            Assert.Equal(expectedResult, result);
        }

        // To see why, note that in
        // 
        // ```markdown
        // > foo
        // >     - bar
        // ```
        // 
        // the `- bar` is indented too far to start a list, and can't
        // be an indented code block because indented code blocks cannot
        // interrupt paragraphs, so it is [paragraph continuation text].
        // 
        // A block quote can be empty:        
        [Fact]
        public void BlockQuotes_Spec202_CommonMark()
        {
            // The following Markdown:
            //     >
            //
            // Should be rendered as:
            //     <blockquote>
            //     </blockquote>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml(">", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n</blockquote>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void BlockQuotes_Spec203_CommonMark()
        {
            // The following Markdown:
            //     >
            //     >  
            //     > 
            //
            // Should be rendered as:
            //     <blockquote>
            //     </blockquote>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml(">\n>  \n> ", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n</blockquote>");

            Assert.Equal(expectedResult, result);
        }

        // A block quote can have initial or final blank lines:        
        [Fact]
        public void BlockQuotes_Spec204_CommonMark()
        {
            // The following Markdown:
            //     >
            //     > foo
            //     >  
            //
            // Should be rendered as:
            //     <blockquote>
            //     <p>foo</p>
            //     </blockquote>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml(">\n> foo\n>  ", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<p>foo</p>\n</blockquote>");

            Assert.Equal(expectedResult, result);
        }

        // A blank line always separates block quotes:        
        [Fact]
        public void BlockQuotes_Spec205_CommonMark()
        {
            // The following Markdown:
            //     > foo
            //     
            //     > bar
            //
            // Should be rendered as:
            //     <blockquote>
            //     <p>foo</p>
            //     </blockquote>
            //     <blockquote>
            //     <p>bar</p>
            //     </blockquote>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("> foo\n\n> bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<p>foo</p>\n</blockquote>\n<blockquote>\n<p>bar</p>\n</blockquote>");

            Assert.Equal(expectedResult, result);
        }

        // (Most current Markdown implementations, including John Gruber's
        // original `Markdown.pl`, will parse this example as a single block quote
        // with two paragraphs.  But it seems better to allow the author to decide
        // whether two block quotes or one are wanted.)
        // 
        // Consecutiveness means that if we put these block quotes together,
        // we get a single block quote:        
        [Fact]
        public void BlockQuotes_Spec206_CommonMark()
        {
            // The following Markdown:
            //     > foo
            //     > bar
            //
            // Should be rendered as:
            //     <blockquote>
            //     <p>foo
            //     bar</p>
            //     </blockquote>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("> foo\n> bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<p>foo\nbar</p>\n</blockquote>");

            Assert.Equal(expectedResult, result);
        }

        // To get a block quote with two paragraphs, use:        
        [Fact]
        public void BlockQuotes_Spec207_CommonMark()
        {
            // The following Markdown:
            //     > foo
            //     >
            //     > bar
            //
            // Should be rendered as:
            //     <blockquote>
            //     <p>foo</p>
            //     <p>bar</p>
            //     </blockquote>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("> foo\n>\n> bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<p>foo</p>\n<p>bar</p>\n</blockquote>");

            Assert.Equal(expectedResult, result);
        }

        // Block quotes can interrupt paragraphs:        
        [Fact]
        public void BlockQuotes_Spec208_CommonMark()
        {
            // The following Markdown:
            //     foo
            //     > bar
            //
            // Should be rendered as:
            //     <p>foo</p>
            //     <blockquote>
            //     <p>bar</p>
            //     </blockquote>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo\n> bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo</p>\n<blockquote>\n<p>bar</p>\n</blockquote>");

            Assert.Equal(expectedResult, result);
        }

        // In general, blank lines are not needed before or after block
        // quotes:        
        [Fact]
        public void BlockQuotes_Spec209_CommonMark()
        {
            // The following Markdown:
            //     > aaa
            //     ***
            //     > bbb
            //
            // Should be rendered as:
            //     <blockquote>
            //     <p>aaa</p>
            //     </blockquote>
            //     <hr />
            //     <blockquote>
            //     <p>bbb</p>
            //     </blockquote>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("> aaa\n***\n> bbb", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<p>aaa</p>\n</blockquote>\n<hr />\n<blockquote>\n<p>bbb</p>\n</blockquote>");

            Assert.Equal(expectedResult, result);
        }

        // However, because of laziness, a blank line is needed between
        // a block quote and a following paragraph:        
        [Fact]
        public void BlockQuotes_Spec210_CommonMark()
        {
            // The following Markdown:
            //     > bar
            //     baz
            //
            // Should be rendered as:
            //     <blockquote>
            //     <p>bar
            //     baz</p>
            //     </blockquote>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("> bar\nbaz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<p>bar\nbaz</p>\n</blockquote>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void BlockQuotes_Spec211_CommonMark()
        {
            // The following Markdown:
            //     > bar
            //     
            //     baz
            //
            // Should be rendered as:
            //     <blockquote>
            //     <p>bar</p>
            //     </blockquote>
            //     <p>baz</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("> bar\n\nbaz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<p>bar</p>\n</blockquote>\n<p>baz</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void BlockQuotes_Spec212_CommonMark()
        {
            // The following Markdown:
            //     > bar
            //     >
            //     baz
            //
            // Should be rendered as:
            //     <blockquote>
            //     <p>bar</p>
            //     </blockquote>
            //     <p>baz</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("> bar\n>\nbaz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<p>bar</p>\n</blockquote>\n<p>baz</p>");

            Assert.Equal(expectedResult, result);
        }

        // It is a consequence of the Laziness rule that any number
        // of initial `>`s may be omitted on a continuation line of a
        // nested block quote:        
        [Fact]
        public void BlockQuotes_Spec213_CommonMark()
        {
            // The following Markdown:
            //     > > > foo
            //     bar
            //
            // Should be rendered as:
            //     <blockquote>
            //     <blockquote>
            //     <blockquote>
            //     <p>foo
            //     bar</p>
            //     </blockquote>
            //     </blockquote>
            //     </blockquote>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("> > > foo\nbar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<blockquote>\n<blockquote>\n<p>foo\nbar</p>\n</blockquote>\n</blockquote>\n</blockquote>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void BlockQuotes_Spec214_CommonMark()
        {
            // The following Markdown:
            //     >>> foo
            //     > bar
            //     >>baz
            //
            // Should be rendered as:
            //     <blockquote>
            //     <blockquote>
            //     <blockquote>
            //     <p>foo
            //     bar
            //     baz</p>
            //     </blockquote>
            //     </blockquote>
            //     </blockquote>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml(">>> foo\n> bar\n>>baz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<blockquote>\n<blockquote>\n<p>foo\nbar\nbaz</p>\n</blockquote>\n</blockquote>\n</blockquote>");

            Assert.Equal(expectedResult, result);
        }

        // When including an indented code block in a block quote,
        // remember that the [block quote marker] includes
        // both the `>` and a following space.  So *five spaces* are needed after
        // the `>`:        
        [Fact]
        public void BlockQuotes_Spec215_CommonMark()
        {
            // The following Markdown:
            //     >     code
            //     
            //     >    not code
            //
            // Should be rendered as:
            //     <blockquote>
            //     <pre><code>code
            //     </code></pre>
            //     </blockquote>
            //     <blockquote>
            //     <p>not code</p>
            //     </blockquote>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml(">     code\n\n>    not code", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<pre><code>code\n</code></pre>\n</blockquote>\n<blockquote>\n<p>not code</p>\n</blockquote>");

            Assert.Equal(expectedResult, result);
        }
    }

    // A [list marker](@) is a
    // [bullet list marker] or an [ordered list marker].
    // 
    // A [bullet list marker](@)
    // is a `-`, `+`, or `*` character.
    // 
    // An [ordered list marker](@)
    // is a sequence of 1--9 arabic digits (`0-9`), followed by either a
    // `.` character or a `)` character.  (The reason for the length
    // limit is that with 10 digits we start seeing integer overflows
    // in some browsers.)
    // 
    // The following rules define [list items]:
    // 
    // 1.  **Basic case.**  If a sequence of lines *Ls* constitute a sequence of
    //     blocks *Bs* starting with a [non-whitespace character] and not separated
    //     from each other by more than one blank line, and *M* is a list
    //     marker of width *W* followed by 1 ≤ *N* ≤ 4 spaces, then the result
    //     of prepending *M* and the following spaces to the first line of
    //     *Ls*, and indenting subsequent lines of *Ls* by *W + N* spaces, is a
    //     list item with *Bs* as its contents.  The type of the list item
    //     (bullet or ordered) is determined by the type of its list marker.
    //     If the list item is ordered, then it is also assigned a start
    //     number, based on the ordered list marker.
    // 
    //     Exceptions:
    // 
    //     1. When the first list item in a [list] interrupts
    //        a paragraph---that is, when it starts on a line that would
    //        otherwise count as [paragraph continuation text]---then (a)
    //        the lines *Ls* must not begin with a blank line, and (b) if
    //        the list item is ordered, the start number must be 1.
    //     2. If any line is a [thematic break][thematic breaks] then
    //        that line is not a list item  
    public class ListItemsTests
    {

        // For example, let *Ls* be the lines        
        [Fact]
        public void ListItems_Spec216_CommonMark()
        {
            // The following Markdown:
            //     A paragraph
            //     with two lines.
            //     
            //         indented code
            //     
            //     > A block quote.
            //
            // Should be rendered as:
            //     <p>A paragraph
            //     with two lines.</p>
            //     <pre><code>indented code
            //     </code></pre>
            //     <blockquote>
            //     <p>A block quote.</p>
            //     </blockquote>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("A paragraph\nwith two lines.\n\n    indented code\n\n> A block quote.", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>");

            Assert.Equal(expectedResult, result);
        }

        // And let *M* be the marker `1.`, and *N* = 2.  Then rule #1 says
        // that the following is an ordered list item with start number 1,
        // and the same contents as *Ls*:        
        [Fact]
        public void ListItems_Spec217_CommonMark()
        {
            // The following Markdown:
            //     1.  A paragraph
            //         with two lines.
            //     
            //             indented code
            //     
            //         > A block quote.
            //
            // Should be rendered as:
            //     <ol>
            //     <li>
            //     <p>A paragraph
            //     with two lines.</p>
            //     <pre><code>indented code
            //     </code></pre>
            //     <blockquote>
            //     <p>A block quote.</p>
            //     </blockquote>
            //     </li>
            //     </ol>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("1.  A paragraph\n    with two lines.\n\n        indented code\n\n    > A block quote.", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>");

            Assert.Equal(expectedResult, result);
        }

        // The most important thing to notice is that the position of
        // the text after the list marker determines how much indentation
        // is needed in subsequent blocks in the list item.  If the list
        // marker takes up two spaces, and there are three spaces between
        // the list marker and the next [non-whitespace character], then blocks
        // must be indented five spaces in order to fall under the list
        // item.
        // 
        // Here are some examples showing how far content must be indented to be
        // put under the list item:        
        [Fact]
        public void ListItems_Spec218_CommonMark()
        {
            // The following Markdown:
            //     - one
            //     
            //      two
            //
            // Should be rendered as:
            //     <ul>
            //     <li>one</li>
            //     </ul>
            //     <p>two</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- one\n\n two", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>one</li>\n</ul>\n<p>two</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void ListItems_Spec219_CommonMark()
        {
            // The following Markdown:
            //     - one
            //     
            //       two
            //
            // Should be rendered as:
            //     <ul>
            //     <li>
            //     <p>one</p>
            //     <p>two</p>
            //     </li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- one\n\n  two", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>\n<p>one</p>\n<p>two</p>\n</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void ListItems_Spec220_CommonMark()
        {
            // The following Markdown:
            //      -    one
            //     
            //          two
            //
            // Should be rendered as:
            //     <ul>
            //     <li>one</li>
            //     </ul>
            //     <pre><code> two
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml(" -    one\n\n     two", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>one</li>\n</ul>\n<pre><code> two\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void ListItems_Spec221_CommonMark()
        {
            // The following Markdown:
            //      -    one
            //     
            //           two
            //
            // Should be rendered as:
            //     <ul>
            //     <li>
            //     <p>one</p>
            //     <p>two</p>
            //     </li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml(" -    one\n\n      two", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>\n<p>one</p>\n<p>two</p>\n</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }

        // It is tempting to think of this in terms of columns:  the continuation
        // blocks must be indented at least to the column of the first
        // [non-whitespace character] after the list marker. However, that is not quite right.
        // The spaces after the list marker determine how much relative indentation
        // is needed.  Which column this indentation reaches will depend on
        // how the list item is embedded in other constructions, as shown by
        // this example:        
        [Fact]
        public void ListItems_Spec222_CommonMark()
        {
            // The following Markdown:
            //        > > 1.  one
            //     >>
            //     >>     two
            //
            // Should be rendered as:
            //     <blockquote>
            //     <blockquote>
            //     <ol>
            //     <li>
            //     <p>one</p>
            //     <p>two</p>
            //     </li>
            //     </ol>
            //     </blockquote>
            //     </blockquote>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("   > > 1.  one\n>>\n>>     two", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<blockquote>\n<ol>\n<li>\n<p>one</p>\n<p>two</p>\n</li>\n</ol>\n</blockquote>\n</blockquote>");

            Assert.Equal(expectedResult, result);
        }

        // Here `two` occurs in the same column as the list marker `1.`,
        // but is actually contained in the list item, because there is
        // sufficient indentation after the last containing blockquote marker.
        // 
        // The converse is also possible.  In the following example, the word `two`
        // occurs far to the right of the initial text of the list item, `one`, but
        // it is not considered part of the list item, because it is not indented
        // far enough past the blockquote marker:        
        [Fact]
        public void ListItems_Spec223_CommonMark()
        {
            // The following Markdown:
            //     >>- one
            //     >>
            //       >  > two
            //
            // Should be rendered as:
            //     <blockquote>
            //     <blockquote>
            //     <ul>
            //     <li>one</li>
            //     </ul>
            //     <p>two</p>
            //     </blockquote>
            //     </blockquote>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml(">>- one\n>>\n  >  > two", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<blockquote>\n<ul>\n<li>one</li>\n</ul>\n<p>two</p>\n</blockquote>\n</blockquote>");

            Assert.Equal(expectedResult, result);
        }

        // Note that at least one space is needed between the list marker and
        // any following content, so these are not list items:        
        [Fact]
        public void ListItems_Spec224_CommonMark()
        {
            // The following Markdown:
            //     -one
            //     
            //     2.two
            //
            // Should be rendered as:
            //     <p>-one</p>
            //     <p>2.two</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("-one\n\n2.two", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>-one</p>\n<p>2.two</p>");

            Assert.Equal(expectedResult, result);
        }

        // A list item may contain blocks that are separated by more than
        // one blank line.        
        [Fact]
        public void ListItems_Spec225_CommonMark()
        {
            // The following Markdown:
            //     - foo
            //     
            //     
            //       bar
            //
            // Should be rendered as:
            //     <ul>
            //     <li>
            //     <p>foo</p>
            //     <p>bar</p>
            //     </li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- foo\n\n\n  bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }

        // A list item may contain any kind of block:        
        [Fact]
        public void ListItems_Spec226_CommonMark()
        {
            // The following Markdown:
            //     1.  foo
            //     
            //         ```
            //         bar
            //         ```
            //     
            //         baz
            //     
            //         > bam
            //
            // Should be rendered as:
            //     <ol>
            //     <li>
            //     <p>foo</p>
            //     <pre><code>bar
            //     </code></pre>
            //     <p>baz</p>
            //     <blockquote>
            //     <p>bam</p>
            //     </blockquote>
            //     </li>
            //     </ol>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("1.  foo\n\n    ```\n    bar\n    ```\n\n    baz\n\n    > bam", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ol>\n<li>\n<p>foo</p>\n<pre><code>bar\n</code></pre>\n<p>baz</p>\n<blockquote>\n<p>bam</p>\n</blockquote>\n</li>\n</ol>");

            Assert.Equal(expectedResult, result);
        }

        // A list item that contains an indented code block will preserve
        // empty lines within the code block verbatim.        
        [Fact]
        public void ListItems_Spec227_CommonMark()
        {
            // The following Markdown:
            //     - Foo
            //     
            //           bar
            //     
            //     
            //           baz
            //
            // Should be rendered as:
            //     <ul>
            //     <li>
            //     <p>Foo</p>
            //     <pre><code>bar
            //     
            //     
            //     baz
            //     </code></pre>
            //     </li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- Foo\n\n      bar\n\n\n      baz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>\n<p>Foo</p>\n<pre><code>bar\n\n\nbaz\n</code></pre>\n</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }

        // Note that ordered list start numbers must be nine digits or less:        
        [Fact]
        public void ListItems_Spec228_CommonMark()
        {
            // The following Markdown:
            //     123456789. ok
            //
            // Should be rendered as:
            //     <ol start="123456789">
            //     <li>ok</li>
            //     </ol>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("123456789. ok", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ol start=\"123456789\">\n<li>ok</li>\n</ol>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void ListItems_Spec229_CommonMark()
        {
            // The following Markdown:
            //     1234567890. not ok
            //
            // Should be rendered as:
            //     <p>1234567890. not ok</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("1234567890. not ok", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>1234567890. not ok</p>");

            Assert.Equal(expectedResult, result);
        }

        // A start number may begin with 0s:        
        [Fact]
        public void ListItems_Spec230_CommonMark()
        {
            // The following Markdown:
            //     0. ok
            //
            // Should be rendered as:
            //     <ol start="0">
            //     <li>ok</li>
            //     </ol>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("0. ok", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ol start=\"0\">\n<li>ok</li>\n</ol>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void ListItems_Spec231_CommonMark()
        {
            // The following Markdown:
            //     003. ok
            //
            // Should be rendered as:
            //     <ol start="3">
            //     <li>ok</li>
            //     </ol>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("003. ok", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ol start=\"3\">\n<li>ok</li>\n</ol>");

            Assert.Equal(expectedResult, result);
        }

        // A start number may not be negative:        
        [Fact]
        public void ListItems_Spec232_CommonMark()
        {
            // The following Markdown:
            //     -1. not ok
            //
            // Should be rendered as:
            //     <p>-1. not ok</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("-1. not ok", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>-1. not ok</p>");

            Assert.Equal(expectedResult, result);
        }

        // 2.  **Item starting with indented code.**  If a sequence of lines *Ls*
        //     constitute a sequence of blocks *Bs* starting with an indented code
        //     block and not separated from each other by more than one blank line,
        //     and *M* is a list marker of width *W* followed by
        //     one space, then the result of prepending *M* and the following
        //     space to the first line of *Ls*, and indenting subsequent lines of
        //     *Ls* by *W + 1* spaces, is a list item with *Bs* as its contents.
        //     If a line is empty, then it need not be indented.  The type of the
        //     list item (bullet or ordered) is determined by the type of its list
        //     marker.  If the list item is ordered, then it is also assigned a
        //     start number, based on the ordered list marker.
        // 
        // An indented code block will have to be indented four spaces beyond
        // the edge of the region where text will be included in the list item.
        // In the following case that is 6 spaces:        
        [Fact]
        public void ListItems_Spec233_CommonMark()
        {
            // The following Markdown:
            //     - foo
            //     
            //           bar
            //
            // Should be rendered as:
            //     <ul>
            //     <li>
            //     <p>foo</p>
            //     <pre><code>bar
            //     </code></pre>
            //     </li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- foo\n\n      bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>\n<p>foo</p>\n<pre><code>bar\n</code></pre>\n</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }

        // And in this case it is 11 spaces:        
        [Fact]
        public void ListItems_Spec234_CommonMark()
        {
            // The following Markdown:
            //       10.  foo
            //     
            //                bar
            //
            // Should be rendered as:
            //     <ol start="10">
            //     <li>
            //     <p>foo</p>
            //     <pre><code>bar
            //     </code></pre>
            //     </li>
            //     </ol>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("  10.  foo\n\n           bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ol start=\"10\">\n<li>\n<p>foo</p>\n<pre><code>bar\n</code></pre>\n</li>\n</ol>");

            Assert.Equal(expectedResult, result);
        }

        // If the *first* block in the list item is an indented code block,
        // then by rule #2, the contents must be indented *one* space after the
        // list marker:        
        [Fact]
        public void ListItems_Spec235_CommonMark()
        {
            // The following Markdown:
            //         indented code
            //     
            //     paragraph
            //     
            //         more code
            //
            // Should be rendered as:
            //     <pre><code>indented code
            //     </code></pre>
            //     <p>paragraph</p>
            //     <pre><code>more code
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("    indented code\n\nparagraph\n\n    more code", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>indented code\n</code></pre>\n<p>paragraph</p>\n<pre><code>more code\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void ListItems_Spec236_CommonMark()
        {
            // The following Markdown:
            //     1.     indented code
            //     
            //        paragraph
            //     
            //            more code
            //
            // Should be rendered as:
            //     <ol>
            //     <li>
            //     <pre><code>indented code
            //     </code></pre>
            //     <p>paragraph</p>
            //     <pre><code>more code
            //     </code></pre>
            //     </li>
            //     </ol>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("1.     indented code\n\n   paragraph\n\n       more code", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ol>\n<li>\n<pre><code>indented code\n</code></pre>\n<p>paragraph</p>\n<pre><code>more code\n</code></pre>\n</li>\n</ol>");

            Assert.Equal(expectedResult, result);
        }

        // Note that an additional space indent is interpreted as space
        // inside the code block:        
        [Fact]
        public void ListItems_Spec237_CommonMark()
        {
            // The following Markdown:
            //     1.      indented code
            //     
            //        paragraph
            //     
            //            more code
            //
            // Should be rendered as:
            //     <ol>
            //     <li>
            //     <pre><code> indented code
            //     </code></pre>
            //     <p>paragraph</p>
            //     <pre><code>more code
            //     </code></pre>
            //     </li>
            //     </ol>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("1.      indented code\n\n   paragraph\n\n       more code", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ol>\n<li>\n<pre><code> indented code\n</code></pre>\n<p>paragraph</p>\n<pre><code>more code\n</code></pre>\n</li>\n</ol>");

            Assert.Equal(expectedResult, result);
        }

        // Note that rules #1 and #2 only apply to two cases:  (a) cases
        // in which the lines to be included in a list item begin with a
        // [non-whitespace character], and (b) cases in which
        // they begin with an indented code
        // block.  In a case like the following, where the first block begins with
        // a three-space indent, the rules do not allow us to form a list item by
        // indenting the whole thing and prepending a list marker:        
        [Fact]
        public void ListItems_Spec238_CommonMark()
        {
            // The following Markdown:
            //        foo
            //     
            //     bar
            //
            // Should be rendered as:
            //     <p>foo</p>
            //     <p>bar</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("   foo\n\nbar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo</p>\n<p>bar</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void ListItems_Spec239_CommonMark()
        {
            // The following Markdown:
            //     -    foo
            //     
            //       bar
            //
            // Should be rendered as:
            //     <ul>
            //     <li>foo</li>
            //     </ul>
            //     <p>bar</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("-    foo\n\n  bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>foo</li>\n</ul>\n<p>bar</p>");

            Assert.Equal(expectedResult, result);
        }

        // This is not a significant restriction, because when a block begins
        // with 1-3 spaces indent, the indentation can always be removed without
        // a change in interpretation, allowing rule #1 to be applied.  So, in
        // the above case:        
        [Fact]
        public void ListItems_Spec240_CommonMark()
        {
            // The following Markdown:
            //     -  foo
            //     
            //        bar
            //
            // Should be rendered as:
            //     <ul>
            //     <li>
            //     <p>foo</p>
            //     <p>bar</p>
            //     </li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("-  foo\n\n   bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }

        // 3.  **Item starting with a blank line.**  If a sequence of lines *Ls*
        //     starting with a single [blank line] constitute a (possibly empty)
        //     sequence of blocks *Bs*, not separated from each other by more than
        //     one blank line, and *M* is a list marker of width *W*,
        //     then the result of prepending *M* to the first line of *Ls*, and
        //     indenting subsequent lines of *Ls* by *W + 1* spaces, is a list
        //     item with *Bs* as its contents.
        //     If a line is empty, then it need not be indented.  The type of the
        //     list item (bullet or ordered) is determined by the type of its list
        //     marker.  If the list item is ordered, then it is also assigned a
        //     start number, based on the ordered list marker.
        // 
        // Here are some list items that start with a blank line but are not empty:        
        [Fact]
        public void ListItems_Spec241_CommonMark()
        {
            // The following Markdown:
            //     -
            //       foo
            //     -
            //       ```
            //       bar
            //       ```
            //     -
            //           baz
            //
            // Should be rendered as:
            //     <ul>
            //     <li>foo</li>
            //     <li>
            //     <pre><code>bar
            //     </code></pre>
            //     </li>
            //     <li>
            //     <pre><code>baz
            //     </code></pre>
            //     </li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("-\n  foo\n-\n  ```\n  bar\n  ```\n-\n      baz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>foo</li>\n<li>\n<pre><code>bar\n</code></pre>\n</li>\n<li>\n<pre><code>baz\n</code></pre>\n</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }

        // When the list item starts with a blank line, the number of spaces
        // following the list marker doesn't change the required indentation:        
        [Fact]
        public void ListItems_Spec242_CommonMark()
        {
            // The following Markdown:
            //     -   
            //       foo
            //
            // Should be rendered as:
            //     <ul>
            //     <li>foo</li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("-   \n  foo", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>foo</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }

        // A list item can begin with at most one blank line.
        // In the following example, `foo` is not part of the list
        // item:        
        [Fact]
        public void ListItems_Spec243_CommonMark()
        {
            // The following Markdown:
            //     -
            //     
            //       foo
            //
            // Should be rendered as:
            //     <ul>
            //     <li></li>
            //     </ul>
            //     <p>foo</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("-\n\n  foo", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li></li>\n</ul>\n<p>foo</p>");

            Assert.Equal(expectedResult, result);
        }

        // Here is an empty bullet list item:        
        [Fact]
        public void ListItems_Spec244_CommonMark()
        {
            // The following Markdown:
            //     - foo
            //     -
            //     - bar
            //
            // Should be rendered as:
            //     <ul>
            //     <li>foo</li>
            //     <li></li>
            //     <li>bar</li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- foo\n-\n- bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>foo</li>\n<li></li>\n<li>bar</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }

        // It does not matter whether there are spaces following the [list marker]:        
        [Fact]
        public void ListItems_Spec245_CommonMark()
        {
            // The following Markdown:
            //     - foo
            //     -   
            //     - bar
            //
            // Should be rendered as:
            //     <ul>
            //     <li>foo</li>
            //     <li></li>
            //     <li>bar</li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- foo\n-   \n- bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>foo</li>\n<li></li>\n<li>bar</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }

        // Here is an empty ordered list item:        
        [Fact]
        public void ListItems_Spec246_CommonMark()
        {
            // The following Markdown:
            //     1. foo
            //     2.
            //     3. bar
            //
            // Should be rendered as:
            //     <ol>
            //     <li>foo</li>
            //     <li></li>
            //     <li>bar</li>
            //     </ol>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("1. foo\n2.\n3. bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ol>\n<li>foo</li>\n<li></li>\n<li>bar</li>\n</ol>");

            Assert.Equal(expectedResult, result);
        }

        // A list may start or end with an empty list item:        
        [Fact]
        public void ListItems_Spec247_CommonMark()
        {
            // The following Markdown:
            //     *
            //
            // Should be rendered as:
            //     <ul>
            //     <li></li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li></li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }

        // However, an empty list item cannot interrupt a paragraph:        
        [Fact]
        public void ListItems_Spec248_CommonMark()
        {
            // The following Markdown:
            //     foo
            //     *
            //     
            //     foo
            //     1.
            //
            // Should be rendered as:
            //     <p>foo
            //     *</p>
            //     <p>foo
            //     1.</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo\n*\n\nfoo\n1.", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo\n*</p>\n<p>foo\n1.</p>");

            Assert.Equal(expectedResult, result);
        }

        // 4.  **Indentation.**  If a sequence of lines *Ls* constitutes a list item
        //     according to rule #1, #2, or #3, then the result of indenting each line
        //     of *Ls* by 1-3 spaces (the same for each line) also constitutes a
        //     list item with the same contents and attributes.  If a line is
        //     empty, then it need not be indented.
        // 
        // Indented one space:        
        [Fact]
        public void ListItems_Spec249_CommonMark()
        {
            // The following Markdown:
            //      1.  A paragraph
            //          with two lines.
            //     
            //              indented code
            //     
            //          > A block quote.
            //
            // Should be rendered as:
            //     <ol>
            //     <li>
            //     <p>A paragraph
            //     with two lines.</p>
            //     <pre><code>indented code
            //     </code></pre>
            //     <blockquote>
            //     <p>A block quote.</p>
            //     </blockquote>
            //     </li>
            //     </ol>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml(" 1.  A paragraph\n     with two lines.\n\n         indented code\n\n     > A block quote.", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>");

            Assert.Equal(expectedResult, result);
        }

        // Indented two spaces:        
        [Fact]
        public void ListItems_Spec250_CommonMark()
        {
            // The following Markdown:
            //       1.  A paragraph
            //           with two lines.
            //     
            //               indented code
            //     
            //           > A block quote.
            //
            // Should be rendered as:
            //     <ol>
            //     <li>
            //     <p>A paragraph
            //     with two lines.</p>
            //     <pre><code>indented code
            //     </code></pre>
            //     <blockquote>
            //     <p>A block quote.</p>
            //     </blockquote>
            //     </li>
            //     </ol>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("  1.  A paragraph\n      with two lines.\n\n          indented code\n\n      > A block quote.", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>");

            Assert.Equal(expectedResult, result);
        }

        // Indented three spaces:        
        [Fact]
        public void ListItems_Spec251_CommonMark()
        {
            // The following Markdown:
            //        1.  A paragraph
            //            with two lines.
            //     
            //                indented code
            //     
            //            > A block quote.
            //
            // Should be rendered as:
            //     <ol>
            //     <li>
            //     <p>A paragraph
            //     with two lines.</p>
            //     <pre><code>indented code
            //     </code></pre>
            //     <blockquote>
            //     <p>A block quote.</p>
            //     </blockquote>
            //     </li>
            //     </ol>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("   1.  A paragraph\n       with two lines.\n\n           indented code\n\n       > A block quote.", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>");

            Assert.Equal(expectedResult, result);
        }

        // Four spaces indent gives a code block:        
        [Fact]
        public void ListItems_Spec252_CommonMark()
        {
            // The following Markdown:
            //         1.  A paragraph
            //             with two lines.
            //     
            //                 indented code
            //     
            //             > A block quote.
            //
            // Should be rendered as:
            //     <pre><code>1.  A paragraph
            //         with two lines.
            //     
            //             indented code
            //     
            //         &gt; A block quote.
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("    1.  A paragraph\n        with two lines.\n\n            indented code\n\n        > A block quote.", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>1.  A paragraph\n    with two lines.\n\n        indented code\n\n    &gt; A block quote.\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }

        // 5.  **Laziness.**  If a string of lines *Ls* constitute a [list
        //     item](#list-items) with contents *Bs*, then the result of deleting
        //     some or all of the indentation from one or more lines in which the
        //     next [non-whitespace character] after the indentation is
        //     [paragraph continuation text] is a
        //     list item with the same contents and attributes.  The unindented
        //     lines are called
        //     [lazy continuation line](@)s.
        // 
        // Here is an example with [lazy continuation lines]:        
        [Fact]
        public void ListItems_Spec253_CommonMark()
        {
            // The following Markdown:
            //       1.  A paragraph
            //     with two lines.
            //     
            //               indented code
            //     
            //           > A block quote.
            //
            // Should be rendered as:
            //     <ol>
            //     <li>
            //     <p>A paragraph
            //     with two lines.</p>
            //     <pre><code>indented code
            //     </code></pre>
            //     <blockquote>
            //     <p>A block quote.</p>
            //     </blockquote>
            //     </li>
            //     </ol>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("  1.  A paragraph\nwith two lines.\n\n          indented code\n\n      > A block quote.", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>");

            Assert.Equal(expectedResult, result);
        }

        // Indentation can be partially deleted:        
        [Fact]
        public void ListItems_Spec254_CommonMark()
        {
            // The following Markdown:
            //       1.  A paragraph
            //         with two lines.
            //
            // Should be rendered as:
            //     <ol>
            //     <li>A paragraph
            //     with two lines.</li>
            //     </ol>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("  1.  A paragraph\n    with two lines.", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ol>\n<li>A paragraph\nwith two lines.</li>\n</ol>");

            Assert.Equal(expectedResult, result);
        }

        // These examples show how laziness can work in nested structures:        
        [Fact]
        public void ListItems_Spec255_CommonMark()
        {
            // The following Markdown:
            //     > 1. > Blockquote
            //     continued here.
            //
            // Should be rendered as:
            //     <blockquote>
            //     <ol>
            //     <li>
            //     <blockquote>
            //     <p>Blockquote
            //     continued here.</p>
            //     </blockquote>
            //     </li>
            //     </ol>
            //     </blockquote>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("> 1. > Blockquote\ncontinued here.", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<ol>\n<li>\n<blockquote>\n<p>Blockquote\ncontinued here.</p>\n</blockquote>\n</li>\n</ol>\n</blockquote>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void ListItems_Spec256_CommonMark()
        {
            // The following Markdown:
            //     > 1. > Blockquote
            //     > continued here.
            //
            // Should be rendered as:
            //     <blockquote>
            //     <ol>
            //     <li>
            //     <blockquote>
            //     <p>Blockquote
            //     continued here.</p>
            //     </blockquote>
            //     </li>
            //     </ol>
            //     </blockquote>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("> 1. > Blockquote\n> continued here.", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<blockquote>\n<ol>\n<li>\n<blockquote>\n<p>Blockquote\ncontinued here.</p>\n</blockquote>\n</li>\n</ol>\n</blockquote>");

            Assert.Equal(expectedResult, result);
        }

        // 6.  **That's all.** Nothing that is not counted as a list item by rules
        //     #1--5 counts as a [list item](#list-items).
        // 
        // The rules for sublists follow from the general rules above.  A sublist
        // must be indented the same number of spaces a paragraph would need to be
        // in order to be included in the list item.
        // 
        // So, in this case we need two spaces indent:        
        [Fact]
        public void ListItems_Spec257_CommonMark()
        {
            // The following Markdown:
            //     - foo
            //       - bar
            //         - baz
            //           - boo
            //
            // Should be rendered as:
            //     <ul>
            //     <li>foo
            //     <ul>
            //     <li>bar
            //     <ul>
            //     <li>baz
            //     <ul>
            //     <li>boo</li>
            //     </ul>
            //     </li>
            //     </ul>
            //     </li>
            //     </ul>
            //     </li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- foo\n  - bar\n    - baz\n      - boo", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>foo\n<ul>\n<li>bar\n<ul>\n<li>baz\n<ul>\n<li>boo</li>\n</ul>\n</li>\n</ul>\n</li>\n</ul>\n</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }

        // One is not enough:        
        [Fact]
        public void ListItems_Spec258_CommonMark()
        {
            // The following Markdown:
            //     - foo
            //      - bar
            //       - baz
            //        - boo
            //
            // Should be rendered as:
            //     <ul>
            //     <li>foo</li>
            //     <li>bar</li>
            //     <li>baz</li>
            //     <li>boo</li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- foo\n - bar\n  - baz\n   - boo", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>foo</li>\n<li>bar</li>\n<li>baz</li>\n<li>boo</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }

        // Here we need four, because the list marker is wider:        
        [Fact]
        public void ListItems_Spec259_CommonMark()
        {
            // The following Markdown:
            //     10) foo
            //         - bar
            //
            // Should be rendered as:
            //     <ol start="10">
            //     <li>foo
            //     <ul>
            //     <li>bar</li>
            //     </ul>
            //     </li>
            //     </ol>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("10) foo\n    - bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ol start=\"10\">\n<li>foo\n<ul>\n<li>bar</li>\n</ul>\n</li>\n</ol>");

            Assert.Equal(expectedResult, result);
        }

        // Three is not enough:        
        [Fact]
        public void ListItems_Spec260_CommonMark()
        {
            // The following Markdown:
            //     10) foo
            //        - bar
            //
            // Should be rendered as:
            //     <ol start="10">
            //     <li>foo</li>
            //     </ol>
            //     <ul>
            //     <li>bar</li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("10) foo\n   - bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ol start=\"10\">\n<li>foo</li>\n</ol>\n<ul>\n<li>bar</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }

        // A list may be the first block in a list item:        
        [Fact]
        public void ListItems_Spec261_CommonMark()
        {
            // The following Markdown:
            //     - - foo
            //
            // Should be rendered as:
            //     <ul>
            //     <li>
            //     <ul>
            //     <li>foo</li>
            //     </ul>
            //     </li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- - foo", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>\n<ul>\n<li>foo</li>\n</ul>\n</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void ListItems_Spec262_CommonMark()
        {
            // The following Markdown:
            //     1. - 2. foo
            //
            // Should be rendered as:
            //     <ol>
            //     <li>
            //     <ul>
            //     <li>
            //     <ol start="2">
            //     <li>foo</li>
            //     </ol>
            //     </li>
            //     </ul>
            //     </li>
            //     </ol>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("1. - 2. foo", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ol>\n<li>\n<ul>\n<li>\n<ol start=\"2\">\n<li>foo</li>\n</ol>\n</li>\n</ul>\n</li>\n</ol>");

            Assert.Equal(expectedResult, result);
        }

        // A list item can contain a heading:        
        [Fact]
        public void ListItems_Spec263_CommonMark()
        {
            // The following Markdown:
            //     - # Foo
            //     - Bar
            //       ---
            //       baz
            //
            // Should be rendered as:
            //     <ul>
            //     <li>
            //     <h1>Foo</h1>
            //     </li>
            //     <li>
            //     <h2>Bar</h2>
            //     baz</li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- # Foo\n- Bar\n  ---\n  baz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>\n<h1>Foo</h1>\n</li>\n<li>\n<h2>Bar</h2>\nbaz</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }
    }

    // A [list](@) is a sequence of one or more
    // list items [of the same type].  The list items
    // may be separated by any number of blank lines.
    // 
    // Two list items are [of the same type](@)
    // if they begin with a [list marker] of the same type.
    // Two list markers are of the
    // same type if (a) they are bullet list markers using the same character
    // (`-`, `+`, or `*`) or (b) they are ordered list numbers with the same
    // delimiter (either `.` or `)`).
    // 
    // A list is an [ordered list](@)
    // if its constituent list items begin with
    // [ordered list markers], and a
    // [bullet list](@) if its constituent list
    // items begin with [bullet list markers].
    // 
    // The [start number](@)
    // of an [ordered list] is determined by the list number of
    // its initial list item.  The numbers of subsequent list items are
    // disregarded.
    // 
    // A list is [loose](@) if any of its constituent
    // list items are separated by blank lines, or if any of its constituent
    // list items directly contain two block-level elements with a blank line
    // between them.  Otherwise a list is [tight](@).
    // (The difference in HTML output is that paragraphs in a loose list are
    // wrapped in `<p>` tags, while paragraphs in a tight list are not.  
    public class ListsTests
    {

        // Changing the bullet or ordered list delimiter starts a new list:        
        [Fact]
        public void Lists_Spec264_CommonMark()
        {
            // The following Markdown:
            //     - foo
            //     - bar
            //     + baz
            //
            // Should be rendered as:
            //     <ul>
            //     <li>foo</li>
            //     <li>bar</li>
            //     </ul>
            //     <ul>
            //     <li>baz</li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- foo\n- bar\n+ baz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>foo</li>\n<li>bar</li>\n</ul>\n<ul>\n<li>baz</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Lists_Spec265_CommonMark()
        {
            // The following Markdown:
            //     1. foo
            //     2. bar
            //     3) baz
            //
            // Should be rendered as:
            //     <ol>
            //     <li>foo</li>
            //     <li>bar</li>
            //     </ol>
            //     <ol start="3">
            //     <li>baz</li>
            //     </ol>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("1. foo\n2. bar\n3) baz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ol>\n<li>foo</li>\n<li>bar</li>\n</ol>\n<ol start=\"3\">\n<li>baz</li>\n</ol>");

            Assert.Equal(expectedResult, result);
        }

        // In CommonMark, a list can interrupt a paragraph. That is,
        // no blank line is needed to separate a paragraph from a following
        // list:        
        [Fact]
        public void Lists_Spec266_CommonMark()
        {
            // The following Markdown:
            //     Foo
            //     - bar
            //     - baz
            //
            // Should be rendered as:
            //     <p>Foo</p>
            //     <ul>
            //     <li>bar</li>
            //     <li>baz</li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("Foo\n- bar\n- baz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>Foo</p>\n<ul>\n<li>bar</li>\n<li>baz</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }

        // `Markdown.pl` does not allow this, through fear of triggering a list
        // via a numeral in a hard-wrapped line:
        // 
        // ``` markdown
        // The number of windows in my house is
        // 14.  The number of doors is 6.
        // ```
        // 
        // Oddly, though, `Markdown.pl` *does* allow a blockquote to
        // interrupt a paragraph, even though the same considerations might
        // apply.
        // 
        // In CommonMark, we do allow lists to interrupt paragraphs, for
        // two reasons.  First, it is natural and not uncommon for people
        // to start lists without blank lines:
        // 
        // ``` markdown
        // I need to buy
        // - new shoes
        // - a coat
        // - a plane ticket
        // ```
        // 
        // Second, we are attracted to a
        // 
        // > [principle of uniformity](@):
        // > if a chunk of text has a certain
        // > meaning, it will continue to have the same meaning when put into a
        // > container block (such as a list item or blockquote).
        // 
        // (Indeed, the spec for [list items] and [block quotes] presupposes
        // this principle.) This principle implies that if
        // 
        // ``` markdown
        //   * I need to buy
        //     - new shoes
        //     - a coat
        //     - a plane ticket
        // ```
        // 
        // is a list item containing a paragraph followed by a nested sublist,
        // as all Markdown implementations agree it is (though the paragraph
        // may be rendered without `<p>` tags, since the list is "tight"),
        // then
        // 
        // ``` markdown
        // I need to buy
        // - new shoes
        // - a coat
        // - a plane ticket
        // ```
        // 
        // by itself should be a paragraph followed by a nested sublist.
        // 
        // Since it is well established Markdown practice to allow lists to
        // interrupt paragraphs inside list items, the [principle of
        // uniformity] requires us to allow this outside list items as
        // well.  ([reStructuredText](http://docutils.sourceforge.net/rst.html)
        // takes a different approach, requiring blank lines before lists
        // even inside other list items.)
        // 
        // In order to solve of unwanted lists in paragraphs with
        // hard-wrapped numerals, we allow only lists starting with `1` to
        // interrupt paragraphs.  Thus,        
        [Fact]
        public void Lists_Spec267_CommonMark()
        {
            // The following Markdown:
            //     The number of windows in my house is
            //     14.  The number of doors is 6.
            //
            // Should be rendered as:
            //     <p>The number of windows in my house is
            //     14.  The number of doors is 6.</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("The number of windows in my house is\n14.  The number of doors is 6.", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>The number of windows in my house is\n14.  The number of doors is 6.</p>");

            Assert.Equal(expectedResult, result);
        }

        // We may still get an unintended result in cases like        
        [Fact]
        public void Lists_Spec268_CommonMark()
        {
            // The following Markdown:
            //     The number of windows in my house is
            //     1.  The number of doors is 6.
            //
            // Should be rendered as:
            //     <p>The number of windows in my house is</p>
            //     <ol>
            //     <li>The number of doors is 6.</li>
            //     </ol>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("The number of windows in my house is\n1.  The number of doors is 6.", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>The number of windows in my house is</p>\n<ol>\n<li>The number of doors is 6.</li>\n</ol>");

            Assert.Equal(expectedResult, result);
        }

        // but this rule should prevent most spurious list captures.
        // 
        // There can be any number of blank lines between items:        
        [Fact]
        public void Lists_Spec269_CommonMark()
        {
            // The following Markdown:
            //     - foo
            //     
            //     - bar
            //     
            //     
            //     - baz
            //
            // Should be rendered as:
            //     <ul>
            //     <li>
            //     <p>foo</p>
            //     </li>
            //     <li>
            //     <p>bar</p>
            //     </li>
            //     <li>
            //     <p>baz</p>
            //     </li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- foo\n\n- bar\n\n\n- baz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>\n<p>foo</p>\n</li>\n<li>\n<p>bar</p>\n</li>\n<li>\n<p>baz</p>\n</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Lists_Spec270_CommonMark()
        {
            // The following Markdown:
            //     - foo
            //       - bar
            //         - baz
            //     
            //     
            //           bim
            //
            // Should be rendered as:
            //     <ul>
            //     <li>foo
            //     <ul>
            //     <li>bar
            //     <ul>
            //     <li>
            //     <p>baz</p>
            //     <p>bim</p>
            //     </li>
            //     </ul>
            //     </li>
            //     </ul>
            //     </li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- foo\n  - bar\n    - baz\n\n\n      bim", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>foo\n<ul>\n<li>bar\n<ul>\n<li>\n<p>baz</p>\n<p>bim</p>\n</li>\n</ul>\n</li>\n</ul>\n</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }

        // To separate consecutive lists of the same type, or to separate a
        // list from an indented code block that would otherwise be parsed
        // as a subparagraph of the final list item, you can insert a blank HTML
        // comment:        
        [Fact]
        public void Lists_Spec271_CommonMark()
        {
            // The following Markdown:
            //     - foo
            //     - bar
            //     
            //     <!-- -->
            //     
            //     - baz
            //     - bim
            //
            // Should be rendered as:
            //     <ul>
            //     <li>foo</li>
            //     <li>bar</li>
            //     </ul>
            //     <!-- -->
            //     <ul>
            //     <li>baz</li>
            //     <li>bim</li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- foo\n- bar\n\n<!-- -->\n\n- baz\n- bim", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>foo</li>\n<li>bar</li>\n</ul>\n<!-- -->\n<ul>\n<li>baz</li>\n<li>bim</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Lists_Spec272_CommonMark()
        {
            // The following Markdown:
            //     -   foo
            //     
            //         notcode
            //     
            //     -   foo
            //     
            //     <!-- -->
            //     
            //         code
            //
            // Should be rendered as:
            //     <ul>
            //     <li>
            //     <p>foo</p>
            //     <p>notcode</p>
            //     </li>
            //     <li>
            //     <p>foo</p>
            //     </li>
            //     </ul>
            //     <!-- -->
            //     <pre><code>code
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("-   foo\n\n    notcode\n\n-   foo\n\n<!-- -->\n\n    code", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>\n<p>foo</p>\n<p>notcode</p>\n</li>\n<li>\n<p>foo</p>\n</li>\n</ul>\n<!-- -->\n<pre><code>code\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }

        // List items need not be indented to the same level.  The following
        // list items will be treated as items at the same list level,
        // since none is indented enough to belong to the previous list
        // item:        
        [Fact]
        public void Lists_Spec273_CommonMark()
        {
            // The following Markdown:
            //     - a
            //      - b
            //       - c
            //        - d
            //         - e
            //        - f
            //       - g
            //      - h
            //     - i
            //
            // Should be rendered as:
            //     <ul>
            //     <li>a</li>
            //     <li>b</li>
            //     <li>c</li>
            //     <li>d</li>
            //     <li>e</li>
            //     <li>f</li>
            //     <li>g</li>
            //     <li>h</li>
            //     <li>i</li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- a\n - b\n  - c\n   - d\n    - e\n   - f\n  - g\n - h\n- i", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>a</li>\n<li>b</li>\n<li>c</li>\n<li>d</li>\n<li>e</li>\n<li>f</li>\n<li>g</li>\n<li>h</li>\n<li>i</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Lists_Spec274_CommonMark()
        {
            // The following Markdown:
            //     1. a
            //     
            //       2. b
            //     
            //         3. c
            //
            // Should be rendered as:
            //     <ol>
            //     <li>
            //     <p>a</p>
            //     </li>
            //     <li>
            //     <p>b</p>
            //     </li>
            //     <li>
            //     <p>c</p>
            //     </li>
            //     </ol>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("1. a\n\n  2. b\n\n    3. c", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ol>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n</li>\n<li>\n<p>c</p>\n</li>\n</ol>");

            Assert.Equal(expectedResult, result);
        }

        // This is a loose list, because there is a blank line between
        // two of the list items:        
        [Fact]
        public void Lists_Spec275_CommonMark()
        {
            // The following Markdown:
            //     - a
            //     - b
            //     
            //     - c
            //
            // Should be rendered as:
            //     <ul>
            //     <li>
            //     <p>a</p>
            //     </li>
            //     <li>
            //     <p>b</p>
            //     </li>
            //     <li>
            //     <p>c</p>
            //     </li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- a\n- b\n\n- c", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n</li>\n<li>\n<p>c</p>\n</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }

        // So is this, with a empty second item:        
        [Fact]
        public void Lists_Spec276_CommonMark()
        {
            // The following Markdown:
            //     * a
            //     *
            //     
            //     * c
            //
            // Should be rendered as:
            //     <ul>
            //     <li>
            //     <p>a</p>
            //     </li>
            //     <li></li>
            //     <li>
            //     <p>c</p>
            //     </li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("* a\n*\n\n* c", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>\n<p>a</p>\n</li>\n<li></li>\n<li>\n<p>c</p>\n</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }

        // These are loose lists, even though there is no space between the items,
        // because one of the items directly contains two block-level elements
        // with a blank line between them:        
        [Fact]
        public void Lists_Spec277_CommonMark()
        {
            // The following Markdown:
            //     - a
            //     - b
            //     
            //       c
            //     - d
            //
            // Should be rendered as:
            //     <ul>
            //     <li>
            //     <p>a</p>
            //     </li>
            //     <li>
            //     <p>b</p>
            //     <p>c</p>
            //     </li>
            //     <li>
            //     <p>d</p>
            //     </li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- a\n- b\n\n  c\n- d", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n<p>c</p>\n</li>\n<li>\n<p>d</p>\n</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Lists_Spec278_CommonMark()
        {
            // The following Markdown:
            //     - a
            //     - b
            //     
            //       [ref]: /url
            //     - d
            //
            // Should be rendered as:
            //     <ul>
            //     <li>
            //     <p>a</p>
            //     </li>
            //     <li>
            //     <p>b</p>
            //     </li>
            //     <li>
            //     <p>d</p>
            //     </li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- a\n- b\n\n  [ref]: /url\n- d", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n</li>\n<li>\n<p>d</p>\n</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }

        // This is a tight list, because the blank lines are in a code block:        
        [Fact]
        public void Lists_Spec279_CommonMark()
        {
            // The following Markdown:
            //     - a
            //     - ```
            //       b
            //     
            //     
            //       ```
            //     - c
            //
            // Should be rendered as:
            //     <ul>
            //     <li>a</li>
            //     <li>
            //     <pre><code>b
            //     
            //     
            //     </code></pre>
            //     </li>
            //     <li>c</li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- a\n- ```\n  b\n\n\n  ```\n- c", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>a</li>\n<li>\n<pre><code>b\n\n\n</code></pre>\n</li>\n<li>c</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }

        // This is a tight list, because the blank line is between two
        // paragraphs of a sublist.  So the sublist is loose while
        // the outer list is tight:        
        [Fact]
        public void Lists_Spec280_CommonMark()
        {
            // The following Markdown:
            //     - a
            //       - b
            //     
            //         c
            //     - d
            //
            // Should be rendered as:
            //     <ul>
            //     <li>a
            //     <ul>
            //     <li>
            //     <p>b</p>
            //     <p>c</p>
            //     </li>
            //     </ul>
            //     </li>
            //     <li>d</li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- a\n  - b\n\n    c\n- d", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>a\n<ul>\n<li>\n<p>b</p>\n<p>c</p>\n</li>\n</ul>\n</li>\n<li>d</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }

        // This is a tight list, because the blank line is inside the
        // block quote:        
        [Fact]
        public void Lists_Spec281_CommonMark()
        {
            // The following Markdown:
            //     * a
            //       > b
            //       >
            //     * c
            //
            // Should be rendered as:
            //     <ul>
            //     <li>a
            //     <blockquote>
            //     <p>b</p>
            //     </blockquote>
            //     </li>
            //     <li>c</li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("* a\n  > b\n  >\n* c", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>a\n<blockquote>\n<p>b</p>\n</blockquote>\n</li>\n<li>c</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }

        // This list is tight, because the consecutive block elements
        // are not separated by blank lines:        
        [Fact]
        public void Lists_Spec282_CommonMark()
        {
            // The following Markdown:
            //     - a
            //       > b
            //       ```
            //       c
            //       ```
            //     - d
            //
            // Should be rendered as:
            //     <ul>
            //     <li>a
            //     <blockquote>
            //     <p>b</p>
            //     </blockquote>
            //     <pre><code>c
            //     </code></pre>
            //     </li>
            //     <li>d</li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- a\n  > b\n  ```\n  c\n  ```\n- d", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>a\n<blockquote>\n<p>b</p>\n</blockquote>\n<pre><code>c\n</code></pre>\n</li>\n<li>d</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }

        // A single-paragraph list is tight:        
        [Fact]
        public void Lists_Spec283_CommonMark()
        {
            // The following Markdown:
            //     - a
            //
            // Should be rendered as:
            //     <ul>
            //     <li>a</li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- a", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>a</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Lists_Spec284_CommonMark()
        {
            // The following Markdown:
            //     - a
            //       - b
            //
            // Should be rendered as:
            //     <ul>
            //     <li>a
            //     <ul>
            //     <li>b</li>
            //     </ul>
            //     </li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- a\n  - b", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>a\n<ul>\n<li>b</li>\n</ul>\n</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }

        // This list is loose, because of the blank line between the
        // two block elements in the list item:        
        [Fact]
        public void Lists_Spec285_CommonMark()
        {
            // The following Markdown:
            //     1. ```
            //        foo
            //        ```
            //     
            //        bar
            //
            // Should be rendered as:
            //     <ol>
            //     <li>
            //     <pre><code>foo
            //     </code></pre>
            //     <p>bar</p>
            //     </li>
            //     </ol>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("1. ```\n   foo\n   ```\n\n   bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ol>\n<li>\n<pre><code>foo\n</code></pre>\n<p>bar</p>\n</li>\n</ol>");

            Assert.Equal(expectedResult, result);
        }

        // Here the outer list is loose, the inner list tight:        
        [Fact]
        public void Lists_Spec286_CommonMark()
        {
            // The following Markdown:
            //     * foo
            //       * bar
            //     
            //       baz
            //
            // Should be rendered as:
            //     <ul>
            //     <li>
            //     <p>foo</p>
            //     <ul>
            //     <li>bar</li>
            //     </ul>
            //     <p>baz</p>
            //     </li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("* foo\n  * bar\n\n  baz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>\n<p>foo</p>\n<ul>\n<li>bar</li>\n</ul>\n<p>baz</p>\n</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Lists_Spec287_CommonMark()
        {
            // The following Markdown:
            //     - a
            //       - b
            //       - c
            //     
            //     - d
            //       - e
            //       - f
            //
            // Should be rendered as:
            //     <ul>
            //     <li>
            //     <p>a</p>
            //     <ul>
            //     <li>b</li>
            //     <li>c</li>
            //     </ul>
            //     </li>
            //     <li>
            //     <p>d</p>
            //     <ul>
            //     <li>e</li>
            //     <li>f</li>
            //     </ul>
            //     </li>
            //     </ul>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("- a\n  - b\n  - c\n\n- d\n  - e\n  - f", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<ul>\n<li>\n<p>a</p>\n<ul>\n<li>b</li>\n<li>c</li>\n</ul>\n</li>\n<li>\n<p>d</p>\n<ul>\n<li>e</li>\n<li>f</li>\n</ul>\n</li>\n</ul>");

            Assert.Equal(expectedResult, result);
        }

        // # Inlines
        // 
        // Inlines are parsed sequentially from the beginning of the character
        // stream to the end (left to right, in left-to-right languages).
        // Thus, for example, in        
        [Fact]
        public void Lists_Spec288_CommonMark()
        {
            // The following Markdown:
            //     `hi`lo`
            //
            // Should be rendered as:
            //     <p><code>hi</code>lo`</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("`hi`lo`", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><code>hi</code>lo`</p>");

            Assert.Equal(expectedResult, result);
        }
    }

    // Any ASCII punctuation character may be backslash-escaped:  
    public class BackslashEscapesTests
    {
        
        [Fact]
        public void BackslashEscapes_Spec289_CommonMark()
        {
            // The following Markdown:
            //     \!\"\#\$\%\&\'\(\)\*\+\,\-\.\/\:\;\<\=\>\?\@\[\\\]\^\_\`\{\|\}\~
            //
            // Should be rendered as:
            //     <p>!&quot;#$%&amp;'()*+,-./:;&lt;=&gt;?@[\]^_`{|}~</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("\\!\\\"\\#\\$\\%\\&\\'\\(\\)\\*\\+\\,\\-\\.\\/\\:\\;\\<\\=\\>\\?\\@\\[\\\\\\]\\^\\_\\`\\{\\|\\}\\~", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>!&quot;#$%&amp;'()*+,-./:;&lt;=&gt;?@[\\]^_`{|}~</p>");

            Assert.Equal(expectedResult, result);
        }

        // Backslashes before other characters are treated as literal
        // backslashes:        
        [Fact]
        public void BackslashEscapes_Spec290_CommonMark()
        {
            // The following Markdown:
            //     \→\A\a\ \3\φ\«
            //
            // Should be rendered as:
            //     <p>\→\A\a\ \3\φ\«</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("\\\t\\A\\a\\ \\3\\φ\\«", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>\\\t\\A\\a\\ \\3\\φ\\«</p>");

            Assert.Equal(expectedResult, result);
        }

        // Escaped characters are treated as regular characters and do
        // not have their usual Markdown meanings:        
        [Fact]
        public void BackslashEscapes_Spec291_CommonMark()
        {
            // The following Markdown:
            //     \*not emphasized*
            //     \<br/> not a tag
            //     \[not a link](/foo)
            //     \`not code`
            //     1\. not a list
            //     \* not a list
            //     \# not a heading
            //     \[foo]: /url "not a reference"
            //
            // Should be rendered as:
            //     <p>*not emphasized*
            //     &lt;br/&gt; not a tag
            //     [not a link](/foo)
            //     `not code`
            //     1. not a list
            //     * not a list
            //     # not a heading
            //     [foo]: /url &quot;not a reference&quot;</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("\\*not emphasized*\n\\<br/> not a tag\n\\[not a link](/foo)\n\\`not code`\n1\\. not a list\n\\* not a list\n\\# not a heading\n\\[foo]: /url \"not a reference\"", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>*not emphasized*\n&lt;br/&gt; not a tag\n[not a link](/foo)\n`not code`\n1. not a list\n* not a list\n# not a heading\n[foo]: /url &quot;not a reference&quot;</p>");

            Assert.Equal(expectedResult, result);
        }

        // If a backslash is itself escaped, the following character is not:        
        [Fact]
        public void BackslashEscapes_Spec292_CommonMark()
        {
            // The following Markdown:
            //     \\*emphasis*
            //
            // Should be rendered as:
            //     <p>\<em>emphasis</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("\\\\*emphasis*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>\\<em>emphasis</em></p>");

            Assert.Equal(expectedResult, result);
        }

        // A backslash at the end of the line is a [hard line break]:        
        [Fact]
        public void BackslashEscapes_Spec293_CommonMark()
        {
            // The following Markdown:
            //     foo\
            //     bar
            //
            // Should be rendered as:
            //     <p>foo<br />
            //     bar</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo\\\nbar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo<br />\nbar</p>");

            Assert.Equal(expectedResult, result);
        }

        // Backslash escapes do not work in code blocks, code spans, autolinks, or
        // raw HTML:        
        [Fact]
        public void BackslashEscapes_Spec294_CommonMark()
        {
            // The following Markdown:
            //     `` \[\` ``
            //
            // Should be rendered as:
            //     <p><code>\[\`</code></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("`` \\[\\` ``", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><code>\\[\\`</code></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void BackslashEscapes_Spec295_CommonMark()
        {
            // The following Markdown:
            //         \[\]
            //
            // Should be rendered as:
            //     <pre><code>\[\]
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("    \\[\\]", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>\\[\\]\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void BackslashEscapes_Spec296_CommonMark()
        {
            // The following Markdown:
            //     ~~~
            //     \[\]
            //     ~~~
            //
            // Should be rendered as:
            //     <pre><code>\[\]
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("~~~\n\\[\\]\n~~~", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>\\[\\]\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void BackslashEscapes_Spec297_CommonMark()
        {
            // The following Markdown:
            //     <http://example.com?find=\*>
            //
            // Should be rendered as:
            //     <p><a href="http://example.com?find=%5C*">http://example.com?find=\*</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<http://example.com?find=\\*>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"http://example.com?find=%5C*\">http://example.com?find=\\*</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void BackslashEscapes_Spec298_CommonMark()
        {
            // The following Markdown:
            //     <a href="/bar\/)">
            //
            // Should be rendered as:
            //     <a href="/bar\/)">

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<a href=\"/bar\\/)\">", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<a href=\"/bar\\/)\">");

            Assert.Equal(expectedResult, result);
        }

        // But they work in all other contexts, including URLs and link titles,
        // link references, and [info strings] in [fenced code blocks]:        
        [Fact]
        public void BackslashEscapes_Spec299_CommonMark()
        {
            // The following Markdown:
            //     [foo](/bar\* "ti\*tle")
            //
            // Should be rendered as:
            //     <p><a href="/bar*" title="ti*tle">foo</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo](/bar\\* \"ti\\*tle\")", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/bar*\" title=\"ti*tle\">foo</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void BackslashEscapes_Spec300_CommonMark()
        {
            // The following Markdown:
            //     [foo]
            //     
            //     [foo]: /bar\* "ti\*tle"
            //
            // Should be rendered as:
            //     <p><a href="/bar*" title="ti*tle">foo</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo]\n\n[foo]: /bar\\* \"ti\\*tle\"", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/bar*\" title=\"ti*tle\">foo</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void BackslashEscapes_Spec301_CommonMark()
        {
            // The following Markdown:
            //     ``` foo\+bar
            //     foo
            //     ```
            //
            // Should be rendered as:
            //     <pre><code class="language-foo+bar">foo
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("``` foo\\+bar\nfoo\n```", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code class=\"language-foo+bar\">foo\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }
    }

    // All valid HTML entity references and numeric character
    // references, except those occuring in code blocks and code spans,
    // are recognized as such and treated as equivalent to the
    // corresponding Unicode characters.  Conforming CommonMark parsers
    // need not store information about whether a particular character
    // was represented in the source using a Unicode character or
    // an entity reference  
    public class EntityAndNumericCharacterReferencesTests
    {

        // [Entity references](@) consist of `&` + any of the valid
        // HTML5 entity names + `;`. The
        // document <https://html.spec.whatwg.org/multipage/entities.json>
        // is used as an authoritative source for the valid entity
        // references and their corresponding code points.        
        [Fact]
        public void EntityAndNumericCharacterReferences_Spec302_CommonMark()
        {
            // The following Markdown:
            //     &nbsp; &amp; &copy; &AElig; &Dcaron;
            //     &frac34; &HilbertSpace; &DifferentialD;
            //     &ClockwiseContourIntegral; &ngE;
            //
            // Should be rendered as:
            //     <p>  &amp; © Æ Ď
            //     ¾ ℋ ⅆ
            //     ∲ ≧̸</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("&nbsp; &amp; &copy; &AElig; &Dcaron;\n&frac34; &HilbertSpace; &DifferentialD;\n&ClockwiseContourIntegral; &ngE;", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>  &amp; © Æ Ď\n¾ ℋ ⅆ\n∲ ≧̸</p>");

            Assert.Equal(expectedResult, result);
        }

        // [Decimal numeric character
        // references](@)
        // consist of `&#` + a string of 1--8 arabic digits + `;`. A
        // numeric character reference is parsed as the corresponding
        // Unicode character. Invalid Unicode code points will be replaced by
        // the REPLACEMENT CHARACTER (`U+FFFD`).  For security reasons,
        // the code point `U+0000` will also be replaced by `U+FFFD`.        
        [Fact]
        public void EntityAndNumericCharacterReferences_Spec303_CommonMark()
        {
            // The following Markdown:
            //     &#35; &#1234; &#992; &#98765432; &#0;
            //
            // Should be rendered as:
            //     <p># Ӓ Ϡ � �</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("&#35; &#1234; &#992; &#98765432; &#0;", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p># Ӓ Ϡ � �</p>");

            Assert.Equal(expectedResult, result);
        }

        // [Hexadecimal numeric character
        // references](@) consist of `&#` +
        // either `X` or `x` + a string of 1-8 hexadecimal digits + `;`.
        // They too are parsed as the corresponding Unicode character (this
        // time specified with a hexadecimal numeral instead of decimal).        
        [Fact]
        public void EntityAndNumericCharacterReferences_Spec304_CommonMark()
        {
            // The following Markdown:
            //     &#X22; &#XD06; &#xcab;
            //
            // Should be rendered as:
            //     <p>&quot; ആ ಫ</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("&#X22; &#XD06; &#xcab;", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>&quot; ആ ಫ</p>");

            Assert.Equal(expectedResult, result);
        }

        // Here are some nonentities:        
        [Fact]
        public void EntityAndNumericCharacterReferences_Spec305_CommonMark()
        {
            // The following Markdown:
            //     &nbsp &x; &#; &#x;
            //     &ThisIsNotDefined; &hi?;
            //
            // Should be rendered as:
            //     <p>&amp;nbsp &amp;x; &amp;#; &amp;#x;
            //     &amp;ThisIsNotDefined; &amp;hi?;</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("&nbsp &x; &#; &#x;\n&ThisIsNotDefined; &hi?;", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>&amp;nbsp &amp;x; &amp;#; &amp;#x;\n&amp;ThisIsNotDefined; &amp;hi?;</p>");

            Assert.Equal(expectedResult, result);
        }

        // Although HTML5 does accept some entity references
        // without a trailing semicolon (such as `&copy`), these are not
        // recognized here, because it makes the grammar too ambiguous:        
        [Fact]
        public void EntityAndNumericCharacterReferences_Spec306_CommonMark()
        {
            // The following Markdown:
            //     &copy
            //
            // Should be rendered as:
            //     <p>&amp;copy</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("&copy", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>&amp;copy</p>");

            Assert.Equal(expectedResult, result);
        }

        // Strings that are not on the list of HTML5 named entities are not
        // recognized as entity references either:        
        [Fact]
        public void EntityAndNumericCharacterReferences_Spec307_CommonMark()
        {
            // The following Markdown:
            //     &MadeUpEntity;
            //
            // Should be rendered as:
            //     <p>&amp;MadeUpEntity;</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("&MadeUpEntity;", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>&amp;MadeUpEntity;</p>");

            Assert.Equal(expectedResult, result);
        }

        // Entity and numeric character references are recognized in any
        // context besides code spans or code blocks, including
        // URLs, [link titles], and [fenced code block][] [info strings]:        
        [Fact]
        public void EntityAndNumericCharacterReferences_Spec308_CommonMark()
        {
            // The following Markdown:
            //     <a href="&ouml;&ouml;.html">
            //
            // Should be rendered as:
            //     <a href="&ouml;&ouml;.html">

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<a href=\"&ouml;&ouml;.html\">", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<a href=\"&ouml;&ouml;.html\">");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EntityAndNumericCharacterReferences_Spec309_CommonMark()
        {
            // The following Markdown:
            //     [foo](/f&ouml;&ouml; "f&ouml;&ouml;")
            //
            // Should be rendered as:
            //     <p><a href="/f%C3%B6%C3%B6" title="föö">foo</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo](/f&ouml;&ouml; \"f&ouml;&ouml;\")", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/f%C3%B6%C3%B6\" title=\"föö\">foo</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EntityAndNumericCharacterReferences_Spec310_CommonMark()
        {
            // The following Markdown:
            //     [foo]
            //     
            //     [foo]: /f&ouml;&ouml; "f&ouml;&ouml;"
            //
            // Should be rendered as:
            //     <p><a href="/f%C3%B6%C3%B6" title="föö">foo</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo]\n\n[foo]: /f&ouml;&ouml; \"f&ouml;&ouml;\"", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/f%C3%B6%C3%B6\" title=\"föö\">foo</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EntityAndNumericCharacterReferences_Spec311_CommonMark()
        {
            // The following Markdown:
            //     ``` f&ouml;&ouml;
            //     foo
            //     ```
            //
            // Should be rendered as:
            //     <pre><code class="language-föö">foo
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("``` f&ouml;&ouml;\nfoo\n```", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code class=\"language-föö\">foo\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }

        // Entity and numeric character references are treated as literal
        // text in code spans and code blocks:        
        [Fact]
        public void EntityAndNumericCharacterReferences_Spec312_CommonMark()
        {
            // The following Markdown:
            //     `f&ouml;&ouml;`
            //
            // Should be rendered as:
            //     <p><code>f&amp;ouml;&amp;ouml;</code></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("`f&ouml;&ouml;`", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><code>f&amp;ouml;&amp;ouml;</code></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EntityAndNumericCharacterReferences_Spec313_CommonMark()
        {
            // The following Markdown:
            //         f&ouml;f&ouml;
            //
            // Should be rendered as:
            //     <pre><code>f&amp;ouml;f&amp;ouml;
            //     </code></pre>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("    f&ouml;f&ouml;", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<pre><code>f&amp;ouml;f&amp;ouml;\n</code></pre>");

            Assert.Equal(expectedResult, result);
        }
    }

    // A [backtick string](@)
    // is a string of one or more backtick characters (`` ` ``) that is neither
    // preceded nor followed by a backtick.
    // 
    // A [code span](@) begins with a backtick string and ends with
    // a backtick string of equal length.  The contents of the code span are
    // the characters between the two backtick strings, with leading and
    // trailing spaces and [line endings] removed, and
    // [whitespace] collapsed to single spaces  
    public class CodeSpansTests
    {

        // This is a simple code span:        
        [Fact]
        public void CodeSpans_Spec314_CommonMark()
        {
            // The following Markdown:
            //     `foo`
            //
            // Should be rendered as:
            //     <p><code>foo</code></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("`foo`", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><code>foo</code></p>");

            Assert.Equal(expectedResult, result);
        }

        // Here two backticks are used, because the code contains a backtick.
        // This example also illustrates stripping of leading and trailing spaces:        
        [Fact]
        public void CodeSpans_Spec315_CommonMark()
        {
            // The following Markdown:
            //     `` foo ` bar  ``
            //
            // Should be rendered as:
            //     <p><code>foo ` bar</code></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("`` foo ` bar  ``", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><code>foo ` bar</code></p>");

            Assert.Equal(expectedResult, result);
        }

        // This example shows the motivation for stripping leading and trailing
        // spaces:        
        [Fact]
        public void CodeSpans_Spec316_CommonMark()
        {
            // The following Markdown:
            //     ` `` `
            //
            // Should be rendered as:
            //     <p><code>``</code></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("` `` `", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><code>``</code></p>");

            Assert.Equal(expectedResult, result);
        }

        // [Line endings] are treated like spaces:        
        [Fact]
        public void CodeSpans_Spec317_CommonMark()
        {
            // The following Markdown:
            //     ``
            //     foo
            //     ``
            //
            // Should be rendered as:
            //     <p><code>foo</code></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("``\nfoo\n``", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><code>foo</code></p>");

            Assert.Equal(expectedResult, result);
        }

        // Interior spaces and [line endings] are collapsed into
        // single spaces, just as they would be by a browser:        
        [Fact]
        public void CodeSpans_Spec318_CommonMark()
        {
            // The following Markdown:
            //     `foo   bar
            //       baz`
            //
            // Should be rendered as:
            //     <p><code>foo bar baz</code></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("`foo   bar\n  baz`", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><code>foo bar baz</code></p>");

            Assert.Equal(expectedResult, result);
        }

        // Not all [Unicode whitespace] (for instance, non-breaking space) is
        // collapsed, however:        
        [Fact]
        public void CodeSpans_Spec319_CommonMark()
        {
            // The following Markdown:
            //     `a  b`
            //
            // Should be rendered as:
            //     <p><code>a  b</code></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("`a  b`", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><code>a  b</code></p>");

            Assert.Equal(expectedResult, result);
        }

        // Q: Why not just leave the spaces, since browsers will collapse them
        // anyway?  A:  Because we might be targeting a non-HTML format, and we
        // shouldn't rely on HTML-specific rendering assumptions.
        // 
        // (Existing implementations differ in their treatment of internal
        // spaces and [line endings].  Some, including `Markdown.pl` and
        // `showdown`, convert an internal [line ending] into a
        // `<br />` tag.  But this makes things difficult for those who like to
        // hard-wrap their paragraphs, since a line break in the midst of a code
        // span will cause an unintended line break in the output.  Others just
        // leave internal spaces as they are, which is fine if only HTML is being
        // targeted.)        
        [Fact]
        public void CodeSpans_Spec320_CommonMark()
        {
            // The following Markdown:
            //     `foo `` bar`
            //
            // Should be rendered as:
            //     <p><code>foo `` bar</code></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("`foo `` bar`", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><code>foo `` bar</code></p>");

            Assert.Equal(expectedResult, result);
        }

        // Note that backslash escapes do not work in code spans. All backslashes
        // are treated literally:        
        [Fact]
        public void CodeSpans_Spec321_CommonMark()
        {
            // The following Markdown:
            //     `foo\`bar`
            //
            // Should be rendered as:
            //     <p><code>foo\</code>bar`</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("`foo\\`bar`", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><code>foo\\</code>bar`</p>");

            Assert.Equal(expectedResult, result);
        }

        // Backslash escapes are never needed, because one can always choose a
        // string of *n* backtick characters as delimiters, where the code does
        // not contain any strings of exactly *n* backtick characters.
        // 
        // Code span backticks have higher precedence than any other inline
        // constructs except HTML tags and autolinks.  Thus, for example, this is
        // not parsed as emphasized text, since the second `*` is part of a code
        // span:        
        [Fact]
        public void CodeSpans_Spec322_CommonMark()
        {
            // The following Markdown:
            //     *foo`*`
            //
            // Should be rendered as:
            //     <p>*foo<code>*</code></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*foo`*`", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>*foo<code>*</code></p>");

            Assert.Equal(expectedResult, result);
        }

        // And this is not parsed as a link:        
        [Fact]
        public void CodeSpans_Spec323_CommonMark()
        {
            // The following Markdown:
            //     [not a `link](/foo`)
            //
            // Should be rendered as:
            //     <p>[not a <code>link](/foo</code>)</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[not a `link](/foo`)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[not a <code>link](/foo</code>)</p>");

            Assert.Equal(expectedResult, result);
        }

        // Code spans, HTML tags, and autolinks have the same precedence.
        // Thus, this is code:        
        [Fact]
        public void CodeSpans_Spec324_CommonMark()
        {
            // The following Markdown:
            //     `<a href="`">`
            //
            // Should be rendered as:
            //     <p><code>&lt;a href=&quot;</code>&quot;&gt;`</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("`<a href=\"`\">`", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><code>&lt;a href=&quot;</code>&quot;&gt;`</p>");

            Assert.Equal(expectedResult, result);
        }

        // But this is an HTML tag:        
        [Fact]
        public void CodeSpans_Spec325_CommonMark()
        {
            // The following Markdown:
            //     <a href="`">`
            //
            // Should be rendered as:
            //     <p><a href="`">`</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<a href=\"`\">`", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"`\">`</p>");

            Assert.Equal(expectedResult, result);
        }

        // And this is code:        
        [Fact]
        public void CodeSpans_Spec326_CommonMark()
        {
            // The following Markdown:
            //     `<http://foo.bar.`baz>`
            //
            // Should be rendered as:
            //     <p><code>&lt;http://foo.bar.</code>baz&gt;`</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("`<http://foo.bar.`baz>`", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><code>&lt;http://foo.bar.</code>baz&gt;`</p>");

            Assert.Equal(expectedResult, result);
        }

        // But this is an autolink:        
        [Fact]
        public void CodeSpans_Spec327_CommonMark()
        {
            // The following Markdown:
            //     <http://foo.bar.`baz>`
            //
            // Should be rendered as:
            //     <p><a href="http://foo.bar.%60baz">http://foo.bar.`baz</a>`</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<http://foo.bar.`baz>`", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"http://foo.bar.%60baz\">http://foo.bar.`baz</a>`</p>");

            Assert.Equal(expectedResult, result);
        }

        // When a backtick string is not closed by a matching backtick string,
        // we just have literal backticks:        
        [Fact]
        public void CodeSpans_Spec328_CommonMark()
        {
            // The following Markdown:
            //     ```foo``
            //
            // Should be rendered as:
            //     <p>```foo``</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("```foo``", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>```foo``</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void CodeSpans_Spec329_CommonMark()
        {
            // The following Markdown:
            //     `foo
            //
            // Should be rendered as:
            //     <p>`foo</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("`foo", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>`foo</p>");

            Assert.Equal(expectedResult, result);
        }

        // The following case also illustrates the need for opening and
        // closing backtick strings to be equal in length:        
        [Fact]
        public void CodeSpans_Spec330_CommonMark()
        {
            // The following Markdown:
            //     `foo``bar``
            //
            // Should be rendered as:
            //     <p>`foo<code>bar</code></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("`foo``bar``", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>`foo<code>bar</code></p>");

            Assert.Equal(expectedResult, result);
        }
    }

    // John Gruber's original [Markdown syntax
    // description](http://daringfireball.net/projects/markdown/syntax#em) says:
    // 
    // > Markdown treats asterisks (`*`) and underscores (`_`) as indicators of
    // > emphasis. Text wrapped with one `*` or `_` will be wrapped with an HTML
    // > `<em>` tag; double `*`'s or `_`'s will be wrapped with an HTML `<strong>`
    // > tag.
    // 
    // This is enough for most users, but these rules leave much undecided,
    // especially when it comes to nested emphasis.  The original
    // `Markdown.pl` test suite makes it clear that triple `***` and
    // `___` delimiters can be used for strong emphasis, and most
    // implementations have also allowed the following patterns:
    // 
    // ``` markdown
    // ***strong emph***
    // ***strong** in emph*
    // ***emph* in strong**
    // **in strong *emph***
    // *in emph **strong***
    // ```
    // 
    // The following patterns are less widely supported, but the intent
    // is clear and they are useful (especially in contexts like bibliography
    // entries):
    // 
    // ``` markdown
    // *emph *with emph* in it*
    // **strong **with strong** in it**
    // ```
    // 
    // Many implementations have also restricted intraword emphasis to
    // the `*` forms, to avoid unwanted emphasis in words containing
    // internal underscores.  (It is best practice to put these in code
    // spans, but users often do not.)
    // 
    // ``` markdown
    // internal emphasis: foo*bar*baz
    // no emphasis: foo_bar_baz
    // ```
    // 
    // The rules given below capture all of these patterns, while allowing
    // for efficient parsing strategies that do not backtrack.
    // 
    // First, some definitions.  A [delimiter run](@) is either
    // a sequence of one or more `*` characters that is not preceded or
    // followed by a non-backslash-escaped `*` character, or a sequence
    // of one or more `_` characters that is not preceded or followed by
    // a non-backslash-escaped `_` character.
    // 
    // A [left-flanking delimiter run](@) is
    // a [delimiter run] that is (a) not followed by [Unicode whitespace],
    // and (b) not followed by a [punctuation character], or
    // preceded by [Unicode whitespace] or a [punctuation character].
    // For purposes of this definition, the beginning and the end of
    // the line count as Unicode whitespace.
    // 
    // A [right-flanking delimiter run](@) is
    // a [delimiter run] that is (a) not preceded by [Unicode whitespace],
    // and (b) not preceded by a [punctuation character], or
    // followed by [Unicode whitespace] or a [punctuation character].
    // For purposes of this definition, the beginning and the end of
    // the line count as Unicode whitespace.
    // 
    // Here are some examples of delimiter runs.
    // 
    //   - left-flanking but not right-flanking:
    // 
    //     ```
    //     ***abc
    //       _abc
    //     **"abc"
    //      _"abc"
    //     ```
    // 
    //   - right-flanking but not left-flanking:
    // 
    //     ```
    //      abc***
    //      abc_
    //     "abc"**
    //     "abc"_
    //     ```
    // 
    //   - Both left and right-flanking:
    // 
    //     ```
    //      abc***def
    //     "abc"_"def"
    //     ```
    // 
    //   - Neither left nor right-flanking:
    // 
    //     ```
    //     abc *** def
    //     a _ b
    //     ```
    // 
    // (The idea of distinguishing left-flanking and right-flanking
    // delimiter runs based on the character before and the character
    // after comes from Roopesh Chander's
    // [vfmd](http://www.vfmd.org/vfmd-spec/specification/#procedure-for-identifying-emphasis-tags).
    // vfmd uses the terminology "emphasis indicator string" instead of "delimiter
    // run," and its rules for distinguishing left- and right-flanking runs
    // are a bit more complex than the ones given here.)
    // 
    // The following rules define emphasis and strong emphasis:
    // 
    // 1.  A single `*` character [can open emphasis](@)
    //     iff (if and only if) it is part of a [left-flanking delimiter run].
    // 
    // 2.  A single `_` character [can open emphasis] iff
    //     it is part of a [left-flanking delimiter run]
    //     and either (a) not part of a [right-flanking delimiter run]
    //     or (b) part of a [right-flanking delimiter run]
    //     preceded by punctuation.
    // 
    // 3.  A single `*` character [can close emphasis](@)
    //     iff it is part of a [right-flanking delimiter run].
    // 
    // 4.  A single `_` character [can close emphasis] iff
    //     it is part of a [right-flanking delimiter run]
    //     and either (a) not part of a [left-flanking delimiter run]
    //     or (b) part of a [left-flanking delimiter run]
    //     followed by punctuation.
    // 
    // 5.  A double `**` [can open strong emphasis](@)
    //     iff it is part of a [left-flanking delimiter run].
    // 
    // 6.  A double `__` [can open strong emphasis] iff
    //     it is part of a [left-flanking delimiter run]
    //     and either (a) not part of a [right-flanking delimiter run]
    //     or (b) part of a [right-flanking delimiter run]
    //     preceded by punctuation.
    // 
    // 7.  A double `**` [can close strong emphasis](@)
    //     iff it is part of a [right-flanking delimiter run].
    // 
    // 8.  A double `__` [can close strong emphasis] iff
    //     it is part of a [right-flanking delimiter run]
    //     and either (a) not part of a [left-flanking delimiter run]
    //     or (b) part of a [left-flanking delimiter run]
    //     followed by punctuation.
    // 
    // 9.  Emphasis begins with a delimiter that [can open emphasis] and ends
    //     with a delimiter that [can close emphasis], and that uses the same
    //     character (`_` or `*`) as the opening delimiter.  The
    //     opening and closing delimiters must belong to separate
    //     [delimiter runs].  If one of the delimiters can both
    //     open and close emphasis, then the sum of the lengths of the
    //     delimiter runs containing the opening and closing delimiters
    //     must not be a multiple of 3.
    // 
    // 10. Strong emphasis begins with a delimiter that
    //     [can open strong emphasis] and ends with a delimiter that
    //     [can close strong emphasis], and that uses the same character
    //     (`_` or `*`) as the opening delimiter.  The
    //     opening and closing delimiters must belong to separate
    //     [delimiter runs].  If one of the delimiters can both open
    //     and close strong emphasis, then the sum of the lengths of
    //     the delimiter runs containing the opening and closing
    //     delimiters must not be a multiple of 3.
    // 
    // 11. A literal `*` character cannot occur at the beginning or end of
    //     `*`-delimited emphasis or `**`-delimited strong emphasis, unless it
    //     is backslash-escaped.
    // 
    // 12. A literal `_` character cannot occur at the beginning or end of
    //     `_`-delimited emphasis or `__`-delimited strong emphasis, unless it
    //     is backslash-escaped.
    // 
    // Where rules 1--12 above are compatible with multiple parsings,
    // the following principles resolve ambiguity:
    // 
    // 13. The number of nestings should be minimized. Thus, for example,
    //     an interpretation `<strong>...</strong>` is always preferred to
    //     `<em><em>...</em></em>`.
    // 
    // 14. An interpretation `<em><strong>...</strong></em>` is always
    //     preferred to `<strong><em>...</em></strong>`.
    // 
    // 15. When two potential emphasis or strong emphasis spans overlap,
    //     so that the second begins before the first ends and ends after
    //     the first ends, the first takes precedence. Thus, for example,
    //     `*foo _bar* baz_` is parsed as `<em>foo _bar</em> baz_` rather
    //     than `*foo <em>bar* baz</em>`.
    // 
    // 16. When there are two potential emphasis or strong emphasis spans
    //     with the same closing delimiter, the shorter one (the one that
    //     opens later) takes precedence. Thus, for example,
    //     `**foo **bar baz**` is parsed as `**foo <strong>bar baz</strong>`
    //     rather than `<strong>foo **bar baz</strong>`.
    // 
    // 17. Inline code spans, links, images, and HTML tags group more tightly
    //     than emphasis.  So, when there is a choice between an interpretation
    //     that contains one of these elements and one that does not, the
    //     former always wins.  Thus, for example, `*[foo*](bar)` is
    //     parsed as `*<a href="bar">foo*</a>` rather than as
    //     `<em>[foo</em>](bar)`.
    // 
    // These rules can be illustrated through a series of examples  
    public class EmphasisAndStrongEmphasisTests
    {

        // Rule 1:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec331_CommonMark()
        {
            // The following Markdown:
            //     *foo bar*
            //
            // Should be rendered as:
            //     <p><em>foo bar</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*foo bar*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>foo bar</em></p>");

            Assert.Equal(expectedResult, result);
        }

        // This is not emphasis, because the opening `*` is followed by
        // whitespace, and hence not part of a [left-flanking delimiter run]:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec332_CommonMark()
        {
            // The following Markdown:
            //     a * foo bar*
            //
            // Should be rendered as:
            //     <p>a * foo bar*</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("a * foo bar*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>a * foo bar*</p>");

            Assert.Equal(expectedResult, result);
        }

        // This is not emphasis, because the opening `*` is preceded
        // by an alphanumeric and followed by punctuation, and hence
        // not part of a [left-flanking delimiter run]:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec333_CommonMark()
        {
            // The following Markdown:
            //     a*"foo"*
            //
            // Should be rendered as:
            //     <p>a*&quot;foo&quot;*</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("a*\"foo\"*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>a*&quot;foo&quot;*</p>");

            Assert.Equal(expectedResult, result);
        }

        // Unicode nonbreaking spaces count as whitespace, too:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec334_CommonMark()
        {
            // The following Markdown:
            //     * a *
            //
            // Should be rendered as:
            //     <p>* a *</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("* a *", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>* a *</p>");

            Assert.Equal(expectedResult, result);
        }

        // Intraword emphasis with `*` is permitted:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec335_CommonMark()
        {
            // The following Markdown:
            //     foo*bar*
            //
            // Should be rendered as:
            //     <p>foo<em>bar</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo*bar*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo<em>bar</em></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec336_CommonMark()
        {
            // The following Markdown:
            //     5*6*78
            //
            // Should be rendered as:
            //     <p>5<em>6</em>78</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("5*6*78", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>5<em>6</em>78</p>");

            Assert.Equal(expectedResult, result);
        }

        // Rule 2:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec337_CommonMark()
        {
            // The following Markdown:
            //     _foo bar_
            //
            // Should be rendered as:
            //     <p><em>foo bar</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("_foo bar_", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>foo bar</em></p>");

            Assert.Equal(expectedResult, result);
        }

        // This is not emphasis, because the opening `_` is followed by
        // whitespace:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec338_CommonMark()
        {
            // The following Markdown:
            //     _ foo bar_
            //
            // Should be rendered as:
            //     <p>_ foo bar_</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("_ foo bar_", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>_ foo bar_</p>");

            Assert.Equal(expectedResult, result);
        }

        // This is not emphasis, because the opening `_` is preceded
        // by an alphanumeric and followed by punctuation:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec339_CommonMark()
        {
            // The following Markdown:
            //     a_"foo"_
            //
            // Should be rendered as:
            //     <p>a_&quot;foo&quot;_</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("a_\"foo\"_", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>a_&quot;foo&quot;_</p>");

            Assert.Equal(expectedResult, result);
        }

        // Emphasis with `_` is not allowed inside words:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec340_CommonMark()
        {
            // The following Markdown:
            //     foo_bar_
            //
            // Should be rendered as:
            //     <p>foo_bar_</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo_bar_", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo_bar_</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec341_CommonMark()
        {
            // The following Markdown:
            //     5_6_78
            //
            // Should be rendered as:
            //     <p>5_6_78</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("5_6_78", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>5_6_78</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec342_CommonMark()
        {
            // The following Markdown:
            //     пристаням_стремятся_
            //
            // Should be rendered as:
            //     <p>пристаням_стремятся_</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("пристаням_стремятся_", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>пристаням_стремятся_</p>");

            Assert.Equal(expectedResult, result);
        }

        // Here `_` does not generate emphasis, because the first delimiter run
        // is right-flanking and the second left-flanking:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec343_CommonMark()
        {
            // The following Markdown:
            //     aa_"bb"_cc
            //
            // Should be rendered as:
            //     <p>aa_&quot;bb&quot;_cc</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("aa_\"bb\"_cc", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>aa_&quot;bb&quot;_cc</p>");

            Assert.Equal(expectedResult, result);
        }

        // This is emphasis, even though the opening delimiter is
        // both left- and right-flanking, because it is preceded by
        // punctuation:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec344_CommonMark()
        {
            // The following Markdown:
            //     foo-_(bar)_
            //
            // Should be rendered as:
            //     <p>foo-<em>(bar)</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo-_(bar)_", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo-<em>(bar)</em></p>");

            Assert.Equal(expectedResult, result);
        }

        // Rule 3:
        // 
        // This is not emphasis, because the closing delimiter does
        // not match the opening delimiter:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec345_CommonMark()
        {
            // The following Markdown:
            //     _foo*
            //
            // Should be rendered as:
            //     <p>_foo*</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("_foo*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>_foo*</p>");

            Assert.Equal(expectedResult, result);
        }

        // This is not emphasis, because the closing `*` is preceded by
        // whitespace:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec346_CommonMark()
        {
            // The following Markdown:
            //     *foo bar *
            //
            // Should be rendered as:
            //     <p>*foo bar *</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*foo bar *", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>*foo bar *</p>");

            Assert.Equal(expectedResult, result);
        }

        // A newline also counts as whitespace:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec347_CommonMark()
        {
            // The following Markdown:
            //     *foo bar
            //     *
            //
            // Should be rendered as:
            //     <p>*foo bar
            //     *</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*foo bar\n*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>*foo bar\n*</p>");

            Assert.Equal(expectedResult, result);
        }

        // This is not emphasis, because the second `*` is
        // preceded by punctuation and followed by an alphanumeric
        // (hence it is not part of a [right-flanking delimiter run]:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec348_CommonMark()
        {
            // The following Markdown:
            //     *(*foo)
            //
            // Should be rendered as:
            //     <p>*(*foo)</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*(*foo)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>*(*foo)</p>");

            Assert.Equal(expectedResult, result);
        }

        // The point of this restriction is more easily appreciated
        // with this example:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec349_CommonMark()
        {
            // The following Markdown:
            //     *(*foo*)*
            //
            // Should be rendered as:
            //     <p><em>(<em>foo</em>)</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*(*foo*)*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>(<em>foo</em>)</em></p>");

            Assert.Equal(expectedResult, result);
        }

        // Intraword emphasis with `*` is allowed:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec350_CommonMark()
        {
            // The following Markdown:
            //     *foo*bar
            //
            // Should be rendered as:
            //     <p><em>foo</em>bar</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*foo*bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>foo</em>bar</p>");

            Assert.Equal(expectedResult, result);
        }

        // Rule 4:
        // 
        // This is not emphasis, because the closing `_` is preceded by
        // whitespace:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec351_CommonMark()
        {
            // The following Markdown:
            //     _foo bar _
            //
            // Should be rendered as:
            //     <p>_foo bar _</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("_foo bar _", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>_foo bar _</p>");

            Assert.Equal(expectedResult, result);
        }

        // This is not emphasis, because the second `_` is
        // preceded by punctuation and followed by an alphanumeric:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec352_CommonMark()
        {
            // The following Markdown:
            //     _(_foo)
            //
            // Should be rendered as:
            //     <p>_(_foo)</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("_(_foo)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>_(_foo)</p>");

            Assert.Equal(expectedResult, result);
        }

        // This is emphasis within emphasis:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec353_CommonMark()
        {
            // The following Markdown:
            //     _(_foo_)_
            //
            // Should be rendered as:
            //     <p><em>(<em>foo</em>)</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("_(_foo_)_", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>(<em>foo</em>)</em></p>");

            Assert.Equal(expectedResult, result);
        }

        // Intraword emphasis is disallowed for `_`:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec354_CommonMark()
        {
            // The following Markdown:
            //     _foo_bar
            //
            // Should be rendered as:
            //     <p>_foo_bar</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("_foo_bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>_foo_bar</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec355_CommonMark()
        {
            // The following Markdown:
            //     _пристаням_стремятся
            //
            // Should be rendered as:
            //     <p>_пристаням_стремятся</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("_пристаням_стремятся", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>_пристаням_стремятся</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec356_CommonMark()
        {
            // The following Markdown:
            //     _foo_bar_baz_
            //
            // Should be rendered as:
            //     <p><em>foo_bar_baz</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("_foo_bar_baz_", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>foo_bar_baz</em></p>");

            Assert.Equal(expectedResult, result);
        }

        // This is emphasis, even though the closing delimiter is
        // both left- and right-flanking, because it is followed by
        // punctuation:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec357_CommonMark()
        {
            // The following Markdown:
            //     _(bar)_.
            //
            // Should be rendered as:
            //     <p><em>(bar)</em>.</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("_(bar)_.", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>(bar)</em>.</p>");

            Assert.Equal(expectedResult, result);
        }

        // Rule 5:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec358_CommonMark()
        {
            // The following Markdown:
            //     **foo bar**
            //
            // Should be rendered as:
            //     <p><strong>foo bar</strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("**foo bar**", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><strong>foo bar</strong></p>");

            Assert.Equal(expectedResult, result);
        }

        // This is not strong emphasis, because the opening delimiter is
        // followed by whitespace:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec359_CommonMark()
        {
            // The following Markdown:
            //     ** foo bar**
            //
            // Should be rendered as:
            //     <p>** foo bar**</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("** foo bar**", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>** foo bar**</p>");

            Assert.Equal(expectedResult, result);
        }

        // This is not strong emphasis, because the opening `**` is preceded
        // by an alphanumeric and followed by punctuation, and hence
        // not part of a [left-flanking delimiter run]:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec360_CommonMark()
        {
            // The following Markdown:
            //     a**"foo"**
            //
            // Should be rendered as:
            //     <p>a**&quot;foo&quot;**</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("a**\"foo\"**", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>a**&quot;foo&quot;**</p>");

            Assert.Equal(expectedResult, result);
        }

        // Intraword strong emphasis with `**` is permitted:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec361_CommonMark()
        {
            // The following Markdown:
            //     foo**bar**
            //
            // Should be rendered as:
            //     <p>foo<strong>bar</strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo**bar**", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo<strong>bar</strong></p>");

            Assert.Equal(expectedResult, result);
        }

        // Rule 6:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec362_CommonMark()
        {
            // The following Markdown:
            //     __foo bar__
            //
            // Should be rendered as:
            //     <p><strong>foo bar</strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("__foo bar__", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><strong>foo bar</strong></p>");

            Assert.Equal(expectedResult, result);
        }

        // This is not strong emphasis, because the opening delimiter is
        // followed by whitespace:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec363_CommonMark()
        {
            // The following Markdown:
            //     __ foo bar__
            //
            // Should be rendered as:
            //     <p>__ foo bar__</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("__ foo bar__", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>__ foo bar__</p>");

            Assert.Equal(expectedResult, result);
        }

        // A newline counts as whitespace:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec364_CommonMark()
        {
            // The following Markdown:
            //     __
            //     foo bar__
            //
            // Should be rendered as:
            //     <p>__
            //     foo bar__</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("__\nfoo bar__", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>__\nfoo bar__</p>");

            Assert.Equal(expectedResult, result);
        }

        // This is not strong emphasis, because the opening `__` is preceded
        // by an alphanumeric and followed by punctuation:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec365_CommonMark()
        {
            // The following Markdown:
            //     a__"foo"__
            //
            // Should be rendered as:
            //     <p>a__&quot;foo&quot;__</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("a__\"foo\"__", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>a__&quot;foo&quot;__</p>");

            Assert.Equal(expectedResult, result);
        }

        // Intraword strong emphasis is forbidden with `__`:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec366_CommonMark()
        {
            // The following Markdown:
            //     foo__bar__
            //
            // Should be rendered as:
            //     <p>foo__bar__</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo__bar__", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo__bar__</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec367_CommonMark()
        {
            // The following Markdown:
            //     5__6__78
            //
            // Should be rendered as:
            //     <p>5__6__78</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("5__6__78", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>5__6__78</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec368_CommonMark()
        {
            // The following Markdown:
            //     пристаням__стремятся__
            //
            // Should be rendered as:
            //     <p>пристаням__стремятся__</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("пристаням__стремятся__", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>пристаням__стремятся__</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec369_CommonMark()
        {
            // The following Markdown:
            //     __foo, __bar__, baz__
            //
            // Should be rendered as:
            //     <p><strong>foo, <strong>bar</strong>, baz</strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("__foo, __bar__, baz__", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><strong>foo, <strong>bar</strong>, baz</strong></p>");

            Assert.Equal(expectedResult, result);
        }

        // This is strong emphasis, even though the opening delimiter is
        // both left- and right-flanking, because it is preceded by
        // punctuation:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec370_CommonMark()
        {
            // The following Markdown:
            //     foo-__(bar)__
            //
            // Should be rendered as:
            //     <p>foo-<strong>(bar)</strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo-__(bar)__", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo-<strong>(bar)</strong></p>");

            Assert.Equal(expectedResult, result);
        }

        // Rule 7:
        // 
        // This is not strong emphasis, because the closing delimiter is preceded
        // by whitespace:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec371_CommonMark()
        {
            // The following Markdown:
            //     **foo bar **
            //
            // Should be rendered as:
            //     <p>**foo bar **</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("**foo bar **", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>**foo bar **</p>");

            Assert.Equal(expectedResult, result);
        }

        // (Nor can it be interpreted as an emphasized `*foo bar *`, because of
        // Rule 11.)
        // 
        // This is not strong emphasis, because the second `**` is
        // preceded by punctuation and followed by an alphanumeric:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec372_CommonMark()
        {
            // The following Markdown:
            //     **(**foo)
            //
            // Should be rendered as:
            //     <p>**(**foo)</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("**(**foo)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>**(**foo)</p>");

            Assert.Equal(expectedResult, result);
        }

        // The point of this restriction is more easily appreciated
        // with these examples:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec373_CommonMark()
        {
            // The following Markdown:
            //     *(**foo**)*
            //
            // Should be rendered as:
            //     <p><em>(<strong>foo</strong>)</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*(**foo**)*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>(<strong>foo</strong>)</em></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec374_CommonMark()
        {
            // The following Markdown:
            //     **Gomphocarpus (*Gomphocarpus physocarpus*, syn.
            //     *Asclepias physocarpa*)**
            //
            // Should be rendered as:
            //     <p><strong>Gomphocarpus (<em>Gomphocarpus physocarpus</em>, syn.
            //     <em>Asclepias physocarpa</em>)</strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("**Gomphocarpus (*Gomphocarpus physocarpus*, syn.\n*Asclepias physocarpa*)**", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><strong>Gomphocarpus (<em>Gomphocarpus physocarpus</em>, syn.\n<em>Asclepias physocarpa</em>)</strong></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec375_CommonMark()
        {
            // The following Markdown:
            //     **foo "*bar*" foo**
            //
            // Should be rendered as:
            //     <p><strong>foo &quot;<em>bar</em>&quot; foo</strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("**foo \"*bar*\" foo**", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><strong>foo &quot;<em>bar</em>&quot; foo</strong></p>");

            Assert.Equal(expectedResult, result);
        }

        // Intraword emphasis:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec376_CommonMark()
        {
            // The following Markdown:
            //     **foo**bar
            //
            // Should be rendered as:
            //     <p><strong>foo</strong>bar</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("**foo**bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><strong>foo</strong>bar</p>");

            Assert.Equal(expectedResult, result);
        }

        // Rule 8:
        // 
        // This is not strong emphasis, because the closing delimiter is
        // preceded by whitespace:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec377_CommonMark()
        {
            // The following Markdown:
            //     __foo bar __
            //
            // Should be rendered as:
            //     <p>__foo bar __</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("__foo bar __", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>__foo bar __</p>");

            Assert.Equal(expectedResult, result);
        }

        // This is not strong emphasis, because the second `__` is
        // preceded by punctuation and followed by an alphanumeric:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec378_CommonMark()
        {
            // The following Markdown:
            //     __(__foo)
            //
            // Should be rendered as:
            //     <p>__(__foo)</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("__(__foo)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>__(__foo)</p>");

            Assert.Equal(expectedResult, result);
        }

        // The point of this restriction is more easily appreciated
        // with this example:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec379_CommonMark()
        {
            // The following Markdown:
            //     _(__foo__)_
            //
            // Should be rendered as:
            //     <p><em>(<strong>foo</strong>)</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("_(__foo__)_", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>(<strong>foo</strong>)</em></p>");

            Assert.Equal(expectedResult, result);
        }

        // Intraword strong emphasis is forbidden with `__`:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec380_CommonMark()
        {
            // The following Markdown:
            //     __foo__bar
            //
            // Should be rendered as:
            //     <p>__foo__bar</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("__foo__bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>__foo__bar</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec381_CommonMark()
        {
            // The following Markdown:
            //     __пристаням__стремятся
            //
            // Should be rendered as:
            //     <p>__пристаням__стремятся</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("__пристаням__стремятся", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>__пристаням__стремятся</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec382_CommonMark()
        {
            // The following Markdown:
            //     __foo__bar__baz__
            //
            // Should be rendered as:
            //     <p><strong>foo__bar__baz</strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("__foo__bar__baz__", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><strong>foo__bar__baz</strong></p>");

            Assert.Equal(expectedResult, result);
        }

        // This is strong emphasis, even though the closing delimiter is
        // both left- and right-flanking, because it is followed by
        // punctuation:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec383_CommonMark()
        {
            // The following Markdown:
            //     __(bar)__.
            //
            // Should be rendered as:
            //     <p><strong>(bar)</strong>.</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("__(bar)__.", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><strong>(bar)</strong>.</p>");

            Assert.Equal(expectedResult, result);
        }

        // Rule 9:
        // 
        // Any nonempty sequence of inline elements can be the contents of an
        // emphasized span.        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec384_CommonMark()
        {
            // The following Markdown:
            //     *foo [bar](/url)*
            //
            // Should be rendered as:
            //     <p><em>foo <a href="/url">bar</a></em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*foo [bar](/url)*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>foo <a href=\"/url\">bar</a></em></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec385_CommonMark()
        {
            // The following Markdown:
            //     *foo
            //     bar*
            //
            // Should be rendered as:
            //     <p><em>foo
            //     bar</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*foo\nbar*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>foo\nbar</em></p>");

            Assert.Equal(expectedResult, result);
        }

        // In particular, emphasis and strong emphasis can be nested
        // inside emphasis:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec386_CommonMark()
        {
            // The following Markdown:
            //     _foo __bar__ baz_
            //
            // Should be rendered as:
            //     <p><em>foo <strong>bar</strong> baz</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("_foo __bar__ baz_", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>foo <strong>bar</strong> baz</em></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec387_CommonMark()
        {
            // The following Markdown:
            //     _foo _bar_ baz_
            //
            // Should be rendered as:
            //     <p><em>foo <em>bar</em> baz</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("_foo _bar_ baz_", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>foo <em>bar</em> baz</em></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec388_CommonMark()
        {
            // The following Markdown:
            //     __foo_ bar_
            //
            // Should be rendered as:
            //     <p><em><em>foo</em> bar</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("__foo_ bar_", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em><em>foo</em> bar</em></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec389_CommonMark()
        {
            // The following Markdown:
            //     *foo *bar**
            //
            // Should be rendered as:
            //     <p><em>foo <em>bar</em></em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*foo *bar**", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>foo <em>bar</em></em></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec390_CommonMark()
        {
            // The following Markdown:
            //     *foo **bar** baz*
            //
            // Should be rendered as:
            //     <p><em>foo <strong>bar</strong> baz</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*foo **bar** baz*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>foo <strong>bar</strong> baz</em></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec391_CommonMark()
        {
            // The following Markdown:
            //     *foo**bar**baz*
            //
            // Should be rendered as:
            //     <p><em>foo<strong>bar</strong>baz</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*foo**bar**baz*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>foo<strong>bar</strong>baz</em></p>");

            Assert.Equal(expectedResult, result);
        }

        // Note that in the preceding case, the interpretation
        // 
        // ``` markdown
        // <p><em>foo</em><em>bar<em></em>baz</em></p>
        // ```
        // 
        // 
        // is precluded by the condition that a delimiter that
        // can both open and close (like the `*` after `foo`)
        // cannot form emphasis if the sum of the lengths of
        // the delimiter runs containing the opening and
        // closing delimiters is a multiple of 3.
        // 
        // The same condition ensures that the following
        // cases are all strong emphasis nested inside
        // emphasis, even when the interior spaces are
        // omitted:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec392_CommonMark()
        {
            // The following Markdown:
            //     ***foo** bar*
            //
            // Should be rendered as:
            //     <p><em><strong>foo</strong> bar</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("***foo** bar*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em><strong>foo</strong> bar</em></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec393_CommonMark()
        {
            // The following Markdown:
            //     *foo **bar***
            //
            // Should be rendered as:
            //     <p><em>foo <strong>bar</strong></em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*foo **bar***", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>foo <strong>bar</strong></em></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec394_CommonMark()
        {
            // The following Markdown:
            //     *foo**bar***
            //
            // Should be rendered as:
            //     <p><em>foo<strong>bar</strong></em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*foo**bar***", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>foo<strong>bar</strong></em></p>");

            Assert.Equal(expectedResult, result);
        }

        // Indefinite levels of nesting are possible:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec395_CommonMark()
        {
            // The following Markdown:
            //     *foo **bar *baz* bim** bop*
            //
            // Should be rendered as:
            //     <p><em>foo <strong>bar <em>baz</em> bim</strong> bop</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*foo **bar *baz* bim** bop*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>foo <strong>bar <em>baz</em> bim</strong> bop</em></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec396_CommonMark()
        {
            // The following Markdown:
            //     *foo [*bar*](/url)*
            //
            // Should be rendered as:
            //     <p><em>foo <a href="/url"><em>bar</em></a></em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*foo [*bar*](/url)*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>foo <a href=\"/url\"><em>bar</em></a></em></p>");

            Assert.Equal(expectedResult, result);
        }

        // There can be no empty emphasis or strong emphasis:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec397_CommonMark()
        {
            // The following Markdown:
            //     ** is not an empty emphasis
            //
            // Should be rendered as:
            //     <p>** is not an empty emphasis</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("** is not an empty emphasis", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>** is not an empty emphasis</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec398_CommonMark()
        {
            // The following Markdown:
            //     **** is not an empty strong emphasis
            //
            // Should be rendered as:
            //     <p>**** is not an empty strong emphasis</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("**** is not an empty strong emphasis", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>**** is not an empty strong emphasis</p>");

            Assert.Equal(expectedResult, result);
        }

        // Rule 10:
        // 
        // Any nonempty sequence of inline elements can be the contents of an
        // strongly emphasized span.        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec399_CommonMark()
        {
            // The following Markdown:
            //     **foo [bar](/url)**
            //
            // Should be rendered as:
            //     <p><strong>foo <a href="/url">bar</a></strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("**foo [bar](/url)**", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><strong>foo <a href=\"/url\">bar</a></strong></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec400_CommonMark()
        {
            // The following Markdown:
            //     **foo
            //     bar**
            //
            // Should be rendered as:
            //     <p><strong>foo
            //     bar</strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("**foo\nbar**", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><strong>foo\nbar</strong></p>");

            Assert.Equal(expectedResult, result);
        }

        // In particular, emphasis and strong emphasis can be nested
        // inside strong emphasis:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec401_CommonMark()
        {
            // The following Markdown:
            //     __foo _bar_ baz__
            //
            // Should be rendered as:
            //     <p><strong>foo <em>bar</em> baz</strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("__foo _bar_ baz__", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><strong>foo <em>bar</em> baz</strong></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec402_CommonMark()
        {
            // The following Markdown:
            //     __foo __bar__ baz__
            //
            // Should be rendered as:
            //     <p><strong>foo <strong>bar</strong> baz</strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("__foo __bar__ baz__", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><strong>foo <strong>bar</strong> baz</strong></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec403_CommonMark()
        {
            // The following Markdown:
            //     ____foo__ bar__
            //
            // Should be rendered as:
            //     <p><strong><strong>foo</strong> bar</strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("____foo__ bar__", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><strong><strong>foo</strong> bar</strong></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec404_CommonMark()
        {
            // The following Markdown:
            //     **foo **bar****
            //
            // Should be rendered as:
            //     <p><strong>foo <strong>bar</strong></strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("**foo **bar****", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><strong>foo <strong>bar</strong></strong></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec405_CommonMark()
        {
            // The following Markdown:
            //     **foo *bar* baz**
            //
            // Should be rendered as:
            //     <p><strong>foo <em>bar</em> baz</strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("**foo *bar* baz**", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><strong>foo <em>bar</em> baz</strong></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec406_CommonMark()
        {
            // The following Markdown:
            //     **foo*bar*baz**
            //
            // Should be rendered as:
            //     <p><strong>foo<em>bar</em>baz</strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("**foo*bar*baz**", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><strong>foo<em>bar</em>baz</strong></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec407_CommonMark()
        {
            // The following Markdown:
            //     ***foo* bar**
            //
            // Should be rendered as:
            //     <p><strong><em>foo</em> bar</strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("***foo* bar**", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><strong><em>foo</em> bar</strong></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec408_CommonMark()
        {
            // The following Markdown:
            //     **foo *bar***
            //
            // Should be rendered as:
            //     <p><strong>foo <em>bar</em></strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("**foo *bar***", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><strong>foo <em>bar</em></strong></p>");

            Assert.Equal(expectedResult, result);
        }

        // Indefinite levels of nesting are possible:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec409_CommonMark()
        {
            // The following Markdown:
            //     **foo *bar **baz**
            //     bim* bop**
            //
            // Should be rendered as:
            //     <p><strong>foo <em>bar <strong>baz</strong>
            //     bim</em> bop</strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("**foo *bar **baz**\nbim* bop**", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><strong>foo <em>bar <strong>baz</strong>\nbim</em> bop</strong></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec410_CommonMark()
        {
            // The following Markdown:
            //     **foo [*bar*](/url)**
            //
            // Should be rendered as:
            //     <p><strong>foo <a href="/url"><em>bar</em></a></strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("**foo [*bar*](/url)**", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><strong>foo <a href=\"/url\"><em>bar</em></a></strong></p>");

            Assert.Equal(expectedResult, result);
        }

        // There can be no empty emphasis or strong emphasis:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec411_CommonMark()
        {
            // The following Markdown:
            //     __ is not an empty emphasis
            //
            // Should be rendered as:
            //     <p>__ is not an empty emphasis</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("__ is not an empty emphasis", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>__ is not an empty emphasis</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec412_CommonMark()
        {
            // The following Markdown:
            //     ____ is not an empty strong emphasis
            //
            // Should be rendered as:
            //     <p>____ is not an empty strong emphasis</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("____ is not an empty strong emphasis", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>____ is not an empty strong emphasis</p>");

            Assert.Equal(expectedResult, result);
        }

        // Rule 11:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec413_CommonMark()
        {
            // The following Markdown:
            //     foo ***
            //
            // Should be rendered as:
            //     <p>foo ***</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo ***", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo ***</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec414_CommonMark()
        {
            // The following Markdown:
            //     foo *\**
            //
            // Should be rendered as:
            //     <p>foo <em>*</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo *\\**", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo <em>*</em></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec415_CommonMark()
        {
            // The following Markdown:
            //     foo *_*
            //
            // Should be rendered as:
            //     <p>foo <em>_</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo *_*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo <em>_</em></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec416_CommonMark()
        {
            // The following Markdown:
            //     foo *****
            //
            // Should be rendered as:
            //     <p>foo *****</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo *****", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo *****</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec417_CommonMark()
        {
            // The following Markdown:
            //     foo **\***
            //
            // Should be rendered as:
            //     <p>foo <strong>*</strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo **\\***", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo <strong>*</strong></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec418_CommonMark()
        {
            // The following Markdown:
            //     foo **_**
            //
            // Should be rendered as:
            //     <p>foo <strong>_</strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo **_**", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo <strong>_</strong></p>");

            Assert.Equal(expectedResult, result);
        }

        // Note that when delimiters do not match evenly, Rule 11 determines
        // that the excess literal `*` characters will appear outside of the
        // emphasis, rather than inside it:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec419_CommonMark()
        {
            // The following Markdown:
            //     **foo*
            //
            // Should be rendered as:
            //     <p>*<em>foo</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("**foo*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>*<em>foo</em></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec420_CommonMark()
        {
            // The following Markdown:
            //     *foo**
            //
            // Should be rendered as:
            //     <p><em>foo</em>*</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*foo**", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>foo</em>*</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec421_CommonMark()
        {
            // The following Markdown:
            //     ***foo**
            //
            // Should be rendered as:
            //     <p>*<strong>foo</strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("***foo**", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>*<strong>foo</strong></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec422_CommonMark()
        {
            // The following Markdown:
            //     ****foo*
            //
            // Should be rendered as:
            //     <p>***<em>foo</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("****foo*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>***<em>foo</em></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec423_CommonMark()
        {
            // The following Markdown:
            //     **foo***
            //
            // Should be rendered as:
            //     <p><strong>foo</strong>*</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("**foo***", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><strong>foo</strong>*</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec424_CommonMark()
        {
            // The following Markdown:
            //     *foo****
            //
            // Should be rendered as:
            //     <p><em>foo</em>***</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*foo****", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>foo</em>***</p>");

            Assert.Equal(expectedResult, result);
        }

        // Rule 12:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec425_CommonMark()
        {
            // The following Markdown:
            //     foo ___
            //
            // Should be rendered as:
            //     <p>foo ___</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo ___", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo ___</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec426_CommonMark()
        {
            // The following Markdown:
            //     foo _\__
            //
            // Should be rendered as:
            //     <p>foo <em>_</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo _\\__", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo <em>_</em></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec427_CommonMark()
        {
            // The following Markdown:
            //     foo _*_
            //
            // Should be rendered as:
            //     <p>foo <em>*</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo _*_", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo <em>*</em></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec428_CommonMark()
        {
            // The following Markdown:
            //     foo _____
            //
            // Should be rendered as:
            //     <p>foo _____</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo _____", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo _____</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec429_CommonMark()
        {
            // The following Markdown:
            //     foo __\___
            //
            // Should be rendered as:
            //     <p>foo <strong>_</strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo __\\___", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo <strong>_</strong></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec430_CommonMark()
        {
            // The following Markdown:
            //     foo __*__
            //
            // Should be rendered as:
            //     <p>foo <strong>*</strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo __*__", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo <strong>*</strong></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec431_CommonMark()
        {
            // The following Markdown:
            //     __foo_
            //
            // Should be rendered as:
            //     <p>_<em>foo</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("__foo_", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>_<em>foo</em></p>");

            Assert.Equal(expectedResult, result);
        }

        // Note that when delimiters do not match evenly, Rule 12 determines
        // that the excess literal `_` characters will appear outside of the
        // emphasis, rather than inside it:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec432_CommonMark()
        {
            // The following Markdown:
            //     _foo__
            //
            // Should be rendered as:
            //     <p><em>foo</em>_</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("_foo__", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>foo</em>_</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec433_CommonMark()
        {
            // The following Markdown:
            //     ___foo__
            //
            // Should be rendered as:
            //     <p>_<strong>foo</strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("___foo__", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>_<strong>foo</strong></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec434_CommonMark()
        {
            // The following Markdown:
            //     ____foo_
            //
            // Should be rendered as:
            //     <p>___<em>foo</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("____foo_", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>___<em>foo</em></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec435_CommonMark()
        {
            // The following Markdown:
            //     __foo___
            //
            // Should be rendered as:
            //     <p><strong>foo</strong>_</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("__foo___", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><strong>foo</strong>_</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec436_CommonMark()
        {
            // The following Markdown:
            //     _foo____
            //
            // Should be rendered as:
            //     <p><em>foo</em>___</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("_foo____", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>foo</em>___</p>");

            Assert.Equal(expectedResult, result);
        }

        // Rule 13 implies that if you want emphasis nested directly inside
        // emphasis, you must use different delimiters:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec437_CommonMark()
        {
            // The following Markdown:
            //     **foo**
            //
            // Should be rendered as:
            //     <p><strong>foo</strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("**foo**", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><strong>foo</strong></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec438_CommonMark()
        {
            // The following Markdown:
            //     *_foo_*
            //
            // Should be rendered as:
            //     <p><em><em>foo</em></em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*_foo_*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em><em>foo</em></em></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec439_CommonMark()
        {
            // The following Markdown:
            //     __foo__
            //
            // Should be rendered as:
            //     <p><strong>foo</strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("__foo__", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><strong>foo</strong></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec440_CommonMark()
        {
            // The following Markdown:
            //     _*foo*_
            //
            // Should be rendered as:
            //     <p><em><em>foo</em></em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("_*foo*_", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em><em>foo</em></em></p>");

            Assert.Equal(expectedResult, result);
        }

        // However, strong emphasis within strong emphasis is possible without
        // switching delimiters:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec441_CommonMark()
        {
            // The following Markdown:
            //     ****foo****
            //
            // Should be rendered as:
            //     <p><strong><strong>foo</strong></strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("****foo****", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><strong><strong>foo</strong></strong></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec442_CommonMark()
        {
            // The following Markdown:
            //     ____foo____
            //
            // Should be rendered as:
            //     <p><strong><strong>foo</strong></strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("____foo____", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><strong><strong>foo</strong></strong></p>");

            Assert.Equal(expectedResult, result);
        }

        // Rule 13 can be applied to arbitrarily long sequences of
        // delimiters:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec443_CommonMark()
        {
            // The following Markdown:
            //     ******foo******
            //
            // Should be rendered as:
            //     <p><strong><strong><strong>foo</strong></strong></strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("******foo******", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><strong><strong><strong>foo</strong></strong></strong></p>");

            Assert.Equal(expectedResult, result);
        }

        // Rule 14:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec444_CommonMark()
        {
            // The following Markdown:
            //     ***foo***
            //
            // Should be rendered as:
            //     <p><em><strong>foo</strong></em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("***foo***", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em><strong>foo</strong></em></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec445_CommonMark()
        {
            // The following Markdown:
            //     _____foo_____
            //
            // Should be rendered as:
            //     <p><em><strong><strong>foo</strong></strong></em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("_____foo_____", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em><strong><strong>foo</strong></strong></em></p>");

            Assert.Equal(expectedResult, result);
        }

        // Rule 15:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec446_CommonMark()
        {
            // The following Markdown:
            //     *foo _bar* baz_
            //
            // Should be rendered as:
            //     <p><em>foo _bar</em> baz_</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*foo _bar* baz_", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>foo _bar</em> baz_</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec447_CommonMark()
        {
            // The following Markdown:
            //     *foo __bar *baz bim__ bam*
            //
            // Should be rendered as:
            //     <p><em>foo <strong>bar *baz bim</strong> bam</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*foo __bar *baz bim__ bam*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>foo <strong>bar *baz bim</strong> bam</em></p>");

            Assert.Equal(expectedResult, result);
        }

        // Rule 16:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec448_CommonMark()
        {
            // The following Markdown:
            //     **foo **bar baz**
            //
            // Should be rendered as:
            //     <p>**foo <strong>bar baz</strong></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("**foo **bar baz**", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>**foo <strong>bar baz</strong></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec449_CommonMark()
        {
            // The following Markdown:
            //     *foo *bar baz*
            //
            // Should be rendered as:
            //     <p>*foo <em>bar baz</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*foo *bar baz*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>*foo <em>bar baz</em></p>");

            Assert.Equal(expectedResult, result);
        }

        // Rule 17:        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec450_CommonMark()
        {
            // The following Markdown:
            //     *[bar*](/url)
            //
            // Should be rendered as:
            //     <p>*<a href="/url">bar*</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*[bar*](/url)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>*<a href=\"/url\">bar*</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec451_CommonMark()
        {
            // The following Markdown:
            //     _foo [bar_](/url)
            //
            // Should be rendered as:
            //     <p>_foo <a href="/url">bar_</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("_foo [bar_](/url)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>_foo <a href=\"/url\">bar_</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec452_CommonMark()
        {
            // The following Markdown:
            //     *<img src="foo" title="*"/>
            //
            // Should be rendered as:
            //     <p>*<img src="foo" title="*"/></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*<img src=\"foo\" title=\"*\"/>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>*<img src=\"foo\" title=\"*\"/></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec453_CommonMark()
        {
            // The following Markdown:
            //     **<a href="**">
            //
            // Should be rendered as:
            //     <p>**<a href="**"></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("**<a href=\"**\">", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>**<a href=\"**\"></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec454_CommonMark()
        {
            // The following Markdown:
            //     __<a href="__">
            //
            // Should be rendered as:
            //     <p>__<a href="__"></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("__<a href=\"__\">", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>__<a href=\"__\"></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec455_CommonMark()
        {
            // The following Markdown:
            //     *a `*`*
            //
            // Should be rendered as:
            //     <p><em>a <code>*</code></em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*a `*`*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>a <code>*</code></em></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec456_CommonMark()
        {
            // The following Markdown:
            //     _a `_`_
            //
            // Should be rendered as:
            //     <p><em>a <code>_</code></em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("_a `_`_", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>a <code>_</code></em></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec457_CommonMark()
        {
            // The following Markdown:
            //     **a<http://foo.bar/?q=**>
            //
            // Should be rendered as:
            //     <p>**a<a href="http://foo.bar/?q=**">http://foo.bar/?q=**</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("**a<http://foo.bar/?q=**>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>**a<a href=\"http://foo.bar/?q=**\">http://foo.bar/?q=**</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec458_CommonMark()
        {
            // The following Markdown:
            //     __a<http://foo.bar/?q=__>
            //
            // Should be rendered as:
            //     <p>__a<a href="http://foo.bar/?q=__">http://foo.bar/?q=__</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("__a<http://foo.bar/?q=__>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>__a<a href=\"http://foo.bar/?q=__\">http://foo.bar/?q=__</a></p>");

            Assert.Equal(expectedResult, result);
        }
    }

    // A link contains [link text] (the visible text), a [link destination]
    // (the URI that is the link destination), and optionally a [link title].
    // There are two basic kinds of links in Markdown.  In [inline links] the
    // destination and title are given immediately after the link text.  In
    // [reference links] the destination and title are defined elsewhere in
    // the document.
    // 
    // A [link text](@) consists of a sequence of zero or more
    // inline elements enclosed by square brackets (`[` and `]`).  The
    // following rules apply:
    // 
    // - Links may not contain other links, at any level of nesting. If
    //   multiple otherwise valid link definitions appear nested inside each
    //   other, the inner-most definition is used.
    // 
    // - Brackets are allowed in the [link text] only if (a) they
    //   are backslash-escaped or (b) they appear as a matched pair of brackets,
    //   with an open bracket `[`, a sequence of zero or more inlines, and
    //   a close bracket `]`.
    // 
    // - Backtick [code spans], [autolinks], and raw [HTML tags] bind more tightly
    //   than the brackets in link text.  Thus, for example,
    //   `` [foo`]` `` could not be a link text, since the second `]`
    //   is part of a code span.
    // 
    // - The brackets in link text bind more tightly than markers for
    //   [emphasis and strong emphasis]. Thus, for example, `*[foo*](url)` is a link.
    // 
    // A [link destination](@) consists of either
    // 
    // - a sequence of zero or more characters between an opening `<` and a
    //   closing `>` that contains no spaces, line breaks, or unescaped
    //   `<` or `>` characters, or
    // 
    // - a nonempty sequence of characters that does not include
    //   ASCII space or control characters, and includes parentheses
    //   only if (a) they are backslash-escaped or (b) they are part of
    //   a balanced pair of unescaped parentheses.  (Implementations
    //   may impose limits on parentheses nesting to avoid performance
    //   issues, but at least three levels of nesting should be supported.)
    // 
    // A [link title](@)  consists of either
    // 
    // - a sequence of zero or more characters between straight double-quote
    //   characters (`"`), including a `"` character only if it is
    //   backslash-escaped, or
    // 
    // - a sequence of zero or more characters between straight single-quote
    //   characters (`'`), including a `'` character only if it is
    //   backslash-escaped, or
    // 
    // - a sequence of zero or more characters between matching parentheses
    //   (`(...)`), including a `)` character only if it is backslash-escaped.
    // 
    // Although [link titles] may span multiple lines, they may not contain
    // a [blank line].
    // 
    // An [inline link](@) consists of a [link text] followed immediately
    // by a left parenthesis `(`, optional [whitespace], an optional
    // [link destination], an optional [link title] separated from the link
    // destination by [whitespace], optional [whitespace], and a right
    // parenthesis `)`. The link's text consists of the inlines contained
    // in the [link text] (excluding the enclosing square brackets).
    // The link's URI consists of the link destination, excluding enclosing
    // `<...>` if present, with backslash-escapes in effect as described
    // above.  The link's title consists of the link title, excluding its
    // enclosing delimiters, with backslash-escapes in effect as described
    // above  
    public class LinksTests
    {

        // Here is a simple inline link:        
        [Fact]
        public void Links_Spec459_CommonMark()
        {
            // The following Markdown:
            //     [link](/uri "title")
            //
            // Should be rendered as:
            //     <p><a href="/uri" title="title">link</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link](/uri \"title\")", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/uri\" title=\"title\">link</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // The title may be omitted:        
        [Fact]
        public void Links_Spec460_CommonMark()
        {
            // The following Markdown:
            //     [link](/uri)
            //
            // Should be rendered as:
            //     <p><a href="/uri">link</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link](/uri)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/uri\">link</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // Both the title and the destination may be omitted:        
        [Fact]
        public void Links_Spec461_CommonMark()
        {
            // The following Markdown:
            //     [link]()
            //
            // Should be rendered as:
            //     <p><a href="">link</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link]()", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"\">link</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec462_CommonMark()
        {
            // The following Markdown:
            //     [link](<>)
            //
            // Should be rendered as:
            //     <p><a href="">link</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link](<>)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"\">link</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // The destination cannot contain spaces or line breaks,
        // even if enclosed in pointy brackets:        
        [Fact]
        public void Links_Spec463_CommonMark()
        {
            // The following Markdown:
            //     [link](/my uri)
            //
            // Should be rendered as:
            //     <p>[link](/my uri)</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link](/my uri)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[link](/my uri)</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec464_CommonMark()
        {
            // The following Markdown:
            //     [link](</my uri>)
            //
            // Should be rendered as:
            //     <p>[link](&lt;/my uri&gt;)</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link](</my uri>)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[link](&lt;/my uri&gt;)</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec465_CommonMark()
        {
            // The following Markdown:
            //     [link](foo
            //     bar)
            //
            // Should be rendered as:
            //     <p>[link](foo
            //     bar)</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link](foo\nbar)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[link](foo\nbar)</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec466_CommonMark()
        {
            // The following Markdown:
            //     [link](<foo
            //     bar>)
            //
            // Should be rendered as:
            //     <p>[link](<foo
            //     bar>)</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link](<foo\nbar>)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[link](<foo\nbar>)</p>");

            Assert.Equal(expectedResult, result);
        }

        // Parentheses inside the link destination may be escaped:        
        [Fact]
        public void Links_Spec467_CommonMark()
        {
            // The following Markdown:
            //     [link](\(foo\))
            //
            // Should be rendered as:
            //     <p><a href="(foo)">link</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link](\\(foo\\))", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"(foo)\">link</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // Any number of parentheses are allowed without escaping, as long as they are
        // balanced:        
        [Fact]
        public void Links_Spec468_CommonMark()
        {
            // The following Markdown:
            //     [link](foo(and(bar)))
            //
            // Should be rendered as:
            //     <p><a href="foo(and(bar))">link</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link](foo(and(bar)))", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"foo(and(bar))\">link</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // However, if you have unbalanced parentheses, you need to escape or use the
        // `<...>` form:        
        [Fact]
        public void Links_Spec469_CommonMark()
        {
            // The following Markdown:
            //     [link](foo\(and\(bar\))
            //
            // Should be rendered as:
            //     <p><a href="foo(and(bar)">link</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link](foo\\(and\\(bar\\))", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"foo(and(bar)\">link</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec470_CommonMark()
        {
            // The following Markdown:
            //     [link](<foo(and(bar)>)
            //
            // Should be rendered as:
            //     <p><a href="foo(and(bar)">link</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link](<foo(and(bar)>)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"foo(and(bar)\">link</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // Parentheses and other symbols can also be escaped, as usual
        // in Markdown:        
        [Fact]
        public void Links_Spec471_CommonMark()
        {
            // The following Markdown:
            //     [link](foo\)\:)
            //
            // Should be rendered as:
            //     <p><a href="foo):">link</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link](foo\\)\\:)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"foo):\">link</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // A link can contain fragment identifiers and queries:        
        [Fact]
        public void Links_Spec472_CommonMark()
        {
            // The following Markdown:
            //     [link](#fragment)
            //     
            //     [link](http://example.com#fragment)
            //     
            //     [link](http://example.com?foo=3#frag)
            //
            // Should be rendered as:
            //     <p><a href="#fragment">link</a></p>
            //     <p><a href="http://example.com#fragment">link</a></p>
            //     <p><a href="http://example.com?foo=3#frag">link</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link](#fragment)\n\n[link](http://example.com#fragment)\n\n[link](http://example.com?foo=3#frag)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"#fragment\">link</a></p>\n<p><a href=\"http://example.com#fragment\">link</a></p>\n<p><a href=\"http://example.com?foo=3#frag\">link</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // Note that a backslash before a non-escapable character is
        // just a backslash:        
        [Fact]
        public void Links_Spec473_CommonMark()
        {
            // The following Markdown:
            //     [link](foo\bar)
            //
            // Should be rendered as:
            //     <p><a href="foo%5Cbar">link</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link](foo\\bar)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"foo%5Cbar\">link</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // URL-escaping should be left alone inside the destination, as all
        // URL-escaped characters are also valid URL characters. Entity and
        // numerical character references in the destination will be parsed
        // into the corresponding Unicode code points, as usual.  These may
        // be optionally URL-escaped when written as HTML, but this spec
        // does not enforce any particular policy for rendering URLs in
        // HTML or other formats.  Renderers may make different decisions
        // about how to escape or normalize URLs in the output.        
        [Fact]
        public void Links_Spec474_CommonMark()
        {
            // The following Markdown:
            //     [link](foo%20b&auml;)
            //
            // Should be rendered as:
            //     <p><a href="foo%20b%C3%A4">link</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link](foo%20b&auml;)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"foo%20b%C3%A4\">link</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // Note that, because titles can often be parsed as destinations,
        // if you try to omit the destination and keep the title, you'll
        // get unexpected results:        
        [Fact]
        public void Links_Spec475_CommonMark()
        {
            // The following Markdown:
            //     [link]("title")
            //
            // Should be rendered as:
            //     <p><a href="%22title%22">link</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link](\"title\")", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"%22title%22\">link</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // Titles may be in single quotes, double quotes, or parentheses:        
        [Fact]
        public void Links_Spec476_CommonMark()
        {
            // The following Markdown:
            //     [link](/url "title")
            //     [link](/url 'title')
            //     [link](/url (title))
            //
            // Should be rendered as:
            //     <p><a href="/url" title="title">link</a>
            //     <a href="/url" title="title">link</a>
            //     <a href="/url" title="title">link</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link](/url \"title\")\n[link](/url 'title')\n[link](/url (title))", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/url\" title=\"title\">link</a>\n<a href=\"/url\" title=\"title\">link</a>\n<a href=\"/url\" title=\"title\">link</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // Backslash escapes and entity and numeric character references
        // may be used in titles:        
        [Fact]
        public void Links_Spec477_CommonMark()
        {
            // The following Markdown:
            //     [link](/url "title \"&quot;")
            //
            // Should be rendered as:
            //     <p><a href="/url" title="title &quot;&quot;">link</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link](/url \"title \\\"&quot;\")", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/url\" title=\"title &quot;&quot;\">link</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // Titles must be separated from the link using a [whitespace].
        // Other [Unicode whitespace] like non-breaking space doesn't work.        
        [Fact]
        public void Links_Spec478_CommonMark()
        {
            // The following Markdown:
            //     [link](/url "title")
            //
            // Should be rendered as:
            //     <p><a href="/url%C2%A0%22title%22">link</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link](/url \"title\")", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/url%C2%A0%22title%22\">link</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // Nested balanced quotes are not allowed without escaping:        
        [Fact]
        public void Links_Spec479_CommonMark()
        {
            // The following Markdown:
            //     [link](/url "title "and" title")
            //
            // Should be rendered as:
            //     <p>[link](/url &quot;title &quot;and&quot; title&quot;)</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link](/url \"title \"and\" title\")", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[link](/url &quot;title &quot;and&quot; title&quot;)</p>");

            Assert.Equal(expectedResult, result);
        }

        // But it is easy to work around this by using a different quote type:        
        [Fact]
        public void Links_Spec480_CommonMark()
        {
            // The following Markdown:
            //     [link](/url 'title "and" title')
            //
            // Should be rendered as:
            //     <p><a href="/url" title="title &quot;and&quot; title">link</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link](/url 'title \"and\" title')", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/url\" title=\"title &quot;and&quot; title\">link</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // (Note:  `Markdown.pl` did allow double quotes inside a double-quoted
        // title, and its test suite included a test demonstrating this.
        // But it is hard to see a good rationale for the extra complexity this
        // brings, since there are already many ways---backslash escaping,
        // entity and numeric character references, or using a different
        // quote type for the enclosing title---to write titles containing
        // double quotes.  `Markdown.pl`'s handling of titles has a number
        // of other strange features.  For example, it allows single-quoted
        // titles in inline links, but not reference links.  And, in
        // reference links but not inline links, it allows a title to begin
        // with `"` and end with `)`.  `Markdown.pl` 1.0.1 even allows
        // titles with no closing quotation mark, though 1.0.2b8 does not.
        // It seems preferable to adopt a simple, rational rule that works
        // the same way in inline links and link reference definitions.)
        // 
        // [Whitespace] is allowed around the destination and title:        
        [Fact]
        public void Links_Spec481_CommonMark()
        {
            // The following Markdown:
            //     [link](   /uri
            //       "title"  )
            //
            // Should be rendered as:
            //     <p><a href="/uri" title="title">link</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link](   /uri\n  \"title\"  )", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/uri\" title=\"title\">link</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // But it is not allowed between the link text and the
        // following parenthesis:        
        [Fact]
        public void Links_Spec482_CommonMark()
        {
            // The following Markdown:
            //     [link] (/uri)
            //
            // Should be rendered as:
            //     <p>[link] (/uri)</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link] (/uri)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[link] (/uri)</p>");

            Assert.Equal(expectedResult, result);
        }

        // The link text may contain balanced brackets, but not unbalanced ones,
        // unless they are escaped:        
        [Fact]
        public void Links_Spec483_CommonMark()
        {
            // The following Markdown:
            //     [link [foo [bar]]](/uri)
            //
            // Should be rendered as:
            //     <p><a href="/uri">link [foo [bar]]</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link [foo [bar]]](/uri)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/uri\">link [foo [bar]]</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec484_CommonMark()
        {
            // The following Markdown:
            //     [link] bar](/uri)
            //
            // Should be rendered as:
            //     <p>[link] bar](/uri)</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link] bar](/uri)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[link] bar](/uri)</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec485_CommonMark()
        {
            // The following Markdown:
            //     [link [bar](/uri)
            //
            // Should be rendered as:
            //     <p>[link <a href="/uri">bar</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link [bar](/uri)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[link <a href=\"/uri\">bar</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec486_CommonMark()
        {
            // The following Markdown:
            //     [link \[bar](/uri)
            //
            // Should be rendered as:
            //     <p><a href="/uri">link [bar</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link \\[bar](/uri)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/uri\">link [bar</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // The link text may contain inline content:        
        [Fact]
        public void Links_Spec487_CommonMark()
        {
            // The following Markdown:
            //     [link *foo **bar** `#`*](/uri)
            //
            // Should be rendered as:
            //     <p><a href="/uri">link <em>foo <strong>bar</strong> <code>#</code></em></a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link *foo **bar** `#`*](/uri)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/uri\">link <em>foo <strong>bar</strong> <code>#</code></em></a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec488_CommonMark()
        {
            // The following Markdown:
            //     [![moon](moon.jpg)](/uri)
            //
            // Should be rendered as:
            //     <p><a href="/uri"><img src="moon.jpg" alt="moon" /></a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[![moon](moon.jpg)](/uri)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/uri\"><img src=\"moon.jpg\" alt=\"moon\" /></a></p>");

            Assert.Equal(expectedResult, result);
        }

        // However, links may not contain other links, at any level of nesting.        
        [Fact]
        public void Links_Spec489_CommonMark()
        {
            // The following Markdown:
            //     [foo [bar](/uri)](/uri)
            //
            // Should be rendered as:
            //     <p>[foo <a href="/uri">bar</a>](/uri)</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo [bar](/uri)](/uri)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[foo <a href=\"/uri\">bar</a>](/uri)</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec490_CommonMark()
        {
            // The following Markdown:
            //     [foo *[bar [baz](/uri)](/uri)*](/uri)
            //
            // Should be rendered as:
            //     <p>[foo <em>[bar <a href="/uri">baz</a>](/uri)</em>](/uri)</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo *[bar [baz](/uri)](/uri)*](/uri)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[foo <em>[bar <a href=\"/uri\">baz</a>](/uri)</em>](/uri)</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec491_CommonMark()
        {
            // The following Markdown:
            //     ![[[foo](uri1)](uri2)](uri3)
            //
            // Should be rendered as:
            //     <p><img src="uri3" alt="[foo](uri2)" /></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("![[[foo](uri1)](uri2)](uri3)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><img src=\"uri3\" alt=\"[foo](uri2)\" /></p>");

            Assert.Equal(expectedResult, result);
        }

        // These cases illustrate the precedence of link text grouping over
        // emphasis grouping:        
        [Fact]
        public void Links_Spec492_CommonMark()
        {
            // The following Markdown:
            //     *[foo*](/uri)
            //
            // Should be rendered as:
            //     <p>*<a href="/uri">foo*</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*[foo*](/uri)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>*<a href=\"/uri\">foo*</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec493_CommonMark()
        {
            // The following Markdown:
            //     [foo *bar](baz*)
            //
            // Should be rendered as:
            //     <p><a href="baz*">foo *bar</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo *bar](baz*)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"baz*\">foo *bar</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // Note that brackets that *aren't* part of links do not take
        // precedence:        
        [Fact]
        public void Links_Spec494_CommonMark()
        {
            // The following Markdown:
            //     *foo [bar* baz]
            //
            // Should be rendered as:
            //     <p><em>foo [bar</em> baz]</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*foo [bar* baz]", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>foo [bar</em> baz]</p>");

            Assert.Equal(expectedResult, result);
        }

        // These cases illustrate the precedence of HTML tags, code spans,
        // and autolinks over link grouping:        
        [Fact]
        public void Links_Spec495_CommonMark()
        {
            // The following Markdown:
            //     [foo <bar attr="](baz)">
            //
            // Should be rendered as:
            //     <p>[foo <bar attr="](baz)"></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo <bar attr=\"](baz)\">", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[foo <bar attr=\"](baz)\"></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec496_CommonMark()
        {
            // The following Markdown:
            //     [foo`](/uri)`
            //
            // Should be rendered as:
            //     <p>[foo<code>](/uri)</code></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo`](/uri)`", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[foo<code>](/uri)</code></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec497_CommonMark()
        {
            // The following Markdown:
            //     [foo<http://example.com/?search=](uri)>
            //
            // Should be rendered as:
            //     <p>[foo<a href="http://example.com/?search=%5D(uri)">http://example.com/?search=](uri)</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo<http://example.com/?search=](uri)>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[foo<a href=\"http://example.com/?search=%5D(uri)\">http://example.com/?search=](uri)</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // There are three kinds of [reference link](@)s:
        // [full](#full-reference-link), [collapsed](#collapsed-reference-link),
        // and [shortcut](#shortcut-reference-link).
        // 
        // A [full reference link](@)
        // consists of a [link text] immediately followed by a [link label]
        // that [matches] a [link reference definition] elsewhere in the document.
        // 
        // A [link label](@)  begins with a left bracket (`[`) and ends
        // with the first right bracket (`]`) that is not backslash-escaped.
        // Between these brackets there must be at least one [non-whitespace character].
        // Unescaped square bracket characters are not allowed inside the
        // opening and closing square brackets of [link labels].  A link
        // label can have at most 999 characters inside the square
        // brackets.
        // 
        // One label [matches](@)
        // another just in case their normalized forms are equal.  To normalize a
        // label, strip off the opening and closing brackets,
        // perform the *Unicode case fold*, strip leading and trailing
        // [whitespace] and collapse consecutive internal
        // [whitespace] to a single space.  If there are multiple
        // matching reference link definitions, the one that comes first in the
        // document is used.  (It is desirable in such cases to emit a warning.)
        // 
        // The contents of the first link label are parsed as inlines, which are
        // used as the link's text.  The link's URI and title are provided by the
        // matching [link reference definition].
        // 
        // Here is a simple example:        
        [Fact]
        public void Links_Spec498_CommonMark()
        {
            // The following Markdown:
            //     [foo][bar]
            //     
            //     [bar]: /url "title"
            //
            // Should be rendered as:
            //     <p><a href="/url" title="title">foo</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo][bar]\n\n[bar]: /url \"title\"", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/url\" title=\"title\">foo</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // The rules for the [link text] are the same as with
        // [inline links].  Thus:
        // 
        // The link text may contain balanced brackets, but not unbalanced ones,
        // unless they are escaped:        
        [Fact]
        public void Links_Spec499_CommonMark()
        {
            // The following Markdown:
            //     [link [foo [bar]]][ref]
            //     
            //     [ref]: /uri
            //
            // Should be rendered as:
            //     <p><a href="/uri">link [foo [bar]]</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link [foo [bar]]][ref]\n\n[ref]: /uri", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/uri\">link [foo [bar]]</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec500_CommonMark()
        {
            // The following Markdown:
            //     [link \[bar][ref]
            //     
            //     [ref]: /uri
            //
            // Should be rendered as:
            //     <p><a href="/uri">link [bar</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link \\[bar][ref]\n\n[ref]: /uri", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/uri\">link [bar</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // The link text may contain inline content:        
        [Fact]
        public void Links_Spec501_CommonMark()
        {
            // The following Markdown:
            //     [link *foo **bar** `#`*][ref]
            //     
            //     [ref]: /uri
            //
            // Should be rendered as:
            //     <p><a href="/uri">link <em>foo <strong>bar</strong> <code>#</code></em></a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[link *foo **bar** `#`*][ref]\n\n[ref]: /uri", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/uri\">link <em>foo <strong>bar</strong> <code>#</code></em></a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec502_CommonMark()
        {
            // The following Markdown:
            //     [![moon](moon.jpg)][ref]
            //     
            //     [ref]: /uri
            //
            // Should be rendered as:
            //     <p><a href="/uri"><img src="moon.jpg" alt="moon" /></a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[![moon](moon.jpg)][ref]\n\n[ref]: /uri", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/uri\"><img src=\"moon.jpg\" alt=\"moon\" /></a></p>");

            Assert.Equal(expectedResult, result);
        }

        // However, links may not contain other links, at any level of nesting.        
        [Fact]
        public void Links_Spec503_CommonMark()
        {
            // The following Markdown:
            //     [foo [bar](/uri)][ref]
            //     
            //     [ref]: /uri
            //
            // Should be rendered as:
            //     <p>[foo <a href="/uri">bar</a>]<a href="/uri">ref</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo [bar](/uri)][ref]\n\n[ref]: /uri", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[foo <a href=\"/uri\">bar</a>]<a href=\"/uri\">ref</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec504_CommonMark()
        {
            // The following Markdown:
            //     [foo *bar [baz][ref]*][ref]
            //     
            //     [ref]: /uri
            //
            // Should be rendered as:
            //     <p>[foo <em>bar <a href="/uri">baz</a></em>]<a href="/uri">ref</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo *bar [baz][ref]*][ref]\n\n[ref]: /uri", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[foo <em>bar <a href=\"/uri\">baz</a></em>]<a href=\"/uri\">ref</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // (In the examples above, we have two [shortcut reference links]
        // instead of one [full reference link].)
        // 
        // The following cases illustrate the precedence of link text grouping over
        // emphasis grouping:        
        [Fact]
        public void Links_Spec505_CommonMark()
        {
            // The following Markdown:
            //     *[foo*][ref]
            //     
            //     [ref]: /uri
            //
            // Should be rendered as:
            //     <p>*<a href="/uri">foo*</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*[foo*][ref]\n\n[ref]: /uri", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>*<a href=\"/uri\">foo*</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec506_CommonMark()
        {
            // The following Markdown:
            //     [foo *bar][ref]
            //     
            //     [ref]: /uri
            //
            // Should be rendered as:
            //     <p><a href="/uri">foo *bar</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo *bar][ref]\n\n[ref]: /uri", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/uri\">foo *bar</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // These cases illustrate the precedence of HTML tags, code spans,
        // and autolinks over link grouping:        
        [Fact]
        public void Links_Spec507_CommonMark()
        {
            // The following Markdown:
            //     [foo <bar attr="][ref]">
            //     
            //     [ref]: /uri
            //
            // Should be rendered as:
            //     <p>[foo <bar attr="][ref]"></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo <bar attr=\"][ref]\">\n\n[ref]: /uri", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[foo <bar attr=\"][ref]\"></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec508_CommonMark()
        {
            // The following Markdown:
            //     [foo`][ref]`
            //     
            //     [ref]: /uri
            //
            // Should be rendered as:
            //     <p>[foo<code>][ref]</code></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo`][ref]`\n\n[ref]: /uri", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[foo<code>][ref]</code></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec509_CommonMark()
        {
            // The following Markdown:
            //     [foo<http://example.com/?search=][ref]>
            //     
            //     [ref]: /uri
            //
            // Should be rendered as:
            //     <p>[foo<a href="http://example.com/?search=%5D%5Bref%5D">http://example.com/?search=][ref]</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo<http://example.com/?search=][ref]>\n\n[ref]: /uri", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[foo<a href=\"http://example.com/?search=%5D%5Bref%5D\">http://example.com/?search=][ref]</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // Matching is case-insensitive:        
        [Fact]
        public void Links_Spec510_CommonMark()
        {
            // The following Markdown:
            //     [foo][BaR]
            //     
            //     [bar]: /url "title"
            //
            // Should be rendered as:
            //     <p><a href="/url" title="title">foo</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo][BaR]\n\n[bar]: /url \"title\"", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/url\" title=\"title\">foo</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // Unicode case fold is used:        
        [Fact]
        public void Links_Spec511_CommonMark()
        {
            // The following Markdown:
            //     [Толпой][Толпой] is a Russian word.
            //     
            //     [ТОЛПОЙ]: /url
            //
            // Should be rendered as:
            //     <p><a href="/url">Толпой</a> is a Russian word.</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[Толпой][Толпой] is a Russian word.\n\n[ТОЛПОЙ]: /url", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/url\">Толпой</a> is a Russian word.</p>");

            Assert.Equal(expectedResult, result);
        }

        // Consecutive internal [whitespace] is treated as one space for
        // purposes of determining matching:        
        [Fact]
        public void Links_Spec512_CommonMark()
        {
            // The following Markdown:
            //     [Foo
            //       bar]: /url
            //     
            //     [Baz][Foo bar]
            //
            // Should be rendered as:
            //     <p><a href="/url">Baz</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[Foo\n  bar]: /url\n\n[Baz][Foo bar]", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/url\">Baz</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // No [whitespace] is allowed between the [link text] and the
        // [link label]:        
        [Fact]
        public void Links_Spec513_CommonMark()
        {
            // The following Markdown:
            //     [foo] [bar]
            //     
            //     [bar]: /url "title"
            //
            // Should be rendered as:
            //     <p>[foo] <a href="/url" title="title">bar</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo] [bar]\n\n[bar]: /url \"title\"", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[foo] <a href=\"/url\" title=\"title\">bar</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec514_CommonMark()
        {
            // The following Markdown:
            //     [foo]
            //     [bar]
            //     
            //     [bar]: /url "title"
            //
            // Should be rendered as:
            //     <p>[foo]
            //     <a href="/url" title="title">bar</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo]\n[bar]\n\n[bar]: /url \"title\"", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[foo]\n<a href=\"/url\" title=\"title\">bar</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // This is a departure from John Gruber's original Markdown syntax
        // description, which explicitly allows whitespace between the link
        // text and the link label.  It brings reference links in line with
        // [inline links], which (according to both original Markdown and
        // this spec) cannot have whitespace after the link text.  More
        // importantly, it prevents inadvertent capture of consecutive
        // [shortcut reference links]. If whitespace is allowed between the
        // link text and the link label, then in the following we will have
        // a single reference link, not two shortcut reference links, as
        // intended:
        // 
        // ``` markdown
        // [foo]
        // [bar]
        // 
        // [foo]: /url1
        // [bar]: /url2
        // ```
        // 
        // (Note that [shortcut reference links] were introduced by Gruber
        // himself in a beta version of `Markdown.pl`, but never included
        // in the official syntax description.  Without shortcut reference
        // links, it is harmless to allow space between the link text and
        // link label; but once shortcut references are introduced, it is
        // too dangerous to allow this, as it frequently leads to
        // unintended results.)
        // 
        // When there are multiple matching [link reference definitions],
        // the first is used:        
        [Fact]
        public void Links_Spec515_CommonMark()
        {
            // The following Markdown:
            //     [foo]: /url1
            //     
            //     [foo]: /url2
            //     
            //     [bar][foo]
            //
            // Should be rendered as:
            //     <p><a href="/url1">bar</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo]: /url1\n\n[foo]: /url2\n\n[bar][foo]", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/url1\">bar</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // Note that matching is performed on normalized strings, not parsed
        // inline content.  So the following does not match, even though the
        // labels define equivalent inline content:        
        [Fact]
        public void Links_Spec516_CommonMark()
        {
            // The following Markdown:
            //     [bar][foo\!]
            //     
            //     [foo!]: /url
            //
            // Should be rendered as:
            //     <p>[bar][foo!]</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[bar][foo\\!]\n\n[foo!]: /url", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[bar][foo!]</p>");

            Assert.Equal(expectedResult, result);
        }

        // [Link labels] cannot contain brackets, unless they are
        // backslash-escaped:        
        [Fact]
        public void Links_Spec517_CommonMark()
        {
            // The following Markdown:
            //     [foo][ref[]
            //     
            //     [ref[]: /uri
            //
            // Should be rendered as:
            //     <p>[foo][ref[]</p>
            //     <p>[ref[]: /uri</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo][ref[]\n\n[ref[]: /uri", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[foo][ref[]</p>\n<p>[ref[]: /uri</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec518_CommonMark()
        {
            // The following Markdown:
            //     [foo][ref[bar]]
            //     
            //     [ref[bar]]: /uri
            //
            // Should be rendered as:
            //     <p>[foo][ref[bar]]</p>
            //     <p>[ref[bar]]: /uri</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo][ref[bar]]\n\n[ref[bar]]: /uri", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[foo][ref[bar]]</p>\n<p>[ref[bar]]: /uri</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec519_CommonMark()
        {
            // The following Markdown:
            //     [[[foo]]]
            //     
            //     [[[foo]]]: /url
            //
            // Should be rendered as:
            //     <p>[[[foo]]]</p>
            //     <p>[[[foo]]]: /url</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[[[foo]]]\n\n[[[foo]]]: /url", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[[[foo]]]</p>\n<p>[[[foo]]]: /url</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec520_CommonMark()
        {
            // The following Markdown:
            //     [foo][ref\[]
            //     
            //     [ref\[]: /uri
            //
            // Should be rendered as:
            //     <p><a href="/uri">foo</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo][ref\\[]\n\n[ref\\[]: /uri", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/uri\">foo</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // Note that in this example `]` is not backslash-escaped:        
        [Fact]
        public void Links_Spec521_CommonMark()
        {
            // The following Markdown:
            //     [bar\\]: /uri
            //     
            //     [bar\\]
            //
            // Should be rendered as:
            //     <p><a href="/uri">bar\</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[bar\\\\]: /uri\n\n[bar\\\\]", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/uri\">bar\\</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // A [link label] must contain at least one [non-whitespace character]:        
        [Fact]
        public void Links_Spec522_CommonMark()
        {
            // The following Markdown:
            //     []
            //     
            //     []: /uri
            //
            // Should be rendered as:
            //     <p>[]</p>
            //     <p>[]: /uri</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[]\n\n[]: /uri", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[]</p>\n<p>[]: /uri</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec523_CommonMark()
        {
            // The following Markdown:
            //     [
            //      ]
            //     
            //     [
            //      ]: /uri
            //
            // Should be rendered as:
            //     <p>[
            //     ]</p>
            //     <p>[
            //     ]: /uri</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[\n ]\n\n[\n ]: /uri", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[\n]</p>\n<p>[\n]: /uri</p>");

            Assert.Equal(expectedResult, result);
        }

        // A [collapsed reference link](@)
        // consists of a [link label] that [matches] a
        // [link reference definition] elsewhere in the
        // document, followed by the string `[]`.
        // The contents of the first link label are parsed as inlines,
        // which are used as the link's text.  The link's URI and title are
        // provided by the matching reference link definition.  Thus,
        // `[foo][]` is equivalent to `[foo][foo]`.        
        [Fact]
        public void Links_Spec524_CommonMark()
        {
            // The following Markdown:
            //     [foo][]
            //     
            //     [foo]: /url "title"
            //
            // Should be rendered as:
            //     <p><a href="/url" title="title">foo</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo][]\n\n[foo]: /url \"title\"", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/url\" title=\"title\">foo</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec525_CommonMark()
        {
            // The following Markdown:
            //     [*foo* bar][]
            //     
            //     [*foo* bar]: /url "title"
            //
            // Should be rendered as:
            //     <p><a href="/url" title="title"><em>foo</em> bar</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[*foo* bar][]\n\n[*foo* bar]: /url \"title\"", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/url\" title=\"title\"><em>foo</em> bar</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // The link labels are case-insensitive:        
        [Fact]
        public void Links_Spec526_CommonMark()
        {
            // The following Markdown:
            //     [Foo][]
            //     
            //     [foo]: /url "title"
            //
            // Should be rendered as:
            //     <p><a href="/url" title="title">Foo</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[Foo][]\n\n[foo]: /url \"title\"", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/url\" title=\"title\">Foo</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // As with full reference links, [whitespace] is not
        // allowed between the two sets of brackets:        
        [Fact]
        public void Links_Spec527_CommonMark()
        {
            // The following Markdown:
            //     [foo] 
            //     []
            //     
            //     [foo]: /url "title"
            //
            // Should be rendered as:
            //     <p><a href="/url" title="title">foo</a>
            //     []</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo] \n[]\n\n[foo]: /url \"title\"", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/url\" title=\"title\">foo</a>\n[]</p>");

            Assert.Equal(expectedResult, result);
        }

        // A [shortcut reference link](@)
        // consists of a [link label] that [matches] a
        // [link reference definition] elsewhere in the
        // document and is not followed by `[]` or a link label.
        // The contents of the first link label are parsed as inlines,
        // which are used as the link's text.  The link's URI and title
        // are provided by the matching link reference definition.
        // Thus, `[foo]` is equivalent to `[foo][]`.        
        [Fact]
        public void Links_Spec528_CommonMark()
        {
            // The following Markdown:
            //     [foo]
            //     
            //     [foo]: /url "title"
            //
            // Should be rendered as:
            //     <p><a href="/url" title="title">foo</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo]\n\n[foo]: /url \"title\"", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/url\" title=\"title\">foo</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec529_CommonMark()
        {
            // The following Markdown:
            //     [*foo* bar]
            //     
            //     [*foo* bar]: /url "title"
            //
            // Should be rendered as:
            //     <p><a href="/url" title="title"><em>foo</em> bar</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[*foo* bar]\n\n[*foo* bar]: /url \"title\"", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/url\" title=\"title\"><em>foo</em> bar</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec530_CommonMark()
        {
            // The following Markdown:
            //     [[*foo* bar]]
            //     
            //     [*foo* bar]: /url "title"
            //
            // Should be rendered as:
            //     <p>[<a href="/url" title="title"><em>foo</em> bar</a>]</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[[*foo* bar]]\n\n[*foo* bar]: /url \"title\"", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[<a href=\"/url\" title=\"title\"><em>foo</em> bar</a>]</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec531_CommonMark()
        {
            // The following Markdown:
            //     [[bar [foo]
            //     
            //     [foo]: /url
            //
            // Should be rendered as:
            //     <p>[[bar <a href="/url">foo</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[[bar [foo]\n\n[foo]: /url", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[[bar <a href=\"/url\">foo</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // The link labels are case-insensitive:        
        [Fact]
        public void Links_Spec532_CommonMark()
        {
            // The following Markdown:
            //     [Foo]
            //     
            //     [foo]: /url "title"
            //
            // Should be rendered as:
            //     <p><a href="/url" title="title">Foo</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[Foo]\n\n[foo]: /url \"title\"", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/url\" title=\"title\">Foo</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // A space after the link text should be preserved:        
        [Fact]
        public void Links_Spec533_CommonMark()
        {
            // The following Markdown:
            //     [foo] bar
            //     
            //     [foo]: /url
            //
            // Should be rendered as:
            //     <p><a href="/url">foo</a> bar</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo] bar\n\n[foo]: /url", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/url\">foo</a> bar</p>");

            Assert.Equal(expectedResult, result);
        }

        // If you just want bracketed text, you can backslash-escape the
        // opening bracket to avoid links:        
        [Fact]
        public void Links_Spec534_CommonMark()
        {
            // The following Markdown:
            //     \[foo]
            //     
            //     [foo]: /url "title"
            //
            // Should be rendered as:
            //     <p>[foo]</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("\\[foo]\n\n[foo]: /url \"title\"", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[foo]</p>");

            Assert.Equal(expectedResult, result);
        }

        // Note that this is a link, because a link label ends with the first
        // following closing bracket:        
        [Fact]
        public void Links_Spec535_CommonMark()
        {
            // The following Markdown:
            //     [foo*]: /url
            //     
            //     *[foo*]
            //
            // Should be rendered as:
            //     <p>*<a href="/url">foo*</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo*]: /url\n\n*[foo*]", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>*<a href=\"/url\">foo*</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // Full and compact references take precedence over shortcut
        // references:        
        [Fact]
        public void Links_Spec536_CommonMark()
        {
            // The following Markdown:
            //     [foo][bar]
            //     
            //     [foo]: /url1
            //     [bar]: /url2
            //
            // Should be rendered as:
            //     <p><a href="/url2">foo</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo][bar]\n\n[foo]: /url1\n[bar]: /url2", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/url2\">foo</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec537_CommonMark()
        {
            // The following Markdown:
            //     [foo][]
            //     
            //     [foo]: /url1
            //
            // Should be rendered as:
            //     <p><a href="/url1">foo</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo][]\n\n[foo]: /url1", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/url1\">foo</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // Inline links also take precedence:        
        [Fact]
        public void Links_Spec538_CommonMark()
        {
            // The following Markdown:
            //     [foo]()
            //     
            //     [foo]: /url1
            //
            // Should be rendered as:
            //     <p><a href="">foo</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo]()\n\n[foo]: /url1", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"\">foo</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Links_Spec539_CommonMark()
        {
            // The following Markdown:
            //     [foo](not a link)
            //     
            //     [foo]: /url1
            //
            // Should be rendered as:
            //     <p><a href="/url1">foo</a>(not a link)</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo](not a link)\n\n[foo]: /url1", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/url1\">foo</a>(not a link)</p>");

            Assert.Equal(expectedResult, result);
        }

        // In the following case `[bar][baz]` is parsed as a reference,
        // `[foo]` as normal text:        
        [Fact]
        public void Links_Spec540_CommonMark()
        {
            // The following Markdown:
            //     [foo][bar][baz]
            //     
            //     [baz]: /url
            //
            // Should be rendered as:
            //     <p>[foo]<a href="/url">bar</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo][bar][baz]\n\n[baz]: /url", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[foo]<a href=\"/url\">bar</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // Here, though, `[foo][bar]` is parsed as a reference, since
        // `[bar]` is defined:        
        [Fact]
        public void Links_Spec541_CommonMark()
        {
            // The following Markdown:
            //     [foo][bar][baz]
            //     
            //     [baz]: /url1
            //     [bar]: /url2
            //
            // Should be rendered as:
            //     <p><a href="/url2">foo</a><a href="/url1">baz</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo][bar][baz]\n\n[baz]: /url1\n[bar]: /url2", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"/url2\">foo</a><a href=\"/url1\">baz</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // Here `[foo]` is not parsed as a shortcut reference, because it
        // is followed by a link label (even though `[bar]` is not defined):        
        [Fact]
        public void Links_Spec542_CommonMark()
        {
            // The following Markdown:
            //     [foo][bar][baz]
            //     
            //     [baz]: /url1
            //     [foo]: /url2
            //
            // Should be rendered as:
            //     <p>[foo]<a href="/url1">bar</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("[foo][bar][baz]\n\n[baz]: /url1\n[foo]: /url2", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>[foo]<a href=\"/url1\">bar</a></p>");

            Assert.Equal(expectedResult, result);
        }
    }

    // Syntax for images is like the syntax for links, with one
    // difference. Instead of [link text], we have an
    // [image description](@).  The rules for this are the
    // same as for [link text], except that (a) an
    // image description starts with `![` rather than `[`, and
    // (b) an image description may contain links.
    // An image description has inline elements
    // as its contents.  When an image is rendered to HTML,
    // this is standardly used as the image's `alt` attribute.  
    public class ImagesTests
    {
        
        [Fact]
        public void Images_Spec543_CommonMark()
        {
            // The following Markdown:
            //     ![foo](/url "title")
            //
            // Should be rendered as:
            //     <p><img src="/url" alt="foo" title="title" /></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("![foo](/url \"title\")", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><img src=\"/url\" alt=\"foo\" title=\"title\" /></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Images_Spec544_CommonMark()
        {
            // The following Markdown:
            //     ![foo *bar*]
            //     
            //     [foo *bar*]: train.jpg "train & tracks"
            //
            // Should be rendered as:
            //     <p><img src="train.jpg" alt="foo bar" title="train &amp; tracks" /></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("![foo *bar*]\n\n[foo *bar*]: train.jpg \"train & tracks\"", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><img src=\"train.jpg\" alt=\"foo bar\" title=\"train &amp; tracks\" /></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Images_Spec545_CommonMark()
        {
            // The following Markdown:
            //     ![foo ![bar](/url)](/url2)
            //
            // Should be rendered as:
            //     <p><img src="/url2" alt="foo bar" /></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("![foo ![bar](/url)](/url2)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><img src=\"/url2\" alt=\"foo bar\" /></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Images_Spec546_CommonMark()
        {
            // The following Markdown:
            //     ![foo [bar](/url)](/url2)
            //
            // Should be rendered as:
            //     <p><img src="/url2" alt="foo bar" /></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("![foo [bar](/url)](/url2)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><img src=\"/url2\" alt=\"foo bar\" /></p>");

            Assert.Equal(expectedResult, result);
        }

        // Though this spec is concerned with parsing, not rendering, it is
        // recommended that in rendering to HTML, only the plain string content
        // of the [image description] be used.  Note that in
        // the above example, the alt attribute's value is `foo bar`, not `foo
        // [bar](/url)` or `foo <a href="/url">bar</a>`.  Only the plain string
        // content is rendered, without formatting.        
        [Fact]
        public void Images_Spec547_CommonMark()
        {
            // The following Markdown:
            //     ![foo *bar*][]
            //     
            //     [foo *bar*]: train.jpg "train & tracks"
            //
            // Should be rendered as:
            //     <p><img src="train.jpg" alt="foo bar" title="train &amp; tracks" /></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("![foo *bar*][]\n\n[foo *bar*]: train.jpg \"train & tracks\"", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><img src=\"train.jpg\" alt=\"foo bar\" title=\"train &amp; tracks\" /></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Images_Spec548_CommonMark()
        {
            // The following Markdown:
            //     ![foo *bar*][foobar]
            //     
            //     [FOOBAR]: train.jpg "train & tracks"
            //
            // Should be rendered as:
            //     <p><img src="train.jpg" alt="foo bar" title="train &amp; tracks" /></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("![foo *bar*][foobar]\n\n[FOOBAR]: train.jpg \"train & tracks\"", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><img src=\"train.jpg\" alt=\"foo bar\" title=\"train &amp; tracks\" /></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Images_Spec549_CommonMark()
        {
            // The following Markdown:
            //     ![foo](train.jpg)
            //
            // Should be rendered as:
            //     <p><img src="train.jpg" alt="foo" /></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("![foo](train.jpg)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><img src=\"train.jpg\" alt=\"foo\" /></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Images_Spec550_CommonMark()
        {
            // The following Markdown:
            //     My ![foo bar](/path/to/train.jpg  "title"   )
            //
            // Should be rendered as:
            //     <p>My <img src="/path/to/train.jpg" alt="foo bar" title="title" /></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("My ![foo bar](/path/to/train.jpg  \"title\"   )", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>My <img src=\"/path/to/train.jpg\" alt=\"foo bar\" title=\"title\" /></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Images_Spec551_CommonMark()
        {
            // The following Markdown:
            //     ![foo](<url>)
            //
            // Should be rendered as:
            //     <p><img src="url" alt="foo" /></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("![foo](<url>)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><img src=\"url\" alt=\"foo\" /></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Images_Spec552_CommonMark()
        {
            // The following Markdown:
            //     ![](/url)
            //
            // Should be rendered as:
            //     <p><img src="/url" alt="" /></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("![](/url)", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><img src=\"/url\" alt=\"\" /></p>");

            Assert.Equal(expectedResult, result);
        }

        // Reference-style:        
        [Fact]
        public void Images_Spec553_CommonMark()
        {
            // The following Markdown:
            //     ![foo][bar]
            //     
            //     [bar]: /url
            //
            // Should be rendered as:
            //     <p><img src="/url" alt="foo" /></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("![foo][bar]\n\n[bar]: /url", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><img src=\"/url\" alt=\"foo\" /></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Images_Spec554_CommonMark()
        {
            // The following Markdown:
            //     ![foo][bar]
            //     
            //     [BAR]: /url
            //
            // Should be rendered as:
            //     <p><img src="/url" alt="foo" /></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("![foo][bar]\n\n[BAR]: /url", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><img src=\"/url\" alt=\"foo\" /></p>");

            Assert.Equal(expectedResult, result);
        }

        // Collapsed:        
        [Fact]
        public void Images_Spec555_CommonMark()
        {
            // The following Markdown:
            //     ![foo][]
            //     
            //     [foo]: /url "title"
            //
            // Should be rendered as:
            //     <p><img src="/url" alt="foo" title="title" /></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("![foo][]\n\n[foo]: /url \"title\"", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><img src=\"/url\" alt=\"foo\" title=\"title\" /></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Images_Spec556_CommonMark()
        {
            // The following Markdown:
            //     ![*foo* bar][]
            //     
            //     [*foo* bar]: /url "title"
            //
            // Should be rendered as:
            //     <p><img src="/url" alt="foo bar" title="title" /></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("![*foo* bar][]\n\n[*foo* bar]: /url \"title\"", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><img src=\"/url\" alt=\"foo bar\" title=\"title\" /></p>");

            Assert.Equal(expectedResult, result);
        }

        // The labels are case-insensitive:        
        [Fact]
        public void Images_Spec557_CommonMark()
        {
            // The following Markdown:
            //     ![Foo][]
            //     
            //     [foo]: /url "title"
            //
            // Should be rendered as:
            //     <p><img src="/url" alt="Foo" title="title" /></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("![Foo][]\n\n[foo]: /url \"title\"", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><img src=\"/url\" alt=\"Foo\" title=\"title\" /></p>");

            Assert.Equal(expectedResult, result);
        }

        // As with reference links, [whitespace] is not allowed
        // between the two sets of brackets:        
        [Fact]
        public void Images_Spec558_CommonMark()
        {
            // The following Markdown:
            //     ![foo] 
            //     []
            //     
            //     [foo]: /url "title"
            //
            // Should be rendered as:
            //     <p><img src="/url" alt="foo" title="title" />
            //     []</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("![foo] \n[]\n\n[foo]: /url \"title\"", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><img src=\"/url\" alt=\"foo\" title=\"title\" />\n[]</p>");

            Assert.Equal(expectedResult, result);
        }

        // Shortcut:        
        [Fact]
        public void Images_Spec559_CommonMark()
        {
            // The following Markdown:
            //     ![foo]
            //     
            //     [foo]: /url "title"
            //
            // Should be rendered as:
            //     <p><img src="/url" alt="foo" title="title" /></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("![foo]\n\n[foo]: /url \"title\"", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><img src=\"/url\" alt=\"foo\" title=\"title\" /></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Images_Spec560_CommonMark()
        {
            // The following Markdown:
            //     ![*foo* bar]
            //     
            //     [*foo* bar]: /url "title"
            //
            // Should be rendered as:
            //     <p><img src="/url" alt="foo bar" title="title" /></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("![*foo* bar]\n\n[*foo* bar]: /url \"title\"", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><img src=\"/url\" alt=\"foo bar\" title=\"title\" /></p>");

            Assert.Equal(expectedResult, result);
        }

        // Note that link labels cannot contain unescaped brackets:        
        [Fact]
        public void Images_Spec561_CommonMark()
        {
            // The following Markdown:
            //     ![[foo]]
            //     
            //     [[foo]]: /url "title"
            //
            // Should be rendered as:
            //     <p>![[foo]]</p>
            //     <p>[[foo]]: /url &quot;title&quot;</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("![[foo]]\n\n[[foo]]: /url \"title\"", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>![[foo]]</p>\n<p>[[foo]]: /url &quot;title&quot;</p>");

            Assert.Equal(expectedResult, result);
        }

        // The link labels are case-insensitive:        
        [Fact]
        public void Images_Spec562_CommonMark()
        {
            // The following Markdown:
            //     ![Foo]
            //     
            //     [foo]: /url "title"
            //
            // Should be rendered as:
            //     <p><img src="/url" alt="Foo" title="title" /></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("![Foo]\n\n[foo]: /url \"title\"", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><img src=\"/url\" alt=\"Foo\" title=\"title\" /></p>");

            Assert.Equal(expectedResult, result);
        }

        // If you just want a literal `!` followed by bracketed text, you can
        // backslash-escape the opening `[`:        
        [Fact]
        public void Images_Spec563_CommonMark()
        {
            // The following Markdown:
            //     !\[foo]
            //     
            //     [foo]: /url "title"
            //
            // Should be rendered as:
            //     <p>![foo]</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("!\\[foo]\n\n[foo]: /url \"title\"", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>![foo]</p>");

            Assert.Equal(expectedResult, result);
        }

        // If you want a link after a literal `!`, backslash-escape the
        // `!`:        
        [Fact]
        public void Images_Spec564_CommonMark()
        {
            // The following Markdown:
            //     \![foo]
            //     
            //     [foo]: /url "title"
            //
            // Should be rendered as:
            //     <p>!<a href="/url" title="title">foo</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("\\![foo]\n\n[foo]: /url \"title\"", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>!<a href=\"/url\" title=\"title\">foo</a></p>");

            Assert.Equal(expectedResult, result);
        }
    }

    // [Autolink](@)s are absolute URIs and email addresses inside
    // `<` and `>`. They are parsed as links, with the URL or email address
    // as the link label.
    // 
    // A [URI autolink](@) consists of `<`, followed by an
    // [absolute URI] not containing `<`, followed by `>`.  It is parsed as
    // a link to the URI, with the URI as the link's label.
    // 
    // An [absolute URI](@),
    // for these purposes, consists of a [scheme] followed by a colon (`:`)
    // followed by zero or more characters other than ASCII
    // [whitespace] and control characters, `<`, and `>`.  If
    // the URI includes these characters, they must be percent-encoded
    // (e.g. `%20` for a space).
    // 
    // For purposes of this spec, a [scheme](@) is any sequence
    // of 2--32 characters beginning with an ASCII letter and followed
    // by any combination of ASCII letters, digits, or the symbols plus
    // ("+"), period ("."), or hyphen ("-")  
    public class AutolinksTests
    {

        // Here are some valid autolinks:        
        [Fact]
        public void Autolinks_Spec565_CommonMark()
        {
            // The following Markdown:
            //     <http://foo.bar.baz>
            //
            // Should be rendered as:
            //     <p><a href="http://foo.bar.baz">http://foo.bar.baz</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<http://foo.bar.baz>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"http://foo.bar.baz\">http://foo.bar.baz</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Autolinks_Spec566_CommonMark()
        {
            // The following Markdown:
            //     <http://foo.bar.baz/test?q=hello&id=22&boolean>
            //
            // Should be rendered as:
            //     <p><a href="http://foo.bar.baz/test?q=hello&amp;id=22&amp;boolean">http://foo.bar.baz/test?q=hello&amp;id=22&amp;boolean</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<http://foo.bar.baz/test?q=hello&id=22&boolean>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"http://foo.bar.baz/test?q=hello&amp;id=22&amp;boolean\">http://foo.bar.baz/test?q=hello&amp;id=22&amp;boolean</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Autolinks_Spec567_CommonMark()
        {
            // The following Markdown:
            //     <irc://foo.bar:2233/baz>
            //
            // Should be rendered as:
            //     <p><a href="irc://foo.bar:2233/baz">irc://foo.bar:2233/baz</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<irc://foo.bar:2233/baz>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"irc://foo.bar:2233/baz\">irc://foo.bar:2233/baz</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // Uppercase is also fine:        
        [Fact]
        public void Autolinks_Spec568_CommonMark()
        {
            // The following Markdown:
            //     <MAILTO:FOO@BAR.BAZ>
            //
            // Should be rendered as:
            //     <p><a href="MAILTO:FOO@BAR.BAZ">MAILTO:FOO@BAR.BAZ</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<MAILTO:FOO@BAR.BAZ>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"MAILTO:FOO@BAR.BAZ\">MAILTO:FOO@BAR.BAZ</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // Note that many strings that count as [absolute URIs] for
        // purposes of this spec are not valid URIs, because their
        // schemes are not registered or because of other problems
        // with their syntax:        
        [Fact]
        public void Autolinks_Spec569_CommonMark()
        {
            // The following Markdown:
            //     <a+b+c:d>
            //
            // Should be rendered as:
            //     <p><a href="a+b+c:d">a+b+c:d</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<a+b+c:d>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"a+b+c:d\">a+b+c:d</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Autolinks_Spec570_CommonMark()
        {
            // The following Markdown:
            //     <made-up-scheme://foo,bar>
            //
            // Should be rendered as:
            //     <p><a href="made-up-scheme://foo,bar">made-up-scheme://foo,bar</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<made-up-scheme://foo,bar>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"made-up-scheme://foo,bar\">made-up-scheme://foo,bar</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Autolinks_Spec571_CommonMark()
        {
            // The following Markdown:
            //     <http://../>
            //
            // Should be rendered as:
            //     <p><a href="http://../">http://../</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<http://../>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"http://../\">http://../</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Autolinks_Spec572_CommonMark()
        {
            // The following Markdown:
            //     <localhost:5001/foo>
            //
            // Should be rendered as:
            //     <p><a href="localhost:5001/foo">localhost:5001/foo</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<localhost:5001/foo>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"localhost:5001/foo\">localhost:5001/foo</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // Spaces are not allowed in autolinks:        
        [Fact]
        public void Autolinks_Spec573_CommonMark()
        {
            // The following Markdown:
            //     <http://foo.bar/baz bim>
            //
            // Should be rendered as:
            //     <p>&lt;http://foo.bar/baz bim&gt;</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<http://foo.bar/baz bim>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>&lt;http://foo.bar/baz bim&gt;</p>");

            Assert.Equal(expectedResult, result);
        }

        // Backslash-escapes do not work inside autolinks:        
        [Fact]
        public void Autolinks_Spec574_CommonMark()
        {
            // The following Markdown:
            //     <http://example.com/\[\>
            //
            // Should be rendered as:
            //     <p><a href="http://example.com/%5C%5B%5C">http://example.com/\[\</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<http://example.com/\\[\\>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"http://example.com/%5C%5B%5C\">http://example.com/\\[\\</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // An [email autolink](@)
        // consists of `<`, followed by an [email address],
        // followed by `>`.  The link's label is the email address,
        // and the URL is `mailto:` followed by the email address.
        // 
        // An [email address](@),
        // for these purposes, is anything that matches
        // the [non-normative regex from the HTML5
        // spec](https://html.spec.whatwg.org/multipage/forms.html#e-mail-state-(type=email)):
        // 
        //     /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?
        //     (?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/
        // 
        // Examples of email autolinks:        
        [Fact]
        public void Autolinks_Spec575_CommonMark()
        {
            // The following Markdown:
            //     <foo@bar.example.com>
            //
            // Should be rendered as:
            //     <p><a href="mailto:foo@bar.example.com">foo@bar.example.com</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<foo@bar.example.com>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"mailto:foo@bar.example.com\">foo@bar.example.com</a></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Autolinks_Spec576_CommonMark()
        {
            // The following Markdown:
            //     <foo+special@Bar.baz-bar0.com>
            //
            // Should be rendered as:
            //     <p><a href="mailto:foo+special@Bar.baz-bar0.com">foo+special@Bar.baz-bar0.com</a></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<foo+special@Bar.baz-bar0.com>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"mailto:foo+special@Bar.baz-bar0.com\">foo+special@Bar.baz-bar0.com</a></p>");

            Assert.Equal(expectedResult, result);
        }

        // Backslash-escapes do not work inside email autolinks:        
        [Fact]
        public void Autolinks_Spec577_CommonMark()
        {
            // The following Markdown:
            //     <foo\+@bar.example.com>
            //
            // Should be rendered as:
            //     <p>&lt;foo+@bar.example.com&gt;</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<foo\\+@bar.example.com>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>&lt;foo+@bar.example.com&gt;</p>");

            Assert.Equal(expectedResult, result);
        }

        // These are not autolinks:        
        [Fact]
        public void Autolinks_Spec578_CommonMark()
        {
            // The following Markdown:
            //     <>
            //
            // Should be rendered as:
            //     <p>&lt;&gt;</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>&lt;&gt;</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Autolinks_Spec579_CommonMark()
        {
            // The following Markdown:
            //     < http://foo.bar >
            //
            // Should be rendered as:
            //     <p>&lt; http://foo.bar &gt;</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("< http://foo.bar >", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>&lt; http://foo.bar &gt;</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Autolinks_Spec580_CommonMark()
        {
            // The following Markdown:
            //     <m:abc>
            //
            // Should be rendered as:
            //     <p>&lt;m:abc&gt;</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<m:abc>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>&lt;m:abc&gt;</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Autolinks_Spec581_CommonMark()
        {
            // The following Markdown:
            //     <foo.bar.baz>
            //
            // Should be rendered as:
            //     <p>&lt;foo.bar.baz&gt;</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<foo.bar.baz>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>&lt;foo.bar.baz&gt;</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Autolinks_Spec582_CommonMark()
        {
            // The following Markdown:
            //     http://example.com
            //
            // Should be rendered as:
            //     <p>http://example.com</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("http://example.com", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>http://example.com</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void Autolinks_Spec583_CommonMark()
        {
            // The following Markdown:
            //     foo@bar.example.com
            //
            // Should be rendered as:
            //     <p>foo@bar.example.com</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo@bar.example.com", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo@bar.example.com</p>");

            Assert.Equal(expectedResult, result);
        }
    }

    // Text between `<` and `>` that looks like an HTML tag is parsed as a
    // raw HTML tag and will be rendered in HTML without escaping.
    // Tag and attribute names are not limited to current HTML tags,
    // so custom tags (and even, say, DocBook tags) may be used.
    // 
    // Here is the grammar for tags:
    // 
    // A [tag name](@) consists of an ASCII letter
    // followed by zero or more ASCII letters, digits, or
    // hyphens (`-`).
    // 
    // An [attribute](@) consists of [whitespace],
    // an [attribute name], and an optional
    // [attribute value specification].
    // 
    // An [attribute name](@)
    // consists of an ASCII letter, `_`, or `:`, followed by zero or more ASCII
    // letters, digits, `_`, `.`, `:`, or `-`.  (Note:  This is the XML
    // specification restricted to ASCII.  HTML5 is laxer.)
    // 
    // An [attribute value specification](@)
    // consists of optional [whitespace],
    // a `=` character, optional [whitespace], and an [attribute
    // value].
    // 
    // An [attribute value](@)
    // consists of an [unquoted attribute value],
    // a [single-quoted attribute value], or a [double-quoted attribute value].
    // 
    // An [unquoted attribute value](@)
    // is a nonempty string of characters not
    // including spaces, `"`, `'`, `=`, `<`, `>`, or `` ` ``.
    // 
    // A [single-quoted attribute value](@)
    // consists of `'`, zero or more
    // characters not including `'`, and a final `'`.
    // 
    // A [double-quoted attribute value](@)
    // consists of `"`, zero or more
    // characters not including `"`, and a final `"`.
    // 
    // An [open tag](@) consists of a `<` character, a [tag name],
    // zero or more [attributes], optional [whitespace], an optional `/`
    // character, and a `>` character.
    // 
    // A [closing tag](@) consists of the string `</`, a
    // [tag name], optional [whitespace], and the character `>`.
    // 
    // An [HTML comment](@) consists of `<!--` + *text* + `-->`,
    // where *text* does not start with `>` or `->`, does not end with `-`,
    // and does not contain `--`.  (See the
    // [HTML5 spec](http://www.w3.org/TR/html5/syntax.html#comments).)
    // 
    // A [processing instruction](@)
    // consists of the string `<?`, a string
    // of characters not including the string `?>`, and the string
    // `?>`.
    // 
    // A [declaration](@) consists of the
    // string `<!`, a name consisting of one or more uppercase ASCII letters,
    // [whitespace], a string of characters not including the
    // character `>`, and the character `>`.
    // 
    // A [CDATA section](@) consists of
    // the string `<![CDATA[`, a string of characters not including the string
    // `]]>`, and the string `]]>`.
    // 
    // An [HTML tag](@) consists of an [open tag], a [closing tag],
    // an [HTML comment], a [processing instruction], a [declaration],
    // or a [CDATA section]  
    public class RawHTMLTests
    {

        // Here are some simple open tags:        
        [Fact]
        public void RawHTML_Spec584_CommonMark()
        {
            // The following Markdown:
            //     <a><bab><c2c>
            //
            // Should be rendered as:
            //     <p><a><bab><c2c></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<a><bab><c2c>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a><bab><c2c></p>");

            Assert.Equal(expectedResult, result);
        }

        // Empty elements:        
        [Fact]
        public void RawHTML_Spec585_CommonMark()
        {
            // The following Markdown:
            //     <a/><b2/>
            //
            // Should be rendered as:
            //     <p><a/><b2/></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<a/><b2/>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a/><b2/></p>");

            Assert.Equal(expectedResult, result);
        }

        // [Whitespace] is allowed:        
        [Fact]
        public void RawHTML_Spec586_CommonMark()
        {
            // The following Markdown:
            //     <a  /><b2
            //     data="foo" >
            //
            // Should be rendered as:
            //     <p><a  /><b2
            //     data="foo" ></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<a  /><b2\ndata=\"foo\" >", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a  /><b2\ndata=\"foo\" ></p>");

            Assert.Equal(expectedResult, result);
        }

        // With attributes:        
        [Fact]
        public void RawHTML_Spec587_CommonMark()
        {
            // The following Markdown:
            //     <a foo="bar" bam = 'baz <em>"</em>'
            //     _boolean zoop:33=zoop:33 />
            //
            // Should be rendered as:
            //     <p><a foo="bar" bam = 'baz <em>"</em>'
            //     _boolean zoop:33=zoop:33 /></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<a foo=\"bar\" bam = 'baz <em>\"</em>'\n_boolean zoop:33=zoop:33 />", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a foo=\"bar\" bam = 'baz <em>\"</em>'\n_boolean zoop:33=zoop:33 /></p>");

            Assert.Equal(expectedResult, result);
        }

        // Custom tag names can be used:        
        [Fact]
        public void RawHTML_Spec588_CommonMark()
        {
            // The following Markdown:
            //     Foo <responsive-image src="foo.jpg" />
            //
            // Should be rendered as:
            //     <p>Foo <responsive-image src="foo.jpg" /></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("Foo <responsive-image src=\"foo.jpg\" />", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>Foo <responsive-image src=\"foo.jpg\" /></p>");

            Assert.Equal(expectedResult, result);
        }

        // Illegal tag names, not parsed as HTML:        
        [Fact]
        public void RawHTML_Spec589_CommonMark()
        {
            // The following Markdown:
            //     <33> <__>
            //
            // Should be rendered as:
            //     <p>&lt;33&gt; &lt;__&gt;</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<33> <__>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>&lt;33&gt; &lt;__&gt;</p>");

            Assert.Equal(expectedResult, result);
        }

        // Illegal attribute names:        
        [Fact]
        public void RawHTML_Spec590_CommonMark()
        {
            // The following Markdown:
            //     <a h*#ref="hi">
            //
            // Should be rendered as:
            //     <p>&lt;a h*#ref=&quot;hi&quot;&gt;</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<a h*#ref=\"hi\">", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>&lt;a h*#ref=&quot;hi&quot;&gt;</p>");

            Assert.Equal(expectedResult, result);
        }

        // Illegal attribute values:        
        [Fact]
        public void RawHTML_Spec591_CommonMark()
        {
            // The following Markdown:
            //     <a href="hi'> <a href=hi'>
            //
            // Should be rendered as:
            //     <p>&lt;a href=&quot;hi'&gt; &lt;a href=hi'&gt;</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<a href=\"hi'> <a href=hi'>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>&lt;a href=&quot;hi'&gt; &lt;a href=hi'&gt;</p>");

            Assert.Equal(expectedResult, result);
        }

        // Illegal [whitespace]:        
        [Fact]
        public void RawHTML_Spec592_CommonMark()
        {
            // The following Markdown:
            //     < a><
            //     foo><bar/ >
            //
            // Should be rendered as:
            //     <p>&lt; a&gt;&lt;
            //     foo&gt;&lt;bar/ &gt;</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("< a><\nfoo><bar/ >", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>&lt; a&gt;&lt;\nfoo&gt;&lt;bar/ &gt;</p>");

            Assert.Equal(expectedResult, result);
        }

        // Missing [whitespace]:        
        [Fact]
        public void RawHTML_Spec593_CommonMark()
        {
            // The following Markdown:
            //     <a href='bar'title=title>
            //
            // Should be rendered as:
            //     <p>&lt;a href='bar'title=title&gt;</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<a href='bar'title=title>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>&lt;a href='bar'title=title&gt;</p>");

            Assert.Equal(expectedResult, result);
        }

        // Closing tags:        
        [Fact]
        public void RawHTML_Spec594_CommonMark()
        {
            // The following Markdown:
            //     </a></foo >
            //
            // Should be rendered as:
            //     <p></a></foo ></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("</a></foo >", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p></a></foo ></p>");

            Assert.Equal(expectedResult, result);
        }

        // Illegal attributes in closing tag:        
        [Fact]
        public void RawHTML_Spec595_CommonMark()
        {
            // The following Markdown:
            //     </a href="foo">
            //
            // Should be rendered as:
            //     <p>&lt;/a href=&quot;foo&quot;&gt;</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("</a href=\"foo\">", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>&lt;/a href=&quot;foo&quot;&gt;</p>");

            Assert.Equal(expectedResult, result);
        }

        // Comments:        
        [Fact]
        public void RawHTML_Spec596_CommonMark()
        {
            // The following Markdown:
            //     foo <!-- this is a
            //     comment - with hyphen -->
            //
            // Should be rendered as:
            //     <p>foo <!-- this is a
            //     comment - with hyphen --></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo <!-- this is a\ncomment - with hyphen -->", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo <!-- this is a\ncomment - with hyphen --></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void RawHTML_Spec597_CommonMark()
        {
            // The following Markdown:
            //     foo <!-- not a comment -- two hyphens -->
            //
            // Should be rendered as:
            //     <p>foo &lt;!-- not a comment -- two hyphens --&gt;</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo <!-- not a comment -- two hyphens -->", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo &lt;!-- not a comment -- two hyphens --&gt;</p>");

            Assert.Equal(expectedResult, result);
        }

        // Not comments:        
        [Fact]
        public void RawHTML_Spec598_CommonMark()
        {
            // The following Markdown:
            //     foo <!--> foo -->
            //     
            //     foo <!-- foo--->
            //
            // Should be rendered as:
            //     <p>foo &lt;!--&gt; foo --&gt;</p>
            //     <p>foo &lt;!-- foo---&gt;</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo <!--> foo -->\n\nfoo <!-- foo--->", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo &lt;!--&gt; foo --&gt;</p>\n<p>foo &lt;!-- foo---&gt;</p>");

            Assert.Equal(expectedResult, result);
        }

        // Processing instructions:        
        [Fact]
        public void RawHTML_Spec599_CommonMark()
        {
            // The following Markdown:
            //     foo <?php echo $a; ?>
            //
            // Should be rendered as:
            //     <p>foo <?php echo $a; ?></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo <?php echo $a; ?>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo <?php echo $a; ?></p>");

            Assert.Equal(expectedResult, result);
        }

        // Declarations:        
        [Fact]
        public void RawHTML_Spec600_CommonMark()
        {
            // The following Markdown:
            //     foo <!ELEMENT br EMPTY>
            //
            // Should be rendered as:
            //     <p>foo <!ELEMENT br EMPTY></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo <!ELEMENT br EMPTY>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo <!ELEMENT br EMPTY></p>");

            Assert.Equal(expectedResult, result);
        }

        // CDATA sections:        
        [Fact]
        public void RawHTML_Spec601_CommonMark()
        {
            // The following Markdown:
            //     foo <![CDATA[>&<]]>
            //
            // Should be rendered as:
            //     <p>foo <![CDATA[>&<]]></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo <![CDATA[>&<]]>", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo <![CDATA[>&<]]></p>");

            Assert.Equal(expectedResult, result);
        }

        // Entity and numeric character references are preserved in HTML
        // attributes:        
        [Fact]
        public void RawHTML_Spec602_CommonMark()
        {
            // The following Markdown:
            //     foo <a href="&ouml;">
            //
            // Should be rendered as:
            //     <p>foo <a href="&ouml;"></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo <a href=\"&ouml;\">", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo <a href=\"&ouml;\"></p>");

            Assert.Equal(expectedResult, result);
        }

        // Backslash escapes do not work in HTML attributes:        
        [Fact]
        public void RawHTML_Spec603_CommonMark()
        {
            // The following Markdown:
            //     foo <a href="\*">
            //
            // Should be rendered as:
            //     <p>foo <a href="\*"></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo <a href=\"\\*\">", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo <a href=\"\\*\"></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void RawHTML_Spec604_CommonMark()
        {
            // The following Markdown:
            //     <a href="\"">
            //
            // Should be rendered as:
            //     <p>&lt;a href=&quot;&quot;&quot;&gt;</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<a href=\"\\\"\">", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>&lt;a href=&quot;&quot;&quot;&gt;</p>");

            Assert.Equal(expectedResult, result);
        }
    }

    // A line break (not in a code span or HTML tag) that is preceded
    // by two or more spaces and does not occur at the end of a block
    // is parsed as a [hard line break](@) (rendered
    // in HTML as a `<br />` tag):  
    public class HardLineBreaksTests
    {
        
        [Fact]
        public void HardLineBreaks_Spec605_CommonMark()
        {
            // The following Markdown:
            //     foo  
            //     baz
            //
            // Should be rendered as:
            //     <p>foo<br />
            //     baz</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo  \nbaz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo<br />\nbaz</p>");

            Assert.Equal(expectedResult, result);
        }

        // For a more visible alternative, a backslash before the
        // [line ending] may be used instead of two spaces:        
        [Fact]
        public void HardLineBreaks_Spec606_CommonMark()
        {
            // The following Markdown:
            //     foo\
            //     baz
            //
            // Should be rendered as:
            //     <p>foo<br />
            //     baz</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo\\\nbaz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo<br />\nbaz</p>");

            Assert.Equal(expectedResult, result);
        }

        // More than two spaces can be used:        
        [Fact]
        public void HardLineBreaks_Spec607_CommonMark()
        {
            // The following Markdown:
            //     foo       
            //     baz
            //
            // Should be rendered as:
            //     <p>foo<br />
            //     baz</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo       \nbaz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo<br />\nbaz</p>");

            Assert.Equal(expectedResult, result);
        }

        // Leading spaces at the beginning of the next line are ignored:        
        [Fact]
        public void HardLineBreaks_Spec608_CommonMark()
        {
            // The following Markdown:
            //     foo  
            //          bar
            //
            // Should be rendered as:
            //     <p>foo<br />
            //     bar</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo  \n     bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo<br />\nbar</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void HardLineBreaks_Spec609_CommonMark()
        {
            // The following Markdown:
            //     foo\
            //          bar
            //
            // Should be rendered as:
            //     <p>foo<br />
            //     bar</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo\\\n     bar", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo<br />\nbar</p>");

            Assert.Equal(expectedResult, result);
        }

        // Line breaks can occur inside emphasis, links, and other constructs
        // that allow inline content:        
        [Fact]
        public void HardLineBreaks_Spec610_CommonMark()
        {
            // The following Markdown:
            //     *foo  
            //     bar*
            //
            // Should be rendered as:
            //     <p><em>foo<br />
            //     bar</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*foo  \nbar*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>foo<br />\nbar</em></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void HardLineBreaks_Spec611_CommonMark()
        {
            // The following Markdown:
            //     *foo\
            //     bar*
            //
            // Should be rendered as:
            //     <p><em>foo<br />
            //     bar</em></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("*foo\\\nbar*", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><em>foo<br />\nbar</em></p>");

            Assert.Equal(expectedResult, result);
        }

        // Line breaks do not occur inside code spans        
        [Fact]
        public void HardLineBreaks_Spec612_CommonMark()
        {
            // The following Markdown:
            //     `code  
            //     span`
            //
            // Should be rendered as:
            //     <p><code>code span</code></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("`code  \nspan`", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><code>code span</code></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void HardLineBreaks_Spec613_CommonMark()
        {
            // The following Markdown:
            //     `code\
            //     span`
            //
            // Should be rendered as:
            //     <p><code>code\ span</code></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("`code\\\nspan`", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><code>code\\ span</code></p>");

            Assert.Equal(expectedResult, result);
        }

        // or HTML tags:        
        [Fact]
        public void HardLineBreaks_Spec614_CommonMark()
        {
            // The following Markdown:
            //     <a href="foo  
            //     bar">
            //
            // Should be rendered as:
            //     <p><a href="foo  
            //     bar"></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<a href=\"foo  \nbar\">", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"foo  \nbar\"></p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void HardLineBreaks_Spec615_CommonMark()
        {
            // The following Markdown:
            //     <a href="foo\
            //     bar">
            //
            // Should be rendered as:
            //     <p><a href="foo\
            //     bar"></p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("<a href=\"foo\\\nbar\">", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p><a href=\"foo\\\nbar\"></p>");

            Assert.Equal(expectedResult, result);
        }

        // Hard line breaks are for separating inline content within a block.
        // Neither syntax for hard line breaks works at the end of a paragraph or
        // other block element:        
        [Fact]
        public void HardLineBreaks_Spec616_CommonMark()
        {
            // The following Markdown:
            //     foo\
            //
            // Should be rendered as:
            //     <p>foo\</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo\\", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo\\</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void HardLineBreaks_Spec617_CommonMark()
        {
            // The following Markdown:
            //     foo  
            //
            // Should be rendered as:
            //     <p>foo</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo  ", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void HardLineBreaks_Spec618_CommonMark()
        {
            // The following Markdown:
            //     ### foo\
            //
            // Should be rendered as:
            //     <h3>foo\</h3>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("### foo\\", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h3>foo\\</h3>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void HardLineBreaks_Spec619_CommonMark()
        {
            // The following Markdown:
            //     ### foo  
            //
            // Should be rendered as:
            //     <h3>foo</h3>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("### foo  ", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h3>foo</h3>");

            Assert.Equal(expectedResult, result);
        }
    }

    // A regular line break (not in a code span or HTML tag) that is not
    // preceded by two or more spaces or a backslash is parsed as a
    // [softbreak](@).  (A softbreak may be rendered in HTML either as a
    // [line ending] or as a space. The result will be the same in
    // browsers. In the examples here, a [line ending] will be used.)  
    public class SoftLineBreaksTests
    {
        
        [Fact]
        public void SoftLineBreaks_Spec620_CommonMark()
        {
            // The following Markdown:
            //     foo
            //     baz
            //
            // Should be rendered as:
            //     <p>foo
            //     baz</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo\nbaz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo\nbaz</p>");

            Assert.Equal(expectedResult, result);
        }

        // Spaces at the end of the line and beginning of the next line are
        // removed:        
        [Fact]
        public void SoftLineBreaks_Spec621_CommonMark()
        {
            // The following Markdown:
            //     foo 
            //      baz
            //
            // Should be rendered as:
            //     <p>foo
            //     baz</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("foo \n baz", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>foo\nbaz</p>");

            Assert.Equal(expectedResult, result);
        }
    }

    // Any characters not given an interpretation by the above rules will
    // be parsed as plain textual content.  
    public class TextualContentTests
    {
        
        [Fact]
        public void TextualContent_Spec622_CommonMark()
        {
            // The following Markdown:
            //     hello $.;'there
            //
            // Should be rendered as:
            //     <p>hello $.;'there</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("hello $.;'there", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>hello $.;'there</p>");

            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void TextualContent_Spec623_CommonMark()
        {
            // The following Markdown:
            //     Foo χρῆν
            //
            // Should be rendered as:
            //     <p>Foo χρῆν</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("Foo χρῆν", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>Foo χρῆν</p>");

            Assert.Equal(expectedResult, result);
        }

        // Internal spaces are preserved verbatim:        
        [Fact]
        public void TextualContent_Spec624_CommonMark()
        {
            // The following Markdown:
            //     Multiple     spaces
            //
            // Should be rendered as:
            //     <p>Multiple     spaces</p>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("CommonMark");
            string result = Markdown.ToHtml("Multiple     spaces", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<p>Multiple     spaces</p>");

            Assert.Equal(expectedResult, result);
        }
    }

    // Extension Description.  
    public class SectionsTests
    {

        // Description 1.
        // test.        
        [Fact]
        public void Sections_Spec1_Sections()
        {
            // The following Markdown:
            //     # foo
            //     ## foo
            //     ### foo
            //
            // Should be rendered as:
            //     <h1>foo</h1>
            //     <section>
            //     <h2>foo</h2>
            //     <section>
            //     <h3>foo</h3>
            //     </section>
            //     </section>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("Sections");
            string result = Markdown.ToHtml("# foo\n## foo\n### foo", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h1>foo</h1>\n<section>\n<h2>foo</h2>\n<section>\n<h3>foo</h3>\n</section>\n</section>");

            Assert.Equal(expectedResult, result);
        }

        // Description 1.
        // test.        
        [Fact]
        public void Sections_Spec1_All()
        {
            // The following Markdown:
            //     # foo
            //     ## foo
            //     ### foo
            //
            // Should be rendered as:
            //     <h1>foo</h1>
            //     <section>
            //     <h2>foo</h2>
            //     <section>
            //     <h3>foo</h3>
            //     </section>
            //     </section>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("All");
            string result = Markdown.ToHtml("# foo\n## foo\n### foo", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h1>foo</h1>\n<section>\n<h2>foo</h2>\n<section>\n<h3>foo</h3>\n</section>\n</section>");

            Assert.Equal(expectedResult, result);
        }

        // Description 2.
        // test.        
        [Fact]
        public void Sections_Spec2_Sections()
        {
            // The following Markdown:
            //     # foo
            //     ## foo
            //     ### foo
            //
            // Should be rendered as:
            //     <h1>foo</h1>
            //     <section>
            //     <h2>foo</h2>
            //     <section>
            //     <h3>foo</h3>
            //     </section>
            //     </section>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("Sections");
            string result = Markdown.ToHtml("# foo\n## foo\n### foo", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h1>foo</h1>\n<section>\n<h2>foo</h2>\n<section>\n<h3>foo</h3>\n</section>\n</section>");

            Assert.Equal(expectedResult, result);
        }

        // Description 2.
        // test.        
        [Fact]
        public void Sections_Spec2_All()
        {
            // The following Markdown:
            //     # foo
            //     ## foo
            //     ### foo
            //
            // Should be rendered as:
            //     <h1>foo</h1>
            //     <section>
            //     <h2>foo</h2>
            //     <section>
            //     <h3>foo</h3>
            //     </section>
            //     </section>

            MarkdownPipeline pipeline = PipelineHelper.CreatePipeline("All");
            string result = Markdown.ToHtml("# foo\n## foo\n### foo", pipeline);
            result = PipelineHelper.Compact(result);
            string expectedResult = PipelineHelper.Compact("<h1>foo</h1>\n<section>\n<h2>foo</h2>\n<section>\n<h3>foo</h3>\n</section>\n</section>");

            Assert.Equal(expectedResult, result);
        }
    }
}

