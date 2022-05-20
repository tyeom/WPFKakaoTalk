using Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services;

public interface IChattingService
{
    /// <summary>
    /// 대화내용 목록 요청
    /// </summary>
    /// <param name="id">대화방 id</param>
    /// <returns></returns>
    Task<IList<Chatting>?> GetChattingMessageAsync(Guid chatRoomId);

    /// <summary>
    /// 이전 채팅 데이터 요청
    /// </summary>
    /// <param name="chatRoomId">대화방 id</param>
    /// <param name="chatDataId">요청할 이전 채팅 데이터 기준 채팅 데이터 id<para/>이 데이터 기준으로 이전 데이터를 요청합니다.</param>
    /// <param name="count">가져올 데이터 개수</param>
    /// <returns></returns>
    Task<IList<Chatting>?> RequestToGetPreviousDataByCountAsync(Guid chatRoomId, Guid chatDataId, int count);

    /// <summary>
    /// 대화방 나가기
    /// </summary>
    /// <param name="chatRoomId"></param>
    void ChattingRoomLogout(Guid chatRoomId);

    /// <summary>
    /// 대화내용 지우기
    /// </summary>
    void ClearChattingMessage();

    /// <summary>
    /// 대화방 초대하기
    /// </summary>
    /// <param name="chatRoomId">초대 대화방 id</param>
    /// <param name="userId">초대대상 사용자 id</param>
    void InviteUser(Guid chatRoomId, Guid userId);

    /// <summary>
    /// 메세지 전송
    /// </summary>
    /// <param name="chatRoomId">대화방 id</param>
    /// <param name="message"></param>
    /// <returns></returns>
    Task<bool> SendMessageAsync(Guid chatRoomId, string message);

    /// <summary>
    /// 사진 전송
    /// </summary>
    /// <param name="chatRoomId">대화방 id</param>
    /// <param name="photoUri"></param>
    /// <returns></returns>
    Task<bool> SendPhotoAsync(Guid chatRoomId, string photoUri);

    /// <summary>
    /// 대화 데이터 수신
    /// </summary>
    /// <param name="chatRoomId"></param>
    /// <returns></returns>
    IAsyncEnumerable<Chatting> ChattingDataStreamAsync(Guid chatRoomId);

    /// <summary>
    /// 대화 데이터 수신 종료 토큰
    /// </summary>
    CancellationTokenSource ChattingDataStreamCancel { get; }

    void CloseChattingDataStream();
}

public class ChattingService : IChattingService
{
    private readonly SemaphoreSlim _sem;
    private CancellationTokenSource? _chattingDataStreamCancel;
    private ConcurrentQueue<Chatting>? _chatDataStreamCollection;
    private System.Windows.Threading.DispatcherTimer _임의테스트_서버_채팅데이터_수신처리;

    public ChattingService()
    {
        _sem = new SemaphoreSlim(0);
        _chattingDataStreamCancel = new();
        _chatDataStreamCollection = new();



        // TODO : ----------------- Test
        {
            User friendUser = new User()
            {
                Id = Guid.NewGuid(),
                FriendUserType = Common.Enums.EFriendUserType.Friend,
                Name = $"친구",
                Nationality = "KR",
                Email = $"friend@wpfkakao.com",
                NickName = $"arooong_친구",
                UserProfile = new()
                {
                    UserProfileImgBase64 = "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",
                }
            };

            DateTime? tmpDateTime = null;

            _임의테스트_서버_채팅데이터_수신처리 = new System.Windows.Threading.DispatcherTimer();
            _임의테스트_서버_채팅데이터_수신처리.Interval = new TimeSpan(0, 0, 5);
            _임의테스트_서버_채팅데이터_수신처리.Tick += (_, __) =>
            {
                bool flag = false;
                if (tmpDateTime is null)
                {
                    flag = true;
                    tmpDateTime = DateTime.Now;
                }
                else
                {
                    if ((DateTime.Now - tmpDateTime.Value).Minutes >= 1)
                        flag = true;
                }

                var randomMsg = $"[Test] 랜덤 수신 메세지 - {System.IO.Path.GetRandomFileName()}";

                Chatting chatting = new Chatting()
                {
                    User = friendUser,
                    Message = randomMsg,
                    ChattingMsgType = Common.Enums.EChattingMsgType.Normal,
                    ChattingSpeechType = Common.Enums.EChattingSpeechType.Opponent,
                    MessageDateTime = DateTime.Now,
                    ShowProfileImg = flag,
                    ShowName = flag,
                    ShowOpponentDateTime = true,
                    ShowMineDateTime = false
                };

                _chatDataStreamCollection.Enqueue(chatting);
                _sem.Release(1);
            };
            _임의테스트_서버_채팅데이터_수신처리.Start();
        }
        // ------------------------ Test


    }

