﻿using Core.ViewModels.Genre;

namespace Core.Services.Contracts
{
    public interface IGenreService
    {
        Task<bool> Create(CreateGenreModel model);

        Task<bool> Edit(EditGenreModel model);

        Task<ListGenreModel> GetById(string id);

        Task<ICollection<ListGenreModel>> GetAll();

        Task<bool> Delete(string id);
    }
}