using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Views.UI;
/// <summary>
/// LoginSettingPopup.xaml에 대한 상호 작용 논리
/// </summary>
public partial class LoginSettingPopup : UserControl
{
    public LoginSettingPopup()
    {
        InitializeComponent();
    }

    private void xHttpProxyUriHint_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        this.xHttpProxyUri.Focus();
    }

    private void xHttpProxyPortHint_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        this.xHttpProxyPort.Focus();
    }

    private void xHttpProxyUri_GotFocus(object sender, RoutedEventArgs e)
    {
        this.xHttpProxyUriHint.Visibility = Visibility.Collapsed;
    }

    private void xHttpProxyUri_LostFocus(object sender, RoutedEventArgs e)
    {
        if(string.IsNullOrEmpty(xHttpProxyUri.Text) is true)
        {
            this.xHttpProxyUriHint.Visibility = Visibility.Visible;
        }
        else
        {
            this.xHttpProxyUriHint.Visibility = Visibility.Collapsed;
        }
    }

    private void xHttpProxyPort_GotFocus(object sender, RoutedEventArgs e)
    {
        this.xHttpProxyPortHint.Visibility = Visibility.Collapsed;
    }

    private void xHttpProxyPort_LostFocus(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(xHttpProxyPort.Text) is true)
        {
            this.xHttpProxyPortHint.Visibility = Visibility.Visible;
        }
        else
        {
            this.xHttpProxyPortHint.Visibility = Visibility.Collapsed;
        }
    }
}
