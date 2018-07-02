## FlexiAlertBlocks
FlexiAlertBlocks contain content that is tangential to their containing articles, such as extra information and warnings.

Every line of a FlexiAlertBlock must start with an `!`. Its first line must be of the form `!<optional space><FlexiAlertBlock type>`, where `<FlexiAlertBlock type>`
is a string containing 1 or more characters from the regex character set `[A-Za-z0-9_-]`. The result of appending `fab-` to `<FlexiAlertBlock type>` is used as the
FlexiAlertBlock's class:

```````````````````````````````` example
! critical-warning
! This is a critical warning.
.
<div class="fab-critical-warning">
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"></path></svg>
<div class="fab-content">
<p>This is a critical warning.</p>
</div>
</div>
````````````````````````````````

The following is not a FlexiAlertBlock since the first line does not contain a FlexiAlertBlock type:

```````````````````````````````` example
! 
! This is a warning.
.
<p>!
! This is a warning.</p>
````````````````````````````````

The following is not a FlexiAlertBlock either, since the first line does not contain a valid FlexiAlertBlock type:

```````````````````````````````` example
! illegal space
! This is a warning.
.
<p>! illegal space
! This is a warning.</p>
````````````````````````````````

The first space after each `!` in a FlexiAlertBlock is ignored. :

```````````````````````````````` example
! warning
!This line will be rendered with 0 leading spaces.
! This line will also be rendered with 0 leading spaces.
.
<div class="fab-warning">
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"></path></svg>
<div class="fab-content">
<p>This line will be rendered with 0 leading spaces.
This line will also be rendered with 0 leading spaces.</p>
</div>
</div>
````````````````````````````````

Lazy continuation is allowed within a FlexiAlertBlock:

```````````````````````````````` example
! info
! This is part of
the info.
! This is also part of
the info.
.
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

`FlexiAlertBlocksExtensionOptions.IconMarkups` can be used to specify icon markups for FlexiAlertBlock types:

```````````````````````````````` options
{
    "flexialertblocks": {
        "iconMarkups": {
            "closer-look": "<svg height=\"24\" viewBox=\"0 0 24 24\" width=\"24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M15.5 14h-.79l-.28-.27C15.41 12.59 16 11.11 16 9.5 16 5.91 13.09 3 9.5 3S3 5.91 3 9.5 5.91 16 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z\"/></svg>"
        }
    }
}
```````````````````````````````` example
! closer-look
! This is a closer look at some topic.
.
<div class="fab-closer-look">
<svg height="24" viewBox="0 0 24 24" width="24" xmlns="http://www.w3.org/2000/svg"><path d="M15.5 14h-.79l-.28-.27C15.41 12.59 16 11.11 16 9.5 16 5.91 13.09 3 9.5 3S3 5.91 3 9.5 5.91 16 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z"/></svg>
<div class="fab-content">
<p>This is a closer look at some topic.</p>
</div>
</div>
````````````````````````````````

Per-FlexiAlertBlock options can be specified if the FlexiOptionBlocks extension is enabled:
```````````````````````````````` example
! warning
! This is a warning.
@{
    "iconMarkup": "<svg><use xlink:href=\"#alternative-warning-icon\"></use></svg>"
}
! warning
! This is a warning with a custom icon.
.
<div class="fab-warning">
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"></path></svg>
<div class="fab-content">
<p>This is a warning.</p>
</div>
</div>
<div class="fab-warning">
<svg><use xlink:href="#alternative-warning-icon"></use></svg>
<div class="fab-content">
<p>This is a warning with a custom icon.</p>
</div>
</div>
````````````````````````````````