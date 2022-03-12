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

        public async Task<(bool isCreated, string error)> Create(CreateTicketModel model)
        {
            bool isCreated = false;
            string error = string.Empty;

            Ticket ticket = mapper.Map<Ticket>(model);

            if (repository.All<Ticket>()
                .Single(x=>x.Seat==model.Seat) != null)
            {
                error = $"A ticket for seat {model.Seat} is already bought";
            }
            else
            {
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
            }


            return (isCreated, error);
        }
    }
}
