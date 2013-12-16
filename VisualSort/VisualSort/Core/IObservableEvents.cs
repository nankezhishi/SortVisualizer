using System;
using System.Linq.Expressions;
using System.Reactive;

namespace VisualSort.Core
{
    public interface IObservableEvents
    {
        IObservable<EventPattern<T>> ObserveEvent<T>(Func<EventHandler<T>> @event) where T : EventArgs;
    }
}
