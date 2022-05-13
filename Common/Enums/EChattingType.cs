using Common.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Enums;

/// <summary>
/// 채팅방 타입
/// </summary>
[TypeConverter(typeof(EnumDescriptionConverter<EChattingRoomType>))]
public enum EChattingRoomType
{
    /// <summary>
    /// 1:1 채팅
    /// </summary>
    [Description("1:1채팅")]
    PersonalChat,

    /// <summary>
    /// 그룹 채팅
    /// </summary>
    [Description("그룹")]
    GroupChat,

    /// <summary>
    /// 플러스 친구 채팅
    /// </summary>
    [Description("플러스 친구 채팅")]
    PlusChat,

    /// <summary>
    /// 오픈 채팅
    /// </summary>
    [Description("오픈 채팅")]
    OpenChat,
}

/// <summary>
/// 채팅 메세지 타입
/// </summary>
public enum EChattingMsgType
{
    /// <summary>
    /// 기본 메세지
    /// </summary>
    Normal,
    /// <summary>
    /// 답장 메세지
    /// </summary>
    Reply,
    /// <summary>
    /// 공지 메세지
    /// </summary>
    Notice
}

/// <summary>
/// 채팅 말풍선 타입
/// </summary>
public enum EChattingSpeechType
{
    /// <summary>
    /// 나
    /// </summary>
    Me,
    /// <summary>
    /// 상대방
    /// </summary>
    Opponent,
}