using Common.Enums;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models;

public class ChattingRoom : ObservableObject
{
    private int _chatCount;
    private DateTime? _lastChatTime;

    public Guid Id { get; init; }

    public List<User>? user { get; set; }

    public string? ThumbnailBase64 { get; set; }

    public string? LastChatMsg { get; set; }

    public int ChatCount
    {
        get => _chatCount;
        set => SetProperty(ref _chatCount, value);
    }

    public DateTime? LastChatTime
    {
        get => _lastChatTime;
        set => SetProperty(ref _lastChatTime, value);
    }

    /// <summary>
    /// 알림 설정 여부
    /// </summary>
    public bool IsNotification { get; set; }

    public EChattingRoomType ChattingType { get; set; } = EChattingRoomType.PersonalChat;
}

public class Chatting : ObservableObject
{
    private bool _showProfileImg;
    private bool _showName;
    private bool _showMineDateTime;
    private bool _showOpponentDateTime;
    private bool _isSelected;
    public User _user;

    public Guid Id { get; set; }

    public User User
    {
        get => _user;
        set => SetProperty(ref _user, value);
    }

    public string? Message { get; set; }

    public string? ImageMessageUri { get; set; }

    public EChattingMsgType ChattingMsgType { get; set; } = EChattingMsgType.Normal;

    public EChattingSpeechType ChattingSpeechType { get; set; } = EChattingSpeechType.Mine;

    public bool ShowProfileImg
    {
        get => _showProfileImg;
        set => SetProperty(ref _showProfileImg, value);
    }

    public bool ShowName
    {
        get => _showName;
        set => SetProperty(ref _showName, value);
    }

    public bool ShowMineDateTime
    {
        get => _showMineDateTime;
        set => SetProperty(ref _showMineDateTime, value);
    }

    public bool ShowOpponentDateTime
    {
        get => _showOpponentDateTime;
        set => SetProperty(ref _showOpponentDateTime, value);
    }

    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    public DateTime MessageDateTime { get; set; }

    public void NotifyPropertyChanged(string propertyName)
    {
        OnPropertyChanged(propertyName);
    }
}