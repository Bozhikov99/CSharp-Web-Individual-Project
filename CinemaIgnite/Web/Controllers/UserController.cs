using AutoMapper;
using Common;
using Core.Services.Contracts;
using Core.ViewModels.Movie;
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
        private readonly RoleManager<IdentityRole> roleManager;

        public UserController(RoleManager<IdentityRole> roleManager, IUserService userService)
        {
            this.userService = userService;
            this.roleManager = roleManager;
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
        public async Task<IActionResult> Profile(int activePage = 0)
        {
            UserProfileModel model = await userService.GetUserProfile();
            IEnumerable<ListTicketModel> tickets = await userService.GetUpcomingTickets();
            IEnumerable<ListMovieModel> favouriteMovies = await userService.GetFavouriteMovies();
            ViewBag.Tickets = tickets;
            ViewBag.Favourites = favouriteMovies;

            int favPages = 0;

            if (favouriteMovies.Count() <= 5)
            {
                favPages++;
            }
            else
            {
                favPages = favouriteMovies.Count() / 5;

                if (favouriteMovies.Count() % 5 != 0)
                {
                    favPages++;
                }
            }


            ViewBag.PagesCount = favPages;
            ViewBag.PageLimit = 5;
            ViewBag.ActivePage = activePage;
            ViewBag.Controller = "User";
            ViewBag.Action = "Profile";

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
