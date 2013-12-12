using System;
using System.Collections.Generic;

namespace VisualSort.Sorters
{
    public class HeapSort<T> : AbstractStepSorter<T>
        where T : IComparable<T>
    {
        public override string ToString()
        {
            return "Heap Sort";
        }

        protected override IEnumerable<int> InitializeSorter()
        {
            for (var i = (_collection.Count - 1) / 2; i >= 0; i--)
            {
                foreach (var r in MaxDownHeapify(_collection.Count, i))
                {
                    yield return r;
                }
            }

            for (int i = 0; i < _collection.Count - 1; i++)
            {
                Exchange(0, _collection.Count - 1 - i);
                yield return 1;
                foreach(var j in MaxDownHeapify(_collection.Count - 1 - i, 0))
                {
                    yield return j;
                }
            }
        }

        private IEnumerable<int> MaxDownHeapify(int length, int index)
        {
            var smallest = index;
            var leftIndex = index * 2 + 1;
            var rightIndex = leftIndex + 1;
            if (leftIndex < length && Compare(leftIndex, index) > 0)
            {
                smallest = leftIndex;
            }

            if (rightIndex < length && Compare(rightIndex, smallest) > 0)
            {
                smallest = rightIndex;
            }

            if (index != smallest)
            {
                Exchange(index, smallest);
                yield return 1;
                foreach(var i in MaxDownHeapify(length, smallest))
                {
                    yield return 1;
                }
            }
        }
    }
}
