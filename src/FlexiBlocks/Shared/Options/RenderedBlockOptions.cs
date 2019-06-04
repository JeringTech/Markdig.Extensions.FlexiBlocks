using Markdig.Syntax;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Jering.Markdig.Extensions.FlexiBlocks
{
    /// <summary>
    /// <para>An abstraction representing options for a rendered <see cref="IBlock"/>.</para>
    /// 
    /// <para>Implements <see cref="IRenderedBlockOptions{T}"/>.</para>
    /// </summary>
    public abstract class RenderedBlockOptions<T> : BlockOptions<T>, IRenderedBlockOptions<T> where T : class, IRenderedBlockOptions<T>
    {
        /// <summary>
        /// Creates a <see cref="RenderedBlockOptions{T}"/>.
        /// </summary>
        /// <param name="blockName">
        /// <para>The rendered <see cref="IBlock"/>'s <a href="https://en.bem.info/methodology/naming-convention/#block-name">BEM block name</a>.</para>
        /// <para>In compliance with the <a href="https://en.bem.info">BEM methodology</a>, this value is the root element's class as well as the prefix for all other classes in the block.</para>
        /// </param>
        /// <param name="attributes">
        /// <para>The HTML attributes for the rendered <see cref="IBlock"/>'s root element.</para>
        /// <para>If the class attribute is specified, its value is appended to default classes. This facilitates <a href="https://en.bem.info/methodology/quick-start/#mix">BEM mixes</a>.</para>
        /// <para>If this value is <c>null</c>, only default classes are assigned to the the root element.</para>
        /// <para>Defaults to <c>null</c>.</para>
        /// </param>
        protected RenderedBlockOptions(string blockName, IDictionary<string, string> attributes)
        {
            BlockName = blockName;
            Attributes = attributes == null ? null :
                attributes is ReadOnlyDictionary<string, string> attributesAsReadOnlyDictionary ? attributesAsReadOnlyDictionary :
                new ReadOnlyDictionary<string, string>(attributes);
        }

        /// <inheritdoc />
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public virtual ReadOnlyDictionary<string, string> Attributes { get; private set; }

        /// <inheritdoc />
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public virtual string BlockName { get; private set; }
    }
}
