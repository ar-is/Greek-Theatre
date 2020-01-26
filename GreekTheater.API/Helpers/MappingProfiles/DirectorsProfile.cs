using AutoMapper;
using GreekTheater.API.Core.Dtos.Director;
using GreekTheater.API.Core.Entities;
using GreekTheater.API.Helpers.Extension_Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Helpers.MappingProfiles
{
    public class DirectorsProfile : Profile
    {
        public DirectorsProfile()
        {
            CreateMap<Director, DirectorDto>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Guid)
                    )
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}")
                    )
                .ForMember(
                    dest => dest.Age,
                    opt => opt.MapFrom(src => src.DateOfBirth.GetCurrentAge(src.DateOfDeath))
                    )
                .ForMember(
                    dest => dest.Performances,
                    opt => opt.MapFrom(src => src.Performances.Select(m => m.DisplayTitle))
                    );

            CreateMap<DirectorForCreationDto, Director>();

            CreateMap<DirectorForUpdateDto, Director>();

            CreateMap<Director, DirectorForUpdateDto>();

            CreateMap<DirectorForCreationWithDateOfDeathDto, Director>();
        }
    }
}
