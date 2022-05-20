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

public class ChattingListViewModel : ViewModelBase
{
    private readonly IUserService _userService;

    private ObservableCollection<ChattingRoom> _chattingRoomList;

    public ChattingListViewModel(IUserService userService)
    {
        _userService = userService;

        ChattingRoomList = new ObservableCollection<ChattingRoom>(_userService.UserInfo.ChattingRoomList);

        // 친구 닉네임 변경 요청 Messenger
        WeakReferenceMessenger.Default.Register<ChattingListViewModel, UpdateFriendNameRequestMessage>(this, this.UpdateFriendNickNameRequest);
    }

    #region Properties
    public ObservableCollection<ChattingRoom> ChattingRoomList
    {
        get => _chattingRoomList;
        set => SetProperty(ref _chattingRoomList, value);
    }
    #endregion  // Properties

    #region Commands
    private RelayCommand<ChattingRoom?> _chatCommand;
    public RelayCommand<ChattingRoom?> ChatCommand
    {
        get
        {
            return _chatCommand ??
                (_chatCommand = new RelayCommand<ChattingRoom?>(this.ChatExecute));
        }
    }
    #endregion  // Commands

    #region Commands Execute Methods
    private void ChatExecute(ChattingRoom? chattingRoom)
    {
        var chattingViewerDialogService =
            ShellViewModel.Services.GetService(typeof(IChattingViewerDialogService)) as ChattingViewerDialogService;

        if (chattingViewerDialogService!.CheckActivate(chattingRoom!.Id.ToString()) is true)
        {
            // CheckActivate에서 해당 채팅 창 활성화
        }
        else
        {
            chattingViewerDialogService!.SetVM(new ChattingViewModel(chattingRoom!));
            chattingViewerDialogService!.Dialog.Show();
        }
    }
    #endregion  // Commands Execute Methods

    #region Methods
    private async void UpdateFriendNickNameRequest(object recipient, UpdateFriendNameRequestMessage requestMessage)
    {
        var result = await _userService.UpdateFriendNameAsync(requestMessage.User.Id, requestMessage.NewName);

        if (result is true)
        {
            requestMessage.Reply(new UpdateFriendNickNameResponseMessage("ok", false));
        }
        else
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
