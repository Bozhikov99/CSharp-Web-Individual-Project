using Core.ViewModels.Genre;

namespace Core.Services.Contracts
{
    public interface IGenreService
    {
        Task Create(CreateGenreModel model);

        Task<bool> Edit(EditGenreModel model);

        Task<ListGenreModel> GetById(string id);

        Task<ICollection<ListGenreModel>> GetAll();

        Task<EditGenreModel> GetEditModel(string id);

        Task Delete(string id);
    }
}
