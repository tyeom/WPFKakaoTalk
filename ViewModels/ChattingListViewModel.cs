using Common.Base;
using Common.Extensions;
using CommunityToolkit.Mvvm.Input;
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

namespace ViewModels;

public class ChattingListViewModel : ViewModelBase
{
    private readonly IUserService _userService;

    private ObservableCollection<ChattingRoom> _chattingRoomList;

    public ChattingListViewModel(IUserService userService)
    {
        _userService = userService;

        ChattingRoomList = new ObservableCollection<ChattingRoom>(_userService.UserInfo.ChattingRoomList);
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
        //
    }
    #endregion  // Commands Execute Methods

    #region Methods
    //
    #endregion  // Methods
}
