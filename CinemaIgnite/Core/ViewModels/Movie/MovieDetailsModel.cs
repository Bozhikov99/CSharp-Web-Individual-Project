using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Movie
{
    public class MovieDetailsModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public TimeSpan Duration { get; set; }

        public int ReleaseYear { get; set; }

        public string Actors { get; set; }

        public string Director { get; set; }

        public string Country { get; set; }

        public double Rating { get; set; }
    }
}