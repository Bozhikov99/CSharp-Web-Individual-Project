using AutoMapper;
using Common.TemplateConstants;
using Core.Services.Contracts;
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

        public TicketService(IMapper mapper, IRepository repository)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<(bool isCreated, string error)> Create(CreateTicketModel model, string userId)
        {
            bool isCreated = false;
            string error = string.Empty;

            Ticket ticket = mapper.Map<Ticket>(model);

            if (IsSeatTaken(model.Seat, model.ProjectionId))
            {
                error = $"A ticket for seat {model.Seat} is already bought";
            }
            else
            {
                try
                {
                    User user = await repository.GetByIdAsync<User>(userId);
                    user.Tickets.Add(ticket);

                    ticket.User = user;
                    Projection projection = await repository.GetByIdAsync<Projection>(model.ProjectionId);
                    await repository.AddAsync(ticket);
                    await repository.SaveChangesAsync();

                    projection.TicketsAvailable--;
                    await NotifyUserOnCreation(model);
                    isCreated = true;
                }
                catch (Exception)
                {
                    error = "Error creating ticket";
                }
            }


            return (isCreated, error);
        }

        private bool IsSeatTaken(int seat, string projectionId)
        {
            bool isTaken = repository.AllReadonly<Ticket>()
                .Any(t => t.Seat == seat && t.ProjectionId == projectionId);

            return isTaken;
        }

        private async Task NotifyUserOnCreation(CreateTicketModel model)
        {
            Projection projection = await repository.GetByIdAsync<Projection>(model.ProjectionId);
            User user = await repository.GetByIdAsync<User>(model.UserId);

            string movieId = projection.MovieId;
            Movie movie = await repository.GetByIdAsync<Movie>(movieId);
            string title = movie.Title;

            DateTime date = projection.Date;

            string day = date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
            string hour = date.ToString("HH:mm", CultureInfo.InvariantCulture);

            Notification notification = new Notification()
            {
                Title = NotificationTemplateConstants.TicketBoughtTitle,
                Text = string.Format(NotificationTemplateConstants.TicketBoughtMessage, title, day, hour),
                Date = DateTime.Now,
                IsChecked = false
            };

            user.Notifications.Add(notification);

            repository.Update(user);
            await repository.SaveChangesAsync();
        }
    }
}
