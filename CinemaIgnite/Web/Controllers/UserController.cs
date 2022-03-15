using AutoMapper;
using Core.ViewModels.User;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IMapper mapper;

        public UserController(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            User user = mapper.Map<User>(model);
            user.UserName = model.Email;

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors
                    .Select(e => e.Description);

                foreach (string e in errors)
                {
                    ModelState.AddModelError(string.Empty, e);
                }

                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserModel model)
        {
            string email = model.Email;

            User? user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                //ModelState.AddModelError(string.Empty, "Invalid data");

                string error = "Invalid data";
                return View("UserError", error);
            }

            bool isValidPassword = await userManager.CheckPasswordAsync(user, model.Password);

            if (!isValidPassword)
            {
                string error = "Invalid data";
                return View("UserError", error);
            }

            await signInManager.SignInAsync(user, true);

            return RedirectToAction("Index", "Home");
        }
    }
}
