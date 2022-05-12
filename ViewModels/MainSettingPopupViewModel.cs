using Common.Base;
using Common.Converters;
using Common.Enums;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.MainSettingViewModels;

namespace ViewModels;

public class MainSettingPopupViewModel : ViewModelBase
{
    private EAutoStartType _autoStartType = EAutoStartType.LockType;
    private ELockModeTime _lockModeTime = ELockModeTime.m1;
    private List<MainSettingPageInfo> _settingPageInfoList = new List<MainSettingPageInfo>(11);

    public MainSettingPopupViewModel()
    {
        this.CreateSettingPage();
    }

    #region Properties
    public List<MainSettingPageInfo> SettingPageInfoList
    {
        get => _settingPageInfoList;
        set => SetProperty(ref _settingPageInfoList, value);
    }

    public EAutoStartType AutoStartType
    {
        get => _autoStartType;
        set => SetProperty(ref _autoStartType, value);
    }

    public ELockModeTime LockModeTime
    {
        get => _lockModeTime;
        set => SetProperty(ref _lockModeTime, value);
    }
    #endregion  // Properties

    #region Commands
    #endregion  // Commands

    #region Commands Execute Methods
    //
    #endregion  // Commands Execute Methods

    #region Methods
    private void CreateSettingPage()
    {
        // 일반
        MainSettingPageInfo general = new MainSettingPageInfo()
        {
            PageName = "일반",
            PageViewModel = ShellViewModel.Services.GetService(typeof(GeneralViewModel)) as GeneralViewModel
        };
        // 프로필
        MainSettingPageInfo profile = new MainSettingPageInfo()
        {
            PageName = "프로필",
            PageViewModel = ShellViewModel.Services.GetService(typeof(ProfileViewModel)) as ProfileViewModel
        };

        // 멀티프로필
        MainSettingPageInfo multiProfile = new MainSettingPageInfo()
        {
            PageName = "멀티프로필"
        };
        // 알림
        MainSettingPageInfo notification = new MainSettingPageInfo()
        {
            PageName = "알림"
        };
        // 친구
        MainSettingPageInfo friend = new MainSettingPageInfo()
        {
            PageName = "친구"
        };
        // 채팅
        MainSettingPageInfo chating = new MainSettingPageInfo()
        {
            PageName = "채팅"
        };
        // 화면
        MainSettingPageInfo window = new MainSettingPageInfo()
        {
            PageName = "화면"
        };
        // 통화
        MainSettingPageInfo call = new MainSettingPageInfo()
        {
            PageName = "통화"
        };
        // 고급
        MainSettingPageInfo advanced = new MainSettingPageInfo()
        {
            PageName = "고급"
        };
        // 실험실
        MainSettingPageInfo testing = new MainSettingPageInfo()
        {
            PageName = "실험실"
        };
        // 정보
        MainSettingPageInfo about = new MainSettingPageInfo()
        {
            PageName = "정보",
            PageViewModel = new AboutViewModel()
        };

        List<MainSettingPageInfo> settingPageInfoList = new List<MainSettingPageInfo>(11);
        settingPageInfoList.Add(general);
        settingPageInfoList.Add(profile);
        settingPageInfoList.Add(multiProfile);
        settingPageInfoList.Add(notification);
        settingPageInfoList.Add(friend);
        settingPageInfoList.Add(chating);
        settingPageInfoList.Add(window);
        settingPageInfoList.Add(call);
        settingPageInfoList.Add(advanced);
        settingPageInfoList.Add(testing);
        settingPageInfoList.Add(about);

        SettingPageInfoList = settingPageInfoList;
    }
    #endregion  // Methods
}
