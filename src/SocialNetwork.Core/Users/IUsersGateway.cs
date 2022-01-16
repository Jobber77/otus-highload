using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Users
{
    public interface IUsersGateway
    {
        Task<IReadOnlyCollection<User>> SelectUsers(CancellationToken token, params long[] ids);
        Task<IReadOnlyCollection<User>> SearchUsers(string name, string surname, CancellationToken token);
        Task<IReadOnlyCollection<User>> SearchUsersByName(string name, CancellationToken token);
        Task<IReadOnlyCollection<User>> SearchUsersBySurname(string surname, CancellationToken token);

        Task<User?> GetUser(long id, CancellationToken token);
    }
}