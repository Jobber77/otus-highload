using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Users.Friends
{
    public interface IUserFriendsGateway
    {
        Task<IReadOnlyCollection<UserFriend>> SelectFriends(User user, CancellationToken token);
        Task<UserFriend?> GetFriend(User user, long friendId, CancellationToken token);
        Task<bool> TryAddFriend(User user, long friendId, CancellationToken token);
        Task DeleteFriend(User user, long friendId, CancellationToken token);
    }
}