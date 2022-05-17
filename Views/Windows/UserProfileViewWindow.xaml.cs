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
using System.Windows.Shapes;

namespace Views.Windows;
/// <summary>
/// UserProfileViewWindow.xaml에 대한 상호 작용 논리
/// </summary>
public partial class UserProfileViewWindow : Window, IUserProfileViewDialog
{
    public UserProfileViewWindow()
    {
        InitializeComponent();
    }

    private void Grid_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            this.DragMove();
        }
    }

    private void xCloseBtn_Click(object sender, RoutedEventArgs e)
    {
        // 프로필 보기 윈도우 창 재사용
        this.Hide();
        this.DataContext = null;
    }

    private void xUpdateNamePopupGrid_MouseMove(object sender, MouseEventArgs e)
    {
        e.Handled = true;
    }
}
