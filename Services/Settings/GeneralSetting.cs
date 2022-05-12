using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Settings;

public class GeneralSetting
{
    /// <summary>
    /// 윈도우 시작 시 자동실행
    /// </summary>
    [Setting(false)]
    public bool? AutoStart
    {
        get;
        set;
    }

    /// <summary>
    /// 자동 로그인
    /// </summary>
    [Setting(false)]
    public bool? AutoLogin
    {
        get;
        set;
    }

    /// <summary>
    /// 자동 로그인시 시작 모드
    /// </summary>
    [Setting(EAutoStartType.LockType)]
    public EAutoStartType AutoStartType
    {
        get;
        set;
    }

    /// <summary>
    /// PC 미사용 시 잠금모드 적용
    /// </summary>
    [Setting(false)]
    public bool? UseLockMode
    {
        get;
        set;
    }

    /// <summary>
    /// 잠금모드 적용 시간
    /// </summary>
    [Setting(ELockModeTime.m1)]
    public ELockModeTime LockModeTime
    {
        get;
        set;
    }
}
