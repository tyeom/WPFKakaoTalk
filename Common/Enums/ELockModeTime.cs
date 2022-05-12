using Common.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Enums;

[TypeConverter(typeof(EnumDescriptionConverter<ELockModeTime>))]
public enum ELockModeTime
{
    [Description("1분 후")]
    m1,
    [Description("2분 후")]
    m2,
    [Description("3분 후")]
    m3,
    [Description("5분 후")]
    m5,
    [Description("10분 후")]
    m10
}
