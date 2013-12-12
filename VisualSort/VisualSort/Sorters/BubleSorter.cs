using System;
using System.Collections.Generic;

namespace VisualSort.Sorters
{
    public class BubleSorter<T> : AbstractStepSorter<T>
        where T : IComparable<T>
    {
        public override string ToString()
        {
            return "Bubble Sort";
        }

        protected override IEnumerable<int> InitializeSorter()
        {
            for (int i = 0; i < _collection.Count; i++)
            {
                for (int j = _collection.Count - 1; j > i; j--)
                {
                    if (Compare(i, j) > 0)
                    {
                        Exchange(i, j);
                        yield return 1;
                    }
                }
            }
        }
    }
}
