using Common.Enums;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models;

public record User
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string PhoneNumber { get; init; } = string.Empty;

    public string Nationality { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string NickName { get; set; } = string.Empty;

    public EFriendUserType FriendUserType { get; set; } = EFriendUserType.Friend;

    public UserProfile UserProfile { get; set; } = new UserProfile();

    public IList<User> FriendList { get; set; } = new List<User>();

    public IList<ChattingRoom> ChattingRoomList { get; set; } = new List<ChattingRoom>();
}

public record UserProfile
{
    /// <summary>
    /// 프로필 이미지 Base64
    /// </summary>
    public string? UserProfileImgBase64 { get; set; }

    /// <summary>
    /// 상태 메세지
    /// </summary>
    public string? Status { get; set; }
}