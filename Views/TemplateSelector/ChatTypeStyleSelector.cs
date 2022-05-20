using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Views.TemplateSelector;

public class ChatTypeStyleSelector : DataTemplateSelector
{
    public ChatTypeStyleSelector() { }

    public DataTemplate NomalTemplate { get; set; }

    public DataTemplate PhotoTemplate { get; set; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        return NomalTemplate;
    }
}
