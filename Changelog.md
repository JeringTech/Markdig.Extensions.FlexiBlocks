# Changelog
This project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html). Refer to 
[The Semantic Versioning Lifecycle](https://www.jering.tech/articles/the-semantic-versioning-lifecycle)
for an overview of semantic versioning.

## [Unreleased](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/compare/0.14.0...HEAD)

## [0.14.0](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/compare/0.13.0...0.14.0) - Jan 2, 2019
### Additions
- Added `Region` property to the `Clipping` type. ([f85b9be](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/commit/f85b9be0d6f1aa46b663477262f146e4e4b3dc3a))
### Changes
- FlexiTableBlocks now wraps `<table>` elements in `<div>`s. ([9d03887](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/commit/9d038876df2b44feb142132dd841d2639008da53))
### Fixes
- Fixed FlexiSectionBlocks not processing inlines in heading blocks. ([73c013e](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/commit/73c013edd25d66cdc5d55a3425ed0ae35703c578))
- Fixed FlexiCodeBlocks empty lines not containing anything. ([e8ff3e8](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/commit/e8ff3e868ddf1b2dd3a276a550b2700818010a2f))
- Fixed FlexiSectionBlocks located immediately after lists being nested in preceding FlexiSectionBlocks regardless of level. ([03816db](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/commit/eab77757c2686525944357550c968539dc7b1946))
- Fixed line embellishing done by FlexiCodeBlocks for markup fragments with multi-line elements. ([ff1c644](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/commit/ff1c644784820df34ec06c8dbd5ab484cdfb16b4))

## [0.13.0](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/compare/0.12.0...0.13.0) - Dec 7, 2018
### Changes
- `FlexiBlocksExtension.Setup` overloads are no longer overridable. `FlexiBlocksExtension` implementers should implement
`FlexiBlocksExtension.SetupParsers` and `FlexiBlocksExtension.SetupRenderers` instead.
- Renamed `Context` enum to `FlexiBlockExceptionContext`.
- `SourceRetrieverService.GetSource` now logs warning instead of debug messages when retrieval attempts fail.
### Fixes
- Fixed `NullReferenceException` thrown by `FlexiTableBlockRenderer` when a table has no head row. 

## [0.12.0](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/compare/0.11.0...0.12.0) - Dec 3, 2018
### Changes
- `FlexiSectionBlockRenderer` is now a singleton service.
- Bumped bumped `Jering.Web.SyntaxHighlighters.HighlightJS` and `Jering.Web.SyntaxHighlighters.Prism`.
- Nuget package now includes source-linked symbols.
- Changed target frameworks from `netstandard2.0` and `netstandard1.3` to `netstandard2.0` and `net461`.
- Updated Nuget package metadata.
- Improved `FlexiBlocksMarkdownPipelineBuilderExtensions`
  - Removed its constructor and members `GetServiceProvider` and `SetDefaultServiceProvider`.
  - Added members `GetOrCreateServiceProvider`, `DisposeServiceProvider` and `Configure<TOptions>`.
- FlexiBlocksException constructor no longer throws an `ArgumentNullException`.
### Fixes
- Made `FlexiCodeBlockRenderer` thread safe.

## [0.11.0](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/compare/0.10.0...0.11.0) - Oct 18, 2018
### Additions
- Added methods `FlexiIncludeBlocksExtension.GetFlexiIncludeBlockTrees` and `FlexiIncludeBlocksExtension.GetIncludedSourcesAbsoluteUris`.
These methods report the depedencies of a processed markdown document.
### Changes
- Cleaned up architecture for extension options. 
- Minor changes to Nuget package title and description.

## [0.10.0](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/compare/0.9.0...0.10.0) - Oct 15, 2018
### Additions
- FlexiCodeBlocks now always renders at least two `<span>`s for each line of code. One with class `line` and
one with class `line-text`.
- FlexiCodeBlocks now renders an icon to represent hidden lines when line numbers aren't contiguous.
- FlexiCodeBlocks now renders copy icon within a `<button>` element.
- FlexSectionBlocks now renders link icon within a `<button>` element.
### Changes
- Renamed `FlexiCodeBlockOptions.LineNumberRanges` to `FlexiCodeBlockOptions.LineNumberLineRanges`. This
reflects under the hood changes to the type that the list contains.
- FlexiBlocksException no longer appends "Flexi" to block type names that do not begin with "Flexi".

## [0.9.0](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/compare/0.8.0...0.9.0) - Oct 12, 2018
### Fixes
- Fixed a FlexiSectionBlockParser bug that was causing it to consume the leading whitespace of every line.

## [0.8.0](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/compare/0.7.0...0.8.0) - Oct 11, 2018
### Additions
- FlexiTableBlocks now have a default class, "flexi-table-block", assigned to their outermost elements.
### Changes
- Replaced "fab" with "flexi-alert-block" in FlexiAlertBlock class names.
- Replaced "fcb" with "flexi-code-block" in FlexiCodeBlock class names.
- Replaced "section-level" with "flexi-section-block" in FlexiSectionBlock class names.

## [0.7.0](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/compare/0.6.0...0.7.0) - Oct 10, 2018
### Additions
- FlexiCodeBlocks now have a default class, "flexi-code-block", assigned to their outermost elements.

## [0.6.0](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/compare/0.5.0...0.6.0) - Oct 10, 2018
### Additions
- Exposed the `ServiceProvider` used by `FlexiBlocksMarkdownPipelineBuilderExtensions`.

## [0.5.0](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/compare/0.4.0...0.5.0) - Oct 4, 2018
### Changes
- Bumped `Jering.Web.SyntaxHighlighters.HighlightJS` and `Jering.Web.SyntaxHighlighters.HighlightJS`.
### Fixes
- Fixed Nuget package description formatting.

## [0.4.0](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/compare/0.3.0...0.4.0) - Sep 29, 2018
### Changes
- Improved Nuget package description, added a title for the package.
### Fixes
- Fixed inherited intellisense comments not appearing when using the netstandard1.3 assembly.
- Fixed some tests getting skipped.

## [0.3.0](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/compare/0.2.0...0.3.0) - Sep 29, 2018
### Changes
- Solution-wide cleanup.

## [0.2.0](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/compare/0.1.0...0.2.0) - Jul 25, 2018
### Changes
- Bumped syntax highlighter versions.

## [0.1.0](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/compare/0.1.0...0.1.0) - Jul 3, 2018
Initial release.