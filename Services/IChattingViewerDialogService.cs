using Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Views.Windows;

namespace Services;

public interface IChattingViewerDialogService
{
    Views.Windows.IChattingViewDialog Dialog { get; }

    void SetVM(ViewModelBase chattingViewVM);

    bool CheckActivate(string chattingRoomId);
}

public class ChattingViewerDialogService : IChattingViewerDialogService
{
    private IChattingViewDialog? _chattingViewWindow;

    public ChattingViewerDialogService(IChattingViewDialog chattingViewWindow)
    {
        _chattingViewWindow = chattingViewWindow;

        _chattingViewWindow.CloseCallback = () =>
        {
            if (_chattingViewWindow.DataContext is ViewModelBase vm)
            {
                vm.Cleanup();
                _chattingViewWindow.DataContext = null;
            }
        };
    }

    public IChattingViewDialog Dialog => _chattingViewWindow;

    public void SetVM(ViewModelBase? vm)
    {
        _chattingViewWindow.DataContext = vm;
    }

    public bool CheckActivate(string chattingRoomId)
    {
        var chatWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(p => p.Tag is not null && p.Tag.ToString() == chattingRoomId);
        if (chatWin is not null)
        {
            _chattingViewWindow = null;
            chatWin.Activate();
            return true;
        }
        else
        {
            return false;
        }
    }
}