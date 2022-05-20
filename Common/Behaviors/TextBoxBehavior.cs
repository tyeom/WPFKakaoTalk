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
                        textBox.PreviewKeyDown += TextBox_PreviewKeyDown;
                    }
                    else
                    {
                        textBox.PreviewKeyDown -= TextBox_PreviewKeyDown;
                    }
                }
                ));

    private static void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine(e.Key.ToString());
        if (e.Key == Key.Enter && ( Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift) ))
        {
            TextBox textBox = sender as TextBox;
            int tmpCaretIndex = textBox.CaretIndex;
            textBox.Text = textBox.Text.Insert(textBox.CaretIndex, Environment.NewLine);
            textBox.CaretIndex = tmpCaretIndex + 1;
        }
        else if (e.Key == Key.Enter)
        {
            TextBox textBox = sender as TextBox;
            ICommand acceptsEnterCommand = GetAcceptsEnterCommand(textBox);

            if(acceptsEnterCommand is not null)
            acceptsEnterCommand.Execute(textBox.Text);

            e.Handled = true;
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
