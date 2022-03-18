using AutoMapper;
using Core.Services.Contracts;
using Core.ViewModels.Ticket;
using Infrastructure.Common;
using Infrastructure.Models;

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
            
            if (IsSeatTaken(model.Seat))
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

                    await repository.AddAsync(ticket);
                    await repository.SaveChangesAsync();
                    isCreated = true;
                }
                catch (Exception)
                {
                    error = "Error creating ticket";
                }
            }


            return (isCreated, error);
        }

        private bool IsSeatTaken(int seat)
        {
            bool isTaken = repository.AllReadonly<Ticket>()
                .Any(t => t.Seat == seat);

            return isTaken;
        }
    }
}
