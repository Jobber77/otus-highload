using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using SocialNetwork.Core.Common;

namespace SocialNetwork.Core.Users.Friends
{
    public class UserFriendsService : IUserFriendsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserFriendsGateway _userFriendsGateway;
        private readonly IUsersGateway _usersGateway;

        public UserFriendsService(IUnitOfWork unitOfWork, IUserFriendsGateway userFriendsGateway, IUsersGateway usersGateway)
        {
            _unitOfWork = unitOfWork;
            _userFriendsGateway = userFriendsGateway;
            _usersGateway = usersGateway;
        }

        public async Task<IReadOnlyCollection<User>> SelectFriends(User user, CancellationToken token)
        {
            var userFriends = await _userFriendsGateway.SelectFriends(user, token);
            
            if (!userFriends.Any())
                return Array.Empty<User>();

            var friends = await _usersGateway.SelectUsers(token,
                userFriends.Select(uf => uf.FriendId).ToArray());
                
            return friends;
        }

        public async Task<Result> AddFriend(User user, long friendId, CancellationToken token)
        {
            if (user.Id == friendId)
                return Result.Fail("You can not add yourself as a friend");
            
            var success = await _userFriendsGateway.TryAddFriend(user, friendId, token);
            if (!success)
                return Result.Fail($"Unable to add friend {friendId} to user {user?.Id}");
            
            await _unitOfWork.Commit(token);
            
            return Result.Ok();
        }

        public async Task DeleteFriend(User user, long friendId, CancellationToken token)
        {
            await _userFriendsGateway.DeleteFriend(user, friendId, token);
            await _unitOfWork.Commit(token);
        }
        
        public async Task<bool> IsFriend(User user, long friendId, CancellationToken token)
        {
            var userFriend = await _userFriendsGateway.GetFriend(user, friendId, token);
            return userFriend is not null;
        }
    }
}