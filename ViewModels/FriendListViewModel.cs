using Common.Base;
using Common.Extensions;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ViewModels.Messaging;

namespace ViewModels;

public class FriendListViewModel : ViewModelBase
{
    private readonly IUserService _userService;
    private readonly IUserProfileViewerDialogService _userProfileViewerDialogService;

    private ICollectionView _friendListView;
    private User _myProfile;
    private ObservableCollection<User> _friendList;

    public FriendListViewModel(IUserService userService, IUserProfileViewerDialogService userProfileViewerDialogService)
    {
        _userService = userService;
        _userProfileViewerDialogService = userProfileViewerDialogService;

        MyProfile = _userService.UserInfo;

        FriendList = new ObservableCollection<User>(_userService.UserInfo.FriendList);
        FriendListView = CollectionViewSource.GetDefaultView(FriendList);

        PropertyGroupDescription groupDescription = new PropertyGroupDescription("FriendUserType");
        FriendListView.GroupDescriptions.Add(groupDescription);
        FriendListView = CollectionViewSource.GetDefaultView(FriendList);
    }

    #region Properties
    public User MyProfile
    {
        get => _myProfile;
        set => SetProperty(ref _myProfile, value);
    }

    public ObservableCollection<User> FriendList
    {
        get => _friendList;
        set => SetProperty(ref _friendList, value);
    }

    public ICollectionView FriendListView
    {
        get => _friendListView;
        set => SetProperty(ref _friendListView, value);
    }
    #endregion  // Properties

    #region Commands
    private RelayCommand<User?> _profileCommand;
    public RelayCommand<User?> ProfileCommand
    {
        get
        {
            return _profileCommand ??
                (_profileCommand = new RelayCommand<User?>(this.ProfileExecute));
        }
    }

    private RelayCommand<User?> _chatCommand;
    public RelayCommand<User?> ChatCommand
    {
        get
        {
            return _chatCommand ??
                (_chatCommand = new RelayCommand<User?>(this.ChatExecute));
        }
    }
    #endregion  // Commands

    #region Commands Execute Methods
    private void ProfileExecute(User? user)
    {
        if (user is null)
        {
            _userProfileViewerDialogService.SetVM(new UserProfileShowViewModel(_userService.UserInfo));
            _userProfileViewerDialogService.Dialog.Show();

            WeakReferenceMessenger.Default.Send<UserInfo, string>(
                new UserInfo(_userService.UserInfo) { IsMe = true},
                "ShowProfile"
            );
        }
        else
        {
            _userProfileViewerDialogService.SetVM(new UserProfileShowViewModel(user));
            _userProfileViewerDialogService.Dialog.Show();

            WeakReferenceMessenger.Default.Send<UserInfo, string>(
                new UserInfo(user!) { IsMe = false },
                "ShowProfile"
            );
        }
    }

    private void ChatExecute(User? user)
    {
        //
    }
    #endregion  // Commands Execute Methods

    #region Methods
    //
    #endregion  // Methods
}
