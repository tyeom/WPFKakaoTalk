using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Common.Behaviors;

public class ListBoxScrollIntoViewBehavior : DependencyObject
{
    public static readonly DependencyProperty ScrollIntoTargetProperty =
            DependencyProperty.RegisterAttached("ScrollIntoTarget",
                typeof(Object),
                typeof(ListBoxScrollIntoViewBehavior),
                new UIPropertyMetadata(null, ScrollIntoTargetChanged)
            );

    public static object GetScrollIntoTarget(DependencyObject obj)
    {
        return (object)obj.GetValue(ScrollIntoTargetProperty);
    }

    public static void SetScrollIntoTarget(DependencyObject obj, object value)
    {
        obj.SetValue(ScrollIntoTargetProperty, value);
    }

    public static void ScrollIntoTargetChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue == null) return;

        var listBox = o as ListBox;
        listBox!.ScrollIntoView(e.NewValue);
    }
}
