using System;

namespace SocialNetwork.Infrastructure.Users
{
    public class UserStore
    {
        public long Id { get; init; }
        public string UserName { get; init; } = string.Empty;
        public string NormalizedUserName { get; init; } = string.Empty;
        public string? PasswordHash { get; init; }
        public string Name { get; init; } = string.Empty;
        public string NormalizedName { get; init; } = string.Empty;
        public string Surname { get; init; } = string.Empty;
        public string NormalizedSurname { get; init; } = string.Empty;
        public int? Age { get; init; }
        public string? Interests { get; init; }
        public string City { get; init; } = string.Empty;
    }
}