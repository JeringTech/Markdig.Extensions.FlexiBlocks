---
mimo_pageDescription: A description of and explanation for Jering.Markdig.Extensions.FlexiBlocks's approach to structurally redundant markup.
mimo_pageTitle: Structurally Redundant Markup
mimo_pageID: structurally-redundant-markup
mimo_date: Oct 19, 2018
mimo_shareOnFacebook: true
mimo_shareOnTwitter:
    hashtags: Markdown,Markdig,FlexiBlocks
    via: JeringTech
---

Consider `<header>` elements in code blocks. Typically, these elements contain a title and/or a copy code button. In cases where
these elements are empty, they are structurally redundant. However, users might still expect them to be displayed with certain styles.
Because of this expectation, such elements aren't removed just because they are structurally redundant.

In general, the approach is ease of use over concision.
- By leaving elements there, the only downside is extra markup elements that can be hidden using display none.
- If elements are removed, they will have to be re-inserted using js, which can lead to flashes and the need to write a bunch of js. Moreover, and critically,
  removing elements can lead to a bad user experience, since it might not be obvious why elements are removed.