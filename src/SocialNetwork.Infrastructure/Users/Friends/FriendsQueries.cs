namespace SocialNetwork.Infrastructure.Users
{
    public class FriendsQueries
    {
        public static string AddFriend => @"
            INSERT INTO UserFriends (
                   UserId,
                   FriendId)
            VALUES (
                   @UserId,
                   @FriendId)";
        
        public static string DeleteFriend => @"DELETE FROM UserFriends WHERE UserId = @UserId AND FriendId = @FriendId";
        
        public static string SelectFriends => @"SELECT UserId, FriendId FROM UserFriends WHERE UserId = @UserId";
        
        public static string GetFriend => @"SELECT UserId, FriendId FROM UserFriends WHERE UserId = @UserId AND FriendId = @FriendId";
    }
}