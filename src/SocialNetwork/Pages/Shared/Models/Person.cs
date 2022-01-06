namespace SocialNetwork.Pages.Shared.Models
{
    public class Person
    {
        public long Id { get; }
        public string Name { get; }
        public string Surname { get; }
        public int? Age { get; }
        
        public Person(long id, string name, string surname, int? age)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Age = age;
        }
    }
}