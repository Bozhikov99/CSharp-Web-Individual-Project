using Core.Services.Contracts;
using Core.ViewModels.Movie;
using Core.ViewModels.Projection;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
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

        public async Task<IActionResult> All(DateTime date)
        {
            IEnumerable<ListProjectionModel> projections = await projectionService.GetAllForDate(date);

            IEnumerable<ListMovieModel> movies = await movieService
                .GetAll(m => m.Projections.Any(p => p.Date.Year == date.Year &&
                p.Date.Month == date.Month &&
                p.Date.Day == date.Day));

            ViewBag.Movies = movies;

            return View(projections);
        }

    }
}
