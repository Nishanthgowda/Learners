using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TechTree.Data;
using TechTree.Models;

namespace TechTree.Controllers
{
    public class UserAuth : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public UserAuth(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]                    //if we use attribute then this method publicly accessible and we dont need special authorization to access it
        [HttpPost]
        [ValidateAntiForgeryToken]          //this filter is added to avoid cross site request forgery
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            loginModel.LoginInValid = "true";

            if(ModelState.IsValid) 
            {
                var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password,loginModel.RememberMe,lockoutOnFailure:false);
                if (result.Succeeded) 
                {
                    loginModel.LoginInValid = "";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt");
                }
            }

            return PartialView("_UserLoginPartial",loginModel);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string url=null)
        {
            await _signInManager.SignOutAsync();
            if(url!=null)
            {
                return LocalRedirect(url);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
