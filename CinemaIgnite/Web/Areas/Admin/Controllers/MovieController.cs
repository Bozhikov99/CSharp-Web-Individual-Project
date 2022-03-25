﻿using Core.Services.Contracts;
using Core.ViewModels.Genre;
using Core.ViewModels.Movie;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    public class MovieController : BaseController
    {
        private readonly IMovieService movieService;
        private readonly IUserService userService;
        private readonly IGenreService genreService;

        public MovieController(IMovieService movieService, IGenreService genreService, IUserService userService)
        {
            this.movieService = movieService;
            this.genreService = genreService;
            this.userService = userService;
        }

        public async Task<IActionResult> Create()
        {
            IEnumerable<ListGenreModel> genres = await genreService.GetAll();
            ViewBag.Genres = genres;
            return View();
        }

        public async Task<IActionResult> All()
        {
            IEnumerable<ListMovieModel> movies = await movieService.GetAll();
            IEnumerable<ListGenreModel> genres = await genreService.GetAll();
            ViewBag.Genres = genres;

            return View(movies);
        }

        public async Task<IActionResult> Edit(string id)
        {
            EditMovieModel model = await movieService.GetEditModel(id);
            IEnumerable<ListGenreModel> genres = await genreService.GetAll();
            ViewBag.Genres = genres;

            return View(model);
        }

        public async Task<IActionResult> SearchByGenre(List<string> genresSearch)
        {
            IEnumerable<ListMovieModel> movies = await movieService.GetAll(m => m.Genres.Any(g => genresSearch.Contains(g.Id)));
            IEnumerable<ListGenreModel> genres = await genreService.GetAll();
            ViewBag.Genres = genres;

            return View(nameof(All), movies);
        }

        public async Task<IActionResult> Delete(string id)
        {
            bool isDeleted = await movieService.Delete(id);

            if (!isDeleted)
            {
                string error = "Error deleting a movie";
                return View("UserError", error);
            }

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Details(string id)
        {
            MovieDetailsModel model = await movieService.GetMovieDetails(id);
            bool isLoggedIn = userService.IsLoggedIn();

            if (isLoggedIn)
            {
                bool isFavourite = userService.HasFavouriteMovie(id);
                (bool hasRating, int? value) = userService.GetRating(id);

                ViewBag.UserId = userService.GetUserId();
                ViewBag.IsFavourite = isFavourite;
                ViewBag.HasRating = hasRating;

                if (hasRating)
                {
                    ViewBag.Rating = value;
                }
            }

            ViewBag.MovieId = id;
            ViewBag.IsLoggedIn = isLoggedIn;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMovieModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            bool isCreated = await movieService.Create(model);

            if (!isCreated)
            {
                string error = "Error creating a movie";
                return View("UserError", error);
            }

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditMovieModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            bool isEdited = await movieService.Edit(model);

            if (!isEdited)
            {
                string error = "Error editing a movie";
                return View("UserError", error);
            }

            return RedirectToAction(nameof(All));
        }
    }
}