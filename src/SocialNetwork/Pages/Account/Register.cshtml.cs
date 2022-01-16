using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SocialNetwork.Core;
using SocialNetwork.Core.Users;

namespace SocialNetwork.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                MinimumLength = 3)]
            [Display(Name = "UserName")]
            public string UserName { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                MinimumLength = 3)]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                MinimumLength = 3)]
            [Display(Name = "Surname")]
            public string Surname { get; set; }

            [Range(1, 100, ErrorMessage = "The {0} must be between {1} and {2}.")]
            [Display(Name = "Age")]
            public int Age { get; set; }

            [StringLength(600, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                MinimumLength = 2)]
            [Display(Name = "Interests")]
            public string Interests { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                MinimumLength = 2)]
            [Display(Name = "City")]
            public string City { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
        
        private readonly IUsersService _usersService;
        private readonly SignInManager<User> _signInManager;

        public RegisterModel(IUsersService usersService, SignInManager<User> signInManager)
        {
            _usersService = usersService;
            _signInManager = signInManager;
        }

        public Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            return Task.CompletedTask;
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken token, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            
            if (!ModelState.IsValid) 
                return Page();
            
            var user = CreateUser();
            var result = await _usersService.CreateUser(user, Input.Password, token);
            if (result.IsSuccess)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Message);
            
            // If we got this far, something failed, redisplay form
            return Page();
        }
        
        private User CreateUser()
        {
            return new User(Input.UserName, Input.Name, Input.Surname, Input.City, Input.Age, Input.Interests);
        }
        
    }
}