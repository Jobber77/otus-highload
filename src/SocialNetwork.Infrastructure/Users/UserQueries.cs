namespace SocialNetwork.Infrastructure.Users
{
    public class UserQueries
    {
        public static string InsertUser => @"
            INSERT INTO Users (
                   UserName,
                   NormalizedUserName,
                   PasswordHash,
                   Name,
                   NormalizedName,
                   Surname,
                   NormalizedSurname,
                   Age,
                   Interests,
                   City)
            VALUES (
                   @UserName,
                   @NormalizedUserName,
                   @PasswordHash,
                   @Name,
                   @NormalizedName,
                   @Surname,
                   @NormalizedSurname,
                   @Age,
                   @Interests,
                   @City)";

        public static string GetUserById => @"
            SELECT
                Id,
                UserName,
                NormalizedUserName,
                PasswordHash,
                Name,
                NormalizedName,
                Surname,
                NormalizedSurname,
                Age,
                Interests,
                City
            FROM Users
            WHERE Id = @Id";
        
        public static string GetUserByNormalizedUserName => @"
            SELECT
                Id,
                UserName,
                NormalizedUserName,
                PasswordHash,
                Name,
                NormalizedName,
                Surname,
                NormalizedSurname,
                Age,
                Interests,
                City
            FROM Users
            WHERE NormalizedUserName = @NormalizedUserName";
    }
}