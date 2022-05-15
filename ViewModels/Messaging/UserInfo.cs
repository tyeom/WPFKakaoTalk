using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Messaging;

public class UserInfo : ValueChangedMessage<User>
{
    public UserInfo(User user) : base(user) { }

    public bool IsMe { get; set; }
}