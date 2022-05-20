using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Messaging;

public class UpdateUserInfo : ValueChangedMessage<User>
{
    public UpdateUserInfo(User user) : base(user) { }
}