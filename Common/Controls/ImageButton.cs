using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Common.Controls;

public class ImageButton : Button
{
    public ImageButton()
    {
        DefaultStyleKey = typeof(ImageButton);
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
    }

    public ImageSource? NormalImage
    {
        get { return base.GetValue(NormalImageProperty) as ImageSource; }
        set { base.SetValue(NormalImageProperty, value); }
    }

    public static readonly DependencyProperty? NormalImageProperty =
      DependencyProperty.Register("NormalImage", typeof(ImageSource), typeof(ImageButton));

    public ImageSource? PressImage
    {
        get { return base.GetValue(PressImageProperty) as ImageSource; }
        set { base.SetValue(PressImageProperty, value); }
    }

    public static readonly DependencyProperty? PressImageProperty =
      DependencyProperty.Register("PressImage", typeof(ImageSource), typeof(ImageButton));

    public ImageSource? DisableImage
    {
        get { return base.GetValue(DisableImageProperty) as ImageSource; }
        set { base.SetValue(DisableImageProperty, value); }
    }

    public static readonly DependencyProperty? DisableImageProperty =
      DependencyProperty.Register("DisableImage", typeof(ImageSource), typeof(ImageButton));

    public bool? IsDisable
    {
        get { return base.GetValue(IsDisableProperty) as bool?; }
        set { base.SetValue(IsDisableProperty, value); }
    }

    public static readonly DependencyProperty? IsDisableProperty =
      DependencyProperty.Register("IsDisable", typeof(bool?), typeof(ImageButton));
}