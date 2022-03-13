﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Movie
{
    public class ListMovieModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public IEnumerable<string> Genres { get; set; }
    }
}