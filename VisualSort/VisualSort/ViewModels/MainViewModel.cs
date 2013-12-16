using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VisualSort.Sorters;

namespace VisualSort.ViewModels
{
    public class MainViewModel : NotifyObject
    {
        private int _itemCount = 25;
        private double _stepDuration = 10.0;
        private Random _random = new Random();
        private IList<DataItem> _itemsSource;
        private int _comparedCount;
        private int _exchangedCount;

        public IEnumerable<AbstractStepSorter<DataItem>> Sorters { get; set; }

        public IByStepSorter<DataItem> SelectedSorter { get; set; }

        public virtual int ComparedCount
        {
            get { return _comparedCount; }
            set { SetProperty(ref _comparedCount, value, () => ComparedCount); }
        }

        public virtual int ExchangedCount
        {
            get { return _exchangedCount; }
            set { SetProperty(ref _exchangedCount, value, () => ExchangedCount); }
        }

        public IList<DataItem> ItemsSource
        {
            get { return _itemsSource; }
        }

        public double StepDuration
        {
            get { return _stepDuration; }
            set
            {
                _stepDuration = value;
                if (SelectedSorter != null)
                {
                    SelectedSorter.StepDuration = TimeSpan.FromMilliseconds(value);
                }
            }
        }

        public MainViewModel()
        {
            _itemsSource = new ObservableCollection<DataItem>(
                Enumerable.Range(1, _itemCount)
                .Select(i => new DataItem(_random.Next(0x00000000, 0x000000FF))));

            Sorters = (from type in AllClasses.FromLoadedAssemblies()
                       where type.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IByStepSorter<>))
                       select Activator.CreateInstance(type.MakeGenericType(typeof(DataItem)))
                       as AbstractStepSorter<DataItem>).ToList();

            foreach (var sorter in Sorters)
            {
                sorter.Finished += (sender, e) => { };
                sorter.Comparing += (sender, e) => ComparedCount++;
                sorter.Exchanging += (sender, e) => ExchangedCount++;
            }
        }

        public void DoSort()
        {
            ComparedCount = 0;
            ExchangedCount = 0;
            SelectedSorter.StepDuration = TimeSpan.FromMilliseconds(StepDuration);
            SelectedSorter.SortAsync(_itemsSource);
        }

        public void Disorder()
        {
            var newValues = Enumerable.Range(1, _itemCount)
                .Select(i => _random.Next(0x00000000, 0x000000FF)).ToList();
            for (int i = 0; i < _itemCount; i++)
            {
                ItemsSource[i].Value = newValues[i];
            }
        }
    }
}
