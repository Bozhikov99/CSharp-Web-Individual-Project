using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Movie
{
    public class EditMovieModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(MovieConstants.TitleMaxLength, MinimumLength = MovieConstants.TitleMinLength, ErrorMessage = "{0} must be between {2} and {1} characters long")]
        public string Title { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        [StringLength(MovieConstants.DescriptionMaxLength, MinimumLength = MovieConstants.DescriptionMinLength, ErrorMessage = "{0} must be between {2} and {1} characters long")]
        public string Description { get; set; }

        public TimeSpan Duration { get; set; }

        [Range(MovieConstants.ReleaseYearMin, MovieConstants.ReleaseYearMax)]
        public int ReleaseYear { get; set; }

        [Required]
        [MinLength(MovieConstants.ActorsMinlength)]
        public string Actors { get; set; }

        [Required]
        [MinLength(MovieConstants.DirectorMinlength)]
        public string Director { get; set; }

        [Required]
        public string Country { get; set; }
    }
}
