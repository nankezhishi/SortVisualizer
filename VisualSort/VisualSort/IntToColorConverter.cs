using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace VisualSort
{
    [ValueConversion(typeof(int), typeof(Brush))]
    public class IntToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var hexValue = System.Convert.ToInt32(value).ToString("X2");
            hexValue = "#" + hexValue + hexValue + hexValue;

            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexValue));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
