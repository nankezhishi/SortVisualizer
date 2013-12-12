using System;
using System.Collections.Generic;

namespace VisualSort.Sorters
{
    public class QuickSort<T> : AbstractStepSorter<T>
        where T : IComparable<T>
    {
        public override string ToString()
        {
            return "Quick Sort";
        }

        protected override IEnumerable<int> InitializeSorter()
        {
            foreach (var step in Sort(0, _collection.Count - 1))
            {
                yield return step;
            }
        }

        private IEnumerable<int> Sort(int startIndex, int endIndex)
        {
            if (startIndex < endIndex)
            {
                int devider = 0;
                foreach (var p in Partition(startIndex, endIndex))
                {
                    yield return p;
                    devider = p;
                }
                foreach (var step in Sort(startIndex, devider - 1))
                {
                    yield return step;
                }
                foreach (var step in Sort(devider + 1, endIndex))
                {
                    yield return step;
                }
            }
        }

        private IEnumerable<int> Partition(int start, int end)
        {
            int p = start - 1;
            for (int i = start; i < end; i++)
            {
                if (Compare(i, end) < 0)
                {
                    p++;
                    Exchange(i, p);
                    yield return p;
                }
            }
            p++;
            Exchange(end, p);
            yield return p;
        }
    }
}
