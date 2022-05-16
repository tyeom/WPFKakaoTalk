using Common.Base;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using Models;
using Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.MainSettingViewModels;

namespace ViewModels;

public class SearchFriendViewModel : ViewModelBase
{
    private readonly IUserService _userService;
    private string _searchFriend;
    private User _searchUser;

    public SearchFriendViewModel(IUserService userService)
    {
        _userService = userService;
    }

    #region Properties
    public string SearchFriend
    {
        get => _searchFriend;
        set => SetProperty(ref _searchFriend, value);
    }

    public User SearchUser
    {
        get => _searchUser;
        set => SetProperty(ref _searchUser, value);
    }
    #endregion  // Properties

    #region Commands
    private AsyncRelayCommand<string> _searchFriendCommand;
    public AsyncRelayCommand<string> SearchFriendCommand
    {
        get
        {
            return _searchFriendCommand ??
                (_searchFriendCommand = new AsyncRelayCommand<string>(this.SearchFriendExecute));
        }
    }

    private RelayCommand _friendAddCommand;
    public RelayCommand FriendAddCommand
    {
        get
        {
            return _friendAddCommand ??
                (_friendAddCommand = new RelayCommand(() =>
                {
                    WeakReferenceMessenger.Default.Send<User, String>(SearchUser, "AddFriend");
                }));
        }
    }
    #endregion  // Commands

    #region Commands Execute Methods
    private async Task SearchFriendExecute(string search)
    {
        var foundFriendUser = await _userService.FindFriend(search);
        if (foundFriendUser is not null)
            SearchUser = foundFriendUser;
    }
    #endregion  // Commands Execute Methods

    #region Methods
    //
    #endregion  // Methods
}
