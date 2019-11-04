using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.Specs
{
    public class FlexiOptionsBlocksSpecs
    {
        [Theory]
        [InlineData("All")]
        public void FlexiOptionsBlocks_Spec1(string extensions)
        {
            //     Start line number: 34
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     o{ "title": "ExampleDocument.cs" }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_has-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title">ExampleDocument.cs</span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + &quot;dummyString&quot;;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"title\": \"ExampleDocument.cs\" }\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div class=\"flexi-code flexi-code_has-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\">ExampleDocument.cs</span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">public string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + &quot;dummyString&quot;;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("All")]
        public void FlexiOptionsBlocks_Spec2(string extensions)
        {
            //     Start line number: 65
            //     --------------- Extra Extensions ---------------
            //     FlexiAlertBlocks
            //     --------------- Markdown ---------------
            //     o{
            //         "type": "warning"
            //     }
            //     ! This is a FlexiAlertBlock.
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-alert flexi-alert_type_warning flexi-alert_has-icon">
            //     <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>This is a FlexiAlertBlock.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("o{\n    \"type\": \"warning\"\n}\n! This is a FlexiAlertBlock.",
                "<div class=\"flexi-alert flexi-alert_type_warning flexi-alert_has-icon\">\n<svg class=\"flexi-alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>This is a FlexiAlertBlock.</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("All")]
        public void FlexiOptionsBlocks_Spec3(string extensions)
        {
            //     Start line number: 85
            //     --------------- Extra Extensions ---------------
            //     FlexiTableBlocks
            //     --------------- Markdown ---------------
            //     o{
            //         "attributes": {
            //             "id" : "table-1"
            //         }
            //     }
            //     +---+---+
            //     | a | b |
            //     +===+===+
            //     | 0 | 1 |
            //     +---+---+
            //     | 2 | 3 |
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards" id="table-1">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     a
            //     </th>
            //     <th class="flexi-table__header">
            //     b
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     a
            //     </div>
            //     <div class="flexi-table__content">
            //     0
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     b
            //     </div>
            //     <div class="flexi-table__content">
            //     1
            //     </div>
            //     </td>
            //     </tr>
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     a
            //     </div>
            //     <div class="flexi-table__content">
            //     2
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     b
            //     </div>
            //     <div class="flexi-table__content">
            //     3
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("o{\n    \"attributes\": {\n        \"id\" : \"table-1\"\n    }\n}\n+---+---+\n| a | b |\n+===+===+\n| 0 | 1 |\n+---+---+\n| 2 | 3 |",
                "<div class=\"flexi-table flexi-table_type_cards\" id=\"table-1\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\na\n</th>\n<th class=\"flexi-table__header\">\nb\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\na\n</div>\n<div class=\"flexi-table__content\">\n0\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nb\n</div>\n<div class=\"flexi-table__content\">\n1\n</div>\n</td>\n</tr>\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\na\n</div>\n<div class=\"flexi-table__content\">\n2\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nb\n</div>\n<div class=\"flexi-table__content\">\n3\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("All")]
        public void FlexiOptionsBlocks_Spec4(string extensions)
        {
            //     Start line number: 166
            //     --------------- Extra Extensions ---------------
            //     FlexiSectionBlocks
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiSectionBlocks": {
            //             "defaultBlockOptions": {
            //                 "element": "nav"
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     # foo
            //     
            //     o{
            //         "element": "article"
            //     }
            //     # foo
            //     --------------- Expected Markup ---------------
            //     <nav class="flexi-section flexi-section_level_1 flexi-section_has-link-icon" id="foo">
            //     <header class="flexi-section__header">
            //     <h1 class="flexi-section__heading">foo</h1>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     </nav>
            //     <article class="flexi-section flexi-section_level_1 flexi-section_has-link-icon" id="foo-1">
            //     <header class="flexi-section__header">
            //     <h1 class="flexi-section__heading">foo</h1>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     </article>

            SpecTestHelper.AssertCompliance("# foo\n\no{\n    \"element\": \"article\"\n}\n# foo",
                "<nav class=\"flexi-section flexi-section_level_1 flexi-section_has-link-icon\" id=\"foo\">\n<header class=\"flexi-section__header\">\n<h1 class=\"flexi-section__heading\">foo</h1>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n</nav>\n<article class=\"flexi-section flexi-section_level_1 flexi-section_has-link-icon\" id=\"foo-1\">\n<header class=\"flexi-section__header\">\n<h1 class=\"flexi-section__heading\">foo</h1>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n</article>",
                extensions,
                false,
                "{\n    \"flexiSectionBlocks\": {\n        \"defaultBlockOptions\": {\n            \"element\": \"nav\"\n        }\n    }\n}");
        }
    }

    public class FlexiAlertBlocksSpecs
    {
        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec1(string extensions)
        {
            //     Start line number: 36
            //     --------------- Markdown ---------------
            //     ! This is a FlexiAlertBlock.
            //     ! This is important information.
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon">
            //     <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>This is a FlexiAlertBlock.
            //     This is important information.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! This is a FlexiAlertBlock.\n! This is important information.",
                "<div class=\"flexi-alert flexi-alert_type_info flexi-alert_has-icon\">\n<svg class=\"flexi-alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>This is a FlexiAlertBlock.\nThis is important information.</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec2(string extensions)
        {
            //     Start line number: 57
            //     --------------- Markdown ---------------
            //     !This line will render identically to the next line.
            //     ! This line will render identically to the previous line.
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon">
            //     <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>This line will render identically to the next line.
            //     This line will render identically to the previous line.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("!This line will render identically to the next line.\n! This line will render identically to the previous line.",
                "<div class=\"flexi-alert flexi-alert_type_info flexi-alert_has-icon\">\n<svg class=\"flexi-alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>This line will render identically to the next line.\nThis line will render identically to the previous line.</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec3(string extensions)
        {
            //     Start line number: 73
            //     --------------- Markdown ---------------
            //     ! These lines belong to the same FlexiAlertBlock.
            //      ! These lines belong to the same FlexiAlertBlock.
            //       ! These lines belong to the same FlexiAlertBlock.
            //        ! These lines belong to the same FlexiAlertBlock.
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon">
            //     <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>These lines belong to the same FlexiAlertBlock.
            //     These lines belong to the same FlexiAlertBlock.
            //     These lines belong to the same FlexiAlertBlock.
            //     These lines belong to the same FlexiAlertBlock.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! These lines belong to the same FlexiAlertBlock.\n ! These lines belong to the same FlexiAlertBlock.\n  ! These lines belong to the same FlexiAlertBlock.\n   ! These lines belong to the same FlexiAlertBlock.",
                "<div class=\"flexi-alert flexi-alert_type_info flexi-alert_has-icon\">\n<svg class=\"flexi-alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>These lines belong to the same FlexiAlertBlock.\nThese lines belong to the same FlexiAlertBlock.\nThese lines belong to the same FlexiAlertBlock.\nThese lines belong to the same FlexiAlertBlock.</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec4(string extensions)
        {
            //     Start line number: 93
            //     --------------- Markdown ---------------
            //     ! This FlexiAlertBlock
            //     contains multiple
            //     lazy continuation lines.
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon">
            //     <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>This FlexiAlertBlock
            //     contains multiple
            //     lazy continuation lines.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! This FlexiAlertBlock\ncontains multiple\nlazy continuation lines.",
                "<div class=\"flexi-alert flexi-alert_type_info flexi-alert_has-icon\">\n<svg class=\"flexi-alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>This FlexiAlertBlock\ncontains multiple\nlazy continuation lines.</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec5(string extensions)
        {
            //     Start line number: 111
            //     --------------- Markdown ---------------
            //     ! This is a FlexiAlertBlock.
            //     
            //     ! This is another FlexiAlertBlock.
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon">
            //     <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>This is a FlexiAlertBlock.</p>
            //     </div>
            //     </div>
            //     <div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon">
            //     <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>This is another FlexiAlertBlock.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! This is a FlexiAlertBlock.\n\n! This is another FlexiAlertBlock.",
                "<div class=\"flexi-alert flexi-alert_type_info flexi-alert_has-icon\">\n<svg class=\"flexi-alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>This is a FlexiAlertBlock.</p>\n</div>\n</div>\n<div class=\"flexi-alert flexi-alert_type_info flexi-alert_has-icon\">\n<svg class=\"flexi-alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>This is another FlexiAlertBlock.</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec6(string extensions)
        {
            //     Start line number: 133
            //     --------------- Markdown ---------------
            //     ![This is an image](/url)
            //     
            //     ![This is neither an image nor a FlexiAlertBlock. Whether or not a line is a valid image, if it begins with `![`, it does not start a FlexiAlertBlock.
            //     --------------- Expected Markup ---------------
            //     <p><img src="/url" alt="This is an image" /></p>
            //     <p>![This is neither an image nor a FlexiAlertBlock. Whether or not a line is a valid image, if it begins with <code>![</code>, it does not start a FlexiAlertBlock.</p>

            SpecTestHelper.AssertCompliance("![This is an image](/url)\n\n![This is neither an image nor a FlexiAlertBlock. Whether or not a line is a valid image, if it begins with `![`, it does not start a FlexiAlertBlock.",
                "<p><img src=\"/url\" alt=\"This is an image\" /></p>\n<p>![This is neither an image nor a FlexiAlertBlock. Whether or not a line is a valid image, if it begins with <code>![</code>, it does not start a FlexiAlertBlock.</p>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec7(string extensions)
        {
            //     Start line number: 145
            //     --------------- Markdown ---------------
            //     ! [This is a FlexiAlertBlock]
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon">
            //     <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>[This is a FlexiAlertBlock]</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! [This is a FlexiAlertBlock]",
                "<div class=\"flexi-alert flexi-alert_type_info flexi-alert_has-icon\">\n<svg class=\"flexi-alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>[This is a FlexiAlertBlock]</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec8(string extensions)
        {
            //     Start line number: 159
            //     --------------- Markdown ---------------
            //     ! This is a FlexiAlertBlock
            //     ![This is an image in a FlexiAlertBlock](/url)
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon">
            //     <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>This is a FlexiAlertBlock
            //     <img src="/url" alt="This is an image in a FlexiAlertBlock" /></p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! This is a FlexiAlertBlock\n![This is an image in a FlexiAlertBlock](/url)",
                "<div class=\"flexi-alert flexi-alert_type_info flexi-alert_has-icon\">\n<svg class=\"flexi-alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>This is a FlexiAlertBlock\n<img src=\"/url\" alt=\"This is an image in a FlexiAlertBlock\" /></p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec9(string extensions)
        {
            //     Start line number: 187
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "blockName": "alert" }
            //     ! This is a FlexiAlertBlock.
            //     --------------- Expected Markup ---------------
            //     <div class="alert alert_type_info alert_has-icon">
            //     <svg class="alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
            //     <div class="alert__content">
            //     <p>This is a FlexiAlertBlock.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"blockName\": \"alert\" }\n! This is a FlexiAlertBlock.",
                "<div class=\"alert alert_type_info alert_has-icon\">\n<svg class=\"alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"/></svg>\n<div class=\"alert__content\">\n<p>This is a FlexiAlertBlock.</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec10(string extensions)
        {
            //     Start line number: 214
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "type": "warning" }
            //     ! This is a FlexiAlertBlock.
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-alert flexi-alert_type_warning flexi-alert_has-icon">
            //     <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>This is a FlexiAlertBlock.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"type\": \"warning\" }\n! This is a FlexiAlertBlock.",
                "<div class=\"flexi-alert flexi-alert_type_warning flexi-alert_has-icon\">\n<svg class=\"flexi-alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>This is a FlexiAlertBlock.</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec11(string extensions)
        {
            //     Start line number: 237
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "icon": "<svg><use xlink:href=\"#alert-icon\"/></svg>" }
            //     ! This is a FlexiAlertBlock.
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon">
            //     <svg class="flexi-alert__icon"><use xlink:href="#alert-icon"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>This is a FlexiAlertBlock.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"icon\": \"<svg><use xlink:href=\\\"#alert-icon\\\"/></svg>\" }\n! This is a FlexiAlertBlock.",
                "<div class=\"flexi-alert flexi-alert_type_info flexi-alert_has-icon\">\n<svg class=\"flexi-alert__icon\"><use xlink:href=\"#alert-icon\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>This is a FlexiAlertBlock.</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec12(string extensions)
        {
            //     Start line number: 252
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{
            //         "icon": null,
            //         "type": "no-default-icon"
            //     }
            //     ! This is a FlexiAlertBlock.
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-alert flexi-alert_type_no-default-icon flexi-alert_no-icon">
            //     <div class="flexi-alert__content">
            //     <p>This is a FlexiAlertBlock.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("o{\n    \"icon\": null,\n    \"type\": \"no-default-icon\"\n}\n! This is a FlexiAlertBlock.",
                "<div class=\"flexi-alert flexi-alert_type_no-default-icon flexi-alert_no-icon\">\n<div class=\"flexi-alert__content\">\n<p>This is a FlexiAlertBlock.</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec13(string extensions)
        {
            //     Start line number: 277
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{
            //         "attributes": {
            //             "id" : "my-custom-id",
            //             "class" : "my-custom-class"
            //         }
            //     }
            //     ! This is a FlexiAlertBlock.
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon my-custom-class" id="my-custom-id">
            //     <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>This is a FlexiAlertBlock.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("o{\n    \"attributes\": {\n        \"id\" : \"my-custom-id\",\n        \"class\" : \"my-custom-class\"\n    }\n}\n! This is a FlexiAlertBlock.",
                "<div class=\"flexi-alert flexi-alert_type_info flexi-alert_has-icon my-custom-class\" id=\"my-custom-id\">\n<svg class=\"flexi-alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>This is a FlexiAlertBlock.</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec14(string extensions)
        {
            //     Start line number: 312
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiAlertBlocks": {
            //             "defaultBlockOptions": {
            //                 "icon": "<svg><use xlink:href=\"#alert-icon\"/></svg>",
            //                 "attributes": {
            //                     "class": "block"
            //                 }
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     ! This is a FlexiAlertBlock.
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon block">
            //     <svg class="flexi-alert__icon"><use xlink:href="#alert-icon"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>This is a FlexiAlertBlock.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! This is a FlexiAlertBlock.",
                "<div class=\"flexi-alert flexi-alert_type_info flexi-alert_has-icon block\">\n<svg class=\"flexi-alert__icon\"><use xlink:href=\"#alert-icon\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>This is a FlexiAlertBlock.</p>\n</div>\n</div>",
                extensions,
                false,
                "{\n    \"flexiAlertBlocks\": {\n        \"defaultBlockOptions\": {\n            \"icon\": \"<svg><use xlink:href=\\\"#alert-icon\\\"/></svg>\",\n            \"attributes\": {\n                \"class\": \"block\"\n            }\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiAlertBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec15(string extensions)
        {
            //     Start line number: 335
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiAlertBlocks": {
            //             "defaultBlockOptions": {
            //                 "blockName": "alert"
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     ! This is a FlexiAlertBlock
            //     
            //     o{
            //         "blockName": "special-alert"
            //     }
            //     ! This is a FlexiAlertBlock with block specific options.
            //     --------------- Expected Markup ---------------
            //     <div class="alert alert_type_info alert_has-icon">
            //     <svg class="alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
            //     <div class="alert__content">
            //     <p>This is a FlexiAlertBlock</p>
            //     </div>
            //     </div>
            //     <div class="special-alert special-alert_type_info special-alert_has-icon">
            //     <svg class="special-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
            //     <div class="special-alert__content">
            //     <p>This is a FlexiAlertBlock with block specific options.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! This is a FlexiAlertBlock\n\no{\n    \"blockName\": \"special-alert\"\n}\n! This is a FlexiAlertBlock with block specific options.",
                "<div class=\"alert alert_type_info alert_has-icon\">\n<svg class=\"alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"/></svg>\n<div class=\"alert__content\">\n<p>This is a FlexiAlertBlock</p>\n</div>\n</div>\n<div class=\"special-alert special-alert_type_info special-alert_has-icon\">\n<svg class=\"special-alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"/></svg>\n<div class=\"special-alert__content\">\n<p>This is a FlexiAlertBlock with block specific options.</p>\n</div>\n</div>",
                extensions,
                false,
                "{\n    \"flexiAlertBlocks\": {\n        \"defaultBlockOptions\": {\n            \"blockName\": \"alert\"\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiAlertBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec16(string extensions)
        {
            //     Start line number: 374
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiAlertBlocks": {
            //             "icons": {
            //                 "closer-look": "<svg><use xlink:href=\"#closer-look-icon\"/></svg>",
            //                 "help": "<svg><use xlink:href=\"#help-icon\"/></svg>"
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     o{ "type": "closer-look" }
            //     ! This is a closer look at some topic.
            //     
            //     o{ "type": "help" }
            //     ! This is a helpful tip.
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-alert flexi-alert_type_closer-look flexi-alert_has-icon">
            //     <svg class="flexi-alert__icon"><use xlink:href="#closer-look-icon"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>This is a closer look at some topic.</p>
            //     </div>
            //     </div>
            //     <div class="flexi-alert flexi-alert_type_help flexi-alert_has-icon">
            //     <svg class="flexi-alert__icon"><use xlink:href="#help-icon"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>This is a helpful tip.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"type\": \"closer-look\" }\n! This is a closer look at some topic.\n\no{ \"type\": \"help\" }\n! This is a helpful tip.",
                "<div class=\"flexi-alert flexi-alert_type_closer-look flexi-alert_has-icon\">\n<svg class=\"flexi-alert__icon\"><use xlink:href=\"#closer-look-icon\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>This is a closer look at some topic.</p>\n</div>\n</div>\n<div class=\"flexi-alert flexi-alert_type_help flexi-alert_has-icon\">\n<svg class=\"flexi-alert__icon\"><use xlink:href=\"#help-icon\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>This is a helpful tip.</p>\n</div>\n</div>",
                extensions,
                false,
                "{\n    \"flexiAlertBlocks\": {\n        \"icons\": {\n            \"closer-look\": \"<svg><use xlink:href=\\\"#closer-look-icon\\\"/></svg>\",\n            \"help\": \"<svg><use xlink:href=\\\"#help-icon\\\"/></svg>\"\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiAlertBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec17(string extensions)
        {
            //     Start line number: 407
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "type": "info" }
            //     ! Info
            //     
            //     o{ "type": "warning" }
            //     ! Warning
            //     
            //     o{ "type": "critical-warning" }
            //     ! Critical warning
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-alert flexi-alert_type_info flexi-alert_has-icon">
            //     <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>Info</p>
            //     </div>
            //     </div>
            //     <div class="flexi-alert flexi-alert_type_warning flexi-alert_has-icon">
            //     <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>Warning</p>
            //     </div>
            //     </div>
            //     <div class="flexi-alert flexi-alert_type_critical-warning flexi-alert_has-icon">
            //     <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     <div class="flexi-alert__content">
            //     <p>Critical warning</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"type\": \"info\" }\n! Info\n\no{ \"type\": \"warning\" }\n! Warning\n\no{ \"type\": \"critical-warning\" }\n! Critical warning",
                "<div class=\"flexi-alert flexi-alert_type_info flexi-alert_has-icon\">\n<svg class=\"flexi-alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>Info</p>\n</div>\n</div>\n<div class=\"flexi-alert flexi-alert_type_warning flexi-alert_has-icon\">\n<svg class=\"flexi-alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>Warning</p>\n</div>\n</div>\n<div class=\"flexi-alert flexi-alert_type_critical-warning flexi-alert_has-icon\">\n<svg class=\"flexi-alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n<div class=\"flexi-alert__content\">\n<p>Critical warning</p>\n</div>\n</div>",
                extensions,
                false);
        }
    }

    public class FlexiBannerBlocksSpecs
    {
        [Theory]
        [InlineData("FlexiBannerBlocks")]
        [InlineData("All")]
        public void FlexiBannerBlocks_Spec1(string extensions)
        {
            //     Start line number: 37
            //     --------------- Markdown ---------------
            //     +++ banner
            //     Title
            //     +++
            //     Blurb
            //     +++
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-banner flexi-banner_no-logo-icon flexi-banner_no-background-icon">
            //     <h1 class="flexi-banner__title">Title</h1>
            //     <span class="flexi-banner__blurb">Blurb</span>
            //     </div>

            SpecTestHelper.AssertCompliance("+++ banner\nTitle\n+++\nBlurb\n+++",
                "<div class=\"flexi-banner flexi-banner_no-logo-icon flexi-banner_no-background-icon\">\n<h1 class=\"flexi-banner__title\">Title</h1>\n<span class=\"flexi-banner__blurb\">Blurb</span>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiBannerBlocks")]
        [InlineData("All")]
        public void FlexiBannerBlocks_Spec2(string extensions)
        {
            //     Start line number: 55
            //     --------------- Markdown ---------------
            //     +++ banner
            //     *Title*
            //     +++
            //     **Blurb**
            //     +++
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-banner flexi-banner_no-logo-icon flexi-banner_no-background-icon">
            //     <h1 class="flexi-banner__title"><em>Title</em></h1>
            //     <span class="flexi-banner__blurb"><strong>Blurb</strong></span>
            //     </div>

            SpecTestHelper.AssertCompliance("+++ banner\n*Title*\n+++\n**Blurb**\n+++",
                "<div class=\"flexi-banner flexi-banner_no-logo-icon flexi-banner_no-background-icon\">\n<h1 class=\"flexi-banner__title\"><em>Title</em></h1>\n<span class=\"flexi-banner__blurb\"><strong>Blurb</strong></span>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiBannerBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiBannerBlocks_Spec3(string extensions)
        {
            //     Start line number: 83
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "blockName": "banner" }
            //     +++ banner
            //     Title
            //     +++
            //     Blurb
            //     +++
            //     --------------- Expected Markup ---------------
            //     <div class="banner banner_no-logo-icon banner_no-background-icon">
            //     <h1 class="banner__title">Title</h1>
            //     <span class="banner__blurb">Blurb</span>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"blockName\": \"banner\" }\n+++ banner\nTitle\n+++\nBlurb\n+++",
                "<div class=\"banner banner_no-logo-icon banner_no-background-icon\">\n<h1 class=\"banner__title\">Title</h1>\n<span class=\"banner__blurb\">Blurb</span>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiBannerBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiBannerBlocks_Spec4(string extensions)
        {
            //     Start line number: 107
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "logoIcon": "<svg><use xlink:href=\"#logo-icon\"/></svg>" }
            //     +++ banner
            //     Title
            //     +++
            //     Blurb
            //     +++
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-banner flexi-banner_has-logo-icon flexi-banner_no-background-icon">
            //     <svg class="flexi-banner__logo-icon"><use xlink:href="#logo-icon"/></svg>
            //     <h1 class="flexi-banner__title">Title</h1>
            //     <span class="flexi-banner__blurb">Blurb</span>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"logoIcon\": \"<svg><use xlink:href=\\\"#logo-icon\\\"/></svg>\" }\n+++ banner\nTitle\n+++\nBlurb\n+++",
                "<div class=\"flexi-banner flexi-banner_has-logo-icon flexi-banner_no-background-icon\">\n<svg class=\"flexi-banner__logo-icon\"><use xlink:href=\"#logo-icon\"/></svg>\n<h1 class=\"flexi-banner__title\">Title</h1>\n<span class=\"flexi-banner__blurb\">Blurb</span>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiBannerBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiBannerBlocks_Spec5(string extensions)
        {
            //     Start line number: 132
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "backgroundIcon": "<svg><use xlink:href=\"#background-icon\"/></svg>" }
            //     +++ banner
            //     Title
            //     +++
            //     Blurb
            //     +++
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-banner flexi-banner_no-logo-icon flexi-banner_has-background-icon">
            //     <svg class="flexi-banner__background-icon"><use xlink:href="#background-icon"/></svg>
            //     <h1 class="flexi-banner__title">Title</h1>
            //     <span class="flexi-banner__blurb">Blurb</span>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"backgroundIcon\": \"<svg><use xlink:href=\\\"#background-icon\\\"/></svg>\" }\n+++ banner\nTitle\n+++\nBlurb\n+++",
                "<div class=\"flexi-banner flexi-banner_no-logo-icon flexi-banner_has-background-icon\">\n<svg class=\"flexi-banner__background-icon\"><use xlink:href=\"#background-icon\"/></svg>\n<h1 class=\"flexi-banner__title\">Title</h1>\n<span class=\"flexi-banner__blurb\">Blurb</span>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiBannerBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiBannerBlocks_Spec6(string extensions)
        {
            //     Start line number: 158
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{
            //         "attributes": {
            //             "id" : "my-custom-id",
            //             "class" : "my-custom-class"
            //         }
            //     }
            //     +++ banner
            //     Title
            //     +++
            //     Blurb
            //     +++
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-banner flexi-banner_no-logo-icon flexi-banner_no-background-icon my-custom-class" id="my-custom-id">
            //     <h1 class="flexi-banner__title">Title</h1>
            //     <span class="flexi-banner__blurb">Blurb</span>
            //     </div>

            SpecTestHelper.AssertCompliance("o{\n    \"attributes\": {\n        \"id\" : \"my-custom-id\",\n        \"class\" : \"my-custom-class\"\n    }\n}\n+++ banner\nTitle\n+++\nBlurb\n+++",
                "<div class=\"flexi-banner flexi-banner_no-logo-icon flexi-banner_no-background-icon my-custom-class\" id=\"my-custom-id\">\n<h1 class=\"flexi-banner__title\">Title</h1>\n<span class=\"flexi-banner__blurb\">Blurb</span>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiBannerBlocks")]
        [InlineData("All")]
        public void FlexiBannerBlocks_Spec7(string extensions)
        {
            //     Start line number: 195
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiBannerBlocks": {
            //             "defaultBlockOptions": {
            //                 "logoIcon": "<svg><use xlink:href=\"#logo-icon\"/></svg>",
            //                 "backgroundIcon": "<svg><use xlink:href=\"#background-icon\"/></svg>",
            //                 "attributes": {
            //                     "class": "block"
            //                 }
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     +++ banner
            //     Title
            //     +++
            //     Blurb
            //     +++
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-banner flexi-banner_has-logo-icon flexi-banner_has-background-icon block">
            //     <svg class="flexi-banner__background-icon"><use xlink:href="#background-icon"/></svg>
            //     <svg class="flexi-banner__logo-icon"><use xlink:href="#logo-icon"/></svg>
            //     <h1 class="flexi-banner__title">Title</h1>
            //     <span class="flexi-banner__blurb">Blurb</span>
            //     </div>

            SpecTestHelper.AssertCompliance("+++ banner\nTitle\n+++\nBlurb\n+++",
                "<div class=\"flexi-banner flexi-banner_has-logo-icon flexi-banner_has-background-icon block\">\n<svg class=\"flexi-banner__background-icon\"><use xlink:href=\"#background-icon\"/></svg>\n<svg class=\"flexi-banner__logo-icon\"><use xlink:href=\"#logo-icon\"/></svg>\n<h1 class=\"flexi-banner__title\">Title</h1>\n<span class=\"flexi-banner__blurb\">Blurb</span>\n</div>",
                extensions,
                false,
                "{\n    \"flexiBannerBlocks\": {\n        \"defaultBlockOptions\": {\n            \"logoIcon\": \"<svg><use xlink:href=\\\"#logo-icon\\\"/></svg>\",\n            \"backgroundIcon\": \"<svg><use xlink:href=\\\"#background-icon\\\"/></svg>\",\n            \"attributes\": {\n                \"class\": \"block\"\n            }\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiBannerBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiBannerBlocks_Spec8(string extensions)
        {
            //     Start line number: 223
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiBannerBlocks": {
            //             "defaultBlockOptions": {
            //                 "blockName": "banner"
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     +++ banner
            //     Title
            //     +++
            //     Blurb
            //     +++
            //     
            //     o{ "blockName": "special-banner" }
            //     +++ banner
            //     Title
            //     +++
            //     Blurb
            //     +++
            //     --------------- Expected Markup ---------------
            //     <div class="banner banner_no-logo-icon banner_no-background-icon">
            //     <h1 class="banner__title">Title</h1>
            //     <span class="banner__blurb">Blurb</span>
            //     </div>
            //     <div class="special-banner special-banner_no-logo-icon special-banner_no-background-icon">
            //     <h1 class="special-banner__title">Title</h1>
            //     <span class="special-banner__blurb">Blurb</span>
            //     </div>

            SpecTestHelper.AssertCompliance("+++ banner\nTitle\n+++\nBlurb\n+++\n\no{ \"blockName\": \"special-banner\" }\n+++ banner\nTitle\n+++\nBlurb\n+++",
                "<div class=\"banner banner_no-logo-icon banner_no-background-icon\">\n<h1 class=\"banner__title\">Title</h1>\n<span class=\"banner__blurb\">Blurb</span>\n</div>\n<div class=\"special-banner special-banner_no-logo-icon special-banner_no-background-icon\">\n<h1 class=\"special-banner__title\">Title</h1>\n<span class=\"special-banner__blurb\">Blurb</span>\n</div>",
                extensions,
                false,
                "{\n    \"flexiBannerBlocks\": {\n        \"defaultBlockOptions\": {\n            \"blockName\": \"banner\"\n        }\n    }\n}");
        }
    }

    public class FlexiCardsBlocksSpecs
    {
        [Theory]
        [InlineData("FlexiCardsBlocks")]
        [InlineData("All")]
        public void FlexiCardsBlocks_Spec1(string extensions)
        {
            //     Start line number: 65
            //     --------------- Markdown ---------------
            //     [[[
            //     +++ card
            //     Title 1
            //     +++
            //     Content 1
            //     +++
            //     Footnote 1
            //     +++
            //     
            //     +++ card
            //     Title 2
            //     +++
            //     Content 2
            //     +++
            //     Footnote 2
            //     +++
            //     [[[
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-cards flexi-cards_size_small">
            //     <div class="flexi-cards__card flexi-cards__card_not-link flexi-cards__card_no-background-icon">
            //     <span class="flexi-cards__card-title">Title 1</span>
            //     <div class="flexi-cards__card-content">
            //     <p>Content 1</p>
            //     </div>
            //     <span class="flexi-cards__card-footnote">Footnote 1</span>
            //     </div>
            //     <div class="flexi-cards__card flexi-cards__card_not-link flexi-cards__card_no-background-icon">
            //     <span class="flexi-cards__card-title">Title 2</span>
            //     <div class="flexi-cards__card-content">
            //     <p>Content 2</p>
            //     </div>
            //     <span class="flexi-cards__card-footnote">Footnote 2</span>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("[[[\n+++ card\nTitle 1\n+++\nContent 1\n+++\nFootnote 1\n+++\n\n+++ card\nTitle 2\n+++\nContent 2\n+++\nFootnote 2\n+++\n[[[",
                "<div class=\"flexi-cards flexi-cards_size_small\">\n<div class=\"flexi-cards__card flexi-cards__card_not-link flexi-cards__card_no-background-icon\">\n<span class=\"flexi-cards__card-title\">Title 1</span>\n<div class=\"flexi-cards__card-content\">\n<p>Content 1</p>\n</div>\n<span class=\"flexi-cards__card-footnote\">Footnote 1</span>\n</div>\n<div class=\"flexi-cards__card flexi-cards__card_not-link flexi-cards__card_no-background-icon\">\n<span class=\"flexi-cards__card-title\">Title 2</span>\n<div class=\"flexi-cards__card-content\">\n<p>Content 2</p>\n</div>\n<span class=\"flexi-cards__card-footnote\">Footnote 2</span>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCardsBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiCardsBlocks_Spec2(string extensions)
        {
            //     Start line number: 108
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     [[[
            //     +++ card
            //     *Title 1*
            //     +++
            //     ```
            //     Content 1
            //     ```
            //     +++
            //     **Footnote 1**
            //     +++
            //     [[[
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-cards flexi-cards_size_small">
            //     <div class="flexi-cards__card flexi-cards__card_not-link flexi-cards__card_no-background-icon">
            //     <span class="flexi-cards__card-title"><em>Title 1</em></span>
            //     <div class="flexi-cards__card-content">
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">Content 1
            //     </code></pre>
            //     </div>
            //     </div>
            //     <span class="flexi-cards__card-footnote"><strong>Footnote 1</strong></span>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("[[[\n+++ card\n*Title 1*\n+++\n```\nContent 1\n```\n+++\n**Footnote 1**\n+++\n[[[",
                "<div class=\"flexi-cards flexi-cards_size_small\">\n<div class=\"flexi-cards__card flexi-cards__card_not-link flexi-cards__card_no-background-icon\">\n<span class=\"flexi-cards__card-title\"><em>Title 1</em></span>\n<div class=\"flexi-cards__card-content\">\n<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">Content 1\n</code></pre>\n</div>\n</div>\n<span class=\"flexi-cards__card-footnote\"><strong>Footnote 1</strong></span>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCardsBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCardsBlocks_Spec3(string extensions)
        {
            //     Start line number: 157
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "blockName": "cards" }
            //     [[[
            //     +++ card
            //     Title 1
            //     +++
            //     Content 1
            //     +++
            //     Footnote 1
            //     +++
            //     [[[
            //     --------------- Expected Markup ---------------
            //     <div class="cards cards_size_small">
            //     <div class="cards__card cards__card_not-link cards__card_no-background-icon">
            //     <span class="cards__card-title">Title 1</span>
            //     <div class="cards__card-content">
            //     <p>Content 1</p>
            //     </div>
            //     <span class="cards__card-footnote">Footnote 1</span>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"blockName\": \"cards\" }\n[[[\n+++ card\nTitle 1\n+++\nContent 1\n+++\nFootnote 1\n+++\n[[[",
                "<div class=\"cards cards_size_small\">\n<div class=\"cards__card cards__card_not-link cards__card_no-background-icon\">\n<span class=\"cards__card-title\">Title 1</span>\n<div class=\"cards__card-content\">\n<p>Content 1</p>\n</div>\n<span class=\"cards__card-footnote\">Footnote 1</span>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCardsBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCardsBlocks_Spec4(string extensions)
        {
            //     Start line number: 189
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "cardSize": "medium" }
            //     [[[
            //     +++ card
            //     Title 1
            //     +++
            //     Content 1
            //     +++
            //     Footnote 1
            //     +++
            //     [[[
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-cards flexi-cards_size_medium">
            //     <div class="flexi-cards__card flexi-cards__card_not-link flexi-cards__card_no-background-icon">
            //     <span class="flexi-cards__card-title">Title 1</span>
            //     <div class="flexi-cards__card-content">
            //     <p>Content 1</p>
            //     </div>
            //     <span class="flexi-cards__card-footnote">Footnote 1</span>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"cardSize\": \"medium\" }\n[[[\n+++ card\nTitle 1\n+++\nContent 1\n+++\nFootnote 1\n+++\n[[[",
                "<div class=\"flexi-cards flexi-cards_size_medium\">\n<div class=\"flexi-cards__card flexi-cards__card_not-link flexi-cards__card_no-background-icon\">\n<span class=\"flexi-cards__card-title\">Title 1</span>\n<div class=\"flexi-cards__card-content\">\n<p>Content 1</p>\n</div>\n<span class=\"flexi-cards__card-footnote\">Footnote 1</span>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCardsBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCardsBlocks_Spec5(string extensions)
        {
            //     Start line number: 221
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ 
            //         "defaultCardOptions": {
            //             "backgroundIcon": "<svg><use xlink:href=\"#background-icon\"/></svg>"
            //         }
            //     }
            //     [[[
            //     +++ card
            //     Title 1
            //     +++
            //     Content 1
            //     +++
            //     Footnote 1
            //     +++
            //     [[[
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-cards flexi-cards_size_small">
            //     <div class="flexi-cards__card flexi-cards__card_not-link flexi-cards__card_has-background-icon">
            //     <svg class="flexi-cards__card-background-icon"><use xlink:href="#background-icon"/></svg>
            //     <span class="flexi-cards__card-title">Title 1</span>
            //     <div class="flexi-cards__card-content">
            //     <p>Content 1</p>
            //     </div>
            //     <span class="flexi-cards__card-footnote">Footnote 1</span>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \n    \"defaultCardOptions\": {\n        \"backgroundIcon\": \"<svg><use xlink:href=\\\"#background-icon\\\"/></svg>\"\n    }\n}\n[[[\n+++ card\nTitle 1\n+++\nContent 1\n+++\nFootnote 1\n+++\n[[[",
                "<div class=\"flexi-cards flexi-cards_size_small\">\n<div class=\"flexi-cards__card flexi-cards__card_not-link flexi-cards__card_has-background-icon\">\n<svg class=\"flexi-cards__card-background-icon\"><use xlink:href=\"#background-icon\"/></svg>\n<span class=\"flexi-cards__card-title\">Title 1</span>\n<div class=\"flexi-cards__card-content\">\n<p>Content 1</p>\n</div>\n<span class=\"flexi-cards__card-footnote\">Footnote 1</span>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCardsBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCardsBlocks_Spec6(string extensions)
        {
            //     Start line number: 252
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ 
            //         "defaultCardOptions": {
            //             "backgroundIcon": "<svg><use xlink:href=\"#background-icon\"/></svg>"
            //         }
            //     }
            //     [[[
            //     +++ card
            //     Title 1
            //     +++
            //     Content 1
            //     +++
            //     Footnote 1
            //     +++
            //     
            //     o{"backgroundIcon": "<svg><use xlink:href=\"#alternative-icon\"/></svg>"}
            //     +++ card
            //     Title 2
            //     +++
            //     Content 2
            //     +++
            //     Footnote 2
            //     +++
            //     [[[
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-cards flexi-cards_size_small">
            //     <div class="flexi-cards__card flexi-cards__card_not-link flexi-cards__card_has-background-icon">
            //     <svg class="flexi-cards__card-background-icon"><use xlink:href="#background-icon"/></svg>
            //     <span class="flexi-cards__card-title">Title 1</span>
            //     <div class="flexi-cards__card-content">
            //     <p>Content 1</p>
            //     </div>
            //     <span class="flexi-cards__card-footnote">Footnote 1</span>
            //     </div>
            //     <div class="flexi-cards__card flexi-cards__card_not-link flexi-cards__card_has-background-icon">
            //     <svg class="flexi-cards__card-background-icon"><use xlink:href="#alternative-icon"/></svg>
            //     <span class="flexi-cards__card-title">Title 2</span>
            //     <div class="flexi-cards__card-content">
            //     <p>Content 2</p>
            //     </div>
            //     <span class="flexi-cards__card-footnote">Footnote 2</span>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \n    \"defaultCardOptions\": {\n        \"backgroundIcon\": \"<svg><use xlink:href=\\\"#background-icon\\\"/></svg>\"\n    }\n}\n[[[\n+++ card\nTitle 1\n+++\nContent 1\n+++\nFootnote 1\n+++\n\no{\"backgroundIcon\": \"<svg><use xlink:href=\\\"#alternative-icon\\\"/></svg>\"}\n+++ card\nTitle 2\n+++\nContent 2\n+++\nFootnote 2\n+++\n[[[",
                "<div class=\"flexi-cards flexi-cards_size_small\">\n<div class=\"flexi-cards__card flexi-cards__card_not-link flexi-cards__card_has-background-icon\">\n<svg class=\"flexi-cards__card-background-icon\"><use xlink:href=\"#background-icon\"/></svg>\n<span class=\"flexi-cards__card-title\">Title 1</span>\n<div class=\"flexi-cards__card-content\">\n<p>Content 1</p>\n</div>\n<span class=\"flexi-cards__card-footnote\">Footnote 1</span>\n</div>\n<div class=\"flexi-cards__card flexi-cards__card_not-link flexi-cards__card_has-background-icon\">\n<svg class=\"flexi-cards__card-background-icon\"><use xlink:href=\"#alternative-icon\"/></svg>\n<span class=\"flexi-cards__card-title\">Title 2</span>\n<div class=\"flexi-cards__card-content\">\n<p>Content 2</p>\n</div>\n<span class=\"flexi-cards__card-footnote\">Footnote 2</span>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCardsBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCardsBlocks_Spec7(string extensions)
        {
            //     Start line number: 308
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{
            //         "attributes": {
            //             "id" : "my-custom-id",
            //             "class" : "my-custom-class"
            //         }
            //     }
            //     [[[
            //     +++ card
            //     Title 1
            //     +++
            //     Content 1
            //     +++
            //     Footnote 1
            //     +++
            //     [[[
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-cards flexi-cards_size_small my-custom-class" id="my-custom-id">
            //     <div class="flexi-cards__card flexi-cards__card_not-link flexi-cards__card_no-background-icon">
            //     <span class="flexi-cards__card-title">Title 1</span>
            //     <div class="flexi-cards__card-content">
            //     <p>Content 1</p>
            //     </div>
            //     <span class="flexi-cards__card-footnote">Footnote 1</span>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("o{\n    \"attributes\": {\n        \"id\" : \"my-custom-id\",\n        \"class\" : \"my-custom-class\"\n    }\n}\n[[[\n+++ card\nTitle 1\n+++\nContent 1\n+++\nFootnote 1\n+++\n[[[",
                "<div class=\"flexi-cards flexi-cards_size_small my-custom-class\" id=\"my-custom-id\">\n<div class=\"flexi-cards__card flexi-cards__card_not-link flexi-cards__card_no-background-icon\">\n<span class=\"flexi-cards__card-title\">Title 1</span>\n<div class=\"flexi-cards__card-content\">\n<p>Content 1</p>\n</div>\n<span class=\"flexi-cards__card-footnote\">Footnote 1</span>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCardsBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCardsBlocks_Spec8(string extensions)
        {
            //     Start line number: 352
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     [[[
            //     o{"url": "/url?a=1&b=2"}
            //     +++ card
            //     Title 1
            //     +++
            //     Content 1
            //     +++
            //     Footnote 1
            //     +++
            //     
            //     +++ card
            //     Title 2
            //     +++
            //     Content 2
            //     +++
            //     Footnote 2
            //     +++
            //     [[[
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-cards flexi-cards_size_small">
            //     <a class="flexi-cards__card flexi-cards__card_is-link flexi-cards__card_no-background-icon" href="/url?a=1&amp;b=2">
            //     <span class="flexi-cards__card-title">Title 1</span>
            //     <div class="flexi-cards__card-content">
            //     <p>Content 1</p>
            //     </div>
            //     <span class="flexi-cards__card-footnote">Footnote 1</span>
            //     </a>
            //     <div class="flexi-cards__card flexi-cards__card_not-link flexi-cards__card_no-background-icon">
            //     <span class="flexi-cards__card-title">Title 2</span>
            //     <div class="flexi-cards__card-content">
            //     <p>Content 2</p>
            //     </div>
            //     <span class="flexi-cards__card-footnote">Footnote 2</span>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("[[[\no{\"url\": \"/url?a=1&b=2\"}\n+++ card\nTitle 1\n+++\nContent 1\n+++\nFootnote 1\n+++\n\n+++ card\nTitle 2\n+++\nContent 2\n+++\nFootnote 2\n+++\n[[[",
                "<div class=\"flexi-cards flexi-cards_size_small\">\n<a class=\"flexi-cards__card flexi-cards__card_is-link flexi-cards__card_no-background-icon\" href=\"/url?a=1&amp;b=2\">\n<span class=\"flexi-cards__card-title\">Title 1</span>\n<div class=\"flexi-cards__card-content\">\n<p>Content 1</p>\n</div>\n<span class=\"flexi-cards__card-footnote\">Footnote 1</span>\n</a>\n<div class=\"flexi-cards__card flexi-cards__card_not-link flexi-cards__card_no-background-icon\">\n<span class=\"flexi-cards__card-title\">Title 2</span>\n<div class=\"flexi-cards__card-content\">\n<p>Content 2</p>\n</div>\n<span class=\"flexi-cards__card-footnote\">Footnote 2</span>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCardsBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCardsBlocks_Spec9(string extensions)
        {
            //     Start line number: 401
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     [[[
            //     o{"backgroundIcon": "<svg><use xlink:href=\"#background-icon\"/></svg>"}
            //     +++ card
            //     Title 1
            //     +++
            //     Content 1
            //     +++
            //     Footnote 1
            //     +++
            //     [[[
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-cards flexi-cards_size_small">
            //     <div class="flexi-cards__card flexi-cards__card_not-link flexi-cards__card_has-background-icon">
            //     <svg class="flexi-cards__card-background-icon"><use xlink:href="#background-icon"/></svg>
            //     <span class="flexi-cards__card-title">Title 1</span>
            //     <div class="flexi-cards__card-content">
            //     <p>Content 1</p>
            //     </div>
            //     <span class="flexi-cards__card-footnote">Footnote 1</span>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("[[[\no{\"backgroundIcon\": \"<svg><use xlink:href=\\\"#background-icon\\\"/></svg>\"}\n+++ card\nTitle 1\n+++\nContent 1\n+++\nFootnote 1\n+++\n[[[",
                "<div class=\"flexi-cards flexi-cards_size_small\">\n<div class=\"flexi-cards__card flexi-cards__card_not-link flexi-cards__card_has-background-icon\">\n<svg class=\"flexi-cards__card-background-icon\"><use xlink:href=\"#background-icon\"/></svg>\n<span class=\"flexi-cards__card-title\">Title 1</span>\n<div class=\"flexi-cards__card-content\">\n<p>Content 1</p>\n</div>\n<span class=\"flexi-cards__card-footnote\">Footnote 1</span>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCardsBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCardsBlocks_Spec10(string extensions)
        {
            //     Start line number: 436
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     [[[
            //     o{
            //         "attributes": {
            //             "id" : "my-custom-id",
            //             "class" : "my-custom-class"
            //         }
            //     }
            //     +++ card
            //     Title 1
            //     +++
            //     Content 1
            //     +++
            //     Footnote 1
            //     +++
            //     [[[
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-cards flexi-cards_size_small">
            //     <div class="flexi-cards__card flexi-cards__card_not-link flexi-cards__card_no-background-icon my-custom-class" id="my-custom-id">
            //     <span class="flexi-cards__card-title">Title 1</span>
            //     <div class="flexi-cards__card-content">
            //     <p>Content 1</p>
            //     </div>
            //     <span class="flexi-cards__card-footnote">Footnote 1</span>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("[[[\no{\n    \"attributes\": {\n        \"id\" : \"my-custom-id\",\n        \"class\" : \"my-custom-class\"\n    }\n}\n+++ card\nTitle 1\n+++\nContent 1\n+++\nFootnote 1\n+++\n[[[",
                "<div class=\"flexi-cards flexi-cards_size_small\">\n<div class=\"flexi-cards__card flexi-cards__card_not-link flexi-cards__card_no-background-icon my-custom-class\" id=\"my-custom-id\">\n<span class=\"flexi-cards__card-title\">Title 1</span>\n<div class=\"flexi-cards__card-content\">\n<p>Content 1</p>\n</div>\n<span class=\"flexi-cards__card-footnote\">Footnote 1</span>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCardsBlocks")]
        [InlineData("All")]
        public void FlexiCardsBlocks_Spec11(string extensions)
        {
            //     Start line number: 482
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiCardsBlocks": {
            //             "defaultBlockOptions": {
            //                 "cardSize": "medium",
            //                 "defaultCardOptions": {
            //                     "backgroundIcon": "<svg><use xlink:href=\"#background-icon\"/></svg>"
            //                 },
            //                 "attributes": {
            //                     "class": "block"
            //                 }
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     [[[
            //     +++ card
            //     Title 1
            //     +++
            //     Content 1
            //     +++
            //     Footnote 1
            //     +++
            //     [[[
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-cards flexi-cards_size_medium block">
            //     <div class="flexi-cards__card flexi-cards__card_not-link flexi-cards__card_has-background-icon">
            //     <svg class="flexi-cards__card-background-icon"><use xlink:href="#background-icon"/></svg>
            //     <span class="flexi-cards__card-title">Title 1</span>
            //     <div class="flexi-cards__card-content">
            //     <p>Content 1</p>
            //     </div>
            //     <span class="flexi-cards__card-footnote">Footnote 1</span>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("[[[\n+++ card\nTitle 1\n+++\nContent 1\n+++\nFootnote 1\n+++\n[[[",
                "<div class=\"flexi-cards flexi-cards_size_medium block\">\n<div class=\"flexi-cards__card flexi-cards__card_not-link flexi-cards__card_has-background-icon\">\n<svg class=\"flexi-cards__card-background-icon\"><use xlink:href=\"#background-icon\"/></svg>\n<span class=\"flexi-cards__card-title\">Title 1</span>\n<div class=\"flexi-cards__card-content\">\n<p>Content 1</p>\n</div>\n<span class=\"flexi-cards__card-footnote\">Footnote 1</span>\n</div>\n</div>",
                extensions,
                false,
                "{\n    \"flexiCardsBlocks\": {\n        \"defaultBlockOptions\": {\n            \"cardSize\": \"medium\",\n            \"defaultCardOptions\": {\n                \"backgroundIcon\": \"<svg><use xlink:href=\\\"#background-icon\\\"/></svg>\"\n            },\n            \"attributes\": {\n                \"class\": \"block\"\n            }\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiCardsBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCardsBlocks_Spec12(string extensions)
        {
            //     Start line number: 520
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiCardsBlocks": {
            //             "defaultBlockOptions": {
            //                 "blockName": "cards",
            //                 "defaultCardOptions": {
            //                     "backgroundIcon": "<svg><use xlink:href=\"#background-icon\"/></svg>"
            //                 }
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     [[[
            //     o{ "backgroundIcon": "<svg><use xlink:href=\"#alternative-icon\"/></svg>" }
            //     +++ card
            //     Title 1
            //     +++
            //     Content 1
            //     +++
            //     Footnote 1
            //     +++
            //     [[[
            //     
            //     o{ "blockName": "special-cards" }
            //     [[[
            //     +++ card
            //     Title 2
            //     +++
            //     Content 2
            //     +++
            //     Footnote 2
            //     +++
            //     [[[
            //     --------------- Expected Markup ---------------
            //     <div class="cards cards_size_small">
            //     <div class="cards__card cards__card_not-link cards__card_has-background-icon">
            //     <svg class="cards__card-background-icon"><use xlink:href="#alternative-icon"/></svg>
            //     <span class="cards__card-title">Title 1</span>
            //     <div class="cards__card-content">
            //     <p>Content 1</p>
            //     </div>
            //     <span class="cards__card-footnote">Footnote 1</span>
            //     </div>
            //     </div>
            //     <div class="special-cards special-cards_size_small">
            //     <div class="special-cards__card special-cards__card_not-link special-cards__card_has-background-icon">
            //     <svg class="special-cards__card-background-icon"><use xlink:href="#background-icon"/></svg>
            //     <span class="special-cards__card-title">Title 2</span>
            //     <div class="special-cards__card-content">
            //     <p>Content 2</p>
            //     </div>
            //     <span class="special-cards__card-footnote">Footnote 2</span>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("[[[\no{ \"backgroundIcon\": \"<svg><use xlink:href=\\\"#alternative-icon\\\"/></svg>\" }\n+++ card\nTitle 1\n+++\nContent 1\n+++\nFootnote 1\n+++\n[[[\n\no{ \"blockName\": \"special-cards\" }\n[[[\n+++ card\nTitle 2\n+++\nContent 2\n+++\nFootnote 2\n+++\n[[[",
                "<div class=\"cards cards_size_small\">\n<div class=\"cards__card cards__card_not-link cards__card_has-background-icon\">\n<svg class=\"cards__card-background-icon\"><use xlink:href=\"#alternative-icon\"/></svg>\n<span class=\"cards__card-title\">Title 1</span>\n<div class=\"cards__card-content\">\n<p>Content 1</p>\n</div>\n<span class=\"cards__card-footnote\">Footnote 1</span>\n</div>\n</div>\n<div class=\"special-cards special-cards_size_small\">\n<div class=\"special-cards__card special-cards__card_not-link special-cards__card_has-background-icon\">\n<svg class=\"special-cards__card-background-icon\"><use xlink:href=\"#background-icon\"/></svg>\n<span class=\"special-cards__card-title\">Title 2</span>\n<div class=\"special-cards__card-content\">\n<p>Content 2</p>\n</div>\n<span class=\"special-cards__card-footnote\">Footnote 2</span>\n</div>\n</div>",
                extensions,
                false,
                "{\n    \"flexiCardsBlocks\": {\n        \"defaultBlockOptions\": {\n            \"blockName\": \"cards\",\n            \"defaultCardOptions\": {\n                \"backgroundIcon\": \"<svg><use xlink:href=\\\"#background-icon\\\"/></svg>\"\n            }\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiCardsBlocks")]
        [InlineData("All")]
        public void FlexiCardsBlocks_Spec13(string extensions)
        {
            //     Start line number: 584
            //     --------------- Markdown ---------------
            //     [[[
            //     +++ card
            //     Title 1
            //     +++
            //     [[[[
            //     +++ card
            //     Nested card
            //     +++
            //     Nested card content
            //     +++
            //     Nested card footnote
            //     +++
            //     [[[[
            //     +++
            //     Footnote 1
            //     +++
            //     [[[
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-cards flexi-cards_size_small">
            //     <div class="flexi-cards__card flexi-cards__card_not-link flexi-cards__card_no-background-icon">
            //     <span class="flexi-cards__card-title">Title 1</span>
            //     <div class="flexi-cards__card-content">
            //     <div class="flexi-cards flexi-cards_size_small">
            //     <div class="flexi-cards__card flexi-cards__card_not-link flexi-cards__card_no-background-icon">
            //     <span class="flexi-cards__card-title">Nested card</span>
            //     <div class="flexi-cards__card-content">
            //     <p>Nested card content</p>
            //     </div>
            //     <span class="flexi-cards__card-footnote">Nested card footnote</span>
            //     </div>
            //     </div>
            //     </div>
            //     <span class="flexi-cards__card-footnote">Footnote 1</span>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("[[[\n+++ card\nTitle 1\n+++\n[[[[\n+++ card\nNested card\n+++\nNested card content\n+++\nNested card footnote\n+++\n[[[[\n+++\nFootnote 1\n+++\n[[[",
                "<div class=\"flexi-cards flexi-cards_size_small\">\n<div class=\"flexi-cards__card flexi-cards__card_not-link flexi-cards__card_no-background-icon\">\n<span class=\"flexi-cards__card-title\">Title 1</span>\n<div class=\"flexi-cards__card-content\">\n<div class=\"flexi-cards flexi-cards_size_small\">\n<div class=\"flexi-cards__card flexi-cards__card_not-link flexi-cards__card_no-background-icon\">\n<span class=\"flexi-cards__card-title\">Nested card</span>\n<div class=\"flexi-cards__card-content\">\n<p>Nested card content</p>\n</div>\n<span class=\"flexi-cards__card-footnote\">Nested card footnote</span>\n</div>\n</div>\n</div>\n<span class=\"flexi-cards__card-footnote\">Footnote 1</span>\n</div>\n</div>",
                extensions,
                false);
        }
    }

    public class FlexiCodeBlocksSpecs
    {
        [Theory]
        [InlineData("FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec1(string extensions)
        {
            //     Start line number: 56
            //     --------------- Markdown ---------------
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + &quot;dummyString&quot;;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">public string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + &quot;dummyString&quot;;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec2(string extensions)
        {
            //     Start line number: 88
            //     --------------- Markdown ---------------
            //     ~~~
            //     <html>
            //         <head>
            //             <title>Example Page</title>
            //         </head>
            //         <body>
            //             <p>Example content.</p>
            //         </body>
            //     </html>
            //     ~~~
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">&lt;html&gt;
            //         &lt;head&gt;
            //             &lt;title&gt;Example Page&lt;/title&gt;
            //         &lt;/head&gt;
            //         &lt;body&gt;
            //             &lt;p&gt;Example content.&lt;/p&gt;
            //         &lt;/body&gt;
            //     &lt;/html&gt;
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("~~~\n<html>\n    <head>\n        <title>Example Page</title>\n    </head>\n    <body>\n        <p>Example content.</p>\n    </body>\n</html>\n~~~",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">&lt;html&gt;\n    &lt;head&gt;\n        &lt;title&gt;Example Page&lt;/title&gt;\n    &lt;/head&gt;\n    &lt;body&gt;\n        &lt;p&gt;Example content.&lt;/p&gt;\n    &lt;/body&gt;\n&lt;/html&gt;\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec3(string extensions)
        {
            //     Start line number: 120
            //     --------------- Markdown ---------------
            //         public exampleFunction(arg: string): string {
            //             // Example comment
            //             return arg + "dummyString";
            //         }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">public exampleFunction(arg: string): string {
            //         // Example comment
            //         return arg + &quot;dummyString&quot;;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("    public exampleFunction(arg: string): string {\n        // Example comment\n        return arg + \"dummyString\";\n    }",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">public exampleFunction(arg: string): string {\n    // Example comment\n    return arg + &quot;dummyString&quot;;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec4(string extensions)
        {
            //     Start line number: 155
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{
            //         "blockName": "code"
            //     }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="code code_no-title code_has-copy-icon code_has-header code_no-syntax-highlights code_no-line-numbers code_has-omitted-lines-icon code_no-highlighted-lines code_no-highlighted-phrases">
            //     <header class="code__header">
            //     <button class="code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="code__pre"><code class="code__code">public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + &quot;dummyString&quot;;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("o{\n    \"blockName\": \"code\"\n}\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div class=\"code code_no-title code_has-copy-icon code_has-header code_no-syntax-highlights code_no-line-numbers code_has-omitted-lines-icon code_no-highlighted-lines code_no-highlighted-phrases\">\n<header class=\"code__header\">\n<button class=\"code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"code__pre\"><code class=\"code__code\">public string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + &quot;dummyString&quot;;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec5(string extensions)
        {
            //     Start line number: 191
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "title" : "ExampleDocument.cs" }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_has-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <span class="flexi-code__title">ExampleDocument.cs</span>
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + &quot;dummyString&quot;;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"title\" : \"ExampleDocument.cs\" }\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div class=\"flexi-code flexi-code_has-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<span class=\"flexi-code__title\">ExampleDocument.cs</span>\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">public string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + &quot;dummyString&quot;;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec6(string extensions)
        {
            //     Start line number: 227
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "copyIcon": "<svg><use xlink:href=\"#material-design-copy\"/></svg>" }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon"><use xlink:href="#material-design-copy"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + &quot;dummyString&quot;;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"copyIcon\": \"<svg><use xlink:href=\\\"#material-design-copy\\\"/></svg>\" }\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\"><use xlink:href=\"#material-design-copy\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">public string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + &quot;dummyString&quot;;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec7(string extensions)
        {
            //     Start line number: 255
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "copyIcon": null }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_no-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + &quot;dummyString&quot;;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"copyIcon\": null }\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_no-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">public string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + &quot;dummyString&quot;;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec8(string extensions)
        {
            //     Start line number: 288
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "renderHeader": "false" }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + &quot;dummyString&quot;;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"renderHeader\": \"false\" }\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_no-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">public string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + &quot;dummyString&quot;;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec9(string extensions)
        {
            //     Start line number: 322
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "language": "csharp" }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_language_csharp flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="token keyword">public</span> <span class="token keyword">string</span> <span class="token function">ExampleFunction</span><span class="token punctuation">(</span><span class="token keyword">string</span> arg<span class="token punctuation">)</span>
            //     <span class="token punctuation">{</span>
            //         <span class="token comment">// Example comment</span>
            //         <span class="token keyword">return</span> arg <span class="token operator">+</span> <span class="token string">"dummyString"</span><span class="token punctuation">;</span>
            //     <span class="token punctuation">}</span>
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"language\": \"csharp\" }\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_language_csharp flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\"><span class=\"token keyword\">public</span> <span class=\"token keyword\">string</span> <span class=\"token function\">ExampleFunction</span><span class=\"token punctuation\">(</span><span class=\"token keyword\">string</span> arg<span class=\"token punctuation\">)</span>\n<span class=\"token punctuation\">{</span>\n    <span class=\"token comment\">// Example comment</span>\n    <span class=\"token keyword\">return</span> arg <span class=\"token operator\">+</span> <span class=\"token string\">\"dummyString\"</span><span class=\"token punctuation\">;</span>\n<span class=\"token punctuation\">}</span>\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec10(string extensions)
        {
            //     Start line number: 358
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{
            //         "syntaxHighlighter": "highlightJS",
            //         "language": "typescript"
            //     }
            //     ```
            //     public exampleFunction(arg: string): string {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_language_typescript flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="hljs-keyword">public</span> exampleFunction(arg: <span class="hljs-built_in">string</span>): <span class="hljs-built_in">string</span> {
            //         <span class="hljs-comment">// Example comment</span>
            //         <span class="hljs-keyword">return</span> arg + <span class="hljs-string">"dummyString"</span>;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("o{\n    \"syntaxHighlighter\": \"highlightJS\",\n    \"language\": \"typescript\"\n}\n```\npublic exampleFunction(arg: string): string {\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_language_typescript flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\"><span class=\"hljs-keyword\">public</span> exampleFunction(arg: <span class=\"hljs-built_in\">string</span>): <span class=\"hljs-built_in\">string</span> {\n    <span class=\"hljs-comment\">// Example comment</span>\n    <span class=\"hljs-keyword\">return</span> arg + <span class=\"hljs-string\">\"dummyString\"</span>;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec11(string extensions)
        {
            //     Start line number: 399
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{
            //         "lineNumbers": [
            //             { "startLine": 2, "endLine": 8, "startNumber": 4 },
            //             { "startLine": 10, "endLine": -2, "startNumber": 32 }
            //         ]
            //     }
            //     ```
            //     
            //     public class ExampleClass
            //     {
            //         public string ExampleFunction1(string arg)
            //         {
            //             // Example comment
            //             return arg + "dummyString";
            //         }
            //     
            //         public string ExampleFunction3(string arg)
            //         {
            //             // Example comment
            //             return arg + "dummyString";
            //         }
            //     }
            //     
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_has-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="flexi-code__line-prefix"><svg class="flexi-code__omitted-lines-icon" xmlns="http://www.w3.org/2000/svg" width="2px" height="10px" viewBox="0 0 2 10"><rect shape-rendering="crispEdges" width="2" height="2"/><rect shape-rendering="crispEdges" y="4" width="2" height="2"/><rect shape-rendering="crispEdges" y="8" width="2" height="2"/></svg></span><span class="flexi-code__line flexi-code__line_omitted-lines">Lines 1 to 3 omitted for brevity</span>
            //     <span class="flexi-code__line-prefix">4</span><span class="flexi-code__line">public class ExampleClass</span>
            //     <span class="flexi-code__line-prefix">5</span><span class="flexi-code__line">{</span>
            //     <span class="flexi-code__line-prefix">6</span><span class="flexi-code__line">    public string ExampleFunction1(string arg)</span>
            //     <span class="flexi-code__line-prefix">7</span><span class="flexi-code__line">    {</span>
            //     <span class="flexi-code__line-prefix">8</span><span class="flexi-code__line">        // Example comment</span>
            //     <span class="flexi-code__line-prefix">9</span><span class="flexi-code__line">        return arg + &quot;dummyString&quot;;</span>
            //     <span class="flexi-code__line-prefix">10</span><span class="flexi-code__line">    }</span>
            //     <span class="flexi-code__line-prefix"><svg class="flexi-code__omitted-lines-icon" xmlns="http://www.w3.org/2000/svg" width="2px" height="10px" viewBox="0 0 2 10"><rect shape-rendering="crispEdges" width="2" height="2"/><rect shape-rendering="crispEdges" y="4" width="2" height="2"/><rect shape-rendering="crispEdges" y="8" width="2" height="2"/></svg></span><span class="flexi-code__line flexi-code__line_omitted-lines">Lines 11 to 31 omitted for brevity</span>
            //     <span class="flexi-code__line-prefix">32</span><span class="flexi-code__line">    public string ExampleFunction3(string arg)</span>
            //     <span class="flexi-code__line-prefix">33</span><span class="flexi-code__line">    {</span>
            //     <span class="flexi-code__line-prefix">34</span><span class="flexi-code__line">        // Example comment</span>
            //     <span class="flexi-code__line-prefix">35</span><span class="flexi-code__line">        return arg + &quot;dummyString&quot;;</span>
            //     <span class="flexi-code__line-prefix">36</span><span class="flexi-code__line">    }</span>
            //     <span class="flexi-code__line-prefix">37</span><span class="flexi-code__line">}</span>
            //     <span class="flexi-code__line-prefix"><svg class="flexi-code__omitted-lines-icon" xmlns="http://www.w3.org/2000/svg" width="2px" height="10px" viewBox="0 0 2 10"><rect shape-rendering="crispEdges" width="2" height="2"/><rect shape-rendering="crispEdges" y="4" width="2" height="2"/><rect shape-rendering="crispEdges" y="8" width="2" height="2"/></svg></span><span class="flexi-code__line flexi-code__line_omitted-lines">Lines 38 to the end omitted for brevity</span>
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("o{\n    \"lineNumbers\": [\n        { \"startLine\": 2, \"endLine\": 8, \"startNumber\": 4 },\n        { \"startLine\": 10, \"endLine\": -2, \"startNumber\": 32 }\n    ]\n}\n```\n\npublic class ExampleClass\n{\n    public string ExampleFunction1(string arg)\n    {\n        // Example comment\n        return arg + \"dummyString\";\n    }\n\n    public string ExampleFunction3(string arg)\n    {\n        // Example comment\n        return arg + \"dummyString\";\n    }\n}\n\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_has-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\"><span class=\"flexi-code__line-prefix\"><svg class=\"flexi-code__omitted-lines-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"2px\" height=\"10px\" viewBox=\"0 0 2 10\"><rect shape-rendering=\"crispEdges\" width=\"2\" height=\"2\"/><rect shape-rendering=\"crispEdges\" y=\"4\" width=\"2\" height=\"2\"/><rect shape-rendering=\"crispEdges\" y=\"8\" width=\"2\" height=\"2\"/></svg></span><span class=\"flexi-code__line flexi-code__line_omitted-lines\">Lines 1 to 3 omitted for brevity</span>\n<span class=\"flexi-code__line-prefix\">4</span><span class=\"flexi-code__line\">public class ExampleClass</span>\n<span class=\"flexi-code__line-prefix\">5</span><span class=\"flexi-code__line\">{</span>\n<span class=\"flexi-code__line-prefix\">6</span><span class=\"flexi-code__line\">    public string ExampleFunction1(string arg)</span>\n<span class=\"flexi-code__line-prefix\">7</span><span class=\"flexi-code__line\">    {</span>\n<span class=\"flexi-code__line-prefix\">8</span><span class=\"flexi-code__line\">        // Example comment</span>\n<span class=\"flexi-code__line-prefix\">9</span><span class=\"flexi-code__line\">        return arg + &quot;dummyString&quot;;</span>\n<span class=\"flexi-code__line-prefix\">10</span><span class=\"flexi-code__line\">    }</span>\n<span class=\"flexi-code__line-prefix\"><svg class=\"flexi-code__omitted-lines-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"2px\" height=\"10px\" viewBox=\"0 0 2 10\"><rect shape-rendering=\"crispEdges\" width=\"2\" height=\"2\"/><rect shape-rendering=\"crispEdges\" y=\"4\" width=\"2\" height=\"2\"/><rect shape-rendering=\"crispEdges\" y=\"8\" width=\"2\" height=\"2\"/></svg></span><span class=\"flexi-code__line flexi-code__line_omitted-lines\">Lines 11 to 31 omitted for brevity</span>\n<span class=\"flexi-code__line-prefix\">32</span><span class=\"flexi-code__line\">    public string ExampleFunction3(string arg)</span>\n<span class=\"flexi-code__line-prefix\">33</span><span class=\"flexi-code__line\">    {</span>\n<span class=\"flexi-code__line-prefix\">34</span><span class=\"flexi-code__line\">        // Example comment</span>\n<span class=\"flexi-code__line-prefix\">35</span><span class=\"flexi-code__line\">        return arg + &quot;dummyString&quot;;</span>\n<span class=\"flexi-code__line-prefix\">36</span><span class=\"flexi-code__line\">    }</span>\n<span class=\"flexi-code__line-prefix\">37</span><span class=\"flexi-code__line\">}</span>\n<span class=\"flexi-code__line-prefix\"><svg class=\"flexi-code__omitted-lines-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"2px\" height=\"10px\" viewBox=\"0 0 2 10\"><rect shape-rendering=\"crispEdges\" width=\"2\" height=\"2\"/><rect shape-rendering=\"crispEdges\" y=\"4\" width=\"2\" height=\"2\"/><rect shape-rendering=\"crispEdges\" y=\"8\" width=\"2\" height=\"2\"/></svg></span><span class=\"flexi-code__line flexi-code__line_omitted-lines\">Lines 38 to the end omitted for brevity</span>\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec12(string extensions)
        {
            //     Start line number: 461
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{
            //         "omittedLinesIcon": "<svg><use xlink:href=\"#material-design-more-vert\"/></svg>",
            //         "lineNumbers": [{"endLine": 2}, {"startLine": 4, "startNumber":10}]
            //     }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //     
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_has-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="flexi-code__line-prefix">1</span><span class="flexi-code__line">public string ExampleFunction(string arg)</span>
            //     <span class="flexi-code__line-prefix">2</span><span class="flexi-code__line">{</span>
            //     <span class="flexi-code__line-prefix"><svg class="flexi-code__omitted-lines-icon"><use xlink:href="#material-design-more-vert"/></svg></span><span class="flexi-code__line flexi-code__line_omitted-lines">Lines 3 to 9 omitted for brevity</span>
            //     <span class="flexi-code__line-prefix">10</span><span class="flexi-code__line">}</span>
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("o{\n    \"omittedLinesIcon\": \"<svg><use xlink:href=\\\"#material-design-more-vert\\\"/></svg>\",\n    \"lineNumbers\": [{\"endLine\": 2}, {\"startLine\": 4, \"startNumber\":10}]\n}\n```\npublic string ExampleFunction(string arg)\n{\n\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_has-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\"><span class=\"flexi-code__line-prefix\">1</span><span class=\"flexi-code__line\">public string ExampleFunction(string arg)</span>\n<span class=\"flexi-code__line-prefix\">2</span><span class=\"flexi-code__line\">{</span>\n<span class=\"flexi-code__line-prefix\"><svg class=\"flexi-code__omitted-lines-icon\"><use xlink:href=\"#material-design-more-vert\"/></svg></span><span class=\"flexi-code__line flexi-code__line_omitted-lines\">Lines 3 to 9 omitted for brevity</span>\n<span class=\"flexi-code__line-prefix\">10</span><span class=\"flexi-code__line\">}</span>\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec13(string extensions)
        {
            //     Start line number: 490
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{
            //         "omittedLinesIcon": null,
            //         "lineNumbers": [{"endLine": 2}, {"startLine": 4, "startNumber":10}]
            //     }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //     
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_has-line-numbers flexi-code_no-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="flexi-code__line-prefix">1</span><span class="flexi-code__line">public string ExampleFunction(string arg)</span>
            //     <span class="flexi-code__line-prefix">2</span><span class="flexi-code__line">{</span>
            //     <span class="flexi-code__line-prefix"></span><span class="flexi-code__line flexi-code__line_omitted-lines">Lines 3 to 9 omitted for brevity</span>
            //     <span class="flexi-code__line-prefix">10</span><span class="flexi-code__line">}</span>
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("o{\n    \"omittedLinesIcon\": null,\n    \"lineNumbers\": [{\"endLine\": 2}, {\"startLine\": 4, \"startNumber\":10}]\n}\n```\npublic string ExampleFunction(string arg)\n{\n\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_has-line-numbers flexi-code_no-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\"><span class=\"flexi-code__line-prefix\">1</span><span class=\"flexi-code__line\">public string ExampleFunction(string arg)</span>\n<span class=\"flexi-code__line-prefix\">2</span><span class=\"flexi-code__line\">{</span>\n<span class=\"flexi-code__line-prefix\"></span><span class=\"flexi-code__line flexi-code__line_omitted-lines\">Lines 3 to 9 omitted for brevity</span>\n<span class=\"flexi-code__line-prefix\">10</span><span class=\"flexi-code__line\">}</span>\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec14(string extensions)
        {
            //     Start line number: 525
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{
            //         "highlightedLines": [
            //             { "endLine": 1 },
            //             { "startLine": 3, "endLine": 4 }
            //         ]
            //     }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_has-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="flexi-code__line flexi-code__line_highlighted">public string ExampleFunction(string arg)</span>
            //     {
            //     <span class="flexi-code__line flexi-code__line_highlighted">    // Example comment</span>
            //     <span class="flexi-code__line flexi-code__line_highlighted">    return arg + &quot;dummyString&quot;;</span>
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("o{\n    \"highlightedLines\": [\n        { \"endLine\": 1 },\n        { \"startLine\": 3, \"endLine\": 4 }\n    ]\n}\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_has-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\"><span class=\"flexi-code__line flexi-code__line_highlighted\">public string ExampleFunction(string arg)</span>\n{\n<span class=\"flexi-code__line flexi-code__line_highlighted\">    // Example comment</span>\n<span class=\"flexi-code__line flexi-code__line_highlighted\">    return arg + &quot;dummyString&quot;;</span>\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec15(string extensions)
        {
            //     Start line number: 566
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{
            //         "highlightedPhrases": [
            //             { "regex": "return (.*?);", "includedMatches": [1] },
            //             { "regex": "string arg" }
            //         ]
            //     }
            //     ```
            //     public class ExampleClass
            //     {
            //         public string ExampleFunction1(string arg)
            //         {
            //             // Example comment
            //             return arg + "dummyString";
            //         }
            //     
            //         public string ExampleFunction2(string arg)
            //         {
            //             // Example comment
            //             return arg + "dummyString";
            //         }
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_has-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">public class ExampleClass
            //     {
            //         public string ExampleFunction1(<span class="flexi-code__highlighted-phrase">string arg</span>)
            //         {
            //             // Example comment
            //             return arg + &quot;dummyString&quot;;
            //         }
            //     
            //         public string ExampleFunction2(<span class="flexi-code__highlighted-phrase">string arg</span>)
            //         {
            //             // Example comment
            //             return <span class="flexi-code__highlighted-phrase">arg + &quot;dummyString&quot;</span>;
            //         }
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("o{\n    \"highlightedPhrases\": [\n        { \"regex\": \"return (.*?);\", \"includedMatches\": [1] },\n        { \"regex\": \"string arg\" }\n    ]\n}\n```\npublic class ExampleClass\n{\n    public string ExampleFunction1(string arg)\n    {\n        // Example comment\n        return arg + \"dummyString\";\n    }\n\n    public string ExampleFunction2(string arg)\n    {\n        // Example comment\n        return arg + \"dummyString\";\n    }\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_has-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">public class ExampleClass\n{\n    public string ExampleFunction1(<span class=\"flexi-code__highlighted-phrase\">string arg</span>)\n    {\n        // Example comment\n        return arg + &quot;dummyString&quot;;\n    }\n\n    public string ExampleFunction2(<span class=\"flexi-code__highlighted-phrase\">string arg</span>)\n    {\n        // Example comment\n        return <span class=\"flexi-code__highlighted-phrase\">arg + &quot;dummyString&quot;</span>;\n    }\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec16(string extensions)
        {
            //     Start line number: 623
            //     --------------- Markdown ---------------
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + &quot;dummyString&quot;;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">public string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + &quot;dummyString&quot;;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec17(string extensions)
        {
            //     Start line number: 649
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "renderingMode": "classic" }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <pre><code>public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + &quot;dummyString&quot;;
            //     }
            //     </code></pre>

            SpecTestHelper.AssertCompliance("o{ \"renderingMode\": \"classic\" }\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<pre><code>public string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + &quot;dummyString&quot;;\n}\n</code></pre>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec18(string extensions)
        {
            //     Start line number: 678
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{
            //         "attributes": {
            //             "id" : "code-1",
            //             "class" : "block"
            //         }
            //     }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases block" id="code-1">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + &quot;dummyString&quot;;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("o{\n    \"attributes\": {\n        \"id\" : \"code-1\",\n        \"class\" : \"block\"\n    }\n}\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases block\" id=\"code-1\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">public string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + &quot;dummyString&quot;;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec19(string extensions)
        {
            //     Start line number: 789
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiCodeBlocks": {
            //             "defaultBlockOptions": {
            //                 "blockName": "code",
            //                 "title": "ExampleDocument.cs",
            //                 "copyIcon": "<svg><use xlink:href=\"#material-design-copy\"/></svg>",
            //                 "language": "html",
            //                 "syntaxHighlighter": "highlightjs",
            //                 "lineNumbers": [{}],
            //                 "omittedLinesIcon": "<svg><use xlink:href=\"#material-design-more-vert\"/></svg>",
            //                 "highlightedLines": [{"startLine": 3, "endLine": 3}],
            //                 "highlightedPhrases": [{"regex":"</.*?>"}],
            //                 "attributes": {"class": "block"}
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     ```
            //     <html>
            //         <head>
            //             <title>Example Page</title>
            //         </head>
            //         <body>
            //             <p>Example content.</p>
            //         </body>
            //     </html>
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="code code_has-title code_has-copy-icon code_has-header code_language_html code_has-syntax-highlights code_has-line-numbers code_has-omitted-lines-icon code_has-highlighted-lines code_has-highlighted-phrases block">
            //     <header class="code__header">
            //     <span class="code__title">ExampleDocument.cs</span>
            //     <button class="code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="code__copy-icon"><use xlink:href="#material-design-copy"/></svg>
            //     </button>
            //     </header>
            //     <pre class="code__pre"><code class="code__code"><span class="code__line-prefix">1</span><span class="code__line"><span class="hljs-tag">&lt;<span class="hljs-name">html</span>&gt;</span></span>
            //     <span class="code__line-prefix">2</span><span class="code__line">    <span class="hljs-tag">&lt;<span class="hljs-name">head</span>&gt;</span></span>
            //     <span class="code__line-prefix">3</span><span class="code__line code__line_highlighted">        <span class="hljs-tag">&lt;<span class="hljs-name">title</span>&gt;</span>Example Page<span class="code__highlighted-phrase"><span class="hljs-tag">&lt;/<span class="hljs-name">title</span>&gt;</span></span></span>
            //     <span class="code__line-prefix">4</span><span class="code__line">    <span class="code__highlighted-phrase"><span class="hljs-tag">&lt;/<span class="hljs-name">head</span>&gt;</span></span></span>
            //     <span class="code__line-prefix">5</span><span class="code__line">    <span class="hljs-tag">&lt;<span class="hljs-name">body</span>&gt;</span></span>
            //     <span class="code__line-prefix">6</span><span class="code__line">        <span class="hljs-tag">&lt;<span class="hljs-name">p</span>&gt;</span>Example content.<span class="code__highlighted-phrase"><span class="hljs-tag">&lt;/<span class="hljs-name">p</span>&gt;</span></span></span>
            //     <span class="code__line-prefix">7</span><span class="code__line">    <span class="code__highlighted-phrase"><span class="hljs-tag">&lt;/<span class="hljs-name">body</span>&gt;</span></span></span>
            //     <span class="code__line-prefix">8</span><span class="code__line"><span class="code__highlighted-phrase"><span class="hljs-tag">&lt;/<span class="hljs-name">html</span>&gt;</span></span></span>
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("```\n<html>\n    <head>\n        <title>Example Page</title>\n    </head>\n    <body>\n        <p>Example content.</p>\n    </body>\n</html>\n```",
                "<div class=\"code code_has-title code_has-copy-icon code_has-header code_language_html code_has-syntax-highlights code_has-line-numbers code_has-omitted-lines-icon code_has-highlighted-lines code_has-highlighted-phrases block\">\n<header class=\"code__header\">\n<span class=\"code__title\">ExampleDocument.cs</span>\n<button class=\"code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"code__copy-icon\"><use xlink:href=\"#material-design-copy\"/></svg>\n</button>\n</header>\n<pre class=\"code__pre\"><code class=\"code__code\"><span class=\"code__line-prefix\">1</span><span class=\"code__line\"><span class=\"hljs-tag\">&lt;<span class=\"hljs-name\">html</span>&gt;</span></span>\n<span class=\"code__line-prefix\">2</span><span class=\"code__line\">    <span class=\"hljs-tag\">&lt;<span class=\"hljs-name\">head</span>&gt;</span></span>\n<span class=\"code__line-prefix\">3</span><span class=\"code__line code__line_highlighted\">        <span class=\"hljs-tag\">&lt;<span class=\"hljs-name\">title</span>&gt;</span>Example Page<span class=\"code__highlighted-phrase\"><span class=\"hljs-tag\">&lt;/<span class=\"hljs-name\">title</span>&gt;</span></span></span>\n<span class=\"code__line-prefix\">4</span><span class=\"code__line\">    <span class=\"code__highlighted-phrase\"><span class=\"hljs-tag\">&lt;/<span class=\"hljs-name\">head</span>&gt;</span></span></span>\n<span class=\"code__line-prefix\">5</span><span class=\"code__line\">    <span class=\"hljs-tag\">&lt;<span class=\"hljs-name\">body</span>&gt;</span></span>\n<span class=\"code__line-prefix\">6</span><span class=\"code__line\">        <span class=\"hljs-tag\">&lt;<span class=\"hljs-name\">p</span>&gt;</span>Example content.<span class=\"code__highlighted-phrase\"><span class=\"hljs-tag\">&lt;/<span class=\"hljs-name\">p</span>&gt;</span></span></span>\n<span class=\"code__line-prefix\">7</span><span class=\"code__line\">    <span class=\"code__highlighted-phrase\"><span class=\"hljs-tag\">&lt;/<span class=\"hljs-name\">body</span>&gt;</span></span></span>\n<span class=\"code__line-prefix\">8</span><span class=\"code__line\"><span class=\"code__highlighted-phrase\"><span class=\"hljs-tag\">&lt;/<span class=\"hljs-name\">html</span>&gt;</span></span></span>\n</code></pre>\n</div>",
                extensions,
                false,
                "{\n    \"flexiCodeBlocks\": {\n        \"defaultBlockOptions\": {\n            \"blockName\": \"code\",\n            \"title\": \"ExampleDocument.cs\",\n            \"copyIcon\": \"<svg><use xlink:href=\\\"#material-design-copy\\\"/></svg>\",\n            \"language\": \"html\",\n            \"syntaxHighlighter\": \"highlightjs\",\n            \"lineNumbers\": [{}],\n            \"omittedLinesIcon\": \"<svg><use xlink:href=\\\"#material-design-more-vert\\\"/></svg>\",\n            \"highlightedLines\": [{\"startLine\": 3, \"endLine\": 3}],\n            \"highlightedPhrases\": [{\"regex\":\"</.*?>\"}],\n            \"attributes\": {\"class\": \"block\"}\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec20(string extensions)
        {
            //     Start line number: 839
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiCodeBlocks": {
            //             "defaultBlockOptions": {
            //                 "lineNumbers": [{}]
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     
            //     o{
            //         "lineNumbers": [
            //             {
            //                 "startLine": 2, "startNumber": 25
            //             }
            //         ]
            //     }
            //     ```
            //     
            //     body {
            //         display: flex;
            //         align-items: center;
            //         font-size: 13px;
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_has-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="flexi-code__line-prefix">1</span><span class="flexi-code__line">public string ExampleFunction(string arg)</span>
            //     <span class="flexi-code__line-prefix">2</span><span class="flexi-code__line">{</span>
            //     <span class="flexi-code__line-prefix">3</span><span class="flexi-code__line">    // Example comment</span>
            //     <span class="flexi-code__line-prefix">4</span><span class="flexi-code__line">    return arg + &quot;dummyString&quot;;</span>
            //     <span class="flexi-code__line-prefix">5</span><span class="flexi-code__line">}</span>
            //     </code></pre>
            //     </div>
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_has-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="flexi-code__line-prefix"><svg class="flexi-code__omitted-lines-icon" xmlns="http://www.w3.org/2000/svg" width="2px" height="10px" viewBox="0 0 2 10"><rect shape-rendering="crispEdges" width="2" height="2"/><rect shape-rendering="crispEdges" y="4" width="2" height="2"/><rect shape-rendering="crispEdges" y="8" width="2" height="2"/></svg></span><span class="flexi-code__line flexi-code__line_omitted-lines">Lines 1 to 24 omitted for brevity</span>
            //     <span class="flexi-code__line-prefix">25</span><span class="flexi-code__line">body {</span>
            //     <span class="flexi-code__line-prefix">26</span><span class="flexi-code__line">    display: flex;</span>
            //     <span class="flexi-code__line-prefix">27</span><span class="flexi-code__line">    align-items: center;</span>
            //     <span class="flexi-code__line-prefix">28</span><span class="flexi-code__line">    font-size: 13px;</span>
            //     <span class="flexi-code__line-prefix">29</span><span class="flexi-code__line">}</span>
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```\n\no{\n    \"lineNumbers\": [\n        {\n            \"startLine\": 2, \"startNumber\": 25\n        }\n    ]\n}\n```\n\nbody {\n    display: flex;\n    align-items: center;\n    font-size: 13px;\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_has-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\"><span class=\"flexi-code__line-prefix\">1</span><span class=\"flexi-code__line\">public string ExampleFunction(string arg)</span>\n<span class=\"flexi-code__line-prefix\">2</span><span class=\"flexi-code__line\">{</span>\n<span class=\"flexi-code__line-prefix\">3</span><span class=\"flexi-code__line\">    // Example comment</span>\n<span class=\"flexi-code__line-prefix\">4</span><span class=\"flexi-code__line\">    return arg + &quot;dummyString&quot;;</span>\n<span class=\"flexi-code__line-prefix\">5</span><span class=\"flexi-code__line\">}</span>\n</code></pre>\n</div>\n<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_has-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\"><span class=\"flexi-code__line-prefix\"><svg class=\"flexi-code__omitted-lines-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"2px\" height=\"10px\" viewBox=\"0 0 2 10\"><rect shape-rendering=\"crispEdges\" width=\"2\" height=\"2\"/><rect shape-rendering=\"crispEdges\" y=\"4\" width=\"2\" height=\"2\"/><rect shape-rendering=\"crispEdges\" y=\"8\" width=\"2\" height=\"2\"/></svg></span><span class=\"flexi-code__line flexi-code__line_omitted-lines\">Lines 1 to 24 omitted for brevity</span>\n<span class=\"flexi-code__line-prefix\">25</span><span class=\"flexi-code__line\">body {</span>\n<span class=\"flexi-code__line-prefix\">26</span><span class=\"flexi-code__line\">    display: flex;</span>\n<span class=\"flexi-code__line-prefix\">27</span><span class=\"flexi-code__line\">    align-items: center;</span>\n<span class=\"flexi-code__line-prefix\">28</span><span class=\"flexi-code__line\">    font-size: 13px;</span>\n<span class=\"flexi-code__line-prefix\">29</span><span class=\"flexi-code__line\">}</span>\n</code></pre>\n</div>",
                extensions,
                false,
                "{\n    \"flexiCodeBlocks\": {\n        \"defaultBlockOptions\": {\n            \"lineNumbers\": [{}]\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec21(string extensions)
        {
            //     Start line number: 908
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{
            //         "language": "csharp",
            //         "highlightedLines": [
            //             { "startLine": 3, "endLine": 3 },
            //             { "startLine": 8, "endLine": 8 }
            //         ],
            //         "highlightedPhrases": [
            //             { "regex": "Multiline.*?1" },
            //             { "regex": "/.*?/", "includedMatches": [1] }
            //         ]
            //     }
            //     ```
            //     /* 
            //         Multiline
            //         comment
            //         1
            //     */
            //     /* 
            //         Multiline
            //         comment
            //         2
            //     */
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_language_csharp flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_has-highlighted-lines flexi-code_has-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="token comment">/* 
            //         <span class="flexi-code__highlighted-phrase">Multiline</span></span>
            //     <span class="flexi-code__line flexi-code__line_highlighted"><span class="token comment"><span class="flexi-code__highlighted-phrase">    comment</span></span></span>
            //     <span class="token comment"><span class="flexi-code__highlighted-phrase">    1</span>
            //     */</span>
            //     <span class="flexi-code__highlighted-phrase"><span class="token comment">/* 
            //         Multiline</span></span>
            //     <span class="flexi-code__line flexi-code__line_highlighted"><span class="flexi-code__highlighted-phrase"><span class="token comment">    comment</span></span></span>
            //     <span class="flexi-code__highlighted-phrase"><span class="token comment">    2
            //     */</span></span>
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("o{\n    \"language\": \"csharp\",\n    \"highlightedLines\": [\n        { \"startLine\": 3, \"endLine\": 3 },\n        { \"startLine\": 8, \"endLine\": 8 }\n    ],\n    \"highlightedPhrases\": [\n        { \"regex\": \"Multiline.*?1\" },\n        { \"regex\": \"/.*?/\", \"includedMatches\": [1] }\n    ]\n}\n```\n/* \n    Multiline\n    comment\n    1\n*/\n/* \n    Multiline\n    comment\n    2\n*/\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_language_csharp flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_has-highlighted-lines flexi-code_has-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\"><span class=\"token comment\">/* \n    <span class=\"flexi-code__highlighted-phrase\">Multiline</span></span>\n<span class=\"flexi-code__line flexi-code__line_highlighted\"><span class=\"token comment\"><span class=\"flexi-code__highlighted-phrase\">    comment</span></span></span>\n<span class=\"token comment\"><span class=\"flexi-code__highlighted-phrase\">    1</span>\n*/</span>\n<span class=\"flexi-code__highlighted-phrase\"><span class=\"token comment\">/* \n    Multiline</span></span>\n<span class=\"flexi-code__line flexi-code__line_highlighted\"><span class=\"flexi-code__highlighted-phrase\"><span class=\"token comment\">    comment</span></span></span>\n<span class=\"flexi-code__highlighted-phrase\"><span class=\"token comment\">    2\n*/</span></span>\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec22(string extensions)
        {
            //     Start line number: 958
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{
            //         "language": "csharp",
            //         "highlightedPhrases": [
            //             { "regex": "comment\\s+re" },
            //             { "regex": "\\+ \"d" }
            //         ]
            //     }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_language_csharp flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_has-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="token keyword">public</span> <span class="token keyword">string</span> <span class="token function">ExampleFunction</span><span class="token punctuation">(</span><span class="token keyword">string</span> arg<span class="token punctuation">)</span>
            //     <span class="token punctuation">{</span>
            //         <span class="token comment">// Example <span class="flexi-code__highlighted-phrase">comment</span></span><span class="flexi-code__highlighted-phrase">
            //         <span class="token keyword">re</span></span><span class="token keyword">turn</span> arg <span class="flexi-code__highlighted-phrase"><span class="token operator">+</span> <span class="token string">"d</span></span><span class="token string">ummyString"</span><span class="token punctuation">;</span>
            //     <span class="token punctuation">}</span>
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("o{\n    \"language\": \"csharp\",\n    \"highlightedPhrases\": [\n        { \"regex\": \"comment\\\\s+re\" },\n        { \"regex\": \"\\\\+ \\\"d\" }\n    ]\n}\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_language_csharp flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_has-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\"><span class=\"token keyword\">public</span> <span class=\"token keyword\">string</span> <span class=\"token function\">ExampleFunction</span><span class=\"token punctuation\">(</span><span class=\"token keyword\">string</span> arg<span class=\"token punctuation\">)</span>\n<span class=\"token punctuation\">{</span>\n    <span class=\"token comment\">// Example <span class=\"flexi-code__highlighted-phrase\">comment</span></span><span class=\"flexi-code__highlighted-phrase\">\n    <span class=\"token keyword\">re</span></span><span class=\"token keyword\">turn</span> arg <span class=\"flexi-code__highlighted-phrase\"><span class=\"token operator\">+</span> <span class=\"token string\">\"d</span></span><span class=\"token string\">ummyString\"</span><span class=\"token punctuation\">;</span>\n<span class=\"token punctuation\">}</span>\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec23(string extensions)
        {
            //     Start line number: 994
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{
            //         "highlightedPhrases": [
            //             { "regex": "comment\\s+re" },
            //             { "regex": "(return )(arg)" },
            //             { "regex": "return" },
            //             { "regex": "rg \\+" }
            //         ]
            //     }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_has-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">public string ExampleFunction(string arg)
            //     {
            //         // Example <span class="flexi-code__highlighted-phrase">comment
            //         return arg +</span> &quot;dummyString&quot;;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("o{\n    \"highlightedPhrases\": [\n        { \"regex\": \"comment\\\\s+re\" },\n        { \"regex\": \"(return )(arg)\" },\n        { \"regex\": \"return\" },\n        { \"regex\": \"rg \\\\+\" }\n    ]\n}\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_has-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">public string ExampleFunction(string arg)\n{\n    // Example <span class=\"flexi-code__highlighted-phrase\">comment\n    return arg +</span> &quot;dummyString&quot;;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec24(string extensions)
        {
            //     Start line number: 1030
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{
            //         "language": "csharp",
            //         "highlightedPhrases": [
            //             { "regex": "string ExampleFunction" },
            //             { "regex": "return" },
            //             { "regex": "(\"dum)myStr(ing\")" }
            //         ]
            //     }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_language_csharp flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_has-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="token keyword">public</span> <span class="flexi-code__highlighted-phrase"><span class="token keyword">string</span> <span class="token function">ExampleFunction</span></span><span class="token punctuation">(</span><span class="token keyword">string</span> arg<span class="token punctuation">)</span>
            //     <span class="token punctuation">{</span>
            //         <span class="token comment">// Example comment</span>
            //         <span class="flexi-code__highlighted-phrase"><span class="token keyword">return</span></span> arg <span class="token operator">+</span> <span class="flexi-code__highlighted-phrase"><span class="token string">"dum</span></span><span class="token string">myStr<span class="flexi-code__highlighted-phrase">ing"</span></span><span class="token punctuation">;</span>
            //     <span class="token punctuation">}</span>
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("o{\n    \"language\": \"csharp\",\n    \"highlightedPhrases\": [\n        { \"regex\": \"string ExampleFunction\" },\n        { \"regex\": \"return\" },\n        { \"regex\": \"(\\\"dum)myStr(ing\\\")\" }\n    ]\n}\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_language_csharp flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_has-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\"><span class=\"token keyword\">public</span> <span class=\"flexi-code__highlighted-phrase\"><span class=\"token keyword\">string</span> <span class=\"token function\">ExampleFunction</span></span><span class=\"token punctuation\">(</span><span class=\"token keyword\">string</span> arg<span class=\"token punctuation\">)</span>\n<span class=\"token punctuation\">{</span>\n    <span class=\"token comment\">// Example comment</span>\n    <span class=\"flexi-code__highlighted-phrase\"><span class=\"token keyword\">return</span></span> arg <span class=\"token operator\">+</span> <span class=\"flexi-code__highlighted-phrase\"><span class=\"token string\">\"dum</span></span><span class=\"token string\">myStr<span class=\"flexi-code__highlighted-phrase\">ing\"</span></span><span class=\"token punctuation\">;</span>\n<span class=\"token punctuation\">}</span>\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec25(string extensions)
        {
            //     Start line number: 1069
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{
            //         "language": "html"
            //     }
            //     ```
            //     <div class="my-class">&</div>
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_language_html flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="token tag"><span class="token tag"><span class="token punctuation">&lt;</span>div</span> <span class="token attr-name">class</span><span class="token attr-value"><span class="token punctuation">=</span><span class="token punctuation">"</span>my-class<span class="token punctuation">"</span></span><span class="token punctuation">></span></span>&amp;<span class="token tag"><span class="token tag"><span class="token punctuation">&lt;/</span>div</span><span class="token punctuation">></span></span>
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("o{\n    \"language\": \"html\"\n}\n```\n<div class=\"my-class\">&</div>\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_language_html flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\"><span class=\"token tag\"><span class=\"token tag\"><span class=\"token punctuation\">&lt;</span>div</span> <span class=\"token attr-name\">class</span><span class=\"token attr-value\"><span class=\"token punctuation\">=</span><span class=\"token punctuation\">\"</span>my-class<span class=\"token punctuation\">\"</span></span><span class=\"token punctuation\">></span></span>&amp;<span class=\"token tag\"><span class=\"token tag\"><span class=\"token punctuation\">&lt;/</span>div</span><span class=\"token punctuation\">></span></span>\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec26(string extensions)
        {
            //     Start line number: 1092
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{
            //         "language": "html",
            //         "syntaxHighlighter": "highlightjs"
            //     }
            //     ```
            //     <div class="my-class">&</div>
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_language_html flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="hljs-tag">&lt;<span class="hljs-name">div</span> <span class="hljs-attr">class</span>=<span class="hljs-string">"my-class"</span>&gt;</span>&amp;<span class="hljs-tag">&lt;/<span class="hljs-name">div</span>&gt;</span>
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("o{\n    \"language\": \"html\",\n    \"syntaxHighlighter\": \"highlightjs\"\n}\n```\n<div class=\"my-class\">&</div>\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_language_html flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\"><span class=\"hljs-tag\">&lt;<span class=\"hljs-name\">div</span> <span class=\"hljs-attr\">class</span>=<span class=\"hljs-string\">\"my-class\"</span>&gt;</span>&amp;<span class=\"hljs-tag\">&lt;/<span class=\"hljs-name\">div</span>&gt;</span>\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec27(string extensions)
        {
            //     Start line number: 1116
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     ```
            //     <div class="my-class">&</div>
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">&lt;div class=&quot;my-class&quot;&gt;&amp;&lt;/div&gt;
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("```\n<div class=\"my-class\">&</div>\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">&lt;div class=&quot;my-class&quot;&gt;&amp;&lt;/div&gt;\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec28(string extensions)
        {
            //     Start line number: 1136
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{
            //         "highlightedPhrases": [{ "regex": "div" }]
            //     }
            //     ```
            //     <div class="my-class">&</div>
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_has-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">&lt;<span class="flexi-code__highlighted-phrase">div</span> class=&quot;my-class&quot;&gt;&amp;&lt;/<span class="flexi-code__highlighted-phrase">div</span>&gt;
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("o{\n    \"highlightedPhrases\": [{ \"regex\": \"div\" }]\n}\n```\n<div class=\"my-class\">&</div>\n```",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_has-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">&lt;<span class=\"flexi-code__highlighted-phrase\">div</span> class=&quot;my-class&quot;&gt;&amp;&lt;/<span class=\"flexi-code__highlighted-phrase\">div</span>&gt;\n</code></pre>\n</div>",
                extensions,
                false);
        }
    }

    public class FlexiFigureBlocksSpecs
    {
        [Theory]
        [InlineData("FlexiFigureBlocks")]
        [InlineData("All")]
        public void FlexiFigureBlocks_Spec1(string extensions)
        {
            //     Start line number: 40
            //     --------------- Markdown ---------------
            //     +++ figure
            //     This is a figure!
            //     +++
            //     Caption
            //     +++
            //     --------------- Expected Markup ---------------
            //     <figure class="flexi-figure flexi-figure_has-name" id="figure-1">
            //     <div class="flexi-figure__content">
            //     <p>This is a figure!</p>
            //     </div>
            //     <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
            //     </figure>

            SpecTestHelper.AssertCompliance("+++ figure\nThis is a figure!\n+++\nCaption\n+++",
                "<figure class=\"flexi-figure flexi-figure_has-name\" id=\"figure-1\">\n<div class=\"flexi-figure__content\">\n<p>This is a figure!</p>\n</div>\n<figcaption class=\"flexi-figure__caption\"><span class=\"flexi-figure__name\">Figure 1. </span>Caption</figcaption>\n</figure>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiFigureBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiFigureBlocks_Spec2(string extensions)
        {
            //     Start line number: 60
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     +++ figure
            //     ```
            //     This is a figure!
            //     ```
            //     +++
            //     **Caption**
            //     +++
            //     --------------- Expected Markup ---------------
            //     <figure class="flexi-figure flexi-figure_has-name" id="figure-1">
            //     <div class="flexi-figure__content">
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">This is a figure!
            //     </code></pre>
            //     </div>
            //     </div>
            //     <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span><strong>Caption</strong></figcaption>
            //     </figure>

            SpecTestHelper.AssertCompliance("+++ figure\n```\nThis is a figure!\n```\n+++\n**Caption**\n+++",
                "<figure class=\"flexi-figure flexi-figure_has-name\" id=\"figure-1\">\n<div class=\"flexi-figure__content\">\n<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">This is a figure!\n</code></pre>\n</div>\n</div>\n<figcaption class=\"flexi-figure__caption\"><span class=\"flexi-figure__name\">Figure 1. </span><strong>Caption</strong></figcaption>\n</figure>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiFigureBlocks")]
        [InlineData("All")]
        public void FlexiFigureBlocks_Spec3(string extensions)
        {
            //     Start line number: 91
            //     --------------- Markdown ---------------
            //     +++ figure
            //     This is the first figure!
            //     +++
            //     Caption
            //     +++
            //     
            //     +++ figure
            //     This is the second figure!
            //     +++
            //     Caption
            //     +++
            //     
            //     +++ figure
            //     This is the third figure!
            //     +++
            //     Caption
            //     +++
            //     --------------- Expected Markup ---------------
            //     <figure class="flexi-figure flexi-figure_has-name" id="figure-1">
            //     <div class="flexi-figure__content">
            //     <p>This is the first figure!</p>
            //     </div>
            //     <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
            //     </figure>
            //     <figure class="flexi-figure flexi-figure_has-name" id="figure-2">
            //     <div class="flexi-figure__content">
            //     <p>This is the second figure!</p>
            //     </div>
            //     <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 2. </span>Caption</figcaption>
            //     </figure>
            //     <figure class="flexi-figure flexi-figure_has-name" id="figure-3">
            //     <div class="flexi-figure__content">
            //     <p>This is the third figure!</p>
            //     </div>
            //     <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 3. </span>Caption</figcaption>
            //     </figure>

            SpecTestHelper.AssertCompliance("+++ figure\nThis is the first figure!\n+++\nCaption\n+++\n\n+++ figure\nThis is the second figure!\n+++\nCaption\n+++\n\n+++ figure\nThis is the third figure!\n+++\nCaption\n+++",
                "<figure class=\"flexi-figure flexi-figure_has-name\" id=\"figure-1\">\n<div class=\"flexi-figure__content\">\n<p>This is the first figure!</p>\n</div>\n<figcaption class=\"flexi-figure__caption\"><span class=\"flexi-figure__name\">Figure 1. </span>Caption</figcaption>\n</figure>\n<figure class=\"flexi-figure flexi-figure_has-name\" id=\"figure-2\">\n<div class=\"flexi-figure__content\">\n<p>This is the second figure!</p>\n</div>\n<figcaption class=\"flexi-figure__caption\"><span class=\"flexi-figure__name\">Figure 2. </span>Caption</figcaption>\n</figure>\n<figure class=\"flexi-figure flexi-figure_has-name\" id=\"figure-3\">\n<div class=\"flexi-figure__content\">\n<p>This is the third figure!</p>\n</div>\n<figcaption class=\"flexi-figure__caption\"><span class=\"flexi-figure__name\">Figure 3. </span>Caption</figcaption>\n</figure>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiFigureBlocks")]
        [InlineData("All")]
        public void FlexiFigureBlocks_Spec4(string extensions)
        {
            //     Start line number: 137
            //     --------------- Markdown ---------------
            //     [figure 1]
            //     [figure 2]
            //     
            //     +++ figure
            //     This is the first figure!
            //     +++
            //     Caption
            //     +++
            //     
            //     +++ figure
            //     This is the second figure!
            //     +++
            //     Caption
            //     +++
            //     
            //     [Figure 1]
            //     [Figure 2]
            //     --------------- Expected Markup ---------------
            //     <p><a href="#figure-1">figure 1</a>
            //     <a href="#figure-2">figure 2</a></p>
            //     <figure class="flexi-figure flexi-figure_has-name" id="figure-1">
            //     <div class="flexi-figure__content">
            //     <p>This is the first figure!</p>
            //     </div>
            //     <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
            //     </figure>
            //     <figure class="flexi-figure flexi-figure_has-name" id="figure-2">
            //     <div class="flexi-figure__content">
            //     <p>This is the second figure!</p>
            //     </div>
            //     <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 2. </span>Caption</figcaption>
            //     </figure>
            //     <p><a href="#figure-1">Figure 1</a>
            //     <a href="#figure-2">Figure 2</a></p>

            SpecTestHelper.AssertCompliance("[figure 1]\n[figure 2]\n\n+++ figure\nThis is the first figure!\n+++\nCaption\n+++\n\n+++ figure\nThis is the second figure!\n+++\nCaption\n+++\n\n[Figure 1]\n[Figure 2]",
                "<p><a href=\"#figure-1\">figure 1</a>\n<a href=\"#figure-2\">figure 2</a></p>\n<figure class=\"flexi-figure flexi-figure_has-name\" id=\"figure-1\">\n<div class=\"flexi-figure__content\">\n<p>This is the first figure!</p>\n</div>\n<figcaption class=\"flexi-figure__caption\"><span class=\"flexi-figure__name\">Figure 1. </span>Caption</figcaption>\n</figure>\n<figure class=\"flexi-figure flexi-figure_has-name\" id=\"figure-2\">\n<div class=\"flexi-figure__content\">\n<p>This is the second figure!</p>\n</div>\n<figcaption class=\"flexi-figure__caption\"><span class=\"flexi-figure__name\">Figure 2. </span>Caption</figcaption>\n</figure>\n<p><a href=\"#figure-1\">Figure 1</a>\n<a href=\"#figure-2\">Figure 2</a></p>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiFigureBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiFigureBlocks_Spec5(string extensions)
        {
            //     Start line number: 189
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "blockName": "figure" }
            //     +++ figure
            //     This is a figure!
            //     +++
            //     Caption
            //     +++
            //     --------------- Expected Markup ---------------
            //     <figure class="figure figure_has-name" id="figure-1">
            //     <div class="figure__content">
            //     <p>This is a figure!</p>
            //     </div>
            //     <figcaption class="figure__caption"><span class="figure__name">Figure 1. </span>Caption</figcaption>
            //     </figure>

            SpecTestHelper.AssertCompliance("o{ \"blockName\": \"figure\" }\n+++ figure\nThis is a figure!\n+++\nCaption\n+++",
                "<figure class=\"figure figure_has-name\" id=\"figure-1\">\n<div class=\"figure__content\">\n<p>This is a figure!</p>\n</div>\n<figcaption class=\"figure__caption\"><span class=\"figure__name\">Figure 1. </span>Caption</figcaption>\n</figure>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiFigureBlocks")]
        [InlineData("All")]
        public void FlexiFigureBlocks_Spec6(string extensions)
        {
            //     Start line number: 218
            //     --------------- Markdown ---------------
            //     [figure 1]
            //     [Figure 1]
            //     
            //     +++ figure
            //     This is a figure!
            //     +++
            //     Caption
            //     +++
            //     
            //     [figure 1]
            //     [Figure 1]
            //     --------------- Expected Markup ---------------
            //     <p><a href="#figure-1">figure 1</a>
            //     <a href="#figure-1">Figure 1</a></p>
            //     <figure class="flexi-figure flexi-figure_has-name" id="figure-1">
            //     <div class="flexi-figure__content">
            //     <p>This is a figure!</p>
            //     </div>
            //     <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
            //     </figure>
            //     <p><a href="#figure-1">figure 1</a>
            //     <a href="#figure-1">Figure 1</a></p>

            SpecTestHelper.AssertCompliance("[figure 1]\n[Figure 1]\n\n+++ figure\nThis is a figure!\n+++\nCaption\n+++\n\n[figure 1]\n[Figure 1]",
                "<p><a href=\"#figure-1\">figure 1</a>\n<a href=\"#figure-1\">Figure 1</a></p>\n<figure class=\"flexi-figure flexi-figure_has-name\" id=\"figure-1\">\n<div class=\"flexi-figure__content\">\n<p>This is a figure!</p>\n</div>\n<figcaption class=\"flexi-figure__caption\"><span class=\"flexi-figure__name\">Figure 1. </span>Caption</figcaption>\n</figure>\n<p><a href=\"#figure-1\">figure 1</a>\n<a href=\"#figure-1\">Figure 1</a></p>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiFigureBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiFigureBlocks_Spec7(string extensions)
        {
            //     Start line number: 244
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "referenceLinkable": false }
            //     +++ figure
            //     This is a figure!
            //     +++
            //     Caption
            //     +++
            //     
            //     [figure 1]
            //     --------------- Expected Markup ---------------
            //     <figure class="flexi-figure flexi-figure_has-name" id="figure-1">
            //     <div class="flexi-figure__content">
            //     <p>This is a figure!</p>
            //     </div>
            //     <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
            //     </figure>
            //     <p>[figure 1]</p>

            SpecTestHelper.AssertCompliance("o{ \"referenceLinkable\": false }\n+++ figure\nThis is a figure!\n+++\nCaption\n+++\n\n[figure 1]",
                "<figure class=\"flexi-figure flexi-figure_has-name\" id=\"figure-1\">\n<div class=\"flexi-figure__content\">\n<p>This is a figure!</p>\n</div>\n<figcaption class=\"flexi-figure__caption\"><span class=\"flexi-figure__name\">Figure 1. </span>Caption</figcaption>\n</figure>\n<p>[figure 1]</p>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiFigureBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiFigureBlocks_Spec8(string extensions)
        {
            //     Start line number: 266
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "generateID": false }
            //     +++ figure
            //     This is a figure!
            //     +++
            //     Caption
            //     +++
            //     
            //     [figure 1]
            //     --------------- Expected Markup ---------------
            //     <figure class="flexi-figure flexi-figure_has-name">
            //     <div class="flexi-figure__content">
            //     <p>This is a figure!</p>
            //     </div>
            //     <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
            //     </figure>
            //     <p>[figure 1]</p>

            SpecTestHelper.AssertCompliance("o{ \"generateID\": false }\n+++ figure\nThis is a figure!\n+++\nCaption\n+++\n\n[figure 1]",
                "<figure class=\"flexi-figure flexi-figure_has-name\">\n<div class=\"flexi-figure__content\">\n<p>This is a figure!</p>\n</div>\n<figcaption class=\"flexi-figure__caption\"><span class=\"flexi-figure__name\">Figure 1. </span>Caption</figcaption>\n</figure>\n<p>[figure 1]</p>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiFigureBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiFigureBlocks_Spec9(string extensions)
        {
            //     Start line number: 288
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ 
            //         "generateID": false,
            //         "attributes": { "id": "custom-id" }
            //     }
            //     +++ figure
            //     This is a figure!
            //     +++
            //     Caption
            //     +++
            //     
            //     [figure 1]
            //     --------------- Expected Markup ---------------
            //     <figure class="flexi-figure flexi-figure_has-name" id="custom-id">
            //     <div class="flexi-figure__content">
            //     <p>This is a figure!</p>
            //     </div>
            //     <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
            //     </figure>
            //     <p><a href="#custom-id">figure 1</a></p>

            SpecTestHelper.AssertCompliance("o{ \n    \"generateID\": false,\n    \"attributes\": { \"id\": \"custom-id\" }\n}\n+++ figure\nThis is a figure!\n+++\nCaption\n+++\n\n[figure 1]",
                "<figure class=\"flexi-figure flexi-figure_has-name\" id=\"custom-id\">\n<div class=\"flexi-figure__content\">\n<p>This is a figure!</p>\n</div>\n<figcaption class=\"flexi-figure__caption\"><span class=\"flexi-figure__name\">Figure 1. </span>Caption</figcaption>\n</figure>\n<p><a href=\"#custom-id\">figure 1</a></p>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiFigureBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiFigureBlocks_Spec10(string extensions)
        {
            //     Start line number: 322
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{"linkLabelContent": "my figure"}
            //     +++ figure
            //     This is the first figure!
            //     +++
            //     Caption
            //     +++
            //     
            //     +++ figure
            //     This is the second figure!
            //     +++
            //     Caption
            //     +++
            //     
            //     [my figure]
            //     [figure 2]
            //     --------------- Expected Markup ---------------
            //     <figure class="flexi-figure flexi-figure_has-name" id="figure-1">
            //     <div class="flexi-figure__content">
            //     <p>This is the first figure!</p>
            //     </div>
            //     <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
            //     </figure>
            //     <figure class="flexi-figure flexi-figure_has-name" id="figure-2">
            //     <div class="flexi-figure__content">
            //     <p>This is the second figure!</p>
            //     </div>
            //     <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 2. </span>Caption</figcaption>
            //     </figure>
            //     <p><a href="#figure-1">Figure 1</a>
            //     <a href="#figure-2">figure 2</a></p>

            SpecTestHelper.AssertCompliance("o{\"linkLabelContent\": \"my figure\"}\n+++ figure\nThis is the first figure!\n+++\nCaption\n+++\n\n+++ figure\nThis is the second figure!\n+++\nCaption\n+++\n\n[my figure]\n[figure 2]",
                "<figure class=\"flexi-figure flexi-figure_has-name\" id=\"figure-1\">\n<div class=\"flexi-figure__content\">\n<p>This is the first figure!</p>\n</div>\n<figcaption class=\"flexi-figure__caption\"><span class=\"flexi-figure__name\">Figure 1. </span>Caption</figcaption>\n</figure>\n<figure class=\"flexi-figure flexi-figure_has-name\" id=\"figure-2\">\n<div class=\"flexi-figure__content\">\n<p>This is the second figure!</p>\n</div>\n<figcaption class=\"flexi-figure__caption\"><span class=\"flexi-figure__name\">Figure 2. </span>Caption</figcaption>\n</figure>\n<p><a href=\"#figure-1\">Figure 1</a>\n<a href=\"#figure-2\">figure 2</a></p>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiFigureBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiFigureBlocks_Spec11(string extensions)
        {
            //     Start line number: 359
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     +++ figure
            //     This is the new first figure!
            //     +++
            //     Caption
            //     +++
            //     
            //     o{"linkLabelContent": "my figure"}
            //     +++ figure
            //     This is the first figure!
            //     +++
            //     Caption
            //     +++
            //     
            //     +++ figure
            //     This is the second figure!
            //     +++
            //     Caption
            //     +++
            //     
            //     [figure 1]
            //     [my figure]
            //     [figure 3]
            //     --------------- Expected Markup ---------------
            //     <figure class="flexi-figure flexi-figure_has-name" id="figure-1">
            //     <div class="flexi-figure__content">
            //     <p>This is the new first figure!</p>
            //     </div>
            //     <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
            //     </figure>
            //     <figure class="flexi-figure flexi-figure_has-name" id="figure-2">
            //     <div class="flexi-figure__content">
            //     <p>This is the first figure!</p>
            //     </div>
            //     <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 2. </span>Caption</figcaption>
            //     </figure>
            //     <figure class="flexi-figure flexi-figure_has-name" id="figure-3">
            //     <div class="flexi-figure__content">
            //     <p>This is the second figure!</p>
            //     </div>
            //     <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 3. </span>Caption</figcaption>
            //     </figure>
            //     <p><a href="#figure-1">figure 1</a>
            //     <a href="#figure-2">Figure 2</a>
            //     <a href="#figure-3">figure 3</a></p>

            SpecTestHelper.AssertCompliance("+++ figure\nThis is the new first figure!\n+++\nCaption\n+++\n\no{\"linkLabelContent\": \"my figure\"}\n+++ figure\nThis is the first figure!\n+++\nCaption\n+++\n\n+++ figure\nThis is the second figure!\n+++\nCaption\n+++\n\n[figure 1]\n[my figure]\n[figure 3]",
                "<figure class=\"flexi-figure flexi-figure_has-name\" id=\"figure-1\">\n<div class=\"flexi-figure__content\">\n<p>This is the new first figure!</p>\n</div>\n<figcaption class=\"flexi-figure__caption\"><span class=\"flexi-figure__name\">Figure 1. </span>Caption</figcaption>\n</figure>\n<figure class=\"flexi-figure flexi-figure_has-name\" id=\"figure-2\">\n<div class=\"flexi-figure__content\">\n<p>This is the first figure!</p>\n</div>\n<figcaption class=\"flexi-figure__caption\"><span class=\"flexi-figure__name\">Figure 2. </span>Caption</figcaption>\n</figure>\n<figure class=\"flexi-figure flexi-figure_has-name\" id=\"figure-3\">\n<div class=\"flexi-figure__content\">\n<p>This is the second figure!</p>\n</div>\n<figcaption class=\"flexi-figure__caption\"><span class=\"flexi-figure__name\">Figure 3. </span>Caption</figcaption>\n</figure>\n<p><a href=\"#figure-1\">figure 1</a>\n<a href=\"#figure-2\">Figure 2</a>\n<a href=\"#figure-3\">figure 3</a></p>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiFigureBlocks")]
        [InlineData("All")]
        public void FlexiFigureBlocks_Spec12(string extensions)
        {
            //     Start line number: 419
            //     --------------- Markdown ---------------
            //     +++ figure
            //     This is a figure!
            //     +++
            //     Caption
            //     +++
            //     --------------- Expected Markup ---------------
            //     <figure class="flexi-figure flexi-figure_has-name" id="figure-1">
            //     <div class="flexi-figure__content">
            //     <p>This is a figure!</p>
            //     </div>
            //     <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
            //     </figure>

            SpecTestHelper.AssertCompliance("+++ figure\nThis is a figure!\n+++\nCaption\n+++",
                "<figure class=\"flexi-figure flexi-figure_has-name\" id=\"figure-1\">\n<div class=\"flexi-figure__content\">\n<p>This is a figure!</p>\n</div>\n<figcaption class=\"flexi-figure__caption\"><span class=\"flexi-figure__name\">Figure 1. </span>Caption</figcaption>\n</figure>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiFigureBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiFigureBlocks_Spec13(string extensions)
        {
            //     Start line number: 435
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "generateID": false }
            //     +++ figure
            //     This is a figure!
            //     +++
            //     Caption
            //     +++
            //     --------------- Expected Markup ---------------
            //     <figure class="flexi-figure flexi-figure_has-name">
            //     <div class="flexi-figure__content">
            //     <p>This is a figure!</p>
            //     </div>
            //     <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
            //     </figure>

            SpecTestHelper.AssertCompliance("o{ \"generateID\": false }\n+++ figure\nThis is a figure!\n+++\nCaption\n+++",
                "<figure class=\"flexi-figure flexi-figure_has-name\">\n<div class=\"flexi-figure__content\">\n<p>This is a figure!</p>\n</div>\n<figcaption class=\"flexi-figure__caption\"><span class=\"flexi-figure__name\">Figure 1. </span>Caption</figcaption>\n</figure>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiFigureBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiFigureBlocks_Spec14(string extensions)
        {
            //     Start line number: 456
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ 
            //         "attributes": {
            //           "id" : "my-custom-id"
            //         }
            //     }
            //     +++ figure
            //     This is a figure!
            //     +++
            //     Caption
            //     +++
            //     --------------- Expected Markup ---------------
            //     <figure class="flexi-figure flexi-figure_has-name" id="my-custom-id">
            //     <div class="flexi-figure__content">
            //     <p>This is a figure!</p>
            //     </div>
            //     <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
            //     </figure>

            SpecTestHelper.AssertCompliance("o{ \n    \"attributes\": {\n      \"id\" : \"my-custom-id\"\n    }\n}\n+++ figure\nThis is a figure!\n+++\nCaption\n+++",
                "<figure class=\"flexi-figure flexi-figure_has-name\" id=\"my-custom-id\">\n<div class=\"flexi-figure__content\">\n<p>This is a figure!</p>\n</div>\n<figcaption class=\"flexi-figure__caption\"><span class=\"flexi-figure__name\">Figure 1. </span>Caption</figcaption>\n</figure>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiFigureBlocks")]
        [InlineData("All")]
        public void FlexiFigureBlocks_Spec15(string extensions)
        {
            //     Start line number: 487
            //     --------------- Markdown ---------------
            //     +++ figure
            //     This is a figure!
            //     +++
            //     Caption
            //     +++
            //     --------------- Expected Markup ---------------
            //     <figure class="flexi-figure flexi-figure_has-name" id="figure-1">
            //     <div class="flexi-figure__content">
            //     <p>This is a figure!</p>
            //     </div>
            //     <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
            //     </figure>

            SpecTestHelper.AssertCompliance("+++ figure\nThis is a figure!\n+++\nCaption\n+++",
                "<figure class=\"flexi-figure flexi-figure_has-name\" id=\"figure-1\">\n<div class=\"flexi-figure__content\">\n<p>This is a figure!</p>\n</div>\n<figcaption class=\"flexi-figure__caption\"><span class=\"flexi-figure__name\">Figure 1. </span>Caption</figcaption>\n</figure>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiFigureBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiFigureBlocks_Spec16(string extensions)
        {
            //     Start line number: 503
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{"renderName": false}
            //     +++ figure
            //     This is a figure!
            //     +++
            //     Caption
            //     +++
            //     --------------- Expected Markup ---------------
            //     <figure class="flexi-figure flexi-figure_no-name" id="figure-1">
            //     <div class="flexi-figure__content">
            //     <p>This is a figure!</p>
            //     </div>
            //     <figcaption class="flexi-figure__caption">Caption</figcaption>
            //     </figure>

            SpecTestHelper.AssertCompliance("o{\"renderName\": false}\n+++ figure\nThis is a figure!\n+++\nCaption\n+++",
                "<figure class=\"flexi-figure flexi-figure_no-name\" id=\"figure-1\">\n<div class=\"flexi-figure__content\">\n<p>This is a figure!</p>\n</div>\n<figcaption class=\"flexi-figure__caption\">Caption</figcaption>\n</figure>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiFigureBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiFigureBlocks_Spec17(string extensions)
        {
            //     Start line number: 531
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{
            //         "attributes": {
            //             "id" : "my-figure",
            //             "class" : "block"
            //         }
            //     }
            //     +++ figure
            //     This is a figure!
            //     +++
            //     Caption
            //     +++
            //     --------------- Expected Markup ---------------
            //     <figure class="flexi-figure flexi-figure_has-name block" id="my-figure">
            //     <div class="flexi-figure__content">
            //     <p>This is a figure!</p>
            //     </div>
            //     <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
            //     </figure>

            SpecTestHelper.AssertCompliance("o{\n    \"attributes\": {\n        \"id\" : \"my-figure\",\n        \"class\" : \"block\"\n    }\n}\n+++ figure\nThis is a figure!\n+++\nCaption\n+++",
                "<figure class=\"flexi-figure flexi-figure_has-name block\" id=\"my-figure\">\n<div class=\"flexi-figure__content\">\n<p>This is a figure!</p>\n</div>\n<figcaption class=\"flexi-figure__caption\"><span class=\"flexi-figure__name\">Figure 1. </span>Caption</figcaption>\n</figure>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiFigureBlocks")]
        [InlineData("All")]
        public void FlexiFigureBlocks_Spec18(string extensions)
        {
            //     Start line number: 570
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiFigureBlocks": {
            //             "defaultBlockOptions": {
            //                 "blockName": "figure",
            //                 "renderName": false
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     +++ figure
            //     This is a figure!
            //     +++
            //     Caption
            //     +++
            //     --------------- Expected Markup ---------------
            //     <figure class="figure figure_no-name" id="figure-1">
            //     <div class="figure__content">
            //     <p>This is a figure!</p>
            //     </div>
            //     <figcaption class="figure__caption">Caption</figcaption>
            //     </figure>

            SpecTestHelper.AssertCompliance("+++ figure\nThis is a figure!\n+++\nCaption\n+++",
                "<figure class=\"figure figure_no-name\" id=\"figure-1\">\n<div class=\"figure__content\">\n<p>This is a figure!</p>\n</div>\n<figcaption class=\"figure__caption\">Caption</figcaption>\n</figure>",
                extensions,
                false,
                "{\n    \"flexiFigureBlocks\": {\n        \"defaultBlockOptions\": {\n            \"blockName\": \"figure\",\n            \"renderName\": false\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiFigureBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiFigureBlocks_Spec19(string extensions)
        {
            //     Start line number: 595
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiFigureBlocks": {
            //             "defaultBlockOptions": {
            //                 "renderName": false
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     o{
            //         "renderName": true
            //     }
            //     +++ figure
            //     This is a figure!
            //     +++
            //     Caption
            //     +++
            //     
            //     +++ figure
            //     This is a figure!
            //     +++
            //     Caption
            //     +++
            //     --------------- Expected Markup ---------------
            //     <figure class="flexi-figure flexi-figure_has-name" id="figure-1">
            //     <div class="flexi-figure__content">
            //     <p>This is a figure!</p>
            //     </div>
            //     <figcaption class="flexi-figure__caption"><span class="flexi-figure__name">Figure 1. </span>Caption</figcaption>
            //     </figure>
            //     <figure class="flexi-figure flexi-figure_no-name" id="figure-2">
            //     <div class="flexi-figure__content">
            //     <p>This is a figure!</p>
            //     </div>
            //     <figcaption class="flexi-figure__caption">Caption</figcaption>
            //     </figure>

            SpecTestHelper.AssertCompliance("o{\n    \"renderName\": true\n}\n+++ figure\nThis is a figure!\n+++\nCaption\n+++\n\n+++ figure\nThis is a figure!\n+++\nCaption\n+++",
                "<figure class=\"flexi-figure flexi-figure_has-name\" id=\"figure-1\">\n<div class=\"flexi-figure__content\">\n<p>This is a figure!</p>\n</div>\n<figcaption class=\"flexi-figure__caption\"><span class=\"flexi-figure__name\">Figure 1. </span>Caption</figcaption>\n</figure>\n<figure class=\"flexi-figure flexi-figure_no-name\" id=\"figure-2\">\n<div class=\"flexi-figure__content\">\n<p>This is a figure!</p>\n</div>\n<figcaption class=\"flexi-figure__caption\">Caption</figcaption>\n</figure>",
                extensions,
                false,
                "{\n    \"flexiFigureBlocks\": {\n        \"defaultBlockOptions\": {\n            \"renderName\": false\n        }\n    }\n}");
        }
    }

    public class FlexiIncludeBlocksSpecs
    {
        [Theory]
        [InlineData("FlexiIncludeBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec1(string extensions)
        {
            //     Start line number: 85
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     i{ "source": "exampleInclude.js" }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">function exampleFunction(arg) {
            //         // Example comment
            //         return arg + 'dummyString';
            //     }
            //     
            //     //#region utility methods
            //     function add(a, b) {
            //         return a + b;
            //     }
            //     //#endregion utility methods
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("i{ \"source\": \"exampleInclude.js\" }",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">function exampleFunction(arg) {\n    // Example comment\n    return arg + 'dummyString';\n}\n\n//#region utility methods\nfunction add(a, b) {\n    return a + b;\n}\n//#endregion utility methods\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec2(string extensions)
        {
            //     Start line number: 113
            //     --------------- Markdown ---------------
            //     i{
            //         "type": "markdown",
            //         "source": "exampleInclude.md"    
            //     }
            //     --------------- Expected Markup ---------------
            //     <p>This is example markdown.</p>
            //     <ul>
            //     <li>This is a list item.</li>
            //     </ul>
            //     <blockquote>
            //     <p>This is a blockquote.</p>
            //     </blockquote>

            SpecTestHelper.AssertCompliance("i{\n    \"type\": \"markdown\",\n    \"source\": \"exampleInclude.md\"    \n}",
                "<p>This is example markdown.</p>\n<ul>\n<li>This is a list item.</li>\n</ul>\n<blockquote>\n<p>This is a blockquote.</p>\n</blockquote>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec3(string extensions)
        {
            //     Start line number: 130
            //     --------------- Markdown ---------------
            //     i {
            //         "type": "markdown",
            //         "source": "exampleInclude.md"    
            //     }
            //     --------------- Expected Markup ---------------
            //     <p>i {
            //     &quot;type&quot;: &quot;markdown&quot;,
            //     &quot;source&quot;: &quot;exampleInclude.md&quot;<br />
            //     }</p>

            SpecTestHelper.AssertCompliance("i {\n    \"type\": \"markdown\",\n    \"source\": \"exampleInclude.md\"    \n}",
                "<p>i {\n&quot;type&quot;: &quot;markdown&quot;,\n&quot;source&quot;: &quot;exampleInclude.md&quot;<br />\n}</p>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec4(string extensions)
        {
            //     Start line number: 167
            //     --------------- Markdown ---------------
            //     i{
            //         "type": "markdown",
            //         "source": "https://raw.githubusercontent.com/JeringTech/Markdig.Extensions.FlexiBlocks/cf4cc222079d2c3845c74826bd7aa1c2c6fd967f/test/FlexiBlocks/exampleInclude.md"
            //     }
            //     --------------- Expected Markup ---------------
            //     <p>This is example markdown.</p>
            //     <ul>
            //     <li>This is a list item.</li>
            //     </ul>
            //     <blockquote>
            //     <p>This is a blockquote.</p>
            //     </blockquote>

            SpecTestHelper.AssertCompliance("i{\n    \"type\": \"markdown\",\n    \"source\": \"https://raw.githubusercontent.com/JeringTech/Markdig.Extensions.FlexiBlocks/cf4cc222079d2c3845c74826bd7aa1c2c6fd967f/test/FlexiBlocks/exampleInclude.md\"\n}",
                "<p>This is example markdown.</p>\n<ul>\n<li>This is a list item.</li>\n</ul>\n<blockquote>\n<p>This is a blockquote.</p>\n</blockquote>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec5(string extensions)
        {
            //     Start line number: 190
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     i{
            //         "source": "exampleInclude.js",
            //         "clippings":[{"endLine": 4}, {"startLine": 7, "endLine": -2}]
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">function exampleFunction(arg) {
            //         // Example comment
            //         return arg + 'dummyString';
            //     }
            //     function add(a, b) {
            //         return a + b;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("i{\n    \"source\": \"exampleInclude.js\",\n    \"clippings\":[{\"endLine\": 4}, {\"startLine\": 7, \"endLine\": -2}]\n}",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">function exampleFunction(arg) {\n    // Example comment\n    return arg + 'dummyString';\n}\nfunction add(a, b) {\n    return a + b;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec6(string extensions)
        {
            //     Start line number: 217
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     i{
            //         "source": "exampleInclude.js",
            //         "clippings":[{"region": "utility methods"}]
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">function add(a, b) {
            //         return a + b;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("i{\n    \"source\": \"exampleInclude.js\",\n    \"clippings\":[{\"region\": \"utility methods\"}]\n}",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">function add(a, b) {\n    return a + b;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec7(string extensions)
        {
            //     Start line number: 240
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     i{
            //         "source": "exampleInclude.js",
            //         "clippings":[{"startString": "#region utility methods", "endString": "#endregion utility methods"}]
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">function add(a, b) {
            //         return a + b;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("i{\n    \"source\": \"exampleInclude.js\",\n    \"clippings\":[{\"startString\": \"#region utility methods\", \"endString\": \"#endregion utility methods\"}]\n}",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">function add(a, b) {\n    return a + b;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec8(string extensions)
        {
            //     Start line number: 263
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     i{
            //         "source": "exampleInclude.js",
            //         "clippings":[{"startLine": 7, "endString": "#endregion utility methods"}]
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">function add(a, b) {
            //         return a + b;
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("i{\n    \"source\": \"exampleInclude.js\",\n    \"clippings\":[{\"startLine\": 7, \"endString\": \"#endregion utility methods\"}]\n}",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">function add(a, b) {\n    return a + b;\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec9(string extensions)
        {
            //     Start line number: 286
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     i{
            //         "source": "exampleInclude.js",
            //         "clippings":[{
            //             "endLine": 1,
            //             "after": "..."
            //         },
            //         {
            //             "startLine": 4,
            //             "endLine": 4
            //         },
            //         {
            //             "startLine": 7, 
            //             "endLine": 7,
            //             "before": ""
            //         },
            //         {
            //             "startLine": 9, 
            //             "endLine": 9,
            //             "before": "..."
            //         }]
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">function exampleFunction(arg) {
            //     ...
            //     }
            //     
            //     function add(a, b) {
            //     ...
            //     }
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("i{\n    \"source\": \"exampleInclude.js\",\n    \"clippings\":[{\n        \"endLine\": 1,\n        \"after\": \"...\"\n    },\n    {\n        \"startLine\": 4,\n        \"endLine\": 4\n    },\n    {\n        \"startLine\": 7, \n        \"endLine\": 7,\n        \"before\": \"\"\n    },\n    {\n        \"startLine\": 9, \n        \"endLine\": 9,\n        \"before\": \"...\"\n    }]\n}",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">function exampleFunction(arg) {\n...\n}\n\nfunction add(a, b) {\n...\n}\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec10(string extensions)
        {
            //     Start line number: 330
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     i{
            //         "source": "exampleInclude.js",
            //         "clippings":[{"dedent": 2}],
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">function exampleFunction(arg) {
            //       // Example comment
            //       return arg + 'dummyString';
            //     }
            //     
            //     //#region utility methods
            //     function add(a, b) {
            //       return a + b;
            //     }
            //     //#endregion utility methods
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("i{\n    \"source\": \"exampleInclude.js\",\n    \"clippings\":[{\"dedent\": 2}],\n}",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">function exampleFunction(arg) {\n  // Example comment\n  return arg + 'dummyString';\n}\n\n//#region utility methods\nfunction add(a, b) {\n  return a + b;\n}\n//#endregion utility methods\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec11(string extensions)
        {
            //     Start line number: 360
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     i{
            //         "source": "exampleInclude.js",
            //         "clippings":[{"indent": 2}],
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">  function exampleFunction(arg) {
            //           // Example comment
            //           return arg + 'dummyString';
            //       }
            //     
            //       //#region utility methods
            //       function add(a, b) {
            //           return a + b;
            //       }
            //       //#endregion utility methods
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("i{\n    \"source\": \"exampleInclude.js\",\n    \"clippings\":[{\"indent\": 2}],\n}",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">  function exampleFunction(arg) {\n      // Example comment\n      return arg + 'dummyString';\n  }\n\n  //#region utility methods\n  function add(a, b) {\n      return a + b;\n  }\n  //#endregion utility methods\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec12(string extensions)
        {
            //     Start line number: 390
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     i{
            //         "source": "exampleInclude.js",
            //         "clippings":[{"collapse": 0.5}]
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">function exampleFunction(arg) {
            //       // Example comment
            //       return arg + 'dummyString';
            //     }
            //     
            //     //#region utility methods
            //     function add(a, b) {
            //       return a + b;
            //     }
            //     //#endregion utility methods
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("i{\n    \"source\": \"exampleInclude.js\",\n    \"clippings\":[{\"collapse\": 0.5}]\n}",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">function exampleFunction(arg) {\n  // Example comment\n  return arg + 'dummyString';\n}\n\n//#region utility methods\nfunction add(a, b) {\n  return a + b;\n}\n//#endregion utility methods\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec13(string extensions)
        {
            //     Start line number: 426
            //     --------------- Markdown ---------------
            //     i{
            //         "type": "markdown",
            //         "source": "exampleInclude.md"
            //     }
            //     --------------- Expected Markup ---------------
            //     <p>This is example markdown.</p>
            //     <ul>
            //     <li>This is a list item.</li>
            //     </ul>
            //     <blockquote>
            //     <p>This is a blockquote.</p>
            //     </blockquote>

            SpecTestHelper.AssertCompliance("i{\n    \"type\": \"markdown\",\n    \"source\": \"exampleInclude.md\"\n}",
                "<p>This is example markdown.</p>\n<ul>\n<li>This is a list item.</li>\n</ul>\n<blockquote>\n<p>This is a blockquote.</p>\n</blockquote>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec14(string extensions)
        {
            //     Start line number: 457
            //     --------------- Markdown ---------------
            //     i{
            //         "cacheOnDisk": false,
            //         "type": "markdown",
            //         "source": "https://raw.githubusercontent.com/JeringTech/Markdig.Extensions.FlexiBlocks/cf4cc222079d2c3845c74826bd7aa1c2c6fd967f/test/FlexiBlocks/exampleInclude.md"
            //     }
            //     --------------- Expected Markup ---------------
            //     <p>This is example markdown.</p>
            //     <ul>
            //     <li>This is a list item.</li>
            //     </ul>
            //     <blockquote>
            //     <p>This is a blockquote.</p>
            //     </blockquote>

            SpecTestHelper.AssertCompliance("i{\n    \"cacheOnDisk\": false,\n    \"type\": \"markdown\",\n    \"source\": \"https://raw.githubusercontent.com/JeringTech/Markdig.Extensions.FlexiBlocks/cf4cc222079d2c3845c74826bd7aa1c2c6fd967f/test/FlexiBlocks/exampleInclude.md\"\n}",
                "<p>This is example markdown.</p>\n<ul>\n<li>This is a list item.</li>\n</ul>\n<blockquote>\n<p>This is a blockquote.</p>\n</blockquote>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec15(string extensions)
        {
            //     Start line number: 572
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiIncludeBlocks": {
            //             "defaultBlockOptions": {
            //                 "type": "markdown"
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     i{
            //         "source": "exampleInclude.md"
            //     }
            //     
            //     i{
            //         "source": "exampleInclude.md"
            //     }
            //     --------------- Expected Markup ---------------
            //     <p>This is example markdown.</p>
            //     <ul>
            //     <li>This is a list item.</li>
            //     </ul>
            //     <blockquote>
            //     <p>This is a blockquote.</p>
            //     </blockquote>
            //     <p>This is example markdown.</p>
            //     <ul>
            //     <li>This is a list item.</li>
            //     </ul>
            //     <blockquote>
            //     <p>This is a blockquote.</p>
            //     </blockquote>

            SpecTestHelper.AssertCompliance("i{\n    \"source\": \"exampleInclude.md\"\n}\n\ni{\n    \"source\": \"exampleInclude.md\"\n}",
                "<p>This is example markdown.</p>\n<ul>\n<li>This is a list item.</li>\n</ul>\n<blockquote>\n<p>This is a blockquote.</p>\n</blockquote>\n<p>This is example markdown.</p>\n<ul>\n<li>This is a list item.</li>\n</ul>\n<blockquote>\n<p>This is a blockquote.</p>\n</blockquote>",
                extensions,
                false,
                "{\n    \"flexiIncludeBlocks\": {\n        \"defaultBlockOptions\": {\n            \"type\": \"markdown\"\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec16(string extensions)
        {
            //     Start line number: 607
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiIncludeBlocks": {
            //             "defaultBlockOptions": {
            //                 "type": "markdown"
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     i{
            //         "source": "exampleInclude.md"
            //     }
            //     
            //     i{
            //         "type": "code",
            //         "source": "exampleInclude.md"
            //     }
            //     --------------- Expected Markup ---------------
            //     <p>This is example markdown.</p>
            //     <ul>
            //     <li>This is a list item.</li>
            //     </ul>
            //     <blockquote>
            //     <p>This is a blockquote.</p>
            //     </blockquote>
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">This is example markdown.
            //     - This is a list item.
            //     &gt; This is a blockquote.
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("i{\n    \"source\": \"exampleInclude.md\"\n}\n\ni{\n    \"type\": \"code\",\n    \"source\": \"exampleInclude.md\"\n}",
                "<p>This is example markdown.</p>\n<ul>\n<li>This is a list item.</li>\n</ul>\n<blockquote>\n<p>This is a blockquote.</p>\n</blockquote>\n<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">This is example markdown.\n- This is a list item.\n&gt; This is a blockquote.\n</code></pre>\n</div>",
                extensions,
                false,
                "{\n    \"flexiIncludeBlocks\": {\n        \"defaultBlockOptions\": {\n            \"type\": \"markdown\"\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec17(string extensions)
        {
            //     Start line number: 655
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiIncludeBlocks": {
            //             "baseUri": "https://raw.githubusercontent.com"
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     i{
            //         "type": "markdown",
            //         "source": "JeringTech/Markdig.Extensions.FlexiBlocks/cf4cc222079d2c3845c74826bd7aa1c2c6fd967f/test/FlexiBlocks/exampleInclude.md"
            //     }
            //     --------------- Expected Markup ---------------
            //     <p>This is example markdown.</p>
            //     <ul>
            //     <li>This is a list item.</li>
            //     </ul>
            //     <blockquote>
            //     <p>This is a blockquote.</p>
            //     </blockquote>

            SpecTestHelper.AssertCompliance("i{\n    \"type\": \"markdown\",\n    \"source\": \"JeringTech/Markdig.Extensions.FlexiBlocks/cf4cc222079d2c3845c74826bd7aa1c2c6fd967f/test/FlexiBlocks/exampleInclude.md\"\n}",
                "<p>This is example markdown.</p>\n<ul>\n<li>This is a list item.</li>\n</ul>\n<blockquote>\n<p>This is a blockquote.</p>\n</blockquote>",
                extensions,
                false,
                "{\n    \"flexiIncludeBlocks\": {\n        \"baseUri\": \"https://raw.githubusercontent.com\"\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks_FlexiOptionsBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec18(string extensions)
        {
            //     Start line number: 683
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     o{
            //         "language": "javascript"
            //     }
            //     i{
            //         "source": "exampleInclude.js"
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_language_javascript flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code"><span class="token keyword">function</span> <span class="token function">exampleFunction</span><span class="token punctuation">(</span>arg<span class="token punctuation">)</span> <span class="token punctuation">{</span>
            //         <span class="token comment">// Example comment</span>
            //         <span class="token keyword">return</span> arg <span class="token operator">+</span> <span class="token string">'dummyString'</span><span class="token punctuation">;</span>
            //     <span class="token punctuation">}</span>
            //     
            //     <span class="token comment">//#region utility methods</span>
            //     <span class="token keyword">function</span> <span class="token function">add</span><span class="token punctuation">(</span>a<span class="token punctuation">,</span> b<span class="token punctuation">)</span> <span class="token punctuation">{</span>
            //         <span class="token keyword">return</span> a <span class="token operator">+</span> b<span class="token punctuation">;</span>
            //     <span class="token punctuation">}</span>
            //     <span class="token comment">//#endregion utility methods</span>
            //     </code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("o{\n    \"language\": \"javascript\"\n}\ni{\n    \"source\": \"exampleInclude.js\"\n}",
                "<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_language_javascript flexi-code_has-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\"><span class=\"token keyword\">function</span> <span class=\"token function\">exampleFunction</span><span class=\"token punctuation\">(</span>arg<span class=\"token punctuation\">)</span> <span class=\"token punctuation\">{</span>\n    <span class=\"token comment\">// Example comment</span>\n    <span class=\"token keyword\">return</span> arg <span class=\"token operator\">+</span> <span class=\"token string\">'dummyString'</span><span class=\"token punctuation\">;</span>\n<span class=\"token punctuation\">}</span>\n\n<span class=\"token comment\">//#region utility methods</span>\n<span class=\"token keyword\">function</span> <span class=\"token function\">add</span><span class=\"token punctuation\">(</span>a<span class=\"token punctuation\">,</span> b<span class=\"token punctuation\">)</span> <span class=\"token punctuation\">{</span>\n    <span class=\"token keyword\">return</span> a <span class=\"token operator\">+</span> b<span class=\"token punctuation\">;</span>\n<span class=\"token punctuation\">}</span>\n<span class=\"token comment\">//#endregion utility methods</span>\n</code></pre>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec19(string extensions)
        {
            //     Start line number: 717
            //     --------------- Markdown ---------------
            //     i{
            //         "type": "markdown",
            //         "source": "https://raw.githubusercontent.com/JeringTech/Markdig.Extensions.FlexiBlocks/cf4cc222079d2c3845c74826bd7aa1c2c6fd967f/test/FlexiBlocks/exampleIncludeWithNestedInclude.md"
            //     }
            //     --------------- Expected Markup ---------------
            //     <p>This is example markdown with an include.</p>
            //     <p>This is example markdown.</p>
            //     <ul>
            //     <li>This is a list item.</li>
            //     </ul>
            //     <blockquote>
            //     <p>This is a blockquote.</p>
            //     </blockquote>

            SpecTestHelper.AssertCompliance("i{\n    \"type\": \"markdown\",\n    \"source\": \"https://raw.githubusercontent.com/JeringTech/Markdig.Extensions.FlexiBlocks/cf4cc222079d2c3845c74826bd7aa1c2c6fd967f/test/FlexiBlocks/exampleIncludeWithNestedInclude.md\"\n}",
                "<p>This is example markdown with an include.</p>\n<p>This is example markdown.</p>\n<ul>\n<li>This is a list item.</li>\n</ul>\n<blockquote>\n<p>This is a blockquote.</p>\n</blockquote>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec20(string extensions)
        {
            //     Start line number: 735
            //     --------------- Markdown ---------------
            //     - First item.
            //     - Second item  
            //     
            //       i{
            //           "type": "markdown",
            //           "source": "exampleInclude.md"
            //       }
            //     - Third item
            //     --------------- Expected Markup ---------------
            //     <ul>
            //     <li><p>First item.</p></li>
            //     <li><p>Second item</p>
            //     <p>This is example markdown.</p>
            //     <ul>
            //     <li>This is a list item.</li>
            //     </ul>
            //     <blockquote>
            //     <p>This is a blockquote.</p>
            //     </blockquote></li>
            //     <li><p>Third item</p></li>
            //     </ul>

            SpecTestHelper.AssertCompliance("- First item.\n- Second item  \n\n  i{\n      \"type\": \"markdown\",\n      \"source\": \"exampleInclude.md\"\n  }\n- Third item",
                "<ul>\n<li><p>First item.</p></li>\n<li><p>Second item</p>\n<p>This is example markdown.</p>\n<ul>\n<li>This is a list item.</li>\n</ul>\n<blockquote>\n<p>This is a blockquote.</p>\n</blockquote></li>\n<li><p>Third item</p></li>\n</ul>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec21(string extensions)
        {
            //     Start line number: 761
            //     --------------- Markdown ---------------
            //     > First line.
            //     > i{
            //     >     "type": "markdown",
            //     >     "source": "exampleInclude.md"
            //     > }
            //     > Third line
            //     --------------- Expected Markup ---------------
            //     <blockquote>
            //     <p>First line.</p>
            //     <p>This is example markdown.</p>
            //     <ul>
            //     <li>This is a list item.</li>
            //     </ul>
            //     <blockquote>
            //     <p>This is a blockquote.</p>
            //     </blockquote>
            //     <p>Third line</p>
            //     </blockquote>

            SpecTestHelper.AssertCompliance("> First line.\n> i{\n>     \"type\": \"markdown\",\n>     \"source\": \"exampleInclude.md\"\n> }\n> Third line",
                "<blockquote>\n<p>First line.</p>\n<p>This is example markdown.</p>\n<ul>\n<li>This is a list item.</li>\n</ul>\n<blockquote>\n<p>This is a blockquote.</p>\n</blockquote>\n<p>Third line</p>\n</blockquote>",
                extensions,
                false);
        }
    }

    public class FlexiPictureBlocksSpecs
    {
        [Theory]
        [InlineData("FlexiPictureBlocks")]
        [InlineData("All")]
        public void FlexiPictureBlocks_Spec1(string extensions)
        {
            //     Start line number: 56
            //     --------------- Markdown ---------------
            //     p{ 
            //       "src": "/file.png",
            //       "alt": "Alternative text"
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner">
            //     <button class="flexi-picture__exit-fullscreen-button" title="Exit fullscreen" aria-label="Exit fullscreen">
            //     <svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     </button>
            //     <div class="flexi-picture__container">
            //     <div class="flexi-picture__error-notice">
            //     <svg class="flexi-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-picture__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     <div class="flexi-picture__picture-container">
            //     <picture class="flexi-picture__picture">
            //     <img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
            //     </picture>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("p{ \n  \"src\": \"/file.png\",\n  \"alt\": \"Alternative text\"\n}",
                "<div class=\"flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner\">\n<button class=\"flexi-picture__exit-fullscreen-button\" title=\"Exit fullscreen\" aria-label=\"Exit fullscreen\">\n<svg class=\"flexi-picture__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n</button>\n<div class=\"flexi-picture__container\">\n<div class=\"flexi-picture__error-notice\">\n<svg class=\"flexi-picture__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-picture__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n<div class=\"flexi-picture__picture-container\">\n<picture class=\"flexi-picture__picture\">\n<img class=\"flexi-picture__image\" data-src=\"/file.png\" alt=\"Alternative text\" tabindex=\"-1\">\n</picture>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiPictureBlocks")]
        [InlineData("All")]
        public void FlexiPictureBlocks_Spec2(string extensions)
        {
            //     Start line number: 103
            //     --------------- Markdown ---------------
            //     p{ 
            //       "blockName": "picture",
            //       "src": "/file.png",
            //       "alt": "Alternative text"
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="picture picture_has-alt picture_is-lazy picture_no-width picture_no-aspect-ratio picture_has-exit-fullscreen-icon picture_has-error-icon picture_has-spinner">
            //     <button class="picture__exit-fullscreen-button" title="Exit fullscreen" aria-label="Exit fullscreen">
            //     <svg class="picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     </button>
            //     <div class="picture__container">
            //     <div class="picture__error-notice">
            //     <svg class="picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="picture__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     <div class="picture__picture-container">
            //     <picture class="picture__picture">
            //     <img class="picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
            //     </picture>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("p{ \n  \"blockName\": \"picture\",\n  \"src\": \"/file.png\",\n  \"alt\": \"Alternative text\"\n}",
                "<div class=\"picture picture_has-alt picture_is-lazy picture_no-width picture_no-aspect-ratio picture_has-exit-fullscreen-icon picture_has-error-icon picture_has-spinner\">\n<button class=\"picture__exit-fullscreen-button\" title=\"Exit fullscreen\" aria-label=\"Exit fullscreen\">\n<svg class=\"picture__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n</button>\n<div class=\"picture__container\">\n<div class=\"picture__error-notice\">\n<svg class=\"picture__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"picture__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n<div class=\"picture__picture-container\">\n<picture class=\"picture__picture\">\n<img class=\"picture__image\" data-src=\"/file.png\" alt=\"Alternative text\" tabindex=\"-1\">\n</picture>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiPictureBlocks")]
        [InlineData("All")]
        public void FlexiPictureBlocks_Spec3(string extensions)
        {
            //     Start line number: 141
            //     --------------- Markdown ---------------
            //     p{ 
            //       "src": "/file.png",
            //       "alt": "Alternative text"
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner">
            //     <button class="flexi-picture__exit-fullscreen-button" title="Exit fullscreen" aria-label="Exit fullscreen">
            //     <svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     </button>
            //     <div class="flexi-picture__container">
            //     <div class="flexi-picture__error-notice">
            //     <svg class="flexi-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-picture__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     <div class="flexi-picture__picture-container">
            //     <picture class="flexi-picture__picture">
            //     <img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
            //     </picture>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("p{ \n  \"src\": \"/file.png\",\n  \"alt\": \"Alternative text\"\n}",
                "<div class=\"flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner\">\n<button class=\"flexi-picture__exit-fullscreen-button\" title=\"Exit fullscreen\" aria-label=\"Exit fullscreen\">\n<svg class=\"flexi-picture__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n</button>\n<div class=\"flexi-picture__container\">\n<div class=\"flexi-picture__error-notice\">\n<svg class=\"flexi-picture__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-picture__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n<div class=\"flexi-picture__picture-container\">\n<picture class=\"flexi-picture__picture\">\n<img class=\"flexi-picture__image\" data-src=\"/file.png\" alt=\"Alternative text\" tabindex=\"-1\">\n</picture>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiPictureBlocks")]
        [InlineData("All")]
        public void FlexiPictureBlocks_Spec4(string extensions)
        {
            //     Start line number: 179
            //     --------------- Markdown ---------------
            //     p{ 
            //       "src": "/file.png",
            //       "alt": "Alternative text"
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner">
            //     <button class="flexi-picture__exit-fullscreen-button" title="Exit fullscreen" aria-label="Exit fullscreen">
            //     <svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     </button>
            //     <div class="flexi-picture__container">
            //     <div class="flexi-picture__error-notice">
            //     <svg class="flexi-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-picture__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     <div class="flexi-picture__picture-container">
            //     <picture class="flexi-picture__picture">
            //     <img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
            //     </picture>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("p{ \n  \"src\": \"/file.png\",\n  \"alt\": \"Alternative text\"\n}",
                "<div class=\"flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner\">\n<button class=\"flexi-picture__exit-fullscreen-button\" title=\"Exit fullscreen\" aria-label=\"Exit fullscreen\">\n<svg class=\"flexi-picture__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n</button>\n<div class=\"flexi-picture__container\">\n<div class=\"flexi-picture__error-notice\">\n<svg class=\"flexi-picture__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-picture__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n<div class=\"flexi-picture__picture-container\">\n<picture class=\"flexi-picture__picture\">\n<img class=\"flexi-picture__image\" data-src=\"/file.png\" alt=\"Alternative text\" tabindex=\"-1\">\n</picture>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiPictureBlocks")]
        [InlineData("All")]
        public void FlexiPictureBlocks_Spec5(string extensions)
        {
            //     Start line number: 210
            //     --------------- Markdown ---------------
            //     p{ 
            //       "src": "/file.png",
            //       "alt": null
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-picture flexi-picture_no-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner">
            //     <button class="flexi-picture__exit-fullscreen-button" title="Exit fullscreen" aria-label="Exit fullscreen">
            //     <svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     </button>
            //     <div class="flexi-picture__container">
            //     <div class="flexi-picture__error-notice">
            //     <svg class="flexi-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-picture__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     <div class="flexi-picture__picture-container">
            //     <picture class="flexi-picture__picture">
            //     <img class="flexi-picture__image" data-src="/file.png" tabindex="-1">
            //     </picture>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("p{ \n  \"src\": \"/file.png\",\n  \"alt\": null\n}",
                "<div class=\"flexi-picture flexi-picture_no-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner\">\n<button class=\"flexi-picture__exit-fullscreen-button\" title=\"Exit fullscreen\" aria-label=\"Exit fullscreen\">\n<svg class=\"flexi-picture__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n</button>\n<div class=\"flexi-picture__container\">\n<div class=\"flexi-picture__error-notice\">\n<svg class=\"flexi-picture__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-picture__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n<div class=\"flexi-picture__picture-container\">\n<picture class=\"flexi-picture__picture\">\n<img class=\"flexi-picture__image\" data-src=\"/file.png\" tabindex=\"-1\">\n</picture>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiPictureBlocks")]
        [InlineData("All")]
        public void FlexiPictureBlocks_Spec6(string extensions)
        {
            //     Start line number: 253
            //     --------------- Markdown ---------------
            //     p{ 
            //       "src": "/file.png",
            //       "alt": "Alternative text"
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner">
            //     <button class="flexi-picture__exit-fullscreen-button" title="Exit fullscreen" aria-label="Exit fullscreen">
            //     <svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     </button>
            //     <div class="flexi-picture__container">
            //     <div class="flexi-picture__error-notice">
            //     <svg class="flexi-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-picture__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     <div class="flexi-picture__picture-container">
            //     <picture class="flexi-picture__picture">
            //     <img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
            //     </picture>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("p{ \n  \"src\": \"/file.png\",\n  \"alt\": \"Alternative text\"\n}",
                "<div class=\"flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner\">\n<button class=\"flexi-picture__exit-fullscreen-button\" title=\"Exit fullscreen\" aria-label=\"Exit fullscreen\">\n<svg class=\"flexi-picture__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n</button>\n<div class=\"flexi-picture__container\">\n<div class=\"flexi-picture__error-notice\">\n<svg class=\"flexi-picture__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-picture__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n<div class=\"flexi-picture__picture-container\">\n<picture class=\"flexi-picture__picture\">\n<img class=\"flexi-picture__image\" data-src=\"/file.png\" alt=\"Alternative text\" tabindex=\"-1\">\n</picture>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiPictureBlocks")]
        [InlineData("All")]
        public void FlexiPictureBlocks_Spec7(string extensions)
        {
            //     Start line number: 284
            //     --------------- Markdown ---------------
            //     p{ 
            //       "src": "/file.png",
            //       "alt": "Alternative text",
            //       "lazy": false
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-picture flexi-picture_has-alt flexi-picture_not-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner">
            //     <button class="flexi-picture__exit-fullscreen-button" title="Exit fullscreen" aria-label="Exit fullscreen">
            //     <svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     </button>
            //     <div class="flexi-picture__container">
            //     <div class="flexi-picture__error-notice">
            //     <svg class="flexi-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-picture__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     <div class="flexi-picture__picture-container">
            //     <picture class="flexi-picture__picture">
            //     <img class="flexi-picture__image" src="/file.png" alt="Alternative text" tabindex="-1">
            //     </picture>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("p{ \n  \"src\": \"/file.png\",\n  \"alt\": \"Alternative text\",\n  \"lazy\": false\n}",
                "<div class=\"flexi-picture flexi-picture_has-alt flexi-picture_not-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner\">\n<button class=\"flexi-picture__exit-fullscreen-button\" title=\"Exit fullscreen\" aria-label=\"Exit fullscreen\">\n<svg class=\"flexi-picture__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n</button>\n<div class=\"flexi-picture__container\">\n<div class=\"flexi-picture__error-notice\">\n<svg class=\"flexi-picture__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-picture__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n<div class=\"flexi-picture__picture-container\">\n<picture class=\"flexi-picture__picture\">\n<img class=\"flexi-picture__image\" src=\"/file.png\" alt=\"Alternative text\" tabindex=\"-1\">\n</picture>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiPictureBlocks")]
        [InlineData("All")]
        public void FlexiPictureBlocks_Spec8(string extensions)
        {
            //     Start line number: 328
            //     --------------- Markdown ---------------
            //     p{ 
            //       "src": "/file.png",
            //       "alt": "Alternative text",
            //       "width": 123
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_has-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner">
            //     <button class="flexi-picture__exit-fullscreen-button" title="Exit fullscreen" aria-label="Exit fullscreen">
            //     <svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     </button>
            //     <div class="flexi-picture__container" style="width:123px">
            //     <div class="flexi-picture__error-notice">
            //     <svg class="flexi-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-picture__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     <div class="flexi-picture__picture-container" style="width:123px">
            //     <picture class="flexi-picture__picture">
            //     <img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
            //     </picture>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("p{ \n  \"src\": \"/file.png\",\n  \"alt\": \"Alternative text\",\n  \"width\": 123\n}",
                "<div class=\"flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_has-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner\">\n<button class=\"flexi-picture__exit-fullscreen-button\" title=\"Exit fullscreen\" aria-label=\"Exit fullscreen\">\n<svg class=\"flexi-picture__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n</button>\n<div class=\"flexi-picture__container\" style=\"width:123px\">\n<div class=\"flexi-picture__error-notice\">\n<svg class=\"flexi-picture__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-picture__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n<div class=\"flexi-picture__picture-container\" style=\"width:123px\">\n<picture class=\"flexi-picture__picture\">\n<img class=\"flexi-picture__image\" data-src=\"/file.png\" alt=\"Alternative text\" tabindex=\"-1\">\n</picture>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiPictureBlocks")]
        [InlineData("All")]
        public void FlexiPictureBlocks_Spec9(string extensions)
        {
            //     Start line number: 372
            //     --------------- Markdown ---------------
            //     p{ 
            //       "src": "/file.png",
            //       "alt": "Alternative text",
            //       "width": 123,
            //       "height": 321
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_has-width flexi-picture_has-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner">
            //     <button class="flexi-picture__exit-fullscreen-button" title="Exit fullscreen" aria-label="Exit fullscreen">
            //     <svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     </button>
            //     <div class="flexi-picture__container" style="width:123px">
            //     <div class="flexi-picture__error-notice">
            //     <svg class="flexi-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-picture__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     <div class="flexi-picture__picture-container" style="width:123px">
            //     <picture class="flexi-picture__picture" style="padding-bottom:260.975609756098%">
            //     <img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
            //     </picture>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("p{ \n  \"src\": \"/file.png\",\n  \"alt\": \"Alternative text\",\n  \"width\": 123,\n  \"height\": 321\n}",
                "<div class=\"flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_has-width flexi-picture_has-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner\">\n<button class=\"flexi-picture__exit-fullscreen-button\" title=\"Exit fullscreen\" aria-label=\"Exit fullscreen\">\n<svg class=\"flexi-picture__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n</button>\n<div class=\"flexi-picture__container\" style=\"width:123px\">\n<div class=\"flexi-picture__error-notice\">\n<svg class=\"flexi-picture__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-picture__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n<div class=\"flexi-picture__picture-container\" style=\"width:123px\">\n<picture class=\"flexi-picture__picture\" style=\"padding-bottom:260.975609756098%\">\n<img class=\"flexi-picture__image\" data-src=\"/file.png\" alt=\"Alternative text\" tabindex=\"-1\">\n</picture>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiPictureBlocks")]
        [InlineData("All")]
        public void FlexiPictureBlocks_Spec10(string extensions)
        {
            //     Start line number: 405
            //     --------------- Markdown ---------------
            //     p{ 
            //       "src": "/file.png",
            //       "alt": "Alternative text",
            //       "width": 0,
            //       "height": 321
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner">
            //     <button class="flexi-picture__exit-fullscreen-button" title="Exit fullscreen" aria-label="Exit fullscreen">
            //     <svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     </button>
            //     <div class="flexi-picture__container">
            //     <div class="flexi-picture__error-notice">
            //     <svg class="flexi-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-picture__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     <div class="flexi-picture__picture-container">
            //     <picture class="flexi-picture__picture">
            //     <img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
            //     </picture>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("p{ \n  \"src\": \"/file.png\",\n  \"alt\": \"Alternative text\",\n  \"width\": 0,\n  \"height\": 321\n}",
                "<div class=\"flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner\">\n<button class=\"flexi-picture__exit-fullscreen-button\" title=\"Exit fullscreen\" aria-label=\"Exit fullscreen\">\n<svg class=\"flexi-picture__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n</button>\n<div class=\"flexi-picture__container\">\n<div class=\"flexi-picture__error-notice\">\n<svg class=\"flexi-picture__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-picture__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n<div class=\"flexi-picture__picture-container\">\n<picture class=\"flexi-picture__picture\">\n<img class=\"flexi-picture__image\" data-src=\"/file.png\" alt=\"Alternative text\" tabindex=\"-1\">\n</picture>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiPictureBlocks")]
        [InlineData("All")]
        public void FlexiPictureBlocks_Spec11(string extensions)
        {
            //     Start line number: 445
            //     --------------- Markdown ---------------
            //     p{ 
            //       "src": "/file.png",
            //       "alt": "Alternative text",
            //       "exitFullscreenIcon": "<svg><use xlink:href=\"#exit-fullscreen-icon\"/></svg>"
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner">
            //     <button class="flexi-picture__exit-fullscreen-button" title="Exit fullscreen" aria-label="Exit fullscreen">
            //     <svg class="flexi-picture__exit-fullscreen-icon"><use xlink:href="#exit-fullscreen-icon"/></svg>
            //     </button>
            //     <div class="flexi-picture__container">
            //     <div class="flexi-picture__error-notice">
            //     <svg class="flexi-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-picture__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     <div class="flexi-picture__picture-container">
            //     <picture class="flexi-picture__picture">
            //     <img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
            //     </picture>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("p{ \n  \"src\": \"/file.png\",\n  \"alt\": \"Alternative text\",\n  \"exitFullscreenIcon\": \"<svg><use xlink:href=\\\"#exit-fullscreen-icon\\\"/></svg>\"\n}",
                "<div class=\"flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner\">\n<button class=\"flexi-picture__exit-fullscreen-button\" title=\"Exit fullscreen\" aria-label=\"Exit fullscreen\">\n<svg class=\"flexi-picture__exit-fullscreen-icon\"><use xlink:href=\"#exit-fullscreen-icon\"/></svg>\n</button>\n<div class=\"flexi-picture__container\">\n<div class=\"flexi-picture__error-notice\">\n<svg class=\"flexi-picture__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-picture__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n<div class=\"flexi-picture__picture-container\">\n<picture class=\"flexi-picture__picture\">\n<img class=\"flexi-picture__image\" data-src=\"/file.png\" alt=\"Alternative text\" tabindex=\"-1\">\n</picture>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiPictureBlocks")]
        [InlineData("All")]
        public void FlexiPictureBlocks_Spec12(string extensions)
        {
            //     Start line number: 477
            //     --------------- Markdown ---------------
            //     p{ 
            //       "src": "/file.png",
            //       "alt": "Alternative text",
            //       "exitFullscreenIcon": null
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_no-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner">
            //     <button class="flexi-picture__exit-fullscreen-button" title="Exit fullscreen" aria-label="Exit fullscreen">
            //     </button>
            //     <div class="flexi-picture__container">
            //     <div class="flexi-picture__error-notice">
            //     <svg class="flexi-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-picture__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     <div class="flexi-picture__picture-container">
            //     <picture class="flexi-picture__picture">
            //     <img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
            //     </picture>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("p{ \n  \"src\": \"/file.png\",\n  \"alt\": \"Alternative text\",\n  \"exitFullscreenIcon\": null\n}",
                "<div class=\"flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_no-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner\">\n<button class=\"flexi-picture__exit-fullscreen-button\" title=\"Exit fullscreen\" aria-label=\"Exit fullscreen\">\n</button>\n<div class=\"flexi-picture__container\">\n<div class=\"flexi-picture__error-notice\">\n<svg class=\"flexi-picture__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-picture__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n<div class=\"flexi-picture__picture-container\">\n<picture class=\"flexi-picture__picture\">\n<img class=\"flexi-picture__image\" data-src=\"/file.png\" alt=\"Alternative text\" tabindex=\"-1\">\n</picture>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiPictureBlocks")]
        [InlineData("All")]
        public void FlexiPictureBlocks_Spec13(string extensions)
        {
            //     Start line number: 515
            //     --------------- Markdown ---------------
            //     p{ 
            //       "src": "/file.png",
            //       "alt": "Alternative text",
            //       "errorIcon": "<svg><use xlink:href=\"#error-icon\"/></svg>"
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner">
            //     <button class="flexi-picture__exit-fullscreen-button" title="Exit fullscreen" aria-label="Exit fullscreen">
            //     <svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     </button>
            //     <div class="flexi-picture__container">
            //     <div class="flexi-picture__error-notice">
            //     <svg class="flexi-picture__error-icon"><use xlink:href="#error-icon"/></svg>
            //     </div>
            //     <div class="flexi-picture__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     <div class="flexi-picture__picture-container">
            //     <picture class="flexi-picture__picture">
            //     <img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
            //     </picture>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("p{ \n  \"src\": \"/file.png\",\n  \"alt\": \"Alternative text\",\n  \"errorIcon\": \"<svg><use xlink:href=\\\"#error-icon\\\"/></svg>\"\n}",
                "<div class=\"flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner\">\n<button class=\"flexi-picture__exit-fullscreen-button\" title=\"Exit fullscreen\" aria-label=\"Exit fullscreen\">\n<svg class=\"flexi-picture__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n</button>\n<div class=\"flexi-picture__container\">\n<div class=\"flexi-picture__error-notice\">\n<svg class=\"flexi-picture__error-icon\"><use xlink:href=\"#error-icon\"/></svg>\n</div>\n<div class=\"flexi-picture__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n<div class=\"flexi-picture__picture-container\">\n<picture class=\"flexi-picture__picture\">\n<img class=\"flexi-picture__image\" data-src=\"/file.png\" alt=\"Alternative text\" tabindex=\"-1\">\n</picture>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiPictureBlocks")]
        [InlineData("All")]
        public void FlexiPictureBlocks_Spec14(string extensions)
        {
            //     Start line number: 547
            //     --------------- Markdown ---------------
            //     p{ 
            //       "src": "/file.png",
            //       "alt": "Alternative text",
            //       "errorIcon": null
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_no-error-icon flexi-picture_has-spinner">
            //     <button class="flexi-picture__exit-fullscreen-button" title="Exit fullscreen" aria-label="Exit fullscreen">
            //     <svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     </button>
            //     <div class="flexi-picture__container">
            //     <div class="flexi-picture__error-notice">
            //     </div>
            //     <div class="flexi-picture__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     <div class="flexi-picture__picture-container">
            //     <picture class="flexi-picture__picture">
            //     <img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
            //     </picture>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("p{ \n  \"src\": \"/file.png\",\n  \"alt\": \"Alternative text\",\n  \"errorIcon\": null\n}",
                "<div class=\"flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_no-error-icon flexi-picture_has-spinner\">\n<button class=\"flexi-picture__exit-fullscreen-button\" title=\"Exit fullscreen\" aria-label=\"Exit fullscreen\">\n<svg class=\"flexi-picture__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n</button>\n<div class=\"flexi-picture__container\">\n<div class=\"flexi-picture__error-notice\">\n</div>\n<div class=\"flexi-picture__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n<div class=\"flexi-picture__picture-container\">\n<picture class=\"flexi-picture__picture\">\n<img class=\"flexi-picture__image\" data-src=\"/file.png\" alt=\"Alternative text\" tabindex=\"-1\">\n</picture>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiPictureBlocks")]
        [InlineData("All")]
        public void FlexiPictureBlocks_Spec15(string extensions)
        {
            //     Start line number: 585
            //     --------------- Markdown ---------------
            //     p{ 
            //       "src": "/file.png",
            //       "alt": "Alternative text",
            //       "spinner": "<div class=\"spinner\"></div>"
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner">
            //     <button class="flexi-picture__exit-fullscreen-button" title="Exit fullscreen" aria-label="Exit fullscreen">
            //     <svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     </button>
            //     <div class="flexi-picture__container">
            //     <div class="flexi-picture__error-notice">
            //     <svg class="flexi-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-picture__spinner spinner"></div>
            //     <div class="flexi-picture__picture-container">
            //     <picture class="flexi-picture__picture">
            //     <img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
            //     </picture>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("p{ \n  \"src\": \"/file.png\",\n  \"alt\": \"Alternative text\",\n  \"spinner\": \"<div class=\\\"spinner\\\"></div>\"\n}",
                "<div class=\"flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner\">\n<button class=\"flexi-picture__exit-fullscreen-button\" title=\"Exit fullscreen\" aria-label=\"Exit fullscreen\">\n<svg class=\"flexi-picture__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n</button>\n<div class=\"flexi-picture__container\">\n<div class=\"flexi-picture__error-notice\">\n<svg class=\"flexi-picture__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-picture__spinner spinner\"></div>\n<div class=\"flexi-picture__picture-container\">\n<picture class=\"flexi-picture__picture\">\n<img class=\"flexi-picture__image\" data-src=\"/file.png\" alt=\"Alternative text\" tabindex=\"-1\">\n</picture>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiPictureBlocks")]
        [InlineData("All")]
        public void FlexiPictureBlocks_Spec16(string extensions)
        {
            //     Start line number: 611
            //     --------------- Markdown ---------------
            //     p{ 
            //       "src": "/file.png",
            //       "alt": "Alternative text",
            //       "spinner": null
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_no-spinner">
            //     <button class="flexi-picture__exit-fullscreen-button" title="Exit fullscreen" aria-label="Exit fullscreen">
            //     <svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     </button>
            //     <div class="flexi-picture__container">
            //     <div class="flexi-picture__error-notice">
            //     <svg class="flexi-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-picture__picture-container">
            //     <picture class="flexi-picture__picture">
            //     <img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
            //     </picture>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("p{ \n  \"src\": \"/file.png\",\n  \"alt\": \"Alternative text\",\n  \"spinner\": null\n}",
                "<div class=\"flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_no-spinner\">\n<button class=\"flexi-picture__exit-fullscreen-button\" title=\"Exit fullscreen\" aria-label=\"Exit fullscreen\">\n<svg class=\"flexi-picture__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n</button>\n<div class=\"flexi-picture__container\">\n<div class=\"flexi-picture__error-notice\">\n<svg class=\"flexi-picture__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-picture__picture-container\">\n<picture class=\"flexi-picture__picture\">\n<img class=\"flexi-picture__image\" data-src=\"/file.png\" alt=\"Alternative text\" tabindex=\"-1\">\n</picture>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiPictureBlocks")]
        [InlineData("All")]
        public void FlexiPictureBlocks_Spec17(string extensions)
        {
            //     Start line number: 654
            //     --------------- Markdown ---------------
            //     p{ 
            //       "src": "/file.png",
            //       "alt": "Alternative text",
            //       "attributes": {
            //           "id" : "my-custom-id",
            //           "class" : "my-custom-class"
            //       }
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner my-custom-class" id="my-custom-id">
            //     <button class="flexi-picture__exit-fullscreen-button" title="Exit fullscreen" aria-label="Exit fullscreen">
            //     <svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     </button>
            //     <div class="flexi-picture__container">
            //     <div class="flexi-picture__error-notice">
            //     <svg class="flexi-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-picture__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     <div class="flexi-picture__picture-container">
            //     <picture class="flexi-picture__picture">
            //     <img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
            //     </picture>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("p{ \n  \"src\": \"/file.png\",\n  \"alt\": \"Alternative text\",\n  \"attributes\": {\n      \"id\" : \"my-custom-id\",\n      \"class\" : \"my-custom-class\"\n  }\n}",
                "<div class=\"flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner my-custom-class\" id=\"my-custom-id\">\n<button class=\"flexi-picture__exit-fullscreen-button\" title=\"Exit fullscreen\" aria-label=\"Exit fullscreen\">\n<svg class=\"flexi-picture__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n</button>\n<div class=\"flexi-picture__container\">\n<div class=\"flexi-picture__error-notice\">\n<svg class=\"flexi-picture__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-picture__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n<div class=\"flexi-picture__picture-container\">\n<picture class=\"flexi-picture__picture\">\n<img class=\"flexi-picture__image\" data-src=\"/file.png\" alt=\"Alternative text\" tabindex=\"-1\">\n</picture>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiPictureBlocks")]
        [InlineData("All")]
        public void FlexiPictureBlocks_Spec18(string extensions)
        {
            //     Start line number: 704
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiPictureBlocks": {
            //             "defaultBlockOptions": {
            //                 "errorIcon": "<svg><use xlink:href=\"#error-icon\"/></svg>",
            //                 "attributes": {
            //                     "class": "block"
            //                 }
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     p{ 
            //       "src": "/file.png",
            //       "alt": "Alternative text"
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner block">
            //     <button class="flexi-picture__exit-fullscreen-button" title="Exit fullscreen" aria-label="Exit fullscreen">
            //     <svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     </button>
            //     <div class="flexi-picture__container">
            //     <div class="flexi-picture__error-notice">
            //     <svg class="flexi-picture__error-icon"><use xlink:href="#error-icon"/></svg>
            //     </div>
            //     <div class="flexi-picture__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     <div class="flexi-picture__picture-container">
            //     <picture class="flexi-picture__picture">
            //     <img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
            //     </picture>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("p{ \n  \"src\": \"/file.png\",\n  \"alt\": \"Alternative text\"\n}",
                "<div class=\"flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner block\">\n<button class=\"flexi-picture__exit-fullscreen-button\" title=\"Exit fullscreen\" aria-label=\"Exit fullscreen\">\n<svg class=\"flexi-picture__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n</button>\n<div class=\"flexi-picture__container\">\n<div class=\"flexi-picture__error-notice\">\n<svg class=\"flexi-picture__error-icon\"><use xlink:href=\"#error-icon\"/></svg>\n</div>\n<div class=\"flexi-picture__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n<div class=\"flexi-picture__picture-container\">\n<picture class=\"flexi-picture__picture\">\n<img class=\"flexi-picture__image\" data-src=\"/file.png\" alt=\"Alternative text\" tabindex=\"-1\">\n</picture>\n</div>\n</div>\n</div>",
                extensions,
                false,
                "{\n    \"flexiPictureBlocks\": {\n        \"defaultBlockOptions\": {\n            \"errorIcon\": \"<svg><use xlink:href=\\\"#error-icon\\\"/></svg>\",\n            \"attributes\": {\n                \"class\": \"block\"\n            }\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiPictureBlocks")]
        [InlineData("All")]
        public void FlexiPictureBlocks_Spec19(string extensions)
        {
            //     Start line number: 746
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiPictureBlocks": {
            //             "defaultBlockOptions": {
            //                 "blockName": "picture"
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     p{ 
            //       "src": "/file.png",
            //       "alt": "Alternative text"
            //     }
            //     
            //     p{ 
            //       "blockname": "special-picture",
            //       "src": "/file.png",
            //       "alt": "Alternative text"
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="picture picture_has-alt picture_is-lazy picture_no-width picture_no-aspect-ratio picture_has-exit-fullscreen-icon picture_has-error-icon picture_has-spinner">
            //     <button class="picture__exit-fullscreen-button" title="Exit fullscreen" aria-label="Exit fullscreen">
            //     <svg class="picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     </button>
            //     <div class="picture__container">
            //     <div class="picture__error-notice">
            //     <svg class="picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="picture__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     <div class="picture__picture-container">
            //     <picture class="picture__picture">
            //     <img class="picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
            //     </picture>
            //     </div>
            //     </div>
            //     </div>
            //     <div class="special-picture special-picture_has-alt special-picture_is-lazy special-picture_no-width special-picture_no-aspect-ratio special-picture_has-exit-fullscreen-icon special-picture_has-error-icon special-picture_has-spinner">
            //     <button class="special-picture__exit-fullscreen-button" title="Exit fullscreen" aria-label="Exit fullscreen">
            //     <svg class="special-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     </button>
            //     <div class="special-picture__container">
            //     <div class="special-picture__error-notice">
            //     <svg class="special-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="special-picture__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     <div class="special-picture__picture-container">
            //     <picture class="special-picture__picture">
            //     <img class="special-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
            //     </picture>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("p{ \n  \"src\": \"/file.png\",\n  \"alt\": \"Alternative text\"\n}\n\np{ \n  \"blockname\": \"special-picture\",\n  \"src\": \"/file.png\",\n  \"alt\": \"Alternative text\"\n}",
                "<div class=\"picture picture_has-alt picture_is-lazy picture_no-width picture_no-aspect-ratio picture_has-exit-fullscreen-icon picture_has-error-icon picture_has-spinner\">\n<button class=\"picture__exit-fullscreen-button\" title=\"Exit fullscreen\" aria-label=\"Exit fullscreen\">\n<svg class=\"picture__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n</button>\n<div class=\"picture__container\">\n<div class=\"picture__error-notice\">\n<svg class=\"picture__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"picture__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n<div class=\"picture__picture-container\">\n<picture class=\"picture__picture\">\n<img class=\"picture__image\" data-src=\"/file.png\" alt=\"Alternative text\" tabindex=\"-1\">\n</picture>\n</div>\n</div>\n</div>\n<div class=\"special-picture special-picture_has-alt special-picture_is-lazy special-picture_no-width special-picture_no-aspect-ratio special-picture_has-exit-fullscreen-icon special-picture_has-error-icon special-picture_has-spinner\">\n<button class=\"special-picture__exit-fullscreen-button\" title=\"Exit fullscreen\" aria-label=\"Exit fullscreen\">\n<svg class=\"special-picture__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n</button>\n<div class=\"special-picture__container\">\n<div class=\"special-picture__error-notice\">\n<svg class=\"special-picture__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"special-picture__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n<div class=\"special-picture__picture-container\">\n<picture class=\"special-picture__picture\">\n<img class=\"special-picture__image\" data-src=\"/file.png\" alt=\"Alternative text\" tabindex=\"-1\">\n</picture>\n</div>\n</div>\n</div>",
                extensions,
                false,
                "{\n    \"flexiPictureBlocks\": {\n        \"defaultBlockOptions\": {\n            \"blockName\": \"picture\"\n        }\n    }\n}");
        }
    }

    public class FlexiQuoteBlocksSpecs
    {
        [Theory]
        [InlineData("FlexiQuoteBlocks")]
        [InlineData("All")]
        public void FlexiQuoteBlocks_Spec1(string extensions)
        {
            //     Start line number: 44
            //     --------------- Markdown ---------------
            //     +++ quote
            //     This is a quote!
            //     +++
            //     Author, in Work
            //     +++
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-quote flexi-quote_has-icon">
            //     <svg class="flexi-quote__icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 14 10"><path d="M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z"/></svg>
            //     <div class="flexi-quote__content">
            //     <blockquote class="flexi-quote__blockquote">
            //     <p>This is a quote!</p>
            //     </blockquote>
            //     <p class="flexi-quote__citation">— Author, in Work</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("+++ quote\nThis is a quote!\n+++\nAuthor, in Work\n+++",
                "<div class=\"flexi-quote flexi-quote_has-icon\">\n<svg class=\"flexi-quote__icon\" xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 14 10\"><path d=\"M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z\"/></svg>\n<div class=\"flexi-quote__content\">\n<blockquote class=\"flexi-quote__blockquote\">\n<p>This is a quote!</p>\n</blockquote>\n<p class=\"flexi-quote__citation\">— Author, in Work</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiQuoteBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiQuoteBlocks_Spec2(string extensions)
        {
            //     Start line number: 68
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     +++ quote
            //     ```
            //     Code you'd like to quote
            //     ```
            //     +++
            //     *Author*, in **Work**
            //     +++
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-quote flexi-quote_has-icon">
            //     <svg class="flexi-quote__icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 14 10"><path d="M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z"/></svg>
            //     <div class="flexi-quote__content">
            //     <blockquote class="flexi-quote__blockquote">
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">Code you'd like to quote
            //     </code></pre>
            //     </div>
            //     </blockquote>
            //     <p class="flexi-quote__citation">— <em>Author</em>, in <strong>Work</strong></p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("+++ quote\n```\nCode you'd like to quote\n```\n+++\n*Author*, in **Work**\n+++",
                "<div class=\"flexi-quote flexi-quote_has-icon\">\n<svg class=\"flexi-quote__icon\" xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 14 10\"><path d=\"M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z\"/></svg>\n<div class=\"flexi-quote__content\">\n<blockquote class=\"flexi-quote__blockquote\">\n<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">Code you'd like to quote\n</code></pre>\n</div>\n</blockquote>\n<p class=\"flexi-quote__citation\">— <em>Author</em>, in <strong>Work</strong></p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiQuoteBlocks")]
        [InlineData("All")]
        public void FlexiQuoteBlocks_Spec3(string extensions)
        {
            //     Start line number: 103
            //     --------------- Markdown ---------------
            //     +++ quote
            //     This is a quote!
            //     +++
            //     Author, in ""Work""
            //     +++
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-quote flexi-quote_has-icon">
            //     <svg class="flexi-quote__icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 14 10"><path d="M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z"/></svg>
            //     <div class="flexi-quote__content">
            //     <blockquote class="flexi-quote__blockquote">
            //     <p>This is a quote!</p>
            //     </blockquote>
            //     <p class="flexi-quote__citation">— Author, in <cite>Work</cite></p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("+++ quote\nThis is a quote!\n+++\nAuthor, in \"\"Work\"\"\n+++",
                "<div class=\"flexi-quote flexi-quote_has-icon\">\n<svg class=\"flexi-quote__icon\" xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 14 10\"><path d=\"M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z\"/></svg>\n<div class=\"flexi-quote__content\">\n<blockquote class=\"flexi-quote__blockquote\">\n<p>This is a quote!</p>\n</blockquote>\n<p class=\"flexi-quote__citation\">— Author, in <cite>Work</cite></p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiQuoteBlocks")]
        [InlineData("All")]
        public void FlexiQuoteBlocks_Spec4(string extensions)
        {
            //     Start line number: 123
            //     --------------- Markdown ---------------
            //     +++ quote
            //     This is a quote!
            //     +++
            //     [Author](author-url.com), in ""[Work](work-url.com)""
            //     +++
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-quote flexi-quote_has-icon">
            //     <svg class="flexi-quote__icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 14 10"><path d="M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z"/></svg>
            //     <div class="flexi-quote__content">
            //     <blockquote class="flexi-quote__blockquote" cite="work-url.com">
            //     <p>This is a quote!</p>
            //     </blockquote>
            //     <p class="flexi-quote__citation">— <a href="author-url.com">Author</a>, in <cite><a href="work-url.com">Work</a></cite></p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("+++ quote\nThis is a quote!\n+++\n[Author](author-url.com), in \"\"[Work](work-url.com)\"\"\n+++",
                "<div class=\"flexi-quote flexi-quote_has-icon\">\n<svg class=\"flexi-quote__icon\" xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 14 10\"><path d=\"M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z\"/></svg>\n<div class=\"flexi-quote__content\">\n<blockquote class=\"flexi-quote__blockquote\" cite=\"work-url.com\">\n<p>This is a quote!</p>\n</blockquote>\n<p class=\"flexi-quote__citation\">— <a href=\"author-url.com\">Author</a>, in <cite><a href=\"work-url.com\">Work</a></cite></p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiQuoteBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiQuoteBlocks_Spec5(string extensions)
        {
            //     Start line number: 158
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "blockName": "quote" }
            //     +++ quote
            //     This is a quote!
            //     +++
            //     Author, in Work
            //     +++
            //     --------------- Expected Markup ---------------
            //     <div class="quote quote_has-icon">
            //     <svg class="quote__icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 14 10"><path d="M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z"/></svg>
            //     <div class="quote__content">
            //     <blockquote class="quote__blockquote">
            //     <p>This is a quote!</p>
            //     </blockquote>
            //     <p class="quote__citation">— Author, in Work</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"blockName\": \"quote\" }\n+++ quote\nThis is a quote!\n+++\nAuthor, in Work\n+++",
                "<div class=\"quote quote_has-icon\">\n<svg class=\"quote__icon\" xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 14 10\"><path d=\"M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z\"/></svg>\n<div class=\"quote__content\">\n<blockquote class=\"quote__blockquote\">\n<p>This is a quote!</p>\n</blockquote>\n<p class=\"quote__citation\">— Author, in Work</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiQuoteBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiQuoteBlocks_Spec6(string extensions)
        {
            //     Start line number: 187
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "icon": "<svg><use xlink:href=\"#alert-icon\"/></svg>" }
            //     +++ quote
            //     This is a quote!
            //     +++
            //     Author, in Work
            //     +++
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-quote flexi-quote_has-icon">
            //     <svg class="flexi-quote__icon"><use xlink:href="#alert-icon"/></svg>
            //     <div class="flexi-quote__content">
            //     <blockquote class="flexi-quote__blockquote">
            //     <p>This is a quote!</p>
            //     </blockquote>
            //     <p class="flexi-quote__citation">— Author, in Work</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"icon\": \"<svg><use xlink:href=\\\"#alert-icon\\\"/></svg>\" }\n+++ quote\nThis is a quote!\n+++\nAuthor, in Work\n+++",
                "<div class=\"flexi-quote flexi-quote_has-icon\">\n<svg class=\"flexi-quote__icon\"><use xlink:href=\"#alert-icon\"/></svg>\n<div class=\"flexi-quote__content\">\n<blockquote class=\"flexi-quote__blockquote\">\n<p>This is a quote!</p>\n</blockquote>\n<p class=\"flexi-quote__citation\">— Author, in Work</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiQuoteBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiQuoteBlocks_Spec7(string extensions)
        {
            //     Start line number: 209
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "icon": null }
            //     +++ quote
            //     This is a quote!
            //     +++
            //     Author, in Work
            //     +++
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-quote flexi-quote_no-icon">
            //     <div class="flexi-quote__content">
            //     <blockquote class="flexi-quote__blockquote">
            //     <p>This is a quote!</p>
            //     </blockquote>
            //     <p class="flexi-quote__citation">— Author, in Work</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"icon\": null }\n+++ quote\nThis is a quote!\n+++\nAuthor, in Work\n+++",
                "<div class=\"flexi-quote flexi-quote_no-icon\">\n<div class=\"flexi-quote__content\">\n<blockquote class=\"flexi-quote__blockquote\">\n<p>This is a quote!</p>\n</blockquote>\n<p class=\"flexi-quote__citation\">— Author, in Work</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiQuoteBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiQuoteBlocks_Spec8(string extensions)
        {
            //     Start line number: 239
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{"citeLink": 1}
            //     +++ quote
            //     This is a quote!
            //     +++
            //     [Author](author-url.com), in ""[Work](work-url.com)"" from ""[Guide](guide-url.com)""
            //     +++
            //     
            //     o{"citeLink": -2}
            //     +++ quote
            //     This is a quote!
            //     +++
            //     [Author](author-url.com), in ""[Work](work-url.com)"" from ""[Guide](guide-url.com)""
            //     +++
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-quote flexi-quote_has-icon">
            //     <svg class="flexi-quote__icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 14 10"><path d="M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z"/></svg>
            //     <div class="flexi-quote__content">
            //     <blockquote class="flexi-quote__blockquote" cite="work-url.com">
            //     <p>This is a quote!</p>
            //     </blockquote>
            //     <p class="flexi-quote__citation">— <a href="author-url.com">Author</a>, in <cite><a href="work-url.com">Work</a></cite> from <cite><a href="guide-url.com">Guide</a></cite></p>
            //     </div>
            //     </div>
            //     <div class="flexi-quote flexi-quote_has-icon">
            //     <svg class="flexi-quote__icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 14 10"><path d="M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z"/></svg>
            //     <div class="flexi-quote__content">
            //     <blockquote class="flexi-quote__blockquote" cite="work-url.com">
            //     <p>This is a quote!</p>
            //     </blockquote>
            //     <p class="flexi-quote__citation">— <a href="author-url.com">Author</a>, in <cite><a href="work-url.com">Work</a></cite> from <cite><a href="guide-url.com">Guide</a></cite></p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("o{\"citeLink\": 1}\n+++ quote\nThis is a quote!\n+++\n[Author](author-url.com), in \"\"[Work](work-url.com)\"\" from \"\"[Guide](guide-url.com)\"\"\n+++\n\no{\"citeLink\": -2}\n+++ quote\nThis is a quote!\n+++\n[Author](author-url.com), in \"\"[Work](work-url.com)\"\" from \"\"[Guide](guide-url.com)\"\"\n+++",
                "<div class=\"flexi-quote flexi-quote_has-icon\">\n<svg class=\"flexi-quote__icon\" xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 14 10\"><path d=\"M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z\"/></svg>\n<div class=\"flexi-quote__content\">\n<blockquote class=\"flexi-quote__blockquote\" cite=\"work-url.com\">\n<p>This is a quote!</p>\n</blockquote>\n<p class=\"flexi-quote__citation\">— <a href=\"author-url.com\">Author</a>, in <cite><a href=\"work-url.com\">Work</a></cite> from <cite><a href=\"guide-url.com\">Guide</a></cite></p>\n</div>\n</div>\n<div class=\"flexi-quote flexi-quote_has-icon\">\n<svg class=\"flexi-quote__icon\" xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 14 10\"><path d=\"M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z\"/></svg>\n<div class=\"flexi-quote__content\">\n<blockquote class=\"flexi-quote__blockquote\" cite=\"work-url.com\">\n<p>This is a quote!</p>\n</blockquote>\n<p class=\"flexi-quote__citation\">— <a href=\"author-url.com\">Author</a>, in <cite><a href=\"work-url.com\">Work</a></cite> from <cite><a href=\"guide-url.com\">Guide</a></cite></p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiQuoteBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiQuoteBlocks_Spec9(string extensions)
        {
            //     Start line number: 285
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{
            //         "attributes": {
            //             "id" : "my-custom-id",
            //             "class" : "my-custom-class"
            //         }
            //     }
            //     +++ quote
            //     This is a quote!
            //     +++
            //     Author, in Work
            //     +++
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-quote flexi-quote_has-icon my-custom-class" id="my-custom-id">
            //     <svg class="flexi-quote__icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 14 10"><path d="M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z"/></svg>
            //     <div class="flexi-quote__content">
            //     <blockquote class="flexi-quote__blockquote">
            //     <p>This is a quote!</p>
            //     </blockquote>
            //     <p class="flexi-quote__citation">— Author, in Work</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("o{\n    \"attributes\": {\n        \"id\" : \"my-custom-id\",\n        \"class\" : \"my-custom-class\"\n    }\n}\n+++ quote\nThis is a quote!\n+++\nAuthor, in Work\n+++",
                "<div class=\"flexi-quote flexi-quote_has-icon my-custom-class\" id=\"my-custom-id\">\n<svg class=\"flexi-quote__icon\" xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 14 10\"><path d=\"M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z\"/></svg>\n<div class=\"flexi-quote__content\">\n<blockquote class=\"flexi-quote__blockquote\">\n<p>This is a quote!</p>\n</blockquote>\n<p class=\"flexi-quote__citation\">— Author, in Work</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiQuoteBlocks")]
        [InlineData("All")]
        public void FlexiQuoteBlocks_Spec10(string extensions)
        {
            //     Start line number: 327
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiQuoteBlocks": {
            //             "defaultBlockOptions": {
            //                 "icon": "<svg><use xlink:href=\"#quote-icon\"/></svg>",
            //                 "citeLink": 0,
            //                 "attributes": {
            //                     "class": "block"
            //                 }
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     +++ quote
            //     This is a quote!
            //     +++
            //     ""[Work](work-url.com)"" by [Author](author-url.com)
            //     +++
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-quote flexi-quote_has-icon block">
            //     <svg class="flexi-quote__icon"><use xlink:href="#quote-icon"/></svg>
            //     <div class="flexi-quote__content">
            //     <blockquote class="flexi-quote__blockquote" cite="work-url.com">
            //     <p>This is a quote!</p>
            //     </blockquote>
            //     <p class="flexi-quote__citation">— <cite><a href="work-url.com">Work</a></cite> by <a href="author-url.com">Author</a></p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("+++ quote\nThis is a quote!\n+++\n\"\"[Work](work-url.com)\"\" by [Author](author-url.com)\n+++",
                "<div class=\"flexi-quote flexi-quote_has-icon block\">\n<svg class=\"flexi-quote__icon\"><use xlink:href=\"#quote-icon\"/></svg>\n<div class=\"flexi-quote__content\">\n<blockquote class=\"flexi-quote__blockquote\" cite=\"work-url.com\">\n<p>This is a quote!</p>\n</blockquote>\n<p class=\"flexi-quote__citation\">— <cite><a href=\"work-url.com\">Work</a></cite> by <a href=\"author-url.com\">Author</a></p>\n</div>\n</div>",
                extensions,
                false,
                "{\n    \"flexiQuoteBlocks\": {\n        \"defaultBlockOptions\": {\n            \"icon\": \"<svg><use xlink:href=\\\"#quote-icon\\\"/></svg>\",\n            \"citeLink\": 0,\n            \"attributes\": {\n                \"class\": \"block\"\n            }\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiQuoteBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiQuoteBlocks_Spec11(string extensions)
        {
            //     Start line number: 358
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiQuoteBlocks": {
            //             "defaultBlockOptions": {
            //                 "blockName": "quote"
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     +++ quote
            //     This is a quote!
            //     +++
            //     Author, in Work
            //     +++
            //     
            //     o{ "blockName": "special-quote" }
            //     +++ quote
            //     This is a quote!
            //     +++
            //     Author, in Work
            //     +++
            //     --------------- Expected Markup ---------------
            //     <div class="quote quote_has-icon">
            //     <svg class="quote__icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 14 10"><path d="M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z"/></svg>
            //     <div class="quote__content">
            //     <blockquote class="quote__blockquote">
            //     <p>This is a quote!</p>
            //     </blockquote>
            //     <p class="quote__citation">— Author, in Work</p>
            //     </div>
            //     </div>
            //     <div class="special-quote special-quote_has-icon">
            //     <svg class="special-quote__icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 14 10"><path d="M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z"/></svg>
            //     <div class="special-quote__content">
            //     <blockquote class="special-quote__blockquote">
            //     <p>This is a quote!</p>
            //     </blockquote>
            //     <p class="special-quote__citation">— Author, in Work</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("+++ quote\nThis is a quote!\n+++\nAuthor, in Work\n+++\n\no{ \"blockName\": \"special-quote\" }\n+++ quote\nThis is a quote!\n+++\nAuthor, in Work\n+++",
                "<div class=\"quote quote_has-icon\">\n<svg class=\"quote__icon\" xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 14 10\"><path d=\"M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z\"/></svg>\n<div class=\"quote__content\">\n<blockquote class=\"quote__blockquote\">\n<p>This is a quote!</p>\n</blockquote>\n<p class=\"quote__citation\">— Author, in Work</p>\n</div>\n</div>\n<div class=\"special-quote special-quote_has-icon\">\n<svg class=\"special-quote__icon\" xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 14 10\"><path d=\"M13,0h-3L8,4v6h6V4h-3L13,0z M5,0H2L0,4v6h6V4H3L5,0z\"/></svg>\n<div class=\"special-quote__content\">\n<blockquote class=\"special-quote__blockquote\">\n<p>This is a quote!</p>\n</blockquote>\n<p class=\"special-quote__citation\">— Author, in Work</p>\n</div>\n</div>",
                extensions,
                false,
                "{\n    \"flexiQuoteBlocks\": {\n        \"defaultBlockOptions\": {\n            \"blockName\": \"quote\"\n        }\n    }\n}");
        }
    }

    public class FlexiSectionBlocksSpecs
    {
        [Theory]
        [InlineData("FlexiSectionBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec1(string extensions)
        {
            //     Start line number: 92
            //     --------------- Markdown ---------------
            //     # Indoor Herb Gardens
            //     An introduction..
            //     
            //     ## Getting Started
            //     
            //     ### Growing Herbs from Cuttings
            //     Information on growing herbs from cuttings..
            //     
            //     ## Caring for Herbs
            //     
            //     ### Watering Herbs
            //     Information on watering herbs..
            //     --------------- Expected Markup ---------------
            //     <section class="flexi-section flexi-section_level_1 flexi-section_has-link-icon" id="indoor-herb-gardens">
            //     <header class="flexi-section__header">
            //     <h1 class="flexi-section__heading">Indoor Herb Gardens</h1>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <p>An introduction..</p>
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="getting-started">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">Getting Started</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <section class="flexi-section flexi-section_level_3 flexi-section_has-link-icon" id="growing-herbs-from-cuttings">
            //     <header class="flexi-section__header">
            //     <h3 class="flexi-section__heading">Growing Herbs from Cuttings</h3>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <p>Information on growing herbs from cuttings..</p>
            //     </section>
            //     </section>
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="caring-for-herbs">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">Caring for Herbs</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <section class="flexi-section flexi-section_level_3 flexi-section_has-link-icon" id="watering-herbs">
            //     <header class="flexi-section__header">
            //     <h3 class="flexi-section__heading">Watering Herbs</h3>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <p>Information on watering herbs..</p>
            //     </section>
            //     </section>
            //     </section>

            SpecTestHelper.AssertCompliance("# Indoor Herb Gardens\nAn introduction..\n\n## Getting Started\n\n### Growing Herbs from Cuttings\nInformation on growing herbs from cuttings..\n\n## Caring for Herbs\n\n### Watering Herbs\nInformation on watering herbs..",
                "<section class=\"flexi-section flexi-section_level_1 flexi-section_has-link-icon\" id=\"indoor-herb-gardens\">\n<header class=\"flexi-section__header\">\n<h1 class=\"flexi-section__heading\">Indoor Herb Gardens</h1>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<p>An introduction..</p>\n<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"getting-started\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">Getting Started</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<section class=\"flexi-section flexi-section_level_3 flexi-section_has-link-icon\" id=\"growing-herbs-from-cuttings\">\n<header class=\"flexi-section__header\">\n<h3 class=\"flexi-section__heading\">Growing Herbs from Cuttings</h3>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<p>Information on growing herbs from cuttings..</p>\n</section>\n</section>\n<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"caring-for-herbs\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">Caring for Herbs</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<section class=\"flexi-section flexi-section_level_3 flexi-section_has-link-icon\" id=\"watering-herbs\">\n<header class=\"flexi-section__header\">\n<h3 class=\"flexi-section__heading\">Watering Herbs</h3>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<p>Information on watering herbs..</p>\n</section>\n</section>\n</section>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec2(string extensions)
        {
            //     Start line number: 173
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "blockName": "section" }
            //     ## foo
            //     --------------- Expected Markup ---------------
            //     <section class="section section_level_2 section_has-link-icon" id="foo">
            //     <header class="section__header">
            //     <h2 class="section__heading">foo</h2>
            //     <button class="section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     </section>

            SpecTestHelper.AssertCompliance("o{ \"blockName\": \"section\" }\n## foo",
                "<section class=\"section section_level_2 section_has-link-icon\" id=\"foo\">\n<header class=\"section__header\">\n<h2 class=\"section__heading\">foo</h2>\n<button class=\"section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n</section>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec3(string extensions)
        {
            //     Start line number: 196
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "element": "nav" }
            //     ## foo
            //     --------------- Expected Markup ---------------
            //     <nav class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="foo">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">foo</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     </nav>

            SpecTestHelper.AssertCompliance("o{ \"element\": \"nav\" }\n## foo",
                "<nav class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"foo\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">foo</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n</nav>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec4(string extensions)
        {
            //     Start line number: 225
            //     --------------- Markdown ---------------
            //     ## Foo Bar Baz
            //     --------------- Expected Markup ---------------
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="foo-bar-baz">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">Foo Bar Baz</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     </section>

            SpecTestHelper.AssertCompliance("## Foo Bar Baz",
                "<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"foo-bar-baz\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">Foo Bar Baz</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n</section>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec5(string extensions)
        {
            //     Start line number: 239
            //     --------------- Markdown ---------------
            //     ## foo
            //     ### `foo`
            //     ## foo 1
            //     --------------- Expected Markup ---------------
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="foo">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">foo</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <section class="flexi-section flexi-section_level_3 flexi-section_has-link-icon" id="foo-1">
            //     <header class="flexi-section__header">
            //     <h3 class="flexi-section__heading"><code>foo</code></h3>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     </section>
            //     </section>
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="foo-1-1">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">foo 1</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     </section>

            SpecTestHelper.AssertCompliance("## foo\n### `foo`\n## foo 1",
                "<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"foo\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">foo</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<section class=\"flexi-section flexi-section_level_3 flexi-section_has-link-icon\" id=\"foo-1\">\n<header class=\"flexi-section__header\">\n<h3 class=\"flexi-section__heading\"><code>foo</code></h3>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n</section>\n</section>\n<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"foo-1-1\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">foo 1</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n</section>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec6(string extensions)
        {
            //     Start line number: 271
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "generateID": false }
            //     ## Foo Bar Baz
            //     --------------- Expected Markup ---------------
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">Foo Bar Baz</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     </section>

            SpecTestHelper.AssertCompliance("o{ \"generateID\": false }\n## Foo Bar Baz",
                "<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">Foo Bar Baz</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n</section>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec7(string extensions)
        {
            //     Start line number: 288
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ 
            //         "attributes": {
            //           "id" : "my-custom-id"
            //         }
            //     }
            //     ## Foo Bar Baz
            //     --------------- Expected Markup ---------------
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="foo-bar-baz">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">Foo Bar Baz</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     </section>

            SpecTestHelper.AssertCompliance("o{ \n    \"attributes\": {\n      \"id\" : \"my-custom-id\"\n    }\n}\n## Foo Bar Baz",
                "<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"foo-bar-baz\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">Foo Bar Baz</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n</section>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec8(string extensions)
        {
            //     Start line number: 316
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{
            //         "linkIcon": "<svg><use xlink:href=\"#material-design-link\"/></svg>"
            //     }
            //     ## foo
            //     --------------- Expected Markup ---------------
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="foo">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">foo</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon"><use xlink:href="#material-design-link"/></svg>
            //     </button>
            //     </header>
            //     </section>

            SpecTestHelper.AssertCompliance("o{\n    \"linkIcon\": \"<svg><use xlink:href=\\\"#material-design-link\\\"/></svg>\"\n}\n## foo",
                "<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"foo\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">foo</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\"><use xlink:href=\"#material-design-link\"/></svg>\n</button>\n</header>\n</section>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec9(string extensions)
        {
            //     Start line number: 335
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "linkIcon": null }
            //     # foo
            //     --------------- Expected Markup ---------------
            //     <section class="flexi-section flexi-section_level_1 flexi-section_no-link-icon" id="foo">
            //     <header class="flexi-section__header">
            //     <h1 class="flexi-section__heading">foo</h1>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     </button>
            //     </header>
            //     </section>

            SpecTestHelper.AssertCompliance("o{ \"linkIcon\": null }\n# foo",
                "<section class=\"flexi-section flexi-section_level_1 flexi-section_no-link-icon\" id=\"foo\">\n<header class=\"flexi-section__header\">\n<h1 class=\"flexi-section__heading\">foo</h1>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n</button>\n</header>\n</section>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec10(string extensions)
        {
            //     Start line number: 364
            //     --------------- Markdown ---------------
            //     [foo]
            //     
            //     ## foo
            //     
            //     [foo]
            //     [Link Text][foo]
            //     --------------- Expected Markup ---------------
            //     <p><a href="#foo">foo</a></p>
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="foo">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">foo</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <p><a href="#foo">foo</a>
            //     <a href="#foo">Link Text</a></p>
            //     </section>

            SpecTestHelper.AssertCompliance("[foo]\n\n## foo\n\n[foo]\n[Link Text][foo]",
                "<p><a href=\"#foo\">foo</a></p>\n<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"foo\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">foo</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<p><a href=\"#foo\">foo</a>\n<a href=\"#foo\">Link Text</a></p>\n</section>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec11(string extensions)
        {
            //     Start line number: 386
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     [foo]
            //     
            //     o{ "referenceLinkable": false }
            //     ## foo
            //     
            //     [foo]
            //     [Link Text][foo]
            //     --------------- Expected Markup ---------------
            //     <p>[foo]</p>
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">foo</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <p>[foo]
            //     [Link Text][foo]</p>
            //     </section>

            SpecTestHelper.AssertCompliance("[foo]\n\no{ \"referenceLinkable\": false }\n## foo\n\n[foo]\n[Link Text][foo]",
                "<p>[foo]</p>\n<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">foo</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<p>[foo]\n[Link Text][foo]</p>\n</section>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec12(string extensions)
        {
            //     Start line number: 411
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     [foo]
            //     
            //     o{ "generateID": false }
            //     ## foo
            //     
            //     [foo]
            //     [Link Text][foo]
            //     --------------- Expected Markup ---------------
            //     <p>[foo]</p>
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">foo</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <p>[foo]
            //     [Link Text][foo]</p>
            //     </section>

            SpecTestHelper.AssertCompliance("[foo]\n\no{ \"generateID\": false }\n## foo\n\n[foo]\n[Link Text][foo]",
                "<p>[foo]</p>\n<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">foo</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<p>[foo]\n[Link Text][foo]</p>\n</section>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec13(string extensions)
        {
            //     Start line number: 437
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     ## Rosemary
            //     ### Watering
            //     Rosemary watering needs..
            //     
            //     ## Lemon Balm
            //     ### Watering
            //     Lemon Balm watering needs..
            //     
            //     ## Peppermint
            //     ### Watering
            //     Similar to [Lemon Balm watering needs][watering 1]...
            //     --------------- Expected Markup ---------------
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="rosemary">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">Rosemary</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <section class="flexi-section flexi-section_level_3 flexi-section_has-link-icon" id="watering">
            //     <header class="flexi-section__header">
            //     <h3 class="flexi-section__heading">Watering</h3>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <p>Rosemary watering needs..</p>
            //     </section>
            //     </section>
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="lemon-balm">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">Lemon Balm</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <section class="flexi-section flexi-section_level_3 flexi-section_has-link-icon" id="watering-1">
            //     <header class="flexi-section__header">
            //     <h3 class="flexi-section__heading">Watering</h3>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <p>Lemon Balm watering needs..</p>
            //     </section>
            //     </section>
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="peppermint">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">Peppermint</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <section class="flexi-section flexi-section_level_3 flexi-section_has-link-icon" id="watering-2">
            //     <header class="flexi-section__header">
            //     <h3 class="flexi-section__heading">Watering</h3>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <p>Similar to <a href="#watering-1">Lemon Balm watering needs</a>...</p>
            //     </section>
            //     </section>

            SpecTestHelper.AssertCompliance("## Rosemary\n### Watering\nRosemary watering needs..\n\n## Lemon Balm\n### Watering\nLemon Balm watering needs..\n\n## Peppermint\n### Watering\nSimilar to [Lemon Balm watering needs][watering 1]...",
                "<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"rosemary\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">Rosemary</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<section class=\"flexi-section flexi-section_level_3 flexi-section_has-link-icon\" id=\"watering\">\n<header class=\"flexi-section__header\">\n<h3 class=\"flexi-section__heading\">Watering</h3>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<p>Rosemary watering needs..</p>\n</section>\n</section>\n<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"lemon-balm\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">Lemon Balm</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<section class=\"flexi-section flexi-section_level_3 flexi-section_has-link-icon\" id=\"watering-1\">\n<header class=\"flexi-section__header\">\n<h3 class=\"flexi-section__heading\">Watering</h3>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<p>Lemon Balm watering needs..</p>\n</section>\n</section>\n<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"peppermint\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">Peppermint</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<section class=\"flexi-section flexi-section_level_3 flexi-section_has-link-icon\" id=\"watering-2\">\n<header class=\"flexi-section__header\">\n<h3 class=\"flexi-section__heading\">Watering</h3>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<p>Similar to <a href=\"#watering-1\">Lemon Balm watering needs</a>...</p>\n</section>\n</section>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec14(string extensions)
        {
            //     Start line number: 512
            //     --------------- Markdown ---------------
            //     ## foo
            //     --------------- Expected Markup ---------------
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="foo">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">foo</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     </section>

            SpecTestHelper.AssertCompliance("## foo",
                "<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"foo\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">foo</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n</section>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec15(string extensions)
        {
            //     Start line number: 527
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "renderingMode": "classic" }
            //     ## foo
            //     --------------- Expected Markup ---------------
            //     <h2>foo</h2>

            SpecTestHelper.AssertCompliance("o{ \"renderingMode\": \"classic\" }\n## foo",
                "<h2>foo</h2>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec16(string extensions)
        {
            //     Start line number: 546
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{
            //         "attributes": {
            //             "id" : "section-1",
            //             "class" : "block"
            //         },
            //         "generateID": false
            //     }
            //     ## foo
            //     --------------- Expected Markup ---------------
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon block" id="section-1">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">foo</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     </section>

            SpecTestHelper.AssertCompliance("o{\n    \"attributes\": {\n        \"id\" : \"section-1\",\n        \"class\" : \"block\"\n    },\n    \"generateID\": false\n}\n## foo",
                "<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon block\" id=\"section-1\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">foo</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n</section>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiSectionBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec17(string extensions)
        {
            //     Start line number: 584
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiSectionBlocks": {
            //             "defaultBlockOptions": {
            //                 "blockName": "section",
            //                 "element": "nav",
            //                 "generateID": false,
            //                 "linkIcon": "<svg><use xlink:href=\"#material-design-link\"/></svg>",
            //                 "attributes": {
            //                     "class": "block"
            //                 }
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     # foo
            //     ## bar
            //     
            //     [foo]
            //     [bar]
            //     --------------- Expected Markup ---------------
            //     <nav class="section section_level_1 section_has-link-icon block">
            //     <header class="section__header">
            //     <h1 class="section__heading">foo</h1>
            //     <button class="section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="section__link-icon"><use xlink:href="#material-design-link"/></svg>
            //     </button>
            //     </header>
            //     <nav class="section section_level_2 section_has-link-icon block">
            //     <header class="section__header">
            //     <h2 class="section__heading">bar</h2>
            //     <button class="section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="section__link-icon"><use xlink:href="#material-design-link"/></svg>
            //     </button>
            //     </header>
            //     <p>[foo]
            //     [bar]</p>
            //     </nav>
            //     </nav>

            SpecTestHelper.AssertCompliance("# foo\n## bar\n\n[foo]\n[bar]",
                "<nav class=\"section section_level_1 section_has-link-icon block\">\n<header class=\"section__header\">\n<h1 class=\"section__heading\">foo</h1>\n<button class=\"section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"section__link-icon\"><use xlink:href=\"#material-design-link\"/></svg>\n</button>\n</header>\n<nav class=\"section section_level_2 section_has-link-icon block\">\n<header class=\"section__header\">\n<h2 class=\"section__heading\">bar</h2>\n<button class=\"section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"section__link-icon\"><use xlink:href=\"#material-design-link\"/></svg>\n</button>\n</header>\n<p>[foo]\n[bar]</p>\n</nav>\n</nav>",
                extensions,
                false,
                "{\n    \"flexiSectionBlocks\": {\n        \"defaultBlockOptions\": {\n            \"blockName\": \"section\",\n            \"element\": \"nav\",\n            \"generateID\": false,\n            \"linkIcon\": \"<svg><use xlink:href=\\\"#material-design-link\\\"/></svg>\",\n            \"attributes\": {\n                \"class\": \"block\"\n            }\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiSectionBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec18(string extensions)
        {
            //     Start line number: 626
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiSectionBlocks": {
            //             "defaultBlockOptions": {
            //                 "element": "nav"
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     o{
            //         "element": "article"
            //     }
            //     # foo
            //     ## bar
            //     --------------- Expected Markup ---------------
            //     <article class="flexi-section flexi-section_level_1 flexi-section_has-link-icon" id="foo">
            //     <header class="flexi-section__header">
            //     <h1 class="flexi-section__heading">foo</h1>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <nav class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="bar">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">bar</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     </nav>
            //     </article>

            SpecTestHelper.AssertCompliance("o{\n    \"element\": \"article\"\n}\n# foo\n## bar",
                "<article class=\"flexi-section flexi-section_level_1 flexi-section_has-link-icon\" id=\"foo\">\n<header class=\"flexi-section__header\">\n<h1 class=\"flexi-section__heading\">foo</h1>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<nav class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"bar\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">bar</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n</nav>\n</article>",
                extensions,
                false,
                "{\n    \"flexiSectionBlocks\": {\n        \"defaultBlockOptions\": {\n            \"element\": \"nav\"\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiSectionBlocks")]
        [InlineData("All")]
        public void FlexiSectionBlocks_Spec19(string extensions)
        {
            //     Start line number: 667
            //     --------------- Markdown ---------------
            //     # foo
            //     
            //     > # foo
            //     > ## foo
            //     
            //     ## foo
            //     --------------- Expected Markup ---------------
            //     <section class="flexi-section flexi-section_level_1 flexi-section_has-link-icon" id="foo">
            //     <header class="flexi-section__header">
            //     <h1 class="flexi-section__heading">foo</h1>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <blockquote>
            //     <section class="flexi-section flexi-section_level_1 flexi-section_has-link-icon" id="foo-1">
            //     <header class="flexi-section__header">
            //     <h1 class="flexi-section__heading">foo</h1>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="foo-2">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">foo</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     </section>
            //     </section>
            //     </blockquote>
            //     <section class="flexi-section flexi-section_level_2 flexi-section_has-link-icon" id="foo-3">
            //     <header class="flexi-section__header">
            //     <h2 class="flexi-section__heading">foo</h2>
            //     <button class="flexi-section__link-button" title="Copy link" aria-label="Copy link">
            //     <svg class="flexi-section__link-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z"/></svg>
            //     </button>
            //     </header>
            //     </section>
            //     </section>

            SpecTestHelper.AssertCompliance("# foo\n\n> # foo\n> ## foo\n\n## foo",
                "<section class=\"flexi-section flexi-section_level_1 flexi-section_has-link-icon\" id=\"foo\">\n<header class=\"flexi-section__header\">\n<h1 class=\"flexi-section__heading\">foo</h1>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<blockquote>\n<section class=\"flexi-section flexi-section_level_1 flexi-section_has-link-icon\" id=\"foo-1\">\n<header class=\"flexi-section__header\">\n<h1 class=\"flexi-section__heading\">foo</h1>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"foo-2\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">foo</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n</section>\n</section>\n</blockquote>\n<section class=\"flexi-section flexi-section_level_2 flexi-section_has-link-icon\" id=\"foo-3\">\n<header class=\"flexi-section__header\">\n<h2 class=\"flexi-section__heading\">foo</h2>\n<button class=\"flexi-section__link-button\" title=\"Copy link\" aria-label=\"Copy link\">\n<svg class=\"flexi-section__link-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"/></svg>\n</button>\n</header>\n</section>\n</section>",
                extensions,
                false);
        }
    }

    public class FlexiTableBlocksSpecs
    {
        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec1(string extensions)
        {
            //     Start line number: 78
            //     --------------- Markdown ---------------
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1   | cell 2   |
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("| header 1 | header 2 |\n|----------|----------|\n| cell 1   | cell 2   |",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec2(string extensions)
        {
            //     Start line number: 121
            //     --------------- Markdown ---------------
            //     The following is not a table:
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1   | cell 2   |
            //     
            //     The following is a table:
            //     
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1   | cell 2   |
            //     --------------- Expected Markup ---------------
            //     <p>The following is not a table:
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1   | cell 2   |</p>
            //     <p>The following is a table:</p>
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("The following is not a table:\n| header 1 | header 2 |\n|----------|----------|\n| cell 1   | cell 2   |\n\nThe following is a table:\n\n| header 1 | header 2 |\n|----------|----------|\n| cell 1   | cell 2   |",
                "<p>The following is not a table:\n| header 1 | header 2 |\n|----------|----------|\n| cell 1   | cell 2   |</p>\n<p>The following is a table:</p>\n<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec3(string extensions)
        {
            //     Start line number: 176
            //     --------------- Markdown ---------------
            //     | header 1 | header 2 | header 3 |
            //     |:---------|:--------:|---------:|
            //     | cell 1   | cell 2   | cell 3   |
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header flexi-table__header_align_start">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header flexi-table__header_align_center">
            //     header 2
            //     </th>
            //     <th class="flexi-table__header flexi-table__header_align_end">
            //     header 3
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data flexi-table__data_align_start">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data flexi-table__data_align_center">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     <td class="flexi-table__data flexi-table__data_align_end">
            //     <div class="flexi-table__label">
            //     header 3
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 3
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("| header 1 | header 2 | header 3 |\n|:---------|:--------:|---------:|\n| cell 1   | cell 2   | cell 3   |",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header flexi-table__header_align_start\">\nheader 1\n</th>\n<th class=\"flexi-table__header flexi-table__header_align_center\">\nheader 2\n</th>\n<th class=\"flexi-table__header flexi-table__header_align_end\">\nheader 3\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data flexi-table__data_align_start\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data flexi-table__data_align_center\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n<td class=\"flexi-table__data flexi-table__data_align_end\">\n<div class=\"flexi-table__label\">\nheader 3\n</div>\n<div class=\"flexi-table__content\">\ncell 3\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec4(string extensions)
        {
            //     Start line number: 230
            //     --------------- Markdown ---------------
            //     |:-------|-------:|
            //     | cell 1 | cell 2 |
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data flexi-table__data_align_start">
            //     cell 1
            //     </td>
            //     <td class="flexi-table__data flexi-table__data_align_end">
            //     cell 2
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("|:-------|-------:|\n| cell 1 | cell 2 |",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data flexi-table__data_align_start\">\ncell 1\n</td>\n<td class=\"flexi-table__data flexi-table__data_align_end\">\ncell 2\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec5(string extensions)
        {
            //     Start line number: 252
            //     --------------- Markdown ---------------
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1   | cell 2   |
            //     |----------|----------|
            //     | cell 3   | cell 4   |
            //     |----------|----------|
            //     | cell 5   | cell 6   |
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     <hr />
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     <hr />
            //     </div>
            //     </td>
            //     </tr>
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 3
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 4
            //     </div>
            //     </td>
            //     </tr>
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     <hr />
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     <hr />
            //     </div>
            //     </td>
            //     </tr>
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 5
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 6
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("| header 1 | header 2 |\n|----------|----------|\n| cell 1   | cell 2   |\n|----------|----------|\n| cell 3   | cell 4   |\n|----------|----------|\n| cell 5   | cell 6   |",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\n<hr />\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\n<hr />\n</div>\n</td>\n</tr>\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 3\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 4\n</div>\n</td>\n</tr>\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\n<hr />\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\n<hr />\n</div>\n</td>\n</tr>\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 5\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 6\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec6(string extensions)
        {
            //     Start line number: 371
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "type": "unresponsive" }
            //     | header 1 | header 2 |
            //     | header 3 | header 4 |
            //     |----------|----------|
            //     | cell 1   | cell 2   |
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_unresponsive">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 3
            //     </th>
            //     <th class="flexi-table__header">
            //     header 4
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     cell 1
            //     </td>
            //     <td class="flexi-table__data">
            //     cell 2
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"type\": \"unresponsive\" }\n| header 1 | header 2 |\n| header 3 | header 4 |\n|----------|----------|\n| cell 1   | cell 2   |",
                "<div class=\"flexi-table flexi-table_type_unresponsive\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 3\n</th>\n<th class=\"flexi-table__header\">\nheader 4\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\ncell 1\n</td>\n<td class=\"flexi-table__data\">\ncell 2\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec7(string extensions)
        {
            //     Start line number: 416
            //     --------------- Markdown ---------------
            //     | cell 1   | cell 2   |
            //     | cell 3   | cell 4   |
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     cell 1
            //     </td>
            //     <td class="flexi-table__data">
            //     cell 2
            //     </td>
            //     </tr>
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     cell 3
            //     </td>
            //     <td class="flexi-table__data">
            //     cell 4
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("| cell 1   | cell 2   |\n| cell 3   | cell 4   |",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\ncell 1\n</td>\n<td class=\"flexi-table__data\">\ncell 2\n</td>\n</tr>\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\ncell 3\n</td>\n<td class=\"flexi-table__data\">\ncell 4\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec8(string extensions)
        {
            //     Start line number: 446
            //     --------------- Markdown ---------------
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     | \| \| \| | \| \| \| |
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     | | |
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     | | |
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("| header 1 | header 2 |\n|----------|----------|\n| \\| \\| \\| | \\| \\| \\| |",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\n| | |\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\n| | |\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec9(string extensions)
        {
            //     Start line number: 489
            //     --------------- Markdown ---------------
            //     | header 1 | header 2 |
            //     |---|---|
            //     | cell 1 | cell 2 |
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("| header 1 | header 2 |\n|---|---|\n| cell 1 | cell 2 |",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec10(string extensions)
        {
            //     Start line number: 532
            //     --------------- Markdown ---------------
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1 | cell 2 | cell 3 |
            //     --------------- Expected Markup ---------------
            //     <p>| header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1 | cell 2 | cell 3 |</p>

            SpecTestHelper.AssertCompliance("| header 1 | header 2 |\n|----------|----------|\n| cell 1 | cell 2 | cell 3 |",
                "<p>| header 1 | header 2 |\n|----------|----------|\n| cell 1 | cell 2 | cell 3 |</p>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec11(string extensions)
        {
            //     Start line number: 544
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     |cell 1    |    cell 2|
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">cell 2
            //     </code></pre>
            //     </div>
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("| header 1 | header 2 |\n|----------|----------|\n|cell 1    |    cell 2|",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\n<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">cell 2\n</code></pre>\n</div>\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec12(string extensions)
        {
            //     Start line number: 597
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     | **header 1** | [header 2](url) | *header 3* |
            //     |----------|----------|----------|
            //     | `cell 1` |    cell 2 | > cell 3 |
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     <strong>header 1</strong>
            //     </th>
            //     <th class="flexi-table__header">
            //     <a href="url">header 2</a>
            //     </th>
            //     <th class="flexi-table__header">
            //     <em>header 3</em>
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     <strong>header 1</strong>
            //     </div>
            //     <div class="flexi-table__content">
            //     <code>cell 1</code>
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     <a href="url">header 2</a>
            //     </div>
            //     <div class="flexi-table__content">
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">cell 2
            //     </code></pre>
            //     </div>
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     <em>header 3</em>
            //     </div>
            //     <div class="flexi-table__content">
            //     <blockquote>
            //     <p>cell 3</p>
            //     </blockquote>
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("| **header 1** | [header 2](url) | *header 3* |\n|----------|----------|----------|\n| `cell 1` |    cell 2 | > cell 3 |",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\n<strong>header 1</strong>\n</th>\n<th class=\"flexi-table__header\">\n<a href=\"url\">header 2</a>\n</th>\n<th class=\"flexi-table__header\">\n<em>header 3</em>\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\n<strong>header 1</strong>\n</div>\n<div class=\"flexi-table__content\">\n<code>cell 1</code>\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\n<a href=\"url\">header 2</a>\n</div>\n<div class=\"flexi-table__content\">\n<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">cell 2\n</code></pre>\n</div>\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\n<em>header 3</em>\n</div>\n<div class=\"flexi-table__content\">\n<blockquote>\n<p>cell 3</p>\n</blockquote>\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec13(string extensions)
        {
            //     Start line number: 663
            //     --------------- Markdown ---------------
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1   | cell 2   |
            //     This line ends the table
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>
            //     <p>This line ends the table</p>

            SpecTestHelper.AssertCompliance("| header 1 | header 2 |\n|----------|----------|\n| cell 1   | cell 2   |\nThis line ends the table",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>\n<p>This line ends the table</p>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec14(string extensions)
        {
            //     Start line number: 708
            //     --------------- Markdown ---------------
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1   | cell 2   |
            //     | This line invalidates the preceding table
            //     
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1   | cell 2   |
            //     
            //     | This line does not invalidate the preceding table
            //     --------------- Expected Markup ---------------
            //     <p>| header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1   | cell 2   |
            //     | This line invalidates the preceding table</p>
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>
            //     <p>| This line does not invalidate the preceding table</p>

            SpecTestHelper.AssertCompliance("| header 1 | header 2 |\n|----------|----------|\n| cell 1   | cell 2   |\n| This line invalidates the preceding table\n\n| header 1 | header 2 |\n|----------|----------|\n| cell 1   | cell 2   |\n\n| This line does not invalidate the preceding table",
                "<p>| header 1 | header 2 |\n|----------|----------|\n| cell 1   | cell 2   |\n| This line invalidates the preceding table</p>\n<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>\n<p>| This line does not invalidate the preceding table</p>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec15(string extensions)
        {
            //     Start line number: 776
            //     --------------- Markdown ---------------
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+--------------+--------------+",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec16(string extensions)
        {
            //     Start line number: 821
            //     --------------- Markdown ---------------
            //     The following is not a table:
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+
            //     
            //     The following is a table:
            //     
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <p>The following is not a table:
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+</p>
            //     <p>The following is a table:</p>
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("The following is not a table:\n+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+--------------+--------------+\n\nThe following is a table:\n\n+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+--------------+--------------+",
                "<p>The following is not a table:\n+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+--------------+--------------+</p>\n<p>The following is a table:</p>\n<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec17(string extensions)
        {
            //     Start line number: 882
            //     --------------- Markdown ---------------
            //     +:-------------+:------------:+-------------:+
            //     | header 1     | header 2     | header 3     |
            //     +==============+==============+==============+
            //     | cell 1       | cell 2       | cell 3       |
            //     +--------------+--------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header flexi-table__header_align_start">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header flexi-table__header_align_center">
            //     header 2
            //     </th>
            //     <th class="flexi-table__header flexi-table__header_align_end">
            //     header 3
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data flexi-table__data_align_start">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data flexi-table__data_align_center">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     <td class="flexi-table__data flexi-table__data_align_end">
            //     <div class="flexi-table__label">
            //     header 3
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 3
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("+:-------------+:------------:+-------------:+\n| header 1     | header 2     | header 3     |\n+==============+==============+==============+\n| cell 1       | cell 2       | cell 3       |\n+--------------+--------------+--------------+",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header flexi-table__header_align_start\">\nheader 1\n</th>\n<th class=\"flexi-table__header flexi-table__header_align_center\">\nheader 2\n</th>\n<th class=\"flexi-table__header flexi-table__header_align_end\">\nheader 3\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data flexi-table__data_align_start\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data flexi-table__data_align_center\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n<td class=\"flexi-table__data flexi-table__data_align_end\">\n<div class=\"flexi-table__label\">\nheader 3\n</div>\n<div class=\"flexi-table__content\">\ncell 3\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec18(string extensions)
        {
            //     Start line number: 938
            //     --------------- Markdown ---------------
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec19(string extensions)
        {
            //     Start line number: 983
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "type": "unresponsive" }
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     + still cell 1 +--------------+
            //     | still cell 1 | cell 3       |
            //     +--------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_unresponsive">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data" rowspan="2">
            //     cell 1
            //     still cell 1
            //     still cell 1
            //     </td>
            //     <td class="flexi-table__data">
            //     cell 2
            //     </td>
            //     </tr>
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     cell 3
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"type\": \"unresponsive\" }\n+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+ still cell 1 +--------------+\n| still cell 1 | cell 3       |\n+--------------+--------------+",
                "<div class=\"flexi-table flexi-table_type_unresponsive\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\" rowspan=\"2\">\ncell 1\nstill cell 1\nstill cell 1\n</td>\n<td class=\"flexi-table__data\">\ncell 2\n</td>\n</tr>\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\ncell 3\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec20(string extensions)
        {
            //     Start line number: 1030
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "type": "unresponsive" }
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +==============+==============+
            //     | still cell 1 | still cell 2 |
            //     +==============+==============+
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_unresponsive">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data" rowspan="2">
            //     <h1>cell 1</h1>
            //     <h1>still cell 1</h1>
            //     </td>
            //     <td class="flexi-table__data" rowspan="2">
            //     <h1>cell 2</h1>
            //     <h1>still cell 2</h1>
            //     </td>
            //     </tr>
            //     <tr class="flexi-table__row">
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"type\": \"unresponsive\" }\n+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+==============+==============+\n| still cell 1 | still cell 2 |\n+==============+==============+",
                "<div class=\"flexi-table flexi-table_type_unresponsive\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\" rowspan=\"2\">\n<h1>cell 1</h1>\n<h1>still cell 1</h1>\n</td>\n<td class=\"flexi-table__data\" rowspan=\"2\">\n<h1>cell 2</h1>\n<h1>still cell 2</h1>\n</td>\n</tr>\n<tr class=\"flexi-table__row\">\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec21(string extensions)
        {
            //     Start line number: 1074
            //     --------------- Markdown ---------------
            //     +--------------+--------------+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+
            //     | cell 3       | cell 4       |
            //     +--------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     cell 1
            //     </td>
            //     <td class="flexi-table__data">
            //     cell 2
            //     </td>
            //     </tr>
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     cell 3
            //     </td>
            //     <td class="flexi-table__data">
            //     cell 4
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("+--------------+--------------+\n| cell 1       | cell 2       |\n+--------------+--------------+\n| cell 3       | cell 4       |\n+--------------+--------------+",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\ncell 1\n</td>\n<td class=\"flexi-table__data\">\ncell 2\n</td>\n</tr>\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\ncell 3\n</td>\n<td class=\"flexi-table__data\">\ncell 4\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec22(string extensions)
        {
            //     Start line number: 1107
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "type": "unresponsive" }
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +--------------+--------------+
            //     | header 3     | header 4     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_unresponsive">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 3
            //     </th>
            //     <th class="flexi-table__header">
            //     header 4
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     cell 1
            //     </td>
            //     <td class="flexi-table__data">
            //     cell 2
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"type\": \"unresponsive\" }\n+--------------+--------------+\n| header 1     | header 2     |\n+--------------+--------------+\n| header 3     | header 4     |\n+==============+==============+\n| cell 1       | cell 2       |\n+--------------+--------------+",
                "<div class=\"flexi-table flexi-table_type_unresponsive\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 3\n</th>\n<th class=\"flexi-table__header\">\nheader 4\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\ncell 1\n</td>\n<td class=\"flexi-table__data\">\ncell 2\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec23(string extensions)
        {
            //     Start line number: 1155
            //     --------------- Markdown ---------------
            //     +--------------+----------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+
            //     
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==========+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+
            //     
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1   | cell 2   |
            //     +--------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <p>+--------------+----------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+</p>
            //     <p>+--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==========+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+</p>
            //     <p>+--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1   | cell 2   |
            //     +--------------+--------------+</p>

            SpecTestHelper.AssertCompliance("+--------------+----------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+--------------+--------------+\n\n+--------------+--------------+\n| header 1     | header 2     |\n+==============+==========+\n| cell 1       | cell 2       |\n+--------------+--------------+\n\n+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1   | cell 2   |\n+--------------+--------------+",
                "<p>+--------------+----------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+--------------+--------------+</p>\n<p>+--------------+--------------+\n| header 1     | header 2     |\n+==============+==========+\n| cell 1       | cell 2       |\n+--------------+--------------+</p>\n<p>+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1   | cell 2   |\n+--------------+--------------+</p>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec24(string extensions)
        {
            //     Start line number: 1193
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "type": "unresponsive" }
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1                      |
            //     +--------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_unresponsive">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data" colspan="2">
            //     cell 1
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"type\": \"unresponsive\" }\n+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1                      |\n+--------------+--------------+",
                "<div class=\"flexi-table flexi-table_type_unresponsive\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\" colspan=\"2\">\ncell 1\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec25(string extensions)
        {
            //     Start line number: 1228
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "type": "unresponsive" }
            //     +--------------+--------------+--------------+
            //     | header 1     | header 2     | header 3     |
            //     +==============+==============+==============+
            //     | no span      | rowspan and colspan         |
            //     +--------------+                             +
            //     | no span      |                             |
            //     +--------------+--------------+--------------+
            //     | rowspan      | colspan                     |
            //     +              +--------------+--------------+
            //     |              | no span      | no span      |
            //     +--------------+--------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_unresponsive">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     <th class="flexi-table__header">
            //     header 3
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     no span
            //     </td>
            //     <td class="flexi-table__data" colspan="2" rowspan="2">
            //     rowspan and colspan
            //     </td>
            //     </tr>
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     no span
            //     </td>
            //     </tr>
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data" rowspan="2">
            //     rowspan
            //     </td>
            //     <td class="flexi-table__data" colspan="2">
            //     colspan
            //     </td>
            //     </tr>
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     no span
            //     </td>
            //     <td class="flexi-table__data">
            //     no span
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"type\": \"unresponsive\" }\n+--------------+--------------+--------------+\n| header 1     | header 2     | header 3     |\n+==============+==============+==============+\n| no span      | rowspan and colspan         |\n+--------------+                             +\n| no span      |                             |\n+--------------+--------------+--------------+\n| rowspan      | colspan                     |\n+              +--------------+--------------+\n|              | no span      | no span      |\n+--------------+--------------+--------------+",
                "<div class=\"flexi-table flexi-table_type_unresponsive\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n<th class=\"flexi-table__header\">\nheader 3\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\nno span\n</td>\n<td class=\"flexi-table__data\" colspan=\"2\" rowspan=\"2\">\nrowspan and colspan\n</td>\n</tr>\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\nno span\n</td>\n</tr>\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\" rowspan=\"2\">\nrowspan\n</td>\n<td class=\"flexi-table__data\" colspan=\"2\">\ncolspan\n</td>\n</tr>\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\nno span\n</td>\n<td class=\"flexi-table__data\">\nno span\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec26(string extensions)
        {
            //     Start line number: 1296
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       |       cell 2 |
            //     +--------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">   cell 2
            //     </code></pre>
            //     </div>
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       |       cell 2 |\n+--------------+--------------+",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\n<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">   cell 2\n</code></pre>\n</div>\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec27(string extensions)
        {
            //     Start line number: 1351
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     +--------------+-----------------+--------------+
            //     | **header 1** | [header 2](url) | *header 3*   |
            //     +==============+=================+==============+
            //     | - cell 1     | ```             | > cell 3     |
            //     | - cell 2     | cell 2          | > cell 3     |
            //     | - cell 3     | ```             | > cell 3     |
            //     +--------------+-----------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     <strong>header 1</strong>
            //     </th>
            //     <th class="flexi-table__header">
            //     <a href="url">header 2</a>
            //     </th>
            //     <th class="flexi-table__header">
            //     <em>header 3</em>
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     <strong>header 1</strong>
            //     </div>
            //     <div class="flexi-table__content">
            //     <ul>
            //     <li>cell 1</li>
            //     <li>cell 2</li>
            //     <li>cell 3</li>
            //     </ul>
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     <a href="url">header 2</a>
            //     </div>
            //     <div class="flexi-table__content">
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">cell 2
            //     </code></pre>
            //     </div>
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     <em>header 3</em>
            //     </div>
            //     <div class="flexi-table__content">
            //     <blockquote>
            //     <p>cell 3
            //     cell 3
            //     cell 3</p>
            //     </blockquote>
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("+--------------+-----------------+--------------+\n| **header 1** | [header 2](url) | *header 3*   |\n+==============+=================+==============+\n| - cell 1     | ```             | > cell 3     |\n| - cell 2     | cell 2          | > cell 3     |\n| - cell 3     | ```             | > cell 3     |\n+--------------+-----------------+--------------+",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\n<strong>header 1</strong>\n</th>\n<th class=\"flexi-table__header\">\n<a href=\"url\">header 2</a>\n</th>\n<th class=\"flexi-table__header\">\n<em>header 3</em>\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\n<strong>header 1</strong>\n</div>\n<div class=\"flexi-table__content\">\n<ul>\n<li>cell 1</li>\n<li>cell 2</li>\n<li>cell 3</li>\n</ul>\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\n<a href=\"url\">header 2</a>\n</div>\n<div class=\"flexi-table__content\">\n<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">cell 2\n</code></pre>\n</div>\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\n<em>header 3</em>\n</div>\n<div class=\"flexi-table__content\">\n<blockquote>\n<p>cell 3\ncell 3\ncell 3</p>\n</blockquote>\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec28(string extensions)
        {
            //     Start line number: 1427
            //     --------------- Markdown ---------------
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     This line ends the table
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>
            //     <p>This line ends the table</p>

            SpecTestHelper.AssertCompliance("+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\nThis line ends the table",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>\n<p>This line ends the table</p>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec29(string extensions)
        {
            //     Start line number: 1473
            //     --------------- Markdown ---------------
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+
            //     | This line invalidates the preceding table
            //     
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     + This line invalidates the preceding table
            //     
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+
            //     
            //     | This line does not invalidate the preceding table
            //     
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     
            //     + This line does not invalidate the preceding table
            //     --------------- Expected Markup ---------------
            //     <p>+--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+
            //     | This line invalidates the preceding table</p>
            //     <p>+--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |</p>
            //     <ul>
            //     <li>This line invalidates the preceding table</li>
            //     </ul>
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>
            //     <p>| This line does not invalidate the preceding table</p>
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>
            //     <ul>
            //     <li>This line does not invalidate the preceding table</li>
            //     </ul>

            SpecTestHelper.AssertCompliance("+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+--------------+--------------+\n| This line invalidates the preceding table\n\n+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+ This line invalidates the preceding table\n\n+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+--------------+--------------+\n\n| This line does not invalidate the preceding table\n\n+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n\n+ This line does not invalidate the preceding table",
                "<p>+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+--------------+--------------+\n| This line invalidates the preceding table</p>\n<p>+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |</p>\n<ul>\n<li>This line invalidates the preceding table</li>\n</ul>\n<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>\n<p>| This line does not invalidate the preceding table</p>\n<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>\n<ul>\n<li>This line does not invalidate the preceding table</li>\n</ul>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec30(string extensions)
        {
            //     Start line number: 1613
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "blockName": "table" }
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1   | cell 2   |
            //     --------------- Expected Markup ---------------
            //     <div class="table table_type_cards">
            //     <table class="table__table">
            //     <thead class="table__head">
            //     <tr class="table__row">
            //     <th class="table__header">
            //     header 1
            //     </th>
            //     <th class="table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="table__body">
            //     <tr class="table__row">
            //     <td class="table__data">
            //     <div class="table__label">
            //     header 1
            //     </div>
            //     <div class="table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="table__data">
            //     <div class="table__label">
            //     header 2
            //     </div>
            //     <div class="table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"blockName\": \"table\" }\n| header 1 | header 2 |\n|----------|----------|\n| cell 1   | cell 2   |",
                "<div class=\"table table_type_cards\">\n<table class=\"table__table\">\n<thead class=\"table__head\">\n<tr class=\"table__row\">\n<th class=\"table__header\">\nheader 1\n</th>\n<th class=\"table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"table__body\">\n<tr class=\"table__row\">\n<td class=\"table__data\">\n<div class=\"table__label\">\nheader 1\n</div>\n<div class=\"table__content\">\ncell 1\n</div>\n</td>\n<td class=\"table__data\">\n<div class=\"table__label\">\nheader 2\n</div>\n<div class=\"table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec31(string extensions)
        {
            //     Start line number: 1667
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "type": "cards" }
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"type\": \"cards\" }\n+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+--------------+--------------+",
                "<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec32(string extensions)
        {
            //     Start line number: 1714
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "type": "fixedTitles" }
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1   | cell 2   |
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_fixed-titles">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     cell 1
            //     </td>
            //     <td class="flexi-table__data">
            //     cell 2
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"type\": \"fixedTitles\" }\n| header 1 | header 2 |\n|----------|----------|\n| cell 1   | cell 2   |",
                "<div class=\"flexi-table flexi-table_type_fixed-titles\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\ncell 1\n</td>\n<td class=\"flexi-table__data\">\ncell 2\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec33(string extensions)
        {
            //     Start line number: 1749
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "type": "unresponsive" }
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_unresponsive">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     cell 1
            //     </td>
            //     <td class="flexi-table__data">
            //     cell 2
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"type\": \"unresponsive\" }\n+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+--------------+--------------+",
                "<div class=\"flexi-table flexi-table_type_unresponsive\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\ncell 1\n</td>\n<td class=\"flexi-table__data\">\ncell 2\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec34(string extensions)
        {
            //     Start line number: 1794
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{
            //         "attributes": {
            //             "id" : "my-custom-id",
            //             "class" : "my-custom-class"
            //         }
            //     }
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1   | cell 2   |
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_cards my-custom-class" id="my-custom-id">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("o{\n    \"attributes\": {\n        \"id\" : \"my-custom-id\",\n        \"class\" : \"my-custom-class\"\n    }\n}\n| header 1 | header 2 |\n|----------|----------|\n| cell 1   | cell 2   |",
                "<div class=\"flexi-table flexi-table_type_cards my-custom-class\" id=\"my-custom-id\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTableBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec35(string extensions)
        {
            //     Start line number: 1859
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiTableBlocks": {
            //             "defaultBlockOptions": {
            //                 "blockName": "table",
            //                 "type": "unresponsive",
            //                 "attributes": {
            //                     "class": "block"
            //                 }
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <div class="table table_type_unresponsive block">
            //     <table class="table__table">
            //     <thead class="table__head">
            //     <tr class="table__row">
            //     <th class="table__header">
            //     header 1
            //     </th>
            //     <th class="table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="table__body">
            //     <tr class="table__row">
            //     <td class="table__data">
            //     cell 1
            //     </td>
            //     <td class="table__data">
            //     cell 2
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+--------------+--------------+",
                "<div class=\"table table_type_unresponsive block\">\n<table class=\"table__table\">\n<thead class=\"table__head\">\n<tr class=\"table__row\">\n<th class=\"table__header\">\nheader 1\n</th>\n<th class=\"table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"table__body\">\n<tr class=\"table__row\">\n<td class=\"table__data\">\ncell 1\n</td>\n<td class=\"table__data\">\ncell 2\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false,
                "{\n    \"flexiTableBlocks\": {\n        \"defaultBlockOptions\": {\n            \"blockName\": \"table\",\n            \"type\": \"unresponsive\",\n            \"attributes\": {\n                \"class\": \"block\"\n            }\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiTableBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiTableBlocks_Spec36(string extensions)
        {
            //     Start line number: 1905
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiTableBlocks": {
            //             "defaultBlockOptions": {
            //                 "type": "unresponsive"
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     | header 1 | header 2 |
            //     |----------|----------|
            //     | cell 1   | cell 2   |
            //     
            //     o{ "type": "cards" }
            //     +--------------+--------------+
            //     | header 1     | header 2     |
            //     +==============+==============+
            //     | cell 1       | cell 2       |
            //     +--------------+--------------+
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-table flexi-table_type_unresponsive">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     cell 1
            //     </td>
            //     <td class="flexi-table__data">
            //     cell 2
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>
            //     <div class="flexi-table flexi-table_type_cards">
            //     <table class="flexi-table__table">
            //     <thead class="flexi-table__head">
            //     <tr class="flexi-table__row">
            //     <th class="flexi-table__header">
            //     header 1
            //     </th>
            //     <th class="flexi-table__header">
            //     header 2
            //     </th>
            //     </tr>
            //     </thead>
            //     <tbody class="flexi-table__body">
            //     <tr class="flexi-table__row">
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 1
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 1
            //     </div>
            //     </td>
            //     <td class="flexi-table__data">
            //     <div class="flexi-table__label">
            //     header 2
            //     </div>
            //     <div class="flexi-table__content">
            //     cell 2
            //     </div>
            //     </td>
            //     </tr>
            //     </tbody>
            //     </table>
            //     </div>

            SpecTestHelper.AssertCompliance("| header 1 | header 2 |\n|----------|----------|\n| cell 1   | cell 2   |\n\no{ \"type\": \"cards\" }\n+--------------+--------------+\n| header 1     | header 2     |\n+==============+==============+\n| cell 1       | cell 2       |\n+--------------+--------------+",
                "<div class=\"flexi-table flexi-table_type_unresponsive\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\ncell 1\n</td>\n<td class=\"flexi-table__data\">\ncell 2\n</td>\n</tr>\n</tbody>\n</table>\n</div>\n<div class=\"flexi-table flexi-table_type_cards\">\n<table class=\"flexi-table__table\">\n<thead class=\"flexi-table__head\">\n<tr class=\"flexi-table__row\">\n<th class=\"flexi-table__header\">\nheader 1\n</th>\n<th class=\"flexi-table__header\">\nheader 2\n</th>\n</tr>\n</thead>\n<tbody class=\"flexi-table__body\">\n<tr class=\"flexi-table__row\">\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 1\n</div>\n<div class=\"flexi-table__content\">\ncell 1\n</div>\n</td>\n<td class=\"flexi-table__data\">\n<div class=\"flexi-table__label\">\nheader 2\n</div>\n<div class=\"flexi-table__content\">\ncell 2\n</div>\n</td>\n</tr>\n</tbody>\n</table>\n</div>",
                extensions,
                false,
                "{\n    \"flexiTableBlocks\": {\n        \"defaultBlockOptions\": {\n            \"type\": \"unresponsive\"\n        }\n    }\n}");
        }
    }

    public class FlexiTabsBlocksSpecs
    {
        [Theory]
        [InlineData("FlexiTabsBlocks")]
        [InlineData("All")]
        public void FlexiTabsBlocks_Spec1(string extensions)
        {
            //     Start line number: 61
            //     --------------- Markdown ---------------
            //     ///
            //     +++ tab
            //     Tab 1
            //     +++
            //     Panel 1
            //     +++
            //     
            //     +++ tab
            //     Tab 2
            //     +++
            //     Panel 2
            //     +++
            //     ///
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-tabs">
            //     <div class="flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal">
            //     <div class="flexi-tabs__tab-list scrollable-indicators__scrollable" role="tablist">
            //     <button class="flexi-tabs__tab flexi-tabs__tab_selected" title="View panel" role="tab" aria-selected="true">Tab 1</button>
            //     <button class="flexi-tabs__tab" title="View panel" role="tab" aria-selected="false" tabindex="-1">Tab 2</button>
            //     </div>
            //     <div class="scrollable-indicators__indicator scrollable-indicators__indicator_start"></div>
            //     <div class="scrollable-indicators__indicator scrollable-indicators__indicator_end"></div>
            //     </div>
            //     <div class="flexi-tabs__tab-panel" tabindex="0" role="tabpanel" aria-label="Tab 1">
            //     <p>Panel 1</p>
            //     </div>
            //     <div class="flexi-tabs__tab-panel flexi-tabs__tab-panel_hidden" tabindex="0" role="tabpanel" aria-label="Tab 2">
            //     <p>Panel 2</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("///\n+++ tab\nTab 1\n+++\nPanel 1\n+++\n\n+++ tab\nTab 2\n+++\nPanel 2\n+++\n///",
                "<div class=\"flexi-tabs\">\n<div class=\"flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal\">\n<div class=\"flexi-tabs__tab-list scrollable-indicators__scrollable\" role=\"tablist\">\n<button class=\"flexi-tabs__tab flexi-tabs__tab_selected\" title=\"View panel\" role=\"tab\" aria-selected=\"true\">Tab 1</button>\n<button class=\"flexi-tabs__tab\" title=\"View panel\" role=\"tab\" aria-selected=\"false\" tabindex=\"-1\">Tab 2</button>\n</div>\n<div class=\"scrollable-indicators__indicator scrollable-indicators__indicator_start\"></div>\n<div class=\"scrollable-indicators__indicator scrollable-indicators__indicator_end\"></div>\n</div>\n<div class=\"flexi-tabs__tab-panel\" tabindex=\"0\" role=\"tabpanel\" aria-label=\"Tab 1\">\n<p>Panel 1</p>\n</div>\n<div class=\"flexi-tabs__tab-panel flexi-tabs__tab-panel_hidden\" tabindex=\"0\" role=\"tabpanel\" aria-label=\"Tab 2\">\n<p>Panel 2</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTabsBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiTabsBlocks_Spec2(string extensions)
        {
            //     Start line number: 100
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     ///
            //     +++ tab
            //     *Tab 1*
            //     +++
            //     - Panel 1
            //     +++
            //     
            //     +++ tab
            //     **Tab 2**
            //     +++
            //     ```
            //     Panel 2
            //     ```
            //     +++
            //     ///
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-tabs">
            //     <div class="flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal">
            //     <div class="flexi-tabs__tab-list scrollable-indicators__scrollable" role="tablist">
            //     <button class="flexi-tabs__tab flexi-tabs__tab_selected" title="View panel" role="tab" aria-selected="true"><em>Tab 1</em></button>
            //     <button class="flexi-tabs__tab" title="View panel" role="tab" aria-selected="false" tabindex="-1"><strong>Tab 2</strong></button>
            //     </div>
            //     <div class="scrollable-indicators__indicator scrollable-indicators__indicator_start"></div>
            //     <div class="scrollable-indicators__indicator scrollable-indicators__indicator_end"></div>
            //     </div>
            //     <div class="flexi-tabs__tab-panel" tabindex="0" role="tabpanel" aria-label="Tab 1">
            //     <ul>
            //     <li>Panel 1</li>
            //     </ul>
            //     </div>
            //     <div class="flexi-tabs__tab-panel flexi-tabs__tab-panel_hidden" tabindex="0" role="tabpanel" aria-label="Tab 2">
            //     <div class="flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases">
            //     <header class="flexi-code__header">
            //     <button class="flexi-code__copy-button" title="Copy code" aria-label="Copy code">
            //     <svg class="flexi-code__copy-icon" xmlns="http://www.w3.org/2000/svg" width="18px" height="18px" viewBox="0 0 18 18"><path fill="none" d="M0,0h18v18H0V0z"/><path d="M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z"/></svg>
            //     </button>
            //     </header>
            //     <pre class="flexi-code__pre"><code class="flexi-code__code">Panel 2
            //     </code></pre>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("///\n+++ tab\n*Tab 1*\n+++\n- Panel 1\n+++\n\n+++ tab\n**Tab 2**\n+++\n```\nPanel 2\n```\n+++\n///",
                "<div class=\"flexi-tabs\">\n<div class=\"flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal\">\n<div class=\"flexi-tabs__tab-list scrollable-indicators__scrollable\" role=\"tablist\">\n<button class=\"flexi-tabs__tab flexi-tabs__tab_selected\" title=\"View panel\" role=\"tab\" aria-selected=\"true\"><em>Tab 1</em></button>\n<button class=\"flexi-tabs__tab\" title=\"View panel\" role=\"tab\" aria-selected=\"false\" tabindex=\"-1\"><strong>Tab 2</strong></button>\n</div>\n<div class=\"scrollable-indicators__indicator scrollable-indicators__indicator_start\"></div>\n<div class=\"scrollable-indicators__indicator scrollable-indicators__indicator_end\"></div>\n</div>\n<div class=\"flexi-tabs__tab-panel\" tabindex=\"0\" role=\"tabpanel\" aria-label=\"Tab 1\">\n<ul>\n<li>Panel 1</li>\n</ul>\n</div>\n<div class=\"flexi-tabs__tab-panel flexi-tabs__tab-panel_hidden\" tabindex=\"0\" role=\"tabpanel\" aria-label=\"Tab 2\">\n<div class=\"flexi-code flexi-code_no-title flexi-code_has-copy-icon flexi-code_has-header flexi-code_no-syntax-highlights flexi-code_no-line-numbers flexi-code_has-omitted-lines-icon flexi-code_no-highlighted-lines flexi-code_no-highlighted-phrases\">\n<header class=\"flexi-code__header\">\n<button class=\"flexi-code__copy-button\" title=\"Copy code\" aria-label=\"Copy code\">\n<svg class=\"flexi-code__copy-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"18px\" height=\"18px\" viewBox=\"0 0 18 18\"><path fill=\"none\" d=\"M0,0h18v18H0V0z\"/><path d=\"M12,1H2v13h2V3h8V1z M12,4l4,4v9H5V4H12z M11,9h4l-4-4V9z\"/></svg>\n</button>\n</header>\n<pre class=\"flexi-code__pre\"><code class=\"flexi-code__code\">Panel 2\n</code></pre>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTabsBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiTabsBlocks_Spec3(string extensions)
        {
            //     Start line number: 162
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ "blockName": "tabs" }
            //     ///
            //     +++ tab
            //     Tab 1
            //     +++
            //     Panel 1
            //     +++
            //     ///
            //     --------------- Expected Markup ---------------
            //     <div class="tabs">
            //     <div class="tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal">
            //     <div class="tabs__tab-list scrollable-indicators__scrollable" role="tablist">
            //     <button class="tabs__tab tabs__tab_selected" title="View panel" role="tab" aria-selected="true">Tab 1</button>
            //     </div>
            //     <div class="scrollable-indicators__indicator scrollable-indicators__indicator_start"></div>
            //     <div class="scrollable-indicators__indicator scrollable-indicators__indicator_end"></div>
            //     </div>
            //     <div class="tabs__tab-panel" tabindex="0" role="tabpanel" aria-label="Tab 1">
            //     <p>Panel 1</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \"blockName\": \"tabs\" }\n///\n+++ tab\nTab 1\n+++\nPanel 1\n+++\n///",
                "<div class=\"tabs\">\n<div class=\"tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal\">\n<div class=\"tabs__tab-list scrollable-indicators__scrollable\" role=\"tablist\">\n<button class=\"tabs__tab tabs__tab_selected\" title=\"View panel\" role=\"tab\" aria-selected=\"true\">Tab 1</button>\n</div>\n<div class=\"scrollable-indicators__indicator scrollable-indicators__indicator_start\"></div>\n<div class=\"scrollable-indicators__indicator scrollable-indicators__indicator_end\"></div>\n</div>\n<div class=\"tabs__tab-panel\" tabindex=\"0\" role=\"tabpanel\" aria-label=\"Tab 1\">\n<p>Panel 1</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTabsBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiTabsBlocks_Spec4(string extensions)
        {
            //     Start line number: 195
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ 
            //         "defaultTabOptions": {
            //             "attributes": {
            //                 "class" : "my-custom-class"
            //             }
            //         }
            //     }
            //     ///
            //     +++ tab
            //     Tab 1
            //     +++
            //     Panel 1
            //     +++
            //     ///
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-tabs">
            //     <div class="flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal">
            //     <div class="flexi-tabs__tab-list scrollable-indicators__scrollable" role="tablist">
            //     <button class="flexi-tabs__tab flexi-tabs__tab_selected" title="View panel" role="tab" aria-selected="true">Tab 1</button>
            //     </div>
            //     <div class="scrollable-indicators__indicator scrollable-indicators__indicator_start"></div>
            //     <div class="scrollable-indicators__indicator scrollable-indicators__indicator_end"></div>
            //     </div>
            //     <div class="flexi-tabs__tab-panel my-custom-class" tabindex="0" role="tabpanel" aria-label="Tab 1">
            //     <p>Panel 1</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \n    \"defaultTabOptions\": {\n        \"attributes\": {\n            \"class\" : \"my-custom-class\"\n        }\n    }\n}\n///\n+++ tab\nTab 1\n+++\nPanel 1\n+++\n///",
                "<div class=\"flexi-tabs\">\n<div class=\"flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal\">\n<div class=\"flexi-tabs__tab-list scrollable-indicators__scrollable\" role=\"tablist\">\n<button class=\"flexi-tabs__tab flexi-tabs__tab_selected\" title=\"View panel\" role=\"tab\" aria-selected=\"true\">Tab 1</button>\n</div>\n<div class=\"scrollable-indicators__indicator scrollable-indicators__indicator_start\"></div>\n<div class=\"scrollable-indicators__indicator scrollable-indicators__indicator_end\"></div>\n</div>\n<div class=\"flexi-tabs__tab-panel my-custom-class\" tabindex=\"0\" role=\"tabpanel\" aria-label=\"Tab 1\">\n<p>Panel 1</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTabsBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiTabsBlocks_Spec5(string extensions)
        {
            //     Start line number: 228
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{ 
            //         "defaultTabOptions": {
            //             "attributes": {
            //                 "class" : "my-custom-class"
            //             }
            //         }
            //     }
            //     ///
            //     +++ tab
            //     Tab 1
            //     +++
            //     Panel 1
            //     +++
            //     
            //     o{ 
            //         "attributes": {
            //             "class" : "alt-custom-class"
            //         }
            //     }
            //     +++ tab
            //     Tab 2
            //     +++
            //     Panel 2
            //     +++
            //     ///
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-tabs">
            //     <div class="flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal">
            //     <div class="flexi-tabs__tab-list scrollable-indicators__scrollable" role="tablist">
            //     <button class="flexi-tabs__tab flexi-tabs__tab_selected" title="View panel" role="tab" aria-selected="true">Tab 1</button>
            //     <button class="flexi-tabs__tab" title="View panel" role="tab" aria-selected="false" tabindex="-1">Tab 2</button>
            //     </div>
            //     <div class="scrollable-indicators__indicator scrollable-indicators__indicator_start"></div>
            //     <div class="scrollable-indicators__indicator scrollable-indicators__indicator_end"></div>
            //     </div>
            //     <div class="flexi-tabs__tab-panel my-custom-class" tabindex="0" role="tabpanel" aria-label="Tab 1">
            //     <p>Panel 1</p>
            //     </div>
            //     <div class="flexi-tabs__tab-panel flexi-tabs__tab-panel_hidden alt-custom-class" tabindex="0" role="tabpanel" aria-label="Tab 2">
            //     <p>Panel 2</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("o{ \n    \"defaultTabOptions\": {\n        \"attributes\": {\n            \"class\" : \"my-custom-class\"\n        }\n    }\n}\n///\n+++ tab\nTab 1\n+++\nPanel 1\n+++\n\no{ \n    \"attributes\": {\n        \"class\" : \"alt-custom-class\"\n    }\n}\n+++ tab\nTab 2\n+++\nPanel 2\n+++\n///",
                "<div class=\"flexi-tabs\">\n<div class=\"flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal\">\n<div class=\"flexi-tabs__tab-list scrollable-indicators__scrollable\" role=\"tablist\">\n<button class=\"flexi-tabs__tab flexi-tabs__tab_selected\" title=\"View panel\" role=\"tab\" aria-selected=\"true\">Tab 1</button>\n<button class=\"flexi-tabs__tab\" title=\"View panel\" role=\"tab\" aria-selected=\"false\" tabindex=\"-1\">Tab 2</button>\n</div>\n<div class=\"scrollable-indicators__indicator scrollable-indicators__indicator_start\"></div>\n<div class=\"scrollable-indicators__indicator scrollable-indicators__indicator_end\"></div>\n</div>\n<div class=\"flexi-tabs__tab-panel my-custom-class\" tabindex=\"0\" role=\"tabpanel\" aria-label=\"Tab 1\">\n<p>Panel 1</p>\n</div>\n<div class=\"flexi-tabs__tab-panel flexi-tabs__tab-panel_hidden alt-custom-class\" tabindex=\"0\" role=\"tabpanel\" aria-label=\"Tab 2\">\n<p>Panel 2</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTabsBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiTabsBlocks_Spec6(string extensions)
        {
            //     Start line number: 284
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     o{
            //         "attributes": {
            //             "id" : "my-custom-id",
            //             "class" : "my-custom-class"
            //         }
            //     }
            //     ///
            //     +++ tab
            //     Tab 1
            //     +++
            //     Panel 1
            //     +++
            //     ///
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-tabs my-custom-class" id="my-custom-id">
            //     <div class="flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal">
            //     <div class="flexi-tabs__tab-list scrollable-indicators__scrollable" role="tablist">
            //     <button class="flexi-tabs__tab flexi-tabs__tab_selected" title="View panel" role="tab" aria-selected="true">Tab 1</button>
            //     </div>
            //     <div class="scrollable-indicators__indicator scrollable-indicators__indicator_start"></div>
            //     <div class="scrollable-indicators__indicator scrollable-indicators__indicator_end"></div>
            //     </div>
            //     <div class="flexi-tabs__tab-panel" tabindex="0" role="tabpanel" aria-label="Tab 1">
            //     <p>Panel 1</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("o{\n    \"attributes\": {\n        \"id\" : \"my-custom-id\",\n        \"class\" : \"my-custom-class\"\n    }\n}\n///\n+++ tab\nTab 1\n+++\nPanel 1\n+++\n///",
                "<div class=\"flexi-tabs my-custom-class\" id=\"my-custom-id\">\n<div class=\"flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal\">\n<div class=\"flexi-tabs__tab-list scrollable-indicators__scrollable\" role=\"tablist\">\n<button class=\"flexi-tabs__tab flexi-tabs__tab_selected\" title=\"View panel\" role=\"tab\" aria-selected=\"true\">Tab 1</button>\n</div>\n<div class=\"scrollable-indicators__indicator scrollable-indicators__indicator_start\"></div>\n<div class=\"scrollable-indicators__indicator scrollable-indicators__indicator_end\"></div>\n</div>\n<div class=\"flexi-tabs__tab-panel\" tabindex=\"0\" role=\"tabpanel\" aria-label=\"Tab 1\">\n<p>Panel 1</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTabsBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiTabsBlocks_Spec7(string extensions)
        {
            //     Start line number: 329
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     ///
            //     o{
            //         "attributes": {
            //             "id" : "my-custom-id",
            //             "class" : "my-custom-class"
            //         }
            //     }
            //     +++ tab
            //     Tab 1
            //     +++
            //     Panel 1
            //     +++
            //     ///
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-tabs">
            //     <div class="flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal">
            //     <div class="flexi-tabs__tab-list scrollable-indicators__scrollable" role="tablist">
            //     <button class="flexi-tabs__tab flexi-tabs__tab_selected" title="View panel" role="tab" aria-selected="true">Tab 1</button>
            //     </div>
            //     <div class="scrollable-indicators__indicator scrollable-indicators__indicator_start"></div>
            //     <div class="scrollable-indicators__indicator scrollable-indicators__indicator_end"></div>
            //     </div>
            //     <div class="flexi-tabs__tab-panel my-custom-class" id="my-custom-id" tabindex="0" role="tabpanel" aria-label="Tab 1">
            //     <p>Panel 1</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("///\no{\n    \"attributes\": {\n        \"id\" : \"my-custom-id\",\n        \"class\" : \"my-custom-class\"\n    }\n}\n+++ tab\nTab 1\n+++\nPanel 1\n+++\n///",
                "<div class=\"flexi-tabs\">\n<div class=\"flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal\">\n<div class=\"flexi-tabs__tab-list scrollable-indicators__scrollable\" role=\"tablist\">\n<button class=\"flexi-tabs__tab flexi-tabs__tab_selected\" title=\"View panel\" role=\"tab\" aria-selected=\"true\">Tab 1</button>\n</div>\n<div class=\"scrollable-indicators__indicator scrollable-indicators__indicator_start\"></div>\n<div class=\"scrollable-indicators__indicator scrollable-indicators__indicator_end\"></div>\n</div>\n<div class=\"flexi-tabs__tab-panel my-custom-class\" id=\"my-custom-id\" tabindex=\"0\" role=\"tabpanel\" aria-label=\"Tab 1\">\n<p>Panel 1</p>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiTabsBlocks")]
        [InlineData("All")]
        public void FlexiTabsBlocks_Spec8(string extensions)
        {
            //     Start line number: 376
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiTabsBlocks": {
            //             "defaultBlockOptions": {
            //                 "defaultTabOptions": {
            //                   "attributes": {
            //                       "class" : "tab-class"
            //                   }
            //                 },
            //                 "attributes": {
            //                     "class": "tabs-class"
            //                 }
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     ///
            //     +++ tab
            //     Tab 1
            //     +++
            //     Panel 1
            //     +++
            //     ///
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-tabs tabs-class">
            //     <div class="flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal">
            //     <div class="flexi-tabs__tab-list scrollable-indicators__scrollable" role="tablist">
            //     <button class="flexi-tabs__tab flexi-tabs__tab_selected" title="View panel" role="tab" aria-selected="true">Tab 1</button>
            //     </div>
            //     <div class="scrollable-indicators__indicator scrollable-indicators__indicator_start"></div>
            //     <div class="scrollable-indicators__indicator scrollable-indicators__indicator_end"></div>
            //     </div>
            //     <div class="flexi-tabs__tab-panel tab-class" tabindex="0" role="tabpanel" aria-label="Tab 1">
            //     <p>Panel 1</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("///\n+++ tab\nTab 1\n+++\nPanel 1\n+++\n///",
                "<div class=\"flexi-tabs tabs-class\">\n<div class=\"flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal\">\n<div class=\"flexi-tabs__tab-list scrollable-indicators__scrollable\" role=\"tablist\">\n<button class=\"flexi-tabs__tab flexi-tabs__tab_selected\" title=\"View panel\" role=\"tab\" aria-selected=\"true\">Tab 1</button>\n</div>\n<div class=\"scrollable-indicators__indicator scrollable-indicators__indicator_start\"></div>\n<div class=\"scrollable-indicators__indicator scrollable-indicators__indicator_end\"></div>\n</div>\n<div class=\"flexi-tabs__tab-panel tab-class\" tabindex=\"0\" role=\"tabpanel\" aria-label=\"Tab 1\">\n<p>Panel 1</p>\n</div>\n</div>",
                extensions,
                false,
                "{\n    \"flexiTabsBlocks\": {\n        \"defaultBlockOptions\": {\n            \"defaultTabOptions\": {\n              \"attributes\": {\n                  \"class\" : \"tab-class\"\n              }\n            },\n            \"attributes\": {\n                \"class\": \"tabs-class\"\n            }\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiTabsBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiTabsBlocks_Spec9(string extensions)
        {
            //     Start line number: 415
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiTabsBlocks": {
            //             "defaultBlockOptions": {
            //                 "defaultTabOptions": {
            //                     "attributes": {
            //                         "class" : "tab-class"
            //                     }
            //                 },
            //                 "attributes": {
            //                     "class": "tabs-class"
            //                 }
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     ///
            //     o{              
            //         "attributes": {
            //             "class": "alt-tab-class"
            //         }
            //     }
            //     +++ tab
            //     Tab 1
            //     +++
            //     Panel 1
            //     +++
            //     ///
            //     
            //     o{              
            //         "attributes": {
            //             "class": "alt-tabs-class"
            //         }
            //     }
            //     ///
            //     +++ tab
            //     Tab 2
            //     +++
            //     Panel 2
            //     +++
            //     ///
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-tabs tabs-class">
            //     <div class="flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal">
            //     <div class="flexi-tabs__tab-list scrollable-indicators__scrollable" role="tablist">
            //     <button class="flexi-tabs__tab flexi-tabs__tab_selected" title="View panel" role="tab" aria-selected="true">Tab 1</button>
            //     </div>
            //     <div class="scrollable-indicators__indicator scrollable-indicators__indicator_start"></div>
            //     <div class="scrollable-indicators__indicator scrollable-indicators__indicator_end"></div>
            //     </div>
            //     <div class="flexi-tabs__tab-panel alt-tab-class" tabindex="0" role="tabpanel" aria-label="Tab 1">
            //     <p>Panel 1</p>
            //     </div>
            //     </div>
            //     <div class="flexi-tabs alt-tabs-class">
            //     <div class="flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal">
            //     <div class="flexi-tabs__tab-list scrollable-indicators__scrollable" role="tablist">
            //     <button class="flexi-tabs__tab flexi-tabs__tab_selected" title="View panel" role="tab" aria-selected="true">Tab 2</button>
            //     </div>
            //     <div class="scrollable-indicators__indicator scrollable-indicators__indicator_start"></div>
            //     <div class="scrollable-indicators__indicator scrollable-indicators__indicator_end"></div>
            //     </div>
            //     <div class="flexi-tabs__tab-panel tab-class" tabindex="0" role="tabpanel" aria-label="Tab 2">
            //     <p>Panel 2</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("///\no{              \n    \"attributes\": {\n        \"class\": \"alt-tab-class\"\n    }\n}\n+++ tab\nTab 1\n+++\nPanel 1\n+++\n///\n\no{              \n    \"attributes\": {\n        \"class\": \"alt-tabs-class\"\n    }\n}\n///\n+++ tab\nTab 2\n+++\nPanel 2\n+++\n///",
                "<div class=\"flexi-tabs tabs-class\">\n<div class=\"flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal\">\n<div class=\"flexi-tabs__tab-list scrollable-indicators__scrollable\" role=\"tablist\">\n<button class=\"flexi-tabs__tab flexi-tabs__tab_selected\" title=\"View panel\" role=\"tab\" aria-selected=\"true\">Tab 1</button>\n</div>\n<div class=\"scrollable-indicators__indicator scrollable-indicators__indicator_start\"></div>\n<div class=\"scrollable-indicators__indicator scrollable-indicators__indicator_end\"></div>\n</div>\n<div class=\"flexi-tabs__tab-panel alt-tab-class\" tabindex=\"0\" role=\"tabpanel\" aria-label=\"Tab 1\">\n<p>Panel 1</p>\n</div>\n</div>\n<div class=\"flexi-tabs alt-tabs-class\">\n<div class=\"flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal\">\n<div class=\"flexi-tabs__tab-list scrollable-indicators__scrollable\" role=\"tablist\">\n<button class=\"flexi-tabs__tab flexi-tabs__tab_selected\" title=\"View panel\" role=\"tab\" aria-selected=\"true\">Tab 2</button>\n</div>\n<div class=\"scrollable-indicators__indicator scrollable-indicators__indicator_start\"></div>\n<div class=\"scrollable-indicators__indicator scrollable-indicators__indicator_end\"></div>\n</div>\n<div class=\"flexi-tabs__tab-panel tab-class\" tabindex=\"0\" role=\"tabpanel\" aria-label=\"Tab 2\">\n<p>Panel 2</p>\n</div>\n</div>",
                extensions,
                false,
                "{\n    \"flexiTabsBlocks\": {\n        \"defaultBlockOptions\": {\n            \"defaultTabOptions\": {\n                \"attributes\": {\n                    \"class\" : \"tab-class\"\n                }\n            },\n            \"attributes\": {\n                \"class\": \"tabs-class\"\n            }\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiTabsBlocks")]
        [InlineData("All")]
        public void FlexiTabsBlocks_Spec10(string extensions)
        {
            //     Start line number: 491
            //     --------------- Markdown ---------------
            //     ///
            //     +++ tab
            //     Tab 1
            //     +++
            //     ////
            //     +++ tab
            //     Nested tab
            //     +++
            //     Nested panel
            //     +++
            //     ////
            //     +++
            //     ///
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-tabs">
            //     <div class="flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal">
            //     <div class="flexi-tabs__tab-list scrollable-indicators__scrollable" role="tablist">
            //     <button class="flexi-tabs__tab flexi-tabs__tab_selected" title="View panel" role="tab" aria-selected="true">Tab 1</button>
            //     </div>
            //     <div class="scrollable-indicators__indicator scrollable-indicators__indicator_start"></div>
            //     <div class="scrollable-indicators__indicator scrollable-indicators__indicator_end"></div>
            //     </div>
            //     <div class="flexi-tabs__tab-panel" tabindex="0" role="tabpanel" aria-label="Tab 1">
            //     <div class="flexi-tabs">
            //     <div class="flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal">
            //     <div class="flexi-tabs__tab-list scrollable-indicators__scrollable" role="tablist">
            //     <button class="flexi-tabs__tab flexi-tabs__tab_selected" title="View panel" role="tab" aria-selected="true">Nested tab</button>
            //     </div>
            //     <div class="scrollable-indicators__indicator scrollable-indicators__indicator_start"></div>
            //     <div class="scrollable-indicators__indicator scrollable-indicators__indicator_end"></div>
            //     </div>
            //     <div class="flexi-tabs__tab-panel" tabindex="0" role="tabpanel" aria-label="Nested tab">
            //     <p>Nested panel</p>
            //     </div>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("///\n+++ tab\nTab 1\n+++\n////\n+++ tab\nNested tab\n+++\nNested panel\n+++\n////\n+++\n///",
                "<div class=\"flexi-tabs\">\n<div class=\"flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal\">\n<div class=\"flexi-tabs__tab-list scrollable-indicators__scrollable\" role=\"tablist\">\n<button class=\"flexi-tabs__tab flexi-tabs__tab_selected\" title=\"View panel\" role=\"tab\" aria-selected=\"true\">Tab 1</button>\n</div>\n<div class=\"scrollable-indicators__indicator scrollable-indicators__indicator_start\"></div>\n<div class=\"scrollable-indicators__indicator scrollable-indicators__indicator_end\"></div>\n</div>\n<div class=\"flexi-tabs__tab-panel\" tabindex=\"0\" role=\"tabpanel\" aria-label=\"Tab 1\">\n<div class=\"flexi-tabs\">\n<div class=\"flexi-tabs__scrollable-indicators scrollable-indicators scrollable-indicators_axis_horizontal\">\n<div class=\"flexi-tabs__tab-list scrollable-indicators__scrollable\" role=\"tablist\">\n<button class=\"flexi-tabs__tab flexi-tabs__tab_selected\" title=\"View panel\" role=\"tab\" aria-selected=\"true\">Nested tab</button>\n</div>\n<div class=\"scrollable-indicators__indicator scrollable-indicators__indicator_start\"></div>\n<div class=\"scrollable-indicators__indicator scrollable-indicators__indicator_end\"></div>\n</div>\n<div class=\"flexi-tabs__tab-panel\" tabindex=\"0\" role=\"tabpanel\" aria-label=\"Nested tab\">\n<p>Nested panel</p>\n</div>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }
    }

    public class FlexiVideoBlocksSpecs
    {
        [Theory]
        [InlineData("FlexiVideoBlocks")]
        [InlineData("All")]
        public void FlexiVideoBlocks_Spec1(string extensions)
        {
            //     Start line number: 74
            //     --------------- Markdown ---------------
            //     v{ 
            //       "src": "/file.mp4"
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
            //     <div class="flexi-video__container" tabindex="-1">
            //     <div class="flexi-video__video-outer-container">
            //     <div class="flexi-video__video-inner-container">
            //     <video class="flexi-video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="flexi-video__controls">
            //     <button class="flexi-video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
            //     </button>
            //     <div class="flexi-video__elapsed-time">
            //     <span class="flexi-video__current-time">0:00</span>
            //     /<span class="flexi-video__duration">0:00</span>
            //     </div>
            //     <div class="flexi-video__progress">
            //     <div class="flexi-video__progress-track">
            //     <div class="flexi-video__progress-played"></div>
            //     <div class="flexi-video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="flexi-video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
            //     <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
            //     </button>
            //     </div>
            //     <div class="flexi-video__error-notice">
            //     <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-video__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("v{ \n  \"src\": \"/file.mp4\"\n}",
                "<div class=\"flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon\">\n<div class=\"flexi-video__container\" tabindex=\"-1\">\n<div class=\"flexi-video__video-outer-container\">\n<div class=\"flexi-video__video-inner-container\">\n<video class=\"flexi-video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"flexi-video__source\" data-src=\"/file.mp4\" type=\"video/mp4\">\n</video>\n</div>\n</div>\n<div class=\"flexi-video__controls\">\n<button class=\"flexi-video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"flexi-video__play-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M8 5v14l11-7z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n<svg class=\"flexi-video__pause-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M6 19h4V5H6v14zm8-14v14h4V5h-4z\"/></svg>\n</button>\n<div class=\"flexi-video__elapsed-time\">\n<span class=\"flexi-video__current-time\">0:00</span>\n/<span class=\"flexi-video__duration\">0:00</span>\n</div>\n<div class=\"flexi-video__progress\">\n<div class=\"flexi-video__progress-track\">\n<div class=\"flexi-video__progress-played\"></div>\n<div class=\"flexi-video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"flexi-video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"flexi-video__fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z\"/></svg>\n<svg class=\"flexi-video__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z\"/></svg>\n</button>\n</div>\n<div class=\"flexi-video__error-notice\">\n<svg class=\"flexi-video__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-video__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiVideoBlocks")]
        [InlineData("All")]
        public void FlexiVideoBlocks_Spec2(string extensions)
        {
            //     Start line number: 139
            //     --------------- Markdown ---------------
            //     v{ 
            //       "blockName": "video",
            //       "src": "/file.mp4"
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="video video_no-poster video_no-width video_no-aspect-ratio video_no-duration video_has-type video_has-spinner video_has-play-icon video_has-pause-icon video_has-fullscreen-icon video_has-exit-fullscreen-icon video_has-error-icon">
            //     <div class="video__container" tabindex="-1">
            //     <div class="video__video-outer-container">
            //     <div class="video__video-inner-container">
            //     <video class="video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="video__source" data-src="/file.mp4" type="video/mp4">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="video__controls">
            //     <button class="video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     <svg class="video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
            //     </button>
            //     <div class="video__elapsed-time">
            //     <span class="video__current-time">0:00</span>
            //     /<span class="video__duration">0:00</span>
            //     </div>
            //     <div class="video__progress">
            //     <div class="video__progress-track">
            //     <div class="video__progress-played"></div>
            //     <div class="video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
            //     <svg class="video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
            //     </button>
            //     </div>
            //     <div class="video__error-notice">
            //     <svg class="video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="video__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("v{ \n  \"blockName\": \"video\",\n  \"src\": \"/file.mp4\"\n}",
                "<div class=\"video video_no-poster video_no-width video_no-aspect-ratio video_no-duration video_has-type video_has-spinner video_has-play-icon video_has-pause-icon video_has-fullscreen-icon video_has-exit-fullscreen-icon video_has-error-icon\">\n<div class=\"video__container\" tabindex=\"-1\">\n<div class=\"video__video-outer-container\">\n<div class=\"video__video-inner-container\">\n<video class=\"video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"video__source\" data-src=\"/file.mp4\" type=\"video/mp4\">\n</video>\n</div>\n</div>\n<div class=\"video__controls\">\n<button class=\"video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"video__play-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M8 5v14l11-7z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n<svg class=\"video__pause-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M6 19h4V5H6v14zm8-14v14h4V5h-4z\"/></svg>\n</button>\n<div class=\"video__elapsed-time\">\n<span class=\"video__current-time\">0:00</span>\n/<span class=\"video__duration\">0:00</span>\n</div>\n<div class=\"video__progress\">\n<div class=\"video__progress-track\">\n<div class=\"video__progress-played\"></div>\n<div class=\"video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"video__fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z\"/></svg>\n<svg class=\"video__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z\"/></svg>\n</button>\n</div>\n<div class=\"video__error-notice\">\n<svg class=\"video__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"video__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiVideoBlocks")]
        [InlineData("All")]
        public void FlexiVideoBlocks_Spec3(string extensions)
        {
            //     Start line number: 199
            //     --------------- Markdown ---------------
            //     v{ 
            //       "src": "/file.mp4"
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
            //     <div class="flexi-video__container" tabindex="-1">
            //     <div class="flexi-video__video-outer-container">
            //     <div class="flexi-video__video-inner-container">
            //     <video class="flexi-video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="flexi-video__controls">
            //     <button class="flexi-video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
            //     </button>
            //     <div class="flexi-video__elapsed-time">
            //     <span class="flexi-video__current-time">0:00</span>
            //     /<span class="flexi-video__duration">0:00</span>
            //     </div>
            //     <div class="flexi-video__progress">
            //     <div class="flexi-video__progress-track">
            //     <div class="flexi-video__progress-played"></div>
            //     <div class="flexi-video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="flexi-video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
            //     <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
            //     </button>
            //     </div>
            //     <div class="flexi-video__error-notice">
            //     <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-video__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("v{ \n  \"src\": \"/file.mp4\"\n}",
                "<div class=\"flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon\">\n<div class=\"flexi-video__container\" tabindex=\"-1\">\n<div class=\"flexi-video__video-outer-container\">\n<div class=\"flexi-video__video-inner-container\">\n<video class=\"flexi-video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"flexi-video__source\" data-src=\"/file.mp4\" type=\"video/mp4\">\n</video>\n</div>\n</div>\n<div class=\"flexi-video__controls\">\n<button class=\"flexi-video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"flexi-video__play-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M8 5v14l11-7z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n<svg class=\"flexi-video__pause-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M6 19h4V5H6v14zm8-14v14h4V5h-4z\"/></svg>\n</button>\n<div class=\"flexi-video__elapsed-time\">\n<span class=\"flexi-video__current-time\">0:00</span>\n/<span class=\"flexi-video__duration\">0:00</span>\n</div>\n<div class=\"flexi-video__progress\">\n<div class=\"flexi-video__progress-track\">\n<div class=\"flexi-video__progress-played\"></div>\n<div class=\"flexi-video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"flexi-video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"flexi-video__fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z\"/></svg>\n<svg class=\"flexi-video__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z\"/></svg>\n</button>\n</div>\n<div class=\"flexi-video__error-notice\">\n<svg class=\"flexi-video__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-video__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiVideoBlocks")]
        [InlineData("All")]
        public void FlexiVideoBlocks_Spec4(string extensions)
        {
            //     Start line number: 258
            //     --------------- Markdown ---------------
            //     v{ 
            //       "src": "/file.mp4",
            //       "type": "custom/type"
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
            //     <div class="flexi-video__container" tabindex="-1">
            //     <div class="flexi-video__video-outer-container">
            //     <div class="flexi-video__video-inner-container">
            //     <video class="flexi-video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="flexi-video__source" data-src="/file.mp4" type="custom/type">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="flexi-video__controls">
            //     <button class="flexi-video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
            //     </button>
            //     <div class="flexi-video__elapsed-time">
            //     <span class="flexi-video__current-time">0:00</span>
            //     /<span class="flexi-video__duration">0:00</span>
            //     </div>
            //     <div class="flexi-video__progress">
            //     <div class="flexi-video__progress-track">
            //     <div class="flexi-video__progress-played"></div>
            //     <div class="flexi-video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="flexi-video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
            //     <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
            //     </button>
            //     </div>
            //     <div class="flexi-video__error-notice">
            //     <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-video__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("v{ \n  \"src\": \"/file.mp4\",\n  \"type\": \"custom/type\"\n}",
                "<div class=\"flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon\">\n<div class=\"flexi-video__container\" tabindex=\"-1\">\n<div class=\"flexi-video__video-outer-container\">\n<div class=\"flexi-video__video-inner-container\">\n<video class=\"flexi-video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"flexi-video__source\" data-src=\"/file.mp4\" type=\"custom/type\">\n</video>\n</div>\n</div>\n<div class=\"flexi-video__controls\">\n<button class=\"flexi-video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"flexi-video__play-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M8 5v14l11-7z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n<svg class=\"flexi-video__pause-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M6 19h4V5H6v14zm8-14v14h4V5h-4z\"/></svg>\n</button>\n<div class=\"flexi-video__elapsed-time\">\n<span class=\"flexi-video__current-time\">0:00</span>\n/<span class=\"flexi-video__duration\">0:00</span>\n</div>\n<div class=\"flexi-video__progress\">\n<div class=\"flexi-video__progress-track\">\n<div class=\"flexi-video__progress-played\"></div>\n<div class=\"flexi-video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"flexi-video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"flexi-video__fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z\"/></svg>\n<svg class=\"flexi-video__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z\"/></svg>\n</button>\n</div>\n<div class=\"flexi-video__error-notice\">\n<svg class=\"flexi-video__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-video__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiVideoBlocks")]
        [InlineData("All")]
        public void FlexiVideoBlocks_Spec5(string extensions)
        {
            //     Start line number: 320
            //     --------------- Markdown ---------------
            //     v{ 
            //       "src": "/file.mp4",
            //       "width": 123
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-video flexi-video_no-poster flexi-video_has-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
            //     <div class="flexi-video__container" tabindex="-1" style="width:123px">
            //     <div class="flexi-video__video-outer-container" style="width:123px">
            //     <div class="flexi-video__video-inner-container">
            //     <video class="flexi-video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="flexi-video__controls">
            //     <button class="flexi-video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
            //     </button>
            //     <div class="flexi-video__elapsed-time">
            //     <span class="flexi-video__current-time">0:00</span>
            //     /<span class="flexi-video__duration">0:00</span>
            //     </div>
            //     <div class="flexi-video__progress">
            //     <div class="flexi-video__progress-track">
            //     <div class="flexi-video__progress-played"></div>
            //     <div class="flexi-video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="flexi-video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
            //     <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
            //     </button>
            //     </div>
            //     <div class="flexi-video__error-notice">
            //     <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-video__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("v{ \n  \"src\": \"/file.mp4\",\n  \"width\": 123\n}",
                "<div class=\"flexi-video flexi-video_no-poster flexi-video_has-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon\">\n<div class=\"flexi-video__container\" tabindex=\"-1\" style=\"width:123px\">\n<div class=\"flexi-video__video-outer-container\" style=\"width:123px\">\n<div class=\"flexi-video__video-inner-container\">\n<video class=\"flexi-video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"flexi-video__source\" data-src=\"/file.mp4\" type=\"video/mp4\">\n</video>\n</div>\n</div>\n<div class=\"flexi-video__controls\">\n<button class=\"flexi-video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"flexi-video__play-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M8 5v14l11-7z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n<svg class=\"flexi-video__pause-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M6 19h4V5H6v14zm8-14v14h4V5h-4z\"/></svg>\n</button>\n<div class=\"flexi-video__elapsed-time\">\n<span class=\"flexi-video__current-time\">0:00</span>\n/<span class=\"flexi-video__duration\">0:00</span>\n</div>\n<div class=\"flexi-video__progress\">\n<div class=\"flexi-video__progress-track\">\n<div class=\"flexi-video__progress-played\"></div>\n<div class=\"flexi-video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"flexi-video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"flexi-video__fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z\"/></svg>\n<svg class=\"flexi-video__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z\"/></svg>\n</button>\n</div>\n<div class=\"flexi-video__error-notice\">\n<svg class=\"flexi-video__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-video__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiVideoBlocks")]
        [InlineData("All")]
        public void FlexiVideoBlocks_Spec6(string extensions)
        {
            //     Start line number: 382
            //     --------------- Markdown ---------------
            //     v{ 
            //       "src": "/file.mp4",
            //       "width": 123,
            //       "height": 321
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-video flexi-video_no-poster flexi-video_has-width flexi-video_has-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
            //     <div class="flexi-video__container" tabindex="-1" style="width:123px">
            //     <div class="flexi-video__video-outer-container" style="width:123px">
            //     <div class="flexi-video__video-inner-container" style="padding-bottom:260.975609756098%">
            //     <video class="flexi-video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="flexi-video__controls">
            //     <button class="flexi-video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
            //     </button>
            //     <div class="flexi-video__elapsed-time">
            //     <span class="flexi-video__current-time">0:00</span>
            //     /<span class="flexi-video__duration">0:00</span>
            //     </div>
            //     <div class="flexi-video__progress">
            //     <div class="flexi-video__progress-track">
            //     <div class="flexi-video__progress-played"></div>
            //     <div class="flexi-video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="flexi-video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
            //     <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
            //     </button>
            //     </div>
            //     <div class="flexi-video__error-notice">
            //     <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-video__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("v{ \n  \"src\": \"/file.mp4\",\n  \"width\": 123,\n  \"height\": 321\n}",
                "<div class=\"flexi-video flexi-video_no-poster flexi-video_has-width flexi-video_has-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon\">\n<div class=\"flexi-video__container\" tabindex=\"-1\" style=\"width:123px\">\n<div class=\"flexi-video__video-outer-container\" style=\"width:123px\">\n<div class=\"flexi-video__video-inner-container\" style=\"padding-bottom:260.975609756098%\">\n<video class=\"flexi-video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"flexi-video__source\" data-src=\"/file.mp4\" type=\"video/mp4\">\n</video>\n</div>\n</div>\n<div class=\"flexi-video__controls\">\n<button class=\"flexi-video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"flexi-video__play-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M8 5v14l11-7z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n<svg class=\"flexi-video__pause-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M6 19h4V5H6v14zm8-14v14h4V5h-4z\"/></svg>\n</button>\n<div class=\"flexi-video__elapsed-time\">\n<span class=\"flexi-video__current-time\">0:00</span>\n/<span class=\"flexi-video__duration\">0:00</span>\n</div>\n<div class=\"flexi-video__progress\">\n<div class=\"flexi-video__progress-track\">\n<div class=\"flexi-video__progress-played\"></div>\n<div class=\"flexi-video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"flexi-video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"flexi-video__fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z\"/></svg>\n<svg class=\"flexi-video__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z\"/></svg>\n</button>\n</div>\n<div class=\"flexi-video__error-notice\">\n<svg class=\"flexi-video__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-video__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiVideoBlocks")]
        [InlineData("All")]
        public void FlexiVideoBlocks_Spec7(string extensions)
        {
            //     Start line number: 433
            //     --------------- Markdown ---------------
            //     v{ 
            //       "src": "/file.mp4",
            //       "width": 0,
            //       "height": 321
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
            //     <div class="flexi-video__container" tabindex="-1">
            //     <div class="flexi-video__video-outer-container">
            //     <div class="flexi-video__video-inner-container">
            //     <video class="flexi-video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="flexi-video__controls">
            //     <button class="flexi-video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
            //     </button>
            //     <div class="flexi-video__elapsed-time">
            //     <span class="flexi-video__current-time">0:00</span>
            //     /<span class="flexi-video__duration">0:00</span>
            //     </div>
            //     <div class="flexi-video__progress">
            //     <div class="flexi-video__progress-track">
            //     <div class="flexi-video__progress-played"></div>
            //     <div class="flexi-video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="flexi-video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
            //     <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
            //     </button>
            //     </div>
            //     <div class="flexi-video__error-notice">
            //     <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-video__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("v{ \n  \"src\": \"/file.mp4\",\n  \"width\": 0,\n  \"height\": 321\n}",
                "<div class=\"flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon\">\n<div class=\"flexi-video__container\" tabindex=\"-1\">\n<div class=\"flexi-video__video-outer-container\">\n<div class=\"flexi-video__video-inner-container\">\n<video class=\"flexi-video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"flexi-video__source\" data-src=\"/file.mp4\" type=\"video/mp4\">\n</video>\n</div>\n</div>\n<div class=\"flexi-video__controls\">\n<button class=\"flexi-video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"flexi-video__play-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M8 5v14l11-7z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n<svg class=\"flexi-video__pause-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M6 19h4V5H6v14zm8-14v14h4V5h-4z\"/></svg>\n</button>\n<div class=\"flexi-video__elapsed-time\">\n<span class=\"flexi-video__current-time\">0:00</span>\n/<span class=\"flexi-video__duration\">0:00</span>\n</div>\n<div class=\"flexi-video__progress\">\n<div class=\"flexi-video__progress-track\">\n<div class=\"flexi-video__progress-played\"></div>\n<div class=\"flexi-video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"flexi-video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"flexi-video__fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z\"/></svg>\n<svg class=\"flexi-video__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z\"/></svg>\n</button>\n</div>\n<div class=\"flexi-video__error-notice\">\n<svg class=\"flexi-video__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-video__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiVideoBlocks")]
        [InlineData("All")]
        public void FlexiVideoBlocks_Spec8(string extensions)
        {
            //     Start line number: 494
            //     --------------- Markdown ---------------
            //     v{ 
            //       "src": "/file.mp4",
            //       "duration": 123.456
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_has-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
            //     <div class="flexi-video__container" tabindex="-1">
            //     <div class="flexi-video__video-outer-container">
            //     <div class="flexi-video__video-inner-container">
            //     <video class="flexi-video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="flexi-video__controls">
            //     <button class="flexi-video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
            //     </button>
            //     <div class="flexi-video__elapsed-time">
            //     <span class="flexi-video__current-time">0:00</span>
            //     /<span class="flexi-video__duration">2:03</span>
            //     </div>
            //     <div class="flexi-video__progress">
            //     <div class="flexi-video__progress-track">
            //     <div class="flexi-video__progress-played"></div>
            //     <div class="flexi-video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="flexi-video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
            //     <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
            //     </button>
            //     </div>
            //     <div class="flexi-video__error-notice">
            //     <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-video__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("v{ \n  \"src\": \"/file.mp4\",\n  \"duration\": 123.456\n}",
                "<div class=\"flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_has-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon\">\n<div class=\"flexi-video__container\" tabindex=\"-1\">\n<div class=\"flexi-video__video-outer-container\">\n<div class=\"flexi-video__video-inner-container\">\n<video class=\"flexi-video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"flexi-video__source\" data-src=\"/file.mp4\" type=\"video/mp4\">\n</video>\n</div>\n</div>\n<div class=\"flexi-video__controls\">\n<button class=\"flexi-video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"flexi-video__play-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M8 5v14l11-7z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n<svg class=\"flexi-video__pause-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M6 19h4V5H6v14zm8-14v14h4V5h-4z\"/></svg>\n</button>\n<div class=\"flexi-video__elapsed-time\">\n<span class=\"flexi-video__current-time\">0:00</span>\n/<span class=\"flexi-video__duration\">2:03</span>\n</div>\n<div class=\"flexi-video__progress\">\n<div class=\"flexi-video__progress-track\">\n<div class=\"flexi-video__progress-played\"></div>\n<div class=\"flexi-video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"flexi-video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"flexi-video__fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z\"/></svg>\n<svg class=\"flexi-video__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z\"/></svg>\n</button>\n</div>\n<div class=\"flexi-video__error-notice\">\n<svg class=\"flexi-video__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-video__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiVideoBlocks")]
        [InlineData("All")]
        public void FlexiVideoBlocks_Spec9(string extensions)
        {
            //     Start line number: 544
            //     --------------- Markdown ---------------
            //     v{ 
            //       "src": "/file.mp4",
            //       "duration": 0
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
            //     <div class="flexi-video__container" tabindex="-1">
            //     <div class="flexi-video__video-outer-container">
            //     <div class="flexi-video__video-inner-container">
            //     <video class="flexi-video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="flexi-video__controls">
            //     <button class="flexi-video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
            //     </button>
            //     <div class="flexi-video__elapsed-time">
            //     <span class="flexi-video__current-time">0:00</span>
            //     /<span class="flexi-video__duration">0:00</span>
            //     </div>
            //     <div class="flexi-video__progress">
            //     <div class="flexi-video__progress-track">
            //     <div class="flexi-video__progress-played"></div>
            //     <div class="flexi-video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="flexi-video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
            //     <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
            //     </button>
            //     </div>
            //     <div class="flexi-video__error-notice">
            //     <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-video__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("v{ \n  \"src\": \"/file.mp4\",\n  \"duration\": 0\n}",
                "<div class=\"flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon\">\n<div class=\"flexi-video__container\" tabindex=\"-1\">\n<div class=\"flexi-video__video-outer-container\">\n<div class=\"flexi-video__video-inner-container\">\n<video class=\"flexi-video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"flexi-video__source\" data-src=\"/file.mp4\" type=\"video/mp4\">\n</video>\n</div>\n</div>\n<div class=\"flexi-video__controls\">\n<button class=\"flexi-video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"flexi-video__play-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M8 5v14l11-7z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n<svg class=\"flexi-video__pause-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M6 19h4V5H6v14zm8-14v14h4V5h-4z\"/></svg>\n</button>\n<div class=\"flexi-video__elapsed-time\">\n<span class=\"flexi-video__current-time\">0:00</span>\n/<span class=\"flexi-video__duration\">0:00</span>\n</div>\n<div class=\"flexi-video__progress\">\n<div class=\"flexi-video__progress-track\">\n<div class=\"flexi-video__progress-played\"></div>\n<div class=\"flexi-video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"flexi-video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"flexi-video__fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z\"/></svg>\n<svg class=\"flexi-video__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z\"/></svg>\n</button>\n</div>\n<div class=\"flexi-video__error-notice\">\n<svg class=\"flexi-video__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-video__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiVideoBlocks")]
        [InlineData("All")]
        public void FlexiVideoBlocks_Spec10(string extensions)
        {
            //     Start line number: 615
            //     --------------- Markdown ---------------
            //     v{ 
            //       "src": "/file.mp4",
            //       "poster": "/file_poster.png"
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-video flexi-video_has-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
            //     <div class="flexi-video__container" tabindex="-1">
            //     <div class="flexi-video__video-outer-container">
            //     <div class="flexi-video__video-inner-container">
            //     <video class="flexi-video__video" preload="none" poster="/file_poster.png" muted playsInline disablePictureInPicture loop>
            //     <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="flexi-video__controls">
            //     <button class="flexi-video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
            //     </button>
            //     <div class="flexi-video__elapsed-time">
            //     <span class="flexi-video__current-time">0:00</span>
            //     /<span class="flexi-video__duration">0:00</span>
            //     </div>
            //     <div class="flexi-video__progress">
            //     <div class="flexi-video__progress-track">
            //     <div class="flexi-video__progress-played"></div>
            //     <div class="flexi-video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="flexi-video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
            //     <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
            //     </button>
            //     </div>
            //     <div class="flexi-video__error-notice">
            //     <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-video__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("v{ \n  \"src\": \"/file.mp4\",\n  \"poster\": \"/file_poster.png\"\n}",
                "<div class=\"flexi-video flexi-video_has-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon\">\n<div class=\"flexi-video__container\" tabindex=\"-1\">\n<div class=\"flexi-video__video-outer-container\">\n<div class=\"flexi-video__video-inner-container\">\n<video class=\"flexi-video__video\" preload=\"none\" poster=\"/file_poster.png\" muted playsInline disablePictureInPicture loop>\n<source class=\"flexi-video__source\" data-src=\"/file.mp4\" type=\"video/mp4\">\n</video>\n</div>\n</div>\n<div class=\"flexi-video__controls\">\n<button class=\"flexi-video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"flexi-video__play-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M8 5v14l11-7z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n<svg class=\"flexi-video__pause-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M6 19h4V5H6v14zm8-14v14h4V5h-4z\"/></svg>\n</button>\n<div class=\"flexi-video__elapsed-time\">\n<span class=\"flexi-video__current-time\">0:00</span>\n/<span class=\"flexi-video__duration\">0:00</span>\n</div>\n<div class=\"flexi-video__progress\">\n<div class=\"flexi-video__progress-track\">\n<div class=\"flexi-video__progress-played\"></div>\n<div class=\"flexi-video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"flexi-video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"flexi-video__fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z\"/></svg>\n<svg class=\"flexi-video__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z\"/></svg>\n</button>\n</div>\n<div class=\"flexi-video__error-notice\">\n<svg class=\"flexi-video__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-video__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiVideoBlocks")]
        [InlineData("All")]
        public void FlexiVideoBlocks_Spec11(string extensions)
        {
            //     Start line number: 672
            //     --------------- Markdown ---------------
            //     v{ 
            //       "src": "/file.mp4",
            //       "spinner": "<div class=\"spinner\"></div>"
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
            //     <div class="flexi-video__container" tabindex="-1">
            //     <div class="flexi-video__video-outer-container">
            //     <div class="flexi-video__video-inner-container">
            //     <video class="flexi-video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="flexi-video__controls">
            //     <button class="flexi-video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
            //     </button>
            //     <div class="flexi-video__elapsed-time">
            //     <span class="flexi-video__current-time">0:00</span>
            //     /<span class="flexi-video__duration">0:00</span>
            //     </div>
            //     <div class="flexi-video__progress">
            //     <div class="flexi-video__progress-track">
            //     <div class="flexi-video__progress-played"></div>
            //     <div class="flexi-video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="flexi-video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
            //     <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
            //     </button>
            //     </div>
            //     <div class="flexi-video__error-notice">
            //     <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-video__spinner spinner"></div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("v{ \n  \"src\": \"/file.mp4\",\n  \"spinner\": \"<div class=\\\"spinner\\\"></div>\"\n}",
                "<div class=\"flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon\">\n<div class=\"flexi-video__container\" tabindex=\"-1\">\n<div class=\"flexi-video__video-outer-container\">\n<div class=\"flexi-video__video-inner-container\">\n<video class=\"flexi-video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"flexi-video__source\" data-src=\"/file.mp4\" type=\"video/mp4\">\n</video>\n</div>\n</div>\n<div class=\"flexi-video__controls\">\n<button class=\"flexi-video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"flexi-video__play-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M8 5v14l11-7z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n<svg class=\"flexi-video__pause-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M6 19h4V5H6v14zm8-14v14h4V5h-4z\"/></svg>\n</button>\n<div class=\"flexi-video__elapsed-time\">\n<span class=\"flexi-video__current-time\">0:00</span>\n/<span class=\"flexi-video__duration\">0:00</span>\n</div>\n<div class=\"flexi-video__progress\">\n<div class=\"flexi-video__progress-track\">\n<div class=\"flexi-video__progress-played\"></div>\n<div class=\"flexi-video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"flexi-video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"flexi-video__fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z\"/></svg>\n<svg class=\"flexi-video__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z\"/></svg>\n</button>\n</div>\n<div class=\"flexi-video__error-notice\">\n<svg class=\"flexi-video__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-video__spinner spinner\"></div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiVideoBlocks")]
        [InlineData("All")]
        public void FlexiVideoBlocks_Spec12(string extensions)
        {
            //     Start line number: 716
            //     --------------- Markdown ---------------
            //     v{ 
            //       "src": "/file.mp4",
            //       "spinner": null
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_no-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
            //     <div class="flexi-video__container" tabindex="-1">
            //     <div class="flexi-video__video-outer-container">
            //     <div class="flexi-video__video-inner-container">
            //     <video class="flexi-video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="flexi-video__controls">
            //     <button class="flexi-video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
            //     </button>
            //     <div class="flexi-video__elapsed-time">
            //     <span class="flexi-video__current-time">0:00</span>
            //     /<span class="flexi-video__duration">0:00</span>
            //     </div>
            //     <div class="flexi-video__progress">
            //     <div class="flexi-video__progress-track">
            //     <div class="flexi-video__progress-played"></div>
            //     <div class="flexi-video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="flexi-video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
            //     <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
            //     </button>
            //     </div>
            //     <div class="flexi-video__error-notice">
            //     <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("v{ \n  \"src\": \"/file.mp4\",\n  \"spinner\": null\n}",
                "<div class=\"flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_no-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon\">\n<div class=\"flexi-video__container\" tabindex=\"-1\">\n<div class=\"flexi-video__video-outer-container\">\n<div class=\"flexi-video__video-inner-container\">\n<video class=\"flexi-video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"flexi-video__source\" data-src=\"/file.mp4\" type=\"video/mp4\">\n</video>\n</div>\n</div>\n<div class=\"flexi-video__controls\">\n<button class=\"flexi-video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"flexi-video__play-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M8 5v14l11-7z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n<svg class=\"flexi-video__pause-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M6 19h4V5H6v14zm8-14v14h4V5h-4z\"/></svg>\n</button>\n<div class=\"flexi-video__elapsed-time\">\n<span class=\"flexi-video__current-time\">0:00</span>\n/<span class=\"flexi-video__duration\">0:00</span>\n</div>\n<div class=\"flexi-video__progress\">\n<div class=\"flexi-video__progress-track\">\n<div class=\"flexi-video__progress-played\"></div>\n<div class=\"flexi-video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"flexi-video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"flexi-video__fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z\"/></svg>\n<svg class=\"flexi-video__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z\"/></svg>\n</button>\n</div>\n<div class=\"flexi-video__error-notice\">\n<svg class=\"flexi-video__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiVideoBlocks")]
        [InlineData("All")]
        public void FlexiVideoBlocks_Spec13(string extensions)
        {
            //     Start line number: 766
            //     --------------- Markdown ---------------
            //     v{ 
            //       "src": "/file.mp4",
            //       "playIcon": "<svg><use xlink:href=\"#play-icon\"/></svg>"
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
            //     <div class="flexi-video__container" tabindex="-1">
            //     <div class="flexi-video__video-outer-container">
            //     <div class="flexi-video__video-inner-container">
            //     <video class="flexi-video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="flexi-video__controls">
            //     <button class="flexi-video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="flexi-video__play-icon"><use xlink:href="#play-icon"/></svg>
            //     <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
            //     </button>
            //     <div class="flexi-video__elapsed-time">
            //     <span class="flexi-video__current-time">0:00</span>
            //     /<span class="flexi-video__duration">0:00</span>
            //     </div>
            //     <div class="flexi-video__progress">
            //     <div class="flexi-video__progress-track">
            //     <div class="flexi-video__progress-played"></div>
            //     <div class="flexi-video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="flexi-video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
            //     <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
            //     </button>
            //     </div>
            //     <div class="flexi-video__error-notice">
            //     <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-video__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("v{ \n  \"src\": \"/file.mp4\",\n  \"playIcon\": \"<svg><use xlink:href=\\\"#play-icon\\\"/></svg>\"\n}",
                "<div class=\"flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon\">\n<div class=\"flexi-video__container\" tabindex=\"-1\">\n<div class=\"flexi-video__video-outer-container\">\n<div class=\"flexi-video__video-inner-container\">\n<video class=\"flexi-video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"flexi-video__source\" data-src=\"/file.mp4\" type=\"video/mp4\">\n</video>\n</div>\n</div>\n<div class=\"flexi-video__controls\">\n<button class=\"flexi-video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"flexi-video__play-icon\"><use xlink:href=\"#play-icon\"/></svg>\n<svg class=\"flexi-video__pause-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M6 19h4V5H6v14zm8-14v14h4V5h-4z\"/></svg>\n</button>\n<div class=\"flexi-video__elapsed-time\">\n<span class=\"flexi-video__current-time\">0:00</span>\n/<span class=\"flexi-video__duration\">0:00</span>\n</div>\n<div class=\"flexi-video__progress\">\n<div class=\"flexi-video__progress-track\">\n<div class=\"flexi-video__progress-played\"></div>\n<div class=\"flexi-video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"flexi-video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"flexi-video__fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z\"/></svg>\n<svg class=\"flexi-video__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z\"/></svg>\n</button>\n</div>\n<div class=\"flexi-video__error-notice\">\n<svg class=\"flexi-video__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-video__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiVideoBlocks")]
        [InlineData("All")]
        public void FlexiVideoBlocks_Spec14(string extensions)
        {
            //     Start line number: 816
            //     --------------- Markdown ---------------
            //     v{ 
            //       "src": "/file.mp4",
            //       "playIcon": null
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_no-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
            //     <div class="flexi-video__container" tabindex="-1">
            //     <div class="flexi-video__video-outer-container">
            //     <div class="flexi-video__video-inner-container">
            //     <video class="flexi-video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="flexi-video__controls">
            //     <button class="flexi-video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
            //     </button>
            //     <div class="flexi-video__elapsed-time">
            //     <span class="flexi-video__current-time">0:00</span>
            //     /<span class="flexi-video__duration">0:00</span>
            //     </div>
            //     <div class="flexi-video__progress">
            //     <div class="flexi-video__progress-track">
            //     <div class="flexi-video__progress-played"></div>
            //     <div class="flexi-video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="flexi-video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
            //     <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
            //     </button>
            //     </div>
            //     <div class="flexi-video__error-notice">
            //     <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-video__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("v{ \n  \"src\": \"/file.mp4\",\n  \"playIcon\": null\n}",
                "<div class=\"flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_no-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon\">\n<div class=\"flexi-video__container\" tabindex=\"-1\">\n<div class=\"flexi-video__video-outer-container\">\n<div class=\"flexi-video__video-inner-container\">\n<video class=\"flexi-video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"flexi-video__source\" data-src=\"/file.mp4\" type=\"video/mp4\">\n</video>\n</div>\n</div>\n<div class=\"flexi-video__controls\">\n<button class=\"flexi-video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"flexi-video__pause-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M6 19h4V5H6v14zm8-14v14h4V5h-4z\"/></svg>\n</button>\n<div class=\"flexi-video__elapsed-time\">\n<span class=\"flexi-video__current-time\">0:00</span>\n/<span class=\"flexi-video__duration\">0:00</span>\n</div>\n<div class=\"flexi-video__progress\">\n<div class=\"flexi-video__progress-track\">\n<div class=\"flexi-video__progress-played\"></div>\n<div class=\"flexi-video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"flexi-video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"flexi-video__fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z\"/></svg>\n<svg class=\"flexi-video__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z\"/></svg>\n</button>\n</div>\n<div class=\"flexi-video__error-notice\">\n<svg class=\"flexi-video__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-video__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiVideoBlocks")]
        [InlineData("All")]
        public void FlexiVideoBlocks_Spec15(string extensions)
        {
            //     Start line number: 872
            //     --------------- Markdown ---------------
            //     v{ 
            //       "src": "/file.mp4",
            //       "pauseIcon": "<svg><use xlink:href=\"#pause-icon\"/></svg>"
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
            //     <div class="flexi-video__container" tabindex="-1">
            //     <div class="flexi-video__video-outer-container">
            //     <div class="flexi-video__video-inner-container">
            //     <video class="flexi-video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="flexi-video__controls">
            //     <button class="flexi-video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     <svg class="flexi-video__pause-icon"><use xlink:href="#pause-icon"/></svg>
            //     </button>
            //     <div class="flexi-video__elapsed-time">
            //     <span class="flexi-video__current-time">0:00</span>
            //     /<span class="flexi-video__duration">0:00</span>
            //     </div>
            //     <div class="flexi-video__progress">
            //     <div class="flexi-video__progress-track">
            //     <div class="flexi-video__progress-played"></div>
            //     <div class="flexi-video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="flexi-video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
            //     <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
            //     </button>
            //     </div>
            //     <div class="flexi-video__error-notice">
            //     <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-video__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("v{ \n  \"src\": \"/file.mp4\",\n  \"pauseIcon\": \"<svg><use xlink:href=\\\"#pause-icon\\\"/></svg>\"\n}",
                "<div class=\"flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon\">\n<div class=\"flexi-video__container\" tabindex=\"-1\">\n<div class=\"flexi-video__video-outer-container\">\n<div class=\"flexi-video__video-inner-container\">\n<video class=\"flexi-video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"flexi-video__source\" data-src=\"/file.mp4\" type=\"video/mp4\">\n</video>\n</div>\n</div>\n<div class=\"flexi-video__controls\">\n<button class=\"flexi-video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"flexi-video__play-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M8 5v14l11-7z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n<svg class=\"flexi-video__pause-icon\"><use xlink:href=\"#pause-icon\"/></svg>\n</button>\n<div class=\"flexi-video__elapsed-time\">\n<span class=\"flexi-video__current-time\">0:00</span>\n/<span class=\"flexi-video__duration\">0:00</span>\n</div>\n<div class=\"flexi-video__progress\">\n<div class=\"flexi-video__progress-track\">\n<div class=\"flexi-video__progress-played\"></div>\n<div class=\"flexi-video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"flexi-video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"flexi-video__fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z\"/></svg>\n<svg class=\"flexi-video__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z\"/></svg>\n</button>\n</div>\n<div class=\"flexi-video__error-notice\">\n<svg class=\"flexi-video__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-video__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiVideoBlocks")]
        [InlineData("All")]
        public void FlexiVideoBlocks_Spec16(string extensions)
        {
            //     Start line number: 922
            //     --------------- Markdown ---------------
            //     v{ 
            //       "src": "/file.mp4",
            //       "pauseIcon": null
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_no-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
            //     <div class="flexi-video__container" tabindex="-1">
            //     <div class="flexi-video__video-outer-container">
            //     <div class="flexi-video__video-inner-container">
            //     <video class="flexi-video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="flexi-video__controls">
            //     <button class="flexi-video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     </button>
            //     <div class="flexi-video__elapsed-time">
            //     <span class="flexi-video__current-time">0:00</span>
            //     /<span class="flexi-video__duration">0:00</span>
            //     </div>
            //     <div class="flexi-video__progress">
            //     <div class="flexi-video__progress-track">
            //     <div class="flexi-video__progress-played"></div>
            //     <div class="flexi-video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="flexi-video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
            //     <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
            //     </button>
            //     </div>
            //     <div class="flexi-video__error-notice">
            //     <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-video__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("v{ \n  \"src\": \"/file.mp4\",\n  \"pauseIcon\": null\n}",
                "<div class=\"flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_no-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon\">\n<div class=\"flexi-video__container\" tabindex=\"-1\">\n<div class=\"flexi-video__video-outer-container\">\n<div class=\"flexi-video__video-inner-container\">\n<video class=\"flexi-video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"flexi-video__source\" data-src=\"/file.mp4\" type=\"video/mp4\">\n</video>\n</div>\n</div>\n<div class=\"flexi-video__controls\">\n<button class=\"flexi-video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"flexi-video__play-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M8 5v14l11-7z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n</button>\n<div class=\"flexi-video__elapsed-time\">\n<span class=\"flexi-video__current-time\">0:00</span>\n/<span class=\"flexi-video__duration\">0:00</span>\n</div>\n<div class=\"flexi-video__progress\">\n<div class=\"flexi-video__progress-track\">\n<div class=\"flexi-video__progress-played\"></div>\n<div class=\"flexi-video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"flexi-video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"flexi-video__fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z\"/></svg>\n<svg class=\"flexi-video__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z\"/></svg>\n</button>\n</div>\n<div class=\"flexi-video__error-notice\">\n<svg class=\"flexi-video__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-video__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiVideoBlocks")]
        [InlineData("All")]
        public void FlexiVideoBlocks_Spec17(string extensions)
        {
            //     Start line number: 978
            //     --------------- Markdown ---------------
            //     v{ 
            //       "src": "/file.mp4",
            //       "fullscreenIcon": "<svg><use xlink:href=\"#fullscreen-icon\"/></svg>"
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
            //     <div class="flexi-video__container" tabindex="-1">
            //     <div class="flexi-video__video-outer-container">
            //     <div class="flexi-video__video-inner-container">
            //     <video class="flexi-video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="flexi-video__controls">
            //     <button class="flexi-video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
            //     </button>
            //     <div class="flexi-video__elapsed-time">
            //     <span class="flexi-video__current-time">0:00</span>
            //     /<span class="flexi-video__duration">0:00</span>
            //     </div>
            //     <div class="flexi-video__progress">
            //     <div class="flexi-video__progress-track">
            //     <div class="flexi-video__progress-played"></div>
            //     <div class="flexi-video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="flexi-video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="flexi-video__fullscreen-icon"><use xlink:href="#fullscreen-icon"/></svg>
            //     <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
            //     </button>
            //     </div>
            //     <div class="flexi-video__error-notice">
            //     <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-video__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("v{ \n  \"src\": \"/file.mp4\",\n  \"fullscreenIcon\": \"<svg><use xlink:href=\\\"#fullscreen-icon\\\"/></svg>\"\n}",
                "<div class=\"flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon\">\n<div class=\"flexi-video__container\" tabindex=\"-1\">\n<div class=\"flexi-video__video-outer-container\">\n<div class=\"flexi-video__video-inner-container\">\n<video class=\"flexi-video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"flexi-video__source\" data-src=\"/file.mp4\" type=\"video/mp4\">\n</video>\n</div>\n</div>\n<div class=\"flexi-video__controls\">\n<button class=\"flexi-video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"flexi-video__play-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M8 5v14l11-7z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n<svg class=\"flexi-video__pause-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M6 19h4V5H6v14zm8-14v14h4V5h-4z\"/></svg>\n</button>\n<div class=\"flexi-video__elapsed-time\">\n<span class=\"flexi-video__current-time\">0:00</span>\n/<span class=\"flexi-video__duration\">0:00</span>\n</div>\n<div class=\"flexi-video__progress\">\n<div class=\"flexi-video__progress-track\">\n<div class=\"flexi-video__progress-played\"></div>\n<div class=\"flexi-video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"flexi-video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"flexi-video__fullscreen-icon\"><use xlink:href=\"#fullscreen-icon\"/></svg>\n<svg class=\"flexi-video__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z\"/></svg>\n</button>\n</div>\n<div class=\"flexi-video__error-notice\">\n<svg class=\"flexi-video__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-video__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiVideoBlocks")]
        [InlineData("All")]
        public void FlexiVideoBlocks_Spec18(string extensions)
        {
            //     Start line number: 1028
            //     --------------- Markdown ---------------
            //     v{ 
            //       "src": "/file.mp4",
            //       "fullscreenIcon": null
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_no-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
            //     <div class="flexi-video__container" tabindex="-1">
            //     <div class="flexi-video__video-outer-container">
            //     <div class="flexi-video__video-inner-container">
            //     <video class="flexi-video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="flexi-video__controls">
            //     <button class="flexi-video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
            //     </button>
            //     <div class="flexi-video__elapsed-time">
            //     <span class="flexi-video__current-time">0:00</span>
            //     /<span class="flexi-video__duration">0:00</span>
            //     </div>
            //     <div class="flexi-video__progress">
            //     <div class="flexi-video__progress-track">
            //     <div class="flexi-video__progress-played"></div>
            //     <div class="flexi-video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="flexi-video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
            //     </button>
            //     </div>
            //     <div class="flexi-video__error-notice">
            //     <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-video__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("v{ \n  \"src\": \"/file.mp4\",\n  \"fullscreenIcon\": null\n}",
                "<div class=\"flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_no-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon\">\n<div class=\"flexi-video__container\" tabindex=\"-1\">\n<div class=\"flexi-video__video-outer-container\">\n<div class=\"flexi-video__video-inner-container\">\n<video class=\"flexi-video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"flexi-video__source\" data-src=\"/file.mp4\" type=\"video/mp4\">\n</video>\n</div>\n</div>\n<div class=\"flexi-video__controls\">\n<button class=\"flexi-video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"flexi-video__play-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M8 5v14l11-7z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n<svg class=\"flexi-video__pause-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M6 19h4V5H6v14zm8-14v14h4V5h-4z\"/></svg>\n</button>\n<div class=\"flexi-video__elapsed-time\">\n<span class=\"flexi-video__current-time\">0:00</span>\n/<span class=\"flexi-video__duration\">0:00</span>\n</div>\n<div class=\"flexi-video__progress\">\n<div class=\"flexi-video__progress-track\">\n<div class=\"flexi-video__progress-played\"></div>\n<div class=\"flexi-video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"flexi-video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"flexi-video__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z\"/></svg>\n</button>\n</div>\n<div class=\"flexi-video__error-notice\">\n<svg class=\"flexi-video__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-video__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiVideoBlocks")]
        [InlineData("All")]
        public void FlexiVideoBlocks_Spec19(string extensions)
        {
            //     Start line number: 1084
            //     --------------- Markdown ---------------
            //     v{ 
            //       "src": "/file.mp4",
            //       "exitFullscreenIcon": "<svg><use xlink:href=\"#exit-fullscreen-icon\"/></svg>"
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
            //     <div class="flexi-video__container" tabindex="-1">
            //     <div class="flexi-video__video-outer-container">
            //     <div class="flexi-video__video-inner-container">
            //     <video class="flexi-video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="flexi-video__controls">
            //     <button class="flexi-video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
            //     </button>
            //     <div class="flexi-video__elapsed-time">
            //     <span class="flexi-video__current-time">0:00</span>
            //     /<span class="flexi-video__duration">0:00</span>
            //     </div>
            //     <div class="flexi-video__progress">
            //     <div class="flexi-video__progress-track">
            //     <div class="flexi-video__progress-played"></div>
            //     <div class="flexi-video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="flexi-video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
            //     <svg class="flexi-video__exit-fullscreen-icon"><use xlink:href="#exit-fullscreen-icon"/></svg>
            //     </button>
            //     </div>
            //     <div class="flexi-video__error-notice">
            //     <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-video__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("v{ \n  \"src\": \"/file.mp4\",\n  \"exitFullscreenIcon\": \"<svg><use xlink:href=\\\"#exit-fullscreen-icon\\\"/></svg>\"\n}",
                "<div class=\"flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon\">\n<div class=\"flexi-video__container\" tabindex=\"-1\">\n<div class=\"flexi-video__video-outer-container\">\n<div class=\"flexi-video__video-inner-container\">\n<video class=\"flexi-video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"flexi-video__source\" data-src=\"/file.mp4\" type=\"video/mp4\">\n</video>\n</div>\n</div>\n<div class=\"flexi-video__controls\">\n<button class=\"flexi-video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"flexi-video__play-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M8 5v14l11-7z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n<svg class=\"flexi-video__pause-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M6 19h4V5H6v14zm8-14v14h4V5h-4z\"/></svg>\n</button>\n<div class=\"flexi-video__elapsed-time\">\n<span class=\"flexi-video__current-time\">0:00</span>\n/<span class=\"flexi-video__duration\">0:00</span>\n</div>\n<div class=\"flexi-video__progress\">\n<div class=\"flexi-video__progress-track\">\n<div class=\"flexi-video__progress-played\"></div>\n<div class=\"flexi-video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"flexi-video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"flexi-video__fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z\"/></svg>\n<svg class=\"flexi-video__exit-fullscreen-icon\"><use xlink:href=\"#exit-fullscreen-icon\"/></svg>\n</button>\n</div>\n<div class=\"flexi-video__error-notice\">\n<svg class=\"flexi-video__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-video__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiVideoBlocks")]
        [InlineData("All")]
        public void FlexiVideoBlocks_Spec20(string extensions)
        {
            //     Start line number: 1134
            //     --------------- Markdown ---------------
            //     v{ 
            //       "src": "/file.mp4",
            //       "exitFullscreenIcon": null
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_no-exit-fullscreen-icon flexi-video_has-error-icon">
            //     <div class="flexi-video__container" tabindex="-1">
            //     <div class="flexi-video__video-outer-container">
            //     <div class="flexi-video__video-inner-container">
            //     <video class="flexi-video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="flexi-video__controls">
            //     <button class="flexi-video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
            //     </button>
            //     <div class="flexi-video__elapsed-time">
            //     <span class="flexi-video__current-time">0:00</span>
            //     /<span class="flexi-video__duration">0:00</span>
            //     </div>
            //     <div class="flexi-video__progress">
            //     <div class="flexi-video__progress-track">
            //     <div class="flexi-video__progress-played"></div>
            //     <div class="flexi-video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="flexi-video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
            //     </button>
            //     </div>
            //     <div class="flexi-video__error-notice">
            //     <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-video__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("v{ \n  \"src\": \"/file.mp4\",\n  \"exitFullscreenIcon\": null\n}",
                "<div class=\"flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_no-exit-fullscreen-icon flexi-video_has-error-icon\">\n<div class=\"flexi-video__container\" tabindex=\"-1\">\n<div class=\"flexi-video__video-outer-container\">\n<div class=\"flexi-video__video-inner-container\">\n<video class=\"flexi-video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"flexi-video__source\" data-src=\"/file.mp4\" type=\"video/mp4\">\n</video>\n</div>\n</div>\n<div class=\"flexi-video__controls\">\n<button class=\"flexi-video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"flexi-video__play-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M8 5v14l11-7z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n<svg class=\"flexi-video__pause-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M6 19h4V5H6v14zm8-14v14h4V5h-4z\"/></svg>\n</button>\n<div class=\"flexi-video__elapsed-time\">\n<span class=\"flexi-video__current-time\">0:00</span>\n/<span class=\"flexi-video__duration\">0:00</span>\n</div>\n<div class=\"flexi-video__progress\">\n<div class=\"flexi-video__progress-track\">\n<div class=\"flexi-video__progress-played\"></div>\n<div class=\"flexi-video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"flexi-video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"flexi-video__fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z\"/></svg>\n</button>\n</div>\n<div class=\"flexi-video__error-notice\">\n<svg class=\"flexi-video__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-video__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiVideoBlocks")]
        [InlineData("All")]
        public void FlexiVideoBlocks_Spec21(string extensions)
        {
            //     Start line number: 1190
            //     --------------- Markdown ---------------
            //     v{ 
            //       "src": "/file.mp4",
            //       "errorIcon": "<svg><use xlink:href=\"#error-icon\"/></svg>"
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
            //     <div class="flexi-video__container" tabindex="-1">
            //     <div class="flexi-video__video-outer-container">
            //     <div class="flexi-video__video-inner-container">
            //     <video class="flexi-video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="flexi-video__controls">
            //     <button class="flexi-video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
            //     </button>
            //     <div class="flexi-video__elapsed-time">
            //     <span class="flexi-video__current-time">0:00</span>
            //     /<span class="flexi-video__duration">0:00</span>
            //     </div>
            //     <div class="flexi-video__progress">
            //     <div class="flexi-video__progress-track">
            //     <div class="flexi-video__progress-played"></div>
            //     <div class="flexi-video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="flexi-video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
            //     <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
            //     </button>
            //     </div>
            //     <div class="flexi-video__error-notice">
            //     <svg class="flexi-video__error-icon"><use xlink:href="#error-icon"/></svg>
            //     </div>
            //     <div class="flexi-video__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("v{ \n  \"src\": \"/file.mp4\",\n  \"errorIcon\": \"<svg><use xlink:href=\\\"#error-icon\\\"/></svg>\"\n}",
                "<div class=\"flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon\">\n<div class=\"flexi-video__container\" tabindex=\"-1\">\n<div class=\"flexi-video__video-outer-container\">\n<div class=\"flexi-video__video-inner-container\">\n<video class=\"flexi-video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"flexi-video__source\" data-src=\"/file.mp4\" type=\"video/mp4\">\n</video>\n</div>\n</div>\n<div class=\"flexi-video__controls\">\n<button class=\"flexi-video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"flexi-video__play-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M8 5v14l11-7z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n<svg class=\"flexi-video__pause-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M6 19h4V5H6v14zm8-14v14h4V5h-4z\"/></svg>\n</button>\n<div class=\"flexi-video__elapsed-time\">\n<span class=\"flexi-video__current-time\">0:00</span>\n/<span class=\"flexi-video__duration\">0:00</span>\n</div>\n<div class=\"flexi-video__progress\">\n<div class=\"flexi-video__progress-track\">\n<div class=\"flexi-video__progress-played\"></div>\n<div class=\"flexi-video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"flexi-video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"flexi-video__fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z\"/></svg>\n<svg class=\"flexi-video__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z\"/></svg>\n</button>\n</div>\n<div class=\"flexi-video__error-notice\">\n<svg class=\"flexi-video__error-icon\"><use xlink:href=\"#error-icon\"/></svg>\n</div>\n<div class=\"flexi-video__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiVideoBlocks")]
        [InlineData("All")]
        public void FlexiVideoBlocks_Spec22(string extensions)
        {
            //     Start line number: 1240
            //     --------------- Markdown ---------------
            //     v{ 
            //       "src": "/file.mp4",
            //       "errorIcon": null
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_no-error-icon">
            //     <div class="flexi-video__container" tabindex="-1">
            //     <div class="flexi-video__video-outer-container">
            //     <div class="flexi-video__video-inner-container">
            //     <video class="flexi-video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="flexi-video__controls">
            //     <button class="flexi-video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
            //     </button>
            //     <div class="flexi-video__elapsed-time">
            //     <span class="flexi-video__current-time">0:00</span>
            //     /<span class="flexi-video__duration">0:00</span>
            //     </div>
            //     <div class="flexi-video__progress">
            //     <div class="flexi-video__progress-track">
            //     <div class="flexi-video__progress-played"></div>
            //     <div class="flexi-video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="flexi-video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
            //     <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
            //     </button>
            //     </div>
            //     <div class="flexi-video__error-notice">
            //     </div>
            //     <div class="flexi-video__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("v{ \n  \"src\": \"/file.mp4\",\n  \"errorIcon\": null\n}",
                "<div class=\"flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_no-error-icon\">\n<div class=\"flexi-video__container\" tabindex=\"-1\">\n<div class=\"flexi-video__video-outer-container\">\n<div class=\"flexi-video__video-inner-container\">\n<video class=\"flexi-video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"flexi-video__source\" data-src=\"/file.mp4\" type=\"video/mp4\">\n</video>\n</div>\n</div>\n<div class=\"flexi-video__controls\">\n<button class=\"flexi-video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"flexi-video__play-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M8 5v14l11-7z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n<svg class=\"flexi-video__pause-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M6 19h4V5H6v14zm8-14v14h4V5h-4z\"/></svg>\n</button>\n<div class=\"flexi-video__elapsed-time\">\n<span class=\"flexi-video__current-time\">0:00</span>\n/<span class=\"flexi-video__duration\">0:00</span>\n</div>\n<div class=\"flexi-video__progress\">\n<div class=\"flexi-video__progress-track\">\n<div class=\"flexi-video__progress-played\"></div>\n<div class=\"flexi-video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"flexi-video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"flexi-video__fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z\"/></svg>\n<svg class=\"flexi-video__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z\"/></svg>\n</button>\n</div>\n<div class=\"flexi-video__error-notice\">\n</div>\n<div class=\"flexi-video__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiVideoBlocks")]
        [InlineData("All")]
        public void FlexiVideoBlocks_Spec23(string extensions)
        {
            //     Start line number: 1307
            //     --------------- Markdown ---------------
            //     v{ 
            //       "src": "/file.mp4",
            //       "attributes": {
            //           "id" : "my-custom-id",
            //           "class" : "my-custom-class"
            //       }
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon my-custom-class" id="my-custom-id">
            //     <div class="flexi-video__container" tabindex="-1">
            //     <div class="flexi-video__video-outer-container">
            //     <div class="flexi-video__video-inner-container">
            //     <video class="flexi-video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="flexi-video__controls">
            //     <button class="flexi-video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
            //     </button>
            //     <div class="flexi-video__elapsed-time">
            //     <span class="flexi-video__current-time">0:00</span>
            //     /<span class="flexi-video__duration">0:00</span>
            //     </div>
            //     <div class="flexi-video__progress">
            //     <div class="flexi-video__progress-track">
            //     <div class="flexi-video__progress-played"></div>
            //     <div class="flexi-video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="flexi-video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
            //     <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
            //     </button>
            //     </div>
            //     <div class="flexi-video__error-notice">
            //     <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-video__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("v{ \n  \"src\": \"/file.mp4\",\n  \"attributes\": {\n      \"id\" : \"my-custom-id\",\n      \"class\" : \"my-custom-class\"\n  }\n}",
                "<div class=\"flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon my-custom-class\" id=\"my-custom-id\">\n<div class=\"flexi-video__container\" tabindex=\"-1\">\n<div class=\"flexi-video__video-outer-container\">\n<div class=\"flexi-video__video-inner-container\">\n<video class=\"flexi-video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"flexi-video__source\" data-src=\"/file.mp4\" type=\"video/mp4\">\n</video>\n</div>\n</div>\n<div class=\"flexi-video__controls\">\n<button class=\"flexi-video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"flexi-video__play-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M8 5v14l11-7z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n<svg class=\"flexi-video__pause-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M6 19h4V5H6v14zm8-14v14h4V5h-4z\"/></svg>\n</button>\n<div class=\"flexi-video__elapsed-time\">\n<span class=\"flexi-video__current-time\">0:00</span>\n/<span class=\"flexi-video__duration\">0:00</span>\n</div>\n<div class=\"flexi-video__progress\">\n<div class=\"flexi-video__progress-track\">\n<div class=\"flexi-video__progress-played\"></div>\n<div class=\"flexi-video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"flexi-video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"flexi-video__fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z\"/></svg>\n<svg class=\"flexi-video__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z\"/></svg>\n</button>\n</div>\n<div class=\"flexi-video__error-notice\">\n<svg class=\"flexi-video__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-video__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }

        [Theory]
        [InlineData("FlexiVideoBlocks")]
        [InlineData("All")]
        public void FlexiVideoBlocks_Spec24(string extensions)
        {
            //     Start line number: 1375
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiVideoBlocks": {
            //             "defaultBlockOptions": {
            //                 "errorIcon": "<svg><use xlink:href=\"#error-icon\"/></svg>",
            //                 "attributes": {
            //                     "class": "block"
            //                 }
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     v{ 
            //       "src": "/file.mp4"
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon block">
            //     <div class="flexi-video__container" tabindex="-1">
            //     <div class="flexi-video__video-outer-container">
            //     <div class="flexi-video__video-inner-container">
            //     <video class="flexi-video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="flexi-video__controls">
            //     <button class="flexi-video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
            //     </button>
            //     <div class="flexi-video__elapsed-time">
            //     <span class="flexi-video__current-time">0:00</span>
            //     /<span class="flexi-video__duration">0:00</span>
            //     </div>
            //     <div class="flexi-video__progress">
            //     <div class="flexi-video__progress-track">
            //     <div class="flexi-video__progress-played"></div>
            //     <div class="flexi-video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="flexi-video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
            //     <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
            //     </button>
            //     </div>
            //     <div class="flexi-video__error-notice">
            //     <svg class="flexi-video__error-icon"><use xlink:href="#error-icon"/></svg>
            //     </div>
            //     <div class="flexi-video__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("v{ \n  \"src\": \"/file.mp4\"\n}",
                "<div class=\"flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon block\">\n<div class=\"flexi-video__container\" tabindex=\"-1\">\n<div class=\"flexi-video__video-outer-container\">\n<div class=\"flexi-video__video-inner-container\">\n<video class=\"flexi-video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"flexi-video__source\" data-src=\"/file.mp4\" type=\"video/mp4\">\n</video>\n</div>\n</div>\n<div class=\"flexi-video__controls\">\n<button class=\"flexi-video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"flexi-video__play-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M8 5v14l11-7z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n<svg class=\"flexi-video__pause-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M6 19h4V5H6v14zm8-14v14h4V5h-4z\"/></svg>\n</button>\n<div class=\"flexi-video__elapsed-time\">\n<span class=\"flexi-video__current-time\">0:00</span>\n/<span class=\"flexi-video__duration\">0:00</span>\n</div>\n<div class=\"flexi-video__progress\">\n<div class=\"flexi-video__progress-track\">\n<div class=\"flexi-video__progress-played\"></div>\n<div class=\"flexi-video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"flexi-video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"flexi-video__fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z\"/></svg>\n<svg class=\"flexi-video__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z\"/></svg>\n</button>\n</div>\n<div class=\"flexi-video__error-notice\">\n<svg class=\"flexi-video__error-icon\"><use xlink:href=\"#error-icon\"/></svg>\n</div>\n<div class=\"flexi-video__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n</div>\n</div>",
                extensions,
                false,
                "{\n    \"flexiVideoBlocks\": {\n        \"defaultBlockOptions\": {\n            \"errorIcon\": \"<svg><use xlink:href=\\\"#error-icon\\\"/></svg>\",\n            \"attributes\": {\n                \"class\": \"block\"\n            }\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiVideoBlocks")]
        [InlineData("All")]
        public void FlexiVideoBlocks_Spec25(string extensions)
        {
            //     Start line number: 1435
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiVideoBlocks": {
            //             "defaultBlockOptions": {
            //                 "blockName": "video"
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     v{ 
            //       "src": "/file.mp4"
            //     }
            //     
            //     v{ 
            //       "blockname": "special-video",
            //       "src": "/file.mp4"
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="video video_no-poster video_no-width video_no-aspect-ratio video_no-duration video_has-type video_has-spinner video_has-play-icon video_has-pause-icon video_has-fullscreen-icon video_has-exit-fullscreen-icon video_has-error-icon">
            //     <div class="video__container" tabindex="-1">
            //     <div class="video__video-outer-container">
            //     <div class="video__video-inner-container">
            //     <video class="video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="video__source" data-src="/file.mp4" type="video/mp4">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="video__controls">
            //     <button class="video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     <svg class="video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
            //     </button>
            //     <div class="video__elapsed-time">
            //     <span class="video__current-time">0:00</span>
            //     /<span class="video__duration">0:00</span>
            //     </div>
            //     <div class="video__progress">
            //     <div class="video__progress-track">
            //     <div class="video__progress-played"></div>
            //     <div class="video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
            //     <svg class="video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
            //     </button>
            //     </div>
            //     <div class="video__error-notice">
            //     <svg class="video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="video__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     </div>
            //     </div>
            //     <div class="special-video special-video_no-poster special-video_no-width special-video_no-aspect-ratio special-video_no-duration special-video_has-type special-video_has-spinner special-video_has-play-icon special-video_has-pause-icon special-video_has-fullscreen-icon special-video_has-exit-fullscreen-icon special-video_has-error-icon">
            //     <div class="special-video__container" tabindex="-1">
            //     <div class="special-video__video-outer-container">
            //     <div class="special-video__video-inner-container">
            //     <video class="special-video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="special-video__source" data-src="/file.mp4" type="video/mp4">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="special-video__controls">
            //     <button class="special-video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="special-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     <svg class="special-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
            //     </button>
            //     <div class="special-video__elapsed-time">
            //     <span class="special-video__current-time">0:00</span>
            //     /<span class="special-video__duration">0:00</span>
            //     </div>
            //     <div class="special-video__progress">
            //     <div class="special-video__progress-track">
            //     <div class="special-video__progress-played"></div>
            //     <div class="special-video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="special-video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="special-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
            //     <svg class="special-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
            //     </button>
            //     </div>
            //     <div class="special-video__error-notice">
            //     <svg class="special-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="special-video__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("v{ \n  \"src\": \"/file.mp4\"\n}\n\nv{ \n  \"blockname\": \"special-video\",\n  \"src\": \"/file.mp4\"\n}",
                "<div class=\"video video_no-poster video_no-width video_no-aspect-ratio video_no-duration video_has-type video_has-spinner video_has-play-icon video_has-pause-icon video_has-fullscreen-icon video_has-exit-fullscreen-icon video_has-error-icon\">\n<div class=\"video__container\" tabindex=\"-1\">\n<div class=\"video__video-outer-container\">\n<div class=\"video__video-inner-container\">\n<video class=\"video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"video__source\" data-src=\"/file.mp4\" type=\"video/mp4\">\n</video>\n</div>\n</div>\n<div class=\"video__controls\">\n<button class=\"video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"video__play-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M8 5v14l11-7z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n<svg class=\"video__pause-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M6 19h4V5H6v14zm8-14v14h4V5h-4z\"/></svg>\n</button>\n<div class=\"video__elapsed-time\">\n<span class=\"video__current-time\">0:00</span>\n/<span class=\"video__duration\">0:00</span>\n</div>\n<div class=\"video__progress\">\n<div class=\"video__progress-track\">\n<div class=\"video__progress-played\"></div>\n<div class=\"video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"video__fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z\"/></svg>\n<svg class=\"video__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z\"/></svg>\n</button>\n</div>\n<div class=\"video__error-notice\">\n<svg class=\"video__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"video__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n</div>\n</div>\n<div class=\"special-video special-video_no-poster special-video_no-width special-video_no-aspect-ratio special-video_no-duration special-video_has-type special-video_has-spinner special-video_has-play-icon special-video_has-pause-icon special-video_has-fullscreen-icon special-video_has-exit-fullscreen-icon special-video_has-error-icon\">\n<div class=\"special-video__container\" tabindex=\"-1\">\n<div class=\"special-video__video-outer-container\">\n<div class=\"special-video__video-inner-container\">\n<video class=\"special-video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"special-video__source\" data-src=\"/file.mp4\" type=\"video/mp4\">\n</video>\n</div>\n</div>\n<div class=\"special-video__controls\">\n<button class=\"special-video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"special-video__play-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M8 5v14l11-7z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n<svg class=\"special-video__pause-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M6 19h4V5H6v14zm8-14v14h4V5h-4z\"/></svg>\n</button>\n<div class=\"special-video__elapsed-time\">\n<span class=\"special-video__current-time\">0:00</span>\n/<span class=\"special-video__duration\">0:00</span>\n</div>\n<div class=\"special-video__progress\">\n<div class=\"special-video__progress-track\">\n<div class=\"special-video__progress-played\"></div>\n<div class=\"special-video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"special-video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"special-video__fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z\"/></svg>\n<svg class=\"special-video__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z\"/></svg>\n</button>\n</div>\n<div class=\"special-video__error-notice\">\n<svg class=\"special-video__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"special-video__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n</div>\n</div>",
                extensions,
                false,
                "{\n    \"flexiVideoBlocks\": {\n        \"defaultBlockOptions\": {\n            \"blockName\": \"video\"\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiVideoBlocks")]
        [InlineData("All")]
        public void FlexiVideoBlocks_Spec26(string extensions)
        {
            //     Start line number: 1551
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiVideoBlocks": {
            //             "mimeTypes": {
            //                 ".3gp": "video/3gpp",
            //                 ".mov": "video/quicktime"
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     v{ 
            //       "src": "/file.3gp"
            //     }
            //     
            //     v{ 
            //       "src": "/file.mov"
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
            //     <div class="flexi-video__container" tabindex="-1">
            //     <div class="flexi-video__video-outer-container">
            //     <div class="flexi-video__video-inner-container">
            //     <video class="flexi-video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="flexi-video__source" data-src="/file.3gp" type="video/3gpp">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="flexi-video__controls">
            //     <button class="flexi-video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
            //     </button>
            //     <div class="flexi-video__elapsed-time">
            //     <span class="flexi-video__current-time">0:00</span>
            //     /<span class="flexi-video__duration">0:00</span>
            //     </div>
            //     <div class="flexi-video__progress">
            //     <div class="flexi-video__progress-track">
            //     <div class="flexi-video__progress-played"></div>
            //     <div class="flexi-video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="flexi-video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
            //     <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
            //     </button>
            //     </div>
            //     <div class="flexi-video__error-notice">
            //     <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-video__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     </div>
            //     </div>
            //     <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
            //     <div class="flexi-video__container" tabindex="-1">
            //     <div class="flexi-video__video-outer-container">
            //     <div class="flexi-video__video-inner-container">
            //     <video class="flexi-video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="flexi-video__source" data-src="/file.mov" type="video/quicktime">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="flexi-video__controls">
            //     <button class="flexi-video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
            //     </button>
            //     <div class="flexi-video__elapsed-time">
            //     <span class="flexi-video__current-time">0:00</span>
            //     /<span class="flexi-video__duration">0:00</span>
            //     </div>
            //     <div class="flexi-video__progress">
            //     <div class="flexi-video__progress-track">
            //     <div class="flexi-video__progress-played"></div>
            //     <div class="flexi-video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="flexi-video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
            //     <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
            //     </button>
            //     </div>
            //     <div class="flexi-video__error-notice">
            //     <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-video__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("v{ \n  \"src\": \"/file.3gp\"\n}\n\nv{ \n  \"src\": \"/file.mov\"\n}",
                "<div class=\"flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon\">\n<div class=\"flexi-video__container\" tabindex=\"-1\">\n<div class=\"flexi-video__video-outer-container\">\n<div class=\"flexi-video__video-inner-container\">\n<video class=\"flexi-video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"flexi-video__source\" data-src=\"/file.3gp\" type=\"video/3gpp\">\n</video>\n</div>\n</div>\n<div class=\"flexi-video__controls\">\n<button class=\"flexi-video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"flexi-video__play-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M8 5v14l11-7z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n<svg class=\"flexi-video__pause-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M6 19h4V5H6v14zm8-14v14h4V5h-4z\"/></svg>\n</button>\n<div class=\"flexi-video__elapsed-time\">\n<span class=\"flexi-video__current-time\">0:00</span>\n/<span class=\"flexi-video__duration\">0:00</span>\n</div>\n<div class=\"flexi-video__progress\">\n<div class=\"flexi-video__progress-track\">\n<div class=\"flexi-video__progress-played\"></div>\n<div class=\"flexi-video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"flexi-video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"flexi-video__fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z\"/></svg>\n<svg class=\"flexi-video__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z\"/></svg>\n</button>\n</div>\n<div class=\"flexi-video__error-notice\">\n<svg class=\"flexi-video__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-video__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n</div>\n</div>\n<div class=\"flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon\">\n<div class=\"flexi-video__container\" tabindex=\"-1\">\n<div class=\"flexi-video__video-outer-container\">\n<div class=\"flexi-video__video-inner-container\">\n<video class=\"flexi-video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"flexi-video__source\" data-src=\"/file.mov\" type=\"video/quicktime\">\n</video>\n</div>\n</div>\n<div class=\"flexi-video__controls\">\n<button class=\"flexi-video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"flexi-video__play-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M8 5v14l11-7z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n<svg class=\"flexi-video__pause-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M6 19h4V5H6v14zm8-14v14h4V5h-4z\"/></svg>\n</button>\n<div class=\"flexi-video__elapsed-time\">\n<span class=\"flexi-video__current-time\">0:00</span>\n/<span class=\"flexi-video__duration\">0:00</span>\n</div>\n<div class=\"flexi-video__progress\">\n<div class=\"flexi-video__progress-track\">\n<div class=\"flexi-video__progress-played\"></div>\n<div class=\"flexi-video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"flexi-video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"flexi-video__fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z\"/></svg>\n<svg class=\"flexi-video__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z\"/></svg>\n</button>\n</div>\n<div class=\"flexi-video__error-notice\">\n<svg class=\"flexi-video__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-video__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n</div>\n</div>",
                extensions,
                false,
                "{\n    \"flexiVideoBlocks\": {\n        \"mimeTypes\": {\n            \".3gp\": \"video/3gpp\",\n            \".mov\": \"video/quicktime\"\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiVideoBlocks")]
        [InlineData("All")]
        public void FlexiVideoBlocks_Spec27(string extensions)
        {
            //     Start line number: 1654
            //     --------------- Markdown ---------------
            //     v{ 
            //       "src": "/file.mp4"
            //     }
            //     
            //     v{ 
            //       "src": "/file.webm"
            //     }
            //     
            //     v{ 
            //       "src": "/file.ogg"
            //     }
            //     --------------- Expected Markup ---------------
            //     <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
            //     <div class="flexi-video__container" tabindex="-1">
            //     <div class="flexi-video__video-outer-container">
            //     <div class="flexi-video__video-inner-container">
            //     <video class="flexi-video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="flexi-video__controls">
            //     <button class="flexi-video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
            //     </button>
            //     <div class="flexi-video__elapsed-time">
            //     <span class="flexi-video__current-time">0:00</span>
            //     /<span class="flexi-video__duration">0:00</span>
            //     </div>
            //     <div class="flexi-video__progress">
            //     <div class="flexi-video__progress-track">
            //     <div class="flexi-video__progress-played"></div>
            //     <div class="flexi-video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="flexi-video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
            //     <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
            //     </button>
            //     </div>
            //     <div class="flexi-video__error-notice">
            //     <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-video__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     </div>
            //     </div>
            //     <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
            //     <div class="flexi-video__container" tabindex="-1">
            //     <div class="flexi-video__video-outer-container">
            //     <div class="flexi-video__video-inner-container">
            //     <video class="flexi-video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="flexi-video__source" data-src="/file.webm" type="video/webm">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="flexi-video__controls">
            //     <button class="flexi-video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
            //     </button>
            //     <div class="flexi-video__elapsed-time">
            //     <span class="flexi-video__current-time">0:00</span>
            //     /<span class="flexi-video__duration">0:00</span>
            //     </div>
            //     <div class="flexi-video__progress">
            //     <div class="flexi-video__progress-track">
            //     <div class="flexi-video__progress-played"></div>
            //     <div class="flexi-video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="flexi-video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
            //     <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
            //     </button>
            //     </div>
            //     <div class="flexi-video__error-notice">
            //     <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-video__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     </div>
            //     </div>
            //     <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
            //     <div class="flexi-video__container" tabindex="-1">
            //     <div class="flexi-video__video-outer-container">
            //     <div class="flexi-video__video-inner-container">
            //     <video class="flexi-video__video" preload="none" muted playsInline disablePictureInPicture loop>
            //     <source class="flexi-video__source" data-src="/file.ogg" type="video/ogg">
            //     </video>
            //     </div>
            //     </div>
            //     <div class="flexi-video__controls">
            //     <button class="flexi-video__play-pause-button" title="Pause/play" aria-label="Pause/play">
            //     <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
            //     <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
            //     </button>
            //     <div class="flexi-video__elapsed-time">
            //     <span class="flexi-video__current-time">0:00</span>
            //     /<span class="flexi-video__duration">0:00</span>
            //     </div>
            //     <div class="flexi-video__progress">
            //     <div class="flexi-video__progress-track">
            //     <div class="flexi-video__progress-played"></div>
            //     <div class="flexi-video__progress-buffered"></div>
            //     </div>
            //     </div>
            //     <button class="flexi-video__fullscreen-button" title="Toggle fullscreen" aria-label="Toggle fullscreen">
            //     <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
            //     <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
            //     </button>
            //     </div>
            //     <div class="flexi-video__error-notice">
            //     <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
            //     </div>
            //     <div class="flexi-video__spinner spinner">
            //         <div class="spinner__rects">
            //             <div class="spinner__rect-1"></div>
            //             <div class="spinner__rect-2"></div>
            //             <div class="spinner__rect-3"></div>
            //         </div>
            //     </div>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("v{ \n  \"src\": \"/file.mp4\"\n}\n\nv{ \n  \"src\": \"/file.webm\"\n}\n\nv{ \n  \"src\": \"/file.ogg\"\n}",
                "<div class=\"flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon\">\n<div class=\"flexi-video__container\" tabindex=\"-1\">\n<div class=\"flexi-video__video-outer-container\">\n<div class=\"flexi-video__video-inner-container\">\n<video class=\"flexi-video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"flexi-video__source\" data-src=\"/file.mp4\" type=\"video/mp4\">\n</video>\n</div>\n</div>\n<div class=\"flexi-video__controls\">\n<button class=\"flexi-video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"flexi-video__play-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M8 5v14l11-7z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n<svg class=\"flexi-video__pause-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M6 19h4V5H6v14zm8-14v14h4V5h-4z\"/></svg>\n</button>\n<div class=\"flexi-video__elapsed-time\">\n<span class=\"flexi-video__current-time\">0:00</span>\n/<span class=\"flexi-video__duration\">0:00</span>\n</div>\n<div class=\"flexi-video__progress\">\n<div class=\"flexi-video__progress-track\">\n<div class=\"flexi-video__progress-played\"></div>\n<div class=\"flexi-video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"flexi-video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"flexi-video__fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z\"/></svg>\n<svg class=\"flexi-video__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z\"/></svg>\n</button>\n</div>\n<div class=\"flexi-video__error-notice\">\n<svg class=\"flexi-video__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-video__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n</div>\n</div>\n<div class=\"flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon\">\n<div class=\"flexi-video__container\" tabindex=\"-1\">\n<div class=\"flexi-video__video-outer-container\">\n<div class=\"flexi-video__video-inner-container\">\n<video class=\"flexi-video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"flexi-video__source\" data-src=\"/file.webm\" type=\"video/webm\">\n</video>\n</div>\n</div>\n<div class=\"flexi-video__controls\">\n<button class=\"flexi-video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"flexi-video__play-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M8 5v14l11-7z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n<svg class=\"flexi-video__pause-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M6 19h4V5H6v14zm8-14v14h4V5h-4z\"/></svg>\n</button>\n<div class=\"flexi-video__elapsed-time\">\n<span class=\"flexi-video__current-time\">0:00</span>\n/<span class=\"flexi-video__duration\">0:00</span>\n</div>\n<div class=\"flexi-video__progress\">\n<div class=\"flexi-video__progress-track\">\n<div class=\"flexi-video__progress-played\"></div>\n<div class=\"flexi-video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"flexi-video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"flexi-video__fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z\"/></svg>\n<svg class=\"flexi-video__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z\"/></svg>\n</button>\n</div>\n<div class=\"flexi-video__error-notice\">\n<svg class=\"flexi-video__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-video__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n</div>\n</div>\n<div class=\"flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon\">\n<div class=\"flexi-video__container\" tabindex=\"-1\">\n<div class=\"flexi-video__video-outer-container\">\n<div class=\"flexi-video__video-inner-container\">\n<video class=\"flexi-video__video\" preload=\"none\" muted playsInline disablePictureInPicture loop>\n<source class=\"flexi-video__source\" data-src=\"/file.ogg\" type=\"video/ogg\">\n</video>\n</div>\n</div>\n<div class=\"flexi-video__controls\">\n<button class=\"flexi-video__play-pause-button\" title=\"Pause/play\" aria-label=\"Pause/play\">\n<svg class=\"flexi-video__play-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M8 5v14l11-7z\"/><path d=\"M0 0h24v24H0z\" fill=\"none\"/></svg>\n<svg class=\"flexi-video__pause-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M6 19h4V5H6v14zm8-14v14h4V5h-4z\"/></svg>\n</button>\n<div class=\"flexi-video__elapsed-time\">\n<span class=\"flexi-video__current-time\">0:00</span>\n/<span class=\"flexi-video__duration\">0:00</span>\n</div>\n<div class=\"flexi-video__progress\">\n<div class=\"flexi-video__progress-track\">\n<div class=\"flexi-video__progress-played\"></div>\n<div class=\"flexi-video__progress-buffered\"></div>\n</div>\n</div>\n<button class=\"flexi-video__fullscreen-button\" title=\"Toggle fullscreen\" aria-label=\"Toggle fullscreen\">\n<svg class=\"flexi-video__fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z\"/></svg>\n<svg class=\"flexi-video__exit-fullscreen-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path shape-rendering=\"crispEdges\" d=\"M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z\"/></svg>\n</button>\n</div>\n<div class=\"flexi-video__error-notice\">\n<svg class=\"flexi-video__error-icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z\"/></svg>\n</div>\n<div class=\"flexi-video__spinner spinner\">\n    <div class=\"spinner__rects\">\n        <div class=\"spinner__rect-1\"></div>\n        <div class=\"spinner__rect-2\"></div>\n        <div class=\"spinner__rect-3\"></div>\n    </div>\n</div>\n</div>\n</div>",
                extensions,
                false);
        }
    }
}

