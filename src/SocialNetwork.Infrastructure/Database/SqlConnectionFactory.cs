using System;
using System.Linq;
using Microsoft.Extensions.Options;

namespace SocialNetwork.Infrastructure.Database
{
    public class SqlConnectionFactory : IConnectionFactory
    {
        private readonly IOptionsMonitor<MySqlOptions> _optionsMonitor;

        public SqlConnectionFactory(IOptionsMonitor<MySqlOptions> optionsMonitor)
        {
            _optionsMonitor = optionsMonitor;
        }
        
        public string GetMaster()
        {
            var masterConnection = _optionsMonitor.CurrentValue.Master;
            if (string.IsNullOrEmpty(masterConnection))
                throw new Exception("Master connection string is not defined");
            return masterConnection;
        }

        public string GetReplica()
        {
            var options = _optionsMonitor.CurrentValue;
            var connections = options.Replicas;
            if (!connections.Any())
            {
                var master = options.Master;
                return string.IsNullOrEmpty(master)
                    ? throw new Exception("Nor Replica Nor Master connection string defined")
                    : master;
            }
            var index = new Random().Next(0, connections.Count);
            var connection= connections[index];
            return connection;
        }
    }
}