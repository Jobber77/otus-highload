using FluentMigrator;

namespace SocialNetwork.Infrastructure.Migrator.Migrations
{
    [Migration(20220123141600, TransactionBehavior.Default, "Create Person search index")]
    public class AddSearchPersonIndex : Migration
    {
        public override void Up()
        {
            Create.Index().OnTable("Users")
                .OnColumn("NormalizedName").Ascending()
                .OnColumn("NormalizedSurname").Ascending();
            Create.Index().OnTable("Users")
                .OnColumn("NormalizedSurname").Ascending();
        }

        public override void Down()
        {
            Delete.Index().OnTable("Users").OnColumns("NormalizedName", "NormalizedSurname");
            Delete.Index().OnTable("Users").OnColumn("NormalizedSurname");
        }
    }
}