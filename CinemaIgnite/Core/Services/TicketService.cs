using AutoMapper;
using Common.TemplateConstants;
using Core.Services.Contracts;
using Core.ViewModels.Movie;
using Core.ViewModels.Projection;
using Core.ViewModels.Ticket;
using Infrastructure.Common;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Core.Services
{
    public class TicketService : ITicketService
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;
        private const string seatTakenMessage = "Едно или повече от местата, които сте посочили вече е заето";
        private const string unexpectedError = "Неочаквана грешка";

        public TicketService(IMapper mapper, IRepository repository)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<(ListMovieModel movie, ListProjectionModel projection)> GetInfo(string id)
        {
            Projection projection = await repository.GetByIdAsync<Projection>(id);
            ListProjectionModel projectionModel = mapper.Map<ListProjectionModel>(projection);

            Movie movie = await repository.GetByIdAsync<Movie>(projection.MovieId);
            ListMovieModel listMovieModel = mapper.Map<ListMovieModel>(movie);

            return (listMovieModel, projectionModel);
        }

        public async Task<(bool isSuccessful, string error)> BuyTickets(int[] seats, string projectionId, string userId)
        {
            bool isSuccessful = false;
            string error = string.Empty;

            bool isAnyTaken = await IsAnyTaken(seats, projectionId);

            if (isAnyTaken)
            {
                error = seatTakenMessage;
                return (isSuccessful, error);
            }

            Projection projection = await repository.GetByIdAsync<Projection>(projectionId);
            User user = await repository.GetByIdAsync<User>(userId);
            List<Ticket> tickets = new List<Ticket>();

            foreach (int s in seats)
            {

                Ticket ticket = new Ticket()
                {
                    ProjectionId = projectionId,
                    Projection = projection,
                    Seat = s,
                    UserId = userId,
                    User = user
                };

                tickets.Add(ticket);
            }

            await NotifyUserOnCreation(userId, projectionId, tickets.Count);

            try
            {
                await repository.AddRangeAsync(tickets);
                projection.TicketsAvailable -= tickets.Count;
                await repository.SaveChangesAsync();
                isSuccessful = true;
            }
            catch (Exception)
            {
                error = unexpectedError;
            }

            return (isSuccessful, error);
        }

        public async Task<IEnumerable<int>> GetTakenSeats(string projectionId)
        {
            IEnumerable<int> seatsTaken = await repository.All<Ticket>(t => t.ProjectionId == projectionId)
                .Select(t => t.Seat)
                .ToArrayAsync();

            return seatsTaken;
        }

        //private bool IsSeatTaken(int seat, string projectionId)
        //{
        //    bool isTaken = repository.AllReadonly<Ticket>()
        //        .Any(t => t.Seat == seat && t.ProjectionId == projectionId);

        //    return isTaken;
        //}

        private async Task<bool> IsAnyTaken(int[] seats, string projectionId)
        {
            bool isAnyTaken = await repository.AllReadonly<Ticket>()
                .AnyAsync(t => seats.Contains(t.Seat) && t.ProjectionId == projectionId);

            return isAnyTaken;
        }

        private async Task NotifyUserOnCreation(string userId, string projectionId, int count)
        {
            Projection projection = await repository.GetByIdAsync<Projection>(projectionId);
            User user = await repository.GetByIdAsync<User>(userId);

            string movieId = projection.MovieId;
            Movie movie = await repository.GetByIdAsync<Movie>(movieId);
            string title = movie.Title;

            DateTime date = projection.Date;

            string day = date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
            string hour = date.ToString("HH:mm", CultureInfo.InvariantCulture);

            string notificationTitle = string.Empty;
            string notificationText = string.Empty;

            if (count > 1)
            {
                notificationTitle = NotificationTemplateConstants.TicketsBoughtTitle;
                notificationText = NotificationTemplateConstants.TicketsBoughtMessage;
            }
            else
            {
                notificationTitle = NotificationTemplateConstants.TicketBoughtTitle;
                notificationText = NotificationTemplateConstants.TicketBoughtMessage;
            }

            Notification notification = new Notification()
            {
                Title = notificationTitle,
                Text = string.Format(notificationText, title, day, hour),
                Date = DateTime.Now,
                IsChecked = false
            };

            user.Notifications.Add(notification);

            repository.Update(user);
            await repository.SaveChangesAsync();
        }
    }
}
