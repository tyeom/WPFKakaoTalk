using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Common.Controls;

[TemplatePart(Name = ProfilePartName, Type = typeof(Button))]
public class ChattingMessageControl : Control
{
    private const string ProfilePartName = "PART_Profile";

    private Button? _profilePart;

    public ChattingMessageControl()
    {
        this.DefaultStyleKey = typeof(ChattingMessageControl);
    }

    public static readonly DependencyProperty ChatContentProperty =
      DependencyProperty.Register("ChatContent", typeof(object), typeof(ChattingMessageControl), new UIPropertyMetadata(null));
    public object? ChatContent
    {
        get { return base.GetValue(ChatContentProperty) as object; }
        set { base.SetValue(ChatContentProperty, value); }
    }

    public static readonly DependencyProperty? SelectionBackgroundProperty =
        DependencyProperty.Register(
            "SelectionBackground",
            typeof(Brush),
            typeof(ChattingMessageControl),
            new PropertyMetadata((SolidColorBrush)(new BrushConverter().ConvertFrom("#FF606264"))!));  // Default Color #FF606264(그레이)
    /// <summary>
    /// 선택영역 배경
    /// </summary>
    public Brush SelectionBackground
    {
        get { return (Brush)this.GetValue(SelectionBackgroundProperty); }
        set { this.SetValue(SelectionBackgroundProperty, value); }
    }

    public static readonly DependencyProperty? IsSelectionProperty =
        DependencyProperty.Register(
            "IsSelection",
            typeof(Boolean),
            typeof(ChattingMessageControl),
            new PropertyMetadata(false));
    /// <summary>
    /// 선택 여부
    /// </summary>
    public bool IsSelection
    {
        get { return (bool)this.GetValue(IsSelectionProperty); }
        set { this.SetValue(IsSelectionProperty, value); }
    }

    public static readonly DependencyProperty FriendInfoCommandProperty =
            DependencyProperty.Register(
                "FriendInfoCommand",
                typeof(ICommand),
                typeof(ChattingMessageControl),
                new UIPropertyMetadata(null));

    public ICommand FriendInfoCommand
    {
        get
        {
            return (ICommand)GetValue(FriendInfoCommandProperty);
        }
        set
        {
            SetValue(FriendInfoCommandProperty, value);
        }
    }

    public static readonly DependencyProperty FriendInfoCommandParameterProperty =
            DependencyProperty.Register(
                "FriendInfoCommandParameter",
                typeof(Object),
                typeof(ChattingMessageControl),
                new UIPropertyMetadata(null));

    public Object? FriendInfoCommandParameter
    {
        get
        {
            return (Object?)GetValue(FriendInfoCommandParameterProperty);
        }
        set
        {
            SetValue(FriendInfoCommandParameterProperty, value);
        }
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _profilePart = GetTemplateChild(ProfilePartName) as Button;

        if (_profilePart is not null)
        {
            _profilePart.Click += (_, __) =>
            {
                if (FriendInfoCommand is not null)
                    FriendInfoCommand.Execute(FriendInfoCommandParameter);
            };
        }
    }
}
