using System;
using System.Linq;
using FluentMigrator.Runner;
using FluentMigrator.Runner.VersionTableInfo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SocialNetwork.Infrastructure.Database;
using SocialNetwork.Infrastructure.Migrator.Migrations;

namespace SocialNetwork.Infrastructure.Migrator
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = CreateServices();
            using var scope = serviceProvider.CreateScope();
            UpdateDatabase(scope.ServiceProvider);
        }

        private static IServiceProvider CreateServices()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureHostConfiguration(b =>
                {
                    b.AddUserSecrets<Program>();
                })
                .ConfigureServices((c, s) => s
                    .AddMySql(c.Configuration)
                    .AddFluentMigratorCore()
                    .ConfigureRunner(rb => rb
                        .AddMySql5()
                        .WithGlobalConnectionString(p =>
                        {
                            var factory = p.GetRequiredService<IConnectionFactory>();
                            var connectionString = factory.GetMaster();
                            if (string.IsNullOrEmpty(connectionString))
                                throw new ArgumentNullException(nameof(connectionString));
                            return connectionString;
                        })
                        .ScanIn(typeof(AddUsers).Assembly).For.Migrations())
                    .AddLogging(lb => lb.AddFluentMigratorConsole()));
            return host.Build().Services;
        }

        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }
    }
}