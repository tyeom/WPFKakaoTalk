using System;
using System.Windows;
using System.Windows.Input;

namespace Views.Windows;

/// <summary>
/// PopWindow.xaml에 대한 상호 작용 논리
/// </summary>
public partial class PopWindow : Window, IDialog
{
    public PopWindow()
    {
        this.DataContext = new PopViewModel();
        InitializeComponent();
    }

    private void xCloseBtn_Click(object sender, RoutedEventArgs e)
    {
        this.Hide();
    }

    private void TitleGrid_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            this.DragMove();
        }
    }
}