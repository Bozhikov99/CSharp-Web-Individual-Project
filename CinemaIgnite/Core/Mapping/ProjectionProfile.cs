using AutoMapper;
using Core.ViewModels.Projection;
using Infrastructure.Models;

namespace Core.Mapping
{
    public class ProjectionProfile : Profile
    {
        public ProjectionProfile()
        {
            CreateMap<Projection, ListProjectionModel>();

            CreateMap<CreateProjectionModel, Projection>();
        }
    }
}
