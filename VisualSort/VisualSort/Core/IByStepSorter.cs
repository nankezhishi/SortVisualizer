using System;
using System.Collections.Generic;

namespace VisualSort
{
    public class SortingEventArgs : EventArgs
    {
        public SortingEventArgs(int index1, int index2)
        {
            Index1 = index1;
            Index2 = index2;
        }

        public int Index1 { get; private set; }

        public int Index2 { get; private set; }
    }

    public interface IByStepSorter<T>
        where T : IComparable<T>
    {
        TimeSpan StepDuration { get; set; }

        void Pause();

        void Step();

        void Continue();

        void SortAsync(IList<T> collection);

        event EventHandler<SortingEventArgs> Comparing;

        event EventHandler<SortingEventArgs> Exchanging;

        event EventHandler Finished;
    }
}
