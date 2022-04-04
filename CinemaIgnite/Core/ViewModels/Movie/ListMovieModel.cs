namespace Core.ViewModels.Movie
{
    public class ListMovieModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public int ReleaseYear { get; set; }

        public double Rating { get; set; }

        public TimeSpan Duration { get; set; }

        public IEnumerable<string> Genres { get; set; }
    }
}
