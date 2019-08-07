
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.Specs
{
    public class CommonMarkSpecs
    {
        [Fact]
        public void Tabs_Spec1()
        {
            // Line number in CommonMark Specs: 350
            // Markdown: \tfoo\tbaz\t\tbim\n
            // Expected HTML: <pre><code>foo\tbaz\t\tbim\n</code></pre>\n

            SpecTestHelper.AssertCompliance("\tfoo\tbaz\t\tbim\n",
                "<pre><code>foo\tbaz\t\tbim\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void Tabs_Spec2()
        {
            // Line number in CommonMark Specs: 357
            // Markdown:   \tfoo\tbaz\t\tbim\n
            // Expected HTML: <pre><code>foo\tbaz\t\tbim\n</code></pre>\n

            SpecTestHelper.AssertCompliance("  \tfoo\tbaz\t\tbim\n",
                "<pre><code>foo\tbaz\t\tbim\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void Tabs_Spec3()
        {
            // Line number in CommonMark Specs: 364
            // Markdown:     a\ta\n    ὐ\ta\n
            // Expected HTML: <pre><code>a\ta\nὐ\ta\n</code></pre>\n

            SpecTestHelper.AssertCompliance("    a\ta\n    ὐ\ta\n",
                "<pre><code>a\ta\nὐ\ta\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void Tabs_Spec4()
        {
            // Line number in CommonMark Specs: 377
            // Markdown:   - foo\n\n\tbar\n
            // Expected HTML: <ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("  - foo\n\n\tbar\n",
                "<ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void Tabs_Spec5()
        {
            // Line number in CommonMark Specs: 390
            // Markdown: - foo\n\n\t\tbar\n
            // Expected HTML: <ul>\n<li>\n<p>foo</p>\n<pre><code>  bar\n</code></pre>\n</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("- foo\n\n\t\tbar\n",
                "<ul>\n<li>\n<p>foo</p>\n<pre><code>  bar\n</code></pre>\n</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void Tabs_Spec6()
        {
            // Line number in CommonMark Specs: 413
            // Markdown: >\t\tfoo\n
            // Expected HTML: <blockquote>\n<pre><code>  foo\n</code></pre>\n</blockquote>\n

            SpecTestHelper.AssertCompliance(">\t\tfoo\n",
                "<blockquote>\n<pre><code>  foo\n</code></pre>\n</blockquote>\n",
                "all",
                true);
        }

        [Fact]
        public void Tabs_Spec7()
        {
            // Line number in CommonMark Specs: 422
            // Markdown: -\t\tfoo\n
            // Expected HTML: <ul>\n<li>\n<pre><code>  foo\n</code></pre>\n</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("-\t\tfoo\n",
                "<ul>\n<li>\n<pre><code>  foo\n</code></pre>\n</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void Tabs_Spec8()
        {
            // Line number in CommonMark Specs: 434
            // Markdown:     foo\n\tbar\n
            // Expected HTML: <pre><code>foo\nbar\n</code></pre>\n

            SpecTestHelper.AssertCompliance("    foo\n\tbar\n",
                "<pre><code>foo\nbar\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void Tabs_Spec9()
        {
            // Line number in CommonMark Specs: 443
            // Markdown:  - foo\n   - bar\n\t - baz\n
            // Expected HTML: <ul>\n<li>foo\n<ul>\n<li>bar\n<ul>\n<li>baz</li>\n</ul>\n</li>\n</ul>\n</li>\n</ul>\n

            SpecTestHelper.AssertCompliance(" - foo\n   - bar\n\t - baz\n",
                "<ul>\n<li>foo\n<ul>\n<li>bar\n<ul>\n<li>baz</li>\n</ul>\n</li>\n</ul>\n</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void Tabs_Spec10()
        {
            // Line number in CommonMark Specs: 461
            // Markdown: #\tFoo\n
            // Expected HTML: <h1>Foo</h1>\n

            SpecTestHelper.AssertCompliance("#\tFoo\n",
                "<h1>Foo</h1>\n",
                "all",
                true);
        }

        [Fact]
        public void Tabs_Spec11()
        {
            // Line number in CommonMark Specs: 467
            // Markdown: *\t*\t*\t\n
            // Expected HTML: <hr />\n

            SpecTestHelper.AssertCompliance("*\t*\t*\t\n",
                "<hr />\n",
                "all",
                true);
        }

        [Fact]
        public void Precedence_Spec12()
        {
            // Line number in CommonMark Specs: 494
            // Markdown: - `one\n- two`\n
            // Expected HTML: <ul>\n<li>`one</li>\n<li>two`</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("- `one\n- two`\n",
                "<ul>\n<li>`one</li>\n<li>two`</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void ThematicBreaks_Spec13()
        {
            // Line number in CommonMark Specs: 533
            // Markdown: ***\n---\n___\n
            // Expected HTML: <hr />\n<hr />\n<hr />\n

            SpecTestHelper.AssertCompliance("***\n---\n___\n",
                "<hr />\n<hr />\n<hr />\n",
                "all",
                true);
        }

        [Fact]
        public void ThematicBreaks_Spec14()
        {
            // Line number in CommonMark Specs: 546
            // Markdown: +++\n
            // Expected HTML: <p>+++</p>\n

            SpecTestHelper.AssertCompliance("+++\n",
                "<p>+++</p>\n",
                "all",
                true);
        }

        [Fact]
        public void ThematicBreaks_Spec15()
        {
            // Line number in CommonMark Specs: 553
            // Markdown: ===\n
            // Expected HTML: <p>===</p>\n

            SpecTestHelper.AssertCompliance("===\n",
                "<p>===</p>\n",
                "all",
                true);
        }

        [Fact]
        public void ThematicBreaks_Spec16()
        {
            // Line number in CommonMark Specs: 562
            // Markdown: --\n**\n__\n
            // Expected HTML: <p>--\n**\n__</p>\n

            SpecTestHelper.AssertCompliance("--\n**\n__\n",
                "<p>--\n**\n__</p>\n",
                "all",
                true);
        }

        [Fact]
        public void ThematicBreaks_Spec17()
        {
            // Line number in CommonMark Specs: 575
            // Markdown:  ***\n  ***\n   ***\n
            // Expected HTML: <hr />\n<hr />\n<hr />\n

            SpecTestHelper.AssertCompliance(" ***\n  ***\n   ***\n",
                "<hr />\n<hr />\n<hr />\n",
                "all",
                true);
        }

        [Fact]
        public void ThematicBreaks_Spec18()
        {
            // Line number in CommonMark Specs: 588
            // Markdown:     ***\n
            // Expected HTML: <pre><code>***\n</code></pre>\n

            SpecTestHelper.AssertCompliance("    ***\n",
                "<pre><code>***\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void ThematicBreaks_Spec19()
        {
            // Line number in CommonMark Specs: 596
            // Markdown: Foo\n    ***\n
            // Expected HTML: <p>Foo\n***</p>\n

            SpecTestHelper.AssertCompliance("Foo\n    ***\n",
                "<p>Foo\n***</p>\n",
                "all",
                true);
        }

        [Fact]
        public void ThematicBreaks_Spec20()
        {
            // Line number in CommonMark Specs: 607
            // Markdown: _____________________________________\n
            // Expected HTML: <hr />\n

            SpecTestHelper.AssertCompliance("_____________________________________\n",
                "<hr />\n",
                "all",
                true);
        }

        [Fact]
        public void ThematicBreaks_Spec21()
        {
            // Line number in CommonMark Specs: 616
            // Markdown:  - - -\n
            // Expected HTML: <hr />\n

            SpecTestHelper.AssertCompliance(" - - -\n",
                "<hr />\n",
                "all",
                true);
        }

        [Fact]
        public void ThematicBreaks_Spec22()
        {
            // Line number in CommonMark Specs: 623
            // Markdown:  **  * ** * ** * **\n
            // Expected HTML: <hr />\n

            SpecTestHelper.AssertCompliance(" **  * ** * ** * **\n",
                "<hr />\n",
                "all",
                true);
        }

        [Fact]
        public void ThematicBreaks_Spec23()
        {
            // Line number in CommonMark Specs: 630
            // Markdown: -     -      -      -\n
            // Expected HTML: <hr />\n

            SpecTestHelper.AssertCompliance("-     -      -      -\n",
                "<hr />\n",
                "all",
                true);
        }

        [Fact]
        public void ThematicBreaks_Spec24()
        {
            // Line number in CommonMark Specs: 639
            // Markdown: - - - -    \n
            // Expected HTML: <hr />\n

            SpecTestHelper.AssertCompliance("- - - -    \n",
                "<hr />\n",
                "all",
                true);
        }

        [Fact]
        public void ThematicBreaks_Spec25()
        {
            // Line number in CommonMark Specs: 648
            // Markdown: _ _ _ _ a\n\na------\n\n---a---\n
            // Expected HTML: <p>_ _ _ _ a</p>\n<p>a------</p>\n<p>---a---</p>\n

            SpecTestHelper.AssertCompliance("_ _ _ _ a\n\na------\n\n---a---\n",
                "<p>_ _ _ _ a</p>\n<p>a------</p>\n<p>---a---</p>\n",
                "all",
                true);
        }

        [Fact]
        public void ThematicBreaks_Spec26()
        {
            // Line number in CommonMark Specs: 664
            // Markdown:  *-*\n
            // Expected HTML: <p><em>-</em></p>\n

            SpecTestHelper.AssertCompliance(" *-*\n",
                "<p><em>-</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void ThematicBreaks_Spec27()
        {
            // Line number in CommonMark Specs: 673
            // Markdown: - foo\n***\n- bar\n
            // Expected HTML: <ul>\n<li>foo</li>\n</ul>\n<hr />\n<ul>\n<li>bar</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("- foo\n***\n- bar\n",
                "<ul>\n<li>foo</li>\n</ul>\n<hr />\n<ul>\n<li>bar</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void ThematicBreaks_Spec28()
        {
            // Line number in CommonMark Specs: 690
            // Markdown: Foo\n***\nbar\n
            // Expected HTML: <p>Foo</p>\n<hr />\n<p>bar</p>\n

            SpecTestHelper.AssertCompliance("Foo\n***\nbar\n",
                "<p>Foo</p>\n<hr />\n<p>bar</p>\n",
                "all",
                true);
        }

        [Fact]
        public void ThematicBreaks_Spec29()
        {
            // Line number in CommonMark Specs: 707
            // Markdown: Foo\n---\nbar\n
            // Expected HTML: <h2>Foo</h2>\n<p>bar</p>\n

            SpecTestHelper.AssertCompliance("Foo\n---\nbar\n",
                "<h2>Foo</h2>\n<p>bar</p>\n",
                "all",
                true);
        }

        [Fact]
        public void ThematicBreaks_Spec30()
        {
            // Line number in CommonMark Specs: 720
            // Markdown: * Foo\n* * *\n* Bar\n
            // Expected HTML: <ul>\n<li>Foo</li>\n</ul>\n<hr />\n<ul>\n<li>Bar</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("* Foo\n* * *\n* Bar\n",
                "<ul>\n<li>Foo</li>\n</ul>\n<hr />\n<ul>\n<li>Bar</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void ThematicBreaks_Spec31()
        {
            // Line number in CommonMark Specs: 737
            // Markdown: - Foo\n- * * *\n
            // Expected HTML: <ul>\n<li>Foo</li>\n<li>\n<hr />\n</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("- Foo\n- * * *\n",
                "<ul>\n<li>Foo</li>\n<li>\n<hr />\n</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void ATXHeadings_Spec32()
        {
            // Line number in CommonMark Specs: 766
            // Markdown: # foo\n## foo\n### foo\n#### foo\n##### foo\n###### foo\n
            // Expected HTML: <h1>foo</h1>\n<h2>foo</h2>\n<h3>foo</h3>\n<h4>foo</h4>\n<h5>foo</h5>\n<h6>foo</h6>\n

            SpecTestHelper.AssertCompliance("# foo\n## foo\n### foo\n#### foo\n##### foo\n###### foo\n",
                "<h1>foo</h1>\n<h2>foo</h2>\n<h3>foo</h3>\n<h4>foo</h4>\n<h5>foo</h5>\n<h6>foo</h6>\n",
                "all",
                true);
        }

        [Fact]
        public void ATXHeadings_Spec33()
        {
            // Line number in CommonMark Specs: 785
            // Markdown: ####### foo\n
            // Expected HTML: <p>####### foo</p>\n

            SpecTestHelper.AssertCompliance("####### foo\n",
                "<p>####### foo</p>\n",
                "all",
                true);
        }

        [Fact]
        public void ATXHeadings_Spec34()
        {
            // Line number in CommonMark Specs: 800
            // Markdown: #5 bolt\n\n#hashtag\n
            // Expected HTML: <p>#5 bolt</p>\n<p>#hashtag</p>\n

            SpecTestHelper.AssertCompliance("#5 bolt\n\n#hashtag\n",
                "<p>#5 bolt</p>\n<p>#hashtag</p>\n",
                "all",
                true);
        }

        [Fact]
        public void ATXHeadings_Spec35()
        {
            // Line number in CommonMark Specs: 812
            // Markdown: \\## foo\n
            // Expected HTML: <p>## foo</p>\n

            SpecTestHelper.AssertCompliance("\\## foo\n",
                "<p>## foo</p>\n",
                "all",
                true);
        }

        [Fact]
        public void ATXHeadings_Spec36()
        {
            // Line number in CommonMark Specs: 821
            // Markdown: # foo *bar* \\*baz\\*\n
            // Expected HTML: <h1>foo <em>bar</em> *baz*</h1>\n

            SpecTestHelper.AssertCompliance("# foo *bar* \\*baz\\*\n",
                "<h1>foo <em>bar</em> *baz*</h1>\n",
                "all",
                true);
        }

        [Fact]
        public void ATXHeadings_Spec37()
        {
            // Line number in CommonMark Specs: 830
            // Markdown: #                  foo                     \n
            // Expected HTML: <h1>foo</h1>\n

            SpecTestHelper.AssertCompliance("#                  foo                     \n",
                "<h1>foo</h1>\n",
                "all",
                true);
        }

        [Fact]
        public void ATXHeadings_Spec38()
        {
            // Line number in CommonMark Specs: 839
            // Markdown:  ### foo\n  ## foo\n   # foo\n
            // Expected HTML: <h3>foo</h3>\n<h2>foo</h2>\n<h1>foo</h1>\n

            SpecTestHelper.AssertCompliance(" ### foo\n  ## foo\n   # foo\n",
                "<h3>foo</h3>\n<h2>foo</h2>\n<h1>foo</h1>\n",
                "all",
                true);
        }

        [Fact]
        public void ATXHeadings_Spec39()
        {
            // Line number in CommonMark Specs: 852
            // Markdown:     # foo\n
            // Expected HTML: <pre><code># foo\n</code></pre>\n

            SpecTestHelper.AssertCompliance("    # foo\n",
                "<pre><code># foo\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void ATXHeadings_Spec40()
        {
            // Line number in CommonMark Specs: 860
            // Markdown: foo\n    # bar\n
            // Expected HTML: <p>foo\n# bar</p>\n

            SpecTestHelper.AssertCompliance("foo\n    # bar\n",
                "<p>foo\n# bar</p>\n",
                "all",
                true);
        }

        [Fact]
        public void ATXHeadings_Spec41()
        {
            // Line number in CommonMark Specs: 871
            // Markdown: ## foo ##\n  ###   bar    ###\n
            // Expected HTML: <h2>foo</h2>\n<h3>bar</h3>\n

            SpecTestHelper.AssertCompliance("## foo ##\n  ###   bar    ###\n",
                "<h2>foo</h2>\n<h3>bar</h3>\n",
                "all",
                true);
        }

        [Fact]
        public void ATXHeadings_Spec42()
        {
            // Line number in CommonMark Specs: 882
            // Markdown: # foo ##################################\n##### foo ##\n
            // Expected HTML: <h1>foo</h1>\n<h5>foo</h5>\n

            SpecTestHelper.AssertCompliance("# foo ##################################\n##### foo ##\n",
                "<h1>foo</h1>\n<h5>foo</h5>\n",
                "all",
                true);
        }

        [Fact]
        public void ATXHeadings_Spec43()
        {
            // Line number in CommonMark Specs: 893
            // Markdown: ### foo ###     \n
            // Expected HTML: <h3>foo</h3>\n

            SpecTestHelper.AssertCompliance("### foo ###     \n",
                "<h3>foo</h3>\n",
                "all",
                true);
        }

        [Fact]
        public void ATXHeadings_Spec44()
        {
            // Line number in CommonMark Specs: 904
            // Markdown: ### foo ### b\n
            // Expected HTML: <h3>foo ### b</h3>\n

            SpecTestHelper.AssertCompliance("### foo ### b\n",
                "<h3>foo ### b</h3>\n",
                "all",
                true);
        }

        [Fact]
        public void ATXHeadings_Spec45()
        {
            // Line number in CommonMark Specs: 913
            // Markdown: # foo#\n
            // Expected HTML: <h1>foo#</h1>\n

            SpecTestHelper.AssertCompliance("# foo#\n",
                "<h1>foo#</h1>\n",
                "all",
                true);
        }

        [Fact]
        public void ATXHeadings_Spec46()
        {
            // Line number in CommonMark Specs: 923
            // Markdown: ### foo \\###\n## foo #\\##\n# foo \\#\n
            // Expected HTML: <h3>foo ###</h3>\n<h2>foo ###</h2>\n<h1>foo #</h1>\n

            SpecTestHelper.AssertCompliance("### foo \\###\n## foo #\\##\n# foo \\#\n",
                "<h3>foo ###</h3>\n<h2>foo ###</h2>\n<h1>foo #</h1>\n",
                "all",
                true);
        }

        [Fact]
        public void ATXHeadings_Spec47()
        {
            // Line number in CommonMark Specs: 937
            // Markdown: ****\n## foo\n****\n
            // Expected HTML: <hr />\n<h2>foo</h2>\n<hr />\n

            SpecTestHelper.AssertCompliance("****\n## foo\n****\n",
                "<hr />\n<h2>foo</h2>\n<hr />\n",
                "all",
                true);
        }

        [Fact]
        public void ATXHeadings_Spec48()
        {
            // Line number in CommonMark Specs: 948
            // Markdown: Foo bar\n# baz\nBar foo\n
            // Expected HTML: <p>Foo bar</p>\n<h1>baz</h1>\n<p>Bar foo</p>\n

            SpecTestHelper.AssertCompliance("Foo bar\n# baz\nBar foo\n",
                "<p>Foo bar</p>\n<h1>baz</h1>\n<p>Bar foo</p>\n",
                "all",
                true);
        }

        [Fact]
        public void ATXHeadings_Spec49()
        {
            // Line number in CommonMark Specs: 961
            // Markdown: ## \n#\n### ###\n
            // Expected HTML: <h2></h2>\n<h1></h1>\n<h3></h3>\n

            SpecTestHelper.AssertCompliance("## \n#\n### ###\n",
                "<h2></h2>\n<h1></h1>\n<h3></h3>\n",
                "all",
                true);
        }

        [Fact]
        public void SetextHeadings_Spec50()
        {
            // Line number in CommonMark Specs: 1004
            // Markdown: Foo *bar*\n=========\n\nFoo *bar*\n---------\n
            // Expected HTML: <h1>Foo <em>bar</em></h1>\n<h2>Foo <em>bar</em></h2>\n

            SpecTestHelper.AssertCompliance("Foo *bar*\n=========\n\nFoo *bar*\n---------\n",
                "<h1>Foo <em>bar</em></h1>\n<h2>Foo <em>bar</em></h2>\n",
                "all",
                true);
        }

        [Fact]
        public void SetextHeadings_Spec51()
        {
            // Line number in CommonMark Specs: 1018
            // Markdown: Foo *bar\nbaz*\n====\n
            // Expected HTML: <h1>Foo <em>bar\nbaz</em></h1>\n

            SpecTestHelper.AssertCompliance("Foo *bar\nbaz*\n====\n",
                "<h1>Foo <em>bar\nbaz</em></h1>\n",
                "all",
                true);
        }

        [Fact]
        public void SetextHeadings_Spec52()
        {
            // Line number in CommonMark Specs: 1030
            // Markdown: Foo\n-------------------------\n\nFoo\n=\n
            // Expected HTML: <h2>Foo</h2>\n<h1>Foo</h1>\n

            SpecTestHelper.AssertCompliance("Foo\n-------------------------\n\nFoo\n=\n",
                "<h2>Foo</h2>\n<h1>Foo</h1>\n",
                "all",
                true);
        }

        [Fact]
        public void SetextHeadings_Spec53()
        {
            // Line number in CommonMark Specs: 1045
            // Markdown:    Foo\n---\n\n  Foo\n-----\n\n  Foo\n  ===\n
            // Expected HTML: <h2>Foo</h2>\n<h2>Foo</h2>\n<h1>Foo</h1>\n

            SpecTestHelper.AssertCompliance("   Foo\n---\n\n  Foo\n-----\n\n  Foo\n  ===\n",
                "<h2>Foo</h2>\n<h2>Foo</h2>\n<h1>Foo</h1>\n",
                "all",
                true);
        }

        [Fact]
        public void SetextHeadings_Spec54()
        {
            // Line number in CommonMark Specs: 1063
            // Markdown:     Foo\n    ---\n\n    Foo\n---\n
            // Expected HTML: <pre><code>Foo\n---\n\nFoo\n</code></pre>\n<hr />\n

            SpecTestHelper.AssertCompliance("    Foo\n    ---\n\n    Foo\n---\n",
                "<pre><code>Foo\n---\n\nFoo\n</code></pre>\n<hr />\n",
                "all",
                true);
        }

        [Fact]
        public void SetextHeadings_Spec55()
        {
            // Line number in CommonMark Specs: 1082
            // Markdown: Foo\n   ----      \n
            // Expected HTML: <h2>Foo</h2>\n

            SpecTestHelper.AssertCompliance("Foo\n   ----      \n",
                "<h2>Foo</h2>\n",
                "all",
                true);
        }

        [Fact]
        public void SetextHeadings_Spec56()
        {
            // Line number in CommonMark Specs: 1092
            // Markdown: Foo\n    ---\n
            // Expected HTML: <p>Foo\n---</p>\n

            SpecTestHelper.AssertCompliance("Foo\n    ---\n",
                "<p>Foo\n---</p>\n",
                "all",
                true);
        }

        [Fact]
        public void SetextHeadings_Spec57()
        {
            // Line number in CommonMark Specs: 1103
            // Markdown: Foo\n= =\n\nFoo\n--- -\n
            // Expected HTML: <p>Foo\n= =</p>\n<p>Foo</p>\n<hr />\n

            SpecTestHelper.AssertCompliance("Foo\n= =\n\nFoo\n--- -\n",
                "<p>Foo\n= =</p>\n<p>Foo</p>\n<hr />\n",
                "all",
                true);
        }

        [Fact]
        public void SetextHeadings_Spec58()
        {
            // Line number in CommonMark Specs: 1119
            // Markdown: Foo  \n-----\n
            // Expected HTML: <h2>Foo</h2>\n

            SpecTestHelper.AssertCompliance("Foo  \n-----\n",
                "<h2>Foo</h2>\n",
                "all",
                true);
        }

        [Fact]
        public void SetextHeadings_Spec59()
        {
            // Line number in CommonMark Specs: 1129
            // Markdown: Foo\\\n----\n
            // Expected HTML: <h2>Foo\\</h2>\n

            SpecTestHelper.AssertCompliance("Foo\\\n----\n",
                "<h2>Foo\\</h2>\n",
                "all",
                true);
        }

        [Fact]
        public void SetextHeadings_Spec60()
        {
            // Line number in CommonMark Specs: 1140
            // Markdown: `Foo\n----\n`\n\n<a title=\"a lot\n---\nof dashes\"/>\n
            // Expected HTML: <h2>`Foo</h2>\n<p>`</p>\n<h2>&lt;a title=&quot;a lot</h2>\n<p>of dashes&quot;/&gt;</p>\n

            SpecTestHelper.AssertCompliance("`Foo\n----\n`\n\n<a title=\"a lot\n---\nof dashes\"/>\n",
                "<h2>`Foo</h2>\n<p>`</p>\n<h2>&lt;a title=&quot;a lot</h2>\n<p>of dashes&quot;/&gt;</p>\n",
                "all",
                true);
        }

        [Fact]
        public void SetextHeadings_Spec61()
        {
            // Line number in CommonMark Specs: 1159
            // Markdown: > Foo\n---\n
            // Expected HTML: <blockquote>\n<p>Foo</p>\n</blockquote>\n<hr />\n

            SpecTestHelper.AssertCompliance("> Foo\n---\n",
                "<blockquote>\n<p>Foo</p>\n</blockquote>\n<hr />\n",
                "all",
                true);
        }

        [Fact]
        public void SetextHeadings_Spec62()
        {
            // Line number in CommonMark Specs: 1170
            // Markdown: > foo\nbar\n===\n
            // Expected HTML: <blockquote>\n<p>foo\nbar\n===</p>\n</blockquote>\n

            SpecTestHelper.AssertCompliance("> foo\nbar\n===\n",
                "<blockquote>\n<p>foo\nbar\n===</p>\n</blockquote>\n",
                "all",
                true);
        }

        [Fact]
        public void SetextHeadings_Spec63()
        {
            // Line number in CommonMark Specs: 1183
            // Markdown: - Foo\n---\n
            // Expected HTML: <ul>\n<li>Foo</li>\n</ul>\n<hr />\n

            SpecTestHelper.AssertCompliance("- Foo\n---\n",
                "<ul>\n<li>Foo</li>\n</ul>\n<hr />\n",
                "all",
                true);
        }

        [Fact]
        public void SetextHeadings_Spec64()
        {
            // Line number in CommonMark Specs: 1198
            // Markdown: Foo\nBar\n---\n
            // Expected HTML: <h2>Foo\nBar</h2>\n

            SpecTestHelper.AssertCompliance("Foo\nBar\n---\n",
                "<h2>Foo\nBar</h2>\n",
                "all",
                true);
        }

        [Fact]
        public void SetextHeadings_Spec65()
        {
            // Line number in CommonMark Specs: 1211
            // Markdown: ---\nFoo\n---\nBar\n---\nBaz\n
            // Expected HTML: <hr />\n<h2>Foo</h2>\n<h2>Bar</h2>\n<p>Baz</p>\n

            SpecTestHelper.AssertCompliance("---\nFoo\n---\nBar\n---\nBaz\n",
                "<hr />\n<h2>Foo</h2>\n<h2>Bar</h2>\n<p>Baz</p>\n",
                "all",
                true);
        }

        [Fact]
        public void SetextHeadings_Spec66()
        {
            // Line number in CommonMark Specs: 1228
            // Markdown: \n====\n
            // Expected HTML: <p>====</p>\n

            SpecTestHelper.AssertCompliance("\n====\n",
                "<p>====</p>\n",
                "all",
                true);
        }

        [Fact]
        public void SetextHeadings_Spec67()
        {
            // Line number in CommonMark Specs: 1240
            // Markdown: ---\n---\n
            // Expected HTML: <hr />\n<hr />\n

            SpecTestHelper.AssertCompliance("---\n---\n",
                "<hr />\n<hr />\n",
                "all",
                true);
        }

        [Fact]
        public void SetextHeadings_Spec68()
        {
            // Line number in CommonMark Specs: 1249
            // Markdown: - foo\n-----\n
            // Expected HTML: <ul>\n<li>foo</li>\n</ul>\n<hr />\n

            SpecTestHelper.AssertCompliance("- foo\n-----\n",
                "<ul>\n<li>foo</li>\n</ul>\n<hr />\n",
                "all",
                true);
        }

        [Fact]
        public void SetextHeadings_Spec69()
        {
            // Line number in CommonMark Specs: 1260
            // Markdown:     foo\n---\n
            // Expected HTML: <pre><code>foo\n</code></pre>\n<hr />\n

            SpecTestHelper.AssertCompliance("    foo\n---\n",
                "<pre><code>foo\n</code></pre>\n<hr />\n",
                "all",
                true);
        }

        [Fact]
        public void SetextHeadings_Spec70()
        {
            // Line number in CommonMark Specs: 1270
            // Markdown: > foo\n-----\n
            // Expected HTML: <blockquote>\n<p>foo</p>\n</blockquote>\n<hr />\n

            SpecTestHelper.AssertCompliance("> foo\n-----\n",
                "<blockquote>\n<p>foo</p>\n</blockquote>\n<hr />\n",
                "all",
                true);
        }

        [Fact]
        public void SetextHeadings_Spec71()
        {
            // Line number in CommonMark Specs: 1284
            // Markdown: \\> foo\n------\n
            // Expected HTML: <h2>&gt; foo</h2>\n

            SpecTestHelper.AssertCompliance("\\> foo\n------\n",
                "<h2>&gt; foo</h2>\n",
                "all",
                true);
        }

        [Fact]
        public void SetextHeadings_Spec72()
        {
            // Line number in CommonMark Specs: 1315
            // Markdown: Foo\n\nbar\n---\nbaz\n
            // Expected HTML: <p>Foo</p>\n<h2>bar</h2>\n<p>baz</p>\n

            SpecTestHelper.AssertCompliance("Foo\n\nbar\n---\nbaz\n",
                "<p>Foo</p>\n<h2>bar</h2>\n<p>baz</p>\n",
                "all",
                true);
        }

        [Fact]
        public void SetextHeadings_Spec73()
        {
            // Line number in CommonMark Specs: 1331
            // Markdown: Foo\nbar\n\n---\n\nbaz\n
            // Expected HTML: <p>Foo\nbar</p>\n<hr />\n<p>baz</p>\n

            SpecTestHelper.AssertCompliance("Foo\nbar\n\n---\n\nbaz\n",
                "<p>Foo\nbar</p>\n<hr />\n<p>baz</p>\n",
                "all",
                true);
        }

        [Fact]
        public void SetextHeadings_Spec74()
        {
            // Line number in CommonMark Specs: 1349
            // Markdown: Foo\nbar\n* * *\nbaz\n
            // Expected HTML: <p>Foo\nbar</p>\n<hr />\n<p>baz</p>\n

            SpecTestHelper.AssertCompliance("Foo\nbar\n* * *\nbaz\n",
                "<p>Foo\nbar</p>\n<hr />\n<p>baz</p>\n",
                "all",
                true);
        }

        [Fact]
        public void SetextHeadings_Spec75()
        {
            // Line number in CommonMark Specs: 1364
            // Markdown: Foo\nbar\n\\---\nbaz\n
            // Expected HTML: <p>Foo\nbar\n---\nbaz</p>\n

            SpecTestHelper.AssertCompliance("Foo\nbar\n\\---\nbaz\n",
                "<p>Foo\nbar\n---\nbaz</p>\n",
                "all",
                true);
        }

        [Fact]
        public void IndentedCodeBlocks_Spec76()
        {
            // Line number in CommonMark Specs: 1392
            // Markdown:     a simple\n      indented code block\n
            // Expected HTML: <pre><code>a simple\n  indented code block\n</code></pre>\n

            SpecTestHelper.AssertCompliance("    a simple\n      indented code block\n",
                "<pre><code>a simple\n  indented code block\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void IndentedCodeBlocks_Spec77()
        {
            // Line number in CommonMark Specs: 1406
            // Markdown:   - foo\n\n    bar\n
            // Expected HTML: <ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("  - foo\n\n    bar\n",
                "<ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void IndentedCodeBlocks_Spec78()
        {
            // Line number in CommonMark Specs: 1420
            // Markdown: 1.  foo\n\n    - bar\n
            // Expected HTML: <ol>\n<li>\n<p>foo</p>\n<ul>\n<li>bar</li>\n</ul>\n</li>\n</ol>\n

            SpecTestHelper.AssertCompliance("1.  foo\n\n    - bar\n",
                "<ol>\n<li>\n<p>foo</p>\n<ul>\n<li>bar</li>\n</ul>\n</li>\n</ol>\n",
                "all",
                true);
        }

        [Fact]
        public void IndentedCodeBlocks_Spec79()
        {
            // Line number in CommonMark Specs: 1440
            // Markdown:     <a/>\n    *hi*\n\n    - one\n
            // Expected HTML: <pre><code>&lt;a/&gt;\n*hi*\n\n- one\n</code></pre>\n

            SpecTestHelper.AssertCompliance("    <a/>\n    *hi*\n\n    - one\n",
                "<pre><code>&lt;a/&gt;\n*hi*\n\n- one\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void IndentedCodeBlocks_Spec80()
        {
            // Line number in CommonMark Specs: 1456
            // Markdown:     chunk1\n\n    chunk2\n  \n \n \n    chunk3\n
            // Expected HTML: <pre><code>chunk1\n\nchunk2\n\n\n\nchunk3\n</code></pre>\n

            SpecTestHelper.AssertCompliance("    chunk1\n\n    chunk2\n  \n \n \n    chunk3\n",
                "<pre><code>chunk1\n\nchunk2\n\n\n\nchunk3\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void IndentedCodeBlocks_Spec81()
        {
            // Line number in CommonMark Specs: 1479
            // Markdown:     chunk1\n      \n      chunk2\n
            // Expected HTML: <pre><code>chunk1\n  \n  chunk2\n</code></pre>\n

            SpecTestHelper.AssertCompliance("    chunk1\n      \n      chunk2\n",
                "<pre><code>chunk1\n  \n  chunk2\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void IndentedCodeBlocks_Spec82()
        {
            // Line number in CommonMark Specs: 1494
            // Markdown: Foo\n    bar\n\n
            // Expected HTML: <p>Foo\nbar</p>\n

            SpecTestHelper.AssertCompliance("Foo\n    bar\n\n",
                "<p>Foo\nbar</p>\n",
                "all",
                true);
        }

        [Fact]
        public void IndentedCodeBlocks_Spec83()
        {
            // Line number in CommonMark Specs: 1508
            // Markdown:     foo\nbar\n
            // Expected HTML: <pre><code>foo\n</code></pre>\n<p>bar</p>\n

            SpecTestHelper.AssertCompliance("    foo\nbar\n",
                "<pre><code>foo\n</code></pre>\n<p>bar</p>\n",
                "all",
                true);
        }

        [Fact]
        public void IndentedCodeBlocks_Spec84()
        {
            // Line number in CommonMark Specs: 1521
            // Markdown: # Heading\n    foo\nHeading\n------\n    foo\n----\n
            // Expected HTML: <h1>Heading</h1>\n<pre><code>foo\n</code></pre>\n<h2>Heading</h2>\n<pre><code>foo\n</code></pre>\n<hr />\n

            SpecTestHelper.AssertCompliance("# Heading\n    foo\nHeading\n------\n    foo\n----\n",
                "<h1>Heading</h1>\n<pre><code>foo\n</code></pre>\n<h2>Heading</h2>\n<pre><code>foo\n</code></pre>\n<hr />\n",
                "all",
                true);
        }

        [Fact]
        public void IndentedCodeBlocks_Spec85()
        {
            // Line number in CommonMark Specs: 1541
            // Markdown:         foo\n    bar\n
            // Expected HTML: <pre><code>    foo\nbar\n</code></pre>\n

            SpecTestHelper.AssertCompliance("        foo\n    bar\n",
                "<pre><code>    foo\nbar\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void IndentedCodeBlocks_Spec86()
        {
            // Line number in CommonMark Specs: 1554
            // Markdown: \n    \n    foo\n    \n\n
            // Expected HTML: <pre><code>foo\n</code></pre>\n

            SpecTestHelper.AssertCompliance("\n    \n    foo\n    \n\n",
                "<pre><code>foo\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void IndentedCodeBlocks_Spec87()
        {
            // Line number in CommonMark Specs: 1568
            // Markdown:     foo  \n
            // Expected HTML: <pre><code>foo  \n</code></pre>\n

            SpecTestHelper.AssertCompliance("    foo  \n",
                "<pre><code>foo  \n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void FencedCodeBlocks_Spec88()
        {
            // Line number in CommonMark Specs: 1623
            // Markdown: ```\n<\n >\n```\n
            // Expected HTML: <pre><code>&lt;\n &gt;\n</code></pre>\n

            SpecTestHelper.AssertCompliance("```\n<\n >\n```\n",
                "<pre><code>&lt;\n &gt;\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void FencedCodeBlocks_Spec89()
        {
            // Line number in CommonMark Specs: 1637
            // Markdown: ~~~\n<\n >\n~~~\n
            // Expected HTML: <pre><code>&lt;\n &gt;\n</code></pre>\n

            SpecTestHelper.AssertCompliance("~~~\n<\n >\n~~~\n",
                "<pre><code>&lt;\n &gt;\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void FencedCodeBlocks_Spec90()
        {
            // Line number in CommonMark Specs: 1650
            // Markdown: ``\nfoo\n``\n
            // Expected HTML: <p><code>foo</code></p>\n

            SpecTestHelper.AssertCompliance("``\nfoo\n``\n",
                "<p><code>foo</code></p>\n",
                "all",
                true);
        }

        [Fact]
        public void FencedCodeBlocks_Spec91()
        {
            // Line number in CommonMark Specs: 1661
            // Markdown: ```\naaa\n~~~\n```\n
            // Expected HTML: <pre><code>aaa\n~~~\n</code></pre>\n

            SpecTestHelper.AssertCompliance("```\naaa\n~~~\n```\n",
                "<pre><code>aaa\n~~~\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void FencedCodeBlocks_Spec92()
        {
            // Line number in CommonMark Specs: 1673
            // Markdown: ~~~\naaa\n```\n~~~\n
            // Expected HTML: <pre><code>aaa\n```\n</code></pre>\n

            SpecTestHelper.AssertCompliance("~~~\naaa\n```\n~~~\n",
                "<pre><code>aaa\n```\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void FencedCodeBlocks_Spec93()
        {
            // Line number in CommonMark Specs: 1687
            // Markdown: ````\naaa\n```\n``````\n
            // Expected HTML: <pre><code>aaa\n```\n</code></pre>\n

            SpecTestHelper.AssertCompliance("````\naaa\n```\n``````\n",
                "<pre><code>aaa\n```\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void FencedCodeBlocks_Spec94()
        {
            // Line number in CommonMark Specs: 1699
            // Markdown: ~~~~\naaa\n~~~\n~~~~\n
            // Expected HTML: <pre><code>aaa\n~~~\n</code></pre>\n

            SpecTestHelper.AssertCompliance("~~~~\naaa\n~~~\n~~~~\n",
                "<pre><code>aaa\n~~~\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void FencedCodeBlocks_Spec95()
        {
            // Line number in CommonMark Specs: 1714
            // Markdown: ```\n
            // Expected HTML: <pre><code></code></pre>\n

            SpecTestHelper.AssertCompliance("```\n",
                "<pre><code></code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void FencedCodeBlocks_Spec96()
        {
            // Line number in CommonMark Specs: 1721
            // Markdown: `````\n\n```\naaa\n
            // Expected HTML: <pre><code>\n```\naaa\n</code></pre>\n

            SpecTestHelper.AssertCompliance("`````\n\n```\naaa\n",
                "<pre><code>\n```\naaa\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void FencedCodeBlocks_Spec97()
        {
            // Line number in CommonMark Specs: 1734
            // Markdown: > ```\n> aaa\n\nbbb\n
            // Expected HTML: <blockquote>\n<pre><code>aaa\n</code></pre>\n</blockquote>\n<p>bbb</p>\n

            SpecTestHelper.AssertCompliance("> ```\n> aaa\n\nbbb\n",
                "<blockquote>\n<pre><code>aaa\n</code></pre>\n</blockquote>\n<p>bbb</p>\n",
                "all",
                true);
        }

        [Fact]
        public void FencedCodeBlocks_Spec98()
        {
            // Line number in CommonMark Specs: 1750
            // Markdown: ```\n\n  \n```\n
            // Expected HTML: <pre><code>\n  \n</code></pre>\n

            SpecTestHelper.AssertCompliance("```\n\n  \n```\n",
                "<pre><code>\n  \n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void FencedCodeBlocks_Spec99()
        {
            // Line number in CommonMark Specs: 1764
            // Markdown: ```\n```\n
            // Expected HTML: <pre><code></code></pre>\n

            SpecTestHelper.AssertCompliance("```\n```\n",
                "<pre><code></code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void FencedCodeBlocks_Spec100()
        {
            // Line number in CommonMark Specs: 1776
            // Markdown:  ```\n aaa\naaa\n```\n
            // Expected HTML: <pre><code>aaa\naaa\n</code></pre>\n

            SpecTestHelper.AssertCompliance(" ```\n aaa\naaa\n```\n",
                "<pre><code>aaa\naaa\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void FencedCodeBlocks_Spec101()
        {
            // Line number in CommonMark Specs: 1788
            // Markdown:   ```\naaa\n  aaa\naaa\n  ```\n
            // Expected HTML: <pre><code>aaa\naaa\naaa\n</code></pre>\n

            SpecTestHelper.AssertCompliance("  ```\naaa\n  aaa\naaa\n  ```\n",
                "<pre><code>aaa\naaa\naaa\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void FencedCodeBlocks_Spec102()
        {
            // Line number in CommonMark Specs: 1802
            // Markdown:    ```\n   aaa\n    aaa\n  aaa\n   ```\n
            // Expected HTML: <pre><code>aaa\n aaa\naaa\n</code></pre>\n

            SpecTestHelper.AssertCompliance("   ```\n   aaa\n    aaa\n  aaa\n   ```\n",
                "<pre><code>aaa\n aaa\naaa\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void FencedCodeBlocks_Spec103()
        {
            // Line number in CommonMark Specs: 1818
            // Markdown:     ```\n    aaa\n    ```\n
            // Expected HTML: <pre><code>```\naaa\n```\n</code></pre>\n

            SpecTestHelper.AssertCompliance("    ```\n    aaa\n    ```\n",
                "<pre><code>```\naaa\n```\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void FencedCodeBlocks_Spec104()
        {
            // Line number in CommonMark Specs: 1833
            // Markdown: ```\naaa\n  ```\n
            // Expected HTML: <pre><code>aaa\n</code></pre>\n

            SpecTestHelper.AssertCompliance("```\naaa\n  ```\n",
                "<pre><code>aaa\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void FencedCodeBlocks_Spec105()
        {
            // Line number in CommonMark Specs: 1843
            // Markdown:    ```\naaa\n  ```\n
            // Expected HTML: <pre><code>aaa\n</code></pre>\n

            SpecTestHelper.AssertCompliance("   ```\naaa\n  ```\n",
                "<pre><code>aaa\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void FencedCodeBlocks_Spec106()
        {
            // Line number in CommonMark Specs: 1855
            // Markdown: ```\naaa\n    ```\n
            // Expected HTML: <pre><code>aaa\n    ```\n</code></pre>\n

            SpecTestHelper.AssertCompliance("```\naaa\n    ```\n",
                "<pre><code>aaa\n    ```\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void FencedCodeBlocks_Spec107()
        {
            // Line number in CommonMark Specs: 1869
            // Markdown: ``` ```\naaa\n
            // Expected HTML: <p><code></code>\naaa</p>\n

            SpecTestHelper.AssertCompliance("``` ```\naaa\n",
                "<p><code></code>\naaa</p>\n",
                "all",
                true);
        }

        [Fact]
        public void FencedCodeBlocks_Spec108()
        {
            // Line number in CommonMark Specs: 1878
            // Markdown: ~~~~~~\naaa\n~~~ ~~\n
            // Expected HTML: <pre><code>aaa\n~~~ ~~\n</code></pre>\n

            SpecTestHelper.AssertCompliance("~~~~~~\naaa\n~~~ ~~\n",
                "<pre><code>aaa\n~~~ ~~\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void FencedCodeBlocks_Spec109()
        {
            // Line number in CommonMark Specs: 1892
            // Markdown: foo\n```\nbar\n```\nbaz\n
            // Expected HTML: <p>foo</p>\n<pre><code>bar\n</code></pre>\n<p>baz</p>\n

            SpecTestHelper.AssertCompliance("foo\n```\nbar\n```\nbaz\n",
                "<p>foo</p>\n<pre><code>bar\n</code></pre>\n<p>baz</p>\n",
                "all",
                true);
        }

        [Fact]
        public void FencedCodeBlocks_Spec110()
        {
            // Line number in CommonMark Specs: 1909
            // Markdown: foo\n---\n~~~\nbar\n~~~\n# baz\n
            // Expected HTML: <h2>foo</h2>\n<pre><code>bar\n</code></pre>\n<h1>baz</h1>\n

            SpecTestHelper.AssertCompliance("foo\n---\n~~~\nbar\n~~~\n# baz\n",
                "<h2>foo</h2>\n<pre><code>bar\n</code></pre>\n<h1>baz</h1>\n",
                "all",
                true);
        }

        [Fact]
        public void FencedCodeBlocks_Spec111()
        {
            // Line number in CommonMark Specs: 1929
            // Markdown: ```ruby\ndef foo(x)\n  return 3\nend\n```\n
            // Expected HTML: <pre><code>def foo(x)\n  return 3\nend\n</code></pre>\n

            SpecTestHelper.AssertCompliance("```ruby\ndef foo(x)\n  return 3\nend\n```\n",
                "<pre><code>def foo(x)\n  return 3\nend\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void FencedCodeBlocks_Spec112()
        {
            // Line number in CommonMark Specs: 1943
            // Markdown: ~~~~    ruby startline=3 $%@#$\ndef foo(x)\n  return 3\nend\n~~~~~~~\n
            // Expected HTML: <pre><code>def foo(x)\n  return 3\nend\n</code></pre>\n

            SpecTestHelper.AssertCompliance("~~~~    ruby startline=3 $%@#$\ndef foo(x)\n  return 3\nend\n~~~~~~~\n",
                "<pre><code>def foo(x)\n  return 3\nend\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void FencedCodeBlocks_Spec113()
        {
            // Line number in CommonMark Specs: 1957
            // Markdown: ````;\n````\n
            // Expected HTML: <pre><code></code></pre>\n

            SpecTestHelper.AssertCompliance("````;\n````\n",
                "<pre><code></code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void FencedCodeBlocks_Spec114()
        {
            // Line number in CommonMark Specs: 1967
            // Markdown: ``` aa ```\nfoo\n
            // Expected HTML: <p><code>aa</code>\nfoo</p>\n

            SpecTestHelper.AssertCompliance("``` aa ```\nfoo\n",
                "<p><code>aa</code>\nfoo</p>\n",
                "all",
                true);
        }

        [Fact]
        public void FencedCodeBlocks_Spec115()
        {
            // Line number in CommonMark Specs: 1978
            // Markdown: ```\n``` aaa\n```\n
            // Expected HTML: <pre><code>``` aaa\n</code></pre>\n

            SpecTestHelper.AssertCompliance("```\n``` aaa\n```\n",
                "<pre><code>``` aaa\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec116()
        {
            // Line number in CommonMark Specs: 2055
            // Markdown: <table><tr><td>\n<pre>\n**Hello**,\n\n_world_.\n</pre>\n</td></tr></table>\n
            // Expected HTML: <table><tr><td>\n<pre>\n**Hello**,\n<p><em>world</em>.\n</pre></p>\n</td></tr></table>\n

            SpecTestHelper.AssertCompliance("<table><tr><td>\n<pre>\n**Hello**,\n\n_world_.\n</pre>\n</td></tr></table>\n",
                "<table><tr><td>\n<pre>\n**Hello**,\n<p><em>world</em>.\n</pre></p>\n</td></tr></table>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec117()
        {
            // Line number in CommonMark Specs: 2084
            // Markdown: <table>\n  <tr>\n    <td>\n           hi\n    </td>\n  </tr>\n</table>\n\nokay.\n
            // Expected HTML: <table>\n  <tr>\n    <td>\n           hi\n    </td>\n  </tr>\n</table>\n<p>okay.</p>\n

            SpecTestHelper.AssertCompliance("<table>\n  <tr>\n    <td>\n           hi\n    </td>\n  </tr>\n</table>\n\nokay.\n",
                "<table>\n  <tr>\n    <td>\n           hi\n    </td>\n  </tr>\n</table>\n<p>okay.</p>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec118()
        {
            // Line number in CommonMark Specs: 2106
            // Markdown:  <div>\n  *hello*\n         <foo><a>\n
            // Expected HTML:  <div>\n  *hello*\n         <foo><a>\n

            SpecTestHelper.AssertCompliance(" <div>\n  *hello*\n         <foo><a>\n",
                " <div>\n  *hello*\n         <foo><a>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec119()
        {
            // Line number in CommonMark Specs: 2119
            // Markdown: </div>\n*foo*\n
            // Expected HTML: </div>\n*foo*\n

            SpecTestHelper.AssertCompliance("</div>\n*foo*\n",
                "</div>\n*foo*\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec120()
        {
            // Line number in CommonMark Specs: 2130
            // Markdown: <DIV CLASS=\"foo\">\n\n*Markdown*\n\n</DIV>\n
            // Expected HTML: <DIV CLASS=\"foo\">\n<p><em>Markdown</em></p>\n</DIV>\n

            SpecTestHelper.AssertCompliance("<DIV CLASS=\"foo\">\n\n*Markdown*\n\n</DIV>\n",
                "<DIV CLASS=\"foo\">\n<p><em>Markdown</em></p>\n</DIV>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec121()
        {
            // Line number in CommonMark Specs: 2146
            // Markdown: <div id=\"foo\"\n  class=\"bar\">\n</div>\n
            // Expected HTML: <div id=\"foo\"\n  class=\"bar\">\n</div>\n

            SpecTestHelper.AssertCompliance("<div id=\"foo\"\n  class=\"bar\">\n</div>\n",
                "<div id=\"foo\"\n  class=\"bar\">\n</div>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec122()
        {
            // Line number in CommonMark Specs: 2157
            // Markdown: <div id=\"foo\" class=\"bar\n  baz\">\n</div>\n
            // Expected HTML: <div id=\"foo\" class=\"bar\n  baz\">\n</div>\n

            SpecTestHelper.AssertCompliance("<div id=\"foo\" class=\"bar\n  baz\">\n</div>\n",
                "<div id=\"foo\" class=\"bar\n  baz\">\n</div>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec123()
        {
            // Line number in CommonMark Specs: 2169
            // Markdown: <div>\n*foo*\n\n*bar*\n
            // Expected HTML: <div>\n*foo*\n<p><em>bar</em></p>\n

            SpecTestHelper.AssertCompliance("<div>\n*foo*\n\n*bar*\n",
                "<div>\n*foo*\n<p><em>bar</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec124()
        {
            // Line number in CommonMark Specs: 2185
            // Markdown: <div id=\"foo\"\n*hi*\n
            // Expected HTML: <div id=\"foo\"\n*hi*\n

            SpecTestHelper.AssertCompliance("<div id=\"foo\"\n*hi*\n",
                "<div id=\"foo\"\n*hi*\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec125()
        {
            // Line number in CommonMark Specs: 2194
            // Markdown: <div class\nfoo\n
            // Expected HTML: <div class\nfoo\n

            SpecTestHelper.AssertCompliance("<div class\nfoo\n",
                "<div class\nfoo\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec126()
        {
            // Line number in CommonMark Specs: 2206
            // Markdown: <div *???-&&&-<---\n*foo*\n
            // Expected HTML: <div *???-&&&-<---\n*foo*\n

            SpecTestHelper.AssertCompliance("<div *???-&&&-<---\n*foo*\n",
                "<div *???-&&&-<---\n*foo*\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec127()
        {
            // Line number in CommonMark Specs: 2218
            // Markdown: <div><a href=\"bar\">*foo*</a></div>\n
            // Expected HTML: <div><a href=\"bar\">*foo*</a></div>\n

            SpecTestHelper.AssertCompliance("<div><a href=\"bar\">*foo*</a></div>\n",
                "<div><a href=\"bar\">*foo*</a></div>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec128()
        {
            // Line number in CommonMark Specs: 2225
            // Markdown: <table><tr><td>\nfoo\n</td></tr></table>\n
            // Expected HTML: <table><tr><td>\nfoo\n</td></tr></table>\n

            SpecTestHelper.AssertCompliance("<table><tr><td>\nfoo\n</td></tr></table>\n",
                "<table><tr><td>\nfoo\n</td></tr></table>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec129()
        {
            // Line number in CommonMark Specs: 2242
            // Markdown: <div></div>\n``` c\nint x = 33;\n```\n
            // Expected HTML: <div></div>\n``` c\nint x = 33;\n```\n

            SpecTestHelper.AssertCompliance("<div></div>\n``` c\nint x = 33;\n```\n",
                "<div></div>\n``` c\nint x = 33;\n```\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec130()
        {
            // Line number in CommonMark Specs: 2259
            // Markdown: <a href=\"foo\">\n*bar*\n</a>\n
            // Expected HTML: <a href=\"foo\">\n*bar*\n</a>\n

            SpecTestHelper.AssertCompliance("<a href=\"foo\">\n*bar*\n</a>\n",
                "<a href=\"foo\">\n*bar*\n</a>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec131()
        {
            // Line number in CommonMark Specs: 2272
            // Markdown: <Warning>\n*bar*\n</Warning>\n
            // Expected HTML: <Warning>\n*bar*\n</Warning>\n

            SpecTestHelper.AssertCompliance("<Warning>\n*bar*\n</Warning>\n",
                "<Warning>\n*bar*\n</Warning>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec132()
        {
            // Line number in CommonMark Specs: 2283
            // Markdown: <i class=\"foo\">\n*bar*\n</i>\n
            // Expected HTML: <i class=\"foo\">\n*bar*\n</i>\n

            SpecTestHelper.AssertCompliance("<i class=\"foo\">\n*bar*\n</i>\n",
                "<i class=\"foo\">\n*bar*\n</i>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec133()
        {
            // Line number in CommonMark Specs: 2294
            // Markdown: </ins>\n*bar*\n
            // Expected HTML: </ins>\n*bar*\n

            SpecTestHelper.AssertCompliance("</ins>\n*bar*\n",
                "</ins>\n*bar*\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec134()
        {
            // Line number in CommonMark Specs: 2309
            // Markdown: <del>\n*foo*\n</del>\n
            // Expected HTML: <del>\n*foo*\n</del>\n

            SpecTestHelper.AssertCompliance("<del>\n*foo*\n</del>\n",
                "<del>\n*foo*\n</del>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec135()
        {
            // Line number in CommonMark Specs: 2324
            // Markdown: <del>\n\n*foo*\n\n</del>\n
            // Expected HTML: <del>\n<p><em>foo</em></p>\n</del>\n

            SpecTestHelper.AssertCompliance("<del>\n\n*foo*\n\n</del>\n",
                "<del>\n<p><em>foo</em></p>\n</del>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec136()
        {
            // Line number in CommonMark Specs: 2342
            // Markdown: <del>*foo*</del>\n
            // Expected HTML: <p><del><em>foo</em></del></p>\n

            SpecTestHelper.AssertCompliance("<del>*foo*</del>\n",
                "<p><del><em>foo</em></del></p>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec137()
        {
            // Line number in CommonMark Specs: 2358
            // Markdown: <pre language=\"haskell\"><code>\nimport Text.HTML.TagSoup\n\nmain :: IO ()\nmain = print $ parseTags tags\n</code></pre>\nokay\n
            // Expected HTML: <pre language=\"haskell\"><code>\nimport Text.HTML.TagSoup\n\nmain :: IO ()\nmain = print $ parseTags tags\n</code></pre>\n<p>okay</p>\n

            SpecTestHelper.AssertCompliance("<pre language=\"haskell\"><code>\nimport Text.HTML.TagSoup\n\nmain :: IO ()\nmain = print $ parseTags tags\n</code></pre>\nokay\n",
                "<pre language=\"haskell\"><code>\nimport Text.HTML.TagSoup\n\nmain :: IO ()\nmain = print $ parseTags tags\n</code></pre>\n<p>okay</p>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec138()
        {
            // Line number in CommonMark Specs: 2379
            // Markdown: <script type=\"text/javascript\">\n// JavaScript example\n\ndocument.getElementById(\"demo\").innerHTML = \"Hello JavaScript!\";\n</script>\nokay\n
            // Expected HTML: <script type=\"text/javascript\">\n// JavaScript example\n\ndocument.getElementById(\"demo\").innerHTML = \"Hello JavaScript!\";\n</script>\n<p>okay</p>\n

            SpecTestHelper.AssertCompliance("<script type=\"text/javascript\">\n// JavaScript example\n\ndocument.getElementById(\"demo\").innerHTML = \"Hello JavaScript!\";\n</script>\nokay\n",
                "<script type=\"text/javascript\">\n// JavaScript example\n\ndocument.getElementById(\"demo\").innerHTML = \"Hello JavaScript!\";\n</script>\n<p>okay</p>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec139()
        {
            // Line number in CommonMark Specs: 2398
            // Markdown: <style\n  type=\"text/css\">\nh1 {color:red;}\n\np {color:blue;}\n</style>\nokay\n
            // Expected HTML: <style\n  type=\"text/css\">\nh1 {color:red;}\n\np {color:blue;}\n</style>\n<p>okay</p>\n

            SpecTestHelper.AssertCompliance("<style\n  type=\"text/css\">\nh1 {color:red;}\n\np {color:blue;}\n</style>\nokay\n",
                "<style\n  type=\"text/css\">\nh1 {color:red;}\n\np {color:blue;}\n</style>\n<p>okay</p>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec140()
        {
            // Line number in CommonMark Specs: 2421
            // Markdown: <style\n  type=\"text/css\">\n\nfoo\n
            // Expected HTML: <style\n  type=\"text/css\">\n\nfoo\n

            SpecTestHelper.AssertCompliance("<style\n  type=\"text/css\">\n\nfoo\n",
                "<style\n  type=\"text/css\">\n\nfoo\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec141()
        {
            // Line number in CommonMark Specs: 2434
            // Markdown: > <div>\n> foo\n\nbar\n
            // Expected HTML: <blockquote>\n<div>\nfoo\n</blockquote>\n<p>bar</p>\n

            SpecTestHelper.AssertCompliance("> <div>\n> foo\n\nbar\n",
                "<blockquote>\n<div>\nfoo\n</blockquote>\n<p>bar</p>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec142()
        {
            // Line number in CommonMark Specs: 2448
            // Markdown: - <div>\n- foo\n
            // Expected HTML: <ul>\n<li>\n<div>\n</li>\n<li>foo</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("- <div>\n- foo\n",
                "<ul>\n<li>\n<div>\n</li>\n<li>foo</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec143()
        {
            // Line number in CommonMark Specs: 2463
            // Markdown: <style>p{color:red;}</style>\n*foo*\n
            // Expected HTML: <style>p{color:red;}</style>\n<p><em>foo</em></p>\n

            SpecTestHelper.AssertCompliance("<style>p{color:red;}</style>\n*foo*\n",
                "<style>p{color:red;}</style>\n<p><em>foo</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec144()
        {
            // Line number in CommonMark Specs: 2472
            // Markdown: <!-- foo -->*bar*\n*baz*\n
            // Expected HTML: <!-- foo -->*bar*\n<p><em>baz</em></p>\n

            SpecTestHelper.AssertCompliance("<!-- foo -->*bar*\n*baz*\n",
                "<!-- foo -->*bar*\n<p><em>baz</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec145()
        {
            // Line number in CommonMark Specs: 2484
            // Markdown: <script>\nfoo\n</script>1. *bar*\n
            // Expected HTML: <script>\nfoo\n</script>1. *bar*\n

            SpecTestHelper.AssertCompliance("<script>\nfoo\n</script>1. *bar*\n",
                "<script>\nfoo\n</script>1. *bar*\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec146()
        {
            // Line number in CommonMark Specs: 2497
            // Markdown: <!-- Foo\n\nbar\n   baz -->\nokay\n
            // Expected HTML: <!-- Foo\n\nbar\n   baz -->\n<p>okay</p>\n

            SpecTestHelper.AssertCompliance("<!-- Foo\n\nbar\n   baz -->\nokay\n",
                "<!-- Foo\n\nbar\n   baz -->\n<p>okay</p>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec147()
        {
            // Line number in CommonMark Specs: 2515
            // Markdown: <?php\n\n  echo '>';\n\n?>\nokay\n
            // Expected HTML: <?php\n\n  echo '>';\n\n?>\n<p>okay</p>\n

            SpecTestHelper.AssertCompliance("<?php\n\n  echo '>';\n\n?>\nokay\n",
                "<?php\n\n  echo '>';\n\n?>\n<p>okay</p>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec148()
        {
            // Line number in CommonMark Specs: 2534
            // Markdown: <!DOCTYPE html>\n
            // Expected HTML: <!DOCTYPE html>\n

            SpecTestHelper.AssertCompliance("<!DOCTYPE html>\n",
                "<!DOCTYPE html>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec149()
        {
            // Line number in CommonMark Specs: 2543
            // Markdown: <![CDATA[\nfunction matchwo(a,b)\n{\n  if (a < b && a < 0) then {\n    return 1;\n\n  } else {\n\n    return 0;\n  }\n}\n]]>\nokay\n
            // Expected HTML: <![CDATA[\nfunction matchwo(a,b)\n{\n  if (a < b && a < 0) then {\n    return 1;\n\n  } else {\n\n    return 0;\n  }\n}\n]]>\n<p>okay</p>\n

            SpecTestHelper.AssertCompliance("<![CDATA[\nfunction matchwo(a,b)\n{\n  if (a < b && a < 0) then {\n    return 1;\n\n  } else {\n\n    return 0;\n  }\n}\n]]>\nokay\n",
                "<![CDATA[\nfunction matchwo(a,b)\n{\n  if (a < b && a < 0) then {\n    return 1;\n\n  } else {\n\n    return 0;\n  }\n}\n]]>\n<p>okay</p>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec150()
        {
            // Line number in CommonMark Specs: 2576
            // Markdown:   <!-- foo -->\n\n    <!-- foo -->\n
            // Expected HTML:   <!-- foo -->\n<pre><code>&lt;!-- foo --&gt;\n</code></pre>\n

            SpecTestHelper.AssertCompliance("  <!-- foo -->\n\n    <!-- foo -->\n",
                "  <!-- foo -->\n<pre><code>&lt;!-- foo --&gt;\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec151()
        {
            // Line number in CommonMark Specs: 2587
            // Markdown:   <div>\n\n    <div>\n
            // Expected HTML:   <div>\n<pre><code>&lt;div&gt;\n</code></pre>\n

            SpecTestHelper.AssertCompliance("  <div>\n\n    <div>\n",
                "  <div>\n<pre><code>&lt;div&gt;\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec152()
        {
            // Line number in CommonMark Specs: 2601
            // Markdown: Foo\n<div>\nbar\n</div>\n
            // Expected HTML: <p>Foo</p>\n<div>\nbar\n</div>\n

            SpecTestHelper.AssertCompliance("Foo\n<div>\nbar\n</div>\n",
                "<p>Foo</p>\n<div>\nbar\n</div>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec153()
        {
            // Line number in CommonMark Specs: 2617
            // Markdown: <div>\nbar\n</div>\n*foo*\n
            // Expected HTML: <div>\nbar\n</div>\n*foo*\n

            SpecTestHelper.AssertCompliance("<div>\nbar\n</div>\n*foo*\n",
                "<div>\nbar\n</div>\n*foo*\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec154()
        {
            // Line number in CommonMark Specs: 2632
            // Markdown: Foo\n<a href=\"bar\">\nbaz\n
            // Expected HTML: <p>Foo\n<a href=\"bar\">\nbaz</p>\n

            SpecTestHelper.AssertCompliance("Foo\n<a href=\"bar\">\nbaz\n",
                "<p>Foo\n<a href=\"bar\">\nbaz</p>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec155()
        {
            // Line number in CommonMark Specs: 2673
            // Markdown: <div>\n\n*Emphasized* text.\n\n</div>\n
            // Expected HTML: <div>\n<p><em>Emphasized</em> text.</p>\n</div>\n

            SpecTestHelper.AssertCompliance("<div>\n\n*Emphasized* text.\n\n</div>\n",
                "<div>\n<p><em>Emphasized</em> text.</p>\n</div>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec156()
        {
            // Line number in CommonMark Specs: 2686
            // Markdown: <div>\n*Emphasized* text.\n</div>\n
            // Expected HTML: <div>\n*Emphasized* text.\n</div>\n

            SpecTestHelper.AssertCompliance("<div>\n*Emphasized* text.\n</div>\n",
                "<div>\n*Emphasized* text.\n</div>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec157()
        {
            // Line number in CommonMark Specs: 2708
            // Markdown: <table>\n\n<tr>\n\n<td>\nHi\n</td>\n\n</tr>\n\n</table>\n
            // Expected HTML: <table>\n<tr>\n<td>\nHi\n</td>\n</tr>\n</table>\n

            SpecTestHelper.AssertCompliance("<table>\n\n<tr>\n\n<td>\nHi\n</td>\n\n</tr>\n\n</table>\n",
                "<table>\n<tr>\n<td>\nHi\n</td>\n</tr>\n</table>\n",
                "all",
                true);
        }

        [Fact]
        public void HTMLBlocks_Spec158()
        {
            // Line number in CommonMark Specs: 2735
            // Markdown: <table>\n\n  <tr>\n\n    <td>\n      Hi\n    </td>\n\n  </tr>\n\n</table>\n
            // Expected HTML: <table>\n  <tr>\n<pre><code>&lt;td&gt;\n  Hi\n&lt;/td&gt;\n</code></pre>\n  </tr>\n</table>\n

            SpecTestHelper.AssertCompliance("<table>\n\n  <tr>\n\n    <td>\n      Hi\n    </td>\n\n  </tr>\n\n</table>\n",
                "<table>\n  <tr>\n<pre><code>&lt;td&gt;\n  Hi\n&lt;/td&gt;\n</code></pre>\n  </tr>\n</table>\n",
                "all",
                true);
        }

        [Fact]
        public void LinkReferenceDefinitions_Spec159()
        {
            // Line number in CommonMark Specs: 2783
            // Markdown: [foo]: /url \"title\"\n\n[foo]\n
            // Expected HTML: <p><a href=\"/url\" title=\"title\">foo</a></p>\n

            SpecTestHelper.AssertCompliance("[foo]: /url \"title\"\n\n[foo]\n",
                "<p><a href=\"/url\" title=\"title\">foo</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void LinkReferenceDefinitions_Spec160()
        {
            // Line number in CommonMark Specs: 2792
            // Markdown:    [foo]: \n      /url  \n           'the title'  \n\n[foo]\n
            // Expected HTML: <p><a href=\"/url\" title=\"the title\">foo</a></p>\n

            SpecTestHelper.AssertCompliance("   [foo]: \n      /url  \n           'the title'  \n\n[foo]\n",
                "<p><a href=\"/url\" title=\"the title\">foo</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void LinkReferenceDefinitions_Spec161()
        {
            // Line number in CommonMark Specs: 2803
            // Markdown: [Foo*bar\\]]:my_(url) 'title (with parens)'\n\n[Foo*bar\\]]\n
            // Expected HTML: <p><a href=\"my_(url)\" title=\"title (with parens)\">Foo*bar]</a></p>\n

            SpecTestHelper.AssertCompliance("[Foo*bar\\]]:my_(url) 'title (with parens)'\n\n[Foo*bar\\]]\n",
                "<p><a href=\"my_(url)\" title=\"title (with parens)\">Foo*bar]</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void LinkReferenceDefinitions_Spec162()
        {
            // Line number in CommonMark Specs: 2812
            // Markdown: [Foo bar]:\n<my%20url>\n'title'\n\n[Foo bar]\n
            // Expected HTML: <p><a href=\"my%20url\" title=\"title\">Foo bar</a></p>\n

            SpecTestHelper.AssertCompliance("[Foo bar]:\n<my%20url>\n'title'\n\n[Foo bar]\n",
                "<p><a href=\"my%20url\" title=\"title\">Foo bar</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void LinkReferenceDefinitions_Spec163()
        {
            // Line number in CommonMark Specs: 2825
            // Markdown: [foo]: /url '\ntitle\nline1\nline2\n'\n\n[foo]\n
            // Expected HTML: <p><a href=\"/url\" title=\"\ntitle\nline1\nline2\n\">foo</a></p>\n

            SpecTestHelper.AssertCompliance("[foo]: /url '\ntitle\nline1\nline2\n'\n\n[foo]\n",
                "<p><a href=\"/url\" title=\"\ntitle\nline1\nline2\n\">foo</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void LinkReferenceDefinitions_Spec164()
        {
            // Line number in CommonMark Specs: 2844
            // Markdown: [foo]: /url 'title\n\nwith blank line'\n\n[foo]\n
            // Expected HTML: <p>[foo]: /url 'title</p>\n<p>with blank line'</p>\n<p>[foo]</p>\n

            SpecTestHelper.AssertCompliance("[foo]: /url 'title\n\nwith blank line'\n\n[foo]\n",
                "<p>[foo]: /url 'title</p>\n<p>with blank line'</p>\n<p>[foo]</p>\n",
                "all",
                true);
        }

        [Fact]
        public void LinkReferenceDefinitions_Spec165()
        {
            // Line number in CommonMark Specs: 2859
            // Markdown: [foo]:\n/url\n\n[foo]\n
            // Expected HTML: <p><a href=\"/url\">foo</a></p>\n

            SpecTestHelper.AssertCompliance("[foo]:\n/url\n\n[foo]\n",
                "<p><a href=\"/url\">foo</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void LinkReferenceDefinitions_Spec166()
        {
            // Line number in CommonMark Specs: 2871
            // Markdown: [foo]:\n\n[foo]\n
            // Expected HTML: <p>[foo]:</p>\n<p>[foo]</p>\n

            SpecTestHelper.AssertCompliance("[foo]:\n\n[foo]\n",
                "<p>[foo]:</p>\n<p>[foo]</p>\n",
                "all",
                true);
        }

        [Fact]
        public void LinkReferenceDefinitions_Spec167()
        {
            // Line number in CommonMark Specs: 2884
            // Markdown: [foo]: /url\\bar\\*baz \"foo\\\"bar\\baz\"\n\n[foo]\n
            // Expected HTML: <p><a href=\"/url%5Cbar*baz\" title=\"foo&quot;bar\\baz\">foo</a></p>\n

            SpecTestHelper.AssertCompliance("[foo]: /url\\bar\\*baz \"foo\\\"bar\\baz\"\n\n[foo]\n",
                "<p><a href=\"/url%5Cbar*baz\" title=\"foo&quot;bar\\baz\">foo</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void LinkReferenceDefinitions_Spec168()
        {
            // Line number in CommonMark Specs: 2895
            // Markdown: [foo]\n\n[foo]: url\n
            // Expected HTML: <p><a href=\"url\">foo</a></p>\n

            SpecTestHelper.AssertCompliance("[foo]\n\n[foo]: url\n",
                "<p><a href=\"url\">foo</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void LinkReferenceDefinitions_Spec169()
        {
            // Line number in CommonMark Specs: 2907
            // Markdown: [foo]\n\n[foo]: first\n[foo]: second\n
            // Expected HTML: <p><a href=\"first\">foo</a></p>\n

            SpecTestHelper.AssertCompliance("[foo]\n\n[foo]: first\n[foo]: second\n",
                "<p><a href=\"first\">foo</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void LinkReferenceDefinitions_Spec170()
        {
            // Line number in CommonMark Specs: 2920
            // Markdown: [FOO]: /url\n\n[Foo]\n
            // Expected HTML: <p><a href=\"/url\">Foo</a></p>\n

            SpecTestHelper.AssertCompliance("[FOO]: /url\n\n[Foo]\n",
                "<p><a href=\"/url\">Foo</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void LinkReferenceDefinitions_Spec171()
        {
            // Line number in CommonMark Specs: 2929
            // Markdown: [ΑΓΩ]: /φου\n\n[αγω]\n
            // Expected HTML: <p><a href=\"/%CF%86%CE%BF%CF%85\">αγω</a></p>\n

            SpecTestHelper.AssertCompliance("[ΑΓΩ]: /φου\n\n[αγω]\n",
                "<p><a href=\"/%CF%86%CE%BF%CF%85\">αγω</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void LinkReferenceDefinitions_Spec173()
        {
            // Line number in CommonMark Specs: 2949
            // Markdown: [\nfoo\n]: /url\nbar\n
            // Expected HTML: <p>bar</p>\n

            SpecTestHelper.AssertCompliance("[\nfoo\n]: /url\nbar\n",
                "<p>bar</p>\n",
                "all",
                true);
        }

        [Fact]
        public void LinkReferenceDefinitions_Spec174()
        {
            // Line number in CommonMark Specs: 2962
            // Markdown: [foo]: /url \"title\" ok\n
            // Expected HTML: <p>[foo]: /url &quot;title&quot; ok</p>\n

            SpecTestHelper.AssertCompliance("[foo]: /url \"title\" ok\n",
                "<p>[foo]: /url &quot;title&quot; ok</p>\n",
                "all",
                true);
        }

        [Fact]
        public void LinkReferenceDefinitions_Spec175()
        {
            // Line number in CommonMark Specs: 2971
            // Markdown: [foo]: /url\n\"title\" ok\n
            // Expected HTML: <p>&quot;title&quot; ok</p>\n

            SpecTestHelper.AssertCompliance("[foo]: /url\n\"title\" ok\n",
                "<p>&quot;title&quot; ok</p>\n",
                "all",
                true);
        }

        [Fact]
        public void LinkReferenceDefinitions_Spec176()
        {
            // Line number in CommonMark Specs: 2982
            // Markdown:     [foo]: /url \"title\"\n\n[foo]\n
            // Expected HTML: <pre><code>[foo]: /url &quot;title&quot;\n</code></pre>\n<p>[foo]</p>\n

            SpecTestHelper.AssertCompliance("    [foo]: /url \"title\"\n\n[foo]\n",
                "<pre><code>[foo]: /url &quot;title&quot;\n</code></pre>\n<p>[foo]</p>\n",
                "all",
                true);
        }

        [Fact]
        public void LinkReferenceDefinitions_Spec177()
        {
            // Line number in CommonMark Specs: 2996
            // Markdown: ```\n[foo]: /url\n```\n\n[foo]\n
            // Expected HTML: <pre><code>[foo]: /url\n</code></pre>\n<p>[foo]</p>\n

            SpecTestHelper.AssertCompliance("```\n[foo]: /url\n```\n\n[foo]\n",
                "<pre><code>[foo]: /url\n</code></pre>\n<p>[foo]</p>\n",
                "all",
                true);
        }

        [Fact]
        public void LinkReferenceDefinitions_Spec178()
        {
            // Line number in CommonMark Specs: 3011
            // Markdown: Foo\n[bar]: /baz\n\n[bar]\n
            // Expected HTML: <p>Foo\n[bar]: /baz</p>\n<p>[bar]</p>\n

            SpecTestHelper.AssertCompliance("Foo\n[bar]: /baz\n\n[bar]\n",
                "<p>Foo\n[bar]: /baz</p>\n<p>[bar]</p>\n",
                "all",
                true);
        }

        [Fact]
        public void LinkReferenceDefinitions_Spec179()
        {
            // Line number in CommonMark Specs: 3026
            // Markdown: # [Foo]\n[foo]: /url\n> bar\n
            // Expected HTML: <h1><a href=\"/url\">Foo</a></h1>\n<blockquote>\n<p>bar</p>\n</blockquote>\n

            SpecTestHelper.AssertCompliance("# [Foo]\n[foo]: /url\n> bar\n",
                "<h1><a href=\"/url\">Foo</a></h1>\n<blockquote>\n<p>bar</p>\n</blockquote>\n",
                "all",
                true);
        }

        [Fact]
        public void LinkReferenceDefinitions_Spec180()
        {
            // Line number in CommonMark Specs: 3041
            // Markdown: [foo]: /foo-url \"foo\"\n[bar]: /bar-url\n  \"bar\"\n[baz]: /baz-url\n\n[foo],\n[bar],\n[baz]\n
            // Expected HTML: <p><a href=\"/foo-url\" title=\"foo\">foo</a>,\n<a href=\"/bar-url\" title=\"bar\">bar</a>,\n<a href=\"/baz-url\">baz</a></p>\n

            SpecTestHelper.AssertCompliance("[foo]: /foo-url \"foo\"\n[bar]: /bar-url\n  \"bar\"\n[baz]: /baz-url\n\n[foo],\n[bar],\n[baz]\n",
                "<p><a href=\"/foo-url\" title=\"foo\">foo</a>,\n<a href=\"/bar-url\" title=\"bar\">bar</a>,\n<a href=\"/baz-url\">baz</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void LinkReferenceDefinitions_Spec181()
        {
            // Line number in CommonMark Specs: 3062
            // Markdown: [foo]\n\n> [foo]: /url\n
            // Expected HTML: <p><a href=\"/url\">foo</a></p>\n<blockquote>\n</blockquote>\n

            SpecTestHelper.AssertCompliance("[foo]\n\n> [foo]: /url\n",
                "<p><a href=\"/url\">foo</a></p>\n<blockquote>\n</blockquote>\n",
                "all",
                true);
        }

        [Fact]
        public void Paragraphs_Spec182()
        {
            // Line number in CommonMark Specs: 3085
            // Markdown: aaa\n\nbbb\n
            // Expected HTML: <p>aaa</p>\n<p>bbb</p>\n

            SpecTestHelper.AssertCompliance("aaa\n\nbbb\n",
                "<p>aaa</p>\n<p>bbb</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Paragraphs_Spec183()
        {
            // Line number in CommonMark Specs: 3097
            // Markdown: aaa\nbbb\n\nccc\nddd\n
            // Expected HTML: <p>aaa\nbbb</p>\n<p>ccc\nddd</p>\n

            SpecTestHelper.AssertCompliance("aaa\nbbb\n\nccc\nddd\n",
                "<p>aaa\nbbb</p>\n<p>ccc\nddd</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Paragraphs_Spec184()
        {
            // Line number in CommonMark Specs: 3113
            // Markdown: aaa\n\n\nbbb\n
            // Expected HTML: <p>aaa</p>\n<p>bbb</p>\n

            SpecTestHelper.AssertCompliance("aaa\n\n\nbbb\n",
                "<p>aaa</p>\n<p>bbb</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Paragraphs_Spec185()
        {
            // Line number in CommonMark Specs: 3126
            // Markdown:   aaa\n bbb\n
            // Expected HTML: <p>aaa\nbbb</p>\n

            SpecTestHelper.AssertCompliance("  aaa\n bbb\n",
                "<p>aaa\nbbb</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Paragraphs_Spec186()
        {
            // Line number in CommonMark Specs: 3138
            // Markdown: aaa\n             bbb\n                                       ccc\n
            // Expected HTML: <p>aaa\nbbb\nccc</p>\n

            SpecTestHelper.AssertCompliance("aaa\n             bbb\n                                       ccc\n",
                "<p>aaa\nbbb\nccc</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Paragraphs_Spec187()
        {
            // Line number in CommonMark Specs: 3152
            // Markdown:    aaa\nbbb\n
            // Expected HTML: <p>aaa\nbbb</p>\n

            SpecTestHelper.AssertCompliance("   aaa\nbbb\n",
                "<p>aaa\nbbb</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Paragraphs_Spec188()
        {
            // Line number in CommonMark Specs: 3161
            // Markdown:     aaa\nbbb\n
            // Expected HTML: <pre><code>aaa\n</code></pre>\n<p>bbb</p>\n

            SpecTestHelper.AssertCompliance("    aaa\nbbb\n",
                "<pre><code>aaa\n</code></pre>\n<p>bbb</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Paragraphs_Spec189()
        {
            // Line number in CommonMark Specs: 3175
            // Markdown: aaa     \nbbb     \n
            // Expected HTML: <p>aaa<br />\nbbb</p>\n

            SpecTestHelper.AssertCompliance("aaa     \nbbb     \n",
                "<p>aaa<br />\nbbb</p>\n",
                "all",
                true);
        }

        [Fact]
        public void BlankLines_Spec190()
        {
            // Line number in CommonMark Specs: 3192
            // Markdown:   \n\naaa\n  \n\n# aaa\n\n  \n
            // Expected HTML: <p>aaa</p>\n<h1>aaa</h1>\n

            SpecTestHelper.AssertCompliance("  \n\naaa\n  \n\n# aaa\n\n  \n",
                "<p>aaa</p>\n<h1>aaa</h1>\n",
                "all",
                true);
        }

        [Fact]
        public void BlockQuotes_Spec191()
        {
            // Line number in CommonMark Specs: 3258
            // Markdown: > # Foo\n> bar\n> baz\n
            // Expected HTML: <blockquote>\n<h1>Foo</h1>\n<p>bar\nbaz</p>\n</blockquote>\n

            SpecTestHelper.AssertCompliance("> # Foo\n> bar\n> baz\n",
                "<blockquote>\n<h1>Foo</h1>\n<p>bar\nbaz</p>\n</blockquote>\n",
                "all",
                true);
        }

        [Fact]
        public void BlockQuotes_Spec192()
        {
            // Line number in CommonMark Specs: 3273
            // Markdown: ># Foo\n>bar\n> baz\n
            // Expected HTML: <blockquote>\n<h1>Foo</h1>\n<p>bar\nbaz</p>\n</blockquote>\n

            SpecTestHelper.AssertCompliance("># Foo\n>bar\n> baz\n",
                "<blockquote>\n<h1>Foo</h1>\n<p>bar\nbaz</p>\n</blockquote>\n",
                "all",
                true);
        }

        [Fact]
        public void BlockQuotes_Spec193()
        {
            // Line number in CommonMark Specs: 3288
            // Markdown:    > # Foo\n   > bar\n > baz\n
            // Expected HTML: <blockquote>\n<h1>Foo</h1>\n<p>bar\nbaz</p>\n</blockquote>\n

            SpecTestHelper.AssertCompliance("   > # Foo\n   > bar\n > baz\n",
                "<blockquote>\n<h1>Foo</h1>\n<p>bar\nbaz</p>\n</blockquote>\n",
                "all",
                true);
        }

        [Fact]
        public void BlockQuotes_Spec194()
        {
            // Line number in CommonMark Specs: 3303
            // Markdown:     > # Foo\n    > bar\n    > baz\n
            // Expected HTML: <pre><code>&gt; # Foo\n&gt; bar\n&gt; baz\n</code></pre>\n

            SpecTestHelper.AssertCompliance("    > # Foo\n    > bar\n    > baz\n",
                "<pre><code>&gt; # Foo\n&gt; bar\n&gt; baz\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void BlockQuotes_Spec195()
        {
            // Line number in CommonMark Specs: 3318
            // Markdown: > # Foo\n> bar\nbaz\n
            // Expected HTML: <blockquote>\n<h1>Foo</h1>\n<p>bar\nbaz</p>\n</blockquote>\n

            SpecTestHelper.AssertCompliance("> # Foo\n> bar\nbaz\n",
                "<blockquote>\n<h1>Foo</h1>\n<p>bar\nbaz</p>\n</blockquote>\n",
                "all",
                true);
        }

        [Fact]
        public void BlockQuotes_Spec196()
        {
            // Line number in CommonMark Specs: 3334
            // Markdown: > bar\nbaz\n> foo\n
            // Expected HTML: <blockquote>\n<p>bar\nbaz\nfoo</p>\n</blockquote>\n

            SpecTestHelper.AssertCompliance("> bar\nbaz\n> foo\n",
                "<blockquote>\n<p>bar\nbaz\nfoo</p>\n</blockquote>\n",
                "all",
                true);
        }

        [Fact]
        public void BlockQuotes_Spec197()
        {
            // Line number in CommonMark Specs: 3358
            // Markdown: > foo\n---\n
            // Expected HTML: <blockquote>\n<p>foo</p>\n</blockquote>\n<hr />\n

            SpecTestHelper.AssertCompliance("> foo\n---\n",
                "<blockquote>\n<p>foo</p>\n</blockquote>\n<hr />\n",
                "all",
                true);
        }

        [Fact]
        public void BlockQuotes_Spec198()
        {
            // Line number in CommonMark Specs: 3378
            // Markdown: > - foo\n- bar\n
            // Expected HTML: <blockquote>\n<ul>\n<li>foo</li>\n</ul>\n</blockquote>\n<ul>\n<li>bar</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("> - foo\n- bar\n",
                "<blockquote>\n<ul>\n<li>foo</li>\n</ul>\n</blockquote>\n<ul>\n<li>bar</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void BlockQuotes_Spec199()
        {
            // Line number in CommonMark Specs: 3396
            // Markdown: >     foo\n    bar\n
            // Expected HTML: <blockquote>\n<pre><code>foo\n</code></pre>\n</blockquote>\n<pre><code>bar\n</code></pre>\n

            SpecTestHelper.AssertCompliance(">     foo\n    bar\n",
                "<blockquote>\n<pre><code>foo\n</code></pre>\n</blockquote>\n<pre><code>bar\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void BlockQuotes_Spec200()
        {
            // Line number in CommonMark Specs: 3409
            // Markdown: > ```\nfoo\n```\n
            // Expected HTML: <blockquote>\n<pre><code></code></pre>\n</blockquote>\n<p>foo</p>\n<pre><code></code></pre>\n

            SpecTestHelper.AssertCompliance("> ```\nfoo\n```\n",
                "<blockquote>\n<pre><code></code></pre>\n</blockquote>\n<p>foo</p>\n<pre><code></code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void BlockQuotes_Spec201()
        {
            // Line number in CommonMark Specs: 3425
            // Markdown: > foo\n    - bar\n
            // Expected HTML: <blockquote>\n<p>foo\n- bar</p>\n</blockquote>\n

            SpecTestHelper.AssertCompliance("> foo\n    - bar\n",
                "<blockquote>\n<p>foo\n- bar</p>\n</blockquote>\n",
                "all",
                true);
        }

        [Fact]
        public void BlockQuotes_Spec202()
        {
            // Line number in CommonMark Specs: 3449
            // Markdown: >\n
            // Expected HTML: <blockquote>\n</blockquote>\n

            SpecTestHelper.AssertCompliance(">\n",
                "<blockquote>\n</blockquote>\n",
                "all",
                true);
        }

        [Fact]
        public void BlockQuotes_Spec203()
        {
            // Line number in CommonMark Specs: 3457
            // Markdown: >\n>  \n> \n
            // Expected HTML: <blockquote>\n</blockquote>\n

            SpecTestHelper.AssertCompliance(">\n>  \n> \n",
                "<blockquote>\n</blockquote>\n",
                "all",
                true);
        }

        [Fact]
        public void BlockQuotes_Spec204()
        {
            // Line number in CommonMark Specs: 3469
            // Markdown: >\n> foo\n>  \n
            // Expected HTML: <blockquote>\n<p>foo</p>\n</blockquote>\n

            SpecTestHelper.AssertCompliance(">\n> foo\n>  \n",
                "<blockquote>\n<p>foo</p>\n</blockquote>\n",
                "all",
                true);
        }

        [Fact]
        public void BlockQuotes_Spec205()
        {
            // Line number in CommonMark Specs: 3482
            // Markdown: > foo\n\n> bar\n
            // Expected HTML: <blockquote>\n<p>foo</p>\n</blockquote>\n<blockquote>\n<p>bar</p>\n</blockquote>\n

            SpecTestHelper.AssertCompliance("> foo\n\n> bar\n",
                "<blockquote>\n<p>foo</p>\n</blockquote>\n<blockquote>\n<p>bar</p>\n</blockquote>\n",
                "all",
                true);
        }

        [Fact]
        public void BlockQuotes_Spec206()
        {
            // Line number in CommonMark Specs: 3504
            // Markdown: > foo\n> bar\n
            // Expected HTML: <blockquote>\n<p>foo\nbar</p>\n</blockquote>\n

            SpecTestHelper.AssertCompliance("> foo\n> bar\n",
                "<blockquote>\n<p>foo\nbar</p>\n</blockquote>\n",
                "all",
                true);
        }

        [Fact]
        public void BlockQuotes_Spec207()
        {
            // Line number in CommonMark Specs: 3517
            // Markdown: > foo\n>\n> bar\n
            // Expected HTML: <blockquote>\n<p>foo</p>\n<p>bar</p>\n</blockquote>\n

            SpecTestHelper.AssertCompliance("> foo\n>\n> bar\n",
                "<blockquote>\n<p>foo</p>\n<p>bar</p>\n</blockquote>\n",
                "all",
                true);
        }

        [Fact]
        public void BlockQuotes_Spec208()
        {
            // Line number in CommonMark Specs: 3531
            // Markdown: foo\n> bar\n
            // Expected HTML: <p>foo</p>\n<blockquote>\n<p>bar</p>\n</blockquote>\n

            SpecTestHelper.AssertCompliance("foo\n> bar\n",
                "<p>foo</p>\n<blockquote>\n<p>bar</p>\n</blockquote>\n",
                "all",
                true);
        }

        [Fact]
        public void BlockQuotes_Spec209()
        {
            // Line number in CommonMark Specs: 3545
            // Markdown: > aaa\n***\n> bbb\n
            // Expected HTML: <blockquote>\n<p>aaa</p>\n</blockquote>\n<hr />\n<blockquote>\n<p>bbb</p>\n</blockquote>\n

            SpecTestHelper.AssertCompliance("> aaa\n***\n> bbb\n",
                "<blockquote>\n<p>aaa</p>\n</blockquote>\n<hr />\n<blockquote>\n<p>bbb</p>\n</blockquote>\n",
                "all",
                true);
        }

        [Fact]
        public void BlockQuotes_Spec210()
        {
            // Line number in CommonMark Specs: 3563
            // Markdown: > bar\nbaz\n
            // Expected HTML: <blockquote>\n<p>bar\nbaz</p>\n</blockquote>\n

            SpecTestHelper.AssertCompliance("> bar\nbaz\n",
                "<blockquote>\n<p>bar\nbaz</p>\n</blockquote>\n",
                "all",
                true);
        }

        [Fact]
        public void BlockQuotes_Spec211()
        {
            // Line number in CommonMark Specs: 3574
            // Markdown: > bar\n\nbaz\n
            // Expected HTML: <blockquote>\n<p>bar</p>\n</blockquote>\n<p>baz</p>\n

            SpecTestHelper.AssertCompliance("> bar\n\nbaz\n",
                "<blockquote>\n<p>bar</p>\n</blockquote>\n<p>baz</p>\n",
                "all",
                true);
        }

        [Fact]
        public void BlockQuotes_Spec212()
        {
            // Line number in CommonMark Specs: 3586
            // Markdown: > bar\n>\nbaz\n
            // Expected HTML: <blockquote>\n<p>bar</p>\n</blockquote>\n<p>baz</p>\n

            SpecTestHelper.AssertCompliance("> bar\n>\nbaz\n",
                "<blockquote>\n<p>bar</p>\n</blockquote>\n<p>baz</p>\n",
                "all",
                true);
        }

        [Fact]
        public void BlockQuotes_Spec213()
        {
            // Line number in CommonMark Specs: 3602
            // Markdown: > > > foo\nbar\n
            // Expected HTML: <blockquote>\n<blockquote>\n<blockquote>\n<p>foo\nbar</p>\n</blockquote>\n</blockquote>\n</blockquote>\n

            SpecTestHelper.AssertCompliance("> > > foo\nbar\n",
                "<blockquote>\n<blockquote>\n<blockquote>\n<p>foo\nbar</p>\n</blockquote>\n</blockquote>\n</blockquote>\n",
                "all",
                true);
        }

        [Fact]
        public void BlockQuotes_Spec214()
        {
            // Line number in CommonMark Specs: 3617
            // Markdown: >>> foo\n> bar\n>>baz\n
            // Expected HTML: <blockquote>\n<blockquote>\n<blockquote>\n<p>foo\nbar\nbaz</p>\n</blockquote>\n</blockquote>\n</blockquote>\n

            SpecTestHelper.AssertCompliance(">>> foo\n> bar\n>>baz\n",
                "<blockquote>\n<blockquote>\n<blockquote>\n<p>foo\nbar\nbaz</p>\n</blockquote>\n</blockquote>\n</blockquote>\n",
                "all",
                true);
        }

        [Fact]
        public void BlockQuotes_Spec215()
        {
            // Line number in CommonMark Specs: 3639
            // Markdown: >     code\n\n>    not code\n
            // Expected HTML: <blockquote>\n<pre><code>code\n</code></pre>\n</blockquote>\n<blockquote>\n<p>not code</p>\n</blockquote>\n

            SpecTestHelper.AssertCompliance(">     code\n\n>    not code\n",
                "<blockquote>\n<pre><code>code\n</code></pre>\n</blockquote>\n<blockquote>\n<p>not code</p>\n</blockquote>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec216()
        {
            // Line number in CommonMark Specs: 3694
            // Markdown: A paragraph\nwith two lines.\n\n    indented code\n\n> A block quote.\n
            // Expected HTML: <p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n

            SpecTestHelper.AssertCompliance("A paragraph\nwith two lines.\n\n    indented code\n\n> A block quote.\n",
                "<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec217()
        {
            // Line number in CommonMark Specs: 3716
            // Markdown: 1.  A paragraph\n    with two lines.\n\n        indented code\n\n    > A block quote.\n
            // Expected HTML: <ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>\n

            SpecTestHelper.AssertCompliance("1.  A paragraph\n    with two lines.\n\n        indented code\n\n    > A block quote.\n",
                "<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec218()
        {
            // Line number in CommonMark Specs: 3749
            // Markdown: - one\n\n two\n
            // Expected HTML: <ul>\n<li>one</li>\n</ul>\n<p>two</p>\n

            SpecTestHelper.AssertCompliance("- one\n\n two\n",
                "<ul>\n<li>one</li>\n</ul>\n<p>two</p>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec219()
        {
            // Line number in CommonMark Specs: 3761
            // Markdown: - one\n\n  two\n
            // Expected HTML: <ul>\n<li>\n<p>one</p>\n<p>two</p>\n</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("- one\n\n  two\n",
                "<ul>\n<li>\n<p>one</p>\n<p>two</p>\n</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec220()
        {
            // Line number in CommonMark Specs: 3775
            // Markdown:  -    one\n\n     two\n
            // Expected HTML: <ul>\n<li>one</li>\n</ul>\n<pre><code> two\n</code></pre>\n

            SpecTestHelper.AssertCompliance(" -    one\n\n     two\n",
                "<ul>\n<li>one</li>\n</ul>\n<pre><code> two\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec221()
        {
            // Line number in CommonMark Specs: 3788
            // Markdown:  -    one\n\n      two\n
            // Expected HTML: <ul>\n<li>\n<p>one</p>\n<p>two</p>\n</li>\n</ul>\n

            SpecTestHelper.AssertCompliance(" -    one\n\n      two\n",
                "<ul>\n<li>\n<p>one</p>\n<p>two</p>\n</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec222()
        {
            // Line number in CommonMark Specs: 3810
            // Markdown:    > > 1.  one\n>>\n>>     two\n
            // Expected HTML: <blockquote>\n<blockquote>\n<ol>\n<li>\n<p>one</p>\n<p>two</p>\n</li>\n</ol>\n</blockquote>\n</blockquote>\n

            SpecTestHelper.AssertCompliance("   > > 1.  one\n>>\n>>     two\n",
                "<blockquote>\n<blockquote>\n<ol>\n<li>\n<p>one</p>\n<p>two</p>\n</li>\n</ol>\n</blockquote>\n</blockquote>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec223()
        {
            // Line number in CommonMark Specs: 3837
            // Markdown: >>- one\n>>\n  >  > two\n
            // Expected HTML: <blockquote>\n<blockquote>\n<ul>\n<li>one</li>\n</ul>\n<p>two</p>\n</blockquote>\n</blockquote>\n

            SpecTestHelper.AssertCompliance(">>- one\n>>\n  >  > two\n",
                "<blockquote>\n<blockquote>\n<ul>\n<li>one</li>\n</ul>\n<p>two</p>\n</blockquote>\n</blockquote>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec224()
        {
            // Line number in CommonMark Specs: 3856
            // Markdown: -one\n\n2.two\n
            // Expected HTML: <p>-one</p>\n<p>2.two</p>\n

            SpecTestHelper.AssertCompliance("-one\n\n2.two\n",
                "<p>-one</p>\n<p>2.two</p>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec225()
        {
            // Line number in CommonMark Specs: 3869
            // Markdown: - foo\n\n\n  bar\n
            // Expected HTML: <ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("- foo\n\n\n  bar\n",
                "<ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec226()
        {
            // Line number in CommonMark Specs: 3886
            // Markdown: 1.  foo\n\n    ```\n    bar\n    ```\n\n    baz\n\n    > bam\n
            // Expected HTML: <ol>\n<li>\n<p>foo</p>\n<pre><code>bar\n</code></pre>\n<p>baz</p>\n<blockquote>\n<p>bam</p>\n</blockquote>\n</li>\n</ol>\n

            SpecTestHelper.AssertCompliance("1.  foo\n\n    ```\n    bar\n    ```\n\n    baz\n\n    > bam\n",
                "<ol>\n<li>\n<p>foo</p>\n<pre><code>bar\n</code></pre>\n<p>baz</p>\n<blockquote>\n<p>bam</p>\n</blockquote>\n</li>\n</ol>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec227()
        {
            // Line number in CommonMark Specs: 3914
            // Markdown: - Foo\n\n      bar\n\n\n      baz\n
            // Expected HTML: <ul>\n<li>\n<p>Foo</p>\n<pre><code>bar\n\n\nbaz\n</code></pre>\n</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("- Foo\n\n      bar\n\n\n      baz\n",
                "<ul>\n<li>\n<p>Foo</p>\n<pre><code>bar\n\n\nbaz\n</code></pre>\n</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec228()
        {
            // Line number in CommonMark Specs: 3936
            // Markdown: 123456789. ok\n
            // Expected HTML: <ol start=\"123456789\">\n<li>ok</li>\n</ol>\n

            SpecTestHelper.AssertCompliance("123456789. ok\n",
                "<ol start=\"123456789\">\n<li>ok</li>\n</ol>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec229()
        {
            // Line number in CommonMark Specs: 3945
            // Markdown: 1234567890. not ok\n
            // Expected HTML: <p>1234567890. not ok</p>\n

            SpecTestHelper.AssertCompliance("1234567890. not ok\n",
                "<p>1234567890. not ok</p>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec230()
        {
            // Line number in CommonMark Specs: 3954
            // Markdown: 0. ok\n
            // Expected HTML: <ol start=\"0\">\n<li>ok</li>\n</ol>\n

            SpecTestHelper.AssertCompliance("0. ok\n",
                "<ol start=\"0\">\n<li>ok</li>\n</ol>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec231()
        {
            // Line number in CommonMark Specs: 3963
            // Markdown: 003. ok\n
            // Expected HTML: <ol start=\"3\">\n<li>ok</li>\n</ol>\n

            SpecTestHelper.AssertCompliance("003. ok\n",
                "<ol start=\"3\">\n<li>ok</li>\n</ol>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec232()
        {
            // Line number in CommonMark Specs: 3974
            // Markdown: -1. not ok\n
            // Expected HTML: <p>-1. not ok</p>\n

            SpecTestHelper.AssertCompliance("-1. not ok\n",
                "<p>-1. not ok</p>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec233()
        {
            // Line number in CommonMark Specs: 3998
            // Markdown: - foo\n\n      bar\n
            // Expected HTML: <ul>\n<li>\n<p>foo</p>\n<pre><code>bar\n</code></pre>\n</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("- foo\n\n      bar\n",
                "<ul>\n<li>\n<p>foo</p>\n<pre><code>bar\n</code></pre>\n</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec234()
        {
            // Line number in CommonMark Specs: 4015
            // Markdown:   10.  foo\n\n           bar\n
            // Expected HTML: <ol start=\"10\">\n<li>\n<p>foo</p>\n<pre><code>bar\n</code></pre>\n</li>\n</ol>\n

            SpecTestHelper.AssertCompliance("  10.  foo\n\n           bar\n",
                "<ol start=\"10\">\n<li>\n<p>foo</p>\n<pre><code>bar\n</code></pre>\n</li>\n</ol>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec235()
        {
            // Line number in CommonMark Specs: 4034
            // Markdown:     indented code\n\nparagraph\n\n    more code\n
            // Expected HTML: <pre><code>indented code\n</code></pre>\n<p>paragraph</p>\n<pre><code>more code\n</code></pre>\n

            SpecTestHelper.AssertCompliance("    indented code\n\nparagraph\n\n    more code\n",
                "<pre><code>indented code\n</code></pre>\n<p>paragraph</p>\n<pre><code>more code\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec236()
        {
            // Line number in CommonMark Specs: 4049
            // Markdown: 1.     indented code\n\n   paragraph\n\n       more code\n
            // Expected HTML: <ol>\n<li>\n<pre><code>indented code\n</code></pre>\n<p>paragraph</p>\n<pre><code>more code\n</code></pre>\n</li>\n</ol>\n

            SpecTestHelper.AssertCompliance("1.     indented code\n\n   paragraph\n\n       more code\n",
                "<ol>\n<li>\n<pre><code>indented code\n</code></pre>\n<p>paragraph</p>\n<pre><code>more code\n</code></pre>\n</li>\n</ol>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec237()
        {
            // Line number in CommonMark Specs: 4071
            // Markdown: 1.      indented code\n\n   paragraph\n\n       more code\n
            // Expected HTML: <ol>\n<li>\n<pre><code> indented code\n</code></pre>\n<p>paragraph</p>\n<pre><code>more code\n</code></pre>\n</li>\n</ol>\n

            SpecTestHelper.AssertCompliance("1.      indented code\n\n   paragraph\n\n       more code\n",
                "<ol>\n<li>\n<pre><code> indented code\n</code></pre>\n<p>paragraph</p>\n<pre><code>more code\n</code></pre>\n</li>\n</ol>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec238()
        {
            // Line number in CommonMark Specs: 4098
            // Markdown:    foo\n\nbar\n
            // Expected HTML: <p>foo</p>\n<p>bar</p>\n

            SpecTestHelper.AssertCompliance("   foo\n\nbar\n",
                "<p>foo</p>\n<p>bar</p>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec239()
        {
            // Line number in CommonMark Specs: 4108
            // Markdown: -    foo\n\n  bar\n
            // Expected HTML: <ul>\n<li>foo</li>\n</ul>\n<p>bar</p>\n

            SpecTestHelper.AssertCompliance("-    foo\n\n  bar\n",
                "<ul>\n<li>foo</li>\n</ul>\n<p>bar</p>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec240()
        {
            // Line number in CommonMark Specs: 4125
            // Markdown: -  foo\n\n   bar\n
            // Expected HTML: <ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("-  foo\n\n   bar\n",
                "<ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec241()
        {
            // Line number in CommonMark Specs: 4153
            // Markdown: -\n  foo\n-\n  ```\n  bar\n  ```\n-\n      baz\n
            // Expected HTML: <ul>\n<li>foo</li>\n<li>\n<pre><code>bar\n</code></pre>\n</li>\n<li>\n<pre><code>baz\n</code></pre>\n</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("-\n  foo\n-\n  ```\n  bar\n  ```\n-\n      baz\n",
                "<ul>\n<li>foo</li>\n<li>\n<pre><code>bar\n</code></pre>\n</li>\n<li>\n<pre><code>baz\n</code></pre>\n</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec242()
        {
            // Line number in CommonMark Specs: 4179
            // Markdown: -   \n  foo\n
            // Expected HTML: <ul>\n<li>foo</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("-   \n  foo\n",
                "<ul>\n<li>foo</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec243()
        {
            // Line number in CommonMark Specs: 4193
            // Markdown: -\n\n  foo\n
            // Expected HTML: <ul>\n<li></li>\n</ul>\n<p>foo</p>\n

            SpecTestHelper.AssertCompliance("-\n\n  foo\n",
                "<ul>\n<li></li>\n</ul>\n<p>foo</p>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec244()
        {
            // Line number in CommonMark Specs: 4207
            // Markdown: - foo\n-\n- bar\n
            // Expected HTML: <ul>\n<li>foo</li>\n<li></li>\n<li>bar</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("- foo\n-\n- bar\n",
                "<ul>\n<li>foo</li>\n<li></li>\n<li>bar</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec245()
        {
            // Line number in CommonMark Specs: 4222
            // Markdown: - foo\n-   \n- bar\n
            // Expected HTML: <ul>\n<li>foo</li>\n<li></li>\n<li>bar</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("- foo\n-   \n- bar\n",
                "<ul>\n<li>foo</li>\n<li></li>\n<li>bar</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec246()
        {
            // Line number in CommonMark Specs: 4237
            // Markdown: 1. foo\n2.\n3. bar\n
            // Expected HTML: <ol>\n<li>foo</li>\n<li></li>\n<li>bar</li>\n</ol>\n

            SpecTestHelper.AssertCompliance("1. foo\n2.\n3. bar\n",
                "<ol>\n<li>foo</li>\n<li></li>\n<li>bar</li>\n</ol>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec247()
        {
            // Line number in CommonMark Specs: 4252
            // Markdown: *\n
            // Expected HTML: <ul>\n<li></li>\n</ul>\n

            SpecTestHelper.AssertCompliance("*\n",
                "<ul>\n<li></li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec248()
        {
            // Line number in CommonMark Specs: 4262
            // Markdown: foo\n*\n\nfoo\n1.\n
            // Expected HTML: <p>foo\n*</p>\n<p>foo\n1.</p>\n

            SpecTestHelper.AssertCompliance("foo\n*\n\nfoo\n1.\n",
                "<p>foo\n*</p>\n<p>foo\n1.</p>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec249()
        {
            // Line number in CommonMark Specs: 4284
            // Markdown:  1.  A paragraph\n     with two lines.\n\n         indented code\n\n     > A block quote.\n
            // Expected HTML: <ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>\n

            SpecTestHelper.AssertCompliance(" 1.  A paragraph\n     with two lines.\n\n         indented code\n\n     > A block quote.\n",
                "<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec250()
        {
            // Line number in CommonMark Specs: 4308
            // Markdown:   1.  A paragraph\n      with two lines.\n\n          indented code\n\n      > A block quote.\n
            // Expected HTML: <ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>\n

            SpecTestHelper.AssertCompliance("  1.  A paragraph\n      with two lines.\n\n          indented code\n\n      > A block quote.\n",
                "<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec251()
        {
            // Line number in CommonMark Specs: 4332
            // Markdown:    1.  A paragraph\n       with two lines.\n\n           indented code\n\n       > A block quote.\n
            // Expected HTML: <ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>\n

            SpecTestHelper.AssertCompliance("   1.  A paragraph\n       with two lines.\n\n           indented code\n\n       > A block quote.\n",
                "<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec252()
        {
            // Line number in CommonMark Specs: 4356
            // Markdown:     1.  A paragraph\n        with two lines.\n\n            indented code\n\n        > A block quote.\n
            // Expected HTML: <pre><code>1.  A paragraph\n    with two lines.\n\n        indented code\n\n    &gt; A block quote.\n</code></pre>\n

            SpecTestHelper.AssertCompliance("    1.  A paragraph\n        with two lines.\n\n            indented code\n\n        > A block quote.\n",
                "<pre><code>1.  A paragraph\n    with two lines.\n\n        indented code\n\n    &gt; A block quote.\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec253()
        {
            // Line number in CommonMark Specs: 4386
            // Markdown:   1.  A paragraph\nwith two lines.\n\n          indented code\n\n      > A block quote.\n
            // Expected HTML: <ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>\n

            SpecTestHelper.AssertCompliance("  1.  A paragraph\nwith two lines.\n\n          indented code\n\n      > A block quote.\n",
                "<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec254()
        {
            // Line number in CommonMark Specs: 4410
            // Markdown:   1.  A paragraph\n    with two lines.\n
            // Expected HTML: <ol>\n<li>A paragraph\nwith two lines.</li>\n</ol>\n

            SpecTestHelper.AssertCompliance("  1.  A paragraph\n    with two lines.\n",
                "<ol>\n<li>A paragraph\nwith two lines.</li>\n</ol>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec255()
        {
            // Line number in CommonMark Specs: 4423
            // Markdown: > 1. > Blockquote\ncontinued here.\n
            // Expected HTML: <blockquote>\n<ol>\n<li>\n<blockquote>\n<p>Blockquote\ncontinued here.</p>\n</blockquote>\n</li>\n</ol>\n</blockquote>\n

            SpecTestHelper.AssertCompliance("> 1. > Blockquote\ncontinued here.\n",
                "<blockquote>\n<ol>\n<li>\n<blockquote>\n<p>Blockquote\ncontinued here.</p>\n</blockquote>\n</li>\n</ol>\n</blockquote>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec256()
        {
            // Line number in CommonMark Specs: 4440
            // Markdown: > 1. > Blockquote\n> continued here.\n
            // Expected HTML: <blockquote>\n<ol>\n<li>\n<blockquote>\n<p>Blockquote\ncontinued here.</p>\n</blockquote>\n</li>\n</ol>\n</blockquote>\n

            SpecTestHelper.AssertCompliance("> 1. > Blockquote\n> continued here.\n",
                "<blockquote>\n<ol>\n<li>\n<blockquote>\n<p>Blockquote\ncontinued here.</p>\n</blockquote>\n</li>\n</ol>\n</blockquote>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec257()
        {
            // Line number in CommonMark Specs: 4467
            // Markdown: - foo\n  - bar\n    - baz\n      - boo\n
            // Expected HTML: <ul>\n<li>foo\n<ul>\n<li>bar\n<ul>\n<li>baz\n<ul>\n<li>boo</li>\n</ul>\n</li>\n</ul>\n</li>\n</ul>\n</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("- foo\n  - bar\n    - baz\n      - boo\n",
                "<ul>\n<li>foo\n<ul>\n<li>bar\n<ul>\n<li>baz\n<ul>\n<li>boo</li>\n</ul>\n</li>\n</ul>\n</li>\n</ul>\n</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec258()
        {
            // Line number in CommonMark Specs: 4493
            // Markdown: - foo\n - bar\n  - baz\n   - boo\n
            // Expected HTML: <ul>\n<li>foo</li>\n<li>bar</li>\n<li>baz</li>\n<li>boo</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("- foo\n - bar\n  - baz\n   - boo\n",
                "<ul>\n<li>foo</li>\n<li>bar</li>\n<li>baz</li>\n<li>boo</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec259()
        {
            // Line number in CommonMark Specs: 4510
            // Markdown: 10) foo\n    - bar\n
            // Expected HTML: <ol start=\"10\">\n<li>foo\n<ul>\n<li>bar</li>\n</ul>\n</li>\n</ol>\n

            SpecTestHelper.AssertCompliance("10) foo\n    - bar\n",
                "<ol start=\"10\">\n<li>foo\n<ul>\n<li>bar</li>\n</ul>\n</li>\n</ol>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec260()
        {
            // Line number in CommonMark Specs: 4526
            // Markdown: 10) foo\n   - bar\n
            // Expected HTML: <ol start=\"10\">\n<li>foo</li>\n</ol>\n<ul>\n<li>bar</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("10) foo\n   - bar\n",
                "<ol start=\"10\">\n<li>foo</li>\n</ol>\n<ul>\n<li>bar</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec261()
        {
            // Line number in CommonMark Specs: 4541
            // Markdown: - - foo\n
            // Expected HTML: <ul>\n<li>\n<ul>\n<li>foo</li>\n</ul>\n</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("- - foo\n",
                "<ul>\n<li>\n<ul>\n<li>foo</li>\n</ul>\n</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec262()
        {
            // Line number in CommonMark Specs: 4554
            // Markdown: 1. - 2. foo\n
            // Expected HTML: <ol>\n<li>\n<ul>\n<li>\n<ol start=\"2\">\n<li>foo</li>\n</ol>\n</li>\n</ul>\n</li>\n</ol>\n

            SpecTestHelper.AssertCompliance("1. - 2. foo\n",
                "<ol>\n<li>\n<ul>\n<li>\n<ol start=\"2\">\n<li>foo</li>\n</ol>\n</li>\n</ul>\n</li>\n</ol>\n",
                "all",
                true);
        }

        [Fact]
        public void ListItems_Spec263()
        {
            // Line number in CommonMark Specs: 4573
            // Markdown: - # Foo\n- Bar\n  ---\n  baz\n
            // Expected HTML: <ul>\n<li>\n<h1>Foo</h1>\n</li>\n<li>\n<h2>Bar</h2>\nbaz</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("- # Foo\n- Bar\n  ---\n  baz\n",
                "<ul>\n<li>\n<h1>Foo</h1>\n</li>\n<li>\n<h2>Bar</h2>\nbaz</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void Lists_Spec264()
        {
            // Line number in CommonMark Specs: 4809
            // Markdown: - foo\n- bar\n+ baz\n
            // Expected HTML: <ul>\n<li>foo</li>\n<li>bar</li>\n</ul>\n<ul>\n<li>baz</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("- foo\n- bar\n+ baz\n",
                "<ul>\n<li>foo</li>\n<li>bar</li>\n</ul>\n<ul>\n<li>baz</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void Lists_Spec265()
        {
            // Line number in CommonMark Specs: 4824
            // Markdown: 1. foo\n2. bar\n3) baz\n
            // Expected HTML: <ol>\n<li>foo</li>\n<li>bar</li>\n</ol>\n<ol start=\"3\">\n<li>baz</li>\n</ol>\n

            SpecTestHelper.AssertCompliance("1. foo\n2. bar\n3) baz\n",
                "<ol>\n<li>foo</li>\n<li>bar</li>\n</ol>\n<ol start=\"3\">\n<li>baz</li>\n</ol>\n",
                "all",
                true);
        }

        [Fact]
        public void Lists_Spec266()
        {
            // Line number in CommonMark Specs: 4843
            // Markdown: Foo\n- bar\n- baz\n
            // Expected HTML: <p>Foo</p>\n<ul>\n<li>bar</li>\n<li>baz</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("Foo\n- bar\n- baz\n",
                "<p>Foo</p>\n<ul>\n<li>bar</li>\n<li>baz</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void Lists_Spec267()
        {
            // Line number in CommonMark Specs: 4920
            // Markdown: The number of windows in my house is\n14.  The number of doors is 6.\n
            // Expected HTML: <p>The number of windows in my house is\n14.  The number of doors is 6.</p>\n

            SpecTestHelper.AssertCompliance("The number of windows in my house is\n14.  The number of doors is 6.\n",
                "<p>The number of windows in my house is\n14.  The number of doors is 6.</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Lists_Spec268()
        {
            // Line number in CommonMark Specs: 4930
            // Markdown: The number of windows in my house is\n1.  The number of doors is 6.\n
            // Expected HTML: <p>The number of windows in my house is</p>\n<ol>\n<li>The number of doors is 6.</li>\n</ol>\n

            SpecTestHelper.AssertCompliance("The number of windows in my house is\n1.  The number of doors is 6.\n",
                "<p>The number of windows in my house is</p>\n<ol>\n<li>The number of doors is 6.</li>\n</ol>\n",
                "all",
                true);
        }

        [Fact]
        public void Lists_Spec269()
        {
            // Line number in CommonMark Specs: 4944
            // Markdown: - foo\n\n- bar\n\n\n- baz\n
            // Expected HTML: <ul>\n<li>\n<p>foo</p>\n</li>\n<li>\n<p>bar</p>\n</li>\n<li>\n<p>baz</p>\n</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("- foo\n\n- bar\n\n\n- baz\n",
                "<ul>\n<li>\n<p>foo</p>\n</li>\n<li>\n<p>bar</p>\n</li>\n<li>\n<p>baz</p>\n</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void Lists_Spec270()
        {
            // Line number in CommonMark Specs: 4965
            // Markdown: - foo\n  - bar\n    - baz\n\n\n      bim\n
            // Expected HTML: <ul>\n<li>foo\n<ul>\n<li>bar\n<ul>\n<li>\n<p>baz</p>\n<p>bim</p>\n</li>\n</ul>\n</li>\n</ul>\n</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("- foo\n  - bar\n    - baz\n\n\n      bim\n",
                "<ul>\n<li>foo\n<ul>\n<li>bar\n<ul>\n<li>\n<p>baz</p>\n<p>bim</p>\n</li>\n</ul>\n</li>\n</ul>\n</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void Lists_Spec271()
        {
            // Line number in CommonMark Specs: 4995
            // Markdown: - foo\n- bar\n\n<!-- -->\n\n- baz\n- bim\n
            // Expected HTML: <ul>\n<li>foo</li>\n<li>bar</li>\n</ul>\n<!-- -->\n<ul>\n<li>baz</li>\n<li>bim</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("- foo\n- bar\n\n<!-- -->\n\n- baz\n- bim\n",
                "<ul>\n<li>foo</li>\n<li>bar</li>\n</ul>\n<!-- -->\n<ul>\n<li>baz</li>\n<li>bim</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void Lists_Spec272()
        {
            // Line number in CommonMark Specs: 5016
            // Markdown: -   foo\n\n    notcode\n\n-   foo\n\n<!-- -->\n\n    code\n
            // Expected HTML: <ul>\n<li>\n<p>foo</p>\n<p>notcode</p>\n</li>\n<li>\n<p>foo</p>\n</li>\n</ul>\n<!-- -->\n<pre><code>code\n</code></pre>\n

            SpecTestHelper.AssertCompliance("-   foo\n\n    notcode\n\n-   foo\n\n<!-- -->\n\n    code\n",
                "<ul>\n<li>\n<p>foo</p>\n<p>notcode</p>\n</li>\n<li>\n<p>foo</p>\n</li>\n</ul>\n<!-- -->\n<pre><code>code\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void Lists_Spec273()
        {
            // Line number in CommonMark Specs: 5047
            // Markdown: - a\n - b\n  - c\n   - d\n    - e\n   - f\n  - g\n - h\n- i\n
            // Expected HTML: <ul>\n<li>a</li>\n<li>b</li>\n<li>c</li>\n<li>d</li>\n<li>e</li>\n<li>f</li>\n<li>g</li>\n<li>h</li>\n<li>i</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("- a\n - b\n  - c\n   - d\n    - e\n   - f\n  - g\n - h\n- i\n",
                "<ul>\n<li>a</li>\n<li>b</li>\n<li>c</li>\n<li>d</li>\n<li>e</li>\n<li>f</li>\n<li>g</li>\n<li>h</li>\n<li>i</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void Lists_Spec274()
        {
            // Line number in CommonMark Specs: 5072
            // Markdown: 1. a\n\n  2. b\n\n    3. c\n
            // Expected HTML: <ol>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n</li>\n<li>\n<p>c</p>\n</li>\n</ol>\n

            SpecTestHelper.AssertCompliance("1. a\n\n  2. b\n\n    3. c\n",
                "<ol>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n</li>\n<li>\n<p>c</p>\n</li>\n</ol>\n",
                "all",
                true);
        }

        [Fact]
        public void Lists_Spec275()
        {
            // Line number in CommonMark Specs: 5096
            // Markdown: - a\n- b\n\n- c\n
            // Expected HTML: <ul>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n</li>\n<li>\n<p>c</p>\n</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("- a\n- b\n\n- c\n",
                "<ul>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n</li>\n<li>\n<p>c</p>\n</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void Lists_Spec276()
        {
            // Line number in CommonMark Specs: 5118
            // Markdown: * a\n*\n\n* c\n
            // Expected HTML: <ul>\n<li>\n<p>a</p>\n</li>\n<li></li>\n<li>\n<p>c</p>\n</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("* a\n*\n\n* c\n",
                "<ul>\n<li>\n<p>a</p>\n</li>\n<li></li>\n<li>\n<p>c</p>\n</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void Lists_Spec277()
        {
            // Line number in CommonMark Specs: 5140
            // Markdown: - a\n- b\n\n  c\n- d\n
            // Expected HTML: <ul>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n<p>c</p>\n</li>\n<li>\n<p>d</p>\n</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("- a\n- b\n\n  c\n- d\n",
                "<ul>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n<p>c</p>\n</li>\n<li>\n<p>d</p>\n</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void Lists_Spec278()
        {
            // Line number in CommonMark Specs: 5162
            // Markdown: - a\n- b\n\n  [ref]: /url\n- d\n
            // Expected HTML: <ul>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n</li>\n<li>\n<p>d</p>\n</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("- a\n- b\n\n  [ref]: /url\n- d\n",
                "<ul>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n</li>\n<li>\n<p>d</p>\n</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void Lists_Spec279()
        {
            // Line number in CommonMark Specs: 5185
            // Markdown: - a\n- ```\n  b\n\n\n  ```\n- c\n
            // Expected HTML: <ul>\n<li>a</li>\n<li>\n<pre><code>b\n\n\n</code></pre>\n</li>\n<li>c</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("- a\n- ```\n  b\n\n\n  ```\n- c\n",
                "<ul>\n<li>a</li>\n<li>\n<pre><code>b\n\n\n</code></pre>\n</li>\n<li>c</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void Lists_Spec280()
        {
            // Line number in CommonMark Specs: 5211
            // Markdown: - a\n  - b\n\n    c\n- d\n
            // Expected HTML: <ul>\n<li>a\n<ul>\n<li>\n<p>b</p>\n<p>c</p>\n</li>\n</ul>\n</li>\n<li>d</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("- a\n  - b\n\n    c\n- d\n",
                "<ul>\n<li>a\n<ul>\n<li>\n<p>b</p>\n<p>c</p>\n</li>\n</ul>\n</li>\n<li>d</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void Lists_Spec281()
        {
            // Line number in CommonMark Specs: 5235
            // Markdown: * a\n  > b\n  >\n* c\n
            // Expected HTML: <ul>\n<li>a\n<blockquote>\n<p>b</p>\n</blockquote>\n</li>\n<li>c</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("* a\n  > b\n  >\n* c\n",
                "<ul>\n<li>a\n<blockquote>\n<p>b</p>\n</blockquote>\n</li>\n<li>c</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void Lists_Spec282()
        {
            // Line number in CommonMark Specs: 5255
            // Markdown: - a\n  > b\n  ```\n  c\n  ```\n- d\n
            // Expected HTML: <ul>\n<li>a\n<blockquote>\n<p>b</p>\n</blockquote>\n<pre><code>c\n</code></pre>\n</li>\n<li>d</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("- a\n  > b\n  ```\n  c\n  ```\n- d\n",
                "<ul>\n<li>a\n<blockquote>\n<p>b</p>\n</blockquote>\n<pre><code>c\n</code></pre>\n</li>\n<li>d</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void Lists_Spec283()
        {
            // Line number in CommonMark Specs: 5278
            // Markdown: - a\n
            // Expected HTML: <ul>\n<li>a</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("- a\n",
                "<ul>\n<li>a</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void Lists_Spec284()
        {
            // Line number in CommonMark Specs: 5287
            // Markdown: - a\n  - b\n
            // Expected HTML: <ul>\n<li>a\n<ul>\n<li>b</li>\n</ul>\n</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("- a\n  - b\n",
                "<ul>\n<li>a\n<ul>\n<li>b</li>\n</ul>\n</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void Lists_Spec285()
        {
            // Line number in CommonMark Specs: 5304
            // Markdown: 1. ```\n   foo\n   ```\n\n   bar\n
            // Expected HTML: <ol>\n<li>\n<pre><code>foo\n</code></pre>\n<p>bar</p>\n</li>\n</ol>\n

            SpecTestHelper.AssertCompliance("1. ```\n   foo\n   ```\n\n   bar\n",
                "<ol>\n<li>\n<pre><code>foo\n</code></pre>\n<p>bar</p>\n</li>\n</ol>\n",
                "all",
                true);
        }

        [Fact]
        public void Lists_Spec286()
        {
            // Line number in CommonMark Specs: 5323
            // Markdown: * foo\n  * bar\n\n  baz\n
            // Expected HTML: <ul>\n<li>\n<p>foo</p>\n<ul>\n<li>bar</li>\n</ul>\n<p>baz</p>\n</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("* foo\n  * bar\n\n  baz\n",
                "<ul>\n<li>\n<p>foo</p>\n<ul>\n<li>bar</li>\n</ul>\n<p>baz</p>\n</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void Lists_Spec287()
        {
            // Line number in CommonMark Specs: 5341
            // Markdown: - a\n  - b\n  - c\n\n- d\n  - e\n  - f\n
            // Expected HTML: <ul>\n<li>\n<p>a</p>\n<ul>\n<li>b</li>\n<li>c</li>\n</ul>\n</li>\n<li>\n<p>d</p>\n<ul>\n<li>e</li>\n<li>f</li>\n</ul>\n</li>\n</ul>\n

            SpecTestHelper.AssertCompliance("- a\n  - b\n  - c\n\n- d\n  - e\n  - f\n",
                "<ul>\n<li>\n<p>a</p>\n<ul>\n<li>b</li>\n<li>c</li>\n</ul>\n</li>\n<li>\n<p>d</p>\n<ul>\n<li>e</li>\n<li>f</li>\n</ul>\n</li>\n</ul>\n",
                "all",
                true);
        }

        [Fact]
        public void Inlines_Spec288()
        {
            // Line number in CommonMark Specs: 5375
            // Markdown: `hi`lo`\n
            // Expected HTML: <p><code>hi</code>lo`</p>\n

            SpecTestHelper.AssertCompliance("`hi`lo`\n",
                "<p><code>hi</code>lo`</p>\n",
                "all",
                true);
        }

        [Fact]
        public void BackslashEscapes_Spec289()
        {
            // Line number in CommonMark Specs: 5389
            // Markdown: \\!\\\"\\#\\$\\%\\&\\'\\(\\)\\*\\+\\,\\-\\.\\/\\:\\;\\<\\=\\>\\?\\@\\[\\\\\\]\\^\\_\\`\\{\\|\\}\\~\n
            // Expected HTML: <p>!&quot;#$%&amp;'()*+,-./:;&lt;=&gt;?@[\\]^_`{|}~</p>\n

            SpecTestHelper.AssertCompliance("\\!\\\"\\#\\$\\%\\&\\'\\(\\)\\*\\+\\,\\-\\.\\/\\:\\;\\<\\=\\>\\?\\@\\[\\\\\\]\\^\\_\\`\\{\\|\\}\\~\n",
                "<p>!&quot;#$%&amp;'()*+,-./:;&lt;=&gt;?@[\\]^_`{|}~</p>\n",
                "all",
                true);
        }

        [Fact]
        public void BackslashEscapes_Spec290()
        {
            // Line number in CommonMark Specs: 5399
            // Markdown: \\\t\\A\\a\\ \\3\\φ\\«\n
            // Expected HTML: <p>\\\t\\A\\a\\ \\3\\φ\\«</p>\n

            SpecTestHelper.AssertCompliance("\\\t\\A\\a\\ \\3\\φ\\«\n",
                "<p>\\\t\\A\\a\\ \\3\\φ\\«</p>\n",
                "all",
                true);
        }

        [Fact]
        public void BackslashEscapes_Spec291()
        {
            // Line number in CommonMark Specs: 5409
            // Markdown: \\*not emphasized*\n\\<br/> not a tag\n\\[not a link](/foo)\n\\`not code`\n1\\. not a list\n\\* not a list\n\\# not a heading\n\\[foo]: /url \"not a reference\"\n
            // Expected HTML: <p>*not emphasized*\n&lt;br/&gt; not a tag\n[not a link](/foo)\n`not code`\n1. not a list\n* not a list\n# not a heading\n[foo]: /url &quot;not a reference&quot;</p>\n

            SpecTestHelper.AssertCompliance("\\*not emphasized*\n\\<br/> not a tag\n\\[not a link](/foo)\n\\`not code`\n1\\. not a list\n\\* not a list\n\\# not a heading\n\\[foo]: /url \"not a reference\"\n",
                "<p>*not emphasized*\n&lt;br/&gt; not a tag\n[not a link](/foo)\n`not code`\n1. not a list\n* not a list\n# not a heading\n[foo]: /url &quot;not a reference&quot;</p>\n",
                "all",
                true);
        }

        [Fact]
        public void BackslashEscapes_Spec292()
        {
            // Line number in CommonMark Specs: 5432
            // Markdown: \\\\*emphasis*\n
            // Expected HTML: <p>\\<em>emphasis</em></p>\n

            SpecTestHelper.AssertCompliance("\\\\*emphasis*\n",
                "<p>\\<em>emphasis</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void BackslashEscapes_Spec293()
        {
            // Line number in CommonMark Specs: 5441
            // Markdown: foo\\\nbar\n
            // Expected HTML: <p>foo<br />\nbar</p>\n

            SpecTestHelper.AssertCompliance("foo\\\nbar\n",
                "<p>foo<br />\nbar</p>\n",
                "all",
                true);
        }

        [Fact]
        public void BackslashEscapes_Spec294()
        {
            // Line number in CommonMark Specs: 5453
            // Markdown: `` \\[\\` ``\n
            // Expected HTML: <p><code>\\[\\`</code></p>\n

            SpecTestHelper.AssertCompliance("`` \\[\\` ``\n",
                "<p><code>\\[\\`</code></p>\n",
                "all",
                true);
        }

        [Fact]
        public void BackslashEscapes_Spec295()
        {
            // Line number in CommonMark Specs: 5460
            // Markdown:     \\[\\]\n
            // Expected HTML: <pre><code>\\[\\]\n</code></pre>\n

            SpecTestHelper.AssertCompliance("    \\[\\]\n",
                "<pre><code>\\[\\]\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void BackslashEscapes_Spec296()
        {
            // Line number in CommonMark Specs: 5468
            // Markdown: ~~~\n\\[\\]\n~~~\n
            // Expected HTML: <pre><code>\\[\\]\n</code></pre>\n

            SpecTestHelper.AssertCompliance("~~~\n\\[\\]\n~~~\n",
                "<pre><code>\\[\\]\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void BackslashEscapes_Spec297()
        {
            // Line number in CommonMark Specs: 5478
            // Markdown: <http://example.com?find=\\*>\n
            // Expected HTML: <p><a href=\"http://example.com?find=%5C*\">http://example.com?find=\\*</a></p>\n

            SpecTestHelper.AssertCompliance("<http://example.com?find=\\*>\n",
                "<p><a href=\"http://example.com?find=%5C*\">http://example.com?find=\\*</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void BackslashEscapes_Spec298()
        {
            // Line number in CommonMark Specs: 5485
            // Markdown: <a href=\"/bar\\/)\">\n
            // Expected HTML: <a href=\"/bar\\/)\">\n

            SpecTestHelper.AssertCompliance("<a href=\"/bar\\/)\">\n",
                "<a href=\"/bar\\/)\">\n",
                "all",
                true);
        }

        [Fact]
        public void BackslashEscapes_Spec299()
        {
            // Line number in CommonMark Specs: 5495
            // Markdown: [foo](/bar\\* \"ti\\*tle\")\n
            // Expected HTML: <p><a href=\"/bar*\" title=\"ti*tle\">foo</a></p>\n

            SpecTestHelper.AssertCompliance("[foo](/bar\\* \"ti\\*tle\")\n",
                "<p><a href=\"/bar*\" title=\"ti*tle\">foo</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void BackslashEscapes_Spec300()
        {
            // Line number in CommonMark Specs: 5502
            // Markdown: [foo]\n\n[foo]: /bar\\* \"ti\\*tle\"\n
            // Expected HTML: <p><a href=\"/bar*\" title=\"ti*tle\">foo</a></p>\n

            SpecTestHelper.AssertCompliance("[foo]\n\n[foo]: /bar\\* \"ti\\*tle\"\n",
                "<p><a href=\"/bar*\" title=\"ti*tle\">foo</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void BackslashEscapes_Spec301()
        {
            // Line number in CommonMark Specs: 5511
            // Markdown: ``` foo\\+bar\nfoo\n```\n
            // Expected HTML: <pre><code>foo\n</code></pre>\n

            SpecTestHelper.AssertCompliance("``` foo\\+bar\nfoo\n```\n",
                "<pre><code>foo\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void EntityAndNumericCharacterReferences_Spec302()
        {
            // Line number in CommonMark Specs: 5538
            // Markdown: &nbsp; &amp; &copy; &AElig; &Dcaron;\n&frac34; &HilbertSpace; &DifferentialD;\n&ClockwiseContourIntegral; &ngE;\n
            // Expected HTML: <p>  &amp; © Æ Ď\n¾ ℋ ⅆ\n∲ ≧̸</p>\n

            SpecTestHelper.AssertCompliance("&nbsp; &amp; &copy; &AElig; &Dcaron;\n&frac34; &HilbertSpace; &DifferentialD;\n&ClockwiseContourIntegral; &ngE;\n",
                "<p>  &amp; © Æ Ď\n¾ ℋ ⅆ\n∲ ≧̸</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EntityAndNumericCharacterReferences_Spec303()
        {
            // Line number in CommonMark Specs: 5557
            // Markdown: &#35; &#1234; &#992; &#98765432; &#0;\n
            // Expected HTML: <p># Ӓ Ϡ � �</p>\n

            SpecTestHelper.AssertCompliance("&#35; &#1234; &#992; &#98765432; &#0;\n",
                "<p># Ӓ Ϡ � �</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EntityAndNumericCharacterReferences_Spec304()
        {
            // Line number in CommonMark Specs: 5570
            // Markdown: &#X22; &#XD06; &#xcab;\n
            // Expected HTML: <p>&quot; ആ ಫ</p>\n

            SpecTestHelper.AssertCompliance("&#X22; &#XD06; &#xcab;\n",
                "<p>&quot; ആ ಫ</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EntityAndNumericCharacterReferences_Spec305()
        {
            // Line number in CommonMark Specs: 5579
            // Markdown: &nbsp &x; &#; &#x;\n&ThisIsNotDefined; &hi?;\n
            // Expected HTML: <p>&amp;nbsp &amp;x; &amp;#; &amp;#x;\n&amp;ThisIsNotDefined; &amp;hi?;</p>\n

            SpecTestHelper.AssertCompliance("&nbsp &x; &#; &#x;\n&ThisIsNotDefined; &hi?;\n",
                "<p>&amp;nbsp &amp;x; &amp;#; &amp;#x;\n&amp;ThisIsNotDefined; &amp;hi?;</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EntityAndNumericCharacterReferences_Spec306()
        {
            // Line number in CommonMark Specs: 5592
            // Markdown: &copy\n
            // Expected HTML: <p>&amp;copy</p>\n

            SpecTestHelper.AssertCompliance("&copy\n",
                "<p>&amp;copy</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EntityAndNumericCharacterReferences_Spec307()
        {
            // Line number in CommonMark Specs: 5602
            // Markdown: &MadeUpEntity;\n
            // Expected HTML: <p>&amp;MadeUpEntity;</p>\n

            SpecTestHelper.AssertCompliance("&MadeUpEntity;\n",
                "<p>&amp;MadeUpEntity;</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EntityAndNumericCharacterReferences_Spec308()
        {
            // Line number in CommonMark Specs: 5613
            // Markdown: <a href=\"&ouml;&ouml;.html\">\n
            // Expected HTML: <a href=\"&ouml;&ouml;.html\">\n

            SpecTestHelper.AssertCompliance("<a href=\"&ouml;&ouml;.html\">\n",
                "<a href=\"&ouml;&ouml;.html\">\n",
                "all",
                true);
        }

        [Fact]
        public void EntityAndNumericCharacterReferences_Spec309()
        {
            // Line number in CommonMark Specs: 5620
            // Markdown: [foo](/f&ouml;&ouml; \"f&ouml;&ouml;\")\n
            // Expected HTML: <p><a href=\"/f%C3%B6%C3%B6\" title=\"föö\">foo</a></p>\n

            SpecTestHelper.AssertCompliance("[foo](/f&ouml;&ouml; \"f&ouml;&ouml;\")\n",
                "<p><a href=\"/f%C3%B6%C3%B6\" title=\"föö\">foo</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EntityAndNumericCharacterReferences_Spec310()
        {
            // Line number in CommonMark Specs: 5627
            // Markdown: [foo]\n\n[foo]: /f&ouml;&ouml; \"f&ouml;&ouml;\"\n
            // Expected HTML: <p><a href=\"/f%C3%B6%C3%B6\" title=\"föö\">foo</a></p>\n

            SpecTestHelper.AssertCompliance("[foo]\n\n[foo]: /f&ouml;&ouml; \"f&ouml;&ouml;\"\n",
                "<p><a href=\"/f%C3%B6%C3%B6\" title=\"föö\">foo</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EntityAndNumericCharacterReferences_Spec311()
        {
            // Line number in CommonMark Specs: 5636
            // Markdown: ``` f&ouml;&ouml;\nfoo\n```\n
            // Expected HTML: <pre><code>foo\n</code></pre>\n

            SpecTestHelper.AssertCompliance("``` f&ouml;&ouml;\nfoo\n```\n",
                "<pre><code>foo\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void EntityAndNumericCharacterReferences_Spec312()
        {
            // Line number in CommonMark Specs: 5649
            // Markdown: `f&ouml;&ouml;`\n
            // Expected HTML: <p><code>f&amp;ouml;&amp;ouml;</code></p>\n

            SpecTestHelper.AssertCompliance("`f&ouml;&ouml;`\n",
                "<p><code>f&amp;ouml;&amp;ouml;</code></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EntityAndNumericCharacterReferences_Spec313()
        {
            // Line number in CommonMark Specs: 5656
            // Markdown:     f&ouml;f&ouml;\n
            // Expected HTML: <pre><code>f&amp;ouml;f&amp;ouml;\n</code></pre>\n

            SpecTestHelper.AssertCompliance("    f&ouml;f&ouml;\n",
                "<pre><code>f&amp;ouml;f&amp;ouml;\n</code></pre>\n",
                "all",
                true);
        }

        [Fact]
        public void CodeSpans_Spec314()
        {
            // Line number in CommonMark Specs: 5678
            // Markdown: `foo`\n
            // Expected HTML: <p><code>foo</code></p>\n

            SpecTestHelper.AssertCompliance("`foo`\n",
                "<p><code>foo</code></p>\n",
                "all",
                true);
        }

        [Fact]
        public void CodeSpans_Spec315()
        {
            // Line number in CommonMark Specs: 5688
            // Markdown: `` foo ` bar  ``\n
            // Expected HTML: <p><code>foo ` bar</code></p>\n

            SpecTestHelper.AssertCompliance("`` foo ` bar  ``\n",
                "<p><code>foo ` bar</code></p>\n",
                "all",
                true);
        }

        [Fact]
        public void CodeSpans_Spec316()
        {
            // Line number in CommonMark Specs: 5698
            // Markdown: ` `` `\n
            // Expected HTML: <p><code>``</code></p>\n

            SpecTestHelper.AssertCompliance("` `` `\n",
                "<p><code>``</code></p>\n",
                "all",
                true);
        }

        [Fact]
        public void CodeSpans_Spec317()
        {
            // Line number in CommonMark Specs: 5707
            // Markdown: ``\nfoo\n``\n
            // Expected HTML: <p><code>foo</code></p>\n

            SpecTestHelper.AssertCompliance("``\nfoo\n``\n",
                "<p><code>foo</code></p>\n",
                "all",
                true);
        }

        [Fact]
        public void CodeSpans_Spec318()
        {
            // Line number in CommonMark Specs: 5719
            // Markdown: `foo   bar\n  baz`\n
            // Expected HTML: <p><code>foo bar baz</code></p>\n

            SpecTestHelper.AssertCompliance("`foo   bar\n  baz`\n",
                "<p><code>foo bar baz</code></p>\n",
                "all",
                true);
        }

        [Fact]
        public void CodeSpans_Spec319()
        {
            // Line number in CommonMark Specs: 5730
            // Markdown: `a  b`\n
            // Expected HTML: <p><code>a  b</code></p>\n

            SpecTestHelper.AssertCompliance("`a  b`\n",
                "<p><code>a  b</code></p>\n",
                "all",
                true);
        }

        [Fact]
        public void CodeSpans_Spec320()
        {
            // Line number in CommonMark Specs: 5750
            // Markdown: `foo `` bar`\n
            // Expected HTML: <p><code>foo `` bar</code></p>\n

            SpecTestHelper.AssertCompliance("`foo `` bar`\n",
                "<p><code>foo `` bar</code></p>\n",
                "all",
                true);
        }

        [Fact]
        public void CodeSpans_Spec321()
        {
            // Line number in CommonMark Specs: 5760
            // Markdown: `foo\\`bar`\n
            // Expected HTML: <p><code>foo\\</code>bar`</p>\n

            SpecTestHelper.AssertCompliance("`foo\\`bar`\n",
                "<p><code>foo\\</code>bar`</p>\n",
                "all",
                true);
        }

        [Fact]
        public void CodeSpans_Spec322()
        {
            // Line number in CommonMark Specs: 5776
            // Markdown: *foo`*`\n
            // Expected HTML: <p>*foo<code>*</code></p>\n

            SpecTestHelper.AssertCompliance("*foo`*`\n",
                "<p>*foo<code>*</code></p>\n",
                "all",
                true);
        }

        [Fact]
        public void CodeSpans_Spec323()
        {
            // Line number in CommonMark Specs: 5785
            // Markdown: [not a `link](/foo`)\n
            // Expected HTML: <p>[not a <code>link](/foo</code>)</p>\n

            SpecTestHelper.AssertCompliance("[not a `link](/foo`)\n",
                "<p>[not a <code>link](/foo</code>)</p>\n",
                "all",
                true);
        }

        [Fact]
        public void CodeSpans_Spec324()
        {
            // Line number in CommonMark Specs: 5795
            // Markdown: `<a href=\"`\">`\n
            // Expected HTML: <p><code>&lt;a href=&quot;</code>&quot;&gt;`</p>\n

            SpecTestHelper.AssertCompliance("`<a href=\"`\">`\n",
                "<p><code>&lt;a href=&quot;</code>&quot;&gt;`</p>\n",
                "all",
                true);
        }

        [Fact]
        public void CodeSpans_Spec325()
        {
            // Line number in CommonMark Specs: 5804
            // Markdown: <a href=\"`\">`\n
            // Expected HTML: <p><a href=\"`\">`</p>\n

            SpecTestHelper.AssertCompliance("<a href=\"`\">`\n",
                "<p><a href=\"`\">`</p>\n",
                "all",
                true);
        }

        [Fact]
        public void CodeSpans_Spec326()
        {
            // Line number in CommonMark Specs: 5813
            // Markdown: `<http://foo.bar.`baz>`\n
            // Expected HTML: <p><code>&lt;http://foo.bar.</code>baz&gt;`</p>\n

            SpecTestHelper.AssertCompliance("`<http://foo.bar.`baz>`\n",
                "<p><code>&lt;http://foo.bar.</code>baz&gt;`</p>\n",
                "all",
                true);
        }

        [Fact]
        public void CodeSpans_Spec327()
        {
            // Line number in CommonMark Specs: 5822
            // Markdown: <http://foo.bar.`baz>`\n
            // Expected HTML: <p><a href=\"http://foo.bar.%60baz\">http://foo.bar.`baz</a>`</p>\n

            SpecTestHelper.AssertCompliance("<http://foo.bar.`baz>`\n",
                "<p><a href=\"http://foo.bar.%60baz\">http://foo.bar.`baz</a>`</p>\n",
                "all",
                true);
        }

        [Fact]
        public void CodeSpans_Spec328()
        {
            // Line number in CommonMark Specs: 5832
            // Markdown: ```foo``\n
            // Expected HTML: <p>```foo``</p>\n

            SpecTestHelper.AssertCompliance("```foo``\n",
                "<p>```foo``</p>\n",
                "all",
                true);
        }

        [Fact]
        public void CodeSpans_Spec329()
        {
            // Line number in CommonMark Specs: 5839
            // Markdown: `foo\n
            // Expected HTML: <p>`foo</p>\n

            SpecTestHelper.AssertCompliance("`foo\n",
                "<p>`foo</p>\n",
                "all",
                true);
        }

        [Fact]
        public void CodeSpans_Spec330()
        {
            // Line number in CommonMark Specs: 5848
            // Markdown: `foo``bar``\n
            // Expected HTML: <p>`foo<code>bar</code></p>\n

            SpecTestHelper.AssertCompliance("`foo``bar``\n",
                "<p>`foo<code>bar</code></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec331()
        {
            // Line number in CommonMark Specs: 6061
            // Markdown: *foo bar*\n
            // Expected HTML: <p><em>foo bar</em></p>\n

            SpecTestHelper.AssertCompliance("*foo bar*\n",
                "<p><em>foo bar</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec332()
        {
            // Line number in CommonMark Specs: 6071
            // Markdown: a * foo bar*\n
            // Expected HTML: <p>a * foo bar*</p>\n

            SpecTestHelper.AssertCompliance("a * foo bar*\n",
                "<p>a * foo bar*</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec333()
        {
            // Line number in CommonMark Specs: 6082
            // Markdown: a*\"foo\"*\n
            // Expected HTML: <p>a*&quot;foo&quot;*</p>\n

            SpecTestHelper.AssertCompliance("a*\"foo\"*\n",
                "<p>a*&quot;foo&quot;*</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec334()
        {
            // Line number in CommonMark Specs: 6091
            // Markdown: * a *\n
            // Expected HTML: <p>* a *</p>\n

            SpecTestHelper.AssertCompliance("* a *\n",
                "<p>* a *</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec335()
        {
            // Line number in CommonMark Specs: 6100
            // Markdown: foo*bar*\n
            // Expected HTML: <p>foo<em>bar</em></p>\n

            SpecTestHelper.AssertCompliance("foo*bar*\n",
                "<p>foo<em>bar</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec336()
        {
            // Line number in CommonMark Specs: 6107
            // Markdown: 5*6*78\n
            // Expected HTML: <p>5<em>6</em>78</p>\n

            SpecTestHelper.AssertCompliance("5*6*78\n",
                "<p>5<em>6</em>78</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec337()
        {
            // Line number in CommonMark Specs: 6116
            // Markdown: _foo bar_\n
            // Expected HTML: <p><em>foo bar</em></p>\n

            SpecTestHelper.AssertCompliance("_foo bar_\n",
                "<p><em>foo bar</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec338()
        {
            // Line number in CommonMark Specs: 6126
            // Markdown: _ foo bar_\n
            // Expected HTML: <p>_ foo bar_</p>\n

            SpecTestHelper.AssertCompliance("_ foo bar_\n",
                "<p>_ foo bar_</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec339()
        {
            // Line number in CommonMark Specs: 6136
            // Markdown: a_\"foo\"_\n
            // Expected HTML: <p>a_&quot;foo&quot;_</p>\n

            SpecTestHelper.AssertCompliance("a_\"foo\"_\n",
                "<p>a_&quot;foo&quot;_</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec340()
        {
            // Line number in CommonMark Specs: 6145
            // Markdown: foo_bar_\n
            // Expected HTML: <p>foo_bar_</p>\n

            SpecTestHelper.AssertCompliance("foo_bar_\n",
                "<p>foo_bar_</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec341()
        {
            // Line number in CommonMark Specs: 6152
            // Markdown: 5_6_78\n
            // Expected HTML: <p>5_6_78</p>\n

            SpecTestHelper.AssertCompliance("5_6_78\n",
                "<p>5_6_78</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec342()
        {
            // Line number in CommonMark Specs: 6159
            // Markdown: пристаням_стремятся_\n
            // Expected HTML: <p>пристаням_стремятся_</p>\n

            SpecTestHelper.AssertCompliance("пристаням_стремятся_\n",
                "<p>пристаням_стремятся_</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec343()
        {
            // Line number in CommonMark Specs: 6169
            // Markdown: aa_\"bb\"_cc\n
            // Expected HTML: <p>aa_&quot;bb&quot;_cc</p>\n

            SpecTestHelper.AssertCompliance("aa_\"bb\"_cc\n",
                "<p>aa_&quot;bb&quot;_cc</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec344()
        {
            // Line number in CommonMark Specs: 6180
            // Markdown: foo-_(bar)_\n
            // Expected HTML: <p>foo-<em>(bar)</em></p>\n

            SpecTestHelper.AssertCompliance("foo-_(bar)_\n",
                "<p>foo-<em>(bar)</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec345()
        {
            // Line number in CommonMark Specs: 6192
            // Markdown: _foo*\n
            // Expected HTML: <p>_foo*</p>\n

            SpecTestHelper.AssertCompliance("_foo*\n",
                "<p>_foo*</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec346()
        {
            // Line number in CommonMark Specs: 6202
            // Markdown: *foo bar *\n
            // Expected HTML: <p>*foo bar *</p>\n

            SpecTestHelper.AssertCompliance("*foo bar *\n",
                "<p>*foo bar *</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec347()
        {
            // Line number in CommonMark Specs: 6211
            // Markdown: *foo bar\n*\n
            // Expected HTML: <p>*foo bar\n*</p>\n

            SpecTestHelper.AssertCompliance("*foo bar\n*\n",
                "<p>*foo bar\n*</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec348()
        {
            // Line number in CommonMark Specs: 6224
            // Markdown: *(*foo)\n
            // Expected HTML: <p>*(*foo)</p>\n

            SpecTestHelper.AssertCompliance("*(*foo)\n",
                "<p>*(*foo)</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec349()
        {
            // Line number in CommonMark Specs: 6234
            // Markdown: *(*foo*)*\n
            // Expected HTML: <p><em>(<em>foo</em>)</em></p>\n

            SpecTestHelper.AssertCompliance("*(*foo*)*\n",
                "<p><em>(<em>foo</em>)</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec350()
        {
            // Line number in CommonMark Specs: 6243
            // Markdown: *foo*bar\n
            // Expected HTML: <p><em>foo</em>bar</p>\n

            SpecTestHelper.AssertCompliance("*foo*bar\n",
                "<p><em>foo</em>bar</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec351()
        {
            // Line number in CommonMark Specs: 6256
            // Markdown: _foo bar _\n
            // Expected HTML: <p>_foo bar _</p>\n

            SpecTestHelper.AssertCompliance("_foo bar _\n",
                "<p>_foo bar _</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec352()
        {
            // Line number in CommonMark Specs: 6266
            // Markdown: _(_foo)\n
            // Expected HTML: <p>_(_foo)</p>\n

            SpecTestHelper.AssertCompliance("_(_foo)\n",
                "<p>_(_foo)</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec353()
        {
            // Line number in CommonMark Specs: 6275
            // Markdown: _(_foo_)_\n
            // Expected HTML: <p><em>(<em>foo</em>)</em></p>\n

            SpecTestHelper.AssertCompliance("_(_foo_)_\n",
                "<p><em>(<em>foo</em>)</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec354()
        {
            // Line number in CommonMark Specs: 6284
            // Markdown: _foo_bar\n
            // Expected HTML: <p>_foo_bar</p>\n

            SpecTestHelper.AssertCompliance("_foo_bar\n",
                "<p>_foo_bar</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec355()
        {
            // Line number in CommonMark Specs: 6291
            // Markdown: _пристаням_стремятся\n
            // Expected HTML: <p>_пристаням_стремятся</p>\n

            SpecTestHelper.AssertCompliance("_пристаням_стремятся\n",
                "<p>_пристаням_стремятся</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec356()
        {
            // Line number in CommonMark Specs: 6298
            // Markdown: _foo_bar_baz_\n
            // Expected HTML: <p><em>foo_bar_baz</em></p>\n

            SpecTestHelper.AssertCompliance("_foo_bar_baz_\n",
                "<p><em>foo_bar_baz</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec357()
        {
            // Line number in CommonMark Specs: 6309
            // Markdown: _(bar)_.\n
            // Expected HTML: <p><em>(bar)</em>.</p>\n

            SpecTestHelper.AssertCompliance("_(bar)_.\n",
                "<p><em>(bar)</em>.</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec358()
        {
            // Line number in CommonMark Specs: 6318
            // Markdown: **foo bar**\n
            // Expected HTML: <p><strong>foo bar</strong></p>\n

            SpecTestHelper.AssertCompliance("**foo bar**\n",
                "<p><strong>foo bar</strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec359()
        {
            // Line number in CommonMark Specs: 6328
            // Markdown: ** foo bar**\n
            // Expected HTML: <p>** foo bar**</p>\n

            SpecTestHelper.AssertCompliance("** foo bar**\n",
                "<p>** foo bar**</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec360()
        {
            // Line number in CommonMark Specs: 6339
            // Markdown: a**\"foo\"**\n
            // Expected HTML: <p>a**&quot;foo&quot;**</p>\n

            SpecTestHelper.AssertCompliance("a**\"foo\"**\n",
                "<p>a**&quot;foo&quot;**</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec361()
        {
            // Line number in CommonMark Specs: 6348
            // Markdown: foo**bar**\n
            // Expected HTML: <p>foo<strong>bar</strong></p>\n

            SpecTestHelper.AssertCompliance("foo**bar**\n",
                "<p>foo<strong>bar</strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec362()
        {
            // Line number in CommonMark Specs: 6357
            // Markdown: __foo bar__\n
            // Expected HTML: <p><strong>foo bar</strong></p>\n

            SpecTestHelper.AssertCompliance("__foo bar__\n",
                "<p><strong>foo bar</strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec363()
        {
            // Line number in CommonMark Specs: 6367
            // Markdown: __ foo bar__\n
            // Expected HTML: <p>__ foo bar__</p>\n

            SpecTestHelper.AssertCompliance("__ foo bar__\n",
                "<p>__ foo bar__</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec364()
        {
            // Line number in CommonMark Specs: 6375
            // Markdown: __\nfoo bar__\n
            // Expected HTML: <p>__\nfoo bar__</p>\n

            SpecTestHelper.AssertCompliance("__\nfoo bar__\n",
                "<p>__\nfoo bar__</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec365()
        {
            // Line number in CommonMark Specs: 6387
            // Markdown: a__\"foo\"__\n
            // Expected HTML: <p>a__&quot;foo&quot;__</p>\n

            SpecTestHelper.AssertCompliance("a__\"foo\"__\n",
                "<p>a__&quot;foo&quot;__</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec366()
        {
            // Line number in CommonMark Specs: 6396
            // Markdown: foo__bar__\n
            // Expected HTML: <p>foo__bar__</p>\n

            SpecTestHelper.AssertCompliance("foo__bar__\n",
                "<p>foo__bar__</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec367()
        {
            // Line number in CommonMark Specs: 6403
            // Markdown: 5__6__78\n
            // Expected HTML: <p>5__6__78</p>\n

            SpecTestHelper.AssertCompliance("5__6__78\n",
                "<p>5__6__78</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec368()
        {
            // Line number in CommonMark Specs: 6410
            // Markdown: пристаням__стремятся__\n
            // Expected HTML: <p>пристаням__стремятся__</p>\n

            SpecTestHelper.AssertCompliance("пристаням__стремятся__\n",
                "<p>пристаням__стремятся__</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec369()
        {
            // Line number in CommonMark Specs: 6417
            // Markdown: __foo, __bar__, baz__\n
            // Expected HTML: <p><strong>foo, <strong>bar</strong>, baz</strong></p>\n

            SpecTestHelper.AssertCompliance("__foo, __bar__, baz__\n",
                "<p><strong>foo, <strong>bar</strong>, baz</strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec370()
        {
            // Line number in CommonMark Specs: 6428
            // Markdown: foo-__(bar)__\n
            // Expected HTML: <p>foo-<strong>(bar)</strong></p>\n

            SpecTestHelper.AssertCompliance("foo-__(bar)__\n",
                "<p>foo-<strong>(bar)</strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec371()
        {
            // Line number in CommonMark Specs: 6441
            // Markdown: **foo bar **\n
            // Expected HTML: <p>**foo bar **</p>\n

            SpecTestHelper.AssertCompliance("**foo bar **\n",
                "<p>**foo bar **</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec372()
        {
            // Line number in CommonMark Specs: 6454
            // Markdown: **(**foo)\n
            // Expected HTML: <p>**(**foo)</p>\n

            SpecTestHelper.AssertCompliance("**(**foo)\n",
                "<p>**(**foo)</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec373()
        {
            // Line number in CommonMark Specs: 6464
            // Markdown: *(**foo**)*\n
            // Expected HTML: <p><em>(<strong>foo</strong>)</em></p>\n

            SpecTestHelper.AssertCompliance("*(**foo**)*\n",
                "<p><em>(<strong>foo</strong>)</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec374()
        {
            // Line number in CommonMark Specs: 6471
            // Markdown: **Gomphocarpus (*Gomphocarpus physocarpus*, syn.\n*Asclepias physocarpa*)**\n
            // Expected HTML: <p><strong>Gomphocarpus (<em>Gomphocarpus physocarpus</em>, syn.\n<em>Asclepias physocarpa</em>)</strong></p>\n

            SpecTestHelper.AssertCompliance("**Gomphocarpus (*Gomphocarpus physocarpus*, syn.\n*Asclepias physocarpa*)**\n",
                "<p><strong>Gomphocarpus (<em>Gomphocarpus physocarpus</em>, syn.\n<em>Asclepias physocarpa</em>)</strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec375()
        {
            // Line number in CommonMark Specs: 6480
            // Markdown: **foo \"*bar*\" foo**\n
            // Expected HTML: <p><strong>foo &quot;<em>bar</em>&quot; foo</strong></p>\n

            SpecTestHelper.AssertCompliance("**foo \"*bar*\" foo**\n",
                "<p><strong>foo &quot;<em>bar</em>&quot; foo</strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec376()
        {
            // Line number in CommonMark Specs: 6489
            // Markdown: **foo**bar\n
            // Expected HTML: <p><strong>foo</strong>bar</p>\n

            SpecTestHelper.AssertCompliance("**foo**bar\n",
                "<p><strong>foo</strong>bar</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec377()
        {
            // Line number in CommonMark Specs: 6501
            // Markdown: __foo bar __\n
            // Expected HTML: <p>__foo bar __</p>\n

            SpecTestHelper.AssertCompliance("__foo bar __\n",
                "<p>__foo bar __</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec378()
        {
            // Line number in CommonMark Specs: 6511
            // Markdown: __(__foo)\n
            // Expected HTML: <p>__(__foo)</p>\n

            SpecTestHelper.AssertCompliance("__(__foo)\n",
                "<p>__(__foo)</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec379()
        {
            // Line number in CommonMark Specs: 6521
            // Markdown: _(__foo__)_\n
            // Expected HTML: <p><em>(<strong>foo</strong>)</em></p>\n

            SpecTestHelper.AssertCompliance("_(__foo__)_\n",
                "<p><em>(<strong>foo</strong>)</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec380()
        {
            // Line number in CommonMark Specs: 6530
            // Markdown: __foo__bar\n
            // Expected HTML: <p>__foo__bar</p>\n

            SpecTestHelper.AssertCompliance("__foo__bar\n",
                "<p>__foo__bar</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec381()
        {
            // Line number in CommonMark Specs: 6537
            // Markdown: __пристаням__стремятся\n
            // Expected HTML: <p>__пристаням__стремятся</p>\n

            SpecTestHelper.AssertCompliance("__пристаням__стремятся\n",
                "<p>__пристаням__стремятся</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec382()
        {
            // Line number in CommonMark Specs: 6544
            // Markdown: __foo__bar__baz__\n
            // Expected HTML: <p><strong>foo__bar__baz</strong></p>\n

            SpecTestHelper.AssertCompliance("__foo__bar__baz__\n",
                "<p><strong>foo__bar__baz</strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec383()
        {
            // Line number in CommonMark Specs: 6555
            // Markdown: __(bar)__.\n
            // Expected HTML: <p><strong>(bar)</strong>.</p>\n

            SpecTestHelper.AssertCompliance("__(bar)__.\n",
                "<p><strong>(bar)</strong>.</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec384()
        {
            // Line number in CommonMark Specs: 6567
            // Markdown: *foo [bar](/url)*\n
            // Expected HTML: <p><em>foo <a href=\"/url\">bar</a></em></p>\n

            SpecTestHelper.AssertCompliance("*foo [bar](/url)*\n",
                "<p><em>foo <a href=\"/url\">bar</a></em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec385()
        {
            // Line number in CommonMark Specs: 6574
            // Markdown: *foo\nbar*\n
            // Expected HTML: <p><em>foo\nbar</em></p>\n

            SpecTestHelper.AssertCompliance("*foo\nbar*\n",
                "<p><em>foo\nbar</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec386()
        {
            // Line number in CommonMark Specs: 6586
            // Markdown: _foo __bar__ baz_\n
            // Expected HTML: <p><em>foo <strong>bar</strong> baz</em></p>\n

            SpecTestHelper.AssertCompliance("_foo __bar__ baz_\n",
                "<p><em>foo <strong>bar</strong> baz</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec387()
        {
            // Line number in CommonMark Specs: 6593
            // Markdown: _foo _bar_ baz_\n
            // Expected HTML: <p><em>foo <em>bar</em> baz</em></p>\n

            SpecTestHelper.AssertCompliance("_foo _bar_ baz_\n",
                "<p><em>foo <em>bar</em> baz</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec388()
        {
            // Line number in CommonMark Specs: 6600
            // Markdown: __foo_ bar_\n
            // Expected HTML: <p><em><em>foo</em> bar</em></p>\n

            SpecTestHelper.AssertCompliance("__foo_ bar_\n",
                "<p><em><em>foo</em> bar</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec389()
        {
            // Line number in CommonMark Specs: 6607
            // Markdown: *foo *bar**\n
            // Expected HTML: <p><em>foo <em>bar</em></em></p>\n

            SpecTestHelper.AssertCompliance("*foo *bar**\n",
                "<p><em>foo <em>bar</em></em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec390()
        {
            // Line number in CommonMark Specs: 6614
            // Markdown: *foo **bar** baz*\n
            // Expected HTML: <p><em>foo <strong>bar</strong> baz</em></p>\n

            SpecTestHelper.AssertCompliance("*foo **bar** baz*\n",
                "<p><em>foo <strong>bar</strong> baz</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec391()
        {
            // Line number in CommonMark Specs: 6620
            // Markdown: *foo**bar**baz*\n
            // Expected HTML: <p><em>foo<strong>bar</strong>baz</em></p>\n

            SpecTestHelper.AssertCompliance("*foo**bar**baz*\n",
                "<p><em>foo<strong>bar</strong>baz</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec392()
        {
            // Line number in CommonMark Specs: 6645
            // Markdown: ***foo** bar*\n
            // Expected HTML: <p><em><strong>foo</strong> bar</em></p>\n

            SpecTestHelper.AssertCompliance("***foo** bar*\n",
                "<p><em><strong>foo</strong> bar</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec393()
        {
            // Line number in CommonMark Specs: 6652
            // Markdown: *foo **bar***\n
            // Expected HTML: <p><em>foo <strong>bar</strong></em></p>\n

            SpecTestHelper.AssertCompliance("*foo **bar***\n",
                "<p><em>foo <strong>bar</strong></em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec394()
        {
            // Line number in CommonMark Specs: 6659
            // Markdown: *foo**bar***\n
            // Expected HTML: <p><em>foo<strong>bar</strong></em></p>\n

            SpecTestHelper.AssertCompliance("*foo**bar***\n",
                "<p><em>foo<strong>bar</strong></em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec395()
        {
            // Line number in CommonMark Specs: 6668
            // Markdown: *foo **bar *baz* bim** bop*\n
            // Expected HTML: <p><em>foo <strong>bar <em>baz</em> bim</strong> bop</em></p>\n

            SpecTestHelper.AssertCompliance("*foo **bar *baz* bim** bop*\n",
                "<p><em>foo <strong>bar <em>baz</em> bim</strong> bop</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec396()
        {
            // Line number in CommonMark Specs: 6675
            // Markdown: *foo [*bar*](/url)*\n
            // Expected HTML: <p><em>foo <a href=\"/url\"><em>bar</em></a></em></p>\n

            SpecTestHelper.AssertCompliance("*foo [*bar*](/url)*\n",
                "<p><em>foo <a href=\"/url\"><em>bar</em></a></em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec397()
        {
            // Line number in CommonMark Specs: 6684
            // Markdown: ** is not an empty emphasis\n
            // Expected HTML: <p>** is not an empty emphasis</p>\n

            SpecTestHelper.AssertCompliance("** is not an empty emphasis\n",
                "<p>** is not an empty emphasis</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec398()
        {
            // Line number in CommonMark Specs: 6691
            // Markdown: **** is not an empty strong emphasis\n
            // Expected HTML: <p>**** is not an empty strong emphasis</p>\n

            SpecTestHelper.AssertCompliance("**** is not an empty strong emphasis\n",
                "<p>**** is not an empty strong emphasis</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec399()
        {
            // Line number in CommonMark Specs: 6704
            // Markdown: **foo [bar](/url)**\n
            // Expected HTML: <p><strong>foo <a href=\"/url\">bar</a></strong></p>\n

            SpecTestHelper.AssertCompliance("**foo [bar](/url)**\n",
                "<p><strong>foo <a href=\"/url\">bar</a></strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec400()
        {
            // Line number in CommonMark Specs: 6711
            // Markdown: **foo\nbar**\n
            // Expected HTML: <p><strong>foo\nbar</strong></p>\n

            SpecTestHelper.AssertCompliance("**foo\nbar**\n",
                "<p><strong>foo\nbar</strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec401()
        {
            // Line number in CommonMark Specs: 6723
            // Markdown: __foo _bar_ baz__\n
            // Expected HTML: <p><strong>foo <em>bar</em> baz</strong></p>\n

            SpecTestHelper.AssertCompliance("__foo _bar_ baz__\n",
                "<p><strong>foo <em>bar</em> baz</strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec402()
        {
            // Line number in CommonMark Specs: 6730
            // Markdown: __foo __bar__ baz__\n
            // Expected HTML: <p><strong>foo <strong>bar</strong> baz</strong></p>\n

            SpecTestHelper.AssertCompliance("__foo __bar__ baz__\n",
                "<p><strong>foo <strong>bar</strong> baz</strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec403()
        {
            // Line number in CommonMark Specs: 6737
            // Markdown: ____foo__ bar__\n
            // Expected HTML: <p><strong><strong>foo</strong> bar</strong></p>\n

            SpecTestHelper.AssertCompliance("____foo__ bar__\n",
                "<p><strong><strong>foo</strong> bar</strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec404()
        {
            // Line number in CommonMark Specs: 6744
            // Markdown: **foo **bar****\n
            // Expected HTML: <p><strong>foo <strong>bar</strong></strong></p>\n

            SpecTestHelper.AssertCompliance("**foo **bar****\n",
                "<p><strong>foo <strong>bar</strong></strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec405()
        {
            // Line number in CommonMark Specs: 6751
            // Markdown: **foo *bar* baz**\n
            // Expected HTML: <p><strong>foo <em>bar</em> baz</strong></p>\n

            SpecTestHelper.AssertCompliance("**foo *bar* baz**\n",
                "<p><strong>foo <em>bar</em> baz</strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec406()
        {
            // Line number in CommonMark Specs: 6758
            // Markdown: **foo*bar*baz**\n
            // Expected HTML: <p><strong>foo<em>bar</em>baz</strong></p>\n

            SpecTestHelper.AssertCompliance("**foo*bar*baz**\n",
                "<p><strong>foo<em>bar</em>baz</strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec407()
        {
            // Line number in CommonMark Specs: 6765
            // Markdown: ***foo* bar**\n
            // Expected HTML: <p><strong><em>foo</em> bar</strong></p>\n

            SpecTestHelper.AssertCompliance("***foo* bar**\n",
                "<p><strong><em>foo</em> bar</strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec408()
        {
            // Line number in CommonMark Specs: 6772
            // Markdown: **foo *bar***\n
            // Expected HTML: <p><strong>foo <em>bar</em></strong></p>\n

            SpecTestHelper.AssertCompliance("**foo *bar***\n",
                "<p><strong>foo <em>bar</em></strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec409()
        {
            // Line number in CommonMark Specs: 6781
            // Markdown: **foo *bar **baz**\nbim* bop**\n
            // Expected HTML: <p><strong>foo <em>bar <strong>baz</strong>\nbim</em> bop</strong></p>\n

            SpecTestHelper.AssertCompliance("**foo *bar **baz**\nbim* bop**\n",
                "<p><strong>foo <em>bar <strong>baz</strong>\nbim</em> bop</strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec410()
        {
            // Line number in CommonMark Specs: 6790
            // Markdown: **foo [*bar*](/url)**\n
            // Expected HTML: <p><strong>foo <a href=\"/url\"><em>bar</em></a></strong></p>\n

            SpecTestHelper.AssertCompliance("**foo [*bar*](/url)**\n",
                "<p><strong>foo <a href=\"/url\"><em>bar</em></a></strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec411()
        {
            // Line number in CommonMark Specs: 6799
            // Markdown: __ is not an empty emphasis\n
            // Expected HTML: <p>__ is not an empty emphasis</p>\n

            SpecTestHelper.AssertCompliance("__ is not an empty emphasis\n",
                "<p>__ is not an empty emphasis</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec412()
        {
            // Line number in CommonMark Specs: 6806
            // Markdown: ____ is not an empty strong emphasis\n
            // Expected HTML: <p>____ is not an empty strong emphasis</p>\n

            SpecTestHelper.AssertCompliance("____ is not an empty strong emphasis\n",
                "<p>____ is not an empty strong emphasis</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec413()
        {
            // Line number in CommonMark Specs: 6816
            // Markdown: foo ***\n
            // Expected HTML: <p>foo ***</p>\n

            SpecTestHelper.AssertCompliance("foo ***\n",
                "<p>foo ***</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec414()
        {
            // Line number in CommonMark Specs: 6823
            // Markdown: foo *\\**\n
            // Expected HTML: <p>foo <em>*</em></p>\n

            SpecTestHelper.AssertCompliance("foo *\\**\n",
                "<p>foo <em>*</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec415()
        {
            // Line number in CommonMark Specs: 6830
            // Markdown: foo *_*\n
            // Expected HTML: <p>foo <em>_</em></p>\n

            SpecTestHelper.AssertCompliance("foo *_*\n",
                "<p>foo <em>_</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec416()
        {
            // Line number in CommonMark Specs: 6837
            // Markdown: foo *****\n
            // Expected HTML: <p>foo *****</p>\n

            SpecTestHelper.AssertCompliance("foo *****\n",
                "<p>foo *****</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec417()
        {
            // Line number in CommonMark Specs: 6844
            // Markdown: foo **\\***\n
            // Expected HTML: <p>foo <strong>*</strong></p>\n

            SpecTestHelper.AssertCompliance("foo **\\***\n",
                "<p>foo <strong>*</strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec418()
        {
            // Line number in CommonMark Specs: 6851
            // Markdown: foo **_**\n
            // Expected HTML: <p>foo <strong>_</strong></p>\n

            SpecTestHelper.AssertCompliance("foo **_**\n",
                "<p>foo <strong>_</strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec419()
        {
            // Line number in CommonMark Specs: 6862
            // Markdown: **foo*\n
            // Expected HTML: <p>*<em>foo</em></p>\n

            SpecTestHelper.AssertCompliance("**foo*\n",
                "<p>*<em>foo</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec420()
        {
            // Line number in CommonMark Specs: 6869
            // Markdown: *foo**\n
            // Expected HTML: <p><em>foo</em>*</p>\n

            SpecTestHelper.AssertCompliance("*foo**\n",
                "<p><em>foo</em>*</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec421()
        {
            // Line number in CommonMark Specs: 6876
            // Markdown: ***foo**\n
            // Expected HTML: <p>*<strong>foo</strong></p>\n

            SpecTestHelper.AssertCompliance("***foo**\n",
                "<p>*<strong>foo</strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec422()
        {
            // Line number in CommonMark Specs: 6883
            // Markdown: ****foo*\n
            // Expected HTML: <p>***<em>foo</em></p>\n

            SpecTestHelper.AssertCompliance("****foo*\n",
                "<p>***<em>foo</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec423()
        {
            // Line number in CommonMark Specs: 6890
            // Markdown: **foo***\n
            // Expected HTML: <p><strong>foo</strong>*</p>\n

            SpecTestHelper.AssertCompliance("**foo***\n",
                "<p><strong>foo</strong>*</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec424()
        {
            // Line number in CommonMark Specs: 6897
            // Markdown: *foo****\n
            // Expected HTML: <p><em>foo</em>***</p>\n

            SpecTestHelper.AssertCompliance("*foo****\n",
                "<p><em>foo</em>***</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec425()
        {
            // Line number in CommonMark Specs: 6907
            // Markdown: foo ___\n
            // Expected HTML: <p>foo ___</p>\n

            SpecTestHelper.AssertCompliance("foo ___\n",
                "<p>foo ___</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec426()
        {
            // Line number in CommonMark Specs: 6914
            // Markdown: foo _\\__\n
            // Expected HTML: <p>foo <em>_</em></p>\n

            SpecTestHelper.AssertCompliance("foo _\\__\n",
                "<p>foo <em>_</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec427()
        {
            // Line number in CommonMark Specs: 6921
            // Markdown: foo _*_\n
            // Expected HTML: <p>foo <em>*</em></p>\n

            SpecTestHelper.AssertCompliance("foo _*_\n",
                "<p>foo <em>*</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec428()
        {
            // Line number in CommonMark Specs: 6928
            // Markdown: foo _____\n
            // Expected HTML: <p>foo _____</p>\n

            SpecTestHelper.AssertCompliance("foo _____\n",
                "<p>foo _____</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec429()
        {
            // Line number in CommonMark Specs: 6935
            // Markdown: foo __\\___\n
            // Expected HTML: <p>foo <strong>_</strong></p>\n

            SpecTestHelper.AssertCompliance("foo __\\___\n",
                "<p>foo <strong>_</strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec430()
        {
            // Line number in CommonMark Specs: 6942
            // Markdown: foo __*__\n
            // Expected HTML: <p>foo <strong>*</strong></p>\n

            SpecTestHelper.AssertCompliance("foo __*__\n",
                "<p>foo <strong>*</strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec431()
        {
            // Line number in CommonMark Specs: 6949
            // Markdown: __foo_\n
            // Expected HTML: <p>_<em>foo</em></p>\n

            SpecTestHelper.AssertCompliance("__foo_\n",
                "<p>_<em>foo</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec432()
        {
            // Line number in CommonMark Specs: 6960
            // Markdown: _foo__\n
            // Expected HTML: <p><em>foo</em>_</p>\n

            SpecTestHelper.AssertCompliance("_foo__\n",
                "<p><em>foo</em>_</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec433()
        {
            // Line number in CommonMark Specs: 6967
            // Markdown: ___foo__\n
            // Expected HTML: <p>_<strong>foo</strong></p>\n

            SpecTestHelper.AssertCompliance("___foo__\n",
                "<p>_<strong>foo</strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec434()
        {
            // Line number in CommonMark Specs: 6974
            // Markdown: ____foo_\n
            // Expected HTML: <p>___<em>foo</em></p>\n

            SpecTestHelper.AssertCompliance("____foo_\n",
                "<p>___<em>foo</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec435()
        {
            // Line number in CommonMark Specs: 6981
            // Markdown: __foo___\n
            // Expected HTML: <p><strong>foo</strong>_</p>\n

            SpecTestHelper.AssertCompliance("__foo___\n",
                "<p><strong>foo</strong>_</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec436()
        {
            // Line number in CommonMark Specs: 6988
            // Markdown: _foo____\n
            // Expected HTML: <p><em>foo</em>___</p>\n

            SpecTestHelper.AssertCompliance("_foo____\n",
                "<p><em>foo</em>___</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec437()
        {
            // Line number in CommonMark Specs: 6998
            // Markdown: **foo**\n
            // Expected HTML: <p><strong>foo</strong></p>\n

            SpecTestHelper.AssertCompliance("**foo**\n",
                "<p><strong>foo</strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec438()
        {
            // Line number in CommonMark Specs: 7005
            // Markdown: *_foo_*\n
            // Expected HTML: <p><em><em>foo</em></em></p>\n

            SpecTestHelper.AssertCompliance("*_foo_*\n",
                "<p><em><em>foo</em></em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec439()
        {
            // Line number in CommonMark Specs: 7012
            // Markdown: __foo__\n
            // Expected HTML: <p><strong>foo</strong></p>\n

            SpecTestHelper.AssertCompliance("__foo__\n",
                "<p><strong>foo</strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec440()
        {
            // Line number in CommonMark Specs: 7019
            // Markdown: _*foo*_\n
            // Expected HTML: <p><em><em>foo</em></em></p>\n

            SpecTestHelper.AssertCompliance("_*foo*_\n",
                "<p><em><em>foo</em></em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec441()
        {
            // Line number in CommonMark Specs: 7029
            // Markdown: ****foo****\n
            // Expected HTML: <p><strong><strong>foo</strong></strong></p>\n

            SpecTestHelper.AssertCompliance("****foo****\n",
                "<p><strong><strong>foo</strong></strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec442()
        {
            // Line number in CommonMark Specs: 7036
            // Markdown: ____foo____\n
            // Expected HTML: <p><strong><strong>foo</strong></strong></p>\n

            SpecTestHelper.AssertCompliance("____foo____\n",
                "<p><strong><strong>foo</strong></strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec443()
        {
            // Line number in CommonMark Specs: 7047
            // Markdown: ******foo******\n
            // Expected HTML: <p><strong><strong><strong>foo</strong></strong></strong></p>\n

            SpecTestHelper.AssertCompliance("******foo******\n",
                "<p><strong><strong><strong>foo</strong></strong></strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec444()
        {
            // Line number in CommonMark Specs: 7056
            // Markdown: ***foo***\n
            // Expected HTML: <p><em><strong>foo</strong></em></p>\n

            SpecTestHelper.AssertCompliance("***foo***\n",
                "<p><em><strong>foo</strong></em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec445()
        {
            // Line number in CommonMark Specs: 7063
            // Markdown: _____foo_____\n
            // Expected HTML: <p><em><strong><strong>foo</strong></strong></em></p>\n

            SpecTestHelper.AssertCompliance("_____foo_____\n",
                "<p><em><strong><strong>foo</strong></strong></em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec446()
        {
            // Line number in CommonMark Specs: 7072
            // Markdown: *foo _bar* baz_\n
            // Expected HTML: <p><em>foo _bar</em> baz_</p>\n

            SpecTestHelper.AssertCompliance("*foo _bar* baz_\n",
                "<p><em>foo _bar</em> baz_</p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec447()
        {
            // Line number in CommonMark Specs: 7079
            // Markdown: *foo __bar *baz bim__ bam*\n
            // Expected HTML: <p><em>foo <strong>bar *baz bim</strong> bam</em></p>\n

            SpecTestHelper.AssertCompliance("*foo __bar *baz bim__ bam*\n",
                "<p><em>foo <strong>bar *baz bim</strong> bam</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec448()
        {
            // Line number in CommonMark Specs: 7088
            // Markdown: **foo **bar baz**\n
            // Expected HTML: <p>**foo <strong>bar baz</strong></p>\n

            SpecTestHelper.AssertCompliance("**foo **bar baz**\n",
                "<p>**foo <strong>bar baz</strong></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec449()
        {
            // Line number in CommonMark Specs: 7095
            // Markdown: *foo *bar baz*\n
            // Expected HTML: <p>*foo <em>bar baz</em></p>\n

            SpecTestHelper.AssertCompliance("*foo *bar baz*\n",
                "<p>*foo <em>bar baz</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec450()
        {
            // Line number in CommonMark Specs: 7104
            // Markdown: *[bar*](/url)\n
            // Expected HTML: <p>*<a href=\"/url\">bar*</a></p>\n

            SpecTestHelper.AssertCompliance("*[bar*](/url)\n",
                "<p>*<a href=\"/url\">bar*</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec451()
        {
            // Line number in CommonMark Specs: 7111
            // Markdown: _foo [bar_](/url)\n
            // Expected HTML: <p>_foo <a href=\"/url\">bar_</a></p>\n

            SpecTestHelper.AssertCompliance("_foo [bar_](/url)\n",
                "<p>_foo <a href=\"/url\">bar_</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec452()
        {
            // Line number in CommonMark Specs: 7118
            // Markdown: *<img src=\"foo\" title=\"*\"/>\n
            // Expected HTML: <p>*<img src=\"foo\" title=\"*\"/></p>\n

            SpecTestHelper.AssertCompliance("*<img src=\"foo\" title=\"*\"/>\n",
                "<p>*<img src=\"foo\" title=\"*\"/></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec453()
        {
            // Line number in CommonMark Specs: 7125
            // Markdown: **<a href=\"**\">\n
            // Expected HTML: <p>**<a href=\"**\"></p>\n

            SpecTestHelper.AssertCompliance("**<a href=\"**\">\n",
                "<p>**<a href=\"**\"></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec454()
        {
            // Line number in CommonMark Specs: 7132
            // Markdown: __<a href=\"__\">\n
            // Expected HTML: <p>__<a href=\"__\"></p>\n

            SpecTestHelper.AssertCompliance("__<a href=\"__\">\n",
                "<p>__<a href=\"__\"></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec455()
        {
            // Line number in CommonMark Specs: 7139
            // Markdown: *a `*`*\n
            // Expected HTML: <p><em>a <code>*</code></em></p>\n

            SpecTestHelper.AssertCompliance("*a `*`*\n",
                "<p><em>a <code>*</code></em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec456()
        {
            // Line number in CommonMark Specs: 7146
            // Markdown: _a `_`_\n
            // Expected HTML: <p><em>a <code>_</code></em></p>\n

            SpecTestHelper.AssertCompliance("_a `_`_\n",
                "<p><em>a <code>_</code></em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec457()
        {
            // Line number in CommonMark Specs: 7153
            // Markdown: **a<http://foo.bar/?q=**>\n
            // Expected HTML: <p>**a<a href=\"http://foo.bar/?q=**\">http://foo.bar/?q=**</a></p>\n

            SpecTestHelper.AssertCompliance("**a<http://foo.bar/?q=**>\n",
                "<p>**a<a href=\"http://foo.bar/?q=**\">http://foo.bar/?q=**</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void EmphasisAndStrongEmphasis_Spec458()
        {
            // Line number in CommonMark Specs: 7160
            // Markdown: __a<http://foo.bar/?q=__>\n
            // Expected HTML: <p>__a<a href=\"http://foo.bar/?q=__\">http://foo.bar/?q=__</a></p>\n

            SpecTestHelper.AssertCompliance("__a<http://foo.bar/?q=__>\n",
                "<p>__a<a href=\"http://foo.bar/?q=__\">http://foo.bar/?q=__</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec459()
        {
            // Line number in CommonMark Specs: 7241
            // Markdown: [link](/uri \"title\")\n
            // Expected HTML: <p><a href=\"/uri\" title=\"title\">link</a></p>\n

            SpecTestHelper.AssertCompliance("[link](/uri \"title\")\n",
                "<p><a href=\"/uri\" title=\"title\">link</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec460()
        {
            // Line number in CommonMark Specs: 7250
            // Markdown: [link](/uri)\n
            // Expected HTML: <p><a href=\"/uri\">link</a></p>\n

            SpecTestHelper.AssertCompliance("[link](/uri)\n",
                "<p><a href=\"/uri\">link</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec461()
        {
            // Line number in CommonMark Specs: 7259
            // Markdown: [link]()\n
            // Expected HTML: <p><a href=\"\">link</a></p>\n

            SpecTestHelper.AssertCompliance("[link]()\n",
                "<p><a href=\"\">link</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec462()
        {
            // Line number in CommonMark Specs: 7266
            // Markdown: [link](<>)\n
            // Expected HTML: <p><a href=\"\">link</a></p>\n

            SpecTestHelper.AssertCompliance("[link](<>)\n",
                "<p><a href=\"\">link</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec463()
        {
            // Line number in CommonMark Specs: 7276
            // Markdown: [link](/my uri)\n
            // Expected HTML: <p>[link](/my uri)</p>\n

            SpecTestHelper.AssertCompliance("[link](/my uri)\n",
                "<p>[link](/my uri)</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec464()
        {
            // Line number in CommonMark Specs: 7283
            // Markdown: [link](</my uri>)\n
            // Expected HTML: <p>[link](&lt;/my uri&gt;)</p>\n

            SpecTestHelper.AssertCompliance("[link](</my uri>)\n",
                "<p>[link](&lt;/my uri&gt;)</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec465()
        {
            // Line number in CommonMark Specs: 7290
            // Markdown: [link](foo\nbar)\n
            // Expected HTML: <p>[link](foo\nbar)</p>\n

            SpecTestHelper.AssertCompliance("[link](foo\nbar)\n",
                "<p>[link](foo\nbar)</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec466()
        {
            // Line number in CommonMark Specs: 7299
            // Markdown: [link](<foo\nbar>)\n
            // Expected HTML: <p>[link](<foo\nbar>)</p>\n

            SpecTestHelper.AssertCompliance("[link](<foo\nbar>)\n",
                "<p>[link](<foo\nbar>)</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec467()
        {
            // Line number in CommonMark Specs: 7309
            // Markdown: [link](\\(foo\\))\n
            // Expected HTML: <p><a href=\"(foo)\">link</a></p>\n

            SpecTestHelper.AssertCompliance("[link](\\(foo\\))\n",
                "<p><a href=\"(foo)\">link</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec468()
        {
            // Line number in CommonMark Specs: 7318
            // Markdown: [link](foo(and(bar)))\n
            // Expected HTML: <p><a href=\"foo(and(bar))\">link</a></p>\n

            SpecTestHelper.AssertCompliance("[link](foo(and(bar)))\n",
                "<p><a href=\"foo(and(bar))\">link</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec469()
        {
            // Line number in CommonMark Specs: 7327
            // Markdown: [link](foo\\(and\\(bar\\))\n
            // Expected HTML: <p><a href=\"foo(and(bar)\">link</a></p>\n

            SpecTestHelper.AssertCompliance("[link](foo\\(and\\(bar\\))\n",
                "<p><a href=\"foo(and(bar)\">link</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec470()
        {
            // Line number in CommonMark Specs: 7334
            // Markdown: [link](<foo(and(bar)>)\n
            // Expected HTML: <p><a href=\"foo(and(bar)\">link</a></p>\n

            SpecTestHelper.AssertCompliance("[link](<foo(and(bar)>)\n",
                "<p><a href=\"foo(and(bar)\">link</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec471()
        {
            // Line number in CommonMark Specs: 7344
            // Markdown: [link](foo\\)\\:)\n
            // Expected HTML: <p><a href=\"foo):\">link</a></p>\n

            SpecTestHelper.AssertCompliance("[link](foo\\)\\:)\n",
                "<p><a href=\"foo):\">link</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec472()
        {
            // Line number in CommonMark Specs: 7353
            // Markdown: [link](#fragment)\n\n[link](http://example.com#fragment)\n\n[link](http://example.com?foo=3#frag)\n
            // Expected HTML: <p><a href=\"#fragment\">link</a></p>\n<p><a href=\"http://example.com#fragment\">link</a></p>\n<p><a href=\"http://example.com?foo=3#frag\">link</a></p>\n

            SpecTestHelper.AssertCompliance("[link](#fragment)\n\n[link](http://example.com#fragment)\n\n[link](http://example.com?foo=3#frag)\n",
                "<p><a href=\"#fragment\">link</a></p>\n<p><a href=\"http://example.com#fragment\">link</a></p>\n<p><a href=\"http://example.com?foo=3#frag\">link</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec473()
        {
            // Line number in CommonMark Specs: 7369
            // Markdown: [link](foo\\bar)\n
            // Expected HTML: <p><a href=\"foo%5Cbar\">link</a></p>\n

            SpecTestHelper.AssertCompliance("[link](foo\\bar)\n",
                "<p><a href=\"foo%5Cbar\">link</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec474()
        {
            // Line number in CommonMark Specs: 7385
            // Markdown: [link](foo%20b&auml;)\n
            // Expected HTML: <p><a href=\"foo%20b%C3%A4\">link</a></p>\n

            SpecTestHelper.AssertCompliance("[link](foo%20b&auml;)\n",
                "<p><a href=\"foo%20b%C3%A4\">link</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec475()
        {
            // Line number in CommonMark Specs: 7396
            // Markdown: [link](\"title\")\n
            // Expected HTML: <p><a href=\"%22title%22\">link</a></p>\n

            SpecTestHelper.AssertCompliance("[link](\"title\")\n",
                "<p><a href=\"%22title%22\">link</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec476()
        {
            // Line number in CommonMark Specs: 7405
            // Markdown: [link](/url \"title\")\n[link](/url 'title')\n[link](/url (title))\n
            // Expected HTML: <p><a href=\"/url\" title=\"title\">link</a>\n<a href=\"/url\" title=\"title\">link</a>\n<a href=\"/url\" title=\"title\">link</a></p>\n

            SpecTestHelper.AssertCompliance("[link](/url \"title\")\n[link](/url 'title')\n[link](/url (title))\n",
                "<p><a href=\"/url\" title=\"title\">link</a>\n<a href=\"/url\" title=\"title\">link</a>\n<a href=\"/url\" title=\"title\">link</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec477()
        {
            // Line number in CommonMark Specs: 7419
            // Markdown: [link](/url \"title \\\"&quot;\")\n
            // Expected HTML: <p><a href=\"/url\" title=\"title &quot;&quot;\">link</a></p>\n

            SpecTestHelper.AssertCompliance("[link](/url \"title \\\"&quot;\")\n",
                "<p><a href=\"/url\" title=\"title &quot;&quot;\">link</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec478()
        {
            // Line number in CommonMark Specs: 7429
            // Markdown: [link](/url \"title\")\n
            // Expected HTML: <p><a href=\"/url%C2%A0%22title%22\">link</a></p>\n

            SpecTestHelper.AssertCompliance("[link](/url \"title\")\n",
                "<p><a href=\"/url%C2%A0%22title%22\">link</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec479()
        {
            // Line number in CommonMark Specs: 7438
            // Markdown: [link](/url \"title \"and\" title\")\n
            // Expected HTML: <p>[link](/url &quot;title &quot;and&quot; title&quot;)</p>\n

            SpecTestHelper.AssertCompliance("[link](/url \"title \"and\" title\")\n",
                "<p>[link](/url &quot;title &quot;and&quot; title&quot;)</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec480()
        {
            // Line number in CommonMark Specs: 7447
            // Markdown: [link](/url 'title \"and\" title')\n
            // Expected HTML: <p><a href=\"/url\" title=\"title &quot;and&quot; title\">link</a></p>\n

            SpecTestHelper.AssertCompliance("[link](/url 'title \"and\" title')\n",
                "<p><a href=\"/url\" title=\"title &quot;and&quot; title\">link</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec481()
        {
            // Line number in CommonMark Specs: 7471
            // Markdown: [link](   /uri\n  \"title\"  )\n
            // Expected HTML: <p><a href=\"/uri\" title=\"title\">link</a></p>\n

            SpecTestHelper.AssertCompliance("[link](   /uri\n  \"title\"  )\n",
                "<p><a href=\"/uri\" title=\"title\">link</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec482()
        {
            // Line number in CommonMark Specs: 7482
            // Markdown: [link] (/uri)\n
            // Expected HTML: <p>[link] (/uri)</p>\n

            SpecTestHelper.AssertCompliance("[link] (/uri)\n",
                "<p>[link] (/uri)</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec483()
        {
            // Line number in CommonMark Specs: 7492
            // Markdown: [link [foo [bar]]](/uri)\n
            // Expected HTML: <p><a href=\"/uri\">link [foo [bar]]</a></p>\n

            SpecTestHelper.AssertCompliance("[link [foo [bar]]](/uri)\n",
                "<p><a href=\"/uri\">link [foo [bar]]</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec484()
        {
            // Line number in CommonMark Specs: 7499
            // Markdown: [link] bar](/uri)\n
            // Expected HTML: <p>[link] bar](/uri)</p>\n

            SpecTestHelper.AssertCompliance("[link] bar](/uri)\n",
                "<p>[link] bar](/uri)</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec485()
        {
            // Line number in CommonMark Specs: 7506
            // Markdown: [link [bar](/uri)\n
            // Expected HTML: <p>[link <a href=\"/uri\">bar</a></p>\n

            SpecTestHelper.AssertCompliance("[link [bar](/uri)\n",
                "<p>[link <a href=\"/uri\">bar</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec486()
        {
            // Line number in CommonMark Specs: 7513
            // Markdown: [link \\[bar](/uri)\n
            // Expected HTML: <p><a href=\"/uri\">link [bar</a></p>\n

            SpecTestHelper.AssertCompliance("[link \\[bar](/uri)\n",
                "<p><a href=\"/uri\">link [bar</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec487()
        {
            // Line number in CommonMark Specs: 7522
            // Markdown: [link *foo **bar** `#`*](/uri)\n
            // Expected HTML: <p><a href=\"/uri\">link <em>foo <strong>bar</strong> <code>#</code></em></a></p>\n

            SpecTestHelper.AssertCompliance("[link *foo **bar** `#`*](/uri)\n",
                "<p><a href=\"/uri\">link <em>foo <strong>bar</strong> <code>#</code></em></a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec488()
        {
            // Line number in CommonMark Specs: 7529
            // Markdown: [![moon](moon.jpg)](/uri)\n
            // Expected HTML: <p><a href=\"/uri\"><img src=\"moon.jpg\" alt=\"moon\" /></a></p>\n

            SpecTestHelper.AssertCompliance("[![moon](moon.jpg)](/uri)\n",
                "<p><a href=\"/uri\"><img src=\"moon.jpg\" alt=\"moon\" /></a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec489()
        {
            // Line number in CommonMark Specs: 7538
            // Markdown: [foo [bar](/uri)](/uri)\n
            // Expected HTML: <p>[foo <a href=\"/uri\">bar</a>](/uri)</p>\n

            SpecTestHelper.AssertCompliance("[foo [bar](/uri)](/uri)\n",
                "<p>[foo <a href=\"/uri\">bar</a>](/uri)</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec490()
        {
            // Line number in CommonMark Specs: 7545
            // Markdown: [foo *[bar [baz](/uri)](/uri)*](/uri)\n
            // Expected HTML: <p>[foo <em>[bar <a href=\"/uri\">baz</a>](/uri)</em>](/uri)</p>\n

            SpecTestHelper.AssertCompliance("[foo *[bar [baz](/uri)](/uri)*](/uri)\n",
                "<p>[foo <em>[bar <a href=\"/uri\">baz</a>](/uri)</em>](/uri)</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec491()
        {
            // Line number in CommonMark Specs: 7552
            // Markdown: ![[[foo](uri1)](uri2)](uri3)\n
            // Expected HTML: <p><img src=\"uri3\" alt=\"[foo](uri2)\" /></p>\n

            SpecTestHelper.AssertCompliance("![[[foo](uri1)](uri2)](uri3)\n",
                "<p><img src=\"uri3\" alt=\"[foo](uri2)\" /></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec492()
        {
            // Line number in CommonMark Specs: 7562
            // Markdown: *[foo*](/uri)\n
            // Expected HTML: <p>*<a href=\"/uri\">foo*</a></p>\n

            SpecTestHelper.AssertCompliance("*[foo*](/uri)\n",
                "<p>*<a href=\"/uri\">foo*</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec493()
        {
            // Line number in CommonMark Specs: 7569
            // Markdown: [foo *bar](baz*)\n
            // Expected HTML: <p><a href=\"baz*\">foo *bar</a></p>\n

            SpecTestHelper.AssertCompliance("[foo *bar](baz*)\n",
                "<p><a href=\"baz*\">foo *bar</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec494()
        {
            // Line number in CommonMark Specs: 7579
            // Markdown: *foo [bar* baz]\n
            // Expected HTML: <p><em>foo [bar</em> baz]</p>\n

            SpecTestHelper.AssertCompliance("*foo [bar* baz]\n",
                "<p><em>foo [bar</em> baz]</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec495()
        {
            // Line number in CommonMark Specs: 7589
            // Markdown: [foo <bar attr=\"](baz)\">\n
            // Expected HTML: <p>[foo <bar attr=\"](baz)\"></p>\n

            SpecTestHelper.AssertCompliance("[foo <bar attr=\"](baz)\">\n",
                "<p>[foo <bar attr=\"](baz)\"></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec496()
        {
            // Line number in CommonMark Specs: 7596
            // Markdown: [foo`](/uri)`\n
            // Expected HTML: <p>[foo<code>](/uri)</code></p>\n

            SpecTestHelper.AssertCompliance("[foo`](/uri)`\n",
                "<p>[foo<code>](/uri)</code></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec497()
        {
            // Line number in CommonMark Specs: 7603
            // Markdown: [foo<http://example.com/?search=](uri)>\n
            // Expected HTML: <p>[foo<a href=\"http://example.com/?search=%5D(uri)\">http://example.com/?search=](uri)</a></p>\n

            SpecTestHelper.AssertCompliance("[foo<http://example.com/?search=](uri)>\n",
                "<p>[foo<a href=\"http://example.com/?search=%5D(uri)\">http://example.com/?search=](uri)</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec498()
        {
            // Line number in CommonMark Specs: 7641
            // Markdown: [foo][bar]\n\n[bar]: /url \"title\"\n
            // Expected HTML: <p><a href=\"/url\" title=\"title\">foo</a></p>\n

            SpecTestHelper.AssertCompliance("[foo][bar]\n\n[bar]: /url \"title\"\n",
                "<p><a href=\"/url\" title=\"title\">foo</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec499()
        {
            // Line number in CommonMark Specs: 7656
            // Markdown: [link [foo [bar]]][ref]\n\n[ref]: /uri\n
            // Expected HTML: <p><a href=\"/uri\">link [foo [bar]]</a></p>\n

            SpecTestHelper.AssertCompliance("[link [foo [bar]]][ref]\n\n[ref]: /uri\n",
                "<p><a href=\"/uri\">link [foo [bar]]</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec500()
        {
            // Line number in CommonMark Specs: 7665
            // Markdown: [link \\[bar][ref]\n\n[ref]: /uri\n
            // Expected HTML: <p><a href=\"/uri\">link [bar</a></p>\n

            SpecTestHelper.AssertCompliance("[link \\[bar][ref]\n\n[ref]: /uri\n",
                "<p><a href=\"/uri\">link [bar</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec501()
        {
            // Line number in CommonMark Specs: 7676
            // Markdown: [link *foo **bar** `#`*][ref]\n\n[ref]: /uri\n
            // Expected HTML: <p><a href=\"/uri\">link <em>foo <strong>bar</strong> <code>#</code></em></a></p>\n

            SpecTestHelper.AssertCompliance("[link *foo **bar** `#`*][ref]\n\n[ref]: /uri\n",
                "<p><a href=\"/uri\">link <em>foo <strong>bar</strong> <code>#</code></em></a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec502()
        {
            // Line number in CommonMark Specs: 7685
            // Markdown: [![moon](moon.jpg)][ref]\n\n[ref]: /uri\n
            // Expected HTML: <p><a href=\"/uri\"><img src=\"moon.jpg\" alt=\"moon\" /></a></p>\n

            SpecTestHelper.AssertCompliance("[![moon](moon.jpg)][ref]\n\n[ref]: /uri\n",
                "<p><a href=\"/uri\"><img src=\"moon.jpg\" alt=\"moon\" /></a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec503()
        {
            // Line number in CommonMark Specs: 7696
            // Markdown: [foo [bar](/uri)][ref]\n\n[ref]: /uri\n
            // Expected HTML: <p>[foo <a href=\"/uri\">bar</a>]<a href=\"/uri\">ref</a></p>\n

            SpecTestHelper.AssertCompliance("[foo [bar](/uri)][ref]\n\n[ref]: /uri\n",
                "<p>[foo <a href=\"/uri\">bar</a>]<a href=\"/uri\">ref</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec504()
        {
            // Line number in CommonMark Specs: 7705
            // Markdown: [foo *bar [baz][ref]*][ref]\n\n[ref]: /uri\n
            // Expected HTML: <p>[foo <em>bar <a href=\"/uri\">baz</a></em>]<a href=\"/uri\">ref</a></p>\n

            SpecTestHelper.AssertCompliance("[foo *bar [baz][ref]*][ref]\n\n[ref]: /uri\n",
                "<p>[foo <em>bar <a href=\"/uri\">baz</a></em>]<a href=\"/uri\">ref</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec505()
        {
            // Line number in CommonMark Specs: 7720
            // Markdown: *[foo*][ref]\n\n[ref]: /uri\n
            // Expected HTML: <p>*<a href=\"/uri\">foo*</a></p>\n

            SpecTestHelper.AssertCompliance("*[foo*][ref]\n\n[ref]: /uri\n",
                "<p>*<a href=\"/uri\">foo*</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec506()
        {
            // Line number in CommonMark Specs: 7729
            // Markdown: [foo *bar][ref]\n\n[ref]: /uri\n
            // Expected HTML: <p><a href=\"/uri\">foo *bar</a></p>\n

            SpecTestHelper.AssertCompliance("[foo *bar][ref]\n\n[ref]: /uri\n",
                "<p><a href=\"/uri\">foo *bar</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec507()
        {
            // Line number in CommonMark Specs: 7741
            // Markdown: [foo <bar attr=\"][ref]\">\n\n[ref]: /uri\n
            // Expected HTML: <p>[foo <bar attr=\"][ref]\"></p>\n

            SpecTestHelper.AssertCompliance("[foo <bar attr=\"][ref]\">\n\n[ref]: /uri\n",
                "<p>[foo <bar attr=\"][ref]\"></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec508()
        {
            // Line number in CommonMark Specs: 7750
            // Markdown: [foo`][ref]`\n\n[ref]: /uri\n
            // Expected HTML: <p>[foo<code>][ref]</code></p>\n

            SpecTestHelper.AssertCompliance("[foo`][ref]`\n\n[ref]: /uri\n",
                "<p>[foo<code>][ref]</code></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec509()
        {
            // Line number in CommonMark Specs: 7759
            // Markdown: [foo<http://example.com/?search=][ref]>\n\n[ref]: /uri\n
            // Expected HTML: <p>[foo<a href=\"http://example.com/?search=%5D%5Bref%5D\">http://example.com/?search=][ref]</a></p>\n

            SpecTestHelper.AssertCompliance("[foo<http://example.com/?search=][ref]>\n\n[ref]: /uri\n",
                "<p>[foo<a href=\"http://example.com/?search=%5D%5Bref%5D\">http://example.com/?search=][ref]</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec510()
        {
            // Line number in CommonMark Specs: 7770
            // Markdown: [foo][BaR]\n\n[bar]: /url \"title\"\n
            // Expected HTML: <p><a href=\"/url\" title=\"title\">foo</a></p>\n

            SpecTestHelper.AssertCompliance("[foo][BaR]\n\n[bar]: /url \"title\"\n",
                "<p><a href=\"/url\" title=\"title\">foo</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec511()
        {
            // Line number in CommonMark Specs: 7781
            // Markdown: [Толпой][Толпой] is a Russian word.\n\n[ТОЛПОЙ]: /url\n
            // Expected HTML: <p><a href=\"/url\">Толпой</a> is a Russian word.</p>\n

            SpecTestHelper.AssertCompliance("[Толпой][Толпой] is a Russian word.\n\n[ТОЛПОЙ]: /url\n",
                "<p><a href=\"/url\">Толпой</a> is a Russian word.</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec512()
        {
            // Line number in CommonMark Specs: 7793
            // Markdown: [Foo\n  bar]: /url\n\n[Baz][Foo bar]\n
            // Expected HTML: <p><a href=\"/url\">Baz</a></p>\n

            SpecTestHelper.AssertCompliance("[Foo\n  bar]: /url\n\n[Baz][Foo bar]\n",
                "<p><a href=\"/url\">Baz</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec513()
        {
            // Line number in CommonMark Specs: 7806
            // Markdown: [foo] [bar]\n\n[bar]: /url \"title\"\n
            // Expected HTML: <p>[foo] <a href=\"/url\" title=\"title\">bar</a></p>\n

            SpecTestHelper.AssertCompliance("[foo] [bar]\n\n[bar]: /url \"title\"\n",
                "<p>[foo] <a href=\"/url\" title=\"title\">bar</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec514()
        {
            // Line number in CommonMark Specs: 7815
            // Markdown: [foo]\n[bar]\n\n[bar]: /url \"title\"\n
            // Expected HTML: <p>[foo]\n<a href=\"/url\" title=\"title\">bar</a></p>\n

            SpecTestHelper.AssertCompliance("[foo]\n[bar]\n\n[bar]: /url \"title\"\n",
                "<p>[foo]\n<a href=\"/url\" title=\"title\">bar</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec515()
        {
            // Line number in CommonMark Specs: 7856
            // Markdown: [foo]: /url1\n\n[foo]: /url2\n\n[bar][foo]\n
            // Expected HTML: <p><a href=\"/url1\">bar</a></p>\n

            SpecTestHelper.AssertCompliance("[foo]: /url1\n\n[foo]: /url2\n\n[bar][foo]\n",
                "<p><a href=\"/url1\">bar</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec516()
        {
            // Line number in CommonMark Specs: 7871
            // Markdown: [bar][foo\\!]\n\n[foo!]: /url\n
            // Expected HTML: <p>[bar][foo!]</p>\n

            SpecTestHelper.AssertCompliance("[bar][foo\\!]\n\n[foo!]: /url\n",
                "<p>[bar][foo!]</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec517()
        {
            // Line number in CommonMark Specs: 7883
            // Markdown: [foo][ref[]\n\n[ref[]: /uri\n
            // Expected HTML: <p>[foo][ref[]</p>\n<p>[ref[]: /uri</p>\n

            SpecTestHelper.AssertCompliance("[foo][ref[]\n\n[ref[]: /uri\n",
                "<p>[foo][ref[]</p>\n<p>[ref[]: /uri</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec518()
        {
            // Line number in CommonMark Specs: 7893
            // Markdown: [foo][ref[bar]]\n\n[ref[bar]]: /uri\n
            // Expected HTML: <p>[foo][ref[bar]]</p>\n<p>[ref[bar]]: /uri</p>\n

            SpecTestHelper.AssertCompliance("[foo][ref[bar]]\n\n[ref[bar]]: /uri\n",
                "<p>[foo][ref[bar]]</p>\n<p>[ref[bar]]: /uri</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec519()
        {
            // Line number in CommonMark Specs: 7903
            // Markdown: [[[foo]]]\n\n[[[foo]]]: /url\n
            // Expected HTML: <p>[[[foo]]]</p>\n<p>[[[foo]]]: /url</p>\n

            SpecTestHelper.AssertCompliance("[[[foo]]]\n\n[[[foo]]]: /url\n",
                "<p>[[[foo]]]</p>\n<p>[[[foo]]]: /url</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec520()
        {
            // Line number in CommonMark Specs: 7913
            // Markdown: [foo][ref\\[]\n\n[ref\\[]: /uri\n
            // Expected HTML: <p><a href=\"/uri\">foo</a></p>\n

            SpecTestHelper.AssertCompliance("[foo][ref\\[]\n\n[ref\\[]: /uri\n",
                "<p><a href=\"/uri\">foo</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec521()
        {
            // Line number in CommonMark Specs: 7924
            // Markdown: [bar\\\\]: /uri\n\n[bar\\\\]\n
            // Expected HTML: <p><a href=\"/uri\">bar\\</a></p>\n

            SpecTestHelper.AssertCompliance("[bar\\\\]: /uri\n\n[bar\\\\]\n",
                "<p><a href=\"/uri\">bar\\</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec522()
        {
            // Line number in CommonMark Specs: 7935
            // Markdown: []\n\n[]: /uri\n
            // Expected HTML: <p>[]</p>\n<p>[]: /uri</p>\n

            SpecTestHelper.AssertCompliance("[]\n\n[]: /uri\n",
                "<p>[]</p>\n<p>[]: /uri</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec523()
        {
            // Line number in CommonMark Specs: 7945
            // Markdown: [\n ]\n\n[\n ]: /uri\n
            // Expected HTML: <p>[\n]</p>\n<p>[\n]: /uri</p>\n

            SpecTestHelper.AssertCompliance("[\n ]\n\n[\n ]: /uri\n",
                "<p>[\n]</p>\n<p>[\n]: /uri</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec524()
        {
            // Line number in CommonMark Specs: 7968
            // Markdown: [foo][]\n\n[foo]: /url \"title\"\n
            // Expected HTML: <p><a href=\"/url\" title=\"title\">foo</a></p>\n

            SpecTestHelper.AssertCompliance("[foo][]\n\n[foo]: /url \"title\"\n",
                "<p><a href=\"/url\" title=\"title\">foo</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec525()
        {
            // Line number in CommonMark Specs: 7977
            // Markdown: [*foo* bar][]\n\n[*foo* bar]: /url \"title\"\n
            // Expected HTML: <p><a href=\"/url\" title=\"title\"><em>foo</em> bar</a></p>\n

            SpecTestHelper.AssertCompliance("[*foo* bar][]\n\n[*foo* bar]: /url \"title\"\n",
                "<p><a href=\"/url\" title=\"title\"><em>foo</em> bar</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec526()
        {
            // Line number in CommonMark Specs: 7988
            // Markdown: [Foo][]\n\n[foo]: /url \"title\"\n
            // Expected HTML: <p><a href=\"/url\" title=\"title\">Foo</a></p>\n

            SpecTestHelper.AssertCompliance("[Foo][]\n\n[foo]: /url \"title\"\n",
                "<p><a href=\"/url\" title=\"title\">Foo</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec527()
        {
            // Line number in CommonMark Specs: 8001
            // Markdown: [foo] \n[]\n\n[foo]: /url \"title\"\n
            // Expected HTML: <p><a href=\"/url\" title=\"title\">foo</a>\n[]</p>\n

            SpecTestHelper.AssertCompliance("[foo] \n[]\n\n[foo]: /url \"title\"\n",
                "<p><a href=\"/url\" title=\"title\">foo</a>\n[]</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec528()
        {
            // Line number in CommonMark Specs: 8021
            // Markdown: [foo]\n\n[foo]: /url \"title\"\n
            // Expected HTML: <p><a href=\"/url\" title=\"title\">foo</a></p>\n

            SpecTestHelper.AssertCompliance("[foo]\n\n[foo]: /url \"title\"\n",
                "<p><a href=\"/url\" title=\"title\">foo</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec529()
        {
            // Line number in CommonMark Specs: 8030
            // Markdown: [*foo* bar]\n\n[*foo* bar]: /url \"title\"\n
            // Expected HTML: <p><a href=\"/url\" title=\"title\"><em>foo</em> bar</a></p>\n

            SpecTestHelper.AssertCompliance("[*foo* bar]\n\n[*foo* bar]: /url \"title\"\n",
                "<p><a href=\"/url\" title=\"title\"><em>foo</em> bar</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec530()
        {
            // Line number in CommonMark Specs: 8039
            // Markdown: [[*foo* bar]]\n\n[*foo* bar]: /url \"title\"\n
            // Expected HTML: <p>[<a href=\"/url\" title=\"title\"><em>foo</em> bar</a>]</p>\n

            SpecTestHelper.AssertCompliance("[[*foo* bar]]\n\n[*foo* bar]: /url \"title\"\n",
                "<p>[<a href=\"/url\" title=\"title\"><em>foo</em> bar</a>]</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec531()
        {
            // Line number in CommonMark Specs: 8048
            // Markdown: [[bar [foo]\n\n[foo]: /url\n
            // Expected HTML: <p>[[bar <a href=\"/url\">foo</a></p>\n

            SpecTestHelper.AssertCompliance("[[bar [foo]\n\n[foo]: /url\n",
                "<p>[[bar <a href=\"/url\">foo</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec532()
        {
            // Line number in CommonMark Specs: 8059
            // Markdown: [Foo]\n\n[foo]: /url \"title\"\n
            // Expected HTML: <p><a href=\"/url\" title=\"title\">Foo</a></p>\n

            SpecTestHelper.AssertCompliance("[Foo]\n\n[foo]: /url \"title\"\n",
                "<p><a href=\"/url\" title=\"title\">Foo</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec533()
        {
            // Line number in CommonMark Specs: 8070
            // Markdown: [foo] bar\n\n[foo]: /url\n
            // Expected HTML: <p><a href=\"/url\">foo</a> bar</p>\n

            SpecTestHelper.AssertCompliance("[foo] bar\n\n[foo]: /url\n",
                "<p><a href=\"/url\">foo</a> bar</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec534()
        {
            // Line number in CommonMark Specs: 8082
            // Markdown: \\[foo]\n\n[foo]: /url \"title\"\n
            // Expected HTML: <p>[foo]</p>\n

            SpecTestHelper.AssertCompliance("\\[foo]\n\n[foo]: /url \"title\"\n",
                "<p>[foo]</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec535()
        {
            // Line number in CommonMark Specs: 8094
            // Markdown: [foo*]: /url\n\n*[foo*]\n
            // Expected HTML: <p>*<a href=\"/url\">foo*</a></p>\n

            SpecTestHelper.AssertCompliance("[foo*]: /url\n\n*[foo*]\n",
                "<p>*<a href=\"/url\">foo*</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec536()
        {
            // Line number in CommonMark Specs: 8106
            // Markdown: [foo][bar]\n\n[foo]: /url1\n[bar]: /url2\n
            // Expected HTML: <p><a href=\"/url2\">foo</a></p>\n

            SpecTestHelper.AssertCompliance("[foo][bar]\n\n[foo]: /url1\n[bar]: /url2\n",
                "<p><a href=\"/url2\">foo</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec537()
        {
            // Line number in CommonMark Specs: 8115
            // Markdown: [foo][]\n\n[foo]: /url1\n
            // Expected HTML: <p><a href=\"/url1\">foo</a></p>\n

            SpecTestHelper.AssertCompliance("[foo][]\n\n[foo]: /url1\n",
                "<p><a href=\"/url1\">foo</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec538()
        {
            // Line number in CommonMark Specs: 8125
            // Markdown: [foo]()\n\n[foo]: /url1\n
            // Expected HTML: <p><a href=\"\">foo</a></p>\n

            SpecTestHelper.AssertCompliance("[foo]()\n\n[foo]: /url1\n",
                "<p><a href=\"\">foo</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec539()
        {
            // Line number in CommonMark Specs: 8133
            // Markdown: [foo](not a link)\n\n[foo]: /url1\n
            // Expected HTML: <p><a href=\"/url1\">foo</a>(not a link)</p>\n

            SpecTestHelper.AssertCompliance("[foo](not a link)\n\n[foo]: /url1\n",
                "<p><a href=\"/url1\">foo</a>(not a link)</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec540()
        {
            // Line number in CommonMark Specs: 8144
            // Markdown: [foo][bar][baz]\n\n[baz]: /url\n
            // Expected HTML: <p>[foo]<a href=\"/url\">bar</a></p>\n

            SpecTestHelper.AssertCompliance("[foo][bar][baz]\n\n[baz]: /url\n",
                "<p>[foo]<a href=\"/url\">bar</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec541()
        {
            // Line number in CommonMark Specs: 8156
            // Markdown: [foo][bar][baz]\n\n[baz]: /url1\n[bar]: /url2\n
            // Expected HTML: <p><a href=\"/url2\">foo</a><a href=\"/url1\">baz</a></p>\n

            SpecTestHelper.AssertCompliance("[foo][bar][baz]\n\n[baz]: /url1\n[bar]: /url2\n",
                "<p><a href=\"/url2\">foo</a><a href=\"/url1\">baz</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Links_Spec542()
        {
            // Line number in CommonMark Specs: 8169
            // Markdown: [foo][bar][baz]\n\n[baz]: /url1\n[foo]: /url2\n
            // Expected HTML: <p>[foo]<a href=\"/url1\">bar</a></p>\n

            SpecTestHelper.AssertCompliance("[foo][bar][baz]\n\n[baz]: /url1\n[foo]: /url2\n",
                "<p>[foo]<a href=\"/url1\">bar</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Images_Spec543()
        {
            // Line number in CommonMark Specs: 8192
            // Markdown: ![foo](/url \"title\")\n
            // Expected HTML: <p><img src=\"/url\" alt=\"foo\" title=\"title\" /></p>\n

            SpecTestHelper.AssertCompliance("![foo](/url \"title\")\n",
                "<p><img src=\"/url\" alt=\"foo\" title=\"title\" /></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Images_Spec544()
        {
            // Line number in CommonMark Specs: 8199
            // Markdown: ![foo *bar*]\n\n[foo *bar*]: train.jpg \"train & tracks\"\n
            // Expected HTML: <p><img src=\"train.jpg\" alt=\"foo bar\" title=\"train &amp; tracks\" /></p>\n

            SpecTestHelper.AssertCompliance("![foo *bar*]\n\n[foo *bar*]: train.jpg \"train & tracks\"\n",
                "<p><img src=\"train.jpg\" alt=\"foo bar\" title=\"train &amp; tracks\" /></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Images_Spec545()
        {
            // Line number in CommonMark Specs: 8208
            // Markdown: ![foo ![bar](/url)](/url2)\n
            // Expected HTML: <p><img src=\"/url2\" alt=\"foo bar\" /></p>\n

            SpecTestHelper.AssertCompliance("![foo ![bar](/url)](/url2)\n",
                "<p><img src=\"/url2\" alt=\"foo bar\" /></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Images_Spec546()
        {
            // Line number in CommonMark Specs: 8215
            // Markdown: ![foo [bar](/url)](/url2)\n
            // Expected HTML: <p><img src=\"/url2\" alt=\"foo bar\" /></p>\n

            SpecTestHelper.AssertCompliance("![foo [bar](/url)](/url2)\n",
                "<p><img src=\"/url2\" alt=\"foo bar\" /></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Images_Spec547()
        {
            // Line number in CommonMark Specs: 8229
            // Markdown: ![foo *bar*][]\n\n[foo *bar*]: train.jpg \"train & tracks\"\n
            // Expected HTML: <p><img src=\"train.jpg\" alt=\"foo bar\" title=\"train &amp; tracks\" /></p>\n

            SpecTestHelper.AssertCompliance("![foo *bar*][]\n\n[foo *bar*]: train.jpg \"train & tracks\"\n",
                "<p><img src=\"train.jpg\" alt=\"foo bar\" title=\"train &amp; tracks\" /></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Images_Spec548()
        {
            // Line number in CommonMark Specs: 8238
            // Markdown: ![foo *bar*][foobar]\n\n[FOOBAR]: train.jpg \"train & tracks\"\n
            // Expected HTML: <p><img src=\"train.jpg\" alt=\"foo bar\" title=\"train &amp; tracks\" /></p>\n

            SpecTestHelper.AssertCompliance("![foo *bar*][foobar]\n\n[FOOBAR]: train.jpg \"train & tracks\"\n",
                "<p><img src=\"train.jpg\" alt=\"foo bar\" title=\"train &amp; tracks\" /></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Images_Spec549()
        {
            // Line number in CommonMark Specs: 8247
            // Markdown: ![foo](train.jpg)\n
            // Expected HTML: <p><img src=\"train.jpg\" alt=\"foo\" /></p>\n

            SpecTestHelper.AssertCompliance("![foo](train.jpg)\n",
                "<p><img src=\"train.jpg\" alt=\"foo\" /></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Images_Spec550()
        {
            // Line number in CommonMark Specs: 8254
            // Markdown: My ![foo bar](/path/to/train.jpg  \"title\"   )\n
            // Expected HTML: <p>My <img src=\"/path/to/train.jpg\" alt=\"foo bar\" title=\"title\" /></p>\n

            SpecTestHelper.AssertCompliance("My ![foo bar](/path/to/train.jpg  \"title\"   )\n",
                "<p>My <img src=\"/path/to/train.jpg\" alt=\"foo bar\" title=\"title\" /></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Images_Spec551()
        {
            // Line number in CommonMark Specs: 8261
            // Markdown: ![foo](<url>)\n
            // Expected HTML: <p><img src=\"url\" alt=\"foo\" /></p>\n

            SpecTestHelper.AssertCompliance("![foo](<url>)\n",
                "<p><img src=\"url\" alt=\"foo\" /></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Images_Spec552()
        {
            // Line number in CommonMark Specs: 8268
            // Markdown: ![](/url)\n
            // Expected HTML: <p><img src=\"/url\" alt=\"\" /></p>\n

            SpecTestHelper.AssertCompliance("![](/url)\n",
                "<p><img src=\"/url\" alt=\"\" /></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Images_Spec553()
        {
            // Line number in CommonMark Specs: 8277
            // Markdown: ![foo][bar]\n\n[bar]: /url\n
            // Expected HTML: <p><img src=\"/url\" alt=\"foo\" /></p>\n

            SpecTestHelper.AssertCompliance("![foo][bar]\n\n[bar]: /url\n",
                "<p><img src=\"/url\" alt=\"foo\" /></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Images_Spec554()
        {
            // Line number in CommonMark Specs: 8286
            // Markdown: ![foo][bar]\n\n[BAR]: /url\n
            // Expected HTML: <p><img src=\"/url\" alt=\"foo\" /></p>\n

            SpecTestHelper.AssertCompliance("![foo][bar]\n\n[BAR]: /url\n",
                "<p><img src=\"/url\" alt=\"foo\" /></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Images_Spec555()
        {
            // Line number in CommonMark Specs: 8297
            // Markdown: ![foo][]\n\n[foo]: /url \"title\"\n
            // Expected HTML: <p><img src=\"/url\" alt=\"foo\" title=\"title\" /></p>\n

            SpecTestHelper.AssertCompliance("![foo][]\n\n[foo]: /url \"title\"\n",
                "<p><img src=\"/url\" alt=\"foo\" title=\"title\" /></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Images_Spec556()
        {
            // Line number in CommonMark Specs: 8306
            // Markdown: ![*foo* bar][]\n\n[*foo* bar]: /url \"title\"\n
            // Expected HTML: <p><img src=\"/url\" alt=\"foo bar\" title=\"title\" /></p>\n

            SpecTestHelper.AssertCompliance("![*foo* bar][]\n\n[*foo* bar]: /url \"title\"\n",
                "<p><img src=\"/url\" alt=\"foo bar\" title=\"title\" /></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Images_Spec557()
        {
            // Line number in CommonMark Specs: 8317
            // Markdown: ![Foo][]\n\n[foo]: /url \"title\"\n
            // Expected HTML: <p><img src=\"/url\" alt=\"Foo\" title=\"title\" /></p>\n

            SpecTestHelper.AssertCompliance("![Foo][]\n\n[foo]: /url \"title\"\n",
                "<p><img src=\"/url\" alt=\"Foo\" title=\"title\" /></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Images_Spec558()
        {
            // Line number in CommonMark Specs: 8329
            // Markdown: ![foo] \n[]\n\n[foo]: /url \"title\"\n
            // Expected HTML: <p><img src=\"/url\" alt=\"foo\" title=\"title\" />\n[]</p>\n

            SpecTestHelper.AssertCompliance("![foo] \n[]\n\n[foo]: /url \"title\"\n",
                "<p><img src=\"/url\" alt=\"foo\" title=\"title\" />\n[]</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Images_Spec559()
        {
            // Line number in CommonMark Specs: 8342
            // Markdown: ![foo]\n\n[foo]: /url \"title\"\n
            // Expected HTML: <p><img src=\"/url\" alt=\"foo\" title=\"title\" /></p>\n

            SpecTestHelper.AssertCompliance("![foo]\n\n[foo]: /url \"title\"\n",
                "<p><img src=\"/url\" alt=\"foo\" title=\"title\" /></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Images_Spec560()
        {
            // Line number in CommonMark Specs: 8351
            // Markdown: ![*foo* bar]\n\n[*foo* bar]: /url \"title\"\n
            // Expected HTML: <p><img src=\"/url\" alt=\"foo bar\" title=\"title\" /></p>\n

            SpecTestHelper.AssertCompliance("![*foo* bar]\n\n[*foo* bar]: /url \"title\"\n",
                "<p><img src=\"/url\" alt=\"foo bar\" title=\"title\" /></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Images_Spec561()
        {
            // Line number in CommonMark Specs: 8362
            // Markdown: ![[foo]]\n\n[[foo]]: /url \"title\"\n
            // Expected HTML: <p>![[foo]]</p>\n<p>[[foo]]: /url &quot;title&quot;</p>\n

            SpecTestHelper.AssertCompliance("![[foo]]\n\n[[foo]]: /url \"title\"\n",
                "<p>![[foo]]</p>\n<p>[[foo]]: /url &quot;title&quot;</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Images_Spec562()
        {
            // Line number in CommonMark Specs: 8374
            // Markdown: ![Foo]\n\n[foo]: /url \"title\"\n
            // Expected HTML: <p><img src=\"/url\" alt=\"Foo\" title=\"title\" /></p>\n

            SpecTestHelper.AssertCompliance("![Foo]\n\n[foo]: /url \"title\"\n",
                "<p><img src=\"/url\" alt=\"Foo\" title=\"title\" /></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Images_Spec564()
        {
            // Line number in CommonMark Specs: 8398
            // Markdown: \\![foo]\n\n[foo]: /url \"title\"\n
            // Expected HTML: <p>!<a href=\"/url\" title=\"title\">foo</a></p>\n

            SpecTestHelper.AssertCompliance("\\![foo]\n\n[foo]: /url \"title\"\n",
                "<p>!<a href=\"/url\" title=\"title\">foo</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Autolinks_Spec565()
        {
            // Line number in CommonMark Specs: 8431
            // Markdown: <http://foo.bar.baz>\n
            // Expected HTML: <p><a href=\"http://foo.bar.baz\">http://foo.bar.baz</a></p>\n

            SpecTestHelper.AssertCompliance("<http://foo.bar.baz>\n",
                "<p><a href=\"http://foo.bar.baz\">http://foo.bar.baz</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Autolinks_Spec566()
        {
            // Line number in CommonMark Specs: 8438
            // Markdown: <http://foo.bar.baz/test?q=hello&id=22&boolean>\n
            // Expected HTML: <p><a href=\"http://foo.bar.baz/test?q=hello&amp;id=22&amp;boolean\">http://foo.bar.baz/test?q=hello&amp;id=22&amp;boolean</a></p>\n

            SpecTestHelper.AssertCompliance("<http://foo.bar.baz/test?q=hello&id=22&boolean>\n",
                "<p><a href=\"http://foo.bar.baz/test?q=hello&amp;id=22&amp;boolean\">http://foo.bar.baz/test?q=hello&amp;id=22&amp;boolean</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Autolinks_Spec567()
        {
            // Line number in CommonMark Specs: 8445
            // Markdown: <irc://foo.bar:2233/baz>\n
            // Expected HTML: <p><a href=\"irc://foo.bar:2233/baz\">irc://foo.bar:2233/baz</a></p>\n

            SpecTestHelper.AssertCompliance("<irc://foo.bar:2233/baz>\n",
                "<p><a href=\"irc://foo.bar:2233/baz\">irc://foo.bar:2233/baz</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Autolinks_Spec568()
        {
            // Line number in CommonMark Specs: 8454
            // Markdown: <MAILTO:FOO@BAR.BAZ>\n
            // Expected HTML: <p><a href=\"MAILTO:FOO@BAR.BAZ\">MAILTO:FOO@BAR.BAZ</a></p>\n

            SpecTestHelper.AssertCompliance("<MAILTO:FOO@BAR.BAZ>\n",
                "<p><a href=\"MAILTO:FOO@BAR.BAZ\">MAILTO:FOO@BAR.BAZ</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Autolinks_Spec569()
        {
            // Line number in CommonMark Specs: 8466
            // Markdown: <a+b+c:d>\n
            // Expected HTML: <p><a href=\"a+b+c:d\">a+b+c:d</a></p>\n

            SpecTestHelper.AssertCompliance("<a+b+c:d>\n",
                "<p><a href=\"a+b+c:d\">a+b+c:d</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Autolinks_Spec570()
        {
            // Line number in CommonMark Specs: 8473
            // Markdown: <made-up-scheme://foo,bar>\n
            // Expected HTML: <p><a href=\"made-up-scheme://foo,bar\">made-up-scheme://foo,bar</a></p>\n

            SpecTestHelper.AssertCompliance("<made-up-scheme://foo,bar>\n",
                "<p><a href=\"made-up-scheme://foo,bar\">made-up-scheme://foo,bar</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Autolinks_Spec571()
        {
            // Line number in CommonMark Specs: 8480
            // Markdown: <http://../>\n
            // Expected HTML: <p><a href=\"http://../\">http://../</a></p>\n

            SpecTestHelper.AssertCompliance("<http://../>\n",
                "<p><a href=\"http://../\">http://../</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Autolinks_Spec572()
        {
            // Line number in CommonMark Specs: 8487
            // Markdown: <localhost:5001/foo>\n
            // Expected HTML: <p><a href=\"localhost:5001/foo\">localhost:5001/foo</a></p>\n

            SpecTestHelper.AssertCompliance("<localhost:5001/foo>\n",
                "<p><a href=\"localhost:5001/foo\">localhost:5001/foo</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Autolinks_Spec573()
        {
            // Line number in CommonMark Specs: 8496
            // Markdown: <http://foo.bar/baz bim>\n
            // Expected HTML: <p>&lt;http://foo.bar/baz bim&gt;</p>\n

            SpecTestHelper.AssertCompliance("<http://foo.bar/baz bim>\n",
                "<p>&lt;http://foo.bar/baz bim&gt;</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Autolinks_Spec574()
        {
            // Line number in CommonMark Specs: 8505
            // Markdown: <http://example.com/\\[\\>\n
            // Expected HTML: <p><a href=\"http://example.com/%5C%5B%5C\">http://example.com/\\[\\</a></p>\n

            SpecTestHelper.AssertCompliance("<http://example.com/\\[\\>\n",
                "<p><a href=\"http://example.com/%5C%5B%5C\">http://example.com/\\[\\</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Autolinks_Spec575()
        {
            // Line number in CommonMark Specs: 8527
            // Markdown: <foo@bar.example.com>\n
            // Expected HTML: <p><a href=\"mailto:foo@bar.example.com\">foo@bar.example.com</a></p>\n

            SpecTestHelper.AssertCompliance("<foo@bar.example.com>\n",
                "<p><a href=\"mailto:foo@bar.example.com\">foo@bar.example.com</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Autolinks_Spec576()
        {
            // Line number in CommonMark Specs: 8534
            // Markdown: <foo+special@Bar.baz-bar0.com>\n
            // Expected HTML: <p><a href=\"mailto:foo+special@Bar.baz-bar0.com\">foo+special@Bar.baz-bar0.com</a></p>\n

            SpecTestHelper.AssertCompliance("<foo+special@Bar.baz-bar0.com>\n",
                "<p><a href=\"mailto:foo+special@Bar.baz-bar0.com\">foo+special@Bar.baz-bar0.com</a></p>\n",
                "all",
                true);
        }

        [Fact]
        public void Autolinks_Spec577()
        {
            // Line number in CommonMark Specs: 8543
            // Markdown: <foo\\+@bar.example.com>\n
            // Expected HTML: <p>&lt;foo+@bar.example.com&gt;</p>\n

            SpecTestHelper.AssertCompliance("<foo\\+@bar.example.com>\n",
                "<p>&lt;foo+@bar.example.com&gt;</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Autolinks_Spec578()
        {
            // Line number in CommonMark Specs: 8552
            // Markdown: <>\n
            // Expected HTML: <p>&lt;&gt;</p>\n

            SpecTestHelper.AssertCompliance("<>\n",
                "<p>&lt;&gt;</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Autolinks_Spec579()
        {
            // Line number in CommonMark Specs: 8559
            // Markdown: < http://foo.bar >\n
            // Expected HTML: <p>&lt; http://foo.bar &gt;</p>\n

            SpecTestHelper.AssertCompliance("< http://foo.bar >\n",
                "<p>&lt; http://foo.bar &gt;</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Autolinks_Spec580()
        {
            // Line number in CommonMark Specs: 8566
            // Markdown: <m:abc>\n
            // Expected HTML: <p>&lt;m:abc&gt;</p>\n

            SpecTestHelper.AssertCompliance("<m:abc>\n",
                "<p>&lt;m:abc&gt;</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Autolinks_Spec581()
        {
            // Line number in CommonMark Specs: 8573
            // Markdown: <foo.bar.baz>\n
            // Expected HTML: <p>&lt;foo.bar.baz&gt;</p>\n

            SpecTestHelper.AssertCompliance("<foo.bar.baz>\n",
                "<p>&lt;foo.bar.baz&gt;</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Autolinks_Spec582()
        {
            // Line number in CommonMark Specs: 8580
            // Markdown: http://example.com\n
            // Expected HTML: <p>http://example.com</p>\n

            SpecTestHelper.AssertCompliance("http://example.com\n",
                "<p>http://example.com</p>\n",
                "all",
                true);
        }

        [Fact]
        public void Autolinks_Spec583()
        {
            // Line number in CommonMark Specs: 8587
            // Markdown: foo@bar.example.com\n
            // Expected HTML: <p>foo@bar.example.com</p>\n

            SpecTestHelper.AssertCompliance("foo@bar.example.com\n",
                "<p>foo@bar.example.com</p>\n",
                "all",
                true);
        }

        [Fact]
        public void RawHTML_Spec584()
        {
            // Line number in CommonMark Specs: 8669
            // Markdown: <a><bab><c2c>\n
            // Expected HTML: <p><a><bab><c2c></p>\n

            SpecTestHelper.AssertCompliance("<a><bab><c2c>\n",
                "<p><a><bab><c2c></p>\n",
                "all",
                true);
        }

        [Fact]
        public void RawHTML_Spec585()
        {
            // Line number in CommonMark Specs: 8678
            // Markdown: <a/><b2/>\n
            // Expected HTML: <p><a/><b2/></p>\n

            SpecTestHelper.AssertCompliance("<a/><b2/>\n",
                "<p><a/><b2/></p>\n",
                "all",
                true);
        }

        [Fact]
        public void RawHTML_Spec586()
        {
            // Line number in CommonMark Specs: 8687
            // Markdown: <a  /><b2\ndata=\"foo\" >\n
            // Expected HTML: <p><a  /><b2\ndata=\"foo\" ></p>\n

            SpecTestHelper.AssertCompliance("<a  /><b2\ndata=\"foo\" >\n",
                "<p><a  /><b2\ndata=\"foo\" ></p>\n",
                "all",
                true);
        }

        [Fact]
        public void RawHTML_Spec587()
        {
            // Line number in CommonMark Specs: 8698
            // Markdown: <a foo=\"bar\" bam = 'baz <em>\"</em>'\n_boolean zoop:33=zoop:33 />\n
            // Expected HTML: <p><a foo=\"bar\" bam = 'baz <em>\"</em>'\n_boolean zoop:33=zoop:33 /></p>\n

            SpecTestHelper.AssertCompliance("<a foo=\"bar\" bam = 'baz <em>\"</em>'\n_boolean zoop:33=zoop:33 />\n",
                "<p><a foo=\"bar\" bam = 'baz <em>\"</em>'\n_boolean zoop:33=zoop:33 /></p>\n",
                "all",
                true);
        }

        [Fact]
        public void RawHTML_Spec588()
        {
            // Line number in CommonMark Specs: 8709
            // Markdown: Foo <responsive-image src=\"foo.jpg\" />\n
            // Expected HTML: <p>Foo <responsive-image src=\"foo.jpg\" /></p>\n

            SpecTestHelper.AssertCompliance("Foo <responsive-image src=\"foo.jpg\" />\n",
                "<p>Foo <responsive-image src=\"foo.jpg\" /></p>\n",
                "all",
                true);
        }

        [Fact]
        public void RawHTML_Spec589()
        {
            // Line number in CommonMark Specs: 8718
            // Markdown: <33> <__>\n
            // Expected HTML: <p>&lt;33&gt; &lt;__&gt;</p>\n

            SpecTestHelper.AssertCompliance("<33> <__>\n",
                "<p>&lt;33&gt; &lt;__&gt;</p>\n",
                "all",
                true);
        }

        [Fact]
        public void RawHTML_Spec590()
        {
            // Line number in CommonMark Specs: 8727
            // Markdown: <a h*#ref=\"hi\">\n
            // Expected HTML: <p>&lt;a h*#ref=&quot;hi&quot;&gt;</p>\n

            SpecTestHelper.AssertCompliance("<a h*#ref=\"hi\">\n",
                "<p>&lt;a h*#ref=&quot;hi&quot;&gt;</p>\n",
                "all",
                true);
        }

        [Fact]
        public void RawHTML_Spec591()
        {
            // Line number in CommonMark Specs: 8736
            // Markdown: <a href=\"hi'> <a href=hi'>\n
            // Expected HTML: <p>&lt;a href=&quot;hi'&gt; &lt;a href=hi'&gt;</p>\n

            SpecTestHelper.AssertCompliance("<a href=\"hi'> <a href=hi'>\n",
                "<p>&lt;a href=&quot;hi'&gt; &lt;a href=hi'&gt;</p>\n",
                "all",
                true);
        }

        [Fact]
        public void RawHTML_Spec592()
        {
            // Line number in CommonMark Specs: 8745
            // Markdown: < a><\nfoo><bar/ >\n
            // Expected HTML: <p>&lt; a&gt;&lt;\nfoo&gt;&lt;bar/ &gt;</p>\n

            SpecTestHelper.AssertCompliance("< a><\nfoo><bar/ >\n",
                "<p>&lt; a&gt;&lt;\nfoo&gt;&lt;bar/ &gt;</p>\n",
                "all",
                true);
        }

        [Fact]
        public void RawHTML_Spec593()
        {
            // Line number in CommonMark Specs: 8756
            // Markdown: <a href='bar'title=title>\n
            // Expected HTML: <p>&lt;a href='bar'title=title&gt;</p>\n

            SpecTestHelper.AssertCompliance("<a href='bar'title=title>\n",
                "<p>&lt;a href='bar'title=title&gt;</p>\n",
                "all",
                true);
        }

        [Fact]
        public void RawHTML_Spec594()
        {
            // Line number in CommonMark Specs: 8765
            // Markdown: </a></foo >\n
            // Expected HTML: <p></a></foo ></p>\n

            SpecTestHelper.AssertCompliance("</a></foo >\n",
                "<p></a></foo ></p>\n",
                "all",
                true);
        }

        [Fact]
        public void RawHTML_Spec595()
        {
            // Line number in CommonMark Specs: 8774
            // Markdown: </a href=\"foo\">\n
            // Expected HTML: <p>&lt;/a href=&quot;foo&quot;&gt;</p>\n

            SpecTestHelper.AssertCompliance("</a href=\"foo\">\n",
                "<p>&lt;/a href=&quot;foo&quot;&gt;</p>\n",
                "all",
                true);
        }

        [Fact]
        public void RawHTML_Spec596()
        {
            // Line number in CommonMark Specs: 8783
            // Markdown: foo <!-- this is a\ncomment - with hyphen -->\n
            // Expected HTML: <p>foo <!-- this is a\ncomment - with hyphen --></p>\n

            SpecTestHelper.AssertCompliance("foo <!-- this is a\ncomment - with hyphen -->\n",
                "<p>foo <!-- this is a\ncomment - with hyphen --></p>\n",
                "all",
                true);
        }

        [Fact]
        public void RawHTML_Spec597()
        {
            // Line number in CommonMark Specs: 8792
            // Markdown: foo <!-- not a comment -- two hyphens -->\n
            // Expected HTML: <p>foo &lt;!-- not a comment -- two hyphens --&gt;</p>\n

            SpecTestHelper.AssertCompliance("foo <!-- not a comment -- two hyphens -->\n",
                "<p>foo &lt;!-- not a comment -- two hyphens --&gt;</p>\n",
                "all",
                true);
        }

        [Fact]
        public void RawHTML_Spec598()
        {
            // Line number in CommonMark Specs: 8801
            // Markdown: foo <!--> foo -->\n\nfoo <!-- foo--->\n
            // Expected HTML: <p>foo &lt;!--&gt; foo --&gt;</p>\n<p>foo &lt;!-- foo---&gt;</p>\n

            SpecTestHelper.AssertCompliance("foo <!--> foo -->\n\nfoo <!-- foo--->\n",
                "<p>foo &lt;!--&gt; foo --&gt;</p>\n<p>foo &lt;!-- foo---&gt;</p>\n",
                "all",
                true);
        }

        [Fact]
        public void RawHTML_Spec599()
        {
            // Line number in CommonMark Specs: 8813
            // Markdown: foo <?php echo $a; ?>\n
            // Expected HTML: <p>foo <?php echo $a; ?></p>\n

            SpecTestHelper.AssertCompliance("foo <?php echo $a; ?>\n",
                "<p>foo <?php echo $a; ?></p>\n",
                "all",
                true);
        }

        [Fact]
        public void RawHTML_Spec600()
        {
            // Line number in CommonMark Specs: 8822
            // Markdown: foo <!ELEMENT br EMPTY>\n
            // Expected HTML: <p>foo <!ELEMENT br EMPTY></p>\n

            SpecTestHelper.AssertCompliance("foo <!ELEMENT br EMPTY>\n",
                "<p>foo <!ELEMENT br EMPTY></p>\n",
                "all",
                true);
        }

        [Fact]
        public void RawHTML_Spec601()
        {
            // Line number in CommonMark Specs: 8831
            // Markdown: foo <![CDATA[>&<]]>\n
            // Expected HTML: <p>foo <![CDATA[>&<]]></p>\n

            SpecTestHelper.AssertCompliance("foo <![CDATA[>&<]]>\n",
                "<p>foo <![CDATA[>&<]]></p>\n",
                "all",
                true);
        }

        [Fact]
        public void RawHTML_Spec602()
        {
            // Line number in CommonMark Specs: 8841
            // Markdown: foo <a href=\"&ouml;\">\n
            // Expected HTML: <p>foo <a href=\"&ouml;\"></p>\n

            SpecTestHelper.AssertCompliance("foo <a href=\"&ouml;\">\n",
                "<p>foo <a href=\"&ouml;\"></p>\n",
                "all",
                true);
        }

        [Fact]
        public void RawHTML_Spec603()
        {
            // Line number in CommonMark Specs: 8850
            // Markdown: foo <a href=\"\\*\">\n
            // Expected HTML: <p>foo <a href=\"\\*\"></p>\n

            SpecTestHelper.AssertCompliance("foo <a href=\"\\*\">\n",
                "<p>foo <a href=\"\\*\"></p>\n",
                "all",
                true);
        }

        [Fact]
        public void RawHTML_Spec604()
        {
            // Line number in CommonMark Specs: 8857
            // Markdown: <a href=\"\\\"\">\n
            // Expected HTML: <p>&lt;a href=&quot;&quot;&quot;&gt;</p>\n

            SpecTestHelper.AssertCompliance("<a href=\"\\\"\">\n",
                "<p>&lt;a href=&quot;&quot;&quot;&gt;</p>\n",
                "all",
                true);
        }

        [Fact]
        public void HardLineBreaks_Spec605()
        {
            // Line number in CommonMark Specs: 8871
            // Markdown: foo  \nbaz\n
            // Expected HTML: <p>foo<br />\nbaz</p>\n

            SpecTestHelper.AssertCompliance("foo  \nbaz\n",
                "<p>foo<br />\nbaz</p>\n",
                "all",
                true);
        }

        [Fact]
        public void HardLineBreaks_Spec606()
        {
            // Line number in CommonMark Specs: 8883
            // Markdown: foo\\\nbaz\n
            // Expected HTML: <p>foo<br />\nbaz</p>\n

            SpecTestHelper.AssertCompliance("foo\\\nbaz\n",
                "<p>foo<br />\nbaz</p>\n",
                "all",
                true);
        }

        [Fact]
        public void HardLineBreaks_Spec607()
        {
            // Line number in CommonMark Specs: 8894
            // Markdown: foo       \nbaz\n
            // Expected HTML: <p>foo<br />\nbaz</p>\n

            SpecTestHelper.AssertCompliance("foo       \nbaz\n",
                "<p>foo<br />\nbaz</p>\n",
                "all",
                true);
        }

        [Fact]
        public void HardLineBreaks_Spec608()
        {
            // Line number in CommonMark Specs: 8905
            // Markdown: foo  \n     bar\n
            // Expected HTML: <p>foo<br />\nbar</p>\n

            SpecTestHelper.AssertCompliance("foo  \n     bar\n",
                "<p>foo<br />\nbar</p>\n",
                "all",
                true);
        }

        [Fact]
        public void HardLineBreaks_Spec609()
        {
            // Line number in CommonMark Specs: 8914
            // Markdown: foo\\\n     bar\n
            // Expected HTML: <p>foo<br />\nbar</p>\n

            SpecTestHelper.AssertCompliance("foo\\\n     bar\n",
                "<p>foo<br />\nbar</p>\n",
                "all",
                true);
        }

        [Fact]
        public void HardLineBreaks_Spec610()
        {
            // Line number in CommonMark Specs: 8926
            // Markdown: *foo  \nbar*\n
            // Expected HTML: <p><em>foo<br />\nbar</em></p>\n

            SpecTestHelper.AssertCompliance("*foo  \nbar*\n",
                "<p><em>foo<br />\nbar</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void HardLineBreaks_Spec611()
        {
            // Line number in CommonMark Specs: 8935
            // Markdown: *foo\\\nbar*\n
            // Expected HTML: <p><em>foo<br />\nbar</em></p>\n

            SpecTestHelper.AssertCompliance("*foo\\\nbar*\n",
                "<p><em>foo<br />\nbar</em></p>\n",
                "all",
                true);
        }

        [Fact]
        public void HardLineBreaks_Spec612()
        {
            // Line number in CommonMark Specs: 8946
            // Markdown: `code  \nspan`\n
            // Expected HTML: <p><code>code span</code></p>\n

            SpecTestHelper.AssertCompliance("`code  \nspan`\n",
                "<p><code>code span</code></p>\n",
                "all",
                true);
        }

        [Fact]
        public void HardLineBreaks_Spec613()
        {
            // Line number in CommonMark Specs: 8954
            // Markdown: `code\\\nspan`\n
            // Expected HTML: <p><code>code\\ span</code></p>\n

            SpecTestHelper.AssertCompliance("`code\\\nspan`\n",
                "<p><code>code\\ span</code></p>\n",
                "all",
                true);
        }

        [Fact]
        public void HardLineBreaks_Spec614()
        {
            // Line number in CommonMark Specs: 8964
            // Markdown: <a href=\"foo  \nbar\">\n
            // Expected HTML: <p><a href=\"foo  \nbar\"></p>\n

            SpecTestHelper.AssertCompliance("<a href=\"foo  \nbar\">\n",
                "<p><a href=\"foo  \nbar\"></p>\n",
                "all",
                true);
        }

        [Fact]
        public void HardLineBreaks_Spec615()
        {
            // Line number in CommonMark Specs: 8973
            // Markdown: <a href=\"foo\\\nbar\">\n
            // Expected HTML: <p><a href=\"foo\\\nbar\"></p>\n

            SpecTestHelper.AssertCompliance("<a href=\"foo\\\nbar\">\n",
                "<p><a href=\"foo\\\nbar\"></p>\n",
                "all",
                true);
        }

        [Fact]
        public void HardLineBreaks_Spec616()
        {
            // Line number in CommonMark Specs: 8986
            // Markdown: foo\\\n
            // Expected HTML: <p>foo\\</p>\n

            SpecTestHelper.AssertCompliance("foo\\\n",
                "<p>foo\\</p>\n",
                "all",
                true);
        }

        [Fact]
        public void HardLineBreaks_Spec617()
        {
            // Line number in CommonMark Specs: 8993
            // Markdown: foo  \n
            // Expected HTML: <p>foo</p>\n

            SpecTestHelper.AssertCompliance("foo  \n",
                "<p>foo</p>\n",
                "all",
                true);
        }

        [Fact]
        public void HardLineBreaks_Spec618()
        {
            // Line number in CommonMark Specs: 9000
            // Markdown: ### foo\\\n
            // Expected HTML: <h3>foo\\</h3>\n

            SpecTestHelper.AssertCompliance("### foo\\\n",
                "<h3>foo\\</h3>\n",
                "all",
                true);
        }

        [Fact]
        public void HardLineBreaks_Spec619()
        {
            // Line number in CommonMark Specs: 9007
            // Markdown: ### foo  \n
            // Expected HTML: <h3>foo</h3>\n

            SpecTestHelper.AssertCompliance("### foo  \n",
                "<h3>foo</h3>\n",
                "all",
                true);
        }

        [Fact]
        public void SoftLineBreaks_Spec620()
        {
            // Line number in CommonMark Specs: 9022
            // Markdown: foo\nbaz\n
            // Expected HTML: <p>foo\nbaz</p>\n

            SpecTestHelper.AssertCompliance("foo\nbaz\n",
                "<p>foo\nbaz</p>\n",
                "all",
                true);
        }

        [Fact]
        public void SoftLineBreaks_Spec621()
        {
            // Line number in CommonMark Specs: 9034
            // Markdown: foo \n baz\n
            // Expected HTML: <p>foo\nbaz</p>\n

            SpecTestHelper.AssertCompliance("foo \n baz\n",
                "<p>foo\nbaz</p>\n",
                "all",
                true);
        }

        [Fact]
        public void TextualContent_Spec622()
        {
            // Line number in CommonMark Specs: 9054
            // Markdown: hello $.;'there\n
            // Expected HTML: <p>hello $.;'there</p>\n

            SpecTestHelper.AssertCompliance("hello $.;'there\n",
                "<p>hello $.;'there</p>\n",
                "all",
                true);
        }

        [Fact]
        public void TextualContent_Spec623()
        {
            // Line number in CommonMark Specs: 9061
            // Markdown: Foo χρῆν\n
            // Expected HTML: <p>Foo χρῆν</p>\n

            SpecTestHelper.AssertCompliance("Foo χρῆν\n",
                "<p>Foo χρῆν</p>\n",
                "all",
                true);
        }

        [Fact]
        public void TextualContent_Spec624()
        {
            // Line number in CommonMark Specs: 9070
            // Markdown: Multiple     spaces\n
            // Expected HTML: <p>Multiple     spaces</p>\n

            SpecTestHelper.AssertCompliance("Multiple     spaces\n",
                "<p>Multiple     spaces</p>\n",
                "all",
                true);
        }
    }
}

