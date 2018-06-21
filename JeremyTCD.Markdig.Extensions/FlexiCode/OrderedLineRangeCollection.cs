using System.Collections;
using System.Collections.Generic;

namespace JeremyTCD.Markdig.Extensions.FlexiCode
{
    /// <summary>
    /// TODO verify that ranges do not overlap and that start lines are in increasing order
    /// </summary>
    public class OrderedLineRangeCollection : IList<LineRange>
    {
        private readonly List<LineRange> _lineRanges;

        public OrderedLineRangeCollection()
        {
            _lineRanges = new List<LineRange>();
        }

        public LineRange this[int index] { get => ((IList<LineRange>)_lineRanges)[index]; set => ((IList<LineRange>)_lineRanges)[index] = value; }

        public int Count => ((IList<LineRange>)_lineRanges).Count;

        public bool IsReadOnly => ((IList<LineRange>)_lineRanges).IsReadOnly;

        public void Add(LineRange item)
        {
            ((IList<LineRange>)_lineRanges).Add(item);
        }

        public void Clear()
        {
            ((IList<LineRange>)_lineRanges).Clear();
        }

        public bool Contains(LineRange item)
        {
            return ((IList<LineRange>)_lineRanges).Contains(item);
        }

        public void CopyTo(LineRange[] array, int arrayIndex)
        {
            ((IList<LineRange>)_lineRanges).CopyTo(array, arrayIndex);
        }

        public IEnumerator<LineRange> GetEnumerator()
        {
            return ((IList<LineRange>)_lineRanges).GetEnumerator();
        }

        public int IndexOf(LineRange item)
        {
            return ((IList<LineRange>)_lineRanges).IndexOf(item);
        }

        public void Insert(int index, LineRange item)
        {
            ((IList<LineRange>)_lineRanges).Insert(index, item);
        }

        public bool Remove(LineRange item)
        {
            return ((IList<LineRange>)_lineRanges).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<LineRange>)_lineRanges).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<LineRange>)_lineRanges).GetEnumerator();
        }
    }
}
