using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Jering.Markdig.Extensions.FlexiBlocks
{
    /// <summary>
    /// An implementation of <see cref="IOptions{TOptions}"/> that allows manual setting of the options instance.
    /// </summary>
    /// <typeparam name="TOptions">The type of this <see cref="IOptions{TOptions}"/>'s options instance.</typeparam>
    public class ExposedOptionsManager<TOptions> : IOptions<TOptions> where TOptions : class, new()
    {
        private TOptions _settableOptions;
        private readonly Lazy<TOptions> _optionsAccessor;
        private readonly IEnumerable<IConfigureOptions<TOptions>> _setups;

        /// <summary>
        /// Creates an <see cref="ExposedOptionsManager{TOptions}"/> instance.
        /// </summary>
        /// <param name="setups">The configuration actions to run.</param>
        public ExposedOptionsManager(IEnumerable<IConfigureOptions<TOptions>> setups)
        {
            _setups = setups;
            _optionsAccessor = new Lazy<TOptions>(CreateOptions);
        }

        private TOptions CreateOptions()
        {
            var result = new TOptions();
            if (_setups != null)
            {
                foreach (var setup in _setups)
                {
                    setup.Configure(result);
                }
            }
            return result;
        }

        /// <summary>
        /// Gets or sets the options instance.
        /// </summary>
        public virtual TOptions Value
        {
            get
            {
                return _settableOptions ?? _optionsAccessor.Value;
            }

            set
            {
                _settableOptions = value;
            }
        }
    }
}
