using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Settings;

public class ProfileSetting
{
    /// <summary>
    /// 아이디 검색 허용
    /// </summary>
    [Setting(true)]
    public bool? AllowIDSearch
    {
        get;
        set;
    }
}
