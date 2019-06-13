using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BankProject.Models;
using Microsoft.AspNetCore.Identity;

namespace BankProject.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<AppUser> userManager;  // elementy Identity do kont
        private SignInManager<AppUser> signInManager;

        public AccountController(UserManager<AppUser> userMgr,
                SignInManager<AppUser> signinMgr)
        {
            userManager = userMgr;
            signInManager = signinMgr;  // Wstrzykniecie zaleznosci
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel details,
                string returnUrl)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByEmailAsync(details.Email);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result =
                            await signInManager.PasswordSignInAsync(
                                user, details.Password, false, false);
                    if (result.Succeeded)
                    {
                        return Redirect(returnUrl ?? "/");
                    }
                }

                ModelState.AddModelError(nameof(LoginModel.Email),
                "Nieprawidłowa nazwa użytkownika lub hasło.");
            }
            return View(details);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}