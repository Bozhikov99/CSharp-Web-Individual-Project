using AutoMapper;
using Core.ViewModels.Projection;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
