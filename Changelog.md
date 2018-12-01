# Changelog
This project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html). Refer to 
[The Semantic Versioning Lifecycle](https://www.jeremytcd.com/articles/the-semantic-versioning-lifecycle)
for an overview of semantic versioning.

## [Unreleased](https://github.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/compare/0.11.1...HEAD)

## [0.11.1](https://github.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/compare/0.11.0...0.11.1) - Dec 1, 2018
### Changes
- Nuget package now includes source-linked symbols.
- Changed target frameworks from `netstandard2.0` and `netstandard1.3` to `netstandard2.0` and `net461`.
- Updated Nuget package metadata.

## [0.11.0](https://github.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/compare/0.10.0...0.11.0) - Oct 18, 2018
### Additions
- Added methods `FlexiIncludeBlocksExtension.GetFlexiIncludeBlockTrees` and `FlexiIncludeBlocksExtension.GetIncludedSourcesAbsoluteUris`.
These methods report the depedencies of a processed markdown document.

### Changes
- Cleaned up architecture for extension options. 
- Minor changes to Nuget package title and description.

## [0.10.0](https://github.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/compare/0.9.0...0.10.0) - Oct 15, 2018
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

## [0.9.0](https://github.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/compare/0.8.0...0.9.0) - Oct 12, 2018
### Fixes
- Fixed a FlexiSectionBlockParser bug that was causing it to consume the leading whitespace of every line.

## [0.8.0](https://github.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/compare/0.7.0...0.8.0) - Oct 11, 2018
### Additions
- FlexiTableBlocks now have a default class, "flexi-table-block", assigned to their outermost elements.
### Changes
- Replaced "fab" with "flexi-alert-block" in FlexiAlertBlock class names.
- Replaced "fcb" with "flexi-code-block" in FlexiCodeBlock class names.
- Replaced "section-level" with "flexi-section-block" in FlexiSectionBlock class names.

## [0.7.0](https://github.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/compare/0.6.0...0.7.0) - Oct 10, 2018
### Additions
- FlexiCodeBlocks now have a default class, "flexi-code-block", assigned to their outermost elements.

## [0.6.0](https://github.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/compare/0.5.0...0.6.0) - Oct 10, 2018
### Additions
- Exposed the `ServiceProvider` used by `FlexiBlocksMarkdownPipelineBuilderExtensions`.

## [0.5.0](https://github.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/compare/0.4.0...0.5.0) - Oct 4, 2018
### Fixes
- Fixed Nuget package description formatting.
### Changes
- Bumped `Jering.Web.SyntaxHighlighters.HighlightJS` and `Jering.Web.SyntaxHighlighters.HighlightJS`.


## [0.4.0](https://github.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/compare/0.3.0...0.4.0) - Sep 29, 2018
### Fixes
- Fixed inherited intellisense comments not appearing when using the netstandard1.3 assembly.
- Fixed some tests getting skipped.
### Changes
- Improved Nuget package description, added a title for the package.

## [0.3.0](https://github.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/compare/0.2.0...0.3.0) - Sep 29, 2018
### Changes
- Solution-wide cleanup.

## [0.2.0](https://github.com/JeremyTCD/Markdig.Extensions.FlexiBlocks/compare/0.1.0...0.2.0) - Jul 25, 2018
### Changes
- Bumped syntax highlighter versions.

## 0.1.0 - Jul 3, 2018
Initial release.