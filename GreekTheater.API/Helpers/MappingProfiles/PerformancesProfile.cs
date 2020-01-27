using AutoMapper;
using GreekTheater.API.Core.Dtos.Performance;
using GreekTheater.API.Core.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Helpers.MappingProfiles
{
    public class PerformancesProfile : Profile
    {
        public PerformancesProfile()
        {
            CreateMap<Performance, PerformanceDto>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Guid)
                    )
                .ForMember(
                    dest => dest.PremiereDate,
                    opt => opt.MapFrom(src => src.PremiereDate.HasValue ? src.PremiereDate.Value.ToString("dd/MM/yyyy", new CultureInfo("el-GR")) : "Not Set Yet")
                    )
                .ForMember(
                    dest => dest.Director,
                    opt => opt.MapFrom(src => $"{src.Director.FullName}")
                    )
                .ForMember(
                    dest => dest.Actors,
                    opt => opt.Ignore()
                    )
                .AfterMap((src, dest) =>
                {
                    dest.Actors = src.Actings.Select(a => a.RoleName + " : " + a.Actor.FullName).ToList();
                })
                .ForMember(
                    dest => dest.Genres,
                    opt => opt.MapFrom(src => src.PerformanceGenres.Select(pg => pg.Genre.Name))
                    );

            CreateMap<PerformanceForCreationDto, Performance>();

            CreateMap<PerformanceForUpdateDto, Performance>();

            CreateMap<Performance, PerformanceForUpdateDto>();

            CreateMap<Performance, PerformanceFullDto>()
                .ForMember(
                    dest => dest.DirectorId,
                    opt => opt.MapFrom(src => src.Director.Guid))
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Guid));
        }
    }
}
