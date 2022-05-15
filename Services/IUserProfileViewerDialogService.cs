using Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Views.Windows;

namespace Services;

public interface IUserProfileViewerDialogService
{
    Views.Windows.IUserProfileViewDialog Dialog { get; }

    void SetVM(ViewModelBase userProfileViewVM);
}

public class UserProfileViewerDialogService : IUserProfileViewerDialogService
{
    private readonly IUserProfileViewDialog _userProfileViewWindow;

    public UserProfileViewerDialogService(IUserProfileViewDialog userProfileViewWindow)
    {
        _userProfileViewWindow = userProfileViewWindow;
    }

    public IUserProfileViewDialog Dialog => _userProfileViewWindow;

    public void SetVM(ViewModelBase vm)
    {
        _userProfileViewWindow.DataContext = vm;
    }
}