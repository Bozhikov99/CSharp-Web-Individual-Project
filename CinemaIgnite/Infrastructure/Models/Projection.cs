using Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class Projection
    {
        public Projection()
        {
            Id = Guid.NewGuid()
                .ToString();

            TicketsSold = new List<Ticket>();
        }

        [Key]
        public string Id { get; set; }

        public DateTime Date { get; set; }

        [Range(ProjectionConstants.TicketsAvailableMin, ProjectionConstants.TicketsAvailableMax)]
        public int TicketsAvailable { get; set; }

        public bool Subtitles { get; set; }

        [Required]
        public string Sound { get; set; }

        [Range((double)ProjectionConstants.TicketPriceMin, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(ProjectionConstants.FormatMaxLength)]
        public string Format { get; set; }

        [ForeignKey(nameof(Movie))]
        public string MovieId { get; set; }

        public virtual Movie Movie { get; set; }

        public virtual ICollection<Ticket> TicketsSold { get; set; }
    }
}