using CozyCorners.Core.Models.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CozyCorners.ViewModels;
using CozyCorners.Extentions;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace CozyCorners.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
      //
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Register()
        {

           
            return View();
        }


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
		{
			var user = new AppUser();
			if (!ModelState.IsValid)
			{
				

				return View();
			}
			try
			{

					user = new AppUser()
					{
						UserName = registerViewModel.UserName,
						Email = registerViewModel.Email
					};
				


				// Create the user in the system
				var createUserResult = await _userManager.CreateAsync(user, registerViewModel.Password);

				if (createUserResult.Succeeded)
				{

					return RedirectToAction(nameof(Signin));


				}
				else
				{
					// If user creation failed, add the errors to the ModelState
					foreach (var error in createUserResult.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}

			return View(registerViewModel);
		}

		public async Task<IActionResult> Signin()
        {

            ClaimsPrincipal claimsPrincipal = HttpContext.User;
            if (claimsPrincipal.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signin(LoginViewModel login)
        {
            var user = await _userManager.FindByNameAsync(login.UserName);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Email Is Invalid");
                return RedirectToAction(nameof(Signin));
            }

            //var result1 = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);

			var result = await _signInManager.PasswordSignInAsync(user.UserName, login.Password, login.RememberMe, lockoutOnFailure: false);

			if (result.Succeeded)
            {
                // Fetch the user's roles
                var roles = await _userManager.GetRolesAsync(user);

                // Create a list of claims including the role claims
                List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, login.UserName),
            new Claim(ClaimTypes.NameIdentifier,user.Id)
        };

                if (roles != null && roles.Any())
                {
                    // Add role claims to the list
                    claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
                }

                // Create claims identity with the claims
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Set authentication properties
                AuthenticationProperties authenticationProperties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = login.RememberMe
                };

                // Sign in the user
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authenticationProperties);

                
                // Redirect to the home page
                return RedirectToAction("Index", "Home");
            }

            // If login failed, show an error message
            ViewData["ValidateMessage"] = "Invalid login attempt.";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
			await _signInManager.SignOutAsync();
			var x = User.Identity.IsAuthenticated;
            return RedirectToAction("Index","Home");
        }

    }
}
