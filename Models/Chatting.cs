using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models;

public record ChattingRoom
{
    public Guid Id { get; init; }

    public List<User>? user { get; set; }

    public string? ThumbnailBase64 { get; set; }

    public string? LastChatMsg { get; set; }

    public int ChatCount { get; set; }

    public DateTime? LastChatTime { get; set; }

    /// <summary>
    /// 알림 설정 여부
    /// </summary>
    public bool IsNotification { get; set; }

    public EChattingRoomType ChattingType { get; set; } = EChattingRoomType.PersonalChat;
}

public record Chatting
{
    public User user { get; set; } = new User();

    public EChattingMsgType ChattingMsgType { get; set; } = EChattingMsgType.Normal;

    public EChattingSpeechType ChattingSpeechType { get; set; } = EChattingSpeechType.Me;
}