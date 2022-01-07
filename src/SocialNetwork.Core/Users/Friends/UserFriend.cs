namespace SocialNetwork.Core.Users.Friends
{
    public class UserFriend
    {
        public long UserId { get; }
        public long FriendId { get; }

        public UserFriend(long userId, long friendId)
        {
            UserId = userId;
            FriendId = friendId;
        }
    }
}