using System;
using System.Windows;
using System.Windows.Data;

namespace Common.Converters;

public class StrLengthConverter : IValueConverter
{
    public object Convert(
        object value,
        Type targetType,
        object parameter,
        System.Globalization.CultureInfo culture)
    {
        if (value is null)
            return 0;

        return value.ToString().Length;
    }

    public object ConvertBack(
        object value,
        Type targetType,
        object parameter,
        System.Globalization.CultureInfo culture)
    {
        return null;
    }
}
