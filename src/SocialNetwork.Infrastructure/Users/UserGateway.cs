using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using SocialNetwork.Core;

namespace SocialNetwork.Infrastructure.Users
{
    public class UserGateway : IUserStore<User>, IUserPasswordStore<User>
    {
        private readonly MySqlConnection _connection;

        public UserGateway(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            _connection = new MySqlConnection(connectionString);
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
                var command = new CommandDefinition(UserQueries.InsertUser, user, cancellationToken: cancellationToken);
                await _connection.ExecuteAsync(command);
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
            
            var command = new CommandDefinition(UserQueries.GetUserById, new { Id = id }, cancellationToken: cancellationToken);
            var found = await _connection.QueryAsync<UserStore>(command);
            var user = found.SingleOrDefault();
            return user is null
                ? null
                : new User(user.Id, user.UserName, user.Name, user.Surname, user.City, user.Age, user.Interests,
                    user.PasswordHash);
        }
        
        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(UserQueries.GetUserByNormalizedUserName, 
                new { NormalizedUserName = normalizedUserName }, 
                cancellationToken: cancellationToken);
            var found = await _connection.QueryAsync<UserStore>(command);
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
            _connection?.Dispose();
        }
    }
}