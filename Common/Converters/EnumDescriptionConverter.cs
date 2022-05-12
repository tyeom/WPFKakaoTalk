using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Converters;

public class EnumDescriptionConverter<T> : EnumConverter
{
    public EnumDescriptionConverter() : base(typeof(T))
    {
    }

    //public override bool CanConvertTo(ITypeDescriptorContext? context, Type destinationType)
    //{
    //    return false;
    //}

    public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());
        if (fi == null)
            return null;

        DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (attributes.Length < 1)
        {
            return null;
        }
        else
        {
            string description = attributes[0].Description;
            return description;
        }
    }
}
