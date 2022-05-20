using System;
using System.Windows;
using System.Windows.Data;

namespace Common.Converters;

public class DoubleConverter : IValueConverter
{
    public object Convert(
        object value,
        Type targetType,
        object parameter,
        System.Globalization.CultureInfo culture)
    {
        double dValue = 0;
        double referenceVal = double.Parse(parameter.ToString()!);
        if (value is null || double.TryParse(value.ToString(), out dValue) is false)
        {
            return 0d;
        }
        else
        {
            return dValue / referenceVal;
        }
    }

    public object ConvertBack(
        object value,
        Type targetType,
        object parameter,
        System.Globalization.CultureInfo culture)
    {
        int val = 0;
        double referenceVal = double.Parse(parameter.ToString()!);
        if (value is null || int.TryParse(value.ToString(), out val) is false)
        {
            return 0;
        }
        else
        {
            return val * referenceVal;
        }
    }
}
