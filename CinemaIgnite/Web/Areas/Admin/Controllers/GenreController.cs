using Common.ValidationConstants;
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
            EditGenreModel genre = await genreService.GetEditModel(id);
            return View(genre);
        }

        public async Task<IActionResult> All(int activePage = 0)
        {
            IEnumerable<ListGenreModel> genres = await genreService.GetAll();

            int pages = 0;

            if (genres.Count() <= 15)
            {
                pages++;
            }
            else
            {
                pages = genres.Count() / 15;

                if (genres.Count() % 15 != 0)
                {
                    pages++;
                }
            }

            ViewBag.PagesCount = pages;
            ViewBag.PageLimit = 15;
            ViewBag.ActivePage = activePage;
            ViewBag.Controller = "Genre";
            ViewBag.Action = "All";

            return View(genres);
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await genreService.Delete(id);
            }
            catch (ArgumentException ae)
            {
                return View("UserError", ae.Message);
            }
            catch (Exception)
            {
                return View("UserError", ErrorMessagesConstants.ErrorDeletingGenre);
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

            try
            {
                await genreService.Create(model);
            }
            catch (ArgumentException ae)
            {
                return View("UserError", ae.Message);
            }
            catch (Exception)
            {
                return View("UserError", ErrorMessagesConstants.ErrorCreatingGenre);
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
            catch (ArgumentException ae)
            {
                return View("UserError", ae.Message);
            }
            catch (Exception)
            {
                return View("UserError", ErrorMessagesConstants.ErrorEditingGenre);
            }

            return RedirectToAction(nameof(All));
        }
    }
}
