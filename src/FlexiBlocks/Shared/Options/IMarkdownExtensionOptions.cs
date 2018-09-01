namespace Jering.Markdig.Extensions.FlexiBlocks
{
    public interface IMarkdownExtensionOptions<T>
    {
        T DefaultBlockOptions { get; set; }
    }
}
