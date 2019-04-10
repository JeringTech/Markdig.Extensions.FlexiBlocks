# FlexiAlerts
FlexiAlerts contain content you'd like to draw readers attention to, such as warnings and important information.

## Usage
```csharp
using Markdig;
using Jering.Markdig.Extensions.FlexiBlocks;

...
var markdownPipelineBuilder = new MarkdownPipelineBuilder();
markdownPipelineBuilder.UseFlexiAlerts(/* Optional extension options */);

MarkdownPipeline markdownPipeline = markdownPipelinBuilder.Build();

string markdown = "! This is a FlexiAlert."
string html = Markdown.ToHtml(markdown, markdownPipeline);
string expectedHtml = "<div class=\"flexi-alert flexi-alert_info\">
<svg class=\"flexi-alert__icon\" xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"/></svg>
<div class=\"flexi-alert__content\">
<p>This is a FlexiAlert.
This is important information.</p>
</div>
</div>";

Assert.Equal(expectedHtml, html)
```

## Basic Syntax
The markdown for a FlexiAlert is a sequence of lines each starting with `!`. For example:

```````````````````````````````` none
--------------- Markdown ---------------
! This is a FlexiAlert.
! This is important information.
--------------- Expected Markup ---------------
<div class="flexi-alert flexi-alert_info">
<svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
<div class="flexi-alert__content">
<p>This is a FlexiAlert.
This is important information.</p>
</div>
</div>
````````````````````````````````

