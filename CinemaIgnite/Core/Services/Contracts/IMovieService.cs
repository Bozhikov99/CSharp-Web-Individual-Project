using Core.ViewModels.Movie;
using Infrastructure.Models;
using System.Linq.Expressions;

namespace Core.Services.Contracts
{
    public interface IMovieService
    {
        Task<bool> Create(CreateMovieModel model);

        Task<bool> Edit(EditMovieModel model);

        Task<bool> Delete(string id);

        Task<IEnumerable<ListMovieModel>> GetAll();

        Task<IEnumerable<ListMovieModel>> GetAll(Expression<Func<Movie, bool>> search);

        Task<EditMovieModel> GetEditModel(string id);

        Task<MovieDetailsModel> GetMovieDetails(string id);

        //Task<MovieDetailsModel> GetById(string id);
    }
}