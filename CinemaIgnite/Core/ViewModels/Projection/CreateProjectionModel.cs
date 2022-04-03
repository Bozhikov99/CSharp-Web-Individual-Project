using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Projection
{
    public class CreateProjectionModel
    {
        public DateTime Date { get; set; }

        public int TicketsAvailable { get; set; }

        public bool Subtitles { get; set; }

        [Required]
        public string Sound { get; set; }

        [Range((double)ProjectionConstants.TicketPriceMin, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(ProjectionConstants.FormatMaxLength)]
        public string Format { get; set; }

        public string MovieId { get; set; }
    }
}
