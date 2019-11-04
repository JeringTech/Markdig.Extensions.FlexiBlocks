---
blockOptions: "../src/FlexiBlocks/Extensions/FlexiPictureBlocks/FlexiPictureBlockOptions.cs"
extensionOptions: "../src/FlexiBlocks/Extensions/FlexiPictureBlocks/FlexiPictureBlocksExtensionOptions.cs"
requiresOptionsExtension: false
---

# FlexiPictureBlocks
A FlexiPictureBlock displays an image.

## Usage
```csharp
using Markdig;
using Jering.Markdig.Extensions.FlexiBlocks;

...
var markdownPipelineBuilder = new MarkdownPipelineBuilder();
markdownPipelineBuilder.UseFlexiPictureBlocks(/* Optional extension options */);

MarkdownPipeline markdownPipeline = markdownPipelineBuilder.Build();

string markdown = @"p{ 
  ""src"": ""/file.png"",
  ""alt"": ""Alternative text""
}";
string html = Markdown.ToHtml(markdown, markdownPipeline);
string expectedHtml = @"<div class=""flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner"">
<button class=""flexi-picture__exit-fullscreen-button"" aria-label=""Exit fullscreen"">
<svg class=""flexi-picture__exit-fullscreen-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path d=""M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z""/><path d=""M0 0h24v24H0z"" fill=""none""/></svg>
</button>
<div class=""flexi-picture__container"">
<div class=""flexi-picture__error-notice"">
<svg class=""flexi-picture__error-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path d=""M0 0h24v24H0z"" fill=""none""/><path d=""M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z""/></svg>
</div>
<div class=""flexi-picture__spinner spinner"">
    <div class=""spinner__rects"">
        <div class=""spinner__rect-1""></div>
        <div class=""spinner__rect-2""></div>
        <div class=""spinner__rect-3""></div>
    </div>
</div>
<div class=""flexi-picture__picture-container"">
<picture class=""flexi-picture__picture"">
<img class=""flexi-picture__image"" data-src=""/file.png"" alt=""Alternative text"" tabindex=""-1"">
</picture>
</div>
</div>
</div>";

Assert.Equal(expectedHtml, html)
```

# Basics
In markdown, a FlexiPictureBlock is a [`FlexiPictureBlockOptions`][options] object in JSON form, prepended with `p`. For example:

```````````````````````````````` none
--------------- Markdown ---------------
p{ 
  "src": "/file.png",
  "alt": "Alternative text"
}
--------------- Expected Markup ---------------
<div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner">
<button class="flexi-picture__exit-fullscreen-button" aria-label="Exit fullscreen">
<svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
</button>
<div class="flexi-picture__container">
<div class="flexi-picture__error-notice">
<svg class="flexi-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
</div>
<div class="flexi-picture__spinner spinner">
    <div class="spinner__rects">
        <div class="spinner__rect-1"></div>
        <div class="spinner__rect-2"></div>
        <div class="spinner__rect-3"></div>
    </div>
</div>
<div class="flexi-picture__picture-container">
<picture class="flexi-picture__picture">
<img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
</picture>
</div>
</div>
</div>
````````````````````````````````

