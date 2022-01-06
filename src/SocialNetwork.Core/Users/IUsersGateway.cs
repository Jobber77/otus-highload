using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Users
{
    public interface IUsersGateway
    {
        Task<IReadOnlyCollection<User>> SelectUsers(CancellationToken token, params long[] ids);
        Task<User?> GetUser(long id, CancellationToken token);
    }
}