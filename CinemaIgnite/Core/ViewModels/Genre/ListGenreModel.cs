using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels.Genre
{
    public class ListGenreModel
    {
        [UIHint("hidden")]
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
