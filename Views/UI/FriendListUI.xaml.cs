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
/// FriendList.xaml에 대한 상호 작용 논리
/// </summary>
public partial class FriendListUI : UserControl
{
    public FriendListUI()
    {
        InitializeComponent();
    }

    private void xSearchFriendHint_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        this.xSearchFriend.Focus();
    }

    private void xSearchFriend_GotFocus(object sender, RoutedEventArgs e)
    {
        this.xSearchBorder.BorderThickness = new Thickness(1);
        this.xSearchFriendHint.Visibility = Visibility.Collapsed;
    }

    private void xSearchFriend_LostFocus(object sender, RoutedEventArgs e)
    {
        this.xSearchBorder.BorderThickness = new Thickness(0);
        if (this.xSearchFriend.Text.Length <= 0)
        {
            this.xSearchFriendHint.Visibility = Visibility.Visible  ;
        }
    }
}
