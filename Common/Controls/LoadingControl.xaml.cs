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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Common.Controls;
/// <summary>
/// LoadingControl.xaml에 대한 상호 작용 논리
/// </summary>
public partial class LoadingControl : UserControl
{
    public LoadingControl()
    {
        InitializeComponent();

        this.IsVisibleChanged += this.LoadingControl_IsVisibleChanged;
    }

    private void LoadingControl_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
    {
        Storyboard storyBoard = (Storyboard)this.FindResource("sbLoading");
        if (storyBoard != null)
        {
            if ((bool)e.NewValue)
            {
                storyBoard.Begin();
            }
            else
            {
                storyBoard.Stop();
            }
        }
    }

    public void Stop()
    {
        Storyboard storyBoard = (Storyboard)this.FindResource("sbLoading");
        if (storyBoard != null)
        {
            storyBoard.Stop();
        }
    }
}
