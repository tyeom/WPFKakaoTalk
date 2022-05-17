using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using Models;

namespace ViewModels.Messaging;

public class UpdateFriendNameRequestMessage : AsyncRequestMessage<UpdateFriendNickNameResponseMessage>
{
    public UpdateFriendNameRequestMessage(User user, string? newName)
    {
        User = user;
        NewName = newName;
    }

    public User User { get; set; }

    public string? NewName { get; set; }
}

public class UpdateFriendNickNameResponseMessage
{
    public UpdateFriendNickNameResponseMessage(string responseMessage, bool isError)
    {
        ResponseMessage = responseMessage;
        IsError = isError;
    }

    public string? ResponseMessage { get; set; }

    public bool IsError { get; set; }
}