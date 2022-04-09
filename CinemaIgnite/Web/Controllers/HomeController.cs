using Core.Services.Contracts;
using Core.ViewModels;
using Core.ViewModels.Movie;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using System.Diagnostics;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMovieService movieService;
        private readonly IHtmlLocalizer<HomeController> localizer;

        public HomeController(
            IMovieService movieService, 
            ILogger<HomeController> logger, 
            IHtmlLocalizer<HomeController> localizer)
        {
            this.localizer = localizer;
            this.movieService = movieService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<ListMovieModel> movies = await movieService.GetAll();
            movies = movies.OrderByDescending(m => m.Rating)
                .Take(3);

            var popular = localizer["Popular"];
            var year = localizer["Year"];
            var duration = localizer["Duration"];
            var genre = localizer["Genre"];
            var min = localizer["Min"];

            ViewData["Popular"] = popular;
            ViewData["Year"] = year;
            ViewData["Duration"] = duration;
            ViewData["Genre"] = genre;
            ViewData["Min"] = min;

            return View(movies);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}