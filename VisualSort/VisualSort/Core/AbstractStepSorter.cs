using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;
using VisualSort.Core;
using VisualSort.Utils;

namespace VisualSort.Sorters
{
    public abstract class AbstractStepSorter<T> : NotifyObject, IByStepSorter<T>, IObservableEvents
        where T : IComparable<T>
    {
        protected IList<T> _collection;
        private TimeSpan _stepDuration;
        private IEnumerator<int> _sorterEnumerator;
        private IDisposable _subscription;

        public TimeSpan StepDuration
        {
            get { return _stepDuration; }
            set
            {
                SetProperty(ref _stepDuration, value, () => StepDuration);

                // Reinitialize the stepper with new duration.
                if (_subscription != null)
                {
                    CreateExecutor();
                }
            }
        }

        public event EventHandler<SortingEventArgs> Comparing;

        public event EventHandler<SortingEventArgs> Exchanging;

        public event EventHandler Finished;

        public void Pause()
        {

        }

        public void Step()
        {

        }

        public void Continue()
        {

        }

        public void SortAsync(IList<T> collection)
        {
            _collection = collection;
            _sorterEnumerator = InitializeSorter().GetEnumerator();
            CreateExecutor();
        }

        protected abstract IEnumerable<int> InitializeSorter();

        protected void Exchange(int i, int j)
        {
            RaiseSortingEvent(Exchanging, i, j);
            T temp = _collection[i];
            _collection[i] = _collection[j];
            _collection[j] = temp;
        }

        protected int Compare(int i, int j)
        {
            RaiseSortingEvent(Comparing, i, j);

            return _collection[i].CompareTo(_collection[j]);
        }

        private void RaiseSortFinishedEvent()
        {
            if (Finished != null)
            {
                Finished(this, EventArgs.Empty);
            }
        }

        private void RaiseSortingEvent(EventHandler<SortingEventArgs> handler, int x, int y)
        {
            if (handler != null)
            {
                handler(this, new SortingEventArgs(x, y));
            }
        }

        private IDisposable CreateExecutor()
        {
            if (_subscription != null)
            {
                _subscription.Dispose();
            }
            var end = Observable.FromEventPattern(a => Finished += a, a => Finished -= a);

            return _subscription = Observable.Interval(StepDuration)
                .TakeUntil(end).ObserveOnDispatcher().Subscribe(MoveToNextStep);
        }

        private void MoveToNextStep(long step)
        {
            if (!_sorterEnumerator.MoveNext())
            {
                RaiseSortFinishedEvent();
            }
        }

        public IObservable<EventPattern<T>> ObserveEvent<T>(Func<EventHandler<T>> @event) where T : EventArgs
        {
            var handler = @event();

            return handler.ToObservable();
        }
    }
}
