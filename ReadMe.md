# FlexiBlocks
[![Build Status](https://dev.azure.com/JeringTech/Markdig.Extensions.FlexiBlocks/_apis/build/status/Jering.Markdig.Extensions.FlexiBlocks-CI?branchName=master)](https://dev.azure.com/JeringTech/Markdig.Extensions.FlexiBlocks/_build/latest?definitionId=5?branchName=master)
[![codecov](https://codecov.io/gh/JeringTech/Markdig.Extensions.FlexiBlocks/branch/master/graph/badge.svg)](https://codecov.io/gh/JeringTech/Markdig.Extensions.FlexiBlocks)
[![License](https://img.shields.io/badge/license-Apache%202.0-blue.svg)](https://github.com/Pkcs11Interop/Pkcs11Interop/blob/master/LICENSE.md)
[![NuGet](https://img.shields.io/nuget/vpre/Jering.Markdig.Extensions.FlexiBlocks.svg?label=nuget)](https://www.nuget.org/packages/Jering.Markdig.Extensions.FlexiBlocks/)

## Overview
FlexiBlocks is a collection of [Markdig](https://github.com/lunet-io/markdig) extensions: 

- [FlexiAlertBlocks](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiAlertBlocksSpecs.md)
- [FlexiBannerBlocks](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiBannerBlocksSpecs.md)
- [FlexiCardsBlocks](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiCardsBlocksSpecs.md)
- [FlexiCodeBlocks](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiCodeBlocksSpecs.md)
- [FlexiFigureBlocks](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiFigureBlocksSpecs.md)
- [FlexiIncludeBlocks](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiIncludeBlocksSpecs.md)
- [FlexiOptionsBlocks](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiOptionsBlocksSpecs.md)
- [FlexiPictureBlocks](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiPictureBlocksSpecs.md)
- [FlexiQuoteBlocks](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiQuoteBlocksSpecs.md)
- [FlexiSectionBlocks](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiSectionBlocksSpecs.md)
- [FlexiTableBlocks](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiTableBlocksSpecs.md)
- [FlexiTabsBlocks](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiTabsBlocksSpecs.md)
- [FlexiVideoBlocks](https://github.com/JeringTech/Markdig.Extensions.FlexiBlocks/blob/master/specs/FlexiVideoBlocksSpecs.md)
- ContextObjects

A documentation site is being built for this project.

## Running Tests
- For FlexiCodeBlocks tests to pass, you'll need [Node.js](https://nodejs.org/en/) installed and on your PATH environment variable. FlexiCodeBlocks uses popular js libraries to perform syntax highlighting.
- For FlexiVideoBlocks tests to pass, you'll need [FFmpeg](https://www.ffmpeg.org/) installed and on your PATH environment variable. FlexiVideoBlocks uses FFmpeg to retrieve video metadata (width, height and duration)
as well as to generate [posters](https://developer.mozilla.org/en-US/docs/Web/HTML/Element/video#attr-poster).

## Frontend Assets
This library generates markup. A lot of functionality requires frontend scripts and styles. For now, the frontend assets
can be found in this project: [Mimo](https://github.com/JeremyTCD/Mimo), e.g the script for FlexiPictureBlocks can be found in `picture.ts`
and its styles can be found in `_flexiPictureBlock.scss`.  

We are working on packaging frontend assets for easy public consumption.

## About
Follow [@JeringTech](https://twitter.com/JeringTech) for updates.