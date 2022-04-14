using Core.Services.Contracts;
using Core.ViewModels.Genre;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;

namespace Web.Controllers
{
    public class GenreController : BaseController
    {
        private readonly IGenreService genreService;
        private readonly IHtmlLocalizer<GenreController> localizer;

        public GenreController(IGenreService genreService, IHtmlLocalizer<GenreController> localizer) 
        {
            this.genreService = genreService;
            this.localizer = localizer;
        }

        public async Task<IActionResult> All()
        {
            IEnumerable<ListGenreModel> genres = await genreService.GetAll();
            return View(genres);
        }
    }
}
