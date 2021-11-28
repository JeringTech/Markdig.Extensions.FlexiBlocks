# Notes

## Empty Elements
- Don't render purposeless empty elements. For example, `<span class="title"></span>` has no purpose if it is empty.
- Render purposeful empty elements. For example, `<button class="exit-button"></button>` is still a button even if it doesn't contain text or an icon.

## Feature and Type Classes
- Feature and type classes should correspond 1:1 to block properties, with the following exceptions:
  - No class required for properties used to build the block, since once the block is built, these properties are no longer useful. E.g `FlexiFigureBlock.LinkLabelContent`.
  - No class required for ID properties. For compatibility with BEM, we don't use IDs for selecting elements. We only use IDs for reference linking when building markdown documents. E.g `FlexiFigureBlock.ID`.
  - No class required for RenderingMode properties added only for CommonMark specs. E.g `FlexiCodeBlock.RenderingMode`.
  - No class required for pre-processed block content. Typically we write block content using `HtmlRenderer.WriteChildren` or `HtmlRenderer.WriteLeafInline`. If we happen to pre-process the content,
    there is no need for an additional class. E.g `FlexiCodeBlock.Code`.
