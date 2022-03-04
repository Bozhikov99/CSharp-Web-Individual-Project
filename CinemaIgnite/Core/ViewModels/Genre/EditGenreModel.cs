using Common;
using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels.Genre
{
    public class EditGenreModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(GenreConstants.NameMaxLength, MinimumLength = GenreConstants.NameMinLength, ErrorMessage = "{0} must be between {2} and {1} characters long.")]
        public string Name { get; set; }
    }
}
