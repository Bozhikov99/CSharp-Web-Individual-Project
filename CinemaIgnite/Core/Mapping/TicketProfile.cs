using AutoMapper;
using Core.ViewModels.Ticket;
using Infrastructure.Models;

namespace Core.Mapping
{
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            CreateMap<CreateTicketModel, Ticket>();
        }
    }
}