    public CancellationTokenSource ChattingDataStreamCancel => _chattingDataStreamCancel;

    public async IAsyncEnumerable<Chatting> ChattingDataStreamAsync(Guid chatRoomId)
    {
        while (_chattingDataStreamCancel.IsCancellationRequested == false)
        {
            await _sem.WaitAsync();

            Chatting chatData;
            if (_chatDataStreamCollection.TryDequeue(out chatData))
            {
                yield return chatData;
            }
        }
    }

    public void ChattingRoomLogout(Guid chatRoomId)
    {
        throw new NotImplementedException();
    }

    public void ClearChattingMessage()
    {
        throw new NotImplementedException();
    }

    public void CloseChattingDataStream()
    {
        if(_임의테스트_서버_채팅데이터_수신처리 is not null)
            _임의테스트_서버_채팅데이터_수신처리.Stop();

        if (_chattingDataStreamCancel is not null)
            _chattingDataStreamCancel.Cancel();

        _chattingDataStreamCancel = null;
        _chatDataStreamCollection!.Clear();
        _chatDataStreamCollection = null;
    }

    public async Task<IList<Chatting>?> GetChattingMessageAsync(Guid id)
    {
        // TODO-Server : id 이용 (상대방 유저 id)
        // TODO-Server : 서버에 대화내용 리스트 요청

        User friendUser = new User()
        {
            Id = Guid.NewGuid(),
            FriendUserType = Common.Enums.EFriendUserType.Friend,
            Name = $"친구",
            Nationality = "KR",
            Email = $"friend@wpfkakao.com",
            NickName = $"arooong_친구",
            UserProfile = new()
            {
                UserProfileImgBase64 = "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",
            }
        };

        List<Chatting> chattingMessageList = new List<Chatting>();
        // 채팅방 리스트 추가 [mock data]
        var tasks = Enumerable.Range(0, 30).Select(p => this.CreateMockDataByChatting(p, friendUser));
        chattingMessageList.AddRange(await Task.WhenAll(tasks));

        return chattingMessageList;
    }

    public async Task<IList<Chatting>?> RequestToGetPreviousDataByCountAsync(Guid chatRoomId, Guid chatDataId, int count)
    {
        User friendUser = new User()
        {
            Id = Guid.NewGuid(),
            FriendUserType = Common.Enums.EFriendUserType.Friend,
            Name = $"친구",
            Nationality = "KR",
            Email = $"friend@wpfkakao.com",
            NickName = $"arooong_친구",
            UserProfile = new()
            {
                UserProfileImgBase64 = "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",
            }
        };

        List<Chatting> chattingMessageList = new List<Chatting>();
        // 채팅방 리스트 추가 [mock data]
        var tasks = Enumerable.Range(0, count).Select(p => this.CreateMockDataByChatting(p, friendUser));
        chattingMessageList.AddRange(await Task.WhenAll(tasks));

        return chattingMessageList;
    }

    public void InviteUser(Guid chatRoomId, Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SendMessageAsync(Guid chatRoomId, string message)
    {
        // TODO-Server : id 이용 (대화방 id)
        // TODO-Server : 서버에 대화전송 요청
        return true;
    }

    public Task<bool> SendPhotoAsync(Guid chatRoomId, string photoUri)
    {
        throw new NotImplementedException();
    }

    private async Task<Chatting> CreateMockDataByChatting(int num, User friendUser)
    {
        return await Task.Run(() =>
        {
            bool flag1 = false;
            bool flag2 = false;

            if (num == 0)
                flag1 = true;
            if (num == 10)
                flag2 = true;

            if (num == 11)
                flag1 = true;
            if (num == 29)
                flag2 = true;

            Chatting chatting = new Chatting()
            {
                User = friendUser,
                Message = $"대화 내용 - {num}",
                ChattingMsgType = Common.Enums.EChattingMsgType.Normal,
                ChattingSpeechType = Common.Enums.EChattingSpeechType.Opponent,
                MessageDateTime = DateTime.Now.AddSeconds(num),
                ShowProfileImg = flag1,
                ShowName = flag1,
                ShowOpponentDateTime = flag2,
                ShowMineDateTime = false
            };

            return chatting;
        });
    }
}