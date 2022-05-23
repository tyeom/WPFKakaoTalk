using Common.Base;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using Services;
using System.Windows.Media;
using ViewModels.Messaging;

namespace ViewModels;

public class ShellViewModel : ObservableObject, IDisposable
{
    private readonly IDialogService _dialogService;
    private Brush _titleBackground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFFFFFFF")!;
    private bool _isLoginSettingBtnShow = false;
    private bool _isMaximizeShow = false;
    private object? _currentDataContext;

    public ShellViewModel(IServiceProvider services)
    {
        Services = services;

        _dialogService = Services.GetService(typeof(Services.IDialogService)) as Services.IDialogService;
        WeakReferenceMessenger.Default.Register<LoginCompleted>(this, this.SetMain);
        WeakReferenceMessenger.Default.Register<LogOutCompleted>(this, this.SetLogin);
    }

    #region Properties
    public static IServiceProvider Services { get; private set; }

    public Brush TitleBackground
    {
        get => _titleBackground;
        set => SetProperty(ref _titleBackground, value);
    }

    public bool IsLoginSettingBtnShow
    {
        get => _isLoginSettingBtnShow;
        set => SetProperty(ref _isLoginSettingBtnShow, value);
    }

    public bool IsMaximizeShow
    {
        get => _isMaximizeShow;
        set => SetProperty(ref _isMaximizeShow, value);
    }

    public object? CurrentDataContext
    {
        get => _currentDataContext;
        set => SetProperty(ref _currentDataContext, value);
    }
    #endregion  // Properties

    #region Commands
    private RelayCommand? _loginSettingCommand;
    public RelayCommand? LoginSettingCommand
    {
        get
        {
            return _loginSettingCommand ??
                (_loginSettingCommand = new RelayCommand(
                    () =>
                    {
                        _dialogService.SetSize(500, 650);
                        _dialogService.SetVM(new LoginSettingPopupViewModel(), "설정");
                        _dialogService.Dialog.ShowDialog();
                    }));
        }
    }
    #endregion  // Commands

    #region Commands Execute Methods
    //
    #endregion  // Commands Execute Methods

    #region Methods
    private void SetMain(object recipient, LoginCompleted loginCompleted)
    {
        if (string.IsNullOrWhiteSpace(loginCompleted.ErrorMessage) is true)
        {
            IsLoginSettingBtnShow = false;
            IsMaximizeShow = true;
            TitleBackground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFFFFFFF")!;

            ViewModelBase mainVM = Services.GetService(typeof(MainViewModel)) as MainViewModel;
            this.ChangeDataContext(mainVM);
        }
        else
        {
            Logger.Log.Write("Login failed");
            Logger.Log.Write(loginCompleted.ErrorMessage);
        }
    }

    private void SetLogin(object recipient, LogOutCompleted logOutCompleted)
    {
        if (string.IsNullOrWhiteSpace(logOutCompleted.ErrorMessage) is true)
        {
            IsLoginSettingBtnShow = true;
            IsMaximizeShow = false;
            TitleBackground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFFFE800")!;

            ViewModelBase loginVM = Services.GetService(typeof(LoginViewModel)) as LoginViewModel;
            this.ChangeDataContext(loginVM);
        }
        else
        {
            Logger.Log.Write("logOut failed");
            Logger.Log.Write(logOutCompleted.ErrorMessage);
        }
    }

    private void ChangeDataContext(ViewModelBase obj)
    {
        CurrentDataContext = obj;
    }

    public void Dispose()
    {
        CurrentDataContext = null;
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }
    #endregion  // Methods
}