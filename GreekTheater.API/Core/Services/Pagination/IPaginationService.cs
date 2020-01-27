using GreekTheater.API.Core.Dtos.Links;
using GreekTheater.API.Helpers.Paging;
using GreekTheater.API.Helpers.ResourceFilters;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Core.Services.Pagination
{
    public interface IPaginationService<T> where T : class
    {
        IEnumerable<LinkDto> CreateLinksForEntity(Guid entityId, string fields);
        IEnumerable<LinkDto> CreateLinksForEntities(ResourceParameters resourceParameters,
            bool hasNext, bool hasPrevious);

        bool IncludeLinks(MediaTypeHeaderValue parsedMediaType)
        {
            return parsedMediaType.SubTypeWithoutSuffix
                .EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
        }

        StringSegment GetPrimaryMediaType(MediaTypeHeaderValue parsedMediaType)
        {
            return IncludeLinks(parsedMediaType) ?
                parsedMediaType.SubTypeWithoutSuffix
                .Substring(0, parsedMediaType.SubTypeWithoutSuffix.Length - 8)
                : parsedMediaType.SubTypeWithoutSuffix;
        }

        string CreateResourceUri(ResourceParameters resourceParameters, ResourceUriType type);
        object CreatePaginationMetadata(PagedList<T> collection, ResourceParameters parameters)
        {
            return new
            {
                totalCount = collection.TotalCount,
                pageSize = collection.PageSize,
                currentPage = collection.CurrentPage,
                totalPages = collection.TotalPages
            };
        }
    }
}
