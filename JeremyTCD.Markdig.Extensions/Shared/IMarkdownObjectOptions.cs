namespace JeremyTCD.Markdig.Extensions
{
    public interface IMarkdownObjectOptions<T>
    {
        HtmlAttributeDictionary Attributes { get; set; }

        T Clone();
    }
}
