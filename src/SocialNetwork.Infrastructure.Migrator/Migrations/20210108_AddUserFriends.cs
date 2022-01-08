using FluentMigrator;

namespace SocialNetwork.Infrastructure.Migrator.Migrations
{
    [Migration(20210108135200, TransactionBehavior.Default, "Create UserFriends table")]
    public class AddUserFriends : Migration
    {
        public override void Up()
        {
            Create.Table("UserFriends")
                .WithColumn("UserId").AsInt64().PrimaryKey().ForeignKey("Users", "Id")
                .WithColumn("FriendId").AsInt64().PrimaryKey().ForeignKey("Users", "Id")
                ;
        }

        public override void Down()
        {
            Delete.Table("UserFriends");
        }
    }
}