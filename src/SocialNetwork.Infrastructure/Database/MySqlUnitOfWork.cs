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

        public MySqlUnitOfWork(IConfiguration configuration)
        {
            Connection = new Lazy<DbConnection>( () =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
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