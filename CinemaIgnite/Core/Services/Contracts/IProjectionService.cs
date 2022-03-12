using Core.ViewModels.Projection;

namespace Core.Services.Contracts
{
    public interface IProjectionService
    {
        Task<(bool isCreated, DateTime date)> Create(CreateProjectionModel model);

        Task<(bool isDeleted, DateTime date)> Delete(string id);

        Task<IEnumerable<ListProjectionModel>> GetAllForMovie(string movieId);

        Task<IEnumerable<ListProjectionModel>> GetAllForDate(DateTime date);

        Task<ProjectionDetails> GetProjectionDetails(string id);

        Task<ProjectionDetails> GetProjectionById(string id);

        bool IsPlaceTaken(int seat);

        Task<bool> DeleteAllForMovie(string movieId);
    }
}