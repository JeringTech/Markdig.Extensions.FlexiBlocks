## Alerts
Alerts are boxes within articles that contain tangential content. Such content can be things like extra information and warnings. Alerts have a similar syntax to 
blockquotes. However, they have very different purposes - according to the [specifications](https://html.spec.whatwg.org/multipage/grouping-content.html#the-blockquote-element)
blockquotes should be used when quoting from external articles.

Every line of an alert must start with an `!`. The first line of an alert must be of the form `!<optional space><alert name>` where `<alert name>`
is a string containing 1 or more characters from the regex character set `[A-Za-z0-9_-]`. The result of appending `alert-` to the alert name is used as the
alert block's class:

```````````````````````````````` example
! critical-warning
! This is a critical warning.
.
<div class="alert-critical-warning">
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"></path></svg>
<div class="alert-content">
<p>This is a critical warning.</p>
</div>
</div>
````````````````````````````````

The block is ignored if the first line does not contain a level name :

```````````````````````````````` example
! 
! This is a warning.
.
<p>!
! This is a warning.</p>
````````````````````````````````

The block is ignored if the first line contains disallowed characters :

```````````````````````````````` example
! illegal space
! This is a warning.
.
<p>! illegal space
! This is a warning.</p>
````````````````````````````````

The first space after `!` is ignored. :

```````````````````````````````` example
! warning
!This line will be rendered with 0 leading spaces.
! This line will also be rendered with 0 leading spaces.
.
<div class="alert-warning">
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"></path></svg>
<div class="alert-content">
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
<div class="alert-info">
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"></path></svg>
<div class="alert-content">
<p>This is part of
the info.
This is also part of
the info.</p>
</div>
</div>
````````````````````````````````

`AlertsExtensionOptions.IconMarkups` can be used to define icon element markup for custom alert types:

```````````````````````````````` options
{
    "alerts": {
        "iconMarkups": {
            "closer-look": "<svg height=\"24\" viewBox=\"0 0 24 24\" width=\"24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M15.5 14h-.79l-.28-.27C15.41 12.59 16 11.11 16 9.5 16 5.91 13.09 3 9.5 3S3 5.91 3 9.5 5.91 16 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z\"/></svg>"
        }
    }
}
```````````````````````````````` example
! closer-look
! This is a closer look at some topic.
.
<div class="alert-closer-look">
<svg height="24" viewBox="0 0 24 24" width="24" xmlns="http://www.w3.org/2000/svg"><path d="M15.5 14h-.79l-.28-.27C15.41 12.59 16 11.11 16 9.5 16 5.91 13.09 3 9.5 3S3 5.91 3 9.5 5.91 16 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z"/></svg>
<div class="alert-content">
<p>This is a closer look at some topic.</p>
</div>
</div>
````````````````````````````````

Per-alert-block options can be overriden if the JSON options extension is enabled:
```````````````````````````````` example
! warning
! This is a warning.
@{
    "iconMarkup": "<svg><use xlink:href=\"#alternative-warning-icon\"></use></svg>"
}
! warning
! This is a special warning.
.
<div class="alert-warning">
<svg viewBox="0 0 24 24" width="24" height="24"><path d="M0 0h24v24H0z" fill="none"></path><path d="M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z"></path></svg>
<div class="alert-content">
<p>This is a warning.</p>
</div>
</div>
<div class="alert-warning">
<svg><use xlink:href="#alternative-warning-icon"></use></svg>
<div class="alert-content">
<p>This is a special warning.</p>
</div>
</div>
````````````````````````````````