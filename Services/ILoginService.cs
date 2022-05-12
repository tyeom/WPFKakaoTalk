using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services;

public interface ILoginService
{
    User UserInfo { get; }

    User UserLogin(string userID, byte[] userPass, out string? error);
}

public class LoginService : ILoginService
{
    private User _user = new User();

    public User UserInfo { get => _user; }

    public User UserLogin(string userID, byte[] userPass, out string? error)
    {
        // TODO-Server : userID, userPass 이용
        // TODO-Server : 서버에 로그인 요청 후 User정보 get

        error = null;
        User user = new User()
        {
            Id = Guid.NewGuid(),
            FriendUserType = Common.Enums.EFriendUserType.Me,
            Name = "WPF카카오톡",
            PhoneNumber = "01012344321",
            Nationality = "KR",
            Email = "wpfKakaoTalk@wpfkakao.com",
            NickName = "arooong"
        };

        _user = user;

        return user;
    }
}
