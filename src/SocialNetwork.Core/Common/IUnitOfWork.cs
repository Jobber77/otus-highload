using System.Threading;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Common
{
    public interface IUnitOfWork
    {
        Task Commit(CancellationToken token);
    }
}