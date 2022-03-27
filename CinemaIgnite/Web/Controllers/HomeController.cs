using Core.Services.Contracts;
using Core.ViewModels;
using Core.ViewModels.Movie;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMovieService movieService;

        public HomeController(IMovieService movieService, ILogger<HomeController> logger)
        {
            this.movieService = movieService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<ListMovieModel> movies = await movieService.GetAll();
            movies = movies.OrderByDescending(m => m.Rating)
                .Take(3);

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