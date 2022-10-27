using Common.Base;
using Common.Extensions;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using Models;
using Services;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ViewModels.Messaging;

namespace ViewModels;

public class ChattingViewModel : ViewModelBase
{
    private readonly ChattingRoom _chattingRoom;
    private readonly IChattingService _chattingService =
        ShellViewModel.Services.GetService(typeof(IChattingService)) as ChattingService;
    private readonly IUserProfileViewerDialogService _userProfileViewerDialogService =
        ShellViewModel.Services.GetService(typeof(IUserProfileViewerDialogService)) as UserProfileViewerDialogService;

    private string _chattingRoomId;
    private string _profileImg;
    private string _title;

    private string _chatMessage;

    private ObservableCollection<Chatting> _chattingMessageList;
    private ObservableCollection<Chatting> _selectedChattingMessageList = new ObservableCollection<Chatting>();
    private Chatting _selectedChattingMessageItem;
    private Guid? _requestPreviousDataGuid;

    public ChattingViewModel(ChattingRoom chattingRoom)
    {
        _chattingRoom = chattingRoom;

        this.Init();
        this.GetChattingMessageAsync();
        this.ReceiveChattingData();

        // 대화 상대의 사용자 정보가 변경 되었을때 메신저
        WeakReferenceMessenger.Default.Register<UpdateUserInfo>(this, this.SetUpdateUserInfo);
    }

    #region Properties
    public string ChattingRoomId
    {
        get => _chattingRoomId;
        set => SetProperty(ref _chattingRoomId, value);
    }

    public string ProfileImg
    {
        get => _profileImg;
        set => SetProperty(ref _profileImg, value);
    }

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public string ChatMessage
    {
        get => _chatMessage;
        set
        {
            SetProperty(ref _chatMessage, value);
            ChatMsgSendCommand.NotifyCanExecuteChanged();
        }
    }

    public ObservableCollection<Chatting> ChattingMessageList
    {
        get => _chattingMessageList;
        set => SetProperty(ref _chattingMessageList, value);
    }

    public ObservableCollection<Chatting> SelectedChattingMessageList
    {
        get => _selectedChattingMessageList;
        set => SetProperty(ref _selectedChattingMessageList, value);
    }


    public Chatting SelectedChattingMessageItem
    {
        get => _selectedChattingMessageItem;
        set => SetProperty(ref _selectedChattingMessageItem, value);
    }
    #endregion  // Properties

    #region Commands
    private RelayCommand _loadedCommand;
    public RelayCommand LoadedCommand
    {
        get
        {
            return _loadedCommand ??
                (_loadedCommand = new RelayCommand(
                    this.LoadedExecute));
        }
    }

    private RelayCommand _profileCommand;
    public RelayCommand ProfileCommand
    {
        get
        {
            return _profileCommand ??
                (_profileCommand = new RelayCommand(this.ProfileExecute));
        }
    }

    private AsyncRelayCommand _chatMsgSendCommand;
    public AsyncRelayCommand ChatMsgSendCommand
    {
        get
        {
            return _chatMsgSendCommand ??
                (_chatMsgSendCommand = new AsyncRelayCommand(this.ChatMsgSendExecute,
                () => string.IsNullOrWhiteSpace(ChatMessage) is false));
        }
    }

    private RelayCommand _scrollIntoBottomCommand;
    public RelayCommand ScrollIntoBottomCommand
    {
        get
        {
            return _scrollIntoBottomCommand ??
                (_scrollIntoBottomCommand = new RelayCommand(this.ScrollIntoBottomExecute, null));
        }
    }

    private AsyncRelayCommand<Guid> _requestGetDataCommand;
    public AsyncRelayCommand<Guid> RequestGetDataCommand
    {
        get
        {
            return _requestGetDataCommand ??
                (_requestGetDataCommand = new AsyncRelayCommand<Guid>(this.RequestGetDataCommandExecute, null));
        }
    }
    #endregion  // Commands

    #region Commands Execute Methods
    private void LoadedExecute()
    {
        if (ChattingMessageList is not null && ChattingMessageList.Count > 0)
            SelectedChattingMessageItem = ChattingMessageList.Last();
    }

    private void ProfileExecute()
    {
        _userProfileViewerDialogService.SetVM(new UserProfileShowViewModel(_chattingRoom.user![1]));
        _userProfileViewerDialogService.Dialog.Show();

        WeakReferenceMessenger.Default.Send<UserInfo, string>(
            new UserInfo(_chattingRoom.user![1]) { IsMe = false },
            "ShowProfile"
        );
    }

