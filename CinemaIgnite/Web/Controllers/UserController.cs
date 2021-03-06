using AutoMapper;
using Common;
using Common.ValidationConstants;
using Core.Services.Contracts;
using Core.ViewModels.Movie;
using Core.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;

namespace Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService userService;
        private readonly IHtmlLocalizer<UserController> localizer;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserController(
            RoleManager<IdentityRole> roleManager,
            IUserService userService,
            IHtmlLocalizer<UserController> localizer)
        {
            this.userService = userService;
            this.roleManager = roleManager;
            this.localizer = localizer;
        }

        public IActionResult Register()
        {
            var register = localizer["Register"];
            var password = localizer["Password"];
            var email = localizer["Email"];
            var name = localizer["FirstName"];
            var lastName = localizer["LastName"];
            var confirmPassword = localizer["ConfirmPassword"];

            ViewData["Email"] = email;
            ViewData["Password"] = password;
            ViewData["Register"] = register;
            ViewData["FirstName"] = name;
            ViewData["LastName"] = lastName;
            ViewData["ConfirmPassword"] = confirmPassword;

            return View();
        }

        public IActionResult Login()
        {
            var login = localizer["Login"];
            var password = localizer["Password"];
            var email = localizer["Email"];

            ViewData["Email"] = email;
            ViewData["Password"] = password;
            ViewData["Login"] = login;

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Profile(int activePage = 0)
        {
            UserProfileModel model = await userService.GetUserProfile();
            IEnumerable<ListMovieModel> favouriteMovies = await userService.GetFavouriteMovies();
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

            var profileHeadline = localizer["ProfileHeadline"];
            var favMovies = localizer["FavouriteMovies"];
            var year = localizer["Year"];
            var genre = localizer["Genre"];
            var min = localizer["Min"];
            var duration = localizer["Duration"];

            ViewData["ProfileHeadline"] = profileHeadline;
            ViewData["FavouriteMovies"] = favMovies;
            ViewData["Year"] = year;
            ViewData["Genre"] = genre;
            ViewData["Duration"] = duration;
            ViewData["Min"] = min;

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
                return View("UserError", ErrorMessagesConstants.ErrorRegistering);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserModel model)
        {
            try
            {
                await userService.Login(model);
            }
            catch (ArgumentException ae)
            {
                return View("UserError", ae.Message);
            }
            catch (Exception)
            {
                return View("UserError", ErrorMessagesConstants.ErrorLoggingIn);
            }


            return RedirectToAction("Index", "Home");
        }


    }
}
