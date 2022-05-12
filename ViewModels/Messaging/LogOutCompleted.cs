using CommunityToolkit.Mvvm.Messaging.Messages;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Messaging;

public class LogOutCompleted : ValueChangedMessage<User>
{
    public LogOutCompleted(User user) : base(user) { }

    public string ErrorMessage { get; set; }
}