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
            //     !This line will be the same as the next line.
            //     ! This line will be the same as the previous line.
            //     --------------- Expected Markup ---------------
            //     <div class="fab-info">
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z"/></svg>
            //     <div class="fab-content">
            //     <p>This line will be the same as the next line.
            //     This line will be the same as the previous line.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("!This line will be the same as the next line.\n! This line will be the same as the previous line.",
                "<div class=\"fab-info\">\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z\"/></svg>\n<div class=\"fab-content\">\n<p>This line will be the same as the next line.\nThis line will be the same as the previous line.</p>\n</div>\n</div>",
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
            //         "iconMarkup": "<svg><use xlink:href=\"#alternative-icon-markup\"></use></svg>"
            //     }
            //     ! This is a FlexiAlertBlock.
            //     --------------- Expected Markup ---------------
            //     <div class="fab-info">
            //     <svg><use xlink:href="#alternative-icon-markup"></use></svg>
            //     <div class="fab-content">
            //     <p>This is a FlexiAlertBlock.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"iconMarkup\": \"<svg><use xlink:href=\\\"#alternative-icon-markup\\\"></use></svg>\"\n}\n! This is a FlexiAlertBlock.",
                "<div class=\"fab-info\">\n<svg><use xlink:href=\"#alternative-icon-markup\"></use></svg>\n<div class=\"fab-content\">\n<p>This is a FlexiAlertBlock.</p>\n</div>\n</div>",
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
        public void FlexiAlertBlocks_Spec8(string extensions)
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
        public void FlexiAlertBlocks_Spec9(string extensions)
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
            //     <div class="fab-info">
            //     <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z"/></svg>
            //     <div class="fab-content">
            //     <p>This is a FlexiAlertBlock.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("@{\n    \"attributes\": {\n        \"id\" : \"info-1\",\n        \"class\" : \"block\"\n    }\n}\n! This is a FlexiAlertBlock.",
                "<div class=\"fab-info\">\n<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z\"/></svg>\n<div class=\"fab-content\">\n<p>This is a FlexiAlertBlock.</p>\n</div>\n</div>",
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
            //     <div class="alert-info">
            //     <svg><use xlink:href="#alert-icon"></use></svg>
            //     <div class="alert-content">
            //     <p>This is a FlexiAlertBlock.</p>
            //     </div>
            //     </div>

            SpecTestHelper.AssertCompliance("! This is a FlexiAlertBlock.",
                "<div class=\"alert-info\">\n<svg><use xlink:href=\"#alert-icon\"></use></svg>\n<div class=\"alert-content\">\n<p>This is a FlexiAlertBlock.</p>\n</div>\n</div>",
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
}

