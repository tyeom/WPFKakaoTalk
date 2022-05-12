using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Enums;

/// <summary>
/// 친구 목록 유저 타입
/// </summary>
public enum EFriendUserType
{
    /// <summary>
    /// 로그인 된 자신
    /// </summary>
    Me,

    /// <summary>
    /// 친구
    /// </summary>
    Friend,

    /// <summary>
    /// 플러스 친구
    /// </summary>
    PlusFrind,

    /// <summary>
    /// 채널
    /// </summary>
    Channel
}
