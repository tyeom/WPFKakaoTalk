using Common.Base;
using Common.Extensions;
using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels;

public class FriendListViewModel : ViewModelBase
{
    private readonly IUserService _userService;

    private ObservableCollection<User> _friendList;

    public FriendListViewModel(IUserService userService)
    {
        _userService = userService;

        FriendList = new ObservableCollection<User>(_userService.UserInfo.FriendList);
    }

    #region Properties
    public ObservableCollection<User> FriendList
    {
        get => _friendList;
        set => SetProperty(ref _friendList, value);
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
