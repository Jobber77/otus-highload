using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using SocialNetwork.Core.Users;
using SocialNetwork.Core.Users.Friends;
using SocialNetwork.Infrastructure.Database;

namespace SocialNetwork.Infrastructure.Users.Friends
{
    public class UserFriendsGateway : IUserFriendsGateway
    {
        private readonly IDatabaseUnitOfWork _unitOfWork;
        private readonly ILogger<UserFriendsGateway> _logger;

        public UserFriendsGateway(IDatabaseUnitOfWork unitOfWork, ILogger<UserFriendsGateway> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IReadOnlyCollection<UserFriend>> SelectFriends(User user, CancellationToken token)
        {
            var command = new CommandDefinition(FriendsQueries.SelectFriends, 
                new { UserId = user.Id}, cancellationToken: token);
            var connection = _unitOfWork.Connection.Value;
            var friends = await connection.QueryAsync<UserFriendStore>(command);
            var result = friends.Select(f => new UserFriend(f.UserId, f.FriendId)).ToList();
            return result;
        }

        public async Task<UserFriend?> GetFriend(User user, long friendId, CancellationToken token)
        {
            var command = new CommandDefinition(FriendsQueries.GetFriend, 
                new { UserId = user.Id, FriendId = friendId }, cancellationToken: token);
            var connection = _unitOfWork.Connection.Value;
            var friend = await connection.QueryFirstOrDefaultAsync<UserFriendStore>(command);
            return friend is null ? null : new UserFriend(friend.UserId, friend.FriendId);
        }

        public async Task<bool> TryAddFriend(User user, long friendId, CancellationToken token)
        {
            try
            {
                var connection = _unitOfWork.Connection.Value;
                var transaction = _unitOfWork.Transaction.Value;
                var command = new CommandDefinition(FriendsQueries.AddFriend, 
                    new { UserId = user.Id, FriendId = friendId}, transaction: transaction, cancellationToken: token);
                await connection.ExecuteAsync(command);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Unable to add friend");
                return false;
            }
        }
        
        public async Task DeleteFriend(User user, long friendId, CancellationToken token)
        {
            var connection = _unitOfWork.Connection.Value;
            var transaction = _unitOfWork.Transaction.Value;
            var command = new CommandDefinition(FriendsQueries.DeleteFriend, 
                new { UserId = user.Id, FriendId = friendId}, transaction: transaction, cancellationToken: token);

            await connection.ExecuteAsync(command);
        }
    }
}