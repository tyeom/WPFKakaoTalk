using Common.Base;
using Common.Helper;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ViewModels.Messaging;

namespace ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly IDialogService _mainSettingDialogService = (IDialogService)ShellViewModel.Services.GetService(typeof(IDialogService))!;
    private readonly IUserService _userService;
    private readonly ISettingService _settingService;

    // Lock Screen Mode
    private bool _isLockScreenMode = false;
    private BitmapImage? _profileImg;
    private string _email;

    private bool _loadingShow = false;
    private bool _showSettingMenuPopup = false;
    private bool _mainPopupShow = false;
    private string? _popupTitle;
    private double _popupWidth;
    private double _popupHeight;
    private ViewModelBase? _popupContentViewModel;
    private ViewModelBase? _contentViewModel;

    public MainViewModel(IUserService userService, ISettingService settingService)
    {
        Logger.Log.Write("MainViewModel Constructor");

        _userService = userService;
        _settingService = settingService;

        if (_settingService.GeneralSetting!.AutoLogin is not null &&
            _settingService.GeneralSetting!.AutoLogin.Value &&
            _settingService.GeneralSetting!.AutoStartType == Common.Enums.EAutoStartType.LockType)
        {
            IsLockScreenMode = true;
        }

        Email = _userService.UserInfo.Email;

        WeakReferenceMessenger.Default.Register<MainPopup>(this, this.ShowMainPopup);
        WeakReferenceMessenger.Default.Register<object, string>(this, "CloseMainPopup", this.CloseMainPopup);

        Logger.Log.Write("MainViewModel Constructor End");
    }

    #region Properties
    public bool LoadingShow
    {
        get => _loadingShow;
        set => SetProperty(ref _loadingShow, value);
    }

    public bool ShowSettingMenuPopup
    {
        get => _showSettingMenuPopup;
        set => SetProperty(ref _showSettingMenuPopup, value);
    }

    public bool IsLockScreenMode
    {
        get => _isLockScreenMode;
        set => SetProperty(ref _isLockScreenMode, value);
    }

    public BitmapImage? ProfileImg
    {
        get => _profileImg;
        set => SetProperty(ref _profileImg, value);
    }

    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    public bool MainPopupShow
    {
        get => _mainPopupShow;
        set => SetProperty(ref _mainPopupShow, value);
    }

    public string? PopupTitle
    {
        get => _popupTitle;
        set => SetProperty(ref _popupTitle, value);
    }

    public double PopupWidth
    {
        get => _popupWidth;
        set => SetProperty(ref _popupWidth, value);
    }

    public double PopupHeight
    {
        get => _popupHeight;
        set => SetProperty(ref _popupHeight, value);
    }

    /// <summary>
    /// 메인 팝업 컨텐츠
    /// </summary>
    public ViewModelBase? PopupContentViewModel
    {
        get => _popupContentViewModel;
        set => SetProperty(ref _popupContentViewModel, value);
    }

    /// <summary>
    /// 메인 컨텐츠
    /// </summary>
    public ViewModelBase? ContentViewModel
    {
        get => _contentViewModel;
        set => SetProperty(ref _contentViewModel, value);
    }
    #endregion  // Properties

    #region Commands
    private AsyncRelayCommand _loadedCommand;
    public AsyncRelayCommand LoadedCommand
    {
        get
        {
            return _loadedCommand ??
                (_loadedCommand = new AsyncRelayCommand(
                    this.LoadedExecute));
        }
    }

    private RelayCommand _unlockModeCommand;
    public RelayCommand UnLockModeCommand
    {
        get
        {
            return _unlockModeCommand ??
                (_unlockModeCommand = new RelayCommand(() => IsLockScreenMode = false));
        }
    }

    private RelayCommand<string> _sideMenuCommand;
    public RelayCommand<string> SideMenuCommand
    {
        get
        {
            return _sideMenuCommand ??
                (_sideMenuCommand = new RelayCommand<string>(this.SideMenuExecute));
        }
    }

    private RelayCommand _settingPopupCommand;
    public RelayCommand SettingPopupCommand
    {
        get
        {
            return _settingPopupCommand ??
                (_settingPopupCommand = new RelayCommand(
                    () => ShowSettingMenuPopup = true));
        }
    }

    private RelayCommand _settingCommand;
    public RelayCommand SettingCommand
    {
        get
        {
            return _settingCommand ??
                (_settingCommand = new RelayCommand(this.SettingExecute));
        }
    }

    private RelayCommand _popupCloseCommand;
    public RelayCommand PopupCloseCommand
    {
        get
        {
            return _popupCloseCommand ??
                (_popupCloseCommand = new RelayCommand(() =>
                {
                    PopupContentViewModel = null;
                    MainPopupShow = false;
                }));
        }
    }

    private RelayCommand _lockModeCommand;
    public RelayCommand LockModeCommand
    {
        get
        {
            return _lockModeCommand ??
                (_lockModeCommand = new RelayCommand(this.LockModeExecute));
        }
    }

    private RelayCommand _logoutCommand;
    public RelayCommand LogoutCommand
    {
        get
        {
            return _logoutCommand ??
                (_logoutCommand = new RelayCommand(this.LogoutExecute));
        }
    }

    private RelayCommand _exitCommand;
    public RelayCommand ExitCommand
    {
        get
        {
            return _exitCommand ??
                (_exitCommand = new RelayCommand(
                    () =>
                    {
                        System.Windows.Application.Current.ShutdownMode = System.Windows.ShutdownMode.OnMainWindowClose;
                        System.Windows.Application.Current.Shutdown();
                    }));
        }
    }
    #endregion  // Commands

    #region Commands Execute Methods
    private async Task LoadedExecute()
    {
        LoadingShow = true;

        // get my profile
        var myProfile = await _userService.GetMyProfileAsync(_userService.UserInfo.Id);
        // get my friend
        var myFriend = await _userService.GetFriendListAsync(_userService.UserInfo.Id);
        // get chatting room
        var chattingRoom = await _userService.GetChattingRoomListAsync(_userService.UserInfo.Id);

        _userService.UserInfo.UserProfile = myProfile!;
        _userService.UserInfo.FriendList = myFriend!;
        _userService.UserInfo.ChattingRoomList = chattingRoom!;

        LoadingShow = false;

        if(IsLockScreenMode)
        {
            this.SetProfileImgAsync();
        }

        ContentViewModel = ShellViewModel.Services!.GetService(typeof(FriendListViewModel)) as FriendListViewModel;
    }

    private void SideMenuExecute(string menu)
    {
        switch(menu)
        {
            case "FriendList":
                ContentViewModel = ShellViewModel.Services.GetService(typeof(FriendListViewModel)) as FriendListViewModel;
                break;
            case "ChattingList":
                ContentViewModel = ShellViewModel.Services.GetService(typeof(ChattingListViewModel)) as ChattingListViewModel;
                break;
            case "MoreMenu":
                ContentViewModel = ShellViewModel.Services.GetService(typeof(MoreMenuViewModel)) as MoreMenuViewModel;
                break;
            default:
                throw new Exception("Side menu를 찾을 수 없습니다.");
                break;
        }
    }

    private void SettingExecute()
    {
        _mainSettingDialogService.SetSize(500, 650);
        _mainSettingDialogService.SetVM(new MainSettingPopupViewModel(), "설정");
        _mainSettingDialogService.Dialog.Show();
    }

    private void LockModeExecute()
    {
        this.SetProfileImgAsync();
        IsLockScreenMode = true;
    }

    private void LogoutExecute()
    {
        if (_userService.UserLogout())
        {
            WeakReferenceMessenger.Default.Send(new LogOutCompleted(_userService.UserInfo));
        }
        else
        {
            WeakReferenceMessenger.Default.Send(new LogOutCompleted(_userService.UserInfo) { ErrorMessage = "로그아웃 요청 오류"});
        }
    }
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

    private void ShowMainPopup(object recipient, MainPopup mainPopup)
    {
        PopupContentViewModel = null;
        PopupContentViewModel = mainPopup!.Value;
        PopupTitle = mainPopup.Title;
        PopupWidth = mainPopup.Width;
        PopupHeight = mainPopup.Height;
        MainPopupShow = true;
    }

    private void CloseMainPopup(object recipient, object? obj)
    {
        PopupContentViewModel = null;
        PopupTitle = null;
        MainPopupShow = false;
    }
    #endregion  // Methods
}
