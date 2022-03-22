using AutoMapper;
using Core.Services.Contracts;
using Core.ViewModels.Ticket;
using Core.ViewModels.User;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            UserProfileModel model = await userService.GetUserProfile();
            IEnumerable<ListTicketModel> tickets = await userService.GetUpcomingTickets();
            ViewBag.Tickets = tickets;

            return View(model);
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
