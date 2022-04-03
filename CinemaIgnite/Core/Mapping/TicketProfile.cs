using AutoMapper;
using Core.ViewModels.Ticket;
using Infrastructure.Models;

namespace Core.Mapping
{
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            CreateMap<Ticket, ListTicketModel>()
                .ForMember(d => d.Date, s => s.MapFrom(p => p.Projection.Date))
                .ForMember(d => d.MovieTitle, s => s.MapFrom(p => p.Projection.Movie.Title));
        }
    }
}
