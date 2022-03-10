using Core.Services.Contracts;
using Core.ViewModels.Movie;
using Core.ViewModels.Projection;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ProjectionController : Controller
    {
        private readonly IProjectionService projectionService;

        //for test purposes
        private readonly IMovieService movieService;

        public ProjectionController(IProjectionService projectionService, IMovieService movieService)
        {
            this.projectionService = projectionService;
            this.movieService = movieService;
        }

        public async Task<IActionResult> Create()
        {
            IEnumerable<ListMovieModel> movies = await movieService.GetAll();
            ViewBag.Movies = movies;

            return View();
        }

        public async Task<IActionResult> All(DateTime date)
        {
            date = DateTime.Today;
            IEnumerable<ListProjectionModel> projections = await projectionService.GetAllForDate(date);

            return View(projections);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectionModel model)
        {
            string error = string.Empty;

            if (!ModelState.IsValid)
            {
                IEnumerable<ListMovieModel> movies = await movieService.GetAll();
                ViewBag.Movies = movies;

                return View();
            }

            try
            {
                (bool isCreated, error) = await projectionService.Create(model);

                if (!isCreated)
                {
                    throw new InvalidOperationException();
                }
            }
            catch (Exception)
            {
                return View("UserError", error);
            }

            return RedirectToAction(nameof(All));
        }
    }
}
