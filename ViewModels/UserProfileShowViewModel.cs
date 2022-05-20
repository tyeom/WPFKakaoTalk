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
    private bool _showUpdateName;
    private string? _updateName;

    public UserProfileShowViewModel(User user)
    {
        User = user;
        UpdateName = user.Name;
        WeakReferenceMessenger.Default.Register<UserInfo, String>(this, "ShowProfile", this.SetUserInfo);
    }

    #region Properties
    public User User
    {
        get => _user;
        set => SetProperty(ref _user, value);
    }

    public bool ShowUpdateName
    {
        get => _showUpdateName;
        set => SetProperty(ref _showUpdateName, value);
    }

    public string? UpdateName
    {
        get => _updateName;
        set
        {
            SetProperty(ref _updateName, value);
        }
    }
    #endregion  // Properties

    #region Commands
    private RelayCommand _showUpdateNameCommand;
    public RelayCommand ShowUpdateNameCommand
    {
        get
        {
            return _showUpdateNameCommand ??
                (_showUpdateNameCommand = new RelayCommand(() => ShowUpdateName = true));
        }
    }

    private RelayCommand _closeUpdateNameCommand;
    public RelayCommand CloseUpdateNameCommand
    {
        get
        {
            return _closeUpdateNameCommand ??
                (_closeUpdateNameCommand = new RelayCommand(() =>
                {
                    ShowUpdateName = false;
                }));
        }
    }

    private AsyncRelayCommand _updateNameCommand;
    public AsyncRelayCommand UpdateNameCommand
    {
        get
        {
            return _updateNameCommand ??
                (_updateNameCommand = new AsyncRelayCommand(this.UpdateNameExecute));
        }
    }

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
    private async Task UpdateNameExecute()
    {
        // 친구 이름 변경 요청
        UpdateFriendNickNameResponseMessage responseMessage =
            await WeakReferenceMessenger.Default.Send<UpdateFriendNameRequestMessage>(
                new UpdateFriendNameRequestMessage(User, UpdateName)
                );

        if (responseMessage.IsError)
        {
            Logger.Log.Write($"친구 이름 변경 오류 : {responseMessage.ResponseMessage}");
        }
        else
        {
            User.UpdateName(UpdateName);
            OnPropertyChanged("User");
            WeakReferenceMessenger.Default.Send<UpdateUserInfo>(new UpdateUserInfo(User));
        }
    }
    #endregion  // Commands Execute Methods

    #region Methods
    private void SetUserInfo(object recipient, UserInfo userInfo)
    {
        //
    }

    public override void Cleanup()
    {
        base.Cleanup();
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }
    #endregion  // Methods
}
