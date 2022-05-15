using Common.Base;
using Microsoft.Toolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Messaging;

namespace ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly IDialogService _dialogService;
    private readonly ILoginService _loginService;
    private string? _userID;
    private byte[]? _password;

    public LoginViewModel(IDialogService dialogService, ILoginService loginService)
    {
        _dialogService = dialogService;
        _loginService = loginService;
    }

    #region Properties
    public string? UserID
    {
        get => _userID;
        set
        {
            SetProperty(ref _userID, value);
            LoginCommand.NotifyCanExecuteChanged();
        }
    }
    #endregion  // Properties

    #region Commands
    private RelayCommand<byte[]?> _loginCommand;
    public RelayCommand<byte[]?> LoginCommand
    {
        get
        {
            return _loginCommand ??
                (_loginCommand = new RelayCommand<byte[]?>(
                    this.LoginExecute,
                    (pass) => {
                        return string.IsNullOrWhiteSpace(UserID) == false;
                    }));
        }
    }
    #endregion  // Commands

    #region Commands Execute Methods
    private void LoginExecute(byte[]? pass)
    {
        _password = pass;

        string? error = null;
        User user = _loginService.UserLogin(UserID, _password, out error);

        WeakReferenceMessenger.Default.Send(new LoginCompleted(user));
    }
    #endregion  // Commands Execute Methods

    #region Methods
    //
    #endregion  // Methods
}
