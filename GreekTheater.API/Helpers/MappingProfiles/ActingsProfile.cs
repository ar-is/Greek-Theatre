using AutoMapper;
using GreekTheater.API.Core.Dtos.Acting;
using GreekTheater.API.Core.Dtos.Performance;
using GreekTheater.API.Core.Entities;
using GreekTheater.API.Core.Repositories;
using GreekTheater.API.Core.Services;
using GreekTheater.API.Persistence.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Helpers.MappingProfiles
{
    public class ActingsProfile : Profile
    {
        private readonly IUnitOfWork _unitOfWork;

        public ActingsProfile(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public ActingsProfile()
        {
            CreateMap<ActingForCreationDto, Acting>()
                .ForMember(
                    dest => dest.ActorId,
                    opt => opt.MapFrom(src => _unitOfWork.Actors.GetActor(src.ActorId).Id));
                //.ForPath(
                //    dest => dest.PerformanceId,
                //    opt => opt.MapFrom(src => _unitOfWork.Performances.GetPerformance(src.PerformanceId).Id));
        }
    }
}
