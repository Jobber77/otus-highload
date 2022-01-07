using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SocialNetwork.Core.Users;
using SocialNetwork.Core.Users.Friends;

namespace SocialNetwork.Pages.Persons
{
    public class Index : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IUsersService _usersService;
        private readonly IUserFriendsService _userFriendsService;

        [BindProperty]
        public long Id { get; set; }
        
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Display(Name = "Age")]
        public int? Age { get; set; }

        [Display(Name = "Interests")]
        public string Interests { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        public bool IsFriend { get; set; }

        public string Error { get; set; }

        public Index(UserManager<User> userManager, IUsersService usersService, 
            IUserFriendsService userFriendsService)
        {
            _userManager = userManager;
            _usersService = usersService;
            _userFriendsService = userFriendsService;
        }
        
        public async Task<IActionResult> OnGet(long id, CancellationToken token)
        {
            var user = await _usersService.GetUser(id, token);
            if (user is null)
                return NotFound();
            var currentUser = await _userManager.GetUserAsync(User);
            var isFriend = await _userFriendsService.IsFriend(currentUser, user.Id, token);
            FillPageData(user, isFriend);
            return Page();
        }
        
        public async Task<IActionResult> OnPostAddFriend(CancellationToken token)
        {
            var user = await _usersService.GetUser(Id, token);
            if (user is null)
                return NotFound();
            var currentUser = await _userManager.GetUserAsync(User);
            var result = await _userFriendsService.AddFriend(currentUser, user.Id, token);
            FillPageData(user, result.IsSuccess, result.Errors.FirstOrDefault()?.Message);
            return Page();
        }
        
        public async Task<IActionResult> OnPostRemoveFriend(CancellationToken token)
        {
            var user = await _usersService.GetUser(Id, token);
            if (user is null)
                return NotFound();
            var currentUser = await _userManager.GetUserAsync(User);
            await _userFriendsService.DeleteFriend(currentUser, user.Id, token);
            return Page();
        }

        private void FillPageData(User user, bool isFriend, string error = null)
        {
            Id = user.Id;
            Name = user.Name;
            Surname = user.Surname;
            Age = user.Age;
            Interests = user.Interests;
            City = user.City;
            IsFriend = isFriend;
            Error = error is not null ? $"Error: {error}" : null;
        }
    }
}