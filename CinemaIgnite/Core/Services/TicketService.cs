using AutoMapper;
using Core.Services.Contracts;
using Core.ViewModels.Ticket;
using Infrastructure.Contracts;
using Infrastructure.Models;

namespace Core.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository repository;
        private readonly IMapper mapper;

        public TicketService(IMapper mapper, ITicketRepository repository)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<(bool isCreated, string error)> Create(CreateTicketModel model)
        {
            bool isCreated = false;
            string error = string.Empty;

            Ticket ticket = mapper.Map<Ticket>(model);

            if (repository.All<Ticket>(t => t.Seat == model.Seat) != null)
            {
                error = $"A ticket for seat {model.Seat} is already bought";
                return (isCreated, error);
            }

            try
            {
                await repository.AddAsync(model);
                await repository.SaveChangesAsync();
                isCreated = true;
            }
            catch (Exception)
            {
                error = "Error creating ticket";
            }

            return (isCreated, error);
        }
    }
}
