using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Projection
{
    public class ListProjectionModel
    {
        public string Id { get; set; }

        public bool Subtitles { get; set; }


        [Range((double)ProjectionConstants.TicketPriceMin, double.MaxValue)]
        public decimal Price { get; set; }

        public DateTime Date { get; set; }

        public string MovieId { get; set; }

        public int TicketsAvailable { get; set; }

        public string Format { get; set; }

        public string Sound { get; set; }
    }
}
