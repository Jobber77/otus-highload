using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Identity;
using SocialNetwork.Core.Users;
using SocialNetwork.Infrastructure.Database;

namespace SocialNetwork.Infrastructure.Users
{
    public class UsersGateway : IUsersGateway, IUserStore<User>, IUserPasswordStore<User>
    {
        private readonly IDatabaseUnitOfWork _unitOfWork;

        public UsersGateway(IDatabaseUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            try
            {
                var connection = _unitOfWork.Connection.Value;
                var transaction = _unitOfWork.Transaction.Value;
                var command = new CommandDefinition(UsersQueries.InsertUser, user, transaction: transaction, cancellationToken: cancellationToken);
                var id = await connection.ExecuteScalarAsync<long>(command);
                user.SetIdentity(id);
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                return IdentityResult.Failed(new IdentityError { Description = e.Message});
            }
            
        }

        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            // TODO: впилить апдейт обновляемых полей
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(IdentityResult.Success);
        }
        
#nullable disable
        
        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (!long.TryParse(userId, out var id))
                throw new ArgumentException("User id must be of long type");

            return await GetUserById(id, cancellationToken);
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(UsersQueries.GetUserByNormalizedUserName, 
                new { NormalizedUserName = normalizedUserName }, 
                cancellationToken: cancellationToken);
            var connection = _unitOfWork.Connection.Value;
            var found = await connection.QueryAsync<UserStore>(command);
            var user = found.SingleOrDefault();
            return user is null
                ? null
                : new User(user.Id, user.UserName, user.Name, user.Surname, user.City, user.Age, user.Interests,
                    user.PasswordHash);
        }
        
#nullable enable

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.UpdatePassword(passwordHash);
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash ?? string.Empty);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }
        
        public void Dispose()
        {
        }

        public async Task<IReadOnlyCollection<User>> SelectUsers(CancellationToken cancellationToken, params long[] ids)
        {
            var command = new CommandDefinition(UsersQueries.GetUsersByIds, 
                new { Ids = ids }, cancellationToken: cancellationToken);
            var connection = _unitOfWork.Connection.Value;
            var found = await connection.QueryAsync<UserStore>(command);
            var result = found.Select(user => new User(user.Id, user.UserName, user.Name, user.Surname, user.City,
                user.Age, user.Interests, user.PasswordHash)).ToList();
            
            return result;
        }

        public async Task<User?> GetUser(long id, CancellationToken token)
        {
            return await GetUserById(id, token);
        }
        
        private async Task<User?> GetUserById(long id, CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(UsersQueries.GetUserById, new {Id = id}, cancellationToken: cancellationToken);
            var connection = _unitOfWork.Connection.Value;
            var found = await connection.QueryAsync<UserStore>(command);
            var user = found.SingleOrDefault();
            return user is null
                ? null
                : new User(user.Id, user.UserName, user.Name, user.Surname, user.City, user.Age, user.Interests,
                    user.PasswordHash);
        }
    }
}