using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Jering.Markdig.Extensions.FlexiBlocks
{
    public class ExposedOptionsManager<TOptions> : IOptions<TOptions> where TOptions : class, new()
    {
        private TOptions _settableOptions;
        private readonly Lazy<TOptions> _optionsAccessor;
        private readonly IEnumerable<IConfigureOptions<TOptions>> _setups;

        /// <summary>
        /// Initializes a new instance with the specified options configurations.
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
        /// The configured options instance.
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
