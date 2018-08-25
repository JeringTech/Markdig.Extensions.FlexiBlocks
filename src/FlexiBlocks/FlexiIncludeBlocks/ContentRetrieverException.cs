using System;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    public class ContentRetrieverException : Exception
    {
        public ContentRetrieverException() : base()
        {
        }

        public ContentRetrieverException(string message) : base(message)
        {
        }

        public ContentRetrieverException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
