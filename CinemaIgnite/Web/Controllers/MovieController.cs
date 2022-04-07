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

        public async Task<IActionResult> All(int activePage = 0, List<string> genresSearch = null, string search = null)
        {
            IEnumerable<ListMovieModel> movies;

            if (search == null)
            {

                if (genresSearch.Count == 0)
                {
                    movies = await movieService.GetAll();
                }
                else
                {
                    movies = await movieService.GetAll(m => m.Genres.Any(g => genresSearch.Contains(g.Id)));
                }
            }
            else
            {
                movies = await movieService.GetAll(m => m.Title.ToLower()
                .Contains(search.ToLower()) ||
                m.Actors.ToLower()
                .Contains(search.ToLower()) ||
                m.Genres.Select(g => g.Name.ToLower())
                .Any(g => g.Contains(search.ToLower())));
            }

            IEnumerable<ListGenreModel> genres = await genreService.GetAll();
            ViewBag.Genres = genres;

            int pages = 0;

            if (movies.Count() <= 10)
            {
                pages++;
            }
            else
            {
                pages = movies.Count() / 10;

                if (movies.Count() % 10 != 0)
                {
                    pages++;
                }
            }

            ViewBag.PagesCount = pages;
            ViewBag.PageLimit = 10;
            ViewBag.ActivePage = activePage;
            ViewBag.Controller = "Movie";
            ViewBag.Action = "All";
            ViewBag.Search= search;

            return View(movies);
        }

        public async Task<IActionResult> Details(string id)
        {
            MovieDetailsModel model = await movieService.GetMovieDetails(id);
            string userId = userService.GetUserId();

            if (userId!=null)
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
            ViewBag.IsLoggedIn = userId != null;

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
