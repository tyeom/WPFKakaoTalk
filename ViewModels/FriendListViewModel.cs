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
    private bool _showSearchFriend;
    private string? _searchFriend;

    public FriendListViewModel(IUserService userService, IUserProfileViewerDialogService userProfileViewerDialogService)
    {
        _userService = userService;
        _userProfileViewerDialogService = userProfileViewerDialogService;

        MyProfile = _userService.UserInfo;

        FriendList = new ObservableCollection<User>(_userService.UserInfo.FriendList);
        FriendListView = CollectionViewSource.GetDefaultView(FriendList);

        PropertyGroupDescription groupDescription = new PropertyGroupDescription("FriendUserType");
        FriendListView.GroupDescriptions.Add(groupDescription);

        if (FriendListView.CanFilter)
        {
            FriendListView.Filter = this.FriendSearchFilter;
        }

        // 친구 추가 Messenger
        WeakReferenceMessenger.Default.Register<User, string>(this, "AddFriend", this.AddFriend);

        // 친구 닉네임 변경 요청 Messenger
        WeakReferenceMessenger.Default.Register<FriendListViewModel, UpdateFriendNameRequestMessage>(this, this.UpdateFriendNickNameRequest);
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

    public bool ShowSearchFriend
    {
        get => _showSearchFriend;
        set => SetProperty(ref _showSearchFriend, value);
    }

    public string? SearchFriend
    {
        get => _searchFriend;
        set
        {
            SetProperty(ref _searchFriend, value);
            FriendListView.Refresh();
        }
    }
    #endregion  // Properties

    #region Commands
    private RelayCommand _showSearchFriendCommand;
    public RelayCommand ShowSearchFriendCommand
    {
        get
        {
            return _showSearchFriendCommand ??
                (_showSearchFriendCommand = new RelayCommand(() => ShowSearchFriend = true));
        }
    }

    private RelayCommand _closeSearchFriendCommand;
    public RelayCommand CloseSearchFriendCommand
    {
        get
        {
            return _closeSearchFriendCommand ??
                (_closeSearchFriendCommand = new RelayCommand(() =>
                {
                    SearchFriend = null;
                    ShowSearchFriend = false;
                }));
        }
    }

    private RelayCommand _addSearchFriendCommand;
    public RelayCommand AddSearchFriendCommand
    {
        get
        {
            return _addSearchFriendCommand ??
                (_addSearchFriendCommand = new RelayCommand(this.AddSearchFriendExecute));
        }
    }

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
    private void AddSearchFriendExecute()
    {
        WeakReferenceMessenger.Default.Send(
            new MainPopup(
                ShellViewModel.Services.GetService(typeof(SearchFriendViewModel)) as SearchFriendViewModel
                ) { Title = "친구 추가", Width = 330, Height = 465}
            );
    }

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
        var chattingViewerDialogService =
            ShellViewModel.Services.GetService(typeof(IChattingViewerDialogService)) as ChattingViewerDialogService;

        var chatRoom = _userService.UserInfo.ChattingRoomList
            // 1:1 채팅 목록 검색
            .Where(p => p.ChattingType == Common.Enums.EChattingRoomType.PersonalChat)
            // 1:1 채팅 목록에서 상대방 친구와 채팅한 내역의 채팅방 검색
            .FirstOrDefault(p => p.user![1].Id == user!.Id);

        if (chatRoom is null)
        {
            var chattingRoom = new ChattingRoom() {
                Id = Guid.NewGuid(),
                user = new() {_userService.UserInfo, user!},
                ThumbnailBase64 = user.UserProfile.UserProfileImgBase64,
                ChattingType = Common.Enums.EChattingRoomType.PersonalChat
            };
            chattingViewerDialogService!.SetVM(new ChattingViewModel(chattingRoom));
            chattingViewerDialogService!.Dialog.Show();

            _userService.UserInfo.ChattingRoomList.Add(chattingRoom);
        }
        else
        {
            if (chattingViewerDialogService!.CheckActivate(chatRoom!.Id.ToString()) is true)
            {
                // CheckActivate에서 해당 채팅 창 활성화
            }
            else
            {
                chattingViewerDialogService!.SetVM(new ChattingViewModel(chatRoom));
                chattingViewerDialogService!.Dialog.Show();
            }
        }
    }
    #endregion  // Commands Execute Methods

    #region Methods
    private bool FriendSearchFilter(object item)
    {
        if (string.IsNullOrWhiteSpace(SearchFriend) is true)
            return true;

        User? userInfo = item as User;
        if(userInfo is not null && userInfo.Name.Contains(SearchFriend) is true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void AddFriend(object recipient, User user)
    {
        FriendList.Add(user);
        FriendListView.Refresh();

        // 메인 팝업 화면 닫기
        WeakReferenceMessenger.Default.Send<object, string>(null, "CloseMainPopup");
    }

    private async void UpdateFriendNickNameRequest(object recipient, UpdateFriendNameRequestMessage requestMessage)
    {
        var result = await _userService.UpdateFriendNameAsync(requestMessage.User.Id, requestMessage.NewName);

        if (result is true)
        {
            requestMessage.Reply(new UpdateFriendNickNameResponseMessage("ok", false));
            FriendListView.Refresh();
        }  else
        {
            requestMessage.Reply(new UpdateFriendNickNameResponseMessage("이름 변경 중 오류가 발생하였습니다.", true));
            return;
        }
    }

    public override void Cleanup()
    {
        base.Cleanup();
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }
    #endregion  // Methods
}
