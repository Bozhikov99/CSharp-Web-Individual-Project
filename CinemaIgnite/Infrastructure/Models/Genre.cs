using Common;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    public class Genre
    {
        public Genre()
        {
            Id = Guid.NewGuid()
                .ToString();

            Movies = new List<Movie>();
        }

        [Key]
        public string Id { get; set; }

        [MaxLength(GenreConstants.NameMaxLength)]
        public string Name { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
    }
}
