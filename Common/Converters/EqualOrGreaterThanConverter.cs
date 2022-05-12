using System;
using System.Globalization;
using System.Windows.Data;

namespace Common.Converters;

public class EqualOrGreaterThanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null) return false;

        int val = 0;
        int compareToValue = 0;

        if (int.TryParse(value.ToString(), out val) &&
            int.TryParse(parameter.ToString(), out compareToValue))
        {
            return val >= compareToValue;
        }
        // Enum타입일 경우
        else if (value is Enum &&
            int.TryParse(parameter.ToString(), out compareToValue))
        {
            val = (int)value;
            return val >= compareToValue;
        }

        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}