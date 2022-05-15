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
        // IDialogService를 재사용 하는 뷰모델이 있을 수 있기에 [MainView > SettingExecute, 메인 환경설정 팝업창]
        // Close시 완전 창을 닫지 않고 숨김처리만 한다.
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