using Common.Base;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Messaging;

public class MainPopup : ValueChangedMessage<ViewModelBase?>
{
    public MainPopup(ViewModelBase? viewModel) : base(viewModel) { }

    public string? Title { get; set; }

    public double Width { get; set; }

    public double Height { get; set; }
}