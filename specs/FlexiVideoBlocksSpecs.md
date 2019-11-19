---
blockOptions: "../src/FlexiBlocks/Extensions/FlexiVideoBlocks/FlexiVideoBlockOptions.cs"
extensionOptions: "../src/FlexiBlocks/Extensions/FlexiVideoBlocks/FlexiVideoBlocksExtensionOptions.cs"
requiresOptionsExtension: false
---

# FlexiVideoBlocks
A FlexiVideoBlock displays a video.

## Usage
```csharp
using Markdig;
using Jering.Markdig.Extensions.FlexiBlocks;

...
var markdownPipelineBuilder = new MarkdownPipelineBuilder();
markdownPipelineBuilder.UseFlexiVideoBlocks(/* Optional extension options */);

MarkdownPipeline markdownPipeline = markdownPipelineBuilder.Build();

string markdown = @"v{ 
  ""src"": ""/file.mp4""
}";
string html = Markdown.ToHtml(markdown, markdownPipeline);
string expectedHtml = @"<div class=""flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon"">
<div class=""flexi-video__container"" tabindex=""-1"">
<div class=""flexi-video__video-outer-container"">
<div class=""flexi-video__video-inner-container"">
<video class=""flexi-video__video"" preload=""auto"" muted playsInline disablePictureInPicture loop>
<source class=""flexi-video__source"" data-src=""/file.mp4"" type=""video/mp4"">
</video>
</div>
</div>
<div class=""flexi-video__controls"">
<button class=""flexi-video__play-pause-button"" aria-label=""Pause/play"">
<svg class=""flexi-video__play-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path d=""M8 5v14l11-7z""/><path d=""M0 0h24v24H0z"" fill=""none""/></svg>
<svg class=""flexi-video__pause-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path shape-rendering=""crispEdges"" d=""M6 19h4V5H6v14zm8-14v14h4V5h-4z""/></svg>
</button>
<div class=""flexi-video__elapsed-time"">
<span class=""flexi-video__current-time"">0:00</span>
/<span class=""flexi-video__duration"">0:00</span>
</div>
<div class=""flexi-video__progress"">
<div class=""flexi-video__progress-track"">
<div class=""flexi-video__progress-played""></div>
<div class=""flexi-video__progress-buffered""></div>
</div>
</div>
<button class=""flexi-video__fullscreen-button"" aria-label=""Toggle fullscreen"">
<svg class=""flexi-video__fullscreen-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path shape-rendering=""crispEdges"" d=""M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z""/></svg>
<svg class=""flexi-video__exit-fullscreen-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path shape-rendering=""crispEdges"" d=""M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z""/></svg>
</button>
</div>
<div class=""flexi-video__error-notice"">
<svg class=""flexi-video__error-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path d=""M0 0h24v24H0z"" fill=""none""/><path d=""M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z""/></svg>
</div>
<div class=""flexi-video__spinner spinner"">
    <div class=""spinner__rects"">
        <div class=""spinner__rect-1""></div>
        <div class=""spinner__rect-2""></div>
        <div class=""spinner__rect-3""></div>
    </div>
</div>
</div>
</div>";

Assert.Equal(expectedHtml, html)
```

# Basics
In markdown, a FlexiVideoBlock is a [`FlexiVideoBlockOptions`][options] object in JSON form, prepended with `v`. For example:

```````````````````````````````` none
--------------- Markdown ---------------
v{ 
  "src": "/file.mp4"
}
--------------- Expected Markup ---------------
<div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
<div class="flexi-video__container" tabindex="-1">
<div class="flexi-video__video-outer-container">
<div class="flexi-video__video-inner-container">
<video class="flexi-video__video" preload="auto" muted playsInline disablePictureInPicture loop>
<source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
</video>
</div>
</div>
<div class="flexi-video__controls">
<button class="flexi-video__play-pause-button" aria-label="Pause/play">
<svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
<svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
</button>
<div class="flexi-video__elapsed-time">
<span class="flexi-video__current-time">0:00</span>
/<span class="flexi-video__duration">0:00</span>
</div>
<div class="flexi-video__progress">
<div class="flexi-video__progress-track">
<div class="flexi-video__progress-played"></div>
<div class="flexi-video__progress-buffered"></div>
</div>
</div>
<button class="flexi-video__fullscreen-button" aria-label="Toggle fullscreen">
<svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
<svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
</button>
</div>
<div class="flexi-video__error-notice">
<svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
</div>
<div class="flexi-video__spinner spinner">
    <div class="spinner__rects">
        <div class="spinner__rect-1"></div>
        <div class="spinner__rect-2"></div>
        <div class="spinner__rect-3"></div>
    </div>
</div>
</div>
</div>
````````````````````````````````

