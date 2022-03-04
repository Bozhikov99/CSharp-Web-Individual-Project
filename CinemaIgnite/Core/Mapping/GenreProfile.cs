using AutoMapper;
using Core.ViewModels.Genre;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping
{
    public class GenreProfile : Profile
    {
        public GenreProfile()
        {
            CreateMap<Genre, ListGenreModel>()
                .ReverseMap();
        }
    }
}
