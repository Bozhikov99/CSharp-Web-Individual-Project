using Core.Services.Contracts;
using Core.ViewModels.Genre;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class GenreController : BaseController
    {
        private readonly IGenreService genreService;

        public GenreController(IGenreService genreService)
        {
            this.genreService = genreService; 
        }

        public async Task<IActionResult> All()
        {
            IEnumerable<ListGenreModel> genres = await genreService.GetAll();
            return View(genres);
        }
    }
}
