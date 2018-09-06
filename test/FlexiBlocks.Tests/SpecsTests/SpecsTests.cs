using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.Specs
{
    public class FlexiAlertBlocksTests
    {
        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec1(string extensions)
        {
            //     --------------- Markdown ---------------
            //     ! critical-warning
            //     ! This is a critical warning.
            //     --------------- Expected Markup ---------------
            //     <div class="fab-critical-warning">
            //     <svg viewBox="0 0 24 24" width="24" height="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"></path></svg>
            //     <div class="fab-content">
            //     <p>This is a critical warning.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! critical-warning\n! This is a critical warning.",
                "<div class=\"fab-critical-warning\">\n<svg viewBox=\"0 0 24 24\" width=\"24\" height=\"24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"></path><path d=\"M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z\"></path></svg>\n<div class=\"fab-content\">\n<p>This is a critical warning.</p>\n</div>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec2(string extensions)
        {
            //     --------------- Markdown ---------------
            //     ! info
            //     ! This is information.
            //     --------------- Expected Markup ---------------
            //     <div class="fab-info">
            //     <svg viewBox="0 0 24 24" width="24" height="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"></path></svg>
            //     <div class="fab-content">
            //     <p>This is information.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! info\n! This is information.",
                "<div class=\"fab-info\">\n<svg viewBox=\"0 0 24 24\" width=\"24\" height=\"24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"></path><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"></path></svg>\n<div class=\"fab-content\">\n<p>This is information.</p>\n</div>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec3(string extensions)
        {
            //     --------------- Markdown ---------------
            //     ! 
            //     ! This is a warning.
            //     --------------- Expected Markup ---------------
            //     <p>!
            //     ! This is a warning.</p>

            SpecTestHelper.AssertCompliance("! \n! This is a warning.",
                "<p>!\n! This is a warning.</p>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec4(string extensions)
        {
            //     --------------- Markdown ---------------
            //     ! illegal space
            //     ! This is a warning.
            //     --------------- Expected Markup ---------------
            //     <p>! illegal space
            //     ! This is a warning.</p>

            SpecTestHelper.AssertCompliance("! illegal space\n! This is a warning.",
                "<p>! illegal space\n! This is a warning.</p>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec5(string extensions)
        {
            //     --------------- Markdown ---------------
            //     ! warning
            //     !This line will be rendered with 0 leading spaces.
            //     ! This line will also be rendered with 0 leading spaces.
            //     --------------- Expected Markup ---------------
            //     <div class="fab-warning">
            //     <svg viewBox="0 0 24 24" width="24" height="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"></path></svg>
            //     <div class="fab-content">
            //     <p>This line will be rendered with 0 leading spaces.
            //     This line will also be rendered with 0 leading spaces.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! warning\n!This line will be rendered with 0 leading spaces.\n! This line will also be rendered with 0 leading spaces.",
                "<div class=\"fab-warning\">\n<svg viewBox=\"0 0 24 24\" width=\"24\" height=\"24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"></path><path d=\"M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z\"></path></svg>\n<div class=\"fab-content\">\n<p>This line will be rendered with 0 leading spaces.\nThis line will also be rendered with 0 leading spaces.</p>\n</div>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec6(string extensions)
        {
            //     --------------- Markdown ---------------
            //     ! info
            //     ! This is part of
            //     the info.
            //     ! This is also part of
            //     the info.
            //     --------------- Expected Markup ---------------
            //     <div class="fab-info">
            //     <svg viewBox="0 0 24 24" width="24" height="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"></path></svg>
            //     <div class="fab-content">
            //     <p>This is part of
            //     the info.
            //     This is also part of
            //     the info.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! info\n! This is part of\nthe info.\n! This is also part of\nthe info.",
                "<div class=\"fab-info\">\n<svg viewBox=\"0 0 24 24\" width=\"24\" height=\"24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"></path><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"></path></svg>\n<div class=\"fab-content\">\n<p>This is part of\nthe info.\nThis is also part of\nthe info.</p>\n</div>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec7(string extensions)
        {
            //     --------------- Markdown ---------------
            //     ! info
            //     ! This is information.
            //     
            //     ! warning
            //     ! This is a warning.
            //     --------------- Expected Markup ---------------
            //     <div class="fab-info">
            //     <svg viewBox="0 0 24 24" width="24" height="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"></path></svg>
            //     <div class="fab-content">
            //     <p>This is information.</p>
            //     </div>
            //     </div>
            //     <div class="fab-warning">
            //     <svg viewBox="0 0 24 24" width="24" height="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"></path></svg>
            //     <div class="fab-content">
            //     <p>This is a warning.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! info\n! This is information.\n\n! warning\n! This is a warning.",
                "<div class=\"fab-info\">\n<svg viewBox=\"0 0 24 24\" width=\"24\" height=\"24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"></path><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"></path></svg>\n<div class=\"fab-content\">\n<p>This is information.</p>\n</div>\n</div>\n<div class=\"fab-warning\">\n<svg viewBox=\"0 0 24 24\" width=\"24\" height=\"24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"></path><path d=\"M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z\"></path></svg>\n<div class=\"fab-content\">\n<p>This is a warning.</p>\n</div>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec8(string extensions)
        {
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "iconMarkup": "<svg><use xlink:href=\"#alternative-warning-icon\"></use></svg>"
            //     }
            //     ! warning
            //     ! This is a warning.
            //     --------------- Expected Markup ---------------
            //     <div class="fab-warning">
            //     <svg><use xlink:href="#alternative-warning-icon"></use></svg>
            //     <div class="fab-content">
            //     <p>This is a warning.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"iconMarkup\": \"<svg><use xlink:href=\\\"#alternative-warning-icon\\\"></use></svg>\"\n}\n! warning\n! This is a warning.",
                "<div class=\"fab-warning\">\n<svg><use xlink:href=\"#alternative-warning-icon\"></use></svg>\n<div class=\"fab-content\">\n<p>This is a warning.</p>\n</div>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec9(string extensions)
        {
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "classNameFormat": "alert-{0}"
            //     }
            //     ! warning
            //     ! This is a warning.
            //     --------------- Expected Markup ---------------
            //     <div class="alert-warning">
            //     <svg viewBox="0 0 24 24" width="24" height="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"></path></svg>
            //     <div class="fab-content">
            //     <p>This is a warning.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"classNameFormat\": \"alert-{0}\"\n}\n! warning\n! This is a warning.",
                "<div class=\"alert-warning\">\n<svg viewBox=\"0 0 24 24\" width=\"24\" height=\"24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"></path><path d=\"M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z\"></path></svg>\n<div class=\"fab-content\">\n<p>This is a warning.</p>\n</div>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec10(string extensions)
        {
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "contentClassName": "alert-content"
            //     }
            //     ! warning
            //     ! This is a warning.
            //     --------------- Expected Markup ---------------
            //     <div class="fab-warning">
            //     <svg viewBox="0 0 24 24" width="24" height="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"></path></svg>
            //     <div class="alert-content">
            //     <p>This is a warning.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"contentClassName\": \"alert-content\"\n}\n! warning\n! This is a warning.",
                "<div class=\"fab-warning\">\n<svg viewBox=\"0 0 24 24\" width=\"24\" height=\"24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"></path><path d=\"M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z\"></path></svg>\n<div class=\"alert-content\">\n<p>This is a warning.</p>\n</div>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec11(string extensions)
        {
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "attributes": {
            //             "id" : "warning-1"
            //         }
            //     }
            //     ! warning
            //     ! This is a warning.
            //     --------------- Expected Markup ---------------
            //     <div id="warning-1" class="fab-warning">
            //     <svg viewBox="0 0 24 24" width="24" height="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"></path></svg>
            //     <div class="fab-content">
            //     <p>This is a warning.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"attributes\": {\n        \"id\" : \"warning-1\"\n    }\n}\n! warning\n! This is a warning.",
                "<div id=\"warning-1\" class=\"fab-warning\">\n<svg viewBox=\"0 0 24 24\" width=\"24\" height=\"24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"></path><path d=\"M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z\"></path></svg>\n<div class=\"fab-content\">\n<p>This is a warning.</p>\n</div>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec12(string extensions)
        {
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiAlertBlocks": {
            //             "iconMarkups": {
            //                 "closer-look": "<svg height=\"24\" viewBox=\"0 0 24 24\" width=\"24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M15.5 14h-.79l-.28-.27C15.41 12.59 16 11.11 16 9.5 16 5.91 13.09 3 9.5 3S3 5.91 3 9.5 5.91 16 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z\"/></svg>",
            //                 "help": "<svg width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 17h-2v-2h2v2zm2.07-7.75l-.9.92C13.45 12.9 13 13.5 13 15h-2v-.5c0-1.1.45-2.1 1.17-2.83l1.24-1.26c.37-.36.59-.86.59-1.41 0-1.1-.9-2-2-2s-2 .9-2 2H8c0-2.21 1.79-4 4-4s4 1.79 4 4c0 .88-.36 1.68-.93 2.25z\"/></svg>"
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     ! closer-look
            //     ! This is a closer look at some topic.
            //     
            //     ! help
            //     ! This is a helpful tip.
            //     --------------- Expected Markup ---------------
            //     <div class="fab-closer-look">
            //     <svg height="24" viewBox="0 0 24 24" width="24" xmlns="http://www.w3.org/2000/svg"><path d="M15.5 14h-.79l-.28-.27C15.41 12.59 16 11.11 16 9.5 16 5.91 13.09 3 9.5 3S3 5.91 3 9.5 5.91 16 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z"/></svg>
            //     <div class="fab-content">
            //     <p>This is a closer look at some topic.</p>
            //     </div>
            //     </div>
            //     <div class="fab-help">
            //     <svg width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 17h-2v-2h2v2zm2.07-7.75l-.9.92C13.45 12.9 13 13.5 13 15h-2v-.5c0-1.1.45-2.1 1.17-2.83l1.24-1.26c.37-.36.59-.86.59-1.41 0-1.1-.9-2-2-2s-2 .9-2 2H8c0-2.21 1.79-4 4-4s4 1.79 4 4c0 .88-.36 1.68-.93 2.25z"/></svg>
            //     <div class="fab-content">
            //     <p>This is a helpful tip.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! closer-look\n! This is a closer look at some topic.\n\n! help\n! This is a helpful tip.",
                "<div class=\"fab-closer-look\">\n<svg height=\"24\" viewBox=\"0 0 24 24\" width=\"24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M15.5 14h-.79l-.28-.27C15.41 12.59 16 11.11 16 9.5 16 5.91 13.09 3 9.5 3S3 5.91 3 9.5 5.91 16 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z\"/></svg>\n<div class=\"fab-content\">\n<p>This is a closer look at some topic.</p>\n</div>\n</div>\n<div class=\"fab-help\">\n<svg width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 17h-2v-2h2v2zm2.07-7.75l-.9.92C13.45 12.9 13 13.5 13 15h-2v-.5c0-1.1.45-2.1 1.17-2.83l1.24-1.26c.37-.36.59-.86.59-1.41 0-1.1-.9-2-2-2s-2 .9-2 2H8c0-2.21 1.79-4 4-4s4 1.79 4 4c0 .88-.36 1.68-.93 2.25z\"/></svg>\n<div class=\"fab-content\">\n<p>This is a helpful tip.</p>\n</div>\n</div>",
                extensions, 
                "{\n    \"flexiAlertBlocks\": {\n        \"iconMarkups\": {\n            \"closer-look\": \"<svg height=\\\"24\\\" viewBox=\\\"0 0 24 24\\\" width=\\\"24\\\" xmlns=\\\"http://www.w3.org/2000/svg\\\"><path d=\\\"M15.5 14h-.79l-.28-.27C15.41 12.59 16 11.11 16 9.5 16 5.91 13.09 3 9.5 3S3 5.91 3 9.5 5.91 16 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z\\\"/></svg>\",\n            \"help\": \"<svg width=\\\"24\\\" height=\\\"24\\\" viewBox=\\\"0 0 24 24\\\"><path d=\\\"M0 0h24v24H0z\\\" fill=\\\"none\\\"/><path d=\\\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 17h-2v-2h2v2zm2.07-7.75l-.9.92C13.45 12.9 13 13.5 13 15h-2v-.5c0-1.1.45-2.1 1.17-2.83l1.24-1.26c.37-.36.59-.86.59-1.41 0-1.1-.9-2-2-2s-2 .9-2 2H8c0-2.21 1.79-4 4-4s4 1.79 4 4c0 .88-.36 1.68-.93 2.25z\\\"/></svg>\"\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec13(string extensions)
        {
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiAlertBlocks": {
            //             "defaultBlockOptions": {
            //                 "iconMarkup": "<svg><use xlink:href=\"#alert-icon\"></use></svg>",
            //                 "classNameFormat": "alert-{0}",
            //                 "contentClassName": "alert-content",
            //                 "attributes": {
            //                     "class": "stretch"
            //                 }
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     ! warning
            //     ! This is a warning.
            //     
            //     ! info
            //     ! This is information.
            //     --------------- Expected Markup ---------------
            //     <div class="stretch alert-warning">
            //     <svg><use xlink:href="#alert-icon"></use></svg>
            //     <div class="alert-content">
            //     <p>This is a warning.</p>
            //     </div>
            //     </div>
            //     <div class="stretch alert-info">
            //     <svg><use xlink:href="#alert-icon"></use></svg>
            //     <div class="alert-content">
            //     <p>This is information.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! warning\n! This is a warning.\n\n! info\n! This is information.",
                "<div class=\"stretch alert-warning\">\n<svg><use xlink:href=\"#alert-icon\"></use></svg>\n<div class=\"alert-content\">\n<p>This is a warning.</p>\n</div>\n</div>\n<div class=\"stretch alert-info\">\n<svg><use xlink:href=\"#alert-icon\"></use></svg>\n<div class=\"alert-content\">\n<p>This is information.</p>\n</div>\n</div>",
                extensions, 
                "{\n    \"flexiAlertBlocks\": {\n        \"defaultBlockOptions\": {\n            \"iconMarkup\": \"<svg><use xlink:href=\\\"#alert-icon\\\"></use></svg>\",\n            \"classNameFormat\": \"alert-{0}\",\n            \"contentClassName\": \"alert-content\",\n            \"attributes\": {\n                \"class\": \"stretch\"\n            }\n        }\n    }\n}");
        }
    }
}

