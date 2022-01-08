using FluentMigrator;

namespace SocialNetwork.Infrastructure.Migrator.Migrations
{
    [Migration(20210107145700, TransactionBehavior.Default, "Create user table")]
    public class AddUsers : Migration
    {
        public override void Up()
        {
            const int defaultStringSize = 256;
            Create.Schema("SocialNetwork");
            Create.Table("Users")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("UserName").AsString(defaultStringSize).NotNullable()
                .WithColumn("NormalizedUserName").AsString(defaultStringSize).NotNullable()
                .WithColumn("PasswordHash").AsString(1024).NotNullable()
                .WithColumn("Name").AsString(defaultStringSize).NotNullable()
                .WithColumn("NormalizedName").AsString(defaultStringSize).NotNullable()
                .WithColumn("Surname").AsString(defaultStringSize).NotNullable()
                .WithColumn("NormalizedSurname").AsString(defaultStringSize).NotNullable()
                .WithColumn("Age").AsInt16().Nullable()
                .WithColumn("Interests").AsString(2048).Nullable()
                .WithColumn("City").AsString(defaultStringSize).NotNullable()
                ;
        }

        public override void Down()
        {
            Delete.Table("Users");
        }
    }
}