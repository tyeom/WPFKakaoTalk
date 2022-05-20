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
/// ChattingWindow.xaml에 대한 상호 작용 논리
/// </summary>
public partial class ChattingWindow : Window, IChattingViewDialog
{
    /// <summary>
    /// 기존 창 크기
    /// </summary>
    private Rect _originWindow;

    public ChattingWindow()
    {
        InitializeComponent();
    }

    public Action CloseCallback { get; set; }

    private void TitleGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

            _originWindow.Width = this.Width;
            _originWindow.Height = this.Height;
        }
        else if (e.LeftButton == MouseButtonState.Pressed)
        {
            this.DragMove();
        }
    }

    private void TitleGrid_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton != MouseButtonState.Pressed)
        {
            return;
        }
        this.DragMove();
    }

    private void MaximizeToggleButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = this.xMaximizeToggleButton.IsChecked == true ? WindowState.Normal : WindowState.Maximized;
    }

    private void MinimizeToggleButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Minimized;
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        if(CloseCallback is not null)
            CloseCallback();

        this.Close();
    }
}
