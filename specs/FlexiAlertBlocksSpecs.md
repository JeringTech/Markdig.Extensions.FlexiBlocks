## FlexiAlertBlocks
FlexiAlertBlocks contain content that is tangential to their containing articles, such as extra information and warnings.

### Basic Syntax
A FlexiAlertBlock is a sequence of lines that each start with`!`. The following is a FlexiAlertBlock:

```````````````````````````````` none
--------------- Markdown ---------------
! This is a FlexiAlertBlock.
! This is tangential content.
--------------- Expected Markup ---------------
<div class="fab-info">
<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z"/></svg>
<div class="fab-content">
<p>This is a FlexiAlertBlock.
This is tangential content.</p>
</div>
</div>
````````````````````````````````
Generated classes, icon markup, and more can be customized or omitted - refer to the [options section](#options) for instructions.

The first space after the starting `!` of each line is optional:

```````````````````````````````` none
--------------- Markdown ---------------
!This line will render identically to the next line.
! This line will render identically to the previous line.
--------------- Expected Markup ---------------
<div class="fab-info">
<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z"/></svg>
<div class="fab-content">
<p>This line will be the same as the next line.
This line will be the same as the previous line.</p>
</div>
</div>
````````````````````````````````

Starting `!`s can be preceded by up to three spaces:

```````````````````````````````` none
--------------- Markdown ---------------
! These lines belong to the same FlexiAlertBlock.
 ! These lines belong to the same FlexiAlertBlock.
  ! These lines belong to the same FlexiAlertBlock.
   ! These lines belong to the same FlexiAlertBlock.
--------------- Expected Markup ---------------
<div class="fab-info">
<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z"/></svg>
<div class="fab-content">
<p>These lines belong to the same FlexiAlertBlock.
These lines belong to the same FlexiAlertBlock.
These lines belong to the same FlexiAlertBlock.
These lines belong to the same FlexiAlertBlock.</p>
</div>
</div>
````````````````````````````````

[Lazy continuation lines](https://spec.commonmark.org/0.28/#lazy-continuation-line) are allowed within a FlexiAlertBlock:

```````````````````````````````` none
--------------- Markdown ---------------
! This FlexiAlertBlock
contains multiple
lazy continuation lines.
--------------- Expected Markup ---------------
<div class="fab-info">
<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z"/></svg>
<div class="fab-content">
<p>This FlexiAlertBlock
contains multiple
lazy continuation lines.</p>
</div>
</div>
````````````````````````````````

A blank line closes a FlexiAlertBlock:

```````````````````````````````` none
--------------- Markdown ---------------
! This is a FlexiAlertBlock.

! This is another FlexiAlertBlock.
--------------- Expected Markup ---------------
<div class="fab-info">
<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z"/></svg>
<div class="fab-content">
<p>This is a FlexiAlertBlock.</p>
</div>
</div>
<div class="fab-info">
<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z"/></svg>
<div class="fab-content">
<p>This is another FlexiAlertBlock.</p>
</div>
</div>
````````````````````````````````

### Options
The FlexiAlertBlocks extension has the following options types:

#### `FlexiAlertBlockOptions`
Options for a FlexiAlertBlock.
##### Properties
- `IconMarkup`
  - Type: `string`
  - Description: The markup for the FlexiAlertBlock's icon. If this value is null, whitespace or an empty string, 
    an attempt is made to retrieve icon markup for this block's type from [FlexiAlertBlocksExtensionOptions](#flexialertblocksextensionoptions), 
    failing which, no icon is rendered.
  - Default: `null`
- `ClassFormat`
  - Type: `string`
  - Description: The format for the FlexiAlertBlock's outermost element's class. The FlexiAlertBlock's type will
    replace "{0}" in the format. If this value is null, whitespace or an empty string,
    no class is assigned.  
  - Default: "fab-{0}"
- `ContentClass`
  - Type: `string`
  - Description: The class of the FlexiAlertBlock's content wrapper. If this value is null, whitespace or an empty string,
    no class is assigned. 
  - Default: "fab-content"
- `AlertType`
  - Type: `string`
  - Description: The FlexiAlertBlock's type. If this value is null, whitespace or an empty string, the FlexiAlertBlock will have no type.
  - Default: "info"
- `Attributes`
  - Type: `IDictionary<string, string>`
  - Description: The HTML attributes for the FlexiAlertBlock's outermost element. If this value is null, no 
    attributes will be assigned to the outermost element.
  - Default: `null`

##### Usage
To specify FlexiAlertBlockOptions for individual FlexiAlertBlocks, the [FlexiOptionsBlock](https://github.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiOptionsBlocksSpecs.md#flexioptionsblocks) extension must be enabled.

`IconMarkup`:
```````````````````````````````` none
--------------- Extra Extensions ---------------
FlexiOptionsBlocks
--------------- Markdown ---------------
@{
    "iconMarkup": "<svg><use xlink:href=\"#alert-icon\"></use></svg>"
}
! This is a FlexiAlertBlock.
--------------- Expected Markup ---------------
<div class="fab-info">
<svg><use xlink:href="#alert-icon"></use></svg>
<div class="fab-content">
<p>This is a FlexiAlertBlock.</p>
</div>
</div>
````````````````````````````````

`ClassFormat`:
```````````````````````````````` none
--------------- Extra Extensions ---------------
FlexiOptionsBlocks
--------------- Markdown ---------------
@{
    "classFormat": "alert-{0}"
}
! This is a FlexiAlertBlock.
--------------- Expected Markup ---------------
<div class="alert-info">
<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z"/></svg>
<div class="fab-content">
<p>This is a FlexiAlertBlock.</p>
</div>
</div>
````````````````````````````````

`ContentClass`:
```````````````````````````````` none
--------------- Extra Extensions ---------------
FlexiOptionsBlocks
--------------- Markdown ---------------
@{
    "contentClass": "alert-content"
}
! This is a FlexiAlertBlock.
--------------- Expected Markup ---------------
<div class="fab-info">
<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z"/></svg>
<div class="alert-content">
<p>This is a FlexiAlertBlock.</p>
</div>
</div>
````````````````````````````````

`Type`:
```````````````````````````````` none
--------------- Extra Extensions ---------------
FlexiOptionsBlocks
--------------- Markdown ---------------
@{
    "type": "warning"
}
! This is a FlexiAlertBlock.
--------------- Expected Markup ---------------
<div class="fab-warning">
<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="m1 21h22l-11-19-11 19zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"/></svg>
<div class="fab-content">
<p>This is a FlexiAlertBlock.</p>
</div>
</div>
````````````````````````````````

`Attributes`:
```````````````````````````````` none
--------------- Extra Extensions ---------------
FlexiOptionsBlocks
--------------- Markdown ---------------
@{
    "attributes": {
        "id" : "info-1",
        "class" : "block"
    }
}
! This is a FlexiAlertBlock.
--------------- Expected Markup ---------------
<div id="info-1" class="block fab-info">
<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z"/></svg>
<div class="fab-content">
<p>This is a FlexiAlertBlock.</p>
</div>
</div>
````````````````````````````````
If a value is specified for the class attribute, it will not override the outermost element's generated class. Instead, it will be 
prepended to the generated class. In the above example, this results in the outermost element's class attribute having the value 
`block fab-info`.

#### `FlexiAlertBlocksExtensionOptions`
Global options for FlexiAlertBlocks. These options can be used to define defaults for all FlexiAlertBlocks. They have
lower precedence than block specific options specified using the FlexiOptionsBlocks extension.
##### Properties
- IconMarkups
  - Type: `Dictionary<string, string>`
  - Description: A map of FlexiAlertBlock types to icon markups. Add markups for custom FlexiAlertBlock types to this dictionary.
  - Default: Contains icon markups for types "info", 
    "warning" and "critical-warning".
- DefaultBlockOptions
  - Type: `FlexiAlertBlockOptions`
  - Description: Default `FlexiAlertBlockOptions` for all FlexiAlertBlocks. 
##### Usage
FlexiAlertBlocksExtensionOptions can be specified when enabling the FlexiAlertBlocks extension:
``` 
MyMarkdownPipelineBuilder.UseFlexiAlertBlocks(myFlexiAlertBlocksExtensionOptions);
```

<!-- TODO cleanup example icon svgs, add links to licenses -->
`IconMarkups`:
```````````````````````````````` none
--------------- Extra Extensions ---------------
FlexiOptionsBlocks
--------------- Extension Options ---------------
{
    "flexiAlertBlocks": {
        "iconMarkups": {
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
<div class="fab-closer-look">
<svg height="24" viewBox="0 0 24 24" width="24" xmlns="http://www.w3.org/2000/svg"><path d="M15.5 14h-.79l-.28-.27C15.41 12.59 16 11.11 16 9.5 16 5.91 13.09 3 9.5 3S3 5.91 3 9.5 5.91 16 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z"/></svg>
<div class="fab-content">
<p>This is a closer look at some topic.</p>
</div>
</div>
<div class="fab-help">
<svg width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 17h-2v-2h2v2zm2.07-7.75l-.9.92C13.45 12.9 13 13.5 13 15h-2v-.5c0-1.1.45-2.1 1.17-2.83l1.24-1.26c.37-.36.59-.86.59-1.41 0-1.1-.9-2-2-2s-2 .9-2 2H8c0-2.21 1.79-4 4-4s4 1.79 4 4c0 .88-.36 1.68-.93 2.25z"/></svg>
<div class="fab-content">
<p>This is a helpful tip.</p>
</div>
</div>
````````````````````````````````

`DefaultBlockOptions`:
```````````````````````````````` none
--------------- Extension Options ---------------
{
    "flexiAlertBlocks": {
        "defaultBlockOptions": {
            "iconMarkup": "<svg><use xlink:href=\"#alert-icon\"></use></svg>",
            "classFormat": "alert-{0}",
            "contentClass": "alert-content",
            "attributes": {
                "class": "block"
            }
        }
    }
}
--------------- Markdown ---------------
! This is a FlexiAlertBlock.
--------------- Expected Markup ---------------
<div class="block alert-info">
<svg><use xlink:href="#alert-icon"></use></svg>
<div class="alert-content">
<p>This is a FlexiAlertBlock.</p>
</div>
</div>
````````````````````````````````

Default FlexiAlertBlockOptions have lower precedence than block specific options:
```````````````````````````````` none
--------------- Extra Extensions ---------------
FlexiOptionsBlocks
--------------- Extension Options ---------------
{
    "flexiAlertBlocks": {
        "defaultBlockOptions": {
            "classFormat": "alert-{0}"
        }
    }
}
--------------- Markdown ---------------
! This is a FlexiAlertBlock

@{
    "classFormat": "special-alert-{0}"
}
! This is a FlexiAlertBlock with block specific options.
--------------- Expected Markup ---------------
<div class="alert-info">
<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z"/></svg>
<div class="fab-content">
<p>This is a FlexiAlertBlock</p>
</div>
</div>
<div class="special-alert-info">
<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M0,0h24v24H0V0z" fill="none"/><path d="m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z"/></svg>
<div class="fab-content">
<p>This is a FlexiAlertBlock with block specific options.</p>
</div>
</div>
````````````````````````````````