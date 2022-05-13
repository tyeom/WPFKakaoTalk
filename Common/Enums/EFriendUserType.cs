using Common.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Enums;

/// <summary>
/// 친구 목록 유저 타입
/// </summary>
[TypeConverter(typeof(EnumDescriptionConverter<EFriendUserType>))]
public enum EFriendUserType
{
    [Description("")]
    None,

    /// <summary>
    /// 친구
    /// </summary>
    [Description("친구")]
    Friend,

    /// <summary>
    /// 프로필 정보 업데이트된 친구
    /// </summary>
    [Description("업데이트한 친구")]
    UpdateFriend,

    /// <summary>
    /// 플러스 친구
    /// </summary>
    [Description("플러스")]
    PlusFrind,

    /// <summary>
    /// 채널
    /// </summary>
    [Description("채널")]
    Channel
}
