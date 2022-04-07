using Core.ViewModels.Movie;
using Infrastructure.Models;
using System.Linq.Expressions;

namespace Core.Services.Contracts
{
    public interface IMovieService
    {
        Task Create(CreateMovieModel model);

        Task Edit(EditMovieModel model);

        Task Delete(string id);

        Task<IEnumerable<ListMovieModel>> GetAll();

        Task<IEnumerable<ListMovieModel>> GetAll(Expression<Func<Movie, bool>> search);

        Task<EditMovieModel> GetEditModel(string id);

        Task<MovieDetailsModel> GetMovieDetails(string id);

        Task<string> GetRating(string id);

        //Task<MovieDetailsModel> GetById(string id);
    }
}