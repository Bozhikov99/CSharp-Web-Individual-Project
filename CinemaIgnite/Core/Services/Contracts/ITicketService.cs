using Core.ViewModels.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Contracts
{
    public interface ITicketService
    {
        Task<(bool isCreated, string error)> Create(CreateTicketModel model);
    }
}
