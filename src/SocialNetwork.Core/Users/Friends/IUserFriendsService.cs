using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;

namespace SocialNetwork.Core.Users.Friends
{
    public interface IUserFriendsService
    {
        Task<IReadOnlyCollection<User>> SelectFriends(User user, CancellationToken token);
        Task<Result> AddFriend(User user, long friendId, CancellationToken token);
        Task DeleteFriend(User user, long friendId, CancellationToken token);
        Task<bool> IsFriend(User user, long friendId, CancellationToken token);
    }
}