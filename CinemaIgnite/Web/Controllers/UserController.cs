using AutoMapper;
using Core.Services.Contracts;
using Core.ViewModels.User;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public UserController(IMapper mapper, IUserService userService)
        {

            this.userService = userService;
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

            var result = await userService.Register(model);

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
            (bool isLoggedIn, string error) = await userService.Login(model);

            if (!isLoggedIn)
            {
                return View("UserError", error);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
