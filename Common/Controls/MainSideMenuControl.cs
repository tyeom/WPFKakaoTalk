using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Common.Controls;

public class MainSideMenuControl : ItemsControl
{
    public MainSideMenuControl()
    {
        this.DefaultStyleKey = typeof(MainSideMenuControl);
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
    }

    protected override DependencyObject GetContainerForItemOverride()
    {
        return new MainSideMenuItemControl();
    }
}

public class MainSideMenuItemControl : RadioButton
{
    public MainSideMenuItemControl()
    {
        this.DefaultStyleKey = typeof(MainSideMenuItemControl);
        base.GroupName = "MainSideMenu";
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
    }

    public Int16 Badge
    {
        get { return (Int16)base.GetValue(BadgeProperty); }
        set { base.SetValue(IconImageProperty, value); }
    }

    public static readonly DependencyProperty BadgeProperty =
      DependencyProperty.Register("Badge", typeof(Int16), typeof(MainSideMenuItemControl));

    public ImageSource? IConImage
    {
        get { return base.GetValue(IconImageProperty) as ImageSource; }
        set { base.SetValue(IconImageProperty, value); }
    }

    public static readonly DependencyProperty IconImageProperty =
      DependencyProperty.Register("IConImage", typeof(ImageSource), typeof(MainSideMenuItemControl));

    public ImageSource? MouseOverIConImage
    {
        get { return base.GetValue(MouseOverIConImageProperty) as ImageSource; }
        set { base.SetValue(MouseOverIConImageProperty, value); }
    }

    public static readonly DependencyProperty MouseOverIConImageProperty =
      DependencyProperty.Register("MouseOverIConImage", typeof(ImageSource), typeof(MainSideMenuItemControl));

    public ImageSource? SelectedIConImage
    {
        get { return base.GetValue(SelectedIConImageProperty) as ImageSource; }
        set { base.SetValue(SelectedIConImageProperty, value); }
    }

    public static readonly DependencyProperty SelectedIConImageProperty =
      DependencyProperty.Register("SelectedIConImage", typeof(ImageSource), typeof(MainSideMenuItemControl));
}