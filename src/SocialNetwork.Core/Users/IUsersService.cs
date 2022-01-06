using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;

namespace SocialNetwork.Core.Users
{
    public interface IUsersService
    {
        Task<Result<User>> CreateUser(User user, string password, CancellationToken token);
        Task<User?> GetUser(long id, CancellationToken token);
        Task<IReadOnlyCollection<User>> SelectUsers(CancellationToken token, params long[] ids);
    }
}