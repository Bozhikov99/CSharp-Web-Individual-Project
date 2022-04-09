using Common;
using Common.ValidationConstants;
using Core.Services.Contracts;
using Core.ViewModels.Movie;
using Core.ViewModels.Projection;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    public class ProjectionController : BaseController
    {
        private readonly IProjectionService projectionService;
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
            ViewBag.TicketsAvailable = ProjectionConstants.TicketsAvailable;

            return View();
        }

        public async Task<IActionResult> All(DateTime date)
        {
            IEnumerable<ListProjectionModel> projections = await projectionService.GetAllForDate(date);

            IEnumerable<ListMovieModel> movies = await movieService
                .GetAll(m => m.Projections.Any(p => p.Date.Year == date.Year &&
                p.Date.Month == date.Month &&
                p.Date.Day == date.Day));

            ViewBag.Movies = movies;
            ViewBag.Date = date;

            return View(projections);
        }

        public async Task<IActionResult> Delete(string id)
        {
            DateTime date;

            try
            {
                date = await projectionService.Delete(id);
            }
            catch (Exception)
            {
                return View("UserError", ErrorMessagesConstants.ErrorDeletingProjection);
            }

            string dateString = date.ToString("yyyy-MM-dd");
            string url = $"https://localhost:44395/Projection/All?date={dateString}";

            return Redirect(url);       //I know this is gross, yet I did not think of another way...
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProjectionModel model)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ListMovieModel> movies = await movieService.GetAll();
                ViewBag.Movies = movies;

                return View();
            }

            (bool isCreated, DateTime date) = await projectionService.Create(model);

            if (!isCreated)
            {
                return View("UserError", ErrorMessagesConstants.ErrorCreatingProjection);
            }

            string dateString = date.ToString("yyyy-MM-dd");
            string url = $"https://localhost:44395/Projection/All?date={dateString}";

            return Redirect(url);
        }
    }
}
