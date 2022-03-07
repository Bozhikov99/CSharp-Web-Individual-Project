using Core.Services.Contracts;
using Core.ViewModels.Genre;
using Core.ViewModels.Movie;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService movieService;

        private readonly IGenreService genreService;

        public MovieController(IMovieService movieService, IGenreService genreService)
        {
            this.movieService = movieService;
            this.genreService = genreService;
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

        [HttpPost]
        public async Task<IActionResult> Create(CreateMovieModel model)
        {
            if (!ModelState.IsValid)
            {
                //IEnumerable<string> errors = ModelState.Values.SelectMany(e => e.Errors)
                //    .Select(e => e.ErrorMessage);
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
