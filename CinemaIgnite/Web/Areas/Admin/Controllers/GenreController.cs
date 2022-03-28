using Core.Services.Contracts;
using Core.ViewModels.Genre;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    public class GenreController : BaseController
    {
        private readonly IGenreService genreService;

        public GenreController(IGenreService genreService)
        {
            this.genreService = genreService;
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            ListGenreModel genre = await genreService.GetById(id);
            return View(genre);
        }

        public async Task<IActionResult> All()
        {
            IEnumerable<ListGenreModel> genres = await genreService.GetAll();
            return View(genres);
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await genreService.Delete(id);
            }
            catch (Exception)
            {
                string error = "Error deleting genre";
                return View("UserError", error);
            }

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGenreModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            bool isCreated = await genreService.Create(model);

            if (!isCreated)
            {
                string error = "Error creating a genre";
                return View("UserError", error);
            }

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditGenreModel model)
        {
            try
            {
                await genreService.Edit(model);
            }
            catch (Exception)
            {
                string error = "Error editing genre";
                return View("UserError", error);
            }

            return RedirectToAction(nameof(All));
        }
    }
}
