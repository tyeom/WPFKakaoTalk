using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Messaging;

public class LoginCompleted : ValueChangedMessage<User>
{
    public LoginCompleted(User user) : base(user) { }

    public string ErrorMessage { get; set; }
}