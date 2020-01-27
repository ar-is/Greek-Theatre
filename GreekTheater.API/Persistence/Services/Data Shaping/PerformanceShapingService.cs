using AutoMapper;
using GreekTheater.API.Core.Dtos.Performance;
using GreekTheater.API.Core.Entities;
using GreekTheater.API.Core.Services.Data_Shaping;
using GreekTheater.API.Core.Services.Pagination;
using GreekTheater.API.Helpers.Extension_Methods;
using GreekTheater.API.Helpers.Paging;
using GreekTheater.API.Helpers.ResourceFilters;
using GreekTheater.API.Helpers.VendorMediaTypes;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Persistence.Services.Data_Shaping
{
    public class PerformanceShapingService : IDataShapingService<Performance>
    {
        private readonly IPaginationService<Performance> _paginationService;
        private readonly IMapper _mapper;

        public PerformanceShapingService(IPaginationService<Performance> paginationService,
            IMapper mapper)
        {
            _paginationService = paginationService
                ?? throw new ArgumentNullException(nameof(paginationService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public object GetShapedCollectionWithLinks(PagedList<Performance> performances,
            ResourceParameters resourceParameters)
        {
            var shapedPerformances = _mapper.Map<IEnumerable<PerformanceDto>>(performances)
                                      .ShapeData(resourceParameters.Fields);

            var shapedPerformancesWithLinks = shapedPerformances.Select(performance =>
            {
                var performanceAsDictionary = performance as IDictionary<string, object>;
                var performanceLinks = _paginationService.CreateLinksForEntity((Guid)performanceAsDictionary["Id"], null);
                performanceAsDictionary.Add("links", performanceLinks);
                return performanceAsDictionary;
            });

            return new
            {
                value = shapedPerformancesWithLinks,
                links = _paginationService.CreateLinksForEntities(resourceParameters, performances.HasNext, performances.HasPrevious)
            };
        }

        public IDictionary<string, object> GetShapedEntity(MediaTypeHeaderValue parsedMediaType,
            Performance performance, string fields)
        {
            if (_paginationService.GetPrimaryMediaType(parsedMediaType) == MediaTypes.PrimaryFullPerformance)
            {
                var fullResourceToReturn = _mapper.Map<PerformanceFullDto>(performance)
                    .ShapeData(fields) as IDictionary<string, object>;

                if (_paginationService.IncludeLinks(parsedMediaType))
                    fullResourceToReturn.Add("links", _paginationService.CreateLinksForEntity(performance.Guid, fields));

                return fullResourceToReturn;
            }

            var friendlyResourceToReturn = _mapper.Map<PerformanceDto>(performance)
                .ShapeData(fields) as IDictionary<string, object>;

            if (_paginationService.IncludeLinks(parsedMediaType))
                friendlyResourceToReturn.Add("links", _paginationService.CreateLinksForEntity(performance.Guid, fields));

            return friendlyResourceToReturn;
        }
    }
}
