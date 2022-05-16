using System;
using System.Windows;
using System.Windows.Data;

namespace Common.Converters;

public class InputStrLengthConverter : IValueConverter
{
    public object Convert(
        object value,
        Type targetType,
        object parameter,
        System.Globalization.CultureInfo culture)
    {
        Int16 strLen = 0;
        Int16 setMaxStrLen = Int16.Parse(parameter.ToString());

        if (value is null)
        {
            return $"0/{setMaxStrLen}";
        }

        strLen = (Int16)value.ToString()!.Length;

        return $"{strLen}/{setMaxStrLen}";
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
