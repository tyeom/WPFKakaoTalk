using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.MarkupExtension;

public class EnumBindingSourceExtension : System.Windows.Markup.MarkupExtension
{
    private Type _enumType;

    /// <summary>
    /// 기본 생성자
    /// </summary>
    public EnumBindingSourceExtension() { }

    /// <summary>
    /// enum 타입 파라메터 생성자
    /// </summary>
    /// <param name="enumType"></param>
    public EnumBindingSourceExtension(Type enumType)
    {
        this.EnumType = enumType;
    }

    public Type EnumType
    {
        get { return this._enumType; }
        private set
        {
            if (value != this._enumType)
            {
                if (null != value)
                {
                    // null 타입 체크
                    Type enumType = Nullable.GetUnderlyingType(value) ?? value;
                    if (!enumType.IsEnum)
                        throw new ArgumentException("Enum 타입이 아닙니다!");
                }

                this._enumType = value;
            }
        }
    }

    public string Fillter
    {
        get; set;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (null == this._enumType)
            throw new InvalidOperationException("Enum 타입이 아닙니다!");

        Type enumType = Nullable.GetUnderlyingType(this._enumType) ?? this._enumType;
        Array enumValues = Enum.GetValues(enumType);

        if (enumType == this._enumType)
        {
            if (string.IsNullOrWhiteSpace(Fillter))
            {
                return enumValues;
            }
            else
            {
                var fillterArr = Fillter.ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                return enumValues.Cast<Enum>().Where(p => fillterArr.Contains(p.ToString()));
            }
        }

        {
            Array enumValArr = Array.CreateInstance(enumType, enumValues.Length + 1);
            enumValues.CopyTo(enumValArr, 1);

            if (string.IsNullOrWhiteSpace(Fillter))
            {
                return enumValues;
            }
            else
            {
                var fillterArr = Fillter.ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                return enumValues.Cast<Enum>().Where(p => fillterArr.Contains(p.ToString()));
            }
        }
    }
}
