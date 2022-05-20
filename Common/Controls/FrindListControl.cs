using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Common.Controls;

[TemplatePart(Name = MyProfilePartName, Type = typeof(Button))]
public class FrindListControl : ListBox
{
    private const string MyProfilePartName = "PART_MyProfile";

    private Button? _myProfile;

    public FrindListControl()
    {
        this.DefaultStyleKey = typeof(FrindListControl);
    }

    public static readonly DependencyProperty MyProfileCommandProperty =
            DependencyProperty.Register(
                "MyProfileCommand",
                typeof(ICommand),
                typeof(FrindListControl),
                new UIPropertyMetadata(null));

    public ICommand MyProfileCommand
    {
        get
        {
            return (ICommand)GetValue(MyProfileCommandProperty);
        }
        set
        {
            SetValue(MyProfileCommandProperty, value);
        }
    }

    public static readonly DependencyProperty MyProfileCommandParameterProperty =
            DependencyProperty.Register(
                "MyProfileCommandParameter",
                typeof(Object),
                typeof(FrindListControl),
                new UIPropertyMetadata(null));

    public Object? MyProfileCommandParameter
    {
        get
        {
            return (Object?)GetValue(MyProfileCommandParameterProperty);
        }
        set
        {
            SetValue(MyProfileCommandParameterProperty, value);
        }
    }

    public BitmapImage? ProfileImg
    {
        get { return base.GetValue(ProfileImgProperty) as BitmapImage; }
        set { base.SetValue(ProfileImgProperty, value); }
    }

    public static readonly DependencyProperty ProfileImgProperty =
      DependencyProperty.Register("ProfileImg", typeof(BitmapImage), typeof(FrindListControl), new UIPropertyMetadata(null));

    public string? MyName
    {
        get { return base.GetValue(MyNameProperty) as string; }
        set { base.SetValue(MyNameProperty, value); }
    }

    public static readonly DependencyProperty MyNameProperty =
      DependencyProperty.Register("MyName", typeof(string), typeof(FrindListControl), new UIPropertyMetadata(null));

    public string? StatusMsg
    {
        get { return base.GetValue(StatusMsgProperty) as string; }
        set { base.SetValue(StatusMsgProperty, value); }
    }

    public static readonly DependencyProperty StatusMsgProperty =
      DependencyProperty.Register("StatusMsg", typeof(string), typeof(FrindListControl), new UIPropertyMetadata(null));

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _myProfile = GetTemplateChild(MyProfilePartName) as Button;
        if (_myProfile is not null)
        {
            _myProfile.Click += (_, __) =>
            {
                if (MyProfileCommand is not null)
                    MyProfileCommand.Execute(MyProfileCommandParameter);
            };
        }
    }

    //protected override DependencyObject GetContainerForItemOverride()
    //{
    //    return new FrindListItemControl();
    //}
}

[TemplatePart(Name = ProfilePartName, Type = typeof(Button))]
[TemplatePart(Name = FriendInfoPartName, Type = typeof(Grid))]
public class FrindListItemControl : Control
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

    public Guid? Id
    {
        get { return base.GetValue(IdProperty) as Guid?; }
        set { base.SetValue(IdProperty, value); }
    }

    public static readonly DependencyProperty IdProperty =
      DependencyProperty.Register("Id", typeof(Guid), typeof(FrindListItemControl), new UIPropertyMetadata(null));

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _profilePart = GetTemplateChild(ProfilePartName) as Button;
        _friendInfoPart = GetTemplateChild(FriendInfoPartName) as Grid;

        if (_profilePart is not null)
        {
            _profilePart.Click += (_, __) =>
            {
                if (FriendInfoCommand is not null)
                    FriendInfoCommand.Execute(FriendInfoCommandParameter);
            };
        }

        if (_friendInfoPart is null) return;
        _friendInfoPart.MouseDown += this.FriendInfoPart_MouseDown;
    }

    private void FriendInfoPart_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
        {
            if(FriendChatCommand is not null)
                FriendChatCommand.Execute(FriendChatCommandParameter);
        }
    }

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

    public static readonly DependencyProperty FriendChatCommandProperty =
            DependencyProperty.Register(
                "FriendChatCommand",
                typeof(ICommand),
                typeof(FrindListItemControl),
                new UIPropertyMetadata(null));

    public ICommand FriendChatCommand
    {
        get
        {
            return (ICommand)GetValue(FriendChatCommandProperty);
        }
        set
        {
            SetValue(FriendChatCommandProperty, value);
        }
    }

    public static readonly DependencyProperty FriendChatCommandParameterProperty =
            DependencyProperty.Register(
                "FriendChatCommandParameter",
                typeof(Object),
                typeof(FrindListItemControl),
                new UIPropertyMetadata(null));

    public Object? FriendChatCommandParameter
    {
        get
        {
            return (Object?)GetValue(FriendChatCommandParameterProperty);
        }
        set
        {
            SetValue(FriendChatCommandParameterProperty, value);
        }
    }
}
