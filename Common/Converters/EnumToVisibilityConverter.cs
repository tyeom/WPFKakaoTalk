using System;
using System.Windows;
using System.Windows.Data;

namespace Common.Converters;

public class EnumToVisibilityConverter : IValueConverter
{
    public object Convert(
        object value,
        Type targetType,
        object parameter,
        System.Globalization.CultureInfo culture)
    {
        //string strEnum = value.ToString();

        //if (parameter != null && parameter.ToString().Equals(strEnum))
        //{
        //    return Visibility.Visible;
        //}
        //else
        //{
        //    return Visibility.Collapsed;
        //}
        if ((bool)value == true)
            return Visibility.Visible;
        else
            return Visibility.Collapsed;
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