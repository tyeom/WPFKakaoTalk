using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Common.Behaviors;

public class TextBoxBehavior : DependencyObject
{
    public static readonly DependencyProperty AcceptsEnterProperty =
            DependencyProperty.RegisterAttached("AcceptsEnter",
                typeof(Boolean),
                typeof(TextBoxBehavior),
                new UIPropertyMetadata(false, (o, e) =>
                {
                    TextBox? textBox = o as TextBox;
                    if (textBox == null)
                    {
                        throw new ArgumentException("This property may only be used on TextBox");
                    }

                    if(((bool)e.NewValue) is true)
                    {
                        textBox.KeyDown += TextBox_KeyDown;
                    }
                    else
                    {
                        textBox.KeyDown -= TextBox_KeyDown;
                    }
                }
                ));

    private static void TextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            TextBox textBox = sender as TextBox;
            ICommand acceptsEnterCommand = GetAcceptsEnterCommand(textBox);

            if(acceptsEnterCommand is not null)
            acceptsEnterCommand.Execute(textBox.Text);
        }
    }

    public static bool GetAcceptsEnter(DependencyObject obj)
    {
        return (bool)obj.GetValue(AcceptsEnterProperty);
    }

    public static void SetAcceptsEnter(DependencyObject obj, bool value)
    {
        obj.SetValue(AcceptsEnterProperty, value);
    }

    public static ICommand GetAcceptsEnterCommand(DependencyObject obj)
    {
        return (ICommand)obj.GetValue(AcceptsEnterCommandProperty);
    }

    public static void SetAcceptsEnterCommand(DependencyObject obj, ICommand value)
    {
        obj.SetValue(AcceptsEnterCommandProperty, value);
    }

    public static readonly DependencyProperty AcceptsEnterCommandProperty =
        DependencyProperty.RegisterAttached(
            "AcceptsEnterCommand",
            typeof(ICommand),
            typeof(TextBoxBehavior),
            new UIPropertyMetadata(null)
        );
}
