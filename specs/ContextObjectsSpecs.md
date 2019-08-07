# ContextObjects

This extension facilitates passing contextual data to parsers and renderers when processing markdown. Though it's used by other extensions in this project, you do not need to 
know how this extension works to use those extensions. It's designed specifically for extensions in this project, which are architected with inversion of control (IOC) in mind
and used through `MarkdownPipelineBuilder` extension methods. 

## What Does This Extension Accomplish?
Here's how Markdig is used:

```csharp
// Build a MarkdownPipeline
var markdownPipelineBuilder = new MarkdownPipelineBuilder();
var myExtension = new MyExtension(); // All extensions do is register parsers and renderers to the final pipeline
markdownPipelineBuilder.Extensions.Add(myExtension);
MarkdownPipeline markdownPipeline = markdownPipelineBuilder.Build();

// Use the MarkdownPipeline
string result = Markdown.ToHtml("dummyMarkdown", markdownPipeline);

// The MarkdownPipeline can be reused
string secondResult = Markdown.ToHtml("moreDummyMarkdown", markdownPipeline);
```

What if you want to pass contextual data to your extension? For example, you might want HTML written by `MyExtension`'s renderer to have certain classes
for specific markdown. You could pass a special class directly to the extension:

```csharp
// Build a MarkdownPipeline
var markdownPipelineBuilder = new MarkdownPipelineBuilder();
string mySpecialClass = "my-special-class";
var myExtension = new MyExtension(mySpecialClass); // MyExtension passes mySpecialClasses to the renderer it registers
markdownPipelineBuilder.Extensions.Add(myExtension);
MarkdownPipeline markdownPipeline = markdownPipelineBuilder.Build();

// Use the MarkdownPipeline
string result = Markdown.ToHtml("dummyMarkdown", markdownPipeline);

// The MarkdownPipeline can be reused. No way to change the special class though.
string secondResult = Markdown.ToHtml("moreDummyMarkdown", markdownPipeline);
```

If you do it this way, you can't change the contextual data when you reuse the `MarkdownPipeline`. Also, you'll need a `MyExtensionFactory` for proper IOC.

You could instead use `MarkdownParserContext`:

```csharp
// Build a MarkdownPipeline
var markdownPipelineBuilder = new MarkdownPipelineBuilder();
var myExtension = new MyExtension();
markdownPipelineBuilder.Extensions.Add(myExtension);
MarkdownPipeline markdownPipeline = markdownPipelineBuilder.Build();

// Create a MarkdownParserContext
var markdownParserContext = new MarkdownParserContext();
markdownParserContext.Properties.Add("myKey", "my-special-class");

// Create a TextWriter
var stringWriter = new StringWriter();

// Use the MarkdownPipeline
Markdown.ToHtml("dummyMarkdown", stringWriter, markdownPipeline, markdownParserContext); // MarkdownParserContext is assigned to BlockProcessor.Context so parsers and renderers can access it
string result = stringWriter.ToString();

// Change the special class
markdownParserContext.Properties["myKey"] = "my-other-special-class";
dummyStringWriter.GetStringBuilder().Length = 0; // Reset text writer
Markdown.ToHtml("moreDummyMarkdown", stringWriter, markdownPipeline, markdownParserContext);
string secondResult = stringWriter.ToString();
```

There are two issues with this approach:
- Options for extensions aren't statically typed, instead they're just added to a `Dictionary<object, object>`.
- Some extensions use the same options throughout their lifetimes. They don't need options to be changeable for pipeline reuses. For such extensions, this approach convolutes things.

The ContextObjects helps extension solve these issues:

```csharp
// Build a MarkdownPipeline
var markdownPipelineBuilder = new MarkdownPipelineBuilder();

// The following 5 lines can be extracted to a MarkdownPipelineBuilder extension method
var contextObjectsExtension = new ContextObjectsExtension(new ContextObjectsStore());
contextObjectsExtension.ContextObjectsStore.Add("myKey", "my-special-class");
markdownPipelineBuilder.Extensions.Add(contextObjectsExtension); // Parsers and renderers can now use ContextObjectsService.TryGetContextObject(object, BlockProcessor, out object) to retrieve contextual data
var myExtension = new MyExtension(); // No need to pass runtime contextual data to MyExtension, this means we don't need to create a MyExtensionFactory for proper IOC
markdownPipelineBuilder.Extensions.Add(myExtension);

MarkdownPipeline markdownPipeline = markdownPipelineBuilder.Build();

// Use the MarkdownPipeline
Markdown.ToHtml("dummyMarkdown", markdownPipeline); // No need for MarkdownParserContext
string result = stringWriter.ToString();

// Create a TextWriter
var stringWriter = new StringWriter();

// Create a MarkdownParserContext
var markdownParserContext = new MarkdownParserContext();
markdownParserContext.Properties.Add("myKey", "my-other-special-class");

// Change the class
Markdown.ToHtml("moreDummyMarkdown", stringWriter, markdownPipeline, markdownParserContext); // MarkdownParserContext values take precedence over what was added to ContextObjectsExtension.ContextObjectsStore
string secondResult = stringWriter.ToString();
```

We can now create a dependency injection (DI) based `MarkdownPipelineBuilder` extension method with statically typed extension options, for example:

```csharp
public static MarkdownPipelineBuilder UseMyExtension(this MarkdownPipelineBuilder pipelineBuilder, IMyExtensionExtensionOptions options = null /* Statically typed options */)
{
    if (!pipelineBuilder.Extensions.Contains<IMyExtension>())
    {
        pipelineBuilder.Extensions.Add(GetOrCreateServiceProvider().GetRequiredService<IMyExtension>()); // No need for an extension factory
    }

    if(options != null)
    {
        ContextObjectsExtension contextObjectsExtension = pipelineBuilder.Extensions.Find<ContextObjectsExtension>();

        if (contextObjectsExtension == null)
        {
            contextObjectsExtension = GetOrCreateServiceProvider().GetRequiredService<ContextObjectsExtension>();
            pipelineBuilder.Extensions.Add(contextObjectsExtension);
        }

        contextObjectsExtension.ContextObjectsStore.Add("myKey", contextObject);
    }

    return pipelineBuilder;
}
```
