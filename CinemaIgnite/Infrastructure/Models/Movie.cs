using Common;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    public class Movie
    {
        public Movie()
        {
            Id = Guid.NewGuid()
                .ToString();

            Ratings = new List<Rating>();

            Genres = new List<Genre>();

            Projections = new List<Projection>();

            UsersFavourited = new List<User>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(MovieConstants.TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        [MaxLength(MovieConstants.DescriptionMaxLength)]
        public string Description { get; set; }

        public TimeSpan Duration { get; set; }

        [Range(MovieConstants.ReleaseYearMin, MovieConstants.ReleaseYearMax)]
        public int ReleaseYear { get; set; }

        [Required]
        public string Actors { get; set; }

        [Required]
        public string Director { get; set; }

        [Required]
        public string Country { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }

        public virtual ICollection<Genre> Genres { get; set; }

        public virtual ICollection<Projection> Projections { get; set; }

        public virtual ICollection<User> UsersFavourited { get; set; }
    }
}
