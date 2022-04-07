using Core.ViewModels.Projection;

namespace Core.Services.Contracts
{
    public interface IProjectionService
    {
        Task<(bool isCreated, DateTime date)> Create(CreateProjectionModel model);

        Task<(bool isDeleted, DateTime date)> Delete(string id);

        Task<IEnumerable<ListProjectionModel>> GetAllForDate(DateTime date);
    }
}