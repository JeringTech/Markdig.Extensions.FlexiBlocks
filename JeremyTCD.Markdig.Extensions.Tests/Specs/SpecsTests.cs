







using Xunit;

namespace JeremyTCD.Markdig.Extensions.Tests.Specs
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
        public void Tabs_Spec1_commonmark()
        {
            // The following Markdown:
            //     →foo→baz→→bim
            //
            // Should be rendered as:
            //     <pre><code>foo→baz→→bim
            //     </code></pre>

            SpecTestHelper.AssertCompliance("\tfoo\tbaz\t\tbim", 
                "<pre><code>foo\tbaz\t\tbim\n</code></pre>", 
                "commonmark");
        }

        [Fact]
        public void Tabs_Spec2_commonmark()
        {
            // The following Markdown:
            //       →foo→baz→→bim
            //
            // Should be rendered as:
            //     <pre><code>foo→baz→→bim
            //     </code></pre>

            SpecTestHelper.AssertCompliance("  \tfoo\tbaz\t\tbim", 
                "<pre><code>foo\tbaz\t\tbim\n</code></pre>", 
                "commonmark");
        }

        [Fact]
        public void Tabs_Spec3_commonmark()
        {
            // The following Markdown:
            //         a→a
            //         ὐ→a
            //
            // Should be rendered as:
            //     <pre><code>a→a
            //     ὐ→a
            //     </code></pre>

            SpecTestHelper.AssertCompliance("    a\ta\n    ὐ\ta", 
                "<pre><code>a\ta\nὐ\ta\n</code></pre>", 
                "commonmark");
        }

        // In the following example, a continuation paragraph of a list
        // item is indented with a tab; this has exactly the same effect
        // as indentation with four spaces would:
        [Fact]
        public void Tabs_Spec4_commonmark()
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

            SpecTestHelper.AssertCompliance("  - foo\n\n\tbar", 
                "<ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>", 
                "commonmark");
        }

        [Fact]
        public void Tabs_Spec5_commonmark()
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

            SpecTestHelper.AssertCompliance("- foo\n\n\t\tbar", 
                "<ul>\n<li>\n<p>foo</p>\n<pre><code>  bar\n</code></pre>\n</li>\n</ul>", 
                "commonmark");
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
        public void Tabs_Spec6_commonmark()
        {
            // The following Markdown:
            //     >→→foo
            //
            // Should be rendered as:
            //     <blockquote>
            //     <pre><code>  foo
            //     </code></pre>
            //     </blockquote>

            SpecTestHelper.AssertCompliance(">\t\tfoo", 
                "<blockquote>\n<pre><code>  foo\n</code></pre>\n</blockquote>", 
                "commonmark");
        }

        [Fact]
        public void Tabs_Spec7_commonmark()
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

            SpecTestHelper.AssertCompliance("-\t\tfoo", 
                "<ul>\n<li>\n<pre><code>  foo\n</code></pre>\n</li>\n</ul>", 
                "commonmark");
        }

        [Fact]
        public void Tabs_Spec8_commonmark()
        {
            // The following Markdown:
            //         foo
            //     →bar
            //
            // Should be rendered as:
            //     <pre><code>foo
            //     bar
            //     </code></pre>

            SpecTestHelper.AssertCompliance("    foo\n\tbar", 
                "<pre><code>foo\nbar\n</code></pre>", 
                "commonmark");
        }

        [Fact]
        public void Tabs_Spec9_commonmark()
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

            SpecTestHelper.AssertCompliance(" - foo\n   - bar\n\t - baz", 
                "<ul>\n<li>foo\n<ul>\n<li>bar\n<ul>\n<li>baz</li>\n</ul>\n</li>\n</ul>\n</li>\n</ul>", 
                "commonmark");
        }

        [Fact]
        public void Tabs_Spec10_commonmark()
        {
            // The following Markdown:
            //     #→Foo
            //
            // Should be rendered as:
            //     <h1>Foo</h1>

            SpecTestHelper.AssertCompliance("#\tFoo", 
                "<h1>Foo</h1>", 
                "commonmark");
        }

        [Fact]
        public void Tabs_Spec11_commonmark()
        {
            // The following Markdown:
            //     *→*→*→
            //
            // Should be rendered as:
            //     <hr />

            SpecTestHelper.AssertCompliance("*\t*\t*\t", 
                "<hr />", 
                "commonmark");
        }
    }

    // Indicators of block structure always take precedence over indicators
    // of inline structure.  So, for example, the following is a list with
    // two items, not a list with one item containing a code span:
    public class PrecedenceTests
    {

        [Fact]
        public void Precedence_Spec12_commonmark()
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

            SpecTestHelper.AssertCompliance("- `one\n- two`", 
                "<ul>\n<li>`one</li>\n<li>two`</li>\n</ul>", 
                "commonmark");
        }
    }

    // A line consisting of 0-3 spaces of indentation, followed by a sequence
    // of three or more matching `-`, `_`, or `*` characters, each followed
    // optionally by any number of spaces, forms a
    // [thematic break](@).
    public class ThematicBreaksTests
    {

        [Fact]
        public void ThematicBreaks_Spec13_commonmark()
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

            SpecTestHelper.AssertCompliance("***\n---\n___", 
                "<hr />\n<hr />\n<hr />", 
                "commonmark");
        }

        // Wrong characters:
        [Fact]
        public void ThematicBreaks_Spec14_commonmark()
        {
            // The following Markdown:
            //     +++
            //
            // Should be rendered as:
            //     <p>+++</p>

            SpecTestHelper.AssertCompliance("+++", 
                "<p>+++</p>", 
                "commonmark");
        }

        [Fact]
        public void ThematicBreaks_Spec15_commonmark()
        {
            // The following Markdown:
            //     ===
            //
            // Should be rendered as:
            //     <p>===</p>

            SpecTestHelper.AssertCompliance("===", 
                "<p>===</p>", 
                "commonmark");
        }

        // Not enough characters:
        [Fact]
        public void ThematicBreaks_Spec16_commonmark()
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

            SpecTestHelper.AssertCompliance("--\n**\n__", 
                "<p>--\n**\n__</p>", 
                "commonmark");
        }

        // One to three spaces indent are allowed:
        [Fact]
        public void ThematicBreaks_Spec17_commonmark()
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

            SpecTestHelper.AssertCompliance(" ***\n  ***\n   ***", 
                "<hr />\n<hr />\n<hr />", 
                "commonmark");
        }

        // Four spaces is too many:
        [Fact]
        public void ThematicBreaks_Spec18_commonmark()
        {
            // The following Markdown:
            //         ***
            //
            // Should be rendered as:
            //     <pre><code>***
            //     </code></pre>

            SpecTestHelper.AssertCompliance("    ***", 
                "<pre><code>***\n</code></pre>", 
                "commonmark");
        }

        [Fact]
        public void ThematicBreaks_Spec19_commonmark()
        {
            // The following Markdown:
            //     Foo
            //         ***
            //
            // Should be rendered as:
            //     <p>Foo
            //     ***</p>

            SpecTestHelper.AssertCompliance("Foo\n    ***", 
                "<p>Foo\n***</p>", 
                "commonmark");
        }

        // More than three characters may be used:
        [Fact]
        public void ThematicBreaks_Spec20_commonmark()
        {
            // The following Markdown:
            //     _____________________________________
            //
            // Should be rendered as:
            //     <hr />

            SpecTestHelper.AssertCompliance("_____________________________________", 
                "<hr />", 
                "commonmark");
        }

        // Spaces are allowed between the characters:
        [Fact]
        public void ThematicBreaks_Spec21_commonmark()
        {
            // The following Markdown:
            //      - - -
            //
            // Should be rendered as:
            //     <hr />

            SpecTestHelper.AssertCompliance(" - - -", 
                "<hr />", 
                "commonmark");
        }

        [Fact]
        public void ThematicBreaks_Spec22_commonmark()
        {
            // The following Markdown:
            //      **  * ** * ** * **
            //
            // Should be rendered as:
            //     <hr />

            SpecTestHelper.AssertCompliance(" **  * ** * ** * **", 
                "<hr />", 
                "commonmark");
        }

        [Fact]
        public void ThematicBreaks_Spec23_commonmark()
        {
            // The following Markdown:
            //     -     -      -      -
            //
            // Should be rendered as:
            //     <hr />

            SpecTestHelper.AssertCompliance("-     -      -      -", 
                "<hr />", 
                "commonmark");
        }

        // Spaces are allowed at the end:
        [Fact]
        public void ThematicBreaks_Spec24_commonmark()
        {
            // The following Markdown:
            //     - - - -    
            //
            // Should be rendered as:
            //     <hr />

            SpecTestHelper.AssertCompliance("- - - -    ", 
                "<hr />", 
                "commonmark");
        }

        // However, no other characters may occur in the line:
        [Fact]
        public void ThematicBreaks_Spec25_commonmark()
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

            SpecTestHelper.AssertCompliance("_ _ _ _ a\n\na------\n\n---a---", 
                "<p>_ _ _ _ a</p>\n<p>a------</p>\n<p>---a---</p>", 
                "commonmark");
        }

        // It is required that all of the [non-whitespace characters] be the same.
        // So, this is not a thematic break:
        [Fact]
        public void ThematicBreaks_Spec26_commonmark()
        {
            // The following Markdown:
            //      *-*
            //
            // Should be rendered as:
            //     <p><em>-</em></p>

            SpecTestHelper.AssertCompliance(" *-*", 
                "<p><em>-</em></p>", 
                "commonmark");
        }

        // Thematic breaks do not need blank lines before or after:
        [Fact]
        public void ThematicBreaks_Spec27_commonmark()
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

            SpecTestHelper.AssertCompliance("- foo\n***\n- bar", 
                "<ul>\n<li>foo</li>\n</ul>\n<hr />\n<ul>\n<li>bar</li>\n</ul>", 
                "commonmark");
        }

        // Thematic breaks can interrupt a paragraph:
        [Fact]
        public void ThematicBreaks_Spec28_commonmark()
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

            SpecTestHelper.AssertCompliance("Foo\n***\nbar", 
                "<p>Foo</p>\n<hr />\n<p>bar</p>", 
                "commonmark");
        }

        // If a line of dashes that meets the above conditions for being a
        // thematic break could also be interpreted as the underline of a [setext
        // heading], the interpretation as a
        // [setext heading] takes precedence. Thus, for example,
        // this is a setext heading, not a paragraph followed by a thematic break:
        [Fact]
        public void ThematicBreaks_Spec29_commonmark()
        {
            // The following Markdown:
            //     Foo
            //     ---
            //     bar
            //
            // Should be rendered as:
            //     <h2>Foo</h2>
            //     <p>bar</p>

            SpecTestHelper.AssertCompliance("Foo\n---\nbar", 
                "<h2>Foo</h2>\n<p>bar</p>", 
                "commonmark");
        }

        // When both a thematic break and a list item are possible
        // interpretations of a line, the thematic break takes precedence:
        [Fact]
        public void ThematicBreaks_Spec30_commonmark()
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

            SpecTestHelper.AssertCompliance("* Foo\n* * *\n* Bar", 
                "<ul>\n<li>Foo</li>\n</ul>\n<hr />\n<ul>\n<li>Bar</li>\n</ul>", 
                "commonmark");
        }

        // If you want a thematic break in a list item, use a different bullet:
        [Fact]
        public void ThematicBreaks_Spec31_commonmark()
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

            SpecTestHelper.AssertCompliance("- Foo\n- * * *", 
                "<ul>\n<li>Foo</li>\n<li>\n<hr />\n</li>\n</ul>", 
                "commonmark");
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
        public void ATXHeadings_Spec32_commonmark()
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

            SpecTestHelper.AssertCompliance("# foo\n## foo\n### foo\n#### foo\n##### foo\n###### foo", 
                "<h1>foo</h1>\n<h2>foo</h2>\n<h3>foo</h3>\n<h4>foo</h4>\n<h5>foo</h5>\n<h6>foo</h6>", 
                "commonmark");
        }

        // More than six `#` characters is not a heading:
        [Fact]
        public void ATXHeadings_Spec33_commonmark()
        {
            // The following Markdown:
            //     ####### foo
            //
            // Should be rendered as:
            //     <p>####### foo</p>

            SpecTestHelper.AssertCompliance("####### foo", 
                "<p>####### foo</p>", 
                "commonmark");
        }

        // At least one space is required between the `#` characters and the
        // heading's contents, unless the heading is empty.  Note that many
        // implementations currently do not require the space.  However, the
        // space was required by the
        // [original ATX implementation](http://www.aaronsw.com/2002/atx/atx.py),
        // and it helps prevent things like the following from being parsed as
        // headings:
        [Fact]
        public void ATXHeadings_Spec34_commonmark()
        {
            // The following Markdown:
            //     #5 bolt
            //     
            //     #hashtag
            //
            // Should be rendered as:
            //     <p>#5 bolt</p>
            //     <p>#hashtag</p>

            SpecTestHelper.AssertCompliance("#5 bolt\n\n#hashtag", 
                "<p>#5 bolt</p>\n<p>#hashtag</p>", 
                "commonmark");
        }

        // This is not a heading, because the first `#` is escaped:
        [Fact]
        public void ATXHeadings_Spec35_commonmark()
        {
            // The following Markdown:
            //     \## foo
            //
            // Should be rendered as:
            //     <p>## foo</p>

            SpecTestHelper.AssertCompliance("\\## foo", 
                "<p>## foo</p>", 
                "commonmark");
        }

        // Contents are parsed as inlines:
        [Fact]
        public void ATXHeadings_Spec36_commonmark()
        {
            // The following Markdown:
            //     # foo *bar* \*baz\*
            //
            // Should be rendered as:
            //     <h1>foo <em>bar</em> *baz*</h1>

            SpecTestHelper.AssertCompliance("# foo *bar* \\*baz\\*", 
                "<h1>foo <em>bar</em> *baz*</h1>", 
                "commonmark");
        }

        // Leading and trailing blanks are ignored in parsing inline content:
        [Fact]
        public void ATXHeadings_Spec37_commonmark()
        {
            // The following Markdown:
            //     #                  foo                     
            //
            // Should be rendered as:
            //     <h1>foo</h1>

            SpecTestHelper.AssertCompliance("#                  foo                     ", 
                "<h1>foo</h1>", 
                "commonmark");
        }

        // One to three spaces indentation are allowed:
        [Fact]
        public void ATXHeadings_Spec38_commonmark()
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

            SpecTestHelper.AssertCompliance(" ### foo\n  ## foo\n   # foo", 
                "<h3>foo</h3>\n<h2>foo</h2>\n<h1>foo</h1>", 
                "commonmark");
        }

        // Four spaces are too much:
        [Fact]
        public void ATXHeadings_Spec39_commonmark()
        {
            // The following Markdown:
            //         # foo
            //
            // Should be rendered as:
            //     <pre><code># foo
            //     </code></pre>

            SpecTestHelper.AssertCompliance("    # foo", 
                "<pre><code># foo\n</code></pre>", 
                "commonmark");
        }

        [Fact]
        public void ATXHeadings_Spec40_commonmark()
        {
            // The following Markdown:
            //     foo
            //         # bar
            //
            // Should be rendered as:
            //     <p>foo
            //     # bar</p>

            SpecTestHelper.AssertCompliance("foo\n    # bar", 
                "<p>foo\n# bar</p>", 
                "commonmark");
        }

        // A closing sequence of `#` characters is optional:
        [Fact]
        public void ATXHeadings_Spec41_commonmark()
        {
            // The following Markdown:
            //     ## foo ##
            //       ###   bar    ###
            //
            // Should be rendered as:
            //     <h2>foo</h2>
            //     <h3>bar</h3>

            SpecTestHelper.AssertCompliance("## foo ##\n  ###   bar    ###", 
                "<h2>foo</h2>\n<h3>bar</h3>", 
                "commonmark");
        }

        // It need not be the same length as the opening sequence:
        [Fact]
        public void ATXHeadings_Spec42_commonmark()
        {
            // The following Markdown:
            //     # foo ##################################
            //     ##### foo ##
            //
            // Should be rendered as:
            //     <h1>foo</h1>
            //     <h5>foo</h5>

            SpecTestHelper.AssertCompliance("# foo ##################################\n##### foo ##", 
                "<h1>foo</h1>\n<h5>foo</h5>", 
                "commonmark");
        }

        // Spaces are allowed after the closing sequence:
        [Fact]
        public void ATXHeadings_Spec43_commonmark()
        {
            // The following Markdown:
            //     ### foo ###     
            //
            // Should be rendered as:
            //     <h3>foo</h3>

            SpecTestHelper.AssertCompliance("### foo ###     ", 
                "<h3>foo</h3>", 
                "commonmark");
        }

        // A sequence of `#` characters with anything but [spaces] following it
        // is not a closing sequence, but counts as part of the contents of the
        // heading:
        [Fact]
        public void ATXHeadings_Spec44_commonmark()
        {
            // The following Markdown:
            //     ### foo ### b
            //
            // Should be rendered as:
            //     <h3>foo ### b</h3>

            SpecTestHelper.AssertCompliance("### foo ### b", 
                "<h3>foo ### b</h3>", 
                "commonmark");
        }

        // The closing sequence must be preceded by a space:
        [Fact]
        public void ATXHeadings_Spec45_commonmark()
        {
            // The following Markdown:
            //     # foo#
            //
            // Should be rendered as:
            //     <h1>foo#</h1>

            SpecTestHelper.AssertCompliance("# foo#", 
                "<h1>foo#</h1>", 
                "commonmark");
        }

        // Backslash-escaped `#` characters do not count as part
        // of the closing sequence:
        [Fact]
        public void ATXHeadings_Spec46_commonmark()
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

            SpecTestHelper.AssertCompliance("### foo \\###\n## foo #\\##\n# foo \\#", 
                "<h3>foo ###</h3>\n<h2>foo ###</h2>\n<h1>foo #</h1>", 
                "commonmark");
        }

        // ATX headings need not be separated from surrounding content by blank
        // lines, and they can interrupt paragraphs:
        [Fact]
        public void ATXHeadings_Spec47_commonmark()
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

            SpecTestHelper.AssertCompliance("****\n## foo\n****", 
                "<hr />\n<h2>foo</h2>\n<hr />", 
                "commonmark");
        }

        [Fact]
        public void ATXHeadings_Spec48_commonmark()
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

            SpecTestHelper.AssertCompliance("Foo bar\n# baz\nBar foo", 
                "<p>Foo bar</p>\n<h1>baz</h1>\n<p>Bar foo</p>", 
                "commonmark");
        }

        // ATX headings can be empty:
        [Fact]
        public void ATXHeadings_Spec49_commonmark()
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

            SpecTestHelper.AssertCompliance("## \n#\n### ###", 
                "<h2></h2>\n<h1></h1>\n<h3></h3>", 
                "commonmark");
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
        public void SetextHeadings_Spec50_commonmark()
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

            SpecTestHelper.AssertCompliance("Foo *bar*\n=========\n\nFoo *bar*\n---------", 
                "<h1>Foo <em>bar</em></h1>\n<h2>Foo <em>bar</em></h2>", 
                "commonmark");
        }

        // The content of the header may span more than one line:
        [Fact]
        public void SetextHeadings_Spec51_commonmark()
        {
            // The following Markdown:
            //     Foo *bar
            //     baz*
            //     ====
            //
            // Should be rendered as:
            //     <h1>Foo <em>bar
            //     baz</em></h1>

            SpecTestHelper.AssertCompliance("Foo *bar\nbaz*\n====", 
                "<h1>Foo <em>bar\nbaz</em></h1>", 
                "commonmark");
        }

        // The underlining can be any length:
        [Fact]
        public void SetextHeadings_Spec52_commonmark()
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

            SpecTestHelper.AssertCompliance("Foo\n-------------------------\n\nFoo\n=", 
                "<h2>Foo</h2>\n<h1>Foo</h1>", 
                "commonmark");
        }

        // The heading content can be indented up to three spaces, and need
        // not line up with the underlining:
        [Fact]
        public void SetextHeadings_Spec53_commonmark()
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

            SpecTestHelper.AssertCompliance("   Foo\n---\n\n  Foo\n-----\n\n  Foo\n  ===", 
                "<h2>Foo</h2>\n<h2>Foo</h2>\n<h1>Foo</h1>", 
                "commonmark");
        }

        // Four spaces indent is too much:
        [Fact]
        public void SetextHeadings_Spec54_commonmark()
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

            SpecTestHelper.AssertCompliance("    Foo\n    ---\n\n    Foo\n---", 
                "<pre><code>Foo\n---\n\nFoo\n</code></pre>\n<hr />", 
                "commonmark");
        }

        // The setext heading underline can be indented up to three spaces, and
        // may have trailing spaces:
        [Fact]
        public void SetextHeadings_Spec55_commonmark()
        {
            // The following Markdown:
            //     Foo
            //        ----      
            //
            // Should be rendered as:
            //     <h2>Foo</h2>

            SpecTestHelper.AssertCompliance("Foo\n   ----      ", 
                "<h2>Foo</h2>", 
                "commonmark");
        }

        // Four spaces is too much:
        [Fact]
        public void SetextHeadings_Spec56_commonmark()
        {
            // The following Markdown:
            //     Foo
            //         ---
            //
            // Should be rendered as:
            //     <p>Foo
            //     ---</p>

            SpecTestHelper.AssertCompliance("Foo\n    ---", 
                "<p>Foo\n---</p>", 
                "commonmark");
        }

        // The setext heading underline cannot contain internal spaces:
        [Fact]
        public void SetextHeadings_Spec57_commonmark()
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

            SpecTestHelper.AssertCompliance("Foo\n= =\n\nFoo\n--- -", 
                "<p>Foo\n= =</p>\n<p>Foo</p>\n<hr />", 
                "commonmark");
        }

        // Trailing spaces in the content line do not cause a line break:
        [Fact]
        public void SetextHeadings_Spec58_commonmark()
        {
            // The following Markdown:
            //     Foo  
            //     -----
            //
            // Should be rendered as:
            //     <h2>Foo</h2>

            SpecTestHelper.AssertCompliance("Foo  \n-----", 
                "<h2>Foo</h2>", 
                "commonmark");
        }

        // Nor does a backslash at the end:
        [Fact]
        public void SetextHeadings_Spec59_commonmark()
        {
            // The following Markdown:
            //     Foo\
            //     ----
            //
            // Should be rendered as:
            //     <h2>Foo\</h2>

            SpecTestHelper.AssertCompliance("Foo\\\n----", 
                "<h2>Foo\\</h2>", 
                "commonmark");
        }

        // Since indicators of block structure take precedence over
        // indicators of inline structure, the following are setext headings:
        [Fact]
        public void SetextHeadings_Spec60_commonmark()
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

            SpecTestHelper.AssertCompliance("`Foo\n----\n`\n\n<a title=\"a lot\n---\nof dashes\"/>", 
                "<h2>`Foo</h2>\n<p>`</p>\n<h2>&lt;a title=&quot;a lot</h2>\n<p>of dashes&quot;/&gt;</p>", 
                "commonmark");
        }

        // The setext heading underline cannot be a [lazy continuation
        // line] in a list item or block quote:
        [Fact]
        public void SetextHeadings_Spec61_commonmark()
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

            SpecTestHelper.AssertCompliance("> Foo\n---", 
                "<blockquote>\n<p>Foo</p>\n</blockquote>\n<hr />", 
                "commonmark");
        }

        [Fact]
        public void SetextHeadings_Spec62_commonmark()
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

            SpecTestHelper.AssertCompliance("> foo\nbar\n===", 
                "<blockquote>\n<p>foo\nbar\n===</p>\n</blockquote>", 
                "commonmark");
        }

        [Fact]
        public void SetextHeadings_Spec63_commonmark()
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

            SpecTestHelper.AssertCompliance("- Foo\n---", 
                "<ul>\n<li>Foo</li>\n</ul>\n<hr />", 
                "commonmark");
        }

        // A blank line is needed between a paragraph and a following
        // setext heading, since otherwise the paragraph becomes part
        // of the heading's content:
        [Fact]
        public void SetextHeadings_Spec64_commonmark()
        {
            // The following Markdown:
            //     Foo
            //     Bar
            //     ---
            //
            // Should be rendered as:
            //     <h2>Foo
            //     Bar</h2>

            SpecTestHelper.AssertCompliance("Foo\nBar\n---", 
                "<h2>Foo\nBar</h2>", 
                "commonmark");
        }

        // But in general a blank line is not required before or after
        // setext headings:
        [Fact]
        public void SetextHeadings_Spec65_commonmark()
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

            SpecTestHelper.AssertCompliance("---\nFoo\n---\nBar\n---\nBaz", 
                "<hr />\n<h2>Foo</h2>\n<h2>Bar</h2>\n<p>Baz</p>", 
                "commonmark");
        }

        // Setext headings cannot be empty:
        [Fact]
        public void SetextHeadings_Spec66_commonmark()
        {
            // The following Markdown:
            //     
            //     ====
            //
            // Should be rendered as:
            //     <p>====</p>

            SpecTestHelper.AssertCompliance("\n====", 
                "<p>====</p>", 
                "commonmark");
        }

        // Setext heading text lines must not be interpretable as block
        // constructs other than paragraphs.  So, the line of dashes
        // in these examples gets interpreted as a thematic break:
        [Fact]
        public void SetextHeadings_Spec67_commonmark()
        {
            // The following Markdown:
            //     ---
            //     ---
            //
            // Should be rendered as:
            //     <hr />
            //     <hr />

            SpecTestHelper.AssertCompliance("---\n---", 
                "<hr />\n<hr />", 
                "commonmark");
        }

        [Fact]
        public void SetextHeadings_Spec68_commonmark()
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

            SpecTestHelper.AssertCompliance("- foo\n-----", 
                "<ul>\n<li>foo</li>\n</ul>\n<hr />", 
                "commonmark");
        }

        [Fact]
        public void SetextHeadings_Spec69_commonmark()
        {
            // The following Markdown:
            //         foo
            //     ---
            //
            // Should be rendered as:
            //     <pre><code>foo
            //     </code></pre>
            //     <hr />

            SpecTestHelper.AssertCompliance("    foo\n---", 
                "<pre><code>foo\n</code></pre>\n<hr />", 
                "commonmark");
        }

        [Fact]
        public void SetextHeadings_Spec70_commonmark()
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

            SpecTestHelper.AssertCompliance("> foo\n-----", 
                "<blockquote>\n<p>foo</p>\n</blockquote>\n<hr />", 
                "commonmark");
        }

        // If you want a heading with `> foo` as its literal text, you can
        // use backslash escapes:
        [Fact]
        public void SetextHeadings_Spec71_commonmark()
        {
            // The following Markdown:
            //     \> foo
            //     ------
            //
            // Should be rendered as:
            //     <h2>&gt; foo</h2>

            SpecTestHelper.AssertCompliance("\\> foo\n------", 
                "<h2>&gt; foo</h2>", 
                "commonmark");
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
        public void SetextHeadings_Spec72_commonmark()
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

            SpecTestHelper.AssertCompliance("Foo\n\nbar\n---\nbaz", 
                "<p>Foo</p>\n<h2>bar</h2>\n<p>baz</p>", 
                "commonmark");
        }

        // Authors who want interpretation 2 can put blank lines around
        // the thematic break,
        [Fact]
        public void SetextHeadings_Spec73_commonmark()
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

            SpecTestHelper.AssertCompliance("Foo\nbar\n\n---\n\nbaz", 
                "<p>Foo\nbar</p>\n<hr />\n<p>baz</p>", 
                "commonmark");
        }

        // or use a thematic break that cannot count as a [setext heading
        // underline], such as
        [Fact]
        public void SetextHeadings_Spec74_commonmark()
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

            SpecTestHelper.AssertCompliance("Foo\nbar\n* * *\nbaz", 
                "<p>Foo\nbar</p>\n<hr />\n<p>baz</p>", 
                "commonmark");
        }

        // Authors who want interpretation 3 can use backslash escapes:
        [Fact]
        public void SetextHeadings_Spec75_commonmark()
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

            SpecTestHelper.AssertCompliance("Foo\nbar\n\\---\nbaz", 
                "<p>Foo\nbar\n---\nbaz</p>", 
                "commonmark");
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
        public void IndentedCodeBlocks_Spec76_commonmark()
        {
            // The following Markdown:
            //         a simple
            //           indented code block
            //
            // Should be rendered as:
            //     <pre><code>a simple
            //       indented code block
            //     </code></pre>

            SpecTestHelper.AssertCompliance("    a simple\n      indented code block", 
                "<pre><code>a simple\n  indented code block\n</code></pre>", 
                "commonmark");
        }

        // If there is any ambiguity between an interpretation of indentation
        // as a code block and as indicating that material belongs to a [list
        // item][list items], the list item interpretation takes precedence:
        [Fact]
        public void IndentedCodeBlocks_Spec77_commonmark()
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

            SpecTestHelper.AssertCompliance("  - foo\n\n    bar", 
                "<ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>", 
                "commonmark");
        }

        [Fact]
        public void IndentedCodeBlocks_Spec78_commonmark()
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

            SpecTestHelper.AssertCompliance("1.  foo\n\n    - bar", 
                "<ol>\n<li>\n<p>foo</p>\n<ul>\n<li>bar</li>\n</ul>\n</li>\n</ol>", 
                "commonmark");
        }

        // The contents of a code block are literal text, and do not get parsed
        // as Markdown:
        [Fact]
        public void IndentedCodeBlocks_Spec79_commonmark()
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

            SpecTestHelper.AssertCompliance("    <a/>\n    *hi*\n\n    - one", 
                "<pre><code>&lt;a/&gt;\n*hi*\n\n- one\n</code></pre>", 
                "commonmark");
        }

        // Here we have three chunks separated by blank lines:
        [Fact]
        public void IndentedCodeBlocks_Spec80_commonmark()
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

            SpecTestHelper.AssertCompliance("    chunk1\n\n    chunk2\n  \n \n \n    chunk3", 
                "<pre><code>chunk1\n\nchunk2\n\n\n\nchunk3\n</code></pre>", 
                "commonmark");
        }

        // Any initial spaces beyond four will be included in the content, even
        // in interior blank lines:
        [Fact]
        public void IndentedCodeBlocks_Spec81_commonmark()
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

            SpecTestHelper.AssertCompliance("    chunk1\n      \n      chunk2", 
                "<pre><code>chunk1\n  \n  chunk2\n</code></pre>", 
                "commonmark");
        }

        // An indented code block cannot interrupt a paragraph.  (This
        // allows hanging indents and the like.)
        [Fact]
        public void IndentedCodeBlocks_Spec82_commonmark()
        {
            // The following Markdown:
            //     Foo
            //         bar
            //     
            //
            // Should be rendered as:
            //     <p>Foo
            //     bar</p>

            SpecTestHelper.AssertCompliance("Foo\n    bar\n", 
                "<p>Foo\nbar</p>", 
                "commonmark");
        }

        // However, any non-blank line with fewer than four leading spaces ends
        // the code block immediately.  So a paragraph may occur immediately
        // after indented code:
        [Fact]
        public void IndentedCodeBlocks_Spec83_commonmark()
        {
            // The following Markdown:
            //         foo
            //     bar
            //
            // Should be rendered as:
            //     <pre><code>foo
            //     </code></pre>
            //     <p>bar</p>

            SpecTestHelper.AssertCompliance("    foo\nbar", 
                "<pre><code>foo\n</code></pre>\n<p>bar</p>", 
                "commonmark");
        }

        // And indented code can occur immediately before and after other kinds of
        // blocks:
        [Fact]
        public void IndentedCodeBlocks_Spec84_commonmark()
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

            SpecTestHelper.AssertCompliance("# Heading\n    foo\nHeading\n------\n    foo\n----", 
                "<h1>Heading</h1>\n<pre><code>foo\n</code></pre>\n<h2>Heading</h2>\n<pre><code>foo\n</code></pre>\n<hr />", 
                "commonmark");
        }

        // The first line can be indented more than four spaces:
        [Fact]
        public void IndentedCodeBlocks_Spec85_commonmark()
        {
            // The following Markdown:
            //             foo
            //         bar
            //
            // Should be rendered as:
            //     <pre><code>    foo
            //     bar
            //     </code></pre>

            SpecTestHelper.AssertCompliance("        foo\n    bar", 
                "<pre><code>    foo\nbar\n</code></pre>", 
                "commonmark");
        }

        // Blank lines preceding or following an indented code block
        // are not included in it:
        [Fact]
        public void IndentedCodeBlocks_Spec86_commonmark()
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

            SpecTestHelper.AssertCompliance("\n    \n    foo\n    \n", 
                "<pre><code>foo\n</code></pre>", 
                "commonmark");
        }

        // Trailing spaces are included in the code block's content:
        [Fact]
        public void IndentedCodeBlocks_Spec87_commonmark()
        {
            // The following Markdown:
            //         foo  
            //
            // Should be rendered as:
            //     <pre><code>foo  
            //     </code></pre>

            SpecTestHelper.AssertCompliance("    foo  ", 
                "<pre><code>foo  \n</code></pre>", 
                "commonmark");
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
        public void FencedCodeBlocks_Spec88_commonmark()
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

            SpecTestHelper.AssertCompliance("```\n<\n >\n```", 
                "<pre><code>&lt;\n &gt;\n</code></pre>", 
                "commonmark");
        }

        // With tildes:
        [Fact]
        public void FencedCodeBlocks_Spec89_commonmark()
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

            SpecTestHelper.AssertCompliance("~~~\n<\n >\n~~~", 
                "<pre><code>&lt;\n &gt;\n</code></pre>", 
                "commonmark");
        }

        // Fewer than three backticks is not enough:
        [Fact]
        public void FencedCodeBlocks_Spec90_commonmark()
        {
            // The following Markdown:
            //     ``
            //     foo
            //     ``
            //
            // Should be rendered as:
            //     <p><code>foo</code></p>

            SpecTestHelper.AssertCompliance("``\nfoo\n``", 
                "<p><code>foo</code></p>", 
                "commonmark");
        }

        // The closing code fence must use the same character as the opening
        // fence:
        [Fact]
        public void FencedCodeBlocks_Spec91_commonmark()
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

            SpecTestHelper.AssertCompliance("```\naaa\n~~~\n```", 
                "<pre><code>aaa\n~~~\n</code></pre>", 
                "commonmark");
        }

        [Fact]
        public void FencedCodeBlocks_Spec92_commonmark()
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

            SpecTestHelper.AssertCompliance("~~~\naaa\n```\n~~~", 
                "<pre><code>aaa\n```\n</code></pre>", 
                "commonmark");
        }

        // The closing code fence must be at least as long as the opening fence:
        [Fact]
        public void FencedCodeBlocks_Spec93_commonmark()
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

            SpecTestHelper.AssertCompliance("````\naaa\n```\n``````", 
                "<pre><code>aaa\n```\n</code></pre>", 
                "commonmark");
        }

        [Fact]
        public void FencedCodeBlocks_Spec94_commonmark()
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

            SpecTestHelper.AssertCompliance("~~~~\naaa\n~~~\n~~~~", 
                "<pre><code>aaa\n~~~\n</code></pre>", 
                "commonmark");
        }

        // Unclosed code blocks are closed by the end of the document
        // (or the enclosing [block quote][block quotes] or [list item][list items]):
        [Fact]
        public void FencedCodeBlocks_Spec95_commonmark()
        {
            // The following Markdown:
            //     ```
            //
            // Should be rendered as:
            //     <pre><code></code></pre>

            SpecTestHelper.AssertCompliance("```", 
                "<pre><code></code></pre>", 
                "commonmark");
        }

        [Fact]
        public void FencedCodeBlocks_Spec96_commonmark()
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

            SpecTestHelper.AssertCompliance("`````\n\n```\naaa", 
                "<pre><code>\n```\naaa\n</code></pre>", 
                "commonmark");
        }

        [Fact]
        public void FencedCodeBlocks_Spec97_commonmark()
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

            SpecTestHelper.AssertCompliance("> ```\n> aaa\n\nbbb", 
                "<blockquote>\n<pre><code>aaa\n</code></pre>\n</blockquote>\n<p>bbb</p>", 
                "commonmark");
        }

        // A code block can have all empty lines as its content:
        [Fact]
        public void FencedCodeBlocks_Spec98_commonmark()
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

            SpecTestHelper.AssertCompliance("```\n\n  \n```", 
                "<pre><code>\n  \n</code></pre>", 
                "commonmark");
        }

        // A code block can be empty:
        [Fact]
        public void FencedCodeBlocks_Spec99_commonmark()
        {
            // The following Markdown:
            //     ```
            //     ```
            //
            // Should be rendered as:
            //     <pre><code></code></pre>

            SpecTestHelper.AssertCompliance("```\n```", 
                "<pre><code></code></pre>", 
                "commonmark");
        }

        // Fences can be indented.  If the opening fence is indented,
        // content lines will have equivalent opening indentation removed,
        // if present:
        [Fact]
        public void FencedCodeBlocks_Spec100_commonmark()
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

            SpecTestHelper.AssertCompliance(" ```\n aaa\naaa\n```", 
                "<pre><code>aaa\naaa\n</code></pre>", 
                "commonmark");
        }

        [Fact]
        public void FencedCodeBlocks_Spec101_commonmark()
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

            SpecTestHelper.AssertCompliance("  ```\naaa\n  aaa\naaa\n  ```", 
                "<pre><code>aaa\naaa\naaa\n</code></pre>", 
                "commonmark");
        }

        [Fact]
        public void FencedCodeBlocks_Spec102_commonmark()
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

            SpecTestHelper.AssertCompliance("   ```\n   aaa\n    aaa\n  aaa\n   ```", 
                "<pre><code>aaa\n aaa\naaa\n</code></pre>", 
                "commonmark");
        }

        // Four spaces indentation produces an indented code block:
        [Fact]
        public void FencedCodeBlocks_Spec103_commonmark()
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

            SpecTestHelper.AssertCompliance("    ```\n    aaa\n    ```", 
                "<pre><code>```\naaa\n```\n</code></pre>", 
                "commonmark");
        }

        // Closing fences may be indented by 0-3 spaces, and their indentation
        // need not match that of the opening fence:
        [Fact]
        public void FencedCodeBlocks_Spec104_commonmark()
        {
            // The following Markdown:
            //     ```
            //     aaa
            //       ```
            //
            // Should be rendered as:
            //     <pre><code>aaa
            //     </code></pre>

            SpecTestHelper.AssertCompliance("```\naaa\n  ```", 
                "<pre><code>aaa\n</code></pre>", 
                "commonmark");
        }

        [Fact]
        public void FencedCodeBlocks_Spec105_commonmark()
        {
            // The following Markdown:
            //        ```
            //     aaa
            //       ```
            //
            // Should be rendered as:
            //     <pre><code>aaa
            //     </code></pre>

            SpecTestHelper.AssertCompliance("   ```\naaa\n  ```", 
                "<pre><code>aaa\n</code></pre>", 
                "commonmark");
        }

        // This is not a closing fence, because it is indented 4 spaces:
        [Fact]
        public void FencedCodeBlocks_Spec106_commonmark()
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

            SpecTestHelper.AssertCompliance("```\naaa\n    ```", 
                "<pre><code>aaa\n    ```\n</code></pre>", 
                "commonmark");
        }

        // Code fences (opening and closing) cannot contain internal spaces:
        [Fact]
        public void FencedCodeBlocks_Spec107_commonmark()
        {
            // The following Markdown:
            //     ``` ```
            //     aaa
            //
            // Should be rendered as:
            //     <p><code></code>
            //     aaa</p>

            SpecTestHelper.AssertCompliance("``` ```\naaa", 
                "<p><code></code>\naaa</p>", 
                "commonmark");
        }

        [Fact]
        public void FencedCodeBlocks_Spec108_commonmark()
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

            SpecTestHelper.AssertCompliance("~~~~~~\naaa\n~~~ ~~", 
                "<pre><code>aaa\n~~~ ~~\n</code></pre>", 
                "commonmark");
        }

        // Fenced code blocks can interrupt paragraphs, and can be followed
        // directly by paragraphs, without a blank line between:
        [Fact]
        public void FencedCodeBlocks_Spec109_commonmark()
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

            SpecTestHelper.AssertCompliance("foo\n```\nbar\n```\nbaz", 
                "<p>foo</p>\n<pre><code>bar\n</code></pre>\n<p>baz</p>", 
                "commonmark");
        }

        // Other blocks can also occur before and after fenced code blocks
        // without an intervening blank line:
        [Fact]
        public void FencedCodeBlocks_Spec110_commonmark()
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

            SpecTestHelper.AssertCompliance("foo\n---\n~~~\nbar\n~~~\n# baz", 
                "<h2>foo</h2>\n<pre><code>bar\n</code></pre>\n<h1>baz</h1>", 
                "commonmark");
        }

        // An [info string] can be provided after the opening code fence.
        // Opening and closing spaces will be stripped, and the first word, prefixed
        // with `language-`, is used as the value for the `class` attribute of the
        // `code` element within the enclosing `pre` element.
        [Fact]
        public void FencedCodeBlocks_Spec111_commonmark()
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

            SpecTestHelper.AssertCompliance("```ruby\ndef foo(x)\n  return 3\nend\n```", 
                "<pre><code class=\"language-ruby\">def foo(x)\n  return 3\nend\n</code></pre>", 
                "commonmark");
        }

        [Fact]
        public void FencedCodeBlocks_Spec112_commonmark()
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

            SpecTestHelper.AssertCompliance("~~~~    ruby startline=3 $%@#$\ndef foo(x)\n  return 3\nend\n~~~~~~~", 
                "<pre><code class=\"language-ruby\">def foo(x)\n  return 3\nend\n</code></pre>", 
                "commonmark");
        }

        [Fact]
        public void FencedCodeBlocks_Spec113_commonmark()
        {
            // The following Markdown:
            //     ````;
            //     ````
            //
            // Should be rendered as:
            //     <pre><code class="language-;"></code></pre>

            SpecTestHelper.AssertCompliance("````;\n````", 
                "<pre><code class=\"language-;\"></code></pre>", 
                "commonmark");
        }

        // [Info strings] for backtick code blocks cannot contain backticks:
        [Fact]
        public void FencedCodeBlocks_Spec114_commonmark()
        {
            // The following Markdown:
            //     ``` aa ```
            //     foo
            //
            // Should be rendered as:
            //     <p><code>aa</code>
            //     foo</p>

            SpecTestHelper.AssertCompliance("``` aa ```\nfoo", 
                "<p><code>aa</code>\nfoo</p>", 
                "commonmark");
        }

        // Closing code fences cannot have [info strings]:
        [Fact]
        public void FencedCodeBlocks_Spec115_commonmark()
        {
            // The following Markdown:
            //     ```
            //     ``` aaa
            //     ```
            //
            // Should be rendered as:
            //     <pre><code>``` aaa
            //     </code></pre>

            SpecTestHelper.AssertCompliance("```\n``` aaa\n```", 
                "<pre><code>``` aaa\n</code></pre>", 
                "commonmark");
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
        public void HTMLBlocks_Spec116_commonmark()
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

            SpecTestHelper.AssertCompliance("<table><tr><td>\n<pre>\n**Hello**,\n\n_world_.\n</pre>\n</td></tr></table>", 
                "<table><tr><td>\n<pre>\n**Hello**,\n<p><em>world</em>.\n</pre></p>\n</td></tr></table>", 
                "commonmark");
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
        public void HTMLBlocks_Spec117_commonmark()
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

            SpecTestHelper.AssertCompliance("<table>\n  <tr>\n    <td>\n           hi\n    </td>\n  </tr>\n</table>\n\nokay.", 
                "<table>\n  <tr>\n    <td>\n           hi\n    </td>\n  </tr>\n</table>\n<p>okay.</p>", 
                "commonmark");
        }

        [Fact]
        public void HTMLBlocks_Spec118_commonmark()
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

            SpecTestHelper.AssertCompliance(" <div>\n  *hello*\n         <foo><a>", 
                " <div>\n  *hello*\n         <foo><a>", 
                "commonmark");
        }

        // A block can also start with a closing tag:
        [Fact]
        public void HTMLBlocks_Spec119_commonmark()
        {
            // The following Markdown:
            //     </div>
            //     *foo*
            //
            // Should be rendered as:
            //     </div>
            //     *foo*

            SpecTestHelper.AssertCompliance("</div>\n*foo*", 
                "</div>\n*foo*", 
                "commonmark");
        }

        // Here we have two HTML blocks with a Markdown paragraph between them:
        [Fact]
        public void HTMLBlocks_Spec120_commonmark()
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

            SpecTestHelper.AssertCompliance("<DIV CLASS=\"foo\">\n\n*Markdown*\n\n</DIV>", 
                "<DIV CLASS=\"foo\">\n<p><em>Markdown</em></p>\n</DIV>", 
                "commonmark");
        }

        // The tag on the first line can be partial, as long
        // as it is split where there would be whitespace:
        [Fact]
        public void HTMLBlocks_Spec121_commonmark()
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

            SpecTestHelper.AssertCompliance("<div id=\"foo\"\n  class=\"bar\">\n</div>", 
                "<div id=\"foo\"\n  class=\"bar\">\n</div>", 
                "commonmark");
        }

        [Fact]
        public void HTMLBlocks_Spec122_commonmark()
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

            SpecTestHelper.AssertCompliance("<div id=\"foo\" class=\"bar\n  baz\">\n</div>", 
                "<div id=\"foo\" class=\"bar\n  baz\">\n</div>", 
                "commonmark");
        }

        // An open tag need not be closed:
        [Fact]
        public void HTMLBlocks_Spec123_commonmark()
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

            SpecTestHelper.AssertCompliance("<div>\n*foo*\n\n*bar*", 
                "<div>\n*foo*\n<p><em>bar</em></p>", 
                "commonmark");
        }

        // A partial tag need not even be completed (garbage
        // in, garbage out):
        [Fact]
        public void HTMLBlocks_Spec124_commonmark()
        {
            // The following Markdown:
            //     <div id="foo"
            //     *hi*
            //
            // Should be rendered as:
            //     <div id="foo"
            //     *hi*

            SpecTestHelper.AssertCompliance("<div id=\"foo\"\n*hi*", 
                "<div id=\"foo\"\n*hi*", 
                "commonmark");
        }

        [Fact]
        public void HTMLBlocks_Spec125_commonmark()
        {
            // The following Markdown:
            //     <div class
            //     foo
            //
            // Should be rendered as:
            //     <div class
            //     foo

            SpecTestHelper.AssertCompliance("<div class\nfoo", 
                "<div class\nfoo", 
                "commonmark");
        }

        // The initial tag doesn't even need to be a valid
        // tag, as long as it starts like one:
        [Fact]
        public void HTMLBlocks_Spec126_commonmark()
        {
            // The following Markdown:
            //     <div *???-&&&-<---
            //     *foo*
            //
            // Should be rendered as:
            //     <div *???-&&&-<---
            //     *foo*

            SpecTestHelper.AssertCompliance("<div *???-&&&-<---\n*foo*", 
                "<div *???-&&&-<---\n*foo*", 
                "commonmark");
        }

        // In type 6 blocks, the initial tag need not be on a line by
        // itself:
        [Fact]
        public void HTMLBlocks_Spec127_commonmark()
        {
            // The following Markdown:
            //     <div><a href="bar">*foo*</a></div>
            //
            // Should be rendered as:
            //     <div><a href="bar">*foo*</a></div>

            SpecTestHelper.AssertCompliance("<div><a href=\"bar\">*foo*</a></div>", 
                "<div><a href=\"bar\">*foo*</a></div>", 
                "commonmark");
        }

        [Fact]
        public void HTMLBlocks_Spec128_commonmark()
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

            SpecTestHelper.AssertCompliance("<table><tr><td>\nfoo\n</td></tr></table>", 
                "<table><tr><td>\nfoo\n</td></tr></table>", 
                "commonmark");
        }

        // Everything until the next blank line or end of document
        // gets included in the HTML block.  So, in the following
        // example, what looks like a Markdown code block
        // is actually part of the HTML block, which continues until a blank
        // line or the end of the document is reached:
        [Fact]
        public void HTMLBlocks_Spec129_commonmark()
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

            SpecTestHelper.AssertCompliance("<div></div>\n``` c\nint x = 33;\n```", 
                "<div></div>\n``` c\nint x = 33;\n```", 
                "commonmark");
        }

        // To start an [HTML block] with a tag that is *not* in the
        // list of block-level tags in (6), you must put the tag by
        // itself on the first line (and it must be complete):
        [Fact]
        public void HTMLBlocks_Spec130_commonmark()
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

            SpecTestHelper.AssertCompliance("<a href=\"foo\">\n*bar*\n</a>", 
                "<a href=\"foo\">\n*bar*\n</a>", 
                "commonmark");
        }

        // In type 7 blocks, the [tag name] can be anything:
        [Fact]
        public void HTMLBlocks_Spec131_commonmark()
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

            SpecTestHelper.AssertCompliance("<Warning>\n*bar*\n</Warning>", 
                "<Warning>\n*bar*\n</Warning>", 
                "commonmark");
        }

        [Fact]
        public void HTMLBlocks_Spec132_commonmark()
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

            SpecTestHelper.AssertCompliance("<i class=\"foo\">\n*bar*\n</i>", 
                "<i class=\"foo\">\n*bar*\n</i>", 
                "commonmark");
        }

        [Fact]
        public void HTMLBlocks_Spec133_commonmark()
        {
            // The following Markdown:
            //     </ins>
            //     *bar*
            //
            // Should be rendered as:
            //     </ins>
            //     *bar*

            SpecTestHelper.AssertCompliance("</ins>\n*bar*", 
                "</ins>\n*bar*", 
                "commonmark");
        }

        // These rules are designed to allow us to work with tags that
        // can function as either block-level or inline-level tags.
        // The `<del>` tag is a nice example.  We can surround content with
        // `<del>` tags in three different ways.  In this case, we get a raw
        // HTML block, because the `<del>` tag is on a line by itself:
        [Fact]
        public void HTMLBlocks_Spec134_commonmark()
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

            SpecTestHelper.AssertCompliance("<del>\n*foo*\n</del>", 
                "<del>\n*foo*\n</del>", 
                "commonmark");
        }

        // In this case, we get a raw HTML block that just includes
        // the `<del>` tag (because it ends with the following blank
        // line).  So the contents get interpreted as CommonMark:
        [Fact]
        public void HTMLBlocks_Spec135_commonmark()
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

            SpecTestHelper.AssertCompliance("<del>\n\n*foo*\n\n</del>", 
                "<del>\n<p><em>foo</em></p>\n</del>", 
                "commonmark");
        }

        // Finally, in this case, the `<del>` tags are interpreted
        // as [raw HTML] *inside* the CommonMark paragraph.  (Because
        // the tag is not on a line by itself, we get inline HTML
        // rather than an [HTML block].)
        [Fact]
        public void HTMLBlocks_Spec136_commonmark()
        {
            // The following Markdown:
            //     <del>*foo*</del>
            //
            // Should be rendered as:
            //     <p><del><em>foo</em></del></p>

            SpecTestHelper.AssertCompliance("<del>*foo*</del>", 
                "<p><del><em>foo</em></del></p>", 
                "commonmark");
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
        public void HTMLBlocks_Spec137_commonmark()
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

            SpecTestHelper.AssertCompliance("<pre language=\"haskell\"><code>\nimport Text.HTML.TagSoup\n\nmain :: IO ()\nmain = print $ parseTags tags\n</code></pre>\nokay", 
                "<pre language=\"haskell\"><code>\nimport Text.HTML.TagSoup\n\nmain :: IO ()\nmain = print $ parseTags tags\n</code></pre>\n<p>okay</p>", 
                "commonmark");
        }

        // A script tag (type 1):
        [Fact]
        public void HTMLBlocks_Spec138_commonmark()
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

            SpecTestHelper.AssertCompliance("<script type=\"text/javascript\">\n// JavaScript example\n\ndocument.getElementById(\"demo\").innerHTML = \"Hello JavaScript!\";\n</script>\nokay", 
                "<script type=\"text/javascript\">\n// JavaScript example\n\ndocument.getElementById(\"demo\").innerHTML = \"Hello JavaScript!\";\n</script>\n<p>okay</p>", 
                "commonmark");
        }

        // A style tag (type 1):
        [Fact]
        public void HTMLBlocks_Spec139_commonmark()
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

            SpecTestHelper.AssertCompliance("<style\n  type=\"text/css\">\nh1 {color:red;}\n\np {color:blue;}\n</style>\nokay", 
                "<style\n  type=\"text/css\">\nh1 {color:red;}\n\np {color:blue;}\n</style>\n<p>okay</p>", 
                "commonmark");
        }

        // If there is no matching end tag, the block will end at the
        // end of the document (or the enclosing [block quote][block quotes]
        // or [list item][list items]):
        [Fact]
        public void HTMLBlocks_Spec140_commonmark()
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

            SpecTestHelper.AssertCompliance("<style\n  type=\"text/css\">\n\nfoo", 
                "<style\n  type=\"text/css\">\n\nfoo", 
                "commonmark");
        }

        [Fact]
        public void HTMLBlocks_Spec141_commonmark()
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

            SpecTestHelper.AssertCompliance("> <div>\n> foo\n\nbar", 
                "<blockquote>\n<div>\nfoo\n</blockquote>\n<p>bar</p>", 
                "commonmark");
        }

        [Fact]
        public void HTMLBlocks_Spec142_commonmark()
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

            SpecTestHelper.AssertCompliance("- <div>\n- foo", 
                "<ul>\n<li>\n<div>\n</li>\n<li>foo</li>\n</ul>", 
                "commonmark");
        }

        // The end tag can occur on the same line as the start tag:
        [Fact]
        public void HTMLBlocks_Spec143_commonmark()
        {
            // The following Markdown:
            //     <style>p{color:red;}</style>
            //     *foo*
            //
            // Should be rendered as:
            //     <style>p{color:red;}</style>
            //     <p><em>foo</em></p>

            SpecTestHelper.AssertCompliance("<style>p{color:red;}</style>\n*foo*", 
                "<style>p{color:red;}</style>\n<p><em>foo</em></p>", 
                "commonmark");
        }

        [Fact]
        public void HTMLBlocks_Spec144_commonmark()
        {
            // The following Markdown:
            //     <!-- foo -->*bar*
            //     *baz*
            //
            // Should be rendered as:
            //     <!-- foo -->*bar*
            //     <p><em>baz</em></p>

            SpecTestHelper.AssertCompliance("<!-- foo -->*bar*\n*baz*", 
                "<!-- foo -->*bar*\n<p><em>baz</em></p>", 
                "commonmark");
        }

        // Note that anything on the last line after the
        // end tag will be included in the [HTML block]:
        [Fact]
        public void HTMLBlocks_Spec145_commonmark()
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

            SpecTestHelper.AssertCompliance("<script>\nfoo\n</script>1. *bar*", 
                "<script>\nfoo\n</script>1. *bar*", 
                "commonmark");
        }

        // A comment (type 2):
        [Fact]
        public void HTMLBlocks_Spec146_commonmark()
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

            SpecTestHelper.AssertCompliance("<!-- Foo\n\nbar\n   baz -->\nokay", 
                "<!-- Foo\n\nbar\n   baz -->\n<p>okay</p>", 
                "commonmark");
        }

        // A processing instruction (type 3):
        [Fact]
        public void HTMLBlocks_Spec147_commonmark()
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

            SpecTestHelper.AssertCompliance("<?php\n\n  echo '>';\n\n?>\nokay", 
                "<?php\n\n  echo '>';\n\n?>\n<p>okay</p>", 
                "commonmark");
        }

        // A declaration (type 4):
        [Fact]
        public void HTMLBlocks_Spec148_commonmark()
        {
            // The following Markdown:
            //     <!DOCTYPE html>
            //
            // Should be rendered as:
            //     <!DOCTYPE html>

            SpecTestHelper.AssertCompliance("<!DOCTYPE html>", 
                "<!DOCTYPE html>", 
                "commonmark");
        }

        // CDATA (type 5):
        [Fact]
        public void HTMLBlocks_Spec149_commonmark()
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

            SpecTestHelper.AssertCompliance("<![CDATA[\nfunction matchwo(a,b)\n{\n  if (a < b && a < 0) then {\n    return 1;\n\n  } else {\n\n    return 0;\n  }\n}\n]]>\nokay", 
                "<![CDATA[\nfunction matchwo(a,b)\n{\n  if (a < b && a < 0) then {\n    return 1;\n\n  } else {\n\n    return 0;\n  }\n}\n]]>\n<p>okay</p>", 
                "commonmark");
        }

        // The opening tag can be indented 1-3 spaces, but not 4:
        [Fact]
        public void HTMLBlocks_Spec150_commonmark()
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

            SpecTestHelper.AssertCompliance("  <!-- foo -->\n\n    <!-- foo -->", 
                "  <!-- foo -->\n<pre><code>&lt;!-- foo --&gt;\n</code></pre>", 
                "commonmark");
        }

        [Fact]
        public void HTMLBlocks_Spec151_commonmark()
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

            SpecTestHelper.AssertCompliance("  <div>\n\n    <div>", 
                "  <div>\n<pre><code>&lt;div&gt;\n</code></pre>", 
                "commonmark");
        }

        // An HTML block of types 1--6 can interrupt a paragraph, and need not be
        // preceded by a blank line.
        [Fact]
        public void HTMLBlocks_Spec152_commonmark()
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

            SpecTestHelper.AssertCompliance("Foo\n<div>\nbar\n</div>", 
                "<p>Foo</p>\n<div>\nbar\n</div>", 
                "commonmark");
        }

        // However, a following blank line is needed, except at the end of
        // a document, and except for blocks of types 1--5, above:
        [Fact]
        public void HTMLBlocks_Spec153_commonmark()
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

            SpecTestHelper.AssertCompliance("<div>\nbar\n</div>\n*foo*", 
                "<div>\nbar\n</div>\n*foo*", 
                "commonmark");
        }

        // HTML blocks of type 7 cannot interrupt a paragraph:
        [Fact]
        public void HTMLBlocks_Spec154_commonmark()
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

            SpecTestHelper.AssertCompliance("Foo\n<a href=\"bar\">\nbaz", 
                "<p>Foo\n<a href=\"bar\">\nbaz</p>", 
                "commonmark");
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
        public void HTMLBlocks_Spec155_commonmark()
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

            SpecTestHelper.AssertCompliance("<div>\n\n*Emphasized* text.\n\n</div>", 
                "<div>\n<p><em>Emphasized</em> text.</p>\n</div>", 
                "commonmark");
        }

        [Fact]
        public void HTMLBlocks_Spec156_commonmark()
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

            SpecTestHelper.AssertCompliance("<div>\n*Emphasized* text.\n</div>", 
                "<div>\n*Emphasized* text.\n</div>", 
                "commonmark");
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
        public void HTMLBlocks_Spec157_commonmark()
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

            SpecTestHelper.AssertCompliance("<table>\n\n<tr>\n\n<td>\nHi\n</td>\n\n</tr>\n\n</table>", 
                "<table>\n<tr>\n<td>\nHi\n</td>\n</tr>\n</table>", 
                "commonmark");
        }

        // There are problems, however, if the inner tags are indented
        // *and* separated by spaces, as then they will be interpreted as
        // an indented code block:
        [Fact]
        public void HTMLBlocks_Spec158_commonmark()
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

            SpecTestHelper.AssertCompliance("<table>\n\n  <tr>\n\n    <td>\n      Hi\n    </td>\n\n  </tr>\n\n</table>", 
                "<table>\n  <tr>\n<pre><code>&lt;td&gt;\n  Hi\n&lt;/td&gt;\n</code></pre>\n  </tr>\n</table>", 
                "commonmark");
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
        public void LinkReferenceDefinitions_Spec159_commonmark()
        {
            // The following Markdown:
            //     [foo]: /url "title"
            //     
            //     [foo]
            //
            // Should be rendered as:
            //     <p><a href="/url" title="title">foo</a></p>

            SpecTestHelper.AssertCompliance("[foo]: /url \"title\"\n\n[foo]", 
                "<p><a href=\"/url\" title=\"title\">foo</a></p>", 
                "commonmark");
        }

        [Fact]
        public void LinkReferenceDefinitions_Spec160_commonmark()
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

            SpecTestHelper.AssertCompliance("   [foo]: \n      /url  \n           'the title'  \n\n[foo]", 
                "<p><a href=\"/url\" title=\"the title\">foo</a></p>", 
                "commonmark");
        }

        [Fact]
        public void LinkReferenceDefinitions_Spec161_commonmark()
        {
            // The following Markdown:
            //     [Foo*bar\]]:my_(url) 'title (with parens)'
            //     
            //     [Foo*bar\]]
            //
            // Should be rendered as:
            //     <p><a href="my_(url)" title="title (with parens)">Foo*bar]</a></p>

            SpecTestHelper.AssertCompliance("[Foo*bar\\]]:my_(url) 'title (with parens)'\n\n[Foo*bar\\]]", 
                "<p><a href=\"my_(url)\" title=\"title (with parens)\">Foo*bar]</a></p>", 
                "commonmark");
        }

        [Fact]
        public void LinkReferenceDefinitions_Spec162_commonmark()
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

            SpecTestHelper.AssertCompliance("[Foo bar]:\n<my%20url>\n'title'\n\n[Foo bar]", 
                "<p><a href=\"my%20url\" title=\"title\">Foo bar</a></p>", 
                "commonmark");
        }

        // The title may extend over multiple lines:
        [Fact]
        public void LinkReferenceDefinitions_Spec163_commonmark()
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

            SpecTestHelper.AssertCompliance("[foo]: /url '\ntitle\nline1\nline2\n'\n\n[foo]", 
                "<p><a href=\"/url\" title=\"\ntitle\nline1\nline2\n\">foo</a></p>", 
                "commonmark");
        }

        // However, it may not contain a [blank line]:
        [Fact]
        public void LinkReferenceDefinitions_Spec164_commonmark()
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

            SpecTestHelper.AssertCompliance("[foo]: /url 'title\n\nwith blank line'\n\n[foo]", 
                "<p>[foo]: /url 'title</p>\n<p>with blank line'</p>\n<p>[foo]</p>", 
                "commonmark");
        }

        // The title may be omitted:
        [Fact]
        public void LinkReferenceDefinitions_Spec165_commonmark()
        {
            // The following Markdown:
            //     [foo]:
            //     /url
            //     
            //     [foo]
            //
            // Should be rendered as:
            //     <p><a href="/url">foo</a></p>

            SpecTestHelper.AssertCompliance("[foo]:\n/url\n\n[foo]", 
                "<p><a href=\"/url\">foo</a></p>", 
                "commonmark");
        }

        // The link destination may not be omitted:
        [Fact]
        public void LinkReferenceDefinitions_Spec166_commonmark()
        {
            // The following Markdown:
            //     [foo]:
            //     
            //     [foo]
            //
            // Should be rendered as:
            //     <p>[foo]:</p>
            //     <p>[foo]</p>

            SpecTestHelper.AssertCompliance("[foo]:\n\n[foo]", 
                "<p>[foo]:</p>\n<p>[foo]</p>", 
                "commonmark");
        }

        // Both title and destination can contain backslash escapes
        // and literal backslashes:
        [Fact]
        public void LinkReferenceDefinitions_Spec167_commonmark()
        {
            // The following Markdown:
            //     [foo]: /url\bar\*baz "foo\"bar\baz"
            //     
            //     [foo]
            //
            // Should be rendered as:
            //     <p><a href="/url%5Cbar*baz" title="foo&quot;bar\baz">foo</a></p>

            SpecTestHelper.AssertCompliance("[foo]: /url\\bar\\*baz \"foo\\\"bar\\baz\"\n\n[foo]", 
                "<p><a href=\"/url%5Cbar*baz\" title=\"foo&quot;bar\\baz\">foo</a></p>", 
                "commonmark");
        }

        // A link can come before its corresponding definition:
        [Fact]
        public void LinkReferenceDefinitions_Spec168_commonmark()
        {
            // The following Markdown:
            //     [foo]
            //     
            //     [foo]: url
            //
            // Should be rendered as:
            //     <p><a href="url">foo</a></p>

            SpecTestHelper.AssertCompliance("[foo]\n\n[foo]: url", 
                "<p><a href=\"url\">foo</a></p>", 
                "commonmark");
        }

        // If there are several matching definitions, the first one takes
        // precedence:
        [Fact]
        public void LinkReferenceDefinitions_Spec169_commonmark()
        {
            // The following Markdown:
            //     [foo]
            //     
            //     [foo]: first
            //     [foo]: second
            //
            // Should be rendered as:
            //     <p><a href="first">foo</a></p>

            SpecTestHelper.AssertCompliance("[foo]\n\n[foo]: first\n[foo]: second", 
                "<p><a href=\"first\">foo</a></p>", 
                "commonmark");
        }

        // As noted in the section on [Links], matching of labels is
        // case-insensitive (see [matches]).
        [Fact]
        public void LinkReferenceDefinitions_Spec170_commonmark()
        {
            // The following Markdown:
            //     [FOO]: /url
            //     
            //     [Foo]
            //
            // Should be rendered as:
            //     <p><a href="/url">Foo</a></p>

            SpecTestHelper.AssertCompliance("[FOO]: /url\n\n[Foo]", 
                "<p><a href=\"/url\">Foo</a></p>", 
                "commonmark");
        }

        [Fact]
        public void LinkReferenceDefinitions_Spec171_commonmark()
        {
            // The following Markdown:
            //     [ΑΓΩ]: /φου
            //     
            //     [αγω]
            //
            // Should be rendered as:
            //     <p><a href="/%CF%86%CE%BF%CF%85">αγω</a></p>

            SpecTestHelper.AssertCompliance("[ΑΓΩ]: /φου\n\n[αγω]", 
                "<p><a href=\"/%CF%86%CE%BF%CF%85\">αγω</a></p>", 
                "commonmark");
        }

        // Here is a link reference definition with no corresponding link.
        // It contributes nothing to the document.
        [Fact]
        public void LinkReferenceDefinitions_Spec172_commonmark()
        {
            // The following Markdown:
            //     [foo]: /url
            //
            // Should be rendered as:


            SpecTestHelper.AssertCompliance("[foo]: /url", 
                "", 
                "commonmark");
        }

        // Here is another one:
        [Fact]
        public void LinkReferenceDefinitions_Spec173_commonmark()
        {
            // The following Markdown:
            //     [
            //     foo
            //     ]: /url
            //     bar
            //
            // Should be rendered as:
            //     <p>bar</p>

            SpecTestHelper.AssertCompliance("[\nfoo\n]: /url\nbar", 
                "<p>bar</p>", 
                "commonmark");
        }

        // This is not a link reference definition, because there are
        // [non-whitespace characters] after the title:
        [Fact]
        public void LinkReferenceDefinitions_Spec174_commonmark()
        {
            // The following Markdown:
            //     [foo]: /url "title" ok
            //
            // Should be rendered as:
            //     <p>[foo]: /url &quot;title&quot; ok</p>

            SpecTestHelper.AssertCompliance("[foo]: /url \"title\" ok", 
                "<p>[foo]: /url &quot;title&quot; ok</p>", 
                "commonmark");
        }

        // This is a link reference definition, but it has no title:
        [Fact]
        public void LinkReferenceDefinitions_Spec175_commonmark()
        {
            // The following Markdown:
            //     [foo]: /url
            //     "title" ok
            //
            // Should be rendered as:
            //     <p>&quot;title&quot; ok</p>

            SpecTestHelper.AssertCompliance("[foo]: /url\n\"title\" ok", 
                "<p>&quot;title&quot; ok</p>", 
                "commonmark");
        }

        // This is not a link reference definition, because it is indented
        // four spaces:
        [Fact]
        public void LinkReferenceDefinitions_Spec176_commonmark()
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

            SpecTestHelper.AssertCompliance("    [foo]: /url \"title\"\n\n[foo]", 
                "<pre><code>[foo]: /url &quot;title&quot;\n</code></pre>\n<p>[foo]</p>", 
                "commonmark");
        }

        // This is not a link reference definition, because it occurs inside
        // a code block:
        [Fact]
        public void LinkReferenceDefinitions_Spec177_commonmark()
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

            SpecTestHelper.AssertCompliance("```\n[foo]: /url\n```\n\n[foo]", 
                "<pre><code>[foo]: /url\n</code></pre>\n<p>[foo]</p>", 
                "commonmark");
        }

        // A [link reference definition] cannot interrupt a paragraph.
        [Fact]
        public void LinkReferenceDefinitions_Spec178_commonmark()
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

            SpecTestHelper.AssertCompliance("Foo\n[bar]: /baz\n\n[bar]", 
                "<p>Foo\n[bar]: /baz</p>\n<p>[bar]</p>", 
                "commonmark");
        }

        // However, it can directly follow other block elements, such as headings
        // and thematic breaks, and it need not be followed by a blank line.
        [Fact]
        public void LinkReferenceDefinitions_Spec179_commonmark()
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

            SpecTestHelper.AssertCompliance("# [Foo]\n[foo]: /url\n> bar", 
                "<h1><a href=\"/url\">Foo</a></h1>\n<blockquote>\n<p>bar</p>\n</blockquote>", 
                "commonmark");
        }

        // Several [link reference definitions]
        // can occur one after another, without intervening blank lines.
        [Fact]
        public void LinkReferenceDefinitions_Spec180_commonmark()
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

            SpecTestHelper.AssertCompliance("[foo]: /foo-url \"foo\"\n[bar]: /bar-url\n  \"bar\"\n[baz]: /baz-url\n\n[foo],\n[bar],\n[baz]", 
                "<p><a href=\"/foo-url\" title=\"foo\">foo</a>,\n<a href=\"/bar-url\" title=\"bar\">bar</a>,\n<a href=\"/baz-url\">baz</a></p>", 
                "commonmark");
        }

        // [Link reference definitions] can occur
        // inside block containers, like lists and block quotations.  They
        // affect the entire document, not just the container in which they
        // are defined:
        [Fact]
        public void LinkReferenceDefinitions_Spec181_commonmark()
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

            SpecTestHelper.AssertCompliance("[foo]\n\n> [foo]: /url", 
                "<p><a href=\"/url\">foo</a></p>\n<blockquote>\n</blockquote>", 
                "commonmark");
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
        public void Paragraphs_Spec182_commonmark()
        {
            // The following Markdown:
            //     aaa
            //     
            //     bbb
            //
            // Should be rendered as:
            //     <p>aaa</p>
            //     <p>bbb</p>

            SpecTestHelper.AssertCompliance("aaa\n\nbbb", 
                "<p>aaa</p>\n<p>bbb</p>", 
                "commonmark");
        }

        // Paragraphs can contain multiple lines, but no blank lines:
        [Fact]
        public void Paragraphs_Spec183_commonmark()
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

            SpecTestHelper.AssertCompliance("aaa\nbbb\n\nccc\nddd", 
                "<p>aaa\nbbb</p>\n<p>ccc\nddd</p>", 
                "commonmark");
        }

        // Multiple blank lines between paragraph have no effect:
        [Fact]
        public void Paragraphs_Spec184_commonmark()
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

            SpecTestHelper.AssertCompliance("aaa\n\n\nbbb", 
                "<p>aaa</p>\n<p>bbb</p>", 
                "commonmark");
        }

        // Leading spaces are skipped:
        [Fact]
        public void Paragraphs_Spec185_commonmark()
        {
            // The following Markdown:
            //       aaa
            //      bbb
            //
            // Should be rendered as:
            //     <p>aaa
            //     bbb</p>

            SpecTestHelper.AssertCompliance("  aaa\n bbb", 
                "<p>aaa\nbbb</p>", 
                "commonmark");
        }

        // Lines after the first may be indented any amount, since indented
        // code blocks cannot interrupt paragraphs.
        [Fact]
        public void Paragraphs_Spec186_commonmark()
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

            SpecTestHelper.AssertCompliance("aaa\n             bbb\n                                       ccc", 
                "<p>aaa\nbbb\nccc</p>", 
                "commonmark");
        }

        // However, the first line may be indented at most three spaces,
        // or an indented code block will be triggered:
        [Fact]
        public void Paragraphs_Spec187_commonmark()
        {
            // The following Markdown:
            //        aaa
            //     bbb
            //
            // Should be rendered as:
            //     <p>aaa
            //     bbb</p>

            SpecTestHelper.AssertCompliance("   aaa\nbbb", 
                "<p>aaa\nbbb</p>", 
                "commonmark");
        }

        [Fact]
        public void Paragraphs_Spec188_commonmark()
        {
            // The following Markdown:
            //         aaa
            //     bbb
            //
            // Should be rendered as:
            //     <pre><code>aaa
            //     </code></pre>
            //     <p>bbb</p>

            SpecTestHelper.AssertCompliance("    aaa\nbbb", 
                "<pre><code>aaa\n</code></pre>\n<p>bbb</p>", 
                "commonmark");
        }

        // Final spaces are stripped before inline parsing, so a paragraph
        // that ends with two or more spaces will not end with a [hard line
        // break]:
        [Fact]
        public void Paragraphs_Spec189_commonmark()
        {
            // The following Markdown:
            //     aaa     
            //     bbb     
            //
            // Should be rendered as:
            //     <p>aaa<br />
            //     bbb</p>

            SpecTestHelper.AssertCompliance("aaa     \nbbb     ", 
                "<p>aaa<br />\nbbb</p>", 
                "commonmark");
        }
    }

    // [Blank lines] between block-level elements are ignored,
    // except for the role they play in determining whether a [list]
    // is [tight] or [loose]
    public class BlankLinesTests
    {
        // Blank lines at the beginning and end of the document are also ignored.
        [Fact]
        public void BlankLines_Spec190_commonmark()
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

            SpecTestHelper.AssertCompliance("  \n\naaa\n  \n\n# aaa\n\n  ", 
                "<p>aaa</p>\n<h1>aaa</h1>", 
                "commonmark");
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
        public void BlockQuotes_Spec191_commonmark()
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

            SpecTestHelper.AssertCompliance("> # Foo\n> bar\n> baz", 
                "<blockquote>\n<h1>Foo</h1>\n<p>bar\nbaz</p>\n</blockquote>", 
                "commonmark");
        }

        // The spaces after the `>` characters can be omitted:
        [Fact]
        public void BlockQuotes_Spec192_commonmark()
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

            SpecTestHelper.AssertCompliance("># Foo\n>bar\n> baz", 
                "<blockquote>\n<h1>Foo</h1>\n<p>bar\nbaz</p>\n</blockquote>", 
                "commonmark");
        }

        // The `>` characters can be indented 1-3 spaces:
        [Fact]
        public void BlockQuotes_Spec193_commonmark()
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

            SpecTestHelper.AssertCompliance("   > # Foo\n   > bar\n > baz", 
                "<blockquote>\n<h1>Foo</h1>\n<p>bar\nbaz</p>\n</blockquote>", 
                "commonmark");
        }

        // Four spaces gives us a code block:
        [Fact]
        public void BlockQuotes_Spec194_commonmark()
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

            SpecTestHelper.AssertCompliance("    > # Foo\n    > bar\n    > baz", 
                "<pre><code>&gt; # Foo\n&gt; bar\n&gt; baz\n</code></pre>", 
                "commonmark");
        }

        // The Laziness clause allows us to omit the `>` before
        // [paragraph continuation text]:
        [Fact]
        public void BlockQuotes_Spec195_commonmark()
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

            SpecTestHelper.AssertCompliance("> # Foo\n> bar\nbaz", 
                "<blockquote>\n<h1>Foo</h1>\n<p>bar\nbaz</p>\n</blockquote>", 
                "commonmark");
        }

        // A block quote can contain some lazy and some non-lazy
        // continuation lines:
        [Fact]
        public void BlockQuotes_Spec196_commonmark()
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

            SpecTestHelper.AssertCompliance("> bar\nbaz\n> foo", 
                "<blockquote>\n<p>bar\nbaz\nfoo</p>\n</blockquote>", 
                "commonmark");
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
        public void BlockQuotes_Spec197_commonmark()
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

            SpecTestHelper.AssertCompliance("> foo\n---", 
                "<blockquote>\n<p>foo</p>\n</blockquote>\n<hr />", 
                "commonmark");
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
        public void BlockQuotes_Spec198_commonmark()
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

            SpecTestHelper.AssertCompliance("> - foo\n- bar", 
                "<blockquote>\n<ul>\n<li>foo</li>\n</ul>\n</blockquote>\n<ul>\n<li>bar</li>\n</ul>", 
                "commonmark");
        }

        // For the same reason, we can't omit the `> ` in front of
        // subsequent lines of an indented or fenced code block:
        [Fact]
        public void BlockQuotes_Spec199_commonmark()
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

            SpecTestHelper.AssertCompliance(">     foo\n    bar", 
                "<blockquote>\n<pre><code>foo\n</code></pre>\n</blockquote>\n<pre><code>bar\n</code></pre>", 
                "commonmark");
        }

        [Fact]
        public void BlockQuotes_Spec200_commonmark()
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

            SpecTestHelper.AssertCompliance("> ```\nfoo\n```", 
                "<blockquote>\n<pre><code></code></pre>\n</blockquote>\n<p>foo</p>\n<pre><code></code></pre>", 
                "commonmark");
        }

        // Note that in the following case, we have a [lazy
        // continuation line]:
        [Fact]
        public void BlockQuotes_Spec201_commonmark()
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

            SpecTestHelper.AssertCompliance("> foo\n    - bar", 
                "<blockquote>\n<p>foo\n- bar</p>\n</blockquote>", 
                "commonmark");
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
        public void BlockQuotes_Spec202_commonmark()
        {
            // The following Markdown:
            //     >
            //
            // Should be rendered as:
            //     <blockquote>
            //     </blockquote>

            SpecTestHelper.AssertCompliance(">", 
                "<blockquote>\n</blockquote>", 
                "commonmark");
        }

        [Fact]
        public void BlockQuotes_Spec203_commonmark()
        {
            // The following Markdown:
            //     >
            //     >  
            //     > 
            //
            // Should be rendered as:
            //     <blockquote>
            //     </blockquote>

            SpecTestHelper.AssertCompliance(">\n>  \n> ", 
                "<blockquote>\n</blockquote>", 
                "commonmark");
        }

        // A block quote can have initial or final blank lines:
        [Fact]
        public void BlockQuotes_Spec204_commonmark()
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

            SpecTestHelper.AssertCompliance(">\n> foo\n>  ", 
                "<blockquote>\n<p>foo</p>\n</blockquote>", 
                "commonmark");
        }

        // A blank line always separates block quotes:
        [Fact]
        public void BlockQuotes_Spec205_commonmark()
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

            SpecTestHelper.AssertCompliance("> foo\n\n> bar", 
                "<blockquote>\n<p>foo</p>\n</blockquote>\n<blockquote>\n<p>bar</p>\n</blockquote>", 
                "commonmark");
        }

        // (Most current Markdown implementations, including John Gruber's
        // original `Markdown.pl`, will parse this example as a single block quote
        // with two paragraphs.  But it seems better to allow the author to decide
        // whether two block quotes or one are wanted.)
        // 
        // Consecutiveness means that if we put these block quotes together,
        // we get a single block quote:
        [Fact]
        public void BlockQuotes_Spec206_commonmark()
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

            SpecTestHelper.AssertCompliance("> foo\n> bar", 
                "<blockquote>\n<p>foo\nbar</p>\n</blockquote>", 
                "commonmark");
        }

        // To get a block quote with two paragraphs, use:
        [Fact]
        public void BlockQuotes_Spec207_commonmark()
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

            SpecTestHelper.AssertCompliance("> foo\n>\n> bar", 
                "<blockquote>\n<p>foo</p>\n<p>bar</p>\n</blockquote>", 
                "commonmark");
        }

        // Block quotes can interrupt paragraphs:
        [Fact]
        public void BlockQuotes_Spec208_commonmark()
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

            SpecTestHelper.AssertCompliance("foo\n> bar", 
                "<p>foo</p>\n<blockquote>\n<p>bar</p>\n</blockquote>", 
                "commonmark");
        }

        // In general, blank lines are not needed before or after block
        // quotes:
        [Fact]
        public void BlockQuotes_Spec209_commonmark()
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

            SpecTestHelper.AssertCompliance("> aaa\n***\n> bbb", 
                "<blockquote>\n<p>aaa</p>\n</blockquote>\n<hr />\n<blockquote>\n<p>bbb</p>\n</blockquote>", 
                "commonmark");
        }

        // However, because of laziness, a blank line is needed between
        // a block quote and a following paragraph:
        [Fact]
        public void BlockQuotes_Spec210_commonmark()
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

            SpecTestHelper.AssertCompliance("> bar\nbaz", 
                "<blockquote>\n<p>bar\nbaz</p>\n</blockquote>", 
                "commonmark");
        }

        [Fact]
        public void BlockQuotes_Spec211_commonmark()
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

            SpecTestHelper.AssertCompliance("> bar\n\nbaz", 
                "<blockquote>\n<p>bar</p>\n</blockquote>\n<p>baz</p>", 
                "commonmark");
        }

        [Fact]
        public void BlockQuotes_Spec212_commonmark()
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

            SpecTestHelper.AssertCompliance("> bar\n>\nbaz", 
                "<blockquote>\n<p>bar</p>\n</blockquote>\n<p>baz</p>", 
                "commonmark");
        }

        // It is a consequence of the Laziness rule that any number
        // of initial `>`s may be omitted on a continuation line of a
        // nested block quote:
        [Fact]
        public void BlockQuotes_Spec213_commonmark()
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

            SpecTestHelper.AssertCompliance("> > > foo\nbar", 
                "<blockquote>\n<blockquote>\n<blockquote>\n<p>foo\nbar</p>\n</blockquote>\n</blockquote>\n</blockquote>", 
                "commonmark");
        }

        [Fact]
        public void BlockQuotes_Spec214_commonmark()
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

            SpecTestHelper.AssertCompliance(">>> foo\n> bar\n>>baz", 
                "<blockquote>\n<blockquote>\n<blockquote>\n<p>foo\nbar\nbaz</p>\n</blockquote>\n</blockquote>\n</blockquote>", 
                "commonmark");
        }

        // When including an indented code block in a block quote,
        // remember that the [block quote marker] includes
        // both the `>` and a following space.  So *five spaces* are needed after
        // the `>`:
        [Fact]
        public void BlockQuotes_Spec215_commonmark()
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

            SpecTestHelper.AssertCompliance(">     code\n\n>    not code", 
                "<blockquote>\n<pre><code>code\n</code></pre>\n</blockquote>\n<blockquote>\n<p>not code</p>\n</blockquote>", 
                "commonmark");
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
        public void ListItems_Spec216_commonmark()
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

            SpecTestHelper.AssertCompliance("A paragraph\nwith two lines.\n\n    indented code\n\n> A block quote.", 
                "<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>", 
                "commonmark");
        }

        // And let *M* be the marker `1.`, and *N* = 2.  Then rule #1 says
        // that the following is an ordered list item with start number 1,
        // and the same contents as *Ls*:
        [Fact]
        public void ListItems_Spec217_commonmark()
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

            SpecTestHelper.AssertCompliance("1.  A paragraph\n    with two lines.\n\n        indented code\n\n    > A block quote.", 
                "<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>", 
                "commonmark");
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
        public void ListItems_Spec218_commonmark()
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

            SpecTestHelper.AssertCompliance("- one\n\n two", 
                "<ul>\n<li>one</li>\n</ul>\n<p>two</p>", 
                "commonmark");
        }

        [Fact]
        public void ListItems_Spec219_commonmark()
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

            SpecTestHelper.AssertCompliance("- one\n\n  two", 
                "<ul>\n<li>\n<p>one</p>\n<p>two</p>\n</li>\n</ul>", 
                "commonmark");
        }

        [Fact]
        public void ListItems_Spec220_commonmark()
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

            SpecTestHelper.AssertCompliance(" -    one\n\n     two", 
                "<ul>\n<li>one</li>\n</ul>\n<pre><code> two\n</code></pre>", 
                "commonmark");
        }

        [Fact]
        public void ListItems_Spec221_commonmark()
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

            SpecTestHelper.AssertCompliance(" -    one\n\n      two", 
                "<ul>\n<li>\n<p>one</p>\n<p>two</p>\n</li>\n</ul>", 
                "commonmark");
        }

        // It is tempting to think of this in terms of columns:  the continuation
        // blocks must be indented at least to the column of the first
        // [non-whitespace character] after the list marker. However, that is not quite right.
        // The spaces after the list marker determine how much relative indentation
        // is needed.  Which column this indentation reaches will depend on
        // how the list item is embedded in other constructions, as shown by
        // this example:
        [Fact]
        public void ListItems_Spec222_commonmark()
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

            SpecTestHelper.AssertCompliance("   > > 1.  one\n>>\n>>     two", 
                "<blockquote>\n<blockquote>\n<ol>\n<li>\n<p>one</p>\n<p>two</p>\n</li>\n</ol>\n</blockquote>\n</blockquote>", 
                "commonmark");
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
        public void ListItems_Spec223_commonmark()
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

            SpecTestHelper.AssertCompliance(">>- one\n>>\n  >  > two", 
                "<blockquote>\n<blockquote>\n<ul>\n<li>one</li>\n</ul>\n<p>two</p>\n</blockquote>\n</blockquote>", 
                "commonmark");
        }

        // Note that at least one space is needed between the list marker and
        // any following content, so these are not list items:
        [Fact]
        public void ListItems_Spec224_commonmark()
        {
            // The following Markdown:
            //     -one
            //     
            //     2.two
            //
            // Should be rendered as:
            //     <p>-one</p>
            //     <p>2.two</p>

            SpecTestHelper.AssertCompliance("-one\n\n2.two", 
                "<p>-one</p>\n<p>2.two</p>", 
                "commonmark");
        }

        // A list item may contain blocks that are separated by more than
        // one blank line.
        [Fact]
        public void ListItems_Spec225_commonmark()
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

            SpecTestHelper.AssertCompliance("- foo\n\n\n  bar", 
                "<ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>", 
                "commonmark");
        }

        // A list item may contain any kind of block:
        [Fact]
        public void ListItems_Spec226_commonmark()
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

            SpecTestHelper.AssertCompliance("1.  foo\n\n    ```\n    bar\n    ```\n\n    baz\n\n    > bam", 
                "<ol>\n<li>\n<p>foo</p>\n<pre><code>bar\n</code></pre>\n<p>baz</p>\n<blockquote>\n<p>bam</p>\n</blockquote>\n</li>\n</ol>", 
                "commonmark");
        }

        // A list item that contains an indented code block will preserve
        // empty lines within the code block verbatim.
        [Fact]
        public void ListItems_Spec227_commonmark()
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

            SpecTestHelper.AssertCompliance("- Foo\n\n      bar\n\n\n      baz", 
                "<ul>\n<li>\n<p>Foo</p>\n<pre><code>bar\n\n\nbaz\n</code></pre>\n</li>\n</ul>", 
                "commonmark");
        }

        // Note that ordered list start numbers must be nine digits or less:
        [Fact]
        public void ListItems_Spec228_commonmark()
        {
            // The following Markdown:
            //     123456789. ok
            //
            // Should be rendered as:
            //     <ol start="123456789">
            //     <li>ok</li>
            //     </ol>

            SpecTestHelper.AssertCompliance("123456789. ok", 
                "<ol start=\"123456789\">\n<li>ok</li>\n</ol>", 
                "commonmark");
        }

        [Fact]
        public void ListItems_Spec229_commonmark()
        {
            // The following Markdown:
            //     1234567890. not ok
            //
            // Should be rendered as:
            //     <p>1234567890. not ok</p>

            SpecTestHelper.AssertCompliance("1234567890. not ok", 
                "<p>1234567890. not ok</p>", 
                "commonmark");
        }

        // A start number may begin with 0s:
        [Fact]
        public void ListItems_Spec230_commonmark()
        {
            // The following Markdown:
            //     0. ok
            //
            // Should be rendered as:
            //     <ol start="0">
            //     <li>ok</li>
            //     </ol>

            SpecTestHelper.AssertCompliance("0. ok", 
                "<ol start=\"0\">\n<li>ok</li>\n</ol>", 
                "commonmark");
        }

        [Fact]
        public void ListItems_Spec231_commonmark()
        {
            // The following Markdown:
            //     003. ok
            //
            // Should be rendered as:
            //     <ol start="3">
            //     <li>ok</li>
            //     </ol>

            SpecTestHelper.AssertCompliance("003. ok", 
                "<ol start=\"3\">\n<li>ok</li>\n</ol>", 
                "commonmark");
        }

        // A start number may not be negative:
        [Fact]
        public void ListItems_Spec232_commonmark()
        {
            // The following Markdown:
            //     -1. not ok
            //
            // Should be rendered as:
            //     <p>-1. not ok</p>

            SpecTestHelper.AssertCompliance("-1. not ok", 
                "<p>-1. not ok</p>", 
                "commonmark");
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
        public void ListItems_Spec233_commonmark()
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

            SpecTestHelper.AssertCompliance("- foo\n\n      bar", 
                "<ul>\n<li>\n<p>foo</p>\n<pre><code>bar\n</code></pre>\n</li>\n</ul>", 
                "commonmark");
        }

        // And in this case it is 11 spaces:
        [Fact]
        public void ListItems_Spec234_commonmark()
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

            SpecTestHelper.AssertCompliance("  10.  foo\n\n           bar", 
                "<ol start=\"10\">\n<li>\n<p>foo</p>\n<pre><code>bar\n</code></pre>\n</li>\n</ol>", 
                "commonmark");
        }

        // If the *first* block in the list item is an indented code block,
        // then by rule #2, the contents must be indented *one* space after the
        // list marker:
        [Fact]
        public void ListItems_Spec235_commonmark()
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

            SpecTestHelper.AssertCompliance("    indented code\n\nparagraph\n\n    more code", 
                "<pre><code>indented code\n</code></pre>\n<p>paragraph</p>\n<pre><code>more code\n</code></pre>", 
                "commonmark");
        }

        [Fact]
        public void ListItems_Spec236_commonmark()
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

            SpecTestHelper.AssertCompliance("1.     indented code\n\n   paragraph\n\n       more code", 
                "<ol>\n<li>\n<pre><code>indented code\n</code></pre>\n<p>paragraph</p>\n<pre><code>more code\n</code></pre>\n</li>\n</ol>", 
                "commonmark");
        }

        // Note that an additional space indent is interpreted as space
        // inside the code block:
        [Fact]
        public void ListItems_Spec237_commonmark()
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

            SpecTestHelper.AssertCompliance("1.      indented code\n\n   paragraph\n\n       more code", 
                "<ol>\n<li>\n<pre><code> indented code\n</code></pre>\n<p>paragraph</p>\n<pre><code>more code\n</code></pre>\n</li>\n</ol>", 
                "commonmark");
        }

        // Note that rules #1 and #2 only apply to two cases:  (a) cases
        // in which the lines to be included in a list item begin with a
        // [non-whitespace character], and (b) cases in which
        // they begin with an indented code
        // block.  In a case like the following, where the first block begins with
        // a three-space indent, the rules do not allow us to form a list item by
        // indenting the whole thing and prepending a list marker:
        [Fact]
        public void ListItems_Spec238_commonmark()
        {
            // The following Markdown:
            //        foo
            //     
            //     bar
            //
            // Should be rendered as:
            //     <p>foo</p>
            //     <p>bar</p>

            SpecTestHelper.AssertCompliance("   foo\n\nbar", 
                "<p>foo</p>\n<p>bar</p>", 
                "commonmark");
        }

        [Fact]
        public void ListItems_Spec239_commonmark()
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

            SpecTestHelper.AssertCompliance("-    foo\n\n  bar", 
                "<ul>\n<li>foo</li>\n</ul>\n<p>bar</p>", 
                "commonmark");
        }

        // This is not a significant restriction, because when a block begins
        // with 1-3 spaces indent, the indentation can always be removed without
        // a change in interpretation, allowing rule #1 to be applied.  So, in
        // the above case:
        [Fact]
        public void ListItems_Spec240_commonmark()
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

            SpecTestHelper.AssertCompliance("-  foo\n\n   bar", 
                "<ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>", 
                "commonmark");
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
        public void ListItems_Spec241_commonmark()
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

            SpecTestHelper.AssertCompliance("-\n  foo\n-\n  ```\n  bar\n  ```\n-\n      baz", 
                "<ul>\n<li>foo</li>\n<li>\n<pre><code>bar\n</code></pre>\n</li>\n<li>\n<pre><code>baz\n</code></pre>\n</li>\n</ul>", 
                "commonmark");
        }

        // When the list item starts with a blank line, the number of spaces
        // following the list marker doesn't change the required indentation:
        [Fact]
        public void ListItems_Spec242_commonmark()
        {
            // The following Markdown:
            //     -   
            //       foo
            //
            // Should be rendered as:
            //     <ul>
            //     <li>foo</li>
            //     </ul>

            SpecTestHelper.AssertCompliance("-   \n  foo", 
                "<ul>\n<li>foo</li>\n</ul>", 
                "commonmark");
        }

        // A list item can begin with at most one blank line.
        // In the following example, `foo` is not part of the list
        // item:
        [Fact]
        public void ListItems_Spec243_commonmark()
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

            SpecTestHelper.AssertCompliance("-\n\n  foo", 
                "<ul>\n<li></li>\n</ul>\n<p>foo</p>", 
                "commonmark");
        }

        // Here is an empty bullet list item:
        [Fact]
        public void ListItems_Spec244_commonmark()
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

            SpecTestHelper.AssertCompliance("- foo\n-\n- bar", 
                "<ul>\n<li>foo</li>\n<li></li>\n<li>bar</li>\n</ul>", 
                "commonmark");
        }

        // It does not matter whether there are spaces following the [list marker]:
        [Fact]
        public void ListItems_Spec245_commonmark()
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

            SpecTestHelper.AssertCompliance("- foo\n-   \n- bar", 
                "<ul>\n<li>foo</li>\n<li></li>\n<li>bar</li>\n</ul>", 
                "commonmark");
        }

        // Here is an empty ordered list item:
        [Fact]
        public void ListItems_Spec246_commonmark()
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

            SpecTestHelper.AssertCompliance("1. foo\n2.\n3. bar", 
                "<ol>\n<li>foo</li>\n<li></li>\n<li>bar</li>\n</ol>", 
                "commonmark");
        }

        // A list may start or end with an empty list item:
        [Fact]
        public void ListItems_Spec247_commonmark()
        {
            // The following Markdown:
            //     *
            //
            // Should be rendered as:
            //     <ul>
            //     <li></li>
            //     </ul>

            SpecTestHelper.AssertCompliance("*", 
                "<ul>\n<li></li>\n</ul>", 
                "commonmark");
        }

        // However, an empty list item cannot interrupt a paragraph:
        [Fact]
        public void ListItems_Spec248_commonmark()
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

            SpecTestHelper.AssertCompliance("foo\n*\n\nfoo\n1.", 
                "<p>foo\n*</p>\n<p>foo\n1.</p>", 
                "commonmark");
        }

        // 4.  **Indentation.**  If a sequence of lines *Ls* constitutes a list item
        //     according to rule #1, #2, or #3, then the result of indenting each line
        //     of *Ls* by 1-3 spaces (the same for each line) also constitutes a
        //     list item with the same contents and attributes.  If a line is
        //     empty, then it need not be indented.
        // 
        // Indented one space:
        [Fact]
        public void ListItems_Spec249_commonmark()
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

            SpecTestHelper.AssertCompliance(" 1.  A paragraph\n     with two lines.\n\n         indented code\n\n     > A block quote.", 
                "<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>", 
                "commonmark");
        }

        // Indented two spaces:
        [Fact]
        public void ListItems_Spec250_commonmark()
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

            SpecTestHelper.AssertCompliance("  1.  A paragraph\n      with two lines.\n\n          indented code\n\n      > A block quote.", 
                "<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>", 
                "commonmark");
        }

        // Indented three spaces:
        [Fact]
        public void ListItems_Spec251_commonmark()
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

            SpecTestHelper.AssertCompliance("   1.  A paragraph\n       with two lines.\n\n           indented code\n\n       > A block quote.", 
                "<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>", 
                "commonmark");
        }

        // Four spaces indent gives a code block:
        [Fact]
        public void ListItems_Spec252_commonmark()
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

            SpecTestHelper.AssertCompliance("    1.  A paragraph\n        with two lines.\n\n            indented code\n\n        > A block quote.", 
                "<pre><code>1.  A paragraph\n    with two lines.\n\n        indented code\n\n    &gt; A block quote.\n</code></pre>", 
                "commonmark");
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
        public void ListItems_Spec253_commonmark()
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

            SpecTestHelper.AssertCompliance("  1.  A paragraph\nwith two lines.\n\n          indented code\n\n      > A block quote.", 
                "<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>", 
                "commonmark");
        }

        // Indentation can be partially deleted:
        [Fact]
        public void ListItems_Spec254_commonmark()
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

            SpecTestHelper.AssertCompliance("  1.  A paragraph\n    with two lines.", 
                "<ol>\n<li>A paragraph\nwith two lines.</li>\n</ol>", 
                "commonmark");
        }

        // These examples show how laziness can work in nested structures:
        [Fact]
        public void ListItems_Spec255_commonmark()
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

            SpecTestHelper.AssertCompliance("> 1. > Blockquote\ncontinued here.", 
                "<blockquote>\n<ol>\n<li>\n<blockquote>\n<p>Blockquote\ncontinued here.</p>\n</blockquote>\n</li>\n</ol>\n</blockquote>", 
                "commonmark");
        }

        [Fact]
        public void ListItems_Spec256_commonmark()
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

            SpecTestHelper.AssertCompliance("> 1. > Blockquote\n> continued here.", 
                "<blockquote>\n<ol>\n<li>\n<blockquote>\n<p>Blockquote\ncontinued here.</p>\n</blockquote>\n</li>\n</ol>\n</blockquote>", 
                "commonmark");
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
        public void ListItems_Spec257_commonmark()
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

            SpecTestHelper.AssertCompliance("- foo\n  - bar\n    - baz\n      - boo", 
                "<ul>\n<li>foo\n<ul>\n<li>bar\n<ul>\n<li>baz\n<ul>\n<li>boo</li>\n</ul>\n</li>\n</ul>\n</li>\n</ul>\n</li>\n</ul>", 
                "commonmark");
        }

        // One is not enough:
        [Fact]
        public void ListItems_Spec258_commonmark()
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

            SpecTestHelper.AssertCompliance("- foo\n - bar\n  - baz\n   - boo", 
                "<ul>\n<li>foo</li>\n<li>bar</li>\n<li>baz</li>\n<li>boo</li>\n</ul>", 
                "commonmark");
        }

        // Here we need four, because the list marker is wider:
        [Fact]
        public void ListItems_Spec259_commonmark()
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

            SpecTestHelper.AssertCompliance("10) foo\n    - bar", 
                "<ol start=\"10\">\n<li>foo\n<ul>\n<li>bar</li>\n</ul>\n</li>\n</ol>", 
                "commonmark");
        }

        // Three is not enough:
        [Fact]
        public void ListItems_Spec260_commonmark()
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

            SpecTestHelper.AssertCompliance("10) foo\n   - bar", 
                "<ol start=\"10\">\n<li>foo</li>\n</ol>\n<ul>\n<li>bar</li>\n</ul>", 
                "commonmark");
        }

        // A list may be the first block in a list item:
        [Fact]
        public void ListItems_Spec261_commonmark()
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

            SpecTestHelper.AssertCompliance("- - foo", 
                "<ul>\n<li>\n<ul>\n<li>foo</li>\n</ul>\n</li>\n</ul>", 
                "commonmark");
        }

        [Fact]
        public void ListItems_Spec262_commonmark()
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

            SpecTestHelper.AssertCompliance("1. - 2. foo", 
                "<ol>\n<li>\n<ul>\n<li>\n<ol start=\"2\">\n<li>foo</li>\n</ol>\n</li>\n</ul>\n</li>\n</ol>", 
                "commonmark");
        }

        // A list item can contain a heading:
        [Fact]
        public void ListItems_Spec263_commonmark()
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

            SpecTestHelper.AssertCompliance("- # Foo\n- Bar\n  ---\n  baz", 
                "<ul>\n<li>\n<h1>Foo</h1>\n</li>\n<li>\n<h2>Bar</h2>\nbaz</li>\n</ul>", 
                "commonmark");
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
        public void Lists_Spec264_commonmark()
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

            SpecTestHelper.AssertCompliance("- foo\n- bar\n+ baz", 
                "<ul>\n<li>foo</li>\n<li>bar</li>\n</ul>\n<ul>\n<li>baz</li>\n</ul>", 
                "commonmark");
        }

        [Fact]
        public void Lists_Spec265_commonmark()
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

            SpecTestHelper.AssertCompliance("1. foo\n2. bar\n3) baz", 
                "<ol>\n<li>foo</li>\n<li>bar</li>\n</ol>\n<ol start=\"3\">\n<li>baz</li>\n</ol>", 
                "commonmark");
        }

        // In CommonMark, a list can interrupt a paragraph. That is,
        // no blank line is needed to separate a paragraph from a following
        // list:
        [Fact]
        public void Lists_Spec266_commonmark()
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

            SpecTestHelper.AssertCompliance("Foo\n- bar\n- baz", 
                "<p>Foo</p>\n<ul>\n<li>bar</li>\n<li>baz</li>\n</ul>", 
                "commonmark");
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
        public void Lists_Spec267_commonmark()
        {
            // The following Markdown:
            //     The number of windows in my house is
            //     14.  The number of doors is 6.
            //
            // Should be rendered as:
            //     <p>The number of windows in my house is
            //     14.  The number of doors is 6.</p>

            SpecTestHelper.AssertCompliance("The number of windows in my house is\n14.  The number of doors is 6.", 
                "<p>The number of windows in my house is\n14.  The number of doors is 6.</p>", 
                "commonmark");
        }

        // We may still get an unintended result in cases like
        [Fact]
        public void Lists_Spec268_commonmark()
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

            SpecTestHelper.AssertCompliance("The number of windows in my house is\n1.  The number of doors is 6.", 
                "<p>The number of windows in my house is</p>\n<ol>\n<li>The number of doors is 6.</li>\n</ol>", 
                "commonmark");
        }

        // but this rule should prevent most spurious list captures.
        // 
        // There can be any number of blank lines between items:
        [Fact]
        public void Lists_Spec269_commonmark()
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

            SpecTestHelper.AssertCompliance("- foo\n\n- bar\n\n\n- baz", 
                "<ul>\n<li>\n<p>foo</p>\n</li>\n<li>\n<p>bar</p>\n</li>\n<li>\n<p>baz</p>\n</li>\n</ul>", 
                "commonmark");
        }

        [Fact]
        public void Lists_Spec270_commonmark()
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

            SpecTestHelper.AssertCompliance("- foo\n  - bar\n    - baz\n\n\n      bim", 
                "<ul>\n<li>foo\n<ul>\n<li>bar\n<ul>\n<li>\n<p>baz</p>\n<p>bim</p>\n</li>\n</ul>\n</li>\n</ul>\n</li>\n</ul>", 
                "commonmark");
        }

        // To separate consecutive lists of the same type, or to separate a
        // list from an indented code block that would otherwise be parsed
        // as a subparagraph of the final list item, you can insert a blank HTML
        // comment:
        [Fact]
        public void Lists_Spec271_commonmark()
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

            SpecTestHelper.AssertCompliance("- foo\n- bar\n\n<!-- -->\n\n- baz\n- bim", 
                "<ul>\n<li>foo</li>\n<li>bar</li>\n</ul>\n<!-- -->\n<ul>\n<li>baz</li>\n<li>bim</li>\n</ul>", 
                "commonmark");
        }

        [Fact]
        public void Lists_Spec272_commonmark()
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

            SpecTestHelper.AssertCompliance("-   foo\n\n    notcode\n\n-   foo\n\n<!-- -->\n\n    code", 
                "<ul>\n<li>\n<p>foo</p>\n<p>notcode</p>\n</li>\n<li>\n<p>foo</p>\n</li>\n</ul>\n<!-- -->\n<pre><code>code\n</code></pre>", 
                "commonmark");
        }

        // List items need not be indented to the same level.  The following
        // list items will be treated as items at the same list level,
        // since none is indented enough to belong to the previous list
        // item:
        [Fact]
        public void Lists_Spec273_commonmark()
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

            SpecTestHelper.AssertCompliance("- a\n - b\n  - c\n   - d\n    - e\n   - f\n  - g\n - h\n- i", 
                "<ul>\n<li>a</li>\n<li>b</li>\n<li>c</li>\n<li>d</li>\n<li>e</li>\n<li>f</li>\n<li>g</li>\n<li>h</li>\n<li>i</li>\n</ul>", 
                "commonmark");
        }

        [Fact]
        public void Lists_Spec274_commonmark()
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

            SpecTestHelper.AssertCompliance("1. a\n\n  2. b\n\n    3. c", 
                "<ol>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n</li>\n<li>\n<p>c</p>\n</li>\n</ol>", 
                "commonmark");
        }

        // This is a loose list, because there is a blank line between
        // two of the list items:
        [Fact]
        public void Lists_Spec275_commonmark()
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

            SpecTestHelper.AssertCompliance("- a\n- b\n\n- c", 
                "<ul>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n</li>\n<li>\n<p>c</p>\n</li>\n</ul>", 
                "commonmark");
        }

        // So is this, with a empty second item:
        [Fact]
        public void Lists_Spec276_commonmark()
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

            SpecTestHelper.AssertCompliance("* a\n*\n\n* c", 
                "<ul>\n<li>\n<p>a</p>\n</li>\n<li></li>\n<li>\n<p>c</p>\n</li>\n</ul>", 
                "commonmark");
        }

        // These are loose lists, even though there is no space between the items,
        // because one of the items directly contains two block-level elements
        // with a blank line between them:
        [Fact]
        public void Lists_Spec277_commonmark()
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

            SpecTestHelper.AssertCompliance("- a\n- b\n\n  c\n- d", 
                "<ul>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n<p>c</p>\n</li>\n<li>\n<p>d</p>\n</li>\n</ul>", 
                "commonmark");
        }

        [Fact]
        public void Lists_Spec278_commonmark()
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

            SpecTestHelper.AssertCompliance("- a\n- b\n\n  [ref]: /url\n- d", 
                "<ul>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n</li>\n<li>\n<p>d</p>\n</li>\n</ul>", 
                "commonmark");
        }

        // This is a tight list, because the blank lines are in a code block:
        [Fact]
        public void Lists_Spec279_commonmark()
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

            SpecTestHelper.AssertCompliance("- a\n- ```\n  b\n\n\n  ```\n- c", 
                "<ul>\n<li>a</li>\n<li>\n<pre><code>b\n\n\n</code></pre>\n</li>\n<li>c</li>\n</ul>", 
                "commonmark");
        }

        // This is a tight list, because the blank line is between two
        // paragraphs of a sublist.  So the sublist is loose while
        // the outer list is tight:
        [Fact]
        public void Lists_Spec280_commonmark()
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

            SpecTestHelper.AssertCompliance("- a\n  - b\n\n    c\n- d", 
                "<ul>\n<li>a\n<ul>\n<li>\n<p>b</p>\n<p>c</p>\n</li>\n</ul>\n</li>\n<li>d</li>\n</ul>", 
                "commonmark");
        }

        // This is a tight list, because the blank line is inside the
        // block quote:
        [Fact]
        public void Lists_Spec281_commonmark()
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

            SpecTestHelper.AssertCompliance("* a\n  > b\n  >\n* c", 
                "<ul>\n<li>a\n<blockquote>\n<p>b</p>\n</blockquote>\n</li>\n<li>c</li>\n</ul>", 
                "commonmark");
        }

        // This list is tight, because the consecutive block elements
        // are not separated by blank lines:
        [Fact]
        public void Lists_Spec282_commonmark()
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

            SpecTestHelper.AssertCompliance("- a\n  > b\n  ```\n  c\n  ```\n- d", 
                "<ul>\n<li>a\n<blockquote>\n<p>b</p>\n</blockquote>\n<pre><code>c\n</code></pre>\n</li>\n<li>d</li>\n</ul>", 
                "commonmark");
        }

        // A single-paragraph list is tight:
        [Fact]
        public void Lists_Spec283_commonmark()
        {
            // The following Markdown:
            //     - a
            //
            // Should be rendered as:
            //     <ul>
            //     <li>a</li>
            //     </ul>

            SpecTestHelper.AssertCompliance("- a", 
                "<ul>\n<li>a</li>\n</ul>", 
                "commonmark");
        }

        [Fact]
        public void Lists_Spec284_commonmark()
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

            SpecTestHelper.AssertCompliance("- a\n  - b", 
                "<ul>\n<li>a\n<ul>\n<li>b</li>\n</ul>\n</li>\n</ul>", 
                "commonmark");
        }

        // This list is loose, because of the blank line between the
        // two block elements in the list item:
        [Fact]
        public void Lists_Spec285_commonmark()
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

            SpecTestHelper.AssertCompliance("1. ```\n   foo\n   ```\n\n   bar", 
                "<ol>\n<li>\n<pre><code>foo\n</code></pre>\n<p>bar</p>\n</li>\n</ol>", 
                "commonmark");
        }

        // Here the outer list is loose, the inner list tight:
        [Fact]
        public void Lists_Spec286_commonmark()
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

            SpecTestHelper.AssertCompliance("* foo\n  * bar\n\n  baz", 
                "<ul>\n<li>\n<p>foo</p>\n<ul>\n<li>bar</li>\n</ul>\n<p>baz</p>\n</li>\n</ul>", 
                "commonmark");
        }

        [Fact]
        public void Lists_Spec287_commonmark()
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

            SpecTestHelper.AssertCompliance("- a\n  - b\n  - c\n\n- d\n  - e\n  - f", 
                "<ul>\n<li>\n<p>a</p>\n<ul>\n<li>b</li>\n<li>c</li>\n</ul>\n</li>\n<li>\n<p>d</p>\n<ul>\n<li>e</li>\n<li>f</li>\n</ul>\n</li>\n</ul>", 
                "commonmark");
        }

        // # Inlines
        // 
        // Inlines are parsed sequentially from the beginning of the character
        // stream to the end (left to right, in left-to-right languages).
        // Thus, for example, in
        [Fact]
        public void Lists_Spec288_commonmark()
        {
            // The following Markdown:
            //     `hi`lo`
            //
            // Should be rendered as:
            //     <p><code>hi</code>lo`</p>

            SpecTestHelper.AssertCompliance("`hi`lo`", 
                "<p><code>hi</code>lo`</p>", 
                "commonmark");
        }
    }

    // Any ASCII punctuation character may be backslash-escaped:
    public class BackslashEscapesTests
    {

        [Fact]
        public void BackslashEscapes_Spec289_commonmark()
        {
            // The following Markdown:
            //     \!\"\#\$\%\&\'\(\)\*\+\,\-\.\/\:\;\<\=\>\?\@\[\\\]\^\_\`\{\|\}\~
            //
            // Should be rendered as:
            //     <p>!&quot;#$%&amp;'()*+,-./:;&lt;=&gt;?@[\]^_`{|}~</p>

            SpecTestHelper.AssertCompliance("\\!\\\"\\#\\$\\%\\&\\'\\(\\)\\*\\+\\,\\-\\.\\/\\:\\;\\<\\=\\>\\?\\@\\[\\\\\\]\\^\\_\\`\\{\\|\\}\\~", 
                "<p>!&quot;#$%&amp;'()*+,-./:;&lt;=&gt;?@[\\]^_`{|}~</p>", 
                "commonmark");
        }

        // Backslashes before other characters are treated as literal
        // backslashes:
        [Fact]
        public void BackslashEscapes_Spec290_commonmark()
        {
            // The following Markdown:
            //     \→\A\a\ \3\φ\«
            //
            // Should be rendered as:
            //     <p>\→\A\a\ \3\φ\«</p>

            SpecTestHelper.AssertCompliance("\\\t\\A\\a\\ \\3\\φ\\«", 
                "<p>\\\t\\A\\a\\ \\3\\φ\\«</p>", 
                "commonmark");
        }

        // Escaped characters are treated as regular characters and do
        // not have their usual Markdown meanings:
        [Fact]
        public void BackslashEscapes_Spec291_commonmark()
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

            SpecTestHelper.AssertCompliance("\\*not emphasized*\n\\<br/> not a tag\n\\[not a link](/foo)\n\\`not code`\n1\\. not a list\n\\* not a list\n\\# not a heading\n\\[foo]: /url \"not a reference\"", 
                "<p>*not emphasized*\n&lt;br/&gt; not a tag\n[not a link](/foo)\n`not code`\n1. not a list\n* not a list\n# not a heading\n[foo]: /url &quot;not a reference&quot;</p>", 
                "commonmark");
        }

        // If a backslash is itself escaped, the following character is not:
        [Fact]
        public void BackslashEscapes_Spec292_commonmark()
        {
            // The following Markdown:
            //     \\*emphasis*
            //
            // Should be rendered as:
            //     <p>\<em>emphasis</em></p>

            SpecTestHelper.AssertCompliance("\\\\*emphasis*", 
                "<p>\\<em>emphasis</em></p>", 
                "commonmark");
        }

        // A backslash at the end of the line is a [hard line break]:
        [Fact]
        public void BackslashEscapes_Spec293_commonmark()
        {
            // The following Markdown:
            //     foo\
            //     bar
            //
            // Should be rendered as:
            //     <p>foo<br />
            //     bar</p>

            SpecTestHelper.AssertCompliance("foo\\\nbar", 
                "<p>foo<br />\nbar</p>", 
                "commonmark");
        }

        // Backslash escapes do not work in code blocks, code spans, autolinks, or
        // raw HTML:
        [Fact]
        public void BackslashEscapes_Spec294_commonmark()
        {
            // The following Markdown:
            //     `` \[\` ``
            //
            // Should be rendered as:
            //     <p><code>\[\`</code></p>

            SpecTestHelper.AssertCompliance("`` \\[\\` ``", 
                "<p><code>\\[\\`</code></p>", 
                "commonmark");
        }

        [Fact]
        public void BackslashEscapes_Spec295_commonmark()
        {
            // The following Markdown:
            //         \[\]
            //
            // Should be rendered as:
            //     <pre><code>\[\]
            //     </code></pre>

            SpecTestHelper.AssertCompliance("    \\[\\]", 
                "<pre><code>\\[\\]\n</code></pre>", 
                "commonmark");
        }

        [Fact]
        public void BackslashEscapes_Spec296_commonmark()
        {
            // The following Markdown:
            //     ~~~
            //     \[\]
            //     ~~~
            //
            // Should be rendered as:
            //     <pre><code>\[\]
            //     </code></pre>

            SpecTestHelper.AssertCompliance("~~~\n\\[\\]\n~~~", 
                "<pre><code>\\[\\]\n</code></pre>", 
                "commonmark");
        }

        [Fact]
        public void BackslashEscapes_Spec297_commonmark()
        {
            // The following Markdown:
            //     <http://example.com?find=\*>
            //
            // Should be rendered as:
            //     <p><a href="http://example.com?find=%5C*">http://example.com?find=\*</a></p>

            SpecTestHelper.AssertCompliance("<http://example.com?find=\\*>", 
                "<p><a href=\"http://example.com?find=%5C*\">http://example.com?find=\\*</a></p>", 
                "commonmark");
        }

        [Fact]
        public void BackslashEscapes_Spec298_commonmark()
        {
            // The following Markdown:
            //     <a href="/bar\/)">
            //
            // Should be rendered as:
            //     <a href="/bar\/)">

            SpecTestHelper.AssertCompliance("<a href=\"/bar\\/)\">", 
                "<a href=\"/bar\\/)\">", 
                "commonmark");
        }

        // But they work in all other contexts, including URLs and link titles,
        // link references, and [info strings] in [fenced code blocks]:
        [Fact]
        public void BackslashEscapes_Spec299_commonmark()
        {
            // The following Markdown:
            //     [foo](/bar\* "ti\*tle")
            //
            // Should be rendered as:
            //     <p><a href="/bar*" title="ti*tle">foo</a></p>

            SpecTestHelper.AssertCompliance("[foo](/bar\\* \"ti\\*tle\")", 
                "<p><a href=\"/bar*\" title=\"ti*tle\">foo</a></p>", 
                "commonmark");
        }

        [Fact]
        public void BackslashEscapes_Spec300_commonmark()
        {
            // The following Markdown:
            //     [foo]
            //     
            //     [foo]: /bar\* "ti\*tle"
            //
            // Should be rendered as:
            //     <p><a href="/bar*" title="ti*tle">foo</a></p>

            SpecTestHelper.AssertCompliance("[foo]\n\n[foo]: /bar\\* \"ti\\*tle\"", 
                "<p><a href=\"/bar*\" title=\"ti*tle\">foo</a></p>", 
                "commonmark");
        }

        [Fact]
        public void BackslashEscapes_Spec301_commonmark()
        {
            // The following Markdown:
            //     ``` foo\+bar
            //     foo
            //     ```
            //
            // Should be rendered as:
            //     <pre><code class="language-foo+bar">foo
            //     </code></pre>

            SpecTestHelper.AssertCompliance("``` foo\\+bar\nfoo\n```", 
                "<pre><code class=\"language-foo+bar\">foo\n</code></pre>", 
                "commonmark");
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
        public void EntityAndNumericCharacterReferences_Spec302_commonmark()
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

            SpecTestHelper.AssertCompliance("&nbsp; &amp; &copy; &AElig; &Dcaron;\n&frac34; &HilbertSpace; &DifferentialD;\n&ClockwiseContourIntegral; &ngE;", 
                "<p>  &amp; © Æ Ď\n¾ ℋ ⅆ\n∲ ≧̸</p>", 
                "commonmark");
        }

        // [Decimal numeric character
        // references](@)
        // consist of `&#` + a string of 1--8 arabic digits + `;`. A
        // numeric character reference is parsed as the corresponding
        // Unicode character. Invalid Unicode code points will be replaced by
        // the REPLACEMENT CHARACTER (`U+FFFD`).  For security reasons,
        // the code point `U+0000` will also be replaced by `U+FFFD`.
        [Fact]
        public void EntityAndNumericCharacterReferences_Spec303_commonmark()
        {
            // The following Markdown:
            //     &#35; &#1234; &#992; &#98765432; &#0;
            //
            // Should be rendered as:
            //     <p># Ӓ Ϡ � �</p>

            SpecTestHelper.AssertCompliance("&#35; &#1234; &#992; &#98765432; &#0;", 
                "<p># Ӓ Ϡ � �</p>", 
                "commonmark");
        }

        // [Hexadecimal numeric character
        // references](@) consist of `&#` +
        // either `X` or `x` + a string of 1-8 hexadecimal digits + `;`.
        // They too are parsed as the corresponding Unicode character (this
        // time specified with a hexadecimal numeral instead of decimal).
        [Fact]
        public void EntityAndNumericCharacterReferences_Spec304_commonmark()
        {
            // The following Markdown:
            //     &#X22; &#XD06; &#xcab;
            //
            // Should be rendered as:
            //     <p>&quot; ആ ಫ</p>

            SpecTestHelper.AssertCompliance("&#X22; &#XD06; &#xcab;", 
                "<p>&quot; ആ ಫ</p>", 
                "commonmark");
        }

        // Here are some nonentities:
        [Fact]
        public void EntityAndNumericCharacterReferences_Spec305_commonmark()
        {
            // The following Markdown:
            //     &nbsp &x; &#; &#x;
            //     &ThisIsNotDefined; &hi?;
            //
            // Should be rendered as:
            //     <p>&amp;nbsp &amp;x; &amp;#; &amp;#x;
            //     &amp;ThisIsNotDefined; &amp;hi?;</p>

            SpecTestHelper.AssertCompliance("&nbsp &x; &#; &#x;\n&ThisIsNotDefined; &hi?;", 
                "<p>&amp;nbsp &amp;x; &amp;#; &amp;#x;\n&amp;ThisIsNotDefined; &amp;hi?;</p>", 
                "commonmark");
        }

        // Although HTML5 does accept some entity references
        // without a trailing semicolon (such as `&copy`), these are not
        // recognized here, because it makes the grammar too ambiguous:
        [Fact]
        public void EntityAndNumericCharacterReferences_Spec306_commonmark()
        {
            // The following Markdown:
            //     &copy
            //
            // Should be rendered as:
            //     <p>&amp;copy</p>

            SpecTestHelper.AssertCompliance("&copy", 
                "<p>&amp;copy</p>", 
                "commonmark");
        }

        // Strings that are not on the list of HTML5 named entities are not
        // recognized as entity references either:
        [Fact]
        public void EntityAndNumericCharacterReferences_Spec307_commonmark()
        {
            // The following Markdown:
            //     &MadeUpEntity;
            //
            // Should be rendered as:
            //     <p>&amp;MadeUpEntity;</p>

            SpecTestHelper.AssertCompliance("&MadeUpEntity;", 
                "<p>&amp;MadeUpEntity;</p>", 
                "commonmark");
        }

        // Entity and numeric character references are recognized in any
        // context besides code spans or code blocks, including
        // URLs, [link titles], and [fenced code block][] [info strings]:
        [Fact]
        public void EntityAndNumericCharacterReferences_Spec308_commonmark()
        {
            // The following Markdown:
            //     <a href="&ouml;&ouml;.html">
            //
            // Should be rendered as:
            //     <a href="&ouml;&ouml;.html">

            SpecTestHelper.AssertCompliance("<a href=\"&ouml;&ouml;.html\">", 
                "<a href=\"&ouml;&ouml;.html\">", 
                "commonmark");
        }

        [Fact]
        public void EntityAndNumericCharacterReferences_Spec309_commonmark()
        {
            // The following Markdown:
            //     [foo](/f&ouml;&ouml; "f&ouml;&ouml;")
            //
            // Should be rendered as:
            //     <p><a href="/f%C3%B6%C3%B6" title="föö">foo</a></p>

            SpecTestHelper.AssertCompliance("[foo](/f&ouml;&ouml; \"f&ouml;&ouml;\")", 
                "<p><a href=\"/f%C3%B6%C3%B6\" title=\"föö\">foo</a></p>", 
                "commonmark");
        }

        [Fact]
        public void EntityAndNumericCharacterReferences_Spec310_commonmark()
        {
            // The following Markdown:
            //     [foo]
            //     
            //     [foo]: /f&ouml;&ouml; "f&ouml;&ouml;"
            //
            // Should be rendered as:
            //     <p><a href="/f%C3%B6%C3%B6" title="föö">foo</a></p>

            SpecTestHelper.AssertCompliance("[foo]\n\n[foo]: /f&ouml;&ouml; \"f&ouml;&ouml;\"", 
                "<p><a href=\"/f%C3%B6%C3%B6\" title=\"föö\">foo</a></p>", 
                "commonmark");
        }

        [Fact]
        public void EntityAndNumericCharacterReferences_Spec311_commonmark()
        {
            // The following Markdown:
            //     ``` f&ouml;&ouml;
            //     foo
            //     ```
            //
            // Should be rendered as:
            //     <pre><code class="language-föö">foo
            //     </code></pre>

            SpecTestHelper.AssertCompliance("``` f&ouml;&ouml;\nfoo\n```", 
                "<pre><code class=\"language-föö\">foo\n</code></pre>", 
                "commonmark");
        }

        // Entity and numeric character references are treated as literal
        // text in code spans and code blocks:
        [Fact]
        public void EntityAndNumericCharacterReferences_Spec312_commonmark()
        {
            // The following Markdown:
            //     `f&ouml;&ouml;`
            //
            // Should be rendered as:
            //     <p><code>f&amp;ouml;&amp;ouml;</code></p>

            SpecTestHelper.AssertCompliance("`f&ouml;&ouml;`", 
                "<p><code>f&amp;ouml;&amp;ouml;</code></p>", 
                "commonmark");
        }

        [Fact]
        public void EntityAndNumericCharacterReferences_Spec313_commonmark()
        {
            // The following Markdown:
            //         f&ouml;f&ouml;
            //
            // Should be rendered as:
            //     <pre><code>f&amp;ouml;f&amp;ouml;
            //     </code></pre>

            SpecTestHelper.AssertCompliance("    f&ouml;f&ouml;", 
                "<pre><code>f&amp;ouml;f&amp;ouml;\n</code></pre>", 
                "commonmark");
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
        public void CodeSpans_Spec314_commonmark()
        {
            // The following Markdown:
            //     `foo`
            //
            // Should be rendered as:
            //     <p><code>foo</code></p>

            SpecTestHelper.AssertCompliance("`foo`", 
                "<p><code>foo</code></p>", 
                "commonmark");
        }

        // Here two backticks are used, because the code contains a backtick.
        // This example also illustrates stripping of leading and trailing spaces:
        [Fact]
        public void CodeSpans_Spec315_commonmark()
        {
            // The following Markdown:
            //     `` foo ` bar  ``
            //
            // Should be rendered as:
            //     <p><code>foo ` bar</code></p>

            SpecTestHelper.AssertCompliance("`` foo ` bar  ``", 
                "<p><code>foo ` bar</code></p>", 
                "commonmark");
        }

        // This example shows the motivation for stripping leading and trailing
        // spaces:
        [Fact]
        public void CodeSpans_Spec316_commonmark()
        {
            // The following Markdown:
            //     ` `` `
            //
            // Should be rendered as:
            //     <p><code>``</code></p>

            SpecTestHelper.AssertCompliance("` `` `", 
                "<p><code>``</code></p>", 
                "commonmark");
        }

        // [Line endings] are treated like spaces:
        [Fact]
        public void CodeSpans_Spec317_commonmark()
        {
            // The following Markdown:
            //     ``
            //     foo
            //     ``
            //
            // Should be rendered as:
            //     <p><code>foo</code></p>

            SpecTestHelper.AssertCompliance("``\nfoo\n``", 
                "<p><code>foo</code></p>", 
                "commonmark");
        }

        // Interior spaces and [line endings] are collapsed into
        // single spaces, just as they would be by a browser:
        [Fact]
        public void CodeSpans_Spec318_commonmark()
        {
            // The following Markdown:
            //     `foo   bar
            //       baz`
            //
            // Should be rendered as:
            //     <p><code>foo bar baz</code></p>

            SpecTestHelper.AssertCompliance("`foo   bar\n  baz`", 
                "<p><code>foo bar baz</code></p>", 
                "commonmark");
        }

        // Not all [Unicode whitespace] (for instance, non-breaking space) is
        // collapsed, however:
        [Fact]
        public void CodeSpans_Spec319_commonmark()
        {
            // The following Markdown:
            //     `a  b`
            //
            // Should be rendered as:
            //     <p><code>a  b</code></p>

            SpecTestHelper.AssertCompliance("`a  b`", 
                "<p><code>a  b</code></p>", 
                "commonmark");
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
        public void CodeSpans_Spec320_commonmark()
        {
            // The following Markdown:
            //     `foo `` bar`
            //
            // Should be rendered as:
            //     <p><code>foo `` bar</code></p>

            SpecTestHelper.AssertCompliance("`foo `` bar`", 
                "<p><code>foo `` bar</code></p>", 
                "commonmark");
        }

        // Note that backslash escapes do not work in code spans. All backslashes
        // are treated literally:
        [Fact]
        public void CodeSpans_Spec321_commonmark()
        {
            // The following Markdown:
            //     `foo\`bar`
            //
            // Should be rendered as:
            //     <p><code>foo\</code>bar`</p>

            SpecTestHelper.AssertCompliance("`foo\\`bar`", 
                "<p><code>foo\\</code>bar`</p>", 
                "commonmark");
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
        public void CodeSpans_Spec322_commonmark()
        {
            // The following Markdown:
            //     *foo`*`
            //
            // Should be rendered as:
            //     <p>*foo<code>*</code></p>

            SpecTestHelper.AssertCompliance("*foo`*`", 
                "<p>*foo<code>*</code></p>", 
                "commonmark");
        }

        // And this is not parsed as a link:
        [Fact]
        public void CodeSpans_Spec323_commonmark()
        {
            // The following Markdown:
            //     [not a `link](/foo`)
            //
            // Should be rendered as:
            //     <p>[not a <code>link](/foo</code>)</p>

            SpecTestHelper.AssertCompliance("[not a `link](/foo`)", 
                "<p>[not a <code>link](/foo</code>)</p>", 
                "commonmark");
        }

        // Code spans, HTML tags, and autolinks have the same precedence.
        // Thus, this is code:
        [Fact]
        public void CodeSpans_Spec324_commonmark()
        {
            // The following Markdown:
            //     `<a href="`">`
            //
            // Should be rendered as:
            //     <p><code>&lt;a href=&quot;</code>&quot;&gt;`</p>

            SpecTestHelper.AssertCompliance("`<a href=\"`\">`", 
                "<p><code>&lt;a href=&quot;</code>&quot;&gt;`</p>", 
                "commonmark");
        }

        // But this is an HTML tag:
        [Fact]
        public void CodeSpans_Spec325_commonmark()
        {
            // The following Markdown:
            //     <a href="`">`
            //
            // Should be rendered as:
            //     <p><a href="`">`</p>

            SpecTestHelper.AssertCompliance("<a href=\"`\">`", 
                "<p><a href=\"`\">`</p>", 
                "commonmark");
        }

        // And this is code:
        [Fact]
        public void CodeSpans_Spec326_commonmark()
        {
            // The following Markdown:
            //     `<http://foo.bar.`baz>`
            //
            // Should be rendered as:
            //     <p><code>&lt;http://foo.bar.</code>baz&gt;`</p>

            SpecTestHelper.AssertCompliance("`<http://foo.bar.`baz>`", 
                "<p><code>&lt;http://foo.bar.</code>baz&gt;`</p>", 
                "commonmark");
        }

        // But this is an autolink:
        [Fact]
        public void CodeSpans_Spec327_commonmark()
        {
            // The following Markdown:
            //     <http://foo.bar.`baz>`
            //
            // Should be rendered as:
            //     <p><a href="http://foo.bar.%60baz">http://foo.bar.`baz</a>`</p>

            SpecTestHelper.AssertCompliance("<http://foo.bar.`baz>`", 
                "<p><a href=\"http://foo.bar.%60baz\">http://foo.bar.`baz</a>`</p>", 
                "commonmark");
        }

        // When a backtick string is not closed by a matching backtick string,
        // we just have literal backticks:
        [Fact]
        public void CodeSpans_Spec328_commonmark()
        {
            // The following Markdown:
            //     ```foo``
            //
            // Should be rendered as:
            //     <p>```foo``</p>

            SpecTestHelper.AssertCompliance("```foo``", 
                "<p>```foo``</p>", 
                "commonmark");
        }

        [Fact]
        public void CodeSpans_Spec329_commonmark()
        {
            // The following Markdown:
            //     `foo
            //
            // Should be rendered as:
            //     <p>`foo</p>

            SpecTestHelper.AssertCompliance("`foo", 
                "<p>`foo</p>", 
                "commonmark");
        }

        // The following case also illustrates the need for opening and
        // closing backtick strings to be equal in length:
        [Fact]
        public void CodeSpans_Spec330_commonmark()
        {
            // The following Markdown:
            //     `foo``bar``
            //
            // Should be rendered as:
            //     <p>`foo<code>bar</code></p>

            SpecTestHelper.AssertCompliance("`foo``bar``", 
                "<p>`foo<code>bar</code></p>", 
                "commonmark");
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
        public void EmphasisAndStrongEmphasis_Spec331_commonmark()
        {
            // The following Markdown:
            //     *foo bar*
            //
            // Should be rendered as:
            //     <p><em>foo bar</em></p>

            SpecTestHelper.AssertCompliance("*foo bar*", 
                "<p><em>foo bar</em></p>", 
                "commonmark");
        }

        // This is not emphasis, because the opening `*` is followed by
        // whitespace, and hence not part of a [left-flanking delimiter run]:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec332_commonmark()
        {
            // The following Markdown:
            //     a * foo bar*
            //
            // Should be rendered as:
            //     <p>a * foo bar*</p>

            SpecTestHelper.AssertCompliance("a * foo bar*", 
                "<p>a * foo bar*</p>", 
                "commonmark");
        }

        // This is not emphasis, because the opening `*` is preceded
        // by an alphanumeric and followed by punctuation, and hence
        // not part of a [left-flanking delimiter run]:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec333_commonmark()
        {
            // The following Markdown:
            //     a*"foo"*
            //
            // Should be rendered as:
            //     <p>a*&quot;foo&quot;*</p>

            SpecTestHelper.AssertCompliance("a*\"foo\"*", 
                "<p>a*&quot;foo&quot;*</p>", 
                "commonmark");
        }

        // Unicode nonbreaking spaces count as whitespace, too:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec334_commonmark()
        {
            // The following Markdown:
            //     * a *
            //
            // Should be rendered as:
            //     <p>* a *</p>

            SpecTestHelper.AssertCompliance("* a *", 
                "<p>* a *</p>", 
                "commonmark");
        }

        // Intraword emphasis with `*` is permitted:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec335_commonmark()
        {
            // The following Markdown:
            //     foo*bar*
            //
            // Should be rendered as:
            //     <p>foo<em>bar</em></p>

            SpecTestHelper.AssertCompliance("foo*bar*", 
                "<p>foo<em>bar</em></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec336_commonmark()
        {
            // The following Markdown:
            //     5*6*78
            //
            // Should be rendered as:
            //     <p>5<em>6</em>78</p>

            SpecTestHelper.AssertCompliance("5*6*78", 
                "<p>5<em>6</em>78</p>", 
                "commonmark");
        }

        // Rule 2:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec337_commonmark()
        {
            // The following Markdown:
            //     _foo bar_
            //
            // Should be rendered as:
            //     <p><em>foo bar</em></p>

            SpecTestHelper.AssertCompliance("_foo bar_", 
                "<p><em>foo bar</em></p>", 
                "commonmark");
        }

        // This is not emphasis, because the opening `_` is followed by
        // whitespace:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec338_commonmark()
        {
            // The following Markdown:
            //     _ foo bar_
            //
            // Should be rendered as:
            //     <p>_ foo bar_</p>

            SpecTestHelper.AssertCompliance("_ foo bar_", 
                "<p>_ foo bar_</p>", 
                "commonmark");
        }

        // This is not emphasis, because the opening `_` is preceded
        // by an alphanumeric and followed by punctuation:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec339_commonmark()
        {
            // The following Markdown:
            //     a_"foo"_
            //
            // Should be rendered as:
            //     <p>a_&quot;foo&quot;_</p>

            SpecTestHelper.AssertCompliance("a_\"foo\"_", 
                "<p>a_&quot;foo&quot;_</p>", 
                "commonmark");
        }

        // Emphasis with `_` is not allowed inside words:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec340_commonmark()
        {
            // The following Markdown:
            //     foo_bar_
            //
            // Should be rendered as:
            //     <p>foo_bar_</p>

            SpecTestHelper.AssertCompliance("foo_bar_", 
                "<p>foo_bar_</p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec341_commonmark()
        {
            // The following Markdown:
            //     5_6_78
            //
            // Should be rendered as:
            //     <p>5_6_78</p>

            SpecTestHelper.AssertCompliance("5_6_78", 
                "<p>5_6_78</p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec342_commonmark()
        {
            // The following Markdown:
            //     пристаням_стремятся_
            //
            // Should be rendered as:
            //     <p>пристаням_стремятся_</p>

            SpecTestHelper.AssertCompliance("пристаням_стремятся_", 
                "<p>пристаням_стремятся_</p>", 
                "commonmark");
        }

        // Here `_` does not generate emphasis, because the first delimiter run
        // is right-flanking and the second left-flanking:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec343_commonmark()
        {
            // The following Markdown:
            //     aa_"bb"_cc
            //
            // Should be rendered as:
            //     <p>aa_&quot;bb&quot;_cc</p>

            SpecTestHelper.AssertCompliance("aa_\"bb\"_cc", 
                "<p>aa_&quot;bb&quot;_cc</p>", 
                "commonmark");
        }

        // This is emphasis, even though the opening delimiter is
        // both left- and right-flanking, because it is preceded by
        // punctuation:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec344_commonmark()
        {
            // The following Markdown:
            //     foo-_(bar)_
            //
            // Should be rendered as:
            //     <p>foo-<em>(bar)</em></p>

            SpecTestHelper.AssertCompliance("foo-_(bar)_", 
                "<p>foo-<em>(bar)</em></p>", 
                "commonmark");
        }

        // Rule 3:
        // 
        // This is not emphasis, because the closing delimiter does
        // not match the opening delimiter:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec345_commonmark()
        {
            // The following Markdown:
            //     _foo*
            //
            // Should be rendered as:
            //     <p>_foo*</p>

            SpecTestHelper.AssertCompliance("_foo*", 
                "<p>_foo*</p>", 
                "commonmark");
        }

        // This is not emphasis, because the closing `*` is preceded by
        // whitespace:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec346_commonmark()
        {
            // The following Markdown:
            //     *foo bar *
            //
            // Should be rendered as:
            //     <p>*foo bar *</p>

            SpecTestHelper.AssertCompliance("*foo bar *", 
                "<p>*foo bar *</p>", 
                "commonmark");
        }

        // A newline also counts as whitespace:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec347_commonmark()
        {
            // The following Markdown:
            //     *foo bar
            //     *
            //
            // Should be rendered as:
            //     <p>*foo bar
            //     *</p>

            SpecTestHelper.AssertCompliance("*foo bar\n*", 
                "<p>*foo bar\n*</p>", 
                "commonmark");
        }

        // This is not emphasis, because the second `*` is
        // preceded by punctuation and followed by an alphanumeric
        // (hence it is not part of a [right-flanking delimiter run]:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec348_commonmark()
        {
            // The following Markdown:
            //     *(*foo)
            //
            // Should be rendered as:
            //     <p>*(*foo)</p>

            SpecTestHelper.AssertCompliance("*(*foo)", 
                "<p>*(*foo)</p>", 
                "commonmark");
        }

        // The point of this restriction is more easily appreciated
        // with this example:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec349_commonmark()
        {
            // The following Markdown:
            //     *(*foo*)*
            //
            // Should be rendered as:
            //     <p><em>(<em>foo</em>)</em></p>

            SpecTestHelper.AssertCompliance("*(*foo*)*", 
                "<p><em>(<em>foo</em>)</em></p>", 
                "commonmark");
        }

        // Intraword emphasis with `*` is allowed:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec350_commonmark()
        {
            // The following Markdown:
            //     *foo*bar
            //
            // Should be rendered as:
            //     <p><em>foo</em>bar</p>

            SpecTestHelper.AssertCompliance("*foo*bar", 
                "<p><em>foo</em>bar</p>", 
                "commonmark");
        }

        // Rule 4:
        // 
        // This is not emphasis, because the closing `_` is preceded by
        // whitespace:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec351_commonmark()
        {
            // The following Markdown:
            //     _foo bar _
            //
            // Should be rendered as:
            //     <p>_foo bar _</p>

            SpecTestHelper.AssertCompliance("_foo bar _", 
                "<p>_foo bar _</p>", 
                "commonmark");
        }

        // This is not emphasis, because the second `_` is
        // preceded by punctuation and followed by an alphanumeric:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec352_commonmark()
        {
            // The following Markdown:
            //     _(_foo)
            //
            // Should be rendered as:
            //     <p>_(_foo)</p>

            SpecTestHelper.AssertCompliance("_(_foo)", 
                "<p>_(_foo)</p>", 
                "commonmark");
        }

        // This is emphasis within emphasis:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec353_commonmark()
        {
            // The following Markdown:
            //     _(_foo_)_
            //
            // Should be rendered as:
            //     <p><em>(<em>foo</em>)</em></p>

            SpecTestHelper.AssertCompliance("_(_foo_)_", 
                "<p><em>(<em>foo</em>)</em></p>", 
                "commonmark");
        }

        // Intraword emphasis is disallowed for `_`:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec354_commonmark()
        {
            // The following Markdown:
            //     _foo_bar
            //
            // Should be rendered as:
            //     <p>_foo_bar</p>

            SpecTestHelper.AssertCompliance("_foo_bar", 
                "<p>_foo_bar</p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec355_commonmark()
        {
            // The following Markdown:
            //     _пристаням_стремятся
            //
            // Should be rendered as:
            //     <p>_пристаням_стремятся</p>

            SpecTestHelper.AssertCompliance("_пристаням_стремятся", 
                "<p>_пристаням_стремятся</p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec356_commonmark()
        {
            // The following Markdown:
            //     _foo_bar_baz_
            //
            // Should be rendered as:
            //     <p><em>foo_bar_baz</em></p>

            SpecTestHelper.AssertCompliance("_foo_bar_baz_", 
                "<p><em>foo_bar_baz</em></p>", 
                "commonmark");
        }

        // This is emphasis, even though the closing delimiter is
        // both left- and right-flanking, because it is followed by
        // punctuation:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec357_commonmark()
        {
            // The following Markdown:
            //     _(bar)_.
            //
            // Should be rendered as:
            //     <p><em>(bar)</em>.</p>

            SpecTestHelper.AssertCompliance("_(bar)_.", 
                "<p><em>(bar)</em>.</p>", 
                "commonmark");
        }

        // Rule 5:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec358_commonmark()
        {
            // The following Markdown:
            //     **foo bar**
            //
            // Should be rendered as:
            //     <p><strong>foo bar</strong></p>

            SpecTestHelper.AssertCompliance("**foo bar**", 
                "<p><strong>foo bar</strong></p>", 
                "commonmark");
        }

        // This is not strong emphasis, because the opening delimiter is
        // followed by whitespace:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec359_commonmark()
        {
            // The following Markdown:
            //     ** foo bar**
            //
            // Should be rendered as:
            //     <p>** foo bar**</p>

            SpecTestHelper.AssertCompliance("** foo bar**", 
                "<p>** foo bar**</p>", 
                "commonmark");
        }

        // This is not strong emphasis, because the opening `**` is preceded
        // by an alphanumeric and followed by punctuation, and hence
        // not part of a [left-flanking delimiter run]:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec360_commonmark()
        {
            // The following Markdown:
            //     a**"foo"**
            //
            // Should be rendered as:
            //     <p>a**&quot;foo&quot;**</p>

            SpecTestHelper.AssertCompliance("a**\"foo\"**", 
                "<p>a**&quot;foo&quot;**</p>", 
                "commonmark");
        }

        // Intraword strong emphasis with `**` is permitted:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec361_commonmark()
        {
            // The following Markdown:
            //     foo**bar**
            //
            // Should be rendered as:
            //     <p>foo<strong>bar</strong></p>

            SpecTestHelper.AssertCompliance("foo**bar**", 
                "<p>foo<strong>bar</strong></p>", 
                "commonmark");
        }

        // Rule 6:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec362_commonmark()
        {
            // The following Markdown:
            //     __foo bar__
            //
            // Should be rendered as:
            //     <p><strong>foo bar</strong></p>

            SpecTestHelper.AssertCompliance("__foo bar__", 
                "<p><strong>foo bar</strong></p>", 
                "commonmark");
        }

        // This is not strong emphasis, because the opening delimiter is
        // followed by whitespace:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec363_commonmark()
        {
            // The following Markdown:
            //     __ foo bar__
            //
            // Should be rendered as:
            //     <p>__ foo bar__</p>

            SpecTestHelper.AssertCompliance("__ foo bar__", 
                "<p>__ foo bar__</p>", 
                "commonmark");
        }

        // A newline counts as whitespace:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec364_commonmark()
        {
            // The following Markdown:
            //     __
            //     foo bar__
            //
            // Should be rendered as:
            //     <p>__
            //     foo bar__</p>

            SpecTestHelper.AssertCompliance("__\nfoo bar__", 
                "<p>__\nfoo bar__</p>", 
                "commonmark");
        }

        // This is not strong emphasis, because the opening `__` is preceded
        // by an alphanumeric and followed by punctuation:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec365_commonmark()
        {
            // The following Markdown:
            //     a__"foo"__
            //
            // Should be rendered as:
            //     <p>a__&quot;foo&quot;__</p>

            SpecTestHelper.AssertCompliance("a__\"foo\"__", 
                "<p>a__&quot;foo&quot;__</p>", 
                "commonmark");
        }

        // Intraword strong emphasis is forbidden with `__`:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec366_commonmark()
        {
            // The following Markdown:
            //     foo__bar__
            //
            // Should be rendered as:
            //     <p>foo__bar__</p>

            SpecTestHelper.AssertCompliance("foo__bar__", 
                "<p>foo__bar__</p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec367_commonmark()
        {
            // The following Markdown:
            //     5__6__78
            //
            // Should be rendered as:
            //     <p>5__6__78</p>

            SpecTestHelper.AssertCompliance("5__6__78", 
                "<p>5__6__78</p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec368_commonmark()
        {
            // The following Markdown:
            //     пристаням__стремятся__
            //
            // Should be rendered as:
            //     <p>пристаням__стремятся__</p>

            SpecTestHelper.AssertCompliance("пристаням__стремятся__", 
                "<p>пристаням__стремятся__</p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec369_commonmark()
        {
            // The following Markdown:
            //     __foo, __bar__, baz__
            //
            // Should be rendered as:
            //     <p><strong>foo, <strong>bar</strong>, baz</strong></p>

            SpecTestHelper.AssertCompliance("__foo, __bar__, baz__", 
                "<p><strong>foo, <strong>bar</strong>, baz</strong></p>", 
                "commonmark");
        }

        // This is strong emphasis, even though the opening delimiter is
        // both left- and right-flanking, because it is preceded by
        // punctuation:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec370_commonmark()
        {
            // The following Markdown:
            //     foo-__(bar)__
            //
            // Should be rendered as:
            //     <p>foo-<strong>(bar)</strong></p>

            SpecTestHelper.AssertCompliance("foo-__(bar)__", 
                "<p>foo-<strong>(bar)</strong></p>", 
                "commonmark");
        }

        // Rule 7:
        // 
        // This is not strong emphasis, because the closing delimiter is preceded
        // by whitespace:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec371_commonmark()
        {
            // The following Markdown:
            //     **foo bar **
            //
            // Should be rendered as:
            //     <p>**foo bar **</p>

            SpecTestHelper.AssertCompliance("**foo bar **", 
                "<p>**foo bar **</p>", 
                "commonmark");
        }

        // (Nor can it be interpreted as an emphasized `*foo bar *`, because of
        // Rule 11.)
        // 
        // This is not strong emphasis, because the second `**` is
        // preceded by punctuation and followed by an alphanumeric:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec372_commonmark()
        {
            // The following Markdown:
            //     **(**foo)
            //
            // Should be rendered as:
            //     <p>**(**foo)</p>

            SpecTestHelper.AssertCompliance("**(**foo)", 
                "<p>**(**foo)</p>", 
                "commonmark");
        }

        // The point of this restriction is more easily appreciated
        // with these examples:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec373_commonmark()
        {
            // The following Markdown:
            //     *(**foo**)*
            //
            // Should be rendered as:
            //     <p><em>(<strong>foo</strong>)</em></p>

            SpecTestHelper.AssertCompliance("*(**foo**)*", 
                "<p><em>(<strong>foo</strong>)</em></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec374_commonmark()
        {
            // The following Markdown:
            //     **Gomphocarpus (*Gomphocarpus physocarpus*, syn.
            //     *Asclepias physocarpa*)**
            //
            // Should be rendered as:
            //     <p><strong>Gomphocarpus (<em>Gomphocarpus physocarpus</em>, syn.
            //     <em>Asclepias physocarpa</em>)</strong></p>

            SpecTestHelper.AssertCompliance("**Gomphocarpus (*Gomphocarpus physocarpus*, syn.\n*Asclepias physocarpa*)**", 
                "<p><strong>Gomphocarpus (<em>Gomphocarpus physocarpus</em>, syn.\n<em>Asclepias physocarpa</em>)</strong></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec375_commonmark()
        {
            // The following Markdown:
            //     **foo "*bar*" foo**
            //
            // Should be rendered as:
            //     <p><strong>foo &quot;<em>bar</em>&quot; foo</strong></p>

            SpecTestHelper.AssertCompliance("**foo \"*bar*\" foo**", 
                "<p><strong>foo &quot;<em>bar</em>&quot; foo</strong></p>", 
                "commonmark");
        }

        // Intraword emphasis:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec376_commonmark()
        {
            // The following Markdown:
            //     **foo**bar
            //
            // Should be rendered as:
            //     <p><strong>foo</strong>bar</p>

            SpecTestHelper.AssertCompliance("**foo**bar", 
                "<p><strong>foo</strong>bar</p>", 
                "commonmark");
        }

        // Rule 8:
        // 
        // This is not strong emphasis, because the closing delimiter is
        // preceded by whitespace:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec377_commonmark()
        {
            // The following Markdown:
            //     __foo bar __
            //
            // Should be rendered as:
            //     <p>__foo bar __</p>

            SpecTestHelper.AssertCompliance("__foo bar __", 
                "<p>__foo bar __</p>", 
                "commonmark");
        }

        // This is not strong emphasis, because the second `__` is
        // preceded by punctuation and followed by an alphanumeric:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec378_commonmark()
        {
            // The following Markdown:
            //     __(__foo)
            //
            // Should be rendered as:
            //     <p>__(__foo)</p>

            SpecTestHelper.AssertCompliance("__(__foo)", 
                "<p>__(__foo)</p>", 
                "commonmark");
        }

        // The point of this restriction is more easily appreciated
        // with this example:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec379_commonmark()
        {
            // The following Markdown:
            //     _(__foo__)_
            //
            // Should be rendered as:
            //     <p><em>(<strong>foo</strong>)</em></p>

            SpecTestHelper.AssertCompliance("_(__foo__)_", 
                "<p><em>(<strong>foo</strong>)</em></p>", 
                "commonmark");
        }

        // Intraword strong emphasis is forbidden with `__`:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec380_commonmark()
        {
            // The following Markdown:
            //     __foo__bar
            //
            // Should be rendered as:
            //     <p>__foo__bar</p>

            SpecTestHelper.AssertCompliance("__foo__bar", 
                "<p>__foo__bar</p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec381_commonmark()
        {
            // The following Markdown:
            //     __пристаням__стремятся
            //
            // Should be rendered as:
            //     <p>__пристаням__стремятся</p>

            SpecTestHelper.AssertCompliance("__пристаням__стремятся", 
                "<p>__пристаням__стремятся</p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec382_commonmark()
        {
            // The following Markdown:
            //     __foo__bar__baz__
            //
            // Should be rendered as:
            //     <p><strong>foo__bar__baz</strong></p>

            SpecTestHelper.AssertCompliance("__foo__bar__baz__", 
                "<p><strong>foo__bar__baz</strong></p>", 
                "commonmark");
        }

        // This is strong emphasis, even though the closing delimiter is
        // both left- and right-flanking, because it is followed by
        // punctuation:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec383_commonmark()
        {
            // The following Markdown:
            //     __(bar)__.
            //
            // Should be rendered as:
            //     <p><strong>(bar)</strong>.</p>

            SpecTestHelper.AssertCompliance("__(bar)__.", 
                "<p><strong>(bar)</strong>.</p>", 
                "commonmark");
        }

        // Rule 9:
        // 
        // Any nonempty sequence of inline elements can be the contents of an
        // emphasized span.
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec384_commonmark()
        {
            // The following Markdown:
            //     *foo [bar](/url)*
            //
            // Should be rendered as:
            //     <p><em>foo <a href="/url">bar</a></em></p>

            SpecTestHelper.AssertCompliance("*foo [bar](/url)*", 
                "<p><em>foo <a href=\"/url\">bar</a></em></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec385_commonmark()
        {
            // The following Markdown:
            //     *foo
            //     bar*
            //
            // Should be rendered as:
            //     <p><em>foo
            //     bar</em></p>

            SpecTestHelper.AssertCompliance("*foo\nbar*", 
                "<p><em>foo\nbar</em></p>", 
                "commonmark");
        }

        // In particular, emphasis and strong emphasis can be nested
        // inside emphasis:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec386_commonmark()
        {
            // The following Markdown:
            //     _foo __bar__ baz_
            //
            // Should be rendered as:
            //     <p><em>foo <strong>bar</strong> baz</em></p>

            SpecTestHelper.AssertCompliance("_foo __bar__ baz_", 
                "<p><em>foo <strong>bar</strong> baz</em></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec387_commonmark()
        {
            // The following Markdown:
            //     _foo _bar_ baz_
            //
            // Should be rendered as:
            //     <p><em>foo <em>bar</em> baz</em></p>

            SpecTestHelper.AssertCompliance("_foo _bar_ baz_", 
                "<p><em>foo <em>bar</em> baz</em></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec388_commonmark()
        {
            // The following Markdown:
            //     __foo_ bar_
            //
            // Should be rendered as:
            //     <p><em><em>foo</em> bar</em></p>

            SpecTestHelper.AssertCompliance("__foo_ bar_", 
                "<p><em><em>foo</em> bar</em></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec389_commonmark()
        {
            // The following Markdown:
            //     *foo *bar**
            //
            // Should be rendered as:
            //     <p><em>foo <em>bar</em></em></p>

            SpecTestHelper.AssertCompliance("*foo *bar**", 
                "<p><em>foo <em>bar</em></em></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec390_commonmark()
        {
            // The following Markdown:
            //     *foo **bar** baz*
            //
            // Should be rendered as:
            //     <p><em>foo <strong>bar</strong> baz</em></p>

            SpecTestHelper.AssertCompliance("*foo **bar** baz*", 
                "<p><em>foo <strong>bar</strong> baz</em></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec391_commonmark()
        {
            // The following Markdown:
            //     *foo**bar**baz*
            //
            // Should be rendered as:
            //     <p><em>foo<strong>bar</strong>baz</em></p>

            SpecTestHelper.AssertCompliance("*foo**bar**baz*", 
                "<p><em>foo<strong>bar</strong>baz</em></p>", 
                "commonmark");
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
        public void EmphasisAndStrongEmphasis_Spec392_commonmark()
        {
            // The following Markdown:
            //     ***foo** bar*
            //
            // Should be rendered as:
            //     <p><em><strong>foo</strong> bar</em></p>

            SpecTestHelper.AssertCompliance("***foo** bar*", 
                "<p><em><strong>foo</strong> bar</em></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec393_commonmark()
        {
            // The following Markdown:
            //     *foo **bar***
            //
            // Should be rendered as:
            //     <p><em>foo <strong>bar</strong></em></p>

            SpecTestHelper.AssertCompliance("*foo **bar***", 
                "<p><em>foo <strong>bar</strong></em></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec394_commonmark()
        {
            // The following Markdown:
            //     *foo**bar***
            //
            // Should be rendered as:
            //     <p><em>foo<strong>bar</strong></em></p>

            SpecTestHelper.AssertCompliance("*foo**bar***", 
                "<p><em>foo<strong>bar</strong></em></p>", 
                "commonmark");
        }

        // Indefinite levels of nesting are possible:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec395_commonmark()
        {
            // The following Markdown:
            //     *foo **bar *baz* bim** bop*
            //
            // Should be rendered as:
            //     <p><em>foo <strong>bar <em>baz</em> bim</strong> bop</em></p>

            SpecTestHelper.AssertCompliance("*foo **bar *baz* bim** bop*", 
                "<p><em>foo <strong>bar <em>baz</em> bim</strong> bop</em></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec396_commonmark()
        {
            // The following Markdown:
            //     *foo [*bar*](/url)*
            //
            // Should be rendered as:
            //     <p><em>foo <a href="/url"><em>bar</em></a></em></p>

            SpecTestHelper.AssertCompliance("*foo [*bar*](/url)*", 
                "<p><em>foo <a href=\"/url\"><em>bar</em></a></em></p>", 
                "commonmark");
        }

        // There can be no empty emphasis or strong emphasis:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec397_commonmark()
        {
            // The following Markdown:
            //     ** is not an empty emphasis
            //
            // Should be rendered as:
            //     <p>** is not an empty emphasis</p>

            SpecTestHelper.AssertCompliance("** is not an empty emphasis", 
                "<p>** is not an empty emphasis</p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec398_commonmark()
        {
            // The following Markdown:
            //     **** is not an empty strong emphasis
            //
            // Should be rendered as:
            //     <p>**** is not an empty strong emphasis</p>

            SpecTestHelper.AssertCompliance("**** is not an empty strong emphasis", 
                "<p>**** is not an empty strong emphasis</p>", 
                "commonmark");
        }

        // Rule 10:
        // 
        // Any nonempty sequence of inline elements can be the contents of an
        // strongly emphasized span.
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec399_commonmark()
        {
            // The following Markdown:
            //     **foo [bar](/url)**
            //
            // Should be rendered as:
            //     <p><strong>foo <a href="/url">bar</a></strong></p>

            SpecTestHelper.AssertCompliance("**foo [bar](/url)**", 
                "<p><strong>foo <a href=\"/url\">bar</a></strong></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec400_commonmark()
        {
            // The following Markdown:
            //     **foo
            //     bar**
            //
            // Should be rendered as:
            //     <p><strong>foo
            //     bar</strong></p>

            SpecTestHelper.AssertCompliance("**foo\nbar**", 
                "<p><strong>foo\nbar</strong></p>", 
                "commonmark");
        }

        // In particular, emphasis and strong emphasis can be nested
        // inside strong emphasis:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec401_commonmark()
        {
            // The following Markdown:
            //     __foo _bar_ baz__
            //
            // Should be rendered as:
            //     <p><strong>foo <em>bar</em> baz</strong></p>

            SpecTestHelper.AssertCompliance("__foo _bar_ baz__", 
                "<p><strong>foo <em>bar</em> baz</strong></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec402_commonmark()
        {
            // The following Markdown:
            //     __foo __bar__ baz__
            //
            // Should be rendered as:
            //     <p><strong>foo <strong>bar</strong> baz</strong></p>

            SpecTestHelper.AssertCompliance("__foo __bar__ baz__", 
                "<p><strong>foo <strong>bar</strong> baz</strong></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec403_commonmark()
        {
            // The following Markdown:
            //     ____foo__ bar__
            //
            // Should be rendered as:
            //     <p><strong><strong>foo</strong> bar</strong></p>

            SpecTestHelper.AssertCompliance("____foo__ bar__", 
                "<p><strong><strong>foo</strong> bar</strong></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec404_commonmark()
        {
            // The following Markdown:
            //     **foo **bar****
            //
            // Should be rendered as:
            //     <p><strong>foo <strong>bar</strong></strong></p>

            SpecTestHelper.AssertCompliance("**foo **bar****", 
                "<p><strong>foo <strong>bar</strong></strong></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec405_commonmark()
        {
            // The following Markdown:
            //     **foo *bar* baz**
            //
            // Should be rendered as:
            //     <p><strong>foo <em>bar</em> baz</strong></p>

            SpecTestHelper.AssertCompliance("**foo *bar* baz**", 
                "<p><strong>foo <em>bar</em> baz</strong></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec406_commonmark()
        {
            // The following Markdown:
            //     **foo*bar*baz**
            //
            // Should be rendered as:
            //     <p><strong>foo<em>bar</em>baz</strong></p>

            SpecTestHelper.AssertCompliance("**foo*bar*baz**", 
                "<p><strong>foo<em>bar</em>baz</strong></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec407_commonmark()
        {
            // The following Markdown:
            //     ***foo* bar**
            //
            // Should be rendered as:
            //     <p><strong><em>foo</em> bar</strong></p>

            SpecTestHelper.AssertCompliance("***foo* bar**", 
                "<p><strong><em>foo</em> bar</strong></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec408_commonmark()
        {
            // The following Markdown:
            //     **foo *bar***
            //
            // Should be rendered as:
            //     <p><strong>foo <em>bar</em></strong></p>

            SpecTestHelper.AssertCompliance("**foo *bar***", 
                "<p><strong>foo <em>bar</em></strong></p>", 
                "commonmark");
        }

        // Indefinite levels of nesting are possible:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec409_commonmark()
        {
            // The following Markdown:
            //     **foo *bar **baz**
            //     bim* bop**
            //
            // Should be rendered as:
            //     <p><strong>foo <em>bar <strong>baz</strong>
            //     bim</em> bop</strong></p>

            SpecTestHelper.AssertCompliance("**foo *bar **baz**\nbim* bop**", 
                "<p><strong>foo <em>bar <strong>baz</strong>\nbim</em> bop</strong></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec410_commonmark()
        {
            // The following Markdown:
            //     **foo [*bar*](/url)**
            //
            // Should be rendered as:
            //     <p><strong>foo <a href="/url"><em>bar</em></a></strong></p>

            SpecTestHelper.AssertCompliance("**foo [*bar*](/url)**", 
                "<p><strong>foo <a href=\"/url\"><em>bar</em></a></strong></p>", 
                "commonmark");
        }

        // There can be no empty emphasis or strong emphasis:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec411_commonmark()
        {
            // The following Markdown:
            //     __ is not an empty emphasis
            //
            // Should be rendered as:
            //     <p>__ is not an empty emphasis</p>

            SpecTestHelper.AssertCompliance("__ is not an empty emphasis", 
                "<p>__ is not an empty emphasis</p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec412_commonmark()
        {
            // The following Markdown:
            //     ____ is not an empty strong emphasis
            //
            // Should be rendered as:
            //     <p>____ is not an empty strong emphasis</p>

            SpecTestHelper.AssertCompliance("____ is not an empty strong emphasis", 
                "<p>____ is not an empty strong emphasis</p>", 
                "commonmark");
        }

        // Rule 11:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec413_commonmark()
        {
            // The following Markdown:
            //     foo ***
            //
            // Should be rendered as:
            //     <p>foo ***</p>

            SpecTestHelper.AssertCompliance("foo ***", 
                "<p>foo ***</p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec414_commonmark()
        {
            // The following Markdown:
            //     foo *\**
            //
            // Should be rendered as:
            //     <p>foo <em>*</em></p>

            SpecTestHelper.AssertCompliance("foo *\\**", 
                "<p>foo <em>*</em></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec415_commonmark()
        {
            // The following Markdown:
            //     foo *_*
            //
            // Should be rendered as:
            //     <p>foo <em>_</em></p>

            SpecTestHelper.AssertCompliance("foo *_*", 
                "<p>foo <em>_</em></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec416_commonmark()
        {
            // The following Markdown:
            //     foo *****
            //
            // Should be rendered as:
            //     <p>foo *****</p>

            SpecTestHelper.AssertCompliance("foo *****", 
                "<p>foo *****</p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec417_commonmark()
        {
            // The following Markdown:
            //     foo **\***
            //
            // Should be rendered as:
            //     <p>foo <strong>*</strong></p>

            SpecTestHelper.AssertCompliance("foo **\\***", 
                "<p>foo <strong>*</strong></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec418_commonmark()
        {
            // The following Markdown:
            //     foo **_**
            //
            // Should be rendered as:
            //     <p>foo <strong>_</strong></p>

            SpecTestHelper.AssertCompliance("foo **_**", 
                "<p>foo <strong>_</strong></p>", 
                "commonmark");
        }

        // Note that when delimiters do not match evenly, Rule 11 determines
        // that the excess literal `*` characters will appear outside of the
        // emphasis, rather than inside it:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec419_commonmark()
        {
            // The following Markdown:
            //     **foo*
            //
            // Should be rendered as:
            //     <p>*<em>foo</em></p>

            SpecTestHelper.AssertCompliance("**foo*", 
                "<p>*<em>foo</em></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec420_commonmark()
        {
            // The following Markdown:
            //     *foo**
            //
            // Should be rendered as:
            //     <p><em>foo</em>*</p>

            SpecTestHelper.AssertCompliance("*foo**", 
                "<p><em>foo</em>*</p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec421_commonmark()
        {
            // The following Markdown:
            //     ***foo**
            //
            // Should be rendered as:
            //     <p>*<strong>foo</strong></p>

            SpecTestHelper.AssertCompliance("***foo**", 
                "<p>*<strong>foo</strong></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec422_commonmark()
        {
            // The following Markdown:
            //     ****foo*
            //
            // Should be rendered as:
            //     <p>***<em>foo</em></p>

            SpecTestHelper.AssertCompliance("****foo*", 
                "<p>***<em>foo</em></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec423_commonmark()
        {
            // The following Markdown:
            //     **foo***
            //
            // Should be rendered as:
            //     <p><strong>foo</strong>*</p>

            SpecTestHelper.AssertCompliance("**foo***", 
                "<p><strong>foo</strong>*</p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec424_commonmark()
        {
            // The following Markdown:
            //     *foo****
            //
            // Should be rendered as:
            //     <p><em>foo</em>***</p>

            SpecTestHelper.AssertCompliance("*foo****", 
                "<p><em>foo</em>***</p>", 
                "commonmark");
        }

        // Rule 12:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec425_commonmark()
        {
            // The following Markdown:
            //     foo ___
            //
            // Should be rendered as:
            //     <p>foo ___</p>

            SpecTestHelper.AssertCompliance("foo ___", 
                "<p>foo ___</p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec426_commonmark()
        {
            // The following Markdown:
            //     foo _\__
            //
            // Should be rendered as:
            //     <p>foo <em>_</em></p>

            SpecTestHelper.AssertCompliance("foo _\\__", 
                "<p>foo <em>_</em></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec427_commonmark()
        {
            // The following Markdown:
            //     foo _*_
            //
            // Should be rendered as:
            //     <p>foo <em>*</em></p>

            SpecTestHelper.AssertCompliance("foo _*_", 
                "<p>foo <em>*</em></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec428_commonmark()
        {
            // The following Markdown:
            //     foo _____
            //
            // Should be rendered as:
            //     <p>foo _____</p>

            SpecTestHelper.AssertCompliance("foo _____", 
                "<p>foo _____</p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec429_commonmark()
        {
            // The following Markdown:
            //     foo __\___
            //
            // Should be rendered as:
            //     <p>foo <strong>_</strong></p>

            SpecTestHelper.AssertCompliance("foo __\\___", 
                "<p>foo <strong>_</strong></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec430_commonmark()
        {
            // The following Markdown:
            //     foo __*__
            //
            // Should be rendered as:
            //     <p>foo <strong>*</strong></p>

            SpecTestHelper.AssertCompliance("foo __*__", 
                "<p>foo <strong>*</strong></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec431_commonmark()
        {
            // The following Markdown:
            //     __foo_
            //
            // Should be rendered as:
            //     <p>_<em>foo</em></p>

            SpecTestHelper.AssertCompliance("__foo_", 
                "<p>_<em>foo</em></p>", 
                "commonmark");
        }

        // Note that when delimiters do not match evenly, Rule 12 determines
        // that the excess literal `_` characters will appear outside of the
        // emphasis, rather than inside it:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec432_commonmark()
        {
            // The following Markdown:
            //     _foo__
            //
            // Should be rendered as:
            //     <p><em>foo</em>_</p>

            SpecTestHelper.AssertCompliance("_foo__", 
                "<p><em>foo</em>_</p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec433_commonmark()
        {
            // The following Markdown:
            //     ___foo__
            //
            // Should be rendered as:
            //     <p>_<strong>foo</strong></p>

            SpecTestHelper.AssertCompliance("___foo__", 
                "<p>_<strong>foo</strong></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec434_commonmark()
        {
            // The following Markdown:
            //     ____foo_
            //
            // Should be rendered as:
            //     <p>___<em>foo</em></p>

            SpecTestHelper.AssertCompliance("____foo_", 
                "<p>___<em>foo</em></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec435_commonmark()
        {
            // The following Markdown:
            //     __foo___
            //
            // Should be rendered as:
            //     <p><strong>foo</strong>_</p>

            SpecTestHelper.AssertCompliance("__foo___", 
                "<p><strong>foo</strong>_</p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec436_commonmark()
        {
            // The following Markdown:
            //     _foo____
            //
            // Should be rendered as:
            //     <p><em>foo</em>___</p>

            SpecTestHelper.AssertCompliance("_foo____", 
                "<p><em>foo</em>___</p>", 
                "commonmark");
        }

        // Rule 13 implies that if you want emphasis nested directly inside
        // emphasis, you must use different delimiters:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec437_commonmark()
        {
            // The following Markdown:
            //     **foo**
            //
            // Should be rendered as:
            //     <p><strong>foo</strong></p>

            SpecTestHelper.AssertCompliance("**foo**", 
                "<p><strong>foo</strong></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec438_commonmark()
        {
            // The following Markdown:
            //     *_foo_*
            //
            // Should be rendered as:
            //     <p><em><em>foo</em></em></p>

            SpecTestHelper.AssertCompliance("*_foo_*", 
                "<p><em><em>foo</em></em></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec439_commonmark()
        {
            // The following Markdown:
            //     __foo__
            //
            // Should be rendered as:
            //     <p><strong>foo</strong></p>

            SpecTestHelper.AssertCompliance("__foo__", 
                "<p><strong>foo</strong></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec440_commonmark()
        {
            // The following Markdown:
            //     _*foo*_
            //
            // Should be rendered as:
            //     <p><em><em>foo</em></em></p>

            SpecTestHelper.AssertCompliance("_*foo*_", 
                "<p><em><em>foo</em></em></p>", 
                "commonmark");
        }

        // However, strong emphasis within strong emphasis is possible without
        // switching delimiters:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec441_commonmark()
        {
            // The following Markdown:
            //     ****foo****
            //
            // Should be rendered as:
            //     <p><strong><strong>foo</strong></strong></p>

            SpecTestHelper.AssertCompliance("****foo****", 
                "<p><strong><strong>foo</strong></strong></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec442_commonmark()
        {
            // The following Markdown:
            //     ____foo____
            //
            // Should be rendered as:
            //     <p><strong><strong>foo</strong></strong></p>

            SpecTestHelper.AssertCompliance("____foo____", 
                "<p><strong><strong>foo</strong></strong></p>", 
                "commonmark");
        }

        // Rule 13 can be applied to arbitrarily long sequences of
        // delimiters:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec443_commonmark()
        {
            // The following Markdown:
            //     ******foo******
            //
            // Should be rendered as:
            //     <p><strong><strong><strong>foo</strong></strong></strong></p>

            SpecTestHelper.AssertCompliance("******foo******", 
                "<p><strong><strong><strong>foo</strong></strong></strong></p>", 
                "commonmark");
        }

        // Rule 14:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec444_commonmark()
        {
            // The following Markdown:
            //     ***foo***
            //
            // Should be rendered as:
            //     <p><em><strong>foo</strong></em></p>

            SpecTestHelper.AssertCompliance("***foo***", 
                "<p><em><strong>foo</strong></em></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec445_commonmark()
        {
            // The following Markdown:
            //     _____foo_____
            //
            // Should be rendered as:
            //     <p><em><strong><strong>foo</strong></strong></em></p>

            SpecTestHelper.AssertCompliance("_____foo_____", 
                "<p><em><strong><strong>foo</strong></strong></em></p>", 
                "commonmark");
        }

        // Rule 15:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec446_commonmark()
        {
            // The following Markdown:
            //     *foo _bar* baz_
            //
            // Should be rendered as:
            //     <p><em>foo _bar</em> baz_</p>

            SpecTestHelper.AssertCompliance("*foo _bar* baz_", 
                "<p><em>foo _bar</em> baz_</p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec447_commonmark()
        {
            // The following Markdown:
            //     *foo __bar *baz bim__ bam*
            //
            // Should be rendered as:
            //     <p><em>foo <strong>bar *baz bim</strong> bam</em></p>

            SpecTestHelper.AssertCompliance("*foo __bar *baz bim__ bam*", 
                "<p><em>foo <strong>bar *baz bim</strong> bam</em></p>", 
                "commonmark");
        }

        // Rule 16:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec448_commonmark()
        {
            // The following Markdown:
            //     **foo **bar baz**
            //
            // Should be rendered as:
            //     <p>**foo <strong>bar baz</strong></p>

            SpecTestHelper.AssertCompliance("**foo **bar baz**", 
                "<p>**foo <strong>bar baz</strong></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec449_commonmark()
        {
            // The following Markdown:
            //     *foo *bar baz*
            //
            // Should be rendered as:
            //     <p>*foo <em>bar baz</em></p>

            SpecTestHelper.AssertCompliance("*foo *bar baz*", 
                "<p>*foo <em>bar baz</em></p>", 
                "commonmark");
        }

        // Rule 17:
        [Fact]
        public void EmphasisAndStrongEmphasis_Spec450_commonmark()
        {
            // The following Markdown:
            //     *[bar*](/url)
            //
            // Should be rendered as:
            //     <p>*<a href="/url">bar*</a></p>

            SpecTestHelper.AssertCompliance("*[bar*](/url)", 
                "<p>*<a href=\"/url\">bar*</a></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec451_commonmark()
        {
            // The following Markdown:
            //     _foo [bar_](/url)
            //
            // Should be rendered as:
            //     <p>_foo <a href="/url">bar_</a></p>

            SpecTestHelper.AssertCompliance("_foo [bar_](/url)", 
                "<p>_foo <a href=\"/url\">bar_</a></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec452_commonmark()
        {
            // The following Markdown:
            //     *<img src="foo" title="*"/>
            //
            // Should be rendered as:
            //     <p>*<img src="foo" title="*"/></p>

            SpecTestHelper.AssertCompliance("*<img src=\"foo\" title=\"*\"/>", 
                "<p>*<img src=\"foo\" title=\"*\"/></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec453_commonmark()
        {
            // The following Markdown:
            //     **<a href="**">
            //
            // Should be rendered as:
            //     <p>**<a href="**"></p>

            SpecTestHelper.AssertCompliance("**<a href=\"**\">", 
                "<p>**<a href=\"**\"></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec454_commonmark()
        {
            // The following Markdown:
            //     __<a href="__">
            //
            // Should be rendered as:
            //     <p>__<a href="__"></p>

            SpecTestHelper.AssertCompliance("__<a href=\"__\">", 
                "<p>__<a href=\"__\"></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec455_commonmark()
        {
            // The following Markdown:
            //     *a `*`*
            //
            // Should be rendered as:
            //     <p><em>a <code>*</code></em></p>

            SpecTestHelper.AssertCompliance("*a `*`*", 
                "<p><em>a <code>*</code></em></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec456_commonmark()
        {
            // The following Markdown:
            //     _a `_`_
            //
            // Should be rendered as:
            //     <p><em>a <code>_</code></em></p>

            SpecTestHelper.AssertCompliance("_a `_`_", 
                "<p><em>a <code>_</code></em></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec457_commonmark()
        {
            // The following Markdown:
            //     **a<http://foo.bar/?q=**>
            //
            // Should be rendered as:
            //     <p>**a<a href="http://foo.bar/?q=**">http://foo.bar/?q=**</a></p>

            SpecTestHelper.AssertCompliance("**a<http://foo.bar/?q=**>", 
                "<p>**a<a href=\"http://foo.bar/?q=**\">http://foo.bar/?q=**</a></p>", 
                "commonmark");
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec458_commonmark()
        {
            // The following Markdown:
            //     __a<http://foo.bar/?q=__>
            //
            // Should be rendered as:
            //     <p>__a<a href="http://foo.bar/?q=__">http://foo.bar/?q=__</a></p>

            SpecTestHelper.AssertCompliance("__a<http://foo.bar/?q=__>", 
                "<p>__a<a href=\"http://foo.bar/?q=__\">http://foo.bar/?q=__</a></p>", 
                "commonmark");
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
        public void Links_Spec459_commonmark()
        {
            // The following Markdown:
            //     [link](/uri "title")
            //
            // Should be rendered as:
            //     <p><a href="/uri" title="title">link</a></p>

            SpecTestHelper.AssertCompliance("[link](/uri \"title\")", 
                "<p><a href=\"/uri\" title=\"title\">link</a></p>", 
                "commonmark");
        }

        // The title may be omitted:
        [Fact]
        public void Links_Spec460_commonmark()
        {
            // The following Markdown:
            //     [link](/uri)
            //
            // Should be rendered as:
            //     <p><a href="/uri">link</a></p>

            SpecTestHelper.AssertCompliance("[link](/uri)", 
                "<p><a href=\"/uri\">link</a></p>", 
                "commonmark");
        }

        // Both the title and the destination may be omitted:
        [Fact]
        public void Links_Spec461_commonmark()
        {
            // The following Markdown:
            //     [link]()
            //
            // Should be rendered as:
            //     <p><a href="">link</a></p>

            SpecTestHelper.AssertCompliance("[link]()", 
                "<p><a href=\"\">link</a></p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec462_commonmark()
        {
            // The following Markdown:
            //     [link](<>)
            //
            // Should be rendered as:
            //     <p><a href="">link</a></p>

            SpecTestHelper.AssertCompliance("[link](<>)", 
                "<p><a href=\"\">link</a></p>", 
                "commonmark");
        }

        // The destination cannot contain spaces or line breaks,
        // even if enclosed in pointy brackets:
        [Fact]
        public void Links_Spec463_commonmark()
        {
            // The following Markdown:
            //     [link](/my uri)
            //
            // Should be rendered as:
            //     <p>[link](/my uri)</p>

            SpecTestHelper.AssertCompliance("[link](/my uri)", 
                "<p>[link](/my uri)</p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec464_commonmark()
        {
            // The following Markdown:
            //     [link](</my uri>)
            //
            // Should be rendered as:
            //     <p>[link](&lt;/my uri&gt;)</p>

            SpecTestHelper.AssertCompliance("[link](</my uri>)", 
                "<p>[link](&lt;/my uri&gt;)</p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec465_commonmark()
        {
            // The following Markdown:
            //     [link](foo
            //     bar)
            //
            // Should be rendered as:
            //     <p>[link](foo
            //     bar)</p>

            SpecTestHelper.AssertCompliance("[link](foo\nbar)", 
                "<p>[link](foo\nbar)</p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec466_commonmark()
        {
            // The following Markdown:
            //     [link](<foo
            //     bar>)
            //
            // Should be rendered as:
            //     <p>[link](<foo
            //     bar>)</p>

            SpecTestHelper.AssertCompliance("[link](<foo\nbar>)", 
                "<p>[link](<foo\nbar>)</p>", 
                "commonmark");
        }

        // Parentheses inside the link destination may be escaped:
        [Fact]
        public void Links_Spec467_commonmark()
        {
            // The following Markdown:
            //     [link](\(foo\))
            //
            // Should be rendered as:
            //     <p><a href="(foo)">link</a></p>

            SpecTestHelper.AssertCompliance("[link](\\(foo\\))", 
                "<p><a href=\"(foo)\">link</a></p>", 
                "commonmark");
        }

        // Any number of parentheses are allowed without escaping, as long as they are
        // balanced:
        [Fact]
        public void Links_Spec468_commonmark()
        {
            // The following Markdown:
            //     [link](foo(and(bar)))
            //
            // Should be rendered as:
            //     <p><a href="foo(and(bar))">link</a></p>

            SpecTestHelper.AssertCompliance("[link](foo(and(bar)))", 
                "<p><a href=\"foo(and(bar))\">link</a></p>", 
                "commonmark");
        }

        // However, if you have unbalanced parentheses, you need to escape or use the
        // `<...>` form:
        [Fact]
        public void Links_Spec469_commonmark()
        {
            // The following Markdown:
            //     [link](foo\(and\(bar\))
            //
            // Should be rendered as:
            //     <p><a href="foo(and(bar)">link</a></p>

            SpecTestHelper.AssertCompliance("[link](foo\\(and\\(bar\\))", 
                "<p><a href=\"foo(and(bar)\">link</a></p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec470_commonmark()
        {
            // The following Markdown:
            //     [link](<foo(and(bar)>)
            //
            // Should be rendered as:
            //     <p><a href="foo(and(bar)">link</a></p>

            SpecTestHelper.AssertCompliance("[link](<foo(and(bar)>)", 
                "<p><a href=\"foo(and(bar)\">link</a></p>", 
                "commonmark");
        }

        // Parentheses and other symbols can also be escaped, as usual
        // in Markdown:
        [Fact]
        public void Links_Spec471_commonmark()
        {
            // The following Markdown:
            //     [link](foo\)\:)
            //
            // Should be rendered as:
            //     <p><a href="foo):">link</a></p>

            SpecTestHelper.AssertCompliance("[link](foo\\)\\:)", 
                "<p><a href=\"foo):\">link</a></p>", 
                "commonmark");
        }

        // A link can contain fragment identifiers and queries:
        [Fact]
        public void Links_Spec472_commonmark()
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

            SpecTestHelper.AssertCompliance("[link](#fragment)\n\n[link](http://example.com#fragment)\n\n[link](http://example.com?foo=3#frag)", 
                "<p><a href=\"#fragment\">link</a></p>\n<p><a href=\"http://example.com#fragment\">link</a></p>\n<p><a href=\"http://example.com?foo=3#frag\">link</a></p>", 
                "commonmark");
        }

        // Note that a backslash before a non-escapable character is
        // just a backslash:
        [Fact]
        public void Links_Spec473_commonmark()
        {
            // The following Markdown:
            //     [link](foo\bar)
            //
            // Should be rendered as:
            //     <p><a href="foo%5Cbar">link</a></p>

            SpecTestHelper.AssertCompliance("[link](foo\\bar)", 
                "<p><a href=\"foo%5Cbar\">link</a></p>", 
                "commonmark");
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
        public void Links_Spec474_commonmark()
        {
            // The following Markdown:
            //     [link](foo%20b&auml;)
            //
            // Should be rendered as:
            //     <p><a href="foo%20b%C3%A4">link</a></p>

            SpecTestHelper.AssertCompliance("[link](foo%20b&auml;)", 
                "<p><a href=\"foo%20b%C3%A4\">link</a></p>", 
                "commonmark");
        }

        // Note that, because titles can often be parsed as destinations,
        // if you try to omit the destination and keep the title, you'll
        // get unexpected results:
        [Fact]
        public void Links_Spec475_commonmark()
        {
            // The following Markdown:
            //     [link]("title")
            //
            // Should be rendered as:
            //     <p><a href="%22title%22">link</a></p>

            SpecTestHelper.AssertCompliance("[link](\"title\")", 
                "<p><a href=\"%22title%22\">link</a></p>", 
                "commonmark");
        }

        // Titles may be in single quotes, double quotes, or parentheses:
        [Fact]
        public void Links_Spec476_commonmark()
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

            SpecTestHelper.AssertCompliance("[link](/url \"title\")\n[link](/url 'title')\n[link](/url (title))", 
                "<p><a href=\"/url\" title=\"title\">link</a>\n<a href=\"/url\" title=\"title\">link</a>\n<a href=\"/url\" title=\"title\">link</a></p>", 
                "commonmark");
        }

        // Backslash escapes and entity and numeric character references
        // may be used in titles:
        [Fact]
        public void Links_Spec477_commonmark()
        {
            // The following Markdown:
            //     [link](/url "title \"&quot;")
            //
            // Should be rendered as:
            //     <p><a href="/url" title="title &quot;&quot;">link</a></p>

            SpecTestHelper.AssertCompliance("[link](/url \"title \\\"&quot;\")", 
                "<p><a href=\"/url\" title=\"title &quot;&quot;\">link</a></p>", 
                "commonmark");
        }

        // Titles must be separated from the link using a [whitespace].
        // Other [Unicode whitespace] like non-breaking space doesn't work.
        [Fact]
        public void Links_Spec478_commonmark()
        {
            // The following Markdown:
            //     [link](/url "title")
            //
            // Should be rendered as:
            //     <p><a href="/url%C2%A0%22title%22">link</a></p>

            SpecTestHelper.AssertCompliance("[link](/url \"title\")", 
                "<p><a href=\"/url%C2%A0%22title%22\">link</a></p>", 
                "commonmark");
        }

        // Nested balanced quotes are not allowed without escaping:
        [Fact]
        public void Links_Spec479_commonmark()
        {
            // The following Markdown:
            //     [link](/url "title "and" title")
            //
            // Should be rendered as:
            //     <p>[link](/url &quot;title &quot;and&quot; title&quot;)</p>

            SpecTestHelper.AssertCompliance("[link](/url \"title \"and\" title\")", 
                "<p>[link](/url &quot;title &quot;and&quot; title&quot;)</p>", 
                "commonmark");
        }

        // But it is easy to work around this by using a different quote type:
        [Fact]
        public void Links_Spec480_commonmark()
        {
            // The following Markdown:
            //     [link](/url 'title "and" title')
            //
            // Should be rendered as:
            //     <p><a href="/url" title="title &quot;and&quot; title">link</a></p>

            SpecTestHelper.AssertCompliance("[link](/url 'title \"and\" title')", 
                "<p><a href=\"/url\" title=\"title &quot;and&quot; title\">link</a></p>", 
                "commonmark");
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
        public void Links_Spec481_commonmark()
        {
            // The following Markdown:
            //     [link](   /uri
            //       "title"  )
            //
            // Should be rendered as:
            //     <p><a href="/uri" title="title">link</a></p>

            SpecTestHelper.AssertCompliance("[link](   /uri\n  \"title\"  )", 
                "<p><a href=\"/uri\" title=\"title\">link</a></p>", 
                "commonmark");
        }

        // But it is not allowed between the link text and the
        // following parenthesis:
        [Fact]
        public void Links_Spec482_commonmark()
        {
            // The following Markdown:
            //     [link] (/uri)
            //
            // Should be rendered as:
            //     <p>[link] (/uri)</p>

            SpecTestHelper.AssertCompliance("[link] (/uri)", 
                "<p>[link] (/uri)</p>", 
                "commonmark");
        }

        // The link text may contain balanced brackets, but not unbalanced ones,
        // unless they are escaped:
        [Fact]
        public void Links_Spec483_commonmark()
        {
            // The following Markdown:
            //     [link [foo [bar]]](/uri)
            //
            // Should be rendered as:
            //     <p><a href="/uri">link [foo [bar]]</a></p>

            SpecTestHelper.AssertCompliance("[link [foo [bar]]](/uri)", 
                "<p><a href=\"/uri\">link [foo [bar]]</a></p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec484_commonmark()
        {
            // The following Markdown:
            //     [link] bar](/uri)
            //
            // Should be rendered as:
            //     <p>[link] bar](/uri)</p>

            SpecTestHelper.AssertCompliance("[link] bar](/uri)", 
                "<p>[link] bar](/uri)</p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec485_commonmark()
        {
            // The following Markdown:
            //     [link [bar](/uri)
            //
            // Should be rendered as:
            //     <p>[link <a href="/uri">bar</a></p>

            SpecTestHelper.AssertCompliance("[link [bar](/uri)", 
                "<p>[link <a href=\"/uri\">bar</a></p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec486_commonmark()
        {
            // The following Markdown:
            //     [link \[bar](/uri)
            //
            // Should be rendered as:
            //     <p><a href="/uri">link [bar</a></p>

            SpecTestHelper.AssertCompliance("[link \\[bar](/uri)", 
                "<p><a href=\"/uri\">link [bar</a></p>", 
                "commonmark");
        }

        // The link text may contain inline content:
        [Fact]
        public void Links_Spec487_commonmark()
        {
            // The following Markdown:
            //     [link *foo **bar** `#`*](/uri)
            //
            // Should be rendered as:
            //     <p><a href="/uri">link <em>foo <strong>bar</strong> <code>#</code></em></a></p>

            SpecTestHelper.AssertCompliance("[link *foo **bar** `#`*](/uri)", 
                "<p><a href=\"/uri\">link <em>foo <strong>bar</strong> <code>#</code></em></a></p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec488_commonmark()
        {
            // The following Markdown:
            //     [![moon](moon.jpg)](/uri)
            //
            // Should be rendered as:
            //     <p><a href="/uri"><img src="moon.jpg" alt="moon" /></a></p>

            SpecTestHelper.AssertCompliance("[![moon](moon.jpg)](/uri)", 
                "<p><a href=\"/uri\"><img src=\"moon.jpg\" alt=\"moon\" /></a></p>", 
                "commonmark");
        }

        // However, links may not contain other links, at any level of nesting.
        [Fact]
        public void Links_Spec489_commonmark()
        {
            // The following Markdown:
            //     [foo [bar](/uri)](/uri)
            //
            // Should be rendered as:
            //     <p>[foo <a href="/uri">bar</a>](/uri)</p>

            SpecTestHelper.AssertCompliance("[foo [bar](/uri)](/uri)", 
                "<p>[foo <a href=\"/uri\">bar</a>](/uri)</p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec490_commonmark()
        {
            // The following Markdown:
            //     [foo *[bar [baz](/uri)](/uri)*](/uri)
            //
            // Should be rendered as:
            //     <p>[foo <em>[bar <a href="/uri">baz</a>](/uri)</em>](/uri)</p>

            SpecTestHelper.AssertCompliance("[foo *[bar [baz](/uri)](/uri)*](/uri)", 
                "<p>[foo <em>[bar <a href=\"/uri\">baz</a>](/uri)</em>](/uri)</p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec491_commonmark()
        {
            // The following Markdown:
            //     ![[[foo](uri1)](uri2)](uri3)
            //
            // Should be rendered as:
            //     <p><img src="uri3" alt="[foo](uri2)" /></p>

            SpecTestHelper.AssertCompliance("![[[foo](uri1)](uri2)](uri3)", 
                "<p><img src=\"uri3\" alt=\"[foo](uri2)\" /></p>", 
                "commonmark");
        }

        // These cases illustrate the precedence of link text grouping over
        // emphasis grouping:
        [Fact]
        public void Links_Spec492_commonmark()
        {
            // The following Markdown:
            //     *[foo*](/uri)
            //
            // Should be rendered as:
            //     <p>*<a href="/uri">foo*</a></p>

            SpecTestHelper.AssertCompliance("*[foo*](/uri)", 
                "<p>*<a href=\"/uri\">foo*</a></p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec493_commonmark()
        {
            // The following Markdown:
            //     [foo *bar](baz*)
            //
            // Should be rendered as:
            //     <p><a href="baz*">foo *bar</a></p>

            SpecTestHelper.AssertCompliance("[foo *bar](baz*)", 
                "<p><a href=\"baz*\">foo *bar</a></p>", 
                "commonmark");
        }

        // Note that brackets that *aren't* part of links do not take
        // precedence:
        [Fact]
        public void Links_Spec494_commonmark()
        {
            // The following Markdown:
            //     *foo [bar* baz]
            //
            // Should be rendered as:
            //     <p><em>foo [bar</em> baz]</p>

            SpecTestHelper.AssertCompliance("*foo [bar* baz]", 
                "<p><em>foo [bar</em> baz]</p>", 
                "commonmark");
        }

        // These cases illustrate the precedence of HTML tags, code spans,
        // and autolinks over link grouping:
        [Fact]
        public void Links_Spec495_commonmark()
        {
            // The following Markdown:
            //     [foo <bar attr="](baz)">
            //
            // Should be rendered as:
            //     <p>[foo <bar attr="](baz)"></p>

            SpecTestHelper.AssertCompliance("[foo <bar attr=\"](baz)\">", 
                "<p>[foo <bar attr=\"](baz)\"></p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec496_commonmark()
        {
            // The following Markdown:
            //     [foo`](/uri)`
            //
            // Should be rendered as:
            //     <p>[foo<code>](/uri)</code></p>

            SpecTestHelper.AssertCompliance("[foo`](/uri)`", 
                "<p>[foo<code>](/uri)</code></p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec497_commonmark()
        {
            // The following Markdown:
            //     [foo<http://example.com/?search=](uri)>
            //
            // Should be rendered as:
            //     <p>[foo<a href="http://example.com/?search=%5D(uri)">http://example.com/?search=](uri)</a></p>

            SpecTestHelper.AssertCompliance("[foo<http://example.com/?search=](uri)>", 
                "<p>[foo<a href=\"http://example.com/?search=%5D(uri)\">http://example.com/?search=](uri)</a></p>", 
                "commonmark");
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
        public void Links_Spec498_commonmark()
        {
            // The following Markdown:
            //     [foo][bar]
            //     
            //     [bar]: /url "title"
            //
            // Should be rendered as:
            //     <p><a href="/url" title="title">foo</a></p>

            SpecTestHelper.AssertCompliance("[foo][bar]\n\n[bar]: /url \"title\"", 
                "<p><a href=\"/url\" title=\"title\">foo</a></p>", 
                "commonmark");
        }

        // The rules for the [link text] are the same as with
        // [inline links].  Thus:
        // 
        // The link text may contain balanced brackets, but not unbalanced ones,
        // unless they are escaped:
        [Fact]
        public void Links_Spec499_commonmark()
        {
            // The following Markdown:
            //     [link [foo [bar]]][ref]
            //     
            //     [ref]: /uri
            //
            // Should be rendered as:
            //     <p><a href="/uri">link [foo [bar]]</a></p>

            SpecTestHelper.AssertCompliance("[link [foo [bar]]][ref]\n\n[ref]: /uri", 
                "<p><a href=\"/uri\">link [foo [bar]]</a></p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec500_commonmark()
        {
            // The following Markdown:
            //     [link \[bar][ref]
            //     
            //     [ref]: /uri
            //
            // Should be rendered as:
            //     <p><a href="/uri">link [bar</a></p>

            SpecTestHelper.AssertCompliance("[link \\[bar][ref]\n\n[ref]: /uri", 
                "<p><a href=\"/uri\">link [bar</a></p>", 
                "commonmark");
        }

        // The link text may contain inline content:
        [Fact]
        public void Links_Spec501_commonmark()
        {
            // The following Markdown:
            //     [link *foo **bar** `#`*][ref]
            //     
            //     [ref]: /uri
            //
            // Should be rendered as:
            //     <p><a href="/uri">link <em>foo <strong>bar</strong> <code>#</code></em></a></p>

            SpecTestHelper.AssertCompliance("[link *foo **bar** `#`*][ref]\n\n[ref]: /uri", 
                "<p><a href=\"/uri\">link <em>foo <strong>bar</strong> <code>#</code></em></a></p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec502_commonmark()
        {
            // The following Markdown:
            //     [![moon](moon.jpg)][ref]
            //     
            //     [ref]: /uri
            //
            // Should be rendered as:
            //     <p><a href="/uri"><img src="moon.jpg" alt="moon" /></a></p>

            SpecTestHelper.AssertCompliance("[![moon](moon.jpg)][ref]\n\n[ref]: /uri", 
                "<p><a href=\"/uri\"><img src=\"moon.jpg\" alt=\"moon\" /></a></p>", 
                "commonmark");
        }

        // However, links may not contain other links, at any level of nesting.
        [Fact]
        public void Links_Spec503_commonmark()
        {
            // The following Markdown:
            //     [foo [bar](/uri)][ref]
            //     
            //     [ref]: /uri
            //
            // Should be rendered as:
            //     <p>[foo <a href="/uri">bar</a>]<a href="/uri">ref</a></p>

            SpecTestHelper.AssertCompliance("[foo [bar](/uri)][ref]\n\n[ref]: /uri", 
                "<p>[foo <a href=\"/uri\">bar</a>]<a href=\"/uri\">ref</a></p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec504_commonmark()
        {
            // The following Markdown:
            //     [foo *bar [baz][ref]*][ref]
            //     
            //     [ref]: /uri
            //
            // Should be rendered as:
            //     <p>[foo <em>bar <a href="/uri">baz</a></em>]<a href="/uri">ref</a></p>

            SpecTestHelper.AssertCompliance("[foo *bar [baz][ref]*][ref]\n\n[ref]: /uri", 
                "<p>[foo <em>bar <a href=\"/uri\">baz</a></em>]<a href=\"/uri\">ref</a></p>", 
                "commonmark");
        }

        // (In the examples above, we have two [shortcut reference links]
        // instead of one [full reference link].)
        // 
        // The following cases illustrate the precedence of link text grouping over
        // emphasis grouping:
        [Fact]
        public void Links_Spec505_commonmark()
        {
            // The following Markdown:
            //     *[foo*][ref]
            //     
            //     [ref]: /uri
            //
            // Should be rendered as:
            //     <p>*<a href="/uri">foo*</a></p>

            SpecTestHelper.AssertCompliance("*[foo*][ref]\n\n[ref]: /uri", 
                "<p>*<a href=\"/uri\">foo*</a></p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec506_commonmark()
        {
            // The following Markdown:
            //     [foo *bar][ref]
            //     
            //     [ref]: /uri
            //
            // Should be rendered as:
            //     <p><a href="/uri">foo *bar</a></p>

            SpecTestHelper.AssertCompliance("[foo *bar][ref]\n\n[ref]: /uri", 
                "<p><a href=\"/uri\">foo *bar</a></p>", 
                "commonmark");
        }

        // These cases illustrate the precedence of HTML tags, code spans,
        // and autolinks over link grouping:
        [Fact]
        public void Links_Spec507_commonmark()
        {
            // The following Markdown:
            //     [foo <bar attr="][ref]">
            //     
            //     [ref]: /uri
            //
            // Should be rendered as:
            //     <p>[foo <bar attr="][ref]"></p>

            SpecTestHelper.AssertCompliance("[foo <bar attr=\"][ref]\">\n\n[ref]: /uri", 
                "<p>[foo <bar attr=\"][ref]\"></p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec508_commonmark()
        {
            // The following Markdown:
            //     [foo`][ref]`
            //     
            //     [ref]: /uri
            //
            // Should be rendered as:
            //     <p>[foo<code>][ref]</code></p>

            SpecTestHelper.AssertCompliance("[foo`][ref]`\n\n[ref]: /uri", 
                "<p>[foo<code>][ref]</code></p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec509_commonmark()
        {
            // The following Markdown:
            //     [foo<http://example.com/?search=][ref]>
            //     
            //     [ref]: /uri
            //
            // Should be rendered as:
            //     <p>[foo<a href="http://example.com/?search=%5D%5Bref%5D">http://example.com/?search=][ref]</a></p>

            SpecTestHelper.AssertCompliance("[foo<http://example.com/?search=][ref]>\n\n[ref]: /uri", 
                "<p>[foo<a href=\"http://example.com/?search=%5D%5Bref%5D\">http://example.com/?search=][ref]</a></p>", 
                "commonmark");
        }

        // Matching is case-insensitive:
        [Fact]
        public void Links_Spec510_commonmark()
        {
            // The following Markdown:
            //     [foo][BaR]
            //     
            //     [bar]: /url "title"
            //
            // Should be rendered as:
            //     <p><a href="/url" title="title">foo</a></p>

            SpecTestHelper.AssertCompliance("[foo][BaR]\n\n[bar]: /url \"title\"", 
                "<p><a href=\"/url\" title=\"title\">foo</a></p>", 
                "commonmark");
        }

        // Unicode case fold is used:
        [Fact]
        public void Links_Spec511_commonmark()
        {
            // The following Markdown:
            //     [Толпой][Толпой] is a Russian word.
            //     
            //     [ТОЛПОЙ]: /url
            //
            // Should be rendered as:
            //     <p><a href="/url">Толпой</a> is a Russian word.</p>

            SpecTestHelper.AssertCompliance("[Толпой][Толпой] is a Russian word.\n\n[ТОЛПОЙ]: /url", 
                "<p><a href=\"/url\">Толпой</a> is a Russian word.</p>", 
                "commonmark");
        }

        // Consecutive internal [whitespace] is treated as one space for
        // purposes of determining matching:
        [Fact]
        public void Links_Spec512_commonmark()
        {
            // The following Markdown:
            //     [Foo
            //       bar]: /url
            //     
            //     [Baz][Foo bar]
            //
            // Should be rendered as:
            //     <p><a href="/url">Baz</a></p>

            SpecTestHelper.AssertCompliance("[Foo\n  bar]: /url\n\n[Baz][Foo bar]", 
                "<p><a href=\"/url\">Baz</a></p>", 
                "commonmark");
        }

        // No [whitespace] is allowed between the [link text] and the
        // [link label]:
        [Fact]
        public void Links_Spec513_commonmark()
        {
            // The following Markdown:
            //     [foo] [bar]
            //     
            //     [bar]: /url "title"
            //
            // Should be rendered as:
            //     <p>[foo] <a href="/url" title="title">bar</a></p>

            SpecTestHelper.AssertCompliance("[foo] [bar]\n\n[bar]: /url \"title\"", 
                "<p>[foo] <a href=\"/url\" title=\"title\">bar</a></p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec514_commonmark()
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

            SpecTestHelper.AssertCompliance("[foo]\n[bar]\n\n[bar]: /url \"title\"", 
                "<p>[foo]\n<a href=\"/url\" title=\"title\">bar</a></p>", 
                "commonmark");
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
        public void Links_Spec515_commonmark()
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

            SpecTestHelper.AssertCompliance("[foo]: /url1\n\n[foo]: /url2\n\n[bar][foo]", 
                "<p><a href=\"/url1\">bar</a></p>", 
                "commonmark");
        }

        // Note that matching is performed on normalized strings, not parsed
        // inline content.  So the following does not match, even though the
        // labels define equivalent inline content:
        [Fact]
        public void Links_Spec516_commonmark()
        {
            // The following Markdown:
            //     [bar][foo\!]
            //     
            //     [foo!]: /url
            //
            // Should be rendered as:
            //     <p>[bar][foo!]</p>

            SpecTestHelper.AssertCompliance("[bar][foo\\!]\n\n[foo!]: /url", 
                "<p>[bar][foo!]</p>", 
                "commonmark");
        }

        // [Link labels] cannot contain brackets, unless they are
        // backslash-escaped:
        [Fact]
        public void Links_Spec517_commonmark()
        {
            // The following Markdown:
            //     [foo][ref[]
            //     
            //     [ref[]: /uri
            //
            // Should be rendered as:
            //     <p>[foo][ref[]</p>
            //     <p>[ref[]: /uri</p>

            SpecTestHelper.AssertCompliance("[foo][ref[]\n\n[ref[]: /uri", 
                "<p>[foo][ref[]</p>\n<p>[ref[]: /uri</p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec518_commonmark()
        {
            // The following Markdown:
            //     [foo][ref[bar]]
            //     
            //     [ref[bar]]: /uri
            //
            // Should be rendered as:
            //     <p>[foo][ref[bar]]</p>
            //     <p>[ref[bar]]: /uri</p>

            SpecTestHelper.AssertCompliance("[foo][ref[bar]]\n\n[ref[bar]]: /uri", 
                "<p>[foo][ref[bar]]</p>\n<p>[ref[bar]]: /uri</p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec519_commonmark()
        {
            // The following Markdown:
            //     [[[foo]]]
            //     
            //     [[[foo]]]: /url
            //
            // Should be rendered as:
            //     <p>[[[foo]]]</p>
            //     <p>[[[foo]]]: /url</p>

            SpecTestHelper.AssertCompliance("[[[foo]]]\n\n[[[foo]]]: /url", 
                "<p>[[[foo]]]</p>\n<p>[[[foo]]]: /url</p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec520_commonmark()
        {
            // The following Markdown:
            //     [foo][ref\[]
            //     
            //     [ref\[]: /uri
            //
            // Should be rendered as:
            //     <p><a href="/uri">foo</a></p>

            SpecTestHelper.AssertCompliance("[foo][ref\\[]\n\n[ref\\[]: /uri", 
                "<p><a href=\"/uri\">foo</a></p>", 
                "commonmark");
        }

        // Note that in this example `]` is not backslash-escaped:
        [Fact]
        public void Links_Spec521_commonmark()
        {
            // The following Markdown:
            //     [bar\\]: /uri
            //     
            //     [bar\\]
            //
            // Should be rendered as:
            //     <p><a href="/uri">bar\</a></p>

            SpecTestHelper.AssertCompliance("[bar\\\\]: /uri\n\n[bar\\\\]", 
                "<p><a href=\"/uri\">bar\\</a></p>", 
                "commonmark");
        }

        // A [link label] must contain at least one [non-whitespace character]:
        [Fact]
        public void Links_Spec522_commonmark()
        {
            // The following Markdown:
            //     []
            //     
            //     []: /uri
            //
            // Should be rendered as:
            //     <p>[]</p>
            //     <p>[]: /uri</p>

            SpecTestHelper.AssertCompliance("[]\n\n[]: /uri", 
                "<p>[]</p>\n<p>[]: /uri</p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec523_commonmark()
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

            SpecTestHelper.AssertCompliance("[\n ]\n\n[\n ]: /uri", 
                "<p>[\n]</p>\n<p>[\n]: /uri</p>", 
                "commonmark");
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
        public void Links_Spec524_commonmark()
        {
            // The following Markdown:
            //     [foo][]
            //     
            //     [foo]: /url "title"
            //
            // Should be rendered as:
            //     <p><a href="/url" title="title">foo</a></p>

            SpecTestHelper.AssertCompliance("[foo][]\n\n[foo]: /url \"title\"", 
                "<p><a href=\"/url\" title=\"title\">foo</a></p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec525_commonmark()
        {
            // The following Markdown:
            //     [*foo* bar][]
            //     
            //     [*foo* bar]: /url "title"
            //
            // Should be rendered as:
            //     <p><a href="/url" title="title"><em>foo</em> bar</a></p>

            SpecTestHelper.AssertCompliance("[*foo* bar][]\n\n[*foo* bar]: /url \"title\"", 
                "<p><a href=\"/url\" title=\"title\"><em>foo</em> bar</a></p>", 
                "commonmark");
        }

        // The link labels are case-insensitive:
        [Fact]
        public void Links_Spec526_commonmark()
        {
            // The following Markdown:
            //     [Foo][]
            //     
            //     [foo]: /url "title"
            //
            // Should be rendered as:
            //     <p><a href="/url" title="title">Foo</a></p>

            SpecTestHelper.AssertCompliance("[Foo][]\n\n[foo]: /url \"title\"", 
                "<p><a href=\"/url\" title=\"title\">Foo</a></p>", 
                "commonmark");
        }

        // As with full reference links, [whitespace] is not
        // allowed between the two sets of brackets:
        [Fact]
        public void Links_Spec527_commonmark()
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

            SpecTestHelper.AssertCompliance("[foo] \n[]\n\n[foo]: /url \"title\"", 
                "<p><a href=\"/url\" title=\"title\">foo</a>\n[]</p>", 
                "commonmark");
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
        public void Links_Spec528_commonmark()
        {
            // The following Markdown:
            //     [foo]
            //     
            //     [foo]: /url "title"
            //
            // Should be rendered as:
            //     <p><a href="/url" title="title">foo</a></p>

            SpecTestHelper.AssertCompliance("[foo]\n\n[foo]: /url \"title\"", 
                "<p><a href=\"/url\" title=\"title\">foo</a></p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec529_commonmark()
        {
            // The following Markdown:
            //     [*foo* bar]
            //     
            //     [*foo* bar]: /url "title"
            //
            // Should be rendered as:
            //     <p><a href="/url" title="title"><em>foo</em> bar</a></p>

            SpecTestHelper.AssertCompliance("[*foo* bar]\n\n[*foo* bar]: /url \"title\"", 
                "<p><a href=\"/url\" title=\"title\"><em>foo</em> bar</a></p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec530_commonmark()
        {
            // The following Markdown:
            //     [[*foo* bar]]
            //     
            //     [*foo* bar]: /url "title"
            //
            // Should be rendered as:
            //     <p>[<a href="/url" title="title"><em>foo</em> bar</a>]</p>

            SpecTestHelper.AssertCompliance("[[*foo* bar]]\n\n[*foo* bar]: /url \"title\"", 
                "<p>[<a href=\"/url\" title=\"title\"><em>foo</em> bar</a>]</p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec531_commonmark()
        {
            // The following Markdown:
            //     [[bar [foo]
            //     
            //     [foo]: /url
            //
            // Should be rendered as:
            //     <p>[[bar <a href="/url">foo</a></p>

            SpecTestHelper.AssertCompliance("[[bar [foo]\n\n[foo]: /url", 
                "<p>[[bar <a href=\"/url\">foo</a></p>", 
                "commonmark");
        }

        // The link labels are case-insensitive:
        [Fact]
        public void Links_Spec532_commonmark()
        {
            // The following Markdown:
            //     [Foo]
            //     
            //     [foo]: /url "title"
            //
            // Should be rendered as:
            //     <p><a href="/url" title="title">Foo</a></p>

            SpecTestHelper.AssertCompliance("[Foo]\n\n[foo]: /url \"title\"", 
                "<p><a href=\"/url\" title=\"title\">Foo</a></p>", 
                "commonmark");
        }

        // A space after the link text should be preserved:
        [Fact]
        public void Links_Spec533_commonmark()
        {
            // The following Markdown:
            //     [foo] bar
            //     
            //     [foo]: /url
            //
            // Should be rendered as:
            //     <p><a href="/url">foo</a> bar</p>

            SpecTestHelper.AssertCompliance("[foo] bar\n\n[foo]: /url", 
                "<p><a href=\"/url\">foo</a> bar</p>", 
                "commonmark");
        }

        // If you just want bracketed text, you can backslash-escape the
        // opening bracket to avoid links:
        [Fact]
        public void Links_Spec534_commonmark()
        {
            // The following Markdown:
            //     \[foo]
            //     
            //     [foo]: /url "title"
            //
            // Should be rendered as:
            //     <p>[foo]</p>

            SpecTestHelper.AssertCompliance("\\[foo]\n\n[foo]: /url \"title\"", 
                "<p>[foo]</p>", 
                "commonmark");
        }

        // Note that this is a link, because a link label ends with the first
        // following closing bracket:
        [Fact]
        public void Links_Spec535_commonmark()
        {
            // The following Markdown:
            //     [foo*]: /url
            //     
            //     *[foo*]
            //
            // Should be rendered as:
            //     <p>*<a href="/url">foo*</a></p>

            SpecTestHelper.AssertCompliance("[foo*]: /url\n\n*[foo*]", 
                "<p>*<a href=\"/url\">foo*</a></p>", 
                "commonmark");
        }

        // Full and compact references take precedence over shortcut
        // references:
        [Fact]
        public void Links_Spec536_commonmark()
        {
            // The following Markdown:
            //     [foo][bar]
            //     
            //     [foo]: /url1
            //     [bar]: /url2
            //
            // Should be rendered as:
            //     <p><a href="/url2">foo</a></p>

            SpecTestHelper.AssertCompliance("[foo][bar]\n\n[foo]: /url1\n[bar]: /url2", 
                "<p><a href=\"/url2\">foo</a></p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec537_commonmark()
        {
            // The following Markdown:
            //     [foo][]
            //     
            //     [foo]: /url1
            //
            // Should be rendered as:
            //     <p><a href="/url1">foo</a></p>

            SpecTestHelper.AssertCompliance("[foo][]\n\n[foo]: /url1", 
                "<p><a href=\"/url1\">foo</a></p>", 
                "commonmark");
        }

        // Inline links also take precedence:
        [Fact]
        public void Links_Spec538_commonmark()
        {
            // The following Markdown:
            //     [foo]()
            //     
            //     [foo]: /url1
            //
            // Should be rendered as:
            //     <p><a href="">foo</a></p>

            SpecTestHelper.AssertCompliance("[foo]()\n\n[foo]: /url1", 
                "<p><a href=\"\">foo</a></p>", 
                "commonmark");
        }

        [Fact]
        public void Links_Spec539_commonmark()
        {
            // The following Markdown:
            //     [foo](not a link)
            //     
            //     [foo]: /url1
            //
            // Should be rendered as:
            //     <p><a href="/url1">foo</a>(not a link)</p>

            SpecTestHelper.AssertCompliance("[foo](not a link)\n\n[foo]: /url1", 
                "<p><a href=\"/url1\">foo</a>(not a link)</p>", 
                "commonmark");
        }

        // In the following case `[bar][baz]` is parsed as a reference,
        // `[foo]` as normal text:
        [Fact]
        public void Links_Spec540_commonmark()
        {
            // The following Markdown:
            //     [foo][bar][baz]
            //     
            //     [baz]: /url
            //
            // Should be rendered as:
            //     <p>[foo]<a href="/url">bar</a></p>

            SpecTestHelper.AssertCompliance("[foo][bar][baz]\n\n[baz]: /url", 
                "<p>[foo]<a href=\"/url\">bar</a></p>", 
                "commonmark");
        }

        // Here, though, `[foo][bar]` is parsed as a reference, since
        // `[bar]` is defined:
        [Fact]
        public void Links_Spec541_commonmark()
        {
            // The following Markdown:
            //     [foo][bar][baz]
            //     
            //     [baz]: /url1
            //     [bar]: /url2
            //
            // Should be rendered as:
            //     <p><a href="/url2">foo</a><a href="/url1">baz</a></p>

            SpecTestHelper.AssertCompliance("[foo][bar][baz]\n\n[baz]: /url1\n[bar]: /url2", 
                "<p><a href=\"/url2\">foo</a><a href=\"/url1\">baz</a></p>", 
                "commonmark");
        }

        // Here `[foo]` is not parsed as a shortcut reference, because it
        // is followed by a link label (even though `[bar]` is not defined):
        [Fact]
        public void Links_Spec542_commonmark()
        {
            // The following Markdown:
            //     [foo][bar][baz]
            //     
            //     [baz]: /url1
            //     [foo]: /url2
            //
            // Should be rendered as:
            //     <p>[foo]<a href="/url1">bar</a></p>

            SpecTestHelper.AssertCompliance("[foo][bar][baz]\n\n[baz]: /url1\n[foo]: /url2", 
                "<p>[foo]<a href=\"/url1\">bar</a></p>", 
                "commonmark");
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
        public void Images_Spec543_commonmark()
        {
            // The following Markdown:
            //     ![foo](/url "title")
            //
            // Should be rendered as:
            //     <p><img src="/url" alt="foo" title="title" /></p>

            SpecTestHelper.AssertCompliance("![foo](/url \"title\")", 
                "<p><img src=\"/url\" alt=\"foo\" title=\"title\" /></p>", 
                "commonmark");
        }

        [Fact]
        public void Images_Spec544_commonmark()
        {
            // The following Markdown:
            //     ![foo *bar*]
            //     
            //     [foo *bar*]: train.jpg "train & tracks"
            //
            // Should be rendered as:
            //     <p><img src="train.jpg" alt="foo bar" title="train &amp; tracks" /></p>

            SpecTestHelper.AssertCompliance("![foo *bar*]\n\n[foo *bar*]: train.jpg \"train & tracks\"", 
                "<p><img src=\"train.jpg\" alt=\"foo bar\" title=\"train &amp; tracks\" /></p>", 
                "commonmark");
        }

        [Fact]
        public void Images_Spec545_commonmark()
        {
            // The following Markdown:
            //     ![foo ![bar](/url)](/url2)
            //
            // Should be rendered as:
            //     <p><img src="/url2" alt="foo bar" /></p>

            SpecTestHelper.AssertCompliance("![foo ![bar](/url)](/url2)", 
                "<p><img src=\"/url2\" alt=\"foo bar\" /></p>", 
                "commonmark");
        }

        [Fact]
        public void Images_Spec546_commonmark()
        {
            // The following Markdown:
            //     ![foo [bar](/url)](/url2)
            //
            // Should be rendered as:
            //     <p><img src="/url2" alt="foo bar" /></p>

            SpecTestHelper.AssertCompliance("![foo [bar](/url)](/url2)", 
                "<p><img src=\"/url2\" alt=\"foo bar\" /></p>", 
                "commonmark");
        }

        // Though this spec is concerned with parsing, not rendering, it is
        // recommended that in rendering to HTML, only the plain string content
        // of the [image description] be used.  Note that in
        // the above example, the alt attribute's value is `foo bar`, not `foo
        // [bar](/url)` or `foo <a href="/url">bar</a>`.  Only the plain string
        // content is rendered, without formatting.
        [Fact]
        public void Images_Spec547_commonmark()
        {
            // The following Markdown:
            //     ![foo *bar*][]
            //     
            //     [foo *bar*]: train.jpg "train & tracks"
            //
            // Should be rendered as:
            //     <p><img src="train.jpg" alt="foo bar" title="train &amp; tracks" /></p>

            SpecTestHelper.AssertCompliance("![foo *bar*][]\n\n[foo *bar*]: train.jpg \"train & tracks\"", 
                "<p><img src=\"train.jpg\" alt=\"foo bar\" title=\"train &amp; tracks\" /></p>", 
                "commonmark");
        }

        [Fact]
        public void Images_Spec548_commonmark()
        {
            // The following Markdown:
            //     ![foo *bar*][foobar]
            //     
            //     [FOOBAR]: train.jpg "train & tracks"
            //
            // Should be rendered as:
            //     <p><img src="train.jpg" alt="foo bar" title="train &amp; tracks" /></p>

            SpecTestHelper.AssertCompliance("![foo *bar*][foobar]\n\n[FOOBAR]: train.jpg \"train & tracks\"", 
                "<p><img src=\"train.jpg\" alt=\"foo bar\" title=\"train &amp; tracks\" /></p>", 
                "commonmark");
        }

        [Fact]
        public void Images_Spec549_commonmark()
        {
            // The following Markdown:
            //     ![foo](train.jpg)
            //
            // Should be rendered as:
            //     <p><img src="train.jpg" alt="foo" /></p>

            SpecTestHelper.AssertCompliance("![foo](train.jpg)", 
                "<p><img src=\"train.jpg\" alt=\"foo\" /></p>", 
                "commonmark");
        }

        [Fact]
        public void Images_Spec550_commonmark()
        {
            // The following Markdown:
            //     My ![foo bar](/path/to/train.jpg  "title"   )
            //
            // Should be rendered as:
            //     <p>My <img src="/path/to/train.jpg" alt="foo bar" title="title" /></p>

            SpecTestHelper.AssertCompliance("My ![foo bar](/path/to/train.jpg  \"title\"   )", 
                "<p>My <img src=\"/path/to/train.jpg\" alt=\"foo bar\" title=\"title\" /></p>", 
                "commonmark");
        }

        [Fact]
        public void Images_Spec551_commonmark()
        {
            // The following Markdown:
            //     ![foo](<url>)
            //
            // Should be rendered as:
            //     <p><img src="url" alt="foo" /></p>

            SpecTestHelper.AssertCompliance("![foo](<url>)", 
                "<p><img src=\"url\" alt=\"foo\" /></p>", 
                "commonmark");
        }

        [Fact]
        public void Images_Spec552_commonmark()
        {
            // The following Markdown:
            //     ![](/url)
            //
            // Should be rendered as:
            //     <p><img src="/url" alt="" /></p>

            SpecTestHelper.AssertCompliance("![](/url)", 
                "<p><img src=\"/url\" alt=\"\" /></p>", 
                "commonmark");
        }

        // Reference-style:
        [Fact]
        public void Images_Spec553_commonmark()
        {
            // The following Markdown:
            //     ![foo][bar]
            //     
            //     [bar]: /url
            //
            // Should be rendered as:
            //     <p><img src="/url" alt="foo" /></p>

            SpecTestHelper.AssertCompliance("![foo][bar]\n\n[bar]: /url", 
                "<p><img src=\"/url\" alt=\"foo\" /></p>", 
                "commonmark");
        }

        [Fact]
        public void Images_Spec554_commonmark()
        {
            // The following Markdown:
            //     ![foo][bar]
            //     
            //     [BAR]: /url
            //
            // Should be rendered as:
            //     <p><img src="/url" alt="foo" /></p>

            SpecTestHelper.AssertCompliance("![foo][bar]\n\n[BAR]: /url", 
                "<p><img src=\"/url\" alt=\"foo\" /></p>", 
                "commonmark");
        }

        // Collapsed:
        [Fact]
        public void Images_Spec555_commonmark()
        {
            // The following Markdown:
            //     ![foo][]
            //     
            //     [foo]: /url "title"
            //
            // Should be rendered as:
            //     <p><img src="/url" alt="foo" title="title" /></p>

            SpecTestHelper.AssertCompliance("![foo][]\n\n[foo]: /url \"title\"", 
                "<p><img src=\"/url\" alt=\"foo\" title=\"title\" /></p>", 
                "commonmark");
        }

        [Fact]
        public void Images_Spec556_commonmark()
        {
            // The following Markdown:
            //     ![*foo* bar][]
            //     
            //     [*foo* bar]: /url "title"
            //
            // Should be rendered as:
            //     <p><img src="/url" alt="foo bar" title="title" /></p>

            SpecTestHelper.AssertCompliance("![*foo* bar][]\n\n[*foo* bar]: /url \"title\"", 
                "<p><img src=\"/url\" alt=\"foo bar\" title=\"title\" /></p>", 
                "commonmark");
        }

        // The labels are case-insensitive:
        [Fact]
        public void Images_Spec557_commonmark()
        {
            // The following Markdown:
            //     ![Foo][]
            //     
            //     [foo]: /url "title"
            //
            // Should be rendered as:
            //     <p><img src="/url" alt="Foo" title="title" /></p>

            SpecTestHelper.AssertCompliance("![Foo][]\n\n[foo]: /url \"title\"", 
                "<p><img src=\"/url\" alt=\"Foo\" title=\"title\" /></p>", 
                "commonmark");
        }

        // As with reference links, [whitespace] is not allowed
        // between the two sets of brackets:
        [Fact]
        public void Images_Spec558_commonmark()
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

            SpecTestHelper.AssertCompliance("![foo] \n[]\n\n[foo]: /url \"title\"", 
                "<p><img src=\"/url\" alt=\"foo\" title=\"title\" />\n[]</p>", 
                "commonmark");
        }

        // Shortcut:
        [Fact]
        public void Images_Spec559_commonmark()
        {
            // The following Markdown:
            //     ![foo]
            //     
            //     [foo]: /url "title"
            //
            // Should be rendered as:
            //     <p><img src="/url" alt="foo" title="title" /></p>

            SpecTestHelper.AssertCompliance("![foo]\n\n[foo]: /url \"title\"", 
                "<p><img src=\"/url\" alt=\"foo\" title=\"title\" /></p>", 
                "commonmark");
        }

        [Fact]
        public void Images_Spec560_commonmark()
        {
            // The following Markdown:
            //     ![*foo* bar]
            //     
            //     [*foo* bar]: /url "title"
            //
            // Should be rendered as:
            //     <p><img src="/url" alt="foo bar" title="title" /></p>

            SpecTestHelper.AssertCompliance("![*foo* bar]\n\n[*foo* bar]: /url \"title\"", 
                "<p><img src=\"/url\" alt=\"foo bar\" title=\"title\" /></p>", 
                "commonmark");
        }

        // Note that link labels cannot contain unescaped brackets:
        [Fact]
        public void Images_Spec561_commonmark()
        {
            // The following Markdown:
            //     ![[foo]]
            //     
            //     [[foo]]: /url "title"
            //
            // Should be rendered as:
            //     <p>![[foo]]</p>
            //     <p>[[foo]]: /url &quot;title&quot;</p>

            SpecTestHelper.AssertCompliance("![[foo]]\n\n[[foo]]: /url \"title\"", 
                "<p>![[foo]]</p>\n<p>[[foo]]: /url &quot;title&quot;</p>", 
                "commonmark");
        }

        // The link labels are case-insensitive:
        [Fact]
        public void Images_Spec562_commonmark()
        {
            // The following Markdown:
            //     ![Foo]
            //     
            //     [foo]: /url "title"
            //
            // Should be rendered as:
            //     <p><img src="/url" alt="Foo" title="title" /></p>

            SpecTestHelper.AssertCompliance("![Foo]\n\n[foo]: /url \"title\"", 
                "<p><img src=\"/url\" alt=\"Foo\" title=\"title\" /></p>", 
                "commonmark");
        }

        // If you just want a literal `!` followed by bracketed text, you can
        // backslash-escape the opening `[`:
        [Fact]
        public void Images_Spec563_commonmark()
        {
            // The following Markdown:
            //     !\[foo]
            //     
            //     [foo]: /url "title"
            //
            // Should be rendered as:
            //     <p>![foo]</p>

            SpecTestHelper.AssertCompliance("!\\[foo]\n\n[foo]: /url \"title\"", 
                "<p>![foo]</p>", 
                "commonmark");
        }

        // If you want a link after a literal `!`, backslash-escape the
        // `!`:
        [Fact]
        public void Images_Spec564_commonmark()
        {
            // The following Markdown:
            //     \![foo]
            //     
            //     [foo]: /url "title"
            //
            // Should be rendered as:
            //     <p>!<a href="/url" title="title">foo</a></p>

            SpecTestHelper.AssertCompliance("\\![foo]\n\n[foo]: /url \"title\"", 
                "<p>!<a href=\"/url\" title=\"title\">foo</a></p>", 
                "commonmark");
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
        public void Autolinks_Spec565_commonmark()
        {
            // The following Markdown:
            //     <http://foo.bar.baz>
            //
            // Should be rendered as:
            //     <p><a href="http://foo.bar.baz">http://foo.bar.baz</a></p>

            SpecTestHelper.AssertCompliance("<http://foo.bar.baz>", 
                "<p><a href=\"http://foo.bar.baz\">http://foo.bar.baz</a></p>", 
                "commonmark");
        }

        [Fact]
        public void Autolinks_Spec566_commonmark()
        {
            // The following Markdown:
            //     <http://foo.bar.baz/test?q=hello&id=22&boolean>
            //
            // Should be rendered as:
            //     <p><a href="http://foo.bar.baz/test?q=hello&amp;id=22&amp;boolean">http://foo.bar.baz/test?q=hello&amp;id=22&amp;boolean</a></p>

            SpecTestHelper.AssertCompliance("<http://foo.bar.baz/test?q=hello&id=22&boolean>", 
                "<p><a href=\"http://foo.bar.baz/test?q=hello&amp;id=22&amp;boolean\">http://foo.bar.baz/test?q=hello&amp;id=22&amp;boolean</a></p>", 
                "commonmark");
        }

        [Fact]
        public void Autolinks_Spec567_commonmark()
        {
            // The following Markdown:
            //     <irc://foo.bar:2233/baz>
            //
            // Should be rendered as:
            //     <p><a href="irc://foo.bar:2233/baz">irc://foo.bar:2233/baz</a></p>

            SpecTestHelper.AssertCompliance("<irc://foo.bar:2233/baz>", 
                "<p><a href=\"irc://foo.bar:2233/baz\">irc://foo.bar:2233/baz</a></p>", 
                "commonmark");
        }

        // Uppercase is also fine:
        [Fact]
        public void Autolinks_Spec568_commonmark()
        {
            // The following Markdown:
            //     <MAILTO:FOO@BAR.BAZ>
            //
            // Should be rendered as:
            //     <p><a href="MAILTO:FOO@BAR.BAZ">MAILTO:FOO@BAR.BAZ</a></p>

            SpecTestHelper.AssertCompliance("<MAILTO:FOO@BAR.BAZ>", 
                "<p><a href=\"MAILTO:FOO@BAR.BAZ\">MAILTO:FOO@BAR.BAZ</a></p>", 
                "commonmark");
        }

        // Note that many strings that count as [absolute URIs] for
        // purposes of this spec are not valid URIs, because their
        // schemes are not registered or because of other problems
        // with their syntax:
        [Fact]
        public void Autolinks_Spec569_commonmark()
        {
            // The following Markdown:
            //     <a+b+c:d>
            //
            // Should be rendered as:
            //     <p><a href="a+b+c:d">a+b+c:d</a></p>

            SpecTestHelper.AssertCompliance("<a+b+c:d>", 
                "<p><a href=\"a+b+c:d\">a+b+c:d</a></p>", 
                "commonmark");
        }

        [Fact]
        public void Autolinks_Spec570_commonmark()
        {
            // The following Markdown:
            //     <made-up-scheme://foo,bar>
            //
            // Should be rendered as:
            //     <p><a href="made-up-scheme://foo,bar">made-up-scheme://foo,bar</a></p>

            SpecTestHelper.AssertCompliance("<made-up-scheme://foo,bar>", 
                "<p><a href=\"made-up-scheme://foo,bar\">made-up-scheme://foo,bar</a></p>", 
                "commonmark");
        }

        [Fact]
        public void Autolinks_Spec571_commonmark()
        {
            // The following Markdown:
            //     <http://../>
            //
            // Should be rendered as:
            //     <p><a href="http://../">http://../</a></p>

            SpecTestHelper.AssertCompliance("<http://../>", 
                "<p><a href=\"http://../\">http://../</a></p>", 
                "commonmark");
        }

        [Fact]
        public void Autolinks_Spec572_commonmark()
        {
            // The following Markdown:
            //     <localhost:5001/foo>
            //
            // Should be rendered as:
            //     <p><a href="localhost:5001/foo">localhost:5001/foo</a></p>

            SpecTestHelper.AssertCompliance("<localhost:5001/foo>", 
                "<p><a href=\"localhost:5001/foo\">localhost:5001/foo</a></p>", 
                "commonmark");
        }

        // Spaces are not allowed in autolinks:
        [Fact]
        public void Autolinks_Spec573_commonmark()
        {
            // The following Markdown:
            //     <http://foo.bar/baz bim>
            //
            // Should be rendered as:
            //     <p>&lt;http://foo.bar/baz bim&gt;</p>

            SpecTestHelper.AssertCompliance("<http://foo.bar/baz bim>", 
                "<p>&lt;http://foo.bar/baz bim&gt;</p>", 
                "commonmark");
        }

        // Backslash-escapes do not work inside autolinks:
        [Fact]
        public void Autolinks_Spec574_commonmark()
        {
            // The following Markdown:
            //     <http://example.com/\[\>
            //
            // Should be rendered as:
            //     <p><a href="http://example.com/%5C%5B%5C">http://example.com/\[\</a></p>

            SpecTestHelper.AssertCompliance("<http://example.com/\\[\\>", 
                "<p><a href=\"http://example.com/%5C%5B%5C\">http://example.com/\\[\\</a></p>", 
                "commonmark");
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
        public void Autolinks_Spec575_commonmark()
        {
            // The following Markdown:
            //     <foo@bar.example.com>
            //
            // Should be rendered as:
            //     <p><a href="mailto:foo@bar.example.com">foo@bar.example.com</a></p>

            SpecTestHelper.AssertCompliance("<foo@bar.example.com>", 
                "<p><a href=\"mailto:foo@bar.example.com\">foo@bar.example.com</a></p>", 
                "commonmark");
        }

        [Fact]
        public void Autolinks_Spec576_commonmark()
        {
            // The following Markdown:
            //     <foo+special@Bar.baz-bar0.com>
            //
            // Should be rendered as:
            //     <p><a href="mailto:foo+special@Bar.baz-bar0.com">foo+special@Bar.baz-bar0.com</a></p>

            SpecTestHelper.AssertCompliance("<foo+special@Bar.baz-bar0.com>", 
                "<p><a href=\"mailto:foo+special@Bar.baz-bar0.com\">foo+special@Bar.baz-bar0.com</a></p>", 
                "commonmark");
        }

        // Backslash-escapes do not work inside email autolinks:
        [Fact]
        public void Autolinks_Spec577_commonmark()
        {
            // The following Markdown:
            //     <foo\+@bar.example.com>
            //
            // Should be rendered as:
            //     <p>&lt;foo+@bar.example.com&gt;</p>

            SpecTestHelper.AssertCompliance("<foo\\+@bar.example.com>", 
                "<p>&lt;foo+@bar.example.com&gt;</p>", 
                "commonmark");
        }

        // These are not autolinks:
        [Fact]
        public void Autolinks_Spec578_commonmark()
        {
            // The following Markdown:
            //     <>
            //
            // Should be rendered as:
            //     <p>&lt;&gt;</p>

            SpecTestHelper.AssertCompliance("<>", 
                "<p>&lt;&gt;</p>", 
                "commonmark");
        }

        [Fact]
        public void Autolinks_Spec579_commonmark()
        {
            // The following Markdown:
            //     < http://foo.bar >
            //
            // Should be rendered as:
            //     <p>&lt; http://foo.bar &gt;</p>

            SpecTestHelper.AssertCompliance("< http://foo.bar >", 
                "<p>&lt; http://foo.bar &gt;</p>", 
                "commonmark");
        }

        [Fact]
        public void Autolinks_Spec580_commonmark()
        {
            // The following Markdown:
            //     <m:abc>
            //
            // Should be rendered as:
            //     <p>&lt;m:abc&gt;</p>

            SpecTestHelper.AssertCompliance("<m:abc>", 
                "<p>&lt;m:abc&gt;</p>", 
                "commonmark");
        }

        [Fact]
        public void Autolinks_Spec581_commonmark()
        {
            // The following Markdown:
            //     <foo.bar.baz>
            //
            // Should be rendered as:
            //     <p>&lt;foo.bar.baz&gt;</p>

            SpecTestHelper.AssertCompliance("<foo.bar.baz>", 
                "<p>&lt;foo.bar.baz&gt;</p>", 
                "commonmark");
        }

        [Fact]
        public void Autolinks_Spec582_commonmark()
        {
            // The following Markdown:
            //     http://example.com
            //
            // Should be rendered as:
            //     <p>http://example.com</p>

            SpecTestHelper.AssertCompliance("http://example.com", 
                "<p>http://example.com</p>", 
                "commonmark");
        }

        [Fact]
        public void Autolinks_Spec583_commonmark()
        {
            // The following Markdown:
            //     foo@bar.example.com
            //
            // Should be rendered as:
            //     <p>foo@bar.example.com</p>

            SpecTestHelper.AssertCompliance("foo@bar.example.com", 
                "<p>foo@bar.example.com</p>", 
                "commonmark");
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
        public void RawHTML_Spec584_commonmark()
        {
            // The following Markdown:
            //     <a><bab><c2c>
            //
            // Should be rendered as:
            //     <p><a><bab><c2c></p>

            SpecTestHelper.AssertCompliance("<a><bab><c2c>", 
                "<p><a><bab><c2c></p>", 
                "commonmark");
        }

        // Empty elements:
        [Fact]
        public void RawHTML_Spec585_commonmark()
        {
            // The following Markdown:
            //     <a/><b2/>
            //
            // Should be rendered as:
            //     <p><a/><b2/></p>

            SpecTestHelper.AssertCompliance("<a/><b2/>", 
                "<p><a/><b2/></p>", 
                "commonmark");
        }

        // [Whitespace] is allowed:
        [Fact]
        public void RawHTML_Spec586_commonmark()
        {
            // The following Markdown:
            //     <a  /><b2
            //     data="foo" >
            //
            // Should be rendered as:
            //     <p><a  /><b2
            //     data="foo" ></p>

            SpecTestHelper.AssertCompliance("<a  /><b2\ndata=\"foo\" >", 
                "<p><a  /><b2\ndata=\"foo\" ></p>", 
                "commonmark");
        }

        // With attributes:
        [Fact]
        public void RawHTML_Spec587_commonmark()
        {
            // The following Markdown:
            //     <a foo="bar" bam = 'baz <em>"</em>'
            //     _boolean zoop:33=zoop:33 />
            //
            // Should be rendered as:
            //     <p><a foo="bar" bam = 'baz <em>"</em>'
            //     _boolean zoop:33=zoop:33 /></p>

            SpecTestHelper.AssertCompliance("<a foo=\"bar\" bam = 'baz <em>\"</em>'\n_boolean zoop:33=zoop:33 />", 
                "<p><a foo=\"bar\" bam = 'baz <em>\"</em>'\n_boolean zoop:33=zoop:33 /></p>", 
                "commonmark");
        }

        // Custom tag names can be used:
        [Fact]
        public void RawHTML_Spec588_commonmark()
        {
            // The following Markdown:
            //     Foo <responsive-image src="foo.jpg" />
            //
            // Should be rendered as:
            //     <p>Foo <responsive-image src="foo.jpg" /></p>

            SpecTestHelper.AssertCompliance("Foo <responsive-image src=\"foo.jpg\" />", 
                "<p>Foo <responsive-image src=\"foo.jpg\" /></p>", 
                "commonmark");
        }

        // Illegal tag names, not parsed as HTML:
        [Fact]
        public void RawHTML_Spec589_commonmark()
        {
            // The following Markdown:
            //     <33> <__>
            //
            // Should be rendered as:
            //     <p>&lt;33&gt; &lt;__&gt;</p>

            SpecTestHelper.AssertCompliance("<33> <__>", 
                "<p>&lt;33&gt; &lt;__&gt;</p>", 
                "commonmark");
        }

        // Illegal attribute names:
        [Fact]
        public void RawHTML_Spec590_commonmark()
        {
            // The following Markdown:
            //     <a h*#ref="hi">
            //
            // Should be rendered as:
            //     <p>&lt;a h*#ref=&quot;hi&quot;&gt;</p>

            SpecTestHelper.AssertCompliance("<a h*#ref=\"hi\">", 
                "<p>&lt;a h*#ref=&quot;hi&quot;&gt;</p>", 
                "commonmark");
        }

        // Illegal attribute values:
        [Fact]
        public void RawHTML_Spec591_commonmark()
        {
            // The following Markdown:
            //     <a href="hi'> <a href=hi'>
            //
            // Should be rendered as:
            //     <p>&lt;a href=&quot;hi'&gt; &lt;a href=hi'&gt;</p>

            SpecTestHelper.AssertCompliance("<a href=\"hi'> <a href=hi'>", 
                "<p>&lt;a href=&quot;hi'&gt; &lt;a href=hi'&gt;</p>", 
                "commonmark");
        }

        // Illegal [whitespace]:
        [Fact]
        public void RawHTML_Spec592_commonmark()
        {
            // The following Markdown:
            //     < a><
            //     foo><bar/ >
            //
            // Should be rendered as:
            //     <p>&lt; a&gt;&lt;
            //     foo&gt;&lt;bar/ &gt;</p>

            SpecTestHelper.AssertCompliance("< a><\nfoo><bar/ >", 
                "<p>&lt; a&gt;&lt;\nfoo&gt;&lt;bar/ &gt;</p>", 
                "commonmark");
        }

        // Missing [whitespace]:
        [Fact]
        public void RawHTML_Spec593_commonmark()
        {
            // The following Markdown:
            //     <a href='bar'title=title>
            //
            // Should be rendered as:
            //     <p>&lt;a href='bar'title=title&gt;</p>

            SpecTestHelper.AssertCompliance("<a href='bar'title=title>", 
                "<p>&lt;a href='bar'title=title&gt;</p>", 
                "commonmark");
        }

        // Closing tags:
        [Fact]
        public void RawHTML_Spec594_commonmark()
        {
            // The following Markdown:
            //     </a></foo >
            //
            // Should be rendered as:
            //     <p></a></foo ></p>

            SpecTestHelper.AssertCompliance("</a></foo >", 
                "<p></a></foo ></p>", 
                "commonmark");
        }

        // Illegal attributes in closing tag:
        [Fact]
        public void RawHTML_Spec595_commonmark()
        {
            // The following Markdown:
            //     </a href="foo">
            //
            // Should be rendered as:
            //     <p>&lt;/a href=&quot;foo&quot;&gt;</p>

            SpecTestHelper.AssertCompliance("</a href=\"foo\">", 
                "<p>&lt;/a href=&quot;foo&quot;&gt;</p>", 
                "commonmark");
        }

        // Comments:
        [Fact]
        public void RawHTML_Spec596_commonmark()
        {
            // The following Markdown:
            //     foo <!-- this is a
            //     comment - with hyphen -->
            //
            // Should be rendered as:
            //     <p>foo <!-- this is a
            //     comment - with hyphen --></p>

            SpecTestHelper.AssertCompliance("foo <!-- this is a\ncomment - with hyphen -->", 
                "<p>foo <!-- this is a\ncomment - with hyphen --></p>", 
                "commonmark");
        }

        [Fact]
        public void RawHTML_Spec597_commonmark()
        {
            // The following Markdown:
            //     foo <!-- not a comment -- two hyphens -->
            //
            // Should be rendered as:
            //     <p>foo &lt;!-- not a comment -- two hyphens --&gt;</p>

            SpecTestHelper.AssertCompliance("foo <!-- not a comment -- two hyphens -->", 
                "<p>foo &lt;!-- not a comment -- two hyphens --&gt;</p>", 
                "commonmark");
        }

        // Not comments:
        [Fact]
        public void RawHTML_Spec598_commonmark()
        {
            // The following Markdown:
            //     foo <!--> foo -->
            //     
            //     foo <!-- foo--->
            //
            // Should be rendered as:
            //     <p>foo &lt;!--&gt; foo --&gt;</p>
            //     <p>foo &lt;!-- foo---&gt;</p>

            SpecTestHelper.AssertCompliance("foo <!--> foo -->\n\nfoo <!-- foo--->", 
                "<p>foo &lt;!--&gt; foo --&gt;</p>\n<p>foo &lt;!-- foo---&gt;</p>", 
                "commonmark");
        }

        // Processing instructions:
        [Fact]
        public void RawHTML_Spec599_commonmark()
        {
            // The following Markdown:
            //     foo <?php echo $a; ?>
            //
            // Should be rendered as:
            //     <p>foo <?php echo $a; ?></p>

            SpecTestHelper.AssertCompliance("foo <?php echo $a; ?>", 
                "<p>foo <?php echo $a; ?></p>", 
                "commonmark");
        }

        // Declarations:
        [Fact]
        public void RawHTML_Spec600_commonmark()
        {
            // The following Markdown:
            //     foo <!ELEMENT br EMPTY>
            //
            // Should be rendered as:
            //     <p>foo <!ELEMENT br EMPTY></p>

            SpecTestHelper.AssertCompliance("foo <!ELEMENT br EMPTY>", 
                "<p>foo <!ELEMENT br EMPTY></p>", 
                "commonmark");
        }

        // CDATA sections:
        [Fact]
        public void RawHTML_Spec601_commonmark()
        {
            // The following Markdown:
            //     foo <![CDATA[>&<]]>
            //
            // Should be rendered as:
            //     <p>foo <![CDATA[>&<]]></p>

            SpecTestHelper.AssertCompliance("foo <![CDATA[>&<]]>", 
                "<p>foo <![CDATA[>&<]]></p>", 
                "commonmark");
        }

        // Entity and numeric character references are preserved in HTML
        // attributes:
        [Fact]
        public void RawHTML_Spec602_commonmark()
        {
            // The following Markdown:
            //     foo <a href="&ouml;">
            //
            // Should be rendered as:
            //     <p>foo <a href="&ouml;"></p>

            SpecTestHelper.AssertCompliance("foo <a href=\"&ouml;\">", 
                "<p>foo <a href=\"&ouml;\"></p>", 
                "commonmark");
        }

        // Backslash escapes do not work in HTML attributes:
        [Fact]
        public void RawHTML_Spec603_commonmark()
        {
            // The following Markdown:
            //     foo <a href="\*">
            //
            // Should be rendered as:
            //     <p>foo <a href="\*"></p>

            SpecTestHelper.AssertCompliance("foo <a href=\"\\*\">", 
                "<p>foo <a href=\"\\*\"></p>", 
                "commonmark");
        }

        [Fact]
        public void RawHTML_Spec604_commonmark()
        {
            // The following Markdown:
            //     <a href="\"">
            //
            // Should be rendered as:
            //     <p>&lt;a href=&quot;&quot;&quot;&gt;</p>

            SpecTestHelper.AssertCompliance("<a href=\"\\\"\">", 
                "<p>&lt;a href=&quot;&quot;&quot;&gt;</p>", 
                "commonmark");
        }
    }

    // A line break (not in a code span or HTML tag) that is preceded
    // by two or more spaces and does not occur at the end of a block
    // is parsed as a [hard line break](@) (rendered
    // in HTML as a `<br />` tag):
    public class HardLineBreaksTests
    {

        [Fact]
        public void HardLineBreaks_Spec605_commonmark()
        {
            // The following Markdown:
            //     foo  
            //     baz
            //
            // Should be rendered as:
            //     <p>foo<br />
            //     baz</p>

            SpecTestHelper.AssertCompliance("foo  \nbaz", 
                "<p>foo<br />\nbaz</p>", 
                "commonmark");
        }

        // For a more visible alternative, a backslash before the
        // [line ending] may be used instead of two spaces:
        [Fact]
        public void HardLineBreaks_Spec606_commonmark()
        {
            // The following Markdown:
            //     foo\
            //     baz
            //
            // Should be rendered as:
            //     <p>foo<br />
            //     baz</p>

            SpecTestHelper.AssertCompliance("foo\\\nbaz", 
                "<p>foo<br />\nbaz</p>", 
                "commonmark");
        }

        // More than two spaces can be used:
        [Fact]
        public void HardLineBreaks_Spec607_commonmark()
        {
            // The following Markdown:
            //     foo       
            //     baz
            //
            // Should be rendered as:
            //     <p>foo<br />
            //     baz</p>

            SpecTestHelper.AssertCompliance("foo       \nbaz", 
                "<p>foo<br />\nbaz</p>", 
                "commonmark");
        }

        // Leading spaces at the beginning of the next line are ignored:
        [Fact]
        public void HardLineBreaks_Spec608_commonmark()
        {
            // The following Markdown:
            //     foo  
            //          bar
            //
            // Should be rendered as:
            //     <p>foo<br />
            //     bar</p>

            SpecTestHelper.AssertCompliance("foo  \n     bar", 
                "<p>foo<br />\nbar</p>", 
                "commonmark");
        }

        [Fact]
        public void HardLineBreaks_Spec609_commonmark()
        {
            // The following Markdown:
            //     foo\
            //          bar
            //
            // Should be rendered as:
            //     <p>foo<br />
            //     bar</p>

            SpecTestHelper.AssertCompliance("foo\\\n     bar", 
                "<p>foo<br />\nbar</p>", 
                "commonmark");
        }

        // Line breaks can occur inside emphasis, links, and other constructs
        // that allow inline content:
        [Fact]
        public void HardLineBreaks_Spec610_commonmark()
        {
            // The following Markdown:
            //     *foo  
            //     bar*
            //
            // Should be rendered as:
            //     <p><em>foo<br />
            //     bar</em></p>

            SpecTestHelper.AssertCompliance("*foo  \nbar*", 
                "<p><em>foo<br />\nbar</em></p>", 
                "commonmark");
        }

        [Fact]
        public void HardLineBreaks_Spec611_commonmark()
        {
            // The following Markdown:
            //     *foo\
            //     bar*
            //
            // Should be rendered as:
            //     <p><em>foo<br />
            //     bar</em></p>

            SpecTestHelper.AssertCompliance("*foo\\\nbar*", 
                "<p><em>foo<br />\nbar</em></p>", 
                "commonmark");
        }

        // Line breaks do not occur inside code spans
        [Fact]
        public void HardLineBreaks_Spec612_commonmark()
        {
            // The following Markdown:
            //     `code  
            //     span`
            //
            // Should be rendered as:
            //     <p><code>code span</code></p>

            SpecTestHelper.AssertCompliance("`code  \nspan`", 
                "<p><code>code span</code></p>", 
                "commonmark");
        }

        [Fact]
        public void HardLineBreaks_Spec613_commonmark()
        {
            // The following Markdown:
            //     `code\
            //     span`
            //
            // Should be rendered as:
            //     <p><code>code\ span</code></p>

            SpecTestHelper.AssertCompliance("`code\\\nspan`", 
                "<p><code>code\\ span</code></p>", 
                "commonmark");
        }

        // or HTML tags:
        [Fact]
        public void HardLineBreaks_Spec614_commonmark()
        {
            // The following Markdown:
            //     <a href="foo  
            //     bar">
            //
            // Should be rendered as:
            //     <p><a href="foo  
            //     bar"></p>

            SpecTestHelper.AssertCompliance("<a href=\"foo  \nbar\">", 
                "<p><a href=\"foo  \nbar\"></p>", 
                "commonmark");
        }

        [Fact]
        public void HardLineBreaks_Spec615_commonmark()
        {
            // The following Markdown:
            //     <a href="foo\
            //     bar">
            //
            // Should be rendered as:
            //     <p><a href="foo\
            //     bar"></p>

            SpecTestHelper.AssertCompliance("<a href=\"foo\\\nbar\">", 
                "<p><a href=\"foo\\\nbar\"></p>", 
                "commonmark");
        }

        // Hard line breaks are for separating inline content within a block.
        // Neither syntax for hard line breaks works at the end of a paragraph or
        // other block element:
        [Fact]
        public void HardLineBreaks_Spec616_commonmark()
        {
            // The following Markdown:
            //     foo\
            //
            // Should be rendered as:
            //     <p>foo\</p>

            SpecTestHelper.AssertCompliance("foo\\", 
                "<p>foo\\</p>", 
                "commonmark");
        }

        [Fact]
        public void HardLineBreaks_Spec617_commonmark()
        {
            // The following Markdown:
            //     foo  
            //
            // Should be rendered as:
            //     <p>foo</p>

            SpecTestHelper.AssertCompliance("foo  ", 
                "<p>foo</p>", 
                "commonmark");
        }

        [Fact]
        public void HardLineBreaks_Spec618_commonmark()
        {
            // The following Markdown:
            //     ### foo\
            //
            // Should be rendered as:
            //     <h3>foo\</h3>

            SpecTestHelper.AssertCompliance("### foo\\", 
                "<h3>foo\\</h3>", 
                "commonmark");
        }

        [Fact]
        public void HardLineBreaks_Spec619_commonmark()
        {
            // The following Markdown:
            //     ### foo  
            //
            // Should be rendered as:
            //     <h3>foo</h3>

            SpecTestHelper.AssertCompliance("### foo  ", 
                "<h3>foo</h3>", 
                "commonmark");
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
        public void SoftLineBreaks_Spec620_commonmark()
        {
            // The following Markdown:
            //     foo
            //     baz
            //
            // Should be rendered as:
            //     <p>foo
            //     baz</p>

            SpecTestHelper.AssertCompliance("foo\nbaz", 
                "<p>foo\nbaz</p>", 
                "commonmark");
        }

        // Spaces at the end of the line and beginning of the next line are
        // removed:
        [Fact]
        public void SoftLineBreaks_Spec621_commonmark()
        {
            // The following Markdown:
            //     foo 
            //      baz
            //
            // Should be rendered as:
            //     <p>foo
            //     baz</p>

            SpecTestHelper.AssertCompliance("foo \n baz", 
                "<p>foo\nbaz</p>", 
                "commonmark");
        }
    }

    // Any characters not given an interpretation by the above rules will
    // be parsed as plain textual content.
    public class TextualContentTests
    {

        [Fact]
        public void TextualContent_Spec622_commonmark()
        {
            // The following Markdown:
            //     hello $.;'there
            //
            // Should be rendered as:
            //     <p>hello $.;'there</p>

            SpecTestHelper.AssertCompliance("hello $.;'there", 
                "<p>hello $.;'there</p>", 
                "commonmark");
        }

        [Fact]
        public void TextualContent_Spec623_commonmark()
        {
            // The following Markdown:
            //     Foo χρῆν
            //
            // Should be rendered as:
            //     <p>Foo χρῆν</p>

            SpecTestHelper.AssertCompliance("Foo χρῆν", 
                "<p>Foo χρῆν</p>", 
                "commonmark");
        }

        // Internal spaces are preserved verbatim:
        [Fact]
        public void TextualContent_Spec624_commonmark()
        {
            // The following Markdown:
            //     Multiple     spaces
            //
            // Should be rendered as:
            //     <p>Multiple     spaces</p>

            SpecTestHelper.AssertCompliance("Multiple     spaces", 
                "<p>Multiple     spaces</p>", 
                "commonmark");
        }
    }

    // A typical article is divided into logical sections. Often, the HTML for logical sections are demarcated by heading elements.
    // The [HTML spec](https://html.spec.whatwg.org/multipage/sections.html#headings-and-sections) encourages wrapping of 
    // logical sections in [sectioning content elements](https://html.spec.whatwg.org/multipage/dom.html#sectioning-content-2).
    // This extension wraps logical sections in `<section>` elements, with nesting dependent on [ATX heading](https://spec.commonmark.org/0.28/#atx-headings)
    // levels.
    public class SectionsTests
    {
        // Sequential higher-level sections are nested:
        [Fact]
        public void Sections_Spec1_jsonoptions_sections()
        {
            // The following Markdown:
            //     # foo
            //     ## foo
            //     ### foo
            //     #### foo
            //
            // Should be rendered as:
            //     <h1>foo</h1>
            //     <section id="foo">
            //     <h2>foo</h2>
            //     <section id="foo-1">
            //     <h3>foo</h3>
            //     <section id="foo-2">
            //     <h4>foo</h4>
            //     </section>
            //     </section>
            //     </section>

            SpecTestHelper.AssertCompliance("# foo\n## foo\n### foo\n#### foo", 
                "<h1>foo</h1>\n<section id=\"foo\">\n<h2>foo</h2>\n<section id=\"foo-1\">\n<h3>foo</h3>\n<section id=\"foo-2\">\n<h4>foo</h4>\n</section>\n</section>\n</section>", 
                "jsonoptions_sections");
        }

        // Sequential higher-level sections are nested:
        [Fact]
        public void Sections_Spec1_all()
        {
            // The following Markdown:
            //     # foo
            //     ## foo
            //     ### foo
            //     #### foo
            //
            // Should be rendered as:
            //     <h1>foo</h1>
            //     <section id="foo">
            //     <h2>foo</h2>
            //     <section id="foo-1">
            //     <h3>foo</h3>
            //     <section id="foo-2">
            //     <h4>foo</h4>
            //     </section>
            //     </section>
            //     </section>

            SpecTestHelper.AssertCompliance("# foo\n## foo\n### foo\n#### foo", 
                "<h1>foo</h1>\n<section id=\"foo\">\n<h2>foo</h2>\n<section id=\"foo-1\">\n<h3>foo</h3>\n<section id=\"foo-2\">\n<h4>foo</h4>\n</section>\n</section>\n</section>", 
                "all");
        }

        // Sequential lower-level sections are not nested.:
        [Fact]
        public void Sections_Spec2_jsonoptions_sections()
        {
            // The following Markdown:
            //     ## foo
            //     # foo
            //
            // Should be rendered as:
            //     <section id="foo">
            //     <h2>foo</h2>
            //     </section>
            //     <h1>foo</h1>

            SpecTestHelper.AssertCompliance("## foo\n# foo", 
                "<section id=\"foo\">\n<h2>foo</h2>\n</section>\n<h1>foo</h1>", 
                "jsonoptions_sections");
        }

        // Sequential lower-level sections are not nested.:
        [Fact]
        public void Sections_Spec2_all()
        {
            // The following Markdown:
            //     ## foo
            //     # foo
            //
            // Should be rendered as:
            //     <section id="foo">
            //     <h2>foo</h2>
            //     </section>
            //     <h1>foo</h1>

            SpecTestHelper.AssertCompliance("## foo\n# foo", 
                "<section id=\"foo\">\n<h2>foo</h2>\n</section>\n<h1>foo</h1>", 
                "all");
        }

        // Sequential same-level sections are not nested:
        [Fact]
        public void Sections_Spec3_jsonoptions_sections()
        {
            // The following Markdown:
            //     ## foo
            //     ## foo
            //
            // Should be rendered as:
            //     <section id="foo">
            //     <h2>foo</h2>
            //     </section>
            //     <section id="foo-1">
            //     <h2>foo</h2>
            //     </section>

            SpecTestHelper.AssertCompliance("## foo\n## foo", 
                "<section id=\"foo\">\n<h2>foo</h2>\n</section>\n<section id=\"foo-1\">\n<h2>foo</h2>\n</section>", 
                "jsonoptions_sections");
        }

        // Sequential same-level sections are not nested:
        [Fact]
        public void Sections_Spec3_all()
        {
            // The following Markdown:
            //     ## foo
            //     ## foo
            //
            // Should be rendered as:
            //     <section id="foo">
            //     <h2>foo</h2>
            //     </section>
            //     <section id="foo-1">
            //     <h2>foo</h2>
            //     </section>

            SpecTestHelper.AssertCompliance("## foo\n## foo", 
                "<section id=\"foo\">\n<h2>foo</h2>\n</section>\n<section id=\"foo-1\">\n<h2>foo</h2>\n</section>", 
                "all");
        }

        // Mixed sections:
        [Fact]
        public void Sections_Spec4_jsonoptions_sections()
        {
            // The following Markdown:
            //     # foo
            //     ## foo
            //     ### foo
            //     ## foo
            //
            // Should be rendered as:
            //     <h1>foo</h1>
            //     <section id="foo">
            //     <h2>foo</h2>
            //     <section id="foo-1">
            //     <h3>foo</h3>
            //     </section>
            //     </section>
            //     <section id="foo-2">
            //     <h2>foo</h2>
            //     </section>

            SpecTestHelper.AssertCompliance("# foo\n## foo\n### foo\n## foo", 
                "<h1>foo</h1>\n<section id=\"foo\">\n<h2>foo</h2>\n<section id=\"foo-1\">\n<h3>foo</h3>\n</section>\n</section>\n<section id=\"foo-2\">\n<h2>foo</h2>\n</section>", 
                "jsonoptions_sections");
        }

        // Mixed sections:
        [Fact]
        public void Sections_Spec4_all()
        {
            // The following Markdown:
            //     # foo
            //     ## foo
            //     ### foo
            //     ## foo
            //
            // Should be rendered as:
            //     <h1>foo</h1>
            //     <section id="foo">
            //     <h2>foo</h2>
            //     <section id="foo-1">
            //     <h3>foo</h3>
            //     </section>
            //     </section>
            //     <section id="foo-2">
            //     <h2>foo</h2>
            //     </section>

            SpecTestHelper.AssertCompliance("# foo\n## foo\n### foo\n## foo", 
                "<h1>foo</h1>\n<section id=\"foo\">\n<h2>foo</h2>\n<section id=\"foo-1\">\n<h3>foo</h3>\n</section>\n</section>\n<section id=\"foo-2\">\n<h2>foo</h2>\n</section>", 
                "all");
        }

        // Sections wrap content:
        [Fact]
        public void Sections_Spec5_jsonoptions_sections()
        {
            // The following Markdown:
            //     # foo
            //     Level 1 content.
            //     ## foo
            //     - Level 2 content line 1.
            //     - Level 2 content line 2.
            //     ### foo
            //     > Level 3 content line 1.
            //     > Level 3 content line 2.
            //
            // Should be rendered as:
            //     <h1>foo</h1>
            //     <p>Level 1 content.</p>
            //     <section id="foo">
            //     <h2>foo</h2>
            //     <ul>
            //     <li>Level 2 content line 1.</li>
            //     <li>Level 2 content line 2.</li>
            //     </ul>
            //     <section id="foo-1">
            //     <h3>foo</h3>
            //     <blockquote>
            //     <p>Level 3 content line 1.
            //     Level 3 content line 2.</p>
            //     </blockquote>
            //     </section>
            //     </section>

            SpecTestHelper.AssertCompliance("# foo\nLevel 1 content.\n## foo\n- Level 2 content line 1.\n- Level 2 content line 2.\n### foo\n> Level 3 content line 1.\n> Level 3 content line 2.", 
                "<h1>foo</h1>\n<p>Level 1 content.</p>\n<section id=\"foo\">\n<h2>foo</h2>\n<ul>\n<li>Level 2 content line 1.</li>\n<li>Level 2 content line 2.</li>\n</ul>\n<section id=\"foo-1\">\n<h3>foo</h3>\n<blockquote>\n<p>Level 3 content line 1.\nLevel 3 content line 2.</p>\n</blockquote>\n</section>\n</section>", 
                "jsonoptions_sections");
        }

        // Sections wrap content:
        [Fact]
        public void Sections_Spec5_all()
        {
            // The following Markdown:
            //     # foo
            //     Level 1 content.
            //     ## foo
            //     - Level 2 content line 1.
            //     - Level 2 content line 2.
            //     ### foo
            //     > Level 3 content line 1.
            //     > Level 3 content line 2.
            //
            // Should be rendered as:
            //     <h1>foo</h1>
            //     <p>Level 1 content.</p>
            //     <section id="foo">
            //     <h2>foo</h2>
            //     <ul>
            //     <li>Level 2 content line 1.</li>
            //     <li>Level 2 content line 2.</li>
            //     </ul>
            //     <section id="foo-1">
            //     <h3>foo</h3>
            //     <blockquote>
            //     <p>Level 3 content line 1.
            //     Level 3 content line 2.</p>
            //     </blockquote>
            //     </section>
            //     </section>

            SpecTestHelper.AssertCompliance("# foo\nLevel 1 content.\n## foo\n- Level 2 content line 1.\n- Level 2 content line 2.\n### foo\n> Level 3 content line 1.\n> Level 3 content line 2.", 
                "<h1>foo</h1>\n<p>Level 1 content.</p>\n<section id=\"foo\">\n<h2>foo</h2>\n<ul>\n<li>Level 2 content line 1.</li>\n<li>Level 2 content line 2.</li>\n</ul>\n<section id=\"foo-1\">\n<h3>foo</h3>\n<blockquote>\n<p>Level 3 content line 1.\nLevel 3 content line 2.</p>\n</blockquote>\n</section>\n</section>", 
                "all");
        }

        // To enable wrapping of level 1 headers, set `SectionExtensionOptions.Level1WrapperElement` to any `SectioningContentElement` value other than `None` and `Undefined`. For example:
        [Fact]
        public void Sections_Spec6_jsonoptions_sections()
        {
            // The following Markdown:
            //     # foo
            //     ## foo
            //
            // With extension options:
            //     {
            //         "sections": {
            //             "level1WrapperElement": "article"
            //         }
            //     }
            //
            // Should be rendered as:
            //     <article id="foo">
            //     <h1>foo</h1>
            //     <section id="foo-1">
            //     <h2>foo</h2>
            //     </section>
            //     </article>

            SpecTestHelper.AssertCompliance("# foo\n## foo", 
                "<article id=\"foo\">\n<h1>foo</h1>\n<section id=\"foo-1\">\n<h2>foo</h2>\n</section>\n</article>", 
                "jsonoptions_sections", 
                "{\n    \"sections\": {\n        \"level1WrapperElement\": \"article\"\n    }\n}");
        }

        // To enable wrapping of level 1 headers, set `SectionExtensionOptions.Level1WrapperElement` to any `SectioningContentElement` value other than `None` and `Undefined`. For example:
        [Fact]
        public void Sections_Spec6_all()
        {
            // The following Markdown:
            //     # foo
            //     ## foo
            //
            // With extension options:
            //     {
            //         "sections": {
            //             "level1WrapperElement": "article"
            //         }
            //     }
            //
            // Should be rendered as:
            //     <article id="foo">
            //     <h1>foo</h1>
            //     <section id="foo-1">
            //     <h2>foo</h2>
            //     </section>
            //     </article>

            SpecTestHelper.AssertCompliance("# foo\n## foo", 
                "<article id=\"foo\">\n<h1>foo</h1>\n<section id=\"foo-1\">\n<h2>foo</h2>\n</section>\n</article>", 
                "all", 
                "{\n    \"sections\": {\n        \"level1WrapperElement\": \"article\"\n    }\n}");
        }

        // To change the element used to wrap level 2+ headers, set `SectionExtensionOptions.Level2PlusWrapperElement". For example:
        [Fact]
        public void Sections_Spec7_jsonoptions_sections()
        {
            // The following Markdown:
            //     ## foo
            //
            // With extension options:
            //     {
            //         "sections": {
            //             "level2PlusWrapperElement": "nav"
            //         }
            //     }
            //
            // Should be rendered as:
            //     <nav id="foo">
            //     <h2>foo</h2>
            //     </nav>

            SpecTestHelper.AssertCompliance("## foo", 
                "<nav id=\"foo\">\n<h2>foo</h2>\n</nav>", 
                "jsonoptions_sections", 
                "{\n    \"sections\": {\n        \"level2PlusWrapperElement\": \"nav\"\n    }\n}");
        }

        // To change the element used to wrap level 2+ headers, set `SectionExtensionOptions.Level2PlusWrapperElement". For example:
        [Fact]
        public void Sections_Spec7_all()
        {
            // The following Markdown:
            //     ## foo
            //
            // With extension options:
            //     {
            //         "sections": {
            //             "level2PlusWrapperElement": "nav"
            //         }
            //     }
            //
            // Should be rendered as:
            //     <nav id="foo">
            //     <h2>foo</h2>
            //     </nav>

            SpecTestHelper.AssertCompliance("## foo", 
                "<nav id=\"foo\">\n<h2>foo</h2>\n</nav>", 
                "all", 
                "{\n    \"sections\": {\n        \"level2PlusWrapperElement\": \"nav\"\n    }\n}");
        }

        // Kebab-case (lowercase words joined by dashes) IDs are generated for each section:
        [Fact]
        public void Sections_Spec8_jsonoptions_sections()
        {
            // The following Markdown:
            //     ## Foo Bar Baz
            //
            // Should be rendered as:
            //     <section id="foo-bar-baz">
            //     <h2>Foo Bar Baz</h2>
            //     </section>

            SpecTestHelper.AssertCompliance("## Foo Bar Baz", 
                "<section id=\"foo-bar-baz\">\n<h2>Foo Bar Baz</h2>\n</section>", 
                "jsonoptions_sections");
        }

        // Kebab-case (lowercase words joined by dashes) IDs are generated for each section:
        [Fact]
        public void Sections_Spec8_all()
        {
            // The following Markdown:
            //     ## Foo Bar Baz
            //
            // Should be rendered as:
            //     <section id="foo-bar-baz">
            //     <h2>Foo Bar Baz</h2>
            //     </section>

            SpecTestHelper.AssertCompliance("## Foo Bar Baz", 
                "<section id=\"foo-bar-baz\">\n<h2>Foo Bar Baz</h2>\n</section>", 
                "all");
        }

        // Auto generation of IDs can be disabled by setting `SectionExtensionOptions.DefaultSectionBlockOptions.GenerateIdentifier` to `false`:
        [Fact]
        public void Sections_Spec9_jsonoptions_sections()
        {
            // The following Markdown:
            //     ## Foo Bar Baz
            //
            // With extension options:
            //     {
            //         "sections": {
            //             "defaultSectionBlockOptions": {
            //                 "generateIdentifier": false
            //             }
            //         }
            //     }
            //
            // Should be rendered as:
            //     <section>
            //     <h2>Foo Bar Baz</h2>
            //     </section>

            SpecTestHelper.AssertCompliance("## Foo Bar Baz", 
                "<section>\n<h2>Foo Bar Baz</h2>\n</section>", 
                "jsonoptions_sections", 
                "{\n    \"sections\": {\n        \"defaultSectionBlockOptions\": {\n            \"generateIdentifier\": false\n        }\n    }\n}");
        }

        // Auto generation of IDs can be disabled by setting `SectionExtensionOptions.DefaultSectionBlockOptions.GenerateIdentifier` to `false`:
        [Fact]
        public void Sections_Spec9_all()
        {
            // The following Markdown:
            //     ## Foo Bar Baz
            //
            // With extension options:
            //     {
            //         "sections": {
            //             "defaultSectionBlockOptions": {
            //                 "generateIdentifier": false
            //             }
            //         }
            //     }
            //
            // Should be rendered as:
            //     <section>
            //     <h2>Foo Bar Baz</h2>
            //     </section>

            SpecTestHelper.AssertCompliance("## Foo Bar Baz", 
                "<section>\n<h2>Foo Bar Baz</h2>\n</section>", 
                "all", 
                "{\n    \"sections\": {\n        \"defaultSectionBlockOptions\": {\n            \"generateIdentifier\": false\n        }\n    }\n}");
        }

        // Sections can be linked to by the text content of their headings:
        [Fact]
        public void Sections_Spec10_jsonoptions_sections()
        {
            // The following Markdown:
            //     [foo]
            //     
            //     ## foo
            //     ### foo bar
            //     [foo bar]
            //     #### foo bar baz
            //     
            //     [Link Text][foo bar baz]
            //
            // Should be rendered as:
            //     <p><a href="#foo">foo</a></p>
            //     <section id="foo">
            //     <h2>foo</h2>
            //     <section id="foo-bar">
            //     <h3>foo bar</h3>
            //     <p><a href="#foo-bar">foo bar</a></p>
            //     <section id="foo-bar-baz">
            //     <h4>foo bar baz</h4>
            //     <p><a href="#foo-bar-baz">Link Text</a></p>
            //     </section>
            //     </section>
            //     </section>

            SpecTestHelper.AssertCompliance("[foo]\n\n## foo\n### foo bar\n[foo bar]\n#### foo bar baz\n\n[Link Text][foo bar baz]", 
                "<p><a href=\"#foo\">foo</a></p>\n<section id=\"foo\">\n<h2>foo</h2>\n<section id=\"foo-bar\">\n<h3>foo bar</h3>\n<p><a href=\"#foo-bar\">foo bar</a></p>\n<section id=\"foo-bar-baz\">\n<h4>foo bar baz</h4>\n<p><a href=\"#foo-bar-baz\">Link Text</a></p>\n</section>\n</section>\n</section>", 
                "jsonoptions_sections");
        }

        // Sections can be linked to by the text content of their headings:
        [Fact]
        public void Sections_Spec10_all()
        {
            // The following Markdown:
            //     [foo]
            //     
            //     ## foo
            //     ### foo bar
            //     [foo bar]
            //     #### foo bar baz
            //     
            //     [Link Text][foo bar baz]
            //
            // Should be rendered as:
            //     <p><a href="#foo">foo</a></p>
            //     <section id="foo">
            //     <h2>foo</h2>
            //     <section id="foo-bar">
            //     <h3>foo bar</h3>
            //     <p><a href="#foo-bar">foo bar</a></p>
            //     <section id="foo-bar-baz">
            //     <h4>foo bar baz</h4>
            //     <p><a href="#foo-bar-baz">Link Text</a></p>
            //     </section>
            //     </section>
            //     </section>

            SpecTestHelper.AssertCompliance("[foo]\n\n## foo\n### foo bar\n[foo bar]\n#### foo bar baz\n\n[Link Text][foo bar baz]", 
                "<p><a href=\"#foo\">foo</a></p>\n<section id=\"foo\">\n<h2>foo</h2>\n<section id=\"foo-bar\">\n<h3>foo bar</h3>\n<p><a href=\"#foo-bar\">foo bar</a></p>\n<section id=\"foo-bar-baz\">\n<h4>foo bar baz</h4>\n<p><a href=\"#foo-bar-baz\">Link Text</a></p>\n</section>\n</section>\n</section>", 
                "all");
        }

        // Linking to sections by the text content of their headings can be disabled by setting `SectionExtensionOptions.DefaultSectionBlockOptions.AutoLinkable` to `false` (note 
        // that linking to sections is also disabled if `SectionExtensionOptions.DefaultSectionBlockOptions.GenerateIdentifier` is set to `false`):
        [Fact]
        public void Sections_Spec11_jsonoptions_sections()
        {
            // The following Markdown:
            //     [foo]
            //     
            //     ## foo
            //     ### foo bar
            //     [foo bar]
            //     #### foo bar baz
            //     
            //     [foo bar baz]
            //
            // With extension options:
            //     {
            //         "sections": {
            //             "defaultSectionBlockOptions": {
            //                 "autoLinkable": false
            //             }
            //         }
            //     }
            //
            // Should be rendered as:
            //     <p>[foo]</p>
            //     <section id="foo">
            //     <h2>foo</h2>
            //     <section id="foo-bar">
            //     <h3>foo bar</h3>
            //     <p>[foo bar]</p>
            //     <section id="foo-bar-baz">
            //     <h4>foo bar baz</h4>
            //     <p>[foo bar baz]</p>
            //     </section>
            //     </section>
            //     </section>

            SpecTestHelper.AssertCompliance("[foo]\n\n## foo\n### foo bar\n[foo bar]\n#### foo bar baz\n\n[foo bar baz]", 
                "<p>[foo]</p>\n<section id=\"foo\">\n<h2>foo</h2>\n<section id=\"foo-bar\">\n<h3>foo bar</h3>\n<p>[foo bar]</p>\n<section id=\"foo-bar-baz\">\n<h4>foo bar baz</h4>\n<p>[foo bar baz]</p>\n</section>\n</section>\n</section>", 
                "jsonoptions_sections", 
                "{\n    \"sections\": {\n        \"defaultSectionBlockOptions\": {\n            \"autoLinkable\": false\n        }\n    }\n}");
        }

        // Linking to sections by the text content of their headings can be disabled by setting `SectionExtensionOptions.DefaultSectionBlockOptions.AutoLinkable` to `false` (note 
        // that linking to sections is also disabled if `SectionExtensionOptions.DefaultSectionBlockOptions.GenerateIdentifier` is set to `false`):
        [Fact]
        public void Sections_Spec11_all()
        {
            // The following Markdown:
            //     [foo]
            //     
            //     ## foo
            //     ### foo bar
            //     [foo bar]
            //     #### foo bar baz
            //     
            //     [foo bar baz]
            //
            // With extension options:
            //     {
            //         "sections": {
            //             "defaultSectionBlockOptions": {
            //                 "autoLinkable": false
            //             }
            //         }
            //     }
            //
            // Should be rendered as:
            //     <p>[foo]</p>
            //     <section id="foo">
            //     <h2>foo</h2>
            //     <section id="foo-bar">
            //     <h3>foo bar</h3>
            //     <p>[foo bar]</p>
            //     <section id="foo-bar-baz">
            //     <h4>foo bar baz</h4>
            //     <p>[foo bar baz]</p>
            //     </section>
            //     </section>
            //     </section>

            SpecTestHelper.AssertCompliance("[foo]\n\n## foo\n### foo bar\n[foo bar]\n#### foo bar baz\n\n[foo bar baz]", 
                "<p>[foo]</p>\n<section id=\"foo\">\n<h2>foo</h2>\n<section id=\"foo-bar\">\n<h3>foo bar</h3>\n<p>[foo bar]</p>\n<section id=\"foo-bar-baz\">\n<h4>foo bar baz</h4>\n<p>[foo bar baz]</p>\n</section>\n</section>\n</section>", 
                "all", 
                "{\n    \"sections\": {\n        \"defaultSectionBlockOptions\": {\n            \"autoLinkable\": false\n        }\n    }\n}");
        }

        // Per-section-block options can be overriden if the JSON options extension is enabled:
        [Fact]
        public void Sections_Spec12_jsonoptions_sections()
        {
            // The following Markdown:
            //     @{
            //         "attributes": {
            //             "class": "book"
            //         }
            //     }
            //     # foo
            //     ## foo
            //     @{
            //         "wrapperElement": "nav"
            //     }
            //     ## foo
            //     @{
            //         "wrapperElement": "aside"
            //     }
            //     # foo
            //
            // With extension options:
            //     {
            //         "sections": {
            //             "level1WrapperElement": "article",
            //             "defaultSectionBlockOptions": {
            //                 "attributes": {
            //                     "class": "chapter"
            //                 }
            //             }
            //         }
            //     }
            //
            // Should be rendered as:
            //     <article class="book" id="foo">
            //     <h1>foo</h1>
            //     <section class="chapter" id="foo-1">
            //     <h2>foo</h2>
            //     </section>
            //     <nav class="chapter" id="foo-2">
            //     <h2>foo</h2>
            //     </nav>
            //     </article>
            //     <aside class="chapter" id="foo-3">
            //     <h1>foo</h1>
            //     </aside>

            SpecTestHelper.AssertCompliance("@{\n    \"attributes\": {\n        \"class\": \"book\"\n    }\n}\n# foo\n## foo\n@{\n    \"wrapperElement\": \"nav\"\n}\n## foo\n@{\n    \"wrapperElement\": \"aside\"\n}\n# foo", 
                "<article class=\"book\" id=\"foo\">\n<h1>foo</h1>\n<section class=\"chapter\" id=\"foo-1\">\n<h2>foo</h2>\n</section>\n<nav class=\"chapter\" id=\"foo-2\">\n<h2>foo</h2>\n</nav>\n</article>\n<aside class=\"chapter\" id=\"foo-3\">\n<h1>foo</h1>\n</aside>", 
                "jsonoptions_sections", 
                "{\n    \"sections\": {\n        \"level1WrapperElement\": \"article\",\n        \"defaultSectionBlockOptions\": {\n            \"attributes\": {\n                \"class\": \"chapter\"\n            }\n        }\n    }\n}");
        }

        // Per-section-block options can be overriden if the JSON options extension is enabled:
        [Fact]
        public void Sections_Spec12_all()
        {
            // The following Markdown:
            //     @{
            //         "attributes": {
            //             "class": "book"
            //         }
            //     }
            //     # foo
            //     ## foo
            //     @{
            //         "wrapperElement": "nav"
            //     }
            //     ## foo
            //     @{
            //         "wrapperElement": "aside"
            //     }
            //     # foo
            //
            // With extension options:
            //     {
            //         "sections": {
            //             "level1WrapperElement": "article",
            //             "defaultSectionBlockOptions": {
            //                 "attributes": {
            //                     "class": "chapter"
            //                 }
            //             }
            //         }
            //     }
            //
            // Should be rendered as:
            //     <article class="book" id="foo">
            //     <h1>foo</h1>
            //     <section class="chapter" id="foo-1">
            //     <h2>foo</h2>
            //     </section>
            //     <nav class="chapter" id="foo-2">
            //     <h2>foo</h2>
            //     </nav>
            //     </article>
            //     <aside class="chapter" id="foo-3">
            //     <h1>foo</h1>
            //     </aside>

            SpecTestHelper.AssertCompliance("@{\n    \"attributes\": {\n        \"class\": \"book\"\n    }\n}\n# foo\n## foo\n@{\n    \"wrapperElement\": \"nav\"\n}\n## foo\n@{\n    \"wrapperElement\": \"aside\"\n}\n# foo", 
                "<article class=\"book\" id=\"foo\">\n<h1>foo</h1>\n<section class=\"chapter\" id=\"foo-1\">\n<h2>foo</h2>\n</section>\n<nav class=\"chapter\" id=\"foo-2\">\n<h2>foo</h2>\n</nav>\n</article>\n<aside class=\"chapter\" id=\"foo-3\">\n<h1>foo</h1>\n</aside>", 
                "all", 
                "{\n    \"sections\": {\n        \"level1WrapperElement\": \"article\",\n        \"defaultSectionBlockOptions\": {\n            \"attributes\": {\n                \"class\": \"chapter\"\n            }\n        }\n    }\n}");
        }
    }

    // Alerts are boxes within articles that contain tangential content. Such content can be things like extra information and warnings. Alerts have a similar syntax to 
    // blockquotes. However, they have very different purposes - according to the [specifications](https://html.spec.whatwg.org/multipage/grouping-content.html#the-blockquote-element)
    // blockquotes should be used when quoting from external articles.
    public class AlertsTests
    {
        // Every line of an alert must start with an `!`. The first line of an alert must be of the form `!<optional space><alert name>` where `<alert name>`
        // is a string containing 1 or more characters from the regex character set `[A-Za-z0-9_-]`. The result of appending `alert-` to the alert name is used as the
        // alert block's class:
        [Fact]
        public void Alerts_Spec1_jsonoptions_alerts()
        {
            // The following Markdown:
            //     ! critical-warning
            //     ! This is a critical warning.
            //
            // Should be rendered as:
            //     <div class="alert-critical-warning">
            //     <p>This is a critical warning.</p>
            //     </div>

            SpecTestHelper.AssertCompliance("! critical-warning\n! This is a critical warning.", 
                "<div class=\"alert-critical-warning\">\n<p>This is a critical warning.</p>\n</div>", 
                "jsonoptions_alerts");
        }

        // Every line of an alert must start with an `!`. The first line of an alert must be of the form `!<optional space><alert name>` where `<alert name>`
        // is a string containing 1 or more characters from the regex character set `[A-Za-z0-9_-]`. The result of appending `alert-` to the alert name is used as the
        // alert block's class:
        [Fact]
        public void Alerts_Spec1_all()
        {
            // The following Markdown:
            //     ! critical-warning
            //     ! This is a critical warning.
            //
            // Should be rendered as:
            //     <div class="alert-critical-warning">
            //     <p>This is a critical warning.</p>
            //     </div>

            SpecTestHelper.AssertCompliance("! critical-warning\n! This is a critical warning.", 
                "<div class=\"alert-critical-warning\">\n<p>This is a critical warning.</p>\n</div>", 
                "all");
        }

        // The block is ignored if the first line does not contain a level name :
        [Fact]
        public void Alerts_Spec2_jsonoptions_alerts()
        {
            // The following Markdown:
            //     ! 
            //     ! This is a warning.
            //
            // Should be rendered as:
            //     <p>!
            //     ! This is a warning.</p>

            SpecTestHelper.AssertCompliance("! \n! This is a warning.", 
                "<p>!\n! This is a warning.</p>", 
                "jsonoptions_alerts");
        }

        // The block is ignored if the first line does not contain a level name :
        [Fact]
        public void Alerts_Spec2_all()
        {
            // The following Markdown:
            //     ! 
            //     ! This is a warning.
            //
            // Should be rendered as:
            //     <p>!
            //     ! This is a warning.</p>

            SpecTestHelper.AssertCompliance("! \n! This is a warning.", 
                "<p>!\n! This is a warning.</p>", 
                "all");
        }

        // The block is ignored if the first line contains disallowed characters :
        [Fact]
        public void Alerts_Spec3_jsonoptions_alerts()
        {
            // The following Markdown:
            //     ! illegal space
            //     ! This is a warning.
            //
            // Should be rendered as:
            //     <p>! illegal space
            //     ! This is a warning.</p>

            SpecTestHelper.AssertCompliance("! illegal space\n! This is a warning.", 
                "<p>! illegal space\n! This is a warning.</p>", 
                "jsonoptions_alerts");
        }

        // The block is ignored if the first line contains disallowed characters :
        [Fact]
        public void Alerts_Spec3_all()
        {
            // The following Markdown:
            //     ! illegal space
            //     ! This is a warning.
            //
            // Should be rendered as:
            //     <p>! illegal space
            //     ! This is a warning.</p>

            SpecTestHelper.AssertCompliance("! illegal space\n! This is a warning.", 
                "<p>! illegal space\n! This is a warning.</p>", 
                "all");
        }

        // The first space after `!` is ignored. :
        [Fact]
        public void Alerts_Spec4_jsonoptions_alerts()
        {
            // The following Markdown:
            //     ! warning
            //     !This line will be rendered with 0 leading spaces.
            //     ! This line will also be rendered with 0 leading spaces.
            //
            // Should be rendered as:
            //     <div class="alert-warning">
            //     <p>This line will be rendered with 0 leading spaces.
            //     This line will also be rendered with 0 leading spaces.</p>
            //     </div>

            SpecTestHelper.AssertCompliance("! warning\n!This line will be rendered with 0 leading spaces.\n! This line will also be rendered with 0 leading spaces.", 
                "<div class=\"alert-warning\">\n<p>This line will be rendered with 0 leading spaces.\nThis line will also be rendered with 0 leading spaces.</p>\n</div>", 
                "jsonoptions_alerts");
        }

        // The first space after `!` is ignored. :
        [Fact]
        public void Alerts_Spec4_all()
        {
            // The following Markdown:
            //     ! warning
            //     !This line will be rendered with 0 leading spaces.
            //     ! This line will also be rendered with 0 leading spaces.
            //
            // Should be rendered as:
            //     <div class="alert-warning">
            //     <p>This line will be rendered with 0 leading spaces.
            //     This line will also be rendered with 0 leading spaces.</p>
            //     </div>

            SpecTestHelper.AssertCompliance("! warning\n!This line will be rendered with 0 leading spaces.\n! This line will also be rendered with 0 leading spaces.", 
                "<div class=\"alert-warning\">\n<p>This line will be rendered with 0 leading spaces.\nThis line will also be rendered with 0 leading spaces.</p>\n</div>", 
                "all");
        }
    }

    // Per-block options are useful for many extensions. For example, per-block options would allow a code extension to add line-numbers to select code blocks. 
    // Json options facilitates per-block options, using a simple and consistent syntax.
    public class JsonOptionsTests
    {
        // Json options are specified as a string above the block they apply to. The first line must begin with `@{`:
        [Fact]
        public void JsonOptions_Spec1_jsonoptions_sections()
        {
            // The following Markdown:
            //     @{"wrapperElement": "Aside"}
            //     # foo
            //
            // Should be rendered as:
            //     <aside id="foo">
            //     <h1>foo</h1>
            //     </aside>

            SpecTestHelper.AssertCompliance("@{\"wrapperElement\": \"Aside\"}\n# foo", 
                "<aside id=\"foo\">\n<h1>foo</h1>\n</aside>", 
                "jsonoptions_sections");
        }

        // Options can be specified across several lines:
        [Fact]
        public void JsonOptions_Spec2_jsonoptions_sections()
        {
            // The following Markdown:
            //     @{
            //         "wrapperElement": "Aside"
            //     }
            //     # foo
            //
            // Should be rendered as:
            //     <aside id="foo">
            //     <h1>foo</h1>
            //     </aside>

            SpecTestHelper.AssertCompliance("@{\n    \"wrapperElement\": \"Aside\"\n}\n# foo", 
                "<aside id=\"foo\">\n<h1>foo</h1>\n</aside>", 
                "jsonoptions_sections");
        }

        // If the first line does not begin with `@{`, the string becomes a paragraph:
        [Fact]
        public void JsonOptions_Spec3_jsonoptions_sections()
        {
            // The following Markdown:
            //     @
            //     {
            //         "wrapperElement": "Aside"
            //     }
            //     # foo
            //
            // Should be rendered as:
            //     <p>@
            //     {
            //     &quot;wrapperElement&quot;: &quot;Aside&quot;
            //     }</p>
            //     <h1>foo</h1>
            //     

            SpecTestHelper.AssertCompliance("@\n{\n    \"wrapperElement\": \"Aside\"\n}\n# foo", 
                "<p>@\n{\n&quot;wrapperElement&quot;: &quot;Aside&quot;\n}</p>\n<h1>foo</h1>\n", 
                "jsonoptions_sections");
        }
    }
}