! By default, a FlexiAlert has icon and content elements. These elements, along with a FlexiAlert's root element, are assigned default classes. Default classes comply with 
! [BEM methodology](https://en.bem.info/).  
!
! FlexiAlerts can be customized, we'll explain how in [a bit](#type).

The first space after the starting `!` of each line is optional:

```````````````````````````````` none
--------------- Markdown ---------------
!This line will render identically to the next line.
! This line will render identically to the previous line.
--------------- Expected Markup ---------------
<div class="flexi-alert flexi-alert_info">
<svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
<div class="flexi-alert__content">
<p>This line will render identically to the next line.
This line will render identically to the previous line.</p>
</div>
</div>
````````````````````````````````

Starting `!`s can be preceded by up to three spaces:

```````````````````````````````` none
--------------- Markdown ---------------
! These lines belong to the same FlexiAlert.
 ! These lines belong to the same FlexiAlert.
  ! These lines belong to the same FlexiAlert.
   ! These lines belong to the same FlexiAlert.
--------------- Expected Markup ---------------
<div class="flexi-alert flexi-alert_info">
<svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
<div class="flexi-alert__content">
<p>These lines belong to the same FlexiAlert.
These lines belong to the same FlexiAlert.
These lines belong to the same FlexiAlert.
These lines belong to the same FlexiAlert.</p>
</div>
</div>
````````````````````````````````

[Lazy continuation lines](https://spec.commonmark.org/0.28/#lazy-continuation-line) are allowed within a FlexiAlert:

```````````````````````````````` none
--------------- Markdown ---------------
! This FlexiAlert
contains multiple
lazy continuation lines.
--------------- Expected Markup ---------------
<div class="flexi-alert flexi-alert_info">
<svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
<div class="flexi-alert__content">
<p>This FlexiAlert
contains multiple
lazy continuation lines.</p>
</div>
</div>
````````````````````````````````

A blank line closes a FlexiAlert:

```````````````````````````````` none
--------------- Markdown ---------------
! This is a FlexiAlert.

! This is another FlexiAlert.
--------------- Expected Markup ---------------
<div class="flexi-alert flexi-alert_info">
<svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
<div class="flexi-alert__content">
<p>This is a FlexiAlert.</p>
</div>
</div>
<div class="flexi-alert flexi-alert_info">
<svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
<div class="flexi-alert__content">
<p>This is another FlexiAlert.</p>
</div>
</div>
````````````````````````````````

## Options

### `FlexiAlertOptions`
Options for a FlexiAlert. To specify `FlexiAlertOptions` for a FlexiAlert, the [Options](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/OptionsSpecs.md#options) extension must be enabled.

#### Properties
##### `BlockName`
  - Type: `string`
  - Description: The `FlexiAlert`'s [BEM block name](https://en.bem.info/methodology/naming-convention/#block-name).
    In compliance with [BEM methodology](https://en.bem.info), this value is the root element's class as well as the prefix for all other classes.
    This value should contain only valid [CSS class characters](https://www.w3.org/TR/CSS21/syndata.html#characters).
    If this value is `null`, the `FlexiAlert` has no classes.
  - Default: "flexi-alert"
  - Usage:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    BlockOptions
    --------------- Markdown ---------------
    @{
        "blockName": "alert"
    }
    ! This is a FlexiAlert.
    --------------- Expected Markup ---------------
    <div class="alert alert_info">
    <svg class="alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
    <div class="alert__content">
    <p>This is a FlexiAlert.</p>
    </div>
    </div>
    ````````````````````````````````

##### `Type`
  - Type: `string`
  - Description: The `FlexiAlert`'s type.
    This value is used in the root element's default [modifier class](https://en.bem.info/methodology/quick-start/#modifier),
"<`BlockName`>_<`Type`>".
    This value is also used to retrieve an icon HTML fragment if `IconHtmlFragment` is null.
    Icon HTML fragments for custom types can be defined in `FlexiAlertsExtensionOptions.IconHtmlFragments`, which contains fragments for types "info",
"warning" and "critical-warning" by default.
    This value should contain only valid [CSS class characters](https://www.w3.org/TR/CSS21/syndata.html#characters).
    If this value is `null`, the root element will have no modifier class and no attempt will be made to retrieve an icon HTML fragment.
  - Default: "info"
  - Usage:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    BlockOptions
    --------------- Markdown ---------------
    @{
        "type": "warning"
    }
    ! This is a FlexiAlert.
    --------------- Expected Markup ---------------
    <div class="flexi-alert flexi-alert_warning">
    <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"/></svg>
    <div class="flexi-alert__content">
    <p>This is a FlexiAlert.</p>
    </div>
    </div>
    ````````````````````````````````

##### `IconHtmlFragment`
  - Type: `string`
  - Description: The `FlexiAlert`'s icon as a HTML fragment.
    A class attribute with value "<`BlockName`>__icon" is added to this fragment's first start tag.
    If this value is `null`, an attempt is made to retrieve a HTML fragment for the `FlexiAlert`'s type from
`FlexiAlertsExtensionOptions.IconHtmlFragments`, failing which, no icon is rendered.
  - Default: `null`
  - Usage:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    BlockOptions
    --------------- Markdown ---------------
    @{
        "iconHtmlFragment": "<svg><use xlink:href=\"#alert-icon\"></use></svg>"
    }
    ! This is a FlexiAlert.
    --------------- Expected Markup ---------------
    <div class="flexi-alert flexi-alert_info">
    <svg class="flexi-alert__icon"><use xlink:href="#alert-icon"></use></svg>
    <div class="flexi-alert__content">
    <p>This is a FlexiAlert.</p>
    </div>
    </div>
    ````````````````````````````````

##### `Attributes`
  - Type: `IDictionary<string, string>`
  - Description: The HTML attributes for the `FlexiAlert`'s root element.
    If the class attribute is specified, its value is appended to default classes. This facilitates [BEM mixes](https://en.bem.info/methodology/quick-start/#mix).
    If this value is `null`, no attributes are renderered other than class with default classes.
  - Default: `null`
  - Usage:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    BlockOptions
    --------------- Markdown ---------------
    @{
        "attributes": {
            "id" : "my-custom-id",
            "class" : "my-custom-class"
        }
    }
    ! This is a FlexiAlert.
    --------------- Expected Markup ---------------
    <div id="my-custom-id" class="flexi-alert flexi-alert_info my-custom-class">
    <svg class="flexi-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
    <div class="flexi-alert__content">
    <p>This is a FlexiAlert.</p>
    </div>
    </div>
    ````````````````````````````````

### `FlexiAlertsExtensionOptions`
Options for the FlexiAlerts extension. There are two ways to specify these options:
- Pass a `FlexiAlertsExtensionsOptions` when calling `MarkdownPipelineBuilderExtensions.UseFlexiAlerts(this MarkdownPipelineBuilder pipelineBuilder, IFlexiAlertsExtensionOptions options)`.
- Insert a `FlexiAlertsExtensionOptions` into a `MarkdownParserContext.Properties` with key `typeof(IFlexiAlertsExtensionOptions)`. Pass the `MarkdownParserContext` when you call a markdown processing method,
  for example, `Markdown.ToHtml(markdown, markdownPipeline, yourMarkdownParserContext)`.  
  This method allows you to specify different extension options when *reusing* pipelines. Options specified using this method take precedence.

#### Constructor Parameters
##### `defaultBlockOptions`
  - Type: `IFlexiAlertOptions`
  - Description: Default `IFlexiAlertOptions` for all `FlexiAlert`s.
    If this value is null, a `FlexiAlertOptions` with default values is used.
  - Default: null
  - Usage:
    ```````````````````````````````` none
    --------------- Extension Options ---------------
    {
        "flexiAlerts": {
            "defaultBlockOptions": {
                "$type": "Jering.Markdig.Extensions.FlexiBlocks.FlexiAlerts.FlexiAlertOptions, Jering.Markdig.Extensions.FlexiBlocks",
                "iconHtmlFragment": "<svg><use xlink:href=\"#alert-icon\"></use></svg>",
                "attributes": {
                    "class": "block"
                }
            }
        }
    }
    --------------- Markdown ---------------
    ! This is a FlexiAlert.
    --------------- Expected Markup ---------------
    <div class="flexi-alert flexi-alert_info block">
    <svg class="flexi-alert__icon"><use xlink:href="#alert-icon"></use></svg>
    <div class="flexi-alert__content">
    <p>This is a FlexiAlert.</p>
    </div>
    </div>
    ````````````````````````````````

    `defaultBlockOptions` has lower precedence than block specific options:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    BlockOptions
    --------------- Extension Options ---------------
    {
        "flexiAlerts": {
            "defaultBlockOptions": {
                "blockName": "alert"
            }
        }
    }
    --------------- Markdown ---------------
    ! This is a FlexiAlert

    @{
        "blockName": "special-alert"
    }
    ! This is a FlexiAlert with block specific options.
    --------------- Expected Markup ---------------
    <div class="alert alert_info">
    <svg class="alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
    <div class="alert__content">
    <p>This is a FlexiAlert</p>
    </div>
    </div>
    <div class="special-alert special-alert_info">
    <svg class="special-alert__icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
    <div class="special-alert__content">
    <p>This is a FlexiAlert with block specific options.</p>
    </div>
    </div>
    ````````````````````````````````

##### `iconHtmlFragments`
  - Type: `IDictionary<string, string>`
  - Description: A map of `FlexiAlert` types to icon HTML fragments.
    If this value is null, a map of icon HTML fragments containing types "info", "warning" and "critical-warning" is used.
  - Default: null
  - Usage:
    ```````````````````````````````` none
    --------------- Extra Extensions ---------------
    BlockOptions
    --------------- Extension Options ---------------
    {
        "flexiAlerts": {
            "iconHtmlFragments": {
                "closer-look": "<svg height=\"24\" viewBox=\"0 0 24 24\" width=\"24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M15.5 14h-.79l-.28-.27C15.41 12.59 16 11.11 16 9.5 16 5.91 13.09 3 9.5 3S3 5.91 3 9.5 5.91 16 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z\"/></svg>",
                "help": "<svg width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 17h-2v-2h2v2zm2.07-7.75l-.9.92C13.45 12.9 13 13.5 13 15h-2v-.5c0-1.1.45-2.1 1.17-2.83l1.24-1.26c.37-.36.59-.86.59-1.41 0-1.1-.9-2-2-2s-2 .9-2 2H8c0-2.21 1.79-4 4-4s4 1.79 4 4c0 .88-.36 1.68-.93 2.25z\"/></svg>"
            }
        }
    }
    --------------- Markdown ---------------
    @{ "type": "closer-look" }
    ! This is a closer look at some topic.

    @{ "type": "help" }
    ! This is a helpful tip.
    --------------- Expected Markup ---------------
    <div class="flexi-alert flexi-alert_closer-look">
    <svg class="flexi-alert__icon" height="24" viewBox="0 0 24 24" width="24" xmlns="http://www.w3.org/2000/svg"><path d="M15.5 14h-.79l-.28-.27C15.41 12.59 16 11.11 16 9.5 16 5.91 13.09 3 9.5 3S3 5.91 3 9.5 5.91 16 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z"/></svg>
    <div class="flexi-alert__content">
    <p>This is a closer look at some topic.</p>
    </div>
    </div>
    <div class="flexi-alert flexi-alert_help">
    <svg class="flexi-alert__icon" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 17h-2v-2h2v2zm2.07-7.75l-.9.92C13.45 12.9 13 13.5 13 15h-2v-.5c0-1.1.45-2.1 1.17-2.83l1.24-1.26c.37-.36.59-.86.59-1.41 0-1.1-.9-2-2-2s-2 .9-2 2H8c0-2.21 1.79-4 4-4s4 1.79 4 4c0 .88-.36 1.68-.93 2.25z"/></svg>
    <div class="flexi-alert__content">
    <p>This is a helpful tip.</p>
    </div>
    </div>
    ````````````````````````````````