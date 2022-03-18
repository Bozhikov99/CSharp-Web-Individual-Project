using Core.Services.Contracts;
using Core.ViewModels.Genre;
using Core.ViewModels.Movie;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class MovieController : BaseController
    {
        private readonly IMovieService movieService;
        private readonly IUserService userService;
        private readonly IGenreService genreService;

        public MovieController(IMovieService movieService, IGenreService genreService, IUserService userService)
        {
            this.movieService = movieService;
            this.genreService = genreService;
            this.userService = userService;
        }

        public async Task<IActionResult> Create()
        {
            IEnumerable<ListGenreModel> genres = await genreService.GetAll();
            ViewBag.Genres = genres;
            return View();
        }

        public async Task<IActionResult> All()
        {
            IEnumerable<ListMovieModel> movies = await movieService.GetAll();
            return View(movies);
        }

        public async Task<IActionResult> Edit(string id)
        {
            EditMovieModel model = await movieService.GetEditModel(id);
            IEnumerable<ListGenreModel> genres = await genreService.GetAll();
            ViewBag.Genres = genres;

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<PartialViewResult> AddMovieToFavourites(string id)
        {
            await userService.AddMovieToFavourites(id);
            ViewBag.IsFavourite = true;
            ViewBag.IsLoggedIn = true;

            return PartialView("_FavouriteMoviePartial");
        }

        [Authorize]
        [HttpPost]
        public async Task<PartialViewResult> RemoveMovieFromFavourites(string id)
        {
            await userService.RemoveMovieFromFavourites(id);
            ViewBag.IsFavourite = false;
            ViewBag.IsLoggedIn = true;

            return PartialView("_FavouriteMoviePartial");
        }

        public async Task<IActionResult> Delete(string id)
        {
            bool isDeleted = await movieService.Delete(id);

            if (!isDeleted)
            {
                string error = "Error deleting a movie";
                return View("UserError", error);
            }

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Details(string id)
        {
            MovieDetailsModel model = await movieService.GetMovieDetails(id);
            bool isLoggedIn = userService.IsLoggedIn();

            if (isLoggedIn)
            {
                bool isFavourite = await userService.HasFavouriteMovie(id);

                ViewBag.UserId = userService.GetUserId();
                ViewBag.IsFavourite = isFavourite;
            }

            ViewBag.MovieId = id;
            ViewBag.IsLoggedIn = isLoggedIn;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMovieModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            bool isCreated = await movieService.Create(model);

            if (!isCreated)
            {
                string error = "Error creating a movie";
                return View("UserError", error);
            }

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditMovieModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            bool isEdited = await movieService.Edit(model);

            if (!isEdited)
            {
                string error = "Error editing a movie";
                return View("UserError", error);
            }

            return RedirectToAction(nameof(All));
        }
    }
}
