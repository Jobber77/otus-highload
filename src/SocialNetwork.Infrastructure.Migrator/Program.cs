using System;
using FluentMigrator.Runner;
using FluentMigrator.Runner.VersionTableInfo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                .ConfigureServices(s => s
                    .AddFluentMigratorCore()
                    .ConfigureRunner(rb => rb
                        .AddMySql5()
                        .WithGlobalConnectionString(p =>
                        {
                            var config = p.GetRequiredService<IConfiguration>();
                            var connectionString = config.GetConnectionString("DefaultConnection");
                            if (connectionString is null)
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