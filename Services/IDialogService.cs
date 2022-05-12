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

    void SetVM(ViewModelBase vm, string? title);
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

    public void SetVM(ViewModelBase vm, string? title)
    {
        if (_popWindow.DataContext is PopViewModel viewModel)
        {
            _popWindow.Title = title;
            viewModel.PopupVM = vm;
        }
    }
}