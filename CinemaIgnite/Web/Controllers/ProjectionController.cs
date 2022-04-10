using Core.Services.Contracts;
using Core.ViewModels.Movie;
using Core.ViewModels.Projection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using System.Globalization;

namespace Web.Controllers
{
    public class ProjectionController : BaseController
    {
        private readonly IProjectionService projectionService;
        private readonly IMovieService movieService;
        private readonly IHtmlLocalizer<ProjectionController> localizer;

        public ProjectionController(
            IProjectionService projectionService,
            IMovieService movieService,
            IHtmlLocalizer<ProjectionController> localizer)
        {
            this.projectionService = projectionService;
            this.movieService = movieService;
            this.localizer = localizer;
        }

        public async Task<IActionResult> All(DateTime date, int activePage = 0)
        {
            IEnumerable<ListProjectionModel> projections = await projectionService.GetAllForDate(date);

            IEnumerable<ListMovieModel> movies = await movieService
                .GetAll(m => m.Projections.Any(p => p.Date.Year == date.Year &&
                p.Date.Month == date.Month &&
                p.Date.Day == date.Day));

            ViewBag.Movies = movies;
            ViewBag.Date = date;

            int pages = 0;

            if (movies.Count() <= 2)
            {
                pages++;
            }
            else
            {
                pages = movies.Count() / 2;

                if (movies.Count() % 2 != 0)
                {
                    pages++;
                }
            }

            ViewBag.PagesCount = pages;
            ViewBag.PageLimit = 2;
            ViewBag.ActivePage = activePage;
            ViewBag.Controller = "Projection";
            ViewBag.Action = "All";

            var projectionsHeadline = localizer["ProjectionsHeadline"];
            var projectionsToday = localizer["ProjectionsToday"];
            var minutes = localizer["Minutes"];
            var levs = localizer["Levs"];

            ViewData["ProjectionsHeadline"] = projectionsHeadline;
            ViewData["ProjectionsToday"] = projectionsToday;
            ViewData["Minutes"] = minutes;
            ViewData["Levs"] = levs;

            return View(projections);
        }

    }
}
