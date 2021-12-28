using System;

namespace SocialNetwork.Core
{
    public class User
    {
        public long Id { get; }
        public string UserName { get; }
        public string NormalizedUserName => UserName.ToUpperInvariant();
        public string? PasswordHash { get; private set; }
        public string Name { get; }
        public string NormalizedName => Name.ToUpperInvariant();
        public string Surname { get; }
        public string NormalizedSurname => Surname.ToUpperInvariant();
        public int? Age { get; }
        public string? Interests { get; }
        public string City { get; }

        private User()
        {
            UserName = string.Empty;
            Name = string.Empty;
            Surname = string.Empty;
            City = string.Empty;
        }

        public User(long id, string userName, string name, string surname, string city, int? age = null, string? interests = null, string? passwordHash = null)
            : this(userName, name, surname, city, age, interests, passwordHash)
        {
            Id = id;
            UserName = userName;
            Name = name;
            Surname = surname;
            Age = age;
            Interests = interests;
            City = city;
            PasswordHash = passwordHash;
        }
        
        public User(string userName, string name, string surname, string city, int? age = null, string? interests = null, string? passwordHash = null)
        {
            UserName = userName;
            Name = name;
            Surname = surname;
            Age = age;
            Interests = interests;
            City = city;
            PasswordHash = passwordHash;
        }

        public void UpdatePassword(string passwordHash)
        {
            PasswordHash = passwordHash;
        }
    }
}