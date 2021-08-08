using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAdvert.web.Models.Accounts;

namespace WebAdvert.web.Controllers
{
    public class Accounts : Controller
    {
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly UserManager<CognitoUser> _userManager;
        private readonly CognitoUserPool _pool;
        public Accounts(SignInManager<CognitoUser> signInManager, UserManager<CognitoUser> userManager,CognitoUserPool pool )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _pool = pool;
        }
        public async Task<IActionResult> Signup()
        {
            var model = new SignupVM();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupVM model)
        {
            if (ModelState.IsValid)
            {
                var user = _pool.GetUser(model.Email);
                if (user.Status != null)
                {
                    ModelState.AddModelError("UserExsists", "User email already exsista");
                    return View(model);
                }

                //user.Attributes.Add(CognitoAttributesConstants)
               
                var createdUser = await _userManager.CreateAsync(user, model.Password);

                if (createdUser.Succeeded)
                {
                    return RedirectToAction("Confirm");
                }

            }
            return View();
        }

        public async Task<IActionResult> Confirm()
        {
            var model = new ConfirmVM();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(ConfirmVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("Not Found", "A user with the given email address was not found");
                return View(model);
            }

            var result = await (_userManager as CognitoUserManager<CognitoUser>).ConfirmSignUpAsync(user, model.Code, false).ConfigureAwait(false);
            if (result.Succeeded)
            {
                return RedirectToAction("Ïndex", "Home");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                }
                return View(model);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Login(LoginModel model)
        {
            return View(model);
        }

        [HttpPost]
        [ActionName("Login")]
        public async Task<IActionResult> LoginPost(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password,
                    model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("LoginError", "Email and password not match");
                }
            }

            return View(model);
        }

    }
}
