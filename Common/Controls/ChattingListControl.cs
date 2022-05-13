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

public class ChattingListControl : ItemsControl
{
    public ChattingListControl()
    {
        this.DefaultStyleKey = typeof(ChattingListControl);
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
    }
}

[TemplatePart(Name = ChatInfoPartName, Type = typeof(Grid))]
public class ChattingListItemControl : ContentControl
{
    private const string ChatInfoPartName = "PART_ChatInfo";

    private Grid? _chatInfoPart;

    public ChattingListItemControl()
    {
        this.DefaultStyleKey = typeof(ChattingListItemControl);
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _chatInfoPart = GetTemplateChild(ChatInfoPartName) as Grid;

        if (_chatInfoPart is null) return;
        _chatInfoPart.MouseDown += this.ChatInfoPart_MouseDown;
    }

    private void ChatInfoPart_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
        {
            if (ChatCommand is not null)
                ChatCommand.Execute(ChatCommandParameter);
        }
    }

    public static readonly DependencyProperty ChatCommandProperty =
            DependencyProperty.Register(
                "ChatCommand",
                typeof(ICommand),
                typeof(ChattingListItemControl),
                new UIPropertyMetadata(null));

    public ICommand ChatCommand
    {
        get
        {
            return (ICommand)GetValue(ChatCommandProperty);
        }
        set
        {
            SetValue(ChatCommandProperty, value);
        }
    }

    public static readonly DependencyProperty ChatCommandParameterProperty =
            DependencyProperty.Register(
                "ChatCommandParameter",
                typeof(Object),
                typeof(ChattingListItemControl),
                new UIPropertyMetadata(null));

    public Object? ChatCommandParameter
    {
        get
        {
            return (Object?)GetValue(ChatCommandParameterProperty);
        }
        set
        {
            SetValue(ChatCommandParameterProperty, value);
        }
    }
}
