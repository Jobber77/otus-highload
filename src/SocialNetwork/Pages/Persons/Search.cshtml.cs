using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SocialNetwork.Core.Users;
using SocialNetwork.Pages.Shared.Models;

namespace SocialNetwork.Pages.Persons
{
    public class Search : PageModel
    {
        private readonly IUsersService _usersService;
        
        [BindProperty(SupportsGet = true)]
        [MinLength(3)]
        public string Name { get; set; }
        
        [BindProperty(SupportsGet = true)]
        [MinLength(3)]
        public string Surname { get; set; }

        public string SearchResultMessage { get; set; } = "Type something to search for people";
        public List<Person> SearchResults { get; set; } = new();

        public Search(IUsersService usersService)
        {
            _usersService = usersService;
        }
        
        public async Task OnGet(CancellationToken token)
        {
            if (string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Surname))
                return;
            
            if (!ModelState.IsValid)
                return;

            var users = await _usersService.SearchUsers(Name, Surname, token);

            if (!users.Any())
            {
                SearchResultMessage = "Nobody found";
                return;
            }
            
            SearchResultMessage = "People found:";
            SearchResults.AddRange(users.Select(u => new Person(u.Id, u.Name, u.Surname, u.Age)));
        }
    }
}