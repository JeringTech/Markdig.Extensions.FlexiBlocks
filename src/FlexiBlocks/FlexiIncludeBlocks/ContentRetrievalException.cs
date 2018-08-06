using System;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    public class ContentRetrievalException : Exception
    {
        public ContentRetrievalException() : base()
        {
        }

        public ContentRetrievalException(string message) : base(message)
        {
        }

        public ContentRetrievalException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
