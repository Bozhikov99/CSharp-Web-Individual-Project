using AutoMapper;
using Core.ViewModels.Ticket;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
