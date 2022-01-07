using FluentMigrator;

namespace SocialNetwork.Infrastructure.Migrator.Migrations
{
    [Migration(20210107145700, TransactionBehavior.Default, "Create user table")]
    public class AddUsers : Migration
    {
        public override void Up()
        {
            Create.Table("Test")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Text").AsString();
        }

        public override void Down()
        {
            Delete.Table("Test");
        }
    }
}