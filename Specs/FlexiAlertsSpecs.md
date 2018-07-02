## FlexiAlerts
FlexiAlerts contain tangential content, such as extra information and warnings.

Every line of a FlexiAlert must start with an `!`. The first line of a FlexiAlert must be of the form `!<optional space><flexi alert type>`, where `<flexi alert type>`
is a string containing 1 or more characters from the regex character set `[A-Za-z0-9_-]`. The result of appending `flexi-alert-` to the `<flexi alert type>` is used as the
FlexiAlert's class:

```````````````````````````````` example
! critical-warning
! This is a critical warning.
.
<div class="flexi-alert-critical-warning">
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"></path></svg>
<div class="flexi-alert-content">
<p>This is a critical warning.</p>
</div>
</div>
````````````````````````````````

The following is not a FlexiAlert since the first line does not contain a FlexiAlert type:

```````````````````````````````` example
! 
! This is a warning.
.
<p>!
! This is a warning.</p>
````````````````````````````````

The following is not a FlexiAlert since the first line does not contain a valid FlexiAlert type:

```````````````````````````````` example
! illegal space
! This is a warning.
.
<p>! illegal space
! This is a warning.</p>
````````````````````````````````

The first space after each `!` is ignored. :

```````````````````````````````` example
! warning
!This line will be rendered with 0 leading spaces.
! This line will also be rendered with 0 leading spaces.
.
<div class="flexi-alert-warning">
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"></path></svg>
<div class="flexi-alert-content">
<p>This line will be rendered with 0 leading spaces.
This line will also be rendered with 0 leading spaces.</p>
</div>
</div>
````````````````````````````````

Lazy continuation is allowed:

```````````````````````````````` example
! info
! This is part of
the info.
! This is also part of
the info.
.
<div class="flexi-alert-info">
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"></path></svg>
<div class="flexi-alert-content">
<p>This is part of
the info.
This is also part of
the info.</p>
</div>
</div>
````````````````````````````````

`FlexiAlertsExtensionOptions.IconMarkups` can be used to specify icon markups for FlexiAlert types:

```````````````````````````````` options
{
    "flexialerts": {
        "iconMarkups": {
            "closer-look": "<svg height=\"24\" viewBox=\"0 0 24 24\" width=\"24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M15.5 14h-.79l-.28-.27C15.41 12.59 16 11.11 16 9.5 16 5.91 13.09 3 9.5 3S3 5.91 3 9.5 5.91 16 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z\"/></svg>"
        }
    }
}
```````````````````````````````` example
! closer-look
! This is a closer look at some topic.
.
<div class="flexi-alert-closer-look">
<svg height="24" viewBox="0 0 24 24" width="24" xmlns="http://www.w3.org/2000/svg"><path d="M15.5 14h-.79l-.28-.27C15.41 12.59 16 11.11 16 9.5 16 5.91 13.09 3 9.5 3S3 5.91 3 9.5 5.91 16 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z"/></svg>
<div class="flexi-alert-content">
<p>This is a closer look at some topic.</p>
</div>
</div>
````````````````````````````````

Per-FlexiAlert-block options can be specified if the FlexiOptions extension is enabled:
```````````````````````````````` example
! warning
! This is a warning.
@{
    "iconMarkup": "<svg><use xlink:href=\"#alternative-warning-icon\"></use></svg>"
}
! warning
! This is a warning with a custom icon.
.
<div class="flexi-alert-warning">
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"></path></svg>
<div class="flexi-alert-content">
<p>This is a warning.</p>
</div>
</div>
<div class="flexi-alert-warning">
<svg><use xlink:href="#alternative-warning-icon"></use></svg>
<div class="flexi-alert-content">
<p>This is a warning with a custom icon.</p>
</div>
</div>
````````````````````````````````