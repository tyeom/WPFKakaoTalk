using Common.Enums;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models;

public record User
{
    public Guid Id { get; init; }

    public string? Name { get; set; }

    public string PhoneNumber { get; init; } = string.Empty;

    public string Nationality { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string? NickName { get; set; }

    public EFriendUserType FriendUserType { get; set; } = EFriendUserType.Friend;

    public UserProfile UserProfile { get; set; } = new UserProfile();

    public IList<User> FriendList { get; set; } = new List<User>();

    public IList<ChattingRoom> ChattingRoomList { get; set; } = new List<ChattingRoom>();

    /// <summary>
    /// 이름 변경
    /// </summary>
    /// <param name="name"></param>
    public void UpdateName(string? name)
    {
        Name = name;
    }
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

    /// <summary>
    /// 프로필 이미지 변경
    /// </summary>
    /// <param name="base64Img"></param>
    public void UpdateUserProfile(string? base64Img)
    {
        UserProfileImgBase64 = base64Img;
    }

    /// <summary>
    /// 상태 메세지 변경
    /// </summary>
    /// <param name="status"></param>
    public void UpdateStatus(string? status)
    {
        Status = status;
    }
}