using Core.ViewModels.Movie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Contracts
{
    public interface IMovieService
    {
        Task<bool> Create(CreateMovieModel model);

        Task<bool> Edit(EditMovieModel model);

        Task<bool> Delete(string id);

        Task<IEnumerable<ListMovieModel>> GetAll();

        //Task<MovieDetailsModel> GetById(string id);
    }
}
