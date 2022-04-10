using Common.ValidationConstants;
using Core.Services.Contracts;
using Core.ViewModels.Genre;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;

namespace Web.Areas.Admin.Controllers
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

        public async Task<IActionResult> Create()
        {
            var createPageTitle = localizer["CreatePageTitle"];
            var namePlaceholder = localizer["NamePlaceholder"];

            ViewData["CreatePageTitle"] = createPageTitle;
            ViewData["NamePlaceholder"] = namePlaceholder;

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

            var genresAllTitle = localizer["GenresAllTitle"];
            var name = localizer["Name"];
            var moviesCount = localizer["MoviesCount"];

            ViewData["GenresAllTitle"] = genresAllTitle;
            ViewData["Name"] = name;
            ViewData["MoviesCount"] = moviesCount;

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
