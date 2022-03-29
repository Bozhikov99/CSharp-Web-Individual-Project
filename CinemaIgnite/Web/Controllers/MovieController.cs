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

        public async Task<IActionResult> All(int page = 0)
        {
            IEnumerable<ListMovieModel> movies = await movieService.GetAll();
            IEnumerable<ListGenreModel> genres = await genreService.GetAll();
            ViewBag.Genres = genres;

            int pages = 0;

            if (movies.Count() <= 5)
            {
                pages++;
            }
            else
            {
                pages = movies.Count() / 5;

                if (movies.Count() % 5 != 0)
                {
                    pages++;
                }
            }

            ViewBag.PagesCount = pages;
            ViewBag.PageLimit = 5;
            ViewBag.ActivePage = page;

            return View(movies);
        }

        public async Task<IActionResult> SearchByGenre(List<string> genresSearch)
        {
            IEnumerable<ListMovieModel> movies = await movieService.GetAll(m => m.Genres.Any(g => genresSearch.Contains(g.Id)));
            IEnumerable<ListGenreModel> genres = await genreService.GetAll();
            ViewBag.Genres = genres;

            return View(nameof(All), movies);
        }

        public async Task<IActionResult> Search(string search)
        {
            IEnumerable<ListMovieModel> movies = await movieService.GetAll(m => m.Title.ToLower()
                .Contains(search.ToLower()) ||
                m.Actors.ToLower()
                .Contains(search.ToLower()) ||
                m.Genres.Select(g => g.Name.ToLower())
                .Any(g => g.Contains(search.ToLower()))
            );

            IEnumerable<ListGenreModel> genres = await genreService.GetAll();
            ViewBag.Genres = genres;

            return View(nameof(All), movies);
        }

        public async Task<IActionResult> Details(string id)
        {
            MovieDetailsModel model = await movieService.GetMovieDetails(id);
            bool isLoggedIn = userService.IsLoggedIn();

            if (isLoggedIn)
            {
                bool isFavourite = userService.HasFavouriteMovie(id);
                (bool hasRating, int? value) = userService.GetRating(id);

                ViewBag.UserId = userService.GetUserId();
                ViewBag.IsFavourite = isFavourite;
                ViewBag.HasRating = hasRating;

                if (hasRating)
                {
                    ViewBag.Rating = value;
                }
            }

            ViewBag.MovieId = id;
            ViewBag.IsLoggedIn = isLoggedIn;

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

        [Authorize]
        [HttpPost]
        public async Task<PartialViewResult> RateMovie(string movieId, int value)
        {
            bool isSuccessful = await userService.RateMovie(movieId, value);
            ViewBag.HasRating = true;
            ViewBag.Rating = value;
            return PartialView("_RateMoviePartial");
        }
    }
}
