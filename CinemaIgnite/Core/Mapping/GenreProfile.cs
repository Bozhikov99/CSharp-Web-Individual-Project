using AutoMapper;
using Core.ViewModels.Genre;
using Infrastructure.Models;

namespace Core.Mapping
{
    public class GenreProfile : Profile
    {
        public GenreProfile()
        {
            CreateMap<Genre, ListGenreModel>()
                .ReverseMap();

            CreateMap<CreateGenreModel, Genre>();

            CreateMap<EditGenreModel, Genre>();
        }
    }
}
