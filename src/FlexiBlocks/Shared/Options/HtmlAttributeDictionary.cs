using System;
using System.Collections;
using System.Collections.Generic;

namespace Jering.Markdig.Extensions.FlexiBlocks
{
    /// <summary>
    /// Represents HTML attributes for an element.
    /// </summary>
    public class HtmlAttributeDictionary : IDictionary<string, string>
    {
        private readonly Dictionary<string, string> _map;

        /// <summary>
        /// Creates a <see cref="HtmlAttributeDictionary"/> instance.
        /// </summary>
        public HtmlAttributeDictionary()
        {
            _map = new Dictionary<string, string>();
        }

        /// <summary>
        /// Creates a <see cref="HtmlAttributeDictionary"/> instance containing the contents in of an existing dictionary.
        /// </summary>
        /// <param name="source">The dictionary to use for initial population.</param>
        public HtmlAttributeDictionary(IDictionary<string, string> source)
        {
            _map = source == null || source.Count == 0 ? new Dictionary<string, string>() : new Dictionary<string, string>(source);
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value associated with the specified key.</returns>
        public string this[string key]
        {
            get => _map[key];
            set => Add(key, value);
        }

        /// <summary>
        /// Gets an <see cref="ICollection{T}"/> containing the keys of the <see cref="HtmlAttributeDictionary"/>.
        /// </summary>
        public ICollection<string> Keys => _map.Keys;

        /// <summary>
        /// Gets an <see cref="ICollection{T}"/> containing the values of the <see cref="HtmlAttributeDictionary"/>.
        /// </summary>
        public ICollection<string> Values => _map.Values;

        /// <summary>
        /// Gets the number of key-value pairs contained in the <see cref="HtmlAttributeDictionary"/>.
        /// </summary>
        public int Count => _map.Count;

        /// <summary>
        /// Gets a value indicating whether the <see cref="HtmlAttributeDictionary"/> is read-only.
        /// </summary>
        public bool IsReadOnly => ((IDictionary<string, string>)_map).IsReadOnly;

        /// <summary>
        /// <para>Adds the specified key and value to the dictionary.</para>
        /// <para>If the key is "class" and a key-value pair already exists with key "class", appends the value to the existing value.</para>
        /// <para>Otherwise, if no key-value pair with the key exists, adds the key-value pair, and if a key-value pair with the key exists, replaces it.</para>
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add.</param>
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

        /// <summary>
        /// <para>Adds the specified key-value pair to the dictionary.</para>
        /// <para>If the key is "class" and a key-value pair already exists with key "class", appends the value to the existing value.</para>
        /// <para>Otherwise, if no key-value pair with the key exists, adds the key-value pair, and if a key-value pair with the key exists, replaces it.</para>
        /// </summary>
        /// <param name="item">The key-value pair to add.</param>
        public void Add(KeyValuePair<string, string> item)
        {
            ((IDictionary<string, string>)_map).Add(item);
        }

        /// <summary>
        /// Removes all key-value pairs.
        /// </summary>
        public void Clear()
        {
            _map.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="Dictionary{TKey,TValue}"/> contains the specified key-value pair.
        /// </summary>
        /// <param name="item">The item to locate in the <see cref="HtmlAttributeDictionary"/>.</param>
        /// <returns>true if the <see cref="HtmlAttributeDictionary"/> contains the specified key-value pair; otherwise, false.</returns>
        public bool Contains(KeyValuePair<string, string> item)
        {
            return ((IDictionary<string, string>)_map).Contains(item);
        }

        /// <summary>
        /// Determines whether the <see cref="Dictionary{TKey,TValue}"/> contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="HtmlAttributeDictionary"/>.</param>
        /// <returns>true if the <see cref="HtmlAttributeDictionary"/> contains an element with the specified key; otherwise, false.</returns>
        public bool ContainsKey(string key)
        {
            return _map.ContainsKey(key);
        }

        /// <summary>
        /// Copies the elements of the <see cref="ICollection{T}"/> to an array, starting at the specified array index.
        /// </summary>
        /// <param name="array">The one-dimensional array that is the destination of the elements copied from <see cref="ICollection{T}"/>. The array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            ((IDictionary<string, string>)_map).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _map.GetEnumerator();
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="HtmlAttributeDictionary"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>true if the element was successfully removed; otherwise, false.</returns>
        public bool Remove(string key)
        {
            return _map.Remove(key);
        }

        /// <summary>
        /// Removes the specified key-value pair from the <see cref="HtmlAttributeDictionary"/>.
        /// </summary>
        /// <param name="item">The key-value pair to remove.</param>
        /// <returns>true if the key-value pair was successfully removed; otherwise, false.</returns>
        public bool Remove(KeyValuePair<string, string> item)
        {
            return ((IDictionary<string, string>)_map).Remove(item);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns>true if the object that implements <see cref="HtmlAttributeDictionary"/> contains an element with the specified key; otherwise, false.</returns>
        public bool TryGetValue(string key, out string value)
        {
            return _map.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _map.GetEnumerator();
        }
    }
}
