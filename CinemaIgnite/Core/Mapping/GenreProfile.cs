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
                .ForMember(d => d.MovieCount, s => s.MapFrom(s => s.Movies.Count))
                .ReverseMap();

            CreateMap<CreateGenreModel, Genre>();

            CreateMap<EditGenreModel, Genre>()
                .ReverseMap();

            //for testing
            CreateMap<ListGenreModel, EditGenreModel>();
        }
    }
}
