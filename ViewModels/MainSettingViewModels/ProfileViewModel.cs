using Common.Base;
using Common.Enums;
using Common.Helper;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ViewModels.MainSettingViewModels;

public class ProfileViewModel : ViewModelBase
{
    private readonly IUserService _userService;
    private readonly ISettingService _settingService;

    private BitmapImage? _profileImg;
    private string _name;
    private string _nickName;
    private string _statusMsg;
    private string _email;
    private string _id;
    private bool _allowIDSearch;

    public ProfileViewModel(IUserService userService, ISettingService settingService)
    {
        _userService = userService;
        _settingService = settingService;

        Name = _userService.UserInfo.Name;
        NickName = _userService.UserInfo.NickName;
        StatusMsg = _userService.UserInfo.UserProfile.Status;
        Email = _userService.UserInfo.Email;
        Id = _userService.UserInfo.Id.ToString();
        AllowIDSearch = _settingService.ProfileSetting!.AllowIDSearch ?? false;

        this.SetProfileImgAsync();
    }

    #region Properties
    public BitmapImage? ProfileImg
    {
        get => _profileImg;
        set => SetProperty(ref _profileImg, value);
    }

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string NickName
    {
        get => _nickName;
        set => SetProperty(ref _nickName, value);
    }

    public string StatusMsg
    {
        get => _statusMsg;
        set => SetProperty(ref _statusMsg, value);
    }

    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    public string Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    public bool AllowIDSearch
    {
        get => _allowIDSearch;
        set
        {
            SetProperty(ref _allowIDSearch, value);

            _settingService.ProfileSetting!.AllowIDSearch = AllowIDSearch;
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
    private async void SetProfileImgAsync()
    {
        if (string.IsNullOrWhiteSpace(_userService.UserInfo.UserProfile.UserProfileImgBase64))
            return;

        await Task.Run(() =>
        {
            Dispatcher? dispatcher = null;
            if (System.Windows.Application.Current != null)
                dispatcher = System.Windows.Application.Current.Dispatcher;

            if (dispatcher is not null)
            {
                dispatcher.BeginInvoke(new Action(() =>
                {
                    ProfileImg = ImageHelper.Base64ToBitmapImage(_userService.UserInfo.UserProfile.UserProfileImgBase64);
                }));
            }
        });
    }
    #endregion  // Methods
}
