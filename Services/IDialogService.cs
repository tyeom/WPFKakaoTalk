using Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Views.Windows;

namespace Services;

public interface IDialogService
{
    Views.Windows.IDialog Dialog { get; }

    void SetSize(double width, double height);

    void SetVM(ViewModelBase vm, string? title, bool duplicateShow = false);
}

public class DialogService : IDialogService
{
    private readonly IDialog _popWindow;

    public DialogService(IDialog popWindow)
    {
        _popWindow = popWindow;
    }

    public IDialog Dialog => _popWindow;

    public void SetSize(double width, double height)
    {
        _popWindow.Width = width;
        _popWindow.Height = height;
    }

    public void SetVM(ViewModelBase vm, string? title, bool duplicateShow = false)
    {
        if (duplicateShow is false)
        {
            // 이미 표시된 팝업창이 있는 경우 활성화하고 return
            foreach (var popupWin in System.Windows.Application.Current.Windows)
            {
                if (_popWindow.DataContext is PopViewModel viewModel)
                {
                    if (viewModel.PopupVM is not null &&
                        viewModel.PopupVM.GetType().FullName == vm.GetType().FullName)
                    {
                        _popWindow.Activate();
                        return;
                    }
                }
            }
        }

        {
            if (_popWindow.DataContext is PopViewModel viewModel)
            {
                _popWindow.Title = title;
                viewModel.PopupVM = vm;
            }
        }
    }
}