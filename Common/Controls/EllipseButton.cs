using System;
using System.Windows;
using System.Windows.Media;

namespace Common.Controls;

public class EllipseButton : ImageButton
{
    public EllipseButton()
    {
        DefaultStyleKey = typeof(EllipseButton);
    }

    public static readonly DependencyProperty? NormalBackgroundProperty =
        DependencyProperty.Register(
            "NormalBackground",
            typeof(Brush),
            typeof(EllipseButton),
            new PropertyMetadata((SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE5E5E5"))));  // Default Color #FFE5E5E5(그레이)
    /// <summary>
    /// 기본 배경
    /// </summary>
    public Brush? NormalBackground
    {
        get { return (Brush)this.GetValue(NormalBackgroundProperty); }
        set { this.SetValue(NormalBackgroundProperty, value); }
    }

    public static readonly DependencyProperty? PressBackgroundProperty =
        DependencyProperty.Register(
            "PressBackground",
            typeof(Brush),
            typeof(EllipseButton),
            new PropertyMetadata((SolidColorBrush)(new BrushConverter().ConvertFrom("#FF69010B")!)));  // Default Color #FF69010B(갈색)
    /// <summary>
    /// Press 배경
    /// </summary>
    public Brush? PressBackground
    {
        get { return (Brush)this.GetValue(PressBackgroundProperty); }
        set { this.SetValue(PressBackgroundProperty, value); }
    }

    public static readonly DependencyProperty? IsEllipseVisibleProperty =
        DependencyProperty.Register(
            "IsEllipseVisible",
            typeof(Boolean),
            typeof(EllipseButton),
            new PropertyMetadata(true));
    /// <summary>
    /// Ellipse 표시 여부
    /// </summary>
    public bool IsEllipseVisible
    {
        get { return (Boolean)this.GetValue(IsEllipseVisibleProperty); }
        set { this.SetValue(IsEllipseVisibleProperty, value); }
    }

    public static readonly DependencyProperty? IsDescriptVisibleProperty =
        DependencyProperty.Register(
            "IsDescriptVisible",
            typeof(Boolean),
            typeof(EllipseButton),
            new PropertyMetadata(true));
    /// <summary>
    /// 버튼 설명 표시 여부
    /// </summary>
    public bool IsDescriptVisible
    {
        get { return (Boolean)this.GetValue(IsDescriptVisibleProperty); }
        set { this.SetValue(IsDescriptVisibleProperty, value); }
    }

    public static readonly DependencyProperty? DescriptProperty =
        DependencyProperty.Register(
            "Descript",
            typeof(String),
            typeof(EllipseButton),
            new PropertyMetadata(string.Empty));
    /// <summary>
    /// 버튼 설명
    /// </summary>
    public string? Descript
    {
        get { return (string)this.GetValue(DescriptProperty); }
        set { this.SetValue(DescriptProperty, value); }
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
    }
}