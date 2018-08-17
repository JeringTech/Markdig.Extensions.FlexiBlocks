using System.Collections.Generic;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    public class IncludeOptions
    {
        public ContentType ContentType { get; set; }
        public List<ClippingArea> ClippingAreas { get; set; }
        public string Source { get; set; }
        public bool CacheRemoteSource { get; set; } = true;
    }
}
