using Core.ViewModels.Projection;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Contracts
{
    public interface IProjectionService
    {
        Task<(bool isCreated, string error)> Create(CreateProjectionModel model);

        Task<bool> Delete(string id);

        Task<IEnumerable<ListProjectionModel>> GetAllForMovie(string movieId);

        Task<IEnumerable<ListProjectionModel>> GetAllForDate(DateTime date);

        Task<ProjectionDetails> GetProjectionDetails(string id);

        Task<ProjectionDetails> GetProjectionById(string id);

        bool IsPlaceTaken(int seat);

        Task<bool> DeleteAllForMovie(string movieId);
    }
}