using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SocialNetwork.Core.Users;
using SocialNetwork.Pages.Shared.Models;

namespace SocialNetwork.Pages.Persons
{
    public class Search : PageModel
    {
        private readonly IUsersService _usersService;
        public string Name { get; set; }

        public string Surname { get; set; }

        public List<Person> SearchResults { get; set; } = new();

        public Search(IUsersService usersService)
        {
            _usersService = usersService;
        }
        
        public async Task OnGet(CancellationToken token)
        {
            var users = await _usersService.SelectUsers(token, 
                Enumerable.Range(1, 20).Select(i => (long)i).ToArray());
            SearchResults.AddRange(users.Select(u => new Person(u.Id, u.Name, u.Surname, u.Age)));
        }
    }
}