! Generated elements are assigned classes that comply with [BEM methodology](https://en.bem.info/). These classes can be customized. We explain how in [options].

## Options
### `FlexiVideoBlockOptions`
Options for a FlexiVideoBlock.

#### Properties

##### `BlockName`
- Type: `string`
- Description: The `FlexiVideoBlock`'s [BEM block name](https://en.bem.info/methodology/naming-convention/#block-name).
  In compliance with [BEM methodology](https://en.bem.info), this value is the `FlexiVideoBlock`'s root element's class as well as the prefix for all other classes in the block.
  This value should contain only valid [CSS class characters](https://www.w3.org/TR/CSS21/syndata.html#characters).
  If this value is `null`, whitespace or an empty string, the `FlexiVideoBlock`'s block name is "flexi-video".
- Default: "flexi-video"
- Examples:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  v{ 
    "blockName": "video",
    "src": "/file.mp4"
  }
  --------------- Expected Markup ---------------
  <div class="video video_no-poster video_no-width video_no-aspect-ratio video_no-duration video_has-type video_has-spinner video_has-play-icon video_has-pause-icon video_has-fullscreen-icon video_has-exit-fullscreen-icon video_has-error-icon">
  <div class="video__container" tabindex="-1">
  <div class="video__video-outer-container">
  <div class="video__video-inner-container">
  <video class="video__video" preload="auto" muted playsInline disablePictureInPicture loop>
  <source class="video__source" data-src="/file.mp4" type="video/mp4">
  </video>
  </div>
  </div>
  <div class="video__controls">
  <button class="video__play-pause-button" aria-label="Pause/play">
  <svg class="video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  <svg class="video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
  </button>
  <div class="video__elapsed-time">
  <span class="video__current-time">0:00</span>
  /<span class="video__duration">0:00</span>
  </div>
  <div class="video__progress">
  <div class="video__progress-track">
  <div class="video__progress-played"></div>
  <div class="video__progress-buffered"></div>
  </div>
  </div>
  <button class="video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
  <svg class="video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
  </button>
  </div>
  <div class="video__error-notice">
  <svg class="video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="video__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  </div>
  </div>
  ````````````````````````````````

##### `Src`
- Type: `string`
- Description: The `FlexiVideoBlock`'s source URI.
  All `FlexiVideoBlock`s are loaded lazily. Therefore, this value is assigned to the source element's data-src attribute.
  On the client, the data-src attribute's value is copied to the src attribute when the `FlexiVideoBlock` is almost visible
  and loading of the `FlexiVideoBlock` is started.
  Benefits of lazy loading include [reducing initial page load time, initial page weight, and system resource usage](https://developers.google.com/web/fundamentals/performance/lazy-loading-guidance/images-and-video#why_lazy_load_images_or_video_instead_of_just_loading_them).
  [Chrome recently implemented native lazy loading](https://web.dev/native-lazy-loading), unfortunately native lazy loading isn't widely supported across browsers, so we don't support native lazy loading yet.
  This value is required and must be a valid URI pointing to a file.
- Examples:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  v{ 
    "src": "/file.mp4"
  }
  --------------- Expected Markup ---------------
  <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
  <div class="flexi-video__container" tabindex="-1">
  <div class="flexi-video__video-outer-container">
  <div class="flexi-video__video-inner-container">
  <video class="flexi-video__video" preload="auto" muted playsInline disablePictureInPicture loop>
  <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
  </video>
  </div>
  </div>
  <div class="flexi-video__controls">
  <button class="flexi-video__play-pause-button" aria-label="Pause/play">
  <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
  </button>
  <div class="flexi-video__elapsed-time">
  <span class="flexi-video__current-time">0:00</span>
  /<span class="flexi-video__duration">0:00</span>
  </div>
  <div class="flexi-video__progress">
  <div class="flexi-video__progress-track">
  <div class="flexi-video__progress-played"></div>
  <div class="flexi-video__progress-buffered"></div>
  </div>
  </div>
  <button class="flexi-video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
  <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
  </button>
  </div>
  <div class="flexi-video__error-notice">
  <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-video__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  </div>
  </div>
  ````````````````````````````````

##### `Type`
- Type: `string`
- Description: The `FlexiVideoBlock`'s MIME type.
  This value is assigned to the source element's type attribute.
  If this value is `null`, whitespace or an empty string, an attempt is made to retrieve a MIME type from
  `IFlexiVideoBlocksExtensionOptions.MimeTypes` using `Src`'s file extension, failing which the type attribute is not rendered.
  MIME types for file extensions can be specified in `IFlexiVideoBlocksExtensionOptions.MimeTypes`. The default implementation of `IFlexiVideoBlocksExtensionOptions.MimeTypes`
  contains MIME types for ".mp4", ".webm" and ".ogg".
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  v{ 
    "src": "/file.mp4",
    "type": "custom/type"
  }
  --------------- Expected Markup ---------------
  <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
  <div class="flexi-video__container" tabindex="-1">
  <div class="flexi-video__video-outer-container">
  <div class="flexi-video__video-inner-container">
  <video class="flexi-video__video" preload="auto" muted playsInline disablePictureInPicture loop>
  <source class="flexi-video__source" data-src="/file.mp4" type="custom/type">
  </video>
  </div>
  </div>
  <div class="flexi-video__controls">
  <button class="flexi-video__play-pause-button" aria-label="Pause/play">
  <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
  </button>
  <div class="flexi-video__elapsed-time">
  <span class="flexi-video__current-time">0:00</span>
  /<span class="flexi-video__duration">0:00</span>
  </div>
  <div class="flexi-video__progress">
  <div class="flexi-video__progress-track">
  <div class="flexi-video__progress-played"></div>
  <div class="flexi-video__progress-buffered"></div>
  </div>
  </div>
  <button class="flexi-video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
  <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
  </button>
  </div>
  <div class="flexi-video__error-notice">
  <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-video__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  </div>
  </div>
  ````````````````````````````````

##### `Width`
- Type: `double`
- Description: The `FlexiVideoBlock`'s width.
  If this value is larger than 0, it is assigned to width style properties of several elements.
  If this value and `Height` are both larger than 0, they're used to calculate the `FlexiVideoBlock`'s aspect ratio,
  which is assigned to a padding-bottom style property.
  The width and padding-bottom style properties [ensure that there is no reflow on video element load](https://www.voorhoede.nl/en/blog/say-no-to-image-reflow/).
  The [CSS Working Group](https://github.com/WICG/intrinsicsize-attribute/issues/16) have proposed a solution to content reflow on video element loads.
  Unfortunately, the solution isn't widely supported, so we do not support it yet.
  If this value is larger than 0, it takes precedence over any width retrieved by file operations.
- Default: 0
- Examples:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  v{ 
    "src": "/file.mp4",
    "width": 123
  }
  --------------- Expected Markup ---------------
  <div class="flexi-video flexi-video_no-poster flexi-video_has-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
  <div class="flexi-video__container" tabindex="-1" style="width:123px">
  <div class="flexi-video__video-outer-container" style="width:123px">
  <div class="flexi-video__video-inner-container">
  <video class="flexi-video__video" preload="auto" muted playsInline disablePictureInPicture loop>
  <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
  </video>
  </div>
  </div>
  <div class="flexi-video__controls">
  <button class="flexi-video__play-pause-button" aria-label="Pause/play">
  <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
  </button>
  <div class="flexi-video__elapsed-time">
  <span class="flexi-video__current-time">0:00</span>
  /<span class="flexi-video__duration">0:00</span>
  </div>
  <div class="flexi-video__progress">
  <div class="flexi-video__progress-track">
  <div class="flexi-video__progress-played"></div>
  <div class="flexi-video__progress-buffered"></div>
  </div>
  </div>
  <button class="flexi-video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
  <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
  </button>
  </div>
  <div class="flexi-video__error-notice">
  <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-video__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  </div>
  </div>
  ````````````````````````````````

##### `Height`
- Type: `double`
- Description: The `FlexiVideoBlock`'s height.
  If this value and `Width` are both larger than 0, they're used to calculate the `FlexiVideoBlock`'s aspect ratio,
  which is assigned to a padding-bottom style property.
  The padding-bottom style property [helps ensure that there is no reflow
  on video element load](https://www.voorhoede.nl/en/blog/say-no-to-image-reflow/).
  The [CSS Working Group](https://github.com/WICG/intrinsicsize-attribute/issues/16) have proposed a solution to content reflow on video element loads.
  Unfortunately, the solution isn't widely supported, so we do not support it yet.
  If this value is larger than 0, it takes precedence over any height retrieved by file operations.
- Default: 0
- Examples:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  v{ 
    "src": "/file.mp4",
    "width": 123,
    "height": 321
  }
  --------------- Expected Markup ---------------
  <div class="flexi-video flexi-video_no-poster flexi-video_has-width flexi-video_has-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
  <div class="flexi-video__container" tabindex="-1" style="width:123px">
  <div class="flexi-video__video-outer-container" style="width:123px">
  <div class="flexi-video__video-inner-container" style="padding-bottom:260.975609756098%">
  <video class="flexi-video__video" preload="auto" muted playsInline disablePictureInPicture loop>
  <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
  </video>
  </div>
  </div>
  <div class="flexi-video__controls">
  <button class="flexi-video__play-pause-button" aria-label="Pause/play">
  <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
  </button>
  <div class="flexi-video__elapsed-time">
  <span class="flexi-video__current-time">0:00</span>
  /<span class="flexi-video__duration">0:00</span>
  </div>
  <div class="flexi-video__progress">
  <div class="flexi-video__progress-track">
  <div class="flexi-video__progress-played"></div>
  <div class="flexi-video__progress-buffered"></div>
  </div>
  </div>
  <button class="flexi-video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
  <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
  </button>
  </div>
  <div class="flexi-video__error-notice">
  <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-video__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  </div>
  </div>
  ````````````````````````````````
  This option has no effect if width is less than or equal to 0:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  v{ 
    "src": "/file.mp4",
    "width": 0,
    "height": 321
  }
  --------------- Expected Markup ---------------
  <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
  <div class="flexi-video__container" tabindex="-1">
  <div class="flexi-video__video-outer-container">
  <div class="flexi-video__video-inner-container">
  <video class="flexi-video__video" preload="auto" muted playsInline disablePictureInPicture loop>
  <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
  </video>
  </div>
  </div>
  <div class="flexi-video__controls">
  <button class="flexi-video__play-pause-button" aria-label="Pause/play">
  <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
  </button>
  <div class="flexi-video__elapsed-time">
  <span class="flexi-video__current-time">0:00</span>
  /<span class="flexi-video__duration">0:00</span>
  </div>
  <div class="flexi-video__progress">
  <div class="flexi-video__progress-track">
  <div class="flexi-video__progress-played"></div>
  <div class="flexi-video__progress-buffered"></div>
  </div>
  </div>
  <button class="flexi-video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
  <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
  </button>
  </div>
  <div class="flexi-video__error-notice">
  <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-video__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  </div>
  </div>
  ````````````````````````````````

##### `Duration`
- Type: `double`
- Description: The `FlexiVideoBlock`'s duration.
  If this value is larger than 0, it is rendered in a span next to the `FlexiVideoBlock`'s progress bar.
  Prefilling the duration span allows end users to know the video's length before it loads.
  If this value is less than or equal to 0, the duration span's content is "0:00". On the client, once the browser
  knows how long the video is, the duration span's contents are updated.
  If this value is larger than 0, it takes precedence over any duration retrieved by file operations.
- Default: 0
- Examples:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  v{ 
    "src": "/file.mp4",
    "duration": 123.456
  }
  --------------- Expected Markup ---------------
  <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_has-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
  <div class="flexi-video__container" tabindex="-1">
  <div class="flexi-video__video-outer-container">
  <div class="flexi-video__video-inner-container">
  <video class="flexi-video__video" preload="auto" muted playsInline disablePictureInPicture loop>
  <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
  </video>
  </div>
  </div>
  <div class="flexi-video__controls">
  <button class="flexi-video__play-pause-button" aria-label="Pause/play">
  <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
  </button>
  <div class="flexi-video__elapsed-time">
  <span class="flexi-video__current-time">0:00</span>
  /<span class="flexi-video__duration">2:03</span>
  </div>
  <div class="flexi-video__progress">
  <div class="flexi-video__progress-track">
  <div class="flexi-video__progress-played"></div>
  <div class="flexi-video__progress-buffered"></div>
  </div>
  </div>
  <button class="flexi-video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
  <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
  </button>
  </div>
  <div class="flexi-video__error-notice">
  <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-video__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  </div>
  </div>
  ````````````````````````````````
  The duration span's content is "0:00" if this value is less than or equal to 0:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  v{ 
    "src": "/file.mp4",
    "duration": 0
  }
  --------------- Expected Markup ---------------
  <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
  <div class="flexi-video__container" tabindex="-1">
  <div class="flexi-video__video-outer-container">
  <div class="flexi-video__video-inner-container">
  <video class="flexi-video__video" preload="auto" muted playsInline disablePictureInPicture loop>
  <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
  </video>
  </div>
  </div>
  <div class="flexi-video__controls">
  <button class="flexi-video__play-pause-button" aria-label="Pause/play">
  <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
  </button>
  <div class="flexi-video__elapsed-time">
  <span class="flexi-video__current-time">0:00</span>
  /<span class="flexi-video__duration">0:00</span>
  </div>
  <div class="flexi-video__progress">
  <div class="flexi-video__progress-track">
  <div class="flexi-video__progress-played"></div>
  <div class="flexi-video__progress-buffered"></div>
  </div>
  </div>
  <button class="flexi-video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
  <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
  </button>
  </div>
  <div class="flexi-video__error-notice">
  <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-video__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  </div>
  </div>
  ````````````````````````````````

##### `GeneratePoster`
- Type: `bool`
- Description: The value specifying whether to generate a poster for the `FlexiVideoBlock`.
  If this value and `EnableFileOperations` are true and `IFlexiVideoBlocksExtensionOptions.LocalMediaDirectory` is not null,
  whitespace or an empty string, the video's first frame is extracted for use as a poster.
  The generated poster is named "<video file name less extension>__poster.png" and placed in the same directory as the video file.
  "<`Src` less extension>__poster.png" is assigned to the video element's poster attribute.
  A poster allows end users to know what the video is about before it loads.
  Therefore, this value should be true if the `FlexiVideoBlock` is immediately visible on page load (above-the-fold) but doesn't
  have a custom poster specified using `Poster`.
  Otherwise, this value should be false.
  Poster generation requires [FFmpeg](https://www.ffmpeg.org/) to be installed and on the path environment variable.
- Default: `false`

##### `Poster`
- Type: `string`
- Description: The `FlexiVideoBlock`'s poster URI.
  If this value is not `null`, whitespace or an empty string, it is assigned to the video element's poster attribute.
  This value takes precedence over any generated poster.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  v{ 
    "src": "/file.mp4",
    "poster": "/file_poster.png"
  }
  --------------- Expected Markup ---------------
  <div class="flexi-video flexi-video_has-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
  <div class="flexi-video__container" tabindex="-1">
  <div class="flexi-video__video-outer-container">
  <div class="flexi-video__video-inner-container">
  <video class="flexi-video__video" preload="auto" poster="/file_poster.png" muted playsInline disablePictureInPicture loop>
  <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
  </video>
  </div>
  </div>
  <div class="flexi-video__controls">
  <button class="flexi-video__play-pause-button" aria-label="Pause/play">
  <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
  </button>
  <div class="flexi-video__elapsed-time">
  <span class="flexi-video__current-time">0:00</span>
  /<span class="flexi-video__duration">0:00</span>
  </div>
  <div class="flexi-video__progress">
  <div class="flexi-video__progress-track">
  <div class="flexi-video__progress-played"></div>
  <div class="flexi-video__progress-buffered"></div>
  </div>
  </div>
  <button class="flexi-video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
  <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
  </button>
  </div>
  <div class="flexi-video__error-notice">
  <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-video__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  </div>
  </div>
  ````````````````````````````````

##### `Spinner`
- Type: `string`
- Description: The `FlexiVideoBlock`'s spinner as an HTML fragment.
  The class "<`BlockName`>__spinner" is assigned to this fragment's first start tag.
  If this value is `null`, whitespace or an empty string, no spinner is rendered.
- Default: a simple spinner
- Examples:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  v{ 
    "src": "/file.mp4",
    "spinner": "<div class=\"spinner\"></div>"
  }
  --------------- Expected Markup ---------------
  <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
  <div class="flexi-video__container" tabindex="-1">
  <div class="flexi-video__video-outer-container">
  <div class="flexi-video__video-inner-container">
  <video class="flexi-video__video" preload="auto" muted playsInline disablePictureInPicture loop>
  <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
  </video>
  </div>
  </div>
  <div class="flexi-video__controls">
  <button class="flexi-video__play-pause-button" aria-label="Pause/play">
  <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
  </button>
  <div class="flexi-video__elapsed-time">
  <span class="flexi-video__current-time">0:00</span>
  /<span class="flexi-video__duration">0:00</span>
  </div>
  <div class="flexi-video__progress">
  <div class="flexi-video__progress-track">
  <div class="flexi-video__progress-played"></div>
  <div class="flexi-video__progress-buffered"></div>
  </div>
  </div>
  <button class="flexi-video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
  <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
  </button>
  </div>
  <div class="flexi-video__error-notice">
  <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-video__spinner spinner"></div>
  </div>
  </div>
  ````````````````````````````````
  No spinner is rendered if this value is `null`, whitespace or an empty string:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  v{ 
    "src": "/file.mp4",
    "spinner": null
  }
  --------------- Expected Markup ---------------
  <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_no-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
  <div class="flexi-video__container" tabindex="-1">
  <div class="flexi-video__video-outer-container">
  <div class="flexi-video__video-inner-container">
  <video class="flexi-video__video" preload="auto" muted playsInline disablePictureInPicture loop>
  <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
  </video>
  </div>
  </div>
  <div class="flexi-video__controls">
  <button class="flexi-video__play-pause-button" aria-label="Pause/play">
  <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
  </button>
  <div class="flexi-video__elapsed-time">
  <span class="flexi-video__current-time">0:00</span>
  /<span class="flexi-video__duration">0:00</span>
  </div>
  <div class="flexi-video__progress">
  <div class="flexi-video__progress-track">
  <div class="flexi-video__progress-played"></div>
  <div class="flexi-video__progress-buffered"></div>
  </div>
  </div>
  <button class="flexi-video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
  <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
  </button>
  </div>
  <div class="flexi-video__error-notice">
  <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  </div>
  </div>
  ````````````````````````````````

##### `PlayIcon`
- Type: `string`
- Description: The `FlexiVideoBlock`'s play icon as an HTML fragment.
  The class "<`BlockName`>__play-icon" is assigned to this fragment's first start tag.
  If this value is `null`, whitespace or an empty string, no play icon is rendered.
- Default: the [Material Design play arrow icon](https://material.io/tools/icons/?icon=play_arrow&style=baseline)
- Examples:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  v{ 
    "src": "/file.mp4",
    "playIcon": "<svg><use xlink:href=\"#play-icon\"/></svg>"
  }
  --------------- Expected Markup ---------------
  <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
  <div class="flexi-video__container" tabindex="-1">
  <div class="flexi-video__video-outer-container">
  <div class="flexi-video__video-inner-container">
  <video class="flexi-video__video" preload="auto" muted playsInline disablePictureInPicture loop>
  <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
  </video>
  </div>
  </div>
  <div class="flexi-video__controls">
  <button class="flexi-video__play-pause-button" aria-label="Pause/play">
  <svg class="flexi-video__play-icon"><use xlink:href="#play-icon"/></svg>
  <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
  </button>
  <div class="flexi-video__elapsed-time">
  <span class="flexi-video__current-time">0:00</span>
  /<span class="flexi-video__duration">0:00</span>
  </div>
  <div class="flexi-video__progress">
  <div class="flexi-video__progress-track">
  <div class="flexi-video__progress-played"></div>
  <div class="flexi-video__progress-buffered"></div>
  </div>
  </div>
  <button class="flexi-video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
  <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
  </button>
  </div>
  <div class="flexi-video__error-notice">
  <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-video__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  </div>
  </div>
  ````````````````````````````````
  No play icon is rendered if this value is `null`, whitespace or an empty string:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  v{ 
    "src": "/file.mp4",
    "playIcon": null
  }
  --------------- Expected Markup ---------------
  <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_no-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
  <div class="flexi-video__container" tabindex="-1">
  <div class="flexi-video__video-outer-container">
  <div class="flexi-video__video-inner-container">
  <video class="flexi-video__video" preload="auto" muted playsInline disablePictureInPicture loop>
  <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
  </video>
  </div>
  </div>
  <div class="flexi-video__controls">
  <button class="flexi-video__play-pause-button" aria-label="Pause/play">
  <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
  </button>
  <div class="flexi-video__elapsed-time">
  <span class="flexi-video__current-time">0:00</span>
  /<span class="flexi-video__duration">0:00</span>
  </div>
  <div class="flexi-video__progress">
  <div class="flexi-video__progress-track">
  <div class="flexi-video__progress-played"></div>
  <div class="flexi-video__progress-buffered"></div>
  </div>
  </div>
  <button class="flexi-video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
  <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
  </button>
  </div>
  <div class="flexi-video__error-notice">
  <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-video__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  </div>
  </div>
  ````````````````````````````````

##### `PauseIcon`
- Type: `string`
- Description: The `FlexiVideoBlock`'s pause icon as an HTML fragment.
  The class "<`BlockName`>__pause-icon" is assigned to this fragment's first start tag.
  If this value is `null`, whitespace or an empty string, no pause icon is rendered.
- Default: a pause icon
- Examples:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  v{ 
    "src": "/file.mp4",
    "pauseIcon": "<svg><use xlink:href=\"#pause-icon\"/></svg>"
  }
  --------------- Expected Markup ---------------
  <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
  <div class="flexi-video__container" tabindex="-1">
  <div class="flexi-video__video-outer-container">
  <div class="flexi-video__video-inner-container">
  <video class="flexi-video__video" preload="auto" muted playsInline disablePictureInPicture loop>
  <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
  </video>
  </div>
  </div>
  <div class="flexi-video__controls">
  <button class="flexi-video__play-pause-button" aria-label="Pause/play">
  <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  <svg class="flexi-video__pause-icon"><use xlink:href="#pause-icon"/></svg>
  </button>
  <div class="flexi-video__elapsed-time">
  <span class="flexi-video__current-time">0:00</span>
  /<span class="flexi-video__duration">0:00</span>
  </div>
  <div class="flexi-video__progress">
  <div class="flexi-video__progress-track">
  <div class="flexi-video__progress-played"></div>
  <div class="flexi-video__progress-buffered"></div>
  </div>
  </div>
  <button class="flexi-video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
  <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
  </button>
  </div>
  <div class="flexi-video__error-notice">
  <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-video__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  </div>
  </div>
  ````````````````````````````````
  No pause icon is rendered if this value is `null`, whitespace or an empty string:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  v{ 
    "src": "/file.mp4",
    "pauseIcon": null
  }
  --------------- Expected Markup ---------------
  <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_no-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
  <div class="flexi-video__container" tabindex="-1">
  <div class="flexi-video__video-outer-container">
  <div class="flexi-video__video-inner-container">
  <video class="flexi-video__video" preload="auto" muted playsInline disablePictureInPicture loop>
  <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
  </video>
  </div>
  </div>
  <div class="flexi-video__controls">
  <button class="flexi-video__play-pause-button" aria-label="Pause/play">
  <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  </button>
  <div class="flexi-video__elapsed-time">
  <span class="flexi-video__current-time">0:00</span>
  /<span class="flexi-video__duration">0:00</span>
  </div>
  <div class="flexi-video__progress">
  <div class="flexi-video__progress-track">
  <div class="flexi-video__progress-played"></div>
  <div class="flexi-video__progress-buffered"></div>
  </div>
  </div>
  <button class="flexi-video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
  <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
  </button>
  </div>
  <div class="flexi-video__error-notice">
  <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-video__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  </div>
  </div>
  ````````````````````````````````

##### `FullscreenIcon`
- Type: `string`
- Description: The `FlexiVideoBlock`'s fullscreen icon as an HTML fragment.
  The class "<`BlockName`>__fullscreen-icon" is assigned to this fragment's first start tag.
  If this value is `null`, whitespace or an empty string, no fullscreen icon is rendered.
- Default: a fullscreen icon
- Examples:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  v{ 
    "src": "/file.mp4",
    "fullscreenIcon": "<svg><use xlink:href=\"#fullscreen-icon\"/></svg>"
  }
  --------------- Expected Markup ---------------
  <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
  <div class="flexi-video__container" tabindex="-1">
  <div class="flexi-video__video-outer-container">
  <div class="flexi-video__video-inner-container">
  <video class="flexi-video__video" preload="auto" muted playsInline disablePictureInPicture loop>
  <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
  </video>
  </div>
  </div>
  <div class="flexi-video__controls">
  <button class="flexi-video__play-pause-button" aria-label="Pause/play">
  <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
  </button>
  <div class="flexi-video__elapsed-time">
  <span class="flexi-video__current-time">0:00</span>
  /<span class="flexi-video__duration">0:00</span>
  </div>
  <div class="flexi-video__progress">
  <div class="flexi-video__progress-track">
  <div class="flexi-video__progress-played"></div>
  <div class="flexi-video__progress-buffered"></div>
  </div>
  </div>
  <button class="flexi-video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="flexi-video__fullscreen-icon"><use xlink:href="#fullscreen-icon"/></svg>
  <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
  </button>
  </div>
  <div class="flexi-video__error-notice">
  <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-video__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  </div>
  </div>
  ````````````````````````````````
  No fullscreen icon is rendered if this value is `null`, whitespace or an empty string:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  v{ 
    "src": "/file.mp4",
    "fullscreenIcon": null
  }
  --------------- Expected Markup ---------------
  <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_no-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
  <div class="flexi-video__container" tabindex="-1">
  <div class="flexi-video__video-outer-container">
  <div class="flexi-video__video-inner-container">
  <video class="flexi-video__video" preload="auto" muted playsInline disablePictureInPicture loop>
  <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
  </video>
  </div>
  </div>
  <div class="flexi-video__controls">
  <button class="flexi-video__play-pause-button" aria-label="Pause/play">
  <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
  </button>
  <div class="flexi-video__elapsed-time">
  <span class="flexi-video__current-time">0:00</span>
  /<span class="flexi-video__duration">0:00</span>
  </div>
  <div class="flexi-video__progress">
  <div class="flexi-video__progress-track">
  <div class="flexi-video__progress-played"></div>
  <div class="flexi-video__progress-buffered"></div>
  </div>
  </div>
  <button class="flexi-video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
  </button>
  </div>
  <div class="flexi-video__error-notice">
  <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-video__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  </div>
  </div>
  ````````````````````````````````

##### `ExitFullscreenIcon`
- Type: `string`
- Description: The `FlexiVideoBlock`'s exit fullscreen icon as an HTML fragment.
  The class "<`BlockName`>__exit-fullscreen-icon" is assigned to this fragment's first start tag.
  If this value is `null`, whitespace or an empty string, no error icon is rendered.
- Default: an exit fullscreen icon
- Examples:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  v{ 
    "src": "/file.mp4",
    "exitFullscreenIcon": "<svg><use xlink:href=\"#exit-fullscreen-icon\"/></svg>"
  }
  --------------- Expected Markup ---------------
  <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
  <div class="flexi-video__container" tabindex="-1">
  <div class="flexi-video__video-outer-container">
  <div class="flexi-video__video-inner-container">
  <video class="flexi-video__video" preload="auto" muted playsInline disablePictureInPicture loop>
  <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
  </video>
  </div>
  </div>
  <div class="flexi-video__controls">
  <button class="flexi-video__play-pause-button" aria-label="Pause/play">
  <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
  </button>
  <div class="flexi-video__elapsed-time">
  <span class="flexi-video__current-time">0:00</span>
  /<span class="flexi-video__duration">0:00</span>
  </div>
  <div class="flexi-video__progress">
  <div class="flexi-video__progress-track">
  <div class="flexi-video__progress-played"></div>
  <div class="flexi-video__progress-buffered"></div>
  </div>
  </div>
  <button class="flexi-video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
  <svg class="flexi-video__exit-fullscreen-icon"><use xlink:href="#exit-fullscreen-icon"/></svg>
  </button>
  </div>
  <div class="flexi-video__error-notice">
  <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-video__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  </div>
  </div>
  ````````````````````````````````
  No exit fullscreen icon is rendered if this value is `null`, whitespace or an empty string:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  v{ 
    "src": "/file.mp4",
    "exitFullscreenIcon": null
  }
  --------------- Expected Markup ---------------
  <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_no-exit-fullscreen-icon flexi-video_has-error-icon">
  <div class="flexi-video__container" tabindex="-1">
  <div class="flexi-video__video-outer-container">
  <div class="flexi-video__video-inner-container">
  <video class="flexi-video__video" preload="auto" muted playsInline disablePictureInPicture loop>
  <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
  </video>
  </div>
  </div>
  <div class="flexi-video__controls">
  <button class="flexi-video__play-pause-button" aria-label="Pause/play">
  <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
  </button>
  <div class="flexi-video__elapsed-time">
  <span class="flexi-video__current-time">0:00</span>
  /<span class="flexi-video__duration">0:00</span>
  </div>
  <div class="flexi-video__progress">
  <div class="flexi-video__progress-track">
  <div class="flexi-video__progress-played"></div>
  <div class="flexi-video__progress-buffered"></div>
  </div>
  </div>
  <button class="flexi-video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
  </button>
  </div>
  <div class="flexi-video__error-notice">
  <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-video__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  </div>
  </div>
  ````````````````````````````````

##### `ErrorIcon`
- Type: `string`
- Description: The `FlexiVideoBlock`'s error icon as an HTML fragment.
  The class "<`BlockName`>__error-icon" is assigned to this fragment's first start tag.
  If this value is `null`, whitespace or an empty string, no error icon is rendered.
- Default: the [Material Design error icon](https://material.io/tools/icons/?icon=error&style=baseline)
- Examples:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  v{ 
    "src": "/file.mp4",
    "errorIcon": "<svg><use xlink:href=\"#error-icon\"/></svg>"
  }
  --------------- Expected Markup ---------------
  <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
  <div class="flexi-video__container" tabindex="-1">
  <div class="flexi-video__video-outer-container">
  <div class="flexi-video__video-inner-container">
  <video class="flexi-video__video" preload="auto" muted playsInline disablePictureInPicture loop>
  <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
  </video>
  </div>
  </div>
  <div class="flexi-video__controls">
  <button class="flexi-video__play-pause-button" aria-label="Pause/play">
  <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
  </button>
  <div class="flexi-video__elapsed-time">
  <span class="flexi-video__current-time">0:00</span>
  /<span class="flexi-video__duration">0:00</span>
  </div>
  <div class="flexi-video__progress">
  <div class="flexi-video__progress-track">
  <div class="flexi-video__progress-played"></div>
  <div class="flexi-video__progress-buffered"></div>
  </div>
  </div>
  <button class="flexi-video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
  <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
  </button>
  </div>
  <div class="flexi-video__error-notice">
  <svg class="flexi-video__error-icon"><use xlink:href="#error-icon"/></svg>
  </div>
  <div class="flexi-video__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  </div>
  </div>
  ````````````````````````````````
  No error icon is rendered if this value is `null`, whitespace or an empty string:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  v{ 
    "src": "/file.mp4",
    "errorIcon": null
  }
  --------------- Expected Markup ---------------
  <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_no-error-icon">
  <div class="flexi-video__container" tabindex="-1">
  <div class="flexi-video__video-outer-container">
  <div class="flexi-video__video-inner-container">
  <video class="flexi-video__video" preload="auto" muted playsInline disablePictureInPicture loop>
  <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
  </video>
  </div>
  </div>
  <div class="flexi-video__controls">
  <button class="flexi-video__play-pause-button" aria-label="Pause/play">
  <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
  </button>
  <div class="flexi-video__elapsed-time">
  <span class="flexi-video__current-time">0:00</span>
  /<span class="flexi-video__duration">0:00</span>
  </div>
  <div class="flexi-video__progress">
  <div class="flexi-video__progress-track">
  <div class="flexi-video__progress-played"></div>
  <div class="flexi-video__progress-buffered"></div>
  </div>
  </div>
  <button class="flexi-video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
  <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
  </button>
  </div>
  <div class="flexi-video__error-notice">
  </div>
  <div class="flexi-video__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  </div>
  </div>
  ````````````````````````````````

##### `EnableFileOperations`
- Type: `bool`
- Description: The value specifying whether file operations are enabled for the `FlexiVideoBlock`.
  If this value is `true` and
  `IFlexiVideoBlocksExtensionOptions.LocalMediaDirectory` is not `null`, whitespace or an empty string and
  `Width`, `Height` or `Duration` is less than or equal to 0 or we need to generate a poster,
  `IFlexiVideoBlocksExtensionOptions.LocalMediaDirectory` is searched recursively for a file with `Src`'s file name,
  and the necessary file operations are performed on the file.
- Default: true

##### `Attributes`
- Type: `IDictionary<string, string>`
- Description: The HTML attributes for the `FlexiVideoBlock`'s root element.
  Attribute names must be lowercase.
  If classes are specified, they are appended to default classes. This facilitates [BEM mixes](https://en.bem.info/methodology/quick-start/#mix).
  If this value is `null`, default classes are still assigned to the root element.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  v{ 
    "src": "/file.mp4",
    "attributes": {
        "id" : "my-custom-id",
        "class" : "my-custom-class"
    }
  }
  --------------- Expected Markup ---------------
  <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon my-custom-class" id="my-custom-id">
  <div class="flexi-video__container" tabindex="-1">
  <div class="flexi-video__video-outer-container">
  <div class="flexi-video__video-inner-container">
  <video class="flexi-video__video" preload="auto" muted playsInline disablePictureInPicture loop>
  <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
  </video>
  </div>
  </div>
  <div class="flexi-video__controls">
  <button class="flexi-video__play-pause-button" aria-label="Pause/play">
  <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
  </button>
  <div class="flexi-video__elapsed-time">
  <span class="flexi-video__current-time">0:00</span>
  /<span class="flexi-video__duration">0:00</span>
  </div>
  <div class="flexi-video__progress">
  <div class="flexi-video__progress-track">
  <div class="flexi-video__progress-played"></div>
  <div class="flexi-video__progress-buffered"></div>
  </div>
  </div>
  <button class="flexi-video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
  <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
  </button>
  </div>
  <div class="flexi-video__error-notice">
  <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-video__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  </div>
  </div>
  ````````````````````````````````

### `FlexiVideoBlocksExtensionOptions`
Options for the FlexiVideoBlocks extension. There are two ways to specify these options:
- Pass a `FlexiVideoBlocksExtensionOptions` when calling `MarkdownPipelineBuilderExtensions.UseFlexiVideoBlocks(this MarkdownPipelineBuilder pipelineBuilder, IFlexiVideoBlocksExtensionOptions options)`.
- Insert a `FlexiVideoBlocksExtensionOptions` into a `MarkdownParserContext.Properties` with key `typeof(IFlexiVideoBlocksExtensionOptions)`. Pass the `MarkdownParserContext` when you call a markdown processing method
  like `Markdown.ToHtml(markdown, stringWriter, markdownPipeline, yourMarkdownParserContext)`.  
  This method allows for different extension options when reusing a pipeline. Options specified using this method take precedence.

#### Constructor Parameters

##### `defaultBlockOptions`
- Type: `IFlexiVideoBlockOptions`
- Description: Default `IFlexiVideoBlockOptions` for all `FlexiVideoBlock`s.
  If this value is `null`, a `FlexiVideoBlockOptions` with default values is used.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extension Options ---------------
  {
      "flexiVideoBlocks": {
          "defaultBlockOptions": {
              "errorIcon": "<svg><use xlink:href=\"#error-icon\"/></svg>",
              "attributes": {
                  "class": "block"
              }
          }
      }
  }
  --------------- Markdown ---------------
  v{ 
    "src": "/file.mp4"
  }
  --------------- Expected Markup ---------------
  <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon block">
  <div class="flexi-video__container" tabindex="-1">
  <div class="flexi-video__video-outer-container">
  <div class="flexi-video__video-inner-container">
  <video class="flexi-video__video" preload="auto" muted playsInline disablePictureInPicture loop>
  <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
  </video>
  </div>
  </div>
  <div class="flexi-video__controls">
  <button class="flexi-video__play-pause-button" aria-label="Pause/play">
  <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
  </button>
  <div class="flexi-video__elapsed-time">
  <span class="flexi-video__current-time">0:00</span>
  /<span class="flexi-video__duration">0:00</span>
  </div>
  <div class="flexi-video__progress">
  <div class="flexi-video__progress-track">
  <div class="flexi-video__progress-played"></div>
  <div class="flexi-video__progress-buffered"></div>
  </div>
  </div>
  <button class="flexi-video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
  <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
  </button>
  </div>
  <div class="flexi-video__error-notice">
  <svg class="flexi-video__error-icon"><use xlink:href="#error-icon"/></svg>
  </div>
  <div class="flexi-video__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  </div>
  </div>
  ````````````````````````````````
  `defaultBlockOptions` has lower precedence than block specific options:
  ```````````````````````````````` none
  --------------- Extension Options ---------------
  {
      "flexiVideoBlocks": {
          "defaultBlockOptions": {
              "blockName": "video"
          }
      }
  }
  --------------- Markdown ---------------
  v{ 
    "src": "/file.mp4"
  }

  v{ 
    "blockname": "special-video",
    "src": "/file.mp4"
  }
  --------------- Expected Markup ---------------
  <div class="video video_no-poster video_no-width video_no-aspect-ratio video_no-duration video_has-type video_has-spinner video_has-play-icon video_has-pause-icon video_has-fullscreen-icon video_has-exit-fullscreen-icon video_has-error-icon">
  <div class="video__container" tabindex="-1">
  <div class="video__video-outer-container">
  <div class="video__video-inner-container">
  <video class="video__video" preload="auto" muted playsInline disablePictureInPicture loop>
  <source class="video__source" data-src="/file.mp4" type="video/mp4">
  </video>
  </div>
  </div>
  <div class="video__controls">
  <button class="video__play-pause-button" aria-label="Pause/play">
  <svg class="video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  <svg class="video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
  </button>
  <div class="video__elapsed-time">
  <span class="video__current-time">0:00</span>
  /<span class="video__duration">0:00</span>
  </div>
  <div class="video__progress">
  <div class="video__progress-track">
  <div class="video__progress-played"></div>
  <div class="video__progress-buffered"></div>
  </div>
  </div>
  <button class="video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
  <svg class="video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
  </button>
  </div>
  <div class="video__error-notice">
  <svg class="video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="video__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  </div>
  </div>
  <div class="special-video special-video_no-poster special-video_no-width special-video_no-aspect-ratio special-video_no-duration special-video_has-type special-video_has-spinner special-video_has-play-icon special-video_has-pause-icon special-video_has-fullscreen-icon special-video_has-exit-fullscreen-icon special-video_has-error-icon">
  <div class="special-video__container" tabindex="-1">
  <div class="special-video__video-outer-container">
  <div class="special-video__video-inner-container">
  <video class="special-video__video" preload="auto" muted playsInline disablePictureInPicture loop>
  <source class="special-video__source" data-src="/file.mp4" type="video/mp4">
  </video>
  </div>
  </div>
  <div class="special-video__controls">
  <button class="special-video__play-pause-button" aria-label="Pause/play">
  <svg class="special-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  <svg class="special-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
  </button>
  <div class="special-video__elapsed-time">
  <span class="special-video__current-time">0:00</span>
  /<span class="special-video__duration">0:00</span>
  </div>
  <div class="special-video__progress">
  <div class="special-video__progress-track">
  <div class="special-video__progress-played"></div>
  <div class="special-video__progress-buffered"></div>
  </div>
  </div>
  <button class="special-video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="special-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
  <svg class="special-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
  </button>
  </div>
  <div class="special-video__error-notice">
  <svg class="special-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="special-video__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  </div>
  </div>
  ````````````````````````````````

##### `localMediaDirectory`
- Type: `string`
- Description: The local directory to search for video files in.
  If this value is `null`, whitespace or an empty string, file operations are disabled for all `FlexiVideoBlock`s.
  This value must be an absolute URI with the file scheme (points to a local directory).
- Default: `null`

##### `mimeTypes`
- Type: `IDictionary<string, string>`
- Description: A map of MIME types to file extensions.
  If this value is `null`, a map of MIME types for file extensions ".mp4", ".webm" and ".ogg" is used.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extension Options ---------------
  {
      "flexiVideoBlocks": {
          "mimeTypes": {
              ".3gp": "video/3gpp",
              ".mov": "video/quicktime"
          }
      }
  }
  --------------- Markdown ---------------
  v{ 
    "src": "/file.3gp"
  }

  v{ 
    "src": "/file.mov"
  }
  --------------- Expected Markup ---------------
  <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
  <div class="flexi-video__container" tabindex="-1">
  <div class="flexi-video__video-outer-container">
  <div class="flexi-video__video-inner-container">
  <video class="flexi-video__video" preload="auto" muted playsInline disablePictureInPicture loop>
  <source class="flexi-video__source" data-src="/file.3gp" type="video/3gpp">
  </video>
  </div>
  </div>
  <div class="flexi-video__controls">
  <button class="flexi-video__play-pause-button" aria-label="Pause/play">
  <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
  </button>
  <div class="flexi-video__elapsed-time">
  <span class="flexi-video__current-time">0:00</span>
  /<span class="flexi-video__duration">0:00</span>
  </div>
  <div class="flexi-video__progress">
  <div class="flexi-video__progress-track">
  <div class="flexi-video__progress-played"></div>
  <div class="flexi-video__progress-buffered"></div>
  </div>
  </div>
  <button class="flexi-video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
  <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
  </button>
  </div>
  <div class="flexi-video__error-notice">
  <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-video__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  </div>
  </div>
  <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
  <div class="flexi-video__container" tabindex="-1">
  <div class="flexi-video__video-outer-container">
  <div class="flexi-video__video-inner-container">
  <video class="flexi-video__video" preload="auto" muted playsInline disablePictureInPicture loop>
  <source class="flexi-video__source" data-src="/file.mov" type="video/quicktime">
  </video>
  </div>
  </div>
  <div class="flexi-video__controls">
  <button class="flexi-video__play-pause-button" aria-label="Pause/play">
  <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
  </button>
  <div class="flexi-video__elapsed-time">
  <span class="flexi-video__current-time">0:00</span>
  /<span class="flexi-video__duration">0:00</span>
  </div>
  <div class="flexi-video__progress">
  <div class="flexi-video__progress-track">
  <div class="flexi-video__progress-played"></div>
  <div class="flexi-video__progress-buffered"></div>
  </div>
  </div>
  <button class="flexi-video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
  <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
  </button>
  </div>
  <div class="flexi-video__error-notice">
  <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-video__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  </div>
  </div>
  ````````````````````````````````
  Default MIME types:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  v{ 
    "src": "/file.mp4"
  }

  v{ 
    "src": "/file.webm"
  }

  v{ 
    "src": "/file.ogg"
  }
  --------------- Expected Markup ---------------
  <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
  <div class="flexi-video__container" tabindex="-1">
  <div class="flexi-video__video-outer-container">
  <div class="flexi-video__video-inner-container">
  <video class="flexi-video__video" preload="auto" muted playsInline disablePictureInPicture loop>
  <source class="flexi-video__source" data-src="/file.mp4" type="video/mp4">
  </video>
  </div>
  </div>
  <div class="flexi-video__controls">
  <button class="flexi-video__play-pause-button" aria-label="Pause/play">
  <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
  </button>
  <div class="flexi-video__elapsed-time">
  <span class="flexi-video__current-time">0:00</span>
  /<span class="flexi-video__duration">0:00</span>
  </div>
  <div class="flexi-video__progress">
  <div class="flexi-video__progress-track">
  <div class="flexi-video__progress-played"></div>
  <div class="flexi-video__progress-buffered"></div>
  </div>
  </div>
  <button class="flexi-video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
  <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
  </button>
  </div>
  <div class="flexi-video__error-notice">
  <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-video__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  </div>
  </div>
  <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
  <div class="flexi-video__container" tabindex="-1">
  <div class="flexi-video__video-outer-container">
  <div class="flexi-video__video-inner-container">
  <video class="flexi-video__video" preload="auto" muted playsInline disablePictureInPicture loop>
  <source class="flexi-video__source" data-src="/file.webm" type="video/webm">
  </video>
  </div>
  </div>
  <div class="flexi-video__controls">
  <button class="flexi-video__play-pause-button" aria-label="Pause/play">
  <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
  </button>
  <div class="flexi-video__elapsed-time">
  <span class="flexi-video__current-time">0:00</span>
  /<span class="flexi-video__duration">0:00</span>
  </div>
  <div class="flexi-video__progress">
  <div class="flexi-video__progress-track">
  <div class="flexi-video__progress-played"></div>
  <div class="flexi-video__progress-buffered"></div>
  </div>
  </div>
  <button class="flexi-video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
  <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
  </button>
  </div>
  <div class="flexi-video__error-notice">
  <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-video__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  </div>
  </div>
  <div class="flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon">
  <div class="flexi-video__container" tabindex="-1">
  <div class="flexi-video__video-outer-container">
  <div class="flexi-video__video-inner-container">
  <video class="flexi-video__video" preload="auto" muted playsInline disablePictureInPicture loop>
  <source class="flexi-video__source" data-src="/file.ogg" type="video/ogg">
  </video>
  </div>
  </div>
  <div class="flexi-video__controls">
  <button class="flexi-video__play-pause-button" aria-label="Pause/play">
  <svg class="flexi-video__play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  <svg class="flexi-video__pause-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
  </button>
  <div class="flexi-video__elapsed-time">
  <span class="flexi-video__current-time">0:00</span>
  /<span class="flexi-video__duration">0:00</span>
  </div>
  <div class="flexi-video__progress">
  <div class="flexi-video__progress-track">
  <div class="flexi-video__progress-played"></div>
  <div class="flexi-video__progress-buffered"></div>
  </div>
  </div>
  <button class="flexi-video__fullscreen-button" aria-label="Toggle fullscreen">
  <svg class="flexi-video__fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z"/></svg>
  <svg class="flexi-video__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path shape-rendering="crispEdges" d="M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z"/></svg>
  </button>
  </div>
  <div class="flexi-video__error-notice">
  <svg class="flexi-video__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-video__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  </div>
  </div>
  ````````````````````````````````

## Incomplete Features

Refer to [Whatwg HTML Spec - Video Element](https://html.spec.whatwg.org/multipage/media.html#the-video-element) for a good summary on the video element.

### Viewport-Based Resource Selection

This is where different resources are retrieved depending on viewport size. Benefits include 
lower data usage, quicker page loading and possibly, better aesthetics (if we use different video compositions for different 
viewport sizes). Given a base video, this extension could generate resources for different viewport sizes. Example markup:

```
<video>
  <source media="(min-width: 1000px)" srcset="./example-video-400px.mp4">
  <source media="(min-width: 500px)" srcset="./example-video-200px.mp4">
  <source src="./example-video-100px.mp4">
</video>
```

The `srcset` attribute of `source` elements can be used to specify different resources for different device-pixel-ratios:

```
<video>
  <source media="(min-width: 1000px)" srcset="./example-video-400px.mp4, ./example-video-800px.mp4 2x">
  <source media="(min-width: 500px)" srcset="./example-video-200px.mp4, ./example-video-400px.mp4 2x">
  <source srcset="./example-video-100px.mp4, ./example-video-200px.mp4 2x">
</video>
```

### Format-Based Resource Selection

This is where different resources are retrieved depending on user-agent support for formats. Benefits include 
lower data usage and quicker page loading. Given a base video, this extension could generate resources in 
different formats. Example markup:

```
<video>
 <source srcset="./example-video.webm" type="video/webm">
 <source srcset="./example-video.mp4" type="video/mp4">
 <source src="./example-video.mp4">
</video>
```

Format-based and viewport-based resource selection can be mixed: 

```
<video>
  <source media="(min-width: 1000px)" srcset="./example-video-400px.webm, ./example-video-800px.webm 2x" type="video/webm">
  <source media="(min-width: 1000px)" srcset="./example-video-400px.mp4, ./example-video-800px.mp4 2x" type="video/mp4">
  <source media="(min-width: 500px)" srcset="./example-video-200px.webm, ./example-video-400px.webm 2x" type="video/webm">
  <source media="(min-width: 500px)" srcset="./example-video-200px.mp4, ./example-video-400px.mp4 2x" type="video/mp4">
  <source srcset="./example-video-100px.mp4, ./example-video-200px.mp4 2x">
</video>
```

Note how we'd have to generate lots of resources and maintain pretty verbose markup if we want to manually cover viewport size/device pixel ratio/format 
support differences. The FlexiVideoBlocks extension could simpifly things, for example, the above markup and associated resources
could be generated from:

v{
    "src": "./example-video.mp4",
    "breakpoints": [ 
        { "(min-width: 1000px)", 400 },
        { "(min-width: 500px)", 200 },
        { "default", 100 }
    ],
    "formats": [ "webm", "mp4" ]
    "devicePixelRatios": [ 1, 2 ]
}

Formats and device pixel ratios could be defaults - not necessary to specify them for every FlexiVideoBlock.

## Avoiding page reflow on responsive video load.  

If the width and height attributes of a video element are specified, the browser uses them to calculate its aspect ratio.
*However*, if in CSS its width and height properties specified, the attribute values and the aspect ratio derived from them are
ignored. We have to set `max-width: 100%`, `height: auto` for video elements if we want them to be responsive, so without some hacking,
we get a page reflow on responsive video load.

At present, we're hacking around the problem using percentage based padding - https://www.voorhoede.nl/en/blog/say-no-to-video-reflow/.
The CSS working group is working on a far cleaner solution to this issue - https://github.com/WICG/intrinsicsize-attribute/issues/16.
When it has been adopted, we should render width and height attributes on video elements.
