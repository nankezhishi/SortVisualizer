using System;
using System.Reactive;
using System.Reactive.Linq;

namespace VisualSort.Utils
{
    public static class ReactiveExtensions
    {
        public static IObservable<EventPattern<T>> ToObservable<T>(this EventHandler<T> @event)
            where T : EventArgs
        {
            return Observable.FromEventPattern<T>(f => @event += f, f => @event -= f);
        }
    }
}
