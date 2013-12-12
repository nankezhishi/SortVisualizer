using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Linq.Expressions;

namespace VisualSort
{
    public class NotifyObject : NotificationObject
    {
        protected void SetProperty<T>(ref T value, T newValue, Expression<Func<T>> propertyGetter)
        {
            if (!Equals(value, newValue))
            {
                value = newValue;
                RaisePropertyChanged(propertyGetter);
            }
        }
    }
}
