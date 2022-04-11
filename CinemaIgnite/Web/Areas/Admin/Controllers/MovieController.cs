using Common.ValidationConstants;
using Core.Services.Contracts;
using Core.ViewModels.Genre;
using Core.ViewModels.Movie;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;

namespace Web.Areas.Admin.Controllers
{
    public class MovieController : BaseController
    {
        private readonly IMovieService movieService;
        private readonly IUserService userService;
        private readonly IGenreService genreService;
        private readonly IHtmlLocalizer<MovieController> localizer;

        public MovieController(
            IMovieService movieService,
            IGenreService genreService,
            IUserService userService,
            IHtmlLocalizer<MovieController> localizer)
        {
            this.movieService = movieService;
            this.genreService = genreService;
            this.userService = userService;
            this.localizer = localizer;
        }

        public async Task<IActionResult> Create()
        {
            IEnumerable<ListGenreModel> genres = await genreService.GetAll();
            ViewBag.Genres = genres;

            var createPageTitle = localizer["CreatePageTitle"];
            var createHeadline = localizer["CreateHeadline"];
            var headlinePlaceholder = localizer["HeadlinePlaceholder"];
            var imageUrlPlaceholder = localizer["ImageUrlPlaceholder"];
            var descriptionPlaceholder = localizer["DescriptionPlaceholder"];
            var durationPlaceholder = localizer["DurationPlaceholder"];
            var releaseYearPlaceholder = localizer["ReleaseYearPlaceholder"];
            var actorsPlaceholder = localizer["ActorsPlaceholder"];
            var directorPlaceholder = localizer["DirectorPlaceholder"];
            var countryPlaceholder = localizer["CountryPlaceholder"];
            var genreIdsPlaceholder = localizer["GenreIdsPlaceholder"];

            ViewData["CreatePageTitle"] = createPageTitle;
            ViewData["CreateHeadline"] = createHeadline;
            ViewData["HeadlinePlaceholder"] = headlinePlaceholder;
            ViewData["ImageUrlPlaceholder"] = imageUrlPlaceholder;
            ViewData["DescriptionPlaceholder"] = descriptionPlaceholder;
            ViewData["DurationPlaceholder"] = durationPlaceholder;
            ViewData["ReleaseYearPlaceholder"] = releaseYearPlaceholder;
            ViewData["ActorsPlaceholder"] = actorsPlaceholder;
            ViewData["DirectorPlaceholder"] = directorPlaceholder;
            ViewData["CountryPlaceholder"] = countryPlaceholder;
            ViewData["GenreIdsPlaceholder"] = genreIdsPlaceholder;

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

            //Pagination parameters
            ViewBag.PagesCount = pages;
            ViewBag.PageLimit = 10;
            ViewBag.ActivePage = activePage;
            ViewBag.Controller = "Movie";
            ViewBag.Action = "All";
            ViewBag.Search = search;

            //Localization parameters
            var searchPlaceholder = localizer["SearchPlaceholder"];
            var moviePageTitle = localizer["MoviePageTitle"];
            var minutes = localizer["Minutes"];
            var year = localizer["Year"];
            var duration = localizer["Duration"];
            var genre = localizer["Genre"];

            ViewData["SearchPlaceholder"] = searchPlaceholder;
            ViewData["MoviePageTitle"] = moviePageTitle;
            ViewData["Minutes"] = minutes;
            ViewData["Year"] = year;
            ViewData["Duration"] = duration;
            ViewData["Genre"] = genre;

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

            //Localization parameters
            var hour = localizer["Hour"];
            var minutes = localizer["Minutes"];
            var actorsPlaceholder = localizer["ActorsPlaceholder"];
            var directorPlaceholder = localizer["DirectorPlaceholder"];
            var countryPlaceholder = localizer["CountryPlaceholder"];

            ViewData["Hour"] = hour;
            ViewData["Minutes"] = minutes;
            ViewData["ActorsPlaceholder"] = actorsPlaceholder;
            ViewData["DirectorPlaceholder"] = directorPlaceholder;
            ViewData["CountryPlaceholder"] = countryPlaceholder;

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
