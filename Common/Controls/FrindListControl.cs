using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Common.Controls;

public class FrindListControl : ItemsControl
{
    public FrindListControl()
    {
        this.DefaultStyleKey = typeof(FrindListControl);
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
    }

    protected override DependencyObject GetContainerForItemOverride()
    {
        return new FrindListItemControl();
    }
}

[TemplatePart(Name = ProfilePartName, Type = typeof(Button))]
[TemplatePart(Name = FriendInfoPartName, Type = typeof(Grid))]
public class FrindListItemControl : ContentControl
{
    private const string ProfilePartName = "PART_Profile";
    private const string FriendInfoPartName = "PART_FriendInfo";

    private Button? _profilePart;
    private Grid? _friendInfoPart;

    public FrindListItemControl()
    {
        this.DefaultStyleKey = typeof(FrindListItemControl);
    }

    public Button? ProfileButton
    {
        get => _profilePart;
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _profilePart = GetTemplateChild(ProfilePartName) as Button;
        _friendInfoPart = GetTemplateChild(FriendInfoPartName) as Grid;

        if (_friendInfoPart == null) return;
        _friendInfoPart.MouseDown += this.FriendInfoPart_MouseDown;
    }

    private void FriendInfoPart_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
        {
            if(FriendInfoCommand is not null)
                FriendInfoCommand.Execute(FriendInfoCommandParameter);
        }
    }

    public BitmapImage? ProfileImg
    {
        get { return base.GetValue(ProfileImgProperty) as BitmapImage; }
        set { base.SetValue(ProfileImgProperty, value); }
    }

    public static readonly DependencyProperty ProfileImgProperty =
      DependencyProperty.Register("ProfileImg", typeof(BitmapImage), typeof(FrindListItemControl), new UIPropertyMetadata(null));

    public string? FriendName
    {
        get { return base.GetValue(FriendNameProperty) as string; }
        set { base.SetValue(FriendNameProperty, value); }
    }

    public static readonly DependencyProperty FriendNameProperty =
      DependencyProperty.Register("FriendName", typeof(string), typeof(FrindListItemControl), new UIPropertyMetadata(null));

    public string? FriendStatusMsg
    {
        get { return base.GetValue(FriendStatusMsgProperty) as string; }
        set { base.SetValue(FriendStatusMsgProperty, value); }
    }

    public static readonly DependencyProperty FriendStatusMsgProperty =
      DependencyProperty.Register("FriendStatusMsg", typeof(string), typeof(FrindListItemControl), new UIPropertyMetadata(null));

    public static readonly DependencyProperty FriendInfoCommandProperty =
            DependencyProperty.Register(
                "FriendInfoCommand",
                typeof(ICommand),
                typeof(FrindListItemControl),
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
                typeof(FrindListItemControl),
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
}
