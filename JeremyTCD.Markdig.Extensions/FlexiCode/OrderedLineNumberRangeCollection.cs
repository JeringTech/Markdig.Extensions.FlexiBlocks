using System.Collections;
using System.Collections.Generic;

namespace JeremyTCD.Markdig.Extensions.FlexiCode
{
    /// <summary>
    /// TODO verify that ranges do not overlap and that start lines are in increasing order
    /// TODO verify that line numbers do not overlap
    /// </summary>
    public class OrderedLineNumberRangeCollection : IList<LineNumberRange>
    {
        private readonly List<LineNumberRange> _lineNumberRanges;

        public OrderedLineNumberRangeCollection()
        {
            _lineNumberRanges = new List<LineNumberRange>();
        }

        public LineNumberRange this[int index] { get => ((IList<LineNumberRange>)_lineNumberRanges)[index]; set => ((IList<LineNumberRange>)_lineNumberRanges)[index] = value; }

        public int Count => ((IList<LineNumberRange>)_lineNumberRanges).Count;

        public bool IsReadOnly => ((IList<LineNumberRange>)_lineNumberRanges).IsReadOnly;

        public void Add(LineNumberRange item)
        {
            ((IList<LineNumberRange>)_lineNumberRanges).Add(item);
        }

        public void Clear()
        {
            ((IList<LineNumberRange>)_lineNumberRanges).Clear();
        }

        public bool Contains(LineNumberRange item)
        {
            return ((IList<LineNumberRange>)_lineNumberRanges).Contains(item);
        }

        public void CopyTo(LineNumberRange[] array, int arrayIndex)
        {
            ((IList<LineNumberRange>)_lineNumberRanges).CopyTo(array, arrayIndex);
        }

        public IEnumerator<LineNumberRange> GetEnumerator()
        {
            return ((IList<LineNumberRange>)_lineNumberRanges).GetEnumerator();
        }

        public int IndexOf(LineNumberRange item)
        {
            return ((IList<LineNumberRange>)_lineNumberRanges).IndexOf(item);
        }

        public void Insert(int index, LineNumberRange item)
        {
            ((IList<LineNumberRange>)_lineNumberRanges).Insert(index, item);
        }

        public bool Remove(LineNumberRange item)
        {
            return ((IList<LineNumberRange>)_lineNumberRanges).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<LineNumberRange>)_lineNumberRanges).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<LineNumberRange>)_lineNumberRanges).GetEnumerator();
        }
    }
}
