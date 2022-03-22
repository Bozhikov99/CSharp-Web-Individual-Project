using Core.Services.Contracts;
using Core.ViewModels.Genre;
using Microsoft.AspNetCore.Mvc;

namespace Web.Views.Shared.Components.Genres
{
    public class GenresViewComponent : ViewComponent
    {
        private readonly IGenreService genreService;

        public GenresViewComponent(IGenreService genreService)
        {
            this.genreService = genreService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<ListGenreModel> genres = await genreService.GetAll();
            return View(genres);
        }
    }
}
