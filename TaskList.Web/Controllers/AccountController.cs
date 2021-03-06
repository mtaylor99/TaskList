﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskList.DAL.Models;

namespace TaskList.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<AspNetUser> userManager;
        private SignInManager<AspNetUser> signInManager;

        public AccountController(UserManager<AspNetUser> userMgr, SignInManager<AspNetUser> signInMgr)
        {
            userManager = userMgr;
            signInManager = signInMgr;
        }

        [AllowAnonymous]
        public ViewResult Login(string returnUrl)
        {
            return View(new LoginModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                AspNetUser user = await userManager.FindByNameAsync(loginModel.Name);

                if (user != null)
                {
                    await signInManager.SignOutAsync();

                    if ((await signInManager.PasswordSignInAsync(user, loginModel.Password, false, false)).Succeeded)
                    {
                        if ((loginModel?.ReturnUrl != null) && (loginModel.ReturnUrl != "/"))
                        {
                            return Redirect(loginModel.ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("List", "Tasks", new { area = "Project" });
                        }
                    }
                }
            }

            ModelState.AddModelError("", "Invalid name or password");

            return View(loginModel);
        }

        public async Task<RedirectResult> Logout(string returnUrl = "/Account/Login")
        {
            await signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
