using System;
using System.Collections;
using System.Collections.Generic;

namespace FlexiBlocks
{
    public class HtmlAttributeDictionary : IDictionary<string, string>
    {
        private readonly Dictionary<string, string> _map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Instantiates a <see cref="HtmlAttributeDictionary"/>.
        /// </summary>
        public HtmlAttributeDictionary()
        {
            _map = new Dictionary<string, string>();
        }

        /// <summary>
        /// Instantiates a <see cref="HtmlAttributeDictionary"/> with the contents in <paramref name="source"/>.
        /// </summary>
        /// <param name="source"></param>
        public HtmlAttributeDictionary(HtmlAttributeDictionary source)
        {
            _map = new Dictionary<string, string>(source._map);
        }

        public string this[string key]
        {
            get => _map[key];
            set => Add(key, value);
        }

        public ICollection<string> Keys => ((IDictionary<string, string>)_map).Keys;

        public ICollection<string> Values => ((IDictionary<string, string>)_map).Values;

        public int Count => _map.Count;

        public bool IsReadOnly => ((IDictionary<string, string>)_map).IsReadOnly;

        /// <summary>
        /// If <paramref name="key"/> is class and a <see cref="KeyValuePair{string, string}"/> already exists with key class, appends <paramref name="value"/> to the existing value.
        /// Otherwise, has the same behaviour as the <see cref="Dictionary{string, string}"/>'s indexer (adds <see cref="KeyValuePair{string, string}"/> if key does not exist,
        /// overwrites value if key exists).
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, string value)
        {
            if (string.Equals(key, "class", StringComparison.OrdinalIgnoreCase) &&
                _map.TryGetValue("class", out string existingClasses))
            {
                _map["class"] = (!string.IsNullOrWhiteSpace(existingClasses) ? existingClasses + " " : "") + value?.Trim();
            }
            else
            {
                _map[key] = value;
            }
        }

        public void Add(KeyValuePair<string, string> item)
        {
            ((IDictionary<string, string>)_map).Add(item);
        }

        public void Clear()
        {
            _map.Clear();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            return ((IDictionary<string, string>)_map).Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return _map.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            ((IDictionary<string, string>)_map).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return ((IDictionary<string, string>)_map).GetEnumerator();
        }

        public bool Remove(string key)
        {
            return _map.Remove(key);
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            return ((IDictionary<string, string>)_map).Remove(item);
        }

        public bool TryGetValue(string key, out string value)
        {
            return _map.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<string, string>)_map).GetEnumerator();
        }
    }
}
