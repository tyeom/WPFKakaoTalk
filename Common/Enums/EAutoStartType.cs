using Common.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Enums;

[TypeConverter(typeof(EnumDescriptionConverter<EAutoStartType>))]
public enum EAutoStartType
{
    [Description("잠금모드")]
    LockType,
    [Description("잠금모드 해제")]
    UnLockType
}
