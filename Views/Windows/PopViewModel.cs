using Common.Base;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Views.Windows;

public class PopViewModel : ViewModelBase
{
    private ViewModelBase? _popupVM;

    public ViewModelBase? PopupVM
    {
        get => _popupVM;
        set => SetProperty(ref _popupVM, value);
    }

    private RelayCommand? _closeCommand;
    public RelayCommand? CloseCommand
    {
        get
        {
            return _closeCommand ??
                (_closeCommand = new RelayCommand(
                    () =>
                    {
                        PopupVM = null;
                    }));
        }
    }
}
