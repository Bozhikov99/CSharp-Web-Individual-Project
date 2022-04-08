using Common;
using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels.Genre
{
    public class CreateGenreModel
    {
        [Required]
        [StringLength(GenreConstants.NameMaxLength, MinimumLength = GenreConstants.NameMinLength, ErrorMessage = "Името трябва да е с дължина между {2} и {1}.")]
        public string Name { get; set; }
    }
}
