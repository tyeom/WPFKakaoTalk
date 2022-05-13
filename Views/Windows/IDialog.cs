using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Views.Windows;

public interface IDialog
{
    public string Title { get; set; }

    public double Width { get; set; }

    public double Height { get; set; }

    object? DataContext { get; set; }

    bool Activate();

    void Show();

    bool? ShowDialog();

    void Close();
}
