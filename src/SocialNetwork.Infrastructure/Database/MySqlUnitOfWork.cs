using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace SocialNetwork.Infrastructure.Database
{
    public class MySqlUnitOfWork : IDatabaseUnitOfWork
    {
        public Lazy<DbTransaction> Transaction { get; }
        public Lazy<DbConnection> Connection { get; }

        public MySqlUnitOfWork(IConnectionFactory connectionFactory)
        {
            Connection = new Lazy<DbConnection>( () =>
            {
                var connectionString = connectionFactory.GetMaster();
                var connection = new MySqlConnection(connectionString);
                connection.Open();
                return connection;
            });

            Transaction = new Lazy<DbTransaction>(() =>
            {
                var connection = Connection.Value;
                var transaction = connection.BeginTransaction();
                return transaction;
            });
        }

        public async Task Commit(CancellationToken token)
        {
            var transaction = Transaction.Value;
            try
            {
                await transaction.CommitAsync(token);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(token);
                throw;
            }
        }

        public void Dispose()
        {
            Transaction.Value.Dispose();
            Connection.Value.Dispose();
        }
    }
}