! Generated elements are assigned classes that comply with [BEM methodology](https://en.bem.info/). These classes can be customized. We explain how in [options].

## Options
### `FlexiPictureBlockOptions`
Options for a FlexiPictureBlock.

#### Properties

##### `BlockName`
- Type: `string`
- Description: The `FlexiPictureBlock`'s [BEM block name](https://en.bem.info/methodology/naming-convention/#block-name).
  In compliance with [BEM methodology](https://en.bem.info), this value is the `FlexiPictureBlock`'s root element's class as well as the prefix for all other classes in the block.
  This value should contain only valid [CSS class characters](https://www.w3.org/TR/CSS21/syndata.html#characters).
  If this value is `null`, whitespace or an empty string, the `FlexiPictureBlock`'s block name is "flexi-picture".
- Default: "flexi-picture"
- Examples:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  p{ 
    "blockName": "picture",
    "src": "/file.png",
    "alt": "Alternative text"
  }
  --------------- Expected Markup ---------------
  <div class="picture picture_has-alt picture_is-lazy picture_no-width picture_no-aspect-ratio picture_has-exit-fullscreen-icon picture_has-error-icon picture_has-spinner">
  <button class="picture__exit-fullscreen-button" aria-label="Exit fullscreen">
  <svg class="picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  </button>
  <div class="picture__container">
  <div class="picture__error-notice">
  <svg class="picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="picture__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  <div class="picture__picture-container">
  <picture class="picture__picture">
  <img class="picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
  </picture>
  </div>
  </div>
  </div>
  ````````````````````````````````

##### `Src`
- Type: `string`
- Description: The `FlexiPictureBlock`'s source URI.
  This value is assigned to the img element's data-src attribute if `Lazy` is true, otherwise it is assigned to the img element's src attribute.
  This value is required and must be a valid URI pointing to a file.
- Examples:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  p{ 
    "src": "/file.png",
    "alt": "Alternative text"
  }
  --------------- Expected Markup ---------------
  <div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner">
  <button class="flexi-picture__exit-fullscreen-button" aria-label="Exit fullscreen">
  <svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  </button>
  <div class="flexi-picture__container">
  <div class="flexi-picture__error-notice">
  <svg class="flexi-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-picture__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  <div class="flexi-picture__picture-container">
  <picture class="flexi-picture__picture">
  <img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
  </picture>
  </div>
  </div>
  </div>
  ````````````````````````````````

##### `Alt`
- Type: `string`
- Description: The `FlexiPictureBlock`'s alt text.
  This value is assigned to the img element's alt attribute.
  If this value is `null`, whitespace or an empty string, the alt attribute is not rendered.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  p{ 
    "src": "/file.png",
    "alt": "Alternative text"
  }
  --------------- Expected Markup ---------------
  <div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner">
  <button class="flexi-picture__exit-fullscreen-button" aria-label="Exit fullscreen">
  <svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  </button>
  <div class="flexi-picture__container">
  <div class="flexi-picture__error-notice">
  <svg class="flexi-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-picture__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  <div class="flexi-picture__picture-container">
  <picture class="flexi-picture__picture">
  <img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
  </picture>
  </div>
  </div>
  </div>
  ````````````````````````````````
  The alt attribute isn't rendered if this value is null, whitespace or an empty string:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  p{ 
    "src": "/file.png",
    "alt": null
  }
  --------------- Expected Markup ---------------
  <div class="flexi-picture flexi-picture_no-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner">
  <button class="flexi-picture__exit-fullscreen-button" aria-label="Exit fullscreen">
  <svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  </button>
  <div class="flexi-picture__container">
  <div class="flexi-picture__error-notice">
  <svg class="flexi-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-picture__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  <div class="flexi-picture__picture-container">
  <picture class="flexi-picture__picture">
  <img class="flexi-picture__image" data-src="/file.png" tabindex="-1">
  </picture>
  </div>
  </div>
  </div>
  ````````````````````````````````

##### `Lazy`
- Type: `bool`
- Description: The value specifying whether the `FlexiPictureBlock` loads lazily.
  If this value is `true`, `Src` is assigned to the img element's data-src attribute.
  On the client, the data-src attribute's value is copied to the src attribute when the `FlexiPictureBlock` is almost visible.
  Browsers automatically begin loading the img element once its src attribute is set.
  If this value is `false`, `Src` is assigned to the img element's src attribute.
  This value should be false if the `FlexiPictureBlock`s is immediately visible on page load (above-the-fold) and true otherwise.
  Benefits of lazy loading include [reducing initial page load time, initial page weight, and system resource usage](https://developers.google.com/web/fundamentals/performance/lazy-loading-guidance/images-and-video#why_lazy_load_images_or_video_instead_of_just_loading_them).
  [Chrome recently implemented native lazy loading](https://web.dev/native-lazy-loading), unfortunately native lazy loading isn't widely supported across browsers, so we don't support native lazy loading yet.
- Default: `true`
- Examples:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  p{ 
    "src": "/file.png",
    "alt": "Alternative text"
  }
  --------------- Expected Markup ---------------
  <div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner">
  <button class="flexi-picture__exit-fullscreen-button" aria-label="Exit fullscreen">
  <svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  </button>
  <div class="flexi-picture__container">
  <div class="flexi-picture__error-notice">
  <svg class="flexi-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-picture__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  <div class="flexi-picture__picture-container">
  <picture class="flexi-picture__picture">
  <img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
  </picture>
  </div>
  </div>
  </div>
  ````````````````````````````````
  `Src` is assigned to the img element's src attribute if `Lazy` is false:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  p{ 
    "src": "/file.png",
    "alt": "Alternative text",
    "lazy": false
  }
  --------------- Expected Markup ---------------
  <div class="flexi-picture flexi-picture_has-alt flexi-picture_not-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner">
  <button class="flexi-picture__exit-fullscreen-button" aria-label="Exit fullscreen">
  <svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  </button>
  <div class="flexi-picture__container">
  <div class="flexi-picture__error-notice">
  <svg class="flexi-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-picture__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  <div class="flexi-picture__picture-container">
  <picture class="flexi-picture__picture">
  <img class="flexi-picture__image" src="/file.png" alt="Alternative text" tabindex="-1">
  </picture>
  </div>
  </div>
  </div>
  ````````````````````````````````

##### `Width`
- Type: `double`
- Description: The `FlexiPictureBlock`'s width.
  If this value is larger than 0, it is assigned to width style properties of several elements.
  If this value and `Height` are both larger than 0, they're used to calculate the `FlexiPictureBlock`'s aspect ratio,
  which is assigned to a padding-bottom style property.
  The width and padding-bottom style properties [ensure that there is no reflow on img element load](https://www.voorhoede.nl/en/blog/say-no-to-image-reflow/).
  The [CSS Working Group](https://github.com/WICG/intrinsicsize-attribute/issues/16) have proposed a solution to content reflow on img element loads.
  Unfortunately, the solution isn't widely supported, so we do not support it yet.
  If this value is larger than 0, it takes precedence over any width retrieved by file operations.
- Default: 0
- Examples:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  p{ 
    "src": "/file.png",
    "alt": "Alternative text",
    "width": 123
  }
  --------------- Expected Markup ---------------
  <div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_has-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner">
  <button class="flexi-picture__exit-fullscreen-button" aria-label="Exit fullscreen">
  <svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  </button>
  <div class="flexi-picture__container" style="width:123px">
  <div class="flexi-picture__error-notice">
  <svg class="flexi-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-picture__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  <div class="flexi-picture__picture-container" style="width:123px">
  <picture class="flexi-picture__picture">
  <img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
  </picture>
  </div>
  </div>
  </div>
  ````````````````````````````````

##### `Height`
- Type: `double`
- Description: The `FlexiPictureBlock`'s height.
  If this value and `Width` are both larger than 0, they're used to calculate the `FlexiPictureBlock`'s aspect ratio,
  which is assigned to a padding-bottom style property.
  The padding-bottom style property [helps ensure that there is no reflow
  on img element load](https://www.voorhoede.nl/en/blog/say-no-to-image-reflow/).
  The [CSS Working Group](https://github.com/WICG/intrinsicsize-attribute/issues/16) have proposed a solution to content reflow on img element loads.
  Unfortunately, the solution isn't widely supported, so we do not support it yet.
  If this value is larger than 0, it takes precedence over any height retrieved by file operations.
- Default: 0
- Examples:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  p{ 
    "src": "/file.png",
    "alt": "Alternative text",
    "width": 123,
    "height": 321
  }
  --------------- Expected Markup ---------------
  <div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_has-width flexi-picture_has-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner">
  <button class="flexi-picture__exit-fullscreen-button" aria-label="Exit fullscreen">
  <svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  </button>
  <div class="flexi-picture__container" style="width:123px">
  <div class="flexi-picture__error-notice">
  <svg class="flexi-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-picture__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  <div class="flexi-picture__picture-container" style="width:123px">
  <picture class="flexi-picture__picture" style="padding-bottom:260.975609756098%">
  <img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
  </picture>
  </div>
  </div>
  </div>
  ````````````````````````````````
  This option has no effect if width is less than or equal to 0:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  p{ 
    "src": "/file.png",
    "alt": "Alternative text",
    "width": 0,
    "height": 321
  }
  --------------- Expected Markup ---------------
  <div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner">
  <button class="flexi-picture__exit-fullscreen-button" aria-label="Exit fullscreen">
  <svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  </button>
  <div class="flexi-picture__container">
  <div class="flexi-picture__error-notice">
  <svg class="flexi-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-picture__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  <div class="flexi-picture__picture-container">
  <picture class="flexi-picture__picture">
  <img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
  </picture>
  </div>
  </div>
  </div>
  ````````````````````````````````

##### `ExitFullscreenIcon`
- Type: `string`
- Description: The `FlexiPictureBlock`'s exit fullscreen icon as an HTML fragment.
  The class "<`BlockName`>__exit-fullscreen-icon" is assigned to this fragment's first start tag.
  If this value is `null`, whitespace or an empty string, no exit fullscreen icon is rendered.
- Default: the [Material Design clear icon](https://material.io/tools/icons/?icon=clear&style=baseline)
- Examples:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  p{ 
    "src": "/file.png",
    "alt": "Alternative text",
    "exitFullscreenIcon": "<svg><use xlink:href=\"#exit-fullscreen-icon\"/></svg>"
  }
  --------------- Expected Markup ---------------
  <div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner">
  <button class="flexi-picture__exit-fullscreen-button" aria-label="Exit fullscreen">
  <svg class="flexi-picture__exit-fullscreen-icon"><use xlink:href="#exit-fullscreen-icon"/></svg>
  </button>
  <div class="flexi-picture__container">
  <div class="flexi-picture__error-notice">
  <svg class="flexi-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-picture__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  <div class="flexi-picture__picture-container">
  <picture class="flexi-picture__picture">
  <img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
  </picture>
  </div>
  </div>
  </div>
  ````````````````````````````````
  No exit fullscreen icon is rendered if this value is `null`, whitespace or an empty string:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  p{ 
    "src": "/file.png",
    "alt": "Alternative text",
    "exitFullscreenIcon": null
  }
  --------------- Expected Markup ---------------
  <div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_no-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner">
  <button class="flexi-picture__exit-fullscreen-button" aria-label="Exit fullscreen">
  </button>
  <div class="flexi-picture__container">
  <div class="flexi-picture__error-notice">
  <svg class="flexi-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-picture__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  <div class="flexi-picture__picture-container">
  <picture class="flexi-picture__picture">
  <img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
  </picture>
  </div>
  </div>
  </div>
  ````````````````````````````````

##### `ErrorIcon`
- Type: `string`
- Description: The `FlexiPictureBlock`'s error icon as an HTML fragment.
  The class "<`BlockName`>__error-icon" is assigned to this fragment's first start tag.
  If this value is `null`, whitespace or an empty string, no error icon is rendered.
- Default: the [Material Design error icon](https://material.io/tools/icons/?icon=error&style=baseline)
- Examples:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  p{ 
    "src": "/file.png",
    "alt": "Alternative text",
    "errorIcon": "<svg><use xlink:href=\"#error-icon\"/></svg>"
  }
  --------------- Expected Markup ---------------
  <div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner">
  <button class="flexi-picture__exit-fullscreen-button" aria-label="Exit fullscreen">
  <svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  </button>
  <div class="flexi-picture__container">
  <div class="flexi-picture__error-notice">
  <svg class="flexi-picture__error-icon"><use xlink:href="#error-icon"/></svg>
  </div>
  <div class="flexi-picture__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  <div class="flexi-picture__picture-container">
  <picture class="flexi-picture__picture">
  <img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
  </picture>
  </div>
  </div>
  </div>
  ````````````````````````````````
  No error icon is rendered if this value is `null`, whitespace or an empty string:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  p{ 
    "src": "/file.png",
    "alt": "Alternative text",
    "errorIcon": null
  }
  --------------- Expected Markup ---------------
  <div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_no-error-icon flexi-picture_has-spinner">
  <button class="flexi-picture__exit-fullscreen-button" aria-label="Exit fullscreen">
  <svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  </button>
  <div class="flexi-picture__container">
  <div class="flexi-picture__error-notice">
  </div>
  <div class="flexi-picture__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  <div class="flexi-picture__picture-container">
  <picture class="flexi-picture__picture">
  <img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
  </picture>
  </div>
  </div>
  </div>
  ````````````````````````````````

##### `Spinner`
- Type: `string`
- Description: The `FlexiPictureBlock`'s spinner as an HTML fragment.
  The class "<`BlockName`>__spinner" is assigned to this fragment's first start tag.
  If this value is `null`, whitespace or an empty string, no spinner is rendered.
- Default: a simple spinner
- Examples:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  p{ 
    "src": "/file.png",
    "alt": "Alternative text",
    "spinner": "<div class=\"spinner\"></div>"
  }
  --------------- Expected Markup ---------------
  <div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner">
  <button class="flexi-picture__exit-fullscreen-button" aria-label="Exit fullscreen">
  <svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  </button>
  <div class="flexi-picture__container">
  <div class="flexi-picture__error-notice">
  <svg class="flexi-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-picture__spinner spinner"></div>
  <div class="flexi-picture__picture-container">
  <picture class="flexi-picture__picture">
  <img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
  </picture>
  </div>
  </div>
  </div>
  ````````````````````````````````
  No spinner is rendered if this value is `null`, whitespace or an empty string:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  p{ 
    "src": "/file.png",
    "alt": "Alternative text",
    "spinner": null
  }
  --------------- Expected Markup ---------------
  <div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_no-spinner">
  <button class="flexi-picture__exit-fullscreen-button" aria-label="Exit fullscreen">
  <svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  </button>
  <div class="flexi-picture__container">
  <div class="flexi-picture__error-notice">
  <svg class="flexi-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-picture__picture-container">
  <picture class="flexi-picture__picture">
  <img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
  </picture>
  </div>
  </div>
  </div>
  ````````````````````````````````

##### `EnableFileOperations`
- Type: `bool`
- Description: The value specifying whether file operations are enabled for the `FlexiPictureBlock`.
  If this value is `true` and
  `IFlexiPictureBlocksExtensionOptions.LocalMediaDirectory` is not `null`, whitespace or an empty string and
  either `Width` or `Height` is less than or equal to 0,
  `IFlexiPictureBlocksExtensionOptions.LocalMediaDirectory` is searched recursively for a file with `Src`'s file name,
  and the necessary file operations are performed on the file.
- Default: true

##### `Attributes`
- Type: `IDictionary<string, string>`
- Description: The HTML attributes for the `FlexiPictureBlock`'s root element.
  Attribute names must be lowercase.
  If classes are specified, they are appended to default classes. This facilitates [BEM mixes](https://en.bem.info/methodology/quick-start/#mix).
  If this value is `null`, default classes are still assigned to the root element.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Markdown ---------------
  p{ 
    "src": "/file.png",
    "alt": "Alternative text",
    "attributes": {
        "id" : "my-custom-id",
        "class" : "my-custom-class"
    }
  }
  --------------- Expected Markup ---------------
  <div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner my-custom-class" id="my-custom-id">
  <button class="flexi-picture__exit-fullscreen-button" aria-label="Exit fullscreen">
  <svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  </button>
  <div class="flexi-picture__container">
  <div class="flexi-picture__error-notice">
  <svg class="flexi-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="flexi-picture__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  <div class="flexi-picture__picture-container">
  <picture class="flexi-picture__picture">
  <img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
  </picture>
  </div>
  </div>
  </div>
  ````````````````````````````````

### `FlexiPictureBlocksExtensionOptions`
Options for the FlexiPictureBlocks extension. There are two ways to specify these options:
- Pass a `FlexiPictureBlocksExtensionOptions` when calling `MarkdownPipelineBuilderExtensions.UseFlexiPictureBlocks(this MarkdownPipelineBuilder pipelineBuilder, IFlexiPictureBlocksExtensionOptions options)`.
- Insert a `FlexiPictureBlocksExtensionOptions` into a `MarkdownParserContext.Properties` with key `typeof(IFlexiPictureBlocksExtensionOptions)`. Pass the `MarkdownParserContext` when you call a markdown processing method
  like `Markdown.ToHtml(markdown, stringWriter, markdownPipeline, yourMarkdownParserContext)`.  
  This method allows for different extension options when reusing a pipeline. Options specified using this method take precedence.

#### Constructor Parameters

##### `defaultBlockOptions`
- Type: `IFlexiPictureBlockOptions`
- Description: Default `IFlexiPictureBlockOptions` for all `FlexiPictureBlock`s.
  If this value is `null`, a `FlexiPictureBlockOptions` with default values is used.
- Default: `null`
- Examples:
  ```````````````````````````````` none
  --------------- Extension Options ---------------
  {
      "flexiPictureBlocks": {
          "defaultBlockOptions": {
              "errorIcon": "<svg><use xlink:href=\"#error-icon\"/></svg>",
              "attributes": {
                  "class": "block"
              }
          }
      }
  }
  --------------- Markdown ---------------
  p{ 
    "src": "/file.png",
    "alt": "Alternative text"
  }
  --------------- Expected Markup ---------------
  <div class="flexi-picture flexi-picture_has-alt flexi-picture_is-lazy flexi-picture_no-width flexi-picture_no-aspect-ratio flexi-picture_has-exit-fullscreen-icon flexi-picture_has-error-icon flexi-picture_has-spinner block">
  <button class="flexi-picture__exit-fullscreen-button" aria-label="Exit fullscreen">
  <svg class="flexi-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  </button>
  <div class="flexi-picture__container">
  <div class="flexi-picture__error-notice">
  <svg class="flexi-picture__error-icon"><use xlink:href="#error-icon"/></svg>
  </div>
  <div class="flexi-picture__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  <div class="flexi-picture__picture-container">
  <picture class="flexi-picture__picture">
  <img class="flexi-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
  </picture>
  </div>
  </div>
  </div>
  ````````````````````````````````
  `defaultBlockOptions` has lower precedence than block specific options:
  ```````````````````````````````` none
  --------------- Extension Options ---------------
  {
      "flexiPictureBlocks": {
          "defaultBlockOptions": {
              "blockName": "picture"
          }
      }
  }
  --------------- Markdown ---------------
  p{ 
    "src": "/file.png",
    "alt": "Alternative text"
  }

  p{ 
    "blockname": "special-picture",
    "src": "/file.png",
    "alt": "Alternative text"
  }
  --------------- Expected Markup ---------------
  <div class="picture picture_has-alt picture_is-lazy picture_no-width picture_no-aspect-ratio picture_has-exit-fullscreen-icon picture_has-error-icon picture_has-spinner">
  <button class="picture__exit-fullscreen-button" aria-label="Exit fullscreen">
  <svg class="picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  </button>
  <div class="picture__container">
  <div class="picture__error-notice">
  <svg class="picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="picture__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  <div class="picture__picture-container">
  <picture class="picture__picture">
  <img class="picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
  </picture>
  </div>
  </div>
  </div>
  <div class="special-picture special-picture_has-alt special-picture_is-lazy special-picture_no-width special-picture_no-aspect-ratio special-picture_has-exit-fullscreen-icon special-picture_has-error-icon special-picture_has-spinner">
  <button class="special-picture__exit-fullscreen-button" aria-label="Exit fullscreen">
  <svg class="special-picture__exit-fullscreen-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/><path d="M0 0h24v24H0z" fill="none"/></svg>
  </button>
  <div class="special-picture__container">
  <div class="special-picture__error-notice">
  <svg class="special-picture__error-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
  </div>
  <div class="special-picture__spinner spinner">
      <div class="spinner__rects">
          <div class="spinner__rect-1"></div>
          <div class="spinner__rect-2"></div>
          <div class="spinner__rect-3"></div>
      </div>
  </div>
  <div class="special-picture__picture-container">
  <picture class="special-picture__picture">
  <img class="special-picture__image" data-src="/file.png" alt="Alternative text" tabindex="-1">
  </picture>
  </div>
  </div>
  </div>
  ````````````````````````````````

##### `localMediaDirectory`
- Type: `string`
- Description: The local directory to search for image files in.
  If this value is `null`, whitespace or an empty string, file operations are disabled for all `FlexiPictureBlock`s.
  This value must be an absolute URI with the file scheme (points to a local directory).
- Default: `null`

## Incomplete Features

Refer to [Whatwg HTML Spec - Images](https://html.spec.whatwg.org/multipage/images.html) for a good summary on images.
Misc interesting reads:
- https://ericportis.com/posts/2014/srcset-sizes/

### Viewport-Based Resource Selection

This is where different resources are retrieved depending on viewport size. Benefits include 
lower data usage, quicker page loading and possibly, better aesthetics (if we use different image compositions for different 
viewport sizes). Given a base image, this extension could generate resources for different viewport sizes. Example markup:

```
<picture>
  <source media="(min-width: 1000px)" srcset="./example-image-400px.jpg">
  <source media="(min-width: 500px)" srcset="./example-image-200px.jpg">
  <img src="./example-image-100px.jpg" alt="Example image">
</picture>
```

The `srcset` attribute of `source` elements can be used to specify different resources for different device-pixel-ratios:

```
<picture>
  <source media="(min-width: 1000px)" srcset="./example-image-400px.jpg, ./example-image-800px.jpg 2x">
  <source media="(min-width: 500px)" srcset="./example-image-200px.jpg, ./example-image-400px.jpg 2x">
  <img srcset="./example-image-100px.jpg, ./example-image-200px.jpg 2x" alt="Example image">
</picture>
```

### Format-Based Resource Selection

This is where different resources are retrieved depending on user-agent support for formats. Benefits include 
lower data usage and quicker page loading. Given a base image, this extension could generate resources in 
different formats. Example markup:

```
<picture>
 <source srcset="./example-image.webp" type="image/webp">
 <source srcset="./example-image.jxr" type="image/vnd.ms-photo">
 <img src="./example-image.jpg" alt="" width="100" height="150">
</picture>
```

Format-based and viewport-based resource selection can be mixed: 

```
<picture>
  <source media="(min-width: 1000px)" srcset="./example-image-400px.webp, ./example-image-800px.webp 2x" type="image/webp">
  <source media="(min-width: 1000px)" srcset="./example-image-400px.jxr, ./example-image-800px.jxr 2x" type="image/vnd.ms-photo">
  <source media="(min-width: 500px)" srcset="./example-image-200px.webp, ./example-image-400px.webp 2x" type="image/webp">
  <source media="(min-width: 500px)" srcset="./example-image-200px.jxr, ./example-image-400px.jxr 2x" type="image/vnd.ms-photo">
  <img srcset="./example-image-100px.jpg, ./example-image-200px.jpg 2x" alt="Example image">
</picture>
```

Note how we'd have to generate lots of resources and maintain pretty verbose markup if we want to manually cover viewport size/device pixel ratio/format 
support differences. The FlexiPictureBlocks extension could simpifly things, for example, the above markup and associated resources
could be generated from:

p{
    "src": "./example-image.jpg",
    "alt": "Example image",
    "breakpoints": [ 
        { "(min-width: 1000px)", 400 },
        { "(min-width: 500px)", 200 },
        { "default", 100 }
    ],
    "formats": [ "webp", "jxr" ]
    "devicePixelRatios": [ 1, 2 ]
}

Formats and device pixel ratios could be defaults - not necessary to specify them for every FlexiPictureBlock.

## Avoiding page reflow on responsive image load.  

If the width and height attributes of an image element are specified, the browser uses them to calculate its aspect ratio.
*However*, if in CSS its width and height properties specified, the attribute values and the aspect ratio derived from them are
ignored. We have to set `max-width: 100%`, `height: auto` for image elements if we want them to be responsive, so without some hacking,
we get a page reflow on responsive image load.

At present, we're hacking around the problem using percentage based padding - https://www.voorhoede.nl/en/blog/say-no-to-image-reflow/.
The CSS working group is working on a far cleaner solution to this issue - https://github.com/WICG/intrinsicsize-attribute/issues/16#issuecomment-503245998.
When it has been adopted, we should render width and height attributes on image elements.