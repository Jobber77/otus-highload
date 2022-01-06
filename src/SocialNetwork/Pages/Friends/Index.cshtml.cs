using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SocialNetwork.Core.Users;
using SocialNetwork.Core.Users.Friends;
using SocialNetwork.Pages.Shared.Models;

namespace SocialNetwork.Pages.Friends
{
    public class Index : PageModel
    {
        private readonly IUserFriendsService _friendsService;
        private readonly UserManager<User> _userManager;
        public List<Person> Friends { get; set; } = new();

        public Index(IUserFriendsService friendsService, UserManager<User> userManager)
        {
            _friendsService = friendsService;
            _userManager = userManager;
        }
        
        public async Task<IActionResult> OnGet(CancellationToken token)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            
            if (currentUser is null)
                return BadRequest("Current user can not be found");
            
            var friends = await _friendsService.SelectFriends(currentUser, token);
            Friends.AddRange(friends.Select(u => new Person(u.Id, u.Name, u.Surname, u.Age)));
            return Page();
        }
    }
}