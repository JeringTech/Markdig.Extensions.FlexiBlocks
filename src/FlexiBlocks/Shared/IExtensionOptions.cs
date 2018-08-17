namespace Jering.Markdig.Extensions.FlexiBlocks
{
    public interface IExtensionOptions<T>
    {
        void CopyTo(T target);
    }
}
