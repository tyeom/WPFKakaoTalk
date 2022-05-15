using Common.Base;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Messaging;

namespace ViewModels;

public class UserProfileShowViewModel : ViewModelBase
{
    private User _user;

    public UserProfileShowViewModel(User user)
    {
        User = user;
        WeakReferenceMessenger.Default.Register<UserInfo, String>(this, "ShowProfile", this.SetUserInfo);
    }

    #region Properties
    public User User
    {
        get => _user;
        set => SetProperty(ref _user, value);
    }
    #endregion  // Properties

    #region Commands
    private RelayCommand _closeCommand;
    public RelayCommand CloseCommand
    {
        get
        {
            return _closeCommand ??
                (_closeCommand = new RelayCommand(() =>
                {
                    WeakReferenceMessenger.Default.Unregister<UserInfo, String>(this, "ShowProfile");
                }));
        }
    }
    #endregion  // Commands

    #region Commands Execute Methods
    //
    #endregion  // Commands Execute Methods

    #region Methods
    private void SetUserInfo(object recipient, UserInfo userInfo)
    {
        //
    }
    #endregion  // Methods
}