    private async Task ChatMsgSendExecute()
    {
        var result = await _chattingService.SendMessageAsync(_chattingRoom.Id, ChatMessage);
        if(result)
        {
            Chatting chatting = new()
            {
                User = _chattingRoom.user![0],
                Message = ChatMessage,
                ChattingMsgType = Common.Enums.EChattingMsgType.Normal,
                ChattingSpeechType = Common.Enums.EChattingSpeechType.Mine,
                ShowProfileImg = false,
                ShowName = false,
                MessageDateTime = DateTime.Now,
                ShowOpponentDateTime = false
            };

            var timeDifference = this.CheckTimeDifference(chatting);
            // 바로 이전 나의 대화 시간과 1분 이상 차이 나지 않은 경우 시간 표기를 마지막 대화에만 표시한다.
            if (timeDifference != -1 && timeDifference == 0)
            {
                ChattingMessageList.Last().ShowMineDateTime = false;
                chatting.ShowMineDateTime = true;
            }
            else
            {
                chatting.ShowMineDateTime = true;
            }

            ChattingMessageList.Add(chatting);
            SelectedChattingMessageItem = chatting;
        }
        else
        {
            Logger.Log.Write($"ChatMsgSend 실패 - {ChatMessage}");
        }

        ChatMessage = String.Empty;
    }

    private void ScrollIntoBottomExecute()
    {
        if (ChattingMessageList is null || ChattingMessageList.Count <= 0)
            return;

        SelectedChattingMessageItem = ChattingMessageList.Last();
    }

    private async Task RequestGetDataCommandExecute(Guid requestGuid)
    {
        var previousData = await _chattingService.RequestToGetPreviousDataByCountAsync(_chattingRoom.Id, ChattingMessageList[0].Id, 50);
        if (previousData is not null)
        {
            _requestPreviousDataGuid = requestGuid;

            ChattingMessageList.AddRangeFirst(previousData);
            SelectedChattingMessageItem = previousData.Last();
        }
    }
    #endregion  // Commands Execute Methods

    #region Methods
    private async void Init()
    {
        ChattingRoomId = _chattingRoom.Id.ToString();
        ProfileImg = _chattingRoom.user![1].UserProfile.UserProfileImgBase64!;
        Title = _chattingRoom.user![1].Name;
    }

    private async Task GetChattingMessageAsync()
    {
        ChattingMessageList = new(await _chattingService.GetChattingMessageAsync(_chattingRoom.Id));
        LoadedExecute();
    }

    private async Task ReceiveChattingData()
    {
        await foreach (var chatData in _chattingService.ChattingDataStreamAsync(_chattingRoom.Id))
        {
            ChattingMessageList.Add(chatData);
            _chattingRoom.ChatCount++;
        }
    }

    private double CheckTimeDifference(Chatting chatting)
    {
        if (ChattingMessageList is null || ChattingMessageList.Count <= 0)
            return -1;

        var lastChattingMessage = ChattingMessageList.Last();
        if (lastChattingMessage.ChattingSpeechType != chatting.ChattingSpeechType)
            return -1;

        return (chatting.MessageDateTime - lastChattingMessage.MessageDateTime).Minutes;
    }

    private void SetUpdateUserInfo(object recipient, UpdateUserInfo userInfo)
    {
        // 상대 정보 업데이트
        if(_chattingRoom.user[1].Id == userInfo.Value.Id)
        {
            _chattingRoom.user[1] = userInfo.Value;
            this.Init();
        }

        // 대화 메세지의 유저 정보 업데이트
        // TODO : 이 부분을 모든 대화 메세지 loop 하여 개별 참조하지 않고
        // TODO : 모든 대화 정보 인스턴스가 같은 한 객체를 바라보도록 처리해서 좀 더 효율적으로 업데이트 처리 하는 고려를 해봐야 함.
        foreach(var chattingMessage in ChattingMessageList)
        {
            // TODO : Mock데이터로 임의 친구 데이터의 채팅 대화 내역이기에 User를 변경해준다.
            // TODO : 실제 유저 데이터의 채팅 대화 내역이라면 해당 처리는 필요 없다.
            chattingMessage.User = userInfo.Value;
            chattingMessage.NotifyPropertyChanged("User");
        }
    }

    public override void Cleanup()
    {
        _chattingService.CloseChattingDataStream();
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }
    #endregion  // Methods
}
