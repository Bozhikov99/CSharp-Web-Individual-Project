using Common.ValidationConstants;
using Core.Services.Contracts;
using Core.ViewModels.Genre;
using Core.ViewModels.Movie;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
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
            ViewBag.Search = search;

            return View(movies);
        }

        public async Task<IActionResult> Edit(string id)
        {
            EditMovieModel model = await movieService.GetEditModel(id);
            IEnumerable<ListGenreModel> genres = await genreService.GetAll();
            ViewBag.Genres = genres;

            return View(model);
        }

        public async Task<IActionResult> SearchByGenre(List<string> genresSearch)
        {
            IEnumerable<ListMovieModel> movies = await movieService.GetAll(m => m.Genres.Any(g => genresSearch.Contains(g.Id)));
            IEnumerable<ListGenreModel> genres = await genreService.GetAll();
            ViewBag.Genres = genres;

            return View(nameof(All), movies);
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await movieService.Delete(id);
            }
            catch (ArgumentException ae)
            {
                return View("UserError", ErrorMessagesConstants.MovieDoesNotExist);
            }
            catch (Exception)
            {

                return View("UserError", ErrorMessagesConstants.ErrorDeletingMovie);
            }

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Details(string id)
        {
            MovieDetailsModel model = await movieService.GetMovieDetails(id);
            string userId = userService.GetUserId();

            ViewBag.MovieId = id;
            ViewBag.IsLoggedIn = userId != null;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMovieModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                await movieService.Create(model);
            }
            catch (ArgumentException ae)
            {
                string error = string.Format(ae.Message, model.Title);
                return View("UserError", ae.Message);
            }
            catch (Exception)
            {
                return View("UserError", ErrorMessagesConstants.ErrorCreatingMovie);
            }


            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditMovieModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                await movieService.Edit(model);
            }
            catch (Exception)
            {
                return View("UserError", ErrorMessagesConstants.ErrorEditingMovie);
            }

            return RedirectToAction(nameof(All));
        }
    }
}
