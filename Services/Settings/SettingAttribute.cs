using System;

namespace Services.Settings;

/// <summary>
/// 환경설정의 기본값 설정
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Property,
                        AllowMultiple = true,
                        Inherited = false)]
public class SettingAttribute : Attribute
{
    private object _defaultValue = null;

    public SettingAttribute(object defaultValue)
    {
        _defaultValue = defaultValue;
    }

    public object DefaultValue
    {
        get => _defaultValue;
    }
}