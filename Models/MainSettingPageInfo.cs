using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models;

public class MainSettingPageInfo : ObservableObject
{
    private Object? _pageViewModel;

    public string? PageName
    {
        get;
        set;
    }

    public Object? PageViewModel
    {
        get => _pageViewModel;
        set => SetProperty(ref _pageViewModel, value);
    }
}
