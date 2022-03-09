using AutoMapper;
using Core.ViewModels.Movie;
using Infrastructure.Models;

namespace Core.Mapping
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<CreateMovieModel, Movie>();

            CreateMap<Movie, ListMovieModel>()
                .ForMember(d => d.Genres, s => s.MapFrom(m => m.Genres.Select(g => g.Name)));

            CreateMap<Movie, EditMovieModel>()
                .ReverseMap();

            CreateMap<Movie, MovieDetailsModel>();
        }
    }
}
