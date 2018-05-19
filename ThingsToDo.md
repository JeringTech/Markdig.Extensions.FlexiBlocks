# Document CommonMark Specifications NBSP problem
Several specs are supposed to contain non-breaking spaces. Unforunately, the 
document on [Github](https://github.com/commonmark/CommonMark/blob/master/spec.txt#L10) contains ordinary spaces (U+0020) instead. 
This causes several tests to fail (319, 478 and 334).

# Sections Extension
Sections extension should start a "new context" in blockquotes and other sectioning roots. This is in line with the spec, which states that outlines in sectioning roots should not affect outlines in their parents.