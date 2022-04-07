using Core.ViewModels.Movie;
using Core.ViewModels.Projection;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Contracts
{
    public interface ITicketService
    {
        Task BuyTickets(int[] seats, string projectionId, string userId);

        Task<IEnumerable<int>> GetTakenSeats(string projectionId);

        Task<(ListMovieModel movie, ListProjectionModel projection)> GetInfo(string id);
    }
}
