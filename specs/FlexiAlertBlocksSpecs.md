## FlexiAlertBlocks
FlexiAlertBlocks contain content that is tangential to their containing articles, such as extra information and warnings.

### Syntax
A FlexiAlertBlock is a sequence of lines that each start with`!`. The first line of the sequence must be of the form `!<optional space><FlexiAlertBlock type>`, where `<FlexiAlertBlock type>`
is a string containing 1 or more characters from the regex character set `[A-Za-z0-9_-]`. 

The following is a FlexiAlertBlock:

```````````````````````````````` example
--------------- Markdown ---------------
! critical-warning
! This is a critical warning.
--------------- Expected Markup ---------------
<div class="fab-critical-warning">
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"></path></svg>
<div class="fab-content">
<p>This is a critical warning.</p>
</div>
</div>
````````````````````````````````

Note how an SVG has been included as the FlexiAlertBlock's icon. The icon markup for each FlexiAlertBlock
can be customized or omitted, options for icon markup are covered in the [options section](#options).

Also note how the class `fab-<FlexiAlertBlock type>` (`fab-critical-warning` in this case) is assigned to the outermost `div` element. Using a 
different FlexiAlertBlock type results in a different class:

```````````````````````````````` example
--------------- Markdown ---------------
! info
! This is information.
--------------- Expected Markup ---------------
<div class="fab-info">
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"></path></svg>
<div class="fab-content">
<p>This is information.</p>
</div>
</div>
````````````````````````````````

The following is not a FlexiAlertBlock since the first line does not contain a FlexiAlertBlock type:

```````````````````````````````` example
--------------- Markdown ---------------
! 
! This is a warning.
--------------- Expected Markup ---------------
<p>!
! This is a warning.</p>
````````````````````````````````

The following is not a FlexiAlertBlock either since the first line does not contain a valid FlexiAlertBlock type - 
`illegal space` contains a space, which isn't in the regex character set `[A-Za-z0-9_-]`:

```````````````````````````````` example
--------------- Markdown ---------------
! illegal space
! This is a warning.
--------------- Expected Markup ---------------
<p>! illegal space
! This is a warning.</p>
````````````````````````````````

The first space after the starting `!` of each line is ignored:

```````````````````````````````` example
--------------- Markdown ---------------
! warning
!This line will be rendered with 0 leading spaces.
! This line will also be rendered with 0 leading spaces.
--------------- Expected Markup ---------------
<div class="fab-warning">
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"></path></svg>
<div class="fab-content">
<p>This line will be rendered with 0 leading spaces.
This line will also be rendered with 0 leading spaces.</p>
</div>
</div>
````````````````````````````````

[Lazy continuation lines](https://spec.commonmark.org/0.28/#lazy-continuation-line) are allowed within a FlexiAlertBlock:

```````````````````````````````` example
--------------- Markdown ---------------
! info
! This is part of
the info.
! This is also part of
the info.
--------------- Expected Markup ---------------
<div class="fab-info">
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"></path></svg>
<div class="fab-content">
<p>This is part of
the info.
This is also part of
the info.</p>
</div>
</div>
````````````````````````````````

A blank line closes a FlexiAlertBlock:

```````````````````````````````` example
--------------- Markdown ---------------
! info
! This is information.

! warning
! This is a warning.
--------------- Expected Markup ---------------
<div class="fab-info">
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"></path></svg>
<div class="fab-content">
<p>This is information.</p>
</div>
</div>
<div class="fab-warning">
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"></path></svg>
<div class="fab-content">
<p>This is a warning.</p>
</div>
</div>
````````````````````````````````

### Options
The FlexiAlertBlocks extension has the following option types:

#### `FlexiAlertBlockOptions`
Options for a FlexiAlertBlock.
##### Properties
- IconMarkup
  - Type: `string`
  - Description: The markup for the FlexiAlertBlock's icon. If the value is null, whitespace or an empty string, 
    no icon markup is rendered.
  - Default: `null`
- ClassNameFormat
  - Type: `string`
  - Description: The format for the FlexiAlertBlock's outermost element's class. The FlexiAlertBlock's type will
    replace "{0}" in the format. If the value is null, whitespace or an empty string,
    no class is assigned.  
  - Default: "fab-{0}"
- ContentClassName
  - Type: `string`
  - Description: The class of the FlexiAlertBlock's content wrapper. If the value is null, whitespace or an empty string,
    no class is assigned. 
  - Default: "fab-content"
- Attributes
  - Type: `HtmlAttributeDictionary`
  - Description: The HTML attributes for the FlexiAlertBlock's outermost element.

##### Usage
To specify FlexiAlertBlockOptions for individual FlexiAlertBlocks, the [FlexiOptionsBlock](https://github.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiOptionsBlocksSpecs.md#flexioptionsblocks) extension must be registered.

Icon markup can specified for a FlexiAlertBlock:

```````````````````````````````` example
--------------- Extra Extensions ---------------
FlexiOptionsBlocks
--------------- Markdown ---------------
@{
    "iconMarkup": "<svg><use xlink:href=\"#alternative-warning-icon\"></use></svg>"
}
! warning
! This is a warning.
--------------- Expected Markup ---------------
<div class="fab-warning">
<svg><use xlink:href="#alternative-warning-icon"></use></svg>
<div class="fab-content">
<p>This is a warning.</p>
</div>
</div>
````````````````````````````````

The format for the FlexiAlertBlock's outermost element's class can be specified:
```````````````````````````````` example
--------------- Extra Extensions ---------------
FlexiOptionsBlocks
--------------- Markdown ---------------
@{
    "classNameFormat": "alert-{0}"
}
! warning
! This is a warning.
--------------- Expected Markup ---------------
<div class="alert-warning">
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"></path></svg>
<div class="fab-content">
<p>This is a warning.</p>
</div>
</div>
````````````````````````````````

The class of the FlexiAlertBlock's content wrapper can be specified:
```````````````````````````````` example
--------------- Extra Extensions ---------------
FlexiOptionsBlocks
--------------- Markdown ---------------
@{
    "contentClassName": "alert-content"
}
! warning
! This is a warning.
--------------- Expected Markup ---------------
<div class="fab-warning">
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"></path></svg>
<div class="alert-content">
<p>This is a warning.</p>
</div>
</div>
````````````````````````````````

The HTML attributes for the FlexiAlertBlock's outermost element can be specified:
```````````````````````````````` example
--------------- Extra Extensions ---------------
FlexiOptionsBlocks
--------------- Markdown ---------------
@{
    "attributes": {
        "id" : "warning-1"
    }
}
! warning
! This is a warning.
--------------- Expected Markup ---------------
<div id="warning-1" class="fab-warning">
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"></path></svg>
<div class="fab-content">
<p>This is a warning.</p>
</div>
</div>
````````````````````````````````

#### `FlexiAlertBlocksExtensionOptions`
##### Description
Global options for FlexiAlertBlocks. These options can be used to define defaults for all FlexiAlertBlocks. Block specific options take precedence over these options.
##### Properties
- IconMarkups
  - Type: `Dictionary<string, string>`
  - Description: A map of FlexiAlertBlock types to icon markups. Add markups for custom FlexiAlertBlock types to this dictionary.
  - Default: Contains icon markups for `info`, 
    `warning` and `critical-warning`.
- DefaultBlockOptions
  - Type: `FlexiAlertBlockOptions`
  - Description: Default `FlexiAlertBlockOptions` for all FlexiAlertBlocks. 
##### Usage
FlexiAlertBlocksExtensionOptions can be specified when registering the FlexiAlertBlocks extension
``` 
MyMarkdownPipelineBuilder.UseFlexiAlertBlocks(myFlexiAlertBlocksExtensionOptions);
```

Default icon markups for custom FlexiAlertBlock types can be specified:
```````````````````````````````` example
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
! closer-look
! This is a closer look at some topic.

! help
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

Default FlexiBlockOptions can be specified:
```````````````````````````````` example
--------------- Extension Options ---------------
{
    "flexiAlertBlocks": {
        "defaultBlockOptions": {
            "iconMarkup": "<svg><use xlink:href=\"#alert-icon\"></use></svg>",
            "classNameFormat": "alert-{0}",
            "contentClassName": "alert-content",
            "attributes": {
                "class": "stretch"
            }
        }
    }
}
--------------- Markdown ---------------
! warning
! This is a warning.

! info
! This is information.
--------------- Expected Markup ---------------
<div class="stretch alert-warning">
<svg><use xlink:href="#alert-icon"></use></svg>
<div class="alert-content">
<p>This is a warning.</p>
</div>
</div>
<div class="stretch alert-info">
<svg><use xlink:href="#alert-icon"></use></svg>
<div class="alert-content">
<p>This is information.</p>
</div>
</div>
````````````````````````````````