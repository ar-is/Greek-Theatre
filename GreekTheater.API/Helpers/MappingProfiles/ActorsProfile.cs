using AutoMapper;
using GreekTheater.API.Core.Dtos.Actor;
using GreekTheater.API.Core.Entities;
using GreekTheater.API.Helpers.Extension_Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Helpers.MappingProfiles
{
    public class ActorsProfile : Profile
    {
        public ActorsProfile()
        {
            CreateMap<Actor, ActorDto>()
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
                    dest => dest.Actings,
                    opt => opt.MapFrom(src => src.Actings.Select(m => m.DisplayTitle))
                    );

            CreateMap<ActorForCreationDto, Actor>();

            CreateMap<ActorForUpdateDto, Actor>();

            CreateMap<Actor, ActorForUpdateDto>();

            CreateMap<ActorForCreationWithDateOfDeathDto, Actor>();
        }
    }
}
