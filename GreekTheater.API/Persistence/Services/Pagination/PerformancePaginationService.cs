using GreekTheater.API.Core.Dtos.Links;
using GreekTheater.API.Core.Entities;
using GreekTheater.API.Core.Services.Pagination;
using GreekTheater.API.Helpers.Paging;
using GreekTheater.API.Helpers.ResourceFilters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Persistence.Services.Pagination
{
    public class PerformancePaginationService : IPaginationService<Performance>, IDependentPaginationService<Performance>
    {
        private readonly IUrlHelper _urlHelper;

        public PerformancePaginationService(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper ?? throw new ArgumentNullException(nameof(urlHelper));
        }

        public string CreateResourceUri(
           ResourceParameters resourceParameters,
           ResourceUriType type)
        {
            var performancesResourceParameters = (PerformancesResourceParameters)resourceParameters;

            string fields = performancesResourceParameters.Fields,
                   orderBy = performancesResourceParameters.OrderBy,
                   title = performancesResourceParameters.Filter == null ?
                "" : performancesResourceParameters.Filter.Title,
                   director = performancesResourceParameters.Filter == null ?
                "" : performancesResourceParameters.Filter.Director;
            int pageNumber = performancesResourceParameters.PageNumber,
                pageSize = performancesResourceParameters.PageSize;
            int? year = performancesResourceParameters.Filter == null ?
                null : performancesResourceParameters.Filter.PremiereYear;

            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetPerformances",
                      new
                      {
                          fields,
                          orderBy,
                          pageNumber = pageNumber - 1,
                          pageSize,
                          title,
                          year,
                          director
                      });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetPerformances",
                      new
                      {
                          fields,
                          orderBy,
                          pageNumber = pageNumber + 1,
                          pageSize,
                          title,
                          year,
                          director
                      });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link("GetPerformances",
                    new
                    {
                        fields,
                        orderBy,
                        pageNumber,
                        pageSize,
                        title,
                        year,
                        director
                    });
            }
        }

        public IEnumerable<LinkDto> CreateLinksForEntity(Guid performanceId, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                  new LinkDto(_urlHelper.Link("GetPerformance", new { performanceId }),
                  "get_performance",
                  "GET"));
            }
            else
            {
                links.Add(
                  new LinkDto(_urlHelper.Link("GetPerformance", new { performanceId, fields }),
                  "get_performance",
                  "GET"));
            }

            links.Add(
                new LinkDto(_urlHelper.Link("DeletePerformance", new { performanceId }),
                "delete_performance",
                "DELETE"));

            return links;
        }

        public IEnumerable<LinkDto> CreateLinksForDependentEntity(Guid directorId,
            Guid performanceId, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                  new LinkDto(_urlHelper.Link("GetPerformance", new { performanceId }),
                  "get_performance",
                  "GET"));

                links.Add(
                  new LinkDto(_urlHelper.Link("GetPerformanceForDirector", new { directorId, performanceId }),
                  "get_performance_for_director",
                  "GET"));
            }
            else
            {
                links.Add(
                  new LinkDto(_urlHelper.Link("GetPerformance", new { performanceId, fields }),
                  "get_performance",
                  "GET"));
            }

            links.Add(
                new LinkDto(_urlHelper.Link("UpdatePerformance", new { directorId, performanceId }),
                "update_performance",
                "PUT"));

            links.Add(
                new LinkDto(_urlHelper.Link("PartiallyUpdatePerformance", new { directorId, performanceId }),
                "partially_update_performance",
                "PATCH"));

            links.Add(
                new LinkDto(_urlHelper.Link("DeletePerformance", new { performanceId }),
                "delete_performance",
                "DELETE"));

            links.Add(
                new LinkDto(_urlHelper.Link("GetPerformancesForDirector", new { directorId }),
                "get_all_performances_for_director",
                "GET"));

            return links;
        }

        public IEnumerable<LinkDto> CreateLinksForEntities(ResourceParameters resourceParameters,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>
            {
                new LinkDto(CreateResourceUri(
                    resourceParameters, ResourceUriType.Current),
                    "current",
                    "GET")
            };

            if (hasNext)
            {
                links.Add(
                new LinkDto(CreateResourceUri(
                    resourceParameters, ResourceUriType.NextPage),
                    "nextPage",
                    "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                new LinkDto(CreateResourceUri(
                    resourceParameters, ResourceUriType.PreviousPage),
                    "previousPage",
                    "GET"));
            }

            return links;
        }
    }
}
