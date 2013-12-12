using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Windows;

namespace VisualSort
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private int _itemCount = 25;
        private double _stepDuration = 10.0;
        private Random _random = new Random();
        private IList<DataItem> _itemsSource;

        public IEnumerable<IByStepSorter<DataItem>> Sorters { get; set; }

        public IByStepSorter<DataItem> SelectedSorter { get; set; }

        public virtual int ComparedCount { get; set; }

        public  virtual int ExchangedCount { get; set; }

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

        public MainWindow()
        {
            _itemsSource = new ObservableCollection<DataItem>(
                Enumerable.Range(1, _itemCount)
                .Select(i => new DataItem(_random.Next(0x00000000, 0x000000FF))));

            Sorters = (from type in AllClasses.FromLoadedAssemblies()
                       where type.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IByStepSorter<>))
                       select Activator.CreateInstance(type.MakeGenericType(typeof(DataItem)))
                       as IByStepSorter<DataItem>).ToList();

            foreach(var sorter in Sorters)
            {
                sorter.Finished += (sender, e) => { };
                Observable.FromEventPattern<SortingEventArgs>(f => sorter.Comparing += f, f => sorter.Comparing -= f)
                    .ObserveOnDispatcher().Subscribe(ep =>
                    {
                        ComparedCount++;
                        RaisePropertyChangedEvent(() => ComparedCount);
                    });

                Observable.FromEventPattern<SortingEventArgs>(f => sorter.Exchanging += f, f => sorter.Exchanging -= f)
                    .ObserveOnDispatcher().Subscribe(ep =>
                    {
                        ExchangedCount++;
                        RaisePropertyChangedEvent(() => ExchangedCount);
                    });
            }

            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs args)
        {
            base.OnInitialized(args);

            var mouseUp = Observable.FromEventPattern(this, "PreviewMouseUp");
            Observable.FromEventPattern(
                (EventHandler<double> h) =>
                    new RoutedPropertyChangedEventHandler<double>((sender, e) => h(sender, e.NewValue)),
                h => stepDurationSlide.ValueChanged += h,
                h => stepDurationSlide.ValueChanged -= h)
                .Buffer(mouseUp)
                .SubscribeOnDispatcher()
                .Subscribe(ep =>
                {
                    if (ep.Any())
                        StepDuration = ep.Last().EventArgs;
                });
        }

        private void OnSortClick(object sender, RoutedEventArgs e)
        {
            ComparedCount = 0;
            ExchangedCount = 0;
            SelectedSorter.StepDuration = TimeSpan.FromMilliseconds(StepDuration);
            SelectedSorter.SortAsync(_itemsSource);
        }

        private void OnDisorderClick(object sender, RoutedEventArgs e)
        {
            var newValues = Enumerable.Range(1, _itemCount)
                .Select(i => _random.Next(0x00000000, 0x000000FF)).ToList();
            for (int i = 0; i < _itemCount; i++)
            {
                ItemsSource[i].Value = newValues[i];
            }
        }

        private void OnRunButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void OnStepButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void RaisePropertyChangedEvent<T>(Expression<Func<T>> exp)
        {
            var tmp = PropertyChanged;
            if (tmp != null)
            {
                tmp(this, new PropertyChangedEventArgs((exp.Body as MemberExpression).Member.Name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
