using FluentMigrator.Runner.VersionTableInfo;

namespace SocialNetwork.Infrastructure.Migrator.Migrations
{
    public class VersionTable : DefaultVersionTableMetaData
    {
        public override string TableName => "MigrationHistory";
    }
}