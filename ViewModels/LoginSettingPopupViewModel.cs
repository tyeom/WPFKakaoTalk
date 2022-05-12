using Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels;

public class LoginSettingPopupViewModel : ViewModelBase
{
    private bool _useHttpProxy;
    private string? _httpProxyUri;
    private string? _httpProxyPort;

    public bool UseHttpProxy
    {
        get => _useHttpProxy;
        set => SetProperty(ref _useHttpProxy, value);
    }

    public string? HttpProxyUri
    {
        get => _httpProxyUri;
        set => SetProperty(ref _httpProxyUri, value);
    }

    public string? HttpProxyPort
    {
        get => _httpProxyPort;
        set => SetProperty(ref _httpProxyPort, value);
    }
}
