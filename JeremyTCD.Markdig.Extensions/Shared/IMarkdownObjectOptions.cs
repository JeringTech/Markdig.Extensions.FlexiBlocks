using System.Collections.Generic;

namespace JeremyTCD.Markdig.Extensions
{
    public interface IMarkdownObjectOptions<T>
    {
        Dictionary<string, string> Attributes { get; set; }
        T Clone();
    }
}
