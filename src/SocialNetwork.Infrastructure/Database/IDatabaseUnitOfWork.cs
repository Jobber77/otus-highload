using System;
using System.Data.Common;
using SocialNetwork.Core.Common;

namespace SocialNetwork.Infrastructure.Database
{
    public interface IDatabaseUnitOfWork : IUnitOfWork, IDisposable
    {
        Lazy<DbTransaction> Transaction { get; }
        Lazy<DbConnection> Connection { get; }
    }
}