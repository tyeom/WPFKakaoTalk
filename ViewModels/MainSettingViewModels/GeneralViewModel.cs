using Common.Base;
using Common.Enums;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.MainSettingViewModels;

public class GeneralViewModel : ViewModelBase
{
    private readonly ISettingService _settingService;
    private EAutoStartType _autoStartType = EAutoStartType.LockType;
    private ELockModeTime _lockModeTime = ELockModeTime.m1;
    private bool _autoStart = false;
    private bool _autoLogin = false;
    private bool _useLock = false;

    public GeneralViewModel(ISettingService settingService)
    {
        _settingService = settingService;

        AutoStartType = _settingService.GeneralSetting!.AutoStartType;
        LockModeTime = _settingService.GeneralSetting!.LockModeTime;
        AutoStart = _settingService.GeneralSetting!.AutoStart ?? false;
        AutoLogin = _settingService.GeneralSetting!.AutoLogin ?? false;
        UseLock = _settingService.GeneralSetting!.UseLockMode ?? false;
    }

    #region Properties
    public EAutoStartType AutoStartType
    {
        get => _autoStartType;
        set
        {
            SetProperty(ref _autoStartType, value);

            _settingService.GeneralSetting!.AutoStartType = AutoStartType;
            _settingService.SaveSetting();
        }
    }

    public ELockModeTime LockModeTime
    {
        get => _lockModeTime;
        set
        {
            SetProperty(ref _lockModeTime, value);

            _settingService.GeneralSetting!.LockModeTime = LockModeTime;
            _settingService.SaveSetting();
        }
    }

    public bool AutoStart
    {
        get => _autoStart;
        set
        {
            SetProperty(ref _autoStart, value);

            _settingService.GeneralSetting!.AutoStart = AutoStart;
            _settingService.SaveSetting();
        }
    }

    public bool AutoLogin
    {
        get => _autoLogin;
        set
        {
            SetProperty(ref _autoLogin, value);

            _settingService.GeneralSetting!.AutoLogin = AutoLogin;
            _settingService.SaveSetting();
        }
    }

    public bool UseLock
    {
        get => _useLock;
        set
        {
            SetProperty(ref _useLock, value);

            _settingService.GeneralSetting!.UseLockMode = UseLock;
            _settingService.SaveSetting();
        }
    }
    #endregion  // Properties

    #region Commands
    //
    #endregion  // Commands

    #region Commands Execute Methods
    //
    #endregion  // Commands Execute Methods

    #region Methods
    //
    #endregion  // Methods
}
