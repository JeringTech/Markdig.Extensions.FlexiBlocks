using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.Specs
{
    public class FlexiAlertBlocksSpecs
    {
        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec1(string extensions)
        {
            //     --------------- Markdown ---------------
            //     ! This is a FlexiAlertBlock.
            //     ! This is tangential content.
            //     --------------- Expected Markup ---------------
            //     <div class="fab-info">
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z"/></svg>
            //     <div class="fab-content">
            //     <p>This is a FlexiAlertBlock.
            //     This is tangential content.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! This is a FlexiAlertBlock.\n! This is tangential content.",
                "<div class=\"fab-info\">\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z\"/></svg>\n<div class=\"fab-content\">\n<p>This is a FlexiAlertBlock.\nThis is tangential content.</p>\n</div>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec2(string extensions)
        {
            //     --------------- Markdown ---------------
            //     !This line will render identically to the next line.
            //     ! This line will render identically to the previous line.
            //     --------------- Expected Markup ---------------
            //     <div class="fab-info">
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z"/></svg>
            //     <div class="fab-content">
            //     <p>This line will render identically to the next line.
            //     This line will render identically to the previous line.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("!This line will render identically to the next line.\n! This line will render identically to the previous line.",
                "<div class=\"fab-info\">\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z\"/></svg>\n<div class=\"fab-content\">\n<p>This line will render identically to the next line.\nThis line will render identically to the previous line.</p>\n</div>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec3(string extensions)
        {
            //     --------------- Markdown ---------------
            //     ! These lines belong to the same FlexiAlertBlock.
            //      ! These lines belong to the same FlexiAlertBlock.
            //       ! These lines belong to the same FlexiAlertBlock.
            //        ! These lines belong to the same FlexiAlertBlock.
            //     --------------- Expected Markup ---------------
            //     <div class="fab-info">
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z"/></svg>
            //     <div class="fab-content">
            //     <p>These lines belong to the same FlexiAlertBlock.
            //     These lines belong to the same FlexiAlertBlock.
            //     These lines belong to the same FlexiAlertBlock.
            //     These lines belong to the same FlexiAlertBlock.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! These lines belong to the same FlexiAlertBlock.\n ! These lines belong to the same FlexiAlertBlock.\n  ! These lines belong to the same FlexiAlertBlock.\n   ! These lines belong to the same FlexiAlertBlock.",
                "<div class=\"fab-info\">\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z\"/></svg>\n<div class=\"fab-content\">\n<p>These lines belong to the same FlexiAlertBlock.\nThese lines belong to the same FlexiAlertBlock.\nThese lines belong to the same FlexiAlertBlock.\nThese lines belong to the same FlexiAlertBlock.</p>\n</div>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec4(string extensions)
        {
            //     --------------- Markdown ---------------
            //     ! This FlexiAlertBlock
            //     contains multiple
            //     lazy continuation lines.
            //     --------------- Expected Markup ---------------
            //     <div class="fab-info">
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z"/></svg>
            //     <div class="fab-content">
            //     <p>This FlexiAlertBlock
            //     contains multiple
            //     lazy continuation lines.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! This FlexiAlertBlock\ncontains multiple\nlazy continuation lines.",
                "<div class=\"fab-info\">\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z\"/></svg>\n<div class=\"fab-content\">\n<p>This FlexiAlertBlock\ncontains multiple\nlazy continuation lines.</p>\n</div>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec5(string extensions)
        {
            //     --------------- Markdown ---------------
            //     ! This is a FlexiAlertBlock.
            //     
            //     ! This is another FlexiAlertBlock.
            //     --------------- Expected Markup ---------------
            //     <div class="fab-info">
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z"/></svg>
            //     <div class="fab-content">
            //     <p>This is a FlexiAlertBlock.</p>
            //     </div>
            //     </div>
            //     <div class="fab-info">
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z"/></svg>
            //     <div class="fab-content">
            //     <p>This is another FlexiAlertBlock.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! This is a FlexiAlertBlock.\n\n! This is another FlexiAlertBlock.",
                "<div class=\"fab-info\">\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z\"/></svg>\n<div class=\"fab-content\">\n<p>This is a FlexiAlertBlock.</p>\n</div>\n</div>\n<div class=\"fab-info\">\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z\"/></svg>\n<div class=\"fab-content\">\n<p>This is another FlexiAlertBlock.</p>\n</div>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec6(string extensions)
        {
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "type": "warning"
            //     }
            //     ! This is a FlexiAlertBlock.
            //     --------------- Expected Markup ---------------
            //     <div class="fab-warning">
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="m1 21h22l-11-19-11 19zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"/></svg>
            //     <div class="fab-content">
            //     <p>This is a FlexiAlertBlock.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"type\": \"warning\"\n}\n! This is a FlexiAlertBlock.",
                "<div class=\"fab-warning\">\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"m1 21h22l-11-19-11 19zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z\"/></svg>\n<div class=\"fab-content\">\n<p>This is a FlexiAlertBlock.</p>\n</div>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec7(string extensions)
        {
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "iconMarkup": "<svg><use xlink:href=\"#alert-icon\"></use></svg>"
            //     }
            //     ! This is a FlexiAlertBlock.
            //     --------------- Expected Markup ---------------
            //     <div class="fab-info">
            //     <svg><use xlink:href="#alert-icon"></use></svg>
            //     <div class="fab-content">
            //     <p>This is a FlexiAlertBlock.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"iconMarkup\": \"<svg><use xlink:href=\\\"#alert-icon\\\"></use></svg>\"\n}\n! This is a FlexiAlertBlock.",
                "<div class=\"fab-info\">\n<svg><use xlink:href=\"#alert-icon\"></use></svg>\n<div class=\"fab-content\">\n<p>This is a FlexiAlertBlock.</p>\n</div>\n</div>",
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
            //         "classFormat": "alert-{0}"
            //     }
            //     ! This is a FlexiAlertBlock.
            //     --------------- Expected Markup ---------------
            //     <div class="alert-info">
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z"/></svg>
            //     <div class="fab-content">
            //     <p>This is a FlexiAlertBlock.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"classFormat\": \"alert-{0}\"\n}\n! This is a FlexiAlertBlock.",
                "<div class=\"alert-info\">\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z\"/></svg>\n<div class=\"fab-content\">\n<p>This is a FlexiAlertBlock.</p>\n</div>\n</div>",
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
            //         "contentClass": "alert-content"
            //     }
            //     ! This is a FlexiAlertBlock.
            //     --------------- Expected Markup ---------------
            //     <div class="fab-info">
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z"/></svg>
            //     <div class="alert-content">
            //     <p>This is a FlexiAlertBlock.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"contentClass\": \"alert-content\"\n}\n! This is a FlexiAlertBlock.",
                "<div class=\"fab-info\">\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z\"/></svg>\n<div class=\"alert-content\">\n<p>This is a FlexiAlertBlock.</p>\n</div>\n</div>",
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
            //         "attributes": {
            //             "id" : "info-1",
            //             "class" : "block"
            //         }
            //     }
            //     ! This is a FlexiAlertBlock.
            //     --------------- Expected Markup ---------------
            //     <div id="info-1" class="block fab-info">
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z"/></svg>
            //     <div class="fab-content">
            //     <p>This is a FlexiAlertBlock.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"attributes\": {\n        \"id\" : \"info-1\",\n        \"class\" : \"block\"\n    }\n}\n! This is a FlexiAlertBlock.",
                "<div id=\"info-1\" class=\"block fab-info\">\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z\"/></svg>\n<div class=\"fab-content\">\n<p>This is a FlexiAlertBlock.</p>\n</div>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiAlertBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec11(string extensions)
        {
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
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
            //     @{ "type": "closer-look" }
            //     ! This is a closer look at some topic.
            //     
            //     @{ "type": "help" }
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

            SpecTestHelper.AssertCompliance("@{ \"type\": \"closer-look\" }\n! This is a closer look at some topic.\n\n@{ \"type\": \"help\" }\n! This is a helpful tip.",
                "<div class=\"fab-closer-look\">\n<svg height=\"24\" viewBox=\"0 0 24 24\" width=\"24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M15.5 14h-.79l-.28-.27C15.41 12.59 16 11.11 16 9.5 16 5.91 13.09 3 9.5 3S3 5.91 3 9.5 5.91 16 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z\"/></svg>\n<div class=\"fab-content\">\n<p>This is a closer look at some topic.</p>\n</div>\n</div>\n<div class=\"fab-help\">\n<svg width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 17h-2v-2h2v2zm2.07-7.75l-.9.92C13.45 12.9 13 13.5 13 15h-2v-.5c0-1.1.45-2.1 1.17-2.83l1.24-1.26c.37-.36.59-.86.59-1.41 0-1.1-.9-2-2-2s-2 .9-2 2H8c0-2.21 1.79-4 4-4s4 1.79 4 4c0 .88-.36 1.68-.93 2.25z\"/></svg>\n<div class=\"fab-content\">\n<p>This is a helpful tip.</p>\n</div>\n</div>",
                extensions, 
                "{\n    \"flexiAlertBlocks\": {\n        \"iconMarkups\": {\n            \"closer-look\": \"<svg height=\\\"24\\\" viewBox=\\\"0 0 24 24\\\" width=\\\"24\\\" xmlns=\\\"http://www.w3.org/2000/svg\\\"><path d=\\\"M15.5 14h-.79l-.28-.27C15.41 12.59 16 11.11 16 9.5 16 5.91 13.09 3 9.5 3S3 5.91 3 9.5 5.91 16 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z\\\"/></svg>\",\n            \"help\": \"<svg width=\\\"24\\\" height=\\\"24\\\" viewBox=\\\"0 0 24 24\\\"><path d=\\\"M0 0h24v24H0z\\\" fill=\\\"none\\\"/><path d=\\\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 17h-2v-2h2v2zm2.07-7.75l-.9.92C13.45 12.9 13 13.5 13 15h-2v-.5c0-1.1.45-2.1 1.17-2.83l1.24-1.26c.37-.36.59-.86.59-1.41 0-1.1-.9-2-2-2s-2 .9-2 2H8c0-2.21 1.79-4 4-4s4 1.79 4 4c0 .88-.36 1.68-.93 2.25z\\\"/></svg>\"\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiAlertBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec12(string extensions)
        {
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiAlertBlocks": {
            //             "defaultBlockOptions": {
            //                 "iconMarkup": "<svg><use xlink:href=\"#alert-icon\"></use></svg>",
            //                 "classFormat": "alert-{0}",
            //                 "contentClass": "alert-content",
            //                 "attributes": {
            //                     "class": "block"
            //                 }
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     ! This is a FlexiAlertBlock.
            //     --------------- Expected Markup ---------------
            //     <div class="block alert-info">
            //     <svg><use xlink:href="#alert-icon"></use></svg>
            //     <div class="alert-content">
            //     <p>This is a FlexiAlertBlock.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! This is a FlexiAlertBlock.",
                "<div class=\"block alert-info\">\n<svg><use xlink:href=\"#alert-icon\"></use></svg>\n<div class=\"alert-content\">\n<p>This is a FlexiAlertBlock.</p>\n</div>\n</div>",
                extensions, 
                "{\n    \"flexiAlertBlocks\": {\n        \"defaultBlockOptions\": {\n            \"iconMarkup\": \"<svg><use xlink:href=\\\"#alert-icon\\\"></use></svg>\",\n            \"classFormat\": \"alert-{0}\",\n            \"contentClass\": \"alert-content\",\n            \"attributes\": {\n                \"class\": \"block\"\n            }\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiAlertBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiAlertBlocks_Spec13(string extensions)
        {
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiAlertBlocks": {
            //             "defaultBlockOptions": {
            //                 "classFormat": "alert-{0}"
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     ! This is a FlexiAlertBlock
            //     
            //     @{
            //         "classFormat": "special-alert-{0}"
            //     }
            //     ! This is a FlexiAlertBlock with block specific options.
            //     --------------- Expected Markup ---------------
            //     <div class="alert-info">
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z"/></svg>
            //     <div class="fab-content">
            //     <p>This is a FlexiAlertBlock</p>
            //     </div>
            //     </div>
            //     <div class="special-alert-info">
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z"/></svg>
            //     <div class="fab-content">
            //     <p>This is a FlexiAlertBlock with block specific options.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! This is a FlexiAlertBlock\n\n@{\n    \"classFormat\": \"special-alert-{0}\"\n}\n! This is a FlexiAlertBlock with block specific options.",
                "<div class=\"alert-info\">\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z\"/></svg>\n<div class=\"fab-content\">\n<p>This is a FlexiAlertBlock</p>\n</div>\n</div>\n<div class=\"special-alert-info\">\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z\"/></svg>\n<div class=\"fab-content\">\n<p>This is a FlexiAlertBlock with block specific options.</p>\n</div>\n</div>",
                extensions, 
                "{\n    \"flexiAlertBlocks\": {\n        \"defaultBlockOptions\": {\n            \"classFormat\": \"alert-{0}\"\n        }\n    }\n}");
        }
    }

    public class FlexiCodeBlocksSpecs
    {
        [Theory]
        [InlineData("FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec1(string extensions)
        {
            //     --------------- Markdown ---------------
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div>
            //     <header>
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
            //     </header>
            //     <pre><code>public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + &quot;dummyString&quot;;
            //     }</code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div>\n<header>\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z\"/></svg>\n</header>\n<pre><code>public string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + &quot;dummyString&quot;;\n}</code></pre>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec2(string extensions)
        {
            //     --------------- Markdown ---------------
            //         public string ExampleFunction(string arg)
            //         {
            //             // Example comment
            //             return arg + "dummyString";
            //         }
            //     --------------- Expected Markup ---------------
            //     <div>
            //     <header>
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
            //     </header>
            //     <pre><code>public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + &quot;dummyString&quot;;
            //     }</code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("    public string ExampleFunction(string arg)\n    {\n        // Example comment\n        return arg + \"dummyString\";\n    }",
                "<div>\n<header>\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z\"/></svg>\n</header>\n<pre><code>public string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + &quot;dummyString&quot;;\n}</code></pre>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec3(string extensions)
        {
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "copyIconMarkup": "<svg><use xlink:href=\"#material-design-copy\"></use></svg>"
            //     }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div>
            //     <header>
            //     <svg><use xlink:href="#material-design-copy"></use></svg>
            //     </header>
            //     <pre><code>public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + &quot;dummyString&quot;;
            //     }</code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"copyIconMarkup\": \"<svg><use xlink:href=\\\"#material-design-copy\\\"></use></svg>\"\n}\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div>\n<header>\n<svg><use xlink:href=\"#material-design-copy\"></use></svg>\n</header>\n<pre><code>public string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + &quot;dummyString&quot;;\n}</code></pre>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec4(string extensions)
        {
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "title": "ExampleDocument.cs"
            //     }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div>
            //     <header>
            //     <span>ExampleDocument.cs</span>
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
            //     </header>
            //     <pre><code>public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + &quot;dummyString&quot;;
            //     }</code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"title\": \"ExampleDocument.cs\"\n}\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div>\n<header>\n<span>ExampleDocument.cs</span>\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z\"/></svg>\n</header>\n<pre><code>public string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + &quot;dummyString&quot;;\n}</code></pre>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec5(string extensions)
        {
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "language": "csharp"
            //     }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div>
            //     <header>
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
            //     </header>
            //     <pre><code class="language-csharp"><span class="token keyword">public</span> <span class="token keyword">string</span> <span class="token function">ExampleFunction</span><span class="token punctuation">(</span><span class="token keyword">string</span> arg<span class="token punctuation">)</span>
            //     <span class="token punctuation">{</span>
            //         <span class="token comment">// Example comment</span>
            //         <span class="token keyword">return</span> arg <span class="token operator">+</span> <span class="token string">"dummyString"</span><span class="token punctuation">;</span>
            //     <span class="token punctuation">}</span></code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"language\": \"csharp\"\n}\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div>\n<header>\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z\"/></svg>\n</header>\n<pre><code class=\"language-csharp\"><span class=\"token keyword\">public</span> <span class=\"token keyword\">string</span> <span class=\"token function\">ExampleFunction</span><span class=\"token punctuation\">(</span><span class=\"token keyword\">string</span> arg<span class=\"token punctuation\">)</span>\n<span class=\"token punctuation\">{</span>\n    <span class=\"token comment\">// Example comment</span>\n    <span class=\"token keyword\">return</span> arg <span class=\"token operator\">+</span> <span class=\"token string\">\"dummyString\"</span><span class=\"token punctuation\">;</span>\n<span class=\"token punctuation\">}</span></code></pre>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec6(string extensions)
        {
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "codeClassFormat": "lang-{0}",
            //         "language": "csharp"
            //     }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div>
            //     <header>
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
            //     </header>
            //     <pre><code class="lang-csharp"><span class="token keyword">public</span> <span class="token keyword">string</span> <span class="token function">ExampleFunction</span><span class="token punctuation">(</span><span class="token keyword">string</span> arg<span class="token punctuation">)</span>
            //     <span class="token punctuation">{</span>
            //         <span class="token comment">// Example comment</span>
            //         <span class="token keyword">return</span> arg <span class="token operator">+</span> <span class="token string">"dummyString"</span><span class="token punctuation">;</span>
            //     <span class="token punctuation">}</span></code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"codeClassFormat\": \"lang-{0}\",\n    \"language\": \"csharp\"\n}\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div>\n<header>\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z\"/></svg>\n</header>\n<pre><code class=\"lang-csharp\"><span class=\"token keyword\">public</span> <span class=\"token keyword\">string</span> <span class=\"token function\">ExampleFunction</span><span class=\"token punctuation\">(</span><span class=\"token keyword\">string</span> arg<span class=\"token punctuation\">)</span>\n<span class=\"token punctuation\">{</span>\n    <span class=\"token comment\">// Example comment</span>\n    <span class=\"token keyword\">return</span> arg <span class=\"token operator\">+</span> <span class=\"token string\">\"dummyString\"</span><span class=\"token punctuation\">;</span>\n<span class=\"token punctuation\">}</span></code></pre>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec7(string extensions)
        {
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "syntaxHighlighter": "highlightJS",
            //         "language": "csharp",
            //     }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div>
            //     <header>
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
            //     </header>
            //     <pre><code class="language-csharp"><span class="hljs-function"><span class="hljs-keyword">public</span> <span class="hljs-keyword">string</span> <span class="hljs-title">ExampleFunction</span>(<span class="hljs-params"><span class="hljs-keyword">string</span> arg</span>)
            //     </span>{
            //         <span class="hljs-comment">// Example comment</span>
            //         <span class="hljs-keyword">return</span> arg + <span class="hljs-string">"dummyString"</span>;
            //     }</code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"syntaxHighlighter\": \"highlightJS\",\n    \"language\": \"csharp\",\n}\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div>\n<header>\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z\"/></svg>\n</header>\n<pre><code class=\"language-csharp\"><span class=\"hljs-function\"><span class=\"hljs-keyword\">public</span> <span class=\"hljs-keyword\">string</span> <span class=\"hljs-title\">ExampleFunction</span>(<span class=\"hljs-params\"><span class=\"hljs-keyword\">string</span> arg</span>)\n</span>{\n    <span class=\"hljs-comment\">// Example comment</span>\n    <span class=\"hljs-keyword\">return</span> arg + <span class=\"hljs-string\">\"dummyString\"</span>;\n}</code></pre>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec8(string extensions)
        {
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "language": "csharp",
            //         "syntaxHighlighter": "highlightJS",
            //         "highlightJSClassPrefix": "highlightjs-"
            //     }
            //     ```
            //     public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div>
            //     <header>
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
            //     </header>
            //     <pre><code class="language-csharp"><span class="highlightjs-function"><span class="highlightjs-keyword">public</span> <span class="highlightjs-keyword">string</span> <span class="highlightjs-title">ExampleFunction</span>(<span class="highlightjs-params"><span class="highlightjs-keyword">string</span> arg</span>)
            //     </span>{
            //         <span class="highlightjs-comment">// Example comment</span>
            //         <span class="highlightjs-keyword">return</span> arg + <span class="highlightjs-string">"dummyString"</span>;
            //     }</code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"language\": \"csharp\",\n    \"syntaxHighlighter\": \"highlightJS\",\n    \"highlightJSClassPrefix\": \"highlightjs-\"\n}\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div>\n<header>\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z\"/></svg>\n</header>\n<pre><code class=\"language-csharp\"><span class=\"highlightjs-function\"><span class=\"highlightjs-keyword\">public</span> <span class=\"highlightjs-keyword\">string</span> <span class=\"highlightjs-title\">ExampleFunction</span>(<span class=\"highlightjs-params\"><span class=\"highlightjs-keyword\">string</span> arg</span>)\n</span>{\n    <span class=\"highlightjs-comment\">// Example comment</span>\n    <span class=\"highlightjs-keyword\">return</span> arg + <span class=\"highlightjs-string\">\"dummyString\"</span>;\n}</code></pre>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec9(string extensions)
        {
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "lineNumberRanges": [
            //             {
            //                 "startLineNumber": 1,
            //                 "endLineNumber": 8,
            //                 "firstLineNumber": 1
            //             },
            //             {
            //                 "startLineNumber": 11,
            //                 "endLineNumber": -1,
            //                 "firstLineNumber": 32
            //             }
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
            //         // Some functions omitted for brevity
            //         ...
            //     
            //         public string ExampleFunction3(string arg)
            //         {
            //             // Example comment
            //             return arg + "dummyString";
            //         }
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div>
            //     <header>
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
            //     </header>
            //     <pre><code><span class="line"><span class="line-number">1</span><span class="line-text">public class ExampleClass</span></span>
            //     <span class="line"><span class="line-number">2</span><span class="line-text">{</span></span>
            //     <span class="line"><span class="line-number">3</span><span class="line-text">    public string ExampleFunction1(string arg)</span></span>
            //     <span class="line"><span class="line-number">4</span><span class="line-text">    {</span></span>
            //     <span class="line"><span class="line-number">5</span><span class="line-text">        // Example comment</span></span>
            //     <span class="line"><span class="line-number">6</span><span class="line-text">        return arg + &quot;dummyString&quot;;</span></span>
            //     <span class="line"><span class="line-number">7</span><span class="line-text">    }</span></span>
            //     <span class="line"><span class="line-number">8</span><span class="line-text"></span></span>
            //     <span class="line"><span class="line-text">    // Some functions omitted for brevity</span></span>
            //     <span class="line"><span class="line-text">    ...</span></span>
            //     <span class="line"><span class="line-number">32</span><span class="line-text"></span></span>
            //     <span class="line"><span class="line-number">33</span><span class="line-text">    public string ExampleFunction3(string arg)</span></span>
            //     <span class="line"><span class="line-number">34</span><span class="line-text">    {</span></span>
            //     <span class="line"><span class="line-number">35</span><span class="line-text">        // Example comment</span></span>
            //     <span class="line"><span class="line-number">36</span><span class="line-text">        return arg + &quot;dummyString&quot;;</span></span>
            //     <span class="line"><span class="line-number">37</span><span class="line-text">    }</span></span>
            //     <span class="line"><span class="line-number">38</span><span class="line-text">}</span></span></code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"lineNumberRanges\": [\n        {\n            \"startLineNumber\": 1,\n            \"endLineNumber\": 8,\n            \"firstLineNumber\": 1\n        },\n        {\n            \"startLineNumber\": 11,\n            \"endLineNumber\": -1,\n            \"firstLineNumber\": 32\n        }\n    ]\n}\n```\npublic class ExampleClass\n{\n    public string ExampleFunction1(string arg)\n    {\n        // Example comment\n        return arg + \"dummyString\";\n    }\n\n    // Some functions omitted for brevity\n    ...\n\n    public string ExampleFunction3(string arg)\n    {\n        // Example comment\n        return arg + \"dummyString\";\n    }\n}\n```",
                "<div>\n<header>\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z\"/></svg>\n</header>\n<pre><code><span class=\"line\"><span class=\"line-number\">1</span><span class=\"line-text\">public class ExampleClass</span></span>\n<span class=\"line\"><span class=\"line-number\">2</span><span class=\"line-text\">{</span></span>\n<span class=\"line\"><span class=\"line-number\">3</span><span class=\"line-text\">    public string ExampleFunction1(string arg)</span></span>\n<span class=\"line\"><span class=\"line-number\">4</span><span class=\"line-text\">    {</span></span>\n<span class=\"line\"><span class=\"line-number\">5</span><span class=\"line-text\">        // Example comment</span></span>\n<span class=\"line\"><span class=\"line-number\">6</span><span class=\"line-text\">        return arg + &quot;dummyString&quot;;</span></span>\n<span class=\"line\"><span class=\"line-number\">7</span><span class=\"line-text\">    }</span></span>\n<span class=\"line\"><span class=\"line-number\">8</span><span class=\"line-text\"></span></span>\n<span class=\"line\"><span class=\"line-text\">    // Some functions omitted for brevity</span></span>\n<span class=\"line\"><span class=\"line-text\">    ...</span></span>\n<span class=\"line\"><span class=\"line-number\">32</span><span class=\"line-text\"></span></span>\n<span class=\"line\"><span class=\"line-number\">33</span><span class=\"line-text\">    public string ExampleFunction3(string arg)</span></span>\n<span class=\"line\"><span class=\"line-number\">34</span><span class=\"line-text\">    {</span></span>\n<span class=\"line\"><span class=\"line-number\">35</span><span class=\"line-text\">        // Example comment</span></span>\n<span class=\"line\"><span class=\"line-number\">36</span><span class=\"line-text\">        return arg + &quot;dummyString&quot;;</span></span>\n<span class=\"line\"><span class=\"line-number\">37</span><span class=\"line-text\">    }</span></span>\n<span class=\"line\"><span class=\"line-number\">38</span><span class=\"line-text\">}</span></span></code></pre>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec10(string extensions)
        {
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "highlightLineRanges": [
            //             {
            //                 "startLineNumber": 1,
            //                 "endLineNumber": 1
            //             },
            //             {
            //                 "startLineNumber": 3,
            //                 "endLineNumber": 4
            //             }
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
            //     <div>
            //     <header>
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
            //     </header>
            //     <pre><code><span class="line highlight"><span class="line-text">public string ExampleFunction(string arg)</span></span>
            //     <span class="line"><span class="line-text">{</span></span>
            //     <span class="line highlight"><span class="line-text">    // Example comment</span></span>
            //     <span class="line highlight"><span class="line-text">    return arg + &quot;dummyString&quot;;</span></span>
            //     <span class="line"><span class="line-text">}</span></span></code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"highlightLineRanges\": [\n        {\n            \"startLineNumber\": 1,\n            \"endLineNumber\": 1\n        },\n        {\n            \"startLineNumber\": 3,\n            \"endLineNumber\": 4\n        }\n    ]\n}\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div>\n<header>\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z\"/></svg>\n</header>\n<pre><code><span class=\"line highlight\"><span class=\"line-text\">public string ExampleFunction(string arg)</span></span>\n<span class=\"line\"><span class=\"line-text\">{</span></span>\n<span class=\"line highlight\"><span class=\"line-text\">    // Example comment</span></span>\n<span class=\"line highlight\"><span class=\"line-text\">    return arg + &quot;dummyString&quot;;</span></span>\n<span class=\"line\"><span class=\"line-text\">}</span></span></code></pre>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec11(string extensions)
        {
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "attributes": {
            //             "id" : "code-1",
            //             "class" : "fcb"
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
            //     <div id="code-1" class="fcb">
            //     <header>
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
            //     </header>
            //     <pre><code>public string ExampleFunction(string arg)
            //     {
            //         // Example comment
            //         return arg + &quot;dummyString&quot;;
            //     }</code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"attributes\": {\n        \"id\" : \"code-1\",\n        \"class\" : \"fcb\"\n    }\n}\n```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div id=\"code-1\" class=\"fcb\">\n<header>\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z\"/></svg>\n</header>\n<pre><code>public string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + &quot;dummyString&quot;;\n}</code></pre>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec12(string extensions)
        {
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiCodeBlocks": {
            //             "defaultBlockOptions": {
            //                 "copyIconMarkup": "<svg><use xlink:href=\"#material-design-copy\"></use></svg>",
            //                 "title": "ExampleDocument.cs",
            //                 "language": "csharp",
            //                 "codeClassFormat": "lang-{0}",
            //                 "syntaxHighlighter": "highlightJS",
            //                 "highlightJSClassPrefix": "highlightjs-",
            //                 "lineNumberRanges": [{}],
            //                 "highlightLineRanges": [{}]
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
            //     --------------- Expected Markup ---------------
            //     <div>
            //     <header>
            //     <span>ExampleDocument.cs</span>
            //     <svg><use xlink:href="#material-design-copy"></use></svg>
            //     </header>
            //     <pre><code class="lang-csharp"><span class="line highlight"><span class="line-number">1</span><span class="line-text"><span class="highlightjs-function"><span class="highlightjs-keyword">public</span> <span class="highlightjs-keyword">string</span> <span class="highlightjs-title">ExampleFunction</span>(<span class="highlightjs-params"><span class="highlightjs-keyword">string</span> arg</span>)</span></span>
            //     <span class="line highlight"><span class="line-number">2</span><span class="line-text"></span>{</span></span>
            //     <span class="line highlight"><span class="line-number">3</span><span class="line-text">    <span class="highlightjs-comment">// Example comment</span></span></span>
            //     <span class="line highlight"><span class="line-number">4</span><span class="line-text">    <span class="highlightjs-keyword">return</span> arg + <span class="highlightjs-string">"dummyString"</span>;</span></span>
            //     <span class="line highlight"><span class="line-number">5</span><span class="line-text">}</span></span></code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("```\npublic string ExampleFunction(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div>\n<header>\n<span>ExampleDocument.cs</span>\n<svg><use xlink:href=\"#material-design-copy\"></use></svg>\n</header>\n<pre><code class=\"lang-csharp\"><span class=\"line highlight\"><span class=\"line-number\">1</span><span class=\"line-text\"><span class=\"highlightjs-function\"><span class=\"highlightjs-keyword\">public</span> <span class=\"highlightjs-keyword\">string</span> <span class=\"highlightjs-title\">ExampleFunction</span>(<span class=\"highlightjs-params\"><span class=\"highlightjs-keyword\">string</span> arg</span>)</span></span>\n<span class=\"line highlight\"><span class=\"line-number\">2</span><span class=\"line-text\"></span>{</span></span>\n<span class=\"line highlight\"><span class=\"line-number\">3</span><span class=\"line-text\">    <span class=\"highlightjs-comment\">// Example comment</span></span></span>\n<span class=\"line highlight\"><span class=\"line-number\">4</span><span class=\"line-text\">    <span class=\"highlightjs-keyword\">return</span> arg + <span class=\"highlightjs-string\">\"dummyString\"</span>;</span></span>\n<span class=\"line highlight\"><span class=\"line-number\">5</span><span class=\"line-text\">}</span></span></code></pre>\n</div>",
                extensions, 
                "{\n    \"flexiCodeBlocks\": {\n        \"defaultBlockOptions\": {\n            \"copyIconMarkup\": \"<svg><use xlink:href=\\\"#material-design-copy\\\"></use></svg>\",\n            \"title\": \"ExampleDocument.cs\",\n            \"language\": \"csharp\",\n            \"codeClassFormat\": \"lang-{0}\",\n            \"syntaxHighlighter\": \"highlightJS\",\n            \"highlightJSClassPrefix\": \"highlightjs-\",\n            \"lineNumberRanges\": [{}],\n            \"highlightLineRanges\": [{}]\n        }\n    }\n}");
        }

        [Theory]
        [InlineData("FlexiCodeBlocks_FlexiOptionsBlocks")]
        [InlineData("All")]
        public void FlexiCodeBlocks_Spec13(string extensions)
        {
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     --------------- Extension Options ---------------
            //     {
            //         "flexiCodeBlocks": {
            //             "defaultBlockOptions": {
            //                 "lineNumberRanges": [{}]
            //             }
            //         }
            //     }
            //     --------------- Markdown ---------------
            //     ```
            //     public string ExampleFunction1(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     
            //     @{
            //         "lineNumberRanges": [
            //             {
            //                 "firstLineNumber": 6
            //             }
            //         ]
            //     }
            //     ```
            //     public string ExampleFunction2(string arg)
            //     {
            //         // Example comment
            //         return arg + "dummyString";
            //     }
            //     ```
            //     --------------- Expected Markup ---------------
            //     <div>
            //     <header>
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
            //     </header>
            //     <pre><code><span class="line"><span class="line-number">1</span><span class="line-text">public string ExampleFunction1(string arg)</span></span>
            //     <span class="line"><span class="line-number">2</span><span class="line-text">{</span></span>
            //     <span class="line"><span class="line-number">3</span><span class="line-text">    // Example comment</span></span>
            //     <span class="line"><span class="line-number">4</span><span class="line-text">    return arg + &quot;dummyString&quot;;</span></span>
            //     <span class="line"><span class="line-number">5</span><span class="line-text">}</span></span></code></pre>
            //     </div>
            //     <div>
            //     <header>
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
            //     </header>
            //     <pre><code><span class="line"><span class="line-number">6</span><span class="line-text">public string ExampleFunction2(string arg)</span></span>
            //     <span class="line"><span class="line-number">7</span><span class="line-text">{</span></span>
            //     <span class="line"><span class="line-number">8</span><span class="line-text">    // Example comment</span></span>
            //     <span class="line"><span class="line-number">9</span><span class="line-text">    return arg + &quot;dummyString&quot;;</span></span>
            //     <span class="line"><span class="line-number">10</span><span class="line-text">}</span></span></code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("```\npublic string ExampleFunction1(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```\n\n@{\n    \"lineNumberRanges\": [\n        {\n            \"firstLineNumber\": 6\n        }\n    ]\n}\n```\npublic string ExampleFunction2(string arg)\n{\n    // Example comment\n    return arg + \"dummyString\";\n}\n```",
                "<div>\n<header>\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z\"/></svg>\n</header>\n<pre><code><span class=\"line\"><span class=\"line-number\">1</span><span class=\"line-text\">public string ExampleFunction1(string arg)</span></span>\n<span class=\"line\"><span class=\"line-number\">2</span><span class=\"line-text\">{</span></span>\n<span class=\"line\"><span class=\"line-number\">3</span><span class=\"line-text\">    // Example comment</span></span>\n<span class=\"line\"><span class=\"line-number\">4</span><span class=\"line-text\">    return arg + &quot;dummyString&quot;;</span></span>\n<span class=\"line\"><span class=\"line-number\">5</span><span class=\"line-text\">}</span></span></code></pre>\n</div>\n<div>\n<header>\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z\"/></svg>\n</header>\n<pre><code><span class=\"line\"><span class=\"line-number\">6</span><span class=\"line-text\">public string ExampleFunction2(string arg)</span></span>\n<span class=\"line\"><span class=\"line-number\">7</span><span class=\"line-text\">{</span></span>\n<span class=\"line\"><span class=\"line-number\">8</span><span class=\"line-text\">    // Example comment</span></span>\n<span class=\"line\"><span class=\"line-number\">9</span><span class=\"line-text\">    return arg + &quot;dummyString&quot;;</span></span>\n<span class=\"line\"><span class=\"line-number\">10</span><span class=\"line-text\">}</span></span></code></pre>\n</div>",
                extensions, 
                "{\n    \"flexiCodeBlocks\": {\n        \"defaultBlockOptions\": {\n            \"lineNumberRanges\": [{}]\n        }\n    }\n}");
        }
    }

    public class FlexiIncludeBlocksSpecs
    {
        [Theory]
        [InlineData("FlexiIncludeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec1(string extensions)
        {
            //     --------------- Markdown ---------------
            //     +{ "type": "Markdown", "sourceUri": "./exampleInclude.md" }
            //     --------------- Expected Markup ---------------
            //     <p>This is example markdown.</p>

            SpecTestHelper.AssertCompliance("+{ \"type\": \"Markdown\", \"sourceUri\": \"./exampleInclude.md\" }",
                "<p>This is example markdown.</p>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec2(string extensions)
        {
            //     --------------- Markdown ---------------
            //     +{
            //         "type": "Markdown",
            //         "sourceUri": "./exampleInclude.md"    
            //     }
            //     --------------- Expected Markup ---------------
            //     <p>This is example markdown.</p>

            SpecTestHelper.AssertCompliance("+{\n    \"type\": \"Markdown\",\n    \"sourceUri\": \"./exampleInclude.md\"    \n}",
                "<p>This is example markdown.</p>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec3(string extensions)
        {
            //     --------------- Markdown ---------------
            //     + {
            //         "type": "Markdown",
            //         "sourceUri": "./exampleInclude.md"    
            //     }
            //     --------------- Expected Markup ---------------
            //     <ul>
            //     <li>{
            //     &quot;type&quot;: &quot;Markdown&quot;,
            //     &quot;sourceUri&quot;: &quot;./exampleInclude.md&quot;<br />
            //     }</li>
            //     </ul>

            SpecTestHelper.AssertCompliance("+ {\n    \"type\": \"Markdown\",\n    \"sourceUri\": \"./exampleInclude.md\"    \n}",
                "<ul>\n<li>{\n&quot;type&quot;: &quot;Markdown&quot;,\n&quot;sourceUri&quot;: &quot;./exampleInclude.md&quot;<br />\n}</li>\n</ul>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec4(string extensions)
        {
            //     --------------- Markdown ---------------
            //     +{
            //         "type": "markdown",
            //         "sourceUri": "https://raw.githubusercontent.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/6998b1c27821d8393ad39beb54f782515c39d98b/test/FlexiBlocks.Tests/exampleInclude.md"
            //     }
            //     --------------- Expected Markup ---------------
            //     <p>This is example markdown.</p>

            SpecTestHelper.AssertCompliance("+{\n    \"type\": \"markdown\",\n    \"sourceUri\": \"https://raw.githubusercontent.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/6998b1c27821d8393ad39beb54f782515c39d98b/test/FlexiBlocks.Tests/exampleInclude.md\"\n}",
                "<p>This is example markdown.</p>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec5(string extensions)
        {
            //     --------------- Markdown ---------------
            //     +{
            //         "type": "markdown",
            //         "baseUri": "https://raw.githubusercontent.com",
            //         "sourceUri": "JeremyTCD/Markdig.Extensions.FlexiBlocks/6998b1c27821d8393ad39beb54f782515c39d98b/test/FlexiBlocks.Tests/exampleInclude.md"
            //     }
            //     --------------- Expected Markup ---------------
            //     <p>This is example markdown.</p>

            SpecTestHelper.AssertCompliance("+{\n    \"type\": \"markdown\",\n    \"baseUri\": \"https://raw.githubusercontent.com\",\n    \"sourceUri\": \"JeremyTCD/Markdig.Extensions.FlexiBlocks/6998b1c27821d8393ad39beb54f782515c39d98b/test/FlexiBlocks.Tests/exampleInclude.md\"\n}",
                "<p>This is example markdown.</p>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec6(string extensions)
        {
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     +{
            //         "type": "Code",
            //         "sourceUri": "./exampleInclude.js"
            //     }
            //     --------------- Expected Markup ---------------
            //     <div>
            //     <header>
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
            //     </header>
            //     <pre><code>function exampleFunction(arg) {
            //         // Example comment
            //         return arg + 'dummyString';
            //     }
            //     
            //     //#region utility methods
            //     function add(a, b) {
            //         return a + b;
            //     }
            //     //#endregion utility methods</code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("+{\n    \"type\": \"Code\",\n    \"sourceUri\": \"./exampleInclude.js\"\n}",
                "<div>\n<header>\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z\"/></svg>\n</header>\n<pre><code>function exampleFunction(arg) {\n    // Example comment\n    return arg + 'dummyString';\n}\n\n//#region utility methods\nfunction add(a, b) {\n    return a + b;\n}\n//#endregion utility methods</code></pre>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec7(string extensions)
        {
            //     --------------- Markdown ---------------
            //     +{
            //         "cacheOnDisk": false,
            //         "type": "markdown",
            //         "sourceURI": "https://raw.githubusercontent.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/6998b1c27821d8393ad39beb54f782515c39d98b/test/FlexiBlocks.Tests/exampleInclude.md"
            //     }
            //     --------------- Expected Markup ---------------
            //     <p>This is example markdown.</p>

            SpecTestHelper.AssertCompliance("+{\n    \"cacheOnDisk\": false,\n    \"type\": \"markdown\",\n    \"sourceURI\": \"https://raw.githubusercontent.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/6998b1c27821d8393ad39beb54f782515c39d98b/test/FlexiBlocks.Tests/exampleInclude.md\"\n}",
                "<p>This is example markdown.</p>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec8(string extensions)
        {
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     +{
            //         "sourceUri": "./exampleInclude.js",
            //         "clippings":[{"endLineNumber": 4}, {"startLineNumber": 7, "endLineNumber": 9}]
            //     }
            //     --------------- Expected Markup ---------------
            //     <div>
            //     <header>
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
            //     </header>
            //     <pre><code>function exampleFunction(arg) {
            //         // Example comment
            //         return arg + 'dummyString';
            //     }
            //     function add(a, b) {
            //         return a + b;
            //     }</code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("+{\n    \"sourceUri\": \"./exampleInclude.js\",\n    \"clippings\":[{\"endLineNumber\": 4}, {\"startLineNumber\": 7, \"endLineNumber\": 9}]\n}",
                "<div>\n<header>\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z\"/></svg>\n</header>\n<pre><code>function exampleFunction(arg) {\n    // Example comment\n    return arg + 'dummyString';\n}\nfunction add(a, b) {\n    return a + b;\n}</code></pre>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec9(string extensions)
        {
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     +{
            //         "sourceUri": "./exampleInclude.js",
            //         "clippings":[{"startDemarcationLineSubstring": "#region utility methods", "endDemarcationLineSubstring": "#endregion utility methods"}]
            //     }
            //     --------------- Expected Markup ---------------
            //     <div>
            //     <header>
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
            //     </header>
            //     <pre><code>function add(a, b) {
            //         return a + b;
            //     }</code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("+{\n    \"sourceUri\": \"./exampleInclude.js\",\n    \"clippings\":[{\"startDemarcationLineSubstring\": \"#region utility methods\", \"endDemarcationLineSubstring\": \"#endregion utility methods\"}]\n}",
                "<div>\n<header>\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z\"/></svg>\n</header>\n<pre><code>function add(a, b) {\n    return a + b;\n}</code></pre>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec10(string extensions)
        {
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     +{
            //         "sourceUri": "./exampleInclude.js",
            //         "clippings":[{"startLineNumber": 7, "endDemarcationLineSubstring": "#endregion utility methods"}]
            //     }
            //     --------------- Expected Markup ---------------
            //     <div>
            //     <header>
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
            //     </header>
            //     <pre><code>function add(a, b) {
            //         return a + b;
            //     }</code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("+{\n    \"sourceUri\": \"./exampleInclude.js\",\n    \"clippings\":[{\"startLineNumber\": 7, \"endDemarcationLineSubstring\": \"#endregion utility methods\"}]\n}",
                "<div>\n<header>\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z\"/></svg>\n</header>\n<pre><code>function add(a, b) {\n    return a + b;\n}</code></pre>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec11(string extensions)
        {
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     +{
            //         "sourceUri": "./exampleInclude.js",
            //         "clippings":[{
            //             "endLineNumber": 1,
            //             "afterContent": "..."
            //         },
            //         {
            //             "startLineNumber": 4,
            //             "endLineNumber": 4
            //         },
            //         {
            //             "startLineNumber": 7, 
            //             "endLineNumber": 7,
            //             "beforeContent": ""
            //         },
            //         {
            //             "startLineNumber": 9, 
            //             "endLineNumber": 9,
            //             "beforeContent": "..."
            //         }]
            //     }
            //     --------------- Expected Markup ---------------
            //     <div>
            //     <header>
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
            //     </header>
            //     <pre><code>function exampleFunction(arg) {
            //     ...
            //     }
            //     
            //     function add(a, b) {
            //     ...
            //     }</code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("+{\n    \"sourceUri\": \"./exampleInclude.js\",\n    \"clippings\":[{\n        \"endLineNumber\": 1,\n        \"afterContent\": \"...\"\n    },\n    {\n        \"startLineNumber\": 4,\n        \"endLineNumber\": 4\n    },\n    {\n        \"startLineNumber\": 7, \n        \"endLineNumber\": 7,\n        \"beforeContent\": \"\"\n    },\n    {\n        \"startLineNumber\": 9, \n        \"endLineNumber\": 9,\n        \"beforeContent\": \"...\"\n    }]\n}",
                "<div>\n<header>\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z\"/></svg>\n</header>\n<pre><code>function exampleFunction(arg) {\n...\n}\n\nfunction add(a, b) {\n...\n}</code></pre>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec12(string extensions)
        {
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     +{
            //         "sourceUri": "./exampleInclude.js",
            //         "clippings":[{"dedentLength": 2}],
            //     }
            //     --------------- Expected Markup ---------------
            //     <div>
            //     <header>
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
            //     </header>
            //     <pre><code>function exampleFunction(arg) {
            //       // Example comment
            //       return arg + 'dummyString';
            //     }
            //     
            //     //#region utility methods
            //     function add(a, b) {
            //       return a + b;
            //     }
            //     //#endregion utility methods</code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("+{\n    \"sourceUri\": \"./exampleInclude.js\",\n    \"clippings\":[{\"dedentLength\": 2}],\n}",
                "<div>\n<header>\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z\"/></svg>\n</header>\n<pre><code>function exampleFunction(arg) {\n  // Example comment\n  return arg + 'dummyString';\n}\n\n//#region utility methods\nfunction add(a, b) {\n  return a + b;\n}\n//#endregion utility methods</code></pre>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec13(string extensions)
        {
            //     --------------- Extra Extensions ---------------
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     +{
            //         "sourceUri": "./exampleInclude.js",
            //         "clippings":[{"collapseRatio": 0.5}]
            //     }
            //     --------------- Expected Markup ---------------
            //     <div>
            //     <header>
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
            //     </header>
            //     <pre><code>function exampleFunction(arg) {
            //       // Example comment
            //       return arg + 'dummyString';
            //     }
            //     
            //     //#region utility methods
            //     function add(a, b) {
            //       return a + b;
            //     }
            //     //#endregion utility methods</code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("+{\n    \"sourceUri\": \"./exampleInclude.js\",\n    \"clippings\":[{\"collapseRatio\": 0.5}]\n}",
                "<div>\n<header>\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z\"/></svg>\n</header>\n<pre><code>function exampleFunction(arg) {\n  // Example comment\n  return arg + 'dummyString';\n}\n\n//#region utility methods\nfunction add(a, b) {\n  return a + b;\n}\n//#endregion utility methods</code></pre>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks_FlexiOptionsBlocks_FlexiCodeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec14(string extensions)
        {
            //     --------------- Extra Extensions ---------------
            //     FlexiOptionsBlocks
            //     FlexiCodeBlocks
            //     --------------- Markdown ---------------
            //     @{
            //         "language": "javascript"
            //     }
            //     +{
            //         "sourceUri": "./exampleInclude.js"
            //     }
            //     --------------- Expected Markup ---------------
            //     <div>
            //     <header>
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z"/></svg>
            //     </header>
            //     <pre><code class="language-javascript"><span class="token keyword">function</span> <span class="token function">exampleFunction</span><span class="token punctuation">(</span>arg<span class="token punctuation">)</span> <span class="token punctuation">{</span>
            //         <span class="token comment">// Example comment</span>
            //         <span class="token keyword">return</span> arg <span class="token operator">+</span> <span class="token string">'dummyString'</span><span class="token punctuation">;</span>
            //     <span class="token punctuation">}</span>
            //     
            //     <span class="token comment">//#region utility methods</span>
            //     <span class="token keyword">function</span> <span class="token function">add</span><span class="token punctuation">(</span>a<span class="token punctuation">,</span> b<span class="token punctuation">)</span> <span class="token punctuation">{</span>
            //         <span class="token keyword">return</span> a <span class="token operator">+</span> b<span class="token punctuation">;</span>
            //     <span class="token punctuation">}</span>
            //     <span class="token comment">//#endregion utility methods</span></code></pre>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"language\": \"javascript\"\n}\n+{\n    \"sourceUri\": \"./exampleInclude.js\"\n}",
                "<div>\n<header>\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"M16,1H2v16h2V3h12V1z M15,5l6,6v12H6V5H15z M14,12h5.5L14,6.5V12z\"/></svg>\n</header>\n<pre><code class=\"language-javascript\"><span class=\"token keyword\">function</span> <span class=\"token function\">exampleFunction</span><span class=\"token punctuation\">(</span>arg<span class=\"token punctuation\">)</span> <span class=\"token punctuation\">{</span>\n    <span class=\"token comment\">// Example comment</span>\n    <span class=\"token keyword\">return</span> arg <span class=\"token operator\">+</span> <span class=\"token string\">'dummyString'</span><span class=\"token punctuation\">;</span>\n<span class=\"token punctuation\">}</span>\n\n<span class=\"token comment\">//#region utility methods</span>\n<span class=\"token keyword\">function</span> <span class=\"token function\">add</span><span class=\"token punctuation\">(</span>a<span class=\"token punctuation\">,</span> b<span class=\"token punctuation\">)</span> <span class=\"token punctuation\">{</span>\n    <span class=\"token keyword\">return</span> a <span class=\"token operator\">+</span> b<span class=\"token punctuation\">;</span>\n<span class=\"token punctuation\">}</span>\n<span class=\"token comment\">//#endregion utility methods</span></code></pre>\n</div>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec15(string extensions)
        {
            //     --------------- Markdown ---------------
            //     +{
            //         "type": "Markdown",
            //         "sourceUri": "./exampleIncludeWithNestedInclude.md"
            //     }
            //     --------------- Expected Markup ---------------
            //     <p>This is example markdown with an include.</p>
            //     <p>This is example markdown.</p>

            SpecTestHelper.AssertCompliance("+{\n    \"type\": \"Markdown\",\n    \"sourceUri\": \"./exampleIncludeWithNestedInclude.md\"\n}",
                "<p>This is example markdown with an include.</p>\n<p>This is example markdown.</p>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec16(string extensions)
        {
            //     --------------- Markdown ---------------
            //     - First item.
            //     - Second item  
            //     
            //       +{
            //           "type": "Markdown",
            //           "sourceUri": "./exampleInclude.md"
            //       }
            //     - Third item
            //     --------------- Expected Markup ---------------
            //     <ul>
            //     <li><p>First item.</p></li>
            //     <li><p>Second item</p>
            //     <p>This is example markdown.</p></li>
            //     <li><p>Third item</p></li>
            //     </ul>

            SpecTestHelper.AssertCompliance("- First item.\n- Second item  \n\n  +{\n      \"type\": \"Markdown\",\n      \"sourceUri\": \"./exampleInclude.md\"\n  }\n- Third item",
                "<ul>\n<li><p>First item.</p></li>\n<li><p>Second item</p>\n<p>This is example markdown.</p></li>\n<li><p>Third item</p></li>\n</ul>",
                extensions);
        }

        [Theory]
        [InlineData("FlexiIncludeBlocks")]
        [InlineData("All")]
        public void FlexiIncludeBlocks_Spec17(string extensions)
        {
            //     --------------- Markdown ---------------
            //     > First line.
            //     > +{
            //     >     "type": "Markdown",
            //     >     "sourceUri": "./exampleInclude.md"
            //     > }
            //     > Third line
            //     --------------- Expected Markup ---------------
            //     <blockquote>
            //     <p>First line.</p>
            //     <p>This is example markdown.</p>
            //     <p>Third line</p>
            //     </blockquote>

            SpecTestHelper.AssertCompliance("> First line.\n> +{\n>     \"type\": \"Markdown\",\n>     \"sourceUri\": \"./exampleInclude.md\"\n> }\n> Third line",
                "<blockquote>\n<p>First line.</p>\n<p>This is example markdown.</p>\n<p>Third line</p>\n</blockquote>",
                extensions);
        }
    }
}

